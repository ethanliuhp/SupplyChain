using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan
{
    public partial class VFundAssessmentCon : BasicUserControl
    {
        private MFinanceMultData model = new MFinanceMultData();
        private VFundAssessmentSearchList searchList;
        private string fundAssessOrAssessCash;

        public VFundAssessmentCon(VFundAssessmentSearchList searchList)
        {
            InitializeComponent();

            this.searchList = searchList;
            this.ViewType = VirtualMachine.Component.WinMVC.core.ViewType.Search;

            this.InitForm();

            this.InitEvent();

            btnOK.Focus();
        }

        public VFundAssessmentCon(VFundAssessmentSearchList searchList, string fundAssessOrAssessCash)
            : this(searchList)
        {

            this.fundAssessOrAssessCash = fundAssessOrAssessCash;
        }

        private void InitForm()
        {
            this.dtpDateBegin.Value = ConstObject.LoginDate.AddMonths(-1);
            this.dtpDateEnd.Value = ConstObject.LoginDate;

            var currentProject = StaticMethod.GetProjectInfo();
            if (currentProject != null && currentProject.Code.Equals(CommonUtil.CompanyProjectCode))
            {
                currentProject = null;
            }
        }

        private void InitEvent()
        {
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();

            if (this.txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Like("Code", "%" + this.txtCodeBegin.Text + "%"));
            }
            oq.AddCriterion(Expression.Eq("OperOrgInfo", ConstObject.TheLogin.TheOperationOrgInfo));
            oq.AddCriterion(Expression.Eq("CreatePerson", ConstObject.LoginPersonInfo));
            //区分专业
            oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));
            oq.AddOrder(new Order("Code", false));

            if (this.fundAssessOrAssessCash.Contains("考核兑现"))
            {
                oq.AddCriterion(Expression.Like("Code", "%考核兑现%"));
            }
            else if (this.fundAssessOrAssessCash.Contains("利息计算"))
            {
                oq.AddCriterion(Expression.Like("Code", "%利息计算%"));
            }

            IList list = model.FinanceMultDataSrv.Query(typeof(FundAssessmentMaster), oq);
            searchList.RefreshData(list);

            this.btnOK.FindForm().Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }
    }
}
