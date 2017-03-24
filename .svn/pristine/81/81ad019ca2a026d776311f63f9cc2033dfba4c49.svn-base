using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost
{
    public class SQLScript
    {
        public static string SubFinanceCostSql
        {
            get
            {
                return @"
                select  t.operorgname as pname,sum(t1.money) as totalMoney
                 from thd_indirectcostmaster t
                 join thd_indirectcostdetail t1 on t.id = t1.parentid
                 where  1=1 and t.state = 5 
                        and t.createdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd') 
                        and t1.accounttitlecode = '6603'
                        and instr(t.opgsyscode,'{2}')>0        
                        {3}
                group by t.operorgname";
            }
        }

        public static string FinanceCostSql
        {
            get
            {
                return @"
                select o.opgname,sum(t1.money) as totalMoney
                 from thd_indirectcostmaster t
                 join thd_indirectcostdetail t1 on t.id = t1.parentid
                 join resoperationorg o on o.opgoperationtype in('b','hd') and instr(t.opgsyscode,o.opgsyscode)>0
                 where  1=1 and t.state = 5 
                        and t.createdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd') 
                        and t1.accounttitlecode = '6603'
                        and instr(t.opgsyscode,'{2}')>0
                        {3}
                group by o.opgname";
            }
        }

        public static string FinanceCostByProjectSql
        {
            get
            {
                return @"
                select to_char(t.createdate,'yyyy-mm-dd') as createdate,sum(t1.money) as totalMoney
                 from thd_indirectcostmaster t
                 join thd_indirectcostdetail t1 on t.id = t1.parentid
                 where  1=1 and t.state = 5 
                        and t.createdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd') 
                        and t1.accounttitlecode = '6603'
                        and t.projectid = '{2}'
                group by to_char(t.createdate,'yyyy-mm-dd')";
            }
        }

        public static string SubCurrencyExchangeSql
        {
            get
            {
                return @"
                 select 
                      to_char(t.createdate,'yyyy-mm-dd') as createdate,t1.money
                 from thd_indirectcostmaster t
                 join thd_indirectcostdetail t1 on t.id = t1.parentid
                 where  1=1 and t.state = 5  and t.createdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd') 
                        and t1.accounttitlecode = '4104'
                        and instr(t.opgsyscode,'{2}')>0 and t.projectid is null";
            }
        }

        public static string CurrencyExchangeSql
        {
            get
            {
                return @"
                 select 
                        to_char(t.createdate,'yyyy-mm-dd') as createdate,t1.money
                 from thd_indirectcostmaster t
                 join thd_indirectcostdetail t1 on t.id = t1.parentid
                 where  1=1 and t.state = 5  and t.createdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd') 
                        and t1.accounttitlecode = '4104'
                        and instr(t.opgsyscode,'{2}')>0 and t.projectid is null
                        and not exists(
                            select 1 from resoperationorg o 
                            where o.opgoperationtype = 'b' and instr(t.opgsyscode,o.opgsyscode)>0
                        )";
            }
        }

        public static string CurrencyExchangeByProjectSql
        {
            get
            {
                return @"
                 select 
                        to_char(t.createdate,'yyyy-mm-dd') as createdate,t1.money
                 from thd_indirectcostmaster t
                 join thd_indirectcostdetail t1 on t.id = t1.parentid
                 where  1=1 and t.state = 5  
                        and t.createdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd') 
                        and t1.accounttitlecode = '4104'
                        and t.projectid = '{2}'
                        ";
            }
        }

        public static string SubBorrowMoneySql
        {
            get
            {
                return @"
                select to_char(t.createdate,'yyyy-mm-dd') as createdate,m.money
                from thd_borrowedordermaster t
                join thd_borrowedorderdetail m on t.id = m.parentid
                where t.createdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd') 
                      and instr(t.opgsyscode,'{2}')>0 and t.projectid is null
                      and t.state = 5 ";
            }
        }

        public static string BorrowMoneySql
        {
            get
            {
                return @"
                select 
                     to_char(t.createdate,'yyyy-mm-dd') as createdate,t1.money
                from thd_indirectcostmaster t
                join thd_indirectcostdetail t1 on t.id = t1.parentid
                where  1=1 and t.state = 5 
                      and t.createdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd') 
                      and t1.accounttitlecode = '2001'
                      and instr(t.opgsyscode,'{2}')>0 and t.projectid is null
                      and not exists(
                          select 1 from resoperationorg o 
                          where o.opgoperationtype = 'b' and instr(t.opgsyscode, o.opgsyscode)>0
                      )";
            }
        }

        public static string BorrowMoneyByProjectSql
        {
            get
            {
                return @"
                select 
                     to_char(t.createdate,'yyyy-mm-dd') as createdate,t1.money
                from thd_borrowedordermaster t
                join thd_borrowedorderdetail t1 on t.id = t1.parentid
                where  1=1 and t.state = 5 
                       and t.createdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd') 
                       and t.projectid = '{2}'
                      ";
            }
        }
    }
}
