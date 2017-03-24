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
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.Client.Main;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng
{
    public partial class VAccountDetailSubject : TBasicDataView
    {
        public MProjectTaskAccount model = new MProjectTaskAccount();

        /// <summary>
        /// 操作的工程任务明细核算
        /// </summary>
        public ProjectTaskDetailAccount optAccountDtl = null;

        private ProjectTaskDetailAccountSubject optSubject = null;

        public MainViewState OptionView = MainViewState.Modify;

        public VAccountDetailSubject(ProjectTaskDetailAccount dtl)
        {
            optAccountDtl = dtl;

            InitializeComponent();

            InitForm();
        }
        public VAccountDetailSubject(ref ProjectTaskDetailAccount dtl)
        {
            optAccountDtl = dtl;

            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();

            cbIsBalance.Items.Add("是");
            cbIsBalance.Items.Add("否");

            LoadAccountSubjectByAccountDtl();

            RefreshControls(MainViewState.Browser);
        }
        private void InitEvents()
        {
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnSaveAndExit.Click += new EventHandler(btnSaveAndExit_Click);
            btnExit.Click += new EventHandler(btnExit_Click);

            gridDetail.CellClick += new DataGridViewCellEventHandler(gridDetail_CellClick);

            txtAccountQuantity.TextChanged += new EventHandler(txtAccountQuantity_TextChanged);
            txtAccountPrice.TextChanged += new EventHandler(txtAccountPrice_TextChanged);
            txtAccountUsageQuantity.TextChanged += new EventHandler(txtAccountUsageQuantity_TextChanged);

            txtProjectAmountPrice.TextChanged += new EventHandler(txtProjectAmountPrice_TextChanged);

            txtProjectAmountUnit.LostFocus += new EventHandler(txtProjectAmountUnit_LostFocus);
            txtPriceUnit.LostFocus += new EventHandler(txtPriceUnit_LostFocus);

            btnSelProjectQnyUnit.Click += new EventHandler(btnSelProjectQnyUnit_Click);
            btnSelPriceUnit.Click += new EventHandler(btnSelPriceUnit_Click);

            this.Load += new EventHandler(VAccountDetailSubject_Load);
        }

        void btnAdd_Click(object sender, EventArgs e)
        {
            VAddAccountUsage frm = new VAddAccountUsage(optAccountDtl);
            frm.ShowDialog();
            if (frm.IsOK)
            {
                List<GWBSDetailCostSubject> listDtlUsage = new List<GWBSDetailCostSubject>();

                foreach (GWBSDetailCostSubject dtlUsage in frm.listAddDtlUsage)
                {
                    bool flag = false;//耗用是否已存在
                    foreach (ProjectTaskDetailAccountSubject accUsage in optAccountDtl.Details)
                    {
                        if (accUsage.BestaetigtCostSubjectGUID != null && accUsage.BestaetigtCostSubjectGUID.Id == dtlUsage.Id)
                        {
                            listDtlUsage.Add(dtlUsage);
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                        continue;

                    ProjectTaskDetailAccountSubject billDtlUsage = new ProjectTaskDetailAccountSubject();
                    billDtlUsage.TheAccountDetail = optAccountDtl;
                    optAccountDtl.Details.Add(billDtlUsage);

                    billDtlUsage.TheProjectGUID = optAccountDtl.TheProjectGUID;
                    billDtlUsage.TheProjectName = optAccountDtl.TheProjectName;

                    billDtlUsage.BestaetigtCostSubjectGUID = dtlUsage;
                    billDtlUsage.BestaetigtCostSubjectName = dtlUsage.Name;
                    billDtlUsage.CostingSubjectGUID = dtlUsage.CostAccountSubjectGUID;
                    billDtlUsage.CostingSubjectName = dtlUsage.CostAccountSubjectName;
                    billDtlUsage.ResourceTypeGUID = dtlUsage.ResourceTypeGUID;
                    billDtlUsage.ResourceTypeName = dtlUsage.ResourceTypeName;
                    billDtlUsage.ResourceTypeQuality = dtlUsage.ResourceTypeQuality;
                    billDtlUsage.ResourceTypeSpec = dtlUsage.ResourceTypeSpec;
                    billDtlUsage.ResourceCategorySysCode = dtlUsage.ResourceCateSyscode;

                    billDtlUsage.QuantityUnitGUID = dtlUsage.ProjectAmountUnitGUID;
                    billDtlUsage.QuantityUnitName = dtlUsage.ProjectAmountUnitName;
                    billDtlUsage.PriceUnitGUID = optAccountDtl.PriceUnitGUID;
                    billDtlUsage.PriceUnitName = optAccountDtl.PriceUnitName;

                    billDtlUsage.ContractQuotaNum = dtlUsage.ContractQuotaQuantity;
                    billDtlUsage.ContractQuantityPrice = dtlUsage.ContractQuantityPrice;
                    billDtlUsage.ContractProjectAmountPrice = dtlUsage.ContractPrice;
                    billDtlUsage.ResContractQuantity = dtlUsage.ContractProjectAmount;
                    billDtlUsage.ContractIncomeTotal = dtlUsage.ContractTotalPrice;

                    billDtlUsage.ResponsibleQuantity = dtlUsage.ResponsibleQuotaNum;
                    billDtlUsage.ResponsibleQnyPrice = dtlUsage.ResponsibilitilyPrice;
                    billDtlUsage.ResponsibleWorkQnyPrice = dtlUsage.ResponsibleWorkPrice;
                    billDtlUsage.ResponsibleUsageQny = dtlUsage.ResponsibilitilyWorkAmount;
                    billDtlUsage.ResponsibleUsageTotal = dtlUsage.ResponsibilitilyTotalPrice;

                    billDtlUsage.PlanQuantity = dtlUsage.PlanQuotaNum;
                    billDtlUsage.PlanQnyPrice = dtlUsage.PlanPrice;
                    billDtlUsage.PlanWorkQnyPrice = dtlUsage.PlanWorkPrice;
                    billDtlUsage.PlanUsageQny = dtlUsage.PlanWorkAmount;
                    billDtlUsage.PlanUsageTotal = dtlUsage.PlanTotalPrice;


                    billDtlUsage.AccountQuantity = billDtlUsage.PlanQuantity;
                    billDtlUsage.AccountPrice = billDtlUsage.PlanQnyPrice;
                    billDtlUsage.AccWorkQnyPrice = billDtlUsage.PlanWorkQnyPrice;
                    billDtlUsage.AccUsageQny = optAccountDtl.AccountProjectAmount * billDtlUsage.AccountQuantity;
                    billDtlUsage.AccountTotalPrice = billDtlUsage.AccUsageQny * billDtlUsage.AccountPrice;

                    billDtlUsage.CurrContractIncomeQny = optAccountDtl.CurrAccFigureProgress * billDtlUsage.ResContractQuantity;
                    billDtlUsage.CurrContractIncomeTotal = billDtlUsage.CurrContractIncomeQny * billDtlUsage.ContractQuantityPrice;
                    billDtlUsage.CurrResponsibleCostQny = optAccountDtl.CurrAccFigureProgress * billDtlUsage.ResponsibleUsageQny;
                    billDtlUsage.CurrResponsibleCostTotal = billDtlUsage.CurrResponsibleCostQny * billDtlUsage.ResponsibleQnyPrice;

                    AddAccountDetailSubjectInGrid(billDtlUsage);
                }

                if (listDtlUsage.Count > 0)
                {
                    string dtlName = "";
                    foreach (GWBSDetailCostSubject usage in listDtlUsage)
                    {
                        dtlName += usage.Name + ",";
                    }
                    dtlName = dtlName.Substring(0, dtlName.Length - 1);
                    MessageBox.Show("已存在耗用“" + dtlName + "”！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridDetail.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请选择要删除的行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                IList listDeleteUsage = new ArrayList();
                List<int> listRowIndex = new List<int>();
                foreach (DataGridViewRow row in gridDetail.SelectedRows)
                {
                    ProjectTaskDetailAccountSubject dtl = row.Tag as ProjectTaskDetailAccountSubject;
                    listDeleteUsage.Add(dtl);
                    listRowIndex.Add(row.Index);
                }

                if (MessageBox.Show("确实要删除选中的核算资源耗用吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                optAccountDtl.Details.RemoveAll(listDeleteUsage.OfType<ProjectTaskDetailAccountSubject>().ToArray());
                //model.DeleteProjectTaskAccountDtlUsage(listDeleteUsage);

                listRowIndex.Sort();
                for (int i = listRowIndex.Count - 1; i > -1; i--)
                {
                    int rowIndex = listRowIndex[i];
                    gridDetail.Rows.RemoveAt(rowIndex);
                }

                MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        void btnSelProjectQnyUnit_Click(object sender, EventArgs e)
        {
            SelectUnit(txtProjectAmountUnit);
        }

        void btnSelPriceUnit_Click(object sender, EventArgs e)
        {
            SelectUnit(txtPriceUnit);
        }

        void txtProjectAmountUnit_LostFocus(object sender, EventArgs e)
        {
            txtPriceUnit.LostFocus -= new EventHandler(txtPriceUnit_LostFocus);
            SetStandardUnit(txtProjectAmountUnit);
            txtPriceUnit.LostFocus += new EventHandler(txtPriceUnit_LostFocus);
        }

        void txtPriceUnit_LostFocus(object sender, EventArgs e)
        {
            txtProjectAmountUnit.LostFocus -= new EventHandler(txtProjectAmountUnit_LostFocus);
            SetStandardUnit(txtPriceUnit);
            txtProjectAmountUnit.LostFocus += new EventHandler(txtProjectAmountUnit_LostFocus);
        }

        private void SetStandardUnit(object sender)
        {
            TextBox tbUnit = sender as TextBox;
            string name = tbUnit.Text.Trim();
            if (name != "")
            {
                if (tbUnit.Tag == null || (tbUnit.Tag != null && (tbUnit.Tag as StandardUnit).Name != name))
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Name", name));
                    IList list = model.ObjectQuery(typeof(StandardUnit), oq);
                    if (list.Count > 0)
                    {
                        tbUnit.Tag = list[0] as StandardUnit;
                    }
                    else
                    {
                        MessageBox.Show("系统目前不存在该计量单位，请检查！");
                        SelectUnit(tbUnit);
                    }
                }
            }
        }
        private void SelectUnit(TextBox txt)
        {
            StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
            if (su != null)
            {
                txt.Tag = su;
                txt.Text = su.Name;
                txt.Focus();
            }
        }

        void VAccountDetailSubject_Load(object sender, EventArgs e)
        {
            if (OptionView == MainViewState.Browser)
            {
                btnAdd.Enabled = false;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                btnSaveAndExit.Enabled = false;
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //核算工程量单价
        void txtProjectAmountPrice_TextChanged(object sender, EventArgs e)
        {
            if (this.ActiveControl != null && this.ActiveControl.Name == txtProjectAmountPrice.Name)//直接修改当前值有效，联动修改不处理
            {
                //i> 直接输入【核算工程量单价】，此时将【核算定额数量】设为1、【核算数量单价】设置为【核算工程量单价】、【数量计量单位】设置为所属{工程任务明细核算}_【工程量计量单位】。

                txtAccountQuantity.Text = "1";
                txtAccountPrice.Text = txtProjectAmountPrice.Text;

                decimal price = ClientUtil.ToDecimal(txtAccountPrice.Text);
                if (optAccountDtl != null)
                {
                    txtAccountUsageQuantity.Text = (optAccountDtl.AccountProjectAmount * 1).ToString();
                    txtAccountTotalPrice.Text = DecimalToString(price * optAccountDtl.AccountProjectAmount * 1);
                }

                txtProjectAmountUnit.Tag = optAccountDtl.QuantityUnitGUID;
                txtProjectAmountUnit.Text = optAccountDtl.QuantityUnitName;
            }
        }
        //核算数量单价
        void txtAccountPrice_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //ii> 直接输入【核算定额数量】和【核算数量单价】，此时设置【核算工程量单价】=【核算定额数量】*【核算数量单价】；
                decimal price = 0;
                decimal quantity = 0;
                decimal usageQuantity = 0;

                if (txtAccountPrice.Text.Trim() != "")
                    price = ClientUtil.ToDecimal(txtAccountPrice.Text);

                if (txtAccountQuantity.Text.Trim() != "")
                {
                    quantity = ClientUtil.ToDecimal(txtAccountQuantity.Text);
                    txtProjectAmountPrice.Text = DecimalToString(price * quantity);
                }

                if (txtAccountUsageQuantity.Text.Trim() != "")
                {
                    usageQuantity = ClientUtil.ToDecimal(txtAccountUsageQuantity.Text);
                    txtAccountTotalPrice.Text = DecimalToString(price * usageQuantity);
                }
            }
            catch { }
        }
        //核算定额数量
        void txtAccountQuantity_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //ii> 直接输入【核算定额数量】和【核算数量单价】，此时设置【核算工程量单价】=【核算定额数量】*【核算数量单价】；
                decimal price = 0;
                decimal quantity = 0;

                if (txtAccountQuantity.Text.Trim() != "")
                    quantity = ClientUtil.ToDecimal(txtAccountQuantity.Text);

                if (txtAccountPrice.Text.Trim() != "")
                {
                    price = ClientUtil.ToDecimal(txtAccountPrice.Text);
                    txtProjectAmountPrice.Text = DecimalToString(price * quantity);
                }
                if (optAccountDtl != null)
                {
                    txtAccountUsageQuantity.Text = DecimalToString(optAccountDtl.AccountProjectAmount * quantity);
                }

            }
            catch { }
        }
        //核算消耗数量
        void txtAccountUsageQuantity_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal price = 0;
                decimal usageQuantity = 0;

                if (txtAccountUsageQuantity.Text.Trim() != "")
                    usageQuantity = ClientUtil.ToDecimal(txtAccountUsageQuantity.Text);

                if (txtAccountPrice.Text.Trim() != "")
                {
                    price = ClientUtil.ToDecimal(txtAccountPrice.Text);
                    txtAccountTotalPrice.Text = DecimalToString(price * usageQuantity);
                }
            }
            catch { }
        }

        void gridDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                ProjectTaskDetailAccountSubject tempSubject = gridDetail.Rows[e.RowIndex].Tag as ProjectTaskDetailAccountSubject;
                if (optSubject == tempSubject)
                    return;

                if (btnSave.Enabled == true && tempSubject != optSubject)
                {
                    if (MessageBox.Show("有尚未保存的成本项信息，要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (!SaveSubjectAccount())
                        {
                            //修改时还原选中的修改行
                            foreach (DataGridViewRow row in gridDetail.Rows)
                            {
                                ProjectTaskDetailAccountSubject temp = row.Tag as ProjectTaskDetailAccountSubject;
                                if (temp.Id == optSubject.Id)
                                {
                                    gridDetail.CurrentCell = row.Cells[0];
                                    break;
                                }
                            }
                            return;
                        }
                    }
                }

                optSubject = gridDetail.Rows[e.RowIndex].Tag as ProjectTaskDetailAccountSubject;

                if (!string.IsNullOrEmpty(optSubject.Id))
                {
                    try
                    {
                        if (optSubject.PriceUnitGUID != null)
                        {
                            string name = optSubject.PriceUnitGUID.Name;
                        }
                        if (optSubject.QuantityUnitGUID != null)
                        {
                            string name = optSubject.QuantityUnitGUID.Name;
                        }
                    }
                    catch
                    {
                        optSubject = LoadRelaObj(optSubject);
                        gridDetail.Rows[e.RowIndex].Tag = optSubject;
                    }
                }

                LoadAccountSubjectInfo(optSubject);
                gridDetail.CurrentCell = gridDetail.Rows[e.RowIndex].Cells[0];
                RefreshControls(MainViewState.Browser);
            }
        }

        private ProjectTaskDetailAccountSubject LoadRelaObj(ProjectTaskDetailAccountSubject dtl)
        {
            if (!string.IsNullOrEmpty(dtl.Id))
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", dtl.Id));
                oq.AddFetchMode("QuantityUnitGUID", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);

                dtl = model.ObjectQuery(typeof(ProjectTaskDetailAccountSubject), oq)[0] as ProjectTaskDetailAccountSubject;
            }

            return dtl;
        }

        private void LoadAccountSubjectByAccountDtl()
        {
            try
            {
                if (optAccountDtl != null)
                {
                    //if (!string.IsNullOrEmpty(optAccountDtl.Id))
                    //{
                    //    ObjectQuery oq = new ObjectQuery();
                    //    oq.AddCriterion(Expression.Eq("Id", optAccountDtl.Id));
                    //    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                    //    ProjectTaskDetailAccount tempDtl = model.ObjectQuery(typeof(ProjectTaskDetailAccount), oq)[0] as ProjectTaskDetailAccount;

                    //    tempDtl.AccountPrice = optAccountDtl.AccountPrice;
                    //    tempDtl.AccountProjectAmount = optAccountDtl.AccountProjectAmount;
                    //    tempDtl.AccountTotalPrice = optAccountDtl.AccountTotalPrice;
                    //    tempDtl.Remark = optAccountDtl.Remark;

                    //    optAccountDtl = tempDtl;
                    //}

                    foreach (ProjectTaskDetailAccountSubject subject in optAccountDtl.Details)
                    {
                        AddAccountDetailSubjectInGrid(subject);
                    }
                    gridDetail.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载数据出错，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        private void LoadAccountSubjectInfo(ProjectTaskDetailAccountSubject subject)
        {

            txtAccountQuantity.Text = DecimalToString(subject.AccountQuantity);
            txtAccountPrice.Text = DecimalToString(subject.AccountPrice);
            txtProjectAmountPrice.Text = DecimalToString(subject.AccWorkQnyPrice);
            txtAccountUsageQuantity.Text = DecimalToString(subject.AccUsageQny);
            txtAccountTotalPrice.Text = DecimalToString(subject.AccountTotalPrice);

            cbIsBalance.Text = subject.IsBalance ? "是" : "否";
            txtProjectAmountUnit.Tag = subject.QuantityUnitGUID;
            txtProjectAmountUnit.Text = subject.QuantityUnitName;

            txtPriceUnit.Tag = subject.PriceUnitGUID;
            txtPriceUnit.Text = subject.PriceUnitName;
        }

        public string DecimalToString(decimal value)
        {
            return decimal.Round(value, 5).ToString();
        }


        private void AddAccountDetailSubjectInGrid(ProjectTaskDetailAccountSubject dtl)
        {
            int index = gridDetail.Rows.Add();
            DataGridViewRow row = gridDetail.Rows[index];

            row.Cells[DtlProjectTaskDetail.Name].Value = dtl.TheAccountDetail.ProjectTaskDtlName;
            row.Cells[DtlIsBalance.Name].Value = dtl.IsBalance ? "是" : "否";
            row.Cells[DtlCostName.Name].Value = dtl.BestaetigtCostSubjectName;
            row.Cells[DtlCostAccountSubject.Name].Value = dtl.CostingSubjectName;

            row.Cells[DtlResourceType.Name].Value = dtl.ResourceTypeName;
            row.Cells[DtlResourceTypeQuanlity.Name].Value = dtl.ResourceTypeQuality;
            row.Cells[DtlResourceTypeSpec.Name].Value = dtl.ResourceTypeSpec;
            row.Cells[DtlAccQuotaQuantity.Name].Value = DecimalToString(dtl.AccountQuantity);
            row.Cells[DtlAccQuantityPrice.Name].Value = DecimalToString(dtl.AccountPrice);
            row.Cells[DtlAccProjectAmountPrice.Name].Value = DecimalToString(dtl.AccWorkQnyPrice);
            row.Cells[DtlAccUsageQuantity.Name].Value = DecimalToString(dtl.AccUsageQny);
            row.Cells[DtlAccTotalPrice.Name].Value = DecimalToString(dtl.AccountTotalPrice);

            row.Cells[DtlCurrContractIncomeQny.Name].Value = DecimalToString(dtl.CurrContractIncomeQny);
            row.Cells[DtlCurrIncomeContractTotal.Name].Value = DecimalToString(dtl.CurrContractIncomeTotal);
            row.Cells[DtlCurrResponsibleCostQny.Name].Value = DecimalToString(dtl.CurrResponsibleCostQny);
            row.Cells[DtlCurrResponsibleCostTotal.Name].Value = DecimalToString(dtl.CurrResponsibleCostTotal);

            row.Tag = dtl;
        }
        private void UpdateAccountDetailSubjectInGrid(ProjectTaskDetailAccountSubject dtl)
        {
            foreach (DataGridViewRow row in gridDetail.Rows)
            {
                ProjectTaskDetailAccountSubject subject = row.Tag as ProjectTaskDetailAccountSubject;

                if ((subject.Id != null && subject.Id == dtl.Id) || subject == dtl)
                {
                    row.Cells[DtlProjectTaskDetail.Name].Value = dtl.TheAccountDetail.ProjectTaskDtlName;
                    row.Cells[DtlIsBalance.Name].Value = dtl.IsBalance ? "是" : "否";
                    row.Cells[DtlCostName.Name].Value = dtl.BestaetigtCostSubjectName;
                    row.Cells[DtlCostAccountSubject.Name].Value = dtl.CostingSubjectName;

                    row.Cells[DtlResourceType.Name].Value = dtl.ResourceTypeName;
                    row.Cells[DtlResourceTypeQuanlity.Name].Value = dtl.ResourceTypeQuality;
                    row.Cells[DtlResourceTypeSpec.Name].Value = dtl.ResourceTypeSpec;
                    row.Cells[DtlAccQuotaQuantity.Name].Value = dtl.AccountQuantity;
                    row.Cells[DtlAccQuantityPrice.Name].Value = DecimalToString(dtl.AccountPrice);
                    row.Cells[DtlAccProjectAmountPrice.Name].Value = DecimalToString(dtl.AccWorkQnyPrice);
                    row.Cells[DtlAccUsageQuantity.Name].Value = DecimalToString(dtl.AccUsageQny);
                    row.Cells[DtlAccTotalPrice.Name].Value = DecimalToString(dtl.AccountTotalPrice);

                    row.Cells[DtlCurrContractIncomeQny.Name].Value = DecimalToString(dtl.CurrContractIncomeQny);
                    row.Cells[DtlCurrIncomeContractTotal.Name].Value = DecimalToString(dtl.CurrContractIncomeTotal);
                    row.Cells[DtlCurrResponsibleCostQny.Name].Value = DecimalToString(dtl.CurrResponsibleCostQny);
                    row.Cells[DtlCurrResponsibleCostTotal.Name].Value = DecimalToString(dtl.CurrResponsibleCostTotal);

                    row.Tag = dtl;

                    break;
                }
            }
        }

        //选择资源类型
        void btnSelectResourceType_Click(object sender, EventArgs e)
        {

        }
        //选择核算科目
        void btnSelectCostSubject_Click(object sender, EventArgs e)
        {

        }
        //保存
        void btnSave_Click(object sender, EventArgs e)
        {
            SaveSubjectAccount();
        }
        //保存并退出
        void btnSaveAndExit_Click(object sender, EventArgs e)
        {
            if (SaveSubjectAccount())
                this.Close();
        }
        //修改
        void btnUpdate_Click(object sender, EventArgs e)
        {
            if (gridDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要修改的行！");
                return;
            }
            if (gridDetail.SelectedRows.Count > 1)
            {
                MessageBox.Show("一次只能修改一行！");
                return;
            }

            optSubject = gridDetail.SelectedRows[0].Tag as ProjectTaskDetailAccountSubject;

            GetAccountSubject();

            RefreshControls(MainViewState.Modify);
        }

        private void GetAccountSubject()
        {
            try
            {
                ClearAccountSubject();

                txtAccountQuantity.Text = optSubject.AccountQuantity.ToString();
                txtAccountPrice.Text = optSubject.AccountPrice.ToString();
                txtProjectAmountPrice.Text = optSubject.AccWorkQnyPrice.ToString();
                txtAccountUsageQuantity.Text = optSubject.AccUsageQny.ToString();
                txtAccountTotalPrice.Text = optSubject.AccountTotalPrice.ToString();

                cbIsBalance.Text = optSubject.IsBalance ? "是" : "否";
                txtProjectAmountUnit.Tag = optSubject.QuantityUnitGUID;
                txtProjectAmountUnit.Text = optSubject.QuantityUnitName;

                txtPriceUnit.Tag = optSubject.PriceUnitGUID;
                txtPriceUnit.Text = optSubject.PriceUnitName;
            }
            catch (Exception exp)
            {
                MessageBox.Show("显示出错：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        private void ClearAccountSubject()
        {


            txtAccountQuantity.Text = "";
            txtAccountPrice.Text = "";
            txtProjectAmountPrice.Text = "";
            txtAccountUsageQuantity.Text = "";
            txtAccountTotalPrice.Text = "";

            txtProjectAmountUnit.Text = "";
            txtPriceUnit.Text = "";
        }

        private bool SaveSubjectAccount()
        {
            try
            {
                if (!ValideSubjectAccountSave())
                    return false;

                if (optSubject.Id == null)//在此页面添加
                {
                    UpdateAccountDetailSubjectInGrid(optSubject);
                }
                else
                {
                    //IList list = new ArrayList();
                    //list.Add(optSubject);
                    //list = model.SaveOrUpdateProjectTaskAccount(list);
                    //optSubject = list[0] as ProjectTaskDetailAccountSubject;

                    UpdateAccountDetailSubjectInGrid(optSubject);
                }

                RefreshControls(MainViewState.Browser);
            }
            catch (Exception exp)
            {
                MessageBox.Show("保存失败，异常信息：" + ExceptionUtil.ExceptionMessage(exp));
                return false;
            }

            return true;
        }

        private bool ValideSubjectAccountSave()
        {
            try
            {


                //if (txtAccountQuantity.Text.Trim() == "")
                //{
                //    MessageBox.Show("核算数量不能为空！");
                //    txtAccountQuantity.Focus();
                //    return false;
                //}
                //if (txtAccountPrice.Text.Trim() == "")
                //{
                //    MessageBox.Show("核算单价不能为空！");
                //    txtAccountPrice.Focus();
                //    return false;
                //}
                //if (txtProjectAmountUnit.Text.Trim() == "")
                //{
                //    MessageBox.Show("工程量单位不能为空！");
                //    txtProjectAmountUnit.Focus();
                //    return false;
                //}

                //if (txtPriceUnit.Text.Trim() == "")
                //{
                //    MessageBox.Show("价格单位不能为空！");
                //    txtPriceUnit.Focus();
                //    return false;
                //}


                try
                {
                    decimal AccountQuantity = 0;
                    if (txtAccountQuantity.Text.Trim() != "")
                        AccountQuantity = Convert.ToDecimal(txtAccountQuantity.Text);

                    optSubject.AccountQuantity = AccountQuantity;
                }
                catch
                {
                    MessageBox.Show("核算定额数量格式填写不正确！");
                    txtAccountQuantity.Focus();
                    return false;
                }

                try
                {
                    decimal AccountPrice = 0;
                    if (txtAccountPrice.Text.Trim() != "")
                        AccountPrice = Convert.ToDecimal(txtAccountPrice.Text);

                    optSubject.AccountPrice = AccountPrice;
                }
                catch
                {
                    MessageBox.Show("核算数量单价格式填写不正确！");
                    txtAccountPrice.Focus();
                    return false;
                }

                try
                {
                    decimal AccWorkQnyPrice = 0;
                    if (txtProjectAmountPrice.Text.Trim() != "")
                        AccWorkQnyPrice = Convert.ToDecimal(txtProjectAmountPrice.Text);

                    optSubject.AccWorkQnyPrice = AccWorkQnyPrice;
                }
                catch
                {
                    MessageBox.Show("核算工程量单价格式填写不正确！");
                    txtProjectAmountPrice.Focus();
                    return false;
                }


                try
                {
                    decimal AccUsageQny = 0;
                    if (txtAccountUsageQuantity.Text.Trim() != "")
                        AccUsageQny = Convert.ToDecimal(txtAccountUsageQuantity.Text);

                    optSubject.AccUsageQny = AccUsageQny;
                }
                catch
                {
                    MessageBox.Show("核算耗用数量格式填写不正确！");
                    txtAccountUsageQuantity.Focus();
                    return false;
                }

                try
                {
                    decimal AccountTotalPrice = 0;
                    if (txtAccountTotalPrice.Text.Trim() != "")
                        AccountTotalPrice = Convert.ToDecimal(txtAccountTotalPrice.Text);

                    optSubject.AccountTotalPrice = AccountTotalPrice;
                }
                catch
                {
                    MessageBox.Show("核算合价格式填写不正确！");
                    txtAccountTotalPrice.Focus();
                    return false;
                }

                optSubject.IsBalance = cbIsBalance.Text.Trim() == "是";

                if (txtProjectAmountUnit.Text.Trim() != "" && txtProjectAmountUnit.Tag != null)
                {
                    optSubject.QuantityUnitGUID = txtProjectAmountUnit.Tag as StandardUnit;
                    if (optSubject.QuantityUnitGUID != null)
                        optSubject.QuantityUnitName = optSubject.QuantityUnitGUID.Name;

                }
                else
                {
                    optSubject.QuantityUnitGUID = null;
                    optSubject.QuantityUnitName = "";
                }

                if (txtPriceUnit.Text.Trim() != "" && txtPriceUnit.Tag != null)
                {
                    optSubject.PriceUnitGUID = txtPriceUnit.Tag as StandardUnit;
                    if (optSubject.PriceUnitGUID != null)
                        optSubject.PriceUnitName = optSubject.PriceUnitGUID.Name;
                }
                else
                {
                    optSubject.PriceUnitGUID = null;
                    optSubject.PriceUnitName = "";
                }

                if (!string.IsNullOrEmpty(optSubject.Id))
                {
                    for (int i = 0; i < optAccountDtl.Details.Count; i++)
                    {
                        ProjectTaskDetailAccountSubject item = optAccountDtl.Details.ElementAt(i);
                        if (item.Id == optSubject.Id)
                        {
                            optAccountDtl.Details.Remove(item);
                            optAccountDtl.Details.Add(optSubject);
                            break;
                        }
                    }
                }

                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
                return false;
            }
        }

        /// <summary>
        /// 刷新控件(窗体中的控件)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshControls(MainViewState state)
        {
            switch (state)
            {
                case MainViewState.AddNew:
                    break;
                case MainViewState.Modify:

                    txtAccountQuantity.Enabled = true;
                    txtAccountPrice.Enabled = true;
                    txtProjectAmountPrice.Enabled = true;
                    txtAccountUsageQuantity.Enabled = true;
                    txtAccountTotalPrice.Enabled = true;

                    cbIsBalance.Enabled = true;
                    txtProjectAmountUnit.Enabled = true;
                    txtPriceUnit.Enabled = true;

                    btnSelProjectQnyUnit.Enabled = true;
                    btnSelPriceUnit.Enabled = true;

                    btnUpdate.Enabled = false;
                    btnAdd.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnSaveAndExit.Enabled = true;
                    break;

                case MainViewState.Browser:

                    txtAccountQuantity.Enabled = false;
                    txtAccountPrice.Enabled = false;
                    txtProjectAmountPrice.Enabled = false;
                    txtAccountUsageQuantity.Enabled = false;
                    txtAccountTotalPrice.Enabled = false;

                    cbIsBalance.Enabled = false;
                    txtProjectAmountUnit.Enabled = false;
                    txtPriceUnit.Enabled = false;

                    btnSelProjectQnyUnit.Enabled = false;
                    btnSelPriceUnit.Enabled = false;

                    if (OptionView == MainViewState.Browser)
                    {
                        btnUpdate.Enabled = false;
                        btnAdd.Enabled = false;
                        btnDelete.Enabled = false;
                    }
                    else
                    {
                        btnUpdate.Enabled = true;
                        btnAdd.Enabled = true;
                        btnDelete.Enabled = true;
                    }
                    btnSave.Enabled = false;
                    btnSaveAndExit.Enabled = false;
                    break;
            }
        }
    }
}
