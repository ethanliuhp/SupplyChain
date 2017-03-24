<%@ WebHandler Language="C#" Class="FactoringDataHandler" %>

using System;
using System.Web;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Service;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Criterion;
using VirtualMachine.Core;

public class FactoringDataHandler : IHttpHandler
{

    IFinanceMultDataSrv service = GlobalClass.FinanceMultDataSrv;

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
 
        switch (context.Request["key"])
        {
            case "Add_SubDetail":
                Add_SubDetail(context);
                break;
            case "Detail_Delete":
                Detail_Delete(context);
                break;
            default:
                break;
        }

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    void Add_SubDetail(HttpContext context)
    {
        var model = FormatData<FactoringDataDetail>(context);
        model.Master = service.GetFactoringDataById(context.Request["ParentId"]);
        service.Save(model);
    }
    void Detail_Delete(HttpContext context)
    {
        ObjectQuery oq = new ObjectQuery();
        Disjunction dis = new Disjunction();
        dis.Add(Expression.Eq("Id", context.Request["id"]));
        oq.AddCriterion(dis);
        IList list = service.QueryDetial(oq);
        foreach (FactoringDataDetail model in list)
        {
            service.Delete(model);
        }



        //string id = context.Request["id"];
        //FactoringDataDetail model = new FactoringDataDetail();
        //model.Id = id;
        //service.Delete(model);
        
    }

    T FormatData<T>(HttpContext context) where T : new()
    {
        var columns = context.Request["columns"];
        var columnsName = columns.Split('|');
        Type type = typeof(T);
        var obj = Activator.CreateInstance(type);
        foreach (var item in columnsName)
        {
            var propery = type.GetProperty(item);
            if (propery == null) continue;
            switch (propery.PropertyType.Name)
            {
                case "Int32":
                    propery.SetValue(obj, Convert.ToInt32(context.Request[item]), null);
                    break;
                case "DateTime":
                    if (!string.IsNullOrEmpty(context.Request[item]))
                        propery.SetValue(obj, DateTime.Parse(context.Request[item]), null);
                    break;
                case "Decimal":
                    propery.SetValue(obj, decimal.Parse(context.Request[item]), null);
                    break;
                default:
                    propery.SetValue(obj, context.Request[item], null);
                    break;
            }
        }
        return (T)obj;
    }

}