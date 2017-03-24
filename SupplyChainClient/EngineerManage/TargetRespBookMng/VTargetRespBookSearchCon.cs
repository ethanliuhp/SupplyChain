using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using NHibernate.Criterion;
using CustomServiceClient.CustomWebSrv;
using Application.Business.Erp.SupplyChain.EngineerManage.Client.TargetRespBookMng;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng
{
    public partial class VTargetRespBookSearchCon : BasicUserControl
    {
        private MTargetRespBookMng model = new MTargetRespBookMng();
        private VTargetRespBookSearchList searchlist;
        
        public VTargetRespBookSearchCon(VTargetRespBookSearchList searchlist)
        {
            InitializeComponent();
            this.searchlist = searchlist;
            this.ViewType = VirtualMachine.Component.WinMVC.core.ViewType.Search;
            InitEvent();
            btnOK.Focus();
        }

        public void InitEvent()
        {
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            cbSignedWhether.Items.AddRange(new object[]{"已签订","未签订"});
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();

            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            if (txtProjectName.Text != "")
            {
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            }
            if (txtHandlePerson.Text != "")
            {
                oq.AddCriterion(Expression.Eq("HandlePerson", txtHandlePerson.Text));
            }
            if (cbSignedWhether.SelectedItem != null)
            {
                oq.AddCriterion(Expression.Like("SignedWhether", cbSignedWhether.SelectedItem));
            }
            oq.AddCriterion(Expression.Ge("CreateDate", this.dtpCreateDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", this.dtpCreateDateEnd.Value.AddDays(1).Date));
            IList list = model.TargetRespBookSrc.GetTargetRespBook(oq);
            searchlist.RefreshData(list);
            this.btnOK.FindForm().Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }
    }
}
