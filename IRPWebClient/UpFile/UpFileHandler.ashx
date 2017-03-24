<%@ WebHandler Language="C#" Class="UpFileHandler" %>

using System;
using System.Web;
using System.Text;
public class UpFileHandler : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        //获取上传的文件的对象  
        HttpPostedFile file = context.Request.Files["btnfile"];
        string path=context.Request.QueryString["hdFolder"];
        //获取上传文件的名称  
        string sFileName = file.FileName;
        //截取获得上传文件的名称(ie上传会把绝对路径也连带上，这里只得到文件的名称)  
        string str = sFileName.Substring(sFileName.LastIndexOf("\\") + 1);
        
        path =(string.IsNullOrEmpty(path)? "~/temp/" :path) + str;
        //保存文件  
        file.SaveAs(context.Server.MapPath(path));
        //HttpRuntime.AppDomainAppVirtualPath主要是获取应用程序虚拟路径名称，因为响应给页面时不会自动添加而导致无法显示图片
        string sWebFilePath = HttpRuntime.AppDomainAppVirtualPath + (path.IndexOf("~") > -1 ? path.Substring(1) : path);
        string sFilePath = context.Server.MapPath(path);
        StringBuilder oBuilder = new StringBuilder();
        oBuilder.Append("[{");
        oBuilder.AppendFormat("\"WebFilePath\":\"{0}\",",UtilClass.EncodeURIComponent( sWebFilePath));
        oBuilder.AppendFormat("\"FilePath\":\"{0}\",", UtilClass.EncodeURIComponent(sFilePath));
        oBuilder.Append("}]");
        context.Response.Write(oBuilder.ToString());//path.Substring(1)用来去除第一个~字符
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}