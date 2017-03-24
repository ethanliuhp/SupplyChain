using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData;
using Application.Business.Erp.SupplyChain.Util;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage
{
    public partial class UcProjectSelector : UserControl
    {
        private string filteSyscode = string.Empty;

        public UcProjectSelector()
        {
            InitializeComponent();

            InitEvents();
        }

        private CurrentProjectInfo selectedProject;

        public CurrentProjectInfo SelectedProject
        {
            get { return selectedProject; }
            set
            {
                selectedProject = value;
                if (value != null)
                {
                    txtProject.Text = value.Name;
                }
                else
                {
                    txtProject.Text = string.Empty;
                }

                if (AfterSelectProjectEvent != null)
                {
                    AfterSelectProjectEvent(this);
                }
            }
        }

        public delegate void AfterSelectProjectEventHandler(object sender);

        public AfterSelectProjectEventHandler AfterSelectProjectEvent;

        private void InitEvents()
        {
            btnProject.Click += new EventHandler(btnProject_Click);
        }

        public void InitData()
        {
            try
            {
                SelectedProject = StaticMethod.GetProjectInfo();
                if (SelectedProject != null && SelectedProject.Code != CommonUtil.CompanyProjectCode)
                {
                    btnProject.Enabled = false;
                }
                else
                {
                    var subCompany = new MFinanceMultData().CurrentProjectSrv.GetSubCompanys();
                    var subC =
                        subCompany.OfType<OperationOrg>().ToList().Find(
                            c => ConstObject.TheOperationOrg.SysCode.StartsWith(c.SysCode));
                    if (subC != null)
                    {
                        filteSyscode = subC.SysCode;
                    }
                    else
                    {
                        filteSyscode = TransUtil.CompanyOpgSyscode;
                    }
                    SelectedProject = null;
                    btnProject.Enabled = true;
                }
            }
            catch
            {
            }
        }

        private void btnProject_Click(object sender, EventArgs e)
        {
            var dlg = new VProjectSelectDialog(filteSyscode);
            dlg.Owner = this.ParentForm;
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            SelectedProject = dlg.SelectedProject;
        }
    }
}
