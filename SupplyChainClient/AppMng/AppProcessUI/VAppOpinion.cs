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

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppProcessUI
{
    public partial class VAppOpinion : TBasicDataView
    {
        private MAppPlatform Model = new MAppPlatform();
        private string billID = string.Empty;
        public VAppOpinion(string id)
        {
            InitializeComponent();
            billID = id;
            InitDate();
        }
        void InitDate()
        {
            AddFgAppSetpsInfo();
        }
        void AddFgAppSetpsInfo()
        {
            IList list_AppStepsInfo = GetAppStepsInfo(billID);
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
        private IList GetAppStepsInfo(string BillId)
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
