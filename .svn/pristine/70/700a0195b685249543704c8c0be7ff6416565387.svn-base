using System;
using System.Collections.Generic;
using System.Text;
using Application.Resource.CommonClass.Domain;
using VirtualMachine.Component.Util;
using System.Runtime.Remoting.Messaging;
using VirtualMachine.SystemAspect.Security.InstanceSecurity.Domain;
using Application.Resource.CommonClass.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Patterns.DataDictionary.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Resource.PersonAndOrganization.SupplierManagement.Service;
using Application.Resource.PersonAndOrganization.ClientManagement.Service;

namespace Application.Business.Erp.SupplyChain.Client
{
    public class ClientAppInitial
    {
        /// <summary>
        /// 在此方法中子模块初始化自己的环境变量
        /// 前置条件:子模块中用到的Context已经初始化放在AppDomain.CurrentDomain中

        /// 在CallContext中已经存放了一个LogInstanceInfo类的实例.描述当前用户
        /// </summary>
        public void Initial()
        {
            BusinessModule a = new BusinessModule();
            a.Id = "3L";
            a.Name = "采购管理";

            ILoginSrv loginSrv = StaticMethod.GetService("LoginSrv") as ILoginSrv;
            LogInstanceInfo logInfo = CallContext.GetData("LogInVM") as LogInstanceInfo;

            Login TheLogin = new Login();
            TheLogin.LoginDate = logInfo.LoginTime;
            TheLogin.TheCurrency = loginSrv.GetBasicCurrencyInfo();
            TheLogin.TheSysRole = loginSrv.GetSysRole(logInfo.LogInRole);
            TheLogin.ThePerson = loginSrv.GetPerson(logInfo.LogInPerson);
            TheLogin.TheBusinessOperators = loginSrv.GetAuthor(logInfo.LogInPerson);
            if (TheLogin.TheSysRole != null && TheLogin.TheSysRole.Id != "")
            {
                TheLogin.TheOperationOrgInfo = loginSrv.GetOperationOrgInfo(TheLogin.TheSysRole.Id);
            }
            TheLogin.TheComponentPeriod = loginSrv.GetFiscalPeriod(a.Id, logInfo.LoginTime);
            TheLogin.FiscalModule = loginSrv.GetFiscalPeriod("7L", logInfo.LoginTime);

            CallContext.LogicalSetData("LoginInformation", TheLogin);
            LoginInfomation.LoginInfo = TheLogin;
            ConstObject.TheLogin = TheLogin;

        }
    }
}
