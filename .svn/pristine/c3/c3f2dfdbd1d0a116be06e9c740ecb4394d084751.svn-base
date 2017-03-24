using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Util;
using System.IO;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Microsoft.Office.Interop.Excel;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostBusinessAccount
{
    public partial class VCostBusinessReport : TBasicDataView
    {
        MCostMonthAccount model = new MCostMonthAccount();
        MStockMng stockModel = new MStockMng();
        IList list_subProject = new ArrayList();//分包合同
        IList list_ledger = new ArrayList();//签证变更
        IList list_cost = new ArrayList();//成本状况
        IList list_consume = new ArrayList();//成本消耗状况

        #region 基本数据
        private string loginPersonName = ConstObject.LoginPersonInfo.Name;
        private string loginDate = ConstObject.LoginDate.ToShortDateString();
        private CurrentProjectInfo projectInfo;
        string mainStr = "商务报表统计封面";
        string needStr = "填报要求";
        string costStr = "项目成本情况统计表";
        string consumeStr = "项目各自消耗指标情况";
        string incomeDiffStr = "项目收入成本变化统计分析表";
        string inBalStr = "内部结算情况";
        #endregion

        public VCostBusinessReport()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }

        private void InitData()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            this.cmbProject.Enabled = false;
            cmbProject.Text = projectInfo.Name;
            IList list = stockModel.StockInSrv.GetFiscalYear();
            this.cmbYear.Items.Clear();
            foreach (int iYear in list)
            {
                this.cmbYear.Items.Insert(this.cmbYear.Items.Count, iYear);
                if (iYear == ConstObject.TheLogin.TheComponentPeriod.NowYear)
                {
                    this.cmbYear.SelectedItem = this.cmbYear.Items[this.cmbYear.Items.Count - 1];
                }
            }
            
            for (int i = 1; i < 13; i++)
            {
                this.cboFiscalMonth.Items.Add(i);
            }
            this.cboFiscalMonth.Text = ConstObject.TheLogin.TheComponentPeriod.NowMonth.ToString();

            this.fGrid_Main.Rows = 1;
            this.fGrid_Require.Rows = 1;
            this.fGrid_CostStat.Rows = 1;
            this.fGrid_Consume.Rows = 1;
            this.fGrid_IncomeDiff.Rows = 1;
            this.fGrid_InBal.Rows = 1;
        }

        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnSelectGWBS.Click += new EventHandler(btSelectGWBS_Click);
        }

        void btSelectGWBS_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            if (txtAccountRootNode.Tag != null)
            {
                frm.DefaultSelectedGWBS = txtAccountRootNode.Tag as GWBSTree;
            }
            frm.IsCheck = true;
            frm.IsRootNode = true;//这个参数是否只选择隶属关系的根节点
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                IList taskList = new ArrayList();
                string taskStr = "";
                foreach (TreeNode node in frm.SelectResult)
                {
                    GWBSTree task = node.Tag as GWBSTree;
                    taskList.Add(task);
                    taskStr += task.Name + "_";
                }
                TreeNode root = frm.SelectResult[0];

                txtAccountRootNode.Text = taskStr;
                txtAccountRootNode.Tag = taskList;
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            string fileName = this.fGrid_Main.ExportToExcel(mainStr, false, false, true);
            if (fileName == "")
                return;
            FlashScreen.Show("正在导出公司商务报表...");
            ApplicationClass excel = new ApplicationClass();

            //主文件对象
            Workbook workbook = excel.Workbooks.Open(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet = workbook.Sheets[1] as Worksheet;
            mySheet.Name = mainStr;

            int startIndex = fileName.LastIndexOf("\\") + 1;
            int endIndex = fileName.IndexOf(".x");
            mainStr = fileName.Substring(startIndex, endIndex - startIndex);

            string tempName = fileName.Replace(mainStr, needStr);
            this.fGrid_Require.ExportToExcel(tempName, false, false, false);
            Workbook workbook1 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet1 = workbook1.Sheets[1] as Worksheet;
            mySheet1.Name = needStr;

            tempName = fileName.Replace(mainStr, costStr);
            this.fGrid_CostStat.ExportToExcel(tempName, false, false, false);
            Workbook workbook2 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet2 = workbook2.Sheets[1] as Worksheet;
            mySheet2.Name = costStr;

            tempName = fileName.Replace(mainStr, consumeStr);
            this.fGrid_Consume.ExportToExcel(tempName, false, false, false);
            Workbook workbook3 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet3 = workbook3.Sheets[1] as Worksheet;
            mySheet3.Name = consumeStr;

            tempName = fileName.Replace(mainStr, incomeDiffStr);
            this.fGrid_IncomeDiff.ExportToExcel(tempName, false, false, false);
            Workbook workbook4 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet4 = workbook4.Sheets[1] as Worksheet;
            mySheet4.Name = incomeDiffStr;

            tempName = fileName.Replace(mainStr, inBalStr);
            this.fGrid_InBal.ExportToExcel(tempName, false, false, false);
            Workbook workbook5 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet5 = workbook5.Sheets[1] as Worksheet;
            mySheet5.Name = inBalStr;


            try
            {
                mySheet5.Copy(Type.Missing, mySheet);
                mySheet4.Copy(Type.Missing, mySheet);
                mySheet3.Copy(Type.Missing, mySheet);
                mySheet2.Copy(Type.Missing, mySheet);
                mySheet1.Copy(Type.Missing, mySheet);

                workbook.Save();

            }
            catch (Exception e1)
            {
                FlashScreen.Close();
                throw new Exception("导出公司商务报表出错！");
            }
            finally
            {
                FlashScreen.Close();
                //关闭工作表和退出Excel
                workbook.Close(false, Type.Missing, Type.Missing);
                workbook1.Close(false, Type.Missing, Type.Missing);
                workbook2.Close(false, Type.Missing, Type.Missing);
                workbook3.Close(false, Type.Missing, Type.Missing);
                workbook4.Close(false, Type.Missing, Type.Missing);
                workbook5.Close(false, Type.Missing, Type.Missing);
                //如果报表文件存在，先删除
                if (File.Exists(fileName.Replace(mainStr, needStr)))
                {
                    File.Delete(fileName.Replace(mainStr, needStr));
                }
                if (File.Exists(fileName.Replace(mainStr, costStr)))
                {
                    File.Delete(fileName.Replace(mainStr, costStr));
                }
                if (File.Exists(fileName.Replace(mainStr, consumeStr)))
                {
                    File.Delete(fileName.Replace(mainStr, consumeStr));
                }
                if (File.Exists(fileName.Replace(mainStr, incomeDiffStr)))
                {
                    File.Delete(fileName.Replace(mainStr, incomeDiffStr));
                }
                if (File.Exists(fileName.Replace(mainStr, inBalStr)))
                {
                    File.Delete(fileName.Replace(mainStr, inBalStr));
                }
                excel.Quit();
                excel = null;
            }
            MessageBox.Show("导出公司商务报表成功！");
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            if (ClientUtil.ToString(this.cmbYear.Text) == "")
            {
                MessageBox.Show("请输入会计年！");
                return;
            }
            if (ClientUtil.ToString(this.cboFiscalMonth.Text) == "")
            {
                MessageBox.Show("请输入会计月！");
                return;
            }
            LoadTempleteFile("商务报表统计封面.flx");
            LoadTempleteFile(costStr + ".flx");
            LoadTempleteFile(consumeStr + ".flx");
            LoadTempleteFile(incomeDiffStr + ".flx");
            LoadTempleteFile(inBalStr + ".flx");
            LoadTempleteFile(needStr + ".flx");

            list_cost.Clear();
            list_consume.Clear();

            //载入数据
            IList nodeList = txtAccountRootNode.Tag as ArrayList;
            IList taskIdList = new ArrayList();
            
            GWBSTree fGWBS = new GWBSTree();
            if (nodeList != null)
            {
                foreach (GWBSTree gwbs in nodeList)
                {
                    taskIdList.Add(gwbs.Id);
                    fGWBS = gwbs;
                }
            }
            OBSManage mOBS = model.CostMonthAccSrv.GetOBSManageByTaskNode(fGWBS.SysCode, projectInfo.Id);
            string accOrgId = "";
            if (mOBS.OrpJob != null)
            {
                accOrgId = mOBS.OrpJob.Id;
            }

            Hashtable ht_resultdata = model.CostMonthAccSrv.GetBusinessReportData(ClientUtil.ToInt(this.cmbYear.Text), ClientUtil.ToInt(this.cboFiscalMonth.Text), projectInfo.Id, accOrgId, taskIdList);
            list_cost.Add((DataDomain)ht_resultdata["1"]);
            list_consume.Add((DataDomain)ht_resultdata["2"]);
            list_ledger = (ArrayList)ht_resultdata["3"];
            list_subProject = (ArrayList)ht_resultdata["4"];


            //载入数据
            this.LoadMainData();
            this.LoadCostStatData();
            this.LoadConsumeData();
            this.LoadIncomeDiffData();
            this.LoadInBalData();

            //设置外观
            CommonUtil.SetFlexGridFace(this.fGrid_Main);
            CommonUtil.SetFlexGridFace(this.fGrid_Require);
            CommonUtil.SetFlexGridFace(this.fGrid_CostStat);
            CommonUtil.SetFlexGridFace(this.fGrid_Consume);
            CommonUtil.SetFlexGridFace(this.fGrid_IncomeDiff);
            //CommonUtil.SetFlexGridFace(this.fGrid_InBal);

        }

        private void LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式
                if (modelName == "商务报表统计封面.flx")
                {
                    fGrid_Main.OpenFile(path + "\\" + modelName);//载入格式
                }
                else if (modelName == "填报要求.flx")
                {
                    fGrid_Require.OpenFile(path + "\\" + modelName);//载入格式
                }
                else if (modelName == "项目成本情况统计表.flx")
                {
                    this.fGrid_CostStat.OpenFile(path + "\\" + modelName);//载入格式
                }
                else if (modelName == "项目各自消耗指标情况.flx")
                {
                    this.fGrid_Consume.OpenFile(path + "\\" + modelName);//载入格式
                }
                else if (modelName == "项目收入成本变化统计分析表.flx")
                {
                    this.fGrid_IncomeDiff.OpenFile(path + "\\" + modelName);//载入格式
                }
                else if (modelName == "内部结算情况.flx")
                {
                    this.fGrid_InBal.OpenFile(path + "\\" + modelName);//载入格式
                }
               
            }
            else
            {
                MessageBox.Show("未找到模板格式文件" + modelName);
                return;
            }
        }

        #region 封面数据
        private void LoadMainData()
        {
            string reportDateStr = this.cmbYear.Text + "年" + this.cboFiscalMonth.Text + "月";
          this.fGrid_Main.Cell(1, 1).Text = reportDateStr;
          this.fGrid_Main.Cell(5, 4).Text = projectInfo.Name;
          this.fGrid_Main.Cell(7, 4).Text = ConstObject.TheLogin.ThePerson.Name;
          this.fGrid_Main.Cell(11, 4).Text = ConstObject.TheLogin.LoginDate.ToShortDateString();
        }
        #endregion

        #region 表一 项目成本情况统计表
        private void LoadCostStatData()
        {
            int dtlStartRowNum = 6;//模板中的行号
            int dtlCount = list_cost.Count;

            //插入明细行
            this.fGrid_CostStat.InsertRow(dtlStartRowNum, dtlCount);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGrid_CostStat.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGrid_CostStat.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            //写入明细数据
            for (int i = 0; i < dtlCount; i++)
            {
                DataDomain cost_domain = (DataDomain)list_cost[i];
                this.fGrid_CostStat.Cell(dtlStartRowNum + i, 1).Text = "1";
                this.fGrid_CostStat.Cell(dtlStartRowNum + i, 2).Text = projectInfo.Name;
                this.fGrid_CostStat.Cell(dtlStartRowNum + i, 3).Text = projectInfo.BuildingArea + "";
                this.fGrid_CostStat.Cell(dtlStartRowNum + i, 4).Text = projectInfo.CivilContractMoney + "";
                this.fGrid_CostStat.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.TransToDecimal(cost_domain.Name1) / 10000 + "";
                this.fGrid_CostStat.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.TransToDecimal(cost_domain.Name2) / 10000 + "";
                this.fGrid_CostStat.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.TransToDecimal(cost_domain.Name3) / 10000 + "";
                this.fGrid_CostStat.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.TransToDecimal(cost_domain.Name4) + "";
                this.fGrid_CostStat.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.TransToDecimal(cost_domain.Name5) / 10000 + "";
                this.fGrid_CostStat.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.TransToDecimal(cost_domain.Name6) / 10000 + "";
                this.fGrid_CostStat.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.TransToDecimal(cost_domain.Name7) / 10000 + "";
                this.fGrid_CostStat.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.TransToDecimal(cost_domain.Name8) / 10000 + "";
                this.fGrid_CostStat.Cell(dtlStartRowNum + i, 13).Text = ClientUtil.TransToDecimal(cost_domain.Name9) / 10000 + "";
                this.fGrid_CostStat.Cell(dtlStartRowNum + i, 14).Text = ClientUtil.TransToDecimal(cost_domain.Name10) / 10000 + "";
                this.fGrid_CostStat.Cell(dtlStartRowNum + i, 15).Text = ClientUtil.TransToDecimal(cost_domain.Name11) / 10000 + "";
                this.fGrid_CostStat.Cell(dtlStartRowNum + i, 16).Text = ClientUtil.TransToDecimal(cost_domain.Name12) / 10000 + "";
                this.fGrid_CostStat.Cell(dtlStartRowNum + i, 17).Text = ClientUtil.TransToDecimal(cost_domain.Name13) / 10000 + "";
                this.fGrid_CostStat.Cell(dtlStartRowNum + i, 18).Text = ClientUtil.TransToDecimal(cost_domain.Name14) / 10000 + "";
                this.fGrid_CostStat.Cell(dtlStartRowNum + i, 19).Text = ClientUtil.TransToDecimal(cost_domain.Name15) / 10000 + "";
                this.fGrid_CostStat.Cell(dtlStartRowNum + i, 20).Text = ClientUtil.TransToDecimal(cost_domain.Name16) / 10000 + "";
                this.fGrid_CostStat.Cell(dtlStartRowNum + i, 21).Text = ClientUtil.TransToDecimal(cost_domain.Name17) / 10000 + "";
                this.fGrid_CostStat.Cell(dtlStartRowNum + i, 22).Text = ClientUtil.TransToDecimal(cost_domain.Name18) / 10000 + "";
                this.fGrid_CostStat.Cell(dtlStartRowNum + i, 23).Text = ClientUtil.TransToDecimal(cost_domain.Name19) / 10000 + "";
            }
        }
        #endregion

        #region 表二 项目各项消耗指标情况
        private void LoadConsumeData()
        {
            int dtlStartRowNum = 7;//模板中的行号
            int dtlCount = list_consume.Count;

            //插入明细行
            this.fGrid_Consume.InsertRow(dtlStartRowNum, dtlCount);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGrid_Consume.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGrid_Consume.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            //写入明细数据
            for (int i = 0; i < dtlCount; i++)
            {
                DataDomain consume_domain = (DataDomain)list_consume[i];
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 1).Text = "1";
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 2).Text = projectInfo.Name;
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 3).Text = projectInfo.BuildingArea + "";
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.TransToDecimal(consume_domain.Name1) / 10000 + "";
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.TransToDecimal(consume_domain.Name2) / 10000 + "";
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(consume_domain.Name3);
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.TransToDecimal(consume_domain.Name4) / 10000 + "";
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.TransToDecimal(consume_domain.Name5) / 10000 + "";
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.TransToDecimal(consume_domain.Name6) / 10000 + "";
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(consume_domain.Name7);
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.ToString(consume_domain.Name8);
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(consume_domain.Name9);
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 13).Text = ClientUtil.ToString(consume_domain.Name10);
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 14).Text = ClientUtil.ToString(consume_domain.Name11);
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 15).Text = ClientUtil.ToString(consume_domain.Name12);
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 16).Text = ClientUtil.ToString(consume_domain.Name13);
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 17).Text = ClientUtil.ToString(consume_domain.Name14);
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 18).Text = ClientUtil.ToString(consume_domain.Name15);
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 19).Text = ClientUtil.ToString(consume_domain.Name16);
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 20).Text = ClientUtil.ToString(consume_domain.Name17);
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 21).Text = ClientUtil.TransToDecimal(consume_domain.Name18) / 10000 + "";
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 22).Text = ClientUtil.ToString(consume_domain.Name19);
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 23).Text = ClientUtil.ToString(consume_domain.Name20);
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 24).Text = ClientUtil.ToString(consume_domain.Name21);
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 25).Text = ClientUtil.TransToDecimal(consume_domain.Name22) / 10000 + "";
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 26).Text = ClientUtil.ToString(consume_domain.Name23);
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 27).Text = ClientUtil.ToString(consume_domain.Name24);
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 28).Text = ClientUtil.ToString(consume_domain.Name25);
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 29).Text = ClientUtil.ToString(consume_domain.Name26);
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 30).Text = ClientUtil.ToString(consume_domain.Name27);
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 31).Text = ClientUtil.ToString(consume_domain.Name28);
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 32).Text = ClientUtil.ToString(consume_domain.Name29);
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 33).Text = ClientUtil.ToString(consume_domain.Name30);
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 34).Text = ClientUtil.ToString(consume_domain.Name31);
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 35).Text = ClientUtil.TransToDecimal(consume_domain.Name32) / 10000 + "";
                this.fGrid_Consume.Cell(dtlStartRowNum + i, 36).Text = ClientUtil.TransToDecimal(consume_domain.Name33) / 10000 + "";
            }
        }
        #endregion

        #region 表三 项目收入成本变化统计分析表
        private void LoadIncomeDiffData()
        {
            int dtlStartRowNum = 5;//模板中的行号
            int dtlCount = list_ledger.Count;
            this.fGrid_IncomeDiff.AutoRedraw = false;

            //插入明细行
            this.fGrid_IncomeDiff.InsertRow(dtlStartRowNum, dtlCount + 1);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGrid_IncomeDiff.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGrid_IncomeDiff.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            this.fGrid_IncomeDiff.Cell(2, 2).Text = this.cmbProject.Text;

            int mergeCount = 1;
            string contractType = "";
            //写入明细数据
            for (int i = 0; i < dtlCount; i++)
            {
                GWBSDetailLedger ledger = (GWBSDetailLedger)list_ledger[i];

                if (contractType == "")
                {
                    contractType = ledger.Temp;
                }
                else
                {
                    if (contractType == ledger.Temp)
                    {
                        FlexCell.Range range_1 = fGrid_IncomeDiff.Range(dtlStartRowNum + i - 1, 1, dtlStartRowNum + i, 1);
                        range_1.Merge();
                        //fGrid_IncomeDiff.Cell(dtlStartRowNum + i, 1).Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 12.0F, FontStyle.Bold);
                        //fGrid_IncomeDiff.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterGeneral;
                        fGrid_IncomeDiff.Cell(dtlStartRowNum + i, 2).Text = "";
                    }
                    else {
                        mergeCount = 1;
                    }
                }
                fGrid_IncomeDiff.Cell(dtlStartRowNum + i, 1).Text = ledger.Temp;
                if (ledger.Temp.Contains("小计") || ledger.Temp.Contains("合计"))
                {
                    fGrid_IncomeDiff.Cell(dtlStartRowNum + i, 2).Text = "";
                }
                else
                {
                    fGrid_IncomeDiff.Cell(dtlStartRowNum + i, 2).Text = mergeCount + "";
                    fGrid_IncomeDiff.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                }
                fGrid_IncomeDiff.Cell(dtlStartRowNum + i, 3).Text = ledger.ProjectTaskName + "-" + ledger.ProjectTaskDtlName;
                fGrid_IncomeDiff.Cell(dtlStartRowNum + i, 3).Alignment = FlexCell.AlignmentEnum.LeftCenter;
                fGrid_IncomeDiff.Cell(dtlStartRowNum + i, 4).Text = ledger.ContractTotalPrice + "";
                fGrid_IncomeDiff.Cell(dtlStartRowNum + i, 5).Text = ledger.PlanTotalPrice + "";
                fGrid_IncomeDiff.Cell(dtlStartRowNum + i, 6).Text = (ledger.ContractTotalPrice - ledger.PlanTotalPrice) + "";
                if (ledger.ContractTotalPrice != 0)
                {
                    fGrid_IncomeDiff.Cell(dtlStartRowNum + i, 7).Text = decimal.Round((ledger.ContractTotalPrice - ledger.PlanTotalPrice) * 100 / ledger.ContractTotalPrice, 2) + "";
                }
                if (contractType != "")
                {
                    contractType = ledger.Temp;
                }
                mergeCount++;
            }
            this.fGrid_IncomeDiff.AutoRedraw = true;
        }
        #endregion

        #region 表四内部结算情况
        private void LoadInBalData()
        {
            int dtlStartRowNum = 5;//模板中的行号
            int dtlCount = list_subProject.Count;
            //this.fGrid_InBal.AutoRedraw = false;
            //插入明细行
            this.fGrid_InBal.InsertRow(dtlStartRowNum, dtlCount);
            //设置单元格的边框，对齐方式
            //FlexCell.Range range = fGrid_InBal.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGrid_InBal.Cols - 1);
            //CommonUtil.SetFlexGridDetailFormat(range);

            //写入明细数据
            for (int i = 0; i < dtlCount; i++)
            {
                SubContractProject subProject = (SubContractProject)list_subProject[i];
                fGrid_InBal.Cell(dtlStartRowNum + i, 1).Text = (i + 1) + "";
                fGrid_InBal.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGrid_InBal.Cell(dtlStartRowNum + i, 2).Text = subProject.BearerOrgName;
                fGrid_InBal.Cell(dtlStartRowNum + i, 3).Text = subProject.SubPackage;
                fGrid_InBal.Cell(dtlStartRowNum + i, 3).WrapText = true;
                fGrid_InBal.Cell(dtlStartRowNum + i, 4).Text = subProject.ContractInterimMoney + "";
                fGrid_InBal.Cell(dtlStartRowNum + i, 4).Alignment = FlexCell.AlignmentEnum.RightCenter;
                fGrid_InBal.Cell(dtlStartRowNum + i, 5).Text = subProject.AddupBalanceMoney + "";
                fGrid_InBal.Cell(dtlStartRowNum + i, 5).Alignment = FlexCell.AlignmentEnum.RightCenter;
                fGrid_InBal.Cell(dtlStartRowNum + i, 6).Text = subProject.BalanceStyle;
                fGrid_InBal.Cell(dtlStartRowNum + i, 6).Alignment = FlexCell.AlignmentEnum.CenterCenter;

                fGrid_InBal.Row(dtlStartRowNum + i).AutoFit();
            }
            fGrid_InBal.Column(2).AutoFit();
            fGrid_InBal.BackColor1 = System.Drawing.SystemColors.ButtonFace;
            fGrid_InBal.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            //this.fGrid_InBal.AutoRedraw = true;
        }
        #endregion

        #region  构造数据

        /// <summary>
        /// 写入合计行的数据
        /// </summary>
        /// <param name="flexGrid">二维表对象</param>
        /// <param name="startRow">计算范围的开始行</param>
        /// <param name="endRow">计算范围的结束行</param>
        /// <param name="startCol">计算范围的开始列</param>
        /// <param name="endCol">计算范围的结束列</param>
        private void WriteSumGridData(CustomFlexGrid flexGrid, int startRow, int endRow, int startCol, int endCol)
        {
            flexGrid.Cell(endRow + 1, startCol - 1).Text = "合计：";
            for (int i = startCol; i <= endCol; i++)
            {
                decimal sumValue = 0;
                for (int t = startRow; t <= endRow; t++)
                {
                    string ifCalSum = ClientUtil.ToString(flexGrid.Cell(t, 1).Tag);
                    if (ifCalSum == "1")
                    {
                        sumValue += ClientUtil.ToDecimal(flexGrid.Cell(t, i).Text);
                    }
                }
                flexGrid.Cell(endRow + 1, i).Text = sumValue + "";
            }
            FlexCell.Range range = flexGrid.Range(endRow + 1, 1, endRow + 1, endCol);
            CommonUtil.SetFlexGridDetailFormat(range);
            flexGrid.Cell(endRow + 1, startCol - 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
        }

        #endregion
    }
}