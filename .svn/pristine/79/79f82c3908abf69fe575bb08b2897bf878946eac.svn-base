using System;
namespace Application.Business.Erp.SupplyChain.StockManage.Stock.Service
{
    public interface IStockQuantitySrv
    {
        System.Collections.IList GetStkMngState(VirtualMachine.Core.ObjectQuery oq);
        System.Collections.IList GetStkMngStateByMaterial(Application.Resource.MaterialResource.Domain.Material theMaterial);
        System.Collections.IList GetStkMngStateByMaterial(string id);
        System.Collections.IList GetStkMngStateUsableQuantity(VirtualMachine.Core.ObjectQuery oq);
        Application.Business.Erp.SupplyChain.StockManage.Stock.Domain.StockQuantity UpdateStkMngStateUsableQuantity(string staCat, string mngStaId, decimal quantity);
    }
}
