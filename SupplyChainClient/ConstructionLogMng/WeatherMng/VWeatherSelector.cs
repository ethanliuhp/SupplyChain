using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.WeatherManage.Domain;
using VirtualMachine.Core;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.WeatherMng
{
    public partial class VWeatherSelector : TBasicDataView
    {
        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        private MWeatherMng model = new MWeatherMng();
        public VWeatherSelector()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }        

        private void InitData()
        {
            this.dtpDateBegin.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }       

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnOK.Click +=new EventHandler(btnOK_Click);
            this.btnCancel.Click +=new EventHandler(btnCancel_Click);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
        }

        void btnCancel_Click(object sender,EventArgs e)
        {
            this.btnCancel.FindForm().Close(); 
        }

        void btnOK_Click(object sender,EventArgs e)
        {
            this.result.Clear();
            if (this.dgDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("没有可以引用的信息！");
                return;
            }
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) break;
                
            }
            result.Add(this.dgDetail.SelectedRows[0].Tag);
            this.btnOK.FindForm().Close();
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgDetail[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colWeather.Name)
            {
                WeatherInfo master = model.WeatherSrv.GetWeatherByCode(ClientUtil.ToString(dgvCell.Value));
                VWeatherMng vWeather = new VWeatherMng();
                vWeather.CurBillMaster = master;
                vWeather.Preview();
            }
        }
        void btnSearch_Click(object sender, EventArgs e)
        {

            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Ge("LogDate", this.dtpDateBegin.Value.Date));
            objectQuery.AddCriterion(Expression.Lt("LogDate", this.dtpDateEnd.Value.AddDays(1).Date));
            IList SearchResult = model.WeatherSrv.GetWeather(objectQuery);
            this.dgDetail.Rows.Clear();
            if (SearchResult.Count > 0 && SearchResult != null)
            {
                foreach (WeatherInfo var in SearchResult)
                {
                    int rowIndex = this.dgDetail.Rows.Add();
                    dgDetail[colCreateDate.Name, rowIndex].Value = var.CreateDate.ToShortDateString();
                    dgDetail[colHandlePerson.Name, rowIndex].Value = var.HandlePersonName;
                    dgDetail[colHumidity.Name, rowIndex].Value = var.RelativeHumidity;
                    dgDetail[colTemperature.Name, rowIndex].Value = var.Temperature;
                    dgDetail[colWeather.Name, rowIndex].Value = var.WeatherCondition;
                    string strWeek = ClientUtil.ToString(var.Week);
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
                    dgDetail[colWeek.Name, rowIndex].Value = strWeek;
                    dgDetail[colWind.Name, rowIndex].Value = var.WindDirection;
                    this.dgDetail.Rows[rowIndex].Tag = var;
                }
            }
            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }
    }
}
