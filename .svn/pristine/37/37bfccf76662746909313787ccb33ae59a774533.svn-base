using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;


namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report
{

    public enum CReport_ExecType
    {

        料具租赁台账,
        料具损耗情况表,
        租赁结算台帐,
        尺寸分段统计表,
        料具分布报表

    }
   
   public  class CReport
    {
       private static IFramework framework = null;

        public CReport(IFramework fm)
        {
            if (framework == null)
                framework = fm;
        }


        public object Excute(params object[] args)
        {
          
                object obj = args[0];
                if (obj != null && obj.GetType() == typeof(CReport_ExecType))
                {
                    CReport_ExecType execType = (CReport_ExecType)obj;
                    switch (execType)
                    {
                      
                        case CReport_ExecType.料具租赁台账:
                            {
                                IMainView mianView3 = framework.GetMainView("新料具租赁台账");
                                if (mianView3 != null)
                                {
                                    mianView3.ViewShow();
                                    return null;
                                }
                                VMaterialHireReport theVMaterialHireReport = new VMaterialHireReport();
                                theVMaterialHireReport.ViewCaption = "新料具租赁台账";
                                theVMaterialHireReport.ViewName = "新料具租赁台账";
                                framework.AddMainView(theVMaterialHireReport);
                                break;
                            }
                        case CReport_ExecType.料具损耗情况表:
                            {
                                IMainView mianView3 = framework.GetMainView("料具损耗情况表");
                                if (mianView3 != null)
                                {
                                    mianView3.ViewShow();
                                    return null;
                                }
                                VMaterialHireLossReport theVMaterialHireReport = new VMaterialHireLossReport();
                                theVMaterialHireReport.ViewCaption = "料具损耗情况表";
                                theVMaterialHireReport.ViewName = "料具损耗情况表";
                                framework.AddMainView(theVMaterialHireReport);
                                break;
                            }
                        case CReport_ExecType.租赁结算台帐:
                            {
                                IMainView mianView3 = framework.GetMainView("租赁结算台帐");
                                if (mianView3 != null)
                                {
                                    mianView3.ViewShow();
                                    return null;
                                }
                                VMaterialHireBalanceReport oView = new VMaterialHireBalanceReport();
                                oView.ViewCaption = "租赁结算台帐";
                                oView.ViewName = "租赁结算台帐";
                                framework.AddMainView(oView);
                                break;
                            }
                        case CReport_ExecType.尺寸分段统计表:
                            {
                                IMainView mianView3 = framework.GetMainView("尺寸分段统计表");
                                if (mianView3 != null)
                                {
                                    mianView3.ViewShow();
                                    return null;
                                }
                                VMaterialHireSizeReport oView = new VMaterialHireSizeReport();
                                oView.ViewCaption = "尺寸分段统计表";
                                oView.ViewName = "尺寸分段统计表";
                                framework.AddMainView(oView);
                                break;
                            }
                        case CReport_ExecType.料具分布报表:
                            {
                                IMainView mianView3 = framework.GetMainView("料具分布报表");
                                if (mianView3 != null)
                                {
                                    mianView3.ViewShow();
                                    return null;
                                }
                                VMaterialHireDistributeReport oView = new VMaterialHireDistributeReport();
                                oView.ViewCaption = "料具分布报表";
                                oView.ViewName = "料具分布报表";
                                framework.AddMainView(oView);
                                break;
                            }
                    }
                }
             return null;
            }
           
        }
    }
 
