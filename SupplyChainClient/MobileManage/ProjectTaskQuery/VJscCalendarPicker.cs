using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery
{
    public partial class VJscCalendarPicker : TBasicToolBarByMobile
    {
        private AutomaticSize automaticSize = new AutomaticSize();
        private int baseyear = 2000;
        private int basemonth = 1;
        Calendar calendar = new Calendar();
        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VJscCalendarPicker()
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            InitData();
            BindDate();
            InitEvent();
        }

        public void InitEvent()
        {
            this.cBox_Year.SelectedIndexChanged += new EventHandler(cBox_Year_SelectedIndexChanged);
            this.cBox_Month.SelectedIndexChanged += new EventHandler(cBox_Month_SelectedIndexChanged);
            this.btnLastYear.Click += new EventHandler(btnLastYear_Click);
            this.btnNextYear.Click += new EventHandler(btnNextYear_Click);
            this.btnLastMonth.Click += new EventHandler(btnLastMonth_Click);
            this.btnNextMonth.Click += new EventHandler(btnNextMonth_Click);
            this.btnReturn.Click += new EventHandler(btnReturn_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.Load += new EventHandler(VJscCalendarPicker_Load);
        }

        void VJscCalendarPicker_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        public void InitData()
        {
            List<int> year = new List<int>();
            for (int i = baseyear; i < baseyear + 100; i++)
            {
                year.Add(i);
            }
            this.cBox_Year.DataSource = year;

            List<int> month = new List<int>();
            for (int i = basemonth; i < basemonth + 12; i++)
            {
                month.Add(i);
            }
            this.cBox_Month.DataSource = month;

            DateTime newDate = DateTime.Now.Date; //获取系统当前时间
            int nyear = int.Parse(newDate.Year.ToString()); //获取当前系统的年
            int nmonth = int.Parse(newDate.Month.ToString()); //获取当前系统的月
            int nday = int.Parse(newDate.Day.ToString()); //获取当前系统的日

            this.cBox_Year.Text = Convert.ToString(nyear);
            this.cBox_Month.Text = Convert.ToString(nmonth);
            //this.DateTimeGrid.SelectedCells[0].Value = Convert.ToString(nday);

            this.DateTimeGrid.RowTemplate.Height = 51;
            this.DateTimeGrid.ColumnHeadersHeight = 58;
        }

        //年份选择事件
        void cBox_Year_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindDate();
        }

        //日期选择事件
        void cBox_Month_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindDate();
        }

        void btnLastYear_Click(object sender, EventArgs e)
        {
            decimal lastyaer = Convert.ToDecimal(this.cBox_Year.Text);
            lastyaer--;
            this.cBox_Year.Text = Convert.ToString(lastyaer);
        }

        void btnNextYear_Click(object sender, EventArgs e)
        {
            decimal nextyeat = Convert.ToDecimal(this.cBox_Year.Text);
            nextyeat++;
            this.cBox_Year.Text = Convert.ToString(nextyeat);
        }

        void btnLastMonth_Click(object sender, EventArgs e)
        {
            decimal lastmonth = Convert.ToDecimal(this.cBox_Month.Text);
            lastmonth--;
            this.cBox_Month.Text = Convert.ToString(lastmonth);
        }

        void btnNextMonth_Click(object sender, EventArgs e)
        {
            decimal nextmonth = Convert.ToDecimal(this.cBox_Month.Text);
            nextmonth++;
            this.cBox_Month.Text = Convert.ToString(nextmonth);
        }

        void btnReturn_Click(object sender, EventArgs e)
        {
            InitData();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            string year = this.cBox_Year.Text;
            string month = this.cBox_Month.Text;
            string day = DateTimeGrid.SelectedCells[0].Value.ToString();

            if (year != "" && month != "" && day != "")
            {
                string date = year + " 年" + month + "月" + day + "日";
                result.Add(date);
            }
            this.btnOK.FindForm().Close();
        }

        /// <summary>
        /// 绑定日期数据
        /// </summary>
        private void BindDate()
        {
            this.DateTimeGrid.DataSource = calendar.GetCalendarData(int.Parse(this.cBox_Year.Text), int.Parse(this.cBox_Month.Text));
            //禁止自带的排序功能
            for (int i = 0; i < DateTimeGrid.Columns.Count; i++)
            {
                DateTimeGrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
    }
}
