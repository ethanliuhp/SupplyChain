<%@ WebHandler Language="C#" Class="FundManagementHandler" %>

using System;
using System.Web;
using System.Data;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using System.Collections.Generic;

public class FundManagementHandler : IHttpHandler
{
    private string[] indexStrs = { "营业收入", "收款", "借款", "保理", "资金存量", "应收账款", "应收未收款", "收现率", "应收款拖欠", "资金占用", "业主审量", "保证金余额", "收保证金", "付保证金" };
    private string[] timeStrs = { "本日", "本月", "本年", "累计", "日均", "月均", "上年" };
    private string[] measurementUnitStrs = { "元", "个", "百分比" };
    private string[] warntStrs = { "正常", "黄色预警", "橙色预警", "", "红色预警" };
    private DateTime date;

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        if (string.IsNullOrEmpty(context.Request["date"]))
            date = DateTime.Now.Date;
        else
            date = Convert.ToDateTime(context.Request["date"]);

        string key = context.Request["key"];
        switch (key)
        {
            case "GetCompanyIndex":                                 // 获取公司指标
                GetCompanyIndex(context);
                break;
            case "Businessincome":                                  // 分公司营业收入
                Businessincome(context);
                break;
            case "Stockmomey":                                      // 资金存量情况
                Stockmomey(context);
                break;
            case "ReceivableMoney":                                 // 收款情况表
                ReceivableMoney(context);
                break;
            case "AccountsReceivable":                              // 应收账款情况表
                AccountsReceivable(context);
                break;
            case "Arrears":                                         // 应收拖欠款情况表
                Arrears(context);
                break;
            case "Occupy":                                          // 资金占用情况表
                Occupy(context);
                break;
            case "Cashrate":                                        // 累计收现率情况表
                Cashrate(context);
                break;
            case "BusinessincomeDetail":                            // 各单位营业收入详情
                BusinessincomeDetail(context);
                break;
            case "CashrateDetail":                                  // 收现率预警
                CashrateDetail(context);
                break;
            case "ReceivablesDetail":                               // 各单位收款详情
                ReceivablesDetail(context);
                break;
            case "OccupyDetail":                                    // 各单位资金占用预警
                OccupyDetail(context);
                break;
            case "AccountsreceivableDetail":                        // 应收账款情况表
                AccountsreceivableDetail(context);
                break;
            case "ArrearsDetail":                                   // 各单位拖欠款情况
                ArrearsDetail(context);
                break;
            case "Margin":                                          // 保证金情况表
                Margin(context);
                break;
            case "BusinessPlannIcome":                              // 营业收入对比分析
                BusinessPlannIcome(context);
                break;
            default:
                break;
        }
        context.Response.End();
    }


    void GetCompanyIndex(HttpContext context)
    {
        string sqlText = string.Format("select id,createdate,indexname, indexid, measurementunitid, measurementunitname,timeid,timename,warninglevelid,warninglevelname, numericalvalue from thd_fundmanagement where organizationlevel='公司' and createdate between to_date('{0} 00:00:00','yyyy-mm-dd hh24:mi:ss') and to_date('{0} 23:59:59','yyyy-mm-dd hh24:mi:ss')", date.ToString("yyyy-MM-dd"));
        var result = GlobalClass.CommonMethodSrv.GetData(sqlText).Tables[0].Select().Select(a => new FundManagement()
        {   // 得到当日公司级别的指标信息统计信息
            ID = a["id"].ToString(),
            IndexName = a["indexname"].ToString(),
            TimeName = a["timename"].ToString(),
            MeasurementUnitName = a["measurementunitname"].ToString(),
            WarningLevelName = a["warninglevelname"].ToString(),
            NumericalValue = Convert.ToDecimal(a["numericalvalue"]),
            Url = "businessincome",
            WarningCount = 999
        }).ToList();
        if (result.Count == 0)
        {
            context.Response.Write("[]");
            return;
        }

        List<FundManagement> list = GetCompanyIndex(result);

        context.Response.Write(JsonConvert.SerializeObject(list));
    }

    /// <summary>
    /// 根据查询的结构，重组公司指标
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    private List<FundManagement> GetCompanyIndex(List<FundManagement> result)
    {
        List<FundManagement> list = new List<FundManagement>();
        // 营业收入
        var businessIncomeTotal = result.ToFundManagement(indexStrs[0], measurementUnitStrs[0], timeStrs[2], warntStrs[0]).FirstOrDefault();
        if (businessIncomeTotal != null)
        {
            businessIncomeTotal.Title = "本年营业收入";
            businessIncomeTotal.Url = "businessincome";
            list.Add(businessIncomeTotal);
        }
        // 收款
        var receivables = result.ToFundManagement(indexStrs[1], measurementUnitStrs[0], null, warntStrs[0]);
        // 当年收款
        var receivableYear = receivables.Where(a => a.TimeName == timeStrs[2]).FirstOrDefault();
        if (receivableYear != null)
        {
            receivableYear.Title = "本年收工程款";
            receivableYear.Url = "receivables";
            list.Add(receivableYear);
        }
        // 收现率
        var cashRate = result.ToFundManagement(indexStrs[7], measurementUnitStrs[2], timeStrs[2], warntStrs[0]).FirstOrDefault();
        if (cashRate != null)
        {
            cashRate.Title = "本年收现率";
            cashRate.Url = "cashrate";
            list.Add(cashRate);
        }

        // 应收账款
        var accountsReceivable = result.ToFundManagement(indexStrs[5], measurementUnitStrs[0], timeStrs[3], warntStrs[0]).FirstOrDefault();
        if (accountsReceivable != null)
        {
            accountsReceivable.Title = "累计应收账款";
            accountsReceivable.Url = "accountsreceivable";
            list.Add(accountsReceivable);
        }
        // 应收未收款
        var arrears = result.ToFundManagement(indexStrs[6], measurementUnitStrs[0], timeStrs[3], warntStrs[0]).FirstOrDefault();
        if (arrears != null)
        {
            arrears.Title = "累计应收拖欠";
            arrears.Url = "arrears";
            list.Add(arrears);
        }

        // 资金存量（集合）
        var stockMoney = result.ToFundManagement(indexStrs[4], measurementUnitStrs[0], null, null);
        // 资金存量
        var stockMoneyTotal = stockMoney.ToFundManagement(null, null, timeStrs[3], warntStrs[0]).FirstOrDefault();
        if (stockMoneyTotal != null)
        {
            stockMoneyTotal.Title = "累计资金存量";
            stockMoneyTotal.Url = "stockmomey";
            list.Add(stockMoneyTotal);
        }
        // 资金占用
        var occupy = result.ToFundManagement(indexStrs[9], measurementUnitStrs[0], timeStrs[3], warntStrs[0]).FirstOrDefault();
        if (occupy != null)
        {
            occupy.Title = "累计资金占用";
            occupy.Url = "occupy";
            list.Add(occupy);
        }

        // 应收保证金
        var margin = result.ToFundManagement(indexStrs[11], measurementUnitStrs[0], timeStrs[3], null).FirstOrDefault();
        if (margin != null)
        {
            margin.Title = "应收保证金";
            margin.Url = "margin";
            list.Add(margin);
        }

        // 预警信息
        string sqlWarning = string.Format("select t1.indexname,sum(numericalvalue) count from thd_fundmanagement t1 where t1.createdate=to_date('{0}','yyyy-mm-dd') and t1.warninglevelname like '%预警%' and measurementunitname='{1}' and organizationlevel='公司'  group by t1.indexname", date.ToString("yyyy-MM-dd"), measurementUnitStrs[1]);   // 查询所有预警
        var warnings = GlobalClass.CommonMethodSrv.GetData(sqlWarning).Tables[0].Select();
        foreach (var item in warnings)
        {
            //if (item["indexname"].ToString() == indexStrs[6]) item["indexname"] = indexStrs[6];     // 指标过滤
            var temp = list.Where(b => b.IndexName == item["indexname"].ToString()).FirstOrDefault();
            if (temp != null)
            {
                temp.WarningCount = Convert.ToInt32(item["count"]);     // 搜索指标赋值预警数量
            }
        }

        return list;
    }

    void Businessincome(HttpContext context)
    {
        var result = GetInfoByParams(context, indexStrs[0], null, measurementUnitStrs[0], warntStrs[0]);

        // 指标信息
        var orgNameList = result.GroupBy(a => a.OperOrgName).ToList().Order();
        List<object> list = new List<object>();
        foreach (var item in orgNameList)
        {
            var opgsyscode = result.Where(a => a.OperOrgName == item.Key).FirstOrDefault().OpgSysCode;
            var monthItem = result.Where(a => a.OperOrgName == item.Key && a.TimeName == timeStrs[1]).FirstOrDefault();
            var yearItem = result.Where(a => a.OperOrgName == item.Key && a.TimeName == timeStrs[2]).FirstOrDefault();
            list.Add(new { Name = item.Key.Substring(0, 2), Month = monthItem == null ? 0 : monthItem.NumericalValue, Year = yearItem == null ? 0 : yearItem.NumericalValue, Unit = monthItem == null ? measurementUnitStrs[0] : monthItem.MeasurementUnitName, ID = opgsyscode });
        }

        context.Response.Write(JsonConvert.SerializeObject(list));
    }

    void Stockmomey(HttpContext context)
    {
        var resultTotal = GetInfoByParams(context, null, null, null, measurementUnitStrs[0], warntStrs[0]);

        var result = resultTotal.Where(a => a.OrganizationLevel == "分公司").ToList();
        var orgNameList = result.GroupBy(a => a.OperOrgName).ToList().Order();
        List<object> list = new List<object>();
        var stockList = result.ToFundManagement(indexStrs[4], null, timeStrs[3], null);
        var borrowList = result.ToFundManagement(indexStrs[2], null, timeStrs[3], null);
        var dailyList = result.ToFundManagement(indexStrs[4], null, timeStrs[4], null);
        // 其中保理
        var resultFactoring = resultTotal.Where(a => a.OrganizationLevel == "公司" && a.IndexName == indexStrs[3]).FirstOrDefault();
        // 其中局借款
        var resultBorrow = resultTotal.Where(a => a.OrganizationLevel == "公司" && a.IndexName == indexStrs[2]).FirstOrDefault();
        foreach (var item in orgNameList)
        {
            var stockValue = stockList.Where(a => a.OperOrgName == item.Key).FirstOrDefault();
            var borrowValue = borrowList.Where(a => a.OperOrgName == item.Key).FirstOrDefault();
            var dailyValue = dailyList.Where(a => a.OperOrgName == item.Key).FirstOrDefault();
            if (stockValue != null || borrowValue != null || dailyValue != null) list.Add(new { Name = item.Key.Contains("公司总部") ? item.Key : item.Key.Substring(0, 2), StockValue = stockValue == null ? 0 : stockValue.NumericalValue, BorrowValue = item.Key.Contains("公司总部") ? resultFactoring.NumericalValue + resultBorrow.NumericalValue : borrowValue == null ? 0 : borrowValue.NumericalValue, DailyValue = dailyValue == null ? 0 : dailyValue.NumericalValue });
        }

        context.Response.Write(JsonConvert.SerializeObject(new { Factoring = resultFactoring == null ? 0 : resultFactoring.NumericalValue, Borrow = resultBorrow == null ? 0 : resultBorrow.NumericalValue, List = list }));
    }

    void ReceivableMoney(HttpContext context)
    {
        var result = GetInfoByParams(context, indexStrs[1], null, measurementUnitStrs[0], warntStrs[0]);

        var orgNameList = result.GroupBy(a => a.OperOrgName).ToList().Order();
        List<object> list = new List<object>();
        var dayList = result.ToFundManagement(null, null, timeStrs[0], null);
        var monthList = result.ToFundManagement(null, null, timeStrs[1], null);
        var yearList = result.ToFundManagement(null, null, timeStrs[2], null);
        foreach (var item in orgNameList)
        {
            var opgsyscode = result.Where(a => a.OperOrgName == item.Key).FirstOrDefault().OpgSysCode;
            var dayValue = dayList.Where(a => a.OperOrgName == item.Key).FirstOrDefault();
            var monthValue = monthList.Where(a => a.OperOrgName == item.Key).FirstOrDefault();
            var yearValue = yearList.Where(a => a.OperOrgName == item.Key).FirstOrDefault();
            list.Add(new { Name = item.Key.Substring(0, 2), Day = dayValue == null ? 0 : dayValue.NumericalValue, Month = monthValue == null ? 0 : monthValue.NumericalValue, Year = yearValue == null ? 0 : yearValue.NumericalValue, ID = opgsyscode });
        }
        context.Response.Write(JsonConvert.SerializeObject(list));
    }

    void AccountsReceivable(HttpContext context)
    {
        var curDate = date;
        //// 上年余额
        //date = new DateTime(curDate.Year - 1, 12, 31);
        //var lastBalance = GetInfoByParams(null, indexStrs[5], timeStrs[3], null, null);
        // 当前余额(累计应收账款)
        var surplusList = GetInfoByParams(null, indexStrs[5], timeStrs[3], measurementUnitStrs[0], warntStrs[0]);
        // 本年
        date = curDate;
        var result = GetInfoByParams(null, null, timeStrs[2], measurementUnitStrs[0], warntStrs[0]);
        // 本年增加
        var addList = result.ToFundManagement(indexStrs[10], null, null, null);
        // 本年减少
        var minusList = result.ToFundManagement(indexStrs[1], null, null, null);

        // 统计出所有分公司
        var lastDeptIds = surplusList.GroupBy(a => a.OperationOrg).ToList();
        var curDeptIds = result.GroupBy(a => a.OperationOrg).ToList();
        var deptList = new List<string>();
        foreach (var item in lastDeptIds)
        {
            deptList.Add(item.Key);
        }
        foreach (var item in curDeptIds)
        {
            if (!deptList.Contains(item.Key))
            {
                deptList.Add(item.Key);
            }
        }
        var list = new List<object>();
        foreach (var item in deptList)
        {
            var surplus = surplusList.Where(a => a.OperationOrg == item).FirstOrDefault();
            var add = addList.Where(a => a.OperationOrg == item).FirstOrDefault();
            var minus = minusList.Where(a => a.OperationOrg == item).FirstOrDefault();
            if (surplus == null && add == null && minus == null) continue;
            string name = surplus != null ? surplus.OperOrgName : add != null ? add.OperOrgName : minus != null ? minus.OperOrgName : "";
            var s = surplus != null ? surplus.NumericalValue : 0;
            var ba = add != null ? add.NumericalValue : 0;
            var m = minus != null ? minus.NumericalValue : 0;
            var b = s - ba + m;
            list.Add(new { Name = name.Substring(0, 2), Balance = b, Add = ba, Minus = m, Surplus = s });
        }
        context.Response.Write(JsonConvert.SerializeObject(list));
    }

    void Arrears(HttpContext context)
    {
        var result = GetInfoByParams(context, indexStrs[6], timeStrs[3], measurementUnitStrs[0], null);
        WarningHelper(context, result, measurementUnitStrs[0], measurementUnitStrs[0]);
    }

    void Occupy(HttpContext context)
    {
        var result = GetInfoByParams(context, indexStrs[9], timeStrs[3], null, null);
        WarningHelper(context, result, measurementUnitStrs[0], measurementUnitStrs[0]);
    }

    void Cashrate(HttpContext context)
    {
        var result = GetInfoByParams(context, indexStrs[7], null, null, null);
        var orgNameList = result.GroupBy(a => a.OperOrgName).ToList().Order();
        List<object> list = new List<object>();
        foreach (var item in orgNameList)
        {
            var orgList = result.Where(a => a.OperOrgName == item.Key);
            var opgsyscode = result.Where(a => a.OperOrgName == item.Key).FirstOrDefault().OpgSysCode;
            var normal = orgList.ToFundManagement(null, measurementUnitStrs[2], timeStrs[2], warntStrs[0]).FirstOrDefault();
            var yellow = orgList.ToFundManagement(null, measurementUnitStrs[1], timeStrs[3], warntStrs[1]).FirstOrDefault();
            var orange = orgList.ToFundManagement(null, measurementUnitStrs[1], timeStrs[3], warntStrs[2]).FirstOrDefault();
            var red = orgList.ToFundManagement(null, measurementUnitStrs[1], timeStrs[3], warntStrs[4]).FirstOrDefault();
            list.Add(new { Name = item.Key.Substring(0, 2), N = normal == null ? 0 : normal.NumericalValue, Y = yellow == null ? 0 : yellow.NumericalValue, O = orange == null ? 0 : orange.NumericalValue, R = red == null ? 0 : red.NumericalValue, ID = opgsyscode });
        }
        context.Response.Write(JsonConvert.SerializeObject(list));
    }

    void Margin(HttpContext context)
    {
        // "11,12,13"
        var result = GetInfoByParams(context, null, null, measurementUnitStrs[0], warntStrs[0]);
        var orgNameList = result.GroupBy(a => a.OperOrgName).ToList().Order();
        List<object> list = new List<object>();
        foreach (var item in orgNameList)
        {
            var orgList = result.Where(a => a.OperOrgName == item.Key);
            var last = orgList.ToFundManagement(indexStrs[11], null, timeStrs[6], null).FirstOrDefault();       // 上年余额
            var add = orgList.ToFundManagement(indexStrs[12], null, timeStrs[2], null).FirstOrDefault();        // 增加
            var minus = orgList.ToFundManagement(indexStrs[13], null, timeStrs[2], null).FirstOrDefault();      // 减少
            var balance = orgList.ToFundManagement(indexStrs[11], null, timeStrs[3], null).FirstOrDefault();    // 本年余额
            if (last == null && add == null && minus == null && balance == null) continue;
            list.Add(new { Name = item.Key.Substring(0, 2), L = last != null ? last.NumericalValue : 0, A = add != null ? add.NumericalValue : 0, M = minus != null ? minus.NumericalValue : 0, B = balance != null ? balance.NumericalValue : 0 });
        }
        context.Response.Write(JsonConvert.SerializeObject(list));
    }

    private void WarningHelper(HttpContext context, List<FundManagement> result, string unitNameTotal, string warningUnit)
    {
        var orgNameList = result.GroupBy(a => a.OperOrgName).ToList().Order();
        List<object> list = new List<object>();
        foreach (var item in orgNameList)
        {
            var orgList = result.Where(a => a.OperOrgName == item.Key);
            var opgsyscode = result.Where(a => a.OperOrgName == item.Key).FirstOrDefault().OpgSysCode;
            var normal = orgList.ToFundManagement(null, unitNameTotal, null, warntStrs[0]).FirstOrDefault();
            var yellow = orgList.ToFundManagement(null, warningUnit, null, warntStrs[1]).FirstOrDefault();
            var orange = orgList.ToFundManagement(null, warningUnit, null, warntStrs[2]).FirstOrDefault();
            var red = orgList.ToFundManagement(null, warningUnit, null, warntStrs[4]).FirstOrDefault();
            list.Add(new { Name = item.Key.Substring(0, 2), N = normal == null ? 0 : normal.NumericalValue, Y = yellow == null ? 0 : yellow.NumericalValue, O = orange == null ? 0 : orange.NumericalValue, R = red == null ? 0 : red.NumericalValue, ID = opgsyscode });
        }
        context.Response.Write(JsonConvert.SerializeObject(list));
    }

    private void WarningHandler(HttpContext context, List<FundManagement> result)
    {
        var orgNameList = result.GroupBy(a => a.OperOrgName).ToList();
        List<object> list = new List<object>();
        foreach (var item in orgNameList)
        {
            var tempList = result.Where(a => a.OperOrgName == item.Key);
            var yellowMoney = tempList.ToFundManagement(null, measurementUnitStrs[0], null, warntStrs[1]).FirstOrDefault();
            var yellowNum = tempList.ToFundManagement(null, measurementUnitStrs[1], null, warntStrs[1]).FirstOrDefault();
            var orangeMoney = tempList.ToFundManagement(null, measurementUnitStrs[0], null, warntStrs[2]).FirstOrDefault();
            var orangeNum = tempList.ToFundManagement(null, measurementUnitStrs[1], null, warntStrs[2]).FirstOrDefault();
            var redMoney = tempList.ToFundManagement(null, measurementUnitStrs[0], null, warntStrs[4]).FirstOrDefault();
            var redNum = tempList.ToFundManagement(null, measurementUnitStrs[1], null, warntStrs[4]).FirstOrDefault();
            if (yellowMoney != null || yellowNum != null) list.Add(new { Name = item.Key, LevelName = warntStrs[1], Num = yellowNum == null ? 0 : yellowNum.NumericalValue, Money = yellowMoney == null ? 0 : yellowMoney.NumericalValue });
            if (orangeMoney != null || orangeNum != null) list.Add(new { Name = item.Key, LevelName = warntStrs[2], Num = orangeNum == null ? 0 : orangeNum.NumericalValue, Money = orangeMoney == null ? 0 : orangeMoney.NumericalValue });
            if (redMoney != null || redNum != null) list.Add(new { Name = item.Key.Substring(0, 2), LevelName = warntStrs[4], Num = redNum == null ? 0 : redNum.NumericalValue, Money = redMoney == null ? 0 : redMoney.NumericalValue });
        }
        context.Response.Write(JsonConvert.SerializeObject(list));
    }

    /// <summary>
    /// 根据指标参数获取信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="indexName">指标名称</param>
    /// <param name="timeName">时间宾栏</param>
    /// <param name="unitName">计量单位</param>
    /// <param name="warningName">预警级别</param>
    /// <returns></returns>
    private List<FundManagement> GetInfoByParams(HttpContext context, string indexName, string timeName, string unitName, string warningName)
    {
        return GetInfoByParams(context, "分公司", indexName, timeName, unitName, warningName);
    }

    /// 根据指标参数获取信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="orgLevel">组织级别</param>
    /// <param name="indexName">指标名称</param>
    /// <param name="timeName">时间宾栏</param>
    /// <param name="unitName">计量单位</param>
    /// <param name="warningName">预警级别</param>
    /// <returns></returns>
    private List<FundManagement> GetInfoByParams(HttpContext context, string orgLevel, string indexName, string timeName, string unitName, string warningName)
    {
        string sqlText = string.Format("select operationorg, organizationlevel, opgsyscode, indexname, operorgname, measurementunitname, timename, warninglevelname, numericalvalue from thd_fundmanagement where organizationlevel like '%{5}%' and createdate between to_date('{0} 00:00:00','yyyy-mm-dd hh24:mi:ss') and to_date('{0} 23:59:59','yyyy-mm-dd hh24:mi:ss') and indexname like '{1}%' and timename like '{2}%' and measurementunitname like '{3}%' and warninglevelname like '{4}%'", date.ToString("yyyy-MM-dd"), indexName, timeName, unitName, warningName, orgLevel);
        var result = GlobalClass.CommonMethodSrv.GetData(sqlText).Tables[0].Select().Select(a => new FundManagement
        {
            OperationOrg = a["operationorg"].ToString(),
            OperOrgName = a["operorgname"].ToString(),
            OpgSysCode = a["opgsyscode"].ToString(),
            IndexName = a["indexname"].ToString(),
            TimeName = a["timename"].ToString(),
            MeasurementUnitName = a["measurementunitname"].ToString(),
            WarningLevelName = a["warninglevelname"].ToString(),
            NumericalValue = Convert.ToDecimal(a["numericalvalue"]),
            OrganizationLevel = a["organizationlevel"].ToString()
        }).ToList();
        return result;
    }

    void BusinessincomeDetail(HttpContext context)
    {
        string sqlText = string.Format("select t1.projectname,t1.numericalvalue from thd_fundmanagebyproject t1 where t1.createdate=to_date('{0}','yyyy-mm-dd') and t1.indexname='{1}' and t1.timename='{2}' and instr(t1.opgsyscode,'{3}')>0  order by t1.numericalvalue", date.ToString("yyyy-MM-dd"), indexStrs[0], timeStrs[2], context.Request["id"]);
        var result = GlobalClass.CommonMethodSrv.GetData(sqlText).Tables[0].Select().Select(a => new
        {
            Name = a["projectname"].ToString(),
            Number = Convert.ToDecimal(a["numericalvalue"])
        }).ToList();
        //if (result.Count <= 10)
        //{
        context.Response.Write(JsonConvert.SerializeObject(new { FrontList = result.Take(10) }));
        //    return;
        //}
        // 如果项目数大于10，则取前五与后五
        //context.Response.Write(JsonConvert.SerializeObject(new { FrontList = result.Take(5), BackList = result.Skip(result.Count - 5) }));
    }

    void CashrateDetail(HttpContext context)
    {
        var result = WarningDetail(indexStrs[7], context.Request["id"]);
        context.Response.Write(JsonConvert.SerializeObject(result));
    }

    void ReceivablesDetail(HttpContext context)
    {
        string sqlText = string.Format("select projectname,numericalvalue from thd_fundmanagebyproject where createdate=to_date('{0}','yyyy-mm-dd') and indexname='{1}' and timename='{2}' and instr(opgsyscode,'{3}')>0 order by numericalvalue", date.ToString("yyyy-MM-dd"), indexStrs[1], timeStrs[2], context.Request["id"]);
        var result = GlobalClass.CommonMethodSrv.GetData(sqlText).Tables[0].Select().Select(a => new
        {
            Name = a["projectname"].ToString(),
            Number = Convert.ToDecimal(a["numericalvalue"])
        }).ToList();
        context.Response.Write(JsonConvert.SerializeObject(result));
    }

    void OccupyDetail(HttpContext context)
    {
        var result = WarningDetail(indexStrs[9], context.Request["id"]);
        context.Response.Write(JsonConvert.SerializeObject(result));
    }

    void AccountsreceivableDetail(HttpContext context)
    {

    }

    void ArrearsDetail(HttpContext context)
    {
        var result = WarningDetail(indexStrs[6], context.Request["id"]);
        context.Response.Write(JsonConvert.SerializeObject(result));
    }

    private List<WarningEntity> WarningDetail(string indexName, string deptId)
    {
        // 黄色1、橙色2、红色3，其他的预警使用99
        string sqlText = string.Format("select projectname, case when warninglevelname='黄色预警' then 1 when warninglevelname='橙色预警' then 2 when warninglevelname='红色预警' then 3 else 99 end as warninglevelname,numericalvalue from thd_fundmanagebyproject where createdate=to_date('{0}','yyyy-mm-dd') and indexname='{1}' and warninglevelname like '%预警%' and instr(opgsyscode,'{2}')>0", date.ToString("yyyy-MM-dd"), indexName, deptId);
        var result = GlobalClass.CommonMethodSrv.GetData(sqlText).Tables[0].Select().Select(a => new WarningEntity()
        {
            Name = a["projectname"].ToString(),
            Warning = Convert.ToInt32(a["warninglevelname"]),
            Number = Convert.ToDecimal(a["numericalvalue"])
        }).ToList();
        return result;
    }


    void BusinessPlannIcome(HttpContext context)
    {

        string sqlText = string.Format(" select operorgname ,sum(income) income ,sum(planincome)planincome from ("
            + " select operorgname ,numericalvalue income,0 planincome"
            + " from thd_fundmanagement where  createdate=to_date('{0}','yyyy-mm-dd') and timename='本年' and organizationlevel = '分公司' and numericalvalue>0 and indexname ='营业收入'"
            + " union all"
            + " select operorgname ,0 income,numericalvalue planincome"
            + " from thd_fundmanagement where createdate=to_date('{1}','yyyy-mm-dd')  and organizationlevel = '分公司' and numericalvalue>0 and indexname ='计划营业收入') group by operorgname"
            , date.ToString("yyyy-MM-dd"), date.ToString("yyyy-MM-dd").Substring(0, 4) + "-1-1");

        var result = GlobalClass.CommonMethodSrv.GetData(sqlText).Tables[0].Select().Select(a => new
        {
            Name = a["operorgname"].ToString(),
            Income = Convert.ToDecimal(a["income"]),
            PlanIncome = Convert.ToDecimal(a["planincome"])

        }).ToList();

        context.Response.Write(JsonConvert.SerializeObject(result));

    }


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    /// <summary>
    /// 返回预警详情的实体
    /// </summary>
    class WarningEntity
    {
        public string Name { get; set; }
        public int Warning { get; set; }
        public decimal Number { get; set; }
    }
}