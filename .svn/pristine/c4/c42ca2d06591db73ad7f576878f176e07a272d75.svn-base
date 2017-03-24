using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI
{
    public class CSchedule
    {
        private static IFramework framework;
        string mainViewName = "总进度计划";

        public CSchedule(IFramework fm)
        {
            if (framework == null) framework = fm;
        }

        public void Start()
        {
            Find("空");
        }

        public void Find(string name)
        {
            string captionName = mainViewName;
            string str = name;
            if (name is string)
            {
                if (mainViewName == "总进度计划" && str == "空")
                {
                   Application.Business.Erp.SupplyChain.Basic.Domain.CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                   str = projectInfo.Name;
                }
                captionName = this.mainViewName + "-" + str;
            }

            IMainView mv = framework.GetMainView(captionName);

            if (mv != null)
            {
                //如果当前视图已经存在，直接显示
                mv.ViewShow();
                mv.ViewName = mainViewName;
                mv.ViewCaption = captionName;
                return;
            }

            //VSchedule vMainView = framework.GetMainView(mainViewName + "-空") as VSchedule;
            VSchedule vMainView = framework.GetMainView(mainViewName + "-" + str) as VSchedule;

            if (vMainView == null)
            {
                vMainView = new VSchedule(EnumScheduleType.总进度计划);
                vMainView.ViewName = mainViewName;

                vMainView.ViewCaption = captionName;
                //载入框架
                framework.AddMainView(vMainView);
            }

            
            vMainView.ViewName = mainViewName;
            vMainView.ViewCaption = captionName;
            vMainView.ViewShow();
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                Start();
            }
            else
            {
                object o = args[0];

                AuthManagerLib.AuthMng.MenusMng.Domain.Menus theMenu = null;
                if (args.Length > 1)
                {
                    theMenu = args[1] as AuthManagerLib.AuthMng.MenusMng.Domain.Menus;
                }

                if (o != null && o.GetType() == typeof(EnumScheduleType))
                {
                    EnumScheduleType excuteType = (EnumScheduleType)o;
                    switch (excuteType)
                    {
                        case EnumScheduleType.总进度计划:
                            IMainView scheduleMV = framework.GetMainView("总进度计划");
                            if (scheduleMV != null)
                            {
                                scheduleMV.ViewShow();
                                return null;
                            }
                            VSchedule vs = new VSchedule(excuteType);
                            vs.ViewCaption = "总进度计划";
                            vs.ViewName = "总进度计划";
                            framework.AddMainView(vs);
                            return null;
                        case EnumScheduleType.总滚动进度计划:
                            IMainView mv2 = framework.GetMainView("总滚动进度计划");
                            if (mv2 != null)
                            {
                                mv2.ViewShow();
                                return null;
                            }
                            VScrollSchedulePlan vs2 = new VScrollSchedulePlan(excuteType);
                            vs2.ViewCaption = "总滚动进度计划";
                            vs2.ViewName = "总滚动进度计划";
                            framework.AddMainView(vs2);
                            return null;
                        case EnumScheduleType.进度计划查询:
                            IMainView mv3 = framework.GetMainView("进度计划查询");
                            if (mv3 != null)
                            {
                                mv3.ViewShow();
                                return null;
                            }
                            VScheduleQuery vsq = new VScheduleQuery();
                            vsq.TheAuthMenu = theMenu;
                            vsq.ViewCaption = "进度计划查询";
                            framework.AddMainView(vsq);
                            return null;
                    }
                }
                else if (o.GetType() == typeof(string))
                {
                    Find(o.ToString());
                }
                else if(o.GetType() == typeof(EnumProductionMngExecType))
                {
                    EnumProductionMngExecType execType = (EnumProductionMngExecType) o;
                    switch (execType)
                    {
                        case EnumProductionMngExecType.工期预警:
                            IMainView dwView = framework.GetMainView("工期预警");
                            if (dwView != null)
                            {
                                dwView.ViewShow();
                                return null;
                            }
                            VDurationWarning vsq = new VDurationWarning();
                            vsq.ViewCaption = "工期预警";
                            framework.AddMainView(vsq);
                            return null;
                            break;
                        case EnumProductionMngExecType.劳动力预测统计:
                            IMainView dwView1 = framework.GetMainView("劳动力预测统计");
                            if (dwView1 != null)
                            {
                                dwView1.ViewShow();
                                return null;
                            }
                            VCostWorkForceRpt vsq1 = new VCostWorkForceRpt();
                            vsq1.ViewCaption = "劳动力预测统计";
                            framework.AddMainView(vsq1);
                            return null;
                            break;

                    }
                }
            }

            return null;
        }
    }
}
