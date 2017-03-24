using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.StockManage.Base.Service;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.Base.StationCategoryUI
{
    public class MStationCategory
    {
        public static IStationCategorySrv theStationCategorySrv;

        public MStationCategory()
        {
            if (theStationCategorySrv == null)
            {
                theStationCategorySrv = StaticMethod.GetService("StationCategorySrv") as IStationCategorySrv;
            }
        }

        #region Ôö,É¾,¸Ä
        public StationCategory Save(StationCategory obj)
        {
            return theStationCategorySrv.SaveOrUpdate(obj);
        }

        public IList Save(IList lst)
        {
            return theStationCategorySrv.SaveOrUpdate(lst);
        }
        public IList Move(StationCategory fromNode, StationCategory toNode)
        {
            return theStationCategorySrv.Move(fromNode, toNode);
        }

        public bool Delete(StationCategory obj)
        {
            return theStationCategorySrv.Delete(obj);
        }
        public bool Delete(IList lst)
        {
            return theStationCategorySrv.Delete(lst);
        }

        #endregion

        #region ²éÑ¯
        public IList GetStationCategory()
        {
            IList list = new ArrayList();
            list = theStationCategorySrv.GetAllObjects(typeof(StationCategory));
            return list;
        }
        public System.Collections.IList GetComplexUnits(StationCategory StationCategory)
        {
            //return MaterialManager.GetComplexUnits(StationCategory);
            return null;
        }
        public System.Collections.IList GetCommonAttributes(StationCategory StationCategory)
        {
            //return MaterialManager.GetCommonAttributes(StationCategory);
            return null;
        }

        #endregion


        public StationCategory GetStationCategory(string id)
        {
            return theStationCategorySrv.GetStationCategory(typeof(StationCategory), id);
        }
    }
}
