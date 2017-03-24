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
    /// ��ƿ�Ŀ
    /// </summary>
    public class MAccountTitle
    {
        public string ErrorInfo; //У����ʾ��Ϣ
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
        /// ��ȡ��ƿ�Ŀ�ڵ㼯��
        /// </summary>
        /// <returns></returns>
        virtual public IList GetAccountTitles()
        {
            IList lsAllTitle = titleService.GetAccountTitles(null);
            return lsAllTitle;
        }

        /// <summary>
        /// �����ƿ�Ŀ
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public AccountTitle SaveAccountTitle(AccountTitle title)
        {
            title = titleService.SaveAccountTitle(title);
            return title;
        }

        /// <summary>
        /// �����Ŀ
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public IList FreezeAccountTitle(AccountTitle title)
        {
            return titleService.FreezeAccountTitle(title);
        }

        /// <summary>
        /// �ⶳ��Ŀ
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public IList UnFreezeAccountTitle(AccountTitle title)
        {
            IList lst = titleService.UnFreezeAccountTitle(title);
            return lst;
        }

        /// <summary>
        /// ɾ����ƿ�Ŀ
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public AccountTitle DeleteAccountTitle(AccountTitle title)
        {
            titleService.DeleteAccountTitle(title);
            return title.ParentNode as AccountTitle;
        }

        /// <summary>
        /// ��ȡ��ƿ�Ŀ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AccountTitle GetAccountTitle(string id)
        {
            return titleService.GetAccountTitle(id);
        }

        /// <summary>
        /// ��ȡ��ǰ�ڵ�������ʽڵ�
        /// </summary>
        /// <returns></returns>
        public IList GetAccountChilds()
        {
            return null;
        }

        /// <summary>
        /// �ƶ���ƿ�Ŀ�ڵ�
        /// </summary>
        /// <param name="title"></param>
        /// <param name="toTitle"></param>
        /// <returns></returns>
        public IList MoveAccountTitle(AccountTitle title, AccountTitle toTitle)
        {
            return titleService.MoveAccountTitle(title, toTitle);
        }

        /// <summary>
        /// ��ȡ��ұ����б�
        /// </summary>
        /// <returns></returns>
        public IList GetCurrencyList()
        {
            return titleService.GetCurrencyList();
        }

        /// <summary>
        /// ��ȡ��Ŀ����
        /// </summary>
        /// <returns></returns>
        public AccountLevel GetAccountLevel()
        {
            return titleService.GetAccountLevel();
        }


        /// <summary>
        /// ��ȡ̨���б�
        /// </summary>
        /// <returns></returns>
        public IList GetDeskAccounts()
        {
            return titleService.GetDeskAccounts(null);
        }


        /// <summary>
        /// У���Ŀ����
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
        /// У��������
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
        /// ����Ŀ�Ƿ��ѱ�ƾ֤������
        /// </summary>
        /// <param name="titleId"></param>
        /// <returns></returns>
        public bool IsReferByVoucher(string accCode)
        {
            return titleService.IsReferByVoucher(accCode);
        }
        /// <summary>
        /// ��ѯ
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
        /// ����
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public bool inserts(string condition)
        {
            return titleService.insert(condition);
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool updates(string sql)
        {
            return titleService.update(sql);
        }
        /// <summary>
        /// ���ݿ�Ŀ�����ȡ��Ŀ
        /// </summary>
        /// <param name="currentCode"></param>
        /// <returns></returns>
        public AccountTitle GetParentAccTitle(string currentCode)
        {
            return titleService.GetAccTitleByCode(currentCode);
        }
        /// <summary>
        /// ����������ȡ��Ŀ
        /// </summary>
        /// <param name="OQ"></param>
        /// <returns></returns>
        public IList GetAccTitle(ObjectQuery OQ)
        {
            return titleService.GetAccTitleByQuery(OQ);
        }
    }
}
