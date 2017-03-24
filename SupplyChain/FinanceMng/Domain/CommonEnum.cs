using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Application.Business.Erp.Financial.BasicAccount.InitialSetting.Domain
{
    /// <summary>
    /// 摘要类型
    /// </summary>
    public enum SummaryType
    { 
        /// <summary>
        /// 会计名词
        /// </summary>
        FiscalNoun,
        /// <summary>
        /// 人名
        /// </summary>
        PersonName,
        /// <summary>
        /// 地名
        /// </summary>
        PlaceName,
        /// <summary>
        /// 商品物资
        /// </summary>
        GoodsName,
        /// <summary>
        /// 客户
        /// </summary>
        ClientName,
        /// <summary>
        /// 其他
        /// </summary>
        Others
    }

    /// <summary>
    /// 摘要类型2
    /// </summary>
    public enum CommSummaryType
    {
        /// <summary>
        /// 公用摘要
        /// </summary>
        PublicSummary,
        /// <summary>
        /// 个人摘要
        /// </summary>
        PrivateSummary
    }

    /// <summary>
    /// 凭证条件类型
    /// </summary>
    public enum output
    {
        /// <summary>
        /// 凭证号
        /// </summary>
        VouNO,
        /// <summary>
        /// 凭证类型
        /// </summary>
        VouType,
        /// <summary>
        /// 凭证日期
        /// </summary>
        VouDate
    }

    /// <summary>
    /// 科目类型
    /// </summary>
    public enum AccountType
    { 
        /// <summary>
        /// 资产
        /// </summary>
        [Description("资产")]
        Asserts,
        /// <summary>
        /// 负债
        /// </summary>
        [Description("负债")]
        Liabilities,
        /// <summary>
        /// 共同
        /// </summary>
        [Description("共同")]
        Together,
        /// <summary>
        /// 所有者权益
        /// </summary>
        [Description("所有者权益")]
        Interests,
        /// <summary>
        /// 成本
        /// </summary>
        [Description("成本")]
        Cost,
        /// <summary>
        /// 损益
        /// </summary>
        [Description("损益")]
        ProfitAndLoss,
        /// <summary>
        /// 全部类型
        /// </summary>
        [Description("全部类型")]
        ALL
    }

    /// <summary>
    /// 帐簿格式
    /// </summary>
    public enum BookStyle
    { 
        /// <summary>
        /// 金额式
        /// </summary>
        Amount,
        /// <summary>
        /// 外币金额式
        /// </summary>
        ForeignAmount,
        /// <summary>
        /// 数量金额式
        /// </summary>
        QuantityAmount,
        /// <summary>
        /// 外币数量式
        /// </summary>
        ForQuanAmount
    }

    /// <summary>
    /// 现金科目
    /// </summary>
    public enum CashAccTitle
    { 
        /// <summary>
        /// 非现金及现金等价物
        /// </summary>
        NotCash,
        /// <summary>
        /// 现金科目
        /// </summary>
        CashTitle,
        /// <summary>
        /// 银行存款科目
        /// </summary>
        BankTitle,
        /// <summary>
        /// 其他现金及现金等价物
        /// </summary>
        OtherCashTitle
    }
}
