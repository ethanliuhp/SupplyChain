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
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng
{
    public partial class VSelectCostItem : TBasicDataView
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public OptionTypeEnum OptType = OptionTypeEnum.选择成本项;

        private CostItem _selectCostItem = null;
        /// <summary>
        /// 获取选择的成本项
        /// </summary>
        public CostItem SelectCostItem
        {
            get { return _selectCostItem; }
            //set { _selectCostItem = value; }
        }
        private IList selectCostItemList = null;
        /// <summary>
        /// 获取选择的成本项(多选)
        /// </summary>
        public IList SelectCostItemList
        {
            get { return selectCostItemList; }
            set { selectCostItemList = value; }
        }
        private CostItemCategory oprNode = null;
        //有权限的业务组织
        private IList lstInstance;

        /// <summary>
        ///缺省选择的成本项
        /// </summary>
        public CostItem DefaultSelectedCostItem = null;
        private string DefaultCateSyscode = string.Empty;
        public MCostItem model;
        private bool isSelectSingle = true;//是否单选成本项
        public VSelectCostItem(MCostItem mot)
        {
            model = mot;
            InitializeComponent();
            txtNoDtlColorFlag.BackColor = ColorTranslator.FromHtml("#D7E8FE");
            InitForm();
        }
        public VSelectCostItem(MCostItem mot, bool selectSingle)
        {
            model = mot;
            InitializeComponent();
            isSelectSingle = selectSingle;
            txtNoDtlColorFlag.BackColor = ColorTranslator.FromHtml("#D7E8FE");
            InitForm();
        }
        public VSelectCostItem(MCostItem mot, string syscode)
        {
            model = mot;
            DefaultCateSyscode = syscode;
            InitializeComponent();
            txtNoDtlColorFlag.BackColor = ColorTranslator.FromHtml("#D7E8FE");
            InitForm();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mot"></param>
        /// <param name="syscode">默认区域关联的成本项分类层次码</param>
        /// <param name="selectSingle">是否单选</param>
        public VSelectCostItem(MCostItem mot, string syscode, bool selectSingle)
        {
            model = mot;
            DefaultCateSyscode = syscode;
            InitializeComponent();
            isSelectSingle = selectSingle;
            txtNoDtlColorFlag.BackColor = ColorTranslator.FromHtml("#D7E8FE");
            InitForm();
        }
        private void InitForm()
        {
            if (!isSelectSingle)
            {
                ItemSelect.Visible = true;
                chkAllChecked.Visible = true;
            }
            LoadCostItemCategoryTreeRootNode();

            InitEvents();
            //LoadCostItemCategoryTree();
        }

        private void InitEvents()
        {
            if (OptType == OptionTypeEnum.查看成本项)
            {
                btnEnter.Enabled = false;
            }

            tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);
            tvwCategory.AfterExpand += new TreeViewEventHandler(tvwCategory_AfterExpand);


            gridCostItem.CellClick += new DataGridViewCellEventHandler(gridCostItem_CellClick);

            btnEnter.Click += new EventHandler(btnEnter_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            this.Load += new EventHandler(VSelectCostItem_Load);
            this.FormClosing += new FormClosingEventHandler(VSelectCostItem_FormClosing);

            this.btnSearch.Click += new EventHandler(btnSearch_Click);

            chkAllChecked.Click += new EventHandler(chkAllChecked_Click);

        }

        void chkAllChecked_Click(object sender, EventArgs e)
        {
            var isAllChecked = chkAllChecked.Checked;
            foreach (DataGridViewRow row in gridCostItem.Rows)
            {
                row.Cells[ItemSelect.Name].Value = isAllChecked;
            }
        }

        void VSelectCostItem_Load(object sender, EventArgs e)
        {
            if (DefaultSelectedCostItem != null)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", DefaultSelectedCostItem.Id));
                oq.AddCriterion(Expression.Eq("ItemState", CostItemState.发布));
                oq.AddFetchMode("TheCostItemCategory", NHibernate.FetchMode.Eager);

                IList list = model.ObjectQuery(typeof(CostItem), oq);

                if (list != null && list.Count > 0)
                {
                    DefaultSelectedCostItem = list[0] as CostItem;

                    foreach (TreeNode tn in tvwCategory.Nodes)
                    {
                        if (tn.Name == DefaultSelectedCostItem.TheCostItemCategory.Id)
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
                        if (SetDefaultSelectedNode(tn))
                            break;
                    }

                    bool hasFlag = false;
                    if (gridCostItem.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow row in gridCostItem.Rows)
                        {
                            CostItem item = row.Tag as CostItem;
                            if (item.Id == DefaultSelectedCostItem.Id)
                            {
                                row.Selected = true;
                                gridCostItem.CurrentCell = row.Cells[0];
                                gridCostItem_CellClick(row.Cells[0], new DataGridViewCellEventArgs(0, row.Index));

                                hasFlag = true;
                                break;
                            }
                        }
                    }

                    if (hasFlag == false)
                    {
                        //tvwCategory.AfterSelect -= new TreeViewEventHandler(tvwCategory_AfterSelect);
                        //tvwCategory.SelectedNode = null;
                        //tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);

                        gridCostItem.Rows.Clear();

                        AddCostItemInfoInGrid(DefaultSelectedCostItem);

                        DataGridViewRow row = gridCostItem.Rows[0];
                        row.Selected = true;
                        gridCostItem.CurrentCell = row.Cells[0];
                        gridCostItem_CellClick(row.Cells[0], new DataGridViewCellEventArgs(0, row.Index));
                    }
                }
            }
        }
        private bool SetDefaultSelectedNode(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Name == DefaultSelectedCostItem.TheCostItemCategory.Id)
                {
                    tvwCategory.SelectedNode = tn;

                    TreeNode theParentNode = tn.Parent;
                    while (theParentNode != null)
                    {
                        theParentNode.Expand();
                        theParentNode = theParentNode.Parent;
                    }
                    return true;
                }
                SetDefaultSelectedNode(tn);
            }

            return false;
        }

        private void LoadCostItemCategoryTreeRootNode()
        {
            try
            {
                tvwCategory.Nodes.Clear();

                ObjectQuery oq = new ObjectQuery();
                if (string.IsNullOrEmpty(DefaultCateSyscode))
                {
                    oq.AddCriterion(Expression.Eq("State", 1));
                    //oq.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
                    oq.AddCriterion(Expression.IsNull("ParentNode"));
                }
                else
                {
                    oq.AddCriterion(Expression.Eq("SysCode", DefaultCateSyscode));
                }

                IList list = model.ObjectQuery(typeof(CostItemCategory), oq);
                if (list.Count > 0)
                {
                    CostItemCategory childNode = list[0] as CostItemCategory;
                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = childNode.Name;
                    tnTmp.Tag = childNode;


                    tvwCategory.Nodes.Add(tnTmp);

                    this.tvwCategory.SelectedNode = this.tvwCategory.Nodes[0];
                    //this.tvwCategory.SelectedNode.Expand();


                    //加载下一层子节点
                    LoadCostItemCategoryByParentNode(tnTmp);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("加载成本项分类出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }
        private void LoadCostItemCategoryTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                IList list = model.GetCostItemCategoryByInstance(projectInfo.Id);
                //lstInstance = listAll[1] as IList;
                //IList list = listAll[0] as IList;
                foreach (CostItemCategory childNode in list)
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
                MessageBox.Show("加载成本项分类出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }
        private void LoadCostItemCategoryByParentNode(TreeNode parentNode)
        {
            try
            {
                CostItemCategory parentCate = parentNode.Tag as CostItemCategory;
                if (parentCate == null)
                    return;

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("State", 1));
                oq.AddCriterion(Expression.Eq("ParentNode.Id", parentCate.Id));
                oq.AddOrder(NHibernate.Criterion.Order.Asc("OrderNo"));

                IList list = model.ObjectQuery(typeof(CostItemCategory), oq);
                if (list.Count > 0)
                {
                    foreach (CostItemCategory childNode in list)
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
        private void LoadSecondLevelCostItemCategoryByParentNode(TreeNode firstParentNode)
        {
            try
            {
                if (firstParentNode.Nodes.Count == 0)
                    return;

                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();

                foreach (TreeNode childNode in firstParentNode.Nodes)
                {
                    if ((childNode.Tag as CostItemCategory) != null)
                        dis.Add(Expression.Eq("ParentNode.Id", (childNode.Tag as CostItemCategory).Id));
                }
                oq.AddCriterion(dis);

                oq.AddCriterion(Expression.Eq("State", 1));
                oq.AddOrder(NHibernate.Criterion.Order.Asc("OrderNo"));

                IList list = model.ObjectQuery(typeof(CostItemCategory), oq);
                if (list.Count > 0)
                {
                    foreach (TreeNode parentNode in firstParentNode.Nodes)
                    {
                        parentNode.Nodes.Clear();
                        CostItemCategory parentCate = parentNode.Tag as CostItemCategory;
                        if (parentCate == null)
                            continue;

                        foreach (CostItemCategory childNode in list)
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

        private void GetNodeDetail()
        {
            try
            {
                ClearAll();

                this.txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath;

                GetCostItemByCategory();
            }
            catch (Exception exp)
            {
                MessageBox.Show("显示出错：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }
        private void GetCostItemByCategory()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheCostItemCategory.Id", oprNode.Id));
            oq.AddCriterion(Expression.Eq("ItemState", CostItemState.发布));
            //oq.AddOrder(NHibernate.Criterion.Order.Desc("CreateTime"));
            //oq.AddOrder(NHibernate.Criterion.Order.Desc("UpdateTime"));
            oq.AddCriterion(Expression.Eq("CostItemType", CostItemType.标准));
            oq.AddFetchMode("TheCostItemCategory", NHibernate.FetchMode.Eager);
            oq.AddOrder(NHibernate.Criterion.Order.Asc("Code"));
            IList list = model.ObjectQuery(typeof(CostItem), oq);

            gridCostItem.Rows.Clear();
            foreach (CostItem item in list)
            {
                AddCostItemInfoInGrid(item);
            }
            gridCostItem.ClearSelection();
        }
        private void ClearAll()
        {
            this.txtCurrentPath.Text = "";
        }
        private void ClearCostItemAll()
        {
            txtCostItemCode.Text = "";
            txtCostItemName.Text = "";
            txtItemQuotaCode.Text = "";

            txtProjectUnit.Text = "";
            txtPriceUnit.Text = "";
            txtPrice.Text = "";
            txtDesc.Text = "";

            txtAdaptLevel.Text = "";
            txtManagementMode.Text = "";
            txtContentType.Text = "";
            txtPricingRate.Text = "";

            gridCostSubject.Rows.Clear();
        }
        private void GetCostItemDetail()
        {
            try
            {
                ClearCostItemAll();

                txtCostItemCode.Text = _selectCostItem.Code;
                txtCostItemName.Text = _selectCostItem.Name;
                txtItemQuotaCode.Text = _selectCostItem.QuotaCode;

                txtProjectUnit.Text = _selectCostItem.ProjectUnitName;
                txtPriceUnit.Text = _selectCostItem.PriceUnitName;
                txtPrice.Text = _selectCostItem.Price.ToString();
                txtDesc.Text = _selectCostItem.CostDesc;

                txtAdaptLevel.Text = _selectCostItem.ApplyLevel.ToString();
                if (_selectCostItem.ManagementMode != null)
                    txtManagementMode.Text = _selectCostItem.ManagementModeName;
                txtContentType.Text = _selectCostItem.ContentType.ToString();
                txtPricingRate.Text = _selectCostItem.PricingRate.ToString();

                txtCostItemCateFilter.Text = _selectCostItem.CateFilterName1;
                txtCostItemCateFilter2.Text = _selectCostItem.CateFilterName2;

                txtCostSubjectFilter1.Text = _selectCostItem.SubjectCateFilterName1;
                txtCostSubjectFilter2.Text = _selectCostItem.SubjectCateFilterName2;
                txtCostSubjectFilter3.Text = _selectCostItem.SubjectCateFilterName3;

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", _selectCostItem.Id));
                oq.AddFetchMode("ListQuotas", NHibernate.FetchMode.Eager);//ListQuotas选择页面使用

                IList list = model.ObjectQuery(typeof(CostItem), oq);
                _selectCostItem = list[0] as CostItem;

                gridCostSubject.Rows.Clear();
                if (_selectCostItem.ListQuotas.Count > 0)
                {
                    oq.Criterions.Clear();
                    oq.FetchModes.Clear();
                    Disjunction dis = new Disjunction();
                    foreach (SubjectCostQuota quota in _selectCostItem.ListQuotas)
                    {
                        dis.Add(Expression.Eq("Id", quota.Id));
                    }
                    oq.AddCriterion(dis);
                    oq.AddFetchMode("ListResources", NHibernate.FetchMode.Eager);

                    IList listQuotas = model.ObjectQuery(typeof(SubjectCostQuota), oq);
                    foreach (SubjectCostQuota quota in listQuotas)
                    {
                        //if (quota.State == SubjectCostQuotaState.生效)
                        //{
                        AddCostQuotaInfoInGrid(quota);
                        //}
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("显示出错：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        //成本项分类操作
        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                oprNode = tvwCategory.SelectedNode.Tag as CostItemCategory;
                this.GetNodeDetail();

                _selectCostItem = null;
                ClearCostItemAll();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
            }
        }

        void tvwCategory_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null || (e.Node.Tag as CostItemCategory) == null)
                return;

            TreeNode tempNode = new TreeNode();

            string countSql = "select distinct thecostitemcategory from thd_costitem where itemState=2 and thecostitemcategory in ( ";
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
                    tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    tn.ForeColor = ColorTranslator.FromHtml("#000000");
                }
                else
                {
                    tn.BackColor = tempNode.BackColor;
                    tn.ForeColor = tempNode.ForeColor;
                }
            }

            LoadSecondLevelCostItemCategoryByParentNode(e.Node);
        }

        //成本项行点击
        void gridCostItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                CostItem temp = gridCostItem.Rows[e.RowIndex].Tag as CostItem;
                if (_selectCostItem != null && temp.Id == _selectCostItem.Id)
                    return;

                _selectCostItem = temp;
                if (e.ColumnIndex == 1)
                {
                    var value = gridCostItem.Rows[e.RowIndex].Cells[ItemSelect.Name].Value;
                    gridCostItem.Rows[e.RowIndex].Cells[ItemSelect.Name].Value = (value == null || (bool)value == false);
                    gridCostItem.CurrentCell = gridCostItem.Rows[e.RowIndex].Cells[0];
                    gridCostItem.Refresh();
                    gridCostSubject.Focus();
                    gridCostItem.Focus();
                    //return;
                }
                GetCostItemDetail();
                gridCostItem.CurrentCell = gridCostItem.Rows[e.RowIndex].Cells[0];
            }
        }

        private void AddCostItemInfoInGrid(CostItem dtl)
        {
            int index = gridCostItem.Rows.Add();
            DataGridViewRow row = gridCostItem.Rows[index];
            row.Cells[ItemName.Name].Value = dtl.Name;
            row.Cells[ItemCode.Name].Value = dtl.Code;
            row.Cells[ItemQuotaCode.Name].Value = dtl.QuotaCode;
            row.Cells[ItemState.Name].Value = dtl.ItemState.ToString();
            row.Cells[ItemContentType.Name].Value = dtl.ContentType.ToString();
            row.Cells[ItemManagementMode.Name].Value = dtl.ManagementModeName;
            row.Cells[ItemDesc.Name].Value = dtl.CostDesc;
            row.Cells[ItemCateCode.Name].Value = dtl.TheCostItemCategory.Code;
            row.Cells[ItemCateName.Name].Value = dtl.TheCostItemCategory.Name;
            row.Tag = dtl;

            gridCostItem.CurrentCell = row.Cells[0];
        }

        private void AddCostQuotaInfoInGrid(SubjectCostQuota dtl)
        {
            int index = gridCostSubject.Rows.Add();
            DataGridViewRow row = gridCostSubject.Rows[index];
            row.Cells[SubjectResUsageName.Name].Value = dtl.Name;
            row.Cells[SubjectCateName.Name].Value = dtl.CostAccountSubjectName;

            DataGridViewComboBoxCell cell = row.Cells[SubjectResourceGroup.Name] as DataGridViewComboBoxCell;
            foreach (ResourceGroup rg in dtl.ListResources)
            {
                string matStr = string.IsNullOrEmpty(rg.ResourceTypeQuality) ? "" : rg.ResourceTypeQuality + ".";
                matStr += string.IsNullOrEmpty(rg.ResourceTypeSpec) ? "" : rg.ResourceTypeSpec + ".";
                matStr += rg.ResourceTypeName;
                matStr += string.IsNullOrEmpty(rg.DiagramNumber) ? "" : "." + rg.DiagramNumber;
                cell.Items.Add(matStr);
            }
            if (cell.Items.Count > 0)
                cell.Value = cell.Items[0].ToString();

            row.Cells[SubjectState.Name].Value = dtl.State.ToString();
            row.Cells[SubjectQuotaQuantity.Name].Value = dtl.QuotaProjectAmount.ToString();
            row.Cells[SubjectQuantityPrice.Name].Value = dtl.QuotaPrice.ToString();
            row.Cells[SubjectPriceUnit.Name].Value = dtl.PriceUnitName;

            row.Cells[SubjectQuantityUnit.Name].Value = dtl.ProjectAmountUnitName;
            row.Cells[SubjectWorkAmountPrice.Name].Value = dtl.QuotaMoney.ToString();

            row.Cells[SubjectWastage.Name].Value = dtl.Wastage.ToString();
            row.Cells[SubjectAssessmentRate.Name].Value = dtl.AssessmentRate.ToString();

            row.Cells[SubjectMainResourceFlag.Name].Value = dtl.MainResourceFlag ? "是" : "否";

            row.Tag = dtl;

            gridCostSubject.CurrentCell = row.Cells[0];
        }

        public bool isOK = false;
        //确定
        void btnEnter_Click(object sender, EventArgs e)
        {
            if (isSelectSingle)
            {
                if (_selectCostItem == null)
                {
                    MessageBox.Show("请选择一条成本项！");
                    gridCostItem.Focus();
                    return;
                }
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", _selectCostItem.Id));
                oq.AddFetchMode("ProjectUnitGUID", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
                _selectCostItem = model.ObjectQuery(typeof(CostItem), oq)[0] as CostItem;

                isOK = true;
            }
            else
            {
                IList checkCostItemList = new ArrayList();
                foreach (DataGridViewRow row in gridCostItem.Rows)
                {
                    if ((bool)row.Cells[ItemSelect.Name].EditedFormattedValue)
                    {
                        CostItem item = row.Tag as CostItem;
                        checkCostItemList.Add(item);
                    }
                }
                if (checkCostItemList == null || checkCostItemList.Count == 0)
                {
                    MessageBox.Show("请选择一条成本项！");
                    gridCostItem.Focus();
                    return;
                }
                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();
                foreach (CostItem i in checkCostItemList)
                {
                    dis.Add(Expression.Eq("Id", i.Id));
                }
                oq.AddCriterion(dis);
                oq.AddFetchMode("ProjectUnitGUID", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
                oq.AddOrder(NHibernate.Criterion.Order.Asc("QuotaCode"));
                //oq.AddOrder(NHibernate.Criterion.Order.Asc("Code"));
                selectCostItemList = model.ObjectQuery(typeof(CostItem), oq);
            }
            this.Close();
        }
        //取消
        void btnCancel_Click(object sender, EventArgs e)
        {
            _selectCostItem = null;
            this.Close();
        }

        void VSelectCostItem_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isOK)
                _selectCostItem = null;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            string cateCode = txtIteamCateCode.Text.Trim();
            string cateName = txtIteamCateName.Text.Trim();
            string quotaCode = txtQuotaCode.Text.Trim();
            string costName = txtCostName.Text.Trim();
            if (cateCode == "" && cateName == "" && quotaCode == "" && costName == "")
            {
                if (MessageBox.Show("没有查询条件会降低查询速度，确定要继续吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    txtIteamCateCode.Focus();
                    return;
                }
            }

            try
            {
                FlashScreen.Show("正在查询加载数据,请稍候......");


                ObjectQuery oq = new ObjectQuery();

                CostItemCategory optCateRange = null;

                if (tvwCategory.SelectedNode != null && tvwCategory.SelectedNode.Tag != null)
                {
                    optCateRange = tvwCategory.SelectedNode.Tag as CostItemCategory;
                }
                else if (!string.IsNullOrEmpty(DefaultCateSyscode))
                {
                    optCateRange = new CostItemCategory();
                    optCateRange.SysCode = DefaultCateSyscode;
                }

                if (optCateRange != null)
                    oq.AddCriterion(Expression.Like("TheCostItemCateSyscode", optCateRange.SysCode, MatchMode.Start));

                if (quotaCode != "")
                    oq.AddCriterion(Expression.Like("QuotaCode", quotaCode, MatchMode.Anywhere));

                if (costName != "")
                    oq.AddCriterion(Expression.Like("Name", costName, MatchMode.Anywhere));

                if (cateName != "" || cateCode != "")
                {

                    string sql = "select t1.id from thd_costitemcategory t1 where 1=1 ";

                    if (optCateRange != null)
                        sql += " and t1.syscode like '" + optCateRange.SysCode + "%'";

                    if (txtIteamCateCode.Text.Trim() != "")
                        sql += " and t1.code like '%" + cateCode + "%'";
                    if (txtIteamCateName.Text.Trim() != "")
                        sql += " and t1.name like '%" + cateName + "%'";

                    sql += " order by code";

                    DataSet ds = model.SearchSQL(sql);

                    Disjunction dis = new Disjunction();
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string cateId = row["id"].ToString();
                            dis.Add(Expression.Eq("TheCostItemCategory.Id", cateId));
                        }
                    }
                    oq.AddCriterion(dis);
                }

                oq.AddCriterion(Expression.Eq("CostItemType", CostItemType.标准));
                oq.AddFetchMode("ProjectUnitGUID", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("TheCostItemCategory", NHibernate.FetchMode.Eager);
                oq.AddOrder(NHibernate.Criterion.Order.Desc("CreateTime"));
                oq.AddOrder(NHibernate.Criterion.Order.Desc("UpdateTime"));

                IList list = model.ObjectQuery(typeof(CostItem), oq);

                gridCostItem.Rows.Clear();
                foreach (CostItem item in list)
                {
                    AddCostItemInfoInGrid(item);
                }
                gridCostItem.ClearSelection();
                FlashScreen.Close();
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("查询出错，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        /// <summary>
        /// 操作类型
        /// </summary>
        public enum OptionTypeEnum
        {
            选择成本项 = 1,
            查看成本项 = 2
        }

    }
}
