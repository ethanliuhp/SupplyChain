using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using System.Collections;
using VirtualMachine.Core;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using System.Data;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockBalManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Util;

namespace Application.Business.Erp.SupplyChain.StockManage.StockInManage.Service
{
    /// <summary>
    /// 入库单服务
    /// </summary>
    public interface IStockInSrv : IBaseService
    {
        //IDao Dao { get; set; }

        void UpdateBillPrintTimes(int billType, string billId);
        int QueryBillPrintTimes(int billType, string billId);

        #region 收料单方法
        /// <summary>
        /// 根据条件统计入库单
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>查询结果</returns>
        DataSet StockInStateSearch(string condition);
        IList ObjectQuery(Type entityType, ObjectQuery oq);

        /// <summary>
        /// 查询入库单
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetStockIn(ObjectQuery objectQuery);
        /// <summary>
        /// 验收结算单查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetStockInBal(ObjectQuery objectQuery);

        /// <summary>
        /// 通过Code查询入库单
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        StockIn GetStockInByCode(string Code, string special, string projectId);
        StockIn GetStockInByCode(string Code,  string projectId);
        /// <summary>
        /// 通过ID查询入库单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StockIn GetStockInById(string id);

        string GetStockInID(string id);
        DateTime GetServerDateTime();
        bool CheckServerDateTime();

        StockInDtl GetStockInDtl(string stockInDtlId);

        /// <summary>
        /// 查询收料单明细
        /// </summary>
        /// <param name="stockInBalDetail">验收结算单明细</param>
        /// <returns></returns>
        IList GetStockInDtlLst(StockInBalDetail stockInBalDetail);

        /// <summary>
        /// 查询收料单明细
        /// </summary>
        /// <param name="stockInBalDetailLst">验收结算单明细集合</param>
        /// <returns></returns>
        IList GetStockInDtlLst(List<StockInBalDetail> stockInBalDetailLst);

        /// <summary>
        /// 查询入库单明细
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetStockInDtl(ObjectQuery oq);

        /// <summary>
        /// 入库查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataSet StockInQuery(string condition);

        /// <summary>
        /// 查询入库冲红时序
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetStockInDtlSeq(ObjectQuery oq);

        /// <summary>
        /// 删除收料入库单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteStockIn(StockIn obj);

        /// <summary>
        /// 保存收料入库单
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <returns></returns>
        StockIn SaveStockIn(StockIn obj, IList movedDtlList);
        /// <summary>
        /// 删除关联入库单的验收结算单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
         bool DeleteStockInBalMaster(StockIn obj);
        #endregion

        #region 收料入库单红单方法
        /// <summary>
        /// 查询入库红单
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetStockInRed(ObjectQuery oq);

        /// <summary>
        /// 根据Id查询入库红单
        /// </summary>
        /// <param name="stockInRedId"></param>
        /// <returns></returns>
        StockInRed GetStockInRedById(string stockInRedId);

        /// <summary>
        /// 根据Code查询入库红单
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        StockInRed GetStockInRedByCode(string code, string special, string projectId);
        StockInRed GetStockInRedByCode(string code, string projectId);
        /// <summary>
        /// 保存收料入库红单
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <returns></returns>
        StockInRed SaveStockInRed(StockInRed obj, IList movedDtlList);

        /// <summary>
        /// 删除收料入库红单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteStockInRed(StockInRed obj);
        #endregion

        #region 入库记账方法
        /// <summary>
        /// 入库记账
        /// </summary>
        /// <param name="hashLst">key StockIn;value id List;key StockInRed;value id List;</param>
        /// <param name="hashCode">key StockIn;value Code List;key StockInRed;value Code List;</param>
        /// <param name="year">记账年</param>
        /// <param name="month">记账月</param>
        /// <param name="tallyPersonId">记账人ID</param>
        /// <param name="tallyPersonName">记账人名称</param>
        /// <param name="projectId">项目</param>
        /// <returns></returns>
        Hashtable TallyStockIn(Hashtable hashLst, Hashtable hashCode, int year, int month, string tallyPersonId, string tallyPersonName, string projectId);
        #endregion
        /// <summary>
        /// 根据验收结算单查找该单据下补差单数量
        /// </summary>
        /// <param name="oStockInBalMaster"></param>
        /// <returns></returns>
        Hashtable GetDiffMonthAdjustByStockInBal(IList lstStockInBalDtlID);

        #region 验收结算单方法
        /// <summary>
        /// 查询验收结算单
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetStockInBalMaster(ObjectQuery oq);
        Hashtable GetStockInBal(ObjectQuery oq, CurrentProjectInfo ProjectInfo);

        Hashtable GetStockInBal(string materialCode, DateTime beginDate, DateTime endDate, CurrentProjectInfo ProjectInfo);

        IList GetStockInDtlByBal(ObjectQuery oq, string projectId, string supplierId);

        /// <summary>
        /// 通过Code查询验收结算单
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        StockInBalMaster GetStockInBalMasterByCode(string code, string projectId);

