using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
//using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatApplyMng.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionRecordMng
{
    public partial class VInspectionSelector : TBasicDataView
    {
        MProductionMng model = new MProductionMng();
        
        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }
        PersonInfo PersonName = new PersonInfo();
        SubContractProject subProject = new SubContractProject();
        public VInspectionSelector(PersonInfo person, SubContractProject project)
        {
            InitializeComponent();
            PersonName = person;
            subProject = project;
            txtHandlePerson.Result.Clear();
            txtHandlePerson.Result.Add(PersonName);
            txtHandlePerson.Value = PersonName.Name;
            InitData();
            InitEvent();     
        }
        public void InitData()
        {
            dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            dtpDateEnd.Value = ConstObject.TheLogin.LoginDate.AddDays(1);
            VBasicDataOptr.InitWBSCheckRequir(cbWBSCheckRequir, true);
        }
        public void InitEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnSelectGWBSDetail.Click += new EventHandler(btnSelectWBS_Click);
            lnkAll.Click +=new EventHandler(lnkAll_Click);
            lnkNone.Click +=new EventHandler(lnkNone_Click);
        }

        //选择工程任务
        void btnSelectWBS_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            List<TreeNode> list = frm.SelectResult;
            if (list.Count > 0)
            {
                txtGWBSCode.Tag = (list[0] as TreeNode).Name;
                txtGWBSCode.Text = (list[0] as TreeNode).Text;
            }
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ObjectQuery oq = new ObjectQuery();
                IList list = new ArrayList();
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                oq.AddCriterion(Expression.Ge("CreateDate", dtpDateBegin.Value.Date));
                oq.AddCriterion(Expression.Lt("CreateDate", dtpDateEnd.Value.AddDays(1).Date));
                oq.AddCriterion(Expression.Eq("CorrectiveSign",1));
                if (subProject.BearerOrg != null)
                {
                    oq.AddCriterion(Expression.Eq("BearTeamName", subProject.BearerOrgName));
                }
                //制单人
                if (txtCreatePerson.Text != "" && txtCreatePerson.Result.Count > 0)
                {
                    oq.AddCriterion(Expression.Eq("CreatePerson", txtCreatePerson.Result[0] as PersonInfo));
                }
                //责任人
                if (txtHandlePerson.Text != "" && txtHandlePerson.Result.Count > 0)
                {
                    oq.AddCriterion(Expression.Eq("HandlePerson", txtHandlePerson.Result[0] as PersonInfo));
                }
                //检查专业
                if (cbWBSCheckRequir.Text != "")
                {
                    oq.AddCriterion(Expression.Eq("InspectionSpecial", cbWBSCheckRequir.Text));
                }
                //工程项目任务
                if (this.txtGWBSCode.Text != "")
                {
                    oq.AddCriterion(Expression.Eq("GWBSTreeName", txtGWBSCode.Text));
                }
                list = model.ProductionManagementSrv.GetInspectionRecord(oq);
                ShowMasterList(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
            
        }

        /// <summary>
        /// 反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkNone_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgMaster.Rows)
            {
                bool isSelected = (bool)var.Cells[colSelect.Name].Value;
                var.Cells[colSelect.Name].Value = !isSelected;
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgMaster.Rows)
            {
                var.Cells[colSelect.Name].Value = true;
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList masterList)
        {
            dgMaster.Rows.Clear();
            if (masterList == null || masterList.Count == 0) return;
            foreach (InspectionRecord master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                
                dgMaster[colDgMasterCode.Name, rowIndex].Value = master.GWBSTreeName;
                dgMaster[colProjectTaskName.Name,rowIndex].Value = master.GWBSTreeName;
                dgMaster[colBearTeamName.Name,rowIndex].Value = master.BearTeamName;
                //dgMaster[colContent.Name,rowIndex].Value = master.InspectionContent;//内容
                dgMaster[colInspectionConclusion.Name, rowIndex].Value = master.InspectionConclusion;//结论
                dgMaster[colInspectionCase.Name, rowIndex].Value = master.InspectionStatus;//检查情况
                dgMaster[colInspectionDate.Name, rowIndex].Value = master.CreateDate.ToShortDateString();//检查时间
                dgMaster[colInspectionPerson.Name, rowIndex].Value = master.CreatePersonName;//检查负责人
                dgMaster[colInspectionSpecail.Name, rowIndex].Value = master.InspectionSpecial;//检查专业

                string strCorrectiveSign = ClientUtil.ToString(master.CorrectiveSign);
                if (strCorrectiveSign.Equals("0"))
                {
                    dgMaster[colCorrectionCase.Name, rowIndex].Value = "不需整改";
                }
                if (strCorrectiveSign.Equals("1"))
                {
                    dgMaster[colCorrectionCase.Name, rowIndex].Value = "需要整改，未进行处理";
                }
                if (strCorrectiveSign.Equals("2"))
                {
                    dgMaster[colCorrectionCase.Name, rowIndex].Value = "需要整改，已进行处理";
                }
                dgMaster.Rows[rowIndex].Tag = master;
            }
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgMaster.Rows)
            {
                if (var.IsNewRow) return;
                if ((bool)var.Cells[0].EditedFormattedValue == true)
                {
                    InspectionRecord detail = var.Tag as InspectionRecord;
                    result.Add(detail);
                }
            }
            //InspectionRecord detail = dgMaster.SelectedRows[0].Tag as InspectionRecord;
            //result.Add(detail);
            this.Close();
            
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
