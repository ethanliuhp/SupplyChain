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
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using VirtualMachine.Core;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.MobileManage.OftenWords;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.WeekSchedules
{
    public partial class VWeekPlanEntry : TBasicDataViewByMobile
    {
        private MWeekPlanMng model = new MWeekPlanMng();
        private EnumExecScheduleType execScheduleType;
        private AutomaticSize automaticSize = new AutomaticSize();
        WeekScheduleDetail weekdetail = new WeekScheduleDetail();
        string interphaseID = string.Empty;
        string controlID = string.Empty;
        string userID = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
        string oftenWord = string.Empty;
        private int d_count = 0;

        public VWeekPlanEntry(EnumExecScheduleType execScheduleType)
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            this.execScheduleType = execScheduleType;
            InitData();
            InitEvent();
            interphaseID = this.Name;
        }

        public void InitData()
        {
            dgDetail.ColumnHeadersHeight = 50;
        }

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            btnChanel.Click +=new EventHandler(btnChanel_Click);
            cbCheckState.Items.AddRange(new object[]{"检查通过","未通过"});
            txtTaskName.DoubleClick += new EventHandler(txtTaskName_DoubleClick);
        }

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgDetail[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;

            WeekScheduleDetail detail = dgDetail.CurrentRow.Tag as WeekScheduleDetail;

            VWeekPlanDetail vwpd = new VWeekPlanDetail(detail);
            vwpd.ShowDialog();
        }

        void btnSearch_Click(object sender, EventArgs e)
        {

            ObjectQuery oq = new ObjectQuery();
            this.dgDetail.Rows.Clear();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Ge("PlannedBeginDate", this.dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("PlannedEndDate", this.dtpDateEnd.Value.AddDays(1).Date));
            if (txtCreatePerson.Text != "")
            {
                oq.AddCriterion(Expression.Eq("Master.HandlePerson", txtCreatePerson.Result[0] as PersonInfo));
                oq.AddCriterion(Expression.Eq("Master.HandlePersonName", txtCreatePerson.Text));
            }
            if (txtTaskName.Text != "")
            {
                oq.AddCriterion(Expression.Like("GWBSTreeName", this.txtTaskName.Text, MatchMode.Anywhere));
            }
            if (cbCheckState.SelectedItem != null)
            {
                oq.AddCriterion(Expression.Like("TaskCheckState", "1", MatchMode.Anywhere));
            }
            //oq.AddCriterion(Expression.Eq("Master.ProjectName", projectInfo.Name));
            oq.AddCriterion(Expression.Eq("Master.ProjectId", projectInfo.Id));
            //IList list = model.ProductionManagementSrv.GetWeekDetail(oq);
            IList list = model.ProductionManagementSrv.GetWeekScheduleDetail(oq);
            d_count = list.Count;
            if (list != null && list.Count > 0)
            {
                foreach (WeekScheduleDetail master in list)
                {
                    int rowIndex = this.dgDetail.Rows.Add();
                    dgDetail[colGWBSName.Name, rowIndex].Value = master.GWBSTreeName;
                    dgDetail[colPlannedBeginDate.Name, rowIndex].Value = master.Master.PlannedBeginDate.ToShortDateString();
                    dgDetail[colPlannedEndDate.Name, rowIndex].Value = master.Master.PlannedEndDate.ToShortDateString();
                    dgDetail.Rows[rowIndex].Tag = master;
                }
            }
        }

        internal void Start(string name)
        {
            throw new NotImplementedException();
        }

        void btnChanel_Click(object sender, EventArgs e)
        {
            ClearView();
            dgDetail.Rows.Clear();
        }

        public void ClearView()
        {
            this.dtpDateBegin.Text = "";
            this.dtpDateEnd.Text = "";
            this.txtCreatePerson.Text="";
            this.txtTaskName.Text = "";
            this.cbCheckState.Text = "";
        }

        void txtTaskName_DoubleClick(object sender, EventArgs e)
        {
            controlID = "txtTaskName";
            oftenWord = txtTaskName.Text;
            VOftenWords vow = new VOftenWords(userID, interphaseID, controlID, oftenWord);
            vow.ShowDialog();
            string result = vow.Result;
            if (result != null)
            {
                txtTaskName.Text = result;
            }
        }

        private void VWeekPlanEntry_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
       
    }
}
