using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialCollectionMng.Domain;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalLedgerMng.Domain;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialReturnMng.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialBalanceMng.Domain;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalSettlementMng.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalContractMng.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialManage.Service
{
    /// <summary>
    /// 料具租赁管理服务
    /// </summary>
    public interface IMatMngSrv : IBaseService
    {
        #region 料具租赁合同
        /// <summary>
        /// 通过ID查询料具租赁合同
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MaterialRentalOrderMaster GetMaterialRentalOrderById(string id);

        /// <summary>
        /// 通过Code查询料具租赁合同
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        MaterialRentalOrderMaster GetMaterialRentalOrderByCode(string code);

        /// <summary>
        /// 查询料具租赁合同
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetMaterialRentalOrder(ObjectQuery objectQuery);
        IList GetMaterialRentalOrderDetail(ObjectQuery objectQuery);
        /// <summary>
        /// 料具租赁合同查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet MaterialRentalOrderQuery(string condition);

        MaterialRentalOrderMaster SaveMaterialRentalOrderMaster(MaterialRentalOrderMaster obj);

        bool VerifyCurrSupplierOrder(SupplierRelationInfo theSupplier, MaterialRentalOrderMaster Master);
        #endregion

        #region 料具收料方法

        MaterialCollectionMaster SaveMaterialCollectionMaster(MaterialCollectionMaster obj);

        MaterialCollectionMaster UpdateMaterialCollectionMaster(MaterialCollectionMaster obj, IList list_DeleteMatCollDtl);

        bool DeleteMaterialCollectionMaster(MaterialCollectionMaster obj);

        MaterialCollectionMaster GetMaterialCollectionMasterById(string id);

        MaterialCollectionMaster GetMaterialCollectionMasterByCode(string code);

        IList GetMaterialCollectionMaster(ObjectQuery objectQuery);

        DataSet MaterialCollectionMasterQuery(string condition);

        #endregion

        #region 料具退料方法
        MaterialReturnMaster GetMaterialReturnById(string id);

        MaterialReturnMaster GetMaterialReturnByCode(string code);

        IList GetMaterialReturnMaster(ObjectQuery objectQuery);
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        MaterialReturnMaster SaveMaterialReturnMaster(MaterialReturnMaster obj);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        MaterialReturnMaster UpdateMaterialReturnMaster(MaterialReturnMaster obj);

        bool DeleteMaterialReturn(MaterialReturnMaster obj);

        DataSet GetMaterialReturnMasterQuery(string condition);
        DataDomain VerifyReturnMatKC(MaterialReturnMaster Master, bool CD);
        bool VerifyReturnMatBusinessDate(DateTime BusinessDate, string projectId);
        bool VerifyCollMatBusinessDate(DateTime BusinessDate, string projectId);

        #endregion

        #region 料具租赁台账
        MaterialRentalLedger SaveMaterialRentalLedger(MaterialRentalLedger obj);
        MaterialRentalLedger GetMatRentalLedgerByMatReturnCollDtlId(string id);
        IList GetMatRentalLedgerByMatReturnCollId(string id);
        IList GetMaterialRentalLedger(ObjectQuery objectQuery);
        MaterialRentalLedger GetMaterialLedgerMasterById(string id);
        DataSet GetMaterialRentalLedgerByCondition(string condition);
        IList GetMaterialRentalLedger(string condition, DateTime BeginDate, string projectId);

        /// <summary>
        /// 根据退料冲减收料的剩余数量
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="rank"></param>
        /// <returns></returns>
        IList GetMatLeftQuantityByNew(MaterialReturnDetail detail, SupplierRelationInfo Supplier, SupplierRelationInfo rank,string projectId);

        IList GetMatLeftQuantityByModify(MaterialReturnDetail detail, SupplierRelationInfo Supplier, SupplierRelationInfo rank,string projectId);
        /// <summary>
        /// 删除退料单时写回台账剩余数量
        /// </summary>
        /// <param name="list"></param>
        void UpdateMatRenLedLeftQuantityByDel(IList list);

        decimal GetMatStockQty(SupplierRelationInfo theSupplier, SupplierRelationInfo theRank, Material material, string projectId);
        #endregion

        #region 料具结算方法
        MaterialBalanceMaster GetMatBalanceMaster(int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo);
        IList GetMatBalanceMaster(ObjectQuery objectQuery);
        MaterialBalanceMaster GetPrrviousMatBalanceMaster(int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo);

        void MaterialReckoning(DateTime OperEndDate, int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier,
            CurrentProjectInfo ProjectInfo, decimal otherMoney, GWBSTree task);

        void MaterialUnReckoning(int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo);

        bool CheckIsNotFirstReckoning(SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo);
        bool CheckReckoning(int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo);
        bool CheckUnReckoning(int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo);
        bool CheckReckoningCurrentMonth(int fiscalYear, int fiscalMonth, SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo);
        #endregion

        #region 设备租赁结算单
        void UpdatePrintTimes(int table ,string id);
        int QueryPrintTimes(int table, string id);
        DataSet QueryForMaterialRentalSettlementPrint(string conditon);
        decimal GetAccumulativeTotalMoney(string projectid, string supplier,DateTime lastdate);
        
        /// <summary>
        /// 通过ID查询设备租赁结算信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MaterialRentalSettlementMaster GetMaterialRentalSettlementById(string id);
        /// <summary>
        /// 通过Code查询设备租赁结算信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        MaterialRentalSettlementMaster GetMaterialRentalSettlementByCode(string code);
        /// <summary>
        /// 查询设备租赁结算信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetMaterialRentalSettlement(ObjectQuery objeceQuery);
        /// <summary>
        /// 设备租赁结算查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet MaterialRentalSettlementQuery(string condition);
        MaterialRentalSettlementMaster AddMaterialRentalSettlement(MaterialRentalSettlementMaster obj);
        MaterialRentalSettlementMaster SaveMaterialRentalSettlement(MaterialRentalSettlementMaster obj);

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


        #region 机械租赁合同
        /// <summary>
        /// 通过ID查询机械租赁合同信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MaterialRentalContractMaster GetMaterialRentalContractById(string id);

        MaterialRentalContractMaster SaveMaterialRentalContract(MaterialRentalContractMaster obj);

        /// <summary>
        /// 查询机械租赁合同信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetMaterialRentalContract(ObjectQuery objeceQuery);
        #endregion
    }
}
