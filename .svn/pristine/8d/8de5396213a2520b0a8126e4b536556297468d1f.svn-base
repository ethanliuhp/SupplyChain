using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;
 
using System.Collections;
using System.Web.UI;
using IRPServiceModel.Services.Common;
using Spring.Context;
using System.Security.Cryptography;
using VirtualMachine.SystemAspect.Security;
using AuthManagerLib.AuthMng.MenusMng.Domain;

 
/// <summary>
///PublicClass 的摘要说明
/// </summary>
public class PublicClass
{
    
    
  
    public delegate void Method();
    public delegate void SelectedIndexChanged(object sender, EventArgs e);
    public static string sUserInfoID = "_UseInfo";
    public static SessionInfo GetUseInfo(System.Web.SessionState.HttpSessionState oSession)
    {
        SessionInfo oSessionInfo = null;
        try
        {
            oSessionInfo = oSession[sUserInfoID] as SessionInfo;
        }
        catch { }
        return oSessionInfo;
    }
    public static void SaveUseInfo(SessionInfo oSessionInfo, System.Web.SessionState.HttpSessionState oSession)
    {
        oSession.Add(sUserInfoID, oSessionInfo);
    }
    /// <summary>
    /// 移除字符串尾部无效的零
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string RemoveInvalidZero(string value)
    {
        if (value.IndexOf(".") > -1)
        {
            while (value.LastIndexOf("0") == value.Length - 1)
            {
                value = value.Substring(0, value.Length - 1);
            }
            if (value.LastIndexOf(".") == value.Length - 1)
                value = value.Substring(0, value.Length - 1);
        }
        return value;
    }
    /// <summary>
    /// 移除数值尾部无效的零
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string RemoveInvalidZero(decimal decValue)
    {
        string value = decValue.ToString();
        if (value.IndexOf(".") > -1)
        {
            while (value.LastIndexOf("0") == value.Length - 1)
            {
                value = value.Substring(0, value.Length - 1);
            }
            if (value.LastIndexOf(".") == value.Length - 1)
                value = value.Substring(0, value.Length - 1);
        }
        return value;
    }

   
    private static string StrFilter(string strSource)
    {
        string resultStr = "";

        resultStr = strSource.Replace("&nbsp;", "");

        return resultStr;
    }

    static public string GetAdminName()
    {
        return GetConfigValue("管理员名称");
    }
    public static string GetAdminJob()
    {
        return GetConfigValue("管理员岗位");
    }
    public static string GetAdminRole()
    {
        return GetConfigValue("管理员角色");
    }
    public static string GetMenuRootCode()
    {
        return GetConfigValue("菜单根节点");
    }
    public static Menus GetRootMenu(){
      Menus oRootMenu=  GlobalClass.CommonMethodSrv.GetMenus(GetMenuRootCode());
      return oRootMenu;
    }
    static public string GetConfigValue(string sName)
    {
        string sValue = string.Empty;
        try
        {
            sValue = ConfigurationManager.AppSettings[sName];
        }
        catch
        {
        }
        return sValue;
    }



    /// <summary>
    /// 是否拥有此菜单权限
    /// </summary>
    /// <param name="EncryptMenuId">菜单ID</param>
    /// <param name="userSession">用户session</param>
    /// <returns></returns>
    public static bool IsHasMenuAuth(string MenuId, SessionInfo userSession)
    {
        if (userSession != null)
        {
            var query = from m in userSession.ListMenus
                        where m.Id == MenuId
                        select m;
            if (query.Count() > 0)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 获取身份验证的Key
    /// </summary>
    /// <returns></returns>
    public static string GetAuthMenuIdEncryptKey()
    {
        VirtualMachine.Component.Util.IFCGuidGenerator gen = new VirtualMachine.Component.Util.IFCGuidGenerator();
        return gen.GeneratorIFCGuid();
    }
    /// <summary>
    /// 获取身份验证的Value
    /// </summary>
    /// <returns></returns>
    public static string GetAuthMenuIdEncryptValue(string key)
    {

        return CryptoString.Encrypt(key,key);
    }
     
    
}
public enum EnumAlign { Left = 0, Center = 1, Right = 2 }
public enum ShowType
{
    AnyPerson = 0,
    Self = 1
}
public enum EditType
{
    Show=0,
    Edit=1,
    Add=2
}
