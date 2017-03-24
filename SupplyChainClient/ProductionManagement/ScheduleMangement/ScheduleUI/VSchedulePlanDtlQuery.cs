using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.Properties;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI
{
    public partial class VSchedulePlanDtlQuery : TBasicDataView
    {
        MProductionMng model = new MProductionMng();

        CurrentProjectInfo projectInfo = null;

        /// <summary>
        /// 要显示的滚动进度计划明细
        /// </summary>
        public ProductionScheduleDetail PlanDtl = null;

        public VSchedulePlanDtlQuery()
        {
            InitializeComponent();
            InitEvent();
            InitForm();
        }

        private void InitForm()
        {
            projectInfo = StaticMethod.GetProjectInfo();
        }

        private void InitEvent()
        {
            btnExit.Click += new EventHandler(btnExit_Click);

            this.Load += new EventHandler(VScheduleSelector_Load);
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        void VScheduleSelector_Load(object sender, EventArgs e)
        {
            if (PlanDtl != null)
            {
                int rowIndex = dgDetail.Rows.Add();
                dgDetail.Rows[rowIndex].Tag = PlanDtl;

                dgDetail[colPlanName.Name, rowIndex].Value = PlanDtl.Master.ScheduleTypeDetail;//计划名称
                dgDetail[colPlanVersion.Name, rowIndex].Value = PlanDtl.Master.ScheduleName;//计划版本

                dgDetail[colGWBSTreeName.Name, rowIndex].Value = PlanDtl.GWBSTreeName;
                dgDetail[colGWBSTreeName.Name, rowIndex].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), PlanDtl.GWBSTreeName, PlanDtl.GWBSTreeSysCode);

                dgDetail[colFigureprogress.Name, rowIndex].Value = PlanDtl.AddupFigureProgress;

                dgDetail[colPlannedBeginDate.Name, rowIndex].Value = StaticMethod.GetShowDateTimeStr(PlanDtl.PlannedBeginDate, false);
                dgDetail[colPlannedEndDate.Name, rowIndex].Value = StaticMethod.GetShowDateTimeStr(PlanDtl.PlannedEndDate, false);
                dgDetail[colPlannedDuration.Name, rowIndex].Value = PlanDtl.PlannedDuration;

                dgDetail[colActualBeginDate.Name, rowIndex].Value = StaticMethod.GetShowDateTimeStr(PlanDtl.ActualBeginDate, false);
                dgDetail[colActualEndDate.Name, rowIndex].Value = StaticMethod.GetShowDateTimeStr(PlanDtl.ActualEndDate, false);
                dgDetail[colActualDuration.Name, rowIndex].Value = PlanDtl.ActualDuration;

                dgDetail[colUnit.Name, rowIndex].Value = PlanDtl.ScheduleUnit;
                dgDetail[colPlanDesc.Name, rowIndex].Value = PlanDtl.Master.Descript;
                dgDetail[colDtlRemark.Name, rowIndex].Value = PlanDtl.TaskDescript;
            }

        }


    }
}
