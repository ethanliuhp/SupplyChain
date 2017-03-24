using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.Financial.InitialData.Domain;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Domain;
//using Application.Business.Erp.Financial.BasicAccount.DailyOperation.Domain;
using System.Data;
using System.Collections;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.InitStruct;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Service;

namespace Application.Business.Erp.Financial.FIUtils
{
    public class FinanceUtil
    {
        /// <summary>
        /// 获取枚举类型个数
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static int GetEnumCnt(Type enumType)
        {
            return (Enum.GetValues(enumType) as Array).Length;
        }

        #region 凭证限制类型
        /// <summary>
        /// 获得凭证限制类型Int
        /// </summary>
        /// <param name="nowType">限制类型</param>
        /// <returns>int</returns>
        //public static int GetResTypeInt(RestrictType nowType)
        //{
        //    switch (nowType)
        //    {
        //        case RestrictType.NoLimits:
        //            return 0;
        //        case RestrictType.HasDebit:
        //            return 1;
        //        case RestrictType.HasNoDebit:
        //            return 2;
        //        case RestrictType.HasCredit:
        //            return 3;
        //        case RestrictType.HasNoCredit:
        //            return 4;
        //        case RestrictType.HasVoucher:
        //            return 5;
        //        case RestrictType.HasNoVoucher:
        //            return 6;
        //        default:
        //            return 0;
        //    }
        //}

        /// <summary>
        /// 获得凭证类型描述
        /// </summary>
        /// <param name="nowType">限制类型</param>
        /// <returns>string</returns>
        //public static string GetResTypeStr(RestrictType nowType)
        //{
        //    switch (nowType)
        //    {
        //        case RestrictType.NoLimits:
        //            return "无限制";
        //        case RestrictType.HasDebit:
        //            return "借方必有";
        //        case RestrictType.HasNoDebit:
        //            return "借方必无";
        //        case RestrictType.HasCredit:
        //            return "贷方必有";
        //        case RestrictType.HasNoCredit:
        //            return "贷方必无";
        //        case RestrictType.HasVoucher:
        //            return "凭证必有";
        //        case RestrictType.HasNoVoucher:
        //            return "凭证必无";
        //        default:
        //            return "无限制";
        //    }
        //}

        /// <summary>
        /// 根据Int获得凭证限制类型描述
        /// </summary>
        /// <param name="nowTypeId"></param>
        /// <returns></returns>
        //public static string GetResTypeStrByInt(int nowTypeId)
        //{
        //    switch (nowTypeId)
        //    {
        //        case 0:
        //            return "无限制";
        //        case 1:
        //            return "借方必有";
        //        case 2:
        //            return "借方必无";
        //        case 3:
        //            return "贷方必有";
        //        case 4:
        //            return "贷方必无";
        //        case 5:
        //            return "凭证必有";
        //        case 6:
        //            return "凭证必无";
        //        default:
        //            return "无限制";
        //    }
        //}

        #endregion

        #region 凭证条件类型
        /// <summary>
        /// 获取凭证条件数值
        /// </summary>
        /// <param name="nowType"></param>
        /// <returns></returns>
        public static int GetOutPutInt(output nowType)
        {
            switch (nowType)
            {
                case output.VouDate:
                    return 0;
                case output.VouNO:
                    return 1;
                case output.VouType:
                    return 2;
                default:
                    return 0;
            }
        }
        /// <summary>
        /// 获取凭证条件字符
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static string GetOutPutStr(output nowType)
        {
            switch (nowType)
            {
                case output.VouDate:
                    return "凭证号";
                case output.VouNO:
                    return "凭证类型";
                case output.VouType:
                    return "凭证日期";
                default:
                    return "凭证号";
            }
        }
        /// <summary>
        /// 获取凭证条件描述
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static string GetOutPutStrByInt(int nowTypeId)
        {
            switch (nowTypeId)
            {
                case 0:
                    return "凭证号";
                case 1:
                    return "凭证类型";
                case 2:
                    return "凭证日期";
                default:
                    return "凭证号";
            }
        }
        #endregion


