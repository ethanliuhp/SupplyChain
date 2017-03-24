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
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyPlanMng
{
    public partial class VSupplyPlanSelector : TBasicDataView
    {
        MSupplyPlanMng model = new MSupplyPlanMng();
        private int totalRecords = 0;
        private decimal sumQuantity = 0M;

        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VSupplyPlanSelector()
        {
            InitializeComponent();
            InitEvent();
            InitForm();
        }

        private void InitForm()
        {
            this.Title = "采购计划单引用";
            dtpDateBegin.Value = ConstObject.LoginDate.AddDays(-7);
            dtpDateEnd.Value = ConstObject.LoginDate;
        }

        private void InitEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnOK1.Click += new EventHandler(btnOK1_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            lnkAll.Click += new EventHandler(lnkAll_Click);
            lnkNone.Click += new EventHandler(lnkNone_Click);
            txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            dgDetail.CellValueChanged += new DataGridViewCellEventHandler(dgDetail_CellValueChanged);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);

            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
        }

        void btnOK1_Click(object sender, EventArgs e)
        {
            this.result.Clear();
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
                    SupplyPlanDetail dtl = var.Tag as SupplyPlanDetail;
                    decimal dQuoteQuantity = 0;
                    dQuoteQuantity = ClientUtil.ToDecimal(var.Cells[colQuoteQuantity.Name].Value);
                    if (dQuoteQuantity <= dtl.LeftQuantity)
                    {
                        dtl.TempData2  = dQuoteQuantity.ToString ();
                       
                    }
                    else
                    {
                        MessageBox.Show("输入的可引用数量应该小于剩余数量");
                        var.Cells[colQuoteQuantity.Name].Selected = true;
                    }
                    dtl.IsSelect = true;
                }
            }
            result.Add(this.dgMaster.SelectedRows[0].Tag);
            this.btnOK1.FindForm().Close();
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
            //throw new NotImplementedException();
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell cell = dgDetail[e.ColumnIndex, e.RowIndex];
            if (cell.OwningColumn.Name == colSelect.Name)
            {
                bool selected = (bool)cell.Value;
                if (selected)
                {
                    totalRecords += 1;
                    sumQuantity += decimal.Parse(dgDetail[colQuantity.Name, e.RowIndex].Value.ToString());
                }
                else
                {
                    totalRecords -= 1;
                    sumQuantity -= decimal.Parse(dgDetail[colQuantity.Name, e.RowIndex].Value.ToString());
                }
                lblRecordTotal.Text = string.Format("共选择【{0}】条记录", totalRecords);
                txtSumQuantity.Text = sumQuantity.ToString();
            }
        }

        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            dgDetail.Rows.Clear();
            Clear();
            SupplyPlanMaster master = dgMaster.CurrentRow.Tag as SupplyPlanMaster;
            if (master == null) return;
            foreach (SupplyPlanDetail dtl in master.Details)
            {
                decimal canUseQuantity = decimal.Subtract(dtl.Quantity, dtl.RefQuantity);
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
                    dgDetail[colzyfl.Name, rowIndex].Value = dtl.SpecialType;//专业分类
                    //dgDetail[colQuoteQuantity.Name, rowIndex].Value = dtl.RefQuantity;//引用数量
                    dgDetail[colQuoteQuantity.Name, rowIndex].Value = dtl.LeftQuantity;
                    //dgDetail[colUsedPart.Name, rowIndex].Value = dtl.UsedPartName;//使用部位
                    //dgDetail[colQuoteQuantity.Name, rowIndex].Value = dtl.DemandLeftQuantity;//引用数量
                    dgDetail[colTechnicalParameters.Name, rowIndex].Value = dtl.TechnologyParameter;
                     
                }
                dgDetail[this.colDiagramNum.Name, rowIndex].Value = dtl.DiagramNumber;
                dgDetail[colQuantity.Name, rowIndex].Value = canUseQuantity;
                dgDetail[colSelect.Name, rowIndex].Value = true;
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

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnCancel.FindForm().Close();
        }

        private void Clear()
        {
            lblRecordTotal.Text = string.Format("共选择【{0}】条记录", 0);
            txtSumQuantity.Text = "";
            totalRecords = 0;
            sumQuantity = 0;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            IList list = new ArrayList();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("ProjectName", projectInfo.Name));
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
            try
            {
                list = model.SupplyPlanSrv.GetSupplyPlan(oq);
                ShowMasterList(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
        }

        /// <summary>
        /// 显示主表
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList masterList)
        {
            dgMaster.Rows.Clear();
            if (masterList == null || masterList.Count == 0) return;
            foreach (SupplyPlanMaster master in masterList)
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
                //dgMaster[colMoney.Name, rowIndex].Value = master.SumMoney;//总金额
                dgMaster[colDgMasterState.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);//状态
                dgMaster[colDgMasterSumQuantity.Name, rowIndex].Value = master.SumQuantity;//总数量
            }
            if (dgMaster != null && dgMaster.Rows.Count > 0)
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
        private bool HasRefQuantity(SupplyPlanMaster master)
        {
            if (master == null || master.Details.Count == 0) return false;
            foreach (SupplyPlanDetail dtl in master.Details)
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
