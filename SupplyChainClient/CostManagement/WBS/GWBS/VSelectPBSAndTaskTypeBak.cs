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
    public partial class VSelectPBSAndTaskTypeBak : TBasicDataView
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

        private Dictionary<string, TreeNode> listCheckedPBSNode = new Dictionary<string, TreeNode>();
        private Dictionary<string, TreeNode> listCheckedTaskTypeNode = new Dictionary<string, TreeNode>();

        /// <summary>
        /// PBS和任务类型组合规则明细
        /// </summary>
        List<PBSRelaTaskTypeRuleDetail> ListRuleDtl = new List<PBSRelaTaskTypeRuleDetail>();

        /// <summary>
        /// 父GWBS节点
        /// </summary>
        public TreeNode ParentNode = null;

        private List<string> _parentPBSType = new List<string>();
        private string _parentTaskType = "";
        /// <summary>
        /// 父对象PBS类型集合
        /// </summary>
        public List<string> ParentPBSType
        {
            get { return _parentPBSType; }
            set { _parentPBSType = value; }
        }
        /// <summary>
        /// 父对象任务类型
        /// </summary>
        public string ParentTaskType
        {
            get { return _parentTaskType; }
            set { _parentTaskType = value; }
        }

        private List<PBSTree> _selectedPBS = new List<PBSTree>();
        private List<ProjectTaskTypeTree> _selectedTaskType = new List<ProjectTaskTypeTree>();
        /// <summary>
        /// 选择的PBS
        /// </summary>
        public List<PBSTree> SelectedPBS
        {
            get { return _selectedPBS; }
            set { _selectedPBS = value; }
        }
        /// <summary>
        /// 选择的任务类型
        /// </summary>
        public List<ProjectTaskTypeTree> SelectedTaskType
        {
            get { return _selectedTaskType; }
            set { _selectedTaskType = value; }
        }

        private TreeNode optPBSNode;
        private PBSTree optPBSObj;
        private TreeNode optTaskTypeNode;
        private ProjectTaskTypeTree optTaskTypeObj;

        public MPBSTree modelPBS = new MPBSTree();
        public MWBSManagement modelTaskType = new MWBSManagement();

        public bool IsSingleSelectTaskType = false;


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
        Color qibiaoColor = Color.Blue;//ColorTranslator.FromHtml("#D7E8FE");

        public VSelectPBSAndTaskTypeBak()
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

            btnEnter.Click += new EventHandler(btnEnter_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            this.Load += new EventHandler(VSelectPBSAndTaskType_Load);
        }
        void VSelectPBSAndTaskType_Load(object sender, EventArgs e)
        {
            if (ParentPBSType.Count == 0 || string.IsNullOrEmpty(ParentTaskType))
            {
                PBSRelaTaskTypeRuleDetail dtl = new PBSRelaTaskTypeRuleDetail();
                dtl.PBSType = "项目";
                dtl.TaskType = "项目";
                ListRuleDtl.Add(dtl);
            }
            else
            {
                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();
                foreach (string type in ParentPBSType)
                {
                    dis.Add(NHibernate.Criterion.Expression.Eq("PBSType", type));
                }
                oq.AddCriterion(dis);
                oq.AddCriterion(NHibernate.Criterion.Expression.Eq("TaskType", ParentTaskType));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                IList list = modelPBS.ObjectQuery(typeof(PBSRelaTaskTypeRuleMaster), oq);
                if (list == null || list.Count == 0)
                {
                    MessageBox.Show("父对象关联的PBS和任务类型不符合GWBS节点规则，请检查！");
                    this.Close();
                }

                foreach (PBSRelaTaskTypeRuleMaster rule in list)
                {
                    ListRuleDtl.AddRange(rule.Details.ToList());
                }
            }

            InitForm();
        }

        private void InitForm()
        {
            RefreshState(MainViewState.Browser);

            LoadPBSTree();


            txtGuoBiaoColorFlag.BackColor = guobiaoColor;
            lblGuobiao.ForeColor = guobiaoColor;
            txtQiBiaoColorFlag.BackColor = qibiaoColor;
            lblQibiao.ForeColor = qibiaoColor;

            LoadTaskTypeTree();

            tvwTaskType.CheckBoxes = !IsSingleSelectTaskType;


            if (ParentNode != null)
            {
                txtParentNodePath.Text = ParentNode.FullPath;
                GWBSTree parent = ParentNode.Tag as GWBSTree;
                txtParentName.Text = parent.Name;
                txtParentTaskType.Text = parent.ProjectTaskTypeGUID.Name + "[" + parent.ProjectTaskTypeGUID.TypeLevel + "]";

                foreach (GWBSRelaPBS rela in parent.ListRelaPBS)
                {
                    cbRelaPBS.Items.Add(GetFullPath(rela.ThePBS) + "[" + rela.ThePBS.StructTypeName + "]");


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
                if (cbRelaPBS.Items.Count > 0)
                    cbRelaPBS.SelectedIndex = 0;


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
                        break;
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
                        break;
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
        void btnEnter_Click(object sender, EventArgs e)
        {
            if (listCheckedPBSNode.Count == 0)
            {
                MessageBox.Show("请勾选PBS节点！");
                return;
            }

            if (IsSingleSelectTaskType)
            {
                listCheckedTaskTypeNode.Clear();

                listCheckedTaskTypeNode.Add(tvwTaskType.SelectedNode.Name, tvwTaskType.SelectedNode);
            }

            if (listCheckedTaskTypeNode.Count == 0)
            {
                MessageBox.Show("请勾选任务类型节点！");
                return;
            }

            SelectedPBS.Clear();
            SelectedTaskType.Clear();

            foreach (var dicPBS in listCheckedPBSNode)
            {
                PBSTree pbs = dicPBS.Value.Tag as PBSTree;

                SelectedPBS.Add(pbs);

                foreach (var dicTask in listCheckedTaskTypeNode)
                {
                    ProjectTaskTypeTree task = dicTask.Value.Tag as ProjectTaskTypeTree;
                    var query = from r in ListRuleDtl
                                where r.PBSType.Trim() == pbs.StructTypeName &&
                                r.TaskType.Trim() == task.TypeLevel.ToString()
                                select r;

                    if (query == null || query.Count() == 0)
                    {
                        MessageBox.Show("选择的PBS节点“" + pbs.Name + "[" + pbs.StructTypeName + "]”和任务类型节点“" + task.Name + "[" + task.TypeLevel + "]”不符合添加规则，请检查！");
                        return;
                    }

                    SelectedTaskType.Add(task);
                }
            }

            //设置节点完整路径
            for (int i = 0; i < SelectedPBS.Count; i++)
            {
                SelectedPBS[i].FullPath = GetFullPath(SelectedPBS[i]);
            }
            for (int i = 0; i < SelectedTaskType.Count; i++)
            {
                SelectedTaskType[i].FullPath = GetFullPath(SelectedTaskType[i]);
            }

            IsOK = true;

            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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
                    && optPBSNode.Parent != null && preselectionNode.Parent != null && optPBSNode.Parent.Name == preselectionNode.Parent.Name)//Name取的对象的ID
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
                        TreeNode tn = optPBSNode.Parent.Nodes[i];

                        isPBSSelectNodeInvoke = true;//设置标志，在check事件中不再处理

                        if (startPBSNodeCheckedState)//如果起始节点当前为选中，就取消选择
                        {
                            TreeNode tempNode = new TreeNode();
                            tn.BackColor = tempNode.BackColor;
                            tn.ForeColor = tempNode.ForeColor;

                            if (listCheckedPBSNode.ContainsKey(tn.Name))
                                listCheckedPBSNode.Remove(tn.Name);

                            tn.Checked = false;
                        }
                        else//如果起始节点当前为未选中，就设置选择
                        {
                            tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                            tn.ForeColor = ColorTranslator.FromHtml("#000000");

                            if (listCheckedPBSNode.ContainsKey(tn.Name))
                                listCheckedPBSNode[tn.Name] = tn;
                            else
                                listCheckedPBSNode.Add(tn.Name, tn);

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
                        TreeNode tn = optPBSNode.Parent.Nodes[i];

                        isPBSSelectNodeInvoke = true;//设置标志，在check事件中不再处理

                        if (startPBSNodeCheckedState)//如果起始节点当前为选中，就取消选择
                        {
                            tn.Checked = false;

                            TreeNode tempNode = new TreeNode();
                            tn.BackColor = tempNode.BackColor;
                            tn.ForeColor = tempNode.ForeColor;

                            if (listCheckedPBSNode.ContainsKey(tn.Name))
                                listCheckedPBSNode.Remove(tn.Name);
                        }
                        else//如果起始节点当前为未选中，就设置选择
                        {
                            tn.Checked = true;

                            tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                            tn.ForeColor = ColorTranslator.FromHtml("#000000");

                            if (listCheckedPBSNode.ContainsKey(tn.Name))
                                listCheckedPBSNode[tn.Name] = tn;
                            else
                                listCheckedPBSNode.Add(tn.Name, tn);
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

                        //e.Node.BackColor = SystemColors.Control;
                        //e.Node.ForeColor = SystemColors.ControlText;

                        if (listCheckedPBSNode.ContainsKey(e.Node.Name))
                            listCheckedPBSNode[e.Node.Name] = e.Node;
                        else
                            listCheckedPBSNode.Add(e.Node.Name, e.Node);
                    }
                    else
                    {
                        TreeNode tempNode = new TreeNode();
                        e.Node.BackColor = tempNode.BackColor;
                        e.Node.ForeColor = tempNode.ForeColor;

                        if (listCheckedPBSNode.ContainsKey(e.Node.Name))
                            listCheckedPBSNode.Remove(e.Node.Name);
                    }

                    SetPBSChildChecked(e.Node);
                }
                else
                {
                    if (e.Node.Checked)
                    {
                        e.Node.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                        e.Node.ForeColor = ColorTranslator.FromHtml("#000000");

                        //e.Node.BackColor = SystemColors.Control;
                        //e.Node.ForeColor = SystemColors.ControlText;

                        if (listCheckedPBSNode.ContainsKey(e.Node.Name))
                            listCheckedPBSNode[e.Node.Name] = e.Node;
                        else
                            listCheckedPBSNode.Add(e.Node.Name, e.Node);
                    }
                    else
                    {
                        TreeNode tempNode = new TreeNode();
                        e.Node.BackColor = tempNode.BackColor;
                        e.Node.ForeColor = tempNode.ForeColor;

                        if (listCheckedPBSNode.ContainsKey(e.Node.Name))
                            listCheckedPBSNode.Remove(e.Node.Name);
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
                            //tn.ForeColor = tempNode.ForeColor;

                            if (listCheckedTaskTypeNode.ContainsKey(tn.Name))
                                listCheckedTaskTypeNode.Remove(tn.Name);

                            tn.Checked = false;
                        }
                        else//如果起始节点当前为未选中，就设置选择
                        {
                            tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                            //tn.ForeColor = ColorTranslator.FromHtml("#000000");

                            if (listCheckedTaskTypeNode.ContainsKey(tn.Name))
                                listCheckedTaskTypeNode[tn.Name] = tn;
                            else
                                listCheckedTaskTypeNode.Add(tn.Name, tn);

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
                            //tn.ForeColor = tempNode.ForeColor;

                            if (listCheckedTaskTypeNode.ContainsKey(tn.Name))
                                listCheckedTaskTypeNode.Remove(tn.Name);
                        }
                        else//如果起始节点当前为未选中，就设置选择
                        {
                            tn.Checked = true;

                            tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                            //tn.ForeColor = ColorTranslator.FromHtml("#000000");

                            if (listCheckedTaskTypeNode.ContainsKey(tn.Name))
                                listCheckedTaskTypeNode[tn.Name] = tn;
                            else
                                listCheckedTaskTypeNode.Add(tn.Name, tn);
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
                        //e.Node.ForeColor = ColorTranslator.FromHtml("#000000");

                        if (listCheckedTaskTypeNode.ContainsKey(e.Node.Name))
                            listCheckedTaskTypeNode[e.Node.Name] = e.Node;
                        else
                            listCheckedTaskTypeNode.Add(e.Node.Name, e.Node);
                    }
                    else
                    {
                        TreeNode tempNode = new TreeNode();
                        e.Node.BackColor = tempNode.BackColor;
                        //e.Node.ForeColor = tempNode.ForeColor;

                        if (listCheckedTaskTypeNode.ContainsKey(e.Node.Name))
                            listCheckedTaskTypeNode.Remove(e.Node.Name);
                    }

                    SetTaskTypeChildChecked(e.Node);
                }
                else
                {
                    if (e.Node.Checked)
                    {
                        e.Node.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                        //e.Node.ForeColor = ColorTranslator.FromHtml("#000000");

                        if (listCheckedTaskTypeNode.ContainsKey(e.Node.Name))
                            listCheckedTaskTypeNode[e.Node.Name] = e.Node;
                        else
                            listCheckedTaskTypeNode.Add(e.Node.Name, e.Node);
                    }
                    else
                    {
                        TreeNode tempNode = new TreeNode();
                        e.Node.BackColor = tempNode.BackColor;
                        //e.Node.ForeColor = tempNode.ForeColor;

                        if (listCheckedTaskTypeNode.ContainsKey(e.Node.Name))
                            listCheckedTaskTypeNode.Remove(e.Node.Name);
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

                    if (listCheckedPBSNode.ContainsKey(tn.Name))
                        listCheckedPBSNode[tn.Name] = tn;
                    else
                        listCheckedPBSNode.Add(tn.Name, tn);
                }
                else
                {
                    TreeNode tempNode = new TreeNode();
                    tn.BackColor = tempNode.BackColor;
                    tn.ForeColor = tempNode.ForeColor;

                    if (listCheckedPBSNode.ContainsKey(tn.Name))
                        listCheckedPBSNode.Remove(tn.Name);
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
                    //tn.ForeColor = ColorTranslator.FromHtml("#000000");

                    if (listCheckedTaskTypeNode.ContainsKey(tn.Name))
                        listCheckedTaskTypeNode[tn.Name] = tn;
                    else
                        listCheckedTaskTypeNode.Add(tn.Name, tn);
                }
                else
                {
                    TreeNode tempNode = new TreeNode();
                    tn.BackColor = tempNode.BackColor;
                    //tn.ForeColor = tempNode.ForeColor;//设置了国企标颜色区分

                    if (listCheckedTaskTypeNode.ContainsKey(tn.Name))
                        listCheckedTaskTypeNode.Remove(tn.Name);
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

                    if (listCheckedPBSNode.ContainsKey(tn.Name))
                        listCheckedPBSNode.Remove(tn.Name);

                    tn.Checked = false;
                }
                else//如果起始节点当前为未选中，就设置选择
                {
                    tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    tn.ForeColor = ColorTranslator.FromHtml("#000000");

                    if (listCheckedPBSNode.ContainsKey(tn.Name))
                        listCheckedPBSNode[tn.Name] = tn;
                    else
                        listCheckedPBSNode.Add(tn.Name, tn);

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
                    //tn.ForeColor = tempNode.ForeColor;//设置了国企标颜色区分

                    if (listCheckedTaskTypeNode.ContainsKey(tn.Name))
                        listCheckedTaskTypeNode.Remove(tn.Name);

                    tn.Checked = false;
                }
                else//如果起始节点当前为未选中，就设置选择
                {
                    tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    //tn.ForeColor = ColorTranslator.FromHtml("#000000");

                    if (listCheckedTaskTypeNode.ContainsKey(tn.Name))
                        listCheckedTaskTypeNode[tn.Name] = tn;
                    else
                        listCheckedTaskTypeNode.Add(tn.Name, tn);

                    tn.Checked = true;
                }
            }
        }

        //private void GetPBSChildChecked(TreeNode parentNode)
        //{
        //    foreach (TreeNode tn in parentNode.Nodes)
        //    {
        //        if (tn.Checked)
        //        {
        //            if (listCheckedPBSNode.ContainsKey(tn.Name))
        //                listCheckedPBSNode[tn.Name] = tn;
        //            else
        //                listCheckedPBSNode.Add(tn.Name, tn);
        //        }
        //        GetPBSChildChecked(tn);
        //    }
        //}
        //private void RemoveChildChecked(TreeNode parentNode)
        //{
        //    foreach (TreeNode tn in parentNode.Nodes)
        //    {
        //        RemoveChildChecked(tn);
        //        if (tn.Checked)
        //        {
        //            if (listCheckedPBSNode.ContainsKey(tn.Name))
        //                listCheckedPBSNode.Remove(tn.Name);
        //        }
        //    }
        //}

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

        private void LoadPBSTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwPBS.Nodes.Clear();
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                IList list = modelPBS.GetPBSTreesByInstance(projectInfo.Id);
                //IList list = listAll[0] as IList;
                foreach (PBSTree childNode in list)
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
        private void LoadTaskTypeTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwTaskType.Nodes.Clear();
                CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                IList list = modelTaskType.GetProjectTaskTypeByInstance();
                //IList list = listAll[0] as IList;
                foreach (ProjectTaskTypeTree childNode in list)
                {
                    if (childNode.State == 0)
                        continue;

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
    }
}
