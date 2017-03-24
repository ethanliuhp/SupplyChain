using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatApplyMng.Domain;
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatProcessMng.Domain;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;

namespace Application.Business.Erp.SupplyChain.WasteMaterialManage.Service
{
   

    /// <summary>
    /// 废旧物资申请服务
    /// </summary>
    public interface IWasteMatSrv : IBaseService
    {

        #region 废旧物资申请
        /// <summary>
        /// 通过ID查询废旧物资申请信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WasteMatApplyMaster GetWasteMatApplyById(string id);
        /// <summary>
        /// 通过Code查询废旧物资申请信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        WasteMatApplyMaster GetWasteMatApplyByCode(string code);
        /// <summary>
        /// 查询废旧物资申请信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetWasteMatApply(ObjectQuery objeceQuery);
        /// <summary>
        /// 废旧物资信息申请查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet WasteMatApplyQuery(string condition);
        WasteMatApplyMaster saveWasteMatApply(WasteMatApplyMaster obj);
        #endregion

        /// <summary>
        /// 删除对象集合
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <returns></returns>
        [TransManager]
        bool Delete(IList list);
        /// <summary>
        /// 保存或修改工程对象关联文档
        /// </summary>
        /// <param name="item">工程对象关联文档</param>
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
        #region 废旧物资申请处理
        ///// <summary>
        ///// 通过ID查询废旧物资申请处理信息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //WasteMatProcessMaster GetWasteMatHandleById(string id);
        /// <summary>
        /// 通过Id查询废旧物资申请处理信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        WasteMatProcessMaster GetWasteMatHandleById(string Id);

        /// <summary>
        /// 通过Code查询废旧物资申请处理信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        WasteMatProcessMaster GetWasteMatHandleByCode(string code);
        /// <summary>
        /// 查询废旧物资申请处理信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetWasteMatHandle(ObjectQuery objeceQuery);
        /// <summary>
        /// 废旧物资信息申请处理查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet WasteMatHandleQuery(string condition);
        //WasteMatProcessMaster saveWasteMatHandle(WasteMatProcessMaster obj);
        
        /// <summary>
        /// 保存废旧物资处理信息
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <returns></returns>
        WasteMatProcessMaster saveWasteMatProcess(WasteMatProcessMaster obj, IList movedDtlList);

        /// <summary>
        /// 根据明细Id查询废旧物资申请单明细
        /// </summary>
        /// <param name="wasteProcessDtlId"></param>
        /// <returns></returns>
        WasteMatApplyDetail GetWasteProcessDetailById(string wasteProcessDtlId);


        /// <summary>
        /// 删除废旧物资处理单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteWasteMaterialProcessMaster(WasteMatProcessMaster obj);

        DateTime GetWasterSQCreateDate(string id);

        //IList GetWasteMaterialProcessHandle(ObjectQuery oq);

        #endregion
    }




}
