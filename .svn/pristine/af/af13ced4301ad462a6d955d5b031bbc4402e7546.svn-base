using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Linq;

using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Main;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonForm;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemCategoryMng
{
    public partial class VSelectCostItemCategory : TBasicDataView
    {
        /// <summary>
        /// 是否选择“确定”按钮
        /// </summary>
        public bool IsOK = false;
        /// <summary>
        /// 选择的成本项分类
        /// </summary>
        public CostItemCategory SelectCategory = null;

        private MCostItem model = new MCostItem();

        public VSelectCostItemCategory()
        {

            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();

            LoadCostItemCategoryTreeRootNode();

            //LoadCostItemCategoryTree();

        }

        private void InitEvents()
        {

            tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);
            tvwCategory.AfterExpand += new TreeViewEventHandler(tvwCategory_AfterExpand);

            btnEnter.Click += new EventHandler(btnEnter_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            this.FormClosing += new FormClosingEventHandler(VSelectCostItem_FormClosing);
        }

        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                SelectCategory = tvwCategory.SelectedNode.Tag as CostItemCategory;
                this.GetNodeDetail();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
            }
        }
        void tvwCategory_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null || (e.Node.Tag as CostItemCategory) == null)
                return;

            LoadSecondLevelCostItemCategoryByParentNode(e.Node);
        }
        //确定
        void btnEnter_Click(object sender, EventArgs e)
        {
            if (SelectCategory == null)
            {
                MessageBox.Show("请选择一个成本项分类！");
                tvwCategory.Focus();
                return;
            }

            IsOK = true;

            this.Close();
        }
        //取消
        void btnCancel_Click(object sender, EventArgs e)
        {
            SelectCategory = null;
            this.Close();
        }
        //关闭窗体
        void VSelectCostItem_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsOK)
                SelectCategory = null;
        }

        private void GetNodeDetail()
        {
            try
            {
                ClearAll();

                this.txtName.Text = SelectCategory.Name;
                this.txtCode.Text = SelectCategory.Code;
                this.txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath;

                if (SelectCategory.CategoryState != 0)
                    this.txtState.Text = SelectCategory.CategoryState.ToString();
                else
                    this.txtState.Text = CostItemCategoryState.制定.ToString();

                this.txtDesc.Text = SelectCategory.Describe;
                this.txtSummary.Text = SelectCategory.Summary;

            }
            catch (Exception exp)
            {
                MessageBox.Show("显示出错：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        private void ClearAll()
        {
            this.txtCurrentPath.Text = "";
            this.txtCode.Text = "";
            this.txtName.Text = "";
            this.txtState.Text = "";
            this.txtDesc.Text = "";
            this.txtSummary.Text = "";
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
        private void LoadCostItemCategoryTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                IList list = model.GetCostItemCategoryByInstance(projectInfo.Id);
                //lstInstance = listAll[1] as IList;
                //IList list = listAll[0] as IList;
                foreach (CostItemCategory childNode in list)
                {
                    if (childNode.State == 0)
                        continue;

                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = childNode.Name;
                    tnTmp.Tag = childNode;
                    if (childNode.ParentNode != null)
                    {
                        TreeNode tnp = null;
                        tnp = hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                        if (tnp != null)
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

    }
}
