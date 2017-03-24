using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Core;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using VirtualMachine.Component.WinMVC.generic;
using NHibernate;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;

namespace Application.Business.Erp.SupplyChain.Client.PeriodRequirePlan
{
    public partial class VProjectTaskOption : TBasicDataView
    {
        private MPeriodRequirePlanMng model = new MPeriodRequirePlanMng();
        private ResourceRequirePlan resPlan = new ResourceRequirePlan();
        private ResourceRequireReceipt resReceipt = new ResourceRequireReceipt();
        private OperationOrgInfo org = new OperationOrgInfo();
        private WeekScheduleMaster schedule = new WeekScheduleMaster();

        private MGWBSTree modelGWBS = new MGWBSTree();
        TreeNode parentNode = new TreeNode();
        private GWBSTree optGWBS = null;

        CurrentProjectInfo projectInfo = null;

        Hashtable ht = new Hashtable();
        bool isSelectNodeInvoke = false;//是否在选择(点击)节点时调用
        private Dictionary<string, TreeNode> listCheckedNode = new Dictionary<string, TreeNode>();

        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public Hashtable Ht
        {
            get { return ht; }
            set { ht = value; }
        }

        public VProjectTaskOption(OperationOrgInfo org, WeekScheduleMaster schedule, ResourceRequirePlan resPlan)
        {
            InitializeComponent();
            this.org = org;
            this.schedule = schedule;
            this.resPlan = resPlan;
            InitData();
            InitEvent();
        }

        public void InitData()
        {
            if (org != null)
            {
                this.txtOperatioOrgName.Tag = org;
                this.txtOperatioOrgName.Text = org.Name;
            }
            if (schedule != null)
            {
                this.txtSchediledPlan.Tag = schedule;
                this.txtSchediledPlan.Text = schedule.PlanName;
            }
            projectInfo = StaticMethod.GetProjectInfo();

            LoadGWBSTree(null);
            tvwGWBS.SelectedNode = null;
        }

        public void InitEvent()
        {
            this.btnGiveUp.Click += new EventHandler(btnGiveUp_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.tvwGWBS.AfterCheck += new TreeViewEventHandler(tvwGWBS_AfterCheck);
            this.lnkAll.Click += new EventHandler(lnkAll_Click);
            this.lnkNone.Click += new EventHandler(lnkNone_Click);
            tvwGWBS.BeforeExpand += new TreeViewCancelEventHandler(tvwGWBS_BeforeExpand);
        }

        void tvwGWBS_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                if (e.Node != null)
                {
                    LoadGWBSTree(e.Node);
                }
            }
            catch
            {
            }
        }

        void btnGiveUp_Click(object sender, EventArgs e)
        {
            ht.Clear();
            this.btnGiveUp.FindForm().Close();
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkAll_Click(object sender, EventArgs e)
        {
            //parentNode.Checked = true;
            foreach (TreeNode node in tvwGWBS.Nodes)
            {
                node.Checked = true;
            }
        }

        /// <summary>
        /// 反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkNone_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in tvwGWBS.Nodes)
            {
                node.Checked = false;
            }
        }

        void tvwGWBS_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (isSelectNodeInvoke)
            {
                isSelectNodeInvoke = false;
            }
            else
            {
                if ((Control.ModifierKeys & Keys.Shift) != 0 || (Control.ModifierKeys & Keys.Control) != 0)
                {
                    if (e.Node.Checked)
                    {
                        if (listCheckedNode.ContainsKey(e.Node.Name))
                            listCheckedNode[e.Node.Name] = e.Node;
                        else
                            listCheckedNode.Add(e.Node.Name, e.Node);
                    }
                    else
                    {
                        if (listCheckedNode.ContainsKey(e.Node.Name))
                            listCheckedNode.Remove(e.Node.Name);
                    }
                }
                else
                {
                    if (e.Node.Checked)
                    {
                        if (listCheckedNode.ContainsKey(e.Node.Name))
                            listCheckedNode[e.Node.Name] = e.Node;
                        else
                            listCheckedNode.Add(e.Node.Name, e.Node);
                    }
                    else
                    {
                        if (listCheckedNode.ContainsKey(e.Node.Name))
                            listCheckedNode.Remove(e.Node.Name);
                    }
                }
            }
            RefreshControls(MainViewState.Check);
            SetChildNodeChecked(e.Node);
        }

        //将选中的节点保存到hashtable中
        private void SetChildNodeChecked(TreeNode parentNode)
        {
            if (parentNode.Checked)
            {
                if (!ht.Contains(parentNode))
                {
                    ht.Add(parentNode, parentNode.Name);
                }
            }
            else
            {
                if (ht.Contains(parentNode))
                {
                    ht.Remove(parentNode);
                }
            }
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
            FlashScreen.Show("正在生成期间需求计划单明细......");
        }

        public void LoadGWBSTree()
        {
            try
            {
                ObjectQuery oq = new ObjectQuery();

                oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
                oq.AddCriterion(Expression.Eq("CategoryNodeType", VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.RootNode)); 
                IList gwbslist = model.ResourceRequirePlanSrv.ObjectQuery(typeof(GWBSTree), oq);
                if (gwbslist.Count == 0) return;

                GWBSTree rootNode= gwbslist[0] as GWBSTree;

                TreeNode tn  = new TreeNode();
                tn.Name = rootNode.Id;
                tn.Text = rootNode.Name;
                tn.Tag = rootNode;
                    tvwGWBS.Nodes.Add(tn);


                if (schedule != null)
                {

                }
                else
                {
                    LoadGWBSTree(tn);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }
        /// <summary>
        /// 加载当前节点的子节点
        /// </summary>
        /// <param name="oNode"></param>
        private void LoadGWBSTree(TreeNode oNode)
        {
            try
            {

                int iLevel = 1;
                string sSysCode = string.Empty;
                if (oNode != null)
                {
                    GWBSTree oTree = oNode.Tag as GWBSTree;
                    iLevel = oTree.Level + 1;
                    sSysCode = oTree.SysCode;
                    oNode.Nodes.Clear();
                }
                else
                {
                    sSysCode = resPlan.TheGWBSTreeGUID.SysCode;
                    iLevel = resPlan.TheGWBSTreeGUID.Level;
                }

                IList list = modelGWBS.GetGWBSTreesByInstance(projectInfo.Id, sSysCode, iLevel);

                foreach (GWBSTree childNode in list)
                {
                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = childNode.Name;
                    tnTmp.Tag = childNode;
                    if (childNode.CategoryNodeType != NodeType.LeafNode)//不为叶节点 就添加一个空节点
                    {
                        tnTmp.Nodes.Add("Test");
                    }
                    if (oNode != null)
                    {
                        oNode.Nodes.Add(tnTmp);

                    }
                    else
                    {
                        tvwGWBS.Nodes.Add(tnTmp);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }
    }
}
