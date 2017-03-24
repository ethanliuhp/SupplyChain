using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Service;
using Application.Resource.MaterialResource.Domain;
using IRPServiceModel.Services.Common;
using Spring.Context;

namespace Application.Business.Erp.SupplyChain.Client.Basic.CommonClass
{
    public class CommonMethod
    {
        public static DateTime GetServerDateTime()
        {
            IStockInSrv theStockInSrv = StaticMethod.GetService("StockInSrv") as IStockInSrv;
            return theStockInSrv.GetServerDateTime();
        }
        public static Material GetMaterialById(string Id)
        {

            return null;
        }
        public static OperationOrg TransToOpeOrg(OperationOrgInfo orgInfo)
        {
            OperationOrg opeOrg = new OperationOrg();
            opeOrg.Id = orgInfo.Id;
            opeOrg.Name = orgInfo.Name;
            return opeOrg;
        }
        public static OperationOrgInfo TransToOrgInfo(OperationOrg opeOrg)
        {
            OperationOrgInfo orgInfo = new OperationOrgInfo();
            orgInfo.Id = opeOrg.Id;
            orgInfo.Name = opeOrg.Name;
            return orgInfo;
        }
        public static string TransToBig(int index)
        {
            string temp = index + "";
            string str = "";
            if (index < 10)
            {
                str = TransToSingleNumber(index);
            }
            else if (index >= 10 && index < 100)
            {
                if (index == 10)
                {
                    str = "十";
                }
                else if (index > 10 && index < 20)
                {
                    //取第二个字符
                    string secondStr = temp.Substring(1);
                    str = "十" + TransToSingleNumber(ClientUtil.ToInt(secondStr));
                }
                else if (index >= 20 && index < 100)
                {
                    //取第二个字符
                    string firstStr = temp.Substring(0, 1);
                    string secondStr = temp.Substring(1);
                    str = TransToSingleNumber(ClientUtil.ToInt(firstStr)) + "十" + TransToSingleNumber(ClientUtil.ToInt(secondStr));
                }
            }

            return str;
        }
        private static string TransToSingleNumber(int number)
        {
            string str = "";
            switch (number)
            {
                case 1:
                    str = "一";
                    break;
                case 2:
                    str = "二";
                    break;
                case 3:
                    str = "三";
                    break;
                case 4:
                    str = "四";
                    break;
                case 5:
                    str = "五";
                    break;
                case 6:
                    str = "六";
                    break;
                case 7:
                    str = "七";
                    break;
                case 8:
                    str = "八";
                    break;
                case 9:
                    str = "九";
                    break;
            }
            return str;
        }
        public static bool VeryValid(string value)
        {
            string valid = "0123456789,.-";
            string temp;
            for (int i = 0; i < value.Length; i++)
            {
                temp = value.Substring(i, 1);
                if (valid.IndexOf(temp) < 0) return false;
            }
            return true;
            //bool valid = true;
            //foreach (char ch in value)
            //{
            //    if (ch < 48 || ch > 57)
            //    {
            //        valid = false;
            //        return valid;
            //    }
            //    else
            //        valid = true;
            //}
            //return valid;
        }
        private static ICommonMethodSrv commonMethodSrv;
        /// <summary>
        /// 公共服务
        /// </summary>
        public static ICommonMethodSrv CommonMethodSrv
        {
            get
            {
                if (commonMethodSrv == null)
                {
                    commonMethodSrv = (AppDomain.CurrentDomain.GetData("IRPServiceModel") as IApplicationContext).GetObject("CommonMethodSrv") as ICommonMethodSrv;
                }
                return commonMethodSrv;
            }
        }
    }
}
