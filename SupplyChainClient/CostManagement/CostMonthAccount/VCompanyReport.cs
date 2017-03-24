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
using Application.Business.Erp.SupplyChain.Client.Util;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.OwnerQuantityMng.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    public partial class VCompanyReport : TBasicDataView
    {
        #region 报表名称
        /// <summary>
        /// 项目经济效益一览表
        /// </summary>
        private string reportXMJJXYYLB = "项目经济效益一览表";
        /// <summary>
        /// 项目经济效益综合表
        /// </summary>
        private string reportXMJJXYZHB = "项目经济效益综合表";
        /// <summary>
        /// 成本消耗指标一览表
        /// </summary>
        private string reportCBXHZBYLB = "成本消耗指标一览表";
        /// <summary>
        /// 成本消耗指标综合表
        /// </summary>
        private string reportCBXHZBZHB = "成本消耗指标综合表";
        /// <summary>
        /// 技经指标一览表
        /// </summary>
        private string reportJJZBYLB = "技经指标一览表";
        /// <summary>
        /// 收入成本变化情况一览表
        /// </summary>
        private string reportSRCBBHQKYLB = "收入成本变化情况一览表";
        /// <summary>
        /// 收入成本变化情况综合表
        /// </summary>
        private string reportSRCBBHQKZHB = "收入成本变化情况综合表";
        /// <summary>
        /// 财务收款情况一览表
        /// </summary>
        private string reportCWSKQKYLB = "财务收款情况一览表";
        /// <summary>
        /// 财务收款情况综合表
        /// </summary>
        private string reportCWSKQKZHB = "财务收款情况综合表";
        #endregion
        MCostMonthAccount model = new MCostMonthAccount();
        private int reportType = 0;
        private string reportDateStr = "";
        private IList resultList = new ArrayList();
        private IList resultTransList = new ArrayList();
        private Hashtable ht_resultdata = new Hashtable();
        Hashtable ht_project = new Hashtable();
        Hashtable ht_ownerQty = new Hashtable();
        Hashtable ht_gwbsLedger = new Hashtable();
        Hashtable ht_sumData = new Hashtable();
        Hashtable ht_subject = new Hashtable();
        Hashtable ht_indicator = new Hashtable();
        public VCompanyReport(int types)
        {
            reportType = types;
            InitializeComponent();
            InitEvent();
            InitData();
        }

        private void InitEvent()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
        }

        private void InitData()
        {
            //初始会计期
            for (int i = 0; i < 13; i++)
            {
                this.cboFiscalYear.Items.Add(ConstObject.TheLogin.TheComponentPeriod.NowYear + (i - 3));
            }
            for (int i = 1; i < 13; i++)
            {
                this.cboFiscalMonth.Items.Add(i);
            }

            this.cboFiscalYear.Text = ConstObject.TheLogin.TheComponentPeriod.NowYear.ToString();
            this.cboFiscalMonth.Text = ConstObject.TheLogin.TheComponentPeriod.NowMonth.ToString();

            //初始报表名称
            if (reportType == 1)
            {
                cmbReportName.Items.Add(reportXMJJXYYLB);
                cmbReportName.Items.Add(reportXMJJXYZHB);
            }
            else if (reportType == 2)
            {
                cmbReportName.Items.Add(reportCBXHZBYLB);
                cmbReportName.Items.Add(reportCBXHZBZHB);
            }
            else if (reportType == 3)
            {
                cmbReportName.Items.Add(reportJJZBYLB);
            }
            else if (reportType == 4)
            {
                cmbReportName.Items.Add(reportSRCBBHQKYLB);
                cmbReportName.Items.Add(reportSRCBBHQKZHB);
            }
            else if (reportType == 5)
            {
                cmbReportName.Items.Add(reportCWSKQKYLB);
                cmbReportName.Items.Add(reportCWSKQKZHB);
            }
            cmbReportName.SelectedIndex = 0;

            this.fGrid_EcoProfitMx.Rows = 1;
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            string reportName = cmbReportName.Text + "";
            string fileName = fGrid_EcoProfitMx.ExportToExcel(reportName, false, false, true);
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            if (ClientUtil.ToString(this.cboFiscalYear.Text) == "")
            {
                MessageBox.Show("请输入会计年！");
                return;
            }
            if (ClientUtil.ToString(this.cboFiscalMonth.Text) == "")
            {
                MessageBox.Show("请输入会计月！");
                return;
            }

            FlashScreen.Show("正在统计公司项目报表...");
            try
            {
                reportDateStr = this.cboFiscalYear.Text + "年" + this.cboFiscalMonth.Text + "月";
                string queryStr = this.cmbReportName.Text;
                LoadTempleteFile(reportXMJJXYYLB + ".flx");
                LoadTempleteFile(reportXMJJXYZHB + ".flx");
                LoadTempleteFile(reportCBXHZBYLB + ".flx");
                LoadTempleteFile(reportCBXHZBZHB + ".flx");
                LoadTempleteFile(reportJJZBYLB + ".flx");
                LoadTempleteFile(reportSRCBBHQKYLB + ".flx");
                LoadTempleteFile(reportSRCBBHQKZHB + ".flx");
                LoadTempleteFile(reportCWSKQKYLB + ".flx");
                LoadTempleteFile(reportCWSKQKZHB + ".flx");

                resultList = model.CostMonthAccSrv.GetAllProjectList();
                ht_resultdata = model.CostMonthAccSrv.GetCompanyReportData(ClientUtil.ToInt(this.cboFiscalYear.Text), ClientUtil.ToInt(this.cboFiscalMonth.Text));
                resultTransList = this.GetReportList(1);

                ht_project = (Hashtable)ht_resultdata["1"];
                ht_ownerQty = (Hashtable)ht_resultdata["2"];
                ht_gwbsLedger = (Hashtable)ht_resultdata["3"];
                ht_sumData = (Hashtable)ht_resultdata["4"];
                ht_subject = (Hashtable)ht_resultdata["5"];
                ht_indicator = (Hashtable)ht_resultdata["6"];


                LoadEconomyProfitMxData();
                LoadCostConsumeMxData();
                LoadTechIndicatorData();
                LoadIncomeCostMxData();
                LoadReceiveMoneyMxData();

                //设置外观
                CommonUtil.SetFlexGridFace(this.fGrid_EcoProfitMx);
                CommonUtil.SetFlexGridFace(this.fGrid_EcoProfit);
                fGrid_CostConsumeMx.BackColor1 = System.Drawing.SystemColors.ButtonFace;
                fGrid_CostConsumeMx.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
                fGrid_CostConsume.BackColor1 = System.Drawing.SystemColors.ButtonFace;
                fGrid_CostConsume.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
                //CommonUtil.SetFlexGridFace(this.fGrid_CostConsumeMx);
                //CommonUtil.SetFlexGridFace(this.fGrid_CostConsume);
                CommonUtil.SetFlexGridFace(this.fGrid_TechIndicator);
                CommonUtil.SetFlexGridFace(this.fGrid_IncomeCostMx);
                CommonUtil.SetFlexGridFace(this.fGrid_IncomeCost);
                CommonUtil.SetFlexGridFace(this.fGrid_ReceiveMoneyMx);
                CommonUtil.SetFlexGridFace(this.fGrid_ReceiveMoney);
            }
            catch (Exception e1)
            {
                throw new Exception("统计公司项目套表出错！");
            }
            finally {
                FlashScreen.Close();
            }
        }

        private void LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式
                if (modelName == reportXMJJXYYLB + ".flx")
                {
                    this.fGrid_EcoProfitMx.OpenFile(path + "\\" + modelName);//载入格式
                    this.fGrid_EcoProfitMx.Cell(1, 1).Text = reportDateStr + modelName.Substring(0, modelName.IndexOf("."));
                }
                else if (modelName == reportXMJJXYZHB + ".flx")
                {
                    this.fGrid_EcoProfit.OpenFile(path + "\\" + modelName);//载入格式
                    this.fGrid_EcoProfit.Cell(1, 1).Text = reportDateStr + modelName.Substring(0, modelName.IndexOf("."));
                }
                else if (modelName == reportCBXHZBYLB + ".flx")
                {
                    this.fGrid_CostConsumeMx.OpenFile(path + "\\" + modelName);//载入格式
                    this.fGrid_CostConsumeMx.Cell(1, 1).Text = reportDateStr + modelName.Substring(0, modelName.IndexOf("."));
                }
                else if (modelName == reportCBXHZBZHB + ".flx")
                {
                    this.fGrid_CostConsume.OpenFile(path + "\\" + modelName);//载入格式
                    this.fGrid_CostConsume.Cell(1, 1).Text = reportDateStr + modelName.Substring(0, modelName.IndexOf("."));
                }
                else if (modelName == reportJJZBYLB + ".flx")
                {
                    this.fGrid_TechIndicator.OpenFile(path + "\\" + modelName);//载入格式
                    this.fGrid_TechIndicator.Cell(1, 1).Text = reportDateStr + modelName.Substring(0, modelName.IndexOf("."));
                }
                else if (modelName == reportSRCBBHQKYLB + ".flx")
                {
                    this.fGrid_IncomeCostMx.OpenFile(path + "\\" + modelName);//载入格式
                    this.fGrid_IncomeCostMx.Cell(1, 1).Text = reportDateStr + modelName.Substring(0, modelName.IndexOf("."));
                }
                else if (modelName == reportSRCBBHQKZHB + ".flx")
                {
                    this.fGrid_IncomeCost.OpenFile(path + "\\" + modelName);//载入格式
                    this.fGrid_IncomeCost.Cell(1, 1).Text = reportDateStr + modelName.Substring(0, modelName.IndexOf("."));
                }
                else if (modelName == reportCWSKQKYLB + ".flx")
                {
                    this.fGrid_ReceiveMoneyMx.OpenFile(path + "\\" + modelName);//载入格式
                    this.fGrid_ReceiveMoneyMx.Cell(1, 1).Text = reportDateStr + modelName.Substring(0, modelName.IndexOf("."));
                }
                else if (modelName == reportCWSKQKZHB + ".flx")
                {
                    this.fGrid_ReceiveMoney.OpenFile(path + "\\" + modelName);//载入格式
                    this.fGrid_ReceiveMoney.Cell(1, 1).Text = reportDateStr + modelName.Substring(0, modelName.IndexOf("."));
                }
                
            }
            else
            {
                MessageBox.Show("未找到模板格式文件" + modelName);
                return;
            }
        }

        #region 项目经济效益一览表/综合表
        private void LoadEconomyProfitMxData()
        {
            IList list_main = new ArrayList();

            #region  项目经济效益一览表
            int dtlStartRowNum = 5;//模板中的行号
            int dtlCount = this.resultTransList.Count;

            //插入明细行
            this.fGrid_EcoProfitMx.InsertRow(dtlStartRowNum, dtlCount);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGrid_EcoProfitMx.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGrid_EcoProfitMx.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            //写入明细数据
            DataDomain domain = new DataDomain();
            for (int i = 0; i < dtlCount; i++)
            {
                CurrentProjectInfo project = (CurrentProjectInfo)resultTransList[i];

                fGrid_EcoProfitMx.Cell(dtlStartRowNum + i, 1).Text = project.Data2;
                fGrid_EcoProfitMx.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGrid_EcoProfitMx.Cell(dtlStartRowNum + i, 2).Text = project.Name;
                fGrid_EcoProfitMx.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                if (project.OwnerOrgName != "头" && project.OwnerOrgName != "尾")
                {
                    if (ht_project.Contains(project.Id))
                    {
                        fGrid_EcoProfitMx.Cell(dtlStartRowNum + i, 3).Text = ((CurrentProjectInfo)ht_project[project.Id]).ResProportion + "";
                    }
                    
                }
                if (project.Id != null && ht_sumData.Contains(project.Id))
                {
                    DataDomain sumDomain = (DataDomain)ht_sumData[project.Id];
                    fGrid_EcoProfitMx.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(sumDomain.Name1);
                    fGrid_EcoProfitMx.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(sumDomain.Name2);
                    domain.Name1 = (ClientUtil.ToDecimal(domain.Name1) + ClientUtil.ToDecimal(sumDomain.Name1)) + "";
                    domain.Name2 = (ClientUtil.ToDecimal(domain.Name2) + ClientUtil.ToDecimal(sumDomain.Name2)) + "";
                    fGrid_EcoProfitMx.Cell(dtlStartRowNum + i, 6).Text = (ClientUtil.ToDecimal(sumDomain.Name1) - ClientUtil.ToDecimal(sumDomain.Name2)) + "";
                    if (ClientUtil.ToDecimal(sumDomain.Name1) != 0)
                    {
                        decimal tValue = decimal.Round((ClientUtil.ToDecimal(sumDomain.Name1) - ClientUtil.ToDecimal(sumDomain.Name2)) *100/ ClientUtil.ToDecimal(sumDomain.Name1), 2);
                        fGrid_EcoProfitMx.Cell(dtlStartRowNum + i, 7).Text = tValue + "";
                        if (tValue >= 1)
                        {
                            fGrid_EcoProfitMx.Cell(dtlStartRowNum + i, 8).Text = "是";
                        }
                        else
                        {
                            fGrid_EcoProfitMx.Cell(dtlStartRowNum + i, 8).Text = "否";
                        }
                    }
                }
                if (project.OwnerOrgName == "尾")
                {
                    domain.Name30 = project.Name;
                    fGrid_EcoProfitMx.Cell(dtlStartRowNum + i, 2).Text = project.Name + "小计";
                    fGrid_EcoProfitMx.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                    fGrid_EcoProfitMx.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(domain.Name1);
                    fGrid_EcoProfitMx.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(domain.Name2);
                    fGrid_EcoProfitMx.Cell(dtlStartRowNum + i, 6).Text = (ClientUtil.ToDecimal(domain.Name1) - ClientUtil.ToDecimal(domain.Name2)) + "";
                    if (ClientUtil.ToDecimal(domain.Name1) != 0)
                    {
                        decimal tValue = decimal.Round((ClientUtil.ToDecimal(domain.Name1) - ClientUtil.ToDecimal(domain.Name2)) *100/ ClientUtil.ToDecimal(domain.Name1), 2);
                        fGrid_EcoProfitMx.Cell(dtlStartRowNum + i, 7).Text = tValue + "";
                        if (tValue >= 1)
                        {
                            fGrid_EcoProfitMx.Cell(dtlStartRowNum + i, 8).Text = "是";
                        }
                        else
                        {
                            fGrid_EcoProfitMx.Cell(dtlStartRowNum + i, 8).Text = "否";
                        }
                    }
                    list_main.Add(domain);
                    domain = new DataDomain();
                }
            }
            #endregion

            #region 项目经济效益综合表
            dtlStartRowNum = 4;//模板中的行号
            dtlCount = list_main.Count;

            //插入明细行
            this.fGrid_EcoProfit.InsertRow(dtlStartRowNum, dtlCount + 2);
            //设置单元格的边框，对齐方式
            range = fGrid_EcoProfit.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount + 1, fGrid_EcoProfit.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);
            DataDomain sum_domain = new DataDomain();
            for (int i = 0; i < dtlCount; i++)
            {
                DataDomain m_domain = (DataDomain)list_main[i];

                fGrid_EcoProfit.Cell(dtlStartRowNum + i, 1).Text = (i + 1) + "";
                fGrid_EcoProfit.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGrid_EcoProfit.Cell(dtlStartRowNum + i, 2).Text = ClientUtil.ToString(m_domain.Name30);
                fGrid_EcoProfit.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGrid_EcoProfit.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(m_domain.Name1);
                fGrid_EcoProfit.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(m_domain.Name2);
                sum_domain.Name1 = (ClientUtil.ToDecimal(sum_domain.Name1) + ClientUtil.ToDecimal(m_domain.Name1)) + "";
                sum_domain.Name2 = (ClientUtil.ToDecimal(sum_domain.Name2) + ClientUtil.ToDecimal(m_domain.Name2)) + "";
                fGrid_EcoProfit.Cell(dtlStartRowNum + i, 5).Text = (ClientUtil.ToDecimal(m_domain.Name1) - ClientUtil.ToDecimal(m_domain.Name2)) + "";
                if (ClientUtil.ToDecimal(m_domain.Name1) != 0)
                {
                    decimal tValue = decimal.Round((ClientUtil.ToDecimal(m_domain.Name1) - ClientUtil.ToDecimal(m_domain.Name2))  *100/ ClientUtil.ToDecimal(m_domain.Name1), 2);
                    fGrid_EcoProfit.Cell(dtlStartRowNum + i, 6).Text = tValue + "";
                }
            }
            //插入综合值
            fGrid_EcoProfit.Cell(dtlStartRowNum + dtlCount, 1).Text = (dtlCount + 1) + "";
            fGrid_EcoProfit.Cell(dtlStartRowNum + dtlCount, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
            fGrid_EcoProfit.Cell(dtlStartRowNum + dtlCount, 2).Text = "综合指标";
            fGrid_EcoProfit.Cell(dtlStartRowNum + dtlCount, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
            fGrid_EcoProfit.Cell(dtlStartRowNum + dtlCount, 3).Text = ClientUtil.ToString(sum_domain.Name1);
            fGrid_EcoProfit.Cell(dtlStartRowNum + dtlCount, 4).Text = ClientUtil.ToString(sum_domain.Name2);
            fGrid_EcoProfit.Cell(dtlStartRowNum + dtlCount, 5).Text = (ClientUtil.ToDecimal(sum_domain.Name1) - ClientUtil.ToDecimal(sum_domain.Name2)) + "";
            if (ClientUtil.ToDecimal(sum_domain.Name1) != 0)
            {
                decimal tValue = decimal.Round((ClientUtil.ToDecimal(sum_domain.Name1) - ClientUtil.ToDecimal(sum_domain.Name2))*100 / ClientUtil.ToDecimal(sum_domain.Name1), 2);
                fGrid_EcoProfit.Cell(dtlStartRowNum + dtlCount, 6).Text = tValue + "";
            }
            //插入平均值
            fGrid_EcoProfit.Cell(dtlStartRowNum + dtlCount + 1, 1).Text = (dtlCount + 2) + "";
            fGrid_EcoProfit.Cell(dtlStartRowNum + dtlCount + 1, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
            fGrid_EcoProfit.Cell(dtlStartRowNum + dtlCount + 1, 2).Text = "平均指标";
            fGrid_EcoProfit.Cell(dtlStartRowNum + dtlCount + 1, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
            if (dtlCount > 0)
            {
                fGrid_EcoProfit.Cell(dtlStartRowNum + dtlCount + 1, 3).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_EcoProfit.Cell(dtlStartRowNum + dtlCount, 3).Text) / dtlCount, 2) + "";
                fGrid_EcoProfit.Cell(dtlStartRowNum + dtlCount + 1, 4).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_EcoProfit.Cell(dtlStartRowNum + dtlCount, 4).Text) / dtlCount, 2) + "";
                fGrid_EcoProfit.Cell(dtlStartRowNum + dtlCount + 1, 5).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_EcoProfit.Cell(dtlStartRowNum + dtlCount, 5).Text) / dtlCount, 2) + "";
                fGrid_EcoProfit.Cell(dtlStartRowNum + dtlCount + 1, 6).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_EcoProfit.Cell(dtlStartRowNum + dtlCount, 6).Text) / dtlCount, 2) + "";
            }
            #endregion
        }
        #endregion

        #region 成本消耗指标一览表/综合表
        private void LoadCostConsumeMxData()
        {
            IList list_main = new ArrayList();
            #region  成本消耗指标一览表
            int dtlStartRowNum = 5;//模板中的行号
            int dtlCount = this.resultTransList.Count;

            //插入明细行
            this.fGrid_CostConsumeMx.InsertRow(dtlStartRowNum, dtlCount);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGrid_CostConsumeMx.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGrid_CostConsumeMx.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            //写入明细数据
            DataDomain domain = new DataDomain();
            int mxCount = 0;
            for (int i = 0; i < dtlCount; i++)
            {
                CurrentProjectInfo project = (CurrentProjectInfo)resultTransList[i];

                fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 1).Text = project.Data2;
                fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 2).Text = project.Name;
                fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                if (project.Id != null && ht_subject.Contains(project.Id))
                {
                    DataDomain subject_domain = (DataDomain)ht_subject[project.Id];
                    fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(subject_domain.Name1);
                    fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(subject_domain.Name2);
                    fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(subject_domain.Name3);
                    fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(subject_domain.Name4);
                    fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(subject_domain.Name5);
                    fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(subject_domain.Name6);
                    fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(subject_domain.Name7);
                    fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(subject_domain.Name8);
                    fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.ToString(subject_domain.Name9);
                    fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(subject_domain.Name10);
                    domain.Name1 = (ClientUtil.ToDecimal(domain.Name1) + ClientUtil.ToDecimal(subject_domain.Name1)) + "";
                    domain.Name2 = (ClientUtil.ToDecimal(domain.Name2) + ClientUtil.ToDecimal(subject_domain.Name2)) + "";
                    domain.Name3 = (ClientUtil.ToDecimal(domain.Name3) + ClientUtil.ToDecimal(subject_domain.Name3)) + "";
                    domain.Name4 = (ClientUtil.ToDecimal(domain.Name4) + ClientUtil.ToDecimal(subject_domain.Name4)) + "";
                    domain.Name5 = (ClientUtil.ToDecimal(domain.Name5) + ClientUtil.ToDecimal(subject_domain.Name5)) + "";
                    domain.Name6 = (ClientUtil.ToDecimal(domain.Name6) + ClientUtil.ToDecimal(subject_domain.Name6)) + "";
                    domain.Name7 = (ClientUtil.ToDecimal(domain.Name7) + ClientUtil.ToDecimal(subject_domain.Name7)) + "";
                    domain.Name8 = (ClientUtil.ToDecimal(domain.Name8) + ClientUtil.ToDecimal(subject_domain.Name8)) + "";
                    domain.Name9 = (ClientUtil.ToDecimal(domain.Name9) + ClientUtil.ToDecimal(subject_domain.Name9)) + "";
                    domain.Name10 = (ClientUtil.ToDecimal(domain.Name10) + ClientUtil.ToDecimal(subject_domain.Name10)) + "";
                    mxCount++;
                }
                if (project.OwnerOrgName == "尾")
                {
                    domain.Name30 = project.Name;
                    fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 2).Text = project.Name + "平均指标";
                    fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                    domain.Name1 = decimal.Round(ClientUtil.ToDecimal(domain.Name1) / mxCount, 2);
                    domain.Name2 = decimal.Round(ClientUtil.ToDecimal(domain.Name2) / mxCount, 2);
                    domain.Name3 = decimal.Round(ClientUtil.ToDecimal(domain.Name3) / mxCount, 2);
                    domain.Name4 = decimal.Round(ClientUtil.ToDecimal(domain.Name4) / mxCount, 2);
                    domain.Name5 = decimal.Round(ClientUtil.ToDecimal(domain.Name5) / mxCount, 2);
                    domain.Name6 = decimal.Round(ClientUtil.ToDecimal(domain.Name6) / mxCount, 2);
                    domain.Name7 = decimal.Round(ClientUtil.ToDecimal(domain.Name7) / mxCount, 2);
                    domain.Name8 = decimal.Round(ClientUtil.ToDecimal(domain.Name8) / mxCount, 2);
                    domain.Name9 = decimal.Round(ClientUtil.ToDecimal(domain.Name9) / mxCount, 2);
                    domain.Name10 = decimal.Round(ClientUtil.ToDecimal(domain.Name10) / mxCount, 2);

                    fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(domain.Name1);
                    fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(domain.Name2);
                    fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(domain.Name3);
                    fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(domain.Name4);
                    fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(domain.Name5);
                    fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(domain.Name6);
                    fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(domain.Name7);
                    fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(domain.Name8);
                    fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.ToString(domain.Name9);
                    fGrid_CostConsumeMx.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(domain.Name10);

                    list_main.Add(domain);
                    mxCount = 1;
                    domain = new DataDomain();
                }
            }
            #endregion

            #region 成本消耗指标综合表
            dtlStartRowNum = 5;//模板中的行号
            dtlCount = list_main.Count;

            //插入明细行
            this.fGrid_CostConsume.InsertRow(dtlStartRowNum, dtlCount + 1);
            //设置单元格的边框，对齐方式
            range = fGrid_CostConsume.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGrid_CostConsume.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);
            DataDomain sum_domain = new DataDomain();
            mxCount = 0;
            for (int i = 0; i < dtlCount; i++)
            {
                DataDomain subject_domain = (DataDomain)list_main[i];

                fGrid_CostConsume.Cell(dtlStartRowNum + i, 1).Text = (i + 1) + "";
                fGrid_CostConsume.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGrid_CostConsume.Cell(dtlStartRowNum + i, 2).Text = ClientUtil.ToString(subject_domain.Name30);
                fGrid_CostConsume.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGrid_CostConsume.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(subject_domain.Name1);
                fGrid_CostConsume.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(subject_domain.Name2);
                fGrid_CostConsume.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(subject_domain.Name3);
                fGrid_CostConsume.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(subject_domain.Name4);
                fGrid_CostConsume.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(subject_domain.Name5);
                fGrid_CostConsume.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(subject_domain.Name6);
                fGrid_CostConsume.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(subject_domain.Name7);
                fGrid_CostConsume.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(subject_domain.Name8);
                fGrid_CostConsume.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.ToString(subject_domain.Name9);
                fGrid_CostConsume.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(subject_domain.Name10);
                domain.Name1 = (ClientUtil.ToDecimal(domain.Name1) + ClientUtil.ToDecimal(subject_domain.Name1)) + "";
                domain.Name2 = (ClientUtil.ToDecimal(domain.Name2) + ClientUtil.ToDecimal(subject_domain.Name2)) + "";
                domain.Name3 = (ClientUtil.ToDecimal(domain.Name3) + ClientUtil.ToDecimal(subject_domain.Name3)) + "";
                domain.Name4 = (ClientUtil.ToDecimal(domain.Name4) + ClientUtil.ToDecimal(subject_domain.Name4)) + "";
                domain.Name5 = (ClientUtil.ToDecimal(domain.Name5) + ClientUtil.ToDecimal(subject_domain.Name5)) + "";
                domain.Name6 = (ClientUtil.ToDecimal(domain.Name6) + ClientUtil.ToDecimal(subject_domain.Name6)) + "";
                domain.Name7 = (ClientUtil.ToDecimal(domain.Name7) + ClientUtil.ToDecimal(subject_domain.Name7)) + "";
                domain.Name8 = (ClientUtil.ToDecimal(domain.Name8) + ClientUtil.ToDecimal(subject_domain.Name8)) + "";
                domain.Name9 = (ClientUtil.ToDecimal(domain.Name9) + ClientUtil.ToDecimal(subject_domain.Name9)) + "";
                domain.Name10 = (ClientUtil.ToDecimal(domain.Name10) + ClientUtil.ToDecimal(subject_domain.Name10)) + "";
                mxCount++;
            }
            //插入平均值
            fGrid_CostConsume.Cell(dtlStartRowNum + dtlCount, 1).Text = (dtlCount + 1) + "";
            fGrid_CostConsume.Cell(dtlStartRowNum + dtlCount, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
            fGrid_CostConsume.Cell(dtlStartRowNum + dtlCount, 2).Text = "综合指标";
            fGrid_CostConsume.Cell(dtlStartRowNum + dtlCount, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
            if (dtlCount > 0)
            {
                domain.Name1 = decimal.Round(ClientUtil.ToDecimal(domain.Name1) / mxCount, 2);
                domain.Name2 = decimal.Round(ClientUtil.ToDecimal(domain.Name2) / mxCount, 2);
                domain.Name3 = decimal.Round(ClientUtil.ToDecimal(domain.Name3) / mxCount, 2);
                domain.Name4 = decimal.Round(ClientUtil.ToDecimal(domain.Name4) / mxCount, 2);
                domain.Name5 = decimal.Round(ClientUtil.ToDecimal(domain.Name5) / mxCount, 2);
                domain.Name6 = decimal.Round(ClientUtil.ToDecimal(domain.Name6) / mxCount, 2);
                domain.Name7 = decimal.Round(ClientUtil.ToDecimal(domain.Name7) / mxCount, 2);
                domain.Name8 = decimal.Round(ClientUtil.ToDecimal(domain.Name8) / mxCount, 2);
                domain.Name9 = decimal.Round(ClientUtil.ToDecimal(domain.Name9) / mxCount, 2);
                domain.Name10 = decimal.Round(ClientUtil.ToDecimal(domain.Name10) / mxCount, 2);

                fGrid_CostConsume.Cell(dtlStartRowNum + dtlCount, 3).Text = ClientUtil.ToString(domain.Name1);
                fGrid_CostConsume.Cell(dtlStartRowNum + dtlCount, 4).Text = ClientUtil.ToString(domain.Name2);
                fGrid_CostConsume.Cell(dtlStartRowNum + dtlCount, 5).Text = ClientUtil.ToString(domain.Name3);
                fGrid_CostConsume.Cell(dtlStartRowNum + dtlCount, 6).Text = ClientUtil.ToString(domain.Name4);
                fGrid_CostConsume.Cell(dtlStartRowNum + dtlCount, 7).Text = ClientUtil.ToString(domain.Name5);
                fGrid_CostConsume.Cell(dtlStartRowNum + dtlCount, 8).Text = ClientUtil.ToString(domain.Name6);
                fGrid_CostConsume.Cell(dtlStartRowNum + dtlCount, 9).Text = ClientUtil.ToString(domain.Name7);
                fGrid_CostConsume.Cell(dtlStartRowNum + dtlCount, 10).Text = ClientUtil.ToString(domain.Name8);
                fGrid_CostConsume.Cell(dtlStartRowNum + dtlCount, 11).Text = ClientUtil.ToString(domain.Name9);
                fGrid_CostConsume.Cell(dtlStartRowNum + dtlCount, 12).Text = ClientUtil.ToString(domain.Name10);
            }
            #endregion

            this.fGrid_CostConsumeMx.Column(2).AutoFit();
            this.fGrid_CostConsume.Column(2).AutoFit();
        }
        #endregion

        #region 技经指标一览表
        private void LoadTechIndicatorData()
        {
            int dtlStartRowNum = 6;//模板中的行号
            IList list_tech= this.GetValidReportList();
            int dtlCount = list_tech.Count;

            //插入明细行
            this.fGrid_TechIndicator.InsertRow(dtlStartRowNum, dtlCount);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGrid_TechIndicator.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGrid_TechIndicator.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            //写入明细数据
            DataDomain domain = new DataDomain();
            for (int i = 0; i < dtlCount; i++)
            {
                CurrentProjectInfo project = (CurrentProjectInfo)list_tech[i];

                fGrid_TechIndicator.Cell(dtlStartRowNum + i, 1).Text = (i + 1) + "";
                fGrid_TechIndicator.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGrid_TechIndicator.Cell(dtlStartRowNum + i, 2).Text = project.Name;
                fGrid_TechIndicator.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;

                if (project.Id != null &&  this.ht_project.Contains(project.Id))
                {
                    CurrentProjectInfo currProject = (CurrentProjectInfo)ht_project[project.Id];
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(currProject.UnderGroundArea);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(currProject.TheGroundArea);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(currProject.UnderGroundArea + currProject.TheGroundArea);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(currProject.UnderGroundLayers);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(currProject.GroundLayers);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(currProject.BuildingHeight);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 9).Text = "";
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 10).Text = currProject.BaseForm;
                    if (currProject.BeginDate > ClientUtil.ToDateTime("2000-01-01") && currProject.EndDate > ClientUtil.ToDateTime("2000-01-01"))
                    {
                        fGrid_TechIndicator.Cell(dtlStartRowNum + i, 11).Text = currProject.BeginDate.ToShortDateString() + "到" + currProject.EndDate.ToShortDateString();
                    }
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 12).Text = currProject.ProjectLocationProvince + "-" + currProject.ProjectLocationCity;
                }

                if (project.Id != null && ht_indicator.Contains(project.Id))
                {
                    DataDomain indicator_domain = (DataDomain)ht_indicator[project.Id];
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 13).Text = ClientUtil.ToString(indicator_domain.Name1);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 14).Text = ClientUtil.ToString(indicator_domain.Name2);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 15).Text = ClientUtil.ToString(indicator_domain.Name3);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 16).Text = ClientUtil.ToString(indicator_domain.Name4);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 17).Text = ClientUtil.ToString(indicator_domain.Name5);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 18).Text = ClientUtil.ToString(indicator_domain.Name6);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 19).Text = ClientUtil.ToString(indicator_domain.Name7);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 20).Text = ClientUtil.ToString(indicator_domain.Name8);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 21).Text = ClientUtil.ToString(indicator_domain.Name9);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 22).Text = ClientUtil.ToString(indicator_domain.Name10);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 23).Text = ClientUtil.ToString(indicator_domain.Name11);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 24).Text = ClientUtil.ToString(indicator_domain.Name12);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 25).Text = ClientUtil.ToString(indicator_domain.Name13);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 26).Text = ClientUtil.ToString(indicator_domain.Name14);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 27).Text = ClientUtil.ToString(indicator_domain.Name15);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 28).Text = ClientUtil.ToString(indicator_domain.Name16);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 29).Text = ClientUtil.ToString(indicator_domain.Name17);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 30).Text = ClientUtil.ToString(indicator_domain.Name18);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 31).Text = ClientUtil.ToString(indicator_domain.Name19);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 32).Text = ClientUtil.ToString(indicator_domain.Name20);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 33).Text = ClientUtil.ToString(indicator_domain.Name21);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 34).Text = ClientUtil.ToString(indicator_domain.Name22);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 35).Text = ClientUtil.ToString(indicator_domain.Name23);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 36).Text = ClientUtil.ToString(indicator_domain.Name24);
                    fGrid_TechIndicator.Cell(dtlStartRowNum + i, 37).Text = ClientUtil.ToString(indicator_domain.Name25);
                }
            }
        }
        #endregion

        #region 收入成本变化情况一览表/综合表
        private void LoadIncomeCostMxData()
        {
            IList list_main = new ArrayList();

            #region   收入成本变化情况一览表
            int dtlStartRowNum = 6;//模板中的行号
            int dtlCount = this.resultTransList.Count;

            //插入明细行
            this.fGrid_IncomeCostMx.InsertRow(dtlStartRowNum, dtlCount);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGrid_IncomeCostMx.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGrid_IncomeCostMx.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            //写入明细数据
            DataDomain domain = new DataDomain();
            for (int i = 0; i < dtlCount; i++)
            {
                CurrentProjectInfo project = (CurrentProjectInfo)resultTransList[i];

                fGrid_IncomeCostMx.Cell(dtlStartRowNum + i, 1).Text = project.Data2;
                fGrid_IncomeCostMx.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGrid_IncomeCostMx.Cell(dtlStartRowNum + i, 2).Text = project.Name;
                fGrid_IncomeCostMx.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                if (project.Id != null && ht_sumData.Contains(project.Id))
                {
                    DataDomain sum_domain = (DataDomain)ht_sumData[project.Id];
                    fGrid_IncomeCostMx.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(sum_domain.Name1);
                    domain.Name1 = (ClientUtil.ToDecimal(domain.Name1) + ClientUtil.ToDecimal(sum_domain.Name1)) + "";
                    if (ht_ownerQty.Contains(project.Id))
                    {
                        OwnerQuantityMaster ownerMaster = (OwnerQuantityMaster)ht_ownerQty[project.Id];
                        fGrid_IncomeCostMx.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(ownerMaster.ConfirmSumMoney);
                        fGrid_IncomeCostMx.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(ownerMaster.CollectionSumMoney);
                        domain.Name2 = (ClientUtil.ToDecimal(domain.Name2) + ClientUtil.ToDecimal(ownerMaster.ConfirmSumMoney)) + "";
                        domain.Name3 = (ClientUtil.ToDecimal(domain.Name3) + ClientUtil.ToDecimal(ownerMaster.CollectionSumMoney)) + "";
                        fGrid_IncomeCostMx.Cell(dtlStartRowNum + i, 6).Text = (ClientUtil.ToDecimal(ownerMaster.ConfirmSumMoney) - ClientUtil.ToDecimal(ownerMaster.CollectionSumMoney)) + "";
                        if (ClientUtil.ToDecimal(ownerMaster.ConfirmSumMoney) != 0)
                        {
                            decimal tValue = decimal.Round((ClientUtil.ToDecimal(ownerMaster.ConfirmSumMoney) - ClientUtil.ToDecimal(ownerMaster.CollectionSumMoney)) * 100 / ClientUtil.ToDecimal(ownerMaster.ConfirmSumMoney), 2);
                            fGrid_IncomeCostMx.Cell(dtlStartRowNum + i, 7).Text = tValue + "";
                        }
                        if (ClientUtil.ToDecimal(sum_domain.Name1) != 0)
                        {
                            decimal tValue = decimal.Round((ClientUtil.ToDecimal(ownerMaster.ConfirmSumMoney) - ClientUtil.ToDecimal(ownerMaster.CollectionSumMoney)) * 100 / ClientUtil.ToDecimal(sum_domain.Name1), 2);
                            fGrid_IncomeCostMx.Cell(dtlStartRowNum + i, 8).Text = tValue + "";
                        }
                    }
                }
                if (project.OwnerOrgName == "尾")
                {
                    domain.Name30 = project.Name;
                    fGrid_IncomeCostMx.Cell(dtlStartRowNum + i, 2).Text = project.Name + "小计";
                    fGrid_IncomeCostMx.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                    fGrid_IncomeCostMx.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(domain.Name1);
                    fGrid_IncomeCostMx.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(domain.Name2);
                    fGrid_IncomeCostMx.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(domain.Name3);
                    fGrid_IncomeCostMx.Cell(dtlStartRowNum + i, 6).Text = (ClientUtil.ToDecimal(domain.Name2) - ClientUtil.ToDecimal(domain.Name3)) + "";
                    if (ClientUtil.ToDecimal(domain.Name2) != 0)
                    {
                        decimal tValue = decimal.Round((ClientUtil.ToDecimal(domain.Name2) - ClientUtil.ToDecimal(domain.Name3)) * 100 / ClientUtil.ToDecimal(domain.Name2), 2);
                        fGrid_IncomeCostMx.Cell(dtlStartRowNum + i, 7).Text = tValue + "";
                    }
                    if (ClientUtil.ToDecimal(domain.Name1) != 0)
                    {
                        decimal tValue = decimal.Round((ClientUtil.ToDecimal(domain.Name2) - ClientUtil.ToDecimal(domain.Name3)) * 100 / ClientUtil.ToDecimal(domain.Name1), 2);
                        fGrid_IncomeCostMx.Cell(dtlStartRowNum + i, 8).Text = tValue + "";
                    }
                    list_main.Add(domain);
                    domain = new DataDomain();
                }
            }
            #endregion

            #region 收入成本变化情况综合表
            dtlStartRowNum = 5;//模板中的行号
            dtlCount = list_main.Count;

            //插入明细行
            this.fGrid_IncomeCost.InsertRow(dtlStartRowNum, dtlCount + 2);
            //设置单元格的边框，对齐方式
            range = fGrid_IncomeCost.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount + 1, fGrid_IncomeCost.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);
            DataDomain sum_income_domain = new DataDomain();
            for (int i = 0; i < dtlCount; i++)
            {
                DataDomain m_domain = (DataDomain)list_main[i];

                fGrid_IncomeCost.Cell(dtlStartRowNum + i, 1).Text = (i + 1) + "";
                fGrid_IncomeCost.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGrid_IncomeCost.Cell(dtlStartRowNum + i, 2).Text = ClientUtil.ToString(m_domain.Name30);
                fGrid_IncomeCost.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGrid_IncomeCost.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(m_domain.Name1);
                fGrid_IncomeCost.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(m_domain.Name2);
                fGrid_IncomeCost.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(m_domain.Name3);
                sum_income_domain.Name1 = (ClientUtil.ToDecimal(sum_income_domain.Name1) + ClientUtil.ToDecimal(m_domain.Name1)) + "";
                sum_income_domain.Name2 = (ClientUtil.ToDecimal(sum_income_domain.Name2) + ClientUtil.ToDecimal(m_domain.Name2)) + "";
                sum_income_domain.Name3 = (ClientUtil.ToDecimal(sum_income_domain.Name3) + ClientUtil.ToDecimal(m_domain.Name3)) + "";
                fGrid_IncomeCost.Cell(dtlStartRowNum + i, 6).Text = (ClientUtil.ToDecimal(m_domain.Name2) - ClientUtil.ToDecimal(m_domain.Name3)) + "";
                if (ClientUtil.ToDecimal(m_domain.Name2) != 0)
                {
                    decimal tValue = decimal.Round((ClientUtil.ToDecimal(m_domain.Name2) - ClientUtil.ToDecimal(m_domain.Name3)) * 100 / ClientUtil.ToDecimal(m_domain.Name2), 2);
                    fGrid_IncomeCost.Cell(dtlStartRowNum + i, 7).Text = tValue + "";
                }
                if (ClientUtil.ToDecimal(m_domain.Name1) != 0)
                {
                    decimal tValue = decimal.Round((ClientUtil.ToDecimal(m_domain.Name2) - ClientUtil.ToDecimal(m_domain.Name3)) * 100 / ClientUtil.ToDecimal(m_domain.Name1), 2);
                    fGrid_IncomeCost.Cell(dtlStartRowNum + i, 8).Text = tValue + "";
                }
            }
            //插入综合值
            fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount, 1).Text = (dtlCount + 1) + "";
            fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
            fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount, 2).Text = "合计";
            fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
            fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount, 3).Text = ClientUtil.ToString(sum_income_domain.Name1);
            fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount, 4).Text = ClientUtil.ToString(sum_income_domain.Name2);
            fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount, 5).Text = ClientUtil.ToString(sum_income_domain.Name3);
            fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount, 6).Text = (ClientUtil.ToDecimal(sum_income_domain.Name2) - ClientUtil.ToDecimal(sum_income_domain.Name3)) + "";
            if (ClientUtil.ToDecimal(sum_income_domain.Name2) != 0)
            {
                decimal tValue = decimal.Round((ClientUtil.ToDecimal(sum_income_domain.Name2) - ClientUtil.ToDecimal(sum_income_domain.Name3)) * 100 / ClientUtil.ToDecimal(sum_income_domain.Name2), 2);
                fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount, 7).Text = tValue + "";
            }
            if (ClientUtil.ToDecimal(sum_income_domain.Name1) != 0)
            {
                decimal tValue = decimal.Round((ClientUtil.ToDecimal(sum_income_domain.Name2) - ClientUtil.ToDecimal(sum_income_domain.Name3)) * 100 / ClientUtil.ToDecimal(sum_income_domain.Name1), 2);
                fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount, 8).Text = tValue + "";
            }

            //插入平均值
            fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount + 1, 1).Text = (dtlCount + 2) + "";
            fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount + 1, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
            fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount + 1, 2).Text = "平均";
            fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount + 1, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
            if (dtlCount > 0)
            {
                fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount + 1, 3).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount, 3).Text) / dtlCount, 2) + "";
                fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount + 1, 4).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount, 4).Text) / dtlCount, 2) + "";
                fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount + 1, 5).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount, 5).Text) / dtlCount, 2) + "";
                fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount + 1, 6).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount, 6).Text) / dtlCount, 2) + "";
                if (ClientUtil.ToDecimal(fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount + 1, 4).Text) != 0)
                {
                    fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount + 1, 7).Text =
                        decimal.Round(ClientUtil.ToDecimal(fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount + 1, 6).Text) * 100 / ClientUtil.ToDecimal(fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount + 1, 4).Text), 2) + "";
                }
                if (ClientUtil.ToDecimal(fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount + 1, 3).Text) != 0)
                {
                    fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount + 1, 8).Text =
                        decimal.Round(ClientUtil.ToDecimal(fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount + 1, 6).Text) * 100 / ClientUtil.ToDecimal(fGrid_IncomeCost.Cell(dtlStartRowNum + dtlCount + 1, 3).Text), 2) + "";
                }
            }
            #endregion
        }
        #endregion

        #region 财务收款情况一览表/综合表
        private void LoadReceiveMoneyMxData()
        {
            IList list_main = new ArrayList();

            #region   财务收款情况一览表
            int dtlStartRowNum = 6;//模板中的行号
            int dtlCount = this.resultTransList.Count;

            //插入明细行
            this.fGrid_ReceiveMoneyMx.InsertRow(dtlStartRowNum, dtlCount);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGrid_ReceiveMoneyMx.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGrid_ReceiveMoneyMx.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            //写入明细数据
            DataDomain domain = new DataDomain();
            for (int i = 0; i < dtlCount; i++)
            {
                CurrentProjectInfo project = (CurrentProjectInfo)resultTransList[i];

                fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 1).Text = project.Data2;
                fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 2).Text = project.Name;
                fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                if (project.Id != null && ht_sumData.Contains(project.Id))
                {
                    DataDomain sum_domain = (DataDomain)ht_sumData[project.Id];
                    fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(sum_domain.Name1);
                    fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(sum_domain.Name2);
                    domain.Name1 = (ClientUtil.ToDecimal(domain.Name1) + ClientUtil.ToDecimal(sum_domain.Name1)) + "";
                    domain.Name2 = (ClientUtil.ToDecimal(domain.Name2) + ClientUtil.ToDecimal(sum_domain.Name2)) + "";

                    if (ht_ownerQty.Contains(project.Id))
                    {
                        OwnerQuantityMaster ownerMaster = (OwnerQuantityMaster)ht_ownerQty[project.Id];
                        fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(ownerMaster.ConfirmSumMoney);
                        fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(ownerMaster.CollectionSumMoney);
                        domain.Name3 = (ClientUtil.ToDecimal(domain.Name3) + ClientUtil.ToDecimal(ownerMaster.ConfirmSumMoney)) + "";
                        domain.Name5 = (ClientUtil.ToDecimal(domain.Name5) + ClientUtil.ToDecimal(ownerMaster.CollectionSumMoney)) + "";
                        if (ht_project.Contains(project.Id))
                        {
                            CurrentProjectInfo currProject = (CurrentProjectInfo)ht_project[project.Id];
                            fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToDecimal(ownerMaster.ConfirmSumMoney) * currProject.ContractCollectRatio + "";
                            domain.Name4 = (ClientUtil.ToDecimal(domain.Name4) + ClientUtil.ToDecimal(fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 6).Text)) + "";
                        }
                    }
                    if (ClientUtil.ToDecimal(fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 6).Text) != 0)
                    {
                        fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 8).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 7).Text) * 100 /
                                            ClientUtil.ToDecimal(fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 6).Text), 2) + "";
                        if (ClientUtil.ToDecimal(fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 8).Text) <= 95)
                        {
                            fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 8).BackColor = System.Drawing.Color.Red;
                        }
                    }
                    fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 9).Text = (ClientUtil.ToDecimal(fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 7).Text) -
                                           ClientUtil.ToDecimal(fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 4).Text)) + "";
                    fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 10).Text = (ClientUtil.ToDecimal(fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 3).Text) -
                                           ClientUtil.ToDecimal(fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 5).Text)) + "";
                    if (ClientUtil.ToDecimal(fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 10).Text) < 0)
                    {
                        fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 10).BackColor = System.Drawing.Color.Red;
                    }
                }
                if (project.OwnerOrgName == "尾")
                {
                    domain.Name30 = project.Name;
                    fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 2).Text = project.Name + "小计";
                    fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                    fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(domain.Name1);
                    fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(domain.Name2);
                    fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(domain.Name3);
                    fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(domain.Name4);
                    fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(domain.Name5);
                    if (ClientUtil.ToDecimal(fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 6).Text) != 0)
                    {
                        fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 8).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 7).Text) * 100 /
                                            ClientUtil.ToDecimal(fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 6).Text), 2) + "";
                        if (ClientUtil.ToDecimal(fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 8).Text) <= 95)
                        {
                            fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 8).BackColor = System.Drawing.Color.Red;
                        }
                    }
                    fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 9).Text = (ClientUtil.ToDecimal(fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 7).Text) -
                                           ClientUtil.ToDecimal(fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 4).Text)) + "";
                    fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 10).Text = (ClientUtil.ToDecimal(fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 3).Text) -
                                           ClientUtil.ToDecimal(fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 5).Text)) + "";
                    if (ClientUtil.ToDecimal(fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 10).Text) < 0)
                    {
                        fGrid_ReceiveMoneyMx.Cell(dtlStartRowNum + i, 10).BackColor = System.Drawing.Color.Red;
                    }

                    list_main.Add(domain);
                    domain = new DataDomain();
                }
            }
            #endregion

            #region 财务收款情况综合表
            dtlStartRowNum = 6;//模板中的行号
            dtlCount = list_main.Count;

            //插入明细行
            this.fGrid_ReceiveMoney.InsertRow(dtlStartRowNum, dtlCount + 2);
            //设置单元格的边框，对齐方式
            range = fGrid_ReceiveMoney.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount + 1, fGrid_ReceiveMoney.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);
            DataDomain sum_income_domain = new DataDomain();
            for (int i = 0; i < dtlCount; i++)
            {
                DataDomain m_domain = (DataDomain)list_main[i];

                fGrid_ReceiveMoney.Cell(dtlStartRowNum + i, 1).Text = (i + 1) + "";
                fGrid_ReceiveMoney.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGrid_ReceiveMoney.Cell(dtlStartRowNum + i, 2).Text = ClientUtil.ToString(m_domain.Name30);
                fGrid_ReceiveMoney.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGrid_ReceiveMoney.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(m_domain.Name1);
                fGrid_ReceiveMoney.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(m_domain.Name2);
                fGrid_ReceiveMoney.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(m_domain.Name3);
                fGrid_ReceiveMoney.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(m_domain.Name4);
                fGrid_ReceiveMoney.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(m_domain.Name5);
                sum_income_domain.Name1 = (ClientUtil.ToDecimal(sum_income_domain.Name1) + ClientUtil.ToDecimal(m_domain.Name1)) + "";
                sum_income_domain.Name2 = (ClientUtil.ToDecimal(sum_income_domain.Name2) + ClientUtil.ToDecimal(m_domain.Name2)) + "";
                sum_income_domain.Name3 = (ClientUtil.ToDecimal(sum_income_domain.Name3) + ClientUtil.ToDecimal(m_domain.Name3)) + "";
                sum_income_domain.Name4 = (ClientUtil.ToDecimal(sum_income_domain.Name4) + ClientUtil.ToDecimal(m_domain.Name4)) + "";
                sum_income_domain.Name5 = (ClientUtil.ToDecimal(sum_income_domain.Name5) + ClientUtil.ToDecimal(m_domain.Name5)) + "";
                if (ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + i, 6).Text) != 0)
                {
                    fGrid_ReceiveMoney.Cell(dtlStartRowNum + i, 8).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + i, 7).Text) * 100 /
                                        ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + i, 6).Text), 2) + "";
                    if (ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + i, 8).Text) <= 95)
                    {
                        fGrid_ReceiveMoney.Cell(dtlStartRowNum + i, 8).BackColor = System.Drawing.Color.Red;
                    }
                }
                fGrid_ReceiveMoney.Cell(dtlStartRowNum + i, 9).Text = (ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + i, 7).Text) -
                                       ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + i, 4).Text)) + "";
                fGrid_ReceiveMoney.Cell(dtlStartRowNum + i, 10).Text = (ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + i, 3).Text) -
                                       ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + i, 5).Text)) + "";
                if (ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + i, 10).Text) < 0)
                {
                    fGrid_ReceiveMoney.Cell(dtlStartRowNum + i, 10).BackColor = System.Drawing.Color.Red;
                }

            }
            //插入综合值
            fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 1).Text = (dtlCount + 1) + "";
            fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
            fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 2).Text = "合计";
            fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
            fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 3).Text = ClientUtil.ToString(sum_income_domain.Name1);
            fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 4).Text = ClientUtil.ToString(sum_income_domain.Name2);
            fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 5).Text = ClientUtil.ToString(sum_income_domain.Name3);
            fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 6).Text = ClientUtil.ToString(sum_income_domain.Name4);
            fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 7).Text = ClientUtil.ToString(sum_income_domain.Name5);
            if (ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 6).Text) != 0)
            {
                fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 8).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 7).Text) * 100 /
                                    ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 6).Text), 2) + "";
                if (ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 8).Text) <= 95)
                {
                    fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 8).BackColor = System.Drawing.Color.Red;
                }
            }
            fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 9).Text = (ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 7).Text) -
                                   ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 4).Text)) + "";
            fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 10).Text = (ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 3).Text) -
                                   ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 5).Text)) + "";
            if (ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 10).Text) < 0)
            {
                fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 10).BackColor = System.Drawing.Color.Red;
            }

            //插入平均值
            fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount + 1, 1).Text = (dtlCount + 2) + "";
            fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount + 1, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
            fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount + 1, 2).Text = "平均";
            fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount + 1, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
            if (dtlCount > 0)
            {
                fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount + 1, 3).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 3).Text) / dtlCount, 2) + "";
                fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount + 1, 4).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 4).Text) / dtlCount, 2) + "";
                fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount + 1, 5).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 5).Text) / dtlCount, 2) + "";
                fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount + 1, 6).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 6).Text) / dtlCount, 2) + "";
                fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount + 1, 7).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount, 7).Text) / dtlCount, 2) + "";
                if (ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount + 1, 6).Text) != 0)
                {
                    fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount + 1, 8).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount + 1, 7).Text) * 100 /
                                        ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount + 1, 6).Text), 2) + "";
                    if (ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount + 1, 8).Text) <= 95)
                    {
                        fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount + 1, 8).BackColor = System.Drawing.Color.Red;
                    }
                }
                fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount + 1, 9).Text = (ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount + 1, 7).Text) -
                                       ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount + 1, 4).Text)) + "";
                fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount + 1, 10).Text = (ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount + 1, 3).Text) -
                                       ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount + 1, 5).Text)) + "";
                if (ClientUtil.ToDecimal(fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount + 1, 10).Text) < 0)
                {
                    fGrid_ReceiveMoney.Cell(dtlStartRowNum + dtlCount + 1, 10).BackColor = System.Drawing.Color.Red;
                }
            }
            #endregion
        }
        #endregion

        #region 辅助算法
        private IList GetValidReportList()
        {
            IList list_valid = new ArrayList();
            Hashtable ht_sumData = (Hashtable)ht_resultdata["4"];
            foreach (CurrentProjectInfo project in resultList)
            {
                if (ht_sumData.Contains(project.Id))
                {
                    list_valid.Add(project);
                }
            }
            return list_valid;
        }

        /// <summary>
        /// 根据显示类型返回项目集合
        /// </summary>
        /// <param name="displayType">1:显示头和小计</param>
        private IList GetReportList(int displayType)
        {
            IList reportList = new ArrayList();
            string currPOrgName = "";
            CurrentProjectInfo preProject = new CurrentProjectInfo();
            int tt = 1;
            int level1 = 1;
            int level2 = 1;
            IList list_valid = this.GetValidReportList();
            foreach (CurrentProjectInfo project in list_valid)
            {
                string pOrgName = project.OwnerOrgName;
                CurrentProjectInfo currProject = new CurrentProjectInfo();
                
                if (currPOrgName != pOrgName)
                {
                    //1.加入上个项目类型的小计尾
                    if (currPOrgName != "")
                    {
                        currProject = new CurrentProjectInfo();
                        currProject.Name = currPOrgName;
                        currProject.OwnerOrgName = "尾";
                        reportList.Add(currProject);
                    }
                    //2.加入小计头
                    currProject = new CurrentProjectInfo();
                    currProject.Name = pOrgName;
                    currProject.OwnerOrgName = "头";
                    currProject.Data2 = CommonUtil.GetChineseNumber(level1);
                    reportList.Add(currProject);
                    currPOrgName = pOrgName;
                    level1++;
                    level2 = 1;
                }

                //3.加入本身项目信息
                currProject = new CurrentProjectInfo();
                currProject.Name = project.Name;
                currProject.OwnerOrgName = project.OwnerOrgName;
                currProject.Id = project.Id;
                currProject.ResProportion = project.ResProportion;
                currProject.Data1 = project.Data1;
                currProject.Data2 = "  " + level2;
                reportList.Add(currProject);

                //4.如果为最后一行,加入小计
                if (tt == list_valid.Count)
                {
                    currProject = new CurrentProjectInfo();
                    currProject.Name = currPOrgName;
                    currProject.OwnerOrgName = "尾";
                    reportList.Add(currProject);
                }
                currPOrgName = pOrgName;
                preProject = project;
                level2++;
                tt++;
            }
            return reportList;
        }
        #endregion
    }
}
