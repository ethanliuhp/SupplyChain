<%@ WebHandler Language="C#" Class="FactoringData" %>

using System;
using System.Web;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Service;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using System.Data;
using Newtonsoft.Json;
using System.Linq;

public class FactoringData : IHttpHandler
{
    IFinanceMultDataSrv service = GlobalClass.FinanceMultDataSrv;
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        string key = context.Request["key"];
        switch (key)
        {
            case "GetMasterList":
                GetMasterList(context);
                break;
            case "MasterAdd":
                MasterAdd(context);
                break;
            default:
                break;
        }
        context.Response.End();
    }

    void GetMasterList(HttpContext context)
    {
        DataSet ds = GlobalClass.CommonMethodSrv.GetData("select id, version, code, createperson, createpersonname, handleperson, handlepersonname, createyear, createmonth, createdate, businessdate, state, operationorg, operorgname, opgsyscode, summoney, descript, realoperationdate from thd_factoringdatamaster where 1=1 ");
        
        var list = ds.Tables[0].Select().Select(a => new {code = a["code"].ToString(),date = Convert.ToDateTime( a["createdate"]).ToString("yyyy-MM-dd")});

        var result = JsonConvert.SerializeObject(list);
        context.Response.Write(result);
    }

    void MasterAdd(HttpContext context)
    {
        var code = context.Request["code"];
        var remark = context.Request["remark"];
        FactoringDataMaster model = new FactoringDataMaster();
        model.Code = code;
        model.CreateDate = DateTime.Now;
        model.Descript = remark;
        service.Save(model);
        context.Response.Write("保存成功！");
    }


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}