using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.Util;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using System.ComponentModel;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Iesi.Collections.Generic;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.CostManagement.QWBS.Domain
{
    /// <summary>
    /// QWBS
    /// </summary>
    [Serializable]
    [Entity]
    public class QWBSManage : CategoryNode
    {
        private string nGUID;
        private decimal qdProjectCost;//标前成本工程量
        private decimal qdPrice;//标前成本综合单价
        private decimal costSumMoney;//成本测算合价
        private StandardUnit projectUnit;//工程量计量单位
        private string projectUnitName;//工程量计量单位名称
        private ISet<GWBSRelaPBS> pbsGUID = new HashedSet<GWBSRelaPBS>();//相关联的PBS
        private string pbsName;
        private string pbsCode;//组织层次码
        private decimal conProQuantity;//合同签订工程量
        private decimal conProMoney;//合同签订合价
        private decimal conPorPrict;//合同签订综合单价
        private StandardUnit priceUnit;//价格计量单位
        private string priceUnitName; 
        private decimal comProQuantity;//竣工结算工程量
        private decimal comProMoney;//竣工结算合价
        private decimal comProPrice;//竣工结算综合价格
        private QWBSLevel taskLevel;//清单任务级别
        private string taskName;//清单任务名称
        private string taskCharacter;//清单任务项目特征
        private string taskDigest;//清单任务摘要
        private DateTime requiredEndDate = ClientUtil.ToDateTime(DateTime.Now);//任务要求完成时间
        private DateTime requiredStartDate = ClientUtil.ToDateTime(DateTime.Now);//任务要求开始时间
        private GWBSTree projectTask;//所属清单任务类型（工程WBS）
        private string projectTaskName;
        private decimal bidDudgetBill;//投标预算工程量
        private decimal bidDudgetMoney;//投标预算合价
        private decimal bidDudgetPrice;//投标预算综合单价
        private decimal ownerBidProject;//业主招标工程量
        private QWBSState taskState;//状态
        //private string code;//清单项编码
        private string projectCode;//清单项目任务编码
        private string projectId;//所属项目
        private string projectName;
        private ContractGroup contractGroup;//契约组
        private string contractGroupName;//契约组名称
        private DateTime updatedDate = DateTime.Now;

        private PersonInfo _ownerGUID;
        private string _ownerName;
        private string _ownerOrgSysCode;
        private ProjectTaskTypeTree _projectTaskTypeGUID;
        private string _projectTaskTypeName;

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
        /// 责任人GUID
        /// </summary>
        public virtual PersonInfo OwnerGUID
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

        ///<summary>
        ///更新时间
        ///</summary>
        virtual public DateTime UpdatedDate
        {
            get { return updatedDate; }
            set { updatedDate = value; }
        }
            
        ///<summary>
        ///名称GUID
        ///</summary>
        virtual public string NGUID
        {
            get { return nGUID; }
            set { nGUID = value; }
        }
        ///<summary>
        ///标前成本工程量
        ///</summary>
        virtual public decimal QDProjectCost
        {
            get { return qdProjectCost; }
            set { qdProjectCost = value; }
        }
        ///<summary>
        ///标前成本综合单价
        ///</summary>
        virtual public decimal QDPrice
        {
            get { return qdPrice; }
            set { qdPrice = value; }
        }
        ///<summary>
        ///成本测算合价
        ///</summary>
        virtual public decimal CostSumMoney
        {
            get { return costSumMoney; }
            set { costSumMoney = value; }
        }
        ///<summary>
        ///工程量计量单位
        ///</summary>
        virtual public StandardUnit ProjectUnit
        {
            get { return projectUnit; }
            set { projectUnit = value; }
        }
        ///<summary>
        ///工程量计量单位名称
        ///</summary>
        virtual public string ProjectUnitName
        {
            get { return projectUnitName; }
            set { projectUnitName = value; }
        }
        ///<summary>
        ///关联的PBS
        ///</summary>
        virtual public ISet<GWBSRelaPBS> PBSGUID
        {
            get { return pbsGUID; }
            set { pbsGUID = value; }
        }
        ///<summary>
        ///PBS名称
        ///</summary>
        virtual public string PBSName
        {
            get { return pbsName; }
            set { pbsName = value; }
        }
        ///<summary>
        ///PBS组织层次码
        ///</summary>
        virtual public string PBSCode
        {
            get { return pbsCode; }
            set { pbsCode = value; }
        }
        ///<summary>
        ///合同签订工程量
        ///</summary>
        virtual public decimal ConProQuantity
        {
            get { return conProQuantity; }
            set { conProQuantity = value; }
        }
        ///<summary>
        ///合同签订合价
        ///</summary>
        virtual public decimal ConProMoney
        {
            get { return conProMoney; }
            set { conProMoney = value; }
        }
        ///<summary>
        ///合同签订综合单价
        ///</summary>
        virtual public decimal ConPorPrict
        {
            get { return conPorPrict; }
            set { conPorPrict = value; }
        }
        ///<summary>
        ///价格计量单位
        ///</summary>
        virtual public StandardUnit PriceUnit
        {
            get { return priceUnit; }
            set { priceUnit = value; }
        }
        ///<summary>
        ///价格计量单位名称
        ///</summary>
        virtual public string PriceUnitName
        {
            get { return priceUnitName; }
            set { priceUnitName = value; }
        }
        ///<summary>
        ///竣工结算工程量
        ///</summary>
        virtual public decimal ComProQuantity
        {
            get { return comProQuantity; }
            set { comProQuantity = value; }
        }
        ///<summary>
        ///竣工结算合价
        ///</summary>
        virtual public decimal ComProMoney
        {
            get { return comProMoney; }
            set { comProMoney = value; }
        }
        ///<summary>
        ///竣工结算综合单价
        ///</summary>
        virtual public decimal ComProPrice
        {
            get { return comProPrice; }
            set { comProPrice = value; }
        }
        ///<summary>
        ///清单任务级别
        ///</summary>
        virtual public QWBSLevel TaskLevel
        {
            get { return taskLevel; }
            set { taskLevel = value; }
        }
        ///<summary>
        ///清单任务名称
        ///</summary>
        virtual public string TaskName
        {
            get { return taskName; }
            set { taskName = value; }
        }
        ///<summary>
        ///清单任务项目特征
        ///</summary>
        virtual public string TaskCharacter
        {
            get { return taskCharacter; }
            set { taskCharacter = value; }
        }
        ///<summary>
        ///清单任务摘要
        ///</summary>
        virtual public string TaskDigest
        {
            get { return taskDigest; }
            set { taskDigest = value; }
        }
        ///<summary>
        ///任务要求完成时间
        ///</summary>
        virtual public DateTime RequiredEndDate
        {
            get { return requiredEndDate; }
            set { requiredEndDate = value; }
        }
        ///<summary>
        ///任务要求开始时间
        ///</summary>
        virtual public DateTime RequiredStartDate
        {
            get { return requiredStartDate; }
            set { requiredStartDate = value; }
        }
        ///<summary>
        ///所属清单任务类型
        ///</summary>
        virtual public GWBSTree ProjectTask
        {
            get { return projectTask; }
            set { projectTask = value; }
        }
        ///<summary>
        ///清单任务名称
        ///</summary>
        virtual public string ProjectTaskName
        {
            get { return projectTaskName; }
            set { projectTaskName = value; }
        }
        ///<summary>
        ///投标预算工程量
        ///</summary>
        virtual public decimal BidDudgetBill
        {
            get { return bidDudgetBill; }
            set { bidDudgetBill = value; }
        }
        ///<summary>
        ///投标预算合价
        ///</summary>
        virtual public decimal BidDudgetMoney
        {
            get { return bidDudgetMoney; }
            set { bidDudgetMoney = value; }
        }
        ///<summary>
        ///投标预算综合价格
        ///</summary>
        virtual public decimal BidDudgetPrice
        {
            get { return bidDudgetPrice; }
            set { bidDudgetPrice = value; }
        }
        ///<summary>
        ///业主招标工程量
        ///</summary>
        virtual public decimal OwnerBidProject
        {
            get { return ownerBidProject; }
            set { ownerBidProject = value; }
        }
        ///<summary>
        /// QWBS状态
        ///</summary>
        virtual public QWBSState TaskState
        {
            get { return taskState; }
            set { taskState = value; }
        }
        /////<summary>
        /////清单项编码
        /////</summary>
        //virtual public string Code
        //{
        //    get { return code; }
        //    set { code = value; }
        //}
        ///<summary>
        ///清单项目任务编码
        ///</summary>
        virtual public string ProjectCode
        {
            get { return projectCode; }
            set { projectCode = value; }
        }
         ///<summary>
        ///所属项目
        ///</summary>
        virtual public string ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }
         ///<summary>
        ///所属项目名称
        ///</summary>
        virtual public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }
        ///<summary>
        ///契约组
        ///</summary>
        virtual public ContractGroup ContractGroup
        {
            get { return contractGroup; }
            set { contractGroup = value; }
        }
        ///<summary>
        ///契约组名称
        ///</summary>
        virtual public string  ContractGroupName
        {
            get { return contractGroupName; }
            set { contractGroupName = value; }
        }
        /// <summary>
        /// 克隆对象
        /// </summary>
        /// <returns></returns>
        public virtual QWBSManage Clone()
        {
            QWBSManage cat = new QWBSManage();
            cat.Name = this.Name;
            //cat.Code = this.Code;
            cat.OrderNo = this.OrderNo;

            //cat.Describe = this.Describe;

            cat.OwnerGUID = this.OwnerGUID;
            cat.OwnerName = this.OwnerName;
            cat.OwnerOrgSysCode = this.OwnerOrgSysCode;

            cat.ProjectId = this.ProjectId;
            cat.ProjectName = this.ProjectName;

            //cat.BearOrgGUID = this.BearOrgGUID;
            //cat.BearOrgName = this.BearOrgName;

            cat.ProjectTaskTypeGUID = this.ProjectTaskTypeGUID;
            cat.ProjectTaskTypeName = this.ProjectTaskTypeName;

            //cat.ContractGroupGUID = this.ContractGroupGUID;
            //cat.ContractGroupCode = this.ContractGroupCode;

            foreach (GWBSRelaPBS rela in this.PBSGUID)
            {
                GWBSRelaPBS newRela = new GWBSRelaPBS();
                newRela.ThePBS = rela.ThePBS;
                newRela.PBSName = rela.PBSName;

                newRela.TheGWBSTree = cat.ProjectTask;
                cat.PBSGUID.Add(newRela);
            }

            return cat;
        }
    }

    /// <summary>
    /// 状态
    /// </summary>
    public enum QWBSState
    {
        [Description("制定")]
        制定 = 0,
        [Description("询价")]
        询价 = 1,
        [Description("提交")]
        提交 = 2,
        [Description("发布")]
        发布 = 3,
        [Description("冻结")]
        冻结 = 4,
        [Description("作废")]
        作废 = 5
    }

    /// <summary>
    /// 工程任务类型级别
    /// </summary>
    public enum QWBSLevel
    {
        [Description("项目")]
        项目 = 0,
        [Description("单位工程")]
        单位工程 = 1,
        [Description("子单位工程")]
        子单位工程 = 2,
        [Description("分部工程")]
        分部工程 = 3,
        [Description("子分部工程")]
        子分部工程 = 4,
        [Description("分项工程")]
        分项工程 = 5,
        [Description("专业")]
        专业 = 6,
        [Description("子专业")]
        子专业 = 7
    }
}
