using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalLedgerMng;

namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialCollection
{

    public enum CMaterialCollection_ExecType
    {
        /// <summary>
        /// 料具收料单查询
        /// </summary>
        MaterialCollectionQuery,
        /// <summary>
        /// 料具租赁台账
        /// </summary>
        MaterialRentalLedgerQuery,
        /// <summary>
        /// 料具租赁流水账
        /// </summary>
        MaterialRentalCurrentCount,
        /// <summary>
        /// 料具台账引用
        /// </summary>
        MaterialRenLedSelector,
        /// <summary>
        /// 料具租赁月结
        /// </summary>
        MaterialMonthlyBalance,
        /// <summary>
        /// 料具租赁月报
        /// </summary>
        MaterialMonthlyQuery
    }

    public class CMaterialCollection
    {
        private static IFramework framework = null;
        string mainViewName = "料具收料单";
        private static VMaterialCollectionSearchList searchList;

        public CMaterialCollection(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VMaterialCollectionSearchList(this);
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
            VMaterialCollection vMainView = framework.GetMainView(mainViewName + "-空") as VMaterialCollection;

            if (vMainView == null)
            {
                vMainView = new VMaterialCollection();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VMaterialCollectionSearchCon searchCon = new VMaterialCollectionSearchCon(searchList);
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
                if (obj != null && obj.GetType() == typeof(CMaterialCollection_ExecType))
                {
                    CMaterialCollection_ExecType execType = (CMaterialCollection_ExecType)obj;
                    switch (execType)
                    {
                        case CMaterialCollection_ExecType.MaterialCollectionQuery:
                            IMainView mroqMv = framework.GetMainView("收料单统计查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VMaterialCollectionQuery vmcq = new VMaterialCollectionQuery();
                            vmcq.ViewCaption = "收料单统计查询";
                            framework.AddMainView(vmcq);
                            return null;

                        case CMaterialCollection_ExecType.MaterialRentalCurrentCount:
                            IMainView mainView = framework.GetMainView("料具租赁流水账");
                            if (mainView != null)
                            {
                                mainView.ViewShow();
                                return null;
                            }
                            VMaterialRentalLedgerQuery theVMaterialRentalLedger = new VMaterialRentalLedgerQuery();
                            theVMaterialRentalLedger.ViewCaption = "料具租赁流水账";
                            framework.AddMainView(theVMaterialRentalLedger);
                            return null;
                        case CMaterialCollection_ExecType.MaterialRentalLedgerQuery:
                            IMainView mainView2 = framework.GetMainView("料具租赁台账");
                            if (mainView2 != null)
                            {
                                mainView2.ViewShow();
                                return null;
                            }
                            VMaterialRentalLedgerQuery1 theVMaterialRentalLedger1 = new VMaterialRentalLedgerQuery1();
                            theVMaterialRentalLedger1.ViewCaption = "料具租赁台账";
                            framework.AddMainView(theVMaterialRentalLedger1);
                            return null;

                        case CMaterialCollection_ExecType.MaterialRenLedSelector:
                            VMaterialRenLedSelector theVMaterialRenLedSelector = new VMaterialRenLedSelector();
                            theVMaterialRenLedSelector.ShowDialog();
                            return theVMaterialRenLedSelector.Result;

                        case CMaterialCollection_ExecType.MaterialMonthlyBalance:
                            IMainView mainView1 = framework.GetMainView("料具租赁月结");
                            if (mainView1 != null)
                            {
                                mainView1.ViewShow();
                                return null;
                            }
                            VMaterialMonthlyBalance theVMaterialMonthlyBalance = new VMaterialMonthlyBalance();
                            theVMaterialMonthlyBalance.ViewCaption = "料具租赁月结";
                            theVMaterialMonthlyBalance.ViewName = "料具租赁月结";
                            framework.AddMainView(theVMaterialMonthlyBalance);
                            return null;

                        case CMaterialCollection_ExecType.MaterialMonthlyQuery:
                            IMainView mianView2 = framework.GetMainView("料具租赁结算表");
                            if (mianView2 != null)
                            {
                                mianView2.ViewShow();
                                return null;
                            }
                            VMaterialMonthlyBalanceQuery theVMaterialMonthlyBalanceQuery = new VMaterialMonthlyBalanceQuery();
                            theVMaterialMonthlyBalanceQuery.ViewCaption = "料具租赁结算表";
                            theVMaterialMonthlyBalanceQuery.ViewName = "料具租赁结算表";
                            framework.AddMainView(theVMaterialMonthlyBalanceQuery);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
