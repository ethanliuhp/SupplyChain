﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;
using NHibernate;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection
{
    public partial class VMaterialHireCollectionSearchCon : BasicUserControl
    {
        private MMaterialHireMng model = new MMaterialHireMng();
        private VMaterialHireCollectionSearchList searchList;
        private EnumMatHireType collectionType;
        public VMaterialHireCollectionSearchCon(VMaterialHireCollectionSearchList searchList, EnumMatHireType collectionType)
        {
            InitializeComponent();
            this.searchList = searchList;
            this.collectionType = collectionType;
            searchList.collectionType = collectionType;
            this.ViewType = VirtualMachine.Component.WinMVC.core.ViewType.Search;
            this.InitEvent();
            this.InitForm();
        }
        public void InitForm()
        {
            this.dtpDateBegin.Value = ConstObject.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.LoginDate;
           // txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode1;
        }
        public void InitEvent()
        {
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
           // this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            //this.txtCodeEnd.Text = this.txtCodeBegin.Text;
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
                oq.AddCriterion(Expression.Like("Code", this.txtCodeBegin.Text,MatchMode.Anywhere));
            }
            oq.AddCriterion(Expression.Eq("MatHireType", collectionType));

            //查询当前项目和自己的单据
            oq.AddCriterion(Expression.Eq("CreatePerson", ConstObject.LoginPersonInfo));
            //CurrentProjectInfo ProjectInfo = StaticMethod.GetProjectInfo();
            //oq.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));

            
                oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value.Date));
                oq.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));
            
            oq.AddOrder(new Order("Code", false));
            oq.AddFetchMode("Details", FetchMode.Eager);
            IList list = model.MaterialHireMngSvr.ObjectQuery(typeof(MatHireCollectionMaster), oq);//GetMaterialHireCollectionMaster(oq);
            searchList.RefreshData(list);
            this.btnOK.FindForm().Close();
        }
    }
}