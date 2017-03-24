using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.OBS.Service
{
   

    /// <summary>
    /// OBS服务
    /// </summary>
    public interface IOBSSrv : IBaseService
    {
        IDao Dao { get; set; }

        #region OBSService服务
        IList GetOBSServiceByName(string Name);
        OBSService GetOBSServiceById(string id);
        IList GetOBSService(ObjectQuery objectQuery);

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
        #endregion


    }




}
