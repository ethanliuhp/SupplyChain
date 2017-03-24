using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using NHibernate.Criterion;
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
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Core;
using ConstObject = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.ConstObject;


namespace Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng
{

    public partial class VOperationOrgAsProject : TBasicDataView
    {
        IList lstProjectState = null;
        private TreeNode tnCurrNode;
        private MProjectDepartment model = new MProjectDepartment();
        private OperationOrg oprOrg = null;
        private Hashtable hashtableMent = new Hashtable();
        MOperationOrg mot = new MOperationOrg();
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();
        private bool isNew = true;
        //有权限的业务组织
        private IList lstInstance;
        private Hashtable hashtableRules = new Hashtable();

        public VOperationOrgAsProject()
        {
            InitializeComponent();
            InitForm();
            RefreshState(MainViewState.Browser);
            LoadOperationOrgTree();
        }

        private void InitForm()
        {
            this.cbIsAccountOrg.Items.Add("是");
            this.cbIsAccountOrg.Items.Add("否");
           
            VBasicDataOptr.InitProjectConstractStage(this.txtConstractStage, false);
            //IList stateList = new ArrayList();
            //stateList.Add("结算");
            //stateList.Add("停工");
            //stateList.Add("无效");
            //this.cboProState.DataSource = stateList;
            this.cboProState.Items.Clear();
            foreach (string sName in Enum.GetNames(typeof(EnumProjectCurrState)))
            {
                this.cboProState.Items.Add(sName);
            }
            VBasicDataOptr.InitProjectType(txtProjectType, false);

            this.cmbIfInProject.Items.Add("否");
            this.cmbIfInProject.Items.Add("是");

            this.InitEvents();
        }

        private void InitEvents()
        {
            this.tvwCategory.NodeMouseClick += new TreeNodeMouseClickEventHandler(tvwCategory_NodeMouseClick);
            tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);
            this.btnSaveProject.Click += new EventHandler(btnSaveProject_Click);
        }

        void btnSaveProject_Click(object sender, EventArgs e)
        {
            if (ClientUtil.ToString(projectInfo.Id) == "" || ClientUtil.ToString(projectInfo.Code) == "")
            {
                throw new Exception("此业务组织无项目相关信息!");
            }
            projectInfo.Name = txtProjectName.Text;
            projectInfo.ManagerDepart = txtProName.Text;
            projectInfo.ConstractStage = txtConstractStage.Text;
            if (this.cmbIfInProject.Text == "是")
            {
                projectInfo.IfSync = 1;
            }
            else
            {
                projectInfo.IfSync = 0;
            }
            projectInfo.ProjectType = txtProjectType.SelectedIndex;
            projectInfo.ProjectCurrState =(int)Enum.Parse(typeof(EnumProjectCurrState ), cboProState.SelectedItem.ToString());
            projectInfo.CreateDate = txtCreateDate.Value;
            model.CurrentSrv.SaveCurrentProjectInfo(projectInfo);
            MessageBox.Show("保存成功！");
        }


