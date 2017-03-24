using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.ExcelImportMng
{
    public enum CExcelImportMng_ExecType
    {
        /// <summary>
        /// 基础数据导入
        /// </summary>
        VExcelImport,

        VExcelImportMng
    }
    
    public class CExcelImportMng
    {
        private static IFramework framework = null;
        string mainViewName = "基础数据导入";

        public CExcelImportMng(IFramework fm)
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
                if (obj != null && obj.GetType() == typeof(CExcelImportMng_ExecType))
                {
                    CExcelImportMng_ExecType execType = (CExcelImportMng_ExecType)obj;
                    switch (execType)
                    {
                        case CExcelImportMng_ExecType.VExcelImport:
                            IMainView mroqMv = framework.GetMainView("基础数据导入");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VExcelImport vmroq = new VExcelImport();
                            vmroq.ViewCaption = "基础数据导入";
                            framework.AddMainView(vmroq);
                            return null;
                        case CExcelImportMng_ExecType.VExcelImportMng:
                            IMainView mo = framework.GetMainView("导入基础数据");
                            if(mo != null)
                            {
                                mo.ViewShow();
                                return null;
                            }
                            VExcelImportMng vexim = new VExcelImportMng();
                            vexim.ViewCaption = "导入基础数据";
                            framework.AddMainView(vexim);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
