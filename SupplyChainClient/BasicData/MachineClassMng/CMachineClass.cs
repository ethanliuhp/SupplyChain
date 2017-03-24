using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Business.Erp.SupplyChain.Client.BasicData.MachineClassMng
{
    public enum MachineClassExcType
    {
        search
    }
    class CMachineClass
    {
       private static VirtualMachine.Component.WinMVC.generic.IFramework framework;
        public CMachineClass(VirtualMachine.Component.WinMVC.generic.IFramework fw)
        {
            if (framework == null)
                framework = fw;
        }
        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                VMachineClass aVMachineClass = new VMachineClass();
                aVMachineClass.ShowDialog();
            }
            else
            {
                MachineClassExcType theMCET = (MachineClassExcType)args[0];
                switch (theMCET)
                {
                    case MachineClassExcType.search:
                        VMachineClassSerach theVMachineClassSerach = new VMachineClassSerach();
                        theVMachineClassSerach.ShowDialog();
                        return theVMachineClassSerach.Result;
                    default:
                        break;
                }
            }
            return null;

        }

    }
}
