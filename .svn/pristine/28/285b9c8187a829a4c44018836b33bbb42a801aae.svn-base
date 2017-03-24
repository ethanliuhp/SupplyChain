using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain
{
    /// <summary>
    /// 立方数据Domain，此Domain并不对于数据库中具体的表
    /// </summary>
    [Serializable]
    [Entity]
    public class CubeData
    {
        private IList dimDataList = new ArrayList();//数据值集合
        private IList dimCodeList = new ArrayList();//维度值对应的编码集合，对应数据值的顺序
        private double result = 0;//结果值
        private string plan;//计划值或评估值等
        private double sonValue = 0;//子项
        private double motherValue = 0;//母项
        private string id;
        private int mensureIndex = -1;//度量值在集合中的位置

        /// <summary>
        /// ID
        /// </summary>
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// mensureIndex
        /// </summary>
        virtual public int MensureIndex
        {
            get { return mensureIndex; }
            set { mensureIndex = value; }
        }

        /// <summary>
        /// 维度值对应的编码集合,例如：{"ywzz","time","indicator"}
        /// </summary>
        virtual public IList DimCodeList
        {
            get { return dimCodeList; }
            set { dimCodeList = value; }
        }

        /// <summary>
        /// 维度值的集合,顺序的组合根据dimNameList的顺序
        /// </summary>
        virtual public IList DimDataList
        {
            get { return dimDataList; }
            set { dimDataList = value; }
        }

        /// <summary>
        /// 结果值
        /// </summary>
        virtual public double Result
        {
            get { return result; }
            set { result = value; }
        }

        /// <summary>
        /// 计划值
        /// </summary>
        virtual public string Plan
        {
            get { return plan; }
            set { plan = value; }
        }

        /// <summary>
        /// 子项
        /// </summary>
        virtual public double SonValue
        {
            get { return sonValue; }
            set { sonValue = value; }
        }

        /// <summary>
        /// 母项
        /// </summary>
        virtual public double MotherValue
        {
            get { return motherValue; }
            set { motherValue = value; }
        }


    }
}
