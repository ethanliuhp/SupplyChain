using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Business.Erp.SupplyChain.ApprovalMng.AppSolutionMng.Service
{
    public class SqlScript
    {
        public static string GetApprovingBillSql
        {
            get { return @"
                with tmp1 as(
                     select e.operationjob,e.operationjobname,
                            e.operationrole,e.operationrolename
                     from resoperationjobwithrole e
                     where e.operationjob = '{0}' 
                ),
                tmp2 as(
                     select d.parentid,d.approle,d.rolename from thd_approleset d
                     where exists(
                           select 1 from tmp1 t where d.approle = t.operationrole
                     )
                ),
                tmp3 as(
                     select c.steporder,c.stepsname,c.parentid,c.apptableset,c.apprelations
                     from thd_appstepsset c
                     where exists(
                           select 1 from tmp2 t where t.parentid = c.id
                     )
                ),
                tmp4 as(
                     select a.LASTMODIFYBY,a.LASTMODIFTIME,a.ISDONE,a.APPSOLUTIONNAME,a.nextstep,
                            a.APPSOLUTION,a.APPTABLESET,a.BILLCREATEPERSONNAME,a.BILLCREATEPERSON,
                            a.BILLCREATEDATE,a.BILLSYSCODE,a.BILLCODE,a.BILLID,a.ID,a.version,a.projectid,a.projectname
                     from thd_approvebill a 
                     where a.isdone = 0
                           and exists(
                               select 1 from tmp3 t 
                               where a.apptableset = t.APPTABLESET and a.appsolution = t.PARENTID
                                     and a.nextstep >= t.steporder
                           )
                           and not exists(
                               select 1 from thd_appstepsinfo s
                               where s.billid = a.billid and s.state = 1 and s.steporder <= a.nextstep
                                     and exists(
                                         select 1 from tmp1 t where t.operationrole = s.approle
                                     )
                                     --and s.auditperson = '{1}'
                           ) {2}
                )

                select * from tmp4"; }
        }

        /// <summary>获取审批步骤信息 已经审核和正需要审核</summary>
        public static string GetApproveStepInfoSql
        {
            get
            {
                return @"with Approved as(select t.appstepsset stepid,t.stepsname ,t.appdate,t.appcomments,to_char(t1.pername) appPerson,to_char(t.rolename) rolename,to_char(t.approle)approle,t.steporder,t.state 
                                            from  thd_appstepsinfo t 
                                            left join resperson t1 on t.auditperson=t1.perid
                                            where t.billid='{0}' and t.state=1 and t.appstatus=2),
                        Approving as (select t1.id stepid,t1.stepsname,sysdate appdate,N'' appcomments,'', t3.rolename,t2.approle,t1.steporder ,0 state 
                                                from thd_approvebill t
                                                join thd_appstepsset t1 on t.appsolution=t1.parentid and  t.nextstep=t1.steporder
                                                join thd_approleset t2 on t1.id=t2.parentid  and t2.approle not in 
                                                        (select t.approle from thd_appstepsinfo t where t.billid='{0}'  and t.state=1 and t.appstatus=2)
                                                join resoperationrole t3 on t2.approle=t3.id
                                                join resoperationjobwithrole t4 on t4.operationrole=t3.id and t4.operationjob='{1}'
                                                where t.billid='{0}' and rownum=1 ),
                       total as (select * from Approved union all select * from Approving)
                       select * from total order by appdate asc ,steporder asc";
            }
        }
    }
}
