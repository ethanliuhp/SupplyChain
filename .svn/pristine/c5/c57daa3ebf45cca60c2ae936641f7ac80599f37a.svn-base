using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI
{
    public partial class VSchduleVersionEdit : Form
    {
        public List<string> ListHasVersions = new List<string>();
        public string NewVersion = "";

        public VSchduleVersionEdit()
        {
            InitializeComponent();

            btnEnter.Click += new EventHandler(btnEnter_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        public VSchduleVersionEdit(string sugVersion)
            : this()
        {
            txtVersion.Text = sugVersion;
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        void btnEnter_Click(object sender, EventArgs e)
        {
            NewVersion = txtVersion.Text.Trim();

            if (ListHasVersions.Contains(NewVersion))
            {
                MessageBox.Show("该版本已存在！");
                txtVersion.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
