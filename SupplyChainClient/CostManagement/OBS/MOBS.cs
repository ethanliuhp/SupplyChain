using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

using VirtualMachine.Core;
using NHibernate.Criterion;

using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.Service;
using Application.Resource.PersonAndOrganization.OrganizationResource.Service;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.FinancialResource;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Service;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using System.Data;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Service;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Service;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.OBS
{
    public class MOBS
    {
        private IOBSSrv obsSrv;

        public IOBSSrv OBSSrv
        {
            get { return obsSrv; }
            set { obsSrv = value; }
        }
        private IContractExcuteSrv contractExcuteSrv;

        public IContractExcuteSrv ContractExcuteSrv
        {
            get { return contractExcuteSrv; }
            set { contractExcuteSrv = value; }
        }
        public MOBS()
        {
            if (obsSrv == null)
                obsSrv = StaticMethod.GetService("OBSSrv") as IOBSSrv;

            if (contractExcuteSrv == null)
                contractExcuteSrv = StaticMethod.GetService("ContractExcuteSrv") as IContractExcuteSrv;
            
        }

        #region OBS服务
        /// <summary>
        /// 保存服务OBS
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public OBSService SaveOBSService(OBSService obj)
        {
            return obsSrv.SaveOBSService(obj);
        }

        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return obsSrv.ObjectQuery(entityType, oq);
        }
        #endregion
        #region OBS管理


        /// <summary>
        /// 保存OBSManage管理负责人
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public OBSManage SaveOBSManage(OBSManage obj)
        {
            return obsSrv.SaveOBSManage(obj);
        }
        #endregion
    }
}
