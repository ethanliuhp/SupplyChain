using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.CostManagement.EngineerChangeMng.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.Service;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.EngineerChangeMng
{
    public class MEngineerChangeMng
    {
        private IEngineerChangeSrv engineerChangeSrv;
        public IEngineerChangeSrv EngineerChangeSrv
        {
            get { return engineerChangeSrv; }
            set { engineerChangeSrv = value; }
        }
        public MEngineerChangeMng()
        {
            if (engineerChangeSrv == null)
            {
                engineerChangeSrv = StaticMethod.GetService("EngineerChangeSrv") as IEngineerChangeSrv;
            }
        }
        /// <summary>
        /// 保存工程更改管理信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public EngineerChangeMaster SaveEngineerChange(EngineerChangeMaster obj)
        {
            return engineerChangeSrv.SaveEngineerChange(obj);
        }

        /// <summary>
        /// 保存废旧物资处理信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        //public WasteMatProcessMaster saveWasteMatHandle(WasteMatProcessMaster obj, IList movedDtlList)
        //{
        //    return wasteMatSrv.saveWasteMatProcess(obj,movedDtlList);
        //}

    }
}
