using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery;
using Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery.DailyInspectionRecord;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.DailyInspection
{
    public partial class VInspectionRecord : TBasicToolBarByMobile
    {
        private AutomaticSize automaticSize = new AutomaticSize();  
        MProjectTaskQuery model = new MProjectTaskQuery();
        private int pageIndex = 0;

        public VInspectionRecord()
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            InitData();
        }

        public void InitData()
        {
            cbInspectionSpecial.Items.Clear();
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.WBS_CheckRequire);
            foreach (BasicDataOptr b in list)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                li.Text = b.BasicName;
                li.Value = b.BasicCode;
                cbInspectionSpecial.Items.Add(li);
            }
            cbInspectionConclusion.Items.AddRange(new object[] { "通过", "不通过" });
        }


        private void VInspectionRecord_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        public override void TBtnPageDown_Click(object sender, EventArgs e)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            if (this.txtWeekSchedule.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("GWBSTreeName", this.txtWeekSchedule.Text, MatchMode.Anywhere));
            }
            if (this.txtBuidlingPart.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("", this.txtBuidlingPart.Text, MatchMode.Anywhere));
            }
            if (this.txtInspectionPerson.Text != "")
            {
                objectQuery.AddCriterion(Expression.Eq("Master.HandlePersonName", txtInspectionPerson.Text));
            }
            if (this.txtConstructionTeam.Text != "")
            {
                objectQuery.AddCriterion(Expression.Eq("SupplierName", this.txtConstructionTeam.Text));
            }
            objectQuery.AddCriterion(Expression.Ge("InspectionDate", this.dtpDateBegin.Value.Date));
            objectQuery.AddCriterion(Expression.Lt("InspectionDate", this.dtpDateEnd.Value.AddDays(1).Date));
            IList list = model.GetInspectionRecord(objectQuery);

            VDailyInspectionRecord v_record = new VDailyInspectionRecord(list, pageIndex);
            v_record.ShowDialog();
        }
    }
}
