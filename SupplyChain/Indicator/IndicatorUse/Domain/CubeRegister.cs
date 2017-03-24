using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain
{
    /// <summary>
    /// 立方注册Domain
    /// </summary>
    [Serializable]
    [Entity]
    public class CubeRegister
    {
        private string id;
        private long version = -1;
        private SystemRegister sysRegister;
        private string cubeCode;
        private string cubeName;
        private IList views = new ArrayList();
        private IList cubeAttribute = new ArrayList();

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
        /// 分系统注册
        /// </summary>
        virtual public SystemRegister SysRegister
        {
            get { return sysRegister; }
            set { sysRegister = value; }
        }

        /// <summary>
        /// 立方编码
        /// </summary>
        virtual public string CubeCode
        {
            get { return cubeCode; }
            set { cubeCode = value; }
        }

        /// <summary>
        /// 立方名称
        /// </summary>
        virtual public string CubeName
        {
            get { return cubeName; }
            set { cubeName = value; }
        }

        /// <summary>
        /// 立方下的视图
        /// </summary>
        virtual public IList Views
        {
            get { return views; }
            set { views = value; }
        }

        /// <summary>
        /// 立方的维度属性
        /// </summary>
        virtual public IList CubeAttribute
        {
            get { return cubeAttribute; }
            set { cubeAttribute = value; }
        }

    }
}
