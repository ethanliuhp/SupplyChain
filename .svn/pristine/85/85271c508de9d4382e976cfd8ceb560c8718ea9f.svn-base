using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Service;
using Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
//using Application.Business.Erp.SupplyChain.CostManagement.EndAccountAuditMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng
{
    public class MSubContractBalance
    {
        private ISubContractBalanceBillSrv subBalSrv;
        public ISubContractBalanceBillSrv SubBalSrv
        {
            get { return subBalSrv; }
            set { subBalSrv = value; }
        }

        public MSubContractBalance()
        {
            if (subBalSrv == null)
                subBalSrv = ConstMethod.GetService("SubContractBalanceBillSrv") as ISubContractBalanceBillSrv;
        }


        #region 终结分包相关操作 
        
        /// <summary>
        /// 保存或修改终结分包
        /// </summary>
        /// <param name="list">终结分包</param>
        /// <returns></returns>
        public IList SaveOrUpdateEndAccountAuditBillList(IList list)
        {

            subBalSrv.SaveOrUpdateEndAccountAuditBill(list);
            return list;

        }

        /// <summary>
        /// 保存或修改终结分包
        /// </summary>
        /// <param name="bill">终结分包</param>
        /// <returns></returns>
        public EndAccountAuditBill SaveOrUpdateEndAccountAuditBill(EndAccountAuditBill bill)
        {
            subBalSrv.SaveOrUpdateEndAccountAuditBill(bill);
            return bill;
        }

        /// <summary>
        /// 保存或修改终结分包明细
        /// </summary>
        /// <param name="bill">终结分包明细</param>
        /// <returns></returns>
        public EndAccountAuditDetail SaveOrUpdateEndAccountAuditBillDetail(EndAccountAuditDetail bill)
        {
            subBalSrv.SaveOrUpdateEndAccountAuditBillDetail(bill);
            return bill;
        }
        /// <summary>
        /// 保存或修改终结分包明细
        /// </summary>
        /// <param name="bill">终结分包明细</param>
        /// <returns></returns>
        public IList SaveOrUpdateEndAccountAuditBillDetailList(IList list)
        {
            subBalSrv.SaveOrUpdateEndAccountAuditBillDetailList(list);
            return list;
        }



        public bool DeleteEndAccountAuditBill(EndAccountAuditBill bill)
        {
            return subBalSrv.DeleteEndAccountAuditBill(bill);
        }

        #endregion


        /// <summary>
        /// 保存或修改分包结算单集合
        /// </susubBalSrvary>
        /// <param name="list">分包结算单集合</param>
        /// <returns></returns>
        public IList SaveOrUpdateSubContractBalanceBill(IList list)
        {
            return subBalSrv.SaveOrUpdateEndAccountAuditBillDetailList(list);
        }

        /// <summary>
        /// 保存或修改分包结算单
        /// </summary>
        /// <param name="bill">分包结算单</param>
        /// <returns></returns>
        public SubContractBalanceBill SaveOrUpdateSubContractBalanceBill(SubContractBalanceBill bill)
        {
            return subBalSrv.SaveOrUpdateSubContractBalanceBill(bill);
        }


        /// <summary>
        /// 根据对象类型和GUID获取对象
        /// </summary>
        /// <param name="entityType">对象类型</param>
        /// <param name="Id">GUID</param>
        /// <returns></returns>
        public Object GetObjectById(Type entityType, string Id)
        {
            return subBalSrv.GetObjectById(entityType, Id);
        }

        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return subBalSrv.ObjectQuery(entityType, oq);
        }

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerTime()
        {
            return subBalSrv.GetServerTime();
        }

        /// <summary>
        /// 结算措施费明细
        /// </summary>
        /// <param name="listMeansureDtl">措施费任务明细集</param>
        /// <returns></returns>
        public List<SubContractBalanceDetail> BalanceSubContractFeeDtl(SubContractProject subProject, List<GWBSDetail> listMeansureDtl, List<SubContractBalanceDetail> listCurrBalDtl)
        {
            return subBalSrv.BalanceSubContractFeeDtl(subProject,listMeansureDtl, listCurrBalDtl);
        }
    }
}
