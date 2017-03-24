using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.BasicData.Domain;
using Application.Resource.BasicData.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using System.Collections;
using Application.Resource.MaterialResource.Service;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.Client.BasicData.ClassesMng
{
    class MClasses
    {
        private static IClassesSrv ClassesSrv = null;
        private static IMaterialService MaterialService = null;
        private static IManageStateService ManageStateService = null;
        private static IPracticalityStateService PracticalityStateService = null;
        public MClasses()
        {
            if (ClassesSrv == null)
            {
                ClassesSrv = StaticMethod.GetService("ClassesSrv") as IClassesSrv;
            }
            if (MaterialService == null)
            {
                MaterialService = StaticMethod.GetService("MaterialService") as IMaterialService;
            }
            if (ManageStateService == null)
            {
                ManageStateService = StaticMethod.GetService("ManageStateService") as IManageStateService;
            }
            if (PracticalityStateService == null)
            {
                PracticalityStateService = StaticMethod.GetService("PracticalityStateService") as IPracticalityStateService;
            }
        }


        public Classes Save(Classes obj)
        {
            if (obj.Id == "")
                return ClassesSrv.Save(obj) as Classes;
            else
                return ClassesSrv.Update(obj) as Classes;
        }

        public bool Delete(Classes obj)
        {
            //IList lst = ClassesSrv.GetObjects(typeof(PracticalityState));
            //ClassesSrv.Delete(lst);

            //lst = ClassesSrv.GetObjects(typeof(ManageState));
            //ClassesSrv.Delete(lst);

            //lst = ClassesSrv.GetObjects(typeof(Material));
            //ClassesSrv.Delete(lst);

            return ClassesSrv.Delete(obj);
        }

        public Classes Update(Classes theSQ)
        {
            return ClassesSrv.Update(theSQ) as Classes;
        }
        public IList GetClasses()
        {
            return ClassesSrv.GetObjects(typeof(Classes));
        }

        public IList GetClasses(ObjectQuery oq)
        {
            return ClassesSrv.GetObjects(typeof(Classes), oq);
        }


    }
}
