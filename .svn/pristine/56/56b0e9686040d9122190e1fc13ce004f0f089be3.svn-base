using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core;
using System.Collections;
using Application.Business.Erp.SupplyChain.PMCAndWarning.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;

using VirtualMachine.Component.Util;
using System.IO;
using System.Windows.Forms;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using VirtualMachine.Core.DataAccess;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.CostManagement.OwnerQuantityMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.QWBS.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.RectificationNoticeMng.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using Application.Business.Erp.SupplyChain.SettlementManagement.ExpensesSettleMng.Domain;


namespace Application.Business.Erp.SupplyChain.PMCAndWarning.Service
{
    public class PMCAndWarningSrv : IPMCAndWarningSrv
    {
        private IDao dao;
        public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }

        /// <summary>
        /// 根据对象类型和GUID获取对象
        /// </summary>
        /// <param name="entityType">对象类型</param>
        /// <param name="Id">GUID</param>
        /// <returns></returns>
        public Object GetObjectById(Type entityType, string Id)
        {
            return dao.Get(entityType, Id);
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
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerTime()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// 保存或修改对象集合
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [TransManager]
        public IList SaveOrUpdate(IList list)
        {
            dao.SaveOrUpdate(list);
            return list;
        }

        /// <summary>
        /// 保存或修改对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public object SaveOrUpdate(object obj)
        {
            dao.SaveOrUpdate(obj);
            return obj;
        }

        /// <summary>
        /// 删除对象或对象集合
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteObjList(IList list)
        {
            return dao.Delete(list);
        }

        /// <summary>
        /// 写入指定动作检查状态的预警信息（包括下属所有指标）
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [TransManager]
        public IList WriteWarningInfo(IList listWarnInfo, StateCheckAction checkAction, CurrentProjectInfo projectInfo)
        {
            if (listWarnInfo == null || checkAction == null || projectInfo == null)
                return null;


            IList listResult = new ArrayList();//生成新的预警信息


            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", checkAction.Id));
            oq.AddFetchMode("ListTargets", NHibernate.FetchMode.Eager);
            IList listCheckAction = dao.ObjectQuery(typeof(StateCheckAction), oq);
            checkAction = listCheckAction[0] as StateCheckAction;
            oq.Criterions.Clear();
            oq.FetchModes.Clear();


            List<string> listTargetId = new List<string>();

            foreach (WarningTarget target in checkAction.ListTargets)
            {
                listTargetId.Add(target.Id);
            }


            //作废已有预警信息
            Disjunction dis = new Disjunction();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("State", WarningInfoStateEnum.有效));
            foreach (string targetId in listTargetId)
            {
                dis.Add(Expression.Eq("TheTarget.Id", targetId));
            }
            oq.AddCriterion(dis);

            IList listWarnInfoInDB = dao.ObjectQuery(typeof(WarningInfo), oq);

            if (listWarnInfoInDB != null && listWarnInfoInDB.Count > 0)
            {
                for (int i = 0; i < listWarnInfoInDB.Count; i++)
                {
                    WarningInfo w = listWarnInfoInDB[i] as WarningInfo;
                    w.State = WarningInfoStateEnum.无效;
                    w.FailureTime = DateTime.Now;
                }

                dao.Update(listWarnInfoInDB);
            }


            //生成新的预警信息
            var queryOptWarning = from w in listWarnInfo.OfType<WarningInfo>()
                                  where w.TheTarget != null && listTargetId.Contains(w.TheTarget.Id)
                                  select w;

            if (queryOptWarning != null && queryOptWarning.Count() > 0)
            {
                foreach (var obj in queryOptWarning)
                {
                    WarningInfo w = new WarningInfo();

                    w.ProjectId = obj.ProjectId;
                    w.ProjectName = obj.ProjectName;
                    w.ProjectSyscode = obj.ProjectSyscode;

                    w.TheTarget = obj.TheTarget;
                    w.TheTargetName = obj.TheTargetName;

                    w.TheWarningObjectId = obj.TheWarningObjectId;
                    w.TheWarningObjectTypeName = obj.TheWarningObjectTypeName;

                    w.State = WarningInfoStateEnum.有效;
                    w.SubmitTime = DateTime.Now;
                    w.Level = obj.Level;

                    w.WarningContent = obj.WarningContent;

                    w.Owner = obj.Owner;
                    w.OwnerName = obj.OwnerName;
                    w.OwnerOrgSysCode = obj.OwnerOrgSysCode;

                    listResult.Add(w);
                }

                dao.SaveOrUpdate(listResult);
            }

            return listResult;
        }

        /// <summary>
        /// 写入指定指标的预警信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [TransManager]
        public IList WriteWarningInfo(IList listWarnInfo, WarningTarget target, CurrentProjectInfo projectInfo)
        {
            if (listWarnInfo == null || projectInfo == null)
                return null;


            IList listResult = new ArrayList();//生成新的预警信息


            ObjectQuery oq = new ObjectQuery();
            //作废已有预警信息
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("State", WarningInfoStateEnum.有效));
            oq.AddCriterion(Expression.Eq("TheTarget.Id", target.Id));

            IList listWarnInfoInDB = dao.ObjectQuery(typeof(WarningInfo), oq);

            if (listWarnInfoInDB != null && listWarnInfoInDB.Count > 0)
            {
                for (int i = 0; i < listWarnInfoInDB.Count; i++)
                {
                    WarningInfo w = listWarnInfoInDB[i] as WarningInfo;
                    w.State = WarningInfoStateEnum.无效;
                    w.FailureTime = DateTime.Now;
                }

                dao.Update(listWarnInfoInDB);
            }


            //生成新的预警信息
            var queryOptWarning = from w in listWarnInfo.OfType<WarningInfo>()
                                  where w.TheTarget != null && w.TheTarget.Id == target.Id
                                  select w;

            if (queryOptWarning != null && queryOptWarning.Count() > 0)
            {
                foreach (var obj in queryOptWarning)
                {
                    WarningInfo w = new WarningInfo();

                    w.ProjectId = obj.ProjectId;
                    w.ProjectName = obj.ProjectName;
                    w.ProjectSyscode = obj.ProjectSyscode;

                    w.TheTarget = obj.TheTarget;
                    w.TheTargetName = obj.TheTargetName;

                    w.TheWarningObjectId = obj.TheWarningObjectId;
                    w.TheWarningObjectTypeName = obj.TheWarningObjectTypeName;

                    w.State = WarningInfoStateEnum.有效;
                    w.SubmitTime = DateTime.Now;
                    w.Level = obj.Level;

                    w.WarningContent = obj.WarningContent;

                    w.Owner = obj.Owner;
                    w.OwnerName = obj.OwnerName;
                    w.OwnerOrgSysCode = obj.OwnerOrgSysCode;

                    listResult.Add(w);
                }

                dao.SaveOrUpdate(listResult);
            }

