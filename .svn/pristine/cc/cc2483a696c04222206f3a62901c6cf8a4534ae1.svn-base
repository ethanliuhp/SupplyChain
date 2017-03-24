using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using System.ComponentModel;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain
{
    /// <summary>
    /// 工程任务类型树
    /// </summary>
    [Serializable]
    public class ProjectTaskTypeTree : CategoryNode
    {
        private ProjectTaskTypeLevel _typeLevel;
        private ProjectTaskTypeStandard _typeStandard;
        private string _checkRequire;

        private string _typeSummary;
        private string _summary;
        private string _projectDocTemplateGUID1;
        private string _projectDocTemplateGUID2;
        private string _projectDocTemplateGUID3;
        private DateTime _updatedDate = DateTime.Now;

        private string _theProjectGUID;
        private string _theProjectName;

        /// <summary>
        /// 工程任务类型级别
        /// </summary>
        public virtual ProjectTaskTypeLevel TypeLevel
        {
            get { return _typeLevel; }
            set { _typeLevel = value; }
        }
        /// <summary>
        /// 遵循标准
        /// </summary>
        public virtual ProjectTaskTypeStandard TypeStandard
        {
            get { return _typeStandard; }
            set { _typeStandard = value; }
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
        /// 工程任务类型摘要
        /// </summary>
        public virtual string TypeSummary
        {
            get { return _typeSummary; }
            set { _typeSummary = value; }
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
        /// 工程文档模板1
        /// </summary>
        public virtual string ProjectDocTemplateGUID1
        {
            get { return _projectDocTemplateGUID1; }
            set { _projectDocTemplateGUID1 = value; }
        }
        /// <summary>
        /// 工程文档模板2
        /// </summary>
        public virtual string ProjectDocTemplateGUID2
        {
            get { return _projectDocTemplateGUID2; }
            set { _projectDocTemplateGUID2 = value; }
        }
        /// <summary>
        /// 工程文档模板3
        /// </summary>
        public virtual string ProjectDocTemplateGUID3
        {
            get { return _projectDocTemplateGUID3; }
            set { _projectDocTemplateGUID3 = value; }
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

        private string _fullPath = "";
        /// <summary>
        /// BPS节点的完整路径（不做持久化）
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
        public virtual ProjectTaskTypeTree Clone()
        {
            ProjectTaskTypeTree cat = new ProjectTaskTypeTree();
            cat.Name = this.Name;
            //cat.Code = this.Code;
            //cat.Describe = this.Describe;
            cat.OrderNo = this.OrderNo;

            //cat.ProjectDocTemplateGUID1 = this.ProjectDocTemplateGUID1;
            //cat.ProjectDocTemplateGUID2 = this.ProjectDocTemplateGUID2;
            //cat.ProjectDocTemplateGUID3 = this.ProjectDocTemplateGUID3;

            cat.TypeLevel = this.TypeLevel;
            cat.TypeStandard = this.TypeStandard;
            cat.CheckRequire = this.CheckRequire;

            cat.TypeSummary = this.TypeSummary;
            cat.Summary = this.Summary;

            cat.TheProjectGUID = this.TheProjectGUID;
            cat.TheProjectName = this.TheProjectName;

            return cat;
        }
    }

    /// <summary>
    /// 工程任务类型级别
    /// </summary>
    public enum ProjectTaskTypeLevel
    {
        [Description("项目")]
        项目 = 0,
        [Description("专业")]
        专业 = 1,
        [Description("单位工程")]
        单位工程 = 2,
        [Description("子单位工程")]
        子单位工程 = 3,
        [Description("分部工程")]
        分部工程 = 4,
        [Description("子分部工程")]
        子分部工程 = 5,
        [Description("分项工程")]
        分项工程 = 6
    }

    /// <summary>
    /// 工程任务类型标准
    /// </summary>
    public enum ProjectTaskTypeStandard
    {
        /// <summary>
        /// 建筑工程施工质量验收统一标准
        /// </summary>
        [Description("GB50300")]
        GB50300 = 1,
        /// <summary>
        /// 企业自定义标准
        /// </summary>
        [Description("企标")]
        企标 = 2
    }
}
