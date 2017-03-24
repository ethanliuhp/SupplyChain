﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatMng
{
    public enum CAppPlatMng_ExecType
    {
        VAppPlMng
    }
    public class CAppPlatMng
    {
         private static IFramework framework = null;
         string mainViewName = "表单审批";
        //private static VWeekPlanSearchList searchList;  

         public CAppPlatMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
        }
         public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
            } 
            else
            {
                object obj=args[0];
                if (obj != null && obj.GetType() == typeof(CAppPlatMng_ExecType))
                {
                    CAppPlatMng_ExecType execType = (CAppPlatMng_ExecType)obj;
                    IMainView mroqMv = null;
                    switch (execType)
                    {
                        case CAppPlatMng_ExecType.VAppPlMng:
                            mroqMv = framework.GetMainView("表单审批");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VAppPlatMng vow = new VAppPlatMng();
                            vow.ViewCaption = "表单审批";
                            framework.AddMainView(vow);
                            vow.ViewShow();
                            return null;
                        //case CAppPlatMng_ExecType.VTest:
                        //    mroqMv = framework.GetMainView("测试界面");
                        //    if (mroqMv != null)
                        //    {
                        //        mroqMv.ViewShow();
                        //        return null;
                        //    }
                        //    VTest vt = new VTest();
                        //    vt.ViewCaption = "测试界面";
                        //    framework.AddMainView(vt);
                        //    return null;
                    }
                }
            }
            return null;
        }
    }
    
}
