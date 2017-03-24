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
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using System.Linq;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng
{
    public partial class VSelectGWBSDetailByMeansureFee : TBasicDataView
    {
        /// <summary>
        /// ѡ��ְ���ʩ����ϸ��GWBS��Χ
        /// </summary>
        public GWBSTree SelectGWBSRange = null;

        private GWBSTree oprNode = null;

        //��Ȩ�޵�GWBSTree
        private IList lstInstance;
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();
        private List<GWBSDetail> _selectGWBSDetail = new List<GWBSDetail>();
        /// <summary>
        /// ѡ��Ĺ���������ϸ����
        /// </summary>
        public List<GWBSDetail> SelectGWBSDetail
        {
            get { return _selectGWBSDetail; }
            set { _selectGWBSDetail = value; }
        }


        public Dictionary<string, object> InitCondition = new Dictionary<string, object>();

        private bool _isOk;

        /// <summary>
        /// �Ƿ���ѡ��
        /// </summary>
        public bool IsOk
        {
            get { return _isOk; }
            set { _isOk = value; }
        }

        public MGWBSTree model = new MGWBSTree();

        public VSelectGWBSDetailByMeansureFee(GWBSTree balRange)
        {
            SelectGWBSRange = balRange;

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
                MessageBox.Show("��������ĳɱ����Ż򶨶��ţ�δ�ҵ���سɱ��");
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
                MessageBox.Show("������ѡ��һ��������ϸ��");
                return;
            }

            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (DataGridViewRow row in gridSelectDetail.Rows)
            {
                GWBSDetail item = row.Tag as GWBSDetail;
                dis.Add(Expression.Eq("Id", item.Id));
            }
            oq.AddCriterion(dis);
            oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ListCostSubjectDetails", NHibernate.FetchMode.Eager);

            IList listDtl = model.ObjectQuery(typeof(GWBSDetail), oq);

            _selectGWBSDetail.AddRange(listDtl.OfType<GWBSDetail>().ToArray());

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
                MessageBox.Show("��ѡ�����ӵ��У�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridQueryDetail.SelectedRows)
            {
                GWBSDetail item = row.Tag as GWBSDetail;
                if (item.State == VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute)
                    listRowIndex.Add(row.Index);
            }

            if (listRowIndex.Count == 0)
            {
                MessageBox.Show("��ѡ����Ч��״̬�Ĵ�ʩ����ϸ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            listRowIndex.Sort();
            for (int i = listRowIndex.Count - 1; i > -1; i--)
            {
                DataGridViewRow row = gridQueryDetail.Rows[listRowIndex[i]];
                GWBSDetail dtl = row.Tag as GWBSDetail;

                if (IsExistsSelectedDtl(dtl) == false)
                    AddDetailInfoInSelectGrid(dtl);

                //GWBSDetail dtl = gridQueryDetail.Rows[listRowIndex[i]].Tag as GWBSDetail;

                //ObjectQuery oq = new ObjectQuery();
                //oq.AddCriterion(NHibernate.Criterion.Expression.Eq("Id", dtl.Id));
                //oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
                //IList listDtl = model.ObjectQuery(typeof(GWBSDetail), oq);
                //dtl = listDtl[0] as GWBSDetail;

                //AddDetailInfoInSelectGrid(dtl);

                gridQueryDetail.Rows.RemoveAt(row.Index);
            }
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            string dtlName = txtTaskName.Text.Trim();
            string dtlDesc = txtTaskDesc.Text.Trim();

            CostItem costItem = null;
            if (txtTaskCostItem.Text.Trim() != "")
                costItem = txtTaskCostItem.Tag as CostItem;

            //if (dtlName == "" && dtlDesc == "" && costItem == null)
            //{
            //    if (MessageBox.Show("û�в�ѯ�����ή����ѯ�ٶȣ�ȷ��Ҫ������", "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //    {
            //        txtTaskCostItem.Focus();
            //        return;
            //    }
            //}

            try
            {
                FlashScreen.Show("���ڲ�ѯ��������,���Ժ�......");

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
                oq.AddCriterion(Expression.Eq("SubContractFeeFlag", true));

                Disjunction dis = new Disjunction();
                string[] sysCodes = SelectGWBSRange.SysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < sysCodes.Length - 1; i++)
                {
                    string sysCode = "";
                    for (int j = 0; j <= i; j++)
                    {
                        sysCode += sysCodes[j] + ".";
                    }
                    dis.Add(Expression.Eq("TheGWBSSysCode", sysCode));
                }
                oq.AddCriterion(Expression.Or(Expression.Like("TheGWBSSysCode", SelectGWBSRange.SysCode, MatchMode.Start), dis));

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

                oq.AddOrder(NHibernate.Criterion.Order.Asc("TheGWBSSysCode"));
                oq.AddOrder(NHibernate.Criterion.Order.Asc("OrderNo"));
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
                MessageBox.Show("��ѯ�������쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        //˫���鿴�ɱ���
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

            foreach (TreeNode node in e.Node.Nodes)
            {
                GWBSTree task = node.Tag as GWBSTree;
                if (task.SubContractFeeFlag)
                {
                    node.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    node.ForeColor = ColorTranslator.FromHtml("#000000");
                }
                else
                {
                    node.BackColor = tempNode.BackColor;
                    node.ForeColor = tempNode.ForeColor;
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
                oq.AddCriterion(Expression.Eq("SubContractFeeFlag", true));
                oq.AddOrder(NHibernate.Criterion.Order.Asc("OrderNo"));
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
                MessageBox.Show("�������ݳ�����" + ExceptionUtil.ExceptionMessage(e));
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


                    //������һ���ӽڵ�
                    LoadCategoryByParentNode(tnTmp);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("���سɱ�����������" + ExceptionUtil.ExceptionMessage(e));
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

                Disjunction dis = new Disjunction();
                string[] sysCodes = SelectGWBSRange.SysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < sysCodes.Length - 1; i++)
                {
                    string sysCode = "";
                    for (int j = 0; j <= i; j++)
                    {
                        sysCode += sysCodes[j] + ".";
                    }
                    dis.Add(Expression.Eq("SysCode", sysCode));
                }
                oq.AddCriterion(Expression.Or(Expression.Like("SysCode", SelectGWBSRange.SysCode, MatchMode.Start), dis));

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
                MessageBox.Show("���سɱ�����������" + ExceptionUtil.ExceptionMessage(e));
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

                Disjunction dis1 = new Disjunction();
                string[] sysCodes = SelectGWBSRange.SysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < sysCodes.Length - 1; i++)
                {
                    string sysCode = "";
                    for (int j = 0; j <= i; j++)
                    {
                        sysCode += sysCodes[j] + ".";
                    }
                    dis1.Add(Expression.Eq("SysCode", sysCode));
                }
                oq.AddCriterion(Expression.Or(Expression.Like("SysCode", SelectGWBSRange.SysCode, MatchMode.Start), dis1));

                oq.AddOrder(NHibernate.Criterion.Order.Asc("OrderNo"));

                IList list = model.ObjectQuery(typeof(GWBSTree), oq);
                if (list.Count > 0)
                {
                    foreach (TreeNode parentNode in firstParentNode.Nodes)
                    {
                        GWBSTree parentCate = parentNode.Tag as GWBSTree;
                        if (parentCate == null)
                            continue;

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
                MessageBox.Show("���سɱ�����������" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        private void AddDetailInfoInGrid(GWBSDetail dtl)
        {
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

            row.Cells[DtlState.Name].Value = StaticMethod.GetWBSTaskStateText(dtl.State);
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

            row.Cells[selState.Name].Value = StaticMethod.GetWBSTaskStateText(dtl.State);
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

        private bool IsExistsSelectedDtl(GWBSDetail dtl)
        {
            foreach (DataGridViewRow row in gridSelectDetail.Rows)
            {
                GWBSDetail item = row.Tag as GWBSDetail;
                if (item.Id == dtl.Id)
                    return true;
            }
            return false;
        }
    }
}