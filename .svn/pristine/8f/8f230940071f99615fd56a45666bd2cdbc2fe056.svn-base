using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

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
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VSelectGWBSTree_OnlyOne : TBasicDataView
    {
        CurrentProjectInfo oProjectInfo = null;
        /// <summary>当前属于非项目时 可以设置当前项目 </summary>
      public   CurrentProjectInfo ProjectInfo
        {
            get
            {
                return oProjectInfo == null ? StaticMethod.GetProjectInfo() : oProjectInfo;
            }
            set
            {
                oProjectInfo = value;
            }
        }
        public SelectNodeMethod SelectMethod = SelectNodeMethod.树状结构选择;
        private Color backColor = ColorTranslator.FromHtml("#D7E8FE");
        private Color ForColor = ColorTranslator.FromHtml("#000000");

        /// <summary>
        /// 拷贝的深度，0代表所有
        /// </summary>
        private int SelectCopyLevel = 0;

        private bool isSelectTreeNodes = false;
        private GWBSTree oprNode = null;

        //有权限的节点
        private IList lstInstance;

        private Hashtable hashtableRules = new Hashtable();

        private Dictionary<string, TreeNode> listCheckedNode = new Dictionary<string, TreeNode>();
        private List<TreeNode> listCopyNode = new List<TreeNode>();
        private bool isCheck = false;

        /// <summary>
        /// 树上是否显示checkbox
        /// </summary>
        public bool IsCheck
        {
            get { return isCheck; }
            set
            {
                isCheck = value;
                tvwCategory.CheckBoxes = isCheck;
            }
        }

        private List<TreeNode> _SelectResult = new List<TreeNode>();

        /// <summary>
        /// 获取选择的节点集合
        /// </summary>
        public List<TreeNode> SelectResult
        {
            get { return _SelectResult; }
            set { _SelectResult = value; }
        }

        /// <summary>
        /// 拷贝深度【0：表示所有】
        /// </summary>
        public int RtnCopyLevel = 0;

        /// <summary>
        /// 是否显示拷贝层级【避免影响其他功能，故加此属性，默认为否】
        /// </summary>
        private bool IsShowCopyLevel = false;

        private bool _IsTreeSelect = false;

        /// <summary>
        /// 是否是子树选择
        /// </summary>
        public bool IsTreeSelect
        {
            get { return _IsTreeSelect; }
            set { _IsTreeSelect = value; }
        }

        private bool _IsSelectSingleNode = false;

        /// <summary>
        /// 是否是单节点选择
        /// </summary>
        public bool IsSelectSingleNode
        {
            get { return _IsSelectSingleNode; }
            set { _IsSelectSingleNode = value; }
        }

        /// <summary>
        /// 决定分层加载的树，点击确认后返回的节点上有自己的子节点
        /// </summary>
        public bool IsSelectTreeNodes
        {
            get { return isSelectTreeNodes; }
            set { isSelectTreeNodes = value; }
        }

        /// <summary>
        // 选择GWBS范围
        /// </summary>
        private GWBSTree DefaultSelectedGWBSRange = null;

        /// <summary>
        /// 缺省选择的GWBS
        /// </summary>
        public GWBSTree DefaultSelectedGWBS = null;

        private bool isRootNode = false;

        /// <summary>
        /// 在checkbox下是否选择 单选框选中的根节点
        /// </summary>
        public bool IsRootNode
        {
            get { return isRootNode; }
            set { isRootNode = value; }
        }

        public MGWBSTree model;

        public VSelectGWBSTree_OnlyOne()
        {
            model = new MGWBSTree();
            InitializeComponent();

            InitForm();
        }

        public VSelectGWBSTree_OnlyOne(GWBSTree selectGWBSRange)
        {
            model = new MGWBSTree();

            DefaultSelectedGWBSRange = selectGWBSRange;

            InitializeComponent();

            InitForm();
        }

        public VSelectGWBSTree_OnlyOne(MGWBSTree mot)
        {
            model = mot;
            InitializeComponent();

            InitForm();
        }
        public VSelectGWBSTree_OnlyOne(MGWBSTree mot, CurrentProjectInfo ProjectInfo)
        {
            this.ProjectInfo = ProjectInfo;
            model = mot;
            InitializeComponent();

            InitForm();
        }
        public VSelectGWBSTree_OnlyOne(MGWBSTree mot, bool isShowCopyLevel)
        {
            IsShowCopyLevel = isShowCopyLevel;
            model = mot;
            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();

            cbSelectMethod.Items.Add(SelectNodeMethod.树状结构选择.ToString());
            cbSelectMethod.Items.Add(SelectNodeMethod.零散节点选择.ToString());
            cbSelectMethod.SelectedIndex = 0;

            tvwCategory.CheckBoxes = IsCheck;

            txtCopyLevel.Text = "0";

            RefreshState(MainViewState.Browser);

            //LoadGWBSTree(null);//在cbSelectMethod_SelectedIndexChanged事件中初始化树控件
        }

        private void InitEvents()
        {
            tvwCategory.AfterCheck += new TreeViewEventHandler(tvwCategory_AfterCheck);
            tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);
            tvwCategory.AfterCollapse += new TreeViewEventHandler(tvwCategory_AfterCollapse);
            tvwCategory.BeforeExpand += new TreeViewCancelEventHandler(tvwCategory_BeforeExpand);

            btnEnter.Click += new EventHandler(btnEnter_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            cbSelectMethod.SelectedIndexChanged += new EventHandler(cbSelectMethod_SelectedIndexChanged);

            this.Load += new EventHandler(VSelectGWBSTree_Load);
            this.FormClosing += new FormClosingEventHandler(VSelectGWBSTree_FormClosing);
        }


        private void VSelectGWBSTree_Load(object sender, EventArgs e)
        {
            if (_IsSelectSingleNode)
            {
                lblAlert.Visible = false;
                lblcopyLevel.Visible = false;
                lblCopyMethod.Visible = false;
                lblLevel.Visible = false;

                txtCopyLevel.Visible = false;
                cbSelectMethod.Visible = false;
            }
            else if (_IsTreeSelect)
            {
                cbSelectMethod.Visible = false;
                lblCopyMethod.Visible = false;
            }

            #region 是否显示拷贝层级

            txtCopyLevel.Visible = IsShowCopyLevel;
            lblAlert.Visible = IsShowCopyLevel;
            lblcopyLevel.Visible = IsShowCopyLevel;
            lblLevel.Visible = IsShowCopyLevel;

            #endregion

            if (DefaultSelectedGWBS != null)
            {
                foreach (TreeNode tn in tvwCategory.Nodes)
                {
                    if (tn.Name == DefaultSelectedGWBS.Id)
                    {
                        tvwCategory.SelectedNode = tn;
                        if (SelectMethod == SelectNodeMethod.零散节点选择)
                            tn.Checked = true;

                        TreeNode theParentNode = tn.Parent;
                        while (theParentNode != null)
                        {
                            theParentNode.Expand();
                            theParentNode = theParentNode.Parent;
                        }
                        break;
                    }
                    if (SetDefaultSelectedNode(tn))
                        break;
                }
            }
        }

        private bool SetDefaultSelectedNode(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Name == DefaultSelectedGWBS.Id)
                {
                    tvwCategory.SelectedNode = tn;
                    if (SelectMethod == SelectNodeMethod.零散节点选择)
                        tn.Checked = true;

                    TreeNode theParentNode = tn.Parent;
                    while (theParentNode != null)
                    {
                        theParentNode.Expand();
                        theParentNode = theParentNode.Parent;
                    }
                    return true;
                }
                SetDefaultSelectedNode(tn);
            }

            return false;
        }

        public bool isOK = false;

        private void btnEnter_Click(object sender, EventArgs e)
        {
            RtnCopyLevel = ClientUtil.ToInt(txtCopyLevel.Text.Trim());
            if (_IsSelectSingleNode) //单节点选择
            {
                if (oprNode == null)
                {
                    MessageBox.Show("请选择一个节点", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                TreeNode selectNode = tvwCategory.SelectedNode;
                if (!CheckValdiate(selectNode))
                {
                    return;
                }
                if (selectNode.Nodes.Count > 0)
                    selectNode.Nodes.Clear();
                if (IsSelectTreeNodes)
                {
                    CurrentProjectInfo projectInfo = ProjectInfo;// StaticMethod.GetProjectInfo();
                    GWBSTree oParentNode = selectNode.Tag as GWBSTree;
                    TreeNode oNode = null;
                    IList lst = model.GetGWBSTreesByLevel(projectInfo.Id, oParentNode.Level, oParentNode.SysCode);
                    if (lst != null && lst.Count > 0)
                    {
                        Hashtable ht = new Hashtable();
                        //oNode = new TreeNode();
                        //oNode.Text = selectNode.Text;
                        //oNode.Tag = selectNode.Tag;
                        ht.Add(oParentNode.Id, selectNode);

                        TreeNode oParentNodeTmp = null;
                        GWBSTree oChildNode = null;
                        // foreach (GWBSTree oChildNode in lst)
                        for (int i = 0; i < lst.Count; i++)
                        {
                            oChildNode = lst[i] as GWBSTree;
                            if (oChildNode.ParentNode != null)
                            {
                                oNode = new TreeNode();
                                oNode.Tag = oChildNode;
                                oNode.Text = oChildNode.Name;
                                if (ht.ContainsKey(oChildNode.ParentNode.Id))
                                {
                                    oParentNodeTmp = ht[oChildNode.ParentNode.Id] as TreeNode;
                                    oParentNodeTmp.Nodes.Add(oNode);
                                    ht.Add(oChildNode.Id, oNode);
                                }


                            }
                        }
                        selectNode = ht[oParentNode.Id] as TreeNode;
                    }
                }
                SelectResult.Add(selectNode);
            }
            else
            {
                // if (  cbSelectMethod.SelectedItem.ToString() == SelectNodeMethod.树状结构选择.ToString()) old
                if (this.isCheck == false &&
                    cbSelectMethod.SelectedItem.ToString() == SelectNodeMethod.树状结构选择.ToString())
                {
                    if (oprNode == null)
                    {
                        MessageBox.Show("请选择一个节点", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        if (txtCopyLevel.Text.Trim() == "")
                        {
                            MessageBox.Show("请输入拷贝深度", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtCopyLevel.Focus();
                            return;
                        }
                        SelectCopyLevel = Convert.ToInt32(txtCopyLevel.Text);

                        if (SelectCopyLevel < 0)
                        {
                            MessageBox.Show("拷贝深度请必须大于或等于0！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("拷贝深度输入格式不正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    TreeNode selectNode = tvwCategory.SelectedNode;
                    if (!CheckValdiate(selectNode))
                    {
                        return;
                    }
                    if (SelectCopyLevel > 0)
                    {
                        if (SelectCopyLevel == 1)
                            selectNode.Nodes.Clear();
                        else
                        {
                            foreach (TreeNode tn in selectNode.Nodes)
                            {
                                int level = 1;
                                SetCopyLevel(tn, level);
                            }
                        }
                    }

                    SelectResult.Add(selectNode);
                }
                else
                {
                    if (listCheckedNode.Count == 0)
                    {
                        MessageBox.Show("请至少选择一个节点", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (IsRootNode)
                    {
                        SelectResult = GetTopCheckNode();
                    }
                    else
                    {
                        foreach (var dic in listCheckedNode)
                        {
                            if (!CheckValdiate(dic.Value))
                            {
                                continue;
                            }
                            SelectResult.Add(dic.Value);
                        }
                    }
                }
            }

            isOK = true;
            this.Close();
        }

        public bool CheckValdiate(TreeNode oNode)
        {
            GWBSTree oGWBSTree = oNode.Tag as GWBSTree;
            if (oGWBSTree == null)
            {
                return false;
            }
            CurrentProjectInfo projectInfo = ProjectInfo;// StaticMethod.GetProjectInfo();
            if (projectInfo.ProjectInfoState == EnumProjectInfoState.新项目)
            {
                bool bResult = bResult = oGWBSTree.TaskState == DocumentState.InExecute;
                if (!bResult)
                    MessageBox.Show(string.Format("[{0}]节点为未发布状态,无法被引用,请重新选择", oGWBSTree.Name), "提示",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
            }
            return true;
        }

        public List<TreeNode> GetTopCheckNode()
        {
            List<TreeNode> lst = new List<TreeNode>();
            foreach (TreeNode oNode in this.tvwCategory.Nodes)
            {
                GetTopCheckNode(lst, oNode);
            }
            return lst;
        }

        public void GetTopCheckNode(List<TreeNode> lst, TreeNode oNode)
        {
            if (oNode != null)
            {
                if (oNode.Checked)
                {
                    lst.Add(oNode);
                }
                else
                {
                    foreach (TreeNode oChildNode in oNode.Nodes)
                    {
                        GetTopCheckNode(lst, oChildNode);
                    }
                }
            }
        }

        private void SetCopyLevel(TreeNode parentNode, int level)
        {
            if (parentNode.Nodes.Count == 0)
                return;

            level += 1;
            if (level == SelectCopyLevel && parentNode.Nodes.Count > 0)
            {
                parentNode.Nodes.Clear();
            }

            foreach (TreeNode node in parentNode.Nodes)
            {
                SetCopyLevel(node, level);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SelectResult.Clear();
            this.Close();
        }

        private void VSelectGWBSTree_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isOK)
                SelectResult.Clear();
        }

        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                oprNode = tvwCategory.SelectedNode.Tag as GWBSTree;
                oprNode = LoadRelaPBS(oprNode);
                this.GetNodeDetail();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
            }
        }

        private void tvwCategory_AfterCheck(object sender, TreeViewEventArgs e)
        {
            tvwCategory.AfterCheck -= new TreeViewEventHandler(tvwCategory_AfterCheck);
            if ((Control.ModifierKeys & Keys.Shift) != 0 || (Control.ModifierKeys & Keys.Control) != 0)
                //如果同时按下了Ctrl //(Control.ModifierKeys & Keys.Shift) != 0 || 
            {
                if (e.Node.Checked)
                {
                    e.Node.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    e.Node.ForeColor = ColorTranslator.FromHtml("#000000");

                    //e.Node.BackColor = SystemColors.Control;
                    //e.Node.ForeColor = SystemColors.ControlText;

                    if (listCheckedNode.ContainsKey(e.Node.Name))
                        listCheckedNode[e.Node.Name] = e.Node;
                    else
                        listCheckedNode.Add(e.Node.Name, e.Node);
                }
                else
                {
                    TreeNode tempNode = new TreeNode();
                    e.Node.BackColor = tempNode.BackColor;
                    e.Node.ForeColor = tempNode.ForeColor;

                    if (listCheckedNode.ContainsKey(e.Node.Name))
                        listCheckedNode.Remove(e.Node.Name);
                }


            }
            else
            {
                if (e.Node.Checked)
                {
                    e.Node.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    e.Node.ForeColor = ColorTranslator.FromHtml("#000000");

                    //e.Node.BackColor = SystemColors.Control;
                    //e.Node.ForeColor = SystemColors.ControlText;

                    if (listCheckedNode.ContainsKey(e.Node.Name))
                        listCheckedNode[e.Node.Name] = e.Node;
                    else
                        listCheckedNode.Add(e.Node.Name, e.Node);
                }
                else
                {
                    TreeNode tempNode = new TreeNode();
                    e.Node.BackColor = tempNode.BackColor;
                    e.Node.ForeColor = tempNode.ForeColor;

                    if (listCheckedNode.ContainsKey(e.Node.Name))
                        listCheckedNode.Remove(e.Node.Name);
                }
            }
            SetChildChecked(e.Node);
            if (!e.Node.Checked)
            {
                SetParentChecked(e.Node);
            }
            RefreshControls(MainViewState.Check);
            tvwCategory.AfterCheck += new TreeViewEventHandler(tvwCategory_AfterCheck);
        }

        private void SetChildChecked(TreeNode parentNode)
        {

            foreach (TreeNode tn in parentNode.Nodes)
            {
                tn.Checked = parentNode.Checked;
                SetChildChecked(tn);


                if (tn.Checked)
                {
                    tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    tn.ForeColor = ColorTranslator.FromHtml("#000000");

                    //tn.BackColor = SystemColors.Control;
                    //tn.ForeColor = SystemColors.ControlText;

                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode[tn.Name] = tn;
                    else
                        listCheckedNode.Add(tn.Name, tn);
                }
                else
                {
                    TreeNode tempNode = new TreeNode();
                    tn.BackColor = tempNode.BackColor;
                    tn.ForeColor = tempNode.ForeColor;

                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode.Remove(tn.Name);
                }
            }
        }

        private void SetParentChecked(TreeNode oChildNode)
        {
            if (!oChildNode.Checked)
            {
                TreeNode oParentNode = oChildNode.Parent;
                if (oParentNode != null)
                {
                    TreeNode tempNode = new TreeNode();
                    oParentNode.BackColor = tempNode.BackColor;
                    oParentNode.ForeColor = tempNode.ForeColor;
                    oParentNode.Checked = false;
                    if (listCheckedNode.ContainsKey(oParentNode.Name))
                        listCheckedNode.Remove(oParentNode.Name);
                    SetParentChecked(oParentNode);
                }
            }

        }

        private void GetChildChecked(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Checked)
                {
                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode[tn.Name] = tn;
                    else
                        listCheckedNode.Add(tn.Name, tn);
                }
                GetChildChecked(tn);
            }
        }

        private void RemoveChildChecked(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                RemoveChildChecked(tn);
                if (tn.Checked)
                {
                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode.Remove(tn.Name);
                }
            }
        }

        private GWBSTree LoadRelaPBS(GWBSTree wbs)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", wbs.Id));
            oq.AddFetchMode("ListRelaPBS", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ProjectTaskTypeGUID", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof (GWBSTree), oq);
            if (list.Count > 0)
            {
                wbs = list[0] as GWBSTree;
            }
            return wbs;
        }

        private void GetNodeDetail()
        {
            try
            {
                ClearAll();

                this.txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath;

                this.txtName.Text = oprNode.Name;
                this.txtCode.Text = oprNode.Code;

                this.txtType.Text = oprNode.ProjectTaskTypeGUID.Name;

                if (oprNode.ListRelaPBS.Count > 0)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddFetchMode("ThePBS", NHibernate.FetchMode.Eager);

                    Disjunction dis = new Disjunction();
                    foreach (GWBSRelaPBS rela in oprNode.ListRelaPBS)
                    {
                        dis.Add(Expression.Eq("Id", rela.Id));
                    }
                    oq.AddCriterion(dis);

                    IList listRela = model.ObjectQuery(typeof (GWBSRelaPBS), oq);

                    foreach (GWBSRelaPBS rela in listRela)
                    {
                        this.cbRelaPBS.Items.Add(rela.ThePBS);
                    }
                    cbRelaPBS.DisplayMember = "Name";
                    cbRelaPBS.SelectedIndex = 0;
                }

                this.txtDesc.Text = oprNode.Describe;

                RefreshControls(MainViewState.Browser);
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
            this.txtType.Text = "";
            this.cbRelaPBS.Items.Clear();
            this.txtDesc.Text = "";
        }

        private void LoadGWBSTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();
                CurrentProjectInfo projectInfo = ProjectInfo;// StaticMethod.GetProjectInfo();
                IList list = model.GetGWBSTreesByInstance(projectInfo.Id);

                //lstInstance = listAll[1] as IList;
                //IList list = listAll[0] as IList;
                foreach (GWBSTree childNode in list)
                {
                    //if (childNode.State == 0)
                    //    continue;

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
                MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// 加载当前节点的子节点
        /// </summary>
        /// <param name="oNode"></param>
        private void LoadGWBSTree(TreeNode oNode)
        {
            // Hashtable hashtable = new Hashtable();
            try
            {
                //tvwCategory.Nodes.Clear();
                CurrentProjectInfo projectInfo = ProjectInfo;//  StaticMethod.GetProjectInfo();
                int iLevel = 1;
                string sSysCode = string.Empty;
                if (oNode != null)
                {
                    GWBSTree oTree = oNode.Tag as GWBSTree;
                    iLevel = oTree.Level + 1;
                    sSysCode = oTree.SysCode;
                    oNode.Nodes.Clear();
                }

                IList list = model.GetGWBSTreesByInstance(projectInfo.Id, sSysCode, iLevel);
                // IList list = model.GetGWBSTreesByInstance(projectInfo.Id);
                // IList list = model.GetGWBSTreesByInstance(projectInfo.Id);
                //lstInstance = listAll[1] as IList;
                //IList list = listAll[0] as IList;
                tvwCategory.AfterCheck -= new TreeViewEventHandler(tvwCategory_AfterCheck);
                foreach (GWBSTree childNode in list)
                {
                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = childNode.Name;
                    tnTmp.Tag = childNode;
                    SetNodeStyle(tnTmp);
                    if (childNode.CategoryNodeType != NodeType.LeafNode) //不为叶节点 就添加一个空节点
                    {
                        tnTmp.Nodes.Add("虚拟节点（懒加载用）");
                    }
                    if (oNode != null)
                    {
                        oNode.Nodes.Add(tnTmp);
                        if (oNode.Checked)
                        {
                            tnTmp.Checked = true;
                            if (!listCheckedNode.ContainsKey(tnTmp.Name))
                            {
                                listCheckedNode.Add(tnTmp.Name, tnTmp);
                            }
                        }

                    }
                    else
                    {
                        tvwCategory.Nodes.Add(tnTmp);
                    }
                }

                tvwCategory.AfterCheck += new TreeViewEventHandler(tvwCategory_AfterCheck);
            }
            catch (Exception e)
            {
                MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        private void tvwCategory_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            try
            {
                //if (e.Node != null)
                //{
                //    e.Node.Nodes.Clear();
                //    e.Node.Nodes.Add("Test");
                //}
            }
            catch
            {
            }
        }

        private void tvwCategory_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                if (e.Node != null)
                {
                    if (e.Node.Nodes.Count > 0)
                    {
                        if (e.Node.Nodes[0].Tag != null)
                        {
                        }
                        else
                        {
                            LoadGWBSTree(e.Node);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        public void SetNodeStyle(TreeNode oNode)
        {
            if (oNode != null)
            {
                GWBSTree oGWBSTree = null;
                oGWBSTree = oNode.Tag as GWBSTree;
                if (oGWBSTree != null)
                {
                    if (oGWBSTree.TaskState != DocumentState.InExecute)
                    {
                        oNode.BackColor = backColor;
                        oNode.ForeColor = ForColor;
                        oNode.ToolTipText = string.Format("{0}[未发布]无法选择", oNode.Text);
                        // oNode.Text = string.Format("{0}[未发布]", oNode.Text);
                    }
                }
                // selectedNode.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                //  selectedNode.ForeColor = ColorTranslator.FromHtml("#000000");
            }
        }

        public override void RefreshControls(MainViewState state)
        {
            switch (state)
            {
                case MainViewState.Modify:

                    break;

                case MainViewState.Browser:

                    break;
            }
        }

        private void GetCopyNode(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Checked)
                {
                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode[tn.Name] = tn;
                    else
                        listCheckedNode.Add(tn.Name, tn);
                }
                else
                {
                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode.Remove(tn.Name);
                }
                GetCopyNode(tn);
            }
        }

        //拷贝方式
        private void cbSelectMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSelectMethod.SelectedItem.ToString() == SelectNodeMethod.零散节点选择.ToString())
            {
                tvwCategory.CheckBoxes = true;
                SelectMethod = SelectNodeMethod.零散节点选择;


                lblAlert.Visible = false;
                lblcopyLevel.Visible = false;
                lblLevel.Visible = false;

                txtCopyLevel.Visible = false;
            }
            else
            {
                tvwCategory.CheckBoxes = false;
                SelectMethod = SelectNodeMethod.树状结构选择;


                //lblAlert.Visible = true;
                //lblcopyLevel.Visible = true;
                //lblLevel.Visible = true;

                //txtCopyLevel.Visible = true;
            }

            TreeNode tn = null;
            if (DefaultSelectedGWBSRange != null)
            {
                tn = new TreeNode();
                tn.Name = DefaultSelectedGWBSRange.Id;
                tn.Text = DefaultSelectedGWBSRange.Name;
                tn.Tag = DefaultSelectedGWBSRange;

                tvwCategory.Nodes.Add(tn);
            }
            LoadGWBSTree(tn);
        }
    }
}
