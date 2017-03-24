using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Service;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;

namespace Application.Business.Erp.SupplyChain.Util
{
    public class KnowledgeUtil
    {
        #region 通用的方法
        public static bool isEmpty(string temp)
        {
            if (temp == null || temp.Equals(""))
                return true;
            else
                return false;
        }


        public static bool isEmpty(object temp)
        {
            if (temp == null || temp.ToString().Equals(""))
                return true;
            else
                return false;
        }

        public static bool isNotEmpty(string temp)
        {
            if (temp != null && !temp.Equals(""))
                return true;
            else
                return false;
        }


        public static bool isNotEmpty(object temp)
        {
            if (temp != null && !temp.ToString().Equals(""))
                return true;
            else
                return false;
        }

        //判断字符是否为数字
        public static bool IfNumber(string str)
        {
            bool ifnumber = true;
            string num = "0123456789";
            char[] c = str.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                string curr_char = c[i].ToString();
                if (!num.Contains(curr_char))
                {
                    ifnumber = false;
                    break;
                }
            }
            return ifnumber;
        }

        //判断字符是否为数字+小数点和负号
        public static bool IfWidthNumber(string str)
        {
            bool ifnumber = true;
            string num = "0123456789.-";
            char[] c = str.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                string curr_char = c[i].ToString();
                if (!num.Contains(curr_char))
                {
                    ifnumber = false;
                    break;
                }
            }
            return ifnumber;
        }

        //判断表达式是不是自定义公式,目前包括三个公式：(SUM([B2]:[B10])、SUMIF([B2]:[B10],<,0)、COUNTIF([B2]:[B10],<,0))
        public static bool IfDefineFunction(string express)
        {
            bool IfDefine = false;
            if (express != null && (express.IndexOf("SUM") != -1 || express.IndexOf("COUNT") != -1))
            {
                IfDefine = true;
            }
            return IfDefine;
        }

        //判断表达式是否包含大写字母
        public static bool IfHaveUpperLetter(string str)
        {
            bool ifLetter = false;
            string letter = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char[] c = str.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                string curr_char = c[i].ToString();
                if (letter.Contains(curr_char))
                {
                    ifLetter = true;
                    break;
                }
            }
            return ifLetter;
        }

        //截取指定字符串中开发字符和结束字符之间的字符串
        public static string SubStr(string str, string begin_str, string end_str)
        {
            int begin_len = begin_str.Length;
            int k = str.IndexOf(end_str, str.IndexOf(begin_str));//得到结束字符的索引位置
            string s = str.Substring(str.IndexOf(begin_str) + begin_len, k - str.IndexOf(begin_str) - begin_len);
            return s;
        }

        //计算一个集合中的和
        public static double SumCellValue(IList value_list)
        {
            double sum = 0;
            foreach (string value in value_list)
            {
                double temp = 0;
                if (IfWidthNumber(value) == false)
                {
                    temp = 0;
                }
                else {
                    if (!"-".Equals(value))
                    {
                        temp = double.Parse(value);
                    }                   
                }
                sum += temp;
            }

            return sum;
        }

        /// <summary>
        /// 计算一个集合中的符合表达式的数值的总和
        /// </summary>
        /// <param name="value_list">值的集合</param>
        /// <param name="symbol">比较符号，包括："<","<=",">",">=","=","!="</param>
        /// <param name="compare">比较的值</param>
        /// <returns></returns>
        public static double SumCellValueByExpress(IList value_list, string symbol, double compare)
        {
            double sum = 0;
            foreach (string value in value_list)
            {
                double temp = 0;
                double add_value = 0;
                if (IfWidthNumber(value) == false)
                {
                    temp = 0;
                }
                else
                {
                    if (!"-".Equals(value))
                    {
                        temp = double.Parse(value);
                    }
                }

                switch (symbol)
                { 
                    case "<":
                        if (temp < compare)
                        {
                            add_value = temp;
                        }
                        break;
                    case "<=":
                        if (temp <= compare)
                        {
                            add_value = temp;
                        }
                        break;
                    case ">":
                        if (temp > compare)
                        {
                            add_value = temp;
                        }
                        break;
                    case ">=":
                        if (temp >= compare)
                        {
                            add_value = temp;
                        }
                        break;
                    case "=":
                        if (temp == compare)
                        {
                            add_value = temp;
                        }
                        break;
                    case "!=":
                        if (temp != compare)
                        {
                            add_value = temp;
                        }
                        break;
                    default:
                        break;
                }

                sum += add_value;
            }

            return sum;
        }


        /// <summary>
        /// 计算一个集合中的符合表达式的个数
        /// </summary>
        /// <param name="value_list">值的集合</param>
        /// <param name="symbol">比较符号，包括："<","<=",">",">=","=","!="</param>
        /// <param name="compare">比较的值</param>
        /// <returns></returns>
        public static int CountCellByExpress(IList value_list, string symbol, double compare)
        {
            int count = 0;
            foreach (string value in value_list)
            {
                double temp = 0;
                if (IfWidthNumber(value) == false)
                {
                    temp = 0;
                }
                else
                {
                    if (!"-".Equals(value))
                    {
                        temp = double.Parse(value);
                    }
                }

                switch (symbol)
                {
                    case "<":
                        if (temp < compare)
                        {
                            count++;
                        }
                        break;
                    case "<=":
                        if (temp <= compare)
                        {
                            count++;
                        }
                        break;
                    case ">":
                        if (temp > compare)
                        {
                            count++;
                        }
                        break;
                    case ">=":
                        if (temp >= compare)
                        {
                            count++;
                        }
                        break;
                    case "=":
                        if (temp == compare)
                        {
                            count++;
                        }
                        break;
                    case "!=":
                        if (temp != compare)
                        {
                            count++;
                        }
                        break;
                    default:
                        break;
                }
            }
            return count;
        }


        //得到一个字符串中出现符号的次数
        public static int GetStrNums(string str,char[] pattern) { 
            int times =0;
            string[] temp = str.Split(pattern);
            for (int i = 1; i < temp.Length; i++)
            {
                times++;
            }
            return times;
        }

        public static byte[] GetBytes(string fileName)
        {
            byte[] result;
            try
            {
                result = File.ReadAllBytes(fileName);
            }
            catch
            {
                throw;
            }
            return result;
        }

        //从技术管理系统服务器端读取EnergyTemplet文件夹目录下的能源模板文件
        public static byte[] ReadEnergyTemplemFromKnowledge(string fileName)
        {
            string currDir = AppDomain.CurrentDomain.BaseDirectory+"EnergyTemplet\\";
            string disk = currDir.Substring(0, currDir.IndexOf(":") + 1);
            string kn_dir = disk + "\\KnowledgeConsole\\服务控制台程序\\" + "EnergyTemplet\\";
            byte[] result;
            try
            {
                string file = kn_dir + fileName;
                result = File.ReadAllBytes(file);
            }
            catch
            {
                throw;
            }
            return result;
        }

        //从服务器端读取EnergyTemplet文件夹目录下的能源模板文件
        public static byte[] ReadEnergTemplet(string fileName)
        {
            string currDir = AppDomain.CurrentDomain.BaseDirectory+"EnergyTemplet\\";
            byte[] result;
            try
            {
                string file = currDir + fileName;
                result = File.ReadAllBytes(file);
            }
            catch
            {
                throw;
            }
            return result;
        }

        //从服务器端读取EnergyTemplet文件夹目录下的能源模板文件
        public static byte[] ReadFileFromServer(string filePath)
        {
            string currDir = AppDomain.CurrentDomain.BaseDirectory;
            byte[] result;
            try
            {
                string file = currDir + filePath;
                result = File.ReadAllBytes(file);
            }
            catch
            {
                throw;
            }
            return result;
        }

        public static void SaveAttachToLocal(string fileName, byte[] source)
        {
            try
            {
                File.WriteAllBytes(fileName, source);
            }
            catch
            {
                throw;
            }
        }

        public static void SaveFileToServer(string fileName, byte[] source)
        {
            string currDir = AppDomain.CurrentDomain.BaseDirectory + "EnergyTemplet\\";
            try
            {
                File.WriteAllBytes(currDir+fileName, source);
            }
            catch
            {
                throw;
            }
        }

        public static void SaveFileToServerByResult(string fileName, byte[] source)
        {
            string currDir = AppDomain.CurrentDomain.BaseDirectory + "ReportResult\\";
            try
            {
                File.WriteAllBytes(currDir + fileName, source);
            }
            catch
            {
                throw;
            }
        }

        //删除文件
        public static void DeleteFileFromServer(string fileName)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "EnergyTemplet\\";
            try
            {
                FileInfo fi = new FileInfo(path + fileName);
                fi.Delete();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static bool IfExistFileInServer(string fileName)
        {          
            string currDir = AppDomain.CurrentDomain.BaseDirectory + "EnergyTemplet\\";
            FileInfo fFile = new FileInfo(currDir+fileName);

            return fFile.Exists;
        }

        public static bool IfExistFiles(string filePath)
        {
            string currDir = AppDomain.CurrentDomain.BaseDirectory ;
            FileInfo fFile = new FileInfo(currDir + filePath);

            return fFile.Exists;
        }

        

        public static bool IfExistFileInKnowlodgeServer(string fileName)
        {
            string currDir = AppDomain.CurrentDomain.BaseDirectory;
            string disk = currDir.Substring(0, currDir.IndexOf(":") + 1);
            string kn_dir = disk + "\\KnowledgeConsole\\服务控制台程序\\" + "EnergyTemplet\\";
            FileInfo fFile = new FileInfo(kn_dir + fileName);

            return fFile.Exists;
        }

        //文件改名
        public static void ChangeFileName(string oldFileName, string newFileName)
        {
            string currDir = AppDomain.CurrentDomain.BaseDirectory + "EnergyTemplet\\";
            File.Move(currDir + oldFileName + ".flx", currDir + newFileName + ".flx");
        }

        public static bool AttachLengthCompare(string fileName, decimal compareFileLength)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(fileName);
                long length = fileInfo.Length;
                decimal fileLength = ((decimal)length) / 1024M / 1024M;
                if (fileLength > compareFileLength)
                {
                    return true;
                }
            }
            catch 
            {
                throw;
            }

            return false;
        }

        public static string TransToString(object o)
        {
            string str = "";
            if (o != null)
            {
                str = o.ToString();
            }
            return str;
        }
        #endregion

        #region 业务中涉及的方法

        /// <summary>
        /// 判断时间的选择是否和视图类型相同
        /// </summary>
        /// <param name="viewtype">1:"每日采集", 2:"每月采集", 3:"季度采集", 4:"每年采集"</param>
        /// <returns></returns>
        public static bool IfRightTime(string dimCode,string viewtype) 
        {
            bool selected = true;
            string timeType = "";
            //通过dimCode得出该时间是月、季度、还是年
            if (dimCode.Length == 4)
            {
                timeType = "4";
            }
            else if (dimCode.Length == 5)
            {
                timeType = "3";
            }
            else if (dimCode.Length == 7)
            {
                timeType = "2";
            }
            else if (dimCode.Length == 9)
            {
                timeType = "5";
            }

            if (!timeType.Equals(viewtype))
            {
                selected = false;
            }

            return selected;
        }

        //通过传入时间(2007-9-12)和报表类型中取得对应的维度代码
        //返回格式为: 四位年+一位季度+两位月份+两位日
        public static string getTimeCodeByType(string currDate, int type)
        {
            string code = "";
            string[] str = currDate.Split('-');
            string year = str[0].ToString();
            string month = str[1].ToString();
            string day = str[2].ToString();

            if (month.Length == 1)
            {
                month = "0" + month;
            }

            if (day.Length == 1)
            {
                day = "0" + day;
            }

            string quarter = "";
            if ("01".Equals(month) || "02".Equals(month) || "03".Equals(month))
            {
                quarter = "1";
            }
            else if ("04".Equals(month) || "05".Equals(month) || "06".Equals(month))
            {
                quarter = "2";
            }
            else if ("07".Equals(month) || "08".Equals(month) || "09".Equals(month))
            {
                quarter = "3";
            }
            else if ("10".Equals(month) || "11".Equals(month) || "12".Equals(month))
            {
                quarter = "4";
            }

            switch (type)
            {
                case 1://年
                    code = year;
                    break;
                case 2://季度
                    code = year + quarter;
                    break;
                case 3://月
                    code = year + quarter + month;
                    break;
                case 4://日
                    code = year + quarter + month + day;
                    break;
            }
            return code;
        }

        /// <summary>
        /// 时间跨度计算
        /// </summary>
        /// <param name="type">1:上月,2:上季度,3:上年,4:本季度,5:本年度, 6:一月, 7:二月 ....</param>
        /// <param name="currDateCode">格式为“两位年+一位季度+两位月份”，2007108</param>
        /// <returns></returns>
        public static string GetCalDateCode(string currDateCode, int type)
        {
            string get_code = "";
            string year = "";
            string quarter = "";
            string month = "";

            switch (type)
            {
                case 1:
                    year = currDateCode.Substring(0, 4);
                    quarter = currDateCode.Substring(4, 1);
                    month = currDateCode.Substring(5);
                    if ("1".Equals(month))
                    {
                        year = (int.Parse(year) - 1) + "";
                        quarter = "4";
                    }
                    else if ("04".Equals(month) || "07".Equals(month) || "4".Equals(month) || "7".Equals(month) || "10".Equals(month))
                    {
                        quarter = (int.Parse(quarter) - 1) + "";
                    }

                    if ("1".Equals(month))
                    {
                        month = "12";
                    }
                    else {
                        month = (int.Parse(month) - 1) + "";
                    }

                    if (month.Length == 1)
                    {
                        get_code = year + quarter + "0" + month;
                    }
                    else
                    {
                        get_code = year + quarter + month;
                    }
                    break;
                case 2:
                    year = currDateCode.Substring(0, 4);
                    quarter = currDateCode.Substring(4, 1);
                    if (currDateCode.Length > 5)
                    {
                        month = currDateCode.Substring(5);
                    }
                    if ("1".Equals(quarter))
                    {
                        year = (int.Parse(year) - 1) + "";
                        quarter = "4";
                    }
                    else {
                        quarter = (int.Parse(quarter) - 1) + "";
                    }
                    
                    get_code = year + quarter + month;
                    break;
                case 3:
                    year = currDateCode.Substring(0, 4);
                    string before_year = (int.Parse(year) - 1) + "";
                    string suffix = currDateCode.Substring(4);
                    get_code = before_year + suffix;
                    break;
                case 4:
                    quarter = currDateCode.Substring(0, 5);
                    get_code = quarter;
                    break;
                case 5:
                    year = currDateCode.Substring(0, 4);
                    get_code = year;
                    break;
                case 6://一月
                    year = currDateCode.Substring(0, 4);
                    get_code = year+"101";
                    break;
                case 7://二月
                    year = currDateCode.Substring(0, 4);
                    get_code = year + "102";
                    break;
                case 8://三月
                    year = currDateCode.Substring(0, 4);
                    get_code = year + "103";
                    break;
                case 9://四月
                    year = currDateCode.Substring(0, 4);
                    get_code = year + "204";
                    break;
                case 10://五月
                    year = currDateCode.Substring(0, 4);
                    get_code = year + "205";
                    break;
                case 11://六月
                    year = currDateCode.Substring(0, 4);
                    get_code = year + "206";
                    break;
                case 12://七月
                    year = currDateCode.Substring(0, 4);
                    get_code = year + "307";
                    break;
                case 13://八月
                    year = currDateCode.Substring(0, 4);
                    get_code = year + "308";
                    break;
                case 14://九月
                    year = currDateCode.Substring(0, 4);
                    get_code = year + "309";
                    break;
                case 15://十月
                    year = currDateCode.Substring(0, 4);
                    get_code = year + "410";
                    break;
                case 16://十一月
                    year = currDateCode.Substring(0, 4);
                    get_code = year + "411";
                    break;
                case 17://十二月
                    year = currDateCode.Substring(0, 4);
                    get_code = year + "412";
                    break;
                default:
                    break;
            }

            return get_code;
        }

        /// <summary>
        /// 能源公式校验
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>-1：非法字符，-2：[]个数不等，-3：()个数不等，4：:的个数和[的个数不对</returns>
        public static int CheckEnergyExpress(string express) 
        {
            int return_value = 0; ;
            string basic_char = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789()[]+-*./:";

            int num_left_1 = 0;//"["的个数
            int num_right_1 = 0;//"]"的个数
            int num_left_2 = 0;//"("的个数
            int num_right_2 = 0;//")"的个数
            int num_colon = 0;//":"的个数

            //判断是否存在非法字符
            char[] c = express.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                string curr_char = c[i].ToString();
                if (!basic_char.Contains(curr_char))
                {
                    return -1;
                }
                if (curr_char.Equals("["))
                {
                    num_left_1 = num_left_1 + 1;
                }
                if (curr_char.Equals("]"))
                {
                    num_right_1 = num_right_1 + 1;
                }
                if (curr_char.Equals("("))
                {
                    num_left_2 = num_left_2 + 1;
                }
                if (curr_char.Equals(")"))
                {
                    num_right_2 = num_right_2 + 1;
                }
                if (curr_char.Equals(":"))
                {
                    num_colon = num_colon + 1;
                }
            }


            if (num_left_1 != num_right_1)
            {
                return -2;
            }

            if (num_left_2 != num_right_2)
            {
                return -3;
            }

            if (num_left_1 != num_colon && num_colon != 0)
            {
                return -4;
            }

            return return_value;

        }

        /// <summary>
        /// 指标管理中的公式校验
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>-1：非法字符，-2：[]个数不等，-3：()个数不等，4：:的个数和[的个数不对</returns>
        public static int CheckEnergyExpressByZb(string express)
        {
            int return_value = 0; ;
            string basic_char = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789()[]+-*./_一二三四五六七八九十本上月时间季度年:";

            int num_left_1 = 0;//"["的个数
            int num_right_1 = 0;//"]"的个数
            int num_left_2 = 0;//"("的个数
            int num_right_2 = 0;//")"的个数
            int num_colon = 0;//":"的个数

            //判断是否存在非法字符
            char[] c = express.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                string curr_char = c[i].ToString();
                if (!basic_char.Contains(curr_char))
                {
                    return -1;
                }
                if (curr_char.Equals("["))
                {
                    num_left_1 = num_left_1 + 1;
                }
                if (curr_char.Equals("]"))
                {
                    num_right_1 = num_right_1 + 1;
                }
                if (curr_char.Equals("("))
                {
                    num_left_2 = num_left_2 + 1;
                }
                if (curr_char.Equals(")"))
                {
                    num_right_2 = num_right_2 + 1;
                }
                if (curr_char.Equals(":"))
                {
                    num_colon = num_colon + 1;
                }
            }


            if (num_left_1 != num_right_1)
            {
                return -2;
            }

            if (num_left_2 != num_right_2)
            {
                return -3;
            }

            if (num_left_1 != num_right_1 && num_colon != 0)
            {
                return -4;
            }

            return return_value;

        }

        //通过字母代号，转换成数值
        public static int GetSignIndex(string str)
        {
            int index = 0;
            for (int i = 0; i < Init_str.Length; i++)
            {
                if (str.Equals(Init_str[i].ToString()))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        //把代号拆分行号(type:1)或者列号(type:2)
        public static string SplitStr(string str, int type)
        {
            string return_str = "";
            string num = "0123456789";
            char[] c = str.ToCharArray();
            int start_num_index = 0;
            for (int i = 0; i < c.Length; i++)
            {
                string curr_char = c[i].ToString();
                if (num.Contains(curr_char))
                {
                    start_num_index = i;
                    break;
                }
            }
            if (type == 1)
            {
                return_str = str.Substring(start_num_index);
            }
            if (type == 2)
            {
                return_str = str.Substring(0, start_num_index);
            }
            return return_str;
        }

        /// <summary>
        /// 根据当前表达式和已有值进行计算，返回带值的表达式
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>当前表达式的值</returns>
        public static string CalExpress(string currExpress, Hashtable ht_value, Hashtable ht_cal)
        {
            char[] patten = { '[', ']' };//分隔符
            string[] temp = currExpress.Split(patten);
            IList signList = new ArrayList();
            for (int i = 1; i < temp.Length; i = i + 2)
            {
                signList.Add(temp[i]);
            }
            foreach (string str in signList)
            {
                //定义了计算表达式，而此次没有计算结果
                if (ht_value["[" + str + "]"] == null && ht_cal[str] != null)
                {
                    return "-1";
                }
                else
                {
                    string value = "";
                    if (ht_value["[" + str + "]"] == null && ht_cal[str] == null)
                    {
                        value = "0";
                    }
                    else
                    {
                        value = ht_value["[" + str + "]"].ToString();
                        if ("".Equals(value))
                        {
                            value = "0";
                        }
                    }
                    currExpress = currExpress.Replace("[" + str + "]", value);
                }
            }
            return currExpress;
        }

        /// <summary>
        /// 自由报表定义中，增加行或列时，公式的重新设置
        /// 算法描述：
        /// 1：通过开始字符和结束字符，可以判定--行列类型，--插入还是删除
        /// ([A:B2])/([F:AA8])
        /// </summary>
        /// <param name="start_str">操作的开始字符，如行为数字，如“3”，如列为字符“E”</param>
        /// <param name="end_str">操作的结束字符，如行为数字，如“3”，如列为字符“E”</param>
        /// <param name="ht_express">主键ID和表达式的对应</param>
        /// <returns></returns>
        public static Hashtable ReSetExpress(string start_str,string end_str,Hashtable ht_express)
        {
            Hashtable ht_trans_exp = new Hashtable();
            int type = CheckSetType(start_str, end_str);
            foreach (string id_str in ht_express.Keys)
            {
                string curr_express = ht_express[id_str].ToString();
                string new_express = GetNewExpress(start_str, curr_express, type);
                ht_trans_exp.Add(id_str, new_express);
            }
            return ht_trans_exp;
        }

        
        public static string GetNewExpress(string start_str,string curr_express, int type)
        {
            string new_express = "";
            char[] pattern ={ '[', ']' };
            string[] temp = curr_express.Split(pattern);
            for (int i = 1; i < temp.Length; i = i + 2)
            {
                new_express += temp[i - 1].ToString();
                string _t = temp[i];
                //取单元格的坐标，如A：B21
                string prefix = _t.Substring(0, _t.IndexOf(":") + 1);
                string cell_sign = _t.Substring(_t.IndexOf(":") + 1);
                string _old = "";
                string _new = "";

                if (KnowledgeUtil.IfHaveUpperLetter(cell_sign) == true)
                {
                    switch (type)
                    {
                        //插入行
                        case 1:
                            //取行号
                            _old = SplitStr(cell_sign, 1);
                            _new = _old;
                            if (int.Parse(_old) >= int.Parse(start_str))
                            {
                                _new = (int.Parse(_old) + 1) + "";
                            }
                            break;
                        //删除行
                        case 2:

                            //取行号
                            _old = SplitStr(cell_sign, 1);
                            _new = _old;
                            if (int.Parse(_old) >= int.Parse(start_str))
                            {
                                _new = (int.Parse(_old) - 1) + "";
                            }
                            break;
                        //插入列
                        case 3:
                            //取列号
                            _old = SplitStr(cell_sign, 2);
                            _new = _old;
                            if (GetSignIndex(_old) >= GetSignIndex(start_str))
                            {
                                _new = Init_str[GetSignIndex(_old) + 1].ToString();
                            }
                            break;
                        //删除列
                        case 4:
                            //取列号
                            _old = SplitStr(cell_sign, 2);
                            _new = _old;
                            if (GetSignIndex(_old) >= GetSignIndex(start_str))
                            {
                                _new = Init_str[GetSignIndex(_old) - 1].ToString();
                            }
                            break;

                        default:
                            break;
                    }
                }
                string new_cell_sign = "";
                if (_old != null && !"".Equals(_old))
                {
                    new_cell_sign = cell_sign.Replace(_old, _new);
                }
                else
                {
                    new_cell_sign = cell_sign;
                }
                new_express += "[" + prefix + new_cell_sign + "]";
            }
            string suffix = temp[temp.Length - 1].ToString();
            new_express = new_express + suffix;
            return new_express;
        }

        /// <summary>
        /// 能源平衡表，增加行或列时，公式的重新设置
        /// 算法描述：
        /// 1：通过开始字符和结束字符，可以判定--行列类型，--插入还是删除
        /// ([A:B2])/([F:AA8])
        /// </summary>
        /// <param name="tab_type">操作能源类型，如(A：为能耗输入)</param>
        /// <param name="start_str">操作的开始字符，如行为数字，如“3”，如列为字符“E”</param>
        /// <param name="end_str">操作的结束字符，如行为数字，如“3”，如列为字符“E”</param>
        /// <param name="ht_express">主键ID和表达式的对应</param>
        /// <returns></returns>
        public static Hashtable ReSetExpressByEnergy(string tab_type,string start_str, string end_str, Hashtable ht_express)
        {
            Hashtable ht_trans_exp = new Hashtable();
            int type = CheckSetType(start_str, end_str);
            foreach (string id_str in ht_express.Keys)
            {
                string curr_express = ht_express[id_str].ToString();
                string new_express = GetNewExpressByEnergy(tab_type, start_str, curr_express, type);
                ht_trans_exp.Add(id_str, new_express);
            }
            return ht_trans_exp;
        }

        //对如（A14+A15+A16+A17）这样的连加进行处理
        public static string GetNewExpressByEnergy(string tab_type,string start_str, string curr_express, int type)
        {
            string new_express = "";
            char[] pattern ={ '[', ']' };
            string[] temp = curr_express.Split(pattern);
            for (int i = 1; i < temp.Length; i = i + 2)
            {
                new_express += temp[i - 1].ToString();
                string _t = temp[i];
                //取单元格的坐标，如A：B21
                string prefix = _t.Substring(0, _t.IndexOf(":") + 1);
                string cell_sign = _t.Substring(_t.IndexOf(":") + 1);
                string _old = "";
                string _new = "";

                if (_t.IndexOf(tab_type) != -1)
                {                                                          
                    switch (type)
                    {
                        //插入行
                        case 1:
                            //取行号
                            _old = SplitStr(cell_sign, 1);
                            _new = _old;
                            if (int.Parse(_old) >= int.Parse(start_str))
                            {
                                _new = (int.Parse(_old) + 1) + "";
                            }
                            break;
                        //删除行
                        case 2:

                            //取行号
                            _old = SplitStr(cell_sign, 1);
                            _new = _old;
                            if (int.Parse(_old) >= int.Parse(start_str))
                            {
                                _new = (int.Parse(_old) - 1) + "";
                            }
                            break;
                        //插入列
                        case 3:
                            //取列号
                            _old = SplitStr(cell_sign, 2);
                            _new = _old;
                            if (GetSignIndex(_old) >= GetSignIndex(start_str))
                            {
                                _new = Init_str[GetSignIndex(_old) + 1].ToString();
                            }
                            break;
                        //删除列
                        case 4:
                            //取列号
                            _old = SplitStr(cell_sign, 2);
                            _new = _old;
                            if (GetSignIndex(_old) >= GetSignIndex(start_str))
                            {
                                _new = Init_str[GetSignIndex(_old) - 1].ToString();
                            }
                            break;

                        default:
                            break;

                    }
                }
                string new_cell_sign = "";
                if (_old != null && !"".Equals(_old))
                {
                    new_cell_sign = cell_sign.Replace(_old, _new);
                }
                else {
                    new_cell_sign = cell_sign;
                }
                new_express += "[" + prefix + new_cell_sign + "]";                
            }
            string suffix = temp[temp.Length-1].ToString();
            new_express = new_express + suffix;
            return new_express;
        }

        /// <summary>
        /// 校验函数的行列，并验证开始字符和结束字符的一致性
        /// 
        /// </summary>
        /// <param name="start_str">操作的开始字符，如行为数字，如“3”，如列为字符“E”</param>
        /// <param name="end_str">操作的结束字符，如行为数字，如“3”，如列为字符“E”</param>
        /// <returns> -1：为行列不一致，1：行+插入，2：行+删除，3：列+插入，4：列+删除 </returns>
        public static int CheckSetType(string start_str, string end_str)
        {
            int type = -1;
            bool start_bool = IfNumber(start_str);
            bool end_bool = IfNumber(end_str);

            if (start_str != null && start_str.Equals("A") && end_str != null && end_str.Equals(""))
            {
                return 4;
            }

            if (start_bool != end_bool)
            {
                return -1;
            }

            //行，即数字
            if (start_bool == true)
            {
                if (int.Parse(start_str) < int.Parse(end_str))
                {
                    type = 1;
                }
                else {
                    type = 2;
                }
            }
            else {
                if (GetSignIndex(start_str) < GetSignIndex(end_str))
                {
                    type = 3;
                }
                else {
                    type = 4;
                }
            }
            
            return type;
        }

        #endregion

        #region 状态描述
        //视图分发状态(ViewDistribute)
        public static int VIEWDIS_CREATE_CODE = 1;
        public static string VIEWDIS_CREATE_NAME = "已生成";

        public static int VIEWDIS_EDIT_CODE = 2;
        public static string VIEWDIS_EDIT_NAME = "编辑中";

        public static int VIEWDIS_SUBMIT_CODE = 3;
        public static string VIEWDIS_SUBMIT_NAME = "已提交";

        //模板录入信息(viewwriteinfo)的状态
        public static int VIEWWRITE_CREATE_CODE = 1;
        public static int VIEWWRITE_SUBMIT_CODE = 2;

        #endregion

        #region 指标模块的基础数据
        //固定维度名称
        public static string TIME_DIM_STR = "时间";
        public static string YWZZ_DIM_STR = "业务组织";

        //时间跨度
        public static string[] TimeSpan = { "时间", "上月", "上季度", "上年", "本季度", "本年度", "一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月","上年十二月" };

        //计算类型
        public static string[] CalculateTypeCode = {"1","2" };
        public static string[] CalculateTypeName = { "累计相加", "累计平均" };
        //时间变量
        public static string[] TimeVarCode = { "101", "102", "103", "204", "205", "206", "307", "308", "309", "410", "411", "412" };
        public static string[] TimeVarName = { "一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月" };
        //维度注册表-来源类型
        public static string[] OriginTypeCode = { "1", "2", "3" };
        public static string[] OriginTypeName = { "系统生成", "外部来源", "手工维护" };
        //指标采集视图-视图类型
        public static string[] ViewTypeCode = { "1", "2", "3", "4", "5" };
        public static string[] ViewTypeName = { "采集视图", "浏览视图", "报表采集视图", "统计报表", "决策支持报表" };
        //模板采集频率
        public static string[] CollectTypeCode = { "1", "2", "3", "4", "5" };
        public static string[] CollectTypeName = { "一次采集", "每月采集", "季度采集", "每年采集", "每日采集" };
        //指标区间类型
        public static string[] ScopeTypeCode = { "1", "2", "3", "4" };
        public static string[] ScopeTypeName = { "开区间()", "闭区间[]", "左开右闭(]", "左闭右开[)" };
        //是否技经指标
        public static string[] IfEconomyCode = { "1"};
        public static string[] IfEconomyName = { "是" };
        //是否显示子项母项
        public static string[] IfDisplaySonMotherCode = { "1" };
        public static string[] IfDisplaySonMotherName = { "是" };
        //是否显示业务组织
        public static string[] IfDisplayYwzzCode = { "1" };
        public static string[] IfDisplayYwzzName = { "是" };
        //是否显示时间
        public static string[] IfDisplayTimeCode = { "1" };
        public static string[] IfDisplayTimeName = { "是" };

        //列号
        public static string[] Init_str = { "", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ" };
        

        #endregion

        #region 能源平衡表基础数据

        //各报表的名字常量
        public static string GGGF_ENERGY = "广钢股份";
        public static string ZG_ENERGY = "珠钢";
        public static string GL_ENERGY = "广铝";
        public static string GT_ENERGY = "广铜";
        public static string ZT_ENERGY = "珠铜";
        public static string TY_ENERGY = "腾业";
        public static string HP_ENERGY = "黄埔";
        public static string DG_ENERGY = "带钢";
        public static string ZH_ENERGY = "综合";

        //模板类型
        public static string[] TempletTypeCode = { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        public static string[] TempletTypeName = { "能耗输入表", "产品产量表", "利润成本表", "煤(焦)成份表", "能源汇总表", "能源平衡明细表", "综合能源汇总表", "综合能源明细表", "能耗指标完成报表" };

        #endregion
    }

    
}
