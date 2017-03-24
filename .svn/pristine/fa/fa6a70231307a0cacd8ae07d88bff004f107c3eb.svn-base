using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using System.Collections;
using VirtualMachine.Component.Util;
using FlexCell;
using VirtualMachine.Core;
using NHibernate.Criterion;
using NHibernate;


namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI
{
    public partial class VPreViewCriPath : TBasicDataView
    {
        private CurrentProjectInfo projectInfo;
        private WeekScheduleMaster CurWSDMaster;
        private DateTime defaultTime = new DateTime(1900, 1, 1);
        private MProductionMng model = new MProductionMng();

        public VPreViewCriPath(CurrentProjectInfo _projectInfo)
        {
            this.projectInfo = _projectInfo;
            this.CurWSDMaster = GetCurWSDMaster();
            InitializeComponent();
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.gdChart.Click += new Grid.ClickEventHandler(gdChart_Click);
            InitFlexGrid();
            GetDelayInfo(CurWSDMaster);

        }
        private WeekScheduleMaster GetCurWSDMaster()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("ExecScheduleType", EnumExecScheduleType.总体进度计划));
            oq.AddOrder(Order.Desc("CreateDate"));

            oq.AddFetchMode("Details", FetchMode.Eager);
            oq.AddFetchMode("Details.RalationDetails", FetchMode.Eager);
            IList listMaster = model.ObjectQuery(typeof(WeekScheduleMaster), oq);
            if (listMaster == null || listMaster.Count == 0)
                return null;
            return listMaster[0] as WeekScheduleMaster;

        }
        void gdChart_Click(object Sender, EventArgs e)
        {
            this.cTextBox1.Text = this.gdChart.ActiveCell.Text;
        }


        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnCancel.FindForm().Close();
        }

        private bool IsExistsInWsdls(WeekScheduleDetail detail)
        {
            foreach (var item in CurWSDMaster.Details)
            {
                if (item.RalationDetails == null || item.RalationDetails.Count == 0)
                    continue;
                //以当前节点作为前置节点
                WeekScheduleRalation wsdr_frind = item.RalationDetails.ToList<WeekScheduleRalation>().Find(a => a.FrontWeekScheduleDetail.Id == detail.Id);
                if (wsdr_frind != null)
                {
                    return true;
                }
            }
            return false;
        }

        private void GetDelayInfo(WeekScheduleMaster WSDMaster)
        {
            if (WSDMaster.Details.Count == 0)
                return;
            //获取总进度计划中当前正在施工的工程任务
            var aa = from a in CurWSDMaster.Details
                     where (a.PlannedBeginDate != null && a.PlannedBeginDate > defaultTime)
                     && (a.PlannedEndDate != null && a.PlannedBeginDate > defaultTime)
                     && (a.ActualBeginDate != null && a.ActualBeginDate > defaultTime)
                     && (a.TaskCompletedPercent > (decimal)0 && a.TaskCompletedPercent < (decimal)100)
                     select a;


            if (aa == null || aa.Count() == 0)
            {
                DateTime minPBeginDate = CurWSDMaster.Details.Where<WeekScheduleDetail>(a => IsExistsInWsdls(a)).OrderBy(p => p.PlannedBeginDate).FirstOrDefault().PlannedBeginDate;

                aa =from a in CurWSDMaster.Details
                    where IsExistsInWsdls(a) && a.PlannedBeginDate == minPBeginDate
                    select a;
            }

            if (aa == null || aa.Count() == 0)
                return ;

            ArrayList listDatePlan = new ArrayList();
            
            foreach (var item in aa)
            {
                DateTime maxDate = item.PlannedEndDate;
                WeekScheduleDetail detail_maxdate = null;
                MaintainWorkDateInfoFollow(CurWSDMaster, item, ref maxDate, ref detail_maxdate,false);
                listDatePlan.Add(new object[] { maxDate, detail_maxdate });
            }

            ArrayList listDateAct = new ArrayList();
            foreach (var item in aa)
            {
                SetActualEndDate(item);
                DateTime maxDate = item.ActualEndDate;
                WeekScheduleDetail detail_maxdate = null;
                MaintainWorkDateInfoFollow(CurWSDMaster, item, ref maxDate, ref detail_maxdate ,true);
                listDateAct.Add(new object[] { maxDate, detail_maxdate });
            }



            DateTime DatePlan = defaultTime;
            WeekScheduleDetail detail_maxPlan = null;
            for (int i = 0; i < listDatePlan.Count; i++)
            {
                if (ClientUtil.ToDateTime(((object[])listDatePlan[i])[0]) > DatePlan)
                { 
                     DatePlan = ClientUtil.ToDateTime(((object[])listDatePlan[i])[0]);
                     detail_maxPlan = ((object[])listDatePlan[i])[1] as WeekScheduleDetail;
                }

            }

            DateTime DateAct = defaultTime;
            WeekScheduleDetail detail_maxAct = null;
            for (int i = 0; i < listDateAct.Count; i++)
            {
                if (ClientUtil.ToDateTime(((object[])listDateAct[i])[0]) > DateAct)
                {
                    DateAct = ClientUtil.ToDateTime(((object[])listDateAct[i])[0]);
                    detail_maxAct = ((object[])listDateAct[i])[1] as WeekScheduleDetail;
                }
            }

            List<WeekScheduleDetail> criPath_Plan = new List<WeekScheduleDetail>();
            List<WeekScheduleDetail> criPath_Act = new List<WeekScheduleDetail>();
            criPath_Plan.Add(detail_maxPlan);
            FindCriPath(CurWSDMaster, detail_maxPlan, false, ref criPath_Plan, ref criPath_Act);


            criPath_Act.Add(detail_maxAct);
            FindCriPath(CurWSDMaster, detail_maxAct, true, ref criPath_Plan, ref criPath_Act);

            FillFlex(criPath_Plan, criPath_Act);

            ShowGridChart(criPath_Plan, criPath_Act);
           
        }

        private void ShowGridChart(List<WeekScheduleDetail> criPath_Plan, List<WeekScheduleDetail> criPath_Act)
        {

            FlexCell.Range range;
            FlexCell.Chart chart;

            grid1.AutoRedraw = false;
            grid1.ExtendLastCol = true;

            // 设置行数/列数
            grid1.Rows = 60;
            grid1.Cols = criPath_Plan.Count > criPath_Act.Count ? criPath_Plan.Count + 2 : criPath_Act.Count + 2;

            // 设置行高/列宽
            grid1.Row(1).Height = 30;

            for (int i = 1; i < grid1.Cols; i++)
            {
                grid1.Column(i).Width = 70;
            }
            grid1.Column(1).Width = 90;
            #region 计划关键路径
            // 设置单元格文字
            range = grid1.Range(1, 1, 1, criPath_Plan.Count + 1);
            range.Merge();
            grid1.Cell(1, 1).Text = CurWSDMaster.ProjectName + "总进度计划设置情况关键路径";
            grid1.Cell(1, 1).FontBold = true;
            range.Alignment = AlignmentEnum.CenterCenter;

            grid1.Cell(2, 1).Text = "节 点";
            grid1.Cell(2, 1).FontBold = true;
            grid1.Cell(2, 1).Alignment = AlignmentEnum.CenterCenter;
            grid1.Cell(3, 1).Text = "计划路径";
            grid1.Cell(3, 1).FontBold = true;
            grid1.Cell(3, 1).Alignment = AlignmentEnum.CenterCenter;
            grid1.Cell(4, 1).Text = "计划开始时间";
            grid1.Cell(4, 1).FontBold = true;
            grid1.Cell(4, 1).Alignment = AlignmentEnum.CenterCenter;
            grid1.Cell(5, 1).Text = "计划结束时间";
            grid1.Cell(5, 1).FontBold = true;
            grid1.Cell(5, 1).Alignment = AlignmentEnum.CenterCenter;
            grid1.Cell(6, 1).Text = "计划工期";
            grid1.Cell(6, 1).FontBold = true;
            grid1.Cell(6, 1).Alignment = AlignmentEnum.CenterCenter;

            int k = 0;
            for (int i = criPath_Plan.Count - 1; i > -1; i--)
            {
                int col = 2 + k;
                WeekScheduleDetail wsdp = criPath_Plan[i] as WeekScheduleDetail;
                grid1.Cell(2, col).Text = wsdp.GWBSTreeName;
                grid1.Cell(2, col).FontBold = true;
                grid1.Cell(3, col).Text = ClientUtil.ToString((wsdp.PlannedEndDate - criPath_Plan[criPath_Plan.Count - 1].PlannedEndDate).Days + 1);
                grid1.Cell(4, col).Text = ClientUtil.ToString(wsdp.PlannedBeginDate.ToString("yyyy-MM-dd"));
                grid1.Cell(5, col).Text = ClientUtil.ToString(wsdp.PlannedEndDate.ToString("yyyy-MM-dd"));
                grid1.Cell(6, col).Text = ClientUtil.ToString(wsdp.PlannedDuration);
                k++;


            }

            // 添加一个3D折线图表，并设置细边框
            range = grid1.Range(7, 1, 17, criPath_Plan.Count+1);
            range.Merge();
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);

            grid1.AddChart(7, 1);
            chart = grid1.Chart(7, 1);
            chart.PlotBy = FlexCell.Chart.PlotTypeEnum.Rows;
            chart.SetDataSource(2, 1, 3, criPath_Plan.Count+1);
            chart.ChartType = FlexCell.Chart.ChartTypeEnum.Line3D;
            chart.DisplayDataLabels = true;
            chart.ScaleFont = false;
            #endregion

            #region 实际关键路径
            // 设置单元格文字
            int beginrow = 18;
            range = grid1.Range(beginrow, 1, beginrow, criPath_Act.Count + 1);
            range.Merge();
            grid1.Cell(beginrow, 1).Text = CurWSDMaster.ProjectName + "实际执行情况关键路径";
            grid1.Cell(beginrow, 1).FontBold = true;
            range.Alignment = AlignmentEnum.CenterCenter;

            grid1.Cell(beginrow + 1, 1).Text = "节 点";
            grid1.Cell(beginrow + 1, 1).FontBold = true;
            grid1.Cell(beginrow + 1, 1).Alignment = AlignmentEnum.CenterCenter;
            grid1.Cell(beginrow + 2, 1).Text = "实际路径";
            grid1.Cell(beginrow + 2, 1).FontBold = true;
            grid1.Cell(beginrow + 2, 1).Alignment = AlignmentEnum.CenterCenter;
            grid1.Cell(beginrow + 3, 1).Text = "实际开始时间";
            grid1.Cell(beginrow + 3, 1).FontBold = true;
            grid1.Cell(beginrow + 3, 1).Alignment = AlignmentEnum.CenterCenter;
            grid1.Cell(beginrow + 4, 1).Text = "实际结束时间";
            grid1.Cell(beginrow + 4, 1).FontBold = true;
            grid1.Cell(beginrow + 4, 1).Alignment = AlignmentEnum.CenterCenter;
            grid1.Cell(beginrow + 5, 1).Text = "实际工期";
            grid1.Cell(beginrow + 5, 1).FontBold = true;
            grid1.Cell(beginrow + 5, 1).Alignment = AlignmentEnum.CenterCenter;

            int j = 0;
            for (int i = criPath_Act.Count - 1; i > -1; i--)
            {
                int col = 2 + j;
                WeekScheduleDetail wsda = criPath_Plan[i] as WeekScheduleDetail;
                grid1.Cell(beginrow + 1, col).Text = wsda.GWBSTreeName;
                grid1.Cell(beginrow + 1, col).FontBold = true;
                grid1.Cell(beginrow + 2, col).Text = ClientUtil.ToString((wsda.ActualEndDate - criPath_Plan[criPath_Plan.Count - 1].PlannedEndDate).Days + 1);
                grid1.Cell(beginrow + 3, col).Text = ClientUtil.ToString(wsda.ActualBeginDate.ToString("yyyy-MM-dd"));
                grid1.Cell(beginrow + 4, col).Text = ClientUtil.ToString(wsda.ActualEndDate.ToString("yyyy-MM-dd"));
                grid1.Cell(beginrow + 5, col).Text = ClientUtil.ToString(wsda.ActualDuration);
                j++;


            }

            // 添加一个3D折线图表，并设置细边框
            range = grid1.Range(beginrow + 6, 1, beginrow +16, criPath_Act.Count + 1);
            range.Merge();
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);

            grid1.AddChart(beginrow + 6, 1);
            chart = grid1.Chart(beginrow + 6, 1);
            chart.PlotBy = FlexCell.Chart.PlotTypeEnum.Rows;
            chart.SetDataSource(beginrow + 1, 1, beginrow + 2, criPath_Act.Count + 1);
            chart.ChartType = FlexCell.Chart.ChartTypeEnum.Line3D;
            chart.DisplayDataLabels = true;
            chart.ScaleFont = false;
            #endregion
            grid1.AutoRedraw = true;
              grid1.Refresh();
        }
        private void FillFlex(List<WeekScheduleDetail> criPath_Plan, List<WeekScheduleDetail> criPath_Act)
        {
            DateTime dt_min1 = criPath_Plan[criPath_Plan.Count - 1].PlannedBeginDate;
            DateTime dt_min2 = criPath_Act[criPath_Act.Count - 1].ActualBeginDate;

            DateTime dt_max1 = criPath_Plan[0].PlannedEndDate;
            DateTime dt_max2 = criPath_Act[0].ActualEndDate;

            DateTime dt_min = dt_min1 > dt_min2 ? dt_min2 : dt_min1;
            DateTime dt_max = dt_max1 > dt_max2 ? dt_max1 : dt_max2;

            int days = (dt_max - dt_min).Days + 1;

            gdChart.Cols = days+1;

            gdChart.Range(1, 1, 2, gdChart.Cols - 1).Merge();
            gdChart.Cell(1, 1).Text = string.Format("{0} 关键路径预览", projectInfo.Name);
            gdChart.Cell(1, 1).Alignment = AlignmentEnum.CenterCenter;
            gdChart.Cell(1, 1).FontSize = 18;
            gdChart.Cell(1, 1).FontBold = true;

            var startRowIndex = 3;
            for (int i = 0; i < days; i += 5)
            {  
                Range rg = gdChart.Range(startRowIndex, i + 1, startRowIndex, (i + 3)>days?days:i+3);
                rg.Merge();
                gdChart.Cell(startRowIndex, i + 1).Text = (dt_min.AddDays(i)).ToString("MM.dd");
                gdChart.Cell(startRowIndex, i + 1).FontBold = true;

                //gdChart.Cell(startRowIndex, i + 1).Text = (dt_min.AddDays(i)).ToString("MM.dd");
                //gdChart.Cell(startRowIndex, i + 1).Font = new Font("Tahoma", 3);
                //gdChart.Cell(startRowIndex, i + 1).FontBold = true;
            }

            startRowIndex++;
            int k = 0;
            foreach (var item in criPath_Plan)
            {
                int bcol = (item.PlannedBeginDate - dt_min).Days + 1;
                int ecol = (item.PlannedEndDate - dt_min).Days + 1;

                Range rg = gdChart.Range(startRowIndex + k % 2, bcol, startRowIndex + k % 2, ecol);
                rg.Merge();
                rg.BackColor = Color.DarkOrange;

                gdChart.Cell(startRowIndex + k % 2, bcol).Text = item.GWBSTreeName;
                k++;
            }

            startRowIndex ++;
            startRowIndex++;

            Range rg2 = gdChart.Range(startRowIndex, 1, startRowIndex, days);
            rg2.Merge();


            startRowIndex++;

            k = 0;
            foreach (var item in criPath_Act)
            {
                int bcol = (item.ActualBeginDate - dt_min).Days + 1;
                int ecol = (item.ActualEndDate - dt_min).Days + 1;

                Range rg3 = gdChart.Range(startRowIndex + k % 2, bcol, startRowIndex + k % 2, ecol);
                rg3.Merge();
                rg3.BackColor = Color.DarkRed;

                gdChart.Cell(startRowIndex + k % 2, bcol).Text = item.GWBSTreeName;
                k++;
            }
            for (int i = 0; i < gdChart.Cols; i++)
            {
                gdChart.Column(i).Width=20; 
            }

            gdChart.Refresh();
        }

        private void InitFlexGrid()
        {
            gdChart.Rows = 9;
            //gdChart.Cols = days;
            gdChart.BorderStyle = BorderStyleEnum.FixedSingle;
            gdChart.BackColorBkg = SystemColors.Control;
            gdChart.DefaultFont = new Font("Tahoma", 9);
            gdChart.DisplayRowArrow = true;
            gdChart.DisplayRowNumber = true;

           
        }

        private void FindCriPath(WeekScheduleMaster CurWSDMaster, WeekScheduleDetail detail, bool isAct, ref List<WeekScheduleDetail> criPath_Plan, ref List<WeekScheduleDetail> criPath_Act)
        {
            if (detail.RalationDetails == null || detail.RalationDetails.Count == 0 )
                return;
            DateTime maxBegindate = defaultTime;

            foreach (var item in detail.RalationDetails)
            {
                //当前节点的前置节点
                WeekScheduleDetail wsd = CurWSDMaster.Details.ToList<WeekScheduleDetail>().Find(a => a.Id == item.FrontWeekScheduleDetail.Id);
                if (item.DelayRule == EnumDelayRule.FS)
                {
                    DateTime dtbegin_wsd = new DateTime(1900, 1, 1);
                    if (isAct)
                    {
                        if (wsd.ActualEndDate == null || wsd.ActualEndDate == defaultTime)
                            continue;
                        dtbegin_wsd = (DateTime)GetDateByDelay(wsd.ActualEndDate, item.DelayDays);
                        //maxBegindate = maxBegindate < dtbegin_wsd ? dtbegin_wsd : maxBegindate;
                        if (dtbegin_wsd == detail.ActualBeginDate)
                        {
                            criPath_Act.Add(wsd);
                            FindCriPath(CurWSDMaster, wsd, isAct, ref criPath_Plan, ref criPath_Act);
                        }
                    }
                    else
                    {
                        if (wsd.PlannedEndDate == null || wsd.PlannedEndDate == defaultTime)
                            continue;
                        dtbegin_wsd = (DateTime)GetDateByDelay(wsd.PlannedEndDate, item.DelayDays);
                        //maxBegindate = maxBegindate < dtbegin_wsd ? dtbegin_wsd : maxBegindate;
                        if (dtbegin_wsd == detail.PlannedBeginDate)
                        {
                            criPath_Plan.Add(wsd);
                            FindCriPath(CurWSDMaster, wsd, isAct, ref criPath_Plan, ref criPath_Act);
                        }
                    }
                }
                if (item.DelayRule == EnumDelayRule.SS)
                {
                    DateTime dtbegin_wsd = new DateTime(1900, 1, 1);
                    if (isAct)
                    {
                        if (wsd.ActualBeginDate == null || wsd.ActualBeginDate == defaultTime)
                            continue;
                        dtbegin_wsd = (DateTime)GetDateByDelay(wsd.ActualBeginDate, item.DelayDays);
                        //maxBegindate = maxBegindate < dtbegin_wsd ? dtbegin_wsd : maxBegindate;
                        if (dtbegin_wsd == detail.ActualBeginDate)
                        {
                            criPath_Act.Add(wsd);
                            FindCriPath(CurWSDMaster, wsd, isAct, ref criPath_Plan, ref criPath_Act);
                        }
                    }
                    else
                    {
                        if (wsd.PlannedBeginDate == null || wsd.PlannedBeginDate == defaultTime)
                            continue;
                        dtbegin_wsd = (DateTime)GetDateByDelay(wsd.PlannedBeginDate, item.DelayDays);
                        //maxBegindate = maxBegindate < dtbegin_wsd ? dtbegin_wsd : maxBegindate;
                        if (dtbegin_wsd == detail.PlannedBeginDate)
                        {
                            criPath_Plan.Add(wsd);
                            FindCriPath(CurWSDMaster, wsd, isAct, ref criPath_Plan, ref criPath_Act);
                        }
                    }
                }
                if (item.DelayRule == EnumDelayRule.FF)
                {
                    DateTime dtend_wsd = new DateTime(1900, 1, 1);
                    DateTime dtbegin_wsd = new DateTime(1900, 1, 1);
                    if (isAct)
                    {
                        if (wsd.ActualEndDate == null || wsd.ActualEndDate == defaultTime)
                            continue;
                        dtend_wsd = (DateTime)GetDateByDelay(wsd.ActualEndDate, item.DelayDays);
                        if (wsd.PlannedDuration == (decimal)0)
                            break;
                        dtbegin_wsd = (DateTime)GetBenginOrEndDate((DateTime?)dtend_wsd, (int?)wsd.PlannedDuration, DateRel.GetBeginDate);
                        //maxBegindate = maxBegindate < dtbegin_wsd ? dtbegin_wsd : maxBegindate;
                        if (dtbegin_wsd == detail.ActualBeginDate)
                        {
                            criPath_Act.Add(wsd);
                            FindCriPath(CurWSDMaster, wsd, isAct, ref criPath_Plan, ref criPath_Act);
                        }

                    }
                    else
                    {
                        if (wsd.PlannedEndDate == null || wsd.PlannedEndDate == defaultTime)
                            continue;
                        dtend_wsd = (DateTime)GetDateByDelay(wsd.PlannedEndDate, item.DelayDays);
                        if (wsd.PlannedDuration == (decimal)0)
                            break;
                        dtbegin_wsd = (DateTime)GetBenginOrEndDate((DateTime?)dtend_wsd, (int?)wsd.PlannedDuration, DateRel.GetBeginDate);
                        //maxBegindate = maxBegindate < dtbegin_wsd ? dtbegin_wsd : maxBegindate;
                        if (dtbegin_wsd == detail.PlannedBeginDate)
                        {
                            criPath_Plan.Add(wsd);
                            FindCriPath(CurWSDMaster, wsd, isAct, ref criPath_Plan, ref criPath_Act);
                        }
                    }
                }
            }
           
        }

        /// <summary>
        /// 根据当前节点的前置节点 维护当前节点工期信息
        /// </summary>
        /// <param name="detail">当前节点</param>
        private void MaintainWorkDateInfoByFront(WeekScheduleMaster CurWSDMaster, WeekScheduleDetail detail,bool isAct)
        {
            if (detail.RalationDetails == null || detail.RalationDetails.Count == 0)
                return;
            DateTime maxBegindate = defaultTime;

            foreach (var item in detail.RalationDetails)
            {
                //当前节点的前置节点
                WeekScheduleDetail wsd = CurWSDMaster.Details.ToList<WeekScheduleDetail>().Find(a => a.Id == item.FrontWeekScheduleDetail.Id);
                if (item.DelayRule == EnumDelayRule.FS)
                {
                    DateTime dtbegin_wsd = new DateTime(1900,1,1);
                    if (isAct)
                    {
                        if (wsd.ActualEndDate == null || wsd.ActualEndDate == defaultTime)
                            continue;
                        dtbegin_wsd = (DateTime)GetDateByDelay(wsd.ActualEndDate, item.DelayDays);
                        maxBegindate = maxBegindate < dtbegin_wsd ? dtbegin_wsd : maxBegindate;
                    }
                    else
                    {
                        if (wsd.PlannedEndDate == null || wsd.PlannedEndDate == defaultTime)
                            continue;
                        dtbegin_wsd = (DateTime)GetDateByDelay(wsd.PlannedEndDate, item.DelayDays);
                        maxBegindate = maxBegindate < dtbegin_wsd ? dtbegin_wsd : maxBegindate;
                    }
                }
                if (item.DelayRule == EnumDelayRule.SS)
                {
                    DateTime dtbegin_wsd = new DateTime(1900, 1, 1);
                    if (isAct)
                    {
                        if (wsd.ActualBeginDate == null || wsd.ActualBeginDate == defaultTime)
                            continue;
                        dtbegin_wsd = (DateTime)GetDateByDelay(wsd.ActualBeginDate, item.DelayDays);
                        maxBegindate = maxBegindate < dtbegin_wsd ? dtbegin_wsd : maxBegindate;
                    }
                    else
                    {
                        if (wsd.PlannedBeginDate == null || wsd.PlannedBeginDate == defaultTime)
                            continue;
                        dtbegin_wsd = (DateTime)GetDateByDelay(wsd.PlannedBeginDate, item.DelayDays);
                        maxBegindate = maxBegindate < dtbegin_wsd ? dtbegin_wsd : maxBegindate;
                    }
                }
                if (item.DelayRule == EnumDelayRule.FF)
                {
                    DateTime dtend_wsd = new DateTime(1900, 1, 1);
                    DateTime dtbegin_wsd = new DateTime(1900, 1, 1);
                    if (isAct)
                    {
                        if (wsd.ActualEndDate == null || wsd.ActualEndDate == defaultTime)
                            continue;
                        dtend_wsd = (DateTime)GetDateByDelay(wsd.ActualEndDate, item.DelayDays);
                        if (wsd.PlannedDuration == (decimal)0)
                            break;
                        dtbegin_wsd = (DateTime)GetBenginOrEndDate((DateTime?)dtend_wsd, (int?)wsd.PlannedDuration, DateRel.GetBeginDate);
                        maxBegindate = maxBegindate < dtbegin_wsd ? dtbegin_wsd : maxBegindate;
                    }
                    else
                    {
                        if (wsd.PlannedEndDate == null || wsd.PlannedEndDate == defaultTime)
                            continue;
                        dtend_wsd = (DateTime)GetDateByDelay(wsd.PlannedEndDate, item.DelayDays);
                        if (wsd.PlannedDuration == (decimal)0)
                            break;
                        dtbegin_wsd = (DateTime)GetBenginOrEndDate((DateTime?)dtend_wsd, (int?)wsd.PlannedDuration, DateRel.GetBeginDate);
                        maxBegindate = maxBegindate < dtbegin_wsd ? dtbegin_wsd : maxBegindate;
                    }
                }
            }
            if (maxBegindate != defaultTime)
            {
                if (isAct)
                {
                    detail.ActualBeginDate = maxBegindate;
                    if (detail.PlannedDuration != (decimal)0)
                        detail.ActualEndDate = (DateTime)GetBenginOrEndDate((DateTime?)maxBegindate, (int?)detail.PlannedDuration, DateRel.GetEndDate);
                }
                else
                {
                    detail.PlannedBeginDate = maxBegindate;
                    if (detail.PlannedDuration != (decimal)0)
                        detail.PlannedEndDate = (DateTime)GetBenginOrEndDate((DateTime?)maxBegindate, (int?)detail.PlannedDuration, DateRel.GetEndDate);
                }
            }

        }

        /// <summary>
        /// 根据当前节点 维护以当前节点为前置任务的节点的工期信息
        /// </summary>
        /// <param name="detail"></param>
        private void MaintainWorkDateInfoFollow(WeekScheduleMaster WSDMaster, WeekScheduleDetail detail,ref DateTime maxDate, ref WeekScheduleDetail detail_maxdate,bool isAct)
        {
            foreach (var item in WSDMaster.Details)
            {
                if (item.RalationDetails == null || item.RalationDetails.Count == 0)
                    continue;
                //以当前节点作为前置节点
                WeekScheduleRalation wsdr_frind = item.RalationDetails.ToList<WeekScheduleRalation>().Find(a => a.FrontWeekScheduleDetail.Id == detail.Id);
                if (wsdr_frind != null)
                {
                    WeekScheduleDetail wsd = wsdr_frind.Master;

                    MaintainWorkDateInfoByFront(WSDMaster, wsd, isAct);
                    if (isAct)
                    {
                        if (wsd.ActualEndDate > maxDate)
                        {
                            maxDate = wsd.ActualEndDate;
                            detail_maxdate = wsd;
                        }

                    }
                    else
                    {
                        if (wsd.PlannedEndDate > maxDate)
                        {
                            maxDate = wsd.PlannedEndDate;
                            detail_maxdate = wsd;
                        }
                    }

                    MaintainWorkDateInfoFollow(WSDMaster, wsd, ref maxDate, ref detail_maxdate,isAct);
                }

            }
        }

        private void SetActualEndDate(WeekScheduleDetail detail)
        {
            if (detail.ActualBeginDate != null && detail.ActualBeginDate != defaultTime && detail.TaskCompletedPercent < (decimal)100)
            {
                if (detail.TaskCompletedPercent == (decimal)0)
                    detail.ActualEndDate = detail.ActualBeginDate.AddDays(ClientUtil.Tofloat((DateTime.Now - detail.ActualBeginDate).Days + 1 + detail.PlannedDuration));
                else
                    detail.ActualEndDate = detail.ActualBeginDate.AddDays(ClientUtil.Tofloat(((detail.ActualEndDate - detail.ActualBeginDate).Days + 1) * 100 / detail.TaskCompletedPercent));
            }
            else
            {
                detail.ActualBeginDate = DateTime.Now;
                detail.ActualEndDate = DateTime.Now.AddDays((float)detail.PlannedDuration);
            }
        }



        private DateTime? GetDateByDelay(DateTime? date1, int delaydays)
        {
            if (date1 != null && date1 != defaultTime)
                return ((DateTime)date1).AddDays(delaydays + 1);
            return null;
        }

        private DateTime? GetBenginOrEndDate(DateTime? dt_date1, int? i_workdays, DateRel dr)
        {
            DateTime? date_Rtn = null;
            if (i_workdays == null)
                return null;
            switch (dr)
            {
                case DateRel.GetBeginDate:
                    if (dt_date1 == null || dt_date1 == defaultTime || i_workdays == null)
                        break;
                    date_Rtn = ((DateTime)dt_date1).AddDays(1 - (int)i_workdays);
                    break;
                case DateRel.GetEndDate:
                    if (dt_date1 == null || dt_date1 == defaultTime || i_workdays == null)
                        break;
                    date_Rtn = ((DateTime)dt_date1).AddDays((int)i_workdays - 1);
                    break;
            }
            return date_Rtn;
        }

    }
   
}
