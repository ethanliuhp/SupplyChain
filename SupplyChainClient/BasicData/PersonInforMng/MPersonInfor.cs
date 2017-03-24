using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.CorporateResource.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Data;
using Application.Business.Erp.SupplyChain.CorporateResource.Domain;

namespace Application.Business.Erp.SupplyChain.Client.BasicData.PersonInforMng
{
    public class MPersonInfor
    {
        private static ICorporateResourceSrv CorporateResourceSrv = null;
        public MPersonInfor()
        {
            if (CorporateResourceSrv == null)
            {
                CorporateResourceSrv = StaticMethod.GetService("CorporateResourceSrv") as ICorporateResourceSrv;
            }
        }

        /// <summary>
        /// 统计公司部门信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetCorporateBantch()
        {
            return CorporateResourceSrv.GetCorporateBantch();
        }
        /// <summary>
        /// 根据部门Id获取给人员信息
        /// </summary>
        /// <param name="perCode"></param>
        /// <returns></returns>
        public DataTable GetPersonInfor(long OrgId)
        {
            return CorporateResourceSrv.GetPersonInforByOrgId(OrgId);
        }
        /// <summary>
        /// 根据OrgId获取公司部门信息
        /// </summary>
        /// <param name="OrgId"></param>
        /// <returns></returns>
        public CorporateBantch GetCorporateBantchById(long OrgId)
        {
            return CorporateResourceSrv.GetCorporateBantchById(OrgId);
        }
        /// <summary>
        /// 根据人员编码获取人员信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetPersonInforByCode(string perCode)
        {
            return CorporateResourceSrv.GetPersonInforByCode(perCode);
        }
    }
}
