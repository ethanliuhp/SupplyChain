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
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData;
using System.Collections;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.WinControls.Controls;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan
{
    public partial class VFundPayApplyFormation : TBasicDataView
    {
        private CurrentProjectInfo projectInfo = null;
        private IList paymentMasterList;
        private MFinanceMultData mOperate = new MFinanceMultData();
        private DataDomain selectSupplier;
        //private IList paymentRequestList;

        OperationOrgInfo subOrgInfo = StaticMethod.GetSubCompanyOrgInfo();

        private ProjectFundPlanDetail selectPlan;
        private ProjectFundPlanMaster selectPlanMaster;
        private PaymentRequest paymentRequest;

        private Boolean rowsAdded = false;
        private Boolean paymentRequestAdded = false;

        public VFundPayApplyFormation()
        {
            InitializeComponent();

            InitData();
        }

        private void InitData()
        {
            this.dtpBeginCreateDate.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpEndCreateDate.Value = ConstObject.TheLogin.LoginDate;

            requestTypeCBox.SelectedIndex = 0;

            projectInfo = StaticMethod.GetProjectInfo();

            InitGrid();
        }

        public void InitEvents()
        {
            this.btnGenerate.Click += new EventHandler(btnGenerate_Click);
            this.submitBtn.Click += new EventHandler(submitBtn_Click);
        }

        private void InitGrid()
        {
            FundPlanOperate.LoadFlexFile(string.Concat("资金支付申请单.flx"), gdFundPayApply);
        }

        public void DisplayFundPayApplyFormation(CustomFlexGrid grid, IList paymentMasterList)
        {
            if (projectInfo == null)
            {
                return;   
            }

            try
            {
                grid.Cell(2, 1).Text = string.Format("单位名称:{0}", subOrgInfo.Name);
                grid.Cell(2, 8).Text = string.Format("申请日期:{0}", DateTime.Now.ToShortDateString());

                int startRowIndex = 5;
                int startColIndex = 1;
                int sequenceNum = 1;

                decimal PresentMonthPlanGatheringAmount = 0m;
                decimal PresentMonthPlanPaymentAmount = 0m;
                decimal PresentMonthRealGatheringAmount = 0m;
                decimal PresentMonthRealPaymentAmount = 0m;

                foreach (PaymentMaster payment in paymentMasterList)
                {
                    if (!rowsAdded)
                    {
                        grid.InsertRow(startRowIndex, 1);
                    }

                    selectPlan = payment.FundPlan;
                    selectPlanMaster = selectPlan.Master as ProjectFundPlanMaster;

                    grid.Cell(startRowIndex, 1).Tag = selectPlanMaster.ProjectId;

                    ObjectQuery objectQuery = new ObjectQuery();

                    objectQuery.AddCriterion(Expression.Eq("ProjectId", selectPlanMaster.ProjectId));

                    //业务时间
                    objectQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpBeginCreateDate.Value.Date));
                    objectQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpEndCreateDate.Value.AddDays(1).Date));

                    //objectQuery.AddCriterion(Expression.Eq("State", DocumentState.InExecute));
                    IList gatheringMasterList = mOperate.FinanceMultDataSrv.GetGatheringMaster(objectQuery);
                    foreach (GatheringMaster gatheringMaster in gatheringMasterList)
                    {
                        PresentMonthRealGatheringAmount += gatheringMaster.SumMoney;
                    }

                    PresentMonthPlanGatheringAmount += selectPlanMaster.PresentMonthGathering;
                    PresentMonthPlanPaymentAmount += selectPlanMaster.PresentMonthPayment;

                    PresentMonthRealPaymentAmount += payment.SumMoney;

                    

                    startColIndex = 1;
                    grid.Cell(startRowIndex, startColIndex).Text = sequenceNum.ToString();
                    startColIndex++;
                    if (projectInfo != null)
                    {
                        grid.Cell(startRowIndex, startColIndex).Text = payment.ProjectName;
                    }
                    startColIndex++;

                    #region 收款单位名称
                    selectSupplier = new DataDomain();
                    if (string.IsNullOrEmpty(payment.TheSupplierRelationInfo))
                    {
                        grid.Cell(startRowIndex, startColIndex).Text = payment.TheCustomerName;
                    }
                    else
                    {
                        grid.Cell(startRowIndex, startColIndex).Text = payment.TheSupplierName;
                    }
                    startColIndex++;
                    #endregion

                    startColIndex++;
                    grid.Cell(startRowIndex, startColIndex).Text = payment.SumMoney.ToString();
                    startColIndex++;
                    grid.Cell(startRowIndex, startColIndex).Text = payment.BankAccountNo;
                    startColIndex++;
                    //省市
                    if (mOperate.FinanceMultDataSrv.GetProjectInfoById(payment.ProjectId) != null)
                    {
                        CurrentProjectInfo proInfo = mOperate.FinanceMultDataSrv.GetProjectInfoById(payment.ProjectId);
                        grid.Cell(startRowIndex, startColIndex).Text = proInfo.ProjectLocationProvince + proInfo.ProjectLocationCity;
                    }
                    startColIndex++;
                    grid.Cell(startRowIndex, startColIndex).Text = payment.BankName;
                    startColIndex++;
                    grid.Cell(startRowIndex, startColIndex).Text = payment.AccountTitleName;
                    startColIndex++;
                    //合同额
                    grid.Cell(startRowIndex, startColIndex).Text = Convert.ToDouble(selectPlan.ContractAmount / 10000).ToString("0.00");
                    startColIndex++;
                    //合同约定比例
                    grid.Cell(startRowIndex, startColIndex).Text = selectPlan.ContractPaymentRatio.ToString("N2");
                    startColIndex++;
                    grid.Cell(startRowIndex, startColIndex).Text = Convert.ToDouble(payment.AddBalMoney / 10000).ToString("0.00");
                    startColIndex++;
                    //累计付款额
                    grid.Cell(startRowIndex, startColIndex).Text = Convert.ToDouble(selectPlan.CumulativePayment / 10000).ToString("0.00");
                    startColIndex++;
                    //实际付款比例
                    grid.Cell(startRowIndex, startColIndex).Text = selectPlan.PaymentRatio.ToString("N2");
                    startColIndex++;

                    grid.Range(startRowIndex, 3, startRowIndex, 4).Merge();
                    sequenceNum++;
                    startRowIndex++;
                }
                rowsAdded = true;
                grid.Cell(3, 3).Text = PresentMonthPlanGatheringAmount.ToString("N2");
                grid.Cell(3, 5).Text = PresentMonthRealGatheringAmount.ToString("N2");
                grid.Cell(3, 7).Text = PresentMonthPlanPaymentAmount.ToString("N2");
                grid.Cell(3, 9).Text = PresentMonthRealPaymentAmount.ToString("N2");
                grid.Range(startRowIndex, 3, startRowIndex, 4).Merge();

                paymentRequest = new PaymentRequest();
                paymentRequest.OperOrgInfoName = subOrgInfo.Name;
                paymentRequest.OperOrgInfo = subOrgInfo;
                paymentRequest.RequestType = requestTypeCBox.SelectedItem.ToString();
                paymentRequest.CurrentPlanGether = PresentMonthPlanGatheringAmount;
                paymentRequest.CurrentPlanPay = PresentMonthPlanPaymentAmount;
                paymentRequest.CurrentRealGether = PresentMonthRealPaymentAmount;
                paymentRequest.DocState = DocumentState.Edit;
                paymentRequest.OpgSysCode = subOrgInfo.SysCode;
                paymentRequest.CreateDate = DateTime.Now;
                paymentRequest.CreatePerson = ConstObject.TheLogin.ThePerson;
                paymentRequest.CreatePersonName = ConstObject.TheLogin.ThePerson.Name;
            }
            catch (Exception ex)
            {
                MessageBox.Show("生成数据出错。\n" + ex.Message);
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        public void DisplayFundPayApplyFormationByExecuteDoc(CustomFlexGrid grid, IList paymentMasterList)
        {
            if (projectInfo == null)
            {
                return;
            }

            try
            {
                grid.Cell(2, 1).Text = string.Format("单位名称:{0}", subOrgInfo.Name);
                grid.Cell(2, 8).Text = string.Format("申请日期:{0}", DateTime.Now.ToShortDateString());

                int startRowIndex = 5;
                int startColIndex = 1;
                int sequenceNum = 1;

                decimal PresentMonthPlanGatheringAmount = 0m;
                decimal PresentMonthPlanPaymentAmount = 0m;
                decimal PresentMonthRealGatheringAmount = 0m;
                decimal PresentMonthRealPaymentAmount = 0m;

                foreach (PaymentMaster payment in paymentMasterList)
                {
                    if (!rowsAdded)
                    {
                        grid.InsertRow(startRowIndex, 1);
                    }

                    selectPlan = payment.FundPlan;
                    selectPlanMaster = selectPlan.Master as ProjectFundPlanMaster;

                    grid.Cell(startRowIndex, 1).Tag = selectPlanMaster.ProjectId;

                    ObjectQuery objectQuery = new ObjectQuery();

                    objectQuery.AddCriterion(Expression.Eq("ProjectId", selectPlanMaster.ProjectId));

                    //业务时间
                    objectQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpBeginCreateDate.Value.Date));
                    objectQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpEndCreateDate.Value.AddDays(1).Date));

                    objectQuery.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
                    IList gatheringMasterList = mOperate.FinanceMultDataSrv.GetGatheringMaster(objectQuery);
                    foreach (GatheringMaster gatheringMaster in gatheringMasterList)
                    {
                        PresentMonthRealGatheringAmount += gatheringMaster.SumMoney;
                    }

                    PresentMonthPlanGatheringAmount += selectPlanMaster.PresentMonthGathering;
                    PresentMonthPlanPaymentAmount += selectPlanMaster.PresentMonthPayment;

                    PresentMonthRealPaymentAmount += payment.SumMoney;



                    startColIndex = 1;
                    grid.Cell(startRowIndex, startColIndex).Text = sequenceNum.ToString();
                    startColIndex++;
                    if (projectInfo != null)
                    {
                        grid.Cell(startRowIndex, startColIndex).Text = payment.ProjectName;
                    }
                    startColIndex++;

                    #region 收款单位名称
                    selectSupplier = new DataDomain();
                    if (string.IsNullOrEmpty(payment.TheSupplierRelationInfo))
                    {
                        grid.Cell(startRowIndex, startColIndex).Text = payment.TheCustomerName;
                    }
                    else
                    {
                        grid.Cell(startRowIndex, startColIndex).Text = payment.TheSupplierName;
                    }
                    startColIndex++;
                    #endregion

                    startColIndex++;
                    grid.Cell(startRowIndex, startColIndex).Text = payment.SumMoney.ToString();
                    startColIndex++;
                    grid.Cell(startRowIndex, startColIndex).Text = payment.BankAccountNo;
                    startColIndex++;
                    //省市
                    if (mOperate.FinanceMultDataSrv.GetProjectInfoById(payment.ProjectId) != null)
                    {
                        CurrentProjectInfo proInfo = mOperate.FinanceMultDataSrv.GetProjectInfoById(payment.ProjectId);
                        grid.Cell(startRowIndex, startColIndex).Text = proInfo.ProjectLocationProvince + proInfo.ProjectLocationCity;
                    }
                    startColIndex++;
                    grid.Cell(startRowIndex, startColIndex).Text = payment.BankName;
                    startColIndex++;
                    grid.Cell(startRowIndex, startColIndex).Text = payment.AccountTitleName;
                    startColIndex++;
                    //合同额
                    grid.Cell(startRowIndex, startColIndex).Text = Convert.ToDouble(selectPlan.ContractAmount / 10000).ToString("0.00");
                    startColIndex++;
                    //合同约定比例
                    grid.Cell(startRowIndex, startColIndex).Text = selectPlan.ContractPaymentRatio.ToString("N2");
                    startColIndex++;
                    grid.Cell(startRowIndex, startColIndex).Text = Convert.ToDouble(payment.AddBalMoney / 10000).ToString("0.00");
                    startColIndex++;
                    //累计付款额
                    grid.Cell(startRowIndex, startColIndex).Text = Convert.ToDouble(selectPlan.CumulativePayment / 10000).ToString("0.00");
                    startColIndex++;
                    //实际付款比例
                    grid.Cell(startRowIndex, startColIndex).Text = selectPlan.PaymentRatio.ToString("N2");
                    startColIndex++;

                    grid.Range(startRowIndex, 3, startRowIndex, 4).Merge();
                    sequenceNum++;
                    startRowIndex++;
                }
                rowsAdded = true;
                grid.Cell(3, 3).Text = PresentMonthPlanGatheringAmount.ToString("N2");
                grid.Cell(3, 5).Text = PresentMonthRealGatheringAmount.ToString("N2");
                grid.Cell(3, 7).Text = PresentMonthPlanPaymentAmount.ToString("N2");
                grid.Cell(3, 9).Text = PresentMonthRealPaymentAmount.ToString("N2");
                grid.Range(startRowIndex, 3, startRowIndex, 4).Merge();

                paymentRequest = new PaymentRequest();
                paymentRequest.OperOrgInfoName = subOrgInfo.Name;
                paymentRequest.OperOrgInfo = subOrgInfo;
                paymentRequest.RequestType = requestTypeCBox.SelectedItem.ToString();
                paymentRequest.CurrentPlanGether = PresentMonthPlanGatheringAmount;
                paymentRequest.CurrentPlanPay = PresentMonthPlanPaymentAmount;
                paymentRequest.CurrentRealGether = PresentMonthRealPaymentAmount;
                paymentRequest.DocState = DocumentState.Edit;
                paymentRequest.OpgSysCode = subOrgInfo.SysCode;
                paymentRequest.CreateDate = DateTime.Now;
                paymentRequest.CreatePerson = ConstObject.TheLogin.ThePerson;
                paymentRequest.CreatePersonName = ConstObject.TheLogin.ThePerson.Name;
            }
            catch (Exception ex)
            {
                MessageBox.Show("生成数据出错。\n" + ex.Message);
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        private void FillFlexCellData(IList paymentMasterList)
        {
            DisplayFundPayApplyFormation(gdFundPayApply, paymentMasterList);

            if (!paymentRequestAdded)
            {
                mOperate.FinanceMultDataSrv.SavePaymentRequest(paymentRequest);
            }
            paymentRequestAdded = true;
            gdFundPayApply.Cell(2, 12).Text = string.Format("申请单号:{0}", paymentRequest.Code);
            foreach (PaymentMaster payment in paymentMasterList)
            {
                payment.RequestBill = paymentRequest;
            }
            mOperate.FinanceMultDataSrv.SavePaymentMaster(paymentMasterList);
            MessageBox.Show("数据生成成功！");
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("正在生成信息......");
            ObjectQuery objectQuery = new ObjectQuery();

            //业务时间
            objectQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpBeginCreateDate.Value.Date));
            objectQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpEndCreateDate.Value.AddDays(1).Date));

            objectQuery.AddCriterion(Expression.Like("OpgSysCode", StaticMethod.GetSubCompanyOrgInfo().SysCode, MatchMode.Start));
            //objectQuery.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));

            objectQuery.AddCriterion(Expression.IsNull("RequestBill"));

            paymentMasterList = mOperate.FinanceMultDataSrv.GetPaymentMaster(objectQuery);
            if (paymentMasterList == null)
            {
                return;
            }

            FillFlexCellData(paymentMasterList);
        }

        private void submitBtn_Click(object sender, EventArgs e)
        {
            if (paymentRequest != null)
            {
                if (MessageBox.Show("是否提交状态！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    paymentRequest.Temp1 = subOrgInfo.SysCode;
                    paymentRequest.DocState = DocumentState.InAudit;
                    mOperate.FinanceMultDataSrv.UpdatePaymentRequest(paymentRequest);
                    MessageBox.Show("数据提交成功！");
                }
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            if (paymentRequest != null)
            {
                if (MessageBox.Show("是否取消提交！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    paymentRequest.DocState = DocumentState.Edit;
                    mOperate.FinanceMultDataSrv.UpdatePaymentRequest(paymentRequest);
                    foreach (PaymentMaster payment in paymentMasterList)
                    {
                        payment.RequestBill = null;
                    }
                    mOperate.FinanceMultDataSrv.SavePaymentMaster(paymentMasterList);
                    MessageBox.Show("状态已取消！");
                }
            }
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            ObjectQuery objectQuery = new ObjectQuery();

            if (mOperate.FinanceMultDataSrv.GetPaymentRequest(objectQuery) == null)
            {
                return;
            }

            if (MessageBox.Show("是否删除数据！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                mOperate.FinanceMultDataSrv.DeletePaymentRequest(paymentRequest);
                MessageBox.Show("数据删除成功！");
            }
        }
    }
}
