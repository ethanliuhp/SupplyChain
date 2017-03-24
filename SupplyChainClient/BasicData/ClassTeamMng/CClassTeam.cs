using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Business.Erp.SupplyChain.Client.BasicData.ClassTeamMng
{
    public enum ClassTeamExcType
    {
        search
    }
  public class CClassTeam
    {
        private static VirtualMachine.Component.WinMVC.generic.IFramework framework;
        public CClassTeam(VirtualMachine.Component.WinMVC.generic.IFramework fw)
        {
            if (framework == null)
                framework = fw;
        }
        public object Excute(params object[] args)
        {


            if (args.Length == 0)
            {
                VClassTeam aVClassTeam = new VClassTeam();
                aVClassTeam.ShowDialog();
            }
            else
            {
                ClassTeamExcType theCTET = (ClassTeamExcType)args[0];
                switch (theCTET)
                {
                    case ClassTeamExcType.search:
                        VClassTeamSerach theVClassTeamSerach = new VClassTeamSerach();
                        theVClassTeamSerach.ShowDialog();
                        return theVClassTeamSerach.Result;
                    default:
                        break;
                }
            }
            return null;

        }


    }
}
