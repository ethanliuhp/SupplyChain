using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.ProductionManagement.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Service;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Service;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using CommonSearch.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.PMCAndWarning.Service;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.GwbsConfirm
{

    public class MConfirmmng
    {
        private static IProductionManagementSrv productionManagementSrv;
        private IContractExcuteSrv contractExcuteSrv;
        private static IProObjectRelaDocumentSrv modelDoc;
        private static IPBSTreeSrv model;
        private static IPMCAndWarningSrv pmcAndWarningSrv;

        //public static PMCAndWarningSrv PmcAndWarningSrv
        //{
        //    get { return MConfirmmng.pmcAndWarningSrv; }
        //    set { MConfirmmng.pmcAndWarningSrv = value; }
        //}

        public IContractExcuteSrv ContractExcuteSrv
        {
            get { return contractExcuteSrv; }
            set { contractExcuteSrv = value; }
        }

        #region 初始化服务
        public IProductionManagementSrv ProductionManagementSrv
        {
            get
            {
                if (productionManagementSrv == null)
                {
                    productionManagementSrv = StaticMethod.GetService("ProductionManagementSrv") as IProductionManagementSrv;
                }
                return productionManagementSrv;
            }
            set { productionManagementSrv = value; }
        }
        #endregion
        public MConfirmmng()
        {
            if (contractExcuteSrv == null)
            {
                contractExcuteSrv = StaticMethod.GetService("ContractExcuteSrv") as IContractExcuteSrv;
            }
            if (modelDoc == null)
                modelDoc = ConstMethod.GetService("ProObjectRelaDocumentSrv") as IProObjectRelaDocumentSrv;
            if (model == null)
                model = ConstMethod.GetService("PBSTreeSrv") as IPBSTreeSrv;
            if (pmcAndWarningSrv == null)
                pmcAndWarningSrv = ConstMethod.GetService("PMCAndWarningSrv") as IPMCAndWarningSrv;
        }
        #region 分包项目
        /// <summary>
        /// 保存分包项目
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public SubContractProject SaveContracExcuteMaster(SubContractProject obj)
        {
            return contractExcuteSrv.SaveContractExcute(obj);
        }

        #endregion

        /// <summary>
        /// 本队伍已确认累计工程量
        /// </summary>
        /// <param name="info">当前操作项目</param>
        /// <param name="dtl">(操作{工程任务确认明细})_【被确认工程任务明细】</param>
        /// <param name="sub">(操作{工程任务确认明细})_【承担者】</param>
        /// <returns></returns>
        public decimal GetTheTeamQuantityConfirmed(CurrentProjectInfo info, GWBSDetail dtl, SubContractProject sub)
        {
            return productionManagementSrv.GetTheTeamQuantityConfirmed(info, dtl, sub);
        }

        /// <summary>
        /// 选择多个队伍的操作
        /// </summary>
        /// <param name="add"></param>
        /// <param name="update"></param>
        /// <param name="delete"></param>
        /// <returns></returns>
        [TransManager]
        public IList OperatingAfterReturnAddList(IList add, IList update, IList delete)
        {
            if (add != null && add.Count > 0)
                return productionManagementSrv.SaveOrUpdate(add);
            if (update != null && update.Count > 0)
                productionManagementSrv.SaveOrUpdate(update);
            if (delete != null && update.Count > 0)
                productionManagementSrv.DeleteByDao(delete);
            return null;
        }
        public DateTime GetPreviousSundayDate()
        {
            return pmcAndWarningSrv.GetPreviousSundayDate();
        }
    }
}
