using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using System.Collections;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VEditGWBSDetailCostSubject : Form
    {
        private GWBSDetailCostSubject _optCostSubject = null;
        /// <summary>
        /// 操作任务明细分科目成本
        /// </summary>
        public GWBSDetailCostSubject OptCostSubject
        {
            get { return _optCostSubject; }
            set { _optCostSubject = value; }
        }

        private GWBSDetail _optGWBSDetail = null;
        /// <summary>
        /// 操作工程WBS明细
        /// </summary>
        public GWBSDetail OptGWBSDetail
        {
            get { return _optGWBSDetail; }
            set { _optGWBSDetail = value; }
        }

        public MGWBSTree model;

        public VEditGWBSDetailCostSubject(MGWBSTree mot)
        {
            model = mot;
            InitializeComponent();
            InitialEvents();
        }

        private void InitialEvents()
        {
            btnSave.Click += new EventHandler(btnSave_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            btnSelectSubject.Click += new EventHandler(btnSelectSubject_Click);

            this.Load += new EventHandler(VEditCostSubjectQuota_Load);
        }

        void VEditCostSubjectQuota_Load(object sender, EventArgs e)
        {
            if (OptCostSubject != null)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", OptCostSubject.Id));
                oq.AddFetchMode("ProjectAmountUnitGUID", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("CostAccountSubjectGUID", NHibernate.FetchMode.Eager);

                OptCostSubject = model.ObjectQuery(typeof(GWBSDetailCostSubject), oq)[0] as GWBSDetailCostSubject;

                txtName.Text = OptCostSubject.Name;

                txtAccountSubject.Tag = OptCostSubject.CostAccountSubjectGUID;
                txtAccountSubject.Text = OptCostSubject.CostAccountSubjectName;

                txtProjectUnit.Text = OptCostSubject.ProjectAmountUnitName;
                txtProjectUnit.Tag = OptCostSubject.ProjectAmountUnitGUID;

                txtPriceUnit.Text = OptCostSubject.PriceUnitName;
                txtPriceUnit.Tag = OptCostSubject.PriceUnitGUID;

                txtContractProjectAmount.Text = OptCostSubject.ContractProjectAmount.ToString();
                txtContractPrice.Text = OptCostSubject.ContractPrice.ToString();
                txtContractTotalPrice.Text = OptCostSubject.ContractTotalPrice.ToString();

                txtResponsibilityProjectAmount.Text = OptCostSubject.ResponsibilitilyWorkAmount.ToString();
                txtResponsibilityPrice.Text = OptCostSubject.ResponsibilitilyPrice.ToString();
                txtResponsibilityTotalPrice.Text = OptCostSubject.ResponsibilitilyTotalPrice.ToString();

                txtPlanProjectAmount.Text = OptCostSubject.PlanWorkAmount.ToString();
                txtPlanPrice.Text = OptCostSubject.PlanPrice.ToString();
                txtPlanTotalPrice.Text = OptCostSubject.PlanTotalPrice.ToString();

                txtAddupAccountProjectAmount.Text = OptCostSubject.AddupAccountProjectAmount.ToString();
                txtAddupAccountCost.Text = OptCostSubject.AddupAccountCost.ToString();

                if (OptCostSubject.AddupAccountCostEndTime != null)
                    txtAddupAccountCostEndTime.Text = OptCostSubject.AddupAccountCostEndTime.Value.ToString();

                txtCurrPeriodAccountProjectAmount.Text = OptCostSubject.CurrentPeriodAccountProjectAmount.ToString();
                txtCurrPeriodAccountCost.Text = OptCostSubject.CurrentPeriodAccountCost.ToString();

                if (OptCostSubject.CurrentPeriodAccountCostEndTime != null)
                    txtCurrPeriodAccountCostEndTime.Text = OptCostSubject.CurrentPeriodAccountCostEndTime.Value.ToString();

                txtAddupBalanceProjectAmount.Text = OptCostSubject.AddupBalanceProjectAmount.ToString();
                txtAddupBalanceTotalPrice.Text = OptCostSubject.AddupBalanceTotalPrice.ToString();
                txtProjectAmountWaste.Text = OptCostSubject.ProjectAmountWasta.ToString();

                txtCurrPeriodBalanceProjectAmount.Text = OptCostSubject.CurrentPeriodBalanceProjectAmount.ToString();
                txtCurrPeroidBalanceTotalPrice.Text = OptCostSubject.CurrentPeriodBalanceTotalPrice.ToString();

            }
            else
            {
                //txtState.Text = SubjectCostQuotaState.编制.ToString();
            }
        }

        //选择核算科目
        void btnSelectSubject_Click(object sender, EventArgs e)
        {
            VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
            frm.ShowDialog();
            if (frm.SelectAccountSubject != null)
            {
                txtAccountSubject.Text = frm.SelectAccountSubject.Name;
                txtAccountSubject.Tag = frm.SelectAccountSubject;
            }
        }
        //保存
        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValideSave())
                    return;
                if (string.IsNullOrEmpty(_optCostSubject.Id))
                {
                    //_optCostSubject.State = GWBSDetailCostSubjectState.编制;

                    if (!string.IsNullOrEmpty(OptGWBSDetail.Id))
                    {
                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("Id", OptGWBSDetail.Id));
                        oq.AddFetchMode("ListCostSubjectDetails", NHibernate.FetchMode.Eager);
                        oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
                        IList list = model.ObjectQuery(typeof(GWBSDetail), oq);
                        OptGWBSDetail = list[0] as GWBSDetail;
                    }
                    _optCostSubject.TheGWBSDetail = OptGWBSDetail;

                    _optCostSubject.TheGWBSTree = OptGWBSDetail.TheGWBS;
                    _optCostSubject.TheGWBSTreeName = OptGWBSDetail.TheGWBS.Name;
                    _optCostSubject.TheGWBSTreeSyscode = OptGWBSDetail.TheGWBS.SysCode;

                    OptGWBSDetail.ListCostSubjectDetails.Add(_optCostSubject);

                    if (!string.IsNullOrEmpty(OptGWBSDetail.Id))//当工程任务明细存在时才保存明细分科目成本，否则在保存任务明细时一起保存
                    {
                        IList listTemp = new ArrayList();
                        listTemp.Add(OptGWBSDetail);
                        listTemp = model.SaveOrUpdateDetail(listTemp);
                        OptGWBSDetail = listTemp[0] as GWBSDetail;

                        _optCostSubject = OptGWBSDetail.ListCostSubjectDetails.ElementAt(OptGWBSDetail.ListCostSubjectDetails.Count - 1);
                    }
                }
                else
                {
                    IList listTemp = new ArrayList();
                    listTemp.Add(_optCostSubject);
                    listTemp = model.SaveOrUpdateCostSubject(listTemp);
                    _optCostSubject = listTemp[0] as GWBSDetailCostSubject;
                }

                MessageBox.Show("保存成功！");
            }
            catch (Exception exp)
            {
                MessageBox.Show("保存失败，异常信息：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }
        private bool ValideSave()
        {
            try
            {
                if (_optCostSubject == null)
                {
                    _optCostSubject = new GWBSDetailCostSubject();
                }
                else if (!string.IsNullOrEmpty(_optCostSubject.Id))
                    _optCostSubject = model.GetObjectById(typeof(GWBSDetailCostSubject), _optCostSubject.Id) as GWBSDetailCostSubject;

                if (string.IsNullOrEmpty(_optCostSubject.TheProjectGUID) || string.IsNullOrEmpty(_optCostSubject.TheProjectName))
                {
                    CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                    if (projectInfo != null)
                    {
                        _optCostSubject.TheProjectGUID = projectInfo.Id;
                        _optCostSubject.TheProjectName = projectInfo.Name;
                    }
                }
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("名称不能为空！");
                    txtName.Focus();
                    return false;
                }
                if (txtAccountSubject.Text.Trim() == "")
                {
                    MessageBox.Show("核算科目不能为空！");
                    txtAccountSubject.Focus();
                    return false;
                }

                if (txtProjectUnit.Text.Trim() == "")
                {
                    MessageBox.Show("工程计量单位不能为空！");
                    txtProjectUnit.Focus();
                    return false;
                }
                if (txtPriceUnit.Text.Trim() == "")
                {
                    MessageBox.Show("价格计量单位不能为空！");
                    txtPriceUnit.Focus();
                    return false;
                }

                _optCostSubject.Name = txtName.Text.Trim();
                if (txtAccountSubject.Tag != null)
                {
                    CostAccountSubject subject = txtAccountSubject.Tag as CostAccountSubject;
                    if (subject != null)
                    {
                        _optCostSubject.CostAccountSubjectGUID = subject;
                        _optCostSubject.CostAccountSubjectName = subject.Name;
                        _optCostSubject.CostAccountSubjectSyscode = subject.SysCode;
                    }
                }
                else
                {
                    MessageBox.Show("请选择核算科目！");
                    txtAccountSubject.Focus();
                    return false;
                }

                _optCostSubject.ProjectAmountUnitGUID = null;
                _optCostSubject.ProjectAmountUnitName = txtProjectUnit.Text.Trim();
                _optCostSubject.PriceUnitGUID = null;
                _optCostSubject.PriceUnitName = txtPriceUnit.Text.Trim();


                try
                {
                    decimal ContractProjectAmount = 0;
                    if (txtContractProjectAmount.Text.Trim() != "")
                        ContractProjectAmount = Convert.ToDecimal(txtContractProjectAmount.Text);

                    _optCostSubject.ContractProjectAmount = ContractProjectAmount;
                }
                catch
                {
                    MessageBox.Show("合同工程量格式填写不正确！");
                    txtContractProjectAmount.Focus();
                    return false;
                }

                try
                {
                    decimal ContractPrice = 0;
                    if (txtContractPrice.Text.Trim() != "")
                        ContractPrice = Convert.ToDecimal(txtContractPrice.Text);

                    _optCostSubject.ContractPrice = ContractPrice;
                }
                catch
                {
                    MessageBox.Show("合同单价格式填写不正确！");
                    txtContractPrice.Focus();
                    return false;
                }

                try
                {
                    decimal ContractTotalPrice = 0;
                    if (txtContractTotalPrice.Text.Trim() != "")
                        ContractTotalPrice = Convert.ToDecimal(txtContractTotalPrice.Text);

                    _optCostSubject.ContractTotalPrice = ContractTotalPrice;
                }
                catch
                {
                    MessageBox.Show("合同合价格式填写不正确！");
                    txtContractTotalPrice.Focus();
                    return false;
                }

                try
                {
                    decimal ResponsibilitilyWorkAmount = 0;
                    if (txtResponsibilityProjectAmount.Text.Trim() != "")
                        ResponsibilitilyWorkAmount = Convert.ToDecimal(txtResponsibilityProjectAmount.Text);

                    _optCostSubject.ResponsibilitilyWorkAmount = ResponsibilitilyWorkAmount;
                }
                catch
                {
                    MessageBox.Show("责任工程量格式填写不正确！");
                    txtResponsibilityProjectAmount.Focus();
                    return false;
                }
                try
                {
                    decimal ResponsibilitilyPrice = 0;
                    if (txtResponsibilityPrice.Text.Trim() != "")
                        ResponsibilitilyPrice = Convert.ToDecimal(txtResponsibilityPrice.Text);

                    _optCostSubject.ResponsibilitilyPrice = ResponsibilitilyPrice;
                }
                catch
                {
                    MessageBox.Show("责任单价格式填写不正确！");
                    txtResponsibilityPrice.Focus();
                    return false;
                }
                try
                {
                    decimal ResponsibilitilyTotalPrice = 0;
                    if (txtResponsibilityTotalPrice.Text.Trim() != "")
                        ResponsibilitilyTotalPrice = Convert.ToDecimal(txtResponsibilityTotalPrice.Text);

                    _optCostSubject.ResponsibilitilyTotalPrice = ResponsibilitilyTotalPrice;
                }
                catch
                {
                    MessageBox.Show("责任合价格式填写不正确！");
                    txtResponsibilityTotalPrice.Focus();
                    return false;
                }
                try
                {
                    decimal PlanWorkAmount = 0;
                    if (txtPlanProjectAmount.Text.Trim() != "")
                        PlanWorkAmount = Convert.ToDecimal(txtPlanProjectAmount.Text);

                    _optCostSubject.PlanWorkAmount = PlanWorkAmount;
                }
                catch
                {
                    MessageBox.Show("计划工程量格式填写不正确！");
                    txtPlanProjectAmount.Focus();
                    return false;
                }
                try
                {
                    decimal PlanPrice = 0;
                    if (txtPlanPrice.Text.Trim() != "")
                        PlanPrice = Convert.ToDecimal(txtPlanPrice.Text);

                    _optCostSubject.PlanPrice = PlanPrice;
                }
                catch
                {
                    MessageBox.Show("计划单价格式填写不正确！");
                    txtPlanPrice.Focus();
                    return false;
                }
                try
                {
                    decimal PlanTotalPrice = 0;
                    if (txtPlanTotalPrice.Text.Trim() != "")
                        PlanTotalPrice = Convert.ToDecimal(txtPlanTotalPrice.Text);

                    _optCostSubject.PlanTotalPrice = PlanTotalPrice;
                }
                catch
                {
                    MessageBox.Show("计划合价格式填写不正确！");
                    txtPlanTotalPrice.Focus();
                    return false;
                }

                try
                {
                    decimal AddupAccountProjectAmount = 0;
                    if (txtAddupAccountProjectAmount.Text.Trim() != "")
                        AddupAccountProjectAmount = Convert.ToDecimal(txtAddupAccountProjectAmount.Text);

                    _optCostSubject.AddupAccountProjectAmount = AddupAccountProjectAmount;
                }
                catch
                {
                    MessageBox.Show("累计核算工程量格式填写不正确！");
                    txtAddupAccountProjectAmount.Focus();
                    return false;
                }
                try
                {
                    decimal AddupAccountCost = 0;
                    if (txtAddupAccountCost.Text.Trim() != "")
                        AddupAccountCost = Convert.ToDecimal(txtAddupAccountCost.Text);

                    _optCostSubject.AddupAccountCost = AddupAccountCost;
                }
                catch
                {
                    MessageBox.Show("累计核算成本格式填写不正确！");
                    txtAddupAccountCost.Focus();
                    return false;
                }
                try
                {
                    decimal CurrentPeriodAccountProjectAmount = 0;
                    if (txtCurrPeriodAccountProjectAmount.Text.Trim() != "")
                        CurrentPeriodAccountProjectAmount = Convert.ToDecimal(txtCurrPeriodAccountProjectAmount.Text);

                    _optCostSubject.CurrentPeriodAccountProjectAmount = CurrentPeriodAccountProjectAmount;
                }
                catch
                {
                    MessageBox.Show("当期核算工程量格式填写不正确！");
                    txtCurrPeriodAccountProjectAmount.Focus();
                    return false;
                }
                try
                {
                    decimal CurrentPeriodAccountCost = 0;
                    if (txtCurrPeriodAccountCost.Text.Trim() != "")
                        CurrentPeriodAccountCost = Convert.ToDecimal(txtCurrPeriodAccountCost.Text);

                    _optCostSubject.CurrentPeriodAccountCost = CurrentPeriodAccountCost;
                }
                catch
                {
                    MessageBox.Show("当期核算成本格式填写不正确！");
                    txtCurrPeriodAccountCost.Focus();
                    return false;
                }
                try
                {
                    decimal AddupBalanceProjectAmount = 0;
                    if (txtAddupBalanceProjectAmount.Text.Trim() != "")
                        AddupBalanceProjectAmount = Convert.ToDecimal(txtAddupBalanceProjectAmount.Text);

                    _optCostSubject.AddupBalanceProjectAmount = AddupBalanceProjectAmount;
                }
                catch
                {
                    MessageBox.Show("累计结算工程量格式填写不正确！");
                    txtAddupBalanceProjectAmount.Focus();
                    return false;
                }
                try
                {
                    decimal AddupBalanceTotalPrice = 0;
                    if (txtAddupBalanceTotalPrice.Text.Trim() != "")
                        AddupBalanceTotalPrice = Convert.ToDecimal(txtAddupBalanceTotalPrice.Text);

                    _optCostSubject.AddupBalanceTotalPrice = AddupBalanceTotalPrice;
                }
                catch
                {
                    MessageBox.Show("累计结算合价格式填写不正确！");
                    txtAddupBalanceTotalPrice.Focus();
                    return false;
                }
                try
                {
                    decimal ProjectAmountWasta = 0;
                    if (txtProjectAmountWaste.Text.Trim() != "")
                        ProjectAmountWasta = Convert.ToDecimal(txtProjectAmountWaste.Text);

                    _optCostSubject.ProjectAmountWasta = ProjectAmountWasta;
                }
                catch
                {
                    MessageBox.Show("工程量损耗格式填写不正确！");
                    txtProjectAmountWaste.Focus();
                    return false;
                }
                try
                {
                    decimal CurrentPeriodBalanceProjectAmount = 0;
                    if (txtCurrPeriodBalanceProjectAmount.Text.Trim() != "")
                        CurrentPeriodBalanceProjectAmount = Convert.ToDecimal(txtCurrPeriodBalanceProjectAmount.Text);

                    _optCostSubject.CurrentPeriodBalanceProjectAmount = CurrentPeriodBalanceProjectAmount;
                }
                catch
                {
                    MessageBox.Show("当期结算工程量格式填写不正确！");
                    txtCurrPeriodBalanceProjectAmount.Focus();
                    return false;
                }
                try
                {
                    decimal CurrentPeriodBalanceTotalPrice = 0;
                    if (txtCurrPeroidBalanceTotalPrice.Text.Trim() != "")
                        CurrentPeriodBalanceTotalPrice = Convert.ToDecimal(txtCurrPeroidBalanceTotalPrice.Text);

                    _optCostSubject.CurrentPeriodBalanceTotalPrice = CurrentPeriodBalanceTotalPrice;
                }
                catch
                {
                    MessageBox.Show("当期结算合价格式填写不正确！");
                    txtCurrPeroidBalanceTotalPrice.Focus();
                    return false;
                }
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
        //取消
        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
