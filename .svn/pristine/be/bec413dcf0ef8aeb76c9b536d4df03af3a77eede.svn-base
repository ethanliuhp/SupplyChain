using System;
using System.Collections;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Domain;
using VirtualMachine.Core;
using Application.Business.Erp.Financial.InitialData.Domain;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.InitStruct;
using System.Collections.Generic;
using System.Data;

namespace Application.Business.Erp.Financial.BasicAccount.InitialSetting.Service
{
    /// <summary>
    /// 科目表Service
    /// </summary>
    public interface IAccountTitleService
    {
        /// <summary>
        /// 科目存盘
        /// </summary>
        /// <param name="tilte"></param>
        /// <returns></returns>
        AccountTitle SaveAccountTitle(AccountTitle tilte);

        /// <summary>
        /// 科目修改
        /// </summary>
        /// <param name="accTitle"></param>
        /// <returns></returns>
        AccountTitle UpdateAccountTitle(AccountTitle accTitle);
        
        /// <summary>
        /// 科目删除
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        bool DeleteAccountTitle(AccountTitle title);

        /// <summary>
        /// 科目冻结
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        bool FreezeAccTitle(AccountTitle title);

        /// <summary>
        /// 科目解冻
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        bool UnFreezeAccTitle(AccountTitle title);

        /// <summary>
        /// 科目冻结
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        IList FreezeAccountTitle(AccountTitle title);
        
        /// <summary>
        /// 科目解冻
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        IList UnFreezeAccountTitle(AccountTitle title);

        /// <summary>
        /// 科目复制
        /// </summary>
        /// <param name="copyInfo"></param>
        /// <returns></returns>
        bool CopyAccTitle(CopyAccTitleSrt copyInfo);

        /// <summary>
        /// 根据条件查询科目，并查询所有父节点，辅助核算拼凑树用
        /// </summary>
        /// <param name="accQuy">条件</param>
        /// <returns>IList</returns>
        IList GetAccTitlesWithFathers(ObjectQuery accQuy);

        /// <summary>
        /// 获取当前科目下级最大最小科目编码
        /// </summary>
        /// <param name="accTitle">科目</param>
        /// <returns></returns>
        IList<AccountTitle> GetNextLevMaxMinCode(AccountTitle accTitle);

        /// <summary>
        /// 科目移动
        /// </summary>
        /// <param name="title"></param>
        /// <param name="toTitle"></param>
        /// <returns></returns>
        IList MoveAccountTitle(AccountTitle title, AccountTitle toTitle);

        /// <summary>
        /// 根据ID获取科目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AccountTitle GetAccountTitle(string id);

        /// <summary>
        /// 根据科目编码获取科目
        /// </summary>
        /// <param name="accCode">科目编码</param>
        /// <returns></returns>
        AccountTitle GetAccTitleByCode(string accCode);

        /// <summary>
        /// 查询科目
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetAccountTitles(ObjectQuery oq);
        
        /// <summary>
        /// 获取币种信息
        /// </summary>
        /// <returns></returns>
        IList GetCurrencyList();

        /// <summary>
        /// 获取科目级别
        /// </summary>
        /// <returns></returns>
        AccountLevel GetAccountLevel();

        /// <summary>
        /// 获取台账明细
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetDeskAccounts(ObjectQuery oq);
        
        /// <summary>
        /// 获取最大科目 传入ObjectQuery失效
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        AccountTitle GetMaxAccountTitle(ObjectQuery oq);
        
        /// <summary>
        /// 获取最小科目 传入ObjectQuery失效
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        AccountTitle GetMinAccountTitle(ObjectQuery oq);

        /// <summary>
        /// 是否被凭证引用
        /// </summary>
        /// <param name="titleId"></param>
        /// <returns></returns>
        bool IsReferByVoucher(string accCode);

        /// <summary>
        /// 查询科目(无权限过滤)
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetAccTitleByQuery(ObjectQuery query);
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataSet Select(string condition);
        /// <summary>
        /// 新增保存
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        System.Collections.IList Save(System.Collections.IList lst);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        System.Collections.IList Update(System.Collections.IList lst);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        bool insert(string condition);
        /// <summary>
        /// 更新,
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        bool update(string condition);
    }
}
