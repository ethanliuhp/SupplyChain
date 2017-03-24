using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyOrderMngCompany
{
    public partial class VProjectSelector : Form
    {
        MSupplyOrderMng model = null;
        private IList result = new ArrayList();

        public IList Result
        {
            get { return result; }
        }

        public VProjectSelector(MSupplyOrderMng theMSupplyOrderMng)
        {
            InitializeComponent();
            this.model = theMSupplyOrderMng;
            InitEvent();
        }

        private void InitEvent()
        {
            btnOk.Click += new EventHandler(btnOk_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            Load += new EventHandler(VProjectSelector_Load);
        }

        void VProjectSelector_Load(object sender, EventArgs e)
        {
            LoadProjectInfo();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        void btnOk_Click(object sender, EventArgs e)
        {
            result.Clear();
            foreach (DataGridViewRow dr in dgProject.Rows)
            {
                object select = dr.Cells[colSelect.Name].Value;
                if (select != null && (bool)select)
                {
                    CurrentProjectInfo pi = dr.Tag as CurrentProjectInfo;
                    result.Add(pi);
                }
            }
            if (result.Count == 0)
            {
                MessageBox.Show("请至少选择一个项目。");
                return;
            }
            Close();
        }

        private void LoadProjectInfo()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddOrder(NHibernate.Criterion.Order.Asc("Name"));
            IList projectLst= model.SupplyOrderSrv.GetDomainByCondition(typeof(CurrentProjectInfo), oq);
            if (projectLst == null || projectLst.Count == 0) return;
            foreach (CurrentProjectInfo pi in projectLst)
            {
                int i = dgProject.Rows.Add();
                DataGridViewRow dr = dgProject.Rows[i];
                dr.Cells[colSelect.Name].Value = false;
                dr.Tag = pi;
                dr.Cells[colProjectName.Name].Value = pi.Name;
            }
        }
    }
}
