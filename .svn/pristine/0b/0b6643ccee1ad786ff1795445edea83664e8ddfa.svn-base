using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.PMCAndWarning.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.PMCAndWarning.MaterialWarning
{
    public partial class VMaterailWarningDetailQuery : Form
    {
        private string targetName = null;
        MPMCAndWarning _MPMCAndWarning = new MPMCAndWarning();
        CurrentProjectInfo pi = null;

        public VMaterailWarningDetailQuery(string targetName)
        {
            InitializeComponent();
            this.targetName = targetName;
            pi = StaticMethod.GetProjectInfo();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void VMaterailWarningDetailQuery_Load(object sender, EventArgs e)
        {
            try
            {
                ShowTabPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show("检索数据出错。\n"+ex.Message);
            }
        }

        private void LoadSupplyOrderWarningInfo()
        {
            dgSupplyOrder.Rows.Clear();
            DataTable dt = _MPMCAndWarning.PMCAndWarningSrv.GetSupplyOrderWarningInfoDetail(pi.Id);
            foreach (DataRow dr in dt.Rows)
            {
                int i = dgSupplyOrder.Rows.Add();
                dgSupplyOrder[colSPOrderCode.Name, i].Value = dr["code"] + "";
                dgSupplyOrder[colSPMatName.Name, i].Value = dr["materialname"] + "";
                dgSupplyOrder[colSPMatSpec.Name, i].Value = dr["materialspec"] + "";
                decimal jssl = ClientUtil.ToDecimal(dr["jssl"]);
                decimal htsl = ClientUtil.ToDecimal(dr["htsl"]);
                dgSupplyOrder[colSPOrderQuantity.Name, i].Value = htsl;
                dgSupplyOrder[colSPBalQuantity.Name, i].Value = jssl;
                dgSupplyOrder[colSPPercent.Name, i].Value = (jssl / htsl*100).ToString("####.##");
            }
            dgSupplyOrder.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void LoadDailyPlanWarningInfo()
        {
            dgDailyPlan.Rows.Clear();
            DataTable dt = _MPMCAndWarning.PMCAndWarningSrv.GetDailyPlanWarningInfoDetail(pi.Id);
            foreach (DataRow dr in dt.Rows)
            {
                int i = dgDailyPlan.Rows.Add();
                dgDailyPlan[colDPMatName.Name, i].Value = dr["materialname"] + "";
                dgDailyPlan[colDPMatSpec.Name, i].Value = dr["materialspec"] + "";
                decimal rjhsl = ClientUtil.ToDecimal(dr["rjhsl"]);
                decimal yjhsl = ClientUtil.ToDecimal(dr["yjhsl"]);
                dgDailyPlan[colDPMonthQty.Name, i].Value = yjhsl;
                dgDailyPlan[colDPDailyQty.Name, i].Value = rjhsl;
                dgDailyPlan[colDPPercent.Name, i].Value = (rjhsl / yjhsl * 100).ToString("####.##");
            }
            dgDailyPlan.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void LoadMonthPlanWarningInfo()
        {
            dgMonthPlan.Rows.Clear();
            DataTable dt = _MPMCAndWarning.PMCAndWarningSrv.GetMonthPlanWarningInfoDetail(pi.Id);
            foreach (DataRow dr in dt.Rows)
            {
                int i = dgMonthPlan.Rows.Add();
                dgMonthPlan[colMPMatName.Name, i].Value = dr["materialname"] + "";
                dgMonthPlan[colMPMatSpec.Name, i].Value = dr["materialspec"] + "";
                decimal zjhsl = ClientUtil.ToDecimal(dr["zjhsl"]);
                decimal yjhsl = ClientUtil.ToDecimal(dr["yjhsl"]);
                dgMonthPlan[colMPMonthQty.Name, i].Value = yjhsl;
                dgMonthPlan[colMPDemandQty.Name, i].Value = zjhsl;
                dgMonthPlan[colDPPercent.Name, i].Value = (yjhsl / zjhsl * 100).ToString("####.##");
            }
            dgMonthPlan.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void LoadStockInWarningInfo()
        {
            dgStockIn.Rows.Clear();
            DataTable dt = _MPMCAndWarning.PMCAndWarningSrv.GetStockInWarningInfoDetail(pi.Id);
            foreach (DataRow dr in dt.Rows)
            {
                int i = dgStockIn.Rows.Add();
                dgStockIn[colStockInMatName.Name, i].Value = dr["materialname"] + "";
                dgStockIn[colStockInMatSpec.Name, i].Value = dr["materialspec"] + "";
                decimal jhsl = ClientUtil.ToDecimal(dr["jhsl"]);
                decimal shsl = ClientUtil.ToDecimal(dr["shsl"]);
                dgStockIn[colStockInQuantity.Name, i].Value = shsl;
                dgStockIn[colStockInDailyPlanQty.Name, i].Value = jhsl;
                dgStockIn[colStockInPercent.Name, i].Value = (shsl / jhsl * 100).ToString("####.##");
            }
            dgStockIn.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void ShowTabPage()
        {
            tabControl1.TabPages.Clear();
            
            if (targetName == WarningTarget.WarningTarget_WZ_SupplyOrder)
            {                
                tabControl1.TabPages.Add(tpSupplyOrder);
                LoadSupplyOrderWarningInfo();
            }
            else if (targetName == WarningTarget.WarningTarget_WZ_DailyPlan)
            {
                tabControl1.TabPages.Add(tpDailyPlan);
                LoadDailyPlanWarningInfo();
            }
            else if (targetName == WarningTarget.WarningTarget_WZ_MonthPlan)
            {
                tabControl1.TabPages.Add(tpMonthPlan);
                LoadMonthPlanWarningInfo();
            }
            else if (targetName == WarningTarget.WarningTarget_WZ_StockIn)
            {
                tabControl1.TabPages.Add(tpStockIn);
                LoadStockInWarningInfo();
            }
        }
    }
}
