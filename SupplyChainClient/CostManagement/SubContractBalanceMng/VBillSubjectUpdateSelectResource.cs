using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng
{
    public partial class VBillSubjectUpdateSelectResource : TBasicDataView
    {
        public string BillCode = string.Empty;
        public bool IsOk = false;
        private MStockMng model = new MStockMng();
        CurrentProjectInfo projectInfo = null;
        public VBillSubjectUpdateSelectResource()
        {
            InitializeComponent();
            InitalData();
            IntialEvent();
        }
        public void IntialEvent()
        {
            this.btnQuery.Click += new EventHandler(btnQuery_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnSure.Click+=new EventHandler(btnSure_Click);
            this.dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
        }
        public void InitalData()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            BillCode = string.Empty;
        }
        public void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                DataGridViewColumn oColumn = dgDetail.Columns[e.ColumnIndex];
                DataGridViewRow oCurrRow = dgDetail.Rows[e.RowIndex];
                if (oColumn == colSelect)
                {
                    if (ClientUtil.ToBool(dgDetail[colSelect.Name, e.RowIndex].EditedFormattedValue))
                    {
                        List<DataGridViewRow> lstRow = dgDetail.Rows.OfType<DataGridViewRow>().Where(a => ClientUtil.ToBool(a.Cells[colSelect.Name].EditedFormattedValue)).ToList();
                        foreach (DataGridViewRow oRow in lstRow)
                        {
                            if (oRow != oCurrRow)
                            {
                                oRow.Cells[colSelect.Name].Value = false;
                            }
                        }
                    }
                }
            }
        
        }
        public void btnQuery_Click(object sender, EventArgs e)
        {
            dgDetail.Rows.Clear();
            string sCondition =  string.Format( "  and ProjectId = '{0}' and  stockoutmanner = 20 ",   projectInfo.Id  );
            if (!string.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                sCondition = string.Format("{0} and code like '%{1}%'", sCondition,txtCode.Text.Trim());
            }
            if (dtpDateBegin.Value <= dtpDateEnd.Value)
            {
                sCondition = string.Format("{0} and CreateDate between to_date('{1}','yyyy-mm-dd') and  to_date('{2}','yyyy-mm-dd') ", sCondition, dtpDateBegin.Value.ToString("yyyy-MM-dd"), dtpDateEnd.Value.ToString("yyyy-MM-dd"));
            }
            if ( txtSupplier.Result!=null && txtSupplier.Result.Count>0 &&txtSupplier.Result[0]!=null)
            {
                SupplierRelationInfo oSupplierRelationInfo = txtSupplier.Result[0] as SupplierRelationInfo;
                sCondition = string.Format(" {0} and  supplierrelation='{1}'",sCondition, oSupplierRelationInfo.Id);
            }
            DataSet ds = model.StockOutSrv.StockOutMasterQuery(sCondition);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int iRow = 0;
                foreach (DataRow oRow in ds.Tables[0].Rows)
                {// t1.id,t1.Code,t1.SupplierRelationName,t1.moveoutprojectname,t1.STATE,t1.IsLimited,
                    //t1.CreateDate,t1.CreatePersonName,t1.Descript,t1.PrintTimes,
                    // t1.SumQuantity,t1.SumMoney
                    iRow = dgDetail.Rows.Add();
                    dgDetail[colCode.Name, iRow].Value = ClientUtil.ToString(oRow["Code"]);
                    dgDetail[this.colCreateDate.Name, iRow].Value = ClientUtil.ToDateTime(oRow["CreateDate"]).ToString("yyyy-mm-dd");
                    dgDetail[this.colCreatePersonName.Name, iRow].Value = ClientUtil.ToString(oRow["CreatePersonName"]);
                    dgDetail[this.colDescript.Name, iRow].Value = ClientUtil.ToString(oRow["Descript"]);
                    dgDetail[this.colMoney.Name, iRow].Value = ClientUtil.ToDecimal(oRow["SumMoney"]);
                    dgDetail[this.colQuantity.Name, iRow].Value = ClientUtil.ToDecimal(oRow["SumQuantity"]);
                    dgDetail[this.colState.Name, iRow].Value = ClientUtil.GetDocStateName( ClientUtil.ToInt(oRow["STATE"]));
                    dgDetail[this.colSupplyInfo.Name, iRow].Value =  ClientUtil.ToString(oRow["SupplierRelationName"]);

                  
                }
            }
        }
        public void btnCancel_Click(object sender, EventArgs e)
        {
            BillCode = string.Empty;
            IsOk = false;
            this.Close();
        }
        public void btnSure_Click(object sender, EventArgs e)
        {
            DataGridViewRow oRow = dgDetail.Rows.OfType<DataGridViewRow>().FirstOrDefault(a => ClientUtil.ToBool(a.Cells[colSelect.Name].Value));
            if (oRow != null)
            {
                BillCode = ClientUtil.ToString(oRow.Cells[colCode.Name].Value);
                IsOk = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("请选择对应出库单");
            }
        }
    }
}
