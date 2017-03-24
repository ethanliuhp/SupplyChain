using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Domain;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.Stock.StockRelationUI
{
    public partial class VStockRelationSelect : Form
    {
        private MStockRelation theMStockRelation = new MStockRelation();
        private StationCategory theStationCategory = null;
        private OperationOrg theOrgInfo = null;
        private Hashtable hashSelect = new Hashtable();
        private IList lstResult = new ArrayList();
        private SupplierRelationInfo theSupplierRelationInfo = null;
        private string ckBusinessType = "";

        public VStockRelationSelect()
        {
            InitializeComponent();
            LoadMaterialCategoryTree();
            InitEvent();
        }
        public IList ShowSelect(StationCategory aStationCategory, SupplierRelationInfo aSupplierRelationInfo, OperationOrg aOrgInfo, string businssType)
        {
            theStationCategory = aStationCategory;
            theSupplierRelationInfo = aSupplierRelationInfo;
            theOrgInfo = aOrgInfo;
            ckBusinessType = businssType;
            this.ShowDialog();
            return lstResult;
        }

        private void InitEvent()
        {
            this.tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);
            this.dgStockRelation.CellEnter += new DataGridViewCellEventHandler(dgStockRelation_CellEnter);
            this.dgStockRelation.CellValidated += new DataGridViewCellEventHandler(dgStockRelation_CellValidated);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnOk.Click += new EventHandler(btnOk_Click);
            this.btnQuery.Click += new EventHandler(btnQuery_Click);
            dgStockRelation.CellContentClick += new DataGridViewCellEventHandler(dgStockRelation_CellContentClick);
            this.dgStockRelation.CellValidating += new DataGridViewCellValidatingEventHandler(dgStockRelation_CellValidating);
        }

        void dgStockRelation_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dgStockRelation.CurrentRow;
            int currentColIndex = e.ColumnIndex;
            if (row.Cells["HaveQty1"].ColumnIndex == currentColIndex)
            {
                //if (ClientUtil.ToString(row.Cells["BundleNo1"].Value) != "")
                //{
                //    (this.dgStockRelation.Rows[e.RowIndex].Tag as StockRelation).RefQuantity = ClientUtil.TransToDecimal(this.dgStockRelation["HaveQty1", e.RowIndex].Value);
                //    ModifySelectResultGrid(this.dgStockRelation.Rows[e.RowIndex].Tag as StockRelation);
                //}
            }
        }

        void dgStockRelation_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (this.dgStockRelation.Columns[e.ColumnIndex].Name.Equals("RefQuantity"))
            {
                //判断引用数量是否大于当前的库存量
                if (ClientUtil.TransToDecimal(e.FormattedValue) > ClientUtil.TransToDecimal(this.dgStockRelation["HaveQty1", e.RowIndex].Value))
                {
                    MessageBox.Show("引用量不能大于当前的可用量！");
                    e.Cancel = true;
                }
            }
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            if (ClientUtil.ToString(this.txtSpec.Text) != "")
            {
                oq.AddCriterion(Expression.Like("TheManageState.Material.Specification", "%" + this.txtSpec.Text + "%"));
            }
            if (ClientUtil.ToString(this.txtStuff.Text) != "")
            {
                oq.AddCriterion(Expression.Like("TheManageState.Material.Stuff", "%" + this.txtStuff.Text + "%"));
            }
            if (ClientUtil.ToString(this.txtName.Text) != "")
            {
                oq.AddCriterion(Expression.Like("TheManageState.Material.Name", "%" + this.txtName.Text + "%"));
            }
            //显示改分类下的所有库存物料
            IList lstTmp = new ArrayList();
            if (this.txtStationCategory.Text != "" && this.txtStationCategory.Result != null && this.txtStationCategory.Result.Count > 0)
            {
                theStationCategory = this.txtStationCategory.Result[0] as StationCategory;
            }
            if (theStationCategory != null)
            {
                oq.AddCriterion(Expression.Eq("TheStationcategory", theStationCategory));
            }
            if (ClientUtil.ToString(ckBusinessType) != "")
            {
                oq.AddCriterion(Expression.Eq("TheStationcategory.BusinessType", ckBusinessType));
            }
            if (ClientUtil.ToString(this.txtBundleNo.Text) != "")
            {
                oq.AddCriterion(Expression.Like("TheManageState.Other1", "%" + this.txtBundleNo.Text + "%"));
            }
            if (theSupplierRelationInfo != null)
            {
                oq.AddCriterion(Expression.Eq("TheManageState.Supplier", theSupplierRelationInfo));
            }
            if (theOrgInfo != null && LoginInfomation.LoginInfo.TheSysRole.Id != "441")
            {
                oq.AddCriterion(Expression.Eq("TheManageState.HandleOrg", theOrgInfo));
            }
            //lstTmp = theMStockRelation.theStockRelationSrv.GetMaterialRelationByQuery(oq);
            this.dgStockRelation.Rows.Clear();
            string spec = "";
            string mName = "";
            if (lstTmp != null && lstTmp.Count > 0)
            {
                foreach (StockRelation var in lstTmp)
                {
    
          
  
                }
                this.dgStockRelation.AutoResizeColumns();
                if (this.dgStockRelation.Rows.Count > 0)
                {
                    this.dgStockRelation.Rows[0].Selected = true;
                }
            }
        }

        void btnOk_Click(object sender, EventArgs e)
        {
            //再次判断锁定数量
            string relIdStr = "0";
            foreach (DataGridViewRow var in this.dgSelect.Rows)
            {
                StockRelation rel = var.Tag as StockRelation;
                if (("," + relIdStr + ",").IndexOf("," + rel.Id + ",") == -1)
                {
                    relIdStr += "," + rel.Id;
                }
            }


            foreach (DataGridViewRow var in this.dgSelect.Rows)
            {

            }
            this.btnOk.FindForm().Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.lstResult.Clear();
            this.btnCancel.FindForm().Close();
        }

        void dgStockRelation_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
         
            ModifySelectResultGrid(this.dgStockRelation.Rows[e.RowIndex].Tag as StockRelation);
        }
        private void ModifySelectResultGrid(StockRelation aStockRelation)
        {
    
        }

        void dgStockRelation_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.dgStockRelation.EditMode = DataGridViewEditMode.EditProgrammatically;
            switch (this.dgStockRelation.Columns[e.ColumnIndex].Name)
            {
                case "RefQuantity":
                    this.dgStockRelation.BeginEdit(true);
                    break;
                default:
                    break;
            }
        }

        void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //显示改分类下的所有库存物料
            IList lstTmp = new ArrayList();
            ObjectQuery oq = new ObjectQuery();
            if (ClientUtil.ToString(this.txtSpec.Text) != "")
            {
                oq.AddCriterion(Expression.Like("TheManageState.Material.Specification", "%" + this.txtSpec.Text + "%"));
            }
            if (ClientUtil.ToString(this.txtName.Text) != "")
            {
                oq.AddCriterion(Expression.Like("TheManageState.Material.Name", "%" + this.txtName.Text + "%"));
            }
            if (ClientUtil.ToString(this.txtBundleNo.Text) != "")
            {
                oq.AddCriterion(Expression.Like("TheManageState.Other1", "%" + this.txtBundleNo.Text + "%"));
            }
            if (theStationCategory != null)
            {
                oq.AddCriterion(Expression.Eq("TheStationcategory", theStationCategory));
            }
            if (theSupplierRelationInfo != null)
            {
                oq.AddCriterion(Expression.Eq("TheManageState.Supplier", theSupplierRelationInfo));
            }
            if (theOrgInfo != null && LoginInfomation.LoginInfo.TheSysRole.Id != "441")
            {
                oq.AddCriterion(Expression.Eq("TheManageState.HandleOrg", theOrgInfo));
            }
            oq.AddCriterion(Expression.Eq("TheManageState.Material.Category", e.Node.Tag as MaterialCategory));
            //lstTmp = theMStockRelation.GetMaterialRelation(e.Node.Tag as MaterialCategory);

        }

        private void LoadMaterialCategoryTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();

                IList list = theMStockRelation.GetAllMaterialCategory();

                foreach (object o in list)
                {
                    MaterialCategory childNode = o as MaterialCategory;
                    if (childNode.State == 0 && childNode.ParentNode != null)
                    {
                        continue;
                    }


                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = childNode.Name;
                    tnTmp.Tag = childNode;
                    if (childNode.ParentNode != null)
                    {
                        TreeNode tnp = null;
                        tnp = hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                        tnp.Nodes.Add(tnTmp);
                    }
                    else
                    {
                        tvwCategory.Nodes.Add(tnTmp);
                    }
                    hashtable.Add(tnTmp.Name, tnTmp);
                }
                if (list.Count > 0)
                {
                    this.tvwCategory.SelectedNode = this.tvwCategory.Nodes[0];
                    this.tvwCategory.SelectedNode.Expand();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("装载分类树出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }
    }
}