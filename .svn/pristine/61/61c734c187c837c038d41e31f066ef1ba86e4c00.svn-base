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

using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using ImportIntegration;
using System.IO;
using System.Diagnostics;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using IRPServiceModel.Domain.Document;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VGWBSTree : TBasicDataView
    {
        private TreeNode currNode = null;
        private GWBSTree oprNode = null;

        //有权限的业务组织
        private IList lstInstance;
        //唯一编码
        private string uniqueCode;

        private InspectionLot record;
        /// <summary>
        /// 检验批
        /// </summary>
        public InspectionLot Record
        {
            get { return record; }
            set { record = value; }
        }

        private Hashtable hashtableRules = new Hashtable();
        private List<TreeNode> listFindNodes = new List<TreeNode>();
        private int showFindNodeIndex = 0;
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
        public PersonInfo person = null;
        public MDocumentCategory docModel = new MDocumentCategory();

        string filePath = string.Empty;
        string objecIsGWBS = string.Empty;
        string addOrUpDate = string.Empty;

        public VGWBSTree(MGWBSTree mot)
        {
            model = mot;
            InitializeComponent();
            person = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;
            InitForm();
        }

        private void InitForm()
        {

            try
            {
                this.TopMost = true;
                FlashScreen.Show("正在初始化数据,请稍候......");

                InitEvents();

                projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();

                //检查要求
                IList list = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetBasicDataByName(VBasicDataOptr.WBS_CheckRequire);
                if (list != null)
                {
                    foreach (BasicDataOptr bdo in list)
                    {
                        cbListCheckRequire.Items.Add(bdo.BasicName);
                    }
                }

                //专业分类
                VBasicDataOptr.InitProfessionCategory(cbSpecialtyClassify, true);

                foreach (string flag in Enum.GetNames(typeof(OverOrUnderGroundFlagEnum)))
                {
                    cbOverOrUnderGroundFlag.Items.Add(flag);
                }

                //DateTime serverTime = model.GetServerTime();
                dtStartTime.Text = "";
                dtEndTime.Text = "";


                if (tvwCategory.Nodes.Count == 0)
                    RefreshControls(MainViewState.Initialize);
                else
                    RefreshControls(MainViewState.Browser);

                //LoadGWBSTreeTree();
                //LoadGWBSTree(null);//分层加载
                LoadGWBSTree();
                List<string> listParams = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetUploadFileParamsByMBP_IRP();
                this.TopMost = true;
                
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("初始化失败，详细信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                FlashScreen.Close();
                this.TopMost = true;
            }
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

            txtKeyWord.TextChanged += new EventHandler(txtKeyWord_TextChanged);
            txtKeyWord.KeyDown += new KeyEventHandler(txtKeyWord_KeyDown);
            btnFindTaskNode.Click += new EventHandler(btnFindTaskNode_Click);
            mnuTree.ItemClicked += new ToolStripItemClickedEventHandler(mnuTree_ItemClicked);

            tabBaseInfo.SelectedIndexChanged += new EventHandler(tabBaseInfo_SelectedIndexChanged);
            btnSelectWBSType.Click += new EventHandler(btnSelectWBSType_Click);

            txtPriceUnit.LostFocus += new EventHandler(txtPriceUnit_LostFocus);
            btnSelPriceUnit.Click += new EventHandler(btnSelPriceUnit_Click);

            btnSave.Click += new EventHandler(btnSave_Click);
            btnPublish.Click += new EventHandler(btnPublish_Click);
            btnExportMPP.Click += new EventHandler(btnExportMPP_Click);
            btnExportExcel.Click += new EventHandler(btnExportExcel_Click);
            btnRemovePBS.Click += new EventHandler(btnRemovePBS_Click);
        }

        //选项卡操作
        void tabBaseInfo_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (tabBaseInfo.SelectedTab == tabPageTaskBaseInfo)
            {
                GWBSTree optTask = tabPageTaskBaseInfo.Tag as GWBSTree;
                if (optTask != null && optTask.Id == oprNode.Id)
                    return;

                oprNode = LoadRelaAttribute(oprNode);

                tvwCategory.SelectedNode.Tag = oprNode;

                this.ShownNodeDetail();

                tabPageTaskBaseInfo.Tag = oprNode;
            }
        }

        private PBSTree selectRelaPBS = null;
        void cbTaskRelaPBS_SelectedIndexChanged(object sender, EventArgs e)
        {
            //selectRelaPBS = cbTaskRelaPBS.SelectedItem as PBSTree;
        }

        void cbTaskRelaPBS_TextUpdate(object sender, EventArgs e)
        {
            
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
            if (e.Button == MouseButtons.Right)// && ConstMethod.Contains(lstInstance, e.Node.Tag as CategoryNode)
            {
                tvwCategory.SelectedNode = e.Node;

                if (oprNode.TaskState != DocumentState.Invalid)
                {
                    mnuTree.Items[增加子节点.Name].Enabled = true;
                    mnuTree.Items[修改节点.Name].Enabled = true;
                }
                else
                {
                    mnuTree.Items[增加子节点.Name].Enabled = false;
                    mnuTree.Items[修改节点.Name].Enabled = false;
                }
                mnuTree.Items[从PBS上拷贝节点.Name].Enabled = mnuTree.Items[修改节点.Name].Enabled;


                if (e.Node.Parent == null)
                {
                    mnuTree.Items[删除节点.Name].Enabled = false;
                    this.mnuTree.Items[插入同级节点.Name].Enabled = false;
                }
                else if (oprNode.TaskState != DocumentState.Edit)
                {
                    mnuTree.Items[删除节点.Name].Enabled = false;
                    this.mnuTree.Items[插入同级节点.Name].Enabled = true;
                }
                else
                {
                    mnuTree.Items[删除节点.Name].Enabled = true;
                    this.mnuTree.Items[插入同级节点.Name].Enabled = true;
                }

                if (listCheckedNode.Count == 0)
                {
                    mnuTree.Items[复制勾选节点.Name].Enabled = false;
                    mnuTree.Items[删除勾选节点.Name].Enabled = false;
                    mnuTree.Items[清除勾选节点.Name].Enabled = false;
                }
                else
                {
                    mnuTree.Items[复制勾选节点.Name].Enabled = true;
                    mnuTree.Items[删除勾选节点.Name].Enabled = true;
                    mnuTree.Items[清除勾选节点.Name].Enabled = true;
                }

                if (listCopyNode.Count == 0)
                    mnuTree.Items[粘贴节点.Name].Enabled = false;
                else
                    mnuTree.Items[粘贴节点.Name].Enabled = true;

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

                if (tabBaseInfo.SelectedTab == tabPageTaskBaseInfo)
                {
                    oprNode = LoadRelaAttribute(oprNode);

                    tvwCategory.SelectedNode.Tag = oprNode;

                    this.ShownNodeDetail();

                    tabPageTaskBaseInfo.Tag = oprNode;
                }
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
            oq.AddFetchMode("ListRelaPBS.ThePBS", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ProjectTaskTypeGUID", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("PriceAmountUnitGUID", NHibernate.FetchMode.Eager);


            IList list = model.ObjectQuery(typeof(GWBSTree), oq);
            if (list.Count > 0)
            {
                wbs = list[0] as GWBSTree;
            }
            return wbs;
        }

        void tvwCategory_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null)
            {
                GWBSTree wbs = model.GetGWBSTreeById(e.Node.Name);
                e.Node.Tag = wbs;
            }
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

        private void ShownNodeDetail()
        {
            try
            {
                ClearAll();

                //基本信息
                this.txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath;
                this.txtTaskState.Text = StaticMethod.GetWBSTaskStateText(oprNode.TaskState);
                this.txtTaskCode.Text = oprNode.Code;
                this.txtTaskName.Text = oprNode.Name;
                this.cbSpecialtyClassify.Text = oprNode.SpecialType;


                if (oprNode.ListRelaPBS.Count > 0)
                {
                    List<PBSTree> listPBS = new List<PBSTree>();
                    for (int i = 0; i < oprNode.ListRelaPBS.Count; i++)
                    {
                        PBSTree pbs = oprNode.ListRelaPBS.ElementAt(i).ThePBS;
                        //pbs.FullPath = GetFullPath(pbs);

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

                this.txtTaskDesc.Text = oprNode.Describe;

                this.txtOwner.Text = oprNode.OwnerName;
                if (oprNode.TaskStateTime != null)
                    this.txtStateTime.Text = oprNode.TaskStateTime.Value.ToString();

                if (oprNode.TaskPlanStartTime != null)
                    this.dtStartTime.Value = oprNode.TaskPlanStartTime.Value;

                if (oprNode.TaskPlanEndTime != null)
                    this.dtEndTime.Value = oprNode.TaskPlanEndTime.Value;

                txtPriceUnit.Text = oprNode.PriceAmountUnitName;
                txtContractTotalPrice.Text = oprNode.ContractTotalPrice.ToString();
                txtResponsibilityTotalPrice.Text = oprNode.ResponsibilityTotalPrice.ToString();
                txtPlanTotalPrice.Text = oprNode.PlanTotalPrice.ToString();

                //检查要求
                if (!string.IsNullOrEmpty(oprNode.CheckRequire))
                {
                    char[] chs = oprNode.CheckRequire.ToCharArray();
                    for (int i = 0; i < chs.Length; i++)
                    {
                        Char c = chs[i];
                        if (c == '0')
                        {
                            if (cbListCheckRequire.Items.Count > i)
                                cbListCheckRequire.SetItemChecked(i, true);
                        }
                        else
                        {
                            if (cbListCheckRequire.Items.Count > i)
                                cbListCheckRequire.SetItemChecked(i, false);
                        }
                    }
                }

                cbResponseAccount.Checked = oprNode.ResponsibleAccFlag;
                cbCostAccount.Checked = oprNode.CostAccFlag;
                cbProductConfirm.Checked = oprNode.ProductConfirmFlag;
                cbSubContractFee.Checked = oprNode.SubContractFeeFlag;

                cbWarehouseFlag.Checked = oprNode.WarehouseFlag;

                txtFigureProgress.Text = oprNode.AddUpFigureProgress.ToString();
                txtCheckBatchNum.Text = oprNode.CheckBatchNumber.ToString();

                cbOverOrUnderGroundFlag.Text = oprNode.OverOrUndergroundFlag.ToString();

                //日常检查要求
                txtDayCheckState.Text = StaticMethod.GetCheckStateShowText(oprNode.DailyCheckState);

                txtAcceptanceCheckState.Text = oprNode.AcceptanceCheckState == 0 ? AcceptanceCheckStateEnum.未通过.ToString() : oprNode.AcceptanceCheckState.ToString();
                txtSuperiorCheckState.Text = oprNode.SuperiorCheckState == 0 ? SuperiorCheckStateEnum.未通过.ToString() : oprNode.SuperiorCheckState.ToString();

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

            this.cbSpecialtyClassify.Text = "";

            this.cbRelaPBS.Items.Clear();
            this.cbRelaPBS.Tag = null;

            this.txtTaskWBSType.Text = "";
            this.txtTaskWBSType.Tag = null;

            this.txtTaskDesc.Text = "";

            this.txtOwner.Text = "";
            this.txtStateTime.Text = "";

            //DateTime serverTime = model.GetServerTime();
            this.dtStartTime.Text = "";
            this.dtEndTime.Text = "";

            txtPriceUnit.Text = "";
            txtContractTotalPrice.Text = "";
            txtResponsibilityTotalPrice.Text = "";
            txtPlanTotalPrice.Text = "";

            cbResponseAccount.Checked = false;
            cbCostAccount.Checked = false;
            cbProductConfirm.Checked = false;
            cbSubContractFee.Checked = false;
            cbWarehouseFlag.Checked = false;

            for (int i = 0; i < cbListCheckRequire.Items.Count; i++)
            {
                cbListCheckRequire.SetItemChecked(i, false);
            }

            txtFigureProgress.Text = "";
            txtCheckBatchNum.Text = "";
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
                model.DeleteGWBSTree(oprNode);
                //删除日志
                LogData log = new LogData();
                log.BillId = oprNode.Id;
                log.BillType = "工程任务";
                log.Code = "任务名称：" + oprNode.Name;
                log.OperType = "删除";
                log.Descript = "工程任务删除记录";
                log.OperPerson = ConstObject.LoginPersonInfo.Name;
                log.ProjectName = oprNode.TheProjectName;
                StaticMethod.InsertLogData(log);

                if (reset)
                {
                    if (tvwCategory.SelectedNode.Parent.Tag == null)
                    {
                        GWBSTree wbs = model.GetGWBSTreeById(tvwCategory.SelectedNode.Parent.Name);
                        tvwCategory.SelectedNode.Parent.Tag = wbs;
                    }
                    GWBSTree parentObj = tvwCategory.SelectedNode.Parent.Tag as GWBSTree;
                    tvwCategory.SelectedNode.Parent.Tag = LoadRelaAttribute(parentObj);
                }

                //如果删除的节点有勾选的从选中集合中移除
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
                    MessageBox.Show("该节点被其它数据所引用，删除前请先删除引用的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                if (tn.Tag != null)
                {
                    GWBSTree wbsSelect = tn.Tag as GWBSTree;
                    if (model.GetGWBSDetailLikeWBSSysCodeSql(wbsSelect.SysCode) > 0)
                    {
                        MessageBox.Show("此节点或其子节点上有发布状态的任务明细，不能删除！");
                        return false;
                    }
                }

                string text = "确认要删除节点“" + tn.Text + "”及下属所有子节点吗？";
                if (MessageBox.Show(text, "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return false;

                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
                return false;
            }
        }


        void txtKeyWord_TextChanged(object sender, EventArgs e)
        {
            listFindNodes.Clear();
            showFindNodeIndex = 0;
        }

        void txtKeyWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyData == Keys.Enter)
            {
                btnFindTaskNode_Click(btnFindTaskNode, new EventArgs());
            }
        }

        //查找/下一个
        void btnFindTaskNode_Click(object sender, EventArgs e)
        {
            if (txtKeyWord.Text.Trim() == "")
                return;

            if (listFindNodes.Count > 0)
            {
                showFindNodeIndex += 1;
                if (showFindNodeIndex > listFindNodes.Count - 1)
                    showFindNodeIndex = 0;

                ShowFindNode(listFindNodes[showFindNodeIndex]);
            }
            else
            {
                string keyWord = txtKeyWord.Text.Trim();

                foreach (TreeNode tn in tvwCategory.Nodes)
                {
                    if (tn.Text.IndexOf(keyWord) > -1)
                    {
                        listFindNodes.Add(tn);
                    }

                    QueryCheckedTreeNode(tn, keyWord);
                }

                if (listFindNodes.Count > 0)
                {
                    showFindNodeIndex = 0;
                    ShowFindNode(listFindNodes[showFindNodeIndex]);

                }
            }
        }
        private void ShowFindNode(TreeNode tn)
        {
            TreeNode theParentNode = tn.Parent;
            while (theParentNode != null)
            {
                theParentNode.Expand();
                theParentNode = theParentNode.Parent;
            }

            tvwCategory.Select();
            tvwCategory.SelectedNode = tn;
        }
        private void QueryCheckedTreeNode(TreeNode parentNode, string keyWord)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Text.IndexOf(keyWord) > -1)
                {
                    listFindNodes.Add(tn);
                }

                QueryCheckedTreeNode(tn, keyWord);
            }
        }

        void mnuTree_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == 增加子节点.Name)
            {
                mnuTree.Hide();
                RefreshControls(MainViewState.AddNew);
                add_Click(null, new EventArgs());
            }
            if (e.ClickedItem.Name == 插入同级节点.Name)
            {
                mnuTree.Hide();
                RefreshControls(MainViewState.Modify);
                InsertBrotherNode();
            }
            else if (e.ClickedItem.Name == 修改节点.Name)
            {
                mnuTree.Hide();

                RefreshControls(MainViewState.Modify);
            }
            else if (e.ClickedItem.Name == 删除节点.Name)
            {
                mnuTree.Hide();
                delete_Click(null, new EventArgs());
                RefreshControls(MainViewState.Browser);
                this.Refresh();
            }
            else if (e.ClickedItem.Name == 撤销.Name)
            {
                mnuTree.Hide();
                RefreshControls(MainViewState.Browser);
                this.tvwCategory_AfterSelect(this.tvwCategory, new TreeViewEventArgs(this.tvwCategory.SelectedNode));
            }
            else if (e.ClickedItem.Name == 保存节点.Name)
            {
                IsSubmit = false;
                mnuTree.Hide();
                SaveView();
            }
            else if (e.ClickedItem.Name == 提交任务.Name)
            {
                IsSubmit = true;
                mnuTree.Hide();
                if (!SaveView()) return;
                this.RefreshControls(MainViewState.Browser);
            }
            else if (e.ClickedItem.Name == 发布节点.Name)
            {
                mnuTree.Hide();

                if (MessageBox.Show("确认要发布当前节点“" + oprNode.Name + "”及下属所有子节点吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                PublisthNodeAndChilds();
                this.RefreshControls(MainViewState.Browser);
            }
            else if (e.ClickedItem.Name == 发布节点及其子节点.Name)
            {
                mnuTree.Hide();

                if (MessageBox.Show("确认要发布当前节点“" + oprNode.Name + "”及下属所有子节点吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                PublisthNodeAndChilds();
                this.RefreshControls(MainViewState.Browser);
            }
            else if (e.ClickedItem.Name == 作废节点.Name)
            {
                mnuTree.Hide();

                if (MessageBox.Show("确认要作废当前节点“" + oprNode.Name + "”及下属所有子节点吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                CancellationNodeAndChilds();
                this.RefreshControls(MainViewState.Browser);
            }
            else if (e.ClickedItem.Name == 作废节点及其子节点.Name)
            {
                mnuTree.Hide();

                if (MessageBox.Show("确认要作废当前节点“" + oprNode.Name + "”及下属所有子节点吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                CancellationNodeAndChilds();
                this.RefreshControls(MainViewState.Browser);
            }
            else if (e.ClickedItem.Name == 复制勾选节点.Name)
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
            else if (e.ClickedItem.Name == 粘贴节点.Name)
            {
                mnuTree.Hide();

                oprNode = LoadRelaAttribute(oprNode);
                currNode.Tag = oprNode;

                #region pbs关联任务类型规则校验
                //ObjectQuery oq = new ObjectQuery();
                //oq.AddFetchMode("ThePBS", NHibernate.FetchMode.Eager);

                //Disjunction dis = new Disjunction();
                //foreach (GWBSRelaPBS rela in oprNode.ListRelaPBS)
                //{
                //    dis.Add(Expression.Eq("Id", rela.Id));
                //}
                //oq.AddCriterion(dis);

                //IList listRela = model.ObjectQuery(typeof(GWBSRelaPBS), oq);

                //oq.Criterions.Clear();
                //oq.FetchModes.Clear();
                //dis = new Disjunction();
                //foreach (GWBSRelaPBS rela in listRela)
                //{
                //    dis.Add(Expression.Eq("PBSType", rela.ThePBS.StructTypeName.Trim()));
                //}
                //oq.AddCriterion(dis);
                //oq.AddCriterion(NHibernate.Criterion.Expression.Eq("TaskType", oprNode.ProjectTaskTypeGUID.TypeLevel.ToString().Trim()));
                //oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                //IList list = model.ObjectQuery(typeof(PBSRelaTaskTypeRuleMaster), oq);
                //if (list == null || list.Count == 0)
                //{
                //    MessageBox.Show("当前任务节点关联的PBS和任务类型不符合GWBS节点规则，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}

                ////获取父节点下允许添加子节点的pbs和任务类型的组合规则
                //List<PBSRelaTaskTypeRuleDetail> ListRuleDtl = new List<PBSRelaTaskTypeRuleDetail>();
                //foreach (PBSRelaTaskTypeRuleMaster rule in list)
                //{
                //    ListRuleDtl.AddRange(rule.Details.ToList());
                //}

                ////规则校验
                //GWBSTree copyWBS = listCopyNode[0].Tag as GWBSTree;
                //copyWBS = LoadRelaAttribute(copyWBS);

                //foreach (GWBSRelaPBS rela in copyWBS.ListRelaPBS)
                //{
                //    var query = from r in ListRuleDtl
                //                where r.PBSType == rela.ThePBS.StructTypeName &&
                //                r.TaskType == copyWBS.ProjectTaskTypeGUID.TypeLevel.ToString()
                //                select r;

                //    if (query == null || query.Count() == 0)
                //    {
                //        MessageBox.Show("当前节点下添加PBS节点“" + rela.ThePBS.Name + "[" + rela.ThePBS.StructTypeName + "]”和任务类型节点“" +
                //            copyWBS.ProjectTaskTypeGUID.Name + "[" + copyWBS.ProjectTaskTypeGUID.TypeLevel + "]”的组合不符合添加规则，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                //        return;
                //    }
                //}
                #endregion

                SaveCopyNode();

                RefreshControls(MainViewState.Check);
            }
            else if (e.ClickedItem.Name == 删除勾选节点.Name)
            {
                mnuTree.Hide();

                //if (MessageBox.Show("确认要永久删除勾选的所有节点吗", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                //    return;

                tvwCategory.SelectedNode = tvwCategory.Nodes[0];

                DeleteCheckedNode();

                RefreshControls(MainViewState.Check);
            }
            else if (e.ClickedItem.Name == 从PBS上拷贝节点.Name)
            {
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
                    VSelectPBSAndTaskType.SelectedPBSAndTaskTypeMode selectMode = frm.SelectedMode;
                    List<PBSTree> listSelectedPBS = frm.SelectedPBS;
                    List<ProjectTaskTypeTree> listSelectedTaskType = frm.SelectedTaskType;
                    SaveSelectNodes(frm.SelectedPBSFirstNode, frm.SelectedTaskTypeFirstNode, listSelectedPBS, listSelectedTaskType, selectMode);

                    //SaveSelectNodes(frm.SelectResult, frm.SelectMethod);

                    RefreshControls(MainViewState.Browser);
                }
            }
            else if (e.ClickedItem.Name == 清除勾选节点.Name)
            {
                mnuTree.Hide();

                ClearSelectedNode(tvwCategory.Nodes[0]);

                listCheckedNode.Clear();

                RefreshControls(MainViewState.Check);
            }
            else if (e.ClickedItem.Name == 成本数据编辑.Name)
            {
                mnuTree.Hide();

                VGWBSDetailCostEditAndUsageEdit frm = new VGWBSDetailCostEditAndUsageEdit();
                frm.DefaultGWBSTreeNode = tvwCategory.SelectedNode;
                frm.ShowDialog();
            }
            else if (e.ClickedItem.Name == 商务数据统计.Name)
            {
                GWBSTree wbs = new GWBSTree();
                if (tvwCategory.SelectedNode.Tag != null)
                {
                    wbs = tvwCategory.SelectedNode.Tag as GWBSTree;
                    VGWBSBusinessStatistics bs = new VGWBSBusinessStatistics(wbs);
                    bs.ShowDialog();
                }
            }
            else if (e.ClickedItem.Name == 工程数据统计.Name)
            {
                GWBSTree wbs = new GWBSTree();
                if (tvwCategory.SelectedNode.Tag != null)
                {
                    wbs = tvwCategory.SelectedNode.Tag as GWBSTree;
                    VGWBSEngineeringStatistics es = new VGWBSEngineeringStatistics(wbs);
                    es.ShowDialog();
                }
            }
            else if (e.ClickedItem.Name == 分摊计划工程量.Name)
            {
                GWBSTree wbs = new GWBSTree();
                if (tvwCategory.SelectedNode.Tag != null)
                {
                    wbs = tvwCategory.SelectedNode.Tag as GWBSTree;
                    VPlanProjectAmountSharing vs = new VPlanProjectAmountSharing(wbs);
                    vs.ShowDialog();
                }
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

                    txtTaskState.Text = StaticMethod.GetWBSTaskStateText(oprNode.TaskState);
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
        /// 发布节点及其子节点
        /// </summary>
        private void PublisthNodeAndChilds()
        {
            if (oprNode != null)
            {
                try
                {

                    oprNode = model.PublisthTaskNodeAndChilds(oprNode);

                    txtTaskState.Text = StaticMethod.GetWBSTaskStateText(oprNode.TaskState);
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

                txtTaskState.Text = StaticMethod.GetWBSTaskStateText(oprNode.TaskState);
                txtStateTime.Text = oprNode.TaskStateTime.ToString();

                tvwCategory.SelectedNode.Tag = oprNode;
            }
        }

        /// <summary>
        /// 作废节点
        /// </summary>
        private void CancellationNodeAndChilds()
        {
            if (oprNode != null)
            {
                oprNode = model.InvalidTaskNodeAndChilds(oprNode);

                txtTaskState.Text = StaticMethod.GetWBSTaskStateText(oprNode.TaskState);
                txtStateTime.Text = oprNode.TaskStateTime.ToString();

                tvwCategory.SelectedNode.Tag = oprNode;
            }
        }

        private void SaveSelectNodes(List<TreeNode> listFirstPBS, List<TreeNode> listFirstTask, List<PBSTree> listPBS, List<ProjectTaskTypeTree> listTaskType, VSelectPBSAndTaskType.SelectedPBSAndTaskTypeMode selectMode)
        {

            if (listFirstPBS.Count > 0 && listFirstTask.Count > 0)
            {
                if (oprNode != null)
                    oprNode = LoadRelaAttribute(oprNode);

                #region 生成WBS节点
                IList lst = new ArrayList();
                if (selectMode == VSelectPBSAndTaskType.SelectedPBSAndTaskTypeMode.多对一)
                {
                    foreach (TreeNode taskTypeNode in listFirstTask)
                    {
                        if (taskTypeNode.Checked == false)
                            continue;

                        ProjectTaskTypeTree task = taskTypeNode.Tag as ProjectTaskTypeTree;

                        GWBSTree wbs = new GWBSTree();
                        wbs.ProjectTaskTypeGUID = task;
                        wbs.ProjectTaskTypeName = task.Name;

                        wbs.CheckRequire = task.CheckRequire.PadRight(11, 'X') + "0";
                        wbs.DailyCheckState = wbs.CheckRequire;

                        foreach (PBSTree pbs in listPBS)
                        {
                            GWBSRelaPBS relaPBS = new GWBSRelaPBS();
                            relaPBS.ThePBS = pbs;

                            relaPBS.TheGWBSTree = wbs;
                            wbs.ListRelaPBS.Add(relaPBS);

                            wbs.Name = pbs.Name + task.Name;
                            wbs.Code = pbs.Code + "-" + task.Code;
                        }

                        if (projectInfo != null)
                        {
                            wbs.TheProjectGUID = projectInfo.Id;
                            wbs.TheProjectName = projectInfo.Name;
                            //wbs.Code = projectInfo.Code.PadLeft(4, '0') + "-" + model.GetCode(typeof(GWBSTree));//服务端生成
                        }


                        wbs.TaskState = DocumentState.Edit;
                        //wbs.TaskStateTime = model.GetServerTime();
                        wbs.OwnerGUID = ConstObject.LoginPersonInfo.Id;
                        wbs.OwnerName = ConstObject.LoginPersonInfo.Name;
                        wbs.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;


                        if (oprNode != null)//非根节点情况
                        {
                            wbs.ParentNode = oprNode;
                            wbs.OrderNo = model.GetMaxOrderNo(wbs) + 1;
                            wbs.Summary = oprNode.Summary + "," + wbs.Name;
                            wbs.OverOrUndergroundFlag = oprNode.OverOrUndergroundFlag;

                            wbs.FullPath = currNode.FullPath + @"\" + wbs.Name;
                            wbs.Level = wbs.ParentNode.Level + 1;

                            wbs.TheTree = oprNode.TheTree;
                            wbs.CategoryNodeType = NodeType.LeafNode;
                        }
                        else
                        {
                            wbs.Name = projectInfo.Name;
                            wbs.OrderNo = 0;
                            wbs.Summary = wbs.Name;
                            wbs.FullPath = wbs.Name;
                            wbs.Level = 1;
                            wbs.CategoryNodeType = NodeType.RootNode;
                        }

                        lst.Add(wbs);

                        //克隆要复制的节点和其子节点的对象
                        PopulateListByCopyNode(taskTypeNode, listPBS, lst, wbs);
                    }
                }
                else if (selectMode == VSelectPBSAndTaskType.SelectedPBSAndTaskTypeMode.一对一)
                {
                    foreach (TreeNode pbsNode in listFirstPBS)
                    {
                        if (pbsNode.Checked == false)
                            continue;

                        PBSTree pbs = pbsNode.Tag as PBSTree;
                        GWBSTree wbs = new GWBSTree();

                        GWBSRelaPBS relaPBS = new GWBSRelaPBS();
                        relaPBS.ThePBS = pbs;
                        relaPBS.TheGWBSTree = wbs;
                        wbs.ListRelaPBS.Add(relaPBS);

                        foreach (ProjectTaskTypeTree task in listTaskType)
                        {
                            wbs.ProjectTaskTypeGUID = task;
                            wbs.ProjectTaskTypeName = task.Name;
                            wbs.CheckRequire = task.CheckRequire.PadRight(11, 'X') + "0";
                            wbs.DailyCheckState = wbs.CheckRequire;

                            wbs.Name = pbs.Name + task.Name;
                        }

                        if (projectInfo != null)
                        {
                            wbs.TheProjectGUID = projectInfo.Id;
                            wbs.TheProjectName = projectInfo.Name;
                            //wbs.Code = projectInfo.Code.PadLeft(4, '0') + "-" + model.GetCode(typeof(GWBSTree));//服务端生成
                        }

                        wbs.TaskState = DocumentState.Edit;
                        //wbs.TaskStateTime = model.GetServerTime();
                        wbs.OwnerGUID = ConstObject.LoginPersonInfo.Id;
                        wbs.OwnerName = ConstObject.LoginPersonInfo.Name;
                        wbs.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;

                        if (oprNode != null)//非根节点情况
                        {
                            wbs.ParentNode = oprNode;
                            wbs.OrderNo = model.GetMaxOrderNo(wbs) + 1;
                            wbs.Summary = oprNode.Summary + "," + wbs.Name;
                            wbs.OverOrUndergroundFlag = oprNode.OverOrUndergroundFlag;

                            wbs.FullPath = currNode.FullPath + @"\" + wbs.Name;
                            wbs.Level = wbs.ParentNode.Level + 1;

                            wbs.TheTree = oprNode.TheTree;
                            wbs.CategoryNodeType = NodeType.LeafNode;
                        }
                        else
                        {
                            wbs.Name = projectInfo.Name;
                            wbs.OrderNo = 0;
                            wbs.Summary = wbs.Name;
                            wbs.FullPath = wbs.Name;
                            wbs.Level = 1;
                            wbs.CategoryNodeType = NodeType.RootNode;
                        }

                        lst.Add(wbs);

                        //克隆要复制的节点和其子节点的对象
                        PopulateListByCopyNode(pbsNode, listTaskType, lst, wbs);
                    }
                }
                #endregion

                if (oprNode == null)
                {
                    lst = model.SaveGWBSTreeRootNode1(lst);
                    GWBSTree wbsNode = lst[0] as GWBSTree;

                    //给根节点tag设值
                    TreeNode rootNode = new TreeNode();
                    rootNode.Tag = wbsNode;
                    rootNode.Name = wbsNode.Id;
                    rootNode.Text = wbsNode.Name;
                    tvwCategory.Nodes.Add(rootNode);

                    //加载子节点
                    IEnumerable<GWBSTree> listGWBS = lst.OfType<GWBSTree>();
                    LoadGWBSChildNode(rootNode, wbsNode, listGWBS);
                }
                else
                {
                    if (oprNode.CategoryNodeType != NodeType.RootNode)
                        oprNode.CategoryNodeType = NodeType.MiddleNode;

                    lst.Insert(0, oprNode);

                    lst = model.SaveGWBSTrees1(lst);

                    oprNode = lst[0] as GWBSTree;
                    currNode.Tag = oprNode;

                    //加载子节点
                    IEnumerable<GWBSTree> listGWBS = lst.OfType<GWBSTree>();
                    LoadGWBSChildNode(currNode, oprNode, listGWBS);

                    currNode.Expand();
                }
            }
        }

        private void LoadGWBSChildNode(TreeNode parentNode, GWBSTree parentObj, IEnumerable<GWBSTree> listGWBS)
        {
            var query = from wbs in listGWBS
                        where wbs.ParentNode != null && wbs.ParentNode.Id == parentObj.Id
                        select wbs;

            foreach (GWBSTree wbs in query)
            {
                TreeNode childNode = new TreeNode();
                childNode.Tag = wbs;
                childNode.Name = wbs.Id;
                childNode.Text = wbs.Name;
                parentNode.Nodes.Add(childNode);

                LoadGWBSChildNode(childNode, wbs, listGWBS);
            }
        }

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

                //ContractGroup cg = txtContractGroupCode.Tag as ContractGroup;
                //txtRelaContractGroupCode.Text = cg.Code;
                //txtRelaContractGroupCode.Tag = cg;


                //if (oprNode.BearOrgGUID != null)
                //{
                //    cbOrg.Text = oprNode.BearOrgName;
                //    cbOrg.Tag = oprNode.BearOrgGUID;
                //}

                //界面显示
                txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath + "\\";
                txtTaskState.Text = StaticMethod.GetWBSTaskStateText(oprNode.TaskState);
                txtStateTime.Text = oprNode.TaskStateTime.Value.ToString();
                txtOwner.Text = oprNode.OwnerName;

                cbOverOrUnderGroundFlag.Text = parentNode.OverOrUndergroundFlag.ToString();

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

                //ContractGroup cg = txtContractGroupCode.Tag as ContractGroup;
                //txtRelaContractGroupCode.Text = cg.Code;
                //txtRelaContractGroupCode.Tag = cg;

                //if (oprNode.BearOrgGUID != null)
                //{
                //    cbOrg.Text = oprNode.BearOrgName;
                //    cbOrg.Tag = oprNode.BearOrgGUID;
                //}
                //界面显示
                txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath + "\\";
                txtTaskState.Text = StaticMethod.GetWBSTaskStateText(oprNode.TaskState);
                txtStateTime.Text = oprNode.TaskStateTime.Value.ToString();
                txtOwner.Text = oprNode.OwnerName;

                cbOverOrUnderGroundFlag.Text = oprNode.OverOrUndergroundFlag.ToString();

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
                //if (txtRelaContractGroupCode.Text == "")
                //{
                //    tabBaseInfo.SelectedIndex = 0;
                //    MessageBox.Show("契约组不能为空!");
                //    txtRelaContractGroupCode.Focus();
                //    return false;
                //}

                if (oprNode == null)
                {
                    oprNode = new GWBSTree();
                }
                else if (!string.IsNullOrEmpty(oprNode.Id))
                {
                    oprNode = LoadRelaAttribute(oprNode);

                    string currentPath = currNode.Parent == null ? "" : currNode.Parent.FullPath;
                    string name = txtTaskName.Text.Trim();
                    oprNode.FullPath = currentPath == "" ? name : currentPath + @"\" + name;
                }
                else if (string.IsNullOrEmpty(oprNode.Id))
                {
                    oprNode.FullPath = txtCurrentPath.Text + txtTaskName.Text.Trim();
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


                #region 绑定关联PBS

                bool validatePBSToParent = false;//校验与父亲的PBS规则
                bool validatePBSToChild = false;//校验与儿子的PBS规则
                if (cbRelaPBS.Tag != null)
                {

                    List<string> listPBS = new List<string>();
                    List<PBSTree> listTempPBS = cbRelaPBS.Tag as List<PBSTree>;
                    if (listTempPBS != null && listTempPBS.Count > 0)
                    {
                        foreach (PBSTree p in listTempPBS)
                        {
                            listPBS.Add(p.Id);
                        }
                    }


                    for (int i = oprNode.ListRelaPBS.Count - 1; i > -1; i--)
                    {
                        GWBSRelaPBS rela = oprNode.ListRelaPBS.ElementAt(i);

                        var query = from r in listPBS
                                    where r == rela.ThePBS.Id
                                    select r;

                        if (query.Count() > 0)//找到表示存在
                        {
                            listPBS.Remove(query.ElementAt(0));
                        }
                        else//没找到表示删除
                        {
                            oprNode.ListRelaPBS.Remove(rela);

                            validatePBSToChild = true;//移除PBS节点时需要校验与儿子的规则
                        }
                    }
                    //剩下的表示新增
                    foreach (string pbsId in listPBS)
                    {
                        GWBSRelaPBS relaPBS = new GWBSRelaPBS();

                        var query = from p in listTempPBS
                                    where p.Id == pbsId
                                    select p;

                        PBSTree pbs = query.ElementAt(0);

                        relaPBS.ThePBS = pbs;
                        relaPBS.PBSName = pbs.Name;

                        relaPBS.TheGWBSTree = oprNode;

                        oprNode.ListRelaPBS.Add(relaPBS);

                        validatePBSToParent = true;//添加PBS节点时需要校验与父亲的规则
                    }
                }
                else
                {
                    tabBaseInfo.SelectedIndex = 0;
                    MessageBox.Show("请选择关联的PBS！");
                    cbRelaPBS.Focus();
                    return false;
                }

                bool validateTaskType = false;//校验任务类型规则
                if (txtTaskWBSType.Tag != null)
                {
                    ProjectTaskTypeTree type = txtTaskWBSType.Tag as ProjectTaskTypeTree;
                    if (type != null)
                    {
                        if (oprNode.ProjectTaskTypeGUID == null || oprNode.ProjectTaskTypeGUID.Id != type.Id)
                        {
                            validateTaskType = true;
                        }

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


                #region PBS规则和任务类型规则校验

                GWBSTree parentWbs = oprNode.ParentNode as GWBSTree;
                if (parentWbs != null)
                {
                    parentWbs = LoadRelaAttribute(parentWbs);
                    if (validatePBSToParent)
                    {
                        List<string> listParentSysCode = new List<string>();

                        if (parentWbs.ListRelaPBS.Count > 0)
                        {
                            ObjectQuery oq = new ObjectQuery();
                            oq.AddFetchMode("ThePBS", NHibernate.FetchMode.Eager);

                            Disjunction dis = new Disjunction();
                            foreach (GWBSRelaPBS rela in parentWbs.ListRelaPBS)
                            {
                                dis.Add(Expression.Eq("Id", rela.Id));
                            }
                            oq.AddCriterion(dis);

                            IList listRela = model.ObjectQuery(typeof(GWBSRelaPBS), oq);

                            foreach (GWBSRelaPBS rela in listRela)
                            {
                                listParentSysCode.Add(rela.ThePBS.SysCode);
                            }
                        }


                        foreach (GWBSRelaPBS rela in oprNode.ListRelaPBS)
                        {
                            PBSTree pbs = rela.ThePBS;
                            var query = from s in listParentSysCode
                                        where pbs.SysCode.IndexOf(s) > -1
                                        select s;

                            if (query.Count() == 0)
                            {
                                MessageBox.Show("选择PBS节点“" + pbs.Name + "”不在父节点所属PBS子树上，这不符合添加规范，请检查！");
                                return false;
                            }
                        }
                    }


                    if (validatePBSToChild)
                    {
                        //校验与子节点的PBS规则
                        if (!string.IsNullOrEmpty(oprNode.Id))
                        {
                            List<string> listParentSysCode = new List<string>();
                            foreach (GWBSRelaPBS rela in oprNode.ListRelaPBS)
                            {
                                listParentSysCode.Add(rela.ThePBS.SysCode);
                            }


                            ObjectQuery oq = new ObjectQuery();
                            oq.AddCriterion(Expression.Eq("ParentNode.Id", oprNode.Id));
                            IList listChild = model.ObjectQuery(typeof(GWBSTree), oq);

                            oq.Criterions.Clear();
                            Disjunction dis = new Disjunction();
                            foreach (GWBSTree wbs in listChild)
                            {
                                dis.Add(Expression.Eq("TheGWBSTree.Id", wbs.Id));
                            }
                            oq.AddCriterion(dis);
                            oq.AddFetchMode("TheGWBSTree", NHibernate.FetchMode.Eager);
                            oq.AddFetchMode("ThePBS", NHibernate.FetchMode.Eager);

                            IList listChildRela = model.ObjectQuery(typeof(GWBSRelaPBS), oq);
                            foreach (GWBSRelaPBS rela in listChildRela)
                            {
                                PBSTree pbs = rela.ThePBS;
                                var query = from s in listParentSysCode
                                            where pbs.SysCode.IndexOf(s) > -1
                                            select s;

                                if (query.Count() == 0)
                                {
                                    MessageBox.Show("当前任务节点的子任务“" + rela.TheGWBSTree.Name + "”的所属PBS节点“" + pbs.Name + "”不在当前任务节点所属PBS子树上，这不符合添加规范，请检查！");
                                    return false;
                                }
                            }
                        }
                    }


                    if (validateTaskType)
                    {
                        //校验与父节点的任务类型规则
                        ProjectTaskTypeTree parentTaskType = parentWbs.ProjectTaskTypeGUID;
                        ProjectTaskTypeTree task = oprNode.ProjectTaskTypeGUID;

                        if (parentTaskType.TypeLevel != ProjectTaskTypeLevel.单位工程 && parentTaskType.TypeLevel != ProjectTaskTypeLevel.子单位工程 && parentTaskType.TypeLevel != ProjectTaskTypeLevel.专业)
                        {
                            if (task.SysCode.IndexOf(parentTaskType.SysCode) == -1 && task.Level <= parentTaskType.Level)
                            {
                                MessageBox.Show("选择任务类型“" + task.Name + "”不在父节点所属任务类型树上或层级高于父节点任务类型层级，这不符合添加规范，请检查！");
                                return false;
                            }
                        }

                        //校验与子节点的任务类型规则
                        if (!string.IsNullOrEmpty(oprNode.Id))
                        {
                            ObjectQuery oq = new ObjectQuery();
                            oq.AddCriterion(Expression.Eq("ParentNode.Id", oprNode.Id));
                            oq.AddFetchMode("ProjectTaskTypeGUID", NHibernate.FetchMode.Eager);
                            IList listChild = model.ObjectQuery(typeof(GWBSTree), oq);

                            parentTaskType = oprNode.ProjectTaskTypeGUID;
                            foreach (GWBSTree wbs in listChild)
                            {
                                task = wbs.ProjectTaskTypeGUID;

                                if (parentTaskType.TypeLevel != ProjectTaskTypeLevel.单位工程 && parentTaskType.TypeLevel != ProjectTaskTypeLevel.子单位工程 && parentTaskType.TypeLevel != ProjectTaskTypeLevel.专业)
                                {
                                    if (task.SysCode.IndexOf(parentTaskType.SysCode) == -1 && task.Level <= parentTaskType.Level)
                                    {
                                        MessageBox.Show("当前任务节点的子任务“" + wbs.Name + "”的任务类型“" + task.Name + "”不在当前任务节点所属任务类型树上或层级高于当前任务节点任务类型层级，这不符合添加规范，请检查！");
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion 规则校验

                #endregion

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
                if (dtStartTime.IsHasValue)
                    oprNode.TaskPlanStartTime = dtStartTime.Value;
                else
                    oprNode.TaskPlanStartTime = null;

                if (dtEndTime.IsHasValue)
                    oprNode.TaskPlanEndTime = dtEndTime.Value;
                else
                    oprNode.TaskPlanEndTime = null;

                StandardUnit su = txtPriceUnit.Tag as StandardUnit;
                oprNode.PriceAmountUnitGUID = su;
                oprNode.PriceAmountUnitName = su != null ? su.Name : null;

                try
                {
                    decimal ContractTotalPrice = 0;
                    if (txtContractTotalPrice.Text.Trim() != "")
                        ContractTotalPrice = Convert.ToDecimal(txtContractTotalPrice.Text);

                    oprNode.ContractTotalPrice = ContractTotalPrice;
                }
                catch
                {
                    MessageBox.Show("合同合价格式填写不正确！");
                    txtContractTotalPrice.Focus();
                    return false;
                }

                try
                {
                    decimal ResponsibilityTotalPrice = 0;
                    if (txtResponsibilityTotalPrice.Text.Trim() != "")
                        ResponsibilityTotalPrice = Convert.ToDecimal(txtResponsibilityTotalPrice.Text);

                    //赋值
                    oprNode.ResponsibilityTotalPrice = ResponsibilityTotalPrice;
                }
                catch
                {
                    MessageBox.Show("责任合价格式填写不正确！");
                    txtResponsibilityTotalPrice.Focus();
                    return false;
                }

                try
                {
                    decimal PlanTotalPrice = 0;
                    if (txtPlanTotalPrice.Text.Trim() != "")
                        PlanTotalPrice = Convert.ToDecimal(txtPlanTotalPrice.Text);

                    //赋值
                    oprNode.PlanTotalPrice = PlanTotalPrice;
                }
                catch
                {
                    MessageBox.Show("计划合价格式填写不正确！");
                    txtPlanTotalPrice.Focus();
                    return false;
                }

                oprNode.ResponsibleAccFlag = cbResponseAccount.Checked;
                oprNode.CostAccFlag = cbCostAccount.Checked;
                oprNode.ProductConfirmFlag = cbProductConfirm.Checked;
                oprNode.SubContractFeeFlag = cbSubContractFee.Checked;

                if (cbWarehouseFlag.Checked)
                {
                    string msg = string.Empty;

                    int validType = 2;
                    GWBSTree validNode = oprNode;
                    if (ViewState != MainViewState.Modify)
                    {
                        validType = 1;
                        validNode = oprNode.ParentNode as GWBSTree;
                    }
                    //2013-12-23
                    //if (ValidateWareFlag(validNode, validType, ref msg) == false)
                    //{
                    //    MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    cbWarehouseFlag.Focus();
                    //    return false;
                    //}
                }
                oprNode.WarehouseFlag = cbWarehouseFlag.Checked;

                string checkRequire = string.Empty;
                for (int i = 0; i < cbListCheckRequire.Items.Count; i++)
                {
                    if (cbListCheckRequire.GetItemChecked(i))
                        checkRequire += "0";
                    else
                        checkRequire += "X";
                }

                if (oprNode.CheckRequire != null && checkRequire.Length < oprNode.CheckRequire.Length)
                    checkRequire = checkRequire + oprNode.CheckRequire.Substring(checkRequire.Length);

                oprNode.CheckRequire = checkRequire;
                oprNode.DailyCheckState = oprNode.CheckRequire;

                try
                {
                    decimal AddUpFigureProgress = 0;
                    if (txtFigureProgress.Text.Trim() != "")
                        AddUpFigureProgress = Convert.ToDecimal(txtFigureProgress.Text);

                    oprNode.AddUpFigureProgress = AddUpFigureProgress;
                }
                catch
                {
                    MessageBox.Show("形象进度格式填写不正确！");
                    txtFigureProgress.Focus();
                    return false;
                }

                try
                {
                    int CheckBatchNumber = 0;
                    if (txtCheckBatchNum.Text.Trim() != "")
                        CheckBatchNumber = Convert.ToInt32(txtCheckBatchNum.Text);

                    oprNode.CheckBatchNumber = CheckBatchNumber;
                }
                catch
                {
                    MessageBox.Show("检验批数格式填写不正确！");
                    txtCheckBatchNum.Focus();
                    return false;
                }

                oprNode.OverOrUndergroundFlag = VirtualMachine.Component.Util.EnumUtil<OverOrUnderGroundFlagEnum>.FromDescription(cbOverOrUnderGroundFlag.Text);

                TreeNode pNode = null;
                if (string.IsNullOrEmpty(oprNode.Id) && IsInsertNode == false)
                    pNode = currNode;
                else
                    pNode = currNode.Parent;

                if ((cbSpecialtyClassify.SelectedItem == null || string.IsNullOrEmpty(cbSpecialtyClassify.SelectedItem.ToString())) && pNode != null)
                {
                    int count = 0;
                    while (pNode != null)
                    {
                        if (!string.IsNullOrEmpty((pNode.Tag as GWBSTree).SpecialType) && (pNode.Tag as GWBSTree).SpecialType != "其他")
                        {
                            count += 1;
                            break;
                        }
                        pNode = pNode.Parent;
                    }
                    if (count == 0)
                    {
                        MessageBox.Show("当前节点或直接、间接父节点上至少应选择一个专业分类！");
                        cbSpecialtyClassify.Focus();
                        return false;
                    }
                }
                oprNode.SpecialType = cbSpecialtyClassify.SelectedItem == null ? null : cbSpecialtyClassify.SelectedItem.ToString();

                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
                return false;
            }
        }
        /// <summary>
        /// 验证仓库标识是否符合规范（从当前节点到根节点的所有节点,或所有子节点上只能设置一个此标志）
        /// </summary>
        /// <param name="currWBSNode">验证的GWBS节点</param>
        /// <param name="validType">验证类型（1.新增，2.修改）</param>
        /// <param name="fullPath">存在此标志节点的完整路径</param>
        /// <returns>true表示可以添加，false则不能</returns>
        private bool ValidateWareFlag(GWBSTree currWBSNode, int validType, ref string msg)
        {
            if (currWBSNode == null)
                return false;

            //注：检查时依据数据库里的数据（防止并发界面数据和实际不一致问题）

            ObjectQuery oq = new ObjectQuery();


            //检查所有父节点的此标志
            string[] sysCodes = currWBSNode.SysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            oq.AddCriterion(Expression.Eq("WarehouseFlag", true));

            Disjunction dis = new Disjunction();
            for (int i = 0; i < sysCodes.Length; i++)
            {
                string sysCode = "";
                for (int j = 0; j <= i; j++)
                {
                    sysCode += sysCodes[j] + ".";

                }

                dis.Add(Expression.Eq("SysCode", sysCode));
            }
            oq.AddCriterion(dis);

            IList listParent = model.ObjectQuery(typeof(GWBSTree), oq);
            if (listParent.Count > 0)
            {
                GWBSTree parentNode = listParent[0] as GWBSTree;

                msg = "在父节点“" + StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), parentNode.Name, parentNode.SysCode) + "”上已经设置了“仓库标志”属性.";

                return false;
            }

            if (validType == 2)
            {
                //检查所有子节点的此标志
                oq.Criterions.Clear();
                oq.AddCriterion(Expression.Eq("WarehouseFlag", true));
                oq.AddCriterion(Expression.Like("SysCode", currWBSNode.SysCode, MatchMode.Start));
                oq.AddCriterion(Expression.Not(Expression.Eq("SysCode", currWBSNode.SysCode)));

                IList listChild = model.ObjectQuery(typeof(GWBSTree), oq);
                if (listChild.Count > 0)
                {
                    GWBSTree childNode = listChild[0] as GWBSTree;

                    msg = "在子节点“" + StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), childNode.Name, childNode.SysCode) + "”上已经设置了“仓库标志”属性.";

                    return false;
                }
            }

            return true;
        }

        public void Start()
        {
            RefreshState(MainViewState.Browser);

            tvwCategory.ItemDrag += new ItemDragEventHandler(tvwCategory_ItemDrag);
            tvwCategory.DragEnter += new DragEventHandler(tvwCategory_DragEnter);
            tvwCategory.DragDrop += new DragEventHandler(tvwCategory_DragDrop);
            tvwCategory.DragOver += new DragEventHandler(tvwCategory_DragOver);

            //LoadGWBSTreeTree();
            //LoadGWBSTree(null);
            LoadGWBSTree();
        }

        private void LoadGWBSTreeTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();
                IList list = model.GetGWBSTreesByInstance(projectInfo.Id);


                if (list == null || list.Count == 0)
                {
                    PBSTree pbs = null;
                    ProjectTaskTypeTree taskType = null;

                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
                    oq.AddCriterion(Expression.IsNull("ParentNode"));
                    oq.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
                    IList listPBS = model.ObjectQuery(typeof(PBSTree), oq);

                    if (listPBS == null || listPBS.Count == 0)
                        return;
                    pbs = listPBS[0] as PBSTree;


                    oq.Criterions.Clear();
                    oq.AddCriterion(Expression.IsNull("ParentNode"));
                    oq.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
                    IList listTaskType = model.ObjectQuery(typeof(ProjectTaskTypeTree), oq);

                    if (listTaskType == null || listTaskType.Count == 0)
                        return;
                    taskType = listTaskType[0] as ProjectTaskTypeTree;


                    IList listAdd = new ArrayList();

                    GWBSTree root = new GWBSTree();

                    DateTime serverTime = model.GetServerTime();

                    root.TheProjectGUID = projectInfo.Id;
                    root.TheProjectName = projectInfo.Name;
                    root.Name = projectInfo.Name;

                    root.Code = projectInfo.Code.PadLeft(4, '0') + "-" + model.GetCode(typeof(GWBSTree));
                    root.CheckRequire = string.IsNullOrEmpty(taskType.CheckRequire) ? ("X".PadRight(11, 'X') + "0") : (taskType.CheckRequire.PadRight(11, 'X') + "0");

                    GWBSRelaPBS relaPBS = new GWBSRelaPBS();
                    relaPBS.ThePBS = pbs;

                    relaPBS.TheGWBSTree = root;
                    root.ListRelaPBS.Add(relaPBS);

                    root.ProjectTaskTypeGUID = taskType;
                    root.ProjectTaskTypeName = taskType.Name;

                    root.TaskState = DocumentState.Edit;
                    root.TaskStateTime = serverTime;
                    root.OwnerGUID = ConstObject.LoginPersonInfo.Id;
                    root.OwnerName = ConstObject.LoginPersonInfo.Name;
                    root.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;
                    root.OrderNo = 0;

                    listAdd.Add(root);

                    model.SaveGWBSTreeRootNode(listAdd);

                    list = model.GetGWBSTreesByInstance(projectInfo.Id);
                }

                if (list != null && list.Count > 0)
                {
                    foreach (GWBSTree childNode in list)
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
                            tvwCategory.Nodes.Add(tnTmp);
                        }
                        hashtable.Add(tnTmp.Name, tnTmp);
                    }

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
                case MainViewState.AddNew:

                    this.mnuTree.Items[撤销.Name].Enabled = true;
                    this.mnuTree.Items[保存节点.Name].Enabled = true;
                    this.mnuTree.Items[提交任务.Name].Enabled = true;
                    this.mnuTree.Items[发布节点.Name].Enabled = false;
                    this.mnuTree.Items[作废节点.Name].Enabled = false;
                    this.mnuTree.Items[发布节点及其子节点.Name].Enabled = false;
                    this.mnuTree.Items[发布节点及其子节点.Name].Enabled = false;

                    this.mnuTree.Items[从PBS上拷贝节点.Name].Enabled = false;
                    this.mnuTree.Items[增加子节点.Name].Enabled = false;
                    this.mnuTree.Items[修改节点.Name].Enabled = false;
                    this.mnuTree.Items[删除节点.Name].Enabled = false;
                    this.mnuTree.Items[成本数据编辑.Name].Enabled = true;

                    btnSave.Enabled = true;
                    btnSelectWBSType.Enabled = true;
                    //btnPublish.Enabled = false;

                    //基本信息
                    this.txtTaskState.ReadOnly = true;
                    this.txtTaskCode.ReadOnly = true;
                    this.txtTaskName.ReadOnly = false;

                    this.btnRemovePBS.Enabled = true;
                    this.txtTaskWBSType.ReadOnly = false;
                    this.txtTaskDesc.ReadOnly = false;

                    this.txtOwner.ReadOnly = true;
                    this.txtStateTime.ReadOnly = true;

                    this.dtStartTime.Enabled = true;
                    this.dtEndTime.Enabled = true;

                    txtPriceUnit.ReadOnly = false;
                    btnSelPriceUnit.Enabled = true;

                    txtContractTotalPrice.ReadOnly = false;
                    txtResponsibilityTotalPrice.ReadOnly = false;
                    txtPlanTotalPrice.ReadOnly = false;

                    cbResponseAccount.Enabled = false;
                    cbCostAccount.Enabled = false;
                    cbProductConfirm.Enabled = false;
                    cbSubContractFee.Enabled = false;
                    cbWarehouseFlag.Enabled = true;

                    cbListCheckRequire.Enabled = true;

                    txtFigureProgress.ReadOnly = true;
                    txtCheckBatchNum.ReadOnly = false;

                    cbOverOrUnderGroundFlag.Enabled = true;
                    break;

                case MainViewState.Modify:

                    this.mnuTree.Items[撤销.Name].Enabled = true;
                    this.mnuTree.Items[保存节点.Name].Enabled = true;
                    this.mnuTree.Items[提交任务.Name].Enabled = true;
                    this.mnuTree.Items[增加子节点.Name].Enabled = false;
                    this.mnuTree.Items[修改节点.Name].Enabled = false;
                    this.mnuTree.Items[删除节点.Name].Enabled = false;
                    this.mnuTree.Items[成本数据编辑.Name].Enabled = true;

                    this.btnSave.Enabled = true;
                    btnSelectWBSType.Enabled = true;

                    //编制状态不允许发布作废和拷贝子节点
                    this.mnuTree.Items[从PBS上拷贝节点.Name].Enabled = false;
                    this.btnPublish.Enabled = false;
                    this.mnuTree.Items[发布节点.Name].Enabled = false;
                    this.mnuTree.Items[作废节点.Name].Enabled = false;
                    this.mnuTree.Items[发布节点及其子节点.Name].Enabled = false;
                    this.mnuTree.Items[发布节点及其子节点.Name].Enabled = false;

                    //基本信息
                    this.txtTaskState.ReadOnly = true;
                    this.txtTaskCode.ReadOnly = true;
                    this.txtTaskName.ReadOnly = false;

                    this.btnRemovePBS.Enabled = true;
                    this.txtTaskWBSType.ReadOnly = false;
                    this.txtTaskDesc.ReadOnly = false;

                    this.txtOwner.ReadOnly = true;
                    this.txtStateTime.ReadOnly = true;
                    this.dtStartTime.Enabled = true;
                    this.dtEndTime.Enabled = true;

                    txtPriceUnit.ReadOnly = false;
                    btnSelPriceUnit.Enabled = true;

                    txtContractTotalPrice.ReadOnly = false;
                    txtResponsibilityTotalPrice.ReadOnly = false;
                    txtPlanTotalPrice.ReadOnly = false;

                    cbResponseAccount.Enabled = false;
                    cbCostAccount.Enabled = false;
                    cbProductConfirm.Enabled = false;
                    cbSubContractFee.Enabled = false;
                    cbWarehouseFlag.Enabled = true;

                    cbListCheckRequire.Enabled = true;

                    txtFigureProgress.ReadOnly = true;
                    txtCheckBatchNum.ReadOnly = false;

                    cbOverOrUnderGroundFlag.Enabled = true;
                    break;

                case MainViewState.Browser:

                    this.mnuTree.Items[撤销.Name].Enabled = false;
                    this.mnuTree.Items[保存节点.Name].Enabled = false;
                    this.mnuTree.Items[增加子节点.Name].Enabled = true;
                    this.mnuTree.Items[修改节点.Name].Enabled = true;
                    this.mnuTree.Items[删除节点.Name].Enabled = true;
                    this.mnuTree.Items[成本数据编辑.Name].Enabled = true;

                    this.btnSave.Enabled = false;
                    btnSelectWBSType.Enabled = false;

                    if (oprNode != null && oprNode.TaskState == DocumentState.Edit)
                    {
                        this.mnuTree.Items[提交任务.Name].Enabled = true;
                    }
                    else
                    {
                        this.mnuTree.Items[提交任务.Name].Enabled = false;
                    }

                    if (oprNode != null && (oprNode.TaskState == DocumentState.Edit || oprNode.TaskState == DocumentState.InAudit))
                    {
                        this.btnPublish.Enabled = true;
                        this.mnuTree.Items[发布节点.Name].Enabled = true;
                        this.mnuTree.Items[发布节点及其子节点.Name].Enabled = true;
                    }
                    else
                    {
                        this.btnPublish.Enabled = false;
                        this.mnuTree.Items[发布节点.Name].Enabled = false;
                        this.mnuTree.Items[发布节点及其子节点.Name].Enabled = false;
                    }

                    if (oprNode != null && (oprNode.TaskState == DocumentState.InExecute))
                    {
                        this.mnuTree.Items[作废节点.Name].Enabled = true;
                        this.mnuTree.Items[作废节点及其子节点.Name].Enabled = true;
                    }
                    else
                    {
                        this.mnuTree.Items[作废节点.Name].Enabled = false;
                        this.mnuTree.Items[作废节点及其子节点.Name].Enabled = false;
                    }

                    //基本信息
                    this.txtCurrentPath.ReadOnly = true;
                    this.txtTaskState.ReadOnly = true;
                    this.txtTaskCode.ReadOnly = true;
                    this.txtTaskName.ReadOnly = true;

                    this.btnRemovePBS.Enabled = false;
                    this.txtTaskWBSType.ReadOnly = true;
                    this.txtTaskDesc.ReadOnly = true;

                    this.txtOwner.ReadOnly = true;
                    this.txtStateTime.ReadOnly = true;
                    this.dtStartTime.Enabled = false;
                    this.dtEndTime.Enabled = false;

                    txtPriceUnit.ReadOnly = true;
                    btnSelPriceUnit.Enabled = false;

                    txtContractTotalPrice.ReadOnly = true;
                    txtResponsibilityTotalPrice.ReadOnly = true;
                    txtPlanTotalPrice.ReadOnly = true;

                    cbResponseAccount.Enabled = false;
                    cbCostAccount.Enabled = false;
                    cbProductConfirm.Enabled = false;
                    cbSubContractFee.Enabled = false;
                    cbWarehouseFlag.Enabled = false;

                    cbListCheckRequire.Enabled = false;

                    txtFigureProgress.ReadOnly = true;
                    txtCheckBatchNum.ReadOnly = true;

                    cbOverOrUnderGroundFlag.Enabled = false;
                    break;

                case MainViewState.Initialize://添加根节点

                    this.mnuTree.Items[从PBS上拷贝节点.Name].Enabled = true;

                    this.mnuTree.Items[撤销.Name].Enabled = false;
                    this.mnuTree.Items[保存节点.Name].Enabled = false;
                    this.mnuTree.Items[提交任务.Name].Enabled = false;
                    this.mnuTree.Items[发布节点.Name].Enabled = false;
                    this.mnuTree.Items[发布节点及其子节点.Name].Enabled = false;
                    this.mnuTree.Items[作废节点.Name].Enabled = false;
                    this.mnuTree.Items[作废节点及其子节点.Name].Enabled = false;
                    this.mnuTree.Items[增加子节点.Name].Enabled = false;
                    this.mnuTree.Items[修改节点.Name].Enabled = false;
                    this.mnuTree.Items[删除节点.Name].Enabled = false;
                    this.mnuTree.Items[删除勾选节点.Name].Enabled = false;
                    this.mnuTree.Items[复制勾选节点.Name].Enabled = false;
                    this.mnuTree.Items[粘贴节点.Name].Enabled = false;

                    this.mnuTree.Items[插入同级节点.Name].Enabled = false;
                    this.mnuTree.Items[清除勾选节点.Name].Enabled = false;
                    this.mnuTree.Items[粘贴节点.Name].Enabled = false;
                    this.mnuTree.Items[成本数据编辑.Name].Enabled = false;

                    //基本信息
                    this.txtCurrentPath.ReadOnly = true;
                    this.txtTaskState.ReadOnly = true;
                    this.txtTaskCode.ReadOnly = true;
                    this.txtTaskName.ReadOnly = true;

                    this.btnRemovePBS.Enabled = false;
                    this.txtTaskWBSType.ReadOnly = true;
                    this.txtTaskDesc.ReadOnly = true;

                    this.txtOwner.ReadOnly = true;
                    this.txtStateTime.ReadOnly = true;
                    this.dtStartTime.Enabled = false;
                    this.dtEndTime.Enabled = false;

                    txtPriceUnit.ReadOnly = true;
                    btnSelPriceUnit.Enabled = false;

                    txtContractTotalPrice.ReadOnly = true;
                    txtResponsibilityTotalPrice.ReadOnly = true;
                    txtPlanTotalPrice.ReadOnly = true;

                    cbResponseAccount.Enabled = false;
                    cbCostAccount.Enabled = false;
                    cbProductConfirm.Enabled = false;
                    cbSubContractFee.Enabled = false;
                    cbWarehouseFlag.Enabled = false;

                    cbListCheckRequire.Enabled = false;

                    txtFigureProgress.ReadOnly = true;
                    txtCheckBatchNum.ReadOnly = true;

                    btnSave.Enabled = false;
                    btnSelectWBSType.Enabled = false;
                    btnPublish.Enabled = false;

                    cbOverOrUnderGroundFlag.Enabled = true;
                    break;
            }

            ViewState = state;
        }

        public override bool ModifyView()
        {
            if (修改节点.Enabled)
            {
                mnuTree_ItemClicked(修改节点, new ToolStripItemClickedEventArgs(修改节点));
                return true;
            }

            return false;
        }

        public override bool CancelView()
        {
            try
            {
                if (撤销.Enabled)
                {
                    mnuTree_ItemClicked(撤销, new ToolStripItemClickedEventArgs(撤销));
                    return true;
                }

                return false;
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
                listCheckedNode.Clear();
                listCopyNode.Clear();

                RefreshState(MainViewState.Browser);

                //LoadGWBSTreeTree();
                //LoadGWBSTree(null);
                LoadGWBSTree();
            }
            catch (Exception e)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(e));
            }
        }

        public override bool SaveView()
        {
            IList docVerifyList = new List<ProjectDocumentVerify>();
            bool isNew = false;
            try
            {
                if (!ValideSave())
                {
                    if (string.IsNullOrEmpty(oprNode.Id))//新增时如果没有校验成功需要清掉关联的PBS
                    {
                        oprNode.ListRelaPBS.Clear();
                    }
                    return false;
                }

                if (IsSubmit)
                {
                    oprNode.TaskState = DocumentState.InAudit;
                    oprNode.TaskStateTime = model.GetServerTime();
                }


                if (oprNode.Id == null)
                {
                    #region 生成工程文档验证***********************************************************************************************

                    docVerifyList = model.GetDocumentTemplatesByTaskType(oprNode);

                    #endregion
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

                        list = model.InsertOrUpdateWBSTrees(list, docVerifyList);

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
                        //lstInstance.Add(oprNode);
                    }
                    else
                    {
                        oprNode = model.SaveGWBSTree(oprNode, docVerifyList);

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
                        tn.Tag = oprNode;

                        this.tvwCategory.SelectedNode = tn;
                        tn.Expand();

                        //新增节点要有权限操作
                        //lstInstance.Add(oprNode);
                    }

                    //**********************************************************************************************************************
                }
                else
                {
                    this.tvwCategory.SelectedNode.Tag = oprNode;
                    this.tvwCategory.SelectedNode.Text = oprNode.Name.ToString();
                }

                List<PBSTree> listPBS = new List<PBSTree>();
                foreach (GWBSRelaPBS rela in oprNode.ListRelaPBS)
                {
                    listPBS.Add(rela.ThePBS);
                }
                cbRelaPBS.Tag = listPBS;

                RefreshControls(MainViewState.Browser);

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
                if ((e.Item as TreeNode).Tag == null)
                {
                    GWBSTree wbs = model.GetGWBSTreeById((e.Item as TreeNode).Name);
                    (e.Item as TreeNode).Tag = wbs;
                }
                GWBSTree org = (e.Item as TreeNode).Tag as GWBSTree;
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
                if (targetNode == null)// || !ConstMethod.Contains(lstInstance, targetNode.Tag as GWBSTree)
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

                            GWBSTree templateWBS = draggedNode.Tag as GWBSTree;

                            GWBSTree catTmp = new GWBSTree();
                            catTmp.Name = templateWBS.Name;

                            GWBSTree parentNode = targetNode.Tag as GWBSTree;
                            parentNode = LoadRelaAttribute(parentNode);

                            catTmp.ParentNode = parentNode;
                            catTmp.TheTree = parentNode.TheTree;
                            catTmp.OrderNo = model.GetMaxOrderNo(catTmp) + 1;

                            catTmp.OwnerGUID = ConstObject.LoginPersonInfo.Id;
                            catTmp.OwnerName = ConstObject.LoginPersonInfo.Name;
                            catTmp.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;

                            catTmp.ProjectTaskTypeGUID = parentNode.ProjectTaskTypeGUID;
                            catTmp.ProjectTaskTypeName = parentNode.ProjectTaskTypeName;
                            catTmp.CheckRequire = parentNode.ProjectTaskTypeGUID.CheckRequire.PadRight(11, 'X') + "0";
                            catTmp.DailyCheckState = catTmp.CheckRequire;
                            catTmp.OverOrUndergroundFlag = parentNode.OverOrUndergroundFlag;

                            foreach (GWBSRelaPBS rela in parentNode.ListRelaPBS)
                            {
                                GWBSRelaPBS newRela = new GWBSRelaPBS();
                                newRela.ThePBS = rela.ThePBS;
                                newRela.PBSName = rela.PBSName;

                                newRela.TheGWBSTree = catTmp;
                                catTmp.ListRelaPBS.Add(newRela);
                            }

                            //系统生成一个唯一编码
                            if (projectInfo != null)
                            {
                                catTmp.TheProjectGUID = projectInfo.Id;
                                catTmp.TheProjectName = projectInfo.Name;
                                //catTmp.Code = projectInfo.Code.PadLeft(4, '0') + "-" + model.GetCode(typeof(GWBSTree));//服务端生成
                            }

                            catTmp.TaskState = DocumentState.Edit;
                            //catTmp.TaskStateTime = model.GetServerTime();
                            catTmp.Summary = parentNode.Summary + "," + catTmp.Name;

                            catTmp.FullPath = targetNode.FullPath + @"\" + catTmp.Name;
                            catTmp.Level = catTmp.ParentNode.Level + 1;
                            catTmp.CategoryNodeType = NodeType.LeafNode;

                            IList lst = new ArrayList();
                            if (parentNode.CategoryNodeType != NodeType.RootNode)
                            {
                                parentNode.CategoryNodeType = NodeType.MiddleNode;
                            }
                            lst.Add(parentNode);
                            lst.Add(catTmp);
                            //克隆要复制的节点和其子节点的对象
                            PopulateListByCopyNode(draggedNode, lst, catTmp);

                            lst = model.SaveGWBSTrees1(lst);

                            //给复制节点的新父节点tag设值
                            targetNode.Tag = lst[0] as GWBSTree;
                            int i = 1;
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
                            GWBSTree draggedObj = draggedNode.Tag as GWBSTree;
                            draggedObj.OwnerGUID = ConstObject.LoginPersonInfo.Id;
                            draggedObj.OwnerName = ConstObject.LoginPersonInfo.Name;
                            draggedObj.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;

                            GWBSTree toObj = targetNode.Tag as GWBSTree;
                            Hashtable dic = model.MoveNode(draggedObj, toObj);
                            if (reset)
                            {
                                GWBSTree cat = null;
                                GWBSTree oldParentObj = oldParentNode.Tag as GWBSTree;
                                if (dic.ContainsKey(oldParentObj.Id))
                                    cat = dic[oldParentObj.Id] as GWBSTree;
                                else
                                    cat = model.GetGWBSTreeById(oldParentObj.Id) as GWBSTree;

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
                                result = model.GWBSTreeOrder(result);
                                ResetTagAfterOrder(draggedNode, result, 0);
                            }
                            else
                            {
                                GWBSTree fromOrg = draggedNode.Tag as GWBSTree;
                                GWBSTree toOrg = targetNode.Tag as GWBSTree;
                                fromOrg.OrderNo = toOrg.OrderNo - 1;
                                IList list = new ArrayList();
                                list.Add(fromOrg);
                                draggedNode.Tag = model.GWBSTreeOrder(list)[0];
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
                //uniqueCode = ConstMethod.GetNextCode(uniqueCode);
                //matCatTmp.Code = uniqueCode;

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
                    //matCatTmp.Code = projectInfo.Code.PadLeft(4, '0') + "-" + model.GetCode(typeof(GWBSTree));//服务端生成
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

                GWBSTree matCatTmp = new GWBSTree();
                matCatTmp.Name = templateWBS.Name;
                matCatTmp.OrderNo = templateWBS.OrderNo;//排序号用克隆节点的排序号

                matCatTmp.ParentNode = parent;
                matCatTmp.TheTree = parent.TheTree;

                matCatTmp.OwnerGUID = parent.OwnerGUID;
                matCatTmp.OwnerName = parent.OwnerName;
                matCatTmp.OwnerOrgSysCode = parent.OwnerOrgSysCode;
                matCatTmp.Author = parent.Author;

                matCatTmp.ProjectTaskTypeGUID = parent.ProjectTaskTypeGUID;
                matCatTmp.ProjectTaskTypeName = parent.ProjectTaskTypeName;
                matCatTmp.CheckRequire = parent.ProjectTaskTypeGUID.CheckRequire.PadRight(11, 'X') + "0";
                matCatTmp.DailyCheckState = matCatTmp.CheckRequire;
                matCatTmp.OverOrUndergroundFlag = parent.OverOrUndergroundFlag;

                foreach (GWBSRelaPBS rela in parent.ListRelaPBS)
                {
                    GWBSRelaPBS newRela = new GWBSRelaPBS();
                    newRela.ThePBS = rela.ThePBS;
                    newRela.PBSName = rela.PBSName;

                    newRela.TheGWBSTree = matCatTmp;
                    matCatTmp.ListRelaPBS.Add(newRela);
                }

                //系统生成一个唯一编码
                if (projectInfo != null)
                {
                    matCatTmp.TheProjectGUID = projectInfo.Id;
                    matCatTmp.TheProjectName = projectInfo.Name;
                    //matCatTmp.Code = projectInfo.Code.PadLeft(4, '0') + "-" + model.GetCode(typeof(GWBSTree));
                }

                matCatTmp.TaskState = DocumentState.Edit;
                //matCatTmp.TaskStateTime = model.GetServerTime();
                matCatTmp.Summary = parent.Summary + "," + matCatTmp.Name;

                matCatTmp.FullPath = ((GWBSTree)matCatTmp.ParentNode).FullPath + @"\" + matCatTmp.Name;
                matCatTmp.Level = matCatTmp.ParentNode.Level + 1;

                if (parent.CategoryNodeType != NodeType.RootNode)
                    parent.CategoryNodeType = NodeType.MiddleNode;
                matCatTmp.CategoryNodeType = NodeType.LeafNode;

                lst.Add(matCatTmp);

                PopulateListByCopyNode(var, lst, matCatTmp);
            }
        }
        private void PopulateListByCopyNode(TreeNode node, List<PBSTree> listPBS, IList lst, GWBSTree parent)
        {
            foreach (TreeNode taskTypeNode in node.Nodes)
            {
                if (taskTypeNode.Checked == false)
                    continue;

                ProjectTaskTypeTree task = taskTypeNode.Tag as ProjectTaskTypeTree;

                GWBSTree wbs = new GWBSTree();
                wbs.ProjectTaskTypeGUID = task;
                wbs.ProjectTaskTypeName = task.Name;
                wbs.CheckRequire = task.CheckRequire.PadRight(11, 'X') + "0";
                wbs.DailyCheckState = wbs.CheckRequire;

                foreach (PBSTree pbs in listPBS)
                {
                    GWBSRelaPBS relaPBS = new GWBSRelaPBS();
                    relaPBS.ThePBS = pbs;

                    relaPBS.TheGWBSTree = wbs;
                    wbs.ListRelaPBS.Add(relaPBS);

                    wbs.Name = pbs.Name + task.Name;
                }

                if (projectInfo != null)
                {
                    wbs.TheProjectGUID = projectInfo.Id;
                    wbs.TheProjectName = projectInfo.Name;
                    //wbs.Code = projectInfo.Code.PadLeft(4, '0') + "-" + model.GetCode(typeof(GWBSTree));//服务端生成
                }

                wbs.OrderNo = taskTypeNode.Index;

                wbs.TaskState = DocumentState.Edit;
                wbs.TaskStateTime = parent.TaskStateTime;
                wbs.OwnerGUID = ConstObject.LoginPersonInfo.Id;
                wbs.OwnerName = ConstObject.LoginPersonInfo.Name;
                wbs.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;
                wbs.Author = parent.Author;

                wbs.Summary = parent.Summary + "," + wbs.Name;

                wbs.ParentNode = parent;
                wbs.TheTree = parent.TheTree;
                wbs.Summary = parent.Summary + "," + wbs.Name;
                wbs.OverOrUndergroundFlag = parent.OverOrUndergroundFlag;

                wbs.FullPath = ((GWBSTree)wbs.ParentNode).FullPath + @"\" + wbs.Name;
                wbs.Level = wbs.ParentNode.Level + 1;
                if (parent.CategoryNodeType != NodeType.RootNode)
                    parent.CategoryNodeType = NodeType.MiddleNode;
                wbs.CategoryNodeType = NodeType.LeafNode;

                lst.Add(wbs);

                PopulateListByCopyNode(taskTypeNode, listPBS, lst, wbs);
            }
        }
        private void PopulateListByCopyNode(TreeNode node, List<ProjectTaskTypeTree> listTaskType, IList lst, GWBSTree parent)
        {
            foreach (TreeNode pbsNode in node.Nodes)
            {
                if (pbsNode.Checked == false)
                    continue;

                PBSTree pbs = pbsNode.Tag as PBSTree;
                GWBSTree wbs = new GWBSTree();

                GWBSRelaPBS relaPBS = new GWBSRelaPBS();
                relaPBS.ThePBS = pbs;
                relaPBS.TheGWBSTree = wbs;
                wbs.ListRelaPBS.Add(relaPBS);

                foreach (ProjectTaskTypeTree task in listTaskType)
                {
                    wbs.ProjectTaskTypeGUID = task;
                    wbs.ProjectTaskTypeName = task.Name;
                    wbs.CheckRequire = task.CheckRequire.PadRight(11, 'X') + "0";
                    wbs.DailyCheckState = wbs.CheckRequire;

                    wbs.Name = pbs.Name + task.Name;
                }

                if (projectInfo != null)
                {
                    wbs.TheProjectGUID = projectInfo.Id;
                    wbs.TheProjectName = projectInfo.Name;
                    //wbs.Code = projectInfo.Code.PadLeft(4, '0') + "-" + model.GetCode(typeof(GWBSTree));//服务端生成
                }

                wbs.OrderNo = pbsNode.Index;
                wbs.TaskState = DocumentState.Edit;
                wbs.TaskStateTime = parent.TaskStateTime;
                wbs.OwnerGUID = ConstObject.LoginPersonInfo.Id;
                wbs.OwnerName = ConstObject.LoginPersonInfo.Name;
                wbs.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;
                wbs.Author = parent.Author;

                wbs.ParentNode = parent;
                wbs.TheTree = parent.TheTree;
                wbs.Summary = parent.Summary + "," + wbs.Name;
                wbs.OverOrUndergroundFlag = parent.OverOrUndergroundFlag;

                wbs.FullPath = ((GWBSTree)wbs.ParentNode).FullPath + @"\" + wbs.Name;
                wbs.Level = wbs.ParentNode.Level + 1;

                if (parent.CategoryNodeType != NodeType.RootNode)
                    parent.CategoryNodeType = NodeType.MiddleNode;
                wbs.CategoryNodeType = NodeType.LeafNode;

                lst.Add(wbs);

                //克隆要复制的节点和其子节点的对象
                PopulateListByCopyNode(pbsNode, listTaskType, lst, wbs);
            }
        }

        private void tvwCategory_DragOver(object sender, DragEventArgs e)
        {
            //Point targetPoint = tvwCategory.PointToClient(new Point(e.X, e.Y));
            //tvwCategory.SelectedNode = tvwCategory.GetNodeAt(targetPoint);
        }

        #endregion

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
                oprNode = LoadRelaAttribute(oprNode);
                foreach (TreeNode node in listCopyNode)
                {
                    GWBSTree templateWBS = node.Tag as GWBSTree;
                    //templateWBS = LoadRelaAttribute(templateWBS);

                    GWBSTree catTmp = new GWBSTree();//继承当前节点的相关属性 //templateWBS.Clone();
                    catTmp.Name = templateWBS.Name;
                    catTmp.TaskState = DocumentState.Edit;

                    catTmp.OwnerGUID = ConstObject.LoginPersonInfo.Id;
                    catTmp.OwnerName = ConstObject.LoginPersonInfo.Name;
                    catTmp.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;
                    catTmp.ProjectTaskTypeGUID = oprNode.ProjectTaskTypeGUID;
                    catTmp.ProjectTaskTypeName = oprNode.ProjectTaskTypeName;

                    catTmp.CheckRequire = oprNode.ProjectTaskTypeGUID.CheckRequire.PadRight(11, 'X') + "0";
                    catTmp.DailyCheckState = catTmp.CheckRequire;
                    catTmp.Summary = oprNode.Summary + "," + catTmp.Name;
                    catTmp.OverOrUndergroundFlag = oprNode.OverOrUndergroundFlag;

                    bool flag = true;//是否为父子关系
                    foreach (GWBSRelaPBS rela in templateWBS.ListRelaPBS)
                    {
                        var query = from pbs in oprNode.ListRelaPBS
                                    where rela.ThePBS.SysCode.Contains(pbs.ThePBS.SysCode)
                                    select pbs;
                        if (query.Count() == 0)
                        {
                            flag = false;
                        }
                    }
                    if (!flag)
                    {
                        foreach (GWBSRelaPBS rela in oprNode.ListRelaPBS)
                        {
                            GWBSRelaPBS newRela = new GWBSRelaPBS();
                            newRela.ThePBS = rela.ThePBS;
                            newRela.PBSName = rela.PBSName;

                            newRela.TheGWBSTree = catTmp;
                            catTmp.ListRelaPBS.Add(newRela);
                        }
                    }
                    else
                    {
                        foreach (GWBSRelaPBS rela in templateWBS.ListRelaPBS)
                        {
                            GWBSRelaPBS newRela = new GWBSRelaPBS();
                            newRela.ThePBS = rela.ThePBS;
                            newRela.PBSName = rela.PBSName;

                            newRela.TheGWBSTree = catTmp;
                            catTmp.ListRelaPBS.Add(newRela);
                        }
                    }

                    if (projectInfo != null)
                    {
                        catTmp.TheProjectGUID = projectInfo.Id;
                        catTmp.TheProjectName = projectInfo.Name;
                        //catTmp.Code = projectInfo.Code.PadLeft(4, '0') + "-" + model.GetCode(typeof(GWBSTree));
                    }

                    //GWBSTree parentNode = oprNode;
                    catTmp.ParentNode = oprNode;
                    catTmp.TheTree = oprNode.TheTree;
                    catTmp.OrderNo = model.GetMaxOrderNo(catTmp) + 1;

                    catTmp.FullPath = ((GWBSTree)catTmp.ParentNode).FullPath + @"\" + catTmp.Name;
                    catTmp.Level = catTmp.ParentNode.Level + 1;
                    catTmp.CategoryNodeType = NodeType.LeafNode;

                    lst.Add(catTmp);

                    GetCopyNode(node, catTmp, ref lst);
                }

                if (oprNode.CategoryNodeType != NodeType.RootNode)
                    oprNode.CategoryNodeType = NodeType.MiddleNode;

                lst.Insert(0, oprNode);

                //保存复制的节点
                lst = model.SaveGWBSTrees1(lst);
                //新增节点要有权限操作
                //(lstInstance as ArrayList).AddRange(lst);
                //给复制节点的新父节点tag设值
                oprNode = lst[0] as GWBSTree;
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
                    //templateWBS = LoadRelaAttribute(templateWBS);

                    GWBSTree catTmp = new GWBSTree(); //templateWBS.Clone();
                    catTmp.Name = templateWBS.Name;
                    catTmp.OrderNo = templateWBS.OrderNo;//排序号就用复制节点的排序号

                    catTmp.ParentNode = saveParentNode;
                    catTmp.TheTree = saveParentNode.TheTree;

                    catTmp.OwnerGUID = saveParentNode.OwnerGUID;
                    catTmp.OwnerName = saveParentNode.OwnerName;
                    catTmp.OwnerOrgSysCode = saveParentNode.OwnerOrgSysCode;
                    catTmp.ProjectTaskTypeGUID = saveParentNode.ProjectTaskTypeGUID;
                    catTmp.ProjectTaskTypeName = saveParentNode.ProjectTaskTypeName;

                    catTmp.CheckRequire = saveParentNode.ProjectTaskTypeGUID.CheckRequire.PadRight(11, 'X') + "0";
                    catTmp.DailyCheckState = catTmp.CheckRequire;
                    catTmp.Summary = saveParentNode.Summary + "," + catTmp.Name;
                    catTmp.OverOrUndergroundFlag = saveParentNode.OverOrUndergroundFlag;

                    //foreach (GWBSRelaPBS rela in oprNode.ListRelaPBS)
                    //{
                    //    GWBSRelaPBS newRela = new GWBSRelaPBS();
                    //    newRela.ThePBS = rela.ThePBS;
                    //    newRela.PBSName = rela.PBSName;

                    //    newRela.TheGWBSTree = catTmp;
                    //    catTmp.ListRelaPBS.Add(newRela);
                    //}

                    bool flag = true;//是否为父子关系
                    foreach (GWBSRelaPBS rela in templateWBS.ListRelaPBS)
                    {
                        var query = from pbs in oprNode.ListRelaPBS
                                    where rela.ThePBS.SysCode.Contains(pbs.ThePBS.SysCode)
                                    select pbs;
                        if (query.Count() == 0)
                        {
                            flag = false;
                        }
                    }
                    if (!flag)
                    {
                        foreach (GWBSRelaPBS rela in oprNode.ListRelaPBS)
                        {
                            GWBSRelaPBS newRela = new GWBSRelaPBS();
                            newRela.ThePBS = rela.ThePBS;
                            newRela.PBSName = rela.PBSName;

                            newRela.TheGWBSTree = catTmp;
                            catTmp.ListRelaPBS.Add(newRela);
                        }
                    }
                    else
                    {
                        foreach (GWBSRelaPBS rela in templateWBS.ListRelaPBS)
                        {
                            GWBSRelaPBS newRela = new GWBSRelaPBS();
                            newRela.ThePBS = rela.ThePBS;
                            newRela.PBSName = rela.PBSName;

                            newRela.TheGWBSTree = catTmp;
                            catTmp.ListRelaPBS.Add(newRela);
                        }
                    }

                    if (projectInfo != null)
                    {
                        catTmp.TheProjectGUID = projectInfo.Id;
                        catTmp.TheProjectName = projectInfo.Name;
                        //catTmp.Code = projectInfo.Code.PadLeft(4, '0') + "-" + model.GetCode(typeof(GWBSTree));
                    }

                    catTmp.Author = saveParentNode.Author;

                    catTmp.FullPath = ((GWBSTree)catTmp.ParentNode).FullPath + @"\" + catTmp.Name;
                    catTmp.Level = catTmp.ParentNode.Level + 1;
                    if (saveParentNode.CategoryNodeType != NodeType.RootNode)
                        saveParentNode.CategoryNodeType = NodeType.MiddleNode;
                    catTmp.CategoryNodeType = NodeType.LeafNode;

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
                IList listFilter = new ArrayList();//过滤之后
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
                if (MessageBox.Show(text, "删除节点", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                string message = "";
                foreach (GWBSTree w in list)
                {
                    if (model.GetGWBSDetailLikeWBSSysCodeSql(w.SysCode) == 0)
                    {
                        listFilter.Add(w);
                    }
                    else
                    {
                        message += "节点【" + w.Name + "】或其子节点，";
                    }
                }

                if (model.DeleteGWBSTree(listFilter))//删除成功
                {
                    if (message != "")
                    {
                        MessageBox.Show(message + "上有发布状态的任务明细，不能删除！");
                    }
                    //从WBS树上移除对应的节点
                    foreach (GWBSTree pbs in listFilter)
                    {
                        //删除日志
                        LogData log = new LogData();
                        log.ProjectID = pbs.TheProjectGUID;
                        log.BillId = pbs.Id;
                        log.BillType = "工程任务";
                        log.Code = "任务名称：" + pbs.Name;
                        log.OperType = "删除";
                        log.Descript = "工程任务删除记录";
                        log.OperPerson = ConstObject.LoginPersonInfo.Name;
                        log.ProjectName = pbs.TheProjectName;
                        StaticMethod.InsertLogData(log);

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
                string message = ex.Message;
                Exception ex1 = ex.InnerException;
                while (ex1 != null && !string.IsNullOrEmpty(ex1.Message))
                {
                    message += ex1.Message;

                    ex1 = ex1.InnerException;
                }

                if (message.IndexOf("违反") > -1 && message.IndexOf("约束") > -1)
                {
                    MessageBox.Show("勾选节点中有节点被其它数据所引用，删除前请先删除引用的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
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

        //导出Excel
        void btnExportExcel_Click(object sender, EventArgs e)
        {

        }

        //发布
        void btnPublish_Click(object sender, EventArgs e)
        {
            mnuTree.Hide();
            if (oprNode != null && string.IsNullOrEmpty(oprNode.Id))
            {
                if (SaveView() == false)
                    return;
            }
            PublisthNode();
            this.RefreshControls(MainViewState.Browser);
        }

        //保存
        void btnSave_Click(object sender, EventArgs e)
        {
            IsSubmit = false;
            mnuTree.Hide();
            if (!SaveView()) return;
            this.RefreshControls(MainViewState.Browser);
        }

        //选择工程任务类型
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
                if (parentTreeNode != null)
                {
                    parentNode = parentTreeNode.Tag as GWBSTree;
                    parentNode = LoadRelaAttribute(parentNode);
                    parentTreeNode.Tag = parentNode;
                }

            }
            else
            {
                parentTreeNode = currNode;
                parentNode = parentTreeNode.Tag as GWBSTree;
                parentNode = LoadRelaAttribute(parentNode);
                parentTreeNode.Tag = parentNode;
            }
            if (parentNode != null)
            {
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
            }

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
                type.FullPath = GetFullPath(type);
                txtTaskWBSType.Text = type.FullPath;
                txtTaskWBSType.Tag = type;

                if (oprNode != null && string.IsNullOrEmpty(oprNode.Id))
                {
                    oprNode.CheckRequire = type.CheckRequire.PadRight(11, 'X') + "0";
                    oprNode.DailyCheckState = oprNode.CheckRequire;

                    //检查要求
                    if (!string.IsNullOrEmpty(oprNode.CheckRequire))
                    {
                        char[] chs = oprNode.CheckRequire.ToCharArray();
                        for (int i = 0; i < chs.Length; i++)
                        {
                            Char c = chs[i];
                            if (c == '0')
                            {
                                if (cbListCheckRequire.Items.Count > i)
                                    cbListCheckRequire.SetItemChecked(i, true);
                            }
                            else
                            {
                                if (cbListCheckRequire.Items.Count > i)
                                    cbListCheckRequire.SetItemChecked(i, false);
                            }
                        }
                    }
                }

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
                for (int i = 0; i < listPBS.Count; i++)
                {
                    listPBS[i].FullPath = GetFullPath(listPBS[i]);
                    cbRelaPBS.Items.Add(listPBS[i]);
                }

                cbRelaPBS.DisplayMember = "FullPath";
                cbRelaPBS.ValueMember = "Id";
                cbRelaPBS.Tag = listPBS;

                cbRelaPBS.SelectedIndex = 0;

                //RefreshControls(MainViewState.Browser);
            }
        }


        void txtPriceUnit_LostFocus(object sender, EventArgs e)
        {
            txtPriceUnit.LostFocus -= new EventHandler(txtPriceUnit_LostFocus);
            SetStandardUnit(sender);
            txtPriceUnit.LostFocus += new EventHandler(txtPriceUnit_LostFocus);
        }

        //选择价格计量单位
        void btnSelPriceUnit_Click(object sender, EventArgs e)
        {
            SelectUnit(txtPriceUnit);
        }

        private void SetStandardUnit(object sender)
        {
            TextBox tbUnit = sender as TextBox;
            string name = tbUnit.Text.Trim();
            if (name != "")
            {
                if (tbUnit.Tag == null || (tbUnit.Tag != null && (tbUnit.Tag as StandardUnit).Name != name))
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Name", name));
                    IList list = model.ObjectQuery(typeof(StandardUnit), oq);
                    if (list.Count > 0)
                    {
                        tbUnit.Tag = list[0] as StandardUnit;
                    }
                    else
                    {
                        MessageBox.Show("系统目前不存在该计量单位，请检查！");
                        SelectUnit(tbUnit);
                    }
                }
            }
            else
                tbUnit.Tag = null;
        }
        private void SelectUnit(TextBox txt)
        {
            StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
            if (su != null)
            {
                txt.Tag = su;
                txt.Text = su.Name;
                txt.Focus();
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

        #region SQL查询加载
        private void LoadGWBSTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();
                DataSet dataSet = model.GetGWBSTreesByInstanceSql(projectInfo.Id);
                DataTable table = dataSet.Tables[0];
                IList list = new ArrayList();
                foreach (DataRow dataRow in table.Rows)
                {
                    GWBSTree wbs = new GWBSTree();
                    wbs.Id = dataRow["Id"].ToString();
                    wbs.Name = dataRow["Name"].ToString();
                    wbs.SysCode = dataRow["SysCode"].ToString();
                    wbs.OrderNo = ClientUtil.ToLong(dataRow["orderNo"]);
                    wbs.ParentNode = new GWBSTree();
                    wbs.ParentNode.Id = dataRow["parentnodeid"].ToString();
                    wbs.SpecialType = dataRow["specialtype"].ToString();
                    list.Add(wbs);
                }
                if (list == null || list.Count == 0)
                {
                    PBSTree pbs = null;
                    ProjectTaskTypeTree taskType = null;

                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
                    oq.AddCriterion(Expression.IsNull("ParentNode"));
                    oq.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
                    IList listPBS = model.ObjectQuery(typeof(PBSTree), oq);

                    if (listPBS == null || listPBS.Count == 0)
                        return;
                    pbs = listPBS[0] as PBSTree;


                    oq.Criterions.Clear();
                    oq.AddCriterion(Expression.IsNull("ParentNode"));
                    oq.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
                    IList listTaskType = model.ObjectQuery(typeof(ProjectTaskTypeTree), oq);

                    if (listTaskType == null || listTaskType.Count == 0)
                        return;
                    taskType = listTaskType[0] as ProjectTaskTypeTree;


                    IList listAdd = new ArrayList();

                    GWBSTree root = new GWBSTree();

                    DateTime serverTime = model.GetServerTime();

                    root.TheProjectGUID = projectInfo.Id;
                    root.TheProjectName = projectInfo.Name;
                    root.Name = projectInfo.Name;
                    root.FullPath = root.Name;

                    root.Code = projectInfo.Code.PadLeft(4, '0') + "-" + model.GetCode(typeof(GWBSTree));
                    root.CheckRequire = string.IsNullOrEmpty(taskType.CheckRequire) ? ("X".PadRight(11, 'X') + "0") : (taskType.CheckRequire.PadRight(11, 'X') + "0");

                    GWBSRelaPBS relaPBS = new GWBSRelaPBS();
                    relaPBS.ThePBS = pbs;

                    relaPBS.TheGWBSTree = root;
                    root.ListRelaPBS.Add(relaPBS);

                    root.ProjectTaskTypeGUID = taskType;
                    root.ProjectTaskTypeName = taskType.Name;

                    root.TaskState = DocumentState.Edit;
                    root.TaskStateTime = serverTime;
                    root.OwnerGUID = ConstObject.LoginPersonInfo.Id;
                    root.OwnerName = ConstObject.LoginPersonInfo.Name;
                    root.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;
                    root.OrderNo = 0;
                    root.CategoryNodeType = NodeType.RootNode;
                    root.Level = 1;
                    root.Summary = root.Name;

                    listAdd.Add(root);

                    model.SaveGWBSTreeRootNode(listAdd);

                    list = model.GetGWBSTreesByInstance(projectInfo.Id);
                }

                if (list != null && list.Count > 0)
                {
                    foreach (GWBSTree childNode in list)
                    {
                        if (childNode.State == 0)
                            continue;

                        TreeNode tnTmp = new TreeNode();
                        tnTmp.Name = childNode.Id.ToString();
                        tnTmp.Text = childNode.Name;
                        tnTmp.Tag = childNode;
                        if (childNode.ParentNode != null && !string.IsNullOrEmpty(childNode.ParentNode.Id))
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

                    this.tvwCategory.SelectedNode = this.tvwCategory.Nodes[0];
                    this.tvwCategory.SelectedNode.Expand();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        #endregion
    }
}
