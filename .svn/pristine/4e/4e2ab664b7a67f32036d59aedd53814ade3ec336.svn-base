using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VDatePeriodDefineSelector : Form
    {
        private MFinanceMultData mOperate;
        private List<DatePeriodDefine> allDefine;

        public VDatePeriodDefineSelector()
        {
            InitializeComponent();

            InitEvents();
        }

        public List<DatePeriodDefine> SelectedPeriods { get; set; }

        private void InitData()
        {
            mOperate = new MFinanceMultData();

            var years = new List<KeyValuePair<int, string>>();
            for (var y = 2010; y <= DateTime.Now.Year + 10; y++)
            {
                years.Add(new KeyValuePair<int, string>(y, string.Concat(y, "年")));
            }

            chkYears.DataSource = years;
            chkYears.DisplayMember = "Value";
        }

        private void InitEvents()
        {
            dgMaster.BorderStyle = BorderStyle.FixedSingle;
            dgMaster.AutoGenerateColumns = false;

            chkYears.SelectedIndexChanged += chkYears_SelectedIndexChanged;
            chkMonth.CheckedChanged += chkFilter_CheckedChanged;
            chkQuarter.CheckedChanged += chkFilter_CheckedChanged;
            chkWeek.CheckedChanged += chkFilter_CheckedChanged;
            chkSelectAll.CheckedChanged += chkSelectAll_CheckedChanged;

            btnOk.Click += btnOk_Click;
            btnClose.Click += btnClose_Click;

            this.Load += VDatePeriodDefineSelector_Load;
        }

        private void SetGridRowChecked(bool isChecked)
        {
            for (int i = 0; i < dgMaster.Rows.Count; i++)
            {
                dgMaster.Rows[i].Cells[colCheck.Name].Value = isChecked;
            }
        }

        private void VDatePeriodDefineSelector_Load(object sender, EventArgs e)
        {
            InitData();
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            SetGridRowChecked(chkSelectAll.Checked);
        }

        private void chkFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (allDefine == null)
            {
                return;
            }
            var list = allDefine;
            if (!chkMonth.Checked)
            {
                list = list.FindAll(d => d.PeriodType != PeriodType.Month);
            }
            if (!chkQuarter.Checked)
            {
                list = list.FindAll(d => d.PeriodType != PeriodType.Quarter);
            }
            if (!chkWeek.Checked)
            {
                list = list.FindAll(d => d.PeriodType != PeriodType.Week);
            }

            dgMaster.DataSource = list;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SelectedPeriods = new List<DatePeriodDefine>();
            for (int i = 0; i < dgMaster.Rows.Count; i++)
            {
                if (dgMaster.Rows[i].Cells[colCheck.Name].Value == null)
                {
                    continue;
                }

                var vl = (bool) dgMaster.Rows[i].Cells[colCheck.Name].Value;
                if(vl)
                {
                    SelectedPeriods.Add(dgMaster.Rows[i].DataBoundItem as DatePeriodDefine);
                }
            }

            this.DialogResult = DialogResult.OK;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void chkYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            allDefine = new List<DatePeriodDefine>();
            foreach (var item in chkYears.CheckedItems)
            {
                var year = ((KeyValuePair<int, string>)item).Key;
                var list =
                    mOperate.FinanceMultDataSrv.GetDatePeriodDefineByYear(year, true).OfType<DatePeriodDefine>().ToList();
                allDefine.AddRange(list);
            }

            if (allDefine != null)
            {
                allDefine = allDefine.FindAll(a => a.PeriodType == PeriodType.Month);
            }

            dgMaster.DataSource = allDefine;
            SetGridRowChecked(true);
        }
    }
}
