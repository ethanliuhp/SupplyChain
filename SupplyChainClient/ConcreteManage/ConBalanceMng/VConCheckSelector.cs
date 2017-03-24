using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using Application.Business.Erp.SupplyChain.ConcreteManage.ConcreteCheckingMng.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.ConcreteManage.ConcreteBalanceMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConBalanceMng
{
    public partial class VConCheckSelector : TBasicDataView
    {
        MConcreteMng model = new MConcreteMng();
        private int totalRecords = 0;
        private decimal sumQuantity = 0;

        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }
        public VConCheckSelector()
        {
            InitializeComponent();
            InitData();
            InitEvent();
        }

        private void InitData()
        {
            dtpDateBegin.Value = ConstObject.LoginDate.AddDays(-7);
            dtpDateEnd.Value = ConstObject.LoginDate;
        }

        private void InitEvent()
        {
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            this.lnkAll.Click += new EventHandler(lnkAll_Click);
            this.lnkNone.Click += new EventHandler(lnkNone_Click);
            this.dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            this.dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            this.dgDetail.CellValueChanged += new DataGridViewCellEventHandler(dgDetail_CellValueChanged);
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
                    totalRecords += 1;
                    sumQuantity += decimal.Parse(dgDetail[colCanQuantity.Name, e.RowIndex].Value.ToString());
                }
                else
                {
                    totalRecords -= 1;
                    sumQuantity -= decimal.Parse(dgDetail[colCanQuantity.Name, e.RowIndex].Value.ToString());
                }
                lblRecordTotal.Text = string.Format("共选择【{0}】条记录", totalRecords);
                txtSumQuantity.Text = sumQuantity.ToString();
            }
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (dgDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colSelect.Name)
            {
                dgDetail.EndEdit();
            }
        }
        bool  IsAddCostDetial(ConcreteCheckingDetail dtl)
        {
            if (dtl.BalVolume == 0 && dtl.Money != 0)
            {
                ConcreteBalanceDetail oConcreteBalanceDetail = model.ConcreteMngSrv.GetConcreteBalanceDetailByConcreteCheckingDetailID(dtl.Id);
                if (oConcreteBalanceDetail == null)
                {
                    return true;
                }
            }
            return false;
        }
        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            dgDetail.Rows.Clear();
            Clear();
            ConcreteCheckingMaster master = dgMaster.CurrentRow.Tag as ConcreteCheckingMaster;
            if (master == null) return;
            foreach (ConcreteCheckingDetail dtl in master.Details)
            {
                decimal canUseQuantity = decimal.Subtract(dtl.BalVolume, dtl.RefQuantity);
                if (IsAddCostDetial(dtl))
                {
                    dtl.TempData = "1";  
                }
                else if (dtl.BalVolume == 0 || (dtl.BalVolume > 0 && canUseQuantity <= 0) || (dtl.BalVolume < 0 && canUseQuantity >= 0))
                {
                    continue;
                }

                int rowIndex = dgDetail.Rows.Add();
                dgDetail.Rows[rowIndex].Tag = dtl;
                if (dtl.MaterialResource != null)
                {
                    dgDetail[colMaterialCode.Name, rowIndex].Tag = dtl.MaterialResource;
                    dgDetail[colMaterialCode.Name, rowIndex].Value = dtl.MaterialResource.Code;
                    dgDetail[colMaterialName.Name, rowIndex].Value = dtl.MaterialResource.Name;
                    dgDetail[colSpec.Name, rowIndex].Value = dtl.MaterialResource.Specification;
                    dgDetail[colDescript.Name, rowIndex].Value = dtl.Descript;//备注
                    dgDetail[colPrice.Name, rowIndex].Value = dtl.Price;//单价
                    dgDetail[colUsedPart.Name, rowIndex].Value = dtl.UsedPartName;//使用部位
                    dgDetail[colSubjectName.Name, rowIndex].Value = dtl.SubjectName;//核算科目
                    dgDetail[colCanQuantity.Name, rowIndex].Value = dtl.BalVolume - dtl.RefQuantity;//引用数量
                    if (dtl.IsPump == true)
                    {
                        dgDetail[colIsPump.Name, rowIndex].Value = true;
                    }
                    else
                    {
                        dgDetail[colIsPump.Name, rowIndex].Value = false;
                    }
                }
                dgDetail[colConCheckQuantity.Name, rowIndex].Value = dtl.BalVolume;
                dgDetail[colSelect.Name, rowIndex].Value = true;
            }
        }

        private void Clear()
        {
            lblRecordTotal.Text = string.Format("共选择【{0}】条记录", 0);
            txtSumQuantity.Text = "";
            totalRecords = 0;
            sumQuantity = 0;
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
                bool isSelected = (bool)var.Cells[colSelect.Name].Value;
                var.Cells[colSelect.Name].Value = !isSelected;
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
                var.Cells[colSelect.Name].Value = true;
            }
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeBegin.Text = this.txtCodeEnd.Text;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearch_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            IList list = new ArrayList();
            CurrentProjectInfo ProjectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));
            oq.AddFetchMode("Details.SubjectGUID", NHibernate.FetchMode.Eager);
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
            if (txtHandlePerson.Text != "" && txtHandlePerson.Result.Count > 0)
            {
                oq.AddCriterion(Expression.Eq("HandlePerson", txtHandlePerson.Result[0] as PersonInfo));
            }
            //oq.AddCriterion(Expression.Eq("DocState", VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute));

            try
            {
                list = model.ConcreteMngSrv.GetConCheckingMaster(oq);
                ShowMasterList(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ExceptionUtil.ExceptionMessage(ex));
            }
            if (dgMaster.RowCount > 0)
            {
                dgMaster.CurrentCell = dgMaster[1, 0];
                dgMaster_SelectionChanged(dgMaster, new EventArgs());
            }

        }

        private void ShowMasterList(IList list)
        {
            dgMaster.Rows.Clear();
            if (list == null || list.Count == 0) return;
            foreach (ConcreteCheckingMaster master in list)
            {
                if (!HasRefQuantity(master)) continue;

                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colDgMasterCode.Name, rowIndex].Value = master.Code;
                string[] sArray = master.CreateDate.ToString().Split(' ');//制单时间
                string stra = sArray[0];
                dgMaster[colDgMasterCreateDate.Name, rowIndex].Value = stra;
                dgMaster[colDgMasterCreatePerson.Name, rowIndex].Value = master.CreatePersonName;//制单人
                dgMaster[colDgMasterDescript.Name, rowIndex].Value = master.Descript;//备注
                dgMaster[colHandlePerson.Name, rowIndex].Value = master.HandlePersonName;//负责人
                dgMaster[colMoney.Name, rowIndex].Value = master.SumMoney;//总金额
                dgMaster[colDgMasterState.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);//状态
                dgMaster[colDgMasterSumQuantity.Name, rowIndex].Value = master.SumVolume;//总数量
            }
            if (dgMaster.RowCount > 0)
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
        private bool HasRefQuantity(ConcreteCheckingMaster master)
        {
            if (master == null || master.Details.Count == 0) return false;
            foreach (ConcreteCheckingDetail dtl in master.Details)
            {
                if (dtl.BalVolume == 0&&dtl .Money!=0&& model .ConcreteMngSrv.GetConcreteBalanceDetailByConcreteCheckingDetailID (dtl .Id)==null)
                {
                    return true;
                }
                if (dtl.BalVolume > 0)
                {
                    if (decimal.Subtract(dtl.BalVolume, dtl.RefQuantity) > 0)
                    {
                        return true;
                    }
                }
                else {
                    if (decimal.Subtract(dtl.BalVolume, dtl.RefQuantity) < 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnOK_Click(object sender, EventArgs e)
        {
            this.result.Clear();
            this.dgDetail.EndEdit();
            if (dgMaster.SelectedRows.Count == 0)
            {
                MessageBox.Show("没有可以引用的对账单！");
                return;
            }
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) return;
                if ((bool)var.Cells[colSelect.Name].Value)
                {
                    ConcreteCheckingDetail dtl = var.Tag as ConcreteCheckingDetail;
                    dtl.IsSelect = true;
                }
            }
            result.Add(this.dgMaster.SelectedRows[0].Tag);
            this.btnOK.FindForm().Close();
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
