using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.ProductionManagement.PenaltyDeductionManagement.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data.SqlClient;
using VirtualMachine.Core.DataAccess;
using CommonSearchLib.BillCodeMng.Service;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using System.Data.OracleClient;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.Service
{
   

    /// <summary>
    /// 罚扣款单服务
    /// </summary>
    public class PenaltyDeductionSrv : BaseService, IPenaltyDeductionSrv
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
        #endregion
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
        /// 保存或修改工程对象关联文档
        /// </summary>
        /// <param name="item">工程对象关联文档</param>
        /// <returns></returns>
        [TransManager]
        public ProObjectRelaDocument SaveOrUpdate(ProObjectRelaDocument item)
        {
            dao.SaveOrUpdate(item);

            return item;
        }

        /// <summary>
        /// 删除对象集合
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <returns></returns>
        [TransManager]
        public bool Delete(IList list)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            bool flag = false;
            foreach (ProObjectRelaDocument cg in list)
            {
                if (!string.IsNullOrEmpty(cg.Id))
                {
                    dis.Add(Expression.Eq("Id", cg.Id));
                    flag = true;
                }
            }
            if (flag)
            {
                oq.AddCriterion(dis);

                IList listTemp = dao.ObjectQuery(typeof(ProObjectRelaDocument), oq);
                if (listTemp != null && listTemp.Count > 0)
                    dao.Delete(listTemp);
            }

            return true;
        }

        #region 罚扣款单
        /// <summary>
        /// 通过ID查询罚扣款单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PenaltyDeductionMaster GetPenaltyDeductionById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.ProjectTask", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.ProjectTaskDetail", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.PenaltySubjectGUID", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.ProductUnitName", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("Details.MaterialResource.StandardUnit", NHibernate.FetchMode.Eager);


            IList list = GetPenaltyDeduction(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as PenaltyDeductionMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询罚扣款单信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public PenaltyDeductionMaster GetPenaltyDeductionByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.ProjectTask", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.ProjectTaskDetail", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.PenaltySubjectGUID", NHibernate.FetchMode.Eager);
            IList list = GetPenaltyDeduction(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as PenaltyDeductionMaster;
            }
            return null;
        }

        /// <summary>
        /// 罚扣款单查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetPenaltyDeduction(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.ProjectTask", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.ProjectTaskDetail", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.PenaltySubjectGUID", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(PenaltyDeductionMaster), objectQuery);
        }

        /// <summary>
        /// 罚扣款单查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet PenaltyDeductionQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.Code,t2.AccountMoney,t2.AccountPrice,t2.BusinessDate,t2.Cause,t2.AccountQuantity,t1.PenaltyDeductionRantName,t1.State,t1.RealOperationDate,t1.Descript
            ,t1.CreatePersonName,t2.MoneyUnitName,t1.PenaltyDeductionReason,t2.PenaltyMoney,t2.PenaltyQuantity,t1.CreateDate,t2.balancedtlguid
            ,t2.PenaltySubject,t2.ProductUnitName,t2.TaskDetailName,t1.PenaltyType,t2.ProjectTaskName
             FROM THD_PenaltyDeductionMaster t1 INNER JOIN THD_PenaltyDeductionDetail t2 ON t1.id = t2.ParentId ";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }


        [TransManager]
        public PenaltyDeductionMaster SavePenaltyDeduction(PenaltyDeductionMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(PenaltyDeductionMaster));
                obj.RealOperationDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as PenaltyDeductionMaster;
        }
        [TransManager]
        public PenaltyDeductionMaster UpdatePenaltyDeduction(PenaltyDeductionMaster obj)
        {
            obj.LastModifyDate = DateTime.Now;
            return UpdateByDao(obj) as PenaltyDeductionMaster;
        }
        #endregion

        #region 多维度报表

        public DataSet GetMainCostData(string sProjectId)
        {
            string sSQL = @"select id, pbsname,subjectname,tlevel,sum(constructionarea) area,sum(gjQty)gjQty,sum(gjMoney)gjMoney,sum(stQty)stQty,sum(stMoney)stMoney,sum(mbQty)mbQty,sum(mbMoney)mbMoney,sum(qtQty)qtQty,sum(qtMoney)qtMoney,sum(qty)qty,sum(money)money
from (select t.id, t.name pbsname,nvl(t5.name,'') subjectname,t.tlevel,t.constructionarea, 
decode(t5.name,'钢筋',t4.responsibilitilyworkamount,0) gjQty ,decode(t5.name,'钢筋',t4.responsibilitilytotalprice,0)gjMoney,
decode(t5.name,'商砼',t4.responsibilitilyworkamount,0) stQty ,decode(t5.name,'商砼',t4.responsibilitilytotalprice,0)stMoney,
decode(t5.name,'模板',t4.responsibilitilyworkamount,0) mbQty ,decode(t5.name,'模板',t4.responsibilitilytotalprice,0)mbMoney,
decode(t5.name,'砌体',t4.responsibilitilyworkamount,0) qtQty ,decode(t5.name,'砌体',t4.responsibilitilytotalprice,0)qtMoney,
t4.responsibilitilyworkamount qty,t4.responsibilitilytotalprice money
from thd_pbstree t
join thd_gwbsrelapbs t1 on t.id=t1.pbsid  
join thd_gwbstree t2 on t1.parentid=t2.id and t2.theprojectguid=:projectId
join thd_gwbsdetail t3 on t2.id=t3.parentid and t3.theprojectguid=:projectId
join thd_gwbsdetailcostsubject t4 on t3.id=t4.gwbsdetailid and t4.theprojectguid=:projectId
join (select  DECODE(T.CODE,'C5110203','钢筋','C5110204','商砼','C51208','模板','C511020701','砌体',t.name) name ,
              T.SYSCODE ||'%' syscode  from THD_COSTACCOUNTSUBJECT T 
              WHERE T.CODE IN ('C5110203','C5110204','C51208','C511020701')) t5 on t4.costaccountsubjectsyscode like t5.syscode 
where t.theprojectguid=:projectId and t.tlevel>2) group by  id, pbsname,subjectname,tlevel  order by tlevel";
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            command.CommandText = sSQL;
            command.Parameters.Add(new OracleParameter("projectId",sProjectId));
            IDataReader read = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(read);
            return ds;
        }
        public DataSet GetNoMainCostData(string sProjectId,string sOrgSysCode)
        {
            string sSQL = string.Empty;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            if (string.IsNullOrEmpty(sProjectId))
            {
                sSQL = @"with temp as (select t.id from resconfig t where t.ownerorgsyscode like :OrgSysCode||'%')
select theprojectname,sum(constructionarea) area,sum(glQty)glQty,sum(glMoney)glMoney,sum(ljQty)ljQty,sum(ljMoney)ljMoney,sum(jxQty)jxQty,sum(jxMoney)jxMoney,sum(aqwmQty)aqwmQty,sum(aqwmMoney)aqwmMoney,
sum(gfQty)gfQty,sum(gfMoney)gfMoney,sum(sjQty)sjQty,sum(sjMoney)sjMoney,sum(qty)qty,sum(money)money
from (select t.theprojectname, t.constructionarea, 
decode(t5.name,'管理',t4.responsibilitilyworkamount,0) glQty ,decode(t5.name,'管理',t4.responsibilitilytotalprice,0)glMoney,
decode(t5.name,'临建',t4.responsibilitilyworkamount,0) ljQty ,decode(t5.name,'临建',t4.responsibilitilytotalprice,0)ljMoney,
decode(t5.name,'机械',t4.responsibilitilyworkamount,0) jxQty ,decode(t5.name,'机械',t4.responsibilitilytotalprice,0)jxMoney,
decode(t5.name,'安全文明',t4.responsibilitilyworkamount,0) aqwmQty ,decode(t5.name,'安全文明',t4.responsibilitilytotalprice,0)aqwmMoney,
decode(t5.name,'规费',t4.responsibilitilyworkamount,0) gfQty ,decode(t5.name,'规费',t4.responsibilitilytotalprice,0)gfMoney,
decode(t5.name,'税金',t4.responsibilitilyworkamount,0) sjQty ,decode(t5.name,'税金',t4.responsibilitilytotalprice,0)sjMoney
,t4.responsibilitilyworkamount qty,t4.responsibilitilytotalprice money
from thd_pbstree t
join thd_gwbsrelapbs t1 on t.id=t1.pbsid  
join thd_gwbstree t2 on t1.parentid=t2.id and t2.theprojectguid in (select * from  temp)
join thd_gwbsdetail t3 on t2.id=t3.parentid and t3.theprojectguid in (select * from  temp)
join thd_gwbsdetailcostsubject t4 on t3.id=t4.gwbsdetailid and t4.theprojectguid in (select * from  temp)
join (select  DECODE(T.CODE,'C513','管理','C516','临建','C51103','机械','C51202','安全文明','C51203','安全文明','C514','规费','C515','税金',t.name) name ,
              T.SYSCODE ||'%' syscode  from THD_COSTACCOUNTSUBJECT T 
              WHERE T.CODE IN ('C513','C516','C51103','C51202','C51203','C514','C515')) t5 on t4.costaccountsubjectsyscode like t5.syscode 
where t.theprojectguid  in (select * from  temp) and t.tlevel>2) group by theprojectname";
                command.Parameters.Add(new OracleParameter("OrgSysCode", sOrgSysCode));
            }
            else
            {
                sSQL = @"select theprojectname,sum(constructionarea) area,sum(glQty)glQty,sum(glMoney)glMoney,sum(ljQty)ljQty,sum(ljMoney)ljMoney,sum(jxQty)jxQty,sum(jxMoney)jxMoney,sum(aqwmQty)aqwmQty,sum(aqwmMoney)aqwmMoney,
sum(gfQty)gfQty,sum(gfMoney)gfMoney,sum(sjQty)sjQty,sum(sjMoney)sjMoney,sum(qty)qty,sum(money)money
from (select t.theprojectname,t.constructionarea, 
decode(t5.name,'管理',t4.responsibilitilyworkamount,0) glQty ,decode(t5.name,'管理',t4.responsibilitilytotalprice,0)glMoney,
decode(t5.name,'临建',t4.responsibilitilyworkamount,0) ljQty ,decode(t5.name,'临建',t4.responsibilitilytotalprice,0)ljMoney,
decode(t5.name,'机械',t4.responsibilitilyworkamount,0) jxQty ,decode(t5.name,'机械',t4.responsibilitilytotalprice,0)jxMoney,
decode(t5.name,'安全文明',t4.responsibilitilyworkamount,0) aqwmQty ,decode(t5.name,'安全文明',t4.responsibilitilytotalprice,0)aqwmMoney,
decode(t5.name,'规费',t4.responsibilitilyworkamount,0) gfQty ,decode(t5.name,'规费',t4.responsibilitilytotalprice,0)gfMoney,
decode(t5.name,'税金',t4.responsibilitilyworkamount,0) sjQty ,decode(t5.name,'税金',t4.responsibilitilytotalprice,0)sjMoney
,t4.responsibilitilyworkamount qty,t4.responsibilitilytotalprice money
from thd_pbstree t
join thd_gwbsrelapbs t1 on t.id=t1.pbsid  
join thd_gwbstree t2 on t1.parentid=t2.id and t2.theprojectguid=:projectId
join thd_gwbsdetail t3 on t2.id=t3.parentid and t3.theprojectguid=:projectId
join thd_gwbsdetailcostsubject t4 on t3.id=t4.gwbsdetailid and t4.theprojectguid=:projectId
join (select  DECODE(T.CODE,'C513','管理','C516','临建','C51103','机械','C51202','安全文明','C51203','安全文明','C514','规费','C515','税金',t.name) name ,
              T.SYSCODE ||'%' syscode  from THD_COSTACCOUNTSUBJECT T 
              WHERE T.CODE IN ('C513','C516','C51103','C51202','C51203','C514','C515')) t5 on t4.costaccountsubjectsyscode like t5.syscode 
where t.theprojectguid=:projectId and t.tlevel>2) group by  theprojectname";
                command.Parameters.Add(new OracleParameter("projectId", sProjectId));
            }
            command.CommandText = sSQL;
            IDataReader read = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(read);
            return ds;
        }
        #endregion
    }





}
