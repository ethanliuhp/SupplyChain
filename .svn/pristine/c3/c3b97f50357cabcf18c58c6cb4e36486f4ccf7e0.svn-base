using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.PenaltyDeductionManagement.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using VirtualMachine.Component.Util;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain
{
    /// <summary>
    /// 检查记录
    /// </summary>
    [Serializable]
    public class InspectionRecord : BaseMaster
    {
        private WeekScheduleDetail _WeekScheduleDetail;
        private GWBSTree _GWBSTree;
        private string _GWBSTreeName;
        private string _GWBSTreeSysCode;
        private PBSTree _PBSTree;
        private string _PBSTreeName;
        private string _GWBSDescription;
        private string inspectionSpecial;
        private string inspectionSpecialCode;
        private string inspectionContent;
        private string specialInspectionState;
        private string inspectionStatus;
        private string inspectionConclusion;
        private string inspecPersonOpgSysCode;
        private string penaltyDeductionMaster;
        private SubContractProject bearTeam;
        private string bearTeamName;
        private int deductionSign;
        private int correctiveSign;
        private int inspectType;//1 日常检查,2质量验收 
        /// <summary>
        /// 检查类型
        /// </summary>
        virtual public int InspectType
        {
            get { return inspectType; }
            set { inspectType = value; }
        }
        /// <summary>
        /// 承担队伍
        /// </summary>
        virtual public SubContractProject BearTeam
        {
            get { return bearTeam; }
            set { bearTeam = value; }
        }

        /// <summary>
        /// 罚扣款标志
        /// </summary>
        virtual public int DeductionSign
        {
            get { return deductionSign; }
            set { deductionSign = value; }
        }
        /// <summary>
        /// 整改标志
        /// </summary>
        virtual public int CorrectiveSign
        {
            get { return correctiveSign; }
            set { correctiveSign = value; }
        }
        /// <summary>
        /// 承担队伍
        /// </summary>
        virtual public string BearTeamName
        {
            get { return bearTeamName; }
            set { bearTeamName = value; }
        }
        /// <summary>
        /// 周进度计划明细GUID
        /// </summary>
        public virtual WeekScheduleDetail WeekScheduleDetail
        {
            get { return _WeekScheduleDetail; }
            set { _WeekScheduleDetail = value; }
        }
        /// <summary>
        /// 工程WBS树(工程项目任务)
        /// </summary>
        public virtual GWBSTree GWBSTree
        {
            get { return _GWBSTree; }
            set { _GWBSTree = value; }
        }
        /// <summary>
        /// 工程WBS树(工程项目任务名称)
        /// </summary>
        public virtual string GWBSTreeName
        {
            get { return _GWBSTreeName; }
            set { _GWBSTreeName = value; }
        }
        /// <summary>
        /// 工程WBS树(工程项目任务层次码)
        /// </summary>
        public virtual string GWBSTreeSysCode
        {
            get { return _GWBSTreeSysCode; }
            set { _GWBSTreeSysCode = value; }
        }
        /// <summary>
        /// 工程项目任务摘要
        /// </summary>
        public virtual string GWBSDescription
        {
            get { return _GWBSDescription; }
            set { _GWBSDescription = value; }
        }
        /// <summary>
        /// 工区
        /// </summary>
        public virtual PBSTree PBSTree
        {
            get { return _PBSTree; }
            set { _PBSTree = value; }
        }
        /// <summary>
        /// 工区名称
        /// </summary>
        public virtual string PBSTreeName
        {
            get { return _PBSTreeName; }
            set { _PBSTreeName = value; }
        }
        /// <summary>
        /// 检查专业
        /// </summary>
        public virtual string InspectionSpecial
        {
            get { return inspectionSpecial; }
            set { inspectionSpecial = value; }
        }
        /// <summary>
        /// 检查专业编号
        /// </summary>
        public virtual string InspectionSpecialCode
        {
            get { return inspectionSpecialCode; }
            set { inspectionSpecialCode = value; }
        }
        /// <summary>
        /// 专业检查状态字位置
        /// </summary>
        public virtual string SpecialInspectionState
        {
            get { return specialInspectionState; }
            set { specialInspectionState = value; }
        }
        /// <summary>
        /// 检查内容说明
        /// </summary>
        public virtual string InspectionContent
        {
            get { return inspectionContent; }
            set { inspectionContent = value; }
        }
        /// <summary>
        /// 检查情况
        /// </summary>
        public virtual string InspectionStatus
        {
            get { return inspectionStatus; }
            set { inspectionStatus = value; }
        }
        /// <summary>
        /// 检查结论
        /// </summary>
        public virtual string InspectionConclusion
        {
            get { return inspectionConclusion; }
            set { inspectionConclusion = value; }
        }
        /// <summary>
        /// 检查责任人组织层次码
        /// </summary>
        public virtual string InspecPersonOpgSysCode
        {
            get { return inspecPersonOpgSysCode; }
            set { inspecPersonOpgSysCode = value; }
        }
        /// <summary>
        /// 罚扣款单GUID
        /// </summary>
        public virtual string PenaltyDeductionMaster
        {
            get { return penaltyDeductionMaster; }
            set { penaltyDeductionMaster = value; }
        }
        private Iesi.Collections.Generic.ISet<RectificationNotice> rectificationNotices = new Iesi.Collections.Generic.HashedSet<RectificationNotice>();
        public virtual Iesi.Collections.Generic.ISet<RectificationNotice> RectificationNotices
        {
            get { return rectificationNotices; }
            set { rectificationNotices = value; }
        }

        /// <summary>
        /// 添加明细(整改通知)
        /// </summary>
        /// <param name="detail"></param>
        public virtual void AddRectificationNotices(RectificationNotice detail)
        {
            detail.Master = this;
            RectificationNotices.Add(detail);
        }

        private Iesi.Collections.Generic.ISet<SitePictureVideo> sitePictureVideos = new Iesi.Collections.Generic.HashedSet<SitePictureVideo>();
        public virtual Iesi.Collections.Generic.ISet<SitePictureVideo> SitePictureVideos
        {
            get { return sitePictureVideos; }
            set { sitePictureVideos = value; }
        }

        /// <summary>
        /// 添加明细(现场照片和视频)
        /// </summary>
        /// <param name="detail"></param>
        public virtual void AddSitePictureVideo(SitePictureVideo detail)
        {
            detail.Master = this;
            SitePictureVideos.Add(detail);
        }

    }
}
