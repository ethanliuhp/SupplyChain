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
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveOutUI
{
    public partial class VSetStockRelationIdleQuantity : TBasicDataView
    {
        private MStockMng model = new MStockMng();

        public VSetStockRelationIdleQuantity()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }        

        private void InitData()
        {
            txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode;
            //pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);

            //专业分类
            VBasicDataOptr.InitProfessionCategory(cboProfessionalCategory, true);
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
            btnSetIdleQuantity.Click += new EventHandler(btnSetIdleQuantity_Click);
        }

        void btnSetIdleQuantity_Click(object sender, EventArgs e)
        {
            int updateCount = 0;
            foreach (DataGridViewRow var in dgDetail.Rows)
            {
                if (var.Tag == null || var.Tag.ToString().Equals("")) continue;
                string stockRelationId = var.Tag as string;
                decimal remainQuantity = decimal.Parse(var.Cells[colQuantity.Name].Value.ToString());
                decimal idleQuantityOfTemp = decimal.Parse(var.Cells[colIdleQuantityOfTemp.Name].Value.ToString());
                object idleQuantityObj = var.Cells[colIdleQuantity.Name].Value;
                if(idleQuantityObj==null || idleQuantityObj.ToString().Equals("")) idleQuantityObj="0";
                if(!CommonMethod.VeryValid(idleQuantityObj.ToString()))
                {
                    MessageBox.Show("请输入正确的闲置数量。");
                    dgDetail.CurrentCell = var.Cells[colIdleQuantity.Name];
                    return;
                }
                decimal idleQuantity = ClientUtil.ToDecimal(idleQuantityObj);
                if (remainQuantity < idleQuantity)
                {
                    MessageBox.Show("闲置数量不能大于库存数量。");
                    dgDetail.CurrentCell = var.Cells[colIdleQuantity.Name];
                    return;
                }
                try
                {
                    if (idleQuantity != idleQuantityOfTemp)
                    {
                        model.StockRelationSrv.SetIdleQuantity(stockRelationId, idleQuantity);
                        var.Cells[colIdleQuantityOfTemp.Name].Value = idleQuantity;
                        updateCount++;
                    }
                } catch (Exception ex)
                {
                    MessageBox.Show(string.Format("更新物资【{0} {1}】闲置数量出错。\n",var.Cells[colMaterialName.Name].Value,var.Cells[colSpec.Name].Value));
                    dgDetail.CurrentCell = var.Cells[colIdleQuantity.Name];
                    return;
                }
            }
            if (updateCount > 0)
            {
                MessageBox.Show("更新完毕。");
            }
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell=dgDetail[e.ColumnIndex, e.RowIndex];
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
            #region 查询条件处理
            string condition = " and t1.projectid = '" + StaticMethod.GetProjectInfo().Id + "'";

            if (this.txtSupplier.Text != "" && this.txtSupplier.Result != null && this.txtSupplier.Result.Count != 0)
            {
                condition = condition + " and t1.SupplierRelation='" + (this.txtSupplier.Result[0] as SupplierRelationInfo).Id.ToString()+"'";
            }

            if (txtOriContractNo.Text != "")
            {
                condition += " and t2.OriginalContractNo like '" + txtOriContractNo.Text + "%'";
            }
            //物资
            if (this.txtMaterial.Text != "")
            {
                condition = condition + " and t2.MaterialName like '%" + this.txtMaterial.Text + "%'";
            }
            if (this.txtMaterial.Text != "" && this.txtMaterial.Result != null && this.txtMaterial.Result.Count != 0)
            {
                Material material = this.txtMaterial.Result[0] as Material;
                if (material.Name == this.txtMaterial.Text)
                {
                    condition = condition + " and t2.material='" + material.Id + "'";
                }
            }
            if (this.txtSpec.Text != "")
            {
                condition = condition + " and t2.MaterialSpec like '%" + this.txtSpec.Text + "%'";
            }

            object proCat = cboProfessionalCategory.SelectedItem;
            if (proCat != null && !string.IsNullOrEmpty(proCat.ToString()))
            {
                condition += " and t2.ProfessionalCategory='" + proCat + "'";
            }
            //物资分类
            if (txtMaterialCategory.Text != "" && txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
            {
                MaterialCategory materialCategory = txtMaterialCategory.Result[0] as MaterialCategory;
                condition += " and t2.materialcode like '" + materialCategory.Code + "%'";
            }

            #endregion
            DataSet dataSet =model.StockRelationSrv.StockRelationQuery4SetIdleQuantity(condition);
            this.dgDetail.Rows.Clear();

            DataTable dataTable = dataSet.Tables[0];
            //if (dataTable == null || dataTable.Rows.Count == 0) return;
            decimal sumQuantity=0,sumMoney=0;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgDetail.Rows.Add();
                dgDetail.Rows[rowIndex].Tag = dataRow["id"];//库存ID
                dgDetail[colCode.Name, rowIndex].Value = dataRow["code"];
                dgDetail[colOriContractNo.Name, rowIndex].Value = dataRow["OriginalContractNo"];
                int stockInManner = int.Parse(dataRow["StockInManner"].ToString());
                if (stockInManner == (int)EnumStockInOutManner.收料入库 || stockInManner == (int)EnumStockInOutManner.盘盈入库)
                {
                    dgDetail[colSupplyInfo.Name, rowIndex].Value = dataRow["SupplierRelationName"];
                } else if (stockInManner == (int)EnumStockInOutManner.调拨入库)
                {
                    dgDetail[colSupplyInfo.Name, rowIndex].Value = dataRow["MoveOutProjectName"];
                }

                dgDetail[colMaterialCode.Name, rowIndex].Value = dataRow["MaterialCode"];
                dgDetail[colMaterialName.Name, rowIndex].Value = dataRow["MaterialName"];
                dgDetail[colSpec.Name, rowIndex].Value = dataRow["MaterialSpec"];

                object quantity = dataRow["RemainQuantity"];
                if (quantity != null)
                {
                    sumQuantity += ClientUtil.ToDecimal(quantity);
                }else
                {
                    quantity = "0";
                }

                dgDetail[colQuantity.Name, rowIndex].Value = quantity;
                dgDetail[colPrice.Name, rowIndex].Value = dataRow["price"];

                object money = dataRow["RemainMoney"];
                if (money != null)
                {
                    sumMoney += ClientUtil.ToDecimal(money);
                }
                dgDetail[colMoney.Name, rowIndex].Value = money;

                object idleQty = dataRow["IdleQuantity"];
                if (idleQty == null || idleQty.ToString().Equals(""))
                {
                    idleQty = "0";
                }
                dgDetail[colIdleQuantity.Name, rowIndex].Value = idleQty;
                dgDetail[colIdleQuantityOfTemp.Name, rowIndex].Value = idleQty;

                //dgDetail[colProfessionalCategory.Name, rowIndex].Value = dataRow["ProfessionalCategory"];
                dgDetail[colUnit.Name, rowIndex].Value = dataRow["MatStandardUnitName"];
            }
                
            //this.txtSumQuantity.Text = sumQuantity.ToString("####.####");
            //txtSumMoney.Text = sumMoney.ToString("####.####");
            lblRecordTotal.Text = "共【"+dataTable.Rows.Count+"】条记录";

            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            MessageBox.Show("查询完毕！");
        }
    }
}
