using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.FinanceMng.Service;

namespace Application.Business.Erp.SupplyChain.Client.FinanceMng.DelimitIndividualBillMng
{
    public class MDelimitIndividualBillMng
    {
        private IDelimitIndividualBillSrv delimitIndividualBillSrv;

        public IDelimitIndividualBillSrv DelimitIndividualBillSrv
        {
            get { return delimitIndividualBillSrv; }
            set { delimitIndividualBillSrv = value; }
        }

        public MDelimitIndividualBillMng()
        {
            if (delimitIndividualBillSrv == null)
            {
                delimitIndividualBillSrv = StaticMethod.GetService("DelimitIndividualBillSrv") as IDelimitIndividualBillSrv;
            }
        }

        //#region 需求总计划
        ///// <summary>
        ///// 保存需求总计划
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public DelimitIndividualBill SaveDelimitIndividualBill(DelimitIndividualBill obj)
        //{
        //    return delimitIndividualBillSrv.SaveDelimitIndividualBill(obj);
        //}

        //#endregion

        //#region 料具收料
        //public MaterialCollectionMaster SaveMaterialCollection(MaterialCollectionMaster obj, IList list)
        //{
        //    //return matMngSrv.SaveMaterialCollectionMaster(obj, list);
        //}
        //#endregion
    }
}
