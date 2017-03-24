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
using Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatformUI;
using Application.Business.Erp.SupplyChain.Approval.AppProcessMng.Domain;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Base.Domain;
using System.Collections;
using VirtualMachine.Component.Util;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan
{
    public partial class VFundPayApplyApprove : TBasicDataView
    {
        private MAppPlatform appPlatform = new MAppPlatform();
        private ApproveBill selectAppBill;

        private BaseMaster selectPaymentRequest;

        private MFinanceMultData mOperate = new MFinanceMultData();
        private IList paymentMasterList = new List<PaymentMaster>();

        private int lineNum = 0;

        public VFundPayApplyApprove()
        {
            InitializeComponent();

            InitData();

            InitEvents();
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
                dgBillList.DataSource = tbBills.OfType<ApproveBill>().ToList().FindAll(
                    a => a.BillCode.StartsWith("资金支付"));

            }
            else
            {
                dgBillList.DataSource = null;
            }

            ClearInfo();
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

        private void InitData()
        {
            InitGrid();

            LoadApproveTask();
        }

        private void InitGrid()
        {
            FundPlanOperate.LoadFlexFile(string.Concat("资金支付申请单.flx"), gdFundPayApply);
        }

        private void InitEvents()
        {
            gdFundPayApply.Click += new FlexCell.Grid.ClickEventHandler(gdFundPayApply_Click);
            this.btnRejectAppointedLine.Click += new EventHandler(btnRejectAppointedLine_Click);
        }

        private void ClearInfo()
        {
            selectAppBill = null;
            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                foreach (var ctrl in tabPage.Controls)
                {
                    if (ctrl is CustomFlexGrid)
                    {
                        LoadTempleteFile(ctrl as CustomFlexGrid, tabPage.Tag + ".flx");
                    }
                }
            }
        }

        private void LoadTempleteFile(CustomFlexGrid grid, string sReportPath)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(sReportPath))
            {
                eFile.CreateTempleteFileFromServer(sReportPath);
                //载入格式和数据
                grid.OpenFile(path + "\\" + sReportPath); //载入格式
                grid.SelectionStart = 0;
            }
            else
            {
                MessageBox.Show("未找到模板格式文件【" + sReportPath + "】");
            }
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

            if (selectAppBill.BillCode.IndexOf("资金支付申请") > -1)
            {
                var list = mOperate.FinanceMultDataSrv.Query(typeof(PaymentRequest), objQuery);
                if (list != null && list.Count > 0)
                {
                    var request = list[0] as PaymentRequest;
                    selectPaymentRequest = request;

                    gdFundPayApply.Cell(2, 1).Text = string.Format("单位名称:{0}", request.OperOrgInfoName);
                    gdFundPayApply.Cell(2, 8).Text = string.Format("申请日期:{0}", request.CreateDate.ToShortDateString());
                    gdFundPayApply.Cell(2, 12).Text = string.Format("申请单号:{0}", request.Code);

                    gdFundPayApply.Cell(3, 3).Text = request.CurrentPlanGether.ToString();
                    gdFundPayApply.Cell(3, 5).Text = request.CurrentRealGether.ToString();
                    gdFundPayApply.Cell(3, 7).Text = request.CurrentPlanPay.ToString();
                    gdFundPayApply.Cell(3, 9).Text = request.CurrentRealPay.ToString();

                    paymentMasterList.Clear();
                    paymentMasterList = mOperate.FinanceMultDataSrv.GetPaymentMasterByRequestBill(request);

                    VFundPayApplyFormation payApplyFormation = new VFundPayApplyFormation();
                    FundPlanOperate.LoadFlexFile(string.Concat("资金支付申请单.flx"), gdFundPayApply);
                    payApplyFormation.DisplayFundPayApplyFormationByExecuteDoc(gdFundPayApply, paymentMasterList);

                    //gdFundPayApply.Locked = false;
                    //gdFundPayApply.Locked = true;
                }
            }
            LoadApproveStep();
        }

        private void btnRejectAppointedLine_Click(object sender, EventArgs e)
        {
            if (paymentMasterList == null)
            {
                return;
            }

            var startRowIndex = 5;
            if (lineNum >= startRowIndex)
            {
                PaymentMaster payment = paymentMasterList[lineNum - startRowIndex] as PaymentMaster;
                payment.DocState = DocumentState.Edit;

                mOperate.FinanceMultDataSrv.SavePaymentMaster(payment);

                MessageBox.Show(string.Format("此明细记录已经驳回！"));
            }
        }

        private void gdFundPayApply_Click(object Sender, EventArgs e)
        {
            lineNum = gdFundPayApply.MouseRow;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (selectPaymentRequest == null)
            {
                return;
            }

            if (selectPaymentRequest is PaymentRequest)
            {
                var stepInfo = BuildStepsInfo(selectAppBill);
                stepInfo.AppStatus = 2;
                stepInfo.TempData = new DataDomain();

                if (!appPlatform.Service.SubmitApprove(stepInfo, selectAppBill))
                {
                    MessageBox.Show("审批失败");
                }

                MessageBox.Show("审批成功");

                LoadApproveTask();

                //ClearInfo();
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
    }
}
