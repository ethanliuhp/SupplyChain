using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Linq;

using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;

using Application.Business.Erp.ResourceManager.Client.Main;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonForm;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using System.Windows.Documents;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng;

using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using ImportIntegration;
using System.IO;
using System.Diagnostics;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Business.Erp.SupplyChain.PMCAndWarning.Domain;
using System.Data.OleDb;


namespace Application.Business.Erp.SupplyChain.Client.PMCAndWarning
{
    public partial class VWaringServerControl : TBasicDataView
    {
        public MPMCAndWarning model = new MPMCAndWarning();

        public VWaringServerControl()
        {
            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();

            SetView();
        }

        private void SetView()
        {
            lblState.Text = WarningServerControl.State.ToString();
            lblStartTime.Text = WarningServerControl.StartTime != null ? WarningServerControl.StartTime.Value.ToString() : "";
        }

        private void InitEvents()
        {
            btnStart.Click += new EventHandler(btnStart_Click);
            btnStop.Click += new EventHandler(btnStop_Click);
        }

        void btnStop_Click(object sender, EventArgs e)
        {
            if (WarningServerControl.State != WarningServerStartStateEnum.已关闭)
            {
                //model.StopWarningServer();
                StopWarningServer();

                WarningServerControl.State = WarningServerStartStateEnum.已关闭;

                SetView();
            }
        }

        void btnStart_Click(object sender, EventArgs e)
        {
            if (WarningServerControl.State != WarningServerStartStateEnum.已启动)
            {
                //model.StartWarningServer();
                StartWarningServer();

                WarningServerControl.State = WarningServerStartStateEnum.已启动;
                WarningServerControl.StartTime = DateTime.Now;

                SetView();
            }
        }


        public override void RefreshControls(MainViewState state)
        {
            switch (state)
            {
                case MainViewState.AddNew:


                    break;

                case MainViewState.Modify:


                    break;

                case MainViewState.Browser:


                    break;

                case MainViewState.Initialize://添加根节点


                    break;
            }

            ViewState = state;
        }

        public override bool ModifyView()
        {

            return false;
        }

        public override bool CancelView()
        {
            try
            {

                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        public override void RefreshView()
        {
            try
            {

            }
            catch (Exception e)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(e));
            }
        }

