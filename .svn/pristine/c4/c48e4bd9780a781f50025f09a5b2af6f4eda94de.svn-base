using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Approval.AppProcessMng.Domain;
using Application.Business.Erp.SupplyChain.ApprovalMng.AppSolutionMng.Service;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatformUI;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using NHibernate;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Core;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan
{
    public partial class VPlanDeclareApprove : TBasicDataView
    {
        private BaseMaster selectPlan;
        private ApproveBill selectAppBill;
        private MFinanceMultData mOperate;
        private MAppPlatform appPlatform = new MAppPlatform();

        public VPlanDeclareApprove()
        {
            InitializeComponent();

            InitEvents();

            InitData();
        }

        private void InitEvents()
        {
            btnOk.Click += new EventHandler(btnOk_Click);
            btnReject.Click += new EventHandler(btnReject_Click);
            txtApproveMoney.tbTextChanged += new EventHandler(txtApproveMoney_TextChanged);

            dgBillList.AutoGenerateColumns = false;
            dgBillList.CellDoubleClick += new DataGridViewCellEventHandler(dgBillList_CellDoubleClick);
        }

        private void InitData()
        {
            InitGrid();

            LoadApproveTask();
        }

        private void InitGrid()
        {
            FundPlanOperate.LoadFlexFile(string.Concat(tpCompanyReport.Tag, ".flx"), gdCompanyReport);

            FundPlanOperate.LoadFlexFile(string.Concat(tpProjectReport.Tag, ".flx"), gdProjectReport);
        }

        private void ClearInfo()
        {
            InitGrid();

            dgvAppSteps.Rows.Clear();

            txtApproveMoney.Text = string.Empty;
            txtReark.Text = string.Empty;
        }

        private void LoadApproveTask()
        {
            var proj = StaticMethod.GetProjectInfo();

            var tbBills = appPlatform.Service.GetApprovingBills(ConstObject.TheLogin.TheSysRole.Id,
                                                                ConstObject.TheLogin.ThePerson.Id,
                                                                proj == null ||
                                                                proj.Code == CommonUtil.CompanyProjectCode
                                                                    ? string.Empty
                                                                    : proj.Id);

            if (tbBills != null)
            {
                dgBillList.DataSource = tbBills.OfType<ApproveBill>().ToList()
                    .FindAll(a => a.BillCode.StartsWith("资金计划"));
            }
            else
            {
                dgBillList.DataSource = null;
            }
            selectAppBill = null;
            selectPlan = null;
        }

        private void LoadApproveStep()
        {
            if (selectAppBill == null)
            {
                return;
            }

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BillId", selectAppBill.BillId));
            oq.AddCriterion(Expression.Eq("AppStatus", ClientUtil.ToLong(2)));
            oq.AddCriterion(Expression.Eq("State", 1));
            oq.AddFetchMode("AuditPerson", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("AppRole", NHibernate.FetchMode.Eager);

            var steps = appPlatform.Service.GetAppStepsInfo(oq);
            dgvAppSteps.Rows.Clear();
            foreach (AppStepsInfo master in steps)
            {
                int index = dgvAppSteps.Rows.Add();

                dgvAppSteps[StepOrder.Name, index].Value = ClientUtil.ToLong(master.StepOrder);
                dgvAppSteps[StepName.Name, index].Value = ClientUtil.ToString(master.StepsName);
                if (master.AppRelations == 0)
                {
                    dgvAppSteps[AppRelations.Name, index].Value = "或";
                }
                else
                {
                    dgvAppSteps[AppRelations.Name, index].Value = "与";
                }
                dgvAppSteps[AppRole.Name, index].Value = master.AppRole.RoleName;
                dgvAppSteps[AppRole.Name, index].Tag = master.AppRole;

                dgvAppSteps[AppComments.Name, index].Value = master.AppComments;
                dgvAppSteps[AppDateTime.Name, index].Value = master.AppDate;
                dgvAppSteps[AppPerson.Name, index].Value = master.AuditPerson.Name;

                dgvAppSteps.Rows[index].Tag = master;
                switch (master.AppStatus)
                {
                    case -1:
                        dgvAppSteps[AppStatus.Name, index].Value = "已撤单";
                        break;
                    case 0:
                        dgvAppSteps[AppStatus.Name, index].Value = "审批中";
                        break;
                    case 1:
                        dgvAppSteps[AppStatus.Name, index].Value = "未通过";
                        break;
                    case 2:
                        dgvAppSteps[AppStatus.Name, index].Value = "已通过";
                        break;
                    default:
                        break;
                }
            }
        }

        private AppStepsInfo BuildStepsInfo(ApproveBill bill)
        {
            if (bill == null)
            {
                return null;
            }

            var stepInfo = new AppStepsInfo();
            stepInfo.AppDate = DateTime.Now;
            stepInfo.AppTableSet = bill.AppTableDefine;
            stepInfo.AuditPerson = ConstObject.TheLogin.ThePerson;
            stepInfo.BillId = bill.BillId;
            stepInfo.StepOrder = bill.NextStep;

            return stepInfo;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            decimal approveMoney = 0;
            if (selectPlan == null || selectAppBill == null)
            {
                MessageBox.Show("请选择要审批的单据");
                return;
            }

            if (!decimal.TryParse(txtApproveMoney.Text.Trim(), out approveMoney))
            {
                MessageBox.Show("请输入批准额度");
                txtApproveMoney.Focus();
                return;
            }

            var totalPlanPay = 0m;
            if (selectPlan is ProjectFundPlanMaster)
            {
                ProjectFundPlanMaster prjPlan = mOperate.FinanceMultDataSrv.GetProjectFundPlanById(selectPlan.Id);
                totalPlanPay = prjPlan.PresentMonthPayment;
            }
            else if (selectPlan is FilialeFundPlanMaster)
            {
                FilialeFundPlanMaster filPlan = mOperate.FinanceMultDataSrv.GetFilialeFundPlanById(selectPlan.Id);
                totalPlanPay = filPlan.PresentMonthPlanPayment;
            }

            if (approveMoney > totalPlanPay)
            {
                MessageBox.Show("批准额度不能大于本期计划申报付款额" + totalPlanPay.ToString("N2") + "，请重新输入！");
                txtApproveMoney.Focus();
                return;
            }

            if (MessageBox.Show("您确认要审批通过？", "审批确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            var stepInfo = BuildStepsInfo(selectAppBill);
            stepInfo.AppStatus = 2;
            stepInfo.AppComments = string.Format("批复额度：{0}，{1}", approveMoney, txtReark.Text);
            stepInfo.TempData = new DataDomain();
            stepInfo.TempData.Name1 = approveMoney;

            if (!appPlatform.Service.SubmitApprove(stepInfo, selectAppBill))
            {
                MessageBox.Show("审批失败");
            }

            MessageBox.Show("审批成功");

            LoadApproveTask();

            ClearInfo();
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            if (selectPlan == null || selectAppBill == null)
            {
                MessageBox.Show("请选择要审批的单据");
                return;
            }

            if(string.IsNullOrEmpty(txtReark.Text.Trim()))
            {
                MessageBox.Show("请输入审批意见");
                txtReark.Focus();
                return;
            }

            if (MessageBox.Show("您确认要审批驳回？", "审批确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            var stepInfo = BuildStepsInfo(selectAppBill);
            stepInfo.AppStatus = 1;
            stepInfo.AppComments = txtReark.Text.Trim();
            stepInfo.TempData = new DataDomain();
            stepInfo.TempData.Name1 = 0m;

            if (!appPlatform.Service.SubmitApprove(stepInfo, selectAppBill))
            {
                MessageBox.Show("审批失败");
                return;
            }

            MessageBox.Show("审批成功");

            LoadApproveTask();

            ClearInfo();
        }

        private void dgBillList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                return;
            }

            if (mOperate == null)
            {
                mOperate = new MFinanceMultData();
            }

            selectAppBill = dgBillList.Rows[e.RowIndex].DataBoundItem as ApproveBill;
            if (selectAppBill == null)
            {
                MessageBox.Show("请选择要审批的单据");
                return;
            }
            selectAppBill.ApproveJob = ConstObject.TheSysRole.Id;

            ObjectQuery objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("Code", selectAppBill.BillCode));
            objQuery.AddFetchMode("Details", FetchMode.Eager);

            if (selectAppBill.BillCode.IndexOf("_项目_") > -1)
            {
                objQuery.AddFetchMode("OtherPayDetails", FetchMode.Eager);
                var list = mOperate.FinanceMultDataSrv.Query(typeof (ProjectFundPlanMaster), objQuery);
                if(list!=null && list.Count>0)
                {
                    var projectPlan = list[0] as ProjectFundPlanMaster;
                    selectPlan = projectPlan;

                    gdProjectReport.Locked = false;
                    FundPlanOperate.DisplayProjectPlanFundFlow(projectPlan, gdProjectReport);
                    FundPlanOperate.DisplayProjectReportDetail(projectPlan, gdProjectReport);
                    gdProjectReport.Locked = true;

                    txtApproveMoney.Text = projectPlan.ApprovalAmount.ToString();
                }

                tpCompanyReport.Parent = null;
                tpProjectReport.Parent = tabControl1;
            }
            else
            {
                objQuery.AddFetchMode("OfficeFundPlanDetails", FetchMode.Eager);
                var list = mOperate.FinanceMultDataSrv.Query(typeof(FilialeFundPlanMaster), objQuery);
                if (list != null && list.Count > 0)
                {
                    var filialePlan = list[0] as FilialeFundPlanMaster;
                    selectPlan = filialePlan;

                    gdCompanyReport.Locked = false;
                    FundPlanOperate.DisplayFilialePlanMaster(filialePlan, gdCompanyReport,
                                                             string.Concat(tpCompanyReport.Tag, ".flx"));
                    FundPlanOperate.DisplayFilialePlanReportDetail(filialePlan, gdCompanyReport);
                    gdCompanyReport.Locked = true;

                    txtApproveMoney.Text = filialePlan.Approval.ToString();
                }

                tpCompanyReport.Parent = tabControl1;
                tpProjectReport.Parent = null;
            }

            LoadApproveStep();
        }

        private void txtApproveMoney_TextChanged(object sender, EventArgs e)
        {
            decimal tmp = 0;
            decimal.TryParse(txtApproveMoney.Text.Trim(), out tmp);

            lbMoney.Text = tmp.ToString("N2");
        }
    }
}
