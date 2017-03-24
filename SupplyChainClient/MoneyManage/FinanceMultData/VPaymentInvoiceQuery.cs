using System;
using System.Collections;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VPaymentInvoiceQuery : TBasicDataView
    {
       private readonly MFinanceMultData mOperate = new MFinanceMultData();

       public VPaymentInvoiceQuery()
        {
            InitializeComponent();

            InitData();

            InitEvents();
        }

        private void InitData()
        {
            dgMaster.AutoGenerateColumns = false;

            datePeriodPicker1.BeginValue = DateTime.Now.AddYears(-1).Date;
            datePeriodPicker1.EndValue = DateTime.Now.Date;

            CurrentProjectInfo proj = StaticMethod.GetProjectInfo();
            if (proj != null && !proj.Code.Equals(CommonUtil.CompanyProjectCode))
            {
                txtOperationOrg.Text = proj.Name;
                txtOperationOrg.Tag = proj;
                btnOperationOrg.Visible = false;
            }
        }

        private void InitEvents()
        {
            btnExcelBill.Click += btnExcelBill_Click;
            btnOperationOrg.Click += btnOperationOrg_Click;
            btnSearchBill.Click += btnSearchBill_Click;
            dgMaster.RowPostPaint += dgMaster_RowPostPaint;
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

                FlashScreen.Show("正在查询信息......");

                IList qList = mOperate.FinanceMultDataSrv.GetPaymentInvoice(projId, orgSysCode,
                                                                            datePeriodPicker1.BeginValue,
                                                                            datePeriodPicker1.EndValue,
                                                                            txtSupply.Text,
                                                                            cmbInvoiceType.Text);
                dgMaster.DataSource = qList;

                FlashScreen.Close();
            }
            catch (Exception ex)
            {
                throw ex;
                FlashScreen.Close();
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

        private void btnExcelBill_Click(object sender, EventArgs e)
        {
            StaticMethod.ExcelClass.SaveDataGridViewToExcel(dgMaster, true);
        }

        private void dgMaster_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            dgMaster.Rows[e.RowIndex].Cells[colRowNo.Name].Value = e.RowIndex + 1;
            
            var dt = dgMaster.Rows[e.RowIndex].DataBoundItem as PaymentInvoice;
            if (dt != null)
            {
                dgMaster.Rows[e.RowIndex].Cells[colStateBill.Name].Value = ClientUtil.GetDocStateName(dt.DocState);//状态
                dgMaster.Rows[e.RowIndex].Cells[colTaxRate.Name].Value = string.Concat(dt.TaxRate, "%");//税率格式化
                dgMaster.Rows[e.RowIndex].Cells[colBHSJE.Name].Value = ClientUtil.ToDecimal(dt.SumMoney - dt.TaxMoney);
            }
        }
    }
}