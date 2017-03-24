using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.FinanceMng.MaterialsettlementMng
{
    public enum CMaterialSettlementMng_ExecType
    {
        /// <summary>
        /// 材料结算查询
        /// </summary>
        MaterialSettlementQuery
    }
    
    public class CMaterialSettlementMng
    {
        private static IFramework framework = null;
        string mainViewName = "材料结算单";
        private static VMaterialSettlementSearchList searchList;

        public CMaterialSettlementMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VMaterialSettlementSearchList(this);
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

            VMaterialSettlementMng vMainView = framework.GetMainView(mainViewName + "-空") as VMaterialSettlementMng;

            if (vMainView == null)
            {
                vMainView = new VMaterialSettlementMng();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VMaterialSettlementSearchCon searchCon = new VMaterialSettlementSearchCon(searchList);

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
                if (obj != null && obj.GetType() == typeof(CMaterialSettlementMng_ExecType))
                {
                    CMaterialSettlementMng_ExecType execType = (CMaterialSettlementMng_ExecType)obj;
                    switch (execType)
                    {
                        case CMaterialSettlementMng_ExecType.MaterialSettlementQuery:
                            IMainView mroqMv = framework.GetMainView("材料结算查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VMaterialSettlementQuery vmroq = new VMaterialSettlementQuery();
                            vmroq.ViewCaption = "材料结算查询";
                            framework.AddMainView(vmroq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
