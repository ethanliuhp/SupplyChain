using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.Secure.Client.Basic.CommonClass;
using Application.Resource.CommonClass.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.Basic.Controls
{
   
        /// <summary>
        /// 账簿查询界面使用的选择会计年月的控件
        /// 使用方法和PeriodPicker相似
        /// </summary>
        public partial class AccMonthPicker : UserControl
        {
            private Login logInfo = new Login();
            public Login loginInfo
            {
                set
                {
                    logInfo = value;
                    AccMonthPicker_Load();
                }
            }

            /// <summary>
            /// 可用此方法进行数据初始化
            /// </summary>
            public void InitData()
            {
                logInfo = ConstObject.TheLogin;
                AccMonthPicker_Load();
            }

            /// <summary>
            /// 选择的会计年月
            /// </summary>
            public string Value
            {
                get { return monthbox.Text; }
                set { monthbox.Text = value; }
            }

            /// <summary>
            /// 选择的会计年
            /// </summary>
            public int AcctYear
            {
                get { return int.Parse(monthbox.Text.Substring(0, 4)); }
            }

            /// <summary>
            /// 选择的会计月
            /// </summary>
            public int AcctMonth
            {
                get { return int.Parse(monthbox.Text.Substring(5)); }
            }

            public AccMonthPicker()
            {
                InitializeComponent();
            }

            void AccMonthPicker_Load()
            {
                int startmonth = logInfo.TheComponentPeriod.InitialMonth;
                int loginyear = logInfo.LoginDate.Year;
                int loginmonth = logInfo.LoginDate.Month;

                if (logInfo.TheComponentPeriod.InitialYear < loginyear)
                    startmonth = 1;
                for (int i = startmonth; i <= loginmonth; i++)
                {
                    string itemstr = "";
                    if (i >= 10)
                        itemstr = loginyear + "." + i;
                    else
                        itemstr = loginyear + ".0" + i;
                    this.monthbox.Items.Add(itemstr);
                }
                monthbox.SelectedIndex = 0;
            }
        }
    }



