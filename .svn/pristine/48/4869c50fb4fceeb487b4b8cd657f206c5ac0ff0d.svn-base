using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Iesi.Collections;
using Iesi.Collections.Generic;
using System.ComponentModel;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain
{
    /// <summary>
    /// 工程WBS树
    /// </summary>
    [Serializable]
    public class GWBSTree : CategoryNode
    {

        private ProjectTaskTypeTree _projectTaskTypeGUID;
        private string _projectTaskTypeName;
        private string _summary;
        private GWBSTreeState _taskState = GWBSTreeState.编制;
        private DateTime? _taskStateTime;
        private decimal _contractWorkAmount;
        private decimal _contractPrice;
        private decimal _contractTotalPrice;

        private string _ownerGUID;
        private string _ownerName;
        private string _ownerOrgSysCode;
        private StandardUnit _workAmountUnitGUID;
        private string _workAmountUnitName;
        private StandardUnit _priceAmountUnitGUID;
        private string _priceAmountUnitName;
        private string _managementFeatureCode;
        private decimal _responsibilityTotalPrice;
        private decimal _planTotalPrice;
        private decimal _projectDeptDefiniteTotalPrice;
        private DateTime? _taskPlanStartTime;
        private DateTime? _taskPlanEndTime;
        private GWBSTreeManagementMode _managementMode;
        private string _checkRequire;

        private ContractGroup _contractGroupGUID;
        private string _contractGroupCode;

        private SupplierRelationInfo _bearOrgGUID;
        private string _bearOrgName;

        private WBSNodeType _nodeType = WBSNodeType.WBS;

        private string _theProjectGUID;
        private string _theProjectName;

        private DateTime _updatedDate = DateTime.Now;

        private ISet<GWBSDetail> _Details = new HashedSet<GWBSDetail>();

        private ISet<GWBSRelaPBS> _listRelaPBS = new HashedSet<GWBSRelaPBS>();

        private bool _isAccountNode = false;

        private string _NGUID;
        private decimal _addUpFigureProgress;
        private bool _responsibleAccFlag;
        private bool _costAccFlag;
        private bool _productConfirmFlag;
        private bool _subContractFeeFlag;
        private int _checkBatchNumber;

        /// <summary>
        /// 名称GUID
        /// </summary>
        public virtual string NGUID
        {
            get { return _NGUID; }
            set { _NGUID = value; }
        }

        /// <summary>
        /// 累计工程形象进度
        /// </summary>
        public virtual decimal AddUpFigureProgress
        {
            get { return _addUpFigureProgress; }
            set { _addUpFigureProgress = value; }
        }

        /// <summary>
        /// 责任核算标志
        /// </summary>
        public virtual bool ResponsibleAccFlag
        {
            get { return _responsibleAccFlag; }
            set { _responsibleAccFlag = value; }
        }

        /// <summary>
        /// 成本核算标志
        /// </summary>
        public virtual bool CostAccFlag
        {
            get { return _costAccFlag; }
            set { _costAccFlag = value; }
        }

        /// <summary>
        /// 成产确认标志
        /// </summary>
        public virtual bool ProductConfirmFlag
        {
            get { return _productConfirmFlag; }
            set { _productConfirmFlag = value; }
        }

        /// <summary>
        /// 分包取费标志
        /// </summary>
        public virtual bool SubContractFeeFlag
        {
            get { return _subContractFeeFlag; }
            set { _subContractFeeFlag = value; }
        }

        /// <summary>
        /// 检验批数目
        /// </summary>
        public virtual int CheckBatchNumber
        {
            get { return _checkBatchNumber; }
            set { _checkBatchNumber = value; }
        }




        /// <summary>
        /// 工程任务类型
        /// </summary>
        public virtual ProjectTaskTypeTree ProjectTaskTypeGUID
        {
            get { return _projectTaskTypeGUID; }
            set { _projectTaskTypeGUID = value; }
        }
        /// <summary>
        /// 工程任务类型名称
        /// </summary>
        public virtual string ProjectTaskTypeName
        {
            get { return _projectTaskTypeName; }
            set { _projectTaskTypeName = value; }
        }
        /// <summary>
        /// 摘要
        /// </summary>
        public virtual string Summary
        {
            get { return _summary; }
            set { _summary = value; }
        }

        /// <summary>
        /// 工程任务状态
        /// </summary>
        public virtual GWBSTreeState TaskState
        {
            get { return _taskState; }
            set { _taskState = value; }
        }
        /// <summary>
        /// 工程任务状态时间
        /// </summary>
        public virtual DateTime? TaskStateTime
        {
            get { return _taskStateTime; }
            set { _taskStateTime = value; }
        }
        /// <summary>
        /// 合同工程量
        /// </summary>
        public virtual decimal ContractWorkAmount
        {
            get { return _contractWorkAmount; }
            set { _contractWorkAmount = value; }
        }
        /// <summary>
        /// 合同单价
        /// </summary>
        public virtual decimal ContractPrice
        {
            get { return _contractPrice; }
            set { _contractPrice = value; }
        }
        /// <summary>
        /// 合同合价
        /// </summary>
        public virtual decimal ContractTotalPrice
        {
            get { return _contractTotalPrice; }
            set { _contractTotalPrice = value; }
        }

        /// <summary>
        /// 责任人GUID
        /// </summary>
        public virtual string OwnerGUID
        {
            get { return _ownerGUID; }
            set { _ownerGUID = value; }
        }
        /// <summary>
        /// 责任人用户名
        /// </summary>
        public virtual string OwnerName
        {
            get { return _ownerName; }
            set { _ownerName = value; }
        }
        /// <summary>
        /// 责任人组织层次码
        /// </summary>
        public virtual string OwnerOrgSysCode
        {
            get { return _ownerOrgSysCode; }
            set { _ownerOrgSysCode = value; }
        }

        /// <summary>
        /// 工程计量单位
        /// </summary>
        public virtual StandardUnit WorkAmountUnitGUID
        {
            get { return _workAmountUnitGUID; }
            set { _workAmountUnitGUID = value; }
        }
        /// <summary>
        /// 工程计量单位名称
        /// </summary>
        public virtual string WorkAmountUnitName
        {
            get { return _workAmountUnitName; }
            set { _workAmountUnitName = value; }
        }
        /// <summary>
        /// 价格计量单位
        /// </summary>
        public virtual StandardUnit PriceAmountUnitGUID
        {
            get { return _priceAmountUnitGUID; }
            set { _priceAmountUnitGUID = value; }
        }
        /// <summary>
        /// 价格计量单位名称
        /// </summary>
        public virtual string PriceAmountUnitName
        {
            get { return _priceAmountUnitName; }
            set { _priceAmountUnitName = value; }
        }
        /// <summary>
        /// 管理特征代码
        /// </summary>
        public virtual string ManagementFeatureCode
        {
            get { return _managementFeatureCode; }
            set { _managementFeatureCode = value; }
        }
        /// <summary>
        /// 责任合价
        /// </summary>
        public virtual decimal ResponsibilityTotalPrice
        {
            get { return _responsibilityTotalPrice; }
            set { _responsibilityTotalPrice = value; }
        }
        /// <summary>
        /// 计划合价
        /// </summary>
        public virtual decimal PlanTotalPrice
        {
            get { return _planTotalPrice; }
            set { _planTotalPrice = value; }
        }
        /// <summary>
        /// 工程部门确定合价
        /// </summary>
        public virtual decimal ProjectDeptDefiniteTotalPrice
        {
            get { return _projectDeptDefiniteTotalPrice; }
            set { _projectDeptDefiniteTotalPrice = value; }
        }
        /// <summary>
        /// 任务计划开始时间
        /// </summary>
        public virtual DateTime? TaskPlanStartTime
        {
            get { return _taskPlanStartTime; }
            set { _taskPlanStartTime = value; }
        }
        /// <summary>
        /// 任务计划结束时间
        /// </summary>
        public virtual DateTime? TaskPlanEndTime
        {
            get { return _taskPlanEndTime; }
            set { _taskPlanEndTime = value; }
        }
        /// <summary>
        /// 管理方式
        /// </summary>
        public virtual GWBSTreeManagementMode ManagementMode
        {
            get { return _managementMode; }
            set { _managementMode = value; }
        }
        /// <summary>
        /// 检查要求
        /// </summary>
        public virtual string CheckRequire
        {
            get { return _checkRequire; }
            set { _checkRequire = value; }
        }

        /// <summary>
        /// 所属项目
        /// </summary>
        public virtual string TheProjectGUID
        {
            get { return _theProjectGUID; }
            set { _theProjectGUID = value; }
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string TheProjectName
        {
            get { return _theProjectName; }
            set { _theProjectName = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime UpdatedDate
        {
            get { return _updatedDate; }
            set { _updatedDate = value; }
        }

        /// <summary>
        /// 节点类型
        /// </summary>
        public virtual WBSNodeType NodeType
        {
            get { return _nodeType; }
            set { _nodeType = value; }
        }

        /// <summary>
        /// 契约组
        /// </summary>
        public virtual ContractGroup ContractGroupGUID
        {
            get { return _contractGroupGUID; }
            set { _contractGroupGUID = value; }
        }

        /// <summary>
        /// 契约组代号
        /// </summary>
        public virtual string ContractGroupCode
        {
            get { return _contractGroupCode; }
            set { _contractGroupCode = value; }
        }

        /// <summary>
        /// 承担组织
        /// </summary>
        public virtual SupplierRelationInfo BearOrgGUID
        {
            get { return _bearOrgGUID; }
            set { _bearOrgGUID = value; }
        }

        /// <summary>
        /// 承担组织名称
        /// </summary>
        public virtual string BearOrgName
        {
            get { return _bearOrgName; }
            set { _bearOrgName = value; }
        }

        /// <summary>
        /// 工程WBS明细
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<GWBSDetail> Details
        {
            get { return _Details; }
            set { _Details = value; }
        }

        /// <summary>
        /// 关联PBS
        /// </summary>
        public virtual ISet<GWBSRelaPBS> ListRelaPBS
        {
            get { return _listRelaPBS; }
            set { _listRelaPBS = value; }
        }

        /// <summary>
        /// 是否为核算节点
        /// </summary>
        public virtual bool IsAccountNode
        {
            get { return _isAccountNode; }
            set { _isAccountNode = value; }
        }
        /// <summary>
        /// 克隆对象
        /// </summary>
        /// <returns></returns>
        public virtual GWBSTree Clone()
        {
            GWBSTree cat = new GWBSTree();
            cat.Name = this.Name;
            //cat.Code = this.Code;
            cat.OrderNo = this.OrderNo;

            cat.Describe = this.Describe;

            cat.OwnerGUID = this.OwnerGUID;
            cat.OwnerName = this.OwnerName;
            cat.OwnerOrgSysCode = this.OwnerOrgSysCode;

            cat.TheProjectGUID = this.TheProjectGUID;
            cat.TheProjectName = this.TheProjectName;

            cat.BearOrgGUID = this.BearOrgGUID;
            cat.BearOrgName = this.BearOrgName;

            cat.ProjectTaskTypeGUID = this.ProjectTaskTypeGUID;
            cat.ProjectTaskTypeName = this.ProjectTaskTypeName;

            //cat.ContractGroupGUID = this.ContractGroupGUID;
            //cat.ContractGroupCode = this.ContractGroupCode;

            foreach (GWBSRelaPBS rela in this.ListRelaPBS)
            {
                GWBSRelaPBS newRela = new GWBSRelaPBS();
                newRela.ThePBS = rela.ThePBS;
                newRela.PBSName = rela.PBSName;

                newRela.TheGWBSTree = cat;
                cat.ListRelaPBS.Add(newRela);
            }

            return cat;
        }
    }

    /// <summary>
    /// GWBS状态
    /// </summary>
    public enum GWBSTreeState
    {
        /// <summary>
        /// 编辑状态
        /// </summary>
        [Description("编制")]
        编制 = 1,
        /// <summary>
        /// 编制完成，提交审批
        /// </summary>
        [Description("提交")]
        提交 = 2,
        /// <summary>
        /// 审批通过，发布执行
        /// </summary>
        [Description("执行")]
        执行 = 3,
        /// <summary>
        /// {进度计划节点}上传了【任务实际完成时间】，并且所有下属{工程任务明细}_【任务完工百分比】均为100%；
        /// </summary>
        [Description("完工")]
        完工 = 4,
        /// <summary>
        /// 可作参考，不能继续使用；
        /// </summary>
        [Description("冻结")]
        冻结 = 5,
        /// <summary>
        /// 该工程项目任务已经无效。
        /// </summary>
        [Description("作废")]
        作废 = 6
    }

    public enum GWBSTreeManagementMode
    {
        [Description("自行完成")]
        自行完成 = 1,
        /// <summary>
        /// 企业内部部门对任务的收入、成本包干，如：安装。
        /// </summary>
        [Description("内部大包")]
        内部大包 = 2,
        /// <summary>
        /// 企业外部对任务的收入、成本包干，如一些专业分包。
        /// </summary>
        [Description("外部大包")]
        外部大包 = 3,
        /// <summary>
        /// 由企业外部承担工程任务的某些工作内容，如劳务分包。
        /// </summary>
        [Description("外部分包")]
        外部分包 = 4
    }

    /// <summary>
    /// 节点类型
    /// </summary>
    public enum WBSNodeType
    {
        WBS = 1,
        PBS = 2
    }
}
