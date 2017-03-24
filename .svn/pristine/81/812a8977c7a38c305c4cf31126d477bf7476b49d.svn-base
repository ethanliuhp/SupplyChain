using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using System.Collections;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VGWBSTreeDetailEdit : TBasicDataView
    {
        public MGWBSTree model = new MGWBSTree();

        public VGWBSTreeDetailEdit()
        {
            InitializeComponent();

            InitForm();
        }

        GWBSDetail oprDtl = null;

        CurrentProjectInfo projectInfo = null;

        private void InitForm()
        {
            InitEvents();

            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();

            //foreach (string state in Enum.GetNames(typeof(GWBSDetailState)))
            //{
            //    cbStateBySearch.Items.Add(state);
            //}

            foreach (string mode in Enum.GetNames(typeof(GWBSTreeManagementMode)))
            {
                cbDtlManagementMethod.Items.Add(mode);
            }

            RefreshDetailControls(MainViewState.Browser);

        }

        private void InitEvents()
        {
            txtCostItemBySearch.LostFocus += new EventHandler(txtCostItemBySearch_LostFocus);
            btnSelectCostItemBySearch.Click += new EventHandler(btnSelectCostItemBySearch_Click);
            btnSelectGWBSBySearch.Click += new EventHandler(btnSelectGWBSBySearch_Click);

            btnQuery.Click += new EventHandler(btnQuery_Click);

            btnSelectContractGroup.Click += new EventHandler(btnSelectContractGroup_Click);

            #region 任务明细
            btnUpdateDetail.Click += new EventHandler(btnUpdateDetail_Click);
            btnDeleteDetail.Click += new EventHandler(btnDeleteDetail_Click);
            btnPublishDetail.Click += new EventHandler(btnPublishDetail_Click);
            btnCancellationDetail.Click += new EventHandler(btnCancellationDetail_Click);
            btnSaveTaskDetail.Click += new EventHandler(btnSaveTaskDetail_Click);
            btnSaveBaseInfo.Click += new EventHandler(btnSave_Click);

            btnSelectCostItem.Click += new EventHandler(btnSelectCostItem_Click);

            gridGWBDetail.CellClick += new DataGridViewCellEventHandler(gridGWBDetail_CellClick);

            txtDtlCostItem.LostFocus += new EventHandler(txtDtlCostItem_LostFocus);


            txtBudgetContractPrice.TextChanged += new EventHandler(txtBudgetContractPrice_TextChanged);
            txtBudgetContractProjectAmount.TextChanged += new EventHandler(txtBudgetContractProjectAmount_TextChanged);

            txtBudgetResponsibilityPrice.TextChanged += new EventHandler(txtBudgetResponsibilityPrice_TextChanged);
            txtBudgetResponsibilityProjectAmount.TextChanged += new EventHandler(txtBudgetResponsibilityProjectAmount_TextChanged);

            txtBudgetPlanPrice.TextChanged += new EventHandler(txtBudgetPlanPrice_TextChanged);
            txtBudgetPlanProjectAmount.TextChanged += new EventHandler(txtBudgetPlanProjectAmount_TextChanged);


            #endregion

            #region 任务明细分科目成本
            btnAddCostSubject.Click += new EventHandler(btnAddCostSubject_Click);
            btnUpdateCostSubject.Click += new EventHandler(btnUpdateCostSubject_Click);
            btnDeleteCostSubject.Click += new EventHandler(btnDeleteCostSubject_Click);
            #endregion
        }

        void txtCostItemBySearch_LostFocus(object sender, EventArgs e)
        {
            string costItemCode = txtCostItemBySearch.Text.Trim();
            if (string.IsNullOrEmpty(costItemCode))
            {
                txtCostItemBySearch.Tag = null;
                //MessageBox.Show("请输入成本项编号或定额编号，或选择一个成本项！");
                return;
            }

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Or(Expression.Eq("Code", costItemCode), Expression.Eq("QuotaCode", costItemCode)));
            IList list = model.ObjectQuery(typeof(CostItem), oq);
            if (list != null && list.Count > 0)
            {
                CostItem item = list[0] as CostItem;
                txtCostItemBySearch.Text = item.Code;
                txtCostItemBySearch.Tag = item;
            }
            else
            {
                txtCostItemBySearch.Tag = null;
                MessageBox.Show("请检查输入的成本项编号或定额编号，未找到相关成本项！");
            }
        }

        void btnSelectCostItemBySearch_Click(object sender, EventArgs e)
        {
            VSelectCostItem frm = new VSelectCostItem(new MCostItem());
            if (txtDtlCostItem.Tag != null)
            {
                frm.DefaultSelectedCostItem = txtDtlCostItem.Tag as CostItem;
            }
            frm.ShowDialog();
            if (frm.SelectCostItem != null)
            {
                txtCostItemBySearch.Text = frm.SelectCostItem.Code;
                txtDtlCostItem.Tag = frm.SelectCostItem;
            }
        }

        void btnSelectGWBSBySearch_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree frm = new VSelectGWBSTree(new MGWBSTree());
            if (txtProTaskNameBySearch.Tag != null)
            {
                frm.DefaultSelectedGWBS = txtProTaskNameBySearch.Tag as GWBSTree;
            }
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                TreeNode root = frm.SelectResult[0];

                GWBSTree task = root.Tag as GWBSTree;
                if (task != null)
                {
                    txtProTaskNameBySearch.Text = task.Name;
                    txtProTaskNameBySearch.Tag = task;
                }
            }
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            gridGWBDetail.Rows.Clear();
            ClearDetailAll();
            QueryGWBSDetailInGrid();
        }

        private void QueryGWBSDetailInGrid()
        {
            ObjectQuery oq = new ObjectQuery();

            if (cbResponseAccountBySearch.SelectedItem != null && cbResponseAccountBySearch.SelectedItem.ToString().Trim() == "是")
                oq.AddCriterion(Expression.Eq("ResponseFlag", 1));
            else if (cbResponseAccountBySearch.SelectedItem != null && cbResponseAccountBySearch.SelectedText.Trim() == "否")
                oq.AddCriterion(Expression.Eq("ResponseFlag", 0));

            if (cbCostAccountBySearch.SelectedItem != null && cbCostAccountBySearch.SelectedItem.ToString().Trim() == "是")
                oq.AddCriterion(Expression.Eq("CostingFlag", 1));
            else if (cbCostAccountBySearch.SelectedItem != null && cbCostAccountBySearch.SelectedItem.ToString().Trim() == "否")
                oq.AddCriterion(Expression.Eq("CostingFlag", 0));

            if (cbProductConfirmBySearch.SelectedItem != null && cbProductConfirmBySearch.SelectedItem.ToString().Trim() == "是")
                oq.AddCriterion(Expression.Eq("ProduceConfirmFlag", 1));
            else if (cbProductConfirmBySearch.SelectedItem != null && cbProductConfirmBySearch.SelectedText.Trim() == "否")
                oq.AddCriterion(Expression.Eq("ProduceConfirmFlag", 0));

            if (cbSubContractFeeBySearch.SelectedItem != null && cbSubContractFeeBySearch.SelectedItem.ToString().Trim() == "是")
                oq.AddCriterion(Expression.Eq("SubContractFeeFlag", true));
            else if (cbSubContractFeeBySearch.SelectedItem != null && cbSubContractFeeBySearch.SelectedText.Trim() == "否")
                oq.AddCriterion(Expression.Eq("SubContractFeeFlag", false));

            //if (cbStateBySearch.SelectedText.Trim() != "")
            //    oq.AddCriterion(Expression.Eq("State", VirtualMachine.Component.Util.EnumUtil<GWBSDetailState>.FromDescription(cbStateBySearch.SelectedText.Trim())));

            //管理方式
            //if (cbMngMethodBySearch.SelectedText.Trim() != "")
            //    oq.AddCriterion(Expression.Eq("State", VirtualMachine.Component.Util.EnumUtil<GWBSDetailState>.FromDescription(cbStateBySearch.SelectedText.Trim())));


            if (txtDtlNameBySearch.Text.Trim() != "")
                oq.AddCriterion(Expression.Like("Name", txtDtlNameBySearch.Text.Trim(), MatchMode.Anywhere));
            if (txtDtlCodeBySearch.Text.Trim() != "")
                oq.AddCriterion(Expression.Like("Code", txtDtlCodeBySearch.Text.Trim(), MatchMode.Anywhere));
            if (txtDtlDescBySearch.Text.Trim() != "")
                oq.AddCriterion(Expression.Like("ContentDesc", txtDtlDescBySearch.Text.Trim(), MatchMode.Anywhere));


            if (txtUsedPartBySearch.Text.Trim() != "")
                oq.AddCriterion(Expression.Like("WorkPart", txtUsedPartBySearch.Text.Trim(), MatchMode.Anywhere));
            if (txtMaterialBySearch.Text.Trim() != "")
                oq.AddCriterion(Expression.Like("WorkUseMaterial", txtMaterialBySearch.Text.Trim(), MatchMode.Anywhere));
            if (txtMethodBySearch.Text.Trim() != "")
                oq.AddCriterion(Expression.Like("WorkMethod", txtMethodBySearch.Text.Trim(), MatchMode.Anywhere));

            if (txtCostItemBySearch.Text.Trim() != "" && txtCostItemBySearch.Tag != null)
                oq.AddCriterion(Expression.Eq("TheCostItem.Id", (txtCostItemBySearch.Tag as CostItem).Id));

            if (txtProTaskNameBySearch.Text.Trim() != "" && txtProTaskNameBySearch.Tag != null)
                oq.AddCriterion(Expression.Eq("TheGWBS.Id", (txtProTaskNameBySearch.Tag as GWBSTree).Id));

            oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
            oq.AddOrder(NHibernate.Criterion.Order.Asc("CreateTime"));

            IList list = model.ObjectQuery(typeof(GWBSDetail), oq);

            gridGWBDetail.Rows.Clear();
            foreach (GWBSDetail dtl in list)
            {
                AddDetailInfoInGrid(dtl);
            }
            gridGWBDetail.ClearSelection();
        }

        //显示任务明细信息
        private void GetTaskNodeDetailInfo()
        {
            try
            {
                ClearDetailAll();

                //基本信息
                txtDtlName.Text = oprDtl.Name;
                txtDtlCode.Text = oprDtl.Code;

                ObjectQuery oqDtl = new ObjectQuery();
                oqDtl.AddCriterion(Expression.Eq("Id", oprDtl.Id));
                oqDtl.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
                oprDtl = model.ObjectQuery(typeof(GWBSDetail), oqDtl)[0] as GWBSDetail;

                if (oprDtl.TheCostItem != null)
                {
                    txtDtlCostItem.Text = oprDtl.TheCostItem.Code;
                    txtDtlCostItem.Tag = oprDtl.TheCostItem;
                }

                try
                {
                    cbDtlOrg.Text = oprDtl.BearOrgName;
                }
                catch { }
                cbDtlOrg.Tag = oprDtl.BearOrgGUID;

                txtDtlDesc.Text = oprDtl.ContentDesc;

                txtDtlUsedPart.Text = oprDtl.WorkPart;
                txtDtlMaterial.Text = oprDtl.WorkUseMaterial;
                txtDtlMethod.Text = oprDtl.WorkMethod;

                cbDtlManagementMethod.SelectedIndex = 0;

                txtDtlState.Text = StaticMethod.GetWBSTaskStateText(oprDtl.State);
                txtDtlStateTime.Text = oprDtl.CurrentStateTime.ToString();

                txtDtlContractGroupCode.Text = oprDtl.ContractGroupCode;
                txtDtlContractGroupCode.Tag = oprDtl.ContractGroupGUID;
                txtDtlContractGroupType.Text = oprDtl.ContractGroupType;

                txtDtlFinishedProjectAmount.Text = oprDtl.FinishedWorkAmount * 100 + "%";
                txtDtlFinishedPercent.Text = oprDtl.TaskFinishedPercent * 100 + "%";

                cbResponseAccount.Checked = oprDtl.ResponseFlag == 1;
                cbCostAccount.Checked = oprDtl.CostingFlag == 1;
                cbProductConfirm.Checked = oprDtl.ProduceConfirmFlag == 1;
                cbSubContractFee.Checked = oprDtl.SubContractFeeFlag;

                //预算信息
                txtBudgetProjectUnit.Text = oprDtl.WorkAmountUnitName;
                txtBudgetPriceUnit.Text = oprDtl.PriceUnitName;

                txtBudgetContractProjectAmount.Text = oprDtl.ContractProjectQuantity.ToString();
                txtBudgetContractPrice.Text = oprDtl.ContractPrice.ToString();
                txtBudgetContractTotalPrice.Text = oprDtl.ContractTotalPrice.ToString();

                txtBudgetResponsibilityProjectAmount.Text = oprDtl.ResponsibilitilyWorkAmount.ToString();
                txtBudgetResponsibilityPrice.Text = oprDtl.ResponsibilitilyPrice.ToString();
                txtBudgetResponsibilityTotalPrice.Text = oprDtl.ResponsibilitilyTotalPrice.ToString();

                txtBudgetPlanProjectAmount.Text = oprDtl.PlanWorkAmount.ToString();
                txtBudgetPlanPrice.Text = oprDtl.PlanPrice.ToString();
                txtBudgetPlanTotalPrice.Text = oprDtl.PlanTotalPrice.ToString();


                //加载分科目成本信息
                gridCostSubject.Rows.Clear();
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheGWBSDetail.Id", oprDtl.Id));
                oq.AddOrder(NHibernate.Criterion.Order.Asc("CreateTime"));
                IList listQuota = model.ObjectQuery(typeof(GWBSDetailCostSubject), oq);
                foreach (GWBSDetailCostSubject subject in listQuota)
                {
                    AddCostSubjectInfoInGrid(subject);
                }

                RefreshDetailControls(MainViewState.Browser);
            }
            catch (Exception exp)
            {
                MessageBox.Show("显示出错：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        private bool ValideSave()
        {
            try
            {
                if (oprDtl == null)
                {
                    if (!valideContractGroup())
                    {
                        return false;
                    }
                }
                else if (!string.IsNullOrEmpty(oprDtl.Id))
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", oprDtl.Id));
                    oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
                    oq.AddFetchMode("ListCostSubjectDetails", NHibernate.FetchMode.Eager);
                    IList list = model.ObjectQuery(typeof(GWBSDetail), oq);
                    oprDtl = list[0] as GWBSDetail;
                }

                if (string.IsNullOrEmpty(oprDtl.TheProjectGUID) || string.IsNullOrEmpty(oprDtl.TheProjectName))
                {
                    if (projectInfo != null)
                    {
                        oprDtl.TheProjectGUID = projectInfo.Id;
                        oprDtl.TheProjectName = projectInfo.Name;
                    }
                }

                #region 基本信息
                if (txtDtlName.Text.Trim() == "")
                {
                    tabBaseInfo.SelectedIndex = 0;
                    MessageBox.Show("明细名称不能为空！");
                    txtDtlName.Focus();
                    return false;
                }
                oprDtl.Name = txtDtlName.Text.Trim();

                if (txtDtlCostItem.Tag != null)//成本项
                {
                    if (string.IsNullOrEmpty(oprDtl.Id))
                        oprDtl.ListCostSubjectDetails.Clear();

                    CostItem item = txtDtlCostItem.Tag as CostItem;
                    if (item != null)
                    {
                        oprDtl.TheCostItem = item;
                    }

                    //把分科目成本加进来 
                    for (int i = gridCostSubject.Rows.Count - 1; i > -1; i--)
                    {
                        DataGridViewRow row = gridCostSubject.Rows[i];

                        GWBSDetailCostSubject subject = row.Tag as GWBSDetailCostSubject;

                        if (string.IsNullOrEmpty(subject.Id) && string.IsNullOrEmpty(subject.TheProjectGUID))//从成本项里带过来的信息Id为null且projectId为null，有可能在保存之前被修改
                        {
                            if (projectInfo != null)
                            {
                                subject.TheProjectGUID = projectInfo.Id;
                                subject.TheProjectName = projectInfo.Name;
                            }
                            else
                            {
                                subject.TheProjectGUID = oprDtl.TheProjectGUID;
                                subject.TheProjectName = oprDtl.TheProjectName;
                            }

                            subject.TheGWBSDetail = oprDtl;

                            subject.TheGWBSTree = oprDtl.TheGWBS;
                            subject.TheGWBSTreeName = oprDtl.TheGWBS.Name;
                            subject.TheGWBSTreeSyscode = oprDtl.TheGWBS.SysCode;

                            oprDtl.ListCostSubjectDetails.Add(subject);
                        }
                        else if (string.IsNullOrEmpty(subject.Id) && !string.IsNullOrEmpty(subject.TheProjectGUID))//手动添加的信息Id为null且projectId不为null
                        {
                            subject.TheGWBSDetail = oprDtl;
                            oprDtl.ListCostSubjectDetails.Add(subject);
                        }
                    }
                }
                else
                {
                    tabBaseInfo.SelectedIndex = 0;
                    MessageBox.Show("请选择成本项！");
                    txtDtlCostItem.Focus();
                    return false;
                }

                if (cbDtlOrg.Result != null && cbDtlOrg.Result.Count > 0) //承担组织
                {
                    SupplierRelationInfo org = cbDtlOrg.Result[0] as SupplierRelationInfo;
                    if (org != null)
                    {
                        oprDtl.BearOrgGUID = org;
                        oprDtl.BearOrgName = cbDtlOrg.Text;
                    }
                }
                else if (cbDtlOrg.Tag == null)
                {
                    tabBaseInfo.SelectedIndex = 0;
                    MessageBox.Show("请选择责任组织！");
                    cbDtlOrg.Focus();
                    return false;
                }

                oprDtl.WorkPart = txtDtlUsedPart.Text.Trim();
                oprDtl.WorkUseMaterial = txtDtlMaterial.Text.Trim();
                oprDtl.WorkMethod = txtDtlMethod.Text.Trim();
                oprDtl.ContentDesc = txtDtlDesc.Text.Trim();

                ContractGroup cg = txtContractGroupCode.Tag as ContractGroup;
                if (oprDtl.ContractGroupGUID != cg.Id)
                {
                    oprDtl.ContractGroupGUID = cg.Id;
                    oprDtl.ContractGroupCode = cg.Code;
                    oprDtl.ContractGroupName = cg.ContractName;
                    oprDtl.ContractGroupType = cg.ContractGroupType;
                }
                #endregion

                #region 预算信息
                oprDtl.WorkAmountUnitGUID = null;
                oprDtl.WorkAmountUnitName = txtBudgetProjectUnit.Text.Trim();

                oprDtl.PriceUnitGUID = null;
                oprDtl.PriceUnitName = txtBudgetPriceUnit.Text.Trim();

                try
                {
                    decimal ContractProjectQuantity = 0;
                    if (txtBudgetContractProjectAmount.Text.Trim() != "")
                        ContractProjectQuantity = ClientUtil.ToDecimal(txtBudgetContractProjectAmount.Text);

                    oprDtl.ContractProjectQuantity = ContractProjectQuantity;
                }
                catch
                {
                    tabBaseInfo.SelectedIndex = 1;
                    MessageBox.Show("合同工程量格式填写不正确！");
                    txtBudgetContractProjectAmount.Focus();
                    return false;
                }

                try
                {
                    decimal ContractPrice = 0;
                    if (txtBudgetContractPrice.Text.Trim() != "")
                        ContractPrice = ClientUtil.ToDecimal(txtBudgetContractPrice.Text);

                    oprDtl.ContractPrice = ContractPrice;
                }
                catch
                {
                    tabBaseInfo.SelectedIndex = 1;
                    MessageBox.Show("合同单价格式填写不正确！");
                    txtBudgetContractPrice.Focus();
                    return false;
                }

                try
                {
                    decimal ContractTotalPrice = 0;
                    if (txtBudgetContractTotalPrice.Text.Trim() != "")
                        ContractTotalPrice = ClientUtil.ToDecimal(txtBudgetContractTotalPrice.Text);

                    oprDtl.ContractTotalPrice = ContractTotalPrice;
                }
                catch
                {
                    tabBaseInfo.SelectedIndex = 1;
                    MessageBox.Show("合同合价格式填写不正确！");
                    txtBudgetContractTotalPrice.Focus();
                    return false;
                }

                try
                {
                    decimal ResponsibilitilyWorkAmount = 0;
                    if (txtBudgetResponsibilityProjectAmount.Text.Trim() != "")
                        ResponsibilitilyWorkAmount = ClientUtil.ToDecimal(txtBudgetResponsibilityProjectAmount.Text);

                    //赋值
                    oprDtl.ResponsibilitilyWorkAmount = ResponsibilitilyWorkAmount;
                }
                catch
                {
                    tabBaseInfo.SelectedIndex = 1;
                    MessageBox.Show("责任工程量格式填写不正确！");
                    txtBudgetResponsibilityProjectAmount.Focus();
                    return false;
                }

                try
                {
                    decimal ResponsibilitilyPrice = 0;
                    if (txtBudgetResponsibilityPrice.Text.Trim() != "")
                        ResponsibilitilyPrice = ClientUtil.ToDecimal(txtBudgetResponsibilityPrice.Text);

                    //赋值
                    oprDtl.ResponsibilitilyPrice = ResponsibilitilyPrice;
                }
                catch
                {
                    tabBaseInfo.SelectedIndex = 1;
                    MessageBox.Show("责任单价填写不正确！");
                    txtBudgetResponsibilityPrice.Focus();
                    return false;
                }

                try
                {
                    decimal ResponsibilitilyTotalPrice = 0;
                    if (txtBudgetResponsibilityTotalPrice.Text.Trim() != "")
                        ResponsibilitilyTotalPrice = ClientUtil.ToDecimal(txtBudgetResponsibilityTotalPrice.Text);

                    //赋值
                    oprDtl.ResponsibilitilyTotalPrice = ResponsibilitilyTotalPrice;
                }
                catch
                {
                    tabBaseInfo.SelectedIndex = 1;
                    MessageBox.Show("责任合价格式填写不正确！");
                    txtBudgetResponsibilityTotalPrice.Focus();
                    return false;
                }

                try
                {
                    decimal PlanWorkAmount = 0;
                    if (txtBudgetPlanProjectAmount.Text.Trim() != "")
                        PlanWorkAmount = ClientUtil.ToDecimal(txtBudgetPlanProjectAmount.Text);

                    //赋值
                    oprDtl.PlanWorkAmount = PlanWorkAmount;
                }
                catch
                {
                    tabBaseInfo.SelectedIndex = 1;
                    MessageBox.Show("计划工程量格式填写不正确！");
                    txtBudgetPlanProjectAmount.Focus();
                    return false;
                }

                try
                {
                    decimal PlanPrice = 0;
                    if (txtBudgetPlanPrice.Text.Trim() != "")
                        PlanPrice = ClientUtil.ToDecimal(txtBudgetPlanPrice.Text);

                    //赋值
                    oprDtl.PlanPrice = PlanPrice;
                }
                catch
                {
                    MessageBox.Show("计划单价格式填写不正确！");
                    tabBaseInfo.SelectedIndex = 1;
                    txtBudgetPlanPrice.Focus();
                    return false;
                }

                try
                {
                    decimal PlanTotalPrice = 0;
                    if (txtBudgetPlanTotalPrice.Text.Trim() != "")
                        PlanTotalPrice = ClientUtil.ToDecimal(txtBudgetPlanTotalPrice.Text);

                    //赋值
                    oprDtl.PlanTotalPrice = PlanTotalPrice;
                }
                catch
                {
                    MessageBox.Show("计划合价格式填写不正确！");
                    tabBaseInfo.SelectedIndex = 1;
                    txtBudgetPlanTotalPrice.Focus();
                    return false;
                }

                #endregion

                oprDtl.ResponseFlag = cbResponseAccount.Checked ? 1 : 0;
                oprDtl.CostingFlag = cbCostAccount.Checked ? 1 : 0;
                oprDtl.ProduceConfirmFlag = oprDtl.CostingFlag;
                //oprDtl.ProduceConfirmFlag = cbProductConfirm.Checked ? 1 : 0;
                oprDtl.SubContractFeeFlag = cbSubContractFee.Checked;

                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
                return false;
            }
        }
        //private decimal DecimalRound(Decimal val)
        //{
        //    return decimal.Round(val, 5);
        //}
        //刷新任务明细相关控件的状态
        public void RefreshDetailControls(MainViewState state)
        {
            switch (state)
            {

                case MainViewState.Modify:

                    btnSaveBaseInfo.Enabled = true;
                    btnSaveTaskDetail.Enabled = true;
                    //不需要修改
                    txtDtlCode.ReadOnly = true;
                    txtDtlState.ReadOnly = true;
                    txtDtlStateTime.ReadOnly = true;

                    txtDtlContractGroupCode.ReadOnly = true;
                    txtDtlContractGroupType.ReadOnly = true;

                    txtDtlFinishedProjectAmount.ReadOnly = true;
                    txtDtlFinishedPercent.ReadOnly = true;

                    //基本信息
                    txtDtlName.ReadOnly = false;

                    txtDtlCostItem.ReadOnly = false;
                    btnSelectCostItem.Enabled = true;
                    cbDtlOrg.Enabled = true;

                    txtDtlUsedPart.ReadOnly = false;
                    txtDtlMaterial.ReadOnly = false;
                    txtDtlDesc.ReadOnly = false;
                    txtDtlMethod.ReadOnly = false;
                    cbDtlManagementMethod.Enabled = true;

                    txtDtlDesc.ReadOnly = false;

                    txtBudgetProjectUnit.ReadOnly = false;
                    txtBudgetPriceUnit.ReadOnly = false;


                    cbResponseAccount.Enabled = true;
                    cbCostAccount.Enabled = true;
                    cbProductConfirm.Enabled = true;
                    cbSubContractFee.Enabled = true;

                    //预算信息
                    txtBudgetProjectUnit.ReadOnly = false;
                    txtBudgetPriceUnit.ReadOnly = false;

                    txtBudgetContractProjectAmount.ReadOnly = false;
                    txtBudgetContractPrice.ReadOnly = false;
                    txtBudgetContractTotalPrice.ReadOnly = false;

                    txtBudgetResponsibilityProjectAmount.ReadOnly = false;
                    txtBudgetResponsibilityPrice.ReadOnly = false;
                    txtBudgetResponsibilityTotalPrice.ReadOnly = false;

                    txtBudgetPlanProjectAmount.ReadOnly = false;
                    txtBudgetPlanPrice.ReadOnly = false;
                    txtBudgetPlanTotalPrice.ReadOnly = false;


                    //核算信息

                    btnAddCostSubject.Enabled = true;
                    btnUpdateCostSubject.Enabled = true;
                    btnDeleteCostSubject.Enabled = true;

                    break;

                case MainViewState.Browser:

                    btnSaveBaseInfo.Enabled = false;
                    btnSaveTaskDetail.Enabled = false;

                    //基本信息
                    txtDtlName.ReadOnly = true;
                    txtDtlCode.ReadOnly = true;
                    txtDtlCostItem.ReadOnly = true;
                    btnSelectCostItem.Enabled = false;
                    cbDtlOrg.Enabled = false;

                    txtDtlUsedPart.ReadOnly = true;
                    txtDtlMaterial.ReadOnly = true;
                    txtDtlDesc.ReadOnly = true;
                    txtDtlMethod.ReadOnly = true;
                    cbDtlManagementMethod.Enabled = false;

                    txtDtlState.ReadOnly = true;
                    txtDtlStateTime.ReadOnly = true;

                    txtDtlContractGroupCode.ReadOnly = true;
                    txtDtlContractGroupType.ReadOnly = true;

                    txtDtlFinishedProjectAmount.ReadOnly = true;
                    txtDtlFinishedPercent.ReadOnly = true;

                    txtDtlDesc.ReadOnly = true;

                    txtBudgetProjectUnit.ReadOnly = true;
                    txtBudgetPriceUnit.ReadOnly = true;


                    cbResponseAccount.Enabled = false;
                    cbCostAccount.Enabled = false;
                    cbProductConfirm.Enabled = false;
                    cbSubContractFee.Enabled = false;


                    //预算信息
                    txtBudgetProjectUnit.ReadOnly = true;
                    txtBudgetPriceUnit.ReadOnly = true;

                    txtBudgetContractProjectAmount.ReadOnly = true;
                    txtBudgetContractPrice.ReadOnly = true;
                    txtBudgetContractTotalPrice.ReadOnly = true;

                    txtBudgetResponsibilityProjectAmount.ReadOnly = true;
                    txtBudgetResponsibilityPrice.ReadOnly = true;
                    txtBudgetResponsibilityTotalPrice.ReadOnly = true;

                    txtBudgetPlanProjectAmount.ReadOnly = true;
                    txtBudgetPlanPrice.ReadOnly = true;
                    txtBudgetPlanTotalPrice.ReadOnly = true;


                    //核算信息

                    if (oprDtl != null && oprDtl.State == VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Invalid)
                    {
                        btnAddCostSubject.Enabled = false;
                        btnUpdateCostSubject.Enabled = false;
                        btnDeleteCostSubject.Enabled = false;
                    }
                    else
                    {
                        btnAddCostSubject.Enabled = true;
                        btnUpdateCostSubject.Enabled = true;
                        btnDeleteCostSubject.Enabled = true;

                    }
                    break;
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

            }
            catch (Exception e)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(e));
            }
        }

        public override bool SaveView()
        {
            try
            {
                if (!ValideSave())
                    return false;

                List<GWBSDetailLedger> listLedger = new List<GWBSDetailLedger>();



                if (oprDtl.Id == null)
                {

                }
                else
                {
                    GWBSTree oprGWBS = model.GetObjectById(typeof(GWBSTree), oprDtl.TheGWBS.Id) as GWBSTree;

                    oprGWBS.ResponsibleAccFlag = oprDtl.ResponseFlag == 1 ? true : false;
                    oprGWBS.CostAccFlag = oprDtl.CostingFlag == 1 ? true : false;
                    oprGWBS.ProductConfirmFlag = oprDtl.ProduceConfirmFlag == 1 ? true : false;
                    oprGWBS.SubContractFeeFlag = oprDtl.SubContractFeeFlag;

                    #region 记录明细台账
                    //GWBSDetailLedger led = new GWBSDetailLedger();
                    ////led.CreateTime = model.GetServerTime();

                    //led.ProjectTaskID = oprGWBS.Id;
                    //led.ProjectTaskName = oprGWBS.Name;
                    //led.ProjectTaskDtlID = oprDtl.Id;
                    //led.ProjectTaskDtlName = oprDtl.Name;

                    //led.TheProjectTaskSysCode = oprGWBS.SysCode;
                    //led.ContractPrice = oprDtl.ContractPrice;
                    //led.ContractWorkAmount = oprDtl.ContractProjectQuantity;
                    //led.ContractTotalPrice = oprDtl.ContractTotalPrice;

                    //led.ResponsiblePrice = oprDtl.ResponsibilitilyPrice;
                    //led.ResponsibleWorkAmount = oprDtl.ResponsibilitilyWorkAmount;
                    //led.ResponsibleTotalPrice = oprDtl.ResponsibilitilyTotalPrice;

                    //led.PlanPrice = oprDtl.PlanPrice;
                    //led.PlanWorkAmount = oprDtl.PlanWorkAmount;
                    //led.PlanTotalPrice = oprDtl.PlanTotalPrice;

                    //led.WorkAmountUnit = oprDtl.WorkAmountUnitGUID;
                    //led.WorkAmountUnitName = oprDtl.WorkAmountUnitName;

                    //led.PriceUnit = oprDtl.PriceUnitGUID;
                    //led.PriceUnitName = oprDtl.PriceUnitName;

                    //led.TheContractGroup = txtContractGroupCode.Tag as ContractGroup;

                    //led.TheProjectGUID = oprDtl.TheProjectGUID;
                    //led.TheProjectName = oprDtl.TheProjectName;

                    //listLedger.Add(led);

                    #endregion

                    IList listResult = model.SaveOrUpdateDetail(oprDtl, oprGWBS, listLedger);
                    oprDtl = listResult[0] as GWBSDetail;
                    oprGWBS = listResult[1] as GWBSTree;

                    UpdateDetailInfoInGrid(oprDtl);
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

        private bool valideContractGroup()
        {
            if (txtContractGroupCode.Text.Trim() == "" || txtContractGroupCode.Tag == null)
            {
                MessageBox.Show("请选择驱动契约组！");

                VSelectWBSContractGroup frm = new VSelectWBSContractGroup(new MWBSContractGroup(), false);
                if (txtContractGroupCode.Tag != null)
                {
                    frm.DefaultSelectedContract = txtContractGroupCode.Tag as ContractGroup;
                }
                frm.ShowDialog();
                if (frm.SelectResult.Count > 0)
                {
                    ContractGroup cg = frm.SelectResult[0] as ContractGroup;
                    txtContractGroupCode.Text = cg.Code;
                    txtContractGroupType.Text = cg.ContractGroupType;
                    txtContractGroupDesc.Text = cg.ContractDesc;
                    txtContractGroupCode.Tag = cg;

                    return true;
                }
                return false;
            }
            return true;
        }

        #region 任务明细操作
        private void ClearDetailAll()
        {
            //基本信息
            txtDtlName.Text = "";
            txtDtlCode.Text = "";

            txtDtlCostItem.Text = "";
            txtDtlCostItem.Tag = null;

            try
            {
                cbDtlOrg.Text = "";
            }
            catch { }
            cbDtlOrg.Tag = null;
            if (cbDtlOrg.Result != null)
                cbDtlOrg.Result.Clear();

            txtDtlUsedPart.Text = "";
            txtDtlMaterial.Text = "";
            txtDtlDesc.Text = "";
            txtDtlMethod.Text = "";
            cbDtlManagementMethod.SelectedIndex = 0;

            txtDtlState.Text = "";
            txtDtlStateTime.Text = "";

            txtDtlContractGroupCode.Text = "";
            txtDtlContractGroupCode.Tag = null;
            txtDtlContractGroupType.Text = "";

            txtDtlFinishedProjectAmount.Text = "";
            txtDtlFinishedPercent.Text = "";

            txtDtlDesc.Text = "";

            txtBudgetProjectUnit.Text = "";
            txtBudgetPriceUnit.Text = "";

            //预算信息
            txtBudgetProjectUnit.Text = "";
            txtBudgetPriceUnit.Text = "";

            txtBudgetContractProjectAmount.Text = "";
            txtBudgetContractPrice.Text = "";
            txtBudgetContractTotalPrice.Text = "";

            txtBudgetResponsibilityProjectAmount.Text = "";
            txtBudgetResponsibilityPrice.Text = "";
            txtBudgetResponsibilityTotalPrice.Text = "";

            txtBudgetPlanProjectAmount.Text = "";
            txtBudgetPlanPrice.Text = "";
            txtBudgetPlanTotalPrice.Text = "";



            gridCostSubject.Rows.Clear();
        }

        void gridGWBDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                GWBSDetail tempItem = gridGWBDetail.Rows[e.RowIndex].Tag as GWBSDetail;
                if (oprDtl != null && tempItem.Id == oprDtl.Id)
                    return;

                if (btnSaveBaseInfo.Enabled && oprDtl != null && tempItem.Id != oprDtl.Id)
                {
                    if (MessageBox.Show("有尚未保存的任务明细信息，要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (!SaveView())
                        {
                            //修改时还原选中的修改行
                            foreach (DataGridViewRow row in gridGWBDetail.Rows)
                            {
                                GWBSDetail temp = row.Tag as GWBSDetail;
                                if (temp.Id == oprDtl.Id)
                                {
                                    gridGWBDetail.CurrentCell = row.Cells[0];
                                    break;
                                }
                            }
                            return;
                        }
                    }
                }


                oprDtl = tempItem;

                if (oprDtl != null)
                {
                    GetTaskNodeDetailInfo();
                }
                gridGWBDetail.CurrentCell = gridGWBDetail.Rows[e.RowIndex].Cells[0];

                RefreshDetailControls(MainViewState.Browser);
            }
        }

        private void AddDetailInfoInGrid(GWBSDetail dtl)
        {
            int index = gridGWBDetail.Rows.Add();
            DataGridViewRow row = gridGWBDetail.Rows[index];
            row.Cells[DtlProjectTaskName.Name].Value = dtl.TheGWBS.Name;
            row.Cells[DtlName.Name].Value = dtl.Name;
            row.Cells[DtlPart.Name].Value = dtl.WorkPart;
            row.Cells[DtlMaterial.Name].Value = dtl.WorkUseMaterial;
            row.Cells[DtlMethod.Name].Value = dtl.WorkMethod;
            if (dtl.TheCostItem != null)
                row.Cells[DtlCostItem.Name].Value = dtl.TheCostItem.Name;
            row.Cells[DtlResponsibleAccount.Name].Value = dtl.ResponseFlag == 1 ? "是" : "否";
            row.Cells[DtlCostAccount.Name].Value = dtl.CostingFlag == 1 ? "是" : "否";
            row.Cells[DtlProductConfirm.Name].Value = dtl.ProduceConfirmFlag == 1 ? "是" : "否";
            row.Cells[DtlSubContractFee.Name].Value = dtl.SubContractFeeFlag == true ? "是" : "否";

            row.Cells[DtlState.Name].Value =  StaticMethod.GetWBSTaskStateText(dtl.State);
            row.Cells[DtlContractGroupCode.Name].Value = dtl.ContractGroupCode;
            row.Cells[DtlDesc.Name].Value = dtl.ContentDesc;

            row.Tag = dtl;

            gridGWBDetail.CurrentCell = row.Cells[0];
            try
            {
                if (dtl.ListCostSubjectDetails != null && dtl.ListCostSubjectDetails.Count > 0)
                {
                    gridCostSubject.Rows.Clear();
                    foreach (GWBSDetailCostSubject subject in dtl.ListCostSubjectDetails)
                    {
                        AddCostSubjectInfoInGrid(subject);
                    }
                }
            }
            catch { }
        }
        private void UpdateDetailInfoInGrid(GWBSDetail dtl)
        {
            for (int i = 0; i < gridGWBDetail.Rows.Count; i++)
            {
                DataGridViewRow row = gridGWBDetail.Rows[i];
                GWBSDetail d = row.Tag as GWBSDetail;
                if (d.Id == dtl.Id)
                {
                    row.Cells["DtlName"].Value = dtl.Name;
                    if (dtl.TheCostItem != null)
                        row.Cells["DtlCostItem"].Value = dtl.TheCostItem.Name;
                    row.Cells["DtlDesc"].Value = dtl.ContentDesc;
                    row.Cells["DtlContractGroupCode"].Value = dtl.ContractGroupCode;
                    row.Cells["DtlPart"].Value = dtl.WorkPart;
                    row.Cells["DtlMaterial"].Value = dtl.WorkUseMaterial;
                    row.Cells["DtlMethod"].Value = dtl.WorkMethod;
                    row.Cells["DtlState"].Value = StaticMethod.GetWBSTaskStateText(dtl.State);
                    row.Tag = dtl;

                    gridGWBDetail.CurrentCell = row.Cells[0];
                    break;
                }
            }
        }
        //选择成本项
        void btnSelectCostItem_Click(object sender, EventArgs e)
        {
            VSelectCostItem frm = new VSelectCostItem(new MCostItem());
            if (txtDtlCostItem.Tag != null)
            {
                frm.DefaultSelectedCostItem = txtDtlCostItem.Tag as CostItem;
            }
            frm.ShowDialog();
            if (frm.SelectCostItem != null)
            {
                if (txtDtlCostItem.Tag != null && txtDtlCostItem.Tag.GetType() == typeof(CostItem))
                {
                    //清掉上次选择的成本项生成的任务明细成本科目
                    for (int i = gridCostSubject.Rows.Count - 1; i > -1; i--)
                    {
                        DataGridViewRow row = gridCostSubject.Rows[i];

                        GWBSDetailCostSubject subject = row.Tag as GWBSDetailCostSubject;
                        if (string.IsNullOrEmpty(subject.Id) && (string.IsNullOrEmpty(subject.TheProjectGUID) || string.IsNullOrEmpty(subject.TheProjectName)))
                        {
                            gridCostSubject.Rows.RemoveAt(i);
                        }
                    }
                }

                txtDtlCostItem.Text = frm.SelectCostItem.Code;
                txtDtlCostItem.Tag = frm.SelectCostItem;

                CostItem item = frm.SelectCostItem;

                ObjectQuery oqQuota = new ObjectQuery();
                Disjunction disQuota = new Disjunction();
                foreach (SubjectCostQuota quota in item.ListQuotas)
                {
                    disQuota.Add(Expression.Eq("Id", quota.Id));
                }
                oqQuota.AddCriterion(disQuota);
                oqQuota.AddCriterion(Expression.Eq("State", SubjectCostQuotaState.生效));
                oqQuota.AddFetchMode("CostAccountSubjectGUID", NHibernate.FetchMode.Eager);
                oqQuota.AddFetchMode("ProjectAmountUnitGUID", NHibernate.FetchMode.Eager);
                oqQuota.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);

                IList listQuota = model.ObjectQuery(typeof(SubjectCostQuota), oqQuota);

                foreach (SubjectCostQuota quota in listQuota)
                {
                    GWBSDetailCostSubject subject = new GWBSDetailCostSubject();
                    subject.Name = quota.Name;
                    subject.State = GWBSDetailCostSubjectState.编制;

                    subject.AssessmentRate = quota.AssessmentRate;

                    subject.CostAccountSubjectGUID = quota.CostAccountSubjectGUID;
                    subject.CostAccountSubjectName = quota.CostAccountSubjectName;

                    subject.PriceUnitGUID = quota.PriceUnitGUID;
                    subject.PriceUnitName = quota.PriceUnitName;

                    subject.ProjectAmountUnitGUID = quota.ProjectAmountUnitGUID;
                    subject.ProjectAmountUnitName = quota.ProjectAmountUnitName;

                    subject.ProjectAmountWasta = quota.Wastage;

                    subject.ResourceTypeGUID = quota.ResourceTypeGUID;
                    subject.ResourceTypeName = quota.ResourceTypeName;

                    AddCostSubjectInfoInGrid(subject);
                }
            }
        }

        //修改明细
        void btnUpdateDetail_Click(object sender, EventArgs e)
        {
            if (gridGWBDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要修改的行！");
                return;
            }
            if (gridGWBDetail.SelectedRows.Count > 1)
            {
                MessageBox.Show("一次只能修改一行！");
                return;
            }

            oprDtl = gridGWBDetail.SelectedRows[0].Tag as GWBSDetail;
            if (oprDtl.State !=  VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit)
            {
                MessageBox.Show("请选择状态为‘编制’的明细进行修改！");
                return;
            }

            if (!valideContractGroup())
            {
                return;
            }

            GetTaskNodeDetailInfo();

            RefreshDetailControls(MainViewState.Modify);
        }
        //发布明细
        void btnPublishDetail_Click(object sender, EventArgs e)
        {
            if (gridGWBDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要发布的明细行！");
                return;
            }
            IList list = new List<GWBSDetail>();
            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridGWBDetail.SelectedRows)
            {
                GWBSDetail cg = row.Tag as GWBSDetail;
                if (cg.State ==  VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit)
                {
                    GWBSDetail temp = model.GetObjectById(typeof(GWBSDetail), cg.Id) as GWBSDetail;
                    temp.State =  VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute;
                    list.Add(temp);

                    listRowIndex.Add(row.Index);
                }
            }
            if (list.Count == 0)
            {
                MessageBox.Show("选择中没有符合发布的明细，请选择状态为‘制定’的明细！");
                return;
            }

            try
            {
                list = model.SaveOrUpdateDetail(list);

                for (int i = 0; i < listRowIndex.Count; i++)
                {
                    int rowIndex = listRowIndex[i];
                    gridGWBDetail.Rows[rowIndex].Tag = list[i];
                    gridGWBDetail.Rows[rowIndex].Cells["DtlState"].Value =StaticMethod.GetWBSTaskStateText( (list[i] as GWBSDetail).State);
                }

                MessageBox.Show("发布成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("发布失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        //删除明细 
        void btnDeleteDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridGWBDetail.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请选择要删除的行！");
                    gridGWBDetail.Focus();
                    return;
                }

                IList list = new List<GWBSDetail>();
                List<int> listRowIndex = new List<int>();
                foreach (DataGridViewRow row in gridGWBDetail.SelectedRows)
                {
                    GWBSDetail item = row.Tag as GWBSDetail;
                    if (item.State == VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit)
                    {
                        list.Add(item);
                        listRowIndex.Add(row.Index);
                    }
                }

                if (list.Count == 0)
                {
                    MessageBox.Show("选择行中没有符合删除的记录，请选择状态为‘制定’的明细行！");
                    return;
                }

                if (MessageBox.Show("删除后不能恢复，您确认要删除选中分科目成本信息吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                model.DeleteDetail(list);

                listRowIndex.Sort();
                for (int i = listRowIndex.Count - 1; i > -1; i--)
                {
                    gridGWBDetail.Rows.RemoveAt(listRowIndex[i]);
                }
                gridGWBDetail.ClearSelection();

                ClearDetailAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        //作废明细
        void btnCancellationDetail_Click(object sender, EventArgs e)
        {
            if (gridGWBDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要作废的明细行！");
                return;
            }
            IList list = new List<GWBSDetail>();
            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridGWBDetail.SelectedRows)
            {
                GWBSDetail cg = row.Tag as GWBSDetail;
                if (cg.State == VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute)
                {
                    GWBSDetail temp = model.GetObjectById(typeof(GWBSDetail), cg.Id) as GWBSDetail;
                    temp.State =  VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Invalid;
                    list.Add(temp);

                    listRowIndex.Add(row.Index);
                }
            }
            if (list.Count == 0)
            {
                MessageBox.Show("选择中没有符合作废的明细，请选择状态为‘有效’的明细！");
                return;
            }

            try
            {
                list = model.SaveOrUpdateDetail(list);

                for (int i = 0; i < listRowIndex.Count; i++)
                {
                    int rowIndex = listRowIndex[i];
                    GWBSDetail temp = list[i] as GWBSDetail;
                    if (temp.Id == oprDtl.Id)
                        oprDtl = temp;
                    gridGWBDetail.Rows[rowIndex].Tag = temp;
                    gridGWBDetail.Rows[rowIndex].Cells["DtlState"].Value =StaticMethod.GetWBSTaskStateText( temp.State);
                }

                RefreshControls(MainViewState.Browser);

                MessageBox.Show("设置成功！");

            }
            catch (Exception ex)
            {
                MessageBox.Show("设置失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        //保存明细
        void btnSaveTaskDetail_Click(object sender, EventArgs e)
        {
            if (!SaveView())
                return;
            RefreshDetailControls(MainViewState.Browser);
        }
        //保存明细
        void btnSave_Click(object sender, EventArgs e)
        {
            if (!SaveView())
                return;

            GetTaskNodeDetailInfo();//刷新契约组信息

            RefreshDetailControls(MainViewState.Browser);
        }

        //选择契约组
        void btnSelectContractGroup_Click(object sender, EventArgs e)
        {
            VSelectWBSContractGroup frm = new VSelectWBSContractGroup(new MWBSContractGroup(), false);
            if (txtContractGroupCode.Tag != null)
            {
                frm.DefaultSelectedContract = txtContractGroupCode.Tag as ContractGroup;
            }
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                ContractGroup cg = frm.SelectResult[0] as ContractGroup;
                txtContractGroupCode.Text = cg.Code;
                txtContractGroupType.Text = cg.ContractGroupType;
                txtContractGroupDesc.Text = cg.ContractDesc;
                txtContractGroupCode.Tag = cg;
            }
        }

        void txtDtlCostItem_LostFocus(object sender, EventArgs e)
        {
            if (txtDtlCostItem.ReadOnly || !txtDtlCostItem.Enabled)
                return;

            string costItemCode = txtDtlCostItem.Text.Trim();
            if (string.IsNullOrEmpty(costItemCode))
            {
                //MessageBox.Show("请输入成本项编号或定额编号，或选择一个成本项！");
                return;
            }
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Or(Expression.Eq("Code", costItemCode), Expression.Eq("QuotaCode", costItemCode)));
            IList list = model.ObjectQuery(typeof(CostItem), oq);
            if (list != null && list.Count > 0)
            {
                CostItem item = list[0] as CostItem;
                oq.Criterions.Clear();
                oq.AddCriterion(Expression.Eq("Id", item.Id));
                oq.AddFetchMode("ListQuotas", NHibernate.FetchMode.Eager);
                item = model.ObjectQuery(typeof(CostItem), oq)[0] as CostItem;

                txtDtlCostItem.Tag = item;

                //清掉上次选择的成本项生成的任务明细成本科目
                for (int i = gridCostSubject.Rows.Count - 1; i > -1; i--)
                {
                    DataGridViewRow row = gridCostSubject.Rows[i];

                    GWBSDetailCostSubject subject = row.Tag as GWBSDetailCostSubject;
                    if (string.IsNullOrEmpty(subject.Id) && (string.IsNullOrEmpty(subject.TheProjectGUID) || string.IsNullOrEmpty(subject.TheProjectName)))
                    {
                        gridCostSubject.Rows.RemoveAt(i);
                    }
                }

                ObjectQuery oqQuota = new ObjectQuery();
                Disjunction disQuota = new Disjunction();
                foreach (SubjectCostQuota quota in item.ListQuotas)
                {
                    disQuota.Add(Expression.Eq("Id", quota.Id));
                }
                oqQuota.AddCriterion(disQuota);
                oqQuota.AddCriterion(Expression.Eq("State", SubjectCostQuotaState.生效));
                oqQuota.AddFetchMode("CostAccountSubjectGUID", NHibernate.FetchMode.Eager);
                oqQuota.AddFetchMode("ProjectAmountUnitGUID", NHibernate.FetchMode.Eager);
                oqQuota.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);

                IList listQuota = model.ObjectQuery(typeof(SubjectCostQuota), oqQuota);

                foreach (SubjectCostQuota quota in listQuota)
                {
                    GWBSDetailCostSubject subject = new GWBSDetailCostSubject();
                    subject.Name = quota.Name;
                    subject.State = GWBSDetailCostSubjectState.编制;

                    subject.AssessmentRate = quota.AssessmentRate;

                    subject.CostAccountSubjectGUID = quota.CostAccountSubjectGUID;
                    subject.CostAccountSubjectName = quota.CostAccountSubjectName;

                    subject.PriceUnitGUID = quota.PriceUnitGUID;
                    subject.PriceUnitName = quota.PriceUnitName;

                    subject.ProjectAmountUnitGUID = quota.ProjectAmountUnitGUID;
                    subject.ProjectAmountUnitName = quota.ProjectAmountUnitName;

                    subject.ProjectAmountWasta = quota.Wastage;

                    subject.ResourceTypeGUID = quota.ResourceTypeGUID;
                    subject.ResourceTypeName = quota.ResourceTypeName;

                    AddCostSubjectInfoInGrid(subject);
                }
            }
            else
            {
                txtDtlCostItem.Tag = null;
                MessageBox.Show("请检查输入的成本项编号或定额编号，未找到相关成本项！");
            }
        }


        void txtBudgetResponsibilityPrice_TextChanged(object sender, EventArgs e)
        {
            if (txtBudgetResponsibilityPrice.Text.Trim() != "" && txtBudgetResponsibilityProjectAmount.Text.Trim() != "")
            {
                try
                {
                    txtBudgetResponsibilityTotalPrice.Text =
                        Decimal.Round(ClientUtil.ToDecimal(txtBudgetResponsibilityPrice.Text) * ClientUtil.ToDecimal(txtBudgetResponsibilityProjectAmount.Text), 3).ToString();
                }
                catch { }
            }
        }
        void txtBudgetResponsibilityProjectAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtBudgetResponsibilityPrice.Text.Trim() != "" && txtBudgetResponsibilityProjectAmount.Text.Trim() != "")
            {
                try
                {
                    txtBudgetResponsibilityTotalPrice.Text =
                        Decimal.Round(ClientUtil.ToDecimal(txtBudgetResponsibilityPrice.Text) * ClientUtil.ToDecimal(txtBudgetResponsibilityProjectAmount.Text), 3).ToString();
                }
                catch { }
            }
        }

        void txtBudgetPlanPrice_TextChanged(object sender, EventArgs e)
        {
            if (txtBudgetPlanPrice.Text.Trim() != "" && txtBudgetPlanProjectAmount.Text.Trim() != "")
            {
                try
                {
                    txtBudgetPlanTotalPrice.Text =
                        Decimal.Round(ClientUtil.ToDecimal(txtBudgetPlanPrice.Text) * ClientUtil.ToDecimal(txtBudgetPlanProjectAmount.Text), 3).ToString();
                }
                catch { }
            }
        }
        void txtBudgetPlanProjectAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtBudgetPlanPrice.Text.Trim() != "" && txtBudgetPlanProjectAmount.Text.Trim() != "")
            {
                try
                {
                    txtBudgetPlanTotalPrice.Text =
                        Decimal.Round(ClientUtil.ToDecimal(txtBudgetPlanPrice.Text) * ClientUtil.ToDecimal(txtBudgetPlanProjectAmount.Text), 3).ToString();
                }
                catch { }
            }
        }

        void txtBudgetContractPrice_TextChanged(object sender, EventArgs e)
        {
            if (txtBudgetContractPrice.Text.Trim() != "" && txtBudgetContractProjectAmount.Text.Trim() != "")
            {
                try
                {
                    txtBudgetContractTotalPrice.Text =
                        Decimal.Round(ClientUtil.ToDecimal(txtBudgetContractPrice.Text) * ClientUtil.ToDecimal(txtBudgetContractProjectAmount.Text), 3).ToString();
                }
                catch { }
            }
        }
        void txtBudgetContractProjectAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtBudgetContractPrice.Text.Trim() != "" && txtBudgetContractProjectAmount.Text.Trim() != "")
            {
                try
                {
                    txtBudgetContractTotalPrice.Text =
                        Decimal.Round(ClientUtil.ToDecimal(txtBudgetContractPrice.Text) * ClientUtil.ToDecimal(txtBudgetContractProjectAmount.Text), 3).ToString();
                }
                catch { }
            }
        }
        #endregion

        #region 任务明细分科目成本操作
        private void AddCostSubjectInfoInGrid(GWBSDetailCostSubject dtl)
        {
            int index = gridCostSubject.Rows.Add();
            DataGridViewRow row = gridCostSubject.Rows[index];
            row.Cells["SubjectName"].Value = dtl.Name;
            row.Cells["SubjectCateName"].Value = dtl.CostAccountSubjectName;
            row.Cells["SubjectContractPrice"].Value = dtl.ContractPrice.ToString();
            row.Cells["SubjectContractProjectAmount"].Value = dtl.ContractProjectAmount;
            row.Cells["SubjectContractTotalPrice"].Value = dtl.ContractTotalPrice;

            row.Cells["SubjectResourceType"].Value = dtl.ResourceTypeName;
            row.Cells["SubjectResponsibilityPrice"].Value = dtl.ResponsibilitilyPrice;
            row.Cells["SubjectResponsibilityProjectAmount"].Value = dtl.ResponsibilitilyWorkAmount;
            row.Cells["SubjectResponsibilityTotalPrice"].Value = dtl.ResponsibilitilyTotalPrice;
            row.Cells["SubjectPlanPrice"].Value = dtl.PlanPrice;

            row.Cells["SubjectPlanProjectAmount"].Value = dtl.PlanWorkAmount;
            row.Cells["SubjectPlanTotalPrice"].Value = dtl.PlanTotalPrice;
            row.Cells["SubjectAddupAccountProjectAmount"].Value = dtl.AddupAccountProjectAmount;
            row.Cells["SubjectAddupAccountCost"].Value = dtl.AddupAccountCost;
            if (dtl.AddupAccountCostEndTime != null)
                row.Cells["SubjectAddupAccountCostEndTime"].Value = dtl.AddupAccountCostEndTime;

            row.Cells["SubjectCurrPeriodAccountProjectAmount"].Value = dtl.CurrentPeriodAccountProjectAmount;
            row.Cells["SubjectCurrPeriodAccountCost"].Value = dtl.CurrentPeriodAccountCost;
            if (dtl.CurrentPeriodAccountCostEndTime != null)
                row.Cells["SubjectCurrPeriodAccountCostEndTime"].Value = dtl.CurrentPeriodAccountCostEndTime;
            row.Cells["SubjectAddupBalanceProjectAmount"].Value = dtl.AddupBalanceProjectAmount;
            row.Cells["SubjectCurrPeriodBalanceProjectAmount"].Value = dtl.CurrentPeriodBalanceProjectAmount;

            row.Cells["SubjectCurrPeriodBalanceTotalPrice"].Value = dtl.CurrentPeriodBalanceTotalPrice;
            row.Cells["SubjectAddupBalanceTotalPrice"].Value = dtl.AddupBalanceTotalPrice;
            row.Cells["SubjectPriceUnit"].Value = dtl.PriceUnitName;
            row.Cells["SubjectProjectAmountUnit"].Value = dtl.ProjectAmountUnitName;
            row.Cells["SubjectAssessmentRate"].Value = dtl.AssessmentRate;

            row.Cells["SubjectProjectAmountWastage"].Value = dtl.ProjectAmountWasta;

            row.Tag = dtl;

            gridCostSubject.CurrentCell = row.Cells[0];
        }
        private void UpdateCostSubjectInfoInGrid(GWBSDetailCostSubject dtl)
        {
            foreach (DataGridViewRow row in gridCostSubject.SelectedRows)
            {
                GWBSDetailCostSubject d = row.Tag as GWBSDetailCostSubject;
                if (d.Id == dtl.Id)
                {
                    row.Cells["SubjectName"].Value = dtl.Name;
                    row.Cells["SubjectCateName"].Value = dtl.CostAccountSubjectName;
                    row.Cells["SubjectContractPrice"].Value = dtl.ContractPrice.ToString();
                    row.Cells["SubjectContractProjectAmount"].Value = dtl.ContractProjectAmount;
                    row.Cells["SubjectContractTotalPrice"].Value = dtl.ContractTotalPrice;

                    row.Cells["SubjectResourceType"].Value = dtl.ResourceTypeName;
                    row.Cells["SubjectResponsibilityPrice"].Value = dtl.ResponsibilitilyPrice;
                    row.Cells["SubjectResponsibilityProjectAmount"].Value = dtl.ResponsibilitilyWorkAmount;
                    row.Cells["SubjectResponsibilityTotalPrice"].Value = dtl.ResponsibilitilyTotalPrice;
                    row.Cells["SubjectPlanPrice"].Value = dtl.PlanPrice;

                    row.Cells["SubjectPlanProjectAmount"].Value = dtl.PlanWorkAmount;
                    row.Cells["SubjectPlanTotalPrice"].Value = dtl.PlanTotalPrice;
                    row.Cells["SubjectAddupAccountProjectAmount"].Value = dtl.AddupAccountProjectAmount;
                    row.Cells["SubjectAddupAccountCost"].Value = dtl.AddupAccountCost;
                    if (dtl.AddupAccountCostEndTime != null)
                        row.Cells["SubjectAddupAccountCostEndTime"].Value = dtl.AddupAccountCostEndTime;

                    row.Cells["SubjectCurrPeriodAccountProjectAmount"].Value = dtl.CurrentPeriodAccountProjectAmount;
                    row.Cells["SubjectCurrPeriodAccountCost"].Value = dtl.CurrentPeriodAccountCost;
                    if (dtl.CurrentPeriodAccountCostEndTime != null)
                        row.Cells["SubjectCurrPeriodAccountCostEndTime"].Value = dtl.CurrentPeriodAccountCostEndTime;
                    row.Cells["SubjectAddupBalanceProjectAmount"].Value = dtl.AddupBalanceProjectAmount;
                    row.Cells["SubjectCurrPeriodBalanceProjectAmount"].Value = dtl.CurrentPeriodBalanceProjectAmount;

                    row.Cells["SubjectCurrPeriodBalanceTotalPrice"].Value = dtl.CurrentPeriodBalanceTotalPrice;
                    row.Cells["SubjectAddupBalanceTotalPrice"].Value = dtl.AddupBalanceTotalPrice;
                    row.Cells["SubjectPriceUnit"].Value = dtl.PriceUnitName;
                    row.Cells["SubjectProjectAmountUnit"].Value = dtl.ProjectAmountUnitName;
                    row.Cells["SubjectAssessmentRate"].Value = dtl.AssessmentRate;

                    row.Cells["SubjectProjectAmountWastage"].Value = dtl.ProjectAmountWasta;
                    row.Tag = dtl;
                    break;
                }
            }
        }
        void btnAddCostSubject_Click(object sender, EventArgs e)
        {
            if (oprDtl == null)
            {
                MessageBox.Show("请先选择一个工程任务明细！");
                return;
            }
            else if (oprDtl.State !=  VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit)
            {
                MessageBox.Show("请选择状态为‘编制’的任务明细进行操作！");
                return;
            }

            bool addCostItemFlag = false;
            if (string.IsNullOrEmpty(oprDtl.Id))
                addCostItemFlag = true;

            VEditGWBSDetailCostSubject frm = new VEditGWBSDetailCostSubject(new MGWBSTree());
            frm.OptGWBSDetail = oprDtl;
            frm.ShowDialog();

            if (frm.OptCostSubject != null)// && !string.IsNullOrEmpty(frm.OptCostQuota.Id)
            {
                oprDtl = frm.OptGWBSDetail;
                if (!addCostItemFlag)
                    UpdateDetailInfoInGrid(oprDtl);

                AddCostSubjectInfoInGrid(frm.OptCostSubject);
            }
        }
        void btnUpdateCostSubject_Click(object sender, EventArgs e)
        {
            if (gridCostSubject.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要修改的行！");
                return;
            }
            if (gridCostSubject.SelectedRows.Count > 1)
            {
                MessageBox.Show("一次只能修改一行！");
                return;
            }
            GWBSDetailCostSubject subject = gridCostSubject.SelectedRows[0].Tag as GWBSDetailCostSubject;

            VEditGWBSDetailCostSubject frm = new VEditGWBSDetailCostSubject(new MGWBSTree());
            frm.OptGWBSDetail = oprDtl;
            frm.OptCostSubject = subject;
            frm.ShowDialog();

            UpdateCostSubjectInfoInGrid(frm.OptCostSubject);
        }
        void btnDeleteCostSubject_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridCostSubject.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请选择要删除的行！");
                    gridCostSubject.Focus();
                    return;
                }

                IList list = new List<GWBSDetailCostSubject>();
                List<int> listRowIndex = new List<int>();
                foreach (DataGridViewRow row in gridCostSubject.SelectedRows)
                {
                    GWBSDetailCostSubject item = row.Tag as GWBSDetailCostSubject;
                    list.Add(item);
                    listRowIndex.Add(row.Index);
                }

                if (MessageBox.Show("删除后不能恢复，您确认要删除选中分科目成本信息吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                model.DeleteCostSubject(list);

                listRowIndex.Sort();
                for (int i = listRowIndex.Count - 1; i > -1; i--)
                {
                    gridCostSubject.Rows.RemoveAt(listRowIndex[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        #endregion
    }
}
