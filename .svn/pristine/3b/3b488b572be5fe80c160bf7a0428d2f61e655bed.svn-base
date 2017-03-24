using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireLedger;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection
{
    public enum CMaterialHireCollection_ExecType
    {
        /// <summary>
        /// 料具收料单
        /// </summary>
        MaterialHireCollection,
        /// <summary>
        /// 钢管检尺单
        /// </summary>
        MaterialHireCollectionGGCheck,
        /// <summary>
        /// 碗扣检尺单
        /// </summary>
        MaterialHireCollectionWKCheck,
        /// <summary>
        /// 料具收料单查询
        /// </summary>
        MaterialHireCollectionQuery,
        /// <summary>
        /// 料具租赁台账
        /// </summary>
        MaterialHireLedgerQuery,
        /// <summary>
        /// 料具租赁流水账
        /// </summary>
        MaterialHireCurrentCount,
        /// <summary>
        /// 料具台账引用
        /// </summary>
        MaterialHireLedSelector,
        /// <summary>
        /// 料具租赁月结
        /// </summary>
        MaterialHireMonthlyBalance,
        /// <summary>
        /// 料具租赁月报
        /// </summary>
        MaterialHireMonthlyQuery//,
       // 料具租赁台账

       
    }
   
   public  class CMaterialHireCollection
    {
       private static IFramework framework = null;
        //string mainViewName = "料具收料单";
        private  VMaterialHireCollectionSearchList searchList;

        public CMaterialHireCollection(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VMaterialHireCollectionSearchList(this);
        }
        public void Start(EnumMatHireType collectionType )
        {
            Find("空", "空", collectionType);
        }

        public void Find(string name, string Id, EnumMatHireType collectionType)
        {
            string mainViewName = (collectionType == EnumMatHireType.普通料具 ? "料具发料单" : (collectionType == EnumMatHireType.钢管 ? "检尺单(钢管)" : "检尺单(碗扣)"));
           string captionName = mainViewName;
            if (name is string)
            {
                captionName = mainViewName + "-" + name;
            }
            IMainView mv = framework.GetMainView(captionName);
            if (mv != null)
            {
                //如果当前视图已经存在，直接显示
                mv.ViewShow();
                return;
            }

            switch (collectionType)
            {
                case EnumMatHireType.钢管:
                    {
                        VMaterialHireCollection vMainView = framework.GetMainView(mainViewName + "-空") as VMaterialHireCollection;

                        if (vMainView == null)
                        {
                            vMainView = new VMaterialHireCollection(collectionType);
                            vMainView.ViewName = mainViewName;

                            //载入查询视图
                            //分配辅助视图
                            vMainView.AssistViews.Add(searchList);
                            VMaterialHireCollectionSearchCon searchCon = new VMaterialHireCollectionSearchCon(searchList, collectionType);
                            vMainView.AssistViews.Add(searchCon);
                            //载入框架
                            framework.AddMainView(vMainView);
                        }
                        vMainView.ViewCaption = captionName;
                        vMainView.ViewName = mainViewName;
                        vMainView.Start(Id);
                        vMainView.ViewShow();
                        break;
                    }
                case EnumMatHireType.普通料具:
                    {

                        VMaterialHireCollection vMainView = framework.GetMainView(mainViewName + "-空") as VMaterialHireCollection;

                        if (vMainView == null)
                        {
                            vMainView = new VMaterialHireCollection(collectionType);
                        vMainView.ViewName = mainViewName;

                        //载入查询视图
                        //分配辅助视图
                        vMainView.AssistViews.Add(searchList);
                        VMaterialHireCollectionSearchCon searchCon = new VMaterialHireCollectionSearchCon(searchList, collectionType);
                        vMainView.AssistViews.Add(searchCon);
                        //载入框架
                        framework.AddMainView(vMainView);
                        }
                        vMainView.ViewCaption = captionName;
                        vMainView.ViewName = mainViewName;
                        vMainView.Start(Id);
                        vMainView.ViewShow();
                        break;
                    }
                case EnumMatHireType.碗扣:
                    {
                        VMaterialHireCollection vMainView = framework.GetMainView(mainViewName + "-空") as VMaterialHireCollection;

                        if (vMainView == null)
                        {
                            vMainView = new VMaterialHireCollection(collectionType);
                            vMainView.ViewName = mainViewName;

                            //载入查询视图
                            //分配辅助视图
                            vMainView.AssistViews.Add(searchList);
                            VMaterialHireCollectionSearchCon searchCon = new VMaterialHireCollectionSearchCon(searchList, collectionType);
                            vMainView.AssistViews.Add(searchCon);
                            //载入框架
                            framework.AddMainView(vMainView);
                        }
                        vMainView.ViewCaption = captionName;
                        vMainView.ViewName = mainViewName;
                        vMainView.Start(Id);
                        vMainView.ViewShow();
                        break;
                        break;
                    }
            }
              
               
 
           
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                Start(EnumMatHireType.普通料具);
            }
            else
            {
                object obj = args[0];
                if (obj != null && obj.GetType() == typeof(CMaterialHireCollection_ExecType))
                {
                    CMaterialHireCollection_ExecType execType = (CMaterialHireCollection_ExecType)obj;
                    switch (execType)
                    {
                        case CMaterialHireCollection_ExecType.MaterialHireCollectionGGCheck:
                            {
                                Start(EnumMatHireType.钢管);
                                break;
                            }
                        case CMaterialHireCollection_ExecType.MaterialHireCollectionWKCheck:
                            {
                                Start(EnumMatHireType.碗扣); break;
                            }
                        case CMaterialHireCollection_ExecType.MaterialHireCollectionQuery:
                            {
                                IMainView mroqMv = framework.GetMainView("发料单统计查询");
                                if (mroqMv != null)
                                {
                                    mroqMv.ViewShow();
                                    return null;
                                }
                                VMaterialHireCollectionQuery vmcq = new VMaterialHireCollectionQuery();
                                vmcq.ViewCaption = "发料单统计查询";
                                framework.AddMainView(vmcq);
                                return null;
                            }
                        case CMaterialHireCollection_ExecType.MaterialHireCurrentCount:
                            {
                                IMainView mainView = framework.GetMainView("料具租赁流水账");
                                if (mainView != null)
                                {
                                    mainView.ViewShow();
                                    return null;
                                }
                                VMaterialHireLedgerQuery theVMaterialRentalLedger = new VMaterialHireLedgerQuery();
                                theVMaterialRentalLedger.ViewCaption = "料具租赁流水账";
                                framework.AddMainView(theVMaterialRentalLedger);
                                return null;
                            }
                        case CMaterialHireCollection_ExecType.MaterialHireLedgerQuery:
                            {
                                IMainView mainView2 = framework.GetMainView("料具租赁台账");
                                if (mainView2 != null)
                                {
                                    mainView2.ViewShow();
                                    return null;
                                }
                                VMaterialHireLedgerQuery1 theVMaterialRentalLedger1 = new VMaterialHireLedgerQuery1();
                                theVMaterialRentalLedger1.ViewCaption = "料具租赁台账";
                                framework.AddMainView(theVMaterialRentalLedger1);
                                return null;
                            }
                        //case CMaterialHireCollection_ExecType.MaterialHireLedSelector:
                        //    VMaterialHireLedgerSelector theVMaterialRenLedSelector = new VMaterialHireLedgerSelector();
                        //    theVMaterialRenLedSelector.ShowDialog();
                        //    return theVMaterialRenLedSelector.Result;

                        case CMaterialHireCollection_ExecType.MaterialHireMonthlyBalance://CMaterialHireCollection_ExecType.MaterialMonthlyBalance:
                            {
                                IMainView mainView1 = framework.GetMainView("料具租赁月结");
                                if (mainView1 != null)
                                {
                                    mainView1.ViewShow();
                                    return null;
                                }
                                VMaterialHireMonthlyBalance theVMaterialMonthlyBalance = new VMaterialHireMonthlyBalance();
                                theVMaterialMonthlyBalance.ViewCaption = "料具租赁月结";
                                theVMaterialMonthlyBalance.ViewName = "料具租赁月结";
                                framework.AddMainView(theVMaterialMonthlyBalance);
                                return null;
                            }
                        case CMaterialHireCollection_ExecType.MaterialHireMonthlyQuery://CMaterialHireCollection_ExecType.MaterialMonthlyQuery:
                            {
                                IMainView mianView2 = framework.GetMainView("新料具租赁月结查询");
                                if (mianView2 != null)
                                {
                                    mianView2.ViewShow();
                                    return null;
                                }
                                VMaterialHireMonthlyBalanceQuery theVMaterialMonthlyBalanceQuery = new VMaterialHireMonthlyBalanceQuery();
                                theVMaterialMonthlyBalanceQuery.ViewCaption = "新料具租赁月结查询";
                                theVMaterialMonthlyBalanceQuery.ViewName = "新料具租赁月结查询";
                                framework.AddMainView(theVMaterialMonthlyBalanceQuery);
                                return null;
                            }
                       
                    }
                }
            }
            return null;
        }
    }
}
