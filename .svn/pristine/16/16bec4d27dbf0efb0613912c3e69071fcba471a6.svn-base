using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Data;
using VirtualMachine.Component.Util;

public partial class UserControls_CostAnalysisFigure : System.Web.UI.UserControl, IPageLink
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

            //Chart1.Series["Series1"].IsValueShownAsLabel = true;
            //Chart1.Series["Series2"].IsValueShownAsLabel = true;
            //Chart1.Series["Series3"].IsValueShownAsLabel = true;
            Chart1.Series["Series1"]["LabelStyle"] = "Right";
            Chart1.Series["Series2"]["LabelStyle"] = "Right";
            Chart1.Series["Series3"]["LabelStyle"] = "Right";
            Chart1.Series["Series4"]["LabelStyle"] = "Right";
            //Chart1.Series["Series1"]["ShowMarkerLines"] = "true";
            //Chart1.Series["Series2"]["ShowMarkerLines"] = "true";
            //Chart1.Series["Series3"]["ShowMarkerLines"] = "true";
        }
    }
    private void BindChart1Data()
    {
        Chart1.Series["Series1"].Points.Clear();
        Chart1.Series["Series2"].Points.Clear();
        Chart1.Series["Series3"].Points.Clear();
        Chart1.Series["Series4"].Points.Clear();

        Random random = new Random();
        string[] times = new string[12];//{ "2012/1", "2012/2", "2012/3", "2012/4", "2012/5", "2012/6","2012/7", "2012/8", "2012/9", "2012/10", "2012/11", "2012/12"};
        int index = 11;
        for (DateTime i = DateTime.Now; i > DateTime.Now.AddYears(-1); i = i.AddMonths(-1))
        {
            times[index] = i.Year + "/" + i.Month;
            index -= 1;
        }

        DataTable[] dts = MGWBS.GWBSSrv.GetProjectCostData(DateTime.Now.AddYears(-1), ProjectSyscode);

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

            Chart1.Series["Series1"].Points.Add(dp);


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
            Chart1.Series["Series2"].Points.Add(dp1);

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
            Chart1.Series["Series3"].Points.Add(dp2);

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
            Chart1.Series["Series4"].Points.Add(dp3);
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
}
