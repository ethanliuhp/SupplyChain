using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using System.ComponentModel;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain
{
    /// <summary>
    /// PBS树
    /// </summary>
    [Serializable]
    public class PBSTree : CategoryNode
    {
        private string _ownerGUID;
        private string _ownerName;
        private string _ownerOrgSysCode;
        private string _documentModelGUID;
        private string _nodeDesc;
        private string _structTypeGUID;
        private string _structTypeName;
        private string _theProjectGUID;
        private string _theProjectName;
        private DateTime _updatedDate = DateTime.Now;
        private string tempGUID;

        /// <summary>
        /// 建筑面积
        /// </summary>
        public virtual decimal? ConstructionArea { get; set; }

        /// <summary>
        /// 临时GUID
        /// </summary>
        public virtual string TempGUID
        {
            get { return tempGUID; }
            set { tempGUID = value; }
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
        /// 管理模型文档
        /// </summary>
        public virtual string DocumentModelGUID
        {
            get { return _documentModelGUID; }
            set { _documentModelGUID = value; }
        }
        /// <summary>
        /// 节点范围描述
        /// </summary>
        public virtual string NodeDesc
        {
            get { return _nodeDesc; }
            set { _nodeDesc = value; }
        }
        /// <summary>
        /// 节点结构类型
        /// </summary>
        public virtual string StructTypeGUID
        {
            get { return _structTypeGUID; }
            set { _structTypeGUID = value; }
        }
        /// <summary>
        /// 结构类型名称
        /// </summary>
        public virtual string StructTypeName
        {
            get { return _structTypeName; }
            set { _structTypeName = value; }
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


        private string _fullPath = "";
        /// <summary>
        /// BPS节点的完整路径
        /// </summary>
        public virtual string FullPath
        {
            get { return _fullPath; }
            set { _fullPath = value; }
        }


        /// <summary>
        /// 克隆对象
        /// </summary>
        /// <returns></returns>
        public virtual PBSTree Clone()
        {
            PBSTree cat = new PBSTree();
            cat.Name = this.Name;
            //cat.Code = this.Code;//系统生成
            cat.OrderNo = this.OrderNo;

            //cat.OwnerGUID = this.OwnerGUID;
            //cat.OwnerName = this.OwnerName;
            //cat.OwnerOrgSysCode = this.OwnerOrgSysCode;
            //cat.DocumentModelGUID = this.DocumentModelGUID;
            //cat.Describe = this.Describe;

            cat.StructTypeGUID = this.StructTypeGUID;
            cat.StructTypeName = this.StructTypeName;

            cat.TheProjectGUID = this.TheProjectGUID;
            cat.TheProjectName = this.TheProjectName;

            return cat;
        }

        public virtual object Clone(string str)
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// 编码位数
        /// </summary>
        public virtual int CodeBit { get; set; }
        private PBSTemplate _template;
        /// <summary>
        /// PBS模版
        /// </summary>
        public virtual PBSTemplate Template
        {
            get
            {
                return _template;
            }
            set
            {
                _template = value;
            }
        }

    }
}
