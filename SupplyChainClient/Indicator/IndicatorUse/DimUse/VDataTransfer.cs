using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Util;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    public partial class VDataTransfer : TBasicDataView
    {
        public VDataTransfer()
        {
            InitializeComponent();
            InitEvents();
        }

        public void Start()
        {
            webReports.Navigate("http://10.1.3.6:8001/reports/handDataBlank.jsp");
        }

        private void InitEvents()
        {
            btnQr.Click += new EventHandler(btnQr_Click);
        }

        void btnQr_Click(object sender, EventArgs e)
        {
            if (edtDate.Text == null || "".Equals(edtDate.Text))
            {
                MessageBox.Show("日期选择不能为空！");
                return;
            }

            if (DialogResult.Yes == KnowledgeMessageBox.QuestionMessage("是否确定本月报表数据？"))
            {
                btnQr.Enabled = false;
                string currDate = edtDate.Text.Trim();
                string currTimeCode = KnowledgeUtil.getTimeCodeByType(currDate, 3);
                webReports.Navigate("http://10.1.3.6:8001/reports/handDataWaiting.jsp?transTimeCode=" + currTimeCode);
            }
        }
    }
}