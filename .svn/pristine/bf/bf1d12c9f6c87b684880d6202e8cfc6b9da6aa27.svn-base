using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
///UserControlBaseClass 的摘要说明
/// </summary>
public class UserControlBaseClass : UserControl
{
    public UserControlBaseClass()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }
    public string GetPageQueryString()
    {
        string queryStr = "";
        for (int i = 0; i < Request.QueryString.Count; i++)
        {
            if (queryStr == "")
                queryStr += "?" + Request.QueryString.Keys[i] + "=" + Request.QueryString[i];
            else
                queryStr += "&" + Request.QueryString.Keys[i] + "=" + Request.QueryString[i];
        }
        return queryStr;
    }
}
