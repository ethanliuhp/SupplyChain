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
using Application.Business.Erp.SupplyChain.Client.CostManagement.OBS;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ProInsRecordMng
{
    public partial class VProInsRecordSelector : TBasicDataView
    {
        MProInsRecordMng model = new MProInsRecordMng();
        
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
        public VProInsRecordSelector(PersonInfo person,SubContractProject project)
        {
            InitializeComponent();
            PersonName = person;
            subProject = project;
            txtSuppiler.Tag = subProject;
            txtSuppiler.Text = subProject.BearerOrgName;
            txtCreatePerson.Result.Clear();
            //txtCreatePerson.Result.Add(person);
            //txtCreatePerson.Value = person.Name;
            InitData();
            InitEvent();
      
        }
        public void InitData()
        {
            dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            dtpDateEnd.Value = ConstObject.TheLogin.LoginDate.AddDays(1);
            VBasicDataOptr.InitWBSCheckRequir(cbWBSCheckRequir, true);
            cbWBSCheckRequir.Items.AddRange(new object[] { "检查通过", "罚款后检查通过", "检查不通过" });
        }
        public void InitEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnSelect.Click +=new EventHandler(btnSelect_Click);
            lnkAll.Click += new EventHandler(lnkAll_Click);
            lnkNone.Click += new EventHandler(lnkNone_Click);
        }

        //选择承担单位
        void btnSelect_Click(object sender, EventArgs e)
        {
            DataGridViewRow theCurrentRow = this.dgMaster.CurrentRow;
            VContractExcuteSelector vmros = new VContractExcuteSelector();
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject engineerMaster = list[0] as SubContractProject;

            txtSuppiler.Text = engineerMaster.BearerOrgName;
            txtSuppiler.Tag = engineerMaster;
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ObjectQuery oq = new ObjectQuery();
                IList list = new ArrayList();
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                oq.AddCriterion(Expression.Eq("Master.ProjectId", projectInfo.Id));
                oq.AddCriterion(Expression.Ge("Master.CreateDate", dtpDateBegin.Value.Date));
                oq.AddCriterion(Expression.Lt("Master.CreateDate", dtpDateEnd.Value.AddDays(1).Date));
                if (txtCodeBegin.Text != "")
                {
                    oq.AddCriterion(Expression.Between("Master.Code", txtCodeBegin.Text, txtCodeEnd.Text));
                }
                //制单人
                if (txtCreatePerson.Text != "" && txtCreatePerson.Result.Count > 0)
                {
                    oq.AddCriterion(Expression.Eq("Master.CreatePerson", txtCreatePerson.Result[0] as PersonInfo));
                }
                //受检责任承担者
                if (txtHandlePerson.Text != "" && txtHandlePerson.Result.Count > 0)
                {
                    oq.AddCriterion(Expression.Eq("InspectionPerson", txtHandlePerson.Result[0] as PersonInfo));
                    oq.AddCriterion(Expression.Eq("InspectionPersonName", txtHandlePerson.Text));
                }
                //检查专业
                if (cbWBSCheckRequir.Text != "")
                {
                    oq.AddCriterion(Expression.Eq("Master.InspectionSpecail", cbWBSCheckRequir.Text));
                }
                //承担单位
                if (this.txtSuppiler.Text != "")
                {
                    oq.AddCriterion(Expression.Eq("InspectionSupplierName", txtSuppiler.Text));
                }
                oq.AddCriterion(Expression.Eq("CorrectiveSign", 1));//查询整改标志为1的单据
                oq.AddCriterion(Expression.Eq("Master.DocState", DocumentState.InExecute));//查询提交状态的单据
                oq.AddFetchMode("Master", NHibernate.FetchMode.Eager);
                list = model.ProfessionInspectionSrv.GetProfessionInspection(oq);
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
                if (var.Cells[colselect.Name].Value != null)
                {
                    bool isSelected = (bool)var.Cells[colselect.Name].Value;
                    var.Cells[colselect.Name].Value = !isSelected;
                }
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
                var.Cells[colselect.Name].Value = true;
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
            foreach (ProfessionInspectionRecordDetail detail in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster[colCreateDate.Name, rowIndex].Value =detail.Master.CreateDate.ToShortDateString();
                dgMaster[colCreatePerson.Name, rowIndex].Value = detail.Master.CreatePersonName;
                dgMaster[colInsLevel.Name, rowIndex].Value = detail.DangerLevel;
                dgMaster[colDangerPart.Name, rowIndex].Value = detail.DangerPart;
                dgMaster[colDangerType.Name, rowIndex].Value = detail.DangerType;
                dgMaster[colDgMasterCode.Name, rowIndex].Value = detail.Master.Code;
                string strConclusion = ClientUtil.ToString(detail.InspectionConclusion);
                if (strConclusion.Equals("0"))
                {
                    dgMaster[colInsConclusion.Name, rowIndex].Value = "检查通过";
                }
                else
                {
                    dgMaster[colInsConclusion.Name, rowIndex].Value = "检查不通过";
                }
                string strInsQK = ClientUtil.ToString(detail.CorrectiveSign);
                if (strInsQK.Equals("0"))
                {
                    dgMaster[colInsQK.Name, rowIndex].Value = "不需整改";
                }
                if (strInsQK.Equals("1"))
                {
                    dgMaster[colInsQK.Name, rowIndex].Value = "需要整改，未进行处理";
                }
                if (strInsQK.Equals("2"))
                {
                    dgMaster[colInsQK.Name, rowIndex].Value = "需要整改，已进行处理";
                }
                dgMaster[colInsContent.Name, rowIndex].Value = detail.InspectionContent;
                dgMaster[colInsDate.Name, rowIndex].Value = detail.InspectionDate.ToShortDateString();
                dgMaster[colInsHandlePerson.Name, rowIndex].Value = detail.InspectionPersonName;
                dgMaster[colInsConclusion.Name, rowIndex].Value = detail.InspectionSituation;
                //通过明细查找主表信息
                ProfessionInspectionRecordMaster master = model.ProfessionInspectionSrv.GetProfessionInspectionRecordById(detail.Master.Id);
                dgMaster[colInsSpecail.Name, rowIndex].Value = master.InspectionSpecail;
                dgMaster[colInsSupplier.Name, rowIndex].Value = detail.InspectionSupplierName;             
                dgMaster.Rows[rowIndex].Tag = detail;
            }
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgMaster.Rows)
            {
                if (var.IsNewRow) return;
                if ((bool)var.Cells[0].EditedFormattedValue == true)
                {
                    ProfessionInspectionRecordDetail detail = var.Tag as ProfessionInspectionRecordDetail;
                    result.Add(detail);
                }
            }
            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
