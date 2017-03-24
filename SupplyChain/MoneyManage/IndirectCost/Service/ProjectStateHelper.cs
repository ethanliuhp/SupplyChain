using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using System.Runtime.Remoting.Messaging;
using System.Data;
using NHibernate;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Service
{
    enum CalulateRange
    {
        ThisMonth = 1,
        All = 2
    }

    public class ProjectStateHelper : IProjectStateHelper
    {

        private readonly List<string> indexList = new List<string>()
        { 
            "节点进度", "分包结算", "业主报量", "计时工占比", "物资验收结算", "料具租赁结算", "机械租赁结算", "财务费用结算"
        };

        #region fields and  properties
        private ISession _dbConfig;
        /// <summary>
        /// 数据库配置
        /// </summary>
        public ISession DbConfig
        {
            get
            {
                if (_dbConfig == null)
                {
                    _dbConfig = CallContext.GetData("nhsession") as ISession;
                }
                return _dbConfig;
            }
        }

        private Dictionary<string, CurrentProjectInfo> _projectList;
        /// <summary>
        /// 公司项目信息：
        ///     仅取字段Id，Name，opgsyscode，Data1，OwnerOrgName，OwnerOrgSysCode
        ///     Data1表示归属组织ID
        /// </summary>
        public Dictionary<string, CurrentProjectInfo> ProjectList
        {
            get
            {
                if (_projectList == null)
                {
                    _projectList = GetProjectList();
                }
                return _projectList;
            }
        }

        private VirtualMachine.Component.Util.IFCGuidGenerator _idGen;
        /// <summary>
        /// id生成器
        /// </summary>
        public VirtualMachine.Component.Util.IFCGuidGenerator IdGen
        {
            get
            {
                if (_idGen == null)
                {
                    _idGen = new VirtualMachine.Component.Util.IFCGuidGenerator();
                }
                return _idGen;
            }
        }
        #endregion


        #region ctor
        private DateTime _date;
        /// <summary>
        /// 获取或设置生成指标的截止日期
        /// </summary>
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private int CreateYear;
        private int CreateMonth;
        public ProjectStateHelper(DateTime CalDate)
        {
            this._date = CalDate.Date;
            GetFiscalPeriod();
        }
        #endregion

        #region 入口
        public void Create()
        {
            foreach (var item in ProjectList)
            {
                Clear(item.Key);

                foreach (var index in indexList)
                {
                    Create(index, item.Value);
                }
            }
        }
        #endregion

        #region 业务功能
        /// <summary>
        /// 清除历史数据
        /// </summary>
        private void Clear(string proid)
        {
            string sql = string.Format("delete from thd_fundmanagebyproject  t1 where t1.businesstype = 4 and t1.projectid ='{0}'  ", proid);
            ExecuteNonQuery(sql);

        }


        private void GetFiscalPeriod()
        {
            string sql = string.Format(@" select t1.fiscalyear as CreateYear,t1.fiscalmonth as CreateMonth  from resfiscalperioddet t1 where t1.begindate <= to_date('{0}','yyyy-MM-dd') and t1.enddate>= to_date('{0}','yyyy-MM-dd')",_date.ToString("yyyy-MM-dd"));
            DataTable dt = ExecuteTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                this.CreateYear = ClientUtil.ToInt(dt.Rows[0]["CreateYear"]);
                this.CreateMonth = ClientUtil.ToInt(dt.Rows[0]["CreateMonth"]);
            }
        }

        private void Create(string index, CurrentProjectInfo project)
        {
            FundManagementByProject fmbp = new FundManagementByProject();
            switch (index)
            {
                case "节点进度":
                    NodeSchedule( project);
                    break;
                case "分包结算":
                    Subcontractbalancebill(CalulateRange.All, project);
                    Subcontractbalancebill(CalulateRange.ThisMonth, project);
                    break;
                case "业主报量":
                    Ownerquantity(project);
                    break;
                case "计时工占比":
                    LaborsporadicRate(project);
                    break;
                case "物资验收结算":
                    StockinBal(CalulateRange.All, project);
                    StockinBal(CalulateRange.ThisMonth, project);
                    break;
                case "料具租赁结算":
                    MaterialBalance(CalulateRange.All, project);
                    MaterialBalance(CalulateRange.ThisMonth, project);
                    break;
                case "机械租赁结算":
                    MaterialRentelSet(CalulateRange.All, project);
                    MaterialRentelSet(CalulateRange.ThisMonth, project);
                    break;
                case "财务费用结算":
                    ExpensesSettle(CalulateRange.All, project);
                    ExpensesSettle(CalulateRange.ThisMonth, project);
                    break;


                default:
                    break;
            }
 
        }

      
       

        #region 施工节点进度
        private void NodeSchedule(CurrentProjectInfo project)
        {
            string sql = string.Format("select t1.gwbstreename,t1.taskcompletedpercent from thd_weekscheduledetail t1 where t1.projectid = '{0}' and t1.taskcompletedpercent < 100 and t1.actualbegindate > to_date('1900-01-01','yyyy-MM-dd')", project.Id);
            DataTable dt = ExecuteTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                var list_fmbp = new List<FundManagementByProject>(); 
                foreach (DataRow dr in dt.Rows)
                {
                    FundManagementByProject fmbp = new FundManagementByProject()
                    {
                        ID = IdGen.GeneratorIFCGuid(),
                        ProjectId = project.Id,
                        ProjectName = project.Name,
                        CreateDate = this._date,
                        OperationOrg = project.Data1,
                        OperOrgName = project.OwnerOrgName,
                        OpgSysCode = project.OwnerOrgSysCode,
                        OrganizationLevel = "项目",
                        Type = IndexType.ProjectState,
                        IndexName = "节点进度",
                        TimeName = "累计",
                        MeasurementUnitName = "百分比",
                        WarningLevelName = "正常",
                        NumericalValue = ClientUtil.ToDecimal(dr["taskcompletedpercent"]),
                        Descript = ClientUtil.ToString(dr["gwbstreename"])
                    };
                    list_fmbp.Add(fmbp);
                }

                InsertFundProject(list_fmbp);
            }
        }
        #endregion

        #region 分包结算金额
        private void Subcontractbalancebill(CalulateRange cr, CurrentProjectInfo project)
        {
            string sql = string.Format(@"select sum(t1.balancemoney) as Money from thd_subcontractbalancebill t1 where  t1.projectid = '{0}' ", project.Id);
            string sqlwhere = string.Format(@"  and t1.createyear = {0} and t1.createmonth = {1} ", this.CreateYear, this.CreateMonth);
            if (cr == CalulateRange.ThisMonth)
                sql += sqlwhere;
            object obj = ExecuteScalar(sql);
            if (obj != null)
            {
                FundManagementByProject fmbp = new FundManagementByProject()
                {
                    ID = IdGen.GeneratorIFCGuid(),
                    ProjectId = project.Id,
                    ProjectName = project.Name,
                    CreateDate = this._date,
                    OperationOrg = project.Data1,
                    OperOrgName = project.OwnerOrgName,
                    OpgSysCode = project.OwnerOrgSysCode,
                    OrganizationLevel = "项目",
                    Type = IndexType.ProjectState,
                    IndexName = "分包结算",
                    TimeName = cr == CalulateRange.ThisMonth ? "本月":"累计",
                    MeasurementUnitName = "元",
                    WarningLevelName = "正常",
                    NumericalValue = ClientUtil.ToDecimal(obj),
                };

                InsertFundProject(fmbp);
            }
        }
        #endregion 

        #region 累计业主报量
        private void Ownerquantity(CurrentProjectInfo project)
        {
            string sql = string.Format(@"select sum(t1.confirmsummoney) as Money from thd_ownerquantitymaster t1 where  t1.projectid = '{0}' ", project.Id);
           
            object obj = ExecuteScalar(sql);
            if (obj != null)
            {
                FundManagementByProject fmbp = new FundManagementByProject()
                {
                    ID = IdGen.GeneratorIFCGuid(),
                    ProjectId = project.Id,
                    ProjectName = project.Name,
                    CreateDate = DateTime.Now,
                    OperationOrg = project.Data1,
                    OperOrgName = project.OwnerOrgName,
                    OpgSysCode = project.OwnerOrgSysCode,
                    OrganizationLevel = "项目",
                    Type = IndexType.ProjectState,
                    IndexName = "业主报量",
                    TimeName =  "累计",
                    MeasurementUnitName = "元",
                    WarningLevelName = "正常",
                    NumericalValue = ClientUtil.ToDecimal(obj),
                };

                InsertFundProject(fmbp);
            }
        
        }

        #endregion

        #region 累计计时工占比
        private void LaborsporadicRate(CurrentProjectInfo project)
        {
            decimal money_All = Laborsporadic(CalulateRange.All, project);
            FundManagementByProject fmbp_All = new FundManagementByProject()
            {
                ID = IdGen.GeneratorIFCGuid(),
                ProjectId = project.Id,
                ProjectName = project.Name,
                CreateDate = DateTime.Now,
                OperationOrg = project.Data1,
                OperOrgName = project.OwnerOrgName,
                OpgSysCode = project.OwnerOrgSysCode,
                OrganizationLevel = "项目",
                Type = IndexType.ProjectState,
                IndexName = "计时工结算",
                TimeName = "累计",
                MeasurementUnitName = "元",
                WarningLevelName = "正常",
                NumericalValue = money_All,
            };
            InsertFundProject(fmbp_All);

            decimal money_DY = Laborsporadic(CalulateRange.ThisMonth, project);
            FundManagementByProject fmbp_DY = new FundManagementByProject()
            {
                ID = IdGen.GeneratorIFCGuid(),
                ProjectId = project.Id,
                ProjectName = project.Name,
                CreateDate = DateTime.Now,
                OperationOrg = project.Data1,
                OperOrgName = project.OwnerOrgName,
                OpgSysCode = project.OwnerOrgSysCode,
                OrganizationLevel = "项目",
                Type = IndexType.ProjectState,
                IndexName = "计时工结算",
                TimeName = "本月",
                MeasurementUnitName = "元",
                WarningLevelName = "正常",
                NumericalValue = money_DY,
            };
            InsertFundProject(fmbp_DY);

            decimal Rate = money_All == 0 ? 0 : money_DY / money_All * 100;
            FundManagementByProject fmbp = new FundManagementByProject()
            {
                ID = IdGen.GeneratorIFCGuid(),
                ProjectId = project.Id,
                ProjectName = project.Name,
                CreateDate = DateTime.Now,
                OperationOrg = project.Data1,
                OperOrgName = project.OwnerOrgName,
                OpgSysCode = project.OwnerOrgSysCode,
                OrganizationLevel = "项目",
                Type = IndexType.ProjectState,
                IndexName = "计时工占比",
                TimeName = "累计",
                MeasurementUnitName = "元",
                WarningLevelName = "正常",
                NumericalValue = ClientUtil.ToDecimal(Rate),
            };

            InsertFundProject(fmbp);

        }

        private decimal Laborsporadic(CalulateRange cr, CurrentProjectInfo project)
        {
            string sql = string.Format(@"  select sum(t2.accountsummoney) as Money from thd_laborsporadicmaster t1 
  left join thd_laborsporadicdetail t2 on t2.parentid = t1.id
   where t1.projectid = '{0}'
   and  t1.laborstate = '计时派工'
   ", project.Id);
            string sqlwhere = string.Format(@"  and t1.createyear = {0} and t1.createmonth = {1} ", this.CreateYear, this.CreateMonth);
            if (cr == CalulateRange.ThisMonth)
                sql += sqlwhere;
            object obj = ExecuteScalar(sql);
            if (obj == null)
                return (decimal)0;
            return ClientUtil.ToDecimal(obj);
        }


        #endregion

        #region 物资验收结算
        private void StockinBal(CalulateRange cr, CurrentProjectInfo project)
        {
            string sql = string.Format(@"select sum(t1.summoney) as Money from thd_stockinbalmaster t1 where  t1.projectid = '{0}' ", project.Id);
            string sqlwhere = string.Format(@"  and t1.createyear = {0} and t1.createmonth = {1} ", this.CreateYear, this.CreateMonth);
            if (cr == CalulateRange.ThisMonth)
                sql += sqlwhere;
            object obj = ExecuteScalar(sql);
            if (obj != null)
            {
                FundManagementByProject fmbp = new FundManagementByProject()
                {
                    ID = IdGen.GeneratorIFCGuid(),
                    ProjectId = project.Id,
                    ProjectName = project.Name,
                    CreateDate = this._date,
                    OperationOrg = project.Data1,
                    OperOrgName = project.OwnerOrgName,
                    OpgSysCode = project.OwnerOrgSysCode,
                    OrganizationLevel = "项目",
                    Type = IndexType.ProjectState,
                    IndexName = "物资验收结算",
                    TimeName = cr == CalulateRange.ThisMonth ? "本月" : "累计",
                    MeasurementUnitName = "元",
                    WarningLevelName = "正常",
                    NumericalValue = ClientUtil.ToDecimal(obj),
                };

                InsertFundProject(fmbp);
            }
            
        }
        #endregion

        #region 料具租赁计算
        private void MaterialBalance(CalulateRange cr, CurrentProjectInfo project)
        {
            string sql = string.Format(@"select sum(t1.summatmoney + t1.othermoney) as Money from thd_MaterialBalanceMaster t1 where  t1.projectid = '{0}' ", project.Id);
            string sqlwhere = string.Format(@"  and t1.createyear = {0} and t1.createmonth = {1} ", this.CreateYear, this.CreateMonth);
            if (cr == CalulateRange.ThisMonth)
                sql += sqlwhere;
            object obj = ExecuteScalar(sql);
            if (obj != null)
            {
                FundManagementByProject fmbp = new FundManagementByProject()
                {
                    ID = IdGen.GeneratorIFCGuid(),
                    ProjectId = project.Id,
                    ProjectName = project.Name,
                    CreateDate = this._date,
                    OperationOrg = project.Data1,
                    OperOrgName = project.OwnerOrgName,
                    OpgSysCode = project.OwnerOrgSysCode,
                    OrganizationLevel = "项目",
                    Type = IndexType.ProjectState,
                    IndexName = "料具租赁结算",
                    TimeName = cr == CalulateRange.ThisMonth ? "本月" : "累计",
                    MeasurementUnitName = "元",
                    WarningLevelName = "正常",
                    NumericalValue = ClientUtil.ToDecimal(obj),
                };

                InsertFundProject(fmbp);
            }
        }
        #endregion

        #region 机械租赁结算
        private void MaterialRentelSet(CalulateRange cr, CurrentProjectInfo project)
        {
            string sql = string.Format(@"select sum(t1.summatmoney) as Money from thd_MaterialBalanceMaster t1 where  t1.projectid = '{0}' ", project.Id);
            string sqlwhere = string.Format(@"  and t1.createyear = {0} and t1.createmonth = {1} ", this.CreateYear, this.CreateMonth);
            if (cr == CalulateRange.ThisMonth)
                sql += sqlwhere;
            object obj = ExecuteScalar(sql);
            if (obj != null)
            {
                FundManagementByProject fmbp = new FundManagementByProject()
                {
                    ID = IdGen.GeneratorIFCGuid(),
                    ProjectId = project.Id,
                    ProjectName = project.Name,
                    CreateDate = this._date,
                    OperationOrg = project.Data1,
                    OperOrgName = project.OwnerOrgName,
                    OpgSysCode = project.OwnerOrgSysCode,
                    OrganizationLevel = "项目",
                    Type = IndexType.ProjectState,
                    IndexName = "机械租赁结算",
                    TimeName = cr == CalulateRange.ThisMonth ? "本月" : "累计",
                    MeasurementUnitName = "元",
                    WarningLevelName = "正常",
                    NumericalValue = ClientUtil.ToDecimal(obj),
                };

                InsertFundProject(fmbp);
            }
        }
        #endregion

        #region 财务费用结算
        private void ExpensesSettle(CalulateRange cr, CurrentProjectInfo project)
        {
            string sql = string.Format(@"select sum(t1.summoney) as Money from thd_ExpensesSettleMaster t1 where  t1.projectid = '{0}' ", project.Id);
            string sqlwhere = string.Format(@"  and t1.createyear = {0} and t1.createmonth = {1} ", this.CreateYear, this.CreateMonth);
            if (cr == CalulateRange.ThisMonth)
                sql += sqlwhere;
            object obj = ExecuteScalar(sql);
            if (obj != null)
            {
                FundManagementByProject fmbp = new FundManagementByProject()
                {
                    ID = IdGen.GeneratorIFCGuid(),
                    ProjectId = project.Id,
                    ProjectName = project.Name,
                    CreateDate = this._date,
                    OperationOrg = project.Data1,
                    OperOrgName = project.OwnerOrgName,
                    OpgSysCode = project.OwnerOrgSysCode,
                    OrganizationLevel = "项目",
                    Type = IndexType.ProjectState,
                    IndexName = "财务费用结算",
                    TimeName = cr == CalulateRange.ThisMonth ? "本月" : "累计",
                    MeasurementUnitName = "元",
                    WarningLevelName = "正常",
                    NumericalValue = ClientUtil.ToDecimal(obj),
                };

                InsertFundProject(fmbp);
            }
        }
        #endregion


        /// <summary>
        /// 获取所有项目的基本信息
        /// </summary>
        private Dictionary<string, CurrentProjectInfo> GetProjectList()
        {
            string sql = "select id,projectname,projectcode,ownerorg,ownerorgname,ownerorgsyscode from resconfig where projectinfostate =1 ";// where nvl(projectcurrstate,0)!=20";
            var list = ExecuteList<CurrentProjectInfo>(sql, a => new CurrentProjectInfo()
            {
                Id = a["id"].ToString(),
                Name = a["projectname"].ToString(),
                Code = a["projectcode"].ToString(),
                Data1 = a["ownerorg"].ToString(),
                OwnerOrgName = a["ownerorgname"].ToString(),
                OwnerOrgSysCode = a["ownerorgsyscode"].ToString()
            });
            return ConvertDictionary(list, "Id");
        }

        /// <summary>
        /// 插入项目指标
        /// </summary>
        /// <param name="obj"></param>
        private void InsertFundProject(FundManagementByProject obj)
        {
            string sql = string.Format(@"insert into thd_fundmanagebyproject (Id,ProjectId,ProjectName,CreateDate,OperationOrg,OperOrgName,OpgSysCode,OrganizationLevel,IndexName, TimeName,MeasurementUnitName,WarningLevelName,NumericalValue,Descript,BusinessType) 
                                        values ('{0}','{1}','{2}',to_date('{3}','yyyy-mm-dd'),'{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12},'{13}',{14})"
                                                , obj.ID, obj.ProjectId, obj.ProjectName, obj.CreateDate.ToString("yyyy-MM-dd"), obj.OperationOrg, obj.OperOrgName, obj.OpgSysCode, obj.OrganizationLevel, obj.IndexName, obj.TimeName, obj.MeasurementUnitName, obj.WarningLevelName, obj.NumericalValue, obj.Descript, (int)obj.Type);
            ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 插入项目指标
        /// </summary>
        /// <param name="list"></param>
        private void InsertFundProject(IEnumerable<FundManagementByProject> list)
        {
            foreach (var item in list)
            {
                InsertFundProject(item);
            }
        }
        #endregion

        #region 非功能辅助方法

        /// <summary>
        /// 根据指定的属性字段，将list转化为dictionary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        private Dictionary<string, T> ConvertDictionary<T>(IEnumerable<T> list, string tag)
        {
            if (list.Count() == 0) return null;                     // 数列为空则直接返回
            var tmpl = list.FirstOrDefault();                       // 获取第一个元素
            var type = tmpl.GetType();                              // 获取当前类型信息
            var property = type.GetProperty(tag);                   // 获取该类型的指定属性信息
            var dic = new Dictionary<string, T>();
            foreach (var item in list)
            {
                var id = property.GetValue(item, null).ToString();  // 获取该属性的值
                if (!dic.ContainsKey(id))                           // 字典中不存在则添加
                {
                    dic.Add(id, item);
                }
            }
            return dic;
        }

        #endregion


        #region database oprate methed
        /// <summary>
        /// 访问数据库，返回指定类型的数据集合
        /// </summary>
        /// <typeparam name="T">用户类型</typeparam>
        /// <param name="sql">查询的sql语句</param>
        /// <param name="handler">格式化操作</param>
        /// <returns>数据集</returns>
        private List<T> ExecuteList<T>(string sql, Func<DataRow, T> handler)
        {
            IDbConnection conn = DbConfig.Connection;
            IDbCommand command = conn.CreateCommand();
            command.CommandText = sql;
            DbConfig.Transaction.Enlist(command);
            using (IDataReader dataReader = command.ExecuteReader())
            {
                DataTable dt = new DataTable();
                dt.Load(dataReader);
                if (dt.Rows.Count == 0) return new List<T>();
                // 根据用户参数，格式化输出
                return dt.Select().Select(handler).ToList();
            }
        }

        /// <summary>
        /// 访问数据库，返回DataTable
        /// </summary>
        /// <param name="sql">查询的sql语句</param>
        /// <returns>数据集</returns>
        private DataTable ExecuteTable(string sql)
        {
            IDbConnection conn = DbConfig.Connection;
            IDbCommand command = conn.CreateCommand();
            command.CommandText = sql;
            DbConfig.Transaction.Enlist(command);
            using (IDataReader dataReader = command.ExecuteReader())
            {
                DataTable dt = new DataTable();
                dt.Load(dataReader);
                return dt;
            }
        }

        /// <summary>
        /// 执行sql语句，无返回值
        /// </summary>
        private void ExecuteNonQuery(string sql)
        {
            IDbConnection conn = DbConfig.Connection;
            IDbCommand command = conn.CreateCommand();
            command.CommandText = sql;
            DbConfig.Transaction.Enlist(command);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// 执行sql语句，返回受影响行数
        /// </summary>
        private int ExecuteNonQuery(string sql, string tag)
        {
            IDbConnection conn = DbConfig.Connection;
            IDbCommand command = conn.CreateCommand();
            command.CommandText = sql;
            DbConfig.Transaction.Enlist(command);
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// 执行sql语句，返回受影响行数
        /// </summary>
        private object ExecuteScalar(string sql)
        {
            IDbConnection conn = DbConfig.Connection;
            IDbCommand command = conn.CreateCommand();
            command.CommandText = sql;
            DbConfig.Transaction.Enlist(command);
            return command.ExecuteScalar();
        }
        #endregion 
    }

}
