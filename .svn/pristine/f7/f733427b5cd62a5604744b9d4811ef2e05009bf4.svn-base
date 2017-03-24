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
using Application.Resource.CommonClass.Domain;
using Iesi.Collections;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Service
{
    /// <summary>
    /// 工程任务类型服务
    /// </summary>
    public class GWBSDetailCostSubjectSrv : IGWBSDetailCostSubjectSrv
    {
        private IDao dao;
        public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }

        /// <summary>
        /// 保存或修改明细分科目成本
        /// </summary>
        /// <param name="list">科目集合</param>
        /// <returns></returns>
        public IList SaveOrUpdateCostSubject(IList list)
        {
            dao.SaveOrUpdate(list);

            return list;
        }

        /// <summary>
        /// 删除明细分科目成本集合
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DeleteCostSubject(IList list)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (GWBSDetailCostSubject cs in list)
            {
                dis.Add(Expression.Eq("Id", cs.Id));
            }

            list = dao.ObjectQuery(typeof(GWBSDetailCostSubject), oq);

            return dao.Delete(list);
        }
    }
}
