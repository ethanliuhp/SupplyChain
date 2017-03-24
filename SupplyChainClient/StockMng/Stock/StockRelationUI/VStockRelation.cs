using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.Client.Util;
using C1.Win.C1FlexGrid;
using Application.Business.Erp.SupplyChain.Client.Basic.Controls;
using System.Diagnostics;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.Stock.StockRelationUI
{
    public partial class VStockRelation : TMasterDetailView
    {
        MStockRelation theMStockRelation = new MStockRelation();
        private IDictionary hashTally = new Hashtable();
        private IDictionary hashCode = new Hashtable();
        private Hashtable cat_ht = new Hashtable();

        public VStockRelation()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }
        private void InitData()
        {
            this.dgDetail.RowTemplate.Height = 18;
            dgDetail.ColumnHeadersHeight = 20;
            dgDetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
            txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode;
            //cat_ht = ClientUtil.GetAllNewCategory();

            //if (LoginInfomation.LoginInfo.TheOperationOrgInfo.CkRight == "1")
            //{
            //    this.optAll.Visible = false;
            //    this.optCost.Visible = false;
            //    this.txtSupplier.Enabled = false;
            //    this.Supplier.Visible = false;
            //    this.OSupplier.Visible = false;
            //}
        }

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            //this.button1.Click += new EventHandler(button1_Click);
            this.txtMaterial.Validating += new CancelEventHandler(txtMaterial_Validating);
        }

        void txtMaterial_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtMaterial.Result != null && this.txtMaterial.Result.Count > 0)
            {
                Material a = this.txtMaterial.Result[0] as Material;
                if (a.Specification != this.txtSpec.Text)
                {
                    this.txtSpec.Text = a.Specification;
                }
                if (a.Stuff != this.txtStuff.Text)
                {
                    this.txtStuff.Text = a.Stuff;
                }
            }
        }

        //void button1_Click(object sender, EventArgs e)
        //{
        //    UCL.Locate("库存查询", StockRelationExcType.StockRelationSelect);
        //}

        void btnExcel_Click(object sender, EventArgs e)
        {
            //this.fgFlex.SaveToExcel(true);
            if (tabControl1.SelectedTab.Name.Equals("tabPage1"))
            {
                StaticMethod.ExcelClass.SaveDataGridViewToExcel(dgDetail, true);
            }
            else if (tabControl1.SelectedTab.Name.Equals("tabPage2"))
            {
                StaticMethod.ExcelClass.SaveDataGridViewToExcel(dgCost, true);
            }
        }

        void btnSearch_Click(object sender, EventArgs e)
        {

   
        }

        void QueryDetail(IList list)
        {

        }

        void QueryCost(IList list)
        {

        }
        /// <summary>
        /// 启动本窗体,(设置状态或重新加载已有的数据)
        /// </summary>
        /// <param name="code">窗体Caption</param>
        public void Start(string code)
        {
            try
            {
            }
            catch (Exception e)
            {
                MessageBox.Show("视图启动出错：" + ExceptionUtil.ExceptionMessage(e)); ;
            }
        }
    }
}