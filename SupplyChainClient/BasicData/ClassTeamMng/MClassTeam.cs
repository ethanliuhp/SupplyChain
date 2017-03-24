using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using System.Collections;
using Application.Resource.BasicData.Service;
using Application.Resource.BasicData.Domain;


namespace Application.Business.Erp.SupplyChain.Client.BasicData.ClassTeamMng
{
    public class MClassTeam
    {
        private static IClassTeamSrv ClassTeamSrv = null;
        public MClassTeam()
        {
            if (ClassTeamSrv == null)
            {
                ClassTeamSrv = StaticMethod.GetService("ClassTeamSrv") as IClassTeamSrv;//
            }

        }


        public ClassTeam Save(ClassTeam obj)
        {
            if (obj.Id == "")
                return ClassTeamSrv.Save(obj) as ClassTeam;//类型转换
            else
                return ClassTeamSrv.Update(obj) as ClassTeam;//类型转换
        }

        public bool Delete(ClassTeam obj)
        {
            return ClassTeamSrv.Delete(obj);
        }

        public ClassTeam Update(ClassTeam theSQ)
        {
            return ClassTeamSrv.Update(theSQ) as ClassTeam;
        }
        public IList GetClassTeam()//
        {
            return ClassTeamSrv.GetObjects(typeof(ClassTeam));
        }

        public IList GetClassTeam(ObjectQuery oq)
        {
            return ClassTeamSrv.GetObjects(typeof(ClassTeam), oq);
        }



    }
}
