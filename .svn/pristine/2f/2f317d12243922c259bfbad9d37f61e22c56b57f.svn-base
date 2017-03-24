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
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatformUI;
using Application.Business.Erp.SupplyChain.Approval.AppProcessMng.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.ApprovalMng.AppTableSetMng.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppProcessUI
{
    public partial class VAppOpinionGWBSConfirm : TBasicDataView
    {
        private MAppPlatform Model = new MAppPlatform();
        private IList confirmList;
        public VAppOpinionGWBSConfirm(IList list)
        {
            InitializeComponent();
            confirmList = list;
            InitDate();
        }
        void InitDate()
        {
            InitEvent();
            if (confirmList != null && confirmList.Count > 0)
            {
                ShowMasterList(confirmList);
            }
        }
        void InitEvent()
        {
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
        }
        /// <summary>
        /// 显示主表
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList masterList)
        {
            dgMaster.Rows.Clear();
            if (masterList == null || masterList.Count == 0) return;
            foreach (GWBSTaskConfirmMaster master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCode.Name, rowIndex].Value = master.Code;
                dgMaster[colCode.Name, rowIndex].ToolTipText = master.Id;
                string[] sArray = master.CreateDate.ToString().Split(' ');
                string stra = sArray[0];
                dgMaster[colCreateDate.Name, rowIndex].Value = stra;
                dgMaster[colConfirmHandlePersonName.Name, rowIndex].Value = master.ConfirmHandlePersonName;
                dgMaster[colProjectName.Name, rowIndex].Value = master.ProjectName;
                if (master.DocState != null)
                {
                    dgMaster[colDocState.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);
                }
            }
            dgMaster.CurrentCell = dgMaster[1, 0];
            dgMaster_SelectionChanged(dgMaster, new EventArgs());
        }
        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            dgDetail.Rows.Clear();
            GWBSTaskConfirmMaster master = dgMaster.SelectedRows[0].Tag as GWBSTaskConfirmMaster;
            if (master == null) return;
            foreach (GWBSTaskConfirm dtl in master.Details)
            {
                int rowIndex = dgDetail.Rows.Add();
                dgDetail.Rows[rowIndex].Tag = dtl;
                dgDetail[colState.Name, rowIndex].Value = dtl.AccountingState;
                dgDetail[colGWBSName.Name, rowIndex].Value = dtl.GWBSTreeName;
                //dgDetail[colGWBSName.Name, rowIndex].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.GWBSTreeName, dtl.GwbsSysCode);
                dgDetail[colDetailNumber.Name, rowIndex].Value = dtl.DetailNumber;
                dgDetail[colGWBSDetail.Name, rowIndex].Value = dtl.GWBSDetailName;
                dgDetail[colActualCompletedQuantity.Name, rowIndex].Value = dtl.ActualCompletedQuantity;
                dgDetail[colQuantityBeforeConfirm.Name, rowIndex].Value = dtl.QuantityBeforeConfirm;
                dgDetail[colProjectTaskType.Name, rowIndex].Value = dtl.ProjectTaskType;
                dgDetail[colTaskCompletedPercent.Name, rowIndex].Value = dtl.ProgressBeforeConfirm;
                dgDetail[colSumCompletedPercent.Name, rowIndex].Value = dtl.ProgressAfterConfirm;
                dgDetail[colTaskHandle.Name, rowIndex].Value = dtl.TaskHandlerName;
                dgDetail[colUnit.Name, rowIndex].Value = dtl.WorkAmountUnitName;
                dgDetail[colMaterialFeeSettlement.Name, rowIndex].Value = dtl.MaterialFeeSettlementFlag.ToString();
                dgDetail[colDescript.Name, rowIndex].Value = dtl.Descript;
                //dgDetail[colConfirmState.Name, rowIndex].Value = dtl.ConfirmState;
                //dgDetail[colConfirmDescript.Name, rowIndex].Value = dtl.ConfirmDescript;
                dgDetail[colRealOperationDate.Name, rowIndex].Value = dtl.RealOperationDate.ToShortDateString();
                dgDetail[colPlannedQuantity.Name, rowIndex].Value = dtl.PlannedQuantity;
            }
            AddFgAppSetpsInfo(master.Id);
        }

        //加载审批意见
        void AddFgAppSetpsInfo(string Id)
        {
            FgAppSetpsInfo.Rows.Clear();
            IList list_AppStepsInfo = GetAppStepsInfo(Id);
            foreach (AppStepsInfo theAppStepsInfo in list_AppStepsInfo)
            {
                int rowIndex = FgAppSetpsInfo.Rows.Add();
                FgAppSetpsInfo[StepOrder.Name, rowIndex].Value = theAppStepsInfo.StepOrder;
                FgAppSetpsInfo[StepName.Name, rowIndex].Value = theAppStepsInfo.StepsName;
                if (theAppStepsInfo.AppRelations == 0)
                {
                    FgAppSetpsInfo[AppRelations.Name, rowIndex].Value = "或";
                }
                else if (theAppStepsInfo.AppRelations == 1)
                {
                    FgAppSetpsInfo[AppRelations.Name, rowIndex].Value = "并";
                }
                FgAppSetpsInfo[AppRole.Name, rowIndex].Value = theAppStepsInfo.RoleName;
                FgAppSetpsInfo[AppPerson.Name, rowIndex].Value = theAppStepsInfo.AuditPerson.Name;
                FgAppSetpsInfo[AppComments.Name, rowIndex].Value = theAppStepsInfo.AppComments;
                FgAppSetpsInfo[AppDateTime.Name, rowIndex].Value = theAppStepsInfo.AppDate;
                switch (theAppStepsInfo.AppStatus)
                {
                    case -1:
                        FgAppSetpsInfo[AppStatus.Name, rowIndex].Value = "已撤单";
                        break;
                    case 0:
                        FgAppSetpsInfo[AppStatus.Name, rowIndex].Value = "审批中";
                        break;
                    case 1:
                        FgAppSetpsInfo[AppStatus.Name, rowIndex].Value = "未通过";
                        break;
                    case 2:
                        FgAppSetpsInfo[AppStatus.Name, rowIndex].Value = "已通过";
                        break;
                    default:
                        break;
                }
            }
        }
        //根据单据ID查询审批意见
        private IList GetAppStepsInfo(string billID)
        {
            IList list = new ArrayList();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BillId", billID));
            //oq.AddCriterion(Expression.Eq("BillAppDate", AppDate));
            oq.AddFetchMode("AppRole", NHibernate.FetchMode.Eager);
            oq.AddOrder(NHibernate.Criterion.Order.Desc("BillAppDate"));
            oq.AddOrder(NHibernate.Criterion.Order.Asc("StepOrder"));
            list = Model.Service.GetAppStepsInfo(oq);
            //AppStepsInfo a = new AppStepsInfo();
            return list;
        }
    }
}
