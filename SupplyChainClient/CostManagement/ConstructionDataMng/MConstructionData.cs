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
using System.Data;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.CostManagement.ConstructionDataMng.Service;
using Application.Business.Erp.SupplyChain.CostManagement.ConstructionDataMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ConstructionDataMng
{
    public class MConstructionData
    {
        private IConstructionDataSrv constructionDataSrv;

        public IConstructionDataSrv ConstructionDataSrv
        {
            get { return constructionDataSrv; }
            set { constructionDataSrv = value; }
        }

        public MConstructionData()
        {
            if (constructionDataSrv == null)
            {
                constructionDataSrv = StaticMethod.GetService("ConstructionDataSrv") as IConstructionDataSrv;
            }
        }

        #region 施工专业基础数据
        /// <summary>
        /// 保存施工专业基础数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ConstructionData SaveConstructionData(ConstructionData obj)
        {
            return constructionDataSrv.SaveConstructionData(obj);
        }

        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(ObjectQuery oq)
        {
            return constructionDataSrv.GetConstructionData(oq);
        }
        #endregion
    }
}
