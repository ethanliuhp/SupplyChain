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
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VSelectPBSAndTaskType_New : TBasicDataView
    {
        private bool _isOK = false;
        /// <summary>
        /// 是否点击了确定
        /// </summary>
        public bool IsOK
        {
            get { return _isOK; }
            set { _isOK = value; }
        }

        /// <summary>
        /// 父GWBS节点
        /// </summary>
        public TreeNode ParentNode = null;
        private List<string> _parentPBSType = new List<string>();
        /// <summary>
        /// 父对象PBS类型集合
        /// </summary>
        public List<string> ParentPBSType
        {
            get { return _parentPBSType; }
            set { _parentPBSType = value; }
        }
        private string _parentTaskType = "";
        /// <summary>
        /// 父对象任务类型
        /// </summary>
        public string ParentTaskType
        {
            get { return _parentTaskType; }
            set { _parentTaskType = value; }
        }


        /// <summary>
        /// PBS和任务类型组合规则明细
        /// </summary>
        private List<PBSRelaTaskTypeRuleDetail> ListRuleDtl = new List<PBSRelaTaskTypeRuleDetail>();

        //复选选择的节点集合
        private Dictionary<string, TreeNode> listCheckedPBSNode = new Dictionary<string, TreeNode>();
        private Dictionary<string, TreeNode> listCheckedTaskTypeNode = new Dictionary<string, TreeNode>();

        private List<PBSTree> _selectedPBS = new List<PBSTree>();
        private List<ProjectTaskTypeTree> _selectedTaskType = new List<ProjectTaskTypeTree>();
        private List<TreeNode> _selectedPBSFirstNode = new List<TreeNode>();
        private List<TreeNode> _selectedTaskTypeFirstNode = new List<TreeNode>();
        /// <summary>
        /// 确认选择的PBS
        /// </summary>
        public List<PBSTree> SelectedPBS
        {
            get { return _selectedPBS; }
            set { _selectedPBS = value; }
        }
        /// <summary>
        /// 确认选择的任务类型
        /// </summary>
        public List<ProjectTaskTypeTree> SelectedTaskType
        {
            get { return _selectedTaskType; }
            set { _selectedTaskType = value; }
        }
        /// <summary>
        /// 选择的PBS顶级节点
        /// </summary>
        public List<TreeNode> SelectedPBSFirstNode
        {
            get { return _selectedPBSFirstNode; }
            set { _selectedPBSFirstNode = value; }
        }
        /// <summary>
        /// 选择的任务类型顶级节点
        /// </summary>
        public List<TreeNode> SelectedTaskTypeFirstNode
        {
            get { return _selectedTaskTypeFirstNode; }
            set { _selectedTaskTypeFirstNode = value; }
        }


        //当前操作的节点
        private TreeNode optPBSNode;
        private PBSTree optPBSObj;
        private TreeNode optTaskTypeNode;
        private ProjectTaskTypeTree optTaskTypeObj;

        private SelectedPBSAndTaskTypeMode _selectedMode = SelectedPBSAndTaskTypeMode.一级对一级;
        /// <summary>
        /// 选择PBS-任务类型的模式
        /// </summary>
        public SelectedPBSAndTaskTypeMode SelectedMode
        {
            get { return _selectedMode; }
            set { _selectedMode = value; }
        }


        private MPBSTree modelPBS = new MPBSTree();
        private MWBSManagement modelTaskType = new MWBSManagement();


        /// <summary>
        /// 初始PBS集(默认选择的PBS)
        /// </summary>
        public List<PBSTree> InitListPBS = new List<PBSTree>();
        /// <summary>
        /// 初始任务类型(默认选择的任务类型)
        /// </summary>
        public ProjectTaskTypeTree InitTaskType = null;

        //企标颜色标识
        Color guobiaoColor = Color.Black;
        Color qibiaoColor = Color.Blue;

        public VSelectPBSAndTaskType_New()
        {
            InitializeComponent();
            InitEvents();
        }

        private void InitEvents()
        {
            tvwPBS.AfterCheck += new TreeViewEventHandler(tvwPBS_AfterCheck);
            tvwPBS.AfterSelect += new TreeViewEventHandler(tvwPBS_AfterSelect);

            tvwTaskType.AfterCheck += new TreeViewEventHandler(tvwTaskType_AfterCheck);
            tvwTaskType.AfterSelect += new TreeViewEventHandler(tvwTaskType_AfterSelect);

            rbSelectedMode3.CheckedChanged += new EventHandler(rbSelectedMode3_CheckedChanged);

            btnEnter.Click += new EventHandler(btnEnter_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            this.Load += new EventHandler(VSelectPBSAndTaskType_Load);
        }

        void VSelectPBSAndTaskType_Load(object sender, EventArgs e)
        {
            InitForm();
        }

        private void InitForm()
        {
            RefreshState(MainViewState.Browser);

            if (ParentNode == null)
            {
                rbPBSAllTree.Checked = true;
                rbTaskTypeAllTree.Checked = true;
            }
            else
            {
                rbPBSChildTree.Checked = true;
                rbTaskTypeChildTree.Checked = true;
            }


            LoadPBSTree(ParentNode);


            txtGuoBiaoColorFlag.BackColor = guobiaoColor;
            lblGuobiao.ForeColor = guobiaoColor;
            txtQiBiaoColorFlag.BackColor = qibiaoColor;
            lblQibiao.ForeColor = qibiaoColor;

            LoadTaskTypeTree(ParentNode);

            tvwTaskType.CheckBoxes = true;
            tvwPBS.CheckBoxes = true;

            if (ParentNode != null)
            {
                txtParentNodePath.Text = ParentNode.FullPath;
                GWBSTree parent = ParentNode.Tag as GWBSTree;
                txtParentName.Text = parent.Name;
                txtParentTaskType.Text = GetFullPath(parent.ProjectTaskTypeGUID) + "[" + parent.ProjectTaskTypeGUID.TypeLevel + "]";

                foreach (GWBSRelaPBS rela in parent.ListRelaPBS)
                {
                    cbRelaPBS.Items.Add(GetFullPath(rela.ThePBS) + "[" + rela.ThePBS.StructTypeName + "]");
                }
                if (cbRelaPBS.Items.Count > 0)
                    cbRelaPBS.SelectedIndex = 0;
            }

            InitOpenAndSelectedPBSTreeNode();
            InitOpenAndSelectedTaskTypeTreeNode();

            if (InitTaskType != null)
            {
                rbSelectedMode1.Enabled = false;
                rbSelectedMode2.Enabled = false;
                rbSelectedMode3.Enabled = false;
            }
        }
        private void InitOpenAndSelectedPBSTreeNode()
        {
            if (ParentNode != null)
            {
                GWBSTree parent = ParentNode.Tag as GWBSTree;
                foreach (GWBSRelaPBS rela in parent.ListRelaPBS)
                {
                    //初始化打开父节点关联的PBS位置
                    foreach (TreeNode tn in tvwPBS.Nodes)
                    {
                        if (tn.Name == rela.ThePBS.Id)
                        {
                            tvwPBS.SelectedNode = tn;

                            TreeNode theParentNode = tn.Parent;
                            while (theParentNode != null)
                            {
                                theParentNode.Expand();
                                theParentNode = theParentNode.Parent;
                            }
                            break;
                        }
                        if (SetDefaultSelectedNode(tvwPBS, tn, rela.ThePBS.Id))
                            break;
                    }
                }
            }


            if (InitListPBS.Count > 0)
            {
                foreach (PBSTree pbs in InitListPBS)
                {
                    //初始选择的PBS集
                    foreach (TreeNode tn in tvwPBS.Nodes)
                    {
                        if (tn.Name == pbs.Id)
                        {
                            tn.Checked = true;

                            TreeNode theParentNode = tn.Parent;
                            while (theParentNode != null)
                            {
                                theParentNode.Expand();
                                theParentNode = theParentNode.Parent;
                            }
                            break;
                        }
                        if (SetDefaultCheckedNode(tn, pbs.Id))
                            break;
                    }
                }
            }
        }
        private void InitOpenAndSelectedTaskTypeTreeNode()
        {
            if (ParentNode != null)
            {
                GWBSTree parent = ParentNode.Tag as GWBSTree;
                //初始化打开父节点关联的工程任务类型位置
                foreach (TreeNode tn in tvwTaskType.Nodes)
                {
                    if (tn.Name == parent.ProjectTaskTypeGUID.Id)
                    {
                        tvwTaskType.SelectedNode = tn;

                        TreeNode theParentNode = tn.Parent;
                        while (theParentNode != null)
                        {
                            theParentNode.Expand();
                            theParentNode = theParentNode.Parent;
                        }
                        break;
                    }
                    if (SetDefaultSelectedNode(tvwTaskType, tn, parent.ProjectTaskTypeGUID.Id))
                    {
                        break;
                    }
                }
            }

            if (InitTaskType != null)
            {
                //初始选择的任务类型
                foreach (TreeNode tn in tvwTaskType.Nodes)
                {
                    if (tn.Name == InitTaskType.Id)
                    {
                        tvwTaskType.SelectedNode = tn;

                        TreeNode theParentNode = tn.Parent;
                        while (theParentNode != null)
                        {
                            theParentNode.Expand();
                            theParentNode = theParentNode.Parent;
                        }
                        break;
                    }
                    if (SetDefaultSelectedNode(tvwTaskType, tn, InitTaskType.Id))
                    {
                        break;
                    }
                }
            }
        }

        private bool SetDefaultSelectedNode(TreeView tv, TreeNode parentNode, string id)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Name == id)
                {
                    tv.SelectedNode = tn;

                    TreeNode theParentNode = tn.Parent;
                    while (theParentNode != null)
                    {
                        theParentNode.Expand();
                        theParentNode = theParentNode.Parent;
                    }
                    return true;
                }
                SetDefaultSelectedNode(tv, tn, id);
            }

            return false;
        }
        private bool SetDefaultCheckedNode(TreeNode parentNode, string id)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Name == id)
                {
                    tn.Checked = true;

                    TreeNode theParentNode = tn.Parent;
                    while (theParentNode != null)
                    {
                        theParentNode.Expand();
                        theParentNode = theParentNode.Parent;
                    }
                    return true;
                }
                SetDefaultCheckedNode(tn, id);
            }

            return false;
        }
        private string GetFullPath(PBSTree pbs)
        {
            string path = string.Empty;

            path = pbs.Name;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", pbs.Id));
            oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
            IList list = modelPBS.ObjectQuery(typeof(PBSTree), oq);

            pbs = list[0] as PBSTree;

            CategoryNode parent = pbs.ParentNode;
            while (parent != null)
            {
                path = parent.Name + "\\" + path;

                oq.Criterions.Clear();
                oq.Criterions.Clear();
                oq.AddCriterion(Expression.Eq("Id", parent.Id));
                oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
                list = modelPBS.ObjectQuery(typeof(PBSTree), oq);

                parent = (list[0] as CategoryNode).ParentNode;
            }

            return path;
        }
        private string GetFullPath(ProjectTaskTypeTree taskType)
        {
            string path = string.Empty;

            path = taskType.Name;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", taskType.Id));
            oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
            IList list = modelPBS.ObjectQuery(typeof(ProjectTaskTypeTree), oq);

            taskType = list[0] as ProjectTaskTypeTree;

            CategoryNode parent = taskType.ParentNode;
            while (parent != null)
            {
                path = parent.Name + "\\" + path;

                oq.Criterions.Clear();
                oq.Criterions.Clear();
                oq.AddCriterion(Expression.Eq("Id", parent.Id));
                oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
                list = modelPBS.ObjectQuery(typeof(ProjectTaskTypeTree), oq);

                parent = (list[0] as CategoryNode).ParentNode;
            }

            return path;
        }

        //确定
        void btnEnter_Click(object sender, EventArgs e)
        {
            listCheckedPBSNode.Clear();
            listCheckedTaskTypeNode.Clear();

            SelectedPBSFirstNode.Clear();
            SelectedTaskTypeFirstNode.Clear();

            SelectedPBS.Clear();
            SelectedTaskType.Clear();

            foreach (TreeNode root in tvwPBS.Nodes)
            {
                if (root.Checked)
                {
                    listCheckedPBSNode.Add(root.Name, root);
                    SelectedPBSFirstNode.Add(root);
                    SelectedPBS.Add(root.Tag as PBSTree);
                }
                else
                {
                    GetCheckedPBSFirstNode(root);
                }

                GetCheckedPBSNode(root);
            }
            foreach (TreeNode root in tvwTaskType.Nodes)
            {
                if (root.Checked)
                {
                    listCheckedTaskTypeNode.Add(root.Name, root);
                    SelectedTaskTypeFirstNode.Add(root);
                    SelectedTaskType.Add(root.Tag as ProjectTaskTypeTree);
                }
                else
                {
                    GetCheckedTaskTypeFirstNode(root);
                }

                GetCheckedTaskTypeNode(root);
            }
            if (listCheckedPBSNode.Count == 0)
            {
                MessageBox.Show("请勾选PBS节点！");
                return;
            }

            if (listCheckedTaskTypeNode.Count == 0)
            {
                MessageBox.Show("请勾选任务类型节点！");
                return;
            }

            //校验
            if (SelectedMode == SelectedPBSAndTaskTypeMode.一级对一级)//一级对一级校验任务类型选择需要符合规则
            {
                //父节点与所选节点数相同时，表示所有节点都在同一层级
                if (SelectedPBSFirstNode.Count != SelectedPBS.Count)
                {
                    MessageBox.Show("当前拷贝模式下，已选中的所有PBS节点必须在同一层级，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //父节点与所选节点数相同时，表示所有节点都在同一层级
                if (SelectedTaskTypeFirstNode.Count != SelectedTaskType.Count)
                {
                    MessageBox.Show("当前拷贝模式下，已选中的所有工程任务类型节点必须在同一层级，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //检查选择的每个根节点下的子节点之间是否连续
                foreach (TreeNode tn in SelectedTaskTypeFirstNode)
                {
                    if (SelectTaskTypeNodeIsSuccession(tn) == false)
                    {
                        MessageBox.Show("任务类型节点“" + tn.FullPath + "”下选择了不连续的子节点，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tn.Expand();

                        return;
                    }
                }
                //检查选择的每个顶级节点下的子节点之间是否连续
                foreach (TreeNode tn in SelectedPBSFirstNode)
                {
                    if (SelectPBSNodeIsSuccession(tn) == false)
                    {
                        MessageBox.Show("PBS节点“" + tn.FullPath + "”下选择了不连续的子节点，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tn.Expand();

                        return;
                    }
                }

                //判断选择的每个顶级根节点是否是同一个父节点
                for (int i = 0; i < SelectedTaskTypeFirstNode.Count - 1; i++)
                {
                    TreeNode nodePrev = SelectedTaskTypeFirstNode[i];
                    TreeNode nodeNext = SelectedTaskTypeFirstNode[i + 1];

                    if (nodePrev.Parent.Name != nodeNext.Parent.Name)
                    {
                        MessageBox.Show("选择的多个任务类型顶级节点不归属同一父节点，这不符合拷贝规则，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                //判断选择的每个顶级根节点是否是同一个父节点
                for (int i = 0; i < SelectedPBSFirstNode.Count - 1; i++)
                {
                    TreeNode nodePrev = SelectedPBSFirstNode[i];
                    TreeNode nodeNext = SelectedPBSFirstNode[i + 1];

                    if (nodePrev.Parent.Name != nodeNext.Parent.Name)
                    {
                        MessageBox.Show("选择的多个PBS顶级节点不归属同一父节点，这不符合拷贝规则，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            else if (SelectedMode == SelectedPBSAndTaskTypeMode.一级对多级)//一级对多级校验PBS选择需要符合规则
            {
                //父节点与所选节点数相同时，表示所有节点都在同一层级
                if (SelectedPBSFirstNode.Count != SelectedPBS.Count && SelectedTaskTypeFirstNode.Count != SelectedTaskType.Count)
                {
                    MessageBox.Show("当前拷贝模式下，已选中的工程任务类型或PBS的所有节点必须在同一层级，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //判断选择的每个顶级根节点是否是同一个父节点
                for (int i = 0; i < SelectedTaskTypeFirstNode.Count - 1; i++)
                {
                    TreeNode nodePrev = SelectedTaskTypeFirstNode[i];
                    TreeNode nodeNext = SelectedTaskTypeFirstNode[i + 1];

                    if (nodePrev.Parent.Name != nodeNext.Parent.Name)
                    {
                        MessageBox.Show("选择的多个任务类型顶级节点不归属同一父节点，这不符合拷贝规则，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                //判断选择的每个顶级根节点是否是同一个父节点
                for (int i = 0; i < SelectedPBSFirstNode.Count - 1; i++)
                {
                    TreeNode nodePrev = SelectedPBSFirstNode[i];
                    TreeNode nodeNext = SelectedPBSFirstNode[i + 1];

                    if (nodePrev.Parent.Name != nodeNext.Parent.Name)
                    {
                        MessageBox.Show("选择的多个PBS顶级节点不归属同一父节点，这不符合拷贝规则，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            else//多对多
            {
                //父节点与所选节点数相同时，表示所有节点都在同一层级
                if (SelectedPBSFirstNode.Count != SelectedPBS.Count && SelectedTaskTypeFirstNode.Count != SelectedTaskType.Count)
                {
                    MessageBox.Show("当前拷贝模式下，已选中的工程任务类型或PBS的所有节点必须在同一层级，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (ParentNode != null)
            {
                GWBSTree wbs = ParentNode.Tag as GWBSTree;
                List<string> listParentSysCode = new List<string>();
                foreach (GWBSRelaPBS rela in wbs.ListRelaPBS)
                {
                    listParentSysCode.Add(rela.ThePBS.SysCode);
                }

                foreach (TreeNode dicPBS in SelectedPBSFirstNode)
                {
                    PBSTree pbs = dicPBS.Tag as PBSTree;
                    var query = from s in listParentSysCode
                                where pbs.SysCode.IndexOf(s) > -1
                                select s;

                    if (query.Count() == 0)
                    {
                        MessageBox.Show("选择PBS节点“" + pbs.Name + "”不在父节点所属PBS子树上，这不符合添加规范，请检查！");
                        return;
                    }
                }

                ProjectTaskTypeTree parentTaskType = wbs.ProjectTaskTypeGUID;
                if (parentTaskType.TypeLevel != ProjectTaskTypeLevel.单位工程 && parentTaskType.TypeLevel != ProjectTaskTypeLevel.子单位工程 && parentTaskType.TypeLevel != ProjectTaskTypeLevel.专业)
                {
                    foreach (TreeNode dicTask in SelectedTaskTypeFirstNode)
                    {
                        ProjectTaskTypeTree task = dicTask.Tag as ProjectTaskTypeTree;

                        if (task.SysCode.IndexOf(parentTaskType.SysCode) == -1 && task.Level <= parentTaskType.Level)
                        {
                            MessageBox.Show("选择任务类型“" + task.Name + "”不在父节点所属任务类型树上或层级高于父节点任务类型层级，这不符合添加规范，请检查！");
                            return;
                        }
                    }
                }
            }
            else//选择根节点
            {
                if (SelectedMode == SelectedPBSAndTaskTypeMode.一级对一级)
                {
                    if (SelectedTaskTypeFirstNode.Count > 1)
                    {
                        MessageBox.Show("WBS只能有一个根节点，当前拷贝模式下，工程任务类型只能选择一个顶级节点！");
                        return;
                    }
                }
                else if (SelectedMode == SelectedPBSAndTaskTypeMode.一级对多级)
                {
                    if (SelectedPBSFirstNode.Count > 1)
                    {
                        MessageBox.Show("WBS只能有一个根节点，当前拷贝模式下，PBS只能选择一个顶级节点！");
                        return;
                    }
                }
                else
                {
                    //父节点与所选节点数相同时，表示所有节点都在同一层级
                    if (SelectedPBSFirstNode.Count != SelectedPBS.Count && SelectedTaskTypeFirstNode.Count != SelectedTaskType.Count)
                    {
                        MessageBox.Show("当前拷贝模式下，已选中的工程任务类型或PBS的所有节点必须在同一层级，请检查！");
                        return;
                    }
                }
            }
            IsOK = true;

            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GetCheckedPBSFirstNode(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Checked)//找到选择的每一个根节点
                {
                    SelectedPBSFirstNode.Add(tn);
                    continue;
                }

                GetCheckedPBSFirstNode(tn);
            }
        }
        private void GetCheckedTaskTypeFirstNode(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Checked)//找到选择的每一个根节点
                {
                    SelectedTaskTypeFirstNode.Add(tn);
                    continue;
                }

                GetCheckedTaskTypeFirstNode(tn);
            }
        }
        private void GetCheckedPBSNode(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Checked)
                {
                    listCheckedPBSNode.Add(tn.Name, tn);
                    SelectedPBS.Add(tn.Tag as PBSTree);
                }

                GetCheckedPBSNode(tn);
            }
        }
        private void GetCheckedTaskTypeNode(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Checked)
                {
                    listCheckedTaskTypeNode.Add(tn.Name, tn);
                    SelectedTaskType.Add(tn.Tag as ProjectTaskTypeTree);
                }

                GetCheckedTaskTypeNode(tn);
            }
        }
        /// <summary>
        /// 判断选择的PBS节点及其子节点是否连续
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool SelectPBSNodeIsSuccession(TreeNode parentNode)
        {
            //查询节点树
            var listLeafNode = from n in listCheckedPBSNode
                               where (n.Value.Tag as PBSTree).SysCode.IndexOf((parentNode.Tag as PBSTree).SysCode) > -1
                               select n;

            foreach (var dic in listLeafNode)
            {
                if (dic.Key != parentNode.Name)//此叶节点不是顶级节点
                {
                    TreeNode tempParent = dic.Value.Parent;

                    while (tempParent.Name != parentNode.Name)
                    {
                        if (tempParent.Checked == false)
                        {
                            return false;
                        }

                        tempParent = tempParent.Parent;
                    }
                }
            }

            return true;
        }
        /// <summary>
        /// 判断选择的任务类型节点及其子节点是否连续
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool SelectTaskTypeNodeIsSuccession(TreeNode parentNode)
        {
            //查询节点树
            var listLeafNode = from n in listCheckedTaskTypeNode
                               where (n.Value.Tag as ProjectTaskTypeTree).SysCode.IndexOf((parentNode.Tag as ProjectTaskTypeTree).SysCode) > -1
                               select n;

            foreach (var dic in listLeafNode)
            {
                if (dic.Key != parentNode.Name)//此叶节点不是顶级节点
                {
                    TreeNode tempParent = dic.Value.Parent;

                    while (tempParent.Name != parentNode.Name)
                    {
                        if (tempParent.Checked == false)
                        {
                            return false;
                        }

                        tempParent = tempParent.Parent;
                    }
                }
            }

            return true;
        }




        bool isPBSSelectNodeInvoke = false;//是否在选择(点击)节点时调用
        bool startPBSNodeCheckedState = false;//按shift多选兄弟节点时起始节点的选中状态
        private void tvwPBS_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                #region 点击树节点时实现多选
                bool isMultiSelect = false;
                TreeNode preselectionNode;//预选择节点

                preselectionNode = e.Node;

                if (optPBSNode != null && optPBSNode.Name != preselectionNode.Name
                    && ((optPBSNode.Parent == preselectionNode.Parent) || (optPBSNode.Parent != null && preselectionNode.Parent != null && optPBSNode.Parent.Name == preselectionNode.Parent.Name)))//Name取的对象的ID
                    isMultiSelect = true;
                else
                    isMultiSelect = false;

                if (optPBSNode != null)
                    startPBSNodeCheckedState = optPBSNode.Checked;

                if (isMultiSelect && (Control.ModifierKeys & Keys.Shift) != 0 && (Control.ModifierKeys & Keys.Control) != 0)//如果同时按下了ctrl+shift
                {
                    int currNodeIndex = optPBSNode.Index;
                    int preselectNodeIndex = preselectionNode.Index;

                    int startIndex = currNodeIndex < preselectNodeIndex ? currNodeIndex : preselectNodeIndex;
                    int endIndex = currNodeIndex < preselectNodeIndex ? preselectNodeIndex : currNodeIndex;

                    for (int i = startIndex; i <= endIndex; i++)
                    {

                        TreeNode tn = null;
                        if (optPBSNode.Parent == null)
                            tn = tvwPBS.Nodes[i];
                        else
                            tn = optPBSNode.Parent.Nodes[i];

                        isPBSSelectNodeInvoke = true;//设置标志，在check事件中不再处理

                        if (startPBSNodeCheckedState)//如果起始节点当前为选中，就取消选择
                        {
                            TreeNode tempNode = new TreeNode();
                            tn.BackColor = tempNode.BackColor;
                            tn.ForeColor = tempNode.ForeColor;
                            tn.Checked = false;
                        }
                        else//如果起始节点当前为未选中，就设置选择
                        {
                            tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                            tn.ForeColor = ColorTranslator.FromHtml("#000000");
                            tn.Checked = true;
                        }

                        SetPBSChildCheckedByMultiSel(tn);
                    }
                }
                else if (isMultiSelect && (Control.ModifierKeys & Keys.Shift) != 0)//如果同时按下了shift
                {
                    int currNodeIndex = optPBSNode.Index;
                    int preselectNodeIndex = preselectionNode.Index;

                    int startIndex = currNodeIndex < preselectNodeIndex ? currNodeIndex : preselectNodeIndex;
                    int endIndex = currNodeIndex < preselectNodeIndex ? preselectNodeIndex : currNodeIndex;

                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        TreeNode tn = null;
                        if (optPBSNode.Parent == null)
                            tn = tvwPBS.Nodes[i];
                        else
                            tn = optPBSNode.Parent.Nodes[i];

                        isPBSSelectNodeInvoke = true;//设置标志，在check事件中不再处理

                        if (startPBSNodeCheckedState)//如果起始节点当前为选中，就取消选择
                        {
                            tn.Checked = false;

                            TreeNode tempNode = new TreeNode();
                            tn.BackColor = tempNode.BackColor;
                            tn.ForeColor = tempNode.ForeColor;
                        }
                        else//如果起始节点当前为未选中，就设置选择
                        {
                            tn.Checked = true;

                            tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                            tn.ForeColor = ColorTranslator.FromHtml("#000000");
                        }
                    }
                }
                #endregion

                optPBSNode = tvwPBS.SelectedNode;

                optPBSObj = optPBSNode.Tag as PBSTree;

                this.GetPBSNodeDetail();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
            }
        }
        void tvwPBS_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (isPBSSelectNodeInvoke)
            {
                isPBSSelectNodeInvoke = false;
            }
            else
            {
                #region 单击复选框操作
                if ((Control.ModifierKeys & Keys.Shift) != 0 || (Control.ModifierKeys & Keys.Control) != 0)//如果同时按下了Ctrl //(Control.ModifierKeys & Keys.Shift) != 0 || 
                {
                    if (e.Node.Checked)
                    {
                        e.Node.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                        e.Node.ForeColor = ColorTranslator.FromHtml("#000000");
                    }
                    else
                    {
                        TreeNode tempNode = new TreeNode();
                        e.Node.BackColor = tempNode.BackColor;
                        e.Node.ForeColor = tempNode.ForeColor;
                    }

                    SetPBSChildChecked(e.Node);
                }
                else
                {
                    if (e.Node.Checked)
                    {
                        e.Node.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                        e.Node.ForeColor = ColorTranslator.FromHtml("#000000");
                    }
                    else
                    {
                        TreeNode tempNode = new TreeNode();
                        e.Node.BackColor = tempNode.BackColor;
                        e.Node.ForeColor = tempNode.ForeColor;
                    }
                }
                #endregion
            }
        }

        bool isTaskTypeSelectNodeInvoke = false;//是否在选择(点击)节点时调用
        bool startTaskTypeNodeCheckedState = false;//按shift多选兄弟节点时起始节点的选中状态
        void tvwTaskType_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                #region 点击树节点时实现多选
                bool isMultiSelect = false;
                TreeNode preselectionNode;//预选择节点

                preselectionNode = e.Node;

                if (optTaskTypeNode != null && optTaskTypeNode.Name != preselectionNode.Name
                    && optTaskTypeNode.Parent != null && preselectionNode.Parent != null && optTaskTypeNode.Parent.Name == preselectionNode.Parent.Name)//Name取的对象的ID
                    isMultiSelect = true;
                else
                    isMultiSelect = false;

                if (optTaskTypeNode != null)
                    startTaskTypeNodeCheckedState = optTaskTypeNode.Checked;

                if (isMultiSelect && (Control.ModifierKeys & Keys.Shift) != 0 && (Control.ModifierKeys & Keys.Control) != 0)//如果同时按下了ctrl+shift
                {
                    int currNodeIndex = optTaskTypeNode.Index;
                    int preselectNodeIndex = preselectionNode.Index;

                    int startIndex = currNodeIndex < preselectNodeIndex ? currNodeIndex : preselectNodeIndex;
                    int endIndex = currNodeIndex < preselectNodeIndex ? preselectNodeIndex : currNodeIndex;

                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        TreeNode tn = optTaskTypeNode.Parent.Nodes[i];

                        isTaskTypeSelectNodeInvoke = true;//设置标志，在check事件中不再处理

                        if (startTaskTypeNodeCheckedState)//如果起始节点当前为选中，就取消选择
                        {
                            TreeNode tempNode = new TreeNode();
                            tn.BackColor = tempNode.BackColor;
                            tn.Checked = false;
                        }
                        else//如果起始节点当前为未选中，就设置选择
                        {
                            tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                            tn.Checked = true;
                        }

                        SetTaskTypeChildCheckedByMultiSel(tn);
                    }
                }
                else if (isMultiSelect && (Control.ModifierKeys & Keys.Shift) != 0)//如果同时按下了shift
                {
                    int currNodeIndex = optTaskTypeNode.Index;
                    int preselectNodeIndex = preselectionNode.Index;

                    int startIndex = currNodeIndex < preselectNodeIndex ? currNodeIndex : preselectNodeIndex;
                    int endIndex = currNodeIndex < preselectNodeIndex ? preselectNodeIndex : currNodeIndex;

                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        TreeNode tn = optTaskTypeNode.Parent.Nodes[i];

                        isTaskTypeSelectNodeInvoke = true;//设置标志，在check事件中不再处理

                        if (startTaskTypeNodeCheckedState)//如果起始节点当前为选中，就取消选择
                        {
                            tn.Checked = false;

                            TreeNode tempNode = new TreeNode();
                            tn.BackColor = tempNode.BackColor;
                        }
                        else//如果起始节点当前为未选中，就设置选择
                        {
                            tn.Checked = true;

                            tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                        }
                    }
                }
                #endregion

                optTaskTypeNode = tvwTaskType.SelectedNode;

                optTaskTypeObj = optTaskTypeNode.Tag as ProjectTaskTypeTree;

                this.GetTaskTypeNodeDetail();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
            }
        }
        void tvwTaskType_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (isTaskTypeSelectNodeInvoke)
            {
                isTaskTypeSelectNodeInvoke = false;
            }
            else
            {
                #region 单击复选框操作
                if ((Control.ModifierKeys & Keys.Shift) != 0 || (Control.ModifierKeys & Keys.Control) != 0)//如果同时按下了Ctrl //(Control.ModifierKeys & Keys.Shift) != 0 || 
                {
                    if (e.Node.Checked)
                    {
                        e.Node.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    }
                    else
                    {
                        TreeNode tempNode = new TreeNode();
                        e.Node.BackColor = tempNode.BackColor;
                    }

                    SetTaskTypeChildChecked(e.Node);
                }
                else
                {
                    if (e.Node.Checked)
                    {
                        e.Node.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    }
                    else
                    {
                        TreeNode tempNode = new TreeNode();
                        e.Node.BackColor = tempNode.BackColor;
                    }
                }
                #endregion
            }
        }

        private void SetPBSChildChecked(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                SetPBSChildChecked(tn);
                tn.Checked = parentNode.Checked;

                if (tn.Checked)
                {
                    tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    tn.ForeColor = ColorTranslator.FromHtml("#000000");
                }
                else
                {
                    TreeNode tempNode = new TreeNode();
                    tn.BackColor = tempNode.BackColor;
                    tn.ForeColor = tempNode.ForeColor;
                }
            }
        }
        private void SetTaskTypeChildChecked(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                SetTaskTypeChildChecked(tn);
                tn.Checked = parentNode.Checked;

                if (tn.Checked)
                {
                    tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                }
                else
                {
                    TreeNode tempNode = new TreeNode();
                    tn.BackColor = tempNode.BackColor;
                }
            }
        }


        private void SetPBSChildCheckedByMultiSel(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                SetPBSChildCheckedByMultiSel(tn);

                isPBSSelectNodeInvoke = true;

                if (startPBSNodeCheckedState)//如果起始节点当前为选中，就取消选择
                {
                    TreeNode tempNode = new TreeNode();
                    tn.BackColor = tempNode.BackColor;
                    tn.ForeColor = tempNode.ForeColor;
                    tn.Checked = false;
                }
                else//如果起始节点当前为未选中，就设置选择
                {
                    tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    tn.ForeColor = ColorTranslator.FromHtml("#000000");
                    tn.Checked = true;
                }
            }
        }
        private void SetTaskTypeChildCheckedByMultiSel(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                SetTaskTypeChildCheckedByMultiSel(tn);

                isTaskTypeSelectNodeInvoke = true;

                if (startTaskTypeNodeCheckedState)//如果起始节点当前为选中，就取消选择
                {
                    TreeNode tempNode = new TreeNode();
                    tn.BackColor = tempNode.BackColor;
                    tn.Checked = false;
                }
                else//如果起始节点当前为未选中，就设置选择
                {
                    tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    tn.Checked = true;
                }
            }
        }

        private void GetPBSNodeDetail()
        {
            try
            {

                this.txtPBSType.Text = optPBSObj.StructTypeName;
                this.txtPBSCurrPath.Text = optPBSNode.FullPath;
                this.txtPBSDesc.Text = optPBSObj.Describe;
            }
            catch (Exception exp)
            {
                MessageBox.Show("显示出错：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }
        private void GetTaskTypeNodeDetail()
        {
            try
            {
                this.txtTaskType.Text = optTaskTypeObj.TypeLevel.ToString();
                this.txtTaskCurrPath.Text = optTaskTypeNode.FullPath;
                this.txtTaskDesc.Text = optTaskTypeObj.Summary;
            }
            catch (Exception exp)
            {
                MessageBox.Show("显示出错：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        private void LoadPBSTree(TreeNode parentGWBSNode)
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwPBS.Nodes.Clear();
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                //加载子树
                IList list = null;
                if (parentGWBSNode == null)
                {
                    list = modelPBS.GetPBSTreesByInstance(projectInfo.Id);
                }
                else
                {
                    IList listPBS = new ArrayList();

                    GWBSTree parent = parentGWBSNode.Tag as GWBSTree;
                    foreach (GWBSRelaPBS rela in parent.ListRelaPBS)
                    {
                        listPBS.Add(rela.ThePBS);
                    }

                    list = modelPBS.GetPBSTreesByInstance(projectInfo.Id, listPBS);
                }


                foreach (PBSTree childNode in list)
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
                        else
                        {
                            tvwPBS.Nodes.Add(tnTmp);
                        }
                    }
                    else
                    {
                        tvwPBS.Nodes.Add(tnTmp);
                    }
                    hashtable.Add(tnTmp.Name, tnTmp);
                }
                if (list.Count > 0)
                {
                    this.tvwPBS.SelectedNode = this.tvwPBS.Nodes[0];
                    this.tvwPBS.SelectedNode.Expand();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("查询出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }
        private void LoadTaskTypeTree(TreeNode parentGWBSNode)
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwTaskType.Nodes.Clear();
                CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                IList list = null;
                if (parentGWBSNode == null)
                    list = modelTaskType.GetProjectTaskTypeByInstance();
                else
                {
                    IList listTaskType = new ArrayList();

                    GWBSTree parent = parentGWBSNode.Tag as GWBSTree;
                    listTaskType.Add(parent.ProjectTaskTypeGUID);

                    list = modelTaskType.GetProjectTaskTypeByInstance(listTaskType);
                }

                list = list.OfType<ProjectTaskTypeTree>().Where(p => p.Level != 9999).ToList();

                foreach (ProjectTaskTypeTree childNode in list)
                {
                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = childNode.Name;
                    tnTmp.Tag = childNode;

                    if (childNode.TypeStandard == ProjectTaskTypeStandard.企标)
                        tnTmp.ForeColor = qibiaoColor;


                    if (childNode.ParentNode != null)
                    {
                        TreeNode tnp = null;
                        tnp = hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                        if (tnp != null)
                            tnp.Nodes.Add(tnTmp);
                        else
                            tvwTaskType.Nodes.Add(tnTmp);
                    }
                    else
                    {
                        tvwTaskType.Nodes.Add(tnTmp);
                    }
                    hashtable.Add(tnTmp.Name, tnTmp);
                }
                if (list.Count > 0)
                {
                    this.tvwTaskType.SelectedNode = this.tvwTaskType.Nodes[0];
                    this.tvwTaskType.SelectedNode.Expand();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("查询出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        private void rbPBSChildTree_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPBSChildTree.Checked)
            {
                LoadPBSTree(ParentNode);

                InitOpenAndSelectedPBSTreeNode();
            }
        }

        private void rbPBSAllTree_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPBSAllTree.Checked)
            {
                LoadPBSTree(null);

                InitOpenAndSelectedPBSTreeNode();
            }
        }

        private void cbTaskTypeChildTree_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTaskTypeChildTree.Checked)
            {
                LoadTaskTypeTree(ParentNode);

                InitOpenAndSelectedTaskTypeTreeNode();
            }
        }

        private void cbTaskTypeAllTree_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTaskTypeAllTree.Checked)
            {
                LoadTaskTypeTree(null);

                InitOpenAndSelectedTaskTypeTreeNode();
            }
        }

        #region n对n模式处理
        //选择模式 pbs-任务类型（一级对一级）
        private void rbSelectedMode1_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSelectedMode1.Checked)
            {
                SelectedMode = SelectedPBSAndTaskTypeMode.一级对一级;
                InitOpenAndSelectedTaskTypeTreeNode();
            }
        }

        //选择模式 pbs-任务类型（一级对多级）
        private void rbSelectedMode2_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSelectedMode2.Checked)
            {
                SelectedMode = SelectedPBSAndTaskTypeMode.一级对多级;
                InitOpenAndSelectedTaskTypeTreeNode();
            }
        }

        /// <summary>
        /// 选择模式 pbs-任务类型（多对多）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbSelectedMode3_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSelectedMode3.Checked)
            {
                SelectedMode = SelectedPBSAndTaskTypeMode.多对多;
                InitOpenAndSelectedTaskTypeTreeNode();
            }
        }
        #endregion n对n模式处理

        /// <summary>
        /// 选择PBS-任务类型模式
        /// </summary>
        public enum SelectedPBSAndTaskTypeMode
        {
            一级对一级 = 1,
            一级对多级 = 2,
            多对多 = 3
        }
    }
}
