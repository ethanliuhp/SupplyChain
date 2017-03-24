using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.ResourceManager.Client.Main;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppProcessUI;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatformUI
{
    public enum CAppPlatform_EnumType
    {
        SetBill,
        ApproveQuery
    }
    public class CAppPlatform
    {
 
        private static VirtualMachine.Component.WinMVC.generic.IFramework  framework;
        public CAppPlatform(VirtualMachine.Component.WinMVC.generic.IFramework fw)
        {
            if (framework == null)
                framework = fw;
        }

        public object Excute(params object[] args)
        {
            if (args.Length != 0)
            {
                if (args.Length > 1)
                {
                    object o = args[1];
                    IMainView mv = null;
                    if (o != null && o.GetType() == typeof(CAppPlatform_EnumType))
                    {
                        CAppPlatform_EnumType t = (CAppPlatform_EnumType)o;
                        if (t == CAppPlatform_EnumType.SetBill)
                        {
                            mv = framework.GetMainView("业务单据修改");
                            if (mv != null)
                            {
                                //如果当前视图已经存在，直接显示
                                mv.ViewShow();
                                return null;
                            }
                            VSetBillProperty oVSetBillProperty = new VSetBillProperty();
                            oVSetBillProperty.ViewCaption = "业务单据修改";
                            framework.AddMainView(oVSetBillProperty);
                            oVSetBillProperty.ViewShow();
                            return null;
                        }
                        else if (t == CAppPlatform_EnumType.ApproveQuery)
                        {
                            mv = framework.GetMainView("单据审批查询");
                            if (mv != null)
                            {
                                //如果当前视图已经存在，直接显示
                                mv.ViewShow();
                                return null;
                            }
                            VAppStatusQueryNew oVappQuery = new VAppStatusQueryNew();
                            oVappQuery.ViewCaption = "单据审批查询";
                            framework.AddMainView(oVappQuery);
                            oVappQuery.ViewShow();
                            return null;
                        }
                    }

                    mv = framework.GetMainView("审批平台");
                    if (mv != null)
                    {
                        //如果当前视图已经存在，直接显示
                        mv.ViewShow();
                        return null;
                    }
                    //VAppPlatform theVAppPlatform = new VAppPlatform();
                    //theVAppPlatform.ViewCaption = "审批平台";

                    VAppPlatformNew theVAppPlatform = new VAppPlatformNew();
                    theVAppPlatform.ViewCaption = "审批平台";
                    framework.AddMainView(theVAppPlatform);
                    theVAppPlatform.ViewShow();
                    return null;
                }
            }
            else
            {
                object o = args[0];
               
               
            }
            return null;
        
        }
    }
}

