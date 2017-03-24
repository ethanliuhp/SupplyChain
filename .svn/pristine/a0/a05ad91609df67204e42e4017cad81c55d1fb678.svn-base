using System;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
 
    partial class VConfirmRightReport : TBasicDataView
    {
        MCostMonthAccount model = new MCostMonthAccount();
        MIndirectCost model1 = new MIndirectCost();
        private CurrentProjectInfo projectInfo;
        DateTime startDate = new DateTime();
        DateTime endDate = new DateTime();
        public VConfirmRightReport()
        {
            InitializeComponent();
            InitData();
            InitEvents();
        }

        private void InitData()
        {
            dtpDateBegin.Value = new DateTime(ConstObject.TheLogin.LoginDate.Year, 1, 1);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;

            projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
            {

                this.lblPSelect.Visible = false;
                this.txtOperationOrg.Visible = false;
                this.btnOperationOrg.Visible = false;
            }           
        }
        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnOperationOrg.Click += new EventHandler(btnOperationOrg_Click);
        }

        void btnOperationOrg_Click(object sender, EventArgs e)
        {
            string opgId = "";
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect(true);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                OperationOrgInfo info = frm.Result[0] as OperationOrgInfo;
                txtOperationOrg.Tag = info;
                txtOperationOrg.Text = info.Name;
                opgId = info.Id;
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dataGrid, true);
        }
        
        void btnQuery_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("正在生成[项目业主报量台账]报告...");
            startDate = dtpDateBegin.Value.Date;
            endDate = dtpDateEnd.Value.Date;
            try
            {    
                OperationOrgInfo info = txtOperationOrg.Tag as OperationOrgInfo;
                IList list = null;
                if (info == null)
                {
                    list = model.CostMonthAccSrv.GetConfirmRight(startDate, endDate, projectInfo.Id, "");
                }
                else
                {
                    string selProjectID = model1.IndirectCostSvr.GetProjectIDByOperationOrg(info.Id);                    
                    if (ClientUtil.ToString(selProjectID) != "")
                    {
                        list = model.CostMonthAccSrv.GetConfirmRight(startDate, endDate, selProjectID, "");
                    }
                    else
                    {
                        list = model.CostMonthAccSrv.GetConfirmRight(startDate, endDate, "", info.SysCode);
                    }
                }
                if (list != null)
                {
                    int iRows=0;
                    int sequenceNumber = 1;
                    decimal totalExaminedMoney=0;
                    decimal fiscalCharges=0;
                    decimal busCostSure=0;
                    decimal deno=0;
                    foreach(DataDomain dd in list){
                        iRows=this.dataGrid.Rows.Add();
                        this.dataGrid.Rows[iRows].Cells[this.colSequenceNum.Name].Value = sequenceNumber+ "";
                        this.dataGrid.Rows[iRows].Cells[this.colSubmitDate.Name].Value=dd.Name1.ToString();
                        this.dataGrid.Rows[iRows].Cells[this.colSubmitMoney.Name].Value=ClientUtil.ToDecimal(dd.Name2).ToString("N2");
                        this.dataGrid.Rows[iRows].Cells[this.colExaminedDate.Name].Value=ClientUtil.ToDateTime(dd.Name3).ToShortDateString();
                        this.dataGrid.Rows[iRows].Cells[this.colCurExaminedMoney.Name].Value=ClientUtil.ToDecimal(dd.Name4).ToString("N2");
                        totalExaminedMoney=ClientUtil.ToDecimal(dd.Name5);
                        this.dataGrid.Rows[iRows].Cells[this.colTotalExaminedMoney.Name].Value=totalExaminedMoney.ToString("N2");
                        fiscalCharges=ClientUtil.ToDecimal(dd.Name6);
                        this.dataGrid.Rows[iRows].Cells[this.colFiscalCharges.Name].Value=fiscalCharges.ToString("N2");
                        busCostSure=ClientUtil.ToDecimal(dd.Name7);
                        this.dataGrid.Rows[iRows].Cells[this.colBusCostSure.Name].Value=busCostSure.ToString("N2");
                        deno=fiscalCharges>busCostSure?fiscalCharges:busCostSure;
                        this.dataGrid.Rows[iRows].Cells[this.colConfirmRightRate.Name].Value= deno == 0 ? "" : (totalExaminedMoney*100 / deno).ToString("N2");
                        this.dataGrid.Rows[iRows].Cells[this.colDescript.Name].Value=dd.Name8.ToString();
                        sequenceNumber++;
                    }
                }
            }
            catch (Exception e1)
            {
                throw new Exception("生成[票据台账]报告异常[" + e1.Message + "]");
            }
            finally
            {
                FlashScreen.Close();
            }            
        }

    }
}
