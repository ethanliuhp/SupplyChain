using System;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using Spring.Context.Support;
using Application.Resource.FinancialResource.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using System.Windows.Forms;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Collections.Specialized;
using System.Data.OleDb;
using Application.Resource.CommonClass.Domain;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Properties;
using System.Diagnostics;
using System.IO;
using Spring.Context;
using VirtualMachine.Component.Util;
using System.Xml;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.BasicData.BaleNoMng.Service;
using Application.Business.Erp.SupplyChain.BasicData.BillUserMng.Domain;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using VirtualMachine.Core.DataAccess;
//using Application.Business.Erp.SupplyChain.ItemMng.Service;
using System.Configuration;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Service;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.NetworkInformation;
using CommonSearchLib.BillCodeMng.Service;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.PLMWebServices;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;

using System.Linq;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Service;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using IRPServiceModel.Domain.Document;

namespace Application.Business.Erp.SupplyChain.Client.Basic.CommonClass
{
    public class ConstObject
    {
        private static Login theLogin = null;
        static CurrencyInfo standardCurrency;
        static PersonInfo loginPersonInfo;
        private static DateTime loginDate;
        private static SysRole theSysRole;
        private static List<OperationRole> roles = null;
        private static OperationOrgInfo theOperationOrg;
        private static bool _ByPrintSpec = false;
        private static bool _FrameWorkNewFlag = false;
        private static string systemId = "";
        private static string _IRPMenuName = "";
        private static string autoSwitchProjectId;
        private static float formScale = 1;

        private static IBillUserSrv baseSrv = null;
        /// <summary>
        /// 基础服务
        /// </summary>
        public static IBillUserSrv BaseSrv
        {
            get
            {
                if (baseSrv == null)
                    baseSrv = StaticMethod.GetService("BillUserSrv") as IBillUserSrv;

                return baseSrv;
            }
            set { baseSrv = value; }
        }



        /// <summary>
        /// 自动切换传递的项目部ID \n
        /// 
        /// </summary>
        public static string AutoSwitchProjectId
        {
            get { return autoSwitchProjectId; }
            set { autoSwitchProjectId = value; }
        }

        /// <summary>
        /// IRP传递的菜单名称
        /// </summary>
        public static string IRPMenuName
        {
            get { return _IRPMenuName; }
            set { _IRPMenuName = value; }
        }

        /// <summary>
        /// 登录系统帐套
        /// </summary>
        public static string TheSystemCode
        {
            get
            {
                return StaticMethod.GetProjectInfo().Id;
            }
            set { systemId = value; }
        }

        static public bool FrameWorkNewFlag
        {
            get
            {
                _FrameWorkNewFlag = ClientUtil.ToBool(ConfigurationManager.AppSettings["FrameWorkNewFlag"]);
                return _FrameWorkNewFlag;
            }
        }

        public static void SendMsg(string BillCode, string ActionCode, string msgContent)
        {
            IBillUserSrv BillUserSrv = StaticMethod.GetService("BillUserSrv") as IBillUserSrv;
            IList toUserCode = new ArrayList();
            IList lst = BillUserSrv.GetObjects(BillCode);
            if (lst.Count > 0)
            {
                BillUser BillUser = lst[0] as BillUser;

                foreach (BillAction BillAction in BillUser.Actions)
                {
                    if (!BillAction.Code.Equals(ActionCode)) continue;
                    foreach (BillPersonInfor BillPersonInfor in BillAction.Persons)
                    {
                        if (BillPersonInfor.PersonInfor != null)
                        {
                            toUserCode.Add(BillPersonInfor.PersonInfor.Code);
                        }
                    }
                    break;
                }

                if (VStartPage.MsgMainForm != null && toUserCode.Count > 0)
                    VStartPage.MsgMainForm.SendMsg(ConstObject.TheLogin.ThePerson.Code, toUserCode, msgContent, BillUser.IsAuto);
            }
        }
        public static void SendMsg(string BillCode, string ActionCode, string msgContent, IList toUserCode)
        {
            IBillUserSrv BillUserSrv = StaticMethod.GetService("BillUserSrv") as IBillUserSrv;
            IList lst = BillUserSrv.GetObjects(BillCode);
            if (lst.Count > 0)
            {
                BillUser BillUser = lst[0] as BillUser;

                foreach (BillAction BillAction in BillUser.Actions)
                {
                    if (!BillAction.Code.Equals(ActionCode)) continue;
                    foreach (BillPersonInfor BillPersonInfor in BillAction.Persons)
                    {
                        if (BillPersonInfor.PersonInfor != null)
                        {
                        }
                    }
                    break;
                }

                if (VStartPage.MsgMainForm != null)
                    VStartPage.MsgMainForm.SendMsg(ConstObject.TheLogin.ThePerson.Code, toUserCode, msgContent, BillUser.IsAuto);
            }
        }

        public static void SendMsg()
        {
            if (VStartPage.MsgMainForm != null)
                VStartPage.MsgMainForm.SendMsg(ConstObject.TheLogin.ThePerson.Code, null, "", false);
        }
        /// <summary>
        /// 按打印规格
        /// </summary>
        public static bool ByPrintSpec
        {
            get { return _ByPrintSpec; }
            set { _ByPrintSpec = value; }
        }

        static public bool IsUpdateProgramAuto
        {
            get
            {
                string aa = System.Configuration.ConfigurationManager.AppSettings["IsUpdateProgramAuto"];
                bool bb = false;
                bb = ClientUtil.ToBool(bb);
                return bb;
            }
        }
        

        public static Login TheLogin
        {
            get
            {
                theLogin = (AppDomain.CurrentDomain.GetData("TheLogin")) as Login;
                return theLogin;
            }
            set
            {
                theLogin = value;
            }
        }
        /// <summary>
        /// 业务部门
        /// </summary>
        public static OperationOrgInfo TheOperationOrg
        {
            get
            {
                if (theLogin != null && theLogin.TheOperationOrgInfo != null && theLogin.TheOperationOrgInfo.Id != "")
                {
                    theOperationOrg = theLogin.TheOperationOrgInfo;
                }
                return theOperationOrg;
            }
            set { theOperationOrg = value; }
        }
        /// <summary>
        /// 岗位
        /// </summary>
        public static SysRole TheSysRole
        {
            get
            {
                if (theLogin != null && theLogin.TheSysRole != null && theLogin.TheSysRole.Id != "")
                {
                    theSysRole = theLogin.TheSysRole;
                }
                return theSysRole;
            }
            set { theSysRole = value; }
        }


        /// <summary>
        /// 角色
        /// </summary>
        public static List<OperationRole> TheRoles
        {
            get
            {
                if (roles == null || roles.Count == 0)
                {
                    if (roles == null)
                        roles = new List<OperationRole>();

                    if (theLogin.TheRoles != null && theLogin.TheRoles.Count > 0)
                    {
                        roles.AddRange(theLogin.TheRoles.OfType<OperationRole>());
                    }
                    else if (theLogin != null && theLogin.TheSysRole != null && !string.IsNullOrEmpty(theLogin.TheSysRole.Id)
                        && BaseSrv != null)
                    {
                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("OperationJob.Id", theLogin.TheSysRole.Id));
                        oq.AddFetchMode("OperationRole", NHibernate.FetchMode.Eager);

                        IList list = BaseSrv.GetObjects(typeof(OperationJobWithRole), oq);
                        foreach (OperationJobWithRole rela in list)
                        {
                            roles.Add(rela.OperationRole);
                        }
                    }
                }
                return roles;
            }
            set { roles = value; }
        }

        /// <summary>
        /// 是否是有系统管理员角色
        /// </summary>
        /// <returns></returns>
        private static bool IsSystemAdministratorRole()
        {
            bool isAdmin = false;
            if (ConstObject.TheRoles != null && ConstObject.TheRoles.Count > 0)
            {
                var query = from r in ConstObject.TheRoles
                            where r.State == 1 && r.RoleName == "系统管理员"
                            select r;

                if (query.Count() > 0)
                    isAdmin = true;
            }
            return isAdmin;
        }

        /// <summary>
        /// 是否是系统管理员岗位
        /// </summary>
        /// <returns></returns>
        private static bool IsSystemAdministratorJob()
        {
            bool isAdmin = false;
            if (ConstObject.TheSysRole != null && ConstObject.TheSysRole.State == 1 && ConstObject.TheSysRole.RoleName == "系统管理员")
            {
                isAdmin = true;
            }
            return isAdmin;
        }

        /// <summary>
        /// 是否是系统管理员
        /// </summary>
        /// <returns></returns>
        public static bool IsSystemAdministrator()
        {
            return (IsSystemAdministratorJob() || IsSystemAdministratorRole());
        }

        /// <summary>
        /// 移动设备窗口缩放比例
        /// 
        /// </summary>
        public static float FormScale
        {
            get
            {
                if (theLogin != null && theLogin.FormScale != null)
                {
                    formScale = theLogin.FormScale;
                }
                return formScale;
            }
            set { formScale = value; }
        }
        /// <summary>
        /// 登录日期
        /// </summary>
        public static DateTime LoginDate
        {
            get
            {
                if (theLogin != null && theLogin.LoginDate != null)
                {
                    loginDate = theLogin.LoginDate;
                }
                return loginDate;
            }
            set { loginDate = value; }
        }
        /// <summary>
        /// 本位币
        /// </summary>
        public static CurrencyInfo StandardCurrency
        {
            get { return standardCurrency; }
            set { standardCurrency = value; }
        }
        /// <summary>
        /// 登录人员
        /// </summary>
        public static PersonInfo LoginPersonInfo
        {
            get
            {
                if (theLogin != null && theLogin.ThePerson != null && theLogin.ThePerson.Id != "")
                {
                    loginPersonInfo = theLogin.ThePerson;
                }
                return loginPersonInfo;
            }
            set { loginPersonInfo = value; }
        }

        private static XmlApplicationContext ctx = null;

        public static XmlApplicationContext Ctx
        {
            get
            {

                if (ctx == null)
                    throw new Exception("AppContext未进行初始化！");
                return ctx;
            }
            set { ctx = value; }
        }

        public static string Connection()
        {
            string result = "";
            string serverIP = System.Configuration.ConfigurationManager.AppSettings["ProIp"];
            Ping p = new Ping();//创建Ping对象p 
            PingReply pr = p.Send(serverIP);//向指定IP或者主机名的计算机发送ICMP协议的ping数据包 
            //return pr.Status;
            if (pr.Status == IPStatus.Success)//如果ping成功             
            {
                result = "连接";
            }
            else
            {
                //Thread.Sleep(100000);//等待十分钟(方便测试的话，你可以改为1000)
                result = "断开";
            }
            return result;
        }
        /// <summary>
        /// 检查服务器与主机联接状态
        /// </summary>
        /// <returns></returns>
        public static string CmdPing()
        {
            string strIp = System.Configuration.ConfigurationManager.AppSettings["ProIp"];
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine("ping -n 1 " + strIp);
            p.StandardInput.WriteLine("exit");
            string strRst = p.StandardOutput.ReadToEnd();
            string pingrst = ProcessPingResultNew(strRst);
            p.Close();
            return pingrst;
        }

        private static string ProcessPingResult(string strRst)
        {
            var pingrst = "";
            if (strRst.IndexOf("(0% loss)") != -1 || strRst.IndexOf("(0% 丢失)") != -1)
            {
                string s = "time=";
                string s1 = "时间=";
                int index = 0;
                int length = 0;
                for (int i = 0; i < strRst.Length; i++)
                {
                    if (strRst.Substring(i, s.Length) == s || strRst.Substring(i, s1.Length) == s1)
                    {
                        if (strRst.Substring(i, s.Length) == s)
                        {
                            index = i + s.Length;
                        }
                        else
                        {
                            index = i + s1.Length;
                        }
                    }
                    if (strRst.Substring(i, 2) == "ms")
                    {
                        length = i - index;
                        break;
                    }
                }
                int time = Convert.ToInt32(strRst.Substring(index, length));
                if (time <= 30 && time > 0)
                {
                    pingrst = "强";
                }
                else if (time < 100)
                {
                    pingrst = "中";
                }
                else
                {
                    pingrst = "弱";
                }
                //pingrst = "连接";
            }
            else if (strRst.IndexOf("Destination host unreachable.") != -1)
                pingrst = "无法连接主机";
            else if (strRst.IndexOf("Request timed out.") != -1)
                pingrst = "连接超时";
            else if (strRst.IndexOf("Unknown host") != -1)
                pingrst = "无法解析主机";
            else
                pingrst = "断开";

            return pingrst;
        }

