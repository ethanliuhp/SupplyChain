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
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using System.Linq;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng
{
    public partial class VSelectCostAccountSubject : TBasicDataView
    {
        CurrentProjectInfo oProjectInfo = null;
        /// <summary>当前属于非项目时 可以设置当前项目 </summary>
        public CurrentProjectInfo ProjectInfo
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
        private CostAccountSubject _SelectAccountSubject = null;
        private bool _IsLeafSelect = true;//是否控制为只能选择叶节点
        private bool isSubBalanceFlag = false;
        /// <summary>
        /// 是否是选择叶节点
        /// </summary>
        public bool IsLeafSelect
        {
            get { return _IsLeafSelect; }
            set { _IsLeafSelect = value; }
        }
        /// <summary>
        /// 选择的核算科目
        /// </summary>
        public CostAccountSubject SelectAccountSubject
        {
            get { return _SelectAccountSubject; }
            set { _SelectAccountSubject = value; }
        }
        /// <summary>
        /// 分包结算标志 是否必须纳入结算
        /// </summary>
        public bool IsSubBalanceFlag
        {
            get { return isSubBalanceFlag; }
            set
            {
                isSubBalanceFlag = value;
            }
        }
        public bool DisplayCheckBox
        {
            get { return this.tvwCategory.CheckBoxes; }
            set { this.tvwCategory.CheckBoxes = value; }
        }
        /// <summary>
        /// 获取路径
        /// </summary>
        public string Path
        {
            get { return txtCurrentPath.Text; }
        }
        //有权限的业务组织
        private IList lstInstance;

        /// <summary>
        ///缺省选择的核算科目
        /// </summary>
        public CostAccountSubject DefaultSelectedAccountSubject = null;

        public MCostAccountSubject model;

        public VSelectCostAccountSubject(MCostAccountSubject mot)
        {
            model = mot;

            InitializeComponent();

            InitForm();
        }

        /// <summary>
        /// 指定的核算科目根节点根节点Code
        /// </summary>
        private string SpecifyRootNodeSubJectCode = string.Empty;
        public VSelectCostAccountSubject(string rootSubJectCode)
        {
            model = new MCostAccountSubject();
            SpecifyRootNodeSubJectCode = rootSubJectCode;
            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();

            tvwCategory.CheckBoxes = false;

            LoadCostAccountSubjectTree();


        }

        private void InitEvents()
        {
            tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);

            btnEnter.Click += new EventHandler(btnEnter_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            this.Load += new EventHandler(VSelectCostAccountSubject_Load);
            this.FormClosing += new FormClosingEventHandler(VSelectCostAccountSubject_FormClosing);

        }



        void VSelectCostAccountSubject_Load(object sender, EventArgs e)
        {
            InitialSelectedCostAccSubject();
        }

        private void InitialSelectedCostAccSubject()
        {
            if (DefaultSelectedAccountSubject != null)
            {
                //初始化打开父节点关联的PBS位置
                foreach (TreeNode tn in tvwCategory.Nodes)
                {
                    if (tn.Name == DefaultSelectedAccountSubject.Id)
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
                    if (SetDefaultSelectedNode(tvwCategory, tn, DefaultSelectedAccountSubject.Id))
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
            if (SelectAccountSubject == null)
            {
                MessageBox.Show("请选择一个核算科目！");
                return;
            }
            if (IsLeafSelect == true)
            {
                if (SelectAccountSubject.CategoryNodeType != NodeType.LeafNode)
                {
                    MessageBox.Show("核算科目必须选择末级节点！");
                    return;
                }
            }
            if (IsSubBalanceFlag && SelectAccountSubject.IfSubBalanceFlag == 2)
            {
                MessageBox.Show("请选择[分包结算标志]为[必须纳入结算]的核算科目节点"); return;
            }
            isOK = true;
            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            SelectAccountSubject = null;
            this.Close();
        }

        void VSelectCostAccountSubject_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isOK)
                SelectAccountSubject = null;
        }

        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                SelectAccountSubject = tvwCategory.SelectedNode.Tag as CostAccountSubject;
                this.GetNodeDetail();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
            }
        }

        private void ClearAll()
        {
            this.txtCurrentPath.Text = "";
            this.txtCode.Text = "";
            this.txtName.Text = "";

            txtOwner.Text = "";
            txtOwner.Tag = "";

            this.txtDesc.Text = "";
            this.txtSummary.Text = "";
            this.txtState.Text = "";
        }

        private void GetNodeDetail()
        {
            try
            {
                ClearAll();

                this.txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath;
                this.txtName.Text = SelectAccountSubject.Name;
                this.txtCode.Text = SelectAccountSubject.Code;
                this.txtOwner.Text = SelectAccountSubject.OwnerName;
                this.txtOwner.Tag = SelectAccountSubject.OwnerGUID;
                if (SelectAccountSubject.SubjectState != 0)
                    this.txtState.Text = SelectAccountSubject.SubjectState.ToString();
                else
                    this.txtState.Text = CostAccountSubjectState.制定.ToString();
                this.txtDesc.Text = SelectAccountSubject.Describe;
                this.txtSummary.Text = SelectAccountSubject.Summary;

                RefreshControls(MainViewState.Browser);
            }
            catch (Exception exp)
            {
                MessageBox.Show("显示出错：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        private void LoadCostAccountSubjectTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();
                CurrentProjectInfo projectInfo = ProjectInfo;// Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                var list = model.GetCostAccountSubjectByInstance().OfType<CostAccountSubject>().ToList();

                if (!string.IsNullOrEmpty(SpecifyRootNodeSubJectCode))
                {
                    var subject = list.FirstOrDefault(p => p.Code == SpecifyRootNodeSubJectCode);
                    list = list.Where(p => p.SysCode.Contains(subject.Id)).ToList();
                }

                //lstInstance = listAll[1] as IList;
                //IList list = listAll[0] as IList;
                foreach (CostAccountSubject childNode in list)
                {
                    //if (childNode.State == 0)
                    //    continue;

                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = childNode.Name;
                    tnTmp.Tag = childNode;
                    if (childNode.ParentNode != null && childNode.Code != SpecifyRootNodeSubJectCode)
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

                    break;

                case MainViewState.Browser:

                    txtCode.Enabled = false;
                    txtName.Enabled = false;
                    txtOwner.Enabled = false;
                    txtDesc.Enabled = false;
                    txtSummary.Enabled = false;

                    txtState.Enabled = false;
                    break;
            }
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

    }
}
