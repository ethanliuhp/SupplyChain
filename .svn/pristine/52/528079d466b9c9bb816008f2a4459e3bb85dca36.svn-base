using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.MoneyManage;
 
namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.CommonControl
{
    public partial class UcTenantSelector : UserControl
    {
        public UcTenantSelector()
        {
            InitializeComponent();
            InitEvents();
            txtTenant.ReadOnly = true;
        }

        private CurrentProjectInfo selectedProject;
        public string ProjectID
        {
            get { return SelectedProject == null ? null : SelectedProject.Id; }
            set {
                if (string.IsNullOrEmpty(value))
                {
                    SelectedProject = null;
                }
                else
                {
                    SelectedProject = new MMaterialHireMng().CurrentProjectSrv.GetProjectById(value);
                }
            }

        }
        public CurrentProjectInfo SelectedProject
        {
            get { return selectedProject; }
            set
            {
                selectedProject = value;
                if (value != null)
                {
                    txtTenant.Text = value.Name;
                }
                else
                {
                    txtTenant.Text = string.Empty;
                }
                if (this.TenantSelectorAfterEvent != null)
                {
                    TenantSelectorAfterEvent(this);
                }
            }
        }
        public bool Validate(bool IsErrorMessageBox)
        {
            bool bResult = false;
            if (SelectedProject == null)
            {
                bResult = false;
                MessageBox.Show("请选择租赁方!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Focus();
            }
            else
            {
                bResult = true;
            }
            return bResult;
        }
 

        private void InitEvents()
        {
            btnTenantSelector.Click += new EventHandler(btnProject_Click);
            txtTenant.KeyDown += new KeyEventHandler(txtTenant_KeyDown);
           // txtTenant.KeyUp += new KeyEventHandler(txtTenant_KeyUp);
            txtTenant.ReadOnlyChanged += new EventHandler(txtTenant_ReadOnlyChanged);
           // txtTenant.EnabledChanged += new EventHandler(txtTenant_ReadOnlyChanged);
            txtTenant.DoubleClick += new EventHandler(btnProject_Click);
        }

        public void InitData()
        {
            
        }
        private void txtTenant_ReadOnlyChanged(object sender, EventArgs e)
        {
            if (txtTenant.ReadOnly == false)
            {
               // txtTenant.EnabledChanged -= txtTenant_ReadOnlyChanged;
                txtTenant.ReadOnlyChanged -= txtTenant_ReadOnlyChanged;
                txtTenant.ReadOnly = true;
                //txtTenant.Enabled = false;
                txtTenant.ReadOnlyChanged += txtTenant_ReadOnlyChanged ;
              ///  txtTenant.EnabledChanged += txtTenant_ReadOnlyChanged;
            }
        }
        private void txtTenant_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.Enabled)
            {
                if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
                {

                    SelectedProject = null;
                    e.Handled = true;
                }
                else
                {
                    e.Handled = false;
                }
            }
            else
            {
                e.Handled = false;
            }
        }
        public delegate void TenantSelectorAfterEventHandler(UcTenantSelector sender);
      public TenantSelectorAfterEventHandler TenantSelectorAfterEvent;
        private void btnProject_Click(object sender, EventArgs e)
        {
            if (this.Enabled)
            {
                var dlg = new VProjectSelectDialog(string.Empty);
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;
                dlg.Owner = this.ParentForm;
                dlg.Text = "选择租赁方";
                if (dlg.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                SelectedProject = dlg.SelectedProject;
            }
        }
    }
}
