using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using VirtualMachine.Core;
using IRPServiceModel.Domain.Document;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng
{
    public partial class VDocumentCategorySelect : TBasicDataView
    {
        string categoryTypeName = string.Empty;
        bool isTempCate;
        public MDocumentCategory model = new MDocumentCategory();
        private CurrentProjectInfo projectInfo = null;
        private DocumentCategory resultCate;

        public DocumentCategory ResultCate
        {
            get { return resultCate; }
            set { resultCate = value; }
        }

        public VDocumentCategorySelect()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="istemp">是不是模版分类</param>
        public VDocumentCategorySelect(bool isOrNotTemp)
        {
            InitializeComponent();
            isTempCate = isOrNotTemp;
            InitEvent();
            InitData();
        }
        private void InitEvent()
        {
            this.btnSortTree.Click += new EventHandler(btnSortTree_Click);
            this.btnSelect.Click += new EventHandler(btnSelect_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
        }

        void InitData()
        {
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            dgvSortList.Visible = false;
            tvwCateGory.Visible = true;
            //LoadDocumentCategory();
            LoadDocumentCategory(null);
        }
        /// <summary>
        /// 表查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSelect_Click(object sender, EventArgs e)
        {
            tvwCateGory.Visible = false;
            dgvSortList.Visible = true;
            ObjectQuery oq = new ObjectQuery();
            if (txtKeyword.Text.Trim() != "")
            {
                oq.AddCriterion(Expression.Like("Name", txtKeyword.Text, MatchMode.Anywhere));
            }
            if (isTempCate)
            {
                oq.AddCriterion(Expression.Eq("ProjectCode", "KB"));
            }
            else
            {
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            }
            IList resultList = model.ObjectQuery(typeof(DocumentCategory), oq);
            dgvSortList.Rows.Clear();
            if (resultList != null&&resultList.Count>0)
            {
                foreach (DocumentCategory node in resultList)
                {
                    int rowIndex = dgvSortList.Rows.Add();
                    dgvSortList[colSortCode.Name, rowIndex].Value = node.Code;
                    dgvSortList[colSortName.Name, rowIndex].Value = node.Name;
                    dgvSortList[colSortExplain.Name, rowIndex].Value = node.Describe;
                    dgvSortList.Rows[rowIndex].Tag = node;
                }
            }
            else
            {
                MessageBox.Show("未找到数据！");
            }
        }

        /// <summary>
        /// 树查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSortTree_Click(object sender, EventArgs e)
        {
            LoadDocumentCategory(null);
        }
        /// <summary>
        /// 加载文档分类
        /// </summary>
        void LoadDocumentCategory()
        {
            dgvSortList.Visible = false;
            tvwCateGory.Visible = true;

            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCateGory.Nodes.Clear();

                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();
                if (isTempCate)
                {
                    oq.AddCriterion(Expression.Eq("ProjectCode", "KB"));
                }
                else
                {
                    dis.Add(Expression.Not(Expression.Eq("ProjectCode", "KB")));
                    dis.Add(Expression.IsNull("ProjectCode"));
                    oq.AddCriterion(dis);
                }
                oq.AddOrder(NHibernate.Criterion.Order.Asc("SysCode"));
                IList cateList = model.ObjectQuery(typeof(DocumentCategory), oq);
                if (cateList != null && cateList.Count > 0)
                {
                    foreach (DocumentCategory childNode in cateList)
                    {
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
                            tvwCateGory.Nodes.Add(tnTmp);
                        }
                        hashtable.Add(tnTmp.Name, tnTmp);
                    }
                    this.tvwCateGory.SelectedNode = this.tvwCateGory.Nodes[0];
                    this.tvwCateGory.SelectedNode.Expand();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(e));
            }

        }
        /// <summary>
        /// 加载当前节点的下一级子节点
        /// </summary>
        /// <param name="oNode"></param>
        void LoadDocumentCategory(TreeNode node)
        {
            try
            {
                int level = 1;
                string sysCode = string.Empty;
                string projectId = string.Empty;
                if (projectInfo != null && projectInfo.Id != null)
                {
                    projectId = projectInfo.Id;
                }
                if (node != null)
                {
                    DocumentCategory cate = node.Tag as DocumentCategory;
                    level = cate.Level + 1;
                    sysCode = cate.SysCode;
                    node.Nodes.Clear();
                }
                IList cateList = model.GetDocumentCategoryChildList(level, sysCode, isTempCate, projectId);
                foreach (DocumentCategory cate in cateList)
                {
                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = cate.Id;
                    tnTmp.Text = cate.Name;
                    tnTmp.Tag = cate;
                    if (cate.CategoryNodeType != VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode)
                    {
                        tnTmp.Nodes.Add("test");
                    }

                    if (node != null)
                    {
                        node.Nodes.Add(tnTmp);
                    }
                    else
                    {
                        tvwCateGory.Nodes.Add(tnTmp);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(e));
            }

        }

        void btnOK_Click(object sender, EventArgs e)
        {
            if (dgvSortList.Visible == true)
            {
                if (dgvSortList.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请选择一个任务节点！");
                    return;
                }
                resultCate = dgvSortList.CurrentRow.Tag as DocumentCategory;
            }
            if (tvwCateGory.Visible == true)
            {
                if (tvwCateGory.SelectedNode == null)
                {
                    MessageBox.Show("请选择一个任务节点！");
                    return;
                }
                resultCate = tvwCateGory.SelectedNode.Tag as DocumentCategory;
            }
            this.Close();
        }


    }
}
