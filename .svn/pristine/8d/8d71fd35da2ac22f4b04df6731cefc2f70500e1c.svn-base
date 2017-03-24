using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireSupply.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyPlanManage.Domain;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.Service
{
    /// <summary>
    /// 采购合同服务
    /// </summary>
    public interface IMaterialHireSupplyOrderSrv : IBaseService
    {
        #region 采购合同
        /// <summary>
        /// 通过ID查询采购合同信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MatHireSupplyOrderMaster GetSupplyOrderById(string id);
        /// <summary>
        /// 通过Code查询采购合同信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        MatHireSupplyOrderMaster GetSupplyOrderByCode(string code);
        /// <summary>
        /// 查询采购合同信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetSupplyOrder(ObjectQuery objeceQuery);
        /// <summary> 
        /// 采购合同查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet SupplyOrderQuery(string condition);
        MatHireSupplyOrderMaster AddSupplyOrder(MatHireSupplyOrderMaster obj, IList movedDtlList);
        MatHireSupplyOrderMaster SaveSupplyOrder(MatHireSupplyOrderMaster obj);
        MatHireSupplyOrderMaster SaveSupplyOrder(MatHireSupplyOrderMaster obj, IList movedDtlList);
        SupplyPlanDetail GetSupplyDetailById(string supplyPlanDtlId);
        DemandMasterPlanDetail GetDemandDetailById(string supplyPlanDtlId);
        bool DeleteSupplyOrder(MatHireSupplyOrderMaster obj);
        Hashtable GetOrderMaterialInfo(string projectId, string supRelId);

        #endregion
        /// <summary>
        /// 删除对象集合
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <returns></returns>
        bool Delete(IList list);
        /// <summary>
        /// 保存或修改工程对象关联文档
        /// </summary>
        /// <param name="item">工程对象关联文档</param>
        /// <returns></returns>
        ProObjectRelaDocument SaveOrUpdate(ProObjectRelaDocument item);
        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        IList ObjectQuery(Type entityType, ObjectQuery oq);

        #region 采购合同(公司)
        /// <summary>
        /// 通过ID查询采购合同信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MatHireSupplyOrderMaster GetSupplyOrderByIdCompany(string id);

        /// <summary>
        /// 通过Code查询采购合同信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        MatHireSupplyOrderMaster GetSupplyOrderByCodeCompany(string code);

        /// <summary>
        /// 采购合同信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetSupplyOrderCompany(ObjectQuery objectQuery);

        /// <summary>
        /// 采购合同信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet SupplyOrderQueryCompany(string condition);

        /// <summary>
        /// 保存采购合同(公司)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        MatHireSupplyOrderMaster SaveSupplyOrderCompany(MatHireSupplyOrderMaster obj);

        /// <summary>
        /// 删除采购合同(公司)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteSupplyOrderCompany(MatHireSupplyOrderMaster obj);
        //SupplyOrderMaster SaveSupplyOrder(SupplyOrderMaster obj);
        #endregion

        ///获取确认单价
        /// </summary>
        /// <param name="sAccountSysCode">所属组织的核算的节点的syscode</param>
        /// <param name="sProjectID">当前项目的ID</param>
        /// <param name="sDiagram">图号</param>
        /// <param name="sMaterialID">物资ID</param>
        /// <returns></returns>
        decimal GetConfirmPrice(string sAccountSysCode, string sProjectID, string sDiagram, string sMaterialID);
        decimal GetConfirmPrice(string sDemandPlanDetailID);

        #region 封存单

        MatHireStockBlockMaster SaveStockBlockMasterOrder(MatHireStockBlockMaster obj);


        MatHireStockBlockMaster GetStockBlockMasterOrderById(string id);

        MatHireStockBlockMaster GetStockBlockMasterOrderCode(string code);

        IList GetStockBlockMasterOrder(ObjectQuery objectQuery);


        IList GetStockBlockMasterOrderDetail(ObjectQuery objectQuery);

        DataSet StockBlockMasterOrderQuery(string condition);

        bool VerifyCurrSupplierOrder(SupplierRelationInfo theSupplier, MatHireOrderMaster Master);
        #endregion
    }

}
