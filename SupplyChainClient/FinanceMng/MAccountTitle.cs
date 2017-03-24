using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Service;
using System.Collections;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using NHibernate;
using Application.Business.Erp.Financial.InitialData.Domain;
using System.Data;
using Application.Business.Erp.SupplyChain.FinanceMng.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;


namespace Application.Business.Erp.Financial.Client.FinanceUI.InitialSetting.AccountTitleUI
{
    /// <summary>
    /// 会计科目
    /// </summary>
    public class MAccountTitle
    {
        public string ErrorInfo; //校验提示信息
        public IAccountTitleService titleService;
        private IAccountTitleTreeSvr accountTitleTreeSvr;
        public IAccountTitleTreeSvr AccountTitleTreeSvr
        {
            get
            {
                if (accountTitleTreeSvr == null)
                {
                    accountTitleTreeSvr = StaticMethod.GetService("AccountTitleTreeSvr") as IAccountTitleTreeSvr;
                }
                return accountTitleTreeSvr;
            }
            set
            {
                accountTitleTreeSvr = value;
            }
        }
        /// <summary>
        /// 获取会计科目节点集合
        /// </summary>
        /// <returns></returns>
        virtual public IList GetAccountTitles()
        {
            IList lsAllTitle = titleService.GetAccountTitles(null);
            return lsAllTitle;
        }

        /// <summary>
        /// 保存会计科目
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public AccountTitle SaveAccountTitle(AccountTitle title)
        {
            title = titleService.SaveAccountTitle(title);
            return title;
        }

        /// <summary>
        /// 冻结科目
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public IList FreezeAccountTitle(AccountTitle title)
        {
            return titleService.FreezeAccountTitle(title);
        }

        /// <summary>
        /// 解冻科目
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public IList UnFreezeAccountTitle(AccountTitle title)
        {
            IList lst = titleService.UnFreezeAccountTitle(title);
            return lst;
        }

        /// <summary>
        /// 删除会计科目
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public AccountTitle DeleteAccountTitle(AccountTitle title)
        {
            titleService.DeleteAccountTitle(title);
            return title.ParentNode as AccountTitle;
        }

        /// <summary>
        /// 获取会计科目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AccountTitle GetAccountTitle(string id)
        {
            return titleService.GetAccountTitle(id);
        }

        /// <summary>
        /// 获取当前节点的所有资节点
        /// </summary>
        /// <returns></returns>
        public IList GetAccountChilds()
        {
            return null;
        }

        /// <summary>
        /// 移动会计科目节点
        /// </summary>
        /// <param name="title"></param>
        /// <param name="toTitle"></param>
        /// <returns></returns>
        public IList MoveAccountTitle(AccountTitle title, AccountTitle toTitle)
        {
            return titleService.MoveAccountTitle(title, toTitle);
        }

        /// <summary>
        /// 获取外币币种列表
        /// </summary>
        /// <returns></returns>
        public IList GetCurrencyList()
        {
            return titleService.GetCurrencyList();
        }

        /// <summary>
        /// 获取科目级长
        /// </summary>
        /// <returns></returns>
        public AccountLevel GetAccountLevel()
        {
            return titleService.GetAccountLevel();
        }


        /// <summary>
        /// 获取台账列表
        /// </summary>
        /// <returns></returns>
        public IList GetDeskAccounts()
        {
            return titleService.GetDeskAccounts(null);
        }


        /// <summary>
        /// 校验科目编码
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool ValidateAccCode(AccountTitle title)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", title.Code));
            oq.AddCriterion(Expression.Not(Expression.Eq("Id", title.Id)));
            IList lst = titleService.GetAccountTitles(oq);
            if (lst != null && lst.Count > 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 校验助记码
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool ValidateAssCode(AccountTitle title)
        {
            if (title.AssisCode.Equals("")) return true;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("AssisCode", title.AssisCode));
            oq.AddCriterion(Expression.Not(Expression.Eq("Id", title.Id)));
            IList lst = titleService.GetAccountTitles(oq);

            if (lst != null && lst.Count > 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 检查科目是否已被凭证所引用
        /// </summary>
        /// <param name="titleId"></param>
        /// <returns></returns>
        public bool IsReferByVoucher(string accCode)
        {
            return titleService.IsReferByVoucher(accCode);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public DataSet select(string Sql)
        {
            return titleService.Select(Sql);
        }

        public IList save(IList lst)
        {
            return titleService.Save(lst);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public bool inserts(string condition)
        {
            return titleService.insert(condition);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool updates(string sql)
        {
            return titleService.update(sql);
        }
        /// <summary>
        /// 根据科目编码获取科目
        /// </summary>
        /// <param name="currentCode"></param>
        /// <returns></returns>
        public AccountTitle GetParentAccTitle(string currentCode)
        {
            return titleService.GetAccTitleByCode(currentCode);
        }
        /// <summary>
        /// 根据条件获取科目
        /// </summary>
        /// <param name="OQ"></param>
        /// <returns></returns>
        public IList GetAccTitle(ObjectQuery OQ)
        {
            return titleService.GetAccTitleByQuery(OQ);
        }
    }
}
