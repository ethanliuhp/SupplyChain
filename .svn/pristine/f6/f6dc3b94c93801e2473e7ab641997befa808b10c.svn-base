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
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Business.Erp.SupplyChain.Client.Util;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VGatherAndPayDepositReport : TBasicDataView
    {
        static string[] arrAccountType = { "","投标保证金","履约保证金","预付款保证金","农民工工资保证金", "安全保证金",
                                             "质量保证金","诚信保证金","其他保证金","房租押金","水电押金",
                                             "散装水泥押金","其他押金","食堂押金","处理废材押金","风险抵押金"};

        MFinanceMultData model = new MFinanceMultData();
        string sReportName = "保证金/押金台账";
        string sReportPath = "保证金和押金台账.flx";
        CurrentProjectInfo projectInfo = null;
        CGatheringMng_ExecType _exeType;
        public VGatherAndPayDepositReport(CGatheringMng_ExecType exeType)
        {
            InitializeComponent();
            this._exeType = exeType;

            IntialData();

            IntialEvent();
        }

        public void IntialData()
        {
            LoadTempleteFile();
            this.reportGrid.Rows = 1;
            this.cmbGatheringType.Items.Clear();
            for (int i = 0; i < arrAccountType.Length; i++)
            {
                this.cmbGatheringType.Items.Insert(this.cmbGatheringType.Items.Count, arrAccountType[i]);
            }
            this.cmbGatheringType.SelectedIndex = 0;
            VisualOperationOrg();
            btnSelRel.Enabled = true;
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
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            btnOperationOrg.Click += new EventHandler(btnOperationOrg_Click);
            this.btnSelRel.Click += new EventHandler(btnSelRel_Click);
        }

        void btnOperationOrg_Click(object sender, EventArgs e)
        {
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect(true);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                OperationOrgInfo info = frm.Result[0] as OperationOrgInfo;
                txtOperationOrg.Tag = info;
                txtOperationOrg.Text = info.Name;
            }
        }
        void btnSelRel_Click(object sender, EventArgs e)
        {
            VCusAndSupRelSelector vSelector = new VCusAndSupRelSelector();
            vSelector.ShowDialog();
            IList list = vSelector.Result;
            if (list == null || list.Count == 0) return;
            DataDomain domain = list[0] as DataDomain;
            this.txtOrgName.Tag = domain;
            this.txtOrgName.Text = ClientUtil.ToString(domain.Name4);
        }
        public void btnExcel_Click(object sender, EventArgs e)
        {
            this.reportGrid.ExportToExcel("保证金(押金)台账", false, false, true);
        }

        public void btnSearch_Click(object sender, EventArgs e)
        {
            FlashScreen.Show(string.Format("正在生成[{0}]报告...", sReportName));
            try
            {
               
                DataTable oTable = null;
                CustomFlexGrid reportGrid = null;
              
                int iStart = 0;
                int iCount = 0;
                int iRowIndex = 0;
                int iCol = 0;
                bool  bSymbol = false;
                decimal dLastRemainMoney = 0, dSumLastRemainMoney = 0,
                    dPayMoney = 0, dSumPayMoney = 0,
                    dGatherMoney = 0, dSumGatherMoney = 0,
                    dCurRemainMoney = 0, dSumCurRemainMoney = 0;
                LoadTempleteFile();
                oTable = GetData();
                reportGrid = this.reportGrid;
                if (oTable != null)
                {
                    iCount = oTable.Rows.Count;
                    iStart = 3;
                    reportGrid.AutoRedraw = false;
                    reportGrid.InsertRow(iStart, iCount);
                    FlexCell.Range oRange = reportGrid.Range(iStart, 1, iStart + iCount, reportGrid.Cols - 1);
                    CommonUtil.SetFlexGridDetailCenter(oRange);
                    reportGrid.BackColor1 = reportGrid.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
                    iCount = 0;
                    iRowIndex = 0;

                    reportGrid.Cell(1, 1).Text = string.Format("{0}[截至{1}]{2}", txtOperationOrg.Text,
                        this.dtpDateEnd.Value.ToString("yyyy年MM月dd日"), sReportName);
                    //opgname, projectname,t.depttype,t.deptname,
                    //accounttitlename,lastremainmoney,paymoney,gathermoney
                    foreach (DataRow oRow in oTable.Rows)
                    {
                        iRowIndex = iStart + iCount;
                   
                        reportGrid.Cell(iRowIndex, 1).Text = (iCount + 1).ToString();
                        reportGrid.Cell(iRowIndex, 2).Text = ClientUtil.ToString(oRow["opgname"]);
                        reportGrid.Cell(iRowIndex, 3).Text = ClientUtil.ToString(oRow["projectname"]);
                        reportGrid.Cell(iRowIndex, 4).Text = ClientUtil.ToString(oRow["depttype"]);
                        reportGrid.Cell(iRowIndex, 5).Text = ClientUtil.ToString(oRow["deptname"]);
                        reportGrid.Cell(iRowIndex, 6).Text = ClientUtil.ToString(oRow["accounttitlename"]);

                        dLastRemainMoney =  ClientUtil.ToDecimal(oRow["lastremainmoney"]);
                          dPayMoney = ClientUtil.ToDecimal(oRow["paymoney"]);
                         dGatherMoney = ClientUtil.ToDecimal(oRow["gathermoney"]);
                      
                      
                        if (ClientUtil.ToString(oRow["depttype"]) == "分供商")
                        {
                            //分供商 本年增加：本年累计收款  本年减少:本年累计付款 上年余额:上年累计收款-上年累计付款
                            dLastRemainMoney = -dLastRemainMoney;
                            dSumPayMoney += dGatherMoney;
                            reportGrid.Cell(iRowIndex, 8).Text = dGatherMoney.ToString("N2");
                            dSumGatherMoney += dPayMoney;
                            reportGrid.Cell(iRowIndex, 9).Text = dPayMoney.ToString("N2");
                            dCurRemainMoney = dLastRemainMoney + (dGatherMoney - dPayMoney);
                            dSumCurRemainMoney += dCurRemainMoney;
                            reportGrid.Cell(iRowIndex, 10).Text = dCurRemainMoney.ToString("N2");
                            dSumLastRemainMoney += dLastRemainMoney;
                            reportGrid.Cell(iRowIndex, 7).Text = dLastRemainMoney.ToString("N2");
                        }
                        else
                        {
                            //客户 本年增加：本年累计付款  本年减少:本年累计收款 上年余额:上年累计付款-上年累计收款
                            dSumPayMoney += dPayMoney;
                            reportGrid.Cell(iRowIndex, 8).Text = dPayMoney.ToString("N2");
                            dSumGatherMoney += dGatherMoney;
                            reportGrid.Cell(iRowIndex, 9).Text = dGatherMoney.ToString("N2");
                            dCurRemainMoney = dLastRemainMoney + (dPayMoney - dGatherMoney);
                            dSumCurRemainMoney += dCurRemainMoney;
                            reportGrid.Cell(iRowIndex, 10).Text = dCurRemainMoney.ToString("N2");
                            dSumLastRemainMoney += dLastRemainMoney;
                            reportGrid.Cell(iRowIndex, 7).Text = dLastRemainMoney.ToString("N2");
                        }
                        iCount++;
                    }
                    iRowIndex = iStart + iCount;
                    reportGrid.Cell(iRowIndex, 7).Text = dSumLastRemainMoney.ToString("N2");
                    reportGrid.Cell(iRowIndex, 8).Text = dSumPayMoney.ToString("N2");
                    reportGrid.Cell(iRowIndex, 9).Text = dSumGatherMoney.ToString("N2");
                    reportGrid.Cell(iRowIndex, 10).Text = dSumCurRemainMoney.ToString("N2");
                }
              
            }
            catch (Exception e1)
            {
                MessageBox.Show(string.Format("生成[{0}]报告异常[{1}]", sReportName, e1.Message));
                //throw new Exception("生成[收款台账]报告异常[" + e1.Message + "]");
            }
            finally
            {
                this.reportGrid.FixedRows = 3;
                this.reportGrid.AutoRedraw = true;
                this.reportGrid.Refresh();
                FlashScreen.Close();
            }
        }
        private DataTable GetData()
        {
            string sProjectId = string.Empty;
            OperationOrgInfo OrgInfo = null;
            string sOrgSysCode = string.Empty;
            string sAccountTitleName = string.Empty;
            string sTheSupplierRelationInfo = string.Empty;
            string sTheCustomerRelationInfo = string.Empty;
            string sType = string.Empty;
            DataSet ds = null;
            DataTable oTable = null;
            DateTime dEnd = dtpDateEnd.Value;
            if (this.txtOperationOrg.Visible)
            {
                if (this.txtOperationOrg.Tag == null)
                {
                    throw new Exception("请选择查询的分支机构/项目");
                }
                else
                {
                    OrgInfo = this.txtOperationOrg.Tag as OperationOrgInfo;
                    sProjectId = model.IndirectCostSvr.GetProjectIDByOperationOrg(OrgInfo.Id);
                    if (string.IsNullOrEmpty(sProjectId))
                    {
                        sOrgSysCode = OrgInfo.SysCode;
                    }
                    else
                    {
                        sOrgSysCode = string.Empty;
                    }
                }
            }
            else
            {
                sProjectId = projectInfo.Id;
            }
            sAccountTitleName = GetAccountTitleName(); 
            if (!string.IsNullOrEmpty(this.txtOrgName.Text) && this.txtOrgName.Tag != null)
            {
                DataDomain currDomain = this.txtOrgName.Tag as DataDomain;
                if (ClientUtil.ToInt(currDomain.Name1) == 1)
                {
                    sTheSupplierRelationInfo = ClientUtil.ToString(currDomain.Name2);
                }
                else
                {
                    sTheCustomerRelationInfo = ClientUtil.ToString(currDomain.Name2);
                }
            }
            sType = cmbType.SelectedItem==null?"":cmbType.SelectedItem.ToString();
            ds = model.FinanceMultDataSrv.QueryGatherAndPayDepositReport(dEnd, sProjectId, sOrgSysCode, sAccountTitleName, sTheSupplierRelationInfo, sTheCustomerRelationInfo, sType);
            oTable = (ds == null || ds.Tables.Count == 0) ? null : ds.Tables[0];
            return oTable;
        }
        private string GetAccountTitleName()
        {
            string sAccountTitleName = cmbGatheringType.SelectedIndex == 0 ? "" :("'"+ cmbGatheringType.SelectedItem.ToString()+"'");
            if (string.IsNullOrEmpty(sAccountTitleName))
            {
                string[] arr = new string[arrAccountType.Length - 1]; 
                for (int i = 1; i < arrAccountType.Length; i++)
                {
                    arr[i - 1] = string.Format("'{0}'", arrAccountType[i]);
                }
                sAccountTitleName = string.Join(",", arr);
            }
            return sAccountTitleName;
        }
        private bool LoadTempleteFile()
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(sReportPath))
            {
                eFile.CreateTempleteFileFromServer(sReportPath);
                //载入格式和数据
                reportGrid.OpenFile(path + "\\" + sReportPath);//载入格式
                reportGrid.SelectionStart = 0;
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