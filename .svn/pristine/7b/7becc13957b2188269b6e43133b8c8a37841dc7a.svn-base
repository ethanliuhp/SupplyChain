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
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI
{
    public partial class VWeekAssignQuery :TBasicDataView
    {
        private CurrentProjectInfo projectInfo = null;
        private SubContractProject subProject;
        private MProductionMng mProductionMng = new MProductionMng();
        private OperationOrgInfo info;

        public VWeekAssignQuery()
        {
            InitializeComponent();
           
            InitEvent();
            InitData();
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        private void InitEvent()
        {
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            dgMaster.CellContentClick += new DataGridViewCellEventHandler(dgMaster_CellContentClick);
        }

        private void InitData()
        {
            this.Title = "任务单查询";
            this.dtpBeginCreateDate.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpEndCreateDate.Value = ConstObject.TheLogin.LoginDate;

            projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
            {
                txtOperationOrg.Text = projectInfo.Name;
                txtOperationOrg.Tag = projectInfo;
                btnOperationOrg.Visible = false;
            }
        }

        private void btnSelectBalOrg_Click(object sender, EventArgs e)
        {
            VContractExcuteSelector vSubProject = new VContractExcuteSelector();
            vSubProject.ShowDialog();
            IList list = vSubProject.Result;
            if (list == null || list.Count == 0) return;

            subProject = list[0] as SubContractProject;
            this.txtAssignTeam.Text = subProject.BearerOrgName;
            this.txtAssignTeam.Tag = subProject;
        }

        private void btnOperationOrg_Click(object sender, EventArgs e)
        {
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect(true);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                info = frm.Result[0] as OperationOrgInfo;
                txtOperationOrg.Tag = info;
                txtOperationOrg.Text = info.Name;
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("正在查询信息......");
            ObjectQuery objectQuery = new ObjectQuery();

            if (subProject != null)
            {
                objectQuery.AddCriterion(Expression.Eq("AssignTeam", subProject.Id));
            }

            if (txtCode.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("Code", txtCode.Text, MatchMode.Anywhere));
            }
            
            if (info != null)
                {
                    objectQuery.AddCriterion(Expression.Like("OrgSysCode", info.SysCode, MatchMode.Start));
                }

            //业务时间
            objectQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpBeginCreateDate.Value.Date));
            objectQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpEndCreateDate.Value.AddDays(1).Date));

            try
            {
                dgMaster.Rows.Clear();
                objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                var list = mProductionMng.ProductionManagementSrv.GetAssignWorkerOrderMasterByOQ(objectQuery);
                foreach (AssignWorkerOrderMaster master in list)
                {
                    int rowIndex = dgMaster.Rows.Add();
                    dgMaster.Rows[rowIndex].Tag = master;
                    dgMaster[colCode.Name, rowIndex].Value = master.Code;
                    dgMaster[colAssignTeam.Name, rowIndex].Value = master.AssignTeamName;
                    dgMaster[colCreateDate.Name, rowIndex].Value = master.CreateDate;
                    dgMaster[colAssignWorkerOrderDescription.Name, rowIndex].Value = master.AssignWorkerOrderDescription;
                    dgMaster[colLastPrintTime.Name, rowIndex].Value = master.LastPrintTime;
                    dgMaster[colLastPrintPerson.Name, rowIndex].Value = master.LastPrintPerson;
                    dgMaster[colCreatePersonName.Name, rowIndex].Value = master.CreatePersonName;
                }
                dgMaster_SelectionChanged(dgMaster, new EventArgs());
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        /// <summary>
        /// 主表变化，明细同步变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            dgDetail.Rows.Clear();
            if (dgMaster.CurrentRow == null) return;
            AssignWorkerOrderMaster master = dgMaster.CurrentRow.Tag as AssignWorkerOrderMaster;
            if (master == null) return;
            foreach (AssignWorkerOrderDetail dtl in master.Details)
            {
                int rowIndex = dgDetail.Rows.Add();
                dgDetail[colGWBSTreeName.Name, rowIndex].Value = dtl.GWBSTreeName;
                dgDetail[colGWBSDetail.Name, rowIndex].Value = dtl.GWBSDetailName;
                dgDetail[colPlanBeginDate.Name, rowIndex].Value = dtl.PlanBeginDate;
                dgDetail[colPlanEndDate.Name, rowIndex].Value = dtl.PlanEndDate;
                dgDetail[colPlanWorkDays.Name, rowIndex].Value = dtl.PlanWorkDays;
                dgDetail[colActualBenginDate.Name, rowIndex].Value = dtl.ActualBenginDate;
                dgDetail[colAssWorkDesc.Name, rowIndex].Value = dtl.AssWorkDesc;
            }
        }

        private void dgMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgMaster[e.ColumnIndex, e.RowIndex];
            if (dgvCell.OwningColumn.Name == colCode.Name)
            {

                AssignWorkerOrderMaster master = dgMaster.Rows[e.RowIndex].Tag as AssignWorkerOrderMaster;
                VWeekAssign vmro = new VWeekAssign();
                vmro.CurBillMaster = master;
                vmro.Preview();
            }
        }

       

    }
}
