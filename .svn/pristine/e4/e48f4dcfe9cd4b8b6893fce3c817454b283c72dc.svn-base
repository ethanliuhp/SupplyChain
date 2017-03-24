using System;
namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockProfitIn
{
    public interface IMProfitIn
    {
        bool Delete(Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain.ProfitIn obj);
        System.Collections.IList GetDetailList(VirtualMachine.Core.ObjectQuery oq);
        System.Collections.Generic.IList<VirtualMachine.Patterns.BusinessEssence.Domain.LinkRule> GetForwardTypes();
        VirtualMachine.Patterns.BusinessEssence.Domain.LinkRule GetLinkRule(string id);
        Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain.ProfitIn GetObjectById(string id);
        System.Collections.IList GetObject(VirtualMachine.Core.ObjectQuery oq);
        Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain.ProfitIn GetObject(string code);
        Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain.ProfitIn Save(Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain.ProfitIn obj);
        System.Collections.IDictionary Tally(System.Collections.IDictionary hashlst, System.Collections.IDictionary hashCode);
        Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain.ProfitIn Update(Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain.ProfitIn obj);

        //账面盘盈盘亏
        bool DeleteByAcct(Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain.AcctLoseAndProfit obj);
        Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain.AcctLoseAndProfit SaveByAcct(Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain.AcctLoseAndProfit obj);
        Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain.AcctLoseAndProfit UpdateByAcct(Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain.AcctLoseAndProfit obj);
        Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain.AcctLoseAndProfit GetObjectByAcct(long id);
        System.Collections.IList GetObjectByAcct(VirtualMachine.Core.ObjectQuery oq);
        Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain.AcctLoseAndProfit GetObjectByAcct(string code);
        System.Collections.IList GetDetailListByAcct(VirtualMachine.Core.ObjectQuery oq);

        /// <summary>
        /// 查询是否已经账面结帐
        /// </summary>
        /// <param name="kjn">会计年</param>
        /// <param name="kjy">会计月</param>
        /// <returns></returns>
        bool QueryStockAcct(int kjn, int kjy);

        bool New();
        bool Modify();
        bool EnterForm();
        bool ExitForm();
        bool Print();
        bool Preview();
    }
}
