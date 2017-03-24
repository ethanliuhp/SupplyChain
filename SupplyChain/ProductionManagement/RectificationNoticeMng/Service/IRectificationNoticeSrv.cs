using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.ProductionManagement.RectificationNoticeMng.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.RectificationNoticeMng.Service
{
   

    /// <summary>
    /// 整改通知单
    /// </summary>
    public interface IRectificationNoticeSrv : IBaseService
    {

        #region 整改通知单
        /// <summary>
        /// 通过ID查询整改通知单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        RectificationNoticeMaster GetRectificationNoticeById(string id);
        /// <summary>
        /// 通过Code查询整改通知单信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        RectificationNoticeMaster GetRectificationNoticeByCode(string code);
        /// <summary>
        /// 查询整改通知单信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetRectificationNotice(ObjectQuery objeceQuery);
        IList GetRectificationDetail(ObjectQuery objectQuery);
        /// <summary>
        /// 整改通知单查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet RectificationNoticeQuery(string condition);
         /// <summary>
        /// 保存主表信息(用于把日常检查 相应文档带过来)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        RectificationNoticeMaster SaveRectificationNoticeOne(RectificationNoticeMaster obj);
        RectificationNoticeMaster SaveRectificationNotice(RectificationNoticeMaster obj);
        RectificationNoticeMaster SaveRectificationNotice(RectificationNoticeMaster obj, IList list, int i);
         /// <summary>
        /// 提交操作(用于把日常检查 相应文档带过来)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="list"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        RectificationNoticeMaster SaveRectificationNoticeOne(RectificationNoticeMaster obj, IList list, int i);
        RectificationNoticeDetail GetRectificationNoticeDetailById(string RecDtlId);
        bool Delete(IList list);
        InspectionRecord SaveRecord(InspectionRecord obj);
        RectificationNoticeDetail SaveRectificationNotice(RectificationNoticeDetail obj);
        ProfessionInspectionRecordDetail SaveProfession(ProfessionInspectionRecordDetail obj);
        IList ObjectQuery(Type entityType, ObjectQuery oq);
        ProObjectRelaDocument SaveOrUpdate(ProObjectRelaDocument item);
        ProfessionInspectionRecordMaster GetProfessionInspectionRecordById(string id);
        IList GetInspectionRecord(ObjectQuery objectQuery);
        #endregion

        
    }




}
