using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData
{
    public class SQLScript
    {
        public static string CostPaymentReportByProjectSql
        {
            get
            {
                //按项目查询显示该项目自开始到查询日期的所有记录
                return @"
                with tmp as (
                  select extract(month from createdate) as monthNo,
                        extract(year from createdate) as year,
                        sum(t1.money) as totalMoney
                  from thd_indirectcostmaster t
                  join thd_indirectcostdetail t1 on t.id = t1.parentid
                  where t.state=5
                        and t.createdate < add_months(to_date('{0}-{3}-01','yyyy-mm-dd'),1)
                        and  t1.accounttitlecode like '54010106%'
                        and t.projectid = '{2}'
                  group by extract(month from createdate),extract(year from createdate)
                ),
                tmp2 as(
                    select t.year,t.month,
                       sum(t1.SUBPROJECTPAYOUT + t1.PERSONCOST + t1.MATERIALCOST + t1.MECHANICALCOST + t1.OTHERDIRECTCOST) as ContractMoney,
                       sum(t1.CIVILANDSETUPBALANCE) as SetUpPayout,
                       0 as zspayput,
                       sum(t1.MainBusinessTax) as MainBusinessTax,
                       sum(t1.MaterialRemain) as MaterialRemain,
                       sum(t1.TempDeviceRemain) as TempDeviceRemain,
                       sum(t1.LowValueConsumableRemain) as LowValueConsumableRemain,
                       sum(t1.ExchangeMaterialRemain) as ExchangeMaterialRemain,
                       sum(t1.SUBPROJECTPAYOUT + t1.PERSONCOST + t1.MATERIALCOST + t1.MECHANICALCOST + t1.OTHERDIRECTCOST
                         + t1.MainBusinessTax + t1.MaterialRemain + t1.TempDeviceRemain + t1.LowValueConsumableRemain
                         + t1.ExchangeMaterialRemain) summoney
                    from thd_financemultdatamaster t 
                    join thd_financemultdatadetail t1 on t.id=t1.parentid
                    where state=5 
                        and to_date(t.year || '-' || lpad(t.month,2,'0') || '-01','yyyy-mm-dd') <= to_date('{0}-{1}-01','yyyy-mm-dd')
                        and t.projectid = '{2}'
                    group by t.year,t.month
                )
                select nvl(a.year,b.year) as year,
                    nvl(a.month,b.monthNo) as month,
                    nvl(a.ContractMoney,0) + nvl(b.totalMoney,0) as ContractMoney,
                    nvl(a.SetUpPayout,0) as SetUpPayout,
                    nvl(a.zspayput,0) as zspayput,
                    nvl(a.MainBusinessTax,0) as MainBusinessTax,
                    nvl(a.MaterialRemain,0) as MaterialRemain,
                    nvl(a.TempDeviceRemain,0) as TempDeviceRemain,
                    nvl(a.LowValueConsumableRemain,0) as LowValueConsumableRemain,
                    nvl(a.ExchangeMaterialRemain,0) as ExchangeMaterialRemain,
                    nvl(a.summoney,0) + nvl(b.totalMoney,0) as summoney
                from tmp2 a
                full join tmp b on a.year = b.year and b.monthNo = a.month order by year asc,month asc  
                ";
            }
        }

        public static string CostPaymentReportBySubSql
        {
            get
            {
                return @"
                with tmp as (
                  select t.projectid,t.projectname,sum(t1.money) as totalMoney
                  from thd_indirectcostmaster t
                  join thd_indirectcostdetail t1 on t.id = t1.parentid
                  where t.state=5 and t.createdate >= to_date('{0}-01-01','yyyy-mm-dd')
                        and t.createdate < add_months(to_date('{0}-{3}-01','yyyy-mm-dd'),1)
                        and  t1.accounttitlecode like '54010106%'
                        and instr(t.opgsyscode,'{2}')>0
                  group by t.projectid,t.projectname
                ),   
                tmp2 as(             
                    select t.projectid,t.projectname,
                           sum(case when t.year = {0} then t1.SUBPROJECTPAYOUT + t1.PERSONCOST + t1.MATERIALCOST + t1.MECHANICALCOST + t1.OTHERDIRECTCOST else 0 end) as ContractMoney,
                           sum(case when t.year = {0} then t1.CIVILANDSETUPBALANCE else 0 end) as SetUpPayout,
                           0 as zspayput,
                           sum(case when t.year = {0} then t1.MainBusinessTax else 0 end) as MainBusinessTax,
                           sum(t1.MaterialRemain) as MaterialRemain,
                           sum(t1.TempDeviceRemain) as TempDeviceRemain,
                           sum(t1.LowValueConsumableRemain) as LowValueConsumableRemain,
                           sum(t1.ExchangeMaterialRemain) as ExchangeMaterialRemain
                    from thd_financemultdatamaster t 
                    join thd_financemultdatadetail t1 on t.id=t1.parentid
                    where  t.state=5 
                        and to_date(t.year || '-' || lpad(t.month,2,'0') || '-01','yyyy-mm-dd') <= to_date('{0}-{1}-01','yyyy-mm-dd')
                        and instr(t.opgsyscode,'{2}')>0
                    group by t.projectid,t.projectname
                )
                select {0} as year,nvl(a.projectname,b.projectname) as projectname, 
                    nvl(a.ContractMoney,0) + nvl(b.totalMoney,0) as ContractMoney,
                    nvl(a.SetUpPayout,0) as SetUpPayout,
                    nvl(a.zspayput,0) as zspayput,
                    nvl(a.MainBusinessTax,0) as MainBusinessTax,
                    nvl(a.MaterialRemain,0) as MaterialRemain,
                    nvl(a.TempDeviceRemain,0) as TempDeviceRemain,
                    nvl(a.LowValueConsumableRemain,0) as LowValueConsumableRemain,
                    nvl(a.ExchangeMaterialRemain,0) as ExchangeMaterialRemain,
                    nvl(a.ContractMoney,0) + nvl(a.MainBusinessTax,0) + nvl(a.MaterialRemain,0) + nvl(a.TempDeviceRemain,0) 
                  + nvl(a.LowValueConsumableRemain,0) + nvl(a.ExchangeMaterialRemain,0) + nvl(b.totalMoney,0) as summoney
                from tmp2 a
                full join tmp b on b.projectid = a.projectid 
                ";
            }
        }

        public static string CostPaymentReportByHeadquartersSql
        {
            get
            {
                return @"
                with tmp as (
                  select o.opgname,sum(t1.money) as totalMoney
                  from thd_indirectcostmaster t
                  join thd_indirectcostdetail t1 on t.id = t1.parentid
                  join resoperationorg o on o.opgoperationtype = 'b' and instr(t.opgsyscode,o.opgsyscode)>0
                  where t.state=5 and  t.createdate >= to_date('{0}-01-01','yyyy-mm-dd')
                        and t.createdate < add_months(to_date('{0}-{3}-01','yyyy-mm-dd'),1)
                        and  t1.accounttitlecode like '54010106%'
                        and instr(t.opgsyscode,'{2}')>0
                  group by o.opgname
                ),
                tmp2 as(
                    select o.opgname,
                       sum(case when t.year = {0} then t1.SUBPROJECTPAYOUT + t1.PERSONCOST + t1.MATERIALCOST + t1.MECHANICALCOST + t1.OTHERDIRECTCOST else 0 end) as ContractMoney,
                       sum(case when t.year = {0} then t1.CIVILANDSETUPBALANCE else 0 end) as SetUpPayout,
                       0 as zspayput,
                       sum(case when t.year = {0} then t1.MainBusinessTax else 0 end) as MainBusinessTax,
                       sum(t1.MaterialRemain) as MaterialRemain,
                       sum(t1.TempDeviceRemain) as TempDeviceRemain,
                       sum(t1.LowValueConsumableRemain) as LowValueConsumableRemain,
                       sum(t1.ExchangeMaterialRemain) as ExchangeMaterialRemain,
                       sum(t1.SUBPROJECTPAYOUT + t1.PERSONCOST + t1.MATERIALCOST + t1.MECHANICALCOST + t1.OTHERDIRECTCOST
                         + t1.MainBusinessTax + t1.MaterialRemain + t1.TempDeviceRemain + t1.LowValueConsumableRemain
                         + t1.ExchangeMaterialRemain) summoney
                    from thd_financemultdatamaster t 
                    join thd_financemultdatadetail t1 on t.id=t1.parentid
                    join resoperationorg o on o.opgoperationtype = 'b' and instr(t.opgsyscode,o.opgsyscode)>0
                    where  t.state=5 and to_date(t.year || '-' || lpad(t.month,2,'0') || '-01','yyyy-mm-dd') <= to_date('{0}-{1}-01','yyyy-mm-dd')
                        and instr(t.opgsyscode,'{2}')>0
                    group by o.opgname
                )
                select {0} as year,nvl(a.opgname,b.opgname) as opgname,
                    nvl(a.ContractMoney,0) + nvl(b.totalMoney,0) as ContractMoney,
                    nvl(a.SetUpPayout,0) as SetUpPayout,
                    nvl(a.zspayput,0) as zspayput,
                    nvl(a.MainBusinessTax,0) as MainBusinessTax,
                    nvl(a.MaterialRemain,0) as MaterialRemain,
                    nvl(a.TempDeviceRemain,0) as TempDeviceRemain,
                    nvl(a.LowValueConsumableRemain,0) as LowValueConsumableRemain,
                    nvl(a.ExchangeMaterialRemain,0) as ExchangeMaterialRemain,
                    nvl(a.ContractMoney,0) + nvl(a.MainBusinessTax,0) + nvl(a.MaterialRemain,0) + nvl(a.TempDeviceRemain,0) 
                  + nvl(a.LowValueConsumableRemain,0) + nvl(a.ExchangeMaterialRemain,0) + nvl(b.totalMoney,0) as summoney
                from tmp2 a
                full join tmp b on b.opgname = a.opgname
                ";
            }
        }

        public static string MainBusinessInComeReportByProjectSql
        {
            get
            {
                //按项目查询显示该项目自开始到查询日期的所有记录
                return @"
                with tmp as (
                  select extract(month from createdate) as monthNo,
                        extract(year from createdate) as year,
                        sum(t1.money) as totalMoney
                  from thd_indirectcostmaster t
                  join thd_indirectcostdetail t1 on t.id = t1.parentid
                  where  t.state=5
                        {3}and t.createdate >= to_date('{0}-{1}-01','yyyy-mm-dd')
                        and t.createdate < add_months(to_date('{0}-{1}-01','yyyy-mm-dd'),1)
                        and t1.accounttitlecode like '54010106%'
                        and t.projectid = '{2}'
                  group by extract(month from createdate),extract(year from createdate)
                ),               
                tmp2 as(
                    select t.year,t.month,
sum(t1.CivilProjectBalance) as CivilProjectBalance,
	sum(t1.SetUpProjectBuild) as SetUpProjectBuild,
	sum(t1.CivilAndSetUpBalance) as CivilAndSetUpBalance,
	sum(t1.CivilAndSetUpPayout) as CivilAndSetUpPayout,
	sum(t1.SetUpPayout) as SetUpPayout,
                           sum(t1.SubProjectPayout+t1.PersonCost+t1.MaterialCost+t1.MechanicalCost+t1.OtherDirectCost+t1.IndirectCost+t1.ContractGrossProfit) as sumMoney,
                           sum(t1.SubProjectPayout) as SubProjectPayout,
                           sum(t1.PersonCost) as PersonCost,
                           sum(t1.MaterialCost) as MaterialCost,
                           sum(t1.MechanicalCost) as MechanicalCost,
                           sum(t1.OtherDirectCost) as OtherDirectCost,
                           sum(t1.ContractGrossProfit) as ContractGrossProfit
                    from thd_financemultdatamaster t
                    join thd_financemultdatadetail t1 on t.id = t1.parentid
                    where  t.state=5
                        {3}and to_date(t.year || '-' || lpad(t.month,2,'0') || '-01','yyyy-mm-dd') >= to_date('{0}-{1}-01','yyyy-mm-dd')
                        and to_date(t.year || '-' || lpad(t.month,2,'0') || '-01','yyyy-mm-dd') <= to_date('{0}-{1}-01','yyyy-mm-dd')
                        and t.projectid = '{2}'
                    group by t.year,t.month
                )
                select nvl(a.year,b.year) as year,nvl(a.month,b.monthNo) as month,
nvl(a.CivilProjectBalance, 0) as CivilProjectBalance,
	nvl(a.SetUpProjectBuild, 0) as SetUpProjectBuild,
	nvl(a.CivilAndSetUpBalance, 0) as CivilAndSetUpBalance,
	nvl(a.CivilAndSetUpPayout, 0) as CivilAndSetUpPayout,
	nvl(a.SetUpPayout, 0) as SetUpPayout,
                       nvl(a.sumMoney,0) + nvl(b.totalMoney,0) AS sumMoney,
                       nvl(a.SubProjectPayout,0) as SubProjectPayout, nvl(a.PersonCost,0) as PersonCost,
                       nvl(a.MaterialCost,0) as MaterialCost,
                       nvl(a.MechanicalCost,0) as MechanicalCost,
                       nvl(a.OtherDirectCost,0) as OtherDirectCost,
                       nvl(b.totalMoney,0) as IndirectCost,
                       nvl(a.ContractGrossProfit,0) as ContractGrossProfit
                from tmp2 a
                full join tmp b on b.year = a.year and b.monthNo = a.month
                order by year,month";
            }
        }

        public static string MainBusinessInComeReportBySubSql
        {
            get
            {
                return @"
                with tmp as (
                  select t.projectid,t.projectname,sum(t1.money) as totalMoney
                  from thd_indirectcostmaster t
                  join thd_indirectcostdetail t1 on t.id = t1.parentid
                  where t.state=5 
                        and t.createdate >= to_date('{0}-{4}-01','yyyy-mm-dd')
                        and t.createdate < add_months(to_date('{0}-{3}-01','yyyy-mm-dd'),1)
                        and instr(t.opgsyscode,'{2}')>0 
                        and  t1.accounttitlecode like '54010106%' 
                  group by t.projectid,t.projectname
                ),
                tmp2 as(
                  select t.projectid,t.projectname,
sum(t1.CivilProjectBalance) as CivilProjectBalance,
	sum(t1.SetUpProjectBuild) as SetUpProjectBuild,
	sum(t1.CivilAndSetUpBalance) as CivilAndSetUpBalance,
	sum(t1.CivilAndSetUpPayout) as CivilAndSetUpPayout,
	sum(t1.SetUpPayout) as SetUpPayout,
                         sum(t1.SubProjectPayout) as SubProjectPayout,
                         sum(t1.PersonCost) as PersonCost,
                         sum(t1.MaterialCost) as MaterialCost,
                         sum(t1.MechanicalCost) as MechanicalCost,
                         sum(t1.OtherDirectCost) as OtherDirectCost,
                         sum(t1.ContractGrossProfit) as ContractGrossProfit, 
                         sum(t1.SubProjectPayout+t1.PersonCost+t1.MaterialCost+t1.MechanicalCost+t1.OtherDirectCost+t1.IndirectCost+t1.ContractGrossProfit) as sumMoney
                  from thd_financemultdatamaster t
                  join thd_financemultdatadetail t1 on t.id = t1.parentid
                  where  t.state=5 and t.year={0} and t.month {5} {1} 
                        and instr(t.opgsyscode,'{2}')>0
                  group by t.projectid,t.projectname
                )
                select {0} as year,
                       nvl(a.projectname,b.projectname) as projectname,
nvl(a.CivilProjectBalance, 0) as CivilProjectBalance,
	nvl(a.SetUpProjectBuild, 0) as SetUpProjectBuild,
	nvl(a.CivilAndSetUpBalance, 0) as CivilAndSetUpBalance,
	nvl(a.CivilAndSetUpPayout, 0) as CivilAndSetUpPayout,
	nvl(a.SetUpPayout, 0) as SetUpPayout,
                       nvl(a.sumMoney,0) + nvl(b.totalMoney,0) AS sumMoney,
                       nvl(a.SubProjectPayout,0) as SubProjectPayout, nvl(a.PersonCost,0) as PersonCost,
                       nvl(a.MaterialCost,0) as MaterialCost,
                       nvl(a.MechanicalCost,0) as MechanicalCost,
                       nvl(a.OtherDirectCost,0) as OtherDirectCost,
                       nvl(b.totalMoney,0) as IndirectCost,
                       nvl(a.ContractGrossProfit,0) as ContractGrossProfit
                from tmp2 a
                full join tmp b on a.projectid = b.projectid
                order by projectname";
            }
        }

        public static string MainBusinessInComeReportByHeadquartersSql
        {
            get
            {
                return @"
                with tmp as (
                  select o.opgname,sum(t1.money) as totalMoney
                  from thd_indirectcostmaster t
                  join thd_indirectcostdetail t1 on t.id = t1.parentid
                  join resoperationorg o on o.opgoperationtype = 'b' and instr(t.opgsyscode,o.opgsyscode)>0
                  where  t.state=5 and t.createdate >= to_date('{0}-{4}-01','yyyy-mm-dd')
                        and t.createdate < add_months(to_date('{0}-{3}-01','yyyy-mm-dd'),1)
                        and instr(t.opgsyscode,'{2}')>0 
                        and  t1.accounttitlecode like '54010106%' 
                  group by o.opgname
                ),
                tmp2 as(
                    select o.opgname,
sum(t1.CivilProjectBalance) as CivilProjectBalance,
	sum(t1.SetUpProjectBuild) as SetUpProjectBuild,
	sum(t1.CivilAndSetUpBalance) as CivilAndSetUpBalance,
	sum(t1.CivilAndSetUpPayout) as CivilAndSetUpPayout,
	sum(t1.SetUpPayout) as SetUpPayout,
                           sum(t1.SubProjectPayout+t1.PersonCost+t1.MaterialCost+t1.MechanicalCost+t1.OtherDirectCost+t1.IndirectCost+t1.ContractGrossProfit) as sumMoney,
                           sum(t1.SubProjectPayout) as SubProjectPayout,
                           sum(t1.PersonCost) as PersonCost,
                           sum(t1.MaterialCost) as MaterialCost,
                           sum(t1.MechanicalCost) as MechanicalCost,
                           sum(t1.OtherDirectCost) as OtherDirectCost,
                           sum(t1.ContractGrossProfit) as ContractGrossProfit
                    from thd_financemultdatamaster t
                    join thd_financemultdatadetail t1 on t.id = t1.parentid
                    join resoperationorg o on o.opgoperationtype = 'b' and instr(t.opgsyscode,o.opgsyscode)>0
                    where  t.state=5 and t.year={0} and t.month {5} {1} and instr(t.opgsyscode,'{2}')>0
                    group by o.opgname
                )
                select {0} as year,
                       nvl(a.opgname,b.opgname) as opgname,
	nvl(a.CivilProjectBalance, 0) as CivilProjectBalance,
	nvl(a.SetUpProjectBuild, 0) as SetUpProjectBuild,
	nvl(a.CivilAndSetUpBalance, 0) as CivilAndSetUpBalance,
	nvl(a.CivilAndSetUpPayout, 0) as CivilAndSetUpPayout,
	nvl(a.SetUpPayout, 0) as SetUpPayout,
                       nvl(a.sumMoney,0) + nvl(b.totalMoney,0) AS sumMoney,
                       nvl(a.SubProjectPayout,0) as SubProjectPayout, nvl(a.PersonCost,0) as PersonCost,
                       nvl(a.MaterialCost,0) as MaterialCost,
                       nvl(a.MechanicalCost,0) as MechanicalCost,
                       nvl(a.OtherDirectCost,0) as OtherDirectCost,
                       nvl(b.totalMoney,0) as IndirectCost,
                       nvl(a.ContractGrossProfit,0) as ContractGrossProfit
                from tmp2 a
                full join tmp b on a.opgname = b.opgname
                order by opgname";
            }
        }

        public static string PaymentInvoiceReportByProjectSql
        {
            get
            {
                return
                @"select p.supplierrelationname,p.accounttitlename,p.supplierscale,
                       p.invoicetype,sum(p.summoney) as summoney,sum(p.taxmoney) as taxmoney,
                       sum(case when p.ifdeduction is null then p.summoney else 0 end) as unDeductionMoney,
                       sum(case when p.ifdeduction is not null then p.summoney else 0 end) as deductionMoney  
                from thd_paymentinvoice p
                where 1 = 1 and p.STATE = 5
                      and p.createdate >= to_date('{0}','yyyy-mm-dd')
                      and p.createdate < to_date('{1}','yyyy-mm-dd')
                      and p.projectid = '{2}'
                group by p.supplierrelationname,p.accounttitlename,p.supplierscale,p.invoicetype
                ";
            }
        } 

        public static string PaymentInvoiceReportBySubSql
        {
            get
            {
                return @"
                select p.projectname,p.invoicetype,
                       sum(p.summoney) as summoney,sum(p.taxmoney) as taxmoney,
                       sum(case when p.ifdeduction is null then p.summoney else 0 end) as unDeductionMoney,
                       sum(case when p.ifdeduction is not null then p.summoney else 0 end) as deductionMoney  
                from thd_paymentinvoice p
                where 1 = 1 and p.STATE = 5
                      and p.createdate >= to_date('{0}','yyyy-mm-dd')
                      and p.createdate < to_date('{1}','yyyy-mm-dd')
                      and instr(p.opgsyscode,'{2}')>0
                group by p.projectname,p.invoicetype";
            }
        }

        public static string PaymentInvoiceReportSql
        {
            get { return @"
                select o.opgname,p.invoicetype,
                       sum(p.summoney) as summoney,sum(p.taxmoney) as taxmoney,
                       sum(case when p.ifdeduction is null then p.summoney else 0 end) as unDeductionMoney,
                       sum(case when p.ifdeduction is not null then p.summoney else 0 end) as deductionMoney  
                from thd_paymentinvoice p
                join resoperationorg o on o.opgstate = 1 and o.opgoperationtype = 'b' and instr(p.opgsyscode,o.opgsyscode) > 0
                where 1 = 1 and p.STATE = 5
                      and p.createdate >= to_date('{0}','yyyy-mm-dd')
                      and p.createdate < to_date('{1}','yyyy-mm-dd')
                group by o.opgname,p.invoicetype"; }
        }

        public static string GetFundSchemeReportAmountAndCostSql
        {
            get { return @"
                with tmp1 as(
                  select * from thd_fundschememaster f
                  where f.id = '{0}'
                ),tmp2 as(
                  select c.*
                  from thd_constructnode c
                  join tmp1 t on t.projectid = c.projectid and c.begindate >= t.schemebegindate and c.enddate <= t.schemeenddate
                ),tmp3 as(
                  select c.id,c.name,d.year,d.month,d.WBSID,d.WBSNAME,d.CurrentRate as Rate
                  from thd_gwbstree c
                  join tmp2 d on d.projectid = c.theprojectguid and instr(c.syscode,d.wbsid)>0
                ),tmp4 as(
                  select d.WBSID,d.WBSNAME,d.RATE,d.year,d.month,s.code,s.name,
                         sum(j.contracttotalprice) as totalprice
                  from thd_gwbsdetail t
                  join tmp3 d on d.id = t.PARENTID
                  join thd_gwbsdetailcostsubject j on j.gwbsdetailid = t.id
                  join THD_COSTACCOUNTSUBJECT s on s.id = j.costaccountsubjectguid 
                  group by d.WBSID,d.WBSNAME,d.RATE,d.year,d.month,s.code,s.name
                )

                select t.year || '年' || t.month || '月' as schemeDate,t.WBSNAME,t.RATE,
                       sum(case when instr(t.code,'C511')>0 then t.totalprice * t.RATE / 100 else 0 end) as zhiJieBL,
                       sum(case when instr(t.code,'C512')>0 or instr(t.code,'C513')>0 or instr(t.code,'C514')>0 or instr(t.code,'C516')>0 then t.totalprice * t.RATE / 100 else 0 end) as cuoShiBL,
                       0 as neiBuBL,0 as jiaFenBL,
                       sum(case when instr(t.code,'C515')>0 then t.totalprice * t.RATE / 100 else 0 end) as xiaoXiangBL,
                       sum(case when instr(t.code,'C51104')>0 then t.totalprice * t.RATE / 100 else 0 end) as zhuanXiangFenBaoCT,
                       sum(case when instr(t.code,'C51101')>0 then t.totalprice * t.RATE / 100 else 0 end) as renGongCT,
                       sum(case when instr(t.code,'C5110203')>0 or instr(t.code,'C5110206')>0 then t.totalprice * t.RATE / 100 else 0 end) as gangCaiCT,  
                       sum(case when instr(t.code,'C5110204')>0 then t.totalprice * t.RATE / 100 else 0 end) as gangCaiCT, 
                       sum(case when instr(t.code,'C51102')>0 and instr(t.code,'C5110203')<=0 and instr(t.code,'C5110206')<=0 and instr(t.code,'C5110204')<=0 then t.totalprice * t.RATE / 100 else 0 end) as qiTaCT,
                       sum(case when instr(t.code,'C5110301')>0 or instr(t.code,'C5110302')>0 or instr(t.code,'C5110303')>0 or instr(t.code,'C5110304')>0 then t.totalprice * t.RATE / 100 else 0 end) as sheBeiCT,
                       sum(case when instr(t.code,'C5110306')>0 then t.totalprice * t.RATE / 100 else 0 end) as shuiDianCT,
                       sum(case when instr(t.code,'C51103')>0 and instr(t.code,'C5110301')<=0 and instr(t.code,'C5110302')<=0 and instr(t.code,'C5110303')<=0 and instr(t.code,'C5110304')<=0 and instr(t.code,'C5110306')<=0 then t.totalprice * t.RATE / 100 else 0 end) as qiTaJiXieCT,
                       sum(case when instr(t.code,'C514')>0 then t.totalprice * t.RATE / 100 else 0 end) as zhengFuCT,
                       sum(case when instr(t.code,'C512')>0 or instr(t.code,'C516')>0 then t.totalprice * t.RATE / 100 else 0 end) as qiTaZhiJieCT,
                       sum(case when instr(t.code,'C513')>0 then t.totalprice * t.RATE / 100 else 0 end) as jianJieCT
                from tmp4 t
                group by t.year,t.month,t.WBSNAME,t.RATE"; }
        }

        public static string QueryFundSchemeReportAmountBySchemeSql
        {
            get { return @"
                with tmp1 as(
                  select * from thd_fundschememaster f
                  where f.id = '{0}'
                ),tmp2 as(
                  select c.*
                  from thd_constructnode c
                  join tmp1 t on t.projectid = c.projectid 
                    and c.begindate >= t.schemebegindate and c.enddate <= t.schemeenddate
                    and c.begindate >= to_DATE('2016-01-01','yyyy-mm-dd')
                ),tmp3 as(
                  select c.id,c.name,d.year,d.month,d.WBSID,d.WBSNAME,d.CurrentRate as Rate
                  from thd_gwbstree c
                  join tmp2 d on d.projectid = c.theprojectguid and instr(c.syscode,d.wbsid)>0
                ),tmp4 as(
                  select d.WBSID,d.WBSNAME,d.RATE,d.year,d.month,s.code,s.name,
                         sum(j.contracttotalprice) as totalprice,
                         sum(j.RESPONSIBILITILYTOTALPRICE) as totalCost
                  from tmp3 d
                  left join thd_gwbsdetail t on d.id = t.PARENTID
                  left join thd_gwbsdetailcostsubject j on j.gwbsdetailid = t.id
                  left join THD_COSTACCOUNTSUBJECT s on s.id = j.costaccountsubjectguid 
                  group by d.WBSID,d.WBSNAME,d.RATE,d.year,d.month,s.code,s.name
                )

                select t.year , t.month ,t.WBSNAME,
                       sum(case when instr(t.code,'C511')>0 then t.totalprice * t.RATE / 100 else 0 end) as zhiJieBL,
                       sum(case when instr(t.code,'C512')>0 or instr(t.code,'C513')>0 or instr(t.code,'C514')>0 or instr(t.code,'C516')>0 then t.totalprice * t.RATE / 100 else 0 end) as cuoShiBL,
                       0 as neiBuBL,0 as jiaFenBL,
                       sum(case when instr(t.code,'C515')>0 then t.totalprice * t.RATE / 100 else 0 end) as xiaoXiangBL,
                       sum(case when instr(t.code,'C51104')>0 then t.totalCost * t.RATE / 100 else 0 end) as zhuanXiangFenBaoCT,
                       sum(case when instr(t.code,'C51101')>0 then t.totalCost * t.RATE / 100 else 0 end) as renGongCT,
                       sum(case when instr(t.code,'C5110203')>0 or instr(t.code,'C5110206')>0 then t.totalCost * t.RATE / 100 else 0 end) as gangCaiCT,  
                       sum(case when instr(t.code,'C5110204')>0 then t.totalCost * t.RATE / 100 else 0 end) as hunNingTuCT, 
                       sum(case when instr(t.code,'C51102')>0 and instr(t.code,'C5110203')<=0 and instr(t.code,'C5110206')<=0 and instr(t.code,'C5110204')<=0 then t.totalCost * t.RATE / 100 else 0 end) as qiTaCT,
                       sum(case when instr(t.code,'C5110301')>0 or instr(t.code,'C5110302')>0 or instr(t.code,'C5110303')>0 or instr(t.code,'C5110304')>0 then t.totalCost * t.RATE / 100 else 0 end) as sheBeiCT,
                       sum(case when instr(t.code,'C5110306')>0 then t.totalCost * t.RATE / 100 else 0 end) as shuiDianCT,
                       sum(case when instr(t.code,'C51103')>0 and instr(t.code,'C5110301')<=0 and instr(t.code,'C5110302')<=0 and instr(t.code,'C5110303')<=0 and instr(t.code,'C5110304')<=0 and instr(t.code,'C5110306')<=0 then t.totalCost * t.RATE / 100 else 0 end) as qiTaJiXieCT,
                       sum(case when instr(t.code,'C514')>0 then t.totalCost * t.RATE / 100 else 0 end) as zhengFuCT,
                       sum(case when instr(t.code,'C512')>0 or instr(t.code,'C516')>0 then t.totalCost * t.RATE / 100 else 0 end) as qiTaZhiJieCT,
                       sum(case when instr(t.code,'C513')>0 then t.totalCost * t.RATE / 100 else 0 end) as jianJieCT
                from tmp4 t
                group by t.year,t.month,t.WBSNAME
                order by t.year,t.month
                ";
            }
        }

        public static string QueryLastYearFundSchemeReportAmountSql
        {
            get
            {
                return @"
                with tmp1 as(
                  select * from thd_fundschememaster f
                  where f.id = '{0}'
                ),tmp2 as(
                  select c.*
                  from thd_constructnode c
                  join tmp1 t on t.projectid = c.projectid and c.enddate < to_date('2016-01-01','yyyy-mm-dd')
                ),tmp3 as(
                  select c.id,c.name,d.year,d.month,d.WBSID,d.WBSNAME,d.CurrentRate as Rate
                  from thd_gwbstree c
                  join tmp2 d on d.projectid = c.theprojectguid and instr(c.syscode,d.wbsid)>0
                ),tmp4 as(
                  select d.WBSID,d.WBSNAME,d.RATE,d.year,d.month,s.code,s.name,
                         sum(j.contracttotalprice) as totalprice,
                         sum(j.RESPONSIBILITILYTOTALPRICE) as totalCost
                  from thd_gwbsdetail t
                  join tmp3 d on d.id = t.PARENTID
                  join thd_gwbsdetailcostsubject j on j.gwbsdetailid = t.id
                  join THD_COSTACCOUNTSUBJECT s on s.id = j.costaccountsubjectguid 
                  group by d.WBSID,d.WBSNAME,d.RATE,d.year,d.month,s.code,s.name
                )

                select 
                       nvl(sum(case when instr(t.code,'C511')>0 then t.totalprice * t.RATE / 100 else 0 end),0) as zhiJieBL,
                       nvl(sum(case when instr(t.code,'C512')>0 or instr(t.code,'C513')>0 or instr(t.code,'C514')>0 or instr(t.code,'C516')>0 then t.totalprice * t.RATE / 100 else 0 end),0) as cuoShiBL,
                       0 as neiBuBL,0 as jiaFenBL,
                       nvl(sum(case when instr(t.code,'C515')>0 then t.totalprice * t.RATE / 100 else 0 end),0) as xiaoXiangBL,
                       nvl(sum(case when instr(t.code,'C51104')>0 then t.totalCost * t.RATE / 100 else 0 end),0) as zhuanXiangFenBaoCT,
                       nvl(sum(case when instr(t.code,'C51101')>0 then t.totalCost * t.RATE / 100 else 0 end),0) as renGongCT,
                       nvl(sum(case when instr(t.code,'C5110203')>0 or instr(t.code,'C5110206')>0 then t.totalCost * t.RATE / 100 else 0 end),0) as gangCaiCT,  
                       nvl(sum(case when instr(t.code,'C5110204')>0 then t.totalCost * t.RATE / 100 else 0 end),0) as hunNingTuCT, 
                       nvl(sum(case when instr(t.code,'C51102')>0 and instr(t.code,'C5110203')<=0 and instr(t.code,'C5110206')<=0 and instr(t.code,'C5110204')<=0 then t.totalCost * t.RATE / 100 else 0 end),0) as qiTaCT,
                       nvl(sum(case when instr(t.code,'C5110301')>0 or instr(t.code,'C5110302')>0 or instr(t.code,'C5110303')>0 or instr(t.code,'C5110304')>0 then t.totalCost * t.RATE / 100 else 0 end),0) as sheBeiCT,
                       nvl(sum(case when instr(t.code,'C5110306')>0 then t.totalCost * t.RATE / 100 else 0 end),0) as shuiDianCT,
                       nvl(sum(case when instr(t.code,'C51103')>0 and instr(t.code,'C5110301')<=0 and instr(t.code,'C5110302')<=0 and instr(t.code,'C5110303')<=0 and instr(t.code,'C5110304')<=0 and instr(t.code,'C5110306')<=0 then t.totalCost * t.RATE / 100 else 0 end),0) as qiTaJiXieCT,
                       nvl(sum(case when instr(t.code,'C514')>0 then t.totalCost * t.RATE / 100 else 0 end),0) as zhengFuCT,
                       nvl(sum(case when instr(t.code,'C512')>0 or instr(t.code,'C516')>0 then t.totalCost * t.RATE / 100 else 0 end),0) as qiTaZhiJieCT,
                       nvl(sum(case when instr(t.code,'C513')>0 then t.totalCost * t.RATE / 100 else 0 end),0) as jianJieCT
                from tmp4 t
                ";
            }
        }

        public static string GetProjectPlanRateSql
        {
            get { return @"
                with tmp1 as(
                     select floor(r.enddate - r.begindate) as totalDays 
                     from resconfig r
                     where r.id = '{0}'
                )
                ,tmp2 as(
                  select sum(nvl(a.palntime,0)) as totalDays
                  from thd_gwbstree a
                  where a.theprojectguid = '{0}' and a.categorynodetype = 2
                )

                select a.totalDays as projectTotalDays,b.totalDays as planTotalDays,
                    case when b.totalDays =0 then 0 else round(a.totalDays / b.totalDays,4) end as rate
                from tmp1 a,tmp2 b"; }
        }

        public static string GetProjectFundFlowSql
        {
            get { return @"
                    with tmp1 as(
                      select sum(d.addupaccquantity) as totalQuantity 
                      from thd_gwbsdetail d
                      join thd_gwbstree t on t.theprojectguid = '{0}' and d.parentid = t.id
                           and d.state = 5 and d.updateddate < to_date('{1}-{2}-01','yyyy-mm-dd')
                    )
                    ,tmp2 as(     
                      select sum(gm.summoney) as totalGathering
                      from thd_gatheringmaster gm 
                      where gm.projectid = '{0}' and gm.state = 5 and gm.createdate < to_date('{1}-{2}-01','yyyy-mm-dd')
                    )
                    ,tmp3 as(
                      select sum(p.summoney) as totalPayment
                      from thd_paymentmaster p
                      where p.projectid = '{0}' and p.state = 5 and p.createdate < to_date('{1}-{2}-01','yyyy-mm-dd')
                    )

                    select nvl(a.totalQuantity,0) as totalQuantity,
                        nvl(b.totalGathering,0) as totalGathering,
                        nvl(c.totalPayment,0) as totalPayment
                    from tmp1 a,tmp2 b,tmp3 c";
            }
        }

        public static string GetFilialeFundFlowSql
        {
            get { return @"
                with tmp1 as(
                  select p.id,p.code,p.projectid,p.projectname
                  from thd_projectfundplanmaster p
                  where inStr('{0}', p.attachbusinessorg)>0
                        and p.year = {1} and p.month = {2} and p.state = 5
                )
                ,tmp2 as(     
                  select sum(gm.summoney) as totalGathering
                  from thd_gatheringmaster gm 
                  join tmp1 t on t.projectid = gm.projectid
                  where gm.state = 5 and gm.createyear = {1} and gm.createmonth < {2}
                )
                ,tmp3 as(
                  select sum(p.summoney) as totalPayment
                  from thd_paymentmaster p
                  join tmp1 t on t.projectid = p.projectid
                  where p.state = 5 and p.createyear = {1} and p.createmonth < {2}
                )

                select 
                    nvl(b.totalGathering,0) as totalGathering,
                    nvl(c.totalPayment,0) as totalPayment
                from tmp2 b,tmp3 c";
            }
        }

        public static string GetPayAccountSql
        {
            get { return @"
            select code,name,id,parentnodeid,tlevel,SYSCODE
            from THD_ACCOUNTTITLETREE
            start with name in('应付账款','其他应付款')
            connect by nocycle prior id=parentnodeid 
            order by code"; }
        }

        public static string GetSupplierOrderSql
        {
            get
            {
                return @"
                select distinct a.supplierrelation,a.supplierrelationname, 
                       f.contractmoney,f.balancestyle,f.processpayrate,f.completepayrate,f.warrantypayrate
                from thd_stockinbaldetail_fwddtl b
                join thd_stockinbaldetail c on c.id = b.stockinbaldetail 
                join thd_stockinbalmaster a on c.parentid = a.id and a.id = '{0}'
                join thd_stkstockindtl d on d.id = b.forwarddetailid
                join thd_supplyorderdetail e on e.id = d.supplyorderdetailid
                join thd_supplyordermaster f on f.id = e.parentid";
            }
        }

        public static string GetUnRelationSubcontractSql
        {
            get
            {
                return @"
                select a.id,a.code,a.balancemoney as billMoney,nvl(i.settlementmoney,0) as relationmoney
                from thd_subcontractbalancebill a
                join thd_subcontractproject p on p.id = a.subcontractprojectid and p.bearerorgguid = '{5}'
                left join(
                     select n.settlement,sum(n.settlementmoney) as settlementmoney
                     from THD_InvoiceSettlementRelation n group by n.settlement
                )i on i.settlement = a.id
                where a.state = {0} and a.projectid = '{1}' and a.balancemoney <> nvl(i.settlementmoney,0)
                      and a.createdate between to_date('{2}','yyyy-mm-dd') and to_date('{3}','yyyy-mm-dd')
                      {4}
                order by a.code";
            }
        }

        public static string GetUnRelationMaterialSettleSql
        {
            get { return @"
                select a.id,a.code,a.SUMMONEY as billMoney,nvl(i.settlementmoney,0) as relationmoney 
                from THD_STOCKINBALMASTER a 
                left join(
                     select n.settlement,sum(n.settlementmoney) as settlementmoney
                     from THD_InvoiceSettlementRelation n group by n.settlement
                )i on i.settlement = a.id
                where a.state = {0} and a.projectid = '{1}' and a.SUMMONEY <> nvl(i.settlementmoney,0)
                      and a.createdate between to_date('{2}','yyyy-mm-dd') and to_date('{3}','yyyy-mm-dd') 
                      and a.SUPPLIERRELATION = '{5}'
                      {4}
                order by a.code";
            }
        }

        public static string GetUnRelationMaterialRentalSql
        {
            get { return @"
                select a.id,a.code,a.SUMMONEY as billMoney,nvl(i.settlementmoney,0) as relationmoney  
                from THD_MaterialRentelSetMaster a
                left join(
                     select n.settlement,sum(n.settlementmoney) as settlementmoney
                     from THD_InvoiceSettlementRelation n group by n.settlement
                )i on i.settlement = a.id
                where a.state = {0} and a.projectid = '{1}' and a.SUMMONEY <> nvl(i.settlementmoney,0)
                      and a.createdate between to_date('{2}','yyyy-mm-dd') and to_date('{3}','yyyy-mm-dd')
                      and a.SUPPLIERRELATION = '{5}'
                      {4}
                order by a.code";
            }
        }
    }
}
