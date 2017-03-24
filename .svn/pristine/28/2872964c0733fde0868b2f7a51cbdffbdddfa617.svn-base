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
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;

namespace Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.PersonManagement
{
    public partial class VPersonManageQuery : TBasicDataView
    {
        private MPersonManage model = new MPersonManage();
        public VPersonManageQuery()
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
            VBasicDataOptr.InitPostType(txtPostType, false);
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
                string billId = (dgDetail.Rows[e.RowIndex].Tag as PersonManage).Id;
                if (ClientUtil.ToString(billId) != "")
                {
                    VPersonManage vOrder = new VPersonManage();
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
        //        PersonManage master = model.PersonManageSrv.GetPersonManageByCode(ClientUtil.ToString(dgvCell.Value));
        //        VPersonManage vPerson = new VPersonManage();
        //        vPerson.CurBillMaster = master;
        //        vPerson.Preview();
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
                if (ConstObject.TheLogin.TheAccountOrgInfo !=null)
                {
                    objectQuery.AddCriterion(Expression.Like("OpgSysCode", ConstObject.TheLogin.TheAccountOrgInfo.SysCode, MatchMode.Start));
                }
                objectQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value.Date));
                objectQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));
                if (txtHandlePerson.Text != "")
                {
                    objectQuery.AddCriterion(Expression.Eq("HandlePerson", txtHandlePerson.Result[0] as PersonInfo));
                }
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo(); 
                objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                string strType = ClientUtil.ToString(txtPostType.SelectedItem);
                if (strType != "")
                {
                    objectQuery.AddCriterion(Expression.Eq("Post", txtPostType.SelectedItem));
                }
                if (comMngType.Text != "")
                {
                    System.Web.UI.WebControls.ListItem li = comMngType.SelectedItem as System.Web.UI.WebControls.ListItem;
                    int values = ClientUtil.ToInt(li.Value);
                    objectQuery.AddCriterion(Expression.Eq("DocState", (DocumentState)values));
                }
                IList SearchResult = model.PersonManageSrv.GetPersonManage(objectQuery);
                this.dgDetail.Rows.Clear();
                if (SearchResult.Count > 0 && SearchResult != null)
                {
                    foreach (PersonManage var in SearchResult)
                    {
                        int rowIndex = this.dgDetail.Rows.Add();
                        dgDetail.Rows[rowIndex].Tag = var;
                        dgDetail[colCreateDate.Name, rowIndex].Value = var.RealOperationDate;
                        dgDetail[colHandlePerson.Name, rowIndex].Value = var.HandlePersonName;
                        dgDetail[colSate.Name, rowIndex].Value = ClientUtil.GetDocStateName((int)var.DocState);
                        if (var.WeatherGlass != null)
                        {
                            dgDetail[colHumidity.Name, rowIndex].Value = var.WeatherGlass.RelativeHumidity;
                            dgDetail[colTemperature.Name, rowIndex].Value = var.WeatherGlass.Temperature;
                            dgDetail[colWeather.Name, rowIndex].Value = var.WeatherGlass.WeatherCondition;
                            dgDetail[colWind.Name, rowIndex].Value = var.WeatherGlass.WindDirection;
                        }
                        dgDetail[colActivity.Name, rowIndex].Value = var.OtherActivities;
                        dgDetail[colMainWork.Name, rowIndex].Value = var.MainWork;
                        dgDetail[colManage.Name, rowIndex].Value = var.ProjectManage;
                        dgDetail[colPart.Name, rowIndex].Value = var.ConstructSite;
                        dgDetail[colProblem.Name, rowIndex].Value = var.Problem;
                        dgDetail[colGWType.Name, rowIndex].Value = var.Post;
                        string strWeek = ConvertDayOfWeekToZh(var.CreateDate.DayOfWeek);
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
        private string ConvertDayOfWeekToZh(System.DayOfWeek strWeek)
        {
            string DayOfWeekZh = " ";
            switch (strWeek.ToString())
            {
                case "Sunday":
                    DayOfWeekZh = "星期日";
                    break;
                case "Monday":
                    DayOfWeekZh = "星期一";
                    break;
                case "Tuesday":
                    DayOfWeekZh = " 星期二";
                    break;
                case "Wednesday":
                    DayOfWeekZh = "星期三";
                    break;
                case "Thursday":
                    DayOfWeekZh = "星期四";
                    break;
                case "Friday":
                    DayOfWeekZh = "星期五";
                    break;
                case "Saturday":
                    DayOfWeekZh = "星期六";
                    break;
            }
            return DayOfWeekZh;
        }
    }
}
