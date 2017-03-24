using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Util;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Component.WinControls.Controls;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VMainBusinessInComeReportReport : TBasicDataView
    {
        MFinanceMultData model = new MFinanceMultData();
        string sReportName = "主营业务收入台账";
        string sReportPath = "主营业务收入台账.flx";
        CurrentProjectInfo projectInfo = null;
        OperationOrgInfo OrgInfo = null;
        FinanceMultDataExecType _exeType;

        public VMainBusinessInComeReportReport(FinanceMultDataExecType exeType)
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
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
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
            this.reportGrid.ExportToExcel(sReportName, false, false, true);
        }

        public void btnSearch_Click(object sender, EventArgs e)
        {
            FlashScreen.Show(string.Format("正在生成[{0}]报告...", sReportName));
            try
            {
                string sProjectId = string.Empty;
                OperationOrgInfo OrgInfo = null;
                string sOrgSysCode = string.Empty;
                DataSet ds = null;
                DataTable oTable = null;
                CustomFlexGrid reportGrid = null;

                string sTemp = string.Empty;
                int iStart = 0;
                int iCount = 0;
                int iRowIndex = 0;
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
                LoadTempleteFile();

                ds = model.FinanceMultDataSrv.QueryMainBusinessInComeReport(
                    sProjectId,
                    sOrgSysCode,
                    ClientUtil.ToInt(cmbYear.SelectedItem.ToString()),
                    ClientUtil.ToInt(cmbMonth.SelectedItem.ToString()),
                    chkOnlyOne.Checked
                    );
                oTable = ds == null || ds.Tables.Count == 0 ? null : ds.Tables[0];
                reportGrid = this.reportGrid;
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
                    reportGrid.Cell(1, 1).Text = string.Format("{0}{1}年{2}月{3}", txtOperationOrg.Text, cmbYear.SelectedItem.ToString(), cmbMonth.SelectedItem.ToString(), sReportName);

                    if (string.IsNullOrEmpty(sProjectId))
                    {
                        reportGrid.Cell(iStart - 1, 2).Text = "名称";
                    }
                    else
                    {
                        reportGrid.Cell(iStart - 1, 2).Text = "年月";
                    }

                    foreach (DataRow oRow in oTable.Rows)
                    {
                        iRowIndex = iStart + iCount;
                        reportGrid.Cell(iRowIndex, 1).Text = (iCount + 1).ToString();
                        if (!string.IsNullOrEmpty(sProjectId))
                        {
                            reportGrid.Cell(iRowIndex, 2).Text = string.Format("{0}-{1}", oRow[0], oRow[1]);
                        }
                        else
                        {
                            reportGrid.Cell(iRowIndex, 2).Text = oRow[1].ToString();
                        }

                        for (var j = 2; j < oTable.Columns.Count; j++)
                        {
                            reportGrid.Cell(iRowIndex, j + 1).Text = ClientUtil.ToDecimal(oRow[j]).ToString("N2");
                        }

                        iCount++;
                    }

                    reportGrid.Column(1).AutoFit();//列自适应
                    reportGrid.Column(2).AutoFit();//列自适应
                    iRowIndex = iStart + iCount;
                    for (var j = 2; j < oTable.Columns.Count; j++)
                    {
                        reportGrid.Cell(iRowIndex, j + 1).Text = ClientUtil.ToDecimal(oTable.Compute(string.Format("sum({0})", oTable.Columns[j].ColumnName), "")).ToString("N2");

                        reportGrid.Column(j + 1).AutoFit();//列自适应
                    }
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(string.Format("生成[{0}]报告异常[{1}]", sReportName, e1.Message));
            }
            finally
            {
                this.reportGrid.BackColor1 = this.reportGrid.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
                this.reportGrid.AutoRedraw = true;
                this.reportGrid.Refresh();
                FlashScreen.Close();
            }
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