            return listResult;
        }

        [TransManager]
        public IList WriteWarningInfo(IList listWarnInfo)
        {
            if (listWarnInfo == null || listWarnInfo.Count == 0) return new ArrayList();
            Dao.SaveOrUpdate(listWarnInfo);
            return listWarnInfo;
        }

        /// <summary>
        /// 获取上一个周日的日期
        /// </summary>
        /// <returns></returns>
        public DateTime GetPreviousSundayDate()
        {
            DateTime nowTime = DateTime.Now;

            if (nowTime.DayOfWeek == DayOfWeek.Sunday)
                return nowTime;
            else if (nowTime.DayOfWeek == DayOfWeek.Monday)
                return nowTime.AddDays(-1);
            else if (nowTime.DayOfWeek == DayOfWeek.Tuesday)
                return nowTime.AddDays(-2);
            else if (nowTime.DayOfWeek == DayOfWeek.Wednesday)
                return nowTime.AddDays(-3);
            else if (nowTime.DayOfWeek == DayOfWeek.Thursday)
                return nowTime.AddDays(-4);
            else if (nowTime.DayOfWeek == DayOfWeek.Friday)
                return nowTime.AddDays(-5);
            else
                return nowTime.AddDays(-6);

        }

        /// <summary>
        /// 获取预警信息
        /// </summary>
        /// <param name="userJob">用户的岗位(当操作的工程项目为NULL时使用)</param>
        /// <param name="optProject">操作的工程项目；公司层面操作多个项目时，该值设置为NULL</param>
        /// <param name="listRole">用户所具有的角色集合</param>
        /// <param name="pushMode">推送方式</param>
        /// <returns></returns>
        public List<WarningInfo> GetWarningInfoByUserInfo(OperationJob userJob, CurrentProjectInfo optProject, List<OperationRole> listRole, WarningPushModeEnum pushMode)
        {
            if (listRole == null || listRole.Count == 0 || pushMode == 0)
                return null;

            List<WarningInfo> listWarnInfo = new List<WarningInfo>();

            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (OperationRole role in listRole)
            {
                dis.Add(Expression.Eq("UserRole.Id", role.Id));
            }
            oq.AddCriterion(dis);
            oq.AddCriterion(Expression.Eq("PushMode", pushMode));
            IList listPushMode = dao.ObjectQuery(typeof(WarningPushMode), oq);


            oq.Criterions.Clear();

            oq.AddCriterion(Expression.Eq("State", WarningInfoStateEnum.有效));

            if (optProject != null)
                oq.AddCriterion(Expression.Eq("ProjectId", optProject.Id));
            else
                oq.AddCriterion(Expression.Like("ProjectSyscode", userJob.OperationOrg.SysCode, MatchMode.Start));

            dis = new Disjunction();
            if (listPushMode != null && listPushMode.Count > 0)
            {
                foreach (WarningPushMode mode in listPushMode)
                {
                    if (mode.TheTarget != null)
                    {
                        dis.Add(Expression.And(Expression.Eq("Level", mode.WarningLevel), Expression.Eq("TheTarget.Id", mode.TheTarget.Id)));
                    }
                }
            }


            oq.AddCriterion(dis);


            listWarnInfo = dao.ObjectQuery(typeof(WarningInfo), oq).OfType<WarningInfo>().ToList();

            return listWarnInfo;
        }

        /// <summary>
        /// 获取预警信息
        /// </summary>
        /// <param name="userOrgSysCode">用户所属组织层次码(当操作的工程项目为NULL时使用)</param>
        /// <param name="optProject">操作的工程项目；公司层面操作多个项目时，该值设置为NULL</param>
        /// <param name="listRole">用户所具有的角色集合</param>
        /// <param name="pushMode">推送方式</param>
        /// <returns></returns>
        public List<WarningInfo> GetWarningInfoByUserInfo(string userOrgSysCode, CurrentProjectInfo optProject, List<OperationRole> listRole, WarningPushModeEnum pushMode)
        {
            if (listRole == null || listRole.Count == 0 || pushMode == 0)
                return null;

            List<WarningInfo> listWarnInfo = new List<WarningInfo>();

            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (OperationRole role in listRole)
            {
                dis.Add(Expression.Eq("UserRole.Id", role.Id));
            }
            oq.AddCriterion(dis);
            oq.AddCriterion(Expression.Eq("PushMode", pushMode));
            IList listPushMode = dao.ObjectQuery(typeof(WarningPushMode), oq);


            oq.Criterions.Clear();

            oq.AddCriterion(Expression.Eq("State", WarningInfoStateEnum.有效));

            if (optProject != null)
                oq.AddCriterion(Expression.Eq("ProjectId", optProject.Id));
            else
                oq.AddCriterion(Expression.Like("ProjectSyscode", userOrgSysCode, MatchMode.Start));

            dis = new Disjunction();

            if (listPushMode != null && listPushMode.Count > 0)
            {
                foreach (WarningPushMode mode in listPushMode)
                {
                    if (mode.TheTarget != null)
                    {
                        dis.Add(Expression.And(Expression.Eq("Level", mode.WarningLevel), Expression.Eq("TheTarget.Id", mode.TheTarget.Id)));
                    }
                }
            }

            oq.AddCriterion(dis);


            listWarnInfo = dao.ObjectQuery(typeof(WarningInfo), oq).OfType<WarningInfo>().ToList();

            return listWarnInfo;
        }

        /// <summary>
        /// 启动预警服务
        /// </summary>
        /// <returns></returns>
        public bool StartWarningServer()
        {
            StreamWriter write = null;
            try
            {
                write = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"WarningLog.txt", true, Encoding.Default);
                write.WriteLine("预警定时轮询服务开始启动.......，启动时间：" + DateTime.Now.ToString());


                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TriggerMode", StateCheckTriggerMode.定时));
                oq.AddCriterion(Expression.Gt("TriggerTerm1", (decimal)0));

                IList listCheckAction = dao.ObjectQuery(typeof(StateCheckAction), oq);
                if (listCheckAction.Count > 0)
                {
                    oq.Criterions.Clear();

                    //oq.AddCriterion(Expression.Eq("ProjectInfoState", EnumProjectInfoState.发布));

                    IList listProject = dao.ObjectQuery(typeof(CurrentProjectInfo), oq);
                    if (listProject != null && listProject.Count > 0)
                    {

                        List<string> listProjectIds = new List<string>();
                        foreach (CurrentProjectInfo p in listProject)
                        {
                            listProjectIds.Add(p.Id);
                        }



                        foreach (StateCheckAction checkAction in listCheckAction)
                        {
                            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

                            timer.Enabled = true;
                            timer.Interval = (int)(checkAction.TriggerTerm1 * 60 * 60 * 1000);//timer.Interval单位毫秒 

                            Dictionary<StateCheckAction, List<string>> dicCheckActionProject = new Dictionary<StateCheckAction, List<string>>();
                            dicCheckActionProject.Add(checkAction, listProjectIds);
                            timer.Tag = dicCheckActionProject;

                            timer.Tick += new EventHandler(timer_Tick);

                            WarningServerControl.Timers.Add(timer);

                            timer.Start();
                        }
                    }
                }

                write.WriteLine("预警定时轮询服务启动完成，完成时间：" + DateTime.Now.ToString());

                return true;
            }
            catch (Exception ex)
            {
                if (write == null)
                    write = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"WarningLog.txt", true, Encoding.Default);

                string msg = ExceptionUtil.ExceptionMessage(ex);
                write.WriteLine(DateTime.Now.ToString() + "预警定时轮询服务启动失败,异常信息：" + msg);

                throw ex;
            }
            finally
            {
                if (write != null)
                {
                    write.Close();
                    write.Dispose();
                }
            }

            return false;
        }

        void timer_Tick(object sender, EventArgs e)
        {

            StreamWriter write = null;
            StateCheckAction exeCheckAction = null;
            try
            {
                Timer timer = sender as Timer;

                if (timer == null)
                    return;
                if (timer.Tag == null)
                    return;

                Dictionary<StateCheckAction, List<string>> dicCheckActionProject = timer.Tag as Dictionary<StateCheckAction, List<string>>;
                if (dicCheckActionProject == null || dicCheckActionProject.Count == 0)
                    return;

                exeCheckAction = dicCheckActionProject.ElementAt(0).Key;
                List<string> listProjectIds = dicCheckActionProject.ElementAt(0).Value;

                if (exeCheckAction.ActionName == "工期状态检查")
                    CheckDurationState(listProjectIds, false, true, null);
                else if (exeCheckAction.ActionName == "资料状态检查")
                    CheckInformationState(listProjectIds, false, true, null);
                if (exeCheckAction.ActionName == WarningTarget.WarningTarget_WZ_SupplyOrder)
                {
                    //CheckDurationSupplyOrder(listProjectIds, false, true, null);
                }

            }
            catch (Exception ex)
            {
                string msg = ExceptionUtil.ExceptionMessage(ex);

                write = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"WarningLog.txt", true, Encoding.Default);

                write.WriteLine(DateTime.Now.ToString() + "状态检查动作“" + (exeCheckAction != null ? exeCheckAction.ActionName : string.Empty) + "”执行服务时失败,异常信息：" + msg);

                throw ex;
            }
            finally
            {
                if (write != null)
                {
                    write.Close();
                    write.Dispose();
                }
            }
        }

        /// <summary>
        /// 停止预警服务
        /// </summary>
        /// <returns></returns>
        public bool StopWarningServer()
        {
            StreamWriter write = null;
            try
            {
                write = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"WarningLog.txt", true, Encoding.Default);
                write.WriteLine("预警定时轮询服务开始关闭.......，关闭时间：" + DateTime.Now.ToString());

                if (WarningServerControl.Timers != null && WarningServerControl.Timers.Count > 0)
                {
                    foreach (System.Windows.Forms.Timer timer in WarningServerControl.Timers)
                    {
                        timer.Stop();
                    }

                    WarningServerControl.Timers.Clear();

                }

                write.WriteLine("预警定时轮询服务启动完成，完成时间：" + DateTime.Now.ToString());

                return true;
            }
            catch (Exception ex)
            {
                if (write == null)
                    write = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"WarningLog.txt", true, Encoding.Default);

                string msg = ExceptionUtil.ExceptionMessage(ex);
                write.WriteLine(DateTime.Now.ToString() + "预警定时轮询服务关闭失败,异常信息：" + msg);

                throw ex;
            }
            finally
            {
                if (write != null)
                {
                    write.Close();
                    write.Dispose();
                }
            }

            return false;
        }

        #region 工期状态检查
        /// <summary>
        /// 工期状态检查(最初设计)
        /// </summary>
        /// <param name="projectIds">项目Id集</param>
        /// <param name="isShowFlag">显示标记</param>
        /// <param name="isBuildWarningFlag">预警标记</param>
        /// <returns></returns>
        [TransManager]
        private List<StateCheckActionValueObject> CheckDurationState2222(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject)
        {
            //预警信息
            List<WarningInfo> listWarningInfo = new List<WarningInfo>();

            // 进度检查集
            List<StateCheckActionValueObject> listSchduleCheck = new List<StateCheckActionValueObject>();

            // 进度检查节点集
            ArrayList listSchduleCheckNodeDtl = new ArrayList();

            if (projectIds != null && projectIds.Count > 0)
            {
                #region 1.获取进度检查节点集
                //A.将<操作项目集>中每个项目所针对的总体滚动进度计划根节点{进度计划节点}加入到<进度检查节点集>中。
                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();
                foreach (string id in projectIds)
                {
                    dis.Add(Expression.Eq("ProjectId", id));
                }
                oq.AddCriterion(dis);
                oq.AddCriterion(Expression.Eq("ScheduleType", EnumScheduleType.总滚动进度计划));
                oq.AddCriterion(Expression.Eq("State", EnumScheduleState.发布));

                IList listPlanMaster = dao.ObjectQuery(typeof(ProductionScheduleMaster), oq);
                oq.Criterions.Clear();

                dis = new Disjunction();
                foreach (ProductionScheduleMaster master in listPlanMaster)
                {
                    dis.Add(Expression.Eq("Master.Id", master.Id));
                }
                oq.AddCriterion(dis);

                oq.AddCriterion(Expression.Eq("Level", 2));//根节点
                oq.AddFetchMode("Master", NHibernate.FetchMode.Eager);
                //oq.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);

                IList listDtl = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq);
                if (listDtl != null && listDtl.Count > 0)
                    listSchduleCheckNodeDtl.AddRange(listDtl);


                oq.Criterions.Clear();
                oq.FetchModes.Clear();

                //B.针对<操作项目集>中每个项目总体滚动进度计划的{进度计划节点}进行查询：
                //【工程项目任务】关联{工程项目任务}_【所属工程任务类型GUID】关联{工程任务类型}_【工程任务类型级别】=3.分部工程
                //将查得的{进度计划节点}加入到<进度检查节点集>中。
                oq.AddCriterion(dis);
                oq.AddCriterion(Expression.Eq("GWBSTree.ProjectTaskTypeGUID.TypeLevel", ProjectTaskTypeLevel.分部工程));
                oq.AddFetchMode("Master", NHibernate.FetchMode.Eager);
                //oq.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);
                listDtl = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq);
                if (listDtl != null && listDtl.Count > 0)
                    listSchduleCheckNodeDtl.AddRange(listDtl);

                oq.Criterions.Clear();
                oq.FetchModes.Clear();

                #endregion

                #region 2.循环处理进度检查集

                DateTime endDate = GetPreviousSundayDate(); //最近上一个周日
                foreach (ProductionScheduleDetail dtl in listSchduleCheckNodeDtl)
                {
                    //2.1获取进度时间
                    GWBSTree optTask = dtl.GWBSTree;
                    DateTime? planBeginTime = null;
                    DateTime? planEndTime = null;
                    DateTime? factBeginTime = null;
                    DateTime? factEndTime = null;
                    DateTime? precastFactEndTime = null;
                    WarningLevelEnum warningLevel = WarningLevelEnum.无预警;
                    string warningContent = "";
                    PersonInfo owner = null;

                    if (dtl.PlannedBeginDate != DateTime.Parse("1900-1-1"))
                        planBeginTime = dtl.PlannedBeginDate;

                    if (dtl.PlannedEndDate != DateTime.Parse("1900-1-1"))
                        planEndTime = dtl.PlannedEndDate;

                    if (dtl.ActualBeginDate != DateTime.Parse("1900-1-1"))
                        factBeginTime = dtl.ActualBeginDate;

                    if (dtl.ActualEndDate != DateTime.Parse("1900-1-1"))
                        factEndTime = dtl.ActualEndDate;

                    int lowWarningLevel = 5;
                    int highWarningLevel = 15;
                    if (dtl.Level == 2)//项目节点（计划根节点）
                    {
                        lowWarningLevel = 30;
                        highWarningLevel = 90;
                    }

                    #region 2.2 是否需要汇集工期时间
                    if (planBeginTime == null || planEndTime == null || factBeginTime == null || factEndTime == null)
                    {
                        //1.构造叶进度计划节点集，使用以下条件查询{进度计划节点}：【结构层次码】包含<进度计划节点>_【结构层次码】AND 是叶结点 设置查得结果为<叶{进度计划节点}集>。
                        oq.AddCriterion(Expression.Eq("Master.Id", dtl.Master.Id));
                        oq.AddCriterion(Expression.Eq("GWBSNodeType", VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode));
                        oq.AddCriterion(Expression.Like("GWBSTreeSysCode", dtl.GWBSTreeSysCode));

                        IEnumerable<ProductionScheduleDetail> listSchduleLeafNode = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq).OfType<ProductionScheduleDetail>();

                        if (listSchduleLeafNode.Count() > 0)
                        {
                            //2.汇集计划时间
                            if (planBeginTime == null)
                            {
                                var query = from n in listSchduleLeafNode
                                            where n.PlannedBeginDate != DateTime.Parse("1900-1-1")
                                            select n;

                                if (query.Count() > 0)
                                {
                                    query = from n in query
                                            where n.PlannedBeginDate == listSchduleLeafNode.Min(t => n.PlannedBeginDate)
                                            select n;

                                    planBeginTime = query.ElementAt(0).PlannedBeginDate;
                                }
                            }
                            if (planEndTime == null)
                            {
                                var query = from n in listSchduleLeafNode
                                            where n.PlannedEndDate != DateTime.Parse("1900-1-1")
                                            select n;

                                if (query.Count() > 0)
                                {
                                    query = from n in query
                                            where n.PlannedEndDate == listSchduleLeafNode.Max(t => n.PlannedEndDate)
                                            select n;

                                    planEndTime = query.ElementAt(0).PlannedEndDate;
                                }
                            }

                            //3.汇集实际开始时间
                            if (factBeginTime == null)
                            {
                                var query = from n in listSchduleLeafNode
                                            where n.ActualBeginDate != DateTime.Parse("1900-1-1")
                                            select n;

                                if (query.Count() > 0)
                                {
                                    query = from n in query
                                            where n.ActualBeginDate == listSchduleLeafNode.Min(t => n.ActualBeginDate)
                                            select n;

                                    factBeginTime = query.ElementAt(0).ActualBeginDate;
                                }
                            }


                            //4.汇集实际结束时间
                            if (factBeginTime != null && factEndTime == null)
                            {
                                var query = from n in listSchduleLeafNode
                                            where n.ActualEndDate != DateTime.Parse("1900-1-1")
                                            select n;

                                if (query.Count() == listSchduleLeafNode.Count())//所有<叶{进度计划节点}集>中各个对象【任务实际结束时间】中均不等于NULL
                                {
                                    query = from n in query
                                            where n.ActualEndDate == listSchduleLeafNode.Max(t => n.ActualEndDate)
                                            select n;

                                    factEndTime = query.ElementAt(0).ActualEndDate;
                                }
                                else
                                {
                                    //计算<预计实际结束时间>

                                    decimal addupFigureProgress = (dao.Get(typeof(GWBSTree), dtl.GWBSTree.Id) as GWBSTree).AddUpFigureProgress;

                                    if (addupFigureProgress == 0)
                                    {
                                        //统计<叶{进度计划节点}集>中每个对象【任务计划结束时间】-【任务计划开始时间】之和，设置为<总权重>。并设置<汇集工程形象进度>=0
                                        int planTimeCount = 0;//总权重 计划时间单位为天，此处累计天数
                                        decimal figureProgressCount = 0;//汇集工程形象进度

                                        foreach (ProductionScheduleDetail leafNode in listSchduleLeafNode)
                                        {
                                            if (leafNode.PlannedEndDate != DateTime.Parse("1900-1-1"))
                                                planTimeCount += (leafNode.PlannedEndDate - leafNode.PlannedBeginDate).Days;
                                        }
                                        //ii）针对<叶{进度计划节点}集>中每个对象<叶{进度计划节点}>进行以下计算：
                                        //W =<叶{进度计划节点}>_【累积工程形象进度】* （<叶{进度计划节点}>_【计划结束时间】- <叶{进度计划节点}>_【计划开始时间】）/ <总权重>
                                        foreach (ProductionScheduleDetail leafNode in listSchduleLeafNode)
                                        {
                                            if (leafNode.PlannedEndDate != DateTime.Parse("1900-1-1"))
                                            {
                                                decimal w = leafNode.AddupFigureProgress * (leafNode.PlannedEndDate - leafNode.PlannedBeginDate).Days / planTimeCount;
                                                figureProgressCount += w;
                                            }
                                        }

                                        addupFigureProgress = figureProgressCount;
                                    }

                                    //d）<预计实际结束时间>=<实际开始时间>+（<截止时间> -<实际开始时间>）/ <累积工程形象进度> 
                                    //（说明：根据叶节点目前实际工期并按其累积工程形象进度推算其工期，再以计划工期为权重汇集叶节点的预计工期形成检查节点预计工期，得到<预计实际结束时间>）
                                    precastFactEndTime = factBeginTime.Value.AddDays((int)((endDate - factBeginTime.Value).Days / addupFigureProgress));
                                }
                            }
                        }
                    }
                    #endregion

                    #region 2.3.确定工期状态
                    DurationStateEnum durState = 0;
                    if (factBeginTime == null)
                    {
                        durState = DurationStateEnum.未开工;
                    }
                    else if (factBeginTime != null && factEndTime != null)
                    {
                        durState = DurationStateEnum.已完工;
                    }
                    else if (factBeginTime != null && factEndTime == null)
                    {
                        durState = DurationStateEnum.未完工;
                    }
                    #endregion


                    #region 追加进度检查集
                    //A.工期时间：
                    //<项目名称>=<检查{进度计划节点}>_【所属进度计划】关联{进度计划}_【所属项目】；
                    //<进度计划节点>=<检查{进度计划节点}>_【GUID】；
                    //<计划开始时间>=<计划开始时间>
                    //<计划结束时间>=<计划结束时间>
                    //<计划工期>=<计划开始时间>-<计划结束时间>+1
                    //<实际开始时间>=If <实际开始时间>！=NULL then =<实际开始时间> Else =“未开工”
                    //<实际结束时间>=If <实际结束时间>！=NULL then =<实际结束时间> Else = “未完工，预计”+<预计实际结束时间>
                    //<预计实际结束时间>=<预计实际结束时间>
                    //<实际工期>=if <实际开始时间>！=NULL AND <实际结束时间>!=NULL  Then =<实际结束时间>-<实际开始时间>+1 Else if <实际开始时间>！=NULL AND <实际结束时间>=NULL  Then = “未完工，预计为”<预计实际结束时间>-<实际开始时间>+1 Else =“未开工”。

                    StateCheckActionValueObject obj = new StateCheckActionValueObject();
                    obj.ProjectId = dtl.Master.ProjectId;
                    obj.ProjectName = dtl.Master.ProjectName;
                    obj.ProjectSysCode = (dao.Get(typeof(CurrentProjectInfo), dtl.Master.ProjectId) as CurrentProjectInfo).OwnerOrgSysCode;
                    obj.SchedulePlanNode = dtl;
                    obj.PlanBeginTime = planBeginTime;
                    obj.PlanEndTime = planEndTime;
                    obj.PlanDuration = (planEndTime.Value - planBeginTime.Value).Days + 1;
                    obj.FactBeginTime = factBeginTime != null ? factEndTime.Value.ToShortDateString() : DurationStateEnum.未开工.ToString();
                    obj.DurationState = durState;
                    obj.FactEndTime = factEndTime != null ? factEndTime.Value.ToShortDateString() : DurationStateEnum.未完工 +
                        (precastFactEndTime != null ? ",预计" + precastFactEndTime.Value.ToShortDateString() + "完工" : "");

                    obj.PrecastFactEndTime = precastFactEndTime;
                    if (factBeginTime != null && factEndTime != null)
                        obj.FactDuration = ((factEndTime.Value - factBeginTime.Value).Days + 1).ToString();
                    else if (factBeginTime != null && factEndTime == null)
                        obj.FactDuration = precastFactEndTime != null ? ("未完工,预计" + ((precastFactEndTime.Value - factBeginTime.Value).Days + 1).ToString() + "天完工") : "";
                    else
                        obj.FactDuration = "未开工";


                    //B.预警： 
                    if (durState == DurationStateEnum.未开工)
                    {
                        System.TimeSpan checkTime = endDate - planBeginTime.Value;
                        if (checkTime.Days <= 0)
                        {
                            warningLevel = WarningLevelEnum.无预警;
                            warningContent = "工期正常";
                        }
                        else if (checkTime.Days <= lowWarningLevel)
                        {
                            warningLevel = WarningLevelEnum.低级别;
                            warningContent = "开工拖期" + checkTime.Days + "天";
                        }
                        else if (checkTime.Days <= highWarningLevel)
                        {
                            warningLevel = WarningLevelEnum.中级别;
                            warningContent = "开工拖期" + checkTime.Days + "天";
                        }
                        else if (checkTime.Days > highWarningLevel)
                        {
                            warningLevel = WarningLevelEnum.高级别;
                            warningContent = "开工拖期" + checkTime.Days + "天";
                        }
                    }
                    else if (durState == DurationStateEnum.已完工)
                    {
                        System.TimeSpan checkTime = factEndTime.Value - planEndTime.Value;
                        if (checkTime.Days <= 0)
                        {
                            warningLevel = WarningLevelEnum.无预警;
                            warningContent = "工期正常";
                        }
                        else if (checkTime.Days <= lowWarningLevel)
                        {
                            warningLevel = WarningLevelEnum.低级别;
                            warningContent = "实际完工拖期" + checkTime.Days + "天";
                        }
                        else if (checkTime.Days <= highWarningLevel)
                        {
                            warningLevel = WarningLevelEnum.中级别;
                            warningContent = "实际完工拖期" + checkTime.Days + "天";
                        }
                        else if (checkTime.Days > highWarningLevel)
                        {
                            warningLevel = WarningLevelEnum.高级别;
                            warningContent = "实际完工拖期" + checkTime.Days + "天";
                        }
                    }
                    else if (durState == DurationStateEnum.未完工)
                    {
                        System.TimeSpan checkTime = precastFactEndTime.Value - planEndTime.Value;
                        if (checkTime.Days <= 0)
                        {
                            warningLevel = WarningLevelEnum.无预警;
                            warningContent = "工期正常";
                        }
                        else if (checkTime.Days <= lowWarningLevel)
                        {
                            warningLevel = WarningLevelEnum.低级别;
                            warningContent = "未完工，预计完工拖期" + checkTime.Days + "天";
                        }
                        else if (checkTime.Days <= highWarningLevel)
                        {
                            warningLevel = WarningLevelEnum.中级别;
                            warningContent = "未完工，预计完工拖期" + checkTime.Days + "天";
                        }
                        else if (checkTime.Days > highWarningLevel)
                        {
                            warningLevel = WarningLevelEnum.高级别;
                            warningContent = "未完工，预计完工拖期" + checkTime.Days + "天";
                        }
                    }

                    //C.预警责任人
                    if (warningLevel == 0)
                        owner = null;
                    else
                    {
                        oq.Criterions.Clear();
                        oq.FetchModes.Clear();

                        //查询{管理OBS}_【工程项目任务】=<检查{进度计划节点}>_【工程项目任务】，取查得对象的【责任人】

                    }

                    #endregion

                    obj.Level = warningLevel;
                    obj.WarningContent = warningContent;
                    obj.Owner = owner;
                    if (owner != null)
                        obj.OwnerName = owner.Name;

                    listSchduleCheck.Add(obj);
                }
                #endregion

                #region 写预警信息
                if (isBuildWarningFlag)
                {
                    //A.查询{进度检查集}_【预警级别】！=0，设置查得结果为<预警进度节点集>。
                    var queryWarning = from s in listSchduleCheck
                                       where s.Level != WarningLevelEnum.无预警
                                       select s;

                    WarningTarget target = null;
                    oq.Criterions.Clear();
                    oq.FetchModes.Clear();
                    oq.AddCriterion(Expression.Eq("TargetCode", "G001"));
                    IList listTarget = dao.ObjectQuery(typeof(WarningTarget), oq);
                    if (listTarget != null && listTarget.Count > 0)
                    {
                        target = listTarget[0] as WarningTarget;
                    }

                    foreach (StateCheckActionValueObject obj in queryWarning)
                    {
                        WarningInfo w = new WarningInfo();
                        w.ProjectId = obj.ProjectId;
                        w.ProjectName = obj.ProjectName;
                        w.ProjectSyscode = obj.ProjectSysCode;
                        w.TheTarget = target;
                        if (target != null)
                            w.TheTargetName = target.TargetName;

                        w.TheWarningObjectId = obj.SchedulePlanNode.Id;
                        w.TheWarningObjectTypeName = obj.SchedulePlanNode.GetType().Name;
                        w.Level = obj.Level;
                        w.Owner = obj.Owner;
                        w.OwnerName = obj.OwnerName;
                        w.WarningContent = obj.WarningContent;

                        listWarningInfo.Add(w);
                    }

                    oq.Criterions.Clear();
                    oq.AddCriterion(Expression.Eq("ActionName", "工期状态检查"));
                    IList listCheckAction = dao.ObjectQuery(typeof(StateCheckAction), oq);
                    if (listCheckAction != null)
                    {
                        WriteWarningInfo(listWarningInfo, listCheckAction[0] as StateCheckAction, currProject);
                    }
                }
                #endregion

            }

            return listSchduleCheck;

        }

        /// <summary>
        /// 工期状态检查
        /// </summary>
        /// <param name="projectIds">项目Id集</param>
        /// <param name="isShowFlag">显示标记</param>
        /// <param name="isBuildWarningFlag">预警标记</param>
        /// <returns></returns>
        [TransManager]
        public List<StateCheckActionValueObject> CheckDurationState(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject)
        {
            if (projectIds == null || projectIds.Count == 0)
                return null;

            //预警信息
            List<WarningInfo> listWarningInfo = new List<WarningInfo>();

            // 进度检查集
            List<StateCheckActionValueObject> listSchduleCheck = new List<StateCheckActionValueObject>();

            // 进度检查节点集
            ArrayList listSchduleCheckNodeDtl = new ArrayList();

            #region 1.获取进度检查节点集
            //A.将<操作项目集>中每个项目所针对的总体滚动进度计划根节点{进度计划节点}加入到<进度检查节点集>中。
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (string id in projectIds)
            {
                dis.Add(Expression.Eq("ProjectId", id));
            }
            oq.AddCriterion(dis);
            oq.AddCriterion(Expression.Eq("ScheduleType", EnumScheduleType.总滚动进度计划));
            oq.AddCriterion(Expression.Eq("DocState", VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute));

            IList listPlanMaster = dao.ObjectQuery(typeof(ProductionScheduleMaster), oq);
            oq.Criterions.Clear();

            dis = new Disjunction();
            foreach (ProductionScheduleMaster master in listPlanMaster)
            {
                dis.Add(Expression.Eq("Master.Id", master.Id));
            }
            oq.AddCriterion(dis);

            oq.AddCriterion(Expression.Eq("Level", 2));//根节点
            oq.AddFetchMode("Master", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);

            IList listDtl = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq);
            if (listDtl != null && listDtl.Count > 0)
                listSchduleCheckNodeDtl.AddRange(listDtl);


            oq.Criterions.Clear();
            oq.FetchModes.Clear();

            //B.针对<操作项目集>中每个项目总体滚动进度计划的{进度计划节点}进行查询：
            //【工程项目任务】关联{工程项目任务}_【所属工程任务类型GUID】关联{工程任务类型}_【工程任务类型级别】=3.分部工程
            //将查得的{进度计划节点}加入到<进度检查节点集>中。
            oq.AddCriterion(dis);
            oq.AddCriterion(Expression.Eq("GWBSTree.ProjectTaskTypeGUID.TypeLevel", ProjectTaskTypeLevel.分部工程));
            oq.AddFetchMode("Master", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);
            listDtl = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq);
            if (listDtl != null && listDtl.Count > 0)
                listSchduleCheckNodeDtl.AddRange(listDtl);

            oq.Criterions.Clear();
            oq.FetchModes.Clear();

            #endregion

            #region 2.循环处理进度检查集

            DateTime endDate = GetPreviousSundayDate().Date; //最近上一个周日
            foreach (ProductionScheduleDetail dtl in listSchduleCheckNodeDtl)
            {
                //2.1获取进度时间
                GWBSTree optTask = dtl.GWBSTree;
                DateTime? planBeginTime = null;
                DateTime? planEndTime = null;
                DateTime? factBeginTime = null;
                DateTime? factEndTime = null;
                DateTime? precastFactEndTime = null;
                WarningLevelEnum warningLevel = WarningLevelEnum.无预警;
                string warningContent = "";
                PersonInfo owner = null;

                if (dtl.PlannedBeginDate != DateTime.Parse("1900-1-1"))
                    planBeginTime = dtl.PlannedBeginDate;

                if (dtl.PlannedEndDate != DateTime.Parse("1900-1-1"))
                    planEndTime = dtl.PlannedEndDate;

                if (dtl.ActualBeginDate != DateTime.Parse("1900-1-1"))
                    factBeginTime = dtl.ActualBeginDate;

                if (dtl.ActualEndDate != DateTime.Parse("1900-1-1"))
                    factEndTime = dtl.ActualEndDate;

                int lowWarningLevel = 5;
                int highWarningLevel = 15;
                if (dtl.Level == 2)//项目节点（计划根节点）
                {
                    lowWarningLevel = 30;
                    highWarningLevel = 90;
                }

                #region 2.2 是否需要汇集工期时间
                if (planBeginTime == null || planEndTime == null || factBeginTime == null || factEndTime == null)
                {
                    //1.构造叶进度计划节点集，使用以下条件查询{进度计划节点}：【结构层次码】包含<进度计划节点>_【结构层次码】AND 是叶结点 设置查得结果为<叶{进度计划节点}集>。
                    oq.AddCriterion(Expression.Eq("Master.Id", dtl.Master.Id));
                    oq.AddCriterion(Expression.Eq("GWBSNodeType", VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode));
                    oq.AddCriterion(Expression.Like("GWBSTreeSysCode", dtl.GWBSTreeSysCode, MatchMode.Start));

                    IEnumerable<ProductionScheduleDetail> listSchduleLeafNode = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq).OfType<ProductionScheduleDetail>();

                    oq.Criterions.Clear();

                    if (listSchduleLeafNode.Count() > 0)
                    {
                        //2.汇集计划时间
                        if (planBeginTime == null)
                        {
                            var query = from n in listSchduleLeafNode
                                        where n.PlannedBeginDate != DateTime.Parse("1900-1-1")
                                        select n;

                            if (query.Count() > 0)
                            {
                                planBeginTime = query.Min(t => t.PlannedBeginDate);
                            }
                        }
                        if (planEndTime == null)
                        {
                            var query = from n in listSchduleLeafNode
                                        where n.PlannedEndDate != DateTime.Parse("1900-1-1")
                                        select n;

                            if (query.Count() > 0)
                            {
                                planEndTime = query.Max(t => t.PlannedEndDate);
                            }
                        }

                        //3.汇集实际开始时间
                        if (factBeginTime == null)
                        {
                            var query = from n in listSchduleLeafNode
                                        where n.ActualBeginDate != DateTime.Parse("1900-1-1")
                                        select n;

                            if (query.Count() > 0)
                            {
                                factBeginTime = query.Min(t => t.ActualBeginDate);
                            }
                        }


                        //4.汇集实际结束时间
                        if (factBeginTime != null && factEndTime == null)
                        {
                            var query = from n in listSchduleLeafNode
                                        where n.ActualEndDate != DateTime.Parse("1900-1-1")
                                        select n;

                            if (query.Count() == listSchduleLeafNode.Count())//所有<叶{进度计划节点}集>中各个对象【任务实际结束时间】中均不等于NULL
                            {
                                factEndTime = query.Max(t => t.ActualEndDate);
                            }
                            else
                            {
                                //计算<预计实际结束时间>

                                decimal addupFigureProgress = (dao.Get(typeof(GWBSTree), dtl.GWBSTree.Id) as GWBSTree).AddUpFigureProgress;

                                if (addupFigureProgress == 0)
                                {
                                    //1.构造叶进度计划节点集，使用以下条件查询{进度计划节点}：【结构层次码】包含<进度计划节点>_【结构层次码】AND 是叶结点 设置查得结果为<叶{进度计划节点}集>。
                                    oq.AddCriterion(Expression.Eq("Master.Id", dtl.Master.Id));
                                    oq.AddCriterion(Expression.Like("GWBSTreeSysCode", dtl.GWBSTreeSysCode, MatchMode.Start));
                                    //oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);

                                    IEnumerable<ProductionScheduleDetail> listChildSchduleNode = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq).OfType<ProductionScheduleDetail>();

                                    int maxLevel = listChildSchduleNode.Max(t => t.Level);
                                    int minLevel = listChildSchduleNode.Min(t => t.Level);


                                    //节点累计工程形象进度X=子节点形象进度Xi按子节点计划工期Di进行加权平均：X= ∑(Xi*(Di/∑Di)) 
                                    //从最深级别开始往上一层层汇集
                                    for (int i = maxLevel; i > minLevel; i--)
                                    {
                                        var queryLevel = from s in listChildSchduleNode
                                                         where s.Level == i
                                                         select s;

                                        int sumDuration = queryLevel.Sum(t => t.PlannedDuration);

                                        //根据父节点分组，以便汇集工期和形象进度到每个父节点上
                                        var queryGroupParent = from p in queryLevel
                                                               group p by new { parentId = p.ParentNode.Id }
                                                                   into g
                                                                   select new { g.Key.parentId };

                                        //汇集上一层节点的形象进度和工期
                                        foreach (var parentObj in queryGroupParent)
                                        {
                                            decimal figureProgress = 0;

                                            var queryParent = from p in listChildSchduleNode
                                                              where p.Id == parentObj.parentId
                                                              select p;

                                            var queryChild = from p in queryLevel
                                                             where p.ParentNode.Id == parentObj.parentId
                                                             select p;

                                            foreach (ProductionScheduleDetail planDtl in queryChild)
                                            {
                                                if (sumDuration != 0)
                                                    figureProgress += planDtl.AddupFigureProgress * (planDtl.PlannedDuration / sumDuration);
                                            }

                                            DateTime queryMaxEndDate = queryChild.Max(t => t.PlannedEndDate);

                                            DateTime queryMinBeginDate = queryChild.Min(t => t.PlannedBeginDate);

                                            int planDuration = (queryMaxEndDate - queryMinBeginDate).Days + 1;

                                            queryParent.ElementAt(0).AddupFigureProgress = figureProgress;
                                            queryParent.ElementAt(0).PlannedDuration = planDuration;
                                        }
                                    }

                                    addupFigureProgress = (from s in listChildSchduleNode
                                                           where s.Level == minLevel
                                                           select s).ElementAt(0).AddupFigureProgress;

                                }

                                //d）<预计实际结束时间>=<实际开始时间>+（<截止时间> -<实际开始时间>）/ <累积工程形象进度> 
                                //（说明：根据叶节点目前实际工期并按其累积工程形象进度推算其工期，再以计划工期为权重汇集叶节点的预计工期形成检查节点预计工期，得到<预计实际结束时间>）
                                if (addupFigureProgress > 0)
                                    precastFactEndTime = factBeginTime.Value.AddDays((int)(((endDate - factBeginTime.Value).Days + 1) / addupFigureProgress));
                            }
                        }
                    }
                }
                #endregion

                #region 2.3.确定工期状态
                DurationStateEnum durState = 0;
                if (factBeginTime == null)
                {
                    durState = DurationStateEnum.未开工;
                }
                else if (factBeginTime != null && factEndTime != null)
                {
                    durState = DurationStateEnum.已完工;
                }
                else if (factBeginTime != null && factEndTime == null)
                {
                    durState = DurationStateEnum.未完工;
                }
                #endregion


                #region 追加进度检查集
                //A.工期时间：
                //<项目名称>=<检查{进度计划节点}>_【所属进度计划】关联{进度计划}_【所属项目】；
                //<进度计划节点>=<检查{进度计划节点}>_【GUID】；
                //<计划开始时间>=<计划开始时间>
                //<计划结束时间>=<计划结束时间>
                //<计划工期>=<计划开始时间>-<计划结束时间>+1
                //<实际开始时间>=If <实际开始时间>！=NULL then =<实际开始时间> Else =“未开工”
                //<实际结束时间>=If <实际结束时间>！=NULL then =<实际结束时间> Else = “未完工，预计”+<预计实际结束时间>
                //<预计实际结束时间>=<预计实际结束时间>
                //<实际工期>=if <实际开始时间>！=NULL AND <实际结束时间>!=NULL  Then =<实际结束时间>-<实际开始时间>+1 Else if <实际开始时间>！=NULL AND <实际结束时间>=NULL  Then = “未完工，预计为”<预计实际结束时间>-<实际开始时间>+1 Else =“未开工”。

                StateCheckActionValueObject obj = new StateCheckActionValueObject();
                obj.ProjectId = dtl.Master.ProjectId;
                obj.ProjectName = dtl.Master.ProjectName;
                obj.ProjectSysCode = (dao.Get(typeof(CurrentProjectInfo), dtl.Master.ProjectId) as CurrentProjectInfo).OwnerOrgSysCode;
                obj.SchedulePlanNode = dtl;
                obj.PlanBeginTime = planBeginTime;
                obj.PlanEndTime = planEndTime;
                if (planEndTime != null && planBeginTime != null)
                    obj.PlanDuration = (planEndTime.Value - planBeginTime.Value).Days + 1;
                obj.FactBeginTime = factBeginTime != null ? factBeginTime.Value.ToShortDateString() : DurationStateEnum.未开工.ToString();
                obj.DurationState = durState;
                obj.FactEndTime = factEndTime != null ? factEndTime.Value.ToShortDateString() : DurationStateEnum.未完工 +
                    (precastFactEndTime != null ? ",预计" + precastFactEndTime.Value.ToShortDateString() + "完工" : "");

                obj.PrecastFactEndTime = precastFactEndTime;
                if (factBeginTime != null && factEndTime != null)
                    obj.FactDuration = ((factEndTime.Value - factBeginTime.Value).Days + 1).ToString();
                else if (factBeginTime != null && factEndTime == null)
                    obj.FactDuration = precastFactEndTime != null ? ("未完工,预计" + ((precastFactEndTime.Value - factBeginTime.Value).Days + 1).ToString() + "天完工") : "";
                else
                    obj.FactDuration = "未开工";


                //B.预警： 
                if (durState == DurationStateEnum.未开工)
                {
                    if (planBeginTime != null)
                    {
                        System.TimeSpan checkTime = endDate - planBeginTime.Value;
                        if (checkTime.Days <= 0)
                        {
                            warningLevel = WarningLevelEnum.无预警;
                            warningContent = "工期正常";
                        }
                        else if (checkTime.Days <= lowWarningLevel)
                        {
                            warningLevel = WarningLevelEnum.低级别;
                            warningContent = "开工拖期" + checkTime.Days + "天";
                        }
                        else if (checkTime.Days <= highWarningLevel)
                        {
                            warningLevel = WarningLevelEnum.中级别;
                            warningContent = "开工拖期" + checkTime.Days + "天";
                        }
                        else if (checkTime.Days > highWarningLevel)
                        {
                            warningLevel = WarningLevelEnum.高级别;
                            warningContent = "开工拖期" + checkTime.Days + "天";
                        }
                    }
                }
                else if (durState == DurationStateEnum.已完工)
                {
                    System.TimeSpan checkTime = factEndTime.Value - planEndTime.Value;
                    if (checkTime.Days <= 0)
                    {
                        warningLevel = WarningLevelEnum.无预警;
                        warningContent = "工期正常";
                    }
                    else if (checkTime.Days <= lowWarningLevel)
                    {
                        warningLevel = WarningLevelEnum.低级别;
                        warningContent = "实际完工拖期" + checkTime.Days + "天";
                    }
                    else if (checkTime.Days <= highWarningLevel)
                    {
                        warningLevel = WarningLevelEnum.中级别;
                        warningContent = "实际完工拖期" + checkTime.Days + "天";
                    }
                    else if (checkTime.Days > highWarningLevel)
                    {
                        warningLevel = WarningLevelEnum.高级别;
                        warningContent = "实际完工拖期" + checkTime.Days + "天";
                    }
                }
                else if (durState == DurationStateEnum.未完工)
                {
                    if (precastFactEndTime != null)
                    {
                        System.TimeSpan checkTime = precastFactEndTime.Value - planEndTime.Value;
                        if (checkTime.Days <= 0)
                        {
                            warningLevel = WarningLevelEnum.无预警;
                            warningContent = "工期正常";
                        }
                        else if (checkTime.Days <= lowWarningLevel)
                        {
                            warningLevel = WarningLevelEnum.低级别;
                            warningContent = "未完工，预计完工拖期" + checkTime.Days + "天";
                        }
                        else if (checkTime.Days <= highWarningLevel)
                        {
                            warningLevel = WarningLevelEnum.中级别;
                            warningContent = "未完工，预计完工拖期" + checkTime.Days + "天";
                        }
                        else if (checkTime.Days > highWarningLevel)
                        {
                            warningLevel = WarningLevelEnum.高级别;
                            warningContent = "未完工，预计完工拖期" + checkTime.Days + "天";
                        }
                    }
                }

                //C.预警责任人
                if (warningLevel == 0)
                    owner = null;
                else
                {
                    oq.Criterions.Clear();
                    oq.FetchModes.Clear();

                    //查询{管理OBS}_【工程项目任务】=<检查{进度计划节点}>_【工程项目任务】，取查得对象的【责任人】

                }

                #endregion

                obj.Level = warningLevel;
                obj.WarningContent = warningContent;
                obj.Owner = owner;
                if (owner != null)
                    obj.OwnerName = owner.Name;

                listSchduleCheck.Add(obj);
            }
            #endregion

            #region 写预警信息
            if (isBuildWarningFlag && listSchduleCheck.Count > 0)
            {
                //A.查询{进度检查集}_【预警级别】！=0，设置查得结果为<预警进度节点集>。
                var queryWarning = from s in listSchduleCheck
                                   where s.Level != WarningLevelEnum.无预警
                                   select s;

                WarningTarget target = null;
                oq.Criterions.Clear();
                oq.FetchModes.Clear();
                oq.AddCriterion(Expression.Eq("TargetCode", "G001"));
                IList listTarget = dao.ObjectQuery(typeof(WarningTarget), oq);
                if (listTarget != null && listTarget.Count > 0)
                {
                    target = listTarget[0] as WarningTarget;
                }

                foreach (StateCheckActionValueObject obj in queryWarning)
                {
                    WarningInfo w = new WarningInfo();
                    w.ProjectId = obj.ProjectId;
                    w.ProjectName = obj.ProjectName;
                    w.ProjectSyscode = obj.ProjectSysCode;
                    w.TheTarget = target;
                    if (target != null)
                        w.TheTargetName = target.TargetName;

                    w.TheWarningObjectId = obj.SchedulePlanNode.Id;
                    w.TheWarningObjectTypeName = obj.SchedulePlanNode.GetType().Name;
                    w.Level = obj.Level;
                    w.Owner = obj.Owner;
                    w.OwnerName = obj.OwnerName;
                    w.WarningContent = obj.WarningContent;

                    listWarningInfo.Add(w);
                }

                if (listWarningInfo != null && listWarningInfo.Count > 0)
                {
                    oq.Criterions.Clear();
                    oq.AddCriterion(Expression.Eq("ActionName", "工期状态检查"));
                    IList listCheckAction = dao.ObjectQuery(typeof(StateCheckAction), oq);
                    if (listCheckAction != null)
                    {
                        StateCheckAction optCheckAction = listCheckAction[0] as StateCheckAction;

                        //var queryProjectGroup = from w in listWarningInfo
                        //                        group w by new { projectId = w.ProjectId }
                        //                            into g
                        //                            select new { g.Key.projectId };

                        //oq.Criterions.Clear();
                        //dis = new Disjunction();
                        //foreach (var pro in queryProjectGroup)
                        //{
                        //    dis.Add(Expression.Eq("Id", pro.projectId));
                        //}
                        //oq.AddCriterion(dis);



                        oq.Criterions.Clear();
                        dis = new Disjunction();
                        foreach (string projectId in projectIds)
                        {
                            dis.Add(Expression.Eq("Id", projectId));
                        }
                        oq.AddCriterion(dis);

                        IList listWriteWaringInfoProject = dao.ObjectQuery(typeof(CurrentProjectInfo), oq);

                        foreach (CurrentProjectInfo optProject in listWriteWaringInfoProject)
                        {
                            var queryProjectWaringInfo = from w in listWarningInfo
                                                         where w.ProjectId == optProject.Id
                                                         select w;

                            WriteWarningInfo(queryProjectWaringInfo.ToList(), optCheckAction, optProject);
                        }
                    }
                }
            }
            #endregion

            if (isShowFlag)
                return listSchduleCheck;
            else
                return null;
        }
        #endregion

        #region 物资预警
        /// <summary>
        /// 物资采购合同
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="isShowFlag"></param>
        /// <param name="isBuildWarningFlag"></param>
        /// <param name="currProject"></param>
        [TransManager]
        public void CheckDurationSupplyOrder(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject)
        {
            if (projectIds == null || projectIds.Count == 0)
                return;
            //预警信息
            IList listWarningInfo = new ArrayList();

            // 集
            //List<StateCheckActionValueObjectOnWZ> listSchduleCheck = new List<StateCheckActionValueObjectOnWZ>();            

            WarningTarget warningTarget = null;
            if (isBuildWarningFlag)
            {
                warningTarget = GetWarningTargetByName(WarningTarget.WarningTarget_WZ_SupplyOrder);
                if (warningTarget == null) return;

                DataTable dt = GetSupplyOrderWarningInfo();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string projectid = dr["projectid"] + "";
                        string projectname = dr["projectname"] + "";
                        WarningInfo wi = BuildWarningInfo(projectid, projectname, warningTarget);
                        wi.WarningContent = "物资采购合同结算达到采购合同额的95%";
                        listWarningInfo.Add(wi);
                    }
                }
                DeleteWarningInfo(warningTarget);
                WriteWarningInfo(listWarningInfo);
            }
        }

        /// <summary>
        /// 物资收料管理
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="isShowFlag"></param>
        /// <param name="isBuildWarningFlag"></param>
        /// <param name="currProject"></param>
        [TransManager]
        public void CheckDurationStockIn(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject)
        {
            if (projectIds == null || projectIds.Count == 0)
                return;
            //预警信息
            IList listWarningInfo = new ArrayList();

            WarningTarget warningTarget = null;
            if (isBuildWarningFlag)
            {
                warningTarget = GetWarningTargetByName(WarningTarget.WarningTarget_WZ_StockIn);
                if (warningTarget == null) return;

                DataTable dt = GetStockInWarningInfo();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string projectid = dr["projectid"] + "";
                        string projectname = dr["projectname"] + "";
                        WarningInfo wi = BuildWarningInfo(projectid, projectname, warningTarget);
                        wi.WarningContent = "物资进场量（采购+调入）达到日常计划量的95%";
                        listWarningInfo.Add(wi);
                    }
                }
                DeleteWarningInfo(warningTarget);
                WriteWarningInfo(listWarningInfo);
            }
        }

        /// <summary>
        /// 物资日常需求计划
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="isShowFlag"></param>
        /// <param name="isBuildWarningFlag"></param>
        /// <param name="currProject"></param>
        [TransManager]
        public void CheckDurationDailyPlan(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject)
        {
            if (projectIds == null || projectIds.Count == 0)
                return;
            //预警信息
            IList listWarningInfo = new ArrayList();

            WarningTarget warningTarget = null;
            if (isBuildWarningFlag)
            {
                warningTarget = GetWarningTargetByName(WarningTarget.WarningTarget_WZ_DailyPlan);
                if (warningTarget == null) return;

                DataTable dt = GetDailyPlanWarningInfo();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string projectid = dr["projectid"] + "";
                        string projectname = dr["projectname"] + "";
                        WarningInfo wi = BuildWarningInfo(projectid, projectname, warningTarget);
                        wi.WarningContent = "物资日常需求计划累计量达到物资月度需求计划量95%";
                        listWarningInfo.Add(wi);
                    }
                }
                DeleteWarningInfo(warningTarget);
                WriteWarningInfo(listWarningInfo);
            }
        }

        /// <summary>
        /// 物资月度需求计划
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="isShowFlag"></param>
        /// <param name="isBuildWarningFlag"></param>
        /// <param name="currProject"></param>
        [TransManager]
        public void CheckDurationMonthPlan(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject)
        {
            if (projectIds == null || projectIds.Count == 0)
                return;
            //预警信息
            IList listWarningInfo = new ArrayList();

            WarningTarget warningTarget = null;
            if (isBuildWarningFlag)
            {
                warningTarget = GetWarningTargetByName(WarningTarget.WarningTarget_WZ_MonthPlan);
                if (warningTarget == null) return;

                DataTable dt = GetMonthPlanWarningInfo();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string projectid = dr["projectid"] + "";
                        string projectname = dr["projectname"] + "";
                        WarningInfo wi = BuildWarningInfo(projectid, projectname, warningTarget);
                        wi.WarningContent = "物资月度需求计划累计量达到物资需求总计划量95%";
                        listWarningInfo.Add(wi);
                    }
                }
                DeleteWarningInfo(warningTarget);
                WriteWarningInfo(listWarningInfo);
            }
        }

        /// <summary>
        /// 获取物资收料预警数据
        /// </summary>
        /// <returns></returns>
        private DataTable GetStockInWarningInfo()
        {
            string conMat = MaterialCategoryCon("b.materialcode");
            string sql = @"select distinct projectid,projectname from(
                select * from (
                select projectid,projectname,material,materialname,materialspec,sum(jhsl) jhsl,sum(shsl) shsl from(
                /*进场数量*/
                select a.projectid,a.projectname,b.material,b.materialname,b.materialspec,0 jhsl,sum(b.quantity) shsl
                from thd_stkstockin a,thd_stkstockindtl b where a.id=b.parentid and a.istally=1 
                and (a.stockinmanner=10 or a.stockinmanner=11) " + conMat + @"
                group by a.projectid,a.projectname,b.material,b.materialname,b.materialspec
                /*日计划数量**/
                union all
                select to_char(a.projectid),to_char(a.projectname),to_char(b.material),to_char(b.materialname),to_char(b.materialspec) materialspec,sum(b.quantity) jhsl,0 shsl
                from thd_dailyplanmaster a,thd_dailyplandetail b where a.id=b.parentid and a.state=5 " + conMat + @"
                group by a.projectid,a.projectname,b.material,b.materialname,
                b.materialspec) group by projectid,projectname,material,materialname,materialspec) where jhsl>0) where shsl/jhsl>=0.95";

            return ExceuteDataTable(sql);
        }

        /// <summary>
        /// 获取物资收料预警数据
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public DataTable GetStockInWarningInfoDetail(string projectId)
        {

            string conMat = MaterialCategoryCon("b.materialcode") + " and a.projectid='" + projectId + "' ";
            string sql = @"select * from(
                select * from (
                select projectid,projectname,material,materialname,materialspec,sum(jhsl) jhsl,sum(shsl) shsl from(
                /*进场数量*/
                select a.projectid,a.projectname,b.material,b.materialname,b.materialspec,0 jhsl,sum(b.quantity) shsl
                from thd_stkstockin a,thd_stkstockindtl b where a.id=b.parentid and a.istally=1 " + conMat + @"
                and (a.stockinmanner=10 or a.stockinmanner=11)
                group by a.projectid,a.projectname,b.material,b.materialname,b.materialspec
                /*日计划数量**/
                union all
                select to_char(a.projectid),to_char(a.projectname),to_char(b.material),to_char(b.materialname),to_char(b.materialspec) materialspec,sum(b.quantity) jhsl,0 shsl
                from thd_dailyplanmaster a,thd_dailyplandetail b where a.id=b.parentid and a.state=5 " + conMat + @"
                group by a.projectid,a.projectname,b.material,b.materialname,
                b.materialspec) group by projectid,projectname,material,materialname,materialspec) where jhsl>0) where shsl/jhsl>=0.95";

            return ExceuteDataTable(sql);
        }

        /// <summary>
        /// 获取物资日常需求计划预警数据
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public DataTable GetDailyPlanWarningInfoDetail(string projectId)
        {
            string conMat = MaterialCategoryCon("b.materialcode") + " and a.projectid='" + projectId + "' ";
            string sql = @"select * from(
                select * from (
                select projectid,projectname,material,materialcode,materialname,materialspec,sum(rjhsl) rjhsl,sum(yjhsl) yjhsl from(
                /*月计划*/
                select a.projectid,a.projectname,b.material,b.materialcode,b.materialname,b.materialspec,0 rjhsl,sum(b.quantity) yjhsl
                from thd_monthlyplanmaster a,thd_monthlyplandetail b where a.id=b.parentid and a.state=5 " + conMat + @"
                group by a.projectid,a.projectname,b.material,b.materialcode,b.materialname,b.materialspec
                /*日计划数量**/
                union all
                select a.projectid,a.projectname,b.material,b.materialcode,b.materialname,b.materialspec materialspec,sum(b.quantity) rjhsl,0 yjhsl
                from thd_dailyplanmaster a,thd_dailyplandetail b where a.id=b.parentid and a.state=5 " + conMat + @"
                group by a.projectid,a.projectname,b.material,b.materialname,b.materialcode,
                b.materialspec) group by projectid,projectname,material,materialcode,materialname,materialspec) where yjhsl>0) where rjhsl/yjhsl>=0.95";

            return ExceuteDataTable(sql);
        }

        /// <summary>
        /// 获取物资日常需求计划预警数据
        /// </summary>
        /// <returns></returns>
        private DataTable GetDailyPlanWarningInfo()
        {
            string conMat = MaterialCategoryCon("b.materialcode");
            string sql = @"select distinct projectid,projectname from(
                select * from (
                select projectid,projectname,material,materialcode,materialname,materialspec,sum(rjhsl) rjhsl,sum(yjhsl) yjhsl from(
                /*月计划*/
                select a.projectid,a.projectname,b.material,b.materialcode,b.materialname,b.materialspec,0 rjhsl,sum(b.quantity) yjhsl
                from thd_monthlyplanmaster a,thd_monthlyplandetail b where a.id=b.parentid and a.state=5 " + conMat + @"
                group by a.projectid,a.projectname,b.material,b.materialcode,b.materialname,b.materialspec
                /*日计划数量**/
                union all
                select a.projectid,a.projectname,b.material,b.materialcode,b.materialname,b.materialspec materialspec,sum(b.quantity) rjhsl,0 yjhsl
                from thd_dailyplanmaster a,thd_dailyplandetail b where a.id=b.parentid and a.state=5 " + conMat + @"
                group by a.projectid,a.projectname,b.material,b.materialname,b.materialcode,
                b.materialspec) group by projectid,projectname,material,materialcode,materialname,materialspec) where yjhsl>0) where rjhsl/yjhsl>=0.95";

            return ExceuteDataTable(sql);
        }

        /// <summary>
        /// 获取物资月度需求计划预警数据
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public DataTable GetMonthPlanWarningInfoDetail(string projectId)
        {
            string conMat = MaterialCategoryCon("b.materialcode") + " and a.projectid='" + projectId + "' ";
            string sql = @"select * from(
                select * from (
                select projectid,projectname,material,materialcode,materialname,materialspec,sum(zjhsl) zjhsl,sum(yjhsl) yjhsl from(
                /*月计划*/
                select a.projectid,a.projectname,b.material,b.materialcode,b.materialname,b.materialspec,0 zjhsl,sum(b.quantity) yjhsl
                from thd_monthlyplanmaster a,thd_monthlyplandetail b where a.id=b.parentid and a.state=5 " + conMat + @"
                group by a.projectid,a.projectname,b.material,b.materialcode,b.materialname,b.materialspec
                /*总计划数量**/
                union all
                select a.projectid,a.projectname,b.material,b.materialcode,b.materialname,b.materialspec materialspec,sum(b.quantity) zjhsl,0 yjhsl
                from thd_demandmasterplanmaster a,thd_demandmasterplandetail b where a.id=b.parentid and a.state=5 " + conMat + @"
                group by a.projectid,a.projectname,b.material,b.materialname,b.materialcode,
                b.materialspec) group by projectid,projectname,material,materialcode,materialname,materialspec) where zjhsl>0) where yjhsl/zjhsl>=0.95";
            return ExceuteDataTable(sql);
        }

        /// <summary>
        /// 获取物资月度需求计划预警数据
        /// </summary>
        /// <returns></returns>
        private DataTable GetMonthPlanWarningInfo()
        {
            string conMat = MaterialCategoryCon("b.materialcode");
            string sql = @"select distinct projectid,projectname from(
                select * from (
                select projectid,projectname,material,materialcode,materialname,materialspec,sum(zjhsl) zjhsl,sum(yjhsl) yjhsl from(
                /*月计划*/
                select a.projectid,a.projectname,b.material,b.materialcode,b.materialname,b.materialspec,0 zjhsl,sum(b.quantity) yjhsl
                from thd_monthlyplanmaster a,thd_monthlyplandetail b where a.id=b.parentid and a.state=5 " + conMat + @"
                group by a.projectid,a.projectname,b.material,b.materialcode,b.materialname,b.materialspec
                /*总计划数量**/
                union all
                select a.projectid,a.projectname,b.material,b.materialcode,b.materialname,b.materialspec materialspec,sum(b.quantity) zjhsl,0 yjhsl
                from thd_demandmasterplanmaster a,thd_demandmasterplandetail b where a.id=b.parentid and a.state=5 " + conMat + @"
                group by a.projectid,a.projectname,b.material,b.materialname,b.materialcode,
                b.materialspec) group by projectid,projectname,material,materialcode,materialname,materialspec) where zjhsl>0) where yjhsl/zjhsl>=0.95";
            return ExceuteDataTable(sql);
        }

        private WarningInfo BuildWarningInfo(string projectId, string projectName, WarningTarget warningTarget)
        {
            WarningInfo wi = new WarningInfo();
            wi.Level = WarningLevelEnum.低级别;
            wi.ProjectId = projectId;
            wi.ProjectName = projectName;
            wi.State = WarningInfoStateEnum.有效;
            wi.SubmitTime = DateTime.Now;
            wi.TheTarget = warningTarget;
            wi.TheTargetName = warningTarget.TargetName;
            return wi;
        }

        private void DeleteWarningInfo(WarningTarget warningTarget)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheTarget.Id", warningTarget.Id));
            IList lstWi = dao.ObjectQuery(typeof(WarningInfo), oq);
            dao.Delete(lstWi);
            return;
        }

        private WarningTarget GetWarningTargetByName(string targetName)
        {
            WarningTarget warningTarget = null;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TargetName", targetName));
            IList lstWt = dao.ObjectQuery(typeof(WarningTarget), oq);
            if (lstWt != null && lstWt.Count > 0)
            {
                warningTarget = lstWt[0] as WarningTarget;
            }
            return warningTarget;
        }

        /// <summary>
        /// 获取采购合同预警数据
        /// </summary>
        /// <returns></returns>
        private DataTable GetSupplyOrderWarningInfo()
        {
            string conMat = MaterialCategoryCon("b.materialcode");
            string sql = @"select distinct projectid,projectname from (
                select a.projectid,a.projectname,b.material,d.supplyorderdetailid,sum(c.quantity*b.price) jssl,sum(e.money) htsl
                from thd_stockinbalmaster a,thd_stockinbaldetail b,thd_stockinbaldetail_fwddtl c,thd_stkstockindtl d,thd_supplyorderdetail e
                where a.id=b.parentid and c.stockinbaldetail=b.id and c.forwarddetailid=d.id and d.supplyorderdetailid=e.id
                and a.state=5 and a.thestockinoutkind=0 and b.quantity-b.refquantity>0 and e.money>0 " + conMat + @"
                group by b.material,d.supplyorderdetailid,a.projectid,a.projectname) where jssl/htsl>=0.95";
            return ExceuteDataTable(sql);
        }

        private DataTable ExceuteDataTable(string sql)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = sql;
            if (command.Connection.State != ConnectionState.Open)
            {
                session = CallContext.GetData("nhsession") as ISession;
                conn = session.Connection;
                command.Connection = conn;
                session.Transaction.Enlist(command);
            }
            IDataReader dr = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dr);
            DataTable dt = new DataTable();
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        /// <summary>
        /// 获取采购合同预警数据
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public DataTable GetSupplyOrderWarningInfoDetail(string projectId)
        {
            string conMat = MaterialCategoryCon("b.materialcode") + " and a.projectid='" + projectId + "' ";
            string sql = @"select * from 
                (select a.projectid,a.projectname,b.material,b.materialname,b.materialspec,
                d.supplyorderdetailid,sum(c.quantity*b.price) jssl,sum(e.money) htsl,f.code
                from thd_stockinbalmaster a,thd_stockinbaldetail b,thd_stockinbaldetail_fwddtl c,thd_stkstockindtl d,
                thd_supplyorderdetail e,thd_supplyordermaster f
                where a.id=b.parentid and c.stockinbaldetail=b.id and c.forwarddetailid=d.id and d.supplyorderdetailid=e.id and e.parentid=f.id
                and a.state=5 and a.thestockinoutkind=0 and b.quantity-b.refquantity>0 and e.money>0 " + conMat + @"
                group by b.material,d.supplyorderdetailid,a.projectid,a.projectname,b.materialname,b.materialspec,f.code)
                where jssl/htsl>=0.95";

            return ExceuteDataTable(sql);
        }

        /// <summary>
        /// 物资分类过滤条件
        /// </summary>
        /// <param name="colNameCon">形如b.materialcode</param>
        /// <returns></returns>
        private string MaterialCategoryCon(string colNameCon)
        {
            IList lstMat = GetMaterialCategoryBasicData();
            string con = "";
            if (lstMat.Count > 0)
            {
                for (int i = 0; i < lstMat.Count; i++)
                {
                    BasicDataOptr bdo = lstMat[i] as BasicDataOptr;
                    if (i == 0)
                    {
                        con += " and (" + colNameCon + " like '" + bdo.BasicCode + "%'";
                    }
                    else
                    {
                        con += " or " + colNameCon + " like '" + bdo.BasicCode + "%'";
                    }
                }
                con += ")";
            }
            return con;
        }

        private static IList lstMaterialCategoryBasicData = new ArrayList();
        /// <summary>
        /// 用于过滤的物资分类基础数据
        /// </summary>
        /// <returns></returns>
        private IList GetMaterialCategoryBasicData()
        {
            if (lstMaterialCategoryBasicData != null && lstMaterialCategoryBasicData.Count > 0) return lstMaterialCategoryBasicData;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BasicName", "预警物资分类"));
            IList lstTemp = dao.ObjectQuery(typeof(BasicDataOptr), oq);
            if (lstTemp != null && lstTemp.Count > 0)
            {
                BasicDataOptr pbd = lstTemp[0] as BasicDataOptr;
                oq.Criterions.Clear();
                oq.AddCriterion(Expression.Eq("ParentId", pbd.Id));
                lstMaterialCategoryBasicData = dao.ObjectQuery(typeof(BasicDataOptr), oq);
            }
            return lstMaterialCategoryBasicData;
        }
        #endregion

        #region 资料状态检查
        /// <summary>
        /// 资料状态检查
        /// </summary>
        /// <param name="projectIds">项目Id集</param>
        /// <param name="isShowFlag">显示标记</param>
        /// <param name="isBuildWarningFlag">预警标记</param>
        /// <param name="currProject">当前操作的项目</param>
        /// <returns></returns>
        [TransManager]
        public List<StateCheckActionValueObjectOnMeans> CheckInformationState(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject)
        {
            if (projectIds == null || projectIds.Count == 0)
                return null;

            // 进度检查集
            List<StateCheckActionValueObjectOnMeans> listSchduleCheck = new List<StateCheckActionValueObjectOnMeans>();

            //预警信息
            List<WarningInfo> listWarningInfo = new List<WarningInfo>();

            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (string id in projectIds)
            {
                dis.Add(Expression.Eq("ProjectID", id));
            }
            oq.AddCriterion(dis);
            oq.AddCriterion(Expression.Eq("VerifySwitch", ProjectDocumentVerifySwitch.验证));
            oq.AddFetchMode("ProjectTask", NHibernate.FetchMode.Eager);

            IList listDocVerify = dao.ObjectQuery(typeof(ProjectDocumentVerify), oq);

            int lowWarningLevel = 3;
            int highWarningLevel = 7;

            DateTime endDate = DateTime.Now.Date; //最近上一个午夜时间

            foreach (ProjectDocumentVerify verify in listDocVerify)
            {
                //A.设置：
                //<所属项目>：=<检查{工程文档验证}>_【所属工程项目任务】关联{工程项目任务}_【所属项目】
                //<工程项目任务>：=<检查{工程文档验证}>_【所属工程项目任务】
                //<文档名称>：=<检查{工程文档验证}>_【文档名称】
                //<提交状态>：=<检查{工程文档验证}>_【提交状态】


                StateCheckActionValueObjectOnMeans obj = new StateCheckActionValueObjectOnMeans();
                CurrentProjectInfo optProject = dao.Get(typeof(CurrentProjectInfo), verify.ProjectTask.TheProjectGUID) as CurrentProjectInfo;
                obj.ProjectId = optProject.Id;
                obj.ProjectName = optProject.Name;
                obj.ProjectSysCode = optProject.OwnerOrgSysCode;
                obj.TaskNode = verify.ProjectTask;
                obj.TaskNodeName = verify.ProjectTaskName;
                obj.TaskNodeSysCode = verify.ProjectTask.SysCode;
                obj.DocumentName = verify.DocumentName;
                obj.submitState = verify.SubmitState;

                if (obj.submitState == ProjectDocumentSubmitState.已提交)
                {
                    obj.Level = WarningLevelEnum.无预警;
                    obj.WarningContent = "文档已提交.";
                }
                else
                {
                    //B.预警： 
                    //a）如果<提交状态>=2.已提交 Then：<预警级别>=0.无预警，<预警信息>=“文档已提交。”
                    //b）如果<提交状态>=1.未提交 Then：
                    //   <检查时间>=（<截止时间>-<检查{工程文档验证}>_【验证开关打开时间】）/24 (转换成天)
                    //   if <检查时间> <= <低级别预警标准> Then <预警级别>="1.低级别"，<预警信息>=“文档”+<文档名称>+“提交拖期”+<检查时间>+“天”
                    //   else if <检查时间> <= <中级别预警标准>  Then <预警级别>="2.中级别"，<预警信息>=“文档”+<文档名称>+“提交拖期”+<检查时间>+“天” 
                    //   else if <检查时间> > <中级别预警标准>  Then <预警级别>="3.高级别"，<预警信息>=“文档”+<文档名称>+“提交拖期”+<检查时间>+“天” 

                    TimeSpan checkTime = endDate - DateTime.Now;

                    if (checkTime.Days <= lowWarningLevel)
                    {
                        obj.Level = WarningLevelEnum.低级别;
                        obj.WarningContent = "文档" + obj.DocumentName + "提交拖期" + checkTime.Days + "天";
                    }
                    else if (checkTime.Days <= highWarningLevel)
                    {
                        obj.Level = WarningLevelEnum.中级别;
                        obj.WarningContent = "文档" + obj.DocumentName + "提交拖期" + checkTime.Days + "天";
                    }
                    else if (checkTime.Days > highWarningLevel)
                    {
                        obj.Level = WarningLevelEnum.高级别;
                        obj.WarningContent = "文档" + obj.DocumentName + "提交拖期" + checkTime.Days + "天";
                    }
                }

                //C.预警责任人
                //查询{管理OBS}_【工程项目任务】=<工程项目任务>，取查得对象的【责任人】
                //

                listSchduleCheck.Add(obj);
            }

            #region 写预警信息
            if (isBuildWarningFlag)
            {
                //A.查询{进度检查集}_【预警级别】！=0，设置查得结果为<预警进度节点集>。
                var queryWarning = from s in listSchduleCheck
                                   where s.Level != WarningLevelEnum.无预警
                                   select s;

                if (queryWarning != null && queryWarning.Count() > 0)
                {
                    WarningTarget target = null;
                    oq.Criterions.Clear();
                    oq.FetchModes.Clear();
                    oq.AddCriterion(Expression.Eq("TargetCode", "A001"));
                    IList listTarget = dao.ObjectQuery(typeof(WarningTarget), oq);
                    if (listTarget != null && listTarget.Count > 0)
                    {
                        target = listTarget[0] as WarningTarget;
                    }

                    foreach (StateCheckActionValueObjectOnMeans obj in queryWarning)
                    {
                        WarningInfo w = new WarningInfo();
                        w.ProjectId = obj.ProjectId;
                        w.ProjectName = obj.ProjectName;
                        w.ProjectSyscode = obj.ProjectSysCode;

                        w.TheTarget = target;
                        if (target != null)
                            w.TheTargetName = target.TargetName;

                        w.TheWarningObjectId = obj.TaskNode.Id;
                        w.TheWarningObjectTypeName = obj.TaskNode.GetType().Name;
                        w.Level = obj.Level;
                        w.Owner = obj.Owner;
                        w.OwnerName = obj.OwnerName;
                        w.WarningContent = obj.WarningContent;

                        listWarningInfo.Add(w);
                    }

                    oq.Criterions.Clear();
                    oq.AddCriterion(Expression.Eq("ActionName", "资料状态检查"));
                    IList listCheckAction = dao.ObjectQuery(typeof(StateCheckAction), oq);
                    if (listCheckAction != null)
                    {

                        StateCheckAction optCheckAction = listCheckAction[0] as StateCheckAction;

                        oq.Criterions.Clear();
                        dis = new Disjunction();
                        foreach (string projectId in projectIds)
                        {
                            dis.Add(Expression.Eq("Id", projectId));
                        }
                        oq.AddCriterion(dis);

                        IList listWriteWaringInfoProject = dao.ObjectQuery(typeof(CurrentProjectInfo), oq);

                        foreach (CurrentProjectInfo optProject in listWriteWaringInfoProject)
                        {
                            var queryProjectWaringInfo = from w in listWarningInfo
                                                         where w.ProjectId == optProject.Id
                                                         select w;

                            WriteWarningInfo(queryProjectWaringInfo.ToList(), optCheckAction, optProject);
                        }
                    }
                }
            }
            #endregion

            return listSchduleCheck;
        }
        #endregion

        #region 商务综合指标检查

        /// <summary>
        /// 获取清单WBS树节点完整路径
        /// </summary>
        /// <param name="cateEntityType"></param>
        /// <param name="nodeName"></param>
        /// <param name="nodeSysCode"></param>
        /// <returns></returns>
        private string GetQWBSTreeFullPath(string nodeName, string nodeSysCode)
        {
            string path = string.Empty;

            path = nodeName;

            if (string.IsNullOrEmpty(nodeSysCode))
                return path;

            string[] sysCodes = nodeSysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            for (int i = 0; i < sysCodes.Length - 1; i++)
            {
                string sysCode = "";
                for (int j = 0; j <= i; j++)
                {
                    sysCode += sysCodes[j] + ".";

                }

                dis.Add(Expression.Eq("SysCode", sysCode));
            }
            oq.AddCriterion(dis);
            IList list = dao.ObjectQuery(typeof(QWBSManage), oq);

            if (list.Count > 0)
            {
                IEnumerable<QWBSManage> queryParent = list.OfType<QWBSManage>();

                queryParent = from p in queryParent
                              orderby p.SysCode.Length descending
                              select p;

                foreach (QWBSManage cate in queryParent)
                {
                    path = cate.TaskName + "\\" + path;
                }
            }

            return path;
        }

        /// <summary>
        /// 任务收款状态检查
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="isShowFlag"></param>
        /// <param name="isBuildWarningFlag"></param>
        /// <param name="currProject"></param>
        /// <returns></returns>
        [TransManager]
        public List<OwnerQuantity> CheckTaskProceedsState(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject)
        {
            List<OwnerQuantity> listQuantity = new List<OwnerQuantity>();

            if (projectIds == null || projectIds.Count == 0)
                return listQuantity;

            //预警信息集
            List<WarningInfo> listWarningInfo = new List<WarningInfo>();

            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (string id in projectIds)
            {
                dis.Add(Expression.Eq("ProjectId", id));
            }
            oq.AddCriterion(dis);
            IEnumerable<OwnerQuantity> listOwnerQuantity = dao.ObjectQuery(typeof(OwnerQuantity), oq).OfType<OwnerQuantity>();

            oq.Criterions.Clear();

            //查询需要写预警信息的业主报量状态数据
            IEnumerable<OwnerQuantity> queryWarningData = from q in listOwnerQuantity
                                                          where (decimal)(q.RealCollectionMoney / q.SumConfirmMoney) < (decimal)0.95
                                                          select q;
            #region 写预警信息
            if (isBuildWarningFlag && queryWarningData.Count() > 0)
            {

                listQuantity = queryWarningData.ToList();

                WarningTarget target = null;
                oq.Criterions.Clear();
                oq.FetchModes.Clear();

                oq.AddCriterion(Expression.Eq("CheckAction.ActionName", WarningTarget.WarningTarget_SW_ComprehensiveCheck));
                oq.AddCriterion(Expression.Eq("TargetName", WarningTarget.WarningTarget_SW_ProceedsTarget));

                IList listTarget = dao.ObjectQuery(typeof(WarningTarget), oq);

                if (listTarget != null && listTarget.Count > 0)
                {
                    target = listTarget[0] as WarningTarget;

                    var queryProjectGroup = from q in listQuantity
                                            group q by new { q.ProjectId }
                                                into g
                                                select new { g.Key.ProjectId };

                    oq.Criterions.Clear();
                    oq.FetchModes.Clear();
                    dis = new Disjunction();

                    foreach (var projectObj in queryProjectGroup)
                    {
                        dis.Add(Expression.Eq("Id", projectObj.ProjectId));
                    }
                    oq.AddCriterion(dis);

                    IEnumerable<CurrentProjectInfo> listWriteWaringInfoProject = dao.ObjectQuery(typeof(CurrentProjectInfo), oq).OfType<CurrentProjectInfo>();

                    //取业主报量单上的责任人
                    oq.Criterions.Clear();
                    dis = new Disjunction();
                    foreach (OwnerQuantity obj in queryWarningData)
                    {
                        dis.Add(Expression.Eq("QWBS.Id", obj.QWBSGUID.Id));
                    }
                    oq.AddCriterion(dis);
                    oq.AddFetchMode("Master.CreatePerson", FetchMode.Eager);
                    IEnumerable<OwnerQuantityDetail> listOwnerQnyDtl = dao.ObjectQuery(typeof(OwnerQuantityDetail), oq).OfType<OwnerQuantityDetail>();

                    foreach (OwnerQuantity obj in queryWarningData)
                    {
                        WarningInfo w = new WarningInfo();
                        w.ProjectId = obj.ProjectId;
                        w.ProjectName = obj.ProjectName;

                        var queryProject = from p in listWriteWaringInfoProject
                                           where p.Id == obj.ProjectId
                                           select p;
                        if (queryProject.Count() > 0)
                            w.ProjectSyscode = queryProject.ElementAt(0).OwnerOrgSysCode;

                        w.TheTarget = target;
                        if (target != null)
                            w.TheTargetName = target.TargetName;

                        w.TheWarningObjectId = obj.Id;
                        w.TheWarningObjectTypeName = obj.GetType().Name;
                        w.Level = WarningLevelEnum.低级别;


                        var queryOwnerQnyDtl = from p in listOwnerQnyDtl
                                               where p.QWBS.Id == obj.QWBSGUID.Id
                                               select p;

                        if (queryOwnerQnyDtl.Count() > 0)
                        {
                            PersonInfo owner = queryOwnerQnyDtl.ElementAt(0).Master.CreatePerson;

                            w.Owner = owner;
                            if (owner != null)
                                w.OwnerName = owner.Name;
                        }

                        w.WarningContent = "清单项目任务“" + GetQWBSTreeFullPath(obj.QWBSName, obj.QWBSSysCode) + "”的【业主确认累积金额】低于【实际收款累积金额】的 95%。";

                        listWarningInfo.Add(w);
                    }


                    foreach (CurrentProjectInfo optProject in listWriteWaringInfoProject)
                    {
                        var queryProjectWaringInfo = from w in listWarningInfo
                                                     where w.ProjectId == optProject.Id
                                                     select w;

                        WriteWarningInfo(queryProjectWaringInfo.ToList(), target, optProject);

                    }
                }
            }
            #endregion

            if (isShowFlag)
                return listQuantity;
            else
                return null;

        }

        /// <summary>
        /// 业主报量指标检查
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="isShowFlag"></param>
        /// <param name="isBuildWarningFlag"></param>
        /// <param name="currProject"></param>
        /// <returns></returns>
        [TransManager]
        public List<OwnerQuantity> CheckOwnerQuantityTarget(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject)
        {
            List<OwnerQuantity> listQuantity = new List<OwnerQuantity>();

            if (projectIds == null || projectIds.Count == 0)
                return listQuantity;

            //预警信息集
            List<WarningInfo> listWarningInfo = new List<WarningInfo>();

            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (string id in projectIds)
            {
                dis.Add(Expression.Eq("ProjectId", id));
            }
            oq.AddCriterion(dis);
            IEnumerable<OwnerQuantity> listOwnerQuantity = dao.ObjectQuery(typeof(OwnerQuantity), oq).OfType<OwnerQuantity>();

            oq.Criterions.Clear();

            //查询需要写预警信息的业主报量状态数据
            IEnumerable<OwnerQuantity> queryWarningData = from q in listOwnerQuantity
                                                          where (decimal)(q.SumConfirmMoney / q.SumSubmitMoney) < (decimal)1
                                                          select q;

            #region 写预警信息
            if (isBuildWarningFlag && queryWarningData.Count() > 0)
            {
                listQuantity = queryWarningData.ToList();

                WarningTarget target = null;
                oq.Criterions.Clear();
                oq.FetchModes.Clear();

                oq.AddCriterion(Expression.Eq("CheckAction.ActionName", WarningTarget.WarningTarget_SW_ComprehensiveCheck));
                oq.AddCriterion(Expression.Eq("TargetName", WarningTarget.WarningTarget_SW_OwnerQuantityTarget));

                IList listTarget = dao.ObjectQuery(typeof(WarningTarget), oq);

                if (listTarget != null && listTarget.Count > 0)
                {
                    target = listTarget[0] as WarningTarget;

                    var queryProjectGroup = from q in listQuantity
                                            group q by new { q.ProjectId }
                                                into g
                                                select new { g.Key.ProjectId };

                    oq.Criterions.Clear();
                    oq.FetchModes.Clear();
                    dis = new Disjunction();

                    foreach (var projectObj in queryProjectGroup)
                    {
                        dis.Add(Expression.Eq("Id", projectObj.ProjectId));
                    }
                    oq.AddCriterion(dis);

                    IEnumerable<CurrentProjectInfo> listWriteWaringInfoProject = dao.ObjectQuery(typeof(CurrentProjectInfo), oq).OfType<CurrentProjectInfo>();

                    //取业主报量单上的责任人
                    oq.Criterions.Clear();
                    dis = new Disjunction();
                    foreach (OwnerQuantity obj in queryWarningData)
                    {
                        dis.Add(Expression.Eq("QWBS.Id", obj.QWBSGUID.Id));
                    }
                    oq.AddCriterion(dis);
                    oq.AddFetchMode("Master.CreatePerson", FetchMode.Eager);
                    IEnumerable<OwnerQuantityDetail> listOwnerQnyDtl = dao.ObjectQuery(typeof(OwnerQuantityDetail), oq).OfType<OwnerQuantityDetail>();

                    foreach (OwnerQuantity obj in queryWarningData)
                    {
                        WarningInfo w = new WarningInfo();
                        w.ProjectId = obj.ProjectId;
                        w.ProjectName = obj.ProjectName;

                        var queryProject = from p in listWriteWaringInfoProject
                                           where p.Id == obj.ProjectId
                                           select p;
                        if (queryProject.Count() > 0)
                            w.ProjectSyscode = queryProject.ElementAt(0).OwnerOrgSysCode;

                        w.TheTarget = target;
                        if (target != null)
                            w.TheTargetName = target.TargetName;

                        w.TheWarningObjectId = obj.Id;
                        w.TheWarningObjectTypeName = obj.GetType().Name;
                        w.Level = WarningLevelEnum.低级别;

                        var queryOwnerQnyDtl = from p in listOwnerQnyDtl
                                               where p.QWBS.Id == obj.QWBSGUID.Id
                                               select p;

                        if (queryOwnerQnyDtl.Count() > 0)
                        {
                            PersonInfo owner = queryOwnerQnyDtl.ElementAt(0).Master.CreatePerson;

                            w.Owner = owner;
                            if (owner != null)
                                w.OwnerName = owner.Name;
                        }

                        w.WarningContent = "清单项目任务“" + GetQWBSTreeFullPath(obj.QWBSName, obj.QWBSSysCode) + "”的【业主确认累积金额】小于【报送累积金额】。";


                        listWarningInfo.Add(w);
                    }

                    foreach (CurrentProjectInfo optProject in listWriteWaringInfoProject)
                    {
                        var queryProjectWaringInfo = from w in listWarningInfo
                                                     where w.ProjectId == optProject.Id
                                                     select w;

                        WriteWarningInfo(queryProjectWaringInfo.ToList(), target, optProject);

                    }
                }
            }
            #endregion

            if (isShowFlag)
                return listQuantity;
            else
                return null;
        }

        #endregion

        #region 整改单预警
        /// <summary>
        /// 整改单 预警
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="isShowFlag"></param>
        /// <param name="isBuildWarningFlag"></param>
        /// <param name="currProject"></param>
        [TransManager]
        public void CheckRectificatNoticeMaster(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject)
        {
            if (projectIds == null || projectIds.Count == 0)
                return;
            //预警信息
            IList listWarningInfo = new ArrayList();

            WarningTarget warningTarget = null;
            if (isBuildWarningFlag)
            {
                warningTarget = GetWarningTargetByName(WarningTarget.WarningTarget_SW_RectificationNoticeMaster);
                if (warningTarget == null) return;

                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();
                dis.Add(Expression.Eq("DocState", VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InAudit));
                oq.AddCriterion(dis);
                IList list = dao.ObjectQuery(typeof(RectificationNoticeMaster), oq);
                if (list != null && list.Count > 0)
                {
                    foreach (RectificationNoticeMaster r in list)
                    {
                        string projectid = r.ProjectId;
                        string projectname = r.ProjectName;
                        WarningInfo wi = BuildWarningInfo(projectid, projectname, warningTarget);
                        wi.WarningContent = "整改单单据号【" + r.Code + "】未回复";
                        listWarningInfo.Add(wi);
                    }
                }
                DeleteWarningInfo(warningTarget);
                WriteWarningInfo(listWarningInfo);
            }
        }
        #endregion

        #region 专业检查 预警
        /// <summary>
        /// 专业检查 预警
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="isShowFlag"></param>
        /// <param name="isBuildWarningFlag"></param>
        /// <param name="currProject"></param>
        [TransManager]
        public void CheckProfessionInspectionRecordMaster(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject)
        {
            if (projectIds == null || projectIds.Count == 0)
                return;
            //预警信息
            IList listWarningInfo = new ArrayList();

            // 集
            //List<StateCheckActionValueObjectOnWZ> listSchduleCheck = new List<StateCheckActionValueObjectOnWZ>();            

            WarningTarget warningTarget = null;
            if (isBuildWarningFlag)
            {
                warningTarget = GetWarningTargetByName(WarningTarget.WarningTarget_SW_ProfessionInspectionRecordMaster);
                if (warningTarget == null) return;

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("DocState", VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute));
                //oq.AddCriterion(Expression.Eq("ProjectId", currProject.Id));
                IList list = dao.ObjectQuery(typeof(ProfessionInspectionRecordMaster), oq);
                if (list != null && list.Count > 0)
                {
                    foreach (ProfessionInspectionRecordMaster r in list)
                    {
                        string projectid = r.ProjectId;
                        string projectname = r.ProjectName;
                        WarningInfo wi = BuildWarningInfo(projectid, projectname, warningTarget);
                        wi.WarningContent = "执行中";
                        listWarningInfo.Add(wi);
                    }
                }
                DeleteWarningInfo(warningTarget);
                WriteWarningInfo(listWarningInfo);
            }
        }
        #endregion

        #region  工单商务复核 预警
        /// <summary>
        /// 工单商务复核
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="isShowFlag"></param>
        /// <param name="isBuildWarningFlag"></param>
        /// <param name="currProject"></param>
        [TransManager]
        public void WorkOrderBusinessReview(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject)
        {
            if (projectIds == null || projectIds.Count == 0)
                return;
            WarningTarget warningTarget = null;
            if (isBuildWarningFlag)
            {
                warningTarget = GetWarningTargetByName(WarningTarget.WarningTarget_SW_ProfessionInspectionRecordMaster);
                if (warningTarget == null) return;
            }
            IList listWarningInfo = new ArrayList();//预警信息
            #region 提报工程量 没核算 
            IList listConfirm = null;//确认单明细集
            //虚拟工单
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("AccountingState", EnumGWBSTaskConfirmAccountingState.未核算));
            oq.AddCriterion(Expression.Not(Expression.Eq("ActualCompletedQuantity", Convert.ToDecimal(0))));
            oq.AddCriterion(Expression.Eq("Master.BillType", EnumConfirmBillType.虚拟工单));
            oq.AddCriterion(Expression.Eq("GWBSDetail.State", DocumentState.InExecute));
            //oq.AddFetchMode("Master", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("GWBSTree",NHibernate.FetchMode.Eager);
            listConfirm = dao.ObjectQuery(typeof(GWBSTaskConfirm), oq);
            oq.Criterions.Clear();
            oq.FetchModes.Clear();
            if (listConfirm == null || listConfirm.Count == 0)
            {
                //查找依据项目周计划开的工单
                oq.AddCriterion(Expression.Eq("AccountingState", EnumGWBSTaskConfirmAccountingState.未核算));
                oq.AddCriterion(Expression.Eq("Master.DocState", DocumentState.InExecute));//审批通过
                oq.AddCriterion(Expression.Eq("Master.BillType", EnumConfirmBillType.计划工单));
                //oq.AddFetchMode("Master", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);
                listConfirm = dao.ObjectQuery(typeof(GWBSTaskConfirm), oq);
                oq.Criterions.Clear();
                oq.FetchModes.Clear();
            }

            if (listConfirm != null && listConfirm.Count > 0)
            {
                foreach (GWBSTaskConfirm c in listConfirm)
                {
                    string projectid = c.GWBSTree.TheProjectGUID;
                    string projectname = c.GWBSTree.TheProjectName;
                    WarningInfo wi = BuildWarningInfo(projectid, projectname, warningTarget);
                    wi.WarningContent = "节点“"+c.GWBSTree.FullPath+"”下的明细【" +c.GWBSDetailName +"】提报工程量没核算";
                    listWarningInfo.Add(wi);
                }
            }
            #endregion
            #region 核算单没进行分包结算单维护
            oq.AddCriterion(Expression.Not(Expression.Eq("DocState", DocumentState.Invalid)));
            oq.AddFetchMode("Details",NHibernate.FetchMode.Eager);
            IList list_PTaskAccount = dao.ObjectQuery(typeof(ProjectTaskAccountBill), oq);
            oq.Criterions.Clear();
            oq.FetchModes.Clear();
            // 结算工程任务核算明细
            foreach (ProjectTaskAccountBill account in list_PTaskAccount)
            {
                //工程任务核算明细信息
                foreach (ProjectTaskDetailAccount detail in account.Details)
                {
                    if (string.IsNullOrEmpty(detail.BalanceDtlGUID))
                    {
                        string projectid = account.ProjectId;
                    string projectname = account.ProjectName;
                    WarningInfo wi = BuildWarningInfo(projectid, projectname, warningTarget);
                    wi.WarningContent = "核算单单据号“"+account.Code+"”下的核算明细【" +detail.ProjectTaskDtlName +"】未分包结算";
                    listWarningInfo.Add(wi);
                    }
                }
            }
            
            #endregion

            DeleteWarningInfo(warningTarget);
            WriteWarningInfo(listWarningInfo);
        }
        #endregion

        #region  设备租赁费用结算 预警
        /// <summary>
        /// 设备租赁费用结算
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="isShowFlag"></param>
        /// <param name="isBuildWarningFlag"></param>
        /// <param name="currProject"></param>
        public void RentalCostClear(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject)
        {
            if (projectIds == null || projectIds.Count == 0)
                return;
            //预警信息
            IList listWarningInfo = new ArrayList();

            WarningTarget warningTarget = null;
            if (isBuildWarningFlag)
            {
                warningTarget = GetWarningTargetByName(WarningTarget.WarningTarget_SW_RectificationNoticeMaster);
                if (warningTarget == null) return;

                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();
                dis.Add(Expression.Eq("DocState", VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute));
                dis.Add(Expression.Eq("DocState", VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit));
                oq.AddCriterion(dis);
                IList list = dao.ObjectQuery(typeof(RectificationNoticeMaster), oq);
                if (list != null && list.Count > 0)
                {
                    foreach (RectificationNoticeMaster r in list)
                    {
                        string projectid = r.ProjectId;
                        string projectname = r.ProjectName;
                        WarningInfo wi = BuildWarningInfo(projectid, projectname, warningTarget);
                        wi.WarningContent = "整改单单据号【" + r.Code + "】未回复";
                        listWarningInfo.Add(wi);
                    }
                }
                DeleteWarningInfo(warningTarget);
                WriteWarningInfo(listWarningInfo);
            }
        }
        #endregion

        #region 费用结算 预警
        /// <summary>
        /// 费用结算
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="isShowFlag"></param>
        /// <param name="isBuildWarningFlag"></param>
        /// <param name="currProject"></param>
        public void CostClear(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject)
        {
            if (projectIds == null || projectIds.Count == 0)
                return;
            //预警信息
            IList listWarningInfo = new ArrayList();

            WarningTarget warningTarget = null;
            if (isBuildWarningFlag)
            {
                warningTarget = GetWarningTargetByName(WarningTarget.WarningTarget_SW_CostClear);
                if (warningTarget == null) return;

                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();
                dis.Add(Expression.Eq("DocState", VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute));
                dis.Add(Expression.Eq("DocState", VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit));
                oq.AddCriterion(dis);
                IList list = dao.ObjectQuery(typeof(ExpensesSettleMaster), oq);
                if (list != null && list.Count > 0)
                {
                    foreach (ExpensesSettleMaster r in list)
                    {
                        string projectid = r.ProjectId;
                        string projectname = r.ProjectName;
                        WarningInfo wi = BuildWarningInfo(projectid, projectname, warningTarget);
                        wi.WarningContent = "费用结算单单据号【" + r.Code + "】请及时审核";
                        listWarningInfo.Add(wi);
                    }
                }
                DeleteWarningInfo(warningTarget);
                WriteWarningInfo(listWarningInfo);
            }
        }
        #endregion

        #region 成本核算 预警
        /// <summary>
        /// 成本核算
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="isShowFlag"></param>
        /// <param name="isBuildWarningFlag"></param>
        /// <param name="currProject"></param>
        public void Costing(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject)
        {
            if (projectIds == null || projectIds.Count == 0)
                return;
            //预警信息
            IList listWarningInfo = new ArrayList();

            WarningTarget warningTarget = null;
            if (isBuildWarningFlag)
            {
                warningTarget = GetWarningTargetByName(WarningTarget.WarningTarget_SW_RectificationNoticeMaster);
                if (warningTarget == null) return;

                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();
                dis.Add(Expression.Eq("DocState", VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute));
                dis.Add(Expression.Eq("DocState", VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit));
                oq.AddCriterion(dis);
                IList list = dao.ObjectQuery(typeof(RectificationNoticeMaster), oq);
                if (list != null && list.Count > 0)
                {
                    foreach (RectificationNoticeMaster r in list)
                    {
                        string projectid = r.ProjectId;
                        string projectname = r.ProjectName;
                        WarningInfo wi = BuildWarningInfo(projectid, projectname, warningTarget);
                        wi.WarningContent = "整改单单据号【" + r.Code + "】未回复";
                        listWarningInfo.Add(wi);
                    }
                }
                DeleteWarningInfo(warningTarget);
                WriteWarningInfo(listWarningInfo);
            }
        }
        #endregion
    }
}
