using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class ForCS_Plugin : PageBaseClass
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TransferTo();
        }
    }

    private void TransferTo()
    {
        
        string pluginUrl = ConfigurationManager.AppSettings["PluginUrl"];
        string redirectToPluginUrl = Request["RedirectTo"];
        string openSubSystem = "true";
        string subSystemId = ConfigurationManager.AppSettings["SubSystemId"];
        string IRPMenuName = Request["PortalMenuName"];

        string user = Request["userCode"];
        string password = "";
        string groupId = "";
        string roleId = Request["groupId"];
        

        if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(roleId))
        {
            //2012.6.18 直接把roleId的值改为groupId(即岗位ID)，groupId的值为菜单名称 这样MBP那边可以不用更改
            groupId = IRPMenuName;

            string loginDate = DateTime.Now.ToShortDateString();
            string oldAddress = "http://" + Request.Url.Authority + Request.ApplicationPath + "/setup.rar";

            string address = oldAddress.Replace(":", "!");
            address = HttpUtility.UrlEncode(address);

            //oldAddress = HttpUtility.UrlEncode(oldAddress);

            string openWinFormUrl = "Owf:OpenForm?subSystem=" + subSystemId + ",user=" + user + ",password=,groupId=" + groupId + ",roleId=" + roleId + ",loginDate=" + loginDate + ",address=" + address;

            string IE7DownURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/IE7.rar";
            string IE8DownURL = "http://" + Request.Url.Authority + Request.ApplicationPath + "/IE8.rar";
            try
            {
                //StreamWriter w1 = new StreamWriter(Server.MapPath("~/log.txt"), true, Encoding.Default);
                //w1.WriteLine(DateTime.Now.ToLongDateString() + openWinFormUrl);
                //w1.Close();
                //w1.Dispose();
            }
            catch
            {

            }
            //openWinFormUrl = "Owf:OpenForm?subSystem=" + 5 + ",user=" + user + ",password=" + password + ",groupId=" + groupId + ",roleId=" + roleId + ",loginDate=" + loginDate;

            if (!string.IsNullOrEmpty(redirectToPluginUrl))
            {
                Response.Redirect(pluginUrl);
            }


            if (!string.IsNullOrEmpty(openSubSystem))
            {
                //Server.Transfer(openWinFormUrl);
                //Response.Redirect(openWinFormUrl);
                //string guid = Guid.NewGuid().ToString().Replace("-","");
                string script = @"<Script language ='Javascript'>
                                var openWin=null;
                                function test()
                                {
                                    var flag=false;

                                    try
                                    {

//                                        var v= checkNavigator();


//                                        if(v.indexOf('IE')==-1)
//                                        {
//                                            alert('请使用IE浏览器!');
//                                            return false;
//                                        }

//                                        if(!document.documentMode && v.substring(2)<7)//非IE7
//                                        {
//                                            alert('请升级至IE7版本.');
//                                            if (confirm('要下载安装IE7吗?')) {
//                                                window.open('" + IE7DownURL + @"');
//                                            }
//                                            return false;
//                                        }
//                                        else if(!document.documentMode && v.substring(2)<8)//非IE8
//                                        {
//                                            alert('请升级至IE8版本.');
//                                            if (confirm('要下载安装IE8吗?')) {
//                                                window.open('" + IE8DownURL + @"');
//                                            }
//                                            return false;
//                                        }

                                        openWin= window.open('" + openWinFormUrl + @"','','height=20,width=40,top=170,left=170,toolbar=no,menubar=no,scrollbars=no,resizable=no,location=no,status=no');

                                        //当openWin为null或者没有操作权限的时候(都会抛出异常)表示没有找到项目管理模块
                                        var parent= openWin.opener;
                                        openWin.opener=null;
                                    }
                                    catch (e)
                                    {
                                        flag=true;
                                    }

                                    if(flag==true)
                                    {
//                                        self.focus();
//
//                                        if (confirm('项目管理模块未安装,要下载安装吗?')) {
//                                            window.open('" + oldAddress + @"');
//                                        }
                                    }
                                    setTimeout('CloseWin()',4000); 
                                }

                                    function CloseWin(){
                                        try{
                                            openWin.opener=null;
                                            openWin.close();
                                        }
                                        catch (e)
                                        {
                                        }
                                    }





                                //检验浏览器
                                function checkNavigator() {
                                    if (navigator.userAgent.indexOf('IE') > 0) {
                                        var versionname = navigator.appVersion;
                                        if (navigator.userAgent.indexOf('360SE') > -1)//判断是否360浏览器
                                            return '360';
                                        else if (versionname.indexOf('MSIE 5') > -1)
                                            return 'IE5';
                                        else if (versionname.indexOf('MSIE 6') > -1)
                                            return 'IE6';
                                        else if (versionname.indexOf('MSIE 9') > -1)
                                            return 'IE9';
                                        else if (versionname.indexOf('MSIE 8') > -1)
                                            return 'IE8';
                                        else if (versionname.indexOf('MSIE 7') > -1)
                                            return 'IE7'
                                        else if (versionname.indexOf('Maxthon') > -1)//傲游浏览器
                                            return 'Maxthon';
                                        else if (versionname.indexOf('TencentTraveler') > -1)//判断是否TT浏览器
                                            return 'TT';
                                        else
                                            return 'IE';

                                    }
                                    if (isFirefox = navigator.userAgent.indexOf('Firefox') > 0)//火狐
                                        return 'Firefox';
                                    if (isSafari = navigator.userAgent.indexOf('Safari') > 0)
                                        return 'Safari';
                                    if (isCamino = navigator.userAgent.indexOf('Camino') > 0)
                                        return 'Camino';
                                    if (isMozilla = navigator.userAgent.indexOf('Gecko') > 0)
                                        return 'Gecko';
                                    return false;
                                }

                                test();

                              </Script>";

                ClientScript.RegisterClientScriptBlock(GetType(), "OpenWindow", script);
            }
        }
        else
        {
            MessageBox("登录人为空！");
        }
    }
}
