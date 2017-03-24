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
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng
{
    public partial class VSubBalanceBatchPrice : TBasicDataView
    {
        public MSubContractBalance model = new MSubContractBalance();
        /// <summary>
        /// 操作的工程任务明细核算
        /// </summary>
        public SubContractBalanceBill master = null;
        private SubContractBalanceSubjectDtl optSubject = null;
        public VSubBalanceBatchPrice(SubContractBalanceBill currMaster)
        {
            master = currMaster;
            InitializeComponent();
            InitForm();
        }

        private void InitForm()
        {
            InitEvents();
            LoadBalanceSubjectByBalanceDtl();
            RefreshControls(MainViewState.Browser);
        }

        private void InitEvents()
        {
            btnBalanceSelect.Click += new EventHandler(btnBalanceSelect_Click);
            this.btnSelectCostSubject.Click += new EventHandler(btnSelectSubject_Click);
            btnSubmit.Click += new EventHandler(btnSubmit_Click);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnSetPrice.Click += new EventHandler(btnSetPrice_Click);
            lnkAll.Click += new EventHandler(lnkAll_Click);
            lnkNone.Click += new EventHandler(lnkNone_Click);
            gridDetail.CellValidating += new DataGridViewCellValidatingEventHandler(gridDetail_CellValidating);
            gridDetail.CellEndEdit += new DataGridViewCellEventHandler(gridDetail_CellEndEdit);
        }

        void btnSetPrice_Click(object sender, EventArgs e)
        {
            decimal newPrice = ClientUtil.ToDecimal(this.txtNewPrice.Text);
            if (newPrice == 0)
            {
                MessageBox.Show("请输入新的价格！");
                return;
            }
            int count = 0;
            foreach (DataGridViewRow var in this.gridDetail.Rows)
            {
                if (ClientUtil.ToBool(var.Cells[colSelected.Name].Value) == true)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                MessageBox.Show("请选择需要修改的记录！");
                return;
            }
            foreach (DataGridViewRow var in this.gridDetail.Rows)
            {
                if (ClientUtil.ToBool(var.Cells[colSelected.Name].Value) == true)
                {
                    SubContractBalanceSubjectDtl dtl = var.Tag as SubContractBalanceSubjectDtl;
                    dtl.BalancePrice = newPrice;
                    dtl.BalanceTotalPrice = decimal.Round(dtl.BalancePrice * dtl.BalanceQuantity, 2);
                    var.Cells[DtlBalancePrice.Name].Value = dtl.BalancePrice;
                    var.Cells[DtlBalanceQuantity.Name].Value = dtl.BalanceQuantity;
                    var.Cells[DtlBalanceTotalPrice.Name].Value = dtl.BalanceTotalPrice;
                    var.Tag = dtl;
                }
            }
        }
        /// <summary>
        /// 反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkNone_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.gridDetail.Rows)
            {
                bool isSelected = (bool)var.Cells[colSelected.Name].Value;
                var.Cells[colSelected.Name].Value = !isSelected;
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.gridDetail.Rows)
            {
                var.Cells[colSelected.Name].Value = true;
            }
        }

        void btnSelectSubject_Click(object sender, EventArgs e)
        {
            VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
            frm.ShowDialog();
            CostAccountSubject cost = frm.SelectAccountSubject;
            if (cost != null)
            {
                this.txtCostSubject.Text = cost.Name;
                this.txtCostSubject.Tag = cost;
            }
        }

        void btnBalanceSelect_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                TreeNode root = frm.SelectResult[0];

                GWBSTree task = root.Tag as GWBSTree;
                if (task != null)
                {
                    txtBalance.Text = task.Name;
                    txtBalance.Tag = task;
                }
            }
        }

        void btnSubmit_Click(object sender, EventArgs e)
        {
            foreach (SubContractBalanceDetail optBalanceDtl in master.Details)
            {
                foreach (SubContractBalanceSubjectDtl subject in optBalanceDtl.Details)
                {

                }
            }
            this.Close();
        }
        void btnSearch_Click(object sender, EventArgs e)
        {
            LoadBalanceSubjectByBalanceDtl();
        }
        void gridDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                object tempValue = gridDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                string value = "";
                if (tempValue != null)
                    value = tempValue.ToString().Trim();

                SubContractBalanceSubjectDtl dtl = gridDetail.Rows[e.RowIndex].Tag as SubContractBalanceSubjectDtl;

                if (gridDetail.Columns[e.ColumnIndex].Name == DtlBalancePrice.Name)//结算单价
                {
                    if (value == "")
                    {
                        dtl.BalancePrice = 0;
                    }
                    else
                    {
                        dtl.BalancePrice = ClientUtil.ToDecimal(value);
                    }
                    dtl.BalanceTotalPrice = dtl.BalancePrice * dtl.BalanceQuantity;
                    gridDetail.Rows[e.RowIndex].Cells[DtlBalanceTotalPrice.Name].Value = decimal.Round(dtl.BalanceTotalPrice, 4);
                }
                gridDetail.Rows[e.RowIndex].Tag = dtl;
            }
        }

        void gridDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && gridDetail.Rows[e.RowIndex].ReadOnly == false)
            {
                object value = e.FormattedValue;
                if (value != null)
                {
                    try
                    {
                        string colName = gridDetail.Columns[e.ColumnIndex].Name;
                        if (colName == DtlBalancePrice.Name || colName == DtlBalanceQuantity.Name)//单价或数量
                        {
                            if (value.ToString() != "")
                                ClientUtil.ToDecimal(value);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("输入格式不正确！");
                        e.Cancel = true;
                    }
                }
            }
        }

        private void SetGridRowReadOnly(bool rowReadOnly)
        {
            //设置编辑的单元格状态
            foreach (DataGridViewRow row in gridDetail.Rows)
            {
                row.ReadOnly = rowReadOnly;
            }
        }

        private void LoadBalanceSubjectByBalanceDtl()
        {
            try
            {
                if (master != null)
                {
                    // GWBS
                    GWBSTree selectGWBS = txtBalance.Tag as GWBSTree;
                    // 科目
                    CostAccountSubject selectCostSub = txtCostSubject.Tag as CostAccountSubject;
                    gridDetail.Rows.Clear();
                    foreach (SubContractBalanceDetail optBalanceDtl in master.Details)
                    {
                        if (optBalanceDtl.FrontBillGUID == null)
                        {
                            continue;
                        }
                        if (this.txtBalance.Text != "" && selectGWBS != null)
                        {
                            if (!optBalanceDtl.BalanceTaskSyscode.Contains(selectGWBS.Id))
                            {
                                continue;
                            }
                        }                                           
                        foreach (SubContractBalanceSubjectDtl subject in optBalanceDtl.Details)
                        {
                            if (this.txtCostSubject.Text != "" && selectCostSub != null)
                            {
                                if (!subject.BalanceSubjectSyscode.Contains(selectCostSub.Id))
                                {
                                    continue;
                                }
                            }
                            //物资
                            if (!txtMaterial.Text.Trim().Equals("") && txtMaterial.Result != null)
                            {
                                if (!subject.ResourceTypeName.Contains(this.txtMaterial.Text))
                                {
                                    continue;
                                }
                            }
                            AddAccountDetailSubjectInGrid(optBalanceDtl, subject);
                        }
                    }
                    gridDetail.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载数据出错，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        private void AddAccountDetailSubjectInGrid(SubContractBalanceDetail optBalanceDtl, SubContractBalanceSubjectDtl dtl)
        {
            int index = gridDetail.Rows.Add();
            DataGridViewRow row = gridDetail.Rows[index];

            row.Cells[DtlProjectTask.Name].Value = optBalanceDtl.BalanceTaskName;
            row.Cells[DtlProjectTaskDetail.Name].Value = optBalanceDtl.BalanceTaskDtlName;
            row.Cells[DtlCostName.Name].Value = dtl.CostName;
            row.Cells[DtlBalanceSubject.Name].Value = dtl.BalanceSubjectName;

            row.Cells[DtlResourceType.Name].Value = dtl.ResourceTypeName;
            row.Cells[DtlResourceSpec.Name].Value = dtl.ResourceTypeSpec;
            row.Cells[DtlResDiagramNumber.Name].Value = dtl.DiagramNumber;
            row.Cells[DtlBalanceQuantity.Name].Value = optBalanceDtl.BalacneQuantity;
            dtl.BalanceQuantity = optBalanceDtl.BalacneQuantity;
            row.Cells[DtlBalancePrice.Name].Value = dtl.BalancePrice;
            decimal totalPrice = decimal.Round(optBalanceDtl.BalacneQuantity * dtl.BalancePrice, 4);
            row.Cells[DtlBalanceTotalPrice.Name].Value = totalPrice;
            dtl.BalanceTotalPrice = totalPrice;
            row.Cells[DtlQuantityUnit.Name].Value = dtl.QuantityUnitName;
            row.Cells[DtlPriceUnit.Name].Value = dtl.PriceUnitName;

            row.Cells[DtlRemark.Name].Value = dtl.Remark;
            row.Tag = dtl;
        }
    }
}
