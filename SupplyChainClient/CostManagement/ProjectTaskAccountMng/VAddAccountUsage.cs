using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Linq;

using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Main;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonForm;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng
{
    public partial class VAddAccountUsage : TBasicDataView
    {
        public MProjectTaskAccount model = new MProjectTaskAccount();
        public List<GWBSDetailCostSubject> listAddDtlUsage = new List<GWBSDetailCostSubject>();

        public bool IsOK = false;

        /// <summary>
        /// 操作的工程任务明细核算
        /// </summary>
        public ProjectTaskDetailAccount optAccountDtl = null;

        public VAddAccountUsage(ProjectTaskDetailAccount dtl)
        {
            optAccountDtl = dtl;

            InitializeComponent();

            InitForm();
        }
        public VAddAccountUsage(ref ProjectTaskDetailAccount dtl)
        {
            optAccountDtl = dtl;

            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();

            LoadAccountSubjectByAccountDtl();

            RefreshControls(MainViewState.Browser);
        }
        private void InitEvents()
        {
            linkAllSel.Click += new EventHandler(linkAllSel_Click);
            linkReverseSel.Click += new EventHandler(linkReverseSel_Click);
            btnEnter.Click += new EventHandler(btnEnter_Click);
            btnExit.Click += new EventHandler(btnExit_Click);
            this.FormClosing += new FormClosingEventHandler(VAddAccountUsage_FormClosing);
        }

        void VAddAccountUsage_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsOK == false)
                listAddDtlUsage.Clear();
        }

        void linkAllSel_Click(object sender, EventArgs e)
        {
            bool selFlag = linkAllSel.Checked;

            foreach (DataGridViewRow row in gridDtlUsage.Rows)
            {
                row.Cells[usageSelect.Name].Value = selFlag;
            }
        }

        void linkReverseSel_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in gridDtlUsage.Rows)
            {
                row.Cells[usageSelect.Name].Value = !Convert.ToBoolean(row.Cells[usageSelect.Name].Value);
            }
        }

        void btnEnter_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in gridDtlUsage.Rows)
            {
                if (Convert.ToBoolean(row.Cells[usageSelect.Name].Value))
                {
                    GWBSDetailCostSubject dtlUsage = row.Tag as GWBSDetailCostSubject;
                    listAddDtlUsage.Add(dtlUsage);
                }
            }

            if (listAddDtlUsage.Count == 0)
            {
                MessageBox.Show("请选择耗用资源！");
                return;
            }

            IsOK = true;

            this.Close();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            listAddDtlUsage.Clear();

            IsOK = false;

            this.Close();
        }

        private void LoadAccountSubjectByAccountDtl()
        {
            try
            {
                if (optAccountDtl == null)
                    return;

                txtDtlName.Text = optAccountDtl.ProjectTaskDtlName;
                txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), optAccountDtl.AccountTaskNodeName, optAccountDtl.AccountTaskNodeSyscode);

                if (optAccountDtl.ProjectTaskDtlGUID == null)
                    return;

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheGWBSDetail.Id", optAccountDtl.ProjectTaskDtlGUID.Id));
                oq.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("ProjectAmountUnitGUID", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("TheGWBSDetail", NHibernate.FetchMode.Eager);

                IList listDtl = model.ObjectQuery(typeof(GWBSDetailCostSubject), oq);
                if (listDtl == null || listDtl.Count == 0)
                    return;

                GWBSDetailCostSubject optDtl = listDtl[0] as GWBSDetailCostSubject;
                txtPlanQuantity.Text = optDtl.TheGWBSDetail.PlanWorkAmount.ToString();

                foreach (GWBSDetailCostSubject subject in listDtl)
                {
                    AddDetailUsageInfoInGrid(subject, false);
                }
                gridDtlUsage.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载数据出错，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        private void AddDetailUsageInfoInGrid(GWBSDetailCostSubject dtl, bool isSelected)
        {
            int index = gridDtlUsage.Rows.Add();
            DataGridViewRow row = gridDtlUsage.Rows[index];

            row.Cells[usageName.Name].Value = dtl.Name;
            row.Cells[usageResourceType.Name].Value = dtl.ResourceTypeName;
            row.Cells[usageSpec.Name].Value = dtl.ResourceTypeSpec;
            row.Cells[usageDiagramNumber.Name].Value = dtl.DiagramNumber;
            row.Cells[usageIsMainResource.Name].Value = dtl.MainResTypeFlag ? "是" : "否";
            row.Cells[usageAccountSubject.Name].Value = dtl.CostAccountSubjectName;

            row.Cells[usageProjectQuantityUnit.Name].Value = dtl.ProjectAmountUnitName;
            row.Cells[usageContractQuotaNum.Name].Value = dtl.ContractQuotaQuantity;
            row.Cells[usageContractQnyPriceResult.Name].Value = dtl.ContractQuantityPrice;
            row.Cells[usageContractWorkAmount.Name].Value = dtl.ContractProjectAmount;
            row.Cells[usageContractTotalPrice.Name].Value = dtl.ContractTotalPrice;

            row.Cells[usageResponsibleQuotaNum.Name].Value = dtl.ResponsibleQuotaNum;
            row.Cells[usageResponsibleQnyPriceResult.Name].Value = dtl.ResponsibilitilyPrice;
            row.Cells[usageResponsibleQuantity.Name].Value = dtl.ResponsibilitilyWorkAmount;
            row.Cells[usageResponsibleTotalPrice.Name].Value = dtl.ResponsibilitilyTotalPrice;

            row.Cells[usagePlanQuotaNum.Name].Value = dtl.PlanQuotaNum;
            row.Cells[usagePlanQnyPriceResult.Name].Value = dtl.PlanPrice;
            row.Cells[usagePlanQuantity.Name].Value = dtl.PlanWorkAmount;
            row.Cells[usagePlanTotalPrice.Name].Value = dtl.PlanTotalPrice;


            row.Tag = dtl;

            if (isSelected)
            {
                row.Cells[usageSelect.Name].Value = true;
            }
        }
    }
}