        /// <summary>
        /// 通过id查询验收结算单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StockInBalMaster GetStockInBalMasterById(string id);

        /// <summary>
        /// 保存验收结算单
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <returns></returns>
        StockInBalMaster SaveStockInBalMaster(StockInBalMaster obj, IList movedDtlList);

        /// <summary>
        /// 验收结算汇总时的保存
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <returns></returns>
        StockInBalMaster SaveStockInBalMaster2(StockInBalMaster obj, IList movedDtlList);

        /// <summary>
        /// 删除验收结算单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteStockInBalMaster(StockInBalMaster obj);

        /// <summary>
        /// 验收结算单查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataSet StockInBalQuery(string condition);

        /// <summary>
        /// 根据ID查询验收结算单明细
        /// </summary>
        /// <param name="detailId"></param>
        /// <returns></returns>
        StockInBalDetail GetStockInBalDetail(string detailId);

        /// <summary>
        /// 提交验收结算单
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <returns></returns>
        StockInBalMaster SubmitStockInBalMaster(StockInBalMaster obj, IList movedDtlList);

        /// <summary>
        /// 提交验收结算单 汇总后方法
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <returns></returns>
        StockInBalMaster SubmitStockInBalMaster2(StockInBalMaster obj, IList movedDtlList);
        #endregion

        #region 验收结算单红单方法
        /// <summary>
        /// 查询验收结算单红单
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetStockInBalRedMaster(ObjectQuery oq);

        /// <summary>
        /// 根据Code查询验收结算单红单
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        StockInBalRedMaster GetStockInBalRedMasterByCode(string code, string projectId);

        /// <summary>
        /// 根据ID查询验收结算单红单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StockInBalRedMaster GetStockInBalRedMasterById(string id);

        /// <summary>
        /// 保存验收结算单红单
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <returns></returns>
        StockInBalRedMaster SaveStockInBalRedMaster(StockInBalRedMaster obj, IList movedDtlList);

        /// <summary>
        /// 删除验收结算单红单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteStockInBalRedMaster(StockInBalRedMaster obj);
        #endregion

        #region 基础数据方法
        /// <summary>
        /// 查询基础数据
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetBasicData(ObjectQuery objectQuery);

        /// <summary>
        /// 通过基础数据名称查询基础数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IList GetBasicDataByParentName(string name);

        /// <summary>
        /// 通过基础数据名称和项目查询基础数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IList GetBasicDataByParentNameAndProjectName(string name,string projectName);

        /// <summary>
        /// 通过SQL删除基础数据
        /// </summary>
        /// <param name="model"></param>
        void DelBasicDataBySql(BasicDataOptr model);

        /// <summary>
        /// 保存基础数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        BasicDataOptr SaveBasicData(BasicDataOptr obj);
        #endregion

        #region 日志操作
        IList GetLogStatReport(string condition);
        IList GetLogData(string condition);
        /// <summary>
        /// 插入日志
        /// </summary>
        bool SaveLogData(LogData model);

        /// <summary>
        /// 插入日志 参数传入顺序为 BillId;OperType;Code;OperPerson;BillType;Descript
        /// </summary>
        /// <param name="args">参数传入顺序为 BillId;OperType;Code;OperPerson;BillType;Descript</param>
        /// <returns></returns>
        bool SaveLogData(params object[] args);
        #endregion

        #region 项目信息
        /// <summary>
        /// 查找项目信息
        /// </summary>
        /// <returns></returns>
        CurrentProjectInfo GetProjectInfo();

        /// <summary>
        /// 查找项目信息
        /// </summary>
        /// <param name="ownerorgsyscode">组织层次码</param>
        /// <returns></returns>
        CurrentProjectInfo GetProjectInfo(string ownerorgsyscode);
        OperationOrgInfo GetSubCompanyOrgInfo(string ownerorgsyscode);
        /// <summary>
        /// 根据ID查找项目
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <returns></returns>
        CurrentProjectInfo GetProjectInfoById(string projectId);

        /// <summary>
        /// 保存工程项目信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        CurrentProjectInfo SaveCurrentProjectInfo(CurrentProjectInfo obj);
        /// <summary>
        /// 删除工程项目信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteCurrentProjectInfo(CurrentProjectInfo obj);

        /// <summary>
        ///判断是否使用SQL Server数据库
        /// </summary>
        /// <returns></returns>
        bool IsUseSQLServer();
        #endregion

        #region 读取flx模板方法
        bool IfExistFileInServer(string fileName);

        byte[] ReadTempletByServer(string fileName);
        #endregion

        #region 查询仓库
        /// <summary>
        /// 查询仓库
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        StationCategory GetStationCategory(string projectId);

        string GetIRPAddress();
        #endregion

        #region 日常需求计划、合同方法
        /// <summary>
        /// 查询日常需求计划
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetDailyPLanMaster(ObjectQuery oq);

        /// <summary>
        /// 根据Id查询日常需求计划明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DailyPlanDetail GetDailyPlanDetailById(string id);

