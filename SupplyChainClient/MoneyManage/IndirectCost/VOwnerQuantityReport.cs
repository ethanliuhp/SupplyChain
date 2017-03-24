using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Util;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost
{
    public partial class VOwnerQuantityReport : TBasicDataView
    {
        MIndirectCost model = new MIndirectCost();
        string sReportName = "甲方审量及应收台账";
        string sReportPath = "甲方审量及应收台账.flx";
        CurrentProjectInfo projectInfo = null;
        OperationOrgInfo OrgInfo = null;
        IndirectCostExecType _exeType;
        public VOwnerQuantityReport(IndirectCostExecType exeType)
        {
            InitializeComponent();
            this._exeType = exeType;
            IntialData();
            IntialEvent();
        }
        public void IntialData()
        {
            dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.Date.AddDays(-7);
            dtpDateEnd.Value = ConstObject.TheLogin.LoginDate.Date;
            OrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
            LoadTempleteFile(this.sReportPath, this.reportGrid);
            this.reportGrid.Rows = 1;
            this.btnExcel.Visible = true;
            projectInfo = StaticMethod.GetProjectInfo();
            //if (projectInfo != null && projectInfo.Code == CommonUtil.CompanyProjectCode)
            //{
            //    this.btnOperationOrg.Visible = true;
            //}
            //else
            //{
            //    this.btnOperationOrg.Visible = false;
            //}
            this.VisualOperationOrg();
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
            reportGrid.ExportToExcel(sReportName, false, false, true);
        }
        public void btnSearch_Click(object sender, EventArgs e)
        {
            FlashScreen.Show(string.Format("正在生成[{0}]报告...", sReportName));
            try
            {
                OperationOrgInfo OrgInfo = null;
                string sProjectID = string.Empty;
                string sOrgSysCode = string.Empty;
                DataSet ds = null;
                DataTable oTable = null;
                CustomFlexGrid reportGrid = null;
                bool bIsProject = false;
                decimal dSubmitQuantity = 0, dSumSubmitQuantity = 0, dTJSubmitQuantity = 0, dSumTJSubmitQuantity = 0, dAZSubmitQuantity = 0, dSumAZSubmitQuantity = 0, dZSSubmitQuantity = 0, dSumZSSubmitQuantity = 0,
                    dTJConfirmMoney = 0, dSumTJConfirmMoney = 0, dAZConfirmMoney = 0, dSumAZConfirmMoney = 0, dZSConfirmMoney = 0, dSumZSConfirmMoney = 0,
                    dConfirmMoney = 0, dSumConfirmMoney = 0, dAcctGatheringMoney = 0, dSumAcctGatheringMoney = 0;
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
                        sProjectID = model.IndirectCostSvr.GetProjectIDByOperationOrg(OrgInfo.Id);
                        bIsProject = !string.IsNullOrEmpty(sProjectID);
                        sOrgSysCode = string.IsNullOrEmpty(sProjectID) ? OrgInfo.SysCode : string.Empty;
                    }
                }
                else
                {
                    sProjectID = projectInfo.Id;
                    bIsProject = true;
                }

                LoadTempleteFile(sReportPath, this.reportGrid);

                ds = model.IndirectCostSvr.QueryOwnerQuantityReport(sProjectID, sOrgSysCode, this.dtpDateBegin.Value, this.dtpDateEnd.Value);
                oTable = ds == null || ds.Tables.Count == 0 ? null : ds.Tables[0];
                reportGrid = this.reportGrid;

                reportGrid.Column(2).Visible = bIsProject;
                reportGrid.Column(3).Visible = !bIsProject;
                reportGrid.Column(4).Visible = !bIsProject;
                reportGrid.Column(8).Visible = bIsProject;
                reportGrid.Column(9).Visible = bIsProject;
                reportGrid.Column(10).Visible = bIsProject;
                reportGrid.Column(11).Visible = bIsProject;
                reportGrid.Column(12).Visible = bIsProject;
                reportGrid.Column(18).Visible = bIsProject;

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
                    reportGrid.Cell(1, 1).Text = string.Format("{0}[{2}]至[{3}]{1}", (OrgInfo == null ? projectInfo.Name : OrgInfo.Name), sReportName, this.dtpDateBegin.Value.Date.ToString("yyyy年MM月dd日"), this.dtpDateEnd.Value.Date.ToString("yyyy年MM月dd日"));
                    foreach (DataRow oRow in oTable.Rows)
                    {
                        //quantitydate submitquantity 
                        //tjsubmitquantity azsubmitquantity zssubmitquantity 
                        //tjsqwbsname azqwbsname zsqwbsname
                        //confirmdate confirmstartdate confirmenddate
                        //tjconfirmmoney azconfirmmoney zsconfirmmoney
                        //confirmmoney GatheringRate acctgatheringmoney Descript
                        iRowIndex = iStart + iCount;
                        reportGrid.Cell(iRowIndex, 1).Text = (iCount + 1).ToString();
                        if (bIsProject)
                        {
                            reportGrid.Cell(iRowIndex, 2).Text = oRow["quantitydate"] == DBNull.Value ? "" : ClientUtil.ToDateTime(oRow["quantitydate"]).ToString("yyyy-MM-dd");//报送时间
                        }
                        // dSubmitQuantity = ClientUtil.ToDecimal(oRow["submitquantity"]);//报送金额
                        // dSumSubmitQuantity += dSubmitQuantity;
                        //reportGrid.Cell(iRowIndex, 3).Text = dSubmitQuantity.ToString("N2");
                        if (!bIsProject)
                        {
                            reportGrid.Cell(iRowIndex, 3).Text = ClientUtil.ToString(oRow["opgname"]);
                            reportGrid.Cell(iRowIndex, 4).Text = ClientUtil.ToString(oRow["projectname"]);
                        }

                        dTJSubmitQuantity = ClientUtil.ToDecimal(oRow["tjsubmitquantity"]);//土建
                        dSumTJSubmitQuantity += dTJSubmitQuantity;
                        reportGrid.Cell(iRowIndex, 5).Text = dTJSubmitQuantity.ToString("N2");

                        dAZSubmitQuantity = ClientUtil.ToDecimal(oRow["azsubmitquantity"]);//安装
                        dSumAZSubmitQuantity += dAZSubmitQuantity;
                        reportGrid.Cell(iRowIndex, 6).Text = dAZSubmitQuantity.ToString("N2");

                        dZSSubmitQuantity = ClientUtil.ToDecimal(oRow["zssubmitquantity"]);//装饰
                        dSumZSSubmitQuantity += dZSSubmitQuantity;
                        reportGrid.Cell(iRowIndex, 7).Text = dZSSubmitQuantity.ToString("N2");
                        if (bIsProject)
                        {
                            reportGrid.Cell(iRowIndex, 8).Text = ClientUtil.ToString(oRow["tjsqwbsname"]);//土建报送工程节点
                            reportGrid.Cell(iRowIndex, 9).Text = ClientUtil.ToString(oRow["azqwbsname"]);//安装报送工程节点
                            reportGrid.Cell(iRowIndex, 10).Text = ClientUtil.ToString(oRow["zsqwbsname"]);//装饰报送工程节点

                            reportGrid.Cell(iRowIndex, 11).Text = oRow["confirmdate"] == DBNull.Value ? "" : ClientUtil.ToDateTime(oRow["confirmdate"]).ToString("yyyy-MM-dd");//甲方审定时间*年*月*日
                            // reportGrid.Cell(iRowIndex, 10).Text  =oRow["confirmdate"]==DBNull.Value?"": ClientUtil.ToDateTime(oRow["confirmdate"]).ToString("yyyy-MM");//甲方审定时间*年*月*日

                            if (oRow["confirmstartdate"] == DBNull.Value && oRow["confirmenddate"] == DBNull.Value)
                            {
                                sTemp = string.Empty;
                            }
                            else
                            {
                                sTemp = string.Format("{0}至{1}", oRow["confirmstartdate"] == DBNull.Value ? "未定" : ClientUtil.ToDateTime(oRow["confirmstartdate"]).ToString("yyyy-MM-dd"),
                                                                  oRow["confirmenddate"] == DBNull.Value ? "未定" : ClientUtil.ToDateTime(oRow["confirmenddate"]).ToString("yyyy-MM-dd"));
                            }
                            reportGrid.Cell(iRowIndex, 12).Text = sTemp;
                        }
                        dTJConfirmMoney = ClientUtil.ToDecimal(oRow["tjconfirmmoney"]);//土建审定金额
                        dSumTJConfirmMoney += dTJConfirmMoney;
                        reportGrid.Cell(iRowIndex, 13).Text = dTJConfirmMoney.ToString("N2");

                        dAZConfirmMoney = ClientUtil.ToDecimal(oRow["azconfirmmoney"]);//安装审定金额
                        dSumAZConfirmMoney += dAZConfirmMoney;
                        reportGrid.Cell(iRowIndex, 14).Text = dAZConfirmMoney.ToString("N2");

                        dZSConfirmMoney = ClientUtil.ToDecimal(oRow["zsconfirmmoney"]);//装饰审定金额
                        dSumZSConfirmMoney += dZSConfirmMoney;
                        reportGrid.Cell(iRowIndex, 15).Text = dZSConfirmMoney.ToString("N2");

                        reportGrid.Cell(iRowIndex, 16).Text = (ClientUtil.ToDecimal(oRow["GatheringRate"]) * 100).ToString("N2");
                        //dConfirmMoney = ClientUtil.ToDecimal(oRow["confirmmoney"]);
                        //dSumConfirmMoney += dConfirmMoney;
                        //reportGrid.Cell(iRowIndex, 15).Text = dConfirmMoney.ToString("N2");

                        dAcctGatheringMoney = ClientUtil.ToDecimal(oRow["acctgatheringmoney"]);
                        dSumAcctGatheringMoney += dAcctGatheringMoney;
                        reportGrid.Cell(iRowIndex, 17).Text = dAcctGatheringMoney.ToString("N2");
                        if (bIsProject)
                        {
                            reportGrid.Cell(iRowIndex, 18).Text = ClientUtil.ToString(oRow["Descript"]);
                        }
                        iCount++;
                    }
                    iRowIndex = iStart + iCount;
                    reportGrid.Cell(iRowIndex, 5).Text = dSumTJSubmitQuantity.ToString("N2");
                    reportGrid.Cell(iRowIndex, 6).Text = dSumAZSubmitQuantity.ToString("N2");
                    reportGrid.Cell(iRowIndex, 7).Text = dSumZSSubmitQuantity.ToString("N2");

                    reportGrid.Cell(iRowIndex, 13).Text = dSumTJConfirmMoney.ToString("N2");
                    reportGrid.Cell(iRowIndex, 14).Text = dSumAZConfirmMoney.ToString("N2");
                    reportGrid.Cell(iRowIndex, 15).Text = dSumZSConfirmMoney.ToString("N2");
                    reportGrid.Cell(iRowIndex, 17).Text = dSumAcctGatheringMoney.ToString("N2");

                    for (int tt = 0; tt < reportGrid.Cols; tt++)
                    {
                        reportGrid.Column(tt).AutoFit();
                    }
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(string.Format("生成[{0}]报告异常[{1}]", sReportName, e1.Message));
                //throw new Exception("生成[收款台账]报告异常[" + e1.Message + "]");
            }
            finally
            {
                //设置外观
                reportGrid.BackColor1 = System.Drawing.SystemColors.ButtonFace;
                reportGrid.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
                this.reportGrid.AutoRedraw = true;
                this.reportGrid.Refresh();
                FlashScreen.Close();
            }
        }


        private bool LoadTempleteFile(string sReportPath, CustomFlexGrid reportGrid)
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

