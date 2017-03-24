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

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostBusinessAccount
{
    public partial class VProduceReport : TBasicDataView
    {
        MCostMonthAccount model = new MCostMonthAccount();
        MStockMng stockModel = new MStockMng();
        IList list_season = new ArrayList();
        IList list_monthProduce = new ArrayList();
        IList list_task = new ArrayList();
        IList list_value = new ArrayList();
        IList list_building = new ArrayList();

        #region 基本数据
        private string loginPersonName = ConstObject.LoginPersonInfo.Name;
        private string loginDate = ConstObject.LoginDate.ToShortDateString();
        private CurrentProjectInfo projectInfo;
        string seasonStr = "季度生产计划表";
        string monProduceStr = "月度生产计划表";
        string taskStr = "施工任务完成情况表";
        string valueStr = "施工产值完成情况表";
        string buildingStr = "在建项目完成情况表";
        #endregion

        public VProduceReport()
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
            this.cboFiscalMonth.Text = ConstObject.TheLogin.TheComponentPeriod.NowMonth.ToString();

            this.cmbType.SelectedIndex = 0;
            this.RemoveTab(this.cmbType.SelectedIndex);

            this.fGrid_Season.Rows = 1;
            this.fGrid_MProduce.Rows = 1;
            this.fGrid_Task.Rows = 1;
            this.fGrid_Value.Rows = 1;
            this.fGrid_Building.Rows = 1;
        }

        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnSearchGWBS.Click += new EventHandler(btnSearchGWBS_Click);
            cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
        }
        //任务节点按钮事件
        void btnSearchGWBS_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                TreeNode root = frm.SelectResult[0];
                GWBSTree task = root.Tag as GWBSTree;
                if (task != null)
                {
                    this.txtGWBS.Text = task.Name;
                    this.txtGWBS.Tag = task;
                }
            }
        }
        void btnExcel_Click(object sender, EventArgs e)
        {
            string fileName = this.fGrid_Season.ExportToExcel(seasonStr, false, false, true);
            if (fileName == "")
                return;
            FlashScreen.Show("正在导出生产施工报表...");
            ApplicationClass excel = new ApplicationClass();

            //主文件对象
            Workbook workbook = excel.Workbooks.Open(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet = workbook.Sheets[1] as Worksheet;
            mySheet.Name = seasonStr;

            int startIndex = fileName.LastIndexOf("\\") + 1;
            int endIndex = fileName.IndexOf(".x");
            seasonStr = fileName.Substring(startIndex, endIndex - startIndex);

            string tempName = fileName.Replace(seasonStr, monProduceStr);
            this.fGrid_MProduce.ExportToExcel(tempName, false, false, false);
            Workbook workbook1 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet1 = workbook1.Sheets[1] as Worksheet;
            mySheet1.Name = monProduceStr;

            tempName = fileName.Replace(seasonStr, taskStr);
            this.fGrid_Task.ExportToExcel(tempName, false, false, false);
            Workbook workbook2 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet2 = workbook2.Sheets[1] as Worksheet;
            mySheet2.Name = taskStr;

            tempName = fileName.Replace(seasonStr, valueStr);
            this.fGrid_Value.ExportToExcel(tempName, false, false, false);
            Workbook workbook3 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet3 = workbook3.Sheets[1] as Worksheet;
            mySheet3.Name = valueStr;

            tempName = fileName.Replace(seasonStr, buildingStr);
            this.fGrid_Building.ExportToExcel(tempName, false, false, false);
            Workbook workbook4 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet4 = workbook4.Sheets[1] as Worksheet;
            mySheet4.Name = buildingStr;


            try
            {
                mySheet4.Copy(Type.Missing, mySheet);
                mySheet3.Copy(Type.Missing, mySheet);
                mySheet2.Copy(Type.Missing, mySheet);
                mySheet1.Copy(Type.Missing, mySheet);

                workbook.Save();

            }
            catch (Exception e1)
            {
                FlashScreen.Close();
                throw new Exception("导出生产施工报表出错！");
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
                //如果报表文件存在，先删除
                if (File.Exists(fileName.Replace(seasonStr, monProduceStr)))
                {
                    File.Delete(fileName.Replace(seasonStr, monProduceStr));
                }
                if (File.Exists(fileName.Replace(seasonStr, taskStr)))
                {
                    File.Delete(fileName.Replace(seasonStr, taskStr));
                }
                if (File.Exists(fileName.Replace(seasonStr, valueStr)))
                {
                    File.Delete(fileName.Replace(seasonStr, valueStr));
                }
                if (File.Exists(fileName.Replace(seasonStr, buildingStr)))
                {
                    File.Delete(fileName.Replace(seasonStr, buildingStr));
                }
                excel.Quit();
                excel = null;
            }
            MessageBox.Show("导出生产施工报表成功！");
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RemoveTab(this.cmbType.SelectedIndex);
            this.fGrid_Season.Rows = 1;
            this.fGrid_MProduce.Rows = 1;
            this.fGrid_Task.Rows = 1;
            this.fGrid_Value.Rows = 1;
            this.fGrid_Building.Rows = 1;
        }

        private void RemoveTab(int rType)
        {
            this.tabProduce.TabPages.Clear();
            this.cboFiscalMonth.Items.Clear();
            if (rType == 0)
            {
                this.tabProduce.TabPages.Remove(this.tabSeason);
                this.tabProduce.TabPages.Add(this.tabMonthProduce);
                this.tabProduce.TabPages.Add(this.tabTaskComplete);
                this.tabProduce.TabPages.Add(this.tabValueComplete);
                this.tabProduce.TabPages.Add(this.tabBuilding);

                this.lblMonth.Text = "月份";
                for (int t = 1; t <= 12; t++)
                {
                    this.cboFiscalMonth.Items.Add(t + "");
                }
                this.cboFiscalMonth.Text = ConstObject.TheLogin.TheComponentPeriod.NowMonth.ToString();
            }
            if (rType == 1)
            {
                this.tabProduce.TabPages.Add(this.tabSeason);
                this.tabProduce.TabPages.Remove(this.tabMonthProduce);
                this.tabProduce.TabPages.Remove(this.tabTaskComplete);
                this.tabProduce.TabPages.Remove(this.tabValueComplete);
                this.tabProduce.TabPages.Remove(this.tabBuilding);

                this.lblMonth.Text = "季度";
                for (int t = 1; t <= 4; t++)
                {
                    this.cboFiscalMonth.Items.Add(t + "");
                }
                this.cboFiscalMonth.Text = CommonUtil.GetCurrSeasonValue(ConstObject.TheLogin.TheComponentPeriod.NowMonth) + "";
            }

        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            if (ClientUtil.ToString(this.cmbYear.Text) == "")
            {
                MessageBox.Show("请输入会计年！");
                return;
            }
            // GWBS
            string taskSyscode = "";
            if (this.txtGWBS.Text != "")
            {
                taskSyscode = (txtGWBS.Tag as GWBSTree).SysCode;
            }
            LoadTempleteFile("季度生产计划表.flx");
            LoadTempleteFile(taskStr + ".flx");
            LoadTempleteFile(valueStr + ".flx");
            LoadTempleteFile(buildingStr + ".flx");
            LoadTempleteFile(monProduceStr + ".flx");

            list_season.Clear();
            list_monthProduce.Clear();
            list_task.Clear();
            list_value.Clear();
            list_building.Clear();

            Hashtable ht_resultdata = model.CostMonthAccSrv.GetProduceReportData(ClientUtil.ToInt(this.cmbYear.Text), ClientUtil.ToInt(this.cboFiscalMonth.Text), projectInfo.Id, taskSyscode);
            list_season = (ArrayList)ht_resultdata["1"];
            list_monthProduce = (ArrayList)ht_resultdata["2"];
            list_task = (ArrayList)ht_resultdata["3"];
            list_value = (ArrayList)ht_resultdata["4"];
            list_building = (ArrayList)ht_resultdata["5"];

            //载入数据
            this.LoadSeasonPlanData();
            this.LoadMonthPlanData();
            this.LoadTaskCompleteData();
            this.LoadValueCompleteData();
            this.LoadBuildingData();

            //设置外观
            CommonUtil.SetFlexGridFace(this.fGrid_Season);
            CommonUtil.SetFlexGridFace(this.fGrid_MProduce);
            CommonUtil.SetFlexGridFace(this.fGrid_Task);
            CommonUtil.SetFlexGridFace(this.fGrid_Value);
            CommonUtil.SetFlexGridFace(this.fGrid_Building);
        }

        private void LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式
                if (modelName == (seasonStr + ".flx"))
                {
                    fGrid_Season.OpenFile(path + "\\" + modelName);//载入格式
                }
                else if (modelName == (monProduceStr + ".flx"))
                {
                    fGrid_MProduce.OpenFile(path + "\\" + modelName);//载入格式
                }
                else if (modelName == (taskStr + ".flx"))
                {
                    this.fGrid_Task.OpenFile(path + "\\" + modelName);//载入格式
                }
                else if (modelName == (valueStr + ".flx"))
                {
                    this.fGrid_Value.OpenFile(path + "\\" + modelName);//载入格式
                }
                else if (modelName == (buildingStr + ".flx"))
                {
                    this.fGrid_Building.OpenFile(path + "\\" + modelName);//载入格式
                }           
            }
            else
            {
                MessageBox.Show("未找到模板格式文件" + modelName);
                return;
            }
        }

        #region 表一 季度生产计划表
        private void LoadSeasonPlanData()
        {
            this.fGrid_Season.Cell(1, 1).Text = this.cmbYear.Text + " 年 " + this.cboFiscalMonth.Text + " 季  度  施  工  生  产  计  划 ";
            this.fGrid_Season.Cell(4, 2).Text = projectInfo.Name;
            this.fGrid_Season.Cell(7, 2).Text = projectInfo.HandlePersonName;
            this.fGrid_Season.Cell(7, 5).Text = ConstObject.TheLogin.ThePerson.Name;
            this.fGrid_Season.Cell(7, 8).Text = ConstObject.TheLogin.LoginDate.ToShortDateString();

            int dtlStartRowNum = 6;//模板中的行号
            int dtlCount = list_season.Count;

            //插入明细行
            this.fGrid_Season.InsertRow(dtlStartRowNum, dtlCount);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGrid_Season.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGrid_Season.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            //写入明细数据
            for (int i = 0; i < dtlCount; i++)
            {
                DataDomain domain = (DataDomain)list_season[i];
                this.fGrid_Season.Cell(dtlStartRowNum + i, 1).Text = "1";
                this.fGrid_Season.Cell(dtlStartRowNum + i, 2).Text = ClientUtil.ToString(domain.Name1) + "/" + this.txtGWBS.Text;
                this.fGrid_Season.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(domain.Name2);
                this.fGrid_Season.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(domain.Name3);
                this.fGrid_Season.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(domain.Name4);
                this.fGrid_Season.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(domain.Name5);
                this.fGrid_Season.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(domain.Name6);
                this.fGrid_Season.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(domain.Name7);
                this.fGrid_Season.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(domain.Name8);
            }
        }
        #endregion

        #region 表二 月度生产计划表
        private void LoadMonthPlanData()
        {
            this.fGrid_MProduce.Cell(1, 1).Text = this.cmbYear.Text + " 年 " + this.cboFiscalMonth.Text + " 月  施  工  生  产  计  划 ";
            this.fGrid_MProduce.Cell(4, 2).Text = projectInfo.Name;
            this.fGrid_MProduce.Cell(7, 2).Text = projectInfo.HandlePersonName;
            this.fGrid_MProduce.Cell(7, 5).Text = ConstObject.TheLogin.ThePerson.Name;
            this.fGrid_MProduce.Cell(7, 8).Text = ConstObject.TheLogin.LoginDate.ToShortDateString();

            int dtlStartRowNum = 6;//模板中的行号
            int dtlCount = list_season.Count;

            //插入明细行
            this.fGrid_MProduce.InsertRow(dtlStartRowNum, dtlCount);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGrid_MProduce.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGrid_MProduce.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            //写入明细数据
            for (int i = 0; i < dtlCount; i++)
            {
                DataDomain domain = (DataDomain)list_monthProduce[i];
                this.fGrid_MProduce.Cell(dtlStartRowNum + i, 1).Text = "1";
                this.fGrid_MProduce.Cell(dtlStartRowNum + i, 2).Text = ClientUtil.ToString(domain.Name1) + "/" + this.txtGWBS.Text;
                this.fGrid_MProduce.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(domain.Name2);
                this.fGrid_MProduce.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(domain.Name3);
                this.fGrid_MProduce.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(domain.Name4);
                this.fGrid_MProduce.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(domain.Name5);
                this.fGrid_MProduce.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(domain.Name6);
                this.fGrid_MProduce.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(domain.Name7);
                this.fGrid_MProduce.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(domain.Name8);
            }
        }
        #endregion

        #region 表三 施工任务完成情况表
        private void LoadTaskCompleteData()
        {
            this.fGrid_Task.Cell(1, 1).Text = this.cmbYear.Text + " 年 " + this.cboFiscalMonth.Text + " 月 施 工 任 务 完  成 情 况 ";
            this.fGrid_Task.Cell(4, 2).Text = projectInfo.Name;
            this.fGrid_Task.Cell(9, 2).Text = projectInfo.HandlePersonName;
            this.fGrid_Task.Cell(9, 11).Text = ConstObject.TheLogin.ThePerson.Name;
            this.fGrid_Task.Cell(9, 16).Text = ConstObject.TheLogin.LoginDate.ToShortDateString();

            int dtlStartRowNum = 8;//模板中的行号
            int dtlCount = list_task.Count;

            //插入明细行
            this.fGrid_Task.InsertRow(dtlStartRowNum, dtlCount);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGrid_Task.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGrid_Task.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            //写入明细数据
            for (int i = 0; i < dtlCount; i++)
            {
                DataDomain domain = (DataDomain)list_task[i];
                this.fGrid_Task.Cell(dtlStartRowNum + i, 1).Text = "1";
                this.fGrid_Task.Cell(dtlStartRowNum + i, 2).Text = ClientUtil.ToString(domain.Name1) + "/" + this.txtGWBS.Text;
                this.fGrid_Task.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(domain.Name2);
                this.fGrid_Task.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(domain.Name3);
                this.fGrid_Task.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(domain.Name4);
                this.fGrid_Task.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(domain.Name5);
                this.fGrid_Task.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(domain.Name6);
                this.fGrid_Task.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(domain.Name7);
                this.fGrid_Task.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(domain.Name8);
                this.fGrid_Task.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(domain.Name9);
                this.fGrid_Task.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.ToString(domain.Name10);
                this.fGrid_Task.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(domain.Name11);
                this.fGrid_Task.Cell(dtlStartRowNum + i, 13).Text = ClientUtil.ToString(domain.Name12);
                this.fGrid_Task.Cell(dtlStartRowNum + i, 14).Text = ClientUtil.ToString(domain.Name13);
                this.fGrid_Task.Cell(dtlStartRowNum + i, 15).Text = ClientUtil.ToString(domain.Name14);
                this.fGrid_Task.Cell(dtlStartRowNum + i, 16).Text = ClientUtil.ToString(domain.Name15);
                this.fGrid_Task.Cell(dtlStartRowNum + i, 17).Text = ClientUtil.ToString(domain.Name16);
                this.fGrid_Task.Cell(dtlStartRowNum + i, 18).Text = ClientUtil.ToString(domain.Name17);
                this.fGrid_Task.Cell(dtlStartRowNum + i, 19).Text = ClientUtil.ToString(domain.Name18);
                this.fGrid_Task.Cell(dtlStartRowNum + i, 20).Text = ClientUtil.ToString(domain.Name19);
                
            }
        }
        #endregion

        #region 表四 施工产值完成情况表
        private void LoadValueCompleteData()
        {
            this.fGrid_Value.Cell(1, 1).Text = this.cmbYear.Text + " 年 " + this.cboFiscalMonth.Text + " 月 施 工 产 值 完  成 情 况 ";
            this.fGrid_Value.Cell(4, 2).Text = projectInfo.Name;
            this.fGrid_Value.Cell(10, 2).Text = projectInfo.HandlePersonName;
            this.fGrid_Value.Cell(10, 11).Text = ConstObject.TheLogin.ThePerson.Name;
            this.fGrid_Value.Cell(10, 16).Text = ConstObject.TheLogin.LoginDate.ToShortDateString();

            int dtlStartRowNum = 9;//模板中的行号
            int dtlCount = list_value.Count;

            //插入明细行
            this.fGrid_Value.InsertRow(dtlStartRowNum, dtlCount);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGrid_Value.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGrid_Value.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            //写入明细数据
            for (int i = 0; i < dtlCount; i++)
            {
                DataDomain domain = (DataDomain)list_value[i];
                this.fGrid_Value.Cell(dtlStartRowNum + i, 1).Text = "1";
                this.fGrid_Value.Cell(dtlStartRowNum + i, 2).Text = ClientUtil.ToString(domain.Name1) + "/" + this.txtGWBS.Text;
                this.fGrid_Value.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(domain.Name2);
                this.fGrid_Value.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(domain.Name3);
                this.fGrid_Value.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(domain.Name4);
                this.fGrid_Value.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(domain.Name5);
                this.fGrid_Value.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(domain.Name6);
                this.fGrid_Value.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(domain.Name7);
                this.fGrid_Value.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(domain.Name8);
                this.fGrid_Value.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(domain.Name9);
                this.fGrid_Value.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.ToString(domain.Name10);
                this.fGrid_Value.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(domain.Name11);
                this.fGrid_Value.Cell(dtlStartRowNum + i, 13).Text = ClientUtil.ToString(domain.Name12);
                this.fGrid_Value.Cell(dtlStartRowNum + i, 14).Text = ClientUtil.ToString(domain.Name13);
                this.fGrid_Value.Cell(dtlStartRowNum + i, 15).Text = ClientUtil.ToString(domain.Name14);
                this.fGrid_Value.Cell(dtlStartRowNum + i, 16).Text = ClientUtil.ToString(domain.Name15);
                this.fGrid_Value.Cell(dtlStartRowNum + i, 17).Text = ClientUtil.ToString(domain.Name16);
                this.fGrid_Value.Cell(dtlStartRowNum + i, 18).Text = ClientUtil.ToString(domain.Name17);
                this.fGrid_Value.Cell(dtlStartRowNum + i, 19).Text = ClientUtil.ToString(domain.Name18);
                this.fGrid_Value.Cell(dtlStartRowNum + i, 20).Text = ClientUtil.ToString(domain.Name19);
                this.fGrid_Value.Cell(dtlStartRowNum + i, 21).Text = ClientUtil.ToString(domain.Name20);
                this.fGrid_Value.Cell(dtlStartRowNum + i, 22).Text = ClientUtil.ToString(domain.Name21);
                this.fGrid_Value.Cell(dtlStartRowNum + i, 23).Text = ClientUtil.ToString(domain.Name22);
                this.fGrid_Value.Cell(dtlStartRowNum + i, 24).Text = ClientUtil.ToString(domain.Name23);
            }
        }
        #endregion

        #region 表五 在建项目完成情况表
        private void LoadBuildingData()
        {
            this.fGrid_Building.Cell(1, 1).Text = this.cmbYear.Text + " 年 " + this.cboFiscalMonth.Text + " 月 在 建 项 目 完 成 简 报 ";
            this.fGrid_Building.Cell(4, 2).Text = projectInfo.Name;
            this.fGrid_Building.Cell(11, 2).Text = projectInfo.HandlePersonName;
            this.fGrid_Building.Cell(11, 11).Text = ConstObject.TheLogin.ThePerson.Name;
            this.fGrid_Building.Cell(11, 16).Text = ConstObject.TheLogin.LoginDate.ToShortDateString();

            int dtlStartRowNum = 10;//模板中的行号
            int dtlCount = list_building.Count;

            //插入明细行
            this.fGrid_Building.InsertRow(dtlStartRowNum, dtlCount);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGrid_Building.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGrid_Building.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            //写入明细数据
            for (int i = 0; i < dtlCount; i++)
            {
                DataDomain domain = (DataDomain)list_building[i];
                this.fGrid_Building.Cell(dtlStartRowNum + i, 1).Text = "1";
                this.fGrid_Building.Cell(dtlStartRowNum + i, 2).Text = ClientUtil.ToString(domain.Name1) + "/" + this.txtGWBS.Text;
                this.fGrid_Building.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(domain.Name2);
                this.fGrid_Building.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(domain.Name3);
                this.fGrid_Building.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(domain.Name4);
                this.fGrid_Building.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(domain.Name5);
                this.fGrid_Building.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(domain.Name6);
                this.fGrid_Building.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(domain.Name7);
                this.fGrid_Building.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(domain.Name8);
                this.fGrid_Building.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(domain.Name9);
                this.fGrid_Building.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.ToString(domain.Name10);
                this.fGrid_Building.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(domain.Name11);
                this.fGrid_Building.Cell(dtlStartRowNum + i, 13).Text = ClientUtil.ToString(domain.Name12);
                this.fGrid_Building.Cell(dtlStartRowNum + i, 14).Text = ClientUtil.ToString(domain.Name13);
                this.fGrid_Building.Cell(dtlStartRowNum + i, 15).Text = ClientUtil.ToString(domain.Name14);
                this.fGrid_Building.Cell(dtlStartRowNum + i, 16).Text = ClientUtil.ToString(domain.Name15);
                this.fGrid_Building.Cell(dtlStartRowNum + i, 17).Text = ClientUtil.ToString(domain.Name16);
                this.fGrid_Building.Cell(dtlStartRowNum + i, 18).Text = ClientUtil.ToString(domain.Name17);
                this.fGrid_Building.Cell(dtlStartRowNum + i, 19).Text = ClientUtil.ToString(domain.Name18);
                this.fGrid_Building.Cell(dtlStartRowNum + i, 20).Text = ClientUtil.ToString(domain.Name19);
                this.fGrid_Building.Cell(dtlStartRowNum + i, 21).Text = ClientUtil.ToString(domain.Name20);
                this.fGrid_Building.Cell(dtlStartRowNum + i, 22).Text = ClientUtil.ToString(domain.Name21);
                this.fGrid_Building.Cell(dtlStartRowNum + i, 23).Text = ClientUtil.ToString(domain.Name22);
                this.fGrid_Building.Cell(dtlStartRowNum + i, 24).Text = ClientUtil.ToString(domain.Name23);
                this.fGrid_Building.Cell(dtlStartRowNum + i, 25).Text = ClientUtil.ToString(domain.Name24);

            }
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