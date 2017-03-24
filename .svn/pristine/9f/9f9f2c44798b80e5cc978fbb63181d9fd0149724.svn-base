using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain;
using System.Collections;
using VirtualMachine.Core;
using System.Data;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireReturn.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireLedger.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireBalance.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.SettlementManagement.MaterialSettleMng.Domain;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireTransportCost.Domain;


namespace Application.Business.Erp.SupplyChain.MaterialHireMng.Service
{
    public interface IMaterialHireMngSvr : IBaseService
    {
        #region 料具租赁合同
        /// <summary>
        /// 通过ID查询料具租赁合同
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       MatHireOrderMaster GetMaterialHireOrderById(string id);

        /// <summary>
        /// 通过Code查询料具租赁合同
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
       MatHireOrderMaster GetMaterialHireOrderByCode(string code);

        /// <summary>
        /// 查询料具租赁合同
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetMaterialHireOrder(ObjectQuery objectQuery);
        IList GetMaterialHireOrderDetail(ObjectQuery objectQuery);
        /// <summary>
        /// 料具租赁合同查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet MaterialHireOrderQuery(string condition);

       MatHireOrderMaster SaveMaterialHireOrderMaster(MatHireOrderMaster obj);

        bool VerifyCurrSupplierOrder(SupplierRelationInfo theSupplier,MatHireOrderMaster Master);
        #endregion

        #region 料具收料方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="IsReturnUp">收料为负数时：是否允许退超</param>
        /// <returns></returns>
        MatHireCollectionMaster SaveMaterialHireCollectionMaster(MatHireCollectionMaster obj, bool IsReturnUp);

        MatHireCollectionMaster UpdateMaterialHireCollectionMaster(MatHireCollectionMaster obj, IList list_DeleteMatCollDtl);

        bool DeleteMaterialHireCollectionMaster(MatHireCollectionMaster obj);

        MatHireCollectionMaster GetMaterialHireCollectionMasterById(string id);

        MatHireCollectionMaster GetMaterialHireCollectionMasterByCode(string code);

        IList GetMaterialHireCollectionMaster(ObjectQuery objectQuery);
     
        DataSet MaterialHireCollectionMasterQuery(string condition);

        #endregion

        #region 料具退料方法
        MatHireReturnMaster GetMaterialHireReturnById(string id);

        MatHireReturnMaster GetMaterialHireReturnByCode(string code);

        IList GetMaterialHireReturnMaster(ObjectQuery objectQuery);
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        MatHireReturnMaster SaveMaterialHireReturnMaster(MatHireReturnMaster obj,bool CanReturnUp);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        MatHireReturnMaster UpdateMaterialHireReturnMaster(MatHireReturnMaster obj);

        bool DeleteMaterialHireReturn(MatHireReturnMaster obj);

        DataSet GetMaterialHireReturnMasterQuery(string condition);
        DataDomain VerifyReturnMatKC(MatHireReturnMaster Master, bool CD);
        bool VerifyReturnMatBusinessDate(DateTime BusinessDate, string projectId);
        bool VerifyCollMatBusinessDate(DateTime BusinessDate, string projectId);

        #endregion

        #region 料具租赁台账
        MatHireLedgerMaster SaveMaterialHireLedgerMaster(MatHireLedgerMaster obj);
        MatHireLedgerMaster GetMaterialHireLedgerByMatReturnCollDtlId(string id);
        IList GetMaterialHireLedgerByMatReturnCollId(string id);
        IList GetMaterialHireLedgerMaster(ObjectQuery objectQuery);
        MatHireLedgerMaster GetMaterialHireLedgerMasterById(string id);
        DataSet GetMaterialHireLedgerByCondition(string condition);
        IList GetMaterialHireLedgerMaster(string condition, DateTime BeginDate, string projectId);

        /// <summary>
        /// 根据退料冲减收料的剩余数量
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="rank"></param>
        /// <returns></returns>
        IList GetMatLeftQuantityByNew(MatHireReturnDetail detail, SupplierRelationInfo Supplier, SupplierRelationInfo rank, string projectId, EnumMatHireType enumMatHireType);

        IList GetMatLeftQuantityByModify(MatHireReturnDetail detail, SupplierRelationInfo Supplier, SupplierRelationInfo rank, string projectId);
        /// <summary>
        /// 删除退料单时写回台账剩余数量
        /// </summary>
        /// <param name="list"></param>
        void UpdateMatRenLedLeftQuantityByDel(IList list);

