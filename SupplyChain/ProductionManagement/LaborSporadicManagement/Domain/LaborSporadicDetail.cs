using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
//using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.LaborSporadicManagement.Domain
{
    /// <summary>
    /// 结算类型
    /// </summary>
    public enum EnumSettlementType
    {
        不需结算 = 0,
        未结算 = 1,
        已结算 = 2
    }

    /// <summary>
    /// 零星用工明细
    /// </summary>
    [Serializable]
    public class LaborSporadicDetail : BaseDetail
    {
        private GWBSTree projectTast;
        private string projectTastName;
        private string projectTaskSyscode;
        private decimal accountPrice;
        private decimal accountSumMoney;
        private decimal accountLaborNum;
        private decimal predictLaborNum;
        private StandardUnit priceUnit;
        private string priceUnitName;
        private DateTime endDate = StringUtil.StrToDateTime("1900-01-01");
        private EnumSettlementType settlementState;
        private DateTime startDate = StringUtil.StrToDateTime("1900-01-01");
        //private string tastDetailName;
        private decimal realLaborNum;
        private StandardUnit quantityUnit;
        private string quantityUnitName;
        private DateTime realOperationDate = StringUtil.StrToDateTime("1900-01-01");
        private CostAccountSubject laborSubject;
        private string laborSubjectName;
        private string laborSubjectSysCode;

        private GWBSDetail projectTastDetail;
        private string projectTastDetailName;

        private Material resourceType;
        private string resourceTypeName;
        private string resourceTypeSpec;
        private string resourceTypeStuff;
        private string resourceSysCode;

        private string _balanceDtlGUID;
        private SubContractProject insteadTeam;
        private string insteadTeamName;
        private DateTime completeDate = StringUtil.StrToDateTime("1900-01-01");
        private string laborDescript;
        private int isCreate = 0;

        private long detailNumber;

        /// <summary>
        /// 人员配置信息
        /// </summary>
        public virtual string WorkersInformation { get; set; }

        /// <summary>
        /// 是否生成扣款单
        /// </summary>
        public virtual int IsCreate
        {
            get { return isCreate; }
            set { isCreate = value; }
        }

        /// <summary>
        /// 用工说明
        /// </summary>
        public virtual string LaborDescript
        {
            get { return laborDescript; }
            set { laborDescript = value; }
        }

        /// <summary>
        /// 资源规格
        /// </summary>
        public virtual string ResourceTypeSpec
        {
            get { return resourceTypeSpec; }
            set { resourceTypeSpec = value; }
        }


        /// <summary>
        /// 资源材质
        /// </summary>
        public virtual string ResourceTypeStuff
        {
            get { return resourceTypeStuff; }
            set { resourceTypeStuff = value; }
        }

        /// <summary>
        /// 资源层次码
        /// </summary>
        public virtual string ResourceSysCode
        {
            get { return resourceSysCode; }
            set { resourceSysCode = value; }
        }

        /// <summary>
        /// 业务完成时间
        /// </summary>
        public virtual DateTime CompleteDate
        {
            get { return completeDate; }
            set { completeDate = value; }
        }

        /// <summary>
        /// 被代工队伍
        /// </summary>
        public virtual SubContractProject InsteadTeam
        {
            get { return insteadTeam; }
            set { insteadTeam = value; }
        }
        /// <summary>
        /// 被代工队伍名称
        /// </summary>
        virtual public string InsteadTeamName
        {
            get { return insteadTeamName; }
            set { insteadTeamName = value; }
        }

        private string penaltyDeductionDetail;

        /// <summary>
        /// 罚扣款单明细GUID
        /// </summary>
        public virtual string PenaltyDeductionDetail
        {
            get { return penaltyDeductionDetail; }
            set { penaltyDeductionDetail = value; }
        }
        /// <summary>
        /// 工程任务名称
        /// </summary>
        virtual public string ProjectTastName
        {
            get { return projectTastName; }
            set { projectTastName = value; }
        }
        /// <summary>
        /// 工程任务节点层次码
        /// </summary>
        virtual public string ProjectTaskSyscode
        {
            get { return projectTaskSyscode; }
            set { projectTaskSyscode = value; }
        }
        /// <summary>
        /// 核算单价
        /// </summary>
        virtual public decimal AccountPrice
        {
            get { return accountPrice; }
            set { accountPrice = value; }
        }
        /// <summary>
        /// 核算合价
        /// </summary>
        virtual public decimal AccountSumMoney
        {
            get { return accountSumMoney; }
            set { accountSumMoney = value; }
        }
        /// <summary>
        /// 核算用工数量
        /// </summary>
        virtual public decimal AccountLaborNum
        {
            get { return accountLaborNum; }
            set { accountLaborNum = value; }
        }
        /// <summary>
        /// 计划用工数量
        /// </summary>
        virtual public decimal PredictLaborNum
        {
            get { return predictLaborNum; }
            set { predictLaborNum = value; }
        }
        /// <summary>
        /// 价格计量单位
        /// </summary>
        virtual public StandardUnit PriceUnit
        {
            get { return priceUnit; }
            set { priceUnit = value; }
        }
        /// <summary>
        /// 价格计量单位名称
        /// </summary>
        virtual public string PriceUnitName
        {
            get { return priceUnitName; }
            set { priceUnitName = value; }
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        virtual public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
        /// <summary>
        /// 结算状态
        /// </summary>
        virtual public EnumSettlementType SettlementState
        {
            get { return settlementState; }
            set { settlementState = value; }
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        virtual public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }
        ///// <summary>
        ///// 任务明细名称
        ///// </summary>
        //virtual public string TastDetailName
        //{
        //    get { return tastDetailName; }
        //    set { tastDetailName = value; }
        //}
        /// <summary>
        /// 数量计量单位
        /// </summary>
        virtual public StandardUnit QuantityUnit
        {
            get { return quantityUnit; }
            set { quantityUnit = value; }
        }
        /// <summary>
        /// 实际用工量
        /// </summary>
        virtual public decimal RealLaborNum
        {
            get { return realLaborNum; }
            set { realLaborNum = value; }
        }
        /// <summary>
        /// 数量单位名称
        /// </summary>
        virtual public string QuantityUnitName
        {
            get { return quantityUnitName; }
            set { quantityUnitName = value; }
        }
        /// <summary>
        /// 业务日期
        /// </summary>
        virtual public DateTime RealOperationDate
        {
            get { return realOperationDate; }
            set { realOperationDate = value; }
        }
        /// <summary>
        /// 用工科目
        /// </summary>
        virtual public CostAccountSubject LaborSubject
        {
            get { return laborSubject; }
            set { laborSubject = value; }
        }
        /// <summary>
        /// 用工科目名称
        /// </summary>
        virtual public string LaborSubjectName
        {
            get { return laborSubjectName; }
            set { laborSubjectName = value; }
        }
        /// <summary>
        /// 用工科目层次码
        /// </summary>
        virtual public string LaborSubjectSysCode
        {
            get { return laborSubjectSysCode; }
            set { laborSubjectSysCode = value; }
        }
        /// <summary>
        /// 工程任务明细
        /// </summary>
        virtual public GWBSDetail ProjectTastDetail
        {
            get { return projectTastDetail; }
            set { projectTastDetail = value; }
        }
        /// <summary>
        /// 工程任务明细名称
        /// </summary>
        virtual public string ProjectTastDetailName
        {
            get { return projectTastDetailName; }
            set { projectTastDetailName = value; }
        }
        /// <summary>
        /// 工程项目任务
        /// </summary>
        virtual public GWBSTree ProjectTast
        {
            get { return projectTast; }
            set { projectTast = value; }
        }
        /// <summary>
        /// 资源类型
        /// </summary>
        virtual public Material ResourceType
        {
            get { return resourceType; }
            set { resourceType = value; }
        }
        /// <summary>
        /// 资源类型名称
        /// </summary>
        virtual public string ResourceTypeName
        {
            get { return resourceTypeName; }
            set { resourceTypeName = value; }
        }

        /// <summary>
        /// 结算明细GUID
        /// </summary>
        public virtual string BalanceDtlGUID
        {
            get { return _balanceDtlGUID; }
            set { _balanceDtlGUID = value; }
        }

        /// <summary>
        /// 明细编号
        /// </summary>
        public virtual long DetailNumber
        {
            get { return detailNumber; }
            set { detailNumber = value; }
        }
    }
}
