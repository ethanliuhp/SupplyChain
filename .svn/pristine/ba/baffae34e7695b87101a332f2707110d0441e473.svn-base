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
using Application.Business.Erp.SupplyChain.CostManagement.OwnerQuantityMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.OwnerQuantityMng.Service;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.OwnerQuantityMng
{
    public class MOwnerQuantityMng
    {
        private IOwnerQuantitySrv ownerQuantitySrv;

        public IOwnerQuantitySrv OwnerQuantitySrv
        {
            get { return ownerQuantitySrv; }
            set { ownerQuantitySrv = value; }
        }

        public MOwnerQuantityMng()
        {
            if (ownerQuantitySrv == null)
            {
                ownerQuantitySrv = StaticMethod.GetService("OwnerQuantitySrv") as IOwnerQuantitySrv;
            }
        }

        #region 业务报量

        /// <summary>
        /// 保存业务报量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public OwnerQuantityMaster SaveOwnerQuantityMaster(OwnerQuantityMaster obj)
        {
            return ownerQuantitySrv.SaveOwnerQuantity(obj);
        }
        #endregion
    }
}
