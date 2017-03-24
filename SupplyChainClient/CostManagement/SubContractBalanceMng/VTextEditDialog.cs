using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng
{
    public partial class VTextEditDialog : Form
    {
        public VTextEditDialog(string txt, bool isRead)
        {
            InitializeComponent();

            btnOk.Click += new EventHandler(btnOk_Click);

            RemarkText = txt;
            txtRemark.ReadOnly = isRead;
            if (isRead)
            {
                this.Text = "备注-查看";
            }
            else
            {
                this.Text = "备注-编辑";
            }
        }

        public string RemarkText
        {
            get { return txtRemark.Text.Trim(); }
            set { txtRemark.Text = value; }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
