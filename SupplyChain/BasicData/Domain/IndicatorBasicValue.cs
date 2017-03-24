using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Business.Erp.SupplyChain.BasicData.Domain
{
    /// <summary>
    /// ָ��ģ��Ļ�������ֵ����
    /// </summary>
    public class IndicatorBasicValue
    {
        private string code;
        private string name;

        /// <summary>
        /// ����
        /// </summary>
        virtual public string Code
        {
            get { return code; }
            set { code = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        virtual public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
