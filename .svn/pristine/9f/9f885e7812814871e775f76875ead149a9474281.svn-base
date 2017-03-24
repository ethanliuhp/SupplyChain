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
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.MobileManage.OftenWords;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using IRPServiceModel.Domain.Document;
using VirtualMachine.Component.Util;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng
{
    public partial class VDocumentsSelect : TBasicDataView
    {
        private MDocumentCategory model = null;
        private CurrentProjectInfo projectInfo = null;
        private TreeNode oprNode;//当前操作节点
        private string isAddOrUpdateOp = string.Empty;
        private IList resultList;
        public IList ResultList
        {
            get { return resultList; }
            set { resultList = value; }
        }

        public VDocumentsSelect()
        {
            InitializeComponent();
            InitcomboBoxData();
            cbCate.Items.AddRange(new object[] { "项目文档", "文档模版" });
            InitEvent();
            IntData();

            if (projectInfo == null || projectInfo.Id == null)
            {
                btnSelect.Enabled = true;
            }
            else
            {
                txtProject.Text = projectInfo.Name;
            }
        }
        /// <summary>
        /// 选择模版  
        /// </summary>
        /// <param name="selectValue">值为“add” 返回结果为多选  值为“update”返回结果为唯一</param>
        public VDocumentsSelect(string selectValue)
        {
            InitializeComponent();
            InitcomboBoxData();
            cbCate.Items.AddRange(new object[] { "文档模版" });
            InitEvent();
            IntData();
            InitControls();
            isAddOrUpdateOp = selectValue;
            if (projectInfo != null)
            {
                txtProject.Text = projectInfo.Name;
            }
        }
        private void InitEvent()
        {
            this.tvwCateGory.AfterSelect += new TreeViewEventHandler(tvwCateGory_AfterSelect);
            this.tvwCateGory.BeforeExpand += new TreeViewCancelEventHandler(tvwCateGory_BeforeExpand);
            this.dgDocumentMast.CellClick += new DataGridViewCellEventHandler(dgDocumentMast_CellClick);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            cbCate.SelectedIndexChanged += new EventHandler(cbCate_SelectedIndexChanged);
            btnSelect.Click += new EventHandler(btnSelect_Click);
        }

        void InitControls()
        {
            DocumentSelect.Visible = true;
            btnOk.Visible = true;
            btnQuit.Visible = true;

            this.btnOk.Click += new EventHandler(btnOk_Click);
            this.btnQuit.Click += new EventHandler(btnQuit_Click);
        }

        /// <summary>
        /// 给复选框默认值
        /// </summary>
        void InitcomboBoxData()
        {
            //文档信息类型
            foreach (string infoType in Enum.GetNames(typeof(DocumentInfoTypeEnum)))
            {
                cmbDocumentInforType.Items.Add(infoType);
            }
            //文档状态
            foreach (DocumentState state in Enum.GetValues(typeof(DocumentState)))
            {
                cmbDocumentStatus.Items.Add(ClientUtil.GetDocStateName(state));
            }
            //文档密级
            foreach (string securityLevel in Enum.GetNames(typeof(DocumentSecurityLevelEnum)))
            {
                cmbSecurityLevel.Items.Add(securityLevel);
            }
            cmbIsOrNo.Items.AddRange(new object[] { "是", "否" });
        }
        void IntData()
        {
            model = new MDocumentCategory();
            //this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            //LoadDocumentCategory();
            cbCate.SelectedIndex = 0;
            cbCate.DropDownStyle = ComboBoxStyle.DropDownList;
            if (tvwCateGory.Nodes.Count > 0)
            {
                tvwCateGory.SelectedNode = tvwCateGory.Nodes[0];
            }
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

                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();
                if (cbCate.Text == "文档模版")
                {
                    oq.AddCriterion(Expression.Eq("ProjectCode", "KB"));
                }
                else
                {
                    dis.Add(Expression.Not(Expression.Eq("ProjectCode", "KB")));
                    dis.Add(Expression.IsNull("ProjectCode"));
                    oq.AddCriterion(dis);
                }
                //oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
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
                IList cateList = model.GetDocumentCategoryChildList(level, sysCode, cbCate.Text == "文档模版" ? true : false,projectId);
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

            DocumentCategory oprCate = e.Node.Tag as DocumentCategory;


            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Category.Id", oprCate.Id));
            //if (oprCate.ProjectCode != "KB")
            //{
            //    oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            //}
            oq.AddFetchMode("ListFiles", NHibernate.FetchMode.Eager);
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

        void dgDocumentMast_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgDocumentDetail.Rows.Clear();
            if (e.RowIndex < 0) return;
            DocumentMaster docMaster = dgDocumentMast.Rows[e.RowIndex].Tag as DocumentMaster;
            if (docMaster.ListFiles != null && docMaster.ListFiles.Count > 0)
            {
                foreach (DocumentDetail docDetail in docMaster.ListFiles)
                {
                    AddDgDocumentDetailInfo(docDetail);
                }
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearch_Click(object sender, EventArgs e)
        {

            FlashScreen.Show("正在查询加载数据，请稍候......");
            DocumentInfoTypeEnum docInfoType = 0;
            foreach (DocumentInfoTypeEnum type in Enum.GetValues(typeof(DocumentInfoTypeEnum)))
            {
                if (type.ToString() == cmbDocumentInforType.Text.Trim())
                {
                    docInfoType = type;
                    break;
                }
            }
            DocumentState docState = DocumentState.Blankness;
            foreach (DocumentState state in Enum.GetValues(typeof(DocumentState)))
            {
                if (ClientUtil.GetDocStateName(state) == cmbDocumentStatus.Text.Trim())
                {
                    docState = state;
                    break;
                }
            }
            DocumentSecurityLevelEnum securityLevel = 0;
            foreach (DocumentSecurityLevelEnum level in Enum.GetValues(typeof(DocumentSecurityLevelEnum)))
            {
                if (level.ToString() == cmbSecurityLevel.Text.Trim())
                {
                    securityLevel = level;
                    break;
                }
            }
            ObjectQuery oq = new ObjectQuery();
            if (docInfoType != 0)
            {
                oq.AddCriterion(Expression.Eq("DocType", docInfoType));
            }
            if (docState != DocumentState.Blankness)
            {
                oq.AddCriterion(Expression.Eq("State", docState));
            }
            if (securityLevel != 0)
            {
                oq.AddCriterion(Expression.Eq("SecurityLevel", securityLevel));
            }
            if (txtDocumentName.Text.Trim() != "")
            {
                oq.AddCriterion(Expression.Like("Name", txtDocumentName.Text,MatchMode.Anywhere));
            }
            if (txtDocumentCode.Text.Trim() != "")
            {
                oq.AddCriterion(Expression.Like("Code", txtDocumentCode.Text,MatchMode.Anywhere));
            }
            if (cmbIsOrNo.Text == "是")
            {
                Disjunction dis = new Disjunction();
                dis.Add(Expression.Not(Expression.Eq("ProjectCode", "KB")));
                dis.Add(Expression.IsNull("ProjectCode"));
                oq.AddCriterion(dis);
                if (projectInfo != null)
                {
                    oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                }
            }
            if (cmbIsOrNo.Text == "否")
            {
                oq.AddCriterion(Expression.Eq("ProjectCode", "KB"));
            }
            if (txtHandlePerson.Text.Trim() != "")
            {
                PersonInfo person = txtHandlePerson.Result[0] as PersonInfo;
                oq.AddCriterion(Expression.Eq("OwnerID.Id", person.Id));
            }
            if (txtKeywordSearch.Text.Trim() != "")
            {
                Disjunction dis = new Disjunction();
                dis.Add(Expression.Like("KeyWords", txtKeywordSearch.Text, MatchMode.Anywhere));
                dis.Add(Expression.Like("Description", txtKeywordSearch.Text, MatchMode.Anywhere));
                oq.AddCriterion(dis);
            }
            oq.AddFetchMode("ListFiles", NHibernate.FetchMode.Eager);
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
            FlashScreen.Close();
        }

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
            dgDocumentMast[colDocumentAuthor.Name, rowIndex].Value = m.Author;
            dgDocumentMast[colKeyword.Name, rowIndex].Value = m.KeyWords;
            dgDocumentMast[colDes.Name, rowIndex].Value = m.Description;
            dgDocumentMast[colSecurityLevel.Name, rowIndex].Value = m.SecurityLevel.ToString();
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
            //dgDocumentDetail[FileExtension.Name, rowIndex].Value = d.ExtendName;
            //dgDocumentDetail[FilePath.Name, rowIndex].Value = d.FilePartPath;
            dgDocumentDetail.Rows[rowIndex].Tag = d;
        }
        #endregion


        //退出
        void btnQuit_Click(object sender, EventArgs e)
        {
            resultList = null;
            this.Close();
        }
        //确定
        void btnOk_Click(object sender, EventArgs e)
        {
            resultList = GetCheckedDocument();
            if (resultList.Count == 0)
            {
                MessageBox.Show("请选择一个模版！");
                return;
            }
            if (isAddOrUpdateOp != "update")
            {
                if (resultList.Count > 1)
                {
                    MessageBox.Show("只能选择一个模版！");
                    return;
                }
            }
            this.Close();
        }
        #region 得到选中文档的集合
        /// <summary>
        /// 得到选中文档的集合
        /// </summary>
        /// <returns></returns>
        IList GetCheckedDocument()
        {
            IList selctedDocList = new ArrayList();
            foreach (DataGridViewRow row in this.dgDocumentMast.Rows)
            {
                if ((bool)row.Cells[DocumentSelect.Name].EditedFormattedValue)
                {
                    DocumentMaster doc = row.Tag as DocumentMaster;
                    selctedDocList.Add(doc);
                }
            }
            return selctedDocList;
        }
        #endregion
    }
}
