using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Resource.MaterialResource.Domain;
using System.ComponentModel;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain
{
    /// <summary>
    /// 工程任务确认明细
    /// </summary>
    [Serializable]
    [Entity]
    public class GWBSTaskConfirm : BaseDetail
    {
        private string id;
        private CostItem costItem;
        private string costItemName;
        private StandardUnit workAmountUnitId;
        private string workAmountUnitName;
        private string projectTaskType;
        private decimal actualCompletedQuantity;
        private EnumGWBSTaskConfirmCollectState collectState;
        private decimal completedPercent;
        private decimal sumCompletedPercent;
        private string descript;
        private DateTime realOperationDate = DateTime.Now;
        private GWBSTree _GWBSTree;
        private string _GWBSTreeName;
        private string gwbsSysCode;
        private GWBSDetail _GWBSDetail;
        private string _GWBSDetailName;
        private SubContractProject taskHandler;
        private string taskHandlerName;
        private EnumGWBSTaskConfirmAccountingState accountingState;
        private string accountingDetailId;
        private DateTime? accountTime;
        private string accountingDetailTempId;
        private List<WeekScheduleDetail> weekScheduleDetail;
        private GWBSTaskConfirmMaster master;
        private string confirmState;
        private string confirmDescript;
        private decimal plannedQuantity;
        private decimal progressAfterConfirm;
        private decimal progressBeforeConfirm;
        private decimal quantiyAfterConfirm;
        private decimal quantityBeforeConfirm;

        private EnumMaterialFeeSettlementFlag materialFeeSettlementFlag = EnumMaterialFeeSettlementFlag.结算;
        private string _dailyCheckState;
        private WeekScheduleDetail weekScheduleDetailGUID;
        private long detailNumber;

        private PersonInfo createPerson;
        /// <summary>
        /// 制单人
        /// </summary>
        virtual public PersonInfo CreatePerson
        {
            get { return createPerson; }
            set { createPerson = value; }
        }
        private string createPersonName;
        /// <summary>
        /// 创建人名称
        /// </summary>
        public virtual string CreatePersonName
        {
            get { return createPersonName; }
            set { createPersonName = value; }
        }
        private OperationOrgInfo operOrgInfo;
        /// <summary>
        /// 业务组织
        /// </summary>
        virtual public OperationOrgInfo OperOrgInfo
        {
            get { return operOrgInfo; }
            set { operOrgInfo = value; }
        }
        private string operOrgInfoName;
        /// <summary>
        /// 业务组织名称
        /// </summary>
        public virtual string OperOrgInfoName
        {
            get { return operOrgInfoName; }
            set { operOrgInfoName = value; }
        }
        private string opgSysCode;
        /// <summary>
        /// 制单人组织层次码
        /// </summary>
        virtual public string OpgSysCode
        {
            get { return opgSysCode; }
            set { opgSysCode = value; }
        }
        /// <summary>
        /// 周进度计划明细
        /// </summary>
        virtual public WeekScheduleDetail WeekScheduleDetailGUID
        {
            get { return weekScheduleDetailGUID; }
            set { weekScheduleDetailGUID = value; }
        }

        /// <summary>
        /// 日常检查记录
        /// </summary>
        public virtual string DailyCheckState
        {
            get { return _dailyCheckState; }
            set { _dailyCheckState = value; }
        }

        /// <summary>
        /// 料费结算标志 0 不结算；1 结算
        /// </summary>
        public virtual EnumMaterialFeeSettlementFlag MaterialFeeSettlementFlag
        {
            get { return materialFeeSettlementFlag; }
            set { materialFeeSettlementFlag = value; }
        }

        /// <summary>
        /// 核算时间
        /// </summary>
        public virtual DateTime? AccountTime
        {
            get { return accountTime; }
            set { accountTime = value; }
        }

        private Iesi.Collections.Generic.ISet<GWBSTaskConfirmNode> nodeDetails = new Iesi.Collections.Generic.HashedSet<GWBSTaskConfirmNode>();
        /// <summary>
        /// 生产节点明细
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<GWBSTaskConfirmNode> NodeDetails
        {
            get { return nodeDetails; }
            set { nodeDetails = value; }
        }

        public virtual void AddNodeDetail(GWBSTaskConfirmNode detail)
        {
            NodeDetails.Add(detail);
            detail.GWBSTaskConfirm = this;
        }

        /// <summary>
        /// 确认前累计工长确认工程量
        /// </summary>
        public virtual decimal QuantityBeforeConfirm
        {
            get { return quantityBeforeConfirm; }
            set { quantityBeforeConfirm = value; }
        }

        /// <summary>
        /// 确认后累计工长确认工程量
        /// </summary>
        public virtual decimal QuantiyAfterConfirm
        {
            get { return quantiyAfterConfirm; }
            set { quantiyAfterConfirm = value; }
        }

        /// <summary>
        /// 确认前累积工长确认形象进度
        /// </summary>
        public virtual decimal ProgressBeforeConfirm
        {
            get { return progressBeforeConfirm; }
            set { progressBeforeConfirm = value; }
        }

        /// <summary>
        /// 确认后累积工长确认形象进度
        /// </summary>
        public virtual decimal ProgressAfterConfirm
        {
            get { return progressAfterConfirm; }
            set { progressAfterConfirm = value; }
        }

        /// <summary>
        /// 计划总工程量
        /// </summary>
        public virtual decimal PlannedQuantity
        {
            get { return plannedQuantity; }
            set { plannedQuantity = value; }
        }

        /// <summary>
        /// 工程任务明细所属GWBS结构层次码
        /// </summary>
        public virtual string GwbsSysCode
        {
            get { return gwbsSysCode; }
            set { gwbsSysCode = value; }
        }

        /// <summary>
        /// 确认依据
        /// </summary>
        public virtual string ConfirmDescript
        {
            get { return confirmDescript; }
            set { confirmDescript = value; }
        }

        /// <summary>
        /// 确认状态
        /// </summary>
        public virtual string ConfirmState
        {
            get { return confirmState; }
            set { confirmState = value; }
        }

        /// <summary>
        /// 工程任务确认主表
        /// </summary>
        public virtual GWBSTaskConfirmMaster Master
        {
            get { return master; }
            set { master = value; }
        }

        /// <summary>
        /// 工程任务名称
        /// </summary>
        public virtual string GWBSTreeName
        {
            get { return _GWBSTreeName; }
            set { _GWBSTreeName = value; }
        }

        /// <summary>
        /// 工程任务
        /// </summary>
        public virtual GWBSTree GWBSTree
        {
            get { return _GWBSTree; }
            set { _GWBSTree = value; }
        }

        /// <summary>
        /// 周进度计划明细(临时存放，不做持久化)
        /// </summary>
        public virtual List<WeekScheduleDetail> WeekScheduleDetail
        {
            get { return weekScheduleDetail; }
            set { weekScheduleDetail = value; }
        }

        /// <summary>
        /// 核算明细ID
        /// </summary>
        public virtual string AccountingDetailId
        {
            get { return accountingDetailId; }
            set { accountingDetailId = value; }
        }

        /// <summary>
        /// 核算明细的临时ID(不做map，用于临时找到生成的核算明细)
        /// </summary>
        public virtual string AccountingDetailTempId
        {
            get { return accountingDetailTempId; }
            set { accountingDetailTempId = value; }
        }

        /// <summary>
        /// 任务承担者名称
        /// </summary>
        public virtual string TaskHandlerName
        {
            get { return taskHandlerName; }
            set { taskHandlerName = value; }
        }

        /// <summary>
        /// 任务承担者
        /// </summary>
        public virtual SubContractProject TaskHandler
        {
            get { return taskHandler; }
            set { taskHandler = value; }
        }


        /// <summary>
        /// 工程任务明细名称
        /// </summary>
        public virtual string GWBSDetailName
        {
            get { return _GWBSDetailName; }
            set { _GWBSDetailName = value; }
        }

        /// <summary>
        /// 工程任务明细
        /// </summary>
        public virtual GWBSDetail GWBSDetail
        {
            get { return _GWBSDetail; }
            set { _GWBSDetail = value; }
        }

        /// <summary>
        /// 业务发生时间(确认工程量截止时间)
        /// </summary>
        public virtual DateTime RealOperationDate
        {
            get { return realOperationDate; }
            set { realOperationDate = value; }
        }

        /// <summary>
        /// 实际与计划差异说明
        /// </summary>
        public virtual string Descript
        {
            get { return descript; }
            set { descript = value; }
        }

        /// <summary>
        /// 累积完成百分比(暂未用)
        /// </summary>
        public virtual decimal SumCompletedPercent
        {
            get { return sumCompletedPercent; }
            set { sumCompletedPercent = value; }
        }

        /// <summary>
        /// 本次确认形象进度（完成百分比）
        /// </summary>
        public virtual decimal CompletedPercent
        {
            get { return completedPercent; }
            set { completedPercent = value; }
        }


        /// <summary>
        /// 汇集状态
        /// </summary>
        public virtual EnumGWBSTaskConfirmCollectState CollectState
        {
            get { return collectState; }
            set { collectState = value; }
        }

        /// <summary>
        /// 核算状态
        /// </summary>
        public virtual EnumGWBSTaskConfirmAccountingState AccountingState
        {
            get { return accountingState; }
            set { accountingState = value; }
        }

        /// <summary>
        /// 实际完成工程量(本次工长确认工程量)
        /// </summary>
        public virtual decimal ActualCompletedQuantity
        {
            get { return actualCompletedQuantity; }
            set { actualCompletedQuantity = value; }
        }

        /// <summary>
        /// 工程量类型
        /// </summary>
        public virtual string ProjectTaskType
        {
            get { return projectTaskType; }
            set { projectTaskType = value; }
        }

        /// <summary>
        /// 工程量计量单位名称
        /// </summary>
        public virtual string WorkAmountUnitName
        {
            get { return workAmountUnitName; }
            set { workAmountUnitName = value; }
        }

        /// <summary>
        /// 工程量计量单位
        /// </summary>
        public virtual StandardUnit WorkAmountUnitId
        {
            get { return workAmountUnitId; }
            set { workAmountUnitId = value; }
        }

        /// <summary>
        /// 成本项名称
        /// </summary>
        public virtual string CostItemName
        {
            get { return costItemName; }
            set { costItemName = value; }
        }

        /// <summary>
        /// 成本项
        /// </summary>
        public virtual CostItem CostItem
        {
            get { return costItem; }
            set { costItem = value; }
        }

        /// <summary>
        /// ID
        /// </summary>
        public virtual string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 明细编号
        /// </summary>
        virtual public long DetailNumber
        {
            get { return detailNumber; }
            set { detailNumber = value; }
        }
    }

    /// <summary>
    /// 核算状态
    /// </summary>
    public enum EnumGWBSTaskConfirmAccountingState
    {
        [Description("未核算")]
        未核算 = 0,
        [Description("己核算")]
        己核算 = 1
    }

    /// <summary>
    /// 汇集状态
    /// </summary>
    public enum EnumGWBSTaskConfirmCollectState
    {
        [Description("未汇集")]
        未汇集 = 0,
        [Description("己汇集")]
        己汇集 = 1
    }

    /// <summary>
    /// 料费结算标志
    /// </summary>
    public enum EnumMaterialFeeSettlementFlag
    {
        [Description("不结算")]
        不结算 = 0,
        [Description("结算")]
        结算 = 1
    }
}
