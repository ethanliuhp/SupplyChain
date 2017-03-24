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
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Business.Erp.SupplyChain.Client.StockMng;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostBusinessAccount
{
    public partial class VSpecialBusinessReport : TBasicDataView
    {
        MCostMonthAccount model = new MCostMonthAccount();
        MStockMng stockModel = new MStockMng();
        OperationOrgInfo accountOrg = new OperationOrgInfo();//归属核算组织
        CostMonthAccountBill costBill = new CostMonthAccountBill();
        IList list_order = new ArrayList();
        DataDomain indicator_domain = new DataDomain();

        #region 基本数据
        private string loginPersonName = ConstObject.LoginPersonInfo.Name;
        private string loginDate = ConstObject.LoginDate.ToShortDateString();
        private CurrentProjectInfo projectInfo;
        string orderStr = "合同签报情况表(安装)";
        string indicatorStr = "项目责任成本消耗指标情况表(安装)";
        #endregion

        public VSpecialBusinessReport()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }

        private void InitData()
        {          
            this.cmbType.SelectedIndex = 0;
            this.RemoveTab(this.cmbType.SelectedIndex);
            projectInfo = StaticMethod.GetProjectInfo();
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

            this.fGrid_Order.Rows = 1;
            this.fGrid_Indicate.Rows = 1;
        }

        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            this.btnSelectGWBS.Click += new EventHandler(btSelectGWBS_Click);
        }

        void btSelectGWBS_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            if (txtAccountRootNode.Tag != null)
            {
                frm.DefaultSelectedGWBS = txtAccountRootNode.Tag as GWBSTree;
            }
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                TreeNode root = frm.SelectResult[0];

                GWBSTree task = root.Tag as GWBSTree;
                if (task != null)
                {
                    txtAccountRootNode.Text = task.Name;
                    txtAccountRootNode.Tag = task;
                }
            }
        }
        void btnModelConfig_Click(object sender, EventArgs e)
        {
            VCostReporterConfig oVCostReporterConfig = new VCostReporterConfig();
            oVCostReporterConfig.Show();
        }
        void btnExcel_Click(object sender, EventArgs e)
        {
            string fileName = this.fGrid_Order.ExportToExcel("商务报表(安装)", false, false, true);
            if (fileName == "")
                return;
            FlashScreen.Show("正在导出商务分析报表...");
            ApplicationClass excel = new ApplicationClass();

            //主文件对象
            Workbook workbook = excel.Workbooks.Open(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet = workbook.Sheets[1] as Worksheet;
            mySheet.Name = orderStr;

            int startIndex = fileName.LastIndexOf("\\") + 1;
            int endIndex = fileName.IndexOf(".x");
            orderStr = fileName.Substring(startIndex, endIndex - startIndex);

            string tempName = fileName.Replace(orderStr, indicatorStr);
            this.fGrid_Indicate.ExportToExcel(tempName, false, false, false);
            Workbook workbook1 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet1 = workbook1.Sheets[1] as Worksheet;
            mySheet1.Name = indicatorStr;

            try
            {
                mySheet1.Copy(Type.Missing, mySheet);
                workbook.Save();
            }
            catch (Exception e1)
            {
                FlashScreen.Close();
                throw new Exception("导出商务分析报表出错！");
            }
            finally
            {
                FlashScreen.Close();
                //关闭工作表和退出Excel
                workbook.Close(false, Type.Missing, Type.Missing);
                workbook1.Close(false, Type.Missing, Type.Missing);
                //如果报表文件存在，先删除
                if (File.Exists(fileName.Replace(orderStr, indicatorStr)))
                {
                    File.Delete(fileName.Replace(orderStr, indicatorStr));
                }
                excel.Quit();
                excel = null;
            }
            MessageBox.Show("导出商务分析报表成功！");
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RemoveTab(this.cmbType.SelectedIndex);
            this.fGrid_Order.Rows = 1;
            this.fGrid_Indicate.Rows = 1;
        }

        private void RemoveTab(int rType)
        {
            this.tabBusinessReport.TabPages.Clear();
            if (rType == 0)
            {
                this.tabBusinessReport.TabPages.Add(this.tabIndicate);
                this.tabBusinessReport.TabPages.Add(this.tabOrder);
            }
            else if (rType == 1)
            {
                this.tabBusinessReport.TabPages.Remove(this.tabIndicate);
                this.tabBusinessReport.TabPages.Add(this.tabOrder);
            }
            else if (rType == 2)
            {
                this.tabBusinessReport.TabPages.Add(this.tabIndicate);
                this.tabBusinessReport.TabPages.Remove(this.tabOrder);
            }
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            if (txtAccountRootNode.Tag == null)
            {
                MessageBox.Show("请选择核算节点！");
                return;
            }

            LoadTempleteFile("合同签报情况表(安装).flx");
            LoadTempleteFile(indicatorStr + ".flx");
            list_order.Clear();
            indicator_domain = new DataDomain();
            //载入数据
            GWBSTree acctNode = txtAccountRootNode.Tag as GWBSTree;
            Hashtable ht = model.CostMonthAccSrv.GetSpecialBusinessData(ClientUtil.ToInt(this.cmbYear.Text), ClientUtil.ToInt(this.cboFiscalMonth.Text), projectInfo.Id, acctNode.Id);
            if (ht != null && ht.Count > 0 )
            {
                list_order = (ArrayList)ht["1"];
                indicator_domain = (DataDomain)ht["2"];
            }

            //载入数据
            this.LoadOrderData();
            this.LoadIndicatorData();

            //设置外观
            CommonUtil.SetFlexGridFace(this.fGrid_Order);
            CommonUtil.SetFlexGridFace(this.fGrid_Indicate);
        }

        private void LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式
                if (modelName == (orderStr + ".flx"))
                {
                    fGrid_Order.OpenFile(path + "\\" + modelName);//载入格式
                }
                else if (modelName == (indicatorStr + ".flx"))
                {
                    fGrid_Indicate.OpenFile(path + "\\" + modelName);//载入格式
                }       
            }
            else
            {
                MessageBox.Show("未找到模板格式文件" + modelName);
                return;
            }
        }

        #region 表一 合同签报情况表
        private void LoadOrderData()
        {
            this.fGrid_Order.Cell(2, 2).Text = projectInfo.Name;

            int dtlStartRowNum = 5;//模板中的行号
            int dtlCount = list_order.Count;

            //插入明细行
            this.fGrid_Order.InsertRow(dtlStartRowNum, dtlCount);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGrid_Order.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGrid_Order.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);
            range.Alignment = FlexCell.AlignmentEnum.CenterCenter;

            //写入明细数据
            for (int i = 0; i < dtlCount; i++)
            {
                DataDomain domain = (DataDomain)list_order[i];
                this.fGrid_Order.Cell(dtlStartRowNum + i, 1).Text = (i + 1) + "";
                this.fGrid_Order.Cell(dtlStartRowNum + i, 2).Text = ClientUtil.ToString(domain.Name1);
                this.fGrid_Order.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(domain.Name2);
                this.fGrid_Order.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(domain.Name3);
                this.fGrid_Order.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(domain.Name4);
                if (ClientUtil.ToString(domain.Name1).IndexOf("小计") != -1)
                {
                    this.fGrid_Order.Range(dtlStartRowNum + i, 2, dtlStartRowNum + i, 4).Merge();
                }
            }
        }
        #endregion

        #region 表二 项目责任成本消耗指标情况表

        private void LoadIndicatorData()
        {
            this.fGrid_Indicate.Cell(2, 2).Text = projectInfo.Name;
            decimal sumIncomeTotalPrice = decimal.Round(ClientUtil.ToDecimal(indicator_domain.Name1) / 10000, 2);
            this.fGrid_Indicate.Cell(4, 4).Text = sumIncomeTotalPrice + "";//累计（合同收入C51101）
            decimal sumXCRealTotalPrice = decimal.Round(ClientUtil.ToDecimal(indicator_domain.Name3) / 10000, 2);
            this.fGrid_Indicate.Cell(5, 4).Text = sumXCRealTotalPrice + "";//累计（实际成本C513）
            if (sumIncomeTotalPrice != 0)
            {
                this.fGrid_Indicate.Cell(6, 4).Text = decimal.Round( sumXCRealTotalPrice * 100 / sumIncomeTotalPrice,2) + "";
            }
            this.fGrid_Indicate.Cell(8, 4).Text = sumIncomeTotalPrice + "";//累计（合同收入C51101）
            decimal sumLWRealTotalPrice = decimal.Round(ClientUtil.ToDecimal(indicator_domain.Name2) / 10000, 2);
            this.fGrid_Indicate.Cell(9, 4).Text = sumLWRealTotalPrice + "";//累计（劳务实际成本C51101）
            if (sumIncomeTotalPrice != 0)
            {
                this.fGrid_Indicate.Cell(10, 4).Text = decimal.Round(sumLWRealTotalPrice * 100 / sumIncomeTotalPrice, 2) + "";
            }
        }
        #endregion
    }
}