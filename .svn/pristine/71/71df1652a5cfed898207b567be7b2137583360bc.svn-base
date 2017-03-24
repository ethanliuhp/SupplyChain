using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Service;
using PortalIntegrationConsole.CommonClass;
using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using PortalIntegrationConsole.Service;

namespace PortalIntegrationConsole
{
    public partial class Form1 : Form
    {
        IGWBSTreeSrv model = null;
        IOrgUsersXmlSrv xmlModel = null;
        int serverExeNumber = 0;
        int repTryNumber = 0;
        ExePlanType exeType = ExePlanType.定点执行一次;

        string DataSyncLogPath = AppDomain.CurrentDomain.BaseDirectory + @"Log";
        string DataSyncLogFilePath = AppDomain.CurrentDomain.BaseDirectory + @"Log\\IRP_KB_PMDataSyncLog.txt";

        public Form1()
        {
            InitializeComponent();
            InitForm();
        }

        private void InitForm()
        {
            if (model == null)
                model = StaticMethod.GetService("SupplyChain", "GWBSTreeSrv") as IGWBSTreeSrv;

            if (xmlModel == null)
                xmlModel = StaticMethod.GetService("PortalIntegrationConsole", "OrgUsersXmlSrv") as IOrgUsersXmlSrv;

            dateTimePickerExeTime.Value = DateTime.Now;
            for (int i = 1; i < 10; i++)
            {
                txtReTryNumber.Items.Add(i);
                txtRepExeTryNumber.Items.Add(i);
            }
            txtReTryNumber.SelectedIndex = 0;
            txtRepExeTryNumber.SelectedIndex = 0;

            if (Directory.Exists(DataSyncLogPath) == false)
                Directory.CreateDirectory(DataSyncLogPath);
        }

        private void btnStartPlan_Click(object sender, EventArgs e)
        {
            try
            {
                repTryNumber = Convert.ToInt32(txtReTryNumber.Text);
                SetEnabledControl(false);
                lblServerState.Text = "执行中";
                btnStartPlan.Text = "执行中...";
                exeType = ExePlanType.定点执行一次;
                timerDataSyncPlan.Start();
            }
            catch
            {

            }
            finally
            {
            }
        }

        private void btnStartRepExePlan_Click(object sender, EventArgs e)
        {
            try
            {
                repTryNumber = Convert.ToInt32(txtRepExeTryNumber.Text);
                SetEnabledControl(false);
                lblServerState.Text = "执行中";
                btnStartRepExePlan.Text = "执行中...";
                exeType = ExePlanType.定时反复执行;
                timerDataSyncPlan.Start();
            }
            catch
            {

            }
            finally
            {
            }
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            try
            {
                SetEnabledControl(false);
                lblServerState.Text = "执行中";
                btnStartServer.Text = "执行中...";
                exeType = ExePlanType.即时执行一次;

                timerDataSyncPlan.Start();
            }
            catch
            {

            }
            finally
            {
            }
        }

        private void timerDataSyncPlan_Tick(object sender, EventArgs e)
        {
            if (exeType == ExePlanType.即时执行一次)
            {
                try
                {
                    timerDataSyncPlan.Stop();
                    MBPSyncIRPData();
                }
                catch
                {

                }
                finally
                {
                    btnStartServer.Text = "启动服务";
                    SetEnabledControl(true);
                }
            }
            else if (exeType == ExePlanType.定点执行一次 && DateTime.Now >= dateTimePickerExeTime.Value)
            {
                try
                {
                    timerDataSyncPlan.Stop();
                    MBPSyncIRPData();
                }
                catch
                {

                }
                finally
                {
                    btnStartPlan.Text = "启动计划";
                    SetEnabledControl(true);
                }
            }
            else
            {
                DateTime exeTime = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + dateTimePickerRepExeTime.Value.TimeOfDay.ToString().Trim());
                if (exeType == ExePlanType.定时反复执行 && (DateTime.Now.ToString() == exeTime.ToString() || DateTime.Now.ToString() == exeTime.AddSeconds(1).ToString()))
                {
                    try
                    {
                        MBPSyncIRPData();
                    }
                    catch
                    {

                    }
                    finally
                    {

                    }
                }
            }
        }