        public override bool SaveView()
        {
            bool isNew = false;
            try
            {
                //if (!ValideSave())
                //    return false;



                RefreshControls(MainViewState.Browser);

                return true;
            }
            catch (Exception exp)
            {
                if (exp.InnerException != null && exp.InnerException.Message.Contains("违反唯一约束条件"))
                    MessageBox.Show("编码必须唯一！");
                else
                    MessageBox.Show("保存失败，异常信息：" + ExceptionUtil.ExceptionMessage(exp));
            }
            return false;
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

                write.WriteLine("预警定时轮询服务关闭完成，完成时间：" + DateTime.Now.ToString());

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
                write.WriteLine("预警服务开始启动.......，启动时间：" + DateTime.Now.ToString());

                ObjectQuery oq = new ObjectQuery();
                //oq.AddCriterion(Expression.Eq("ProjectInfoState", EnumProjectInfoState.发布));
                IList listProject = model.ObjectQuery(typeof(CurrentProjectInfo), oq);
                List<string> listProjectIds = new List<string>();
                if (listProject != null && listProject.Count > 0)
                {
                    foreach (CurrentProjectInfo p in listProject)
                    {
                        listProjectIds.Add(p.Id);
                    }
                }

                //启动定时执行服务（间隔固定时长执行），精确到秒
                oq.Criterions.Clear();
                oq.AddCriterion(Expression.Eq("TriggerMode", StateCheckTriggerMode.定时));
                oq.AddCriterion(Expression.Gt("TriggerTerm1", (decimal)0));

                IList listCheckAction = model.ObjectQuery(typeof(StateCheckAction), oq);
                if (listCheckAction.Count > 0 && listProjectIds.Count > 0)
                {
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

                //启动定点执行服务（固定时间点执行），精确到秒
                oq.Criterions.Clear();
                oq.AddCriterion(Expression.Eq("TriggerMode", StateCheckTriggerMode.定点));

                IEnumerable<StateCheckAction> listCheckActionPoint = model.ObjectQuery(typeof(StateCheckAction), oq).OfType<StateCheckAction>();
                listCheckActionPoint = from c in listCheckActionPoint
                                       where c.TriggerTerm2 != null && c.TriggerTerm2.Trim() != ""
                                       select c;

                if (listCheckActionPoint.Count() > 0)
                {
                    List<StateCheckAction> listValidCheckAction = new List<StateCheckAction>();

                    foreach (StateCheckAction item in listCheckActionPoint)
                    {
                        try
                        {
                            //校验时间有效

                            DateTime.Parse("1900-1-1 " + item.TriggerTerm2.Trim());

                            listValidCheckAction.Add(item);
                        }
                        catch { }
                    }


                    if (listValidCheckAction.Count > 0 && listProjectIds.Count > 0)
                    {
                        foreach (StateCheckAction checkAction in listValidCheckAction)
                        {
                            System.Windows.Forms.Timer timerPoint = new System.Windows.Forms.Timer();

                            timerPoint.Enabled = true;
                            timerPoint.Interval = 1000;//timer.Interval单位毫秒  每1秒钟执行一次

                            Dictionary<StateCheckAction, List<string>> dicCheckActionProject = new Dictionary<StateCheckAction, List<string>>();
                            dicCheckActionProject.Add(checkAction, listProjectIds);

                            timerPoint.Tag = dicCheckActionProject;

                            timerPoint.Tick += new EventHandler(timerPoint_Tick);

                            WarningServerControl.Timers.Add(timerPoint);

                            timerPoint.Start();
                        }
                    }
                }


                write.WriteLine("预警服务启动完毕，完成时间：" + DateTime.Now.ToString());

                return true;
            }
            catch (Exception ex)
            {
                if (write == null)
                    write = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"WarningLog.txt", true, Encoding.Default);

                string msg = ExceptionUtil.ExceptionMessage(ex);
                write.WriteLine(DateTime.Now.ToString() + "预警服务启动失败,异常信息：" + msg);

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

        //定时预警
        void timer_Tick(object sender, EventArgs e)
        {

            StreamWriter write = null;
            StateCheckAction exeCheckAction = null;
            Timer timer = sender as Timer;

            try
            {

                if (timer == null)
                    return;
                if (timer.Tag == null)
                    return;

                Dictionary<StateCheckAction, List<string>> dicCheckActionProject = timer.Tag as Dictionary<StateCheckAction, List<string>>;
                if (dicCheckActionProject == null || dicCheckActionProject.Count == 0)
                    return;

                timer.Stop();

                exeCheckAction = dicCheckActionProject.ElementAt(0).Key;
                List<string> listProjectIds = dicCheckActionProject.ElementAt(0).Value;

                if (exeCheckAction.ActionName == WarningTarget.WarningTarget_GQ_StateCheck)//工期状态检查
                {
                    model.PMCAndWarningSrv.CheckDurationState(listProjectIds, false, true, null);
                }
                else if (exeCheckAction.ActionName == WarningTarget.WarningTarget_ZL_StateCheck)//资料状态检查
                {
                    model.PMCAndWarningSrv.CheckInformationState(listProjectIds, false, true, null);
                }
                if (exeCheckAction.ActionName == WarningTarget.WarningTarget_WZ_SupplyOrder)//物资采购合同
                {
                    model.PMCAndWarningSrv.CheckDurationSupplyOrder(listProjectIds, false, true, null);
                }
                if (exeCheckAction.ActionName == WarningTarget.WarningTarget_WZ_StockIn)//物资收料管理
                {
                    model.PMCAndWarningSrv.CheckDurationStockIn(listProjectIds, false, true, null);
                }
                if (exeCheckAction.ActionName == WarningTarget.WarningTarget_WZ_DailyPlan)//物资日常需求计划
                {
                    model.PMCAndWarningSrv.CheckDurationDailyPlan(listProjectIds, false, true, null);
                }
                if (exeCheckAction.ActionName == WarningTarget.WarningTarget_WZ_MonthPlan)//物资月度需求计划
                {
                    model.PMCAndWarningSrv.CheckDurationMonthPlan(listProjectIds, false, true, null);
                }
                if (exeCheckAction.ActionName == WarningTarget.WarningTarget_SW_RectificationNoticeMaster)//整改单
                {
                    model.PMCAndWarningSrv.CheckRectificatNoticeMaster(listProjectIds, false, true, null);
                }
                if (exeCheckAction.ActionName == WarningTarget.WarningTarget_SW_ProfessionInspectionRecordMaster)//专业检查
                {
                    model.PMCAndWarningSrv.CheckProfessionInspectionRecordMaster(listProjectIds, false, true, null);
                }

                if (exeCheckAction.ActionName == WarningTarget.WarningTarget_SW_WorkOrderBusinessReview)//工单商务复核
                {
                    model.PMCAndWarningSrv.WorkOrderBusinessReview(listProjectIds, false, true, null);
                }
                if (exeCheckAction.ActionName == WarningTarget.WarningTarget_SW_RentalCostClear)//设备费用租赁结算
                {
                    model.PMCAndWarningSrv.RentalCostClear(listProjectIds, false, true, null);
                }
                if (exeCheckAction.ActionName == WarningTarget.WarningTarget_SW_CostClear)//费用结算
                {
                    model.PMCAndWarningSrv.CostClear(listProjectIds, false, true, null);
                }
                if (exeCheckAction.ActionName == WarningTarget.WarningTarget_SW_Costing)//成本核算
                {
                    model.PMCAndWarningSrv.Costing(listProjectIds, false, true, null);
                }

                timer.Start();

            }
            catch (Exception ex)
            {
                if (timer != null)
                    timer.Start();

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

        //定点预警
        void timerPoint_Tick(object sender, EventArgs e)
        {
            StreamWriter write = null;
            StateCheckAction exeCheckAction = null;
            Timer timer = sender as Timer;

            try
            {

                if (timer == null)
                    return;
                if (timer.Tag == null)
                    return;

                Dictionary<StateCheckAction, List<string>> dicCheckActionProject = timer.Tag as Dictionary<StateCheckAction, List<string>>;
                if (dicCheckActionProject == null || dicCheckActionProject.Count == 0)
                    return;

                exeCheckAction = dicCheckActionProject.ElementAt(0).Key;

                DateTime exeTime = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + exeCheckAction.TriggerTerm2.Trim());
                if (DateTime.Now.ToString() == exeTime.ToString() || DateTime.Now.ToString() == exeTime.AddSeconds(1).ToString())
                {
                    timer.Stop();

                    List<string> listProjectIds = dicCheckActionProject.ElementAt(0).Value;

                    if (exeCheckAction.ActionName == WarningTarget.WarningTarget_GQ_StateCheck)//工期状态检查
                    {
                        model.PMCAndWarningSrv.CheckDurationState(listProjectIds, false, true, null);
                    }
                    else if (exeCheckAction.ActionName == WarningTarget.WarningTarget_ZL_StateCheck)//资料信息检查
                    {
                        model.PMCAndWarningSrv.CheckInformationState(listProjectIds, false, true, null);
                    }
                    else if (exeCheckAction.ActionName == WarningTarget.WarningTarget_SW_ComprehensiveCheck)//综合指标检查
                    {
                        model.PMCAndWarningSrv.CheckTaskProceedsState(listProjectIds, false, true, null);
                        model.PMCAndWarningSrv.CheckOwnerQuantityTarget(listProjectIds, false, true, null);
                    }

                    if (exeCheckAction.ActionName == WarningTarget.WarningTarget_WZ_SupplyOrder)//物资采购合同
                    {
                        model.PMCAndWarningSrv.CheckDurationSupplyOrder(listProjectIds, false, true, null);
                    }
                    if (exeCheckAction.ActionName == WarningTarget.WarningTarget_WZ_StockIn)//物资收料管理
                    {
                        model.PMCAndWarningSrv.CheckDurationStockIn(listProjectIds, false, true, null);
                    }
                    if (exeCheckAction.ActionName == WarningTarget.WarningTarget_WZ_DailyPlan)//物资日常需求计划
                    {
                        model.PMCAndWarningSrv.CheckDurationDailyPlan(listProjectIds, false, true, null);
                    }
                    if (exeCheckAction.ActionName == WarningTarget.WarningTarget_WZ_MonthPlan)//物资月度需求计划
                    {
                        model.PMCAndWarningSrv.CheckDurationMonthPlan(listProjectIds, false, true, null);
                    }
                    if (exeCheckAction.ActionName == WarningTarget.WarningTarget_SW_RectificationNoticeMaster)//整改单
                    {
                        model.PMCAndWarningSrv.CheckRectificatNoticeMaster(listProjectIds, false, true, null);
                    }
                    if (exeCheckAction.ActionName == WarningTarget.WarningTarget_SW_ProfessionInspectionRecordMaster)//专业检查
                    {
                        model.PMCAndWarningSrv.CheckProfessionInspectionRecordMaster(listProjectIds, false, true, null);
                    }

                    if (exeCheckAction.ActionName == WarningTarget.WarningTarget_SW_WorkOrderBusinessReview)//工单商务复核
                    {
                        model.PMCAndWarningSrv.WorkOrderBusinessReview(listProjectIds, false, true, null);
                    }
                    if (exeCheckAction.ActionName == WarningTarget.WarningTarget_SW_RentalCostClear)//设备费用租赁结算
                    {
                        model.PMCAndWarningSrv.RentalCostClear(listProjectIds, false, true, null);
                    }
                    if (exeCheckAction.ActionName == WarningTarget.WarningTarget_SW_CostClear)//费用结算
                    {
                        model.PMCAndWarningSrv.CostClear(listProjectIds, false, true, null);
                    }
                    if (exeCheckAction.ActionName == WarningTarget.WarningTarget_SW_Costing)//成本核算
                    {
                        model.PMCAndWarningSrv.Costing(listProjectIds, false, true, null);
                    }

                    System.Threading.Thread.Sleep(2000);//休眠两秒

                    timer.Start();

                }

            }
            catch (Exception ex)
            {

                if (timer1 != null)
                    timer.Start();

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
    }
}
