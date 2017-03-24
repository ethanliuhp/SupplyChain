using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Core;
using System.Collections;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalLedgerMng.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VCusAndSupRelSelector : TBasicDataView
    {
        private IList result = new ArrayList();
        CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }
        MFinanceMultData model = new MFinanceMultData();
        public VCusAndSupRelSelector()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            txtCode.tbKeyDown += new KeyEventHandler(txtCode_tbKeyDown);
            txtName.tbKeyDown += new KeyEventHandler(txtCode_tbKeyDown);

            dgDetail.CellDoubleClick+=new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
        }

        private void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnOK_Click(btnOK, null);
        }

        private void txtCode_tbKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(btnSearch, null);
            }
        }

        public void InitData()
        {

        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnCancel.FindForm().Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.result.Clear();
            this.dgDetail.EndEdit();
            result.Add(this.dgDetail.SelectedRows[0].Tag);
            this.btnOK.FindForm().Close();
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgDetail.Rows.Clear();
            this.GetAcceptBillInfo();
        }

        private void GetAcceptBillInfo()
        {
            ObjectQuery oq = new ObjectQuery();
            IList list = new ArrayList();
            string cusCondition = "";
            string supCondition = "";
            if (this.txtCode.Text != "")
            {
                supCondition += " and t1.code like  '%" + this.txtCode.Text + "%'";
                cusCondition += " and t1.cusrelcode like  '%" + this.txtCode.Text + "%'";
            }
            if (this.txtName.Text != "")
            {
                supCondition += " and t2.orgname like  '%" + this.txtName.Text + "%'";
                cusCondition += " and t2.orgname like  '%" + this.txtName.Text + "%'";
            }
            list = model.FinanceMultDataSrv.QueryCusAndSupInfoByCondition(supCondition, cusCondition);

            if (list.Count > 0)
            {
                this.dgDetail.Rows.Clear();
                foreach (DataDomain domain in list)
                {
                    int rowIndex = dgDetail.Rows.Add();
                    dgDetail.Rows[rowIndex].Tag = domain;
                    dgDetail[this.colCategory.Name, rowIndex].Value = domain.Name8;
                    dgDetail[this.colCode.Name, rowIndex].Value = domain.Name3;
                    dgDetail[this.colName.Name, rowIndex].Value = domain.Name4;
                }
            }
        }
    }
}
