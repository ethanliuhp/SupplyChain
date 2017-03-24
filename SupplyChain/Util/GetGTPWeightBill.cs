using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using GTP.Cloud.RestClient;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
namespace Application.Business.Erp.SupplyChain.Util
{
    public enum EnumServerType
    {
        收料磅单=0,
        作废收料磅单=1,
        发料磅单=2,
        作废发料磅单=3,
        所有=100

    }
   public  static  class GetGTPWeightBill
    {
       private static  RestServiceClient restClient = null;
        static  string LicensePath = AppDomain.CurrentDomain.BaseDirectory + "auth.lic";
        const string ConstUrlPath = "/api/inspection/v1.0/integrate/";
        const int ConstPageSize = 200;
        static  string ServerAddress;
         static GetGTPWeightBill()
        {
            try
            {
                if (restClient == null)
                {
                    if (File.Exists(LicensePath))
                    {
                        RestServiceClient.LoadLicense(LicensePath);
                        restClient = new RestServiceClient();
                        ServerAddress = RestServiceClient.RestRootAddress + ConstUrlPath;

                    }
                    else
                    {
                        throw new Exception(string.Format("授权文件不存在:{0}", LicensePath));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("加载授权文件时错误信息提示:{0}", ex.Message));
            }
        }
        //public GetGTPWeightBill()
        //    : this(LicensePath)
        //{
             
        //}
        //public  GetGTPWeightBill(string sLicensePath)
        //{
        //    try
        //    {
        //        if (restClient == null)
        //        {
        //            if (File.Exists(sLicensePath))
        //            {
        //                RestServiceClient.LoadLicense(sLicensePath);
        //                restClient = new RestServiceClient();
        //                ServerAddress = RestServiceClient.RestRootAddress + ConstUrlPath;
        //            }
        //            else
        //            {
        //                throw new Exception("授权文件不存在");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public  static   List<WeightBillMaster> QueryWeightBill(EnumServerType serverType, string sProjectCode, DateTime dStart, DateTime dEnd)
        {
           
            int iPageIndex = 0;
            bool bFlag = true;
            string sParam=GetParam(sProjectCode,dStart,dEnd);
            string sTempUrl = GetServerUrl(serverType);
            string sUrl = string.Empty, sResult = string.Empty; ;
            RestResponseInfo restResponseInfo = null;
            List<WeightBillMaster> lstResult = new List<WeightBillMaster>();
            List<WeightBillMaster> lstTemp = null;
            try
            {
                do
                {
                    bFlag=false;
                    sUrl = sTempUrl + string.Format(sParam, iPageIndex);
                    restResponseInfo = restClient.Get(sUrl);
                    if (restResponseInfo.Success)
                    {
                        using (Stream oStream = restResponseInfo.ResponseStream)
                        {
                            sResult = ConvertJsonString(oStream);
                            lstTemp = ParseJson(sResult);
                            if (lstTemp != null && lstTemp.Count > 0)
                            {
                                lstResult.AddRange(lstTemp);
                            }
                            bFlag=(lstTemp!=null && lstTemp.Count>0);
                        }
                    }
                    else
                    {
                        throw new Exception(string.Format("过磅单服务请求失败[{0}]:{1}", restResponseInfo.ErrorCode, restResponseInfo.ErrorMessage));
 
                    }
                    iPageIndex++;
                } while (bFlag);
              
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("获取数据时错误信息提示:{0}", ex.Message));
            }
            return lstResult;
        }
        public static  string GetServerUrl(EnumServerType serverType)
        {
            string sResult = ServerAddress;
            switch (serverType)
            {
                case EnumServerType.发料磅单:
                    {
                        sResult += "getFLlist"; break;
                    }
                case EnumServerType.作废发料磅单:
                    {
                        sResult += "getFLDeletelist"; break;
                    }
                case EnumServerType.收料磅单:
                    {
                        sResult += "getSLlist"; break;
                    }
                case EnumServerType.作废收料磅单:
                    {
                        sResult += "getSLDeletelist"; break;
                    }
                case EnumServerType.所有:
                    {
                        sResult += "getlist"; break;
                    }     
            }
            return sResult;
        }
        public   static   string GetParam(string sProjectCode, DateTime dStart, DateTime dEnd)
        {
            string [] arrParam=new string[5];
            arrParam[0] = string.Format("?projectCode={0}", sProjectCode);
            arrParam[1] = string.Format("beginTimestamp={0:yyyyMMddHHmmss}", dStart);
            arrParam[2] = string.Format("endTimestamp={0:yyyyMMddHHmmss}", dEnd);
            arrParam[3] = "pageIndex={0}";
            arrParam[4] = string.Format("pageSize={0}",ConstPageSize);
            return string.Join("&",arrParam);
        }
        public static  List<WeightBillMaster> ParseJson(string sJsonString)
        {
            List<WeightBillMaster> lst = null;

            JObject jData = JObject.Parse(sJsonString);
         
            bool bSuccess=false;
            if (Boolean.TryParse(jData["success"].ToString(), out bSuccess))
            {
                if (bSuccess)
                {
                    JToken jBills =((JObject)jData["data"])["Bills"];
                    if (jBills.Count() > 0)
                    {
                        lst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WeightBillMaster>>(jBills.ToString());
                    }
                    
                }
            }



            return lst;
        }

