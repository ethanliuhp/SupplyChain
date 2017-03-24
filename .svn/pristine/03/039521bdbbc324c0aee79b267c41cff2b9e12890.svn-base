using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.ProInRecord
{
    public enum CProInRecordMng_exectype
    {
        VProInRecordMng
    }
    public class CProInRecordMng
    {
        private static IFramework framework = null;
        string mainViewName = "专业检查";

        public CProInRecordMng(IFramework fm)
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
                if (obj != null && obj.GetType() == typeof(CProInRecordMng_exectype))
                {
                    CProInRecordMng_exectype execType = (CProInRecordMng_exectype)obj;
                    switch (execType)
                    {
                        case CProInRecordMng_exectype.VProInRecordMng:
                            IMainView mroqMv = framework.GetMainView("专业检查");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VProInRecordMng vscm = new VProInRecordMng();
                            vscm.ViewCaption = "专业检查";
                            framework.AddMainView(vscm);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
