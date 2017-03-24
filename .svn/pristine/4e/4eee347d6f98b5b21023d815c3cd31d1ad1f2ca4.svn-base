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
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using System.Collections;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using VirtualMachine.Component.Util;
using VirtualMachine.Core;
using NHibernate.Criterion;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VPaymentInvoiceMng :TMasterDetailView
    {
        private readonly MFinanceMultData mOperate = new MFinanceMultData();

        public VPaymentInvoiceMng()
        {
            InitializeComponent();

            InitData();

            InitEvents();
        }

        private void InitData()
        {
            dgMaster.AutoGenerateColumns = false;
            CurrentProjectInfo proj = StaticMethod.GetProjectInfo();
            if (proj != null && !proj.Code.Equals(CommonUtil.CompanyProjectCode))
            {
                txtOperationOrg.Text = proj.Name;
                txtOperationOrg.Tag = proj;
                btnOperationOrg.Visible = false;
            }
            this.dtCreateDate.BeginValue = ConstObject.TheLogin.LoginDate.AddYears(-1);
            this.dtCreateDate.EndValue = ConstObject.TheLogin.LoginDate;
        }

        private void InitEvents()
        {
           
            btnOperationOrg.Click += btnOperationOrg_Click;
            btnSearchBill.Click += btnSearchBill_Click;
           
            btnSave.Click+=new EventHandler(this.BtnSave_Click);
        }

        private void btnSearchBill_Click(object sender, EventArgs e)
        {
            try
            {
                string projId = string.Empty;
                string orgSysCode = string.Empty;
                if (!btnOperationOrg.Visible)
                {
                    var proj = txtOperationOrg.Tag as CurrentProjectInfo;
                    if (proj != null)
                    {
                        projId = proj.Id;
                    }
                }
                else
                {
                    var org = txtOperationOrg.Tag as OperationOrgInfo;
                    if (org == null)
                    {
                        MessageBox.Show("请选择查询范围");
                        return;
                    }

                    projId = mOperate.IndirectCostSvr.GetProjectIDByOperationOrg(org.Id);
                    if (string.IsNullOrEmpty(projId))
                    {
                        orgSysCode = org.SysCode;
                    }
                }
                ObjectQuery oQuery = new ObjectQuery();
                if (string.IsNullOrEmpty(projId))
                {
                    oQuery.AddCriterion(Expression.Like("OpgSysCode", orgSysCode, MatchMode.Start));
                }
                else
                {
                    oQuery.AddCriterion(Expression.Eq("ProjectId", projId));
                }
                if (!string.IsNullOrEmpty(this.txtCode.Text.Trim()))
                {
                    oQuery.AddCriterion(Expression.Like("Code", this.txtCode.Text.Trim(), MatchMode.Anywhere));
                }
                oQuery.AddCriterion(Expression.Between("CreateDate",dtCreateDate.BeginValue,dtCreateDate.EndValue));
                if (btnNo.Checked)
                {
                    oQuery.AddCriterion(Expression.IsNull("IfDeduction"));
                }
                else if(btnYes.Checked)
                {
                    oQuery.AddCriterion(Expression.Eq("IfDeduction", "是"));
                }
                IList qList = mOperate.FinanceMultDataSrv.GetPaymentInvoice(oQuery);
                FlashScreen.Show("正在查询信息......");
                DataBind(qList);
           
                FlashScreen.Close();
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
            }
        }
        public void DataBind(IList lstDetail)
        {
            dgMaster.Rows.Clear();
            int iRowIndex = 0;
            DataGridViewRow oRow = null;
            if (lstDetail != null && lstDetail.Count > 0)
            {
                foreach (PaymentInvoice o in lstDetail)
                {
                    iRowIndex = dgMaster.Rows.Add();
                    oRow = dgMaster.Rows[iRowIndex];
                    oRow.Tag = o;
                    oRow.Cells[this.colRowNo.Name].Value = iRowIndex + 1;
                    oRow.Cells[this.colCodeBill.Name].Value = o.Code;
                    oRow.Cells[this.colCreateDate.Name].Value = o.CreateDate.ToString();
                    oRow.Cells[this.colCreatePersonBill.Name].Value = o.CreatePersonName;
                    oRow.Cells[this.colDescriptBill.Name].Value = o.Descript;
                    oRow.Cells[this.colIfDeduction.Name].Value = o.IfDeduction;
                    oRow.Cells[this.colInvoiceCode.Name].Value = o.InvoiceCode;
                    oRow.Cells[this.colInvoiceNo.Name].Value = o.InvoiceNo;
                    oRow.Cells[this.colInvoiceType.Name].Value = o.InvoiceType;
                    oRow.Cells[this.colMoney.Name].Value = o.SumMoney.ToString("N2");
                    oRow.Cells[this.colPayOrg.Name].Value = o.TheSupplierName;
                    oRow.Cells[this.colPayType.Name].Value = o.AccountTitleName;
                    oRow.Cells[this.colProjectName.Name].Value = o.ProjectName;
                    oRow.Cells[this.colRealOperationDateBill.Name].Value = o.RealOperationDate.ToString();
                    oRow.Cells[this.colStateBill.Name].Value = ClientUtil.GetDocStateName(o.DocState);
                    oRow.Cells[this.colSupplierScale.Name].Value = o.SupplierScale;
                    oRow.Cells[this.colTaxMoney.Name].Value = o.TaxMoney.ToString("N2");
                    oRow.Cells[this.colTaxRate.Name].Value = o.TaxRate.ToString("N2");
                    oRow.Cells[this.colTransferType.Name].Value = o.TransferType;
                    oRow.Cells[this.colTransferTax.Name].Value = o.TransferTax.ToString("N2");
                     
                }
            }
        }
        private void btnOperationOrg_Click(object sender, EventArgs e)
        {
            var frm = new VCommonOperationOrgSelect(true);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                var info = frm.Result[0] as OperationOrgInfo;
                txtOperationOrg.Tag = info;
                txtOperationOrg.Text = info.Name;
            }
        }
       
        public void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                dgMaster.EndEdit();
                IList lstResult = new ArrayList();
                string sIfDeduction;
                string sTransferType;
                decimal sTransferTax;
                foreach (DataGridViewRow oRow in dgMaster.Rows)
                {
                    var dt = oRow.Tag as PaymentInvoice;
                    sIfDeduction = ClientUtil.ToString(oRow.Cells[this.colIfDeduction.Name].Value);
                    sIfDeduction = string.IsNullOrEmpty(sIfDeduction) ? null : sIfDeduction;
                    sTransferType = ClientUtil.ToString(oRow.Cells[this.colTransferType.Name].Value);
                    sTransferType = string.IsNullOrEmpty(sTransferType) ? null : sTransferType;
                    sTransferTax = ClientUtil.ToDecimal(oRow.Cells[this.colTransferTax.Name].Value);
                    if (dt != null && (!string.Equals(dt.IfDeduction, sIfDeduction) || !string.Equals(dt.TransferType, sTransferType) || dt.TransferTax != sTransferTax))
                    {
                        dt.IfDeduction = sIfDeduction;
                        dt.TransferType = sTransferType;
                        dt.TransferTax = sTransferTax;
                        lstResult.Add(dt);
                    }
                }
                if (lstResult.Count > 0)
                {
                    mOperate.FinanceMultDataSrv.SavePaymentInvoice(lstResult);
                    btnSearchBill_Click(this.btnSearchBill, e);
                }
                else
                {
                    throw new Exception("请修改后在保存。");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("保存失败:{0}",ex.Message));
            }
          
        }
        
    }
}