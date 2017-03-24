using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireBalance.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Microsoft.Office.Interop.Excel;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.Util;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using System.IO;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection
{
    public partial class VMaterialHireMonthlyBalanceQuery : TBasicDataView
    {
        MMaterialHireMng model = new MMaterialHireMng();
        
        MatHireBalanceMaster currMaster = new MatHireBalanceMaster();
        private string rentalExp = "租赁费用";
        private string otherExp = "其他费用";

        public VMaterialHireMonthlyBalanceQuery()
        {
            InitializeComponent();
            InitData();
            InitEvent();
        }

        private void InitData()
        {
            txtYear.Text = ConstObject.TheLogin.TheComponentPeriod.NowYear.ToString();
            txtMonth.Text = ConstObject.TheLogin.TheComponentPeriod.NowMonth.ToString();
            txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode1;
        }
        private void InitEvent()
        {
            btnExcel.Click += new EventHandler(btnExcel_Click);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            this.dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
        }

        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            MatHireBalanceMaster master =null;
            dgCostDetail1.Rows.Clear();
            dgOtherCost.Rows.Clear();
            IList list_temp = new ArrayList();
            Hashtable ht = new Hashtable();
            if (dgMaster.CurrentRow.Tag!=null && dgMaster.CurrentRow.Tag is string)
            {
                ObjectQuery oQuery = new ObjectQuery();
              oQuery.AddCriterion(Expression.Eq("Id", dgMaster.CurrentRow.Tag as string ));
              IList lstTemp = model.MaterialHireMngSvr.GetMatBalanceMaster(oQuery);
              master = lstTemp == null || lstTemp.Count == 0 ? null : lstTemp[0] as MatHireBalanceMaster;
              dgMaster.CurrentRow.Tag = master;
            }
            else
            {
                master = dgMaster.CurrentRow.Tag as MatHireBalanceMaster;
            }
            currMaster = master;
            if (master == null) return;
            //租赁费用
            foreach (MatHireBalanceDetail BalanceDetail in master.Details)
            {
                int rowIndex = dgCostDetail1.Rows.Add();
                dgCostDetail1.Rows[rowIndex].Tag = BalanceDetail;
                dgCostDetail1[colMatCode.Name, rowIndex].Value = BalanceDetail.MaterialCode;
                dgCostDetail1[colMatName.Name, rowIndex].Value = BalanceDetail.MaterialName;
                dgCostDetail1[colMatSpec.Name, rowIndex].Value = BalanceDetail.MaterialSpec;
                dgCostDetail1[colDtlEndDate.Name, rowIndex].Value = BalanceDetail.EndDate.ToShortDateString();
                dgCostDetail1[colDtlStartDate.Name, rowIndex].Value = BalanceDetail.StartDate.ToShortDateString();
                dgCostDetail1[colApproachQuantity.Name, rowIndex].Value = BalanceDetail.ApproachQuantity;
                dgCostDetail1[colExitQuantity.Name, rowIndex].Value = BalanceDetail.ExitQuantity;
                dgCostDetail1[colUnusedQuantity.Name, rowIndex].Value = BalanceDetail.UnusedBalQuantity;
                dgCostDetail1[colRentalPrice.Name, rowIndex].Value = BalanceDetail.RentalPrice;
                dgCostDetail1[colDays.Name, rowIndex].Value = BalanceDetail.Days;
                dgCostDetail1[colMoney.Name, rowIndex].Value = BalanceDetail.Money;
                dgCostDetail1[colState.Name, rowIndex].Value = BalanceDetail.BalState;
                dgCostDetail1[colBalRule.Name, rowIndex].Value = BalanceDetail.BalRule;
                dgCostDetail1[colMatUnit.Name, rowIndex].Value = BalanceDetail.MatStandardUnitName;

                dgCostDetail1[colMatCollCode.Name, rowIndex].Value = BalanceDetail.MatCollCode;
                dgCostDetail1[colMatCollDtlQty.Name, rowIndex].Value = BalanceDetail.MatCollDtlQty;
                dgCostDetail1[colMatReturnCode.Name, rowIndex].Value = BalanceDetail.MatReturnCode;
                dgCostDetail1[colMatReturnDtlQty.Name, rowIndex].Value = BalanceDetail.MatReturnDtlQty;

                //根据料具汇总数量(收料，退料，结存，金额)
                if (list_temp.Count == 0)
                {
                    MatHireBalanceDetail t_BalDetail = new MatHireBalanceDetail();
                    t_BalDetail.MaterialResource = BalanceDetail.MaterialResource;
                    t_BalDetail.ApproachQuantity = BalanceDetail.ApproachQuantity;
                    t_BalDetail.ExitQuantity = BalanceDetail.ExitQuantity;
                    t_BalDetail.Money = BalanceDetail.Money;
                    t_BalDetail.UnusedBalQuantity = BalanceDetail.UnusedBalQuantity;
                    t_BalDetail.MaterialCode = BalanceDetail.MaterialCode;
                    t_BalDetail.MaterialName = BalanceDetail.MaterialName;
                    list_temp.Add(BalanceDetail);
                }
                else
                {
                    for (int i = 0; i < list_temp.Count; i++)
                    {
                        MatHireBalanceDetail Detail = list_temp[i] as MatHireBalanceDetail;
                        if (BalanceDetail.MaterialResource.Id == Detail.MaterialResource.Id)
                        {
                            Detail.ApproachQuantity += BalanceDetail.ApproachQuantity;
                            Detail.ExitQuantity += BalanceDetail.ExitQuantity;
                            Detail.Money += BalanceDetail.Money;
                            Detail.UnusedBalQuantity += BalanceDetail.UnusedBalQuantity;
                            break;
                        }
                        else if (i == list_temp.Count - 1)
                        {
                            MatHireBalanceDetail t_BalDetail = new MatHireBalanceDetail();
                            t_BalDetail.MaterialResource = BalanceDetail.MaterialResource;
                            t_BalDetail.ApproachQuantity = BalanceDetail.ApproachQuantity;
                            t_BalDetail.ExitQuantity = BalanceDetail.ExitQuantity;
                            t_BalDetail.Money = BalanceDetail.Money;
                            t_BalDetail.UnusedBalQuantity = BalanceDetail.UnusedBalQuantity;
                            t_BalDetail.MaterialCode = BalanceDetail.MaterialCode;
                            t_BalDetail.MaterialName = BalanceDetail.MaterialName;
                            list_temp.Add(t_BalDetail);
                            break;
                        }
                    }
                }

            }

            //添加汇总信息
            foreach (MatHireBalanceDetail BalanceDetail in list_temp)
            {
                int rowIndex = dgCostDetail1.Rows.Add();
                dgCostDetail1[colMatCode.Name, rowIndex].Value = BalanceDetail.MaterialCode;
                dgCostDetail1[colMatName.Name, rowIndex].Value = "["+BalanceDetail.MaterialName + "]合计";
                dgCostDetail1[colUnusedQuantity.Name, rowIndex].Value = BalanceDetail.UnusedBalQuantity;
                dgCostDetail1[colMoney.Name, rowIndex].Value = BalanceDetail.Money;
                dgCostDetail1[colApproachQuantity.Name, rowIndex].Value = BalanceDetail.ApproachQuantity;
                dgCostDetail1[colExitQuantity.Name, rowIndex].Value = BalanceDetail.ExitQuantity;
            }

            int c= master.MatBalOtherCostDetails.Count;

            //其他费用
            foreach (MatHireBalanceOtherCostDtl MatBalOtherCostDetail in master.MatBalOtherCostDetails)
            {
                if (MatBalOtherCostDetail.CostMoney != 0)
                {
                    int rowIndex = dgOtherCost.Rows.Add();
                    dgOtherCost.Rows[rowIndex].Tag = MatBalOtherCostDetail;
                    dgOtherCost[colBusinessType.Name, rowIndex].Value = MatBalOtherCostDetail.BusinessType;
                    dgOtherCost[colBusinessCode.Name, rowIndex].Value = MatBalOtherCostDetail.BusinessCode;
                    dgOtherCost[colOtherCostType.Name, rowIndex].Value = MatBalOtherCostDetail.CostType;
                    dgOtherCost[colMaterialCode.Name, rowIndex].Value = MatBalOtherCostDetail.MaterialCode;
                    dgOtherCost[colMaterialName.Name, rowIndex].Value = MatBalOtherCostDetail.MaterialName;
                    dgOtherCost[colMaterialSpec.Name, rowIndex].Value = MatBalOtherCostDetail.MaterialSpec;
                    dgOtherCost[colOtherCostMoney.Name, rowIndex].Value = MatBalOtherCostDetail.CostMoney;
                    //MatBalOtherCostDetail MatCostDetail = new MatBalOtherCostDetail();
                    //MatCostDetail.CostType = MatBalOtherCostDetail.CostType;
                    //MatCostDetail.BusinessType
                    if (ht.Count == 0)
                    {
                        ht.Add(MatBalOtherCostDetail.CostType, MatBalOtherCostDetail.CostMoney);
                    }
                    else
                    {
                        if (ht.Contains(MatBalOtherCostDetail.CostType))
                        {
                            decimal temp = (decimal)ht[MatBalOtherCostDetail.CostType];
                            MatBalOtherCostDetail.CostMoney += temp;
                            ht.Remove(MatBalOtherCostDetail.CostType);
                            ht.Add(MatBalOtherCostDetail.CostType, MatBalOtherCostDetail.CostMoney);

                        }
                        else
                        {
                            ht.Add(MatBalOtherCostDetail.CostType, MatBalOtherCostDetail.CostMoney);
                        }
                    }
                }
               
            }
            //添加汇总信息
            foreach (System.Collections.DictionaryEntry objSys in ht)
            {
                int rowIndex = dgOtherCost.Rows.Add();
                dgOtherCost[colOtherCostType.Name, rowIndex].Value = ClientUtil.ToString(objSys.Key) + "合计";
                dgOtherCost[colOtherCostMoney.Name, rowIndex].Value = ClientUtil.ToString(objSys.Value);
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
            dgCostDetail1.Rows.Clear();
            dgOtherCost.Rows.Clear();

            ObjectQuery oq = new ObjectQuery();
            if (this.TenantSelector.SelectedProject != null)
            {
                oq.AddCriterion(Expression.Eq("ProjectId", this.TenantSelector.SelectedProject.Id));
            }
            if (txtYear.Text != "")
            {
                oq.AddCriterion(Expression.Eq("FiscalYear", Convert.ToInt32(txtYear.Text)));
            }
            if (txtMonth.Text != "")
            {
                oq.AddCriterion(Expression.Eq("FiscalMonth", Convert.ToInt32(txtMonth.Text)));
            }
            if (txtSupplier.Text != "")
            {
                oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", txtSupplier.Result[0] as SupplierRelationInfo));
            }

            try
            {
                FlashScreen.Show("正在查询料具月度结算表...");

                IList list = model.MaterialHireMngSvr.ObjectQuery(typeof(MatHireBalanceMaster),oq);  //model.MaterialHireMngSvr.GetMatBalanceMaster(oq);
                ShowMasterList(list);
                FlashScreen.Close();
                System.Threading.Thread.Sleep(1);
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                System.Threading.Thread.Sleep(1);
                this.dgMaster.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
          

            this.dgMaster.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            this.dgMaster.Focus();
            MessageBox.Show("查询完毕！");
              
        }
        //显示主表
        private void ShowMasterList(IList list)
        {
            this.dgMaster.SelectionChanged -= new EventHandler(dgMaster_SelectionChanged);
            dgMaster.Rows.Clear();
            if (list == null || list.Count == 0) return;
            foreach (MatHireBalanceMaster master in list)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master.Id;
                dgMaster[colOriContractNo.Name, rowIndex].Value = master.OldContractNum;
                dgMaster[colFiscalYear.Name, rowIndex].Value = master.FiscalYear;
                dgMaster[colFiscalMonth.Name, rowIndex].Value = master.FiscalMonth;
                dgMaster[this.colOtherMoney.Name, rowIndex].Value = master.OtherMoney;
                dgMaster[colSupplyInfo.Name, rowIndex].Value = master.SupplierName;
                dgMaster[colStartDate.Name, rowIndex].Value = master.StartDate.ToShortDateString();
                dgMaster[colEndDate.Name, rowIndex].Value = master.EndDate.ToShortDateString();
                dgMaster[colSumMoney.Name, rowIndex].Value = master.SumMatMoney;
                dgMaster[colSumQuantity.Name, rowIndex].Value = master.SumMatQuantity;
                dgMaster[colOperDate.Name, rowIndex].Value = master.CreateDate.ToShortDateString();
                dgMaster[colOtherMoney.Name, rowIndex].Value = master.OtherMoney;
            }
            dgMaster.CurrentCell = dgMaster[1, 0];
            dgMaster_SelectionChanged(dgMaster, new EventArgs());
            this.dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
        }
        /// <summary>
        /// 导出EXCEL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnExcel_Click(object sender, EventArgs e)
        {
            string fileName = StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgMaster, false);
            if (fileName == "")
                return;
            FlashScreen.Show("正在导出月度结算报表...");
            ApplicationClass excel = new ApplicationClass();
            int startIndex = fileName.LastIndexOf("\\") + 1;
            int endIndex = fileName.IndexOf(".x");
            string mainStr = fileName.Substring(startIndex, endIndex - startIndex);

            //主文件对象
            Workbook workbook = excel.Workbooks.Open(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet = workbook.Sheets[1] as Worksheet;
            string mainExp = currMaster.SupplierName + currMaster.FiscalYear + "年" + currMaster.FiscalMonth + "月" + "结算信息";
            mySheet.Name = mainExp;

            string tempName = fileName.Replace(mainStr, rentalExp);
            StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgCostDetail1, tempName);
            Workbook workbook1 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet1 = workbook1.Sheets[1] as Worksheet;
            mySheet1.Name = rentalExp;

            tempName = fileName.Replace(mainStr, otherExp);
            StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgOtherCost, tempName);
            Workbook workbook2 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet2 = workbook2.Sheets[1] as Worksheet;
            mySheet2.Name = otherExp;

            try
            {
                mySheet2.Copy(Type.Missing, mySheet);
                mySheet1.Copy(Type.Missing, mySheet);
                workbook.Save();
            }
            catch (Exception e1)
            {
                FlashScreen.Close();
                throw new Exception("导出月度结算报表出错！");
            }
            finally
            {
                FlashScreen.Close();
                //关闭工作表和退出Excel
                workbook.Close(false, Type.Missing, Type.Missing);
                workbook1.Close(false, Type.Missing, Type.Missing);
                workbook2.Close(false, Type.Missing, Type.Missing);
                //如果报表文件存在，先删除
                if (File.Exists(fileName.Replace(mainStr, rentalExp)))
                {
                    File.Delete(fileName.Replace(mainStr, rentalExp));
                }
                if (File.Exists(fileName.Replace(mainStr, otherExp)))
                {
                    File.Delete(fileName.Replace(mainStr, otherExp));
                }
                excel.Quit();
                excel = null;
            }
            MessageBox.Show("导出月度结算报表成功！");

            //if (tabControl1.SelectedTab.Name.Equals("tabPage1"))
            //{
            //    StaticMethod.ExcelClass.SaveDataGridViewToExcel(dgCostDetail1, true);
            //}
            //else if (tabControl1.SelectedTab.Name.Equals("tabPage2"))
            //{
            //    StaticMethod.ExcelClass.SaveDataGridViewToExcel(dgOtherCost, true);
            //}
            //Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgMaster, true);
        }

    }
}
