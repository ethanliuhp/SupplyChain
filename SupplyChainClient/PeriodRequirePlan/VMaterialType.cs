using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.Secure.Client.Basic.Template;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using System.Collections;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.Client.PeriodRequirePlan
{
    public partial class VMaterialType : TBasicDataView
    {
        private IList result = new ArrayList();

        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VMaterialType()
        {
            InitializeComponent();
            InitEvent();
        }

        private void InitEvent()
        {
            this.btnSelect.Click += new EventHandler(btnSelect_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnCancel.FindForm().Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            if (txtMaterial.Text != "")
            {
                this.btnOK.FindForm().Close();
            }
        }

        void btnSelect_Click(object sender, EventArgs e)
        {
            CommonMaterial materialSelectorfs = new CommonMaterial();
            materialSelectorfs.OpenSelect();
            IList list = materialSelectorfs.Result;
            if (list != null && list.Count > 0)
            {
                Material theMaterial = list[0] as Material;
                this.txtMaterial.Tag = theMaterial;
                this.txtMaterial.Text = theMaterial.Name;
                this.txtMaterialSuffer.Text = theMaterial.Specification;
                result.Add(theMaterial);
            }
        }

    }
}
