using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost
{
    public enum EnumBorrowedOrder
    {
        借款单查询=0
    }
    public class CBorrowedOrder
    {
        public CBorrowedOrder(IFramework fm)
        {
            if (framework == null)
                framework = fm;
             searchList = new VBorrowedOrderSearchList(this);
        }
        private static IFramework framework = null;
        private static VBorrowedOrderSearchList searchList;
        string mainViewName = "借款单";
        public object Excute(params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                IMainView mv = null;
                EnumBorrowedOrder execType=(EnumBorrowedOrder)args[0] ;
                string captionName = execType.ToString();
                switch (execType)
                {
                    case EnumBorrowedOrder.借款单查询:
                        {
                            mv = framework.GetMainView(captionName);
                            if (mv == null)
                            {
                                mv = new VBorrowedOrderQuery(execType);
                                framework.AddMainView(mv);
                                mv.ViewCaption = captionName;
                                mv.ViewName = captionName;
                            }
                            mv.ViewShow();
                            break;
                        }
                }
                 
            }
            else
            {
                Start();
            }
            return null;
        }

        private void Start()
        {
            Find("空","空");
        }

        public void Find(string name,string id)
        {
            string captionName = mainViewName;                          
            if (name is string)
            {
                captionName = mainViewName + "-" + name;                // 视图标题 
            }
            IMainView mv = framework.GetMainView(captionName);          // 在主窗体中获取当前视图
            if (mv != null)
            {
                // 如果当前视图存在，则直接显示
                mv.ViewShow();
                return;
            }
            // 视图不存在
            VBorrowedOrder vMainView = new VBorrowedOrder();
           
            // 载入查询视图
            vMainView.ViewDeletedEvent += new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(a =>
            {
                var vBorrowOrder = a as VBorrowedOrder;
                if (vBorrowOrder != null) searchList.RemoveRow(vMainView.master.Id);
                IList list = framework.GetMainViews(vMainView.ViewName);
                if (list.Count > 1)
                {
                    framework.CloseMainView(vMainView);
                }
            });
            // 分配辅助视图
            vMainView.AssistViews.Add(searchList);
            VBorrowedOrderSearchCon searchCon = new VBorrowedOrderSearchCon(searchList);
            vMainView.AssistViews.Add(searchCon);
            // 载入框架
            framework.AddMainView(vMainView);
            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.RegisteViewToSubmit();
            vMainView.Start(id);
        }
    }
}
