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
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using System.Collections;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VMustNotGatheringReport : TBasicDataView
    {

        #region
        const string ConstProjectGatheringReport = @"项目收款台帐.flx";
        const string ConstProjectNotGatheringReport = @"项目应收未收工程款台账.flx";
        const string ConstOrgGatheringReport = @"分支机构收款单.flx";
        const string ConstOrgNotGatheringReport = @"分支结构应收未收工程款台账.flx";

        QueryState queryState = QueryState.其他;
        #endregion
        private MFinanceMultData model = new MFinanceMultData();
        CurrentProjectInfo projectInfo;
        CGatheringMng_ExecType _ExecType;
        public VMustNotGatheringReport(CGatheringMng_ExecType execType)
        {
            InitializeComponent();
            this._ExecType = execType;
            IntialData();
            IntialEvent();
        }
        public void IntialData()
        {

            string sValue = string.Empty;
            projectInfo = StaticMethod.GetProjectInfo();
            dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.Date.AddMonths(-1);
            dtpDateEnd.Value = ConstObject.TheLogin.LoginDate.Date;
            projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
            {
                this.lblPSelect.Visible = false;
                this.txtOperationOrg.Visible = false;
                this.btnOperationOrg.Visible = false;

                //ReportPath = ConstProjectGatheringReport;
            }
            else
            {
                // ReportPath = ConstOrgGatheringReport;
            }
            // LoadTempleteFile();
            this.reportGrid.Rows = 1;
        }
        public void IntialEvent()
        {

            this.btnSearch.Click += new EventHandler(btnSearch_Click);

            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            btnOperationOrg.Click += new EventHandler(btnOperationOrg_Click);
        }
        public void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                FlashScreen.Show("正在生成[收款台账]报告...");
                string sProjectID = string.Empty;
                string sOrgSysCode = string.Empty;
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
                    }

                }
                else
                {
                    sProjectID = projectInfo.Id;
                }
                if (string.IsNullOrEmpty(sProjectID))
                {
                    queryState = QueryState.组织;
                    btnSearchOrg_Click(sOrgSysCode);
                }
                else
                {
                    queryState = QueryState.项目;
                    btnSearchProject_Click(sProjectID);
                }
                FlashScreen.Close();
            }
            catch (Exception e1)
            {
                FlashScreen.Close();
                MessageBox.Show("生成[台账]报告异常[" + e1.Message + "]");
                queryState = QueryState.其他;
            }

        }
        public void btnSearchProject_Click(string sProjectID)
        {
            LoadTempleteFile(ConstProjectNotGatheringReport);
            LoadProjectNotGatheringData(sProjectID);
        }
        public void btnSearchOrg_Click(string sOrgSysCode)
        {
            LoadTempleteFile(ConstOrgNotGatheringReport);
            LoadOrgNotGatheringData(sOrgSysCode);
        }
        void btnExcel_Click(object sender, EventArgs e)
        {
            if (queryState == QueryState.其他)
            {
                MessageBox.Show("请先查询报表");
            }
            else
            {
                string reportName = queryState == QueryState.项目 ? "项目应收拖欠款台账" : "工程款应收拖欠款台账";
                reportGrid.ExportToExcel(reportName, false, false, true);
            }
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
        private bool LoadTempleteFile(string ReportPath)
        {
            string modelName = ReportPath;
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式和数据
                reportGrid.OpenFile(path + "\\" + modelName);//载入格式
            }
            else
            {
                MessageBox.Show("未找到模板格式文件【" + modelName + "】");
                return false;
            }


            return true;
        }

        private void LoadProjectNotGatheringData(string sProjectID)
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            DateTime dConfirmDate, dGatheringDate;
            startDate = dtpDateBegin.Value.Date;
            endDate = dtpDateEnd.Value.Date;

            int iStart = 3;
            int iCount = 0, iRowIndex = 0;
            DateTime minDate = DateTime.Parse("1900-1-1");
            decimal dTJConfrim = 0, dTJSumConfrim = 0,
                    dAZConfrim = 0, dAZSumConfrim = 0,
                     dZSConfrim = 0, dZSSumConfrim = 0,
                     dRate = 0,
                     dMustGathering = 0, dSumMustGathering = 0,
                     dNotGathering = 0, dSumNotGathering = 0,
                     dGathering = 0, dSumGathering = 0;

            DataSet ds = model.FinanceMultDataSrv.QueryProjectNotGatheringAccountReport(sProjectID, startDate, endDate);
            reportGrid.Cell(1, 1).Text = string.Format("{0}[{1}]至[{2}]应收拖欠工程款台帐", txtOperationOrg.Text, dtpDateBegin.Value.Date.ToString("yyyy年MM月dd日"), dtpDateEnd.Value.Date.ToString("yyyy年MM月dd日"));
            if (ds != null && ds.Tables.Count > 0)
            {
                reportGrid.InsertRow(iStart, ds.Tables[0].Rows.Count);
                iCount = 0;
                reportGrid.AutoRedraw = false;
                FlexCell.Range range = reportGrid.Range(iStart, 1, iStart + ds.Tables[0].Rows.Count, reportGrid.Cols - 1);
                range.Locked = true;
                CommonUtil.SetFlexGridDetailCenter(range);
                //confirmdate quantitytype RightRate confirmstartdate confirmenddate MustGathering Gathering

                //confirmdate,quantitytype,gatheringrate,confirmstartdate,confirmenddate,DelayTime,
                //confirmmoney,acctgatheringmoney,createdate,GatheringDate 
                foreach (DataRow oRow in ds.Tables[0].Rows)
                {
                    iRowIndex = iStart + iCount;
                    SetValue(iRowIndex, 1, (iCount + 1).ToString());//序号
                    dConfirmDate = ClientUtil.ToDateTime(oRow["confirmdate"]);
                    SetValue(iRowIndex, 2, oRow["confirmdate"] == DBNull.Value ? "" : dConfirmDate.ToString("yyyy-MM-dd"));
                    switch (ClientUtil.ToString(oRow["quantitytype"]))
                    {
                        case "土建":
                            {
                                dTJConfrim = ClientUtil.ToDecimal(oRow["confirmmoney"]);
                                dTJSumConfrim += dTJConfrim;
                                dAZConfrim = 0;
                                dZSConfrim = 0;
                                SetValue(iRowIndex, 3, dTJConfrim.ToString("N2"));
                                SetValue(iRowIndex, 4, dAZConfrim.ToString("N2"));
                                SetValue(iRowIndex, 5, dZSConfrim.ToString("N2"));
                                break;
                            }
                        case "安装":
                            {
                                dTJConfrim = 0;
                                dAZConfrim = ClientUtil.ToDecimal(oRow["confirmmoney"]);
                                dAZSumConfrim += dAZConfrim;
                                dZSConfrim = 0;
                                SetValue(iRowIndex, 3, dTJConfrim.ToString("N2"));
                                SetValue(iRowIndex, 4, dAZConfrim.ToString("N2"));
                                SetValue(iRowIndex, 5, dZSConfrim.ToString("N2"));
                                break;
                            }
                        case "装饰":
                            {
                                dTJConfrim = 0;
                                dAZConfrim = 0;
                                dZSConfrim = ClientUtil.ToDecimal(oRow["confirmmoney"]);
                                dZSSumConfrim += dZSConfrim;
                                SetValue(iRowIndex, 3, dTJConfrim.ToString("N2"));
                                SetValue(iRowIndex, 4, dAZConfrim.ToString("N2"));
                                SetValue(iRowIndex, 5, dZSConfrim.ToString("N2"));
                                break;
                            }
                    }
                    dRate = ClientUtil.ToDecimal(oRow["gatheringrate"]);
                    SetValue(iRowIndex, 6, (dRate * 100).ToString("N2"));
                    dGathering = ClientUtil.ToDecimal(oRow["Gathering"]);
                    dSumGathering += dGathering;
                    dMustGathering = ClientUtil.ToDecimal(oRow["acctgatheringmoney"]);
                    dSumMustGathering += dMustGathering;
                    SetValue(iRowIndex, 7, dMustGathering.ToString("N2"));
                    dGatheringDate = ClientUtil.ToDateTime(oRow["GatheringDate"]);
                    SetValue(iRowIndex, 8, oRow["GatheringDate"] == DBNull.Value ? "" : dGatheringDate.ToString("yyyy-MM-dd"));
                    SetValue(iRowIndex, 9, dGathering.ToString("N2"));
                    SetValue(iRowIndex, 10, oRow["confirmstartdate"] == DBNull.Value ? "" : ClientUtil.ToDateTime(oRow["confirmstartdate"]).ToString("yyyy-MM-dd"));
                    SetValue(iRowIndex, 11, oRow["confirmenddate"] == DBNull.Value ? "" : ClientUtil.ToDateTime(oRow["confirmenddate"]).ToString("yyyy-MM-dd"));
                    dNotGathering = dMustGathering - dGathering;
                    SetValue(iRowIndex, 12, dNotGathering.ToString("N2"));
                    //if (oRow["confirmdate"] != DBNull.Value && oRow["GatheringDate"] != DBNull.Value)
                    //{
                    //    SetValue(iRowIndex, 13, (minDate < dGatheringDate && minDate < dConfirmDate) ? (dGatheringDate.Date - dConfirmDate.Date).Days.ToString() : "");
                    //}
                    dSumNotGathering += dNotGathering;
                    iCount++;
                }
                iRowIndex = iStart + iCount;
                SetValue(iRowIndex, 3, dTJSumConfrim.ToString("N2"));
                SetValue(iRowIndex, 4, dAZSumConfrim.ToString("N2"));
                SetValue(iRowIndex, 5, dZSSumConfrim.ToString("N2"));
                SetValue(iRowIndex, 7, dSumMustGathering.ToString("N2"));
                SetValue(iRowIndex, 9, dSumGathering.ToString("N2"));
                SetValue(iRowIndex, 12, dSumNotGathering.ToString("N2"));
                reportGrid.BackColor1 = System.Drawing.SystemColors.ButtonFace;
                reportGrid.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
                for (int tt = 0; tt < reportGrid.Cols; tt++)
                {
                    reportGrid.Column(tt).AutoFit();
                }
                reportGrid.AutoRedraw = true;
                reportGrid.Refresh();
            }

        }

        private void LoadOrgNotGatheringData(string sOrgSysCode)
        {

            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            DataTable oTable = null;
            DataSet dataSet = null;
            int iStart = 3, iRowIndex = 0;

            int iCount = 0;
            decimal dTJConfrim = 0, dRate = 0, dTJSumConfrim = 0, dAZConfrim = 0, dAZSumConfrim = 0, dGathering = 0, dSumGathering = 0,
                dMustGathering = 0, dSumMustGathering = 0, dNotGathering = 0, dSumNotGathering = 0;
            startDate = dtpDateBegin.Value.Date;
            endDate = dtpDateEnd.Value.Date;
            dataSet = model.FinanceMultDataSrv.QueryOrgNotGatheringAccountReport(sOrgSysCode, startDate, endDate);
            reportGrid.Cell(1, 1).Text = string.Format("{0}[{1}]至[{2}]应收拖欠工程款台帐", txtOperationOrg.Text, dtpDateBegin.Value.Date.ToString("yyyy年MM月dd日"), dtpDateEnd.Value.Date.ToString("yyyy年MM月dd日"));
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                oTable = dataSet.Tables[0];
                reportGrid.InsertRow(iStart, oTable.Rows.Count);//插入行
                reportGrid.AutoRedraw = false;
                FlexCell.Range range = reportGrid.Range(iStart, 1, iStart + oTable.Rows.Count, reportGrid.Cols - 1);
                CommonUtil.SetFlexGridDetailCenter(range);
                range.Locked = true;
                foreach (DataRow oRow in oTable.Rows)
                {//orgName projectid projectname gatheringrate azConfrim tjConfrim acctgatheringmoney gathering
                    //projectid projectname quantitytype gatheringrate MustGathering Gathering
                    iRowIndex = iStart + iCount;
                    SetValue(iRowIndex, 1, (iCount + 1).ToString());
                    SetValue(iRowIndex, 2, ClientUtil.ToString(oRow["orgName"]));
                    SetValue(iRowIndex, 3, ClientUtil.ToString(oRow["projectname"]));

                    dTJConfrim = ClientUtil.ToDecimal(oRow["tjConfrim"]);
                    dAZConfrim = ClientUtil.ToDecimal(oRow["azConfrim"]);
                    dTJSumConfrim += dTJConfrim;
                    dAZSumConfrim += dAZConfrim;
                    SetValue(iRowIndex, 4, dTJConfrim.ToString("N2"));
                    SetValue(iRowIndex, 5, dAZConfrim.ToString("N2"));


                    dRate = ClientUtil.ToDecimal(oRow["gatheringrate"]);
                    SetValue(iRowIndex, 6, (dRate * 100).ToString("N2"));
                    //合同应收款
                    dMustGathering = ClientUtil.ToDecimal(oRow["acctgatheringmoney"]);
                    SetValue(iRowIndex, 7, dMustGathering.ToString("N2"));
                    dSumMustGathering += dMustGathering;
                    //收款
                    dGathering = ClientUtil.ToDecimal(oRow["Gathering"]);
                    SetValue(iRowIndex, 8, dGathering.ToString("N2"));
                    dSumGathering += dGathering;
                    //未收款
                    
                    dNotGathering =ClientUtil.ToInt(oRow["flag"])==0?0: (dMustGathering - dGathering);//应收未收工程款=累计审定金额*最大收款比例-累计收款金额
                    SetValue(iRowIndex, 9, dNotGathering.ToString("N2"));
                    dSumNotGathering += dNotGathering;
                    iCount++;
                }
                iRowIndex = iStart + iCount;
                SetValue(iRowIndex, 4, dTJSumConfrim.ToString("N2"));
                SetValue(iRowIndex, 5, dAZSumConfrim.ToString("N2"));
                SetValue(iRowIndex, 8, dSumGathering.ToString("N2"));
                SetValue(iRowIndex, 7, dSumMustGathering.ToString("N2"));
                SetValue(iRowIndex, 9, dSumNotGathering.ToString("N2"));
                reportGrid.BackColor1 = System.Drawing.SystemColors.ButtonFace;
                reportGrid.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
                for (int tt = 0; tt < reportGrid.Cols; tt++)
                {
                    reportGrid.Column(tt).AutoFit();
                }

                reportGrid.AutoRedraw = true;
                reportGrid.Refresh();
            }
            btnSearch.Focus();
        }
        private void SetValue(int iRowIndex, int iColumnIndex, string sValue)
        {
            FlexCell.Cell oCell = reportGrid.Cell(iRowIndex, iColumnIndex);
            oCell.Text = sValue;
            // oCell.Alignment = FlexCell.AlignmentEnum.RightCenter;
            // oCell.set_Border(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            // oCell.FontSize = 12;
        }
    }
}
