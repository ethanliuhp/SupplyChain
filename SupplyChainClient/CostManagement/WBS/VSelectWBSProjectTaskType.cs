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


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS
{
    public partial class VSelectWBSProjectTaskType : TBasicDataView
    {
        public List<TreeNode> SelectResult = new List<TreeNode>();

        private ProjectTaskTypeTree oprNode = null;

        /// <summary>
        ///缺省选择的任务类型节点
        /// </summary>
        public ProjectTaskTypeTree DefaultSelectedTaskType = null;

        public MWBSManagement model;

        public VSelectWBSProjectTaskType(MWBSManagement mot)
        {
            model = mot;
            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();

            RefreshState(MainViewState.Browser);

            tvwCategory.CheckBoxes = false;

            LoadProjectTaskTypeTreeTree();
        }

        private void InitEvents()
        {
            tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);

            btnEnter.Click += new EventHandler(btnEnter_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            this.Load += new EventHandler(VSelectWBSProjectTaskType_Load);
            this.FormClosing += new FormClosingEventHandler(VSelectWBSProjectTaskType_FormClosing);
        }

        void VSelectWBSProjectTaskType_Load(object sender, EventArgs e)
        {
            InitialSelectedTaskType();
        }

        private void InitialSelectedTaskType()
        {
            if (DefaultSelectedTaskType != null)
            {
                //初始化打开父节点关联的PBS位置
                foreach (TreeNode tn in tvwCategory.Nodes)
                {
                    if (tn.Name == DefaultSelectedTaskType.Id)
                    {
                        tvwCategory.SelectedNode = tn;

                        TreeNode theParentNode = tn.Parent;
                        while (theParentNode != null)
                        {
                            theParentNode.Expand();
                            theParentNode = theParentNode.Parent;
                        }
                        break;
                    }
                    if (SetDefaultSelectedNode(tvwCategory, tn, DefaultSelectedTaskType.Id))
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


        public bool isOK = false;
        void btnEnter_Click(object sender, EventArgs e)
        {
            if (tvwCategory.SelectedNode == null)
            {
                MessageBox.Show("请选择一个节点", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TreeNode selectNode = tvwCategory.SelectedNode;
            if (selectNode.Nodes.Count > 0)
                selectNode.Nodes.Clear();

            SelectResult.Add(selectNode);
            isOK = true;
            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            SelectResult.Clear();
            this.Close();
        }

        void VSelectWBSProjectTaskType_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isOK)
                SelectResult.Clear();
        }

        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                oprNode = tvwCategory.SelectedNode.Tag as ProjectTaskTypeTree;
                this.GetNodeDetail();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
            }
        }

        private void LoadProjectTaskTypeTreeTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();
                CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                IList list = model.GetProjectTaskTypeByInstance();
                //IList lstInstance = listAll[1] as IList;
                //IList list = listAll[0] as IList;
                foreach (ProjectTaskTypeTree childNode in list)
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
                if (list.Count > 0)
                {
                    this.tvwCategory.SelectedNode = this.tvwCategory.Nodes[0];
                    this.tvwCategory.SelectedNode.Expand();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("查询工程任务类型出错：" + ExceptionUtil.ExceptionMessage(e));
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

                this.txtLevel.Text = oprNode.TypeLevel.ToString();
                this.txtStandard.Text = oprNode.TypeStandard.ToString();
                this.txtCheckRequire.Text = oprNode.CheckRequire;
                this.txtSummary.Text = oprNode.TypeSummary;
                this.txtDesc.Text = oprNode.Summary;

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
            this.txtLevel.Text = "";
            this.txtStandard.Text = "";
            this.txtCheckRequire.Text = "";
            this.txtSummary.Text = "";
            this.txtDesc.Text = "";
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

    }
}
