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
using Application.Business.Erp.SupplyChain.ConcreteManage.ConcreteBalanceMng.Domain;
using VirtualMachine.Component.Util;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConBalanceMng.ConBalanceRedUI
{
    public partial class VConBalanceSelector : TBasicDataView
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

        public VConBalanceSelector()
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
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            lnkAll.Click += new EventHandler(lnkAll_Click);
            lnkNone.Click += new EventHandler(lnkNone_Click);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            dgDetail.CellValueChanged += new DataGridViewCellEventHandler(dgDetail_CellValueChanged);
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

        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            dgDetail.Rows.Clear();
            Clear();
            ConcreteBalanceMaster master = dgMaster.CurrentRow.Tag as ConcreteBalanceMaster;
            if (master == null) return;
            foreach (ConcreteBalanceDetail dtl in master.Details)
            {
                decimal canUseQuantity = decimal.Subtract(dtl.BalanceVolume, dtl.RefQuantity);
                if (canUseQuantity <= 0) continue;

                int rowIndex = dgDetail.Rows.Add();
                dgDetail.Rows[rowIndex].Tag = dtl;
                if (dtl.MaterialResource != null)
                {
                    dgDetail[colMaterialCode.Name, rowIndex].Value = dtl.MaterialResource.Code;
                    dgDetail[colMaterialName.Name, rowIndex].Value = dtl.MaterialResource.Name;
                    dgDetail[colSpec.Name, rowIndex].Value = dtl.MaterialResource.Specification;
                    dgDetail[colDescript.Name, rowIndex].Value = dtl.Descript;//备注
                    dgDetail[colPrice.Name, rowIndex].Value = dtl.Price;//单价
                    dgDetail[colUsedPart.Name, rowIndex].Value = dtl.UsedPartName;//使用部位
                    dgDetail[colConCheckQuantity.Name, rowIndex].Value = dtl.CheckingVolume;
                    dgDetail[colCanQuantity.Name, rowIndex].Value = dtl.BalanceVolume - dtl.RefQuantity;
                    if (dtl.IsPump == true)
                    {
                        dgDetail[colIsPump.Name, rowIndex].Value = true;
                    }
                    else
                    {
                        dgDetail[colIsPump.Name, rowIndex].Value = false;
                    }
                }
                dgDetail[colSelect.Name, rowIndex].Value = true;
            }
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
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
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
            if (txtHandlePerson.Text != "" && txtHandlePerson.Result.Count > 0)
            {
                oq.AddCriterion(Expression.Eq("HandlePerson", txtHandlePerson.Result[0] as PersonInfo));
            }
            oq.AddCriterion(Expression.Eq("DocState", VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InAudit));

            try
            {
                list = model.ConcreteMngSrv.GetConcreteBalanceMaster(oq);
                ShowMasterList(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ExceptionUtil.ExceptionMessage(ex));
            }
            if (dgMaster.Rows.Count > 0)
            {
                dgMaster.CurrentCell = dgMaster[1, 0];
                dgMaster_SelectionChanged(dgMaster, new EventArgs());
            }
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.result.Clear();
            this.dgDetail.EndEdit();
            if (dgMaster.SelectedRows.Count == 0)
            {
                MessageBox.Show("没有可以引用结算单！");
                return;
            }
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) return;
                if ((bool)var.Cells[colSelect.Name].Value)
                {
                    ConcreteBalanceDetail dtl = var.Tag as ConcreteBalanceDetail;
                    dtl.IsSelect = true;
                }
            }
            result.Add(this.dgMaster.SelectedRows[0].Tag);
            this.btnOK.FindForm().Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnCancel.FindForm().Close();
        }
        private void ShowMasterList(IList list)
        {
            dgMaster.Rows.Clear();
            if (list == null || list.Count == 0) return;
            foreach (ConcreteBalanceMaster master in list)
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
                dgMaster[colDgMasterSumQuantity.Name, rowIndex].Value = master.SumVolumeQuantity;//总数量
            }
            dgMaster.CurrentCell = dgMaster[1, 0];
            dgMaster_SelectionChanged(dgMaster, new EventArgs());
        }

        /// <summary>
        /// 判断主表是否有可引用数量
        /// </summary>
        /// <param name="master"></param>
        /// <returns></returns>
        private bool HasRefQuantity(ConcreteBalanceMaster master)
        {
            if (master == null || master.Details.Count == 0) return false;
            foreach (ConcreteBalanceDetail dtl in master.Details)
            {
                if (decimal.Subtract(dtl.BalanceVolume, dtl.RefQuantity) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void Clear()
        {
            lblRecordTotal.Text = string.Format("共选择【{0}】条记录", 0);
            txtSumQuantity.Text = "";
            totalRecords = 0;
            sumQuantity = 0;
        }
    }
}
