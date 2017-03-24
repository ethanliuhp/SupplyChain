using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.DemandMasterPlanMng
{
    //public enum CDemandMasterPlanMng_ExecType
    //{
    //    /// <summary>
    //    /// 需求总计划查询
    //    /// </summary>
    //    DemandMasterPlanQuery,
    //    /// <summary>
    //    /// 需求总计划引用
    //    /// </summary>
    //    DemandMasterPlanRef
    //}
    
    public class CDemandMasterPlanMng
    {
        private static IFramework framework = null;
        string mainViewName = "需求总计划单";
        private static VDemandMasterPlanSearchList searchList;

        public CDemandMasterPlanMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VDemandMasterPlanSearchList(this);
        }

        public void Start(EnumDemandType execType)
        {
            Find("空",execType,"空");
        }

        public void Find(string name, EnumDemandType execType,string Id)
        {
            if (execType == EnumDemandType.土建)
            {
                mainViewName = "需求总计划单(土建)";
            }
            else if (execType == EnumDemandType.安装)
            {
                mainViewName = "需求总计划单(安装)";
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

            VDemandMasterPlanMng vMainView = framework.GetMainView(mainViewName + "-空") as VDemandMasterPlanMng;

            if (vMainView == null)
            {
                vMainView = new VDemandMasterPlanMng();
                vMainView.DemandType = execType;
                vMainView.ViewName = mainViewName;

                //载入查询视图
                vMainView.ViewDeletedEvent += new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(vSaleBudget_ViewDeletedEvent);
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VDemandMasterPlanSearchCon searchCon = new VDemandMasterPlanSearchCon(searchList, execType);

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

            VDemandMasterPlanMng vDmand = mv as VDemandMasterPlanMng;
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
                object obj = args[0];
                AuthManagerLib.AuthMng.MenusMng.Domain.Menus theMenu = null;
                if (args.Length > 1)
                {
                    theMenu = args[1] as AuthManagerLib.AuthMng.MenusMng.Domain.Menus;
                }
                if (obj != null && typeof(EnumDemandType) == obj.GetType())
                {
                    EnumDemandType excuteType = (EnumDemandType)obj;
                    switch (excuteType)
                    {
                        case EnumDemandType.土建:
                        case EnumDemandType.安装:
                            Start(excuteType);
                            break;
                        case EnumDemandType.demandSearch:
                            IMainView mroqMv = framework.GetMainView("需求总计划查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VDemandMasterPlanQuery vmroq = new VDemandMasterPlanQuery();
                            vmroq.TheAuthMenu = theMenu;
                            vmroq.ViewCaption = "需求总计划查询";
                            framework.AddMainView(vmroq);
                            return null;
                        case EnumDemandType.companyDemandSearch:
                            IMainView mroqMv1 = framework.GetMainView("需求总计划查询(公司)");
                            if (mroqMv1 != null)
                            {
                                mroqMv1.ViewShow();
                                return null;
                            }
                            VCompanyDemandMasterPlanQuery vmroq1 = new VCompanyDemandMasterPlanQuery();                        
                            vmroq1.ViewCaption = "需求总计划查询(公司)";
                            framework.AddMainView(vmroq1);
                            return null;
                        case EnumDemandType.companySupplyPlan:
                            IMainView mv2 = framework.GetMainView("采购成本统计表");
                            if (mv2 != null)
                            {
                                mv2.ViewShow();
                                return null;
                            }
                            VCompanySupplyQuery vSupplyQuery = new VCompanySupplyQuery();
                            vSupplyQuery.ViewCaption = "采购成本统计表";
                            framework.AddMainView(vSupplyQuery);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
