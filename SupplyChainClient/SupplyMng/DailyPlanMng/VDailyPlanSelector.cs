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
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.DailyPlanMng
{
    public partial class VDailyPlanSelector : Form
    {
        MDailyPlanMng model = new MDailyPlanMng();
        private int totalRecords = 0;
        private decimal sumQuantity = 0;
        public int selectType = 0;//1为商品砼
        public List<string> MaterialCodes { get; set; }
        private IList result = new ArrayList();
         private EnumSupplyType supplyType;
        /// <summary>
        /// 用来区分专业
        /// </summary>
        public EnumSupplyType SupplyType
        {
            get { return supplyType; }
            set { supplyType = value;
            
            }
        }
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VDailyPlanSelector()
        {
            InitializeComponent();
            InitData();
            InitEvent();
        }
        public VDailyPlanSelector(EnumSupplyType SupplyType)
        {
            this.SupplyType = SupplyType;
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
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
            lnkAll.Click += new EventHandler(lnkAll_Click);
            lnkNone.Click += new EventHandler(lnkNone_Click);
            txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            dgDetail.CellValueChanged += new DataGridViewCellEventHandler(dgDetail_CellValueChanged);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);

        }

        void btnOK_Click(object sender, EventArgs e)
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
                    DailyPlanDetail dtl = var.Tag as DailyPlanDetail;
                    dtl.IsSelect = true;
                }
            }
            result.Add(this.dgMaster.SelectedRows[0].Tag);
            this.btnOK.FindForm().Close();
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            IList list = new ArrayList();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            oq.AddCriterion(Expression.Ge("CreateDate", dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", dtpDateEnd.Value.AddDays(1).Date));
            string sSpecial = string.Empty;
            if (this.SupplyType == EnumSupplyType.土建)
            {
                sSpecial = "土建";

            }
            else if (this.SupplyType == EnumSupplyType.安装)
            {
                sSpecial = "安装";
            }
            if (!string.IsNullOrEmpty(sSpecial))
            {
                oq.AddCriterion(Expression.Eq("Special", sSpecial));
            }
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
                oq.AddFetchMode("MaterialCategory", NHibernate.FetchMode.Eager);
                list = model.DailyPlanSrv.GetDailyPlan(oq);
                ShowMasterList(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            btnCancel.FindForm().Close();
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
            DailyPlanMaster master = dgMaster.CurrentRow.Tag as DailyPlanMaster;
            if (master == null) return;
            foreach (DailyPlanDetail dtl in master.Details)
            {
                if (selectType == 1)
                {
                    if (dtl.MaterialCode.StartsWith("I11201") == false)
                    {
                        continue;
                    }
                }
                if (selectType == 0)
                {
                    //if (dtl.MaterialCode.StartsWith("I142") == false)
                    //{
                    //    continue;
                    //}
                    if (!IsExistMaterialHire(dtl))
                    {
                        continue;
                    }
                }
                decimal canUseQuantity = decimal.Subtract(dtl.Quantity, dtl.RefQuantity);
                if (canUseQuantity <= 0) continue;

                int rowIndex = dgDetail.Rows.Add();
                dgDetail.Rows[rowIndex].Tag = dtl;
                if (dtl.MaterialResource != null)
                {
                    dgDetail[colMaterialCode.Name, rowIndex].Value = dtl.MaterialResource.Code;
                    dgDetail[colMaterialName.Name, rowIndex].Value = dtl.MaterialResource.Name;
                    dgDetail[colSpec.Name, rowIndex].Value = dtl.MaterialResource.Specification;
                    dgDetail[colPrice.Name, rowIndex].Value = dtl.Price;
                    if (dtl.UsedPart != null)
                        dgDetail[colUsedPart.Name, rowIndex].Tag = dtl.UsedPart;
                    dgDetail[colUsedPart.Name, rowIndex].Value = dtl.UsedPartName;
                    dgDetail[colDescript.Name, rowIndex].Value = dtl.Descript;
                    dgDetail[colQuoteQuantity.Name, rowIndex].Value = dtl.Quantity - dtl.RefQuantity;
                    dgDetail[colUsedRank.Name, rowIndex].Value = dtl.UsedRankName;
                }
                dgDetail[this.colDiagramNum.Name, rowIndex].Value = dtl.DiagramNumber;
                dgDetail[colQuantity.Name, rowIndex].Value = dtl.Quantity;
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

        /// <summary>
        /// 显示主表
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList masterList)
        {
            dgMaster.Rows.Clear();
            dgDetail.Rows.Clear();
            if (masterList == null || masterList.Count == 0) return;
            foreach (DailyPlanMaster master in masterList)
            {
                if (selectType == 1)
                {
                    int count = 0;
                    foreach (DailyPlanDetail detail in master.Details)
                    {
                        if (detail.MaterialCode.StartsWith("I11201") == true)
                        {
                            count++;
                        }
                    }
                    if (count == 0)
                        continue;
                }
                else if (selectType == 0)
                {
                    //int count = 0;
                    //foreach (DailyPlanDetail detail in master.Details)
                    //{
                    //    if (detail.MaterialCode.StartsWith("I142") == true)
                    //    {
                    //        count++;
                    //    }
                    //}
                    //if (count == 0)
                    //    continue;
                    if (!IsExistMaterialHire(master))
                    {
                        continue;
                    }
                }
                else if (selectType == 3)
                {
                    int count = 0;
                    count++;
                    if (count == 0)
                        continue; 
                }
                if (!HasRefQuantity(master)) continue;
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colDgMasterCode.Name, rowIndex].Value = master.Code;
                string[] sArray = master.CreateDate.ToString().Split(' ');
                string stra = sArray[0];
                dgMaster[colDgMasterCreateDate.Name, rowIndex].Value = stra;
                dgMaster[colDgMasterCreatePerson.Name, rowIndex].Value = master.CreatePersonName;
                dgMaster[colDgMasterDescript.Name, rowIndex].Value = master.Descript;
                dgMaster[colHandlePerson.Name, rowIndex].Value = master.HandlePersonName;
                dgMaster[colSumMoney.Name, rowIndex].Value = master.SumMoney;
                dgMaster[colDgMasterState.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);
                dgMaster[colDgMasterSumQuantity.Name, rowIndex].Value = master.SumQuantity;
            }
            if (dgMaster.Rows.Count > 0)
            {
                dgMaster.CurrentCell = dgMaster[1, 0];
                dgMaster_SelectionChanged(dgMaster, new EventArgs());
            }
        }
        public bool IsExistMaterialHire(DailyPlanMaster master)
        {
            bool bResult = false;
            if (MaterialCodes == null || MaterialCodes.Count == 0)
            {
                if (master.Details.OfType<DailyPlanDetail>().FirstOrDefault(a => a.MaterialCode.StartsWith("I142")) != null)
                {
                    bResult = true;
                }
            }
            else
            {
                if (master.Details.OfType<DailyPlanDetail>().FirstOrDefault(a => MaterialCodes.Contains(a.MaterialCode)) != null)
                {
                    bResult = true;
                }
            }
            return bResult;
        }
        public bool IsExistMaterialHire(DailyPlanDetail detail)
        {
            bool bResult = false;
            if (MaterialCodes == null || MaterialCodes.Count == 0)
            {
                if (detail.MaterialCode.StartsWith("I142"))
                {
                    bResult = true;
                }
            }
            else
            {
                if (MaterialCodes.Contains(detail.MaterialCode))
                {
                    bResult = true;
                }
            }
            return bResult;
        }
        /// <summary>
        /// 判断主表是否有可引用数量
        /// </summary>
        /// <param name="master"></param>
        /// <returns></returns>
        private bool HasRefQuantity(DailyPlanMaster master)
        {
            if (master == null || master.Details.Count == 0) return false;
            foreach (DailyPlanDetail dtl in master.Details)
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
