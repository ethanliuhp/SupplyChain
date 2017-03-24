using System;
using System.Collections.Generic;
using System.Text;
using Application.Resource.BasicData.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

using System.Collections;
using VirtualMachine.Core;
using Application.Resource.BasicData.Domain;

namespace Application.Business.Erp.SupplyChain.Client.BasicData.MachineClassMng
{
    public class MMachineClass
    {
        private static IMachineClassSrv MachineClassSrv = null;
        public MMachineClass()
        {
            if (MachineClassSrv == null)
            {
                MachineClassSrv = StaticMethod.GetService("MachineClassSrv") as IMachineClassSrv;
            }

        }
        public MachineClass Save(MachineClass obj)
        {
            if (obj.Id == "")
                return MachineClassSrv.Save(obj) as MachineClass;//类型转换
            else
                return MachineClassSrv.Update(obj) as MachineClass;//类型转换

        }

        public bool Delete(MachineClass obj)
        {
            return MachineClassSrv.Delete(obj);
        }

        public MachineClass Update(MachineClass theMC)
        {
            return MachineClassSrv.Update(theMC) as MachineClass;
        }
        public IList GetMachineClass()
        {
            return MachineClassSrv.GetObjects(typeof(MachineClass));
        }

        public IList GetMachineClass(ObjectQuery oq)
        {
            return MachineClassSrv.GetObjects(typeof(MachineClass), oq);
        }


    }
}
