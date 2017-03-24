using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.StockManage.StockInventory.Domain;
using System.Collections;
using System.Data;

namespace Application.Business.Erp.SupplyChain.StockManage.StockInventory.Service
{
   

    /// <summary>
    /// 月度盘点服务
    /// </summary>
    public interface IStockInventorySrv : IBaseService
    {
        IDao Dao { get; set; }

        #region 月度盘点
        /// <summary>
        /// 通过ID查询月度盘点信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StockInventoryMaster GetStockInventoryById(string id);
        /// <summary>
        /// 通过Code查询月度盘点信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        StockInventoryMaster GetStockInventoryByCode(string code);
        /// <summary>
        /// 查询月度盘点信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetStockInventory(ObjectQuery objeceQuery);
        /// <summary>
        /// 月度盘点查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet StockInventoryQuery(string condition);
        StockInventoryMaster SaveStockInventory(StockInventoryMaster obj, IList movedDtlList);

        /// <summary>
        /// 获取安装的月度盘点报表数据
        /// </summary>
        /// <param name="sProfessionCategory">专业分类</param>
        /// <param name="sProjectId">项目ID</param>
        /// <param name="sUsedRankId">劳务队伍ID</param>
        /// <returns></returns>
        DataTable GetInventoryReport(string sProfessionCategory, string sProjectId, string sUsedRankId, int iYear, int iMonth);
        /// <summary>
        /// 计算出该项目下的该物资出库的 认价均价和合同均价
        /// </summary>
        /// <param name="sMaterialID"></param>
        /// <param name="sProjectID"></param>
        /// <returns></returns>
        DataTable GetAvgPrice(string sMaterialID, string sProjectID);
        DataTable GetSubject(string sMaterialID, string sProjectID);
        #endregion
     
    }




}
