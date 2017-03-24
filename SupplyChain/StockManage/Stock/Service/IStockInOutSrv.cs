using System;
using System.Collections;
using VirtualMachine.Core;
using System.Data;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
namespace Application.Business.Erp.SupplyChain.StockManage.Stock.Service
{
    public interface IStockInOutSrv
    {
        /// <summary>
        /// 结帐
        /// </summary>
        /// <param name="fiscalYear">会计年</param>
        /// <param name="fiscalMonth">会计月</param>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        bool Reckoning(StockInOut oStockInOut, CurrentProjectInfo projectInfo, PersonInfo oPersonInfo);

        /// <summary>
        /// 反结帐
        /// </summary>
        /// <param name="fiscalYear">会计年</param>
        /// <param name="fiscalMonth">会计月</param>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        bool UnReckoning(StockInOut oStockInOut);
        /// <summary>
        /// 检查指定的会计期是否结帐
        /// </summary>
        /// <param name="fiscalYear">会计年</param>
        /// <param name="fiscalMonth">会计月</param>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>        
        bool CheckReckoning(int fiscalYear, int fiscalMonth, string projectId);      
 
        /// <summary>
        /// 检查是否有出入库单未记账
        /// </summary>
        /// <param name="accountYear"></param>
        /// <param name="accountMonth"></param>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        string CheckStockInOutIsTally(int accountYear, int accountMonth, string projectId);

        /// <summary>
        /// 检查是否有收料入库单未做验收结算单
        /// </summary>
        /// <param name="accountYear"></param>
        /// <param name="accountMonth"></param>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        string CheckStockInIsBal(int accountYear, int accountMonth, string projectId);

        /// <summary>
        /// 仓库收发存查询
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetStockInOut(ObjectQuery oq);

        /// <summary>
        /// 仓库收发存查询
        /// </summary>
        /// <param name="condition">本期条件</param>
        /// <param name="addCondition">累计条件</param>
        /// <returns></returns>
        DataSet StockInOutQuery(string condition, string addCondition);

        bool CheckIfNewProject(string projectId);

        string GetEndDateByFiscalPeriod(int kjn, int kjy);
        string GetStartDateByFiscalPeriod(int kjn, int kjy);

        #region 物资报表
        /// <summary>
        /// 材料收发存月报表
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="projectId">项目ID</param>
        /// <param name="wzfl">物资分类</param>
        /// <returns></returns>
        DataSet WZBB_clsfcybb(string beginDate, string endDate, string projectId,string wzfl);
        /// <summary>
        /// 材料收发存月报表
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="projectId">项目ID</param>
        /// <param name="lstMaterialCategory">物资集合</param>
        /// <returns></returns>
        DataSet WZBB_clsfcybb(string beginDate, string endDate, string projectId, IList lstMaterialCategory);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="sProjectId"></param>
        /// <param name="sCostSysCode"></param>
        /// <param name="sGWBSSysCode"></param>
        /// <returns></returns>
        DataSet  WZBB_skmwztj(string beginDate,string endDate,string  sProjectId,string  sCostSysCode  ,string sGWBSSysCode );
        /// <summary>
        /// 材料收发存月报表 消耗明细
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectId"></param>
        /// <param name="_GWBSTreeSysCode">使用部位层次码</param>
        /// <param name="wzfl">物资分类</param>
        /// <returns></returns>
        DataSet WZBB_clsfcybb_xhmx(string beginDate, string endDate, string projectId, string _GWBSTreeSysCode,string wzfl);
        DataSet WZBB_clsfcybb_xhmx(string beginDate, string endDate, string projectId, string _GWBSTreeSysCode, IList lstMaterialCategory);
        DataSet WZBB_clsfcybb_xhmx(string beginDate, string endDate, string projectId,   IList lstMaterialCategory,IList lstUsePart);
        DataSet WZBB_clsfcybb_xhmx(string beginDate, string endDate, string projectId, IList lstMaterialCategory);
        /// <summary>
        /// 商品砼收发存月报表
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="projectId">项目ID</param>
        /// <param name="categoryCode"></param>
        /// <param name="supplier"></param>
        /// <returns></returns>
        DataSet WZBB_sptsfcybb(string beginDate, string endDate, string projectId, string categoryCode, SupplierRelationInfo supplier);

        /// <summary>
        /// 商品砼收发存月报表 消耗明细
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectId"></param>
        /// <param name="_GWBSTreeSysCode">使用部位层次码</param>
        /// <param name="categoryCode"></param>
        /// <param name="supplier"></param>
        /// <returns></returns>
        DataSet WZBB_sptsfcybb_xhmx(string beginDate, string endDate, string projectId, string _GWBSTreeSysCode, string categoryCode, SupplierRelationInfo supplier);
        DataSet WZBB_sptsfcybb_xhmx_UserPart(string beginDate, string endDate, string projectId, string categoryCode, SupplierRelationInfo supplier);
        DataSet WZBB_sptsfcybb_xhmx(string beginDate, string endDate, string projectId, string categoryCode, SupplierRelationInfo supplier);
        DataSet WZBB_sptsfcybb_xhmx(string beginDate, string endDate, string projectId, string categoryCode, SupplierRelationInfo supplier, IList lstUsePart);

        /// <summary>
        /// 材料收发存汇总表
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectId"></param>
        /// <param name="isSummary">是否汇总表 true 汇总表</param>
        /// <returns></returns>
        DataSet WZBB_clsfchzb(string beginDate, string endDate, string projectId, bool isSummary);

