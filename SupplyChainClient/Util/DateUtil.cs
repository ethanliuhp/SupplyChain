using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;

namespace Application.Business.Erp.SupplyChain.Client.Util
{

    public class DateUtil
    {
        private static char[] chinese = new char[] { '��', 'һ', '��', '��', '��', '��', '��', '��', '��', '��' };
        private static string baseRetract = "  ";//�����ո�

        public static string Num2Chinese(int s)
        {
            //����������ת������.
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
        /// ������ת��Ϊ�������ڣ�ֻȡ���ꡢ��
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string Date2Chinese(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;

            return Num2Chinese(year) + "��" + Num2Chinese(month) + "��";
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
        /// ��ʽ����
        /// </summary>
        /// <param name="mutiple">����</param>
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
        /// ת���ַ����ʽ��������
        /// </summary>
        /// <param name="expression">������ʽ</param>
        /// <param name="digit">����С����λ��</param>
        /// <returns>���ֵ</returns>
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

        //�õ�һ���ַ������ض��ַ��ĸ���
        public static int GetStrCountInChar(string str, char c)
        {
            if (str == null || str == "")
                return 0;
            string[] temp = str.Split(c);
            return temp.Length - 1;
        }

        //��ȡ���
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
