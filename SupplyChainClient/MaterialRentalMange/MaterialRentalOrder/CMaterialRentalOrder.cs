using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalOrder
{
    public enum CMaterialRentalOrder_ExecType
    {
        /// <summary>
        /// 料具租赁合同查询
        /// </summary>
        MaterialRentalOrderQuery,
        /// <summary>
        /// 料具租赁合同引用
        /// </summary>
        MaterialRentalOrderRef,
        /// <summary>
        /// 料具租赁复制
        /// </summary>
        MaterialRentalOrderCopy
    }

    public class CMaterialRentalOrder
    {
        private static IFramework framework = null;
        string mainViewName = "料具租赁合同";
        private static VMaterialRentalOrderSearchList searchList;

        public CMaterialRentalOrder(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VMaterialRentalOrderSearchList(this);
        }

        public void Start()
        {
            Find("空", "空");
        }

        public void Find(string name, string id)
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

            VMaterialRentalOrder vMainView = framework.GetMainView(mainViewName + "-空") as VMaterialRentalOrder;

            if (vMainView == null)
            {
                vMainView = new VMaterialRentalOrder();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VMaterialRentalOrderSearchCon searchCon = new VMaterialRentalOrderSearchCon(searchList);

                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(id);

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
                if (obj != null && obj.GetType() == typeof(CMaterialRentalOrder_ExecType))
                {
                    CMaterialRentalOrder_ExecType execType = (CMaterialRentalOrder_ExecType)obj;
                    switch (execType)
                    {
                        case CMaterialRentalOrder_ExecType.MaterialRentalOrderQuery:
                            IMainView mroqMv = framework.GetMainView("料具租赁合同查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VMaterialRentalOrderQuery vmroq = new VMaterialRentalOrderQuery();
                            vmroq.ViewCaption = "料具租赁合同查询";
                            framework.AddMainView(vmroq);
                            return null;
                        case CMaterialRentalOrder_ExecType.MaterialRentalOrderCopy:
                            IMainView mroqMv1 = framework.GetMainView("料具租赁合同复制");
                            if (mroqMv1 != null)
                            {
                                mroqMv1.ViewShow();
                                return null;
                            }
                            VMaterialRentalOrderCopy vmroq1 = new VMaterialRentalOrderCopy();
                            vmroq1.ViewCaption = "料具租赁合同复制";
                            framework.AddMainView(vmroq1);
                            return null;
                        case CMaterialRentalOrder_ExecType.MaterialRentalOrderRef:
                            VMaterialRentalOrderSelector vmros = new VMaterialRentalOrderSelector();
                            vmros.ShowDialog();
                            return vmros.Result;
                    }
                }
            }
            return null;
        }
    }
}
