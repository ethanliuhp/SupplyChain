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
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveOutUI
{
    public partial class VStockRelationIdleQuantityQuery : TBasicDataView
    {
        private MStockMng model = new MStockMng();

        public VStockRelationIdleQuantityQuery()
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
            txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode;
            //pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);

            //专业分类
            VBasicDataOptr.InitProfessionCategory(cboProfessionalCategory, true);

            dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
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
            try
            {
                #region 查询条件处理
                string condition = "";
                FlashScreen.Show("正在查询信息......");

                if (this.txtSupplier.Text != "" && this.txtSupplier.Result != null && this.txtSupplier.Result.Count != 0)
                {
                    condition = condition + " and t1.SupplierRelation='" + (this.txtSupplier.Result[0] as SupplierRelationInfo).Id.ToString() + "'";
                }

                if (txtOriContractNo.Text != "")
                {
                    condition += " and t2.OriginalContractNo like '" + txtOriContractNo.Text + "%'";
                }

                if (this.txtMaterial.Text != "")
                {
                    condition = condition + " and t2.MaterialName like '%" + this.txtMaterial.Text + "%'";
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
                if (comMngType.Text != "")
                {
                    System.Web.UI.WebControls.ListItem li = comMngType.SelectedItem as System.Web.UI.WebControls.ListItem;
                    int values = ClientUtil.ToInt(li.Value);
                    condition += "and t1.State= '" + values + "'";
                }
                #endregion
                DataSet dataSet = model.StockRelationSrv.StockRelationQuery4IdleQuantityQuery(condition);
                this.dgDetail.Rows.Clear();

                DataTable dataTable = dataSet.Tables[0];
                //if (dataTable == null || dataTable.Rows.Count == 0) return;
                decimal sumQuantity = 0, sumMoney = 0;
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
                    }
                    else if (stockInManner == (int)EnumStockInOutManner.调拨入库)
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
                    }
                    else
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
                    dgDetail[colProjectName.Name, rowIndex].Value = dataRow["ProjectName"];

                    //dgDetail[colProfessionalCategory.Name, rowIndex].Value = dataRow["ProfessionalCategory"];
                    dgDetail[colUnit.Name, rowIndex].Value = dataRow["MatStandardUnitName"];
                }
                FlashScreen.Close();
                //this.txtSumQuantity.Text = sumQuantity.ToString("####.####");
                //txtSumMoney.Text = sumMoney.ToString("####.####");
                lblRecordTotal.Text = "共【" + dataTable.Rows.Count + "】条记录";
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
