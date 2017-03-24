using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public enum CPaymentInvoiceType
    {
        PaymentInvoice, //付款发票
        PaymentInvoiceQuery, //付款发票查询
        PaymentInvoiceReport, //付款发票台账
        付款发票抵扣维护
    }

    public   class CPaymentInvoice
    {
        private static IFramework framework = null;
        string mainViewName = "付款发票维护";
        private static VPaymentInvoiceSearchList searchList;

        public CPaymentInvoice(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VPaymentInvoiceSearchList(this);
        }

        public void Start(CPaymentInvoiceType execType)
        {
            Find("空", "空");
        }

        public void Find(string name, string Id)
        {
            mainViewName = "付款发票维护";
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

            VPaymentInvoice vMainView = framework.GetMainView(mainViewName + "-空") as VPaymentInvoice;

            if (vMainView == null)
            {
                vMainView = new VPaymentInvoice();
                //vMainView.MonthlyType = execType;
                vMainView.ViewName = mainViewName;

                //载入查询视图
                vMainView.ViewDeletedEvent += vSaleBudget_ViewDeletedEvent;
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VPaymentInvoiceSearchCon searchCon = new VPaymentInvoiceSearchCon(searchList);

                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(Id);

            vMainView.ViewShow();
        }

        void vSaleBudget_ViewDeletedEvent(object sender)
        {
            IMainView mv = sender as IMainView;

            VFinanceMultData vDmand = mv as VFinanceMultData;
            if (vDmand != null)
                searchList.RemoveRow(vDmand.CurBillMaster.Id);


            IList lst = framework.GetMainViews(mv.ViewName);

            if (lst.Count > 1)
                framework.CloseMainView(mv);
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                Start(0);
            }
            else
            {
                object o = args[0];
                CPaymentInvoiceType execType = (CPaymentInvoiceType)o;
                switch (execType)
                {
                    case CPaymentInvoiceType.PaymentInvoice:
                        {
                            Start(execType);
                            break;
                        }
                    case CPaymentInvoiceType.PaymentInvoiceQuery:
                        {
                            mainViewName = "付款发票查询";
                            var mv = framework.GetMainView(mainViewName);
                            if (mv == null)
                            {
                                mv = new VPaymentInvoiceQuery();
                                framework.AddMainView(mv);
                                mv.ViewCaption = mainViewName;
                            }
                            mv.ViewShow();
                            break;
                        }
                    case CPaymentInvoiceType.付款发票抵扣维护:
                        {
                            mainViewName = "付款发票抵扣维护";
                            var mv = framework.GetMainView(mainViewName);
                            if (mv == null)
                            {
                                mv = new VPaymentInvoiceMng();
                                framework.AddMainView(mv);
                                mv.ViewCaption = mainViewName;
                                mv.ViewName = mainViewName;
                            }
                            mv.ViewShow();
                            break;
                        }
                    case CPaymentInvoiceType.PaymentInvoiceReport:
                        {
                            mainViewName = "付款发票台账";
                           var mv = framework.GetMainView(mainViewName);
                            if (mv == null)
                            {
                                mv = new VPaymentInvoiceReport();
                                framework.AddMainView(mv);
                                mv.ViewCaption = mainViewName;
                            }
                            mv.ViewShow();
                            break;
                        }
                    default:
                        {
                            Start(execType);
                            break;
                        }
                }
            }
            return null;
        }
    }
}
