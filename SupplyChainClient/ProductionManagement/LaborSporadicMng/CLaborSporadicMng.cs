using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng
{
    public enum CLaborSporadicMng_ExecType
    {
        /// <summary>
        /// 零星用工查询
        /// </summary>
        LaborQuery,
        LaborSporadicQuery,
        LaborSporadicSelect,
        LaborSporadicSHCQuery,
        LaborSporadicSelector,
        SubPackageVisaSelect,
        TimeDespatching
    }

    public class CLaborSporadicMng
    {
        private static IFramework framework = null;
        string mainViewName = "零星用工单维护";
        private static VLaborSporadicSearchList searchList;

        public CLaborSporadicMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VLaborSporadicSearchList(this);
        }

        public void Start(EnumLaborType Type)
        {
            Find("空", Type, "空");
        }

        public void Find(string name, EnumLaborType Type, string Id)
        {
            if (Type == EnumLaborType.代工)
            {
                mainViewName = "代工单维护";
            }
            if (Type == EnumLaborType.派工)
            {
                mainViewName = "零星用工单维护";
            }
            if (Type == EnumLaborType.分包签证)
            {
                mainViewName = "分包签证单维护";
            }
            if (Type == EnumLaborType.计时派工)
            {
                mainViewName = "计时派工单维护";
            }
            //if (Type == EnumLaborType.代工核算)
            //{
            //    mainViewName = "代工核算单";
            //}
            //if (Type == EnumLaborType.派工核算)
            //{
            //    mainViewName = "派工核算单";
            //}
            if (Type == EnumLaborType.逐日派工)
            {
                mainViewName = "逐日派工单维护";
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
            if (Type == EnumLaborType.代工)
            {
                VLaborSporadicMng vMainView = framework.GetMainView(mainViewName + "-空") as VLaborSporadicMng;

                if (vMainView == null)
                {
                    vMainView = new VLaborSporadicMng();
                    vMainView.ViewName = mainViewName;
                    //载入查询视图
                    //分配辅助视图
                    vMainView.AssistViews.Add(searchList);
                    VLaborSporadicSearchCon searchCon = new VLaborSporadicSearchCon(searchList, Type);
                    vMainView.AssistViews.Add(searchCon);
                    //载入框架
                    framework.AddMainView(vMainView);
                }
                vMainView.ViewCaption = captionName;
                vMainView.ViewName = mainViewName;
                vMainView.Start(Id);

                vMainView.ViewShow();
            }
            if (Type == EnumLaborType.逐日派工)
            {
                VLaborSporadicPlan vMainView = framework.GetMainView(mainViewName + "-空") as VLaborSporadicPlan;

                if (vMainView == null)
                {
                    vMainView = new VLaborSporadicPlan();
                    vMainView.ViewName = mainViewName;
                    //载入查询视图
                    //分配辅助视图
                    VLaborSporadicPlanSearchList planSearchList = new VLaborSporadicPlanSearchList(this);
                    vMainView.AssistViews.Add(planSearchList);
                    VLaborSporadicPlanSearchCon searchCon = new VLaborSporadicPlanSearchCon(planSearchList, Type);
                    vMainView.AssistViews.Add(searchCon);
                    //载入框架
                    framework.AddMainView(vMainView);
                }
                vMainView.ViewCaption = captionName;
                vMainView.ViewName = mainViewName;
                vMainView.Start(Id);

                vMainView.ViewShow();
            }
            if (Type == EnumLaborType.派工)
            {
                VLaborSporadic vMainView = framework.GetMainView(mainViewName + "-空") as VLaborSporadic;
                if (vMainView == null)
                {
                    vMainView = new VLaborSporadic();
                    vMainView.ViewName = mainViewName;
                    //载入查询视图
                    //分配辅助视图
                    vMainView.AssistViews.Add(searchList);
                    VLaborSporadicSearchCon searchCon = new VLaborSporadicSearchCon(searchList, Type);
                    vMainView.AssistViews.Add(searchCon);
                    //载入框架
                    framework.AddMainView(vMainView);
                }
                vMainView.ViewCaption = captionName;
                vMainView.ViewName = mainViewName;
                vMainView.Start(Id);
                vMainView.ViewShow();
            }
            if (Type == EnumLaborType.分包签证)
            {
                VSubPackageVisa vMainView = framework.GetMainView(mainViewName + "-空") as VSubPackageVisa;
                if (vMainView == null)
                {
                    vMainView = new VSubPackageVisa();
                    vMainView.ViewName = mainViewName;
                    //载入查询视图
                    //分配辅助视图
                    vMainView.AssistViews.Add(searchList);
                    VLaborSporadicSearchCon searchCon = new VLaborSporadicSearchCon(searchList, Type);
                    vMainView.AssistViews.Add(searchCon);
                    //载入框架
                    framework.AddMainView(vMainView);
                }
                vMainView.ViewCaption = captionName;
                vMainView.ViewName = mainViewName;
                vMainView.Start(Id);
                vMainView.ViewShow();
            }
            if (Type == EnumLaborType.计时派工)
            {
                VTimeDispatching vMainView = framework.GetMainView(mainViewName + "-空") as VTimeDispatching;
                if (vMainView == null)
                {
                    vMainView = new VTimeDispatching() { ViewName = mainViewName };
                    //分配辅助视图
                    vMainView.AssistViews.Add(searchList);
                    VLaborSporadicSearchCon searchCon = new VLaborSporadicSearchCon(searchList, Type);
                    vMainView.AssistViews.Add(searchCon);
                    //载入框架
                    framework.AddMainView(vMainView);
                }
                vMainView.ViewCaption = captionName;
                vMainView.ViewName = mainViewName;
                vMainView.Start(Id);
                vMainView.ViewShow();
            }
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                //Start();
            }
            else
            {
                object obj = args[0];
                if (obj != null && obj.GetType() == typeof(CLaborSporadicMng_ExecType))
                {
                    CLaborSporadicMng_ExecType execType = (CLaborSporadicMng_ExecType)obj;
                    switch (execType)
                    {
                        case CLaborSporadicMng_ExecType.LaborSporadicQuery:
                            IMainView mroqMv = framework.GetMainView("零星用工单商务查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            string strName = "零星用工单商务查询";
                            VLaborSporadicQueryByBill vmroq = new VLaborSporadicQueryByBill(strName);
                            vmroq.ViewCaption = "零星用工单商务查询";
                            framework.AddMainView(vmroq);
                            return null;
                        case CLaborSporadicMng_ExecType.LaborSporadicSHCQuery:
                            IMainView mroqMva = framework.GetMainView("零星用工单生产查询");
                            if (mroqMva != null)
                            {
                                mroqMva.ViewShow();
                                return null;
                            }
                            string strNamea = "零星用工单生产查询";
                            VLaborSporadicQueryByBill vmroqa = new VLaborSporadicQueryByBill(strNamea);
                            vmroqa.ViewCaption = "零星用工单生产查询";
                            framework.AddMainView(vmroqa);
                            return null;
                        case CLaborSporadicMng_ExecType.LaborQuery:
                            IMainView mroqMv11 = framework.GetMainView("代工单查询");
                            if (mroqMv11 != null)
                            {
                                mroqMv11.ViewShow();
                                return null;
                            }
                            VLaborQuery vmroq11 = new VLaborQuery();
                            vmroq11.ViewCaption = "代工单查询";
                            framework.AddMainView(vmroq11);
                            return null;
                        case CLaborSporadicMng_ExecType.LaborSporadicSelect:
                            IMainView mroqMv1 = framework.GetMainView("零星用工单核算");
                            if (mroqMv1 != null)
                            {
                                mroqMv1.ViewShow();
                                return null;
                            }
                            VLaborSporadicSelect vmroq1 = new VLaborSporadicSelect();
                            vmroq1.ViewCaption = "零星用工单核算";
                            framework.AddMainView(vmroq1);
                            return null;
                        case CLaborSporadicMng_ExecType.SubPackageVisaSelect:
                            IMainView temp1 = framework.GetMainView("分包签证单审核");
                            if (temp1 != null)
                            {
                                temp1.ViewShow();
                                return null;
                            }
                            framework.AddMainView(new VSubPackageVisaSelect() { ViewCaption = "分包签证单审核" });
                            return null;
                        case CLaborSporadicMng_ExecType.TimeDespatching:
                            IMainView temp2 = framework.GetMainView("计时派工单核算");
                            if (temp2 != null)
                            {
                                temp2.ViewShow();
                                return null;
                            }
                            framework.AddMainView(new VTimeDispatchingSelect() { ViewCaption = "计时派工单核算" });
                            return null;
                        case CLaborSporadicMng_ExecType.LaborSporadicSelector:
                            IMainView mroqMv2 = framework.GetMainView("代工单核算");
                            if (mroqMv2 != null)
                            {
                                mroqMv2.ViewShow();
                                return null;
                            }
                            VLaborSporadicSelector vmroq2 = new VLaborSporadicSelector();
                            vmroq2.ViewCaption = "代工单核算";
                            framework.AddMainView(vmroq2);
                            return null;

                    }
                }
                else if (obj != null && obj.GetType() == typeof(EnumLaborType))
                {
                    EnumLaborType laborType = (EnumLaborType)obj;
                    switch (laborType)
                    {
                        case EnumLaborType.代工:
                        case EnumLaborType.派工:
                        case EnumLaborType.分包签证:
                        case EnumLaborType.计时派工:
                        case EnumLaborType.逐日派工:
                            //case EnumLaborType.代工核算:
                            //case EnumLaborType.派工核算:
                            Start(laborType);
                            break;
                    }
                }
            }
            return null;
        }
    }
}
