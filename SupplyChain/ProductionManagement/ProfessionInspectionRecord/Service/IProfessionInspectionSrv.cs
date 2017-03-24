using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Service
{
   

    /// <summary>
    /// 专业检查记录
    /// </summary>
    public interface IProfessionInspectionSrv : IBaseService
    {

        #region 专业检查记录
        /// <summary>
        /// 通过ID查询专业检查记录信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ProfessionInspectionRecordMaster GetProfessionInspectionRecordById(string id);
        /// <summary>
        /// 通过Code查询专业检查记录信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        ProfessionInspectionRecordMaster GetProfessionInspectionRecordByCode(string code);
        /// <summary>
        /// 查询专业检查记录信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetProfessionInspectionRecordPlan(ObjectQuery objeceQuery);
        /// <summary>
        /// 专业检查记录查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet ProfessionInspectionRecordQuery(string condition);
        ProfessionInspectionRecordMaster SaveProfessionInspectionRecordPlan(ProfessionInspectionRecordMaster obj);
        IList GetProfessionInspection(ObjectQuery objectQuery);
        ProfessionInspectionRecordDetail GetProfessionInspectionRecordDetailById(string ProDtlId);
        IList ObjectQuery(Type entityType, ObjectQuery oq);
        #endregion

        
    }




}
