using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.EngineerManage.TargetRespBookManage.Domain
{
    /// <summary>
    /// 目标节点
    /// </summary>
    [Serializable]
    public class TargetProgressNode : BaseDetail
    {
        //private string id;
        private string unitName;   //计量单位名称
        private StandardUnit unitId;   //价格计量单位
        private string nodeNameId;   //节点名称
        private TargetRespBook targetRespBookGuid;   //目标责任书
        private decimal benefitGoal;   //效益目标
        private string figurativeProgress;   //形象进度
        private string effectiveStatus; //有效状态
        private decimal predictValue;   //预计产值
        private DateTime planCompleteDate;   //预计完成时间

        ///// <summary>
        ///// id
        ///// </summary>
        //virtual public string Id
        //{
        //    get { return id; }
        //    set { id = value; }
        //}

        //private decimal version;

        //virtual public decimal Version
        //{
        //    get { return version; }
        //    set { version = value; }
        //}
        /// <summary>
        /// 计量单位名称
        /// </summary>
        virtual public string UnitName
        {
            get { return unitName; }
            set { unitName = value; }
        }
        /// <summary>
        /// 价格计量单位
        /// </summary>
        virtual public StandardUnit UnitId
        {
            get { return unitId; }
            set { unitId = value; }
        }
        /// <summary>
        /// 节点名称
        /// </summary>
        virtual public string NodeNameId
        {
            get { return nodeNameId; }
            set { nodeNameId = value; }
        }
        /// <summary>
        /// 目标责任书
        /// </summary>
        virtual public TargetRespBook TargetRespBookGuid
        {
            get { return targetRespBookGuid; }
            set { targetRespBookGuid = value; }
        }
        /// <summary>
        /// 效益目标
        /// </summary>
        virtual public decimal BenefitGoal
        {
            get { return benefitGoal; }
            set { benefitGoal = value; }
        }
        /// <summary>
        /// 形象进度
        /// </summary>
        virtual public string FigurativeProgress
        {
            get { return figurativeProgress; }
            set { figurativeProgress = value; }
        }
        /// <summary>
        /// 有效状态
        /// </summary>
        virtual public string EffectiveStatus
        {
            get { return effectiveStatus; }
            set { effectiveStatus = value; }
        }
        /// <summary>
        /// 预计产值
        /// </summary>
        virtual public decimal PredictValue
        {
            get { return predictValue; }
            set { predictValue = value; }
        }
        /// <summary>
        /// 预计完成时间
        /// </summary>
        virtual public DateTime PlanCompleteDate
        {
            get { return planCompleteDate; }
            set { planCompleteDate = value; }
        }
    }
}
