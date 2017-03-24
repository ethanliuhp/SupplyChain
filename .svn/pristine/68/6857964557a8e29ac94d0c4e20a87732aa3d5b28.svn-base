using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.SupplyManage.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.BasicData.Service;
using Application.Business.Erp.SupplyChain.BasicData.UnitMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Service;

namespace Application.Business.Erp.SupplyChain.Client.BasicData.UnitMng
{
    public class MUnitMng
    {
        //private static IUnitSrv model;
        private static IGWBSTreeSrv model;
        private IUnitSrv unitSrv;

        public IUnitSrv UnitSrv
        {
            get { return unitSrv; }
            set { unitSrv = value; }
        }

        public MUnitMng()
        {
            if (unitSrv == null)
            {
                unitSrv = StaticMethod.GetService("UnitSrv") as IUnitSrv;
            }
        }

        #region 计量单位
        /// <summary>
        /// 保存计量单位
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public UnitMaster SaveUnitMaster(UnitMaster obj)
        {
            return unitSrv.SaveUnit(obj);
        }

        ///// <summary>
        ///// 获取业务组织节点集合(返回有权限和无权限的集合)
        ///// </summary>
        //public IList GetOpeOrgsByInstance()
        //{
        //    return model.GetOpeOrgsByInstance();
        //}

        #endregion
    }
}
