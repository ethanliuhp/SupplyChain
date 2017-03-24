using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.ProductionManagement.PenaltyDeductionManagement.Domain;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.Service
{
   

    /// <summary>
    /// 罚扣款单服务
    /// </summary>
    public interface IPenaltyDeductionSrv : IBaseService
    {

        #region 罚扣款单
        /// <summary>
        /// 通过ID查询罚扣款单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PenaltyDeductionMaster GetPenaltyDeductionById(string id);
        /// <summary>
        /// 通过Code查询罚扣款单信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        PenaltyDeductionMaster GetPenaltyDeductionByCode(string code);
        /// <summary>
        /// 查询罚扣款单信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetPenaltyDeduction(ObjectQuery objeceQuery);
        /// <summary>
        /// 罚扣款单查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
    
        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        IList ObjectQuery(Type entityType, ObjectQuery oq);
        bool Delete(IList list);
        /// <summary>
        /// 保存或修改会议管理
        /// </summary>
        /// <param name="item">工程对象关联文档</param>
        /// <returns></returns>
        [TransManager]
        ProObjectRelaDocument SaveOrUpdate(ProObjectRelaDocument item);
        DataSet PenaltyDeductionQuery(string condition);
        PenaltyDeductionMaster SavePenaltyDeduction(PenaltyDeductionMaster obj);
        PenaltyDeductionMaster UpdatePenaltyDeduction(PenaltyDeductionMaster obj);
        #endregion
        #region 多维度报表
        DataSet GetMainCostData(string sProjectId);
        DataSet GetNoMainCostData(string sProjectId, string sOrgSysCode);
        #endregion

    }




}
