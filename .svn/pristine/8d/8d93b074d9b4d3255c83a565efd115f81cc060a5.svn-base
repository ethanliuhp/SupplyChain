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

namespace Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Service
{
    public enum IndexType : int
    {
        /// <summary>
        /// 资金：0
        /// </summary>
        Fund = 0,
        /// <summary>
        /// 物资：1
        /// </summary>
        Material = 1,
        /// <summary>
        /// 商务：2
        /// </summary>
        Business = 2,
        /// <summary>
        /// 工程：3
        /// </summary>
        Project = 3,
        /// <summary>
        /// 项目状态
        /// </summary>
        ProjectState=4

        
    }
    public class Index : IIndex
    {
        #region 构造函数
        public Index(DateTime date)
        {
            Date = date;
        }
        #endregion

        #region 字段/属性
        private readonly string[] times = { "本日", "本月", "本年", "累计", "日均", "月均", "上年", "月末", "上月" };
        private readonly string[] units = { "元", "个", "百分比" };
        private readonly string[] warns = { "正常", "黄色预警", "橙色预警", "红色预警" };
        private readonly List<string> indexList = new List<string>()
        { 
            "计时工", "代工扣款", "对外报量", "钢筋库存", "结存额度", "主材使用", "主材平均采购价格", "停工项目"
        };
        private DateTime _date;
        /// <summary>
        /// 获取或设置生成指标的截止日期
        /// </summary>
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }
        /// <summary>
        /// 当月开始日期
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                var curTime = Date.AddMonths(-1);
                var day = 28;
                if (curTime.Year % 4 == 0 && curTime.Year % 100 != 100)
                {
                    day = 29;
                }
                return new DateTime(curTime.Year, curTime.Month, day);
            }
        }
        /// <summary>
        /// 当月截止日期
        /// </summary>
        public DateTime EndDate
        {
            get
            {
                return new DateTime(Date.Year, Date.Month, 28);

            }
        }
        /// <summary>
        /// 本年第一天
        /// </summary>
        public DateTime FirstDay
        {
            get
            {
                return new DateTime(Date.Year, 1, 1);
            }
        }
        private Dictionary<string, OperationOrgInfo> _orgList;
        /// <summary>
        /// 分公司信息：
        ///     仅取字段Id，Name，Code
        /// </summary>
        public Dictionary<string, OperationOrgInfo> OrgList
        {
            get
            {
                if (_orgList == null)
                {
                    _orgList = GetOrgList();
                }
                return _orgList;
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

        #region 业务方法
        /// <summary>
        /// 根据默认的经营指标，生成统计信息
        /// </summary>
        public void Create()
        {
            Clear();        // 清除本日已生成的记录
            foreach (var index in indexList)
            {
                Create(index);
            }
        }
        /// <summary>
        /// 根据指定的指标名称生成统计信息
        /// </summary>
        /// <param name="index"></param>
        public void Create(string index)
        {
            switch (index)
            {
                case "计时工":
                    CreatePartTimer();
                    break;
                case "代工扣款":
                    CreateOEMDeduction();
                    break;
                case "对外报量":
                    CreateReporting();
                    break;
                case "钢筋库存":
                    CreateSteelBarStock();
                    break;
                case "结存额度":
                    CreateSteelBarBalance();
                    break;
                case "主材使用":
                    CreateMainMaterialUse();
                    break;
                case "主材平均采购价格":
                    CreateMainMaterialPrice();
                    break;
                case "停工项目":
                    CreateShutdown();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 计时工统计分析
        /// </summary>
        private void CreatePartTimer()
        {
            string indexName = "计时工";
            // 获取截止到今日的零星用工单记录，并格式化
            string sql = string.Format("select t1.createdate, t1.projectid, t1.projectname, t2.balancetotalprice money, t3.balancesubjectcode code from thd_subcontractbalancebill t1, thd_subcontractbalancedetail t2, thd_subcontractbalsubjectdtl t3 where t1.id=t2.parentid and t2.id=t3.parentid and t2.fontbilltype=4 and t1.createdate<=to_date('{0}','yyyy-mm-dd') and state=5", Date.ToString("yyyy-MM-dd"));
            var list = ExecuteList(sql, a => new MediumData()
            {
                CreateDate = Convert.ToDateTime(a["createdate"]),
                Id = a["projectid"].ToString(),
                Name = a["projectname"].ToString(),
                Money = Convert.ToDecimal(a["money"]),
                IndexName = indexName,
                Other = a["code"].ToString()
            });

            #region 本月
            // 获取本月发生的零星用工记录
            var curMonthRecord = list.Where(a => a.CreateDate >= StartDate && a.CreateDate <= EndDate);
            // 项目
            var monthProject = CalcProjectIndex(curMonthRecord, times[1], units[0], warns[0]);
            InsertFundProject(monthProject);                          // 插入数据库
            // 分公司
            var monthSub = CalcSubIndex(monthProject, indexName, times[1], units[0], warns[0]);     // 获取所有分公司本月累计的零星用工指标
            InsertFundManagement(monthSub);
            #endregion

            #region 累计
            // 项目
            var allProject = CalcProjectIndex(list, times[3], units[0], warns[0]);    // 获取所有项目累计的零星用工金额
            InsertFundProject(allProject);              // 插入数据库
            // 分公司
            var allSub = CalcSubIndex(allProject, indexName, times[3], units[0], warns[0]); // 获取所有分公司累计的零星用工记录
            InsertFundManagement(allSub);
            #endregion

            #region 累计劳务费
            var laborMoney = list.Where(a => a.Other.Contains("C51101"));  // 获取所有劳务费记录
            var laborProjects = CalcProjectIndex(laborMoney, null, null, null);       // 计算各项目劳务费用合计
            var laborDic = ConvertDictionary(laborProjects, "ProjectId");  // 转化为数据字典
            //  项目
            var laborList = new List<FundManagementByProject>();    // 存储项目劳务费占用比
            foreach (var item in allProject)                        // 遍历所有项目
            {
                if (!laborDic.ContainsKey(item.ProjectId)) continue;// 不存在劳务费，不做统计
                decimal moneyLabor = laborDic[item.ProjectId].NumericalValue; // 劳务费用
                decimal tag = item.NumericalValue == 0 ? 0 : Math.Round(moneyLabor / item.NumericalValue, 4) * 100;
                var laborRecord = new FundManagementByProject()     // 生成劳务费用占比记录
                {
                    ID = IdGen.GeneratorIFCGuid(),
                    ProjectId = item.ID,
                    ProjectName = item.ProjectName,
                    CreateDate = Date,
                    OperationOrg = item.OperationOrg,
                    OperOrgName = item.OperOrgName,
                    OpgSysCode = item.OpgSysCode,
                    OrganizationLevel = "项目",
                    Type = IndexType.Business,
                    IndexName = item.IndexName,
                    TimeName = times[3],
                    MeasurementUnitName = units[2],
                    WarningLevelName = warns[0],
                    NumericalValue = tag
                };
                laborList.Add(laborRecord);
            }
            InsertFundProject(laborList);
            //  分公司
            var laborSubs = CalcSubIndex(laborProjects, indexName, times[3], units[2], warns[0]);
            var laborSubDic = ConvertDictionary(laborSubs, "OperationOrg");   // 根据id转化为数据字典
            var laborSubList = new List<FundManagement>();          // 存储分公司劳务费占用比
            foreach (var item in allSub)
            {
                decimal moneyLabor = 0;                             // 劳务费用
                if (laborSubDic.ContainsKey(item.OperationOrg))
                {
                    moneyLabor = laborSubDic[item.OperationOrg].NumericalValue;
                }
                decimal tax = item.NumericalValue == 0 ? 0 : Math.Round(moneyLabor / item.NumericalValue, 4) * 100;
                var laborRecord = new FundManagement()              // 生成劳务费用占比记录
                {
                    ID = IdGen.GeneratorIFCGuid(),
                    CreateDate = Date,
                    OperationOrg = item.OperationOrg,
                    OperOrgName = item.OperOrgName,
                    OpgSysCode = item.OpgSysCode,
                    OrganizationLevel = "分公司",
                    Type = IndexType.Business,
                    IndexName = item.IndexName,
                    TimeName = times[3],
                    MeasurementUnitName = units[2],
                    WarningLevelName = warns[0],
                    NumericalValue = tax
                };
                laborSubList.Add(laborRecord);
            }
            InsertFundManagement(laborSubList);
            #endregion

            #region 公司合计
            var totalMonth = CalcCompanyIndex(monthSub, indexName, times[1], units[0], warns[0]);
            InsertFundManagement(totalMonth);   // 公司本月计时工金额合计
            var totalAll = CalcCompanyIndex(allSub, indexName, times[3], units[0], warns[0]);
            InsertFundManagement(totalAll);     // 累计计时工金额合计
            var totalTax = CalcCompanyIndex(null, indexName, times[3], units[2], warns[0]);
            var totalLaborMoney = laborSubs.Sum(a => a.NumericalValue);  // 累计所有劳务费
            var laborTax = Math.Round(totalLaborMoney / totalAll.NumericalValue, 4) * 100;  // 劳务费比例
            totalTax.NumericalValue = laborTax;
            InsertFundManagement(totalTax);     // 累计劳务费占比
            #endregion
        }
        /// <summary>
        /// 代工扣款统计分析
        /// </summary>
        private void CreateOEMDeduction()
        {
            string indexName = "代工扣款";
            string sql = string.Format("select t1.createdate, t1.projectid, t1.projectname, t2.balancetotalprice money, t2.fontbilltype type from thd_subcontractbalancebill t1, thd_subcontractbalancedetail t2 where t1.id=t2.parentid and t2.fontbilltype in (5,3) and t1.createdate<=to_date('{0}','yyyy-mm-dd') and state=5", Date.ToString("yyyy-MM-dd"));
            var list = ExecuteList(sql, a => new MediumData()
            {
                CreateDate = Convert.ToDateTime(a["createdate"]),
                Id = a["projectid"].ToString(),
                Name = a["projectname"].ToString(),
                Money = Convert.ToDecimal(a["money"]),
                IndexName = indexName,
                Other = a["type"] + ""
            });
            var OEMList = list.Where(a => a.Other == "5");
            var debitList = list.Where(a => a.Other == "3");

            #region 本月代工单
            var curMonthRecord = OEMList.Where(a => a.CreateDate >= StartDate && a.CreateDate <= EndDate);
            var monthProject = CalcProjectIndex(curMonthRecord, times[1], units[0], warns[0]);
            InsertFundProject(monthProject);
            var monthSub = CalcSubIndex(monthProject, indexName, times[1], units[0], warns[0]);
            InsertFundManagement(monthSub);
            #endregion

            #region 累计代工单
            var allProject = CalcProjectIndex(OEMList, times[3], units[0], warns[0]);
            InsertFundProject(allProject);
            var allSub = CalcSubIndex(allProject, indexName, times[3], units[0], warns[0]);
            InsertFundManagement(allSub);
            #endregion

            #region 累计已扣款
            string indexDebit = "代工已扣款";
            var debitProject = CalcProjectIndex(debitList, indexDebit, times[3], units[0], warns[0]);
            InsertFundProject(debitProject);
            var debitSub = CalcSubIndex(debitProject, indexDebit, times[3], units[0], warns[0]);
            InsertFundManagement(debitSub);
            #endregion

            #region 公司合计
            var totalMonth = CalcCompanyIndex(monthSub, indexName, times[1], units[0], warns[0]);
            InsertFundManagement(totalMonth);   // 公司本月代工金额合计
            var totalAll = CalcCompanyIndex(allSub, indexName, times[3], units[0], warns[0]);
            InsertFundManagement(totalAll);     // 累计代工金额合计
            var totalDebit = CalcCompanyIndex(debitSub, indexDebit, times[3], units[0], warns[0]);
            InsertFundManagement(totalDebit);   // 累计已扣款合计
            #endregion
        }
        /// <summary>
        /// 对外报量统计
        /// </summary>
        private void CreateReporting()
        {
            string indexName = "对外报量";
            /*                  数据获取                    */
            string sqlBook = "select t1.projectid,nvl(sum(mainbusinesstax+materialremain+tempdeviceremain+lowvalueconsumableremain+exchangematerialremain+personcost+materialcost+mechanicalcost+otherdirectcost+indirectcost+contractgrossprofit),0) as fiscal,nvl(sum(buscostsure),0) as business from thd_financemultdatamaster t1,thd_financemultdatadetail t2, resconfig t3 where t1.id=t2.parentid and t1.state=5 and t1.projectid=t3.id group by t1.projectid";   // 项目财务账面信息
            string sqlReport = "select createdate,projectid,submitsumquantity reportmoney,confirmmoney from thd_ownerquantitymaster t1, thd_ownerquantitydetail t2 where t1.id=t2.parentid and t1.state=5"; // 业主报量信息
            var startDate = new DateTime(Date.AddMonths(-1).Year, Date.AddMonths(-1).Month, 11);
            var endDate = new DateTime(Date.Year, Date.Month, 10);
            var bookList = ExecuteList(sqlBook, a => new ProjectBook()  // 项目账面信息列表
            {
                Id = a["projectid"].ToString(),
                Fiscal = Convert.ToDecimal(a["fiscal"]),
                Business = Convert.ToDecimal(a["business"])
            });
            bookList = bookList.Where(a => a.Money > 0).ToList();   // 排除账面信息财务、商务均为0的项目
            var reportList = ExecuteList(sqlReport, a => new            // 所有业主报量记录
            {
                CreateDate = Convert.ToDateTime(a["createdate"]),
                Id = a["projectid"].ToString(),
                ReportMoney = Convert.ToDecimal(a["reportmoney"]),
                ConfirmMoney = Convert.ToDecimal(a["confirmmoney"])
            });
            //===========================================================
            /*                  统计计算                */

            // 项目
            var monthRecord = reportList.Where(a => a.CreateDate >= startDate && a.CreateDate <= endDate);   // 当月记录
            var monthReport = new List<MediumData>();       // 当月各项目报量合计列表
            var monthConfirm = new List<MediumData>();      // 当月各项目确认合计列表
            var allConfirm = new List<MediumData>();        // 各项目累计确认金额列表
            var taxList = new List<MediumData>();           // 各项目确权率列表
            foreach (var book in bookList)      // 遍历项目账面信息
            {
                var projectRecord = monthRecord.Where(a => a.Id == book.Id);// 当月项目报量记录
                var reportMoney = projectRecord.Sum(a => a.ReportMoney);    // 本月报送金额
                var confirmMoney = projectRecord.Sum(a => a.ConfirmMoney);  // 本月确认金额
                var allMoney = reportList.Where(a => a.Id == book.Id).Sum(a => a.ConfirmMoney);

                monthReport.Add(new MediumData() { Id = book.Id, IndexName = indexName + "-报送", Money = reportMoney });
                monthConfirm.Add(new MediumData() { Id = book.Id, IndexName = indexName + "-确认", Money = confirmMoney });
                allConfirm.Add(new MediumData() { Id = book.Id, IndexName = indexName, Money = allMoney });
                taxList.Add(new MediumData() { Id = book.Id, IndexName = indexName, Money = Math.Round(allMoney / book.Money, 4) * 100 });
            }
            // 计算各项目的对应指标，并写入数据库
            var monthReportProject = CalcProjectIndex(monthReport, times[1], units[0], warns[0]);
            var monthConfirmProject = CalcProjectIndex(monthConfirm, times[1], units[0], warns[0]);
            var allProject = CalcProjectIndex(allConfirm, times[3], units[0], warns[0]);
            var taxProject = CalcProjectIndex(taxList, times[3], units[2], warns[0]);
            InsertFundProject(monthReportProject);
            InsertFundProject(monthConfirmProject);
            InsertFundProject(allProject);
            InsertFundProject(taxProject);

            // 分公司
            var monthReportSub = CalcSubIndex(monthReportProject, indexName + "-报送", times[1], units[0], warns[0]);
            var monthConfirmSub = CalcSubIndex(monthConfirmProject, indexName + "-确认", times[1], units[0], warns[0]);
            var allConfirmSub = CalcSubIndex(allProject, indexName, times[3], units[0], warns[0]);
            InsertFundManagement(monthReportSub);
            InsertFundManagement(monthConfirmSub);
            InsertFundManagement(allConfirmSub);
            // 统计各分公司的所有项目账面金额
            var allBook = CalcSubConfirm(bookList);
            var taxSub = new List<FundManagement>();
            foreach (var item in allBook.Keys)      // 计算各分公司确权率
            {
                var temp = allConfirmSub.Where(a => a.OperationOrg == item).FirstOrDefault();
                var money = temp == null ? 0 : temp.NumericalValue;
                var tax = allBook[item] == 0 ? 0 : Math.Round(money / allBook[item], 4) * 100;
                var target = temp.Clone() as FundManagement;
                target.NumericalValue = tax;
                target.MeasurementUnitName = units[2];
                taxSub.Add(target);
            }
            InsertFundManagement(taxSub);

            // 预警
            var yellow = taxProject.Where(a => a.NumericalValue > 120);
            var orange = taxProject.Where(a => a.NumericalValue > 90 && a.NumericalValue < 110);
            var red = taxProject.Where(a => a.NumericalValue < 90);
            var yellowIndex = CalcWarnIndex(yellow, times[3], warns[1]);
            var orangeIndex = CalcWarnIndex(orange, times[3], warns[2]);
            var redIndex = CalcWarnIndex(red, times[3], warns[3]);
            InsertFundManagement(yellowIndex);
            InsertFundManagement(orangeIndex);
            InsertFundManagement(redIndex);
        }
        /// <summary>
        /// 钢筋库存统计
        /// </summary>
        private void CreateSteelBarStock()
        {
            string indexName = "钢筋库存";
            string sqlIn = "select sum(t2.money) money, projectid from thd_stkstockin t1, thd_stkstockindtl t2 where t1.id=t2.parentid and createdate<=to_date('{0}','yyyy-mm-dd') and (instr(t2.materialcode,'I1100101')>0 or instr(t2.materialcode,'I1100102')>0 or instr(t2.materialcode,'I1100113')>0) group by projectid";    // 各项目钢筋入库总额
            string sqlOut = "select createdate, projectid, t2.money from thd_stkstockout t1, thd_stkstockoutdtl t2 where t1.id=t2.parentid and createdate<=to_date('{0}','yyyy-mm-dd') and (instr(t2.materialcode,'I1100101')>0 or instr(t2.materialcode,'I1100102')>0 or instr(t2.materialcode,'I1100113')>0)";                       // 每条钢筋出库记录
            var startDate = new DateTime(Date.AddMonths(-1).Year, Date.AddMonths(-1).Month, 11);
            var endDate = new DateTime(Date.Year, Date.Month, 10);
            var inTable = ExecuteList(string.Format(sqlIn, endDate.ToString("yyyy-MM-dd")), a => new MediumData()
            {
                Id = a["projectid"].ToString(),
                Money = a["money"] == DBNull.Value ? 0 : Convert.ToDecimal(a["money"])
            });
            var outTable = ExecuteList(string.Format(sqlOut, endDate.ToString("yyyy-MM-dd")), a => new MediumData()
            {
                CreateDate = Convert.ToDateTime(a["createdate"]),
                Id = a["projectid"].ToString(),
                Money = a["money"] == DBNull.Value ? 0 : Convert.ToDecimal(a["money"])
            });

            //==========================项目===========================
            // 各项目钢筋总入库
            var inProject = CalcProjectIndex(inTable, indexName, times[3], units[0], warns[0]);
            // 各项目钢筋总出库
            var outProject = CalcProjectIndex(outTable, null, null, null, null);
            // 各项目截止当月的钢筋总库存：进库-出库[没进库记录的项目不参与统计]
            var balanceProject = inProject.Select(item =>
            {
                var obj = item.Clone() as FundManagementByProject;
                var temp = outProject.Where(a => a.ProjectId == obj.ProjectId).FirstOrDefault();
                obj.NumericalValue = item.NumericalValue - (temp == null ? 0 : temp.NumericalValue);
                return obj;
            });
            InsertFundProject(balanceProject);
            // 各项目当月消耗钢筋金额
            var monthOutProject = CalcProjectIndex(outTable.Where(a => a.CreateDate >= startDate && a.CreateDate <= endDate), null, null, null, null);
            // 库存与月消耗占比
            var taxProject = balanceProject.Select(item =>
            {
                var obj = item.Clone() as FundManagementByProject;
                var temp = monthOutProject.Where(a => a.ProjectId == obj.ProjectId).FirstOrDefault();
                var num = (temp == null || temp.NumericalValue == 0) ? 0 : Math.Round(obj.NumericalValue / temp.NumericalValue, 4);
                obj.NumericalValue = num * 100;
                obj.TimeName = times[1];
                obj.MeasurementUnitName = units[2];
                return obj;
            });
            InsertFundProject(taxProject);          // 库存占比写入数据库
            //=========================================================
            //==========================分公司=========================
            var balanceSub = CalcSubIndex(balanceProject, indexName, times[3], units[0], warns[0]);
            InsertFundManagement(balanceSub);
            // 所占比例
            var monthOutSub = CalcSubIndex(monthOutProject, null, null, null, null);
            var TaxSub = balanceSub.Select(item =>
            {
                var obj = item.Clone() as FundManagement;
                var temp = monthOutSub.Where(a => a.OpgSysCode == obj.OpgSysCode).FirstOrDefault();
                var num = (temp == null || temp.NumericalValue == 0) ? 0 : Math.Round(obj.NumericalValue / temp.NumericalValue, 4);
                obj.NumericalValue = num * 100;
                obj.TimeName = times[1];
                obj.MeasurementUnitName = units[2];
                return obj;
            });
            InsertFundManagement(TaxSub);
            // 预警
            var warnProject = taxProject.Where(a => a.NumericalValue > 40);
            var warnSub = CalcWarnIndex(warnProject, times[1], warns[3]);
            InsertFundManagement(warnSub);
            //=========================================================
            //==========================公司===========================
            // 结存
            var balanceCompany = CalcCompanyIndex(balanceSub, indexName, times[3], units[0], warns[0]);
            InsertFundManagement(balanceCompany);
            var monthOutCompany = monthOutSub.Sum(a => a.NumericalValue);
            // 所占比例
            var taxCompany = balanceCompany.Clone() as FundManagement;
            taxCompany.NumericalValue = monthOutCompany == 0 ? 0 : Math.Round(balanceCompany.NumericalValue / monthOutCompany, 4) * 100;
            taxCompany.MeasurementUnitName = units[2];
            taxCompany.TimeName = times[1];
            InsertFundManagement(taxCompany);
            // 预警
            var warnCompany = balanceCompany.Clone() as FundManagement;
            warnCompany.NumericalValue = warnSub.Sum(a => a.NumericalValue);
            warnCompany.MeasurementUnitName = units[1];
            warnCompany.TimeName = times[1];
            warnCompany.WarningLevelName = warns[3];
            InsertFundManagement(warnCompany);
            //=========================================================
        }
        /// <summary>
        /// 结存额度统计
        /// </summary>
        private void CreateSteelBarBalance()
        {
            string indexName = "结存额度";
            string sqlIn = "select createdate, projectid, summoney from thd_stkstockin where createdate<=to_date('{0}','yyyy-mm-dd')";
            string sqlOut = "select projectid, sum(summoney) summoney from thd_stkstockout where createdate<=to_date('{0}','yyyy-mm-dd') group by projectid";
            var startDate = new DateTime(Date.AddMonths(-1).Year, Date.AddMonths(-1).Month, 11);
            var endDate = new DateTime(Date.Year, Date.Month, 10);

            var inTable = ExecuteList(string.Format(sqlIn, endDate.ToString("yyyy-MM-dd")), a => new MediumData()
            {
                CreateDate = Convert.ToDateTime(a["createdate"]),
                Id = a["projectid"].ToString(),
                Money = a["summoney"] == DBNull.Value ? 0 : Convert.ToDecimal(a["summoney"])
            });         // 项目进库表[每条入库记录]
            var outTable = ExecuteList(string.Format(sqlOut, endDate.ToString("yyyy-MM-dd")), a => new MediumData()
            {
                Id = a["projectid"].ToString(),
                Money = a["summoney"] == DBNull.Value ? 0 : Convert.ToDecimal(a["summoney"])
            });         // 项目出库表[汇总后的各项目]
            //==========================项目=============================
            // 各项目总进库量
            var inProject = CalcProjectIndex(inTable, indexName, times[3], units[0], warns[0]);
            // 各项目截止当月的总库存：进库-出库[没进库记录的项目不参与统计]
            var balanceProject = inProject.Select(item =>
            {
                var obj = item.Clone() as FundManagementByProject;
                var temp = outTable.Where(a => a.Id == obj.ProjectId).FirstOrDefault();
                obj.NumericalValue = item.NumericalValue - (temp == null ? 0 : temp.Money);
                return obj;
            });
            InsertFundProject(balanceProject);      // 总库存写入记录数据库
            // 当月进库
            var monthInProject = CalcProjectIndex(inTable.Where(a => a.CreateDate >= startDate && a.CreateDate <= endDate), indexName, null, null, null);
            // 库存占比
            var taxProject = balanceProject.Select(item =>
            {
                var obj = item.Clone() as FundManagementByProject;
                var temp = monthInProject.Where(a => a.ProjectId == obj.ProjectId).FirstOrDefault();
                var num = (temp == null || temp.NumericalValue == 0) ? 0 : Math.Round(obj.NumericalValue / temp.NumericalValue, 4);
                obj.NumericalValue = num * 100;
                obj.TimeName = times[1];
                obj.MeasurementUnitName = units[2];
                return obj;
            });
            InsertFundProject(taxProject);          // 库存占比写入数据库
            //============================================================
            //=========================分公司=============================
            var balanceSub = CalcSubIndex(balanceProject, indexName, times[3], units[0], warns[0]);
            InsertFundManagement(balanceSub);
            // 所占比例
            var monthInSub = CalcSubIndex(monthInProject, null, null, null, null);
            var TaxSub = balanceSub.Select(item =>
            {
                var obj = item.Clone() as FundManagement;
                var temp = monthInSub.Where(a => a.OpgSysCode == obj.OpgSysCode).FirstOrDefault();
                var num = (temp == null || temp.NumericalValue == 0) ? 0 : Math.Round(obj.NumericalValue / temp.NumericalValue, 4);
                obj.NumericalValue = num * 100;
                obj.TimeName = times[1];
                obj.MeasurementUnitName = units[2];
                return obj;
            });
            InsertFundManagement(TaxSub);
            // 预警
            var warnProject = taxProject.Where(a => a.NumericalValue > 40);
            var warnSub = CalcWarnIndex(warnProject, times[1], warns[3]);
            InsertFundManagement(warnSub);
            //============================================================
            //=========================公司===============================
            // 结存
            var balanceCompany = CalcCompanyIndex(balanceSub, indexName, times[3], units[0], warns[0]);
            InsertFundManagement(balanceCompany);
            var monthInCompany = monthInSub.Sum(a => a.NumericalValue);
            // 所占比例
            var taxCompany = balanceCompany.Clone() as FundManagement;
            taxCompany.NumericalValue = monthInCompany == 0 ? 0 : Math.Round(balanceCompany.NumericalValue / monthInCompany, 4) * 100;
            taxCompany.MeasurementUnitName = units[2];
            taxCompany.TimeName = times[1];
            InsertFundManagement(taxCompany);
            // 预警
            var warnCompany = balanceCompany.Clone() as FundManagement;
            warnCompany.NumericalValue = warnSub.Sum(a => a.NumericalValue);
            warnCompany.MeasurementUnitName = units[1];
            warnCompany.TimeName = times[1];
            warnCompany.WarningLevelName = warns[3];
            InsertFundManagement(warnCompany);
            //============================================================
        }
        /// <summary>
        /// 主材使用情况统计
        /// </summary>
        private void CreateMainMaterialUse()
        {
            string indexName = "主材使用";
            string sqlIn = "select t1.projectid,sum(t2.quantity) money,t2.materialcode from thd_stkstockin t1, thd_stkstockindtl t2 where t1.id=t2.parentid and t1.createdate>= to_date('{0}','yyyy-mm-dd') and t1.createdate<=to_date('{1}','yyyy-mm-dd') and t1.thestockinoutkind in (0,1) and (instr(t2.materialcode,'I1100101')>0 or instr(t2.materialcode,'I1100102')>0 or instr(t2.materialcode,'I1100113')>0 or instr(t2.materialcode,'I11201')>0 or instr(t2.materialcode,'I11101')>0 or instr(t2.materialcode,'I13303')>0 or instr(t2.materialcode,'I1330402')>0) group by projectid,materialcode";
            string sqlOut = "select t1.projectid,sum(quantity) money,t2.materialcode from thd_stkstockout t1, thd_stkstockoutdtl t2 where t1.id=t2.parentid and t1.createdate >= to_date('{0}','yyyy-mm-dd') and t1.createdate<=to_date('{1}','yyyy-mm-dd') and t1.thestockinoutkind in (0,1) and (instr(t2.materialcode,'I1100101')>0 or instr(t2.materialcode,'I1100102')>0 or instr(t2.materialcode,'I1100113')>0 or instr(t2.materialcode,'I11201')>0 or instr(t2.materialcode,'I11101')>0 or instr(t2.materialcode,'I13303')>0 or instr(t2.materialcode,'I1330402')>0) group by projectid,materialcode";
            var startDate = new DateTime(Date.AddMonths(-1).Year, Date.AddMonths(-1).Month, 11);
            var endDate = new DateTime(Date.Year, Date.Month, 10);
            var inTable = ExecuteList(string.Format(sqlIn, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd")), a => new MediumData()
            {
                Id = a["projectid"].ToString(),
                Money = a["money"] == DBNull.Value ? 0 : Convert.ToDecimal(a["money"]),
                Other = a["materialcode"].ToString()
            });     // 各项目本月采购主材料信息
            var outTable = ExecuteList(string.Format(sqlOut, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd")), a => new MediumData()
            {
                Id = a["projectid"].ToString(),
                Money = a["money"] == DBNull.Value ? 0 : Convert.ToDecimal(a["money"]),
                Other = a["materialcode"].ToString()
            });     // 各项目本月消耗主材料信息

            /*       统计每个主材的使用情况      */
            // 钢筋
            CreateMainMaterialUse(inTable, outTable, indexName + "-钢筋", a => a.Other.StartsWith("I1100101") || a.Other.StartsWith("I1100102") || a.Other.StartsWith("I1100113"));
            // 混凝土
            CreateMainMaterialUse(inTable, outTable, indexName + "-混凝土", a => a.Other.StartsWith("I11201"));
            // 水泥
            CreateMainMaterialUse(inTable, outTable, indexName + "-水泥", a => a.Other.StartsWith("I11101"));
            // 周转材料
            CreateMainMaterialUse(inTable, outTable, indexName + "-周转材料", a => a.Other.StartsWith("I13303") || a.Other.StartsWith("I1330402"));

        }
        /// <summary>
        /// 主材使用情况统计
        /// </summary>
        /// <param name="inTable">各项目主材采购</param>
        /// <param name="outTable">各项目主材消耗</param>
        /// <param name="indexName">指标名称</param>
        /// <param name="handler">筛选条件</param>
        private void CreateMainMaterialUse(IEnumerable<MediumData> inTable, IEnumerable<MediumData> outTable, string indexName, Func<MediumData, bool> handler)
        {
            var inIndexName = indexName + "-采购";
            var outIndexName = indexName + "-消耗";
            var curInTable = inTable.Where(handler);
            var curOutTable = outTable.Where(handler);
            // 项目
            var inProject = CalcProjectIndex(curInTable, inIndexName, times[1], units[1], warns[0]);
            var outProject = CalcProjectIndex(curOutTable, outIndexName, times[1], units[1], warns[0]);
            InsertFundProject(inProject);
            InsertFundProject(outProject);
            // 分公司
            var inSub = CalcSubIndex(inProject, inIndexName, times[1], units[1], warns[0]);
            var outSub = CalcSubIndex(outProject, outIndexName, times[1], units[1], warns[0]);
            InsertFundManagement(inSub);
            InsertFundManagement(outSub);
            // 公司
            InsertFundManagement(CalcCompanyIndex(inSub, inIndexName, times[1], units[1], warns[0]));
            InsertFundManagement(CalcCompanyIndex(outSub, outIndexName, times[1], units[1], warns[0]));
        }
        /// <summary>
        /// 主材平均采购价格统计
        /// </summary>
        private void CreateMainMaterialPrice()
        {
            string indexName = "主材平均采购价格";
            string sql = "select t1.projectid, sum(t2.quantity) quantity, sum(t2.money) money, t2.materialcode from thd_stkstockout t1,thd_stkstockoutdtl t2 where t1.id=t2.parentid and t1.createdate >= to_date('{0}','yyyy-mm-dd') and  t1.createdate<=to_date('{1}','yyyy-mm-dd') and t1.thestockinoutkind in (0,1) group by projectid, t2.materialcode";
            var startDate = new DateTime(Date.AddMonths(-1).Year, Date.AddMonths(-1).Month, 11);
            var endDate = new DateTime(Date.Year, Date.Month, 10);
            var table = ExecuteList(string.Format(sql, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd")), a => new MediumData()
            {
                Id = a["projectid"].ToString(),
                Num = Convert.ToDecimal(a["quantity"]),
                Money = Convert.ToDecimal(a["money"]),
                Other = a["materialcode"].ToString()
            });
            // 钢筋
            CreateMainMaterialPrice(table, indexName + "-钢筋", a => a.Other.StartsWith("I1100101") || a.Other.StartsWith("I1100102") || a.Other.StartsWith("I1100113"));
            // 混凝土
            CreateMainMaterialPrice(table, indexName + "-混凝土", a => a.Other.StartsWith("I11201"));
            // 水泥
            CreateMainMaterialPrice(table, indexName + "-水泥", a => a.Other.StartsWith("I11101"));
        }
        /// <summary>
        /// 主材平均采购价格统计
        /// </summary>
        /// <param name="table">本月主材采购情况表</param>
        /// <param name="indexName">指标名称</param>
        /// <param name="handler">筛选条件</param>
        private void CreateMainMaterialPrice(IEnumerable<MediumData> table, string indexName, Func<MediumData, bool> handler)
        {
            var data = table.Where(handler);
            // 项目
            var projectList = CalcProjectIndex(data, indexName, times[1], units[0], warns[0]);
            foreach (var item in projectList)
            {
                // 项目采购总数量
                var num = data.Where(a => a.Id == item.ProjectId).Sum(a => a.Num);
                item.TimeID = item.NumericalValue.ToString();   // 记录项目采购主材总金额
                item.Descript = num.ToString();                 // 记录每个项目的主材采购数量
                // 计算单价
                item.NumericalValue = num == 0 ? 0 : Math.Round(item.NumericalValue / num, 2);
            }
            InsertFundProject(projectList);     // 写入数据库
            // 分公司
            var subList = CalcSubIndex(projectList, indexName, times[1], units[0], warns[0]);
            decimal totalNum = 0;
            decimal totalMoney = 0;
            foreach (var item in subList)
            {
                // 分公司采购总数量
                var list = projectList.Where(a => a.OpgSysCode.Contains(item.OpgSysCode));
                var num = list.Sum(a => Convert.ToDecimal(a.Descript));
                var money = list.Sum(a => Convert.ToDecimal(a.TimeID));
                // 计算单价 
                item.NumericalValue = num == 0 ? 0 : Math.Round(money / num, 2);
                totalNum += num;
                totalMoney += money;
            }
            InsertFundManagement(subList);
            // 公司
            var companyIndex = CalcCompanyIndex(subList, indexName, times[1], units[0], warns[0]);
            companyIndex.NumericalValue = totalNum == 0 ? 0 : Math.Round(totalMoney / totalNum, 2);
            InsertFundManagement(companyIndex);
        }
        /// <summary>
        /// 计算各分公司的停工项目数
        /// </summary>
        private void CreateShutdown()
        {
            string indexName = "停工项目";
            string sql = "select ownerorgsyscode code from resconfig where projectcurrstate=2";
            var list = ExecuteList(sql, a => new FundManagementByProject() { OpgSysCode = a["code"].ToString(), NumericalValue = 1 });
            var sub = CalcSubIndex(list, indexName, times[0], units[1], warns[0]);
            sub.Select(a => { a.Type = IndexType.Project; return ""; });
            InsertFundManagement(sub);
            var company = CalcCompanyIndex(sub, indexName, times[0], units[1], warns[0]);
            company.Type = IndexType.Project;
            InsertFundManagement(company);
        }


        /// <summary>
        /// 计算每个分公司所有的项目账面金额
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private Dictionary<string, decimal> CalcSubConfirm(IEnumerable<ProjectBook> list)
        {
            var dic = new Dictionary<string, decimal>();
            foreach (var item in OrgList.Keys)
            {
                var money = list.Where(a => ProjectList[a.Id].OwnerOrgSysCode.Contains(OrgList[item].SysCode)).Sum(a => a.Money);
                dic.Add(item, money);
            }
            return dic;
        }

        /// <summary>
        /// 清楚指定日期的指标记录
        /// </summary>
        private void Clear()
        {
            string sql = string.Format("delete thd_fundmanagement where businesstype={0} and createdate=to_date('{1}','yyyy-mm-dd')", (int)IndexType.Business, Date.ToString("yyyy-MM-dd"));
            ExecuteNonQuery(sql);
            sql = string.Format("delete thd_fundmanagebyproject where businesstype={0} and createdate=to_date('{1}','yyyy-mm-dd')", (int)IndexType.Business, Date.ToString("yyyy-MM-dd"));
            ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 计算项目指标
        /// </summary>
        /// <param name="list">需要统计的列表</param>
        /// <param name="indexName">指标名称</param>
        /// <param name="time">时间单位</param>
        /// <param name="unit">计量单位</param>
        /// <param name="warn">预警级别</param>
        /// <returns></returns>
        private List<FundManagementByProject> CalcProjectIndex(IEnumerable<MediumData> list, string indexName, string time, string unit, string warn)
        {
            // 根据项目ID分组
            var ids = list.GroupBy(a => a.Id);
            var projects = new List<FundManagementByProject>();
            foreach (var id in ids)
            {
                var tempList = list.Where(a => a.Id == id.Key);
                var tempObj = new FundManagementByProject()
                {
                    ID = IdGen.GeneratorIFCGuid(),
                    ProjectId = id.Key,
                    ProjectName = tempList.First().Name,
                    CreateDate = Date,
                    OrganizationLevel = "项目",
                    Type = IndexType.Business,
                    IndexName = indexName,
                    TimeName = time,
                    MeasurementUnitName = unit,
                    WarningLevelName = warn,
                    NumericalValue = tempList.Sum(a => a.Money)
                };
                projects.Add(tempObj);
            }
            foreach (var item in projects)
            {
                var info = ProjectList[item.ProjectId];
                item.ProjectName = info.Name;
                item.OperationOrg = info.Data1;
                item.OperOrgName = info.OwnerOrgName;
                item.OpgSysCode = info.OwnerOrgSysCode;
            }
            return projects;
        }
        /// <summary>
        /// 计算项目指标
        /// </summary>
        /// <param name="list">需要统计的列表</param>
        /// <param name="time">时间单位</param>
        /// <param name="unit">计量单位</param>
        /// <param name="warn">预警级别</param>
        /// <returns></returns>
        private List<FundManagementByProject> CalcProjectIndex(IEnumerable<MediumData> list, string time, string unit, string warn)
        {
            if (list.Count() == 0) return new List<FundManagementByProject>();
            return CalcProjectIndex(list, list.First().IndexName, time, unit, warn);
        }
        /// <summary>
        /// 初始化分公司指标列表，合计各分公司的金额/数量
        /// </summary>
        /// <param name="list">项目指标列表</param>
        /// <returns></returns>
        private List<FundManagement> CalcSubIndex(IEnumerable<FundManagementByProject> list, string indexName, string time, string unit, string warn)
        {
            var subList = new List<FundManagement>();
            foreach (var key in OrgList.Keys)
            {
                var orgInfo = OrgList[key];
                var tmpl = list.Where(a => a.OpgSysCode.Contains(orgInfo.SysCode));
                var tempObj = new FundManagement()
                {
                    ID = IdGen.GeneratorIFCGuid(),
                    CreateDate = Date,
                    OperationOrg = orgInfo.Id,
                    OperOrgName = orgInfo.Name,
                    OpgSysCode = orgInfo.SysCode,
                    OrganizationLevel = "分公司",
                    Type = IndexType.Business,
                    IndexName = indexName,
                    TimeName = time,
                    MeasurementUnitName = unit,
                    WarningLevelName = warn,
                    NumericalValue = tmpl.Sum(a => a.NumericalValue)
                };
                subList.Add(tempObj);
            }
            return subList;
        }
        /// <summary>
        /// 计算公司指标
        /// </summary>
        /// <param name="list"></param>
        /// <param name="indexName"></param>
        /// <param name="time"></param>
        /// <param name="unit"></param>
        /// <param name="warn"></param>
        /// <returns></returns>
        private FundManagement CalcCompanyIndex(IEnumerable<FundManagement> list, string indexName, string time, string unit, string warn)
        {
            var total = list == null ? 0 : list.Sum(a => a.NumericalValue);
            var totalRecord = new FundManagement()
            {
                ID = IdGen.GeneratorIFCGuid(),
                CreateDate = Date,
                OrganizationLevel = "公司",
                Type = IndexType.Business,
                IndexName = indexName,
                TimeName = time,
                MeasurementUnitName = unit,
                WarningLevelName = warn,
                NumericalValue = total
            };
            return totalRecord;
        }
        /// <summary>
        /// 计算预警的指标
        /// </summary>
        /// <returns></returns>
        private List<FundManagement> CalcWarnIndex(IEnumerable<FundManagementByProject> list, string time, string warn)
        {
            var subList = new List<FundManagement>();
            foreach (var key in OrgList.Keys)
            {
                var orgInfo = OrgList[key];
                var tmpl = list.Where(a => a.OpgSysCode.Contains(orgInfo.SysCode));
                if (tmpl.Count() == 0) continue;
                var tempObj = new FundManagement()
                {
                    ID = IdGen.GeneratorIFCGuid(),
                    CreateDate = Date,
                    OperationOrg = orgInfo.Id,
                    OperOrgName = orgInfo.Name,
                    OpgSysCode = orgInfo.SysCode,
                    OrganizationLevel = "分公司",
                    Type = IndexType.Business,
                    IndexName = tmpl.First().IndexName,
                    TimeName = time,
                    MeasurementUnitName = units[1],
                    WarningLevelName = warn,
                    NumericalValue = tmpl.Count()
                };
                subList.Add(tempObj);
            }
            return subList;
        }
        #endregion

        #region 功能方法
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
        /// <summary>
        /// 获取分公司信息
        /// </summary>
        private Dictionary<string, OperationOrgInfo> GetOrgList()
        {
            string sql = "select t1.opgid,t1.opgname,t1.opgsyscode from resoperationorg t1 where t1.opgstate=1 and t1.opgoperationtype='b'";
            var list = ExecuteList<OperationOrgInfo>(sql, a => new OperationOrgInfo()
            {
                Id = a["opgid"].ToString(),
                Name = a["opgname"].ToString(),
                SysCode = a["opgsyscode"].ToString()
            });
            return ConvertDictionary(list, "Id");
        }
        /// <summary>
        /// 获取所有项目的基本信息
        /// </summary>
        private Dictionary<string, CurrentProjectInfo> GetProjectList()
        {
            string sql = "select id,projectname,projectcode,ownerorg,ownerorgname,ownerorgsyscode from resconfig";// where nvl(projectcurrstate,0)!=20";
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
        /// 插入公司/分公司指标
        /// </summary>
        /// <param name="obj">插入对象</param>
        private void InsertFundManagement(FundManagement obj)
        {
            string sql = string.Format("insert into thd_fundmanagement (Id,CreateDate,OperationOrg,OperOrgName,OpgSysCode,OrganizationLevel,IndexName,TimeName,MeasurementUnitName,WarningLevelName,NumericalValue,Descript,BusinessType) values('{0}',to_date('{1}','yyyy-mm-dd'),'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}',{12})", obj.ID, obj.CreateDate.ToString("yyyy-MM-dd"), obj.OperationOrg, obj.OperOrgName, obj.OpgSysCode, obj.OrganizationLevel, obj.IndexName, obj.TimeName, obj.MeasurementUnitName, obj.WarningLevelName, obj.NumericalValue, obj.Descript, (int)obj.Type);
            ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 插入公司/分公司指标
        /// </summary>
        /// <param name="obj">插入对象</param>
        private void InsertFundManagement(IEnumerable<FundManagement> list)
        {
            foreach (var item in list)
            {
                InsertFundManagement(item);
            }
        }
        /// <summary>
        /// 插入项目指标
        /// </summary>
        /// <param name="obj"></param>
        private void InsertFundProject(FundManagementByProject obj)
        {
            string sql = string.Format("insert into thd_fundmanagebyproject (Id,ProjectId,ProjectName,CreateDate,OperationOrg,OperOrgName,OpgSysCode,OrganizationLevel,IndexName, TimeName,MeasurementUnitName,WarningLevelName,NumericalValue,Descript,BusinessType) values ('{0}','{1}','{2}',to_date('{3}','yyyy-mm-dd'),'{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12},'{13}',{14})", obj.ID, obj.ProjectId, obj.ProjectName, obj.CreateDate.ToString("yyyy-MM-dd"), obj.OperationOrg, obj.OperOrgName, obj.OpgSysCode, obj.OrganizationLevel, obj.IndexName, obj.TimeName, obj.MeasurementUnitName, obj.WarningLevelName, obj.NumericalValue, obj.Descript, (int)obj.Type);
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

        #region 访问数据库
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
        #endregion

        #region 数据媒介类
        private class MediumData : ICloneable
        {
            public DateTime CreateDate { get; set; }
            public string Id { get; set; }
            public string Name { get; set; }
            public decimal Money { get; set; }
            public string IndexName { get; set; }
            public string Other { get; set; }
            public decimal Num { get; set; }
            public object Clone()
            {
                return this.MemberwiseClone();
            }
        }
        /// <summary>
        /// 项目账面信息ViewModel
        /// </summary>
        private class ProjectBook
        {
            /// <summary>
            /// 项目ID
            /// </summary>
            public string Id { get; set; }
            /// <summary>
            /// 财务资金支出
            /// </summary>
            public decimal Fiscal { get; set; }
            /// <summary>
            /// 商务确认成本
            /// </summary>
            public decimal Business { get; set; }
            private decimal _money;
            /// <summary>
            /// 去财务与商务中较大的数值
            /// </summary>
            public decimal Money
            {
                get
                {
                    if (_money == 0)
                    {
                        _money = Fiscal > Business ? Fiscal : Business;
                    }
                    return _money;
                }
            }
        }
        #endregion


    }
}
