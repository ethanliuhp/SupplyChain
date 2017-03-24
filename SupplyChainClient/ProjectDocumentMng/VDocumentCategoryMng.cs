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
using IRPServiceModel.Domain.Document;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using Application.Business.Erp.SupplyChain.Client.FileUpload;
using System.IO;
using IRPServiceModel.Basic;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng;

namespace Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng
{
    public partial class VDocumentCategoryMng : TBasicDataView
    {
        private MDocumentCategory model = null;
        private CurrentProjectInfo projectInfo = null;
        private TreeNode oprNode;//当前操作节点
        private DocumentCategory oprCate;//当前操作文档分类

        public VDocumentCategoryMng()
        {
            InitializeComponent();
            InitData();
        }
        void InitData()
        {
            model = new MDocumentCategory();
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            InitEvent();

            dgDocumentMast.ReadOnly = true;
            cbCate.Items.AddRange(new object[] { "文档模板", "项目文档" });
            cbCate.SelectedIndex = 1;
            cbCate.DropDownStyle = ComboBoxStyle.DropDownList;

            txtCateName.ReadOnly = true;
            txtCateCode.ReadOnly = true;
            btnSave.Enabled = false;

            if (tvwCateGory.Nodes.Count > 0)
            {
                tvwCateGory.SelectedNode = tvwCateGory.Nodes[0];
            }

            if (projectInfo == null || projectInfo.Id == null)
            {
                btnSelect.Enabled = true;
            }
            else
            {
                txtProject.Text = projectInfo.Name;
            }
        }
        void InitEvent()
        {
            tvwCateGory.AfterSelect += new TreeViewEventHandler(tvwCateGory_AfterSelect);
            //tvwCateGory.BeforeSelect += new TreeViewCancelEventHandler(tvwCateGory_BeforeSelect);
            tvwCateGory.NodeMouseClick += new TreeNodeMouseClickEventHandler(tvwCateGory_NodeMouseClick);
            tvwCateGory.BeforeExpand += new TreeViewCancelEventHandler(tvwCateGory_BeforeExpand);
            mnuCateGory.ItemClicked += new ToolStripItemClickedEventHandler(mnuCateGory_ItemClicked);
            btnSave.Click += new EventHandler(btnSave_Click);

            dgDocumentMast.CellClick += new DataGridViewCellEventHandler(dgDocumentMast_CellClick);

            btnDocumentMaterAdd.Click += new EventHandler(btnDocumentMaterAdd_Click);
            btnDocumentMasterUpdate.Click += new EventHandler(btnDocumentMasterUpdate_Click);
            btnDocumentMasterDelete.Click += new EventHandler(btnDocumentMasterDelete_Click);

            btnDocumentDetailAdd.Click += new EventHandler(btnDocumentDetailAdd_Click);
            btnDocumentDetailUpdate.Click += new EventHandler(btnDocumentDetailUpdate_Click);
            btnDocumentDetailDelete.Click += new EventHandler(btnDocumentDetailDelete_Click);
            btnDocumentDetailDownLoad.Click += new EventHandler(btnDocumentDetailDownLoad_Click);
            btnDocumentDetailShow.Click += new EventHandler(btnDocumentDetailShow_Click);

            //dgDocumentMast.MouseClick += new MouseEventHandler(dgDocumentMast_MouseClick);
            //dgDocumentMast.CellMouseClick += new DataGridViewCellMouseEventHandler(dgDocumentMast_CellMouseClick);
            //mnuDocumentMaster.ItemClicked += new ToolStripItemClickedEventHandler(mnuDocumentMaster_ItemClicked);

            //dgDocumentDetail.MouseClick += new MouseEventHandler(dgDocumentDetail_MouseClick);
            //dgDocumentDetail.CellMouseClick += new DataGridViewCellMouseEventHandler(dgDocumentDetail_CellMouseClick);
            //mnuDocumentDetail.ItemClicked += new ToolStripItemClickedEventHandler(mnuDocumentDetail_ItemClicked);
            cbCate.SelectedIndexChanged += new EventHandler(cbCate_SelectedIndexChanged);
            lnkCheckAll.Click += new EventHandler(lnkCheckAll_Click);
            lnkCheckAllNot.Click += new EventHandler(lnkCheckAllNot_Click);

            btnSelect.Click += new EventHandler(btnSelect_Click);
        }

