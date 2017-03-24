using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core;
using System.Collections;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Service;
using System.Runtime.Remoting.Messaging;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.Financial.GlobalInfo;
using NHibernate.Criterion;
using NHibernate;
using Application.Business.Erp.Financial.InitialData.Domain;
using Application.Business.Erp.Financial.BasicAccount.AssisAccount.Domain;
using VirtualMachine.Core.DataAccess;
using System.Data;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.InitStruct;
using VirtualMachine.Component.Util;
using Application.Resource.CommonClass.Domain;

namespace Application.Business.Erp.Financial.BasicAccount.InitialSetting.Service
{
    /// <summary>
    /// 会计科目Service 
    /// </summary>
    public class AccountTitleService : IAccountTitleService
    {
        private IDao dao;
        virtual public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }

        private IDao nakedDao;
        virtual public IDao NakedDao
        {
            get { return nakedDao; }
            set { nakedDao = value; }
        }

        private IDataAccess dBDao;
        virtual public IDataAccess DBDao
        {
            get { return dBDao; }
            set { dBDao = value; }
        }

        private ICategoryNodeService nodeSrv;
        virtual public ICategoryNodeService NodeSrv
        {
            get { return nodeSrv; }
            set { nodeSrv = value; }
        }

        //private ICommon commSrv;
        //virtual public ICommon CommSrv
        //{
        //    get { return commSrv; }
        //    set { commSrv = value; }
        //}

        /// <summary>
        /// 会计科目存盘
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [TransManager]
        virtual public AccountTitle SaveAccountTitle(AccountTitle title)
        {
            if (title.ParentNode.CategoryNodeType != NodeType.RootNode)
            {
                title.AccLevelName = (title.ParentNode as AccountTitle).AccLevelName + "_" + title.Name;
            }
            else
            {
                title.AccLevelName = title.Name;
            }

            if (title.Id =="")
            {

                Login li = CallContextUtil.LogicalGetData<Login>("LoginInformation");

                IBusinessOperators author = li.TheBusinessOperators;
                title.Author = author;
                title.CreateDate = li.LoginDate;
                title.FiscalYear = li.TheComponentPeriod.NowYear;
                nodeSrv.AddChildNode(title);
            }
            else
            {
                nodeSrv.UpdateCategoryNode(title);
            }
            return title;
        }

