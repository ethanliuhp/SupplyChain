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
using Application.Business.Erp.SupplyChain.ProductionManagement.PenaltyDeductionManagement.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement
{
    public partial class VPenaltyDeductionSearchCon : BasicUserControl
    {
        private MPenaltyDeductionMng model = new MPenaltyDeductionMng();
        private VPenaltyDeductionSearchList searchList;

        public VPenaltyDeductionSearchCon(VPenaltyDeductionSearchList searchList)
        {
            this.searchList = searchList;
            InitializeComponent();
            this.ViewType = VirtualMachine.Component.WinMVC.core.ViewType.Search;
            this.InitForm();
            this.InitEvent();
            btnOK.Focus();
            InitPlanType();
        }
        private void InitPlanType()
        {
            //罚扣原因
            ((ComboBox)txtPenaltyReason).Items.AddRange(Enum.GetNames(typeof(PenaltyDeductionType)));
        }

        private void InitForm()
        {
            this.dtpDateBegin.Value = ConstObject.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.LoginDate;
            this.txtPenaltyUnit.SupplierCatCode = CommonUtil.SupplierCatCode3 + "-";
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
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));
            //if (!ClientUtil.ToString(this.txtPenaltyReason.SelectedItem).Equals(""))
            //{
            oq.AddCriterion(Expression.Like("PenaltyDeductionReason", "日常检查罚款"));//专业罚款类型
            //}
            //if (!ClientUtil.ToString(this.txtPenaltyUnit.Text).Equals(""))
            //{
            //    oq.AddCriterion(Expression.Ge("", this.txtPenaltyUnit.Text.ToString()));//罚扣队伍
            //}
            if (txtPenaltyUnit.Value != "" && txtPenaltyUnit.Result.Count > 0)
            {
                oq.AddCriterion(Expression.Eq("PenaltyDeductionRantName", this.txtPenaltyUnit.Value));//根据名字查询罚款队伍
            }
            oq.AddOrder(new Order("Code", false));
            IList list = model.PenaltyDeductionSrv.GetPenaltyDeduction(oq);
            searchList.RefreshData(list);
            this.btnOK.FindForm().Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }
    }
}
