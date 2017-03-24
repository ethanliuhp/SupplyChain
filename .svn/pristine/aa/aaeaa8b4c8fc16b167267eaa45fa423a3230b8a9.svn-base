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
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using System.Windows.Documents;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;



namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VGWBSTreeBak : TBasicDataView
    {
        private TreeNode currNode = null;

        private GWBSTree oprNode = null;

        //有权限的业务组织
        private IList lstInstance;
        //唯一编码
        private string uniqueCode;

        private Hashtable hashtableRules = new Hashtable();

        private Dictionary<string, TreeNode> listCheckedNode = new Dictionary<string, TreeNode>();

        /// <summary>
        /// 复制的顶级节点集合
        /// </summary>
        private List<TreeNode> listCopyNode = new List<TreeNode>();
        /// <summary>
        /// 复制的所有子节点集合，用于清除选择的节点时还能找到复制的节点
        /// </summary>
        Dictionary<string, TreeNode> listCopyNodeAll = new Dictionary<string, TreeNode>();

        CurrentProjectInfo projectInfo = null;

        /// <summary>
        /// 是否是插入的节点
        /// </summary>
        private bool IsInsertNode = false;

        /// <summary>
        /// 是否是提交(还有保存)
        /// </summary>
        private bool IsSubmit = false;

        public MGWBSTree model;

        public VGWBSTreeBak(MGWBSTree mot)
        {
            model = mot;
            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();

            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();

            LoadGWBSTreeTree();

            if (tvwCategory.Nodes.Count == 0)
                RefreshControls(MainViewState.Initialize);
            else
                RefreshControls(MainViewState.Browser);

            DateTime serverTime = model.GetServerTime();
            dtStartTime.Value = serverTime.Date;
            dtEndTime.Value = serverTime.Date.AddDays(7);
        }

        private void InitEvents()
        {
            tvwCategory.MouseDown += new MouseEventHandler(tvwCategory_MouseDown);//处理没有根节点的情况
            tvwCategory.NodeMouseClick += new TreeNodeMouseClickEventHandler(tvwCategory_NodeMouseClick);
            tvwCategory.AfterCheck += new TreeViewEventHandler(tvwCategory_AfterCheck);
            tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);

            tvwCategory.ItemDrag += new ItemDragEventHandler(tvwCategory_ItemDrag);
            tvwCategory.DragEnter += new DragEventHandler(tvwCategory_DragEnter);
            tvwCategory.DragDrop += new DragEventHandler(tvwCategory_DragDrop);
            tvwCategory.DragOver += new DragEventHandler(tvwCategory_DragOver);

            mnuTree.ItemClicked += new ToolStripItemClickedEventHandler(mnuTree_ItemClicked);

            btnSelectContractGroup.Click += new EventHandler(btnSelectContractGroup_Click);
            btnChangeTaskContract.Click += new EventHandler(btnChangeTaskContract_Click);


            btnSelectRelaPBS.Click += new EventHandler(btnSelectRelaPBS_Click);
            btnSelectWBSType.Click += new EventHandler(btnSelectWBSType_Click);

            btnSave.Click += new EventHandler(btnSave_Click);
            btnPublish.Click += new EventHandler(btnPublish_Click);
            btnExportMPP.Click += new EventHandler(btnExportMPP_Click);
            btnExportExcel.Click += new EventHandler(btnExportExcel_Click);

            //cbTaskRelaPBS.SelectedIndexChanged += new EventHandler(cbTaskRelaPBS_SelectedIndexChanged);
            //cbTaskRelaPBS.TextUpdate += new EventHandler(cbTaskRelaPBS_TextUpdate);
            btnRemovePBS.Click += new EventHandler(btnRemovePBS_Click);
        }

        //选择GWBS驱动契约组
        void btnSelectContractGroup_Click(object sender, EventArgs e)
        {
            VSelectWBSContractGroup frm = new VSelectWBSContractGroup(new MWBSContractGroup(), false);
            if (txtContractGroupCode.Tag != null)
            {
                frm.DefaultSelectedContract = txtContractGroupCode.Tag as ContractGroup;
            }
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                ContractGroup cg = frm.SelectResult[0] as ContractGroup;
                txtContractGroupCode.Text = cg.Code;
                txtContractGroupType.Text = cg.ContractGroupType;
                txtContractGroupDesc.Text = cg.ContractDesc;
                txtContractGroupCode.Tag = cg;
            }
        }
        //更改节点契约组
        void btnChangeTaskContract_Click(object sender, EventArgs e)
        {
            VSelectWBSContractGroup frm = new VSelectWBSContractGroup(new MWBSContractGroup(), false);
            if (txtRelaContractGroupCode.Tag != null)
            {
                frm.DefaultSelectedContract = txtRelaContractGroupCode.Tag as ContractGroup;
            }
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                ContractGroup cg = frm.SelectResult[0] as ContractGroup;
                txtRelaContractGroupCode.Text = cg.Code;
                txtRelaContractGroupCode.Tag = cg;
            }
        }

        private PBSTree selectRelaPBS = null;
        void cbTaskRelaPBS_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectRelaPBS = cbTaskRelaPBS.SelectedItem as PBSTree;
        }

        void cbTaskRelaPBS_TextUpdate(object sender, EventArgs e)
        {
            if (cbTaskRelaPBS.Text.Trim() == "")
            {
                PBSTree pbs = cbTaskRelaPBS.SelectedItem as PBSTree;

                if (pbs == null)//解决选择后取不到选择项的情况
                    pbs = selectRelaPBS;

                if (pbs != null)
                {
                    List<PBSTree> listPBS = cbTaskRelaPBS.Tag as List<PBSTree>;
                    for (int i = 0; i < listPBS.Count; i++)
                    {
                        PBSTree p = listPBS[i];
                        if (p.Id == pbs.Id)
                        {
                            listPBS.RemoveAt(i);
                            break;
                        }
                    }
                    cbTaskRelaPBS.Items.Remove(pbs);

                    if (cbTaskRelaPBS.Items.Count > 0)
                        cbTaskRelaPBS.SelectedIndex = 0;
                }
            }
        }

        void btnRemovePBS_Click(object sender, EventArgs e)
        {
            if (cbRelaPBS.SelectedItem == null)
            {
                MessageBox.Show("请选择要移除的PBS！");
                return;
            }

            PBSTree pbs = cbRelaPBS.SelectedItem as PBSTree;

            if (pbs != null)
            {
                List<PBSTree> listPBS = cbRelaPBS.Tag as List<PBSTree>;
                for (int i = 0; i < listPBS.Count; i++)
                {
                    PBSTree p = listPBS[i];
                    if (p.Id == pbs.Id)
                    {
                        listPBS.RemoveAt(i);
                        break;
                    }
                }
                cbRelaPBS.Items.Remove(pbs);

                if (cbRelaPBS.Items.Count > 0)
                    cbRelaPBS.SelectedIndex = 0;
            }
        }

        void tvwCategory_MouseDown(object sender, MouseEventArgs e)
        {
            if (tvwCategory.Nodes.Count == 0 && e.Button == MouseButtons.Right)
            {
                RefreshControls(MainViewState.Initialize);
                mnuTree.Show(tvwCategory, new Point(e.X, e.Y));
            }
        }

        void tvwCategory_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right && ConstMethod.Contains(lstInstance, e.Node.Tag as CategoryNode))
            {
                tvwCategory.SelectedNode = e.Node;
                oprNode = e.Node.Tag as GWBSTree;

                if (oprNode.ResponsibleAccFlag)
                {
                    mnuTree.Items["设置核算节点"].Text = "设为非核算节点";
                }
                else
                {
                    mnuTree.Items["设置核算节点"].Text = "设为核算节点";
                }

                if (oprNode.TaskState != DocumentState.Invalid)
                {
                    mnuTree.Items["增加子节点"].Enabled = true;
                    mnuTree.Items["修改节点"].Enabled = true;
                }
                else
                {
                    mnuTree.Items["增加子节点"].Enabled = false;
                    mnuTree.Items["修改节点"].Enabled = false;
                }
                mnuTree.Items["从PBS上拷贝节点"].Enabled = mnuTree.Items["修改节点"].Enabled;


                if (e.Node.Parent == null)
                {
                    mnuTree.Items["删除节点"].Enabled = false;
                    this.mnuTree.Items["插入同级节点"].Enabled = false;
                }
                else if (oprNode.TaskState != DocumentState.Edit)
                {
                    mnuTree.Items["删除节点"].Enabled = false;
                    this.mnuTree.Items["插入同级节点"].Enabled = true;
                }
                else
                {
                    mnuTree.Items["删除节点"].Enabled = true;
                    this.mnuTree.Items["插入同级节点"].Enabled = true;
                }

                if (listCheckedNode.Count == 0)
                {
                    mnuTree.Items["复制勾选节点"].Enabled = false;
                    mnuTree.Items["删除勾选节点"].Enabled = false;
                    mnuTree.Items["清除勾选节点"].Enabled = false;
                }
                else
                {
                    mnuTree.Items["复制勾选节点"].Enabled = true;
                    mnuTree.Items["删除勾选节点"].Enabled = true;
                    mnuTree.Items["清除勾选节点"].Enabled = true;
                }

                if (listCopyNode.Count == 0)
                    mnuTree.Items["粘贴节点"].Enabled = false;
                else
                    mnuTree.Items["粘贴节点"].Enabled = true;


                mnuTree.Show(tvwCategory, new Point(e.X, e.Y));
            }
        }

        bool isSelectNodeInvoke = false;//是否在选择(点击)节点时调用
        bool startNodeCheckedState = false;//按shift多选兄弟节点时起始节点的选中状态
        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                #region 点击树节点时实现多选
                bool isMultiSelect = false;
                TreeNode preselectionNode;//预选择节点

                preselectionNode = e.Node;

                if (currNode != null && currNode.Name != preselectionNode.Name
                    && currNode.Parent != null && preselectionNode.Parent != null && currNode.Parent.Name == preselectionNode.Parent.Name)//Name取的对象的ID
                    isMultiSelect = true;
                else
                    isMultiSelect = false;

                if (currNode != null)
                    startNodeCheckedState = currNode.Checked;

                if (isMultiSelect && (Control.ModifierKeys & Keys.Shift) != 0 && (Control.ModifierKeys & Keys.Control) != 0)//如果同时按下了ctrl+shift
                {
                    int currNodeIndex = currNode.Index;
                    int preselectNodeIndex = preselectionNode.Index;

                    int startIndex = currNodeIndex < preselectNodeIndex ? currNodeIndex : preselectNodeIndex;
                    int endIndex = currNodeIndex < preselectNodeIndex ? preselectNodeIndex : currNodeIndex;

                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        TreeNode tn = currNode.Parent.Nodes[i];

                        isSelectNodeInvoke = true;//设置标志，在check事件中不再处理

                        if (startNodeCheckedState)//如果起始节点当前为选中，就取消选择
                        {
                            TreeNode tempNode = new TreeNode();
                            tn.BackColor = tempNode.BackColor;
                            tn.ForeColor = tempNode.ForeColor;

                            if (listCheckedNode.ContainsKey(tn.Name))
                                listCheckedNode.Remove(tn.Name);

                            tn.Checked = false;
                        }
                        else//如果起始节点当前为未选中，就设置选择
                        {
                            tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                            tn.ForeColor = ColorTranslator.FromHtml("#000000");

                            if (listCheckedNode.ContainsKey(tn.Name))
                                listCheckedNode[tn.Name] = tn;
                            else
                                listCheckedNode.Add(tn.Name, tn);

                            tn.Checked = true;
                        }

                        SetChildCheckedByMultiSel(tn);
                    }
                }
                else if (isMultiSelect && (Control.ModifierKeys & Keys.Shift) != 0)//如果同时按下了shift
                {


                    int currNodeIndex = currNode.Index;
                    int preselectNodeIndex = preselectionNode.Index;

                    int startIndex = currNodeIndex < preselectNodeIndex ? currNodeIndex : preselectNodeIndex;
                    int endIndex = currNodeIndex < preselectNodeIndex ? preselectNodeIndex : currNodeIndex;

                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        TreeNode tn = currNode.Parent.Nodes[i];

                        isSelectNodeInvoke = true;//设置标志，在check事件中不再处理

                        if (startNodeCheckedState)//如果起始节点当前为选中，就取消选择
                        {
                            tn.Checked = false;

                            TreeNode tempNode = new TreeNode();
                            tn.BackColor = tempNode.BackColor;
                            tn.ForeColor = tempNode.ForeColor;

                            if (listCheckedNode.ContainsKey(tn.Name))
                                listCheckedNode.Remove(tn.Name);
                        }
                        else//如果起始节点当前为未选中，就设置选择
                        {
                            tn.Checked = true;

                            tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                            tn.ForeColor = ColorTranslator.FromHtml("#000000");

                            if (listCheckedNode.ContainsKey(tn.Name))
                                listCheckedNode[tn.Name] = tn;
                            else
                                listCheckedNode.Add(tn.Name, tn);
                        }
                    }
                }
                #endregion

                currNode = tvwCategory.SelectedNode;

                oprNode = tvwCategory.SelectedNode.Tag as GWBSTree;
                oprNode = LoadRelaAttribute(oprNode);

                //tvwCategory.SelectedNode.Tag = oprNode;

                this.GetNodeDetail();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
            }
        }

        private string GetFullPath(PBSTree pbs)
        {
            string path = string.Empty;

            path = pbs.Name;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", pbs.Id));
            oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof(PBSTree), oq);

            pbs = list[0] as PBSTree;

            CategoryNode parent = pbs.ParentNode;
            while (parent != null)
            {
                path = parent.Name + "\\" + path;

                oq.Criterions.Clear();
                oq.Criterions.Clear();
                oq.AddCriterion(Expression.Eq("Id", parent.Id));
                oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
                list = model.ObjectQuery(typeof(PBSTree), oq);

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
            IList list = model.ObjectQuery(typeof(ProjectTaskTypeTree), oq);

            taskType = list[0] as ProjectTaskTypeTree;

            CategoryNode parent = taskType.ParentNode;
            while (parent != null)
            {
                path = parent.Name + "\\" + path;

                oq.Criterions.Clear();
                oq.Criterions.Clear();
                oq.AddCriterion(Expression.Eq("Id", parent.Id));
                oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
                list = model.ObjectQuery(typeof(ProjectTaskTypeTree), oq);

                parent = (list[0] as CategoryNode).ParentNode;
            }

            return path;
        }

        private GWBSTree LoadRelaAttribute(GWBSTree wbs)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", wbs.Id));
            oq.AddFetchMode("ListRelaPBS", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ProjectTaskTypeGUID", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("BearOrgGUID", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ContractGroupGUID", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("PriceAmountUnitGUID", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("WorkAmountUnitGUID", NHibernate.FetchMode.Eager);

            IList list = model.ObjectQuery(typeof(GWBSTree), oq);
            if (list.Count > 0)
            {
                wbs = list[0] as GWBSTree;
            }
            return wbs;
        }

        void tvwCategory_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (isSelectNodeInvoke)
            {
                isSelectNodeInvoke = false;
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
                #endregion
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

        private void SetChildCheckedByMultiSel(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                SetChildCheckedByMultiSel(tn);

                isSelectNodeInvoke = true;

                if (startNodeCheckedState)//如果起始节点当前为选中，就取消选择
                {
                    TreeNode tempNode = new TreeNode();
                    tn.BackColor = tempNode.BackColor;
                    tn.ForeColor = tempNode.ForeColor;

                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode.Remove(tn.Name);

                    tn.Checked = false;
                }
                else//如果起始节点当前为未选中，就设置选择
                {
                    tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    tn.ForeColor = ColorTranslator.FromHtml("#000000");

                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode[tn.Name] = tn;
                    else
                        listCheckedNode.Add(tn.Name, tn);

                    tn.Checked = true;
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

                //基本信息
                this.txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath;
                this.txtTaskState.Text = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetWBSTaskStateText(oprNode.TaskState);
                this.txtTaskCode.Text = oprNode.Code;
                this.txtTaskName.Text = oprNode.Name;
                this.txtIsAccountNode.Text = oprNode.ResponsibleAccFlag ? "是" : "否";

                //try
                //{
                //    this.cbOrg.Text = oprNode.BearOrgName;
                //}
                //catch { }

                //this.cbOrg.Tag = oprNode.BearOrgGUID;

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

                    List<PBSTree> listPBS = new List<PBSTree>();
                    for (int i = 0; i < listRela.Count; i++)
                    {
                        PBSTree pbs = (listRela[i] as GWBSRelaPBS).ThePBS;
                        pbs.FullPath = GetFullPath(pbs);

                        cbRelaPBS.Items.Add(pbs);
                        listPBS.Add(pbs);
                    }

                    cbRelaPBS.DisplayMember = "FullPath";
                    cbRelaPBS.ValueMember = "Id";
                    cbRelaPBS.Tag = listPBS;

                    //cbRelaPBS.SelectedIndex = 0;
                }

                if (oprNode.ProjectTaskTypeGUID != null)
                {

                    this.txtTaskWBSType.Text = GetFullPath(oprNode.ProjectTaskTypeGUID);
                    this.txtTaskWBSType.Tag = oprNode.ProjectTaskTypeGUID;
                }

                //if (oprNode.ContractGroupGUID != null)
                //{
                //    this.txtRelaContractGroupCode.Text = oprNode.ContractGroupGUID.Code;
                //    this.txtRelaContractGroupCode.Tag = oprNode.ContractGroupGUID;
                //}

                this.txtTaskDesc.Text = oprNode.Describe;

                //基本信息1
                this.txtOwner.Text = oprNode.OwnerName;
                if (oprNode.TaskStateTime != null)
                    this.txtStateTime.Text = oprNode.TaskStateTime.Value.ToString();

                if (oprNode.TaskPlanStartTime != null)
                    this.dtStartTime.Value = oprNode.TaskPlanStartTime.Value;

                if (oprNode.TaskPlanEndTime != null)
                    this.dtEndTime.Value = oprNode.TaskPlanEndTime.Value;

                //txtProjectUnit.Text = oprNode.WorkAmountUnitName;
                //txtPriceUnit.Text = oprNode.PriceAmountUnitName;

                //成本信息
                //txtContractProjectAmount.Text = oprNode.ContractWorkAmount.ToString();
                //txtContractPrice.Text = oprNode.ContractPrice.ToString();
                txtContractTotalPrice.Text = oprNode.ContractTotalPrice.ToString();

                txtResponsibilityProjectAmount.Text = "";
                txtResponsibilityPrice.Text = "";
                txtResponsibilityTotalPrice.Text = oprNode.ResponsibilityTotalPrice.ToString();

                txtPlanProjectAmount.Text = "";
                txtPlanPrice.Text = "";
                txtPlanTotalPrice.Text = oprNode.PlanTotalPrice.ToString();

                RefreshControls(MainViewState.Browser);
            }
            catch (Exception exp)
            {
                MessageBox.Show("显示出错：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        private void ClearAll()
        {
            //基本信息
            this.txtCurrentPath.Text = "";
            this.txtTaskState.Text = "";
            this.txtTaskCode.Text = "";
            this.txtTaskName.Text = "";
            this.txtIsAccountNode.Text = "";

            try
            {
                cbOrg.Text = "";
            }
            catch { }
            this.cbOrg.Tag = null;
            if (cbOrg.Result != null)
                cbOrg.Result.Clear();

            this.cbRelaPBS.Items.Clear();
            this.cbRelaPBS.Tag = null;
            //this.cbRelaPBS.Text = "";

            this.txtTaskWBSType.Text = "";
            this.txtTaskWBSType.Tag = null;

            this.txtRelaContractGroupCode.Text = "";
            this.txtRelaContractGroupCode.Tag = null;

            this.txtTaskDesc.Text = "";

            //基本信息1
            this.txtOwner.Text = "";
            this.txtStateTime.Text = "";

            DateTime serverTime = model.GetServerTime();
            this.dtStartTime.Value = serverTime.Date;
            this.dtEndTime.Value = serverTime.Date.AddDays(7);

            txtProjectUnit.Text = "";
            txtPriceUnit.Text = "";

            //成本信息
            txtContractProjectAmount.Text = "";
            txtContractPrice.Text = "";
            txtContractTotalPrice.Text = "";

            txtResponsibilityProjectAmount.Text = "";
            txtResponsibilityPrice.Text = "";
            txtResponsibilityTotalPrice.Text = "";

            txtPlanProjectAmount.Text = "";
            txtPlanPrice.Text = "";
            txtPlanTotalPrice.Text = "";
        }

        private void DeleteNode()
        {
            try
            {
                if (!ValideDelete())
                    return;
                bool reset = false;
                //父节点只有这一个子节点，并且父节点有权限操作，删除后要重新设置父节点tag
                if (tvwCategory.SelectedNode.Parent.Nodes.Count == 1 && ConstMethod.Contains(lstInstance, tvwCategory.SelectedNode.Parent.Tag as CategoryNode))
                {
                    reset = true;
                }
                model.DeleteGWBSTree(oprNode);

                if (reset)
                {
                    GWBSTree org = model.GetGWBSTreeById((tvwCategory.SelectedNode.Parent.Tag as GWBSTree).Id);
                    if (org != null)
                        tvwCategory.SelectedNode.Parent.Tag = org;
                }

                //如果复制的节点有勾选的加入到选中集合
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
                MessageBox.Show("删除节点出错：" + exp.Message);
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

        void mnuTree_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text.Trim() == "增加子节点")
            {
                mnuTree.Hide();
                if (txtContractGroupCode.Text.Trim() == "" || txtContractGroupCode.Tag == null)
                {
                    MessageBox.Show("请选择驱动契约组！");
                    btnSelectContractGroup.Focus();
                    return;
                }

                RefreshControls(MainViewState.AddNew);
                add_Click(null, new EventArgs());
            }
            if (e.ClickedItem.Text.Trim() == "插入同级节点")
            {
                mnuTree.Hide();
                if (txtContractGroupCode.Text.Trim() == "" || txtContractGroupCode.Tag == null)
                {
                    MessageBox.Show("请选择驱动契约组！");
                    btnSelectContractGroup.Focus();
                    return;
                }

                RefreshControls(MainViewState.Modify);
                InsertBrotherNode();
            }
            else if (e.ClickedItem.Text.Trim() == "修改节点")
            {
                mnuTree.Hide();
                if (txtContractGroupCode.Text.Trim() == "" || txtContractGroupCode.Tag == null)
                {
                    MessageBox.Show("请选择驱动契约组！");
                    btnSelectContractGroup.Focus();
                    return;
                }

                RefreshControls(MainViewState.Modify);

                ContractGroup cg = txtContractGroupCode.Tag as ContractGroup;
                txtRelaContractGroupCode.Text = cg.Code;
                txtRelaContractGroupCode.Tag = cg;
            }
            else if (e.ClickedItem.Text.Trim() == "设为核算节点")
            {
                try
                {
                    mnuTree.Hide();
                    oprNode = model.GetGWBSTreeById(oprNode.Id);
                    oprNode.ResponsibleAccFlag = true;
                    oprNode = model.SaveGWBSTree(oprNode);
                    currNode.Tag = oprNode;
                    txtIsAccountNode.Text = "是";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
                }
            }
            else if (e.ClickedItem.Text.Trim() == "设为非核算节点")
            {
                try
                {
                    mnuTree.Hide();
                    oprNode = model.GetGWBSTreeById(oprNode.Id);
                    oprNode.ResponsibleAccFlag = false;
                    oprNode = model.SaveGWBSTree(oprNode);
                    currNode.Tag = oprNode;
                    txtIsAccountNode.Text = "否";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
                }
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
                IsSubmit = false;
                mnuTree.Hide();
                if (!SaveView()) return;
                this.RefreshControls(MainViewState.Browser);
            }
            else if (e.ClickedItem.Text.Trim() == "提交任务")
            {
                IsSubmit = true;
                mnuTree.Hide();
                if (!SaveView()) return;
                this.RefreshControls(MainViewState.Browser);
            }
            else if (e.ClickedItem.Text.Trim() == "发布节点")
            {
                mnuTree.Hide();
                PublisthNode();
                this.RefreshControls(MainViewState.Browser);
            }
            else if (e.ClickedItem.Text.Trim() == "作废节点")
            {
                mnuTree.Hide();
                CancellationNode();
                this.RefreshControls(MainViewState.Browser);
            }
            else if (e.ClickedItem.Text.Trim() == "复制勾选节点")
            {
                mnuTree.Hide();

                listCopyNode.Clear();
                listCopyNodeAll.Clear();

                GetCheckedNode(tvwCategory.Nodes[0]);

                //检查选择的每个根节点下的子节点之间是否连续
                foreach (TreeNode tn in listCopyNode)
                {
                    if (SelectNodeIsSuccession(tn) == false)
                    {
                        MessageBox.Show("节点“" + tn.FullPath + "”下选择了不连续的子节点，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tn.ExpandAll();

                        listCopyNode.Clear();
                        listCopyNodeAll.Clear();

                        return;
                    }
                }

                //判断选择的每个根节点是否是同一个父节点
                for (int i = 0; i < listCopyNode.Count - 1; i++)
                {
                    TreeNode nodePrev = listCopyNode[i];
                    TreeNode nodeNext = listCopyNode[i + 1];

                    if (nodePrev.Parent.Name != nodeNext.Parent.Name)
                    {
                        MessageBox.Show("选择的多个顶级节点不归属同一父节点，这不符合拷贝规则，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        listCopyNode.Clear();
                        listCopyNodeAll.Clear();

                        return;
                    }
                }

                RefreshControls(MainViewState.Check);
            }
            else if (e.ClickedItem.Text.Trim() == "粘贴节点")
            {
                if (txtContractGroupCode.Text.Trim() == "" || txtContractGroupCode.Tag == null)
                {
                    MessageBox.Show("请选择驱动契约组！");
                    btnSelectContractGroup.Focus();
                    return;
                }

                mnuTree.Hide();

                oprNode = LoadRelaAttribute(oprNode);
                currNode.Tag = oprNode;

                ObjectQuery oq = new ObjectQuery();
                oq.AddFetchMode("ThePBS", NHibernate.FetchMode.Eager);

                Disjunction dis = new Disjunction();
                foreach (GWBSRelaPBS rela in oprNode.ListRelaPBS)
                {
                    dis.Add(Expression.Eq("Id", rela.Id));
                }
                oq.AddCriterion(dis);

                IList listRela = model.ObjectQuery(typeof(GWBSRelaPBS), oq);

                oq.Criterions.Clear();
                oq.FetchModes.Clear();
                dis = new Disjunction();
                foreach (GWBSRelaPBS rela in listRela)
                {
                    dis.Add(Expression.Eq("PBSType", rela.ThePBS.StructTypeName.Trim()));
                }
                oq.AddCriterion(dis);
                oq.AddCriterion(NHibernate.Criterion.Expression.Eq("TaskType", oprNode.ProjectTaskTypeGUID.TypeLevel.ToString().Trim()));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                IList list = model.ObjectQuery(typeof(PBSRelaTaskTypeRuleMaster), oq);
                if (list == null || list.Count == 0)
                {
                    MessageBox.Show("当前任务节点关联的PBS和任务类型不符合GWBS节点规则，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //获取父节点下允许添加子节点的pbs和任务类型的组合规则
                List<PBSRelaTaskTypeRuleDetail> ListRuleDtl = new List<PBSRelaTaskTypeRuleDetail>();
                foreach (PBSRelaTaskTypeRuleMaster rule in list)
                {
                    ListRuleDtl.AddRange(rule.Details.ToList());
                }

                //规则校验
                GWBSTree copyWBS = listCopyNode[0].Tag as GWBSTree;
                copyWBS = LoadRelaAttribute(copyWBS);

                foreach (GWBSRelaPBS rela in copyWBS.ListRelaPBS)
                {
                    var query = from r in ListRuleDtl
                                where r.PBSType == rela.ThePBS.StructTypeName &&
                                r.TaskType == copyWBS.ProjectTaskTypeGUID.TypeLevel.ToString()
                                select r;

                    if (query == null || query.Count() == 0)
                    {
                        MessageBox.Show("当前节点下添加PBS节点“" + rela.ThePBS.Name + "[" + rela.ThePBS.StructTypeName + "]”和任务类型节点“" +
                            copyWBS.ProjectTaskTypeGUID.Name + "[" + copyWBS.ProjectTaskTypeGUID.TypeLevel + "]”的组合不符合添加规则，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        return;
                    }
                }

                SaveCopyNode();

                RefreshControls(MainViewState.Check);
            }
            else if (e.ClickedItem.Text.Trim() == "删除勾选节点")
            {
                mnuTree.Hide();

                tvwCategory.SelectedNode = tvwCategory.Nodes[0];

                DeleteCheckedNode();

                RefreshControls(MainViewState.Check);
            }
            else if (e.ClickedItem.Text.Trim() == "从PBS上拷贝节点")
            {
                if (txtContractGroupCode.Text.Trim() == "" || txtContractGroupCode.Tag == null)
                {
                    MessageBox.Show("请选择驱动契约组！");
                    btnSelectContractGroup.Focus();
                    return;
                }

                mnuTree.Hide();

                if (oprNode != null)
                {
                    oprNode = LoadRelaAttribute(oprNode);
                    currNode.Tag = oprNode;

                    if (oprNode.ListRelaPBS.Count == 0 || oprNode.ProjectTaskTypeGUID == null)
                    {
                        MessageBox.Show("请先确定当前任务关联的PBS和任务类型！");
                        return;
                    }
                }


                VSelectPBSAndTaskType frm = new VSelectPBSAndTaskType();

                if (oprNode != null)//设置父对象
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
                        frm.ParentPBSType.Add(rela.ThePBS.StructTypeName);
                    }
                    frm.ParentTaskType = oprNode.ProjectTaskTypeGUID.TypeLevel.ToString();
                }
                frm.ParentNode = currNode;
                frm.ShowDialog();

                if (frm.IsOK)
                {
                    SaveSelectNodes(frm.SelectedPBS, frm.SelectedTaskType);

                    //SaveSelectNodes(frm.SelectResult, frm.SelectMethod);

                    RefreshControls(MainViewState.Browser);
                }
            }
            else if (e.ClickedItem.Text.Trim() == "清除勾选节点")
            {
                mnuTree.Hide();

                ClearSelectedNode(tvwCategory.Nodes[0]);

                listCheckedNode.Clear();

                RefreshControls(MainViewState.Check);
            }
        }

        private void ClearSelectedNode(TreeNode parentNode)
        {
            TreeNode tempNode = new TreeNode();
            foreach (TreeNode tn in parentNode.Nodes)
            {
                tn.Checked = false;
                tn.BackColor = tempNode.BackColor;
                tn.ForeColor = tempNode.ForeColor;

                ClearSelectedNode(tn);
            }
        }

        /// <summary>
        /// 发布节点
        /// </summary>
        private void PublisthNode()
        {
            if (oprNode != null)
            {
                try
                {
                    oprNode = model.GetObjectById(typeof(GWBSTree), oprNode.Id) as GWBSTree;

                    oprNode.TaskState = DocumentState.InExecute;
                    oprNode.TaskStateTime = model.GetServerTime();

                    oprNode = model.SaveGWBSTree(oprNode);

                    txtTaskState.Text =Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetWBSTaskStateText( oprNode.TaskState);
                    txtStateTime.Text = oprNode.TaskStateTime.ToString();

                    tvwCategory.SelectedNode.Tag = oprNode;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
                }
            }
        }

        /// <summary>
        /// 作废节点
        /// </summary>
        private void CancellationNode()
        {
            if (oprNode != null)
            {
                oprNode = model.GetObjectById(typeof(GWBSTree), oprNode.Id) as GWBSTree;

                oprNode.TaskState = DocumentState.Invalid;
                oprNode.TaskStateTime = model.GetServerTime();

                oprNode = model.SaveGWBSTree(oprNode);

                txtTaskState.Text = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetWBSTaskStateText(oprNode.TaskState);
                txtStateTime.Text = oprNode.TaskStateTime.ToString();

                tvwCategory.SelectedNode.Tag = oprNode;
            }
        }

        private void SaveSelectNodes(List<PBSTree> listPBS, List<ProjectTaskTypeTree> listTask)
        {
            if (listPBS.Count > 0 && listTask.Count > 0)
            {
                IList lst = new ArrayList();
                foreach (ProjectTaskTypeTree task in listTask)
                {
                    GWBSTree wbs = new GWBSTree();
                    wbs.ProjectTaskTypeGUID = task;

                    foreach (PBSTree pbs in listPBS)
                    {
                        GWBSRelaPBS relaPBS = new GWBSRelaPBS();
                        relaPBS.ThePBS = pbs;

                        relaPBS.TheGWBSTree = wbs;
                        wbs.ListRelaPBS.Add(relaPBS);

                        if (string.IsNullOrEmpty(wbs.Name))
                        {
                            wbs.Name = pbs.Name + task.Name;
                        }
                    }

                    if (projectInfo != null)
                    {
                        wbs.TheProjectGUID = projectInfo.Id;
                        wbs.TheProjectName = projectInfo.Name;
                        wbs.Code = projectInfo.Code.PadLeft(4, '0') + "-" + model.GetCode(typeof(GWBSTree));
                    }

                    //ContractGroup contractGroup = txtContractGroupCode.Tag as ContractGroup;
                    //wbs.ContractGroupCode = contractGroup.Code;
                    //wbs.ContractGroupGUID = contractGroup;

                    wbs.TaskState = DocumentState.Edit;
                    wbs.TaskStateTime = model.GetServerTime();
                    wbs.OwnerGUID = ConstObject.LoginPersonInfo.Id;
                    wbs.OwnerName = ConstObject.LoginPersonInfo.Name;
                    wbs.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;

                    wbs.OrderNo = 0;
                    if (oprNode != null)//非根节点情况
                    {
                        wbs.ParentNode = oprNode;
                        wbs.OrderNo = model.GetMaxOrderNo(wbs);
                    }
                    else
                        wbs.Name = projectInfo.Name;

                    lst.Add(wbs);
                }

                if (oprNode == null)
                {
                    lst = model.SaveGWBSTreeRootNode(lst);

                    GWBSTree wbsNode = lst[0] as GWBSTree;

                    //给根节点tag设值
                    TreeNode rootNode = new TreeNode();
                    rootNode.Tag = wbsNode;
                    rootNode.Name = wbsNode.Id;
                    rootNode.Text = wbsNode.Name;
                    tvwCategory.Nodes.Add(rootNode);
                }
                else
                {
                    lst = model.SaveGWBSTrees(lst);

                    foreach (GWBSTree wbsNode in lst)
                    {
                        //给根节点tag设值
                        TreeNode childNode = new TreeNode();
                        childNode.Tag = wbsNode;
                        childNode.Name = wbsNode.Id;
                        childNode.Text = wbsNode.Name;
                        currNode.Nodes.Add(childNode);
                    }
                    currNode.Expand();
                }

                //新增节点要有权限操作
                (lstInstance as ArrayList).AddRange(lst);
            }
        }

        //保存从pbs上选择的节点(没用)
        //private void SaveSelectNodes(List<TreeNode> selectResult, SelectNodeMethod selectModel)
        //{
        //    if (selectResult.Count > 0)
        //    {
        //        if (selectModel == SelectNodeMethod.树状结构选择)
        //        {
        //            if (tvwCategory.Nodes.Count == 0)//无根节点情况
        //            {
        //                #region 无根节点情况
        //                TreeNode selectNode = selectResult[0];
        //                PBSTree pbsNode = selectNode.Tag as PBSTree;

        //                GWBSTree wbsRootNode = new GWBSTree();
        //                wbsRootNode.Code = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
        //                uniqueCode = wbsRootNode.Code;
        //                //wbsRootNode.OrderNo = pbsNode.OrderNo;

        //                wbsRootNode.Name = pbsNode.Name;
        //                wbsRootNode.TaskState = GWBSTreeState.编制;
        //                wbsRootNode.TaskStateTime = model.GetServerTime();
        //                wbsRootNode.OwnerGUID = ConstObject.LoginPersonInfo.Id;
        //                wbsRootNode.OwnerName = ConstObject.LoginPersonInfo.Name;
        //                wbsRootNode.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;

        //                //CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
        //                if (projectInfo != null)
        //                {
        //                    wbsRootNode.TheProjectGUID = projectInfo.Id;
        //                    wbsRootNode.TheProjectName = projectInfo.Name;
        //                }

        //                GWBSRelaPBS relaPBS = new GWBSRelaPBS();

        //                relaPBS.ThePBS = pbsNode;
        //                relaPBS.PBSName = pbsNode.Name;

        //                relaPBS.TheGWBSTree = wbsRootNode;
        //                wbsRootNode.ListRelaPBS.Add(relaPBS);


        //                IList lst = new ArrayList();
        //                lst.Add(wbsRootNode);

        //                //克隆选择的节点和其子节点的对象
        //                PopulateList(selectNode, lst, wbsRootNode);

        //                lst = model.SaveGWBSTreeRootNode(lst);
        //                wbsRootNode = lst[0] as GWBSTree;

        //                lst = model.SaveGWBSTrees(lst);

        //                //wbsRootNode = lst[0] as GWBSTree;

        //                //新增节点要有权限操作
        //                (lstInstance as ArrayList).AddRange(lst);

        //                //给根节点tag设值
        //                TreeNode rootNode = new TreeNode();
        //                tvwCategory.Nodes.Add(rootNode);
        //                rootNode.Tag = wbsRootNode;
        //                rootNode.Name = wbsRootNode.Id;
        //                rootNode.Text = wbsRootNode.Name;

        //                int i = 0;
        //                CopyNodeByPBS(rootNode, selectNode, lst, ref i);

        //                rootNode.Expand();

        //                //CopyObjToTag(selectNode, lst, ref i);

        //                #endregion
        //            }
        //            else if (oprNode != null)//有根节点情况
        //            {
        //                #region 有根节点情况
        //                oprNode = model.GetObjectById(typeof(GWBSTree), oprNode.Id) as GWBSTree;

        //                TreeNode selectNode = selectResult[0];
        //                PBSTree pbsNode = selectNode.Tag as PBSTree;

        //                // 克隆选择的顶级节点
        //                GWBSTree wbsRootNode = new GWBSTree();
        //                wbsRootNode.Code = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
        //                uniqueCode = wbsRootNode.Code;

        //                wbsRootNode.ParentNode = oprNode;

        //                wbsRootNode.OrderNo = tvwCategory.SelectedNode.Nodes.Count + 1;

        //                wbsRootNode.Name = pbsNode.Name;
        //                wbsRootNode.TaskState = GWBSTreeState.编制;
        //                wbsRootNode.TaskStateTime = model.GetServerTime();
        //                wbsRootNode.OwnerGUID = ConstObject.LoginPersonInfo.Id;
        //                wbsRootNode.OwnerName = ConstObject.LoginPersonInfo.Name;
        //                wbsRootNode.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;

        //                //CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
        //                if (projectInfo != null)
        //                {
        //                    wbsRootNode.TheProjectGUID = projectInfo.Id;
        //                    wbsRootNode.TheProjectName = projectInfo.Name;
        //                }

        //                GWBSRelaPBS relaPBS = new GWBSRelaPBS();

        //                relaPBS.ThePBS = pbsNode;
        //                relaPBS.PBSName = pbsNode.Name;

        //                relaPBS.TheGWBSTree = wbsRootNode;
        //                wbsRootNode.ListRelaPBS.Add(relaPBS);

        //                wbsRootNode.Summary = oprNode.Summary + "," + wbsRootNode.Name;



        //                IList lst = new ArrayList();
        //                lst.Add(wbsRootNode);

        //                lst = model.SaveGWBSTrees(lst);

        //                wbsRootNode = lst[0] as GWBSTree;

        //                //克隆选择的节点的子节点的对象
        //                PopulateList(selectNode, lst, wbsRootNode);

        //                lst = model.SaveGWBSTrees(lst);

        //                //新增节点要有权限操作
        //                (lstInstance as ArrayList).AddRange(lst);

        //                //给根节点tag设值
        //                TreeNode rootNode = new TreeNode();
        //                tvwCategory.SelectedNode.Nodes.Add(rootNode);
        //                rootNode.Tag = wbsRootNode;
        //                rootNode.Name = wbsRootNode.Id;
        //                rootNode.Text = wbsRootNode.Name;

        //                int i = 0;
        //                CopyNodeByPBS(rootNode, selectNode, lst, ref i);


        //                tvwCategory.SelectedNode.Expand();

        //                #endregion
        //            }
        //        }
        //        else if (selectModel == SelectNodeMethod.零散节点选择)
        //        {
        //            if (tvwCategory.Nodes.Count == 0)//无根节点情况
        //            {
        //                if (selectResult.Count > 1)
        //                {
        //                    MessageBox.Show("不能选择多个根节点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                    return;
        //                }

        //                #region 添加根节点
        //                TreeNode selectNode = selectResult[0];
        //                PBSTree pbsNode = selectNode.Tag as PBSTree;

        //                GWBSTree wbsRootNode = new GWBSTree();
        //                wbsRootNode.Code = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
        //                uniqueCode = wbsRootNode.Code;
        //                //wbsRootNode.OrderNo = pbsNode.OrderNo;

        //                wbsRootNode.Name = pbsNode.Name;
        //                wbsRootNode.TaskState = GWBSTreeState.编制;
        //                wbsRootNode.TaskStateTime = model.GetServerTime();
        //                wbsRootNode.OwnerGUID = ConstObject.LoginPersonInfo.Id;
        //                wbsRootNode.OwnerName = ConstObject.LoginPersonInfo.Name;
        //                wbsRootNode.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;

        //                //CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
        //                if (projectInfo != null)
        //                {
        //                    wbsRootNode.TheProjectGUID = projectInfo.Id;
        //                    wbsRootNode.TheProjectName = projectInfo.Name;
        //                }

        //                GWBSRelaPBS relaPBS = new GWBSRelaPBS();
        //                relaPBS.ThePBS = pbsNode;
        //                relaPBS.PBSName = pbsNode.Name;

        //                relaPBS.TheGWBSTree = wbsRootNode;
        //                wbsRootNode.ListRelaPBS.Add(relaPBS);


        //                IList lst = new ArrayList();
        //                lst.Add(wbsRootNode);
        //                lst = model.SaveGWBSTreeRootNode(lst);

        //                //wbsRootNode = lst[0] as GWBSTree;

        //                ////新增节点要有权限操作
        //                //lstInstance.Add(wbsRootNode);

        //                ////给根节点tag设值
        //                //TreeNode rootNode = new TreeNode();
        //                //tvwCategory.Nodes.Add(rootNode);
        //                //rootNode.Tag = wbsRootNode;
        //                //rootNode.Name = wbsRootNode.Id;
        //                //rootNode.Text = wbsRootNode.Name;

        //                LoadGWBSTreeTree();
        //                #endregion
        //            }
        //            else if (oprNode != null)//有根节点情况
        //            {
        //                #region 有根节点情况
        //                IList lst = new ArrayList();

        //                oprNode = model.GetObjectById(typeof(GWBSTree), oprNode.Id) as GWBSTree;
        //                foreach (TreeNode selectNode in selectResult)
        //                {
        //                    PBSTree pbsNode = selectNode.Tag as PBSTree;

        //                    // 克隆选择的顶级节点
        //                    GWBSTree wbsRootNode = new GWBSTree();
        //                    wbsRootNode.Code = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
        //                    uniqueCode = wbsRootNode.Code;

        //                    wbsRootNode.ParentNode = oprNode;

        //                    wbsRootNode.OrderNo = tvwCategory.SelectedNode.Nodes.Count + 1;

        //                    wbsRootNode.Name = pbsNode.Name;
        //                    wbsRootNode.TaskState = GWBSTreeState.编制;
        //                    wbsRootNode.TaskStateTime = model.GetServerTime();
        //                    wbsRootNode.OwnerGUID = ConstObject.LoginPersonInfo.Id;
        //                    wbsRootNode.OwnerName = ConstObject.LoginPersonInfo.Name;
        //                    wbsRootNode.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;

        //                    //CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
        //                    if (projectInfo != null)
        //                    {
        //                        wbsRootNode.TheProjectGUID = projectInfo.Id;
        //                        wbsRootNode.TheProjectName = projectInfo.Name;
        //                    }

        //                    GWBSRelaPBS relaPBS = new GWBSRelaPBS();

        //                    relaPBS.ThePBS = pbsNode;
        //                    relaPBS.PBSName = pbsNode.Name;

        //                    relaPBS.TheGWBSTree = wbsRootNode;
        //                    wbsRootNode.ListRelaPBS.Add(relaPBS);


        //                    wbsRootNode.Summary = oprNode.Summary + "," + wbsRootNode.Name;

        //                    lst.Add(wbsRootNode);

        //                }

        //                lst = model.SaveGWBSTrees(lst);

        //                //新增节点要有权限操作
        //                (lstInstance as ArrayList).AddRange(lst);

        //                tvwCategory.SelectedNode.Tag = (lst[0] as GWBSTree).ParentNode;

        //                CopyNodeByPBS(tvwCategory.SelectedNode, selectResult, lst);

        //                tvwCategory.SelectedNode.Expand();

        //                #endregion
        //            }
        //        }
        //    }
        //}

        private void CopyNodeByPBS(TreeNode parentNode, TreeNode pbsParentNode, IList lst, ref int i)
        {
            if (pbsParentNode.Nodes.Count == 0)
                return;

            foreach (TreeNode tn in pbsParentNode.Nodes)
            {
                TreeNode childNode = new TreeNode();

                i++;
                GWBSTree obj = lst[i] as GWBSTree;
                childNode.Name = obj.Id;
                childNode.Text = obj.Name;
                childNode.Tag = obj;
                parentNode.Nodes.Add(childNode);

                CopyNodeByPBS(childNode, tn, lst, ref i);
            }
        }
        private void CopyNodeByPBS(TreeNode parentNode, List<TreeNode> listPbsNodes, IList lst)
        {
            for (int i = 0; i < listPbsNodes.Count; i++)
            {
                TreeNode childNode = new TreeNode();

                GWBSTree obj = lst[i] as GWBSTree;
                childNode.Name = obj.Id;
                childNode.Text = obj.Name;
                childNode.Tag = obj;
                parentNode.Nodes.Add(childNode);
            }
        }

        void add_Click(object sender, EventArgs e)
        {
            try
            {
                IsInsertNode = false;

                ClearAll();

                oprNode = new GWBSTree();

                GWBSTree parentNode = currNode.Tag as GWBSTree;
                parentNode = LoadRelaAttribute(parentNode);
                currNode.Tag = parentNode;

                //oprNode.BearOrgGUID = parentNode.BearOrgGUID;
                //oprNode.BearOrgName = parentNode.BearOrgName;

                if (projectInfo != null)
                {
                    oprNode.TheProjectGUID = projectInfo.Id;
                    oprNode.TheProjectName = projectInfo.Name;
                }
                oprNode.ParentNode = this.tvwCategory.SelectedNode.Tag as GWBSTree;

                oprNode.TaskState = DocumentState.Edit;
                oprNode.TaskStateTime = model.GetServerTime();

                oprNode.OwnerGUID = ConstObject.LoginPersonInfo.Id;
                oprNode.OwnerName = ConstObject.LoginPersonInfo.Name;
                oprNode.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;

                ContractGroup cg = txtContractGroupCode.Tag as ContractGroup;
                txtRelaContractGroupCode.Text = cg.Code;
                txtRelaContractGroupCode.Tag = cg;


                //if (oprNode.BearOrgGUID != null)
                //{
                //    cbOrg.Text = oprNode.BearOrgName;
                //    cbOrg.Tag = oprNode.BearOrgGUID;
                //}

                //界面显示
                txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath + "\\";
                txtTaskState.Text = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetWBSTaskStateText(oprNode.TaskState);
                txtStateTime.Text = oprNode.TaskStateTime.Value.ToString();
                txtOwner.Text = oprNode.OwnerName;


                txtTaskName.Focus();
            }
            catch (Exception exp)
            {
                MessageBox.Show("增加节点出错：" + exp.Message);
            }
        }

        private void InsertBrotherNode()
        {
            try
            {
                IsInsertNode = true;

                ClearAll();

                oprNode = new GWBSTree();

                GWBSTree parentNode = currNode.Parent.Tag as GWBSTree;
                parentNode = LoadRelaAttribute(parentNode);
                currNode.Parent.Tag = parentNode;

                //oprNode.BearOrgGUID = parentNode.BearOrgGUID;
                //oprNode.BearOrgName = parentNode.BearOrgName;

                if (projectInfo != null)
                {
                    oprNode.TheProjectGUID = projectInfo.Id;
                    oprNode.TheProjectName = projectInfo.Name;
                }
                oprNode.ParentNode = (currNode.Parent.Tag as GWBSTree);

                oprNode.TaskState = DocumentState.Edit;
                oprNode.TaskStateTime = model.GetServerTime();

                oprNode.OwnerGUID = ConstObject.LoginPersonInfo.Id;
                oprNode.OwnerName = ConstObject.LoginPersonInfo.Name;
                oprNode.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;

                ContractGroup cg = txtContractGroupCode.Tag as ContractGroup;
                txtRelaContractGroupCode.Text = cg.Code;
                txtRelaContractGroupCode.Tag = cg;

                //if (oprNode.BearOrgGUID != null)
                //{
                //    cbOrg.Text = oprNode.BearOrgName;
                //    cbOrg.Tag = oprNode.BearOrgGUID;
                //}
                //界面显示
                txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath + "\\";
                txtTaskState.Text = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetWBSTaskStateText(oprNode.TaskState);
                txtStateTime.Text = oprNode.TaskStateTime.Value.ToString();
                txtOwner.Text = oprNode.OwnerName;


                txtTaskName.Focus();
            }
            catch (Exception exp)
            {
                MessageBox.Show("增加节点出错：" + exp.Message);
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
                //if (txtTaskCode.Text.Trim() == "")
                //{
                //    MessageBox.Show("任务编码不能为空!");
                //    txtTaskCode.Focus();
                //    return false;
                //}
                if (txtTaskName.Text.Trim() == "")
                {
                    tabBaseInfo.SelectedIndex = 0;
                    MessageBox.Show("任务名称不能为空!");
                    txtTaskName.Focus();
                    return false;
                }
                //if (cbOrg.Text.Trim() == "")
                //{
                //    MessageBox.Show("承担组织不能为空!");
                //    cbOrg.Focus();
                //    return false;
                //}
                if (cbRelaPBS.Items.Count == 0)
                {
                    tabBaseInfo.SelectedIndex = 0;
                    MessageBox.Show("任务关联的PBS不能为空!");
                    btnSelectWBSType.Focus();
                    return false;
                }
                if (txtTaskWBSType.Text == "")
                {
                    tabBaseInfo.SelectedIndex = 0;
                    MessageBox.Show("WBS任务类型不能为空!");
                    txtTaskWBSType.Focus();
                    return false;
                }
                if (txtRelaContractGroupCode.Text == "")
                {
                    tabBaseInfo.SelectedIndex = 0;
                    MessageBox.Show("检查要求不能为空!");
                    txtRelaContractGroupCode.Focus();
                    return false;
                }

                if (oprNode == null)
                {
                    oprNode = new GWBSTree();
                }
                else if (!string.IsNullOrEmpty(oprNode.Id))
                {
                    oprNode = LoadRelaAttribute(oprNode);
                }

                if (string.IsNullOrEmpty(oprNode.TheProjectGUID) || string.IsNullOrEmpty(oprNode.TheProjectName))
                {
                    //CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                    if (projectInfo != null)
                    {
                        oprNode.TheProjectGUID = projectInfo.Id;
                        oprNode.TheProjectName = projectInfo.Name;
                    }
                }

                oprNode.Code = txtTaskCode.Text.Trim();

                //基本信息
                oprNode.Name = txtTaskName.Text.Trim();

                if (oprNode.ParentNode != null)
                    oprNode.Summary = (oprNode.ParentNode as GWBSTree).Summary + "," + oprNode.Name;



                //承担组织
                //if (cbOrg.Text.Trim() != "")
                //{
                //    if (cbOrg.Result != null && cbOrg.Result.Count > 0)
                //    {
                //        SupplierRelationInfo org = cbOrg.Result[0] as SupplierRelationInfo;
                //        if (org != null)
                //        {
                //            oprNode.BearOrgGUID = org;
                //            oprNode.BearOrgName = cbOrg.Text;
                //        }
                //    }
                //}
                //else
                //{
                //    oprNode.BearOrgGUID = null;
                //    oprNode.BearOrgName = "";
                //}


                if (cbRelaPBS.Tag != null)
                {
                    List<PBSTree> listPBS = cbRelaPBS.Tag as List<PBSTree>;
                    if (listPBS != null)
                    {
                        for (int i = oprNode.ListRelaPBS.Count - 1; i > -1; i--)
                        {
                            GWBSRelaPBS rela = oprNode.ListRelaPBS.ElementAt(i);

                            var query = from r in listPBS
                                        where r.Id == rela.ThePBS.Id
                                        select r;

                            if (query.Count() > 0)//找到表示存在
                            {
                                listPBS.Remove(query.ElementAt(0));
                            }
                            else//没找到表示删除
                            {
                                oprNode.ListRelaPBS.Remove(rela);
                            }
                        }
                        //剩下的表示新增
                        foreach (PBSTree pbs in listPBS)
                        {
                            GWBSRelaPBS relaPBS = new GWBSRelaPBS();

                            relaPBS.ThePBS = pbs;
                            relaPBS.PBSName = pbs.Name;

                            relaPBS.TheGWBSTree = oprNode;

                            oprNode.ListRelaPBS.Add(relaPBS);
                        }
                    }
                }
                else
                {
                    tabBaseInfo.SelectedIndex = 0;
                    MessageBox.Show("请选择关联的PBS！");
                    cbRelaPBS.Focus();
                    return false;
                }

                if (txtTaskWBSType.Tag != null)
                {
                    ProjectTaskTypeTree type = txtTaskWBSType.Tag as ProjectTaskTypeTree;
                    if (type != null)
                    {
                        oprNode.ProjectTaskTypeGUID = type;
                        oprNode.ProjectTaskTypeName = type.Name;
                    }
                }
                else
                {
                    tabBaseInfo.SelectedIndex = 0;
                    MessageBox.Show("请选择WBS任务类型！");
                    txtTaskWBSType.Focus();
                    return false;
                }

                oprNode.Describe = txtTaskDesc.Text.Trim();

                //if (txtRelaContractGroupCode.Tag != null)
                //{
                //    oprNode.ContractGroupGUID = txtRelaContractGroupCode.Tag as ContractGroup;
                //    oprNode.ContractGroupCode = txtRelaContractGroupCode.Text.Trim();
                //}
                //else
                //{
                //    tabBaseInfo.SelectedIndex = 0;
                //    MessageBox.Show("请选择一个契约组！");
                //    btnChangeTaskContract.Focus();
                //    return false;
                //}

                //基本信息1
                oprNode.TaskPlanStartTime = dtStartTime.Value;
                oprNode.TaskPlanEndTime = dtEndTime.Value;

                //oprNode.WorkAmountUnitGUID = null;
                //oprNode.WorkAmountUnitName = txtProjectUnit.Text.Trim();

                oprNode.PriceAmountUnitGUID = null;
                oprNode.PriceAmountUnitName = txtPriceUnit.Text.Trim();


                //成本信息
                //try
                //{
                //    decimal ContractWorkAmount = 0;
                //    if (txtContractProjectAmount.Text.Trim() != "")
                //        ContractWorkAmount = ClientUtil.ToDecimal(txtContractProjectAmount.Text);

                //    oprNode.ContractWorkAmount = ContractWorkAmount;
                //}
                //catch
                //{
                //    tabBaseInfo.SelectedIndex = 1;
                //    MessageBox.Show("合同工程量格式填写不正确！");
                //    txtContractProjectAmount.Focus();
                //    return false;
                //}

                //try
                //{
                //    decimal ContractPrice = 0;
                //    if (txtContractPrice.Text.Trim() != "")
                //        ContractPrice = ClientUtil.ToDecimal(txtContractPrice.Text);

                //    oprNode.ContractPrice = ContractPrice;
                //}
                //catch
                //{
                //    tabBaseInfo.SelectedIndex = 1;
                //    MessageBox.Show("合同单价格式填写不正确！");
                //    txtContractPrice.Focus();
                //    return false;
                //}

                try
                {
                    decimal ContractTotalPrice = 0;
                    if (txtContractTotalPrice.Text.Trim() != "")
                        ContractTotalPrice = ClientUtil.ToDecimal(txtContractTotalPrice.Text);

                    oprNode.ContractTotalPrice = ContractTotalPrice;
                }
                catch
                {
                    tabBaseInfo.SelectedIndex = 1;
                    MessageBox.Show("合同合价格式填写不正确！");
                    txtContractTotalPrice.Focus();
                    return false;
                }

                try
                {
                    decimal ResponsibilityProjectAmount = 0;
                    if (txtResponsibilityProjectAmount.Text.Trim() != "")
                        ResponsibilityProjectAmount = ClientUtil.ToDecimal(txtResponsibilityProjectAmount.Text);

                    //赋值
                }
                catch
                {
                    tabBaseInfo.SelectedIndex = 1;
                    MessageBox.Show("责任工程量格式填写不正确！");
                    txtResponsibilityProjectAmount.Focus();
                    return false;
                }

                try
                {
                    decimal ResponsibilityPrice = 0;
                    if (txtResponsibilityProjectAmount.Text.Trim() != "")
                        ResponsibilityPrice = ClientUtil.ToDecimal(txtResponsibilityPrice.Text);

                    //赋值
                }
                catch
                {
                    tabBaseInfo.SelectedIndex = 1;
                    MessageBox.Show("责任工程量单价填写不正确！");
                    txtResponsibilityPrice.Focus();
                    return false;
                }

                try
                {
                    decimal ResponsibilityTotalPrice = 0;
                    if (txtResponsibilityTotalPrice.Text.Trim() != "")
                        ResponsibilityTotalPrice = ClientUtil.ToDecimal(txtResponsibilityTotalPrice.Text);

                    //赋值
                    oprNode.ResponsibilityTotalPrice = ResponsibilityTotalPrice;
                }
                catch
                {
                    tabBaseInfo.SelectedIndex = 1;
                    MessageBox.Show("责任合价格式填写不正确！");
                    txtResponsibilityTotalPrice.Focus();
                    return false;
                }

                try
                {
                    decimal PlanProjectAmount = 0;
                    if (txtPlanProjectAmount.Text.Trim() != "")
                        PlanProjectAmount = ClientUtil.ToDecimal(txtPlanProjectAmount.Text);

                    //赋值
                    //oprNode. = PlanProjectAmount ;
                }
                catch
                {
                    tabBaseInfo.SelectedIndex = 1;
                    MessageBox.Show("计划工程量格式填写不正确！");
                    txtPlanProjectAmount.Focus();
                    return false;
                }

                try
                {
                    decimal PlanPrice = 0;
                    if (txtPlanPrice.Text.Trim() != "")
                        PlanPrice = ClientUtil.ToDecimal(txtPlanPrice.Text);

                    //赋值
                    //oprNode. = PlanPrice;
                }
                catch
                {
                    MessageBox.Show("计划单价格式填写不正确！");
                    tabBaseInfo.SelectedIndex = 1;
                    txtPlanPrice.Focus();
                    return false;
                }

                try
                {
                    decimal PlanTotalPrice = 0;
                    if (txtPlanTotalPrice.Text.Trim() != "")
                        PlanTotalPrice = ClientUtil.ToDecimal(txtPlanTotalPrice.Text);

                    //赋值
                    oprNode.PlanTotalPrice = PlanTotalPrice;
                }
                catch
                {
                    MessageBox.Show("计划合价格式填写不正确！");
                    tabBaseInfo.SelectedIndex = 1;
                    txtPlanTotalPrice.Focus();
                    return false;
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

            LoadGWBSTreeTree();
        }

        private void LoadGWBSTreeTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();
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
                MessageBox.Show("查询业务组织出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        public override void RefreshControls(MainViewState state)
        {
            switch (state)
            {
                case MainViewState.AddNew:

                    this.mnuTree.Items["撤销"].Enabled = true;
                    this.mnuTree.Items["保存节点"].Enabled = true;
                    this.mnuTree.Items["提交任务"].Enabled = true;
                    this.mnuTree.Items["发布节点"].Enabled = false;
                    this.mnuTree.Items["作废节点"].Enabled = false;

                    this.mnuTree.Items["从PBS上拷贝节点"].Enabled = false;
                    this.mnuTree.Items["增加子节点"].Enabled = false;
                    this.mnuTree.Items["修改节点"].Enabled = false;
                    this.mnuTree.Items["删除节点"].Enabled = false;


                    btnSave.Enabled = true;
                    btnSelectRelaPBS.Enabled = true;
                    btnSelectWBSType.Enabled = true;
                    btnPublish.Enabled = false;

                    //基本信息
                    //this.txtCurrentPath.Enabled = true;
                    this.txtTaskState.ReadOnly = true;
                    this.txtTaskCode.ReadOnly = true;
                    this.txtTaskName.ReadOnly = false;

                    this.cbOrg.Enabled = true;
                    //this.cbRelaPBS.Enabled = true;
                    this.btnRemovePBS.Enabled = true;
                    this.txtTaskWBSType.ReadOnly = false;
                    this.txtTaskDesc.ReadOnly = false;

                    this.txtRelaContractGroupCode.ReadOnly = true;
                    this.btnChangeTaskContract.Enabled = true;

                    //基本信息1
                    this.txtOwner.ReadOnly = true;
                    this.txtStateTime.ReadOnly = true;

                    this.dtStartTime.Enabled = true;
                    this.dtEndTime.Enabled = true;

                    txtProjectUnit.ReadOnly = false;
                    txtPriceUnit.ReadOnly = false;

                    //成本信息
                    txtContractProjectAmount.ReadOnly = false;
                    txtContractPrice.ReadOnly = false;
                    txtContractTotalPrice.ReadOnly = false;

                    txtResponsibilityProjectAmount.ReadOnly = false;
                    txtResponsibilityPrice.ReadOnly = false;
                    txtResponsibilityTotalPrice.ReadOnly = false;

                    txtPlanProjectAmount.ReadOnly = false;
                    txtPlanPrice.ReadOnly = false;
                    txtPlanTotalPrice.ReadOnly = false;
                    break;

                case MainViewState.Modify:

                    this.mnuTree.Items["撤销"].Enabled = true;
                    this.mnuTree.Items["保存节点"].Enabled = true;
                    this.mnuTree.Items["提交任务"].Enabled = true;
                    this.mnuTree.Items["增加子节点"].Enabled = false;
                    this.mnuTree.Items["修改节点"].Enabled = false;
                    this.mnuTree.Items["删除节点"].Enabled = false;

                    this.btnSave.Enabled = true;
                    btnSelectRelaPBS.Enabled = true;
                    btnSelectWBSType.Enabled = true;

                    //编制状态不允许发布作废和拷贝子节点
                    this.mnuTree.Items["从PBS上拷贝节点"].Enabled = false;
                    this.btnPublish.Enabled = false;
                    this.mnuTree.Items["发布节点"].Enabled = false;
                    this.mnuTree.Items["作废节点"].Enabled = false;

                    //基本信息
                    //this.txtCurrentPath.Enabled = true;
                    this.txtTaskState.ReadOnly = true;
                    this.txtTaskCode.ReadOnly = true;
                    this.txtTaskName.ReadOnly = false;

                    this.cbOrg.Enabled = true;
                    //this.cbRelaPBS.Enabled = true;
                    this.btnRemovePBS.Enabled = true;
                    this.txtTaskWBSType.ReadOnly = false;
                    this.txtRelaContractGroupCode.ReadOnly = true;
                    this.btnChangeTaskContract.Enabled = true;
                    this.txtTaskDesc.ReadOnly = false;

                    //基本信息1
                    this.txtOwner.ReadOnly = true;
                    this.txtStateTime.ReadOnly = true;
                    this.dtStartTime.Enabled = true;
                    this.dtEndTime.Enabled = true;

                    txtProjectUnit.ReadOnly = false;
                    txtPriceUnit.ReadOnly = false;

                    //成本信息
                    txtContractProjectAmount.ReadOnly = false;
                    txtContractPrice.ReadOnly = false;
                    txtContractTotalPrice.ReadOnly = false;

                    txtResponsibilityProjectAmount.ReadOnly = false;
                    txtResponsibilityPrice.ReadOnly = false;
                    txtResponsibilityTotalPrice.ReadOnly = false;

                    txtPlanProjectAmount.ReadOnly = false;
                    txtPlanPrice.ReadOnly = false;
                    txtPlanTotalPrice.ReadOnly = false;
                    break;

                case MainViewState.Browser:

                    this.mnuTree.Items["撤销"].Enabled = false;
                    this.mnuTree.Items["保存节点"].Enabled = false;
                    this.mnuTree.Items["增加子节点"].Enabled = true;
                    this.mnuTree.Items["修改节点"].Enabled = true;
                    this.mnuTree.Items["删除节点"].Enabled = true;

                    this.btnSave.Enabled = false;
                    btnSelectRelaPBS.Enabled = false;
                    btnSelectWBSType.Enabled = false;

                    if (oprNode != null && oprNode.TaskState == DocumentState.Edit)
                    {
                        this.mnuTree.Items["提交任务"].Enabled = true;
                    }
                    else
                    {
                        this.mnuTree.Items["提交任务"].Enabled = false;
                    }

                    if (oprNode != null && (oprNode.TaskState == DocumentState.Edit || oprNode.TaskState == DocumentState.InAudit))
                    {
                        this.btnPublish.Enabled = true;
                        this.mnuTree.Items["发布节点"].Enabled = true;
                    }
                    else
                    {
                        this.btnPublish.Enabled = false;
                        this.mnuTree.Items["发布节点"].Enabled = false;
                    }

                    if (oprNode != null && oprNode.TaskState == DocumentState.InExecute)
                    {
                        this.mnuTree.Items["作废节点"].Enabled = true;
                    }
                    else
                    {
                        this.mnuTree.Items["作废节点"].Enabled = false;
                    }

                    //基本信息
                    this.txtCurrentPath.ReadOnly = true;
                    this.txtTaskState.ReadOnly = true;
                    this.txtTaskCode.ReadOnly = true;
                    this.txtTaskName.ReadOnly = true;

                    this.cbOrg.Enabled = false;
                    //this.cbRelaPBS.Enabled = false;
                    this.btnRemovePBS.Enabled = false;
                    this.txtTaskWBSType.ReadOnly = true;
                    this.txtRelaContractGroupCode.ReadOnly = true;
                    this.btnChangeTaskContract.Enabled = false;
                    this.txtTaskDesc.ReadOnly = true;

                    //基本信息1
                    this.txtOwner.ReadOnly = true;
                    this.txtStateTime.ReadOnly = true;
                    this.dtStartTime.Enabled = false;
                    this.dtEndTime.Enabled = false;

                    txtProjectUnit.ReadOnly = true;
                    txtPriceUnit.ReadOnly = true;

                    //成本信息
                    txtContractProjectAmount.ReadOnly = true;
                    txtContractPrice.ReadOnly = true;
                    txtContractTotalPrice.ReadOnly = true;

                    txtResponsibilityProjectAmount.ReadOnly = true;
                    txtResponsibilityPrice.ReadOnly = true;
                    txtResponsibilityTotalPrice.ReadOnly = true;

                    txtPlanProjectAmount.ReadOnly = true;
                    txtPlanPrice.ReadOnly = true;
                    txtPlanTotalPrice.ReadOnly = true;
                    break;

                case MainViewState.Initialize://添加根节点

                    this.mnuTree.Items["从PBS上拷贝节点"].Enabled = true;

                    this.mnuTree.Items["撤销"].Enabled = false;
                    this.mnuTree.Items["保存节点"].Enabled = false;
                    this.mnuTree.Items["提交任务"].Enabled = false;
                    this.mnuTree.Items["发布节点"].Enabled = false;
                    this.mnuTree.Items["作废节点"].Enabled = false;
                    this.mnuTree.Items["增加子节点"].Enabled = false;
                    this.mnuTree.Items["修改节点"].Enabled = false;
                    this.mnuTree.Items["删除节点"].Enabled = false;
                    this.mnuTree.Items["删除勾选节点"].Enabled = false;
                    this.mnuTree.Items["复制勾选节点"].Enabled = false;
                    this.mnuTree.Items["粘贴节点"].Enabled = false;


                    //基本信息
                    this.txtCurrentPath.ReadOnly = true;
                    this.txtTaskState.ReadOnly = true;
                    this.txtTaskCode.ReadOnly = true;
                    this.txtTaskName.ReadOnly = true;

                    this.cbOrg.Enabled = false;
                    //this.cbRelaPBS.Enabled = false;
                    this.btnRemovePBS.Enabled = false;
                    this.txtTaskWBSType.ReadOnly = true;
                    this.txtRelaContractGroupCode.ReadOnly = true;
                    this.btnChangeTaskContract.Enabled = false;
                    this.txtTaskDesc.ReadOnly = true;

                    //基本信息1
                    this.txtOwner.ReadOnly = true;
                    this.txtStateTime.ReadOnly = true;
                    this.dtStartTime.Enabled = false;
                    this.dtEndTime.Enabled = false;

                    txtProjectUnit.ReadOnly = true;
                    txtPriceUnit.ReadOnly = true;

                    //成本信息
                    txtContractProjectAmount.ReadOnly = true;
                    txtContractPrice.ReadOnly = true;
                    txtContractTotalPrice.ReadOnly = true;

                    txtResponsibilityProjectAmount.ReadOnly = true;
                    txtResponsibilityPrice.ReadOnly = true;
                    txtResponsibilityTotalPrice.ReadOnly = true;

                    txtPlanProjectAmount.ReadOnly = true;
                    txtPlanPrice.ReadOnly = true;
                    txtPlanTotalPrice.ReadOnly = true;
                    break;
            }

            ViewState = state;
        }

        public override bool ModifyView()
        {
            return true;
        }

        public override bool CancelView()
        {
            try
            {
                LoadGWBSTreeTree();
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
                oprNode = null;
                currNode = null;

                listCheckedNode.Clear();
                listCopyNode.Clear();

                LoadGWBSTreeTree();
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

                if (IsSubmit)
                {
                    oprNode.TaskState = DocumentState.InAudit;
                    oprNode.TaskStateTime = model.GetServerTime();
                }

                if (oprNode.Id == null)
                {
                    isNew = true;

                    if (projectInfo != null)
                    {
                        oprNode.Code = projectInfo.Code.PadLeft(4, '0') + "-" + model.GetCode(typeof(GWBSTree));
                    }

                    if (IsInsertNode)
                    {
                        IList list = new ArrayList();

                        long orderNo = (currNode.Tag as GWBSTree).OrderNo;
                        oprNode.OrderNo = orderNo;

                        list.Add(oprNode);

                        TreeNode parentNode = currNode.Parent;
                        for (int i = currNode.Index; i < parentNode.Nodes.Count; i++)
                        {
                            GWBSTree pbs = parentNode.Nodes[i].Tag as GWBSTree;
                            pbs.OrderNo += 1;
                            list.Add(pbs);
                        }

                        list = model.InsertOrUpdateWBSTrees(list);

                        oprNode = list[0] as GWBSTree;

                        //插入子节点的父节点需要重新设置Tag
                        currNode.Parent.Tag = oprNode.ParentNode;


                        //更新tag
                        for (int i = currNode.Index; i < parentNode.Nodes.Count; i++)
                        {
                            GWBSTree pbs = parentNode.Nodes[i].Tag as GWBSTree;

                            foreach (GWBSTree p in list)
                            {
                                if (p.Id == pbs.Id)
                                {
                                    parentNode.Nodes[i].Tag = p;
                                    break;
                                }
                            }
                        }

                        TreeNode tn = this.tvwCategory.SelectedNode.Parent.Nodes.Insert(currNode.Index, oprNode.Name.ToString());
                        tn.Name = oprNode.Id;
                        tn.Tag = oprNode;

                        this.tvwCategory.SelectedNode = tn;
                        tn.Expand();

                        //新增节点要有权限操作
                        lstInstance.Add(oprNode);
                    }
                    else
                    {
                        oprNode = model.SaveGWBSTree(oprNode);
                    }
                }
                else
                {
                    isNew = false;
                    oprNode = model.SaveGWBSTree(oprNode);
                }


                if (isNew)
                {
                    if (!IsInsertNode)
                    {
                        //要添加子节点的节点以前没有子节点，需要重新设置Tag
                        tvwCategory.SelectedNode.Tag = oprNode.ParentNode;

                        TreeNode tn = this.tvwCategory.SelectedNode.Nodes.Add(oprNode.Id.ToString(), oprNode.Name.ToString());
                        tn.Name = oprNode.Name;

                        tn.Tag = oprNode;
                        this.tvwCategory.SelectedNode = tn;
                        tn.Expand();

                        //新增节点要有权限操作
                        lstInstance.Add(oprNode);
                    }
                }
                else
                {
                    this.tvwCategory.SelectedNode.Tag = oprNode;
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
                GWBSTree org = (e.Item as TreeNode).Tag as GWBSTree;
                //有权限的节点才允许拖动操作
                if (org != null && ConstMethod.Contains(lstInstance, org))
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
                if (targetNode != null && !ConstMethod.Contains(lstInstance, targetNode.Tag as GWBSTree))
                    return;
                TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

                if (TreeViewUtil.CanMoveNode(draggedNode, targetNode))
                {
                    #region 规则校验
                    GWBSTree newParent = targetNode.Tag as GWBSTree;
                    newParent = LoadRelaAttribute(newParent);
                    targetNode.Tag = newParent;

                    ObjectQuery oq = new ObjectQuery();
                    oq.AddFetchMode("ThePBS", NHibernate.FetchMode.Eager);

                    Disjunction dis = new Disjunction();
                    foreach (GWBSRelaPBS rela in newParent.ListRelaPBS)
                    {
                        dis.Add(Expression.Eq("Id", rela.Id));
                    }
                    oq.AddCriterion(dis);

                    IList listRela = model.ObjectQuery(typeof(GWBSRelaPBS), oq);

                    oq.Criterions.Clear();
                    oq.FetchModes.Clear();
                    dis = new Disjunction();
                    foreach (GWBSRelaPBS rela in listRela)
                    {
                        dis.Add(Expression.Eq("PBSType", rela.ThePBS.StructTypeName.Trim()));
                    }
                    oq.AddCriterion(dis);
                    oq.AddCriterion(NHibernate.Criterion.Expression.Eq("TaskType", newParent.ProjectTaskTypeGUID.TypeLevel.ToString().Trim()));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                    IList list = model.ObjectQuery(typeof(PBSRelaTaskTypeRuleMaster), oq);
                    if (list == null || list.Count == 0)
                    {
                        MessageBox.Show("目标任务节点关联的PBS和任务类型不符合GWBS节点规则，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    //获取父节点下允许添加子节点的pbs和任务类型的组合规则
                    List<PBSRelaTaskTypeRuleDetail> ListRuleDtl = new List<PBSRelaTaskTypeRuleDetail>();
                    foreach (PBSRelaTaskTypeRuleMaster rule in list)
                    {
                        ListRuleDtl.AddRange(rule.Details.ToList());
                    }

                    //规则校验
                    GWBSTree copyWBS = draggedNode.Tag as GWBSTree;
                    copyWBS = LoadRelaAttribute(copyWBS);

                    foreach (GWBSRelaPBS rela in copyWBS.ListRelaPBS)
                    {
                        var query = from r in ListRuleDtl
                                    where r.PBSType == rela.ThePBS.StructTypeName &&
                                    r.TaskType == copyWBS.ProjectTaskTypeGUID.TypeLevel.ToString()
                                    select r;

                        if (query == null || query.Count() == 0)
                        {
                            //排序
                            if (draggedNode.Parent == targetNode.Parent)
                            {
                                if (MessageBox.Show("您是要执行排序操作吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    draggedNode.Remove();
                                    targetNode.Parent.Nodes.Insert(targetNode.Index, draggedNode);

                                    if (draggedNode.PrevNode != null)
                                    {
                                        IList result = new ArrayList();
                                        GWBSTree prevOrg = draggedNode.PrevNode.Tag as GWBSTree;
                                        SetNextNodeOrder(draggedNode, result, prevOrg.OrderNo + 1);
                                        result = model.SaveGWBSTrees(result);
                                        ResetTagAfterOrder(draggedNode, result, 0);
                                    }
                                    else
                                    {
                                        GWBSTree fromOrg = draggedNode.Tag as GWBSTree;
                                        GWBSTree toOrg = targetNode.Tag as GWBSTree;
                                        fromOrg.OrderNo = toOrg.OrderNo - 1;
                                        draggedNode.Tag = model.SaveGWBSTree(fromOrg);
                                    }
                                    //保证拖动后马上修改保存不出错
                                    this.tvwCategory.SelectedNode = draggedNode;
                                    return;
                                }
                            }

                            MessageBox.Show("在目标节点下添加PBS节点“" + rela.ThePBS.Name + "[" + rela.ThePBS.StructTypeName + "]”和任务类型节点“" +
                                copyWBS.ProjectTaskTypeGUID.Name + "[" + copyWBS.ProjectTaskTypeGUID.TypeLevel + "]”的组合不符合添加规则，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            return;
                        }
                    }
                    #endregion

                    //以前的父节点
                    TreeNode oldParentNode = draggedNode.Parent;
                    bool reset = false;
                    //父节点只有这一个子节点，并且父节点有权限操作，移动后要重新设置父节点tag
                    if (oldParentNode.Nodes.Count == 1 && ConstMethod.Contains(lstInstance, oldParentNode.Tag as CategoryNode))
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

                            GWBSTree templateWBS = draggedNode.Tag as GWBSTree;
                            templateWBS = LoadRelaAttribute(templateWBS);

                            GWBSTree catTmp = templateWBS.Clone();

                            //系统生存一个唯一编码
                            if (projectInfo != null)
                            {
                                catTmp.TheProjectGUID = projectInfo.Id;
                                catTmp.TheProjectName = projectInfo.Name;
                                catTmp.Code = projectInfo.Code.PadLeft(4, '0') + "-" + model.GetCode(typeof(GWBSTree));
                            }

                            //ContractGroup contractGroup = txtContractGroupCode.Tag as ContractGroup;
                            //catTmp.ContractGroupCode = contractGroup.Code;
                            //catTmp.ContractGroupGUID = contractGroup;

                            catTmp.ParentNode = targetNode.Tag as GWBSTree;
                            catTmp.OrderNo = model.GetMaxOrderNo(catTmp) + 1;
                            //draggedNode.Tag = catTmp;

                            IList lst = new ArrayList();
                            lst.Add(catTmp);
                            //克隆要复制的节点和其子节点的对象
                            PopulateListByCopyNode(draggedNode, lst, catTmp);
                            lst = model.SaveGWBSTrees(lst);
                            //新增节点要有权限操作
                            (lstInstance as ArrayList).AddRange(lst);
                            //给复制节点的新父节点tag设值
                            targetNode.Tag = (lst[0] as GWBSTree).ParentNode;
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
                            GWBSTree toObj = targetNode.Tag as GWBSTree;
                            IDictionary dic = model.MoveNode(draggedNode.Tag as GWBSTree, toObj);
                            if (reset)
                            {
                                GWBSTree cat = model.GetGWBSTreeById((oldParentNode.Tag as GWBSTree).Id);
                                oldParentNode.Tag = cat;
                            }
                            targetNode.Tag = dic[(targetNode.Tag as GWBSTree).Id.ToString()];
                            //根据返回的数据进行节点tag赋值
                            ResetTagAfterMove(draggedNode, dic);
                        }
                        //排序
                        else if (draggedNode.Parent == targetNode.Parent)
                        {
                            if (draggedNode.PrevNode != null)
                            {
                                IList result = new ArrayList();
                                GWBSTree prevOrg = draggedNode.PrevNode.Tag as GWBSTree;
                                SetNextNodeOrder(draggedNode, result, prevOrg.OrderNo + 1);
                                result = model.SaveGWBSTrees(result);
                                ResetTagAfterOrder(draggedNode, result, 0);
                            }
                            else
                            {
                                GWBSTree fromOrg = draggedNode.Tag as GWBSTree;
                                GWBSTree toOrg = targetNode.Tag as GWBSTree;
                                fromOrg.OrderNo = toOrg.OrderNo - 1;
                                draggedNode.Tag = model.SaveGWBSTree(fromOrg);
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
            GWBSTree org = node.Tag as GWBSTree;
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
            node.Tag = dic[(node.Tag as GWBSTree).Id.ToString()];
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
            GWBSTree obj = lst[i] as GWBSTree;
            node.Name = obj.Id;
            node.Text = obj.Name;
            node.Tag = obj;

            if (node.Nodes.Count == 0)
                return;
            foreach (TreeNode var in node.Nodes)
            {
                i++;
                CopyObjToTag(var, lst, ref i);
            }
        }

        private void PopulateList(TreeNode node, IList lst, GWBSTree parent)
        {
            if (node.Nodes.Count == 0)
                return;

            for (int i = 0; i < node.Nodes.Count; i++)
            {
                TreeNode var = node.Nodes[i];
                PBSTree pbsNode = var.Tag as PBSTree;

                GWBSTree matCatTmp = new GWBSTree();
                uniqueCode = ConstMethod.GetNextCode(uniqueCode);
                matCatTmp.Code = uniqueCode;

                matCatTmp.OrderNo = i + 1;

                matCatTmp.Name = pbsNode.Name;
                matCatTmp.TaskState = DocumentState.Edit;
                matCatTmp.TaskStateTime = parent.TaskStateTime;

                matCatTmp.OwnerGUID = ConstObject.LoginPersonInfo.Id;
                matCatTmp.OwnerName = ConstObject.LoginPersonInfo.Name;
                matCatTmp.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;

                //CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                if (projectInfo != null)
                {
                    matCatTmp.TheProjectGUID = projectInfo.Id;
                    matCatTmp.TheProjectName = projectInfo.Name;
                }


                GWBSRelaPBS relaPBS = new GWBSRelaPBS();

                relaPBS.ThePBS = pbsNode;
                relaPBS.PBSName = pbsNode.Name;

                relaPBS.TheGWBSTree = matCatTmp;
                matCatTmp.ListRelaPBS.Add(relaPBS);


                matCatTmp.ParentNode = parent;

                matCatTmp.Summary = parent.Summary + "," + matCatTmp.Name;

                lst.Add(matCatTmp);

                PopulateList(var, lst, matCatTmp);
            }
        }

        private void PopulateListByCopyNode(TreeNode node, IList lst, GWBSTree parent)
        {
            foreach (TreeNode var in node.Nodes)
            {
                GWBSTree templateWBS = var.Tag as GWBSTree;
                templateWBS = LoadRelaAttribute(templateWBS);

                GWBSTree matCatTmp = templateWBS.Clone();

                //系统生存一个唯一编码
                if (projectInfo != null)
                {
                    matCatTmp.TheProjectGUID = projectInfo.Id;
                    matCatTmp.TheProjectName = projectInfo.Name;
                    matCatTmp.Code = projectInfo.Code.PadLeft(4, '0') + "-" + model.GetCode(typeof(GWBSTree));
                }

                uniqueCode = ConstMethod.GetNextCode(uniqueCode);
                matCatTmp.Code = uniqueCode;
                matCatTmp.ParentNode = parent;
                matCatTmp.TaskState = DocumentState.Edit;
                matCatTmp.TaskStateTime = model.GetServerTime();

                matCatTmp.Summary = parent.Summary + "," + matCatTmp.Name;

                //var.Tag = matCatTmp;
                lst.Add(matCatTmp);
                PopulateListByCopyNode(var, lst, matCatTmp);
            }
        }

        private void tvwCategory_DragOver(object sender, DragEventArgs e)
        {
            //Point targetPoint = tvwCategory.PointToClient(new Point(e.X, e.Y));
            //tvwCategory.SelectedNode = tvwCategory.GetNodeAt(targetPoint);
        }

        #endregion

        //private void SaveCopyNode()
        //{
        //    if (listCopyNode.Count > 0)
        //    {
        //        IList lst = new ArrayList();
        //        foreach (TreeNode draggedNode in listCopyNode)
        //        {
        //            GWBSTree catTmp = (draggedNode.Tag as GWBSTree).Clone();

        //            //系统生存一个唯一编码
        //            catTmp.Code = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
        //            uniqueCode = catTmp.Code;
        //            catTmp.ParentNode = tvwCategory.SelectedNode.Tag as GWBSTree;
        //            catTmp.OrderNo = model.GetMaxOrderNo(catTmp) + 1;
        //            catTmp.TaskState = GWBSTreeState.编制;
        //            catTmp.TaskStateTime = model.GetServerTime();

        //            lst.Add(catTmp);
        //        }
        //        //保存复制的节点
        //        lst = model.SaveGWBSTrees(lst);
        //        //新增节点要有权限操作
        //        (lstInstance as ArrayList).AddRange(lst);
        //        //给复制节点的新父节点tag设值
        //        tvwCategory.SelectedNode.Tag = (lst[0] as GWBSTree).ParentNode;

        //        foreach (GWBSTree pbs in lst)
        //        {
        //            TreeNode tnTmp = new TreeNode();
        //            tnTmp.Name = pbs.Id.ToString();
        //            tnTmp.Text = pbs.Name;
        //            tnTmp.Tag = pbs;

        //            tvwCategory.SelectedNode.Nodes.Add(tnTmp);
        //        }

        //        listCopyNode.Clear();
        //    }
        //}

        private void GetCheckedNode(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Checked)//找到选择的每一个根节点
                {
                    listCopyNode.Add(tn);
                    continue;
                }

                GetCheckedNode(tn);
            }
        }

        /// <summary>
        /// 判断选择的节点及其子节点是否连续
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool SelectNodeIsSuccession(TreeNode parentNode)
        {
            //查询节点树
            var listLeafNode = from n in listCheckedNode
                               where (n.Value.Tag as GWBSTree).SysCode.IndexOf((parentNode.Tag as GWBSTree).SysCode) > -1
                               select n;

            foreach (var dic in listLeafNode)
            {
                if (listCopyNodeAll.Keys.Contains(dic.Key) == false)
                    listCopyNodeAll.Add(dic.Key, dic.Value);

                if (dic.Key != parentNode.Name)//此叶节点不是顶级节点
                {
                    TreeNode tempParent = dic.Value.Parent;

                    while (tempParent.Name != parentNode.Name)
                    {
                        if (tempParent.Checked == false)
                        {
                            return false;
                        }

                        if (listCopyNodeAll.Keys.Contains(tempParent.Name) == false)
                            listCopyNodeAll.Add(tempParent.Name, tempParent);

                        tempParent = tempParent.Parent;
                    }
                }
            }

            return true;
        }

        private void SaveCopyNode()
        {
            if (listCopyNode.Count > 0)
            {
                IList lst = new ArrayList();
                foreach (TreeNode node in listCopyNode)
                {
                    GWBSTree templateWBS = node.Tag as GWBSTree;
                    templateWBS = LoadRelaAttribute(templateWBS);

                    GWBSTree catTmp = templateWBS.Clone();

                    if (projectInfo != null)
                    {
                        catTmp.TheProjectGUID = projectInfo.Id;
                        catTmp.TheProjectName = projectInfo.Name;
                        catTmp.Code = projectInfo.Code.PadLeft(4, '0') + "-" + model.GetCode(typeof(GWBSTree));
                    }

                    //ContractGroup contractGroup = txtContractGroupCode.Tag as ContractGroup;
                    //catTmp.ContractGroupCode = contractGroup.Code;
                    //catTmp.ContractGroupGUID = contractGroup;

                    GWBSTree parentNode = oprNode;// tvwCategory.SelectedNode.Tag as PBSTree
                    catTmp.ParentNode = parentNode;
                    catTmp.OrderNo = model.GetMaxOrderNo(catTmp) + 1;

                    lst.Add(catTmp);

                    GetCopyNode(node, catTmp, ref lst);
                }

                //保存复制的节点
                lst = model.SaveGWBSTrees(lst);
                //新增节点要有权限操作
                (lstInstance as ArrayList).AddRange(lst);
                //给复制节点的新父节点tag设值
                oprNode = (lst[0] as GWBSTree).ParentNode as GWBSTree;
                tvwCategory.SelectedNode.Tag = oprNode;

                IEnumerable<GWBSTree> listCopyPBS = lst.OfType<GWBSTree>();

                IEnumerable<GWBSTree> listCopyRoot = from n in listCopyPBS
                                                     where n.ParentNode.Id == oprNode.Id
                                                     select n;

                foreach (GWBSTree pbs in listCopyRoot)
                {
                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = pbs.Id.ToString();
                    tnTmp.Text = pbs.Name;
                    tnTmp.Tag = pbs;

                    tvwCategory.SelectedNode.Nodes.Add(tnTmp);

                    AddCopyChildNode(tnTmp, pbs, listCopyPBS);
                }

                tvwCategory.SelectedNode.Expand();

                //listCopyNode.Clear();
            }
        }

        private void AddCopyChildNode(TreeNode parentNode, GWBSTree parentPBS, IEnumerable<GWBSTree> listCopyPBS)
        {
            IEnumerable<GWBSTree> listCopyChild = from n in listCopyPBS
                                                  where n.ParentNode.Id == parentPBS.Id
                                                  select n;

            foreach (GWBSTree pbs in listCopyChild)
            {
                TreeNode tnTmp = new TreeNode();
                tnTmp.Name = pbs.Id.ToString();
                tnTmp.Text = pbs.Name;
                tnTmp.Tag = pbs;

                parentNode.Nodes.Add(tnTmp);

                AddCopyChildNode(tnTmp, pbs, listCopyPBS);
            }
        }

        /// <summary>
        /// 获取要复制的节点
        /// </summary>
        private void GetCopyNode(TreeNode copyParentNode, GWBSTree saveParentNode, ref IList list)
        {
            foreach (TreeNode node in copyParentNode.Nodes)
            {
                if (listCopyNodeAll.Keys.Contains(node.Name))
                {
                    GWBSTree templateWBS = node.Tag as GWBSTree;
                    templateWBS = LoadRelaAttribute(templateWBS);

                    GWBSTree catTmp = templateWBS.Clone();

                    if (projectInfo != null)
                    {
                        catTmp.TheProjectGUID = projectInfo.Id;
                        catTmp.TheProjectName = projectInfo.Name;
                        catTmp.Code = projectInfo.Code.PadLeft(4, '0') + "-" + model.GetCode(typeof(GWBSTree));
                    }
                    //ContractGroup contractGroup = txtContractGroupCode.Tag as ContractGroup;
                    //catTmp.ContractGroupCode = contractGroup.Code;
                    //catTmp.ContractGroupGUID = contractGroup;

                    catTmp.ParentNode = saveParentNode;
                    //catTmp.OrderNo = model.GetMaxOrderNo(parentNode) + 1;//排序号就用复制节点的排序号

                    list.Add(catTmp);

                    GetCopyNode(node, catTmp, ref list);
                }
            }
        }

        //删除勾选节点
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
                    GWBSTree cg = dic.Value.Tag as GWBSTree;
                    if (cg.TaskState == DocumentState.Edit || cg.TaskState == 0)
                        list.Add(dic.Value.Tag as GWBSTree);
                }

                if (list.Count == 0)
                {
                    MessageBox.Show("选择节点中没有允许删除的节点，只能删除“编制”状态的节点！");
                    return;
                }

                string text = "要删除勾选的所有节点吗？该操作将连它们的所有子节点一同删除！";
                if (MessageBox.Show(text, "删除节点", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

                if (model.DeleteGWBSTree(list))//删除成功
                {
                    //从PBS树上移除对应的节点
                    foreach (GWBSTree pbs in list)
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

                        if (listCheckedNode.ContainsKey(pbs.Id))
                        {
                            listCheckedNode.Remove(pbs.Id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ex));
            }
        }
        private bool RemoveTreeNode(TreeNode parentNode, GWBSTree pbs)
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

        #region 操作按钮
        void btnExportMPP_Click(object sender, EventArgs e)
        {

        }

        void btnExportExcel_Click(object sender, EventArgs e)
        {

        }

        void btnPublish_Click(object sender, EventArgs e)
        {
            mnuTree.Hide();
            PublisthNode();
            this.RefreshControls(MainViewState.Browser);
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            IsSubmit = false;
            mnuTree.Hide();
            if (!SaveView()) return;
            this.RefreshControls(MainViewState.Browser);
        }

        void btnSelectWBSType_Click(object sender, EventArgs e)
        {
            VSelectPBSAndTaskType frm = new VSelectPBSAndTaskType();
            frm.IsSingleSelectTaskType = true;

            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("ThePBS", NHibernate.FetchMode.Eager);
            Disjunction dis = new Disjunction();

            TreeNode parentTreeNode = null;
            GWBSTree parentNode = null;

            if (IsInsertNode || ViewState == MainViewState.Modify)//修改节点
            {
                parentTreeNode = currNode.Parent;
                parentNode = parentTreeNode.Tag as GWBSTree;
                parentNode = LoadRelaAttribute(parentNode);
                parentTreeNode.Tag = parentNode;

            }
            else
            {
                parentTreeNode = currNode;
                parentNode = parentTreeNode.Tag as GWBSTree;
                parentNode = LoadRelaAttribute(parentNode);
                parentTreeNode.Tag = parentNode;
            }

            foreach (GWBSRelaPBS rela in parentNode.ListRelaPBS)
            {
                dis.Add(Expression.Eq("Id", rela.Id));
            }
            oq.AddCriterion(dis);

            IList listRela = model.ObjectQuery(typeof(GWBSRelaPBS), oq);
            foreach (GWBSRelaPBS rela in listRela)
            {
                frm.ParentPBSType.Add(rela.ThePBS.StructTypeName);
            }
            frm.ParentTaskType = parentNode.ProjectTaskTypeGUID.TypeLevel.ToString();
            frm.ParentNode = parentTreeNode;

            //设置初始选择的对象
            if (txtTaskWBSType.Tag != null)
                frm.InitTaskType = txtTaskWBSType.Tag as ProjectTaskTypeTree;
            if (cbRelaPBS.Tag != null)
            {
                frm.InitListPBS = cbRelaPBS.Tag as List<PBSTree>;
            }

            frm.ShowDialog();

            if (frm.IsOK)
            {
                List<PBSTree> listPBS = frm.SelectedPBS;
                List<ProjectTaskTypeTree> listTaskType = frm.SelectedTaskType;

                //SaveSelectNodes(frm.SelectedPBS, frm.SelectedTaskType);

                ProjectTaskTypeTree type = listTaskType[0];
                txtTaskWBSType.Text = type.FullPath;
                txtTaskWBSType.Tag = type;


                //List<PBSTree> listOld = cbRelaPBS.Tag as List<PBSTree>;
                //if (listOld != null && listOld.Count > 0)
                //{
                //    foreach (PBSTree oldPBS in listOld)
                //    {
                //        for (int i = 0; i < listPBS.Count; i++)
                //        {
                //            PBSTree newPBS = listPBS[i];
                //            if (oldPBS.Id == newPBS.Id)
                //            {
                //                break;
                //            }

                //            if (i == listPBS.Count - 1)
                //                listPBS.Add(oldPBS);
                //        }
                //    }
                //}

                cbRelaPBS.Items.Clear();
                foreach (PBSTree pbs in listPBS)
                {
                    cbRelaPBS.Items.Add(pbs);
                }

                cbRelaPBS.DisplayMember = "FullPath";
                cbRelaPBS.ValueMember = "Id";
                cbRelaPBS.Tag = listPBS;

                cbRelaPBS.SelectedIndex = 0;

                //RefreshControls(MainViewState.Browser);
            }
        }

        void btnSelectRelaPBS_Click(object sender, EventArgs e)
        {
            VSelectPBSNode frm = new VSelectPBSNode();
            frm.SelectMethod = SelectNodeMethod.零散节点选择;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                List<TreeNode> listSelectNode = frm.SelectResult;

                List<PBSTree> listPBS = new List<PBSTree>();
                foreach (TreeNode tn in listSelectNode)
                {
                    PBSTree pbs = tn.Tag as PBSTree;
                    listPBS.Add(pbs);
                    cbRelaPBS.Items.Add(pbs);
                }

                cbRelaPBS.DisplayMember = "Name";
                cbRelaPBS.ValueMember = "Id";

                List<PBSTree> listOld = cbRelaPBS.Tag as List<PBSTree>;
                if (listOld != null && listOld.Count > 0)
                {
                    listPBS.AddRange(listOld);
                }
                cbRelaPBS.Tag = listPBS;

                cbRelaPBS.SelectedIndex = 0;
            }
        }
        #endregion
    }
}
