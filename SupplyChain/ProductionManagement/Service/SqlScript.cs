using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.Service
{
    public class SqlScript
    {
        public static string GetProjectDelaysSql1
        {
            get
            {
                return @"
                with tmpProjLife as(
                     select r.id,floor(r.enddate - r.begindate) as projectLiveDays 
                     from resconfig r
                )
                ,tmpPlan as(
                  select a.theprojectguid,sum(nvl(a.palntime,0)) as planTotalDays
                  from thd_gwbstree a
                  where a.categorynodetype = 2
                  group by a.theprojectguid
                )
                ,tmpDelay as(
                  select g.id as taskId,g.name as taskName,g.addupfigureprogress,
                         g.theprojectguid,g.theprojectname,
                         g.taskplanstarttime,g.taskplanendtime,g.palntime,
                         g.realstartdate,g.realenddate,g.ownerorgsyscode,
                         round(case when floor(sysdate - g.taskplanstarttime) >= g.palntime then 1
                              else floor(sysdate - g.taskplanstarttime) / g.palntime
                         end,5) as planRate,
                         round(case when g.realstartdate is null then floor(sysdate - g.taskplanstarttime)
                              else (floor(sysdate - g.taskplanstarttime) / g.palntime - g.addupfigureprogress / 100) * g.palntime
                         end + 1,0) as delayDays,g.fullpath
                  from thd_gwbstree g
                  where 1 = 1 
                        and g.categorynodetype = 2 and g.palntime > 0
                        and g.addupfigureprogress < 100
                        and(
                               g.realstartdate is not null
                            or (g.realstartdate is null and g.taskplanstarttime < trunc(sysdate))
                        )
                ),tmpCost as(--剔除材料成本
                  select d.parentid as TaskId,j.resourcetypename,
                         sum(nvl(j.RESPONSIBILITILYTOTALPRICE,0)) as totalPrice
                  from thd_gwbsdetailcostsubject j
                  join thd_gwbsdetail d on j.gwbsdetailid = d.id
                  where exists(
                     select 1 from tmpDelay t where t.TaskId  = d.parentid
                  ) and instr(j.resourcecatesyscode,'1.29BioV9QP5T9tJmw1VKARN.')=0 and d.state = 5
                  group by d.parentid,j.resourcetypename
                ),tmpCostDetail as(
                  select t.TaskId,
                         wmsys.wm_concat(to_char(t.resourcetypename) || ':' || to_char(round(t.totalPrice / t3.palntime * t3.delayDays,2))) as costRemark,
                         round(sum(t.totalPrice / t3.palntime * t3.delayDays),2) as totalPrice
                  from tmpCost t
                  join tmpDelay t3 on t.TaskId = t3.TaskId
                  where t.totalPrice <> 0
                  group by t.TaskId
                )

                select t3.*, t4.totalPrice,t4.costRemark,trunc(sysdate) as createDate,
                       round((t3.palntime / t2.planTotalDays) * t3.delayDays * (t1.projectLiveDays / t2.planTotalDays),5) as projectDelayDays,
                       case when t3.delayDays between 1 and 2 then 0
                            when t3.delayDays between 3 and 7 then 1
                            when t3.delayDays between 8 and 14 then 2
                            when t3.delayDays >= 15 then 3
                       end as WarnLevel,
                       t5.opgname
                from tmpDelay t3
                left join tmpProjLife t1 on t1.id = t3.theprojectguid
                left join tmpPlan t2 on t2.theprojectguid = t3.theprojectguid
                left join tmpCostDetail t4 on t4.TaskId = t3.TaskId
                left join(
                       select o.opgsyscode,o.opgname from resoperationorg o where o.opgstate=1 and o.opgoperationtype='b'
                  )t5 on instr(t3.ownerorgsyscode,t5.opgsyscode) > 0
                where 1 = 1 {0}
                order by t3.theprojectname,t3.delayDays desc
                ";
            }
        }

        public static string GetProjectDelaysSql
        {
            get
            {
                return @"
                with tmpDelay as(
                  select g.gwbstree as taskId,g.gwbstreename as taskName,g.taskcompletedpercent as addupfigureprogress,
                         g.projectid as theprojectguid,g.projectname as theprojectname,
                         g.plannedbegindate as taskplanstarttime,g.plannedenddate as taskplanendtime,g.plannedduration as palntime,
                         g.actualbegindate as realstartdate,g.actualenddate as realenddate,m.handlepersonsyscode as ownerorgsyscode,
                         round(case when floor(sysdate - g.plannedbegindate) >= g.plannedduration then 1
                              else floor(sysdate - g.plannedbegindate) / g.plannedduration
                         end,5) as planRate
                        ,round(case
                           when g.actualbegindate is null then floor(sysdate - g.plannedbegindate)
                           else

                              case when nvl(g.taskcompletedpercent,0) > 0 then 
                                   floor(g.actualbegindate+((floor(sysdate - g.plannedbegindate) +1)*100/g.taskcompletedpercent) - g.plannedenddate)
                                   else
                                   floor((g.actualbegindate+ floor(sysdate - g.plannedbegindate) +1 +g.plannedduration)-g.plannedenddate )
                                       end
                           end 
                         ) as delayDays
                 ,g.gwbstreesyscode as fullpath
                  --from thd_gwbstree g
                  from thd_weekscheduledetail g left join thd_weekschedulemaster m 
                       on g.parentid = m.id and m.Execscheduletype = 40
                  where 1 = 1 
                        and g.nodetype = 2 and g.plannedduration > 0
                        and nvl(g.taskcompletedpercent,0) >0
                        and g.taskcompletedpercent < 100
                        and(
                               g.actualbegindate is not null
                            or (g.actualbegindate is null and g.plannedbegindate < trunc(sysdate))
                        )
                ),tmpCost as(--剔除材料成本
                  select d.parentid as TaskId,j.resourcetypename,
                         sum(nvl(j.RESPONSIBILITILYTOTALPRICE,0)) as totalPrice
                  from thd_gwbsdetailcostsubject j
                  join thd_gwbsdetail d on j.gwbsdetailid = d.id
                  where exists(
                     select 1 from tmpDelay t where t.TaskId  = d.parentid
                  ) and instr(j.resourcecatesyscode,'1.29BioV9QP5T9tJmw1VKARN.')=0 and d.state = 5
                  group by d.parentid,j.resourcetypename
                ),tmpCostDetail as(
                  select t.TaskId,
                         wmsys.wm_concat(to_char(t.resourcetypename) || ':' || to_char(round(t.totalPrice / t3.palntime * t3.delayDays,2))) as costRemark,
                         round(sum(t.totalPrice / t3.palntime * t3.delayDays),2) as totalPrice
                  from tmpCost t
                  join tmpDelay t3 on t.TaskId = t3.TaskId
                  where t.totalPrice <> 0
                  group by t.TaskId
                )

                select t3.*, t4.totalPrice,t4.costRemark,trunc(sysdate) as createDate,
                       0 as projectDelayDays,
                       case when t3.delayDays between 1 and 2 then 0
                            when t3.delayDays between 3 and 7 then 1
                            when t3.delayDays between 8 and 14 then 2
                            when t3.delayDays >= 15 then 3
                       end as WarnLevel,
                       t5.opgname
                from tmpDelay t3
                left join tmpCostDetail t4 on t4.TaskId = t3.TaskId
                left join(
                       select o.opgsyscode,o.opgname from resoperationorg o where o.opgstate=1 and o.opgoperationtype='b'
                  )t5 on instr(t3.ownerorgsyscode,t5.opgsyscode) > 0
                where 1 = 1 {0}
                order by t3.theprojectname,t3.delayDays desc
                ";
            }
        }

        public static string GetProjectTotalDelaysSql1
        {
            get { return @"
                select b.projectid,b.projectname,b.createdate,
                       sum(b.projectdelaydays) as  projectdelaydays,
                       sum(b.delaycosts) as delaycosts
                from thd_durationdelaywarn b
                where b.projectid = '{0}'
                      and b.createdate between  to_Date('{1}','yyyy-mm-dd') and to_Date('{2}','yyyy-mm-dd')
                group by b.projectid,b.projectname,b.createdate
                order by b.createdate";
            }
        }

        public static string GetProjectTotalDelaysSql
        {
            get
            {
                return @"
                select b.projectid,b.projectname,b.createdate,
                       b.projectdelaydays,
                       b.delaycosts
                from thd_durationdelaywarn b
                where b.projectid = '{0}'
                      and b.IsProjectDelay =1
                      and b.createdate between  to_Date('{1}','yyyy-mm-dd') and to_Date('{2}','yyyy-mm-dd')
                order by b.createdate";
            }
        }
    }
}
