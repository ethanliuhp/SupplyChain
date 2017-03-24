using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement
{
    public enum CPenaltyDeductionMng_ExecType
    {
        /// <summary>
        /// 罚扣款单查询
        /// </summary>
        PenaltyQuery,
        PenaltyDeductionQuery,
        PenaltyDeductionSelect,
        SceneManageFeelReport,   //现场管理费分析对比表
        MechanicalCostComparisonRpt,//机械费成本分析对比表
        TempDedit           // 暂扣款


    }

    public class CPenaltyDeductionMng
    {
        private static IFramework framework = null;
        string mainViewName = "罚款单维护";
        private static VPenaltyDeductionSearchList searchList;

        public CPenaltyDeductionMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VPenaltyDeductionSearchList(this);
        }

        public void Start()
        {
            Find("空", "空");
        }

        public void Find(string name, string Id)
        {
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

            VPenaltyDeductionMng vMainView = framework.GetMainView(mainViewName + "-空") as VPenaltyDeductionMng;

            if (vMainView == null)
            {
                vMainView = new VPenaltyDeductionMng();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VPenaltyDeductionSearchCon searchCon = new VPenaltyDeductionSearchCon(searchList);

                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(Id);

            vMainView.ViewShow();
        }

        public void Find(string name, string id, CPenaltyDeductionMng_ExecType type)
        {
            switch (type)
            {
                case CPenaltyDeductionMng_ExecType.TempDedit:
                    TempDeditView(name, id);
                    break;
                default:
                    break;
            }
        }


        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                Start();
            }
            else
            {
                object obj = args[0];
                if (obj != null && obj.GetType() == typeof(CPenaltyDeductionMng_ExecType))
                {
                    CPenaltyDeductionMng_ExecType execType = (CPenaltyDeductionMng_ExecType)obj;
                    switch (execType)
                    {
                        case CPenaltyDeductionMng_ExecType.PenaltyDeductionQuery:
                            IMainView mroqMv = framework.GetMainView("罚款单查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VPenaltyDeductionQuery vmroq = new VPenaltyDeductionQuery();
                            vmroq.ViewCaption = "罚款单查询";
                            framework.AddMainView(vmroq);
                            return null;
                        case CPenaltyDeductionMng_ExecType.PenaltyQuery:
                            IMainView mroqMv11 = framework.GetMainView("扣款单查询");
                            if (mroqMv11 != null)
                            {
                                mroqMv11.ViewShow();
                                return null;
                            }
                            VPenaltyQuery vmroq11 = new VPenaltyQuery();
                            vmroq11.ViewCaption = "扣款单查询";
                            framework.AddMainView(vmroq11);
                            return null;
                        case CPenaltyDeductionMng_ExecType.PenaltyDeductionSelect:
                            IMainView mroqMv2 = framework.GetMainView("罚款核算单");
                            if (mroqMv2 != null)
                            {
                                mroqMv2.ViewShow();
                                return null;
                            }
                            VPenaltyDeductionSelect vmroq2 = new VPenaltyDeductionSelect();
                            vmroq2.ViewCaption = "罚款核算单";
                            framework.AddMainView(vmroq2);
                            return null;


                        #region  20160819 HJ
                        case CPenaltyDeductionMng_ExecType.SceneManageFeelReport:
                            IMainView mroqMv3 = framework.GetMainView("现场管理费分析对比表");
                            if (mroqMv3 != null)
                            {
                                mroqMv3.ViewShow();
                                return null;
                            }
                            VSceneManagementReport vmroq3 = new VSceneManagementReport();
                            vmroq3.ViewCaption = "现场管理费分析对比表";
                            framework.AddMainView(vmroq3);
                            return null;
                        #endregion


                        #region 机械费用对比表
                        case CPenaltyDeductionMng_ExecType.MechanicalCostComparisonRpt:
                            IMainView mroqMv_mccr = framework.GetMainView("机械费成本分析对比表");
                            if (mroqMv_mccr != null)
                            {
                                mroqMv_mccr.ViewShow();
                                return null;
                            }
                            VMechanicalCostComparisonRpt vmroq_mccr = new VMechanicalCostComparisonRpt();
                            mroqMv_mccr.ViewCaption = "机械费成本分析对比表";
                            framework.AddMainView(vmroq_mccr);
                            return null;
                        #endregion


                        case CPenaltyDeductionMng_ExecType.TempDedit:
                            TempDeditView("空", "空");
                            return null;
                    }
                }
            }
            return null;
        }


        private void TempDeditView(string name, string id)
        {
            string viewName = "暂扣款单维护";
            string captionName = viewName + "-" + name;

            IMainView mv = framework.GetMainView(captionName);

            if (mv != null)
            {
                //如果当前视图已经存在，直接显示
                mv.ViewShow();
                return;
            }

            VTempDebitMng vMainView = framework.GetMainView(viewName + "-空") as VTempDebitMng;

            if (vMainView == null)
            {
                vMainView = new VTempDebitMng();
                vMainView.ViewName = viewName;
                
                //载入查询视图
                //分配辅助视图
                var searchList = new VTempDebitMngSearchList(this);
                vMainView.AssistViews.Add(searchList);
                VTempDebitMngSearchCon searchCon = new VTempDebitMngSearchCon(searchList);

                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = viewName;
            vMainView.Start(id);
            vMainView.ViewShow();
        }

    }
}
