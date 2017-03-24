using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalManage.MaterialRentalSettlementMng
{
    public enum CMaterialRentalSettlementPlan_ExecType
    {
        /// <summary>
        /// 设备租赁结算单
        /// </summary>
        MaterialRentalSettlementQuery
    }

    public class CMaterialRentalSettlement
    {
        private static IFramework framework = null;
        string mainViewName = "机械租赁结算单";
        private static VMaterialRentalSettlementSearchList searchList;

        public CMaterialRentalSettlement(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VMaterialRentalSettlementSearchList(this);
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

            VMaterialRentalSettlement vMainView = framework.GetMainView(mainViewName + "-空") as VMaterialRentalSettlement;

            if (vMainView == null)
            {
                vMainView = new VMaterialRentalSettlement();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VMaterialRentalSettlementSearchCon searchCon = new VMaterialRentalSettlementSearchCon(searchList);

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
                if (obj != null && obj.GetType() == typeof(CMaterialRentalSettlementPlan_ExecType))
                {
                    CMaterialRentalSettlementPlan_ExecType execType = (CMaterialRentalSettlementPlan_ExecType)obj;
                    switch (execType)
                    {
                        case CMaterialRentalSettlementPlan_ExecType.MaterialRentalSettlementQuery:
                            IMainView mroqMv = framework.GetMainView("机械租赁结算单查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VMaterialRentalSettlementQuery vmroq = new VMaterialRentalSettlementQuery();
                            vmroq.ViewCaption = "机械租赁结算单查询";
                            framework.AddMainView(vmroq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