        #region 摘要类型
        /// <summary>
        /// 获得摘要类型数值
        /// </summary>
        /// <param name="nowType">摘要类型</param>
        /// <returns>int</returns>
        public static int GetSummaryTypeInt(SummaryType nowType)
        {
            switch (nowType)
            {
                case SummaryType.FiscalNoun:
                    return 0;
                case SummaryType.PersonName:
                    return 1;
                case SummaryType.PlaceName:
                    return 2;
                case SummaryType.GoodsName:
                    return 3;
                case SummaryType.ClientName:
                    return 4;
                case SummaryType.Others:
                    return 5;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 获得摘要类型字符串
        /// </summary>
        /// <param name="nowType"></param>
        /// <returns>string</returns>
        public static string GetSummaryTypeStr(SummaryType nowType)
        {
            switch (nowType)
            {
                case SummaryType.FiscalNoun:
                    return "会计名词";
                case SummaryType.PersonName:
                    return "人名";
                case SummaryType.PlaceName:
                    return "地名";
                case SummaryType.GoodsName:
                    return "商品物资";
                case SummaryType.ClientName:
                    return "客户";
                case SummaryType.Others:
                    return "其他";
                default:
                    return "会计名词";
            }
        }

        /// <summary>
        /// 根据Int返回摘要描述
        /// </summary>
        /// <param name="nowTypeId">摘要Int</param>
        /// <returns></returns>
        public static string GetSummaryTypeStrByInt(int nowTypeId)
        {
            switch (nowTypeId)
            {
                case 0:
                    return "会计名词";
                case 1:
                    return "人名";
                case 2:
                    return "地名";
                case 3:
                    return "商品物资";
                case 4:
                    return "客户";
                case 5:
                    return "其他";
                default:
                    return "会计名词";
            }
        }

        /// <summary>
        /// 根据Int返回摘要
        /// </summary>
        /// <param name="nowTypeId">摘要Int</param>
        /// <returns></returns>
        public static SummaryType GetSummaryTypeByInt(int nowTypeId)
        {
            switch (nowTypeId)
            {
                case 0:
                    return SummaryType.FiscalNoun;
                case 1:
                    return SummaryType.PersonName;
                case 2:
                    return SummaryType.PlaceName;
                case 3:
                    return SummaryType.GoodsName;
                case 4:
                    return SummaryType.ClientName;
                case 5:
                    return SummaryType.Others;
                default:
                    return SummaryType.FiscalNoun;
            }
        }

        #endregion

        #region 摘要类型2
        /// <summary>
        /// 获得摘要类型数值
        /// </summary>
        /// <param name="nowType">摘要类型</param>
        /// <returns>int</returns>
        public static int GetSumTypeInt(CommSummaryType nowType)
        {
            switch (nowType)
            {
                case CommSummaryType.PublicSummary:
                    return 0;
                case CommSummaryType.PrivateSummary:
                    return 1;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 获得摘要类型字符串
        /// </summary>
        /// <param name="nowType"></param>
        /// <returns>string</returns>
        public static string GetSumTypeStr(CommSummaryType nowType)
        {
            switch (nowType)
            {
                case CommSummaryType.PublicSummary:
                    return "公用摘要";
                case CommSummaryType.PrivateSummary:
                    return "个人摘要";
                default:
                    return "公用摘要";
            }
        }

        /// <summary>
        /// 根据Int返回摘要描述
        /// </summary>
        /// <param name="nowTypeId">摘要Int</param>
        /// <returns></returns>
        public static string GetCommSumTypeStrByInt(int nowTypeId)
        {
            switch (nowTypeId)
            {
                case 0:
                    return "公用摘要";
                case 1:
                    return "个人摘要";
                default:
                    return "公用摘要";
            }
        }

        /// <summary>
        /// 根据Int返回摘要
        /// </summary>
        /// <param name="nowTypeId">摘要Int</param>
        /// <returns></returns>
        public static CommSummaryType GetCommSumTypeByInt(int nowTypeId)
        {
            switch (nowTypeId)
            {
                case 0:
                    return CommSummaryType.PublicSummary;
                case 1:
                    return CommSummaryType.PrivateSummary;
                default:
                    return CommSummaryType.PublicSummary;
            }
        }

        #endregion
        #region 科目类型
        /// <summary>
        /// 获得科目类型Int
        /// </summary>
        /// <param name="nowType">科目类型</param>
        /// <returns>int</returns>
        public static int GetAccountTypeInt(AccountType nowType)
        {
            switch (nowType)
            {
                case AccountType.Asserts:
                    return 0;
                case AccountType.Liabilities:
                    return 1;
                case AccountType.Interests:
                    return 2;
                case AccountType.Cost:
                    return 3;
                case AccountType.ProfitAndLoss:
                    return 4;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 获得科目类型字符
        /// </summary>
        /// <param name="nowType">科目类型</param>
        /// <returns>string</returns>
        public static string GetAccountTypeStr(AccountType nowType)
        {
            switch (nowType)
            {
                case AccountType.Asserts:
                    return "资产";
                case AccountType.Liabilities:
                    return "负债";
                case AccountType.Together:
                    return "共同";
                case AccountType.Interests:
                    return "所有者权益";
                case AccountType.Cost:
                    return "成本";
                case AccountType.ProfitAndLoss:
                    return "损益";
                default:
                    return "全部";
            }
        }

        /// <summary>
        /// 根据Int获得科目类型描述
        /// </summary>
        /// <param name="nowTypeId">科目类型Int</param>
        /// <returns>string</returns>
        public static string GetAccountTypeStrByInt(int nowTypeId)
        {
            switch (nowTypeId)
            {
                case 0:
                    return "资产";
                case 1:
                    return "负债";
                case 2:
                    return "共同";
                case 3:
                    return "所有者权益";
                case 4:
                    return "成本";
                case 5:
                    return "损益";
                default:
                    return "全部";
            }
        }

        public static AccountType GetAccountType(int nowTypeId)
        {
            switch (nowTypeId)
            {
                case 0:
                    return AccountType.Asserts;
                case 1:
                    return AccountType.Liabilities;
                case 2:
                    return AccountType.Together;
                case 3:
                    return AccountType.Interests;
                case 4:
                    return AccountType.Cost;
                case 5:
                    return AccountType.ProfitAndLoss;
                default:
                    return AccountType.ALL;
            }
        }

        public static string GetAccountTypeByValue(int i)
        {
            string nowType = Enum.GetName(typeof(AccountType), i);
            switch (nowType)
            {
                case "Asserts":
                    return "资产";
                case "Cost":
                    return "成本";
                case "Interests":
                    return "所有者权益";
                case "Liabilities":
                    return "负债";
                case "ProfitAndLoss":
                    return "损益";
                case "Together":
                    return "共同";
                default:
                    return "全部";
            }

        }

        #endregion

        #region 帐簿格式
        /// <summary>
        /// 获取帐簿格式数值
        /// </summary>
        /// <param name="nowType">帐簿格式</param>
        /// <returns>int</returns>
        public static int GetBookStyleInt(BookStyle nowType)
        {
            switch (nowType)
            {
                case BookStyle.Amount:
                    return 0;
                case BookStyle.ForeignAmount:
                    return 1;
                case BookStyle.QuantityAmount:
                    return 2;
                case BookStyle.ForQuanAmount:
                    return 3;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 获取帐簿格式字符
        /// </summary>
        /// <param name="nowType">帐簿格式</param>
        /// <returns>string</returns>
        public static string GetBookStyleStr(BookStyle nowType)
        {
            switch (nowType)
            {
                case BookStyle.Amount:
                    return "金额式";
                case BookStyle.ForeignAmount:
                    return "外币金额式";
                case BookStyle.QuantityAmount:
                    return "数量金额式";
                case BookStyle.ForQuanAmount:
                    return "外币数量式";
                default:
                    return "金额式";
            }
        }

        /// <summary>
        /// 根据Int获取帐簿格式描述
        /// </summary>
        /// <param name="nowTypeId">帐簿格式Int</param>
        /// <returns></returns>
        public static string GetBookStyleStrByInt(int nowTypeId)
        {
            switch (nowTypeId)
            {
                case 0:
                    return "金额式";
                case 1:
                    return "外币金额式";
                case 2:
                    return "数量金额式";
                case 3:
                    return "外币数量式";
                default:
                    return "金额式";
            }
        }

        public static BookStyle GetBookStyleById(int nowTypeId)
        {
            switch (nowTypeId)
            {
                case 0:
                    return BookStyle.Amount;
                case 1:
                    return BookStyle.ForeignAmount;
                case 2:
                    return BookStyle.QuantityAmount;
                case 3:
                    return BookStyle.ForQuanAmount;
                default:
                    return BookStyle.Amount;
            }
        }

        #endregion

        #region 现金科目
        /// <summary>
        /// 获取现金科目数值
        /// </summary>
        /// <param name="nowType">现金科目</param>
        /// <returns>int</returns>
        public static int GetCashAccTitleInt(CashAccTitle nowType)
        {
            switch (nowType)
            {
                case CashAccTitle.NotCash:
                    return 0;
                case CashAccTitle.CashTitle:
                    return 1;
                case CashAccTitle.BankTitle:
                    return 2;
                case CashAccTitle.OtherCashTitle:
                    return 3;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 获取现金科目字符
        /// </summary>
        /// <param name="nowType">现金科目</param>
        /// <returns>string</returns>
        public static string GetCashAccTitleStr(CashAccTitle nowType)
        {
            switch (nowType)
            {
                case CashAccTitle.NotCash:
                    return "非现金及现金等价物";
                case CashAccTitle.CashTitle:
                    return "现金科目";
                case CashAccTitle.BankTitle:
                    return "银行存款科目";
                case CashAccTitle.OtherCashTitle:
                    return "其他现金及现金等价物";
                default:
                    return "非现金及现金等价物";
            }
        }

        /// <summary>
        /// 根据Int获取现金科目字符
        /// </summary>
        /// <param name="nowTypeId">现金科目Int</param>
        /// <returns>string</returns>
        public static string GetCashAccTitleStrByInt(int nowTypeId)
        {
            switch (nowTypeId)
            {
                case 0:
                    return "非现金及现金等价物";
                case 1:
                    return "现金科目";
                case 2:
                    return "银行存款科目";
                case 3:
                    return "其他现金及现金等价物";
                default:
                    return "非现金及现金等价物";
            }
        }

        public static CashAccTitle GetCashAccTitleByInt(int nowTypeId)
        {
            switch (nowTypeId)
            {
                case 0:
                    return CashAccTitle.NotCash;
                case 1:
                    return CashAccTitle.CashTitle;
                case 2:
                    return CashAccTitle.BankTitle;
                case 3:
                    return CashAccTitle.OtherCashTitle;
                default:
                    return CashAccTitle.NotCash;
            }
        }
        #endregion

        #region 凭证状态
        /// <summary>
        /// 获得凭证状态类型Int
        /// </summary>
        /// <param name="nowType">凭证状态</param>
        /// <returns>int</returns>
        //public static int GetVoucherStateInt(VoucherState nowType)
        //{
        //    switch (nowType)
        //    {
        //        case VoucherState.Valid:
        //            return 0;
        //        case VoucherState.CashierSign:
        //            return 1;
        //        case VoucherState.ManagerSign:
        //            return 2;
        //        case VoucherState.AuditPass:
        //            return 3;
        //        case VoucherState.AccountPass:
        //            return 4;
        //        case VoucherState.WrongVoucher:
        //            return 5;
        //        case VoucherState.Cancel:
        //            return 6;
        //        default:
        //            return 0;
        //    }
        //}

        //public static VoucherState GetVoucherStateEnum(int vouState)
        //{
        //    switch (vouState)
        //    { 
        //        case 0:
        //            return VoucherState.Valid;
        //        case 1:
        //            return VoucherState.CashierSign;
        //        case 2:
        //            return VoucherState.ManagerSign;
        //        case 3:
        //            return VoucherState.AuditPass;
        //        case 4:
        //            return VoucherState.AccountPass;
        //        case 5:
        //            return VoucherState.WrongVoucher;
        //        case 6:
        //            return VoucherState.Cancel;
        //        default:
        //            return VoucherState.Valid;
        //    }
        //}

        /// <summary>
        /// 获得凭证状态描述
        /// </summary>
        /// <param name="nowType">凭证状态</param>
        ///// <returns>string</returns>
        //public static string GetVoucherStateStr(VoucherState nowType)
        //{
        //    switch (nowType)
        //    {
        //        case VoucherState.Valid:
        //            return "有效";
        //        case VoucherState.CashierSign:
        //            return "出纳签字";
        //        case VoucherState.ManagerSign:
        //            return "主管签字";
        //        case VoucherState.AuditPass:
        //            return "审批通过";
        //        case VoucherState.AccountPass:
        //            return "记账";
        //        case VoucherState.WrongVoucher:
        //            return "标错";
        //        case VoucherState.Cancel:
        //            return "作废";
        //        default:
        //            return "有效";
        //    }
        //}

        /// <summary>
        /// 根据Int获得凭证状态描述
        /// </summary>
        /// <param name="nowTypeId"></param>
        /// <returns></returns>
        public static string GetVoucherStateStrByInt(int nowTypeId)
        {
            switch (nowTypeId)
            {
                case 0:
                    return "有效";
                case 1:
                    return "出纳签字";
                case 2:
                    return "主管签字";
                case 3:
                    return "审批通过";
                case 4:
                    return "记账";
                case 5:
                    return "标错";
                case 6:
                    return "作废";
                default:
                    return "有效";
            }
        }
        #endregion


        /// <summary>
        /// 格式化凭证号为六位数字格式
        /// </summary>
        /// <param name="VoucherNO"></param>
        /// <returns></returns>
        public static string TransVoucherNo(int VoucherNO)
        {
            int len1 = VoucherNO.ToString().Length;
            string prefix = "";
            if (len1 < 6)
            {
                for (int i = len1; i < 6; i++)
                {
                    prefix += "0";
                }
            }
            return prefix.Trim() + VoucherNO.ToString();
        }

        /// <summary>
        /// 格式凭证号为带凭证字的格式
        /// </summary>
        /// <param name="vh"></param>
        ///// <returns></returns>
        //public static string TransVoucherNoWithTypeMark(Voucher vh)
        //{
        //    int len1 = vh.VoucherNO.ToString().Length;
        //    string prefix = "";
        //    if (len1 < 6)
        //    {
        //        for (int i = len1; i < 6; i++)
        //        {
        //            prefix += "0";
        //        }
        //    }
        //    return vh.VouType.TypeMark+"_"+prefix.Trim() + vh.VoucherNO.ToString();
        //}

        /// <summary>
        /// DataSet数据转换
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static DataSet ConvertDataSetToString(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Columns.Count == 0)
            {
                return null;
            }
                
            DataTable strdt = new DataTable();
            IList dires = new ArrayList();
            int accday = ds.Tables[0].Columns.IndexOf("accday");
            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                if (ds.Tables[0].Columns[i].ColumnName.ToUpper().EndsWith("DIRE"))
                {
                    strdt.Columns.Add(ds.Tables[0].Columns[i].ColumnName, typeof(string));
                    dires.Add(i);
                }
                else
                {
                    strdt.Columns.Add(ds.Tables[0].Columns[i].ColumnName, ds.Tables[0].Columns[i].DataType);
                }                    
            }
            
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                DataRow strdr = strdt.NewRow();
                for (int i = 0; i < strdt.Columns.Count;i++ )
                {
                    if(i==accday )
                    {
                        int dayint=int.Parse(row[i].ToString());
                        if (dayint < 1 || dayint > 31)
                            strdr[i] = DBNull.Value;  
                        else
                            strdr[i] = row[i];
                    }
                    else if (dires.Contains(i) && ds.Tables[0].Columns[i].DataType == typeof(decimal))
                    {
                        if (row[i].ToString().Equals(""))
                        {
                            strdr[i] = DBNull.Value;
                        }
                        else
                        {
                            int dirint = int.Parse(row[i].ToString());
                            if (dirint == 0)
                            {
                                strdr[i] = "平";
                            }
                            else if (dirint == 1)
                            {
                                strdr[i] = "借";
                            }
                            else
                            {
                                strdr[i] = "贷";
                            }
                        }
                    }
                    else if (ds.Tables[0].Columns[i].DataType == typeof(decimal))
                    {
                        if (row[i].ToString().Equals("0")||(row[i].ToString().Equals("")))
                            strdr[i] = DBNull.Value;
                        else
                        {
                            strdr[i] = row[i];
                        }
                    }
                    else
                    {
                        strdr[i] = row[i];
                    }
                }
                strdt.Rows.Add(strdr);
            }
            DataSet strds = new DataSet();
            strds.Tables.Add(strdt);
            return strds;
        }

        /// <summary>
        /// 把dataset转为二维数组，报表打印使用
        /// </summary>
        /// <param name="dataset"></param>
        /// <returns></returns>
        public static string[,] ConvertDataSetToArray(DataSet dataset)
        {
            if (dataset == null || dataset.Tables.Count == 0 || dataset.Tables[0].Columns.Count == 0)
                return null;
            DataTable table = dataset.Tables[0];
            string[,] array = new string[table.Rows.Count, table.Columns.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    array[i, j] = table.Rows[i][j].ToString();
                }
            }
            return array;
        }

        /// <summary>
        /// 根据辅助类型显示辅助描述
        /// </summary>
        /// <param name="assType"></param>
        /// <returns></returns>
        public static string ShowAssisDesc(int assType)
        {
            switch (assType)
            { 
                case 1:
                    return "部门";
                case 2:
                    return "个人";
                case 3:
                    return "客户";
                case 4:
                    return "供应商";
                case 5:
                    return "项目";
                case 13:
                    return "部门客户";
                case 14:
                    return "部门供应商";
                case 15:
                    return "项目部门";
                case 35:
                    return "项目客户";
                case 45:
                    return "项目供应商";
                default:
                    return "部门";
            }
        }
    }
}