        /// <summary>
        /// 材料收发存汇总表 消耗明细
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectId"></param>
        /// <param name="_GWBSTreeSysCode"></param>
        /// <param name="isSummary">是否汇总表 true 汇总表</param>
        /// <returns></returns>
        DataSet WZBB_clsfchzb_xhmx(string beginDate, string endDate, string projectId, string _GWBSTreeSysCode, bool isSummary);
        DataSet WZBB_clsfchzb_xhmx(string beginDate, string endDate, string projectId, bool isSummary);
        DataSet WZBB_clsfchzb_xhmx(string beginDate, string endDate, string projectId, IList lstUsePart, bool isSummary);
        
        /// <summary>
        /// 获取材料收发存汇总表 部位信息
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        DataSet WZBB_clsfchzb_UserPart(string beginDate, string endDate, string projectID, bool isSummary);
        /// <summary>
        /// 调拨材料统计表(出库)
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        DataSet WZBB_dbck(string beginDate, string endDate, string projectId);

        /// <summary>
        /// 调拨材料统计表(入库)、甲供材料汇总表
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectId"></param>
        /// <param name="isMaterialProvider">true 甲供</param>
        /// <returns></returns>
        DataSet WZBB_dbrk(string beginDate, string endDate, string projectId, bool isMaterialProvider);

        /// <summary>
        /// 材料收发存汇总表 冲减材料成本
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectId"></param>
        /// <param name="isSummary">是否汇总表 true 汇总表</param>
        /// <returns></returns>
        Hashtable WZBB_cjclcbje(string beginDate, string endDate, string projectId, bool isSummary);


        
        /// <summary>
        /// 材料收发存台帐
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="materailId"></param>
        /// <returns></returns>
        DataSet WZBB_clsfctz(string projectid, string materailId, string beginDate, string endDate);
        #endregion

        #region 料具报表

        /// <summary>
        /// 料具租赁月报
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="fiscalYear"></param>
        /// <param name="fiscalMonth"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        DataSet WZBB_Ljzlyb(string projectId, int fiscalYear, int fiscalMonth, string supplierId);

        /// <summary>
        /// 料具租赁月报 其他费用
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="fiscalYear"></param>
        /// <param name="fiscalMonth"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        DataSet WZBB_Ljzlyb_qtfy(string projectId, int fiscalYear, int fiscalMonth, string supplierId);

        /// <summary>
        /// 料具租赁报表 消耗明细
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="fiscalYear"></param>
        /// <param name="fiscalMonth"></param>
        /// <param name="supplierId"></param>
        /// <param name="usedPartSysCode"></param>
        /// <returns></returns>
        DataSet WZBB_Ljzlyb_xhmx(string projectId, int fiscalYear, int fiscalMonth, string supplierId, string usedPartSysCode);
        #endregion

        /// <summary>
        /// 物资消耗对比表
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="materialCategory"></param>
        /// <returns></returns>
        DataSet WZBB_Wzxhdbb(string projectId, string beginDate, string endDate, MaterialCategory materialCategory);
        /// <summary>
        /// 获取物资消耗结算信息
        /// </summary>
        /// <param name="sProjectID"></param>
        /// <param name="sYear"></param>
        /// <param name="sMonth"></param>
        /// <returns></returns>
        IList WZXH_Query(string sProjectID, int iYear, int iMonth);
        /// <summary>
        /// 是否能结帐
        /// </summary>
        /// <param name="sProjectID"></param>
        /// <param name="sSysCode"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonth"></param>
        /// <returns></returns>
        bool IsAccount(string sProjectID, string sSysCode, int iYear, int iMonth);
         /// <summary>
        /// 除了本月外其他月份结了没
        /// </summary>
        /// <param name="fiscalYear">会计年</param>
        /// <param name="fiscalMonth">会计月</param>
        /// <returns></returns>
        bool CheckIfOtherMonthAcc(string projectId, int iYear, int iMonth);
        /// <summary>
     /// 获取该项目下最早记账的会计年月
     /// </summary>
     /// <param name="sProjectID"></param>
     /// <returns></returns>
        DataTable GetMinAccTimeByProjectID(string sProjectID);
         /// <summary>
        /// 获取该核节点下最近结算记录
        /// </summary>
        /// <param name="sProjectID"></param>
        /// <param name="sSysCode"></param>
        /// <returns></returns>
            DataTable  GetMaxAccTimeBySysCode(string sProjectID, string sSysCode);
        /// <summary>
        /// 判断该结算是否核算 如果true表示结算已经在商务那边核算了 false表示结算在商务那边还没核算了
        /// </summary>
        /// <param name="sStockInOutID">结算ID</param>
        /// <returns></returns>
            bool IsAccounted(string sStockInOutID);
        /// <summary>
        /// 材料收发存月报表 核算科目
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectId"></param>
        /// <param name="lstMaterialCategory"></param>
        /// <returns></returns>
           DataSet WZBB_GetCostAccountSubjectCat(string beginDate, string endDate, string projectId, IList lstMaterialCategory);
           DataSet WZBB_GetCostAccountSubjectSum(string beginDate, string endDate, string projectId, IList lstMaterialCategory);
           DataSet WZBB_GetCostAccountSubjectSum(string beginDate, string endDate, string projectId, IList lstMaterialCategory,IList lstUsePart);
        /// <summary>
           /// 根据部位统计料具退场数量
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="fiscalYear"></param>
        /// <param name="fiscalMonth"></param>
        /// <param name="supplierId"></param>
        /// <param name="lstUsePart"></param>
        /// <returns></returns>
           DataSet WZBB_LjzlybByUsePart(string projectId, int fiscalYear, int fiscalMonth, string supplierId, IList lstUsePart);

      
    }


}
