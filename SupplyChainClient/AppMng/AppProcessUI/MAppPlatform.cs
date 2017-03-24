using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using VirtualMachine.Core;
using System;
using Application.Business.Erp.SupplyChain.ApprovalMng.AppSolutionMng.Service;
using Application.Business.Erp.SupplyChain.Approval.AppProcessMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using CommonSearch.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatformUI
{
    public class MAppPlatform
    {
        private static IAppSrv service = null;
        private static IPBSTreeSrv model;
        public IAppSrv Service
        {
            get { return service; }
            set { service = value; }
        }
        public MAppPlatform()
        {
            if (service == null)
            {
                service = StaticMethod.GetService("RefAppSrv") as IAppSrv;
            }
            if (model == null)
                model = ConstMethod.GetService("PBSTreeSrv") as IPBSTreeSrv;
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
        public IList Save(IList lst)
        {
            return service.Save(lst);
        }
        public IList GetObjects(Type t, ObjectQuery oq)
        {
            return service.GetObjects(t, oq);
        }
        public bool Delete(IList lst)
        {
            return service.Delete(lst);
        }

        public IList GetOpeOrgsByInstance()
        {
            return service.GetOpeOrgsByInstance();
        }

        public object GetDomain(Type t, ObjectQuery l)
        {
            return service.GetDomain(t, l);
        }
        public IList GetDomainByCondition(string ClassName, ObjectQuery oq)
        {
            return service.GetDomainByCondition(ClassName, oq);
        }
        public IList GetDetailProperties(string DetailClassName)
        {
            return service.GetDetailProperties(DetailClassName);
        }
        public IList GetMasterProperties(string MasterClassName)
        {
            return service.GetMasterProperties(MasterClassName);
        }

        public IList GetAppMasterProperties(string parentId)
        {
            return service.GetAppMasterProperties(parentId);
        }

        public IList GetAppDetailProperties(string parentId)
        {
            return service.GetAppDetailProperties(parentId);
        }
    }
}