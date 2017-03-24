using System;
namespace Application.Business.Erp.SupplyChain.Client.StockManage.Base.StationCategoryUI
{
    public interface IMStationCategory
    {
        bool Delete(Application.Business.Erp.SupplyChain.StockManage.Base.Domain.StationCategory obj);
        bool Delete(System.Collections.IList lst);
        System.Collections.IList GetCommonAttributes(Application.Business.Erp.SupplyChain.StockManage.Base.Domain.StationCategory StationCategory);
        System.Collections.IList GetComplexUnits(Application.Business.Erp.SupplyChain.StockManage.Base.Domain.StationCategory StationCategory);
        Application.Business.Erp.SupplyChain.StockManage.Base.Domain.StationCategory GetStationCategory(string id);
        System.Collections.IList GetStationCategory();
        System.Collections.IList Move(Application.Business.Erp.SupplyChain.StockManage.Base.Domain.StationCategory fromNode, Application.Business.Erp.SupplyChain.StockManage.Base.Domain.StationCategory toNode);
        Application.Business.Erp.SupplyChain.StockManage.Base.Domain.StationCategory Save(Application.Business.Erp.SupplyChain.StockManage.Base.Domain.StationCategory obj);
        System.Collections.IList Save(System.Collections.IList lst);
    }
}
