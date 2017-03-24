using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.ConstructionManagement.Service;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Basic.Service;

namespace Application.Business.Erp.SupplyChain.Client.Basic
{
    public class MProgramManage
    {
        private ICurrentProjectSrv currentProjectSrv;
        public ICurrentProjectSrv CurrentProjectSrv
        {
            get { return currentProjectSrv; }
            set { currentProjectSrv = value; }
        }
        public MProgramManage()
        {
            if (currentProjectSrv == null)
            {
                currentProjectSrv = StaticMethod.GetService("CurrentProjectSrv") as ICurrentProjectSrv;
            }
        }
        /// <summary>
        /// 保存物资价信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MaterialInterfacePrice SaveMaterialPrice(MaterialInterfacePrice obj)
        {
            return currentProjectSrv.SaveMaterialPrice(obj);
        }
        /// <summary>
        /// 保存项目降低率信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ProgramReduceRate SaveRate(ProgramReduceRate obj)
        {
            return currentProjectSrv.SaveProgramRate(obj);
        }
    }
}
