using System;
using Application.Business.Erp.SupplyChain.Base.Service;
using System.Collections;
using VirtualMachine.Core;
using Application.Resource.FinancialResource.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.StockManage.StockLossOut.Domain;
using System.Data;
namespace Application.Business.Erp.SupplyChain.StockManage.StockLossOut.Service
{
    public interface ILossOutSrv : IBaseService
    {
        /// <summary>
        /// 盘亏单记账
        /// </summary>
        /// <param name="hashLst"></param>
        /// <param name="hashCode"></param>
        /// <param name="tallyDate"></param>
        /// <param name="componentPeriod"></param>
        /// <param name="auditPerson"></param>
        /// <returns></returns>
        Hashtable Tally(Hashtable hashLst, Hashtable hashCode, DateTime tallyDate, ComponentPeriod componentPeriod, PersonInfo auditPerson, string projectId);

        /// <summary>
        /// 查询盘亏单
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetLossOut(ObjectQuery oq);

        /// <summary>
        /// 保存盘亏单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        LossOut SaveLossOut(LossOut obj);

        /// <summary>
        /// 删除盘亏单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteLossOut(LossOut obj);
        /// <summary>
        /// 查询盘亏单
        /// </summary>
        /// <param name="sCondition">条件</param>
        /// <returns></returns>
        DataSet LossOutQuery(string sCondition);
    }
}
