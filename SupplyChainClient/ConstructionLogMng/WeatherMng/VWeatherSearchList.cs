using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.WeatherManage.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.WeatherMng
{
    public partial class VWeatherSearchList : SearchList
    {
        private CWeather cWeather;
        public VWeatherSearchList(CWeather cWeather)
        {
            InitializeComponent();
            this.cWeather = cWeather;
            InitDgSearch();
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            DateTime time = ClientUtil.ToDateTime(dgSearchResult.Rows[e.RowIndex].Cells["CreateDate"].Value);

            string strCode = time.ToShortDateString().Replace("-","");
            
            object Id = dgSearchResult.Rows[e.RowIndex].Cells["Id"].Value;

            cWeather.Find(strCode,ClientUtil.ToString(Id));
            
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("Id", "序列码");
            AddColumn("WeatherCondition", "天气状况");
            AddColumn("WindDirection", "风力风向");
            AddColumn("CreateDate", "日期");
            AddColumn("Temperature", "温度");
            AddColumn("RelativeHumidity", "相对湿度");
            AddColumn("Week", "星期");
            AddColumn("HandlePerson", "负责人");
            AddColumn("RealOperationDate", "制单日期");
            dgSearchResult.RowTemplate.Height = 18;
            dgSearchResult.ColumnHeadersHeight = 20;
            dgSearchResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgSearchResult.Columns["Id"].Visible = false;
            dgSearchResult.CellDoubleClick += new DataGridViewCellEventHandler(dgSearchResult_CellContentDoubleClick);
        }

        public void RefreshData(IList lst)
        {
            //从Model中取数
            dgSearchResult.Rows.Clear();
            foreach (WeatherInfo obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["WeatherCondition"].Value = obj.WeatherCondition;
                dr.Cells["WindDirection"].Value = obj.WindDirection;
                dr.Cells["CreateDate"].Value = obj.CreateDate.ToShortDateString();
                dr.Cells["Temperature"].Value = obj.Temperature;
                dr.Cells["RelativeHumidity"].Value = obj.RelativeHumidity;
                string strWeek = ClientUtil.ToString(obj.Week);
                if (strWeek.Equals("1"))
                {
                    strWeek = "星期一";
                }
                if (strWeek.Equals("2"))
                {
                    strWeek = "星期二";
                }
                if (strWeek.Equals("3"))
                {
                    strWeek = "星期三";
                }
                if (strWeek.Equals("4"))
                {
                    strWeek = "星期四";
                }
                if (strWeek.Equals("5"))
                {
                    strWeek = "星期五";
                }
                if (strWeek.Equals("6"))
                {
                    strWeek = "星期六";
                }
                if (strWeek.Equals("7"))
                {
                    strWeek = "星期日";
                }
                dr.Cells["Week"].Value = strWeek;
                dr.Cells["HandlePerson"].Value = obj.HandlePersonName;
                dr.Cells["RealOperationDate"].Value = obj.RealOperationDate.ToShortDateString();
            }
            this.ViewShow();
            this.dgSearchResult.AutoResizeColumns();
        }
        public void RemoveRow(string id)
        {
            foreach (DataGridViewRow row in this.dgSearchResult.Rows)
            {
                if (!row.IsNewRow)
                {
                    if (row.Cells["Id"].Value.ToString() == id)
                    {
                        this.dgSearchResult.Rows.Remove(row);
                        break;
                    }
                }
            }
        }
    }
}
