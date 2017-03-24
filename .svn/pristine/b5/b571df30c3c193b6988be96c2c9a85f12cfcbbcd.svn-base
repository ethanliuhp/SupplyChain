using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.ProductionManagement.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Service;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Service;
using System.Data;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement
{
    public enum EnumProductionMngExecType
    {
        进度计划,
        执行进度计划查询,
        工程任务确认单查询,
        月计划查询,
        周计划查询,
        项目周计划维护,
        周计划确认,
        工程量确认单维护_非计划,
        总进度计划审批,
        工期预警,
        劳动力预测统计,
        周计划派工单维护,
        任务单查询,
        任务单维护
    }

    public class MProductionMng
    {
        private static IProductionManagementSrv productionManagementSrv;
        private static IProObjectRelaDocumentSrv modelDoc;
        private static IPBSTreeSrv model;
        private static ISpecialCostSrv specialCostSrv;

        #region 初始化服务
        public ISpecialCostSrv SpecialCostSrv
        {
            get
            {
                if (specialCostSrv == null)
                {
                    specialCostSrv = StaticMethod.GetService("SpecialCostSrv") as ISpecialCostSrv;
                }
                return specialCostSrv;
            }
            set { MProductionMng.specialCostSrv = value; }
        }

        public IProductionManagementSrv ProductionManagementSrv
        {
            get
            {
                if (productionManagementSrv == null)
                {
                    productionManagementSrv = StaticMethod.GetService("ProductionManagementSrv") as IProductionManagementSrv;
                }
                return productionManagementSrv;
            }
            set { productionManagementSrv = value; }
        }
        #endregion

        public MProductionMng()
        {
            if (modelDoc == null)
                modelDoc = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.ConstMethod.GetService("ProObjectRelaDocumentSrv") as IProObjectRelaDocumentSrv;
            if (model == null)
                model = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.ConstMethod.GetService("PBSTreeSrv") as IPBSTreeSrv;
        }

        /// <summary>
        /// 删除工程对象关联文档对象集合
        /// </summary>
        /// <param name="list">工程对象关联文档对象集合</param>
        /// <returns></returns>
        public bool DeleteProObjRelaDoc(IList list)
        {
            return modelDoc.DeleteProObjRelaDoc(list);
        }

        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return model.ObjectQuery(entityType, oq);
        }

        public DataTable GetCostBudget(string projectid, string sysCode)
        {
            return model.GetCostBudget(projectid, sysCode).Tables[0];
        }

        public bool DeleteByIList(IList lstObj)
        {
            return productionManagementSrv.DeleteByIList(lstObj);
        }
    }
}
