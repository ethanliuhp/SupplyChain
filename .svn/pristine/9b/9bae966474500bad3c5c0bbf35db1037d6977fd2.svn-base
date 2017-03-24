using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.BasicData.Domain
{
    [Serializable]
    [Entity]
    public class BasicDataDetail
    {
        private long id = -1;
        private long version = -1;
        private string code;
        private string name;
        private BasicDatas basicData;

        virtual public BasicDatas BasicData
        {
            get { return basicData; }
            set { basicData = value; }
        }

        /// <summary>
        /// ±àÂë
        /// </summary>
        virtual public string Code
        {
            get { return code; }
            set { code = value; }
        }

        /// <summary>
        /// Ãû³Æ
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
