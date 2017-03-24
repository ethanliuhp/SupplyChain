using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.Main
{
    public static class CommonPlus
    {
        /// <summary>
        /// 找到Form中包含指定标识的控件
        /// </summary>
        /// <param name="form"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public static Control[] FindControlsByTag(this Form form, string tagName)
        {
            List<Control> list = new List<Control>();
            var controlArr = form.Controls;
            foreach (var item in controlArr)
            {
                if (((Control)item).Tag + string.Empty == tagName)
                {
                    list.Add((Control)item);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 将IList转化为List<T>类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> Select<T>(this IList list) where T : class
        {
            List<T> result = new List<T>();
            foreach (var item in list)
            {
                result.Add(item as T);
            }
            return result;
        }

        /// <summary>
        /// 将数组拼接为条件表达式
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string JoinArray(params string[] param)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < param.Length; i++)
            {
                if (i == param.Length - 1)
                {
                    sb.Append("'" + param[i] + "'");
                }
                else
                {
                    sb.Append("'" + param[i] + "',");
                }
            }
            return sb.ToString();
        }
    }
}
