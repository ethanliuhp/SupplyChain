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
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI
{
    public partial class VWeekScheduleQuery : TBasicDataView
    {
        private MProductionMng model = new MProductionMng();
        private EnumExecScheduleType execScheduleType;

        public VWeekScheduleQuery(EnumExecScheduleType execScheduleType)
        {
            InitializeComponent();
            this.execScheduleType = execScheduleType;
            InitEvent();
            InitData();
        }

        private void InitData()
        {
            //this.dtpDateBegin.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
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
            #region 查询条件处理
            string condition = "";
            if (this.txtCodeBegin.Text != "")
            {
                condition = condition + " and t1.Code between '" + this.txtCodeBegin.Text + "' and '" + this.txtCodeEnd.Text + "'";
            }

            if (StaticMethod.IsUseSQLServer())
            {
                condition += " and t1.CreateDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and t1.CreateDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
            }
            else
            {
                condition += " and t1.CreateDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
            }

            if (!txtHandlePerson.Text.Trim().Equals("") && txtHandlePerson.Result != null)
            {
                condition = condition + " and t1.handleperson='" + (txtHandlePerson.Result[0] as PersonInfo).Id + "'";
            }

            if (!txtCreatePerson.Text.Trim().Equals("") && txtCreatePerson.Result != null && txtCreatePerson.Result.Count > 0)
            {
                condition += " and t1.CreatePerson='" + (txtCreatePerson.Result[0] as PersonInfo).Id + "'";
            }

            if (execScheduleType == EnumExecScheduleType.周进度计划)
            {
                condition += " and t1.execScheduleType=" + (int)EnumExecScheduleType.周进度计划;
            }
            else if (execScheduleType == EnumExecScheduleType.月度进度计划)
            {
                condition += " and t1.execScheduleType=" + (int)EnumExecScheduleType.月度进度计划;
            }

            if (txtDescript.Text != "")
            {
                condition += " and t1.Descript like '%"+txtDescript.Text+"%'";
            }

            //加上项目查询条件
            condition += " and t1.projectId='" + StaticMethod.GetProjectInfo().Id + "'";

            #endregion
            DataSet dataSet = model.ProductionManagementSrv.WeekScheduleQuery(condition);
            this.dgDetail.Rows.Clear();

            DataTable dataTable = dataSet.Tables[0];
            //if (dataTable == null || dataTable.Rows.Count == 0) return;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgDetail.Rows.Add();
                dgDetail[colCode.Name, rowIndex].Value = dataRow["code"];
                object objState = dataRow["State"];
                if (objState != null)
                {
                    dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                }
                dgDetail[colGWBSName.Name, rowIndex].Value = dataRow["GWBSTreeName"];
                dgDetail[colPBSName.Name, rowIndex].Value = dataRow["PBSTreeName"];
                dgDetail[colPlannedBeginDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["PlannedBeginDate"]).ToShortDateString();
                dgDetail[colPlannedEndDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["PlannedEndDate"]).ToShortDateString();
                dgDetail[colPlannedDuration.Name, rowIndex].Value = dataRow["PlannedDuration"];

                dgDetail[colPlannedWrokload.Name, rowIndex].Value = dataRow["PlannedWrokload"];
                dgDetail[colActualBeginDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["ActualBeginDate"]).ToShortDateString();

                dgDetail[colActualEndDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["ActualEndDate"]).ToShortDateString();
                dgDetail[colActualDuration.Name, rowIndex].Value = dataRow["ActualDuration"];
                dgDetail[colTaskCompletedPercent.Name, rowIndex].Value = dataRow["TaskCompletedPercent"];
                dgDetail[colMainTaskContent.Name, rowIndex].Value = dataRow["MainTaskContent"];
                dgDetail[colCompletionAnalysis.Name, rowIndex].Value = dataRow["CompletionAnalysis"];
                dgDetail[colPlanConformity.Name, rowIndex].Value = dataRow["PlanConformity"];
                dgDetail[colTaskCheckState.Name, rowIndex].Value = dataRow["TaskCheckState"];
            }

            lblRecordTotal.Text = "共【" + dataTable.Rows.Count + "】条记录";

            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            MessageBox.Show("查询完毕！");
        }
    }
}
