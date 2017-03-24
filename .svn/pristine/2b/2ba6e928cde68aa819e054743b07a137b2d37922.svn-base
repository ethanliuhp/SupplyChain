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
namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.DocumentsApprovalMng
{
    public partial class VDocApprovalQuery : TBasicDataView
    {
       private MDocApprovalMng model = new MDocApprovalMng();

       public VDocApprovalQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }
        public void InitData()
        {
            this.cmoApprovalStyle.Items.AddRange(new object[] { "商务报表", "其他" });  
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
                    VDocApprovalMaintain vImple = new VDocApprovalMaintain();
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
            //业务日期

            if (StaticMethod.IsUseSQLServer())
            {
                condition += " and t1.CreateDate>='" + dtpCreateDateBegin.Value.Date.ToShortDateString() + "' and t1.CreateDate<'" + dtpCreateDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
            }
            else
            {
                condition += " and t1.CreateDate>=to_date('" + dtpCreateDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<to_date('" + dtpCreateDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
            }

            //文档名称
             if (this.txtChName.Text != "")
            {
                condition = condition + " and t1.ProjectName like '%" + this.txtChName.Text + "%'";
            }
           
            //责任人名称
            if (this.txtPersonCharge.Text != "")
            {
                condition = condition + " and t1.CreatePersonName like '%" + this.txtPersonCharge.Text + "%'";
            }

            //收发类型
            if (this.cmoApprovalStyle.SelectedItem != null)
            {
                condition += "and t1.ApprovalStyle = '" + cmoApprovalStyle.SelectedItem + "'";
            }
            condition += "and t1.DocState <>" + "0" + "";
            #endregion

            DataSet dataSet = model.DocApprovalMngSrv.DocApprovalQuery(condition);
            this.dgDetail.Rows.Clear();

            DataTable dataTable = dataSet.Tables[0];
            decimal sumQuantity = 0;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgDetail.Rows.Add();

                DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                currRow.Tag = ClientUtil.ToString(dataRow["Id"]);
                dgDetail[colChName.Name, rowIndex].Value = dataRow["ChName"].ToString();
                dgDetail[colPersonCharge.Name, rowIndex].Value = dataRow["CreatePersonName"].ToString();
                dgDetail[colApprovalStyle.Name, rowIndex].Value = dataRow["ApprovalStyle"].ToString();
                if (ClientUtil.ToDateTime(dataRow["RealOperationDate"]) > ClientUtil.ToDateTime("1900-01-01"))
                {
                    dgDetail[colRealOperationDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["RealOperationDate"]).ToString();     //制单时间;
                }
                object objState = dataRow["DocState"];
                if (objState != null)
                {
                    dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                }
                dgDetail[colRemark.Name, rowIndex].Value = dataRow["Remark"].ToString();
             
                if (ClientUtil.ToDateTime(dataRow["SubmitDate"]) > ClientUtil.ToDateTime("1900-01-01"))
                {
                    dgDetail[colSubmitDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["SubmitDate"]).ToString();     //提交时间;
                }
                if (ClientUtil.ToDateTime(dataRow["CreateDate"]) > ClientUtil.ToDateTime("1900-01-01"))
                {
                    dgDetail[colCreateDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["CreateDate"]).ToString();     //业务时间;
                }
            }

            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            //MessageBox.Show("查询完毕！");
        }
    }
}
