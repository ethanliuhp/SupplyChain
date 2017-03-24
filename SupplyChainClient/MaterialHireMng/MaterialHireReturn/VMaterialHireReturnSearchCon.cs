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
using Application.Business.Erp.SupplyChain.Basic.Domain;
using NHibernate.Criterion;
using System.Collections;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireReturn.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireReturn
{
    public partial class VMaterialHireReturnSearchCon: BasicUserControl
    {
        private MMaterialHireMng model = new MMaterialHireMng();
        private VMaterialHireReturnSearchList searchList;
        public bool IsLoss = false;
        public EnumMatHireType MatHireType;

        public VMaterialHireReturnSearchCon(VMaterialHireReturnSearchList searchList, bool IsLoss, EnumMatHireType MatHireType)
        {
            InitializeComponent();
            this.searchList = searchList;
            this.IsLoss = IsLoss;
            this.MatHireType = MatHireType;
            this.ViewType = VirtualMachine.Component.WinMVC.core.ViewType.Search;
            InitEvent();
            InitForm();
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
           
       
            oq.AddCriterion(Expression.Eq("IsLoss", IsLoss));
            oq.AddCriterion(Expression.Eq("MatHireType", MatHireType));
            if (this.txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Between("Code", this.txtCodeBegin.Text, this.txtCodeEnd.Text));
            }
            //查询当前项目和自己的单据
            oq.AddCriterion(Expression.Eq("CreatePerson", ConstObject.LoginPersonInfo));
            //CurrentProjectInfo ProjectInfo = StaticMethod.GetProjectInfo();
            //oq.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));

            //if (cbCreateDate.Checked == true)
            //{
                oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value.Date));
                oq.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));
            //}
            //else if (cbOperDate.Checked == true)
            //{
            //    oq.AddCriterion(Expression.Ge("RealOperationDate", this.dtpDateBegin.Value.Date));
            //    oq.AddCriterion(Expression.Lt("RealOperationDate", this.dtpDateEnd.Value.AddDays(1).Date));
            //}

            //if (txtOriConNo.Text != "")
            //{
            //    oq.AddCriterion(Expression.Like("OldContractNum", txtOriConNo.Text, MatchMode.Anywhere));
            //}
            if (txtSupplier.Text != "" && txtSupplier.Result.Count > 0)
            {
                oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", txtSupplier.Result[0] as SupplierRelationInfo));
            }
            oq.AddOrder(new Order("Code", false));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            IList list = model.MaterialHireMngSvr.ObjectQuery(typeof(MatHireReturnMaster),oq);
            searchList.RefreshData(list);
            this.btnOK.FindForm().Close();
        }
    }
}

