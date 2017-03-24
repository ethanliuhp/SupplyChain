using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery
{
    public class Calendar
    {
        private DataTable dataTable = new DataTable(); //数据表

        public Calendar()
        {
            dataTable.Columns.Add("日", typeof(int));
            dataTable.Columns.Add("一", typeof(int));
            dataTable.Columns.Add("二", typeof(int));
            dataTable.Columns.Add("三", typeof(int));
            dataTable.Columns.Add("四", typeof(int));
            dataTable.Columns.Add("五", typeof(int));
            dataTable.Columns.Add("六", typeof(int));
        }

        /// <summary>
        /// 通过制定的年月获取该月的数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns>指定年月获取该月的数据</returns>
        public DataTable GetCalendarData(int year, int month)
        {
            dataTable.Clear();
            int day = 1, begin = 0;
            DateTime dat = new DateTime(year, month, day);
            System.DayOfWeek Week = dat.DayOfWeek;
            switch (Week)
            {
                case DayOfWeek.Sunday: begin = 0; break;
                case DayOfWeek.Monday: begin = 1; break;
                case DayOfWeek.Tuesday: begin = 2; break;
                case DayOfWeek.Wednesday: begin = 3; break;
                case DayOfWeek.Thursday: begin = 4; break;
                case DayOfWeek.Friday: begin = 5; break;
                case DayOfWeek.Saturday: begin = 6; break;
                default: begin = -1; break;
            }
            int allday = 1;      //本月总天数
            DateTime dd = dat;
            //计算指定月份的总天数
            while (dd.Month == dat.Month)
            {
                allday++;
                dd = dd.AddDays(1);
            }
            //向数据表中添加数据
            while (day < allday)
            {
                DataRow dr = dataTable.NewRow();
                while (begin < 7 && day < allday)
                {
                    dr[begin] = day;
                    day++;
                    begin++;
                }
                dataTable.Rows.Add(dr);
                begin = 0;
            }
            return dataTable;
        }
    }
}
