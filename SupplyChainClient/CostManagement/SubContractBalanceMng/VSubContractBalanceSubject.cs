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
using VirtualMachine.Patterns.BusinessEssence.Domain;
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


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng
{
    public partial class VSubContractBalanceSubject : TBasicDataView
    {
        public MSubContractBalance model = new MSubContractBalance();

        /// <summary>
        /// 操作的工程任务明细核算
        /// </summary>
        public SubContractBalanceDetail optBalanceDtl = null;

        private SubContractBalanceSubjectDtl optSubject = null;
        CurrentProjectInfo projectInfo = null;

        public VSubContractBalanceSubject(SubContractBalanceDetail dtl)
        {
            optBalanceDtl = dtl;

            InitializeComponent();

            InitForm();
        }

        public delegate void AfterBalanceValueChangedEvent(SubContractBalanceDetail dt);
        public AfterBalanceValueChangedEvent AfterBalanceValueChanged;

        private void InitForm()
        {
            InitEvents();
            LoadBalanceSubjectByBalanceDtl();
            RefreshControls(MainViewState.Browser);
            SetColumnEditable();
        }

        private void InitEvents()
        {
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnClose.Click += new EventHandler(btnClose_Click);

            gridDetail.CellValidating += new DataGridViewCellValidatingEventHandler(gridDetail_CellValidating);
            gridDetail.CellEndEdit += new DataGridViewCellEventHandler(gridDetail_CellEndEdit);
        }

        private void SetColumnEditable()
        {
            //var isEdit = optBalanceDtl != null && optBalanceDtl.Master != null && optBalanceDtl.Master.DocState == DocumentState.Edit;
            var isEdit = true;
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            if (projectInfo.ProjectInfoState == EnumProjectInfoState.新项目 && optBalanceDtl != null && optBalanceDtl.FontBillType != FrontBillType.计时派工)
            {
                isEdit = false;
            }
            isEdit = true;//2016-12-21
            DtlBalancePrice.ReadOnly = !isEdit;
        }

        void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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

                if (gridDetail.Columns[e.ColumnIndex].Name == DtlBalancePrice.Name) //核算工程量
                {
                    if (value == "")
                    {
                        dtl.BalancePrice = 0;
                    }
                    else
                    {
                        dtl.BalancePrice = ClientUtil.ToDecimal(value);
                    }
                    dtl.BalanceTotalPrice = dtl.BalancePrice*dtl.BalanceQuantity;
                    gridDetail.Rows[e.RowIndex].Cells[DtlBalanceTotalPrice.Name].Value =
                        decimal.Round(dtl.BalanceTotalPrice, 4);
                }
                if (gridDetail.Columns[e.ColumnIndex].Name == DtlBalanceQuantity.Name) //核算工程量
                {
                    if (value == "")
                    {
                        dtl.BalanceQuantity = 0;
                    }
                    else
                    {
                        dtl.BalanceQuantity = ClientUtil.ToDecimal(value);
                    }

                    dtl.BalanceTotalPrice = dtl.BalancePrice*dtl.BalanceQuantity;
                    gridDetail.Rows[e.RowIndex].Cells[DtlBalanceTotalPrice.Name].Value =
                        decimal.Round(dtl.BalanceTotalPrice, 4);
                }
                else if (gridDetail.Columns[e.ColumnIndex].Name == DtlRemark.Name)
                {
                    dtl.Remark = value;
                }
                gridDetail.Rows[e.RowIndex].Tag = dtl;

                var itemIndex = optBalanceDtl.Details.ToList().FindIndex(a => a.Id == dtl.Id);
                if (itemIndex >= 0)
                {
                    optBalanceDtl.Details.ToList()[itemIndex] = dtl;
                }
                optBalanceDtl.BalancePrice = optBalanceDtl.Details.Sum(a => a.BalancePrice);
                optBalanceDtl.BalacneQuantity = optBalanceDtl.Details.Average(a => a.BalanceQuantity);
                optBalanceDtl.BalanceTotalPrice = optBalanceDtl.Details.Sum(a => a.BalanceTotalPrice);

                if (AfterBalanceValueChanged != null)
                {
                    AfterBalanceValueChanged(optBalanceDtl);
                }
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
                if (optBalanceDtl != null)
                {
                    gridDetail.Rows.Clear();
                    foreach (SubContractBalanceSubjectDtl subject in optBalanceDtl.Details)
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

        private void AddAccountDetailSubjectInGrid(SubContractBalanceSubjectDtl dtl)
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

        private void UpdateAccountDetailSubjectInGrid(SubContractBalanceSubjectDtl dtl)
        {
            foreach (DataGridViewRow row in gridDetail.SelectedRows)
            {
                SubContractBalanceSubjectDtl subject = row.Tag as SubContractBalanceSubjectDtl;

                if (subject.Id == dtl.Id)
                {
                    row.Cells[DtlProjectTask.Name].Value = optBalanceDtl.BalanceTaskName;
                    row.Cells[DtlProjectTaskDetail.Name].Value = optBalanceDtl.BalanceTaskDtlName;
                    row.Cells[DtlCostName.Name].Value = dtl.CostName;
                    row.Cells[DtlBalanceSubject.Name].Value = dtl.BalanceSubjectName;

                    row.Cells[DtlResourceType.Name].Value = dtl.ResourceTypeName;
                    row.Cells[DtlResourceSpec.Name].Value = dtl.ResourceTypeSpec;
                    row.Cells[DtlResDiagramNumber.Name].Value = dtl.DiagramNumber;

                    row.Cells[DtlBalanceQuantity.Name].Value = dtl.BalanceQuantity;
                    row.Cells[DtlBalancePrice.Name].Value = dtl.BalancePrice;
                    row.Cells[DtlBalanceTotalPrice.Name].Value = dtl.BalanceTotalPrice;
                    row.Cells[DtlQuantityUnit.Name].Value = dtl.QuantityUnitName;
                    row.Cells[DtlPriceUnit.Name].Value = dtl.PriceUnitName;

                    row.Cells[DtlRemark.Name].Value = dtl.Remark;

                    row.Tag = dtl;

                    break;
                }
            }
        }

        //保存
        void btnSave_Click(object sender, EventArgs e)
        {
            SaveSubjectAccount();
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

            int index = gridDetail.SelectedRows[0].Index;
            gridDetail.ClearSelection();
            gridDetail.Rows[index].ReadOnly = false;
            gridDetail.Rows[index].Cells["DtlBalancePrice"].ReadOnly = false;
            gridDetail.CurrentCell = gridDetail.Rows[index].Cells[DtlBalancePrice.Name];
            gridDetail.BeginEdit(false);
        }

        private bool SaveSubjectAccount()
        {
            try
            {
                IList list = new ArrayList();
                List<int> listRowIndex = new List<int>();
                foreach (DataGridViewRow row in gridDetail.Rows)
                {
                    if (!row.ReadOnly)
                    {
                        list.Add(row.Tag as SubContractBalanceSubjectDtl);
                        listRowIndex.Add(row.Index);
                    }
                }

                if (list.Count > 0)
                {
                    if (!string.IsNullOrEmpty(optBalanceDtl.Id))
                    {
                        list = model.SaveOrUpdateSubContractBalanceBill(list);

                        for (int i = 0; i < listRowIndex.Count; i++)
                        {
                            int rowIndex = listRowIndex[i];
                            gridDetail.Rows[rowIndex].Tag = list[i];

                        }
                    }
                    optBalanceDtl.Details.Clear();
                    foreach (SubContractBalanceSubjectDtl subject in list)
                    {
                        subject.TheBalanceDetail = optBalanceDtl;
                        optBalanceDtl.Details.Add(subject);
                    }
                }

                //SetGridRowReadOnly(true);

                MessageBox.Show("保存成功！");
            }
            catch (Exception exp)
            {
                MessageBox.Show("保存失败，异常信息：" + ExceptionUtil.ExceptionMessage(exp));
                return false;
            }

            return true;
        }

    }
}
