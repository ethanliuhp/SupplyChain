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
   public  enum QueryState
    {
        项目 = 0,
        组织 = 1,
        其他=2
    }
    public partial class VGatheringMngReport : TBasicDataView
    {
        #region
        const string ConstProjectGatheringReport = @"项目收款台帐.flx";
        const string ConstProjectNotGatheringReport = @"项目应收未收工程款台账.flx";
        const string ConstOrgGatheringReport = @"分支机构收款单.flx";
        const string ConstOrgNotGatheringReport = @"分支结构应收未收工程款台账.flx";
        QueryState queryState = QueryState.其他;
        //string ReportPath = string.Empty;
     
        #endregion
        private MFinanceMultData model = new MFinanceMultData();
        CurrentProjectInfo projectInfo;
        CGatheringMng_ExecType _ExecType;
        public VGatheringMngReport(CGatheringMng_ExecType execType)
        {
            InitializeComponent();
            this._ExecType = execType;
            IntialData();
            IntialEvent();
        }
        public void IntialData()
        {
             
            string sValue = string.Empty;
            projectInfo=StaticMethod.GetProjectInfo();
            dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.Date.AddMonths(-1);
            dtpDateEnd.Value = ConstObject.TheLogin.LoginDate.Date;
            projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
            {
                this.lblPSelect.Visible = false;
                this.txtOperationOrg.Visible = false;
                this.btnOperationOrg.Visible = false;
                this.reportGrid.Column(1).Visible = false;
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
            
            this.btnExcel.Click+=new EventHandler(btnExcel_Click);
            btnOperationOrg.Click+=new EventHandler(btnOperationOrg_Click);
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
        public void btnSearchProject_Click( string sProjectID)
        {
            LoadTempleteFile(ConstProjectGatheringReport);
            LoadProjectGatheringData(sProjectID);
        }
        public void btnSearchOrg_Click( string sOrgSysCode)
        {
            LoadTempleteFile(ConstOrgGatheringReport );
            LoadOrgGatheringData(sOrgSysCode);
        }
        void btnExcel_Click(object sender, EventArgs e)
        {
            if (queryState == QueryState.其他)
            {
                MessageBox.Show("请先查询报表");
            }
            else
            {
                string reportName = queryState==QueryState.项目? "项目收款台账" : "工程款回收汇总表";
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
        private void LoadProjectGatheringData(string sProjectID)
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
       

            // sProjectID = projectInfo.Id;
            reportGrid.Column(3).Visible = false;
            startDate = dtpDateBegin.Value.Date;
            endDate = dtpDateEnd.Value.Date;
            reportGrid.Column(14).AutoFit();
            reportGrid.Column(16).AutoFit();
            
            int iStart = 4;
            int iCount = 0;
            decimal dSumGathering = 0, dSumBill = 0, dSumWaterElecMoney = 0, dSumPenaltyMoney = 0,
                dSumOtherMoney = 0, dSumWorkerMoney = 0, dSumConcreteMoney = 0, dSumAgreementMoney = 0, dSumOtherItemMoney = 0,
                dSumMoney = 0, dSumInvoice = 0,dSumHB=0;
            reportGrid.Cell(1, 1).Text = string.Format("{0}[{1}]至[{2}]收款台账", txtOperationOrg.Text, dtpDateBegin.Value.Date.ToString("yyyy年MM月dd日"), dtpDateEnd.Value.Date.ToString("yyyy年MM月dd日"));
            IList lst = model.FinanceMultDataSrv.QueryProjectGatheringAccountReport(sProjectID, startDate, endDate);
            if (lst != null && lst.Count > 0)
            {
                iCount = lst.Count;
                reportGrid.InsertRow(iStart, iCount);
                iCount = 0;
                reportGrid.AutoRedraw = false;
                FlexCell.Range range = reportGrid.Range(iStart, 1, iStart + lst.Count, reportGrid.Cols - 1);
                CommonUtil.SetFlexGridDetailCenter(range);
                string sMasterID = string.Empty;
                int iStartRow = -1;
                reportGrid.Column(15).Mask = FlexCell.MaskEnum.DefaultMask;
                foreach (DataDomain oData in lst)
                {
                    SetValue(iStart + iCount, 1, (iCount + 1).ToString());//序号
                    reportGrid.Cell(iStart + iCount, 1).Tag = ClientUtil.ToString(oData.Name1);//收款明细ID 
                    reportGrid.Cell(iStart + iCount, 2).Tag = ClientUtil.ToString(oData.Name18);//收款主表ID 
                    SetValue(iStart + iCount, 2, ClientUtil.ToString(oData.Name2));//项目名称
                    //if (!string.Equals(sMasterID, ClientUtil.ToString(oData.Name18)))
                    //{
                    //    if (iStartRow != -1 && iCount - iStartRow > 1)
                    //    {
                    //        range = reportGrid.Range(iStart + iStartRow, 14, iStart + iCount - 1, 14);
                    //        range.Merge();
                    //        range = reportGrid.Range(iStart + iStartRow, 15, iStart + iCount - 1, 15);
                    //        range.Merge();
                    //        range = reportGrid.Range(iStart + iStartRow, 16, iStart + iCount - 1, 16);
                    //        range.Merge();
                    //    }
                    //    iStartRow = iCount;
                    //    sMasterID = ClientUtil.ToString(oData.Name18);
                    //}
                    SetValue(iStart + iCount, 2, ClientUtil.ToDateTime(oData.Name3).ToString("yyyy-MM-dd"));//收款时间

                    SetValue(iStart + iCount, 3, string.Format("{0:N2}", ClientUtil.ToDecimal(oData.Name4)));//收款金额

                    dSumGathering += ClientUtil.ToDecimal(oData.Name4);
                    SetValue(iStart + iCount, 4, string.Format("{0:N2}", ClientUtil.ToDecimal(oData.Name5)));//货币
                    dSumHB += ClientUtil.ToDecimal(oData.Name5);
                    SetValue(iStart + iCount, 5, string.Format("{0:N2}", ClientUtil.ToDecimal(oData.Name6)));//票据金额

                    dSumBill += ClientUtil.ToDecimal(oData.Name6);
                    SetValue(iStart + iCount, 6, string.Format("{0:N2}", ClientUtil.ToDecimal(oData.Name7))); ;//代扣水费

                    dSumWaterElecMoney += ClientUtil.ToDecimal(oData.Name7);
                    SetValue(iStart + iCount, 7, string.Format("{0:N2}", ClientUtil.ToDecimal(oData.Name8))); ;//代扣罚款

                    dSumPenaltyMoney += ClientUtil.ToDecimal(oData.Name8);
                    SetValue(iStart + iCount, 8, string.Format("{0:N2}", ClientUtil.ToDecimal(oData.Name9))); ;//代扣费用其他

                    dSumOtherMoney += ClientUtil.ToDecimal(oData.Name9);
                    SetValue(iStart + iCount, 9, string.Format("{0:N2}", ClientUtil.ToDecimal(oData.Name10))); ;//代扣农民保障金

                    dSumWorkerMoney += ClientUtil.ToDecimal(oData.Name10);
                    SetValue(iStart + iCount, 10, string.Format("{0:N2}", ClientUtil.ToDecimal(oData.Name11))); ;//代扣散装水泥押金

                    dSumConcreteMoney += ClientUtil.ToDecimal(oData.Name11);
                    SetValue(iStart + iCount, 11, string.Format("{0:N2}", ClientUtil.ToDecimal(oData.Name12))); ;//代扣履约保证金

                    dSumAgreementMoney += ClientUtil.ToDecimal(oData.Name12);
                    SetValue(iStart + iCount, 12, string.Format("{0:N2}", ClientUtil.ToDecimal(oData.Name13))); ;//代扣款项其他

                    dSumOtherItemMoney += ClientUtil.ToDecimal(oData.Name13);
                    SetValue(iStart + iCount, 13, string.Format("{0:N2}", ClientUtil.ToDecimal(oData.Name14))); ;//合计 

                    dSumMoney += ClientUtil.ToDecimal(oData.Name14);
                    SetValue(iStart + iCount, 14, ClientUtil.ToString(oData.Name15));//发票日期
                    reportGrid.Cell(iStart + iCount, 14).WrapText = true;
                    SetValue(iStart + iCount, 15,string.Equals(reportGrid.Cell(iStart + iCount-1, 2).Tag, reportGrid.Cell(iStart + iCount, 2).Tag )?"":string.Format("{0:N2}", ClientUtil.ToDecimal(oData.Name16))); ;//发票金额

                    dSumInvoice += ClientUtil.ToDecimal(oData.Name16);
                    reportGrid.Cell(iStart + iCount, 16).Mask = FlexCell.MaskEnum.DefaultMask;
                    reportGrid.Cell(iStart + iCount, 16).CellType = FlexCell.CellTypeEnum.TextBox;
                    reportGrid.Cell(iStart + iCount, 16).WrapText = true;
                    SetValue(iStart + iCount, 16, ClientUtil.ToString(oData.Name17));//发票号
                    
                    iCount++;
                }
               
                    SetValue(iStart + iCount, 3, string.Format("{0:N2}", dSumGathering));
                SetValue(iStart + iCount, 4, string.Format("{0:N2}", dSumHB));
                SetValue(iStart + iCount, 5, string.Format("{0:N2}", dSumBill));

                SetValue(iStart + iCount, 6, string.Format("{0:N2}", dSumWaterElecMoney));

                SetValue(iStart + iCount, 7, string.Format("{0:N2}", dSumPenaltyMoney));

                SetValue(iStart + iCount, 8, string.Format("{0:N2}", dSumOtherMoney));

                SetValue(iStart + iCount, 9, string.Format("{0:N2}", dSumWorkerMoney));

                SetValue(iStart + iCount, 10, string.Format("{0:N2}", dSumConcreteMoney));

                SetValue(iStart + iCount, 11, string.Format("{0:N2}", dSumAgreementMoney));

                SetValue(iStart + iCount, 12, string.Format("{0:N2}", dSumOtherItemMoney));

                SetValue(iStart + iCount, 13, string.Format("{0:N2}", dSumMoney));

                SetValue(iStart + iCount, 15, string.Format("{0:N2}", dSumInvoice));
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

        private void LoadOrgGatheringData(string sOrgSysCode)
        {

            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
        
            DataTable oTable = null;
            DataSet dataSet = null;
            int iStart = 4, iRowIndex = 0;

            int iCount = 0;
            decimal dSumGathering = 0, dSumCurreyMoney = 0, dSumBill = 0, dSumWaterElecMoney = 0, dSumPenaltyMoney = 0,
                dSumOtherMoney = 0, dSumWorkerMoney = 0, dSumConcreteMoney = 0, dSumAgreementMoney = 0, dSumOtherItemMoney = 0,
                dSumMoney = 0, dSumInvoice = 0;

            reportGrid.Column(4).Visible = false;
            startDate = dtpDateBegin.Value.Date;
            endDate = dtpDateEnd.Value.Date;
            reportGrid.Cell(1, 1).Text = string.Format("{0}[{1}]至[{2}]收款台账", txtOperationOrg.Text, dtpDateBegin.Value.Date.ToString("yyyy年MM月dd日"), dtpDateEnd.Value.Date.ToString("yyyy年MM月dd日"));
            dataSet = model.FinanceMultDataSrv.QueryOrgGatheringAccountReport(sOrgSysCode, startDate, endDate);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                oTable = dataSet.Tables[0];
                reportGrid.InsertRow(iStart, oTable.Rows.Count);//插入行
                reportGrid.AutoRedraw = false;
                FlexCell.Range range = reportGrid.Range(iStart, 1, iStart + oTable.Rows.Count, reportGrid.Cols - 1);
                CommonUtil.SetFlexGridDetailCenter(range);
                foreach (DataRow oRow in oTable.Rows)
                {
                    //projectname gatheringmoney CurreyBillMoney AcceptanceBillMoney WaterElecMoney PenaltyMoney
                    //othermoney WorkerMoney ConcreteMoney AgreementMoney OtherItemMoney  summoney
                    iRowIndex = iStart + iCount;
                    SetValue(iRowIndex, 1, (iCount + 1).ToString());
                    SetValue(iRowIndex, 2, ClientUtil.ToString(oRow["opgname"]));
                    SetValue(iRowIndex, 3, ClientUtil.ToString(oRow["projectname"]));
                    SetValue(iRowIndex, 4, ClientUtil.ToDecimal(oRow["gatheringmoney"]).ToString("N2"));
                    dSumGathering += ClientUtil.ToDecimal(oRow["gatheringmoney"]);
                    SetValue(iRowIndex, 5, ClientUtil.ToDecimal(oRow["CurreyBillMoney"]).ToString("N2"));
                    dSumCurreyMoney += ClientUtil.ToDecimal(oRow["CurreyBillMoney"]);
                    SetValue(iRowIndex, 6, ClientUtil.ToDecimal(oRow["AcceptanceBillMoney"]).ToString("N2"));
                    dSumBill += ClientUtil.ToDecimal(oRow["AcceptanceBillMoney"]);
                    SetValue(iRowIndex, 7, ClientUtil.ToDecimal(oRow["WaterElecMoney"]).ToString("N2"));
                    dSumWaterElecMoney += ClientUtil.ToDecimal(oRow["WaterElecMoney"]);
                    SetValue(iRowIndex, 8, ClientUtil.ToDecimal(oRow["PenaltyMoney"]).ToString("N2"));
                    dSumPenaltyMoney += ClientUtil.ToDecimal(oRow["PenaltyMoney"]);
                    SetValue(iRowIndex, 9, ClientUtil.ToDecimal(oRow["othermoney"]).ToString("N2"));
                    dSumOtherMoney += ClientUtil.ToDecimal(oRow["othermoney"]);
                    SetValue(iRowIndex, 10, ClientUtil.ToDecimal(oRow["WorkerMoney"]).ToString("N2"));
                    dSumWorkerMoney += ClientUtil.ToDecimal(oRow["WorkerMoney"]);
                    SetValue(iRowIndex, 11, ClientUtil.ToDecimal(oRow["ConcreteMoney"]).ToString("N2"));
                    dSumConcreteMoney += ClientUtil.ToDecimal(oRow["ConcreteMoney"]);
                    SetValue(iRowIndex, 12, ClientUtil.ToDecimal(oRow["AgreementMoney"]).ToString("N2"));
                    dSumAgreementMoney += ClientUtil.ToDecimal(oRow["AgreementMoney"]);
                    SetValue(iRowIndex, 13, ClientUtil.ToDecimal(oRow["OtherItemMoney"]).ToString("N2"));
                    dSumOtherItemMoney += ClientUtil.ToDecimal(oRow["OtherItemMoney"]);
                    SetValue(iRowIndex, 14, ClientUtil.ToDecimal(oRow["InvoiceMoney"]).ToString("N2"));
                    dSumInvoice += ClientUtil.ToDecimal(oRow["InvoiceMoney"]);

                    SetValue(iRowIndex, 15, ClientUtil.ToDecimal(oRow["summoney"]).ToString("N2"));
                    dSumMoney += ClientUtil.ToDecimal(oRow["summoney"]);
                    iCount++;
                }
                iRowIndex = iStart + iCount;
                SetValue(iRowIndex, 4, dSumGathering.ToString("N2"));
                SetValue(iRowIndex, 5, dSumCurreyMoney.ToString("N2"));
                SetValue(iRowIndex, 6, dSumBill.ToString("N2"));
                SetValue(iRowIndex, 7, dSumWaterElecMoney.ToString("N2"));
                SetValue(iRowIndex, 8, dSumPenaltyMoney.ToString("N2"));
                SetValue(iRowIndex, 9, dSumOtherMoney.ToString("N2"));
                SetValue(iRowIndex, 10, dSumWorkerMoney.ToString("N2"));
                SetValue(iRowIndex, 11, dSumConcreteMoney.ToString("N2"));
                SetValue(iRowIndex, 12, dSumAgreementMoney.ToString("N2"));
                SetValue(iRowIndex, 13, dSumOtherItemMoney.ToString("N2"));
                SetValue(iRowIndex, 14, dSumInvoice.ToString("N2"));

                SetValue(iRowIndex, 15, dSumMoney.ToString("N2"));
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
        
        private void SetValue(int iRowIndex, int iColumnIndex,string sValue)
        {
          FlexCell.Cell oCell=  reportGrid.Cell(iRowIndex, iColumnIndex);
          oCell.Text = sValue;
          //oCell.Alignment = FlexCell.AlignmentEnum.RightCenter;
          //oCell.set_Border(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
          //oCell.FontSize = 12;
        }

       
    }
}
