<%@ Application Language="C#" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        //在应用程序启动时运行的代码
        try
        {
            #region 初始化配置
            //log4net.Config.XmlConfigurator.Configure();
            //log4net.LogManager.GetLogger(typeof(Global)).Debug("kkkkkkkkkkkkkkkkkkkkkkkkkkk");

            VirtualMachine.Component.ContextConfigure.ContextConfig config = VirtualMachine.Component.ContextConfigure.ContextConfig.DeSerialize(Server.MapPath("~/ContextConfig.xml"));
            AppDomain.CurrentDomain.SetData("ContextConfig", config);
            //log4net.LogManager.GetLogger(typeof(Global)).Debug("序列化结束");
            config.InitialContextGroup();

            #endregion

            Application["ComponentArtWebUI_AppKey"] = @"This edition of ComponentArt Web.UI is licensed for teamdesign application only.";
        }
        catch (Exception ex)
        {
            System.IO.StreamWriter write = new System.IO.StreamWriter(Server.MapPath("~/") + "\\error.txt", false, Encoding.Default);
            write.WriteLine("error:" + getMessage(ex));
            write.Close();
            write.Dispose();
            throw ex;
        }
    }

    protected string getMessage(Exception e)
    {
        if (e.InnerException == null)
            return "Message:" + e.Message;
        else
            return "Message:" + e.Message + Environment.NewLine + "InnerException:" + getMessage(e.InnerException);
    }

    void Application_End(object sender, EventArgs e)
    {
        //在应用程序关闭时运行的代码

    }

    void Application_Error(object sender, EventArgs e)
    {
        //在出现未处理的错误时运行的代码

    }

    void Session_Start(object sender, EventArgs e)
    {
        //在新会话启动时运行的代码

    }

    void Session_End(object sender, EventArgs e)
    {
        //在会话结束时运行的代码。 
        // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
        // InProc 时，才会引发 Session_End 事件。如果会话模式 
        //设置为 StateServer 或 SQLServer，则不会引发该事件。

    }
       
</script>

