using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using VirtualMachine.Core;
using System.Collections;
using System.ComponentModel;
using NHibernate.Criterion;
using NHibernate;
using VirtualMachine.Component.Util;
using System.Data;
using System.Runtime.Remoting.Messaging;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.Service
{
    public class DurationDelayHelper 
    {

        private DateTime defaultTime = new DateTime(1900, 1, 1);

        private ISession _dbConfig;
        /// <summary>
        /// 数据库配置
        /// </summary>
        public ISession DbConfig
        {
            get
            {
                if (_dbConfig == null)
                {
                    _dbConfig = CallContext.GetData("nhsession") as ISession;
                }
                return _dbConfig;
            }
        }
        //加载总进度计划
        public List<DurationDelayWarn> GetProjectDelayInfo(IList listMaster, string projId)
        {
           

            if (listMaster == null || listMaster.Count == 0)
                return null;

            List<WeekScheduleMaster> listWSDMaster = listMaster.OfType<WeekScheduleMaster>().ToList<WeekScheduleMaster>();

            DateTime lastMonFirDay = new DateTime(1900,1,1);
            DateTime lastMonLastDay=new DateTime(1900,1,1);
            int lastFiscalYear=0;
            int lastFiscalMonth =0;

            MaintainFiscalInfo(ref lastMonFirDay, ref lastMonLastDay, ref lastFiscalYear, ref lastFiscalMonth);
  
            var list = new List<DurationDelayWarn>();
            foreach (var wsdMaster in listWSDMaster)
            {
                if (wsdMaster.Details.Count == 0)
                    continue;
                int delaydays = GetDelayDays(wsdMaster);
                //int delaydays = 53;
                
                var item = new DurationDelayWarn();
                item.ProjectId = wsdMaster.ProjectId;
                item.ProjectName = wsdMaster.ProjectName;
                item.CreateDate = DateTime.Now.Date;
                item.ProjectDelayDays = delaydays;
                item.DelayCosts = GetProjectDelayCost(wsdMaster.ProjectId, lastMonFirDay.ToString("yyyy-MM-dd"), lastMonLastDay.ToString("yyyy-MM-dd"), lastFiscalYear, lastFiscalMonth) * delaydays;
                item.IsProjectDelay = true;

                list.Add(item);

                GetProjectDelayCostDetail(wsdMaster, lastMonFirDay.ToString("yyyy-MM-dd"), lastMonLastDay.ToString("yyyy-MM-dd"), lastFiscalYear, lastFiscalMonth, delaydays, ref list);
            }

            return list;
        }



        private void MaintainFiscalInfo(ref DateTime lastMonFirDay, ref DateTime lastMonLastDay, ref int lastFiscalYear, ref int lastFiscalMonth)
        {
            string sql = string.Format(@"select t.fiscalyear,t.fiscalmonth,t.begindate,t.enddate from (select * from resfiscalperioddet t 
where t.enddate <to_date('{0}','yyyy-MM-dd')
      order by t.enddate desc) t
      where rownum =1", DateTime.Now.ToString("yyyy-MM-dd"));

            DataTable dt = ExecuteTable(sql);
            if (dt == null || dt.Rows.Count == 0)
            {
                return ;
            }


            lastMonFirDay = ClientUtil.ToDateTime(dt.Rows[0]["begindate"]);
            lastMonLastDay = ClientUtil.ToDateTime(dt.Rows[0]["enddate"]).Date;
            lastFiscalYear = ClientUtil.ToInt(dt.Rows[0]["fiscalyear"]);
            lastFiscalMonth = ClientUtil.ToInt(dt.Rows[0]["fiscalmonth"]);
        }

        private void GetProjectDelayCostDetail(WeekScheduleMaster wsdMaster, string lastMonFirDay, string lastMonLastDay, int lastFiscalYear, int lastFiscalMonth, decimal delaydays, ref  List<DurationDelayWarn> list)
        {
            int days = (ClientUtil.ToDateTime(lastMonLastDay) - ClientUtil.ToDateTime(lastMonFirDay)).Days + 1;

            decimal money_jx = 0, money_lj = 0, money_cw = 0;
            string sql = "";
            DataTable dt = null;

            int warnLevel = GetWarnLevel(ClientUtil.ToInt(delaydays));

            //机械租赁费 ——机械租赁结算单 明细
            sql = string.Format(@"select t4.material,t4.materialname,sum(t4.settlemoney) as settlemoney
                                    from THD_MaterialRentelSetMaster t2
                                            left join thd_materialrentalsetdetail t4 on t4.parentid = t2.id
                                    where 1 = 1
                                           and t2.state = 5
                                           and t2.projectid = '{0}'
                                           and t2.createdate >= to_date('{1}', 'yyyy-MM-dd')
                                           and t2.createdate <= to_date('{2}', 'yyyy-MM-dd')
                                           group by t4.material,t4.materialname", wsdMaster.ProjectId, lastMonFirDay, lastMonLastDay);
            dt = ExecuteTable(sql);

            if (dt != null && dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    money_jx = ClientUtil.ToDecimal(dr["settlemoney"]);
                    var item = new DurationDelayWarn();
                    item.ProjectId = wsdMaster.ProjectId;
                    item.ProjectName = wsdMaster.ProjectName;
                    item.CreateDate = DateTime.Now.Date;
                    item.DelayDays = ClientUtil.ToInt(delaydays);
                    item.TaskName = ClientUtil.ToString(dr["materialname"]) + "租赁";
                    item.DelayCosts = money_jx / days * delaydays;
                    item.WarnLevel = warnLevel;
                    item.IsProjectDelay = false;
                    list.Add(item); 
                }
               
            }

            //料具租赁费 ——料具租赁结算单
            sql = string.Format("select sum(t.summatmoney) as summoney from thd_MaterialBalanceMaster t  where 1=1 and t.state = 5 and t.projectid = '{0}' and  t.fiscalyear = {1} and t.fiscalmonth = {2}  ", wsdMaster.ProjectId, lastFiscalYear, lastFiscalMonth);

            dt = ExecuteTable(sql);

            if (dt != null && dt.Rows.Count != 0)
            {
                money_lj = ClientUtil.ToDecimal(dt.Rows[0]["summoney"]);
                var item = new DurationDelayWarn();
                item.ProjectId = wsdMaster.ProjectId;
                item.ProjectName = wsdMaster.ProjectName;
                item.CreateDate = DateTime.Now.Date;
                item.DelayDays = ClientUtil.ToInt(delaydays); 
                item.TaskName = "料具租赁费";
                item.DelayCosts = money_lj / days * delaydays;
                item.WarnLevel = warnLevel;
                item.IsProjectDelay = false;
                list.Add(item);
            }

            //费用 ——费用结算单
            sql = string.Format(@"select t2.accountcostsubject,t2.accountcostname,sum(t2.money) as money
                                      from thd_ExpensesSettleMaster t1
                                           left join thd_expensessettledetail t2 on t2.parentid = t1.id
                                     where 1 = 1
                                       and t1.state = 5
                                       and t1.projectid = '{0}'
                                       and t1.createdate >= to_date('{1}', 'yyyy-MM-dd')
                                       and t1.createdate <= to_date('{2}', 'yyyy-MM-dd')
                                       group by t2.accountcostsubject,t2.accountcostname", wsdMaster.ProjectId, lastMonFirDay, lastMonLastDay);
            dt = ExecuteTable(sql);

            if (dt != null && dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    money_cw = ClientUtil.ToDecimal(dr["money"]);
                    var item = new DurationDelayWarn();
                    item.ProjectId = wsdMaster.ProjectId;
                    item.ProjectName = wsdMaster.ProjectName;
                    item.CreateDate = DateTime.Now.Date;
                    item.DelayDays = ClientUtil.ToInt(delaydays); 
                    item.TaskName = ClientUtil.ToString(dr["accountcostname"]);
                    item.DelayCosts = money_cw / days * delaydays;
                    item.WarnLevel = warnLevel;
                    item.IsProjectDelay = false;
                    list.Add(item);
                }
               
            }

            
        }

        private int GetWarnLevel(int p)
        {
            int rtn_WL = 0;
            if (p >= 0 && p <= 2)
                rtn_WL = 0;
            else if (p >= 3 && p <= 7)
                rtn_WL = 1;
            else if (p >= 8 && p <= 14)
                rtn_WL = 2;
            else if (p >= 15)
                rtn_WL = 3;
            return rtn_WL; 
        }
        private decimal GetProjectDelayCost(string projId, string  lastMonFirDay, string lastMonLastDay, int lastFiscalYear, int lastFiscalMonth)
        {
            int days = (ClientUtil.ToDateTime(lastMonLastDay) - ClientUtil.ToDateTime(lastMonFirDay)).Days + 1;

            decimal money_jx = 0,money_lj =0 ,money_cw =0;
            string sql = "";
            DataTable dt = null;


            //机械租赁费 ——机械租赁结算单 明细
            sql = string.Format(@"select sum(t.summoney) as summoney from THD_MaterialRentelSetMaster t
                                where 1=1 
                                and t.projectid='{0}'
                                and t.createdate >=to_date('{1}','yyyy-MM-dd')
                                and t.createdate<=to_date('{2}','yyyy-MM-dd')", projId, lastMonFirDay, lastMonLastDay);
            dt = ExecuteTable(sql);

            if (dt != null && dt.Rows.Count != 0)
            {
                money_jx = ClientUtil.ToDecimal(dt.Rows[0]["summoney"]);
            }

            //料具租赁费 ——料具租赁结算单

            sql = string.Format("select sum(t.summatmoney) as summoney from thd_MaterialBalanceMaster t  where 1=1 and t.projectid = '{0}' and  t.fiscalyear = {1} and t.fiscalmonth = {2}  ", projId, lastFiscalYear, lastFiscalMonth);

            dt = ExecuteTable(sql);

            if (dt != null && dt.Rows.Count != 0)
            {
                money_lj = ClientUtil.ToDecimal(dt.Rows[0]["summoney"]);
            }
            //费用 ——费用结算单 明细
            sql = string.Format(@"select sum(t.summoney) as summoney from thd_ExpensesSettleMaster t  
                                 where 1=1 
                                and t.projectid='{0}'
                                and t.createdate >=to_date('{1}','yyyy-MM-dd')
                                and t.createdate<=to_date('{2}','yyyy-MM-dd')", projId, lastMonFirDay, lastMonLastDay);
            dt = ExecuteTable(sql);

            if (dt != null && dt.Rows.Count != 0)
            {
                money_cw = ClientUtil.ToDecimal(dt.Rows[0]["summoney"]);
            }

            return (money_jx + money_lj　+　money_cw)/days;
        }

        private bool IsExistsInWsdls(WeekScheduleMaster WSDMaster,WeekScheduleDetail detail)
        {
            foreach (var item in WSDMaster.Details)
            {
                if (item.RalationDetails == null || item.RalationDetails.Count == 0 )
                    continue;
                //以当前节点作为前置节点
                WeekScheduleRalation wsdr_frind = item.RalationDetails.ToList<WeekScheduleRalation>().Find(a => a.FrontWeekScheduleDetail.Id == detail.Id);
                if (wsdr_frind != null)
                {
                    return true;
                }
            }
            return false;
        }

        private int GetDelayDays(WeekScheduleMaster WSDMaster)
        {
            //获取总进度计划中当前正在施工的工程任务
            var aa = from a in WSDMaster.Details
                     where (a.PlannedBeginDate != null && a.PlannedBeginDate != defaultTime)
                     && (a.PlannedEndDate != null && a.PlannedBeginDate != defaultTime)
                     && (a.ActualBeginDate != null && a.ActualBeginDate != defaultTime)
                     && (a.TaskCompletedPercent > (decimal)0 && a.TaskCompletedPercent < (decimal)100)
                     select a;

            if (aa == null || aa.Count() == 0)
            {
                var bb = WSDMaster.Details.Where<WeekScheduleDetail>(a => IsExistsInWsdls(WSDMaster, a) && a.PlannedBeginDate > defaultTime);
                if (bb == null || bb.Count() == 0)
                    return 0;

                DateTime minPBeginDate = bb.OrderBy(p => p.PlannedBeginDate).FirstOrDefault().PlannedBeginDate;

                aa = from a in WSDMaster.Details
                     where IsExistsInWsdls(WSDMaster,a) && a.PlannedBeginDate == minPBeginDate
                     select a;
            }
            if (aa == null || aa.Count() == 0)
                return 0;

            ArrayList listDatePlan = new ArrayList();

            foreach (var item in aa)
            {
                DateTime maxDate = item.PlannedEndDate;
                WeekScheduleDetail detail_maxdate = null;
                MaintainWorkDateInfoFollow(WSDMaster, item, ref maxDate, ref detail_maxdate, false);
                listDatePlan.Add(maxDate);
            }

            ArrayList listDateAct = new ArrayList();
            foreach (var item in aa)
            {

                SetActualEndDate(item);

                DateTime maxDate = item.Temp_ActualEndDate;
                WeekScheduleDetail detail_maxdate = null;
                MaintainWorkDateInfoFollow(WSDMaster, item, ref maxDate, ref detail_maxdate, true);
                listDateAct.Add(maxDate);
            }


            DateTime DatePlan = defaultTime;
            for (int i = 0; i < listDatePlan.Count; i++)
            {
                DatePlan = ClientUtil.ToDateTime(listDatePlan[i]) > DatePlan ? ClientUtil.ToDateTime(listDatePlan[i]) : DatePlan;
            }

            DateTime DateAct = defaultTime;
            for (int i = 0; i < listDateAct.Count; i++)
            {
                DateAct = ClientUtil.ToDateTime(listDateAct[i]) > DateAct ? ClientUtil.ToDateTime(listDateAct[i]) : DateAct;
            }

            return (DateAct - DatePlan).Days;

        }

        /// <summary>
        /// 根据当前节点的前置节点 维护当前节点工期信息
        /// </summary>
        /// <param name="detail">当前节点</param>
        private void MaintainWorkDateInfoByFront(WeekScheduleMaster CurWSDMaster, WeekScheduleDetail detail, bool isAct)
        {
            if (detail.RalationDetails == null || detail.RalationDetails.Count == 0)
                return;
            DateTime maxBegindate = defaultTime;

            foreach (var item in detail.RalationDetails)
            {
                //当前节点的前置节点
                WeekScheduleDetail wsd = CurWSDMaster.Details.ToList<WeekScheduleDetail>().Find(a => a.Id == item.FrontWeekScheduleDetail.Id);
                if (item.DelayRule == EnumDelayRule.FS)
                {
                    DateTime dtbegin_wsd = new DateTime(1900, 1, 1);
                    if (isAct)
                    {
                        if (wsd.Temp_ActualEndDate == null || wsd.Temp_ActualEndDate == defaultTime)
                            continue;
                        dtbegin_wsd = (DateTime)GetDateByDelay(wsd.Temp_ActualEndDate, item.DelayDays);
                        maxBegindate = maxBegindate < dtbegin_wsd ? dtbegin_wsd : maxBegindate;
                    }
                    else
                    {
                        if (wsd.PlannedEndDate == null || wsd.PlannedEndDate == defaultTime)
                            continue;
                        dtbegin_wsd = (DateTime)GetDateByDelay(wsd.PlannedEndDate, item.DelayDays);
                        maxBegindate = maxBegindate < dtbegin_wsd ? dtbegin_wsd : maxBegindate;
                    }
                }
                if (item.DelayRule == EnumDelayRule.SS)
                {
                    DateTime dtbegin_wsd = new DateTime(1900, 1, 1);
                    if (isAct)
                    {
                        if (wsd.Temp_ActualBeginDate == null || wsd.Temp_ActualBeginDate == defaultTime)
                            continue;
                        dtbegin_wsd = (DateTime)GetDateByDelay(wsd.Temp_ActualBeginDate, item.DelayDays);
                        maxBegindate = maxBegindate < dtbegin_wsd ? dtbegin_wsd : maxBegindate;
                    }
                    else
                    {
                        if (wsd.PlannedBeginDate == null || wsd.PlannedBeginDate == defaultTime)
                            continue;
                        dtbegin_wsd = (DateTime)GetDateByDelay(wsd.PlannedBeginDate, item.DelayDays);
                        maxBegindate = maxBegindate < dtbegin_wsd ? dtbegin_wsd : maxBegindate;
                    }
                }
                if (item.DelayRule == EnumDelayRule.FF)
                {
                    DateTime dtend_wsd = new DateTime(1900, 1, 1);
                    DateTime dtbegin_wsd = new DateTime(1900, 1, 1);
                    if (isAct)
                    {
                        if (wsd.Temp_ActualEndDate == null || wsd.Temp_ActualEndDate == defaultTime)
                            continue;
                        dtend_wsd = (DateTime)GetDateByDelay(wsd.Temp_ActualEndDate, item.DelayDays);
                        if (wsd.PlannedDuration == (decimal)0)
                            break;
                        dtbegin_wsd = (DateTime)GetBenginOrEndDate((DateTime?)dtend_wsd, (int?)wsd.PlannedDuration, DateRel.GetBeginDate);
                        maxBegindate = maxBegindate < dtbegin_wsd ? dtbegin_wsd : maxBegindate;
                    }
                    else
                    {
                        if (wsd.PlannedEndDate == null || wsd.PlannedEndDate == defaultTime)
                            continue;
                        dtend_wsd = (DateTime)GetDateByDelay(wsd.PlannedEndDate, item.DelayDays);
                        if (wsd.PlannedDuration == (decimal)0)
                            break;
                        dtbegin_wsd = (DateTime)GetBenginOrEndDate((DateTime?)dtend_wsd, (int?)wsd.PlannedDuration, DateRel.GetBeginDate);
                        maxBegindate = maxBegindate < dtbegin_wsd ? dtbegin_wsd : maxBegindate;
                    }
                }
            }
            if (maxBegindate != defaultTime)
            {
                if (isAct)
                {
                    detail.Temp_ActualBeginDate = maxBegindate;
                    if (detail.PlannedDuration != (decimal)0)
                        detail.Temp_ActualEndDate = (DateTime)GetBenginOrEndDate((DateTime?)maxBegindate, (int?)detail.PlannedDuration, DateRel.GetEndDate);
                }
                else
                {
                    detail.PlannedBeginDate = maxBegindate;
                    if (detail.PlannedDuration != (decimal)0)
                        detail.PlannedEndDate = (DateTime)GetBenginOrEndDate((DateTime?)maxBegindate, (int?)detail.PlannedDuration, DateRel.GetEndDate);
                }
            }

        }

        /// <summary>
        /// 根据当前节点 维护以当前节点为前置任务的节点的工期信息
        /// </summary>
        /// <param name="detail"></param>
        private void MaintainWorkDateInfoFollow(WeekScheduleMaster WSDMaster, WeekScheduleDetail detail, ref DateTime maxDate, ref WeekScheduleDetail detail_maxdate, bool isAct)
        {
            foreach (var item in WSDMaster.Details)
            {
                if (item.RalationDetails == null || item.RalationDetails.Count == 0)
                    continue;
                //以当前节点作为前置节点
                WeekScheduleRalation wsdr_frind = item.RalationDetails.ToList<WeekScheduleRalation>().Find(a => a.FrontWeekScheduleDetail.Id == detail.Id);
                if (wsdr_frind != null)
                {
                    WeekScheduleDetail wsd = wsdr_frind.Master;

                    MaintainWorkDateInfoByFront(WSDMaster, wsd, isAct);
                    if (isAct)
                    {
                        if (wsd.Temp_ActualEndDate > maxDate)
                        {
                            maxDate = wsd.Temp_ActualEndDate;
                            detail_maxdate = wsd;
                        }

                    }
                    else
                    {
                        if (wsd.PlannedEndDate > maxDate)
                        {
                            maxDate = wsd.PlannedEndDate;
                            detail_maxdate = wsd;
                        }
                    }

                    MaintainWorkDateInfoFollow(WSDMaster, wsd, ref maxDate, ref detail_maxdate, isAct);
                }

            }
        }

        private void SetActualEndDate(WeekScheduleDetail detail)
        {
            if (detail.ActualBeginDate != null && detail.ActualBeginDate != defaultTime && detail.TaskCompletedPercent < (decimal)100)
            {
                if (detail.TaskCompletedPercent == (decimal)0)
                    detail.Temp_ActualEndDate = detail.ActualBeginDate.AddDays(ClientUtil.Tofloat((DateTime.Now - detail.ActualBeginDate).Days + 1 + detail.PlannedDuration));
                else
                    detail.Temp_ActualEndDate = detail.ActualBeginDate.AddDays(ClientUtil.Tofloat(((detail.ActualEndDate - detail.ActualBeginDate).Days + 1) * 100 / detail.TaskCompletedPercent));
            }
            else
            {
                detail.Temp_ActualEndDate = DateTime.Now;
                detail.Temp_ActualEndDate = DateTime.Now.AddDays((float)detail.PlannedDuration);
            }
        }

        private DateTime? GetDateByDelay(DateTime? date1, int delaydays)
        {
            if (date1 != null && date1 != defaultTime)
                return ((DateTime)date1).AddDays(delaydays + 1);
            return null;
        }

        private DateTime? GetBenginOrEndDate(DateTime? dt_date1, int? i_workdays, DateRel dr)
        {
            DateTime? date_Rtn = null;
            if (i_workdays == null)
                return null;
            switch (dr)
            {
                case DateRel.GetBeginDate:
                    if (dt_date1 == null || dt_date1 == defaultTime || i_workdays == null)
                        break;
                    date_Rtn = ((DateTime)dt_date1).AddDays(1 - (int)i_workdays);
                    break;
                case DateRel.GetEndDate:
                    if (dt_date1 == null || dt_date1 == defaultTime || i_workdays == null)
                        break;
                    date_Rtn = ((DateTime)dt_date1).AddDays((int)i_workdays - 1);
                    break;
            }
            return date_Rtn;
        }

        /// <summary>
        /// 访问数据库，返回DataTable
        /// </summary>
        /// <param name="sql">查询的sql语句</param>
        /// <returns>数据集</returns>
        private DataTable ExecuteTable(string sql)
        {
            IDbConnection conn = DbConfig.Connection;
            IDbCommand command = conn.CreateCommand();
            command.CommandText = sql;
            DbConfig.Transaction.Enlist(command);
            using (IDataReader dataReader = command.ExecuteReader())
            {
                DataTable dt = new DataTable();
                dt.Load(dataReader);
                return dt;
            }
        }

    }
    enum DateRel
    {
        [Description("获取开始时间")]
        GetBeginDate = 1,
        [Description("获取结束时间")]
        GetEndDate = 2,
        [Description("获取工期")]
        GetWorkDays = 3
    }
}