        private void SetEnabledControl(bool isEnabled)
        {
            btnStartPlan.Enabled = isEnabled;
            btnStartServer.Enabled = isEnabled;
            btnStartRepExePlan.Enabled = isEnabled;
            txtReTryNumber.Enabled = isEnabled;
            txtRepExeTryNumber.Enabled = isEnabled;
            dateTimePickerExeTime.Enabled = isEnabled;
            dateTimePickerRepExeTime.Enabled = isEnabled;
        }

        #region 同步数据到IRP和知识库服务

        private void MBPSyncIRPData()
        {
            try
            {
                //动作优先级逻辑(新增、删除、修改、关系变动)
                //1.[删除]优先级最高,只要[删除]标志为1,则无需判断其他标志,删除本对象,如果为[岗位],则[人员岗位关系]、[岗位角色关系]都删除
                //2.[新增]优先级次之,如果存在[新增]和[修改],则直接调用新增接口(因为目前数据是状态数据)。
                //3.[修改]优先级最低,如果没有[删除]、[新增]发生,则是[修改]接口
                //4.[关系变动]用来处理[人员岗位关系]、[岗位角色关系]

                //0.初始条件 关闭XML内存加载
                ObjectQuery oq = null;
                List<string> listSql = new List<string>();

                #region 1.同步角色
                oq = new ObjectQuery();
                //获取要同步的角色
                oq.AddCriterion(Expression.Sql(" AddState = 1 or UpdateState =  1 or DelState = 1"));
                IList list_Role = model.ObjectQuery(typeof(OperationRole), oq);
                //校验IRP和知识库中有无该角色，没有的话提示先添加角色
                string noExistsRoles = "";
                foreach (OperationRole role in list_Role)
                {
                    if (role.AddState == 1 || role.UpdateState == 1)
                    {
                        //调用IRP接口 查询[角色]；PM和IRP的角色已在数据库层面做了同步

                        //调用KB接口 查询[角色]
                        PortalIntegrationConsole.KBOrgUserSyncDataSrv.Role retRole = ReloadIRPXml.KBService.GetRoleByName(role.RoleName);
                        if (retRole == null)
                        {
                            noExistsRoles += role.RoleName + "、";
                            break;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(noExistsRoles))
                {
                    noExistsRoles = noExistsRoles.Substring(0, noExistsRoles.Length - 1);
                    lblServerState.Text = "停止";
                    MessageBox.Show("角色【" + noExistsRoles + "】在知识库中不存在，请先通过网页添加后再同步！");
                    return;
                }

                WriteDataSyncLog("数据同步服务已启动，同步开始......");

                foreach (OperationRole oprRole in list_Role)
                {
                    if (oprRole.DelState == 1 && oprRole.State == 0)//删除后没有调用addRole重新启用的角色需要物理删除
                    {
                        //调用IRP接口 删除[角色]
                        xmlModel.DeleteRoleNode(oprRole);
                        WriteDataSyncLog("IRP删除角色成功，角色代码：" + oprRole.RoleCode + ",角色名称：" + oprRole.RoleName);

                        //调用KB接口 删除[角色]
                        PortalIntegrationConsole.KBOrgUserSyncDataSrv.Role KBRole = new PortalIntegrationConsole.KBOrgUserSyncDataSrv.Role();
                        KBRole.RoleCode = oprRole.RoleCode;
                        ReloadIRPXml.KBService.DeleteRole(KBRole);
                        WriteDataSyncLog("KB删除角色成功，角色代码：" + oprRole.RoleCode + ",角色名称：" + oprRole.RoleName);

                        //修改项目管理中数据标记
                        listSql.Add("update resoperationrole set AddState = 0,UpdateState = 0,DelState = 0 where id='" + oprRole.Id + "';");
                    }
                    else if (oprRole.AddState == 1 || oprRole.UpdateState == 1)
                    {
                        //调用IRP接口 新增[岗位]
                        xmlModel.InsertRoleNode(oprRole);
                        WriteDataSyncLog("IRP修改角色成功，角色代码：" + oprRole.RoleCode + ",角色名称：" + oprRole.RoleName);

                        //调用KB接口 新增[岗位]
                        PortalIntegrationConsole.KBOrgUserSyncDataSrv.Role KBRole = new PortalIntegrationConsole.KBOrgUserSyncDataSrv.Role();
                        KBRole.RoleCode = oprRole.RoleCode;
                        KBRole.RoleName = oprRole.RoleName;
                        ReloadIRPXml.KBService.AddRole(KBRole);
                        WriteDataSyncLog("KB修改角色成功，角色代码：" + oprRole.RoleCode + ",角色名称：" + oprRole.RoleName);

                        //修改项目管理中数据标记
                        listSql.Add("update resoperationrole set AddState = 0,UpdateState = 0 where id='" + oprRole.Id + "';");
                    }
                }

                if (listSql.Count > 0)
                {
                    ExeSql(listSql);
                    WriteDataSyncLog("PM修改角色同步标记成功！此次共修改角色“" + listSql.Count + "”个");
                    listSql.Clear();
                }
                #endregion

                #region 2.同步岗位
                oq = new ObjectQuery();
                //获取要同步的岗位-非修改过岗位角色关系的数据，不需要加载岗位角色关系及角色数据
                oq.AddCriterion(Expression.Sql(" AddState = 1 or UpdateState =  1 or DelState = 1"));
                oq.AddFetchMode("OperationOrg", NHibernate.FetchMode.Eager);
                IList list_job = model.ObjectQuery(typeof(OperationJob), oq);

                foreach (OperationJob oprJob in list_job)
                {
                    if (oprJob.DelState == 1)
                    {
                        //调用IRP接口 删除[岗位]和[人员岗位关系]、[岗位角色关系]
                        xmlModel.DeleteOperationJobNode(oprJob);
                        WriteDataSyncLog("IRP删除岗位成功，岗位代码：" + oprJob.Code + ",岗位名称：" + oprJob.Name);

                        //调用KB接口 删除[岗位]和[人员岗位关系]、[岗位角色关系]
                        PortalIntegrationConsole.KBOrgUserSyncDataSrv.Post KBJob = new PortalIntegrationConsole.KBOrgUserSyncDataSrv.Post();
                        KBJob.PosiCode = oprJob.Code;
                        ReloadIRPXml.KBService.DeletePost(KBJob);
                        WriteDataSyncLog("KB删除岗位成功，岗位代码：" + oprJob.Code + ",岗位名称：" + oprJob.Name);

                        //修改项目管理中数据标记
                        listSql.Add("update resoperationjob set AddState = 0,UpdateState =  0 ,DelState = 0,RelState = 0 where opjid='" + oprJob.Id + "';");
                    }
                    else if (oprJob.AddState == 1)
                    {
                        //调用IRP接口 新增[岗位]
                        xmlModel.InsertOperationJobNode(oprJob);
                        WriteDataSyncLog("IRP新增岗位成功，岗位代码：" + oprJob.Code + ",岗位名称：" + oprJob.Name);

                        //调用KB接口 新增[岗位]
                        PortalIntegrationConsole.KBOrgUserSyncDataSrv.Post KBJob = new PortalIntegrationConsole.KBOrgUserSyncDataSrv.Post();
                        KBJob.PosiCode = oprJob.Code;
                        KBJob.PosiName = oprJob.Name;
                        KBJob.OrderNo = (int)oprJob.OrderNo;
                        KBJob.OrgCode = oprJob.OperationOrg.Code;
                        ReloadIRPXml.KBService.AddPost(oprJob.OperationOrg.Code, KBJob);
                        WriteDataSyncLog("KB新增岗位成功，岗位代码：" + oprJob.Code + ",岗位名称：" + oprJob.Name);

                        //修改项目管理中数据标记
                        listSql.Add("update resoperationjob set AddState = 0,UpdateState =  0 where opjid='" + oprJob.Id + "';");
                    }
                    else if (oprJob.UpdateState == 1)
                    {
                        //调用IRP接口 修改[岗位]
                        xmlModel.ModifyOperationJobNode(oprJob);
                        WriteDataSyncLog("IRP修改岗位成功，岗位代码：" + oprJob.Code + ",岗位名称：" + oprJob.Name);

                        //调用KB接口 修改[岗位]
                        PortalIntegrationConsole.KBOrgUserSyncDataSrv.Post KBJob = new PortalIntegrationConsole.KBOrgUserSyncDataSrv.Post();
                        KBJob.PosiCode = oprJob.Code;
                        KBJob.PosiName = oprJob.Name;
                        KBJob.OrderNo = (int)oprJob.OrderNo;
                        KBJob.OrgCode = oprJob.OperationOrg.Code;
                        ReloadIRPXml.KBService.UpdatePost(KBJob);
                        WriteDataSyncLog("KB修改岗位成功，岗位代码：" + oprJob.Code + ",岗位名称：" + oprJob.Name);

                        //修改项目管理中数据标记
                        listSql.Add("update resoperationjob set UpdateState =  0 where opjid='" + oprJob.Id + "';");

                    }
                }

                if (listSql.Count > 0)
                {
                    ExeSql(listSql);
                    WriteDataSyncLog("PM修改岗位同步标记成功！此次共修改岗位“" + listSql.Count + "”个");
                    listSql.Clear();
                }

                #endregion

                #region 3.同步人员

                oq = new ObjectQuery();

                //获取要同步的用户
                oq.AddCriterion(Expression.Sql(" AddState = 1 or UpdateState =  1 or DelState = 1"));
                oq.AddFetchMode("ConnMethod", NHibernate.FetchMode.Eager);
                IList list_User = model.ObjectQuery(typeof(StandardPerson), oq);

                foreach (StandardPerson person in list_User)
                {
                    if (person.DelState == 1)
                    {
                        //调用IRP接口 删除[用户]
                        xmlModel.DeleteUserNode(person);
                        WriteDataSyncLog("IRP删除用户成功，用户代码：" + person.Code + ",用户名称：" + person.Name);

                        //调用KB接口 删除[用户]
                        ReloadIRPXml.KBService.DeleteUser(person.Code);
                        WriteDataSyncLog("KB删除用户成功，用户代码：" + person.Code + ",用户名称：" + person.Name);

                        //修改项目管理中数据标记
                        listSql.Add("update resperson set AddState = 0,UpdateState =  0,DelState = 0,RelState = 0 where perid='" + person.Id + "';");
                    }
                    else if (person.AddState == 1)
                    {
                        //调用IRP接口 新增[用户]
                        xmlModel.InsertUserNode(person);
                        WriteDataSyncLog("IRP新增用户成功，用户代码：" + person.Code + ",用户名称：" + person.Name);

                        //调用KB接口 新增[用户]
                        PortalIntegrationConsole.KBOrgUserSyncDataSrv.User KBUser = new PortalIntegrationConsole.KBOrgUserSyncDataSrv.User();
                        KBUser.UserCode = person.Code;
                        KBUser.UserName = person.Name;
                        KBUser.Password = person.Password;
                        ReloadIRPXml.KBService.AddUser(KBUser);
                        WriteDataSyncLog("KB新增用户成功，用户代码：" + person.Code + ",用户名称：" + person.Name);

                        //修改项目管理中数据标记
                        listSql.Add("update resperson set AddState = 0,UpdateState =  0 where perid='" + person.Id + "';");
                    }
                    else if (person.UpdateState == 1)
                    {
                        //调用IRP接口 修改[用户]
                        xmlModel.ModifyUserNode(person);
                        WriteDataSyncLog("IRP修改用户成功，用户代码：" + person.Code + ",用户名称：" + person.Name);

                        //调用KB接口 修改[用户]
                        PortalIntegrationConsole.KBOrgUserSyncDataSrv.User KBUser = new PortalIntegrationConsole.KBOrgUserSyncDataSrv.User();
                        KBUser.UserCode = person.Code;
                        KBUser.UserName = person.Name;
                        KBUser.Password = person.Password;
                        ReloadIRPXml.KBService.UpdateUser(KBUser);
                        WriteDataSyncLog("KB修改用户成功，用户代码：" + person.Code + ",用户名称：" + person.Name);

                        //修改项目管理中数据标记
                        listSql.Add("update resperson set UpdateState =  0 where perid='" + person.Id + "';");
                    }
                }

                if (listSql.Count > 0)
                {
                    ExeSql(listSql);
                    WriteDataSyncLog("PM修改用户同步标记成功！此次共修改用户“" + listSql.Count + "”个");
                    listSql.Clear();
                }
                #endregion

                #region 4.同步人员岗位关系
                //通过人员变动标志来查询[人员岗位关系],条件[人员.delState = 0 and 人员.RelState = 1]
                oq = new ObjectQuery();
                oq.AddCriterion(Expression.Sql(" DelState = 0 and RelState = 1"));
                IList list_UpdateJobUser = model.ObjectQuery(typeof(StandardPerson), oq);

                if (list_UpdateJobUser.Count > 0)
                {
                    oq.Criterions.Clear();
                    Disjunction dis = new Disjunction();
                    foreach (StandardPerson person in list_UpdateJobUser)
                    {
                        dis.Add(Expression.Eq("StandardPerson.Id", person.Id));
                    }
                    oq.AddFetchMode("OperationJob", NHibernate.FetchMode.Eager);
                    oq.AddCriterion(dis);
                    List<PersonOnJob> list_UserOnJob = model.ObjectQuery(typeof(PersonOnJob), oq).OfType<PersonOnJob>().ToList();

                    foreach (StandardPerson person in list_UpdateJobUser)
                    {
                        List<OperationJob> listJob = (from job in list_UserOnJob
                                                      where job.StandardPerson.Id == person.Id
                                                      select job.OperationJob).ToList();

                        string jobCodes = "";
                        foreach (OperationJob job in listJob)
                        {
                            jobCodes += job.Code + ",";
                        }
                        if (jobCodes.Length > 0)
                            jobCodes = jobCodes.Substring(0, jobCodes.Length - 1);

                        //调用IRP接口 修改[人员岗位关系]
                        xmlModel.PersonOnJob(person.Code, listJob);
                        WriteDataSyncLog("IRP修改用户上岗成功，用户代码：" + person.Code + ",岗位代码：" + jobCodes);

                        //调用KB接口 修改[岗位角色关系]
                        ReloadIRPXml.KBService.UpdateEmpPost(jobCodes, person.Code);
                        WriteDataSyncLog("KB修改用户上岗成功，用户代码：" + person.Code + ",岗位代码：" + jobCodes);

                        //修改项目管理中数据标记
                        listSql.Add("update resperson set RelState =  0 where perid='" + person.Id + "';");
                    }

                    if (listSql.Count > 0)
                    {
                        ExeSql(listSql);
                        WriteDataSyncLog("PM修改用户上岗同步标记成功！此次共修改上岗用户“" + listSql.Count + "”个");
                        listSql.Clear();
                    }
                }

                #endregion

                #region 5.同步岗位角色关系
                //通过岗位变动标志来查询[岗位角色关系],条件[岗位.delState = 0]
                //通过人员变动标志来查询[人员岗位关系],条件[人员.delState = 0] and [岗位.delState = 0]
                //获取要同步的岗位-修改过岗位角色关系且没被删除的数据，需要加载岗位角色关系及角色数据
                oq = new ObjectQuery();
                oq.AddCriterion(Expression.Sql(" RelState = 1 and DelState = 0"));
                oq.AddFetchMode("JobWithRole", NHibernate.FetchMode.Eager);
                IList list_RelJob = model.ObjectQuery(typeof(OperationJob), oq);

                oq.Criterions.Clear();
                oq.FetchModes.Clear();
                Disjunction disRole = new Disjunction();
                foreach (OperationJob oprJob in list_RelJob)
                {
                    foreach (OperationJobWithRole item in oprJob.JobWithRole)
                    {
                        disRole.Add(Expression.Eq("Id", item.OperationRole.Id));
                    }
                }
                oq.AddCriterion(disRole);
                List<OperationRole> listJobWithRole = model.ObjectQuery(typeof(OperationRole), oq).OfType<OperationRole>().ToList();
                foreach (OperationJob oprJob in list_RelJob)
                {
                    foreach (OperationJobWithRole item in oprJob.JobWithRole)
                    {
                        var queryJobWithRole = from j in listJobWithRole
                                               where j.Id == item.OperationRole.Id
                                               select j;
                        item.OperationRole = queryJobWithRole.ElementAt(0);
                    }
                }

                foreach (OperationJob oprJob in list_RelJob)
                {
                    string roleCodes = "";
                    foreach (OperationJobWithRole item in oprJob.JobWithRole)
                    {
                        roleCodes += item.OperationRole.RoleCode + ",";
                    }
                    if (!string.IsNullOrEmpty(roleCodes))
                        roleCodes = roleCodes.Substring(0, roleCodes.Length - 1);

                    //调用IRP接口 修改[岗位角色关系]
                    xmlModel.JobLinkRole(oprJob);
                    WriteDataSyncLog("IRP修改岗位角色成功，岗位代码：" + oprJob.Code + ",角色代码：" + roleCodes);

                    //调用KB接口 修改[岗位角色关系]
                    ReloadIRPXml.KBService.UpdateEmpRole(roleCodes, oprJob.Code);
                    WriteDataSyncLog("KB修改岗位角色成功，岗位代码：" + oprJob.Code + ",角色代码：" + roleCodes);

                    //修改项目管理中数据标记
                    listSql.Add("update resoperationjob set RelState =  0 where opjid='" + oprJob.Id + "';");
                }

                if (listSql.Count > 0)
                {
                    ExeSql(listSql);
                    WriteDataSyncLog("PM修改岗位角色同步标记成功！此次共修改岗位“" + listSql.Count + "”个");
                    listSql.Clear();
                }
                #endregion

                #region 6 加载XML文件
                ReloadIRPXml.IRPService.SyncXMLData();
                WriteDataSyncLog("IRP同步XML到内存操作成功。");
                ReloadIRPXml.KBService.SyncXMLData();
                WriteDataSyncLog("KB同步XML到内存操作成功。");
                #endregion

                if (exeType == ExePlanType.定点执行一次 || exeType == ExePlanType.即时执行一次)
                {
                    SetEnabledControl(true);
                    lblServerState.Text = "停止";
                }

                WriteDataSyncLog("数据同步服务执行结束，全部操作成功。");
            }
            catch (Exception ex)
            {
                lblServerState.Text = "停止";
                WriteDataSyncLog("数据同步服务执行出现异常，服务已停止，异常信息：" + VirtualMachine.Component.Util.ExceptionUtil.ExceptionMessage(ex));
                if (serverExeNumber < repTryNumber)
                {
                    serverExeNumber += 1;
                    WriteDataSyncLog("正在执行第“" + serverExeNumber + "”次重试......");
                    MBPSyncIRPData();
                }
                else
                {
                    SetEnabledControl(true);
                }
            }
            finally
            {

            }
        }

        private void ExeSql(List<string> listSql)
        {
            int exeRecord = 20;//1次执行的原子sql数量
            int exethreadSleepMS = 100;//执行sql线程休眠毫秒数
            string sql = "Begin ";
            for (var i = 0; i < listSql.Count; i++)
            {
                sql += listSql[i];
                if ((i != 0 && (i + 1) % exeRecord == 0) || i == listSql.Count - 1)
                {
                    System.Threading.Thread.Sleep(exethreadSleepMS);
                    sql += " End;";
                    model.SaveSQL(sql);
                    sql = "Begin ";
                }
            }
        }

        private void WriteDataSyncLog(string message)
        {
            try
            {
                message = DateTime.Now.ToString() + "：" + message;
                //1.写日志文件
                System.IO.StreamWriter write = new System.IO.StreamWriter(DataSyncLogFilePath, true, Encoding.Default);
                write.WriteLine(message);
                write.Close();
                write.Dispose();
                //2.写界面消息
                WriteUIMessage(message);
            }
            catch
            {
            }
        }
        private void WriteUIMessage(string message)
        {
            if (gridLog.Rows.Count >= 300)
            {
                //移除第一行
                gridLog.Rows.RemoveAt(0);
            }

            //新增行
            int rowIndex = gridLog.Rows.Add();
            DataGridViewRow row = gridLog.Rows[rowIndex];
            row.Cells[colContent.Name].Value = message;
        }
        #endregion

    }

    public enum ExePlanType
    {
        定点执行一次 = 1,
        定时反复执行 = 2,
        即时执行一次 = 3
    }
}
