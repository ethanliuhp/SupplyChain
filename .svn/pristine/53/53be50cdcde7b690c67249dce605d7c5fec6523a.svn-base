using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain
{
    /// <summary>
    /// 立方中的维度属性Domain
    /// </summary>
    [Serializable]
    [Entity]
    public class CubeAttribute
    {
        private string id;
        private long version = -1;
        private CubeRegister cubeRegis;
        private string dimensionId;
        private string dimensionCode;
        private string dimensionName;

        /// <summary>
        /// ID
        /// </summary>
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 版本号
        /// </summary>
        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }

        /// <summary>
        /// 立方注册
        /// </summary>
        virtual public CubeRegister CubeRegis
        {
            get { return cubeRegis; }
            set { cubeRegis = value; }
        }

        /// <summary>
        /// 维度注册ID
        /// </summary>
        virtual public string DimensionId
        {
            get { return dimensionId; }
            set { dimensionId = value; }
        }

        /// <summary>
        /// 维度注册编码
        /// </summary>
        virtual public string DimensionCode
        {
            get { return dimensionCode; }
            set { dimensionCode = value; }
        }

        /// <summary>
        /// 维度注册名称
        /// </summary>
        virtual public string DimensionName
        {
            get { return dimensionName; }
            set { dimensionName = value; }
        }

    }
}
