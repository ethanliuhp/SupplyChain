﻿using System;
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
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.PersonManagement
{
    public partial class VPersonManageSearchCon : BasicUserControl
    {
        private MPersonManage model = new MPersonManage();
        private VPersonManageSearchList searchList;
        public VPersonManageSearchCon(VPersonManageSearchList searchList)
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
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));
            oq.AddCriterion(Expression.Eq("HandlePerson", ConstObject.LoginPersonInfo));
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));

            //oq.AddCriterion(Expression.Eq("DocState", DocumentState.Edit));

            IList list = model.PersonManageSrv.GetPersonManage(oq);
            searchList.RefreshData(list);
            this.btnOK.FindForm().Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }
    }
}