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
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.BasicDomain;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInRedUI;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveInRedUI;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.StockMng;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutUI
{
    public partial class VStockOutSelectList : TBasicDataView
    {
        private MStockMng model = new MStockMng();
        IList lstSelectResult = new ArrayList();
        IList lstSelectDiagram = new ArrayList();
        IList lstSelectMaterial = new ArrayList();
        string sProfessionalCategory = string.Empty;
        public IList LstSelectMaterial
        {
            get { return lstSelectMaterial; }
        }
        public IList LstSelectResult
        {
            get { return lstSelectResult; }
        }
        public IList LstSelectDiagram
        {
            get { return lstSelectDiagram; }
        }
        public VStockOutSelectList(string sProfessionalCategory)
        {
            InitializeComponent();
            InitEvent();
            InitData();
            this.sProfessionalCategory = sProfessionalCategory;
        }

        private void InitData()
        {
            //this.dtpDateBegin.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);

            //专业分类
            VBasicDataOptr.InitProfessionCategory(cboProfessionalCategory, true);
            txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode;
            this.txtMaterialCategory.rootCatCode = "01";
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
            btnClose.Click += new EventHandler(btnClose_Click);
            btnSure.Click += new EventHandler(btnSure_Click);
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgDetail[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colCode.Name)
            {
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            #region 查询条件处理
            string condition = " and t1.projectid =  '" + projectInfo.Id + "'";

            if (StaticMethod.IsUseSQLServer())
            {
                condition += " and t1.CreateDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and t1.CreateDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
            }
            else
            {
                condition += " and t1.CreateDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
            }

            if (this.txtSupplier.Text != "" && this.txtSupplier.Result != null && this.txtSupplier.Result.Count != 0)
            {
                condition = condition + " and t1.SupplierRelation='" + (this.txtSupplier.Result[0] as SupplierRelationInfo).Id.ToString() + "'";
            }

            if (txtOriContractNo.Text != "")
            {
                condition += " and t2.OriginalContractNo like '" + txtOriContractNo.Text + "%'";
            }

            if (this.txtMaterial.Text != "" && this.txtMaterial.Result != null && this.txtMaterial.Result.Count != 0)
            {
                Material material = this.txtMaterial.Result[0] as Material;
                if (material.Name == this.txtMaterial.Text)
                {
                    condition = condition + " and t2.material='" + material.Id + "'";
                }
            }
            if (this.txtMaterial.Text != "")
            {
                condition = condition + " and t2.MaterialName like '%" + this.txtMaterial.Text + "%'";
            }
            if (this.txtSpec.Text != "")
            {
                condition = condition + " and t2.MaterialSpec like '%" + this.txtSpec.Text + "%'";
            }

            //object proCat = cboProfessionalCategory.SelectedItem;
            //if (proCat != null && !string.IsNullOrEmpty(proCat.ToString()))
            //{
            //    condition += " and t2.ProfessionalCategory='" + proCat + "'";
            //}
            if (!string.IsNullOrEmpty(sProfessionalCategory))
            {
                condition += " and t2.ProfessionalCategory='" + sProfessionalCategory + "'";
            }
            //物资分类
            if (txtMaterialCategory.Text != "" && txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
            {
                MaterialCategory materialCategory = txtMaterialCategory.Result[0] as MaterialCategory;
                condition += " and t2.materialcode like '" + materialCategory.Code + "%'";
            }
            condition += " and  t1.special='安装'  ";
            #endregion
            DataSet dataSet;
            //if (checkBox.Checked)
            //{
            dataSet = model.StockRelationSrv.StockRelationQuery(condition);
            //dataSet = model.StockRelationSrv.StockRelationSummary(condition);
            //}
            //else
            //{
            //    dataSet = model.StockRelationSrv.StockRelationQuery(condition);
            //}
            this.dgDetail.Rows.Clear();

            DataTable dataTable = dataSet.Tables[0];
            //if (dataTable == null || dataTable.Rows.Count == 0) return;
            decimal sumQuantity = 0, sumMoney = 0;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgDetail.Rows.Add();
                if (checkBox.Checked)
                {
                    colCode.Visible = false;
                    colOriContractNo.Visible = false;
                    colSupplyInfo.Visible = false;
                }
                else
                {
                    colCode.Visible = true;
                    colSpec.Visible = true;
                    colOriContractNo.Visible = true;
                    colSupplyInfo.Visible = true;
                    dgDetail[colSelect.Name, rowIndex].Tag = dataRow["MaterialID"];
                    dgDetail[colCode.Name, rowIndex].Value = dataRow["code"];
                    dgDetail[colSpec.Name, rowIndex].Value = dataRow["MaterialSpec"];
                    dgDetail[colOriContractNo.Name, rowIndex].Value = ClientUtil.ToString(dataRow["OriginalContractNo"]);
                    int stockInManner = int.Parse(dataRow["StockInManner"].ToString());
                   
                    if (stockInManner == (int)EnumStockInOutManner.收料入库 || stockInManner == (int)EnumStockInOutManner.盘盈入库)
                    {
                        dgDetail[colSupplyInfo.Name, rowIndex].Value = dataRow["SupplierRelationName"];
                    }
                    else if (stockInManner == (int)EnumStockInOutManner.调拨入库)
                    {
                        dgDetail[colSupplyInfo.Name, rowIndex].Value = dataRow["MoveOutProjectName"];
                    }
                }

                dgDetail[colMaterialCode.Name, rowIndex].Value = ClientUtil.ToString(dataRow["MaterialCode"]);
                dgDetail[colMaterialName.Name, rowIndex].Value = ClientUtil.ToString(dataRow["MaterialName"]);
                dgDetail[colSpec.Name, rowIndex].Value = ClientUtil.ToString(dataRow["MaterialSpec"]);
                dgDetail[colDiagramnumber.Name, rowIndex].Value = ClientUtil.ToString(dataRow["diagramnumber"]);
                object quantity = dataRow["RemainQuantity"];
                object money = dataRow["RemainMoney"];
                if (quantity != null)
                {
                    dgDetail[colPrice.Name, rowIndex].Value = decimal.Round(ClientUtil.TransToDecimal(money) / ClientUtil.TransToDecimal(quantity), 4);
                    sumQuantity += ClientUtil.ToDecimal(quantity);
                }
                else
                {
                    quantity = "0";
                }

                dgDetail[colQuantity.Name, rowIndex].Value = quantity;
                if (money != null)
                {
                    sumMoney += ClientUtil.ToDecimal(money);
                }
                dgDetail[colMoney.Name, rowIndex].Value = money;

                dgDetail[colIdleQuantity.Name, rowIndex].Value = ClientUtil.ToString(dataRow["IdleQuantity"]);

                //dgDetail[colProfessionalCategory.Name, rowIndex].Value = dataRow["ProfessionalCategory"];
                dgDetail[colUnit.Name, rowIndex].Value = dataRow["MatStandardUnitName"];
                dgDetail.Rows[rowIndex].Tag = dataRow;
            }

            this.txtSumQuantity.Text = sumQuantity.ToString("####.####");
            txtSumMoney.Text = sumMoney.ToString("####.####");
            lblRecordTotal.Text = "共【" + dataTable.Rows.Count + "】条记录";

            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            MessageBox.Show("查询完毕！");
        }
        //btnClose .Click +=new EventHandler(btnClose_Click);
        // btnSure .Click +=new EventHandler(btnSure_Click);
        public void btnClose_Click(object sender, EventArgs e)
        {
            lstSelectResult.Clear();
            this.Close();
        }
        public void btnSure_Click(object sender, EventArgs e)
        {
            DataGridViewRow oRow = null;
            DataGridViewCheckBoxCell oCell = null;
            DataGridViewCell oCellDiag = null;
            lstSelectResult.Clear();
            for (int i = 0; i < this.dgDetail.Rows.Count; i++)
            {
                oRow = this.dgDetail.Rows[i];
                oCell = oRow.Cells[colSelect.Name] as DataGridViewCheckBoxCell;
                oCellDiag = oRow.Cells[colDiagramnumber.Name] as DataGridViewCell;
                if (oCell.Value != null && bool.Parse(oCell.Value.ToString()))
                {
                    if (!lstSelectResult.Contains(oCell.Tag.ToString()))
                    {
                        lstSelectResult.Insert(lstSelectResult.Count, oCell.Tag.ToString());
                        IList list = model.StockOutSrv.GetMaterial(oCell.Tag.ToString());
                        if (list != null && list.Count == 1)
                        {
                            lstSelectMaterial.Insert(lstSelectMaterial.Count, list[0] as Material);
                            lstSelectDiagram.Insert(lstSelectDiagram.Count, ClientUtil.ToString(oCellDiag.Value));
                        }
                    }
                }
            }
            this.Close();
        }
    }
}
