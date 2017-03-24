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
using NHibernate;
using System.Runtime.Remoting.Messaging;
using VirtualMachine.Core.DataAccess;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Service;
using System.Configuration;
using System.Diagnostics;
using Application.Business.Erp.SupplyChain.ProductionManagement.Service;

namespace PortalIntegrationConsole
{
    public partial class AutoBusinessService : Form
    {
        IGWBSTreeSrv model = null;
        IIndirectCostSvr indirectModel = null;
        IProductionManagementSrv proMSrv = null;
        int serverExeNumber = 0;
        int repTryNumber = 0;
        AutoExePlanType exeType = AutoExePlanType.定点执行一次;

        string DataSyncLogPath = AppDomain.CurrentDomain.BaseDirectory + @"Log";
        string DataSyncLogFilePath = AppDomain.CurrentDomain.BaseDirectory + @"Log\\IRP_KB_PMDataSyncLog.txt";

        public AutoBusinessService()
        {
            InitializeComponent();
            InitForm();
        }
        // 是否发送排名提示消息
        private bool isSendMsg;

        #region 界面控制
        private void InitForm()
        {
            if (model == null)
                model = StaticMethod.GetService("SupplyChain", "GWBSTreeSrv") as IGWBSTreeSrv;

            if (indirectModel == null)
                indirectModel = StaticMethod.GetService("SupplyChain", "IndirectCostSvr") as IIndirectCostSvr;

            if (proMSrv == null)
                proMSrv = StaticMethod.GetService("SupplyChain", "ProductionManagementSrv") as IProductionManagementSrv;

            for (int i = 1; i < 10; i++)
            {
                txtRepExeTryNumber.Items.Add(i);
            }
            txtRepExeTryNumber.SelectedIndex = 0;

            if (Directory.Exists(DataSyncLogPath) == false)
                Directory.CreateDirectory(DataSyncLogPath);

            isSendMsg = Convert.ToBoolean(ConfigurationManager.AppSettings["RankingMsgSend"]);
        }

        private void btnStartPlan_Click(object sender, EventArgs e)
        {
            try
            {
                SetEnabledControl(false);
                lblServerState.Text = "执行中";
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
                exeType = AutoExePlanType.定时反复执行;
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
                MBPAutoServiceData();
                //SetEnabledControl(false);
                //lblServerState.Text = "执行中";
                //btnStartServer.Text = "执行中...";
                //exeType = AutoExePlanType.即时执行一次;
                //timerDataSyncPlan.Start();
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
            if (exeType == AutoExePlanType.即时执行一次)
            {
                try
                {
                    timerDataSyncPlan.Stop();
                    MBPAutoServiceData();
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
            else
            {
                DateTime exeTime = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + dateTimePickerRepExeTime.Value.TimeOfDay.ToString().Trim());
                if (exeType == AutoExePlanType.定时反复执行 && (DateTime.Now.ToString() == exeTime.ToString() || DateTime.Now.ToString() == exeTime.AddSeconds(1).ToString()))
                {
                    try
                    {
                        MBPAutoServiceData();
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
            btnStartServer.Enabled = isEnabled;
            btnStartRepExePlan.Enabled = isEnabled;
            txtRepExeTryNumber.Enabled = isEnabled;
            dateTimePickerRepExeTime.Enabled = isEnabled;
        }
        #endregion

        #region 业务定时服务

        private void MBPAutoServiceData()
        {
            try
            {
                //调用资金管理服务
                DateTime currDate = TransUtil.ToDateTime(DateTime.Now.AddDays(-1).ToShortDateString());//当日
                indirectModel.CompanyKeyInfoService(currDate);

                // 调用信息系统项目使用情况排名统计服务
                ProjectRanking pr = new ProjectRanking();
                pr.IsSend = isSendMsg;
                pr.CreateData();
                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                {
                    // 每周一发送一次提示
                    var result = pr.SendMsg(60);
                }

                #region 工期预警
                proMSrv.CreateProjectDelayDays("");
                #endregion

                #region 项目状态值计算
                indirectModel.CalulationProjectState();
                #endregion
               

                if (exeType == AutoExePlanType.定点执行一次 || exeType == AutoExePlanType.即时执行一次)
                {
                    SetEnabledControl(true);
                    lblServerState.Text = "停止";
                }

                WriteDataSyncLog("数据定时服务执行结束，全部操作成功。");
            }
            catch (Exception ex)
            {
                lblServerState.Text = "停止";
                WriteDataSyncLog("数据定时服务执行出现异常，服务已停止，异常信息：" + VirtualMachine.Component.Util.ExceptionUtil.ExceptionMessage(ex));
                if (serverExeNumber < repTryNumber)
                {
                    serverExeNumber += 1;
                    WriteDataSyncLog("正在执行第“" + serverExeNumber + "”次重试......");
                    MBPAutoServiceData();
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

        /// <summary>
        /// 给得分小于60分的项目经理发送消息提醒
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            //var ret = MessageBox.Show("该操作将给系统排名中配置的相关人员发送广讯通消息，是否确认？", "提示", MessageBoxButtons.OKCancel);
            //if (ret == DialogResult.Cancel) return;
            //ProjectRanking pr = new ProjectRanking();
            //pr.IsSend = isSendMsg;
            //var result = pr.SendMsg(60);
            //MessageBox.Show(result);

            //ProjectRanking pr = new ProjectRanking();
            //pr.CreateData();
            //MessageBox.Show("生成成功");


            //DateTime currDate = TransUtil.ToDateTime(DateTime.Now.AddDays(-1).ToShortDateString());//当日
            //indirectModel.CompanyKeyInfoService(currDate);

            #region 商务指标

            //DateTime currDate = TransUtil.ToDateTime(DateTime.Now.AddDays(-1).ToShortDateString());//当日
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //DateTime currDate = new DateTime(2015, 7, 2);
            //indirectModel.CompanyKeyInfoService(currDate);
            //sw.Stop();

            //MessageBox.Show("生成成功，耗时：" + (sw.ElapsedMilliseconds / 1000) + "秒");
            #endregion
        }

    }

    public enum AutoExePlanType
    {
        定点执行一次 = 1,
        定时反复执行 = 2,
        即时执行一次 = 3
    }
}
