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
using Iesi.Collections;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Service
{
    /// <summary>
    /// 契约组服务
    /// </summary>
    public class ContractGroupSrv : IContractGroupSrv
    {
        private IDao dao;
        public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }

        /// <summary>
        /// 获取编号
        /// </summary>
        /// <returns></returns>
        public string GetContractGroupCode()
        {
            return "QY-" + string.Format("{0:yyyyMMdd}", DateTime.Now);
        }

        /// <summary>
        /// 获取契约组明细编号
        /// </summary>
        /// <returns></returns>
        public string GetContractGroupDetailCode(string contractGroupCode, int detailNum)
        {
            return contractGroupCode + detailNum.ToString().PadLeft(3, '0');
        }

        /// <summary>
        /// 保存或修改契约组集合
        /// </summary>
        /// <param name="list">契约组集合</param>
        /// <returns></returns>
        [TransManager]
        public IList SaveOrUpdateContractGroup(IList list)
        {
            dao.SaveOrUpdate(list);

            return list;
        }

        /// <summary>
        /// 保存或修改契约组
        /// </summary>
        /// <param name="group">契约组</param>
        /// <returns></returns>
        [TransManager]
        public ContractGroup SaveOrUpdateContractGroup(ContractGroup cg)
        {
            dao.SaveOrUpdate(cg);
            return cg;
        }

        /// <summary>
        /// 删除契约组集合
        /// </summary>
        /// <param name="list">契约组集合</param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteContractGroup(IList list)
        {
            foreach (ContractGroup cg in list)
            {
                if (!string.IsNullOrEmpty(cg.Id))
                {
                    object obj = dao.Get(typeof(ContractGroup), cg.Id);
                    if (obj != null)
                        dao.Delete(obj);
                }
            }
            return true;
        }

        /// <summary>
        /// 契约组明细集合
        /// </summary>
        /// <param name="list">契约组明细集合</param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteContractGroupDetail(IList list)
        {
            foreach (ContractGroupDetail dtl in list)
            {
                if (!string.IsNullOrEmpty(dtl.Id))
                {
                    dao.Delete(dao.Get(typeof(ContractGroupDetail), dtl.Id));
                }
            }
            return true;
        }

        /// <summary>
        /// 根据对象类型和GUID获取对象
        /// </summary>
        /// <param name="entityType">对象类型</param>
        /// <param name="Id">GUID</param>
        /// <returns></returns>
        public Object GetObjectById(Type entityType, string Id)
        {
            return dao.Get(entityType, Id);
        }

        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return dao.ObjectQuery(entityType, oq);
        }

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerTime()
        {
            return DateTime.Now;
        }
    }
}
