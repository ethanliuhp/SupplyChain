using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using NHibernate.Criterion;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.StockMng;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI
{
    public partial class VStockInForward : TBasicDataView
    {
        MStockMng theMStockIn = new MStockMng();
        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VStockInForward()
        {
            InitializeComponent();
            InitForm();
            InitEvent();
            InitData();
            txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode;
        }
        private void InitEvent()
        {
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.dgSupplyReceving.SelectionChanged += new EventHandler(dgSupplyReceving_SelectionChanged);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            this.lnkAll.Click += new EventHandler(lnkAll_Click);
            this.lnkNone.Click += new EventHandler(lnkNone_Click);
        }

        private void InitData()
        {

        }

        void lnkNone_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                var.Cells["Select"].Value = false;
            }
        }

        void lnkAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                var.Cells["Select"].Value = true;
            }
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            (this.dgDetail.Rows[e.RowIndex].Tag as StockInDtl).IsSelect = (bool)this.dgDetail["Select", e.RowIndex].Value;


        }

        void dgSupplyReceving_SelectionChanged(object sender, EventArgs e)
        {
            if (dgSupplyReceving.CurrentRow.Tag == null) return;
            //显示明细
            dgDetail.Rows.Clear();
            foreach (StockInDtl var in (dgSupplyReceving.CurrentRow.Tag as StockIn).Details)
            {
                int i = this.dgDetail.Rows.Add();
                this.dgDetail["Select", i].Value = false;
                this.dgDetail["MaterialCode", i].Tag = var.MaterialResource;
                this.dgDetail["MaterialCode", i].Value = var.MaterialResource.Code;
                this.dgDetail["MaterialName", i].Value = var.MaterialResource.Name;
                this.dgDetail["MaterialSpec", i].Value = var.MaterialResource.Specification;
                //this.dgDetail["BundleNo", i].Value = var.BundleNo;

                this.dgDetail["Unit", i].Value = var.MatStandardUnit.Name;

                this.dgDetail["Quantity", i].Value = var.Quantity - var.RefQuantity;
                this.dgDetail["Price", i].Value = var.Price;
                this.dgDetail["Money", i].Value = (var.Quantity - var.RefQuantity) * var.Price;
                this.dgDetail["Descript", i].Value = var.Descript;
                ////管理实例属性
                //this.dgDetail["fromFactory", i].Value = var.FromManufactory;
                //this.dgDetail["ContractNo", i].Value = var.FromContract;
                ////this.dgDetail["fromContract", i].Value = var.FromContract;
                //this.dgDetail["Batch", i].Value = var.Batch;
                this.dgDetail.Rows[i].Tag = var;
            }
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            IList list = new ArrayList();
            oq.AddCriterion(Expression.Between("CreateDate", this.dtpDateBegin.Value.Date, this.dtpDateEnd.Value.Date));
            oq.AddCriterion(Expression.Eq("IsTally", true));
            oq.AddOrder(new Order("Id", false));
            if (this.txtSupplier.Text != "" && this.txtSupplier.Result != null && this.txtSupplier.Result.Count != 0)
            {
                oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", this.txtSupplier.Result[0]));
            }
            if (this.txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Between("Code", this.txtCodeBegin.Text, this.txtCodeEnd.Text));
            }
            //if (LoginInfomation.LoginInfo.TheOperationOrgInfo.CkRight == "1")
            //{
            //    oq.AddCriterion(Expression.Eq("OperOrgInfo", LoginInfomation.LoginInfo.TheOperationOrgInfo));
            //}
            IList l_manner = new ArrayList();
            l_manner.Add("41");
            oq.AddCriterion(Expression.In("TheStockInManner.Id", l_manner));
            list = theMStockIn.StockInSrv.GetDomainByCondition(typeof(StockIn), oq);

            //显示
            this.dgSupplyReceving.Rows.Clear();
            this.dgDetail.Rows.Clear();
            foreach (StockIn var in list)
            {
                int i = this.dgSupplyReceving.Rows.Add();
                this.dgSupplyReceving["Id", i].Value = var.Id.ToString();
                this.dgSupplyReceving["Code", i].Value = var.Code;
                if (var.TheStationCategory != null)
                    this.dgSupplyReceving["TheStationCategory", i].Value = var.TheStationCategory.Name;

                if (var.TheSupplierRelationInfo != null)
                    this.dgSupplyReceving["SupplyInfo", i].Value = var.TheSupplierRelationInfo.SupplierInfo.Name;

                //this.dgSupplyReceving["FromContract", i].Value = var.ContractNo;
                this.dgSupplyReceving["Remark", i].Value = var.Descript;
                this.dgSupplyReceving.Rows[i].Tag = var;
            }
            if (this.dgSupplyReceving.Rows.Count > 0)
            {
                dgSupplyReceving.CurrentCell = this.dgSupplyReceving[1, 0];
                dgSupplyReceving_SelectionChanged(this.dgSupplyReceving, new EventArgs());
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.result.Clear();
            this.dgDetail.EndEdit();
            if (this.dgSupplyReceving.SelectedRows.Count == 0)
            {
                MessageBox.Show("没有可以引用的到货单！");
                return;
            }
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) break;
                StockInDtl dtl = var.Tag as StockInDtl;
                dtl.IsSelect = (bool)var.Cells["Select"].Value;
            }
            this.result.Add(this.dgSupplyReceving.SelectedRows[0].Tag);
            this.btnOK.FindForm().Close();
        }

        private void InitForm()
        {
            this.Title = "入库单引用";
            dtpDateBegin.Value = ConstObject.LoginDate.AddMonths(-1);
            dtpDateEnd.Value = ConstObject.LoginDate;

            this.dgSupplyReceving.RowTemplate.Height = 18;
            this.dgSupplyReceving.ColumnHeadersHeight = 20;

            this.dgDetail.RowTemplate.Height = 18;
            this.dgDetail.ColumnHeadersHeight = 20;
        }
    }
}