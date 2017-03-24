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
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.ConcreteManage.PouringNoteMng.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.ConcreteManage.PumpingPoundsMng
{
    public partial class VSelectUsedPart : TBasicDataView
    {
        MConcreteMng model = new MConcreteMng();
        public SupplierRelationInfo Supplier;
        public string SupplierName;
        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VSelectUsedPart()
        {
            InitializeComponent();
            InitData();
            InitEvent();
        }

        private void InitEvent()
        {
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.Load += new EventHandler(VSelectUsedPart_Load);
        }

        void VSelectUsedPart_Load(object sender, EventArgs e)
        {
            if (Supplier != null)
            {
                txtSupplier.Result.Clear();
                txtSupplier.Result.Add(Supplier);
                txtSupplier.Tag = Supplier;
                txtSupplier.Text = SupplierName;
            }
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Ge("CreateDate", dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", dtpDateEnd.Value.AddDays(1).Date));

            if (txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Between("Code", txtCodeBegin.Text, txtCodeEnd.Text));
            }
            if (txtSupplier.Text != "")
            {
                oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo.Id", (txtSupplier.Result[0] as SupplierRelationInfo).Id.ToString()));
            }
            try
            {
                IList lst = model.ConcreteMngSrv.GetPouringNoteMaster(oq);
                ShowMasterList(lst);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        private void ShowMasterList(IList lst)
        {
            dgMaster.Rows.Clear();
            if (lst == null || lst.Count == 0) return;
            foreach (PouringNoteMaster master in lst)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster[colCode.Name, rowIndex].Value = master.Code;
                dgMaster[colCreateDate.Name, rowIndex].Value = master.CreateDate.ToString("yyyy-MM-dd");
                dgMaster[colCreatePerson.Name, rowIndex].Value = master.CreatePersonName;
                dgMaster[colHandlePerson.Name, rowIndex].Value = master.HandlePersonName;
                dgMaster[colUsedPart.Name, rowIndex].Value = master.UsedPartName;
                dgMaster[colSumQuantity.Name, rowIndex].Value = master.SumQuantity;
                dgMaster[colMoney.Name, rowIndex].Value = master.SumMoney;
                dgMaster[colDescript.Name, rowIndex].Value = master.Descript;
                dgMaster.Rows[rowIndex].Tag = master;
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            btnCancel.FindForm().Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.result.Clear();
            if (dgMaster.SelectedRows.Count == 0)
            {
                MessageBox.Show("没有找到浇筑记录单！");
                return;
            }

            result.Add(this.dgMaster.SelectedRows[0].Tag);
            this.btnOK.FindForm().Close();
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeBegin.Text = this.txtCodeEnd.Text;
        }

        private void InitData()
        {
            dtpDateBegin.Value = ConstObject.LoginDate.AddDays(-7);
            dtpDateEnd.Value = ConstObject.LoginDate;
        }
    }
}