        decimal GetMatStockQty(SupplierRelationInfo theSupplier, SupplierRelationInfo theRank, Material material, string projectId, EnumMatHireType enumMatHireType);
        #endregion

        #region 料具结算方法
        MatHireBalanceMaster GetMatBalanceMaster(int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo);
        IList GetMatBalanceMaster(ObjectQuery objectQuery);
        MatHireBalanceMaster GetPrrviousMatBalanceMaster(int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo);

        void MaterialReckoning(DateTime OperEndDate, int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier,
            CurrentProjectInfo ProjectInfo,decimal dChangeMoney);

        void MaterialUnReckoning(int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo);

        bool CheckIsNotFirstReckoning(SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo);
        bool CheckReckoning(int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo);
        bool CheckUnReckoning(int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo);
        bool CheckReckoningCurrentMonth(int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo);
        #endregion

        #region 设备租赁结算单
        void UpdatePrintTimes(int table, string id);
        int QueryPrintTimes(int table, string id);
        DataSet QueryForMaterialHireSettlementPrint(string conditon);
        decimal GetAccumulativeTotalMoney(string projectid, string supplier, DateTime lastdate);

        /// <summary>
        /// 通过ID查询设备租赁结算信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MaterialSettleMaster GetMaterialHireSettlementById(string id);
        /// <summary>
        /// 通过Code查询设备租赁结算信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        MaterialSettleMaster GetMaterialHireSettlementByCode(string code);
        /// <summary>
        /// 查询设备租赁结算信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetMaterialHireSettlement(ObjectQuery objeceQuery);
        /// <summary>
        /// 设备租赁结算查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet MaterialHireSettlementQuery(string condition);
        MaterialSettleMaster AddMaterialHireSettlement(MaterialSettleMaster obj);
        MaterialSettleMaster SaveMaterialHireSettlement(MaterialSettleMaster obj);

        IList GetMaterialSubjectByParentId(string id);
        #endregion
        /// <summary>
        /// 删除对象集合
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <returns></returns>
        [TransManager]
        bool Delete(IList list);
        /// <summary>
        /// 保存或修改收发邀请函
        /// </summary>
        /// <param name="item">收发邀请函</param>
        /// <returns></returns>
        [TransManager]
        ProObjectRelaDocument SaveOrUpdate(ProObjectRelaDocument item);
        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        IList ObjectQuery(Type entityType, ObjectQuery oq);

        void CreateMatSetBalInfoByYearAndMonth(int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo);

        /// <summary>
        /// 判断是否核算 true：商务已经对该结算记录核算了 或者  没有找到该结算记录
        /// false：有结算记录 商务没有核算
        /// </summary>
        /// <param name="fiscalYear">会计年</param>
        /// <param name="fiscalMonth">月</param>
        /// <param name="theSupplier">提供商</param>
        /// <param name="ProjectInfo">项目</param>
        /// <param name="sMsg">错误提示</param>
        /// <returns></returns>
        bool IsAccount(int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo, ref string sMsg);
        /// <summary>
        /// 获取租赁台账报表数据
        /// </summary>
        /// <param name="dStart"></param>
        /// <param name="dEnd"></param>
        /// <param name="sSupplierRelationId"></param>
        /// <param name="sProjectId"></param>
        /// <param name="sMaterialId"></param>
        /// <returns></returns>
        DataSet GetMaterialHireReport(DateTime dStart, DateTime dEnd, string sSupplierRelationId, string sProjectId, string sMaterialId);
      /// <summary>
      /// 在建项目料具使用情况
      /// </summary>
      /// <param name="iYear"></param>
      /// <param name="iMonth"></param>
      /// <returns></returns>
        DataSet GetMaterialHireBuilding(int iYear, int iMonth);

        #region 运输费
        MatHireTranCostMaster SaveMatHireTranCostMaster(MatHireTranCostMaster master);
        bool DeleteMatHireTranCostMaster(MatHireTranCostMaster master);
        MatHireTranCostMaster GetMatHireTranCostMasterById(string sID);
        IList GetMatHireTranCostMaster(ObjectQuery oQuery);
        DataSet QueryMaterialHireTranCost(string condition);
        #endregion
        Hashtable GetPreviousJC(DateTime BeginDate, string sMaterialIds, string projectId, string supplyId);
        IList QueryMatHireBalanceReport(DateTime startTime, DateTime endTime, string sProjectId, string sSupplyId);
        DataSet QueryMaterialSizeReport(DateTime startTime, DateTime endTime, string sContractID, string sMaterialCode);
        DataSet QueryMaterialDistributeReport(DateTime dEnd);
    }
}
