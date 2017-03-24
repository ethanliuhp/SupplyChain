using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectCopyMng.Service;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectCopyMng
{
    public class MProjectCopy
    {
        private IProjectCopySrv cprojectCopySrv;
        public IProjectCopySrv ProjectCopySrv
        {
            get { return cprojectCopySrv; }
            set { cprojectCopySrv = value; }
        }

         public MProjectCopy()
        {
            if (cprojectCopySrv == null)
                cprojectCopySrv = StaticMethod.GetService("ProjectCopySrv") as IProjectCopySrv;
        }
    }
}