        private static string ProcessPingResultNew(string strRst)
        {
            var pingrst = "";

            var lines = strRst.Split(new char[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (lines.Count == 5)
            {
                pingrst = lines[3].ToLower().Replace("ping", "").Trim();
            }
            else if (lines.Count > 5)
            {
                pingrst = lines[4];
                var endIndex = pingrst.IndexOf("ms");
                if (endIndex > 0)
                {
                    var startIndex1 = Math.Max(pingrst.LastIndexOf("time="), pingrst.LastIndexOf("time<")) + 5;
                    var startIndex2 = Math.Max(pingrst.LastIndexOf("时间="), pingrst.LastIndexOf("时间<")) + 3;
                    var startIndex = Math.Max(startIndex1, startIndex2);
                    var time = Convert.ToInt32(pingrst.Substring(startIndex, endIndex - startIndex));
                    if (time <= 30 && time > 0)
                    {
                        pingrst = "强";
                    }
                    else if (time < 100)
                    {
                        pingrst = "中";
                    }
                    else
                    {
                        pingrst = "弱";
                    }
                }
            }

            return pingrst;
        }

        /// <summary>
        /// 获取单据状态
        /// </summary>
        /// <param name="oBillState"></param>
        /// <returns></returns>
        public static string BillState(VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState oBillState)
        {

            string sState = string.Empty;
            switch (oBillState)
            {
                case VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit:
                    {
                        sState = "编辑";
                        break;
                    }
                case VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Valid:
                    {
                        sState = "有效";
                        break;
                    }
                case VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Invalid:
                    {
                        sState = "无效";
                        break;
                    }
                case VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InAudit:
                    {
                        sState = "审批中";
                        break;
                    }
                case VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Suspend:
                    {
                        sState = "挂起";
                        break;
                    }
                case VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute:
                    {
                        sState = "执行中";
                        break;
                    }
                case VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Freeze:
                    {
                        sState = "冻结";
                        break;
                    }
                case VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Completed:
                    {
                        sState = "结束";
                        break;
                    }
                default:
                    {
                        sState = "未知";
                        break;
                    }

            }
            return sState;
        }
    }

    /// <summary>
    /// 金额相关函数
    /// </summary>
    public class CurrencyComUtil
    {
        /// <summary>
        /// 根据字符转换成大写金额
        /// </summary>
        /// <param name="moneyStr">金额字串</param>
        /// <returns>string</returns>
        public static string GetMoneyChinese(string moneyStr)
        {
            decimal monDec = 0;
            try
            {
                monDec = ClientUtil.ToDecimal(moneyStr);
            }
            catch { }
            return GetMoneyChinese(monDec);
        }

        /// <summary>
        /// 根据金额转换成大写金额
        /// </summary>
        /// <param name="Money">金额</param>
        /// <returns>string</returns>
        public static string GetMoneyChinese(decimal Money)
        {
            int i;
            string mstrSource;

            if (Money == 0)
            {
                return "";
            }
            mstrSource = Math.Abs(Money).ToString("#0.00");
            i = mstrSource.IndexOf(".");
            if (i > 0)
            {
                mstrSource = mstrSource.Replace(".", "");
            }
            if (mstrSource.Substring(0, 1) == "0")
            {
                mstrSource = mstrSource.Remove(0, 1);
            }

            mstrSource = NumstrToChinese(mstrSource);
            if (mstrSource.Length == 0)
            {
                return "";
            }

            mstrSource = mstrSource.Replace("0", "零");
            mstrSource = mstrSource.Replace("1", "壹");
            mstrSource = mstrSource.Replace("2", "贰");
            mstrSource = mstrSource.Replace("3", "叁");
            mstrSource = mstrSource.Replace("4", "肆");
            mstrSource = mstrSource.Replace("5", "伍");
            mstrSource = mstrSource.Replace("6", "陆");
            mstrSource = mstrSource.Replace("7", "柒");
            mstrSource = mstrSource.Replace("8", "捌");
            mstrSource = mstrSource.Replace("9", "玖");
            mstrSource = mstrSource.Replace("M", "亿");
            mstrSource = mstrSource.Replace("W", "万");
            mstrSource = mstrSource.Replace("S", "仟");
            mstrSource = mstrSource.Replace("H", "佰");
            mstrSource = mstrSource.Replace("T", "拾");
            mstrSource = mstrSource.Replace("Y", "圆");
            mstrSource = mstrSource.Replace("J", "角");
            mstrSource = mstrSource.Replace("F", "分");
            if (mstrSource.Substring(mstrSource.Length - 1, 1) != "分")
            {
                mstrSource = mstrSource + "整";
                //switch (mstrSource.Substring(mstrSource.Length - 1, 1))
                //{
                //    case "角":
                //        mstrSource = mstrSource + "零分";
                //        break;
                //    case "圆":
                //        mstrSource = mstrSource + "零角零分";
                //        break;
                //    case "拾":
                //        mstrSource = mstrSource + "零圆零角零分";
                //        break;
                //    case "佰":
                //        mstrSource = mstrSource + "零拾零圆零角零分";
                //        break;
                //    case "仟":
                //        mstrSource = mstrSource + "零佰零拾零圆零角零分";
                //        break;
                //    case "万":
                //        mstrSource = mstrSource + "零仟零佰零拾零圆零角零分";
                //        break;


                //    default:
                //        break;
                //}
            }

            //负
            if (Money < 0)
            {
                mstrSource = "负" + mstrSource;
            }
            return mstrSource;
        }

        //转换数字
        private char CharToNum(char x)
        {
            string stringChnNames = "零一二三四五六七八九";
            string stringNumNames = "0123456789";
            return stringChnNames[stringNumNames.IndexOf(x)];
        }

        //转换万以下整数
        private string WanStrToInt(string x)
        {
            string[] stringArrayLevelNames = new string[4] { "", "十", "百", "千" };
            string ret = "";
            int i;
            for (i = x.Length - 1; i >= 0; i--)
                if (x[i] == '0')
                {
                    ret = CharToNum(x[i]) + ret;
                }
                else
                {
                    ret = CharToNum(x[i]) + stringArrayLevelNames[x.Length - 1 - i] + ret;
                }
            while ((i = ret.IndexOf("零零")) != -1)
            {
                ret = ret.Remove(i, 1);
            }
            if (ret[ret.Length - 1] == '零' && ret.Length > 1)
            {
                ret = ret.Remove(ret.Length - 1, 1);
            }
            if (ret.Length >= 2 && ret.Substring(0, 2) == "一十")
            {
                ret = ret.Remove(0, 1);
            }
            return ret;
        }
        //转换整数
        private string StrToInt(string x)
        {
            int len = x.Length;
            string ret, temp;
            if (len <= 4)
            {
                ret = WanStrToInt(x);
            }
            else if (len <= 8)
            {
                ret = WanStrToInt(x.Substring(0, len - 4)) + "万";
                temp = WanStrToInt(x.Substring(len - 4, 4));
                if (temp.IndexOf("千") == -1 && temp != "")
                    ret += "零" + temp;
                else
                    ret += temp;
            }
            else
            {
                ret = WanStrToInt(x.Substring(0, len - 8)) + "亿";
                temp = WanStrToInt(x.Substring(len - 8, 4));
                if (temp.IndexOf("千") == -1 && temp != "")
                {
                    ret += "零" + temp;
                }
                else
                {
                    ret += temp;
                }
                ret += "万";
                temp = WanStrToInt(x.Substring(len - 4, 4));
                if (temp.IndexOf("千") == -1 && temp != "")
                {
                    ret += "零" + temp;
                }
                else
                {
                    ret += temp;
                }

            }
            int i;
            if ((i = ret.IndexOf("零万")) != -1)
            {
                ret = ret.Remove(i + 1, 1);
            }
            while ((i = ret.IndexOf("零零")) != -1)
            {
                ret = ret.Remove(i, 1);
            }
            if (ret[ret.Length - 1] == '零' && ret.Length > 1)
            {
                ret = ret.Remove(ret.Length - 1, 1);
            }
            return ret;
        }
        //转换小数
        private string StrToDouble(string x)
        {
            string ret = "";
            for (int i = 0; i < x.Length; i++)
            {
                ret += CharToNum(x[i]);
            }
            return ret;
        }

        private string NumToChn(string x)
        {
            if (x.Length == 0)
            {
                return "";
            }
            string ret = "";
            if (x[0] == '-')
            {
                ret = "负";
                x = x.Remove(0, 1);
            }
            if (x[0].ToString() == ".")
            {
                x = "0" + x;
            }
            if (x[x.Length - 1].ToString() == ".")
            {
                x = x.Remove(x.Length - 1, 1);
            }
            if (x.IndexOf(".") > -1)
            {
                ret += StrToInt(x.Substring(0, x.IndexOf("."))) + "点" + StrToDouble(x.Substring(x.IndexOf(".") + 1));
            }
            else
            {
                ret += StrToInt(x);
            }
            return ret;
        }

        //金额转换
        private static string NumstrToChinese(string numstr)
        {
            int i;
            int j;
            string mstrChar;
            string[] mstrFlag = new string[4];
            string mstrReturn = "";
            bool mblnAddzero = false;

            mstrFlag[0] = "";
            mstrFlag[1] = "T";
            mstrFlag[2] = "H";
            mstrFlag[3] = "S";

            for (i = 1; i <= numstr.Length; i++)
            {
                j = numstr.Length - i;
                mstrChar = numstr.Substring(i - 1, 1);
                if (mstrChar != "0" && j > 1) { mstrReturn = mstrReturn + mstrChar + mstrFlag[(j - 2) % 4]; }
                if (mstrChar == "0" && mblnAddzero == false)
                {
                    mstrReturn = mstrReturn + "0";
                    mblnAddzero = true;
                }
                if (j == 14)
                {
                    if (mstrReturn.Substring(mstrReturn.Length - 1) == "0")
                    { mstrReturn = mstrReturn.Substring(0, mstrReturn.Length - 1) + "W0"; }
                    else
                    { mstrReturn = mstrReturn + "W"; }
                }
                if (j == 2)
                {
                    if (mstrReturn.Substring(mstrReturn.Length - 1, 1) == "0")
                    { mstrReturn = mstrReturn.Substring(0, mstrReturn.Length - 1) + "Y0"; }
                    else
                    { mstrReturn = mstrReturn + "Y"; }
                    //元
                }
                if (j == 6)
                {
                    if (mstrReturn.Length > 2)
                    {
                        if (mstrReturn.Substring(mstrReturn.Length - 2) != "M0")
                        {
                            if (mstrReturn.Substring(mstrReturn.Length - 1) == "0")
                            { mstrReturn = mstrReturn.Substring(0, mstrReturn.Length - 1) + "W0"; }
                            else
                            { mstrReturn = mstrReturn + "W"; }
                        }
                    }
                    else
                    {
                        if (mstrReturn.Substring(mstrReturn.Length - 1) == "0")
                        { mstrReturn = mstrReturn.Substring(0, mstrReturn.Length - 1) + "W0"; }
                        else
                        { mstrReturn = mstrReturn + "W"; }
                    }
                }
                if (j == 10)
                {
                    if (mstrReturn.Substring(mstrReturn.Length - 1) == "0")
                    { mstrReturn = mstrReturn.Substring(0, mstrReturn.Length - 1) + "M0"; }
                    else
                    { mstrReturn = mstrReturn + "M"; }
                }
                if (j == 0 && mstrChar != "0") { mstrReturn = mstrReturn + mstrChar + "F"; }
                if (j == 1 && mstrChar != "0") { mstrReturn = mstrReturn + mstrChar + "J"; }
                if (mstrChar != "0") { mblnAddzero = false; }
            }
            if (mstrReturn.Substring(0, 1) == "1" && mstrReturn.Substring(1, 1) == mstrFlag[1] && (mstrReturn.StartsWith("1TW0") || mstrReturn.StartsWith("1TY0") || mstrReturn.StartsWith("1TM0"))) { mstrReturn = mstrReturn.Substring(1); }
            if (mstrReturn.Substring(mstrReturn.Length - 1, 1) == "0") { mstrReturn = mstrReturn.Substring(0, mstrReturn.Length - 1); }
            if (mstrReturn.Substring(0, 1) == "0") { mstrReturn = mstrReturn.Substring(1); }
            if (mstrReturn.Substring(mstrReturn.Length - 1, 1) == "M" || mstrReturn.Substring(mstrReturn.Length - 1, 1) == "W" || mstrReturn.Substring(mstrReturn.Length - 1, 1) == "S" || mstrReturn.Substring(mstrReturn.Length - 1, 1) == "H" || mstrReturn.Substring(mstrReturn.Length - 1, 1) == "T") { mstrReturn = mstrReturn + "Y"; }
            return mstrReturn;
        }

    }

    public class StaticMethod
    {
        static IApplicationContext springContext = null;
        static IApplicationContext moduleContext = null;

        /// <summary>
        /// 判断对象是否被持久化
        /// </summary>
        /// <param name="proxy">对象</param>
        /// <returns></returns>
        static public bool IsInitialized(object proxy)
        {
            return NHibernate.NHibernateUtil.IsInitialized(proxy);
        }
        /// <summary>
        /// 根据type获得服务(规则接口去掉I,就是AppContext里面的服务对象名称)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static public object GetService(System.Type type)
        {
            return GetService(type.Name.Substring(1));
        }

        static public object GetRefService(System.Type type)
        {
            return GetService("Ref" + type.Name.Substring(1));
        }
        static public T GetService<T>()
        {
            return (T)GetService(typeof(T).Name.Substring(1));
        }
        /// <summary>
        /// 获得服务根据名称
        /// </summary>
        /// <param name="serviceName">服务对象名称</param>
        /// <returns></returns>
        static public object GetService(string serviceName)
        {
            if (springContext == null)
            {
                springContext = AppDomain.CurrentDomain.GetData("SupplyChain") as IApplicationContext;
            }

            return springContext.GetObject(serviceName);
            //return ServiceFactory.GetServiceByName(serviceName);            
        }


        //打印错误
        static public string ExceptionMessage(Exception e)
        {
            string s = "";
            //s = e.StackTrace.ToString();
            //s = s + "-----------------------------------------------" + "\n\r";

            s = s + e.Message.ToString() + "\n";

            while (e.InnerException != null)
            {
                e = e.InnerException;
                s = s + e.Message.ToString() + "\n";
            }

            //if (e.InnerException != null)
            //    return e.InnerException.Message;
            //else
            //{
            //    if (e is NHibernate.StaleObjectStateException)
            //        return "当前数据可能已被其他人修改，请刷新后再作处理！";

            return s;

        }

        static public object GetModule(string moduleName)
        {
            if (moduleContext == null)
            {

                moduleContext = AppDomain.CurrentDomain.GetData("SupplyChainClient") as IApplicationContext;
            }
            object obj = moduleContext.GetObject(moduleName);
            return obj;
        }
        static public object GetRefModule(Type aModuleType)
        {
            object obj = GetModule("Ref" + aModuleType.Name);
            return obj;
        }
        static public XmlDocument GetXmlEmbedded(Assembly aAssembly, string fileName)
        {
            if (aAssembly == null)
                aAssembly = typeof(StaticMethod).Assembly;
            Stream stream = aAssembly.GetManifestResourceStream(fileName);

            System.Xml.XmlTextReader x = new System.Xml.XmlTextReader(stream);
            string s = "";
            while (x.Read())
            {
                if (x.NodeType == System.Xml.XmlNodeType.Text)
                {
                    s = s + x.Value;
                }
            }
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(s);
            return doc;
        }

        static public void InitClientModuleContext(Assembly aAssembly)
        {
            Stream strm = aAssembly.GetManifestResourceStream("Application.Business.Erp.SupplyChain.Client.ModuleContext.xml");

            XmlDocument xd = new XmlDocument();
            xd.Load(strm);
            xd.Save(AppDomain.CurrentDomain.BaseDirectory + "ModuleContext.xml");


            Spring.Context.Support.XmlApplicationContext ac = new Spring.Context.Support.XmlApplicationContext(AppDomain.CurrentDomain.BaseDirectory + "ModuleContext.xml");
            AppDomain.CurrentDomain.SetData(aAssembly.GetName().Name, ac);
            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "ModuleContext.xml");
        }

        ///<summary>
        /// 文本输入的类型
        /// </summary>
        public enum EnmTextInputKind
        {
            Expinteger = 0,
            ExpDecimal = 1,
            ExpChar = 2,
            ExpPhonecode = 3,
            ExpPostcode = 4,
            ExpDate = 5,
            ExpCustom = 6
        }
        /// <summary>
        /// 判断输入的值是否符合指定的类型
        /// </summary>
        /// <param name="textInputValue">输入值</param>
        /// <param name="textInputKind">类型</param>
        /// <param name="customKindValue">自定义</param>
        /// <returns></returns>
        public static bool CheckInputText(string textInputValue, EnmTextInputKind textInputKind, string customKindValue)
        {
            string ValidateString = "";

            switch (textInputKind)
            {
                case EnmTextInputKind.Expinteger:
                    ValidateString = "-0123456789";
                    break;
                case EnmTextInputKind.ExpDecimal:
                    ValidateString = "0123456789.+-";
                    break;
                case EnmTextInputKind.ExpChar:
                    ValidateString = "abcdefghijklmnopqrstuvwxyz";
                    break;
                case EnmTextInputKind.ExpPhonecode:
                    ValidateString = "0123456789-()";
                    break;
                case EnmTextInputKind.ExpPostcode:
                    ValidateString = "0123456789";
                    break;
                case EnmTextInputKind.ExpDate:
                    ValidateString = "0123456789-./";
                    break;
                case EnmTextInputKind.ExpCustom:
                    ValidateString = customKindValue;
                    break;
                default:
                    break;
            }
            if (textInputValue == "\b") return true;
            return (ValidateString.IndexOf(textInputValue) == -1 ? false : true);
        }

        /// <summary>
        /// 判断单元格内容是否大于零，是否数字
        /// </summary>
        /// <param name="cellValue"></param>
        /// <returns></returns>
        public static bool ValidDecimalCell(object cellValue)
        {
            if (cellValue == null || cellValue.ToString().Equals("")) return false;
            decimal d = 0M;
            if (!decimal.TryParse(cellValue.ToString(), out d))
            {
                return false;
            }
            else
            {
                if (d <= 0)
                {
                    return false;
                }
            }

            return true;
        }

        #region  读取Excel文件
        public class ExcelClass
        {


            /// <summary>
            /// 读取指定Excel文件的Sheet
            /// </summary>
            /// <param name="fileName">Excel文件名</param>
            /// <returns>IList</returns>
            public static IList ReadExcelSheet(string fileName)
            {
                try
                {
                    IList list = new ArrayList();
                    object objMissing = System.Reflection.Missing.Value;
                    //打开excel文件   
                    Excel.ApplicationClass my = new Excel.ApplicationClass();
                    //打开工作簿   
                    Excel.Workbook mybook = my.Workbooks.Open(fileName, objMissing, objMissing, objMissing,
                                                            objMissing, objMissing, objMissing,
                                                            objMissing, objMissing, objMissing,
                                                            objMissing, objMissing, objMissing, objMissing, objMissing);
                    foreach (Excel.Worksheet mysheet in mybook.Worksheets)
                    {
                        list.Add(mysheet.Name.ToString());
                    }
                    mybook.Close(null, fileName, null);
                    my.Quit();
                    return list;
                }
                catch (Exception ee)
                {
                    throw ee;
                }
            }
            /// <summary>
            /// 读取指定的Excel文件
            /// </summary>
            /// <param name="fileName">Excel文件名称</param>
            /// <returns>返回第一个Sheet的内容DataSet.Talbes[0]</returns>
            public static DataSet ReadDataFromExcelOlE(string fileName)
            {
                try
                {
                    IList list = ReadExcelSheet(fileName);
                    return ReadDataFromExcelOlE(fileName, list[0].ToString());
                }
                catch (Exception ee)
                {
                    throw ee;
                }
            }
            /// <summary>
            /// 读取指定的Excel文件
            /// </summary>
            /// <param name="fileName">Excel文件名称</param>
            /// <returns>返回Sheet的内容DataSet.Talbes[sheetName]</returns>
            public static DataSet ReadDataFromExcelOlE(string fileName, string sheetName)
            {
                try
                {
                    string strCon = " Provider = Microsoft.Jet.OLEDB.4.0 ; Data Source = " + fileName + ";Extended Properties=Excel 8.0";
                    OleDbConnection myConn = new OleDbConnection(strCon);

                    string strCom = " Select * FROM [" + sheetName + "$] ";
                    myConn.Open();

                    OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
                    DataSet myDataSet = new DataSet();
                    myCommand.Fill(myDataSet, "[" + sheetName + "$]");
                    myConn.Close();
                    return myDataSet;
                }
                catch (Exception ee)
                {
                    throw ee;
                }
            }
            public static DataSet ReadDataFromExcel(string fileName)
            {
                Excel.ApplicationClass my = new Excel.ApplicationClass();

                try
                {
                    DataSet dataSet = new DataSet();

                    object objMissing = System.Reflection.Missing.Value;

                    //打开工作簿   
                    Excel.Workbook mybook = my.Workbooks.Open(fileName, objMissing, objMissing, objMissing,
                                                            objMissing, objMissing, objMissing,
                                                            objMissing, objMissing, objMissing,
                                                            objMissing, objMissing, objMissing, objMissing, objMissing);

                    Excel.Worksheet mySheet = mybook.Sheets[1] as Excel.Worksheet;
                    mySheet.Activate();
                    dataSet = ReadExcelSheet(mySheet);
                    mybook.Close(null, fileName, null);
                    my.Quit();
                    return dataSet;
                }
                catch (Exception ee)
                {
                    throw ee;
                }
                finally
                {
                    my.Quit();
                }
            }
            /// <summary>
            /// 读取指定的Excel文件
            /// </summary>
            /// <param name="fileName">Excel文件名称</param>
            /// <returns>返回Sheet的内容DataSet.Talbes[sheetName]</returns>
            public static DataSet ReadDataFromExcel(string fileName, string sheetName)
            {
                Excel.ApplicationClass my = new Excel.ApplicationClass();

                try
                {
                    DataSet dataSet = new DataSet();

                    object objMissing = System.Reflection.Missing.Value;

                    //打开工作簿   
                    Excel.Workbook mybook = my.Workbooks.Open(fileName, objMissing, objMissing, objMissing,
                                                            objMissing, objMissing, objMissing,
                                                            objMissing, objMissing, objMissing,
                                                            objMissing, objMissing, objMissing, objMissing, objMissing);

                    Excel.Worksheet mySheet = mybook.Sheets[sheetName] as Excel.Worksheet;
                    mySheet.Activate();
                    dataSet = ReadExcelSheet(mySheet);
                    mybook.Close(null, fileName, null);
                    my.Quit();
                    return dataSet;
                }
                catch (Exception ee)
                {
                    throw ee;
                }
                finally
                {
                    my.Quit();
                }
            }
            private static DataSet ReadExcelSheet(Excel.Worksheet mySheet)
            {
                try
                {
                    long lngMaxCol = 0;
                    long lngMaxRow = 0;
                    DataSet dataSet = new DataSet();
                    dataSet.Tables.Add(mySheet.Name.ToString());
                    for (int j = 1; j < mySheet.Columns.Count; j++)
                    {
                        if ((mySheet.Cells[1, j] as Excel.Range).Text == "") break;
                        lngMaxCol++;
                    }

                    for (int i = 1; i < mySheet.Rows.Count; i++)
                    {
                        if ((mySheet.Cells[i, 1] as Excel.Range).Text == "") break;
                        lngMaxRow++;
                    }
                    //获得表头
                    for (int j = 1; j < lngMaxCol + 1; j++)
                    {
                        dataSet.Tables[0].Columns.Add((mySheet.Cells[1, j] as Excel.Range).Text.ToString());
                    }
                    //添加行
                    for (int i = 2; i < lngMaxRow + 1; i++)
                    {
                        DataRow dataRow = dataSet.Tables[0].NewRow();
                        for (int j = 1; j < lngMaxCol + 1; j++)
                        {
                            dataRow[j - 1] = (mySheet.Cells[i, j] as Excel.Range).Text;
                        }
                        dataSet.Tables[0].Rows.Add(dataRow);
                    }
                    return dataSet;
                }
                catch (Exception ee)
                {
                    throw ee;
                }
            }

            #region DataGridView保存Excel
            public static string SaveDataGridViewToExcel(DataGridView theDataGridView, bool isOpen)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Excel|*.xlsx";
                DialogResult result = saveFileDialog1.ShowDialog(theDataGridView.FindForm());
                if (result != DialogResult.OK) return "";

                //theDataGridView.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
                theDataGridView.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
                //this.DataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
                Clipboard.Clear();
                theDataGridView.SelectAll();
                DataGridViewToExcel(theDataGridView.GetClipboardContent(), saveFileDialog1.FileName, isOpen);
                Clipboard.Clear();
                if (theDataGridView.Rows.Count > 0)
                {
                    theDataGridView.CurrentCell = theDataGridView[0, 0];
                    theDataGridView.Rows[0].Selected = true;
                }
                return saveFileDialog1.FileName;
            }

            public static string GetDataGridViewToExcelName(DataGridView theDataGridView)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Excel|*.xlsx";
                DialogResult result = saveFileDialog1.ShowDialog(theDataGridView.FindForm());
                if (result != DialogResult.OK) return "";

                return saveFileDialog1.FileName;
            }

