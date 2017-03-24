using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Configuration;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

/// <summary>
///UtilityClass 的摘要说明
/// </summary>
public class UtilityClass
{
    public UtilityClass()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    public static bool MSChart2Excel(Chart chart)
    {
        return true;
    }

    public static bool MSChart2Word(Chart chart)
    {
        return true;
    }

    public static string DecimalRound(decimal value, int len)
    {
        return decimal.Round(value, 2).ToString();
    }

    /// <summary>
    /// C# HTTP Request请求程序模拟  向服务器送出请求
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public static string SendRequest(string urlKeyName, string param)
    {
        try
        {
            //BIMServerJsonApi
            string BIMServerJsonApiUrl = ConfigurationSettings.AppSettings[urlKeyName];            

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(param);
            HttpWebRequest request =
            (HttpWebRequest)HttpWebRequest.Create(BIMServerJsonApiUrl+param);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            request.Timeout = Int32.MaxValue;// 10 * 3600 * 1000;
            Stream sm = request.GetRequestStream();
            sm.Write(data, 0, data.Length);
            sm.Close();

            HttpWebResponse response =
            (HttpWebResponse)request.GetResponse();

            if (response.StatusCode.ToString() != "OK")
            {
                throw new Exception(response.StatusDescription.ToString());
            }

            StreamReader myreader = new StreamReader(
            response.GetResponseStream(), Encoding.UTF8);
            string responseText = myreader.ReadToEnd();

            //JObject resultJson = JObject.Parse(responseText);

            return responseText;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
   

}
