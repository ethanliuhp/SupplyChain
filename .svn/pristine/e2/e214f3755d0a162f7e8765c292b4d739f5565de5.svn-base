using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborSporadicManagement.Domain;
using System.Collections;
using System.Data;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.PenaltyDeductionManagement.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.Service
{
   

    /// <summary>
    /// 零星用工派工服务
    /// </summary>
    public interface ILaborSporadicSrv : IBaseService
    {
        #region 零星用工派工
        /// <summary>
        /// 通过ID查询零星用工派工信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        LaborSporadicMaster GetLaborSporadicById(string id);
        /// <summary>
        /// 通过Code查询零星用工派工信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        LaborSporadicMaster GetLaborSporadicByCode(string code);
        /// <summary>
        /// 查询零星用工派工信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetLaborSporadic(ObjectQuery objeceQuery);
        /// <summary>
        /// 零星用工派工查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet LaborSporadicQuery(string condition);
        LaborSporadicMaster SaveLaborSporadic(LaborSporadicMaster obj);
        LaborSporadicMaster SaveLaborSporadic1(LaborSporadicMaster obj);
        LaborSporadicMaster UpdateLaborSporadic(LaborSporadicMaster curBillMaster);
        LaborSporadicMaster SaveOrUpdateLaborSporadic(LaborSporadicMaster curBillMaster, IList lstPenaltyDeductionMaster);
        Material SearchMaterial(string Name);
        IList GetMaterial(ObjectQuery objectQuery);
        IList ObjectQuery(Type entityType, ObjectQuery oq);
        PenaltyDeductionMaster SavePenaltyDeduction(PenaltyDeductionMaster obj);
        DataSet GetAuthedLaborSporadic(string sProjectId);

        LaborSporadicMaster SaveLaborSporadic(LaborSporadicMaster obj, List<LaborSporadicDetail> lstRemoveDetail);
        #endregion

        
    }

}
