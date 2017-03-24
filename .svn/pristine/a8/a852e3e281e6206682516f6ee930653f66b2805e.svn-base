using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Collections;
using VirtualMachine.Core;
using NHibernate;
using NHibernate.Criterion;
using Application.Business.Erp.Financial.Client.FinanceUI.InitialSetting.AccountTitleUI;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VAccountLock : TBasicDataView
    {
        private MFinanceMultData model = new MFinanceMultData();

        private VAccountLock()
        {
            InitializeComponent();

            InitEvents();
        }

        public VAccountLock(FinanceMultDataExecType eType)
            : this()
        {
            InitOrg();
        }

        private void InitOrg()
        {
            if (ConstObject.TheOperationOrg != null)
            {
                txtOperationOrg.Text = ConstObject.TheOperationOrg.Name;
                txtOperationOrg.Tag = ConstObject.TheOperationOrg;
            }

            if (ConstObject.TheLogin != null)
            {
                ucAccountPeriodCombox1.SetDefaultMonth(ConstObject.TheLogin.LoginDate.Month - 1);
            }
        }

        private void InitEvents()
        {
            ucAccountPeriodCombox1.AfterSelectedAccountPeriodEvnt += AfterSelectPeriod;

            btnLock.Click += btnLock_Click;
            btnUnLock.Click += btnUnLock_Click;
            btnOperationOrg.Click += btnOperationOrg_Click;
            btnQuery.Click += btnQuery_Click;

            chkAll.CheckedChanged += chkAll_CheckedChanged;
            chkOther.CheckedChanged += chkOther_CheckedChanged;
            //chkUnLocked.CheckedChanged += chkUnLocked_CheckedChanged;
            //chkHasLocked.CheckedChanged += chkHasLocked_CheckedChanged;
        }

        private void Query(bool isShowMes)
        {
            if (ucAccountPeriodCombox1.SelectedPeriod == null)
            {
                if (isShowMes)
                {
                    MessageBox.Show("请选择会计期间");
                    ucAccountPeriodCombox1.Focus();
                }
                return;
            }

            var org = txtOperationOrg.Tag as OperationOrgInfo;
            if (org == null)
            {
                if (isShowMes)
                {
                    MessageBox.Show("请选择查询范围");
                    btnOperationOrg.Focus();
                }
                return;
            }

            try
            {
                FlashScreen.Show("正在查询，请稍候...");

                ObjectQuery objQuery = new ObjectQuery();
                objQuery.AddCriterion(Expression.Like("OwnerOrgSysCode", org.SysCode, MatchMode.Start));
                //objQuery.AddCriterion(Expression.Eq("ProjectCurrState", 0));
                objQuery.AddCriterion(Expression.Sql(" nvl(projectcurrstate,0) != 20 "));

                var projList = model.FinanceMultDataSrv.Query(typeof(CurrentProjectInfo), objQuery).OfType<CurrentProjectInfo>().ToList();
                if (projList == null || projList.Count == 0)
                {
                    dgMaster.Rows.Clear();
                    return;
                }

                var pIdList = new List<string>();
                projList.ForEach(p => pIdList.Add(p.Id));

                objQuery = new ObjectQuery();
                objQuery.AddCriterion(Expression.Eq("AccountYear", ucAccountPeriodCombox1.SelectedPeriod.AccountYear));
                objQuery.AddCriterion(Expression.Eq("AccountMonth", ucAccountPeriodCombox1.SelectedPeriod.AccountMonth));
                objQuery.AddCriterion(Expression.In("ProjectId", pIdList));
                var lockList = model.FinanceMultDataSrv.Query(typeof(LockAccountMaster), objQuery).OfType<LockAccountMaster>().ToList();
                if (lockList == null)
                {
                    lockList = new List<LockAccountMaster>();
                }

                var q = from prj in projList
                        join lk in lockList on prj.Id equals lk.ProjectId into tmp
                        from tt in tmp.DefaultIfEmpty()
                        orderby prj.Name
                        select new
                        {
                            ProjectOrg = prj.OwnerOrgName,
                            ProjectName = prj.Name,
                            ProjectId = prj.Id,
                            Status = tt == null ? "未结账" : "已结账",
                            LockId = tt == null ? null : tt.Id,
                            EndDate = tt == null ? ucAccountPeriodCombox1.SelectedPeriod.EndDate.ToShortDateString() : tt.AccountEndDate.ToShortDateString()
                        };

                var qList = q.ToList();
                if (chkHasLocked.Checked && !chkUnLocked.Checked)
                {
                    qList = qList.FindAll(a => a.Status.Equals("已结账"));
                }
                else if (!chkHasLocked.Checked && chkUnLocked.Checked)
                {
                    qList = qList.FindAll(a => a.Status.Equals("未结账"));
                }
                else if (!chkHasLocked.Checked && !chkUnLocked.Checked)
                {
                    dgMaster.Rows.Clear();
                    return;
                }

                var rowIndex = 0;
                dgMaster.Rows.Clear();
                foreach (var item in qList)
                {
                    rowIndex = dgMaster.Rows.Add();

                    dgMaster.Rows[rowIndex].Tag = item;
                    dgMaster[colRowNo.Name, rowIndex].Value = rowIndex + 1;
                    dgMaster[colCheckProject.Name, rowIndex].Value = false;
                    dgMaster[colProjectName.Name, rowIndex].Value = item.ProjectName;
                    dgMaster[colProjectOrg.Name, rowIndex].Value = item.ProjectOrg;
                    dgMaster[colState.Name, rowIndex].Value = item.Status;
                    dgMaster[colPeriodEndDate.Name, rowIndex].Value = item.EndDate;

                    var style = new DataGridViewCellStyle();
                    if (item.Status.Equals("已结账"))
                    {
                        style.ForeColor = Color.Red;
                    }
                    else
                    {
                        style.ForeColor = Color.Green;
                    }

                    dgMaster[colState.Name, rowIndex].Style = style;
                }

                chkAll.Checked = chkOther.Checked = false;
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        private void AfterSelectPeriod(object sender)
        {
            Query(false);
        }

        private List<LockAccountMaster> GetCheckedRows(bool isLock)
        {
            if (dgMaster.RowCount == 0)
            {
                return null;
            }

            var lst = new List<LockAccountMaster>();
            for (var i = 0; i < dgMaster.Rows.Count; i++)
            {
                var isChk = Convert.ToBoolean(dgMaster[0, i].Value);
                if (!isChk)
                {
                    continue;
                }

                if (isLock && dgMaster[colState.Name, i].Value.ToString().Equals("未结账"))
                {
                    continue;
                }
                else if (!isLock && dgMaster[colState.Name, i].Value.ToString().Equals("已结账"))
                {
                    continue;
                }

                var item = dgMaster.Rows[i].Tag;

                var la = new LockAccountMaster();
                la.OperOrgInfo = ConstObject.TheOperationOrg;
                la.OperOrgInfoName = ConstObject.TheOperationOrg.Name;
                la.OpgSysCode = ConstObject.TheOperationOrg.SysCode;
                la.CreateDate = DateTime.Now;
                la.CreatePerson = ConstObject.TheLogin.ThePerson;
                la.CreatePersonName = ConstObject.TheLogin.ThePerson.Name;
                la.AccountYear = ucAccountPeriodCombox1.SelectedPeriod.AccountYear;
                la.AccountMonth = ucAccountPeriodCombox1.SelectedPeriod.AccountMonth;
                la.AccountEndDate = ucAccountPeriodCombox1.SelectedPeriod.EndDate;
                la.ProjectName = dgMaster[colProjectName.Name, i].Value.ToString();
                la.ProjectId = item.GetType().GetProperty("ProjectId").GetValue(item, null).ToString();

                var vl = item.GetType().GetProperty("LockId").GetValue(item, null);
                if (vl != null)
                {
                    la.Id = vl.ToString();
                }

                lst.Add(la);
            }

            return lst;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            Query(true);
        }

        private void btnLock_Click(object sender, EventArgs e)
        {
            var lst = GetCheckedRows(false);
            if (lst == null || lst.Count == 0)
            {
                MessageBox.Show("请选择未结账的记录");
                return;
            }

            var res = model.FinanceMultDataSrv.SaveAccountLockMaster(lst);
            if (string.IsNullOrEmpty(res))
            {
                MessageBox.Show("结账成功");
                Query(true);
            }
            else
            {
                MessageBox.Show(res);
            }
        }

        private void btnUnLock_Click(object sender, EventArgs e)
        {
            var lst = GetCheckedRows(true);
            if (lst == null || lst.Count == 0)
            {
                MessageBox.Show("请选择已结账的记录");
                return;
            }

            var res = model.FinanceMultDataSrv.DeleteAccountLockMaster(lst);
            if (string.IsNullOrEmpty(res))
            {
                MessageBox.Show("反结账成功");
                Query(true);
            }
            else
            {
                MessageBox.Show(res);
            }
        }

        private void btnOperationOrg_Click(object sender, EventArgs e)
        {
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect();
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                OperationOrgInfo info = frm.Result[0] as OperationOrgInfo;
                txtOperationOrg.Tag = info;
                txtOperationOrg.Text = info.Name;

                Query(false);
            }
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            for (var i = 0; i < dgMaster.RowCount; i++)
            {
                dgMaster[colCheckProject.Name, i].Value = chkAll.Checked;
            }
        }

        private void chkOther_CheckedChanged(object sender, EventArgs e)
        {
            for (var i = 0; i < dgMaster.RowCount; i++)
            {
                var vl = Convert.ToBoolean(dgMaster[colCheckProject.Name, i].Value);
                dgMaster[colCheckProject.Name, i].Value = !vl;
            }
        }

        private void chkHasLocked_CheckedChanged(object sender, EventArgs e)
        {
            Query(true);
        }

        private void chkUnLocked_CheckedChanged(object sender, EventArgs e)
        {
            Query(true);
        }
    }
}
