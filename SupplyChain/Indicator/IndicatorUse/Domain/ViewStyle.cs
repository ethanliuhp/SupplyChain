using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;
using System.Collections;
using System.Linq;

namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain
{
    [Serializable]
    [Entity]
    public class ViewStyle
    {
        private string id;
        private long version = -1;
        private int rangeOrder = 0;
        private string direction;
        private ViewMain main;
        private IList details = new ArrayList();
        private string cubeAttrId;
        private string oldCatRootId;
        private string oldCatRootName;

        /// <summary>
        /// 旧根节点名称
        /// </summary>
        virtual public string OldCatRootName
        {
            get { return oldCatRootName; }
            set { oldCatRootName = value; }
        }

        /// <summary>
        /// 旧根节点ID
        /// </summary>
        virtual public string OldCatRootId
        {
            get { return oldCatRootId; }
            set { oldCatRootId = value; }
        }

        /// <summary>
        /// 立方属性ID
        /// </summary>
        virtual public string CubeAttrId
        {
            get { return cubeAttrId; }
            set { cubeAttrId = value; }
        }

        virtual public IList Details
        {
            get { return this.ReOrderStyleMx(details); }
            set { details = value; }
        }

        /// <summary>
        /// 视图主表
        /// </summary>
        virtual public ViewMain Main
        {
            get { return main; }
            set { main = value; }
        }

        /// <summary>
        /// 方向（行、列)
        /// </summary>
        virtual public string Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        /// <summary>
        /// 显示顺序
        /// </summary>
        virtual public int RangeOrder
        {
            get { return rangeOrder; }
            set { rangeOrder = value; }
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

        // <summary>
        ///重新排序已选中的格式明细
        /// </summary>
        /// <param name="checkedStyleMx">已选中的格式明细集合</param>
        private IList ReOrderStyleMx(IList checkedStyleMx)
        {
            //先排列已选中的集合顺序checkedStyleMx
            List<ViewStyleDimension> temp = new List<ViewStyleDimension>();
            
            foreach (ViewStyleDimension vsd in checkedStyleMx)
            {
                this.BubbleOrder(vsd, temp);
            }
            checkedStyleMx.Clear();
            foreach (ViewStyleDimension o in temp) checkedStyleMx.Add(o);
            return checkedStyleMx;
        }

        //冒泡算法，插入一个vsd
        private List<ViewStyleDimension> BubbleOrder(ViewStyleDimension vsd, List<ViewStyleDimension> existVsdList)
        {
            if (existVsdList.Count == 0)
            {
                existVsdList.Add(vsd);
                return existVsdList;
            }
            bool ifMax = true;//判断是否在目前最大的值
            foreach (ViewStyleDimension obj in existVsdList)
            {
                if (vsd.OrderNo < obj.OrderNo)
                {
                    existVsdList.Insert(existVsdList.IndexOf(obj), vsd);
                    ifMax = false;
                    break;
                }
            }
            if (ifMax == true)
            {
                existVsdList.Add(vsd);
            }

            return existVsdList;
        }

    }
}
