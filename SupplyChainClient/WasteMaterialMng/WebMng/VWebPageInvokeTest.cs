using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Configuration;

namespace Application.Business.Erp.SupplyChain.Client.WebMng
{
    public partial class VWebPageInvokeTest : TBasicDataView
    {
        public VWebPageInvokeTest()
        {
            InitializeComponent();
            InitEvent();
        }

        private void InitEvent()
        {
            //btnLoadPage.Click += new EventHandler(btnLoadPage_Click);
            //this.Load += new EventHandler(VWebPageInvokeTest_Load);
        }

        void VWebPageInvokeTest_Load(object sender, EventArgs e)
        {
            LoadPage();
        }

        void btnLoadPage_Click(object sender, EventArgs e)
        {
            LoadPage();
        }

        public void LoadPage()
        {
            //string urlStr = "http://localhost/ProjectManage";
            //string urlStr = "http://www.cscec3b.com/";            
            //string urlStr = "owf:OpenForm?subSystem=4,user=admin,password=1";

            string url = ConfigurationManager.AppSettings["UrlIRPLogin"];
            url += "?isForCS=true&user=system&password=manager&transferType=EntityManagement&objectType=DOCUMENT";
            this.webBrowser1.Navigate(url);
        }
    }
}
