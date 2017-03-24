using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace Application.Business.Erp.SupplyChain.Client.Basic.CommonClass
{
    public class Enums
    {


    }

    public enum EditStyle
    {
        Text = 0,
        DateTime = 1,
        ComboBox = 2
    }

    public enum EditBorderStyle
    {
        None = 0,
        FixedSingle = 1,
    }

    public enum MaskStyle
    {
        None, DateOnly, Decimal, Digit
    };

    public enum MouseStatus
    {
        MouseEnter,
        MouseLeave,
        MouseDown,
        MouseUp
    }

    public enum EnumProjectCurrState
    {
        新开工 = 10,
        在建 = 0,
        收尾 = 1,
        完工未结算 = 2,
        完工已结算 = 3,
        停工 = 4,
        无效 = 20
    }

    ///// <summary>
    ///// 枚举类型操作类
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    //public class EnumUtil<T>
    //{
    //    private static string GetDescription(T enumValue, string defDesc)
    //    {

    //        FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

    //        if (null != fi)
    //        {
    //            object[] attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
    //            if (attrs != null && attrs.Length > 0)
    //                return ((DescriptionAttribute)attrs[0]).Description;
    //        }

    //        return defDesc;
    //    }
    //    /// <summary>
    //    /// 返回枚举值的描述
    //    /// </summary>
    //    /// <param name="enumValue"></param>
    //    /// <returns></returns>
    //    public static string GetDescription(T enumValue)
    //    {
    //        return GetDescription(enumValue, string.Empty);
    //    }
    //    /// <summary>
    //    /// 返回描述的枚举值
    //    /// </summary>
    //    /// <param name="description"></param>
    //    /// <returns></returns>
    //    public static T FromDescription(string description)
    //    {
    //        Type t = typeof(T);
    //        foreach (FieldInfo fi in t.GetFields())
    //        {
    //            object[] attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
    //            if (attrs != null && attrs.Length > 0)
    //            {
    //                foreach (DescriptionAttribute attr in attrs)
    //                {
    //                    if (attr.Description.Equals(description))
    //                        return (T)fi.GetValue(null);
    //                }
    //            }
    //        }
    //        return default(T);
    //    }
    //    /// <summary>
    //    /// 获得枚举类型的所有的描述
    //    /// </summary>
    //    /// <returns></returns>
    //    public static IList<string> GetDescriptions()
    //    {
    //        Type t = typeof(T);
    //        IList<string> lstReturn = new List<string>();
    //        foreach (FieldInfo fi in t.GetFields())
    //        {
    //            DescriptionAttribute[] attrs = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
    //            if (attrs != null && attrs.Length > 0)
    //            {
    //                foreach (DescriptionAttribute attr in attrs)
    //                {
    //                    lstReturn.Add(attr.Description);
    //                }
    //            }
    //        }
    //        return lstReturn;
    //    }
    //}
}
