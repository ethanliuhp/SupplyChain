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
using VirtualMachine.Core;
using System.Collections;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.PersonManagement.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.ConstructionReport.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.ConstructionReport
{
    public partial class VConstructionReportQuery : TBasicDataView
    {
        private MConstructionReport model = new MConstructionReport();
        public VConstructionReportQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }        

        private void InitData()
        {
            comMngType.Items.Clear();
            Array tem = Enum.GetValues(typeof(DocumentState));
            for (int i = 0; i < tem.Length; i++)
            {
                DocumentState s = (DocumentState)tem.GetValue(i);
                int k = (int)s;
                if (k != 0)
                {
                    string strNewName = ClientUtil.GetDocStateName(k);
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                    li.Text = strNewName;
                    li.Value = k.ToString();
                    comMngType.Items.Add(li);
                }
            }
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
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            //dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
        }
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = (dgDetail.Rows[e.RowIndex].Tag as ConstructReport).Id;
                if (ClientUtil.ToString(billId) != "")
                {
                    VConstructionReport vOrder = new VConstructionReport();
                    vOrder.Start(billId);
                    vOrder.ShowDialog();
                }
            }
        }

        //void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
        //    DataGridViewCell dgvCell = dgDetail[e.ColumnIndex, e.RowIndex];
        //    if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
        //    if (dgvCell.OwningColumn.Name == colWeather.Name)
        //    {
        //        ConstructReport master = model.ConstructionReportSrv.GetConstructReportByCode(ClientUtil.ToString(dgvCell.Value));
        //        VConstructionReport vConstruction = new VConstructionReport();
        //        vConstruction.CurBillMaster = master;
        //        vConstruction.Preview();
        //    }
        //}

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                FlashScreen.Show("正在查询信息......");
                ObjectQuery objectQuery = new ObjectQuery();
                objectQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value.Date));
                objectQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));
                if (txtHandlePerson.Text != "")
                {
                    objectQuery.AddCriterion(Expression.Eq("HandlePerson", txtHandlePerson.Result[0] as PersonInfo));
                    objectQuery.AddCriterion(Expression.Eq("HandlePersonName", txtHandlePerson.Value));
                }
                if (comMngType.Text != "")
                {
                    System.Web.UI.WebControls.ListItem li = comMngType.SelectedItem as System.Web.UI.WebControls.ListItem;
                    int values = ClientUtil.ToInt(li.Value);
                    objectQuery.AddCriterion(Expression.Eq("DocState", (DocumentState)values));
                }
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                IList SearchResult = model.ConstructionReportSrv.GetConstructReport(objectQuery);
                this.dgDetail.Rows.Clear();
                if (SearchResult.Count > 0 && SearchResult != null)
                {
                    foreach (ConstructReport var in SearchResult)
                    {
                        int rowIndex = this.dgDetail.Rows.Add();
                        dgDetail.Rows[rowIndex].Tag = var;
                        dgDetail[colCreateDate.Name, rowIndex].Value = var.RealOperationDate;
                        dgDetail[colHandlePerson.Name, rowIndex].Value = var.HandlePersonName;
                        dgDetail[colHumidity.Name, rowIndex].Value = var.WeatherGlass.RelativeHumidity;
                        dgDetail[colTemperature.Name, rowIndex].Value = var.WeatherGlass.Temperature;
                        dgDetail[colWeather.Name, rowIndex].Value = var.WeatherGlass.WeatherCondition;
                        dgDetail[colWind.Name, rowIndex].Value = var.WeatherGlass.WindDirection;
                        dgDetail[colCompletionSchedule.Name, rowIndex].Value = var.CompletionSchedule;
                        dgDetail[colMaterialCase.Name, rowIndex].Value = var.MaterialCase;
                        dgDetail[colPart.Name, rowIndex].Value = var.ConstructSite;
                        dgDetail[colOtherActivities.Name, rowIndex].Value = var.OtherActivities;
                        dgDetail[colProblem.Name, rowIndex].Value = var.Problem;
                        dgDetail[colProjectManage.Name, rowIndex].Value = var.ProjectManage;
                        dgDetail[colSafetyControl.Name, rowIndex].Value = var.SafetyControl;
                        dgDetail[colCreatePerson.Name, rowIndex].Value = var.CreatePersonName;
                        dgDetail[colProject.Name, rowIndex].Value = var.ProjectName;
                        string strWeek = ClientUtil.ToString(var.WeatherGlass.Week);
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
                        dgDetail[colDate.Name, rowIndex].Value = var.CreateDate.ToShortDateString() + "    " + strWeek;
                    }
                }
                FlashScreen.Close();
                lblRecordTotal.Text = "共【" + SearchResult.Count + "】条记录";
                this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
            finally
            {
                FlashScreen.Close();
            }
        }
    }
}
