using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.OBS.Service
{
   

    /// <summary>
    /// OBS服务
    /// </summary>
    public interface IOBSSrv : IBaseService
    {

        #region OBSService服务
        IList GetOperationOrgs(ObjectQuery oq);
        IList GetOBSServiceByName(string Name);
        OBSService GetOBSServiceById(string id);
        IList GetOBSService(ObjectQuery objectQuery);
         /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [TransManager]
        bool DeleteService(IList list);
        [TransManager]
        OBSService SaveOBSService(OBSService obj);


        IList ObjectQuery(Type entityType, ObjectQuery oq);
        #endregion

        #region OBSManage管理
        OBSManage GetOBSManageById(string id);
        IList GetOBSManage(ObjectQuery objectQuery);
        IList GetOBSManageByName(string Name);
        IList GetOBSManageByProjectTaskId(string Id);
        [TransManager]
        OBSManage SaveOBSManage(OBSManage obj);
        [TransManager]
        void SaveOBSManages(IList list, OperationOrg oprOrg, Hashtable ht);
        OBSPerson SaveOBSPerson(OBSPerson obj);
        IList GetOBSPerson(ObjectQuery objectQuery);
        [TransManager]
        void OBSManagSave(IList lists);
        /// <summary>
        /// 岗位角色信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetJobRole(ObjectQuery objectQuery);
         /// <summary>
        /// 用户岗位信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetPersonjob(ObjectQuery objectQuery);
        #endregion
        void SavePersons(IList list, OperationOrg oprOrg, Hashtable ht);
        //void SaveManags(IList list, OperationOrg oprOrg, Hashtable ht);
        IList GetOperationOrg(ObjectQuery objectQuery);
    }

}