        /// <summary>
        /// 科目表更新
        /// </summary>
        /// <param name="accTitle"></param>
        /// <returns></returns>
        [TransManager]
        virtual public AccountTitle UpdateAccountTitle(AccountTitle accTitle)
        {
            try
            {
                AccountTitle rtnAccTitle = null;
                //需要更新分级名称
                AccountTitle oldAcc = Dao.Get(typeof(AccountTitle), accTitle.Id) as AccountTitle;
                string oldAccName = oldAcc.Name;

                if (accTitle.Name == oldAcc.Name)
                {
                    rtnAccTitle = Dao.SaveOrUpdateCopy(accTitle) as AccountTitle;
                }
                else
                {
                    string oldLevName = accTitle.AccLevelName;
                    string[] oldLevs = oldLevName.Split(new char[] { '_' });

                    int oldLevLen = oldLevs.Length;
                    if (oldLevLen == 1)
                    {
                        accTitle.AccLevelName = accTitle.Name;
                    }
                    else
                    {
                        string updStr = accTitle.Name;

                        oldLevs[oldLevLen - 1] = updStr;
                        string newLev = "";
                        for (int i = 0; i < oldLevLen - 1; i++)
                        {
                            newLev += oldLevs[i] + "_";
                        }
                        newLev += updStr;
                        accTitle.AccLevelName = newLev;
                    }
                    rtnAccTitle = Dao.SaveOrUpdateCopy(accTitle) as AccountTitle;

                    if (accTitle.CategoryNodeType != NodeType.LeafNode)
                    {
                        //中间节点更新名称，需要批量更新分级名称
                        ObjectQuery accQuy = new ObjectQuery();
                        accQuy.AddCriterion(Expression.Not(Expression.Eq("Id", accTitle.Id)));
                        accQuy.AddCriterion(Expression.Like("SysCode", accTitle.SysCode + "%"));
                        accQuy.AddCriterion(Expression.Eq("State", 1));

                        IList lsAccs = Dao.ObjectQuery(typeof(AccountTitle), accQuy);

                        foreach (AccountTitle cirAcc in lsAccs)
                        {
                            cirAcc.AccLevelName = cirAcc.AccLevelName.Replace(oldAccName + "_", accTitle.Name + "_");
                        }

                        Dao.Update(lsAccs);
                    }
                }

                return rtnAccTitle;
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// 删除科目,将节点状态改为无效
        /// 将State更改为0
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [TransManager]
        virtual public bool DeleteAccountTitle(AccountTitle title)
        {
            // nodeSrv.InvalidateNode(title);
            nodeSrv.DeleteCategoryNode(title);
            return true;
        }

        /// <summary>
        /// 科目冻结
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [TransManager]
        virtual public bool FreezeAccTitle(AccountTitle title)
        {
            //是否影响子节点标志
            bool invChilds = false;
            Type nodeType = typeof(AccountTitle);

            //判断子节点
            ObjectQuery cldQuy = new ObjectQuery();
            cldQuy.AddCriterion(Expression.Eq("ParentNode.Id", title.Id));
            cldQuy.AddCriterion(Expression.Eq("State", 1));
            IList lsCld = NakedDao.ObjectQuery(nodeType, cldQuy);
            if (lsCld.Count != 0)
            {
                invChilds = true;
            }

            title.FreezeAccount = true;

            NakedDao.Update(title);

            if (invChilds)
            {
                //子节点需更新
                foreach (AccountTitle cirNode in lsCld)
                {
                    cirNode.FreezeAccount = true;
                    NakedDao.Update(cirNode);
                }
            }

            return true;
        }

        /// <summary>
        /// 科目解冻
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [TransManager]
        virtual public bool UnFreezeAccTitle(AccountTitle title)
        {
            //是否影响子节点标志
            bool invChilds = false;
            Type nodeType = typeof(AccountTitle);

            //判断子节点
            ObjectQuery cldQuy = new ObjectQuery();
            cldQuy.AddCriterion(Expression.Eq("ParentNode.Id", title.Id));
            cldQuy.AddCriterion(Expression.Eq("State", 1));
            IList lsCld = NakedDao.ObjectQuery(nodeType, cldQuy);
            if (lsCld.Count != 0)
            {
                invChilds = true;
            }

            title.FreezeAccount = false;

            NakedDao.Update(title);

            if (invChilds)
            {
                //子节点需更新
                foreach (AccountTitle cirNode in lsCld)
                {
                    cirNode.FreezeAccount = false;
                    NakedDao.Update(cirNode);
                }
            }

            return true;
        }


        /// <summary>
        /// 冻结科目
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [TransManager]
        public IList FreezeAccountTitle(AccountTitle title)
        {
            dao.ReAttatch(title);
            ArrayList lstNodes = new ArrayList();
            lstNodes.Add(title);
            IList lst = nodeSrv.GetALLChildNodes(title);
            lstNodes.AddRange(lst);
            for (int i = 0; i < lstNodes.Count; i++)
            {
                AccountTitle acc = lstNodes[i] as AccountTitle;
                acc.FreezeAccount = true;
                nodeSrv.UpdateCategoryNode(acc);
            }
            return lstNodes;
        }

        /// <summary>
        /// 解冻科目
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [TransManager]
        public IList UnFreezeAccountTitle(AccountTitle title)
        {
            dao.ReAttatch(title);
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("SysCode", title.SysCode + "%"));
            oq.AddCriterion(Expression.Not(Expression.Eq("SysCode", title.SysCode)));
            oq.AddOrder(Order.Asc("SysCode"));

            IList lst = nodeSrv.GetALLChildNodes(title, oq);
            lst.Add(title);
            for (int i = 0; i < lst.Count; i++)
            {
                AccountTitle acc = lst[i] as AccountTitle;
                acc.FreezeAccount = false;
                nodeSrv.UpdateCategoryNode(acc);
            }
            return lst;
        }

        /// <summary>
        /// 获取当前科目下级最大最小科目编码
        /// </summary>
        /// <param name="accTitle">科目</param>
        /// <returns></returns>
        virtual public IList<AccountTitle> GetNextLevMaxMinCode(AccountTitle accTitle)
        {
            IList<AccountTitle> lsCodes = new List<AccountTitle>();

            int minCode = 0;
            int maxCode = 0;

            AccountTitle minAcc = null;
            AccountTitle maxAcc = null;

            try
            {
                Dao.ReAttatch(accTitle);
                foreach (AccountTitle cirAcc in accTitle.ChildNodes)
                {
                    int cirCode = StringUtil.StrToInt(cirAcc.AccountCode);
                    if (minCode == 0)
                    {
                        minCode = cirCode;
                        maxCode = cirCode;
                        minAcc = cirAcc;
                        maxAcc = cirAcc;
                    }

                    if (cirCode > maxCode)
                    {
                        maxCode = cirCode;
                        maxAcc = cirAcc;
                    }
                    if (cirCode < minCode)
                    {
                        minCode = cirCode;
                        minAcc = cirAcc;
                    }
                }

                lsCodes.Add(minAcc);
                lsCodes.Add(maxAcc);
                return lsCodes;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 科目复制
        /// </summary>
        /// <param name="copyInfo"></param>
        /// <returns></returns>
        [TransManager]
        virtual public bool CopyAccTitle(CopyAccTitleSrt copyInfo)
        {
            //try
            //{
            //    IList<DataAccessParameter> lsParams = new List<DataAccessParameter>();
            //    DBDao.SetInParameterForExec("psrcaccid", OracleDbType.Decimal, copyInfo.SrcAcc.Id, ref lsParams);
            //    DBDao.SetInParameterForExec("pbegincode", OracleDbType.Varchar2, copyInfo.SrcFromCode, ref lsParams);
            //    DBDao.SetInParameterForExec("pendcode", OracleDbType.Varchar2, copyInfo.SrcToCode, ref lsParams);
            //    DBDao.SetInParameterForExec("pcurraccid", OracleDbType.Decimal, copyInfo.CurrAcc.Id, ref lsParams);
            //    DBDao.SetInParameterForExec("pcurrparid", OracleDbType.Decimal, copyInfo.CurrAcc.ParentNode.Id, ref lsParams);
            //    DBDao.SetInParameterForExec("psamelev", OracleDbType.Decimal, (copyInfo.SameLevel ? 1 : 0), ref lsParams);
            //    DBDao.SetInParameterForExec("pcopylev", OracleDbType.Decimal, copyInfo.CopyLevel, ref lsParams);
            //    DBDao.SetInParameterForExec("pstartcode", OracleDbType.Varchar2, copyInfo.NewBeginCode, ref lsParams);

            //    try
            //    {
            //        DBDao.ExecuteNonQuery(CommandType.StoredProcedure, lsParams, "pkgfiaccbasic.proccopyacctitle");
            //        return true;
            //    }
            //    catch
            //    {
            //        throw;
            //    }
            //}
            //catch
            //{
            //    throw;
            //}
            return true;
        }


        /// <summary>
        /// 根据条件查询科目，并查询所有父节点，辅助核算拼凑树用
        /// </summary>
        /// <param name="accQuy">条件</param>
        /// <returns>IList</returns>
        virtual public IList GetAccTitlesWithFathers(ObjectQuery accQuy)
        {
            IList lsAcc = Dao.ObjectQuery(typeof(AccountTitle), accQuy);

            IList lsRtnAccs = new ArrayList();

            if (lsAcc.Count > 0)
            {
                foreach (AccountTitle cirAcc in lsAcc)
                {
                    ListFatherTitles(cirAcc, ref lsRtnAccs);
                }
            }
            return lsRtnAccs;
        }

        private void ListFatherTitles(AccountTitle currAcc, ref IList lsAccTitles)
        {
            if (currAcc.ParentNode == null)
            {
                //根节点
                if (!lsAccTitles.Contains(currAcc))
                {
                    lsAccTitles.Add(currAcc);
                }
                return;
            }
            if (!lsAccTitles.Contains(currAcc))
            {
                lsAccTitles.Add(currAcc);
            }
            ListFatherTitles(currAcc.ParentNode as AccountTitle, ref lsAccTitles);
        }


        [TransManager]
        public IList MoveAccountTitle(AccountTitle title, AccountTitle toTitle)
        {
            nodeSrv.MoveNode(title, toTitle);
            IList lstNodes = new ArrayList();
            lstNodes.Add(title);
            IList lst = nodeSrv.GetALLChildNodes(title);
            foreach (AccountTitle acc in lst)
            {
                lstNodes.Add(acc);
            }
            return lstNodes;
        }

        /// <summary>
        /// 获取分类节点目录
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetAccountTitles(ObjectQuery oq)
        {
            if (oq == null)
            {
                oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("State", (int)1));
                //2007-8-1,添加根据年份过滤科目
                Login login = CallContextUtil.LogicalGetData<Login>("LoginInformation");

                // oq.AddCriterion(Expression.Eq("FiscalYear", li.TheComponentPeriod.NowYear));
                oq.AddOrder(Order.Asc("AccountCode"));
                //oq.AddFetchMode("ForeignCurrency", FetchMode.Eager);
                //oq.AddFetchMode("DeskAcc", FetchMode.Eager);
            }
            IList lst = nodeSrv.GetNodesByObjectQuery(typeof(AccountTitle), oq);
            return lst;
        }

        /// <summary>
        /// 根据ID获取科目编码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        virtual public AccountTitle GetAccountTitle(string id)
        {
            return nodeSrv.GetCategoryNodeById(id, typeof(AccountTitle)) as AccountTitle;
        }

        /// <summary>
        /// 查询科目(无权限过滤)
        /// </summary>
        virtual public IList GetAccTitleByQuery(ObjectQuery query)
        {
            return dao.ObjectQuery(typeof(AccountTitle), query);
        }

        /// <summary>
        /// 根据科目编码获取科目
        /// </summary>
        /// <param name="accCode">科目编码</param>
        /// <returns>AccountTitle</returns>
        virtual public AccountTitle GetAccTitleByCode(string accCode)
        {
            AccountTitle rtnAcc = null;

            ObjectQuery accQuy = new ObjectQuery();
            accQuy.AddCriterion(Expression.Eq("AccountCode", accCode));
            accQuy.AddCriterion(Expression.Eq("State", 1));
            //2007-8-1,添加根据年份过滤科目
            Login li = CallContextUtil.LogicalGetData<Login>("LoginInformation");

            accQuy.AddCriterion(Expression.Eq("FiscalYear", li.TheComponentPeriod.NowYear));

            IList lsAcc = dao.ObjectQuery(typeof(AccountTitle), accQuy);

            if (lsAcc.Count == 1)
            {
                rtnAcc = lsAcc[0] as AccountTitle;
            }
            return rtnAcc;
        }

        /// <summary>
        /// 获取最大科目
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        virtual public AccountTitle GetMaxAccountTitle(ObjectQuery oq)
        {
            string quyStr = "select max(acccode) from thd_fiacctitle where accstate=1 and accnodelevel=2";
            string maxCode = DBDao.OpenQueryScalar(quyStr, null).ToString();
            AccountTitle maxAcc = GetAccTitleByCode(maxCode);
            return maxAcc;
        }

        /// <summary>
        /// 获取最小科目
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        virtual public AccountTitle GetMinAccountTitle(ObjectQuery oq)
        {
            string quyStr = "select min(acccode) from thd_fiacctitle where accstate=1 and accnodelevel=2";
            string minCode = DBDao.OpenQueryScalar(quyStr, null).ToString();
            AccountTitle minAcc = GetAccTitleByCode(minCode);
            return minAcc;
        }


        /// <summary>
        /// 获取外币币种信息
        /// </summary>
        /// <returns></returns>
        public IList GetCurrencyList()
        {
            //  return CommSrv.GetForeignCurrency();
            return null;
        }

        /// <summary>
        /// 获取科目定义级长信息
        /// </summary>
        /// <returns></returns>
        public AccountLevel GetAccountLevel()
        {
            return dao.Get(typeof(AccountLevel), 1L) as AccountLevel;
        }

        /// <summary>
        /// 获取台帐信息
        /// </summary>
        /// <returns></returns>
        public IList GetDeskAccounts(ObjectQuery oq)
        {
            if (oq == null)
            {
                oq = new ObjectQuery();
            }
            return dao.ObjectQuery(typeof(DeskAccount), oq);
        }

        /// <summary>
        /// 检查科目是否已被凭证所引用
        /// </summary>
        /// <param name="titleId"></param>
        /// <returns></returns>
        virtual public bool IsReferByVoucher(string accCode)
        {
            //string querySql = "select count(voudetid) from fivoucherdet where acccode like :incode and detyear=:inyear";

            //IList<DataAccessParameter> lsParams = new List<DataAccessParameter>();
            //DBDao.SetInParameterForQuery(":incode", OracleDbType.Varchar2, accCode + "%", ref lsParams);
            ////2007-8-1,添加根据年份过滤科目
            //Login li = CallContext.GetData("LoginInformation") as Login;
            //DBDao.SetInParameterForQuery(":inyear", OracleDbType.Decimal, li.CompPeriod.NowYear, ref lsParams);
            //Object cntValue = DBDao.OpenQueryScalar(querySql, lsParams);

            //if (cntValue.ToString() == "0")
            //{
            //    return false;
            //}/
            return true;
        }

        #region 查询


        public DataSet Select(string condition)
        {
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string strSql = condition;
            command.CommandText = strSql;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            return DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
        }

        #endregion

        #region 保存/更新
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        [TransManager]
        public IList Save(IList lst)
        {
            Dao.Save(lst);
            return lst;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        [TransManager]
        public IList Update(IList lst)
        {
            Dao.Update(lst);
            return lst;
        }
        /// <summary>
        /// 增加/更新
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public bool insert(string condition)
        {
            int count = 0;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string strSql = condition;
            command.CommandText = strSql;
            command.CommandType = CommandType.Text;
            count = command.ExecuteNonQuery();
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public bool update(string condition)
        {
            int counts = 0;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string strSql = condition;
            command.CommandText = strSql;
            command.CommandType = CommandType.Text;
            counts = command.ExecuteNonQuery();
            if (counts > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion



    }
}
