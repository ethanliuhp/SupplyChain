using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage
{
    public partial class VProjectSelectDialog : Form
    {
        private List<CurrentProjectInfo> allProjects;
        private MFinanceMultData operate = new MFinanceMultData();
        private IList subCompanys;
        private string filteSyscode;
        public VProjectSelectDialog(string filteSyscode)
        {
            InitializeComponent();

            this.filteSyscode = filteSyscode;

            InitEvents();
        }

        public CurrentProjectInfo SelectedProject { get; set; }

        private void InitEvents()
        {
            dgMaster.AutoGenerateColumns = false;
            dgMaster.RowPostPaint += dgMaster_RowPostPaint;
            dgMaster.MouseDoubleClick += new MouseEventHandler(dgMaster_MouseDoubleClick);

            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnFind.Click += new EventHandler(btnFind_Click);
            btnOk.Click += new EventHandler(btnOk_Click);

            txtSearchKey.KeyUp += new KeyEventHandler(txtSearchKey_KeyUp);

            var bgWork = new BackgroundWorker();
            bgWork.DoWork += new DoWorkEventHandler(bgWork_DoWork);
            bgWork.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWork_RunWorkerCompleted);
            bgWork.RunWorkerAsync();
        }

        private void bgWork_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dgMaster.DataSource = allProjects.ToArray();
        }

        private void bgWork_DoWork(object sender, DoWorkEventArgs e)
        {
            allProjects = operate.CurrentProjectSrv.GetAllValideProject().OfType<CurrentProjectInfo>().ToList();
            if (allProjects != null && !string.IsNullOrEmpty(filteSyscode))
            {
                allProjects = allProjects.FindAll(a =>!string.IsNullOrEmpty(a.OwnerOrgSysCode) && a.OwnerOrgSysCode.StartsWith(filteSyscode));
            }
            subCompanys = operate.CurrentProjectSrv.GetSubCompanys();
            if (subCompanys != null)
            {
                foreach (var project in allProjects)
                {
                    var subCom = subCompanys.OfType<OperationOrg>().ToList().Find(s => project.OwnerOrgSysCode.Contains(s.SysCode));
                    project.Data1 = subCom == null ? string.Empty : subCom.Name;
                }
            }

            allProjects = allProjects.OrderByDescending(a => a.Data1).ThenBy(b => b.Name).ToList();
        }

        private void txtSearchKey_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnFind_Click(null, null);
            }
        }

        private void dgMaster_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btnOk_Click(null, null);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if(dgMaster.SelectedRows.Count==0)
            {
                MessageBox.Show("请选择项目");
                return;
            }

            SelectedProject = dgMaster.SelectedRows[0].DataBoundItem as CurrentProjectInfo;
            this.DialogResult = DialogResult.OK;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            var sKey = txtSearchKey.Text.Trim();
            if (string.IsNullOrEmpty(sKey) || allProjects == null)
            {
                dgMaster.DataSource = allProjects == null ? null : allProjects.ToArray();
            }
            else
            {
                dgMaster.DataSource = allProjects.FindAll(p => p.Name.Contains(sKey) || p.Code.Contains(sKey)
                                                               || (!string.IsNullOrEmpty(p.ProjectLocationProvince) && p.ProjectLocationProvince.Contains(sKey))
                                                               || (!string.IsNullOrEmpty(p.ProjectLocationCity) && p.ProjectLocationCity.Contains(sKey))
                                                               || (!string.IsNullOrEmpty(p.ProjectLocationDescript) && p.ProjectLocationDescript.Contains(sKey))
                                                               ).ToArray();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void dgMaster_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgMaster.RowCount)
            {
                return;
            }

            var item = dgMaster.Rows[e.RowIndex].DataBoundItem as CurrentProjectInfo;
            if (item != null)
            {
                var ads = item.ProjectLocationProvince;
                if (!string.IsNullOrEmpty(ads))
                {
                    ads = ads + "省";
                }

                if (!string.IsNullOrEmpty(item.ProjectLocationCity))
                {
                    ads = ads + item.ProjectLocationCity + "市";
                }

                ads = ads + item.ProjectLocationDescript;

                dgMaster.Rows[e.RowIndex].Cells[colProjectAddress.Name].Value = ads;
            }
        }
    }
}
