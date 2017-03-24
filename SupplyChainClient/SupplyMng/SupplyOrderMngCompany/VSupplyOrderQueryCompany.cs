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
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyOrderMngCompany
{
    public partial class VSupplyOrderQueryCompany : TBasicDataView
    {
        private MSupplyOrderMng model = new MSupplyOrderMng();

        public VSupplyOrderQueryCompany()
        {
            InitializeComponent();
            InitEvent();
            InitData();
            InitZYFL();
        }

        private void InitZYFL()
        {
            //添加专业分类下拉框
            VBasicDataOptr.InitProfessionCategory(comSpecailType, false);
            //comSpecailType.ContextMenuStrip = cmsDg;
        }

        private void InitData()
        {
            //this.dtpDateBegin.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
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
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);

        }

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = dgDetail.Rows[e.RowIndex].Tag as string;
                if (ClientUtil.ToString(billId) != "")
                {
                    VSupplyOrderCompany vOrder = new VSupplyOrderCompany();
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
            if (dgvCell.OwningColumn.Name == colCode.Name)
            {
                SupplyOrderMaster master = model.SupplyOrderSrv.GetSupplyOrderByCode(dgvCell.Value.ToString());
                VSupplyOrderCompany vmro = new VSupplyOrderCompany();
                vmro.CurBillMaster = master;
                vmro.Preview();
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            #region 查询条件处理
            string condition = "";
            //CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            condition += "and t1.ProjectId is null";
            //单据号
            if (this.txtCodeBegin.Text != "")
            {
                if (this.txtCodeBegin.Text == this.txtCodeEnd.Text)
                {
                    condition = condition + "and t1.Code like '%"+ this.txtCodeBegin.Text +"%'";//模糊查询
                }
                else
                {
                    condition = condition + " and t1.Code between '" + this.txtCodeBegin.Text + "' and '" + this.txtCodeEnd.Text + "'";//精确查询
                }
            }
            //制单日期
            if (StaticMethod.IsUseSQLServer())
            {
                condition += " and t1.CreateDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and t1.CreateDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
            }
            else
            {
                condition += " and t1.CreateDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
            }
            //负责人
            if (!txtHandlePerson.Text.Trim().Equals("") && txtHandlePerson.Result != null)
            {
                condition = condition + " and t1.handleperson='" + (txtHandlePerson.Result[0] as PersonInfo).Id + "'";
            }
            //制单人
            if (!txtCreatePerson.Text.Trim().Equals("") && txtCreatePerson.Result != null && txtCreatePerson.Result.Count > 0)
            {
                condition += " and t1.CreatePerson='" + (txtCreatePerson.Result[0] as PersonInfo).Id + "'";
            }

            //会计月
            if (!txtAccountMonth.Text.Trim().Equals(""))
            {
                condition += "and t1.CreateMonth = '"+ txtAccountMonth.Text +"'";
            }

            //会计年
            if (!txtAccountYear.Text.Trim().Equals(""))
            {
                condition += "and t1.CreateYear = '"+ txtAccountYear.Text +"'";
            }

            //供应商
            if (this.txtSupplier.Text != "" && this.txtSupplier.Result != null && this.txtSupplier.Result.Count != 0)
            {
                condition = condition + " and t1.SupplierRelation='" + (this.txtSupplier.Result[0] as SupplierRelationInfo).Id.ToString() + "'";
            }

            //采购合同号
            if (!this.txtSupplyNum.Text.Trim().Equals(""))
            {
                condition += "and t1.OldContractNum like '%"+ txtSupplyNum.Text +"%'";
            }

            //专业分类
            if (this.comSpecailType.SelectedItem != null)
            {
                condition += "and t2.SpecialType = '" + comSpecailType.SelectedItem + "'";
            }

            //物资
            if (this.txtMaterial.Text != "")
            {
                condition = condition + " and t2.MaterialName like '%" + this.txtMaterial.Text + "%'";
            }
            //规格型号
            if (this.txtMaterialSpec.Text != "")
            {
                condition = condition + " and t2.MaterialSpec like '%" + this.txtMaterialSpec.Text + "%'";
            }
            #endregion
            DataSet dataSet = model.SupplyOrderSrv.SupplyOrderQueryCompany(condition);
            this.dgDetail.Rows.Clear();

            DataTable dataTable = dataSet.Tables[0];
            //if (dataTable == null || dataTable.Rows.Count == 0) return;
            decimal sumQuantity = 0;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgDetail.Rows.Add();

                DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                currRow.Tag = ClientUtil.ToString(dataRow["id"]);
                dgDetail[colCode.Name, rowIndex].Value = dataRow["code"];
                
                object objState = dataRow["State"];
                if (objState != null)
                {
                    dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                }
                dgDetail[colMaterialCode.Name, rowIndex].Value = dataRow["MaterialCode"].ToString();//物资编码
                dgDetail[colMaterialName.Name, rowIndex].Value = dataRow["MaterialName"].ToString();//物资名称
                dgDetail[colSpec.Name, rowIndex].Value = dataRow["MaterialSpec"].ToString(); //规格型号
                dgDetail[colUnit.Name, rowIndex].Value = dataRow["MatStandardUnitName"].ToString();//计量单位
                object quantity = dataRow["Quantity"];//总数量
                if (quantity != null)
                {
                    sumQuantity += decimal.Parse(quantity.ToString());
                }
                dgDetail[colQuantity.Name, rowIndex].Value = quantity;
                dgDetail[colPrice.Name, rowIndex].Value = dataRow["SupplyPrice"].ToString();//采购单价
                dgDetail[colSupplier.Name, rowIndex].Value = dataRow["SupplierName"].ToString();//供应商
                dgDetail[colSupplyContractNum.Name, rowIndex].Value = dataRow["OldContractNum"].ToString();//采购合同号
                dgDetail[colDescript.Name, rowIndex].Value = dataRow["Descript"].ToString();//备注
                dgDetail[colCreatePerson.Name, rowIndex].Value = dataRow["CreatePersonName"].ToString();//制单人
                string b = dataRow["CreateDate"].ToString();
                string[] bArray = b.Split(' ');
                string strb = bArray[0];
                this.txtSumMoney.Text = ClientUtil.ToString(dataRow["ContractMoney"]);//总金额/合约金额
                dgDetail[colCreateDate.Name, rowIndex].Value = strb;//制单日期
                dgDetail[colSumMoney.Name, rowIndex].Value = dataRow["Money"].ToString();//金额
            }

            this.txtSumQuantity.Text = sumQuantity.ToString("#,###.####");
            lblRecordTotal.Text = "共【" + dataTable.Rows.Count + "】条记录";

            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            MessageBox.Show("查询完毕！");
        }
    }
}