        private void LoadOperationOrgTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();
                //IList list = mOperationOrgTree.GetOperationOrgs(typeof(OperationOrg));
                IList listAll = mot.GetOpeOrgsByInstance();
                lstInstance = listAll[1] as IList;
                IList list = listAll[0] as IList;
                foreach (OperationOrg childNode in list)
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
                MessageBox.Show("查询业务组织出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                oprOrg = tvwCategory.SelectedNode.Tag as OperationOrg;
                hashtableMent = new Hashtable();
                string opgid = oprOrg.Id;
                ObjectQuery oq = new ObjectQuery();
                if (opgid != null && opgid != "")
                {
                    oq.AddCriterion(Expression.Eq("OwnerOrg.Id", opgid));
                    IList projectLst = model.CurrentSrv.GetCurrentProjectInfo(oq);
                    if (projectLst != null && projectLst.Count > 0)
                    {
                        projectInfo = projectLst[0] as CurrentProjectInfo;
                    }
                    else {
                        projectInfo = new CurrentProjectInfo();
                    }
                }               

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
                //业务组织信息
                this.txtName.Text = oprOrg.Name;
                this.txtCode.Text = oprOrg.Code;
                this.cbIsAccountOrg.Text = oprOrg.IsAccountOrg ? "是" : "否";
                this.txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath;
                this.txtPlace.Text = oprOrg.Place;

                //项目信息
                txtProjectName.Text = projectInfo.Name;
                this.txtProjectCode.Text = projectInfo.Code;
                txtProName.Text = projectInfo.ManagerDepart;
                txtConstractStage.Text = projectInfo.ConstractStage;
                txtProjectType.SelectedIndex = projectInfo.ProjectType;//工程类型
                if (projectInfo.IfSync == 1)
                {
                    this.cmbIfInProject.Text = "是";
                }
                else
                {
                    this.cmbIfInProject.Text = "否";
                }
                if (projectInfo.CreateDate > ClientUtil.ToDateTime("2000-1-1"))
                {
                    txtCreateDate.Value = ClientUtil.ToDateTime(projectInfo.CreateDate);//进场日期
                }
                else {
                    txtCreateDate.Value = DateTime.Now;//进场日期
                }
                this.cboProState.SelectedIndex = projectInfo.ProjectCurrState;//工程项目执行状态
            }
            catch (Exception exp)
            {
                MessageBox.Show("显示出错：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }
        private void ClearAll()
        {
            this.txtCurrentPath.Text = "";
            this.txtName.Text = "";
            this.txtCode.Text = "";
            this.cbIsAccountOrg.Text = "否";
            this.txtPlace.Text = "";
        }

        private bool ValideSave()
        {

            try
            {
                if (oprOrg == null)
                    oprOrg = new OperationOrg();
                if (txtName.Text.Trim() == "")
                    throw new Exception("名称不能为空!");
                if (txtCode.Text.Trim() == "")
                    throw new Exception("编码不能为空!");
                oprOrg.Name = txtName.Text.Trim();
                oprOrg.Code = txtCode.Text.Trim();
                oprOrg.IsAccountOrg = cbIsAccountOrg.Text.Trim() == "是";
                oprOrg.AuthorCode = ConstObject.LoginPersonInfo.Code;
                oprOrg.Place = txtPlace.Text.Trim ();

                return true;

            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
                return false;
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
                LoadOperationOrgTree();
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
                LoadOperationOrgTree();
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
                if (oprOrg.Id == null)
                    isNew = true;
                else
                    isNew = false;
                oprOrg = mot.SaveOperationOrg(oprOrg);

                hashtableMent.Clear();
                if (isNew)
                {
                    //要添加子节点的节点以前没有子节点，需要重新设置Tag
                    if (tvwCategory.SelectedNode.Nodes.Count == 0)
                        tvwCategory.SelectedNode.Tag = oprOrg.ParentNode;
                    TreeNode tn = this.tvwCategory.SelectedNode.Nodes.Add(oprOrg.Id.ToString(), oprOrg.Name.ToString());
                    //新增节点要有权限操作
                    lstInstance.Add(oprOrg);
                    tn.Tag = oprOrg;
                    this.tvwCategory.SelectedNode = tn;
                    tn.Expand();
                }
                else
                {
                    this.tvwCategory.SelectedNode.Text = oprOrg.Name.ToString();
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

        void tvwCategory_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
           
        }

        public void ReloadTreeNode()
        {

            hashtableMent.Clear();
            if (isNew)
            {
                //要添加子节点的节点以前没有子节点，需要重新设置Tag
                if (tvwCategory.SelectedNode.Nodes.Count == 0)
                    tvwCategory.SelectedNode.Tag = oprOrg.ParentNode;
                TreeNode tn = this.tvwCategory.SelectedNode.Nodes.Add(oprOrg.Id.ToString(), oprOrg.Name.ToString());
                //新增节点要有权限操作
                lstInstance.Add(oprOrg);
                tn.Tag = oprOrg;
                this.tvwCategory.SelectedNode = tn;
                tn.Expand();
            }
            else
            {
                this.tvwCategory.SelectedNode.Text = oprOrg.Name.ToString();
            }
        }
     
    }
}
