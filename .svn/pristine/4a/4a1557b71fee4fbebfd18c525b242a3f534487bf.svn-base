using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using NHibernate.Criterion;
using VirtualMachine.Core;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI
{
    public partial class VScheduleApprove : TBasicDataView
    {
        private MProductionMng model = new MProductionMng();
        private WeekScheduleMaster selectSchedule;

        public VScheduleApprove()
        {
            InitializeComponent();

            InitEvents();

            LoadApprovingSchedule();
        }

        private void InitEvents()
        {
            btnAgree.Click += new EventHandler(btnAgree_Click);
            btnReject.Click += new EventHandler(btnReject_Click);
        }

        private void ClearScheduleInfo()
        {
            txtProjectName.Text = string.Empty;
            txtPlanName.Text = string.Empty;
            txtSubmitBy.Text = string.Empty;
            txtSubmitTime.Text = string.Empty;

            //ucScheduleDetailViewer1.ClearScheduleDetail();
        }

        private void LoadApprovingSchedule()
        {
            var projectInfo = StaticMethod.GetProjectInfo();
            if(projectInfo==null|| projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
            {
                return;
            }

            ObjectQuery objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            objQuery.AddCriterion(Expression.Eq("ExecScheduleType", EnumExecScheduleType.总体进度计划));
            objQuery.AddCriterion(Expression.Eq("DocState", DocumentState.InAudit));
            objQuery.AddOrder(Order.Desc("SubmitDate"));

            var list = model.ProductionManagementSrv.GetWeekScheduleMaster(objQuery);
            if (list == null || list.Count == 0)
            {
                ClearScheduleInfo();
                return;
            }

            selectSchedule = list[0] as WeekScheduleMaster;

            txtProjectName.Text = selectSchedule.ProjectName;
            txtPlanName.Text = selectSchedule.PlanName;
            //txtSubmitBy.Text = selectSchedule.CreatePersonName;
            //txtSubmitTime.Text = selectSchedule.SubmitDate.ToString();

            ucScheduleDetailViewer1.LoadScheduleDetails(selectSchedule);
            //ucScheduleDetailViewer1.SetFactPlanVisble(false);
        }

        private void ApproveSchedule(bool isAgree)
        {
            if (selectSchedule == null)
            {
                MessageBox.Show("没有总进度计划需要审核");
                return;
            }

            bool isSaved = ucScheduleDetailViewer1.OnSave(isAgree);

            if (isSaved)
            {
                //model.ProductionManagementSrv.SaveOrUpdateByDao(selectSchedule);

                MessageBox.Show("审批成功");

                LoadApprovingSchedule();
            }
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            if (selectSchedule == null)
            {
                MessageBox.Show("请选择待审批的记录");
                return;
            }

            if(string.IsNullOrEmpty( txtApproveRemark.Text.Trim()))
            {
                MessageBox.Show("审批[不通过]必须填写审批意见！");
                return;
            }

            if (MessageBox.Show("确认要审批不通过吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            ApproveSchedule(false);
        }

        private void btnAgree_Click(object sender, EventArgs e)
        {
            if (selectSchedule == null)
            {
                MessageBox.Show("请选择待审批的记录");
                return;
            }

            ApproveSchedule(true);
        }
    }
}
