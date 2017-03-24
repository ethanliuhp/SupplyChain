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
using VirtualMachine.Core.Expression;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan
{
    public partial class VResourceDemandPlan : TBasicDataView
    {
        private ResourceRequirePlan optPlan = null;

        private TreeNode optGWBSNode = null;
        private GWBSTree optGWBS = null;

        private CurrentProjectInfo projectInfo = null;
        string planType = ResourceRequirePlanType.��������ƻ�.ToString();

        private bool isQuery = false;

        public MRollingDemandPlan model;
        private MGWBSTree modelGWBS = new MGWBSTree();

        public VResourceDemandPlan(MRollingDemandPlan mot)
        {
            model = mot;

            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();

            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();

            DateTime now = model.GetServerTime();
            dtMadeStartDateQuery.Value = now.Date.AddDays(-30);
            dtMadeEndDateQuery.Value = now.Date;


            LoadGWBSTreeTree();
            tvwCategory.SelectedNode = null;

            foreach (string type in Enum.GetNames(typeof(ResourceRequirePlanDetailResourceType)))
            {
                cbResourceType.Items.Add(type);
            }
            if (cbResourceType.Items.Count > 0)
                cbResourceType.SelectedIndex = 0;

            //��Դ����ƻ�����
            VBasicDataOptr.InitBasicDataByCurrProjectInfo(planType, cbPlanName, false);
            foreach (string value in cbPlanName.Items)
            {
                cbPlanNameQuery.Items.Add(value);
            }
            if (cbPlanName.Items.Count > 0)
            {
                cbPlanName.SelectedIndex = 0;
            }

            txtNoDtlColorFlag.BackColor = ColorTranslator.FromHtml("#D7E8FE");



            //if (cbPlanName.Items.Count > 0)
            //{
            //    ObjectQuery oq = new ObjectQuery();
            //    oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
            //    oq.AddCriterion(Expression.Eq("PlanType", planType));
            //    oq.AddCriterion(Expression.Eq("RequirePlanVersion", cbPlanName.SelectedItem.ToString()));

            //    IList list = model.ObjectQuery(typeof(ResourceRequirePlan), oq);

            //    if (list.Count > 0)
            //    {
            //        optPlan = list[0] as ResourceRequirePlan;
            //    }
            //    else
            //    {
            //        optPlan = null;
            //    }
            //}
        }

        private void InitEvents()
        {
            tabPlan.SelectedIndexChanged += new EventHandler(tabPlan_SelectedIndexChanged);

            btnQuery.Click += new EventHandler(btnQuery_Click);

            tvwCategory.AfterExpand += new TreeViewEventHandler(tvwCategory_AfterExpand);
            tvwCategory.NodeMouseClick += new TreeNodeMouseClickEventHandler(tvwCategory_NodeMouseClick);
            mnuTree.ItemClicked += new ToolStripItemClickedEventHandler(mnuTree_ItemClicked);


            cbPlanName.SelectedIndexChanged += new EventHandler(cbPlanName_SelectedIndexChanged);
            cbResourceType.SelectedIndexChanged += new EventHandler(cbResourceType_SelectedIndexChanged);

            btnAddPlan.Click += new EventHandler(btnAddPlan_Click);
            btnSaveMaster.Click += new EventHandler(btnSaveMaster_Click);
            btnPublishMaster.Click += new EventHandler(btnPublishMaster_Click);

            btnSelectWBSNode.Click += new EventHandler(btnSelectWBSNode_Click);
            tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);

            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnPublish.Click += new EventHandler(btnPublish_Click);
            btnCancellation.Click += new EventHandler(btnCancellation_Click);

            gridResourceRequireDetail.CellValidating += new DataGridViewCellValidatingEventHandler(gridResourceRequireDetail_CellValidating);

            gridResourceRequireDetail.CellEndEdit += new DataGridViewCellEventHandler(gridResourceRequireDetail_CellEndEdit);

            gridResourceRequireDetail.CellDoubleClick += new DataGridViewCellEventHandler(gridResourceRequireDetail_CellDoubleClick);
        }


        private void LoadGWBSTreeTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();
                IList list = modelGWBS.GetGWBSTreesByInstance(projectInfo.Id);

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
                MessageBox.Show("�������ݳ���" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        void tabPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabPlan.SelectedTab == tabPagePlanInfo)
            {
                string planName = cbPlanName.Text.Trim();
                if (!string.IsNullOrEmpty(planName))
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                    oq.AddCriterion(Expression.Eq("PlanType", planType));
                    oq.AddCriterion(Expression.Eq("RequirePlanVersion", planName));

                    IList list = model.ObjectQuery(typeof(ResourceRequirePlan), oq);
                   
                    if (list.Count > 0)
                    {
                        optPlan = list[0] as ResourceRequirePlan;

                        isQuery = false;
                    }
                    else
                    {
                        optPlan = null;
                    }
                }
            }
        }

        #region ��Դ����ƻ�����
        //��ѯ
        private void btnQuery_Click(object sender, EventArgs e)
        {
            //����У��
            bool isSaveFlag = true;
            foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
            {
                if (!row.ReadOnly)
                {
                    isSaveFlag = false;
                    break;
                }
            }
            if (!isSaveFlag)
            {
                if (MessageBox.Show("��ϸ��δ���棬Ҫ������", "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!ValidateSaveDetail(null))
                        return;
                }
            }


            string planName = cbPlanNameQuery.Text.Trim();

            DateTime madeStartDate = dtMadeStartDateQuery.Value;
            DateTime madeEndDate = dtMadeEndDateQuery.Value;
            madeEndDate = madeEndDate.AddDays(1).AddSeconds(-1);

            if (madeStartDate > madeEndDate)
            {
                MessageBox.Show("�Ƶ���ʼ���ڲ��ܴ��ڽ������ڣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtMadeStartDateQuery.Focus();
                return;
            }
            //else if ((madeEndDate - madeStartDate).Days > 31)
            //{
            //    MessageBox.Show("�Ƶ����ڷ�Χ���ܳ���һ���£�");
            //    dtMadeEndDateQuery.Focus();
            //    return;
            //}

            PersonInfo responsibilityPerson = null;
            if (txtResponsibilityPersonQuery.Result != null && txtResponsibilityPersonQuery.Result.Count > 0)
                responsibilityPerson = txtResponsibilityPersonQuery.Result[0] as PersonInfo;

            string spec = txtSpecification.Text.Trim();
            string nodeName = txtNodeNameQuery.Text.Trim();

            ObjectQuery oq = new ObjectQuery();
            
            ObjectQuery oqMain = new ObjectQuery();
            if (responsibilityPerson != null)
                oqMain.AddCriterion(Expression.Eq("HandlePerson.Id", responsibilityPerson.Id));
            if (!string.IsNullOrEmpty(planName))
                oqMain.AddCriterion(Expression.Like("RequirePlanVersion", planName, MatchMode.Anywhere));

            if (oqMain.Criterions.Count > 0)//���������ѯ����ʹ��ģ����ѯ
            {
                oqMain.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                oqMain.AddCriterion(Expression.Eq("PlanType", planType));

                Disjunction dis = new Disjunction();

                IList listPlan = model.ObjectQuery(typeof(ResourceRequirePlan), oqMain);
                if (listPlan.Count > 0)
                {
                    foreach (ResourceRequirePlan plan in listPlan)
                    {
                        dis.Add(Expression.Eq("TheResourceRequirePlan.Id", plan.Id));
                    }
                }
                
                oq.AddCriterion(dis);
            }
            else
            {
                oq.AddCriterion(Expression.Eq("TheResourceRequirePlan.TheProjectGUID", projectInfo.Id));
                oq.AddCriterion(Expression.Eq("TheResourceRequirePlan.PlanType", planType));
            }

            oq.AddCriterion(Expression.Ge("TheResourceRequirePlan.CreateDate", madeStartDate));
            oq.AddCriterion(Expression.Le("TheResourceRequirePlan.CreateDate", madeEndDate));

            oq.AddCriterion(Expression.Eq("MaterialName", cbResourceType.SelectedItem.ToString()));


            if (!string.IsNullOrEmpty(spec))
                oq.AddCriterion(Expression.Like("MaterialSpec", spec, MatchMode.Anywhere));

            if (!string.IsNullOrEmpty(nodeName))
                oq.AddCriterion(Expression.Like("TheGWBSTaskName", nodeName, MatchMode.Anywhere));

            //oq.AddFetchMode("TheGWBSTaskGUID", NHibernate.FetchMode.Eager);

            oq.AddOrder(NHibernate.Criterion.Order.Asc("CreateTime"));
            oq.AddOrder(NHibernate.Criterion.Order.Asc("TheGWBSTaskName"));

            IList list = model.ObjectQuery(typeof(ResourceRequirePlanDetail), oq);

            optPlan = null;
            tvwCategory.SelectedNode = null;
            optGWBS = null;
            optGWBSNode = null;
            isQuery = true;

            gridResourceRequireDetail.Rows.Clear();

            if (list != null)
            {
                foreach (ResourceRequirePlanDetail tempDtl in list)
                {
                    AddResourceRequireDetailInGrid(tempDtl, false, true);
                }

                gridResourceRequireDetail.ClearSelection();
            }

            SetVisibleColumn();
        }

        void btnSelectWBSNode_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree frm = new VSelectGWBSTree(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                tvwCategory.Nodes.Clear();

                TreeNode selectRootNode = frm.SelectResult[0];
                TreeNode root = new TreeNode();
                root.Name = selectRootNode.Name;
                root.Text = selectRootNode.Text;
                root.Tag = selectRootNode.Tag;
                tvwCategory.Nodes.Add(root);

                LoadSelectTaskTree(root, selectRootNode);
            }
        }

        void tvwCategory_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)// && ConstMethod.Contains(lstInstance, e.Node.Tag as CategoryNode)
            {
                tvwCategory.SelectedNode = e.Node;

                optGWBSNode = e.Node;
                optGWBS = e.Node.Tag as GWBSTree;

                if (cbResourceType.SelectedItem.ToString() == ResourceRequirePlanDetailResourceType.����.ToString())
                {
                    mnuTree.Show(tvwCategory, new Point(e.X, e.Y));
                }
            }
        }

        void mnuTree_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text.Trim() == "����ƻ���Դ����")
            {
                mnuTree.Hide();

                if (optPlan == null)
                {
                    MessageBox.Show("������ӻ򱣴�ƻ�ͷ��Ϣ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnSaveMaster.Focus();
                    return;
                }
                if (optPlan.State != ResourceRequirePlanState.�ƶ�)
                {
                    MessageBox.Show("�üƻ�״̬Ϊ��" + optPlan.State + "��,��ѡ��״̬Ϊ���ƶ����ļƻ������ϸ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbPlanName.Focus();
                    return;
                }


                VImportPlanTaskDetail frm = new VImportPlanTaskDetail();
                frm.DefaultSelectedGWBS = optGWBS;
                frm.ShowDialog();

                if (frm.isOK)
                {

                    Dictionary<Dictionary<Material, StandardUnit>, Decimal> listMatAndPlanQuantity = frm.SelectResult;

                    DateTime serverTime = model.GetServerTime();
                    string resType = cbResourceType.SelectedItem.ToString();

                    foreach (var dic in listMatAndPlanQuantity)
                    {
                        Dictionary<Material, StandardUnit> dicKey = dic.Key;


                        ResourceRequirePlanDetail dtl = new ResourceRequirePlanDetail();

                        dtl.TheProjectGUID = projectInfo.Id;
                        dtl.TheProjectName = projectInfo.Name;

                        dtl.TheGWBSTaskGUID = optGWBS;
                        dtl.TheGWBSTaskName = optGWBS.Name;
                        dtl.TheGWBSSysCode = optGWBS.SysCode;

                        dtl.State = ResourceRequirePlanDetailState.����;
                        dtl.StateUpdateTime = serverTime;

                        dtl.MaterialName = resType;



                        Material mat = dicKey.Keys.ElementAt(0);
                        dtl.MaterialResource = mat;
                        dtl.MaterialName = mat.Name;
                        dtl.MaterialCode = mat.Code;
                        dtl.MaterialSpec = mat.Specification;
                        dtl.MaterialStuff = mat.Quality;

                        dtl.QuantityUnitGUID = dicKey.Values.ElementAt(0);
                        if (dtl.QuantityUnitGUID != null)
                            dtl.QuantityUnitName = dtl.QuantityUnitGUID.Name;


                        dtl.PlanRequireQuantity = dic.Value;


                        AddResourceRequireDetailInGrid(dtl, false, false);
                    }

                    gridResourceRequireDetail.CurrentCell =
                        resType == ResourceRequirePlanDetailResourceType.����.ToString() ?
                        gridResourceRequireDetail.Rows[gridResourceRequireDetail.Rows.Count - listMatAndPlanQuantity.Count].Cells[ResourceName.Name] :
                        gridResourceRequireDetail.Rows[gridResourceRequireDetail.Rows.Count - listMatAndPlanQuantity.Count].Cells[ServiceType.Name];

                    gridResourceRequireDetail.BeginEdit(false);
                }
            }
        }

        void tvwCategory_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (optPlan != null)
            {
                TreeNode tempNode = new TreeNode();

                string countSql = "select count(0) from THD_ResourceRequirePlanDetail where TheProjectGUID='" + projectInfo.Id + "' and ParentId='" + optPlan.Id + "' and  TheGWBSTaskGUID=";
                foreach (TreeNode tn in e.Node.Nodes)
                {
                    string sqlQuery = countSql + "'" + tn.Name + "'";
                    DataSet ds = model.SearchSQL(sqlQuery);
                    if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) == 0)
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
            }
        }

        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (optPlan == null)
                    return;

                GWBSTree tempOptGWBS = tvwCategory.SelectedNode.Tag as GWBSTree;
                if (optGWBS != null && tempOptGWBS.Id == optGWBS.Id)
                    return;

                //����У��
                bool isSaveFlag = true;
                foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
                {
                    if (!row.ReadOnly)
                    {
                        isSaveFlag = false;
                        break;
                    }
                }
                if (!isSaveFlag)
                {
                    string msg = "��ϸ��δ���棬Ҫ������";
                    if (optGWBS != null)
                        msg = "����ڵ㡰" + optGWBS.Name + "���µ�" + msg;

                    if (MessageBox.Show(msg, "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (!ValidateSaveDetail(null))
                        {
                            tvwCategory.SelectedNode = optGWBSNode;
                            return;
                        }
                    }
                }

                optGWBSNode = tvwCategory.SelectedNode;
                optGWBS = tempOptGWBS;

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheResourceRequirePlan.Id", optPlan.Id));
                oq.AddCriterion(Expression.Eq("TheGWBSTaskGUID.Id", optGWBS.Id));
                oq.AddCriterion(Expression.Eq("MaterialName", cbResourceType.SelectedItem.ToString()));

                //oq.AddFetchMode("TheGWBSTaskGUID", NHibernate.FetchMode.Eager);
                
                oq.AddOrder(NHibernate.Criterion.Order.Asc("CreateTime"));

                IList listDtl = model.ObjectQuery(typeof(ResourceRequirePlanDetail), oq);

                gridResourceRequireDetail.Rows.Clear();

                foreach (ResourceRequirePlanDetail dtl in listDtl)
                {
                    AddResourceRequireDetailInGrid(dtl, false, true);
                }

                //for (int i = gridResourceRequireDetail.Rows.Count - 1; i > -1; i--)
                //{
                //    DataGridViewRow row = gridResourceRequireDetail.Rows[i];

                //    ResourceRequirePlanDetail dtl = row.Tag as ResourceRequirePlanDetail;
                //    if (dtl.TheGWBSTaskGUID.Id == optGWBS.Id)
                //    {
                //        row.Visible = true;
                //    }
                //    else
                //    {
                //        row.Visible = false;
                //    }
                //}
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSelectTaskTree(TreeNode parentNode, TreeNode selectNode)
        {
            foreach (TreeNode tn in selectNode.Nodes)
            {
                TreeNode node = new TreeNode();
                node.Name = tn.Name;
                node.Text = tn.Text;
                node.Tag = tn.Tag;
                parentNode.Nodes.Add(node);

                LoadSelectTaskTree(node, tn);
            }
        }

        void cbPlanName_SelectedIndexChanged(object sender, EventArgs e)
        {

            cbPlanName.SelectedIndexChanged -= new EventHandler(cbPlanName_SelectedIndexChanged);

            bool isSaveFlag = true;
            foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
            {
                if (!row.ReadOnly)
                {
                    isSaveFlag = false;
                    break;
                }
            }
            if (!isSaveFlag)
            {
                if (MessageBox.Show("��ϸ��δ���棬Ҫ������", "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!ValidateSaveDetail(null))
                    {
                        cbPlanName.Text = optPlan.RequirePlanVersion;

                        cbPlanName.SelectedIndexChanged += new EventHandler(cbPlanName_SelectedIndexChanged);
                        return;
                    }
                }
            }

            string planName = cbPlanName.SelectedItem.ToString();

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("PlanType", planType));
            oq.AddCriterion(Expression.Eq("RequirePlanVersion", planName));
           
            //oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof(ResourceRequirePlan), oq);

            if (list.Count > 0)
            {
                optPlan = list[0] as ResourceRequirePlan;
            }
            else
                optPlan = null;

            ClearRequirePlan();

            LoadResourceRequirePlanData(false);

            if (optPlan != null)
            {
                oq.Criterions.Clear();
                oq.FetchModes.Clear();
                oq.AddCriterion(Expression.Eq("TheResourceRequirePlan.Id", optPlan.Id));
                oq.AddCriterion(Expression.Eq("MaterialName", cbResourceType.SelectedItem.ToString()));
                //oq.AddFetchMode("TheGWBSTaskGUID", NHibernate.FetchMode.Eager);
               
                IList listDtl = model.ObjectQuery(typeof(ResourceRequirePlanDetail), oq);
                foreach (ResourceRequirePlanDetail dtl in listDtl)
                {
                    AddResourceRequireDetailInGrid(dtl, false, true);
                }
            }

            cbPlanName.SelectedIndexChanged += new EventHandler(cbPlanName_SelectedIndexChanged);
        }

        void cbResourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isSaveFlag = true;
            foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
            {
                if (!row.ReadOnly)
                {
                    isSaveFlag = false;
                    break;
                }
            }
            if (!isSaveFlag)
            {
                if (MessageBox.Show("��ϸ��δ���棬Ҫ������", "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string type = cbResourceType.SelectedItem.ToString() == ResourceRequirePlanDetailResourceType.����.ToString() ? ResourceRequirePlanDetailResourceType.����.ToString() : ResourceRequirePlanDetailResourceType.����.ToString();

                    if (!ValidateSaveDetail(type))
                    {
                        cbResourceType.SelectedIndexChanged -= new EventHandler(cbResourceType_SelectedIndexChanged);

                        cbResourceType.Text = type;

                        cbResourceType.SelectedIndexChanged += new EventHandler(cbResourceType_SelectedIndexChanged);
                        return;
                    }
                }
            }

            if (isQuery)
            {
                btnQuery_Click(btnQuery, new EventArgs());
                return;
            }
            else if (optPlan == null)
            {
                return;
            }

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheResourceRequirePlan.Id", optPlan.Id));

            if (optGWBS != null)
            {
                oq.AddCriterion(Expression.Eq("TheGWBSTaskGUID.Id", optGWBS.Id));
            }

            oq.AddCriterion(Expression.Eq("MaterialName", cbResourceType.SelectedItem.ToString()));
            //oq.AddFetchMode("TheGWBSTaskGUID", NHibernate.FetchMode.Eager);
             
            oq.AddOrder(NHibernate.Criterion.Order.Asc("CreateTime"));

            IList listDtl = model.ObjectQuery(typeof(ResourceRequirePlanDetail), oq);

            gridResourceRequireDetail.Rows.Clear();

            foreach (ResourceRequirePlanDetail dtl in listDtl)
            {
                AddResourceRequireDetailInGrid(dtl, false, true);
            }

            SetVisibleColumn();

        }

        private void ClearRequirePlan()
        {
            cbResourceType.SelectedIndex = 0;
            txtOwner.Text = "";
            txtCreateTime.Text = "";
            txtState.Text = "";

            tvwCategory.SelectedNode = null;
            optGWBS = null;

            gridResourceRequireDetail.Rows.Clear();
        }

        private void LoadResourceRequirePlanData(bool isLoadDetails)
        {
            if (optPlan != null)
            {
                txtOwner.Text = optPlan.HandlePersonName;
                txtCreateTime.Text = optPlan.CreateDate.ToString();
                txtState.Text = optPlan.State.ToString();

                if (isLoadDetails)
                {
                    string resType = cbResourceType.SelectedItem.ToString();

                    var query = from dtl in optPlan.Details
                                where dtl.MaterialName == resType
                                orderby dtl.ServiceType ascending
                                orderby dtl.MaterialName ascending
                                orderby dtl.TheGWBSTaskName ascending
                                orderby dtl.CreateTime ascending
                                select dtl;

                    foreach (ResourceRequirePlanDetail dtl in query)
                    {
                        AddResourceRequireDetailInGrid(dtl, false, true);
                    }
                }
            }
        }

        //�����ƻ�������Ϣ
        void btnAddPlan_Click(object sender, EventArgs e)
        {
            bool isSaveFlag = true;
            foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
            {
                if (!row.ReadOnly)
                {
                    isSaveFlag = false;
                    break;
                }
            }
            if (!isSaveFlag)
            {
                if (MessageBox.Show("��ϸ��δ���棬Ҫ������", "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!ValidateSaveDetail(null))
                        return;
                }
            }
            cbPlanName.Text = "";
            cbResourceType.SelectedIndex = 0;
            txtState.Text = ResourceRequirePlanState.�ƶ�.ToString();
            txtOwner.Text = ConstObject.LoginPersonInfo.Name;
            txtCreateTime.Text = "";

            cbPlanName.Focus();

        }
        //����ƻ�������Ϣ
        void btnSaveMaster_Click(object sender, EventArgs e)
        {
            string planName = cbPlanName.Text.Trim();
            if (planName == "")
            {
                MessageBox.Show("��ѡ��һ���ƻ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbPlanName.Focus();
                return;
            }
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("PlanType", planType));
            oq.AddCriterion(Expression.Eq("RequirePlanVersion", planName));
            IList listPlan = model.ObjectQuery(typeof(ResourceRequirePlan), oq);
            if (listPlan.Count > 0)
            {
                MessageBox.Show("�Ѵ�������Ϊ��" + planName + "���ļƻ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbPlanName.Focus();
                return;
            }

            try
            {
                optPlan = new ResourceRequirePlan();
                optPlan.ProjectId = projectInfo.Id;
                optPlan.ProjectName = projectInfo.Name;
                optPlan.PlanType = planType;
                optPlan.HandlePerson = ConstObject.LoginPersonInfo;
                optPlan.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                optPlan.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;

                optPlan.RequirePlanVersion = planName;
                optPlan.CreateDate = model.GetServerTime();
                optPlan.State = ResourceRequirePlanState.�ƶ�;

                optPlan = model.SaveOrUpdateResourceRequirePlan(optPlan);

                LogData log = new LogData();
                log.BillId = optPlan.Id;
                log.BillType = planType;
                log.Code = optPlan.Code;
                log.OperType = "����";
                log.Descript = "";
                log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                log.ProjectName = optPlan.ProjectName;
                Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.InsertLogData(log);


                LoadResourceRequirePlanData(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex), "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //�����ƻ�������Ϣ
        void btnPublishMaster_Click(object sender, EventArgs e)
        {
            string planName = cbPlanName.Text.Trim();
            if (planName == "")
            {
                MessageBox.Show("��ѡ��Ҫ�����ļƻ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbPlanName.Focus();
                return;
            }
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("PlanType", planType));
            oq.AddCriterion(Expression.Eq("RequirePlanVersion", planName));

            IList listPlan = model.ObjectQuery(typeof(ResourceRequirePlan), oq);
            if (listPlan.Count > 0)
            {
                ResourceRequirePlan plan = listPlan[0] as ResourceRequirePlan;
                if (plan.State == ResourceRequirePlanState.����)
                {
                    MessageBox.Show("�ƻ���" + planName + "���ѷ�����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else
            {
                MessageBox.Show("�ƻ���" + planName + "����δ���棬���ȱ��棡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnSave.Focus();
                return;
            }

            try
            {
                if (!ValidateSaveDetail(null))
                {
                    return;
                }

                oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", optPlan.Id));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                IList listTemp = model.ObjectQuery(typeof(ResourceRequirePlan), oq);
                optPlan = listTemp[0] as ResourceRequirePlan;

                optPlan.State = ResourceRequirePlanState.����;
                for (int i = 0; i < optPlan.Details.Count; i++)
                {
                    ResourceRequirePlanDetail dtl = optPlan.Details.ElementAt(i);
                    dtl.State = ResourceRequirePlanDetailState.����;
                }

                optPlan = model.SaveOrUpdateResourceRequirePlan(optPlan);

                ClearRequirePlan();

                LoadResourceRequirePlanData(false);


                oq.Criterions.Clear();
                oq.FetchModes.Clear();
                oq.AddCriterion(Expression.Eq("TheResourceRequirePlan.Id", optPlan.Id));
                //oq.AddFetchMode("TheGWBSTaskGUID", NHibernate.FetchMode.Eager);

                IList listDtl = model.ObjectQuery(typeof(ResourceRequirePlanDetail), oq);
                foreach (ResourceRequirePlanDetail dtl in listDtl)
                {
                    AddResourceRequireDetailInGrid(dtl, false, true);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex), "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region ��Դ����ƻ���ϸ
        void btnAdd_Click(object sender, EventArgs e)
        {
            if (optPlan == null)
            {
                MessageBox.Show("����ѡ��һ�����ƻ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbPlanName.Focus();
                return;
            }
            else if (optGWBS == null)
            {
                MessageBox.Show("����ѡ��һ����������ڵ㣡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (tvwCategory.Nodes.Count == 0)
                {
                    btnSelectWBSNode_Click(btnSelectWBSNode, new EventArgs());
                }
                return;
            }
            if (optPlan.State != ResourceRequirePlanState.�ƶ�)
            {
                MessageBox.Show("�üƻ�״̬Ϊ��" + optPlan.State + "��,��ѡ��״̬Ϊ���ƶ����ļƻ������ϸ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbPlanName.Focus();
                return;
            }

            ResourceRequirePlanDetail dtl = new ResourceRequirePlanDetail();
            dtl.TheGWBSTaskGUID = optGWBS;
            dtl.TheGWBSTaskName = optGWBS.Name;
            dtl.TheGWBSSysCode = optGWBS.SysCode;

            dtl.TheProjectGUID = projectInfo.Id;
            dtl.TheProjectName = projectInfo.Name;

            dtl.State = ResourceRequirePlanDetailState.����;
            DateTime serverTime = model.GetServerTime();
            dtl.StateUpdateTime = serverTime;

            dtl.MaterialName = cbResourceType.SelectedItem.ToString();

            AddResourceRequireDetailInGrid(dtl, true, false);

            gridResourceRequireDetail.BeginEdit(false);
        }

        void gridResourceRequireDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1 && gridResourceRequireDetail.Rows[e.RowIndex].ReadOnly == false)
            {
                ResourceRequirePlanDetail dtl = gridResourceRequireDetail.Rows[e.RowIndex].Tag as ResourceRequirePlanDetail;
                if (gridResourceRequireDetail.Columns[e.ColumnIndex].Name == ServiceType.Name)//��������
                {
                    VSelectServiceType frm = new VSelectServiceType();
                    frm.ShowDialog();
                    if (frm.SelectServiceType.Count > 0)
                    {
                        dtl.ServiceType = frm.SelectServiceType[0];
                        gridResourceRequireDetail.Rows[e.RowIndex].Cells[ServiceType.Name].Value = dtl.ServiceType;
                        gridResourceRequireDetail.Rows[e.RowIndex].Tag = dtl;

                        DateTime serverTime = model.GetServerTime();
                        for (int i = 1; i < frm.SelectServiceType.Count; i++)
                        {
                            ResourceRequirePlanDetail tempDtl = new ResourceRequirePlanDetail();

                            tempDtl.TheProjectGUID = projectInfo.Id;
                            tempDtl.TheProjectName = projectInfo.Name;

                            tempDtl.TheGWBSTaskGUID = dtl.TheGWBSTaskGUID;
                            tempDtl.TheGWBSTaskName = dtl.TheGWBSTaskName;
                            tempDtl.TheGWBSSysCode = dtl.TheGWBSSysCode;

                            tempDtl.State = ResourceRequirePlanDetailState.����;
                            tempDtl.StateUpdateTime = serverTime;

                            tempDtl.MaterialName = ResourceRequirePlanDetailResourceType.����.ToString();

                            tempDtl.ServiceType = frm.SelectServiceType[i];

                            InsertResourceRequireDetailInGrid(tempDtl, e.RowIndex);
                        }
                    }
                }
                else if (gridResourceRequireDetail.Columns[e.ColumnIndex].Name == ResourceCode.Name
                    || gridResourceRequireDetail.Columns[e.ColumnIndex].Name == ResourceName.Name
                    || gridResourceRequireDetail.Columns[e.ColumnIndex].Name == ResourceQuality.Name
                    || gridResourceRequireDetail.Columns[e.ColumnIndex].Name == Spec.Name)//ѡ������
                {
                    CommonMaterial materialSelector = new CommonMaterial();
                    materialSelector.OpenSelect();

                    IList list = materialSelector.Result;
                    if (list.Count > 0)
                    {
                        Material mat = list[0] as Material;
                        dtl.MaterialResource = mat;
                        dtl.MaterialCode = mat.Code;
                        dtl.MaterialName = mat.Name;
                        dtl.MaterialSpec = mat.Specification;
                        dtl.MaterialStuff = mat.Quality;

                        gridResourceRequireDetail.Rows[e.RowIndex].Cells[ResourceName.Name].Value = dtl.MaterialName;
                        gridResourceRequireDetail.Rows[e.RowIndex].Cells[ResourceCode.Name].Value = dtl.MaterialCode;
                        gridResourceRequireDetail.Rows[e.RowIndex].Cells[ResourceQuality.Name].Value = dtl.MaterialStuff;
                        gridResourceRequireDetail.Rows[e.RowIndex].Cells[Spec.Name].Value = dtl.MaterialSpec;
                        gridResourceRequireDetail.Rows[e.RowIndex].Tag = dtl;

                        DateTime serverTime = model.GetServerTime();
                        for (int i = 1; i < list.Count; i++)
                        {
                            Material theMaterial = list[i] as Material;

                            ResourceRequirePlanDetail tempDtl = new ResourceRequirePlanDetail();

                            tempDtl.TheProjectGUID = projectInfo.Id;
                            tempDtl.TheProjectName = projectInfo.Name;

                            tempDtl.TheGWBSTaskGUID = dtl.TheGWBSTaskGUID;
                            tempDtl.TheGWBSTaskName = dtl.TheGWBSTaskName;
                            tempDtl.TheGWBSSysCode = dtl.TheGWBSSysCode;

                            tempDtl.State = ResourceRequirePlanDetailState.����;
                            tempDtl.StateUpdateTime = serverTime;

                            tempDtl.MaterialName = ResourceRequirePlanDetailResourceType.����.ToString();

                            tempDtl.MaterialResource = theMaterial;
                            tempDtl.MaterialCode = theMaterial.Code;
                            tempDtl.MaterialName = theMaterial.Name;
                            tempDtl.MaterialSpec = theMaterial.Specification;
                            tempDtl.MaterialStuff = theMaterial.Quality;

                            InsertResourceRequireDetailInGrid(tempDtl, e.RowIndex);
                        }

                        gridResourceRequireDetail.CurrentCell = gridResourceRequireDetail.Rows[e.RowIndex].Cells[ResourceQuality.Name];
                        gridResourceRequireDetail.CurrentCell = gridResourceRequireDetail.Rows[e.RowIndex].Cells[ResourceName.Name];
                    }
                }
                else if (gridResourceRequireDetail.Columns[e.ColumnIndex].Name == QuantityUnit.Name)
                {
                    StandardUnit su = UCL.Locate("������λά��", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                    if (su != null)
                    {
                        dtl.QuantityUnitGUID = su;
                        dtl.QuantityUnitName = su.Name;

                        gridResourceRequireDetail.Rows[e.RowIndex].Cells[QuantityUnit.Name].Value = su.Name;
                        gridResourceRequireDetail.Rows[e.RowIndex].Cells[QuantityUnit.Name].Tag = su;

                        gridResourceRequireDetail.CurrentCell = gridResourceRequireDetail.Rows[e.RowIndex].Cells[0];
                        gridResourceRequireDetail.CurrentCell = gridResourceRequireDetail.Rows[e.RowIndex].Cells[QuantityUnit.Name];
                    }
                }
            }
        }


        void gridResourceRequireDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && gridResourceRequireDetail.Rows[e.RowIndex].ReadOnly == false)
            {
                //object value = gridResourceRequireDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue;
                object value = e.FormattedValue;
                if (value != null)
                {
                    try
                    {
                        string colName = gridResourceRequireDetail.Columns[e.ColumnIndex].Name;
                        if (colName == PlanQuantity.Name)//�ƻ�������
                        {
                            if (value.ToString() != "")
                                ClientUtil.ToDecimal(value);
                        }
                        else if (colName == QuantityUnit.Name)
                        {
                            string name = value.ToString();
                            if (name != "")
                            {
                                StandardUnit currUnit = gridResourceRequireDetail.Rows[e.RowIndex].Cells[QuantityUnit.Name].Tag as StandardUnit;

                                if (currUnit == null || (currUnit != null && currUnit.Name != name))
                                {
                                    ObjectQuery oq = new ObjectQuery();
                                    oq.AddCriterion(Expression.Eq("Name", name));
                                    IList list = model.ObjectQuery(typeof(StandardUnit), oq);
                                    if (list.Count > 0)
                                    {
                                        gridResourceRequireDetail.Rows[e.RowIndex].Cells[QuantityUnit.Name].Tag = list[0] as StandardUnit;
                                    }
                                    else
                                    {
                                        MessageBox.Show("ϵͳĿǰ�����ڸü�����λ�����飡");
                                        e.Cancel = true;
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("�����ʽ����ȷ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                    }
                }
            }
        }

        void gridResourceRequireDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && gridResourceRequireDetail.Rows[e.RowIndex].ReadOnly == false)
            {
                object tempValue = gridResourceRequireDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                string value = "";
                if (tempValue != null)
                    value = tempValue.ToString().Trim();

                ResourceRequirePlanDetail dtl = gridResourceRequireDetail.Rows[e.RowIndex].Tag as ResourceRequirePlanDetail;

                if (gridResourceRequireDetail.Columns[e.ColumnIndex].Name == ResourceName.Name &&
                    string.IsNullOrEmpty(value))//���ϱ�� �ô�ֻ�ж�ɾ�����ϵ����
                {
                    dtl.MaterialResource = null;
                    dtl.MaterialCode = "";
                    dtl.MaterialName = "";
                    dtl.MaterialSpec = "";
                    dtl.MaterialStuff = "";

                    UpdateResourceRequireDetailInGrid(e.RowIndex, dtl, false);
                }
                else if (gridResourceRequireDetail.Columns[e.ColumnIndex].Name == PlanQuantity.Name)//�ƻ�������
                {
                    if (!string.IsNullOrEmpty(value))
                        dtl.PlanRequireQuantity = ClientUtil.ToDecimal(value);
                    else
                        dtl.PlanRequireQuantity = 0;
                }
                else if (gridResourceRequireDetail.Columns[e.ColumnIndex].Name == QuantityUnit.Name)
                {
                    StandardUnit currUnit = gridResourceRequireDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag as StandardUnit;
                    if (!string.IsNullOrEmpty(value) && currUnit != null)
                    {
                        dtl.QuantityUnitGUID = currUnit;
                        dtl.QuantityUnitName = currUnit.Name;
                    }
                    else
                    {
                        dtl.QuantityUnitGUID = null;
                        dtl.QuantityUnitName = "";
                    }
                }
                else if (gridResourceRequireDetail.Columns[e.ColumnIndex].Name == DiagramNumber.Name)
                {
                    if (!string.IsNullOrEmpty(value))
                        dtl.DiagramNumber = value;
                }
            }
        }

        private void AddResourceRequireDetailInGrid(ResourceRequirePlanDetail dtl, bool isSetCurrentCell, bool isReadOnly)
        {
            int index = gridResourceRequireDetail.Rows.Add();
            DataGridViewRow row = gridResourceRequireDetail.Rows[index];

            row.Cells[TaskName.Name].ToolTipText = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.TheGWBSTaskName, dtl.TheGWBSSysCode);
            row.Cells[TaskName.Name].Value = dtl.TheGWBSTaskName;

            row.Cells[CreateTime.Name].Value = dtl.CreateTime.ToString();
            row.Cells[State.Name].Value = dtl.State.ToString();

            row.Cells[ServiceType.Name].Value = dtl.ServiceType;

            row.Cells[ResourceName.Name].Value = dtl.MaterialName;
            row.Cells[ResourceCode.Name].Value = dtl.MaterialCode;
            row.Cells[ResourceQuality.Name].Value = dtl.MaterialStuff;
            row.Cells[Spec.Name].Value = dtl.MaterialSpec;

            row.Cells[PlanQuantity.Name].Value = dtl.PlanRequireQuantity;

            row.Cells[QuantityUnit.Name].Value = dtl.QuantityUnitName;

            row.Cells[DiagramNumber.Name].Value = dtl.DiagramNumber;

            row.Tag = dtl;

            row.ReadOnly = isReadOnly;

            string resourceType = cbResourceType.SelectedItem.ToString();

            SetVisibleColumn();

            if (isSetCurrentCell)
            {
                if (resourceType == ResourceRequirePlanDetailResourceType.����.ToString())
                    gridResourceRequireDetail.CurrentCell = row.Cells[ResourceName.Name];
                else
                    gridResourceRequireDetail.CurrentCell = row.Cells[ServiceType.Name];
            }
        }
        private void InsertResourceRequireDetailInGrid(ResourceRequirePlanDetail dtl, int beforeIndex)
        {
            gridResourceRequireDetail.Rows.Insert(beforeIndex + 1, 1);
            DataGridViewRow row = gridResourceRequireDetail.Rows[beforeIndex + 1];

            row.Cells[TaskName.Name].Value = dtl.TheGWBSTaskName;
            row.Cells[TaskName.Name].ToolTipText = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.TheGWBSTaskName, dtl.TheGWBSSysCode);

            row.Cells[CreateTime.Name].Value = dtl.CreateTime.ToString();
            row.Cells[State.Name].Value = dtl.State.ToString();

            row.Cells[ServiceType.Name].Value = dtl.ServiceType;

            row.Cells[ResourceName.Name].Value = dtl.MaterialName;
            row.Cells[ResourceCode.Name].Value = dtl.MaterialCode;
            row.Cells[ResourceQuality.Name].Value = dtl.MaterialStuff;
            row.Cells[Spec.Name].Value = dtl.MaterialSpec;

            row.Cells[PlanQuantity.Name].Value = dtl.PlanRequireQuantity;

            row.Cells[QuantityUnit.Name].Value = dtl.QuantityUnitName;
            row.Cells[DiagramNumber.Name].Value = dtl.DiagramNumber;
            row.Tag = dtl;
        }
        private void UpdateResourceRequireDetailInGrid(ResourceRequirePlanDetail dtl, bool isSetCurrentCell, bool isReadOnly)
        {
            foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
            {
                ResourceRequirePlanDetail tempDtl = row.Tag as ResourceRequirePlanDetail;
                if (!string.IsNullOrEmpty(tempDtl.Id) && tempDtl.Id == dtl.Id)
                {
                    row.Cells[TaskName.Name].Value = dtl.TheGWBSTaskName;

                    row.Cells[CreateTime.Name].Value = dtl.CreateTime.ToString();
                    row.Cells[State.Name].Value = dtl.State.ToString();

                    row.Cells[ServiceType.Name].Value = dtl.ServiceType;

                    row.Cells[ResourceName.Name].Value = dtl.MaterialName;
                    row.Cells[ResourceCode.Name].Value = dtl.MaterialCode;
                    row.Cells[ResourceQuality.Name].Value = dtl.MaterialStuff;
                    row.Cells[Spec.Name].Value = dtl.MaterialSpec;

                    row.Cells[PlanQuantity.Name].Value = dtl.PlanRequireQuantity;

                    row.Cells[QuantityUnit.Name].Value = dtl.QuantityUnitName;
                    row.Cells[DiagramNumber.Name].Value = dtl.DiagramNumber;
                    row.Tag = dtl;

                    row.ReadOnly = isReadOnly;

                    if (isSetCurrentCell)
                    {
                        string resourceType = cbResourceType.SelectedItem.ToString();

                        if (resourceType == ResourceRequirePlanDetailResourceType.����.ToString())
                            gridResourceRequireDetail.CurrentCell = row.Cells[ResourceName.Name];
                        else
                            gridResourceRequireDetail.CurrentCell = row.Cells[ServiceType.Name];
                    }

                    break;
                }
            }
        }
        private void UpdateResourceRequireDetailInGrid(int updateRowIndex, ResourceRequirePlanDetail dtl, bool isSetCurrentCell)
        {
            DataGridViewRow row = gridResourceRequireDetail.Rows[updateRowIndex];

            row.Cells[TaskName.Name].Value = dtl.TheGWBSTaskName;
            row.Cells[CreateTime.Name].Value = dtl.CreateTime.ToString();
            row.Cells[State.Name].Value = dtl.State.ToString();

            row.Cells[ServiceType.Name].Value = dtl.ServiceType;

            row.Cells[ResourceName.Name].Value = dtl.MaterialName;
            row.Cells[ResourceCode.Name].Value = dtl.MaterialCode;
            row.Cells[ResourceQuality.Name].Value = dtl.MaterialStuff;
            row.Cells[Spec.Name].Value = dtl.MaterialSpec;

            row.Cells[PlanQuantity.Name].Value = dtl.PlanRequireQuantity;

            row.Cells[QuantityUnit.Name].Value = dtl.QuantityUnitName;
            row.Cells[DiagramNumber.Name].Value = dtl.DiagramNumber;
            row.Tag = dtl;

            if (isSetCurrentCell)
            {
                string resourceType = cbResourceType.SelectedItem.ToString();

                if (resourceType == ResourceRequirePlanDetailResourceType.����.ToString())
                    gridResourceRequireDetail.CurrentCell = row.Cells[ResourceName.Name];
                else
                    gridResourceRequireDetail.CurrentCell = row.Cells[ServiceType.Name];
            }
        }

        private void SetVisibleColumn()
        {
            string resourceType = cbResourceType.SelectedItem.ToString();
            if (resourceType == ResourceRequirePlanDetailResourceType.����.ToString())
            {
                ServiceType.Visible = false;
                ResourceName.Visible = true;
                ResourceQuality.Visible = true;
                //ResourceCode.Visible = true;
                Spec.Visible = true;
            }
            else
            {
                ServiceType.Visible = true;
                ResourceName.Visible = false;
                ResourceQuality.Visible = false;
                //ResourceCode.Visible = false;
                Spec.Visible = false;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (gridResourceRequireDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��һ��Ҫ�޸ĵļƻ���ϸ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (gridResourceRequireDetail.SelectedRows.Count > 1)
            {
                MessageBox.Show("һ��ֻ���޸�һ�У�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ResourceRequirePlanDetail dtl = gridResourceRequireDetail.SelectedRows[0].Tag as ResourceRequirePlanDetail;
            if (dtl.State != ResourceRequirePlanDetailState.����)
            {
                MessageBox.Show("��ѡ��״̬Ϊ�����ơ��ļƻ���ϸ�����޸ģ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string resourceType = cbResourceType.SelectedItem.ToString();
            if (resourceType == ResourceRequirePlanDetailResourceType.����.ToString())
            {
                gridResourceRequireDetail.CurrentCell = gridResourceRequireDetail.SelectedRows[0].Cells[ResourceName.Name];
            }
            else
            {
                gridResourceRequireDetail.CurrentCell = gridResourceRequireDetail.SelectedRows[0].Cells[ServiceType.Name];
            }

            gridResourceRequireDetail.SelectedRows[0].ReadOnly = false;
            gridResourceRequireDetail.SelectedRows[0].Selected = false;
            gridResourceRequireDetail.BeginEdit(false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gridResourceRequireDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫɾ�����У�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {

                IList list = new List<ResourceRequirePlanDetail>();
                List<int> listRowIndex = new List<int>();
                foreach (DataGridViewRow row in gridResourceRequireDetail.SelectedRows)
                {
                    ResourceRequirePlanDetail dtl = row.Tag as ResourceRequirePlanDetail;
                    if (dtl.State == ResourceRequirePlanDetailState.����)
                    {
                        if (!string.IsNullOrEmpty(dtl.Id))
                        {
                            list.Add(dtl);
                        }
                        listRowIndex.Add(row.Index);
                    }
                }

                if (listRowIndex.Count == 0)
                {
                    MessageBox.Show("ѡ����û�з���ɾ�����У���ѡ��״̬Ϊ�����ơ��ƻ���ϸ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("ɾ�����ָܻ�����ȷ��Ҫɾ��ѡ��ļƻ���ϸ��", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                if (list.Count > 0)
                {
                    model.DeleteResourceRequirePlanDetail(list);

                    foreach (ResourceRequirePlanDetail planDtl in list)
                    {
                        LogData log = new LogData();
                        log.BillId = planDtl.Id;
                        log.BillType = planType;
                        log.Code = "";
                        log.OperType = "ɾ��";
                        log.Descript = "";
                        log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                        log.ProjectName = planDtl.TheProjectName;
                        Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.InsertLogData(log);
                    }
                }

                listRowIndex.Sort();
                for (int i = listRowIndex.Count - 1; i > -1; i--)
                {
                    gridResourceRequireDetail.Rows.RemoveAt(listRowIndex[i]);
                }

                gridResourceRequireDetail.ClearSelection();

            }
            catch (Exception ex)
            {
                MessageBox.Show("ɾ��ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex), "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateSaveDetail(null))
                MessageBox.Show("����ɹ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //private bool ValidateSaveDetail()
        //{
        //    try
        //    {
        //        ObjectQuery oq = new ObjectQuery();
        //        if (optPlan != null)//ȷ��һ���ƻ�����༭��ϸ
        //        {
        //            oq.AddCriterion(Expression.Eq("Id", optPlan.Id));

        //            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

        //            IList listTemp = model.ObjectQuery(typeof(ResourceRequirePlan), oq);
        //            optPlan = listTemp[0] as ResourceRequirePlan;

        //            string resType = cbResourceType.SelectedItem.ToString();

        //            List<int> listRowIndex = new List<int>();
        //            List<int> listDetailIndex = new List<int>();
        //            foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
        //            {
        //                if (!row.ReadOnly)
        //                {
        //                    int detailIndex = 0;

        //                    ResourceRequirePlanDetail dtl = row.Tag as ResourceRequirePlanDetail;
        //                    if (resType == ResourceRequirePlanDetailResourceType.����.ToString() && dtl.MaterialGUID == null)
        //                    {
        //                        MessageBox.Show("��" + (row.Index + 1) + "δѡ���������ͣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                        gridResourceRequireDetail.CurrentCell = row.Cells[ResourceName.Name];
        //                        return false;
        //                    }
        //                    else if (resType == ResourceRequirePlanDetailResourceType.����.ToString() && string.IsNullOrEmpty(dtl.ServiceType))
        //                    {
        //                        MessageBox.Show("��" + (row.Index + 1) + "δѡ���������ͣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                        gridResourceRequireDetail.CurrentCell = row.Cells[ServiceType.Name];
        //                        return false;
        //                    }


        //                    if (!string.IsNullOrEmpty(dtl.Id))
        //                    {
        //                        for (int i = 0; i < optPlan.Details.Count; i++)
        //                        {
        //                            ResourceRequirePlanDetail temp = optPlan.Details.ElementAt(i);
        //                            if (temp.Id == dtl.Id)
        //                            {
        //                                temp.MaterialName = dtl.MaterialName;
        //                                temp.ServiceType = dtl.ServiceType;

        //                                temp.MaterialGUID = dtl.MaterialGUID;
        //                                temp.MaterialCode = dtl.MaterialCode;
        //                                temp.MaterialName = dtl.MaterialName;
        //                                temp.MaterialSpec = dtl.MaterialSpec;
        //                                temp.MaterialQuality = dtl.MaterialQuality;

        //                                temp.FirstOfferRequireQuantity = dtl.FirstOfferRequireQuantity;
        //                                temp.ResponsibilityRequireQuantity = dtl.ResponsibilityRequireQuantity;
        //                                temp.PlanRequireQuantity = dtl.PlanRequireQuantity;

        //                                temp.MonthPlanPublishQuantity = dtl.MonthPlanPublishQuantity;
        //                                temp.ApproachPlanPublishQuantity = dtl.ApproachPlanPublishQuantity;
        //                                temp.ApproachPlanExecuteQuantity = dtl.ApproachPlanExecuteQuantity;

        //                                temp.PlanBeginApproachDate = dtl.PlanBeginApproachDate;
        //                                temp.PlanEndApproachDate = dtl.PlanEndApproachDate;

        //                                detailIndex = i;
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        dtl.TheResourceRequirePlan = optPlan;
        //                        optPlan.Details.Add(dtl);

        //                        detailIndex = optPlan.Details.Count - 1;
        //                    }

        //                    listRowIndex.Add(row.Index);

        //                    listDetailIndex.Add(detailIndex);
        //                }
        //            }

        //            if (listRowIndex.Count == 0)
        //            {
        //                return true;
        //            }

        //            IList listLog = new ArrayList();

        //            for (int i = 0; i < listRowIndex.Count; i++)
        //            {
        //                int rowIndex = listRowIndex[i];
        //                ResourceRequirePlanDetail oldPlanDtl = gridResourceRequireDetail.Rows[rowIndex].Tag as ResourceRequirePlanDetail;

        //                LogData log = new LogData();
        //                log.BillId = oldPlanDtl.Id;
        //                log.BillType = planType;
        //                log.Code = "";
        //                if (string.IsNullOrEmpty(oldPlanDtl.Id))
        //                    log.OperType = "����";
        //                else
        //                    log.OperType = "�޸�";
        //                log.Descript = "";
        //                log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
        //                log.ProjectName = oldPlanDtl.TheProjectName;
        //                listLog.Add(log);
        //                //Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.InsertLogData(log);
        //            }

        //            optPlan = model.SaveOrUpdateResourceRequirePlan(optPlan);

        //            for (int i = 0; i < listRowIndex.Count; i++)
        //            {
        //                int rowIndex = listRowIndex[i];

        //                ResourceRequirePlanDetail planDtl = optPlan.Details.ElementAt(listDetailIndex[i]);

        //                LogData log = listLog[i] as LogData;
        //                log.BillId = planDtl.Id;
        //                Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.InsertLogData(log);

        //                gridResourceRequireDetail.Rows[rowIndex].Tag = planDtl;
        //                gridResourceRequireDetail.Rows[rowIndex].ReadOnly = true;
        //            }
        //        }
        //        else//��ѯ�޸�
        //        {

        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex), "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return false;
        //    }
        //}

        private bool ValidateSaveDetail(string resType)
        {
            try
            {
                if (string.IsNullOrEmpty(resType))
                    resType = cbResourceType.SelectedItem.ToString();

                ObjectQuery oq = new ObjectQuery();
                if (optPlan != null)//ȷ��һ���ƻ�����༭��ϸ
                {
                    oq.AddCriterion(Expression.Eq("Id", optPlan.Id));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                    IList listTemp = model.ObjectQuery(typeof(ResourceRequirePlan), oq);
                    optPlan = listTemp[0] as ResourceRequirePlan;

                    List<int> listRowIndex = new List<int>();
                    List<int> listDetailIndex = new List<int>();
                    foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
                    {
                        if (!row.ReadOnly)
                        {
                            int detailIndex = 0;

                            ResourceRequirePlanDetail dtl = row.Tag as ResourceRequirePlanDetail;
                            if (resType == ResourceRequirePlanDetailResourceType.����.ToString() && dtl.MaterialResource == null)
                            {
                                MessageBox.Show("��" + (row.Index + 1) + "δѡ���������ͣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                gridResourceRequireDetail.CurrentCell = row.Cells[ResourceName.Name];
                                return false;
                            }
                            else if (resType == ResourceRequirePlanDetailResourceType.����.ToString() && string.IsNullOrEmpty(dtl.ServiceType))
                            {
                                MessageBox.Show("��" + (row.Index + 1) + "δѡ���������ͣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                gridResourceRequireDetail.CurrentCell = row.Cells[ServiceType.Name];
                                return false;
                            }


                            if (!string.IsNullOrEmpty(dtl.Id))
                            {
                                for (int i = 0; i < optPlan.Details.Count; i++)
                                {
                                    ResourceRequirePlanDetail temp = optPlan.Details.ElementAt(i);
                                    if (temp.Id == dtl.Id)
                                    {
                                        temp.MaterialName = dtl.MaterialName;
                                        temp.ServiceType = dtl.ServiceType;

                                        temp.MaterialResource = dtl.MaterialResource;
                                        temp.MaterialCode = dtl.MaterialCode;
                                        temp.MaterialName = dtl.MaterialName;
                                        temp.MaterialSpec = dtl.MaterialSpec;
                                        temp.MaterialStuff = dtl.MaterialStuff;

                                        temp.FirstOfferRequireQuantity = dtl.FirstOfferRequireQuantity;
                                        temp.ResponsibilityRequireQuantity = dtl.ResponsibilityRequireQuantity;
                                        temp.PlanRequireQuantity = dtl.PlanRequireQuantity;

                                        temp.MonthPlanPublishQuantity = dtl.MonthPlanPublishQuantity;
                                        temp.ApproachPlanPublishQuantity = dtl.ApproachPlanPublishQuantity;
                                        temp.ApproachPlanExecuteQuantity = dtl.ApproachPlanExecuteQuantity;

                                        temp.PlanBeginApproachDate = dtl.PlanBeginApproachDate;
                                        temp.PlanEndApproachDate = dtl.PlanEndApproachDate;

                                        temp.QuantityUnitGUID = dtl.QuantityUnitGUID;
                                        temp.QuantityUnitName = dtl.QuantityUnitName;
                                        temp.DiagramNumber = dtl.DiagramNumber;
                                        detailIndex = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                dtl.TheResourceRequirePlan = optPlan;
                                optPlan.Details.Add(dtl);

                                detailIndex = optPlan.Details.Count - 1;
                            }

                            listRowIndex.Add(row.Index);

                            listDetailIndex.Add(detailIndex);
                        }
                    }

                    if (listRowIndex.Count == 0)
                    {
                        return true;
                    }

                    IList listLog = new ArrayList();

                    for (int i = 0; i < listRowIndex.Count; i++)
                    {
                        int rowIndex = listRowIndex[i];
                        ResourceRequirePlanDetail oldPlanDtl = gridResourceRequireDetail.Rows[rowIndex].Tag as ResourceRequirePlanDetail;

                        LogData log = new LogData();
                        log.BillId = oldPlanDtl.Id;
                        log.BillType = planType;
                        log.Code = "";
                        if (string.IsNullOrEmpty(oldPlanDtl.Id))
                            log.OperType = "����";
                        else
                            log.OperType = "�޸�";
                        log.Descript = "";
                        log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                        log.ProjectName = oldPlanDtl.TheProjectName;
                        listLog.Add(log);
                        //Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.InsertLogData(log);
                    }

                    optPlan = model.SaveOrUpdateResourceRequirePlan(optPlan);

                    for (int i = 0; i < listRowIndex.Count; i++)
                    {
                        int rowIndex = listRowIndex[i];

                        ResourceRequirePlanDetail planDtl = optPlan.Details.ElementAt(listDetailIndex[i]);

                        LogData log = listLog[i] as LogData;
                        log.BillId = planDtl.Id;
                        Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.InsertLogData(log);

                        gridResourceRequireDetail.Rows[rowIndex].Tag = planDtl;
                        gridResourceRequireDetail.Rows[rowIndex].ReadOnly = true;
                    }
                }
                else //��ѯ�޸�
                {
                    #region ��ѯ�޸�

                    IList listUpdate = new List<ResourceRequirePlanDetail>();
                    foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
                    {
                        if (!row.ReadOnly)
                        {
                            ResourceRequirePlanDetail dtl = row.Tag as ResourceRequirePlanDetail;

                            if (resType == ResourceRequirePlanDetailResourceType.����.ToString() && dtl.MaterialResource == null)
                            {
                                MessageBox.Show("��" + (row.Index + 1) + "δѡ���������ͣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                gridResourceRequireDetail.CurrentCell = row.Cells[ResourceName.Name];
                                return false;
                            }
                            else if (resType == ResourceRequirePlanDetailResourceType.����.ToString() && string.IsNullOrEmpty(dtl.ServiceType))
                            {
                                MessageBox.Show("��" + (row.Index + 1) + "δѡ���������ͣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                gridResourceRequireDetail.CurrentCell = row.Cells[ServiceType.Name];
                                return false;
                            }

                            if (!string.IsNullOrEmpty(dtl.Id))
                            {

                                ResourceRequirePlanDetail temp = model.GetObjectById(typeof(ResourceRequirePlanDetail), dtl.Id) as ResourceRequirePlanDetail;

                                temp.MaterialName = dtl.MaterialName;
                                temp.ServiceType = dtl.ServiceType;

                                temp.MaterialResource = dtl.MaterialResource;
                                temp.MaterialCode = dtl.MaterialCode;
                                temp.MaterialName = dtl.MaterialName;
                                temp.MaterialSpec = dtl.MaterialSpec;
                                temp.MaterialStuff = dtl.MaterialStuff;

                                temp.FirstOfferRequireQuantity = dtl.FirstOfferRequireQuantity;
                                temp.ResponsibilityRequireQuantity = dtl.ResponsibilityRequireQuantity;
                                temp.PlanRequireQuantity = dtl.PlanRequireQuantity;

                                temp.MonthPlanPublishQuantity = dtl.MonthPlanPublishQuantity;
                                temp.ApproachPlanPublishQuantity = dtl.ApproachPlanPublishQuantity;
                                temp.ApproachPlanExecuteQuantity = dtl.ApproachPlanExecuteQuantity;

                                temp.PlanBeginApproachDate = dtl.PlanBeginApproachDate;
                                temp.PlanEndApproachDate = dtl.PlanEndApproachDate;

                                temp.QuantityUnitGUID = dtl.QuantityUnitGUID;
                                temp.QuantityUnitName = dtl.QuantityUnitName;

                                listUpdate.Add(temp);
                            }
                        }
                    }

                    listUpdate = model.SaveOrUpdateResourcePlanDetail(listUpdate);

                    for (int i = 0; i < listUpdate.Count; i++)
                    {
                        ResourceRequirePlanDetail planDtl = listUpdate[i] as ResourceRequirePlanDetail;

                        LogData log = new LogData();
                        log.BillId = planDtl.Id;
                        log.BillType = planType;
                        log.Code = "";
                        log.OperType = "�޸�";
                        log.Descript = "";
                        log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                        log.ProjectName = planDtl.TheProjectName;

                        Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.InsertLogData(log);

                        UpdateResourceRequireDetailInGrid(planDtl, false, true);
                    }
                    #endregion
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex), "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            if (gridResourceRequireDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫ�������У�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            IList list = new List<ResourceRequirePlanDetail>();
            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridResourceRequireDetail.SelectedRows)
            {
                ResourceRequirePlanDetail dtl = row.Tag as ResourceRequirePlanDetail;
                if (!string.IsNullOrEmpty(dtl.Id) && dtl.State == ResourceRequirePlanDetailState.����)
                {
                    ResourceRequirePlanDetail temp = model.GetObjectById(typeof(ResourceRequirePlanDetail), dtl.Id) as ResourceRequirePlanDetail;
                    temp.State = ResourceRequirePlanDetailState.����;

                    list.Add(temp);

                    listRowIndex.Add(row.Index);
                }
            }
            if (list.Count == 0)
            {
                MessageBox.Show("ѡ����û����Ҫ�����ļƻ���ϸ����ѡ��״̬Ϊ�����ơ��ļƻ���ϸ��\n�����δ�������ϸ�����ȱ�����ٷ�����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                list = model.SaveOrUpdateResourcePlanDetail(list);

                for (int i = 0; i < listRowIndex.Count; i++)
                {
                    int rowIndex = listRowIndex[i];
                    gridResourceRequireDetail.Rows[rowIndex].Tag = list[i];
                    gridResourceRequireDetail.Rows[rowIndex].Cells["State"].Value = (list[i] as ResourceRequirePlanDetail).State.ToString();
                    gridResourceRequireDetail.Rows[rowIndex].ReadOnly = true;
                }

                MessageBox.Show("�����ɹ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex), "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancellation_Click(object sender, EventArgs e)
        {
            if (gridResourceRequireDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫ���ϵ��У�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            IList list = new List<ResourceRequirePlanDetail>();
            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridResourceRequireDetail.SelectedRows)
            {
                ResourceRequirePlanDetail dtl = row.Tag as ResourceRequirePlanDetail;
                if (!string.IsNullOrEmpty(dtl.Id) && dtl.State == ResourceRequirePlanDetailState.����)
                {
                    ResourceRequirePlanDetail temp = model.GetObjectById(typeof(ResourceRequirePlanDetail), dtl.Id) as ResourceRequirePlanDetail;
                    temp.State = ResourceRequirePlanDetailState.����;

                    list.Add(temp);

                    listRowIndex.Add(row.Index);
                }
            }
            if (list.Count == 0)
            {
                MessageBox.Show("ѡ����û�з������ϵļƻ���ϸ����ѡ��״̬Ϊ���������ļƻ���ϸ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                list = model.SaveOrUpdateResourcePlanDetail(list);

                for (int i = 0; i < listRowIndex.Count; i++)
                {
                    int rowIndex = listRowIndex[i];

                    gridResourceRequireDetail.Rows[rowIndex].Tag = list[i];
                    gridResourceRequireDetail.Rows[rowIndex].Cells["State"].Value = (list[i] as ResourceRequirePlanDetail).State.ToString();
                    gridResourceRequireDetail.Rows[rowIndex].ReadOnly = true;
                }

                MessageBox.Show("�������óɹ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("��������ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex), "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        public void Start()
        {
            RefreshState(MainViewState.Browser);
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
