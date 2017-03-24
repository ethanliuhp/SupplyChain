using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborSporadicManagement.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using NHibernate.Criterion;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionLotMng
{
    public partial class VInspectionLotSelect : TBasicDataView
    {
        private MInspectionLotMng model = new MInspectionLotMng();
        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }
        public VInspectionLotSelect()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }
        private void InitData()
        {
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            this.btnSearchProject.Click +=new EventHandler(btnSearchProject_Click);
        }
        //工程项目任务
        void btnSearchProject_Click(object sender,EventArgs e)
        {
            VWeekSelector vss = new VWeekSelector();
            vss.ShowDialog();
            IList list = vss.Result;
            if (list == null || list.Count == 0) return;
            foreach (WeekScheduleDetail detail in list)
            {
                txtProjectTask.Text = detail.GWBSTreeName;
                txtProjectTask.Tag = detail.GWBSTree;
            }
  
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }       

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
        }
        void btnOK_Click(object sender,EventArgs e)
        {
            if (dgDetail.Rows.Count > 0)
            {
                if (this.dgDetail.SelectedRows == null)
                {
                    MessageBox.Show("请选择一条检验批信息！");
                    return;
                }
                InspectionLot detail = dgDetail.CurrentRow.Tag as InspectionLot;
                result.Add(detail);
                this.btnOK.FindForm().Close();
            }
        }

        void btnCancel_Click( object sender,EventArgs e)
        {
            this.Close();
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                if (txtCodeBegin.Text != "")
                {
                    oq.AddCriterion(Expression.Between("Code", txtCodeBegin.Text, txtCodeEnd.Text));
                }

                if (txtCreatePerson.Text != "" && txtCreatePerson.Result.Count > 0)
                {
                    oq.AddCriterion(Expression.Equals("CreatePerson", txtCreatePerson.Result[0] as PersonInfo));
                }
                if (txtProjectTask.Text != "")
                {
                    oq.AddCriterion(Expression.Equals("ProjectTaskName", txtProjectTask.Text));
                }
                //oq.AddFetchMode("Master", NHibernate.FetchMode.Eager);
            
                list = model.InspectionLotSrv.GetInspectionLot(oq);
                ShowMasterList(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
        }

        /// <summary>
        /// 显示主表及明细信息
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList masterList)
        {
            dgDetail.Rows.Clear();
            if (masterList == null || masterList.Count == 0) return;
            foreach (InspectionLot detail in masterList)
            {
                int rowIndex = dgDetail.Rows.Add();
                dgDetail.Rows[rowIndex].Tag = detail;
                this.dgDetail[colCreateDate.Name, rowIndex].Value = detail.CreateDate.ToShortDateString();
                this.dgDetail[colAccountStatue.Name, rowIndex].Value = detail.AccountStatus;//验收结算状态
                this.dgDetail[colCreatePerson.Name, rowIndex].Value = detail.CreatePersonName;
                this.dgDetail[colCreatePerson.Name, rowIndex].Tag = detail.CreatePerson;
                this.dgDetail[colHandlePerson.Name, rowIndex].Tag = detail.HandlePerson;
                this.dgDetail[colHandlePerson.Name, rowIndex].Value = detail.HandlePersonName;
                this.dgDetail[colProjectName.Name, rowIndex].Value = detail.ProjectTaskName;
                this.dgDetail[colProjectName.Name, rowIndex].Tag = detail.ProjectTask;
                this.dgDetail[colUpdateDate.Name, rowIndex].Value = detail.InsUpdateDate.ToShortDateString();
            }
            if (dgDetail.Rows.Count == 0) return;
            dgDetail.CurrentCell = dgDetail[1, 0];
        }
    }
}
