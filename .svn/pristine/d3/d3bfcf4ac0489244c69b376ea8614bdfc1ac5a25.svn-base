using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.OftenWords
{
    public enum COftenWords_ExecType
    {
        VOftenWords,
        VTest
    }
    public class COftenWords
    {
        private static IFramework framework = null;
        //string mainViewName = "常用短语维护";

        public COftenWords(IFramework fm)
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
                if (obj != null && obj.GetType() == typeof(COftenWords_ExecType))
                {
                    COftenWords_ExecType execType = (COftenWords_ExecType)obj;
                    IMainView mroqMv = null;
                    switch (execType)
                    {
                        case COftenWords_ExecType.VOftenWords:
                            mroqMv = framework.GetMainView("常用短语维护");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VOftenWords vow = new VOftenWords();
                            vow.ViewCaption = "常用短语维护";
                            framework.AddMainView(vow);
                            return null;
                        case COftenWords_ExecType.VTest:
                            mroqMv = framework.GetMainView("测试界面");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VTest vt = new VTest();
                            vt.ViewCaption = "测试界面";
                            framework.AddMainView(vt);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
