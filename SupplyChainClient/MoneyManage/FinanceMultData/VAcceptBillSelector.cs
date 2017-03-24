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
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VAcceptBillSelector : TBasicDataView
    {
        private IList result = new ArrayList();
        CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
        OperationOrgInfo subOrgInfo = StaticMethod.GetSubCompanyOrgInfo();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }
        MFinanceMultData model = new MFinanceMultData();
        public VAcceptBillSelector()
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

            txtBillNo.tbKeyDown+=new KeyEventHandler(txtBillNo_tbKeyDown);
            dgDetail.CellDoubleClick+=new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
        }

        private void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnOK_Click(btnOK, null);
        }

        private void txtBillNo_tbKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(btnSearch, null);
            }
        }

        public void InitData()
        {
            this.dtpDateBeginBill.Value = ConstObject.TheLogin.LoginDate.AddMonths(-3);
            this.dtpExpireDate.Value = ConstObject.TheLogin.LoginDate.AddYears(1);
            VBasicDataOptr.InitBillType(this.cmbBillType, true);
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnCancel.FindForm().Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.result.Clear();
            this.dgDetail.EndEdit();
            if (this.dgDetail.SelectedRows.Count>0 &
                this.dgDetail.SelectedRows[0] != null)
            {
                result.Add(this.dgDetail.SelectedRows[0].Tag);
            }
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

            oq.AddCriterion(Expression.Like("OpgSysCode", "%" + subOrgInfo.SysCode + "%"));
            if (this.txtBillNo.Text != "")
            {
                oq.AddCriterion(Expression.Like("BillNo", "%" + txtBillNo.Text + "%"));
            }
            if (this.cmbBillType.Text != "")
            {
                oq.AddCriterion(Expression.Like("BillType", "%" + cmbBillType.Text + "%"));
            }
            oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBeginBill.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEndBill.Value.AddDays(1).Date));
            oq.AddCriterion(Expression.Lt("ExpireDate", this.dtpExpireDate.Value.AddDays(1).Date));
            oq.AddCriterion(Expression.IsNull("PaymentMxId"));
            list = model.FinanceMultDataSrv.GetAcceptanceBill(oq);

            if (list.Count > 0)
            {
                this.dgDetail.Rows.Clear();
                foreach (AcceptanceBill bill in list)
                {
                    int rowIndex = dgDetail.Rows.Add();
                    dgDetail.Rows[rowIndex].Tag = bill;
                    dgDetail[this.colBillDate.Name, rowIndex].Value = bill.CreateDate.ToShortDateString();
                    dgDetail[this.colBillNo.Name, rowIndex].Value = bill.BillNo;
                    dgDetail[this.colBillType.Name, rowIndex].Value = bill.BillType;
                    dgDetail[this.colExpireDate.Name, rowIndex].Value = bill.ExpireDate.ToShortDateString();
                    dgDetail[this.colMoney.Name, rowIndex].Value = bill.SumMoney;
                    dgDetail[this.colCreatePerson.Name, rowIndex].Value = bill.CreatePersonName;
                    dgDetail[this.colProjectName.Name, rowIndex].Value = bill.ProjectName;
                }
            }
        }
    }
}
