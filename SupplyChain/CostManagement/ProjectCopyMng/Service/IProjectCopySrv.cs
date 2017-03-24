using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.ProjectCopyMng.Service
{
    /// <summary>
    /// 工程项目复制
    /// </summary>
    public interface IProjectCopySrv : IBaseService
    {
        #region 工程项目复制
        [TransManager]
        void DeleteCopy(CurrentProjectInfo ProjectInfoLeft, CurrentProjectInfo ProjectInfoRight, IList listPBS, IList listWBS);
        [TransManager]
        void SaveCopy(CurrentProjectInfo ProjectInfoLeft, CurrentProjectInfo ProjectInfoRight, IList listPBS, IList listWBS);
        
        [TransManager]
        PBSTree SavePBSTree(PBSTree obj, CurrentProjectInfo ProjectInfoRight);

        #endregion
        
    }




}
