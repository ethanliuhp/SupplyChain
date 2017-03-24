using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public enum CPaymentMng_ExecType
    {
        Payment,//付款单
        PaymentOther,//付款单(非工程款)
        PaymentQuery,//付款单查询
        PaymentIntial,//付款单数据初始化
        DepositImport// 保证金导入
    }

    public class CPaymentMng
    {
        private static IFramework framework = null;
        string mainViewName = "付款单";
        private static VPaymentSearchList searchList;

        public CPaymentMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VPaymentSearchList(this);
        }

        public void Start(int paymentType)
        {
            Find("空", "空", paymentType);
        }
        //paymentType,0:付款单(工程款),1:付款单(非工程款)
        public void Find(string name, string Id, int paymentType)
        {
            if (paymentType == 0)
            {
                mainViewName = "付款单(工程款)";
            }
            else
            {
                mainViewName = "付款单(非工程款)";
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

            VPaymentMng vMainView = framework.GetMainView(mainViewName + "-空") as VPaymentMng;

            if (vMainView == null)
            {
                vMainView = new VPaymentMng(paymentType);
                vMainView.ViewName = mainViewName;
                vMainView.PaymentType = paymentType;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VPaymentSearchCon searchCon = new VPaymentSearchCon(searchList, paymentType);

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
                Start(0);
            }
            else
            {
                object obj = args[0];
                CPaymentMng_ExecType executeType = (CPaymentMng_ExecType)obj;
                switch (executeType)
                {
                    case CPaymentMng_ExecType.Payment:
                        Start(0);
                        break;
                    case CPaymentMng_ExecType.PaymentOther:
                        Start(1);
                        break;
                    case CPaymentMng_ExecType.PaymentQuery:
                        string viewName = "付款单查询";
                        IMainView mv = framework.GetMainView(viewName);
                        if (mv == null)
                        {
                            mv = new VPaymentQuery();
                            framework.AddMainView(mv);
                            mv.ViewCaption = viewName;
                        }
                        mv.ViewShow();
                        break;
                    case CPaymentMng_ExecType.PaymentIntial:
                        {

                            viewName = "付款数据初始化";
                             mv = framework.GetMainView(viewName);
                            if (mv == null)
                            {
                                mv = new VPaymentMngIntial(executeType);
                                framework.AddMainView(mv);
                                mv.ViewCaption = viewName;
                            }
                            mv.ViewShow();
                            break;
                        }
                    case CPaymentMng_ExecType.DepositImport:
                        {
                            viewName = "保证金导入";

                            mv = framework.GetMainView(viewName);
                            if (mv == null)
                            {
                                mv = new VDepositImport(executeType);
                                framework.AddMainView(mv);
                                mv.ViewCaption = viewName;
                            }
                            mv.ViewShow();
                            break;
                        }
                }

            }
            return null;
        }
    }
}
