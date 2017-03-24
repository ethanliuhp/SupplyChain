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
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyOrderMng
{
    public partial class VSupplyOrderCompanySelector : TBasicDataView
    {
        public  string DetialId  { get; set; }
        private MSupplyOrderMng model = new MSupplyOrderMng();

        public VSupplyOrderCompanySelector()
        {
            InitializeComponent();
            InitialEvent();
            InitialForm();
        }

        private void InitialEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
            txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            dgDetail.SelectionChanged += new EventHandler(dgDetail_SelectionChanged);
            //pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
        }

        private void InitialForm()
        {
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
        }

        private void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        void dgDetail_SelectionChanged(object sender, EventArgs e)
        {
           
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DetialId = this.dgDetail.SelectedRows.Count > 0 ? this.dgDetail.SelectedRows[0].Tag.ToString() : null;
            this.btnOK.FindForm().Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            string  condition = string.Empty;
            condition+= "and t3.ProjectId = '"+projectInfo.Id+"'";

            //单据号
            if (this.txtCodeBegin.Text != "")
            {
                if (this.txtCodeBegin.Text == this.txtCodeEnd.Text)
                {
                    condition = condition + "and t1.Code like '%" + this.txtCodeBegin.Text + "%'";//模糊查询
                }
                else
                {
                    condition = condition + " and t1.Code between '" + this.txtCodeBegin.Text + "' and '" + this.txtCodeEnd.Text + "'";//精确查询
                }
            }

            //供应商
            if (this.txtSupplier.Text != "" && this.txtSupplier.Result != null && this.txtSupplier.Result.Count != 0)
            {
                condition = condition + " and t1.SupplierRelation='" + (this.txtSupplier.Result[0] as SupplierRelationInfo).Id.ToString() + "'";
            }

            //物资
            if (this.txtMaterial.Text != "")
            {
                condition = condition + " and t2.MaterialName like '%" + this.txtMaterial.Text + "%'";
            }

            var dataSet = model.SupplyOrderSrv.GetProjectCompanyOrderSupply(condition);

            this.dgDetail.Rows.Clear();

            DataTable dataTable = dataSet.Tables[0];
            //if (dataTable == null || dataTable.Rows.Count == 0) return;
            decimal sumQuantity = 0;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgDetail.Rows.Add();

                DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                currRow.Tag = ClientUtil.ToString(dataRow["detailId"]);
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
                dgDetail[colCreateDate.Name, rowIndex].Value = strb;//制单日期
                dgDetail[colSumMoney.Name, rowIndex].Value = dataRow["Money"].ToString();//金额
            }
        }
    }
}
