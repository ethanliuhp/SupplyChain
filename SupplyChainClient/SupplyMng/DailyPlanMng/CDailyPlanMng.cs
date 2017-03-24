using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.DailyPlanMng
{
    //public enum CDailyPlanMng_ExecType
    //{
    //    /// <summary>
    //    /// 日常需求计划查询
    //    /// </summary>
    //    DailyPlanQuery,
    //    /// <summary>
    //    /// 日常需求计划引用
    //    /// </summary>
    //    DailyPlanRef
    //}
    
    public class CDailyPlanMng
    {
        private static IFramework framework = null;
        string mainViewName = "日常需求计划单";
        private static VDailyPlanSearchList searchList;

        public CDailyPlanMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VDailyPlanSearchList(this);
        }

        public void Start(EnumDailyType execType)
        {
            Find("空",execType,"空");
        }

        public void Find(string name, EnumDailyType execType,string Id)
        {
            if (execType == EnumDailyType.土建)
            {
                mainViewName = "日常需求计划单(土建)";
            }
            else if (execType == EnumDailyType.安装)
            {
                mainViewName = "日常需求计划单(安装)";
            }
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

            VDailyPlanMng vMainView = framework.GetMainView(mainViewName + "-空") as VDailyPlanMng;

            if (vMainView == null)
            {
                vMainView = new VDailyPlanMng();
                vMainView.DailyType = execType;
                vMainView.ViewName = mainViewName;

                //载入查询视图
                vMainView.ViewDeletedEvent += new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(vSaleBudget_ViewDeletedEvent);
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VDailyPlanSearchCon searchCon = new VDailyPlanSearchCon(searchList,execType);

                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(Id);

            vMainView.ViewShow();
        }

        void vSaleBudget_ViewDeletedEvent(object sender)
        {
            IMainView mv = sender as IMainView;

            VDailyPlanMng vDmand = mv as VDailyPlanMng;
            if (vDmand != null)
                searchList.RemoveRow(vDmand.CurBillMaster.Id);


            IList lst = framework.GetMainViews(mv.ViewName);

            if (lst.Count > 1)
                framework.CloseMainView(mv);
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                //Start();
            } else
            {
                object obj=args[0];
                AuthManagerLib.AuthMng.MenusMng.Domain.Menus theMenu = null;
                if (args.Length > 1)
                {
                    theMenu = args[1] as AuthManagerLib.AuthMng.MenusMng.Domain.Menus;
                }
                if (obj != null && typeof(EnumDailyType) == obj.GetType())
                {
                    EnumDailyType excuteType = (EnumDailyType)obj;
                    switch (excuteType)
                    {
                        case EnumDailyType.土建:
                        case EnumDailyType.安装:
                            Start(excuteType);
                            break;
                        case EnumDailyType.dailySearch:
                            IMainView mroqMv = framework.GetMainView("日常需求计划查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VDailyPlanQuery vmroq = new VDailyPlanQuery();
                            vmroq.TheAuthMenu = theMenu;
                            vmroq.ViewCaption = "日常需求计划查询";
                            framework.AddMainView(vmroq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
