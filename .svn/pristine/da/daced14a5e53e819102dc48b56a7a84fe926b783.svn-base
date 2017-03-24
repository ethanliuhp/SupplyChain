using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI
{
    public class CWeekSchedule
    {
        private static IFramework framework = null;
        string mainViewName = "执行进度计划";
        private static VWeekScheduleSearchList searchList;

        public CWeekSchedule(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VWeekScheduleSearchList(this);
        }

        public void Start(EnumExecScheduleType execScheduleType)
        {
            Find("空", execScheduleType);
        }

        public void Find(string name, EnumExecScheduleType execScheduleType)
        {
            if (execScheduleType == EnumExecScheduleType.周进度计划)
            {
                mainViewName = "工区周计划维护";
            }
            else
            {
                mainViewName = "进度计划编制";

            }
            //else if (execScheduleType == EnumExecScheduleType.季计划)
            //{
            //    mainViewName = "进度计划编制";
            //}
            //else if (execScheduleType == EnumExecScheduleType.月计划)
            //{
            //    mainViewName = "进度计划编制";
            //}

            string captionName = mainViewName;

            if (name is string)
            {
                captionName = this.mainViewName + "-" + name;
            }

            IMainView mv = framework.GetMainView(captionName);

            if (mv != null)
            {
                //如果当前视图已经存在，直接显示
                mv.ViewShow();
                return;
            }

            if (execScheduleType == EnumExecScheduleType.周进度计划)
            {
                VWeekScheduleBak2 vMainViewbak2 = framework.GetMainView(mainViewName + "-空") as VWeekScheduleBak2;

                if (vMainViewbak2 == null)
                {

                    vMainViewbak2 = new VWeekScheduleBak2(execScheduleType);
                    vMainViewbak2.ViewName = mainViewName;

                    //载入查询视图
                    //分配辅助视图
                    vMainViewbak2.AssistViews.Add(searchList);
                    VWeekScheduleSearchCon searchCon = new VWeekScheduleSearchCon(searchList, execScheduleType);

                    vMainViewbak2.AssistViews.Add(searchCon);

                    //载入框架
                    framework.AddMainView(vMainViewbak2);
                }

                vMainViewbak2.ViewCaption = captionName;
                vMainViewbak2.ViewName = mainViewName;
                vMainViewbak2.Start(name);
                vMainViewbak2.ViewShow();
            }
            else
            {
                VWeekSchedule vMainView = framework.GetMainView(mainViewName + "-空") as VWeekSchedule;

                if (vMainView == null)
                {

                    vMainView = new VWeekSchedule(execScheduleType);
                    vMainView.ViewName = mainViewName;

                    //载入查询视图
                    //分配辅助视图
                    vMainView.AssistViews.Add(searchList);
                    VWeekScheduleSearchCon searchCon = new VWeekScheduleSearchCon(searchList, execScheduleType);

                    vMainView.AssistViews.Add(searchCon);

                    //载入框架
                    framework.AddMainView(vMainView);
                }

                vMainView.ViewCaption = captionName;
                vMainView.ViewName = mainViewName;
                vMainView.Start(name);
                vMainView.ViewShow();
            }

            
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
            }
            else
            {
                object obj = args[0];
                if (obj != null)
                {
                    if (obj is EnumExecScheduleType)
                    {
                        EnumExecScheduleType execScheduleType = (EnumExecScheduleType)obj;
                        Start(execScheduleType);
                        return null;
                    }
                    else if (obj is EnumProductionMngExecType)
                    {
                        EnumProductionMngExecType execType = (EnumProductionMngExecType)obj;
                        switch (execType)
                        {
                            case EnumProductionMngExecType.周计划查询:
                                IMainView ws = framework.GetMainView("周进度计划查询");
                                if (ws != null)
                                {
                                    ws.ViewShow();
                                    return null;
                                }
                                VWeekScheduleQuery vwsq = new VWeekScheduleQuery(EnumExecScheduleType.周进度计划);
                                vwsq.ViewCaption = "周进度计划查询";
                                framework.AddMainView(vwsq);
                                return null;
                            case EnumProductionMngExecType.月计划查询:
                                {
                                    IMainView mv1 = framework.GetMainView("月计划查询");
                                    if (mv1 != null)
                                    {
                                        mv1.ViewShow();
                                        return null;
                                    }
                                    VWeekScheduleQuery mv2 = new VWeekScheduleQuery(EnumExecScheduleType.月度进度计划);
                                    mv2.ViewCaption = "月计划查询";
                                    framework.AddMainView(mv2);
                                    return null;
                                }
                            case EnumProductionMngExecType.项目周计划维护:
                                {
                                    IMainView mv1 = framework.GetMainView("项目周计划维护");
                                    if (mv1 != null)
                                    {
                                        mv1.ViewShow();
                                        return null;
                                    }
                                    VWeekScheduleEdit mv2 = new VWeekScheduleEdit();
                                    mv2.ViewCaption = "项目周计划维护";
                                    framework.AddMainView(mv2);
                                    return null;
                                }
                            case EnumProductionMngExecType.周计划确认:
                                IMainView mv3 = framework.GetMainView("周进度计划确认");
                                if (mv3 != null)
                                {
                                    mv3.ViewShow();
                                    return null;
                                }
                                VWeekScheduleConfirm vwsc = new VWeekScheduleConfirm();
                                vwsc.ViewCaption = "周进度计划确认";
                                framework.AddMainView(vwsc);
                                return null;
                            case EnumProductionMngExecType.总进度计划审批:
                                IMainView sa = framework.GetMainView("总进度计划审批");
                                if (sa != null)
                                {
                                    sa.ViewShow();
                                    return null;
                                }
                                VScheduleApprove scheduleApprove = new VScheduleApprove();
                                scheduleApprove.ViewCaption = "总进度计划审批";
                                framework.AddMainView(scheduleApprove);
                                return null;
                            case EnumProductionMngExecType.任务单维护:
                                {
                                    IMainView mv1 = framework.GetMainView("任务单维护");
                                    if (mv1 != null)
                                    {
                                        mv1.ViewShow();
                                        return null;
                                    }
                                    VWeekAssign mv2 = new VWeekAssign();
                                    mv2.ViewCaption = "任务单维护";
                                    mv2.ViewName = "任务单维护";
                                    mv2.RegisteViewToSubmit();
                                    framework.AddMainView(mv2);
                                    return null;
                                }
                            case EnumProductionMngExecType.任务单查询:
                                {
                                    IMainView mv1 = framework.GetMainView("任务单查询");
                                    if (mv1 != null)
                                    {
                                        mv1.ViewShow();
                                        return null;
                                    }
                                    VWeekAssignQuery mv2 = new VWeekAssignQuery();
                                    mv2.ViewCaption = "任务单查询";
                                    mv2.ViewName = "任务单查询";
                                    framework.AddMainView(mv2);
                                    return null;
                                }
                        }
                    }
                }
            }
            return null;
        }

        #region 周进度计划汇总
        public void Start()
        {
            Find("空");
        }

        public void Find(string name)
        {
            string mainViewName = "项目周计划维护";

            string captionName = mainViewName;

            if (name is string)
            {
                captionName = mainViewName + "-" + name;
            }

            IMainView mv = framework.GetMainView(captionName);

            if (mv != null)
            {
                //如果当前视图已经存在，直接显示
                mv.ViewShow();
                return;
            }

            VWeekScheduleSummary vMainView = framework.GetMainView(mainViewName + "-空") as VWeekScheduleSummary;

            if (vMainView == null)
            {
                vMainView = new VWeekScheduleSummary();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VWeekScheduleSearchCon searchCon = new VWeekScheduleSearchCon(searchList, EnumExecScheduleType.周进度计划);

                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(name);

            vMainView.ViewShow();
        }
        #endregion
    }
}
