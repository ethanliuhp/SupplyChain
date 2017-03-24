using System;
namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockLossOut
{
    public interface IMLossOut
    {
        bool Delete(Application.Business.Erp.SupplyChain.StockManage.StockLossOut.Domain.LossOut obj);
        System.Collections.IList GetDetailList(VirtualMachine.Core.ObjectQuery oq);
        System.Collections.Generic.IList<VirtualMachine.Patterns.BusinessEssence.Domain.LinkRule> GetForwardTypes();
        VirtualMachine.Patterns.BusinessEssence.Domain.LinkRule GetLinkRule(string id);
        Application.Business.Erp.SupplyChain.StockManage.StockLossOut.Domain.LossOut GetObjectById(string id);
        System.Collections.IList GetObject(VirtualMachine.Core.ObjectQuery oq);
        Application.Business.Erp.SupplyChain.StockManage.StockLossOut.Domain.LossOut GetObject(string code);
        Application.Business.Erp.SupplyChain.StockManage.StockLossOut.Domain.LossOut Save(Application.Business.Erp.SupplyChain.StockManage.StockLossOut.Domain.LossOut obj);
        System.Collections.IDictionary Tally(System.Collections.IDictionary hashlst, System.Collections.IDictionary hashCode);
        Application.Business.Erp.SupplyChain.StockManage.StockLossOut.Domain.LossOut Update(Application.Business.Erp.SupplyChain.StockManage.StockLossOut.Domain.LossOut obj);
        bool New();
        bool Modify();
        bool EnterForm();
        bool ExitForm();
        bool Print();
        bool Preview();
    }
}
