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
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.GWBSConfirmUI
{
    public partial class VGWBSConfirmQuery : TBasicDataView
    {
        private MProductionMng model = new MProductionMng();
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();

        public VGWBSConfirmQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        private void InitData()
        {
            //this.dtpDateBegin.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
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
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            //DataGridViewCell dgvCell = dgDetail[e.ColumnIndex, e.RowIndex];
            //if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            //if (dgvCell.OwningColumn.Name == colCode.Name)
            //{
            //    WeekScheduleMaster master = model.ProductionManagementSrv.GetWeekScheduleMasterByCode(dgvCell.Value.ToString());
            //    VWeekSchedule vmro = new VWeekSchedule();
            //    vmro.CurBillMaster = master;
            //    vmro.Preview();
            //}
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.ProjectId", projectInfo.Id));
            if (txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Between("Master.Code", txtCodeBegin.Text, txtCodeEnd.Text));
            }
            oq.AddCriterion(Expression.Ge("Master.CreateDate", dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("Master.CreateDate", dtpDateEnd.Value.AddDays(1).Date));
            if (txtCreatePerson.Text != "" && txtCreatePerson.Result.Count > 0)
            {
                oq.AddCriterion(Expression.Eq("Master.ConfirmHandlePerson", (txtCreatePerson.Result[0] as PersonInfo).Id));
            }
            if (txtGWBSTreeName.Text != "")
            {
                oq.AddCriterion(Expression.Like("GWBSTreeName", txtGWBSTreeName.Text,MatchMode.Anywhere));
            }
            oq.AddFetchMode("Master", NHibernate.FetchMode.Eager);
            oq.AddOrder(Order.Desc("Master.Code"));
            try
            {
                IList list = model.ProductionManagementSrv.GetGWBSTaskConfirm(oq);
                ShowData(list);
            } catch (Exception ex)
            {
                MessageBox.Show("查询出错。\n"+ex.Message);
            }
        }

        private void ShowData(IList list)
        {
            dgDetail.Rows.Clear();
            if (list == null || list.Count == 0) return;
            foreach (GWBSTaskConfirm obj in list)
            {
                int rowIndex = dgDetail.Rows.Add();
                dgDetail[colCode.Name, rowIndex].Value = obj.Master.Code;
                dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(obj.Master.DocState);
                dgDetail[colGWBSName.Name, rowIndex].Value = obj.GWBSTreeName;
                dgDetail[colGWBSDetail.Name, rowIndex].Value = obj.GWBSDetailName;
                dgDetail[colActualCompletedQuantity.Name, rowIndex].Value = obj.ActualCompletedQuantity;
                dgDetail[colQuantityBeforeConfirm.Name, rowIndex].Value = obj.QuantityBeforeConfirm;
                //dgDetail[colProjectType.Name, rowIndex].Value = obj.ProjectTaskType;
                dgDetail[colTaskCompletedPercent.Name, rowIndex].Value = obj.ProgressBeforeConfirm;
                dgDetail[colSumCompletedPercent.Name, rowIndex].Value = obj.ProgressAfterConfirm;
                dgDetail[colTaskHandler.Name, rowIndex].Value = obj.Master.TaskHandleName; ;
                dgDetail[colUnit.Name, rowIndex].Value = obj.WorkAmountUnitName;
                //dgDetail[colRealOperationDate.Name, rowIndex].Value = obj.RealOperationDate;
                dgDetail[colMaterialFeeSettlement.Name, rowIndex].Value = obj.MaterialFeeSettlementFlag.ToString();
                dgDetail[colDescript.Name, rowIndex].Value = obj.Descript;
            }
            lblRecordTotal.Text = "共【" + list.Count + "】条记录";

            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            MessageBox.Show("查询完毕！");
        }
    }
}
