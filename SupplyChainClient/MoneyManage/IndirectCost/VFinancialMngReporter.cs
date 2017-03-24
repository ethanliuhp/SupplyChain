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
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost
{
    public partial class VFinancialMngReporter : TBasicDataView
    {
        private MIndirectCost model = new MIndirectCost();
        private string[] arrType = { "借款台账", "财务费用台账", "货币上交台帐" };
        private CurrentProjectInfo projectInfo;
        private bool isProject = false;

        public VFinancialMngReporter()
        {
            InitializeComponent();

            InitalData();

            IntialEvent();
        }

        private void InitalData()
        {
            dtpDateBegin.Value = ConstObject.LoginDate.AddDays(-7);
            dtpDateEnd.Value = ConstObject.LoginDate;

            projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
            {
                this.customLabel3.Visible = false;
                this.txtOperationOrg.Visible = false;
                this.btnOperationOrg.Visible = false;
                chkIsSelf.Visible = false;

                isProject = true;
            }
            reportGrid.Rows = 1;

            if (isProject)
            {
                cmbType.DataSource = arrType.Take(2).ToList();
            }
            else
            {
                cmbType.DataSource = arrType;
            }

            cmbType_SelectedIndexChanged(null, null);
        }

        private void IntialEvent()
        {
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnOperationOrg.Click += new EventHandler(btnOperationOrg_Click);
            cmbType.SelectedIndexChanged += cmbType_SelectedIndexChanged;
        }

        private void LoadFlex()
        {
            string sTitle = string.Empty;
            string[] arrColumn = null;
            int[] numColumn = null;
            FlexCell.Cell oCell = null;
            switch (this.cmbType.SelectedItem.ToString())
            {
                case "借款台账":
                    {
                        sTitle = arrType[0];
                        arrColumn = new string[] { "序号", "日期", "金额" };
                        numColumn = new int[] { 3 };

                        break;
                    }
                case "财务费用台账":
                    {
                        sTitle = arrType[1];
                        arrColumn = new string[] { "序号", "名称", "金额" };
                        numColumn = new int[] { 3 };
                        break;
                    }
                case "货币上交台帐":
                    {
                        sTitle = arrType[2];
                        arrColumn = new string[] { "序号", "日期", "货币上交" };
                        numColumn = new int[] { 3 };
                        break;
                    }
            }
            if (!string.IsNullOrEmpty(sTitle) && arrColumn != null && arrColumn.Length > 0)
            {
                reportGrid.Rows = 1;
                reportGrid.Cols = 1;
                reportGrid.Rows = 4;
                reportGrid.Cols = arrColumn.Length + 1;
                reportGrid.Column(0).Visible = false;
                reportGrid.Row(0).Visible = false;
                FlexCell.Range oRange = reportGrid.Range(1, 1, 1, reportGrid.Cols - 1);//设置表头
                oRange.Merge();
                oCell = reportGrid.Cell(1, 1);
                oCell.Text = string.Format("{0}[{1}]至[{2}]{3}", this.txtOperationOrg.Text,
                    this.dtpDateBegin.Value.Date.ToString("yyyy年MM月dd日"), this.dtpDateEnd.Value.Date.ToString("yyyy年MM月dd日"), sTitle);
                oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
                oCell.FontBold = true;
                oCell.FontSize = 15;
                reportGrid.Row(1).Height = 30;
                reportGrid.BackColor1 = reportGrid.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
                short iWidth = (short)((750 - 50) / (arrColumn.Length - 1));
                for (int i = 0; i < arrColumn.Length; i++)
                {
                    oCell = reportGrid.Cell(2, i + 1);
                    oCell.Text = arrColumn[i];
                    oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
                    if (i == 0)
                    {
                        reportGrid.Column(i + 1).Width = 50;
                    }
                    else
                    {
                        reportGrid.Column(i + 1).Width = iWidth;
                    }

                }
                foreach (int iColumn in numColumn)//初始化金额列表
                {
                    reportGrid.Column(iColumn).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                    reportGrid.Column(iColumn).Mask = FlexCell.MaskEnum.Numeric;
                    reportGrid.Column(iColumn).DecimalLength = 2;

                }
                reportGrid.Cell(3, 1).Text = "合计";
            }
        }

        private void LoadData()
        {
            bool bFlag = false;
            int iRowCount = 0;
            int iStart = 3;
            int iRowIndex = 0;
            decimal dMoney = 0;
            decimal dSumMoney = 0;
            EnumAccountSymbol AccountSymbol = EnumAccountSymbol.其他;
            DataSet ds = null;
            string sOrgSysCode = string.Empty;
            string sProjectID = string.Empty;
            GetOperate(out sOrgSysCode, out sProjectID);
            switch (this.cmbType.SelectedItem.ToString())
            {
                case "借款台账"://局借款台账
                    AccountSymbol = EnumAccountSymbol.借款标志;
                    bFlag = true;
                    break;
                case "财务费用台账"://财务费用台账
                    AccountSymbol = EnumAccountSymbol.财务费用标志;
                    bFlag = true;
                    break;
                case "货币上交台帐"://利润上交、货币上交台帐
                    AccountSymbol = EnumAccountSymbol.上交标志;
                    bFlag = true;
                    break;
            }
            if (bFlag)
            {
                ds = model.IndirectCostSvr.QueryFinancialAccount(sProjectID, sOrgSysCode, dtpDateBegin.Value, dtpDateEnd.Value, AccountSymbol, !chkIsSelf.Checked);
            }
            if (ds != null && ds.Tables.Count > 0)
            {
                iRowCount = ds.Tables[0].Rows.Count;
                reportGrid.InsertRow(iStart, iRowCount);
                FlexCell.Range range = reportGrid.Range(1, 1, iStart + iRowCount, reportGrid.Cols - 1);
                CommonUtil.SetFlexGridDetailCenter(range);

                if (AccountSymbol == EnumAccountSymbol.财务费用标志)
                {
                    reportGrid.Cell(iStart - 1, 2).Text = !string.IsNullOrEmpty(sProjectID) ? "日期" : "名称";
                }

                for (int i = 0; i < iRowCount; i++)
                {
                    iRowIndex = iStart + i;
                    reportGrid.Cell(iRowIndex, 1).Text = (i + 1).ToString();
                    reportGrid.Cell(iRowIndex, 2).Text = ClientUtil.ToString(ds.Tables[0].Rows[i][0]);

                    switch (AccountSymbol)
                    {
                        case EnumAccountSymbol.借款标志:
                            dMoney = ClientUtil.ToDecimal(ds.Tables[0].Rows[i][1]);
                            dSumMoney += dMoney;
                            reportGrid.Cell(iRowIndex, 3).Text = dMoney.ToString("N2");
                            break;
                        case EnumAccountSymbol.财务费用标志:
                            dMoney = ClientUtil.ToDecimal(ds.Tables[0].Rows[i][1]);
                            dSumMoney += dMoney;
                            reportGrid.Cell(iRowIndex, 3).Text = dMoney.ToString("N2");
                            break;
                        case EnumAccountSymbol.上交标志:
                            dMoney = ClientUtil.ToDecimal(ds.Tables[0].Rows[i][1]);
                            dSumMoney += dMoney;
                            reportGrid.Cell(iRowIndex, 3).Text = dMoney.ToString("N2");
                            break;
                    }
                }

                iRowIndex += 1;
                reportGrid.Cell(iRowIndex, 3).Text = dSumMoney.ToString("N2");
            }

            FlexCell.Range oRange = reportGrid.Range(1, 1, reportGrid.Rows - 1, reportGrid.Cols - 1);
            if (oRange != null)
            {
                oRange.set_Borders(FlexCell.EdgeEnum.Inside | FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            }
            //设置外观
            reportGrid.BackColor1 = System.Drawing.SystemColors.ButtonFace;
            reportGrid.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
        }

        private void GetOperate(out string sOrgSysCode, out string sProjectID)
        {
            sOrgSysCode = string.Empty;
            sProjectID = string.Empty;
            OperationOrgInfo oOperationOrgInfo = null;
            if (this.btnOperationOrg.Visible)
            {
                if (this.txtOperationOrg.Tag == null)
                {
                    throw new Exception("请选择组织机构");
                }
                else
                {
                    oOperationOrgInfo = this.txtOperationOrg.Tag as OperationOrgInfo;
                    sOrgSysCode = oOperationOrgInfo.SysCode;
                    sProjectID = model.IndirectCostSvr.GetProjectIDByOperationOrg(oOperationOrgInfo.Id);
                    if (!string.IsNullOrEmpty(sProjectID))
                    {
                        sOrgSysCode = string.Empty;
                    }
                }

            }
            else
            {
                sProjectID = projectInfo.Id;
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            this.reportGrid.ExportToExcel(this.cmbType.SelectedItem.ToString(), false, false, true);
        }

        private void btnOperationOrg_Click(object sender, EventArgs e)
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            reportGrid.AutoRedraw = false;

            LoadFlex();

            LoadData();

            reportGrid.AutoRedraw = true;

            reportGrid.Refresh();
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkIsSelf.Visible = !isProject && cmbType.SelectedIndex == 1;
        }
    }
}
