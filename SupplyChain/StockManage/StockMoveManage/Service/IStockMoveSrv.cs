using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using System.Data;
using System.Collections;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Service
{
    /// <summary>
    /// 调拨单接口服务
    /// </summary>
    public interface IStockMoveSrv : IBaseService
    {
        #region 调拨入库方法
        IList GetStockMoveIn(ObjectQuery oq);

        StockMoveIn GetStockMoveInByCode(string code, string special, string projectId);
        StockMoveIn GetStockMoveInByCode(string code, string projectId);
        StockMoveIn GetStockMoveInById(string id);
        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        IList ObjectQuery(Type entityType, ObjectQuery oq);

        /// <summary>
        /// 根据Id查询调拨入库单明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StockMoveInDtl GetStockMoveInDtlById(string id);

        StockMoveIn SaveStockMoveIn(StockMoveIn obj, IList movedDtlList);
        StockMoveIn SaveStockMoveIn1(StockMoveIn obj, IList movedDtlList);
        StockMoveOut SaveStockMoveOut1(StockMoveOut obj);
        bool DeleteStockMoveIn(StockMoveIn obj);
        #endregion

        #region 调拨入库红单方法
        /// <summary>
        /// 查询调拨入库红单
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetStockMoveInRed(ObjectQuery oq);

        /// <summary>
        /// 根据Code查询调拨入库红单
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        StockMoveInRed GetStockMoveInRedByCode(string code, string special, string projectId);
        StockMoveInRed GetStockMoveInRedByCode(string code,string projectId);
        /// <summary>
        /// 根据Id查询调拨入库红单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StockMoveInRed GetStockMoveInRedById(string id);

        /// <summary>
        /// 保存调拨入库红单
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <returns></returns>
        StockMoveInRed SaveStockMoveInRed(StockMoveInRed obj, IList movedDtlList);

        /// <summary>
        /// 删除调拨入库红单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteStockMoveInRed(StockMoveInRed obj);
        #endregion

        #region 调拨出库方法
        IList GetStockMoveOut(ObjectQuery oq);

        StockMoveOut GetStockMoveOutByCode(string code, string special, string projectId);
        StockMoveOut GetStockMoveOutByCode(string code, string projectId);

        StockMoveOut GetStockMoveOutById(string id);

        StockMoveOut SaveStockMoveOut(StockMoveOut obj);

        StockMoveOutDtl GetStockMoveOutDtlById(string id);
        #endregion

        #region 调拨出库红单方法
        IList GetStockMoveOutRed(ObjectQuery oq);

        StockMoveOutRed GetStockMoveOutRedByCode(string code, string special, string projectId);
        StockMoveOutRed GetStockMoveOutRedByCode(string code, string projectId);

        StockMoveOutRed GetStockMoveOutRedById(string id);

        /// <summary>
        /// 保存出库红单
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <returns></returns>
        StockMoveOutRed SaveStockMoveOutRed(StockMoveOutRed obj, IList movedDtlList);

        /// <summary>
        /// 删除调拨出库红单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteStockMoveOutRed(StockMoveOutRed obj);
        #endregion
        DataSet DepartQuery(string condition);
    }
}
