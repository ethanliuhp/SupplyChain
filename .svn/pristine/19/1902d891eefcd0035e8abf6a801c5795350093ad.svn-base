using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.BasicData.BaleNoMng.Domain;
using VirtualMachine.Core.Expression;
using System.Collections;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.MaterialResource.Service;
using VirtualMachine.Component.Util;
using NHibernate;
using System.Data;
using System.Runtime.Remoting.Messaging;

namespace Application.Business.Erp.SupplyChain.BasicData.BaleNoMng.Service
{
    public class BaleNoFactorySrv : Application.Business.Erp.SupplyChain.BasicData.BaleNoMng.Service.IBaleNoFactorySrv
    {
        private IDao _Dao;
        virtual public IDao Dao
        {
            get { return _Dao; }
            set { _Dao = value; }
        }
        private IPracticalityStateService _PracticalityStateService;

        virtual public IPracticalityStateService PracticalityStateService
        {
            get { return _PracticalityStateService; }
            set { _PracticalityStateService = value; }
        }

        //[TransManager]
        //public string GetMaxBaleNo(enmOpeRole aOpeRole, DateTime createDate)
        //{
        //    switch (aOpeRole)
        //    {
        //        case enmOpeRole.CusCureProduce:
        //            return GetMaxBaleNo("P", createDate);                    
        //        case enmOpeRole.CusCureSemiProduce:
        //            return GetMaxBaleNo("B", createDate);
        //        case enmOpeRole.CusCureStuff:
        //            return GetMaxBaleNo("Y", createDate);
        //        case enmOpeRole.LowPrice:
        //            throw new Exception("实例前缀未进行定义");
        //        case enmOpeRole.Recycle:
        //            return GetMaxBaleNo("P", createDate);
        //        case enmOpeRole.SparePart:
        //            throw new Exception("实例前缀未进行定义");
        //        case enmOpeRole.produce:
        //            return GetMaxBaleNo("P", createDate);
        //        case enmOpeRole.semiProduce:
        //            return GetMaxBaleNo("B", createDate);
        //        case enmOpeRole.stuff:
        //            return GetMaxBaleNo("Y", createDate);
        //        case enmOpeRole.wrappage:
        //            throw new Exception("实例前缀未进行定义");
        //        default:
        //            throw new Exception("数据错误");
        //    }
        //}



