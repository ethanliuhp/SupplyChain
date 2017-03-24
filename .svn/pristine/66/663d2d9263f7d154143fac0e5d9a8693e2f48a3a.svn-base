using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;
namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder
{
    public partial class VMaterialHireOrderSelectMaterial : TBasicDataView
    {
        MMaterialHireMng model = new MMaterialHireMng();
        private int totalRecords = 0;
        private decimal sumQuantity = 0M;
        EnumMatHireType MatHireType;
        private IList result = new ArrayList();
        private bool IsProject = false;
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VMaterialHireOrderSelectMaterial(EnumMatHireType MatHireType)
        {
            InitializeComponent();
            InitEvent();
            this.MatHireType = MatHireType;
            InitForm();
        }

        private void InitForm()
        {
            this.Title = "料具租赁合同引用";
            dtpDateBegin.Value = ConstObject.LoginDate.AddDays(-7);
            dtpDateEnd.Value = ConstObject.LoginDate;
            txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode1;
            this.lnkAll.Visible = this.lnkNone.Visible = this.MatHireType == EnumMatHireType.普通料具;
             
        }

        private void InitEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
         this.customButton1.Click += new EventHandler(btnOK_Click);
         this.customButton2.Click += new EventHandler(btnCancel_Click);
            lnkAll.Click += new EventHandler(lnkAll_Click);
            lnkNone.Click += new EventHandler(lnkNone_Click);
            txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            dgDetail.CellValueChanged += new DataGridViewCellEventHandler(dgDetail_CellValueChanged);

            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);

            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            chkHide.CheckedChanged+=new EventHandler(chkHide_CheckedChanged);
        }
        public void chkHide_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow oRow in dgDetail.Rows)
            {
                if (oRow.Cells[colSelect.Name].ReadOnly)
                {
                    oRow.Visible = !chkHide.Checked;
                }
            }
        }
        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (dgDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colSelect.Name)
            {
                dgDetail.EndEdit();
            }
        }

        void dgDetail_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell cell = dgDetail[e.ColumnIndex, e.RowIndex];
            if (cell.OwningColumn.Name == colSelect.Name)
            {
                bool selected = (bool)cell.Value;
                if (selected)
                {
                    if (this.MatHireType != EnumMatHireType.普通料具 )
                    {
                        totalRecords = 0;
                        this.dgDetail.CellValueChanged -= new DataGridViewCellEventHandler(dgDetail_CellValueChanged);
                        List<DataGridViewCell> lstCell = dgDetail.Rows.OfType<DataGridViewRow>().
                            Where(a => a.Index != e.RowIndex && ClientUtil.ToBool(a.Cells[colSelect.Name].Value))
                            .Select(a => a.Cells[colSelect.Name]).ToList();
                        foreach (DataGridViewCell oCell in lstCell)
                        {
                            oCell.Value = false;
                        }
                        this.dgDetail.CellValueChanged += new DataGridViewCellEventHandler(dgDetail_CellValueChanged);
                    }
                    totalRecords += 1;
                   // sumQuantity += decimal.Parse(dgDetail[colQuantity.Name, e.RowIndex].Value.ToString());
                }
                else
                {
                    totalRecords -= 1;
                    //sumQuantity -= decimal.Parse(dgDetail[colQuantity.Name, e.RowIndex].Value.ToString());
                }
                lblRecordTotal.Text = string.Format("共选择【{0}】条记录", totalRecords);
               // txtSumQuantity.Text = sumQuantity.ToString();
            }
        }

        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            Hashtable htStockQty = null;
            string sToolTip = "";
            decimal dQuanitity = 0;
            dgDetail.Rows.Clear();
            Clear();
            MatHireOrderMaster master = null;//dgMaster.CurrentRow.Tag as MatHireOrderMaster;
            if (dgMaster.CurrentRow.Tag is MatHireOrderMaster)
            {
                master=dgMaster.CurrentRow.Tag as MatHireOrderMaster;
            }
            else{
                string sMasterId=dgMaster.CurrentRow.Tag as string;
                if (!string.IsNullOrEmpty(sMasterId))
                {
                    ObjectQuery oQuery = new ObjectQuery();
                    oQuery.AddCriterion(Expression.Eq("Id", sMasterId));
                    IList lst = model.MaterialHireMngSvr.GetMaterialHireOrder(oQuery);
                    if (lst != null && lst.Count > 0)
                    {
                        FilterData(lst);
                        master = lst[0] as MatHireOrderMaster;
                        dgMaster.CurrentRow.Tag = master;
                    }
                }
            }
            if (master == null) return;
             string sMaterialIDs=string.Join(",", master.TempData.OfType<MatHireOrderDetail>().Where(a => a.BasicDtlCostSets.Count > 0).Select(a => string.Format("'{0}'",a.MaterialResource.Id)).Distinct().ToArray());
              htStockQty =string.IsNullOrEmpty(sMaterialIDs)?null: model.MaterialHireMngSvr.GetPreviousJC(DateTime.MaxValue, sMaterialIDs, master.ProjectId, master.TheSupplierRelationInfo.Id);
            
            foreach (MatHireOrderDetail dtl in master.TempData)
            {
               // decimal canUseQuantity = decimal.Subtract(dtl.Quantity, dtl.RefQuantity);
                //if (canUseQuantity <= 0) continue;

                int rowIndex = dgDetail.Rows.Add();
                dgDetail.Rows[rowIndex].Tag = dtl;

                dgDetail[colDescript.Name, rowIndex].Value = dtl.Descript;
                if (dtl.MaterialResource != null)
                {
                    dgDetail[colMaterialCode.Name, rowIndex].Value = dtl.MaterialResource.Code;
                    dgDetail[colMaterialName.Name, rowIndex].Value = dtl.MaterialResource.Name;
                    dgDetail[colSpec.Name, rowIndex].Value = dtl.MaterialResource.Specification;
                    if (htStockQty != null && htStockQty.Count>0 && htStockQty.ContainsKey(dtl.MaterialResource.Id))
                    {
                        dQuanitity = ClientUtil.ToDecimal(htStockQty[dtl.MaterialResource.Id]);
                        dtl.TempData3 = dQuanitity.ToString();
                        dgDetail[this.colLeftQty.Name, rowIndex].Value = dQuanitity == 0 ? "" : dQuanitity.ToString("N2");
                    }
                }
                if (dtl.MatStandardUnit != null)
                {
                    dgDetail[colUnit.Name, rowIndex].Value = dtl.MatStandardUnit.Name;
                }
                dgDetail[colPrice.Name, rowIndex].Value = dtl.Price;
                //dgDetail[colQuantity.Name, rowIndex].Value = canUseQuantity;
                dgDetail[colSelect.Name, rowIndex].Value =this.MatHireType==EnumMatHireType.普通料具;
                if (dtl.BasicDtlCostSets.Count() == 0)//没有设置费用明细  价格、公式定义、保养及维修
                {
                    sToolTip=string.Format("编号为[{0}][{1}]物资未定义规则，无法进行发料", dtl.MaterialResource.Code, dtl.MaterialResource.Name);
                    dgDetail[colSelect.Name, rowIndex].ReadOnly = true;
                    dgDetail[colSelect.Name, rowIndex].Value = false;
                    foreach (DataGridViewColumn oColumn in dgDetail.Columns)
                    {
                        dgDetail[oColumn.Index, rowIndex].ToolTipText = sToolTip;
                    }
                    dgDetail.Rows[ rowIndex].Visible = !chkHide.Checked;
                }
                dgDetail[colDescript.Name, rowIndex].Value = dtl.Descript;
            }
            
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        /// <summary>
        /// 反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkNone_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (!var.Cells[colSelect.Name].ReadOnly)
                {
                    bool isSelected = (bool)var.Cells[colSelect.Name].Value;
                    var.Cells[colSelect.Name].Value = !isSelected;
                }
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (!var.Cells[colSelect.Name].ReadOnly)
                {
                    var.Cells[colSelect.Name].Value = true;
                }
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.customButton1.FindForm().Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.result.Clear();
            bool IsSelect = false;
            this.dgDetail.EndEdit();
            if (this.dgMaster.SelectedRows.Count == 0)
            {
                MessageBox.Show("没有可以引用的合同！");
                return;
            }

            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) break;
                if ((bool)var.Cells[colSelect.Name].Value)
                {
                    MatHireOrderDetail dtl = var.Tag as MatHireOrderDetail;
                    dtl.IsSelect = true;
                    IsSelect = true;
                    if (this.MatHireType == EnumMatHireType.钢管 || this.MatHireType == EnumMatHireType.碗扣)
                        break;
                }
            }
            if (IsSelect)
            {
                result.Add(this.dgMaster.SelectedRows[0].Tag);
            }
            else
            {
                if (MessageBox.Show("未选择对应物资信息,确认退出吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    return;
                }
            }
            this.customButton1.FindForm().Close();

        }

        private void Clear()
        {
            lblRecordTotal.Text = string.Format("共选择【{0}】条记录", 0);
           // txtSumQuantity.Text = "";
            totalRecords = 0;
            sumQuantity = 0;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            IList list = new ArrayList();
           
            oq.AddCriterion(Expression.Ge("CreateDate", dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", dtpDateEnd.Value.AddDays(1).Date));
            if (txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Between("Code", txtCodeBegin.Text, txtCodeEnd.Text));
            }
            if (txtCreatePerson.Text != "" && txtCreatePerson.Result.Count > 0)
            {
                oq.AddCriterion(Expression.Eq("CreatePerson", txtCreatePerson.Result[0] as PersonInfo));
            }
            //if (txtHandlePerson.Text != "" && txtHandlePerson.Result.Count > 0)
            //{
            //    oq.AddCriterion(Expression.Eq("HandlePerson", txtHandlePerson.Result[0] as PersonInfo));
            //}
            if (txtSupplier.Text != "" && txtSupplier.Result.Count > 0)
            {
                oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", txtSupplier.Result[0] as SupplierRelationInfo));
            }
            if (txtOriContractNo.Text != "")
            {
                oq.AddCriterion(Expression.Like("OriginalContractNo", txtOriContractNo.Text, MatchMode.Anywhere));
            }
            if (!string.IsNullOrEmpty(txtOldCode.Text))
            {
                oq.AddCriterion(Expression.Like("BillCode", txtOldCode.Text, MatchMode.Anywhere));
            }
            try
            {
                oq.AddFetchMode("TheSupplierRelationInfo",NHibernate.FetchMode.Eager);
                list = model.MaterialHireMngSvr.ObjectQuery(typeof(MatHireOrderMaster), oq); //model.MaterialHireMngSvr.GetMaterialHireOrder(oq);
                //list = FilterData(list);
                ShowMasterList(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" +ExceptionUtil.ExceptionMessage( ex));
            }
        }
        public IList FilterData(IList lstData)
        {
            bool isFind = false;
            MatHireOrderMaster oOrder = null;
            if (lstData != null)
            {
                for (int i = lstData.Count - 1; i > -1; i--)
                {
                    oOrder = lstData[i] as MatHireOrderMaster;
                    if (MatHireType == EnumMatHireType.普通料具)
                    {
                    oOrder.TempData= oOrder.Details.OfType<MatHireOrderDetail>().
                        Where(a => !(a.MaterialResource.Code.StartsWith(CommonUtil.MaterialCateGGCode) || a.MaterialResource.Code.StartsWith(CommonUtil.MaterialCateWKCode))).ToList();
                    }
                    else if (MatHireType == EnumMatHireType.钢管)
                    {
                        oOrder.TempData = oOrder.Details.OfType<MatHireOrderDetail>().
                            Where(a => a.MaterialResource.Code.StartsWith(CommonUtil.MaterialCateGGCode))
                            .ToList();
                    }
                    else if (MatHireType == EnumMatHireType.碗扣)
                    {
                        oOrder.TempData = oOrder.Details.OfType<MatHireOrderDetail>().
                            Where(a => a.MaterialResource.Code.StartsWith(CommonUtil.MaterialCateWKCode)).
                            ToList();
                    }
                    
                    //if (oOrder.TempData == null || oOrder.TempData.Count==0)
                    //{
                    //    lstData.Remove(oOrder);
                    //}
                }
            }
           return  lstData;
        }
        /// <summary>
        /// 显示主表
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList masterList)
        {
            dgMaster.Rows.Clear();
            if (masterList == null || masterList.Count == 0) return;
            foreach (MatHireOrderMaster master in masterList)
            {
                //if (!HasRefQuantity(master)) continue;
       
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master.Id; //master;
                dgMaster[colDgMasterCode.Name, rowIndex].Value = master.Code;
                dgMaster[colDgMasterCreateDate.Name, rowIndex].Value = master.CreateDate;
                dgMaster[colDgMasterCreatePerson.Name, rowIndex].Value = master.CreatePersonName;
                dgMaster[colDgMasterDescript.Name, rowIndex].Value = master.Descript;
                dgMaster[colDgMasterHandlePerson.Name, rowIndex].Value = master.HandlePersonName;
                dgMaster[colDgMasterOriContractNo.Name, rowIndex].Value = master.OriginalContractNo;
                dgMaster[colDgMasterState.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);
                //dgMaster[colDgMasterSumQuantity.Name, rowIndex].Value = master.SumQuantity;
                if (master.TheSupplierRelationInfo != null)
                {
                    dgMaster[colDgMasterSupply.Name, rowIndex].Value = master.TheSupplierRelationInfo.SupplierInfo.Name;
                }
                dgMaster[this.colProject.Name, rowIndex].Value = master.ProjectName;
                dgMaster[this.colBalRule.Name, rowIndex].Value = master.BalRule;
                dgMaster[this.colDgMasterDescript.Name, rowIndex].Value = master.Descript;
            }
            if (dgMaster.Rows.Count > 0)
            {
                dgMaster.CurrentCell = dgMaster[1, 0];
                dgMaster_SelectionChanged(dgMaster, new EventArgs());
            }
           
        }

        /// <summary>
        /// 判断主表是否有可引用数量
        /// </summary>
        /// <param name="master"></param>
        /// <returns></returns>
        private bool HasRefQuantity(MatHireOrderMaster master)
        {
            if (master == null || master.Details.Count == 0) return false;
            foreach (MatHireOrderDetail dtl in master.Details)
            {
                if (decimal.Subtract(dtl.Quantity, dtl.RefQuantity) > 0)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
