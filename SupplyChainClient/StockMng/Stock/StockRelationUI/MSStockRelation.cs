using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
//using Application.Business.Erp.SupplyChain.SaleManage.SaleBudget.Domain;
//using Application.Resource.FinancialResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Service;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Service;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Domain;
using System.Data;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.Stock.StockRelationUI
{
    public class MStockRelation
    {
        public IStockRelationSrv theStockRelationSrv = null;
        private static IStockQuantitySrv theStockQuantitySrv = null;
        public MStockRelation()
        {
            if (theStockRelationSrv == null)
            {
                theStockRelationSrv = StaticMethod.GetService("StockRelationSrv") as IStockRelationSrv;
            }
            if (theStockQuantitySrv == null)
            {
                theStockQuantitySrv = StaticMethod.GetService("StockQuantitySrv") as IStockQuantitySrv;
            }
        }
        public IList GetObject(ObjectQuery oq)
        {
            return null;
        }
        public IList GetAllMaterialCategory()
        {
            return null;
        }
        public IList GetMaterialRelation(MaterialCategory aMaterialCategory)
        {
            return null;
        }
        public IList GetMaterialRelation(MaterialCategory aMaterialCategory, StationCategory aStationCategory)
        {
            return null;
        }
        public IList GetMaterialRelation(StationCategory aStationCategory, SupplierRelationInfo aSupplierRelationInfo)
        {
            return null;
        }


        public IList GetMaterialRelation(StationCategory aStationCategory)
        {
            return null;
        }
        public IList GetManageStateUsableQuantity(ObjectQuery oq)
        {
            return theStockQuantitySrv.GetStkMngStateUsableQuantity(oq);
            //return theStockRelationSrv.GetManageStateUsableQuantity(oq);
        }

        /// <summary>
        /// 获取发布版本号
        /// </summary>
        /// <param name="personInfoId">发布人</param>
        /// <param name="dateStr">日期20100702</param>
        /// <returns></returns>
        public string GetPublishVersion(string personInfoId, string dateStr)
        {
            return null;
        }

        public IList GetMaterialPrice(ObjectQuery oq)
        {
            return null;
        }

        public DataTable GetMaterialRelationAndPrice(Hashtable paras)
        {
            return null;
        }
    }
}