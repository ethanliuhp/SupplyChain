using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections;
using VirtualMachine.Core.Attributes;
using Iesi.Collections.Generic;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain
{
    /// <summary>
    /// 契约组
    /// </summary>
    [Serializable]
    [Entity]
    public class ContractGroup
    {
        private string code;
        private string id;
        private long version;
        private string _contractVersion;
        private string _createPersonGUID;
        private string _createPersonName;
        private DateTime _createDate = DateTime.Now;
        private string _createPersonSysCode;
        private ContractGroupState _state;
        private string _contractGroupType;
        private string _projectId;
        private string _projectName;
        private string _contractDesc;

        private Iesi.Collections.Generic.ISet<ContractGroupDetail> _details = new Iesi.Collections.Generic.HashedSet<ContractGroupDetail>();

        /// <summary>
        /// ID
        /// </summary>
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 版本
        /// </summary>
        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }
        /// <summary>
        /// Code
        /// </summary>
        virtual public string Code
        {
            get { return code; }
            set { code = value; }
        }

        /// <summary>
        /// 契约版本
        /// </summary>
        public virtual string ContractVersion
        {
            get { return _contractVersion; }
            set { _contractVersion = value; }
        }

        /// <summary>
        /// 创建人GUID
        /// </summary>
        public virtual string CreatePersonGUID
        {
            get { return _createPersonGUID; }
            set { _createPersonGUID = value; }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual string CreatePersonName
        {
            get { return _createPersonName; }
            set { _createPersonName = value; }
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        public virtual DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        /// <summary>
        /// 创建人组织层次码
        /// </summary>
        public virtual string CreatePersonSysCode
        {
            get { return _createPersonSysCode; }
            set { _createPersonSysCode = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual ContractGroupState State
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// 契约组类型
        /// </summary>
        public virtual string ContractGroupType
        {
            get { return _contractGroupType; }
            set { _contractGroupType = value; }
        }

        /// <summary>
        /// 项目Id
        /// </summary>
        public virtual string ProjectId
        {
            get { return _projectId; }
            set { _projectId = value; }
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }

        /// <summary>
        /// 契约说明
        /// </summary>
        public virtual string ContractDesc
        {
            get { return _contractDesc; }
            set { _contractDesc = value; }
        }

        /// <summary>
        /// 契约明细
        /// </summary>
        public virtual ISet<ContractGroupDetail> Details
        {
            get { return _details; }
            set { _details = value; }
        }

        /// <summary>
        /// 复制对象属性值
        /// </summary>
        /// <returns></returns>
        public virtual ContractGroup CopyAttributeValue(ContractGroup targetObj)
        {
            //
            return targetObj;
        }

    }

    /// <summary>
    /// 契约组状态
    /// </summary>
    public enum ContractGroupState
    {
        /// <summary>
        /// 制定
        /// </summary>
        [Description("制定")]
        制定 = 1,
        /// <summary>
        /// 已经确定了版本号，不允许再修改
        /// </summary>
        [Description("发布")]
        发布 = 2,
        /// <summary>
        ///已经启动了契约组执行流程
        /// </summary>
        [Description("执行")]
        执行 = 3,
        /// <summary>
        /// 契约组执行流程已经结束
        /// </summary>
        [Description("结束")]
        结束 = 4,
        /// <summary>
        /// 契约组被作废
        /// </summary>
        [Description("作废")]
        作废 = 5
    }
}
