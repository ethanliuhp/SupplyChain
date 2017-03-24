using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.Secure.GlobalInfo;
using Application.Business.Erp.Secure.Client.Basic.CommonClass;
using Application.Resource.CommonClass.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.Basic.Controls
{
    /// <summary>
    /// 账簿查询界面使用的选择会计年月的控件
    /// 例子见　Application.Business.Erp.Financial.Client.FinanceUI.AccountBook.GeneralAccBook.VGeneralAccQuery
    /// </summary>
    public partial class PeriodPicker : UserControl
    {
        private Login logInfo = new Login();
        public Login loginInfo
        {
            set
            {
                logInfo = value;
                PeriodPicker_Load();
            }
        }

        private bool theSameYear = false;
        public bool TheSameYear
        {
            get { return theSameYear; }
            set { theSameYear = value; }
        }
        /// <summary>
        /// 可用此方法进行数据初始化
        /// </summary>
        public void InitData()
        {
            logInfo = ConstObject.TheLogin;
            PeriodPicker_Load();
        }

        /// <summary>
        /// 起始年月
        /// </summary>
        public string BeginValue
        {
            get
            {
                if (monthstart.Text == null || monthstart.Text.Equals(""))
                    return "";
                else
                    return monthstart.Text.Substring(0, 4) + monthstart.Text.Substring(5);
            }
            set { monthstart.Text = value; }
        }

        /// <summary>
        /// 截至年月
        /// </summary>
        public string EndValue
        {
            get
            {
                if (monthend.Text == null || monthend.Text.Equals(""))
                    return "";
                else
                    return monthend.Text.Substring(0, 4) + monthend.Text.Substring(5);
            }
            set { monthend.Text = value; }
        }

        /// <summary>
        /// 起始年
        /// </summary>
        public int BeginYear
        {
            get { return int.Parse(monthstart.Text.Substring(0, 4)); }
        }

        /// <summary>
        /// 起始月
        /// </summary>
        public int BeginMonth
        {
            get { return int.Parse(monthstart.Text.Substring(5)); }
        }

        /// <summary>
        /// 截至年
        /// </summary>
        public int EndYear
        {
            get { return int.Parse(monthend.Text.Substring(0, 4)); }
        }

        /// <summary>
        /// 截至月
        /// </summary>
        public int EndMonth
        {
            get { return int.Parse(monthend.Text.Substring(5)); }
        }

        public PeriodPicker()
        {
            InitializeComponent();
            //this.Load += new EventHandler(PeriodPicker_Load);
            this.SizeChanged += new EventHandler(PeriodPicker_SizeChanged);

        }

        void PeriodPicker_SizeChanged(object sender, EventArgs e)
        {
            int boxwidth = (this.Size.Width - 30) / 2;
            monthend.Width = boxwidth;
            monthstart.Width = boxwidth;
        }

        void PeriodPicker_Load()
        {
            int inityear = logInfo.TheComponentPeriod.InitialYear;
            int initmonth = logInfo.TheComponentPeriod.InitialMonth;
            int loginyear = logInfo.LoginDate.Year;
            int loginmonth = logInfo.LoginDate.Month;
            if (inityear == loginyear)
            {
                for (int i = initmonth; i <= loginmonth; i++)
                {
                    string itemstr = "";
                    if (i >= 10)
                        itemstr = inityear + "." + i;
                    else
                        itemstr = inityear + ".0" + i;
                    monthstart.Items.Add(itemstr);
                    monthend.Items.Add(itemstr);
                }
            }
            else if (inityear < loginyear)
            {
                if (inityear < loginyear - 1)
                {
                    initmonth = 1;
                    inityear = loginyear - 1;
                }
                if (!TheSameYear)
                {
                    for (int i = initmonth; i <= 12; i++)
                    {
                        string itemstr = "";
                        if (i >= 10)
                            itemstr = inityear + "." + i;
                        else
                            itemstr = inityear + ".0" + i;
                        monthstart.Items.Add(itemstr);
                    }
                }
                for (int i = 0; i <= loginmonth; i++)
                {
                    string itemstr = "";
                    if (i >= 10)
                        itemstr = loginyear + "." + i;
                    else
                        itemstr = loginyear + ".0" + i;
                    monthstart.Items.Add(itemstr);
                    monthend.Items.Add(itemstr);
                }
            }
            //if(monthstart.Text.Equals(""))
            monthstart.SelectedIndex = monthend.Items.Count - 1;
            //if(monthend.Text.Equals(""))
            monthend.SelectedIndex = monthend.Items.Count - 1;
            monthstart.TextChanged += new EventHandler(monthstart_TextChanged);
            //monthend.TextChanged += new EventHandler(monthend_TextChanged);
            monthend.SelectedIndexChanged += new EventHandler(monthend_SelectedIndexChanged);
        }

        void monthend_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(monthstart.Text.Replace('.', '0')) > int.Parse(monthend.Text.Replace('.', '0')))
            {
                MessageBox.Show("截至日期应该大于起始日期！");
                monthend.Text = monthstart.Text;
            }
        }

        void monthend_TextChanged(object sender, EventArgs e)
        {
            object o = monthend.SelectedItem;
            string s = monthend.SelectedText;
            object v = monthend.SelectedValue;
            int i = monthend.SelectedIndex;
            if (int.Parse(monthstart.Text.Replace('.', '0')) > int.Parse(monthend.Text.Replace('.', '0')))
            {
                monthend.Text = monthstart.Text;
                MessageBox.Show("截至日期应该大于起始日期！");
            }
        }

        void monthstart_TextChanged(object sender, EventArgs e)
        {
            if (!monthstart.Text.Equals("") && (monthend.Text.Equals("") ||
                int.Parse(monthstart.Text.Replace('.', '0')) > int.Parse(monthend.Text.Replace('.', '0'))))
            {
                monthend.Text = monthstart.Text;
            }
        }

    }
}