        //当登陆人所属项目为空时 选择项目
        void btnSelect_Click(object sender, EventArgs e)
        {
            VSelectProjectInfo frm = new VSelectProjectInfo();
            CurrentProjectInfo extProject = new CurrentProjectInfo();
            frm.ListExtendProject.Add(extProject);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                bool flag = false;
                CurrentProjectInfo selectProject = frm.Result[0] as CurrentProjectInfo;

                if (projectInfo != null && projectInfo.Id != null && projectInfo.Id != selectProject.Id)
                {
                    flag = true;
                }
                else if (projectInfo == null)
                {
                    flag = true;
                }
                if (flag)
                {
                    txtProject.Text = selectProject.Name;
                    projectInfo = selectProject;
                    tvwCateGory.Nodes.Clear();
                    LoadDocumentCategory(null);
                    if (tvwCateGory.Nodes.Count > 0)
                    {
                        tvwCateGory.SelectedNode = tvwCateGory.Nodes[0];
                    }
                    flag = false;
                }
            }
        }

        #region 文档分类
        void tvwCateGory_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {

        }

        void tvwCateGory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (oprNode != null)
            {
                TreeNode tn = new TreeNode();
                oprNode.BackColor = tn.BackColor;
                oprNode.ForeColor = tn.ForeColor;
            }

            tvwCateGory.SelectedNode = e.Node;
            oprNode = e.Node;
            if (oprNode == null) return;
            oprNode.BackColor = ColorTranslator.FromHtml("#D7E8FE");
            oprNode.ForeColor = ColorTranslator.FromHtml("#000000");

