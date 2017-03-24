using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.Basic;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Resource.CommonClass.Domain;
using VirtualMachine.Component.Util;
using VirtualMachine.Patterns.DataDictionary.Domain;
using VirtualMachine.SystemAspect.Security;
using ResourceManagerClient;
using VirtualMachine.Component.ExceptionHandle;
using Application.Business.Erp.Secure.Client.Basic.CommonClass;
using VirtualMachine.Component.ContextConfigure;
using encrypt.Notify;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using System.Collections;
using Application.Resource.FinancialResource.RelateClass;
using System.Configuration;
using System.Net.NetworkInformation;
using Application.Business.Erp.SupplyChain.Util;

namespace SupplyChainClient
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// 
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            string specialScale = "";
            string[] ArrPara = null;
            try
            {
                System.Windows.Forms.Application.ThreadException += new ThreadExceptionEventHandler(MyHandler);
                System.Windows.Forms.Application.EnableVisualStyles();
                System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

                #region ע������
                //string ipAddress = System.Configuration.ConfigurationManager.AppSettings["ProIp"];//��˾VPN����IP��ַ
                //string VPNName = "CSCEC3B-VPN"; //VPN�����������ƣ���ʾ�ڿͻ��˵�����
                //string IPToPing = "www.cscec3b.com"; //VPN������������ַ
                //string UserName = "vpnuser"; //��¼�˺�
                //string PassWord = "123456"; //��¼����
                //if (!System.Diagnostics.Debugger.IsAttached && 1 == 2)
                //{
                //    VPNHelper myVPNHelper2 = new VPNHelper();
                //    Ping ping = new Ping();//����Ping����p
                //    PingReply pr = ping.Send(ipAddress);//��ָ��IP�����������ļ��������ICMPЭ���ping���ݰ� 

                //    if (pr.Status == IPStatus.TimedOut)//���ping�ɹ� 
                //    {
                //        pr = ping.Send(IPToPing);
                //        if (pr.Status == IPStatus.TimedOut)
                //        {
                //            MessageBox.Show("�޷����ӻ�����������������");
                //            return;
                //        }
                //        myVPNHelper2.VPNName = VPNName;
                //        myVPNHelper2.IPToPing = IPToPing;
                //        myVPNHelper2.UserName = UserName;
                //        myVPNHelper2.PassWord = PassWord;

                //        //�Ѿ����죬���������������ȶϿ����ӡ�
                //        for (int t = 0; t < 4; t++)
                //        {
                //            pr = ping.Send(ipAddress);
                //            if (pr.Status == IPStatus.TimedOut)
                //            {
                //                myVPNHelper2.CreateOrUpdateVPN();
                //                myVPNHelper2.TryConnectVPN();
                //                Thread.Sleep(1000);
                //            }
                //            else if (pr.Status == IPStatus.Success)
                //            {
                //                break;
                //            }
                //        }

                //        //pr = ping.Send(ipAddress);//��ָ��IP�����������ļ��������ICMPЭ���ping���ݰ� 
                //        if (pr.Status == IPStatus.TimedOut)//���ping�ɹ�             
                //        {
                //            MessageBox.Show("�޷����ӵ���˾����������");
                //            return;
                //        }
                //    }
                //}
                #endregion

                if (args.Length == 0)
                {
                    string ifLogin = System.Configuration.ConfigurationManager.AppSettings["ByPrintSpec"];
                    if (ifLogin != "True")
                    {
                        return;
                    }
                }

                FlashScreen.Show("���ڼ�������");

                NHibernate.Cfg.Environment.UseReflectionOptimizer = false;
                string formScale = ClientUtil.ToString(ConfigurationManager.AppSettings["FormScale"]);//�ƶ��豸���ű���

                ClearCach();
                FlashScreen.Close();

                if (!System.Diagnostics.Debugger.IsAttached)
                {
                    ProgramUpdate();
                }
                
                if (args.Length == 0)
                {
                    if (!LoginWithNoArgs(ref specialScale))
                    {
                        return;
                    }
                }
                else if (IsJoinLogin(args))
                {
                    if (!LoginWithJoinArgs(ArrPara, args))
                    {
                        return;
                    }
                }
                else
                {
                    LoginWithArgs(args);
                }

                string SysManager = System.Configuration.ConfigurationManager.AppSettings["SysManager"].ToString();
                string PartSysManager = System.Configuration.ConfigurationManager.AppSettings["PartSysManager"].ToString();
               
                LoginInfomation.LoginInfo = AppDomain.CurrentDomain.GetData("TheLogin") as Login;
                CallContextUtil.LogicalSetData<Login>("LoginInformation", AppDomain.CurrentDomain.GetData("TheLogin") as Login);
                CallContextUtil.LogicalSetData<ComponentPeriod>("FiscalPeriod", (AppDomain.CurrentDomain.GetData("TheLogin") as Login).FiscalModule);
                CallContextUtil.LogicalSetData<ComponentPeriod>("ComponentPeriod", (AppDomain.CurrentDomain.GetData("TheLogin") as Login).TheComponentPeriod);
                CallContextUtil.LogicalSetData<long>("BSCode", ClientUtil.ToLong(System.Configuration.ConfigurationManager.AppSettings["BSCode"]));
                CallContextUtil.LogicalSetData<bool>("ProducePlanDateDefault", ClientUtil.ToBool(System.Configuration.ConfigurationManager.AppSettings["ProducePlanDateDefault"]));

                if (args.Length == 0)
                {
                    if (specialScale == "-1" || specialScale == "0")
                    {
                        System.Windows.Forms.Application.Run(new Framework(ArrPara));
                    }
                    else
                    {
                        ConstObject.TheLogin = AppDomain.CurrentDomain.GetData("TheLogin") as Login;
                        System.Windows.Forms.Application.Run(new VMobileMainMenu());
                    }
                }
                else
                {
                    System.Windows.Forms.Application.Run(new Framework(ArrPara));
                }

                ClearTempData();

            }
            catch (Exception ee)
            {
                string ss = VirtualMachine.Component.Util.ExceptionUtil.ExceptionMessage(ee);

                if (ss.Contains(":8998"))
                    MessageBox.Show("�����жϻ��������������,����ϵͳ����Ա��ϵ��");
                else
                    MessageBox.Show(ss);
                
            }
            finally
            {
            }
        }

        static void ProgramUpdate()
        {
            string ipAddress = "10.70.18.161";
            string updateServerAddress = System.Configuration.ConfigurationManager.AppSettings["ProIp"];
            Ping ping = new Ping();//����Ping����
            PingReply pr = ping.Send(ipAddress);//��ָ��IP�����������ļ��������ICMPЭ���ping���ݰ� 
            if (pr.Status == IPStatus.Success && ClientUtil.ToString(updateServerAddress).IndexOf("cscec3b.com") != -1)//���ping�ɹ�
            {
                updateServerAddress = ipAddress;
            }
            //������³���
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string serverAdd = System.Configuration.ConfigurationManager.AppSettings["ProChannel"] +
                               "://" + updateServerAddress +
                               ":" + System.Configuration.ConfigurationManager.AppSettings["ProPort"];
            basePath = basePath.Replace(' ', '�');

            Process p = Process.Start(AppDomain.CurrentDomain.BaseDirectory + @"ProgramUpdate.exe", basePath + @"SupplyChainClient.exe " + serverAdd + "/UpdateService");
            p.WaitForExit();
        }

        static bool LoginWithNoArgs(ref string specialScale)
        {
            BusinessModule aa = new BusinessModule();
            aa.Id = "16";
            BusinessModule bb = new BusinessModule();
            bb.Id = "16";
            specialScale = System.Configuration.ConfigurationManager.AppSettings["FormScale"];

            ////*****************************************888
            specialScale = "-1";
            //specialScale = "0";
            bool loginResult = false;
            if (specialScale == "-1")
            {
                CLogin loginTmp = new CLogin(aa, bb);
                Login loginInfo = loginTmp.Excute() as Login;
                loginResult = loginTmp.IsSuccess;
            }
            else if (specialScale == "0")
            {
                CLogin loginTmp = new CLogin(aa, bb);
                Login loginInfo = loginTmp.Excute(LoginType.loginIndex, "ouyangxinmin") as Login;
                loginResult = loginTmp.IsSuccess;
            }
            else
            {
                Application.Business.Erp.ResourceManager.Client.Basic.CMobileLogin loginTmp = new Application.Business.Erp.ResourceManager.Client.Basic.CMobileLogin(aa, bb);
                Login loginInfo = loginTmp.Excute() as Login;
                loginResult = loginTmp.IsSuccess;
            }

            return loginResult;
        }

        static bool LoginWithJoinArgs(string[] ArrPara, string[] args)
        {
            //string sTest = string.Empty;
            //L={0} T={1} W={2} H={3} Name={4} UsrCode={5}
            ArrPara = new string[7];
            int iLeft = GetArg(args[0], "L=");
            ArrPara[0] = iLeft.ToString();
            int iTop = GetArg(args[1], "T=");
            ArrPara[1] = iTop.ToString();
            int iWidth = GetArg(args[2], "W=");
            ArrPara[2] = iWidth.ToString();
            int iHeight = GetArg(args[3], "H=");
            ArrPara[3] = iHeight.ToString();
            string sText = GetValue(args[4], "Name=");
            ArrPara[4] = sText;
            string sPerCode = GetValue(args[5], "UsrCode=").ToLower();
            ArrPara[5] = sPerCode;
            string sMenuName = GetValue(args[6], "Menu=");

            if (sMenuName == "CompanyBusinessManagement")
            {
                ConstObject.IRPMenuName = "��˾�������";
            }
            else if (sMenuName == "ProjectBasicManagement")
            {
                ConstObject.IRPMenuName = "��Ŀ����ҵ��";
            }
            else if (sMenuName == "ProjectResourceManagement")
            {
                ConstObject.IRPMenuName = "������Դ����";
            }
            ArrPara[6] = ConstObject.IRPMenuName;
            // MessageBox.Show(sMenuName);
            //sTest=args[0]+"  "+args[1]+"  "+args[2]+"  "+args[3]+"  "+args[4]+"  "+args[5]+"  ";
            //MessageBox.Show(sTest );
            BusinessModule aa = new BusinessModule();
            aa.Id = "16";
            BusinessModule bb = new BusinessModule();
            bb.Id = "16";
            CLogin loginTmp = new CLogin(aa, bb);
            Login loginInfo = loginTmp.Excute(LoginType.loginIndex, sPerCode) as Login;
            return loginTmp.IsSuccess;
        }

        static void LoginWithArgs(string[] args)
        {
            if (args[0].Equals("autologin=true"))
            {
                //������ʽ string parameters = @"autologin=true username=" + userName + " password=" + password+" groupId="+groupId+" roleId="+roleId+" loginDate=\""+loginDate+"\"";
                //�Ż���¼������ʽ
                //autologin=true username=admin password=1 groupId=1 roleId=215 loginDate=2012-2-14
                //��Ŀ���л���¼��ʽ
                //autologin=true username=admin password=1 groupId=1 roleId=215 loginDate=2012-2-14 projectId="1tu$4a1dz33wfGRwfmGuq9"
                //ִ���Զ���¼

                string s = args[1];
                int index = s.IndexOf("=");
                //string[] sa = s.Split(new char[] { '=' });
                string userName = s.Substring(index + 1);

                //����
                s = args[2];
                index = s.IndexOf("=");
                //sa = s.Split(new char[] { '=' });
                string password = s.Substring(index + 1);

                //�˵�����
                string IRPMenuName = "";
                s = args[3];
                if (s != null)
                {
                    index = s.IndexOf("=");
                    IRPMenuName = s.Substring(index + 1);
                    if (IRPMenuName == "CompanyBusinessManagement")
                    {
                        ConstObject.IRPMenuName = "��˾�������";
                    }
                    else if (IRPMenuName == "ProjectBasicManagement")
                    {
                        ConstObject.IRPMenuName = "��Ŀ����ҵ��";
                    }
                    else if (IRPMenuName == "ProjectResourceManagement")
                    {
                        ConstObject.IRPMenuName = "������Դ����";
                    }
                }

                //��λ
                string role = "";
                s = args[4];
                if (s != null)
                {
                    index = s.IndexOf("=");
                    role = s.Substring(index + 1);
                }
                //��¼����
                string loginDate = "";
                s = args[5];
                if (s != null)
                {
                    index = s.IndexOf("=");
                    loginDate = s.Substring(index + 1);
                }

                if (args.Length > 6)
                {
                    //��ĿID
                    s = args[6];
                    if (s != null)
                    {
                        index = s.IndexOf("=");
                        string projectId = s.Substring(index + 1);
                        if (!string.IsNullOrEmpty(projectId))
                        {
                            ConstObject.IRPMenuName = "��Ŀ����ҵ��";
                            ConstObject.AutoSwitchProjectId = projectId;
                        }
                    }
                }

                CLogin aa = new CLogin();
                if (!aa.DoAutoLogin(userName, password, role, loginDate, "16", "16"))
                {
                    //MessageBox.Show("�������");
                    //return;
                }
            }
            else
            {

                Login aLogin = (Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.ConstMethod.DeserializeFromFile(AppDomain.CurrentDomain.BaseDirectory + "Login.dat") as Login);
                AppDomain.CurrentDomain.SetData("TheLogin", aLogin);
                CLogin aa = new CLogin();
                aa.DoLogin(aLogin.ThePerson.Id, "", aLogin.TheSysRole.Id, aLogin.LoginDate, "3");
                AppDomain.CurrentDomain.SetData("TheLogin", aLogin);
            }
        }

        static void ClearTempData()
        {
            //ɾ��������ʱ�ļ�
            string tempFilePreviewPath = AppDomain.CurrentDomain.BaseDirectory + "TempFilePreview\\";
            try
            {
                if (System.IO.Directory.Exists(tempFilePreviewPath))
                {
                    System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(tempFilePreviewPath);
                    System.IO.DirectoryInfo[] dirInfos = dir.GetDirectories();
                    for (int i = dirInfos.Length - 1; i > -1; i--)
                    {
                        System.IO.DirectoryInfo dirChild = dirInfos[i];

                        System.IO.FileInfo[] fileInfos = dirChild.GetFiles();
                        for (int j = fileInfos.Length - 1; j > -1; j--)
                        {
                            fileInfos[j].Delete();
                        }

                        dirChild.Delete();
                    }
                }
            }
            catch
            {
            }
        }

        static string[] TestArg()
        {
            string[] ArrPara = null;
            ArrPara = new string[6];
            
            ArrPara[0] = "200";
           
            ArrPara[1] = "200";
            
            ArrPara[2] = "500";
            
            ArrPara[3] ="500";
           
            ArrPara[4] = "";
            
            ArrPara[5] = "";
            return ArrPara;
                   
        }

        static bool IsJoinLogin(string[] args)
        {
            bool Flag = false;
            
            string sStartsWith="L=";
            if (args.Length > 0)
            {
                if (!string.IsNullOrEmpty(args[0]) && args[0].StartsWith(sStartsWith))
                {
                   
                    Flag = true;
                }
                
            }
            return Flag;
        }

        static string GetValue(string sValue, string sStartWith)
        {
            sValue = sValue.Substring(sStartWith.Length);
            //sValue = sValue.Replace(sStartWith, "");
            return sValue;
        }

        static int GetArg(string sValue, string sStartWith)
        {
            int iValue = 0;
            try
            {
                if(!string.IsNullOrEmpty (sValue ))
                {
                    sValue = GetValue(sValue, sStartWith);
                    iValue = ClientUtil.ToInt(sValue);
                }
            }
            catch
            {
            }
            return iValue;
        }

        static void JoinLogin(string[] args)
        {
            //L={0} T={1} W={2} H={3} Name={4} UsrCode={5}
            int iLeft = GetArg(args[0], "L=");
            int ITop = GetArg(args[1], "T=");
            int iWidth = GetArg(args[2], "W=");
            int iHeight = GetArg(args[3], "H=");
            string  sText=GetValue (args [4],"Name=");
            string sPerCode = GetValue(args[5], "UsrCode=");

        }

        static void MyHandler(object sender, ThreadExceptionEventArgs args)
        {
            string ss = VirtualMachine.Component.Util.ExceptionUtil.ExceptionMessage(args.Exception);

            if (ss.Contains(":8998"))
                MessageBox.Show("�����жϻ��������������,����ϵͳ����Ա��ϵ��");
            else
                MessageBox.Show(ss);
            //MessageBox.Show(VirtualMachine.Component.Util.ExceptionUtil.ExceptionMessage(args.Exception));
        }

        static void ClearCach()
        {
        }
    }
}
