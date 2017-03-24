using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VSubjectAndDateSelector : Form
    {
        private MFinanceMultData mOperate = new MFinanceMultData();

        private VSubjectAndDateSelector()
        {
            InitializeComponent();
        }

        public VSubjectAndDateSelector(List<CostAccountSubject> subjects)
            : this()
        {
            InitEvent();

            InitData();

            chkSubjectList.DataSource = subjects;
            chkSubjectList.DisplayMember = "Name";
            chkSubjectList.ValueMember = "Code";

            for (var i = 0; i < chkSubjectList.Items.Count; i++)
            {
                chkSubjectList.SetItemChecked(i, true);
            }
        }

        public List<ConstructNodeSubject> SelectNodeSubjects { get; set; }

        private void InitEvent()
        {
            btnOk.Click += new EventHandler(btnOk_Click);
            btnClear.Click += new EventHandler(btnClear_Click);
            chkSelectAll.CheckedChanged += new EventHandler(chkSelectAll_CheckedChanged);
            radBtnPeriodWbs.CheckedChanged += new EventHandler(radBtnPeriodWbs_CheckedChanged);
            radBtnWbsPeriod.CheckedChanged += new EventHandler(radBtnWbsPeriod_CheckedChanged);

            dgvPeriods.AutoGenerateColumns = false;
        }

        private void InitData()
        {
            var projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null)
            {
                var list = mOperate.FinanceMultDataSrv.GetConstructNodeByProject(projectInfo.Id);
                dgvPeriods.DataSource = list.OfType<ConstructNode>().ToArray();
            }

            radBtnWbsPeriod.Checked = true;
        }

        private void SetRowChecked(bool isChecked)
        {
            for (var i = 0; i < dgvPeriods.RowCount; i++)
            {
                dgvPeriods.Rows[i].Cells[colRowCheck.Name].Value = isChecked;
            }
        }

        private void OrderRows(int ordBy)
        {
            var list = dgvPeriods.DataSource as ConstructNode[];
            if (list == null)
            {
                return;
            }

            if (ordBy == 1)
            {
                dgvPeriods.DataSource = list.OrderBy(a => a.WBSName).ThenBy(b => b.DatePeriod.PeriodCode).ToArray();
                colWbsName.DisplayIndex = 2;
                colPeriodName.DisplayIndex = 3;
            }
            else if (ordBy == 2)
            {
                dgvPeriods.DataSource = list.OrderBy(a => a.DatePeriod.PeriodCode).ThenBy(b => b.WBSName).ToArray();
                colWbsName.DisplayIndex = 3;
                colPeriodName.DisplayIndex = 2;
            }
        }

        private decimal GetCellValue(string colName, int rIndex)
        {
            var val = dgvPeriods.Rows[rIndex].Cells[colName].Value;
            if (val == null)
            {
                return 0;
            }

            decimal tmp = 0;
            decimal.TryParse(val.ToString(), out tmp);
            return tmp;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (chkSubjectList.CheckedItems.Count == 0)
            {
                MessageBox.Show("请选择科目");
                return;
            }

            if (dgvPeriods.Rows.Count == 0)
            {
                MessageBox.Show("请选择期间");
                return;
            }

            var retList = new List<ConstructNodeSubject>();
            foreach (CostAccountSubject subjItem in chkSubjectList.CheckedItems)
            {
                var lastRate = 0m;
                for (var i = 0; i < dgvPeriods.RowCount; i++)
                {
                    var isChecked = dgvPeriods.Rows[i].Cells[colRowCheck.Name].Value;
                    if (isChecked == null || !((bool)isChecked))
                    {
                        continue;
                    }

                    var nodeSubject = new ConstructNodeSubject();
                    nodeSubject.Subject = subjItem;
                    nodeSubject.SubjectCode = subjItem.Code;
                    nodeSubject.SubjectName = subjItem.Name;

                    var cNode = dgvPeriods.Rows[i].DataBoundItem as ConstructNode;
                    if (cNode == null)
                    {
                        continue;
                    }
                    nodeSubject.WBSTree = cNode.WBSTree;
                    nodeSubject.WbsName = cNode.WBSName;
                    nodeSubject.DatePeriod = cNode.DatePeriod;
                    nodeSubject.Year = cNode.Year;
                    nodeSubject.Month = cNode.Month;
                    nodeSubject.BeginDate = cNode.BeginDate;
                    nodeSubject.EndDate = cNode.EndDate;
                    nodeSubject.Rate = GetCellValue(colRate.Name, i);
                    nodeSubject.CurrentRate = nodeSubject.Rate - lastRate;
                    if (nodeSubject.CurrentRate < 0)
                    {
                        MessageBox.Show(string.Format("期间{0}的累计进度小于上一个期间的累计进度，请重新设置", nodeSubject.DatePeriod.PeriodName));
                        return;
                    }
                    else if (i == dgvPeriods.RowCount - 1 && nodeSubject.Rate > 0 && nodeSubject.Rate != 100)
                    {
                        MessageBox.Show(string.Format("期间{0}的累计进度必须等于100，请重新设置", nodeSubject.DatePeriod.PeriodName));
                        return;
                    }
                    lastRate = nodeSubject.Rate;

                    retList.Add(nodeSubject);
                }
            }

            if (retList.Count == 0)
            {
                MessageBox.Show("请勾选科目及任务");
                return;
            }

            SelectNodeSubjects = retList;
            this.DialogResult = DialogResult.OK;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            SetRowChecked(false);
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            SetRowChecked(chkSelectAll.Checked);
        }

        private void radBtnWbsPeriod_CheckedChanged(object sender, EventArgs e)
        {
            OrderRows(1);
        }

        private void radBtnPeriodWbs_CheckedChanged(object sender, EventArgs e)
        {
            OrderRows(2);
        }
    }
}
