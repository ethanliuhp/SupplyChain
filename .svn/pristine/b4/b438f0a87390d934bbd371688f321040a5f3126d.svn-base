using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AuthManager.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.Basic.Controls
{
    public partial class UCDateTimePicker : UserControl
    {
        private DateTime _value;
        private bool _isHasTime = false;
        public UCDateTimePicker()
        {
            InitializeComponent();
            InitialEvents();
        }
        void InitialEvents()
        {
            dateTimePicker1.CloseUp += new EventHandler(dateTimePicker1_ValueChanged);
            this.Leave += new EventHandler(UCDateTimePicker_Leave);

            DateTime time = DateTime.Now;
            if (_isHasTime)
            {
                _value = time;
                txtTime.Text = time.ToString();
            }
            else
            {
                _value = time.Date;
                txtTime.Text = time.ToShortDateString();
            }
            dateTimePicker1.GotFocus += new EventHandler(dateTimePicker1_GotFocus);
        }



        void dateTimePicker1_GotFocus(object sender, EventArgs e)
        {
            txtTime.Focus();
            txtTime.SelectionStart = this.txtTime.Text.Length;
        }

        void UCDateTimePicker_Leave(object sender, EventArgs e)
        {
            DateTime time = new DateTime();
            string timeStr = txtTime.Text;
            if (timeStr != "")
            {
                timeStr = timeStr.Replace('：', ':');
                timeStr = timeStr.Replace('－', '-');
                timeStr = timeStr.Replace('１', '1');
                timeStr = timeStr.Replace('２', '2');
                timeStr = timeStr.Replace('３', '3');
                timeStr = timeStr.Replace('４', '4');
                timeStr = timeStr.Replace('５', '5');
                timeStr = timeStr.Replace('６', '6');
                timeStr = timeStr.Replace('７', '7');
                timeStr = timeStr.Replace('８', '8');
                timeStr = timeStr.Replace('９', '9');
                timeStr = timeStr.Replace('０', '0');
                if (!DateTime.TryParse(timeStr, out time))
                {
                    MessageBox.Show("输入时间格式不正确!", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.txtTime.Focus();
                    return;
                }
            }
            _value = time;
        }

        void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (_isHasTime)
            {
                _value = dateTimePicker1.Value;
                txtTime.Text = _value.ToString();
            }
            else
            {
                _value = dateTimePicker1.Value.Date;
                txtTime.Text = _value.ToShortDateString();
            }
            txtTime.Focus();
            txtTime.SelectionStart = txtTime.Text.Length;
        }

        /// <summary>
        /// 时间值
        /// </summary>
        public DateTime Value
        {
            get { return _value; }
            set
            {
                if (_isHasTime)
                {
                    _value = value;
                    this.txtTime.Text = value.ToString();
                }
                else
                {
                    _value = value.Date;
                    this.txtTime.Text = value.ToShortDateString();
                }
            }
        }
        /// <summary>
        /// 获取或设置控件的文本值
        /// </summary>
        public string Text
        {
            get { return txtTime.Text; }
            set { txtTime.Text = value; }
        }
        /// <summary>
        /// 获取一个bool型值,判断是否填写了时间值
        /// </summary>
        public bool IsHasValue
        {
            get
            {
                if (string.IsNullOrEmpty(this.txtTime.Text))
                    return false;
                else
                    return true;
            }
        }
        /// <summary>
        /// 获取或设置一个bool值,确定值中是否包含时间
        /// </summary>
        public bool IsHasTime
        {
            get { return _isHasTime; }
            set { _isHasTime = value; }
        }
    }
}
