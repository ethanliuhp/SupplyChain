using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain
{
    /// <summary>
    /// 契约组明细
    /// </summary>
    [Serializable]
    [Entity]
    public class ContractGroupDetail
    {
        private string id;
        private long version;
        private string code;
        private string _name;
        private string _contractType;
        private string _createPersonGUID;
        private string _createPersonName;
        private string _createPersonSysCode;
        private DateTime _createTime = DateTime.Now;
        private string _remark;
        private string _projectId;
        private string _projectName;
        private ContractGroup _theContractGroup;

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
        /// 契约编号
        /// </summary>
        virtual public string Code
        {
            get { return code; }
            set { code = value; }
        }
        /// <summary>
        /// 契约名称
        /// </summary>
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 契约类型
        /// </summary>
        public virtual string ContractType
        {
            get { return _contractType; }
            set { _contractType = value; }
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
        /// 创建人组织层次码
        /// </summary>
        public virtual string CreatePersonSysCode
        {
            get { return _createPersonSysCode; }
            set { _createPersonSysCode = value; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark
        {
            get { return _remark; }
            set { _remark = value; }
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
        /// 所属契约组
        /// </summary>
        public virtual ContractGroup TheContractGroup
        {
            get { return _theContractGroup; }
            set { _theContractGroup = value; }
        }
    }
}
