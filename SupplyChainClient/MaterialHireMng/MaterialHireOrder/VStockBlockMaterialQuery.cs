using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain;
using Application.Business.Erp.SupplyChain.Client.PLMWebServices;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder
{
    public partial class VStockBlockMaterialQuery : TBasicDataView
    {
        private MMaterialHireMng model = new MMaterialHireMng();
        CurrentProjectInfo projectInfo;
        public VStockBlockMaterialQuery()
        {
            InitializeComponent();
            dtpDateBeginBill.Value = DateTime.Now.AddDays(-7);
            dtpDateEndBill.Value = DateTime.Now;
            btnSearchBill.Click+=new EventHandler(btnSearchBill_Click);
            this.dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            this.dgMaster.CellContentClick += new DataGridViewCellEventHandler(dgMaster_CellContentClick);
        }
        public void btnSearchBill_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            if (this.txtCodeBeginBill.Text != "")
            {
                if (this.txtCodeBeginBill.Text == this.txtCodeEndBill.Text)
                {
                    oq.AddCriterion(Expression.Like("Code", this.txtCodeBeginBill.Text, MatchMode.Start));
                }
                else
                {
                    oq.AddCriterion(Expression.Ge("Code", this.txtCodeBeginBill.Text));
                    oq.AddCriterion(Expression.Lt("Code", this.txtCodeEndBill.Text));
                }
            }
            if (TenantSelectorBill.SelectedProject != null)
            {
                oq.AddCriterion(Expression.Eq("ProjectId", TenantSelectorBill.SelectedProject.Id));
            }
            if (txtSupplyBill.Result != null && txtSupplyBill.Result.Count > 0 && txtSupplyBill.Result[0] is SupplierRelationInfo)
            {
                oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", (txtSupplyBill.Result[0] as SupplierRelationInfo)));
            }
           
            oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBeginBill.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEndBill.Value.AddDays(1).Date));
            if (this.txtCreatePersonBill.Text != "")
            {
                oq.AddCriterion(Expression.Eq("CreatePersonName", this.txtCreatePersonBill.Text));
            }
            if (!string.IsNullOrEmpty(txtOldCode.Text))
            {
                oq.AddCriterion(Expression.Like("BillCode", txtOldCode.Text, MatchMode.Anywhere));
            }
            IList list = model.MaterialHireSupplyOrderSrv.GetStockBlockMasterOrder(oq);
            dgMaster.Rows.Clear();
            dgDetail.Rows.Clear();
            ShowMasterList(list);

        }
        public void ShowMasterList(IList lstMaster)
        {
            if (lstMaster == null || lstMaster.Count == 0) return;
            foreach (MatHireStockBlockMaster master in lstMaster)
            {
                int i = this.dgMaster.Rows.Add();
                this.dgMaster[colCodeMaster.Name, i].Value = master.Code;
                this.dgMaster[colStateMaster.Name, i].Value = ClientUtil.GetDocStateName(master.DocState);
                this.dgMaster[colCreateDateMaster.Name, i].Value = master.CreateDate.ToShortDateString();
                this.dgMaster[colRealOperationDateMaster.Name, i].Value = master.RealOperationDate.ToShortDateString();
                this.dgMaster[colSupplyInfoMaster.Name, i].Tag = master.TheSupplierRelationInfo;
                this.dgMaster[colSupplyInfoMaster.Name, i].Value = master.SupplierName;
                this.dgMaster[colProjectNameBill.Name, i].Value = master.ProjectName;
                this.dgMaster[colBalRuleMaster.Name, i].Value = master.BalRule;
                this.dgMaster[colPersonName.Name, i].Value = master.CreatePersonName;
                this.dgMaster[colBlockStart.Name, i].Value = master.BlockStartTime.ToString("yyyy-MM-dd");
                this.dgMaster[this.colBlockEnd.Name, i].Value = master.BlockFinishTime.ToString("yyyy-MM-dd");
                this.dgMaster[this.colTheme.Name, i].Value = master.Theme;
                this.dgMaster[this.colMasterDescript.Name, i].Value = master.Descript;
                this.dgMaster[colBalState.Name, i].Value = master.BalState == 1 ? "是" : "否";
                this.dgMaster[this.colBillCode.Name, i].Value = master.BillCode;
                this.dgMaster.Rows[i].Tag = master;
            }
            dgMaster.CurrentCell = dgMaster[1, 0];
            dgMaster_SelectionChanged(null,null);
        }
        public void ShowDetailList( )
        {
            dgDetail.Rows.Clear();
            MatHireStockBlockMaster master = dgMaster.CurrentRow.Tag as MatHireStockBlockMaster;
           
            if (master == null) return;
            if (master.MatHireType == EnumMatHireType.碗扣)
            {
                colType.Visible = true;
                colLength.Visible = false;
            }
            else if (master.MatHireType == EnumMatHireType.钢管)
            {
                colType.Visible = false;
                colLength.Visible = true;
            }
            else
            {
                colType.Visible = false;
                colLength.Visible = false;
            }
            foreach (MatHireStockBlockDetail var in master.Details)
            {
                int i = this.dgDetail.Rows.Add();
                this.dgDetail[colId.Name, i].Value = var.Id;
                this.dgDetail[colStockId.Name, i].Value = var.StockId;
                this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;
                this.dgDetail[colQuantity.Name, i].Value = var.Quantity;
                this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    this.dgDetail[this.colType.Name, i].Value = var.MaterialType;
                    this.dgDetail[this.colLength.Name, i].Value = var.MaterialLength;
                //设置使用部位
                this.dgDetail[colUsedPart.Name, i].Tag = var.UsedPart;
                this.dgDetail[colUsedPart.Name, i].Value = var.UsedPartName;

                //设置使用队伍
                this.dgDetail[this.colBorrowUnit.Name, i].Tag = var.BorrowUnit;
                this.dgDetail[colBorrowUnit.Name, i].Value = var.BorrowUnitName;
                //科目
                this.dgDetail[colSubject.Name, i].Tag = var.SubjectGUID;
                this.dgDetail[colSubject.Name, i].Value = var.SubjectGUID == null ? "" : var.SubjectGUID.Name;
                this.dgDetail[colPrice.Name, i].Value = var.Price;

                this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
                this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnitName;
                this.dgDetail[colMoney.Name, i].Value = var.Money;
                this.dgDetail.Rows[i].Tag = var;
            }
        }
        public void dgMaster_SelectionChanged(object sender, EventArgs  e)
        {
            ShowDetailList();
        }
        public void dgMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgDetail[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || dgMaster.Rows[e.RowIndex].Tag==null) return;
            if (e.ColumnIndex == colCodeMaster.Index)
            {
                MatHireStockBlockMaster master = dgMaster.Rows[e.RowIndex].Tag as MatHireStockBlockMaster;
                VStockBlockMaterial vmro = new VStockBlockMaterial(master.MatHireType);
                vmro.Start(master.Id);
                vmro.StartPosition = FormStartPosition.CenterParent;
                vmro.RefreshControls(VirtualMachine.Component.WinMVC.generic.MainViewState.Browser);
                vmro.ShowDialog();
            }
        }
         
    }
}
