using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.CostManagement.ConstructionDataMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.ConstructionDataMng.Service
{
   

    /// <summary>
    /// OBS服务
    /// </summary>
    public interface IConstructionDataSrv : IBaseService
    {
        IDao Dao { get; set; }

        #region 施工专业基础数据
        IList GetConstructionData(ObjectQuery objectQuery);
        IList GetConstructionDataBySerailNum(int SerailNum);
        ConstructionData GetConstructionDateById(string id);
        [TransManager]
        ConstructionData SaveConstructionData(ConstructionData obj);
        #endregion


    }




}
