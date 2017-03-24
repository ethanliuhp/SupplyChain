using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.ConcreteManage.PouringNoteMng.Domain;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.ConcreteManage.PumpingPoundsMng.Domain;
using Application.Business.Erp.SupplyChain.ConcreteManage.ConcreteCheckingMng.Domain;
using Application.Business.Erp.SupplyChain.ConcreteManage.ConcreteBalanceMng.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain;

namespace Application.Business.Erp.SupplyChain.ConcreteManage.Service
{
    /// <summary>
    /// 商品砼管理服务
    /// </summary>
    public interface IConcreteManSrv : IBaseService
    {
        #region 商品砼浇筑记录单方法
        PouringNoteMaster SavePouringNoteMaster(PouringNoteMaster obj, IList moveDtlList);
        bool DeletePouringNoteMaster(PouringNoteMaster obj);
        PouringNoteMaster GetPouringNoteMasterById(string id);
        PouringNoteMaster GetPouringNoteMasterByCode(string code);
        DailyPlanDetail GetDailyPlanDetail(string dailyPlanDetailId);
        IList GetPouringNoteMaster(ObjectQuery objectQuery);
        IList GetPouringNoteDetail(ObjectQuery objectQuery);
        DataSet GetPouringNoteMasterQuery(string condition);
        decimal GetPumpMoneyByContract(string supplyRelId);
        #endregion

        #region 抽磅单方法
        PumpingPoundsMaster SavePumpingPoundsMaster(PumpingPoundsMaster obj);
        PumpingPoundsMaster GetPumpingPoundsMasterById(string id);
        PumpingPoundsMaster GetPumpingPoundsMasterByCode(string code);
        IList GetPumpingPoundsMaster(ObjectQuery objectQuery);
        DataSet GetPumpingPoundsMasterQuery(string condition);
        #endregion

        #region 商品砼对账单
        //ConcreteCheckingMaster SaveConCheckingMaster(ConcreteCheckingMaster obj);
        ConcreteCheckingMaster SaveConcreteCheck(ConcreteCheckingMaster obj);
        bool DeleteConCheckMaster(ConcreteCheckingMaster obj);
        ConcreteCheckingMaster GetConCheckingMasterById(string id);
        ConcreteCheckingMaster GetConCheckingMasterByCode(string code);
        IList GetConCheckingMaster(ObjectQuery objectQuery);
        DataSet GetConCheckingMasterQuery(string condition);
        ConcreteCheckingDetail GetConCkeckingDetailById(string detailId);
        string GetLastEndDate(SupplierRelationInfo theSupplier);
        #endregion

        #region 商品砼结算单
        ConcreteBalanceMaster SaveConcreteBalanceMaster(ConcreteBalanceMaster obj, IList movedDtlList);
        ConcreteBalanceMaster GetConcreteBalanceMasterById(string id);
        ConcreteBalanceMaster GetConcreteBalanceMasterByCode(string code);
        IList GetConcreteBalanceMaster(ObjectQuery objectQuery);
        bool DeleteConcreteBalance(ConcreteBalanceMaster obj);
        void TallyConcreteBalance(ConcreteBalanceMaster obj, string projectId);
        DataSet GetConereteBalanceByQuery(string condition);
        void TallyConcreteBalanceByApproval(string billId);
        decimal GetAddSumMoney(string projectID, string supprelid);
        /// <summary>
        /// 根据对账单明细的ID查找结算单
        /// </summary>
        /// <param name="ConcreteCheckingDetailID"></param>
        /// <returns></returns>
        ConcreteBalanceDetail GetConcreteBalanceDetailByConcreteCheckingDetailID(string ConcreteCheckingDetailID);
        #endregion

        #region 商品砼结算红单
        ConcreteBalanceRedMaster SaveConcreteBalanceRedMaster(ConcreteBalanceRedMaster obj, IList movedDtlList);
        ConcreteBalanceRedMaster GetConcreteBalanceRedMasterById(string id);
        ConcreteBalanceRedMaster GetConcreteBalanceRedMasterByCode(string code);
        IList GetConcreteBalanceRedMaster(ObjectQuery objectQuery);
        ConcreteBalanceDetail GetConBalDetailById(string detailId);
        bool DeleteConcreteBalanceRed(ConcreteBalanceRedMaster obj);
        void TallyConcreteBalanceRed(ConcreteBalanceRedMaster obj, string projectId);
        #endregion

        IList GetFiscalInfo(DateTime createDate);

    }
}
