using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.PersonManagement.Service;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.PersonManagement.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.Service;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Service;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Service;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Service;

namespace Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.PersonManagement
{
    public class MPersonManage
    {
        private IPersonManageSrv personManageSrv;
        public IPersonManageSrv PersonManageSrv
        {
            get { return personManageSrv; }
            set { personManageSrv = value; }
        }
        private static IProductionManagementSrv model;
        private static IStockInSrv stockIn;
        private static IStockMoveSrv stockMove;
        private static IProfessionInspectionSrv proInspect;
        public MPersonManage()
        {
            if (personManageSrv == null)
            {
                personManageSrv = StaticMethod.GetService("PersonManageSrv") as IPersonManageSrv;
            }
            if (model == null)
                model = ConstMethod.GetService("ProductionManagementSrv") as IProductionManagementSrv;
            if (stockIn == null)
                stockIn = ConstMethod.GetService("StockInSrv") as IStockInSrv;
            if (stockMove == null)
                stockMove = ConstMethod.GetService("StockMoveSrv") as IStockMoveSrv;
            if (proInspect == null)
                proInspect = ConstMethod.GetService("ProfessionInspectionSrv") as IProfessionInspectionSrv;
        }
        /// <summary>
        /// 保存管理人员日志信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public PersonManage SavePersonManage(PersonManage obj)
        {
            return personManageSrv.SavePersonManage(obj);
        }
        /// <summary>
        /// 查询周计划明细信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList GetWeekDetail(ObjectQuery obj)
        {
            return model.GetWeekDetail(obj);
        }
        /// <summary>
        /// 查询收料入库信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList GetStockIn(ObjectQuery obj)
        {
            return stockIn.GetStockIn(obj);
        }
        /// <summary>
        /// 查询调拨入库信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList GetStockMoveIn(ObjectQuery obj)
        {
            return stockMove.GetStockMoveIn(obj);
        }
        /// <summary>
        /// 查询调拨出库信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList GetStockMoveOut(ObjectQuery obj)
        {
            return stockMove.GetStockMoveOut(obj);
        }
        /// <summary>
        /// 查询日常检查记录信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList GetInspectionRecord(ObjectQuery obj)
        {
            return model.GetInspectionRecord(obj);
        }
        /// <summary>
        /// 查询专业检查记录信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList GetProfessionInspectionRecordPlan(ObjectQuery obj)
        {
            return proInspect.GetProfessionInspectionRecordPlan(obj);
        }
    }
}
