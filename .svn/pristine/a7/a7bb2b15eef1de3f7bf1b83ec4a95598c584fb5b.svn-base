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
    public partial class WorkFlowHistoryPage : TBasicDataView
    {
        public WorkFlowHistoryPage()
        {
            InitializeComponent();
        }

        public void LoadPage()
        {
            string guid = Guid.NewGuid().ToString();
            string url = ConfigurationManager.AppSettings["UrlIRPLogin"];
            url += "?isForCS=true&user=system&password=manager&transferType=WorkFlowHistory&objectId=AAAA16A14B818A577453D648AD63&random="+guid;
            this.webBrowser1.Navigate(url);
        }

        private void WorkFlowHistoryPage_Load(object sender, EventArgs e)
        {
            LoadPage();
        }
    }
}
