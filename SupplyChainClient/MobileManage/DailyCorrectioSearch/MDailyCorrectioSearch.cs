using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.ProductionManagement.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.ProductionManagement.RectificationNoticeMng.Service;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.DailyCorrectioSearch
{
    public class MDailyCorrectioSearch
    {
     private static IProductionManagementSrv productionManagementSrv;
        private static IRectificationNoticeSrv rectificationNoticeSrv;
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
        public IRectificationNoticeSrv RectificationNoticeSrv
        {
            get { return rectificationNoticeSrv; }
            set { rectificationNoticeSrv = value; }
        }
        #endregion

        public MDailyCorrectioSearch()
        {
            if (rectificationNoticeSrv == null)
            {
                rectificationNoticeSrv = StaticMethod.GetService("RectificationNoticeSrv") as IRectificationNoticeSrv;
            }
        }
    }
}
