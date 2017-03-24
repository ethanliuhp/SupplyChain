using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain
{
    /// <summary>
    /// 月度核算过程日志
    /// </summary>
    [Serializable]
    public class CostMonthAccountLog
    {
        private string _id;
        private string _theProjectName;
        private int kjn;
        private int kjy;
        private string _accountTaskName;
        private int serialNum;
        private string descripts;
        private string logType;

        /// <summary>
        /// ID
        /// </summary>
        public virtual string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string TheProjectName
        {
            get { return _theProjectName; }
            set { _theProjectName = value; }
        }
        /// <summary>
        /// 会计年
        /// </summary>
        public virtual int Kjn
        {
            get { return kjn; }
            set { kjn = value; }
        }
        /// <summary>
        /// 会计月
        /// </summary>
        public virtual int Kjy
        {
            get { return kjy; }
            set { kjy = value; }
        }
        /// <summary>
        /// 顺序号
        /// </summary>
        public virtual int SerialNum
        {
            get { return serialNum; }
            set { serialNum = value; }
        }

        /// <summary>
        /// 核算的GWBS树节点名称
        /// </summary>
        public virtual string AccountTaskName
        {
            get { return _accountTaskName; }
            set { _accountTaskName = value; }
        }

        /// <summary>
        /// 日志的生成类型
        /// </summary>
        public virtual string LogType
        {
            get { return logType; }
            set { logType = value; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Descripts
        {
            get { return descripts; }
            set { descripts = value; }
        }
        
    }
}
