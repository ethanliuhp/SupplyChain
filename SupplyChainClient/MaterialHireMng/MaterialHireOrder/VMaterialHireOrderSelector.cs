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
    public partial class VMaterialHireOrderSelector : TBasicDataView
    {
        MMaterialHireMng model = new MMaterialHireMng();
        private int totalRecords = 0;
        private decimal sumQuantity = 0M;
        EnumMatHireType MatHireType;
        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VMaterialHireOrderSelector(EnumMatHireType MatHireType)
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
        }

        private void InitEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
         this.customButton1.Click += new EventHandler(btnOK_Click);
         this.customButton1.Click += new EventHandler(btnCancel_Click);
            txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            this.dgMaster.CellContentDoubleClick += new DataGridViewCellEventHandler(dgMaster_CellContentDoubleClick);

            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
        }
        void dgMaster_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                this.btnOK_Click(null, null);
            }
        }
        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }

   


        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        
         
        void btnCancel_Click(object sender, EventArgs e)
        {
            this.customButton1.FindForm().Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.result.Clear();
            if (this.dgMaster.SelectedRows .Count==0)
            {
                MessageBox.Show("没有可以引用的合同！");
                return;
            }
            MatHireOrderMaster master = this.dgMaster.SelectedRows[0].Tag as MatHireOrderMaster;
            ObjectQuery oQuery=new ObjectQuery ();
            oQuery.AddCriterion(Expression.Eq("Id",master.Id));
            IList lst = model.MaterialHireMngSvr.GetMaterialHireOrder(oQuery);
            if (lst != null && lst.Count > 0)
            {
                master = lst[0] as MatHireOrderMaster;
            }
            result.Add(master);
            this.customButton1.FindForm().Close();
        }

        private void Clear()
        {
           // lblRecordTotal.Text = string.Format("共选择【{0}】条记录", 0);
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
                oq.AddFetchMode("TheSupplierRelationInfo", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
                list = model.MaterialHireMngSvr.ObjectQuery(typeof(MatHireOrderMaster),oq);//model.MaterialHireMngSvr.GetMaterialHireOrder(oq);
                list = FilterData(list);
                ShowMasterList(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
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
                    else if (MatHireType == EnumMatHireType.其他)
                    {
                        oOrder.TempData = oOrder.Details.OfType<MatHireOrderDetail>().ToList();
                    }
                    if (oOrder.TempData == null || oOrder.TempData.Count==0)
                    {
                        lstData.Remove(oOrder);
                    }
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
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colDgMasterCode.Name, rowIndex].Value = master.Code;
                dgMaster[colDgMasterCreateDate.Name, rowIndex].Value = master.CreateDate;
                dgMaster[colDgMasterCreatePerson.Name, rowIndex].Value = master.CreatePersonName;
                dgMaster[colDgMasterDescript.Name, rowIndex].Value = master.Descript;
                dgMaster[colDgMasterHandlePerson.Name, rowIndex].Value = master.HandlePersonName;
                dgMaster[colDgMasterOriContractNo.Name, rowIndex].Value = master.OriginalContractNo;
                dgMaster[colDgMasterState.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);
                dgMaster[colDgMasterSumQuantity.Name, rowIndex].Value = master.SumQuantity;
                if (master.TheSupplierRelationInfo != null)
                {
                    dgMaster[colDgMasterSupply.Name, rowIndex].Value = master.TheSupplierRelationInfo.SupplierInfo.Name;
                }
                dgMaster[this.colProject.Name, rowIndex].Value = master.ProjectName;
            }
            if (dgMaster.Rows.Count > 0)
            {
                dgMaster.CurrentCell = dgMaster[1, 0];
              
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