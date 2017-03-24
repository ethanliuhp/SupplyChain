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
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using NHibernate.Criterion;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using System.Collections;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Resource.MaterialResource.Domain;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    public partial class VCostMonthAcctGenQuery : TBasicDataView
    {
        private MCostMonthAccount model = new MCostMonthAccount();
        private int baseYear = 1990;
        private int baseStartMonth = 1;
        private int baseEndMonth = 1;
        public VCostMonthAcctGenQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }
        private void InitData()
        {
            //时间下拉框赋值
            List<int> startYear = new List<int>();//开始年
            for (int i = baseYear; i < baseYear + 100; i++)
            {
                startYear.Add(i);
            }
            this.cmoYear.DataSource = startYear;//开始月
            this.cmoYear.Text = ConstObject.LoginDate.Year.ToString();

            List<int> startMonth = new List<int>();
            for (int i = baseStartMonth; i < baseStartMonth + 12; i++)
            {
                startMonth.Add(i);
            }
            List<int> endYear = new List<int>();//结束年
            for (int i = baseYear; i < baseYear + 100; i++)
            {
                endYear.Add(i);
            }
            this.cmoEndYear.DataSource = endYear;
            this.cmoEndYear.Text = ConstObject.LoginDate.Year.ToString();

            List<int> endMonth = new List<int>();//结束月
            for (int i = baseEndMonth; i < baseEndMonth + 12; i++)
            {
                endMonth.Add(i);
            }
            this.cmoEndMonth.DataSource = endMonth;
            this.cmoEndMonth.Text = ConstObject.LoginDate.Month.ToString();
            this.cmoStartMonth.DataSource = startMonth;
            this.cmoStartMonth.Text = (ClientUtil.ToInt(cmoEndMonth.Text) - 1).ToString();

            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null)
            {
                txtPrjName.Tag = projectInfo;
                txtPrjName.Text = projectInfo.Name;
            }
        }

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnSelSubject.Click += new EventHandler(btnSelSubject_Click);
            this.btnSearchGWBS.Click += new EventHandler(btnSearchGWBS_Click);
            this.btnSearchPrj.Click += new EventHandler(btnSearchPrj_Click);
            this.btnClear.Click += new EventHandler(btnClear_Click);
        }
        void btnClear_Click(object sender, EventArgs e)
        {
            this.txtSubject.Text = "";
            this.txtSubject.Tag = null;
            this.txtGWBS.Text = "";
            this.txtGWBS.Tag = null;
            this.txtMaterialCategory.Text = "";
            this.txtMaterial.Text = "";
        }
        //核算科目选择按钮事件
        void btnSelSubject_Click(object sender, EventArgs e)
        {
            VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
            frm.IsLeafSelect = false;
            frm.ShowDialog();
            CostAccountSubject cost = frm.SelectAccountSubject;
            if (cost != null)
            {
                this.txtSubject.Text = cost.Name;
                this.txtSubject.Tag = cost;
            }
        }
        //项目选择按钮事件
        void btnSearchPrj_Click(object sender, EventArgs e)
        {
            VSelectProjectInfo frm = new VSelectProjectInfo();
            frm.ShowDialog();

            if (frm.Result != null && frm.Result.Count > 0)
            {
                CurrentProjectInfo selectProject = frm.Result[0] as CurrentProjectInfo;
                if (selectProject != null)
                {
                    this.txtPrjName.Text = selectProject.Name;
                }
            }

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
        //Excel按钮事件
        void btnExcel_Click(object sender, EventArgs e)
        {
            if (this.tabCostReport.SelectedTab.Name.Equals("tabMonthData"))
            {
                Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgConsume, true);
            }
            else if (tabCostReport.SelectedTab.Name.Equals("tabBillData"))
            {
                Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgBill, true);
            }        
        }

        //查询按钮事件
        void btnSearch_Click(object sender, EventArgs e)
        {
            ////验证结束月与起始月
            //if (ClientUtil.ToInt(cmoYear.Text) * 100 + ClientUtil.ToInt(cmoStartMonth.Text) >= ClientUtil.ToInt(this.cmoEndYear.Text) * 100 + ClientUtil.ToInt(this.cmoEndMonth.Text))
            //{
            //    MessageBox.Show("起始年月必须小于结束年月，请检查！");
            //    return;
            //}
            #region 查询条件处理
            string condition = "";
            string materialGuid = "";
            string materialCatGuid = "";
            string subjectGuid = "";
            string taskGuid = "";
            CurrentProjectInfo projectInfo = new CurrentProjectInfo();
            projectInfo = StaticMethod.GetProjectInfo();
            condition += "and t1.theprojectguid = '" + projectInfo.Id + "'";
            ////起始年月
            //if (this.cmoYear.SelectedItem != null)
            //{
            //    condition += "and t1.kjn*100+t1.kjy >= " + (ClientUtil.ToInt(cmoYear.Text) * 100 + ClientUtil.ToInt(cmoStartMonth.Text));
            //}

            ////结束年月
            //if (this.cmoEndYear.SelectedItem != null)
            //{
            //    condition += "and t1.kjn*100+t1.kjy <= " + (ClientUtil.ToInt(this.cmoEndYear.Text) * 100 + ClientUtil.ToInt(this.cmoEndMonth.Text));
            //}
            // 物资

            //会计年月
            if (this.cmoYear.SelectedItem != null)
            {
                condition += "and t1.kjn= " + ClientUtil.ToInt(cmoYear.Text) + " and t1.kjy = " + ClientUtil.ToInt(cmoStartMonth.Text);
            }
            if (this.txtMaterial.Text != "")
            {
                Material material = this.txtMaterial.Result[0] as Material;
                materialGuid = material.Id;
                condition += "and t3.resourcetypeguid = '" + materialGuid + "'";
            }
            // GWBS
            if (this.txtGWBS.Text != "")
            {
                taskGuid = (txtGWBS.Tag as GWBSTree).Id;
                condition += " and t2.accounttasknodesyscode like '%" + taskGuid + "%'";

            }
            // 科目
            if (this.txtSubject.Text != "")
            {
                subjectGuid = (txtSubject.Tag as CostAccountSubject).Id;
                //condition += "and t3.costsubjectsyscode like '%" + subjectGuid + "%'";
                string subjectCode = (txtSubject.Tag as CostAccountSubject).Code;
                condition += " and t3.costsubjectcode like '%" + subjectCode + "%'";

            }
            // 资源分类
            if (this.txtMaterialCategory.Text != "")
            {
                MaterialCategory materialCategory = txtMaterialCategory.Result[0] as MaterialCategory;
                materialCatGuid = materialCategory.Id;
                condition += " and t3.resourcesyscode like '%" + materialCatGuid + "%'";

            }
            #endregion

            #region 本月成本核算信息
            decimal sumContractMoney = 0;
            decimal sumResponseMoney = 0;
            decimal sumPlanMoney = 0;
            decimal sumRealMoney = 0;
            DataSet dataSet = model.CostMonthAccSrv.CostMonthAcctGeneralQuery(condition);
            this.dgConsume.Rows.Clear();

            DataTable dataTable = dataSet.Tables[0];
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgConsume.Rows.Add();
                dgConsume[this.colUsePart.Name, rowIndex].Value = ClientUtil.ToString(dataRow["accounttasknodename"]);
                dgConsume[this.colRationUnitName.Name, rowIndex].Value = ClientUtil.ToString(dataRow["rationunitname"]);
                dgConsume[this.colCostingSubjectName.Name, rowIndex].Value = ClientUtil.ToString(dataRow["costingsubjectname"]);
                dgConsume[this.colResourceTypeName.Name, rowIndex].Value = ClientUtil.ToString(dataRow["resourcetypename"]);
                dgConsume[this.colSpec.Name, rowIndex].Value = ClientUtil.ToString(dataRow["resourcetypespec"]);
                dgConsume[this.colCurrRealConsumeQty1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["currrealconsumequantity"]);
                dgConsume[this.colCurrRealConsumeTPrice1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["currrealconsumetotalprice"]);
                sumRealMoney += ClientUtil.ToDecimal(dataRow["currrealconsumetotalprice"]);
                dgConsume[this.colCurrRealConsumePlanQty1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["currrealconsumeplanquantity"]);
                dgConsume[this.colCurrRealConsumePlanTPrice1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["currrealconsumeplantotalprice"]);
                sumPlanMoney += ClientUtil.ToDecimal(dataRow["currrealconsumeplantotalprice"]);
                dgConsume[this.colCurrIncomeQty1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["currincomequantity"]);
                dgConsume[this.colCurrIncomeTotalPrice1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["currincometotalprice"]);
                sumContractMoney += ClientUtil.ToDecimal(dataRow["currincometotalprice"]);
                dgConsume[this.colCurrResponsiConsumeQty1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["currresponsiconsumequantity"]);
                dgConsume[this.colCurrResConsumeTotalPrice1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["currresponsiconsumetotalprice"]);
                sumResponseMoney += ClientUtil.ToDecimal(dataRow["currresponsiconsumetotalprice"]);
                dgConsume[this.colSumRealConsumeQty1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["sumrealconsumequantity"]);
                dgConsume[this.colSumRealConsumeTotalPrice1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["sumrealconsumetotalprice"]);
                dgConsume[this.colSumRealConsumePlanQty1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["sumrealconsumeplanquantity"]);
                dgConsume[this.colSumRealConsumePlanTPrice1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["sumrealconsumeplantotalprice"]);
                dgConsume[this.colSumIncomeQuantity1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["sumincomequantity"]);
                dgConsume[this.colSumIncomeTotalPrice1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["sumincometotalprice"]);
                dgConsume[this.colSumResponsiConsumeQy1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["sumresponsiconsumequantity"]);
                dgConsume[this.colSumResponsiConsumeTPrice1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["sumresponsiconsumetotalprice"]);
            }
            this.dgConsume.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            #endregion

            #region 汇总原始单据信息

            
            DataSet billDateSet = model.CostMonthAccSrv.QueryGenCostMonthInfo(projectInfo.Id, ClientUtil.ToInt(cmoYear.Text),
                ClientUtil.ToInt(cmoStartMonth.Text), materialGuid, materialCatGuid, subjectGuid, taskGuid);
            this.dgBill.Rows.Clear();

            DataTable billDataTable = billDateSet.Tables[0];
            foreach (DataRow dataRow in billDataTable.Rows)
            {
                int rowIndex = this.dgBill.Rows.Add();
                dgBill[this.colResourceName1.Name, rowIndex].Value = ClientUtil.ToString(dataRow["resourcetypename"]);
                dgBill[this.colResourceSpec1.Name, rowIndex].Value = ClientUtil.ToString(dataRow["resourcetypespec"]);
                dgBill[this.colResourceStuff1.Name, rowIndex].Value = ClientUtil.ToString(dataRow["resourcetypequality"]);
                dgBill[this.colAccSubject1.Name, rowIndex].Value = ClientUtil.ToString(dataRow["bestaetigtcostsubjectname"]);
                dgBill[this.colTaskName1.Name, rowIndex].Value = ClientUtil.ToString(dataRow["accounttasknodename"]);
                dgBill[this.colQuantity1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["realqty"]);
                dgBill[this.colMoney1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["realmoney"]);
                
                dgBill[this.colContractQty1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["currcontractincomeqny"]);
                dgBill[this.colContractMny1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["currcontractincometotal"]);
                
                dgBill[this.colResponQty1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["currresponsiblecostqny"]);
                dgBill[this.colResponMny1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["currresponsiblecosttotal"]);
                
                dgBill[this.colPlanQty1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["accusageqny"]);
                dgBill[this.colPlanMny1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["accounttotalprice"]);
                
                dgBill[this.colBillCode1.Name, rowIndex].Value = ClientUtil.ToString(dataRow["code"]);
                dgBill[this.colBillType1.Name, rowIndex].Value = ClientUtil.ToString(dataRow["billtype"]);
            }
            this.dgBill.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            this.txtSumRealMoney.Text = sumRealMoney.ToString("#,###.####");
            this.txtSumContractMny.Text = sumContractMoney.ToString("#,###.####");
            this.txtSumRespMoney.Text = sumResponseMoney.ToString("#,###.####");
            this.txtSumPlanMoney.Text = sumPlanMoney.ToString("#,###.####");
            #endregion
        }
    }
}
