using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.Util;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost
{
    public partial class VAccountMngReport : TBasicDataView
    {
        MIndirectCost model = new MIndirectCost();
        const string ConstAccount = "管理费用台账_费用类别.flx";
        const string ConstOrg = "管理费用台账_组织.flx";
        CurrentProjectInfo projectInfo = null;
        OperationOrgInfo OrgInfo = null;
        EnumAccountMng _exeType;
        public VAccountMngReport( EnumAccountMng exeType)
        {
            InitializeComponent();
            this._exeType = exeType;
            IntialData();
            IntialEvent();
        }
        public　void IntialData()
        {
            dtpDateBegin.Value = new DateTime(ConstObject.TheLogin.LoginDate.Year, 1, 1);
            dtpDateEnd.Value = ConstObject.TheLogin.LoginDate.Date;
            OrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
            LoadTempleteFile(ConstAccount, this.reportGridAccount);
            LoadTempleteFile(ConstOrg, this.reportGridOrg);
            this.reportGridAccount.Rows = 1;
            this.reportGridOrg.Rows = 1;
            this.btnExcel.Visible = true;
            VisualOperationOrg();
        }
        public void VisualOperationOrg()
        {
            bool flag = false;
            projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && projectInfo.Code == CommonUtil.CompanyProjectCode)
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            this.btnOperationOrg.Visible = flag;
            this.lblPSelect.Visible = flag;
            this.txtOperationOrg.Visible = flag;
        }
        public void IntialEvent()
        {
            this.btnExcel.Click+=new EventHandler(btnExcel_Click);
            this.btnSearch.Click+=new EventHandler(btnSearch_Click);
            btnOperationOrg.Click += new EventHandler(btnOperationOrg_Click);
        }
        void btnOperationOrg_Click(object sender, EventArgs e)
        {
            //string opgId = "";
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect(true);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                OperationOrgInfo info = frm.Result[0] as OperationOrgInfo;
                txtOperationOrg.Tag = info;
                txtOperationOrg.Text = info.Name;
               // opgId = info.Id;
            }
        }
        public void btnExcel_Click(object sender, EventArgs e)
        {
            CustomFlexGrid flex = null;
            if (this.tab2.SelectedTab == this.tabPage1)
            {

                flex = this.reportGridOrg;
            }
            else if (this.tab2.SelectedTab == this.tabPage2)
            {
                flex = this.reportGridAccount;
            }
            else
            {
            }
            if (flex != null)
            {
                flex.ExportToExcel("管理费用台账_" + this.tab2.SelectedTab.Text, false, false, true);
            }
        }
        public void btnSearch_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("正在生成[管理费用台账]报告...");
            try
            {
                OperationOrgInfo OrgInfo = null;
                string sOrgSysCode = string.Empty, sProjectID = string.Empty;
                DataSet ds = null;
                DataTable oTable = null;
                CustomFlexGrid reportGrid = null;
                int iStart = 0;
                int iCount = 0;
                int iRowIndex = 0;
                decimal dMoney = 0, dSumMoney = 0, dBudgetMoney = 0, dSumBudgetMoney = 0;
                if (this.txtOperationOrg.Tag == null)
                {
                    throw new Exception("请选择查询的分支机构/项目");
                }
                else
                {
                    OrgInfo = this.txtOperationOrg.Tag as OperationOrgInfo;
                    sOrgSysCode = OrgInfo.SysCode;
                    sProjectID = model.IndirectCostSvr.GetProjectIDByOperationOrg(OrgInfo.Id);
                }
                #region 组织
                dMoney = 0; dSumMoney = 0; dBudgetMoney = 0; dSumBudgetMoney = 0;
                LoadTempleteFile(ConstOrg, this.reportGridOrg);

                ds = model.IndirectCostSvr.QueryManageCostGroupOrg(sOrgSysCode, sProjectID, this.dtpDateBegin.Value, this.dtpDateEnd.Value);
                oTable = ds == null || ds.Tables.Count == 0 ? null : ds.Tables[0];
                reportGrid = this.reportGridOrg;
                if (oTable != null)
                {
                    iCount = oTable.Rows.Count;
                    iStart = 3;
                    reportGrid.AutoRedraw = false;
                    reportGrid.InsertRow(iStart, iCount);
                    FlexCell.Range oRange = reportGrid.Range(iStart, 1, iStart + iCount, reportGrid.Cols - 1);
                    CommonUtil.SetFlexGridDetailCenter(oRange);
                    iCount = 0;
                    iRowIndex = 0;
                    // reportGrid.Cell(1, 1).Text = string.Format("{0}管理费用台账",OrgInfo.Name);
                    reportGrid.Cell(1, 1).Text = string.Format("{0}[{1}]至[{2}]按[组织]分类的管理费用台账", txtOperationOrg.Text, dtpDateBegin.Value.Date.ToString("yyyy年MM月dd日"), dtpDateEnd.Value.Date.ToString("yyyy年MM月dd日"));
                    foreach (DataRow oRow in oTable.Rows)
                    {
                        iRowIndex = iStart + iCount;
                        reportGrid.Cell(iRowIndex, 1).Text = (iCount + 1).ToString();
                        reportGrid.Cell(iRowIndex, 2).Text = ClientUtil.ToString(oRow["orginfoname"]);
                        dBudgetMoney = ClientUtil.ToDecimal(oRow["budgetmoney"]);
                        dSumBudgetMoney += dBudgetMoney;
                        reportGrid.Cell(iRowIndex, 3).Text = dBudgetMoney.ToString("N2");
                        dMoney = ClientUtil.ToDecimal(oRow["money"]);
                        dSumMoney += dMoney;
                        reportGrid.Cell(iRowIndex, 4).Text = dMoney.ToString("N2");
                        reportGrid.Cell(iRowIndex, 5).Text = dBudgetMoney == 0 ? "" : (dMoney*100 / dBudgetMoney).ToString("N2");
                        iCount++;
                    }
                    iRowIndex = iStart + iCount;
                    reportGrid.Cell(iRowIndex, 3).Text = dSumBudgetMoney.ToString("N2");
                    reportGrid.Cell(iRowIndex, 4).Text = dSumMoney.ToString("N2");

                }
                #endregion

                #region 费用类型
                dMoney = 0; dSumMoney = 0; dBudgetMoney = 0; dSumBudgetMoney = 0;
                LoadTempleteFile(ConstAccount, this.reportGridAccount);
                this.reportGridAccount.AutoRedraw = false;
                ds = model.IndirectCostSvr.QueryManageCostGroupAccountTitle(sOrgSysCode, sProjectID, this.dtpDateBegin.Value, this.dtpDateEnd.Value);
                reportGrid = this.reportGridAccount;
                oTable = ds == null || ds.Tables.Count == 0 ? null : ds.Tables[0];
                if (oTable != null)
                {
                    iCount = oTable.Rows.Count;
                    iStart = 3;
                    reportGrid.AutoRedraw = false;
                    reportGrid.InsertRow(iStart, iCount);
                    FlexCell.Range oRange = reportGrid.Range(iStart, 1, iStart + iCount, reportGrid.Cols - 1);
                    CommonUtil.SetFlexGridDetailCenter(oRange);
                    iCount = 0;
                    iRowIndex = 0;
                   // reportGrid.Cell(1, 1).Text = string.Format("{0}管理费用台账", OrgInfo.Name);
                    reportGrid.Cell(1, 1).Text = string.Format("{0}[{1}]至[{2}]按[费用类别]分类的管理费用台账", txtOperationOrg.Text, dtpDateBegin.Value.Date.ToString("yyyy年MM月dd日"), dtpDateEnd.Value.Date.ToString("yyyy年MM月dd日"));
                   
                    foreach (DataRow oRow in oTable.Rows)
                    {
                        iRowIndex = iStart + iCount;
                        reportGrid.Cell(iRowIndex, 1).Text = (iCount + 1).ToString();
                        reportGrid.Cell(iRowIndex, 2).Text = ClientUtil.ToString(oRow["accounttitlename"]);
                        dBudgetMoney = ClientUtil.ToDecimal(oRow["budgetmoney"]);
                        dSumBudgetMoney += dBudgetMoney;
                        reportGrid.Cell(iRowIndex, 3).Text = dBudgetMoney.ToString("N2");
                        dMoney = ClientUtil.ToDecimal(oRow["money"]);
                        dSumMoney += dMoney;
                        reportGrid.Cell(iRowIndex, 4).Text = dMoney.ToString("N2");
                        reportGrid.Cell(iRowIndex, 5).Text = dBudgetMoney == 0 ? "" : (dMoney*100 / dBudgetMoney).ToString("N2");
                        iCount++;
                    }
                    iRowIndex = iStart + iCount;
                    reportGrid.Cell(iRowIndex, 3).Text = dSumBudgetMoney.ToString("N2");
                    reportGrid.Cell(iRowIndex, 4).Text = dSumMoney.ToString("N2");
                }
                #endregion
            }
            catch (Exception e1)
            {
                MessageBox.Show("生成[管理费用台账]报告异常[" + e1.Message + "]");
                //throw new Exception("生成[收款台账]报告异常[" + e1.Message + "]");
            }
            finally
            {
                this.reportGridOrg.BackColorBkg = this.reportGridOrg.BackColor1 = System.Drawing.SystemColors.ButtonFace;
                this.reportGridAccount.BackColorBkg = this.reportGridAccount.BackColor1 = System.Drawing.SystemColors.ButtonFace;
                this.reportGridOrg.AutoRedraw = true;
                this.reportGridOrg.Refresh();
                this.reportGridAccount.AutoRedraw = true;
                this.reportGridAccount.Refresh();
                FlashScreen.Close();
            }
        }
       
       
        private bool LoadTempleteFile(string sReportPath,CustomFlexGrid reportGrid )
        {
 
          
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(sReportPath))
            {
                eFile.CreateTempleteFileFromServer(sReportPath);
                //载入格式和数据
                reportGrid.OpenFile(path + "\\" + sReportPath);//载入格式
            }
            else
            {
                MessageBox.Show("未找到模板格式文件【" + sReportPath + "】");
                return false;
            }


            return true;
        }

        

        

        

       

        
    }
}
