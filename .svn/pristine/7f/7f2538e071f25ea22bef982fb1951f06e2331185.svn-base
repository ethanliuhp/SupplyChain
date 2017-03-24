using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using NHibernate;
using NHibernate.Criterion;
using VirtualMachine.Core;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VConstructNodeSubject : TBasicDataView
    {
        private CurrentProjectInfo projectInfo;
        private MFinanceMultData mOperate = new MFinanceMultData();
        private List<CostAccountSubject> usedSubjects;
        private List<ConstructNodeSubject> allNodeSubjects; 

        public VConstructNodeSubject()
        {
            InitializeComponent();

            InitEevent();

            InitData();
        }

        private void InitEevent()
        {
            btnBatchSetRate.Click += new EventHandler(btnBatchSetRate_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnSubject.Click += new EventHandler(btnSubject_Click);
            chkSelectAll.CheckedChanged += new EventHandler(chkSelectAll_CheckedChanged);
            radBtnSubjectWbs.CheckedChanged+=new EventHandler(radBtnSubjectWbs_CheckedChanged);
            radBtnWbsSubject.CheckedChanged+=new EventHandler(radBtnWbsSubject_CheckedChanged);

            cmbSubjects.SelectedValueChanged += new EventHandler(cmbSubjects_SelectedValueChanged);

            dgvNodeSubject.AutoGenerateColumns = false;
        }

        private void InitData()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
            {
                txtProject.Text = projectInfo.Name;
            }
            else
            {
                txtProject.Text = string.Empty;
            }

            GetUsedSubjects();

            LoadNodeSubject();
        }

        private void GetUsedSubjects()
        {
            var subjectCodes = new List<string>() { "C51201", "C51202", "C51203", "C51204", "C51103", "C513" };
            Disjunction codesCondition = new Disjunction();
            foreach (var code in subjectCodes)
            {
                codesCondition.Add(Expression.Eq("Code", code));
            }

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(codesCondition);
            usedSubjects =
                mOperate.FinanceMultDataSrv.Query(typeof (CostAccountSubject), oq).OfType<CostAccountSubject>()
                    .OrderBy(a => a.Name).ToList();

            var subjectKeys = new List<KeyValuePair<string, string>>();
            subjectKeys.Add(new KeyValuePair<string, string>("",""));
            usedSubjects.ForEach(a => subjectKeys.Add(new KeyValuePair<string, string>(a.Code, a.Name)));

            cmbSubjects.DataSource = subjectKeys;
            cmbSubjects.DisplayMember = "Value";
            cmbSubjects.ValueMember = "Key";
        }

        private void LoadNodeSubject()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddFetchMode("DatePeriod",FetchMode.Eager);

            allNodeSubjects = 
                mOperate.FinanceMultDataSrv.Query(typeof (ConstructNodeSubject), oq).OfType<ConstructNodeSubject>()
                    .OrderBy(a => a.SubjectName).ThenBy(b => b.Year).ThenBy(c => c.Month).ToList();

            DisplayData(allNodeSubjects);
        }

        private void SetConstructNodeSubjectRate(int startRow, int endRow, decimal rate, decimal change)
        {
            for (int i = startRow; i <= Math.Min(endRow, dgvNodeSubject.Rows.Count); i++)
            {
                var item = dgvNodeSubject.Rows[i - 1].DataBoundItem as ConstructNodeSubject;
                if (item != null)
                {
                    item.Rate = rate;
                }

                rate = rate + change;
            }

            dgvNodeSubject.Refresh();
        }

        private ConstructNodeSubject FindPreviewNode(ConstructNodeSubject cNode)
        {
            var list = dgvNodeSubject.DataSource as List<ConstructNodeSubject>;
            if (cNode == null || list == null)
            {
                return null;
            }

            var fList = list.FindAll(f => f.SubjectCode == cNode.SubjectCode);
            if (cNode.Month == 1)
            {
                return fList.Find(f => f.Year == cNode.Year - 1 && f.Month == 12);
            }
            else
            {
                return fList.Find(f => f.Year == cNode.Year && f.Month == cNode.Month - 1);
            }
        }

        private void DisplayData(List<ConstructNodeSubject> list)
        {
            if (list == null)
            {
                list = new List<ConstructNodeSubject>();
            }

            dgvNodeSubject.DataSource = list.ToArray();
            lbRowCount.Text = string.Format("共计 {0} 行", list.Count);

            if (radBtnSubjectWbs.Checked)
            {
                OrderRows(2);
            }
            else if (radBtnWbsSubject.Checked)
            {
                OrderRows(1);
            }
            else
            {
                radBtnWbsSubject.Checked = true;
            }
        }

        private void OrderRows(int ordBy)
        {
            var list = dgvNodeSubject.DataSource as ConstructNodeSubject[];
            if (list == null)
            {
                return;
            }

            if (ordBy == 1)
            {
                dgvNodeSubject.DataSource = list.OrderBy(a => a.WbsName).ThenBy(b => b.SubjectName).ToArray();
                colWbsName.DisplayIndex = 2;
                colSubjectCode.DisplayIndex = 3;
                colSubjectName.DisplayIndex = 4;
            }
            else if (ordBy == 2)
            {
                dgvNodeSubject.DataSource = list.OrderBy(a => a.SubjectName).ThenBy(b => b.WbsName).ToArray();
                colWbsName.DisplayIndex = 4;
                colSubjectCode.DisplayIndex = 2;
                colSubjectName.DisplayIndex = 3;
            }
        }

        private void btnSubject_Click(object sender, EventArgs e)
        {
            VSubjectAndDateSelector dlg = new VSubjectAndDateSelector(usedSubjects);
            dlg.Owner = this.FindForm();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (allNodeSubjects == null)
                {
                    allNodeSubjects = new List<ConstructNodeSubject>();
                }
                foreach (var nodeSubject in dlg.SelectNodeSubjects)
                {
                    if (
                        !allNodeSubjects.Exists(
                            a => a.WBSTree.Id == nodeSubject.WBSTree.Id &&
                                 a.DatePeriod.PeriodCode == nodeSubject.DatePeriod.PeriodCode &&
                                 a.SubjectCode == nodeSubject.SubjectCode))
                    {
                        nodeSubject.ProjectId = projectInfo.Id;
                        nodeSubject.ProjectName = projectInfo.Name;
                        nodeSubject.CreateDate = DateTime.Now;
                        nodeSubject.CreatePerson = ConstObject.TheLogin.ThePerson;
                        nodeSubject.CreatePersonName = nodeSubject.CreatePerson.Name;

                        nodeSubject.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                        nodeSubject.OperOrgInfoName = nodeSubject.OperOrgInfo.Name;
                        nodeSubject.OpgSysCode = nodeSubject.OperOrgInfo.SysCode;

                        allNodeSubjects.Add(nodeSubject);
                    }
                }

                DisplayData(allNodeSubjects);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var list = dgvNodeSubject.DataSource as ConstructNodeSubject[];
            if (list == null || !list.Any())
            {
                MessageBox.Show("没有数据可供保存");
                return;
            }
            foreach (var cNode in list)
            {
                var preNode = FindPreviewNode(cNode);
                cNode.CurrentRate = cNode.Rate - (preNode == null ? 0 : preNode.Rate);
            }

            if (mOperate.FinanceMultDataSrv.SaveOrUpdateList(list))
            {
                MessageBox.Show("保存成功");
            }
            else
            {
                MessageBox.Show("保存失败");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var list = dgvNodeSubject.DataSource as ConstructNodeSubject[];
            if (list == null || !list.Any())
            {
                MessageBox.Show("请选择要删除的记录");
                return;
            }

            var dbList = list.ToList().FindAll(d => d.IsChecked && !string.IsNullOrEmpty(d.Id));
            foreach (var dl in dbList)
            {
                mOperate.FinanceMultDataSrv.Delete(dl);
            }

            DisplayData(list.ToList().FindAll(d => !d.IsChecked));
        }

        private void btnBatchSetRate_Click(object sender, EventArgs e)
        {
            VBatchSetConstructNodeRate dlg = new VBatchSetConstructNodeRate();
            dlg.Owner = this;
            dlg.TopMost = true;
            dlg.AfterClickOkEvent += SetConstructNodeSubjectRate;
            dlg.Show();
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvNodeSubject.Rows.Count; i++)
            {
                dgvNodeSubject.Rows[i].Cells[colRowCheck.Name].Value = chkSelectAll.Checked;
            }
        }

        private void cmbSubjects_SelectedValueChanged(object sender, EventArgs e)
        {
            if (allNodeSubjects == null || string.IsNullOrEmpty(cmbSubjects.SelectedValue.ToString()))
            {
                DisplayData(allNodeSubjects);
            }
            else
            {
                DisplayData(allNodeSubjects.FindAll(a => a.SubjectCode == cmbSubjects.SelectedValue.ToString()));
            }
        }

        private void radBtnWbsSubject_CheckedChanged(object sender, EventArgs e)
        {
            OrderRows(1);
        }

        private void radBtnSubjectWbs_CheckedChanged(object sender, EventArgs e)
        {
            OrderRows(2);
        }
    }
}
