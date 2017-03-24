using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;

namespace Application.Business.Erp.SupplyChain.Client
{
    public partial class Test1 : TMasterDetailView
    {
        public Test1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Test2 a = new Test2();
            a.Show();
        }

        int a = 1;

        protected override void OnPaint(PaintEventArgs e)
        {
            this.Title = a++.ToString();
            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            this.Title = a++.ToString();
            base.OnResize(e);
        }
    }
}
