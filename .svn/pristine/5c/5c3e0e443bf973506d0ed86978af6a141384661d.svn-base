using System;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Service;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Service;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Core;
using IRPServiceModel.Services.Common;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    public class MCostMonthAccount
    {
        private static ICostMonthAccountSrv costMonthAccSrv;
        private static IIndirectCostSvr indirectCostSvr;
        private static IPBSTreeSrv model;
        private static ICommonMethodSrv commonMethodSrv;
        
        public ICostMonthAccountSrv CostMonthAccSrv
        {
            get { return costMonthAccSrv; }
            set { costMonthAccSrv = value; }
        }

        public MCostMonthAccount()
        {
            if (costMonthAccSrv == null)
                costMonthAccSrv = StaticMethod.GetService("CostMonthAccountSrv") as ICostMonthAccountSrv;
            if (model == null)
                model = ConstMethod.GetService("PBSTreeSrv") as IPBSTreeSrv;
        }

        public IIndirectCostSvr IndirectCostSvr
        {
            get
            {
                if (indirectCostSvr == null)
                {
                    indirectCostSvr = StaticMethod.GetService("IndirectCostSvr") as IIndirectCostSvr;
                }
                return indirectCostSvr;
            }
        }

        public ICommonMethodSrv CommonMethodSrv
        {
            get
            {
                if (commonMethodSrv == null)
                {
                    commonMethodSrv = StaticMethod.GetService("CommonMethodSrv") as ICommonMethodSrv;
                }
                return commonMethodSrv;
            }
        }

        /// <summary>
        /// 根据选择节点过滤明细信息
        /// </summary>
        /// <param name="priceRatio">价格单位系数,比如万元为10000</param>
        /// <param name="taskList">多选的节点GUID</param>
        public CostMonthAccountBill TransCostMonthAccountBill(CostMonthAccountBill costBill, IList taskGUIDList, decimal priceRatio)
        {
            int pointDigit = 4;
            IList list = new ArrayList();
            foreach (CostMonthAccountDtl dtl in costBill.Details)
            {
                dtl.CurrIncomeTotalPrice = decimal.Round(dtl.CurrIncomeTotalPrice / priceRatio, pointDigit);
                dtl.CurrRealPrice = decimal.Round(dtl.CurrRealPrice / priceRatio, pointDigit);
                dtl.CurrRealTotalPrice = decimal.Round(dtl.CurrRealTotalPrice / priceRatio, pointDigit);
                dtl.CurrResponsiTotalPrice = decimal.Round(dtl.CurrResponsiTotalPrice / priceRatio, pointDigit);
                dtl.SumEarnValue = decimal.Round(dtl.SumEarnValue / priceRatio, pointDigit);
                dtl.SumIncomeTotalPrice = decimal.Round(dtl.SumIncomeTotalPrice / priceRatio, pointDigit);
                dtl.SumRealTotalPrice = decimal.Round(dtl.SumRealTotalPrice / priceRatio, pointDigit);
                dtl.SumResponsiTotalPrice = decimal.Round(dtl.SumResponsiTotalPrice / priceRatio, pointDigit);
                foreach (CostMonthAccDtlConsume dtlConsume in dtl.Details)
                {
                    dtlConsume.CurrIncomeTotalPrice = decimal.Round(dtlConsume.CurrIncomeTotalPrice / priceRatio, pointDigit);
                    dtlConsume.CurrRealConsumePlanTotalPrice = decimal.Round(dtlConsume.CurrRealConsumePlanTotalPrice / priceRatio, pointDigit);
                    dtlConsume.CurrRealConsumePrice = decimal.Round(dtlConsume.CurrRealConsumePrice / priceRatio, pointDigit);
                    dtlConsume.CurrRealConsumeTotalPrice = decimal.Round(dtlConsume.CurrRealConsumeTotalPrice / priceRatio, pointDigit);
                    dtlConsume.CurrResponsiConsumeTotalPrice = decimal.Round(dtlConsume.CurrResponsiConsumeTotalPrice / priceRatio, pointDigit);
                    dtlConsume.SumIncomeTotalPrice = decimal.Round(dtlConsume.SumIncomeTotalPrice / priceRatio, pointDigit);
                    dtlConsume.SumRealConsumePlanTotalPrice = decimal.Round(dtlConsume.SumRealConsumePlanTotalPrice / priceRatio, pointDigit);
                    dtlConsume.SumRealConsumeTotalPrice = decimal.Round(dtlConsume.SumRealConsumeTotalPrice / priceRatio, pointDigit);
                    dtlConsume.SumResponsiConsumeTotalPrice = decimal.Round(dtlConsume.SumResponsiConsumeTotalPrice / priceRatio, pointDigit);
                }
                foreach (string accountTaskGUID in taskGUIDList)
                {
                    if (dtl.AccountTaskNodeSyscode.Contains(accountTaskGUID))
                    {
                        list.Add(dtl);
                        break;
                    }
                }
            }
            costBill.Details.Clear();
            foreach (CostMonthAccountDtl dtl in list)
            {
                costBill.Details.Add(dtl);
            }
            return costBill;
        }

        public CostMonthAccountBill TransCostMonthAccountBillNew(CostMonthAccountBill costBill, IList taskGUIDList, decimal priceRatio)
        {
            int pointDigit = 4;
            IList list = new ArrayList();
            foreach (CostMonthAccountDtl dtl in costBill.Details)
            {
                var nodeSyscode = dtl.AccountTaskNodeSyscode;
                foreach (CostMonthAccDtlConsume dtlConsume in dtl.Details)
                {
                    if(string.IsNullOrEmpty(nodeSyscode))
                    {
                        nodeSyscode = dtlConsume.AccountTaskNodeSyscode;
                    }
                    dtlConsume.CurrRealConsumeQuantity = decimal.Round(dtlConsume.CurrRealConsumeQuantity / priceRatio, pointDigit);
                    dtlConsume.CurrRealConsumeTotalPrice = decimal.Round(dtlConsume.CurrRealConsumeTotalPrice / priceRatio, pointDigit);
                    dtlConsume.CurrRealConsumePlanQuantity = decimal.Round(dtlConsume.CurrRealConsumePlanQuantity / priceRatio, pointDigit);
                    dtlConsume.CurrRealConsumePlanTotalPrice = decimal.Round(dtlConsume.CurrRealConsumePlanTotalPrice / priceRatio, pointDigit);
                    dtlConsume.CurrIncomeQuantity = decimal.Round(dtlConsume.CurrIncomeQuantity / priceRatio, pointDigit);
                    dtlConsume.CurrIncomeTotalPrice = decimal.Round(dtlConsume.CurrIncomeTotalPrice / priceRatio, pointDigit);
                    dtlConsume.CurrResponsiConsumeQuantity = decimal.Round(dtlConsume.CurrResponsiConsumeQuantity / priceRatio, pointDigit);
                    dtlConsume.CurrResponsiConsumeTotalPrice = decimal.Round(dtlConsume.CurrResponsiConsumeTotalPrice / priceRatio, pointDigit);
                    dtlConsume.SumRealConsumeQuantity = decimal.Round(dtlConsume.SumRealConsumeQuantity / priceRatio, pointDigit);
                    dtlConsume.SumRealConsumePlanQuantity = decimal.Round(dtlConsume.SumRealConsumePlanQuantity / priceRatio, pointDigit);
                    dtlConsume.SumRealConsumeTotalPrice = decimal.Round(dtlConsume.SumRealConsumeTotalPrice / priceRatio, pointDigit);
                    dtlConsume.SumRealConsumePlanTotalPrice = decimal.Round(dtlConsume.SumRealConsumePlanTotalPrice / priceRatio, pointDigit);
                    dtlConsume.SumIncomeQuantity = decimal.Round(dtlConsume.SumIncomeQuantity / priceRatio, pointDigit);
                    dtlConsume.SumIncomeTotalPrice = decimal.Round(dtlConsume.SumIncomeTotalPrice / priceRatio, pointDigit);
                    dtlConsume.SumResponsiConsumeQuantity = decimal.Round(dtlConsume.SumResponsiConsumeQuantity / priceRatio, pointDigit);
                    dtlConsume.SumResponsiConsumeTotalPrice = decimal.Round(dtlConsume.SumResponsiConsumeTotalPrice / priceRatio, pointDigit);

                    foreach (string accountTaskGUID in taskGUIDList)
                    {
                        if (!string.IsNullOrEmpty(nodeSyscode) && nodeSyscode.Contains(accountTaskGUID))
                        {
                            list.Add(dtlConsume);
                            break;
                        }
                    }
                }
            }

            foreach (CostMonthAccountDtl dtl in costBill.Details)
            {
                dtl.Details.Clear();
                foreach (CostMonthAccDtlConsume consume in list)
                {
                    if (consume.TheAccountDetail.Id == dtl.Id)
                    {
                        dtl.Details.Add(consume);
                    }
                }
            }

            return costBill;
        }

        //合并多个月度成本核算信息
        public CostMonthAccountBill CombCostMonthAccountBill(IList billList)
        {
            CostMonthAccountBill costBill = new CostMonthAccountBill();
            foreach (CostMonthAccountBill bill in billList)
            {
                if (costBill.Id == null)
                {
                    costBill = bill;
                }
                else {
                    foreach (CostMonthAccountDtl dtl in bill.Details)
                    {
                        dtl.TheAccountBill = costBill;
                        costBill.Details.Add(dtl);
                    }
                }
            }
            return costBill;
        }

        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return model.ObjectQuery(entityType, oq);
        }
    }
}
