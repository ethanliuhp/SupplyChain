using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Core;
using NHibernate.Exceptions;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using VirtualMachine.Core.DataAccess;
using Application.Business.Erp.SupplyChain.StockManage.Base.Service;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Service;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.CommonClass.Domain;
using NHibernate.Criterion;
using CommonSearchLib.BillCodeMng.Service;
using Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Service;


namespace Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Service
{
    /// <summary>
    /// 转仓单服务
    /// </summary>
    public class StockMoveSrv : BaseService, IStockMoveSrv
    {
        #region Code生成方法
        private IBillCodeRuleSrv billCodeRuleSrv;
        public IBillCodeRuleSrv BillCodeRuleSrv
        {
            get { return billCodeRuleSrv; }
            set { billCodeRuleSrv = value; }
        }
        private IDailyPlanSrv dailyPlanSrv;
        public IDailyPlanSrv DailyPlanSrv
        {
            get { return dailyPlanSrv; }
            set { dailyPlanSrv = value; }
        }
        private string GetCode(Type type)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now);
        }

        private string GetCode(Type type,string special)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now,special);
        }

        private string GetCode(Type type, string projectId, string matCatAbb)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, projectId, matCatAbb);
        }
        #endregion

        #region 调拨入库方法
        public IList GetStockMoveIn(ObjectQuery oq)
        {
            oq.AddFetchMode("Details", FetchMode.Eager);
            oq.AddFetchMode("MaterialCategory", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(StockMoveIn), oq);
        }

        public StockMoveIn GetStockMoveInByCode(string code, string special, string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", code));
            oq.AddCriterion(Expression.Eq("Special", special));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            IList list = GetStockMoveIn(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockMoveIn;
            }
            return null;
        }

        public StockMoveIn GetStockMoveInByCode(string code, string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", code));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            IList list = GetStockMoveIn(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockMoveIn;
            }
            return null;
        }

        public StockMoveIn GetStockMoveInById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = GetStockMoveIn(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockMoveIn;
            }
            return null;
        }

        /// <summary>
        /// 根据Id查询调拨入库单明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StockMoveInDtl GetStockMoveInDtlById(string id)
        {
            return Dao.Get(typeof(StockMoveInDtl), id) as StockMoveInDtl;
        }

        [TransManager]
        public StockMoveIn SaveStockMoveIn(StockMoveIn obj, IList movedDtlList)
        {
            if (obj.Id == null)
            {
                if (obj.Special == "土建")
                {
                    obj.Code = GetCode(typeof(StockMoveIn), obj.ProjectId, (obj.MaterialCategory==null?"":obj.MaterialCategory.Abbreviation));
                }
                else if (obj.Special == "安装")
                {
                    obj.Code = GetCode(typeof(StockMoveIn), obj.ProjectId, obj.ProfessionCategory);
                }
                obj.LastModifyDate = DateTime.Now;
                obj.RealOperationDate = DateTime.Now;
                if (obj.DocState == DocumentState.InExecute || obj.DocState == DocumentState.InAudit)
                {
                    obj.SubmitDate = DateTime.Now;
                }
                obj = SaveByDao(obj) as StockMoveIn;
                //新增时修改前续单据的引用数量
                foreach (StockMoveInDtl dtl in obj.Details)
                {
                    DailyPlanDetail forwardDtl = DailyPlanSrv.GetDailyPlanDetail(dtl.ForwardDetailId);
                 
                    forwardDtl.RefQuantity = forwardDtl.RefQuantity + Math.Abs(dtl.Quantity);//RefQuery为引用数量

                    dao.Save(forwardDtl);
                }
            }
            else
            {
                if (obj.DocState == DocumentState.InExecute || obj.DocState == DocumentState.InAudit)
                {
                    obj.SubmitDate = DateTime.Now;
                }
                obj.LastModifyDate = DateTime.Now;
                obj = SaveOrUpdateByDao(obj) as StockMoveIn;
                foreach (StockMoveInDtl dtl in obj.Details)
                {
                    DailyPlanDetail forwardDtl = DailyPlanSrv.GetDailyPlanDetail(dtl.ForwardDetailId);
                    if (dtl.Id == null)
                    {
                        forwardDtl.RefQuantity = forwardDtl.RefQuantity + Math.Abs(dtl.Quantity) - Math.Abs(Convert.ToDecimal(dtl.TempData));
                    }
                    else
                    {
                        forwardDtl.RefQuantity = forwardDtl.RefQuantity + Math.Abs(dtl.Quantity) - Math.Abs(Convert.ToDecimal(dtl.TempData));
                    }
                    dao.Save(forwardDtl);
                }

                //修改时对于删除的明细 删除引用数量
                foreach (StockMoveInDtl dtl in movedDtlList)
                {
                    DailyPlanDetail forwardDtl = DailyPlanSrv.GetDailyPlanDetail(dtl.ForwardDetailId);
                    forwardDtl.RefQuantity = forwardDtl.RefQuantity - Math.Abs(Convert.ToDecimal(dtl.TempData));
                    dao.Save(forwardDtl);
                }
            }
            return obj;
        }

        [TransManager]
        public StockMoveIn SaveStockMoveIn1(StockMoveIn obj, IList movedDtlList)
        {
            

                obj.Code = GetCode(typeof(StockMoveIn), obj.ProjectId, (obj.MaterialCategory == null ? "商品砼" : obj.MaterialCategory.Abbreviation));


                obj.LastModifyDate = DateTime.Now;
                obj.RealOperationDate = DateTime.Now;
                if (obj.DocState == DocumentState.InExecute || obj.DocState == DocumentState.InAudit)
                {
                    obj.SubmitDate = DateTime.Now;
                }
                obj = SaveByDao(obj) as StockMoveIn;
                //新增时修改前续单据的引用数量
                return obj;
             
        }
        [TransManager]
        public bool DeleteStockMoveIn(StockMoveIn obj)
        {
            if (obj.Id == null) return true;
            //删除明细时 删除引用数量
            foreach (StockMoveInDtl dtl in obj.Details)
            {
                if (dtl.Id != null)
                {
                    DailyPlanDetail forwardDtl = DailyPlanSrv.GetDailyPlanDetail(dtl.ForwardDetailId);
                    forwardDtl.RefQuantity = forwardDtl.RefQuantity - Math.Abs(dtl.Quantity);
                    dao.Save(forwardDtl);
                }
            }
            return dao.Delete(obj);
        }
        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return dao.ObjectQuery(entityType, oq);
        }

        #endregion

        #region 调拨入库红单方法
        /// <summary>
        /// 查询调拨入库红单
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetStockMoveInRed(ObjectQuery oq)
        {
            oq.AddFetchMode("Details", FetchMode.Eager);
            return dao.ObjectQuery(typeof(StockMoveInRed), oq);
        }

        /// <summary>
        /// 根据Code查询调拨入库红单
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public StockMoveInRed GetStockMoveInRedByCode(string code, string special, string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", code));
            oq.AddCriterion(Expression.Eq("Special", special));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            IList list = GetStockMoveInRed(oq);
            if (list != null && list.Count > 0) return list[0] as StockMoveInRed;
            return null;
        }
        public StockMoveInRed GetStockMoveInRedByCode(string code,string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", code));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            IList list = GetStockMoveInRed(oq);
            if (list != null && list.Count > 0) return list[0] as StockMoveInRed;
            return null;
        }

        /// <summary>
        /// 根据Id查询调拨入库红单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StockMoveInRed GetStockMoveInRedById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = GetStockMoveInRed(oq);
            if (list != null && list.Count > 0) return list[0] as StockMoveInRed;
            return null;
        }

        /// <summary>
        /// 根据Id查询日常需求计划明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private DailyPlanDetail GetDailyPlanDetailById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = Dao.ObjectQuery(typeof(DailyPlanDetail), oq);
            if (list != null && list.Count > 0) return list[0] as DailyPlanDetail;
            return null;
        }

        /// <summary>
        /// 保存调拨入库红单
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <returns></returns>
        [TransManager]
        public StockMoveInRed SaveStockMoveInRed(StockMoveInRed obj, IList movedDtlList)
        {
            obj.LastModifyDate = DateTime.Now;
            if (obj.Id == null)
            {
                if (obj.Special == "土建")
                {
                    obj.Code = GetCode(typeof(StockMoveInRed), obj.ProjectId, obj.MaterialCategory.Abbreviation);
                }
                else if (obj.Special == "安装")
                {
                    obj.Code = GetCode(typeof(StockMoveInRed), obj.ProjectId, obj.ProfessionCategory);
                }
                obj.RealOperationDate = DateTime.Now;
                if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
                {
                    obj.SubmitDate = DateTime.Now;
                }
                obj = SaveByDao(obj) as StockMoveInRed;
                //新增时修改前续单据的引用数量
                foreach (StockMoveInRedDtl dtl in obj.Details)
                {
                    StockMoveInDtl forwardDtl = GetStockMoveInDtlById(dtl.ForwardDetailId);
                    forwardDtl.RefQuantity = forwardDtl.RefQuantity + Math.Abs(dtl.Quantity);
                    dao.Save(forwardDtl);

                    //减少日常需求计划的引用数量
                    DailyPlanDetail dailyPlanDetail = GetDailyPlanDetailById(forwardDtl.ForwardDetailId);
                    if (dailyPlanDetail != null)
                    {
                        dailyPlanDetail.RefQuantity = dailyPlanDetail.RefQuantity - Math.Abs(dtl.Quantity);
                        dao.SaveOrUpdate(dailyPlanDetail);
                    }
                }
            } else
            {
                if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
                {
                    obj.SubmitDate = DateTime.Now;
                }
                obj = SaveOrUpdateByDao(obj) as StockMoveInRed;
                foreach (StockMoveInRedDtl dtl in obj.Details)
                {
                    StockMoveInDtl forwardDtl = GetStockMoveInDtlById(dtl.ForwardDetailId);
                    if (dtl.Id == null)
                    {
                        forwardDtl.RefQuantity = forwardDtl.RefQuantity + Math.Abs(dtl.Quantity);
                    } else
                    {
                        //修改时修改前续单据的引用数量为 引用数量+当前冲红数量与上次冲红数量的差值
                        forwardDtl.RefQuantity = forwardDtl.RefQuantity + Math.Abs(dtl.Quantity) - Math.Abs(dtl.QuantityTemp);
                    }
                    dao.Save(forwardDtl);

                    //日常需求计划引用数量处理
                    DailyPlanDetail dailyPlanDetail = GetDailyPlanDetailById(forwardDtl.ForwardDetailId);
                    if (dailyPlanDetail != null)
                    {
                        if (dtl.Id == null)
                        {
                            //新增时减少引用数量
                            dailyPlanDetail.RefQuantity = dailyPlanDetail.RefQuantity - Math.Abs(dtl.Quantity);
                        }
                        else
                        {
                            //修改时减少引用数量
                            dailyPlanDetail.RefQuantity = dailyPlanDetail.RefQuantity - (Math.Abs(dtl.Quantity) - Math.Abs(dtl.QuantityTemp));
                        }
                        dao.SaveOrUpdate(dailyPlanDetail);//保存日常需求计划明细
                    }
                }

                //修改时对于删除的明细 删除引用数量
                foreach (StockMoveInRedDtl dtl in movedDtlList)
                {
                    StockMoveInDtl forwardDtl = GetStockMoveInDtlById(dtl.ForwardDetailId);
                    forwardDtl.RefQuantity = forwardDtl.RefQuantity - Math.Abs(dtl.Quantity);
                    dao.Save(forwardDtl);

                    //增加日常需求计划的引用数量
                    DailyPlanDetail dailyPlanDetail = GetDailyPlanDetailById(forwardDtl.ForwardDetailId);
                    if (dailyPlanDetail != null)
                    {
                        dailyPlanDetail.RefQuantity = dailyPlanDetail.RefQuantity + Math.Abs(dtl.Quantity);
                        dao.SaveOrUpdate(dailyPlanDetail);
                    }
                }
            }

            return obj;
        }

        /// <summary>
        /// 删除调拨入库红单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteStockMoveInRed(StockMoveInRed obj)
        {
            if (obj.Id == null) return true;
            //删除明细时 删除引用数量
            foreach (StockMoveInRedDtl dtl in obj.Details)
            {
                if (dtl.Id != null)
                {
                    StockMoveInDtl forwardDtl = GetStockMoveInDtlById(dtl.ForwardDetailId);
                    forwardDtl.RefQuantity = forwardDtl.RefQuantity - Math.Abs(dtl.Quantity);
                    dao.Save(forwardDtl);

                    //增加日常需求计划的引用数量
                    DailyPlanDetail dailyPlanDetail = GetDailyPlanDetailById(forwardDtl.ForwardDetailId);
                    if (dailyPlanDetail != null)
                    {
                        dailyPlanDetail.RefQuantity = dailyPlanDetail.RefQuantity + Math.Abs(dtl.Quantity);
                        dao.SaveOrUpdate(dailyPlanDetail);
                    }
                }
            }
            return dao.Delete(obj);
        }
        #endregion

        #region 调拨出库方法
        public IList GetStockMoveOut(ObjectQuery oq)
        {
            oq.AddFetchMode("Details", FetchMode.Eager);
            oq.AddFetchMode("MaterialCategory", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(StockMoveOut), oq);
        }

        public StockMoveOut GetStockMoveOutByCode(string code,string special,string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", code));
            oq.AddCriterion(Expression.Eq("Special", special));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            IList list = GetStockMoveOut(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockMoveOut;
            }
            return null;
        }
        public StockMoveOut GetStockMoveOutByCode(string code, string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", code));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            IList list = GetStockMoveOut(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockMoveOut;
            }
            return null;
        }

        public StockMoveOut GetStockMoveOutById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = GetStockMoveOut(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockMoveOut;
            }
            return null;
        }

        [TransManager]
        public StockMoveOut SaveStockMoveOut(StockMoveOut obj)
        {
            if (obj.Id == null)
            {
                if (obj.Special == "土建")
                {
                    obj.Code = GetCode(typeof(StockMoveOut), obj.ProjectId,(obj.MaterialCategory==null?"": obj.MaterialCategory.Abbreviation));
                }
                else if (obj.Special == "安装")
                {
                    obj.Code = GetCode(typeof(StockMoveOut), obj.ProjectId, obj.ProfessionCategory);
                }
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InExecute || obj.DocState == DocumentState.InAudit)
            {
                obj.SubmitDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as StockMoveOut;
        }
        [TransManager]
        public StockMoveOut SaveStockMoveOut1(StockMoveOut obj)
        {
            if (obj.Id == null)
            {
                if (obj.Special == "土建")
                {
                    obj.Code = GetCode(typeof(StockMoveOut), obj.ProjectId, (obj.MaterialCategory == null ? "商品砼" : obj.MaterialCategory.Abbreviation));
                }
                else if (obj.Special == "安装")
                {
                    obj.Code = GetCode(typeof(StockMoveOut), obj.ProjectId, obj.ProfessionCategory);
                }
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InExecute || obj.DocState == DocumentState.InAudit)
            {
                obj.SubmitDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as StockMoveOut;
        }

        public StockMoveOutDtl GetStockMoveOutDtlById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = dao.ObjectQuery(typeof(StockMoveOutDtl), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockMoveOutDtl;
            }
            return null;
        }

        #endregion

        #region 调拨出库红单方法
        public IList GetStockMoveOutRed(ObjectQuery oq)
        {
            oq.AddFetchMode("Details", FetchMode.Eager);
            return Dao.ObjectQuery(typeof(StockMoveOutRed), oq);
        }

        public StockMoveOutRed GetStockMoveOutRedByCode(string code, string special, string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", code));
            oq.AddCriterion(Expression.Eq("Special", special));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            IList list = GetStockMoveOutRed(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockMoveOutRed;
            }
            return null;
        }

        public StockMoveOutRed GetStockMoveOutRedByCode(string code, string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", code));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            IList list = GetStockMoveOutRed(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockMoveOutRed;
            }
            return null;
        }

        public StockMoveOutRed GetStockMoveOutRedById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = GetStockMoveOutRed(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockMoveOutRed;
            }
            return null;
        }

        [TransManager]
        public StockMoveOutRed SaveStockMoveOutRed(StockMoveOutRed obj, IList movedDtlList)
        {
            obj.LastModifyDate = DateTime.Now;
            if (obj.Id == null)
            {
                if (obj.Special == "土建")
                {
                    obj.Code = GetCode(typeof(StockMoveOutRed), obj.ProjectId, obj.MaterialCategory.Abbreviation);
                }
                else if (obj.Special == "安装")
                {
                    obj.Code = GetCode(typeof(StockMoveOutRed), obj.ProjectId, obj.ProfessionCategory);
                }
                obj.RealOperationDate = DateTime.Now;
                if (obj.DocState == DocumentState.InExecute || obj.DocState == DocumentState.InAudit)
                {
                    obj.SubmitDate = DateTime.Now;
                }
                obj = SaveByDao(obj) as StockMoveOutRed;
                //新增时修改前续单据的引用数量
                foreach (StockMoveOutRedDtl dtl in obj.Details)
                {
                    StockMoveOutDtl forwardDtl = GetStockMoveOutDtlById(dtl.ForwardDetailId);
                    forwardDtl.RefQuantity = forwardDtl.RefQuantity + Math.Abs(dtl.Quantity);
                    dao.Save(forwardDtl);
                }
              
            } 
            else
            {
                if (obj.DocState == DocumentState.InExecute || obj.DocState == DocumentState.InAudit)
                {
                    obj.SubmitDate = DateTime.Now;
                }
                obj = SaveOrUpdateByDao(obj) as StockMoveOutRed;
                foreach (StockMoveOutRedDtl dtl in obj.Details)
                {
                    StockMoveOutDtl forwardDtl = GetStockMoveOutDtlById(dtl.ForwardDetailId);
                    if (dtl.Id == null)
                    {
                        forwardDtl.RefQuantity = forwardDtl.RefQuantity + Math.Abs(dtl.Quantity);
                    } else
                    {
                        //修改时修改前续单据的引用数量为 引用数量+当前冲红数量与上次冲红数量的差值
                        forwardDtl.RefQuantity = forwardDtl.RefQuantity + Math.Abs(dtl.Quantity) - Math.Abs(dtl.QuantityTemp);
                    }
                    dao.Save(forwardDtl);
                }

                //修改时对于删除的明细 删除引用数量
                foreach (StockMoveOutRedDtl dtl in movedDtlList)
                {
                    StockMoveOutDtl forwardDtl = GetStockMoveOutDtlById(dtl.ForwardDetailId);
                    forwardDtl.RefQuantity = forwardDtl.RefQuantity - Math.Abs(dtl.Quantity);
                    dao.Save(forwardDtl);
                }
            }
             
            
          

            return obj;
        }

        [TransManager]
        public bool DeleteStockMoveOutRed(StockMoveOutRed obj)
        {
            if (obj.Id == null) return true;
            //删除明细时 删除引用数量
            foreach (StockMoveOutRedDtl dtl in obj.Details)
            {
                if (dtl.Id != null)
                {
                    StockMoveOutDtl forwardDtl = GetStockMoveOutDtlById(dtl.ForwardDetailId);
                    forwardDtl.RefQuantity = forwardDtl.RefQuantity - Math.Abs(dtl.Quantity);
                    dao.Save(forwardDtl);
                }
            }
            return dao.Delete(obj);
        }

        #endregion



        #region 部门查询


        /// <summary>
        /// 部门信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public DataSet DepartQuery(string condition)
        {
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"select * from (select t.projectname as ProjectName from ResConfig t union select t1.orgname as ProjectName from resorganization t1 inner join (select t3.orgid,t3.orgname from RESSUPPLIERRELATION t2 inner join resorganization t3 on t2.ORGID = t3.orgid) t4 on t1.orgid = t4.orgid )";
            sql += " where 1=1 " + condition + "";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }
        #endregion
    }
}
