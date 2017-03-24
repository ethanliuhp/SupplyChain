using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborSporadicManagement.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data.SqlClient;
using VirtualMachine.Core.DataAccess;
using CommonSearchLib.BillCodeMng.Service;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.PenaltyDeductionManagement.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.Util;
using System.Data.OracleClient;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.Service
{
    /// <summary>
    /// 零星用工派工服务
    /// </summary>
    public class LaborSporadicSrv : BaseService, ILaborSporadicSrv
    {
        #region Code生成方法
        private IBillCodeRuleSrv billCodeRuleSrv;
        public IBillCodeRuleSrv BillCodeRuleSrv
        {
            get { return billCodeRuleSrv; }
            set { billCodeRuleSrv = value; }
        }

        private string GetCode(Type type)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now);
        }
        /// <summary>
        /// 根据项目生成Code
        /// </summary>
        /// <param name="type"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        private string GetCode(Type type, string projectId)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, projectId);
        }
        #endregion

        #region 零星用工派工
        /// <summary>
        /// 通过ID查询零星用工派工信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LaborSporadicMaster GetLaborSporadicById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            //objectQuery.AddFetchMode("Details.ProjectTast", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("HandlePerson", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.ProjectTast", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.ProjectTastDetail", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.LaborSubject", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.InsteadTeam", NHibernate.FetchMode.Eager);

            IList list = GetLaborSporadic(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as LaborSporadicMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询零星用工派工信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public LaborSporadicMaster GetLaborSporadicByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.ProjectTast", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.ProjectTastDetail", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.LaborSubject", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.InsteadTeam", NHibernate.FetchMode.Eager);
            IList list = GetLaborSporadic(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as LaborSporadicMaster;
            }
            return null;
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

        /// <summary>
        /// 零星用工派工查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetLaborSporadic(ObjectQuery objectQuery)
        {
            //objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("Details.ProjectTast", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("Details.ProjectTastDetail", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("Details.LaborSubject", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("Details.InsteadTeam", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(LaborSporadicMaster), objectQuery);
        }

        /// <summary>
        /// 零星用工派工查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet LaborSporadicQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.Code,t1.BearTeam,t1.CreateDate,t1.CreatePersonName,t1.BearTeamName,t2.ProjectTastName,t2.LaborDescript,t1.State,t2.balancedtlguid,
                t2.ProjectTastDetailName,t2.LaborSubjectName,t2.PredictLaborNum,t2.RealLaborNum,t2.AccountLaborNum,t2.PriceUnitName,
                t2.StartDate,t2.EndDate,t2.RealOperationDate,t2.AccountPrice,t2.AccountSumMoney,t2.QuantityUnitName,t2.TastDetailName,t1.LaborState,t2.InsteadTeamName,t2.IsCreate
                FROM THD_LaborSporadicMaster t1 INNER JOIN THD_LaborSporadicDetail t2 ON t1.Id = t2.ParentId";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        [TransManager]
        public LaborSporadicMaster SaveLaborSporadic(LaborSporadicMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(LaborSporadicMaster), obj.ProjectId);
                if (obj.LaborState == "逐日派工")
                {
                    obj.Code = obj.Code.Replace("零星用工", "逐日派工");
                }
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as LaborSporadicMaster;

        }

        [TransManager]
        public LaborSporadicMaster SaveLaborSporadic(LaborSporadicMaster obj, List<LaborSporadicDetail> lstRemoveDetail)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = Expression.Disjunction();
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(LaborSporadicMaster), obj.ProjectId);
                if (obj.LaborState == "逐日派工")
                {
                    obj.Code = obj.Code.Replace("零星用工", "逐日派工");
                }
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;

            var master = obj as LaborSporadicMaster;
            if (master != null && master.Details != null && master.Details.Count != 0)
            {
                foreach (LaborSporadicDetail item in master.Details)
                {
                    if (!string.IsNullOrEmpty(item.ForwardDetailId))
                    {
                        dis.Add(Expression.Eq("Id", item.ForwardDetailId));
                    }
                }
                oq.AddCriterion(dis);
                var lstDetail = dao.ObjectQuery(typeof(LaborSporadicDetail), oq).OfType<LaborSporadicDetail>().ToList();
                foreach (LaborSporadicDetail detail in lstDetail)
                {
                    detail.RefQuantity = detail.PredictLaborNum;
                }
                dao.Update(lstDetail);
            }
            obj = SaveOrUpdateByDao(obj) as LaborSporadicMaster;

            //切换逐日派工单时，删除掉之前详细
            oq = new ObjectQuery();
            dis = Expression.Disjunction();
            if (lstRemoveDetail != null && lstRemoveDetail.Count != 0)
            {
                foreach (LaborSporadicDetail item in lstRemoveDetail)
                {
                    if (!string.IsNullOrEmpty(item.ForwardDetailId))
                    {
                        dis.Add(Expression.Eq("Id", item.ForwardDetailId));
                    }
                }
                oq.AddCriterion(dis);
                var lstDetail = dao.ObjectQuery(typeof(LaborSporadicDetail), oq).OfType<LaborSporadicDetail>().ToList();

                if (lstDetail == null || lstDetail.Count == 0)
                {
                    foreach (var detail in lstDetail)
                    {
                        detail.RefQuantity = 0;
                    }
                    dao.Update(lstDetail);
                    dao.Delete(lstRemoveDetail);
                }
            }

            return obj;
        }
        public LaborSporadicMaster SaveLaborSporadic1(LaborSporadicMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(LaborSporadicMaster), obj.ProjectId);
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            // return SaveOrUpdateByDao(obj) as LaborSporadicMaster;
            LaborSporadicMaster master = SaveOrUpdateByDao(obj) as LaborSporadicMaster;
            if (master.DocState == DocumentState.InExecute)
            {
                foreach (LaborSporadicDetail oDetail in master.Details)
                {
                    oDetail.IsCreate = 0;
                }
                UpdateLaborSporadic(master);
            }
            return master;
        }
        [TransManager]
        public LaborSporadicMaster SaveOrUpdateLaborSporadic(LaborSporadicMaster curBillMaster, IList lstPenaltyDeductionMaster)
        {
            string sPenaltyDeductionMasterIds = string.Empty;
            if (lstPenaltyDeductionMaster != null && lstPenaltyDeductionMaster.Count > 0)
            {
                foreach (PenaltyDeductionMaster obj in lstPenaltyDeductionMaster)
                {
                    dao.Save(obj);
                }

                sPenaltyDeductionMasterIds = string.Join("|", lstPenaltyDeductionMaster.OfType<PenaltyDeductionMaster>().Select(a => a.Id).ToArray());
            }
            if (curBillMaster != null)
            {
                int iCount = curBillMaster.Details.OfType<LaborSporadicDetail>().Where(a => a.IsCreate == 1).Count();
                if (curBillMaster.Details.Count() == iCount)
                {
                    curBillMaster.DocState = DocumentState.InExecute;
                }
                if (!string.IsNullOrEmpty(sPenaltyDeductionMasterIds))
                {
                    if (!string.IsNullOrEmpty(curBillMaster.PenaltyDeductionMaster))
                    {
                        curBillMaster.PenaltyDeductionMaster += "|";
                    }
                    curBillMaster.PenaltyDeductionMaster += sPenaltyDeductionMasterIds;
                }
                curBillMaster.LastModifyDate = DateTime.Now;
                curBillMaster = UpdateByDao(curBillMaster) as LaborSporadicMaster;
            }
            return curBillMaster;
        }
        [TransManager]
        public LaborSporadicMaster UpdateLaborSporadic(LaborSporadicMaster curBillMaster)
        {
            //查找相同的被代工队伍
            Hashtable ht = new Hashtable();
            foreach (LaborSporadicDetail theLaborSporadicDetail in curBillMaster.Details)
            {
                if (theLaborSporadicDetail.IsCreate == 0)
                {

                    PenaltyDeductionMaster thePenaltyDeductionMaster = new PenaltyDeductionMaster();
                    PenaltyDeductionDetail thePenaltyDeductionDetail = new PenaltyDeductionDetail();
                    thePenaltyDeductionMaster.CreatePerson = curBillMaster.CreatePerson;
                    thePenaltyDeductionMaster.CreatePersonName = curBillMaster.CreatePersonName;
                    thePenaltyDeductionMaster.CreateDate = curBillMaster.CreateDate;
                    thePenaltyDeductionMaster.CreateYear = curBillMaster.CreateYear;
                    thePenaltyDeductionMaster.CreateMonth = curBillMaster.CreateMonth;
                    thePenaltyDeductionMaster.PenaltyType = EnumUtil<PenaltyDeductionType>.FromDescription("代工扣款");
                    thePenaltyDeductionMaster.OperOrgInfoName = curBillMaster.OperOrgInfoName;//登录人姓名
                    thePenaltyDeductionMaster.OperOrgInfo = curBillMaster.OperOrgInfo;//
                    thePenaltyDeductionMaster.OpgSysCode = curBillMaster.OpgSysCode;
                    thePenaltyDeductionMaster.HandlePerson = curBillMaster.HandlePerson;
                    thePenaltyDeductionMaster.HandlePersonName = curBillMaster.HandlePersonName;
                    thePenaltyDeductionMaster.DocState = DocumentState.InExecute;
                    thePenaltyDeductionMaster.PenaltyDeductionReason = "代工扣款";
                    thePenaltyDeductionMaster.ProjectId = curBillMaster.ProjectId;
                    thePenaltyDeductionMaster.ProjectName = curBillMaster.ProjectName;
                    if (theLaborSporadicDetail.InsteadTeam == null) continue;
                    thePenaltyDeductionMaster.PenaltyDeductionRant = theLaborSporadicDetail.InsteadTeam;
                    thePenaltyDeductionMaster.PenaltyDeductionRantName = theLaborSporadicDetail.InsteadTeamName;
                    if (theLaborSporadicDetail.AccountPrice != 0)
                    {
                        thePenaltyDeductionDetail.ProjectTask = theLaborSporadicDetail.ProjectTast;
                        thePenaltyDeductionDetail.ProjectTaskName = theLaborSporadicDetail.ProjectTastName;
                        thePenaltyDeductionDetail.ProjectTaskSyscode = theLaborSporadicDetail.ProjectTaskSyscode;
                        thePenaltyDeductionDetail.AccountMoney = theLaborSporadicDetail.AccountSumMoney;//核算金额
                        thePenaltyDeductionDetail.ProjectTaskDetail = theLaborSporadicDetail.ProjectTastDetail;
                        thePenaltyDeductionDetail.TaskDetailName = theLaborSporadicDetail.ProjectTastDetailName;
                        //thePenaltyDeductionDetail.ProjectDetailSysCode = theLaborSporadicDetail;
                        thePenaltyDeductionDetail.AccountPrice = theLaborSporadicDetail.AccountPrice;//合算单价
                        thePenaltyDeductionDetail.BusinessDate = DateTime.Now.Date;//业务发生时间
                        thePenaltyDeductionDetail.Cause = theLaborSporadicDetail.LaborDescript;
                        thePenaltyDeductionDetail.AccountQuantity = theLaborSporadicDetail.AccountLaborNum;//核算工程量
                        thePenaltyDeductionDetail.AccountState = theLaborSporadicDetail.SettlementState;//核算状态
                        thePenaltyDeductionDetail.PenaltySubjectGUID = theLaborSporadicDetail.LaborSubject;//用工科目
                        thePenaltyDeductionDetail.PenaltySubject = theLaborSporadicDetail.LaborSubjectName;//用工科目名称
                        thePenaltyDeductionDetail.PenaltySysCode = theLaborSporadicDetail.LaborSubjectSysCode;//用工科目层次码
                        thePenaltyDeductionDetail.ProductUnit = theLaborSporadicDetail.QuantityUnit;//工程量计量单位
                        thePenaltyDeductionDetail.ProductUnitName = theLaborSporadicDetail.QuantityUnitName;//工程量计量单位名称
                        thePenaltyDeductionDetail.ProjectTask = theLaborSporadicDetail.ProjectTast;//工程项目任务
                        thePenaltyDeductionDetail.ProjectTaskName = theLaborSporadicDetail.ProjectTastName;//工程项任务名称
                        thePenaltyDeductionDetail.TaskDetailName = theLaborSporadicDetail.ProjectTastDetailName;//工程项任务明细名称
                        thePenaltyDeductionDetail.PenaltyQuantity = theLaborSporadicDetail.AccountLaborNum;//罚款用工量
                        thePenaltyDeductionDetail.ForwardDetailId = theLaborSporadicDetail.Id;//罚扣款明细GUID：临时使用不存储数据库
                        //thePenaltyDeductionDetail.PenaltyType = ClientUtil.ToString(PenaltyDeductionType.代工扣款);//罚扣类型
                        thePenaltyDeductionDetail.MoneyUnitName = theLaborSporadicDetail.PriceUnitName;
                        thePenaltyDeductionDetail.MoneyUnit = theLaborSporadicDetail.PriceUnit;
                        //保存关联零星用工明细
                        thePenaltyDeductionDetail.LaborDetailGUID = theLaborSporadicDetail;
                        thePenaltyDeductionDetail.ResourceType = theLaborSporadicDetail.ResourceType;
                        thePenaltyDeductionDetail.ResourceTypeName = theLaborSporadicDetail.ResourceTypeName;
                        thePenaltyDeductionDetail.ResourceTypeSpec = theLaborSporadicDetail.ResourceTypeSpec;
                        thePenaltyDeductionDetail.ResourceTypeStuff = theLaborSporadicDetail.ResourceTypeStuff;
                        thePenaltyDeductionDetail.ResourceSysCode = theLaborSporadicDetail.ResourceSysCode;

                        if (ht.Count == 0)
                        {
                            thePenaltyDeductionMaster.AddDetail(thePenaltyDeductionDetail);
                            ht.Add(thePenaltyDeductionMaster, thePenaltyDeductionMaster.PenaltyDeductionRantName);
                        }
                        else
                        {
                            PenaltyDeductionMaster master = new PenaltyDeductionMaster();
                            if (ht.ContainsValue(theLaborSporadicDetail.InsteadTeamName))
                            {
                                foreach (System.Collections.DictionaryEntry objht in ht)
                                {
                                    if (objht.Value.ToString().Equals(theLaborSporadicDetail.InsteadTeamName))
                                    {
                                        master = objht.Key as PenaltyDeductionMaster;
                                        break;
                                    }
                                }
                                master.AddDetail(thePenaltyDeductionDetail);
                                ht.Remove(master);
                                ht.Add(master, master.PenaltyDeductionRantName);

                            }
                            else
                            {
                                thePenaltyDeductionMaster.AddDetail(thePenaltyDeductionDetail);
                                ht.Add(thePenaltyDeductionMaster, thePenaltyDeductionMaster.PenaltyDeductionRantName);
                            }
                        }
                        theLaborSporadicDetail.IsCreate = 1;
                    }
                }
            }

            if (ht.Count > 0)
            {
                foreach (System.Collections.DictionaryEntry objectht in ht)
                {
                    PenaltyDeductionMaster mat = new PenaltyDeductionMaster();
                    mat = objectht.Key as PenaltyDeductionMaster;

                    //核算金额不为0才生成扣款单
                    if (mat.SumMoney != 0)
                    {
                        mat = SavePenaltyDeduction(mat);
                    }
                }
            }
            bool flag = true;
            foreach (LaborSporadicDetail dtl in curBillMaster.Details)
            {
                if (dtl.IsCreate == 0)
                {
                    //有一个iscreate是0就是有没有审核的单据
                    flag = false;
                    break;
                }
            }

            if (flag)
            {
                curBillMaster.DocState = DocumentState.InExecute;
            }
            curBillMaster.LastModifyDate = DateTime.Now;
            return UpdateByDao(curBillMaster) as LaborSporadicMaster;
        }

        [TransManager]
        public PenaltyDeductionMaster SavePenaltyDeduction(PenaltyDeductionMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(PenaltyDeductionMaster));
            }
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as PenaltyDeductionMaster;
        }

        [TransManager]
        public Material SearchMaterial(string name)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Name", name));
            IList list = GetMaterial(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as Material;
            }
            return null;
        }
        /// <summary>
        /// 物资查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetMaterial(ObjectQuery objectQuery)
        {
            return Dao.ObjectQuery(typeof(Material), objectQuery);
        }
        public DataSet GetAuthedLaborSporadic(string sProjectId)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            command.CommandText = "select  t.laborstate,count(t.id) cnt from thd_laborsporadicmaster t where t.state=4  and t.projectid=:ProjectId  group by t.laborstate having count(t.id)>0";
            command.Parameters.Add(new OracleParameter("ProjectId", sProjectId));
            IDataReader reader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(reader);
            return ds;
        }
        #endregion
    }





}
