using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.SupplyManage.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProductionManagement.AcceptanceInspectionMng.Service;
using Application.Business.Erp.SupplyChain.ProductionManagement.AcceptanceInspectionMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.AcceptanceInspectionMng
{
    public class MAcceptanceInspectionMng
    {
        private IAcceptanceInspectionSrv acceptanceInspectionSrv;

        public IAcceptanceInspectionSrv AcceptanceInspectionSrv
        {
            get { return acceptanceInspectionSrv; }
            set { acceptanceInspectionSrv = value; }
        }

        public MAcceptanceInspectionMng()
        {
            if (acceptanceInspectionSrv == null)
            {
                acceptanceInspectionSrv = StaticMethod.GetService("AcceptanceInspectionSrv") as IAcceptanceInspectionSrv;
            }
        }

        #region 验收检查记录
        /// <summary>
        /// 保存验收检查记录
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public AcceptanceInspection SaveandAcceptanceInspection(AcceptanceInspection obj)
        {
            return acceptanceInspectionSrv.SaveAcceptanceInspection(obj);
        }
        #endregion
    }
}
