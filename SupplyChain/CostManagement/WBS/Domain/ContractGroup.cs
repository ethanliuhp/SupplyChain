using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections;
using VirtualMachine.Core.Attributes;
using Iesi.Collections.Generic;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain
{
    /// <summary>
    /// 契约（契约组）
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
        private DateTime submitDate = StringUtil.StrToDateTime("1900-01-01");

        /// <summary>
        /// 提交时间
        /// </summary>
        virtual public DateTime SubmitDate
        {
            get { return submitDate; }
            set { submitDate = value; }
        }
        private DateTime realOperationDate = DateTime.Now;

        /// <summary>
        /// 制单时间
        /// </summary>
        virtual public DateTime RealOperationDate
        {
            get { return realOperationDate; }
            set { realOperationDate = value; }
        }


        private string settleType;//结算方式

        virtual public string SettleType
        {
            get { return settleType; }
            set { settleType = value; }
        }
        private string bearRange;//承担范围

        virtual public string BearRange
        {
            get { return bearRange; }
            set { bearRange = value; }
        }
        private DateTime singDate;//签订时间

        virtual public DateTime SingDate
        {
            get { return singDate; }
            set { singDate = value; }
        }


        private decimal projectVisa;
        /// <summary>
        /// 工期签证天数
        /// </summary>
        public virtual decimal ProjectVisa
        {
            get { return projectVisa; }
            set { projectVisa = value; }
        }
        private decimal confirmMoney;
        /// <summary>
        /// 确认金额
        /// </summary>
        public virtual decimal ConfirmMoney
        {
            get { return confirmMoney; }
            set { confirmMoney = value; }
        }
        private decimal submitMoney;
        /// <summary>
        /// 报送金额
        /// </summary>
        public virtual decimal SubmitMoney
        {
            get { return submitMoney; }
            set { submitMoney = value; }
        }
        private StandardUnit projectUnit;
        /// <summary>
        /// 工期计量单位
        /// </summary>
        public virtual StandardUnit ProjectUnit
        {
            get { return projectUnit; }
            set { projectUnit = value; }
        }

        private string projectUnitName;
        /// <summary>
        /// 工期计量单位名称
        /// </summary>
        public virtual string ProjectUnitName
        {
            get { return projectUnitName; }
            set { projectUnitName = value; }
        }

        private StandardUnit priceUnit;
        /// <summary>
        /// 价格计量单位
        /// </summary>
        public virtual StandardUnit PriceUnit
        {
            get { return priceUnit; }
            set { priceUnit = value; }
        }

        private string priceUnitName;
        /// <summary>
        /// 价格计量单位名称
        /// </summary>
        public virtual string PriceUnitName
        {
            get { return priceUnitName; }
            set { priceUnitName = value; }
        }

        private string _contractNumber;
        private ContractGroup _changeContract;
        private string _contractName;
        /// <summary>
        /// 契约编号（契约的业务编号）
        /// </summary>
        public virtual string ContractNumber
        {
            get { return _contractNumber; }
            set { _contractNumber = value; }
        }
        /// <summary>
        /// 变更契约（描述本契约所变更或补充的契约，如：工程变更所针对的工程合同契约等）
        /// </summary>
        public virtual ContractGroup ChangeContract
        {
            get { return _changeContract; }
            set { _changeContract = value; }
        }
        /// <summary>
        /// 契约名称
        /// </summary>
        public virtual string ContractName
        {
            get { return _contractName; }
            set { _contractName = value; }
        }


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
        /// 契约编码（业务标识。格式：“QY-”+<8位时间串>）
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
        /// 工程变更说明
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
