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
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VSelectGWBSDetail : TBasicDataView
    {
        private GWBSTree oprNode = null;

        //有权限的GWBSTree
        private IList lstInstance;
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();
        private List<GWBSDetail> _selectGWBSDetail = new List<GWBSDetail>();
        /// <summary>
        /// 选择的工程任务明细集合
        /// </summary>
        public List<GWBSDetail> SelectGWBSDetail
        {
            get { return _selectGWBSDetail; }
            set { _selectGWBSDetail = value; }
        }


        public Dictionary<string, object> InitCondition = new Dictionary<string, object>();

        private bool _isOk;

        /// <summary>
        /// 是否点击选择
        /// </summary>
        public bool IsOk
        {
            get { return _isOk; }
            set { _isOk = value; }
        }

        public MGWBSTree model;

        public VSelectGWBSDetail(MGWBSTree mot)
        {
            model = mot;
            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();

            txtNoDtlColorFlag.BackColor = ColorTranslator.FromHtml("#D7E8FE");

            tvwCategory.CheckBoxes = false;

            projectInfo = StaticMethod.GetProjectInfo();

            LoadCategoryTreeRootNode();
        }

        private void InitEvents()
        {
            tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);
            tvwCategory.AfterExpand += new TreeViewEventHandler(tvwCategory_AfterExpand);

            txtTaskCostItem.LostFocus += new EventHandler(txtCostItemBySearch_LostFocus);
            btnSelectCostItemBySearch.Click += new EventHandler(btnSelectCostItemBySearch_Click);
            gridQueryDetail.CellDoubleClick += new DataGridViewCellEventHandler(gridQueryDetail_CellDoubleClick);

            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnAddToSelectList.Click += new EventHandler(btnAddToSelectList_Click);
            btnRemoveSelectDtl.Click += new EventHandler(btnRemoveSelectDtl_Click);

            btnSelect.Click += new EventHandler(btnSelect_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            this.FormClosing += new FormClosingEventHandler(VSelectGWBSDetail_FormClosing);
        }

        void txtCostItemBySearch_LostFocus(object sender, EventArgs e)
        {
            string costItemCode = txtTaskCostItem.Text.Trim();
            if (string.IsNullOrEmpty(costItemCode))
            {
                txtTaskCostItem.Tag = null;
                return;
            }

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Or(Expression.Eq("Code", costItemCode), Expression.Eq("QuotaCode", costItemCode)));
            IList list = model.ObjectQuery(typeof(CostItem), oq);
            if (list != null && list.Count > 0)
            {
                CostItem item = list[0] as CostItem;
                txtTaskCostItem.Tag = item;
            }
            else
            {
                txtTaskCostItem.Tag = null;
                MessageBox.Show("请检查输入的成本项编号或定额编号，未找到相关成本项！");
                txtTaskCostItem.Focus();
            }
        }

        void btnSelectCostItemBySearch_Click(object sender, EventArgs e)
        {
            VSelectCostItem frm = new VSelectCostItem(new MCostItem());
            if (txtTaskCostItem.Text.Trim() != "" && txtTaskCostItem.Tag != null)
            {
                frm.DefaultSelectedCostItem = txtTaskCostItem.Tag as CostItem;
            }
            frm.ShowDialog();
            if (frm.SelectCostItem != null)
            {
                if (!string.IsNullOrEmpty(frm.SelectCostItem.QuotaCode))
                    txtTaskCostItem.Text = frm.SelectCostItem.QuotaCode;
                else
                    txtTaskCostItem.Text = frm.SelectCostItem.Name;

                txtTaskCostItem.Tag = frm.SelectCostItem;
            }
        }
        void btnSelect_Click(object sender, EventArgs e)
        {
            if (gridSelectDetail.Rows.Count == 0)
            {
                MessageBox.Show("请至少选择一条任务明细！");
                return;
            }

            foreach (DataGridViewRow row in gridSelectDetail.Rows)
            {
                _selectGWBSDetail.Add(row.Tag as GWBSDetail);
            }

            _isOk = true;

            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            _selectGWBSDetail.Clear();
            this.Close();
        }

        void VSelectGWBSDetail_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_isOk)
                _selectGWBSDetail.Clear();
        }

        void btnRemoveSelectDtl_Click(object sender, EventArgs e)
        {
            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridSelectDetail.SelectedRows)
            {
                listRowIndex.Add(row.Index);
            }
            listRowIndex.Sort();

            for (int i = listRowIndex.Count - 1; i > -1; i--)
            {
                gridSelectDetail.Rows.RemoveAt(listRowIndex[i]);
            }
        }

        void btnAddToSelectList_Click(object sender, EventArgs e)
        {
            if (gridQueryDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择添加的行！");
                return;
            }

            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridQueryDetail.SelectedRows)
            {
                listRowIndex.Add(row.Index);
            }

            listRowIndex.Sort();
            for (int i = listRowIndex.Count - 1; i > -1; i--)
            {
                GWBSDetail dtl = gridQueryDetail.Rows[listRowIndex[i]].Tag as GWBSDetail;
                AddDetailInfoInSelectGrid(dtl);

                //GWBSDetail dtl = gridQueryDetail.Rows[listRowIndex[i]].Tag as GWBSDetail;

                //ObjectQuery oq = new ObjectQuery();
                //oq.AddCriterion(NHibernate.Criterion.Expression.Eq("Id", dtl.Id));
                //oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
                //IList listDtl = model.ObjectQuery(typeof(GWBSDetail), oq);
                //dtl = listDtl[0] as GWBSDetail;

                //AddDetailInfoInSelectGrid(dtl);

                gridQueryDetail.Rows.RemoveAt(listRowIndex[i]);
            }
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            string dtlName = txtTaskName.Text.Trim();
            string dtlDesc = txtTaskDesc.Text.Trim();

            CostItem costItem = null;
            if (txtTaskCostItem.Text.Trim() != "")
                costItem = txtTaskCostItem.Tag as CostItem;

            if (dtlName == "" && dtlDesc == "" && costItem == null)
            {
                if (MessageBox.Show("没有查询条件会降慢查询速度，确定要继续吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    txtTaskCostItem.Focus();
                    return;
                }
            }

            try
            {
                FlashScreen.Show("正在查询加载数据,请稍候......");

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));

                if (InitCondition.Count > 0)
                {
                    foreach (var obj in InitCondition)
                    {
                        oq.AddCriterion(Expression.Eq(obj.Key, obj.Value));
                    }
                }

                if (costItem != null)
                    oq.AddCriterion(Expression.Eq("TheCostItem.Id", costItem.Id));

                if (dtlName != "")
                    oq.AddCriterion(Expression.Like("Name", dtlName, MatchMode.Anywhere));

                if (dtlDesc != "")
                    oq.AddCriterion(Expression.Like("ContentDesc", dtlDesc, MatchMode.Anywhere));


                oq.AddOrder(NHibernate.Criterion.Order.Asc("CreateTime"));
                oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);

                IList list = model.ObjectQuery(typeof(GWBSDetail), oq);

                gridQueryDetail.Rows.Clear();
                foreach (GWBSDetail dtl in list)
                {
                    AddDetailInfoInGrid(dtl);
                }

            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("查询出错，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        //双击查看成本项
        void gridQueryDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            btnAddToSelectList_Click(btnAddToSelectList, new EventArgs());
        }

        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                oprNode = tvwCategory.SelectedNode.Tag as GWBSTree;

                LoadGWBSDetailInGrid();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
            }
        }
        private void tvwCategory_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null || (e.Node.Tag as GWBSTree) == null)
                return;

            TreeNode tempNode = new TreeNode();

            string countSql = "select distinct ParentId from THD_GWBSDetail where ParentId in ( ";
            string where = "";
            foreach (TreeNode tn in e.Node.Nodes)
            {
                where += "'" + tn.Name + "',";
            }
            countSql += where.Substring(0, where.Length - 1) + ")";
          
            DataSet ds = model.SearchSQL(countSql);
            DataTable dt = ds.Tables[0];

            List<string> listParentId = new List<string>();

            foreach (DataRow row in dt.Rows)
            {
                string parentId = row[0].ToString();
                listParentId.Add(parentId);
            }

            foreach (TreeNode tn in e.Node.Nodes)
            {
                if (listParentId.Contains(tn.Name) == false)
                {
                    tn.BackColor = tempNode.BackColor;
                    tn.ForeColor = tempNode.ForeColor;
                }
                else
                {
                    tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    tn.ForeColor = ColorTranslator.FromHtml("#000000");  
                }
            }

            LoadSecondLevelCategoryByParentNode(e.Node);
        }

        private void LoadGWBSDetailInGrid()
        {
            if (oprNode != null)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheGWBS.Id", oprNode.Id));
                if (InitCondition.Count > 0)
                {
                    foreach (var obj in InitCondition)
                    {
                        oq.AddCriterion(Expression.Eq(obj.Key, obj.Value));
                    }
                }
                oq.AddOrder(NHibernate.Criterion.Order.Asc("CreateTime"));
                oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);

                IList list = model.ObjectQuery(typeof(GWBSDetail), oq);

                gridQueryDetail.Rows.Clear();
                foreach (GWBSDetail dtl in list)
                {
                    AddDetailInfoInGrid(dtl);
                }
                gridQueryDetail.ClearSelection();
            }
        }

        public void Start()
        {
            RefreshState(MainViewState.Browser);

            LoadGWBSTreeTree();
        }

        private void LoadGWBSTreeTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
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
                MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }
        private void LoadCategoryTreeRootNode()
        {
            try
            {
                tvwCategory.Nodes.Clear();

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
                oq.AddCriterion(Expression.Eq("State", 1));
                //oq.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
                oq.AddCriterion(Expression.Eq("Level", 1));

                IList list = model.ObjectQuery(typeof(GWBSTree), oq);
                if (list.Count > 0)
                {
                    GWBSTree childNode = list[0] as GWBSTree;

                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = childNode.Name;
                    tnTmp.Tag = childNode;


                    tvwCategory.Nodes.Add(tnTmp);

                    this.tvwCategory.SelectedNode = this.tvwCategory.Nodes[0];
                    //this.tvwCategory.SelectedNode.Expand();


                    //加载下一层子节点
                    LoadCategoryByParentNode(tnTmp);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("加载成本项分类出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }
        private void LoadCategoryByParentNode(TreeNode parentNode)
        {
            try
            {
                GWBSTree parentCate = parentNode.Tag as GWBSTree;
                if (parentCate == null)
                    return;

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("State", 1));
                oq.AddCriterion(Expression.Eq("ParentNode.Id", parentCate.Id));
                oq.AddOrder(NHibernate.Criterion.Order.Asc("OrderNo"));

                IList list = model.ObjectQuery(typeof(GWBSTree), oq);
                if (list.Count > 0)
                {
                    foreach (GWBSTree childNode in list)
                    {
                        TreeNode tnTmp = new TreeNode();
                        tnTmp.Name = childNode.Id.ToString();
                        tnTmp.Text = childNode.Name;
                        tnTmp.Tag = childNode;

                        parentNode.Nodes.Add(tnTmp);

                        //this.tvwCategory.SelectedNode = this.tvwCategory.Nodes[0];
                        //this.tvwCategory.SelectedNode.Expand();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("加载成本项分类出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }
        private void LoadSecondLevelCategoryByParentNode(TreeNode firstParentNode)
        {
            try
            {
                if (firstParentNode.Nodes.Count == 0)
                    return;

                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();

                foreach (TreeNode childNode in firstParentNode.Nodes)
                {
                    if ((childNode.Tag as GWBSTree) != null)
                        dis.Add(Expression.Eq("ParentNode.Id", (childNode.Tag as GWBSTree).Id));
                }
                oq.AddCriterion(dis);

                oq.AddCriterion(Expression.Eq("State", 1));
                oq.AddOrder(NHibernate.Criterion.Order.Asc("OrderNo"));

                IList list = model.ObjectQuery(typeof(GWBSTree), oq);
                if (list.Count > 0)
                {
                    foreach (TreeNode parentNode in firstParentNode.Nodes)
                    {
                        GWBSTree parentCate = parentNode.Tag as GWBSTree;
                        if (parentCate == null)
                            continue;
                        parentNode.Nodes.Clear();
                        foreach (GWBSTree childNode in list)
                        {
                            if (childNode.ParentNode.Id != parentCate.Id)
                                continue;

                            TreeNode tnTmp = new TreeNode();
                            tnTmp.Name = childNode.Id.ToString();
                            tnTmp.Text = childNode.Name;
                            tnTmp.Tag = childNode;

                            parentNode.Nodes.Add(tnTmp);

                            //this.tvwCategory.SelectedNode = this.tvwCategory.Nodes[0];
                            //this.tvwCategory.SelectedNode.Expand();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("加载成本项分类出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        private void AddDetailInfoInGrid(GWBSDetail dtl)
        {
            if (dtl.State != VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute)
                return;
            int index = gridQueryDetail.Rows.Add();
            DataGridViewRow row = gridQueryDetail.Rows[index];
            row.Cells[DtlTaskName.Name].Value = dtl.TheGWBS.Name;
            row.Cells[DtlName.Name].Value = dtl.Name;

            if (dtl.TheCostItem != null)
            {
                row.Cells[DtlCostItem.Name].Value = dtl.TheCostItem.Name;
                row.Cells[DtlCostItemQuota.Name].Value = dtl.TheCostItem.QuotaCode;
            }

            row.Cells[DtlContractGroupCode.Name].Value = dtl.ContractGroupCode;
            row.Cells[DtlContractGroupName.Name].Value = dtl.ContractGroupName;

            row.Cells[DtlResourceName.Name].Value = dtl.MainResourceTypeName;
            row.Cells[DtlResourceQuality.Name].Value = dtl.MainResourceTypeQuality;
            row.Cells[DtlResourceSpec.Name].Value = dtl.MainResourceTypeSpec;

            row.Cells[DtlState.Name].Value =StaticMethod.GetWBSTaskStateText( dtl.State);
            row.Cells[DtlDesc.Name].Value = dtl.ContentDesc;

            row.Tag = dtl;

            gridQueryDetail.CurrentCell = row.Cells[0];
        }

        private void AddDetailInfoInSelectGrid(GWBSDetail dtl)
        {
            int index = gridSelectDetail.Rows.Add();
            DataGridViewRow row = gridSelectDetail.Rows[index];
            row.Cells[selTaskName.Name].Value = dtl.TheGWBS.Name;
            row.Cells[selDtlName.Name].Value = dtl.Name;
            if (dtl.TheCostItem != null)
            {
                row.Cells[selCostItem.Name].Value = dtl.TheCostItem.Name;
                row.Cells[selCostItemQuota.Name].Value = dtl.TheCostItem.QuotaCode;
            }

            row.Cells[selContractCode.Name].Value = dtl.ContractGroupCode;
            row.Cells[selContractName.Name].Value = dtl.ContractGroupName;

            row.Cells[selResourceName.Name].Value = dtl.MainResourceTypeName;
            row.Cells[selResourceQuality.Name].Value = dtl.MainResourceTypeQuality;
            row.Cells[selResourceSpec.Name].Value = dtl.MainResourceTypeSpec;

            row.Cells[selState.Name].Value =StaticMethod.GetWBSTaskStateText( dtl.State);
            row.Cells[selDesc.Name].Value = dtl.ContentDesc;

            row.Tag = dtl;

            gridSelectDetail.CurrentCell = row.Cells[0];
        }

        private void AddDetailInfoInSelectGrid(DataGridViewRow selRow)
        {
            GWBSDetail dtl = selRow.Tag as GWBSDetail;

            int index = gridSelectDetail.Rows.Add();
            DataGridViewRow row = gridSelectDetail.Rows[index];
            row.Cells[selTaskName.Name].Value = dtl.TheGWBS.Name;
            row.Cells[selDtlName.Name].Value = dtl.Name;
            if (dtl.TheCostItem != null)
            {
                row.Cells[selCostItem.Name].Value = dtl.TheCostItem.Name;
                row.Cells[selCostItemQuota.Name].Value = dtl.TheCostItem.QuotaCode;
            }

            row.Cells[selContractName.Name].Value = dtl.ContractGroupName;
            row.Cells[selContractCode.Name].Value = dtl.ContractGroupCode;

            row.Cells[selResourceName.Name].Value = dtl.MainResourceTypeName;
            row.Cells[selResourceQuality.Name].Value = dtl.MainResourceTypeQuality;
            row.Cells[selResourceSpec.Name].Value = dtl.MainResourceTypeSpec;

            row.Cells[selState.Name].Value = StaticMethod.GetWBSTaskStateText(dtl.State);
            row.Cells[selDesc.Name].Value = dtl.ContentDesc;

            row.Tag = dtl;

            gridSelectDetail.CurrentCell = row.Cells[0];
        }
    }
}