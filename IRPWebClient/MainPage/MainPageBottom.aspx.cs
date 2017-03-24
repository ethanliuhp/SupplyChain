using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComponentArt.Web.UI;
using System.Collections;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using System.Data;
using VirtualMachine.Component.Util;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using NHibernate.Criterion;

public partial class MainPage_MainPageBottom : PageBaseClass
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack == false)
        {
            if (Request.QueryString["ProjectId"] != null)
            {
                string projectId = Request.QueryString["ProjectId"];
                string ProjectName = Request.QueryString["ProjectName"];
                string projectSyscode = Request.QueryString["ProjectSyscode"];
                string projectType = Request.QueryString["ProjectType"];

                //lblWarningTitle.InnerText = ProjectName;

                txtOrgName.Value = ProjectName;

                string userCode = "";
                if (Request.QueryString["userCode"] != null)
                {
                    userCode = Request.QueryString["userCode"];
                    List<string> listOrgSyscode = MGWBS.GWBSSrv.GetOperationOrgSyscodeByUser(userCode);
                    var query = from o in listOrgSyscode
                                where projectSyscode.IndexOf(o) > -1
                                select o;

                    if (query.Count() > 0)
                        txtOptFlag.Value = "true";
                    else
                        txtOptFlag.Value = "false";
                }

                InitData(projectId, ProjectName, projectSyscode, projectType);
            }

            if (Request.QueryString["proInfoAuth"] != null && Request.QueryString["proInfoAuth"].Trim() == "1")
            {
                //trProjectBaseInfo.Visible = false;
                //trProjectBaseInfoCostAuth.Visible = true;
                trProjectBaseInfo.Style["display"] = "none";
                trProjectBaseInfoCostAuth.Style["display"] = "block";
                txtBaseinfo.Value = "false";
            }
        }
    }

    private void InitData(string projectId, string projectName, string projectSyscode, string projectType)
    {
        projectType = projectType.ToLower();

        //机构类型 h 总公司；hd 总公司部门；b 分公司；bd 分公司部门；zgxmb 直管项目部；fgsxmb 分公司项目部

        #region 项目预警

        IList listWarningTarget = MGWBS.GWBSSrv.GetProjectWarningTargetInfo(projectId, projectSyscode, projectType);

        Color warningTargetBySafeColor = (Color)listWarningTarget[0];
        Color warningTargetByQualityColor = (Color)listWarningTarget[1];
        Color warningTargetByDurationColor = (Color)listWarningTarget[2];
        Color warningTargetByCostColor = (Color)listWarningTarget[3];

        decimal warningTargetBySafe = decimal.Round((decimal)listWarningTarget[4], 4);
        decimal warningTargetByQuality = decimal.Round((decimal)listWarningTarget[5], 4);
        decimal warningTargetByDuration = decimal.Round((decimal)listWarningTarget[6], 4);
        decimal warningTargetByCostRealProfit = decimal.Round((decimal)listWarningTarget[7], 4);//成本指标：实际/平均实际利润率
        decimal warningTargetByCostResTurnedOver = decimal.Round((decimal)listWarningTarget[8], 4);//成本指标：实际/平均责任上缴比率

        int projectCount = Convert.ToInt32(listWarningTarget[9]);//在建项目总个数
        string projectConstractstage = listWarningTarget[10].ToString();
        bool projectHasData = Convert.ToBoolean(listWarningTarget[11]);
        int readyProjectCount = Convert.ToInt32(listWarningTarget[12]);//准备项目总个数
        int basicProjectCount = Convert.ToInt32(listWarningTarget[13]);//基础项目总个数
        int structProjectCount = Convert.ToInt32(listWarningTarget[14]);//结构项目总个数
        int fitupProjectCount = Convert.ToInt32(listWarningTarget[15]);//装修项目总个数
        int endingProjectCount = Convert.ToInt32(listWarningTarget[16]);//收尾项目总个数

        int precastProjectCount = Convert.ToInt32(listWarningTarget[21]);//21预埋项目总个数
        int installProjectCount = Convert.ToInt32(listWarningTarget[22]);//22安装项目总个数
        int joinDebuggerProjectCount = Convert.ToInt32(listWarningTarget[23]);//23联调项目总个数

        decimal warningTargetByCostSumProfit = decimal.Round(ClientUtil.ToDecimal(listWarningTarget[17]) / 10000, 2);//利润额 = 合同收入-实际成本
        decimal warningTargetByCostProfitRate = decimal.Round((decimal)listWarningTarget[18], 2);//利润率 = 利润额/合同收入
        decimal warningTargetByCostSumLower = decimal.Round((decimal)(ClientUtil.ToDecimal(listWarningTarget[19]) / 10000), 2);//超成本降低额 = 责任成本-实际成本
        decimal warningTargetByCostRateLower = decimal.Round((decimal)listWarningTarget[20], 2);//超成本降低率 = 超成本降低额/责任成本

        Chart2.Series[0].Points[0].Color = warningTargetBySafeColor;//安全
        Chart2.Series[0].Points[1].Color = warningTargetByQualityColor;//质量
        Chart2.Series[0].Points[2].Color = warningTargetByCostColor;//成本
        Chart2.Series[0].Points[3].Color = warningTargetByDurationColor;//工期

        Chart2.Series[0].Points[0].ToolTip = ModelToolTip(1, warningTargetBySafeColor) + "；" + GetWaringDesc2(1, projectType, projectHasData, warningTargetBySafeColor, warningTargetBySafe, 0);//安全
        Chart2.Series[0].Points[1].ToolTip = ModelToolTip(2, warningTargetByQualityColor) + "；" + GetWaringDesc2(2, projectType, projectHasData, warningTargetByQualityColor, warningTargetByQuality, 0);//质量
        if (txtOptFlag.Value == "true")
            Chart2.Series[0].Points[2].ToolTip = ModelToolTip(3, warningTargetByCostColor) + "；" + "利润额：" + warningTargetByCostSumProfit + "万元；利润率：" + warningTargetByCostProfitRate + "%；超成本降低额：" + warningTargetByCostSumLower + "万元；超成本降低率：" + warningTargetByCostRateLower + "%"; //GetWaringDesc2(3, projectType, projectHasData, warningTargetByCostColor, warningTargetByCostRealProfit, warningTargetByCostResTurnedOver);//成本
        Chart2.Series[0].Points[3].ToolTip = ModelToolTip(4, warningTargetByDurationColor) + "；" + GetWaringDesc2(4, projectType, projectHasData, warningTargetByDurationColor, warningTargetByDuration, 0); ;//工期

        //lblSafeDesc.InnerHtml = ModelToolTip(1, warningTargetBySafeColor) + "，" + GetWaringDesc(1, projectType, projectHasData, warningTargetBySafeColor, warningTargetBySafe, 0);
        //lblSafeDesc.Style.Add("color", warningTargetBySafeColor.Name);

        //lblQualityDesc.InnerHtml = ModelToolTip(2, warningTargetByQualityColor) + "，" + GetWaringDesc(2, projectType, projectHasData, warningTargetByQualityColor, warningTargetByQuality, 0);
        //lblQualityDesc.Style.Add("color", warningTargetByQualityColor.Name);

        //lblCostDesc.InnerHtml = ModelToolTip(3, warningTargetByCostColor) + "，" + GetWaringDesc(3, projectType, projectHasData, warningTargetByCostColor, warningTargetByCostRealProfit, warningTargetByCostResTurnedOver);
        //lblCostDesc.Style.Add("color", warningTargetByCostColor.Name);

        //lblDurationDesc.InnerHtml = ModelToolTip(4, warningTargetByDurationColor) + "，" + GetWaringDesc(4, projectType, projectHasData, warningTargetByDurationColor, warningTargetByDuration, 0);
        //lblDurationDesc.Style.Add("color", warningTargetByDurationColor.Name);

        TextAnnotation t = (TextAnnotation)Chart2.Annotations["txtProjectCount"];

        if (projectType == "h" || projectType == "b")
        {
            t.Text = "在建 " + projectCount + "个\n\n准备" + readyProjectCount + "    基础" + basicProjectCount
                + "\n\n预埋" + precastProjectCount + "    结构" + structProjectCount
                + "\n\n安装" + installProjectCount + "    装修" + fitupProjectCount
                + "\n\n联调" + joinDebuggerProjectCount + "    收尾" + endingProjectCount + "";
        }
        else
        {
            int maxStrLen = 11;

            string projectConstractstageStr = "施工阶段：" + projectConstractstage;

            string projectConstractstageStr1 = "";
            if (projectConstractstageStr.Length > maxStrLen)
            {
                projectConstractstageStr1 = projectConstractstageStr.Substring(maxStrLen);
                projectConstractstageStr = projectConstractstageStr.Substring(0, maxStrLen) + "\n\n";
            }

            while (projectConstractstageStr1.Length > maxStrLen)
            {
                projectConstractstageStr += projectConstractstageStr1.Substring(0, maxStrLen) + "\n\n";
                projectConstractstageStr1 = projectConstractstageStr1.Substring(maxStrLen);
            }

            projectConstractstageStr += projectConstractstageStr1;

            t.Text = projectConstractstageStr;
        }
        #endregion

        #region 项目监控
        string sqlOrg = "select t1.opgcode opgcode from resoperationorg t1 where t1.opgid ='" + projectId + "'";
        DataSet dsOrg = MGWBS.GWBSSrv.SearchSQL(sqlOrg);
        if (dsOrg.Tables[0].Rows.Count > 0)
        {
            string opgcode = dsOrg.Tables[0].Rows[0]["opgcode"].ToString();
            LoadPic(opgcode);
        }
        #endregion

        #region 统计指标
        //ComponentArt.Web.UI.TabStripTab tabPage1 = new ComponentArt.Web.UI.TabStripTab();
        //tabPage1.ID = "tabPage1";
        //tabPage1.Text = "月度成本情况分析图";
        //tabPage1.TextAlign = ComponentArt.Web.UI.TextAlign.Center;
        //tabPage1.TextWrap = false;
        //tabPage1.PageViewId = "View_1";

        //ComponentArt.Web.UI.TabStripTab tabPage2 = new ComponentArt.Web.UI.TabStripTab();
        //tabPage2.ID = "tabPage2";
        //tabPage2.Text = "累计成本情况分析图";
        //tabPage2.TextAlign = ComponentArt.Web.UI.TextAlign.Center;
        //tabPage2.TextWrap = false;
        //tabPage2.PageViewId = "View_2";

        //TabPageLink.Tabs.Add(tabPage1);
        //TabPageLink.Tabs.Add(tabPage2);

        //TabPageLink.SelectedTab = tabPage1;

        //PageView view = new PageView();
        //view.BorderColor = Color.Transparent;
        //view.ID = "View_1";
        //string controlPath = "~/UserControls/CostAnalysisFigure.ascx";
        //IPageLink viewControl = (IPageLink)LoadPageControl(controlPath);
        //viewControl.ProjectSyscode = projectSyscode;
        //view.Controls.Add((UserControl)viewControl);

        //PageView view2 = new PageView();
        //view2.ID = "View_2";
        //controlPath = "~/UserControls/ProfitChart.ascx";
        //IPageLink viewControl2 = (IPageLink)LoadPageControl(controlPath);
        //viewControl2.ProjectSyscode = projectSyscode;
        //view2.Controls.Add((UserControl)viewControl2);

        //MPLinkControl.PageViews.Add(view);
        //MPLinkControl.PageViews.Add(view2);

        //if (projectType == "zgxmb" || projectType == "fgsxmb")
        //{
        //    ComponentArt.Web.UI.TabStripTab tabPage3 = new ComponentArt.Web.UI.TabStripTab();
        //    tabPage3.Text = "项目基本信息";
        //    tabPage3.TextAlign = ComponentArt.Web.UI.TextAlign.Center;
        //    tabPage3.TextWrap = false;
        //    tabPage3.PageViewId = "View_3";
        //    TabPageLink.Tabs.Add(tabPage3);

        //    PageView view3 = new PageView();
        //    view3.ID = "View_3";
        //    controlPath = "~/UserControls/ProjectBaseInfo.ascx";
        //    IPageLink viewControl3 = (IPageLink)LoadPageControl(controlPath);
        //    viewControl3.ProjectId = projectId;
        //    view3.Controls.Add((UserControl)viewControl3);
        //    MPLinkControl.PageViews.Add(view3);
        //}

        //MPLinkControl.SelectedIndex = 0;

        #endregion

        BindChartDataByMonthCostAnalysis(projectId, projectSyscode);

        BindChartDataByAddCostAnalysis(projectId, projectSyscode);

        BindProjectBaseInfoData(projectId);
    }


    /// <summary>
    /// 获取预警颜色的预警信息
    /// </summary>
    /// <param name="p">预警类别（1.安全，2.质量，3.成本，4.工期）</param>
    /// <param name="c">预警颜色</param>
    /// <returns></returns>
    private string ModelToolTip(int p, Color c)
    {
        if (p == 1)
        {
            return "安全预警状态：" + (c == Color.Green ? "正常" : c == Color.Yellow ? "一般" : c == Color.Orange ? "重要" : "紧急");
        }
        else if (p == 2)
        {
            return "质量预警状态：" + (c == Color.Green ? "正常" : c == Color.Yellow ? "一般" : c == Color.Orange ? "重要" : "严重");
        }
        else if (p == 3)
        {
            return "成本预警状态：" + (c == Color.Green ? "正常" : c == Color.Yellow ? "相对亏损" : "绝对亏损");
        }
        else
            return "工期预警状态：" + (c == Color.Green ? "正常" : c == Color.Yellow ? "正常延误" : c == Color.Orange ? "一般延误" : "严重延误");
    }
    /// <summary>
    /// //获取预警详细信息 
    /// </summary>
    /// <param name="p">预警类别（1.安全，2.质量，3.成本，4.工期）</param>
    /// <param name="projectType">机构类型（h 总公司；hd 总公司部门；b 分公司；bd 分公司部门；zgxmb 直管项目部；fgsxmb 分公司项目部）</param>
    /// <param name="warnColor">预警颜色</param>
    /// <param name="targetValue">指标值1</param>
    /// <param name="targetValue2">指标值2（成本预警时使用）</param>
    /// <returns></returns>
    private string GetWaringDesc(int p, string projectType, bool projectHasData, Color warnColor, decimal targetValue, decimal targetValue2)
    {
        string desc = "";

        if (p == 1)
        {
            if (projectType == "h" || projectType == "b")
            {
                if (warnColor == Color.Green)
                    desc = "<label title='安全隐患指数＜5%，当前值为:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%'>【判定指标】<label/>";
                else if (warnColor == Color.Yellow)
                    desc = "<label title='5%≤安全隐患指数＜20%，当前值为:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%'>【判定指标】<label/>";
                else if (warnColor == Color.Orange)
                    desc = "<label title='20%≤安全隐患指数＜40%，当前值为:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%'>【判定指标】<label/>";
                else
                    desc = "<label title='40%≤安全隐患指数，当前值为:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%'>【判定指标】<label/>";
            }
            else
            {
                if (warnColor == Color.Green)
                    desc = "<label title='安全隐患指数＜5%，当前值为:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%'>【判定指标】<label/>";
                else if (warnColor == Color.Yellow)
                    desc = "<label title='5%≤安全隐患指数＜15%，当前值为:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%'>【判定指标】<label/>";
                else if (warnColor == Color.Orange)
                    desc = "<label title='15%≤安全隐患指数＜30%，当前值为:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%'>【判定指标】<label/>";
                else
                    desc = "<label title='30%≤安全隐患指数，当前值为:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%'>【判定指标】<label/>";
            }
        }
        else if (p == 2)
        {
            if (projectType == "h" || projectType == "b")
            {
                if (warnColor == Color.Green)
                    desc = "<label title='日常检查不通过指数＜5%，当前值为:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%'>【判定指标】<label/>";
                else if (warnColor == Color.Yellow)
                    desc = "<label title='5%≤日常检查不通过指数＜20%，当前值为:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%'>【判定指标】<label/>";
                else if (warnColor == Color.Orange)
                    desc = "<label title='20%≤日常检查不通过指数＜40%，当前值为:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%'>【判定指标】<label/>";
                else
                    desc = "<label title='40%≤日常检查不通过指数，当前值为:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%'>【判定指标】<label/>";
            }
            else
            {
                if (warnColor == Color.Green)
                    desc = "<label title='日常检查不通过指数＜5%，当前值为:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%'>【判定指标】<label/>";
                else if (warnColor == Color.Yellow)
                    desc = "<label title='5%≤日常检查不通过指数＜15%，当前值为:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%'>【判定指标】<label/>";
                else if (warnColor == Color.Orange)
                    desc = "<label title='15%≤日常检查不通过指数＜30%，当前值为:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%'>【判定指标】<label/>";
                else
                    desc = "<label title='30%≤日常检查不通过指数，当前值为:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%'>【判定指标】<label/>";
            }
        }
        else if (p == 3)
        {
            if (projectHasData)
            {
                if (projectType == "h" || projectType == "b")
                {
                    if (warnColor == Color.Green)
                        desc = "<label title='平均责任上缴比例＜实际平均利润率；当前[实际平均利润率]:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%,[平均责任上缴比例]:" + UtilityClass.DecimalRound(targetValue2 * 100, 2) + "%'>【判定指标】<label/>";
                    else if (warnColor == Color.Yellow)
                        desc = "<label title='0＜实际平均利润率≤平均责任上缴比例；当前[实际平均利润率]:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%,[平均责任上缴比例]:" + UtilityClass.DecimalRound(targetValue2 * 100, 2) + "%'>【判定指标】<label/>";
                    else
                        desc = "<label title='实际平均利润率≤0；当前[实际平均利润率]:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%,[平均责任上缴比例]:" + UtilityClass.DecimalRound(targetValue2 * 100, 2) + "%'>【判定指标】<label/>";
                }
                else
                {
                    if (warnColor == Color.Green)
                        desc = "<label title='责任上缴比例＜实际利润率；当前[实际利润率]:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%,[责任上缴比例]:" + UtilityClass.DecimalRound(targetValue2 * 100, 2) + "%'>【判定指标】<label/>";
                    else if (warnColor == Color.Yellow)
                        desc = "<label title='0＜实际利润率≤责任上缴比例；当前[实际利润率]:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%,[责任上缴比例]:" + UtilityClass.DecimalRound(targetValue2 * 100, 2) + "%'>【判定指标】<label/>";
                    else
                        desc = "<label title='实际利润率≤0；当前[实际利润率]:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%,[责任上缴比例]:" + UtilityClass.DecimalRound(targetValue2 * 100, 2) + "%'>【判定指标】<label/>";
                }
            }
            else
                desc = "【当前机构尚未核算】";
        }
        else if (p == 4)
        {
            if (projectType == "h" || projectType == "b")
            {
                if (warnColor == Color.Green)
                    desc = "<label title='任务工期延误比例＜5%，当前值为:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%'>【判定指标】<label/>";
                else if (warnColor == Color.Yellow)
                    desc = "<label title='5%≤任务工期延误比例＜15%，当前值为:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%'>【判定指标】<label/>";
                else if (warnColor == Color.Orange)
                    desc = "<label title='15%≤任务工期延误比例＜30%，当前值为:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%'>【判定指标】<label/>";
                else
                    desc = "<label title='30%≤任务工期延误比例，当前值为:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%'>【判定指标】<label/>";
            }
            else
            {
                if (warnColor == Color.Green)
                    desc = "<label title='任务工期延误比例＜5%，当前值为:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%'>【判定指标】<label/>";
                else if (warnColor == Color.Yellow)
                    desc = "<label title='5%≤任务工期延误比例＜15%，当前值为:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%'>【判定指标】<label/>";
                else if (warnColor == Color.Orange)
                    desc = "<label title='15%≤任务工期延误比例＜30%，当前值为:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%'>【判定指标】<label/>";
                else
                    desc = "<label title='30%≤任务工期延误比例，当前值为:" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%'>【判定指标】<label/>";
            }
        }


        return desc;
    }

    /// <summary>
    /// //获取预警详细信息 
    /// </summary>
    /// <param name="p">预警类别（1.安全，2.质量，3.成本，4.工期）</param>
    /// <param name="projectType">机构类型（h 总公司；hd 总公司部门；b 分公司；bd 分公司部门；zgxmb 直管项目部；fgsxmb 分公司项目部）</param>
    /// <param name="warnColor">预警颜色</param>
    /// <param name="targetValue">指标值1</param>
    /// <param name="targetValue2">指标值2（成本预警时使用）</param>
    /// <returns></returns>
    private string GetWaringDesc2(int p, string projectType, bool projectHasData, Color warnColor, decimal targetValue, decimal targetValue2)
    {
        string desc = "";

        if (p == 1)
        {
            desc = "安全隐患指数：" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%";
        }
        else if (p == 2)
        {
            desc = "日常检查不通过率：" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%";
        }
        else if (p == 3)
        {
            if (projectHasData)
            {
                desc = "实际利润率：" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%，责任上缴比例：" + UtilityClass.DecimalRound(targetValue2 * 100, 2) + "%";
            }
            else
                desc = "【当前机构尚未核算】";
        }
        else if (p == 4)
        {
            desc = "工期延误比例：" + UtilityClass.DecimalRound(targetValue * 100, 2) + "%";
        }

        return desc;
    }

    private Control LoadPageControl(string controlPath)
    {
        return Page.LoadControl(controlPath);
    }

    /// <summary>
    /// 绑定月度成本分析数据
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="projectSyscode"></param>
    private void BindChartDataByMonthCostAnalysis(string projectId, string projectSyscode)
    {
        ChartMonthCostAnalysis.Series["Series1"].Points.Clear();
        ChartMonthCostAnalysis.Series["Series2"].Points.Clear();
        ChartMonthCostAnalysis.Series["Series3"].Points.Clear();
        ChartMonthCostAnalysis.Series["Series4"].Points.Clear();

        //ChartMonthCostAnalysis.Series["Series1"].LegendText = "合同收入";
        //ChartMonthCostAnalysis.Series["Series2"].LegendText = "责任成本";
        //ChartMonthCostAnalysis.Series["Series3"].LegendText = "实际成本";
        //ChartMonthCostAnalysis.Series["Series4"].LegendText = "实际收款";


        Random random = new Random();
        string[] times = new string[12];//{ "2012/1", "2012/2", "2012/3", "2012/4", "2012/5", "2012/6","2012/7", "2012/8", "2012/9", "2012/10", "2012/11", "2012/12"};
        int index = 11;
        for (DateTime i = DateTime.Now; i > DateTime.Now.AddYears(-1); i = i.AddMonths(-1))
        {
            times[index] = i.Year + "/" + i.Month;
            index -= 1;
        }

        DataTable[] dts = MGWBS.GWBSSrv.GetProjectCostData(DateTime.Now.AddYears(-1), projectSyscode);

        //获取起始和结束数据点来设置只有数据点之间的连线
        int firstContractDataIndex = -1;
        int lastContractDataIndex = -1;
        int firstResponsibleDataIndex = -1;
        int lastResponsibleDataIndex = -1;
        int firstRealDataIndex = -1;
        int lastRealDataIndex = -1;
        int firstCollectionsummoneyDataIndex = -1;
        int lastCollectionsummoneyDataIndex = -1;

        #region 获取起始和结束数据点来设置只有数据点之间的连线
        for (int pointIndex = 0; pointIndex < times.Length; pointIndex++)
        {
            string date = times[pointIndex];
            foreach (DataRow row in dts[0].Rows)
            {
                if (DateTime.Parse(date) == DateTime.Parse(row[0].ToString()))
                {
                    if (ClientUtil.ToDecimal(row[1]) != 0)
                    {
                        if (firstContractDataIndex == -1 || firstContractDataIndex > pointIndex)
                            firstContractDataIndex = pointIndex;

                        if (lastContractDataIndex == -1 || lastContractDataIndex < pointIndex)
                            lastContractDataIndex = pointIndex;
                    }
                    if (ClientUtil.ToDecimal(row[2]) != 0)
                    {
                        if (firstResponsibleDataIndex == -1 || firstResponsibleDataIndex > pointIndex)
                            firstResponsibleDataIndex = pointIndex;

                        if (lastResponsibleDataIndex == -1 || lastResponsibleDataIndex < pointIndex)
                            lastResponsibleDataIndex = pointIndex;
                    }
                    if (ClientUtil.ToDecimal(row[3]) != 0)
                    {
                        if (firstRealDataIndex == -1 || firstRealDataIndex > pointIndex)
                            firstRealDataIndex = pointIndex;

                        if (lastRealDataIndex == -1 || lastRealDataIndex < pointIndex)
                            lastRealDataIndex = pointIndex;
                    }
                    break;
                }
            }
            foreach (DataRow row in dts[1].Rows)
            {
                if (DateTime.Parse(date) == DateTime.Parse(row[0].ToString()))
                {
                    if (ClientUtil.ToDecimal(row[1]) != 0)
                    {
                        if (firstCollectionsummoneyDataIndex == -1 || firstCollectionsummoneyDataIndex > pointIndex)
                            firstCollectionsummoneyDataIndex = pointIndex;

                        if (lastCollectionsummoneyDataIndex == -1 || lastCollectionsummoneyDataIndex < pointIndex)
                            lastCollectionsummoneyDataIndex = pointIndex;
                    }
                    break;
                }
            }
        }
        #endregion

        #region 加载MSChart图形数据

        for (int pointIndex = 0; pointIndex < times.Length; pointIndex++)
        {
            string date = times[pointIndex];
            decimal contractTotalPrice = 0;
            decimal responsibleTotalPrice = 0;
            decimal realTotalPrice = 0;
            decimal realCollectionsummoney = 0;//实际收款合同额

            foreach (DataRow row in dts[0].Rows)
            {
                if (DateTime.Parse(date) == DateTime.Parse(row[0].ToString()))
                {
                    contractTotalPrice = decimal.Round(ClientUtil.ToDecimal(row[1]) / 10000, 2);
                    responsibleTotalPrice = decimal.Round(ClientUtil.ToDecimal(row[2]) / 10000, 2);
                    realTotalPrice = decimal.Round(ClientUtil.ToDecimal(row[3]) / 10000, 2);
                    break;
                }
            }
            foreach (DataRow row in dts[1].Rows)
            {
                if (DateTime.Parse(date) == DateTime.Parse(row[0].ToString()))
                {
                    realCollectionsummoney = decimal.Round(ClientUtil.ToDecimal(row[1]) / 10000, 2);
                    break;
                }
            }

            //Chart1.Series["Series1"].Points.AddXY(date, contractTotalPrice);
            //Chart1.Series["Series2"].Points.AddXY(date, responsibleTotalPrice);
            //Chart1.Series["Series3"].Points.AddXY(date, realTotalPrice);
            //Chart1.Series["Series4"].Points.AddXY(date, realCollectionsummoney);

            //if (DateTime.Parse(date + "-1") == DateTime.Now.Date)
            //    date += "年/月";

            date = date.Substring(2);

            DataPoint dp = new DataPoint();
            dp.AxisLabel = date;
            dp.LegendText = contractTotalPrice.ToString();
            dp.YValues = new double[] { (double)contractTotalPrice };
            if (contractTotalPrice != 0 && pointIndex == lastContractDataIndex)
            {
                dp.IsValueShownAsLabel = true;
            }
            //if (pointIndex <= firstContractDataIndex || pointIndex > lastContractDataIndex)
            if (pointIndex > lastContractDataIndex)
            {
                dp.Color = System.Drawing.Color.Transparent;
            }
            dp.BorderWidth = 2;
            dp.BorderColor = System.Drawing.Color.Red;

            ChartMonthCostAnalysis.Series["Series1"].Points.Add(dp);


            DataPoint dp1 = new DataPoint();
            dp1.AxisLabel = date;
            dp1.LegendText = responsibleTotalPrice.ToString();
            dp1.YValues = new double[] { (double)responsibleTotalPrice };
            if (responsibleTotalPrice != 0 && pointIndex == lastResponsibleDataIndex)
            {
                dp1.IsValueShownAsLabel = true;
            }
            //if (pointIndex <= firstResponsibleDataIndex || pointIndex > lastResponsibleDataIndex)
            if (pointIndex > lastResponsibleDataIndex)
            {
                dp1.Color = System.Drawing.Color.Transparent;
            }
            ChartMonthCostAnalysis.Series["Series2"].Points.Add(dp1);

            DataPoint dp2 = new DataPoint();
            dp2.AxisLabel = date;
            dp2.LegendText = realTotalPrice.ToString();
            dp2.YValues = new double[] { (double)realTotalPrice };
            if (realTotalPrice != 0 && pointIndex == lastRealDataIndex)
            {
                dp2.IsValueShownAsLabel = true;
            }
            //if (pointIndex <= firstRealDataIndex || pointIndex > lastRealDataIndex)
            if (pointIndex > lastRealDataIndex)
            {
                dp2.Color = System.Drawing.Color.Transparent;
            }
            ChartMonthCostAnalysis.Series["Series3"].Points.Add(dp2);

            DataPoint dp3 = new DataPoint();
            dp3.AxisLabel = date;
            dp3.LegendText = realCollectionsummoney.ToString();
            dp3.YValues = new double[] { (double)realCollectionsummoney };
            if (realCollectionsummoney != 0 && pointIndex == lastCollectionsummoneyDataIndex)
            {
                dp3.IsValueShownAsLabel = true;
            }
            //if (pointIndex <= firstCollectionsummoneyDataIndex || pointIndex > lastCollectionsummoneyDataIndex)
            if (pointIndex > lastCollectionsummoneyDataIndex)
            {
                dp3.Color = System.Drawing.Color.Transparent;
            }
            ChartMonthCostAnalysis.Series["Series4"].Points.Add(dp3);
        }
        //Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid..LineColor = System.Drawing.Color.Red;
        //Chart1.Legends[2].ForeColor = System.Drawing.Color.Red;
        #endregion

        #region 加载图形底部说明信息

        List<int> listLastDataIndex = new List<int>();
        listLastDataIndex.Add(lastContractDataIndex);
        listLastDataIndex.Add(lastResponsibleDataIndex);
        listLastDataIndex.Add(lastRealDataIndex);
        listLastDataIndex.Add(lastCollectionsummoneyDataIndex);

        var maxDataIndex = listLastDataIndex.Max(p => p);

        if (maxDataIndex != -1)
        {
            string date = times[maxDataIndex];
            //lblCostInfoTitle.InnerText = date.Replace("/", "年") + "月 成本信息";

            decimal lastContractTotalPrice = 0;
            decimal lastResonsibleTotalPrice = 0;
            decimal lastRealTotalPrice = 0;
            decimal lastColltionsummoneyTotalPrice = 0;

            foreach (DataRow row in dts[0].Rows)
            {
                if (DateTime.Parse(date) == DateTime.Parse(row[0].ToString()))
                {
                    lastContractTotalPrice = decimal.Round(ClientUtil.ToDecimal(row[1]) / 10000, 2);
                    lastResonsibleTotalPrice = decimal.Round(ClientUtil.ToDecimal(row[2]) / 10000, 2);
                    lastRealTotalPrice = decimal.Round(ClientUtil.ToDecimal(row[3]) / 10000, 2);
                    break;
                }
            }
            foreach (DataRow row in dts[1].Rows)
            {
                if (DateTime.Parse(date) == DateTime.Parse(row[0].ToString()))
                {
                    lastColltionsummoneyTotalPrice = decimal.Round(ClientUtil.ToDecimal(row[1]) / 10000, 2);
                    break;
                }
            }

            //TextAnnotation lblContractTotalPrice = (TextAnnotation)((AnnotationGroup)Chart1.Annotations["lblContractTotalPriceGroup"]).Annotations["lblContractTotalPrice"];
            //TextAnnotation lblResonsibleTotalPrice = (TextAnnotation)((AnnotationGroup)Chart1.Annotations["lblResonsibleTotalPrice"]).Annotations["lblResonsibleTotalPrice"];
            //TextAnnotation lblRealTotalPrice = (TextAnnotation)((AnnotationGroup)Chart1.Annotations["lblRealTotalPrice"]).Annotations["lblRealTotalPrice"];
            //TextAnnotation lblRealColltionsummoney = (TextAnnotation)((AnnotationGroup)Chart1.Annotations["lblRealColltionsummoney"]).Annotations["lblRealColltionsummoney"];
            //TextAnnotation lblProfit = (TextAnnotation)((AnnotationGroup)Chart1.Annotations["lblProfit"]).Annotations["lblProfit"];

            //lblContractTotalPrice.Text = "累计合同收入:" + lastContractTotalPrice.ToString() + " 万元";
            //lblResonsibleTotalPrice.Text = "累计责任成本:" + lastResonsibleTotalPrice.ToString() + " 万元";
            //lblRealTotalPrice.Text = "累计实际成本:" + lastRealTotalPrice.ToString() + " 万元";
            //lblRealColltionsummoney.Text = "累计收款:" + lastColltionsummoneyTotalPrice.ToString() + " 万元";

            //if (lastContractTotalPrice != 0)
            //    lblProfit.Text = "利润率:" + UtilityClass.DecimalRound(((lastContractTotalPrice - lastRealTotalPrice) / lastContractTotalPrice) * 100, 2).ToString() + "%";

            lblContractTotalPrice.InnerText = lastContractTotalPrice.ToString();
            lblResonsibleTotalPrice.InnerText = lastResonsibleTotalPrice.ToString();
            lblRealTotalPrice.InnerText = lastRealTotalPrice.ToString();
            lblRealColltionsummoney.InnerText = lastColltionsummoneyTotalPrice.ToString();

            if (lastContractTotalPrice != 0)
                lblProfit.InnerText = UtilityClass.DecimalRound(((lastContractTotalPrice - lastRealTotalPrice) / lastContractTotalPrice) * 100, 2).ToString() + "%";
        }
        #endregion
    }

    /// <summary>
    /// 绑定累计成本分析数据
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="projectSyscode"></param>
    private void BindChartDataByAddCostAnalysis(string projectId, string projectSyscode)
    {
        ChartAddCostAnalysis.Series["Series1"].Points.Clear();
        //ChartAddCostAnalysis.Series["Series2"].Points.Clear();
        ChartAddCostAnalysis.Series["Series3"].Points.Clear();
        ChartAddCostAnalysis.Series["Series4"].Points.Clear();
        ChartAddCostAnalysis.Series["Series5"].Points.Clear();
        //ChartAddCostAnalysis.Series["Series6"].Points.Clear();
        ChartAddCostAnalysis.Series["Series7"].Points.Clear();
        ChartAddCostAnalysis.Series["Series8"].Points.Clear();

        string[] times = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };

        int lastYear = DateTime.Now.Year - 1;
        int thisYear = DateTime.Now.Year;

        ChartAddCostAnalysis.Series["Series1"].LegendText = lastYear + "合同收入";
        //ChartAddCostAnalysis.Series["Series2"].LegendText = lastYear + "累计责任成本";
        ChartAddCostAnalysis.Series["Series3"].LegendText = lastYear + "实际成本";
        ChartAddCostAnalysis.Series["Series4"].LegendText = lastYear + "实际收款";
        ChartAddCostAnalysis.Series["Series5"].LegendText = thisYear + "合同收入";
        //ChartAddCostAnalysis.Series["Series6"].LegendText = thisYear + "累计责任成本";
        ChartAddCostAnalysis.Series["Series7"].LegendText = thisYear + "实际成本";
        ChartAddCostAnalysis.Series["Series8"].LegendText = thisYear + "实际收款";

        DataTable[] dts = MGWBS.GWBSSrv.GetProjectCostDataByYear(DateTime.Parse(lastYear + "-1-1"), projectSyscode);

        //获取起始和结束数据点来设置只有数据点之间的连线
        int firstContractDataIndex = -1;
        int lastContractDataIndex = -1;
        int firstResponsibleDataIndex = -1;
        int lastResponsibleDataIndex = -1;
        int firstRealDataIndex = -1;
        int lastRealDataIndex = -1;
        int firstCollectionsummoneyDataIndex = -1;
        int lastCollectionsummoneyDataIndex = -1;

        int firstContractDataIndex2 = -1;
        int lastContractDataIndex2 = -1;
        int firstResponsibleDataIndex2 = -1;
        int lastResponsibleDataIndex2 = -1;
        int firstRealDataIndex2 = -1;
        int lastRealDataIndex2 = -1;
        int firstCollectionsummoneyDataIndex2 = -1;
        int lastCollectionsummoneyDataIndex2 = -1;

        #region 获取起始和结束数据点来设置只有数据点之间的连线
        for (int pointIndex = 0; pointIndex < times.Length; pointIndex++)
        {
            string date = times[pointIndex];
            foreach (DataRow row in dts[0].Rows)
            {
                if (DateTime.Parse(lastYear + "-" + date) == DateTime.Parse(row[0].ToString()))
                {
                    if (ClientUtil.ToDecimal(row[1]) != 0)
                    {
                        if (firstContractDataIndex == -1 || firstContractDataIndex > pointIndex)
                            firstContractDataIndex = pointIndex;

                        if (lastContractDataIndex == -1 || lastContractDataIndex < pointIndex)
                            lastContractDataIndex = pointIndex;
                    }
                    if (ClientUtil.ToDecimal(row[2]) != 0)
                    {
                        if (firstResponsibleDataIndex == -1 || firstResponsibleDataIndex > pointIndex)
                            firstResponsibleDataIndex = pointIndex;

                        if (lastResponsibleDataIndex == -1 || lastResponsibleDataIndex < pointIndex)
                            lastResponsibleDataIndex = pointIndex;
                    }
                    if (ClientUtil.ToDecimal(row[3]) != 0)
                    {
                        if (firstRealDataIndex == -1 || firstRealDataIndex > pointIndex)
                            firstRealDataIndex = pointIndex;

                        if (lastRealDataIndex == -1 || lastRealDataIndex < pointIndex)
                            lastRealDataIndex = pointIndex;
                    }
                    break;
                }
                if (DateTime.Parse(thisYear + "-" + date) == DateTime.Parse(row[0].ToString()))
                {
                    if (ClientUtil.ToDecimal(row[1]) != 0)
                    {
                        if (firstContractDataIndex2 == -1 || firstContractDataIndex2 > pointIndex)
                            firstContractDataIndex2 = pointIndex;

                        if (lastContractDataIndex2 == -1 || lastContractDataIndex2 < pointIndex)
                            lastContractDataIndex2 = pointIndex;
                    }
                    if (ClientUtil.ToDecimal(row[2]) != 0)
                    {
                        if (firstResponsibleDataIndex2 == -1 || firstResponsibleDataIndex2 > pointIndex)
                            firstResponsibleDataIndex2 = pointIndex;

                        if (lastResponsibleDataIndex2 == -1 || lastResponsibleDataIndex2 < pointIndex)
                            lastResponsibleDataIndex2 = pointIndex;
                    }
                    if (ClientUtil.ToDecimal(row[3]) != 0)
                    {
                        if (firstRealDataIndex2 == -1 || firstRealDataIndex2 > pointIndex)
                            firstRealDataIndex2 = pointIndex;

                        if (lastRealDataIndex2 == -1 || lastRealDataIndex2 < pointIndex)
                            lastRealDataIndex2 = pointIndex;
                    }
                    break;
                }
            }
            foreach (DataRow row in dts[1].Rows)
            {
                if (DateTime.Parse(lastYear + "-" + date) == DateTime.Parse(row[0].ToString()))
                {
                    if (ClientUtil.ToDecimal(row[1]) != 0)
                    {
                        if (firstCollectionsummoneyDataIndex == -1 || firstCollectionsummoneyDataIndex > pointIndex)
                            firstCollectionsummoneyDataIndex = pointIndex;

                        if (lastCollectionsummoneyDataIndex == -1 || lastCollectionsummoneyDataIndex < pointIndex)
                            lastCollectionsummoneyDataIndex = pointIndex;
                    }
                    break;
                }
                if (DateTime.Parse(thisYear + "-" + date) == DateTime.Parse(row[0].ToString()))
                {
                    if (ClientUtil.ToDecimal(row[1]) != 0)
                    {
                        if (firstCollectionsummoneyDataIndex2 == -1 || firstCollectionsummoneyDataIndex2 > pointIndex)
                            firstCollectionsummoneyDataIndex2 = pointIndex;

                        if (lastCollectionsummoneyDataIndex2 == -1 || lastCollectionsummoneyDataIndex2 < pointIndex)
                            lastCollectionsummoneyDataIndex2 = pointIndex;
                    }
                    break;
                }
            }
        }
        #endregion

        #region 加载MSChart图形数据

        for (int pointIndex = 0; pointIndex < times.Length; pointIndex++)
        {
            string date = times[pointIndex];

            decimal contractTotalPriceLastYear = 0;
            decimal responsibleTotalPriceLastYear = 0;
            decimal realTotalPriceLastYear = 0;
            decimal realCollectionsummoneyLastYear = 0;//实际收款合同额

            decimal contractTotalPriceThisYear = 0;
            decimal responsibleTotalPriceThisYear = 0;
            decimal realTotalPriceThisYear = 0;
            decimal realCollectionsummoneyThisYear = 0;//实际收款合同额

            foreach (DataRow row in dts[0].Rows)
            {
                if (DateTime.Parse(lastYear + "-" + date) == DateTime.Parse(row[0].ToString()))
                {
                    contractTotalPriceLastYear = decimal.Round(ClientUtil.ToDecimal(row[1]) / 10000, 2);
                    responsibleTotalPriceLastYear = decimal.Round(ClientUtil.ToDecimal(row[2]) / 10000, 2);
                    realTotalPriceLastYear = decimal.Round(ClientUtil.ToDecimal(row[3]) / 10000, 2);
                }
                if (DateTime.Parse(thisYear + "-" + date) == DateTime.Parse(row[0].ToString()))
                {
                    contractTotalPriceThisYear = decimal.Round(ClientUtil.ToDecimal(row[1]) / 10000, 2);
                    responsibleTotalPriceThisYear = decimal.Round(ClientUtil.ToDecimal(row[2]) / 10000, 2);
                    realTotalPriceThisYear = decimal.Round(ClientUtil.ToDecimal(row[3]) / 10000, 2);
                }
            }
            foreach (DataRow row in dts[1].Rows)
            {
                if (DateTime.Parse(lastYear + "-" + date) == DateTime.Parse(row[0].ToString()))
                {
                    realCollectionsummoneyLastYear = decimal.Round(ClientUtil.ToDecimal(row[1]) / 10000, 2);
                }
                if (DateTime.Parse(thisYear + "-" + date) == DateTime.Parse(row[0].ToString()))
                {
                    realCollectionsummoneyThisYear = decimal.Round(ClientUtil.ToDecimal(row[1]) / 10000, 2);
                }
            }

            date += "月";

            #region 上一年指标

            DataPoint dp = new DataPoint();
            dp.AxisLabel = date;
            dp.LegendText = contractTotalPriceLastYear.ToString();
            dp.YValues = new double[] { (double)contractTotalPriceLastYear };
            if (contractTotalPriceLastYear != 0 && pointIndex == lastContractDataIndex)
            {
                dp.IsValueShownAsLabel = true;
            }
            //if (pointIndex <= firstContractDataIndex || pointIndex > lastContractDataIndex)
            if (pointIndex > lastContractDataIndex)
            {
                dp.Color = System.Drawing.Color.Transparent;
            }
            ChartAddCostAnalysis.Series["Series1"].Points.Add(dp);

            //DataPoint dp1 = new DataPoint();
            //dp1.AxisLabel = date;
            //dp1.LegendText = responsibleTotalPriceLastYear.ToString();
            //dp1.YValues = new double[] { (double)responsibleTotalPriceLastYear };
            //if (responsibleTotalPriceLastYear != 0 && pointIndex == lastResponsibleDataIndex)
            //{
            //    dp1.IsValueShownAsLabel = true;
            //}
            //if (pointIndex <= firstResponsibleDataIndex || pointIndex > lastResponsibleDataIndex)
            //{
            //    dp1.Color = System.Drawing.Color.Transparent;
            //}
            //Chart1.Series["Series2"].Points.Add(dp1);

            DataPoint dp2 = new DataPoint();
            dp2.AxisLabel = date;
            dp2.LegendText = realTotalPriceLastYear.ToString();
            dp2.YValues = new double[] { (double)realTotalPriceLastYear };
            if (realTotalPriceLastYear != 0 && pointIndex == lastRealDataIndex)
            {
                dp2.IsValueShownAsLabel = true;
            }
            //if (pointIndex <= firstRealDataIndex || pointIndex > lastRealDataIndex)
            if (pointIndex > lastRealDataIndex)
            {
                dp2.Color = System.Drawing.Color.Transparent;
            }
            ChartAddCostAnalysis.Series["Series3"].Points.Add(dp2);

            DataPoint dp3 = new DataPoint();
            dp3.AxisLabel = date;
            dp3.LegendText = realCollectionsummoneyLastYear.ToString();
            dp3.YValues = new double[] { (double)realCollectionsummoneyLastYear };
            if (realCollectionsummoneyLastYear != 0 && pointIndex == lastCollectionsummoneyDataIndex)
            {
                dp3.IsValueShownAsLabel = true;
            }
            //if (pointIndex <= firstCollectionsummoneyDataIndex || pointIndex > lastCollectionsummoneyDataIndex)
            if (pointIndex > lastCollectionsummoneyDataIndex)
            {
                dp3.Color = System.Drawing.Color.Transparent;
            }
            ChartAddCostAnalysis.Series["Series4"].Points.Add(dp3);

            #endregion

            #region 今年指标

            DataPoint dp4 = new DataPoint();
            dp4.AxisLabel = date;
            dp4.LegendText = contractTotalPriceThisYear.ToString();
            dp4.YValues = new double[] { (double)contractTotalPriceThisYear };
            if (contractTotalPriceThisYear != 0 && pointIndex == lastContractDataIndex2)
            {
                dp4.IsValueShownAsLabel = true;
            }
            //if (pointIndex <= firstContractDataIndex2 || pointIndex > lastContractDataIndex2)
            if (pointIndex > lastContractDataIndex2)
            {
                dp4.Color = System.Drawing.Color.Transparent;
            }
            ChartAddCostAnalysis.Series["Series5"].Points.Add(dp4);

            //DataPoint dp5 = new DataPoint();
            //dp5.AxisLabel = date;
            //dp5.LegendText = responsibleTotalPriceThisYear.ToString();
            //dp5.YValues = new double[] { (double)responsibleTotalPriceThisYear };
            //if (responsibleTotalPriceThisYear != 0 && pointIndex == lastResponsibleDataIndex2)
            //{
            //    dp5.IsValueShownAsLabel = true;
            //}
            //if (pointIndex <= firstResponsibleDataIndex2 || pointIndex > lastResponsibleDataIndex2)
            //{
            //    dp5.Color = System.Drawing.Color.Transparent;
            //}
            //ChartAddCostAnalysis.Series["Series6"].Points.Add(dp5);

            DataPoint dp6 = new DataPoint();
            dp6.AxisLabel = date;
            dp6.LegendText = realTotalPriceThisYear.ToString();
            dp6.YValues = new double[] { (double)realTotalPriceThisYear };
            if (realTotalPriceThisYear != 0 && pointIndex == lastRealDataIndex2)
            {
                dp6.IsValueShownAsLabel = true;
            }
            //if (pointIndex <= firstRealDataIndex2 || pointIndex > lastRealDataIndex2)
            if (pointIndex > lastRealDataIndex2)
            {
                dp6.Color = System.Drawing.Color.Transparent;
            }
            ChartAddCostAnalysis.Series["Series7"].Points.Add(dp6);

            DataPoint dp7 = new DataPoint();
            dp7.AxisLabel = date;
            dp7.LegendText = realCollectionsummoneyThisYear.ToString();
            dp7.YValues = new double[] { (double)realCollectionsummoneyThisYear };
            if (realCollectionsummoneyThisYear != 0 && pointIndex == lastCollectionsummoneyDataIndex2)
            {
                dp7.IsValueShownAsLabel = true;
            }
            //if (pointIndex <= firstCollectionsummoneyDataIndex2 || pointIndex > lastCollectionsummoneyDataIndex2)
            if (pointIndex > lastCollectionsummoneyDataIndex2)
            {
                dp7.Color = System.Drawing.Color.Transparent;
            }
            ChartAddCostAnalysis.Series["Series8"].Points.Add(dp7);

            #endregion

        }

        #endregion

        #region 加载图形底部说明信息

        //List<int> listLastDataIndex = new List<int>();
        //listLastDataIndex.Add(lastContractDataIndex);
        //listLastDataIndex.Add(lastResponsibleDataIndex);
        //listLastDataIndex.Add(lastRealDataIndex);
        //listLastDataIndex.Add(lastCollectionsummoneyDataIndex);

        //var maxDataIndex = listLastDataIndex.Max(p => p);

        //if (maxDataIndex != -1)
        //{
        //    string date = times[maxDataIndex];
        //    lblCostInfoTitle.InnerText = lastYear + "年" + date + "月 成本信息";

        //    decimal lastContractTotalPrice = 0;
        //    decimal lastResonsibleTotalPrice = 0;
        //    decimal lastRealTotalPrice = 0;
        //    decimal lastColltionsummoneyTotalPrice = 0;

        //    foreach (DataRow row in dts[0].Rows)
        //    {
        //        if (DateTime.Parse(lastYear + "-" + date) == DateTime.Parse(row[0].ToString()))
        //        {
        //            lastContractTotalPrice = decimal.Round(ClientUtil.ToDecimal(row[1]) / 10000, 2);
        //            lastResonsibleTotalPrice = decimal.Round(ClientUtil.ToDecimal(row[2]) / 10000, 2);
        //            lastRealTotalPrice = decimal.Round(ClientUtil.ToDecimal(row[3]) / 10000, 2);
        //            break;
        //        }
        //    }
        //    foreach (DataRow row in dts[1].Rows)
        //    {
        //        if (DateTime.Parse(lastYear + "-" + date) == DateTime.Parse(row[0].ToString()))
        //        {
        //            lastColltionsummoneyTotalPrice = decimal.Round(ClientUtil.ToDecimal(row[1]) / 10000, 2);
        //            break;
        //        }
        //    }

        //    lblContractTotalPrice.InnerText = lastContractTotalPrice.ToString();
        //    lblResonsibleTotalPrice.InnerText = lastResonsibleTotalPrice.ToString();
        //    lblRealTotalPrice.InnerText = lastRealTotalPrice.ToString();
        //    lblRealColltionsummoney.InnerText = lastColltionsummoneyTotalPrice.ToString();

        //    if (lastContractTotalPrice != 0)
        //        lblProfit.InnerText = UtilityClass.DecimalRound(((lastContractTotalPrice - lastRealTotalPrice) / lastContractTotalPrice) * 100, 2).ToString() + "%";
        //}
        //else
        //    lblCostInfoTitle.InnerText = lastYear + "年--月 成本信息";

        //listLastDataIndex.Clear();
        //listLastDataIndex.Add(lastContractDataIndex2);
        //listLastDataIndex.Add(lastResponsibleDataIndex2);
        //listLastDataIndex.Add(lastRealDataIndex2);
        //listLastDataIndex.Add(lastCollectionsummoneyDataIndex2);

        //maxDataIndex = listLastDataIndex.Max(p => p);

        //decimal lastContractTotalPrice2 = 0;
        //decimal lastResonsibleTotalPrice2 = 0;
        //decimal lastRealTotalPrice2 = 0;
        //decimal lastColltionsummoneyTotalPrice2 = 0;
        //if (maxDataIndex != -1)
        //{
        //    string date = times[maxDataIndex];
        //    lblCostInfoTitle2.InnerText = thisYear + "年" + date + "月 成本信息";

        //    foreach (DataRow row in dts[0].Rows)
        //    {
        //        if (DateTime.Parse(thisYear + "-" + date) == DateTime.Parse(row[0].ToString()))
        //        {
        //            lastContractTotalPrice2 = decimal.Round(ClientUtil.ToDecimal(row[1]) / 10000, 2);
        //            lastResonsibleTotalPrice2 = decimal.Round(ClientUtil.ToDecimal(row[2]) / 10000, 2);
        //            lastRealTotalPrice2 = decimal.Round(ClientUtil.ToDecimal(row[3]) / 10000, 2);
        //            break;
        //        }
        //    }
        //    foreach (DataRow row in dts[1].Rows)
        //    {
        //        if (DateTime.Parse(thisYear + "-" + date) == DateTime.Parse(row[0].ToString()))
        //        {
        //            lastColltionsummoneyTotalPrice2 = decimal.Round(ClientUtil.ToDecimal(row[1]) / 10000, 2);
        //            break;
        //        }
        //    }

        //    lblContractTotalPrice2.InnerText = lastContractTotalPrice2.ToString();
        //    lblResonsibleTotalPrice2.InnerText = lastResonsibleTotalPrice2.ToString();
        //    lblRealTotalPrice2.InnerText = lastRealTotalPrice2.ToString();
        //    lblRealColltionsummoney2.InnerText = lastColltionsummoneyTotalPrice2.ToString();

        //    if (lastContractTotalPrice2 != 0)
        //        lblProfit2.InnerText = UtilityClass.DecimalRound(((lastContractTotalPrice2 - lastRealTotalPrice2) / lastContractTotalPrice2) * 100, 2).ToString() + "%";
        //}
        //else
        //    lblCostInfoTitle2.InnerText = thisYear + "年--月 成本信息";

        #endregion

        //利润分析图
        //DataSet ds = MGWBS.GWBSSrv.GetProjectProfitData(DateTime.Now.AddYears(-1), ProjectSyscode);

        //for (int pointIndex = 0; pointIndex < times.Length; pointIndex++)
        //{
        //    string date = times[pointIndex];
        //    decimal profitTotalPrice = 0;

        //    foreach (DataRow row in ds.Tables[0].Rows)
        //    {
        //        if (DateTime.Parse(date) == DateTime.Parse(row[0].ToString()))
        //        {
        //            profitTotalPrice = decimal.Round(ClientUtil.ToDecimal(row[1]) / 10000, 2);
        //            break;
        //        }
        //    }

        //    Chart1.Series["Series1"].Points.AddXY(date, profitTotalPrice);
        //}
    }

    /// <summary>
    /// 绑定项目基本信息
    /// </summary>
    private void BindProjectBaseInfoData(string projectId)
    {
        ObjectQuery oq = new ObjectQuery();
        oq.AddCriterion(Expression.Eq("OwnerOrg.Id", projectId));
        IList listProject = MGWBS.GWBSSrv.ObjectQuery(typeof(CurrentProjectInfo), oq);
        CurrentProjectInfo projectInfo = null;
        if (listProject.Count > 0)
        {
            projectInfo = listProject[0] as CurrentProjectInfo;

            LoadProject(projectInfo);

            LoasdOrder(projectInfo);
        }
    }

    //工程简介
    void LoadProject(CurrentProjectInfo projectInfo)
    {
        txtProjectName.InnerText = projectInfo.Name;
        txtProName.InnerText = projectInfo.ManagerDepart;
        //txtCode.InnerText = ProjectInfo.Code;
        //txtProjectType.InnerText = projectInfo.ProjectType;//工程类型
        //txtContractWay.InnerText = projectInfo.ContractType;//承包方式
        txtProvince.InnerText = (string.IsNullOrEmpty(projectInfo.ProjectLocationProvince) ? "" : projectInfo.ProjectLocationProvince + "省")
            + (string.IsNullOrEmpty(projectInfo.ProjectLocationCity) ? "" : projectInfo.ProjectLocationCity + "市")
            + projectInfo.ProjectLocationDescript;
        txtContracte.InnerText = projectInfo.ContractArea; //承包范围

        //txtStructType.InnerText = projectInfo.StructureType;//结构类型
        txtBace.InnerText = projectInfo.BaseForm;//基础形式
        txtLifeCycleState.InnerText = EnumUtil<EnumProjectLifeCycle>.GetDescription(projectInfo.ProjectLifeCycle);//生命周期状态、施工阶段
        if (projectInfo.BeginDate > Convert.ToDateTime("2000-01-01"))
        {
            txtCreateDate.InnerText = projectInfo.BeginDate.ToShortDateString();//开工日期
        }
    }
    //合同摘要
    void LoasdOrder(CurrentProjectInfo projectInfo)
    {
        //if (projectInfo.BeginDate > ClientUtil.ToDateTime("2000-01-01"))
        //{
        //    txtStartDate.Value = projectInfo.BeginDate;
        //}
        //if (projectInfo.EndDate > ClientUtil.ToDateTime("2000-01-01"))
        //{
        //    txtCompleteDate.Value = projectInfo.EndDate;
        //}
        //txtQuanlityTarget.InnerText = projectInfo.QuanlityTarget;//质量目标
        //txtQualityReword.InnerText = projectInfo.QualityReword;//质量奖惩
        //txtSaftyTarget.InnerText = projectInfo.SaftyTarget;//安全目标
        //txtSaftyReword.InnerText = projectInfo.SaftyReword;//安全奖惩
        //txtProReword.InnerText = projectInfo.ProjecRewordt;//工期奖惩

        txtMoneySource.InnerText = EnumUtil<EnumSourcesOfFunding>.GetDescription(projectInfo.SourcesOfFunding);//资金来源
        //txtMoneyStates.SelectedIndex = projectInfo.IsFundsAvailabed;//资金到位情况
        txtProjectCost.InnerText = UtilityClass.DecimalRound(projectInfo.ProjectCost / 10000, 4);//工程造价
        //txtRealPreMoney.InnerText = UtilityClass.DecimalRound(projectInfo.RealPerMoney / 10000, 4);//实际预算总金额
        txtConstractMoney.InnerText = UtilityClass.DecimalRound(projectInfo.CivilContractMoney / 10000, 4);//土建合同金额
        txtInstallOrderMoney.InnerText = UtilityClass.DecimalRound(projectInfo.InstallOrderMoney / 10000, 4);//安装合同金额
        txtCollectProport.InnerText = UtilityClass.DecimalRound(projectInfo.ContractCollectRatio, 4);//合同收款比例
        //txtTurnProport.InnerText = UtilityClass.DecimalRound(projectInfo.ResProportion, 4);//责任上缴比例
        //txtGroundPrice.InnerText = UtilityClass.DecimalRound(projectInfo.BigModualGroundUpPrice, 4);
        //txtUnderPrice.InnerText = UtilityClass.DecimalRound(projectInfo.BigModualGroundDownPrice, 4);
        txtExplain.InnerText = projectInfo.Descript;//备注信息（项目说明）
    }


    private void LoadPic(string projectCode)
    {
        //projectCode = "10100100051";
        string param = "Depart=" + projectCode;
        string str = UtilityClass.SendRequest("BIMServerJsonApi", param);
        if (str != null && str.Length > 2)
        {
            str = str.Substring(1, str.Length - 2);
            string[] picList = str.Split(',');
            string path = picList[picList.Count() - 1];
            path = path.Substring(1, path.Length - 2);
            imgPic.Visible = true;
            imgPic.ImageUrl = path;
            imgPicBig.ImageUrl = path;
            btnPicUrl.Text = projectCode;
        }
        else
        {
            imgPic.Visible = false;
        }
    }

    protected void btnPicUrl_Click(object sender, EventArgs e)
    {
        string url = "http://www.cscec3b.com:166/Cameral/Project.aspx?Depart=" + btnPicUrl.Text;

        //string url = "http://www.cscec3b.com:166/Cameral/Depart.aspx?Depart=" + btnPicUrl.Text;

        //Response.Write("<script language='javascript'>window.open('" + url + "');</script>");
        //Response.Redirect(url, false);
        Response.Write(" <script> parent.parent.window.location.href= '" + url + "' </script> ");
    }
}
