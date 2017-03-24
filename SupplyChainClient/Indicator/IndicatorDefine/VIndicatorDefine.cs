using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorDefine.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Enums;


namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorDefine
{
    public partial class VIndicatorDefine : TBasicDataView
    {
        private MIndicatorDefine model = new MIndicatorDefine();
        
        public VIndicatorDefine()
        {
            InitializeComponent();
        }

        internal void Start()
        {
            LoadIndicatorCategoryTree();
        }

        private void LoadIndicatorCategoryTree()
        {
            try
            {
                Hashtable table = new Hashtable();
                TVIndicatorCategory.Nodes.Clear();
                IList list = model.IndicatorDefSrv.GetCategorys();
                foreach (IndicatorCategory cat in list)
                {
                    TreeNode tmpNode = new TreeNode();
                    tmpNode.Name = cat.Id.ToString();
                    tmpNode.Text = cat.Name;
                    tmpNode.Tag = cat;
                    if (cat.ParentNode != null)
                    {
                        TreeNode node = table[cat.ParentNode.Id.ToString()] as TreeNode;
                        node.Nodes.Add(tmpNode);
                    }
                    else
                    {
                        TVIndicatorCategory.Nodes.Add(tmpNode);
                    }
                    tmpNode.ContextMenuStrip = this.contextMenuStripIndicatorCategory;
                    table.Add(tmpNode.Name, tmpNode);
                }
                //选中顶级节点，并且展开一级子节点
                if (list.Count > 0)
                {
                    TVIndicatorCategory.SelectedNode = TVIndicatorCategory.Nodes[0];
                    TVIndicatorCategory.SelectedNode.Expand();
                }
            }
            catch (Exception e)
            {
                KnowledgeMessageBox.InforMessage("装载指标分类树出错：" + StaticMethod.ExceptionMessage(e));
            }
        }

        private void menuItemAddNode_Click(object sender, EventArgs e)
        {
            AddCategory();
        }

        private void AddCategory()
        {
            ////VIndicatorCategoryInfo categoryInfo = new VIndicatorCategoryInfo();
            ////categoryInfo.ShowDialog(pnlFloor.FindForm());

            TreeNode node = TVIndicatorCategory.SelectedNode;
            if (node == null)
            {
                KnowledgeMessageBox.InforMessage("请先选择一个分类。");
                return;
            }
            if (node != null)
            {
                TreeNode newNode = new TreeNode("新类别");
                newNode.ContextMenuStrip = this.contextMenuStripIndicatorCategory;
                node.Nodes.Add(newNode);
                TVIndicatorCategory.LabelEdit = true;
                TVIndicatorCategory.SelectedNode = newNode;
                newNode.BeginEdit();

                IndicatorCategory category = new IndicatorCategory();
                category.Name = newNode.Text;
                category.ParentNode = node.Tag as IndicatorCategory;
                newNode.Tag = category;
            } 
        }

        private void menuItemDeleteCategory_Click(object sender, EventArgs e)
        {
            DeleteChildNodes();
        }

        /// <summary>
        /// 删除下级类别
        /// </summary>
        private void DeleteChildNodes()
        {
            TreeNode node = TVIndicatorCategory.SelectedNode;
            if (node == null)
            {
                KnowledgeMessageBox.InforMessage("请先选择一个分类。");
                return;
            }
            if (node != null)
            {
                DialogResult dr = KnowledgeMessageBox.QuestionMessage("确定要删除当前节点及其所有子节点吗？");
                if (dr == DialogResult.Yes)
                {
                    IndicatorCategory category = node.Tag as IndicatorCategory;
                    bool canRemove = true;
                    try
                    {
                        model.IndicatorDefSrv.DeleteCategory(category);
                    }
                    catch (Exception ex)
                    {
                        KnowledgeMessageBox.InforMessage("删除节点出错：" + StaticMethod.ExceptionMessage(ex));
                        canRemove = false;
                    }
                    if (canRemove)
                    {
                        node.Remove();
                    }
                }
            }
        }

        private void menuItemRenameCagetory_Click(object sender, EventArgs e)
        {
            RenameChildNodes();
        }

        /// <summary>
        /// 重命名下级类别
        /// </summary>
        private void RenameChildNodes()
        {
            TreeNode node = TVIndicatorCategory.SelectedNode;
            if (node == null)
            {
                KnowledgeMessageBox.InforMessage("请先选择一个分类。");
                return;
            }
            if (node != null)
            {
                TVIndicatorCategory.LabelEdit = true;
                node.BeginEdit();
            }
        }

        private void menuItemAddIndicator_Click(object sender, EventArgs e)
        {
            AddIndicator();
        }

        /// <summary>
        /// 添加指标
        /// </summary>
        private void AddIndicator()
        {
            TreeNode node = TVIndicatorCategory.SelectedNode;
            if (node == null)
            {
                KnowledgeMessageBox.InforMessage("请先选择一个分类。");
                return;
            }
            if (node != null)
            {
                IndicatorCategory category = node.Tag as IndicatorCategory;
                IndicatorDefinition indicator = new IndicatorDefinition();
                VIndicatorInfo indicatorInfo = new VIndicatorInfo(indicator);
                if (indicatorInfo.Open(pnlFloor.FindForm(), ref indicator, Application.Business.Erp.SupplyChain.Enums.Action.New))
                {
                    indicator.Category = category;
                    indicator.CreatedDate = DateTime.Now;
                    indicator.Author = ConstObject.LoginPersonInfo;
                    indicator.TheOpeOrg = ConstObject.TheOperationOrg;

                    try
                    {
                        model.IndicatorDefSrv.SaveIndicatorDefinition(indicator);
                        LoadNewIndicator(indicator);
                    }
                    catch (Exception ex)
                    {
                        KnowledgeMessageBox.InforMessage("添加指标出错。\n"+StaticMethod.ExceptionMessage(ex));
                    }
                }
            }
        }

        //节点右键事件
        private void TVIndicatorCategory_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode node = TVIndicatorCategory.SelectedNode;
                if (node != null && node.Level == 0)
                {
                    menuItemDeleteCategory.Visible = false;
                    menuItemRenameCategory.Visible = false;
                }
                else
                {
                    menuItemDeleteCategory.Visible = true;
                    menuItemRenameCategory.Visible = true;
                }
            }
        }

        private void TVIndicatorCategory_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            string label = e.Label;
            TreeNode editingNode = e.Node;
            IndicatorCategory category = editingNode.Tag as IndicatorCategory;
            if (label == null)
            {
                category.Name = editingNode.Text;
                category.Code = category.Name;
                try
                {
                    IndicatorCategory temp=model.IndicatorDefSrv.SaveCategory(category);
                    editingNode.Tag = temp;
                    TVIndicatorCategory.LabelEdit = false;
                }
                catch (Exception ex)
                {
                    KnowledgeMessageBox.InforMessage("保存指标分类节点出错：" + StaticMethod.ExceptionMessage(ex));
                    editingNode.BeginEdit();
                }
            }
            else if (label.Trim().Equals(""))
            {
                KnowledgeMessageBox.InforMessage("类别名称不能为空！");
                e.CancelEdit = true;
                editingNode.BeginEdit();
            }
            else
            {
                category.Name = label;
                category.Code = category.Name;
                try
                {
                    IndicatorCategory temp=model.IndicatorDefSrv.SaveCategory(category);
                    editingNode.Tag = temp;
                    TVIndicatorCategory.LabelEdit = false;
                }
                catch (Exception ex)
                {
                    KnowledgeMessageBox.InforMessage("保存分类节点出错：" + StaticMethod.ExceptionMessage(ex));
                    editingNode.BeginEdit();
                }
            }
        }

        private void TVIndicatorCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            IndicatorCategory category = node.Tag as IndicatorCategory;
            if (category != null)
            {
                LoadIndicators(category);
            }
        }

        private void menuItemEditIndicator_Click(object sender, EventArgs e)
        {
            ViewIndicatorDetail();
        }

        /// <summary>
        /// 查看编辑指标
        /// </summary>
        private void ViewIndicatorDetail()
        {
            if (dgvIndicator.SelectedRows.Count == 0)
            {
                KnowledgeMessageBox.InforMessage("请先选择一个指标。");
                return;
            }
            foreach (DataGridViewRow row in dgvIndicator.SelectedRows)
            {
                IndicatorDefinition indicator = row.Tag as IndicatorDefinition;
                if (indicator != null)
                {
                    if (indicator.Author != null)
                    {
                        if (indicator.Author.Id == ConstObject.LoginPersonInfo.Id)
                        {
                            VIndicatorInfo indicatorInfo = new VIndicatorInfo(indicator);
                            if (indicatorInfo.Open(pnlFloor.FindForm(), ref indicator, Application.Business.Erp.SupplyChain.Enums.Action.Edit))
                            {
                                try
                                {
                                    IndicatorDefinition temp=model.IndicatorDefSrv.SaveIndicatorDefinition(indicator);
                                    FillRow(row, temp);
                                }
                                catch (Exception ex)
                                {
                                    KnowledgeMessageBox.InforMessage("保存指标出错。\n"+StaticMethod.ExceptionMessage(ex));
                                }
                            }
                        }
                        else
                        {
                            VIndicatorInfo indicatorInfo = new VIndicatorInfo(indicator);
                            indicatorInfo.Open(pnlFloor.FindForm(), Application.Business.Erp.SupplyChain.Enums.Action.View);
                        }
                    }
                    else
                    {
                        VIndicatorInfo indicatorInfo = new VIndicatorInfo(indicator);
                        indicatorInfo.Open(pnlFloor.FindForm(), Application.Business.Erp.SupplyChain.Enums.Action.View);
                    }
                }
                break;
            }
        }

        private void menuItemDeleteIndicator_Click(object sender, EventArgs e)
        {
            DeleteIndicator();
        }

        /// <summary>
        /// 删除指标
        /// </summary>
        private void DeleteIndicator()
        {
            if (dgvIndicator.SelectedRows.Count == 0)
            {
                KnowledgeMessageBox.InforMessage("请先选择一个指标。");
            }

            foreach (DataGridViewRow row in dgvIndicator.SelectedRows)
            {
                if (DialogResult.Yes == KnowledgeMessageBox.QuestionMessage("确定要删除当前指标吗？"))
                {
                    try
                    {
                        model.IndicatorDefSrv.DeleteIndicatorDefinition((IndicatorDefinition)row.Tag);
                        dgvIndicator.Rows.Remove(row);
                    }
                    catch (Exception ex)
                    {
                        KnowledgeMessageBox.InforMessage("删除指标出错。",ex);
                    }
                }
                
                break;
            }
        }

        private void dgvIndicator_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                foreach (DataGridViewRow row in dgvIndicator.SelectedRows)
                {
                    IndicatorDefinition indicator = row.DataBoundItem as IndicatorDefinition;
                    if (indicator != null)
                    {
                        if (indicator.Author != null)
                        {
                            if (indicator.Author.Id == ConstObject.LoginPersonInfo.Id)
                            {
                                menuItemEditIndicator.Text = "编辑";
                                menuItemDeleteIndicator.Enabled = true;
                            }
                            else
                            {
                                menuItemEditIndicator.Text = "查看";
                                menuItemDeleteIndicator.Enabled = false;
                            }
                        }
                        else
                        {
                            menuItemEditIndicator.Text = "查看";
                            menuItemDeleteIndicator.Enabled = false;
                        }
                    }
                    break;
                }
            }
        }

        //装载指标列表
        private void LoadIndicators(IndicatorCategory category)
        {
            try
            {
                dgvIndicator.Rows.Clear();
                if (category == null)
                    return;
                IList list = model.IndicatorDefSrv.GetIndicatorDefinitionByCategory(category);
                foreach (IndicatorDefinition indicatorDefinition in list)
                {
                    LoadNewIndicator(indicatorDefinition);
                }
            }
            catch (Exception e)
            {
                KnowledgeMessageBox.InforMessage("装载指标列表出错。\n" + StaticMethod.ExceptionMessage(e));
            }
        }

        //装载指标
        private void LoadNewIndicator(IndicatorDefinition obj)
        {
            int i = dgvIndicator.Rows.Add();
            LoadIndicator(obj, i);
        }

        //装载指标
        private void LoadIndicator(IndicatorDefinition obj, int i)
        {
            try
            {
                DataGridViewRow r = dgvIndicator.Rows[i];
                FillRow(r, obj);
            }
            catch (Exception e)
            {
                KnowledgeMessageBox.InforMessage("装载指标出错。\n" + StaticMethod.ExceptionMessage(e));
            }
        }

        private void FillRow(DataGridViewRow row, IndicatorDefinition obj)
        {
            if (row == null)
            {
                return;
            }
            row.Tag = obj;
            row.Cells["IndicatorCode"].Value = obj.Code;
            row.Cells["IndicatorName"].Value = obj.Name;
            row.Cells["Unit"].Value = obj.UnitId;
            row.Cells["IndicatorRemark"].Value = obj.Description;            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            FindIndicator();
        }

        private void FindIndicator()
        {
            ObjectQuery oq = new ObjectQuery();
            string name = txtIndicatorName.Text;
            if (name != null && !name.Trim().Equals(""))
            { 
                oq.AddCriterion(Expression.Like("Name",name+"%"));
            }

            string code = txtIndicatorCode.Text;
            if (code != null && !code.Trim().Equals(""))
            { 
                oq.AddCriterion(Expression.Like("Code",code+"%"));
            }

            try
            {
                IList list = model.IndicatorDefSrv.GetIndicatorDefinitionByQuery(oq);
                dgvIndicator.Rows.Clear();
                foreach (IndicatorDefinition obj in list)
                {
                    LoadNewIndicator(obj);
                }
            }
            catch (Exception ex)
            {
                KnowledgeMessageBox.InforMessage("查找指标出错。\n"+StaticMethod.ExceptionMessage(ex));
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtIndicatorCode.Text = "";
            txtIndicatorName.Text = "";
        }

        private void lblCategoryAdd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AddCategory();
        }

        private void lblCategoryEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RenameChildNodes();
        }

        private void lblCategoryDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DeleteChildNodes();
        }

        private void lblIndicatorAdd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AddIndicator();
        }

        private void lblIndicatorEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ViewIndicatorDetail();
        }

        private void lblIndicatorDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DeleteIndicator();
        }

        private void dgvIndicator_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ViewIndicatorDetail();
        }
    }
}