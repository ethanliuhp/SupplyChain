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

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng
{
    public class MDocumentMng
    {
        private static IProObjectRelaDocumentSrv modelDoc;

        public MDocumentMng()
        {
            if (modelDoc == null)
                modelDoc = ConstMethod.GetService("ProObjectRelaDocumentSrv") as IProObjectRelaDocumentSrv;
        }

        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return modelDoc.ObjectQuery(entityType, oq);
        }

        /// <summary>
        /// 保存或修改对象集合
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <returns></returns>
        public IList SaveOrUpdate(IList list)
        {
            return modelDoc.SaveOrUpdate(list);
        }

        /// <summary>
        /// 保存或修改对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public object SaveOrUpdate(object obj)
        {
            return modelDoc.SaveOrUpdate(obj);
        }

        /// <summary>
        /// 删除工程对象关联文档集合
        /// </summary>
        /// <param name="list">工程对象关联文档集合</param>
        /// <returns></returns>
        public bool DeleteProObjRelaDoc(IList list)
        {
            return modelDoc.DeleteProObjRelaDoc(list);
        }

        /// <summary>
        /// 删除对象集合
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <returns></returns>
        [TransManager]
        public bool Delete(IList list)
        {
            return modelDoc.Delete(list);
        }
    }
}
