using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;

namespace Application.Business.Erp.SupplyChain.Client.Util
{

    public class DateUtil
    {
        private static char[] chinese = new char[] { '零', '一', '二', '三', '四', '五', '六', '七', '八', '九' };
        private static string baseRetract = "  ";//两个空格

        public static string Num2Chinese(int s)
        {
            //将单个数字转成中文.
            string temp = "" + s;
            int slen = temp.Length;
            string result = "";
            for (int i = 0; i < slen; i++)
            {
                result += chinese.GetValue(int.Parse(temp.Substring(i, 1)));
            }
            return result;
        }

        /// <summary>
        /// 把日期转化为中文日期，只取了年、月
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string Date2Chinese(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;

            return Num2Chinese(year) + "年" + Num2Chinese(month) + "月";
        }

        public static bool IsNumber(string numStr)
        {
            string valid = "0123456789.";
            string temp;
            for (int i = 0; i < numStr.Length; i++)
            {
                temp = numStr.Substring(i, 1);
                if (valid.IndexOf(temp) < 0) return false;
            }
            return true;
        }

        /// <summary>
        /// 格式缩进
        /// </summary>
        /// <param name="mutiple">倍数</param>
        /// <returns></returns>
        public static string retractBlank(int mutiple)
        {
            for (int i = 1; i < mutiple; i++)
            {
                baseRetract += baseRetract;
            }
            return baseRetract;
        }

        /// <summary>
        /// 转换字符表达式，计算结果
        /// </summary>
        /// <param name="expression">计算表达式</param>
        /// <param name="digit">返回小数点位数</param>
        /// <returns>结果值</returns>
        public static string CalculateExpression(string expression, int digit)
        {
            DataTable dt = new DataTable();
            string value = dt.Compute(expression, "").ToString();
            if (value != null && !"".Equals(value))
            {
                value = Math.Round(double.Parse(value), digit) + "";
            }
            return value;
        }

        //得到一个字符串中特定字符的个数
        public static int GetStrCountInChar(string str, char c)
        {
            if (str == null || str == "")
                return 0;
            string[] temp = str.Split(c);
            return temp.Length - 1;
        }

        //截取金额
        public static string GetStrMoneyInchar(string str)
        {
            if (str.Length > 6)
            {
                str = str.Substring(str.Length - 6);
            }
            return str;
        }

    }

}
