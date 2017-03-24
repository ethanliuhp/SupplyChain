using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.BasicData.BaleNoMng.Domain
{
    /// <summary>
    /// 原料捆包号生成器
    /// </summary>
    [Serializable]
    [Entity]
    public class BaleNoFactory
    {
        private long _Id=-1;
        private long _Version=-1;
        private string _Prefix;
        private long _Year;
        private long _Month;
        private long _MaxNo;
        private DateTime _CreateDate=DateTime.Parse("1900-01-01");
        private string _BaleNo;
        /// <summary>
        /// 捆包号
        /// </summary>
        virtual public string BaleNo
        {
            get { return _BaleNo; }
            set { _BaleNo = value; }
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        virtual public DateTime CreateDate
        {
            get { return _CreateDate; }
            set { _CreateDate = value; }
        }

     
        /// <summary>
        /// 捆包原卷最大号
        /// </summary>
        virtual public long MaxNo
        {
            get { return _MaxNo; }
            set { _MaxNo = value; }
        }
        /// <summary>
        /// 月
        /// </summary>
        virtual public long Month
        {
            get { return _Month; }
            set { _Month = value; }
        }

        /// <summary>
        /// 年
        /// </summary>
        virtual public long Year
        {
            get { return _Year; }
            set { _Year = value; }
        }

        /// <summary>
        /// 前缀
        /// </summary>
        virtual public string Prefix
        {
            get { return _Prefix; }
            set { _Prefix = value; }
        }
        /// <summary>
        /// 版本
        /// </summary>
        virtual public long Version
        {
            get { return _Version; }
            set { _Version = value; }
        }
        /// <summary>
        /// 序号
        /// </summary>
        virtual public long Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

    }
}
