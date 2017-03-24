using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.DailyCorrection
{

    public enum CDailyCorrection_ExecType
    {
        VDailyCorrectionMaster
    }
    public class CDailyCorrection
    {
        private static IFramework framework = null;
        string mainViewName = "整改单确认";

        public CDailyCorrection(IFramework fm)
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
                object obj = args[0];
                if (obj != null && obj.GetType() == typeof(CDailyCorrection_ExecType))
                {
                    CDailyCorrection_ExecType execType = (CDailyCorrection_ExecType)obj;
                    switch (execType)
                    {
                        case CDailyCorrection_ExecType.VDailyCorrectionMaster:
                            IMainView mroqMv = framework.GetMainView("整改单确认");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            IList list = new ArrayList();
                            int i = 0;
                            VDailyCorrectionMaster vgm = new VDailyCorrectionMaster(list,i);
                            vgm.ViewCaption = "整改单确认";
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
