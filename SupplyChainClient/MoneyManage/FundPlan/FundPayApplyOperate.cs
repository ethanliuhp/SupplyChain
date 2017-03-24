using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinControls.Controls;
using System.Collections;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan
{
    class FundPayApplyOperate
    {
        public static void DisplayFundPayApplyFormation(PaymentMaster payment, CustomFlexGrid gdFundPayApply)
        {
            int startRowIndex = 5;
            int startColIndex = 1;
            int sequenceNum = 1;
            MFinanceMultData mOperate = new MFinanceMultData();
            ProjectFundPlanDetail selectPlan = payment.FundPlan;

            #region 收款单位名称
            DataDomain selectSupplier = new DataDomain();
            if (string.IsNullOrEmpty(payment.TheSupplierRelationInfo))
            {
                gdFundPayApply.Cell(startRowIndex, startColIndex).Text = payment.TheCustomerName;
            }
            else
            {
                gdFundPayApply.Cell(startRowIndex, startColIndex).Text = payment.TheSupplierName;
            }
            startColIndex++;
            #endregion

            startColIndex++;
            gdFundPayApply.Cell(startRowIndex, startColIndex).Text = payment.SumMoney.ToString();
            startColIndex++;
            gdFundPayApply.Cell(startRowIndex, startColIndex).Text = payment.BankAccountNo;
            startColIndex++;
            //省市
            if (mOperate.FinanceMultDataSrv.GetProjectInfoById(payment.ProjectId) != null)
            {
                CurrentProjectInfo proInfo = mOperate.FinanceMultDataSrv.GetProjectInfoById(payment.ProjectId);
                gdFundPayApply.Cell(startRowIndex, startColIndex).Text = proInfo.ProjectLocationProvince + proInfo.ProjectLocationCity;
            }
            startColIndex++;
            gdFundPayApply.Cell(startRowIndex, startColIndex).Text = payment.BankName;
            startColIndex++;
            gdFundPayApply.Cell(startRowIndex, startColIndex).Text = payment.AccountTitleName;
            startColIndex++;
            //合同额
            gdFundPayApply.Cell(startRowIndex, startColIndex).Text = Convert.ToDouble(selectPlan.ContractAmount / 10000).ToString("0.00");
            startColIndex++;
            //合同约定比例
            gdFundPayApply.Cell(startRowIndex, startColIndex).Text = selectPlan.ContractPaymentRatio.ToString("N2");
            startColIndex++;
            gdFundPayApply.Cell(startRowIndex, startColIndex).Text = Convert.ToDouble(payment.AddBalMoney / 10000).ToString("0.00");
            startColIndex++;
            //累计付款额
            gdFundPayApply.Cell(startRowIndex, startColIndex).Text = Convert.ToDouble(selectPlan.CumulativePayment / 10000).ToString("0.00");
            startColIndex++;
            //实际付款比例
            gdFundPayApply.Cell(startRowIndex, startColIndex).Text = selectPlan.PaymentRatio.ToString("N2");
            startColIndex++;

            gdFundPayApply.Range(startRowIndex, 3, startRowIndex, 4).Merge();
            sequenceNum++;
            startRowIndex++;
        }
    }
}
