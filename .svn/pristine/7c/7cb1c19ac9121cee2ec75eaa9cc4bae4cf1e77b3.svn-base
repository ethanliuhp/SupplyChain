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
using Application.Business.Erp.SupplyChain.Client.CostManagement.OwnerQuantityMng;
using Application.Business.Erp.SupplyChain.CostManagement.OwnerQuantityMng.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VOwnerQuantityDtlSelector : TBasicDataView
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
        MOwnerQuantityMng model = new MOwnerQuantityMng();
        public VOwnerQuantityDtlSelector()
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
        }
        public void InitData()
        {
            this.dtpDateBeginBill.Value = ConstObject.TheLogin.LoginDate.AddYears(-1);
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
            string condition = " and t1.projectid = '" + projectInfo.Id + "'";
            if (this.txtBillNo.Text != "")
            {
                condition += " and t1.Code like '%" + this.txtBillNo.Text + "%'";//模糊查询
            }
            condition += " and t1.CreateDate>=to_date('" + this.dtpDateBeginBill.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<to_date('" + this.dtpDateEndBill.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
            DataSet dataSet = model.OwnerQuantitySrv.OwnerQuantityQuery(condition);

            this.dgDetail.Rows.Clear();
            DataTable dataTable = dataSet.Tables[0];
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgDetail.Rows.Add();
                dgDetail[this.colBillCode.Name, rowIndex].Value = ClientUtil.ToString(dataRow["Code"]);
                dgDetail[this.colConfirmDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["ConfirmDate"]).ToShortDateString();
                dgDetail[this.colConfirmMoney.Name, rowIndex].Value = ClientUtil.ToString(dataRow["ConfirmMoney"]);
                dgDetail[this.colCreatePerson.Name, rowIndex].Value = ClientUtil.ToString(dataRow["CreatePersonName"]);
                dgDetail[this.colDescript.Name, rowIndex].Value = ClientUtil.ToString(dataRow["DtlDescript"]);
                dgDetail[this.colQuantityDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["QuantityDate"]).ToShortDateString();
                dgDetail[this.colQuantityMoney.Name, rowIndex].Value = ClientUtil.ToString(dataRow["SubmitQuantity"]);
                dgDetail[this.colQWBS.Name, rowIndex].Value = ClientUtil.ToString(dataRow["QWBSName"]);
                DataDomain domain = new DataDomain();
                domain.Name1 = ClientUtil.ToString(dataRow["dtlID"]);
                domain.Name2 = ClientUtil.ToString(dataRow["Code"]);
                domain.Name3 = ClientUtil.ToDateTime(dataRow["QuantityDate"]).ToShortDateString();
                domain.Name4 = ClientUtil.ToString(dataRow["SubmitQuantity"]);
                domain.Name5 = ClientUtil.ToString(dataRow["ConfirmMoney"]);
                domain.Name6 = ClientUtil.ToString(dataRow["QWBSName"]);
                this.dgDetail.Rows[rowIndex].Tag = domain;

            }
        }
    }
}
