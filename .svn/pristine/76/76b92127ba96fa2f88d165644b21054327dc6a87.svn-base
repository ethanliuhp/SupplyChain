using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.StockMng;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyPlanMng
{
    public enum CSupplyPlanMng_ExecType
    {
        /// <summary>
        /// 采购计划查询
        /// </summary>
        SupplyPlanMngQuery
    }
    
    public class CSupplyPlanMng
    {
        private static IFramework framework = null;
        string mainViewName = "专业需求计划";
        private static VSupplyPlanSearchList searchList;

        public CSupplyPlanMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VSupplyPlanSearchList(this);
        }

        public void Start()
        {
            Find("空","空");
        }

        public void Find(string name,string Id)
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

            VSupplyPlanMng vMainView = framework.GetMainView(mainViewName + "-空") as VSupplyPlanMng;

            if (vMainView == null)
            {
                vMainView = new VSupplyPlanMng(EnumSupplyType.安装);
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VSupplyPlanSearchCon searchCon = new VSupplyPlanSearchCon(searchList);

                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(Id);

            vMainView.ViewShow();
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                Start();
            } else
            {
                object obj=args[0];
                if (obj != null && obj.GetType() == typeof(CSupplyPlanMng_ExecType))
                {
                    CSupplyPlanMng_ExecType execType = (CSupplyPlanMng_ExecType)obj;
                    switch (execType)
                    {
                        case CSupplyPlanMng_ExecType.SupplyPlanMngQuery:
                            //IMainView mroqMv = framework.GetMainView("采购计划查询");
                            IMainView mroqMv = framework.GetMainView("专业需求计划查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VSupplyPlanMngQuery vmroq = new VSupplyPlanMngQuery(EnumStockExecType.安装);
                            //vmroq.ViewCaption = "采购计划查询";
                            vmroq.ViewCaption = "专业需求计划查询";
                            framework.AddMainView(vmroq);
                            return null;
                        //case CMonthlyPlanMng_ExecType.MaterialRentalOrderRef:
                        //    VMaterialRentalOrderSelector vmros = new VMaterialRentalOrderSelector();
                        //    vmros.ShowDialog();
                        //    return vmros.Result;
                    }
                }
            }
            return null;
        }
    }
}
