using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain;

namespace Application.Business.Erp.SupplyChain.Client.StockMng.StockInMng.StockInUI
{
    public partial class VStockInSeq : TBasicDataView
    {
        MStockMng model = new MStockMng();
        private string stockInRedDtlId;

        public VStockInSeq(string stockInRedDtlId)
        {
            InitializeComponent();
            InitEvent();
            this.stockInRedDtlId = stockInRedDtlId;
        }

        private void InitEvent()
        {
            btnClose.Click += new EventHandler(btnClose_Click);
            this.Load += new EventHandler(VStockInSeq_Load);
        }

        void VStockInSeq_Load(object sender, EventArgs e)
        {
            Query();
        }

        void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Query()
        {
            dgDetail.Rows.Clear();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("StockInRedDtlId",stockInRedDtlId));
            oq.AddOrder(new Order("Step", true));
            try
            {
                IList list = model.StockInSrv.GetStockInDtlSeq(oq);
                if (list == null || list.Count == 0) return;
                foreach (StockInDtlSeq seq in list)
                {
                    int rowIndex = dgDetail.Rows.Add();
                    dgDetail[colMaterialCode.Name, rowIndex].Value = seq.MaterialCode;
                    dgDetail[colMaterialName.Name, rowIndex].Value = seq.MaterialName;
                    dgDetail[colSpec.Name, rowIndex].Value = seq.MaterialSpec;
                    dgDetail[colDescript.Name, rowIndex].Value = seq.Descript;
                    dgDetail[colQuantity.Name, rowIndex].Value = seq.Quantity.ToString("#########.####");
                    dgDetail[colPrice.Name, rowIndex].Value = seq.Price.ToString("##########.########");
                }
            } catch (Exception ex)
            {
                MessageBox.Show("查询入库冲红时序出错。\n"+ex.Message);
            }
        }
    }
}
