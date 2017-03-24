using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VCostItemsZoning : TBasicDataView
    {
        private MCostItem model = new MCostItem();
        private CostItemCategory result;

        public CostItemCategory Result
        {
            get { return result; }
            set { result = value; }
        }
        public VCostItemsZoning()
        {
            InitializeComponent();
            InitData();
            InitEvent();
        }
        void InitData()
        {
            LoadCostItemCategoryTreeRootNode();
        }

        void InitEvent()
        {
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnQuit.Click += new EventHandler(btnQuit_Click);
            this.tvwCategory.AfterExpand += new TreeViewEventHandler(tvwCategory_AfterExpand);
        }
        private void LoadCostItemCategoryTreeRootNode()
        {
            try
            {
                tvwCategory.Nodes.Clear();

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("State", 1));
                //oq.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
                oq.AddCriterion(Expression.IsNull("ParentNode"));

                IList list = model.ObjectQuery(typeof(CostItemCategory), oq);
                if (list.Count > 0)
                {
                    CostItemCategory childNode = list[0] as CostItemCategory;

                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = childNode.Name;
                    tnTmp.Tag = childNode;


                    tvwCategory.Nodes.Add(tnTmp);

                    this.tvwCategory.SelectedNode = this.tvwCategory.Nodes[0];
                    //this.tvwCategory.SelectedNode.Expand();


                    //加载下一层子节点
                    LoadCostItemCategoryByParentNode(tnTmp);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("加载成本项分类出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }
        private void LoadCostItemCategoryByParentNode(TreeNode parentNode)
        {
            try
            {
                CostItemCategory parentCate = parentNode.Tag as CostItemCategory;
                if (parentCate == null)
                    return;

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("State", 1));
                oq.AddCriterion(Expression.Eq("ParentNode.Id", parentCate.Id));
                oq.AddOrder(NHibernate.Criterion.Order.Asc("OrderNo"));

                IList list = model.ObjectQuery(typeof(CostItemCategory), oq);
                if (list.Count > 0)
                {
                    foreach (CostItemCategory childNode in list)
                    {
                        TreeNode tnTmp = new TreeNode();
                        tnTmp.Name = childNode.Id.ToString();
                        tnTmp.Text = childNode.Name;
                        tnTmp.Tag = childNode;

                        parentNode.Nodes.Add(tnTmp);

                        //this.tvwCategory.SelectedNode = this.tvwCategory.Nodes[0];
                        //this.tvwCategory.SelectedNode.Expand();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("加载成本项分类出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }
        private void LoadSecondLevelCostItemCategoryByParentNode(TreeNode firstParentNode)
        {
            try
            {
                if (firstParentNode.Nodes.Count == 0)
                    return;

                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();

                foreach (TreeNode childNode in firstParentNode.Nodes)
                {
                    if ((childNode.Tag as CostItemCategory) != null)
                        dis.Add(Expression.Eq("ParentNode.Id", (childNode.Tag as CostItemCategory).Id));
                }
                oq.AddCriterion(dis);

                oq.AddCriterion(Expression.Eq("State", 1));
                oq.AddOrder(NHibernate.Criterion.Order.Asc("OrderNo"));

                IList list = model.ObjectQuery(typeof(CostItemCategory), oq);
                if (list.Count > 0)
                {
                    foreach (TreeNode parentNode in firstParentNode.Nodes)
                    {
                        CostItemCategory parentCate = parentNode.Tag as CostItemCategory;
                        if (parentCate == null)
                            continue;

                        foreach (CostItemCategory childNode in list)
                        {
                            if (childNode.ParentNode.Id != parentCate.Id)
                                continue;

                            TreeNode tnTmp = new TreeNode();
                            tnTmp.Name = childNode.Id.ToString();
                            tnTmp.Text = childNode.Name;
                            tnTmp.Tag = childNode;

                            parentNode.Nodes.Add(tnTmp);

                            //this.tvwCategory.SelectedNode = this.tvwCategory.Nodes[0];
                            //this.tvwCategory.SelectedNode.Expand();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("加载成本项分类出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }
        void tvwCategory_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null || (e.Node.Tag as CostItemCategory) == null)
                return;

            TreeNode tempNode = new TreeNode();

            string countSql = "select distinct thecostitemcategory from thd_costitem where thecostitemcategory in ( ";
            string where = "";
            foreach (TreeNode tn in e.Node.Nodes)
            {
                where += "'" + tn.Name + "',";
            }
            countSql += where.Substring(0, where.Length - 1) + ")";

            DataSet ds = model.SearchSQL(countSql);
            DataTable dt = ds.Tables[0];

            List<string> listParentId = new List<string>();

            foreach (DataRow row in dt.Rows)
            {
                string parentId = row[0].ToString();
                listParentId.Add(parentId);
            }

            //foreach (TreeNode tn in e.Node.Nodes)
            //{
            //    if (listParentId.Contains(tn.Name) == false)
            //    {
            //        tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
            //        tn.ForeColor = ColorTranslator.FromHtml("#000000");
            //    }
            //    else
            //    {
            //        tn.BackColor = tempNode.BackColor;
            //        tn.ForeColor = tempNode.ForeColor;
            //    }
            //}

            LoadSecondLevelCostItemCategoryByParentNode(e.Node);
        }

        
        void btnOK_Click(object sender, EventArgs e)
        {
            if (tvwCategory.SelectedNode != null)
            {
                result = tvwCategory.SelectedNode.Tag as CostItemCategory;
            }
            else
            {
                result = null;
            }
            this.Close();
        }
        void btnQuit_Click(object sender, EventArgs e)
        {
            result = null;
            this.Close();
        }

    }
}
