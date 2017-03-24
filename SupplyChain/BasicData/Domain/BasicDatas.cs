using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.BasicData.Domain
{
    /// <summary>
    /// 基础数据
    /// </summary>
    [Serializable]
    [Entity]
    public class BasicDatas
    {
        private long id = -1;
        private long version = -1;
        private string name;
        private IList basicDataDetails=new ArrayList();

        /// <summary>
        /// 明细
        /// </summary>
        virtual public IList BasicDataDetails
        {
            get { return basicDataDetails; }
            set { basicDataDetails = value; }
        }

        /// <summary>
        /// 基础表名称
        /// </summary>
        virtual public string Name
        {
            get { return name; }
            set { name = value; }
        }

        virtual public long Id
        {
            get { return id; }
            set { id = value; }
        }

        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }
    }
}
