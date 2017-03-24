using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using NHibernate.Criterion;
using VirtualMachine.Core;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage
{
    public partial class VFundPlanSelector : Form
    {
        private CurrentProjectInfo currentProject;
        private List<ProjectFundPlanDetail> allPlans;
        private int planYear, planMonth;
        private string supplierName;

        public VFundPlanSelector(CurrentProjectInfo proj, int year, int mon, string spName)
        {
            InitializeComponent();

            currentProject = proj;
            planYear = year;
            planMonth = mon;
            supplierName = spName;

            InitEvents();

            InitData();
        }

        public ProjectFundPlanDetail SelectedPlan { get; set; }

        private void InitData()
        {
            var objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            objQuery.AddCriterion(Expression.Eq("CreateYear", planYear));
            objQuery.AddCriterion(Expression.Eq("CreateMonth", planMonth));
            
            if (currentProject != null)
            {
                objQuery.AddCriterion(Expression.Eq("ProjectId", currentProject.Id));
            }

            var list =
                new MFinanceMultData().FinanceMultDataSrv.GetProjectFundPlanMasterByOQ(objQuery).OfType
                    <ProjectFundPlanMaster>().ToList();
            allPlans = new List<ProjectFundPlanDetail>();
            foreach (var planMaster in list)
            {
                if(!string.IsNullOrEmpty(supplierName))
                {
                    var details = planMaster.Details.OfType<ProjectFundPlanDetail>().ToList()
                        .FindAll(a => a.CreditorUnitLeadingOfficial == supplierName);
                    allPlans.AddRange(details);
                }
                else
                {
                    allPlans.AddRange(planMaster.Details.OfType<ProjectFundPlanDetail>());
                }
            }

            dgMaster.DataSource = allPlans == null ? null : allPlans.ToArray();
        }

        private void InitEvents()
        {
            dgMaster.AutoGenerateColumns = false;
            dgMaster.MouseDoubleClick += new MouseEventHandler(dgMaster_MouseDoubleClick);

            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnOk.Click += new EventHandler(btnOk_Click);
            btnFind.Click += new EventHandler(btnFind_Click);

            txtKey.KeyUp += new KeyEventHandler(txtKey_KeyUp);
        }

        private void dgMaster_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btnOk_Click(null, null);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (dgMaster.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择项目");
                return;
            }

            SelectedPlan = dgMaster.SelectedRows[0].DataBoundItem as ProjectFundPlanDetail;
            this.DialogResult = DialogResult.OK;
        }

        private void txtKey_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnFind_Click(null, null);
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            var sKey = txtKey.Text.Trim();
            if (string.IsNullOrEmpty(sKey) || allPlans == null)
            {
                dgMaster.DataSource = allPlans == null ? null : allPlans.ToArray();
            }
            else
            {
                dgMaster.DataSource =
                    allPlans.FindAll(p => p.Master.ProjectName.Contains(sKey) || p.Master.Code.Contains(sKey)).ToArray();
            }
        }
    }
}
