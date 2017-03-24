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
    public partial class VSelectPBSAndTaskType : TBasicDataView
    {
        private bool _isOK = false;
        /// <summary>
        /// �Ƿ�����ȷ��
        /// </summary>
        public bool IsOK
        {
            get { return _isOK; }
            set { _isOK = value; }
        }

        /// <summary>
        /// ��GWBS�ڵ�
        /// </summary>
        public TreeNode ParentNode = null;
        private List<string> _parentPBSType = new List<string>();
        /// <summary>
        /// ������PBS���ͼ���
        /// </summary>
        public List<string> ParentPBSType
        {
            get { return _parentPBSType; }
            set { _parentPBSType = value; }
        }
        private string _parentTaskType = "";
        /// <summary>
        /// ��������������
        /// </summary>
        public string ParentTaskType
        {
            get { return _parentTaskType; }
            set { _parentTaskType = value; }
        }


        /// <summary>
        /// PBS������������Ϲ�����ϸ
        /// </summary>
        private List<PBSRelaTaskTypeRuleDetail> ListRuleDtl = new List<PBSRelaTaskTypeRuleDetail>();

        //��ѡѡ��Ľڵ㼯��
        private Dictionary<string, TreeNode> listCheckedPBSNode = new Dictionary<string, TreeNode>();
        private Dictionary<string, TreeNode> listCheckedTaskTypeNode = new Dictionary<string, TreeNode>();

        private List<PBSTree> _selectedPBS = new List<PBSTree>();
        private List<ProjectTaskTypeTree> _selectedTaskType = new List<ProjectTaskTypeTree>();
        private List<TreeNode> _selectedPBSFirstNode = new List<TreeNode>();
        private List<TreeNode> _selectedTaskTypeFirstNode = new List<TreeNode>();
        /// <summary>
        /// ȷ��ѡ���PBS
        /// </summary>
        public List<PBSTree> SelectedPBS
        {
            get { return _selectedPBS; }
            set { _selectedPBS = value; }
        }
        /// <summary>
        /// ȷ��ѡ�����������
        /// </summary>
        public List<ProjectTaskTypeTree> SelectedTaskType
        {
            get { return _selectedTaskType; }
            set { _selectedTaskType = value; }
        }
        /// <summary>
        /// ѡ���PBS�����ڵ�
        /// </summary>
        public List<TreeNode> SelectedPBSFirstNode
        {
            get { return _selectedPBSFirstNode; }
            set { _selectedPBSFirstNode = value; }
        }
        /// <summary>
        /// ѡ����������Ͷ����ڵ�
        /// </summary>
        public List<TreeNode> SelectedTaskTypeFirstNode
        {
            get { return _selectedTaskTypeFirstNode; }
            set { _selectedTaskTypeFirstNode = value; }
        }


        //��ǰ�����Ľڵ�
        private TreeNode optPBSNode;
        private PBSTree optPBSObj;
        private TreeNode optTaskTypeNode;
        private ProjectTaskTypeTree optTaskTypeObj;

        private SelectedPBSAndTaskTypeMode _selectedMode = SelectedPBSAndTaskTypeMode.���һ;
        /// <summary>
        /// ѡ��PBS-�������͵�ģʽ
        /// </summary>
        public SelectedPBSAndTaskTypeMode SelectedMode
        {
            get { return _selectedMode; }
            set { _selectedMode = value; }
        }


        private MPBSTree modelPBS = new MPBSTree();
        private MWBSManagement modelTaskType = new MWBSManagement();


        /// <summary>
        /// ��ʼPBS��(Ĭ��ѡ���PBS)
        /// </summary>
        public List<PBSTree> InitListPBS = new List<PBSTree>();
        /// <summary>
        /// ��ʼ��������(Ĭ��ѡ�����������)
        /// </summary>
        public ProjectTaskTypeTree InitTaskType = null;

        public bool IsSingleSelectTaskType = false;


        //�����ɫ��ʶ
        Color guobiaoColor = Color.Black;
        Color qibiaoColor = Color.Blue;//ColorTranslator.FromHtml("#D7E8FE");

        public VSelectPBSAndTaskType()
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
            #region ���У�����
            //if (ParentPBSType.Count == 0 || string.IsNullOrEmpty(ParentTaskType))
            //{
            //    PBSRelaTaskTypeRuleDetail dtl = new PBSRelaTaskTypeRuleDetail();
            //    dtl.PBSType = "��Ŀ";
            //    dtl.TaskType = "��Ŀ";
            //    ListRuleDtl.Add(dtl);
            //}
            //else
            //{
            //    ObjectQuery oq = new ObjectQuery();
            //    Disjunction dis = new Disjunction();
            //    foreach (string type in ParentPBSType)
            //    {
            //        dis.Add(NHibernate.Criterion.Expression.Eq("PBSType", type));
            //    }
            //    oq.AddCriterion(dis);
            //    oq.AddCriterion(NHibernate.Criterion.Expression.Eq("TaskType", ParentTaskType));
            //    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

            //    IList list = modelPBS.ObjectQuery(typeof(PBSRelaTaskTypeRuleMaster), oq);
            //    if (list == null || list.Count == 0)
            //    {
            //        MessageBox.Show("�����������PBS���������Ͳ�����GWBS�ڵ�������飡");
            //        this.Close();
            //    }

            //    foreach (PBSRelaTaskTypeRuleMaster rule in list)
            //    {
            //        ListRuleDtl.AddRange(rule.Details.ToList());
            //    }
            //}
            #endregion

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

            tvwTaskType.CheckBoxes = !IsSingleSelectTaskType;

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
            }
        }
        private void InitOpenAndSelectedPBSTreeNode()
        {
            if (ParentNode != null)
            {
                GWBSTree parent = ParentNode.Tag as GWBSTree;
                foreach (GWBSRelaPBS rela in parent.ListRelaPBS)
                {
                    //��ʼ���򿪸��ڵ������PBSλ��
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
                    //��ʼѡ���PBS��
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
                //��ʼ���򿪸��ڵ�����Ĺ�����������λ��
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

            if (InitTaskType != null)
            {
                //��ʼѡ�����������
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

        //ȷ��
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
                    GetCheckedPBSFirstNode(root);

                GetCheckedPBSNode(root);
            }

            if (tvwTaskType.Nodes[0].Checked)
            {
                listCheckedTaskTypeNode.Add(tvwTaskType.Nodes[0].Name, tvwTaskType.Nodes[0]);
                SelectedTaskTypeFirstNode.Add(tvwTaskType.Nodes[0]);
                SelectedTaskType.Add(tvwTaskType.Nodes[0].Tag as ProjectTaskTypeTree);
            }
            else
                GetCheckedTaskTypeFirstNode(tvwTaskType.Nodes[0]);

            GetCheckedTaskTypeNode(tvwTaskType.Nodes[0]);

            if (IsSingleSelectTaskType)
            {
                listCheckedTaskTypeNode.Clear();

                listCheckedTaskTypeNode.Add(tvwTaskType.SelectedNode.Name, tvwTaskType.SelectedNode);
                SelectedTaskTypeFirstNode.Add(tvwTaskType.SelectedNode);
                SelectedTaskType.Add(tvwTaskType.SelectedNode.Tag as ProjectTaskTypeTree);
            }

            if (listCheckedPBSNode.Count == 0)
            {
                MessageBox.Show("�빴ѡPBS�ڵ㣡");
                return;
            }

            if (listCheckedTaskTypeNode.Count == 0)
            {
                MessageBox.Show("�빴ѡ�������ͽڵ㣡");
                return;
            }

            //У��
            if (SelectedMode == SelectedPBSAndTaskTypeMode.���һ)//���һУ����������ѡ����Ҫ���Ϲ���
            {
                //���ѡ���ÿ�����ڵ��µ��ӽڵ�֮���Ƿ�����
                foreach (TreeNode tn in SelectedTaskTypeFirstNode)
                {
                    if (SelectTaskTypeNodeIsSuccession(tn) == false)
                    {
                        MessageBox.Show("�������ͽڵ㡰" + tn.FullPath + "����ѡ���˲��������ӽڵ㣬���飡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tn.Expand();

                        return;
                    }
                }
                //�ж�ѡ���ÿ���������ڵ��Ƿ���ͬһ�����ڵ�
                for (int i = 0; i < SelectedTaskTypeFirstNode.Count - 1; i++)
                {
                    TreeNode nodePrev = SelectedTaskTypeFirstNode[i];
                    TreeNode nodeNext = SelectedTaskTypeFirstNode[i + 1];

                    if (nodePrev.Parent.Name != nodeNext.Parent.Name)
                    {
                        MessageBox.Show("ѡ��Ķ���������Ͷ����ڵ㲻����ͬһ���ڵ㣬�ⲻ���Ͽ����������飡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            else if (SelectedMode == SelectedPBSAndTaskTypeMode.һ��һ)//һ��һУ��PBSѡ����Ҫ���Ϲ���
            {
                //���ѡ���ÿ�������ڵ��µ��ӽڵ�֮���Ƿ�����
                foreach (TreeNode tn in SelectedPBSFirstNode)
                {
                    if (SelectPBSNodeIsSuccession(tn) == false)
                    {
                        MessageBox.Show("PBS�ڵ㡰" + tn.FullPath + "����ѡ���˲��������ӽڵ㣬���飡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tn.Expand();

                        return;
                    }
                }
                //�ж�ѡ���ÿ���������ڵ��Ƿ���ͬһ�����ڵ�
                for (int i = 0; i < SelectedPBSFirstNode.Count - 1; i++)
                {
                    TreeNode nodePrev = SelectedPBSFirstNode[i];
                    TreeNode nodeNext = SelectedPBSFirstNode[i + 1];

                    if (nodePrev.Parent.Name != nodeNext.Parent.Name)
                    {
                        MessageBox.Show("ѡ��Ķ��PBS�����ڵ㲻����ͬһ���ڵ㣬�ⲻ���Ͽ����������飡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
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
                        MessageBox.Show("ѡ��PBS�ڵ㡰" + pbs.Name + "�����ڸ��ڵ�����PBS�����ϣ��ⲻ������ӹ淶�����飡");
                        return;
                    }

                    //SelectedPBS.Add(pbs);
                }

                ProjectTaskTypeTree parentTaskType = wbs.ProjectTaskTypeGUID;
                if (parentTaskType.TypeLevel != ProjectTaskTypeLevel.��λ���� && parentTaskType.TypeLevel != ProjectTaskTypeLevel.�ӵ�λ���� && parentTaskType.TypeLevel != ProjectTaskTypeLevel.רҵ)
                {
                    foreach (TreeNode dicTask in SelectedTaskTypeFirstNode)
                    {
                        ProjectTaskTypeTree task = dicTask.Tag as ProjectTaskTypeTree;

                        if (task.SysCode.IndexOf(parentTaskType.SysCode) == -1 && task.Level <= parentTaskType.Level)
                        {
                            MessageBox.Show("ѡ���������͡�" + task.Name + "�����ڸ��ڵ����������������ϻ�㼶���ڸ��ڵ��������Ͳ㼶���ⲻ������ӹ淶�����飡");
                            return;
                        }

                        //SelectedTaskType.Add(task);
                    }
                }
                //else
                //{
                //    foreach (var dicTask in listCheckedTaskTypeNode)
                //    {
                //        ProjectTaskTypeTree task = dicTask.Value.Tag as ProjectTaskTypeTree;

                //        SelectedTaskType.Add(task);
                //    }
                //}
            }
            else//ѡ����ڵ�
            {
                //foreach (var dicPBS in listCheckedPBSNode)
                //{
                //    PBSTree pbs = dicPBS.Value.Tag as PBSTree;
                //    SelectedPBS.Add(pbs);
                //}

                //foreach (var dicTask in listCheckedTaskTypeNode)
                //{
                //    ProjectTaskTypeTree task = dicTask.Value.Tag as ProjectTaskTypeTree;
                //    SelectedTaskType.Add(task);
                //}

                if (SelectedMode == SelectedPBSAndTaskTypeMode.���һ)
                {
                    if (SelectedTaskTypeFirstNode.Count > 1)
                    {
                        MessageBox.Show("WBSֻ����һ�����ڵ㣬��ǰ����ģʽ�£�������������ֻ��ѡ��һ�������ڵ㣡");
                        return;
                    }
                }
                else
                {
                    if (SelectedPBSFirstNode.Count > 1)
                    {
                        MessageBox.Show("WBSֻ����һ�����ڵ㣬��ǰ����ģʽ�£�PBSֻ��ѡ��һ�������ڵ㣡");
                        return;
                    }
                }
            }

            //foreach (var dicPBS in listCheckedPBSNode)
            //{
            //    PBSTree pbs = dicPBS.Value.Tag as PBSTree;

            //    SelectedPBS.Add(pbs);

            //    foreach (var dicTask in listCheckedTaskTypeNode)
            //    {
            //        ProjectTaskTypeTree task = dicTask.Value.Tag as ProjectTaskTypeTree;
            //        var query = from r in ListRuleDtl
            //                    where r.PBSType.Trim() == pbs.StructTypeName &&
            //                    r.TaskType.Trim() == task.TypeLevel.ToString()
            //                    select r;

            //        if (query == null || query.Count() == 0)
            //        {
            //            MessageBox.Show("ѡ���PBS�ڵ㡰" + pbs.Name + "[" + pbs.StructTypeName + "]�����������ͽڵ㡰" + task.Name + "[" + task.TypeLevel + "]����������ӹ������飡");
            //            return;
            //        }

            //        SelectedTaskType.Add(task);
            //    }
            //}

            //���ýڵ�����·��
            //for (int i = 0; i < SelectedPBS.Count; i++)
            //{
            //    SelectedPBS[i].FullPath = GetFullPath(SelectedPBS[i]);
            //}
            //for (int i = 0; i < SelectedTaskType.Count; i++)
            //{
            //    SelectedTaskType[i].FullPath = GetFullPath(SelectedTaskType[i]);
            //}


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
                if (tn.Checked)//�ҵ�ѡ���ÿһ�����ڵ�
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
                if (tn.Checked)//�ҵ�ѡ���ÿһ�����ڵ�
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
        /// �ж�ѡ���PBS�ڵ㼰���ӽڵ��Ƿ�����
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool SelectPBSNodeIsSuccession(TreeNode parentNode)
        {
            //��ѯ�ڵ���
            var listLeafNode = from n in listCheckedPBSNode
                               where (n.Value.Tag as PBSTree).SysCode.IndexOf((parentNode.Tag as PBSTree).SysCode) > -1
                               select n;

            foreach (var dic in listLeafNode)
            {
                if (dic.Key != parentNode.Name)//��Ҷ�ڵ㲻�Ƕ����ڵ�
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
        /// �ж�ѡ����������ͽڵ㼰���ӽڵ��Ƿ�����
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool SelectTaskTypeNodeIsSuccession(TreeNode parentNode)
        {
            //��ѯ�ڵ���
            var listLeafNode = from n in listCheckedTaskTypeNode
                               where (n.Value.Tag as ProjectTaskTypeTree).SysCode.IndexOf((parentNode.Tag as ProjectTaskTypeTree).SysCode) > -1
                               select n;

            foreach (var dic in listLeafNode)
            {
                if (dic.Key != parentNode.Name)//��Ҷ�ڵ㲻�Ƕ����ڵ�
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




        bool isPBSSelectNodeInvoke = false;//�Ƿ���ѡ��(���)�ڵ�ʱ����
        bool startPBSNodeCheckedState = false;//��shift��ѡ�ֵܽڵ�ʱ��ʼ�ڵ��ѡ��״̬
        private void tvwPBS_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                #region ������ڵ�ʱʵ�ֶ�ѡ
                bool isMultiSelect = false;
                TreeNode preselectionNode;//Ԥѡ��ڵ�

                preselectionNode = e.Node;

                if (optPBSNode != null && optPBSNode.Name != preselectionNode.Name
                    && ((optPBSNode.Parent == preselectionNode.Parent) || (optPBSNode.Parent != null && preselectionNode.Parent != null && optPBSNode.Parent.Name == preselectionNode.Parent.Name)))//Nameȡ�Ķ����ID
                    isMultiSelect = true;
                else
                    isMultiSelect = false;

                if (optPBSNode != null)
                    startPBSNodeCheckedState = optPBSNode.Checked;

                if (isMultiSelect && (Control.ModifierKeys & Keys.Shift) != 0 && (Control.ModifierKeys & Keys.Control) != 0)//���ͬʱ������ctrl+shift
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

                        isPBSSelectNodeInvoke = true;//���ñ�־����check�¼��в��ٴ���

                        if (startPBSNodeCheckedState)//�����ʼ�ڵ㵱ǰΪѡ�У���ȡ��ѡ��
                        {
                            TreeNode tempNode = new TreeNode();
                            tn.BackColor = tempNode.BackColor;
                            tn.ForeColor = tempNode.ForeColor;

                            //if (listCheckedPBSNode.ContainsKey(tn.Name))
                            //    listCheckedPBSNode.Remove(tn.Name);

                            tn.Checked = false;
                        }
                        else//�����ʼ�ڵ㵱ǰΪδѡ�У�������ѡ��
                        {
                            tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                            tn.ForeColor = ColorTranslator.FromHtml("#000000");

                            //if (listCheckedPBSNode.ContainsKey(tn.Name))
                            //    listCheckedPBSNode[tn.Name] = tn;
                            //else
                            //    listCheckedPBSNode.Add(tn.Name, tn);

                            tn.Checked = true;
                        }

                        SetPBSChildCheckedByMultiSel(tn);
                    }
                }
                else if (isMultiSelect && (Control.ModifierKeys & Keys.Shift) != 0)//���ͬʱ������shift
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

                        isPBSSelectNodeInvoke = true;//���ñ�־����check�¼��в��ٴ���

                        if (startPBSNodeCheckedState)//�����ʼ�ڵ㵱ǰΪѡ�У���ȡ��ѡ��
                        {
                            tn.Checked = false;

                            TreeNode tempNode = new TreeNode();
                            tn.BackColor = tempNode.BackColor;
                            tn.ForeColor = tempNode.ForeColor;

                            //if (listCheckedPBSNode.ContainsKey(tn.Name))
                            //    listCheckedPBSNode.Remove(tn.Name);
                        }
                        else//�����ʼ�ڵ㵱ǰΪδѡ�У�������ѡ��
                        {
                            tn.Checked = true;

                            tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                            tn.ForeColor = ColorTranslator.FromHtml("#000000");

                            //if (listCheckedPBSNode.ContainsKey(tn.Name))
                            //    listCheckedPBSNode[tn.Name] = tn;
                            //else
                            //    listCheckedPBSNode.Add(tn.Name, tn);
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
                #region ������ѡ�����
                if ((Control.ModifierKeys & Keys.Shift) != 0 || (Control.ModifierKeys & Keys.Control) != 0)//���ͬʱ������Ctrl //(Control.ModifierKeys & Keys.Shift) != 0 || 
                {
                    if (e.Node.Checked)
                    {
                        e.Node.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                        e.Node.ForeColor = ColorTranslator.FromHtml("#000000");

                        //e.Node.BackColor = SystemColors.Control;
                        //e.Node.ForeColor = SystemColors.ControlText;

                        //if (listCheckedPBSNode.ContainsKey(e.Node.Name))
                        //    listCheckedPBSNode[e.Node.Name] = e.Node;
                        //else
                        //    listCheckedPBSNode.Add(e.Node.Name, e.Node);
                    }
                    else
                    {
                        TreeNode tempNode = new TreeNode();
                        e.Node.BackColor = tempNode.BackColor;
                        e.Node.ForeColor = tempNode.ForeColor;

                        //if (listCheckedPBSNode.ContainsKey(e.Node.Name))
                        //    listCheckedPBSNode.Remove(e.Node.Name);
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

                        //if (listCheckedPBSNode.ContainsKey(e.Node.Name))
                        //    listCheckedPBSNode[e.Node.Name] = e.Node;
                        //else
                        //    listCheckedPBSNode.Add(e.Node.Name, e.Node);
                    }
                    else
                    {
                        TreeNode tempNode = new TreeNode();
                        e.Node.BackColor = tempNode.BackColor;
                        e.Node.ForeColor = tempNode.ForeColor;

                        //if (listCheckedPBSNode.ContainsKey(e.Node.Name))
                        //    listCheckedPBSNode.Remove(e.Node.Name);
                    }
                }
                #endregion
            }
        }

        bool isTaskTypeSelectNodeInvoke = false;//�Ƿ���ѡ��(���)�ڵ�ʱ����
        bool startTaskTypeNodeCheckedState = false;//��shift��ѡ�ֵܽڵ�ʱ��ʼ�ڵ��ѡ��״̬
        void tvwTaskType_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                #region ������ڵ�ʱʵ�ֶ�ѡ
                bool isMultiSelect = false;
                TreeNode preselectionNode;//Ԥѡ��ڵ�

                preselectionNode = e.Node;

                if (optTaskTypeNode != null && optTaskTypeNode.Name != preselectionNode.Name
                    && optTaskTypeNode.Parent != null && preselectionNode.Parent != null && optTaskTypeNode.Parent.Name == preselectionNode.Parent.Name)//Nameȡ�Ķ����ID
                    isMultiSelect = true;
                else
                    isMultiSelect = false;

                if (optTaskTypeNode != null)
                    startTaskTypeNodeCheckedState = optTaskTypeNode.Checked;

                if (isMultiSelect && (Control.ModifierKeys & Keys.Shift) != 0 && (Control.ModifierKeys & Keys.Control) != 0)//���ͬʱ������ctrl+shift
                {
                    int currNodeIndex = optTaskTypeNode.Index;
                    int preselectNodeIndex = preselectionNode.Index;

                    int startIndex = currNodeIndex < preselectNodeIndex ? currNodeIndex : preselectNodeIndex;
                    int endIndex = currNodeIndex < preselectNodeIndex ? preselectNodeIndex : currNodeIndex;

                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        TreeNode tn = optTaskTypeNode.Parent.Nodes[i];

                        isTaskTypeSelectNodeInvoke = true;//���ñ�־����check�¼��в��ٴ���

                        if (startTaskTypeNodeCheckedState)//�����ʼ�ڵ㵱ǰΪѡ�У���ȡ��ѡ��
                        {
                            TreeNode tempNode = new TreeNode();
                            tn.BackColor = tempNode.BackColor;
                            //tn.ForeColor = tempNode.ForeColor;

                            //if (listCheckedTaskTypeNode.ContainsKey(tn.Name))
                            //    listCheckedTaskTypeNode.Remove(tn.Name);

                            tn.Checked = false;
                        }
                        else//�����ʼ�ڵ㵱ǰΪδѡ�У�������ѡ��
                        {
                            tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                            //tn.ForeColor = ColorTranslator.FromHtml("#000000");

                            //if (listCheckedTaskTypeNode.ContainsKey(tn.Name))
                            //    listCheckedTaskTypeNode[tn.Name] = tn;
                            //else
                            //    listCheckedTaskTypeNode.Add(tn.Name, tn);

                            tn.Checked = true;
                        }

                        SetTaskTypeChildCheckedByMultiSel(tn);
                    }
                }
                else if (isMultiSelect && (Control.ModifierKeys & Keys.Shift) != 0)//���ͬʱ������shift
                {
                    int currNodeIndex = optTaskTypeNode.Index;
                    int preselectNodeIndex = preselectionNode.Index;

                    int startIndex = currNodeIndex < preselectNodeIndex ? currNodeIndex : preselectNodeIndex;
                    int endIndex = currNodeIndex < preselectNodeIndex ? preselectNodeIndex : currNodeIndex;

                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        TreeNode tn = optTaskTypeNode.Parent.Nodes[i];

                        isTaskTypeSelectNodeInvoke = true;//���ñ�־����check�¼��в��ٴ���

                        if (startTaskTypeNodeCheckedState)//�����ʼ�ڵ㵱ǰΪѡ�У���ȡ��ѡ��
                        {
                            tn.Checked = false;

                            TreeNode tempNode = new TreeNode();
                            tn.BackColor = tempNode.BackColor;
                            //tn.ForeColor = tempNode.ForeColor;

                            //if (listCheckedTaskTypeNode.ContainsKey(tn.Name))
                            //    listCheckedTaskTypeNode.Remove(tn.Name);
                        }
                        else//�����ʼ�ڵ㵱ǰΪδѡ�У�������ѡ��
                        {
                            tn.Checked = true;

                            tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                            //tn.ForeColor = ColorTranslator.FromHtml("#000000");

                            //if (listCheckedTaskTypeNode.ContainsKey(tn.Name))
                            //    listCheckedTaskTypeNode[tn.Name] = tn;
                            //else
                            //    listCheckedTaskTypeNode.Add(tn.Name, tn);
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
                #region ������ѡ�����
                if ((Control.ModifierKeys & Keys.Shift) != 0 || (Control.ModifierKeys & Keys.Control) != 0)//���ͬʱ������Ctrl //(Control.ModifierKeys & Keys.Shift) != 0 || 
                {
                    if (e.Node.Checked)
                    {
                        e.Node.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                        //e.Node.ForeColor = ColorTranslator.FromHtml("#000000");

                        //if (listCheckedTaskTypeNode.ContainsKey(e.Node.Name))
                        //    listCheckedTaskTypeNode[e.Node.Name] = e.Node;
                        //else
                        //    listCheckedTaskTypeNode.Add(e.Node.Name, e.Node);
                    }
                    else
                    {
                        TreeNode tempNode = new TreeNode();
                        e.Node.BackColor = tempNode.BackColor;
                        //e.Node.ForeColor = tempNode.ForeColor;

                        //if (listCheckedTaskTypeNode.ContainsKey(e.Node.Name))
                        //    listCheckedTaskTypeNode.Remove(e.Node.Name);
                    }

                    SetTaskTypeChildChecked(e.Node);
                }
                else
                {
                    if (e.Node.Checked)
                    {
                        e.Node.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                        //e.Node.ForeColor = ColorTranslator.FromHtml("#000000");

                        //if (listCheckedTaskTypeNode.ContainsKey(e.Node.Name))
                        //    listCheckedTaskTypeNode[e.Node.Name] = e.Node;
                        //else
                        //    listCheckedTaskTypeNode.Add(e.Node.Name, e.Node);
                    }
                    else
                    {
                        TreeNode tempNode = new TreeNode();
                        e.Node.BackColor = tempNode.BackColor;
                        //e.Node.ForeColor = tempNode.ForeColor;

                        //if (listCheckedTaskTypeNode.ContainsKey(e.Node.Name))
                        //    listCheckedTaskTypeNode.Remove(e.Node.Name);
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

                    //if (listCheckedPBSNode.ContainsKey(tn.Name))
                    //    listCheckedPBSNode[tn.Name] = tn;
                    //else
                    //    listCheckedPBSNode.Add(tn.Name, tn);
                }
                else
                {
                    TreeNode tempNode = new TreeNode();
                    tn.BackColor = tempNode.BackColor;
                    tn.ForeColor = tempNode.ForeColor;

                    //if (listCheckedPBSNode.ContainsKey(tn.Name))
                    //    listCheckedPBSNode.Remove(tn.Name);
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

                    //if (listCheckedTaskTypeNode.ContainsKey(tn.Name))
                    //    listCheckedTaskTypeNode[tn.Name] = tn;
                    //else
                    //    listCheckedTaskTypeNode.Add(tn.Name, tn);
                }
                else
                {
                    TreeNode tempNode = new TreeNode();
                    tn.BackColor = tempNode.BackColor;
                    //tn.ForeColor = tempNode.ForeColor;//�����˹������ɫ����

                    //if (listCheckedTaskTypeNode.ContainsKey(tn.Name))
                    //    listCheckedTaskTypeNode.Remove(tn.Name);
                }
            }
        }


        private void SetPBSChildCheckedByMultiSel(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                SetPBSChildCheckedByMultiSel(tn);

                isPBSSelectNodeInvoke = true;

                if (startPBSNodeCheckedState)//�����ʼ�ڵ㵱ǰΪѡ�У���ȡ��ѡ��
                {
                    TreeNode tempNode = new TreeNode();
                    tn.BackColor = tempNode.BackColor;
                    tn.ForeColor = tempNode.ForeColor;

                    //if (listCheckedPBSNode.ContainsKey(tn.Name))
                    //    listCheckedPBSNode.Remove(tn.Name);

                    tn.Checked = false;
                }
                else//�����ʼ�ڵ㵱ǰΪδѡ�У�������ѡ��
                {
                    tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    tn.ForeColor = ColorTranslator.FromHtml("#000000");

                    //if (listCheckedPBSNode.ContainsKey(tn.Name))
                    //    listCheckedPBSNode[tn.Name] = tn;
                    //else
                    //    listCheckedPBSNode.Add(tn.Name, tn);

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

                if (startTaskTypeNodeCheckedState)//�����ʼ�ڵ㵱ǰΪѡ�У���ȡ��ѡ��
                {
                    TreeNode tempNode = new TreeNode();
                    tn.BackColor = tempNode.BackColor;
                    //tn.ForeColor = tempNode.ForeColor;//�����˹������ɫ����

                    //if (listCheckedTaskTypeNode.ContainsKey(tn.Name))
                    //    listCheckedTaskTypeNode.Remove(tn.Name);

                    tn.Checked = false;
                }
                else//�����ʼ�ڵ㵱ǰΪδѡ�У�������ѡ��
                {
                    tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    //tn.ForeColor = ColorTranslator.FromHtml("#000000");

                    //if (listCheckedTaskTypeNode.ContainsKey(tn.Name))
                    //    listCheckedTaskTypeNode[tn.Name] = tn;
                    //else
                    //    listCheckedTaskTypeNode.Add(tn.Name, tn);

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
                MessageBox.Show("��ʾ����" + ExceptionUtil.ExceptionMessage(exp));
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
                MessageBox.Show("��ʾ����" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        private void LoadPBSTree(TreeNode parentGWBSNode)
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwPBS.Nodes.Clear();
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                //��������
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

                //IList list = listAll[0] as IList;
                foreach (PBSTree childNode in list)
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
                MessageBox.Show("��ѯ����" + ExceptionUtil.ExceptionMessage(e));
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
                //IList list = listAll[0] as IList;
                foreach (ProjectTaskTypeTree childNode in list)
                {
                    //if (childNode.State == 0)
                    //    continue;

                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = childNode.Name;
                    tnTmp.Tag = childNode;

                    if (childNode.TypeStandard == ProjectTaskTypeStandard.���)
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
                MessageBox.Show("��ѯ����" + ExceptionUtil.ExceptionMessage(e));
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

        //ѡ��ģʽ pbs-�������ͣ����һ��
        private void rbSelectedMode1_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSelectedMode1.Checked)
            {
                SelectedMode = SelectedPBSAndTaskTypeMode.���һ;

                tvwTaskType.CheckBoxes = true;
                IsSingleSelectTaskType = false;
                listCheckedTaskTypeNode.Clear();
                SelectedTaskType.Clear();
                SelectedTaskTypeFirstNode.Clear();

                tvwTaskType.Refresh();

                InitOpenAndSelectedTaskTypeTreeNode();
            }
        }

        //ѡ��ģʽ pbs-�������ͣ�һ��һ��
        private void rbSelectedMode2_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSelectedMode2.Checked)
            {
                SelectedMode = SelectedPBSAndTaskTypeMode.һ��һ;

                tvwTaskType.CheckBoxes = false;
                IsSingleSelectTaskType = true;
                listCheckedTaskTypeNode.Clear();
                SelectedTaskType.Clear();
                SelectedTaskTypeFirstNode.Clear();

                tvwTaskType.Refresh();

                InitOpenAndSelectedTaskTypeTreeNode();
            }
        }

        /// <summary>
        /// ѡ��PBS-��������ģʽ
        /// </summary>
        public enum SelectedPBSAndTaskTypeMode
        {
            ���һ = 1,
            һ��һ = 2
        }
    }
}
