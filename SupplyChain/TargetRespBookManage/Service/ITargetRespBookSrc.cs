using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using System.Collections;
using Application.Business.Erp.SupplyChain.TargetRespBookManage.Domain;
using VirtualMachine.Core;

namespace Application.Business.Erp.SupplyChain.TargetRespBookManage.Service
{
    /// <summary>
    /// 目标责任书服务
    /// </summary>
    public interface ITargetRespBookSrc : IBaseService
    {
        #region 废旧物资申请
        /// <summary>
        /// 通过ID查询目标责任书信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TargetRespBook GetTargetRespBookById(string id);
        /// <summary>
        /// 通过Code查询目标责任书信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        TargetRespBook GetWTargetRespBookByCode(string code);
        /// <summary>
        /// 查询目标责任书信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetTargetRespBook(ObjectQuery objectQuery);
        /// <summary>
        /// 保存目标责任书
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        TargetRespBook saveTargetRespBook(TargetRespBook obj);
        ///// <summary>
        ///// 废旧物资信息申请查询
        ///// </summary>
        ///// <param name="condition"></param>
        ///// <param name="dateBegin"></param>
        ///// <param name="dateEnd"></param>
        ///// <returns></returns>
        //DataSet WasteMatApplyQuery(string condition);
        //WasteMatApplyMaster saveWasteMatApply(WasteMatApplyMaster obj);
        #endregion
    }
}
