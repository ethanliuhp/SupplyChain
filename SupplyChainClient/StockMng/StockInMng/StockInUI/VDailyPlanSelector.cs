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
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI
{
    public partial class VDailyPlanSelector : TBasicDataView
    {
        MStockMng model = new MStockMng();
        private int totalRecords = 0;
        private decimal sumQuantity=0M;
        private SupplierRelationInfo supplierRelationInfo;
        //业务类型
         EnumStockExecType excuteType;
        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public SupplierRelationInfo SupplierRelationInfo
        {
            get { return supplierRelationInfo; }
            set { supplierRelationInfo = value; }
        }
        
        public VDailyPlanSelector()
        {
            InitializeComponent();
            InitEvent();
            InitForm();
            InitData();

            btnSearch.Focus();
        }
        public VDailyPlanSelector(EnumStockExecType excuteType)
        {
            this.excuteType = excuteType;
            InitializeComponent();
            InitEvent();
            InitForm();
            InitData();
            if (excuteType == EnumStockExecType.安装)
            {
                colSDConfirmPrice.Visible = true;
                colConfrimPrice.Visible = true;
            }
            else
            {
                colSDConfirmPrice.Visible = false ;
                colConfrimPrice.Visible = false;
            }
        }

        private void InitData()
        {
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
            Load += new EventHandler(VDailyPlanSelector_Load);
            dgSupplyOrderMasger.SelectionChanged += new EventHandler(dgSupplyOrderMasger_SelectionChanged);
            btnSetPrice.Click += new EventHandler(btnSetPrice_Click);

            //pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
        }

        void btnSetPrice_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow planDetailRow in dgDetail.Rows)
            {
                foreach (DataGridViewRow dr in dgSupplyOrderDetail.Rows)
                {
                    SupplyOrderDetail orderDetail = dr.Tag as SupplyOrderDetail;
                    Material material = dr.Cells[colSDMaterialCode.Name].Tag as Material;
                    object materialName = dr.Cells[colSDMaterialName.Name].Value;
                    object materialSpec = dr.Cells[colSDMaterialSpec.Name].Value;
                    object orderPrice = dr.Cells[colSDPrice.Name].Value;
                     //认价价格
                    object orderConfrimPrice = dr.Cells[colSDConfirmPrice.Name ].Value;

                    DailyPlanDetail planDetail = planDetailRow.Tag as DailyPlanDetail;
                    Material planMaterial = planDetailRow.Cells[colMaterialCode.Name].Tag as Material;
                    object planPrice = planDetailRow.Cells[colPrice.Name].Value;
                   
                    if (orderDetail.MaterialCode.StartsWith("01"))
                    {
                        planDetailRow.Cells[colPrice.Name].Value = 0;
                        planDetail.SupplyOrderDetail = orderDetail;
                    }
                    else
                    {
                        if (material.Id == planMaterial.Id && ClientUtil.ToString(orderDetail.DiagramNumber).Trim() == ClientUtil.ToString(planDetail.DiagramNumber).Trim())
                        {
                            if (planPrice == null || planPrice.ToString().Equals("") || planPrice == orderPrice)
                            {
                                planDetailRow.Cells[colPrice.Name].Value = orderPrice;
                                planDetailRow.Cells[colConfrimPrice.Name].Value = orderConfrimPrice;//认价价格
                                planDetail.SupplyOrderDetail = orderDetail;
                            }
                            else
                            {
                                if (DialogResult.Yes == MessageBox.Show(string.Format("是否把物资【{0} {1}】的价格【{2}】改为【{3}】？", materialName, materialSpec, planPrice, orderPrice), "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1))
                                {
                                    planDetailRow.Cells[colPrice.Name].Value = orderPrice;
                                    planDetailRow.Cells[colConfrimPrice.Name].Value = orderConfrimPrice;//认价价格
                                    planDetail.SupplyOrderDetail = orderDetail;
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

        void dgSupplyOrderMasger_SelectionChanged(object sender, EventArgs e)
        {
            SupplyOrderMaster orderMaster = dgSupplyOrderMasger.CurrentRow.Tag as SupplyOrderMaster;
            if (orderMaster == null) return;
            dgSupplyOrderDetail.Rows.Clear();
            foreach (SupplyOrderDetail orderDetail in orderMaster.Details)
            {
                int i = dgSupplyOrderDetail.Rows.Add();
                dgSupplyOrderDetail.Rows[i].Tag = orderDetail;
                dgSupplyOrderDetail[colSDMaterialCode.Name, i].Value = orderDetail.MaterialCode;
                dgSupplyOrderDetail[colSDMaterialCode.Name, i].Tag = orderDetail.MaterialResource;
                dgSupplyOrderDetail[colSDMaterialName.Name, i].Value = orderDetail.MaterialName;
                dgSupplyOrderDetail[colSDMaterialSpec.Name, i].Value = orderDetail.MaterialSpec;
                dgSupplyOrderDetail[colSDQty.Name, i].Value = orderDetail.Quantity;
                //dgSupplyOrderDetail[colSDPrice.Name, i].Value = orderDetail.SupplyPrice.ToString("###########.##");
                dgSupplyOrderDetail[colSDPrice.Name, i].Value = orderDetail.ModifyPrice;
                 //确认价格
                dgSupplyOrderDetail[colSDConfirmPrice.Name, i].Value = orderDetail.ConfirmPrice;
                
                dgSupplyOrderDetail[colSDBrand.Name, i].Value = orderDetail.Brand;
                dgSupplyOrderDetail[colDiagramNumber.Name, i].Value = orderDetail.DiagramNumber;
            }
        }

        void VDailyPlanSelector_Load(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            try
            {
                //本项目合同
                oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", supplierRelationInfo));
                oq.AddCriterion(Expression.Eq("ProjectId", StaticMethod.GetProjectInfo().Id));
                if (this.excuteType == EnumStockExecType.安装)
                {
                    oq.AddCriterion(Expression.Eq("Special", "安装"));
                }
                else if (this.excuteType == EnumStockExecType.土建)
                {
                    oq.AddCriterion(Expression.Eq("Special", "土建"));
                }
                IList orderList = model.StockInSrv.GetSupplyOrderMaster(oq);

                //公司合同
                oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Master.TheSupplierRelationInfo", supplierRelationInfo));
                //oq.AddCriterion(Expression.Sql("{alias}.ProjectDetails.ProjectId='"+StaticMethod.GetProjectInfo().Id+"'"));
                oq.AddCriterion(Expression.Eq("ProjectId.Id", StaticMethod.GetProjectInfo().Id));
                oq.AddFetchMode("Master", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("Master.Details", NHibernate.FetchMode.Eager);

                List<SupplyOrderMaster> lst = new List<SupplyOrderMaster>();
                IList orderListCompany = model.StockInSrv.GetDomainByCondition(typeof(SupplyOrderProjectDetail),oq);
                if (orderListCompany != null && orderListCompany.Count > 0)
                {
                    foreach (SupplyOrderProjectDetail detail in orderListCompany)
                    {
                        if (!lst.Contains(detail.Master))
                        {
                            lst.Add(detail.Master);
                        }
                    }
                }               

                if (orderList != null || orderList.Count > 0)
                {
                    ShowSupplyOrderMaster(orderList);
                }

                if (lst.Count > 0)
                {
                    ShowSupplyOrderMaster(lst);
                }

                if (dgSupplyOrderMasger.Rows.Count > 0)
                {
                    dgSupplyOrderMasger.CurrentCell = dgSupplyOrderMasger[1, 0];
                    dgSupplyOrderMasger_SelectionChanged(dgSupplyOrderMasger, new EventArgs());
                }
            } catch (Exception ex)
            {
                MessageBox.Show("查询采购合同出错。\n"+ex.Message);
            }
        }

        private void ShowSupplyOrderMaster(IList list)
        {
            foreach (SupplyOrderMaster supplyOrderMaster in list)
            {
                int i = dgSupplyOrderMasger.Rows.Add();
                dgSupplyOrderMasger.Rows[i].Tag = supplyOrderMaster;
                dgSupplyOrderMasger[colSMCode.Name, i].Value = supplyOrderMaster.Code;
                dgSupplyOrderMasger[colSMOriginalContractNo.Name, i].Value = supplyOrderMaster.OldContractNum;
                dgSupplyOrderMasger[colSMSupplier.Name, i].Value = supplyOrderMaster.SupplierName;
                dgSupplyOrderMasger[colSMCreateDate.Name, i].Value = supplyOrderMaster.CreateDate.ToShortDateString();
                dgSupplyOrderMasger[colSMCreatePerson.Name, i].Value = supplyOrderMaster.CreatePersonName;
                dgSupplyOrderMasger[colSMOperator.Name, i].Value = supplyOrderMaster.HandlePersonName;
                dgSupplyOrderMasger[colSMSumQty.Name, i].Value = supplyOrderMaster.SumQuantity;
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
                } else
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
            DailyPlanMaster master = dgMaster.CurrentRow.Tag as DailyPlanMaster;
            if (master == null) return;
            foreach (DailyPlanDetail dtl in master.Details)
            {
                decimal canUseQuantity = decimal.Subtract(dtl.Quantity, dtl.RefQuantity);
                if (canUseQuantity <= 0) continue;

                int rowIndex = dgDetail.Rows.Add();
                dgDetail.Rows[rowIndex].Tag = dtl;

                dgDetail[colMaterialCode.Name, rowIndex].Value = dtl.MaterialCode;
                dgDetail[colMaterialCode.Name, rowIndex].Tag = dtl.MaterialResource;
                dgDetail[colMaterialName.Name, rowIndex].Value = dtl.MaterialName;
                dgDetail[colSpec.Name, rowIndex].Value = dtl.MaterialSpec;
                //dgDetail[colUnit.Name, rowIndex].Value = dtl.MatStandardUnitName;                
                dgDetail[colQuantity.Name, rowIndex].Value = canUseQuantity;
                //dgDetail[colPrice.Name, rowIndex].Value = ;
                dgDetail[colDescript.Name, rowIndex].Value = dtl.Descript;
                dgDetail[colSelect.Name, rowIndex].Value = true;
                dgDetail[colDiagramNum.Name, rowIndex].Value = dtl.DiagramNumber;
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
                MessageBox.Show("没有可以引用的日常需求计划！");
                return;
            }
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) break;
                if ((bool)var.Cells[colSelect.Name].Value)
                {
                    //if (!HasPrice(var)) return;
                    DailyPlanDetail dtl = var.Tag as DailyPlanDetail;
                    if (dtl.SupplyOrderDetail == null)
                    {                        
                        MessageBox.Show(string.Format("物资【{0} {1}】未关联合同，请先关联合同。",var.Cells[colMaterialName.Name].Value,var.Cells[colSpec.Name].Value));
                        return;
                    }
                    dtl.IsSelect = true;
                    object priceO = var.Cells[colPrice.Name].Value;
                    if (priceO == null || priceO.ToString().Equals(""))
                    {
                        dtl.Price = 0;
                    }
                    else
                    {
                        dtl.Price = decimal.Parse(var.Cells[colPrice.Name].Value.ToString());
                         
                    }
                    if (this.excuteType == EnumStockExecType.安装)
                    {
                        dtl.TempData = ClientUtil.ToString(var.Cells[colConfrimPrice.Name].Value);
                    }
                }               
            }
            result.Add(this.dgMaster.SelectedRows[0].Tag);
            result.Add(this.dgSupplyOrderMasger.SelectedRows[0].Tag);
            this.btnOK.FindForm().Close();
        }

        private bool HasPrice(DataGridViewRow var)
        {
            object price = var.Cells[colPrice.Name].Value;
            object matName = var.Cells[colMaterialName.Name].Value;
            object matSpec = var.Cells[colSpec.Name].Value;
            if (price == null || price.ToString() == "")
            {
                MessageBox.Show(string.Format("物资【{0} {1}】采购单价为空。", matName, matSpec));
                dgDetail.CurrentCell = var.Cells[colPrice.Name];
                return false;
            }
            if (decimal.Parse(price.ToString()) <= 0)
            {
                MessageBox.Show(string.Format("物资【{0} {1}】采购单价不能为0。", matName, matSpec));
                dgDetail.CurrentCell = var.Cells[colPrice.Name];
                return false;
            }
            return true;
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
            oq.AddCriterion(Expression.Ge("CreateDate",dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", dtpDateEnd.Value.AddDays(1).Date));
            if (txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Between("Code", txtCodeBegin.Text, txtCodeEnd.Text));
            }
            if (this.excuteType == EnumStockExecType.安装)
            {
                oq.AddCriterion (Expression.Eq ("Special","安装"));

            }
            else if (this.excuteType == EnumStockExecType.土建 )
            {
                oq.AddCriterion(Expression.Eq("Special", "土建"));
            }
            if (txtCreatePerson.Text != "" && txtCreatePerson.Result.Count > 0)
            {
                oq.AddCriterion(Expression.Eq("CreatePerson", txtCreatePerson.Result[0] as PersonInfo));
            }

            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            oq.AddCriterion(Expression.Eq("ProjectId", StaticMethod.GetProjectInfo().Id));
            try
            {
                oq.AddFetchMode("MaterialCategory", NHibernate.FetchMode.Eager);
                list = model.StockInSrv.GetDailyPLanMaster(oq);
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
            dgMaster.Rows.Clear();
            if (masterList == null || masterList.Count == 0) return;
            foreach (DailyPlanMaster master in masterList)
            {
                if (!HasRefQuantity(master)) continue;

                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colDgMasterCode.Name, rowIndex].Value = master.Code;
                dgMaster[colDgMasterCreateDate.Name, rowIndex].Value = master.CreateDate.ToShortDateString();
                dgMaster[colDgMasterCreatePerson.Name, rowIndex].Value = master.CreatePersonName;
                dgMaster[colDgMasterHandlePerson.Name, rowIndex].Value = master.HandlePersonName;                         
                dgMaster[colDgMasterSumQuantity.Name, rowIndex].Value = master.SumQuantity;
                dgMaster[colDgMasterSumMoney.Name, rowIndex].Value = master.SumMoney;                
            }
            if (dgMaster.Rows.Count == 0)
            {
                MessageBox.Show("没有符合条件的日常需求计划。");
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
        private bool HasRefQuantity(BaseMaster master)
        {
            if (master == null || master.Details.Count == 0) return false;
            foreach (BaseDetail dtl in master.Details)
            {
                if (decimal.Subtract(dtl.Quantity, dtl.RefQuantity)>0)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
