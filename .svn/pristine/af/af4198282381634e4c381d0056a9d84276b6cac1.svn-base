<%@ WebHandler Language="C#" Class="MainHandler" %>

using System;
using System.Web;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Service;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using NHibernate.Criterion;
using VirtualMachine.Core;

public class MainHandler : IHttpHandler
{

    IFinanceMultDataSrv service = GlobalClass.FinanceMultDataSrv;

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        var key = context.Request["key"];
        switch (key)
        {
            case "GetDetailById":
                GetDetailById(context);
                break;
            case "GetMasterInfo":
                GetMasterInfo(context);
                break;
            case "SaveDetail":
                SaveDetail(context);
                break;
            case "EditDetail":
                EditDetail(context);
                break;
            case "DeleteDetail":
                DeleteDetail(context);
                break;
            default:
                break;
        }
        context.Response.End();
    }

    void GetDetailById(HttpContext context)
    {
        string id = context.Request["id"];

        var model = service.GetFactoringDataById(id);
        var detail = model.Details;
        string result = string.Empty;
        if (detail != null && detail.Count > 0)
        {
            result = JsonConvert.SerializeObject(detail.Select(a =>
                {
                    var info = (FactoringDataDetail)a;
                    return new { Id = info.Id, DepartmentName = info.DepartmentName, ProjectName = info.ProjectName, BankName = info.BankName, Balance = info.Balance, Rate = info.Rate, StartDate = info.StartDate.ToString("yyyy-MM-dd"), EndDate = info.EndDate.ToString("yyyy-MM-dd"), PayType = info.PayType, StartChargingDate = info.StartChargingDate.ToString("yyyy-MM-dd"), EndChargingDate = info.EndChargingDate.ToString("yyyy-MM-dd"), TotalDay = info.TotalDay, AmountPayable = info.AmountPayable };
                }));
        }
        context.Response.Write(result);
    }

    void GetMasterInfo(HttpContext context)
    {
        var master = service.GetFactoringDataById(context.Request["id"]);
        context.Response.Write(JsonConvert.SerializeObject(new { Code = master.Code, CreatePersonName = master.CreatePersonName, HandlePersonName = master.HandlePersonName, Descript = master.Descript }));
    }

    void SaveDetail(HttpContext context)
    {
        var str = context.Request["data"];
        var data = (List<FactoringDataDetail>)JsonConvert.DeserializeObject(str, typeof(List<FactoringDataDetail>));
        var parentId = context.Request["id"];
        var master = service.GetFactoringDataById(parentId);
        foreach (var item in data)
        {
            item.Master = master;
        }
        service.SaveList(data);
        GetDetailById(context);
    }

    void EditDetail(HttpContext context)
    {
        var id = context.Request["id"];
        var str = context.Request["data"];
        var data = (FactoringDataDetail)JsonConvert.DeserializeObject(str, typeof(FactoringDataDetail));
        var master = service.GetFactoringDataById(id);
        data.Master = master;
        service.Update(data);
    }

    void DeleteDetail(HttpContext context)
    {
        ObjectQuery oq = new ObjectQuery();
        Disjunction dis = new Disjunction();
        dis.Add(Expression.Eq("Id", context.Request["detailId"]));
        oq.AddCriterion(dis);
        IList list = service.QueryDetial(oq);
        foreach (FactoringDataDetail model in list)
        {
            service.Delete(model);
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}