        /// <summary>
        /// 获得原料的最大号
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <param name="createDate">当前日期</param>
        /// <returns></returns>
        [TransManager]
        public string GetMaxBaleNo(string prefix, int seqLength, DateTime createDate)
        {
            string MaxBaleNo = "";
            int year = createDate.Year;
            int month = createDate.Month;
            int day = createDate.Day;
            if (seqLength == 0)
            {
                seqLength = 4;
            }
            ObjectQuery oq = new ObjectQuery();
            //先锁表
            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Prefix", "00000000"));
            IList lst1 = Dao.ObjectQuery(typeof(BaleNoFactory), oq);


            BaleNoFactory tmpBaleNoFactory;
            if (lst1.Count == 0)
            {
                tmpBaleNoFactory = new BaleNoFactory();
                tmpBaleNoFactory.Prefix = "00000000";
                tmpBaleNoFactory.BaleNo = "00000000";
                tmpBaleNoFactory.Year = 2008L;
                tmpBaleNoFactory.Month = 1L;
                tmpBaleNoFactory.CreateDate = createDate;
                Dao.Save(tmpBaleNoFactory);
            }
            else
            {
                tmpBaleNoFactory = lst1[0] as BaleNoFactory;
                Dao.Update(tmpBaleNoFactory);
            }

            #region 初始化暂时使用
            IList lst = new ArrayList();
            if (prefix.Equals("PN"))
            {
                oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Prefix", prefix));
                //oq.AddCriterion(Expression.Eq("Year", (long)year));
                //oq.AddCriterion(Expression.Eq("Month", (long)month));
                lst = Dao.ObjectQuery(typeof(BaleNoFactory), oq);

                if (lst.Count == 0)//当月新第一条记录
                {
                    BaleNoFactory BaleNoFactory = new BaleNoFactory();
                    BaleNoFactory.Prefix = prefix;
                    BaleNoFactory.Year = (long)year;
                    BaleNoFactory.Month = (long)month;
                    BaleNoFactory.CreateDate = createDate;
                    BaleNoFactory.MaxNo = 1;
                    BaleNoFactory.BaleNo = prefix + "1".PadLeft(seqLength, '0');// prefix + year.ToString().Substring(2, 2) + month.ToString("00") + day.ToString("00") +"0001";
                    Dao.Save(BaleNoFactory);
                    MaxBaleNo = BaleNoFactory.BaleNo;
                }
                else
                {
                    BaleNoFactory tmp1 = lst[0] as BaleNoFactory;
                    //处理获得的记录
                    long maxNo = tmp1.MaxNo;
                    string baleNo = prefix + ((long)(maxNo + 1)).ToString("").PadLeft(seqLength, '0');//prefix + year.ToString().Substring(2, 2) + month.ToString("00") + day.ToString("00") + ((long)(maxNo+1)).ToString("0000");
                    tmp1.MaxNo += 1;
                    tmp1.CreateDate = createDate;
                    tmp1.BaleNo = baleNo;
                    Dao.Update(tmp1);
                    MaxBaleNo = tmp1.BaleNo;
                }
            }
            else
            {
                oq = new ObjectQuery();
                if (prefix.Substring(0, 1).Equals("M") &&
                    prefix.Length == 6)
                {
                    //原料特殊处理
                    //oq.AddCriterion(Expression.Like("BaleNo", "M", MatchMode.Start));
                    //oq.AddCriterion(Expression.Like("BaleNo", prefix.Substring(2, 4), MatchMode.Anywhere));

                    ISession session = CallContext.GetData("nhsession") as ISession;
                    IDbConnection cnn = session.Connection;

                    IDataReader dataReader = null;
                    IDbCommand cmd = cnn.CreateCommand();
                    ITransaction tran = CallContext.GetData("ntransaction") as ITransaction;
                    tran.Enlist(cmd);
                    cmd.CommandText =
                                    "select top 1 BaleNo\n" +
                                    "from dbo.ResPracticState\n" +
                                    "where BaleNo like 'M%" + prefix.Substring(2, 4) + "%'\n" +
                                    "      and substring(baleNo,3,4)='" + prefix.Substring(2, 4) + "'\n" +
                                    "order by substring(baleNo,7,4) desc";
                    if (dataReader != null && !dataReader.IsClosed)
                    {
                        dataReader.Close();
                    }
                    dataReader = cmd.ExecuteReader(CommandBehavior.Default);

                    bool isExist = false;
                    while (dataReader.Read())
                    {
                        isExist = true;

                        long maxNo = ClientUtil.ToLong(ClientUtil.ToString(dataReader["BaleNo"]).Substring(prefix.Length, seqLength));
                        string baleNo = prefix + ((long)(maxNo + 1)).ToString("").PadLeft(seqLength, '0');//prefix + year.ToString().Substring(2, 2) + month.ToString("00") + day.ToString("00") + ((long)(maxNo+1)).ToString("0000");
                        //tmp1.MaxNo += 1;
                        //tmp1.CreateDate = createDate;
                        //tmp1.BaleNo = baleNo;
                        //Dao.Update(tmp1);
                        MaxBaleNo = baleNo;
                    }
                    dataReader.Close();
                    if (!isExist)
                    {
                        BaleNoFactory BaleNoFactory = new BaleNoFactory();
                        BaleNoFactory.Prefix = prefix;
                        BaleNoFactory.Year = (long)year;
                        BaleNoFactory.Month = (long)month;
                        BaleNoFactory.CreateDate = createDate;
                        BaleNoFactory.MaxNo = 1;
                        BaleNoFactory.BaleNo = prefix + "1".PadLeft(seqLength, '0');// prefix + year.ToString().Substring(2, 2) + month.ToString("00") + day.ToString("00") +"0001";
                        Dao.Save(BaleNoFactory);
                        MaxBaleNo = BaleNoFactory.BaleNo;
                    }

                }
                else
                {
                    ISession session = CallContext.GetData("nhsession") as ISession;
                    IDbConnection cnn = session.Connection;

                    IDataReader dataReader = null;
                    IDbCommand cmd = cnn.CreateCommand();
                    ITransaction tran = CallContext.GetData("ntransaction") as ITransaction;
                    tran.Enlist(cmd);
                    cmd.CommandText =
                                    "select top 1 BaleNo\n" +
                                    "from dbo.ResPracticState\n" +
                                    "where BaleNo like '" + prefix + "%'\n" +
                                    "order by BaleNo desc";
                    if (dataReader != null && !dataReader.IsClosed)
                    {
                        dataReader.Close();
                    }
                    dataReader = cmd.ExecuteReader(CommandBehavior.Default);

                    bool isExist = false;
                    while (dataReader.Read())
                    {
                        isExist = true;
                        try
                        {
                            long maxNo = ClientUtil.ToLong(ClientUtil.ToString(dataReader["BaleNo"]).Substring(prefix.Length, seqLength));
                            string baleNo = prefix + ((long)(maxNo + 1)).ToString("").PadLeft(seqLength, '0');//prefix + year.ToString().Substring(2, 2) + month.ToString("00") + day.ToString("00") + ((long)(maxNo+1)).ToString("0000");
                            MaxBaleNo = baleNo;
                        }
                        catch
                        {
                            throw new Exception("捆包号[" + ClientUtil.ToString(dataReader["BaleNo"]) + "]错误，请检查或通知管理员进行修改！");
                        }
                    }
                    dataReader.Close();
                    if (!isExist)
                    {
                        BaleNoFactory BaleNoFactory = new BaleNoFactory();
                        BaleNoFactory.Prefix = prefix;
                        BaleNoFactory.Year = (long)year;
                        BaleNoFactory.Month = (long)month;
                        BaleNoFactory.CreateDate = createDate;
                        BaleNoFactory.MaxNo = 1;
                        BaleNoFactory.BaleNo = prefix + "1".PadLeft(seqLength, '0');// prefix + year.ToString().Substring(2, 2) + month.ToString("00") + day.ToString("00") +"0001";
                        Dao.Save(BaleNoFactory);
                        MaxBaleNo = BaleNoFactory.BaleNo;
                    }
                    #region 屏蔽使用Dao查询，有Bug，如果查询出来的实例，在原来类上存在，则报错

                    //oq.AddCriterion(Expression.Like("BaleNo", prefix, MatchMode.Start));

                    //oq.AddOrder(new NHibernate.Criterion.Order("BaleNo", false));
                    //lst = Dao.ObjectQuery(typeof(PracticalityState), oq);

                    //if (lst.Count == 0)//当月新第一条记录
                    //{
                    //    BaleNoFactory BaleNoFactory = new BaleNoFactory();
                    //    BaleNoFactory.Prefix = prefix;
                    //    BaleNoFactory.Year = (long)year;
                    //    BaleNoFactory.Month = (long)month;
                    //    BaleNoFactory.CreateDate = createDate;
                    //    BaleNoFactory.MaxNo = 1;
                    //    BaleNoFactory.BaleNo = prefix + "1".PadLeft(seqLength, '0');// prefix + year.ToString().Substring(2, 2) + month.ToString("00") + day.ToString("00") +"0001";
                    //    Dao.Save(BaleNoFactory);
                    //    MaxBaleNo = BaleNoFactory.BaleNo;
                    //}
                    //else
                    //{
                    //    PracticalityState tmp1 = lst[0] as PracticalityState;
                    //    try
                    //    {
                    //        //处理获得的记录

                    //        long maxNo = ClientUtil.ToLong(tmp1.BaleNo.Substring(prefix.Length, seqLength));//tmp1.MaxNo;
                    //        string baleNo = prefix + ((long)(maxNo + 1)).ToString("").PadLeft(seqLength, '0');//prefix + year.ToString().Substring(2, 2) + month.ToString("00") + day.ToString("00") + ((long)(maxNo+1)).ToString("0000");
                    //        //tmp1.MaxNo += 1;
                    //        //tmp1.CreateDate = createDate;
                    //        //tmp1.BaleNo = baleNo;
                    //        //Dao.Update(tmp1);
                    //        MaxBaleNo = baleNo;
                    //    }
                    //    catch
                    //    {
                    //        throw new Exception("捆包号[" + tmp1.BaleNo + "]错误，请检查或通知管理员进行修改！");
                    //    }
                    //}
                    #endregion
                }
                //oq.AddCriterion(Expression.Eq("Year", (long)year));
                //oq.AddCriterion(Expression.Eq("Month", (long)month));
            }

            #endregion
            #region 初始化暂时屏蔽
            //oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("Prefix", prefix));
            ////oq.AddCriterion(Expression.Eq("Year", (long)year));
            ////oq.AddCriterion(Expression.Eq("Month", (long)month));
            //IList lst = Dao.ObjectQuery(typeof(BaleNoFactory), oq);

            //if (lst.Count == 0)//当月新第一条记录
            //{
            //    BaleNoFactory BaleNoFactory = new BaleNoFactory();
            //    BaleNoFactory.Prefix = prefix;
            //    BaleNoFactory.Year = (long)year;
            //    BaleNoFactory.Month = (long)month;
            //    BaleNoFactory.CreateDate = createDate;
            //    BaleNoFactory.MaxNo = 1;
            //    BaleNoFactory.BaleNo = prefix + "1".PadLeft(seqLength, '0');// prefix + year.ToString().Substring(2, 2) + month.ToString("00") + day.ToString("00") +"0001";
            //    Dao.Save(BaleNoFactory);
            //    MaxBaleNo = BaleNoFactory.BaleNo;
            //}
            //else
            //{
            //    BaleNoFactory tmp1 = lst[0] as BaleNoFactory;
            //    //处理获得的记录
            //    long maxNo = tmp1.MaxNo;
            //    string baleNo = prefix + ((long)(maxNo + 1)).ToString("").PadLeft(seqLength, '0');//prefix + year.ToString().Substring(2, 2) + month.ToString("00") + day.ToString("00") + ((long)(maxNo+1)).ToString("0000");
            //    tmp1.MaxNo += 1;
            //    tmp1.CreateDate = createDate;
            //    tmp1.BaleNo = baleNo;
            //    Dao.Update(tmp1);
            //    MaxBaleNo = tmp1.BaleNo;
            //}
            #endregion
            return MaxBaleNo;
        }
    }
}
