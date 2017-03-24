using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.ConcreteManage.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.ConcreteManage
{
    public class MConcreteMng
    {
        private IConcreteManSrv concreteMngSrv;

        public IConcreteManSrv ConcreteMngSrv
        {
            get { return concreteMngSrv; }
            set { concreteMngSrv = value; }
        }

        public MConcreteMng()
        {
            if (concreteMngSrv == null)
            {
                concreteMngSrv = StaticMethod.GetService("ConcreteMngSrv") as IConcreteManSrv;
            }
        }
    }
}
