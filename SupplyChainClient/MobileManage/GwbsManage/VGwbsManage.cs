using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Windows.Media.Media3D;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.GwbsManage
{
    public partial class VGwbsManage : TBasicToolBarByMobile
    {
        public MGWBSTree model = new MGWBSTree();
        private AutomaticSize automaticSize = new AutomaticSize();
        CurrentProjectInfo projectInfo = null;
        public VGwbsManage()
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            InitialEvent();
        }
        private void InitialEvent()
        {
            searchBtn.Click += new EventHandler(searchBtn_Click);
            detailBtns.Click += new EventHandler(detailBtns_Click);
            detailBtn.Click += new EventHandler(detailBtn_Click);
            searchTxt.TextChanged += new EventHandler(searchTxt_TextChanged);

            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
        }

        void searchTxt_TextChanged(object sender, EventArgs e)
        {
            if (searchTxt.Text.Trim() == "")
            {
                gridTask.Rows.Clear();
            }
        }

        void searchBtn_Click(object sender, EventArgs e)
        {

            string text = searchTxt.Text;

            this.tvwCategory.Visible = false;
            this.gridTask.Visible = true;

            if (text == "")
            {
                MessageBox.Show("搜索条件不可以为空，请检查！");
                return;
            }




            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
            oq.AddCriterion(Expression.Like("Summary", text, MatchMode.Anywhere));
            IList list = model.ObjectQuery(typeof(GWBSTree), oq);
            if (list.Count == 0)
            {
                MessageBox.Show("您所搜索的内容不存在，请检查！");
            }

            GWBSTree gwbs = new GWBSTree();
            GWBSDetail dtl = new GWBSDetail();
            //dtl.TheGWBS.Id = "";


            gridTask.Rows.Clear();
            foreach (GWBSTree item in list)
            {
                int rowIndex = gridTask.Rows.Add();
                DataGridViewRow row = gridTask.Rows[rowIndex];
                row.Tag = item;
                row.Cells[colSummary.Name].Value = item.Summary;
                row.Cells[colTaskName.Name].Value = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), item);
            }



        }

        void treeBtn_Click(object sender, EventArgs e)
        {


            tvwCategory.CheckBoxes = false;
            if (tvwCategory.Visible == true && tvwCategory.SelectedNode != null)
            {

            }
            else if (gridTask.Visible == true && gridTask.SelectedRows.Count > 0)
            {
                GWBSTree gwbs = gridTask.SelectedRows[0].Tag as GWBSTree;

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
                oq.AddCriterion(Expression.Like("SysCode", gwbs.SysCode, MatchMode.Start));
                oq.AddCriterion(Expression.Eq("State", 1));
                oq.AddOrder(Order.Asc("Level"));
                oq.AddOrder(Order.Asc("OrderNo"));

                IList list = model.ObjectQuery(typeof(GWBSTree), oq);
                LoadGWBSTreeTree(list, gwbs);
            }
            else
            {
                LoadGWBSTreeTree();
            }
            this.gridTask.Visible = false;
            this.tvwCategory.Visible = true;
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
                MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        private void LoadGWBSTreeTree(IList list, GWBSTree gwbs)
        {

            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();

                foreach (GWBSTree childNode in list)
                {
                    if (childNode.State == 0)
                        continue;

                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = childNode.Name;
                    tnTmp.Tag = childNode;

                    if (childNode.Id == gwbs.Id)
                    {
                        tvwCategory.Nodes.Add(tnTmp);
                    }
                    else if (childNode.ParentNode != null)
                    {
                        TreeNode tnp = null;
                        tnp = hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                        if (tnp != null)
                            tnp.Nodes.Add(tnTmp);
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


        private string GetFullPath(GWBSTree wbs)
        {
            string path = string.Empty;

            path = wbs.Name;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", wbs.Id));
            oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof(GWBSTree), oq);

            wbs = list[0] as GWBSTree;
            CategoryNode parent = wbs.ParentNode;
            while (parent != null)
            {
                path = parent.Name + "\\" + path;

                oq.Criterions.Clear();
                oq.Criterions.Clear();
                oq.AddCriterion(Expression.Eq("Id", parent.Id));
                oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
                list = model.ObjectQuery(typeof(GWBSTree), oq);

                parent = (list[0] as CategoryNode).ParentNode;
            }

            return path;
        }
        void detailBtns_Click(object sender, EventArgs e)
        {
            GWBSTree selectWBS = null;
            if (tvwCategory.Visible == true)
            {
                if (tvwCategory.SelectedNode == null)
                {
                    MessageBox.Show("请选择一个任务节点！");
                    return;
                }
                selectWBS = tvwCategory.SelectedNode.Tag as GWBSTree;
            }
            else if (gridTask.Visible == true)
            {
                if (gridTask.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请选择一个任务节点！");
                    return;
                }
                selectWBS = gridTask.SelectedRows[0].Tag as GWBSTree;
            }


            VGwbsManagedetails vgs = new VGwbsManagedetails();
            //vgs.Close();
            vgs.DefaultGWBSTree = selectWBS;
            vgs.ShowDialog();
        }


        void detailBtn_Click(object sender, EventArgs e)
        {
            GWBSTree selectWBS = null;
            if (tvwCategory.Visible == true)
            {
                if (tvwCategory.SelectedNode == null)
                {
                    MessageBox.Show("请选择一个任务节点！");
                    return;
                }
                selectWBS = tvwCategory.SelectedNode.Tag as GWBSTree;
            }
            else if (gridTask.Visible == true)
            {
                if (gridTask.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请选择一个任务节点！");
                    return;
                }
                selectWBS = gridTask.SelectedRows[0].Tag as GWBSTree;
            }
            selectWBS = LoadRelaAttribute(selectWBS);

            VGwbsManagedetail vg = new VGwbsManagedetail();
            vg.DefaultGWBSTree = selectWBS;
            vg.ShowDialog();


        }
        private GWBSTree LoadRelaAttribute(GWBSTree wbs)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", wbs.Id));
            oq.AddFetchMode("ListRelaPBS", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ProjectTaskTypeGUID", NHibernate.FetchMode.Eager);

            IList list = model.ObjectQuery(typeof(GWBSTree), oq);
            if (list.Count > 0)
            {
                wbs = list[0] as GWBSTree;
            }
            return wbs;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void VGwbsManage_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
    }
}
