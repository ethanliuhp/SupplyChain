using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using System.ComponentModel;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain
{
    /// <summary>
    /// 工程任务确认主表
    /// </summary>
    [Serializable]
    [Entity]
    public class GWBSTaskConfirmMaster : BaseMaster
    {
        private string id;
        //private string code;
        private PersonInfo confirmHandlePerson;
        private string confirmHandlePersonName;
        //private DateTime createDate = DateTime.Now;
        private DateTime? confirmDate;
        //private DocumentState docState;
        //private string projectId;
        //private string projectName;
        //private string opgSysCode;
        private SubContractProject subContractProject;
        private string taskHandleName;
        private EnumConfirmBillType _billType = EnumConfirmBillType.计划工单;

        /// <summary>
        /// 工单类型
        /// </summary>
        public virtual EnumConfirmBillType BillType
        {
            get { return _billType; }
            set { _billType = value; }
        }

        /// <summary>
        /// 承担者名称
        /// </summary>
        public virtual string TaskHandleName
        {
            get { return taskHandleName; }
            set { taskHandleName = value; }
        }

        /// <summary>
        /// 分包项目
        /// </summary>
        public virtual SubContractProject SubContractProject
        {
            get { return subContractProject; }
            set { subContractProject = value; }
        }

        /// <summary>
        /// 组织层次码
        /// </summary>
        //public virtual string OpgSysCode
        //{
        //    get { return opgSysCode; }
        //    set { opgSysCode = value; }
        //}

        //private Iesi.Collections.Generic.ISet<GWBSTaskConfirm> details = new Iesi.Collections.Generic.HashedSet<GWBSTaskConfirm>();
        ///// <summary>
        ///// 明细集合
        ///// </summary>
        //public virtual Iesi.Collections.Generic.ISet<GWBSTaskConfirm> Details
        //{
        //    get { return details; }
        //    set { details = value; }
        //}

        /// <summary>
        /// 添加明细
        /// </summary>
        /// <param name="detail"></param>
        public virtual void AddDetail(GWBSTaskConfirm detail)
        {
            detail.Master = this;
            Details.Add(detail);
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        //public virtual string ProjectName
        //{
        //    get { return projectName; }
        //    set { projectName = value; }
        //}

        /// <summary>
        /// 项目ID
        /// </summary>
        //public virtual string ProjectId
        //{
        //    get { return projectId; }
        //    set { projectId = value; }
        //}

        /// <summary>
        /// 状态
        /// </summary>
        //public virtual DocumentState DocState
        //{
        //    get { return docState; }
        //    set { docState = value; }
        //}

        /// <summary>
        /// 创建时间
        /// </summary>
        //public virtual DateTime CreateDate
        //{
        //    get { return createDate; }
        //    set { createDate = value; }
        //}

        /// <summary>
        /// 确认时间
        /// </summary>
        public virtual DateTime? ConfirmDate
        {
            get { return confirmDate; }
            set { confirmDate = value; }
        }

        /// <summary>
        /// 确认人名称（现存责任人名称）
        /// </summary>
        public virtual string ConfirmHandlePersonName
        {
            get { return confirmHandlePersonName; }
            set { confirmHandlePersonName = value; }
        }

        /// <summary>
        /// 确认人（现存责任人）
        /// </summary>
        public virtual PersonInfo ConfirmHandlePerson
        {
            get { return confirmHandlePerson; }
            set { confirmHandlePerson = value; }
        }

        //public virtual string Code
        //{
        //    get { return code; }
        //    set { code = value; }
        //}

        public virtual string Id
        {
            get { return id; }
            set { id = value; }
        }
    }

    /// <summary>
    /// 确认单类型
    /// </summary>
    public enum EnumConfirmBillType
    {
        [Description("计划工单")]
        计划工单 = 0,
        [Description("虚拟工单")]
        虚拟工单 = 1
    }
}
