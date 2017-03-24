using System;
using VirtualMachine.Core;
using System.Collections;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using VirtualMachine.Core.DataAccess;
using VirtualMachine.Patterns.CategoryTreePattern.Service;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.CommonClass.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Domain;

namespace Application.Business.Erp.Financial.BasicAccount.InitialSetting.Service
{
    public class CostProjectSrv : Application.Business.Erp.Financial.BasicAccount.InitialSetting.Service.ICostProjectSrv
    {
        private IDao _Dao;
        virtual public IDao Dao
        {
            get { return _Dao; }
            set { _Dao = value; }
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

        /// <summary>
        /// 存盘
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [TransManager]
        virtual public CostProject SaveCostProject(CostProject title)
        {
            if (title.Id == "")
            {
                Login li = VirtualMachine.Component.Util.CallContextUtil.LogicalGetData<Login>("LoginInformation");
                IBusinessOperators author = li.TheBusinessOperators;
                title.Author = author;
                title.CreateDate = li.LoginDate;
                nodeSrv.AddChildNode(title);
            }
            else
            {
                nodeSrv.UpdateCategoryNode(title);
            }
            string err = CostProjectUpdate();
            return title;
        }

        /// <summary>
        /// 科目表更新
        /// </summary>
        /// <param name="accTitle"></param>
        /// <returns></returns>
        [TransManager]
        virtual public CostProject UpdateCostProject(CostProject accTitle)
        {
            try
            {
                CostProject rtnAccTitle = null;
                //需要更新分级名称
                CostProject oldAcc = Dao.Get(typeof(CostProject), accTitle.Id) as CostProject;
                string oldAccName = oldAcc.Name;

                if (accTitle.Name == oldAcc.Name)
                {
                    rtnAccTitle = Dao.SaveOrUpdateCopy(accTitle) as CostProject;
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
                    rtnAccTitle = Dao.SaveOrUpdateCopy(accTitle) as CostProject;

                    if (accTitle.CategoryNodeType != NodeType.LeafNode)
                    {
                        //中间节点更新名称，需要批量更新分级名称
                        ObjectQuery accQuy = new ObjectQuery();
                        accQuy.AddCriterion(Expression.Not(Expression.Eq("Id", accTitle.Id)));
                        accQuy.AddCriterion(Expression.Like("SysCode", accTitle.SysCode + "%"));
                        accQuy.AddCriterion(Expression.Eq("State", 1));

                        IList lsAccs = Dao.ObjectQuery(typeof(CostProject), accQuy);

                        foreach (CostProject cirAcc in lsAccs)
                        {
                            cirAcc.AccLevelName = cirAcc.AccLevelName.Replace(oldAccName + "_", accTitle.Name + "_");
                        }

                        Dao.Update(lsAccs);
                    }
                    string err = CostProjectUpdate();
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
        virtual public bool DeleteCostProject(CostProject title)
        {
            nodeSrv.DeleteCategoryNode(title);
            return true;
        }

        /// <summary>
        /// 根据条件查询科目，并查询所有父节点，辅助核算拼凑树用
        /// </summary>
        /// <param name="accQuy">条件</param>
        /// <returns>IList</returns>
        virtual public IList GetCostProjectsWithFathers(ObjectQuery accQuy)
        {
            IList lsAcc = Dao.ObjectQuery(typeof(CostProject), accQuy);

            IList lsRtnAccs = new ArrayList();

            if (lsAcc.Count > 0)
            {
                foreach (CostProject cirAcc in lsAcc)
                {
                    ListFatherCostProjects(cirAcc, ref lsRtnAccs);
                }
            }
            return lsRtnAccs;
        }

        private void ListFatherCostProjects(CostProject currAcc, ref IList lsAccTitles)
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
            ListFatherCostProjects(currAcc.ParentNode as CostProject, ref lsAccTitles);
        }


        /// <summary>
        /// 获取分类节点目录
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetCostProjects(ObjectQuery oq)
        {
            if (oq == null)
            {
                oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("State", (int)1));
                oq.AddOrder(Order.Asc("Code"));
                oq.AddFetchMode("AccTitle", FetchMode.Eager);
            }
            IList lst = nodeSrv.GetNodesByObjectQuery(typeof(CostProject), oq);
            return lst;
        }

        /// <summary>
        /// 根据ID获取科目编码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        virtual public CostProject GetCostProject(string id)
        {
            return nodeSrv.GetCategoryNodeById(id, typeof(CostProject)) as CostProject;
        }

        /// <summary>
        /// 查询科目(无权限过滤)
        /// </summary>
        virtual public IList GetCostProjectByQuery(ObjectQuery query)
        {
            return nakedDao.ObjectQuery(typeof(CostProject), query);
        }

        /// <summary>
        /// 检查科目是否已被成本计算所引用 True 已经引用
        /// </summary>
        /// <param name="titleId"></param>
        /// <returns></returns>
        virtual public bool IsReferByCostAccount(string CostProjectId)
        {
            return false;
            //ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq( "CostProject.Id", CostProjectId));

            //IList ls = Dao.ObjectQuery(typeof(ProduceCostTotalDtl), oq);

            //if (ls.Count==0)
            //{
            //    return false;
            //}
            //return true;
        }


        /// <summary>
        /// 保存新增数据
        /// </summary>
        /// <param name="obj">要保存的对象</param>
        /// <returns>保存后的对象</returns>
        [TransManager]
        virtual public Object Save(Object obj)
        {
            Dao.Save(obj);
            return obj;
        }
        /// <summary>
        /// 保存新增数据
        /// </summary>
        /// <param name="lst">要保存的对象IList</param>
        /// <returns>保存后的IList</returns>
        [TransManager]
        virtual public IList Save(IList lst)
        {
            Dao.SaveOrUpdate(lst);
            return lst;
        }

        /// <summary>
        /// 保存要修改的数据
        /// </summary>
        /// <param name="obj">要修改的对象</param>
        /// <returns>修改后的对象</returns>
        [TransManager]
        virtual public Object Update(Object obj)
        {
            Dao.Update(obj);
            return obj;
        }
        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="lst">要修改的对象IList</param>
        /// <returns>修改后的对象IList</returns>
        [TransManager]
        virtual public IList Update(IList lst)
        {
            Dao.Update(lst);
            return lst;
        }
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="lst">要删除的对象IList</param>
        /// <returns>True 删除成功，False 删除失败</returns>
        [TransManager]
        virtual public bool Delete(IList lst)
        {
            Dao.Delete(lst);
            return true;
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="obj">要删除的对象</param>
        /// <returns>True 删除成功，False 删除失败</returns>
        virtual public bool Delete(Object obj)
        {
            Dao.Delete(obj);
            return true;
        }
        /// <summary>
        /// 查询制定类型的所有的数据
        /// </summary>
        /// <param name="aType"></param>
        /// <returns></returns>
        virtual public IList GetObjects(Type aType)
        {
            return GetObjects(aType, new ObjectQuery());
        }
        /// <summary>
        /// 按条件查询制定类型的数据
        /// </summary>
        /// <param name="aType">类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns>查询结果IList</returns>
        virtual public IList GetObjects(Type aType, ObjectQuery oq)
        {
            IList lstReturn = new ArrayList();
            lstReturn = Dao.ObjectQuery(aType, oq);
            return lstReturn;
        }
        public string CostProjectUpdate()
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection connect = session.Connection;
            IDbCommand cmd = connect.CreateCommand();
            ITransaction tran = CallContext.GetData("ntransaction") as ITransaction;
            tran.Enlist(cmd);
            cmd.CommandText = "P_CostProjectUpdate";
            cmd.CommandType = CommandType.StoredProcedure;

            IDbDataParameter err = cmd.CreateParameter();
            err.DbType = DbType.AnsiString;
            err.Direction = ParameterDirection.Output;
            err.ParameterName = "errMsg";
            err.Size = 500;
            cmd.Parameters.Add(err);
            cmd.ExecuteNonQuery();
            return err.Value.ToString();
        }

        #region ICostProjectSrv 成员

        /// <summary>
        /// 生产成本班组明细统计
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public DataSet ProductCostClassteamSearch(string condition)
        {
            IDataReader dataReader;
            if (condition == "")
            {
                condition = " 1=1 ";
            }
            ISession session = CallContext.GetData("nhibernate") as ISession;
            IDbConnection connect = session.Connection;
            IDbCommand command = connect.CreateCommand();
            string strSql = "select t3.Name as 项目分类,t2.Name as 原材料\n" +
                            "from THD_CostProduceCostDtl t1 left join RES_CostProject t2 on t1.CostProject=t2.Id \n" +
                            "left join Res_BasClassTeam t3 on t1.ClassTeam=t3.Id \n" +
                            "where '" + condition + "' \n" +
                            "group by t3.Name,t2.Name \n" +
                            "order by t3.Name \n";
            command.CommandText = strSql;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            return DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

        }

        #endregion
    }
}
