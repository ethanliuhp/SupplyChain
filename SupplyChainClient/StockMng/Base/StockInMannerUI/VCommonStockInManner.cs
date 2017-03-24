using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Core;
using System.Collections;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.Base.StockOutPurposeUI;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.Base.StockInMannerUI
{
    public partial class VCommonStockInManner : TBasicDataView
    {
        private IList result = new ArrayList();
        public IList Result
        {
            get { return result; }
        }
        private static MStockInManner theMStockInManner = new MStockInManner();

        public VCommonStockInManner()
        {
            InitializeComponent();
            InitData();
            InitEvent();
        }
        private void InitEvent()
        {
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            if (this.grdDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要返回的行！");
                return;
            }
            foreach (DataGridViewRow var in this.grdDetail.SelectedRows)
            {
                if ((var.Tag as StockInManner).State == 1)
                    result.Add(var.Tag);
            }
            this.Close();
        }

        public void OpenSelectView(ObjectQuery oq, IWin32Window owner)
        {
            this.ShowDialog(owner);
        }
        private void InitData()
        {
            IList lst = theMStockInManner.GetData();
            foreach (StockInManner var in lst)
            {
                int i = grdDetail.Rows.Add();
                grdDetail.Rows[i].Tag = var;
                grdDetail["Code", i].Value = var.Code;
                grdDetail["Name", i].Value = var.Name;
                grdDetail["State", i].Value = var.State;
            }
            if (this.grdDetail.Rows.Count > 0)
            {
                this.grdDetail.Rows[0].Selected = true;
            }
        }
    }
}