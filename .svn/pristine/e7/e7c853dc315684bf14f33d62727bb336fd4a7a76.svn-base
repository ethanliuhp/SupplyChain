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

public partial class MainPage_MainPageBottomBak : System.Web.UI.Page
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

                InitData(projectId, ProjectName, projectSyscode, projectType);
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

        Chart2.Series[0].Points[0].Color = warningTargetBySafeColor;//安全
        Chart2.Series[0].Points[1].Color = warningTargetByQualityColor;//质量
        Chart2.Series[0].Points[2].Color = warningTargetByCostColor;//成本
        Chart2.Series[0].Points[3].Color = warningTargetByDurationColor;//工期

        //Chart2.Series[0].Points[0].ToolTip = ModelToolTip(1, warningTargetBySafeColor);//安全
        //Chart2.Series[0].Points[1].ToolTip = ModelToolTip(2, warningTargetByQualityColor);//质量
        //Chart2.Series[0].Points[2].ToolTip = ModelToolTip(3, warningTargetByCostColor);//成本
        //Chart2.Series[0].Points[3].ToolTip = ModelToolTip(4, warningTargetByDurationColor);//工期

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
            t.Text = "在建 " + projectCount + "个\n\n准备" + readyProjectCount + "    基础" + basicProjectCount + "\n\n结构" + structProjectCount + "    装修" + fitupProjectCount + "\n\n收尾" + endingProjectCount + "";
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

        #region 统计指标
        ComponentArt.Web.UI.TabStripTab tabPage1 = new ComponentArt.Web.UI.TabStripTab();
        tabPage1.ID = "tabPage1";
        tabPage1.Text = "月度成本情况分析图";
        tabPage1.TextAlign = ComponentArt.Web.UI.TextAlign.Center;
        tabPage1.TextWrap = false;
        tabPage1.PageViewId = "View_1";

        ComponentArt.Web.UI.TabStripTab tabPage2 = new ComponentArt.Web.UI.TabStripTab();
        tabPage2.ID = "tabPage2";
        tabPage2.Text = "累计成本情况分析图";
        tabPage2.TextAlign = ComponentArt.Web.UI.TextAlign.Center;
        tabPage2.TextWrap = false;
        tabPage2.PageViewId = "View_2";

        TabPageLink.Tabs.Add(tabPage1);
        TabPageLink.Tabs.Add(tabPage2);

        TabPageLink.SelectedTab = tabPage1;

        PageView view = new PageView();
        view.BorderColor = Color.Transparent;
        view.ID = "View_1";
        string controlPath = "~/UserControls/CostAnalysisFigure.ascx";
        IPageLink viewControl = (IPageLink)LoadPageControl(controlPath);
        viewControl.ProjectSyscode = projectSyscode;
        view.Controls.Add((UserControl)viewControl);

        PageView view2 = new PageView();
        view2.ID = "View_2";
        controlPath = "~/UserControls/ProfitChart.ascx";
        IPageLink viewControl2 = (IPageLink)LoadPageControl(controlPath);
        viewControl2.ProjectSyscode = projectSyscode;
        view2.Controls.Add((UserControl)viewControl2);

        MPLinkControl.PageViews.Add(view);
        MPLinkControl.PageViews.Add(view2);

        if (projectType == "zgxmb" || projectType == "fgsxmb")
        {
            ComponentArt.Web.UI.TabStripTab tabPage3 = new ComponentArt.Web.UI.TabStripTab();
            tabPage3.Text = "项目基本信息";
            tabPage3.TextAlign = ComponentArt.Web.UI.TextAlign.Center;
            tabPage3.TextWrap = false;
            tabPage3.PageViewId = "View_3";
            TabPageLink.Tabs.Add(tabPage3);

            PageView view3 = new PageView();
            view3.ID = "View_3";
            controlPath = "~/UserControls/ProjectBaseInfo.ascx";
            IPageLink viewControl3 = (IPageLink)LoadPageControl(controlPath);
            viewControl3.ProjectId = projectId;
            view3.Controls.Add((UserControl)viewControl3);
            MPLinkControl.PageViews.Add(view3);
        }

        MPLinkControl.SelectedIndex = 0;

        #endregion

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
            return "安全：" + (c == Color.Green ? "正常" : c == Color.Yellow ? "一般" : c == Color.Orange ? "重要" : "紧急");
        }
        else if (p == 2)
        {
            return "质量：" + (c == Color.Green ? "正常" : c == Color.Yellow ? "一般" : c == Color.Orange ? "重要" : "严重");
        }
        else if (p == 3)
        {
            return "成本：" + (c == Color.Green ? "正常" : c == Color.Yellow ? "相对亏损" : "绝对亏损");
        }
        else
            return "工期：" + (c == Color.Green ? "正常" : c == Color.Yellow ? "正常延误" : c == Color.Orange ? "一般延误" : "严重延误");
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

    private Control LoadPageControl(string controlPath)
    {
        return Page.LoadControl(controlPath);
    }
}
