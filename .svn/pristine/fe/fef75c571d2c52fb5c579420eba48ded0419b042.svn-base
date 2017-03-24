using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.UI.MobileControls;
using System.Collections;
using System.Collections.Generic;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using System.IO;

/// <summary>
/// Summary description for CommonHelper
/// </summary>
public static class CommonHelper
{
    //public static string[] DepartmentOrder = ConfigurationManager.AppSettings["DepartmentOrder"].Split(';');
    public static string[] SimilarIndex = ConfigurationManager.AppSettings["SimilarIndex"].Split('|');
    public static string[] stateDescripe = { "编辑", "有效", "无效", "审批中", "挂起", "执行中", "冻结", "结束" };
    /// <summary>
    /// 添加列表项目到指定的位置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="target"></param>
    /// <param name="obj"></param>
    /// <param name="index"></param>
    public static void Add<T>(this IList target, T obj, int index)
    {
        IList temp = new List<T>();
        for (int i = 0; i < target.Count; i++)
        {
            if (i == index)
            {
                temp.Add(obj);
            }
            temp.Add(target[i]);
        }
    }

    /// <summary>
    /// 筛选指标
    /// </summary>
    /// <param name="drs">集合</param>
    /// <param name="indexName">指标名称</param>
    /// <param name="measurementunitname">计量单位</param>
    /// <param name="timeName">时间宾栏</param>
    /// <param name="warninglevelname">预警级别</param>
    /// <returns></returns>
    public static List<FundManagement> ToFundManagement(this IEnumerable<FundManagement> list, string indexName, string measurementunitname, string timeName, string warninglevelname)
    {
        return list.Where(a =>
        {
            if (!string.IsNullOrEmpty(indexName)) if (a.IndexName != indexName) return false;
            if (!string.IsNullOrEmpty(measurementunitname)) if (a.MeasurementUnitName != measurementunitname) return false;
            if (!string.IsNullOrEmpty(timeName)) if (a.TimeName != timeName) return false;
            if (!string.IsNullOrEmpty(warninglevelname)) if (a.WarningLevelName != warninglevelname) return false;
            return true;
        }).ToList();
    }

    private static List<string> _departmentList;
    /// <summary>
    /// 分支结构
    /// </summary>
    public static List<string> DepartmentList
    {
        get
        {
            if (_departmentList == null)
            {
                var departmentStr = File.ReadAllText(HttpContext.Current.Server.MapPath("~/www/data/department.json"));
                _departmentList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(departmentStr);
            }
            return _departmentList;
        }
    }
    /// <summary>
    /// 分支机构排序
    /// </summary>
    /// <param name="?"></param>
    /// <returns></returns>
    public static List<IGrouping<string, FundManagement>> Order(this List<IGrouping<string, FundManagement>> groups)
    {
        var result = new List<IGrouping<string, FundManagement>>();
        foreach (var item in DepartmentList)
        {
            var temp = groups.Where(a => a.Key == item).FirstOrDefault();
            if (temp != null) result.Add(temp);
        }
        return result;
    }

}
