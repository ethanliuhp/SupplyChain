using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain
{
    /// <summary>
    /// 采购管理基类明细
    /// </summary>
    [Serializable]
    public class BaseSupplyDetail : BaseDetail
    {
        //private decimal refQuantity;
        private string specialType;
        private string qualityStandard;
        private string manufacturer;
        private MaterialCategory materialCategory;
        private string materialCategoryName;
        private SupplierRelationInfo usedRank;
        private string usedRankName;

        /// <summary>
        /// 专业分类
        /// </summary>
        virtual public string SpecialType
        {
            get { return specialType; }
            set { specialType = value; }
        }
        /// <summary>
        /// 质量标准
        /// </summary>
        virtual public string QualityStandard
        {
            get { return qualityStandard; }
            set { qualityStandard = value; }
        }
        /// <summary>
        /// 引用数量
        /// </summary>
        //virtual public decimal RefQuantity
        //{
        //    get { return refQuantity; }
        //    set { refQuantity = value; }
        //}
        /// <summary>
        /// 生产厂家
        /// </summary>
        virtual public string Manufacturer
        {
            get { return manufacturer; }
            set { manufacturer = value; }
        }
        /// 材料类型
        /// </summary>
        virtual public MaterialCategory MaterialCategory
        {
            get { return materialCategory; }
            set { materialCategory = value; }
        }
        /// <summary>
        /// 材料类型名称
        /// </summary>
        virtual public string MaterialCategoryName
        {
            get { return materialCategoryName; }
            set { materialCategoryName = value; }
        }
        /// <summary>
        /// 使用队伍ID
        /// </summary>
        virtual public SupplierRelationInfo UsedRank
        {
            get { return usedRank; }
            set { usedRank = value; }
        }
        /// <summary>
        /// 使用队伍名称
        /// </summary>
        virtual public string UsedRankName
        {
            get { return usedRankName; }
            set { usedRankName = value; }
        }
    }
}
