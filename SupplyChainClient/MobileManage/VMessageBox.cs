using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage
{
    public partial class VMessageBox : TBasicToolBarByMobile
    {
        public VMessageBox()
        {
            InitializeComponent();
            InitEvents();
            this.toolStrip1.Visible = false;
        }

        public void InitEvents()
        {
            this.btnExit.Click += new EventHandler(btnExit_Click);
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.btnExit.FindForm().Close();
        }
    }
}
