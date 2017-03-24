using System;
using System.Collections;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using VirtualMachine.Core;
using System.Collections.Generic;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Domain;
using System.Data;
using VirtualMachine.Core.DataAccess;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
namespace Application.Business.Erp.SupplyChain.StockManage.Stock.Service
{
    public interface IStockRelationSrv : IBaseService
    {
        /// <summary>
        /// 根据资源查询库存
        /// </summary>
        /// <param name="MaterialId"></param>
        /// <returns></returns>
        IList GetStockRelationByMaterial(string MaterialId);

        /// <summary>
        /// 根据资源、专业查询库存
        /// </summary>
        /// <param name="MaterialId"></param>
        /// <param name="special"></param>
        /// <returns></returns>
        IList GetStockRelationByMaterialAndSpecial(string MaterialId, string special, string projectId);

        IList GetStockRelation(ObjectQuery oq);

        IList GetStockRelationByStockInDtlId(string stockInDtlId);

        /// <summary>
        /// 根据入库明细ID查询库存
        /// </summary>
        /// <param name="stockInDtlId"></param>
        /// <returns></returns>
        decimal GetRemainQuantityByStockInDtlId(string stockInDtlId);

        /// <summary>
        /// 库存查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataSet StockRelationQuery(string condition);
        DataSet StockKCQuantity(string condition);
        DataSet StockKCQuantity(int iYear, int iMonth, string sProjectID, string sMaterialCode);
        /// <summary>
        /// 库存汇总查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataSet StockRelationSummary(string condition);
        /// <summary>
        /// 仓库收发台帐
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataSet StockSequenceQuery(string condition);

        /// <summary>
        /// 库存查询(闲置物资设置)
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataSet StockRelationQuery4SetIdleQuantity(string condition);

        /// <summary>
        /// 库存查询(闲置物资查询)
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataSet StockRelationQuery4IdleQuantityQuery(string condition);

        /// <summary>
        /// 设置闲置数量
        /// </summary>
        /// <param name="stockRelationId"></param>
        /// <param name="idleQuantity"></param>
        void SetIdleQuantity(string stockRelationId, decimal idleQuantity);
        ///// <summary>
        ///// 根据入库明细的ID查找对应的关系表中库存大于零的记录
        ///// </summary>
        ///// <param name="sStockInDtlID"></param>
        ///// <returns></returns>
        //IList GetStockRelationByStockInDtlID(string sStockInDtlID);
    }
}
