using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain
{
    /// <summary>
    /// 事实定义
    /// </summary>
    [Serializable]
    [Entity]
    public class FactDefine
    {
        private string id;
        private CubeRegister cubeRegister;
        private string factName;
        private StandardUnit standardUnit;
        private string standardUnitName;
        private int state;

        /// <summary>
        /// 状态
        /// </summary>
        public virtual int State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// 计量单位 名称
        /// </summary>
        public virtual string StandardUnitName
        {
            get { return standardUnitName; }
            set { standardUnitName = value; }
        }

        /// <summary>
        /// 计量单位
        /// </summary>
        public virtual StandardUnit StandardUnit
        {
            get { return standardUnit; }
            set { standardUnit = value; }
        }

        /// <summary>
        /// 事实名称
        /// </summary>
        public virtual string FactName
        {
            get { return factName; }
            set { factName = value; }
        }

        /// <summary>
        /// 主题 立方
        /// </summary>
        public virtual CubeRegister CubeRegister
        {
            get { return cubeRegister; }
            set { cubeRegister = value; }
        }

        /// <summary>
        ///ID 
        /// </summary>
        public virtual string Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}
