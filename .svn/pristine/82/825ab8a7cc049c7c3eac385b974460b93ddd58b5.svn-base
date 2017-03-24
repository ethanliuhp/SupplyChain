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


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VSelectGWBSTreeByAddDetail : TBasicDataView
    {

        public bool isOK = false;

        private GWBSTree oprNode = null;

        //有权限的节点
        private IList lstInstance;

        private Hashtable hashtableRules = new Hashtable();

        private Dictionary<string, TreeNode> listCheckedNode = new Dictionary<string, TreeNode>();

        private List<TreeNode> _SelectTreeNodes = new List<TreeNode>();
        private List<GWBSTree> _SelectResult = new List<GWBSTree>();

        /// <summary>
        /// 获取选中的节点集合
        /// </summary>
        public List<GWBSTree> SelectResult
        {
            get { return _SelectResult; }
            set { _SelectResult = value; }
        }
        /// <summary>
        /// 选择的树节点
        /// </summary>
        public List<TreeNode> SelectTreeNodes
        {
            get { return _SelectTreeNodes; }
            set { _SelectTreeNodes = value; }
        }

        private TreeNodeCollection _SelectTree;
        /// <summary>
        /// 选择的目标工程任务树
        /// </summary>
        public TreeNodeCollection SelectTree
        {
            get { return _SelectTree; }
            set { _SelectTree = value; }
        }

        public MGWBSTree model;

        public VSelectGWBSTreeByAddDetail(MGWBSTree mot)
        {
            model = mot;
            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();

            RefreshState(MainViewState.Browser);


        }

        private void InitEvents()
        {
            tvwCategory.AfterCheck += new TreeViewEventHandler(tvwCategory_AfterCheck);
            tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);
            //tvwCategory.AfterCollapse += new TreeViewEventHandler(tvwCategory_AfterCollapse);
            tvwCategory.BeforeExpand += new TreeViewCancelEventHandler(tvwCategory_BeforeExpand);

            btnEnter.Click += new EventHandler(btnEnter_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            btnQuery.Click += new EventHandler(btnQuery_Click);
            cbCancelChecked.Click += new EventHandler(cbCancelChecked_Click);

            this.Load += new EventHandler(VSelectGWBSTreeByAddDetail_Load);
            this.FormClosing += new FormClosingEventHandler(VSelectGWBSTreeByAddDetail_FormClosing);
        }

        void VSelectGWBSTreeByAddDetail_Load(object sender, EventArgs e)
        {
            //LoadGWBSTree();

            LoadGWBSTree(null);
        }


        void cbCancelChecked_Click(object sender, EventArgs e)
        {
            List<TreeNode> list = new List<TreeNode>();
            foreach (var dic in listCheckedNode)
            {
                list.Add(dic.Value);
            }

            for (int i = 0; i < list.Count; i++)
            {
                list[i].Checked = false;
            }
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            string keyWord = txtKeyWord.Text.Trim();
            if (keyWord == "")
            {
                MessageBox.Show("请输入要过滤的关键字！");
                txtKeyWord.Focus();
                return;
            }

            bool selectedFlag = false;
            foreach (TreeNode tn in tvwCategory.Nodes)
            {
                if (tn.Text.IndexOf(keyWord) > -1)
                {
                    tn.Checked = true;
                    tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    tn.ForeColor = ColorTranslator.FromHtml("#000000");

                    TreeNode theParentNode = tn.Parent;
                    while (theParentNode != null)
                    {
                        theParentNode.Expand();
                        theParentNode = theParentNode.Parent;
                    }

                    selectedFlag = true;
                }
                else
                    tn.Checked = false;

                QueryCheckedTreeNode(tn, keyWord, ref selectedFlag);
            }

            if (selectedFlag)
            {
                cbCancelChecked.Checked = true;
            }
        }

        private void QueryCheckedTreeNode(TreeNode parentNode, string keyWord, ref bool selectedFlag)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Text.IndexOf(keyWord) > -1)
                {
                    tn.Checked = true;
                    tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    tn.ForeColor = ColorTranslator.FromHtml("#000000");

                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode[tn.Name] = tn;
                    else
                        listCheckedNode.Add(tn.Name, tn);

                    TreeNode theParentNode = tn.Parent;
                    while (theParentNode != null)
                    {
                        theParentNode.Expand();
                        theParentNode = theParentNode.Parent;
                    }

                    selectedFlag = true;
                }
                else
                {
                    tn.Checked = false;

                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode.Remove(tn.Name);
                }

                QueryCheckedTreeNode(tn, keyWord, ref selectedFlag);
            }
        }


        void btnEnter_Click(object sender, EventArgs e)
        {
            if (listCheckedNode.Count == 0)
            {
                MessageBox.Show("请至少勾选一个节点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            foreach (var dic in listCheckedNode)
            {
                GWBSTree obj = dic.Value.Tag as GWBSTree;
                SelectResult.Add(obj);
                SelectTreeNodes.Add(dic.Value);
            }

            this.Opacity = 0;
            isOK = true;
            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            listCheckedNode.Clear();
            SelectResult.Clear();
            SelectTreeNodes.Clear();
            this.Close();
        }

        void VSelectGWBSTreeByAddDetail_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isOK)
            {
                listCheckedNode.Clear();
                SelectResult.Clear();
                SelectTreeNodes.Clear();
            }
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

        void tvwCategory_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) != 0 || (Control.ModifierKeys & Keys.Control) != 0)//如果同时按下了Ctrl //(Control.ModifierKeys & Keys.Shift) != 0 || 
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

                SetChildChecked(e.Node);
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

            if (listCheckedNode.Count > 0)
                cbCancelChecked.Checked = true;
            else
                cbCancelChecked.Checked = false;

            RefreshControls(MainViewState.Check);
        }

        private void tvwCategory_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node != null)
                {
                    e.Node.Nodes.Clear();
                    e.Node.Nodes.Add("Test");
                }
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
                    LoadGWBSTree(e.Node);
                }
            }
            catch
            {
            }
        }

        private GWBSTree LoadRelaPBS(GWBSTree wbs)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", wbs.Id));
            oq.AddFetchMode("ListRelaPBS", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof(GWBSTree), oq);
            if (list.Count > 0)
            {
                wbs = list[0] as GWBSTree;
            }
            return wbs;
        }

        private void SetChildChecked(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                SetChildChecked(tn);
                tn.Checked = parentNode.Checked;

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

        private void GetNodeDetail()
        {
            try
            {
                ClearAll();

                //this.txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath;

                this.txtName.Text = oprNode.Name;
                this.txtCode.Text = oprNode.Code;

                this.txtType.Text = oprNode.ProjectTaskTypeName;

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

                    IList listRela = model.ObjectQuery(typeof(GWBSRelaPBS), oq);

                    foreach (GWBSRelaPBS rela in listRela)
                    {
                        this.txtRelaPBS.Items.Add(rela.ThePBS);
                    }
                    txtRelaPBS.DisplayMember = "Name";
                    txtRelaPBS.SelectedIndex = 0;
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
            //this.txtCurrentPath.Text = "";
            this.txtCode.Text = "";
            this.txtName.Text = "";
            this.txtType.Text = "";
            this.txtRelaPBS.Items.Clear();
            this.txtDesc.Text = "";
        }

        private void LoadGWBSTree()
        {
            if (SelectTree != null && SelectTree.Count > 0)
            {
                foreach (TreeNode node in SelectTree)
                {
                    tvwCategory.Nodes.Add(node);
                }
            }
            else
            {
                Hashtable hashtable = new Hashtable();
                try
                {
                    tvwCategory.Nodes.Clear();
                    CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
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
        }
        /// <summary>
        /// 加载当前节点的子节点
        /// </summary>
        /// <param name="oNode"></param>
        private void LoadGWBSTree(TreeNode oNode)
        {
            try
            {
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
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

                foreach (GWBSTree childNode in list)
                {
                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = childNode.Name;
                    tnTmp.Tag = childNode;
                    if (childNode.CategoryNodeType != NodeType.LeafNode)//不为叶节点 就添加一个空节点
                    {
                        tnTmp.Nodes.Add("Test");
                    }
                    if (oNode != null)
                    {
                        oNode.Nodes.Add(tnTmp);

                    }
                    else
                    {
                        tvwCategory.Nodes.Add(tnTmp);
                    }
                }


            }
            catch (Exception e)
            {
                MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(e));
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

        private void GetCheckedNode(TreeNode parentNode)
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
                GetCheckedNode(tn);
            }
        }
    }
}
