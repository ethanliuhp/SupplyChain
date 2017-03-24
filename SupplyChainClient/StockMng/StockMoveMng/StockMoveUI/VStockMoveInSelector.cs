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
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain;
using AuthManagerLib.AuthMng.AuthConfigMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI
{
    public partial class VStockMoveInSelector : TBasicDataView
    {
        MStockMng model = new MStockMng();
        private int totalRecords = 0;
        private decimal sumQuantity=0M,sumMoney=0M;
        private EnumStockExecType execType;
        
        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VStockMoveInSelector(EnumStockExecType execType)
        {
            this.execType = execType;
            InitializeComponent();
            InitEvent();
            InitForm();            
        }

        private void InitForm()
        {
            dtpDateBegin.Value = ConstObject.LoginDate.AddDays(-7);
            dtpDateEnd.Value = ConstObject.LoginDate;
        }

        private void InitEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            lnkAll.Click += new EventHandler(lnkAll_Click);
            lnkNone.Click += new EventHandler(lnkNone_Click);
            txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            dgDetail.CellValueChanged += new DataGridViewCellEventHandler(dgDetail_CellValueChanged);

            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            dgDetail.SelectionChanged += new EventHandler(dgDetail_SelectionChanged);

        }

        void dgDetail_SelectionChanged(object sender, EventArgs e)
        {
            StockMoveInDtl stockInDtl = dgDetail.CurrentRow.Tag as StockMoveInDtl;
            dgStockOut.Rows.Clear();
            if (stockInDtl == null) return;
            try
            {
                DataSet dataSet = model.StockOutSrv.QueryStockOutDtl(stockInDtl.Id);
                if (dataSet == null || dataSet.Tables.Count==0) return;
                DataTable dt = dataSet.Tables[0];
                if (dt == null || dt.Rows.Count == 0) return;

                foreach (DataRow dr in dt.Rows)
                {
                    int rowIndex = dgStockOut.Rows.Add();
                    dgStockOut[colDgStockOutCode.Name, rowIndex].Value = dr["Code"];
                    dgStockOut[colDgStockOutPrice.Name, rowIndex].Value = dr["Price"];
                    dgStockOut[colDgStockOutQuantity.Name, rowIndex].Value = dr["Quantity"];
                    dgStockOut[colDgStockOutRedQuantity.Name, rowIndex].Value = dr["RefQuantity"];
                }
            } catch (Exception ex)
            {
                MessageBox.Show("根据入库明细查询出库明细出错。"+ex.Message);
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
                    //sumMoney += decimal.Parse(dgDetail[colMoney.Name, e.RowIndex].Value.ToString());
                } else
                {
                    totalRecords -= 1;
                    sumQuantity -= decimal.Parse(dgDetail[colQuantity.Name, e.RowIndex].Value.ToString());
                    //sumMoney -= decimal.Parse(dgDetail[colMoney.Name, e.RowIndex].Value.ToString());
                }
                lblRecordTotal.Text = string.Format("共选择【{0}】条记录", totalRecords);
                txtSumQuantity.Text = sumQuantity.ToString("##########.####");
                txtSumMoney.Text = sumMoney.ToString("##########.##");
            }
        }

        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            dgDetail.Rows.Clear();
            Clear();
            StockMoveIn master = dgMaster.CurrentRow.Tag as StockMoveIn;
            if (master == null) return;
            foreach (StockMoveInDtl dtl in master.Details)
            {
                decimal canUseQuantity = dtl.Quantity - dtl.RefQuantity;
                if (canUseQuantity <= 0) continue;

                int rowIndex = dgDetail.Rows.Add();
                dgDetail.Rows[rowIndex].Tag = dtl;

                //dgDetail[colDescript.Name, rowIndex].Value = dtl.Descript;
                dgDetail[colMaterialCode.Name, rowIndex].Value = dtl.MaterialCode;
                dgDetail[colMaterialName.Name, rowIndex].Value = dtl.MaterialName;
                dgDetail[colSpec.Name, rowIndex].Value = dtl.MaterialSpec;
                //dgDetail[colUnit.Name, rowIndex].Value = dtl.MatStandardUnitName;
                dgDetail[colPrice.Name, rowIndex].Value = dtl.Price.ToString("##########.########");
                dgDetail[colQuantity.Name, rowIndex].Value = canUseQuantity.ToString("##########.####");
                dgDetail[this.colDiagramNum.Name, rowIndex].Value = dtl.DiagramNumber;
                try
                {
                    decimal remainQty = model.StockRelationSrv.GetRemainQuantityByStockInDtlId(dtl.Id);
                    if (remainQty == 0)
                    {
                        dgDetail[colStockQuantity.Name, rowIndex].Value = "0";
                    } else
                    {
                        dgDetail[colStockQuantity.Name, rowIndex].Value = remainQty.ToString("##########.####");
                    }
                } catch (Exception ex)
                {
                    MessageBox.Show(string.Format("查询物资【{0} {1}】库存出错。",dtl.MaterialName,dtl.MaterialSpec));
                    dgDetail[colStockQuantity.Name, rowIndex].Value = "0";
                }
                dgDetail[colRedQuantity.Name, rowIndex].Value = (dtl.RefQuantity == 0) ? "0" : dtl.RefQuantity.ToString("##########.####");
                dgDetail[colCanUseQuantity.Name, rowIndex].Value = (dtl.Quantity - dtl.RefQuantity).ToString("##########.####");

                //dgDetail[colMoney.Name, rowIndex].Value = (dtl.Price * canUseQuantity).ToString("##########.##");
                dgDetail[colSelect.Name, rowIndex].Value = true;
            }

            dgDetail.CurrentCell = dgDetail[1, 0];
            dgDetail_SelectionChanged(dgDetail, new EventArgs());
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
                bool isSelected=(bool)var.Cells[colSelect.Name].Value;
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
            this.btnOK.FindForm().Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.result.Clear();
            this.dgDetail.EndEdit();
            if (this.dgMaster.SelectedRows.Count == 0)
            {
                MessageBox.Show("没有可以引用的入库单！");
                return;
            }
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) break;
                if ((bool)var.Cells[colSelect.Name].Value)
                {
                    StockMoveInDtl dtl = var.Tag as StockMoveInDtl;
                    dtl.IsSelect = true;
                }               
            }
            result.Add(this.dgMaster.SelectedRows[0].Tag);
            this.btnOK.FindForm().Close();
        }

        private void Clear()
        {
            lblRecordTotal.Text = string.Format("共选择【{0}】条记录", 0);
            txtSumQuantity.Text = "";
            txtSumMoney.Text = "";
            totalRecords = 0;
            sumQuantity = 0;
            sumMoney = 0;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            dgMaster.Rows.Clear();
            dgDetail.Rows.Clear();
            dgStockOut.Rows.Clear();
            
            ObjectQuery oq = new ObjectQuery();
            IList list = new ArrayList();
            oq.AddCriterion(Expression.Ge("CreateDate",dtpDateBegin.Value.Date));
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
                oq.AddCriterion(Expression.Eq("HandlePerson",txtHandlePerson.Result[0] as PersonInfo));
            }
            if (txtMoveOutProjectName.Text != "")
            {
                oq.AddCriterion(Expression.Like("MoveOutProjectName", txtMoveOutProjectName.Text, MatchMode.Anywhere));
            }
            if (chkMaterialProvider.Checked)
            {
                oq.AddCriterion(Expression.Eq("MaterialProvider", 1));
            }
            
            oq.AddCriterion(Expression.Eq("Special", Enum.GetName(typeof(EnumStockExecType), execType)));
            //区分项目
            oq.AddCriterion(Expression.Eq("ProjectId", StaticMethod.GetProjectInfo().Id));
            oq.AddCriterion(Expression.Eq("IsTally", 1));
            try
            {
                list = model.StockMoveSrv.GetStockMoveIn(oq);
                ShowMasterList(list);
            } catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n"+ex.Message);
            }
        }

        /// <summary>
        /// 显示主表
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList masterList)
        {            
            if (masterList == null || masterList.Count == 0)
            {
                MessageBox.Show("未查询到数据。");
                return;
            }
            foreach (StockMoveIn master in masterList)
            {
                if (!HasRefQuantity(master)) continue;

                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colDgMasterCode.Name, rowIndex].Value = master.Code;
                dgMaster[colDgMasterCreateDate.Name, rowIndex].Value = master.CreateDate;
                dgMaster[colDgMasterCreatePerson.Name, rowIndex].Value = master.CreatePersonName;
                dgMaster[colDgMasterDescript.Name, rowIndex].Value = master.Descript;
                dgMaster[colDgMasterHandlePerson.Name, rowIndex].Value = master.HandlePersonName;

                dgMaster[colDgMasterState.Name, rowIndex].Value =ClientUtil.GetDocStateName( master.DocState);
                dgMaster[colDgMasterSumQuantity.Name, rowIndex].Value = master.SumQuantity;
                dgMaster[colDgMasterSumMoney.Name, rowIndex].Value = master.SumMoney;

                dgMaster[colDgMasterProjectName.Name, rowIndex].Value = master.MoveOutProjectName;
                dgMaster[colDgMasterProvider.Name, rowIndex].Value = (master.MaterialProvider == 1) ? "是" : "否";
            }
            if (dgMaster.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据。");
                return;
            }
            dgMaster.CurrentCell = dgMaster[1, 0];
            dgMaster_SelectionChanged(dgMaster, new EventArgs());
        }

        /// <summary>
        /// 判断主表是否有可引用数量
        /// </summary>
        /// <param name="master"></param>
        /// <returns></returns>
        private bool HasRefQuantity(StockMoveIn master)
        {
            if (master == null || master.Details.Count == 0) return false;
            foreach (StockMoveInDtl dtl in master.Details)
            {
                if ((dtl.Quantity - dtl.RefQuantity) > 0)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
