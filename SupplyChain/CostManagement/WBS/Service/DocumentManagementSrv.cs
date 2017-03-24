using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using VirtualMachine.Core;
using VirtualMachine.Patterns.CategoryTreePattern.Service;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using System.Collections;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.Util;
using NHibernate.Criterion;
using VirtualMachine.SystemAspect.Security;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data;
using VirtualMachine.Core.DataAccess;
using Application.Resource.FinancialResource;
using Application.Resource.PersonAndOrganization.OrganizationResource.Service;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.CommonClass.Domain;
using CommonSearchLib.BillCodeMng.Service;
using Application.Business.Erp.SupplyChain.Base.Service;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Service
{
    /// <summary>
    /// 文档对象管理
    /// </summary>
    public class DocumentManagementSrv : BaseService, IDocumentManagementSrv
    {
        /// <summary>
        /// 通过ID查询文档对象管理信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DocumentManagement GetDocumentManagementById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetDocumentManagement(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as DocumentManagement;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询文档对象管理信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DocumentManagement GetDocumentManagementByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetDocumentManagement(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as DocumentManagement;
            }
            return null;
        }

        /// <summary>
        /// 文档对象管理信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetDocumentManagement(ObjectQuery objectQuery)
        {
            return Dao.ObjectQuery(typeof(DocumentManagement), objectQuery);
        }

    }
}
