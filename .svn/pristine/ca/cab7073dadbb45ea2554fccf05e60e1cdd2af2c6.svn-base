using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ImplementationPlan
{
    public partial class VImplementationQuery : TBasicDataView
    {
        private MImplementationPlan model = new MImplementationPlan();

        public VImplementationQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData(); 
        }
        public void InitData()
        {
            VBasicDataOptr.InitImplantType(txtConstructionStyle, false);
        }
        public void InitEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
        }
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = dgDetail.Rows[e.RowIndex].Tag as string;
                if (ClientUtil.ToString(billId) != "")
                {
                    VImplementationMaintain vImple = new VImplementationMaintain();
                    vImple.Start(billId);
                    vImple.ShowDialog();
                }
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }


        void btnSearch_Click(object sender, EventArgs e)
        {
            #region 查询条件处理
            string condition = "";
            CurrentProjectInfo projectInfo = new CurrentProjectInfo();
            projectInfo = StaticMethod.GetProjectInfo();
            condition += "and t1.ProjectId = '" + projectInfo.Id + "'";
            //创建日期
            if (StaticMethod.IsUseSQLServer())
            {
                condition += " and t1.CreateDate>='" + dtpCreateDateBegin.Value.Date.ToShortDateString() + "' and t1.CreateDate<'" + dtpCreateDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
            }
            else
            {
                condition += " and t1.CreateDate>=to_date('" + dtpCreateDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<to_date('" + dtpCreateDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
            }

            //负责人
            if (!txtPersonCharge.Text.Trim().Equals("") && txtPersonCharge.Result != null)
            {
                condition = condition + " and t1.DutyOfficer='" + (txtPersonCharge.Result[0] as PersonInfo).Name + "'";
            }
            //文档名称
            if (this.txtTextName.Text != "")
            {
                condition = condition + " and t1.FileName like '%" + this.txtTextName.Text + "%'";
            }
            //结构类型
            if (this.txtConstructionStyle.SelectedItem != null)
            {
                condition += "and t1.StructType = '" + txtConstructionStyle.SelectedItem + "'";
            }
            condition += "and t1.DocState <>" + "0" + "";

            #endregion
            DataSet dataSet = model.ImplementSrv.ImplementationQuery(condition);
            this.dgDetail.Rows.Clear();
            DataTable dataTable = dataSet.Tables[0];
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgDetail.Rows.Add();
                DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                currRow.Tag = ClientUtil.ToString(dataRow["id"]);
                dgDetail[colProjectName.Name, rowIndex].Value = dataRow["ProName"].ToString();
                dgDetail[colDocumentName.Name, rowIndex].Value = dataRow["FileName"].ToString();
                dgDetail[colEnGoal.Name, rowIndex].Value = dataRow["EnGoal"].ToString();
                dgDetail[colCostObjective.Name, rowIndex].Value = dataRow["CostObjective"].ToString();
                dgDetail[colPeriodTarget.Name, rowIndex].Value = dataRow["PeriodTarget"].ToString();
                dgDetail[colObjectiveName.Name, rowIndex].Value = dataRow["ObjectiveName"].ToString();
                dgDetail[colPersonCharge.Name, rowIndex].Value = dataRow["DofficerName"].ToString();
                if (ClientUtil.ToDateTime(dataRow["RealOperationDate"]) > ClientUtil.ToDateTime("1900-01-01"))
                {
                    dgDetail[colRealOperationDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["RealOperationDate"]).ToString();     //制单时间;
                }
                string b = dataRow["CreateDate"].ToString();
                string[] bArray = b.Split(' ');
                string strb = bArray[0];
                dgDetail[colCreateDate.Name, rowIndex].Value = strb;
               //int objState = dataRow["DocState"];
               //if (objState != null)
               //{
               //    dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(dataRow["DocState"]);
               //}
            }
            //this.txtSumQuantity.Text = sumQuantity.ToString("#,###.####");
            lblRecordTotal.Text = "共【" + dataTable.Rows.Count + "】条记录";

            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            //MessageBox.Show("查询完毕！");
        }


    }
}
