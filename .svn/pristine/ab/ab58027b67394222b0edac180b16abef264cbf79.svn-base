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
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI
{
    public partial class VWeekScheduleSearchCon : BasicUserControl
    {
        private MProductionMng model = new MProductionMng();
        private VWeekScheduleSearchList searchList;
        private EnumExecScheduleType execScheduleType;

        public VWeekScheduleSearchCon(VWeekScheduleSearchList searchList, EnumExecScheduleType execScheduleType)
        {
            this.searchList = searchList;
            this.execScheduleType = execScheduleType;
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

            //查询自己的单据
            oq.AddCriterion(Expression.Eq("CreatePerson", ConstObject.LoginPersonInfo));

            oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));

            //if (cboExecScheduleType.SelectedItem != null)
            //{
            //    oq.AddCriterion(Expression.Eq("ExecScheduleType", cboExecScheduleType.SelectedItem.ToString()));
            //}
            oq.AddCriterion(Expression.Eq("ProjectId", StaticMethod.GetProjectInfo().Id));
            if (this.execScheduleType == EnumExecScheduleType.周进度计划)
            {
                oq.AddCriterion(Expression.Eq("ExecScheduleType", this.execScheduleType));
            }
            //else if (this.execScheduleType == EnumExecScheduleType.月度进度计划)
            //{
            //    oq.AddCriterion(Expression.Not(Expression.Eq("ExecScheduleType", EnumExecScheduleType.周进度计划)));
            //}
            else
            {
                Disjunction dis = new Disjunction();
                dis.Add(Expression.Eq("ExecScheduleType", EnumExecScheduleType.年度进度计划));
                dis.Add(Expression.Eq("ExecScheduleType", EnumExecScheduleType.季度进度计划));
                dis.Add(Expression.Eq("ExecScheduleType", EnumExecScheduleType.月度进度计划));

                oq.AddCriterion(dis);

            }

            oq.AddOrder(new Order("Code", false));
            IList list = model.ProductionManagementSrv.GetWeekScheduleMaster(oq);
            searchList.RefreshData(list);
            this.btnOK.FindForm().Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }
    }
}
