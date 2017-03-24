using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using VirtualMachine.Core.Attributes;


namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain
{
    /// <summary>
    /// 分系统注册Domain
    /// </summary>
    [Serializable]
    [Entity]
    public class SystemRegister
    {
        private string id;
        private long version = -1;
        private int state = 1;
        private string systemName;
        private string systemCode;
        private IList dimensions = new ArrayList();
        private IList cubes = new ArrayList();

        /// <summary>
        /// 分系统的维度
        /// </summary>
        virtual public IList Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }

        /// <summary>
        /// 分系统的立方
        /// </summary>
        virtual public IList Cubes
        {
            get { return cubes; }
            set { cubes = value; }
        }

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
        ///
        /// </summary>
        virtual public int State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// 分系统注册名称
        /// </summary>
        virtual public string SystemName
        {
            get { return systemName; }
            set { systemName = value; }
        }

        /// <summary>
        /// 分系统注册编码
        /// </summary>
        virtual public string SystemCode
        {
            get { return systemCode; }
            set { systemCode = value; }
        }
    }
}
