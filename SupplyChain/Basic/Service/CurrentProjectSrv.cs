using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data.SqlClient;
using VirtualMachine.Core.DataAccess;
using CommonSearchLib.BillCodeMng.Service;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;

namespace Application.Business.Erp.SupplyChain.Basic.Service
{


    /// <summary>
    /// 项目部基本信息
    /// </summary>
    public class CurrentProjectSrv : BaseService, ICurrentProjectSrv
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

        private string GetCode(Type type, string specail)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, specail);
        }

        /// <summary>
        /// 根据项目 物资分类(专业分类) 生成Code
        /// </summary>
        /// <param name="type"></param>
        /// <param name="projectId"></param>
        /// <param name="matCatAbb"></param>
        /// <returns></returns>
        private string GetCode(Type type, string projectId, string matCatAbb)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, projectId, matCatAbb);
        }
        #endregion

        #region 项目部基本信息

        /// <summary>
        ///  通过业务组织查询项目信息
        /// </summary>
        /// <returns></returns>
        public DataDomain GetProjectInfoByOpgId(string opgId)
        {
            DataDomain domain = new DataDomain();

            string sql = @"select t1.id,t1.projectname,t1.PROJECTCURRSTATE,t1.CONTRACTCOLLECTRATIO,t1.LASTUPDATEDATE 
                            from resconfig t1,resoperationorg t2 where t1.ownerorg=t2.opgid and t2.opgid='" + opgId + "'";
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    domain.Name1 = TransUtil.ToString(dataRow["id"]);
                    domain.Name2 = TransUtil.ToString(dataRow["projectname"]);
                    domain.Name3 = TransUtil.ToString(dataRow["PROJECTCURRSTATE"]);
                    domain.Name4 = TransUtil.ToString(dataRow["CONTRACTCOLLECTRATIO"]);
                    domain.Name5 = TransUtil.ToDateTime(dataRow["LASTUPDATEDATE"]);
                }
            }
            return domain;
        }

        public CurrentProjectInfo GetProjectInfoByOwnOrg(string ownOrgId)
        {
            var objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("OwnerOrg.Id", ownOrgId));

            var list = Dao.ObjectQuery(typeof (CurrentProjectInfo), objQuery);
            if (list == null || list.Count == 0)
            {
                return null;
            }

            return list[0] as CurrentProjectInfo;
        }

        //保存业务信息
        [TransManager]
        public OperationOrg SaveOperationOrg(OperationOrg obj)
        {
            obj.UpdatedDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as OperationOrg;
        }

        public IList GetStandardOperationJob()
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("State", 1));
            objectQuery.AddCriterion(Expression.Eq("OpjType", 1));
            objectQuery.AddFetchMode("OperationOrg", FetchMode.Eager);
            objectQuery.AddFetchMode("JobWithRole", FetchMode.Eager);

            return dao.ObjectQuery(typeof(OperationJob), objectQuery);
        }

        private List<DataDomain> QueryData(string sql)
        {
            if (string.IsNullOrEmpty(sql))
            {
                return null;
            }

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection oConn = session.Connection;
            IDbCommand oCommand = oConn.CreateCommand();
            oCommand.CommandText = sql;
            IDataReader oRead = oCommand.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(oRead);

            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }

            var retList = new List<DataDomain>();
            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var dt = new DataDomain();
                var type = dt.GetType();
                for (var j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    var pi = type.GetProperty(string.Format("Name{0}", j + 1));
                    pi.SetValue(dt, ds.Tables[0].Rows[i][j], null);
                }

                retList.Add(dt);
            }

            return retList;
        }

        public List<DataDomain> GetAllPersonJobsByOrg(string orgId, string pId)
        {
            string sql = @"
            select j.opjid,j.opjname,
                   case when p.peronjobid is not null and p.state = 1 then '已申请'
                        when p.peronjobid is not null and p.state = 0 then '待审核'
                        else '可申请'
                   end as jobState,p.peronjobid
            from resoperationjob j
            left join respersononjob p on p.operationjobid = j.opjid and p.perid = '{1}'
            where j.opjorgid = '{0}'";

            return QueryData(string.Format(sql, orgId, pId));
        }

        public List<DataDomain> GetPersonJobRequest()
        {
            string sql = @"
                select r.peronjobid,r.operationjobid,j.opjname,r.perid,p.pername,g.opgname,rg.opgsyscode
                from respersononjob r
                join resoperationjob j on j.opjid = r.operationjobid
                join resperson p on p.perid = r.perid
                join resoperationorg g on g.opgid = j.opjorgid
                join resoperationorg rg on rg.opgoperationtype = 'b' and instr(g.opgsyscode,rg.opgsyscode)>0
                where r.state = 0
                ORDER BY g.opgname,p.pername,j.opjname";

            return QueryData(sql);
        }

        public List<DataDomain> GetAllJobsByPerson(string perId)
        {
            var sql = @"
            select j.opjid,j.opjname,
                   case when p.peronjobid is not null and p.state = 1 then '已申请'
                        when p.peronjobid is not null and p.state = 0 then '待审核'
                        else '可申请'
                   end as jobState,p.peronjobid,j.opjorgid,g.opgname
            from respersononjob p
            join resoperationjob j on p.operationjobid = j.opjid and p.perid = '{0}'
            join resoperationorg g on g.opgid = j.opjorgid
            order by g.opgname,j.opjname";

            return QueryData(string.Format(sql, perId));
        }

        public List<DataDomain> GetBranchSyscodesByPersonCode(string code)
        {
            string s = @"
                select rg.opgsyscode
                from respersononjob r
                join resoperationjob j on j.opjid = r.operationjobid
                join resperson p on p.perid = r.perid
                join resoperationorg g on g.opgid = j.opjorgid
                join resoperationorg rg on rg.opgoperationtype = 'b' and instr(g.opgsyscode,rg.opgsyscode)>0
                where r.state = 1 and p.percode = '{0}'
                group by rg.opgsyscode";
            return QueryData(string.Format(s, code));
        }

        //保存业务信息
        [TransManager]
        public CurrentProjectInfo SaveCurrentProjectInfo(CurrentProjectInfo obj)
        {
            obj.LastUpdateDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as CurrentProjectInfo;
        }

        [TransManager]
        public CurrentProjectInfo AffirmProject(OperationOrg newOrg, CurrentProjectInfo selectProject, IList opgJobs)
        {
            if (newOrg == null || selectProject == null || opgJobs == null)
            {
                throw new Exception("确认信息不能空");
            }

            if (!Dao.SaveOrUpdate(newOrg))
            {
                throw new Exception("项目启动创建新组织失败");
            }

            newOrg.SysCode = string.Concat(newOrg.ParentNode.SysCode, newOrg.Id, ".");
            if (!Dao.SaveOrUpdate(newOrg))
            {
                throw new Exception("项目启动更新新组织失败");
            }

            selectProject.OwnerOrg = newOrg;
            selectProject.OwnerOrgName = selectProject.OwnerOrg.Name;
            selectProject.OwnerOrgSysCode = selectProject.OwnerOrg.SysCode;
            selectProject.Code = selectProject.OwnerOrg.Code;

            if (!Dao.SaveOrUpdate(selectProject))
            {
                throw new Exception("项目启动更新项目组织信息失败");
            }

            foreach (OperationJob opgJob in opgJobs)
            {
                opgJob.Code = string.Concat(selectProject.OwnerOrg.Code, opgJob.Code);
                opgJob.OperationOrg = selectProject.OwnerOrg;
                //opgJob.SysCode = selectProject.OwnerOrg.SysCode;
                //opgJob.Id = System.Guid.NewGuid().ToString();
                opgJob.SysCode = selectProject.OwnerOrg.SysCode;// string.Format("{0}{1}.", selectProject.OwnerOrg.SysCode, opgJob.Id);
            }

            if (!Dao.SaveOrUpdate(opgJobs))
            {
                throw new Exception("项目启动创建标准岗位失败");
            }
            else
            {
                foreach (OperationJob opgJob in opgJobs)
                {
                    opgJob.SysCode = string.Format("{0}{1}.", opgJob.SysCode, opgJob.Id);
                }
                if (!Dao.SaveOrUpdate(opgJobs))
                {
                    throw new Exception("项目启动创建标准岗位层次码失败");
                }
            }

            return selectProject;
        }

        /// <summary>
        /// 通过ID查询项目信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CurrentProjectInfo GetProjectById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetCurrentProjectInfo(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as CurrentProjectInfo;
            }
            return null;
        }

        /// <summary>
        /// 查询项目业务信息
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetCurrentProjectInfo(ObjectQuery oq)
        {
            IList list = new ArrayList();

            oq.AddFetchMode("OwnerOrg", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("ProRelationUnitdetails", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("SelFeeDetails", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("SelFeeFormulas", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("SelFeeTemplateMaster", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("ListSumAreaParame", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("ListMachineCostParame", NHibernate.FetchMode.Eager);
            list = Dao.ObjectQuery(typeof(CurrentProjectInfo), oq);
            if (list == null || list.Count == 0)
            {
                return null;
            }

            var projectinfo = list[0] as CurrentProjectInfo;

            List<PeriodNode> lstDetails = GetListByFunc<PeriodNode>(projectinfo.Id, "ProjectId.Id");
            projectinfo.Details.Clear();
            projectinfo.Details.AddAll(lstDetails);

            List<ProRelationUnit> lstProRelationUnitdetails = GetListByFunc<ProRelationUnit>(projectinfo.Id, "ProjectId.Id");
            projectinfo.ProRelationUnitdetails.Clear();
            projectinfo.ProRelationUnitdetails.AddAll(lstProRelationUnitdetails);

            List<SelFeeDtl> lstSelFeeDtl = GetListByFunc<SelFeeDtl>(projectinfo.Id, "ProjectInfo.Id");
            projectinfo.SelFeeDetails.Clear();
            projectinfo.SelFeeDetails.AddAll(lstSelFeeDtl);

            List<SelFeeFormula> lstSelFeeFormula = GetListByFunc<SelFeeFormula>(projectinfo.Id, "ProjectInfo.Id");
            projectinfo.SelFeeFormulas.Clear();
            projectinfo.SelFeeFormulas.AddAll(lstSelFeeFormula);

            //List<SelFeeTemplateMaster> lstSelFeeTemplateMaster = GetListByFunc<SelFeeTemplateMaster>(projectinfo.SelFeeTemplateMaster.Id, "SelFeeTemplateMaster"); ;
            //projectinfo.SelFeeTemplateMaster = lstSelFeeTemplateMaster[0];

            List<SumAreaParame> lstSumArea = GetListByFunc<SumAreaParame>(projectinfo.Id, "ProjectId");
            projectinfo.ListSumAreaParame.Clear();
            projectinfo.ListSumAreaParame.AddAll(lstSumArea);

            List<MachineCostParame> lstMachineCost = GetListByFunc<MachineCostParame>(projectinfo.Id, "ProjectId");
            projectinfo.ListMachineCostParame.Clear();
            projectinfo.ListMachineCostParame.AddAll(lstMachineCost);

            return list;
        }

        /// <summary>
        /// 通过委托方式获取对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arg"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private List<T> GetListByFunc<T>(string arg, string propertyName) 
            where T:class
        {
            Func<string, List<T>> method = (id) =>
            {
                ObjectQuery objectQuery = new ObjectQuery();
                objectQuery.AddCriterion(Expression.Eq(propertyName, id));
                return Dao.ObjectQuery(typeof(T), objectQuery).OfType<T>().ToList();
            };
            IAsyncResult cookie = method.BeginInvoke(arg, null, null);
            List<T> result = method.EndInvoke(cookie);
            return result;
        }

        public IList GetAllValideProject()
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Not(Expression.Eq("ProjectCurrState", 20)));
            objectQuery.AddCriterion(Expression.Not(Expression.Eq("ProjectCurrState", 10)));

            return Dao.ObjectQuery(typeof(CurrentProjectInfo), objectQuery);
        }

        public DataSet GetCurrentProjectInfo(string sWhere)
        {
            string sOrder = " order by t.projectname";
            string sSQL = @"select t.id, t.projectcode Code,trim(t.projectname) Name,t.handlepersonname ,t.projecttype,
t.projectcurrstate,t.projectinfostate,t.ConstractStage,t.projectcost,t.buildingarea, 
t.BuildingHeight,t.GroundLayers,t.UnderGroundLayers,projecttypedescript,materialnote from resconfig t
where exists(select 1 from resoperationorg t1 where t.ownerorg=t1.opgid and t1.opgstate=1) and
 t.projectname is not null ";
            ISession oSession = CallContext.GetData("nhsession") as ISession;
            IDbConnection oConn = oSession.Connection;
            IDbCommand oCommand = oConn.CreateCommand();
            oCommand.CommandText = sSQL + sWhere + sOrder;
            IDataReader dataReader = oCommand.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return ds;
        }
        public void SaveUpdateCurrentProjectInfo(IList lstCurProjectSQL)
        {
            ISession oSession = CallContext.GetData("nhsession") as ISession;

            IDbConnection oConn = oSession.Connection;
            IDbTransaction tr = oConn.BeginTransaction();

            IDbCommand oCommand = oConn.CreateCommand();
            oCommand.Transaction = tr;
            try
            {
                foreach (string sSQL in lstCurProjectSQL)
                {
                    oCommand.CommandText = sSQL;
                    oCommand.ExecuteNonQuery();
                }
                tr.Commit();
            }
            catch (Exception ex)
            {
                tr.Rollback();
                throw ex;
            }

        }
        /// <summary>
        /// 查询项目业务信息(通用)
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList QueryCurrentProjectInfo(ObjectQuery oq)
        {
            IList list = new ArrayList();
            list = Dao.ObjectQuery(typeof(CurrentProjectInfo), oq);
            return list;
        }

        public StandardPerson GetStandardPerson(string id)
        {
            return Dao.Get(typeof(StandardPerson), id) as StandardPerson;
        }

        public StandardPerson GetStandardPersonByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));
            objectQuery.AddCriterion(Expression.Eq("State", 1));

            var list = Dao.ObjectQuery(typeof(StandardPerson), objectQuery);
            if (list == null || list.Count == 0)
            {
                return null;
            }

            return list[0] as StandardPerson;
        }

        /// <summary>
        ///  通过当前登陆人归属组织找到其归属的分公司
        /// </summary>
        /// <returns></returns>
        public string GetBelongOperationOrg(string operOrgSyscode)
        {
            string opgId = "";
            string sql = "select t1.opgid from resoperationorg t1 where t1.opgoperationtype in ('b') and instr('" + operOrgSyscode + "',t1.opgid)>0";
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    opgId = TransUtil.ToString(dataRow["opgid"]);
                }
            }
            return opgId;
        }
        public OperationOrg GetOwnerOrgByProjectId(string sProjectId)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", sProjectId));
            objectQuery.AddFetchMode("OwnerOrg", FetchMode.Eager);
            IList list = Dao.ObjectQuery(typeof(CurrentProjectInfo), objectQuery);
            return list == null || list.Count == 0 ? null : (list[0] as CurrentProjectInfo).OwnerOrg;

        }
        #endregion

        #region 物资信息价
        /// <summary>
        /// 通过ID查询物资信息价信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MaterialInterfacePrice GetMaterialPriceById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetMaterialPrice(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MaterialInterfacePrice;
            }
            return null;
        }
        /// <summary>
        /// 物资信息价信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetMaterialPrice(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("MaterialGUID", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(MaterialInterfacePrice), objectQuery);
        }
        [TransManager]
        public MaterialInterfacePrice SaveMaterialPrice(MaterialInterfacePrice obj)
        {
            obj.LastUpdateDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as MaterialInterfacePrice;
        }

        #endregion

        #region 项目降低率
        /// <summary>
        /// 通过ID查询项目降低率信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProgramReduceRate GetProgramRateById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetProgramRate(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ProgramReduceRate;
            }
            return null;
        }
        /// <summary>
        /// 项目降低率信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetProgramRate(ObjectQuery objectQuery)
        {
            return Dao.ObjectQuery(typeof(ProgramReduceRate), objectQuery);
        }
        public bool GetProgramRate(string sProjectID, string sMatCatID, string sSupplyID)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("ProjectId", sProjectID));
            objectQuery.AddCriterion(Expression.Eq("MaterialCategory.Id", sMatCatID));
            objectQuery.AddCriterion(Expression.Eq("Supplyer.Id", sSupplyID));
            objectQuery.AddCriterion(Expression.Eq("State", "1"));
            IList lstProgramReduceRate = Dao.ObjectQuery(typeof(ProgramReduceRate), objectQuery);
            return lstProgramReduceRate != null && lstProgramReduceRate.Count > 0;
        }
        [TransManager]
        public ProgramReduceRate SaveProgramRate(ProgramReduceRate obj)
        {
            obj.LastUpdateDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as ProgramReduceRate;
        }
        [TransManager]
        public bool SaveProgramRate(IList lstProgramReduceRate)
        {
            bool bFlag = false;
            try
            {
                bFlag = dao.SaveOrUpdate(lstProgramReduceRate);
            }
            catch
            {
                bFlag = false;
            }
            return bFlag;
        }

        #endregion

        #region 项目使用状态
        /// <summary>
        ///  通过日期查询项目使用状态
        /// </summary>
        /// <returns></returns>
        public Hashtable QueryProjectStateInfo(string projectID, DateTime startDate, DateTime endDate)
        {
            Hashtable ht = new Hashtable();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            DateTime lastStartDate = startDate.AddDays(-30);
            DateTime lastEndDate = startDate.AddDays(-1);
            string projectName = "";

            #region 项目基本信息
            //项目基本信息、合同摘要
            string basinInfo = "";//项目基本信息
            string perandjobinfo = "";//项目人员岗位信息
            string sql = "select t1.projectname,substr(t1.projectlocationdescript,0,10)||'...' locationdesc,substr(t1.contractarea,0,10)||'...' contractarea," +
                        " t1.buildingarea,t1.constractstage,t1.mappoint,t1.begindate," +
                        " substr(t1.qualityreword,0,10)||'...' qualityreword From resconfig t1 where t1.id = '" + projectID + "'";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    projectName = TransUtil.ToString(dataRow["projectname"]);
                    DateTime beginDate = TransUtil.ToDateTime(dataRow["begindate"]);
                    string bDate = "";
                    if (beginDate > TransUtil.ToDateTime("2000-01-01"))
                    {
                        bDate = beginDate.ToShortDateString();
                    }
                    else
                    {
                        bDate = " ";
                    }
                    basinInfo = "[工程地点：" + TransUtil.ToString(dataRow["locationdesc"]) + "] 、 "
                        + "[开工日期：" + bDate + "]、 "
                        + "[承包范围：" + TransUtil.ToString(dataRow["contractarea"]) + "]、 "
                        + "[建筑面积：" + TransUtil.ToString(dataRow["buildingarea"]) + "]、 "
                        + "[施工阶段：" + TransUtil.ToString(dataRow["constractstage"]) + "]、 "
                        + "[地图坐标：" + TransUtil.ToString(dataRow["mappoint"]) + "]、 "
                        + "[质量奖罚：" + TransUtil.ToString(dataRow["qualityreword"]) + "] ； ";
                }
            }
            //客户信息
            sql = " select count(*) count from THD_ProRelationUnit where parentid = '" + projectID + "'";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    basinInfo += "[客户信息：" + TransUtil.ToString(dataRow["count"]) + "条] ； ";
                }
            }
            //施工策划信息
            sql = "select count(*) count from THD_PeroidNode where parentid = '" + projectID + "'";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    basinInfo += "[施工工期节点信息：" + TransUtil.ToString(dataRow["count"]) + "条] ； ";
                }
            }
            //人员岗位设置、签名图片
            sql = "  select t3.opjname,t4.perid,t5.perphoto,t6.operationrolename From resconfig t1,resoperationorg t2, " +
                   " resoperationjob t3,respersononjob t4,resperson t5,resoperationjobwithrole t6 " +
                   " where t1.ownerorg=t2.opgid and instr(t3.opjsyscode,t2.opgid) !=0 and t3.opjid=t4.operationjobid  " +
                   " and t3.opjid=t6.operationjob and t4.perid=t5.perid and t1.id='" + projectID + "'";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            string jobstr = "";
            int jobcount = 0;
            int percount = 0;
            string perstr = "";
            string rolestr = "";
            int rolecount = 0;
            int photocount = 0;
            string photostr = "";
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string opjname = TransUtil.ToString(dataRow["opjname"]);
                    string perid = TransUtil.ToString(dataRow["perid"]);
                    string perphoto = TransUtil.ToString(dataRow["perphoto"]);
                    string operationrolename = TransUtil.ToString(dataRow["operationrolename"]);
                    if (jobstr.Contains(opjname) == false)
                    {
                        if (jobstr == "")
                        {
                            jobstr = opjname;
                        }
                        else
                        {
                            jobstr += "、" + opjname;
                        }
                        jobcount++;
                    }
                    if (rolestr.Contains(operationrolename) == false)
                    {
                        if (rolestr == "")
                        {
                            rolestr = operationrolename;
                        }
                        else
                        {
                            rolestr += "、" + operationrolename;
                        }
                        rolecount++;
                    }
                    if (perstr.Contains(perid) == false)
                    {
                        perstr += "、" + perid;
                        percount++;
                    }
                    if (TransUtil.ToString(perphoto) != "" && photostr.Contains(perphoto) == false)
                    {
                        photostr += "、" + perphoto;
                        photocount++;
                    }
                }
            }
            perandjobinfo += "[岗位设置：" + jobcount + "个]， ";
            perandjobinfo += "[角色设置：" + rolecount + "个]， ";
            perandjobinfo += "[人员设置：" + percount + "人]， ";
            perandjobinfo += "[上传签名照：" + photocount + "人]； ";
            ht.Add("basinInfo", basinInfo);
            ht.Add("perandjobinfo", perandjobinfo);
            #endregion

            #region 施工部位划分
            string pbsinfo = "";
            int pbscount = 0;
            Hashtable ht_pbs = new Hashtable();
            IList list_pbs = new ArrayList();
            sql = " select t1.structtypename,count(*) count from thd_pbstree t1 where t1.theprojectguid='" + projectID + "'" +
                    " group by t1.tlevel,t1.structtypename order by t1.tlevel,t1.structtypename";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DataDomain domain = new DataDomain();
                    string structtypename = TransUtil.ToString(dataRow["structtypename"]);
                    int count = TransUtil.ToInt(dataRow["count"]);
                    domain.Name1 = structtypename;
                    domain.Name2 = count + "";
                    if (list_pbs.Count == 0)
                    {
                        list_pbs.Add(domain);
                    }
                    else
                    {
                        bool ifExist = false;
                        foreach (DataDomain temp in list_pbs)
                        {
                            if (TransUtil.ToString(temp.Name1) == structtypename)
                            {
                                ifExist = true;
                                temp.Name2 = TransUtil.ToInt(temp.Name2) + count + "";
                            }
                        }
                        if (ifExist == false)
                        {
                            list_pbs.Add(domain);
                        }
                    }
                    pbscount += count;
                }
            }
            pbsinfo = "[施工部位一共：" + pbscount + "个]，其中：";
            foreach (DataDomain domain in list_pbs)
            {
                pbsinfo += "[" + domain.Name1 + "：" + domain.Name2 + "个]，";
            }
            ht.Add("pbsinfo", pbsinfo);
            #endregion

            #region 施工任务划分
            string gwbsinfo = "";
            string gwbsstr = "";
            string pbsstr = "";
            string tasktypestr = "";
            int gwbscount = 0;
            int pbsrelcount = 0;
            int confirmcount = 0;
            int tasktypecount = 0;
            sql = " select t1.id,t2.pbsid,t1.projecttasktypename,t1.productconfirmflag from thd_gwbstree t1,thd_gwbsrelapbs t2 " +
                    " where t1.id=t2.parentid and t1.taskstate in (0,1,5) and t1.theprojectguid='" + projectID + "'";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string id = TransUtil.ToString(dataRow["id"]);
                    string pbsid = TransUtil.ToString(dataRow["pbsid"]);
                    string projecttasktypename = TransUtil.ToString(dataRow["productconfirmflag"]);
                    int productconfirmflag = TransUtil.ToInt(dataRow["productconfirmflag"]);
                    if (!gwbsstr.Contains(id))
                    {
                        gwbsstr += id + "、";
                        gwbscount++;
                        if (productconfirmflag == 1)
                        {
                            confirmcount++;
                        }
                    }
                    if (!pbsstr.Contains(pbsid))
                    {
                        pbsstr += pbsid + "、";
                        pbsrelcount++;
                    }
                    if (!tasktypestr.Contains(projecttasktypename))
                    {
                        tasktypestr += projecttasktypename + "、";
                        tasktypecount++;
                    }
                }
            }
            gwbsinfo = "[施工任务共：" + gwbscount + "个]， 其中[生产节点有：" + confirmcount + "个]，" +
                        "关联的[任务类型有：" + tasktypecount + "个]，关联的[施工部位有：" + pbsrelcount + "个]";
            ht.Add("gwbsinfo", gwbsinfo);
            #endregion

            #region 成本信息维护
            string costbudgetinfo = "";
            int wdetailcount = 0;
            int responsecount = 0;
            int producecount = 0;
            int costingcount = 0;
            int plancount = 0;
            int contractcount = 0;
            //明细数量，成本核算数量，合同工程量、单价
            sql = " select t1.responseflag,t1.produceconfirmflag,t1.costingflag,t1.plantotalprice,t1.contracttotalprice " +
                    " from thd_gwbsdetail t1 where t1.state in (0,5) and t1.theprojectguid='" + projectID + "'";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    if (TransUtil.ToInt(dataRow["responseflag"]) == 1)
                    {
                        responsecount++;
                    }
                    if (TransUtil.ToInt(dataRow["produceconfirmflag"]) == 1)
                    {
                        producecount++;
                    }
                    if (TransUtil.ToInt(dataRow["costingflag"]) == 1)
                    {
                        costingcount++;
                    }
                    if (TransUtil.ToDecimal(dataRow["plantotalprice"]) > 0)
                    {
                        plancount++;
                    }
                    if (TransUtil.ToDecimal(dataRow["contracttotalprice"]) > 0)
                    {
                        contractcount++;
                    }
                    wdetailcount++;
                }
            }
            costbudgetinfo = "[施工任务明细共：" + wdetailcount + "条]，其中[责任明细：" + responsecount + "条]，[生产明细："
                            + producecount + "条]，[成本核算明细：" + costingcount + "条]，预算明细中已设置[计划成本信息："
                            + plancount + "条]，[合同收入信息：" + contractcount + "条]，";
            //资源耗用条数
            sql = "select count(id) count from thd_gwbsdetailcostsubject t1 where t1.theprojectguid='" + projectID + "'";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    costbudgetinfo += "[资源耗用：" + TransUtil.ToInt(dataRow["count"]) + "条]， ";
                }
            }
            //签证变更数
            sql = " select count(*) count from THD_GWBSDETAILLEDGER t1 where t1.createtime >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') " +
                    "and t1.createtime <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.theprojectguid='" + projectID + "'";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    costbudgetinfo += "[签证变更：" + TransUtil.ToInt(dataRow["count"]) + "条] ";
                }
            }
            ht.Add("costbudgetinfo", costbudgetinfo);
            #endregion

            #region 现场生产管理
            string produceinfo = "";
            string gwbsconfirminfo = "";//工程量提报
            string ladborinfo = "";//零星用工
            string gwbsacctinfo = "";//工程任务核算
            sql = " select t2.quantitybeforeconfirm,t2.gwbsdetail from thd_gwbstaskconfirmmaster t1, thd_gwbstaskconfirmdetail t2 " +
                    " where t1.id=t2.parentid and t1.projectid = '" + projectID + "'";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            int gconfirmcount = 0;
            string gdtlidstr = "";
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string dtlId = TransUtil.ToString(dataRow["gwbsdetail"]);
                    decimal confirmqty = TransUtil.ToDecimal(dataRow["quantitybeforeconfirm"]);
                    if (confirmqty > 0 && !gdtlidstr.Contains(dtlId))
                    {
                        gconfirmcount++;
                        gdtlidstr += "，" + dtlId;
                    }
                }
            }
            gwbsconfirminfo = "已提报的工程任务明细为：[" + gconfirmcount + "]； ";
            //零星用工信息
            sql = "  select t1.laborstate From thd_laborsporadicmaster t1  where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') " +
                    "and t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.projectid='" + projectID + "'";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            int laborcount = 0;
            int replacecount = 0;
            int timecount = 0;
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string laborstate = TransUtil.ToString(dataRow["laborstate"]);
                    if (laborstate != null && laborstate.Contains("零星"))
                    {
                        laborcount++;
                    }
                    if (laborstate != null && laborstate.Contains("代"))
                    {
                        replacecount++;
                    }
                    if (laborstate != null && laborstate.Contains("时"))
                    {
                        timecount++;
                    }
                }
            }
            ladborinfo = "其中[零星用工单：" + laborcount + "张]， [代工单：" + replacecount + "张]， [计时派工：" + timecount + "张]；";
            //工程任务核算单
            sql = "  select t1.id,t2.projecttaskdtlguid from thd_projecttaskaccountbill t1,thd_projecttaskdetailaccount t2 " +
                    " where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                    " t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.id=t2.parentid  and t1.theprojectguid='" + projectID + "' ";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            int accountbillcount = 0;
            int acctbilldtlcount = 0;
            string accountbillidstr = "";
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string dtlId = TransUtil.ToString(dataRow["projecttaskdtlguid"]);
                    string billId = TransUtil.ToString(dataRow["id"]);
                    if (!accountbillidstr.Contains(billId))
                    {
                        accountbillcount++;
                        accountbillidstr += "，" + billId;
                    }
                    acctbilldtlcount++;
                }
            }
            gwbsacctinfo = "其中[工程任务核算单：" + accountbillcount + "张]， [核算工程任务明细：" + acctbilldtlcount + "次]";
            produceinfo = gwbsconfirminfo + ladborinfo + gwbsacctinfo;
            ht.Add("produceinfo", produceinfo);
            #endregion

            #region 质安检查整改
            //整改通知单、罚款单、日常检查
            string safecheckinfo = "";
            string rectibillidstr = "";
            int rectibillcount = 0;
            int rectdtlcount = 0;
            sql = " select t1.id from thd_rectificatnoticemaster t1,thd_rectificatnoticedetail t2 " +
                    " where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                    " t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.id=t2.parentid  and t1.projectid='" + projectID + "'";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string id = TransUtil.ToString(dataRow["id"]);
                    if (!rectibillidstr.Contains(id))
                    {
                        rectibillcount++;
                        rectibillidstr += "，" + id;
                    }
                    rectdtlcount++;
                }
            }
            safecheckinfo += "在此期间开具[整改通知单：" + rectibillcount + "张]， 其中[整改明细：" + rectdtlcount + "条]， ";
            sql = " select t1.id from thd_penaltydeductionmaster t1,thd_penaltydeductiondetail t2 " +
                    " where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                    " t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.id=t2.parentid  and t1.projectid='" + projectID + "'";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            string penaltybillidstr = "";
            int penaltybillcount = 0;
            int penaltydtlcount = 0;
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string id = TransUtil.ToString(dataRow["id"]);
                    if (!penaltybillidstr.Contains(id))
                    {
                        penaltybillcount++;
                        penaltybillidstr += "，" + id;
                    }
                    penaltydtlcount++;
                }
            }
            safecheckinfo += "在此期间开具[罚款单：" + rectibillcount + "张]， 其中[罚款明细：" + rectdtlcount + "条]， ";

            sql = "    select count(t1.id) count from thd_inspectionrecord t1 where t1.inspectiondate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                    " t1.inspectiondate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.projectid='" + projectID + "' ";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            int checkcount = 0;
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    checkcount = TransUtil.ToInt(dataRow["count"]);
                }
            }
            safecheckinfo += "[检查记录单：" + checkcount + "张]";
            ht.Add("safecheckinfo", safecheckinfo);
            #endregion

            #region 物资管理
            //物资单据，收料单、领料单、结算单、商品砼记录单、料具收料、退料、物资消耗结算
            string materialinfo = "";
            sql = " select '收料' bustype,to_char(t1.id) id from thd_stkstockin t1,thd_stkstockindtl t2 where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                    " t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.id=t2.parentid and t1.projectid='" + projectID + "' " +
                    " union all " +
                    " select '领料' bustype,to_char(t1.id) id from thd_stkstockout t1,thd_stkstockoutdtl t2 where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                    " t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.id=t2.parentid and t1.projectid='" + projectID + "' " +
                    " union all " +
                    " select '结算' bustype,to_char(t1.id) id from thd_stockinbalmaster t1,thd_stockinbaldetail t2 where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                    " t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.id=t2.parentid and t1.projectid='" + projectID + "' " +
                    " union all " +
                    " select '料具收料' bustype,to_char(t1.id) id from thd_materialcollectionmaster t1,thd_materialcollectiondetail t2 where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                    " t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.id=t2.parentid and t1.projectid='" + projectID + "' " +
                    " union all " +
                    " select '料具退料' bustype,to_char(t1.id) id from thd_materialreturnmaster t1,thd_materialreturndetail t2 where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                    " t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.id=t2.parentid and t1.projectid='" + projectID + "' " +
                    " union all " +
                    " select '浇筑记录' bustype,to_char(t1.id) id from thd_pouringnotemaster t1,thd_pouringnotedetail t2 where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                    " t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.id=t2.parentid and t1.projectid='" + projectID + "'";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            string stockinbillstr = "";
            int stockinbillcount = 0;
            int stockindtlcount = 0;
            string stockoutbillstr = "";
            int stockoutbillcount = 0;
            int stockoutdtlcount = 0;
            string stockbalbillstr = "";
            int stockbalbillcount = 0;
            int stockbaldtlcount = 0;
            string mcollbillstr = "";
            int mcollbillcount = 0;
            int mcolldtlcount = 0;
            string mreturnbillstr = "";
            int mreturnbillcount = 0;
            int mreturndtlcount = 0;
            string pouringbillstr = "";
            int pouringbillcount = 0;
            int pouringdtlcount = 0;
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string id = TransUtil.ToString(dataRow["id"]);
                    string bustype = TransUtil.ToString(dataRow["bustype"]);
                    if (bustype == "收料")
                    {
                        if (!stockinbillstr.Contains(id))
                        {
                            stockinbillcount++;
                            stockinbillstr += "，" + id;
                        }
                        stockindtlcount++;
                    }
                    if (bustype == "领料")
                    {
                        if (!stockoutbillstr.Contains(id))
                        {
                            stockoutbillcount++;
                            stockoutbillstr += "，" + id;
                        }
                        stockoutdtlcount++;
                    }
                    if (bustype == "结算")
                    {
                        if (!stockbalbillstr.Contains(id))
                        {
                            stockbalbillcount++;
                            stockbalbillstr += "，" + id;
                        }
                        stockbaldtlcount++;
                    }
                    if (bustype == "料具收料")
                    {
                        if (!mcollbillstr.Contains(id))
                        {
                            mcollbillcount++;
                            mcollbillstr += "，" + id;
                        }
                        mcolldtlcount++;
                    }
                    if (bustype == "料具退料")
                    {
                        if (!mreturnbillstr.Contains(id))
                        {
                            mreturnbillcount++;
                            mreturnbillstr += "，" + id;
                        }
                        mreturndtlcount++;
                    }
                    if (bustype == "浇筑记录")
                    {
                        if (!pouringbillstr.Contains(id))
                        {
                            pouringbillcount++;
                            pouringbillstr += "，" + id;
                        }
                        pouringdtlcount++;
                    }
                }
            }
            materialinfo += "在此期间[收料入库单：" + stockinbillcount + "张]，其中[入库明细" + stockindtlcount + "条]，" +
                            "[领料出库单：" + stockoutbillcount + "张]，其中[出库明细" + stockoutdtlcount + "条]，" +
                            "[验收结算单：" + stockbalbillcount + "张]，其中[结算明细" + stockbaldtlcount + "条]，" +
                            "[料具收料单：" + mcollbillcount + "张]，其中[收料明细" + mcolldtlcount + "条]，" +
                            "[料具退料单：" + mreturnbillcount + "张]，其中[退料明细" + mreturndtlcount + "条]，" +
                            "[商品砼浇筑记录单：" + pouringbillcount + "张]，其中[浇筑记录明细" + pouringdtlcount + "条]； ";

            sql = " select t1.id from thd_materialsettlemaster t1, thd_materialsettledetail t2 " +
                    " where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                    " t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and  t1.id=t2.parentid " +
                    " and t1.projectid='" + projectID + "'";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            string materialsetbillstr = "";
            int materialsetbillcount = 0;
            int materialsetdtlcount = 0;
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string id = TransUtil.ToString(dataRow["id"]);
                    if (!materialsetbillstr.Contains(id))
                    {
                        materialsetbillcount++;
                        materialsetbillstr += "，" + id;
                    }
                    materialsetdtlcount++;
                }
            }
            materialinfo += "生成[月度物资实际耗用单：" + materialsetbillcount + "张]， 其中[实际耗用明细：" + materialsetdtlcount + "条]";
            ht.Add("materialinfo", materialinfo);
            #endregion

            #region 分包结算
            string subbalinfo = "";
            string subbalbillstr = "";
            int subbalbillcount = 0;
            int subbaldtlcount = 0;
            sql = " select t1.id from thd_subcontractbalancebill t1, thd_subcontractbalancedetail t2 " +
                    " where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                    " t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and  t1.id=t2.parentid " +
                    " and t1.projectid='" + projectID + "'";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string id = TransUtil.ToString(dataRow["id"]);
                    if (!subbalbillstr.Contains(id))
                    {
                        subbalbillcount++;
                        subbalbillstr += "，" + id;
                    }
                    subbaldtlcount++;
                }
            }
            subbalinfo += "在此期间[分包结算单：" + subbalbillcount + "张]， 其中[分包结算明细：" + subbaldtlcount + "条]";
            ht.Add("subbalinfo", subbalinfo);
            #endregion

            #region 月度成本分析
            string costinfo = "";
            string otherexpenseinfo = "";
            string monthcostinfo = "";
            //商品砼结算、财务费用、机械费用、月度成本分析
            sql = " select '商品砼结算' bustype,to_char(t1.id) id from thd_concretebalancemaster t1,thd_concretebalancedetail t2 where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                    " t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.id=t2.parentid and t1.projectid='" + projectID + "' " +
                    " union all " +
                    " select '财务费用' bustype,to_char(t1.id) id from thd_expensessettlemaster t1,thd_expensessettledetail t2 where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                    " t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.id=t2.parentid and t1.projectid='" + projectID + "' " +
                    " union all " +
                    " select '设备费用' bustype,to_char(t1.id) id from thd_materialrentelsetmaster t1,thd_materialrentalsetdetail t2 where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                    " t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.id=t2.parentid and t1.projectid='" + projectID + "' ";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            string conbalbillstr = "";
            int conbalbillcount = 0;
            int conbaldtlcount = 0;
            string expsettlebillstr = "";
            int expsettlebillcount = 0;
            int expsettledtlcount = 0;
            string matrentalbillstr = "";
            int matrentalbillcount = 0;
            int matrentaldtlcount = 0;
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string id = TransUtil.ToString(dataRow["id"]);
                    string bustype = TransUtil.ToString(dataRow["bustype"]);
                    if (bustype == "商品砼结算")
                    {
                        if (!conbalbillstr.Contains(id))
                        {
                            conbalbillcount++;
                            conbalbillstr += "，" + id;
                        }
                        conbaldtlcount++;
                    }
                    if (bustype == "财务费用")
                    {
                        if (!expsettlebillstr.Contains(id))
                        {
                            expsettlebillcount++;
                            expsettlebillstr += "，" + id;
                        }
                        expsettledtlcount++;
                    }
                    if (bustype == "设备费用")
                    {
                        if (!matrentalbillstr.Contains(id))
                        {
                            matrentalbillcount++;
                            matrentalbillstr += "，" + id;
                        }
                        matrentaldtlcount++;
                    }
                }
            }
            otherexpenseinfo += "在此期间[商品砼结算单：" + conbalbillcount + "张]，其中[商品砼结算明细" + conbaldtlcount + "条]，" +
                            "[财务结算单：" + expsettlebillcount + "张]，其中[财务结算明细" + expsettledtlcount + "条]，" +
                            "[设备租赁费用单：" + matrentalbillcount + "张]，其中[设备租赁费用明细" + matrentaldtlcount + "条]； ";

            //通过日期查找会计期
            sql = " select t1.fiscalyear,t1.fiscalmonth from RESFISCALPERIODDET t1 where t1.enddate<= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') " +
                    " and t1.begindate>= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') ";
            string costsql = "";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int fiscalyear = TransUtil.ToInt(dataRow["fiscalyear"]);
                    int fiscalmonth = TransUtil.ToInt(dataRow["fiscalmonth"]);
                    if (costsql == "")
                    {
                        costsql += "and ( ( t1.kjn = " + fiscalyear + " and t1.kjy = " + fiscalmonth + ")";
                    }
                    else
                    {
                        costsql += " or ( t1.kjn = " + fiscalyear + " and t1.kjy = " + fiscalmonth + ")";
                    }
                }
            }
            if (costsql != "")
            {
                costsql += " ) ";
            }
            else
            {
                costsql += " and 0 = 1 ";
            }

            sql = " select t1.kjn,t1.kjy,t1.createtime,count(*) count from thd_costmonthaccount t1,thd_costmonthaccountdtl t2 where t1.id=t2.parentid and " +
                    " t1.theprojectguid='" + projectID + "' " + costsql + "group by t1.kjn,t1.kjy,t1.createtime ";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int kjn = TransUtil.ToInt(dataRow["kjn"]);
                    int kjy = TransUtil.ToInt(dataRow["kjy"]);
                    int count = TransUtil.ToInt(dataRow["count"]);
                    string createtime = TransUtil.ToDateTime(dataRow["createtime"]).ToShortDateString();
                    monthcostinfo += "在此期间[会计年：" + kjn + ",会计月：" + kjy
                        + ",在{" + createtime + "}生成了月度成本分析数据, 生成成本分析明细共：" + count + "条]；";
                }
            }
            costinfo = otherexpenseinfo + "\r\n" + monthcostinfo;
            ht.Add("costinfo", costinfo);
            #endregion

            #region 系统整体应用
            string useinfo = "";
            sql = "  select count(*) count from thd_logdata t1 where t1.operdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                    " t1.operdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.projectname = '" + projectName + "' group by t1.operperson ";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            int usercount = 0;
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int temp = TransUtil.ToInt(dataRow["count"]);
                    usercount++;
                }
            }
            decimal useRatio = 0;
            if (percount > 0)
            {
                useRatio = Decimal.Round(usercount * 100 / percount, 2);
            }
            useinfo += "在此期间在系统中[执行人员为：" + usercount + "人]， [系统人员设置为：" + percount + "人]， [人员使用率为：" + useRatio + "%] ";
            ht.Add("useinfo", useinfo);
            #endregion

            return ht;
        }

        /// <summary>
        ///  通过日期查询项目使用状态
        /// </summary>
        /// <param name="opgID">业务组织ID</param>
        /// <returns></returns>
        public IList QueryAllProjectStateInfo(string opgID, DateTime startDate, DateTime endDate)
        {
            IList list = new ArrayList();
            string okflag = "√";
            string noflag = "×";
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();

            #region 项目基本信息
            //项目基本信息、合同摘要
            string sql = "";
            if (opgID == "")
            {
                sql = "select  t1.id,t1.projectname,t1.ownerorgsyscode,t1.createdate,t1.qualityreword,t1.projectlocationdescript,t1.contractarea,t1.constractstage,t1.mappoint From resconfig t1 " +
                            " where nvl(t1.projectcurrstate,0)!=20 and t1.projecttype=0 order by substr(t1.ownerorgsyscode,0,length(t1.ownerorgsyscode)-33),t1.createdate desc ";
            }
            else
            {
                sql = "select  t1.id,t1.projectname,t1.ownerorgsyscode,t1.createdate,t1.qualityreword,t1.projectlocationdescript,t1.contractarea,t1.constractstage,t1.mappoint From resconfig t1 " +
                            " where instr(t1.ownerorgsyscode,'" + opgID + "')>0  and t1.projecttype=0 and nvl(t1.projectcurrstate,0)!=20 order by substr(t1.ownerorgsyscode,0,length(t1.ownerorgsyscode)-33),t1.createdate desc ";

            }
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DataDomain domain = new DataDomain();
                    domain.Name4 = TransUtil.ToString(dataRow["ownerorgsyscode"]);
                    domain.Name1 = TransUtil.ToString(dataRow["id"]);
                    domain.Name2 = TransUtil.ToString(dataRow["projectname"]);
                    DateTime createdate = TransUtil.ToDateTime(dataRow["createdate"]);
                    if (createdate > TransUtil.ToDateTime("2000-01-01"))
                    {
                        domain.Name3 = createdate.Year + "年";
                    }
                    else
                    {
                        domain.Name3 = "其他年份";
                    }
                    int tt = 0;
                    if (TransUtil.ToString(dataRow["qualityreword"]) != "")
                    {
                        tt++;
                    }
                    if (TransUtil.ToString(dataRow["projectlocationdescript"]) != "")
                    {
                        tt++;
                    }
                    if (TransUtil.ToString(dataRow["contractarea"]) != "")
                    {
                        tt++;
                    }
                    if (TransUtil.ToString(dataRow["constractstage"]) != "")
                    {
                        tt++;
                    }
                    if (TransUtil.ToString(dataRow["qualityreword"]) != "")
                    {
                        tt++;
                    }
                    if (TransUtil.ToString(dataRow["mappoint"]) != "")
                    {
                        tt++;
                    }
                    if (tt >= 3)
                    {
                        domain.Name6 = okflag;
                    }
                    else
                    {
                        domain.Name6 = noflag;
                    }
                    list.Add(domain);
                }
            }
            //归属分公司
            Hashtable subCompany_ht = new Hashtable();
            sql = "select t1.opgname,t1.opgid from resoperationorg t1 where t1.oldcode='1' ";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    subCompany_ht.Add(TransUtil.ToString(dataRow["opgid"]), TransUtil.ToString(dataRow["opgname"]));
                }
            }
            #endregion

            foreach (DataDomain domain in list)
            {
                int okcount = 0;
                if (domain.Name6.ToString().Contains(okflag))
                {
                    okcount++;
                }

                //归属分公司
                string ownSyscode = TransUtil.ToString(domain.Name4);
                bool ifSub = false;
                foreach (string opgid in subCompany_ht.Keys)
                {
                    if (ownSyscode.Contains(opgid))
                    {
                        string opgname = subCompany_ht[opgid] as string;
                        domain.Name4 = opgname;
                        ifSub = true;
                    }
                }
                if (ifSub == false)
                {
                    domain.Name4 = domain.Name2;
                }
                string projectID = TransUtil.ToString(domain.Name1);

                //人员岗位设置、签名图片
                sql = "  select count(t4.perid) count From resconfig t1,resoperationorg t2, " +
                       " resoperationjob t3,respersononjob t4,resoperationjobwithrole t6 " +
                       " where t1.ownerorg=t2.opgid and instr(t3.opjsyscode,t2.opgid) !=0 and t3.opjid=t4.operationjobid  " +
                       " and t3.opjid=t6.operationjob and t1.id='" + projectID + "'";
                command.CommandText = sql;
                dataReader = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
                if (ds != null)
                {
                    DataTable dataTable = ds.Tables[0];
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        int count = TransUtil.ToInt(dataRow["count"]);
                        if (count >= 5)
                        {
                            domain.Name5 = okflag;
                            okcount++;
                        }
                        else
                        {
                            domain.Name5 = noflag;
                        }

                    }
                }
                #region 施工部位划分
                sql = " select count(*) count from thd_pbstree t1 where t1.theprojectguid='" + projectID + "'";
                command.CommandText = sql;
                dataReader = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
                if (ds != null)
                {
                    DataTable dataTable = ds.Tables[0];
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        int count = TransUtil.ToInt(dataRow["count"]);
                        if (count >= 5)
                        {
                            domain.Name7 = okflag;
                            okcount++;
                        }
                        else
                        {
                            domain.Name7 = noflag;
                        }
                    }
                }
                #endregion

                #region 施工任务划分
                sql = " select count(t1.id) count from thd_gwbstree t1 where t1.taskstate in (0,1,5) and t1.theprojectguid='" + projectID + "'";
                command.CommandText = sql;
                dataReader = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
                if (ds != null)
                {
                    DataTable dataTable = ds.Tables[0];
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        int count = TransUtil.ToInt(dataRow["count"]);
                        if (count >= 5)
                        {
                            domain.Name8 = okflag;
                            okcount++;
                        }
                        else
                        {
                            domain.Name8 = noflag;
                        }
                    }
                }
                #endregion

                #region 成本信息维护
                //明细数量，成本核算数量，合同工程量、单价
                sql = " select count(t1.id) count " +
                        " from thd_gwbsdetail t1 where t1.state in (0,5) and t1.theprojectguid='" + projectID + "'";
                command.CommandText = sql;
                dataReader = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
                if (ds != null)
                {
                    DataTable dataTable = ds.Tables[0];
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        int count = TransUtil.ToInt(dataRow["count"]);
                        if (count >= 5)
                        {
                            domain.Name9 = okflag;
                            okcount++;
                        }
                        else
                        {
                            domain.Name9 = noflag;
                        }
                    }
                }
                #endregion

                #region 现场生产管理
                sql = " select count(t2.id) count from thd_gwbstaskconfirmmaster t1, thd_gwbstaskconfirmdetail t2 " +
                   " where t1.id=t2.parentid and t1.projectid = '" + projectID + "'";
                command.CommandText = sql;
                dataReader = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
                if (ds != null)
                {
                    DataTable dataTable = ds.Tables[0];
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        int count = TransUtil.ToInt(dataRow["count"]);
                        if (count >= 1)
                        {
                            domain.Name10 = okflag;
                            okcount++;
                        }
                        else
                        {
                            domain.Name10 = noflag;
                        }
                    }
                }
                if (TransUtil.ToString(domain.Name10) == noflag)
                {
                    //零星用工信息
                    sql = "  select count(t1.id) count From thd_laborsporadicmaster t1  where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') " +
                            "and t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.projectid='" + projectID + "'";
                    command.CommandText = sql;
                    dataReader = command.ExecuteReader();
                    ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
                    if (ds != null)
                    {
                        DataTable dataTable = ds.Tables[0];
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            int count = TransUtil.ToInt(dataRow["count"]);
                            if (count >= 1)
                            {
                                domain.Name10 = okflag;
                                okcount++;
                            }
                        }
                    }
                }
                #endregion

                #region 工程任务核算单
                sql = "  select count(t1.id) count from thd_projecttaskaccountbill t1 " +
                        " where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                        " t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.theprojectguid='" + projectID + "' ";
                command.CommandText = sql;
                dataReader = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
                if (ds != null)
                {
                    DataTable dataTable = ds.Tables[0];
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        int count = TransUtil.ToInt(dataRow["count"]);
                        if (count >= 1)
                        {
                            domain.Name11 = okflag;
                            okcount++;
                        }
                        else
                        {
                            domain.Name11 = noflag;
                        }
                    }
                }
                #endregion

                #region 物资管理
                //物资单据，收料单、领料单、结算单
                sql = " select '收料' bustype,count(t1.id) count from thd_stkstockin t1 where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                        " t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.projectid='" + projectID + "' " +
                        " union all " +
                        " select '领料' bustype,count(t1.id) count from thd_stkstockout t1 where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                        " t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.projectid='" + projectID + "' " +
                        " union all " +
                        " select '结算' bustype,count(t1.id) count from thd_stockinbalmaster t1 where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                        " t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd')  and t1.projectid='" + projectID + "' ";
                command.CommandText = sql;
                dataReader = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
                bool ifWZ = false;
                if (ds != null)
                {
                    DataTable dataTable = ds.Tables[0];
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        int count = TransUtil.ToInt(dataRow["count"]);
                        if (count >= 1)
                        {
                            ifWZ = true;
                        }

                    }
                }
                if (ifWZ == true)
                {
                    domain.Name12 = okflag;
                    okcount++;
                }
                else
                {
                    domain.Name12 = noflag;
                }
                #endregion

                #region 分包结算
                sql = " select count(t1.id) count from thd_subcontractbalancebill t1 " +
                        " where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                        " t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd')  and t1.projectid='" + projectID + "'";
                command.CommandText = sql;
                dataReader = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
                if (ds != null)
                {
                    DataTable dataTable = ds.Tables[0];
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        int count = TransUtil.ToInt(dataRow["count"]);
                        if (count >= 1)
                        {
                            domain.Name13 = okflag;
                            okcount++;
                        }
                        else
                        {
                            domain.Name13 = noflag;
                        }
                    }
                }
                #endregion

                #region 质安检查整改
                //整改通知单、罚款单、日常检查
                sql = " select count(t1.id) count from thd_rectificatnoticemaster t1  where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                        " t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.projectid='" + projectID + "'" +
                        " union all " +
                        " select count(t1.id) count from thd_penaltydeductionmaster t1 where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                        " t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.projectid='" + projectID + "'" +
                        " union all " +
                        "   select count(t1.id) count from thd_inspectionrecord t1 where t1.inspectiondate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                        " t1.inspectiondate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.projectid='" + projectID + "' ";
                bool ifCheck = false;
                command.CommandText = sql;
                dataReader = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
                if (ds != null)
                {
                    DataTable dataTable = ds.Tables[0];
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        int count = TransUtil.ToInt(dataRow["count"]);
                        if (count >= 1)
                        {
                            ifCheck = true;
                        }
                    }
                    if (ifCheck == true)
                    {
                        domain.Name14 = okflag;
                        okcount++;
                    }
                    else
                    {
                        domain.Name14 = noflag;
                    }
                }
                #endregion

                #region 财务费用结算
                sql = "select count(t1.id) count from thd_expensessettlemaster t1 where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                    " t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.projectid='" + projectID + "' ";
                command.CommandText = sql;
                dataReader = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
                if (ds != null)
                {
                    DataTable dataTable = ds.Tables[0];
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        int count = TransUtil.ToInt(dataRow["count"]);
                        if (count >= 1)
                        {
                            domain.Name15 = okflag;
                            okcount++;
                        }
                        else
                        {
                            domain.Name15 = noflag;
                        }
                    }
                }
                #endregion

                #region 设备租赁结算
                sql = " select count(t1.id) count from thd_materialrentelsetmaster t1 where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                    " t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.projectid='" + projectID + "' "; ;
                command.CommandText = sql;
                dataReader = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
                if (ds != null)
                {
                    DataTable dataTable = ds.Tables[0];
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        int count = TransUtil.ToInt(dataRow["count"]);
                        if (count >= 1)
                        {
                            domain.Name16 = okflag;
                            okcount++;
                        }
                        else
                        {
                            domain.Name16 = noflag;
                        }
                    }
                }
                #endregion

                #region 物资耗用结算
                sql = " select count(t1.id) count from thd_materialsettlemaster t1 " +
                        " where t1.createdate >= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and " +
                        " t1.createdate <= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.projectid='" + projectID + "'";
                command.CommandText = sql;
                dataReader = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
                if (ds != null)
                {
                    DataTable dataTable = ds.Tables[0];
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        int count = TransUtil.ToInt(dataRow["count"]);
                        if (count >= 1)
                        {
                            domain.Name17 = okflag;
                            okcount++;
                        }
                        else
                        {
                            domain.Name17 = noflag;
                        }
                    }
                }
                #endregion

                #region 月度成本分析
                //通过日期查找会计期
                sql = " select t1.fiscalyear,t1.fiscalmonth from RESFISCALPERIODDET t1 where t1.enddate<= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') " +
                        " and t1.begindate>= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') ";
                string costsql = "";
                command.CommandText = sql;
                dataReader = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
                if (ds != null)
                {
                    DataTable dataTable = ds.Tables[0];
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        int fiscalyear = TransUtil.ToInt(dataRow["fiscalyear"]);
                        int fiscalmonth = TransUtil.ToInt(dataRow["fiscalmonth"]);
                        if (costsql == "")
                        {
                            costsql += "and ( ( t1.kjn = " + fiscalyear + " and t1.kjy = " + fiscalmonth + ")";
                        }
                        else
                        {
                            costsql += " or ( t1.kjn = " + fiscalyear + " and t1.kjy = " + fiscalmonth + ")";
                        }
                    }
                }
                if (costsql != "")
                {
                    costsql += " ) ";
                }
                else
                {
                    costsql += " and 0 = 1 ";
                }

                sql = " select count(t1.id) count from thd_costmonthaccount t1 where t1.theprojectguid='" + projectID + "' " + costsql;
                command.CommandText = sql;
                dataReader = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
                if (ds != null)
                {
                    DataTable dataTable = ds.Tables[0];
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        int count = TransUtil.ToInt(dataRow["count"]);
                        if (count >= 1)
                        {
                            domain.Name18 = okflag;
                            okcount++;
                        }
                        else
                        {
                            domain.Name18 = noflag;
                        }
                    }
                }

                if (okcount >= 12)
                {
                    domain.Name30 = okflag;
                }
                else
                {
                    domain.Name30 = noflag;
                }

                #endregion
            }
            return list;
        }
        #endregion

        #region 项目综合数据统计
        /// <summary>
        ///  通过日期查询项目综合数据信息(公司生产会上使用)、各项目汇报系统应用和数据情况
        /// </summary>
        /// <returns></returns>
        public Hashtable QueryProjectDataInfo(string projectID, int kjn, int kjy)
        {
            Hashtable ht = new Hashtable();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();

            #region 商务信息
            //通过日期查找会计期
            //string sql = " select t1.fiscalyear,t1.fiscalmonth from RESFISCALPERIODDET t1 where t1.enddate<= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') " +
            //        " and t1.begindate>= to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') ";
            //string fiscalsql = "";
            //command.CommandText = sql;
            //IDataReader dataReader = command.ExecuteReader();
            //DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            //if (ds != null)
            //{
            //    DataTable dataTable = ds.Tables[0];
            //    foreach (DataRow dataRow in dataTable.Rows)
            //    {
            //        int fiscalyear = TransUtil.ToInt(dataRow["fiscalyear"]);
            //        int fiscalmonth = TransUtil.ToInt(dataRow["fiscalmonth"]);
            //        if (fiscalsql == "")
            //        {
            //            fiscalsql += "and ( ( t1.kjn = " + fiscalyear + " and t1.kjy = " + fiscalmonth + ")";
            //        }
            //        else
            //        {
            //            fiscalsql += " or ( t1.kjn = " + fiscalyear + " and t1.kjy = " + fiscalmonth + ")";
            //        }
            //    }
            //}
            //if (fiscalsql != "")
            //{
            //    fiscalsql += " ) ";
            //}

            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            string sql = " select t1.begindate,t1.enddate from RESFISCALPERIODDET t1 where t1.fiscalyear=" + kjn + " and t1.fiscalmonth= " + kjy;
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    startDate = TransUtil.ToDateTime(dataRow["begindate"]);
                    endDate = TransUtil.ToDateTime(dataRow["enddate"]);
                }
            }

            string currCostIdstr = "";//当期的月度成本ID连接
            sql = " select t1.id from thd_costmonthaccount t1 where t1.theprojectguid='" + projectID + "' and t1.kjn= " + kjn + " and t1.kjy = " + kjy;
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string id = TransUtil.ToString(dataRow["id"]);
                    currCostIdstr += id + "，";
                }
            }

            sql = "  select '分包结算' billtype,to_char(t1.monthaccbillid) monthbillid,t1.balancemoney money from thd_subcontractbalancebill t1 " +
                  "     where t1.monthaccbillid is not null and t1.projectid='" + projectID + "'" +
                  "  union all" +
                  "  select '材料消耗' billtype,to_char(t1.monthaccountbill) monthbillid,t1.summoney money from thd_materialsettlemaster t1 " +
                  "    where t1.monthaccountbill is not null and t1.projectid ='" + projectID + "' " +
                  "  union all " +
                  "  select '机械租赁' billtype,to_char(t1.monthaccountbillid) monthbillid,t1.summoney money from thd_materialrentelsetmaster t1 " +
                  "    where t1.monthaccountbillid is not null and t1.projectid ='" + projectID + "' " +
                  "  union all " +
                  "  select '财务费用' billtype,to_char(t1.monthlysettlment) monthbillid,t1.summoney money from thd_expensessettlemaster t1 " +
                  "    where t1.monthlysettlment is not null and t1.projectid='" + projectID + "'";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            decimal currSubMoney = 0;
            decimal currMateralMoney = 0;
            decimal currRentelMoney = 0;
            decimal currExpenseMoney = 0;
            decimal addSubMoney = 0;
            decimal addMateralMoney = 0;
            decimal addRentelMoney = 0;
            decimal addExpenseMoney = 0;

            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string billtype = TransUtil.ToString(dataRow["billtype"]);
                    string monthbillid = TransUtil.ToString(dataRow["monthbillid"]);
                    decimal money = TransUtil.ToDecimal(dataRow["money"]);
                    if (billtype == "分包结算")
                    {
                        if (currCostIdstr.Contains(monthbillid))
                        {
                            currSubMoney += money;
                        }
                        addSubMoney += money;
                    }
                    if (billtype == "材料消耗")
                    {
                        if (currCostIdstr.Contains(monthbillid))
                        {
                            currMateralMoney += money;
                        }
                        addMateralMoney += money;
                    }
                    if (billtype == "机械租赁")
                    {
                        if (currCostIdstr.Contains(monthbillid))
                        {
                            currRentelMoney += money;
                        }
                        addRentelMoney += money;
                    }
                    if (billtype == "财务费用")
                    {
                        if (currCostIdstr.Contains(monthbillid))
                        {
                            currExpenseMoney += money;
                        }
                        addExpenseMoney += money;
                    }
                }
            }
            DataDomain domain = new DataDomain();
            domain.Name1 = Decimal.Round(currSubMoney / 10000, 2) + "";
            domain.Name2 = Decimal.Round(currMateralMoney / 10000, 2) + "";
            domain.Name3 = Decimal.Round(currRentelMoney / 10000, 2) + "";
            domain.Name4 = Decimal.Round(currExpenseMoney / 10000, 2) + "";
            domain.Name5 = Decimal.Round(addSubMoney / 10000, 2) + "";
            domain.Name6 = Decimal.Round(addMateralMoney / 10000, 2) + "";
            domain.Name7 = Decimal.Round(addRentelMoney / 10000, 2) + "";
            domain.Name8 = Decimal.Round(addExpenseMoney / 10000, 2) + "";


            //业主报量
            sql = " select round(sum(t2.confirmmoney)/10000,2) sumconfirmmoney,round(sum(t2.collectionmoney)/10000,2) sumcollectionmoney " +
                     " from thd_ownerquantitymaster t1,thd_ownerquantitydetail t2 " +
                     " where t1.id=t2.parentid and t1.state=5 and t1.projectid='" + projectID + "' ";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    decimal sumconfirmmoney = TransUtil.ToDecimal(dataRow["sumconfirmmoney"]);
                    decimal sumcollectionmoney = TransUtil.ToDecimal(dataRow["sumcollectionmoney"]);
                    domain.Name12 = sumconfirmmoney + "";
                    domain.Name13 = sumcollectionmoney + "";
                }
            }
            ht.Add("businessinfo", domain);
            #endregion

            #region 物资信息
            DataDomain matSupplyDomain = new DataDomain();
            //当期、累计验收结算金额[钢筋:I110]
            decimal currSteelMoney = 0;
            decimal currConcreteMoney = 0;
            decimal currOtherMoney = 0;
            decimal addSteelMoney = 0;
            decimal addConcreteMoney = 0;
            decimal addOtherMoney = 0;
            sql = "   select t1.createdate,t2.materialcode,t2.money From thd_stockinbalmaster t1,thd_stockinbaldetail t2 where t1.id=t2.parentid and" +
                    " t1.state=5 and t1.projectid='" + projectID + "' and t1.createdate<= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') ";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string materialcode = TransUtil.ToString(dataRow["materialcode"]);
                    DateTime CreateDate = TransUtil.ToDateTime(dataRow["CreateDate"]);
                    decimal money = TransUtil.ToDecimal(dataRow["money"]);
                    if (materialcode.Contains("I110"))
                    {
                        if (CreateDate >= startDate)//当期
                        {
                            currSteelMoney += money;
                        }
                        addSteelMoney += money;
                    }
                    else
                    {
                        if (CreateDate >= startDate)//当期
                        {
                            currOtherMoney += money;
                        }
                        addOtherMoney += money;
                    }
                }
            }
            matSupplyDomain.Name1 = Decimal.Round(currSteelMoney / 10000, 2) + "";
            matSupplyDomain.Name2 = Decimal.Round(currConcreteMoney / 10000, 2) + "";
            matSupplyDomain.Name3 = Decimal.Round(currOtherMoney / 10000, 2) + "";
            matSupplyDomain.Name4 = Decimal.Round(addSteelMoney / 10000, 2) + "";
            matSupplyDomain.Name5 = Decimal.Round(addConcreteMoney / 10000, 2) + "";
            matSupplyDomain.Name6 = Decimal.Round(addOtherMoney / 10000, 2) + "";
            ht.Add("matsupplyinfo", matSupplyDomain);

            //领料出库消耗、料具租赁、废旧物资
            DataDomain matOutDomain = new DataDomain();
            decimal currSteelOutMoney = 0;
            decimal currConcreteOutMoney = 0;
            decimal currOtherOutMoney = 0;
            decimal currMateBalMoney = 0;
            decimal addSteelOutMoney = 0;
            decimal addConcreteOutMoney = 0;
            decimal addOtherOutMoney = 0;
            decimal addMateBalMoney = 0;
            sql = " select '领料出库' bustype,to_char(t2.materialcode) materialcode,t1.CreateDate, t2.money from thd_stkstockout t1,thd_stkstockoutdtl t2 where t1.id=t2.parentid and t1.projectid =  '"
                    + projectID + "' and t1.stockoutmanner in (20) and t1.istally=1 and t1.CreateDate<= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd')"
                + " union all "
                    + " select '料具租赁' bustype,'' materialcode,t1.CreateDate,t1.summatmoney money from thd_materialbalancemaster t1 " +
                    " where t1.CreateDate<= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd')  and t1.projectid='" + projectID + "'"
                + " union all "
                    + " select '废旧物资' bustype,to_char(t2.materialcode) materialcode,t1.CreateDate,-t2.totalvalue money from thd_wastematprocessmaster t1,thd_wastematprocessdetail t2 " +
                    " where t1.id=t2.parentid and t1.state=5 and t1.CreateDate<= to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd')  and t1.projectid='" + projectID + "'";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string bustype = TransUtil.ToString(dataRow["bustype"]);
                    string materialcode = TransUtil.ToString(dataRow["materialcode"]);
                    DateTime CreateDate = TransUtil.ToDateTime(dataRow["CreateDate"]);
                    decimal money = TransUtil.ToDecimal(dataRow["money"]);
                    if (bustype == "料具租赁")
                    {
                        if (CreateDate >= startDate)//当期
                        {
                            currMateBalMoney += money;
                        }
                        addMateBalMoney += money;
                    }
                    else
                    {
                        if (materialcode.Contains("I110"))
                        {
                            if (CreateDate >= startDate)//当期
                            {
                                currSteelOutMoney += money;
                            }
                            addSteelOutMoney += money;
                        }
                        else if (materialcode.Contains("I112"))
                        {
                            if (CreateDate >= startDate)//当期
                            {
                                currConcreteOutMoney += money;
                            }
                            addConcreteOutMoney += money;
                        }
                        else
                        {
                            if (CreateDate >= startDate)//当期
                            {
                                currOtherOutMoney += money;
                            }
                            addOtherOutMoney += money;
                        }
                    }
                }
            }
            matOutDomain.Name1 = Decimal.Round(currSteelOutMoney / 10000, 2) + "";
            matOutDomain.Name2 = Decimal.Round(currConcreteOutMoney / 10000, 2) + "";
            matOutDomain.Name3 = Decimal.Round(currMateBalMoney / 10000, 2) + "";
            matOutDomain.Name4 = Decimal.Round(currOtherOutMoney / 10000, 2) + "";
            matOutDomain.Name5 = Decimal.Round(addSteelOutMoney / 10000, 2) + "";
            matOutDomain.Name6 = Decimal.Round(addConcreteOutMoney / 10000, 2) + "";
            matOutDomain.Name7 = Decimal.Round(addMateBalMoney / 10000, 2) + "";
            matOutDomain.Name8 = Decimal.Round(addOtherOutMoney / 10000, 2) + "";
            ht.Add("materialoutinfo", matOutDomain);

            #endregion

            return ht;

        }
        #endregion

        #region 常用短语查询
        /// <summary>
        /// 常用短语查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetOftenWords(ObjectQuery objectQuery)
        {
            return Dao.ObjectQuery(typeof(OftenWord), objectQuery);
        }
        /// <summary>
        /// 查重
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="interphaseID"></param>
        /// <param name="controlID"></param>
        /// <param name="oftenWord"></param>
        /// <returns></returns>
        public OftenWord GetOftenWordByOftenWord(string userID, string interphaseID, string controlID, string oftenWord)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("UserID", userID));
            objectQuery.AddCriterion(Expression.Eq("InterphaseID", interphaseID));
            objectQuery.AddCriterion(Expression.Eq("ControlID", controlID));
            objectQuery.AddCriterion(Expression.Eq("OftenWords", oftenWord));
            IList list = GetOftenWords(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as OftenWord;
            }
            return null;
        }
        /// <summary>
        /// 保存常用短语
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public OftenWord SaveOftenWords(OftenWord obj)
        {
            return SaveOrUpdateByDao(obj) as OftenWord;
        }
        #endregion

        #region 公共方法
        /// <summary>
        ///  通过项目和角色查询人员信息
        /// </summary>
        /// <returns></returns>
        public IList GetPersonListByProjectAndRole(string projectId, string roleId)
        {
            IList list = new ArrayList();
            string sql = "select t4.perid,t4.pername,t4.percode,t4.states " +
                         " From resconfig t1,resoperationjob t2,respersononjob t3,resperson  t4, " +
                         " resoperationjobwithrole t5 " +
                         " where t1.ownerorg=t2.opjorgid and t2.opjid=t3.operationjobid " +
                         " and t2.opjid=t5.operationjob and t3.perid=t4.perid " +
                         " and t1.id='" + projectId + "'";
            if (TransUtil.ToString(roleId) != "")
            {
                sql += " and and t5.operationrole='" + roleId + "'";
            }
            sql += " group by t4.perid,t4.pername,t4.percode,t4.states ";
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    StandardPerson person = new StandardPerson();
                    person.Id = TransUtil.ToString(dataRow["perid"]);
                    person.Code = TransUtil.ToString(dataRow["percode"]);
                    person.Name = TransUtil.ToString(dataRow["pername"]);
                    person.State = TransUtil.ToInt(dataRow["states"]);
                    list.Add(person);
                }
            }
            return list;
        }

        public IList GetSubCompanys()
        {
            var objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("OperationType", "b"));

            return Dao.ObjectQuery(typeof (OperationOrg), objQuery);
        }

        #endregion

        public DataSet LoadPerson(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"select t1.opgname,t2.opjname,t4.pername,t6.rolename from resoperationorg t1 
                            join resoperationjob t2 on t1.opgid=t2.opjorgid
                            join respersononjob t3 on t2.opjid=t3.operationjobid
                            join resperson t4 on t3.perid=t4.perid
                            join resoperationjobwithrole t5 on t2.opjid=t5.operationjob
                            join resoperationrole t6 on t5.operationrole=t6.id";
            sql += " where t1.opgsyscode like " + "'%" + condition + "%'";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        public bool SaveMaterialInterfacePrice(IList lstMaterialInterfacePrice)
        {
            bool bFlag = false;
            try
            {
                bFlag = Dao.SaveOrUpdate(lstMaterialInterfacePrice);
            }
            catch
            {

            }
            return bFlag;
        }

        #region 项目台账
        public IList GetAllProjectLedger()
        {
            ObjectQuery objQuery = new ObjectQuery();
            objQuery.AddFetchMode("CreatePerson", FetchMode.Eager);
            objQuery.AddFetchMode("OperOrgInfo", FetchMode.Eager);

            return Dao.ObjectQuery(typeof(ProjectLedger), objQuery);
        }

        public ProjectLedger GetProjectLedgerById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            return Dao.Get(typeof(ProjectLedger), id) as ProjectLedger;
        }

        public ProjectLedger SaveProjectLedger(ProjectLedger obj)
        {
            Dao.SaveOrUpdate(obj);
            return obj;
        }

        public bool DeleteProjectLedger(ProjectLedger obj)
        {
            return Dao.Delete(obj);
        }

        public List<string> GetColumnValues(string col)
        {
            string sql = "select p.{0} from Thd_ProjectLedger p group by p.{0}";
            var ds = QueryDataToDataSet(string.Format(sql, col));
            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }

            var retList = new List<string>();
            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                retList.Add(ds.Tables[0].Rows[i][0].ToString());
            }

            return retList;
        }

        #endregion
    }

}
