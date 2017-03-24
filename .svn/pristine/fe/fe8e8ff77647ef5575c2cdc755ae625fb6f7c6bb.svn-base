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
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalLedgerMng
{
    public partial class VMaterialRentalLedgerQuery : TBasicDataView
    {
        MMatRentalMng model = new MMatRentalMng();
        public VMaterialRentalLedgerQuery()
        {
            InitializeComponent();
            this.InitData();
            this.InitEvent();
        }

        private void InitData()
        {
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-30);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            this.txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode1;
            txtUsedBank.SupplierCatCode = CommonUtil.SupplierCatCode3;
            this.txtMaterial.materialCatCode = CommonUtil.TurnMaterialMatCode;
        }
        private void InitEvent()
        {
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
        }


        void btnSearch_Click(object sender, EventArgs e)
        {
            #region 定义查询条件
            string condition = "";
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            condition += " ProjectId = '" + projectInfo.Id + "'";
            if (StaticMethod.IsUseSQLServer())
            {
                condition += " and SystemDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and  SystemDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
            }
            else
            {
                condition += " and SystemDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and  SystemDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
            }

            if (this.txtSupplier.Text != "" && this.txtSupplier.Result != null && this.txtSupplier.Result.Count != 0)
            {
                condition = condition + " and  SupplierRelation='" + (this.txtSupplier.Result[0] as SupplierRelationInfo).Id.ToString() + "'";
            }

            if (this.txtUsedBank.Text != "" && this.txtUsedBank.Result != null && this.txtUsedBank.Result.Count != 0)
            {
                condition = condition + " and  TheRank='" + (this.txtUsedBank.Result[0] as SupplierRelationInfo).Id.ToString() + "'";
            }

            if (txtOriContractNo.Text != "")
            {
                condition += " and  OldContractNum like '" + txtOriContractNo.Text + "%'";
            }

            if (this.txtMaterial.Text != "")
            {
                condition = condition + " and MaterialName like '%" + this.txtMaterial.Text + "%'";
            }
            if (this.txtSpec.Text != "")
            {
                condition = condition + " and MaterialSpec like '%" + this.txtSpec.Text + "%'";
            }
            #endregion

            DataSet dataSet = model.MatMngSrv.GetMaterialRentalLedgerByCondition(condition);
            this.dgDetail.Rows.Clear();

            DataTable dataTable = dataSet.Tables[0];
            //if (dataTable == null || dataTable.Rows.Count == 0) return;
            decimal sumCollQuantity = 0;
            decimal sumReturnQuantity = 0;
            DateTime dateTime;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgDetail.Rows.Add();

                int washType = Convert.ToInt32(dataRow["WashType"]);
                if (washType == 0)
                {
                    object collQuantity = dataRow["CollectionQuantity"];
                    if (collQuantity != null)
                    {
                        decimal collQty = ClientUtil.ToDecimal(collQuantity);
                        //收料数量为负数: 代表为退料
                        if (collQty < 0)
                        {
                            sumReturnQuantity += Math.Abs(collQty);
                        }
                        else
                        {
                            sumCollQuantity += Math.Abs(collQty);
                        }
                    }
                    dgDetail[colType.Name, rowIndex].Value = "收料单";
                    dgDetail[colQuantity.Name, rowIndex].Value = collQuantity;
                }
                else if (washType == 1)
                {
                    object returnQuantity = dataRow["ReturnQuantity"];
                    if (returnQuantity != null)
                    {
                        decimal returnQty = ClientUtil.ToDecimal(returnQuantity);
                        //退料数量为负数：代表为收料
                        if (returnQty < 0)
                        {
                            sumCollQuantity += Math.Abs(returnQty);
                        }
                        else
                        {
                            sumReturnQuantity += Math.Abs(returnQty);
                        }
                    }
                    dgDetail[colType.Name, rowIndex].Value = "退料单";
                    dgDetail[colQuantity.Name, rowIndex].Value = returnQuantity;
                }

                dgDetail[colOperDate.Name, rowIndex].Value = ClientUtil.ToDateTime((dataRow["RealOperationDate"])).ToShortDateString();
                dgDetail[colOriContractNo.Name, rowIndex].Value = dataRow["OldContractNum"];
                dgDetail[colSupplyInfo.Name, rowIndex].Value = dataRow["SupplierName"];
                dgDetail[colCode.Name, rowIndex].Value = dataRow["BillCode"];
                dgDetail[colMaterialCode.Name, rowIndex].Value = dataRow["MaterialCode"];
                dgDetail[colMaterialName.Name, rowIndex].Value = dataRow["MaterialName"];
                dgDetail[colSpec.Name, rowIndex].Value = dataRow["MaterialSpec"];
                dgDetail[colRankName.Name, rowIndex].Value = dataRow["TheRankName"];
                dgDetail[colLeftQuantity.Name, rowIndex].Value = dataRow["LeftQuantity"];

                dgDetail[colCreateDate.Name, rowIndex].Value = (dataRow["SystemDate"]);
                dgDetail[colPrice.Name, rowIndex].Value = dataRow["RentalPrice"];
                dgDetail[colUnit.Name, rowIndex].Value = dataRow["MatStandardUnitName"];
            }
            this.txtSumReturnQuantity.Text = sumReturnQuantity.ToString("#,###.####");
            this.txtSumCollQuantity.Text = sumCollQuantity.ToString("#,###.####");
            lblRecordTotal.Text = "共【" + dataTable.Rows.Count + "】条记录";

            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            MessageBox.Show("查询完毕！");
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }
    }
}
