using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Drawing;
using VirtualMachine.Component.Util;

public partial class MainPage_ProjectWarnQuery : PageBaseClass
{
    Dictionary<CurrentProjectInfo, IList> list = new Dictionary<CurrentProjectInfo, IList>();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gridProject.DataSource = null;
            gridProject.DataBind();
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        ObjectQuery oq = new ObjectQuery();
        string projectName = txtProjectName.Value.Trim();
        string projectType = selProjectType.Value;
        string projectInfoState = selProjectState.Value;
        string projectStage = selProjectStage.Value;
        if (projectName == "" && projectType == "" && projectInfoState == "" && projectStage == "")
        {
            MessageBox("请至少输入一个查询条件！");
            txtProjectName.Focus();
            return;
        }

        if (!string.IsNullOrEmpty(projectName))
        {
            oq.AddCriterion(Expression.Like("Name", projectName, MatchMode.Anywhere));
        }
        
        if (!string.IsNullOrEmpty(projectType))
        {
            oq.AddCriterion(Expression.Eq("ProjectType", (int)VirtualMachine.Component.Util.EnumUtil<EnumProjectType>.FromDescription(projectType)));
        }
        
        if (!string.IsNullOrEmpty(projectInfoState))
        {
            oq.AddCriterion(Expression.Eq("ProjectInfoState", VirtualMachine.Component.Util.EnumUtil<EnumProjectInfoState>.FromDescription(projectInfoState)));
        }
        
        if (!string.IsNullOrEmpty(projectStage))
        {
            oq.AddCriterion(Expression.Eq("ProjectLifeCycle", VirtualMachine.Component.Util.EnumUtil<EnumProjectLifeCycle>.FromDescription(projectStage)));
        }

        list = MGWBS.GWBSSrv.QueryProjectWarnInfo(oq);
        List<CurrentProjectInfo> listProject = new List<CurrentProjectInfo>();
        foreach (CurrentProjectInfo p in list.Keys)
        {
            listProject.Add(p);
        }
        gridProject.DataSource = listProject;
        gridProject.DataBind();
    }
    protected void gridProject_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();

            CurrentProjectInfo project = e.Row.DataItem as CurrentProjectInfo;
            IList listWarningTarget = list[project];

            Color warningTargetBySafeColor = (Color)listWarningTarget[0];
            Color warningTargetByQualityColor = (Color)listWarningTarget[1];
            Color warningTargetByDurationColor = (Color)listWarningTarget[2];
            Color warningTargetByCostColor = (Color)listWarningTarget[3];

            bool projectHasData = Convert.ToBoolean(listWarningTarget[11]);

            decimal warningTargetBySafe = decimal.Round((decimal)listWarningTarget[4], 4);
            decimal warningTargetByQuality = decimal.Round((decimal)listWarningTarget[5], 4);
            decimal warningTargetByDuration = decimal.Round((decimal)listWarningTarget[6], 4);
            decimal warningTargetByCostRealProfit = decimal.Round((decimal)listWarningTarget[7], 4);//成本指标：实际/平均实际利润率
            decimal warningTargetByCostResTurnedOver = decimal.Round((decimal)listWarningTarget[8], 4);//成本指标：实际/平均责任上缴比率

            decimal warningTargetByCostSumProfit = decimal.Round(ClientUtil.ToDecimal(listWarningTarget[17]) / 10000, 2);//利润额 = 合同收入-实际成本
            decimal warningTargetByCostProfitRate = decimal.Round((decimal)listWarningTarget[18], 2);//利润率 = 利润额/合同收入
            decimal warningTargetByCostSumLower = decimal.Round((decimal)(ClientUtil.ToDecimal(listWarningTarget[19]) / 10000), 2);//超成本降低额 = 责任成本-实际成本
            decimal warningTargetByCostRateLower = decimal.Round((decimal)listWarningTarget[20], 2);//超成本降低率 = 超成本降低额/责任成本

            e.Row.Cells[6].BackColor = warningTargetByCostColor;
            e.Row.Cells[6].ToolTip = ModelToolTip(3, warningTargetByCostColor) + "；" + "利润额：" + warningTargetByCostSumProfit + "万元；利润率："
                    + warningTargetByCostProfitRate + "%；超成本降低额：" + warningTargetByCostSumLower + "万元；超成本降低率：" + warningTargetByCostRateLower + "%"; //成本

            e.Row.Cells[7].BackColor = warningTargetByDurationColor;
            e.Row.Cells[7].ToolTip = ModelToolTip(4, warningTargetByDurationColor) + "；" + GetWaringDesc2(4, project.OwnerOrg.OperationType, projectHasData, warningTargetByDurationColor, warningTargetByDuration, 0); ;//工期

            e.Row.Cells[8].BackColor = warningTargetByQualityColor;
            e.Row.Cells[8].ToolTip = ModelToolTip(2, warningTargetByQualityColor) + "；" + GetWaringDesc2(2, project.OwnerOrg.OperationType, projectHasData, warningTargetByQualityColor, warningTargetByQuality, 0);//质量

            e.Row.Cells[9].BackColor = warningTargetBySafeColor;
            e.Row.Cells[9].ToolTip = ModelToolTip(1, warningTargetBySafeColor) + "；" + GetWaringDesc2(1, project.OwnerOrg.OperationType, projectHasData, warningTargetBySafeColor, warningTargetBySafe, 0);//安全

        }
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
}
