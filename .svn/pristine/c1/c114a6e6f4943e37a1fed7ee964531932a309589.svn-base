using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using System.ComponentModel;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain
{
    /// <summary>
    /// 成本项分类
    /// </summary>
    [Serializable]
    public class CostItemCategory : CategoryNode
    {
        private DateTime _createTime = DateTime.Now;
        private DateTime _updatedDate = DateTime.Now;
        private string _summary;
        private CostItemCategoryState _categoryState;
        private string _theProjectGUID;
        private string _theProjectName;
        private CostItemCategoryTypeEnum _categoryType;

        /// <summary>
        /// 成本项分类类型
        /// </summary>
        public virtual CostItemCategoryTypeEnum CategoryType
        {
            get { return _categoryType; }
            set { _categoryType = value; }
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
        /// 最后更新时间
        /// </summary>
        public virtual DateTime UpdatedDate
        {
            get { return _updatedDate; }
            set { _updatedDate = value; }
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
        /// 分类状态
        /// </summary>
        public virtual CostItemCategoryState CategoryState
        {
            get { return _categoryState; }
            set { _categoryState = value; }
        }

        /// <summary>
        /// 克隆对象
        /// </summary>
        /// <returns></returns>
        public virtual CostItemCategory Clone()
        {
            CostItemCategory cat = new CostItemCategory();
            cat.Name = this.Name;
            cat.Code = this.Code;
            cat.OrderNo = this.OrderNo;

            cat.Describe = this.Describe;
            cat.Summary = this.Summary;

            cat.TheProjectGUID = this.TheProjectGUID;
            cat.TheProjectName = this.TheProjectName;

            return cat;
        }
    }

    /// <summary>
    /// 成本项分类状态
    /// </summary>
    public enum CostItemCategoryState
    {
        [Description("制定")]
        制定 = 1,
        [Description("发布")]
        发布 = 2,
        [Description("冻结")]
        冻结 = 3,
        [Description("作废")]
        作废 = 4
    }

        /// <summary>
    /// 成本项分类类型
    /// </summary>
    public enum CostItemCategoryTypeEnum
    {
        [Description("土建")]
        土建 = 1,
        [Description("安装")]
        安装 = 2
    }
}
