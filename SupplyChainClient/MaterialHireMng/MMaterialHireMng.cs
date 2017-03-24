using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.MaterialHireMng.Service;
using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Service;
using Application.Resource.MaterialResource.Service;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng
{
    public enum EnumMatHireStockExecType
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
    public class MMaterialHireMng
    {
        private static  IMaterialService materialService;
        public static IMaterialService MaterialService
        {
            get
            {
                if (materialService == null)
                {
                    materialService = StaticMethod.GetService("MaterialService") as IMaterialService;
                }
                return materialService;
            }

        }
        private static IMaterialHireMngSvr materialHireMngSvr;
        private static IMaterialHireStockInSrv materialHireStockInSrv;
        private static IMaterialHireStockOutSrv materialHireStockOutSrv;
        private static IMaterialHireSupplyOrderSrv materialHireSupplyOrderSrv;
        private static ICurrentProjectSrv currentProjectSrv;
        public ICurrentProjectSrv CurrentProjectSrv
        {
            get
            {
                if (currentProjectSrv == null)
                {
                    currentProjectSrv = StaticMethod.GetService("CurrentProjectSrv") as ICurrentProjectSrv;
                }
                return currentProjectSrv;
            }
        }
        /// <summary>
        /// 获取料具管理服务
        /// </summary>
        public IMaterialHireMngSvr MaterialHireMngSvr
        {
            get
            {
                if (materialHireMngSvr == null)
                {
                    materialHireMngSvr = StaticMethod.GetService("MaterialHireMngSvr") as IMaterialHireMngSvr;
                }
                return materialHireMngSvr;
            }
        }

        /// <summary>
        /// 获取料具入库单
        /// </summary>
        public IMaterialHireStockInSrv MaterialHireStockInSrv
        {
            get
            {
                if (materialHireStockInSrv == null)
                {
                    materialHireStockInSrv = StaticMethod.GetService("MaterialHireStockInSrv") as IMaterialHireStockInSrv;
                }
                return materialHireStockInSrv;
            }
        }
        /// <summary>
        /// 获取料具出库单
        /// </summary>
        public IMaterialHireStockOutSrv MaterialHireStockOutSrv
        {
            get
            {
                if (materialHireStockOutSrv == null)
                {
                    materialHireStockOutSrv = StaticMethod.GetService("MaterialHireStockOutSrv") as IMaterialHireStockOutSrv;
                }
                return materialHireStockOutSrv;
            }
        }

        /// <summary>
        /// 获取料具封存单服务
        /// </summary>
        public IMaterialHireSupplyOrderSrv MaterialHireSupplyOrderSrv
        {
            get
            {
                if (materialHireSupplyOrderSrv == null)
                {
                    materialHireSupplyOrderSrv = StaticMethod.GetService("MaterialHireSupplyOrderSrv") as IMaterialHireSupplyOrderSrv;
                }
                return materialHireSupplyOrderSrv;
            }
        }


        ///// <summary>
        ///// 保存调拨入库单
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public StockMoveIn SaveStockMoveIn(StockMoveIn obj, IList movedDtlList)
        //{
        //    return StockMoveSrv.SaveStockMoveIn(obj, movedDtlList);
        //}

        /// <summary>
        /// 入库记账
        /// </summary>
        /// <param name="hashBillId"></param>
        /// <param name="hashBillCode"></param>
        /// <returns></returns>
        public Hashtable TallyStockIn(Hashtable hashBillId, Hashtable hashBillCode)
        {
            return MaterialHireStockInSrv.TallyStockIn(hashBillId, hashBillCode, ConstObject.TheLogin.TheComponentPeriod.NowYear, ConstObject.TheLogin.TheComponentPeriod.NowMonth, ConstObject.TheLogin.ThePerson.Id, ConstObject.TheLogin.ThePerson.Name, StaticMethod.GetProjectInfo().Id);
        }

        /// <summary>
        /// 出库记账
        /// </summary>
        /// <param name="hashBillId"></param>
        /// <param name="hashBillCode"></param>
        /// <returns></returns>
        public Hashtable TallyStockOut(Hashtable hashBillId, Hashtable hashBillCode)
        {
            return MaterialHireStockOutSrv.TallyStockOut(hashBillId, hashBillCode, ConstObject.TheLogin.TheComponentPeriod.NowYear, ConstObject.TheLogin.TheComponentPeriod.NowMonth, ConstObject.TheLogin.ThePerson.Id, ConstObject.TheLogin.ThePerson.Name, StaticMethod.GetProjectInfo().Id);
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
            IList basicCatLst = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_StockBalNoSumCategory);
            if (basicCatLst != null && basicCatLst.Count > 0)
            {
                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = Expression.Disjunction();
                foreach (BasicDataOptr bdo in basicCatLst)
                {
                    dis.Add(Expression.Eq("Code", bdo.BasicCode));
                }
                oq.AddCriterion(dis);
                CategoryNotSum = MaterialHireStockInSrv.GetObjects(typeof(MaterialCategory), oq);
            }
            return CategoryNotSum;
        }
    }
}
