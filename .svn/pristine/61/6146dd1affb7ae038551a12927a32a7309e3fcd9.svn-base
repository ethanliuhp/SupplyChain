using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.ContextConfigure;
using System.ServiceModel;
using Application.Business.Erp.PortalIntegration.Service;
using System.ServiceModel.Description;

namespace PortalIntegration
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ContextConfig config = ContextConfig.DeSerialize(AppDomain.CurrentDomain.BaseDirectory + @"\ContextConfig.xml");
                AppDomain.CurrentDomain.SetData("ContextConfig", config);
                config.InitialContextGroup();

                RunService();

            }
            catch (Exception ex)
            {
                //System.IO.StreamWriter write = new System.IO.StreamWriter("error.txt", false, Encoding.Default);
                //write.WriteLine("Message:" + ex.Message);
                //write.WriteLine("InnerException:" + ex.InnerException);

                //write.Close();
                //write.Dispose();

                string ss = VirtualMachine.Component.Util.ExceptionUtil.ExceptionMessage(ex);
                if (ss.Contains(":8998"))
                {
                    Console.WriteLine("网络中断或服务器重新启动,请与系统管理员联系！");
                    Console.Read();
                }
                else
                {
                    Console.WriteLine(ss);
                    Console.Read();
                }
            }
        }

        private static void RunService()
        {
            string address = System.Configuration.ConfigurationSettings.AppSettings["ServiceAddress"];
            ServiceHost svcHost;
            svcHost = new ServiceHost(typeof(PortalService), new Uri(address));

            // Check to see if the service host already has a ServiceMetadataBehavior
            ServiceMetadataBehavior smb = svcHost.Description.Behaviors.Find<ServiceMetadataBehavior>();

            // If not, add one
            if (smb == null)
                smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Default;//.Policy15;
            svcHost.Description.Behaviors.Add(smb);
            ServiceDebugBehavior db = svcHost.Description.Behaviors.Find<ServiceDebugBehavior>();
            db.HttpHelpPageEnabled = true;
            db.IncludeExceptionDetailInFaults = true;

            // Add MEX endpoint
            svcHost.AddServiceEndpoint(
              ServiceMetadataBehavior.MexContractName,
              MetadataExchangeBindings.CreateMexHttpBinding(),
              "mex"
            );
            // Add application endpoint
            //WSHttpBinding binding = new WSHttpBinding();
            //WebHttpBinding binding = new WebHttpBinding();
            //binding.Security.Mode = WebHttpSecurityMode.None;
            BasicHttpBinding binding = new BasicHttpBinding();
            //binding.MaxReceivedMessageSize = int.MaxValue;
            //binding.MaxBufferSize = int.MaxValue;
            //binding.ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas()
            //{
            //    MaxStringContentLength = 2147483647
            //};
            binding.Security.Mode = BasicHttpSecurityMode.None; //SecurityMode.None;
            svcHost.AddServiceEndpoint(typeof(IPortalService), binding, "");
            // Open the service host to accept incoming calls
            svcHost.Open();

            // The service can now be accessed.
            Console.WriteLine("中建三局Portal与TRP同步组织用户服务已启动.");
            Console.WriteLine("服务地址：" + address);
            Console.WriteLine("键入exit退出程序.");
            Console.WriteLine();
            while (true)
            {
                string s = Console.ReadLine();
                if (s.Equals("exit"))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("键入exit退出程序");
                }
            }

            // Close the ServiceHostBase to shutdown the service.
            svcHost.Close();
        }
    }
}
