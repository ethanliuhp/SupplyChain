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
    public partial class VMessageYesOrNo : TBasicToolBarByMobile
    {
        public bool yesOrNo = false;

        public VMessageYesOrNo()
        {
            InitializeComponent();
            InitEvents();
            this.toolStrip1.Visible = false;
        }

        public void InitEvents()
        {
            this.btnNo.Click += new EventHandler(btnNo_Click);
            this.btnYes.Click += new EventHandler(btnYes_Click);
        }

        void btnNo_Click(object sender, EventArgs e)
        {
            yesOrNo = false;
            this.Close();
        }

        void btnYes_Click(object sender, EventArgs e)
        {
            yesOrNo = true;
            this.Close();
        }

        
    }
}
