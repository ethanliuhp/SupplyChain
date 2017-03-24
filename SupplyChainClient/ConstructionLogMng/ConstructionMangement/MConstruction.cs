using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.ConstructionManagement.Service;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.ConstructionManagement.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.ConstructionManagement
{
    public class MConstruction
    {
        private IConstructionSrv constructionSrv;
        public IConstructionSrv ConstructionSrv
        {
            get { return constructionSrv; }
            set { constructionSrv = value; }
        }
        public MConstruction()
        {
            if (constructionSrv == null)
            {
                constructionSrv = StaticMethod.GetService("ConstructionSrv") as IConstructionSrv;
            }
        }
        /// <summary>
        /// 保存施工日志信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ConstructionManage SaveConstruction(ConstructionManage obj)
        {
            return constructionSrv.SaveConstruction(obj);
        }
    }
}
