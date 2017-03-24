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
using Application.Business.Erp.SupplyChain.Client.Basic;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.SpecialCost
{
    public partial class VSpecialCostQuery : TBasicDataView
    {
        private MSpecialCost model = new MSpecialCost();
        CurrentProjectInfo projectInfo;
        public VSpecialCostQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }        

        private void InitData()
        {
            //txtHandlePerson.Tag = ConstObject.LoginPersonInfo;
            //txtHandlePerson.Value = ConstObject.LoginPersonInfo.Name;
            VBasicDataOptr.InitZXCostType(txtCostType, true);
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
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            this.btnSearchGWBS.Click += new EventHandler(btnSearchGWBS_Click);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            dgMaster.SelectionChanged +=new EventHandler(dgMaster_SelectionChanged);
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

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = dgDetail.Rows[e.RowIndex].Tag as string;
                if (ClientUtil.ToString(billId) != "")
                {
                    VSpecialCost vOrder = new VSpecialCost();
                    vOrder.Start(billId);
                    vOrder.ShowDialog();
                }
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
            if (detail == null) return;
            
            int i = dgDetail.Rows.Add();
            string strType = detail.AccountingStyle;
            if (strType == "0")
            {
                dgDetail[colKJType.Name, i].Value = "月度";
            }
            else
            {
                dgDetail[colKJType.Name, i].Value = "季度";
            }
            dgDetail[colStartDate.Name, i].Value = detail.AccountingStartDate;
            dgDetail[colEndDate.Name, i].Value = detail.AccountingEndDate;
            dgDetail[colKJPlanMoney.Name, i].Value = detail.CurrentPlanIncome/10000;
            dgDetail[colDQRealMoney.Name, i].Value = detail.CurrentRealIncome/10000;
            dgDetail[colDQJD.Name, i].Value = detail.CurrentPlanProgress/10000;
            dgDetail[colDQRealOut.Name, i].Value = detail.CurrentRealPay/10000;
            dgDetail[colDQAccountJD.Name, i].Value = detail.CurrentRealProgress;
            dgDetail.Rows[i].Tag = detail;
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));
            if (!txtHandlePerson.Text.Trim().Equals("") && txtHandlePerson.Result != null)
            {
                oq.AddCriterion(Expression.Eq("HandlePerson", txtHandlePerson.Result[0] as PersonInfo));
                oq.AddCriterion(Expression.Eq("HandlePersonName", txtHandlePerson.Value));
            }
            if (txtGWBS.Text != "")
            {
                oq.AddCriterion(Expression.Like("EngTaskName", txtGWBS.Text, MatchMode.Anywhere));
            }
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            IList list = model.SpecialCostSrv.GetSpecialCost(oq);
            dgMaster.Rows.Clear();
            dgDetail.Rows.Clear();
            if (list.Count <= 0) return;
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
                dgMaster[colCreateDate.Name, rowIndex].Value = master.CreateDate;
                dgMaster.Rows[rowIndex].Tag = master;
                dgDetail.Rows.Clear();
            }
            SpecialCostMaster mat = list[0] as SpecialCostMaster;
            foreach (SpecialCostDetail dtl in mat.Details)
            {
                FillDgDetail(dtl);
            }
            //MessageBox.Show("查询完毕！");
        }
    }
}
