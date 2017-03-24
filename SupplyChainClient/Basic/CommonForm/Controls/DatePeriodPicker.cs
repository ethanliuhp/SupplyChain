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
    public partial class DatePeriodPicker : UserControl
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
        public DateTime BeginValue
        {
            get
            {
                return TimePickerstart.Value;
            }
            set { TimePickerstart.Value = value; }
        }

        /// <summary>
        /// 截至年月
        /// </summary>
        public DateTime EndValue
        {
            get
            {
                return TimePickerend.Value;
            }
            set { TimePickerend.Value = value; }
        }

 
        public DatePeriodPicker()
        {
            InitializeComponent();
            //this.Load += new EventHandler(PeriodPicker_Load);
            this.SizeChanged += new EventHandler(PeriodPicker_SizeChanged);

        }

        void PeriodPicker_SizeChanged(object sender, EventArgs e)
        {
            int boxwidth = (this.Size.Width - 23) / 2;
            TimePickerend.Width = boxwidth;
            TimePickerstart.Width = boxwidth;
        }

        void PeriodPicker_Load()
        {
            string mindata = logInfo.TheComponentPeriod.InitialMonth + "/1/"
             + logInfo.TheComponentPeriod.InitialYear + " 0:0:0";
            TimePickerstart.MinDate=DateTime.Parse(mindata);
            TimePickerstart.MaxDate = logInfo.LoginDate;
            if (logInfo.LoginDate.AddMonths(-1) > TimePickerstart.MinDate)
                TimePickerstart.Value = DateTime.Parse(logInfo.LoginDate.AddMonths(-1).Month + "/1/" +
                    logInfo.LoginDate.AddMonths(-1).Year + " 0:0:0");
            else
                TimePickerstart.Value = TimePickerstart.MinDate;
            TimePickerend.MinDate = this.TimePickerstart.MinDate;
            TimePickerend.MaxDate = TimePickerstart.MaxDate;
            TimePickerend.Value = TimePickerend.MaxDate;
            this.TimePickerstart.ValueChanged += new EventHandler(dtpzdrqbegin_ValueChanged);
            this.TimePickerend.ValueChanged += new EventHandler(TimePickerend_ValueChanged);
        }

        void TimePickerend_ValueChanged(object sender, EventArgs e)
        {
            if (TimePickerend.Value < TimePickerstart.Value)
            {
                MessageBox.Show("截止日期应该大于起始日期");
                TimePickerend.Value = TimePickerstart.Value;
            }
        }

        void dtpzdrqbegin_ValueChanged(object sender, EventArgs e)
        {
            if (TimePickerend.Value < TimePickerstart.Value)
            {
                TimePickerend.Value = TimePickerstart.Value;
            }
        }

    }
}
