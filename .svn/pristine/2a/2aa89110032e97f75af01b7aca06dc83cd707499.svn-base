using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Business.Erp.SupplyChain.Client.BasicData.ClassesMng
{
    public enum ClassesExcType
    {
        search
    }
   public class CClasses
    {
        private static VirtualMachine.Component.WinMVC.generic.IFramework framework;
        public CClasses(VirtualMachine.Component.WinMVC.generic.IFramework fw)
        {
            if (framework == null)
                framework = fw;
        }
        public object Excute(params object[] args)
        {


            if (args.Length == 0)
            {
                VClasses aVClasses = new VClasses();
                aVClasses.ShowDialog();
            }
            else
            {
                ClassesExcType theCTET = (ClassesExcType)args[0];
                switch (theCTET)
                {
                    case ClassesExcType.search:
                        VClassesSerach theVClassesSerach = new VClassesSerach();
                        theVClassesSerach.ShowDialog();
                        return theVClassesSerach.Result;
                    default:
                        break;
                }
            }
            return null;

        }
    }
}
