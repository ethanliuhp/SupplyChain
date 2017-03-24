using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VirtualMachine.Component.ContextConfigure;

namespace PortalIntegrationConsole
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                ContextConfig config = ContextConfig.DeSerialize(AppDomain.CurrentDomain.BaseDirectory + @"\ContextConfig.xml");
                AppDomain.CurrentDomain.SetData("ContextConfig", config);
                config.InitialContextGroup();
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
                    MessageBox.Show(ss);
                }
            }

            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new AutoBusinessService());
        }
    }
}
