using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain
{
    [Entity]
    [Serializable]
    public class GWBSDetailImport
    {
        private string _id;
        /// <summary>
        /// 
        /// </summary>
        public virtual string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 所在项目项目IE
        /// </summary>
        public virtual string ProjectID { get; set; }
        /// <summary>
        /// 图号
        /// </summary>
        public virtual string DiagramNumber { get; set; }

        private string _parentid;
        /// <summary>
        /// 父节点ID
        /// </summary>
        public virtual string ParentId
        {
            get { return _parentid; }
            set { _parentid = value; }
        }

        private string _name;
        /// <summary>
        /// 任务明细名称
        /// </summary>
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _code;
        /// <summary>
        /// 定额编码
        /// </summary>
        public virtual string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        private decimal? _contractartificialprice;
        /// <summary>
        /// 合同收入 人工单价
        /// </summary>
        public virtual decimal? ContractArtificialPrice
        {
            get { return _contractartificialprice; }
            set { _contractartificialprice = value; }
        }

        private decimal? _contractmaterialprice;
        /// <summary>
        /// 合同收入 材料单价
        /// </summary>
        public virtual decimal? ContractMaterialPrice
        {
            get { return _contractmaterialprice; }
            set { _contractmaterialprice = value; }
        }

        private decimal? _contractquantity;
        /// <summary>
        /// 合同收入 数量
        /// </summary>
        public virtual decimal? ContractQuantity
        {
            get { return _contractquantity; }
            set { _contractquantity = value; }
        }

        private decimal? _responsibilityartificialprice;
        /// <summary>
        /// 责任成本 人工单价
        /// </summary>
        public virtual decimal? ResponsibilityArtificialPrice
        {
            get { return _responsibilityartificialprice; }
            set { _responsibilityartificialprice = value; }
        }

        private decimal? _responsibilitymaterialprice;
        /// <summary>
        /// 责任成本 材料单价
        /// </summary>
        public virtual decimal? ResponsibilityMaterialPrice
        {
            get { return _responsibilitymaterialprice; }
            set { _responsibilitymaterialprice = value; }
        }

        private decimal? _responsibilityquantity;
        /// <summary>
        /// 责任成本 数量
        /// </summary>
        public virtual decimal? ResponsibilityQuantity
        {
            get { return _responsibilityquantity; }
            set { _responsibilityquantity = value; }
        }

        private decimal? _planartificialprice;
        /// <summary>
        /// 计划成本 人工单价
        /// </summary>
        public virtual decimal? PlanArtificialPrice
        {
            get { return _planartificialprice; }
            set { _planartificialprice = value; }
        }

        private decimal? _planmaterialprice;
        /// <summary>
        /// 计划成本 材料单价
        /// </summary>
        public virtual decimal? PlanMaterialPrice
        {
            get { return _planmaterialprice; }
            set { _planmaterialprice = value; }
        }

        private decimal? _planquantity;
        /// <summary>
        /// 计划成本 数量
        /// </summary>
        public virtual decimal? PlanQuantity
        {
            get { return _planquantity; }
            set { _planquantity = value; }
        }

        private Enum_ImportType _state;
        /// <summary>
        /// 状态【1：新建；2：已提交】
        /// </summary>
        public virtual Enum_ImportType State
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// 排序序号
        /// </summary>
        public virtual int OrderNo { get; set; }
    }

    public enum Enum_ImportType
    {
        新建 = 1,
        已提交 = 2
    }
}
