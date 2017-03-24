using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.ProductionManagement.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.WeekSchedules
{
    public class MWeekPlanMng
    {
        private static IProductionManagementSrv productionManagementSrv;

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
       
        public MWeekPlanMng()
        { }

    }
}
