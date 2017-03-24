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
        /// ��������
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
        /// �Զ��л����ݵ���Ŀ��ID \n
        /// 
        /// </summary>
        public static string AutoSwitchProjectId
        {
            get { return autoSwitchProjectId; }
            set { autoSwitchProjectId = value; }
        }

        /// <summary>
        /// IRP���ݵĲ˵�����
        /// </summary>
        public static string IRPMenuName
        {
            get { return _IRPMenuName; }
            set { _IRPMenuName = value; }
        }

        /// <summary>
        /// ��¼ϵͳ����
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
        /// ����ӡ���
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
        /// ҵ����
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
        /// ��λ
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
        /// ��ɫ
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
        /// �Ƿ�����ϵͳ����Ա��ɫ
        /// </summary>
        /// <returns></returns>
        private static bool IsSystemAdministratorRole()
        {
            bool isAdmin = false;
            if (ConstObject.TheRoles != null && ConstObject.TheRoles.Count > 0)
            {
                var query = from r in ConstObject.TheRoles
                            where r.State == 1 && r.RoleName == "ϵͳ����Ա"
                            select r;

                if (query.Count() > 0)
                    isAdmin = true;
            }
            return isAdmin;
        }

        /// <summary>
        /// �Ƿ���ϵͳ����Ա��λ
        /// </summary>
        /// <returns></returns>
        private static bool IsSystemAdministratorJob()
        {
            bool isAdmin = false;
            if (ConstObject.TheSysRole != null && ConstObject.TheSysRole.State == 1 && ConstObject.TheSysRole.RoleName == "ϵͳ����Ա")
            {
                isAdmin = true;
            }
            return isAdmin;
        }

        /// <summary>
        /// �Ƿ���ϵͳ����Ա
        /// </summary>
        /// <returns></returns>
        public static bool IsSystemAdministrator()
        {
            return (IsSystemAdministratorJob() || IsSystemAdministratorRole());
        }

        /// <summary>
        /// �ƶ��豸�������ű���
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
        /// ��¼����
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
        /// ��λ��
        /// </summary>
        public static CurrencyInfo StandardCurrency
        {
            get { return standardCurrency; }
            set { standardCurrency = value; }
        }
        /// <summary>
        /// ��¼��Ա
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
                    throw new Exception("AppContextδ���г�ʼ����");
                return ctx;
            }
            set { ctx = value; }
        }

        public static string Connection()
        {
            string result = "";
            string serverIP = System.Configuration.ConfigurationManager.AppSettings["ProIp"];
            Ping p = new Ping();//����Ping����p 
            PingReply pr = p.Send(serverIP);//��ָ��IP�����������ļ��������ICMPЭ���ping���ݰ� 
            //return pr.Status;
            if (pr.Status == IPStatus.Success)//���ping�ɹ�             
            {
                result = "����";
            }
            else
            {
                //Thread.Sleep(100000);//�ȴ�ʮ����(������ԵĻ�������Ը�Ϊ1000)
                result = "�Ͽ�";
            }
            return result;
        }
        /// <summary>
        /// ������������������״̬
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
            if (strRst.IndexOf("(0% loss)") != -1 || strRst.IndexOf("(0% ��ʧ)") != -1)
            {
                string s = "time=";
                string s1 = "ʱ��=";
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
                    pingrst = "ǿ";
                }
                else if (time < 100)
                {
                    pingrst = "��";
                }
                else
                {
                    pingrst = "��";
                }
                //pingrst = "����";
            }
            else if (strRst.IndexOf("Destination host unreachable.") != -1)
                pingrst = "�޷���������";
            else if (strRst.IndexOf("Request timed out.") != -1)
                pingrst = "���ӳ�ʱ";
            else if (strRst.IndexOf("Unknown host") != -1)
                pingrst = "�޷���������";
            else
                pingrst = "�Ͽ�";

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
                    var startIndex2 = Math.Max(pingrst.LastIndexOf("ʱ��="), pingrst.LastIndexOf("ʱ��<")) + 3;
                    var startIndex = Math.Max(startIndex1, startIndex2);
                    var time = Convert.ToInt32(pingrst.Substring(startIndex, endIndex - startIndex));
                    if (time <= 30 && time > 0)
                    {
                        pingrst = "ǿ";
                    }
                    else if (time < 100)
                    {
                        pingrst = "��";
                    }
                    else
                    {
                        pingrst = "��";
                    }
                }
            }

            return pingrst;
        }

        /// <summary>
        /// ��ȡ����״̬
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
                        sState = "�༭";
                        break;
                    }
                case VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Valid:
                    {
                        sState = "��Ч";
                        break;
                    }
                case VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Invalid:
                    {
                        sState = "��Ч";
                        break;
                    }
                case VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InAudit:
                    {
                        sState = "������";
                        break;
                    }
                case VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Suspend:
                    {
                        sState = "����";
                        break;
                    }
                case VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute:
                    {
                        sState = "ִ����";
                        break;
                    }
                case VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Freeze:
                    {
                        sState = "����";
                        break;
                    }
                case VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Completed:
                    {
                        sState = "����";
                        break;
                    }
                default:
                    {
                        sState = "δ֪";
                        break;
                    }

            }
            return sState;
        }
    }

    /// <summary>
    /// �����غ���
    /// </summary>
    public class CurrencyComUtil
    {
        /// <summary>
        /// �����ַ�ת���ɴ�д���
        /// </summary>
        /// <param name="moneyStr">����ִ�</param>
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
        /// ���ݽ��ת���ɴ�д���
        /// </summary>
        /// <param name="Money">���</param>
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

            mstrSource = mstrSource.Replace("0", "��");
            mstrSource = mstrSource.Replace("1", "Ҽ");
            mstrSource = mstrSource.Replace("2", "��");
            mstrSource = mstrSource.Replace("3", "��");
            mstrSource = mstrSource.Replace("4", "��");
            mstrSource = mstrSource.Replace("5", "��");
            mstrSource = mstrSource.Replace("6", "½");
            mstrSource = mstrSource.Replace("7", "��");
            mstrSource = mstrSource.Replace("8", "��");
            mstrSource = mstrSource.Replace("9", "��");
            mstrSource = mstrSource.Replace("M", "��");
            mstrSource = mstrSource.Replace("W", "��");
            mstrSource = mstrSource.Replace("S", "Ǫ");
            mstrSource = mstrSource.Replace("H", "��");
            mstrSource = mstrSource.Replace("T", "ʰ");
            mstrSource = mstrSource.Replace("Y", "Բ");
            mstrSource = mstrSource.Replace("J", "��");
            mstrSource = mstrSource.Replace("F", "��");
            if (mstrSource.Substring(mstrSource.Length - 1, 1) != "��")
            {
                mstrSource = mstrSource + "��";
                //switch (mstrSource.Substring(mstrSource.Length - 1, 1))
                //{
                //    case "��":
                //        mstrSource = mstrSource + "���";
                //        break;
                //    case "Բ":
                //        mstrSource = mstrSource + "������";
                //        break;
                //    case "ʰ":
                //        mstrSource = mstrSource + "��Բ������";
                //        break;
                //    case "��":
                //        mstrSource = mstrSource + "��ʰ��Բ������";
                //        break;
                //    case "Ǫ":
                //        mstrSource = mstrSource + "�����ʰ��Բ������";
                //        break;
                //    case "��":
                //        mstrSource = mstrSource + "��Ǫ�����ʰ��Բ������";
                //        break;


                //    default:
                //        break;
                //}
            }

            //��
            if (Money < 0)
            {
                mstrSource = "��" + mstrSource;
            }
            return mstrSource;
        }

        //ת������
        private char CharToNum(char x)
        {
            string stringChnNames = "��һ�����������߰˾�";
            string stringNumNames = "0123456789";
            return stringChnNames[stringNumNames.IndexOf(x)];
        }

        //ת������������
        private string WanStrToInt(string x)
        {
            string[] stringArrayLevelNames = new string[4] { "", "ʮ", "��", "ǧ" };
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
            while ((i = ret.IndexOf("����")) != -1)
            {
                ret = ret.Remove(i, 1);
            }
            if (ret[ret.Length - 1] == '��' && ret.Length > 1)
            {
                ret = ret.Remove(ret.Length - 1, 1);
            }
            if (ret.Length >= 2 && ret.Substring(0, 2) == "һʮ")
            {
                ret = ret.Remove(0, 1);
            }
            return ret;
        }
        //ת������
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
                ret = WanStrToInt(x.Substring(0, len - 4)) + "��";
                temp = WanStrToInt(x.Substring(len - 4, 4));
                if (temp.IndexOf("ǧ") == -1 && temp != "")
                    ret += "��" + temp;
                else
                    ret += temp;
            }
            else
            {
                ret = WanStrToInt(x.Substring(0, len - 8)) + "��";
                temp = WanStrToInt(x.Substring(len - 8, 4));
                if (temp.IndexOf("ǧ") == -1 && temp != "")
                {
                    ret += "��" + temp;
                }
                else
                {
                    ret += temp;
                }
                ret += "��";
                temp = WanStrToInt(x.Substring(len - 4, 4));
                if (temp.IndexOf("ǧ") == -1 && temp != "")
                {
                    ret += "��" + temp;
                }
                else
                {
                    ret += temp;
                }

            }
            int i;
            if ((i = ret.IndexOf("����")) != -1)
            {
                ret = ret.Remove(i + 1, 1);
            }
            while ((i = ret.IndexOf("����")) != -1)
            {
                ret = ret.Remove(i, 1);
            }
            if (ret[ret.Length - 1] == '��' && ret.Length > 1)
            {
                ret = ret.Remove(ret.Length - 1, 1);
            }
            return ret;
        }
        //ת��С��
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
                ret = "��";
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
                ret += StrToInt(x.Substring(0, x.IndexOf("."))) + "��" + StrToDouble(x.Substring(x.IndexOf(".") + 1));
            }
            else
            {
                ret += StrToInt(x);
            }
            return ret;
        }

        //���ת��
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
                    //Ԫ
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
        /// �ж϶����Ƿ񱻳־û�
        /// </summary>
        /// <param name="proxy">����</param>
        /// <returns></returns>
        static public bool IsInitialized(object proxy)
        {
            return NHibernate.NHibernateUtil.IsInitialized(proxy);
        }
        /// <summary>
        /// ����type��÷���(����ӿ�ȥ��I,����AppContext����ķ����������)
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
        /// ��÷����������
        /// </summary>
        /// <param name="serviceName">�����������</param>
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


        //��ӡ����
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
            //        return "��ǰ���ݿ����ѱ��������޸ģ���ˢ�º���������";

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
        /// �ı����������
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
        /// �ж������ֵ�Ƿ����ָ��������
        /// </summary>
        /// <param name="textInputValue">����ֵ</param>
        /// <param name="textInputKind">����</param>
        /// <param name="customKindValue">�Զ���</param>
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
        /// �жϵ�Ԫ�������Ƿ�����㣬�Ƿ�����
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

        #region  ��ȡExcel�ļ�
        public class ExcelClass
        {


            /// <summary>
            /// ��ȡָ��Excel�ļ���Sheet
            /// </summary>
            /// <param name="fileName">Excel�ļ���</param>
            /// <returns>IList</returns>
            public static IList ReadExcelSheet(string fileName)
            {
                try
                {
                    IList list = new ArrayList();
                    object objMissing = System.Reflection.Missing.Value;
                    //��excel�ļ�   
                    Excel.ApplicationClass my = new Excel.ApplicationClass();
                    //�򿪹�����   
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
            /// ��ȡָ����Excel�ļ�
            /// </summary>
            /// <param name="fileName">Excel�ļ�����</param>
            /// <returns>���ص�һ��Sheet������DataSet.Talbes[0]</returns>
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
            /// ��ȡָ����Excel�ļ�
            /// </summary>
            /// <param name="fileName">Excel�ļ�����</param>
            /// <returns>����Sheet������DataSet.Talbes[sheetName]</returns>
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

                    //�򿪹�����   
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
            /// ��ȡָ����Excel�ļ�
            /// </summary>
            /// <param name="fileName">Excel�ļ�����</param>
            /// <returns>����Sheet������DataSet.Talbes[sheetName]</returns>
            public static DataSet ReadDataFromExcel(string fileName, string sheetName)
            {
                Excel.ApplicationClass my = new Excel.ApplicationClass();

                try
                {
                    DataSet dataSet = new DataSet();

                    object objMissing = System.Reflection.Missing.Value;

                    //�򿪹�����   
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
                    //��ñ�ͷ
                    for (int j = 1; j < lngMaxCol + 1; j++)
                    {
                        dataSet.Tables[0].Columns.Add((mySheet.Cells[1, j] as Excel.Range).Text.ToString());
                    }
                    //�����
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

            #region DataGridView����Excel
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
                //���а�ʵ��
                try
                {
                    Clipboard.Clear();
                    Clipboard.SetText(dataObject.GetText());
                    //Clipboard.SetDataObject(dataObject);                    
                    // ��ʼ�½�Excel
                    object m_objOpt = System.Reflection.Missing.Value;
                    Excel.Application m_objExcel = new Excel.Application();
                    Excel.Workbooks m_objBooks = (Excel.Workbooks)m_objExcel.Workbooks;
                    Excel._Workbook m_objBook = (Excel._Workbook)(m_objBooks.Add(m_objOpt));

                    // ճ������
                    Excel.Sheets m_objSheets = (Excel.Sheets)m_objBook.Worksheets;
                    Excel._Worksheet m_objSheet = (Excel._Worksheet)(m_objSheets.get_Item(1));
                    Excel.Range m_objRange = m_objSheet.get_Range("A1", m_objOpt);
                    m_objSheet.Paste(m_objRange, false);
                    //�����е������Զ������еĿ��
                    m_objSheet.Columns.AutoFit();

                    // �����˳�
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
            /// ����DataGridViewѡ������ݵ�Excel
            /// </summary>
            /// <param name="dataObject">ѡ��ĵ�Ԫ��</param>
            /// <param name="filePath">Excel�ļ�·��</param>
            /// <returns>True �ɹ�,False ʧ��</returns>
            public static bool DataGridViewToExcel(DataObject dataObject, string filePath, bool isOpen)
            {
                Exception exc;
                try
                {
                    //�ж��ļ��Ƿ����
                    if (System.IO.File.Exists(filePath))
                    {
                        //exc = new Exception(filePath + "\n" + "�ļ��Ѿ�����!");
                        //throw exc;
                        File.Delete(filePath);
                    }
                    if (!System.IO.Directory.GetParent(filePath).Exists)
                    {
                        exc = new Exception(System.IO.Directory.GetParent(filePath).FullName.ToString() + "\n" + "Ŀ¼�ļ��в�����!");
                        throw exc;
                    }
                    //����
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
        #region ��ȡ�������
        /// <summary>
        /// ���ָ�������������
        /// </summary>
        /// <param name="className">������</param>
        /// <returns>PropertyInfo��������</returns>
        public static IList ClassGetProperty(string className)
        {
            IList list = new ArrayList();
            try
            {
                Type typeObj = Type.GetType(className);
                if (typeObj == null)
                    throw new Exception("����[" + className + "]����ȷ");
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
                    throw new Exception("���Ͳ���Ϊ�գ�");
                PropertyInfo[] propInfo = type.GetProperties();
                return propInfo;
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        /// <summary>
        /// ���ָ�����͵��Ƿ���ĳ���͵�����
        /// </summary>
        /// <param name="type">���</param>
        /// <param name="typeInclude">�������</param>
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
        #region ��ָ�����ʹ�DataSet������ʵ��
        /// <summary>
        /// ��dataSet��������ݰ�propToExcelHeaderText�Ķ�Ӧ��ϵ,����type��ʵ����
        /// </summary>
        /// <param name="type">������ʵ��������</param>
        /// <param name="propToExcelHeaderText">����Excel��ͷ��Ӧ:"propName|ExcelHeaderText"</param>
        /// <param name="dataSet">��Excel���ȡ������DataSet</param>
        /// <returns>���ɵ�ʵ��Ilist</returns>
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
                throw new Exception("�����Ʋ����ڣ�");
            IList list = new ArrayList();
            string errMsg = "";
            long intRow = 0L;

            foreach (DataRow varRow in dt.Rows)
            {
                intRow++;
                Type type = null;
                if (varRow[0].ToString() == "�ⲿԱ��")
                    type = typeof(ExteriorPerson);
                else
                    type = typeof(Employee);
                Object obj = Activator.CreateInstance(type);
                PropertyInfo[] pis = obj.GetType().GetProperties();
                foreach (DataColumn varCol in dt.Columns)
                {
                    errMsg = "��[" + intRow.ToString() + "]�е�[" + varCol.Caption + "]���ݴ���";
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
                    throw new Exception("�����Ʋ����ڣ�");
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
                    errMsg = "��[" + intRow.ToString() + "]�е�[" + varCol.Caption + "]���ݴ���";
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

        #region ��־����
        private static IStockInSrv theStockInSrv;
        /// <summary>
        /// ������־
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
        /// ������־ ��������˳��Ϊ BillId;OperType;Code;OperPerson;BillType;Descript;ProjectName
        /// </summary>
        /// <param name="args">��������˳��Ϊ BillId;OperType;Code;OperPerson;BillType;Descript;ProjectName</param>
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
        /// ��ѯ��־
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

        #region ҵ�񵥾ݸ�������

        private static IGWBSTreeSrv theBaseSrv;

        /// <summary>
        /// ���뵥������
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
        /// ��ȡ����������Ϣ
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
        /// ��ȡ����������Ϣ
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
        /// ��ȡ����������Ϣ����
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

        #region ������Ŀ
        private static Hashtable projectInfoHash = new Hashtable();
        private static Hashtable subCompanyInfoHash = new Hashtable();
        /// <summary>
        /// ������Ŀ��Ϣ
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
                //��ȡ��ǰ��½�˶�Ӧ�ĺ�����֯

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
                System.Windows.MessageBox.Show("δ��ȡ��������Ŀ��Ϣ��");
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

        #region �ж�ϵͳʹ�õ��Ƿ���SQL Server���ݿ�
        /// <summary>
        /// �ж�ϵͳʹ�õ��Ƿ���SQL Server���ݿ� ���ڴ���sql��ѯ���
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

        #region ��ȡflxģ�巽��
        /// <summary>
        /// �ж�ģ���ļ��Ƿ����
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
        /// �ӷ�������ȡģ���ļ�
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

        #region ��ѯ�ֿ�
        private static List<StationCategory> stationCategoryList;
        /// <summary>
        /// ��ѯ�ֿ�
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
                //System.Windows.MessageBox.Show("δ��ȡ���ֿ���Ϣ��");
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

        #region �������ݲ���

        private static Hashtable htBasicData;

        /// <summary>
        /// ͨ���������ݱ����Ʋ�ѯ��������
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
        /// ͨ���������ݱ����ƺ���Ŀ���Ʋ�ѯ��������
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
        /// �������ڵõ����ڼ�
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
                    week = "����һ";
                    break;
                case DayOfWeek.Tuesday:
                    week = "���ڶ�";
                    break;
                case DayOfWeek.Wednesday:
                    week = "������";
                    break;
                case DayOfWeek.Thursday:
                    week = "������";
                    break;
                case DayOfWeek.Friday:
                    week = "������";
                    break;
                case DayOfWeek.Saturday:
                    week = "������";
                    break;
                case DayOfWeek.Sunday:
                    week = "������";
                    break;
            }
            return week;
        }

        #region �ĵ�/�������

        private const double KBCount = 1024;
        private const double MBCount = KBCount * 1024;
        private const double GBCount = MBCount * 1024;
        private const double TBCount = GBCount * 1024;

        /// <summary>
        /// �õ���Ӧ�ļ��Ĵ�С
        /// </summary>
        /// <param name="size">�ļ���С���ֽڣ�</param>
        /// <param name="roundCount">С��λ��</param>
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
        /// ��ȡMBP�ϴ��ļ���IRPʹ�õ��ĵ�����(1.�ĵ��������ͣ�2.�ĵ��ṹ���ͣ�3.�ĵ����ĵ������ϵ��������)
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
        /// ��ȡMBP�ϴ��ļ���֪ʶ��ʹ�õ��ĵ�����(1.�ĵ��������ͣ�2.�ĵ��ṹ���ͣ�3.�ĵ����ĵ������ϵ��������)
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
        /// ��Ŀ����Web����
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
        /// ֪ʶ��Web����
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
        /// ����ĵ�����
        /// </summary>
        /// <param name="listFileIds">�������ĵ�����Id�������������</param>
        /// <param name="listFileBytes">�������ļ��򱾵��ļ�·�����飨����fileType������֣�����Ƕ������ļ���listFileBytes�����п����һ������Byte[]��һ��Byte[]�����ʾһ���������ļ�������Ǳ����ļ���listFileBytes�����п����һ�����������ļ���·����</param>
        /// <param name="fileNames">�ļ����Ƽ������ļ�Ϊ�������ļ�ʱʹ�ã������ж��ļ���ʽ�Ƿ�Ϊ�����ϴ���ʽ</param>
        /// <param name="documentObjectType">�ĵ��Ķ�������</param>
        /// <param name="saveMode">���淽ʽ��1.һ���ļ�����һ���ĵ�����2.�����ļ�����һ���ĵ�����</param>
        /// <param name="dicKeyValue">�ĵ�����ȱʡҪ���õ�����ֵ�����飨dicKeyValue�п����һ������DictionaryObjectInfo[]��һ��DictionaryObjectInfo[]����洢һ���ļ�������ֵ�Լ���</param>
        /// <param name="sessionid">�ỰId����ѡ���������û��sessionid����ҪuserName��jobId������</param>
        /// <param name="userName">�û�������ѡ������</param>
        /// <param name="jobId">������λ����ѡ������</param>
        /// <param name="localVault">�ļ������ƣ���ѡ������</param>
        /// <returns>�쳣����</returns>
        public static ErrorStack AddDocumentByCustom(out string[] listFileIds, object[] listFileBytes, string[] fileNames, string documentObjectType, string saveMode,
        DictionaryObjectInfo[] dicKeyValue, string sessionid, string userName, string jobId, string localVault)
        {
            return PLMWS.AddDocumentByCustom(out listFileIds, listFileBytes, fileNames, documentObjectType, saveMode, dicKeyValue, sessionid, userName, jobId, localVault);
        }

        /// <summary>
        /// �޸��ĵ�����
        /// </summary>
        /// <param name="listFileIds">�ĵ�����Id��</param>
        /// <param name="documentObjectType">�ĵ��Ķ�������</param>
        /// <param name="listFileBytes">�������ļ��򱾵��ļ�·�����飨����fileType������֣�����Ƕ������ļ���listFileBytes�����п����һ������Byte[]��һ��Byte[]�����ʾһ���������ļ�������Ǳ����ļ���listFileBytes�����п����һ�����������ļ���·����</param>
        /// <param name="fileNames">�ļ����Ƽ������ļ�Ϊ�������ļ�ʱʹ�ã������ж��ļ���ʽ�Ƿ�Ϊ�����ϴ���ʽ</param>
        /// <param name="updateMode">���·�ʽ��1.������ļ���2.����ԭ���ļ���</param>
        /// <param name="dicKeyValue">�ĵ�����ȱʡҪ���õ�����ֵ�����飨dicKeyValue�п����һ������DictionaryObjectInfo[]��һ��DictionaryObjectInfo[]����洢һ���ļ�������ֵ�Լ���</param>
        /// <param name="sessionid">�ỰId����ѡ���������û��sessionid����ҪuserName��jobId������</param>
        /// <param name="userName">�û�������ѡ������</param>
        /// <param name="jobId">������λ����ѡ������</param>
        /// <param name="localVault">�ļ������ƣ���ѡ������</param>
        /// <returns>�쳣����</returns>
        public static ErrorStack UpdateDocumentByCustom(string[] listFileIds, string documentObjectType, object[] listFileBytes, string[] fileNames, string updateMode,
            DictionaryObjectInfo[] dicKeyValue, string sessionid, string userName, string jobId, string localVault)
        {
            return PLMWS.UpdateDocumentByCustom(listFileIds, documentObjectType, listFileBytes, fileNames, updateMode, dicKeyValue, sessionid, userName, jobId, localVault);
        }

        /// <summary>
        /// �ļ�����
        /// </summary>
        /// <param name="listFileBytes">�����ļ��Ķ����������飨���������</param>
        /// <param name="listFileNames">�����ļ������ƣ����������</param>
        /// <param name="listFileIds">Ҫ���ص��ĵ�����Id��</param>
        /// <param name="sessionid">�ỰId����ѡ���������û��sessionid����ҪuserName��jobId������</param>
        /// <param name="userName">�û�������ѡ������</param>
        /// <param name="jobId">������λ����ѡ������</param>
        /// <param name="localVault">�ļ������ƣ���ѡ������</param>
        /// <returns>�쳣����</returns>
        public static ErrorStack DownLoadDocumentByCustom(out object[] listFileBytes, out string[] listFileNames, string[] listFileIds, string sessionid, string userName, string jobId, string localVault)
        {
            return PLMWS.DownLoadDocumentByCustom(out listFileBytes, out listFileNames, listFileIds, sessionid, userName, jobId, localVault);
        }

        /// <summary>
        /// ɾ���ĵ�
        /// </summary>
        /// <param name="listFileIds">Ҫɾ�����ĵ�����Id��</param>
        /// <param name="deleteMode">ɾ����ʽ��1.ɾ����ǰ�汾��2.ɾ�����а汾��</param>
        /// <param name="sessionid">�ỰId����ѡ���������û��sessionid����ҪuserName��jobId������</param>
        /// <param name="userName">�û�������ѡ������</param>
        /// <param name="jobId">������λ����ѡ������</param>
        /// <param name="localVault">�ļ������ƣ���ѡ������</param>
        /// <returns>�쳣����</returns>
        public static ErrorStack DeleteDocumentByCustom(string[] listFileIds, string deleteMode, string sessionid, string userName, string jobId, string localVault)
        {
            return PLMWS.DeleteDocumentByCustom(listFileIds, deleteMode, sessionid, userName, jobId, localVault);
        }




        /// <summary>
        /// �ϴ�/�����ĵ�(��Ŀ����IRP)
        /// </summary>
        /// <param name="listDocument">��Ŀ�ĵ��������飬һ�������ʾһ��ʵ��</param>
        /// <param name="saveMode">���淽ʽ��1.һ���ļ�����һ���ĵ�����,2.�����ļ�����һ���ĵ�����</param>
        /// <param name="sessionid">�ỰId����ѡ���������û��sessionid����ҪuserName��jobId������</param>
        /// <param name="userName">�û�������ѡ������</param>
        /// <param name="jobId">������λ����ѡ������</param>
        /// <param name="localVault">�ļ������ƣ���ѡ������</param>
        /// <param name="listResult">�������ĵ��������飨���������</param>
        /// <returns>�쳣����</returns>
        public static ErrorStack AddDocumentByIRP(ProjectDocument[] listDocument, DocumentSaveMode saveMode,
         string sessionid, string userName, string jobId, string localVault, out ProjectDocument[] listResult)
        {
            return PLMWS.AddDocumentByCustomExtend(out listResult, listDocument, saveMode, sessionid, userName, jobId, localVault);
        }
        /// <summary>
        /// �ϴ�/�����ĵ�(֪ʶ��)
        /// </summary>
        /// <param name="listDocument">��Ŀ�ĵ��������飬һ�������ʾһ��ʵ��</param>
        /// <param name="saveMode">���淽ʽ��1.һ���ļ�����һ���ĵ�����,2.�����ļ�����һ���ĵ�����</param>
        /// <param name="sessionid">�ỰId����ѡ���������û��sessionid����ҪuserName��jobId������</param>
        /// <param name="userName">�û�������ѡ������</param>
        /// <param name="jobId">������λ����ѡ������</param>
        /// <param name="localVault">�ļ������ƣ���ѡ������</param>
        /// <param name="listResult">�������ĵ��������飨���������</param>
        /// <returns>�쳣����</returns>
        public static PLMWebServicesByKB.ErrorStack AddDocumentByKB(PLMWebServicesByKB.ProjectDocument[] listDocument, PLMWebServicesByKB.DocumentSaveMode saveMode,
         string sessionid, string userName, string jobId, string localVault, out PLMWebServicesByKB.ProjectDocument[] listResult)
        {
            return PLMWSByKB.AddDocumentByCustomExtend(out listResult, listDocument, saveMode, sessionid, userName, jobId, localVault);
        }

        /// <summary>
        /// �ļ�����(��Ŀ����IRP)
        /// </summary>
        /// <param name="listDocument">Ҫ���ص��ĵ�ֵ����</param>
        /// <param name="sessionid">�ỰId����ѡ���������û��sessionid����ҪuserName��jobId������</param>
        /// <param name="userName">�û�������ѡ������</param>
        /// <param name="jobId">������λ����ѡ������</param>
        /// <param name="localVault">�ļ������ƣ���ѡ������</param>
        /// <param name="listResult">�����ص��ĵ��������飨���������</param>
        /// <returns>�쳣����</returns>
        public static ErrorStack DownLoadDocumentByIRP(ProjectDocument[] listDocument, string sessionid, string userName, string jobId, string localVault, out ProjectDocument[] listResult)
        {
            return PLMWS.DownLoadDocumentByCustomExtend(out listResult, listDocument, sessionid, userName, jobId, localVault);
        }

        /// <summary>
        /// �ļ�����(֪ʶ��)
        /// </summary>
        /// <param name="listDocument">Ҫ���ص��ĵ�ֵ����</param>
        /// <param name="sessionid">�ỰId����ѡ���������û��sessionid����ҪuserName��jobId������</param>
        /// <param name="userName">�û�������ѡ������</param>
        /// <param name="jobId">������λ����ѡ������</param>
        /// <param name="localVault">�ļ������ƣ���ѡ������</param>
        /// <param name="listResult">�����ص��ĵ��������飨���������</param>
        /// <returns>�쳣����</returns>
        public static PLMWebServicesByKB.ErrorStack DownLoadDocumentByKB(PLMWebServicesByKB.ProjectDocument[] listDocument, string sessionid, string userName, string jobId, string localVault, out PLMWebServicesByKB.ProjectDocument[] listResult)
        {
            return PLMWSByKB.DownLoadDocumentByCustomExtend(out listResult, listDocument, sessionid, userName, jobId, localVault);
        }


        /// <summary>
        /// �ļ���ѯ����Ŀ����IRP��
        /// </summary>
        /// <param name="projectCodeQuery">������Ŀ����</param>
        /// <param name="projectNameQuery">������Ŀ����</param>
        /// <param name="docCodeQuery">�ĵ�����</param>
        /// <param name="docNameQuery">�ĵ�����</param>
        /// <param name="docExtendNameQuery">�ĵ���չ��</param>
        /// <param name="docInfoTypeQuery">�ĵ���Ϣ����</param>
        /// <param name="docVersionQuery">�ĵ��汾</param>
        /// <param name="docRevisionQuery">�ĵ����</param>
        /// <param name="docCreateBeginTimeQuery">�ĵ�������ʼʱ��</param>
        /// <param name="docCreateEndTimeQuery">�ĵ���������ʱ��</param>
        /// <param name="docOwnerQuery">�ĵ�������(�û���)</param>
        /// <param name="docStateQuery">�ĵ�״̬</param>
        /// <param name="docKeyWordQuery">�����ؼ��֣����ĵ����⡢���ߡ��ĵ�˵�����ĵ��ؼ�����������</param>
        /// <param name="sessionid">�ỰId����ѡ���������û��sessionid����ҪuserName��jobId������</param>
        /// <param name="userName">�û�������ѡ������</param>
        /// <param name="jobId">������λ����ѡ������</param>
        /// <param name="localVault">�ļ������ƣ���ѡ������</param>
        /// <param name="listResult">���ؽ���������������</param>
        /// <returns>�쳣����</returns>
        public static ErrorStack GetProjectDocumentByIRP(string projectCodeQuery, string projectNameQuery, string docCodeQuery, string docNameQuery, string docExtendNameQuery,
            DocumentInfoType? docInfoTypeQuery, DocumentQueryVersion queryMode, string docVersionQuery, string docRevisionQuery, DateTime? docCreateBeginTimeQuery, DateTime? docCreateEndTimeQuery, string docOwnerQuery,
            DocumentState? docStateQuery, string docKeyWordQuery, string sessionid, string userName, string jobId, string localVault, out ProjectDocument[] listResult)
        {

            return PLMWS.GetProjectDocumentByCustomExtend(projectCodeQuery, projectNameQuery, docCodeQuery, docNameQuery, docExtendNameQuery, docInfoTypeQuery, queryMode, docVersionQuery,
docRevisionQuery, docCreateBeginTimeQuery, docCreateEndTimeQuery, docOwnerQuery, docStateQuery, docKeyWordQuery, sessionid, userName, jobId, localVault, out listResult);

        }

        /// <summary>
        /// �ļ���ѯ(֪ʶ��)
        /// </summary>
        /// <param name="projectCodeQuery">������Ŀ����</param>
        /// <param name="projectNameQuery">������Ŀ����</param>
        /// <param name="docCodeQuery">�ĵ�����</param>
        /// <param name="docNameQuery">�ĵ�����</param>
        /// <param name="docExtendNameQuery">�ĵ���չ��</param>
        /// <param name="docInfoTypeQuery">�ĵ���Ϣ����</param>
        /// <param name="docVersionQuery">�ĵ��汾</param>
        /// <param name="docRevisionQuery">�ĵ����</param>
        /// <param name="docCreateBeginTimeQuery">�ĵ�������ʼʱ��</param>
        /// <param name="docCreateEndTimeQuery">�ĵ���������ʱ��</param>
        /// <param name="docOwnerQuery">�ĵ�������(�û���)</param>
        /// <param name="docStateQuery">�ĵ�״̬</param>
        /// <param name="docKeyWordQuery">�����ؼ��֣����ĵ����⡢���ߡ��ĵ�˵�����ĵ��ؼ�����������</param>
        /// <param name="sessionid">�ỰId����ѡ���������û��sessionid����ҪuserName��jobId������</param>
        /// <param name="userName">�û�������ѡ������</param>
        /// <param name="jobId">������λ����ѡ������</param>
        /// <param name="localVault">�ļ������ƣ���ѡ������</param>
        /// <param name="listResult">���ؽ���������������</param>
        /// <returns>�쳣����</returns>
        public static PLMWebServicesByKB.ErrorStack GetProjectDocumentByKB(string projectCodeQuery, string projectNameQuery, string docCodeQuery, string docNameQuery, string docExtendNameQuery,
            PLMWebServicesByKB.DocumentInfoType? docInfoTypeQuery, PLMWebServicesByKB.DocumentQueryVersion queryMode, string docVersionQuery, string docRevisionQuery, DateTime? docCreateBeginTimeQuery, DateTime? docCreateEndTimeQuery, string docOwnerQuery,
            PLMWebServicesByKB.DocumentState? docStateQuery, string docKeyWordQuery, string sessionid, string userName, string jobId, string localVault, out PLMWebServicesByKB.ProjectDocument[] listResult)
        {

            return PLMWSByKB.GetProjectDocumentByCustomExtend(projectCodeQuery, projectNameQuery, docCodeQuery, docNameQuery, docExtendNameQuery, docInfoTypeQuery, queryMode, docVersionQuery,
docRevisionQuery, docCreateBeginTimeQuery, docCreateEndTimeQuery, docOwnerQuery, docStateQuery, docKeyWordQuery, sessionid, userName, jobId, localVault, out listResult);

        }


        /// <summary>
        /// �ļ���ѯ����Ŀ����IRP��
        /// </summary>
        /// <param name="projectCodeQuery">������Ŀ����</param>
        /// <param name="projectNameQuery">������Ŀ����</param>
        /// <param name="docCodeQuery">�ĵ�����</param>
        /// <param name="docNameQuery">�ĵ�����</param>
        /// <param name="docExtendNameQuery">�ĵ���չ��</param>
        /// <param name="docInfoTypeQuery">�ĵ���Ϣ����</param>
        /// <param name="docVersionQuery">�ĵ��汾</param>
        /// <param name="docRevisionQuery">�ĵ����</param>
        /// <param name="docCreateBeginTimeQuery">�ĵ�������ʼʱ��</param>
        /// <param name="docCreateEndTimeQuery">�ĵ���������ʱ��</param>
        /// <param name="docOwnerQuery">�ĵ�������(�û���)</param>
        /// <param name="docStateQuery">�ĵ�״̬</param>
        /// <param name="docKeyWordQuery">�����ؼ��֣����ĵ����⡢���ߡ��ĵ�˵�����ĵ��ؼ�����������</param>
        /// <param name="sessionid">�ỰId����ѡ���������û��sessionid����ҪuserName��jobId������</param>
        /// <param name="userName">�û�������ѡ������</param>
        /// <param name="jobId">������λ����ѡ������</param>
        /// <param name="localVault">�ļ������ƣ���ѡ������</param>
        /// <param name="listResult">���ؽ���������������</param>
        /// <returns>�쳣����</returns>
        public static ErrorStack GetProjectDocumentByIRP(string[] fileIds, DocumentQueryVersion queryMode, string sessionid, string userName, string jobId, string localVault, out ProjectDocument[] listResult)
        {

            return PLMWS.GetProjectDocumentByDocumentId(out listResult, fileIds, queryMode, sessionid, userName, jobId, localVault);

        }

        /// <summary>
        /// �ļ���ѯ��֪ʶ�⣩
        /// </summary>
        /// <param name="projectCodeQuery">������Ŀ����</param>
        /// <param name="projectNameQuery">������Ŀ����</param>
        /// <param name="docCodeQuery">�ĵ�����</param>
        /// <param name="docNameQuery">�ĵ�����</param>
        /// <param name="docExtendNameQuery">�ĵ���չ��</param>
        /// <param name="docInfoTypeQuery">�ĵ���Ϣ����</param>
        /// <param name="docVersionQuery">�ĵ��汾</param>
        /// <param name="docRevisionQuery">�ĵ����</param>
        /// <param name="docCreateBeginTimeQuery">�ĵ�������ʼʱ��</param>
        /// <param name="docCreateEndTimeQuery">�ĵ���������ʱ��</param>
        /// <param name="docOwnerQuery">�ĵ�������(�û���)</param>
        /// <param name="docStateQuery">�ĵ�״̬</param>
        /// <param name="docKeyWordQuery">�����ؼ��֣����ĵ����⡢���ߡ��ĵ�˵�����ĵ��ؼ�����������</param>
        /// <param name="sessionid">�ỰId����ѡ���������û��sessionid����ҪuserName��jobId������</param>
        /// <param name="userName">�û�������ѡ������</param>
        /// <param name="jobId">������λ����ѡ������</param>
        /// <param name="localVault">�ļ������ƣ���ѡ������</param>
        /// <param name="listResult">���ؽ���������������</param>
        /// <returns>�쳣����</returns>
        public static PLMWebServicesByKB.ErrorStack GetProjectDocumentByKB(string[] fileIds, PLMWebServicesByKB.DocumentQueryVersion queryMode, string sessionid, string userName, string jobId, string localVault, out PLMWebServicesByKB.ProjectDocument[] listResult)
        {
            return PLMWSByKB.GetProjectDocumentByDocumentId(out listResult, fileIds, queryMode, sessionid, userName, jobId, localVault);
        }


        /// <summary>
        /// �޸��ĵ�����(��Ŀ����IRP)
        /// </summary>
        /// <param name="listDocument">Ҫ�޸ĵ��ĵ�����</param>
        /// <param name="updateMode">���·�ʽ��1.���һ���°���ļ���2.����ԭ�����°���ļ���</param>
        /// <param name="sessionid">�ỰId����ѡ���������û��sessionid����ҪuserName��jobId������</param>
        /// <param name="userName">�û�������ѡ������</param>
        /// <param name="jobId">������λ����ѡ������</param>
        /// <param name="localVault">�ļ������ƣ���ѡ������</param>
        /// <param name="listResult">�޸ĺ���ĵ����󼯣����������</param>
        /// <returns>�쳣����</returns>
        public static ErrorStack UpdateDocumentByIRP(ProjectDocument[] listDocument, DocumentUpdateMode updateMode, string sessionid, string userName, string jobId, string localVault, out ProjectDocument[] listResult)
        {
            return PLMWS.UpdateDocumentByCustomExtend(out listResult, listDocument, updateMode, sessionid, userName, jobId, localVault);
        }

        /// <summary>
        /// �޸��ĵ�����(֪ʶ��)
        /// </summary>
        /// <param name="listDocument">Ҫ�޸ĵ��ĵ�����</param>
        /// <param name="updateMode">���·�ʽ��1.���һ���°���ļ���2.����ԭ�����°���ļ���</param>
        /// <param name="sessionid">�ỰId����ѡ���������û��sessionid����ҪuserName��jobId������</param>
        /// <param name="userName">�û�������ѡ������</param>
        /// <param name="jobId">������λ����ѡ������</param>
        /// <param name="localVault">�ļ������ƣ���ѡ������</param>
        /// <param name="listResult">�޸ĺ���ĵ����󼯣����������</param>
        /// <returns>�쳣����</returns>
        public static PLMWebServicesByKB.ErrorStack UpdateDocumentByKB(PLMWebServicesByKB.ProjectDocument[] listDocument, PLMWebServicesByKB.DocumentUpdateMode updateMode, string sessionid, string userName, string jobId, string localVault, out PLMWebServicesByKB.ProjectDocument[] listResult)
        {
            return PLMWSByKB.UpdateDocumentByCustomExtend(out listResult, listDocument, updateMode, sessionid, userName, jobId, localVault);
        }

        /// <summary>
        /// ɾ���ĵ�����(��Ŀ����IRP)
        /// </summary>
        /// <param name="listDocument">Ҫɾ�����ĵ�����</param>
        /// <param name="deleteMode">ɾ����ʽ��1.ɾ�����°汾��2.ɾ�����а汾��</param>
        /// <param name="sessionid">�ỰId����ѡ���������û��sessionid����ҪuserName��jobId������</param>
        /// <param name="userName">�û�������ѡ������</param>
        /// <param name="jobId">������λ����ѡ������</param>
        /// <param name="localVault">�ļ������ƣ���ѡ������</param>
        /// <returns>�쳣����</returns>
        public static ErrorStack DeleteDocumentByIRP(ProjectDocument[] listDocument, DocumentDeleteMode deleteMode, string sessionid, string userName, string jobId, string localVault)
        {
            return PLMWS.DeleteDocumentByCustomExtend(listDocument, deleteMode, sessionid, userName, jobId, localVault);
        }

        /// <summary>
        /// ɾ���ĵ�����(֪ʶ��)
        /// </summary>
        /// <param name="listDocument">Ҫɾ�����ĵ�����</param>
        /// <param name="deleteMode">ɾ����ʽ��1.ɾ�����°汾��2.ɾ�����а汾��</param>
        /// <param name="sessionid">�ỰId����ѡ���������û��sessionid����ҪuserName��jobId������</param>
        /// <param name="userName">�û�������ѡ������</param>
        /// <param name="jobId">������λ����ѡ������</param>
        /// <param name="localVault">�ļ������ƣ���ѡ������</param>
        /// <returns>�쳣����</returns>
        public static PLMWebServicesByKB.ErrorStack DeleteDocumentByKB(PLMWebServicesByKB.ProjectDocument[] listDocument, PLMWebServicesByKB.DocumentDeleteMode deleteMode, string sessionid, string userName, string jobId, string localVault)
        {
            return PLMWSByKB.DeleteDocumentByCustomExtend(listDocument, deleteMode, sessionid, userName, jobId, localVault);
        }

        /// <summary>
        /// ��Ŀ�ĵ���������
        /// </summary>
        public enum ProjectDocumentCategoryTypeEnum
        {
            /// <summary>
            /// ֪ʶ��
            /// </summary>
            CLASSDOCUMENT = 1,
            /// <summary>
            ///��Ŀ�ĵ��� 
            /// </summary>
            CLASSIRPDOCUMENT = 2
        }

        /// <summary>
        /// ��Ŀ����
        /// </summary>
        public enum ProjectInfoTypeEnum
        {
            /// <summary>
            /// ֪ʶ��
            /// </summary>
            KB = 1
        }

        /// <summary>
        /// ֪ʶ������ѯ
        /// </summary>
        /// <param name="CategoryTypeName">��������������ƣ�֪ʶ����ķ���������ƣ�</param>
        /// <param name="queryMode">��ѯ��ʽ��1.����ʽ,2��ʽ��</param>
        /// <param name="sessionid">�ỰId����ѡ���������û��sessionid����ҪuserName��jobId������</param>
        /// <param name="userName">�û�������ѡ������</param>
        /// <param name="jobId">������λ����ѡ������</param>
        /// <param name="localVault">�ļ������ƣ���ѡ������</param>
        /// <param name="listResult">��ѯ�������������������</param>
        /// <returns>�쳣����</returns>
        public static PLMWebServicesByKB.ErrorStack GetDocumentCategoryByKB(ProjectDocumentCategoryTypeEnum projectCateType, PLMWebServicesByKB.CategoryQueryModeEnum queryMode, string queryKeyWords, string sessionid, string userName,
            string jobId, string localVault, out PLMWebServicesByKB.CategoryNode[] listResult)
        {
            return PLMWSByKB.GetCategoryByCustomExtend(out listResult, projectCateType.ToString(), queryMode, queryKeyWords, sessionid, userName, jobId, localVault);
        }

        /// <summary>
        /// ��Ŀ����IRP�������ѯ
        /// </summary>
        /// <param name="CategoryTypeName">��������������ƣ���Ŀ������ķ���������ƣ�</param>
        /// <param name="queryMode">��ѯ��ʽ��1.����ʽ,2��ʽ��</param>
        /// <param name="sessionid">�ỰId����ѡ���������û��sessionid����ҪuserName��jobId������</param>
        /// <param name="userName">�û�������ѡ������</param>
        /// <param name="jobId">������λ����ѡ������</param>
        /// <param name="localVault">�ļ������ƣ���ѡ������</param>
        /// <param name="listResult">��ѯ�������������������</param>
        /// <returns>�쳣����</returns>
        public static ErrorStack GetDocumentCategoryByIRP(ProjectDocumentCategoryTypeEnum projectCateType, CategoryQueryModeEnum queryMode, string queryKeyWords, string sessionid, string userName,
            string jobId, string localVault, out PLMWebServices.CategoryNode[] listResult)
        {
            return PLMWS.GetCategoryByCustomExtend(out listResult, projectCateType.ToString(), queryMode, queryKeyWords, sessionid, userName, jobId, localVault);
        }

        #endregion

        #region ���ṹ
        private static IGWBSTreeSrv treeCateSrv = null;
        /// <summary>
        /// ��������������
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
        /// ��ȡ������������·��
        /// </summary>
        /// <param name="nodeObj"></param>
        /// <returns></returns>
        public static string GetCategorTreeFullPath(Type cateEntityType, VirtualMachine.Patterns.CategoryTreePattern.Domain.CategoryNode nodeObj)
        {
            return TreeCateSrv.GetCategorTreeFullPath(cateEntityType, nodeObj);
        }

        /// <summary>
        /// ��ȡ������������·��
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
        ///// ��ȡ������������·��
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
        /// ȥ��decimalβ�����õ�0
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
        /// ʱ���ʽ���ַ���������1900-1-1����ʾ�����
        /// </summary>
        /// <param name="date"></param>
        /// <param name="longDate"></param>
        /// <returns></returns>
        public static string GetShowDateTimeStr(DateTime date, bool longDate)
        {
            return date == DateTime.Parse("1900-1-1") ? "" : longDate ? date.ToLongDateString() : date.ToShortDateString();
        }

        /// <summary>
        /// ��ȡ���״̬��ʾ���ı���WBS�ճ����ȣ�0��δ��飻1�����δͨ����2�����ͨ����X�������顣��
        /// </summary>
        /// <param name="checkStateStr"></param>
        /// <returns></returns>
        public static string GetCheckStateShowText(string checkStateStr)
        {
            if (string.IsNullOrEmpty(checkStateStr))
                return "";

            string checkRequireShow = string.Empty;


            //string[] checkRequireName = new string[] { "�����ʼ�", "�ʼ�Ա�ʼ�", "�����ʼ�", "���̽���", "��ȫרҵ", "����רҵ", "����רҵ" };

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
                        checkRequireShow += (checkRequire[index].BasicName + "δ���" + "/");
                        break;
                    case '1':
                        checkRequireShow += (checkRequire[index].BasicName + "���δͨ��" + "/");
                        break;
                    case '2':
                        checkRequireShow += (checkRequire[index].BasicName + "���ͨ��" + "/");
                        break;
                    case 'X':
                        checkRequireShow += (checkRequire[index].BasicName + "������" + "/");
                        count++;
                        break;
                }
                index++;

            }

            if (count == length)
                checkRequireShow = "������";
            else if (checkRequireShow.Length > 0)
                checkRequireShow = checkRequireShow.Substring(0, checkRequireShow.Length - 1);

            return checkRequireShow;
        }

        /// <summary>
        /// ��ȡ���Ҫ����ʾ���ı���WBS���Ҫ��0��δ��飻1�����ͨ����2���������ͨ����3�����δͨ����4������У�X�������顣��
        /// </summary>
        /// <param name="checkStateStr"></param>
        /// <returns></returns>
        public static string GetCheckRequireShowText(string checkStateStr)
        {
            if (string.IsNullOrEmpty(checkStateStr))
                return "";

            string checkRequireShow = string.Empty;

            string[] checkRequireName = new string[] { "�����ʼ�", "�ʼ�Ա�ʼ�", "�����ʼ�", "���̽���", "��ȫרҵ", "����רҵ", "����רҵ", "", "", "", "", "������ȷ��" };

            int index = 0;
            foreach (char c in checkStateStr)
            {
                if (index < checkRequireName.Length && !string.IsNullOrEmpty(checkRequireName[index]))
                {
                    if (index == 11)//������ȷ�ϱ�־
                    {
                        if (c == '0')
                        {
                            checkRequireShow += (checkRequireName[index] + "δȷ��");
                        }
                        if (c == '1')
                        {
                            checkRequireShow += (checkRequireName[index] + "ȷ��");
                        }
                    }
                    else
                    {
                        switch (c)
                        {
                            case '0':
                                checkRequireShow += (checkRequireName[index] + "δ���" + "/");
                                break;
                            case '1':
                                checkRequireShow += (checkRequireName[index] + "���ͨ��" + "/");
                                break;
                            case '2':
                                checkRequireShow += (checkRequireName[index] + "�������ͨ��" + "/");
                                break;
                            case '3':
                                checkRequireShow += (checkRequireName[index] + "���δͨ��" + "/");
                                break;
                            case '4':
                                checkRequireShow += (checkRequireName[index] + "�����" + "/");
                                break;
                            case 'X':
                                checkRequireShow += (checkRequireName[index] + "������" + "/");
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
        /// ��ȡ�ճ����ͨ��״̬�ı�("ͨ��"��"δͨ��")
        /// </summary>
        /// <param name="checkState"></param>
        /// <returns></returns>
        public static string GetCheckStatePassStr(string checkState)
        {
            if (string.IsNullOrEmpty(checkState))
                return "ͨ��";
            else if (checkState.Length == 12)//��12λΪ������ȷ�ϱ�־��ǰ��Ϊ����־
                checkState = checkState.Substring(0, 11);

            return (checkState.IndexOf("0") == -1 && checkState.IndexOf("1") == -1) ? "ͨ��" : "δͨ��";
        }

        /// <summary>
        /// ��ȡ�ڵ�����ö�ٵ�����˵��
        /// </summary>
        /// <param name="nodeType"></param>
        /// <returns></returns>
        public static string GetNodeTypeStr(VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType nodeType)
        {
            string result = "";
            switch (nodeType)
            {
                case NodeType.LeafNode:
                    result = "Ҷ�ڵ�";
                    break;
                case NodeType.MiddleNode:
                    result = "�м�ڵ�";
                    break;
                case NodeType.RootNode:
                    result = "���ڵ�";
                    break;
                default:
                    break;
            }
            return result;
        }

        /// <summary>
        /// ��ȡָ����Ŀ������������ϸ�ĺ�ͬ�ϼۡ����κϼۡ��ƻ��ϼ�����
        /// </summary>
        /// <param name="targetTask"></param>
        /// <returns>[��ͬ�ϼۣ����κϼۣ��ƻ��ϼ�]</returns>
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

            if (query.Count() == 0)//����������ɱ�������ϸ��ȡ��������������ϸ�ļƻ���������ϸ�ĺϼ�
            {
                foreach (GWBSDetail detail in listDtl)
                {
                    contractTotalPrice += detail.ContractTotalPrice;
                    responsibilitilyTotalPrice += detail.ResponsibilitilyTotalPrice;
                    planTotalPrice += detail.PlanTotalPrice;
                }
            }
            else//��������ɱ�������ϸ����ȡ�ɱ�������ϸ�ļƻ���������ϸ�ĺϼ�
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
        /// ��ȡָ����Ŀ������������ϸ�ĺ�ͬ�ϼۡ����κϼۡ��ƻ��ϼ�����
        /// </summary>
        /// <param name="targetTask"></param>
        /// <returns>[��ͬ�ϼۣ����κϼۣ��ƻ��ϼ�]</returns>
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

            if (query.Count() == 0)//����������ɱ�������ϸ��ȡ��������������ϸ�ļƻ���������ϸ�ĺϼ�
            {
                foreach (GWBSDetail detail in listDtl)
                {
                    contractTotalPrice += detail.ContractTotalPrice;
                    responsibilitilyTotalPrice += detail.ResponsibilitilyTotalPrice;
                    planTotalPrice += detail.PlanTotalPrice;
                }
            }
            else//��������ɱ�������ϸ����ȡ�ɱ�������ϸ�ļƻ���������ϸ�ĺϼ�
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
        /// ��ȡ��Ŀ������ʾ��״̬
        /// </summary>
        /// <param name="taskState"></param>
        /// <returns></returns>
        public static string GetWBSTaskStateText(VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState taskState)
        {
            if (taskState == VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute)
                return "ִ����";
            else
                return ClientUtil.GetDocStateName(taskState);
        }
        /// <summary>
        /// ��ȡ������㵥��״̬
        /// </summary>
        /// <param name="taskState"></param>
        /// <returns></returns>
        public static string GetProjectTaskAccountBillStateText(VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState taskState)
        {
            if (taskState == VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Valid)
                return "�ύ";
            else
                return ClientUtil.GetDocStateName(taskState);
        }
        /// <summary>
        /// �õ�Ĭ���ļ���
        /// </summary>
        /// <returns></returns>
        public static FileCabinet GetDefaultFileCabinet()
        {
            FileCabinet appFileCabinet = null;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("UsedState", UseState.����));
            IList listFileCabinet = TreeCateSrv.ObjectQuery(typeof(FileCabinet), oq);
            if (listFileCabinet.Count > 0)
            {
                appFileCabinet = listFileCabinet[0] as FileCabinet;
            }
            return appFileCabinet;
        }

        private static bool _isEnabledDataAuth = false;
        /// <summary>
        /// �Ƿ���������Ȩ��
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
        /// �õ���Ƭ�ڿͻ��˵�·������ʾ��
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

                //string address = "http://10.70.18.203//TestFile//Files//0001//0001//0001.0001.000027V_��Ч.jpg";
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
