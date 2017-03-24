using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VPlanProjectAmountSharing : TBasicDataView
    {
        GWBSTree wbs = new GWBSTree();
        IList accountingGWBSDetailList = new ArrayList();//核算工程任务明细集
        IList planProjectsAmountList = new ArrayList();//计划工程量集
        string nodeFullPath = string.Empty;
        //decimal sharingSummaryAmount = 0;
        IList sharingProjectsAmountList = new ArrayList();//分摊工程量集
        PlanProjectsAmountValueObj operatePlanProjectAmount = null;
        int mouseRightRowIndex = -1;
        IList leafNodeWBSDetailList = new ArrayList();//叶节点工程任务明细
        //string operateType = string.Empty;
        IList checkProduceNodeList = null;//当前核算节点分摊下的生产节点集
        GWBSDetail addDetail = new GWBSDetail();

        CurrentProjectInfo projectInfo = null;
        MGWBSTree model = new MGWBSTree();
        public VPlanProjectAmountSharing()
        {
            InitializeComponent();
        }
        public VPlanProjectAmountSharing(GWBSTree w)
        {
            InitializeComponent();
            wbs = w;
            projectInfo = StaticMethod.GetProjectInfo();
            nodeFullPath = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), wbs.Name, wbs.SysCode);
            InitEvents();
            InitDate();
        }
        public VPlanProjectAmountSharing(TreeNode node)
        {
            InitializeComponent();
            wbs = node.Tag as GWBSTree;
            projectInfo = StaticMethod.GetProjectInfo();
            nodeFullPath = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), wbs.Name, wbs.SysCode);
            InitEvents();
            InitDate(node);
        }
        void InitEvents()
        {
            lnkCheckAll.Click += new EventHandler(lnkCheckAll_Click);
            lnkInverse.Click += new EventHandler(lnkInverse_Click);
            btnSumbitSharing.Click += new EventHandler(btnSumbitSharing_Click);
            dgProjectTaskDetail.CellClick += new DataGridViewCellEventHandler(dgProjectTaskDetail_CellClick);
            //dgSubordinatesProductionNode.CellContentClick += new DataGridViewCellEventHandler(dgSubordinatesProductionNode_CellContentClick);
            dgSharingProjectAmount.CellEndEdit += new DataGridViewCellEventHandler(dgSharingProjectAmount_CellEndEdit);
            dgProjectTaskDetail.CellMouseDown += new DataGridViewCellMouseEventHandler(dgProjectTaskDetail_CellMouseDown);
            cmuGWBSDetailMouseRight.ItemClicked += new ToolStripItemClickedEventHandler(cmuGWBSDetailMouseRight_ItemClicked);
            //tvwGWBS.BeforeSelect += new TreeViewCancelEventHandler(tvwGWBS_BeforeSelect);
            tvwGWBS.AfterExpand += new TreeViewEventHandler(tvwGWBS_AfterExpand);
            tvwGWBS.BeforeCheck += new TreeViewCancelEventHandler(tvwGWBS_BeforeCheck);
            tvwGWBS.AfterCheck += new TreeViewEventHandler(tvwGWBS_AfterCheck);
            //tvwGWBS.AfterSelect += new TreeViewEventHandler(tvwGWBS_AfterSelect);

            cbBulkSharing.CheckedChanged += new EventHandler(cbBulkSharing_CheckedChanged);
            cbShowNoSharingDetail.CheckedChanged += new EventHandler(cbShowNoSharingDetail_CheckedChanged);
        }

        void InitDate()
        {
            FlashScreen.Show("正在加载数据，请稍候……");
            accountingGWBSDetailList = GetAccountingGWBSDetailList();
            txtAccountingNodePath.Text = nodeFullPath;

            sharingProjectsAmountList = GetSharingProjectsAmountList();

            LoadTvwGWBS();

            PlanSelect.Visible = false;
            planProjectsAmountList = GetPlanProjectsAmountList();

            if (planProjectsAmountList != null && planProjectsAmountList.Count > 0)
            {
                dgProjectTaskDetailInsert(planProjectsAmountList);
            }
            FlashScreen.Close();
        }

        void InitDate(TreeNode node)
        {
            FlashScreen.Show("正在加载数据，请稍候……");
            accountingGWBSDetailList = GetAccountingGWBSDetailList();
            txtAccountingNodePath.Text = nodeFullPath;

            sharingProjectsAmountList = GetSharingProjectsAmountList();
            TreeNode tnTmp = new TreeNode();
            tnTmp.Name = wbs.Id;
            tnTmp.Text = wbs.Name;
            tnTmp.Tag = wbs;
            tvwGWBS.Nodes.Add(tnTmp);
            LoadTvwGWBS(tnTmp);

            PlanSelect.Visible = false;
            planProjectsAmountList = GetPlanProjectsAmountList();

            if (planProjectsAmountList != null && planProjectsAmountList.Count > 0)
            {
                dgProjectTaskDetailInsert(planProjectsAmountList);
            }
            FlashScreen.Close();
        }

        /// <summary>
        /// 选中节点为  (核算{工程项目任务})，取  (核算{工程项目任务})  关联{工程任务明细}，构成  (核算{工程任务明细}集)
        /// </summary>
        /// <returns></returns>
        IList GetAccountingGWBSDetailList()
        {
            //GWBSDetail d = new GWBSDetail();
            //d.ListCostSubjectDetails
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheGWBS.Id", wbs.Id));
            oq.AddCriterion(Expression.Eq("CostingFlag", 1));
            //oq.AddCriterion(Expression.Not(Expression.Eq("State", DocumentState.Invalid)));
            oq.AddCriterion(Expression.Eq("State", DocumentState.InExecute));
            oq.AddFetchMode("TheGWBS", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("TheGWBS.ProjectTaskTypeGUID", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ListCostSubjectDetails", NHibernate.FetchMode.Eager);
            oq.AddOrder(Order.Asc("OrderNo"));
            return model.ObjectQuery(typeof(GWBSDetail), oq);
        }
        /// <summary>
        /// 分摊工程量集
        /// </summary>
        /// <returns></returns>
        IList GetSharingProjectsAmountList()
        {
            IList list = new ArrayList();
            //IList accountingGWBSDetailList = GetAccountingGWBSDetailList();
            if (accountingGWBSDetailList != null && accountingGWBSDetailList.Count > 0)
            {
                GWBSDetail wbsDetail = accountingGWBSDetailList[0] as GWBSDetail;
                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();
                oq.AddCriterion(Expression.Eq("TheGWBS.CategoryNodeType", VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode));
                oq.AddCriterion(Expression.Like("TheGWBSSysCode", wbsDetail.TheGWBSSysCode, MatchMode.Start));
                foreach (GWBSDetail detail in accountingGWBSDetailList)
                {
                    if (detail.MainResourceTypeId != null)
                    {
                        if (!string.IsNullOrEmpty(detail.DiagramNumber))
                        {
                            dis.Add(Expression.And(Expression.And(Expression.Eq("MainResourceTypeId", detail.MainResourceTypeId), Expression.Eq("TheCostItem.Id", detail.TheCostItem.Id)), Expression.Eq("DiagramNumber", detail.DiagramNumber)));
                        }
                        else
                        {
                            dis.Add(Expression.And(Expression.And(Expression.Eq("MainResourceTypeId", detail.MainResourceTypeId), Expression.Eq("TheCostItem.Id", detail.TheCostItem.Id)), Expression.IsNull("DiagramNumber")));
                        }
                    }
                    else
                    {
                        dis.Add(Expression.And(Expression.Eq("TheCostItem.Id", detail.TheCostItem.Id), Expression.IsNull("MainResourceTypeId")));
                    }

                }
                oq.AddCriterion(dis);
                oq.AddCriterion(Expression.Eq("State", DocumentState.InExecute));
                oq.AddFetchMode("TheGWBS", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("TheGWBS.Details", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("TheGWBS.ProjectTaskTypeGUID", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
                leafNodeWBSDetailList = model.ObjectQuery(typeof(GWBSDetail), oq);
                if (leafNodeWBSDetailList != null && leafNodeWBSDetailList.Count > 0)
                {
                    foreach (GWBSDetail d in leafNodeWBSDetailList)
                    {
                        SharingProjectsAmountValueObj spa = new SharingProjectsAmountValueObj();
                        string leadNodeFullPath = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), d.TheGWBS.Name, d.TheGWBS.SysCode);
                        string leadNodePath = leadNodeFullPath.Replace(nodeFullPath, "");
                        spa.Node = d.TheGWBS;
                        spa.LeafNodePath = leadNodePath;
                        spa.ProjectTaskDetailGUID = d;
                        spa.ProjectTaskDetaikName = d.Name;
                        spa.MainResourceTypeId = d.MainResourceTypeId;
                        spa.CostItems = d.TheCostItem;
                        spa.ExistingProjectAmount = d.PlanWorkAmount;
                        spa.TheSharingSummaryAmount = 0;
                        spa.DiagramNumber = d.DiagramNumber;
                        //sharingSummaryAmount += d.PlanWorkAmount;
                        list.Add(spa);
                    }

                }
            }
            return list;
        }

        /// <summary>
        /// 计划工程量集
        /// </summary>
        /// <returns></returns>
        IList GetPlanProjectsAmountList()
        {
            IList list = new ArrayList();
            //IList accountingGWBSDetailList = GetAccountingGWBSDetailList();
            if (accountingGWBSDetailList != null && accountingGWBSDetailList.Count > 0)
            {
                foreach (GWBSDetail detail in accountingGWBSDetailList)
                {
                    //detail.
                    PlanProjectsAmountValueObj ppa = new PlanProjectsAmountValueObj();
                    ppa.ProjectTaskDetailGUID = detail;
                    ppa.ProjectTaskDetaikName = detail.Name;
                    ppa.MainResourceTypeId = detail.MainResourceTypeId;
                    ppa.MainResourceTypeName = detail.MainResourceTypeName;
                    ppa.MainResourceTypeSpec = detail.MainResourceTypeSpec;
                    ppa.CostItems = detail.TheCostItem;
                    ppa.PlanProjectAmount = detail.PlanWorkAmount;
                    ppa.DiagramNumber = detail.DiagramNumber;
                    ppa.TheSharingSummaryAmount = 0;
                    //ppa.SharingSummaryAmount = sharingSummaryAmount;
                    foreach (GWBSDetail d in leafNodeWBSDetailList)
                    {
                        if (detail.TheCostItem.Id == d.TheCostItem.Id && detail.MainResourceTypeId == d.MainResourceTypeId && detail.DiagramNumber == d.DiagramNumber)
                        {
                            ppa.SharingSummaryAmount += d.PlanWorkAmount;
                        }
                    }
                    ppa.OldSharingSummaryAmount = ppa.SharingSummaryAmount;
                    list.Add(ppa);
                }
            }
            return list;
        }

        /// <summary>
        /// 工程任务明细列表
        /// </summary>
        void dgProjectTaskDetailInsert(IList list)
        {
            foreach (PlanProjectsAmountValueObj ppa in list)
            {
                int rowIndex = dgProjectTaskDetail.Rows.Add();
                dgProjectTaskDetail[PlanGWBSDetailName.Name, rowIndex].Value = ppa.ProjectTaskDetaikName;
                dgProjectTaskDetail[PlanMainResourceTypeName.Name, rowIndex].Value = ppa.MainResourceTypeName;
                dgProjectTaskDetail[PlanMainResourceTypeSpec.Name, rowIndex].Value = ppa.MainResourceTypeSpec;
                dgProjectTaskDetail[PlanTheCostItemName.Name, rowIndex].Value = ppa.CostItems.Name;
                dgProjectTaskDetail[PlanDiagramNumber.Name, rowIndex].Value = ppa.DiagramNumber;
                dgProjectTaskDetail[PlanProjectAmount.Name, rowIndex].Value = ppa.PlanProjectAmount;
                dgProjectTaskDetail[PlanSharingSummaryAmount.Name, rowIndex].Value = ppa.SharingSummaryAmount.ToString();
                dgProjectTaskDetail[PlanTheSharingSummaryAmount.Name, rowIndex].Value = ppa.TheSharingSummaryAmount.ToString();
                dgProjectTaskDetail[PlanQuotaCode.Name, rowIndex].Value = ppa.CostItems.QuotaCode;
                dgProjectTaskDetail.Rows[rowIndex].Tag = ppa;
            }
            dgProjectTaskDetail.ClearSelection();
        }

        //显示未分摊完的明细
        void cbShowNoSharingDetail_CheckedChanged(object sender, EventArgs e)
        {
            dgProjectTaskDetail.Rows.Clear();
            foreach (PlanProjectsAmountValueObj ppa in planProjectsAmountList)
            {
                if (cbShowNoSharingDetail.Checked)
                {
                    if (ppa.SharingSummaryAmount >= ppa.PlanProjectAmount)
                    {
                        continue;
                    }
                }
                int rowIndex = dgProjectTaskDetail.Rows.Add();
                dgProjectTaskDetail[PlanGWBSDetailName.Name, rowIndex].Value = ppa.ProjectTaskDetaikName;
                dgProjectTaskDetail[PlanMainResourceTypeName.Name, rowIndex].Value = ppa.MainResourceTypeName;
                dgProjectTaskDetail[PlanMainResourceTypeSpec.Name, rowIndex].Value = ppa.MainResourceTypeSpec;
                dgProjectTaskDetail[PlanTheCostItemName.Name, rowIndex].Value = ppa.CostItems.Name;
                dgProjectTaskDetail[PlanDiagramNumber.Name, rowIndex].Value = ppa.DiagramNumber;
                dgProjectTaskDetail[PlanProjectAmount.Name, rowIndex].Value = ppa.PlanProjectAmount;
                dgProjectTaskDetail[PlanSharingSummaryAmount.Name, rowIndex].Value = ppa.SharingSummaryAmount.ToString();
                dgProjectTaskDetail[PlanTheSharingSummaryAmount.Name, rowIndex].Value = ppa.TheSharingSummaryAmount.ToString();
                dgProjectTaskDetail[PlanQuotaCode.Name, rowIndex].Value = ppa.CostItems.QuotaCode;
                dgProjectTaskDetail.Rows[rowIndex].Tag = ppa;
            }
        }
        /// <summary>
        /// 生产节点集
        /// </summary>
        /// <returns></returns>
        //IList GetProductNodeList()
        //{
        //    ObjectQuery oq = new ObjectQuery();
        //    oq.AddCriterion(Expression.Eq("CategoryNodeType", VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode));
        //    oq.AddCriterion(Expression.Like("SysCode", wbs.SysCode, MatchMode.Start));
        //    oq.AddFetchMode("ProjectTaskTypeGUID", NHibernate.FetchMode.Eager);
        //    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
        //    return model.ObjectQuery(typeof(GWBSTree), oq);
        //}

        void LoadTvwGWBS()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("SysCode", wbs.SysCode + "%"));
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));

            oq.AddFetchMode("ProjectTaskTypeGUID", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
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
                    tnTmp.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    tnTmp.ForeColor = ColorTranslator.FromHtml("#000000");
                }


                if (childNode.ParentNode != null)
                {
                    TreeNode tnp = null;
                    tnp = hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                    if (tnp != null)
                        tnp.Nodes.Add(tnTmp);
                    else
                    {
                        tvwGWBS.Nodes.Add(tnTmp);
                    }
                }
                else
                {
                    tvwGWBS.Nodes.Add(tnTmp);
                }
                hashtable.Add(tnTmp.Name, tnTmp);
                tvwGWBS.ExpandAll();
            }
        }
        //分层加载
        void LoadTvwGWBS(TreeNode node)
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
                oq.AddOrder(Order.Asc("Level"));
                oq.AddOrder(Order.Asc("OrderNo"));

                oq.AddFetchMode("ProjectTaskTypeGUID", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
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
                        tnTmp.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                        tnTmp.ForeColor = ColorTranslator.FromHtml("#000000");
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
            }
            catch (Exception e)
            {
                MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        void tvwGWBS_AfterExpand(object sender, TreeViewEventArgs e)
        {
            LoadTvwGWBS(e.Node);
            if (checkProduceNodeList != null && checkProduceNodeList.Count > 0)
            {
                foreach (TreeNode node in e.Node.Nodes)
                {
                    if (node.Tag != null)
                    {
                        foreach (GWBSTree g in checkProduceNodeList)
                        {
                            GWBSTree t = node.Tag as GWBSTree;
                            if (t.Id == g.Id)
                            {
                                node.Checked = true;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 选择核算明细 加载分摊工程量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgProjectTaskDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                PlanProjectsAmountValueObj ppa = dgProjectTaskDetail.Rows[e.RowIndex].Tag as PlanProjectsAmountValueObj;
                operatePlanProjectAmount = ppa;

                tvwGWBS.AfterCheck -= new TreeViewEventHandler(tvwGWBS_AfterCheck);
                tvwGWBS.BeforeCheck -= new TreeViewCancelEventHandler(tvwGWBS_BeforeCheck);
                Inverse(tvwGWBS.Nodes[0]);
                tvwGWBS.AfterCheck += new TreeViewEventHandler(tvwGWBS_AfterCheck);
                tvwGWBS.BeforeCheck += new TreeViewCancelEventHandler(tvwGWBS_BeforeCheck);

                LoadSharingProjectAmount(ppa);
            }

        }
        void LoadSharingProjectAmount(PlanProjectsAmountValueObj ppa)
        {
            dgSharingProjectAmount.Rows.Clear();
            checkProduceNodeList = new ArrayList();
            if (sharingProjectsAmountList != null && sharingProjectsAmountList.Count > 0)
            {
                foreach (SharingProjectsAmountValueObj spa in sharingProjectsAmountList)
                {
                    if (ppa.MainResourceTypeId == spa.MainResourceTypeId && ppa.CostItems.Id == spa.CostItems.Id && ppa.DiagramNumber == spa.DiagramNumber)
                    {
                        int rowIndex = dgSharingProjectAmount.Rows.Add();
                        dgSharingProjectAmount[SharingLeafNodePath.Name, rowIndex].Value = spa.LeafNodePath;
                        dgSharingProjectAmount[SharingGWBSDetail.Name, rowIndex].Value = spa.ProjectTaskDetaikName;
                        dgSharingProjectAmount[SharingExistProjectAmount.Name, rowIndex].Value = spa.ExistingProjectAmount.ToString();
                        dgSharingProjectAmount[SharingTheSharingProjectAmount.Name, rowIndex].Value = spa.TheSharingSummaryAmount.ToString();
                        dgSharingProjectAmount[SharingQuotaCode.Name, rowIndex].Value = spa.CostItems.QuotaCode;
                        dgSharingProjectAmount.Rows[rowIndex].Tag = spa;
                        if (!spa.Showflag)//false 不可编辑
                        {
                            dgSharingProjectAmount[SharingTheSharingProjectAmount.Name, rowIndex].ReadOnly = true;
                            dgSharingProjectAmount[SharingTheSharingProjectAmount.Name, rowIndex].Style.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                        }
                        Selected(spa.Node, spa.Showflag);
                    }
                }
            }
        }
        void Selected(GWBSTree g, bool flag)
        {
            if (flag == false) return;
            checkProduceNodeList.Add(g);
            if (tvwGWBS.Nodes.Count > 0)
            {
                Checked(tvwGWBS.Nodes[0], g);
            }
        }

        void Checked(TreeNode parentNodes, GWBSTree g)
        {
            foreach (TreeNode node in parentNodes.Nodes)
            {
                if (node.Tag != null)
                {
                    GWBSTree t = node.Tag as GWBSTree;
                    if (t.Id == g.Id)
                    {
                        node.Checked = true;
                    }
                }
                Checked(node, g);
            }
        }

        /// <summary>
        /// 汇总输入工程量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgSharingProjectAmount_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgSharingProjectAmount.Rows[e.RowIndex].Cells[SharingTheSharingProjectAmount.Name].ColumnIndex)
            {
                try
                {
                    SharingProjectsAmountValueObj spavo = dgSharingProjectAmount.Rows[e.RowIndex].Tag as SharingProjectsAmountValueObj;
                    PlanProjectsAmountValueObj ppa = dgProjectTaskDetail.SelectedRows[0].Tag as PlanProjectsAmountValueObj;

                    decimal theSharingSummaryAmountTotal = 0;

                    foreach (DataGridViewRow row in dgSharingProjectAmount.Rows)
                    {
                        theSharingSummaryAmountTotal += ClientUtil.ToDecimal(row.Cells[SharingTheSharingProjectAmount.Name].Value);
                    }
                    if ((ppa.OldSharingSummaryAmount + theSharingSummaryAmountTotal) > ppa.PlanProjectAmount)
                    {
                        MessageBox.Show("分摊的总工程量不能大于计划工程量！");
                        dgSharingProjectAmount[SharingTheSharingProjectAmount.Name, e.RowIndex].Value = 0;
                        return;
                    }
                    spavo.TheSharingSummaryAmount = ClientUtil.ToDecimal(dgSharingProjectAmount[SharingTheSharingProjectAmount.Name, e.RowIndex].Value);

                    UpdateProjectTaskDetail(ppa, theSharingSummaryAmountTotal, "无用");
                }
                catch
                {
                    MessageBox.Show("输入有误！请重新输入！");
                    dgSharingProjectAmount[SharingTheSharingProjectAmount.Name, e.RowIndex].Value = 0;
                }
            }
        }

        void UpdateProjectTaskDetail(PlanProjectsAmountValueObj ppa, decimal total, string s)
        {
            ppa.TheSharingSummaryAmount = total;
            //foreach (SharingProjectsAmountValueObj spa in sharingProjectsAmountList)
            //{
            //    if (ppa.MainResourceTypeId == spa.MainResourceTypeId && ppa.CostItems.Id == spa.CostItems.Id)
            //    {
            //        //ppa.SharingSummaryAmount += spa.TheSharingSummaryAmount;
            //        ppa.TheSharingSummaryAmount += spa.TheSharingSummaryAmount;
            //    }
            //}

            //ppa.SharingSummaryAmount += ppa.TheSharingSummaryAmount;
            //foreach (GWBSDetail d in accountingGWBSDetailList)
            //{
            //    if (ppa.ProjectTaskDetailGUID.Id == d.Id)
            //    {
            ppa.SharingSummaryAmount = ppa.OldSharingSummaryAmount + ppa.TheSharingSummaryAmount;
            //    }
            //}
            int rowIndex = dgProjectTaskDetail.SelectedRows[0].Index;
            dgProjectTaskDetail[PlanSharingSummaryAmount.Name, rowIndex].Value = ppa.SharingSummaryAmount;
            dgProjectTaskDetail[PlanTheSharingSummaryAmount.Name, rowIndex].Value = ppa.TheSharingSummaryAmount;
            dgProjectTaskDetail.Rows[rowIndex].Tag = ppa;
        }
        void UpdateProjectTaskDetail(PlanProjectsAmountValueObj ppa, decimal num)
        {
            ppa.TheSharingSummaryAmount = 0;

            foreach (DataGridViewRow row in dgSharingProjectAmount.Rows)
            {
                ppa.TheSharingSummaryAmount += ClientUtil.ToDecimal(row.Cells[SharingTheSharingProjectAmount.Name].Value);
            }
            ppa.TheSharingSummaryAmount -= num;
            ppa.SharingSummaryAmount = ppa.OldSharingSummaryAmount + ppa.TheSharingSummaryAmount;

            int rowIndex = dgProjectTaskDetail.SelectedRows[0].Index;
            dgProjectTaskDetail[PlanSharingSummaryAmount.Name, rowIndex].Value = ppa.SharingSummaryAmount;
            dgProjectTaskDetail[PlanTheSharingSummaryAmount.Name, rowIndex].Value = ppa.TheSharingSummaryAmount;
            dgProjectTaskDetail.Rows[rowIndex].Tag = ppa;
        }

        /// <summary>
        /// 核算工程任务明细右键菜单  类比分摊
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgProjectTaskDetail_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    mouseRightRowIndex = e.RowIndex;
                    if (dgProjectTaskDetail.Rows[e.RowIndex].Selected == true)
                    {
                        //dgProjectTaskDetail.ClearSelection();
                        dgProjectTaskDetail.Rows[e.RowIndex].Selected = false;
                    }
                    cmuGWBSDetailMouseRight.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }
        void cmuGWBSDetailMouseRight_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == 类比分摊.Name)
            {
                cmuGWBSDetailMouseRight.Hide();
                if (dgProjectTaskDetail.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请选择操作行！");
                    return;
                }
                PlanProjectsAmountValueObj operating = dgProjectTaskDetail.SelectedRows[0].Tag as PlanProjectsAmountValueObj;
                PlanProjectsAmountValueObj analogy = dgProjectTaskDetail.Rows[mouseRightRowIndex].Tag as PlanProjectsAmountValueObj;
                if (analogy.TheSharingSummaryAmount == 0)
                {
                    MessageBox.Show("本次分摊汇总量为0，请重新选择！");
                    return;
                }
                decimal analogyFactor = (operating.PlanProjectAmount - operating.SharingSummaryAmount) / analogy.TheSharingSummaryAmount;
                IList operatingSharingProjectsAmountList = new ArrayList();
                IList analogySharingProjectsAmountList = new ArrayList();
                if (sharingProjectsAmountList != null && sharingProjectsAmountList.Count > 0)
                {
                    foreach (SharingProjectsAmountValueObj spa in sharingProjectsAmountList)
                    {
                        if (spa.MainResourceTypeId == operating.MainResourceTypeId && spa.CostItems.Id == operating.CostItems.Id && spa.DiagramNumber == operating.DiagramNumber)
                            operatingSharingProjectsAmountList.Add(spa);
                        if (spa.MainResourceTypeId == analogy.MainResourceTypeId && spa.CostItems.Id == analogy.CostItems.Id && spa.DiagramNumber == analogy.DiagramNumber)
                            analogySharingProjectsAmountList.Add(spa);
                    }
                }
                else
                {
                    return;
                }

                #region  操作分摊工程量集
                if (operatingSharingProjectsAmountList != null && operatingSharingProjectsAmountList.Count > 0)
                {
                    List<SharingProjectsAmountValueObj> spaList = new List<SharingProjectsAmountValueObj>();
                    #region
                    for (int i = 0; i < operatingSharingProjectsAmountList.Count; i++)
                    {
                        //foreach (SharingProjectsAmountValueObj ospa in operatingSharingProjectsAmountList)
                        //{
                        SharingProjectsAmountValueObj ospa = operatingSharingProjectsAmountList[i] as SharingProjectsAmountValueObj;
                        bool flag = false;
                        SharingProjectsAmountValueObj aspaIs = null;
                        if (analogySharingProjectsAmountList != null && analogySharingProjectsAmountList.Count > 0)
                        {
                            foreach (SharingProjectsAmountValueObj aspa in analogySharingProjectsAmountList)
                            {
                                if (ospa.LeafNodePath == aspa.LeafNodePath)
                                {
                                    flag = true;
                                    aspaIs = aspa;
                                    break;
                                }
                            }
                        }
                        if (flag)
                        {
                            ospa.TheSharingSummaryAmount = analogyFactor * aspaIs.TheSharingSummaryAmount;
                            analogySharingProjectsAmountList.Remove(aspaIs);
                        }
                        else
                        {
                            if (ospa.ExistingProjectAmount == 0)
                            {
                                spaList.Add(ospa);
                                //operatingSharingProjectsAmountList.Remove(ospa);
                                //sharingProjectsAmountList.Remove(ospa);
                            }
                            else
                            {
                                //foreach (DataGridViewRow row in dgSharingProjectAmount.Rows)
                                for (int j = 0; j < dgSharingProjectAmount.Rows.Count; j++)
                                {
                                    DataGridViewRow row = dgSharingProjectAmount.Rows[j];
                                    SharingProjectsAmountValueObj spavo = row.Tag as SharingProjectsAmountValueObj;
                                    //if (spavo.LeafNodePath == ospa.LeafNodePath && spavo.CostItems.Id == ospa.CostItems.Id && spavo.MainResourceTypeId == ospa.MainResourceTypeId && row.Cells[SharingTheSharingProjectAmount.Name].ReadOnly == false)
                                    if (spavo == ospa && row.Cells[SharingTheSharingProjectAmount.Name].ReadOnly == false)
                                    {
                                        ospa.TheSharingSummaryAmount = 0;
                                        row.Cells[SharingTheSharingProjectAmount.Name].Value = 0;
                                        row.Cells[SharingTheSharingProjectAmount.Name].ReadOnly = true;
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    if (spaList != null && spaList.Count > 0)
                    {
                        foreach (SharingProjectsAmountValueObj spa in spaList)
                        {
                            operatingSharingProjectsAmountList.Remove(spa);
                            sharingProjectsAmountList.Remove(spa);
                        }
                    }
                    //#endregion
                }
                #endregion

                #region 类比分摊工程量集
                if (analogySharingProjectsAmountList != null && analogySharingProjectsAmountList.Count > 0)
                {
                    foreach (SharingProjectsAmountValueObj aspa in analogySharingProjectsAmountList)
                    {
                        SharingProjectsAmountValueObj spa = new SharingProjectsAmountValueObj();
                        spa.LeafNodePath = aspa.LeafNodePath;
                        spa.Node = aspa.Node;//analogy.ProjectTaskDetailGUID.TheGWBS;
                        spa.ProjectTaskDetaikName = operating.ProjectTaskDetaikName;
                        spa.MainResourceTypeId = operating.MainResourceTypeId;
                        spa.CostItems = operating.CostItems;
                        spa.ExistingProjectAmount = 0;
                        spa.TheSharingSummaryAmount = analogyFactor * aspa.TheSharingSummaryAmount;
                        spa.DiagramNumber = operating.DiagramNumber;
                        sharingProjectsAmountList.Add(spa);
                        operatingSharingProjectsAmountList.Add(spa);
                    }
                }
                #endregion

                dgSharingProjectAmount.Rows.Clear();

                tvwGWBS.AfterCheck -= new TreeViewEventHandler(tvwGWBS_AfterCheck);
                tvwGWBS.BeforeCheck -= new TreeViewCancelEventHandler(tvwGWBS_BeforeCheck);
                Inverse(tvwGWBS.Nodes[0]);
                tvwGWBS.AfterCheck += new TreeViewEventHandler(tvwGWBS_AfterCheck);
                tvwGWBS.BeforeCheck += new TreeViewCancelEventHandler(tvwGWBS_BeforeCheck);

                decimal theSharingSummaryAmountTotal = 0;
                if (operatingSharingProjectsAmountList != null && operatingSharingProjectsAmountList.Count > 0)
                {
                    foreach (SharingProjectsAmountValueObj spa in operatingSharingProjectsAmountList)
                    {
                        int rowIndex = dgSharingProjectAmount.Rows.Add();
                        dgSharingProjectAmount[SharingLeafNodePath.Name, rowIndex].Value = spa.LeafNodePath;
                        dgSharingProjectAmount[SharingGWBSDetail.Name, rowIndex].Value = spa.ProjectTaskDetaikName;
                        dgSharingProjectAmount[SharingExistProjectAmount.Name, rowIndex].Value = spa.ExistingProjectAmount.ToString();
                        dgSharingProjectAmount[SharingTheSharingProjectAmount.Name, rowIndex].Value = spa.TheSharingSummaryAmount.ToString();
                        dgSharingProjectAmount[SharingQuotaCode.Name, rowIndex].Value = spa.CostItems.QuotaCode;
                        dgSharingProjectAmount.Rows[rowIndex].Tag = spa;

                        theSharingSummaryAmountTotal += spa.TheSharingSummaryAmount;
                        Selected(spa.Node, spa.Showflag);
                    }
                }
                PlanProjectsAmountValueObj ppa = dgProjectTaskDetail.SelectedRows[0].Tag as PlanProjectsAmountValueObj;
                UpdateProjectTaskDetail(ppa, theSharingSummaryAmountTotal, "无用");
            }
            else if (e.ClickedItem.Name == 批量分摊.Name)//按比例分摊
            {
                cmuGWBSDetailMouseRight.Hide();
                dgProjectTaskDetail.ClearSelection();
                dgProjectTaskDetail.Rows[mouseRightRowIndex].Selected = true;
                btnSumbitSharing.Focus();
                PlanProjectsAmountValueObj ppa = dgProjectTaskDetail.SelectedRows[0].Tag as PlanProjectsAmountValueObj;
                IList list = new ArrayList();
                foreach (DataGridViewRow row in dgSharingProjectAmount.Rows)
                {
                    if (!row.Cells[SharingTheSharingProjectAmount.Name].ReadOnly)
                    {
                        SharingProjectsAmountValueObj spa = row.Tag as SharingProjectsAmountValueObj;
                        spa.rowIndex = row.Index;
                        list.Add(spa);
                    }
                }
                if (list == null || list.Count == 0 || ppa.PlanProjectAmount == 0) return;
                VSharingProportion frm = new VSharingProportion(list, ppa.PlanProjectAmount - ppa.OldSharingSummaryAmount);
                frm.ShowDialog();
                IList resultList = frm.ResultList;
                if (resultList == null || resultList.Count == 0) return;
                foreach (SharingProjectsAmountValueObj spa in resultList)
                {
                    dgSharingProjectAmount[SharingTheSharingProjectAmount.Name, spa.rowIndex].Value = spa.TheSharingSummaryAmount;
                    SharingProjectsAmountValueObj spa1 = dgSharingProjectAmount.Rows[spa.rowIndex].Tag as SharingProjectsAmountValueObj;
                    spa1.TheSharingSummaryAmount = spa.TheSharingSummaryAmount;
                    dgSharingProjectAmount.Rows[spa.rowIndex].Tag = spa1;
                }
                UpdateProjectTaskDetail(ppa, 0);
            }
        }


        /// <summary>
        /// 提交分摊
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSumbitSharing_Click(object sender, EventArgs e)
        {
            IList updateDetailList = new ArrayList();
            IList addGWBSDetailList = new ArrayList();
            IList updateLeadNodeList = new ArrayList();
            if (cbBulkSharing.Checked && dgSharingProjectAmount.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgSharingProjectAmount.Rows)
                {
                    SharingProjectsAmountValueObj s = row.Tag as SharingProjectsAmountValueObj;
                    sharingProjectsAmountList.Add(s);
                }
            }
            if (sharingProjectsAmountList == null && sharingProjectsAmountList.Count == 0) return;
            foreach (SharingProjectsAmountValueObj spa in sharingProjectsAmountList)
            {
                if (!((spa.TheSharingSummaryAmount != 0 && spa.ProjectTaskDetailGUID != null) || (spa.ExistingProjectAmount == 0 && spa.TheSharingSummaryAmount != 0)))
                    continue;
                bool flag = true;
                GWBSTree gwbsTree = spa.Node;
                if (updateLeadNodeList != null && updateLeadNodeList.Count > 0)
                {
                    foreach (GWBSTree tree in updateLeadNodeList)
                    {
                        if (gwbsTree.Id == tree.Id)
                        {
                            gwbsTree = tree;
                            flag = false;
                            break;
                        }
                    }
                }
                //SharingProjectsAmountValueObj spa = row.Tag as SharingProjectsAmountValueObj;
                #region 更新工程量

                if (spa.TheSharingSummaryAmount != 0 && spa.ProjectTaskDetailGUID != null)
                {
                    GWBSDetail detail = spa.ProjectTaskDetailGUID;
                    foreach (GWBSDetail d in gwbsTree.Details)
                    {
                        if (d.Id == detail.Id)
                        {
                            detail = d;
                            break;
                        }
                    }
                    GWBSDetail accountingDtl = GetAccountingDtlByProduceDtl(detail);

                    detail.PlanWorkAmount += spa.TheSharingSummaryAmount;
                    if (accountingDtl != null)
                    {
                        decimal apportionmentFactor = detail.PlanWorkAmount / accountingDtl.PlanWorkAmount;
                        detail.ContractProjectQuantity = apportionmentFactor * accountingDtl.ContractProjectQuantity;
                        detail.ContractPrice = accountingDtl.ContractPrice;
                        detail.ContractTotalPrice = detail.ContractProjectQuantity * detail.ContractPrice;
                        detail.ResponsibilitilyWorkAmount = apportionmentFactor * accountingDtl.ResponsibilitilyWorkAmount;
                        detail.ResponsibilitilyPrice = accountingDtl.ResponsibilitilyPrice;
                        detail.ResponsibilitilyTotalPrice = detail.ResponsibilitilyWorkAmount * detail.ResponsibilitilyPrice;
                        detail.PlanPrice = accountingDtl.PlanPrice;
                        detail.PlanTotalPrice = detail.PlanWorkAmount * detail.PlanPrice;
                    }
                    //updateDetailList.Add(detail);
                    if (flag)
                    {
                        updateLeadNodeList.Add(gwbsTree);
                    }
                    continue;
                }

                #endregion
                #region 新建工程量
                if (spa.ExistingProjectAmount == 0 && spa.TheSharingSummaryAmount != 0)
                {
                    if (accountingGWBSDetailList != null && accountingGWBSDetailList.Count > 0)
                    {
                        #region 新建 赋值
                        foreach (GWBSDetail detail in accountingGWBSDetailList)
                        {
                            if (detail.MainResourceTypeId == spa.MainResourceTypeId && detail.TheCostItem.Id == spa.CostItems.Id && detail.DiagramNumber == spa.DiagramNumber)
                            {
                                //GWBSDetail addDetail = new GWBSDetail();
                                addDetail = new GWBSDetail();

                                #region code
                                int maxOrderNo = 1;
                                int count = gwbsTree.Details.Count;
                                int code = count + 1;
                                //获取父对象下最大明细号
                                for (int i = 0; i < count; i++)
                                {
                                    GWBSDetail dtl = gwbsTree.Details.ElementAt<GWBSDetail>(i);

                                    if (dtl != null)
                                    {
                                        if (dtl.OrderNo > maxOrderNo)
                                            maxOrderNo = dtl.OrderNo;

                                        if (!string.IsNullOrEmpty(dtl.Code))
                                        {
                                            try
                                            {
                                                if (dtl.Code.IndexOf("-") > -1)
                                                {
                                                    int tempCode = Convert.ToInt32(dtl.Code.Substring(dtl.Code.LastIndexOf("-") + 1));
                                                    if (tempCode >= code)
                                                        code = tempCode + 1;
                                                }
                                            }
                                            catch
                                            {

                                            }
                                        }
                                    }
                                }
                                addDetail.OrderNo = maxOrderNo + 1;
                                addDetail.Code = gwbsTree.Code + "-" + code.ToString().PadLeft(5, '0');
                                #endregion

                                addDetail.TheProjectGUID = projectInfo.Id;
                                addDetail.TheProjectName = projectInfo.Name;

                                addDetail.Name = detail.Name;
                                addDetail.TheGWBS = gwbsTree;
                                addDetail.TheGWBSSysCode = gwbsTree.SysCode;
                                //addDetail.TheGWBSFullPath = gwbsTree.FullPath;
                                addDetail.State = VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute;
                                addDetail.CurrentStateTime = model.GetServerTime();

                                addDetail.ContractGroupGUID = detail.ContractGroupGUID;
                                addDetail.ContractGroupName = detail.ContractGroupName;
                                addDetail.ContractGroupCode = detail.ContractGroupCode;
                                addDetail.ContractGroupType = detail.ContractGroupType;

                                addDetail.ProjectTaskTypeCode = gwbsTree.ProjectTaskTypeGUID.Code;

                                addDetail.Summary = detail.Summary;
                                addDetail.ContentDesc = detail.ContentDesc;

                                addDetail.SubContractStepRate = 0;

                                #region 成本项
                                addDetail.TheCostItem = detail.TheCostItem;
                                addDetail.TheCostItemCateSyscode = detail.TheCostItemCateSyscode;

                                CostItem item = detail.TheCostItem;
                                if (item != null)
                                {
                                    addDetail.WorkAmountUnitGUID = item.ProjectUnitGUID;
                                    addDetail.WorkAmountUnitName = item.ProjectUnitName;
                                    addDetail.PriceUnitGUID = item.PriceUnitGUID;
                                    addDetail.PriceUnitName = item.PriceUnitName;

                                }
                                #endregion

                                addDetail.MainResourceTypeId = detail.MainResourceTypeId;
                                addDetail.MainResourceTypeName = detail.MainResourceTypeName;
                                addDetail.MainResourceTypeQuality = detail.MainResourceTypeQuality;
                                addDetail.MainResourceTypeSpec = detail.MainResourceTypeSpec;

                                addDetail.DiagramNumber = detail.DiagramNumber;

                                addDetail.DiagramNumber = detail.DiagramNumber;

                                GWBSDetail accountingDtl = GetAccountingDtlByProduceDtl(addDetail);
                                addDetail.PlanWorkAmount = spa.TheSharingSummaryAmount;
                                if (accountingDtl != null)
                                {
                                    decimal apportionmentFactor = addDetail.PlanWorkAmount / accountingDtl.PlanWorkAmount;
                                    addDetail.ContractProjectQuantity = apportionmentFactor * accountingDtl.ContractProjectQuantity;
                                    addDetail.ContractPrice = accountingDtl.ContractPrice;
                                    addDetail.ContractTotalPrice = addDetail.ContractProjectQuantity * addDetail.ContractPrice;

                                    addDetail.ResponsibilitilyWorkAmount = apportionmentFactor * accountingDtl.ResponsibilitilyWorkAmount;
                                    addDetail.ResponsibilitilyPrice = accountingDtl.ResponsibilitilyPrice;
                                    addDetail.ResponsibilitilyTotalPrice = addDetail.ResponsibilitilyWorkAmount * addDetail.ResponsibilitilyPrice;

                                    addDetail.PlanPrice = accountingDtl.PlanPrice;
                                    //addDetail.PlanTotalPrice = addDetail.PlanWorkAmount * addDetail.PlanPrice;
                                }
                                else
                                {
                                    addDetail.ResponsibilitilyWorkAmount = 0;
                                    addDetail.ResponsibilitilyPrice = 0;
                                    addDetail.ResponsibilitilyTotalPrice = 0;

                                    addDetail.ContractProjectQuantity = 0;
                                    addDetail.ContractPrice = 0;
                                    addDetail.ContractTotalPrice = 0;

                                    addDetail.PlanPrice = detail.PlanPrice;

                                }
                                addDetail.PlanTotalPrice = addDetail.PlanWorkAmount * addDetail.PlanPrice;
                                addDetail.ResponseFlag = 0;
                                addDetail.CostingFlag = 0;
                                addDetail.SubContractFeeFlag = false;
                                addDetail.ProduceConfirmFlag = 1;

                                addDetail.AddupAccQuantity = 0;
                                addDetail.AddupAccFigureProgress = 0;
                                addDetail.QuantityConfirmed = 0;
                                addDetail.ProgressConfirmed = 0;
                                //资源耗用明细
                                foreach (GWBSDetailCostSubject cs in detail.ListCostSubjectDetails)
                                {
                                    AddResourceUsageInTaskDetail(cs);
                                }
                                //addGWBSDetailList.Add(addDetail);

                                gwbsTree.Details.Add(addDetail);
                                //updateDetailList.Add(addDetail);
                            }
                        }
                        #endregion

                        gwbsTree.ResponsibleAccFlag = false;
                        gwbsTree.CostAccFlag = false;
                        gwbsTree.SubContractFeeFlag = false;
                        gwbsTree.ProductConfirmFlag = true;

                        //decimal contractTotalPrice = 0;
                        //decimal responsibleTotalPrice = 0;
                        //decimal planTotalPrice = 0;
                        //foreach (GWBSDetail dtl in gwbsTree.Details)
                        //{
                        //    contractTotalPrice += dtl.ContractTotalPrice;
                        //    responsibleTotalPrice += dtl.ResponsibilitilyTotalPrice;
                        //    planTotalPrice += dtl.PlanTotalPrice;
                        //}
                        //gwbsTree.ContractTotalPrice = contractTotalPrice;
                        //gwbsTree.ResponsibilityTotalPrice = responsibleTotalPrice;
                        //gwbsTree.PlanTotalPrice = planTotalPrice;
                        if (flag)
                        {
                            updateLeadNodeList.Add(gwbsTree);
                        }
                    }
                }
                #endregion
            }

            //if (updateDetailList != null && updateDetailList.Count > 0)
            //    model.SaveOrUpdate(updateDetailList);
            //if (addGWBSDetailList != null && addGWBSDetailList.Count > 0)
            //    model.SaveOrUpdate(addGWBSDetailList);
            if (updateLeadNodeList != null && updateLeadNodeList.Count > 0)
                model.SaveOrUpdate(updateLeadNodeList);
            int index = dgProjectTaskDetail.CurrentRow.Index;
            //this.Close();
            //重新加载数据
            accountingGWBSDetailList = GetAccountingGWBSDetailList();

            sharingProjectsAmountList = GetSharingProjectsAmountList();

            PlanSelect.Visible = false;
            //cbBulkSharing.CheckedChanged -=new EventHandler(cbBulkSharing_CheckedChanged);
            //cbShowNoSharingDetail.CheckedChanged -= new EventHandler(cbShowNoSharingDetail_CheckedChanged);
            cbBulkSharing.Checked = false;
            cbShowNoSharingDetail.Checked = false;
            planProjectsAmountList = GetPlanProjectsAmountList();
            tvwGWBS.CollapseAll();
            if (planProjectsAmountList != null && planProjectsAmountList.Count > 0)
            {
                dgProjectTaskDetail.Rows.Clear();
                dgProjectTaskDetailInsert(planProjectsAmountList);
                dgProjectTaskDetail.CurrentCell = dgProjectTaskDetail[2, index];
                dgProjectTaskDetail_CellClick(dgProjectTaskDetail, new DataGridViewCellEventArgs(2, index));
            }
            //cbBulkSharing.CheckedChanged += new EventHandler(cbBulkSharing_CheckedChanged);
            //cbShowNoSharingDetail.CheckedChanged += new EventHandler(cbShowNoSharingDetail_CheckedChanged);
        }

        /// <summary>
        /// 添加资源耗用到工程任务明细
        /// </summary>
        /// <param name="quota"></param>
        private void AddResourceUsageInTaskDetail(ref GWBSDetail oprDtl, SubjectCostQuota quota)
        {
            GWBSDetailCostSubject subject = new GWBSDetailCostSubject();
            subject.Name = quota.Name;
            subject.MainResTypeFlag = quota.MainResourceFlag;

            subject.ProjectAmountUnitGUID = quota.ProjectAmountUnitGUID;
            subject.ProjectAmountUnitName = quota.ProjectAmountUnitName;

            subject.PriceUnitGUID = quota.PriceUnitGUID;
            subject.PriceUnitName = quota.PriceUnitName;

            //合同
            subject.ContractQuotaQuantity = quota.QuotaProjectAmount;
            subject.ContractQuantityPrice = quota.QuotaPrice;
            subject.ContractBasePrice = subject.ContractQuantityPrice;
            subject.ContractPricePercent = 1;
            subject.ContractPrice = subject.ContractQuotaQuantity * subject.ContractQuantityPrice;
            subject.ContractProjectAmount = addDetail.ContractProjectQuantity * subject.ContractQuotaQuantity;
            subject.ContractTotalPrice = subject.ContractProjectAmount * subject.ContractQuantityPrice;

            //责任
            subject.ResponsibleQuotaNum = quota.QuotaProjectAmount;
            subject.ResponsibilitilyPrice = quota.QuotaPrice;
            subject.ResponsibleBasePrice = subject.ResponsibilitilyPrice;
            subject.ResponsiblePricePercent = 1;
            subject.ResponsibleWorkPrice = subject.ResponsibleQuotaNum * subject.ResponsibilitilyPrice;
            subject.ResponsibilitilyWorkAmount = addDetail.ResponsibilitilyWorkAmount * subject.ResponsibleQuotaNum;
            subject.ResponsibilitilyTotalPrice = subject.ResponsibilitilyWorkAmount * subject.ResponsibilitilyPrice;//责任包干单价未知

            //计划
            subject.PlanQuotaNum = quota.QuotaProjectAmount;
            subject.PlanPrice = quota.QuotaPrice;
            subject.PlanPricePercent = 1;
            subject.PlanBasePrice = subject.PlanPrice;
            subject.PlanWorkPrice = subject.PlanQuotaNum * subject.PlanPrice;
            subject.PlanWorkAmount = addDetail.PlanWorkAmount * subject.PlanQuotaNum;
            subject.PlanTotalPrice = subject.PlanWorkAmount * subject.PlanPrice;//计划包干单价未知

            subject.AssessmentRate = quota.AssessmentRate;

            subject.ResourceUsageQuota = quota;

            if (quota.ListResources.Count > 0)
            {
                ResourceGroup itemResource = quota.ListResources.ElementAt(0);

                subject.ResourceTypeGUID = itemResource.ResourceTypeGUID;
                subject.ResourceTypeCode = itemResource.ResourceTypeCode;
                subject.ResourceTypeName = itemResource.ResourceTypeName;
                subject.ResourceTypeQuality = itemResource.ResourceTypeQuality;
                subject.ResourceTypeSpec = itemResource.ResourceTypeSpec;
                subject.ResourceCateSyscode = itemResource.ResourceCateSyscode;
                subject.DiagramNumber = itemResource.DiagramNumber;
            }

            subject.CostAccountSubjectGUID = quota.CostAccountSubjectGUID;
            subject.CostAccountSubjectName = quota.CostAccountSubjectName;
            if (quota.CostAccountSubjectGUID != null)
                subject.CostAccountSubjectSyscode = quota.CostAccountSubjectGUID.SysCode;

            subject.ProjectAmountWasta = quota.Wastage;

            subject.State = GWBSDetailCostSubjectState.编制;

            subject.TheProjectGUID = addDetail.TheProjectGUID;
            subject.TheProjectName = addDetail.TheProjectName;

            subject.TheGWBSDetail = addDetail;

            subject.TheGWBSTree = addDetail.TheGWBS;
            subject.TheGWBSTreeName = addDetail.TheGWBS.Name;
            subject.TheGWBSTreeSyscode = addDetail.TheGWBS.SysCode;

            addDetail.ListCostSubjectDetails.Add(subject);
        }

        /// <summary>
        /// 添加资源耗用到工程任务明细
        /// </summary>
        /// <param name="quota"></param>
        private void AddResourceUsageInTaskDetail(GWBSDetailCostSubject cs)
        {
            GWBSDetailCostSubject subject = new GWBSDetailCostSubject();
            subject.Name = cs.Name;
            subject.MainResTypeFlag = cs.MainResTypeFlag;

            subject.ProjectAmountUnitGUID = cs.ProjectAmountUnitGUID;
            subject.ProjectAmountUnitName = cs.ProjectAmountUnitName;

            subject.PriceUnitGUID = cs.PriceUnitGUID;
            subject.PriceUnitName = cs.PriceUnitName;

            //合同
            subject.ContractQuotaQuantity = cs.ContractQuotaQuantity;
            subject.ContractQuantityPrice = cs.ContractQuantityPrice;
            subject.ContractBasePrice = cs.ContractBasePrice;
            subject.ContractPricePercent = 1;
            subject.ContractPrice = subject.ContractQuotaQuantity * subject.ContractQuantityPrice;
            subject.ContractProjectAmount = addDetail.ContractProjectQuantity * subject.ContractQuotaQuantity;
            subject.ContractTotalPrice = subject.ContractProjectAmount * subject.ContractQuantityPrice;

            //责任
            subject.ResponsibleQuotaNum = cs.ResponsibleQuotaNum;
            subject.ResponsibilitilyPrice = cs.ResponsibilitilyPrice;
            subject.ResponsibleBasePrice = subject.ResponsibilitilyPrice;
            subject.ResponsiblePricePercent = 1;
            subject.ResponsibleWorkPrice = subject.ResponsibleQuotaNum * subject.ResponsibilitilyPrice;
            subject.ResponsibilitilyWorkAmount = addDetail.ResponsibilitilyWorkAmount * subject.ResponsibleQuotaNum;
            subject.ResponsibilitilyTotalPrice = subject.ResponsibilitilyWorkAmount * subject.ResponsibilitilyPrice;//责任包干单价未知

            //计划
            subject.PlanQuotaNum = cs.PlanQuotaNum;
            subject.PlanPrice = cs.PlanPrice;
            subject.PlanPricePercent = 1;
            subject.PlanBasePrice = subject.PlanPrice;
            subject.PlanWorkPrice = subject.PlanQuotaNum * subject.PlanPrice;
            subject.PlanWorkAmount = addDetail.PlanWorkAmount * subject.PlanQuotaNum;
            subject.PlanTotalPrice = subject.PlanWorkAmount * subject.PlanPrice;//计划包干单价未知

            subject.AssessmentRate = cs.AssessmentRate;

            subject.ResourceUsageQuota = cs.ResourceUsageQuota;

            //if (quota.ListResources.Count > 0)
            //{
            //    ResourceGroup itemResource = quota.ListResources.ElementAt(0);

            subject.ResourceTypeGUID = cs.ResourceTypeGUID;
            subject.ResourceTypeCode = cs.ResourceTypeCode;
            subject.ResourceTypeName = cs.ResourceTypeName;
            subject.ResourceTypeQuality = cs.ResourceTypeQuality;
            subject.ResourceTypeSpec = cs.ResourceTypeSpec;
            subject.ResourceCateSyscode = cs.ResourceCateSyscode;
            subject.DiagramNumber = cs.DiagramNumber;
            //}

            subject.CostAccountSubjectGUID = cs.CostAccountSubjectGUID;
            subject.CostAccountSubjectName = cs.CostAccountSubjectName;
            //if (quota.CostAccountSubjectGUID != null)
            subject.CostAccountSubjectSyscode = cs.CostAccountSubjectSyscode;

            subject.ProjectAmountWasta = cs.ProjectAmountWasta;

            subject.State = GWBSDetailCostSubjectState.编制;

            subject.TheProjectGUID = addDetail.TheProjectGUID;
            subject.TheProjectName = addDetail.TheProjectName;

            subject.TheGWBSDetail = addDetail;

            subject.TheGWBSTree = addDetail.TheGWBS;
            subject.TheGWBSTreeName = addDetail.TheGWBS.Name;
            subject.TheGWBSTreeSyscode = addDetail.TheGWBS.SysCode;

            addDetail.ListCostSubjectDetails.Add(subject);
        }

        //GWBSDetail GetAccountingDtlByProduceDtl(GWBSDetail d)
        //{
        //    ObjectQuery oq = new ObjectQuery();
        //    oq.AddCriterion(Expression.Eq("State", DocumentState.InExecute));
        //    oq.AddCriterion(Expression.Eq("CostingFlag", 1));

        //    string[] sysCodes = d.TheGWBSSysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
        //    Disjunction dis = new Disjunction();
        //    for (int i = 0; i < sysCodes.Length; i++)
        //    {
        //        string sysCode = "";
        //        for (int j = 0; j <= i; j++)
        //        {
        //            sysCode += sysCodes[j] + ".";
        //        }

        //        dis.Add(Expression.Eq("TheGWBSSysCode", sysCode));
        //    }
        //    oq.AddCriterion(dis);
        //    oq.AddCriterion(Expression.Eq("ContractGroupGUID", d.ContractGroupGUID));
        //    if (d.MainResourceTypeId != null)
        //        oq.AddCriterion(Expression.Eq("MainResourceTypeId", d.MainResourceTypeId));
        //    else
        //        oq.AddCriterion(Expression.IsNull("MainResourceTypeId"));
        //    if (d.DiagramNumber != null)
        //        oq.AddCriterion(Expression.Eq("DiagramNumber", d.DiagramNumber));
        //    else
        //        oq.AddCriterion(Expression.IsNull("DiagramNumber"));
        //    oq.AddCriterion(Expression.Eq("TheCostItem.Id", d.TheCostItem.Id));

        //    IList accDtlList = model.ObjectQuery(typeof(GWBSDetail), oq);

        //    if (accDtlList != null && accDtlList.Count > 0)
        //    {
        //        return accDtlList[0] as GWBSDetail;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        GWBSDetail GetAccountingDtlByProduceDtl(GWBSDetail d)
        {
            foreach (GWBSDetail dtl in accountingGWBSDetailList)
            {
                if (dtl.TheCostItem.Id == d.TheCostItem.Id && dtl.MainResourceTypeId == d.MainResourceTypeId && dtl.DiagramNumber == d.DiagramNumber)
                {
                    return dtl;
                }
            }
            return null;
        }

        #region 全选 反选
        /// <summary>
        /// 反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkInverse_Click(object sender, EventArgs e)
        {
            //tvwGWBS.AfterCheck -= new TreeViewEventHandler(tvwGWBS_AfterCheck);
            //tvwGWBS.BeforeCheck -= new TreeViewCancelEventHandler(tvwGWBS_BeforeCheck);

            Inverse(tvwGWBS.Nodes[0]);

            //tvwGWBS.AfterCheck += new TreeViewEventHandler(tvwGWBS_AfterCheck);
            //tvwGWBS.BeforeCheck += new TreeViewCancelEventHandler(tvwGWBS_BeforeCheck);
        }
        void Inverse(TreeNode parentNodes)
        {
            foreach (TreeNode node in parentNodes.Nodes)
            {
                if (node.Checked)
                {
                    node.Checked = false;
                }
                Inverse(node);
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkCheckAll_Click(object sender, EventArgs e)
        {
            CheckAll(tvwGWBS.Nodes[0]);
        }
        void CheckAll(TreeNode parentNodes)
        {
            foreach (TreeNode node in parentNodes.Nodes)
            {
                if (!node.Checked)
                {
                    node.Checked = true;
                }
                CheckAll(node);
            }
        }
        #endregion

        void EnabledTvwGWBSCheckBox()
        {
            foreach (TreeNode node in tvwGWBS.Nodes)
            {
                GWBSTree t = node.Tag as GWBSTree;
                if (t.CategoryNodeType != VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode)
                {
                    node.ForeColor = SystemColors.Control;
                }
            }
        }

        void tvwGWBS_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node != null)
            {
                //MessageBox.Show(e.Node.Text);
                GWBSTree t = e.Node.Tag as GWBSTree;
                if (t.CategoryNodeType != VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode)
                {
                    e.Cancel = true;
                }
            }
        }

        void tvwGWBS_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node != null)
            {
                GWBSTree t = e.Node.Tag as GWBSTree;
                if (t.CategoryNodeType != VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode || operatePlanProjectAmount == null)
                {
                    e.Cancel = true;
                }
            }
        }

        void tvwGWBS_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                GWBSTree t = e.Node.Tag as GWBSTree;
                string leadNodeFullPath = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), t.Name, t.SysCode);
                string leadNodePath = leadNodeFullPath.Replace(nodeFullPath, "");

                if (operatePlanProjectAmount != null)
                {
                    IList list = new ArrayList();
                    //DataGridViewCell cell = dgSubordinatesProductionNode.Rows[e.RowIndex].Cells[colSelect.Name];
                    //bool clickAfter = Convert.ToBoolean(cell.EditedFormattedValue);
                    if (e.Node.Checked == true)
                    {
                        #region 新增
                        //GWBSTree t = dgSubordinatesProductionNode.Rows[e.RowIndex].Tag as GWBSTree;

                        bool flag = true;
                        for (int i = 0; i < dgSharingProjectAmount.Rows.Count; i++)
                        {
                            string path = dgSharingProjectAmount[SharingLeafNodePath.Name, i].Value.ToString();
                            if (leadNodePath == path)
                            {
                                SharingProjectsAmountValueObj spa = dgSharingProjectAmount.Rows[i].Tag as SharingProjectsAmountValueObj;
                                spa.Showflag = true;
                                dgSharingProjectAmount.Rows[i].Tag = spa;

                                dgSharingProjectAmount[SharingTheSharingProjectAmount.Name, i].ReadOnly = false;
                                dgSharingProjectAmount[SharingTheSharingProjectAmount.Name, i].Style.BackColor = Color.White;
                                flag = false;
                            }

                        }

                        //if (sharingProjectsAmountList != null && sharingProjectsAmountList.Count > 0)
                        //{
                        //    foreach (SharingProjectsAmountValueObj spa in sharingProjectsAmountList)
                        //    {
                        //        if (spa.LeafNodePath == leadNodePath)
                        //            list.Add(spa);
                        //    }
                        //}


                        //if (list != null && list.Count > 0)
                        //{
                        //    //dgSharingProjectAmount.Rows.Clear();
                        //    List<int> removeRowIndex = new List<int>();
                        //    foreach (DataGridViewRow row in dgSharingProjectAmount.Rows)
                        //    {
                        //        string path = row.Cells[SharingLeafNodePath.Name].Value.ToString();
                        //        if (path == leadNodePath)
                        //        {
                        //            removeRowIndex.Add(row.Index);
                        //        }
                        //    }
                        //    if (removeRowIndex.Count > 0)
                        //    {
                        //        int remountCount = 0;
                        //        foreach (int index in removeRowIndex)
                        //        {
                        //            dgSharingProjectAmount.Rows.RemoveAt(index - remountCount);
                        //            remountCount++;
                        //        }
                        //    }
                        //    foreach (SharingProjectsAmountValueObj spa in list)
                        //    {
                        //        int rowIndex = dgSharingProjectAmount.Rows.Add();
                        //        dgSharingProjectAmount[SharingLeafNodePath.Name, rowIndex].Value = spa.LeafNodePath;
                        //        dgSharingProjectAmount[SharingGWBSDetail.Name, rowIndex].Value = spa.ProjectTaskDetaikName;
                        //        dgSharingProjectAmount[SharingExistProjectAmount.Name, rowIndex].Value = spa.ExistingProjectAmount.ToString();
                        //        dgSharingProjectAmount[SharingTheSharingProjectAmount.Name, rowIndex].Value = spa.TheSharingSummaryAmount.ToString();
                        //        dgSharingProjectAmount[SharingTheSharingProjectAmount.Name, rowIndex].ReadOnly = false;
                        //        dgSharingProjectAmount.Rows[rowIndex].Tag = spa;
                        //    }
                        //}
                        if (flag)
                        {
                            SharingProjectsAmountValueObj spa = new SharingProjectsAmountValueObj();
                            PlanProjectsAmountValueObj ppa = dgProjectTaskDetail.SelectedRows[0].Tag as PlanProjectsAmountValueObj;
                            spa.LeafNodePath = leadNodePath;
                            spa.Node = t;
                            spa.ProjectTaskDetaikName = ppa.ProjectTaskDetaikName;

                            //if (ppa.MainResourceTypeId != null)
                            spa.MainResourceTypeId = ppa.MainResourceTypeId;
                            spa.DiagramNumber = ppa.DiagramNumber;
                            spa.CostItems = ppa.CostItems;
                            spa.ExistingProjectAmount = 0;
                            spa.TheSharingSummaryAmount = 0;
                            sharingProjectsAmountList.Add(spa);

                            int rowIndex = dgSharingProjectAmount.Rows.Add();
                            dgSharingProjectAmount[SharingLeafNodePath.Name, rowIndex].Value = spa.LeafNodePath;
                            dgSharingProjectAmount[SharingGWBSDetail.Name, rowIndex].Value = spa.ProjectTaskDetaikName;
                            dgSharingProjectAmount[SharingExistProjectAmount.Name, rowIndex].Value = spa.ExistingProjectAmount.ToString();
                            dgSharingProjectAmount[SharingTheSharingProjectAmount.Name, rowIndex].Value = spa.TheSharingSummaryAmount.ToString();
                            dgSharingProjectAmount[SharingQuotaCode.Name, rowIndex].Value = spa.CostItems.QuotaCode;
                            //dgSharingProjectAmount[SharingTheSharingProjectAmount.Name, rowIndex].ReadOnly = false ;
                            dgSharingProjectAmount.Rows[rowIndex].Tag = spa;
                        }
                        #endregion
                    }
                    else
                    {
                        #region 修改
                        //List<SharingProjectsAmountValueObj> removeList = new List<SharingProjectsAmountValueObj>();
                        List<int> removeRowIndex = new List<int>();
                        //int rowIndex = -1;
                        //GWBSTree t = dgSubordinatesProductionNode.Rows[e.RowIndex].Tag as GWBSTree;
                        //string leadNodeFullPath = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), t.Name, t.SysCode);
                        //string leadNodePath = leadNodeFullPath.Replace(nodeFullPath, "");

                        for (int i = 0; i < dgSharingProjectAmount.Rows.Count; i++)
                        {
                            SharingProjectsAmountValueObj spa = dgSharingProjectAmount.Rows[i].Tag as SharingProjectsAmountValueObj;
                            //string path = dgSharingProjectAmount.Rows[i].Cells[SharingLeafNodePath.Name].Value.ToString();
                            if (spa.LeafNodePath == leadNodePath)
                            {
                                if (spa.ExistingProjectAmount != 0)
                                {
                                    decimal num = ClientUtil.ToDecimal(dgSharingProjectAmount.Rows[i].Cells[SharingTheSharingProjectAmount.Name].Value);
                                    if (num != 0)
                                    {
                                        PlanProjectsAmountValueObj ppa = dgProjectTaskDetail.SelectedRows[0].Tag as PlanProjectsAmountValueObj;
                                        UpdateProjectTaskDetail(ppa, num);
                                    }

                                    dgSharingProjectAmount.Rows[i].Cells[SharingTheSharingProjectAmount.Name].Value = 0;
                                    dgSharingProjectAmount.Rows[i].Cells[SharingTheSharingProjectAmount.Name].ReadOnly = true;
                                    dgSharingProjectAmount.Rows[i].Cells[SharingTheSharingProjectAmount.Name].Style.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                                    spa.TheSharingSummaryAmount = 0;
                                    spa.Showflag = false;
                                }
                                else
                                {
                                    sharingProjectsAmountList.Remove(spa);
                                    decimal num = ClientUtil.ToDecimal(dgSharingProjectAmount.Rows[i].Cells[SharingTheSharingProjectAmount.Name].Value);
                                    if (num != 0)
                                    {
                                        PlanProjectsAmountValueObj ppa = dgProjectTaskDetail.SelectedRows[0].Tag as PlanProjectsAmountValueObj;
                                        UpdateProjectTaskDetail(ppa, num);
                                    }
                                    removeRowIndex.Add(i);
                                }


                            }
                        }
                        //for (int i = 0; i < sharingProjectsAmountList.Count; i++)
                        //{
                        //    SharingProjectsAmountValueObj spa = sharingProjectsAmountList[i] as SharingProjectsAmountValueObj;
                        //    //foreach (SharingProjectsAmountValueObj spa in sharingProjectsAmountList)
                        //    //{
                        //    if (spa.LeafNodePath == leadNodePath)
                        //    {
                        //        if (spa.ExistingProjectAmount != 0)
                        //        {
                        //            foreach (DataGridViewRow row in dgSharingProjectAmount.Rows)
                        //            {
                        //                SharingProjectsAmountValueObj s = row.Tag as SharingProjectsAmountValueObj;
                        //                if (s == spa && row.Cells[SharingTheSharingProjectAmount.Name].ReadOnly == false)
                        //                {
                        //                    row.Cells[SharingTheSharingProjectAmount.Name].Value = 0;
                        //                    row.Cells[SharingTheSharingProjectAmount.Name].ReadOnly = true;
                        //                }
                        //                spa.TheSharingSummaryAmount = 0;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            foreach (DataGridViewRow row in dgSharingProjectAmount.Rows)
                        //            {
                        //                SharingProjectsAmountValueObj s = row.Tag as SharingProjectsAmountValueObj;
                        //                if (s == spa && row.Cells[SharingExistProjectAmount.Name].Value.ToString() == "0")
                        //                {
                        //                    rowIndex = row.Index;
                        //                }
                        //            }
                        //            removeList.Add(spa);
                        //            if (rowIndex >= 0)
                        //            {
                        //                dgSharingProjectAmount.Rows.RemoveAt(rowIndex);
                        //                rowIndex = -1;
                        //            }
                        //        }

                        //    }
                        //}
                        //if (removeList != null && removeList.Count > 0)
                        //{
                        //    foreach (SharingProjectsAmountValueObj spa in removeList)
                        //    {
                        //        sharingProjectsAmountList.Remove(spa);
                        //    }
                        //}
                        if (removeRowIndex != null && removeRowIndex.Count > 0)
                        {
                            int index = 0;
                            foreach (int rowIndex in removeRowIndex)
                            {
                                dgSharingProjectAmount.Rows.RemoveAt(rowIndex - index);
                                index++;
                            }
                        }
                        #endregion
                    }
                }
            }
        }

        #region 批量分摊 多个核算明细分摊在同一个生产节点下
        void cbBulkSharing_CheckedChanged(object sender, EventArgs e)
        {
            if (cbBulkSharing.Checked)
            {
                PlanSelect.Visible = true;
                dgProjectTaskDetail.RowHeadersVisible = false;
                dgProjectTaskDetail.AddNoColumn = false;
                tvwGWBS.CheckBoxes = false;
                tvwGWBS.SelectedNode = null;

                lnkCheckAll.Enabled = false;
                lnkInverse.Enabled = false;

                dgProjectTaskDetail.CellClick -= new DataGridViewCellEventHandler(dgProjectTaskDetail_CellClick);
                dgProjectTaskDetail.CellMouseDown -= new DataGridViewCellMouseEventHandler(dgProjectTaskDetail_CellMouseDown);
                cmuGWBSDetailMouseRight.ItemClicked -= new ToolStripItemClickedEventHandler(cmuGWBSDetailMouseRight_ItemClicked);
                tvwGWBS.BeforeSelect -= new TreeViewCancelEventHandler(tvwGWBS_BeforeSelect);
                tvwGWBS.BeforeCheck -= new TreeViewCancelEventHandler(tvwGWBS_BeforeCheck);
                tvwGWBS.AfterCheck -= new TreeViewEventHandler(tvwGWBS_AfterCheck);
                tvwGWBS.AfterSelect += new TreeViewEventHandler(tvwGWBS_AfterSelect);

                dgSharingProjectAmount.Rows.Clear();
                foreach (DataGridViewRow row in dgProjectTaskDetail.Rows)
                {
                    decimal sharingSummaryAmount = Convert.ToDecimal(row.Cells[PlanSharingSummaryAmount.Name].EditedFormattedValue.ToString());
                    if (sharingSummaryAmount == 0)
                    {
                        row.Cells[PlanSelect.Name].Value = false;
                        row.Cells[PlanSelect.Name].ReadOnly = false;
                    }
                    else
                    {
                        row.Cells[PlanSelect.Name].Value = false;
                        row.Cells[PlanSelect.Name].ReadOnly = true;
                    }
                }
            }
            else
            {
                PlanSelect.Visible = false;
                dgProjectTaskDetail.RowHeadersVisible = true;
                dgProjectTaskDetail.AddNoColumn = true;
                tvwGWBS.CheckBoxes = true;

                lnkCheckAll.Enabled = false;
                lnkInverse.Enabled = false;

                dgProjectTaskDetail.CellClick += new DataGridViewCellEventHandler(dgProjectTaskDetail_CellClick);
                dgProjectTaskDetail.CellMouseDown += new DataGridViewCellMouseEventHandler(dgProjectTaskDetail_CellMouseDown);
                cmuGWBSDetailMouseRight.ItemClicked += new ToolStripItemClickedEventHandler(cmuGWBSDetailMouseRight_ItemClicked);
                tvwGWBS.BeforeSelect += new TreeViewCancelEventHandler(tvwGWBS_BeforeSelect);
                tvwGWBS.BeforeCheck += new TreeViewCancelEventHandler(tvwGWBS_BeforeCheck);
                tvwGWBS.AfterCheck += new TreeViewEventHandler(tvwGWBS_AfterCheck);
                tvwGWBS.AfterSelect -= new TreeViewEventHandler(tvwGWBS_AfterSelect);


                dgProjectTaskDetail_CellClick(dgProjectTaskDetail, new DataGridViewCellEventArgs(0, dgProjectTaskDetail.SelectedRows[0].Index));
            }
            //tvwGWBS.ExpandAll();
        }

        void tvwGWBS_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (cbBulkSharing.Checked)
            {
                if (e.Node != null)
                {
                    GWBSTree t = e.Node.Tag as GWBSTree;
                    if (t.CategoryNodeType == VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode)
                    {
                        dgSharingProjectAmount.Rows.Clear();
                        foreach (DataGridViewRow row in dgProjectTaskDetail.Rows)
                        {
                            if ((bool)row.Cells[PlanSelect.Name].EditedFormattedValue)
                            {
                                SharingProjectsAmountValueObj spa = new SharingProjectsAmountValueObj();
                                PlanProjectsAmountValueObj ppa = row.Tag as PlanProjectsAmountValueObj;
                                spa.LeafNodePath = "\\" + t.Name;
                                spa.Node = t;
                                spa.ProjectTaskDetaikName = ppa.ProjectTaskDetaikName;

                                //if (ppa.MainResourceTypeId != null)
                                spa.MainResourceTypeId = ppa.MainResourceTypeId;
                                spa.DiagramNumber = ppa.DiagramNumber;
                                spa.CostItems = ppa.CostItems;
                                spa.ExistingProjectAmount = 0;
                                spa.TheSharingSummaryAmount = ppa.PlanProjectAmount;

                                int rowIndex = dgSharingProjectAmount.Rows.Add();
                                dgSharingProjectAmount[SharingLeafNodePath.Name, rowIndex].Value = spa.LeafNodePath;
                                dgSharingProjectAmount[SharingGWBSDetail.Name, rowIndex].Value = spa.ProjectTaskDetaikName;
                                dgSharingProjectAmount[SharingExistProjectAmount.Name, rowIndex].Value = spa.ExistingProjectAmount.ToString();
                                dgSharingProjectAmount[SharingTheSharingProjectAmount.Name, rowIndex].Value = spa.TheSharingSummaryAmount.ToString();
                                dgSharingProjectAmount[SharingQuotaCode.Name, rowIndex].Value = spa.CostItems.QuotaCode;
                                dgSharingProjectAmount.Rows[rowIndex].Tag = spa;
                                dgSharingProjectAmount.Rows[rowIndex].ReadOnly = true;
                            }
                        }
                    }
                }
            }
        }

        #endregion

    }
}
