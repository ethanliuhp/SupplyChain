using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using System.ComponentModel;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireSupply.BasicDomain
{
    /// <summary>
    /// 采购管理基类主表
    /// </summary>
    [Serializable]
    public class MatHireBasicSupplyMaster : BaseMaster
    {
        private string compilation;
        private MaterialCategory materialCategory;
        private string materialCategoryName;
        private string materialCategoryCode;
        private ExecuteDemandPlanTypeEnum planType = ExecuteDemandPlanTypeEnum.滚动计划;

        /// <summary>
        /// 编制说明
        /// </summary>
        virtual public string Compilation
        {
            get { return compilation; }
            set { compilation = value; }
        }
        /// 材料类型
        /// </summary>
        virtual public MaterialCategory MaterialCategory
        {
            get { return materialCategory; }
            set { materialCategory = value; }
        }
        /// <summary>
        /// 材料类型编码
        /// </summary>
        virtual public string MaterialCategoryCode
        {
            get { return materialCategoryCode; }
            set { materialCategoryCode = value; }
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
        /// 计划类型
        /// </summary>
        virtual public ExecuteDemandPlanTypeEnum PlanType
        {
            get { return planType; }
            set { planType = value; }
        }
    }
    /// <summary>
    /// 计划类型
    /// </summary>
    public enum ExecuteDemandPlanTypeEnum
    {
        [Description("滚动计划")]
        滚动计划 = 0,
        [Description("物资计划")]
        物资计划 = 1
    }

    /// <summary>
    /// 需求计划类型
    /// </summary>
    public enum RemandPlanType
    {
        [Description("需求总计划")]
        需求总计划 = 1,
        [Description("节点需求计划")]
        节点需求计划 = 2,
        [Description("月度需求计划")]
        月度需求计划 = 3,
        [Description("日常需求计划")]
        日常需求计划 = 4,
        [Description("劳务需求计划")]
        劳务需求计划 = 5
    }

}
