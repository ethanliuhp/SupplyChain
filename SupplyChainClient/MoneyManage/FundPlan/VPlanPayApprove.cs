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
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Approval.AppProcessMng.Domain;
using VirtualMachine.Core;
using VirtualMachine.Component.Util;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData;
using NHibernate;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using FlexCell;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan
{
    public partial class VPlanPayApprove : TBasicDataView
    {
        private MFinanceMultData mOperate;
        private MAppPlatform appPlatform = new MAppPlatform();
        private ApproveBill selectAppBill;
        private PaymentMaster selectPayment;
        private CurrentProjectInfo currentProject;
        private Boolean rowAdd = false;

        public VPlanPayApprove()
        {
            InitializeComponent();

            InitData();
        }

        private void InitEvents()
        {
            dgBillList.CellDoubleClick += new DataGridViewCellEventHandler(dgBillList_CellDoubleClick);
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
            if (selectPayment == null)
            {
                return;
            }

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BillId", selectPayment.Id));
            oq.AddCriterion(Expression.Eq("AppStatus", ClientUtil.ToLong(2)));
            oq.AddCriterion(Expression.Eq("State", 1));
            oq.AddFetchMode("AuditPerson", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("AppRole", NHibernate.FetchMode.Eager);

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

        private void ClearInfo()
        {
            selectAppBill = null;
            selectPayment = null;
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

        private void InitData()
        {
            InitGrid();

            LoadApproveTask();
        }

        private void InitGrid()
        {
            FundPlanOperate.LoadFlexFile(string.Concat(tpPlanPay.Tag, ".flx"), gdPlanPay);

            FundPlanOperate.LoadFlexFile(string.Concat(tpPlanPay2.Tag, ".flx"), gdPlanPay2);
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
            if (selectAppBill == null)
            {
                MessageBox.Show("请选择要审批的单据");
                return;
            }
            selectAppBill.ApproveJob = ConstObject.TheSysRole.Id;

            ObjectQuery objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("Code", selectAppBill.BillCode));
            objQuery.AddFetchMode("Details", FetchMode.Eager);

            selectPayment = mOperate.FinanceMultDataSrv.GetPaymentMasterById(selectAppBill.BillId);

            if (selectAppBill.BillCode.IndexOf("资金支付") > -1)
            {
                    ProjectFundPlanDetail fundPlanDetail;

                    if (selectPayment == null)
                    {
                        return;
                    }
                    else
                    {
                        fundPlanDetail = selectPayment.FundPlan;
                    }

                    int startRowIndex = 3;

                    var isSimple = string.IsNullOrEmpty(selectPayment.FundPlanCode);
                    var grid = isSimple ? gdPlanPay2 : gdPlanPay;
                    if (!string.IsNullOrEmpty(selectPayment.FundPlanCode))//资金支付审批单1
                    {
                        gdPlanPay.Cell(startRowIndex, 1).Text = "项目：" + selectPayment.ProjectName;
                        gdPlanPay.Cell(startRowIndex, 5).Text = selectPayment.CreateDate.ToShortDateString();
                        gdPlanPay.Cell(startRowIndex, 6).Text = selectPayment.FundPlanCode;
                        startRowIndex++;
                        gdPlanPay.Cell(startRowIndex, 4).Text = selectPayment.TheSupplierName;
                        gdPlanPay.Cell(startRowIndex, 6).Text = selectPayment.OriginalCollectionUnit;
                        startRowIndex++;
                        gdPlanPay.Cell(startRowIndex, 4).Text = selectPayment.BankAccountNo;
                        gdPlanPay.Cell(startRowIndex, 6).Text = selectPayment.AccountTitleName;
                        startRowIndex++;
                        gdPlanPay.Cell(startRowIndex, 4).Text = selectPayment.BankName;
                        gdPlanPay.Cell(startRowIndex, 6).Text = selectPayment.PaymentClause;
                        startRowIndex++;
                        gdPlanPay.Cell(startRowIndex, 4).Text = fundPlanDetail.ContractAmount.ToString("N2"); ;
                        gdPlanPay.Cell(startRowIndex, 6).Text = fundPlanDetail.CumulativeSettlement.ToString("N2"); ;
                        startRowIndex++;
                        gdPlanPay.Cell(startRowIndex, 4).Text = fundPlanDetail.CumulativePayment.ToString("N2"); ;
                        gdPlanPay.Cell(startRowIndex, 6).Text = fundPlanDetail.PaymentRatio.ToString("N2"); ;
                        startRowIndex++;
                        gdPlanPay.Cell(startRowIndex, 4).Text = fundPlanDetail.Quota.ToString("N2");
                        startRowIndex++;
                        startRowIndex++;
                        startRowIndex++;
                        startRowIndex++;
                        gdPlanPay.Cell(startRowIndex, 4).Text = selectPayment.SumMoney.ToString("N2");

                        gdPlanPay.Cell(gdPlanPay.Rows - 1, 4).Text = selectPayment.CreatePersonName;
                        gdPlanPay.Cell(gdPlanPay.Rows - 1, 6).Text = selectPayment.CreateDate.ToString();
                    }
                    else//资金支付审批单2
                    {
                        currentProject = mOperate.CurrentProjectSrv.GetProjectById(selectPayment.ProjectId);
                        if (currentProject != null)
                        {
                            gdPlanPay2.Cell(startRowIndex, 1).Text = string.Format("项目：{0}", currentProject.Name);
                            gdPlanPay2.Cell(startRowIndex, 1).Tag = currentProject.Id;
                        }
                        else
                        {
                            gdPlanPay2.Cell(startRowIndex, 1).Text = string.Empty;
                            gdPlanPay2.Cell(startRowIndex, 1).Tag = string.Empty;
                        }
                        gdPlanPay2.Cell(startRowIndex, 5).Text = selectPayment.CreateDate.ToString("yyyy-MM-dd");
                        startRowIndex++;
                        gdPlanPay2.Cell(startRowIndex, 4).Text = selectPayment.TheSupplierName;
                        gdPlanPay2.Cell(startRowIndex, 6).Text = selectPayment.AccountTitleName;
                        startRowIndex++;
                        gdPlanPay2.Cell(startRowIndex, 4).Text = selectPayment.BankAccountNo;
                        //gdPlanPay2.Cell(startRowIndex, 6).Text;
                        startRowIndex++;
                        gdPlanPay2.Cell(startRowIndex, 4).Text = selectPayment.BankName;
                        gdPlanPay2.Cell(startRowIndex, 6).Text = selectPayment.RefundDate.ToShortDateString();
                        startRowIndex++;
                        startRowIndex++;
                        gdPlanPay2.Cell(startRowIndex, 4).Text = selectPayment.SumMoney.ToString();

                        gdPlanPay2.Cell(gdPlanPay2.Rows - 1, 4).Text = selectPayment.CreatePersonName;
                        gdPlanPay2.Cell(gdPlanPay2.Rows - 1, 6).Text = selectPayment.CreateDate.ToString();
                    }

                    #region 明细
                    var startRow = isSimple ? 9 : 14;
                    var rIndex = 0;
                    foreach (PaymentDetail pay in selectPayment.Details)
                    {
                        rIndex++;

                        if (!rowAdd)
                        {
                            AddNewDetailRow(grid);
                        }

                        grid.Cell(startRow + rIndex, 1).Text = rIndex.ToString();
                        grid.Cell(startRow + rIndex, 1).Tag = pay.Id;
                        grid.Cell(startRow + rIndex, 2).Text = pay.Descript;

                        SetPayDetailType(startRow + rIndex, 2, grid);
                        if (pay.AcceptBillID != null)
                        {
                            grid.Cell(startRow + rIndex, 3).Text = "票据号码";
                            grid.Cell(startRow + rIndex, 4).Tag = pay.AcceptBillID.Id;
                            grid.Cell(startRow + rIndex, 4).Text = pay.AcceptBillID.BillNo;
                            grid.Cell(startRow + rIndex, 5).Text = "票据金额";
                            grid.Cell(startRow + rIndex, 6).Text = pay.AcceptBillID.SumMoney.ToString("N2");
                        }
                        else
                        {
                            grid.Cell(startRow + rIndex, 3).Text = pay.Money.ToString("N2");
                        }
                    }

                    #endregion
            }
            LoadApproveStep();
        }

        private void SetPayDetailType(int rIndex, int cIndex, CustomFlexGrid grid)
        {
            var txt = grid.Cell(rIndex, cIndex).Text;
            var range = grid.Range(rIndex, cIndex + 1, rIndex, grid.Cols - 1);
            range.Locked = false;
            range.MergeCells = false;
            if (txt.Contains("汇票"))
            {
                grid.Cell(rIndex, cIndex + 1).Text = "票据号码";
                grid.Cell(rIndex, cIndex + 3).Text = "票据金额";

                range.set_Borders(EdgeEnum.Inside, LineStyleEnum.Thin);
                range.set_Borders(EdgeEnum.Bottom, LineStyleEnum.Thin);

                range.Locked = true;
                grid.Cell(rIndex, cIndex + 2).CellType = CellTypeEnum.Button;
                grid.Cell(rIndex, cIndex + 2).Locked = false;
            }
            else
            {
                range.Merge();
                range.ClearText();
            }
        }

        private void btnAgree_Click(object sender, EventArgs e)
        {
            if (selectPayment
                == null)
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

        private void AddNewDetailRow(CustomFlexGrid grid)
        {
            grid.InsertRow(grid.Rows - 1, 1);
            var range = grid.Range(grid.Rows - 2, 1, grid.Rows - 2, grid.Cols - 1);
            range.set_Borders(EdgeEnum.Inside, LineStyleEnum.Thin);
            range.set_Borders(EdgeEnum.Bottom, LineStyleEnum.Thin);
            range.FontSize = 10;

            var indexCell = grid.Cell(grid.Rows - 2, 1);
            indexCell.Text = (indexCell.Row - (grid == gdPlanPay ? 14 : 9)).ToString();
            indexCell.FontSize = 10;
            indexCell.Locked = true;
            indexCell.FontBold = true;
            indexCell.Alignment = AlignmentEnum.CenterCenter;

            var cmbCell = grid.Cell(grid.Rows - 2, 2);
            cmbCell.CellType = CellTypeEnum.ComboBox;
            cmbCell.FontSize = 10;

            grid.ComboBox(0).DropDownFont = grid.DefaultFont;
            grid.ComboBox(0).DropDownWidth = 120;
            rowAdd = true;
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            if (selectPayment == null || selectAppBill == null)
            {
                MessageBox.Show("请选择要审批的单据");
                return;
            }

            if (string.IsNullOrEmpty(txtRemark.Text.Trim()))
            {
                MessageBox.Show("请输入审批意见");
                txtRemark.Focus();
                return;
            }

            if (MessageBox.Show("您确认要审批驳回？", "审批确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            var stepInfo = BuildStepsInfo(selectAppBill);
            stepInfo.AppStatus = 1;
            stepInfo.AppComments = txtRemark.Text.Trim();
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
    }
}
