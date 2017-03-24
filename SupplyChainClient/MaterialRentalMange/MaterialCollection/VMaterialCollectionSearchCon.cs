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

namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialCollection
{
    public partial class VMaterialCollectionSearchCon : BasicUserControl
    {
        private MMatRentalMng model = new MMatRentalMng();
        private VMaterialCollectionSearchList searchList;

        public VMaterialCollectionSearchCon(VMaterialCollectionSearchList searchList)
        {
            InitializeComponent();
            this.searchList = searchList;
            this.ViewType = VirtualMachine.Component.WinMVC.core.ViewType.Search;
            this.InitEvent();
            this.InitForm();
        }
        public void InitForm()
        {
            this.dtpDateBegin.Value = ConstObject.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.LoginDate;
            txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode1;
        }
        public void InitEvent()
        {
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnOK_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();

            if (this.txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Between("Code", this.txtCodeBegin.Text, this.txtCodeEnd.Text));
            }

            //查询当前项目和自己的单据
            //oq.AddCriterion(Expression.Eq("CreatePerson", ConstObject.LoginPersonInfo));
            CurrentProjectInfo ProjectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));

            if (cbCreateDate.Checked == true)
            {
                oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value.Date));
                oq.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));
            }
            else if (cbOperDate.Checked == true)
            {
                oq.AddCriterion(Expression.Ge("RealOperationDate", this.dtpDateBegin.Value.Date));
                oq.AddCriterion(Expression.Lt("RealOperationDate", this.dtpDateEnd.Value.AddDays(1).Date));
            }

            if (txtOriConNo.Text != "")
            {
                oq.AddCriterion(Expression.Like("OldContractNum", txtOriConNo.Text, MatchMode.Anywhere));
            }
            if (txtSupplier.Text != "" && txtSupplier.Result.Count > 0)
            { 
                oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", txtSupplier.Result[0] as SupplierRelationInfo));
            }
            oq.AddOrder(new Order("Code", false));
            IList list = model.MatMngSrv.GetMaterialCollectionMaster(oq);
            searchList.RefreshData(list);
            this.btnOK.FindForm().Close();
        }
    }
}
