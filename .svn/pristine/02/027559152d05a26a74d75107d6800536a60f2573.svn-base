using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.MonthlyPlanMng
{
    //public enum CMonthlyPlanMng_ExecType
    //{
    //     <summary>
    //     月度需求计划查询
    //     </summary>
    //    MonthlyPlanMngQuery,
    //     <summary>
    //     计划单引用
    //     </summary>
    //    MonthlyPlanRef
    //}
    
    public class CMonthlyPlanMng
    {
        private static IFramework framework = null;
        string mainViewName = "月度需求计划";
        private static VMonthlyPlanSearchList searchList;

        public CMonthlyPlanMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VMonthlyPlanSearchList(this);
        }

        public void Start(EnumMonthlyType execType)
        {
            Find("空",execType,"空");
        }

        public void Find(string name, EnumMonthlyType execType,string Id)
        {
            if (execType == EnumMonthlyType.土建)
            {
                mainViewName = "月度需求计划单(土建)";
            }
            else if (execType == EnumMonthlyType.安装)
            {
                mainViewName = "月度需求计划单(安装)";
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

            VMonthlyPlanMng vMainView = framework.GetMainView(mainViewName + "-空") as VMonthlyPlanMng;

            if (vMainView == null)
            {
                vMainView = new VMonthlyPlanMng();
                vMainView.MonthlyType = execType;
                vMainView.ViewName = mainViewName;

                //载入查询视图
                vMainView.ViewDeletedEvent += new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(vSaleBudget_ViewDeletedEvent);
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VMonthlyPlanSearchCon searchCon = new VMonthlyPlanSearchCon(searchList, execType);

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

            VMonthlyPlanMng vDmand = mv as VMonthlyPlanMng;
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
                if (obj != null && typeof(EnumMonthlyType) == obj.GetType())
                {
                    EnumMonthlyType excuteType = (EnumMonthlyType)obj;
                    switch (excuteType)
                    {
                        case EnumMonthlyType.土建:
                        case EnumMonthlyType.安装:
                            Start(excuteType);
                            break;
                        case EnumMonthlyType.monthlySearch:
                            IMainView mroqMv = framework.GetMainView("月度需求计划查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VMonthlyPlanMngQuery vmroq = new VMonthlyPlanMngQuery();
                            vmroq.TheAuthMenu = theMenu;
                            vmroq.ViewCaption = "月度需求计划查询";
                            framework.AddMainView(vmroq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
