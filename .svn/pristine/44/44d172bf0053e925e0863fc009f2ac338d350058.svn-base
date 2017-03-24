using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount;
using Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng
{
    public partial class VSubContractBalDetailSelect : TBasicDataView
    {
        MCostMonthAccount model = new MCostMonthAccount();
        private SubContractBalanceBill master;

        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VSubContractBalDetailSelect(SubContractBalanceBill bill)
        {
            master = bill;
            InitializeComponent();
            InitEvent();
            InitData();
        }

        private void InitEvent()
        {
            btnOK1.Click += new EventHandler(btnOK1_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            lnkAll.Click += new EventHandler(lnkAll_Click);
            lnkNone.Click += new EventHandler(lnkNone_Click);
            this.dgDetail.SelectionChanged += new EventHandler(dgDetail_SelectionChanged);
        }

        void btnOK1_Click(object sender, EventArgs e)
        {
            this.result.Clear();
            this.dgDetail.EndEdit();
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) break;
                if (ClientUtil.ToBool(var.Cells[colSelected.Name].Value) == true)
                {
                    SubContractBalanceDetail dtl = var.Tag as SubContractBalanceDetail;
                    result.Add(dtl);
                }
            }
            this.btnOK1.FindForm().Close();
        }

        /// <summary>
        /// 反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkNone_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                bool isSelected = (bool)var.Cells[colSelected.Name].Value;
                var.Cells[colSelected.Name].Value = !isSelected;
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                var.Cells[colSelected.Name].Value = true;
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnCancel.FindForm().Close();
        }

        private void Clear()
        {
            lblRecordTotal.Text = string.Format("共选择【{0}】条记录", 0);
            txtSumQuantity.Text = "";
        }

        private void InitData()
        {
            this.txtAccountRootNode.Text = master.BalanceTaskName;
            this.txtBalOrg.Text = master.SubContractUnitName;
            this.txtBeginDate.Text = master.BeginTime.ToShortDateString();
            this.txtEndDate.Text = master.EndTime.ToShortDateString();
            this.dgDetail.Rows.Clear();
            if (master == null) return;
            //分包结算明细
            foreach (SubContractBalanceDetail detail in master.Details)
            {
                int rowIndex = dgDetail.Rows.Add();
                dgDetail.Rows[rowIndex].Tag = detail;
                dgDetail[this.colDTaskNode.Name, rowIndex].Value = detail.BalanceTaskName;
                dgDetail[this.colDTaskMx.Name, rowIndex].Value = detail.BalanceTaskDtlName;
                dgDetail[this.colBalMoney.Name, rowIndex].Value = detail.BalanceTotalPrice;
                dgDetail[this.colBalPrice.Name, rowIndex].Value = detail.BalancePrice;
                dgDetail[this.colBalQty.Name, rowIndex].Value = detail.BalacneQuantity;
                dgDetail[this.colForwardType.Name, rowIndex].Value = detail.FontBillType.ToString();
                dgDetail[this.colQtyUnit.Name, rowIndex].Value = detail.QuantityUnitName;
                dgDetail[this.colPriceUnit.Name, rowIndex].Value = detail.PriceUnitName;
            }
            if (master.Details.Count > 0)
            {
                dgDetail_SelectionChanged(master, new EventArgs());
            }
        }

        void dgDetail_SelectionChanged(object sender, EventArgs e)
        {
            this.dgSubject.Rows.Clear();

            SubContractBalanceDetail detail = dgDetail.CurrentRow.Tag as SubContractBalanceDetail;
            if (detail == null) return;
            //分包结算物资耗用
            foreach (SubContractBalanceSubjectDtl dtlConsume in detail.Details)
            {
                int rowIndex = this.dgSubject.Rows.Add();
                dgSubject.Rows[rowIndex].Tag = dtlConsume;
                dgSubject[this.colCostName.Name, rowIndex].Value = dtlConsume.CostName;
                
                dgSubject[this.colResourceType.Name, rowIndex].Value = dtlConsume.ResourceTypeName;
                dgSubject[this.colResourceSpec.Name, rowIndex].Value = dtlConsume.ResourceTypeSpec;
                dgSubject[this.colResDiagramNumber.Name, rowIndex].Value = dtlConsume.DiagramNumber;

                dgSubject[this.colBalSubject.Name, rowIndex].Value = dtlConsume.BalanceSubjectName;
                dgSubject[this.colBalQty1.Name, rowIndex].Value = dtlConsume.BalanceQuantity;
                dgSubject[this.colBalPrice1.Name, rowIndex].Value = dtlConsume.BalancePrice;
                dgSubject[this.colBalMoney1.Name, rowIndex].Value = dtlConsume.BalanceTotalPrice;
                dgSubject[this.colQtyUnit1.Name, rowIndex].Value = dtlConsume.QuantityUnitName;
                dgSubject[this.colPriceUnit1.Name, rowIndex].Value = dtlConsume.PriceUnitName;
            }
        }

    }
}
