using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.Collections;

public partial class MainPage_MainPageTop : PageBaseClass
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack == false)
        {
            Chart2.Series[0].Points[0].Color = System.Drawing.Color.Green;
            Chart2.Series[0].Points[1].Color = System.Drawing.Color.Green;
            Chart2.Series[0].Points[2].Color = System.Drawing.Color.Green;
            Chart2.Series[0].Points[3].Color = System.Drawing.Color.Green;

            if (Request.QueryString["ProjectId"] != null)
            {
                string projectId = Request.QueryString["ProjectId"];
                string ProjectName = Request.QueryString["ProjectName"];
                string projectSyscode = Request.QueryString["ProjectSyscode"];
                string projectType = Request.QueryString["ProjectType"];

                InitData(projectId, ProjectName, projectSyscode, projectType);
            }

            #region 饼图

            //BindChart2Data();

            #endregion
        }
    }
    private void InitData(string projectId, string projectName, string projectSyscode, string projectType)
    {
        projectType = projectType.ToLower();

        //机构类型 h 总公司；hd 总公司部门；b 分公司；bd 分公司部门；zgxmb 直管项目部；fgsxmb 分公司项目部

        #region 安全预警

        IList listWarningTarget = MGWBS.GWBSSrv.GetProjectWarningTargetInfo(projectId, projectSyscode, projectType);

        Color warningTargetBySafeColor = (Color)listWarningTarget[0];
        Color warningTargetByQualityColor = (Color)listWarningTarget[1];
        Color warningTargetByDurationColor = (Color)listWarningTarget[2];
        Color warningTargetByCostColor = (Color)listWarningTarget[3];

        int projectCount = Convert.ToInt32(listWarningTarget[4]);
        string projectConstractstage = listWarningTarget[5].ToString();


        Chart2.Series[0].Points[0].Color = warningTargetBySafeColor;//安全
        Chart2.Series[0].Points[1].Color = warningTargetByQualityColor;//质量
        Chart2.Series[0].Points[2].Color = warningTargetByCostColor;//成本
        Chart2.Series[0].Points[3].Color = warningTargetByDurationColor;//工期

        Chart2.Series[0].Points[0].ToolTip = ModelToolTip(1, warningTargetBySafeColor);//安全
        Chart2.Series[0].Points[1].ToolTip = ModelToolTip(2, warningTargetByQualityColor);//质量
        Chart2.Series[0].Points[2].ToolTip = ModelToolTip(3, warningTargetByCostColor);//成本
        Chart2.Series[0].Points[3].ToolTip = ModelToolTip(4, warningTargetByDurationColor);//工期

        TextAnnotation t = (TextAnnotation)Chart2.Annotations["txtProjectCount"];

        if (projectType == "h" || projectType == "b")
        {
            t.Text = "在建项目" + projectCount + "个";
        }
        else
        {
            string projectConstractstageStr = "施工阶段：" + projectConstractstage;

            string projectConstractstageStr1 = "";
            if (projectConstractstageStr.Length > 12)
            {
                projectConstractstageStr1 = projectConstractstageStr.Substring(12);
                projectConstractstageStr = projectConstractstageStr.Substring(0, 12) + "\n\n";
            }

            while (projectConstractstageStr1.Length > 12)
            {
                projectConstractstageStr += projectConstractstageStr1.Substring(0, 12) + "\n\n";
                projectConstractstageStr1 = projectConstractstageStr1.Substring(12);
            }

            projectConstractstageStr += projectConstractstageStr1;

            t.Text = projectConstractstageStr;
        }
        #endregion

    }
    //1.安全，2.质量，3.成本，4.工期
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

    private void SetChart2Data()
    {
        Chart2.Series["Default"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), this.DropDownListPieType.SelectedItem.ToString(), true);
        this.HoleSizeList.Enabled = (this.DropDownListPieType.SelectedItem.ToString() != "Pie");

        Chart2.Series["Default"]["PieLabelStyle"] = this.LabelStyleList.SelectedItem.ToString();
        Chart2.Series["Default"]["DoughnutRadius"] = this.HoleSizeList.SelectedItem.ToString();

        foreach (DataPoint point in Chart2.Series["Default"].Points)
        {
            point["Exploded"] = "false";
            if (point.AxisLabel == this.ExplodedPointList.SelectedItem.ToString())
            {
                point["Exploded"] = "true";
            }
        }

        Chart2.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = CheckboxShow3D.Checked;

        Chart2.Series[0]["PieDrawingStyle"] = this.DropdownlistPieDrawingStyle.SelectedItem.ToString();

        if (this.CheckboxShow3D.Checked)
            this.DropdownlistPieDrawingStyle.Enabled = false;
        else
            this.DropdownlistPieDrawingStyle.Enabled = true;
    }
    //饼状图
    private void BindChart2Data()
    {
        string[] xValues = { "安全", "质量", "成本", "工期" };//"武汉", "北京", "上海", "天津", "济南", "郑州", "西安"
        double[] yValues = { 25, 25, 25, 25 };

        Chart2.Series["Default"].Points.DataBindXY(xValues, yValues);
    }
    protected void DropDownListPieType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Chart2.Series["Default"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), this.DropDownListPieType.SelectedItem.ToString(), true);
        this.HoleSizeList.Enabled = (this.DropDownListPieType.SelectedItem.ToString() != "Pie");
    }

    protected void ExplodedPointList_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (DataPoint point in Chart2.Series["Default"].Points)
        {
            point["Exploded"] = "false";
            if (point.LegendText == this.ExplodedPointList.SelectedItem.ToString())
            {
                point["Exploded"] = "true";
            }
        }
    }
    protected void HoleSizeList_SelectedIndexChanged(object sender, EventArgs e)
    {
        Chart2.Series["Default"]["DoughnutRadius"] = this.HoleSizeList.SelectedItem.ToString();
    }
    protected void LabelStyleList_SelectedIndexChanged(object sender, EventArgs e)
    {
        Chart2.Series["Default"]["PieLabelStyle"] = this.LabelStyleList.SelectedItem.ToString();
    }
    protected void DropdownlistPieDrawingStyle_SelectedIndexChanged(object sender, EventArgs e)
    {
        Chart2.Series[0]["PieDrawingStyle"] = this.DropdownlistPieDrawingStyle.SelectedItem.ToString();
    }
    protected void CheckboxShow3D_CheckedChanged(object sender, EventArgs e)
    {
        Chart2.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = CheckboxShow3D.Checked;

        if (this.CheckboxShow3D.Checked)
            this.DropdownlistPieDrawingStyle.Enabled = false;
        else
            this.DropdownlistPieDrawingStyle.Enabled = true;
    }
    protected void DropDownListColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        Chart2.Series[0].Points[2].Color = System.Drawing.Color.FromName(DropDownListColor.SelectedValue);
    }
}
