using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyOrderMngCompany
{
    public partial class VSupplyOrderSearchConCompany : BasicUserControl
    {
        private MSupplyOrderMng model = new MSupplyOrderMng();
        private VSupplyOrderSearchListCompany searchList;
        private EnumSupplyType supplyType;

        public VSupplyOrderSearchConCompany(VSupplyOrderSearchListCompany searchList, EnumSupplyType supplyType)
        {
            this.searchList = searchList;
            this.supplyType = supplyType;
            InitializeComponent();            
            this.ViewType = VirtualMachine.Component.WinMVC.core.ViewType.Search;
            this.InitForm();
            this.InitEvent();
            btnOK.Focus();
        }


        public VSupplyOrderSearchConCompany(VSupplyOrderSearchListCompany searchList)
        {
            this.searchList = searchList;
            InitializeComponent();
            this.ViewType = VirtualMachine.Component.WinMVC.core.ViewType.Search;
            this.InitForm();
            this.InitEvent();
            btnOK.Focus();
        }

        private void InitForm()
        {
            this.dtpDateBegin.Value = ConstObject.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.LoginDate;
        }

        private void InitEvent()
        {
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();

            if (this.txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Between("Code", this.txtCodeBegin.Text, this.txtCodeEnd.Text));
            }
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Eq("CreatePerson", ConstObject.LoginPersonInfo));
            //oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            //oq.AddCriterion(Expression.Eq("ProjectName", projectInfo.Name));
            //区分专业
            //oq.AddCriterion(Expression.Eq("Special", Enum.GetName(typeof(EnumSupplyType), supplyType)));
            oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));
            if (txtBuyer.Text != "" && txtBuyer.Result.Count > 0)
            {
                oq.AddCriterion(Expression.Eq("SupplierName", this.txtBuyer.Text));//供应商
            }
            if (txtPurchaseContractNo.Text != "")
            {
                oq.AddCriterion(Expression.Eq("OldContractNum", this.txtPurchaseContractNo.Text));//原始合同号
            }
            oq.AddOrder(new Order("Code", false));
            IList list = model.SupplyOrderSrv.GetSupplyOrderCompany(oq);
            searchList.RefreshData(list);
            this.btnOK.FindForm().Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }
    }
}