        #region 数据转换
        public static string ConvertJsonString(string str)
        {
            //格式化json字符串
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }
        public static string ConvertJsonString(Stream oStream)
        {
            //格式化json字符串
            JsonSerializer serializer = new JsonSerializer();
            using (TextReader oTextReader = new StreamReader(oStream))
            {
                using (JsonTextReader jtr = new JsonTextReader(oTextReader))
                {
                    object obj = serializer.Deserialize(jtr);
                    if (obj != null)
                    {
                        using (StringWriter textWriter = new StringWriter())
                        {
                            using (JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                                //{
                                //    Formatting = Formatting.Indented,
                                //    Indentation = 4,
                                //    IndentChar = ' '
                                //}
                             )
                            {
                                serializer.Serialize(jsonWriter, obj);
                                return textWriter.ToString();
                            }
                        }
                    }
                    else
                    {
                        return "";
                    }
                }
            }

        }
        public static string ConvertJsonString1(Stream oStream)
        {
            //格式化json字符串
            JsonSerializer serializer = new JsonSerializer();
            using (TextReader oTextReader = new StreamReader(oStream))
            {
                using (JsonTextReader jtr = new JsonTextReader(oTextReader))
                {
                    object obj = serializer.Deserialize(jtr);
                    if (obj != null)
                    {
                        using (StringWriter textWriter = new StringWriter())
                        {
                            using (JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                            {
                                Formatting = Formatting.Indented,
                                Indentation = 4,
                                IndentChar = ' '
                            }
                             )
                            {
                                serializer.Serialize(jsonWriter, obj);
                                return textWriter.ToString();
                            }
                        }
                    }
                    else
                    {
                        return "";
                    }
                }
            }

        }
        #endregion
    }
    //{"BDBH":"ZD17XM-20160608-1000",
    //"BDCL":[{"BDCLID":600306,"BZDW":"吨","BZSL":29,"CLBM":"I112010200035","CLLBBM":"I1120102","CLLBID":1462323,"CLLBMC":"特殊混凝土","CLMC":"细石混凝土","DWHSID":0,"FC":-0.1,"GGXH":"C15","GSCLZDID":1509640,"HSXS":2.385,"JLDW":"立方米","KL":0,"PCJG":0,"PCLX":"吨","SJSL":12.159,"SJWC":0.38,"SJZL":29,"SYHSXS":"1立方米=2.385吨","YDSL":12,"YDZL":28.62,"ZC":10,"billId":1580674,"id":1986323,"projectId":1438621,"tenantId":1438458}],
    //"BFBM":"01","BZ":"","CCSJ":"20160608125715","CJSJ":"20160608101026",
    //"CLPCBL":1.33,"CMSJ":"20160608101026","CPH":"鄂000515","CPSJ":"20160608125715","DJLY":"称重",
    //"DWGC":"后湖","DYCS":2,"FBDMC":"","GBY":"张金波","GSGYSID":1457344,
    //"GUID":"27eb22a1-0bc7-4c65-a28c-36866f1b2d28","GYSBM":"K0100415a","GYSMC":"中建商混混凝土有限公司（后湖站）",
    //"JZ":29,"LX":0,"LX_YT":"采购","MZ":45.62,"PCJG":0,"PZ":16.62,"PZPCJG":0,"QRCode":"",
    //"SJ":"","SJZLHJ":29,"STATUS":1,"SYBW":"4-4区底板承台垫层","SingleMaterial":true,"Timestamp":"20160716214352",
    //"YDXCZLHJ":28.52,"YDZLHJ":28.62,"YSDJBH":"","YSDW":"","ZDHP":false,"ZHXGSJ":"20160716091300",
    //"billTypeId":1,"completionTime":1465361835000,"createTime":1465351826000,"discrepantNumber":0,
    //"grossWeightTime":1465351826000,"id":1580674,"lastModifyTime":1468631580000,"projectCode":"zd17",
    //"projectId":1438621,"rowTimestamp":1468676632000,"tareWeightTime":1465361835000}
    /// <summary>
    /// 过磅单主表
    /// </summary>
   [Serializable]
   public class WeightBillMaster
   {
       // "BDBH":"BL-20160506-PXXM-01-0005","BDCL":
       /// <summary>
       /// 单据编号，不唯一 
       /// </summary>
       public string BDBH { get; set; }
       /// <summary>
       /// 磅单材料 
       /// </summary>
       public IList<WeightBillDetail> BDCL { get; set; }
       //"BFBM":"","BZ":"水果","CCSJ":"20160506091624","CJSJ":"20160506091900",
       //"BFBM":"01","BZ":"","CCSJ":"20160608125715","CJSJ":"20160608101026",
      // public string BFBM { get; set; }
       /// <summary>
       /// 备注
       /// </summary>
       //public string BZ { get; set; }
       /// <summary>
       /// 出场时间
       /// </summary>
     //  public string CCSJ { get; set; }
     //  public string CJSJ { get; set; }
       //"CLPCBL":0,"CMSJ":"20160506091624","CPH":"鲁B22222","CPSJ":"20160506091624","DJLY":"补录",
       //"CLPCBL":1.33,"CMSJ":"20160608101026","CPH":"鄂000515","CPSJ":"20160608125715","DJLY":"称重",
     //  public decimal CLPCBL { get; set; }
       /// <summary>
       ///  称毛时间
       /// </summary>
       public string CMSJ { get; set; }
       /// <summary>
       /// 车牌号
       /// </summary>
      public string CPH { get; set; }
       /// <summary>
       /// 称皮时间 自动回皮时，cpsj为空
       /// </summary>
    //   public string CPSJ { get; set; }
       /// <summary>
       /// 单据来源，有“称重”、“补录”
       /// </summary>
       public string DJLY { get; set; }
       //"DWGC":"02","DYCS":0,"FBDMC":"","GBY":"系统管理员","GSGYSID":1448422,
       //"DWGC":"后湖","DYCS":2,"FBDMC":"","GBY":"张金波","GSGYSID":1457344,
       /// <summary>
       /// -单位工程
       /// </summary>
       public string DWGC { get; set; }
     //  public string DYCS { get; set; }
     //  public string FBDMC { get; set; }
       /// <summary>
       /// 过磅员
       /// </summary>
      public string GBY { get; set; }
   //    public int GSGYSID { get; set; }
       //"GUID":"6b960205-0a01-4f5b-ae53-cde600ecefb5","GYSBM":"K00673","GYSMC":"武汉市武昌区盛鑫达建材经营部",
       //"GUID":"27eb22a1-0bc7-4c65-a28c-36866f1b2d28","GYSBM":"K0100415a","GYSMC":"中建商混混凝土有限公司（后湖站）",
       /// <summary>
       /// 唯一标识
       /// </summary>
       public string GUID { get; set; }
       /// <summary>
       /// 供应商编码
       /// </summary>
       public string GYSBM { get; set; }
       /// <summary>
       /// 供应商名称
       /// </summary>
       public string GYSMC { get; set; }
       //"IsHaveImage":true,"JZ":60,"LX":0,"LX_YT":"采购","MZ":80,"PCJG":-1,"PZ":20,"PZPCJG":0,"QRCode":"",
       //                   "JZ":29,"LX":0,"LX_YT":"采购","MZ":45.62,"PCJG":0,"PZ":16.62,"PZPCJG":0,"QRCode":"",
       //public bool IsHaveImage { get; set; }
       /// <summary>
       ///  净重
       /// </summary>
       public decimal JZ { get; set; }
       /// <summary>
       /// 类型，0收料，1发料
       /// </summary>
       public int LX { get; set; }
       /// <summary>
       /// 类型，收料（采购、调入、甲供），发料（发料、废旧物资、调出、售出）
       /// </summary>
       public string LX_YT { get; set; }
       /// <summary>
       /// 毛重
       /// </summary>
    //   public decimal MZ { get; set; }
       /// <summary>
       /// 偏差结果，0正常 1超正差 -1 超负差  
       /// </summary>
    //   public int PCJG { get; set; }
       /// <summary>
       /// 皮重
       /// </summary>
  //     public decimal PZ { get; set; }
   //    public decimal PZPCJG { get; set; }
   //    public string QRCode { get; set; }
       //"SJ":"","SJZLHJ":0,"STATUS":1,"SXBS":true,"SYBW":"都是","SingleMaterial":true,"Timestamp":"20160506092118",
       //"SJ":"","SJZLHJ":29,"STATUS":1,"SYBW":"4-4区底板承台垫层","SingleMaterial":true,"Timestamp":"20160716214352",
  //     public string SJ { get; set; }
   //    public decimal SJZLHJ { get; set; }
       /// <summary>
       /// 1正常 0 作废
       /// </summary>
    //   public int STATUS { get; set; }
       //public bool SXBS { get; set; }
       /// <summary>
       /// 使用部位
       /// </summary>
       public string SYBW { get; set; }
  //     public bool SingleMaterial { get; set; }
       /// <summary>
       /// 时间戳
       /// </summary>
  //     DateTime Timestamp { get; set; }
       //"YDXCZLHJ":0,"YDZLHJ":0,"YSDJBH":"543265435","YSDW":"","ZDHP":false,"ZHXGSJ":"20160506092103",
       //"YDXCZLHJ":28.52,"YDZLHJ":28.62,"YSDJBH":"","YSDW":"","ZDHP":false,"ZHXGSJ":"20160716091300",
//       public decimal YDXCZLHJ { get; set; }
  //     public decimal YDZLHJ { get; set; }
       /// <summary>
       ///  原始单据编码
       /// </summary>
   //    public string YSDJBH { get; set; }
   //    public string YSDW { get; set; }
   //    public bool ZDHP { get; set; }
       /// <summary>
       /// 最后修改时间
       /// </summary>
 //      public string ZHXGSJ { get; set; }
       //"billTypeId":201,"completionTime":1462497384000,"createTime":1462497540000,"discrepantNumber":1,
       //"billTypeId":1,"completionTime":1465361835000,"createTime":1465351826000,"discrepantNumber":0,
    //   public int billTypeId { get; set; }
  //     public long completionTime { get; set; }
   //    public long createTime { get; set; }
  //     public int discrepantNumber { get; set; }
       //"grossWeightTime":1462497384000,"id":1489532,"lastModifyTime":1462497663000,"projectCode":"PLUS2014SD001",
       //"grossWeightTime":1465351826000,"id":1580674,"lastModifyTime":1468631580000,"projectCode":"zd17",
   //    public long grossWeightTime { get; set; }
       public long id { get; set; }
 //      public long lastModifyTime { get; set; }
       //项目编码
       public string projectCode { get; set; }
       //"projectId":1438628,"rowTimestamp":1462497678000,"tareWeightTime":1462497384000,"tenantId":1438458
       //"projectId":1438621,"rowTimestamp":1468676632000,"tareWeightTime":1465361835000
       public long projectId { get; set; }
   //    public long rowTimestamp { get; set; }
    //   public long tareWeightTime { get; set; }
   //    public long tenantId { set; get; }
   }
    /// <summary>
    /// 过磅单材料
    /// </summary>
   [Serializable]
   public class WeightBillDetail
   {
       //{"BDCLID":600306,"BZDW":"吨","BZSL":29,"CLBM":"I112010200035","CLLBBM":"I1120102","CLLBID":1462323,
       //"CLLBMC":"特殊混凝土","CLMC":"细石混凝土","DWHSID":0,"FC":-0.1,"GGXH":"C15","GSCLZDID":1509640,"HSXS":2.385,
       //"JLDW":"立方米","KL":0,"PCJG":0,"PCLX":"吨","SJSL":12.159,"SJWC":0.38,"SJZL":29,"SYHSXS":"1立方米=2.385吨",
       //"YDSL":12,"YDZL":28.62,"ZC":10,"billId":1580674,"id":1986323,"projectId":1438621,"tenantId":1438458}],

