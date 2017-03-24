using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VSettlementSelector : Form
    {
        private string billType;
        private string projectId;
        private SupplierRelationInfo supplier;
        private decimal surplus;
        private MFinanceMultData mOperate;

        public VSettlementSelector()
        {
            InitializeComponent();

            var curDt = DateTime.Now.Date;
            dtpBeginDate.Value = curDt.AddMonths(-1);
            dtpEndDate.Value = curDt;

            btnFind.Click += new EventHandler(btnFind_Click);
            btnOk.Click += new EventHandler(btnOk_Click);
            dgSettlements.CellClick += new DataGridViewCellEventHandler(dgSettlements_CellClick);
        }

        public VSettlementSelector(string billType, string projId, MFinanceMultData mOperate, SupplierRelationInfo supplier, decimal surplus)
            : this()
        {
            this.billType = billType;
            projectId = projId;
            this.mOperate = mOperate;
            this.supplier = supplier;
            this.surplus = surplus;

            Text = "选择" + billType;
            txtSuplier.Text = supplier.SupplierInfo.Name;
            txtSurplus.Text = surplus.ToString("N2");
        }

        public List<InvoiceSettlementRelation> SelectedBills
        {
            get
            {
                var list = new List<InvoiceSettlementRelation>();
                for (var i = 0; i < dgSettlements.Rows.Count; i++)
                {
                    if (ClientUtil.ToBool(dgSettlements.Rows[i].Cells[0].Value))
                    {
                        var item = new InvoiceSettlementRelation();
                        item.Settlement = ClientUtil.ToString(dgSettlements.Rows[i].Cells[colBillId.Name].Value);
                        item.SettlementCode = ClientUtil.ToString(dgSettlements.Rows[i].Cells[colBillCode.Name].Value);
                        item.SettlementMoney =
                            ClientUtil.ToDecimal(dgSettlements.Rows[i].Cells[colRelationMoney.Name].Value);
                        item.TotalSettlementMoney = ClientUtil.ToDecimal(dgSettlements.Rows[i].Cells[colBillMoney.Name].Value);
                        item.RelationIndex = ClientUtil.ToInt(dgSettlements.Rows[i].Cells[colRelationIndex.Name].Value);

                        list.Add(item);
                    }
                }

                return list;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (SelectedBills.Count == 0)
            {
                MessageBox.Show("请选择结算单据");
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            var list = mOperate.FinanceMultDataSrv.GetSettlementsByType(dtpBeginDate.Value, dtpEndDate.Value, projectId,
                                                                        txtCode.Text.Trim(), billType, supplier.Id);
            if(list==null || list.Count==0)
            {
                MessageBox.Show("没有符合条件的记录，请修改查找条件再试");
                return;
            }

            dgSettlements.Rows.Clear();
            foreach (var dataDomain in list)
            {
                var rIndex = dgSettlements.Rows.Add(1);
                dgSettlements.Rows[rIndex].Cells[colBillId.Name].Value = dataDomain.Name1;
                dgSettlements.Rows[rIndex].Cells[colBillCode.Name].Value = dataDomain.Name2;
                dgSettlements.Rows[rIndex].Cells[colBillMoney.Name].Value = dataDomain.Name3;
                dgSettlements.Rows[rIndex].Cells[colRelationMoney.Name].Value = dataDomain.Name4;
                dgSettlements.Rows[rIndex].Cells[colSurplus.Name].Value = ClientUtil.ToDecimal(dataDomain.Name3) - ClientUtil.ToDecimal(dataDomain.Name4);
                dgSettlements.Rows[rIndex].Cells[colRelationIndex.Name].Value = rIndex + 1;
            }
        }

        private void dgSettlements_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtSelect.Text = SelectedBills.Sum(s => s.TotalSettlementMoney - s.SettlementMoney).ToString("N2");
        }
    }
}