        /// <summary>
        /// 查询采购合同主表
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetSupplyOrderMaster(ObjectQuery oq);
        #endregion

        object GetObjectById(Type type, string id);
        CurrentProjectInfo GetProjectInfo(ObjectQuery oq);

        /// <summary>
        /// 主要针对安装验收结算明细的id 通过入库单明细的id查找验收结算单明细的id
        /// </summary>
        /// <param name="sStockInBalDtlForworkID"></param>
        /// <returns></returns>
        string GetStockInBalDtlID(string sStockInBalDtlForworkID);
        /// <summary>
        /// 主要针对安装验收结算单的id   通过入库单的id查找验收结算单的id
        /// </summary>
        /// <param name="sStockInBalForworkID"></param>
        /// <returns></returns>
        DataSet  GetStockInBalID(string sStockInBalForworkID);
       /// <summary>
       /// 判断CreateDate时间的单据是否已经结帐
       /// </summary>
       /// <param name="CreateDate">单据时间</param>
       /// <param name="sProjectID">项目id</param>
       /// <returns></returns>
        string CheckAccounted(OperationOrgInfo oAccountOrg,DateTime CreateDate,   string sProjectID);

        string CheckBusinessDate(OperationOrgInfo oAccountOrg, DateTime CreateDate, IList materialList, string sProjectID);

        /// <summary>
        /// z针对结算单调价后做的结算红单冲红
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        StockInRed SaveStockInRed(StockInRed obj);
         /// <summary>
        /// 提交调价单
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <param name="lstStock">存放顺序：入库红单  入库蓝单  出库调价单  入库调价单</param>
        /// <returns></returns>
   
        StockInBalMaster SubmitStockInBalMaster3(StockInBalMaster obj, IList movedDtlList, IList lstStock);
        void SubmitStockInBalMasterByAudit(StockInBalMaster stockInBalMaster);
     /// <summary>
     /// 记账
     /// </summary>
        /// <param name="lstStock">存放顺序：入库红单  入库蓝单  出库调价单  入库调价单</param>
     /// <returns></returns>
        string TallyList(IList lstStock, StockInBalMaster oStockInBalMaster, PersonInfo oPersonInfo);
        /// <summary>
        /// 查找会计年
        /// </summary>
        /// <returns></returns>
        IList GetFiscalYear();
        /// <summary>
        /// 查询成本分析
        /// </summary>
        /// <param name="sProfessionCategory"></param>
        /// <param name="sProjectId"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonth"></param>
        /// <returns></returns>
         IList  QueryStockCost( string  sProjectId, int   iYear,int  iMonth); 
        /// <summary>
        /// 获取物资盘点耗用信息
        /// </summary>
        /// <param name="sProjectID"></param>
        /// <param name="sUserPartID"></param>
        /// <param name="sUserRand"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonth"></param>
        /// <returns></returns>
         DataTable QueryStoreInventory(string sProjectID, string sUserPartID,string sAccountTaskSysCode, string sUserRand,string sProfessionCat, int iYear, int iMonth);
        /// <summary>
        /// 获取工程核算明细信息
        /// </summary>
        /// <param name="sProjectID"></param>
        /// <param name="sUserPartID"></param>
        /// <param name="sUserRand"></param>
        /// <param name="sProfessionCat"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonth"></param>
        /// <returns></returns>
         DataTable QueryStoreBalance(string sProjectID, string sUserPartID, string sUserRand,  int iYear, int iMonth);
        /// <summary>
        /// 查询物资台帐
        /// </summary>
        /// <param name="sProjectID"></param>
        /// <param name="sStartDate"></param>
        /// <param name="sEndDate"></param>
        /// <returns></returns>
         DataTable QueryMaterialAccount(string sProjectID, string sStartDate, string sEndDate);


          /// <summary>
        /// 根据业务时间计算出会计年月
        /// </summary>
        /// <param name="CreateDate"></param>
        /// <returns></returns>
         DataTable GetFiscaDate(DateTime CreateDate);

         /// <summary>
         /// 根据 项目名称  单据ID获取没有记账的单据
         /// </summary>
         /// <param name="sProjectName"></param>
         /// <param name="sCode"></param>
         /// <returns></returns>
         DataSet QueryListStockInNotHS(string sProjectName, string sCode);
         DataSet QueryListStockInBalNotHS(string sProjectName, string sCode);
         bool UpdateStockInNotHS(string sID, DateTime Time, string sDescript, int iYear, int iMonth);
         bool UpdateStockInBalNotHS( string  sID,  DateTime  Time,  string sDescript,  int iYear, int  iMonth);
         decimal GetRemainQty(string sDailyPlanDetialID);

        /// <summary>
        /// 过磅单查询
        /// </summary>
        /// <param name="projectCode"></param>
        /// <param name="dStart"></param>
        /// <param name="dEnd"></param>
        /// <returns></returns>
         IList<WeightBillDetail> QueryWeightBill(string projectCode, DateTime dStart, DateTime dEnd, string sSupplyCode, string sMaterialCode);

    }
}
