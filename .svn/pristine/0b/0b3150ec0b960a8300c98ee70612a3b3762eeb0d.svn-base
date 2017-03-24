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
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatApplyMng.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.SpecialCost
{
    public partial class VSpecialCostSelecter : TBasicDataView
    {
        private MSpecialCost model = new MSpecialCost();
        CurrentProjectInfo projectInfo;
        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VSpecialCostSelecter()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }        

        private void InitData()
        {
            txtHandlePerson.Tag = ConstObject.LoginPersonInfo;
            txtHandlePerson.Value = ConstObject.LoginPersonInfo.Name;
            this.dtpDateBegin.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            projectInfo = StaticMethod.GetProjectInfo();
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }       

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnCancel.Click +=new EventHandler(btnCancel_Click);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            this.btnSearchGWBS.Click += new EventHandler(btnSearchGWBS_Click);
            dgMaster.SelectionChanged +=new EventHandler(dgMaster_SelectionChanged);
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            SpecialCostMaster mast = dgMaster.CurrentRow.Tag as SpecialCostMaster;
            if (mast == null) return;
            dgDetail.Rows.Clear();
            foreach (SpecialCostDetail dtl in mast.Details)
            {
                FillDgDetail(dtl);
            }
        }

        void btnSearchGWBS_Click(object sender,EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            List<TreeNode> list = frm.SelectResult;
            if (list.Count > 0)
            {
                this.txtGWBS.Tag = (list[0] as TreeNode).Tag as GWBSTree;
                this.txtGWBS.Text = (list[0] as TreeNode).Text;
            }
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgDetail[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colGWBSName.Name)
            {
                SpecialCostMaster master = model.SpecialCostSrv.GetSpecialCostByCode(ClientUtil.ToString(dgvCell.Value));
                VSpecialCost vWasteApply = new VSpecialCost();
                vWasteApply.CurBillMaster = master;
                vWasteApply.Preview();
            }
        }

        void FillDgDetail(SpecialCostDetail detail)
        {
            int i = dgDetail.Rows.Add();
            dgDetail[colKJType.Name, i].Value = detail.AccountingStyle;
            dgDetail[colStartDate.Name, i].Value = detail.AccountingStartDate;
            dgDetail[colEndDate.Name, i].Value = detail.AccountingEndDate;
            dgDetail[colKJPlanMoney.Name, i].Value = detail.CurrentPlanIncome;
            dgDetail[colDQRealMoney.Name, i].Value = detail.CurrentRealIncome;
            dgDetail[colDQJD.Name, i].Value = detail.CurrentPlanProgress;
            dgDetail[colDQRealOut.Name, i].Value = detail.CurrentRealPay;
            dgDetail[colDQAccountJD.Name, i].Value = detail.CurrentRealProgress;
            dgDetail.Rows[i].Tag = detail;
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            if (dgMaster.CurrentRow != null)
            {
                SpecialCostMaster costmanster = dgMaster.CurrentRow.Tag as SpecialCostMaster;
                result.Add(costmanster);
            }
            else
            {
                MessageBox.Show("请选择专项管理费用信息");
            }
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));
            //oq.AddCriterion(Expression.Eq("HandelPerson", ConstObject.LoginPersonInfo));
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            IList list = model.SpecialCostSrv.GetSpecialCost(oq);
            if (list.Count > 0)
            {
                dgMaster.Rows.Clear();
                foreach (SpecialCostMaster master in list)
                {
                    int rowIndex = dgMaster.Rows.Add();
                    dgMaster[colGWBSName.Name, rowIndex].Value = master.EngTaskName;
                    dgMaster[colGWBSName.Name, rowIndex].Tag = master.EngTaskId;
                    dgMaster[colHandlePerson.Name, rowIndex].Value = master.HandlePersonName;
                    dgMaster[colHandlePerson.Name, rowIndex].Tag = master.HandlePerson;
                    dgMaster[ColCostType.Name, rowIndex].Value = master.CostType;
                    dgMaster[colOrderMoney.Name, rowIndex].Value = master.ContractTotalIncome/10000;
                    dgMaster[colManageRace.Name, rowIndex].Value = master.ContractProfit;
                    dgMaster[colRealMoney.Name, rowIndex].Value = master.RealIncome / 10000;
                    dgMaster[colPlanMoney.Name, rowIndex].Value = master.ContractTotalIPay/10000;
                    dgMaster[colRealPlanMoney.Name, rowIndex].Value = master.RealPay/10000;
                    dgMaster[colAccountJD.Name, rowIndex].Value = master.AccountingProgress;
                    dgMaster[colSubmitDate.Name, rowIndex].Value = master.SubmitDate;
                    dgMaster.Rows[rowIndex].Tag = master;
                    dgDetail.Rows.Clear();
                    foreach (SpecialCostDetail detail in master.Details)
                    {
                        FillDgDetail(detail);
                    }
                }
            }
        }
    }
}
