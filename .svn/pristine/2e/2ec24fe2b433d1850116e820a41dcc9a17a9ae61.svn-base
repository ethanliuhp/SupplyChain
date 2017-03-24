using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using System.ComponentModel;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain
{
    /// <summary>
    /// 劳动力定额成本
    /// </summary>
    [Serializable]
    [Entity]
    public class CostWorkForce
    {
        private string _id;
        /// <summary>主键</summary>
        public virtual string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private long _version;
        /// <summary></summary>
        public virtual long Version
        {
            get { return _version; }
            set { _version = value; }
        }

        private string _code;
        /// <summary></summary>
        public virtual string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        private string _name;
        /// <summary></summary>
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _resourceTypeGUID;
        ///<summary>资源类型GUID</summary>
        public virtual string ResourceTypeGUID
        {
            set { this._resourceTypeGUID = value; }
            get { return this._resourceTypeGUID; }
        }

        private string _resourceTypeName;
        ///<summary>资源类型名称</summary>
        public virtual string ResourceTypeName
        {
            set { this._resourceTypeName = value; }
            get { return this._resourceTypeName; }
        }

        private string _resourceTypeCode;
        ///<summary>资源编码</summary>
        public virtual string ResourceTypeCode
        {
            set { this._resourceTypeCode = value; }
            get { return this._resourceTypeCode; }
        }

        private string _resourceTypeStuff;
        ///<summary>资源材料</summary>
        public virtual string ResourceTypeStuff
        {
            set { this._resourceTypeStuff = value; }
            get { return this._resourceTypeStuff; }
        }

        private string _resourceTypeSpec;
        ///<summary>资源规格</summary>
        public virtual string ResourceTypeSpec
        {
            set { this._resourceTypeSpec = value; }
            get { return this._resourceTypeSpec; }
        }

        private decimal _maxWorkdays;
        /// <summary>单位工程量所需工日上限</summary>
        public virtual decimal MaxWorkdays
        {
            get { return _maxWorkdays; }
            set { _maxWorkdays = value; }
        }

        private decimal _minWorkdays;
        /// <summary>单位工程量所需工日下限</summary>
        public virtual decimal MinWorkdays
        {
            get { return _minWorkdays; }
            set { _minWorkdays = value; }
        }

        private decimal _maxQutity;
        ///<summary>每工日完成工程量上限</summary>
        public virtual decimal MaxQutity
        {
            set { this._maxQutity = value; }
            get { return this._maxQutity; }
        }

        private decimal _minQutity;
        ///<summary>每工日完成工程量下限</summary>
        public virtual decimal MinQutity
        {
            set { this._minQutity = value; }
            get { return this._minQutity; }
        }

        //private CostAccountSubject _costAccountSubjectGuid;
        ///// <summary>成本核算科目</summary>
        //public virtual CostAccountSubject CostAccountSubjectGuid
        //{
        //    get { return _costAccountSubjectGuid; }
        //    set { _costAccountSubjectGuid = value; }
        //}

        //private string  _costAccountSubjectCode;
        /////<summary>成本核算科目名称</summary>
        //public virtual string  CostAccountSubjectCode
        //{
        //    set { this._costAccountSubjectCode = value; }
        //    get { return this._costAccountSubjectCode; }
        //}

        //private string _costAccountSubjectName;
        ///// <summary>成本核算科目名称</summary>
        //public virtual string CostAccountSubjectName
        //{
        //    get { return _costAccountSubjectName; }
        //    set { _costAccountSubjectName = value; }
        //}

        private CostItem _theCostItem;
        /// <summary> 所属成本项 </summary>
        public virtual CostItem TheCostItem
        {
            get { return _theCostItem; }
            set { _theCostItem = value; }
        }
        private CostWorkForceState _state;
        ///<summary>状态</summary>
        public virtual CostWorkForceState State
        {
            set { this._state = value; }
            get { return this._state; }
        }

        private DateTime _createTime;
        ///<summary>创建时间</summary>
        public virtual DateTime CreateTime
        {
            set { this._createTime = value; }
            get { return this._createTime; }
        }


      

    }
    /// <summary>
    /// 劳动力成本定额状态
    /// </summary>
    public enum CostWorkForceState
    {
        [Description("编制")]
        编制 = 1,
        [Description("生效")]
        生效 = 2,
        [Description("冻结")]
        冻结 = 3,
        [Description("作废")]
        作废 = 4
    }
}
