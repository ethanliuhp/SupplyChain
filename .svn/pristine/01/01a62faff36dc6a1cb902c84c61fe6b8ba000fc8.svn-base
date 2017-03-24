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

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.SpecialAccount
{
    public partial class VSpecialAccountQuery : TBasicDataView
    {
        private MSpecialAccount model = new MSpecialAccount();
        CurrentProjectInfo projectInfo;
        public VSpecialAccountQuery()
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
            //dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            this.btnSearchGWBS.Click += new EventHandler(btnSearchGWBS_Click);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            dgMaster.SelectionChanged +=new EventHandler(dgMaster_SelectionChanged);
        }

        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            SpeCostSettlementMaster mast = dgMaster.CurrentRow.Tag as SpeCostSettlementMaster;
            dgDetail.Rows.Clear();
            if (mast != null)
            {
                FillDgDetail(mast);
            }
        }

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = dgDetail.Rows[e.RowIndex].Tag as string;
                if (ClientUtil.ToString(billId) != "")
                {
                    VSpecialAccount vOrder = new VSpecialAccount();
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

        void FillDgDetail(SpeCostSettlementMaster master)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", master.Id));
            oq.AddFetchMode("EngTaskId", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("SpeCostMngId", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("SpeCostMngId.Details", NHibernate.FetchMode.Eager);

            IList listDtl = model.SpecialCostSrv.ObjectQuery(typeof(SpeCostSettlementDetail), oq);
            dgDetail.Rows.Clear();
            foreach (SpeCostSettlementDetail dtl in listDtl)
            {
                int i = dgDetail.Rows.Add();
                this.dgDetail[colAccountMoney.Name, i].Value = dtl.SettlementMoney / 10000;//结算金额
                this.dgDetail[colGWBSTree.Name, i].Value = dtl.EngTaskName;//工程任务名称
                this.dgDetail[colGWBSTree.Name, i].Tag = dtl.EngTask;
                this.dgDetail[colOrderMoney.Name, i].Value = dtl.Money / 10000;
                this.dgDetail[colManageMoney.Name, i].Value = dtl.ManageMoney / 10000;
                this.dgDetail[colManageRace.Name, i].Value = dtl.ManageAcer * 100;
                this.dgDetail[colElectRace.Name, i].Value = dtl.ElectAcer * 100;
                this.dgDetail[colElectMoney.Name, i].Value = dtl.ElectMoney / 10000;
                this.dgDetail[colPayMentFees.Name, i].Value = dtl.PayMentFees / 10000;
                this.dgDetail[colOtherMoney.Name, i].Value = dtl.OtherAccruals / 10000;
                this.dgDetail.Rows[i].Tag = dtl;
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Ge("SettlementDate", this.dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("SettlementDate", this.dtpDateEnd.Value.AddDays(1).Date));
            if (!txtHandlePerson.Text.Trim().Equals("") && txtHandlePerson.Result != null)
            {
                oq.AddCriterion(Expression.Eq("HandlePerson", txtHandlePerson.Result[0] as PersonInfo));
            }
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));


            ObjectQuery oqDetail = new ObjectQuery();
            if (txtGWBS.Text != "")
            {
                oqDetail.AddCriterion(Expression.Like("EngTaskName", txtGWBS.Text, MatchMode.Anywhere));
            }


            IList list = model.SpecialCostSrv.GetSpecialDetailAccount(oqDetail);
            dgMaster.Rows.Clear();
            dgDetail.Rows.Clear();
            Disjunction dis = new Disjunction();
            if (list.Count > 0)
            {
                IEnumerable<SpeCostSettlementDetail> queryDtl = list.OfType<SpeCostSettlementDetail>();
                var queryMaster = from d in queryDtl
                                  group d by new { d.Master.Id }
                                      into g
                                      select new
                                      {
                                          g.Key.Id
                                      };
                foreach (var parent in queryMaster)
                {
                    dis.Add(Expression.Eq("Id", parent.Id));
                }
            }
            oq.AddCriterion(dis);
            try
            {
                IList listss = model.SpecialCostSrv.ObjectQuery(typeof(SpeCostSettlementMaster), oq);
                FillMaster(listss);

            }
            catch (Exception ex)
            {
                MessageBox.Show("查询专业检查出错。\n" + ExceptionUtil.ExceptionMessage(ex));
            }

            //MessageBox.Show("查询完毕！");
        }

        private void FillMaster(IList masterList)
        {
            dgMaster.Rows.Clear();
            if (masterList == null || masterList.Count == 0) return;
            foreach (SpeCostSettlementMaster master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster[colAccountDate.Name, rowIndex].Value = master.SettlementDate.ToShortDateString();
                dgMaster[ColProject.Name, rowIndex].Value = master.SubcontractUnitName;
                dgMaster[colHandlePerson.Name, rowIndex].Value = master.HandlePersonName;
                dgMaster[colHandlePerson.Name, rowIndex].Tag = master.HandlePerson;
                if (master.SubmitDate.Date>ClientUtil.ToDateTime("1990-01-01"))
                {
                    dgMaster[colSubmitDate.Name, rowIndex].Value = master.SubmitDate.ToShortDateString();
                }
                dgMaster.Rows[rowIndex].Tag = master;
                dgDetail.Rows.Clear();
            }

            dgMaster.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            if (dgMaster.Rows.Count > 0)
            {
                dgMaster.CurrentCell = dgMaster.Rows[0].Cells[0];
                dgMaster_CellClick(dgMaster, new DataGridViewCellEventArgs(0, 0));
            }
        }
        void dgMaster_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dr = dgMaster.CurrentRow;
            if (dr == null) return;
            SpeCostSettlementMaster master = dr.Tag as SpeCostSettlementMaster;
            if (master == null) return;
            FillDgDetail(master);
        }
    }
}
