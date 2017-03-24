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
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppSolutionSetUI;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.HumanResource;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.OBS
{
    public partial class VOBSMng : TBasicDataView
    {
        private GWBSTree oprNode = null;
        public MGWBSTree model;
        private MOperationJob operationmodel;
        private MPersonOnJob personmodel;
        public MProjectDepartment projectmodel;
        public MOperationOrg org;
        public MOBS OBSModel = new MOBS();
        public MPerson perModel;

        private TreeNode currNode = null;
        private TreeNode orgName = null;
        private OperationOrg oprOrg = null;

        CurrentProjectInfo projectInfo;
        Hashtable ht = new Hashtable();
        bool ergodic = false;
        bool isSelectNodeInvoke = false;//是否在选择(点击)节点时调用
        private Dictionary<string, TreeNode> listCheckedNode = new Dictionary<string, TreeNode>();

        private IList lstInstance;
        private IList list;//数据库中存在的管理OBS信息
        private IList listPerson;//数据库中存在的管理OBS人员信息

        private OBSPerson curBillPerson;
        /// <summary>
        /// 管理OBS人员单据
        /// </summary>
        public OBSPerson CurBillPerson
        {
            get { return curBillPerson; }
            set { curBillPerson = value; }
        }

        private OBSManage curBillManage;
        /// <summary>
        /// 管理OBS单据
        /// </summary>
        public OBSManage CurBillManage
        {
            get { return curBillManage; }
            set { curBillManage = value; }
        }

        public VOBSMng(MGWBSTree mot, MPerson mp, MOperationJob ope, MPersonOnJob per, MProjectDepartment mod, MOperationOrg morg)
        {
            model = mot;
            perModel = mp;
            operationmodel = ope;
            personmodel = per;
            org = morg;
            projectmodel = mod;
            InitializeComponent();
            InitData();
            InitBegion();
            InitForm();
        }
        private void InitData()
        {
            tvwOperation.CheckBoxes = false;
            txtStates.Items.AddRange(new object[] { "编辑", "执行", "作废" });
            projectInfo = StaticMethod.GetProjectInfo();
            oprOrg = projectInfo.OwnerOrg;
        }
        private void InitBegion()
        {
            tvwGWBS.AfterCheck += new TreeViewEventHandler(tvwGWBS_AfterCheck);
            tvwGWBS.AfterExpand += new TreeViewEventHandler(tvwGWBS_AfterExpand);
            tvwOperation.AfterSelect += new TreeViewEventHandler(tvwOperation_AfterSelect);
            tvwOperation.MouseDown += new MouseEventHandler(tvwOperation_MouseDown);
            btnSave.Click += new EventHandler(btnSave_Click);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
        }

        void tvwOperation_MouseDown(object sender, MouseEventArgs e)
        {
            if (tvwOperation.SelectedNode != null)
            {
                ht.Clear();
                TreeNode tnode = tvwGWBS.SelectedNode;
                TreeNode node = checkRoot(tnode);//查找根节点
                NodeUnChecked(node);//所有节点设置为未选中
                oprOrg = tvwOperation.SelectedNode.Tag as OperationOrg;
                //LoadPerson();
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("OrpJob", oprOrg));
                oq.AddFetchMode("OperOrgInfo", NHibernate.FetchMode.Eager);
                list = OBSModel.OBSSrv.GetOBSManage(oq);
                if (list.Count > 0)
                {
                    foreach (OBSManage manage in list)
                    {
                        string strId = manage.ProjectTask.Id;
                        treenode(node, strId);
                        txtStates.SelectedItem = ClientUtil.ToString(manage.MngState);
                        dtStartTime.Value = manage.BeginDate;
                        dtEndTime.Value = manage.EndDate;
                    }
                }
            }
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)//当单击复选框，同时处于组合编辑状态时 
            {
                foreach (DataGridViewRow row in this.dgDetail.Rows)
                {
                    if (row.Cells[1].Value != null && Convert.ToBoolean(row.Cells[1].Value) == true)
                    {
                        row.Cells[colSelect.Name].Value = "false";
                    }
                }
                dgDetail[colSelect.Name, e.RowIndex].Value = "true";
                TreeNode tnode = tvwGWBS.SelectedNode;
                TreeNode node = checkRoot(tnode);//查找根节点
                NodeUnChecked(node);//所有节点设置为未选中
                string _selectValue = dgDetail.Rows[e.RowIndex].Cells["colSelect"].EditedFormattedValue.ToString();
                if (_selectValue == "True")
                {
                    //获得当前行的人员姓名
                    string strPersonName = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colName.Name].Value);
                    OperationRole role = dgDetail.CurrentRow.Cells[colRole.Name].Tag as OperationRole;
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("OrpJob", oprOrg));
                    oq.AddCriterion(Expression.Eq("PersonName", strPersonName));
                    oq.AddCriterion(Expression.Eq("PersonRole", role));
                    oq.AddFetchMode("OperOrgInfo", NHibernate.FetchMode.Eager);
                    listPerson = OBSModel.OBSSrv.GetOBSPerson(oq);
                    if (listPerson.Count > 0)
                    {
                        foreach (OBSPerson person in listPerson)
                        {
                            string strId = person.ProjectTask.Id;
                            treenode(node, strId);
                        }
                    }
                }
            }
        }

        private void InitForm()
        {
            //LoadGWBSTreeTree();
            LoadGWBSTreeTree(null);
            LoadProject();
            //LoadPerson();
        }
        //选择GWBS树
        void tvwGWBS_AfterCheck(object sender, TreeViewEventArgs e)
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
            SetChildNodeChecked(e.Node);
        }
        //将点击中的节点的子节点的选择状态设置成点击节点的状态
        private void SetChildChecked(TreeNode parentNode)
        {
            tvwGWBS.SelectedNode = parentNode;
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
        //将选中的节点保存到hashtable中
        private void SetChildNodeChecked(TreeNode parentNode)
        {
            if (parentNode.Tag == null) return;
            GWBSTree treeHT = parentNode.Tag as GWBSTree;
            if (parentNode.Checked)
            {
                if (ht.Count == 0)
                {
                    ht.Add(treeHT, treeHT.Name);
                }
                else
                {
                    if (!ht.Contains(treeHT))
                    {
                        bool flag = false;
                        foreach (System.Collections.DictionaryEntry objHT in ht)
                        {
                            //TreeNode node = objHT.Key as TreeNode;
                            GWBSTree tree = objHT.Key as GWBSTree;
                            if (tree.SysCode.Contains(treeHT.SysCode))
                            {
                                ht.Remove(tree);
                                ht.Add(treeHT, treeHT.Name);
                                flag = true;
                                break;
                            }
                            else
                            {
                                if (treeHT.SysCode.Contains(tree.SysCode))
                                {
                                    flag = true;
                                    break;
                                }
                            }
                        }
                        if (!flag)
                        {
                            ht.Add(treeHT, treeHT.Name);
                        }
                    }
                }
            }
            else
            {
                if (ht != null && ht.Count > 0)
                {
                    if (ht.Contains(treeHT))
                    {
                        ht.Remove(treeHT);
                    }
                }
            }
            TreeNode nodes = parentNode;
            TreeNode tnode = checkRoot(nodes);
            foreach (TreeNode childNode in parentNode.Nodes)
            {
                if (parentNode.Checked)
                {
                    treenode(childNode, (tnode.Tag as GWBSTree).Id);
                }
                childNode.Checked = parentNode.Checked;
                treenode(childNode, (parentNode.Tag as GWBSTree).Id);
                //SetChildNodeChecked(childNode);
            }
        }
        //递归撤销树节点的选中状态
        private void NodeUnChecked(TreeNode parentNode)
        {
            parentNode.Checked = false;
            foreach (TreeNode node in parentNode.Nodes)
            {
                node.Checked = false;
                NodeUnChecked(node);
            }
        }
        //递归设置树节点的选中状态
        private void treenode(TreeNode nodes, string id)
        {
            if (nodes.Tag != null)
            {
                if (id == (nodes.Tag as GWBSTree).Id)
                {
                    nodes.Checked = true;
                }
            }
            foreach (TreeNode node in nodes.Nodes)
            {
                if (node.Tag != null)
                {
                    if ((node.Tag as GWBSTree).Id == id)
                    {
                        node.Checked = true;
                    }
                }
                treenode(node, id);
            }
        }

        //选择业务组织树
        void tvwOperation_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                ht.Clear();
                TreeNode tnode = tvwGWBS.SelectedNode;
                TreeNode node = checkRoot(tnode);//查找根节点
                NodeUnChecked(node);//所有节点设置为未选中
                oprOrg = tvwOperation.SelectedNode.Tag as OperationOrg;
                //LoadPerson();
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("OrpJob", oprOrg));
                oq.AddFetchMode("OperOrgInfo", NHibernate.FetchMode.Eager);
                list = OBSModel.OBSSrv.GetOBSManage(oq);
                if (list.Count > 0)
                {
                    foreach (OBSManage manage in list)
                    {
                        string strId = manage.ProjectTask.Id;
                        treenode(node, strId);
                        txtStates.SelectedItem = ClientUtil.ToString(manage.MngState);
                        dtStartTime.Value = manage.BeginDate;
                        dtEndTime.Value = manage.EndDate;
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ViewModle()) return;
                IList listManages = new ArrayList();
                IList listPersons = new ArrayList();
                bool flag = false;
                foreach (System.Collections.DictionaryEntry obj in ht)
                {
                    //TreeNode node = obj.Key as TreeNode;
                    GWBSTree node = obj.Key as GWBSTree;
                    OperationOrg org = new OperationOrg();
                    foreach (DataGridViewRow var in this.dgDetail.Rows)
                    {
                        if (ClientUtil.ToBool(var.Cells[colSelect.Name].Value))
                        {
                            //人员
                            OBSPerson curBillPerson = new OBSPerson();
                            ObjectQuery oq = new ObjectQuery();
                            oq.AddCriterion(Expression.Eq("OrpJob", oprOrg));
                            oq.AddCriterion(Expression.Eq("PersonName", var.Cells[colName.Name].Value));
                            oq.AddCriterion(Expression.Eq("PersonRole", var.Cells[colRole.Name].Tag as OperationRole));
                            oq.AddCriterion(Expression.Eq("ProjectTask", node));
                            IList list = OBSModel.OBSSrv.GetOBSPerson(oq);
                            if (list.Count > 0)
                            {
                                curBillPerson = list[0] as OBSPerson;
                            }
                            curBillPerson.PersonStates = ClientUtil.ToString(txtStates.Text);
                            curBillPerson.BeginDate = dtStartTime.Value;
                            curBillPerson.EndDate = dtEndTime.Value;
                            curBillPerson.HandlePerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;//负责人
                            curBillPerson.HandlePersonName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;//负责人名称
                            curBillPerson.CreatePerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;//制单人编号
                            curBillPerson.CreatePersonName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;//制单人名称
                            curBillPerson.CreateDate = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginDate;//制单时间
                            curBillPerson.CreateYear = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginDate.Year;//制单年
                            curBillPerson.CreateMonth = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginDate.Month;//制单月

                            curBillPerson.OperOrgInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheLogin.TheOperationOrgInfo;//业务组织
                            curBillPerson.OperOrgInfoName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheLogin.TheOperationOrgInfo.Name;//业务组织名称
                            curBillPerson.OpgSysCode = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheLogin.TheOperationOrgInfo.SysCode;//业务组织编号

                            curBillPerson.ProjectId = projectInfo.Id;
                            curBillPerson.ProjectName = projectInfo.Name;
                            curBillPerson.RoleName = ClientUtil.ToString(var.Cells[colRole.Name].Value);
                            curBillPerson.PersonRole = var.Cells[colRole.Name].Tag as OperationRole;
                            curBillPerson.PersonName = ClientUtil.ToString(var.Cells[colName.Name].Value);
                            curBillPerson.ManagePerson = var.Cells[colName.Name].Tag as StandardPerson;
                            curBillPerson.ProjectTask = node;
                            curBillPerson.ProjectTaskName = node.Name;
                            curBillPerson.ProjectTaskSysCode = node.SysCode;
                            curBillPerson.OrpJob = oprOrg as OperationOrg;
                            curBillPerson.OrgJobName = (oprOrg as OperationOrg).Name;
                            curBillPerson.OrgJobSysCode = (oprOrg as OperationOrg).SysCode;
                            //保存信息
                            listPersons.Add(curBillPerson);
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        //项目
                        curBillManage = new OBSManage();
                        ObjectQuery objectQuery = new ObjectQuery();
                        objectQuery.AddCriterion(Expression.Eq("ProjectTask", node));
                        objectQuery.AddCriterion(Expression.Eq("OrpJob", oprOrg as OperationOrg));
                        IList lists = OBSModel.OBSSrv.GetOBSManage(objectQuery);
                        if (lists.Count > 0)
                        {
                            curBillManage = lists[0] as OBSManage;
                        }
                        curBillManage.MngState = ClientUtil.ToString(txtStates.Text);
                        curBillManage.BeginDate = dtStartTime.Value;
                        curBillManage.EndDate = dtEndTime.Value;
                        curBillManage.HandlePerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;//负责人
                        curBillManage.HandlePersonName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;//负责人名称
                        curBillManage.CreatePerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;//制单人编号
                        curBillManage.CreatePersonName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;//制单人名称
                        curBillManage.CreateDate = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginDate;//制单时间
                        curBillManage.CreateYear = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginDate.Year;//制单年
                        curBillManage.CreateMonth = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginDate.Month;//制单月
                        curBillManage.OperOrgInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheLogin.TheOperationOrgInfo;//登录业务组织
                        curBillManage.OperOrgInfoName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheLogin.TheOperationOrgInfo.Name;//业务组织名称
                        curBillManage.OpgSysCode = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheLogin.TheOperationOrgInfo.SysCode;//业务组织编号
                        curBillManage.ProjectId = projectInfo.Id;
                        curBillManage.ProjectName = projectInfo.Name;
                        curBillManage.ProjectTask = node;
                        curBillManage.ProjectTaskName = node.Name;
                        curBillManage.ProjectTaskSysCode = node.SysCode;
                        curBillManage.OrpJob = oprOrg as OperationOrg;
                        curBillManage.OrgJobName = (oprOrg as OperationOrg).Name;
                        curBillManage.OrgJobSysCode = (oprOrg as OperationOrg).SysCode;
                        listManages.Add(curBillManage);
                    }
                }
                bool f = false;
                if (ht.Count == 0)
                {
                    OBSModel.OBSSrv.SaveOBSManages(listManages, oprOrg, ht);
                    f = true;
                }
                if (listManages.Count > 0)
                {
                    //OBSModel.OBSSrv.GetOperationOrgs
                    OBSModel.OBSSrv.SaveOBSManages(listManages, oprOrg, ht);
                    //OBSModel.OBSSrv.SaveOBSManage(listManages[0] as OBSManage);
                    f = true;
                }
                if (listPersons.Count > 0)
                {
                    OBSModel.OBSSrv.SavePersons(listPersons, oprOrg, ht);
                    f = true;
                }
                if (f)
                {
                    MessageBox.Show("保存成功！");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(err));
            }
        }

        bool ViewModle()
        {
            if (tvwOperation.SelectedNode.Tag == null)
            {
                MessageBox.Show("请选择业务组织！");
                return false;
            }
            //if (ClientUtil.ToDateTime(dtStartTime.Value) >= ClientUtil.ToDateTime(dtEndTime.Value))
            //{
            //    MessageBox.Show("开始时间不可晚于结束时间！");
            //    return false;
            //}
            //currNode = tvwGWBS.SelectedNode;
            //checkRoot(currNode);
            //ergodic = false;
            //ErgodicTree(currNode);
            //if (!ergodic)
            //{
            //if (ht.Count == 0)
            //{
            //    MessageBox.Show("请选择GWBS树节点！");
            //    return false;
            //}
            //if (txtStates.Text == "")
            //{
            //    MessageBox.Show("请选择状态信息！");
            //    return false;
            //}
            return true;
        }
        //查找根节点
        private TreeNode checkRoot(TreeNode node)
        {
            if (node == null) return node;
            if (node.Parent != null)
            {
                node = node.Parent;
                node = checkRoot(node);
            }
            return node;
        }

        private void ErgodicTree(TreeNode currNode)
        {
            if (currNode.Nodes.Count == 0)
            {
                if (currNode.Checked)
                {
                    ergodic = true;
                }
            }
            foreach (TreeNode node in currNode.Nodes)
            {
                if (node.Checked)
                {
                    ergodic = true;
                }
                else
                {
                    ErgodicTree(node);
                }
            }
        }

        //GWBS树
        void LoadGWBSTreeTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwGWBS.Nodes.Clear();
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                IList list = model.GetGWBSTreesByInstance(projectInfo.Id);
                foreach (GWBSTree childNode in list)
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
                        {
                            tnp.Nodes.Add(tnTmp);
                        }
                    }
                    else
                    {
                        tvwGWBS.Nodes.Add(tnTmp);
                    }
                    hashtable.Add(tnTmp.Name, tnTmp);
                }
                if (list.Count > 0)
                {
                    this.tvwGWBS.SelectedNode = this.tvwGWBS.Nodes[0];
                    this.tvwGWBS.SelectedNode.Expand();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("查询业务组织出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }
        //GWBS树 分层加载
        void LoadGWBSTreeTree(TreeNode node)
        {
            try
            {
                ObjectQuery oq = new ObjectQuery();
                int level = 1;
                string sysCode = string.Empty;
                if (node != null)
                {
                    GWBSTree selectWBS = node.Tag as GWBSTree;
                    level = selectWBS.Level + 1;
                    sysCode = selectWBS.SysCode;
                    node.Nodes.Clear();
                }
                oq.AddCriterion(Expression.Eq("Level", level));
                oq.AddCriterion(Expression.Like("SysCode", sysCode + "%"));
                oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
                oq.AddCriterion(Expression.Eq("State", 1));
                oq.AddOrder(Order.Asc("Level"));
                oq.AddOrder(Order.Asc("OrderNo"));

                IList lst = model.ObjectQuery(typeof(GWBSTree), oq);

                Hashtable hashtable = new Hashtable();

                foreach (GWBSTree childNode in lst)
                {
                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = childNode.Name;
                    tnTmp.Tag = childNode;

                    if (childNode.CategoryNodeType != VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode)
                    {
                        tnTmp.Nodes.Add("test");
                    }

                    if (node != null)
                    {
                        node.Nodes.Add(tnTmp);
                    }
                    else
                    {
                        tvwGWBS.Nodes.Add(tnTmp);
                    }
                }
                if (lst.Count > 0)
                {
                    this.tvwGWBS.SelectedNode = this.tvwGWBS.Nodes[0];
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        void tvwGWBS_AfterExpand(object sender, TreeViewEventArgs e)
        {
            LoadGWBSTreeTree(e.Node);
            if (list != null && list.Count > 0)
            {
                foreach (OBSManage manage in list)
                {
                    string strId = manage.ProjectTask.Id;
                    treenode(e.Node, strId);
                    txtStates.SelectedItem = ClientUtil.ToString(manage.MngState);
                    dtStartTime.Value = manage.BeginDate;
                    dtEndTime.Value = manage.EndDate;
                }
            }
        }

        //人员角色
        void LoadPerson()
        {
            //查询用户岗位中岗位归属当前选中项目的信息
            //PersonOnJob人员上岗
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("OperationJob.OperationOrg", oprOrg));
            IList listAll = OBSModel.OBSSrv.GetPersonjob(oq);
            //查询岗位角色中岗位归属当前选中项目的信息
            //OperationJobWithRole岗位关联角色
            ObjectQuery oqy = new ObjectQuery();
            oqy.AddCriterion(Expression.Eq("OperationJob.OperationOrg", oprOrg));
            IList listJob = OBSModel.OBSSrv.GetJobRole(oqy);
            if (listAll.Count <= 0) return;
            dgDetail.Rows.Clear();
            foreach (PersonOnJob PJob in listAll)
            {
                foreach (OperationJobWithRole jobRole in listJob)
                {
                    if (jobRole.OperationJobName == PJob.OperationJob.Name)
                    {
                        int i = dgDetail.Rows.Add();
                        dgDetail[colName.Name, i].Value = PJob.StandardPerson.Name;
                        dgDetail[colName.Name, i].Tag = PJob.StandardPerson;
                        dgDetail[colRole.Name, i].Value = jobRole.OperationRoleName;
                        dgDetail[colRole.Name, i].Tag = jobRole.OperationRole;
                    }
                }
            }
        }
        //业务组织
        void LoadProject()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwOperation.Nodes.Clear();
                string strCode = "";
                //if (Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheLogin.TheAccountOrgInfo == null)
                //{
                //    strCode = this.projectInfo.OwnerOrgSysCode;
                //}
                //else
                //{
                //    strCode = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheLogin.TheAccountOrgInfo.SysCode;
                //}
                strCode = this.projectInfo.OwnerOrgSysCode;
                ObjectQuery oq = new ObjectQuery();
                //oq.AddCriterion(Expression.Sql(" instr('" + strCode + "',{alias}.OPGSYSCODE)>0"));
                oq.AddCriterion(Expression.Like("SysCode", strCode, MatchMode.Start));
                //oq.AddCriterion(Expression.Eq("IsAccountOrg", true));
                oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
                IList listAll = OBSModel.OBSSrv.GetOperationOrgs(oq);
                if (listAll.Count == 0) return;

                foreach (OperationOrg childNode in listAll)
                {
                    if (childNode.State == 0)
                        continue;
                    //2013-06-24
                    if (childNode.IsAccountOrg == false && childNode.Id != this.projectInfo.OwnerOrg.Id)
                        continue;

                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = childNode.Name;
                    tnTmp.Tag = childNode;
                    if (childNode.Id == this.projectInfo.OwnerOrg.Id)
                    {
                        tvwOperation.Nodes.Add(tnTmp);
                    }
                    else
                    {
                        if (childNode.ParentNode != null)
                        {
                            TreeNode tnp = null;
                            tnp = hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                            if (tnp != null)
                                tnp.Nodes.Add(tnTmp);
                        }
                    }
                    hashtable.Add(tnTmp.Name, tnTmp);
                }
                //if (listAll.Count > 0)
                //{
                //    this.tvwOperation.SelectedNode = this.tvwOperation.Nodes[0];
                //    this.tvwOperation.SelectedNode.Expand();
                //}
            }
            catch (Exception e)
            {
                MessageBox.Show("查询业务组织出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }
    }
}