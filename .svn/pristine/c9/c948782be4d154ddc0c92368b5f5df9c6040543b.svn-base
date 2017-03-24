using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using CommonSearchLib.BillCodeMng.Service;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Service;
using Application.Business.Erp.SupplyChain.StockManage.Base.Service;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Service;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Service;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Service;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Basic.Service;
using Application.Business.Erp.SupplyChain.ExcelImportMng.Service;
using Application.Business.Erp.SupplyChain.SupplyManage.Service;

namespace Application.Business.Erp.SupplyChain.Client.StockMng
{
    public enum EnumStockExecType
    {
        /// <summary>
        /// 入库单统计查询
        /// </summary>
        stateSearch,
        /// <summary>
        /// 基础数据维护
        /// </summary>
        basicDataOptr,
        /// <summary>
        /// 日志查询
        /// </summary>
        logDataQuery,
        /// <summary>
        /// 日志统计
        /// </summary>
        logStatReport,
        /// <summary>
        /// 调拨入库单查询
        /// </summary>
        StockMoveInQuery,
        /// <summary>
        /// 领料出库查询
        /// </summary>
        StockOutQuery,
        /// <summary>
        /// 调拨出库查询
        /// </summary>
        StockMoveOutQuery,
        /// <summary>
        /// 验收结算单查询
        /// </summary>
        StockInBalQuery,
        /// <summary>
        /// 库存查询
        /// </summary>
        StockRelationQuery,
        /// <summary>
        /// 仓库收发台账
        /// </summary>
        仓库收发台账,
        /// <summary>
        /// 仓库收发存月报
        /// </summary>
        仓库收发存月报,
        /// <summary>
        /// 闲置物资维护
        /// </summary>
        闲置物资维护,
        /// <summary>
        /// 公司调配查询
        /// </summary>
        公司调配查询,
        /// <summary>
        /// 土建 单据
        /// </summary>
        土建,
        /// <summary>
        /// 安装 单据
        /// </summary>
        安装,
        仓库收发存报表,
        料具租赁,
        成本对比分析表,
        单据修改,
        辅材数据比例统计,

    }
    
    public class MStockMng
    {
        private static IStockInSrv theStockInSrv;
        private static IStockMoveSrv stockMoveSrv;
        private static IStockOutSrv stockOutSrv;
        private static IStockRelationSrv stockRelationSrv;
        private static IStockInOutSrv stockInOutSrv;
        private static ICurrentProjectSrv currentProjectSrv;
        private static IExcelImportSrv excelImportSrv;
        private static ISupplyOrderSrv supplyOrderSrv;
        public MStockMng()
        {
        }

        #region 初始化服务
        public IStockMoveSrv StockMoveSrv
        {
            get 
            {
                if (stockMoveSrv == null) stockMoveSrv = StaticMethod.GetService("StockMoveSrv") as IStockMoveSrv;
                return stockMoveSrv; 
            }
            set { stockMoveSrv = value; }
        }
        public ISupplyOrderSrv SupplyOrderSrv
        {
            get
            {
                if (supplyOrderSrv == null) supplyOrderSrv = StaticMethod.GetService("SupplyOrderSrv") as ISupplyOrderSrv;
                return supplyOrderSrv;
            }
            set { supplyOrderSrv = value; }
        }

        public ICurrentProjectSrv CurrentProjectSrv
        {
            get
            {
                if (currentProjectSrv == null) currentProjectSrv = StaticMethod.GetService("CurrentProjectSrv") as ICurrentProjectSrv;
                return currentProjectSrv;
            }
            set { currentProjectSrv = value; }
        }


        public IStockInSrv StockInSrv
        {
            get 
            {
                if (theStockInSrv == null) theStockInSrv = StaticMethod.GetService("StockInSrv") as IStockInSrv;
                return theStockInSrv; 
            }
            set { theStockInSrv = value; }
        }

        public IStockOutSrv StockOutSrv
        {
            get 
            {
                if (stockOutSrv == null) stockOutSrv = StaticMethod.GetService("StockOutSrv") as IStockOutSrv;
                return stockOutSrv;
            }
            set { stockOutSrv = value; }
        }

        public IStockRelationSrv StockRelationSrv
        {
            get 
            {
                if (stockRelationSrv == null) stockRelationSrv = StaticMethod.GetService("StockRelationSrv") as IStockRelationSrv;
                return stockRelationSrv;
            }
            set { stockRelationSrv = value; }
        }

        public IStockInOutSrv StockInOutSrv
        {
            get 
            {
                if (stockInOutSrv == null) stockInOutSrv = StaticMethod.GetService("StockInOutSrv") as IStockInOutSrv;
                return stockInOutSrv;
            }
            set { stockInOutSrv = value; }
        }
        public IExcelImportSrv ExcelImportSrv
        {
            get
            {
                if (excelImportSrv == null) excelImportSrv = StaticMethod.GetService("ExcelImportSrv") as IExcelImportSrv;
                return excelImportSrv;
            }
            set { excelImportSrv = value; }
        }
        #endregion

        /// <summary>
        /// 保存调拨入库单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public StockMoveIn SaveStockMoveIn(StockMoveIn obj, IList movedDtlList)
        {
            return StockMoveSrv.SaveStockMoveIn(obj, movedDtlList);
        }

        /// <summary>
        /// 入库记账
        /// </summary>
        /// <param name="hashBillId"></param>
        /// <param name="hashBillCode"></param>
        /// <returns></returns>
        public Hashtable TallyStockIn(Hashtable hashBillId,Hashtable hashBillCode)
        {
            return StockInSrv.TallyStockIn(hashBillId, hashBillCode, ConstObject.TheLogin.TheComponentPeriod.NowYear, ConstObject.TheLogin.TheComponentPeriod.NowMonth, ConstObject.TheLogin.ThePerson.Id, ConstObject.TheLogin.ThePerson.Name, StaticMethod.GetProjectInfo().Id);
        }

        /// <summary>
        /// 出库记账
        /// </summary>
        /// <param name="hashBillId"></param>
        /// <param name="hashBillCode"></param>
        /// <returns></returns>
        public Hashtable TallyStockOut(Hashtable hashBillId, Hashtable hashBillCode)
        {
            return StockOutSrv.TallyStockOut(hashBillId, hashBillCode, ConstObject.TheLogin.TheComponentPeriod.NowYear, ConstObject.TheLogin.TheComponentPeriod.NowMonth, ConstObject.TheLogin.ThePerson.Id, ConstObject.TheLogin.ThePerson.Name, StaticMethod.GetProjectInfo().Id);
        }


        private static IList CategoryNotSum;
        /// <summary>
        /// 验收结算单不需汇总的分类
        /// </summary>
        /// <returns></returns>
        public IList GetCategoryNotSum()
        {
            if (CategoryNotSum != null && CategoryNotSum.Count > 0) return CategoryNotSum;
            CategoryNotSum = new ArrayList();
            IList basicCatLst=StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_StockBalNoSumCategory);
            if (basicCatLst != null && basicCatLst.Count > 0)
            {
                ObjectQuery oq=new ObjectQuery();
                Disjunction dis = Expression.Disjunction();
                foreach (BasicDataOptr bdo in basicCatLst)
                {
                    dis.Add(Expression.Eq("Code", bdo.BasicCode));    
                }
                oq.AddCriterion(dis);
                CategoryNotSum=StockInSrv.GetObjects(typeof(MaterialCategory), oq);
            }
            return CategoryNotSum;
        }
    }
}
