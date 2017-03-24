using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.BusinessProposalManage
{
    public enum EnumBusinessPropoasal
    {
        /// <summary>
        /// 商务维护查询
        /// </summary>
        BusinessProposalSeaech,
    }
    public class CBusinessProposal
    {
        private static IFramework framework = null;
        string mainViewName = "商务策划维护";
        private static VBusinessProposalSearchList searchList;

        public CBusinessProposal(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VBusinessProposalSearchList(this);
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

            VBusinessProposal vMainView = framework.GetMainView(mainViewName + "-空") as VBusinessProposal;

            if (vMainView == null)
            {
                vMainView = new VBusinessProposal();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VBusinessProposalSearchCon searchCon = new VBusinessProposalSearchCon(searchList);
                //VWasteMaterialHandleSearchList searchCon = new VWasteMaterialHandleSearchList(searchList);

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
            }
            else
            {
                object obj = args[0];
                if (obj != null && obj.GetType() == typeof(EnumBusinessPropoasal))
                {
                    EnumBusinessPropoasal execType = new EnumBusinessPropoasal();
                    switch (execType)
                    {
                        case EnumBusinessPropoasal.BusinessProposalSeaech:
                            IMainView mroqMv = framework.GetMainView("商务策划查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VBusinessProposalQuery vcdq = new VBusinessProposalQuery();
                            vcdq.ViewCaption = "商务策划查询";
                            framework.AddMainView(vcdq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
