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
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng
{
    public partial class VCostAccountSubject : TBasicDataView
    {
        private CostAccountSubject oprNode = null;
        private bool isNew = true;
        //有权限的业务组织
        private IList lstInstance;

        private Hashtable hashtableRules = new Hashtable();

        private Dictionary<string, TreeNode> listCheckedNode = new Dictionary<string, TreeNode>();
        private List<TreeNode> listCopyNode = new List<TreeNode>();

        CurrentProjectInfo projectInfo = null;

        public MCostAccountSubject model;

        public VCostAccountSubject(MCostAccountSubject mot)
        {
            model = mot;

            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();

            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();

            LoadCostAccountSubjectTree();

            //RefreshControls(MainViewState.Browser);
            //cbIfSubBalanceFlag.Items.AddRange(new object[] { "必须纳入结算", "不需纳入结算", "由工长确定", "由预算员确定", "由合同确定" });
            cbIfSubBalanceFlag.Items.AddRange(new object[] { "必须纳入结算", "不需纳入结算"});
        }

        private void InitEvents()
        {
            tvwCategory.NodeMouseClick += new TreeNodeMouseClickEventHandler(tvwCategory_NodeMouseClick);

            tvwCategory.AfterCheck += new TreeViewEventHandler(tvwCategory_AfterCheck);
            tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);

            mnuTree.ItemClicked += new ToolStripItemClickedEventHandler(mnuTree_ItemClicked);

            tvwCategory.ItemDrag += new ItemDragEventHandler(tvwCategory_ItemDrag);
            tvwCategory.DragEnter += new DragEventHandler(tvwCategory_DragEnter);
            tvwCategory.DragDrop += new DragEventHandler(tvwCategory_DragDrop);
            tvwCategory.DragOver += new DragEventHandler(tvwCategory_DragOver);


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

            RefreshControls(MainViewState.Check);
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

        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                oprNode = tvwCategory.SelectedNode.Tag as CostAccountSubject;
                this.GetNodeDetail();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
            }
        }

        private void GetNodeDetail()
        {
            try
            {
                ClearAll();

                this.txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath;
                this.txtName.Text = oprNode.Name;
                this.txtCode.Text = oprNode.Code;
                this.txtOwner.Text = oprNode.OwnerName;
                this.txtOwner.Tag = oprNode.OwnerGUID;
                if (oprNode.SubjectState != 0)
                    this.txtState.Text = oprNode.SubjectState.ToString();
                else
                    this.txtState.Text = CostAccountSubjectState.制定.ToString();
                this.txtDesc.Text = oprNode.Describe;
                this.txtCostSubject.Text = oprNode.AccountingSubjectName;
                this.txtCostSubject.Tag = oprNode.AccountingSubjectGUID;
                this.txtSummary.Text = oprNode.Summary;
                if (oprNode.IfSubBalanceFlag == 1)
                {
                    this.cbIfSubBalanceFlag.Text = "必须纳入结算";
                }
                if (oprNode.IfSubBalanceFlag == 2)
                {
                    this.cbIfSubBalanceFlag.Text = "不需纳入结算";
                }
                if (oprNode.IfSubBalanceFlag == 3)
                {
                    this.cbIfSubBalanceFlag.Text = "由工长确定";
                }
                if (oprNode.IfSubBalanceFlag == 4)
                {
                    this.cbIfSubBalanceFlag.Text = "由预算员确定";
                }
                if (oprNode.IfSubBalanceFlag == 5)
                {
                    this.cbIfSubBalanceFlag.Text = "由合同确定";
                }

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

            txtOwner.Text = ConstObject.LoginPersonInfo.Name;
            txtOwner.Tag = ConstObject.LoginPersonInfo.Id;

            if (txtOwner.Result != null)
                txtOwner.Result.Clear();
            this.txtDesc.Text = "";
            this.txtSummary.Text = "";
            this.txtState.Text = "";
            this.cbIfSubBalanceFlag.Text = null;
        }

        private void DeleteNode()
        {
            try
            {
                if (!ValideDelete())
                    return;
                bool reset = false;
                //父节点只有这一个子节点，并且父节点有权限操作，删除后要重新设置父节点tag
                if (tvwCategory.SelectedNode.Parent.Nodes.Count == 1)// && ConstMethod.Contains(lstInstance, tvwCategory.SelectedNode.Parent.Tag as CategoryNode)
                {
                    reset = true;
                }
                model.DeleteCostAccountSubject(oprNode);

                if (reset)
                {
                    CostAccountSubject org = model.GetCostAccountSubjectById((tvwCategory.SelectedNode.Parent.Tag as CostAccountSubject).Id);
                    if (org != null)
                        tvwCategory.SelectedNode.Parent.Tag = org;
                }

                //如果复制的节点有勾选的从选中集合中移除
                if (tvwCategory.SelectedNode.Checked)
                {
                    if (listCheckedNode.ContainsKey(tvwCategory.SelectedNode.Name))
                        listCheckedNode.Remove(tvwCategory.SelectedNode.Name);

                    RemoveChildChecked(tvwCategory.SelectedNode);
                }

                this.tvwCategory.Nodes.Remove(this.tvwCategory.SelectedNode);
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
                    MessageBox.Show("该节点被资源耗用定额或其它数据所引用，删除前请先删除引用的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("删除节点出错：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        private bool ValideDelete()
        {
            try
            {
                TreeNode tn = tvwCategory.SelectedNode;
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

        void add_Click(object sender, EventArgs e)
        {
            try
            {
                ClearAll();
                oprNode = new CostAccountSubject();
                oprNode.SubjectState = CostAccountSubjectState.制定;
                txtState.Text = oprNode.SubjectState.ToString();

                oprNode.TheProjectGUID = projectInfo.Id;
                oprNode.TheProjectName = projectInfo.Name;

                oprNode.ParentNode = this.tvwCategory.SelectedNode.Tag as CostAccountSubject;

                txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath + "\\";
                txtCode.Focus();
            }
            catch (Exception exp)
            {
                MessageBox.Show("增加节点出错：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        void delete_Click(object sender, EventArgs e)
        {
            DeleteNode();
        }

        void saveItem_Click(object sender, EventArgs e)
        {
            SaveView();
        }

        private bool ValideSave()
        {
            try
            {
                if (oprNode == null)
                {
                    oprNode = new CostAccountSubject();
                    oprNode.SubjectState = CostAccountSubjectState.制定;
                    txtState.Text = oprNode.SubjectState.ToString();
                }
                else if (oprNode.SubjectState == 0)
                {
                    oprNode.SubjectState = CostAccountSubjectState.制定;
                    txtState.Text = oprNode.SubjectState.ToString();
                }

                if (string.IsNullOrEmpty(oprNode.TheProjectGUID) || string.IsNullOrEmpty(oprNode.TheProjectName))
                {
                    oprNode.TheProjectGUID = projectInfo.Id;
                    oprNode.TheProjectName = projectInfo.Name;
                }
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("名称不能为空!");
                    txtName.Focus();
                    return false;
                }
                if (txtCode.Text.Trim() == "")
                {
                    MessageBox.Show("编码不能为空!");
                    txtCode.Focus();
                    return false;
                }
                if (txtOwner.Text.Trim() == "")
                {
                    MessageBox.Show("责任者不能为空!");
                    txtOwner.Focus();
                    return false;
                }
                oprNode.Name = txtName.Text.Trim();
                oprNode.Code = txtCode.Text.Trim();
                if (txtOwner.Result != null && txtOwner.Result.Count > 0)
                {
                    PersonInfo per = txtOwner.Result[0] as PersonInfo;
                    if (per != null)
                    {
                        oprNode.OwnerGUID = per.Id;
                        oprNode.OwnerName = per.Name;
                    }
                }
                else if (txtOwner.Tag == null)
                {
                    MessageBox.Show("请选择责任人！");
                    txtOwner.Focus();
                    return false;
                }

                oprNode.Describe = txtDesc.Text.Trim();
                oprNode.Summary = txtSummary.Text.Trim();
                if (!string.IsNullOrEmpty(cbIfSubBalanceFlag.Text) && cbIfSubBalanceFlag.SelectedItem != null)
                {
                    if (cbIfSubBalanceFlag.SelectedItem.Equals("必须纳入结算"))
                    {
                        oprNode.IfSubBalanceFlag = 1;
                    }
                    if (cbIfSubBalanceFlag.SelectedItem.Equals("不需纳入结算"))
                    {
                        oprNode.IfSubBalanceFlag = 2;
                    }
                    //if (cbIfSubBalanceFlag.SelectedItem.Equals("由工长确定"))
                    //{
                    //    oprNode.IfSubBalanceFlag = 3;
                    //}
                    //if (cbIfSubBalanceFlag.SelectedItem.Equals("由预算员确定"))
                    //{
                    //    oprNode.IfSubBalanceFlag = 4;
                    //}
                    //if (cbIfSubBalanceFlag.SelectedItem.Equals("由合同确定"))
                    //{
                    //    oprNode.IfSubBalanceFlag = 5;
                    //}
                }
                return true;

            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
                return false;
            }
        }

        public void Start()
        {
            RefreshState(MainViewState.Browser);

            tvwCategory.ItemDrag += new ItemDragEventHandler(tvwCategory_ItemDrag);
            tvwCategory.DragEnter += new DragEventHandler(tvwCategory_DragEnter);
            tvwCategory.DragDrop += new DragEventHandler(tvwCategory_DragDrop);
            tvwCategory.DragOver += new DragEventHandler(tvwCategory_DragOver);

            LoadCostAccountSubjectTree();
        }

        private void LoadCostAccountSubjectTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();
                IList list = model.GetCostAccountSubjectByInstance();
                //lstInstance = listAll[1] as IList;
                //IList list = listAll[0] as IList;

                if (list.Count > 0)
                {
                    IEnumerable<CostAccountSubject> listTemp = from t in list.OfType<CostAccountSubject>()
                                                               where t.CategoryNodeType == NodeType.RootNode || t.ParentNode == null
                                                               select t;

                    if (listTemp != null && listTemp.Count() > 0)
                    {
                        CostAccountSubject root = listTemp.ElementAt(0);

                        if (string.IsNullOrEmpty(root.TheProjectGUID))
                        {
                            root.Name = projectInfo.Name;
                            root.SysCode = root.Id + ".";

                            root.TheProjectGUID = projectInfo.Id;
                            root.TheProjectName = projectInfo.Name;

                            if (string.IsNullOrEmpty(root.Code))
                                root.Code = projectInfo.Code + model.GetCode(typeof(CostAccountSubject));

                            model.SaveCostAccountSubject(root);

                            list = model.GetCostAccountSubjectByInstance();
                            //lstInstance = listAll[1] as IList;
                            //list = listAll[0] as IList;
                        }
                    }
                }
                else
                {
                    IList listAdd = new List<CostAccountSubject>();

                    CostAccountSubject root = new CostAccountSubject();
                    root.Name = projectInfo.Name;
                    root.Code = projectInfo.Code + model.GetCode(typeof(CostAccountSubject));
                    root.TheProjectGUID = projectInfo.Id;
                    root.TheProjectName = projectInfo.Name;

                    listAdd.Add(root);
                    model.SaveCostAccountSubjectRootNode(listAdd);

                    list = model.GetCostAccountSubjectByInstance();
                    //lstInstance = listAll[1] as IList;
                    //list = listAll[0] as IList;
                }

                foreach (CostAccountSubject childNode in list)
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

        public override void RefreshControls(MainViewState state)
        {
            switch (state)
            {
                case MainViewState.Modify:

                    this.mnuTree.Items["撤销"].Enabled = true;
                    this.mnuTree.Items["保存节点"].Enabled = true;
                    this.mnuTree.Items["增加子节点"].Enabled = false;
                    this.mnuTree.Items["修改节点"].Enabled = false;
                    this.mnuTree.Items["删除节点"].Enabled = false;

                    this.mnuTree.Items["发布节点"].Enabled = false;
                    this.mnuTree.Items["冻结节点"].Enabled = false;
                    this.mnuTree.Items["作废节点"].Enabled = false;

                    this.linkAdd.Enabled = false;
                    this.linkUpdate.Enabled = false;
                    this.linkDelete.Enabled = false;
                    this.linkCancel.Enabled = true;
                    this.linkSave.Enabled = true;

                    txtCode.ReadOnly = false;
                    txtName.ReadOnly = false;
                    txtOwner.ReadOnly = false;
                    txtDesc.ReadOnly = false;
                    txtSummary.ReadOnly = false;
                    txtCostSubject.ReadOnly = false;
                    cbIfSubBalanceFlag.Enabled = true;

                    txtState.ReadOnly = true;
                    break;

                case MainViewState.Browser:

                    this.mnuTree.Items["增加子节点"].Enabled = false;
                    this.mnuTree.Items["修改节点"].Enabled = false;
                    this.mnuTree.Items["删除节点"].Enabled = false;

                    this.mnuTree.Items["撤销"].Enabled = false;
                    this.mnuTree.Items["保存节点"].Enabled = false;

                    this.mnuTree.Items["发布节点"].Enabled = false;
                    this.mnuTree.Items["冻结节点"].Enabled = false;
                    this.mnuTree.Items["作废节点"].Enabled = false;

                    this.linkAdd.Enabled = false;
                    this.linkUpdate.Enabled = false;

                    this.linkCancel.Enabled = false;
                    this.linkSave.Enabled = false;

                    this.linkDelete.Enabled = false;

                    if (oprNode.SubjectState == 0 || oprNode.SubjectState == CostAccountSubjectState.制定)
                    {
                        this.mnuTree.Items["增加子节点"].Enabled = true;
                        this.mnuTree.Items["修改节点"].Enabled = true;
                        this.mnuTree.Items["删除节点"].Enabled = true;
                        this.mnuTree.Items["发布节点"].Enabled = true;

                        this.linkAdd.Enabled = true;
                        this.linkUpdate.Enabled = true;
                        this.linkDelete.Enabled = true;
                    }
                    else if (oprNode.SubjectState == CostAccountSubjectState.发布)
                    {
                        this.mnuTree.Items["增加子节点"].Enabled = true;
                        this.mnuTree.Items["冻结节点"].Enabled = true;
                        this.mnuTree.Items["作废节点"].Enabled = true;

                        linkAdd.Enabled = true;
                    }
                    else if (oprNode.SubjectState == CostAccountSubjectState.冻结)
                    {
                        this.mnuTree.Items["发布节点"].Enabled = true;
                        this.mnuTree.Items["作废节点"].Enabled = true;
                    }

                    if (tvwCategory.SelectedNode != null && tvwCategory.SelectedNode.Parent == null)
                        this.linkDelete.Enabled = false;

                    txtCode.ReadOnly = true;
                    txtName.ReadOnly = true;
                    txtOwner.ReadOnly = true;
                    txtDesc.ReadOnly = true;
                    txtSummary.ReadOnly = true;
                    txtCostSubject.ReadOnly = true;
                    cbIfSubBalanceFlag.Enabled = false;

                    txtState.ReadOnly = true;
                    break;
            }
        }

        public override bool ModifyView()
        {
            return true;
        }

        public override bool CancelView()
        {
            try
            {
                LoadCostAccountSubjectTree();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        public override void RefreshView()
        {
            try
            {
                LoadCostAccountSubjectTree();
            }
            catch (Exception e)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(e));
            }
        }

        public override bool SaveView()
        {
            bool isNew = false;
            try
            {
                if (!ValideSave())
                    return false;
                if (oprNode.Id == null)
                    isNew = true;
                else
                    isNew = false;
                oprNode = model.SaveCostAccountSubject(oprNode);

                if (isNew)
                {
                    //要添加子节点的节点以前没有子节点，需要重新设置Tag
                    if (tvwCategory.SelectedNode.Nodes.Count == 0)
                        tvwCategory.SelectedNode.Tag = oprNode.ParentNode;
                    TreeNode tn = this.tvwCategory.SelectedNode.Nodes.Add(oprNode.Id.ToString(), oprNode.Name.ToString());
                    //新增节点要有权限操作
                    //lstInstance.Add(oprNode);
                    tn.Tag = oprNode;
                    this.tvwCategory.SelectedNode = tn;
                    tn.Expand();
                }
                else
                {
                    this.tvwCategory.SelectedNode.Text = oprNode.Name.ToString();
                }
                return true;
            }
            catch (Exception exp)
            {
                if (exp.InnerException != null && exp.InnerException.Message.Contains("违反唯一约束条件"))
                    MessageBox.Show("编码必须唯一！");
                else
                    MessageBox.Show("保存组织树错误：" + ExceptionUtil.ExceptionMessage(exp));
            }
            return false;
        }

        #region 节点拖拽移动

        private void tvwCategory_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                CostAccountSubject org = (e.Item as TreeNode).Tag as CostAccountSubject;
                //有权限的节点才允许拖动操作
                if (org != null)// && ConstMethod.Contains(lstInstance, org)
                {
                    DoDragDrop(e.Item, DragDropEffects.All);
                }
            }
        }

        private void tvwCategory_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void tvwCategory_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                Point targetPoint = tvwCategory.PointToClient(new Point(e.X, e.Y));
                TreeNode targetNode = tvwCategory.GetNodeAt(targetPoint);
                //目标节点没有权限不允许操作
                if (targetNode == null)//|| !ConstMethod.Contains(lstInstance, targetNode.Tag as CostAccountSubject)
                    return;
                TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

                if (TreeViewUtil.CanMoveNode(draggedNode, targetNode))
                {
                    //以前的父节点
                    TreeNode oldParentNode = draggedNode.Parent;
                    bool reset = false;
                    //父节点只有这一个子节点，并且父节点有权限操作，移动后要重新设置父节点tag
                    if (oldParentNode.Nodes.Count == 1)// && ConstMethod.Contains(lstInstance, oldParentNode.Tag as CategoryNode)
                    {
                        reset = true;
                    }

                    frmTreeMoveCopy frmTmp = new frmTreeMoveCopy();
                    frmTmp.TargetNode = targetNode;
                    frmTmp.DraggedNode = draggedNode;
                    if (draggedNode.Parent == targetNode.Parent)
                        frmTmp.IsOrder = true;
                    frmTmp.ShowDialog();
                    if (frmTmp.IsOK == true)
                    {
                        //复制树节点
                        if (frmTmp.MoveOrCopy == enmMoveOrCopy.copy)
                        {
                            draggedNode = frmTmp.DraggedNode;
                            CostAccountSubject catTmp = (draggedNode.Tag as CostAccountSubject).Clone();
                            //系统生存一个唯一编码
                            catTmp.Code = projectInfo.Code + model.GetCode(typeof(CostAccountSubject));

                            catTmp.ParentNode = targetNode.Tag as CostAccountSubject;
                            catTmp.OrderNo = model.GetMaxOrderNo(catTmp) + 1;
                            //draggedNode.Tag = catTmp;

                            IList lst = new ArrayList();
                            lst.Add(catTmp);
                            //克隆要复制的节点和其子节点的对象
                            PopulateList(draggedNode, lst, catTmp);
                            lst = model.SaveCostAccountSubjects(lst);
                            //新增节点要有权限操作
                            //(lstInstance as ArrayList).AddRange(lst);
                            //给复制节点的新父节点tag设值
                            targetNode.Tag = (lst[0] as CostAccountSubject).ParentNode;
                            int i = 0;
                            CopyObjToTag(draggedNode, lst, ref i);

                            //如果复制的节点有勾选的加入到选中集合
                            foreach (TreeNode tn in targetNode.Nodes)
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
                        //移动树节点
                        else if (frmTmp.MoveOrCopy == enmMoveOrCopy.move)
                        {
                            CostAccountSubject toObj = targetNode.Tag as CostAccountSubject;
                            IDictionary dic = model.MoveNode(draggedNode.Tag as CostAccountSubject, toObj);
                            if (reset)
                            {
                                CostAccountSubject cat = model.GetCostAccountSubjectById((oldParentNode.Tag as CostAccountSubject).Id);
                                oldParentNode.Tag = cat;
                            }
                            targetNode.Tag = dic[(targetNode.Tag as CostAccountSubject).Id.ToString()];
                            //根据返回的数据进行节点tag赋值
                            ResetTagAfterMove(draggedNode, dic);
                        }
                        //排序
                        else if (draggedNode.Parent == targetNode.Parent)
                        {
                            if (draggedNode.PrevNode != null)
                            {
                                IList result = new ArrayList();
                                CostAccountSubject prevOrg = draggedNode.PrevNode.Tag as CostAccountSubject;
                                SetNextNodeOrder(draggedNode, result, prevOrg.OrderNo + 1);
                                result = model.SaveCostAccountSubjects(result);
                                ResetTagAfterOrder(draggedNode, result, 0);
                            }
                            else
                            {
                                CostAccountSubject fromOrg = draggedNode.Tag as CostAccountSubject;
                                CostAccountSubject toOrg = targetNode.Tag as CostAccountSubject;
                                fromOrg.OrderNo = toOrg.OrderNo - 1;
                                draggedNode.Tag = model.SaveCostAccountSubject(fromOrg);
                            }
                        }
                        //保证拖动后马上修改保存不出错
                        this.tvwCategory.SelectedNode = draggedNode;
                    }
                }//用户如果把节点移到空白区再选中被拖动节点
                else if (targetNode == null)
                {
                    tvwCategory.SelectedNode = draggedNode;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("移动分类出错：" + ExceptionUtil.ExceptionMessage(ee));
            }
        }
        //设置后续节点的排序号
        private void SetNextNodeOrder(TreeNode node, IList list, long order)
        {
            CostAccountSubject org = node.Tag as CostAccountSubject;
            org.OrderNo = order;
            list.Add(org);
            if (node.NextNode != null)
            {
                SetNextNodeOrder(node.NextNode, list, order + 1);
            }
        }
        //排序后重新设置节点的Tag
        private void ResetTagAfterOrder(TreeNode node, IList lst, int i)
        {
            node.Tag = lst[i];
            if (node.NextNode != null)
                ResetTagAfterOrder(node.NextNode, lst, i + 1);
        }
        //移动后重新设置节点的Tag
        private void ResetTagAfterMove(TreeNode node, IDictionary dic)
        {
            node.Tag = dic[(node.Tag as CostAccountSubject).Id.ToString()];
            if (node.Nodes.Count == 0)
                return;
            foreach (TreeNode var in node.Nodes)
            {
                ResetTagAfterMove(var, dic);
            }
        }
        //复制后重新设置节点的Tag
        private void CopyObjToTag(TreeNode node, IList lst, ref int i)
        {
            node.Name = (lst[i] as CostAccountSubject).Id;
            node.Tag = lst[i];

            if (node.Nodes.Count == 0)
                return;
            foreach (TreeNode var in node.Nodes)
            {
                i++;
                CopyObjToTag(var, lst, ref i);
            }
        }

        private void PopulateList(TreeNode node, IList lst, CostAccountSubject parent)
        {
            if (node.Nodes.Count == 0)
                return;

            for (int i = 0; i < node.Nodes.Count; i++)
            {
                TreeNode var = node.Nodes[i];
                CostAccountSubject matCatTmp = (var.Tag as CostAccountSubject).Clone();

                matCatTmp.Code = projectInfo.Code + model.GetCode(typeof(CostAccountSubject));
                matCatTmp.ParentNode = parent;
                matCatTmp.OrderNo = i + 1;
                //var.Tag = matCatTmp;
                lst.Add(matCatTmp);
                PopulateList(var, lst, matCatTmp);
            }
        }

        private void tvwCategory_DragOver(object sender, DragEventArgs e)
        {
            //Point targetPoint = tvwCategory.PointToClient(new Point(e.X, e.Y));
            //tvwCategory.SelectedNode = tvwCategory.GetNodeAt(targetPoint);
        }

        #endregion

        void mnuTree_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text.Trim() == "增加子节点")
            {
                RefreshControls(MainViewState.Modify);
                add_Click(null, new EventArgs());
            }
            else if (e.ClickedItem.Text.Trim() == "修改节点")
            {
                RefreshControls(MainViewState.Modify);
                txtCode.Focus();
            }
            else if (e.ClickedItem.Text.Trim() == "删除节点")
            {
                mnuTree.Hide();
                delete_Click(null, new EventArgs());
                RefreshControls(MainViewState.Browser);
                this.Refresh();
            }
            else if (e.ClickedItem.Text.Trim() == "撤销")
            {
                mnuTree.Hide();
                RefreshControls(MainViewState.Browser);
                this.tvwCategory_AfterSelect(this.tvwCategory, new TreeViewEventArgs(this.tvwCategory.SelectedNode));
            }
            else if (e.ClickedItem.Text.Trim() == "保存节点")
            {
                mnuTree.Hide();
                if (!SaveView()) return;
                this.RefreshControls(MainViewState.Browser);
            }
            else if (e.ClickedItem.Text.Trim() == "复制勾选节点")
            {
                mnuTree.Hide();

                listCopyNode.Clear();
                foreach (var dic in listCheckedNode)
                {
                    listCopyNode.Add(dic.Value);
                }

                RefreshControls(MainViewState.Check);
            }
            else if (e.ClickedItem.Text.Trim() == "粘贴节点")
            {
                mnuTree.Hide();
                SaveCopyNode();

                RefreshControls(MainViewState.Check);
            }
            else if (e.ClickedItem.Text.Trim() == "删除勾选节点")
            {
                mnuTree.Hide();
                DeleteCheckedNode();

                RefreshControls(MainViewState.Check);
            }
            else if (e.ClickedItem.Text.Trim() == "发布节点")
            {
                mnuTree.Hide();
                PublishNode();
                RefreshControls(MainViewState.Browser);
            }
            else if (e.ClickedItem.Text.Trim() == "冻结节点")
            {
                mnuTree.Hide();
                FreezeNode();
                RefreshControls(MainViewState.Browser);
            }
            else if (e.ClickedItem.Text.Trim() == "作废节点")
            {
                mnuTree.Hide();
                CancellationNode();
                RefreshControls(MainViewState.Browser);
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

        private void SaveCopyNode()
        {
            if (listCopyNode.Count > 0)
            {
                IList lst = new ArrayList();
                foreach (TreeNode draggedNode in listCopyNode)
                {
                    CostAccountSubject catTmp = (draggedNode.Tag as CostAccountSubject).Clone();

                    //系统生存一个唯一编码
                    catTmp.Code = projectInfo.Code + model.GetCode(typeof(CostAccountSubject));

                    catTmp.ParentNode = tvwCategory.SelectedNode.Tag as CostAccountSubject;
                    catTmp.OrderNo = model.GetMaxOrderNo(catTmp) + 1;

                    lst.Add(catTmp);
                }
                //保存复制的节点
                lst = model.SaveCostAccountSubjects(lst);
                //新增节点要有权限操作
                //(lstInstance as ArrayList).AddRange(lst);
                //给复制节点的新父节点tag设值
                tvwCategory.SelectedNode.Tag = (lst[0] as CostAccountSubject).ParentNode;

                foreach (CostAccountSubject pbs in lst)
                {
                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = pbs.Id.ToString();
                    tnTmp.Text = pbs.Name;
                    tnTmp.Tag = pbs;

                    tvwCategory.SelectedNode.Nodes.Add(tnTmp);
                }

                listCopyNode.Clear();
            }
        }

        private void DeleteCheckedNode()
        {
            try
            {
                IList list = new ArrayList();
                foreach (var dic in listCheckedNode)
                {
                    if (dic.Value.Parent == null)
                    {
                        MessageBox.Show("根节点不允许删除！");
                        return;
                    }
                    list.Add(dic.Value.Tag as CostAccountSubject);
                }

                string text = "要删除勾选的所有节点吗？该操作将连它们的所有子节点一同删除！";
                if (MessageBox.Show(text, "删除节点", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

                if (model.DeleteCostAccountSubject(list))//删除成功
                {
                    //从PBS树上移除对应的节点
                    foreach (CostAccountSubject pbs in list)
                    {
                        foreach (TreeNode tn in tvwCategory.Nodes)
                        {
                            if (tn.Name == pbs.Id)
                            {
                                tvwCategory.Nodes.Remove(tn);
                                break;
                            }

                            if (tn.Nodes.Count > 0)
                            {
                                if (RemoveTreeNode(tn, pbs))
                                    break;
                            }
                        }
                    }
                    listCheckedNode.Clear();
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Exception ex1 = ex.InnerException;
                while (ex1 != null && !string.IsNullOrEmpty(ex1.Message))
                {
                    message += ex1.Message;

                    ex1 = ex1.InnerException;
                }

                if (message.IndexOf("违反") > -1 && message.IndexOf("约束") > -1)
                {
                    MessageBox.Show("勾选节点中有节点被资源耗用定额或其它数据所引用，删除前请先删除引用的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show(ExceptionUtil.ExceptionMessage(ex));
            }
        }
        private bool RemoveTreeNode(TreeNode parentNode, CostAccountSubject pbs)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Name == pbs.Id)
                {
                    parentNode.Nodes.Remove(tn);
                    return true;
                }
                if (tn.Nodes.Count > 0)
                {
                    if (RemoveTreeNode(tn, pbs))
                        return true;
                }
            }
            return false;
        }

        private void PublishNode()
        {
            try
            {
                oprNode = model.GetCostAccountSubjectById(oprNode.Id) as CostAccountSubject;
                oprNode.SubjectState = CostAccountSubjectState.发布;

                oprNode = model.SaveCostAccountSubject(oprNode);
                tvwCategory.SelectedNode.Tag = oprNode;

                txtState.Text = oprNode.SubjectState.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        private void FreezeNode()
        {
            try
            {
                oprNode = model.GetCostAccountSubjectById(oprNode.Id) as CostAccountSubject;
                oprNode.SubjectState = CostAccountSubjectState.冻结;

                oprNode = model.SaveCostAccountSubject(oprNode);
                tvwCategory.SelectedNode.Tag = oprNode;

                txtState.Text = oprNode.SubjectState.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        private void CancellationNode()
        {
            try
            {
                oprNode = model.GetCostAccountSubjectById(oprNode.Id) as CostAccountSubject;
                oprNode.SubjectState = CostAccountSubjectState.作废;

                oprNode = model.SaveCostAccountSubject(oprNode);
                tvwCategory.SelectedNode.Tag = oprNode;

                txtState.Text = oprNode.SubjectState.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        void tvwCategory_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)// && ConstMethod.Contains(lstInstance, tvwCategory.SelectedNode.Tag as CategoryNode)
            {
                tvwCategory.SelectedNode = e.Node;

                if (e.Node.Parent == null)
                    mnuTree.Items["删除节点"].Enabled = false;

                if (listCheckedNode.Count == 0)
                {
                    mnuTree.Items["复制勾选节点"].Enabled = false;
                    mnuTree.Items["删除勾选节点"].Enabled = false;
                }
                else
                {
                    mnuTree.Items["复制勾选节点"].Enabled = true;
                    mnuTree.Items["删除勾选节点"].Enabled = true;
                }

                if (listCopyNode.Count == 0)
                    mnuTree.Items["粘贴节点"].Enabled = false;
                else
                    mnuTree.Items["粘贴节点"].Enabled = true;


                mnuTree.Show(tvwCategory, new Point(e.X, e.Y));
            }
        }

        public void ReloadTreeNode()
        {
            if (isNew)
            {
                //要添加子节点的节点以前没有子节点，需要重新设置Tag
                if (tvwCategory.SelectedNode.Nodes.Count == 0)
                    tvwCategory.SelectedNode.Tag = oprNode.ParentNode;
                TreeNode tn = this.tvwCategory.SelectedNode.Nodes.Add(oprNode.Id.ToString(), oprNode.Name.ToString());
                //新增节点要有权限操作
                //lstInstance.Add(oprNode);
                tn.Tag = oprNode;
                this.tvwCategory.SelectedNode = tn;
                tn.Expand();
            }
            else
            {
                this.tvwCategory.SelectedNode.Text = oprNode.Name.ToString();
            }
        }

        #region 操作按钮
        private void linkAdd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RefreshControls(MainViewState.Modify);
            add_Click(null, new EventArgs());
        }

        private void linkUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RefreshControls(MainViewState.Modify);
        }

        private void linkDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mnuTree.Hide();
            delete_Click(null, new EventArgs());
            RefreshControls(MainViewState.Browser);
            this.Refresh();
        }

        private void linkCancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mnuTree.Hide();
            RefreshControls(MainViewState.Browser);
            this.tvwCategory_AfterSelect(this.tvwCategory, new TreeViewEventArgs(this.tvwCategory.SelectedNode));
        }

        private void linkSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mnuTree.Hide();
            if (!SaveView()) return;
            this.RefreshControls(MainViewState.Browser);
        }

        private void linkCopy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mnuTree.Hide();

            listCopyNode.Clear();
            foreach (var dic in listCheckedNode)
            {
                listCopyNode.Add(dic.Value);
            }
            RefreshControls(MainViewState.Check);
        }

        private void linkPaste_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mnuTree.Hide();
            SaveCopyNode();
            RefreshControls(MainViewState.Check);
        }

        private void linkDeleteChecked_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mnuTree.Hide();
            DeleteCheckedNode();
            RefreshControls(MainViewState.Check);
        }

        private void linkPublish_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void linkCancellation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        #endregion

    }
}
