<%@ WebHandler Language="C#" Class="Main" %>

using System;
using System.Web;
using System.Data;
using System.Linq;
using Newtonsoft.Json;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Service;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;

public class Main : IHttpHandler {
    /// <summary>
    /// 服务对象
    /// </summary>
    IFinanceMultDataSrv service = GlobalClass.FinanceMultDataSrv;
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        string key = context.Request["key"];
        switch (key)
        {
            case "GetMasterList":
                GetMasterList(context);
                break;
            default:
                break;
        }
        context.Response.End();
        
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    void GetMasterList(HttpContext context)
    {
        DataSet ds = GlobalClass.CommonMethodSrv.GetData("select rownum as num,id, code, createpersonname, handlepersonname, to_char(createdate,'YYYY-MM-DD') as createdate, state, descript from thd_factoringdatamaster where 1=1");

        var result = ds.Tables[0].Select().Select(a => new { id = a["id"], code = a["code"], createpersonname = a["createpersonname"], handlepersonname = a["handlepersonname"], createdate = a["createdate"], state = a["state"], descript = a["descript"] });

        context.Response.Write(JsonConvert.SerializeObject(result));
    }

 
    public bool IsReusable {
        get {
            return false;
        }
    }

}