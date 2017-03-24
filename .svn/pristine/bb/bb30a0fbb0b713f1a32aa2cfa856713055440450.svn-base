
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Business.Erp.SupplyChain.PMCAndWarning.Domain
{
    /// <summary>
    /// 预警指标
    /// </summary>
    [Serializable]
    public class WarningTarget
    {
        /// <summary>
        /// 工期状态检查
        /// </summary>
        public static string WarningTarget_GQ_StateCheck = "工期状态检查";
        /// <summary>
        /// 资料状态检查
        /// </summary>
        public static string WarningTarget_ZL_StateCheck = "资料状态检查";
        /// <summary>
        /// 物资采购合同
        /// </summary>
        public static string WarningTarget_WZ_SupplyOrder = "物资采购合同";
        /// <summary>
        /// 物资收料管理
        /// </summary>
        public static string WarningTarget_WZ_StockIn = "物资收料管理";
        /// <summary>
        /// 物资日常需求计划
        /// </summary>
        public static string WarningTarget_WZ_DailyPlan = "物资日常需求计划";
        /// <summary>
        /// 物资月度需求计划
        /// </summary>
        public static string WarningTarget_WZ_MonthPlan = "物资月度需求计划";

        /// <summary>
        /// 商务综合指标检查
        /// </summary>
        public static string WarningTarget_SW_ComprehensiveCheck = "商务综合指标检查";
        /// <summary>
        /// 收款指标（商务综合指标检查）
        /// </summary>
        public static string WarningTarget_SW_ProceedsTarget = "收款指标";
        /// <summary>
        /// 业主报量指标（商务综合指标检查）
        /// </summary>
        public static string WarningTarget_SW_OwnerQuantityTarget = "业主报量指标";
        /// <summary>
        /// 整改单
        /// </summary>
        public static string WarningTarget_SW_RectificationNoticeMaster = "整改通知";
        /// <summary>
        /// 专业检查
        /// </summary>
        public static string WarningTarget_SW_ProfessionInspectionRecordMaster = "专业检查";
        /// <summary>
        /// 工单商务复核
        /// </summary>
        public static string WarningTarget_SW_WorkOrderBusinessReview = "工单商务复核";
        /// <summary>
        /// 设备费用租赁结算
        /// </summary>
        public static string WarningTarget_SW_RentalCostClear = "设备费用租赁结算";
        /// <summary>
        /// 费用结算
        /// </summary>
        public static string WarningTarget_SW_CostClear = "费用结算";
        /// <summary>
        /// 成本核算
        /// </summary>
        public static string WarningTarget_SW_Costing = "成本核算";


        private string id;
        private long version;
        private string _targetCode;
        private string _targetName;
        private string _targetDesc;
        private string _targetCate;
        private StateCheckAction _checkAction;


        /// <summary>
        /// ID
        /// </summary>
        public virtual string Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 版本
        /// </summary>
        public virtual long Version
        {
            get { return version; }
            set { version = value; }
        }
        /// <summary>
        /// 指标代码
        /// </summary>
        public virtual string TargetCode
        {
            get { return _targetCode; }
            set { _targetCode = value; }
        }
        /// <summary>
        /// 指标名称
        /// </summary>
        public virtual string TargetName
        {
            get { return _targetName; }
            set { _targetName = value; }
        }
        /// <summary>
        /// 指标描述
        /// </summary>
        public virtual string TargetDesc
        {
            get { return _targetDesc; }
            set { _targetDesc = value; }
        }
        /// <summary>
        /// 指标分类
        /// </summary>
        public virtual string TargetCate
        {
            get { return _targetCate; }
            set { _targetCate = value; }
        }
        /// <summary>
        /// 状态检查动作
        /// </summary>
        public virtual StateCheckAction CheckAction
        {
            get { return _checkAction; }
            set { _checkAction = value; }
        }

    }
}
