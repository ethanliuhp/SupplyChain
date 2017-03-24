using System;
using System.Collections;
using VirtualMachine.Core;
namespace Application.Business.Erp.SupplyChain.StockManage.Base.Service
{
    public interface IStationCategorySrv
    {
        bool Delete(Application.Business.Erp.SupplyChain.StockManage.Base.Domain.StationCategory obj);
        bool Delete(System.Collections.IList lst);
        System.Collections.IList GetALLChildNodes(Application.Business.Erp.SupplyChain.StockManage.Base.Domain.StationCategory obj);
        System.Collections.IList GetAllObjects(Type t);
        Application.Business.Erp.SupplyChain.StockManage.Base.Domain.StationCategory GetStationCategory(Type type, string id);
        Application.Business.Erp.SupplyChain.StockManage.Base.Domain.StationCategory GetStationCategory(Application.Business.Erp.SupplyChain.StockManage.Base.Domain.StationCategory matCat);
        System.Collections.IList GetStationCategory(Type type, VirtualMachine.Core.ObjectQuery oq);
        Application.Business.Erp.SupplyChain.StockManage.Base.Domain.StationCategory GetStationCategoryById(string id);
        System.Collections.IList Invalidate(Application.Business.Erp.SupplyChain.StockManage.Base.Domain.StationCategory obj);
        System.Collections.IList Move(Application.Business.Erp.SupplyChain.StockManage.Base.Domain.StationCategory obj, Application.Business.Erp.SupplyChain.StockManage.Base.Domain.StationCategory toObj);
        Application.Business.Erp.SupplyChain.StockManage.Base.Domain.StationCategory SaveOrUpdate(Application.Business.Erp.SupplyChain.StockManage.Base.Domain.StationCategory obj);
        System.Collections.IList SaveOrUpdate(System.Collections.IList lst);
        IList GetObjectsByCondition(Type t, ObjectQuery oq);
    }
}
