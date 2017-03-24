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
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using NHibernate.Criterion;
using VirtualMachine.Core;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage
{
    public partial class VAccountSelector : Form
    {
        private List<AccountTitleTree> allAccounts;

        public VAccountSelector()
        {
            InitializeComponent();

            InitEvents();

            InitData();
        }

        public AccountTitleTree SelectedAccount { get; set; }

        private void InitData()
        {
            allAccounts =
                new MFinanceMultData().FinanceMultDataSrv.GetPayAccounts().OfType<AccountTitleTree>().ToList();

            DiaplayAccounts(allAccounts);
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

        private void DiaplayAccounts(List<AccountTitleTree> list)
        {
            dgMaster.Rows.Clear();
            if (list == null)
            {
                return;
            }

            foreach (var account in list)
            {
                var rIndex = dgMaster.Rows.Add(1);

                dgMaster.Rows[rIndex].Tag = account;
                dgMaster.Rows[rIndex].Cells["colCode"].Value = account.Code.PadLeft(account.Code.Length + account.Level, ' ');
                dgMaster.Rows[rIndex].Cells["colName"].Value = account.Name.PadLeft(account.Name.Length + account.Level, ' ');
            }
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

            SelectedAccount = dgMaster.SelectedRows[0].Tag as AccountTitleTree;
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
            if (string.IsNullOrEmpty(sKey) || allAccounts == null)
            {
                DiaplayAccounts(allAccounts);
            }
            else
            {
                DiaplayAccounts(allAccounts.FindAll(p => p.Code.Contains(sKey) || p.Name.Contains(sKey)));
            }
        }
    }
}
