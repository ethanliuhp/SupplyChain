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
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic;
using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.PenaltyDeductionManagement.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement
{
    public partial class VPenaltyDeductionSelect : TBasicDataView
    {
        MPenaltyDeductionMng model = new MPenaltyDeductionMng();
         private PenaltyDeductionMaster curBillMaster;
         /// <summary>
         /// 当前单据
         /// </summary>
         public PenaltyDeductionMaster CurBillMaster
         {
             get { return curBillMaster; }
             set { curBillMaster = value; }
         }

        public VPenaltyDeductionSelect()
        {
            InitializeComponent();
            InitEvent();
            InitForm();            
        }

        private void InitForm()
        {
            dtpDateBegin.Value = ConstObject.LoginDate.AddDays(-7);
            dtpDateEnd.Value = ConstObject.LoginDate;
            txtSporadicType.Items.AddRange(new object[] { "计时派工", "零星用工"});
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode3;
        }

        private void InitEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
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
            PenaltyDeductionMaster master = dgMaster.CurrentRow.Tag as PenaltyDeductionMaster;
            if (master == null) return;
            foreach (PenaltyDeductionDetail dtl in master.Details)
            {
                int rowIndex = dgDetail.Rows.Add();
                dgDetail.Rows[rowIndex].Tag = dtl;
                string a = dtl.BusinessDate.ToString();
                string[] aArray = a.Split(' ');
                string strz = aArray[0];
                this.dgDetail[colBusinessDate.Name, rowIndex].Value = strz;
                this.dgDetail[colPenaltyMoney.Name, rowIndex].Value = dtl.PenaltyMoney;
                this.dgDetail[colPenaltyQuantity.Name, rowIndex].Value = dtl.PenaltyQuantity;
                //this.dgDetail[colPenaltySubject.Name, i].Tag = var.PenaltySubject;//罚扣款科目
                this.dgDetail[colProductUnit.Name, rowIndex].Tag = dtl.ProductUnit;
                this.dgDetail[colProductUnit.Name, rowIndex].Value = dtl.ProductUnitName;
                this.dgDetail[colProjectDetail.Name, rowIndex].Value = dtl.TaskDetailName;
                this.dgDetail[colProjectType.Name, rowIndex].Value = dtl.ProjectTaskName;
                this.dgDetail[colPenaltySubject.Name, rowIndex].Value = dtl.PenaltySubject;
                this.dgDetail[colPenaltyType1.Name, rowIndex].Value = dtl.PenaltyType;
                this.dgDetail[colAccountMoney.Name, rowIndex].Value = dtl.AccountMoney;
                this.dgDetail[colAccountPrice.Name, rowIndex].Value = dtl.AccountPrice;
                this.dgDetail[colAccountQuantity.Name, rowIndex].Value = dtl.AccountQuantity;
                this.dgDetail[colPriceUnit.Name, rowIndex].Value = dtl.MoneyUnitName;
                this.dgDetail[colPriceUnit.Name, rowIndex].Tag = dtl.MoneyUnit;
                dgDetail[colSelect.Name, rowIndex].Value = true;
            }
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.dgDetail.EndEdit();
            if (this.dgMaster.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择信息！");
                return;
            }
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) break;

                if (var.IsNewRow) break;
                PenaltyDeductionDetail curBillDtl = new PenaltyDeductionDetail();
                if (var.Tag != null)
                {
                    curBillDtl = var.Tag as PenaltyDeductionDetail;
                    if (curBillDtl.Id == null)
                    {
                        curBillMaster.Details.Remove(curBillDtl);
                    }
                }
                PenaltyDeductionDetail dtl = var.Tag as PenaltyDeductionDetail;
                dtl.AccountQuantity = ClientUtil.ToInt(var.Cells[colAccountQuantity.Name].Value);//核算工程量
                dtl.AccountPrice = ClientUtil.ToDecimal(var.Cells[colAccountPrice.Name].Value);//核算单价
                dtl.AccountMoney = ClientUtil.ToDecimal(var.Cells[colAccountMoney.Name].Value);//核算合价
                dtl.MoneyUnit = var.Cells[colPriceUnit.Name].Tag as StandardUnit;
                dtl.MoneyUnitName = ClientUtil.ToString(var.Cells[colPriceUnit.Name].Value);//价格单位 
                curBillMaster.AddDetail(dtl);     
            }
            curBillMaster = model.PenaltyDeductionSrv.UpdatePenaltyDeduction(curBillMaster);
            this.ViewCaption = "派工核算单" + "-" + curBillMaster.Code.ToString();
            MessageBox.Show("保存成功！");
            return;
        }

        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            bool flag = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            if (colName == colAccountPrice.Name || colName == colAccountQuantity.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colAccountQuantity.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colAccountQuantity.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("核算用工量为数字！");
                        this.dgDetail.Rows[e.RowIndex].Cells[colAccountQuantity.Name].Value = "";
                        //dgDetail.CurrentCell = dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name];
                        flag = false;
                    }
                }
                if (dgDetail.Rows[e.RowIndex].Cells[colAccountPrice.Name].Value != null)
                {
                    string temp_price = dgDetail.Rows[e.RowIndex].Cells[colAccountPrice.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_price);
                    if (validity == false)
                    {
                        MessageBox.Show("核算单价为数字！");
                        this.dgDetail.Rows[e.RowIndex].Cells[colAccountPrice.Name].Value = "";
                        //dgDetail.CurrentCell = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name];
                        flag = false;
                    }
                }
                if (flag)
                {
                    //根据单价和数量计算金额  
                    decimal money = 0;
                    object price = dgDetail.Rows[e.RowIndex].Cells[colAccountPrice.Name].Value;
                    object quantity = dgDetail.Rows[e.RowIndex].Cells[colAccountQuantity.Name].Value;
                    for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                    {
                        quantity = dgDetail.Rows[i].Cells[colAccountQuantity.Name].Value;
                        if (quantity == null) quantity = 0;

                        if (ClientUtil.ToString(dgDetail.Rows[i].Cells[colAccountQuantity.Name].Value) != "" && ClientUtil.ToString(dgDetail.Rows[i].Cells[colAccountPrice.Name].Value) != "")
                        {
                            money = ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colAccountQuantity.Name].Value) * ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colAccountPrice.Name].Value);
                            dgDetail.Rows[i].Cells[colAccountMoney.Name].Value = ClientUtil.ToString(money);
                        }
                    }
                }

            }
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            IList list = new ArrayList();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
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
            if (txtSupply.Text != "" && txtSupply.Result.Count > 0)
            {
                oq.AddCriterion(Expression.Eq("PenaltyDeductionRantName", txtSupply.Text.ToString()));
            }
            if (txtSporadicType.SelectedItem != "" && txtSporadicType.SelectedItem != null)
            {
                //oq.AddCriterion(Expression.Eq("LaborState", txtSporadicType.SelectedItem));
            }
            else
            {
                //oq.AddCriterion(Expression.Like("LaborState", "代工"));
                //oq.AddCriterion(Expression.NotEqProperty("LaborState","代工"));
            }
            try
            {
                list = model.PenaltyDeductionSrv.GetPenaltyDeduction(oq);
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
            foreach (PenaltyDeductionMaster master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colDgMasterCode.Name, rowIndex].Value = master.Code;
                string[] sArray = master.CreateDate.ToString().Split(' ');
                string stra = sArray[0];
                dgMaster[colDgMasterCreateDate.Name, rowIndex].Value = stra;
                dgMaster[colDgMasterCreatePerson.Name, rowIndex].Value = master.CreatePersonName;
                dgMaster[colDgMasterDescript.Name, rowIndex].Value = master.Descript;
                dgMaster[colDgMasterState.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);
                dgMaster[colPenaltyTeam.Name, rowIndex].Value = master.PenaltyDeductionRantName;//罚款队伍名称
                dgMaster[colPenaltyTeam.Name, rowIndex].Tag = master.PenaltyDeductionRant;
                dgMaster[colPenaltyType.Name, rowIndex].Value = master.PenaltyType;//专业罚款类型
                curBillMaster = master;
            }
            dgMaster.CurrentCell = dgMaster[1, 0];
            dgMaster_SelectionChanged(dgMaster, new EventArgs());
        }
    }
}