            public static void SaveDataGridViewToExcel(DataGridView theDataGridView, string fileName)
            {
                theDataGridView.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
                Clipboard.Clear();
                theDataGridView.SelectAll();
                DataGridViewToExcel(theDataGridView.GetClipboardContent(), fileName, false);
                Clipboard.Clear();
                if (theDataGridView.Rows.Count > 0)
                {
                    theDataGridView.CurrentCell = theDataGridView[0, 0];
                    theDataGridView.Rows[0].Selected = true;
                }
            }

            private static bool DataGridViewToExcel(DataObject dataObject, string filePath)
            {
                //剪切板实现
                try
                {
                    Clipboard.Clear();
                    Clipboard.SetText(dataObject.GetText());
                    //Clipboard.SetDataObject(dataObject);                    
                    // 开始新建Excel
                    object m_objOpt = System.Reflection.Missing.Value;
                    Excel.Application m_objExcel = new Excel.Application();
                    Excel.Workbooks m_objBooks = (Excel.Workbooks)m_objExcel.Workbooks;
                    Excel._Workbook m_objBook = (Excel._Workbook)(m_objBooks.Add(m_objOpt));

                    // 粘贴数据
                    Excel.Sheets m_objSheets = (Excel.Sheets)m_objBook.Worksheets;
                    Excel._Worksheet m_objSheet = (Excel._Worksheet)(m_objSheets.get_Item(1));
                    Excel.Range m_objRange = m_objSheet.get_Range("A1", m_objOpt);
                    m_objSheet.Paste(m_objRange, false);
                    //根据列的内容自动调整列的宽度
                    m_objSheet.Columns.AutoFit();

                    // 保存退出
                    m_objBook.SaveAs(filePath, m_objOpt, m_objOpt,
                        m_objOpt, m_objOpt, m_objOpt, Excel.XlSaveAsAccessMode.xlNoChange, m_objOpt, m_objOpt,
                        m_objOpt, m_objOpt, m_objOpt);
                    m_objBook.Close(false, m_objOpt, m_objOpt);
                    m_objExcel.Quit();
                }
                catch (Exception ee)
                {
                    throw ee;
                }
                return true;
            }
            /// <summary>
            /// 拷贝DataGridView选择的内容到Excel
            /// </summary>
            /// <param name="dataObject">选择的单元格</param>
            /// <param name="filePath">Excel文件路径</param>
            /// <returns>True 成功,False 失败</returns>
            public static bool DataGridViewToExcel(DataObject dataObject, string filePath, bool isOpen)
            {
                Exception exc;
                try
                {
                    //判断文件是否存在
                    if (System.IO.File.Exists(filePath))
                    {
                        //exc = new Exception(filePath + "\n" + "文件已经存在!");
                        //throw exc;
                        File.Delete(filePath);
                    }
                    if (!System.IO.Directory.GetParent(filePath).Exists)
                    {
                        exc = new Exception(System.IO.Directory.GetParent(filePath).FullName.ToString() + "\n" + "目录文件夹不存在!");
                        throw exc;
                    }
                    //保存
                    if (!DataGridViewToExcel(dataObject, filePath))
                        return false;
                    if (isOpen)
                    {
                        Process process = new Process();
                        process.StartInfo.FileName = filePath;
                        process.Start();
                        process.Close();
                        //Process.Start(filePath);
                    }
                }
                catch (Exception ee)
                {
                    throw ee;
                }
                return true;
            }
            #endregion
        }
        #endregion
        #region 读取类的属性
        /// <summary>
        /// 获得指定类的所有属性
        /// </summary>
        /// <param name="className">类名称</param>
        /// <returns>PropertyInfo属性数组</returns>
        public static IList ClassGetProperty(string className)
        {
            IList list = new ArrayList();
            try
            {
                Type typeObj = Type.GetType(className);
                if (typeObj == null)
                    throw new Exception("类名[" + className + "]不正确");
                PropertyInfo[] propInfo = typeObj.GetProperties();
                foreach (PropertyInfo var in propInfo)
                {
                    list.Add(var.Name.ToString());
                }
                return list;
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        public static PropertyInfo[] ClassGetProperty(Type type)
        {
            IList list = new ArrayList();
            try
            {
                if (type == null)
                    throw new Exception("类型不能为空！");
                PropertyInfo[] propInfo = type.GetProperties();
                return propInfo;
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        /// <summary>
        /// 获得指定类型的是否是某类型的子类
        /// </summary>
        /// <param name="type">类别</param>
        /// <param name="typeInclude">包含类别</param>
        /// <returns></returns>
        public static bool GetTypeInclude(Type type, Type typeInclude)
        {
            if (type == typeInclude)
                return true;
            else
                if (type.BaseType == null)
                    return false;
                else
                    return GetTypeInclude(type.BaseType, typeInclude);
        }
        #endregion
        #region 按指定类型从DataSet中生成实例
        /// <summary>
        /// 把dataSet里面的数据按propToExcelHeaderText的对应关系,生成type的实例，
        /// </summary>
        /// <param name="type">待生成实例的类型</param>
        /// <param name="propToExcelHeaderText">属性Excel表头对应:"propName|ExcelHeaderText"</param>
        /// <param name="dataSet">从Excel表读取的数据DataSet</param>
        /// <returns>生成的实例Ilist</returns>
        public static IList ClassCreateInstance(Type type, NameValueCollection propToExcelHeaderText, DataSet dataSet)
        {
            IList list = new ArrayList();
            try
            {
                list = ClassCreateInstanceByTable(type, propToExcelHeaderText, dataSet.Tables[0]);
                return list;
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        public static IList PersonCreateInstance(NameValueCollection propToExcelHeaderText, DataSet dataSet, string dataSetTableName)
        {
            DataTable dt = dataSet.Tables[dataSetTableName];
            if (dt == null)
                throw new Exception("表名称不存在！");
            IList list = new ArrayList();
            string errMsg = "";
            long intRow = 0L;

            foreach (DataRow varRow in dt.Rows)
            {
                intRow++;
                Type type = null;
                if (varRow[0].ToString() == "外部员工")
                    type = typeof(ExteriorPerson);
                else
                    type = typeof(Employee);
                Object obj = Activator.CreateInstance(type);
                PropertyInfo[] pis = obj.GetType().GetProperties();
                foreach (DataColumn varCol in dt.Columns)
                {
                    errMsg = "第[" + intRow.ToString() + "]行的[" + varCol.Caption + "]数据错误！";
                    foreach (PropertyInfo pi in pis)
                    {

                        if (ClientUtil.ToString(propToExcelHeaderText.Get(pi.Name)) == varCol.ColumnName)
                        {
                            if (pi.PropertyType.FullName == "System.Int64")
                                pi.SetValue(obj, Convert.ToInt64(varRow[varCol]), null);
                            else if (pi.PropertyType.FullName == "System.Boolean")
                                pi.SetValue(obj, Convert.ToBoolean(varRow[varCol]), null);
                            else if (pi.PropertyType.FullName == "System.DateTime")
                                pi.SetValue(obj, Convert.ToDateTime(varRow[varCol]), null);
                            else if (pi.PropertyType.FullName == "System.Decimal")
                                pi.SetValue(obj, ClientUtil.ToDecimal(varRow[varCol]), null);
                            else if (pi.PropertyType.FullName == "System.Double")
                                pi.SetValue(obj, Convert.ToDouble(varRow[varCol]), null);
                            else if (pi.PropertyType.FullName == "System.Int16" || pi.PropertyType.FullName == "System.Int32")
                                pi.SetValue(obj, Convert.ToInt32(varRow[varCol]), null);
                            else if (pi.PropertyType.FullName == "System.Single")
                                pi.SetValue(obj, Convert.ToSingle(varRow[varCol]), null);
                            else if (pi.PropertyType.FullName == "System.String")
                                pi.SetValue(obj, ClientUtil.ToString(varRow[varCol]), null);
                            else
                                pi.SetValue(obj, null, null);
                            break;
                        }
                        else
                        {
                            if (pi.PropertyType == typeof(IList))
                            {
                                pi.SetValue(obj, new ArrayList(), null);
                            }
                        }
                    }
                }
                list.Add(obj);
            }
            return list;
        }
        public static IList ClassCreateInstance(Type type, NameValueCollection propToExcelHeaderText, DataSet dataSet, string dataSetTableName)
        {
            IList list = new ArrayList();
            try
            {
                DataTable dt = dataSet.Tables[dataSetTableName];
                if (dt == null)
                    throw new Exception("表名称不存在！");
                list = ClassCreateInstanceByTable(type, propToExcelHeaderText, dt);
                return list;
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        private static IList ClassCreateInstanceByTable(Type type, NameValueCollection propToExcelHeaderText, DataTable dataTable)
        {
            IList list = new ArrayList();
            string errMsg = "";
            long intRow = 0L;

            foreach (DataRow varRow in dataTable.Rows)
            {
                intRow++;
                Object obj = Activator.CreateInstance(type);
                PropertyInfo[] pis = obj.GetType().GetProperties();
                foreach (DataColumn varCol in dataTable.Columns)
                {
                    errMsg = "第[" + intRow.ToString() + "]行的[" + varCol.Caption + "]数据错误！";
                    foreach (PropertyInfo pi in pis)
                    {

                        if (ClientUtil.ToString(propToExcelHeaderText.Get(pi.Name)) == varCol.ColumnName)
                        {
                            if (pi.PropertyType.FullName == "System.Int64")
                                pi.SetValue(obj, Convert.ToInt64(varRow[varCol]), null);
                            else if (pi.PropertyType.FullName == "System.Boolean")
                                pi.SetValue(obj, Convert.ToBoolean(varRow[varCol]), null);
                            else if (pi.PropertyType.FullName == "System.DateTime")
                                pi.SetValue(obj, Convert.ToDateTime(varRow[varCol]), null);
                            else if (pi.PropertyType.FullName == "System.Decimal")
                                pi.SetValue(obj, ClientUtil.ToDecimal(varRow[varCol]), null);
                            else if (pi.PropertyType.FullName == "System.Double")
                                pi.SetValue(obj, Convert.ToDouble(varRow[varCol]), null);
                            else if (pi.PropertyType.FullName == "System.Int16" || pi.PropertyType.FullName == "System.Int32")
                                pi.SetValue(obj, Convert.ToInt32(varRow[varCol]), null);
                            else if (pi.PropertyType.FullName == "System.Single")
                                pi.SetValue(obj, Convert.ToSingle(varRow[varCol]), null);
                            else if (pi.PropertyType.FullName == "System.String")
                                pi.SetValue(obj, ClientUtil.ToString(varRow[varCol]), null);
                            else
                                pi.SetValue(obj, null, null);
                            break;
                        }
                        else
                        {
                            if (pi.PropertyType == typeof(IList))
                            {
                                pi.SetValue(obj, new ArrayList(), null);
                            }
                        }
                    }
                }
                list.Add(obj);
            }
            return list;
        }
        #endregion

        public static Object GetImageResource(string name)
        {
            object obj = Resources.ResourceManager.GetObject(name);
            return obj;
        }

        #region 日志操作
        private static IStockInSrv theStockInSrv;
        /// <summary>
        /// 插入日志
        /// </summary>
        /// <param name="logData"></param>
        /// <returns></returns>
        public static bool InsertLogData(LogData logData)
        {
            if (theStockInSrv == null)
            {
                theStockInSrv = StaticMethod.GetService("StockInSrv") as IStockInSrv;
            }
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            logData.ProjectID = projectInfo.Id;
            return theStockInSrv.SaveLogData(logData);
        }

        /// <summary>
        /// 插入日志 参数传入顺序为 BillId;OperType;Code;OperPerson;BillType;Descript;ProjectName
        /// </summary>
        /// <param name="args">参数传入顺序为 BillId;OperType;Code;OperPerson;BillType;Descript;ProjectName</param>
        /// <returns></returns>
        public static bool InsertLogData(params object[] args)
        {
            if (theStockInSrv == null)
            {
                theStockInSrv = StaticMethod.GetService("StockInSrv") as IStockInSrv;
            }
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            LogData logData = new LogData();
            if (args != null && args.Length > 0)
            {
                logData.OperDate = DateTime.Now;
                for (int i = 0; i < args.Length; i++)
                {
                    object obj = args[i];
                    if (i == 0)
                    {
                        logData.BillId = ClientUtil.ToString(obj);
                    }
                    else if (i == 1)
                    {
                        logData.OperType = ClientUtil.ToString(obj);
                    }
                    else if (i == 2)
                    {
                        logData.Code = ClientUtil.ToString(obj);
                    }
                    else if (i == 3)
                    {
                        logData.OperPerson = ClientUtil.ToString(obj);
                    }
                    else if (i == 4)
                    {
                        logData.BillType = ClientUtil.ToString(obj);

                    }
                    else if (i == 5)
                    {
                        logData.Descript = ClientUtil.ToString(obj);
                    }
                    else if (i == 6)
                    {
                        logData.ProjectName = ClientUtil.ToString(obj);
                    }
                }
                logData.ProjectID = projectInfo.Id;
            }

            return theStockInSrv.SaveLogData(logData);
        }

        /// <summary>
        /// 查询日志
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public static IList GetLogData(ObjectQuery oq)
        {
            if (theStockInSrv == null)
            {
                theStockInSrv = StaticMethod.GetService("StockInSrv") as IStockInSrv;
            }
            return theStockInSrv.GetObjects(typeof(LogData), oq);
        }
        #endregion

        public static bool CheckServerDateTime()
        {
            if (theStockInSrv == null)
            {
                theStockInSrv = StaticMethod.GetService("StockInSrv") as IStockInSrv;
            }
            return theStockInSrv.CheckServerDateTime();
        }

        #region 业务单据跟帖操作

        private static IGWBSTreeSrv theBaseSrv;

        /// <summary>
        /// 插入单据评论
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public static bool SaveBillComment(BillComments comment)
        {
            if (theStockInSrv == null)
            {
                theStockInSrv = StaticMethod.GetService("StockInSrv") as IStockInSrv;
            }

            comment.CommentCommitTime = theStockInSrv.GetServerDateTime();

            theStockInSrv.SaveByDao(comment);

            return true;
        }
        /// <summary>
        /// 获取单据评论信息
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public static List<BillComments> GetBillComments(ObjectQuery oq)
        {
            if (theStockInSrv == null)
            {
                theStockInSrv = StaticMethod.GetService("StockInSrv") as IStockInSrv;
            }

            List<BillComments> list = theStockInSrv.GetObjects(typeof(BillComments), oq).OfType<BillComments>().ToList();

            return list;
        }
        /// <summary>
        /// 获取单据评论信息
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        public static List<BillComments> GetCommentsByBillId(string billId)
        {
            if (theStockInSrv == null)
            {
                theStockInSrv = StaticMethod.GetService("StockInSrv") as IStockInSrv;
            }
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BillID", billId));
            oq.AddOrder(NHibernate.Criterion.Order.Asc("CommentCommitTime"));

            List<BillComments> list = theStockInSrv.GetObjects(typeof(BillComments), oq).OfType<BillComments>().ToList();

            return list;
        }
        /// <summary>
        /// 获取单据评论信息总数
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        public static int GetCommentsCountByBillId(string billId)
        {
            if (theBaseSrv == null)
            {
                theBaseSrv = StaticMethod.GetService("GWBSTreeSrv") as IGWBSTreeSrv;
            }

            string sql = "select count(0) from THD_BillComments where BillID='" + billId + "'";

            DataSet ds = theBaseSrv.SearchSQL(sql);

            int count = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

            return count;
        }

        #endregion

        #region 查找项目
        private static Hashtable projectInfoHash = new Hashtable();
        private static Hashtable subCompanyInfoHash = new Hashtable();
        /// <summary>
        /// 查找项目信息
        /// </summary>
        /// <returns></returns>
        public static CurrentProjectInfo GetProjectInfo()
        {
            string key = ConstObject.LoginPersonInfo.Id + "-" + ConstObject.TheOperationOrg.Id;
            CurrentProjectInfo projectInfo = null;
            if (projectInfoHash != null && projectInfoHash.Count > 0)
            {
                projectInfo = projectInfoHash[key] as CurrentProjectInfo;
                if (projectInfo != null) return projectInfo;
            }
            if (theStockInSrv == null)
            {
                theStockInSrv = StaticMethod.GetService("StockInSrv") as IStockInSrv;
            }

            try
            {
                if (!string.IsNullOrEmpty(ConstObject.AutoSwitchProjectId))
                {
                    projectInfo = theStockInSrv.GetProjectInfoById(ConstObject.AutoSwitchProjectId);
                }
                else
                {
                    projectInfo = theStockInSrv.GetProjectInfo(ConstObject.TheOperationOrg.SysCode);
                }
                //获取当前登陆人对应的核算组织

            }
            catch
            {

            }
            if (projectInfo == null)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Code", CommonUtil.CompanyProjectCode));
                projectInfo = theStockInSrv.GetProjectInfo(oq);
            }

            if (projectInfo == null)
            {
                System.Windows.MessageBox.Show("未获取到归属项目信息。");
                return null;
            }
            projectInfoHash.Add(key, projectInfo);
            return projectInfo;
        }
        public static OperationOrgInfo GetSubCompanyOrgInfo()
        {
            OperationOrgInfo subOrgInfo = new OperationOrgInfo();
            string key = ConstObject.LoginPersonInfo.Id + "-" + ConstObject.TheOperationOrg.Id;
            if (subCompanyInfoHash != null && subCompanyInfoHash.Count > 0)
            {
                subOrgInfo = subCompanyInfoHash[key] as OperationOrgInfo;
                if (subOrgInfo != null) return subOrgInfo;
            }
            if (theStockInSrv == null)
            {
                theStockInSrv = StaticMethod.GetService("StockInSrv") as IStockInSrv;
            }
            subOrgInfo = theStockInSrv.GetSubCompanyOrgInfo(ConstObject.TheOperationOrg.SysCode);
            if (subOrgInfo != null && ClientUtil.ToString(subOrgInfo.Id) != "")
            {
                subCompanyInfoHash.Add(key, subOrgInfo);
            }
            return subOrgInfo;
        }
        #endregion

        #region 判断系统使用的是否是SQL Server数据库
        /// <summary>
        /// 判断系统使用的是否是SQL Server数据库 用于处理sql查询语句
        /// </summary>
        /// <returns></returns>
        public static bool IsUseSQLServer()
        {
            if (theStockInSrv == null)
            {
                theStockInSrv = StaticMethod.GetService("StockInSrv") as IStockInSrv;
            }
            return theStockInSrv.IsUseSQLServer();
        }
        #endregion

        #region 读取flx模板方法
        /// <summary>
        /// 判断模板文件是否存在
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IfExistFileInServer(string fileName)
        {
            if (theStockInSrv == null)
            {
                theStockInSrv = StaticMethod.GetService("StockInSrv") as IStockInSrv;
            }
            return theStockInSrv.IfExistFileInServer(fileName);
        }

        /// <summary>
        /// 从服务器读取模板文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static byte[] ReadTempletByServer(string fileName)
        {
            if (theStockInSrv == null)
            {
                theStockInSrv = StaticMethod.GetService("StockInSrv") as IStockInSrv;
            }
            return theStockInSrv.ReadTempletByServer(fileName);
        }
        #endregion

        #region 查询仓库
        private static List<StationCategory> stationCategoryList;
        /// <summary>
        /// 查询仓库
        /// </summary>
        /// <returns></returns>
        public static StationCategory GetStationCategory()
        {
            if (stationCategoryList != null && stationCategoryList.Count > 0)
            {
                return stationCategoryList[0];
            }

            if (theStockInSrv == null)
            {
                theStockInSrv = StaticMethod.GetService("StockInSrv") as IStockInSrv;
            }
            CurrentProjectInfo cpi = GetProjectInfo();
            StationCategory stationCategory = theStockInSrv.GetStationCategory(cpi.Id);
            if (stationCategory == null)
            {
                //System.Windows.MessageBox.Show("未获取到仓库信息。");
            }
            else
            {
                stationCategoryList = new List<StationCategory>();
                stationCategoryList.Add(stationCategory);
                return stationCategory;
            }
            return null;
        }
        #endregion

        #region 基础数据操作

        private static Hashtable htBasicData;

        /// <summary>
        /// 通过基础数据表名称查询基础数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IList GetBasicDataByName(string name)
        {
            IList list = null;
            if (htBasicData != null && htBasicData.Count > 0)
            {
                list = (IList)htBasicData[name];
            }
            else
            {
                htBasicData = new Hashtable();
            }
            if (list != null && list.Count > 0) return list;

            if (theStockInSrv == null)
            {
                theStockInSrv = StaticMethod.GetService("StockInSrv") as IStockInSrv;
            }

            list = theStockInSrv.GetBasicDataByParentName(name);
            if (list != null && list.Count > 0)
            {
                htBasicData.Add(name, list);
                return list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 通过基础数据表名称和项目名称查询基础数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IList GetBasicDataByNameAndProjectName(string name, string projectName)
        {
            if (theStockInSrv == null)
            {
                theStockInSrv = StaticMethod.GetService("StockInSrv") as IStockInSrv;
            }

            return theStockInSrv.GetBasicDataByParentNameAndProjectName(name, projectName);

        }
        #endregion

        /// <summary>
        /// 根据日期得到星期几
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GetDayOfWeek(DateTime dt)
        {
            DayOfWeek dow = dt.DayOfWeek;
            string week = "";
            switch (dow)
            {
                case DayOfWeek.Monday:
                    week = "星期一";
                    break;
                case DayOfWeek.Tuesday:
                    week = "星期二";
                    break;
                case DayOfWeek.Wednesday:
                    week = "星期三";
                    break;
                case DayOfWeek.Thursday:
                    week = "星期四";
                    break;
                case DayOfWeek.Friday:
                    week = "星期五";
                    break;
                case DayOfWeek.Saturday:
                    week = "星期六";
                    break;
                case DayOfWeek.Sunday:
                    week = "星期日";
                    break;
            }
            return week;
        }

        #region 文档/分类操作

        private const double KBCount = 1024;
        private const double MBCount = KBCount * 1024;
        private const double GBCount = MBCount * 1024;
        private const double TBCount = GBCount * 1024;

        /// <summary>
        /// 得到适应文件的大小
        /// </summary>
        /// <param name="size">文件大小（字节）</param>
        /// <param name="roundCount">小数位数</param>
        /// <returns></returns>
        public static string GetFileAutoSizeString(double size, int roundCount)
        {
            if (KBCount > size)
            {
                return Math.Round(size, roundCount) + "B";
            }
            else if (MBCount > size)
            {
                return Math.Round(size / KBCount, roundCount) + "KB";
            }
            else if (GBCount > size)
            {
                return Math.Round(size / MBCount, roundCount) + "MB";
            }
            else if (TBCount > size)
            {
                return Math.Round(size / GBCount, roundCount) + "GB";
            }
            else
            {
                return Math.Round(size / TBCount, roundCount) + "TB";
            }
        }

        /// <summary>
        /// 获取MBP上传文件到IRP使用的文档参数(1.文档对象类型，2.文档结构类型，3.文档和文档分类关系对象类型)
        /// </summary>
        /// <returns></returns>
        public static List<string> GetUploadFileParamsByMBP_IRP()
        {
            List<string> listParams = new List<string>();
            string FileObjectTypeName = "DOCUMENT";
            string FileStructureType = "FILESTRUCTURE";
            string DocumentCateLinkTypeName = "CLASSIRPDOCUMENT";

            string configFloderPath = AppDomain.CurrentDomain.BaseDirectory + @"FileUploadConfig";
            string confirFilePath = configFloderPath + @"\\UploadDocumentConfig.xml";
            if (Directory.Exists(configFloderPath) && File.Exists(confirFilePath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(confirFilePath);

                XmlElement root = xmlDoc.DocumentElement;
                if (root != null)
                {
                    foreach (XmlNode node in root.ChildNodes)
                    {
                        if (node.Name.ToUpper() == "ParamConfig".ToUpper())
                        {
                            if (node.Attributes["name"].Value.ToUpper() == "FileObjectType".ToUpper())
                            {
                                FileObjectTypeName = node.Attributes["value"].Value.ToUpper();
                            }
                            else if (node.Attributes["name"].Value.ToUpper() == "FileStructureType".ToUpper())
                            {
                                FileStructureType = node.Attributes["value"].Value.ToUpper();
                            }
                            else if (node.Attributes["name"].Value.ToUpper() == "DocumentCateLinkTypeName".ToUpper())
                            {
                                DocumentCateLinkTypeName = node.Attributes["value"].Value.ToUpper();
                            }
                        }
                    }
                }
            }

            listParams.Add(FileObjectTypeName);
            listParams.Add(FileStructureType);
            listParams.Add(DocumentCateLinkTypeName);
            return listParams;
        }


        /// <summary>
        /// 获取MBP上传文件到知识库使用的文档参数(1.文档对象类型，2.文档结构类型，3.文档和文档分类关系对象类型)
        /// </summary>
        /// <returns></returns>
        public static List<string> GetUploadFileParamsByKB()
        {
            List<string> listParams = new List<string>();
            string FileObjectTypeName = "DOCUMENT";
            string FileStructureType = "FILESTRUCTURE";
            string DocumentCateLinkTypeName = "CLASSDOCUMENT";

            string configFloderPath = AppDomain.CurrentDomain.BaseDirectory + @"FileUploadConfig";
            string confirFilePath = configFloderPath + @"\\UploadDocumentConfig.xml";
            if (Directory.Exists(configFloderPath) && File.Exists(confirFilePath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(confirFilePath);

                XmlElement root = xmlDoc.DocumentElement;
                if (root != null)
                {
                    foreach (XmlNode node in root.ChildNodes)
                    {
                        if (node.Name.ToUpper() == "ParamConfig".ToUpper())
                        {
                            if (node.Attributes["name"].Value.ToUpper() == "FileObjectTypeByKB".ToUpper())
                            {
                                FileObjectTypeName = node.Attributes["value"].Value.ToUpper();
                            }
                            else if (node.Attributes["name"].Value.ToUpper() == "FileStructureTypeByKB".ToUpper())
                            {
                                FileStructureType = node.Attributes["value"].Value.ToUpper();
                            }
                            else if (node.Attributes["name"].Value.ToUpper() == "DocumentCateLinkTypeNameByKB".ToUpper())
                            {
                                DocumentCateLinkTypeName = node.Attributes["value"].Value.ToUpper();
                            }
                        }
                    }
                }
            }

            listParams.Add(FileObjectTypeName);
            listParams.Add(FileStructureType);
            listParams.Add(DocumentCateLinkTypeName);
            return listParams;
        }

        private static PLMWebServices.PLMWebServicesSoapClient _PLMWS = null;
        /// <summary>
        /// 项目管理Web服务
        /// </summary>
        public static PLMWebServices.PLMWebServicesSoapClient PLMWS
        {
            get
            {

                if (_PLMWS == null)
                {
                    _PLMWS = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.PLMWebServicesSoapClient();
                }
                return StaticMethod._PLMWS;
            }
            set
            {
                StaticMethod._PLMWS = value;
            }
        }

        private static PLMWebServicesByKB.PLMWebServicesSoapClient _PLMWSByKB = null;
        /// <summary>
        /// 知识库Web服务
        /// </summary>
        public static PLMWebServicesByKB.PLMWebServicesSoapClient PLMWSByKB
        {
            get
            {

                if (_PLMWSByKB == null)
                {
                    _PLMWSByKB = new Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.PLMWebServicesSoapClient();
                }
                return StaticMethod._PLMWSByKB;
            }
            set
            {
                StaticMethod._PLMWSByKB = value;
            }
        }

        public static string KB_System_UserName = "system";
        public static string KB_System_JobId = "07qqRojVT3s89VGoHaKsVE";


        /// <summary>
        /// 添加文档对象
        /// </summary>
        /// <param name="listFileIds">保存后的文档对象Id集（输出参数）</param>
        /// <param name="listFileBytes">二进制文件或本地文件路径数组（根据fileType标记区分，如果是二进制文件：listFileBytes数组中可添加一个或多个Byte[]，一个Byte[]对象表示一个二进制文件，如果是本地文件：listFileBytes数组中可添加一个或多个本地文件的路径）</param>
        /// <param name="fileNames">文件名称集，当文件为二进制文件时使用，用来判断文件格式是否为允许上传格式</param>
        /// <param name="documentObjectType">文档的对象类型</param>
        /// <param name="saveMode">保存方式（1.一个文件生成一个文档对象，2.所有文件生成一个文档对象）</param>
        /// <param name="dicKeyValue">文档对象缺省要设置的属性值对数组（dicKeyValue中可添加一个或多个DictionaryObjectInfo[]，一个DictionaryObjectInfo[]对象存储一个文件的属性值对集）</param>
        /// <param name="sessionid">会话Id（可选参数，如果没有sessionid就需要userName、jobId参数）</param>
        /// <param name="userName">用户名（可选参数）</param>
        /// <param name="jobId">所属岗位（可选参数）</param>
        /// <param name="localVault">文件柜名称（可选参数）</param>
        /// <returns>异常对象</returns>
        public static ErrorStack AddDocumentByCustom(out string[] listFileIds, object[] listFileBytes, string[] fileNames, string documentObjectType, string saveMode,
        DictionaryObjectInfo[] dicKeyValue, string sessionid, string userName, string jobId, string localVault)
        {
            return PLMWS.AddDocumentByCustom(out listFileIds, listFileBytes, fileNames, documentObjectType, saveMode, dicKeyValue, sessionid, userName, jobId, localVault);
        }

        /// <summary>
        /// 修改文档对象
        /// </summary>
        /// <param name="listFileIds">文档对象Id集</param>
        /// <param name="documentObjectType">文档的对象类型</param>
        /// <param name="listFileBytes">二进制文件或本地文件路径数组（根据fileType标记区分，如果是二进制文件：listFileBytes数组中可添加一个或多个Byte[]，一个Byte[]对象表示一个二进制文件，如果是本地文件：listFileBytes数组中可添加一个或多个本地文件的路径）</param>
        /// <param name="fileNames">文件名称集，当文件为二进制文件时使用，用来判断文件格式是否为允许上传格式</param>
        /// <param name="updateMode">更新方式（1.添加新文件，2.覆盖原有文件）</param>
        /// <param name="dicKeyValue">文档对象缺省要设置的属性值对数组（dicKeyValue中可添加一个或多个DictionaryObjectInfo[]，一个DictionaryObjectInfo[]对象存储一个文件的属性值对集）</param>
        /// <param name="sessionid">会话Id（可选参数，如果没有sessionid就需要userName、jobId参数）</param>
        /// <param name="userName">用户名（可选参数）</param>
        /// <param name="jobId">所属岗位（可选参数）</param>
        /// <param name="localVault">文件柜名称（可选参数）</param>
        /// <returns>异常对象</returns>
        public static ErrorStack UpdateDocumentByCustom(string[] listFileIds, string documentObjectType, object[] listFileBytes, string[] fileNames, string updateMode,
            DictionaryObjectInfo[] dicKeyValue, string sessionid, string userName, string jobId, string localVault)
        {
            return PLMWS.UpdateDocumentByCustom(listFileIds, documentObjectType, listFileBytes, fileNames, updateMode, dicKeyValue, sessionid, userName, jobId, localVault);
        }

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="listFileBytes">下载文件的二进制流数组（输出参数）</param>
        /// <param name="listFileNames">下载文件的名称（输出参数）</param>
        /// <param name="listFileIds">要下载的文档对象Id集</param>
        /// <param name="sessionid">会话Id（可选参数，如果没有sessionid就需要userName、jobId参数）</param>
        /// <param name="userName">用户名（可选参数）</param>
        /// <param name="jobId">所属岗位（可选参数）</param>
        /// <param name="localVault">文件柜名称（可选参数）</param>
        /// <returns>异常对象</returns>
        public static ErrorStack DownLoadDocumentByCustom(out object[] listFileBytes, out string[] listFileNames, string[] listFileIds, string sessionid, string userName, string jobId, string localVault)
        {
            return PLMWS.DownLoadDocumentByCustom(out listFileBytes, out listFileNames, listFileIds, sessionid, userName, jobId, localVault);
        }

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <param name="listFileIds">要删除的文档对象Id集</param>
        /// <param name="deleteMode">删除方式（1.删除当前版本，2.删除所有版本）</param>
        /// <param name="sessionid">会话Id（可选参数，如果没有sessionid就需要userName、jobId参数）</param>
        /// <param name="userName">用户名（可选参数）</param>
        /// <param name="jobId">所属岗位（可选参数）</param>
        /// <param name="localVault">文件柜名称（可选参数）</param>
        /// <returns>异常对象</returns>
        public static ErrorStack DeleteDocumentByCustom(string[] listFileIds, string deleteMode, string sessionid, string userName, string jobId, string localVault)
        {
            return PLMWS.DeleteDocumentByCustom(listFileIds, deleteMode, sessionid, userName, jobId, localVault);
        }




        /// <summary>
        /// 上传/保存文档(项目管理IRP)
        /// </summary>
        /// <param name="listDocument">项目文档对象数组，一个对象表示一个实例</param>
        /// <param name="saveMode">保存方式（1.一个文件生成一个文档对象,2.所有文件生成一个文档对象）</param>
        /// <param name="sessionid">会话Id（可选参数，如果没有sessionid就需要userName、jobId参数）</param>
        /// <param name="userName">用户名（可选参数）</param>
        /// <param name="jobId">所属岗位（可选参数）</param>
        /// <param name="localVault">文件柜名称（可选参数）</param>
        /// <param name="listResult">保存后的文档对象数组（输出参数）</param>
        /// <returns>异常对象</returns>
        public static ErrorStack AddDocumentByIRP(ProjectDocument[] listDocument, DocumentSaveMode saveMode,
         string sessionid, string userName, string jobId, string localVault, out ProjectDocument[] listResult)
        {
            return PLMWS.AddDocumentByCustomExtend(out listResult, listDocument, saveMode, sessionid, userName, jobId, localVault);
        }
        /// <summary>
        /// 上传/保存文档(知识库)
        /// </summary>
        /// <param name="listDocument">项目文档对象数组，一个对象表示一个实例</param>
        /// <param name="saveMode">保存方式（1.一个文件生成一个文档对象,2.所有文件生成一个文档对象）</param>
        /// <param name="sessionid">会话Id（可选参数，如果没有sessionid就需要userName、jobId参数）</param>
        /// <param name="userName">用户名（可选参数）</param>
        /// <param name="jobId">所属岗位（可选参数）</param>
        /// <param name="localVault">文件柜名称（可选参数）</param>
        /// <param name="listResult">保存后的文档对象数组（输出参数）</param>
        /// <returns>异常对象</returns>
        public static PLMWebServicesByKB.ErrorStack AddDocumentByKB(PLMWebServicesByKB.ProjectDocument[] listDocument, PLMWebServicesByKB.DocumentSaveMode saveMode,
         string sessionid, string userName, string jobId, string localVault, out PLMWebServicesByKB.ProjectDocument[] listResult)
        {
            return PLMWSByKB.AddDocumentByCustomExtend(out listResult, listDocument, saveMode, sessionid, userName, jobId, localVault);
        }

        /// <summary>
        /// 文件下载(项目管理IRP)
        /// </summary>
        /// <param name="listDocument">要下载的文档值对象集</param>
        /// <param name="sessionid">会话Id（可选参数，如果没有sessionid就需要userName、jobId参数）</param>
        /// <param name="userName">用户名（可选参数）</param>
        /// <param name="jobId">所属岗位（可选参数）</param>
        /// <param name="localVault">文件柜名称（可选参数）</param>
        /// <param name="listResult">可下载的文档对象数组（输出参数）</param>
        /// <returns>异常对象</returns>
        public static ErrorStack DownLoadDocumentByIRP(ProjectDocument[] listDocument, string sessionid, string userName, string jobId, string localVault, out ProjectDocument[] listResult)
        {
            return PLMWS.DownLoadDocumentByCustomExtend(out listResult, listDocument, sessionid, userName, jobId, localVault);
        }

        /// <summary>
        /// 文件下载(知识库)
        /// </summary>
        /// <param name="listDocument">要下载的文档值对象集</param>
        /// <param name="sessionid">会话Id（可选参数，如果没有sessionid就需要userName、jobId参数）</param>
        /// <param name="userName">用户名（可选参数）</param>
        /// <param name="jobId">所属岗位（可选参数）</param>
        /// <param name="localVault">文件柜名称（可选参数）</param>
        /// <param name="listResult">可下载的文档对象数组（输出参数）</param>
        /// <returns>异常对象</returns>
        public static PLMWebServicesByKB.ErrorStack DownLoadDocumentByKB(PLMWebServicesByKB.ProjectDocument[] listDocument, string sessionid, string userName, string jobId, string localVault, out PLMWebServicesByKB.ProjectDocument[] listResult)
        {
            return PLMWSByKB.DownLoadDocumentByCustomExtend(out listResult, listDocument, sessionid, userName, jobId, localVault);
        }


        /// <summary>
        /// 文件查询（项目管理IRP）
        /// </summary>
        /// <param name="projectCodeQuery">所属项目代码</param>
        /// <param name="projectNameQuery">所属项目名称</param>
        /// <param name="docCodeQuery">文档代码</param>
        /// <param name="docNameQuery">文档名称</param>
        /// <param name="docExtendNameQuery">文档扩展名</param>
        /// <param name="docInfoTypeQuery">文档信息类型</param>
        /// <param name="docVersionQuery">文档版本</param>
        /// <param name="docRevisionQuery">文档版次</param>
        /// <param name="docCreateBeginTimeQuery">文档创建起始时间</param>
        /// <param name="docCreateEndTimeQuery">文档创建结束时间</param>
        /// <param name="docOwnerQuery">文档创建者(用户名)</param>
        /// <param name="docStateQuery">文档状态</param>
        /// <param name="docKeyWordQuery">搜索关键字（在文档标题、作者、文档说明、文档关键字中搜索）</param>
        /// <param name="sessionid">会话Id（可选参数，如果没有sessionid就需要userName、jobId参数）</param>
        /// <param name="userName">用户名（可选参数）</param>
        /// <param name="jobId">所属岗位（可选参数）</param>
        /// <param name="localVault">文件柜名称（可选参数）</param>
        /// <param name="listResult">返回结果集（输出参数）</param>
        /// <returns>异常对象</returns>
        public static ErrorStack GetProjectDocumentByIRP(string projectCodeQuery, string projectNameQuery, string docCodeQuery, string docNameQuery, string docExtendNameQuery,
            DocumentInfoType? docInfoTypeQuery, DocumentQueryVersion queryMode, string docVersionQuery, string docRevisionQuery, DateTime? docCreateBeginTimeQuery, DateTime? docCreateEndTimeQuery, string docOwnerQuery,
            DocumentState? docStateQuery, string docKeyWordQuery, string sessionid, string userName, string jobId, string localVault, out ProjectDocument[] listResult)
        {

            return PLMWS.GetProjectDocumentByCustomExtend(projectCodeQuery, projectNameQuery, docCodeQuery, docNameQuery, docExtendNameQuery, docInfoTypeQuery, queryMode, docVersionQuery,
docRevisionQuery, docCreateBeginTimeQuery, docCreateEndTimeQuery, docOwnerQuery, docStateQuery, docKeyWordQuery, sessionid, userName, jobId, localVault, out listResult);

        }

        /// <summary>
        /// 文件查询(知识库)
        /// </summary>
        /// <param name="projectCodeQuery">所属项目代码</param>
        /// <param name="projectNameQuery">所属项目名称</param>
        /// <param name="docCodeQuery">文档代码</param>
        /// <param name="docNameQuery">文档名称</param>
        /// <param name="docExtendNameQuery">文档扩展名</param>
        /// <param name="docInfoTypeQuery">文档信息类型</param>
        /// <param name="docVersionQuery">文档版本</param>
        /// <param name="docRevisionQuery">文档版次</param>
        /// <param name="docCreateBeginTimeQuery">文档创建起始时间</param>
        /// <param name="docCreateEndTimeQuery">文档创建结束时间</param>
        /// <param name="docOwnerQuery">文档创建者(用户名)</param>
        /// <param name="docStateQuery">文档状态</param>
        /// <param name="docKeyWordQuery">搜索关键字（在文档标题、作者、文档说明、文档关键字中搜索）</param>
        /// <param name="sessionid">会话Id（可选参数，如果没有sessionid就需要userName、jobId参数）</param>
        /// <param name="userName">用户名（可选参数）</param>
        /// <param name="jobId">所属岗位（可选参数）</param>
        /// <param name="localVault">文件柜名称（可选参数）</param>
        /// <param name="listResult">返回结果集（输出参数）</param>
        /// <returns>异常对象</returns>
        public static PLMWebServicesByKB.ErrorStack GetProjectDocumentByKB(string projectCodeQuery, string projectNameQuery, string docCodeQuery, string docNameQuery, string docExtendNameQuery,
            PLMWebServicesByKB.DocumentInfoType? docInfoTypeQuery, PLMWebServicesByKB.DocumentQueryVersion queryMode, string docVersionQuery, string docRevisionQuery, DateTime? docCreateBeginTimeQuery, DateTime? docCreateEndTimeQuery, string docOwnerQuery,
            PLMWebServicesByKB.DocumentState? docStateQuery, string docKeyWordQuery, string sessionid, string userName, string jobId, string localVault, out PLMWebServicesByKB.ProjectDocument[] listResult)
        {

            return PLMWSByKB.GetProjectDocumentByCustomExtend(projectCodeQuery, projectNameQuery, docCodeQuery, docNameQuery, docExtendNameQuery, docInfoTypeQuery, queryMode, docVersionQuery,
docRevisionQuery, docCreateBeginTimeQuery, docCreateEndTimeQuery, docOwnerQuery, docStateQuery, docKeyWordQuery, sessionid, userName, jobId, localVault, out listResult);

        }


        /// <summary>
        /// 文件查询（项目管理IRP）
        /// </summary>
        /// <param name="projectCodeQuery">所属项目代码</param>
        /// <param name="projectNameQuery">所属项目名称</param>
        /// <param name="docCodeQuery">文档代码</param>
        /// <param name="docNameQuery">文档名称</param>
        /// <param name="docExtendNameQuery">文档扩展名</param>
        /// <param name="docInfoTypeQuery">文档信息类型</param>
        /// <param name="docVersionQuery">文档版本</param>
        /// <param name="docRevisionQuery">文档版次</param>
        /// <param name="docCreateBeginTimeQuery">文档创建起始时间</param>
        /// <param name="docCreateEndTimeQuery">文档创建结束时间</param>
        /// <param name="docOwnerQuery">文档创建者(用户名)</param>
        /// <param name="docStateQuery">文档状态</param>
        /// <param name="docKeyWordQuery">搜索关键字（在文档标题、作者、文档说明、文档关键字中搜索）</param>
        /// <param name="sessionid">会话Id（可选参数，如果没有sessionid就需要userName、jobId参数）</param>
        /// <param name="userName">用户名（可选参数）</param>
        /// <param name="jobId">所属岗位（可选参数）</param>
        /// <param name="localVault">文件柜名称（可选参数）</param>
        /// <param name="listResult">返回结果集（输出参数）</param>
        /// <returns>异常对象</returns>
        public static ErrorStack GetProjectDocumentByIRP(string[] fileIds, DocumentQueryVersion queryMode, string sessionid, string userName, string jobId, string localVault, out ProjectDocument[] listResult)
        {

            return PLMWS.GetProjectDocumentByDocumentId(out listResult, fileIds, queryMode, sessionid, userName, jobId, localVault);

        }

        /// <summary>
        /// 文件查询（知识库）
        /// </summary>
        /// <param name="projectCodeQuery">所属项目代码</param>
        /// <param name="projectNameQuery">所属项目名称</param>
        /// <param name="docCodeQuery">文档代码</param>
        /// <param name="docNameQuery">文档名称</param>
        /// <param name="docExtendNameQuery">文档扩展名</param>
        /// <param name="docInfoTypeQuery">文档信息类型</param>
        /// <param name="docVersionQuery">文档版本</param>
        /// <param name="docRevisionQuery">文档版次</param>
        /// <param name="docCreateBeginTimeQuery">文档创建起始时间</param>
        /// <param name="docCreateEndTimeQuery">文档创建结束时间</param>
        /// <param name="docOwnerQuery">文档创建者(用户名)</param>
        /// <param name="docStateQuery">文档状态</param>
        /// <param name="docKeyWordQuery">搜索关键字（在文档标题、作者、文档说明、文档关键字中搜索）</param>
        /// <param name="sessionid">会话Id（可选参数，如果没有sessionid就需要userName、jobId参数）</param>
        /// <param name="userName">用户名（可选参数）</param>
        /// <param name="jobId">所属岗位（可选参数）</param>
        /// <param name="localVault">文件柜名称（可选参数）</param>
        /// <param name="listResult">返回结果集（输出参数）</param>
        /// <returns>异常对象</returns>
        public static PLMWebServicesByKB.ErrorStack GetProjectDocumentByKB(string[] fileIds, PLMWebServicesByKB.DocumentQueryVersion queryMode, string sessionid, string userName, string jobId, string localVault, out PLMWebServicesByKB.ProjectDocument[] listResult)
        {
            return PLMWSByKB.GetProjectDocumentByDocumentId(out listResult, fileIds, queryMode, sessionid, userName, jobId, localVault);
        }


        /// <summary>
        /// 修改文档对象(项目管理IRP)
        /// </summary>
        /// <param name="listDocument">要修改的文档对象集</param>
        /// <param name="updateMode">更新方式（1.添加一个新版次文件，2.覆盖原有最新版次文件）</param>
        /// <param name="sessionid">会话Id（可选参数，如果没有sessionid就需要userName、jobId参数）</param>
        /// <param name="userName">用户名（可选参数）</param>
        /// <param name="jobId">所属岗位（可选参数）</param>
        /// <param name="localVault">文件柜名称（可选参数）</param>
        /// <param name="listResult">修改后的文档对象集（输出参数）</param>
        /// <returns>异常对象</returns>
        public static ErrorStack UpdateDocumentByIRP(ProjectDocument[] listDocument, DocumentUpdateMode updateMode, string sessionid, string userName, string jobId, string localVault, out ProjectDocument[] listResult)
        {
            return PLMWS.UpdateDocumentByCustomExtend(out listResult, listDocument, updateMode, sessionid, userName, jobId, localVault);
        }

        /// <summary>
        /// 修改文档对象(知识库)
        /// </summary>
        /// <param name="listDocument">要修改的文档对象集</param>
        /// <param name="updateMode">更新方式（1.添加一个新版次文件，2.覆盖原有最新版次文件）</param>
        /// <param name="sessionid">会话Id（可选参数，如果没有sessionid就需要userName、jobId参数）</param>
        /// <param name="userName">用户名（可选参数）</param>
        /// <param name="jobId">所属岗位（可选参数）</param>
        /// <param name="localVault">文件柜名称（可选参数）</param>
        /// <param name="listResult">修改后的文档对象集（输出参数）</param>
        /// <returns>异常对象</returns>
        public static PLMWebServicesByKB.ErrorStack UpdateDocumentByKB(PLMWebServicesByKB.ProjectDocument[] listDocument, PLMWebServicesByKB.DocumentUpdateMode updateMode, string sessionid, string userName, string jobId, string localVault, out PLMWebServicesByKB.ProjectDocument[] listResult)
        {
            return PLMWSByKB.UpdateDocumentByCustomExtend(out listResult, listDocument, updateMode, sessionid, userName, jobId, localVault);
        }

        /// <summary>
        /// 删除文档对象(项目管理IRP)
        /// </summary>
        /// <param name="listDocument">要删除的文档对象集</param>
        /// <param name="deleteMode">删除方式（1.删除最新版本，2.删除所有版本）</param>
        /// <param name="sessionid">会话Id（可选参数，如果没有sessionid就需要userName、jobId参数）</param>
        /// <param name="userName">用户名（可选参数）</param>
        /// <param name="jobId">所属岗位（可选参数）</param>
        /// <param name="localVault">文件柜名称（可选参数）</param>
        /// <returns>异常对象</returns>
        public static ErrorStack DeleteDocumentByIRP(ProjectDocument[] listDocument, DocumentDeleteMode deleteMode, string sessionid, string userName, string jobId, string localVault)
        {
            return PLMWS.DeleteDocumentByCustomExtend(listDocument, deleteMode, sessionid, userName, jobId, localVault);
        }

        /// <summary>
        /// 删除文档对象(知识库)
        /// </summary>
        /// <param name="listDocument">要删除的文档对象集</param>
        /// <param name="deleteMode">删除方式（1.删除最新版本，2.删除所有版本）</param>
        /// <param name="sessionid">会话Id（可选参数，如果没有sessionid就需要userName、jobId参数）</param>
        /// <param name="userName">用户名（可选参数）</param>
        /// <param name="jobId">所属岗位（可选参数）</param>
        /// <param name="localVault">文件柜名称（可选参数）</param>
        /// <returns>异常对象</returns>
        public static PLMWebServicesByKB.ErrorStack DeleteDocumentByKB(PLMWebServicesByKB.ProjectDocument[] listDocument, PLMWebServicesByKB.DocumentDeleteMode deleteMode, string sessionid, string userName, string jobId, string localVault)
        {
            return PLMWSByKB.DeleteDocumentByCustomExtend(listDocument, deleteMode, sessionid, userName, jobId, localVault);
        }

        /// <summary>
        /// 项目文档分类类型
        /// </summary>
        public enum ProjectDocumentCategoryTypeEnum
        {
            /// <summary>
            /// 知识库
            /// </summary>
            CLASSDOCUMENT = 1,
            /// <summary>
            ///项目文档库 
            /// </summary>
            CLASSIRPDOCUMENT = 2
        }

        /// <summary>
        /// 项目类型
        /// </summary>
        public enum ProjectInfoTypeEnum
        {
            /// <summary>
            /// 知识库
            /// </summary>
            KB = 1
        }

        /// <summary>
        /// 知识库分类查询
        /// </summary>
        /// <param name="CategoryTypeName">分类对象类型名称（知识库里的分类对象名称）</param>
        /// <param name="queryMode">查询方式（1.树方式,2表方式）</param>
        /// <param name="sessionid">会话Id（可选参数，如果没有sessionid就需要userName、jobId参数）</param>
        /// <param name="userName">用户名（可选参数）</param>
        /// <param name="jobId">所属岗位（可选参数）</param>
        /// <param name="localVault">文件柜名称（可选参数）</param>
        /// <param name="listResult">查询分类结果集（输出参数）</param>
        /// <returns>异常对象</returns>
        public static PLMWebServicesByKB.ErrorStack GetDocumentCategoryByKB(ProjectDocumentCategoryTypeEnum projectCateType, PLMWebServicesByKB.CategoryQueryModeEnum queryMode, string queryKeyWords, string sessionid, string userName,
            string jobId, string localVault, out PLMWebServicesByKB.CategoryNode[] listResult)
        {
            return PLMWSByKB.GetCategoryByCustomExtend(out listResult, projectCateType.ToString(), queryMode, queryKeyWords, sessionid, userName, jobId, localVault);
        }

        /// <summary>
        /// 项目管理（IRP）分类查询
        /// </summary>
        /// <param name="CategoryTypeName">分类对象类型名称（项目管理里的分类对象名称）</param>
        /// <param name="queryMode">查询方式（1.树方式,2表方式）</param>
        /// <param name="sessionid">会话Id（可选参数，如果没有sessionid就需要userName、jobId参数）</param>
        /// <param name="userName">用户名（可选参数）</param>
        /// <param name="jobId">所属岗位（可选参数）</param>
        /// <param name="localVault">文件柜名称（可选参数）</param>
        /// <param name="listResult">查询分类结果集（输出参数）</param>
        /// <returns>异常对象</returns>
        public static ErrorStack GetDocumentCategoryByIRP(ProjectDocumentCategoryTypeEnum projectCateType, CategoryQueryModeEnum queryMode, string queryKeyWords, string sessionid, string userName,
            string jobId, string localVault, out PLMWebServices.CategoryNode[] listResult)
        {
            return PLMWS.GetCategoryByCustomExtend(out listResult, projectCateType.ToString(), queryMode, queryKeyWords, sessionid, userName, jobId, localVault);
        }

        #endregion

        #region 树结构
        private static IGWBSTreeSrv treeCateSrv = null;
        /// <summary>
        /// 分类树操作服务
        /// </summary>
        public static IGWBSTreeSrv TreeCateSrv
        {
            get
            {
                if (treeCateSrv == null)
                    treeCateSrv = StaticMethod.GetService("GWBSTreeSrv") as IGWBSTreeSrv;

                return StaticMethod.treeCateSrv;
            }
            set { StaticMethod.treeCateSrv = value; }
        }

        /// <summary>
        /// 获取分类对象的完整路径
        /// </summary>
        /// <param name="nodeObj"></param>
        /// <returns></returns>
        public static string GetCategorTreeFullPath(Type cateEntityType, VirtualMachine.Patterns.CategoryTreePattern.Domain.CategoryNode nodeObj)
        {
            return TreeCateSrv.GetCategorTreeFullPath(cateEntityType, nodeObj);
        }

        /// <summary>
        /// 获取分类对象的完整路径
        /// </summary>
        /// <param name="cateEntityType"></param>
        /// <param name="nodeName"></param>
        /// <param name="nodeSysCode"></param>
        /// <returns></returns>
        public static string GetCategorTreeFullPath(Type cateEntityType, string nodeName, string nodeSysCode)
        {
            return TreeCateSrv.GetCategorTreeFullPath(cateEntityType, nodeName, nodeSysCode);
        }

        ///// <summary>
        ///// 获取分类对象的完整路径
        ///// </summary>
        ///// <param name="nodeObj"></param>
        ///// <returns></returns>
        //public static string GetCategorTreeFullPath2222(Type cateEntityType, CategoryNode nodeObj)
        //{
        //    string path = string.Empty;

        //    path = nodeObj.Name;

        //    ObjectQuery oq = new ObjectQuery();
        //    oq.AddCriterion(Expression.Eq("Id", nodeObj.Id));
        //    oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
        //    IList list = TreeCateSrv.ObjectQuery(cateEntityType, oq);

        //    nodeObj = list[0] as CategoryNode;

        //    CategoryNode parent = nodeObj.ParentNode;
        //    while (parent != null)
        //    {
        //        path = parent.Name + "\\" + path;

        //        oq.Criterions.Clear();
        //        oq.Criterions.Clear();
        //        oq.AddCriterion(Expression.Eq("Id", parent.Id));
        //        oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
        //        list = TreeCateSrv.ObjectQuery(cateEntityType, oq);

        //        parent = (list[0] as CategoryNode).ParentNode;
        //    }

        //    return path;
        //}
        #endregion

        /// <summary>
        /// 去掉decimal尾数无用的0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DecimelTrimEnd0(decimal value)
        {
            string valueStr = value.ToString();
            if (valueStr.IndexOf(".") > -1)
            {
                valueStr = valueStr.TrimEnd('0');
                if (valueStr.IndexOf(".") == valueStr.Length - 1)
                    valueStr = valueStr.Substring(0, valueStr.Length - 1);
            }
            return valueStr;
        }

        /// <summary>
        /// 时间格式化字符串，处理1900-1-1不显示的情况
        /// </summary>
        /// <param name="date"></param>
        /// <param name="longDate"></param>
        /// <returns></returns>
        public static string GetShowDateTimeStr(DateTime date, bool longDate)
        {
            return date == DateTime.Parse("1900-1-1") ? "" : longDate ? date.ToLongDateString() : date.ToShortDateString();
        }

        /// <summary>
        /// 获取检查状态显示的文本（WBS日常检查等，0：未检查；1：检查未通过；2：检查通过；X：无需检查。）
        /// </summary>
        /// <param name="checkStateStr"></param>
        /// <returns></returns>
        public static string GetCheckStateShowText(string checkStateStr)
        {
            if (string.IsNullOrEmpty(checkStateStr))
                return "";

            string checkRequireShow = string.Empty;


            //string[] checkRequireName = new string[] { "工长质检", "质检员质检", "监理质检", "工程进度", "安全专业", "物资专业", "技术专业" };

            List<BasicDataOptr> checkRequireList1 = StaticMethod.GetBasicDataByName(VBasicDataOptr.WBS_CheckRequire).OfType<BasicDataOptr>().ToList();
            var dailyCheckStateList = from dcs in checkRequireList1
                                      orderby dcs.BasicCode ascending
                                      select dcs;
            int length = dailyCheckStateList.Count();
            int count = 0;
            BasicDataOptr[] checkRequire = dailyCheckStateList.ToArray();

            int index = 0;
            foreach (char c in checkStateStr.Substring(0, length))
            {
                switch (c)
                {
                    case '0':
                        checkRequireShow += (checkRequire[index].BasicName + "未检查" + "/");
                        break;
                    case '1':
                        checkRequireShow += (checkRequire[index].BasicName + "检查未通过" + "/");
                        break;
                    case '2':
                        checkRequireShow += (checkRequire[index].BasicName + "检查通过" + "/");
                        break;
                    case 'X':
                        checkRequireShow += (checkRequire[index].BasicName + "无需检查" + "/");
                        count++;
                        break;
                }
                index++;

            }

            if (count == length)
                checkRequireShow = "无需检查";
            else if (checkRequireShow.Length > 0)
                checkRequireShow = checkRequireShow.Substring(0, checkRequireShow.Length - 1);

            return checkRequireShow;
        }

        /// <summary>
        /// 获取检查要求显示的文本（WBS检查要求，0：未检查；1：检查通过；2：罚款后检查通过；3：检查未通过；4：检查中；X：无需检查。）
        /// </summary>
        /// <param name="checkStateStr"></param>
        /// <returns></returns>
        public static string GetCheckRequireShowText(string checkStateStr)
        {
            if (string.IsNullOrEmpty(checkStateStr))
                return "";

            string checkRequireShow = string.Empty;

            string[] checkRequireName = new string[] { "工长质检", "质检员质检", "监理质检", "工程进度", "安全专业", "物资专业", "技术专业", "", "", "", "", "工程量确认" };

            int index = 0;
            foreach (char c in checkStateStr)
            {
                if (index < checkRequireName.Length && !string.IsNullOrEmpty(checkRequireName[index]))
                {
                    if (index == 11)//工程量确认标志
                    {
                        if (c == '0')
                        {
                            checkRequireShow += (checkRequireName[index] + "未确认");
                        }
                        if (c == '1')
                        {
                            checkRequireShow += (checkRequireName[index] + "确认");
                        }
                    }
                    else
                    {
                        switch (c)
                        {
                            case '0':
                                checkRequireShow += (checkRequireName[index] + "未检查" + "/");
                                break;
                            case '1':
                                checkRequireShow += (checkRequireName[index] + "检查通过" + "/");
                                break;
                            case '2':
                                checkRequireShow += (checkRequireName[index] + "罚款后检查通过" + "/");
                                break;
                            case '3':
                                checkRequireShow += (checkRequireName[index] + "检查未通过" + "/");
                                break;
                            case '4':
                                checkRequireShow += (checkRequireName[index] + "检查中" + "/");
                                break;
                            case 'X':
                                checkRequireShow += (checkRequireName[index] + "无需检查" + "/");
                                break;
                        }
                    }
                }


                index++;
            }

            if (checkRequireShow.Length > 0 && checkRequireShow.LastIndexOf("/") == checkRequireShow.Length - 1)
                checkRequireShow = checkRequireShow.Substring(0, checkRequireShow.Length - 1);

            return checkRequireShow;

        }

        /// <summary>
        /// 获取日常检查通过状态文本("通过"或"未通过")
        /// </summary>
        /// <param name="checkState"></param>
        /// <returns></returns>
        public static string GetCheckStatePassStr(string checkState)
        {
            if (string.IsNullOrEmpty(checkState))
                return "通过";
            else if (checkState.Length == 12)//第12位为工程量确认标志，前面为检查标志
                checkState = checkState.Substring(0, 11);

            return (checkState.IndexOf("0") == -1 && checkState.IndexOf("1") == -1) ? "通过" : "未通过";
        }

        /// <summary>
        /// 获取节点类型枚举的中文说明
        /// </summary>
        /// <param name="nodeType"></param>
        /// <returns></returns>
        public static string GetNodeTypeStr(VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType nodeType)
        {
            string result = "";
            switch (nodeType)
            {
                case NodeType.LeafNode:
                    result = "叶节点";
                    break;
                case NodeType.MiddleNode:
                    result = "中间节点";
                    break;
                case NodeType.RootNode:
                    result = "根节点";
                    break;
                default:
                    break;
            }
            return result;
        }

        /// <summary>
        /// 获取指定项目任务下任务明细的合同合价、责任合价、计划合价数据
        /// </summary>
        /// <param name="targetTask"></param>
        /// <returns>[合同合价，责任合价，计划合价]</returns>
        public static List<decimal> GetTaskDtlTotalPrice(GWBSTree targetTask)
        {
            List<decimal> listResult = new List<decimal>();

            decimal contractTotalPrice = 0;
            decimal responsibilitilyTotalPrice = 0;
            decimal planTotalPrice = 0;

            List<GWBSDetail> listDtl = (from d in targetTask.Details
                                        where d.State != VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Invalid
                                        select d).ToList();

            var query = from d in listDtl
                        where d.CostingFlag == 1
                        select d;

            if (query.Count() == 0)//如果不包含成本核算明细，取下面所有生产明细的计划和责任明细的合价
            {
                foreach (GWBSDetail detail in listDtl)
                {
                    contractTotalPrice += detail.ContractTotalPrice;
                    responsibilitilyTotalPrice += detail.ResponsibilitilyTotalPrice;
                    planTotalPrice += detail.PlanTotalPrice;
                }
            }
            else//如果包含成本核算明细，则取成本核算明细的计划和责任明细的合价
            {
                foreach (GWBSDetail dtl in query)
                {
                    contractTotalPrice += dtl.ContractTotalPrice;
                    responsibilitilyTotalPrice += dtl.ResponsibilitilyTotalPrice;
                    planTotalPrice += dtl.PlanTotalPrice;
                }
            }


            listResult.Add(contractTotalPrice);
            listResult.Add(responsibilitilyTotalPrice);
            listResult.Add(planTotalPrice);

            return listResult;

        }

        /// <summary>
        /// 获取指定项目任务下任务明细的合同合价、责任合价、计划合价数据
        /// </summary>
        /// <param name="targetTask"></param>
        /// <returns>[合同合价，责任合价，计划合价]</returns>
        public static List<decimal> GetTaskDtlTotalPrice(IList list)
        {
            List<decimal> listResult = new List<decimal>();

            decimal contractTotalPrice = 0;
            decimal responsibilitilyTotalPrice = 0;
            decimal planTotalPrice = 0;

            List<GWBSDetail> listDtl = (from d in list.OfType<GWBSDetail>()
                                        where d.State != VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Invalid
                                        select d).ToList();

            var query = from d in listDtl
                        where d.CostingFlag == 1
                        select d;

            if (query.Count() == 0)//如果不包含成本核算明细，取下面所有生产明细的计划和责任明细的合价
            {
                foreach (GWBSDetail detail in listDtl)
                {
                    contractTotalPrice += detail.ContractTotalPrice;
                    responsibilitilyTotalPrice += detail.ResponsibilitilyTotalPrice;
                    planTotalPrice += detail.PlanTotalPrice;
                }
            }
            else//如果包含成本核算明细，则取成本核算明细的计划和责任明细的合价
            {
                foreach (GWBSDetail dtl in query)
                {
                    contractTotalPrice += dtl.ContractTotalPrice;
                    responsibilitilyTotalPrice += dtl.ResponsibilitilyTotalPrice;
                    planTotalPrice += dtl.PlanTotalPrice;
                }
            }

            listResult.Add(contractTotalPrice);
            listResult.Add(responsibilitilyTotalPrice);
            listResult.Add(planTotalPrice);

            return listResult;

        }

        /// <summary>
        /// 获取项目任务显示的状态
        /// </summary>
        /// <param name="taskState"></param>
        /// <returns></returns>
        public static string GetWBSTaskStateText(VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState taskState)
        {
            if (taskState == VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute)
                return "执行中";
            else
                return ClientUtil.GetDocStateName(taskState);
        }
        /// <summary>
        /// 获取任务核算单的状态
        /// </summary>
        /// <param name="taskState"></param>
        /// <returns></returns>
        public static string GetProjectTaskAccountBillStateText(VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState taskState)
        {
            if (taskState == VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Valid)
                return "提交";
            else
                return ClientUtil.GetDocStateName(taskState);
        }
        /// <summary>
        /// 得到默认文件柜
        /// </summary>
        /// <returns></returns>
        public static FileCabinet GetDefaultFileCabinet()
        {
            FileCabinet appFileCabinet = null;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("UsedState", UseState.启用));
            IList listFileCabinet = TreeCateSrv.ObjectQuery(typeof(FileCabinet), oq);
            if (listFileCabinet.Count > 0)
            {
                appFileCabinet = listFileCabinet[0] as FileCabinet;
            }
            return appFileCabinet;
        }

        private static bool _isEnabledDataAuth = false;
        /// <summary>
        /// 是否启用数据权限
        /// </summary>
        public static bool IsEnabledDataAuth
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationSettings.AppSettings["IsEnabledDataAuth"]))
                    _isEnabledDataAuth = Convert.ToBoolean(ConfigurationSettings.AppSettings["IsEnabledDataAuth"]);
                return StaticMethod._isEnabledDataAuth;
            }
            set { StaticMethod._isEnabledDataAuth = value; }
        }

        /// <summary>
        /// 得到照片在客户端的路径（显示）
        /// </summary>
        /// <param name="theFileCabinetId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetPicturePath(string theFileCabinetId, string fileName)
        {
            string path = "";
            FileCabinet appFileCabinet = null;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", theFileCabinetId));
            IList listFileCabinet = TreeCateSrv.ObjectQuery(typeof(FileCabinet), oq);
            if (listFileCabinet != null && listFileCabinet.Count > 0)
            {
                appFileCabinet = listFileCabinet[0] as FileCabinet;
                string fileFullPath1 = AppDomain.CurrentDomain.BaseDirectory + "TempFilePreview\\" + Guid.NewGuid().ToString().Replace("-", "");
                if (!Directory.Exists(fileFullPath1))
                    Directory.CreateDirectory(fileFullPath1);
                string tempFileFullPath = fileFullPath1 + @"\\" + fileName;

                //string address = "http://10.70.18.203//TestFile//Files//0001//0001//0001.0001.000027V_特效.jpg";
                string baseAddress = @appFileCabinet.TransportProtocal.ToString().ToLower() + "://" + appFileCabinet.ServerName + "/" + appFileCabinet.Path + "/";

                string address = baseAddress + "PersonPicture//" + fileName;
                try
                {
                    IRPServiceModel.Basic.UtilityClass.WebClientObj.DownloadFile(address, tempFileFullPath);
                    path = tempFileFullPath;
                }
                catch (Exception)
                {
                }
            }
            return path;
        }
    }
}