       //{"BDCLID":700002,"BZDW":"吨","BZSL":56.4,"CLBM":"I110010100108","CLLBBM":"I1100101","CLLBID":1461562,
       //{"BDCLID":600306,"BZDW":"吨","BZSL":29,"CLBM":"I112010200035","CLLBBM":"I1120102","CLLBID":1462323,
       public long BDCLID { get; set; }
       public string BZDW { get; set; }
       public decimal BZSL { get; set; }
       /// <summary>
       /// 材料编码
       /// </summary>
       public string CLBM { get; set; }
       public string CLLBBM { get; set; }
       public long CLLBID { get; set; }
       //"CLLBMC":"圆钢","CLMC":"圆钢（HPB300）(HPB300)","FC":-3,"GGXH":"16mm","GSCLZDID":1499750,"HSXS":1,
       //"CLLBMC":"特殊混凝土","CLMC":"细石混凝土","DWHSID":0,"FC":-0.1,"GGXH":"C15","GSCLZDID":1509640,"HSXS":2.385,
       public string CLLBMC { get; set; }
       /// <summary>
       /// 材料名称
       /// </summary>
       public string CLMC { get; set; }
       public long DWHSID { get;set;}
       /// <summary>
       /// 负差
       /// </summary>
     //  public decimal FC { get; set; }
       /// <summary>
       /// 规格型号
       /// </summary>
       public string GGXH { get; set; }
    //   public long GSCLZDID { get; set; }
    //   public decimal HSXS { get; set; }
       //"JLDW":"吨","KL":3.6,"PCJG":-1,"PCLX":"%","SJSL":56.4,"SJWC":-6.83,"SJZL":56.4,"SYHSXS":"1吨=1吨",
       //"JLDW":"立方米","KL":0,"PCJG":0,"PCLX":"吨","SJSL":12.159,"SJWC":0.38,"SJZL":29,"SYHSXS":"1立方米=2.385吨",
       /// <summary>
       /// 计量单位
       /// </summary>
       public string JLDW { get; set; }
       /// <summary>
       /// 扣量
       /// </summary>
   //    public decimal KL { get; set; }
       /// <summary>
       /// 偏差结果，0正常 1超正差 -1 超负差
       /// </summary>
    //   public int PCJG { get; set; }
       /// <summary>
       /// 偏差类型（%或吨）
       /// </summary>
    //   public string PCLX { get; set; }
       /// <summary>
       /// 实际数量
       /// </summary>
       public decimal SJSL { get; set; }
     //  public decimal SJWC { get; set; }
       /// <summary>
       /// 实际重量,单位是吨
       /// </summary>
     //  public decimal SJZL { get; set; }
       /// <summary>
       /// 使用换算系数
       /// </summary>
     //  public string SYHSXS { get; set; }
       //"YDSL":60.535,"YDZL":60.535,"ZC":3,"ZWCFW":false,"billId":1489532,"id":1486973,"projectId":1438628,
       //"YDSL":12,"YDZL":28.62,"ZC":10,"billId":1580674,"id":1986323,"projectId":1438621,"tenantId":1438458}],
       /// <summary>
       /// 运单数量
       /// </summary>
   //    public decimal YDSL { get; set; }
       /// <summary>
       /// 运单重量,单位是吨
       /// </summary>
    //   public decimal YDZL { get; set; }
       /// <summary>
       /// 正差
       /// </summary>
   //    public int ZC { get; set; }
      // public bool ZWCFW { get; set; }
   //    public long billId { get; set; }
       /// <summary>
       /// 磅单材料ID
       /// </summary>
      public long id { get; set; }
    //   public long projectId { get; set; }
       //"tenantId":1438458}
   //    public long tenantId { get; set; }
       public WeightBillMaster Master { get; set; }
   }
}
