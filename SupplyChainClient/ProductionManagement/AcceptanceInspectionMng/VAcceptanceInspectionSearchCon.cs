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
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.AcceptanceInspectionMng
{
    public partial class VAcceptanceInspectionSearchCon : BasicUserControl
    {
        private MAcceptanceInspectionMng model = new MAcceptanceInspectionMng();
        private VAcceptanceInspectionSearchList searchList;
        public VAcceptanceInspectionSearchCon(VAcceptanceInspectionSearchList searchList)
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

        //查询
        void btnOK_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            if (this.txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Between("Code", this.txtCodeBegin.Text, this.txtCodeEnd.Text));
            }
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Ge("InspectionDate", this.dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("InspectionDate", this.dtpDateEnd.Value.AddDays(1).Date));
            oq.AddCriterion(Expression.Eq("CreatePerson", ConstObject.LoginPersonInfo));
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddOrder(new Order("Code", false));
            IList list = model.AcceptanceInspectionSrv.GetAcceptanceInspection(oq);
            searchList.RefreshData(list);
            this.btnOK.FindForm().Close();
        }

        //取消
        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }
    }
}
