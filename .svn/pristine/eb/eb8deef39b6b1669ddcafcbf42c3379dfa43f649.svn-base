<%@ WebHandler Language="C#" Class="GatheringData" %>

using System;
using System.Web;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

public class GatheringData : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string sMethodName = context.Request["key"];
        switch (sMethodName)
        {
            case "GetBill":
                {
                    GetBill(context);
                    break;
                }
        }
    }
   public  void GetBill(HttpContext context)
    {
        var id = context.Request["id"];
        var master = GlobalClass.FinanceMultDataSrv.GetGatheringMasterById(id);
        if (master == null)
        {
            context.Response.Write("{\"total\":0,\"rows\":[]}");
            return;
            return;
        }
        var result = new
        {
            code = master.Code,
            theCustomerName = master.TheCustomerName,
            theCustomerID = master.TheCustomerRelationInfo,
            createDate = master.CreateDate,
            total = master.Details.Count,
            state = master.DocState,
            
            rows = master.Details.Select(a =>
            {
                var temp = (GatheringDetail)a;
                return new
                {
                    Id = temp.Id,
                    Money = temp.Money,
                    Descript = temp.Descript
                };
            }).ToList()
        };
        context.Response.Write(JsonConvert.SerializeObject(result));
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}