            oprCate = oprNode.Tag as DocumentCategory;
            txtCateName.Text = oprCate.Name;
            txtCateCode.Text = oprCate.Code;
            txtCateName.ReadOnly = true;
            txtCateCode.ReadOnly = true;
            btnSave.Enabled = false;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Category.Id", oprCate.Id));
            if (oprCate.ProjectCode != "KB")
            {
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            }
            IList listDocMaster = model.ObjectQuery(typeof(DocumentMaster), oq);
            dgDocumentMast.Rows.Clear();
            dgDocumentDetail.Rows.Clear();
            if (listDocMaster != null && listDocMaster.Count > 0)
            {
                foreach (DocumentMaster m in listDocMaster)
                {
                    AddDgDocumentMastInfo(m);
                }
                dgDocumentMast.CurrentCell = dgDocumentMast[2, 0];
                dgDocumentMast_CellClick(dgDocumentMast, new DataGridViewCellEventArgs(2, 0));
            }

        }
        void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValideSave()) return;
            try
            {
                bool isNew = false;
                if (oprCate == null || oprCate.Id == null)
                {
                    isNew = true;

                    if (oprCate == null)
                    {
                        oprCate = new DocumentCategory();
                        oprCate.ParentNode = null;
                    }

                    if (cbCate.Text == "文档模板")
                    {
                        //cate.ProjectId = projectInfo.Id;
                        oprCate.ProjectName = "知识库";
                        oprCate.ProjectCode = "KB";
                    }
                    else
                    {
                        if (projectInfo != null)
                        {
                            oprCate.ProjectId = projectInfo.Id;
                            oprCate.ProjectName = projectInfo.Name;
                            oprCate.ProjectCode = projectInfo.Code;
                            //oprCate.Code = projectInfo.Code.PadLeft(4, '0');//+ "-" + model.GetCode(typeof(DocumentCategory));
                        }
                    }
                }
                oprCate.Name = txtCateName.Text;
                oprCate.Code = txtCateCode.Text;
                oprCate = model.SaveCateTree(oprCate);
                if (isNew)
                {
                    oprNode.Tag = oprCate.ParentNode;
                    TreeNode tn = this.tvwCateGory.SelectedNode.Nodes.Add(oprCate.Id.ToString(), oprCate.Name.ToString());
                    tn.Name = oprCate.Id;
                    tn.Tag = oprCate;
                    this.tvwCateGory.SelectedNode = tn;
                    tn.Expand();
                }
                else
                {
                    this.tvwCateGory.SelectedNode.Text = oprCate.Name;
                    this.tvwCateGory.SelectedNode.Tag = oprCate;
                }
                //MessageBox.Show("新增成功！");
                btnSave.Enabled = false;
                txtCateName.ReadOnly = true;
                txtCateCode.ReadOnly = true;
            }
            catch (Exception exp)
            {
                MessageBox.Show("增加节点出错：" + exp.Message);
            }
        }

        #region 文档分类 右键菜单 操作
        void tvwCateGory_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tvwCateGory.SelectedNode = e.Node;
                if (oprNode == tvwCateGory.Nodes[0])
                {
                    删除节点.Enabled = false;
                    修改节点.Enabled = false;
                }
                else
                {
                    删除节点.Enabled = true;
                    修改节点.Enabled = true;
                }
                mnuCateGory.Show(tvwCateGory, new Point(e.X, e.Y));
            }
        }
        void mnuCateGory_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == 增加子节点.Name)
            {
                mnuCateGory.Hide();
                txtCateName.ReadOnly = false;
                txtCateCode.ReadOnly = false;
                btnSave.Enabled = true;
                txtCateName.Text = "";
                txtCateCode.Text = "";
                oprCate = new DocumentCategory();
                oprCate.ParentNode = tvwCateGory.SelectedNode.Tag as DocumentCategory;
            }
            else if (e.ClickedItem.Name == 删除节点.Name)
            {
                mnuCateGory.Hide();
                DeleteNode();
            }
            else if (e.ClickedItem.Name == 修改节点.Name)
            {
                mnuCateGory.Hide();
                txtCateName.ReadOnly = false;
                txtCateCode.ReadOnly = false;
                btnSave.Enabled = true;
            }
        }

        void DeleteNode()
        {
            try
            {
                if (!ValideDelete()) return;

                bool reset = false;
                //父节点只有这一个子节点，并且父节点有权限操作，删除后要重新设置父节点tag
                if (tvwCateGory.SelectedNode.Parent.Nodes.Count == 1)
                {
                    reset = true;
                }

                model.DeleteCateTree(oprCate);

                if (reset)
                {
                    DocumentCategory org = model.GetCateTreeById((tvwCateGory.SelectedNode.Parent.Tag as DocumentCategory).Id);
                    if (org != null)
                        tvwCateGory.SelectedNode.Parent.Tag = org;
                }

                //如果复制的节点有勾选的从选中集合中移除
                //if (tvwCategory.SelectedNode.Checked)
                //{
                //    if (listCheckedNode.ContainsKey(tvwCategory.SelectedNode.Name))
                //        listCheckedNode.Remove(tvwCategory.SelectedNode.Name);

                //    RemoveChildChecked(tvwCategory.SelectedNode);
                //}

                this.tvwCateGory.Nodes.Remove(this.tvwCateGory.SelectedNode);
            }
            catch (Exception exp)
            {
                string message = exp.Message;
                Exception ex1 = exp.InnerException;
                while (ex1 != null && !string.IsNullOrEmpty(ex1.Message))
                {
                    message += ex1.Message;

                    ex1 = ex1.InnerException;
                }

                if (message.IndexOf("违反") > -1 && message.IndexOf("约束") > -1)
                {
                    MessageBox.Show("该节点被工程WBS或其它数据所引用，删除前请先删除引用的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("删除节点出错：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }
        private bool ValideDelete()
        {
            try
            {
                TreeNode tn = tvwCateGory.SelectedNode;
                if (tn == null)
                {
                    MessageBox.Show("请先选择要删除的节点！");
                    return false;
                }
                if (tn.Parent == null)
                {
                    MessageBox.Show("根节点不允许删除！");
                    return false;
                }
                string text = "要删除当前选中的节点吗？该操作将连它的所有子节点一同删除！";
                if (MessageBox.Show(text, "删除节点", MessageBoxButtons.YesNo) == DialogResult.No)
                    return false;

                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
                return false;
            }
        }


        #endregion

        void cbCate_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadDocumentCategory();
            tvwCateGory.Nodes.Clear();
            LoadDocumentCategory(null);
            if (tvwCateGory.Nodes.Count > 0)
            {
                tvwCateGory.SelectedNode = tvwCateGory.Nodes[0];
            }
        }

        void tvwCateGory_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            LoadDocumentCategory(e.Node);
        }

        /// <summary>
        /// 加载文档分类
        /// </summary>
        void LoadDocumentCategory()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCateGory.Nodes.Clear();
                //项目文档分类是夸项目的，无需存项目信息
                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();
                if (cbCate.Text == "文档模板")
                {
                    oq.AddCriterion(Expression.Eq("ProjectCode", "KB"));
                }
                else
                {
                    dis.Add(Expression.Not(Expression.Eq("ProjectCode", "KB")));
                    dis.Add(Expression.IsNull("ProjectCode"));
                    oq.AddCriterion(dis);
                }
                //oq.AddOrder(NHibernate.Criterion.Order.Asc("SysCode"));

                oq.AddCriterion(Expression.Eq("State", 1));
                oq.AddOrder(Order.Asc("Level"));
                oq.AddOrder(Order.Asc("OrderNo"));
                IList cateList = model.ObjectQuery(typeof(DocumentCategory), oq);
                if (cateList == null || cateList.Count == 0)
                {
                    DocumentCategory cate = new DocumentCategory();
                    cate.ParentNode = null;
                    if (cbCate.Text == "文档模板")
                    {
                        //cate.ProjectId = projectInfo.Id;
                        cate.ProjectName = "知识库";
                        cate.ProjectCode = "KB";
                    }
                    cate.Name = cbCate.Text + "分类";
                    cate.Code = "0001";
                    cate = model.SaveCateTree(cate);

                    cateList = model.ObjectQuery(typeof(DocumentCategory), oq);
                }
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
                IList cateList = model.GetDocumentCategoryChildList(level, sysCode, cbCate.Text == "文档模板" ? true : false, projectId);
                if (node == null && (cateList == null || cateList.Count == 0) && projectInfo != null && !string.IsNullOrEmpty(projectInfo.Id))
                {
                    DocumentCategory cate = new DocumentCategory();
                    cate.ParentNode = null;
                    if (cbCate.Text == "文档模板")
                    {
                        //cate.ProjectId = projectInfo.Id;
                        cate.ProjectName = "知识库";
                        cate.ProjectCode = "KB";
                    }
                    else
                    {
                        cate.ProjectId = projectInfo.Id;
                        cate.ProjectName = projectInfo.Name;
                        cate.ProjectCode = projectInfo.Code;
                    }
                    cate.Name = cbCate.Text + "分类";
                    cate.Code = "0001";
                    cate = model.SaveCateTree(cate);

                    cateList = model.GetDocumentCategoryChildList(level, sysCode, cbCate.Text == "文档模板" ? true : false, projectId);
                }
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

        bool ValideSave()
        {
            if (txtCateName.Text.Trim() == "")
            {
                MessageBox.Show("文档分类名称不能为空！");
                return false;
            }
            if (txtCateCode.Text.Trim() == "")
            {
                MessageBox.Show("文档分类代码不能为空！");
                return false;
            }
            return true;
        }
        #endregion

        #region 文档 文件 操作
        #region 文档主表
        void dgDocumentMast_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgDocumentDetail.Rows.Clear();
            DocumentMaster docMaster = dgDocumentMast.Rows[e.RowIndex].Tag as DocumentMaster;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", docMaster.Id));
            oq.AddFetchMode("TheFileCabinet", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof(DocumentDetail), oq);
            if (list != null && list.Count > 0)
            {
                foreach (DocumentDetail docDetail in list)
                {
                    AddDgDocumentDetailInfo(docDetail);
                }
            }
        }
        //新增
        void btnDocumentMaterAdd_Click(object sender, EventArgs e)
        {
            bool isMode = false;
            isMode = cbCate.Text == "文档模板" ? true : false;
            VDocumentMasterInfo frm = new VDocumentMasterInfo(null, oprCate, isMode);
            frm.ShowDialog();
            DocumentMaster resultMater = frm.Result;
            if (resultMater != null)
            {
                AddDgDocumentMastInfo(resultMater);
                dgDocumentMast.CurrentCell = dgDocumentMast[2, dgDocumentMast.Rows.Count - 1];
                dgDocumentMast_CellClick(dgDocumentMast, new DataGridViewCellEventArgs(2, dgDocumentMast.Rows.Count - 1));
            }
        }
        //修改
        void btnDocumentMasterUpdate_Click(object sender, EventArgs e)
        {
            if (dgDocumentMast.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要修改的文档！");
                return;
            }
            DocumentMaster master = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
            bool isMode = false;
            isMode = cbCate.Text == "文档模板" ? true : false;
            VDocumentMasterInfo frm = new VDocumentMasterInfo(master, oprCate, isMode);
            frm.ShowDialog();
            DocumentMaster resultMater = frm.Result;
            if (resultMater != null)
            {
                AddDgDocumentMastInfo(resultMater, dgDocumentMast.SelectedRows[0].Index);
            }
        }
        //删除
        void btnDocumentMasterDelete_Click(object sender, EventArgs e)
        {
            if (dgDocumentMast.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的文档！");
                return;
            }
            if (MessageBox.Show("要删除当前文档吗？该操作将连它的所有文件一同删除！", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DocumentMaster mas = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
                IList list = new ArrayList();
                list.Add(mas);
                if (model.Delete(list))
                {
                    MessageBox.Show("删除成功！");
                    dgDocumentMast.Rows.Remove(dgDocumentMast.SelectedRows[0]);
                }
                else
                {
                    MessageBox.Show("删除失败！");
                }
            }
        }

        #region （已注释）
        //void dgDocumentMast_MouseClick(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        mnuDocumentMaster.Items[文档新增.Name].Enabled = true;
        //        mnuDocumentMaster.Items[文档修改.Name].Enabled = false;
        //        mnuDocumentMaster.Items[文档删除.Name].Enabled = false;
        //        mnuDocumentMaster.Show(MousePosition.X, MousePosition.Y);
        //    }
        //}

        //void dgDocumentMast_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        if (e.RowIndex >= 0)
        //        {
        //            mnuDocumentMaster.Items[文档新增.Name].Enabled = true;
        //            mnuDocumentMaster.Items[文档修改.Name].Enabled = true;
        //            mnuDocumentMaster.Items[文档删除.Name].Enabled = true;
        //            mnuDocumentMaster.Show(MousePosition.X, MousePosition.Y);
        //        }
        //    }
        //}

        //void mnuDocumentMaster_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        //{
        //    if (e.ClickedItem.Name == 文档新增.Name)
        //    {
        //        mnuDocumentMaster.Hide();
        //        VDocumentMasterInfoAdd frm = new VDocumentMasterInfoAdd(null, oprCate);
        //        frm.ShowDialog();
        //        DocumentMaster resultMater = frm.Result;
        //        if (resultMater != null)
        //        {
        //            AddDgDocumentMastInfo(resultMater);
        //            dgDocumentMast.CurrentCell = dgDocumentMast[2, dgDocumentMast.Rows.Count - 1];
        //            dgDocumentMast_CellClick(dgDocumentMast, new DataGridViewCellEventArgs(2, dgDocumentMast.Rows.Count - 1));
        //        }
        //    }
        //    else if (e.ClickedItem.Name == 文档修改.Name)
        //    {
        //        mnuDocumentMaster.Hide();
        //        DocumentMaster master = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
        //        VDocumentMasterInfoUpdate frm = new VDocumentMasterInfoUpdate(master, oprCate);
        //        frm.ShowDialog();
        //        DocumentMaster resultMater = frm.Result;
        //        if (resultMater != null)
        //        {
        //            AddDgDocumentMastInfo(resultMater, dgDocumentMast.SelectedRows[0].Index);
        //        }
        //    }
        //    else if (e.ClickedItem.Name == 文档删除.Name)
        //    {
        //        mnuDocumentMaster.Hide();
        //        if (MessageBox.Show("要删除当前文档吗？该操作将连它的所有文件一同删除！", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //        {
        //            DocumentMaster mas = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
        //            IList list = new ArrayList();
        //            list.Add(mas);
        //            if (model.Delete(list))
        //            {
        //                MessageBox.Show("删除成功！");
        //                dgDocumentMast.Rows.Remove(dgDocumentMast.SelectedRows[0]);
        //            }
        //            else
        //            {
        //                MessageBox.Show("删除失败！");
        //            }
        //        }
        //    }
        //}

        #endregion
        #endregion

        #region 文件
        //新增
        void btnDocumentDetailAdd_Click(object sender, EventArgs e)
        {
            if (dgDocumentMast.Rows.Count == 0) return;

            DocumentMaster docMaster = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
            VDocumentFileUpload frm = new VDocumentFileUpload(docMaster, "add");
            frm.ShowDialog();
            IList resultList = frm.ResultList;
            if (resultList != null && resultList.Count > 0)
            {
                foreach (DocumentDetail dtl in resultList)
                {
                    AddDgDocumentDetailInfo(dtl);
                }
            }
        }
        //修改
        void btnDocumentDetailUpdate_Click(object sender, EventArgs e)
        {
            if (dgDocumentMast.Rows.Count == 0)
            {
                MessageBox.Show("请选择要修改的文件！");
                return;
            }
            DocumentMaster docMaster = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
            VDocumentFileModify frm = new VDocumentFileModify(docMaster);
            frm.ShowDialog();
            IList resultList = frm.ResultList;
            if (resultList != null && resultList.Count > 0)
            {
                DocumentDetail dtl = resultList[0] as DocumentDetail;
                AddDgDocumentDetailInfo(dtl, dgDocumentDetail.SelectedRows[0].Index);
            }
        }
        //删除
        void btnDocumentDetailDelete_Click(object sender, EventArgs e)
        {

            List<DataGridViewRow> rowList = new List<DataGridViewRow>();
            IList deleteFileList = new ArrayList();
            foreach (DataGridViewRow row in dgDocumentDetail.Rows)
            {
                if ((bool)row.Cells[FileSelect.Name].EditedFormattedValue)
                {
                    DocumentDetail dtl = row.Tag as DocumentDetail; ;
                    rowList.Add(row);
                    deleteFileList.Add(dtl);
                }
            }
            if (deleteFileList.Count == 0)
            {
                MessageBox.Show("请勾选要删除的数据！");
                return;
            }
            if (MessageBox.Show("要删除当前勾选文件吗！", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (model.Delete(deleteFileList))
                {
                    MessageBox.Show("删除成功！");
                    //DocumentMaster master = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
                    //foreach (DocumentDetail dtl in deleteFileList)
                    //{
                    //    master.ListFiles.Remove(dtl);
                    //}
                    //dgDocumentMast.SelectedRows[0].Tag = master;
                }
                else
                {
                    MessageBox.Show("删除失败！");
                    return;
                }
                if (rowList != null && rowList.Count > 0)
                {
                    foreach (DataGridViewRow row in rowList)
                    {
                        dgDocumentDetail.Rows.Remove(row);
                    }
                }
            }
        }
        //预览
        void btnDocumentDetailShow_Click(object sender, EventArgs e)
        {
            IList list = new ArrayList();
            foreach (DataGridViewRow row in dgDocumentDetail.Rows)
            {
                if ((bool)row.Cells[FileSelect.Name].EditedFormattedValue)
                {
                    DocumentDetail dtl = row.Tag as DocumentDetail;
                    list.Add(dtl);
                }
            }

            if (list.Count == 0)
            {
                MessageBox.Show("勾选文件才能预览，请选择要预览的文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {

                List<string> listFileFullPaths = new List<string>();
                string fileFullPath1 = AppDomain.CurrentDomain.BaseDirectory + "TempFilePreview\\" + Guid.NewGuid().ToString().Replace("-", "");
                foreach (DocumentDetail docFile in list)
                {
                    //DocumentDetail docFile = dgDocumentDetail.SelectedRows[0].Tag as DocumentDetail;
                    if (!Directory.Exists(fileFullPath1))
                        Directory.CreateDirectory(fileFullPath1);

                    string tempFileFullPath = fileFullPath1 + @"\\" + docFile.FileName;

                    //string address = "http://10.70.18.203//TestFile//Files//0001//0001//0001.0001.000027V_特效.jpg";
                    string baseAddress = @docFile.TheFileCabinet.TransportProtocal.ToString().ToLower() + "://" + docFile.TheFileCabinet.ServerName + "/" + docFile.TheFileCabinet.Path + "/";

                    string address = baseAddress + docFile.FilePartPath;
                    UtilityClass.WebClientObj.DownloadFile(address, tempFileFullPath);
                    listFileFullPaths.Add(tempFileFullPath);
                }
                foreach (string fileFullPath in listFileFullPaths)
                {
                    FileInfo file = new FileInfo(fileFullPath);

                    //定义一个ProcessStartInfo实例
                    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                    //设置启动进程的初始目录
                    info.WorkingDirectory = file.DirectoryName;//Application.StartupPath; 
                    //设置启动进程的应用程序或文档名
                    info.FileName = file.Name;
                    //设置启动进程的参数
                    info.Arguments = "";
                    //启动由包含进程启动信息的进程资源
                    try
                    {
                        System.Diagnostics.Process.Start(info);
                    }
                    catch (System.ComponentModel.Win32Exception we)
                    {
                        MessageBox.Show(this, we.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("未将对象引用设置到对象的实例") > -1)
                {
                    MessageBox.Show("操作失败，不存在的文档对象！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //下载
        void btnDocumentDetailDownLoad_Click(object sender, EventArgs e)
        {
            IList downList = new ArrayList();
            foreach (DataGridViewRow row in dgDocumentDetail.Rows)
            {
                if ((bool)row.Cells[FileSelect.Name].EditedFormattedValue)
                {
                    DocumentDetail dtl = row.Tag as DocumentDetail;
                    downList.Add(dtl);
                }
            }
            if (downList != null && downList.Count > 0)
            {
                VDocumentPublicDownload frm = new VDocumentPublicDownload(downList);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("请勾选要下载的文件！");
                return;
            }
        }

        //反选
        void lnkCheckAllNot_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgDocumentDetail.Rows)
            {
                row.Cells[FileSelect.Name].Value = false;
            }
        }
        //全选
        void lnkCheckAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgDocumentDetail.Rows)
            {
                row.Cells[FileSelect.Name].Value = true;
            }
        }

        #region （已注释）
        //void dgDocumentDetail_MouseClick(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        mnuDocumentDetail.Items[文件新增.Name].Enabled = true;
        //        mnuDocumentDetail.Items[文件修改.Name].Enabled = false;
        //        mnuDocumentDetail.Items[文件删除.Name].Enabled = false;
        //        mnuDocumentDetail.Show(MousePosition.X, MousePosition.Y);
        //    }
        //}

        //void dgDocumentDetail_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        if (e.RowIndex >= 0)
        //        {
        //            mnuDocumentDetail.Items[文件新增.Name].Enabled = true;
        //            mnuDocumentDetail.Items[文件修改.Name].Enabled = true;
        //            mnuDocumentDetail.Items[文件删除.Name].Enabled = true;
        //            mnuDocumentDetail.Show(MousePosition.X, MousePosition.Y);
        //        }
        //    }
        //}

        //void mnuDocumentDetail_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        //{
        //    if (e.ClickedItem.Name == 文件新增.Name)
        //    {
        //        mnuDocumentDetail.Hide();
        //        DocumentMaster docMaster = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
        //        VDocumentFileUpload frm = new VDocumentFileUpload(docMaster);

        //    }
        //    else if (e.ClickedItem.Name == 文件修改.Name)
        //    {
        //        mnuDocumentDetail.Hide();
        //    }
        //    else if (e.ClickedItem.Name == 文件删除.Name)
        //    {
        //        mnuDocumentDetail.Hide();
        //        if (MessageBox.Show("要删除当前文件吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //        {
        //            //DocumentMaster mas = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
        //            //IList list = new ArrayList();
        //            //list.Add(mas);
        //            //if (model.Delete(list))
        //            //{
        //            //    MessageBox.Show("删除成功！");
        //            //    dgDocumentMast.Rows.Remove(dgDocumentMast.SelectedRows[0]);
        //            //}
        //            //else
        //            //{
        //            //    MessageBox.Show("删除失败！");
        //            //}
        //        }
        //    }
        //}
        #endregion

        #endregion

        #region 列表里添加数据
        void AddDgDocumentMastInfo(DocumentMaster m)
        {
            int rowIndex = dgDocumentMast.Rows.Add();
            AddDgDocumentMastInfo(m, rowIndex);
        }
        void AddDgDocumentMastInfo(DocumentMaster m, int rowIndex)
        {
            dgDocumentMast[colDocumentName.Name, rowIndex].Value = m.Name;
            dgDocumentMast[colCreateTime.Name, rowIndex].Value = m.CreateTime;
            dgDocumentMast[colDocumentCode.Name, rowIndex].Value = m.Code;
            dgDocumentMast[colDocumentInforType.Name, rowIndex].Value = m.DocType.ToString();
            dgDocumentMast[colDocumentState.Name, rowIndex].Value = ClientUtil.GetDocStateName(m.State);
            dgDocumentMast[colOwnerName.Name, rowIndex].Value = m.OwnerName;
            dgDocumentMast.Rows[rowIndex].Tag = m;
        }

        void AddDgDocumentDetailInfo(DocumentDetail d)
        {
            int rowIndex = dgDocumentDetail.Rows.Add();
            AddDgDocumentDetailInfo(d, rowIndex);
        }
        void AddDgDocumentDetailInfo(DocumentDetail d, int rowIndex)
        {
            dgDocumentDetail[FileName.Name, rowIndex].Value = d.FileName;
            dgDocumentDetail[FileExtension.Name, rowIndex].Value = d.ExtendName;
            dgDocumentDetail[FilePath.Name, rowIndex].Value = d.FilePartPath;
            dgDocumentDetail.Rows[rowIndex].Tag = d;
        }
        #endregion
        #endregion
    }
}
