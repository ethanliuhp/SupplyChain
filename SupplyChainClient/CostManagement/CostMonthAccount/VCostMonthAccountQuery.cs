using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Microsoft.Office.Interop.Excel;
using System.IO;
using Application.Business.Erp.SupplyChain.Client.StockMng;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    public partial class VCostMonthAccountQuery: TBasicDataView
    {
        MCostMonthAccount model = new MCostMonthAccount();
        MStockMng stockModel = new MStockMng();
        CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
        private string accountMx = "成本核算明细";
        private string accountConsume = "成本核算资源耗用";

        public VCostMonthAccountQuery()
        {
            InitializeComponent();
            InitData();
            InitEvent();
        }

        private void InitData()
        {
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
        }
        private void InitEvent()
        {
            btnExcel.Click += new EventHandler(btnExcel_Click);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            this.dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            this.dgCostDetail.SelectionChanged += new EventHandler(dgCostDetail_SelectionChanged);
        }

        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            dgCostDetail.Rows.Clear();
            dgTotalConsume.Rows.Clear();

            CostMonthAccountBill master = dgMaster.CurrentRow.Tag as CostMonthAccountBill;
            if (master == null) return;
            DataSet dataSet = model.CostMonthAccSrv.QuerytCostMonthAccountBill(master.Id);
            //单据组成
            this.dgBill.Rows.Clear();
            System.Data.DataTable dataTable = dataSet.Tables[0];
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgBill.Rows.Add();
                dgBill[this.colBillCode.Name, rowIndex].Value = dataRow["code"];
                dgBill[this.colBillType.Name, rowIndex].Value = dataRow["billtype"];
                dgBill[this.colMoney.Name, rowIndex].Value = dataRow["summoney"];
                dgBill[this.colPersonName.Name, rowIndex].Value = dataRow["personname"];
                dgBill[this.colCreateDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["createdate"]).ToShortDateString();
            }

            //月度成本核算明细
            foreach (CostMonthAccountDtl detail in master.Details)
            {
                int rowIndex = dgCostDetail.Rows.Add();
                dgCostDetail.Rows[rowIndex].Tag = detail;
                dgCostDetail[this.colDTaskNode.Name, rowIndex].Value = detail.AccountTaskNodeName;
                dgCostDetail[this.colDTaskMx.Name, rowIndex].Value = detail.ProjectTaskDtlName;
                dgCostDetail[this.colCostItem.Name, rowIndex].Value = detail.CostItemName;
                dgCostDetail[this.colCurrRealQty.Name, rowIndex].Value = detail.CurrRealQuantity;
                dgCostDetail[this.colCurrRealPrice.Name, rowIndex].Value = detail.CurrRealPrice;
                dgCostDetail[this.colCurrRealTotalPrice.Name, rowIndex].Value = detail.CurrRealTotalPrice;
                dgCostDetail[this.colCurrIncomeQty.Name, rowIndex].Value = detail.CurrIncomeQuantity;
                dgCostDetail[this.colCurrIncomeTotalPrice.Name, rowIndex].Value = detail.CurrIncomeTotalPrice;
                dgCostDetail[this.colCurrResponsiQty.Name, rowIndex].Value = detail.CurrResponsiQuantity;
                dgCostDetail[this.colCurrResTotalPrice.Name, rowIndex].Value = detail.CurrResponsiTotalPrice;
                dgCostDetail[this.colCurrEarnValue.Name, rowIndex].Value = detail.CurrEarnValue;
                dgCostDetail[this.colCurrCompletedPercent.Name, rowIndex].Value = detail.CurrCompletedPercent;
                dgCostDetail[this.colSumRealQuantity.Name, rowIndex].Value = detail.SumRealQuantity;
                dgCostDetail[this.colSumRealTotalPrice.Name, rowIndex].Value = detail.SumRealQuantity;
                dgCostDetail[this.colSumResponsiQuantity.Name, rowIndex].Value = detail.SumResponsiQuantity;
                dgCostDetail[this.colSumResTotalPrice.Name, rowIndex].Value = detail.SumResponsiTotalPrice;
                dgCostDetail[this.colSumIncomeQuantity.Name, rowIndex].Value = detail.SumIncomeQuantity;
                dgCostDetail[this.colSumIncomeTotalPrice.Name, rowIndex].Value = detail.SumIncomeTotalPrice;
                dgCostDetail[this.colSumEarnValue.Name, rowIndex].Value = detail.SumEarnValue;
                dgCostDetail[this.colSumCompletedPercent.Name, rowIndex].Value = detail.SumCompletedPercent;

                //月度成本核算物资耗用
                foreach (CostMonthAccDtlConsume dtlConsume in detail.Details)
                {
                    int rowIndexConsume = this.dgTotalConsume.Rows.Add();
                    dgTotalConsume.Rows[rowIndexConsume].Tag = dtlConsume;
                    dgTotalConsume[this.colRationUnitName4.Name, rowIndexConsume].Value = dtlConsume.RationUnitName;
                    dgTotalConsume[this.colCostingSubjectName4.Name, rowIndexConsume].Value = dtlConsume.CostingSubjectName;
                    dgTotalConsume[this.colResourceTypeName4.Name, rowIndexConsume].Value = dtlConsume.ResourceTypeName;
                    dgTotalConsume[this.colCurrRealConsumeQty4.Name, rowIndexConsume].Value = dtlConsume.CurrRealConsumeQuantity;
                    dgTotalConsume[this.colCurrRealConsumePrice4.Name, rowIndexConsume].Value = dtlConsume.CurrRealConsumePrice;
                    dgTotalConsume[this.colCurrRealConsumeTPrice4.Name, rowIndexConsume].Value = dtlConsume.CurrRealConsumeTotalPrice;
                    dgTotalConsume[this.colCurrRealConsumePlanQty4.Name, rowIndexConsume].Value = dtlConsume.CurrRealConsumePlanQuantity;
                    dgTotalConsume[this.colCurrRealConsumePlanTPrice4.Name, rowIndexConsume].Value = dtlConsume.CurrRealConsumePlanTotalPrice;
                    dgTotalConsume[this.colCurrIncomeQty4.Name, rowIndexConsume].Value = dtlConsume.CurrIncomeQuantity;
                    dgTotalConsume[this.colCurrIncomeTotalPrice4.Name, rowIndexConsume].Value = dtlConsume.CurrIncomeTotalPrice;
                    dgTotalConsume[this.colCurrResponsiConsumeQty4.Name, rowIndexConsume].Value = dtlConsume.CurrResponsiConsumeQuantity;
                    dgTotalConsume[this.colCurrResConsumeTotalPrice4.Name, rowIndexConsume].Value = dtlConsume.CurrResponsiConsumeTotalPrice;
                    dgTotalConsume[this.colSumRealConsumeQty4.Name, rowIndexConsume].Value = dtlConsume.SumRealConsumeQuantity;
                    dgTotalConsume[this.colSumRealConsumeTotalPrice4.Name, rowIndexConsume].Value = dtlConsume.SumRealConsumeTotalPrice;
                    dgTotalConsume[this.colSumRealConsumePlanQty4.Name, rowIndexConsume].Value = dtlConsume.SumRealConsumePlanQuantity;
                    dgTotalConsume[this.colSumRealConsumePlanTPrice4.Name, rowIndexConsume].Value = dtlConsume.SumRealConsumePlanTotalPrice;
                    dgTotalConsume[this.colSumIncomeQuantity4.Name, rowIndexConsume].Value = dtlConsume.SumIncomeQuantity;
                    dgTotalConsume[this.colSumIncomeTotalPrice4.Name, rowIndexConsume].Value = dtlConsume.SumIncomeTotalPrice;
                    dgTotalConsume[this.colSumResponsiConsumeQy4.Name, rowIndexConsume].Value = dtlConsume.SumResponsiConsumeQuantity;
                    dgTotalConsume[this.colSumResponsiConsumeTPrice4.Name, rowIndexConsume].Value = dtlConsume.SumResponsiConsumeTotalPrice;
                }

            }

            dgCostDetail_SelectionChanged(dgMaster, new EventArgs());
        }

        void dgCostDetail_SelectionChanged(object sender, EventArgs e)
        {
            this.dgConsume.Rows.Clear();
            if (dgCostDetail.CurrentRow == null)
                return;
            CostMonthAccountDtl detail = dgCostDetail.CurrentRow.Tag as CostMonthAccountDtl;
            if (detail == null) return;
            //月度成本核算物资耗用
            foreach (CostMonthAccDtlConsume dtlConsume in detail.Details)
            {
                int rowIndex = this.dgConsume.Rows.Add();
                dgConsume.Rows[rowIndex].Tag = dtlConsume;
                dgConsume[this.colRationUnitName.Name, rowIndex].Value = dtlConsume.RationUnitName;
                dgConsume[this.colCostingSubjectName.Name, rowIndex].Value = dtlConsume.CostingSubjectName;
                dgConsume[this.colResourceTypeName.Name, rowIndex].Value = dtlConsume.ResourceTypeName;
                dgConsume[this.colCurrRealConsumeQty1.Name, rowIndex].Value = dtlConsume.CurrRealConsumeQuantity;
                dgConsume[this.colCurrRealConsumePrice1.Name, rowIndex].Value = dtlConsume.CurrRealConsumePrice;
                dgConsume[this.colCurrRealConsumeTPrice1.Name, rowIndex].Value = dtlConsume.CurrRealConsumeTotalPrice;
                dgConsume[this.colCurrRealConsumePlanQty1.Name, rowIndex].Value = dtlConsume.CurrRealConsumePlanQuantity;
                dgConsume[this.colCurrRealConsumePlanTPrice1.Name, rowIndex].Value = dtlConsume.CurrRealConsumePlanTotalPrice;
                dgConsume[this.colCurrIncomeQty1.Name, rowIndex].Value = dtlConsume.CurrIncomeQuantity;
                dgConsume[this.colCurrIncomeTotalPrice1.Name, rowIndex].Value = dtlConsume.CurrIncomeTotalPrice;
                dgConsume[this.colCurrResponsiConsumeQty1.Name, rowIndex].Value = dtlConsume.CurrResponsiConsumeQuantity;
                dgConsume[this.colCurrResConsumeTotalPrice1.Name, rowIndex].Value = dtlConsume.CurrResponsiConsumeTotalPrice;
                dgConsume[this.colSumRealConsumeQty1.Name, rowIndex].Value = dtlConsume.SumRealConsumeQuantity;
                dgConsume[this.colSumRealConsumeTotalPrice1.Name, rowIndex].Value = dtlConsume.SumRealConsumeTotalPrice;
                dgConsume[this.colSumRealConsumePlanQty1.Name, rowIndex].Value = dtlConsume.SumRealConsumePlanQuantity;
                dgConsume[this.colSumRealConsumePlanTPrice1.Name, rowIndex].Value = dtlConsume.SumRealConsumePlanTotalPrice;
                dgConsume[this.colSumIncomeQuantity1.Name, rowIndex].Value = dtlConsume.SumIncomeQuantity;
                dgConsume[this.colSumIncomeTotalPrice1.Name, rowIndex].Value = dtlConsume.SumIncomeTotalPrice;
                dgConsume[this.colSumResponsiConsumeQy1.Name, rowIndex].Value = dtlConsume.SumResponsiConsumeQuantity;
                dgConsume[this.colSumResponsiConsumeTPrice1.Name, rowIndex].Value = dtlConsume.SumResponsiConsumeTotalPrice;
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgMaster.Rows.Clear();           
            dgCostDetail.Rows.Clear();
            this.dgConsume.Rows.Clear();

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            if (cmbYear.Text != "")
            {
                oq.AddCriterion(Expression.Eq("Kjn", Convert.ToInt32(this.cmbYear.Text)));
            }
            if (cboFiscalMonth.Text != "")
            {
                oq.AddCriterion(Expression.Eq("Kjy", Convert.ToInt32(this.cboFiscalMonth.Text)));
            }

            try
            {
                IList list = model.CostMonthAccSrv.GetCostMonthAccountBill(oq);
                
                //获取相关单据信息
                ShowMasterList(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }

            this.dgMaster.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            //MessageBox.Show("查询完毕！");
        }
        //显示主表
        private void ShowMasterList(IList list)
        {
            dgMaster.Rows.Clear();
            if (list == null || list.Count == 0) return;
            foreach (CostMonthAccountBill master in list)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[this.colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);
                dgMaster[this.colFiscalYear.Name, rowIndex].Value = master.Kjn;
                dgMaster[colFiscalMonth.Name, rowIndex].Value = master.Kjy;
                dgMaster[this.colAccountOrg.Name, rowIndex].Value = master.AccountOrgName;
                dgMaster[colEndDate.Name, rowIndex].Value = master.EndTime.ToShortDateString();
                dgMaster[colOperDate.Name, rowIndex].Value = master.CreateDate.ToShortDateString();
                dgMaster[this.colTaskNode.Name, rowIndex].Value = master.AccountTaskName;
                dgMaster[this.colPerson.Name, rowIndex].Value = master.AccountPersonName;
            }
            dgMaster.CurrentCell = dgMaster[1, 0];
            dgMaster_SelectionChanged(dgMaster, new EventArgs());
        }
        /// <summary>
        /// 导出EXCEL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnExcel_Click(object sender, EventArgs e)
        {
            string fileName = StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgMaster, false);
            CostMonthAccountBill master = dgMaster.CurrentRow.Tag as CostMonthAccountBill;
            if (master == null)
            {
                MessageBox.Show("未查询到月度成本核算数据！");
                return;
            }
            if (fileName == "")
                return;
            FlashScreen.Show("正在导出月度成本核算报表...");
            ApplicationClass excel = new ApplicationClass();
            int startIndex = fileName.LastIndexOf("\\") + 1;
            int endIndex = fileName.IndexOf(".x");
            string mainStr = fileName.Substring(startIndex, endIndex - startIndex);

            //主文件对象
            Workbook workbook = excel.Workbooks.Open(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet = workbook.Sheets[1] as Worksheet;
            string mainExp = master.Kjn + "年" + master.Kjy + "月" + "成本核算信息";
            mySheet.Name = mainExp;

            string tempName = fileName.Replace(mainStr, accountMx);
            StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgCostDetail, tempName);
            Workbook workbook1 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet1 = workbook1.Sheets[1] as Worksheet;
            mySheet1.Name = accountMx;

            tempName = fileName.Replace(mainStr, accountConsume);
            StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgTotalConsume, tempName);
            Workbook workbook2 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet2 = workbook2.Sheets[1] as Worksheet;
            mySheet2.Name = accountConsume;

            try
            {
                mySheet2.Copy(Type.Missing, mySheet);
                mySheet1.Copy(Type.Missing, mySheet);
                workbook.Save();
            }
            catch (Exception e1)
            {
                FlashScreen.Close();
                throw new Exception("导出月度成本核算报表出错！");
            }
            finally
            {
                FlashScreen.Close();
                //关闭工作表和退出Excel
                workbook.Close(false, Type.Missing, Type.Missing);
                workbook1.Close(false, Type.Missing, Type.Missing);
                workbook2.Close(false, Type.Missing, Type.Missing);
                //如果报表文件存在，先删除
                if (File.Exists(fileName.Replace(mainStr, accountMx)))
                {
                    File.Delete(fileName.Replace(mainStr, accountMx));
                }
                if (File.Exists(fileName.Replace(mainStr, accountConsume)))
                {
                    File.Delete(fileName.Replace(mainStr, accountConsume));
                }
                excel.Quit();
                excel = null;
            }
            MessageBox.Show("导出月度成本核算报表成功！");
        }

    }
}
