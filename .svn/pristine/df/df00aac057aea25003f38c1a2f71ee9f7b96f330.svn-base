using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

using VirtualMachine.Core;
using NHibernate.Criterion;

using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.Service;
using Application.Resource.PersonAndOrganization.OrganizationResource.Service;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.FinancialResource;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Service;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Service;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.PMCAndWarning.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.PMCAndWarning
{
    public class MPMCAndWarning
    {
        private static IPMCAndWarningSrv model;

        public IPMCAndWarningSrv PMCAndWarningSrv
        {
            get { return model; }
        }

        public MPMCAndWarning()
        {
            if (model == null)
            {
                model = StaticMethod.GetService("PMCAndWarningSrv") as IPMCAndWarningSrv;
                //model = ConstMethod.GetService("PMCAndWarningSrv") as IPMCAndWarningSrv;
            }
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


        /// <summary>
        /// 根据对象类型和GUID获取对象
        /// </summary>
        /// <param name="entityType">对象类型</param>
        /// <param name="Id">GUID</param>
        /// <returns></returns>
        public Object GetObjectById(Type entityType, string Id)
        {
            return model.GetObjectById(entityType, Id);
        }

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerTime()
        {
            return model.GetServerTime();
        }

        /// <summary>
        /// 保存或修改对象集合
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>        
        public IList SaveOrUpdate(IList list)
        {
            return model.SaveOrUpdate(list);
        }

        /// <summary>
        /// 保存或修改对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public object SaveOrUpdate(object obj)
        {
            return model.SaveOrUpdate(obj);
        }

        /// <summary>
        /// 删除对象或对象集合
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool DeleteObjList(IList list)
        {
            return model.DeleteObjList(list);
        }

        /// <summary>
        /// 启动预警服务
        /// </summary>
        /// <returns></returns>
        public bool StartWarningServer()
        {
            return model.StartWarningServer();
        }

        /// <summary>
        /// 停止预警服务
        /// </summary>
        /// <returns></returns>
        public bool StopWarningServer()
        {
            return model.StopWarningServer();
        }
    }
}
