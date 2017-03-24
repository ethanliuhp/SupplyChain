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
using Application.Business.Erp.SupplyChain.CostManagement.OwnerQuantityMng.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.QWBS;
using Application.Business.Erp.SupplyChain.CostManagement.QWBS.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.OwnerQuantityMng
{
    public partial class VOwnerQuantitySearch : TBasicDataView
    {
        private MOwnerQuantityMng model = new MOwnerQuantityMng();

        public VOwnerQuantitySearch()
        {
            InitializeComponent();
            InitEvent();
        }        

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            btnSelect.Click +=new EventHandler(btnSelect_Click);
        }

        void btnSelect_Click(object sender,EventArgs e)
        {
            //选择清单WBS
            VQWBSSelect frm = new VQWBSSelect(new MQWBSManagement());
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                TreeNode root = frm.SelectResult[0];

                QWBSManage task = root.Tag as QWBSManage;
                if (task != null)
                {
                    this.txtCodeBegin.Text = task.TaskName;
                    this.txtCodeBegin.Tag = task;
                }
            }
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            //DataGridViewCell dgvCell = dgDetail[e.ColumnIndex, e.RowIndex];
            //if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            //if (dgvCell.OwningColumn.Name == colCode.Name)
            //{
            //    OwnerQuantityMaster master = model.OwnerQuantitySrv.GetOwnerQuantityByCode(dgvCell.Value.ToString());
            //    VOwnerQuantityMng vmro = new VOwnerQuantityMng();
            //    vmro.CurBillMaster = master;
            //    vmro.Preview();
            //}
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }
        void btnSearch_Click(object sender, EventArgs e)
        {
            string condition = "";
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            condition += "and ProjectId = '" + projectInfo.Id + "'";
            //单据号
            if (this.txtCodeBegin.Text != "")
            {
                if (this.txtCodeBegin.Text  != "")
                {
                    condition = condition + "and QWBSName like '%" + this.txtCodeBegin.Text + "%'";

                }
            }
            DataSet dataSet = model.OwnerQuantitySrv.OwnerQuantity(condition);
            this.dgDetail.Rows.Clear();

            DataTable dataTable = dataSet.Tables[0];
            decimal sumQuantity = 0;
            decimal sumMoney = 0;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgDetail.Rows.Add();
                dgDetail[colCollectSumMoney.Name, rowIndex].Value = dataRow["RealCollectionMoney"];
                dgDetail[colConfirmSumMoney.Name, rowIndex].Value = dataRow["SumConfirmMoney"];
                dgDetail[colProject.Name, rowIndex].Value = dataRow["ProjectName"];
                dgDetail[colPriceUnit.Name, rowIndex].Value = dataRow["UnitPriceName"];
                dgDetail[colSubmitSumMoney.Name, rowIndex].Value = dataRow["SumSubmitMoney"];
                string b = dataRow["LastUpdateDate"].ToString();
                string[] bArray = b.Split(' ');
                string strb = bArray[0];
                dgDetail[colLastUpdateDate.Name, rowIndex].Value = strb;
            }
            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            //MessageBox.Show("查询完毕！");
        }
    }
}
