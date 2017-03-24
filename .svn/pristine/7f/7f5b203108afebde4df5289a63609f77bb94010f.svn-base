using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using VirtualMachine.Component.Util;

public partial class UserControls_ProfitChart : System.Web.UI.UserControl, IPageLink
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
            //Chart1.Series["Series2"]["LabelStyle"] = "Right";
            Chart1.Series["Series3"]["LabelStyle"] = "Right";
            Chart1.Series["Series4"]["LabelStyle"] = "Right";
            Chart1.Series["Series5"]["LabelStyle"] = "Right";
            //Chart1.Series["Series6"]["LabelStyle"] = "Right";
            Chart1.Series["Series7"]["LabelStyle"] = "Right";
            Chart1.Series["Series8"]["LabelStyle"] = "Right";
            //Chart1.Series["Series1"]["ShowMarkerLines"] = "true";
            //Chart1.Series["Series2"]["ShowMarkerLines"] = "true";
            //Chart1.Series["Series3"]["ShowMarkerLines"] = "true";
        }
    }
    private void BindChart1Data()
    {
        Chart1.Series["Series1"].Points.Clear();
        //Chart1.Series["Series2"].Points.Clear();
        Chart1.Series["Series3"].Points.Clear();
        Chart1.Series["Series4"].Points.Clear();
        Chart1.Series["Series5"].Points.Clear();
        //Chart1.Series["Series6"].Points.Clear();
        Chart1.Series["Series7"].Points.Clear();
        Chart1.Series["Series8"].Points.Clear();

        string[] times = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };

        int lastYear = DateTime.Now.Year - 1;
        int thisYear = DateTime.Now.Year;

        Chart1.Series["Series1"].LegendText = lastYear + "合同收入";
        //Chart1.Series["Series2"].LegendText = lastYear + "累计责任成本";
        Chart1.Series["Series3"].LegendText = lastYear + "实际成本";
        Chart1.Series["Series4"].LegendText = lastYear + "实际收款";
        Chart1.Series["Series5"].LegendText = thisYear + "合同收入";
        //Chart1.Series["Series6"].LegendText = thisYear + "累计责任成本";
        Chart1.Series["Series7"].LegendText = thisYear + "实际成本";
        Chart1.Series["Series8"].LegendText = thisYear + "实际收款";

        DataTable[] dts = MGWBS.GWBSSrv.GetProjectCostDataByYear(DateTime.Parse(lastYear + "-1-1"), ProjectSyscode);

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
            Chart1.Series["Series1"].Points.Add(dp);

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
            Chart1.Series["Series3"].Points.Add(dp2);

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
            Chart1.Series["Series4"].Points.Add(dp3);

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
            Chart1.Series["Series5"].Points.Add(dp4);

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
            //Chart1.Series["Series6"].Points.Add(dp5);

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
            Chart1.Series["Series7"].Points.Add(dp6);

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
            Chart1.Series["Series8"].Points.Add(dp7);

            #endregion

        }
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
            lblCostInfoTitle.InnerText = lastYear + "年" + date + "月 成本信息";

            decimal lastContractTotalPrice = 0;
            decimal lastResonsibleTotalPrice = 0;
            decimal lastRealTotalPrice = 0;
            decimal lastColltionsummoneyTotalPrice = 0;

            foreach (DataRow row in dts[0].Rows)
            {
                if (DateTime.Parse(lastYear + "-" + date) == DateTime.Parse(row[0].ToString()))
                {
                    lastContractTotalPrice = decimal.Round(ClientUtil.ToDecimal(row[1]) / 10000, 2);
                    lastResonsibleTotalPrice = decimal.Round(ClientUtil.ToDecimal(row[2]) / 10000, 2);
                    lastRealTotalPrice = decimal.Round(ClientUtil.ToDecimal(row[3]) / 10000, 2);
                    break;
                }
            }
            foreach (DataRow row in dts[1].Rows)
            {
                if (DateTime.Parse(lastYear + "-" + date) == DateTime.Parse(row[0].ToString()))
                {
                    lastColltionsummoneyTotalPrice = decimal.Round(ClientUtil.ToDecimal(row[1]) / 10000, 2);
                    break;
                }
            }

            lblContractTotalPrice.InnerText = lastContractTotalPrice.ToString();
            lblResonsibleTotalPrice.InnerText = lastResonsibleTotalPrice.ToString();
            lblRealTotalPrice.InnerText = lastRealTotalPrice.ToString();
            lblRealColltionsummoney.InnerText = lastColltionsummoneyTotalPrice.ToString();

            if (lastContractTotalPrice != 0)
                lblProfit.InnerText = UtilityClass.DecimalRound(((lastContractTotalPrice - lastRealTotalPrice) / lastContractTotalPrice) * 100, 2).ToString() + "%";
        }
        else
            lblCostInfoTitle.InnerText = lastYear + "年--月 成本信息";

        listLastDataIndex.Clear();
        listLastDataIndex.Add(lastContractDataIndex2);
        listLastDataIndex.Add(lastResponsibleDataIndex2);
        listLastDataIndex.Add(lastRealDataIndex2);
        listLastDataIndex.Add(lastCollectionsummoneyDataIndex2);

        maxDataIndex = listLastDataIndex.Max(p => p);

        decimal lastContractTotalPrice2 = 0;
        decimal lastResonsibleTotalPrice2 = 0;
        decimal lastRealTotalPrice2 = 0;
        decimal lastColltionsummoneyTotalPrice2 = 0;
        if (maxDataIndex != -1)
        {
            string date = times[maxDataIndex];
            lblCostInfoTitle2.InnerText = thisYear + "年" + date + "月 成本信息";

            foreach (DataRow row in dts[0].Rows)
            {
                if (DateTime.Parse(thisYear + "-" + date) == DateTime.Parse(row[0].ToString()))
                {
                    lastContractTotalPrice2 = decimal.Round(ClientUtil.ToDecimal(row[1]) / 10000, 2);
                    lastResonsibleTotalPrice2 = decimal.Round(ClientUtil.ToDecimal(row[2]) / 10000, 2);
                    lastRealTotalPrice2 = decimal.Round(ClientUtil.ToDecimal(row[3]) / 10000, 2);
                    break;
                }
            }
            foreach (DataRow row in dts[1].Rows)
            {
                if (DateTime.Parse(thisYear + "-" + date) == DateTime.Parse(row[0].ToString()))
                {
                    lastColltionsummoneyTotalPrice2 = decimal.Round(ClientUtil.ToDecimal(row[1]) / 10000, 2);
                    break;
                }
            }

            lblContractTotalPrice2.InnerText = lastContractTotalPrice2.ToString();
            lblResonsibleTotalPrice2.InnerText = lastResonsibleTotalPrice2.ToString();
            lblRealTotalPrice2.InnerText = lastRealTotalPrice2.ToString();
            lblRealColltionsummoney2.InnerText = lastColltionsummoneyTotalPrice2.ToString();

            if (lastContractTotalPrice2 != 0)
                lblProfit2.InnerText = UtilityClass.DecimalRound(((lastContractTotalPrice2 - lastRealTotalPrice2) / lastContractTotalPrice2) * 100, 2).ToString() + "%";
        }
        else
            lblCostInfoTitle2.InnerText = thisYear + "年--月 成本信息";
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
}
