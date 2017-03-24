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
using Application.Business.Erp.SupplyChain.ConstructionLogManage.ConstructionManagement.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.ConstructionManagement
{
    public partial class VConstructionQuery : TBasicDataView
    {
        private MConstruction model = new MConstruction();
        public VConstructionQuery()
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
            //VBasicDataOptr.InitPostType(txtPostType, false);
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
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
        }
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = (dgDetail.Rows[e.RowIndex].Tag as ConstructionManage).Id;
                if (ClientUtil.ToString(billId) != "")
                {
                    VConstruction vOrder = new VConstruction();
                    vOrder.Start(billId);
                    vOrder.ShowDialog();
                }
            }
        }
        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgDetail[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colDate.Name)
            {
                //ConstructionManage master = model.ConstructionSrv.GetConstructionByCode(ClientUtil.ToString(dgvCell.Value));
                ConstructionManage master = this.dgDetail.CurrentRow.Tag as ConstructionManage;
                VConstruction vConstruction = new VConstruction();
                vConstruction.CurBillMaster = master;
                vConstruction.Preview();
            }
        }

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
                if (ConstObject.TheLogin.TheAccountOrgInfo != null)
                {
                    objectQuery.AddCriterion(Expression.Like("OpgSysCode", ConstObject.TheLogin.TheAccountOrgInfo.SysCode, MatchMode.Start));
                }
                objectQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value.Date));
                objectQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));
                if (txtHandlePerson.Text != "")
                {
                    objectQuery.AddCriterion (Expression.Eq("HandlePerson", txtHandlePerson.Result[0] as PersonInfo));
                    objectQuery.AddCriterion(Expression.Eq("HandlePersonName", txtHandlePerson.Value));
                }
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                if (comMngType.Text != "")
                { 
                    System.Web.UI.WebControls.ListItem li = comMngType.SelectedItem as System.Web.UI.WebControls.ListItem;
                    int values = ClientUtil.ToInt(li.Value);
                    objectQuery.AddCriterion(Expression.Eq("DocState", (DocumentState)values));
                }
                IList SearchResult = model.ConstructionSrv.GetConstruction(objectQuery);
                this.dgDetail.Rows.Clear();
                if (SearchResult.Count > 0 && SearchResult != null)
                {
                    foreach (ConstructionManage var in SearchResult)
                    {
                        int rowIndex = this.dgDetail.Rows.Add();
                        dgDetail.Rows[rowIndex].Tag = var;
                        dgDetail[colHandlePerson.Name, rowIndex].Value = var.HandlePersonName;
                        dgDetail[colHumidity.Name, rowIndex].Value = var.WeatherGlass.RelativeHumidity;
                        dgDetail[colTemperature.Name, rowIndex].Value = var.WeatherGlass.Temperature;
                        dgDetail[colWeather.Name, rowIndex].Value = var.WeatherGlass.WeatherCondition;
                        dgDetail[colWind.Name, rowIndex].Value = var.WeatherGlass.WindDirection;
                        dgDetail[colEmergency.Name, rowIndex].Value = var.Emergency;
                        dgDetail[colProductRecord.Name, rowIndex].Value = var.ProductionRecord;
                        dgDetail[colPart.Name, rowIndex].Value = var.ConstructSite;
                        dgDetail[colWorkRecord.Name, rowIndex].Value = var.WorkRecord;
                        dgDetail[colDate.Name, rowIndex].Value = var.CreateDate.ToShortDateString();
                        dgDetail[colCreateDate.Name, rowIndex].Value = var.RealOperationDate;
                        dgDetail[colCreatePerson.Name, rowIndex].Value = var.CreatePersonName;
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
