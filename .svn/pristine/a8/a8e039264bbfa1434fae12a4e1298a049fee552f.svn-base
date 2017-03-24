using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.AcceptanceInspectionMng.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.RectificationNoticeMng.Domain
{
    /// <summary>
    /// 整改通知明细
    /// </summary>
    [Serializable]
    public class RectificationNoticeDetail : BaseDetail
    {
        private string questionState;
        private string forwordCode;
        private string forwordContent;
        private InspectionRecord forwordInsLot;
        private string problemCode;
        private AcceptanceInspection accepIns;
        private DateTime requiredDate = StringUtil.StrToDateTime("1900-01-01");
        private string dangerPart;
        private string dangerLevel;
        private string dangerType;
        private string rectContent;
        private int rectConclusion;
        private DateTime rectDate;
        private DateTime rectSendDate;
        private string rectrequired;
        private ProfessionInspectionRecordDetail professionDetail;
        private int isCreated = 0;

        virtual public int IsCreated
        {
            get { return isCreated; }
            set { isCreated = value; }
        }

       
        /// <summary>
        /// 问题说明
        /// </summary>
        virtual public string QuestionState
        {
            get { return questionState; }
            set { questionState = value; }
        }
        /// <summary>
        /// 前驱单据号
        /// </summary>
        virtual public string ForwordCode
        {
            get { return forwordCode; }
            set { forwordCode = value; }
        }
        /// <summary>
        /// 前驱检查内容说明
        /// </summary>
        virtual public string ForwordContent
        {
            get { return forwordContent; }
            set { forwordContent = value; }
        }
        /// <summary>
        /// 日常检查记录
        /// </summary>
        virtual public InspectionRecord ForwordInsLot
        {
            get { return forwordInsLot; }
            set { forwordInsLot = value; }
        }
        /// <summary>
        /// 问题代码
        /// </summary>
        virtual public string ProblemCode
        {
            get { return problemCode; }
            set { problemCode = value; }
        }
        /// <summary>
        /// 验收检查记录
        /// </summary>
        virtual public AcceptanceInspection AccepIns
        {
            get { return accepIns; }
            set { accepIns = value; }
        }
            /// <summary>
        /// 要求整改时间
        /// </summary>
        virtual public DateTime RequiredDate
        {
            get { return requiredDate; }
            set { requiredDate = value; }
        }
        /// <summary>
        /// 隐患部位
        /// </summary>
        virtual public string DangerPart
        {
            get { return dangerPart; }
            set { dangerPart = value; }
        }
        /// <summary>
        /// 隐患级别
        /// </summary>
        virtual public string DangerLevel
        {
            get { return dangerLevel; }
            set { dangerLevel = value; }
        }
        /// <summary>
        /// 隐患类型
        /// </summary>
        virtual public string DangerType
        {
            get { return dangerType; }
            set { dangerType = value; }
        }
        /// <summary>
        /// 整改措施和整改说明
        /// </summary>
        virtual public string RectContent
        {
            get { return rectContent; }
            set { rectContent = value; }
        }
        /// <summary>
        /// 整改结论
        /// </summary>
        virtual public int RectConclusion
        {
            get { return rectConclusion; }
            set { rectConclusion = value; }
        }
        /// <summary>
        /// 整改结论的时间
        /// </summary>
        virtual public DateTime RectDate
        {
            get { return rectDate; }
            set { rectDate = value; }
        }
        /// <summary>
        /// 整改下发时间
        /// </summary>
        virtual public DateTime RectSendDate
        {
            get { return rectSendDate; }
            set { rectSendDate = value; }
        }
        /// <summary>
        /// 整改要求
        /// </summary>
        virtual public string Rectrequired
        {
            get { return rectrequired; }
            set { rectrequired = value; }
        }
        /// <summary>
        /// 专业检查记录明细
        /// </summary>
        virtual public ProfessionInspectionRecordDetail ProfessionDetail
        {
            get { return professionDetail; }
            set { professionDetail = value; }
        }
          
    }
}
