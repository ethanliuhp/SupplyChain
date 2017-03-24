using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain
{
    [Serializable]
    [Entity]
    public class ViewStyleDimension
    {
        private string id;
        private long version = -1;
        private ViewStyleDimension parentId;
        private string name;
        private string dimUnit;
        private ViewStyle viewStyleId;
        private Iesi.Collections.Generic.ISet<ViewStyleDimension> childNodes = new Iesi.Collections.Generic.HashedSet<ViewStyleDimension>();
        private string dimCatId;
        private int orderNo;

        /// <summary>
        /// 维度分类ID
        /// </summary>
        virtual public string DimCatId
        {
            get { return dimCatId; }
            set { dimCatId = value; }
        }

        /// <summary>
        /// 子节点
        /// </summary>
        virtual public Iesi.Collections.Generic.ISet<ViewStyleDimension> ChildNodes
        {
            get { return childNodes; }
            set { childNodes = value; }
        }

        /// <summary>
        /// 视图主表ID
        /// </summary>
        virtual public ViewStyle ViewStyleId
        {
            get { return viewStyleId; }
            set { viewStyleId = value; }
        }

        /// <summary>
        /// 名称
        /// </summary>
        virtual public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 计量单位
        /// </summary>
        virtual public string DimUnit
        {
            get { return dimUnit; }
            set { dimUnit = value; }
        }

        virtual public ViewStyleDimension ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }

        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }

        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 排列顺序
        /// </summary>
        virtual public int OrderNo
        {
            get { return orderNo; }
            set { orderNo = value; }
        }
    }
}
