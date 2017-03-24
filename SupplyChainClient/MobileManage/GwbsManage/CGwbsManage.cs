using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.GwbsManage
{
    public enum CGwbsManage_ExecType
    {
        VGwbsManage
    }
    public class CGwbsManage
    {
         private static IFramework framework = null;
         string mainViewName = "施工任务信息查询";

        public CGwbsManage(IFramework fm)
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
                if (obj != null && obj.GetType() == typeof(CGwbsManage_ExecType))
                {
                    CGwbsManage_ExecType execType = (CGwbsManage_ExecType)obj;
                    switch (execType)
                    {
                        case CGwbsManage_ExecType.VGwbsManage:
                            IMainView mroqMv = framework.GetMainView("施工任务信息查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VGwbsManage vgm = new VGwbsManage();
                            vgm.ViewCaption = "施工任务信息查询";
                            framework.AddMainView(vgm);
                            return null;
                        //case CExcelImportMng_ExecType.VExcelImportMng:
                        //    IMainView mo = framework.GetMainView("导入基础数据");
                        //    if(mo != null)
                        //    {
                        //        mo.ViewShow();
                        //        return null;
                        //    }
                        //    VExcelImportMng vexim = new VExcelImportMng();
                        //    vexim.ViewCaption = "导入基础数据";
                        //    framework.AddMainView(vexim);
                        //    return null;
                    }
                }
            }
            return null;
        }
    }
}
