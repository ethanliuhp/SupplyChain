using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Application.Business.Erp.SupplyChain.Basic.Service;
using Application.Business.Erp.SupplyChain.ProductionManagement.Service;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.RectificationNoticeMng.Service;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery
{
    public class MProjectTaskQuery
    {
        private static ICurrentProjectSrv currentProjectSrv;
        private static IProductionManagementSrv productionManagementSrv;
        private static IRectificationNoticeSrv rectificationNoticeSrv;

        public MProjectTaskQuery()
        {
            if (currentProjectSrv == null)
                currentProjectSrv = ConstMethod.GetService("CurrentProjectSrv") as ICurrentProjectSrv;
            if (productionManagementSrv == null)
                productionManagementSrv = ConstMethod.GetService("ProductionManagementSrv") as IProductionManagementSrv;
            if (rectificationNoticeSrv == null)
                rectificationNoticeSrv = ConstMethod.GetService("RectificationNoticeSrv") as IRectificationNoticeSrv;
        }

        public bool DeleteByDao(WeekScheduleDetail obj)
        {
            return productionManagementSrv.DeleteByDao(obj);
        }
        public bool DeleteByDao(InspectionRecord obj)
        {
            return productionManagementSrv.DeleteByDao(obj);
        }
        public WeekScheduleDetail SaveOrUpdateByDao(WeekScheduleDetail obj)
        {
            return productionManagementSrv.SaveOrUpdateByDao(obj) as WeekScheduleDetail;
        }
        public InspectionRecord SaveOrUpdateByDao(InspectionRecord obj)
        {
            return productionManagementSrv.SaveOrUpdateByDao(obj) as InspectionRecord;
        }
        public IList GetWeekScheduleDetail(ObjectQuery objectQuery)
        {
            return productionManagementSrv.GetWeekScheduleDetail(objectQuery);
        }
        public IList GetInspectionRecord(ObjectQuery objectQuery)
        {
            return productionManagementSrv.GetInspectionRecord(objectQuery);
        }
        public WeekScheduleDetail UpdateWeekScheduleDetail(WeekScheduleDetail obj)
        {
            return productionManagementSrv.UpdateWeekScheduleDetail(obj);
        }
        public IList GetPersonListByProjectAndRole(string projectId, string roleId)
        {
            return currentProjectSrv.GetPersonListByProjectAndRole(projectId, roleId);
        }
        public IList GetGWBSTaskConfirmMaster(ObjectQuery oq)
        {
            return productionManagementSrv.GetGWBSTaskConfirmMaster(oq);
        }
        public IList GetRectificationNotice(ObjectQuery objectQuery)
        {
            return rectificationNoticeSrv.GetRectificationNotice(objectQuery);
        }

    }
}
