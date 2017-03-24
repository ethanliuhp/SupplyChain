using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Approval.AppProcessMng.Domain;
using Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Domain;
using Application.Business.Erp.SupplyChain.Approval.AppTableSetMng.Domain;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatformUI;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Util;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using NHibernate.Criterion;
using VirtualMachine.Core;

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppProcessUI
{
    public partial class VAppStatusQueryNew : TBasicDataView
    {
        private string rootSyscode;
        private MAppPlatform Model = new MAppPlatform();
        private List<ApproveBill> bills;

        public VAppStatusQueryNew()
        {
            InitializeComponent();

            InitEvents();

            InitData();
        }

        private void GetRootSyscode()
        {
            if (!string.IsNullOrEmpty(rootSyscode))
            {
                return;
            }

            var proj = StaticMethod.GetProjectInfo();
            if(proj != null && proj.Code != CommonUtil.CompanyProjectCode)
            {
                rootSyscode = proj.OwnerOrgSysCode;
                return;
            }

            var subCompany = StaticMethod.GetSubCompanyOrgInfo();
            if (subCompany != null)
            {
                rootSyscode = subCompany.SysCode;
            }
            else if (ConstObject.TheOperationOrg.SysCode.StartsWith(TransUtil.CompanyOpgSyscode))
            {
                rootSyscode = TransUtil.CompanyOpgSyscode;
            }
            else
            {
                rootSyscode = ConstObject.TheOperationOrg.SysCode;
            }
        }

        private void InitData()
        {
            datePeriodPicker1.BeginValue = DateTime.Now.Date.AddMonths(-1);

            GetRootSyscode();

            GetAppSets();
        }

        private void GetAppSets()
        {
            dgvTypes.Rows.Clear();
            dgvBills.Rows.Clear();
            dgvSteps.Rows.Clear();

            var list = Model.Service.GetApprovingSets(txtSearchType.Text.Trim(), rootSyscode);
            foreach (AppTableSet set in list)
            {
                var rIndex = dgvTypes.Rows.Add(1);
                dgvTypes.Rows[rIndex].Tag = set;
                dgvTypes.Rows[rIndex].Cells[0].Value = set.TableName;
            }
        }

        private void ShowBills(List<ApproveBill> bls)
        {
            dgvBills.Rows.Clear();
            dgvSteps.Rows.Clear();
            if (bls == null)
            {
                return;
            }

            foreach (var bill in bls)
            {
                var rIndex = dgvBills.Rows.Add(1);
                var colIndex = 0;
                dgvBills.Rows[rIndex].Tag = bill;
                dgvBills.Rows[rIndex].Cells[colIndex++].Value = bill.BillCode;
                dgvBills.Rows[rIndex].Cells[colIndex++].Value = bill.IsDone ? "已审结" : "审批中";
                dgvBills.Rows[rIndex].Cells[colIndex++].Value = bill.IsDone ? "-" : bill.NextStep.ToString();
                dgvBills.Rows[rIndex].Cells[colIndex++].Value = bill.AppSolutionName;
                dgvBills.Rows[rIndex].Cells[colIndex++].Value = bill.LastModifTime;
                dgvBills.Rows[rIndex].Cells[colIndex++].Value = bill.BillCreatePersonName;
                dgvBills.Rows[rIndex].Cells[colIndex++].Value = bill.BillCreateDate.ToShortDateString();
                dgvBills.Rows[rIndex].Cells[colIndex].Value = bill.ProjectName;
            }
        }

        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            txtSearchType.KeyUp += new KeyEventHandler(txtSearchType_KeyUp);
            dgvTypes.CellClick += new DataGridViewCellEventHandler(dgvTypes_CellClick);
            dgvBills.CellClick += new DataGridViewCellEventHandler(dgvBills_CellClick);
            chkComplete.CheckedChanged += new EventHandler(chkComplete_CheckedChanged);
        }

        void chkComplete_CheckedChanged(object sender, EventArgs e)
        {
            if(chkComplete.Checked)
            {
                ShowBills(bills);
            }
            else
            {
                ShowBills(bills.FindAll(a => !a.IsDone));
            }
        }

        void dgvBills_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dgvBills.SelectedRows.Count==0)
            {
                MessageBox.Show("请选择单据");
                return;
            }

            var bl = dgvBills.SelectedRows[0].Tag as ApproveBill;
            if(bl == null)
            {
                MessageBox.Show("获取单据信息失败");
                return;
            }

            var stepDef = Model.Service.GetDefineStepsBySoulution(bl.AppSolution.Id).OfType<AppStepsSet>().ToList();
            var appSteps = Model.Service.GetAppStepsInfoByBill(bl);

            dgvSteps.Rows.Clear();
            foreach (var stepsSet in stepDef)
            {
                var rIndex = dgvSteps.Rows.Add(1);
                var colIndex = 0;
                dgvSteps.Rows[rIndex].Cells[colIndex++].Value = stepsSet.StepOrder;
                dgvSteps.Rows[rIndex].Cells[colIndex++].Value = stepsSet.StepsName;

                var relation = stepsSet.AppRelations == 0 ? " 或 " : " 和 ";
                dgvSteps.Rows[rIndex].Cells[colIndex++].Value =
                        string.Join(relation, stepsSet.AppRoleSets.Select(a => a.RoleName).ToArray());

                var approvedSteps = appSteps.FindAll(a => a.StepOrder == stepsSet.StepOrder);
                if(approvedSteps.Count > 0)
                {
                    var appPersons = new List<string>();
                    approvedSteps.ForEach(a => appPersons.Add(a.RoleName + "：" + a.AuditPerson.Name));
                    dgvSteps.Rows[rIndex].Cells[colIndex++].Value = string.Join("；", appPersons.ToArray());
                    dgvSteps.Rows[rIndex].Cells[colIndex++].Value = stepsSet.StepOrder == bl.NextStep ? "审核中" : "已审核";
                    dgvSteps.Rows[rIndex].Cells[colIndex].Value = approvedSteps.Max(a => a.AppDate);
                }
                else
                {
                    dgvSteps.Rows[rIndex].Cells[colIndex++].Value = string.Empty;
                    dgvSteps.Rows[rIndex].Cells[colIndex++].Value = stepsSet.StepOrder == bl.NextStep ? "审核中" : "<待审批>";
                    dgvSteps.Rows[rIndex].Cells[colIndex].Value = string.Empty;
                }
            }
        }

        void dgvTypes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnQuery_Click(btnQuery, null);
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            if(dgvTypes.SelectedRows.Count == 0 )
            {
                MessageBox.Show("请选择审批类型");
                return;
            }
            var set = dgvTypes.SelectedRows[0].Tag as AppTableSet;
            if(set == null)
            {
                MessageBox.Show("获取审批类型失败");
                return;
            }

            bills = Model.Service.GetApproveBillByType(
                set.Id, txtBillCode.Text.Trim(),
                rootSyscode, datePeriodPicker1.BeginValue,
                datePeriodPicker1.EndValue).OfType<ApproveBill>().ToList();

            chkComplete_CheckedChanged(chkComplete, e);
        }

        void txtSearchType_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                GetAppSets();
            }
        }
    }
}
