using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain
{
    [Serializable]
    [Entity]
    public class ViewRuleDef
    {
        private string id;
        private long version = -1;
        private ViewMain main;
        private string cellSign;
        private string calExpress;
        private string saveExpress;
        private string displayRule;
        private string timeVar;

        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }

        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }

        /// <summary>
        /// 视图主表
        /// </summary>
        virtual public ViewMain Main
        {
            get { return main; }
            set { main = value; }
        }

        /// <summary>
        /// 单元格代号
        /// </summary>
        virtual public string CellSign
        {
            get { return cellSign; }
            set { cellSign = value; }
        }

        /// <summary>
        /// 计算表达式
        /// </summary>
        virtual public string CalExpress
        {
            get { return calExpress; }
            set { calExpress = value; }
        }

        /// <summary>
        /// 存储表达式
        /// </summary>
        virtual public string SaveExpress
        {
            get { return saveExpress; }
            set { saveExpress = value; }
        }

        /// <summary>
        /// 显示规则
        /// </summary>
        virtual public string DisplayRule
        {
            get { return displayRule; }
            set { displayRule = value; }
        }

        /// <summary>
        /// 时间变量
        /// </summary>
        virtual public string TimeVar
        {
            get { return timeVar; }
            set { timeVar = value; }
        }

       
    }
}
