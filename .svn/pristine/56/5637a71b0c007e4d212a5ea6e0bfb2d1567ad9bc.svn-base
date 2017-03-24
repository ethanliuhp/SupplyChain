using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;

public partial class UserControls_ReceivablesAnalysisFigure : System.Web.UI.UserControl, IPageLink
{
    private string _ProjectId;
    public string ProjectId
    {
        get { return _ProjectId; }
        set { _ProjectId = value; }
    }
    private string _ProjectSyscode;
    public string ProjectSyscode
    {
        get { return _ProjectSyscode; }
        set { _ProjectSyscode = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack == false)
        {
            BindChart1Data();

            Chart1.Series["Series1"].IsValueShownAsLabel = true;
            Chart1.Series["Series2"].IsValueShownAsLabel = true;
            Chart1.Series["Series1"]["LabelStyle"] = "Auto";
            Chart1.Series["Series2"]["LabelStyle"] = "Auto";
            Chart1.Series["Series1"]["ShowMarkerLines"] = "true";
            Chart1.Series["Series2"]["ShowMarkerLines"] = "true";
        }
    }
    private void BindChart1Data()
    {
        Chart1.Series["Series1"].Points.Clear();
        Chart1.Series["Series2"].Points.Clear();

        Random random = new Random();

        string[] times = new string[12];//{ "2012/1", "2012/2", "2012/3", "2012/4", "2012/5", "2012/6","2012/7", "2012/8", "2012/9", "2012/10", "2012/11", "2012/12"};
        int index = 11;
        for (DateTime i = DateTime.Now; i > DateTime.Now.AddYears(-1); i = i.AddMonths(-1))
        {
            times[index] = i.Year + "/" + i.Month;
            index -= 1;
        }

        DataTable[] dts = MGWBS.GWBSSrv.GetProjectCollectionsummoneyData(DateTime.Now.AddYears(-1), ProjectSyscode);

        for (int pointIndex = 0; pointIndex < times.Length; pointIndex++)
        {
            string date = times[pointIndex];
            decimal contractTotalPrice = 0;
            decimal collectionTotalPrice = 0;

            foreach (DataRow row in dts[0].Rows)
            {
                if (DateTime.Parse(date) == DateTime.Parse(row[0].ToString()))
                {
                    contractTotalPrice = decimal.Round(Convert.ToDecimal(row[1]) / 10000, 2);
                    break;
                }
            }
            foreach (DataRow row in dts[1].Rows)
            {
                if (DateTime.Parse(date) == DateTime.Parse(row[0].ToString()))
                {
                    collectionTotalPrice = decimal.Round(Convert.ToDecimal(row[1]) / 10000, 2);
                    break;
                }
            }

            Chart1.Series["Series1"].Points.AddXY(date, contractTotalPrice);
            Chart1.Series["Series2"].Points.AddXY(date, collectionTotalPrice);
        }


        //Color[] colors = { System.Drawing.ColorTranslator.FromHtml("#418CF0"), System.Drawing.ColorTranslator.FromHtml("#FCB441") ,
        //                 System.Drawing.ColorTranslator.FromHtml("#E0400A") ,System.Drawing.ColorTranslator.FromHtml("#056492") ,
        //                 System.Drawing.ColorTranslator.FromHtml("#BFBFBF") ,System.Drawing.ColorTranslator.FromHtml("#1A3B69") ,
        //                 System.Drawing.ColorTranslator.FromHtml("#FFE382") ,System.Drawing.ColorTranslator.FromHtml("#129CDD") ,
        //                 System.Drawing.ColorTranslator.FromHtml("#CA6B4B") ,System.Drawing.ColorTranslator.FromHtml("#005CDB") ,
        //                 System.Drawing.ColorTranslator.FromHtml("#F3D288") ,System.Drawing.ColorTranslator.FromHtml("#506381") };

        //Chart1.Series["Series1"].Points.Clear();
        //Chart2.Series["Series1"].Points.Clear();
        //Chart3.Series["Series1"].Points.Clear();

        //string[] xValues = { "2012/1", "2012/2", "2012/3", "2012/4", "2012/5", "2012/6",
        //                 "2012/7", "2012/8", "2012/9", "2012/10", "2012/11", "2012/12"};
        //double[] yValues = { 80, 120, 60, 140, 50, 100, 50, 100, 50, 100, 50, 100 };

        //Random random = new Random();
        //for (int pointIndex = 0; pointIndex < xValues.Length; pointIndex++)
        //{
        //    string xValue = xValues[pointIndex];
        //    double yValue = yValues[pointIndex];

        //    System.Web.UI.DataVisualization.Charting.DataPoint dp1 = new System.Web.UI.DataVisualization.Charting.DataPoint();
        //    dp1.LegendText = xValue;
        //    dp1.AxisLabel = yValue.ToString();
        //    dp1.YValues = new double[] { yValue };
        //    //dp1.Color = colors[pointIndex];
        //    Chart1.Series["Series1"].Points.Add(dp1);

        //    double yValue2 = (double)random.Next((int)(yValue - yValue * 0.3), (int)yValue);
        //    System.Web.UI.DataVisualization.Charting.DataPoint dp2 = new System.Web.UI.DataVisualization.Charting.DataPoint();
        //    dp2.LegendText = xValue;
        //    dp2.AxisLabel = yValue2.ToString();
        //    dp2.YValues = new double[] { yValue2 };
        //    dp2.Color = colors[pointIndex];
        //    Chart2.Series["Series1"].Points.Add(dp2);

        //    double yValue3 = (double)yValue2 / yValue;
        //    System.Web.UI.DataVisualization.Charting.DataPoint dp3 = new System.Web.UI.DataVisualization.Charting.DataPoint();
        //    dp3.LegendText = xValue;
        //    dp3.YValues = new double[] { yValue3 };
        //    dp3.Color = colors[pointIndex];
        //    Chart3.Series["Series1"].Points.Add(dp3);
        //}


    }
}
