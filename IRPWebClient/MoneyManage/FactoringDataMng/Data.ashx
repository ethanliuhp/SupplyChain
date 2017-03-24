<%@ WebHandler Language="C#" Class="Data" %>

using System;
using System.Web;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Service;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

public class Data : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        string key = context.Request["key"];
        switch (key)
        {
            case "GetMasterById":
                GetMasterById(context);
                break;
            case "SaveEdit":
                SaveEdit(context);
                break;
            default:
                break;
        }
        context.Response.End();
    }

    /// <summary>
    ///  获取主表信息
    /// </summary>
    /// <param name="context"></param>
    void GetMasterById(HttpContext context)
    {
        var id = context.Request["id"];
        var master = GlobalClass.FinanceMultDataSrv.GetFactoringDataById(id);
        if (master == null) return;
        var result = new
        {
            code = master.Code,
            createPerson = master.CreatePersonName,
            handlePersonName = master.HandlePersonName,
            descript = master.Descript,
            total = master.Details.Count,
            state = master.DocState,
            rows = master.Details.Select(a =>
            {
                var temp = (FactoringDataDetail)a;
                return new
                {
                    Id = temp.Id,
                    DepartmentName = temp.DepartmentName,
                    ProjectName = temp.ProjectName,
                    BankName = temp.BankName,
                    Balance = temp.Balance,
                    Rate = temp.Rate,
                    StartDate = temp.StartDate.ToString("yyy-MM-dd"),
                    EndDate = temp.EndDate.ToString("yyy-MM-dd"),
                    PayType = temp.PayType,
                    StartChargingDate = temp.StartChargingDate.ToString("yyy-MM-dd"),
                    EndChargingDate = temp.EndChargingDate.ToString("yyy-MM-dd"),
                    TotalDay = temp.TotalDay,
                    AmountPayable = temp.AmountPayable
                };
            }).ToList()
        };
        context.Response.Write(JsonConvert.SerializeObject(result));
    }

    /// <summary>
    /// 保存修改内容
    /// </summary>
    /// <param name="context"></param>
    void SaveEdit(HttpContext context)
    {
        var id = context.Request["id"];
        var master = GlobalClass.FinanceMultDataSrv.GetFactoringDataById(id);
        // 处理删除数据
        if (!string.IsNullOrEmpty(context.Request["deletedRow"]))
        {
            var ids = context.Request["deletedRow"].Split('|').ToList();
            var details = master.Details.Where(a => ids.Contains(a.Id)).ToList();
            master.Details.RemoveAll(details);
        }
        // 处理修改数据
        var updatedRow = JsonConvert.DeserializeObject<List<FactoringDataDetail>>(context.Request["updatedRow"]);
        if (updatedRow.Count > 0)
        {
            foreach (var updateItem in updatedRow)
            {
                var oldItem = master.Details.Where(a => a.Id == updateItem.Id).FirstOrDefault() as FactoringDataDetail;
                if (oldItem == null) continue;
                UpdateRecord(oldItem, updateItem);
            }
        }
        // 处理新增的数据
        var insertedRow = JsonConvert.DeserializeObject<List<FactoringDataDetail>>(context.Request["insertedRow"]);
        if (insertedRow.Count > 0)
        {
            foreach (var item in insertedRow)
            {
                item.TotalDay = (item.EndChargingDate - item.StartChargingDate).Days;
                item.Money = item.AmountPayable;
                item.Master = master;
                master.Details.Add(item);
            }
        }
        // 保存修改
        GlobalClass.FinanceMultDataSrv.Update(master);
    }

    /// <summary>
    /// 修改原数据
    /// </summary>
    /// <param name="oldItem"></param>
    /// <param name="updateItem"></param>
    void UpdateRecord(FactoringDataDetail oldItem, FactoringDataDetail updateItem)
    {
        oldItem.DepartmentName = updateItem.DepartmentName;
        oldItem.ProjectName = updateItem.ProjectName;
        oldItem.BankName = updateItem.BankName;
        oldItem.Balance = updateItem.Balance;
        oldItem.Rate = updateItem.Rate;
        oldItem.StartDate = updateItem.StartDate;
        oldItem.EndDate = updateItem.EndDate;
        oldItem.PayType = updateItem.PayType;
        oldItem.StartChargingDate = updateItem.StartChargingDate;
        oldItem.EndChargingDate = updateItem.EndChargingDate;
        oldItem.TotalDay = (updateItem.EndChargingDate - updateItem.StartChargingDate).Days;
        oldItem.AmountPayable = updateItem.AmountPayable;
        oldItem.Money = oldItem.AmountPayable;
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}