using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.Client.StockMng;

namespace Application.Business.Erp.SupplyChain.Client.WebMng
{
    public partial class ReloadIrpXML : Form
    {
        public ReloadIrpXML()
        {
            InitializeComponent();
        }

        private void btnReoladUserXML_Click(object sender, EventArgs e)
        {
            //IRPWebService.PLMWebServices plm = new Application.Business.Erp.SupplyChain.Client.IRPWebService.PLMWebServices();
            //plm.ReloadUserXML();
        }

        private void btnUpdateIRPJob_Click(object sender, EventArgs e)
        {
            MStockMng model = new MStockMng();
            UpdateIRPXml.UpdateJob(model.StockInSrv);
        }

        private void btnUpdateIRPRole_Click(object sender, EventArgs e)
        {
            MStockMng model = new MStockMng();
            UpdateIRPXml.UpdateRole(model.StockInSrv);
        }

        private void btnUpdateIRPUser_Click(object sender, EventArgs e)
        {
            MStockMng model = new MStockMng();
            UpdateIRPXml.UpdateUser(model.StockInSrv);
        }

        private void btnUpdateIRPPersonOnJob_Click(object sender, EventArgs e)
        {
            MStockMng model = new MStockMng();
            UpdateIRPXml.PersonOnJob(model.StockInSrv);
        }
    }
}
