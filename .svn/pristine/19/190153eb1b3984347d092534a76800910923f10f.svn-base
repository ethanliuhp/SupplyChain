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
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount;
using Application.Business.Erp.SupplyChain.StockManage.StockBalManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VFactoringDataReport : TBasicDataView
    {
        MFinanceMultData service = new MFinanceMultData();
        private CurrentProjectInfo projectInfo;
        string detailExptr = "保理台帐";
        string flexName = "保理台帐.flx";
        IList list = new ArrayList();

        public VFactoringDataReport()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }

        private void InitData()
        {
            dtpDateBegin.Value = DateTime.Now.AddYears(-1);
            dtpDateEnd.Value = DateTime.Now;
            this.fGridDetail.Rows = 1;
            LoadTempleteFile(flexName);
        }

        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
        }


        void btnExcel_Click(object sender, EventArgs e)
        {
            fGridDetail.ExportToExcel(detailExptr, false, false, true);
        }

        private void LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式
                if (modelName == "保理台帐.flx")
                {
                    fGridDetail.OpenFile(path + "\\" + modelName);//载入格式
                }
            }
            else
            {
                MessageBox.Show("未找到模板格式文件" + modelName);
                return;
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            LoadTempleteFile(flexName);
            LoadDetailFile();
        }

        private void LoadDetailFile()
        {
            FlashScreen.Show("正在生成[保理台帐]报告...");
            try
            {
                var startDate = dtpDateBegin.Value.Date;
                var endDate = dtpDateEnd.Value.Date;
                var condition = new ObjectQuery();
                condition.AddCriterion(Expression.Ge("Master.CreateDate", startDate));
                condition.AddCriterion(Expression.Le("Master.CreateDate", endDate));
                list = service.FinanceMultDataSrv.Query(typeof(FactoringDataDetail), condition);
                decimal dSumBalance = 0, dSumAmountPayable = 0;
                int curRow = 0;
                fGridDetail.AutoRedraw = false;
                fGridDetail.InsertRow(3, list.Count);
                string sTitle = string.Format("{0}至{1}保理业务费用明细表", startDate.Date.ToString("yyyy年MM月dd日"), endDate.Date.ToString("yyyy年MM月dd日"));
                fGridDetail.Cell(1, 1).Text = sTitle;
                for (int i = 0; i < list.Count; i++)
                {
                    var temp = (FactoringDataDetail)list[i];
                    curRow = 3 + i;
                    fGridDetail.Cell(curRow, 1).Text = (i + 1).ToString();
                    fGridDetail.Cell(curRow, 2).Text = temp.DepartmentName;
                    fGridDetail.Cell(curRow, 3).Text = temp.ProjectName;
                    fGridDetail.Cell(curRow, 4).Text = temp.BankName;
                    dSumBalance += temp.Balance;
                    fGridDetail.Cell(curRow, 5).Text = temp.Balance.ToString();
                    fGridDetail.Cell(curRow, 6).Text = (temp.Rate * 100).ToString("0.0") + "%";
                    fGridDetail.Cell(curRow, 7).Text = temp.StartDate.ToString("yyyy-MM-dd");
                    fGridDetail.Cell(curRow,8).Text = temp.EndDate.ToString("yyyy-MM-dd");
                    fGridDetail.Cell(curRow, 9).Text = temp.PayType;
                    fGridDetail.Cell(curRow, 10).Text = temp.StartChargingDate.ToString("yyyy-MM-dd");
                    fGridDetail.Cell(curRow, 11).Text = temp.EndChargingDate.ToString("yyyy-MM-dd");
                    fGridDetail.Cell(curRow, 12).Text = temp.TotalDay.ToString();
                    fGridDetail.Cell(curRow, 13).Text = temp.AmountPayable.ToString();
                    dSumAmountPayable += temp.AmountPayable;

                }
                fGridDetail.Cell(curRow + 1, 5).Text = dSumBalance.ToString();
                fGridDetail.Cell(curRow + 1, 13).Text = dSumAmountPayable.ToString();
                for (int tt = 0; tt < fGridDetail.Cols; tt++)
                {
                    fGridDetail.Column(tt).AutoFit();
                }
            }
            catch (Exception e1)
            {
                throw new Exception("生成[保理台帐]报告异常[" + e1.Message + "]");
            }
            finally
            {
                fGridDetail.BackColor1 = System.Drawing.SystemColors.ButtonFace;
                fGridDetail.BackColorBkg = System.Drawing.SystemColors.ButtonFace;

                fGridDetail.AutoRedraw = true;
                fGridDetail.Refresh();
                FlashScreen.Close();
            }

        }

    }
}