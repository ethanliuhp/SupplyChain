using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;

public partial class TestPage1 :PageBaseClass
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack == false)
        {
            #region 饼图

            //BindChart2Data();

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

            #endregion
        }
    }

    //饼状图
    private void BindChart2Data()
    {
        string[] xValues = { "武汉", "北京", "上海", "天津", "济南", "郑州", "西安" };//"武汉", "北京", "上海", "天津", "济南", "郑州", "西安"
        double[] yValues = { 65.62, 75.54, 60.45, 55.73, 70.42, 55.55, 50.35 };

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
}
