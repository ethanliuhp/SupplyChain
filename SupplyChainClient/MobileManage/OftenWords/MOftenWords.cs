using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Basic;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Basic.Service;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.OftenWords
{
    public class MOftenWords
    {
        private ICurrentProjectSrv currentProjectSrv;

        public ICurrentProjectSrv CurrentProjectSrv
        {
            get { return currentProjectSrv;}
            set { currentProjectSrv = value; }
        }

        public MOftenWords()
        {
            if (currentProjectSrv == null)
            {
                currentProjectSrv = StaticMethod.GetService("CurrentProjectSrv") as ICurrentProjectSrv;
            }
        }
        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        //public OftenWords SaveWeather(WeatherInfo obj)
        //{
        //    return weatherSrv.SaveWeather(obj);
        //}
    }
}
