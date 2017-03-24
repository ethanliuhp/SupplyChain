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

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng
{
    public partial class VEditCostSubjectQuota : Form
    {
        private SubjectCostQuota _optCostQuota = null;
        /// <summary>
        /// 操作分科目成本定额
        /// </summary>
        public SubjectCostQuota OptCostQuota
        {
            get { return _optCostQuota; }
            set { _optCostQuota = value; }
        }

        private CostItem _optCostItem = null;
        /// <summary>
        /// 操作成本项
        /// </summary>
        public CostItem OptCostItem
        {
            get { return _optCostItem; }
            set { _optCostItem = value; }
        }

        public MCostItem model;

        public VEditCostSubjectQuota(MCostItem mot)
        {
            model = mot;
            InitializeComponent();
            InitialEvents();
        }

        private void InitialEvents()
        {
            btnSave.Click += new EventHandler(btnSave_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            btnSelectPriceUnit.Click += new EventHandler(btnSelectPriceUnit_Click);
            btnSelectProjectUnit.Click += new EventHandler(btnSelectProjectUnit_Click);
            btnSelectResource.Click += new EventHandler(btnSelectResource_Click);
            btnSelectSubject.Click += new EventHandler(btnSelectSubject_Click);

            this.Load += new EventHandler(VEditCostSubjectQuota_Load);
        }

        void VEditCostSubjectQuota_Load(object sender, EventArgs e)
        {
            if (OptCostQuota != null)
            {
                txtState.Text = OptCostQuota.State.ToString();

                txtName.Text = OptCostQuota.Name;

                txtAccountSubject.Tag = OptCostQuota.CostAccountSubjectGUID;
                txtAccountSubject.Text = OptCostQuota.CostAccountSubjectName;

                txtResource.Tag = OptCostQuota.ResourceTypeGUID;
                txtResource.Text = OptCostQuota.ResourceTypeName;

                txtContractProjectAmount.Text = OptCostQuota.QuotaProjectAmount.ToString();
                txtContractPrice.Text = OptCostQuota.QuotaPrice.ToString();
                txtMoneyQuota.Text = OptCostQuota.QuotaMoney.ToString();
                txtWastage.Text = OptCostQuota.Wastage.ToString();

                txtProjectUnit.Text = OptCostQuota.ProjectAmountUnitName;
                txtProjectUnit.Tag = OptCostQuota.ProjectAmountUnitGUID;

                txtPriceUnit.Text = OptCostQuota.PriceUnitName;
                txtPriceUnit.Tag = OptCostQuota.PriceUnitGUID;

                txtAssessmentRate.Text = OptCostQuota.AssessmentRate.ToString();

                cbQuantityOrg.Text = OptCostQuota.QuantityResponsibleOrgName;
                cbQuantityOrg.Tag = OptCostQuota.QuantityResponsibleOrgGUID;

                cbPriceOrg.Text = OptCostQuota.PriceResponsibleOrgName;
                cbPriceOrg.Tag = OptCostQuota.PriceResponsibleOrgGUID;
            }
            else
            {
                txtState.Text = SubjectCostQuotaState.编制.ToString();
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
        //选择资源
        void btnSelectResource_Click(object sender, EventArgs e)
        {

        }
        //选择工程量计量单位
        void btnSelectProjectUnit_Click(object sender, EventArgs e)
        {

        }
        //选择价格计量单位
        void btnSelectPriceUnit_Click(object sender, EventArgs e)
        {

        }
        //保存
        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValideSave())
                    return;
                if (string.IsNullOrEmpty(_optCostQuota.Id))
                {
                    _optCostQuota.State = SubjectCostQuotaState.编制;

                    if (!string.IsNullOrEmpty(OptCostItem.Id))
                    {
                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("Id", OptCostItem.Id));
                        oq.AddFetchMode("ListQuotas", NHibernate.FetchMode.Eager);
                        IList list = model.ObjectQuery(typeof(CostItem), oq);
                        OptCostItem = list[0] as CostItem;
                    }
                    _optCostQuota.TheCostItem = OptCostItem;
                    OptCostItem.ListQuotas.Add(_optCostQuota);

                    if (!string.IsNullOrEmpty(OptCostItem.Id))//当成本项存在时才保存明细，否则在保存成本项时一起保存明细
                    {
                        IList listTemp = new ArrayList();
                        listTemp.Add(OptCostItem);
                        listTemp = model.SaveOrUpdateCostItem(listTemp);
                        OptCostItem = listTemp[0] as CostItem;

                        _optCostQuota = OptCostItem.ListQuotas.ElementAt(OptCostItem.ListQuotas.Count - 1);
                    }
                }
                else
                {
                    IList listTemp = new ArrayList();
                    listTemp.Add(_optCostQuota);
                    listTemp = model.SaveOrUpdateCostItemQuota(listTemp);
                    _optCostQuota = listTemp[0] as SubjectCostQuota;
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
                if (_optCostQuota == null)
                {
                    _optCostQuota = new SubjectCostQuota();
                }

                if (string.IsNullOrEmpty(_optCostQuota.TheProjectGUID) || string.IsNullOrEmpty(_optCostQuota.TheProjectName))
                {
                    CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                    if (projectInfo != null)
                    {
                        _optCostQuota.TheProjectGUID = projectInfo.Id;
                        _optCostQuota.TheProjectName = projectInfo.Name;
                    }
                }
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("分科目成本名称不能为空！");
                    txtName.Focus();
                    return false;
                }
                if (txtAccountSubject.Text.Trim() == "")
                {
                    MessageBox.Show("核算科目不能为空！");
                    txtAccountSubject.Focus();
                    return false;
                }
                if (txtResource.Text.Trim() == "")
                {
                    MessageBox.Show("资源不能为空！");
                    txtResource.Focus();
                    return false;
                }
                if (txtContractProjectAmount.Text.Trim() == "")
                {
                    MessageBox.Show("定额工程量不能为空！");
                    txtContractProjectAmount.Focus();
                    return false;
                }
                if (txtContractPrice.Text.Trim() == "")
                {
                    MessageBox.Show("定额单价不能为空！");
                    txtContractPrice.Focus();
                    return false;
                }
                if (txtMoneyQuota.Text.Trim() == "")
                {
                    MessageBox.Show("定额金额不能为空！");
                    txtMoneyQuota.Focus();
                    return false;
                }
                if (txtWastage.Text.Trim() == "")
                {
                    MessageBox.Show("损耗不能为空！");
                    txtWastage.Focus();
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
                if (txtAssessmentRate.Text.Trim() == "")
                {
                    MessageBox.Show("分摊比例不能为空！");
                    txtAssessmentRate.Focus();
                    return false;
                }
                if (cbQuantityOrg.Text.Trim() == "")
                {
                    MessageBox.Show("数量责任组织不能为空！");
                    cbQuantityOrg.Focus();
                    return false;
                }
                if (cbPriceOrg.Text.Trim() == "")
                {
                    MessageBox.Show("单价责任组织不能为空！");
                    cbPriceOrg.Focus();
                    return false;
                }

                //_optCostQuota.Name = txtName.Text.Trim();
                if (txtAccountSubject.Tag != null)
                {
                    CostAccountSubject subject = txtAccountSubject.Tag as CostAccountSubject;
                    if (subject != null)
                    {
                        _optCostQuota.CostAccountSubjectGUID = subject.Id;
                        _optCostQuota.CostAccountSubjectName = subject.Name;
                    }
                }
                else
                {
                    MessageBox.Show("请选择核算科目！");
                    txtAccountSubject.Focus();
                    return false;
                }

                try
                {
                    decimal QuotaProjectAmount = 0;
                    if (txtContractProjectAmount.Text.Trim() != "")
                        QuotaProjectAmount = Convert.ToDecimal(txtContractProjectAmount.Text);

                    _optCostQuota.QuotaProjectAmount = QuotaProjectAmount;
                }
                catch
                {
                    MessageBox.Show("定额工程量格式填写不正确！");
                    txtContractProjectAmount.Focus();
                    return false;
                }

                try
                {
                    decimal QuotaPrice = 0;
                    if (txtContractPrice.Text.Trim() != "")
                        QuotaPrice = Convert.ToDecimal(txtContractPrice.Text);

                    _optCostQuota.QuotaPrice = QuotaPrice;
                }
                catch
                {
                    MessageBox.Show("定额单价格式填写不正确！");
                    txtContractPrice.Focus();
                    return false;
                }

                try
                {
                    decimal QuotaMoney = 0;
                    if (txtMoneyQuota.Text.Trim() != "")
                        QuotaMoney = Convert.ToDecimal(txtMoneyQuota.Text);

                    _optCostQuota.QuotaMoney = QuotaMoney;
                }
                catch
                {
                    MessageBox.Show("定额金额格式填写不正确！");
                    txtMoneyQuota.Focus();
                    return false;
                }

                try
                {
                    decimal Wastage = 0;
                    if (txtWastage.Text.Trim() != "")
                        Wastage = Convert.ToDecimal(txtWastage.Text);

                    _optCostQuota.Wastage = Wastage;
                }
                catch
                {
                    MessageBox.Show("损耗格式填写不正确！");
                    txtWastage.Focus();
                    return false;
                }

                try
                {
                    decimal AssessmentRate = 0;
                    if (txtAssessmentRate.Text.Trim() != "")
                        AssessmentRate = Convert.ToDecimal(txtAssessmentRate.Text);

                    _optCostQuota.AssessmentRate = AssessmentRate;
                }
                catch
                {
                    MessageBox.Show("分摊比例格式填写不正确！");
                    txtAssessmentRate.Focus();
                    return false;
                }

                if (cbQuantityOrg.Result != null && cbQuantityOrg.Result.Count > 0)
                {
                    OperationOrgInfo org = cbQuantityOrg.Result[0] as OperationOrgInfo;
                    if (org != null)
                    {
                        _optCostQuota.QuantityResponsibleOrgGUID = org.Id;
                        _optCostQuota.QuantityResponsibleOrgName = org.Name;
                    }
                }
                else if (cbQuantityOrg.Tag == null)
                {
                    MessageBox.Show("请选择数量责任组织！");
                    cbQuantityOrg.Focus();
                    return false;
                }

                if (cbPriceOrg.Result != null && cbPriceOrg.Result.Count > 0)
                {
                    OperationOrgInfo org = cbPriceOrg.Result[0] as OperationOrgInfo;
                    if (org != null)
                    {
                        _optCostQuota.PriceResponsibleOrgGUID = org.Id;
                        _optCostQuota.PriceResponsibleOrgName = org.Name;
                    }
                }
                else if (cbPriceOrg.Tag == null)
                {
                    MessageBox.Show("请选择单价责任组织！");
                    cbPriceOrg.Focus();
                    return false;
                }

                _optCostQuota.ResourceTypeGUID = "";
                _optCostQuota.ResourceTypeName = txtResource.Text.Trim();

                _optCostQuota.ProjectAmountUnitGUID = "";
                _optCostQuota.ProjectAmountUnitName = txtProjectUnit.Text.Trim();
                _optCostQuota.PriceUnitGUID = "";
                _optCostQuota.PriceUnitName = txtPriceUnit.Text.Trim();

                return true;

            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
                return false;
            }
        }
        //取消
        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
