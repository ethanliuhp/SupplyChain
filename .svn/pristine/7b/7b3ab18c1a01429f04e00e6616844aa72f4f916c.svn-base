using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalManage.MaterialRentalContractMng
{
    public enum CMaterialRentalContractPlan_ExecType
    {
        /// <summary>
        /// 设备租赁合同单
        /// </summary>
        MaterialRentalContractQuery
    }

    public class CMaterialRentalContract
    {
        private static IFramework framework = null;
        string mainViewName = "机械租赁合同维护";
        private static VMaterialRentalContractSearchList searchList;//for fixed

        public CMaterialRentalContract(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VMaterialRentalContractSearchList(this);
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

            VMaterialRentalContract vMainView = framework.GetMainView(mainViewName + "-空") as VMaterialRentalContract;

            if (vMainView == null)
            {
                vMainView = new VMaterialRentalContract();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VMaterialRentalContractSearchCon searchCon = new VMaterialRentalContractSearchCon(searchList);

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
                if (obj != null && obj.GetType() == typeof(CMaterialRentalContractPlan_ExecType))
                {
                    CMaterialRentalContractPlan_ExecType execType = (CMaterialRentalContractPlan_ExecType)obj;
                    switch (execType)
                    {
                        case CMaterialRentalContractPlan_ExecType.MaterialRentalContractQuery:
                            IMainView mroqMv = framework.GetMainView("机械租赁合同维护");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VMaterialRentalContractQuery vmroq = new VMaterialRentalContractQuery();
                            vmroq.ViewCaption = "机械租赁合同维护";
                            framework.AddMainView(vmroq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
