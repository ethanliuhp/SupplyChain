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
using Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatformUI;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Application.Business.Erp.SupplyChain.Util;
using FlexCell;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Core;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage
{
    public partial class VFundShcemeApprove : TBasicDataView
    {
        private MFinanceMultData mOperate;
        private FundPlanningMaster selectFundScheme;
        private ApproveBill selectAppBill;
        private MAppPlatform appPlatform = new MAppPlatform();
        private string rootSyscode;

        public VFundShcemeApprove()
        {
            InitializeComponent();

            InitEvents();

            GetRootSyscode();

            LoadApproveTask();
        }

        private void InitEvents()
        {
            dgBillList.AutoGenerateColumns = false;
            dgBillList.CellDoubleClick += new DataGridViewCellEventHandler(dgBillList_CellDoubleClick);

            btnAgree.Click += new EventHandler(btnAgree_Click);
            btnReject.Click += new EventHandler(btnReject_Click);
        }

        private void GetRootSyscode()
        {
            if (!string.IsNullOrEmpty(rootSyscode))
            {
                return;
            }

            var proj = StaticMethod.GetProjectInfo();
            if (proj != null && proj.Code != CommonUtil.CompanyProjectCode)
            {
                rootSyscode = proj.OwnerOrgSysCode;
                return;
            }

            var subCompany = StaticMethod.GetSubCompanyOrgInfo();
            if (subCompany != null)
            {
                rootSyscode = subCompany.SysCode;
            }
            else if (ConstObject.TheOperationOrg.SysCode.StartsWith(TransUtil.CompanyOpgSyscode))
            {
                rootSyscode = TransUtil.CompanyOpgSyscode;
            }
            else
            {
                rootSyscode = ConstObject.TheOperationOrg.SysCode;
            }
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
                    .FindAll(a => a.BillCode.StartsWith("资金策划") && a.BillSysCode.StartsWith(rootSyscode));
            }
            else
            {
                dgBillList.DataSource = null;
            }

            ClearInfo();
        }

        private void LoadApproveStep()
        {
            dgvAppSteps.Rows.Clear();
            if (selectFundScheme == null)
            {
                return;
            }

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BillId", selectFundScheme.Id));
            oq.AddCriterion(Expression.Eq("AppStatus", ClientUtil.ToLong(2)));
            oq.AddCriterion(Expression.Eq("State", 1));
            oq.AddFetchMode("AuditPerson", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("AppRole", NHibernate.FetchMode.Eager);
            oq.AddOrder(new Order("StepOrder", true));

            var steps = appPlatform.Service.GetAppStepsInfo(oq);
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

        private void ClearInfo()
        {
            selectAppBill = null;
            selectFundScheme = null;

            dgvAppSteps.Rows.Clear();

            ucFundSchemeDetail1.InitReport();
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

            selectAppBill = dgBillList.CurrentRow.DataBoundItem as ApproveBill;
            selectAppBill.ApproveJob = ConstObject.TheSysRole.Id;

            selectFundScheme =
                mOperate.FinanceMultDataSrv.GetFundSchemeById(
                    dgBillList.Rows[e.RowIndex].Cells[colBillId.Name].Value.ToString());
            
            LoadApproveStep();

            ucFundSchemeDetail1.LoadFundScheme(selectFundScheme);
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            if (selectFundScheme == null)
            {
                MessageBox.Show("请选择待审批的记录");
                return;
            }

            if(string.IsNullOrEmpty(txtRemark.Text.Trim()))
            {
                MessageBox.Show("审批[不通过]必须填写审批意见！");
                txtRemark.Focus();
                return;
            }

            if (MessageBox.Show("确认要审批不通过吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            var stepInfo = BuildStepsInfo(selectAppBill);
            stepInfo.AppStatus = 1;
            stepInfo.AppComments = txtRemark.Text.Trim();

            if (!appPlatform.Service.SubmitApprove(stepInfo, selectAppBill))
            {
                MessageBox.Show("审批失败");
            }
            else
            {
                MessageBox.Show("审批成功");

                LoadApproveTask();
            }
        }

        private void btnAgree_Click(object sender, EventArgs e)
        {
            if (selectFundScheme == null)
            {
                MessageBox.Show("请选择待审批的记录");
                return;
            }

            if (MessageBox.Show("确认要审批通过吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            var stepInfo = BuildStepsInfo(selectAppBill);
            stepInfo.AppStatus = 2;
            stepInfo.AppComments = txtRemark.Text.Trim();
            if (!appPlatform.Service.SubmitApprove(stepInfo, selectAppBill))
            {
                MessageBox.Show("审批失败");
            }
            else
            {
                MessageBox.Show("审批成功");

                LoadApproveTask();
            }
        }
    }
}
