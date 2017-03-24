using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Application.Business.Erp.SupplyChain.GuestCompany.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.BasicData.GusetCompanyMng
{
    public class MGuestCompany
    {
        private static IGuestCompanySrv GuestCompanySrv = null;
        public MGuestCompany()
        {
            if (GuestCompanySrv == null)
            {
                GuestCompanySrv = StaticMethod.GetService("GuestCompanySrv") as IGuestCompanySrv;
            }
        }
        /// <summary>
        /// 获取客商信息
        /// </summary>
        /// <returns></returns>
        public IList GetGuestComMess()
        {
            return GuestCompanySrv.GetGuestComMess();
        }
    }
}
