using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.SupplyManage.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng
{
    public class MSupplyPlanMng
    {
        private ISupplyPlanSrv supplyPlanSrv;
        public ISupplyPlanSrv  SupplyPlanSrv
        {
            get { return supplyPlanSrv; }
            set { supplyPlanSrv = value; }
        }

        public MSupplyPlanMng()
        {
            if (supplyPlanSrv == null)
            {
                supplyPlanSrv = StaticMethod.GetService("SupplyPlanSrv") as ISupplyPlanSrv;
            }
        }

        #region 采购计划
        ///// <summary>
        ///// 保存采购计划信息
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public SupplyPlanMaster SaveSupplyPlanMng(SupplyPlanMaster obj)
        //{
        //    return supplyPlanSrv.SaveSupplyPlan(obj);
        //}

        #endregion

        //#region 料具收料
        //public MaterialCollectionMaster SaveMaterialCollection(MaterialCollectionMaster obj, IList list)
        //{
        //    //return matMngSrv.SaveMaterialCollectionMaster(obj, list);
        //}
        //#endregion
    }
}
