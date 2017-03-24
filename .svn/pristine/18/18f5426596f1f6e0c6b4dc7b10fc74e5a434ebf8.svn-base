using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Context;
using System.Data.SqlClient;
using System.Configuration;

namespace PortalIntegrationConsole.CommonClass
{
    public class StaticMethod
    {
        static IApplicationContext springContext = null;

        /// <summary>
        /// 获得服务根据名称
        /// </summary>
        /// <param name="serviceName">服务对象名称</param>
        /// <returns></returns>
        static public object GetService(string accountName,string serviceName)
        {
            if (springContext == null)
            {
                springContext = AppDomain.CurrentDomain.GetData("默认帐套") as IApplicationContext;

                if (springContext == null)
                {
                    springContext = AppDomain.CurrentDomain.GetData(accountName) as IApplicationContext;
                }
            }
            return springContext.GetObject(serviceName);
            //return ServiceFactory.GetServiceByName(serviceName);            
        }
    }
}
