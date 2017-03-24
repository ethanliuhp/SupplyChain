using System;
using System.Collections.Generic;
using System.Text;
using Application.Resource.CommonClass.Domain;

namespace Application.Business.Erp.SupplyChain.Client.Util
{
    public class LoginInfomation
    {
        private static Login loginInfo;

        public static Login LoginInfo
        {
            get { return LoginInfomation.loginInfo; }
            set { LoginInfomation.loginInfo = value; }
        }


    }
}
