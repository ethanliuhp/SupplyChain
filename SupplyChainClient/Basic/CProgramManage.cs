using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.Basic
{
    public enum CProgramManageMng_ExecType
    {
        /// <summary>
        /// 物资价查询
        /// </summary>
        ProgramManage
    }

    public class CProgramManage
    {
        private static IFramework framework = null;
        string mainViewName = "信息价格维护";

        public CProgramManage(IFramework fm)
        {
            if (framework == null)
                framework = fm;
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
            } else
            {
                object obj=args[0];
                if (obj != null && obj.GetType() == typeof(CProgramManageMng_ExecType))
                {
                    CProgramManageMng_ExecType execType = (CProgramManageMng_ExecType)obj;
                    switch (execType)
                    {
                        case CProgramManageMng_ExecType.ProgramManage:
                            IMainView mroqMv = framework.GetMainView("信息价格维护");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VProgramManage vmroq = new VProgramManage();
                            vmroq.ViewCaption = "信息价格维护";
                            framework.AddMainView(vmroq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
