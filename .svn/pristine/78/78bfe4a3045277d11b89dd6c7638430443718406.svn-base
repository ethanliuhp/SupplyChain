using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.IO;
using Microsoft.Reporting.WebForms;

public partial class TestPage : PageBaseClass
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack == false)
        {
            #region 线性条形图
            BindChart1Data();

            Chart1.Series["Series1"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ChartTypeList.SelectedItem.Text, true);
            Chart1.Series["Series2"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ChartTypeList.SelectedItem.Text, true);
            Chart1.Series["Series3"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ChartTypeList.SelectedItem.Text, true);

            if (PointLabelsList.SelectedItem.Text != "None")
            {
                Chart1.Series["Series1"].IsValueShownAsLabel = true;
                Chart1.Series["Series2"].IsValueShownAsLabel = true;
                Chart1.Series["Series3"].IsValueShownAsLabel = true;
                if (PointLabelsList.SelectedItem.Text != "Auto")
                {
                    Chart1.Series["Series1"]["LabelStyle"] = PointLabelsList.SelectedItem.Text;
                    Chart1.Series["Series2"]["LabelStyle"] = PointLabelsList.SelectedItem.Text;
                    Chart1.Series["Series3"]["LabelStyle"] = PointLabelsList.SelectedItem.Text;
                }
            }

            Chart1.Series["Series1"]["ShowMarkerLines"] = ShowMarkers.Checked.ToString();
            Chart1.Series["Series2"]["ShowMarkerLines"] = ShowMarkers.Checked.ToString();
            Chart1.Series["Series3"]["ShowMarkerLines"] = ShowMarkers.Checked.ToString();
            #endregion
        }
    }
    private void BindChart1Data()
    {
        Chart1.Series["Series1"].Points.Clear();
        Chart1.Series["Series2"].Points.Clear();
        Chart1.Series["Series3"].Points.Clear();

        Random random = new Random();
        int[] times = { 2006, 2007, 2008, 2009, 2010, 2011, 2012 };
        for (int pointIndex = 0; pointIndex < times.Length; pointIndex++)
        {
            Chart1.Series["Series1"].Points.AddXY(times[pointIndex], random.Next(300, 800));
            Chart1.Series["Series2"].Points.AddXY(times[pointIndex], random.Next(100, 500));
            Chart1.Series["Series3"].Points.AddXY(times[pointIndex], random.Next(300, 1000));
        }
    }
    protected void btnChangeData_Click(object sender, EventArgs e)
    {
        BindChart1Data();
    }
    protected void ChartTypeList_SelectedIndexChanged(object sender, EventArgs e)
    {
        Chart1.Series["Series1"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ChartTypeList.SelectedItem.Text, true);
        Chart1.Series["Series2"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ChartTypeList.SelectedItem.Text, true);
        Chart1.Series["Series3"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ChartTypeList.SelectedItem.Text, true);
    }
    protected void PointLabelsList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (PointLabelsList.SelectedItem.Text != "None")
        {
            Chart1.Series["Series1"].IsValueShownAsLabel = true;
            Chart1.Series["Series2"].IsValueShownAsLabel = true;
            Chart1.Series["Series3"].IsValueShownAsLabel = true;
            if (PointLabelsList.SelectedItem.Text != "Auto")
            {
                Chart1.Series["Series1"]["LabelStyle"] = PointLabelsList.SelectedItem.Text;
                Chart1.Series["Series2"]["LabelStyle"] = PointLabelsList.SelectedItem.Text;
                Chart1.Series["Series3"]["LabelStyle"] = PointLabelsList.SelectedItem.Text;
            }
        }
    }
    protected void ShowMarkers_CheckedChanged(object sender, EventArgs e)
    {
        Chart1.Series["Series1"]["ShowMarkerLines"] = ShowMarkers.Checked.ToString();
        Chart1.Series["Series2"]["ShowMarkerLines"] = ShowMarkers.Checked.ToString();
        Chart1.Series["Series3"]["ShowMarkerLines"] = ShowMarkers.Checked.ToString();
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        Guid g = Guid.NewGuid();
        string guid = g.ToString().Replace("-", "");
        string imgDir = Server.MapPath("~/images/imageTemp/");
        string imageUrl = imgDir + guid + ".JPG";

        if (Directory.Exists(imgDir) == false)
            Directory.CreateDirectory(imgDir);

        Chart1.SaveImage(imageUrl);

        Microsoft.Reporting.WebForms.ReportParameter[] ReportParameters = new Microsoft.Reporting.WebForms.ReportParameter[3];
        ReportParameters[0] = new Microsoft.Reporting.WebForms.ReportParameter("ReportParameter1", "file:///" + Server.MapPath("~/images/imageTemp/") + guid + ".JPG");
        ReportParameters[1] = new Microsoft.Reporting.WebForms.ReportParameter("ReportParameter2", "我的报表！");
        ReportParameters[2] = new Microsoft.Reporting.WebForms.ReportParameter("ReportParameter3", "我的报表！");
        ReportViewer1.LocalReport.SetParameters(ReportParameters);


        //if (File.Exists(imageUrl))
        //{
        //    File.Delete(imageUrl);
        //}
    }
}
