using System;
using Application.Business.Erp.SupplyChain.Base.Service;
using System.Collections;
using Application.Resource.FinancialResource.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using System.Data;
using Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain;
namespace Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Service
{
    /// <summary>
    /// 盘盈单服务接口
    /// </summary>
    public interface IProfitInSrv : IBaseService
    {
        /// <summary>
        ///  盘盈单记账
        /// </summary>
        /// <param name="hashLst"></param>
        /// <param name="hashCode"></param>
        /// <param name="tallyDate"></param>
        /// <param name="componentPeriod"></param>
        /// <param name="AuditPerson"></param>
        /// <returns></returns>
        Hashtable Tally(Hashtable hashLst, Hashtable hashCode, DateTime tallyDate, ComponentPeriod componentPeriod, PersonInfo AuditPerson, string projectId);
        /// <summary>
        /// 盘盈查询
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        DataSet ProfitInQuery(string condition);
        ProfitIn SaveProfitIn(ProfitIn oProfitIn);
    }
}
