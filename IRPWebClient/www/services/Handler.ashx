<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.Data;
using IRPServiceModel.Services.Common;
using Spring.Context;

public class Handler : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        var a = AppDomain.CurrentDomain.GetData("IRPServiceModel") as IApplicationContext;
        ICommonMethodSrv commonMethodSrv = a.GetObject("CommonMethodSrv") as ICommonMethodSrv;

        //context.Response.Write("dwdwdwd");

        DataSet ds = commonMethodSrv.GetData("select id, version, code, createperson, createpersonname, handleperson, handlepersonname, createyear, createmonth, createdate, businessdate, state, operationorg, operorgname, opgsyscode, summoney, descript, realoperationdate from thd_factoringdatamaster where 1=1 ");
        
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}