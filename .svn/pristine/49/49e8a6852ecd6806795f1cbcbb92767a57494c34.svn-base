using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Util;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using FlexCell;
using NHibernate.Criterion;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Core;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI
{
    public partial class VDurationWarning : TBasicDataView
    {
        private MFinanceMultData finOperate = new MFinanceMultData();
        private CurrentProjectInfo projectInfo;
        private string selectProject;
        private List<DurationDelayWarn> delayDetails;

        private List<DurationDelayWarn> delayProject;

        public VDurationWarning()
        {
            InitializeComponent();

            InitEvents();

            InitData();

            InitTotalReport();
        }

        private void InitEvents()
        {
            btnOperationOrg.Click += new EventHandler(btnOperationOrg_Click);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnSave.Visible = false;

            flexGridTotal.Click += new Grid.ClickEventHandler(flexGrid_Click);
        }

        private void InitData()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
            {
                txtOperationOrg.Text = projectInfo.Name;
                txtOperationOrg.Tag = projectInfo;
                btnOperationOrg.Visible = false;
            }
            else
            {
                projectInfo = null;
                btnOperationOrg.Visible = true;
            }
        }

        private void InitTotalReport()
        {
            flexGridTotal.AutoRedraw = false;
            flexGridTotal.Column(0).Visible = true;

            flexGridTotal.SelectionMode = SelectionModeEnum.Free;
            flexGridTotal.ExtendLastCol = true;
            flexGridTotal.DisplayFocusRect = true;
            flexGridTotal.LockButton = true;
            flexGridTotal.ReadonlyFocusRect = FocusRectEnum.Solid;
            flexGridTotal.BorderStyle = BorderStyleEnum.FixedSingle;
            flexGridTotal.ScrollBars = ScrollBarsEnum.Both;
            flexGridTotal.BackColorBkg = SystemColors.Control;
            flexGridTotal.DefaultFont = new Font("Tahoma", 10);
            flexGridTotal.DisplayRowArrow = true;
            flexGridTotal.DisplayRowNumber = true;

            var cols = new List<string>() { "项目名称", "绿色预警", "蓝色预警", "黄色预警", "红色预警", "预计项目延期天数", "预计成本增加", "" };
            flexGridTotal.Rows = 2;
            flexGridTotal.Cols = cols.Count + 1;
            for (int i = 0; i < cols.Count; i++)
            {
                var cell = flexGridTotal.Cell(1, i + 1);
                cell.Text = cols[i];
                cell.Locked = true;
                cell.FontSize = 13;
                cell.FontBold = true;
                cell.BackColor = SystemColors.Control;
                if (i > 0)
                {
                    cell.Alignment = AlignmentEnum.CenterCenter;
                    flexGridTotal.Column(i + 1).Width = Convert.ToInt16(cell.Text.Length*20);
                }
                else
                {
                    flexGridTotal.Column(i + 1).Width = 400;
                }
            }

            flexGridTotal.Range(0, 1, 0, cols.Count).Merge();
            var titlCell = flexGridTotal.Cell(0, 1);
            titlCell.Text = string.Format("{0} {1} 工期预警",
                                                     projectInfo == null ? string.Empty : projectInfo.Name,
                                                     DateTime.Now.ToString("yyyy-MM-dd"));
            titlCell.FontSize = 13;
            titlCell.FontBold = true;
            
            flexGridTotal.AutoRedraw = true;
            flexGridTotal.Refresh();
        }

        private void InitDetailReport()
        {
            flexGridDetail.AutoRedraw = false;
            flexGridDetail.Column(0).Visible = true;

            flexGridDetail.SelectionMode = SelectionModeEnum.Free;
            flexGridDetail.ExtendLastCol = true;
            flexGridDetail.DisplayFocusRect = true;
            flexGridDetail.LockButton = true;
            flexGridDetail.ReadonlyFocusRect = FocusRectEnum.Solid;
            flexGridDetail.BorderStyle = BorderStyleEnum.FixedSingle;
            flexGridDetail.ScrollBars = ScrollBarsEnum.Both;
            flexGridDetail.BackColorBkg = SystemColors.Control;
            flexGridDetail.DefaultFont = new Font("Tahoma", 10);
            flexGridDetail.DisplayRowArrow = true;
            flexGridDetail.DisplayRowNumber = true;

            //var cols = new List<string>() { "任务名称", "计划开始日期", "计划结束日期", "计划工期", "计划进度", "实际开始日期", "累计进度", "延误天数", "成本增加", "预警等级", "延误导致成本增加", "任务全路径" };
            var cols = new List<string>() { "任务名称",  "延误天数", "成本增加", "预警等级" };
            flexGridDetail.Rows = 2;
            flexGridDetail.Cols = cols.Count + 1;
            for (int i = 0; i < cols.Count; i++)
            {
                var cell = flexGridDetail.Cell(1, i + 1);
                cell.Text = cols[i];
                cell.Locked = true;
                cell.FontSize = 13;
                cell.FontBold = true;
                cell.BackColor = SystemColors.Control;
                if (i == 0)
                {
                    flexGridDetail.Column(i + 1).Width = 200;
                }
                else if(i == cols.Count - 1)
                {
                    flexGridDetail.Column(i + 1).Width = 400;
                }
                else
                {
                    cell.Alignment = AlignmentEnum.CenterCenter;
                    flexGridDetail.Column(i + 1).Width = Convert.ToInt16(20 * cell.Text.Length);
                }
            }

            flexGridDetail.Range(0, 1, 0, cols.Count).Merge();
            var titlCell = flexGridDetail.Cell(0, 1);
            titlCell.Text = string.Format("{0} {1} 工期预警",
                                                     projectInfo == null ? string.Empty : projectInfo.Name,
                                                     DateTime.Now.ToString("yyyy-MM-dd"));
            titlCell.FontSize = 13;
            titlCell.FontBold = true;

            flexGridDetail.AutoRedraw = true;
            flexGridTotal.Refresh();
        }

        private IList GetDelayDetails(bool isPro)
        {
            ObjectQuery objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("CreateDate", dtpDate.Value.Date));
            objQuery.AddCriterion(Expression.Eq("IsProjectDelay", isPro));
            if (projectInfo != null)
            {
                objQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            }
            else
            {
                var org = txtOperationOrg.Tag as OperationOrgInfo;
                if (org != null)
                {
                    objQuery.AddCriterion(Expression.Like("OrgSyscode", org.SysCode, MatchMode.Start));
                }
            }

            return finOperate.FinanceMultDataSrv.Query(typeof(DurationDelayWarn), objQuery);
        }

        private void ShowTotalReport()
        {
            InitTotalReport();

            delayDetails = GetDelayDetails(false).OfType<DurationDelayWarn>().ToList();
            if (delayDetails == null || delayDetails.Count == 0)
            {
                return;
            }

            delayProject = GetDelayDetails(true).OfType<DurationDelayWarn>().ToList();
            if (delayProject == null || delayProject.Count == 0)
            {
                return;
            }

            #region 计算预警等级

            var total = from q in delayDetails
                        group q by new {q.ProjectId, q.ProjectName}
                        into gps
                        select new
                                   {
                                       gps.Key.ProjectId,
                                       gps.Key.ProjectName,
                                       GreenWarn = gps.Count(q => q.WarnLevel == 0),
                                       BlueWarn = gps.Count(q => q.WarnLevel == 1),
                                       YellowWarn = gps.Count(q => q.WarnLevel == 2),
                                       RedWarn = gps.Count(q => q.WarnLevel == 3),
                                       //DelayDays = gps.Sum(q => q.ProjectDelayDays),
                                       //DelayCosts = gps.Sum(q => q.DelayCosts)
                                       DelayDays = GetProjectDelayDays(delayProject, gps.Key.ProjectId),
                                       DelayCosts = GetProjectDelayCosts(delayProject, gps.Key.ProjectId)
                                   };
            var records =
                total.OrderByDescending(a => a.RedWarn).ThenByDescending(b => b.YellowWarn)
                    .ThenByDescending(c => c.BlueWarn).ThenByDescending(d => d.GreenWarn);
            #endregion

            #region 显示
            flexGridTotal.Rows = records.Count() + 2;
            flexGridTotal.FrozenRows = 1;
            var startRowIndex = 2;
            foreach (var record in records)
            {
                for (int j = 1; j < flexGridTotal.Cols; j++)
                {
                    var cell = flexGridTotal.Cell(startRowIndex, j);
                    cell.Locked = true;
                    cell.Alignment = AlignmentEnum.CenterCenter;
                    cell.FontBold = true;
                    cell.ForeColor = Color.Black;
                    if (j == 1)
                    {
                        cell.FontUnderline = true;
                        cell.Text = record.ProjectName;
                        cell.Tag = record.ProjectId;
                        cell.Alignment = AlignmentEnum.LeftCenter;
                        cell.FontBold = false;
                    }
                    else if (j == 2)
                    {
                        cell.Text = record.GreenWarn.ToString();
                        cell.ForeColor = Color.Green;
                    }
                    else if (j == 3)
                    {
                        cell.Text = record.BlueWarn.ToString();
                        cell.ForeColor = Color.Blue;
                    }
                    else if (j == 4)
                    {
                        cell.Text = record.YellowWarn.ToString();
                        cell.ForeColor = Color.DarkOrange;
                    }
                    else if (j == 5)
                    {
                        cell.Text = record.RedWarn.ToString();
                        cell.ForeColor = Color.Red;
                    }
                    else if (j == 6)
                    {
                        cell.Text = record.DelayDays.ToString("N0");
                        cell.Alignment = AlignmentEnum.RightCenter;
                    }
                    else if (j == 7)
                    {
                        cell.Text = record.DelayCosts.ToString("N2");
                        cell.Alignment = AlignmentEnum.RightCenter;
                        flexGridTotal.Column(cell.Col).AutoFit();
                    }
                    else
                    {
                        cell.Text = string.Empty;
                    }
                }
                startRowIndex++;
            }
            #endregion
        }

        private decimal GetProjectDelayCosts(List<DurationDelayWarn> delayProject, string projectid)
        {
            DurationDelayWarn ddw = delayProject.Find(p => p.ProjectId == projectid);
            if (ddw != null)
                return ddw.DelayCosts;
            else
                return (decimal)0;
        }

        private int GetProjectDelayDays(List<DurationDelayWarn> delayProject, string projectid)
        {
            DurationDelayWarn ddw = delayProject.Find(p => p.ProjectId == projectid);
            if (ddw != null)
                return ClientUtil.ToInt(ddw.ProjectDelayDays);
            else
                return 0;
        }

        private void ShowDetailReport()
        {
            InitDetailReport();
            //delayDetails = GetDelayDetails(false).OfType<DurationDelayWarn>().ToList();
            if (delayDetails == null || delayDetails.Count == 0)
            {
                return;
            }

            if (delayDetails == null || delayDetails.Count == 0 || string.IsNullOrEmpty(selectProject))
            {
                return;
            }

            var list = delayDetails.FindAll(a => a.ProjectId == selectProject).OrderByDescending(a => a.WarnLevel);
            if (list.Count() == 0)
            {
                return;
            }

            var titlCell = flexGridDetail.Cell(0, 1);
            titlCell.Text = string.Format("{0} {1} 工期预警", list.First().ProjectName, DateTime.Now.ToString("yyyy-MM-dd"));
            flexGridDetail.Rows = list.Count() + 2;
            flexGridDetail.FrozenRows = 1;
            var startRowIndex = 2;
            foreach (var record in list)
            {
                for (int j = 1; j < flexGridDetail.Cols; j++)
                {
                    var cell = flexGridDetail.Cell(startRowIndex, j);
                    cell.Locked = true;
                    cell.Alignment = AlignmentEnum.CenterCenter;
                    cell.FontBold = false;
                    cell.ForeColor = Color.Black;
                    cell.FontUnderline = false;
                    if (j == 1)
                    {
                        cell.Text = record.TaskName;
                        //cell.Tag = record.Task.Id;
                        cell.Alignment = AlignmentEnum.LeftCenter;
                        flexGridDetail.Column(cell.Col).AutoFit();
                    }
                    //else if (j == 2)
                    //{
                    //    cell.Text = record.PlanBeginDate.ToString("yyyy-MM-dd");
                    //}
                    //else if (j == 3)
                    //{
                    //    cell.Text = record.PlanEndDate.ToString("yyyy-MM-dd");
                    //}
                    //else if (j == 4)
                    //{
                    //    cell.Text = record.PlanTime.ToString();
                    //}
                    //else if (j == 5)
                    //{
                    //    cell.Text = record.PlanRate.ToString("P2");
                    //}
                    //else if (j == 6)
                    //{
                    //    cell.Text = record.RealBeginDate.ToString("yyyy-MM-dd");
                    //}
                    //else if (j == 7)
                    //{
                    //    cell.Text = (record.RealRate/100).ToString("P2");
                    //}
                    else if (j == 2)
                    {
                        cell.Text = record.DelayDays.ToString("N0");
                    }
                    else if (j == 3)
                    {
                        cell.Text = record.DelayCosts.ToString("N2");
                        cell.Alignment = AlignmentEnum.RightCenter;
                        flexGridDetail.Column(cell.Col).AutoFit();
                    }
                    else if (j == 4)
                    {
                        cell.FontBold = true;
                        switch (record.WarnLevel)
                        {
                            case 0:
                                cell.Text = "绿色";
                                cell.ForeColor = Color.Green;
                                break;
                            case 1:
                                cell.Text = "蓝色";
                                cell.ForeColor = Color.Blue;
                                break;
                            case 2:
                                cell.Text = "黄色";
                                cell.ForeColor = Color.DarkOrange;
                                break;
                            case 3:
                                cell.Text = "红色";
                                cell.ForeColor = Color.Red;
                                break;
                        }
                    }
                    //else if (j == 11)//延误导致成本增加
                    //{
                    //    cell.Alignment = AlignmentEnum.LeftCenter;
                    //    cell.Text = record.CostDetail;
                    //    flexGridDetail.Column(cell.Col).AutoFit();
                    //}
                    //else if (j == 12)
                    //{
                    //    cell.Text = record.TaskFullPath;
                    //    cell.Alignment = AlignmentEnum.LeftCenter;
                    //    flexGridDetail.Column(cell.Col).AutoFit();
                    //}
                }
                startRowIndex++;
            }
        }

        private void ShowDelayChart()
        {
            if (string.IsNullOrEmpty(selectProject))
            {
                return;
            }

            var splitDay = 6;
            var endDate = DateTime.Now.Date;
            var bgDate = DateTime.Now.Date.AddDays(-1*splitDay);

            var list =
                new MProductionMng().ProductionManagementSrv.GetProjectTotalDelayDays(selectProject, bgDate, endDate)
                    .OfType<DurationDelayWarn>().ToList();
            if (list == null || list.Count == 0)
            {
                return;
            }

            gdChart.Rows = 26;
            gdChart.Cols = 20;
            gdChart.BorderStyle = BorderStyleEnum.FixedSingle;
            gdChart.BackColorBkg = SystemColors.Control;
            gdChart.DefaultFont = new Font("Tahoma", 9);
            gdChart.DisplayRowArrow = true;
            gdChart.DisplayRowNumber = true;

            #region 延期趋势图
            gdChart.Range(1, 1, 2, gdChart.Cols - 1).Merge();
            gdChart.Cell(1, 1).Text = string.Format("{0} 延期趋势图", list.First().ProjectName);
            gdChart.Cell(1, 1).Alignment = AlignmentEnum.CenterCenter;
            gdChart.Cell(1, 1).FontSize = 18;
            gdChart.Cell(1, 1).FontBold = true;

            var cols = new string[] { "日期", "延期天数", "成本增加(万元)" };
            var startRowIndex = 3;
            for (var i = 0; i < cols.Length; i++)
            {
                gdChart.Cell(startRowIndex, i + 1).Text = cols[i];
                gdChart.Cell(startRowIndex, i + 1).FontBold = true;
            }
            
            foreach(var item in list)
            {
                startRowIndex++;
                gdChart.Cell(startRowIndex, 1).Text = item.CreateDate.ToString("yyyy-MM-dd");

                gdChart.Cell(startRowIndex, 2).Text = (item.ProjectDelayDays).ToString("N0");
                gdChart.Cell(startRowIndex, 2).Alignment = AlignmentEnum.RightCenter;

                gdChart.Cell(startRowIndex, 3).Text = (item.DelayCosts / 10000).ToString("N4");
                gdChart.Cell(startRowIndex, 3).Alignment = AlignmentEnum.RightCenter;

                gdChart.Column(1).AutoFit();
                gdChart.Column(2).AutoFit();
                gdChart.Column(3).AutoFit();
            }

            gdChart.Range(3, 4, list.Count + 3, gdChart.Cols - 1).Merge();
            gdChart.AddChart(3, 4);
            Chart chart1 = gdChart.Chart(3, 4);
            chart1.LineWidth = 3;
            chart1.DisplayDataLabels = true;
            chart1.PlotBy = Chart.PlotTypeEnum.Columns;
            chart1.ChartType = Chart.ChartTypeEnum.Line;
            chart1.SetDataSource(3, 1, list.Count + 3, 3);
            #endregion

            if (projectInfo!=null)
            {
                gdChart.Refresh();
                return;
            }

            #region 延期分布图

            startRowIndex++;
            gdChart.Range(startRowIndex, 1, 1 + startRowIndex, gdChart.Cols - 1).Merge();
            gdChart.Cell(startRowIndex, 1).Text = string.Format("{0} 项目延期分布图", dtpDate.Value.Date.ToString("yyyy-MM-dd"));
            gdChart.Cell(startRowIndex, 1).Alignment = AlignmentEnum.CenterCenter;
            gdChart.Cell(startRowIndex, 1).FontSize = 18;
            gdChart.Cell(startRowIndex, 1).FontBold = true;

            cols = new string[] {"归属组织", "绿色预警", "蓝色预警", "黄色预警", "红色预警"};
            startRowIndex += 2;
            for (var i = 0; i < cols.Length; i++)
            {
                gdChart.Cell(startRowIndex, i + 1).Text = cols[i];
                gdChart.Cell(startRowIndex, i + 1).FontBold = true;
            }

            var q = from dt in delayDetails
                    group dt by dt.OwnerOrg
                    into gps
                    select new
                               {
                                   gps.Key,
                                   GreenWarn = gps.Count(dt => dt.WarnLevel == 0),
                                   BlueWarn = gps.Count(dt => dt.WarnLevel == 1),
                                   YellowWarn = gps.Count(dt => dt.WarnLevel == 2),
                                   RedWarn = gps.Count(dt => dt.WarnLevel == 3)
                               };

            foreach(var tm in q)
            {
                startRowIndex++;
                gdChart.Cell(startRowIndex, 1).Text = string.IsNullOrEmpty(tm.Key) ? "-" : tm.Key;
                gdChart.Cell(startRowIndex, 2).Text = tm.GreenWarn.ToString("N0");
                gdChart.Cell(startRowIndex, 3).Text = tm.BlueWarn.ToString("N0");
                gdChart.Cell(startRowIndex, 4).Text = tm.YellowWarn.ToString("N0");
                gdChart.Cell(startRowIndex, 5).Text = tm.RedWarn.ToString("N0");

                for (var i = 2; i <= 5;i++ )
                {
                    gdChart.Cell(startRowIndex, i).Alignment = AlignmentEnum.CenterCenter;
                    gdChart.Column(i).AutoFit();
                }
            }

            gdChart.Range(startRowIndex - q.Count(), 6, startRowIndex, gdChart.Cols - 1).Merge();
            gdChart.AddChart(startRowIndex - q.Count(), 6);
            Chart chart2 = gdChart.Chart(startRowIndex - q.Count(), 6);
            chart2.LineWidth = 3;
            chart2.DisplayDataLabels = true;
            chart2.PlotBy = Chart.PlotTypeEnum.Columns;
            chart2.ChartType = Chart.ChartTypeEnum.ColumnClustered;
            chart2.SetDataSource(startRowIndex - q.Count(), 1, startRowIndex, 5);
            #endregion

            gdChart.Refresh();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("查询中，请稍候...");

            selectProject = string.Empty;

            ShowTotalReport();

            InitDetailReport();

            FlashScreen.Close();
        }

        private void btnOperationOrg_Click(object sender, EventArgs e)
        {
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect(true);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                OperationOrgInfo info = frm.Result[0] as OperationOrgInfo;

                txtOperationOrg.Tag = info;
                txtOperationOrg.Text = info != null ? info.Name : string.Empty;
            }
        }

        private void flexGrid_Click(object sender, EventArgs e)
        {
            if (flexGridTotal.ActiveCell.Col != 1)
            {
                return;
            }

            FlashScreen.Show("加载中，请稍候...");

            selectProject = flexGridTotal.ActiveCell.Tag;

            ShowDetailReport();

            ShowDelayChart();

            tabControl1.SelectTab(tabPageDetail);

            FlashScreen.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var projId = projectInfo == null ? string.Empty : projectInfo.Id;

            MProductionMng mpm = new MProductionMng();


            if (mpm.ProductionManagementSrv.CreateProjectDelayDays(projId))
            //if (mpm.ProductionManagementSrv.CreateProjectDelayDays(""))
            {
                MessageBox.Show("保存成功");
            }
            else
            {
                MessageBox.Show("保存失败");
            }
        }
       
    }
}
