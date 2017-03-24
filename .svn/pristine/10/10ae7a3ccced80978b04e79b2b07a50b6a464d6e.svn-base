using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI;
using Application.Business.Erp.SupplyChain.Client.Properties;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using FlexCell;
using IRPServiceModel.Basic;
using IRPServiceModel.Domain.Document;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Core;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI
{
    public partial class VWeekScheduleEdit : TBasicDataView
    {
        private MProductionMng model = new MProductionMng();
        private WeekScheduleMaster weekPlan;
        private WeekScheduleMaster monthPlan;
        private CurrentProjectInfo projectInfo;
        private Hashtable detailHashtable = new Hashtable();
        private WeekScheduleDetail findDetail;
        private List<string> listDtlIds = new List<string>();

        private const int startImageCol = 1, endImageCol = 19;
        private const string imageExpand = "imageExpand";
        private const string imageCollapse = "imageCollapse";
        private DateTime defaultTime = new DateTime(1900, 1, 1);

        public VWeekScheduleEdit()
        {
            InitializeComponent();

            InitEvents();

            InitData();
        }

        private void InitEvents()
        {
            cmbMonthPlan.SelectedIndexChanged += new EventHandler(cmbMonthPlan_SelectedIndexChanged);
            cmbWeekPlan.SelectedIndexChanged += new EventHandler(cmbWeekPlan_SelectedIndexChanged);

            btnCreateNewVersion.Click += new EventHandler(btnCreateNewVersion_Click);
            btnDeletePlan.Click += new EventHandler(btnDeletePlan_Click);
            btnDownLoadDocument.Click += new EventHandler(btnDownLoadDocument_Click);
            btnFindNext.Click += new EventHandler(btnFindNext_Click);
            btnOpenDocument.Click += new EventHandler(btnOpenDocument_Click);
            btnExport.Click += btnExportToMPP_Click;
            btnCreateBill.Click += new EventHandler(btnCreateBill_Click);

            chkOnlyWeek.CheckedChanged += new EventHandler(chkOnlyWeek_CheckedChanged);
            lnkCheckAll.Click += new EventHandler(lnkCheckAll_Click);
            lnkCheckAllNot.Click += new EventHandler(lnkCheckAllNot_Click);

            dgDocumentMast.CellClick += new DataGridViewCellEventHandler(dgDocumentMast_CellClick);
            flexGrid.Click += new Grid.ClickEventHandler(flexGrid_Click);

            dtpBeginDate.ValueChanged += new EventHandler(dtpBeginDate_ValueChanged);
        }

        private void InitData()
        {
            //归属项目
            projectInfo = StaticMethod.GetProjectInfo();

            InitFlexGrid(10);

            InitMonthPlan();

            dtpBeginDate.Value = DateTime.Now;
        }

        private IList GetProductionSchedules(EnumExecScheduleType schType, string forwardBillId)
        {
            if (projectInfo == null)
            {
                return null;
            }

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("ExecScheduleType", schType));
            //oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            if (!string.IsNullOrEmpty(forwardBillId))
            {
                oq.AddCriterion(Expression.Eq("ForwardBillId", forwardBillId));
            }
            oq.AddOrder(Order.Desc("CreateDate"));

            return model.ProductionManagementSrv.GetWeekScheduleMaster(oq);
        }

        private void InitMonthPlan()
        {
            var schedules = GetProductionSchedules(EnumExecScheduleType.总体进度计划, null);
            if (schedules != null && schedules.Count > 0)
            {
                var list = schedules.OfType<WeekScheduleMaster>().ToList();
                list.Insert(0, new WeekScheduleMaster());

                cmbMonthPlan.DataSource = list;
                cmbMonthPlan.DisplayMember = "PlanName";
                cmbMonthPlan.ValueMember = "Id";
            }
            else
            {
                cmbMonthPlan.DataSource = null;
                flexGrid.Rows = 1;
                dgDocumentDetail.Rows.Clear();
                dgDocumentMast.Rows.Clear();
            }
        }

        private void InitWeekPlan(string mPlanId)
        {
            var schedules = GetProductionSchedules(EnumExecScheduleType.周进度计划, mPlanId);
            if (schedules != null && schedules.Count > 0)
            {
                var list = schedules.OfType<WeekScheduleMaster>().ToList();
                list.Insert(0, new WeekScheduleMaster());

                cmbWeekPlan.DataSource = list;
                cmbWeekPlan.DisplayMember = "PlanName";
                cmbWeekPlan.ValueMember = "Id";
            }
            else
            {
                cmbWeekPlan.DataSource = null;
                flexGrid.Rows = 1;
                dgDocumentDetail.Rows.Clear();
                dgDocumentMast.Rows.Clear();
            }
        }

        private void InitFlexGrid(int rows)
        {
            flexGrid.AutoRedraw = false;
            flexGrid.Column(0).Visible = true;

            flexGrid.Rows = rows;
            flexGrid.Cols = 36;//其中0列隐藏 1-19 为放置图片列 20-31为数据列

            flexGrid.SelectionMode = SelectionModeEnum.ByCell;
            flexGrid.ExtendLastCol = true;
            flexGrid.DisplayFocusRect = false;
            flexGrid.LockButton = true;
            flexGrid.ReadonlyFocusRect = FocusRectEnum.Solid;
            flexGrid.BorderStyle = BorderStyleEnum.FixedSingle;
            flexGrid.ScrollBars = ScrollBarsEnum.Both;
            flexGrid.BackColorBkg = SystemColors.Control;
            flexGrid.DefaultFont = new Font("Tahoma", 8);
            flexGrid.DisplayRowArrow = true;
            flexGrid.DisplayRowNumber = true;

            Range range;

            for (int i = startImageCol; i <= endImageCol; i++)
            {
                flexGrid.Column(i).TabStop = false;
                flexGrid.Column(i).Width = 20;
            }

            flexGrid.Column(endImageCol + 13).Width = 300;//计划说明
            flexGrid.Column(endImageCol + 1).Width = 120;//本周计划开始时间
            flexGrid.Column(endImageCol + 2).Width = 120;//本周计划结束时间

            flexGrid.Column(endImageCol + 1).Alignment = AlignmentEnum.CenterCenter;
            flexGrid.Column(endImageCol + 2).Alignment = AlignmentEnum.CenterCenter;

            for (int i = 0; i < rows; i++)
            {
                range = flexGrid.Range(i, startImageCol, i, endImageCol);
                range.Merge();

                flexGrid.Cell(i, endImageCol + 3).Text = "天";
            }

            // 加载图片
            flexGrid.Images.Add(Resources.ImageExpend, imageExpand);
            flexGrid.Images.Add(Resources.ImageFold, imageCollapse);

            flexGrid.Cell(0, 1).Text = "任务名称";
            flexGrid.Cell(0, 1).Alignment = AlignmentEnum.CenterCenter;

            flexGrid.Cell(0, endImageCol + 1).Text = "本周计划开始时间";//20
            flexGrid.Cell(0, endImageCol + 2).Text = "本周计划结束时间";//21
            flexGrid.Cell(0, endImageCol + 3).Text = "计划开始-结束时间";//22

            flexGrid.Cell(0, endImageCol + 4).Text = "计划工期";//22
            flexGrid.Cell(0, endImageCol + 5).Text = "工期计量单位";//23
            flexGrid.Cell(0, endImageCol + 6).Text = "周一";//27
            flexGrid.Cell(0, endImageCol + 7).Text = "周二";//27
            flexGrid.Cell(0, endImageCol + 8).Text = "周三";//27
            flexGrid.Cell(0, endImageCol + 9).Text = "周四";//27
            flexGrid.Cell(0, endImageCol + 10).Text = "周五";//27
            flexGrid.Cell(0, endImageCol + 11).Text = "周六";//27
            flexGrid.Cell(0, endImageCol + 12).Text = "周日";//27
            flexGrid.Cell(0, endImageCol + 13).Text = "计划说明";//27
            flexGrid.Cell(0, endImageCol + 14).Text = "实际开始时间";//24
            flexGrid.Cell(0, endImageCol + 15).Text = "实际结束时间";//25
            flexGrid.Cell(0, endImageCol + 16).Text = "实际工期";//26

            flexGrid.Column(endImageCol + 1).CellType = CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 2).CellType = CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 3).AutoFit();
            flexGrid.Column(endImageCol + 14).CellType = CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 15).CellType = CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 16).Mask = MaskEnum.Digital;

            flexGrid.Column(endImageCol + 14).Visible = false;
            flexGrid.Column(endImageCol + 15).Visible = false;
            flexGrid.Column(endImageCol + 16).Visible = false;

            // Refresh
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        private void SetRowBold(int rowIndex, bool isBold)
        {
            for (var i = 0; i < flexGrid.Cols; i++)
            {
                flexGrid.Cell(rowIndex, i).FontBold = isBold;
            }
        }

        private void SetRowFontColor(int rowIndex, Color fontColor)
        {
            for (var i = 0; i < flexGrid.Cols; i++)
            {
                flexGrid.Cell(rowIndex, i).ForeColor = fontColor;
            }
        }

        private void SetRowVisible(int row1, int row2, bool value)
        {
            flexGrid.AutoRedraw = false;
            for (int i = row1; i <= row2; i++)
            {
                flexGrid.Row(i).Visible = value;
            }
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        private void FillFlex()
        {
            detailHashtable.Clear();
            listDtlIds.Clear();

            flexGrid.AutoRedraw = false;
            flexGrid.Rows = 1;
            flexGrid.Column(endImageCol + 3).Locked = true;

            if (weekPlan == null || monthPlan == null)
            {
                return;
            }

            IList list;
            IList weekDetails = model.ProductionManagementSrv.GetWeekChilds(weekPlan);
            if (!chkOnlyWeek.Checked)
            {
                list = model.ProductionManagementSrv.GetWeekChilds(monthPlan);
            }
            else
            {
                list = weekDetails;
            }

            if (list != null && list.Count > 0)
            {
                flexGrid.Rows = list.Count + 1;
                int rowIndex = 1;
                foreach (WeekScheduleDetail detail in list)
                {
                    detail.RowIndex = rowIndex;
                    detailHashtable.Add(detail.Id, detail);
                    listDtlIds.Add(detail.Id);

                    flexGrid.Cell(rowIndex, 0).Tag = detail.Id;
                    if (detail.ChildCount > 0)
                    {
                        flexGrid.Cell(rowIndex, detail.Level).SetImage(imageCollapse);
                    }

                    SetRowBold(rowIndex, detail.ChildCount > 0);

                    Range rangeTemp = flexGrid.Range(rowIndex, detail.Level + 1, rowIndex, endImageCol);
                    rangeTemp.Merge();

                    flexGrid.Cell(rowIndex, detail.Level + 1).Text = detail.GWBSTreeName ?? detail.ProjectName + "总进度计划";

                    //行是否只读
                    flexGrid.Row(rowIndex).Locked = true;


                    //计划开始-结束时间
                    flexGrid.Cell(rowIndex, endImageCol + 3).Text = (detail.PlannedBeginDate == defaultTime ? "" : detail.PlannedBeginDate.ToShortDateString())
                                                                    + "-"
                                                                    +( detail.PlannedEndDate == defaultTime ? "" : detail.PlannedEndDate.ToShortDateString());

                    //本周时间
                    var isInWeekDetails =
                        weekDetails.OfType<WeekScheduleDetail>().Any(a => a.GWBSTreeSysCode == detail.GWBSTreeSysCode);
                    var minDate = detail.PlannedBeginDate < weekPlan.PlannedBeginDate
                                      ? weekPlan.PlannedBeginDate
                                      : detail.PlannedBeginDate;
                    var maxDate = detail.PlannedEndDate < weekPlan.PlannedEndDate
                                      ? detail.PlannedEndDate
                                      : weekPlan.PlannedEndDate;

                    //本周计划开始时间
                    flexGrid.Cell(rowIndex, endImageCol + 1).Text = isInWeekDetails ? minDate.ToShortDateString() : "";

                    //本周计划结束时间
                    flexGrid.Cell(rowIndex, endImageCol + 2).Text = isInWeekDetails ? maxDate.ToShortDateString() : "";
                  

                    //计划工期
                    if (detail.PlannedBeginDate != defaultTime && detail.PlannedEndDate != defaultTime &&
                        detail.PlannedBeginDate <= detail.PlannedEndDate)
                    {
                        if (isInWeekDetails)
                        {
                            flexGrid.Cell(rowIndex, endImageCol + 4).Text =
                                string.Format("{0}/{1}", (maxDate - minDate).Days + 1
                                              , (detail.PlannedEndDate - detail.PlannedBeginDate).Days + 1);
                        }
                        else
                        {
                            flexGrid.Cell(rowIndex, endImageCol + 4).Text =
                                ((detail.PlannedEndDate - detail.PlannedBeginDate).Days + 1).ToString();
                        }
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 4).Text = "";
                    }
                    flexGrid.Cell(rowIndex, endImageCol + 4).Alignment = AlignmentEnum.CenterCenter;

                    //工期计量单位                    
                    flexGrid.Cell(rowIndex, endImageCol + 5).Text = detail.ScheduleUnit;
                    flexGrid.Cell(rowIndex, endImageCol + 5).Alignment = AlignmentEnum.CenterCenter;

                    //星期
                    for (var dy = minDate; isInWeekDetails && dy <= maxDate; dy = dy.AddDays(1))
                    {
                        var dwk = dy.DayOfWeek;
                        var colOffset = 0;
                        if (dwk == 0)
                        {
                            colOffset = 12;
                        }
                        else
                        {
                            colOffset = 5 + (int) dwk;
                        }

                        flexGrid.Cell(rowIndex, endImageCol + colOffset).Text = "√";
                        flexGrid.Cell(rowIndex, endImageCol + colOffset).Alignment = AlignmentEnum.CenterCenter;
                        flexGrid.Cell(rowIndex, endImageCol + colOffset).BackColor = SystemColors.Control;
                    }

                    //计划说明
                    flexGrid.Cell(rowIndex, endImageCol + 13).Text = detail.Descript; //"计划说明";//27
                    flexGrid.Cell(rowIndex, endImageCol + 13).Locked = false;

                    //实际开始时间
                    if (detail.ActualBeginDate == defaultTime)
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 14).Text = "";
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 14).Text = detail.ActualBeginDate.ToShortDateString();
                    }

                    //实际结束时间
                    if (detail.ActualEndDate == defaultTime)
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 15).Text = "";
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 15).Text = detail.ActualEndDate.ToShortDateString();
                    }

                    //实际工期
                    flexGrid.Cell(rowIndex, endImageCol + 16).Text =
                        detail.ActualDuration == 0 ? "" : detail.ActualDuration.ToString();

                    if (isInWeekDetails)
                    {
                        SetRowFontColor(rowIndex, Color.Black);
                    }
                    else
                    {
                        SetRowFontColor(rowIndex, Color.DimGray);
                    }

                    rowIndex = rowIndex + 1;
                }
            }

            //1-19列的背景色
            Range range = flexGrid.Range(1, startImageCol, flexGrid.Rows - 1, endImageCol);
            if (range != null)
            {
                range.Alignment = AlignmentEnum.LeftCenter;
                range.BackColor = SystemColors.Control;
            }

            //设置计划实际信息列的背景色
            range = flexGrid.Range(1, endImageCol + 14, flexGrid.Rows - 1, endImageCol + 16);
            if (range != null)
            {
                range.Alignment = AlignmentEnum.LeftCenter;
                range.BackColor = SystemColors.Control;
            }

            flexGrid.Column(endImageCol + 3).AutoFit();
            for (int i = 6; i <= 12; i++)
            {
                flexGrid.Column(endImageCol + i).TabStop = false;
                flexGrid.Column(endImageCol + i).AutoFit();
            }

            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();

            lbRowCount.Text = string.Format("共计 {0} 行", flexGrid.Rows);
        }

        private string CodingVersion()
        {
            var schCount = 0;
            var startChar = (int)'A';
            var ver = string.Format("周进-[{0}]-{2}/{3}-{1}", DateTime.Now.Year, (char) (startChar + schCount),
                                    dtpBeginDate.Value.ToString("MM.dd"), dtpEndDate.Value.ToString("MM.dd"));

            var list = GetProductionSchedules(EnumExecScheduleType.周进度计划, monthPlan.Id).OfType<WeekScheduleMaster>().ToList();
            while (list != null && list.Exists(p => p.PlanName.Equals(ver)))
            {
                schCount++;
                ver = string.Format("周进-[{0}]-{2}/{3}-{1}", DateTime.Now.Year, (char)(startChar + schCount),
                                    dtpBeginDate.Value.ToString("MM.dd"), dtpEndDate.Value.ToString("MM.dd"));
            }

            return ver;
        }

        private WeekScheduleMaster NewWeekPlan()
        {
            var wkPlan = new WeekScheduleMaster();
            wkPlan.ProjectId = projectInfo.Id;
            wkPlan.ProjectName = projectInfo.Name;
            wkPlan.ExecScheduleType = EnumExecScheduleType.周进度计划;
            wkPlan.CreateDate = model.ProductionManagementSrv.GetServerTime();
            wkPlan.HandlePerson = ConstObject.LoginPersonInfo;
            wkPlan.HandlePersonName = ConstObject.LoginPersonInfo.Name;
            wkPlan.CreatePerson = ConstObject.LoginPersonInfo;
            wkPlan.CreatePersonName = ConstObject.LoginPersonInfo.Name;
            wkPlan.HandleOrg = ConstObject.TheOperationOrg;
            wkPlan.HandlePersonSyscode = ConstObject.TheOperationOrg.SysCode;
            wkPlan.DocState = monthPlan.DocState;
            wkPlan.ForwardBillCode = monthPlan.Code;
            wkPlan.ForwardBillId = monthPlan.Id;

            return wkPlan;
        }

        private void CreateNew(string newVer,ref string msg)
        {
            monthPlan = model.ProductionManagementSrv.GetWeekScheduleMasterById(monthPlan.Id);
            if (monthPlan == null)
            {
                return;
            }

            var bgDate = dtpBeginDate.Value.Date;
            var endDate = dtpEndDate.Value.Date;

            //var details = from d in monthPlan.Details
            //              where d.NodeType == NodeType.LeafNode
            //                    && d.State == DocumentState.InExecute
            //                    && ((d.PlannedBeginDate >= bgDate && d.PlannedBeginDate <= endDate)
            //                    || (d.PlannedEndDate >= bgDate && d.PlannedEndDate <= endDate))
            //              select d;

            var details = from d in monthPlan.Details
                          where d.NodeType == VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode
                                && d.State == DocumentState.InExecute
                                && (
                                        (
                                            (d.PlannedBeginDate <= bgDate && d.PlannedEndDate >= bgDate)
                                            || (d.PlannedBeginDate <= endDate && d.PlannedEndDate >= endDate)
                                            || (d.PlannedBeginDate > bgDate && d.PlannedEndDate < endDate)
                                        ) 
                                        ||
                                        (d.PlannedEndDate < bgDate && d.TaskCompletedPercent < 100)
                                    )

                          select d;

            if (details.Count() == 0)
            {
                msg = "创建新版本周计划出错：您所设置的时间不包含总进度计划的已经审核的任务节点，请检查！";
                return;
            }

            FlashScreen.Show("正在生成周计划，请稍候...");
            var pNodes = new List<WeekScheduleDetail>();
            foreach (var dt in details)
            {
                var pNode = dt.ParentNode;
                while (pNode != null)
                {
                    if (!pNodes.Contains(pNode))
                    {
                        pNodes.Add(pNode);
                    }

                    pNode = pNode.ParentNode;
                }
            }

            pNodes.AddRange(details);

            var wkPlan = NewWeekPlan();
            wkPlan.PlanName = newVer;
            wkPlan.PlannedBeginDate = bgDate;
            wkPlan.PlannedEndDate = endDate;
            wkPlan.Descript = txtPlanDesc.Text.Trim();

            weekPlan = model.ProductionManagementSrv.CreateSubSchdulePlan(wkPlan, pNodes);

            InitWeekPlan(monthPlan.Id);

            FlashScreen.Close();

            cmbWeekPlan.Text = newVer;
        }

        public override bool DeleteView()
        {
            try
            {
                var res = model.ProductionManagementSrv.DeleteWeekPlan(weekPlan);

                if (res==-1)
                {
                    MessageBox.Show("删除失败：没有获取到周计划数据");
                    return false;
                }
                else if(res==-2)
                {
                    MessageBox.Show("删除失败：该周计划的任务单已经打印，不允许删除");
                    return false;
                }
                    
                LogData log = new LogData();
                log.BillId = weekPlan.Id;
                log.BillType = "周进度计划";
                log.Code = "";
                log.OperType = "删除";
                log.Descript = weekPlan.Descript;
                log.OperPerson = ConstObject.LoginPersonInfo.Name;
                log.ProjectName = weekPlan.ProjectName;
                StaticMethod.InsertLogData(log);

                weekPlan = null;

                InitWeekPlan(monthPlan.Id);

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("删除失败，异常信息：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        private void cmbMonthPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMonthPlan.SelectedIndex == 0)
            {
                monthPlan = null;
                cmbWeekPlan.DataSource = null;
                flexGrid.Rows = 1;
                dgDocumentDetail.Rows.Clear();
                dgDocumentMast.Rows.Clear();
            }
            else
            {
                monthPlan = cmbMonthPlan.SelectedItem as WeekScheduleMaster;
                if (monthPlan != null)
                {
                    InitWeekPlan(monthPlan.Id);
                }
            }
        }

        private void cmbWeekPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbWeekPlan.SelectedIndex == 0)
            {
                flexGrid.Rows = 1;
                dgDocumentDetail.Rows.Clear();
                dgDocumentMast.Rows.Clear();
            }
            else
            {
                weekPlan = cmbWeekPlan.SelectedItem as WeekScheduleMaster;
                if (weekPlan != null)
                {
                    dtpBeginDate.Value = weekPlan.PlannedBeginDate;
                    dtpEndDate.Value = weekPlan.PlannedEndDate;
                    txtPlanDesc.Text = weekPlan.Descript;

                    FillFlex();

                    FillDoc();
                }
            }
        }

        private void chkOnlyWeek_CheckedChanged(object sender, EventArgs e)
        {
            FillFlex();
        }

        private void btnDeletePlan_Click(object sender, EventArgs e)
        {
            if (weekPlan == null)
            {
                MessageBox.Show("请选择一个计划!");
                return;
            }

            if (MessageBox.Show("您确认要删除当前版本的周计划？", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                FlashScreen.Show("正在删除周进度计划，请稍候...");

                DeleteView();

                FlashScreen.Close();
            }
        }

        private void btnCreateNewVersion_Click(object sender, EventArgs e)
        {
            if (monthPlan == null || string.IsNullOrEmpty(monthPlan.Id))
            {
                MessageBox.Show("请先选择总体进度计划！");
                return;
            }

            var mes = string.Format("您确定创建{0}-{1}的周进度计划？",
                                    dtpBeginDate.Value.ToShortDateString(), dtpEndDate.Value.ToShortDateString());
            if (MessageBox.Show(mes, "生成确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            VSchduleVersionEdit frm = new VSchduleVersionEdit(CodingVersion());
            var list = cmbWeekPlan.DataSource as List<WeekScheduleMaster>;
            if (list != null)
            {
                list.ForEach(v => frm.ListHasVersions.Add(v.PlanName));
            }

            if (frm.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string msg = "";
            CreateNew(frm.NewVersion,ref msg);
            if (msg != "")
            {
                MessageBox.Show(msg);
            }
        }

        private void dtpBeginDate_ValueChanged(object sender, EventArgs e)
        {
            dtpEndDate.Value = dtpBeginDate.Value.AddDays(6);
        }

        void AddDgDocumentMastInfo(DocumentMaster m)
        {
            int rowIndex = dgDocumentMast.Rows.Add();
            AddDgDocumentMastInfo(m, rowIndex);
        }

        void AddDgDocumentMastInfo(DocumentMaster m, int rowIndex)
        {
            dgDocumentMast[colDocumentName.Name, rowIndex].Value = m.Name;
            dgDocumentMast[colCreateTime.Name, rowIndex].Value = m.CreateTime;
            dgDocumentMast[colDocumentCode.Name, rowIndex].Value = m.Code;
            dgDocumentMast[colDocumentInforType.Name, rowIndex].Value = m.DocType.ToString();
            dgDocumentMast[colDocumentState.Name, rowIndex].Value = ClientUtil.GetDocStateName(m.State);
            dgDocumentMast[colOwnerName.Name, rowIndex].Value = m.OwnerName;
            dgDocumentMast.Rows[rowIndex].Tag = m;
        }

        void AddDgDocumentDetailInfo(DocumentDetail d)
        {
            int rowIndex = dgDocumentDetail.Rows.Add();
            AddDgDocumentDetailInfo(d, rowIndex);
        }

        void AddDgDocumentDetailInfo(DocumentDetail d, int rowIndex)
        {
            dgDocumentDetail[FileName.Name, rowIndex].Value = d.FileName;
            //dgDocumentDetail[FileExtension.Name, rowIndex].Value = d.ExtendName;
            //dgDocumentDetail[FilePath.Name, rowIndex].Value = d.FilePartPath;
            dgDocumentDetail.Rows[rowIndex].Tag = d;
        }

        //加载文档数据
        void FillDoc()
        {
            dgDocumentMast.Rows.Clear();
            dgDocumentDetail.Rows.Clear();

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", weekPlan.Id));
            IList listObj = model.ObjectQuery(typeof(ProObjectRelaDocument), oq);
            if (listObj != null && listObj.Count > 0)
            {
                oq.Criterions.Clear();
                Disjunction dis = new Disjunction();
                foreach (ProObjectRelaDocument obj in listObj)
                {
                    dis.Add(Expression.Eq("Id", obj.DocumentGUID));
                }
                oq.AddCriterion(dis);
                oq.AddFetchMode("ListFiles", NHibernate.FetchMode.Eager);
                IList docList = model.ObjectQuery(typeof(DocumentMaster), oq);
                if (docList != null && docList.Count > 0)
                {
                    foreach (DocumentMaster m in docList)
                    {
                        AddDgDocumentMastInfo(m);
                    }
                    dgDocumentMast.CurrentCell = dgDocumentMast[2, 0];
                }
            }
        }

        void dgDocumentMast_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgDocumentDetail.Rows.Clear();
            DocumentMaster docMaster = dgDocumentMast.Rows[e.RowIndex].Tag as DocumentMaster;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", docMaster.Id));
            oq.AddFetchMode("TheFileCabinet", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof(DocumentDetail), oq);
            if (list != null && list.Count > 0)
            {
                foreach (DocumentDetail docDetail in list)
                {
                    AddDgDocumentDetailInfo(docDetail);
                }
            }
        }

        //查找下一个
        void btnFindNext_Click(object sender, EventArgs e)
        {
            var findKey = txtFindKey.Text.Trim();
            if (string.IsNullOrEmpty(findKey))
            {
                MessageBox.Show("请输入查找关键字！");
                return;
            }

            if (weekPlan == null)
            {
                MessageBox.Show("当前没有要操作的计划！");
                return;
            }
            else if (flexGrid.Rows <= 2)
            {
                MessageBox.Show("当前计划尚没有计划明细！");
                return;
            }

            var findList = from dt in detailHashtable.Values.OfType<WeekScheduleDetail>()
                           where dt.GWBSTreeName != null && dt.GWBSTreeName.Contains(findKey)
                           orderby dt.RowIndex
                           select dt;
            if (findList.Count() == 0)
            {
                MessageBox.Show("没有找到含关键字【" + findKey + "】的行");
                return;
            }

            var findItem = findList.First();
            if (findDetail != null)
            {
                findItem = findList.FirstOrDefault(a => a.RowIndex > findDetail.RowIndex);

                flexGrid.Cell(findDetail.RowIndex, findDetail.Level + 1).BackColor = SystemColors.Control;
                flexGrid.Cell(findDetail.RowIndex, findDetail.Level + 1).ForeColor = Color.Black;
            }

            if (findItem == null)
            {
                findItem = findList.First();
            }

            findDetail = findItem;

            flexGrid.Cell(findItem.RowIndex, findItem.Level + 1).BackColor = Color.Green;
            flexGrid.Cell(findItem.RowIndex, findItem.Level + 1).ForeColor = Color.White;

            lbFindCount.Text = string.Format("共查找到 {0} 个，当前在 {1} 行", findList.Count(), findItem.RowIndex);
        }

        //下载
        void btnDownLoadDocument_Click(object sender, EventArgs e)
        {
            IList downList = new ArrayList();
            foreach (DataGridViewRow row in dgDocumentDetail.Rows)
            {
                if ((bool)row.Cells[FileSelect.Name].EditedFormattedValue)
                {
                    DocumentDetail dtl = row.Tag as DocumentDetail;
                    downList.Add(dtl);
                }
            }
            if (downList != null && downList.Count > 0)
            {
                VDocumentPublicDownload frm = new VDocumentPublicDownload(downList);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("请勾选要下载的文件！");
            }
        }

        //预览
        void btnOpenDocument_Click(object sender, EventArgs e)
        {
            IList list = new ArrayList();
            foreach (DataGridViewRow row in dgDocumentDetail.Rows)
            {
                if ((bool)row.Cells[FileSelect.Name].EditedFormattedValue)
                {
                    DocumentDetail dtl = row.Tag as DocumentDetail;
                    list.Add(dtl);
                }
            }

            if (list.Count == 0)
            {
                MessageBox.Show("勾选文件才能预览，请选择要预览的文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {

                List<string> listFileFullPaths = new List<string>();
                string fileFullPath1 = AppDomain.CurrentDomain.BaseDirectory + "TempFilePreview\\" + Guid.NewGuid().ToString().Replace("-", "");
                foreach (DocumentDetail docFile in list)
                {
                    //DocumentDetail docFile = dgDocumentDetail.SelectedRows[0].Tag as DocumentDetail;
                    if (!Directory.Exists(fileFullPath1))
                        Directory.CreateDirectory(fileFullPath1);

                    string tempFileFullPath = fileFullPath1 + @"\\" + docFile.FileName;

                    //string address = "http://10.70.18.203//TestFile//Files//0001//0001//0001.0001.000027V_特效.jpg";
                    string baseAddress = @docFile.TheFileCabinet.TransportProtocal.ToString().ToLower() + "://" + docFile.TheFileCabinet.ServerName + "/" + docFile.TheFileCabinet.Path + "/";

                    string address = baseAddress + docFile.FilePartPath;
                    UtilityClass.WebClientObj.DownloadFile(address, tempFileFullPath);
                    listFileFullPaths.Add(tempFileFullPath);
                }
                foreach (string fileFullPath in listFileFullPaths)
                {
                    FileInfo file = new FileInfo(fileFullPath);

                    //定义一个ProcessStartInfo实例
                    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                    //设置启动进程的初始目录
                    info.WorkingDirectory = file.DirectoryName;//Application.StartupPath; 
                    //设置启动进程的应用程序或文档名
                    info.FileName = file.Name;
                    //设置启动进程的参数
                    info.Arguments = "";
                    //启动由包含进程启动信息的进程资源
                    try
                    {
                        System.Diagnostics.Process.Start(info);
                    }
                    catch (System.ComponentModel.Win32Exception we)
                    {
                        MessageBox.Show(this, we.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("未将对象引用设置到对象的实例") > -1)
                {
                    MessageBox.Show("操作失败，不存在的文档对象！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //反选
        void lnkCheckAllNot_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgDocumentDetail.Rows)
            {
                row.Cells[FileSelect.Name].Value = false;
            }
        }

        //全选
        void lnkCheckAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgDocumentDetail.Rows)
            {
                row.Cells[FileSelect.Name].Value = true;
            }
        }

        //折叠、展开
        void flexGrid_Click(object Sender, EventArgs e)
        {
            if (flexGrid.ActiveCell.Col > endImageCol)
            {
                return;
            }

            flexGrid.AutoRedraw = false;
            bool isVisble = false;
            if (flexGrid.ActiveCell.ImageKey == imageExpand)
            {
                flexGrid.ActiveCell.SetImage(imageCollapse);
                isVisble = true;
            }
            else if (flexGrid.ActiveCell.ImageKey == imageCollapse)
            {
                flexGrid.ActiveCell.SetImage(imageExpand);
                isVisble = false;
            }
            else
            {
                flexGrid.AutoRedraw = true;
                return;
            }

            int activeRowIndex = flexGrid.ActiveCell.Row;
            string detailId = flexGrid.Cell(activeRowIndex, 0).Tag;
            int childs = 0;
            if (detailHashtable.ContainsKey(detailId))
            {
                var detail = detailHashtable[detailId] as WeekScheduleDetail;
                childs = (from a in detailHashtable.Values.OfType<WeekScheduleDetail>()
                          where a.SysCode.StartsWith(detail.SysCode) && a.Id != detailId
                          select a
                         ).Count();
            }

            SetRowVisible(activeRowIndex + 1, activeRowIndex + childs, isVisble);

            flexGrid.AutoRedraw = true;
        }

        //导出
        void btnExportToMPP_Click(object sender, EventArgs e)
        {
            if (weekPlan == null)
            {
                MessageBox.Show("当前没有要导出的计划!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "项目 (*.MPP)|*.MPP";
            sfd.RestoreDirectory = true;
            sfd.FileName = "总进度计划_" + weekPlan.PlanName + "_" + string.Format("{0:yyyy年MM月dd日HH点mm分}", DateTime.Now);
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string fileName = sfd.FileName;
                if (System.IO.File.Exists(fileName))
                {
                    IList list = model.ProductionManagementSrv.GetWeekChilds(weekPlan);
                    MSProjectUtil.UpdateProject(fileName, list, listDtlIds, this.Handle);
                }
                else
                {
                    IList list = model.ProductionManagementSrv.GetWeekChilds(weekPlan);
                    MSProjectUtil.CreateMPP(fileName, list, listDtlIds, this.Handle);
                }
            }


        }

        //生成任务单
        private void btnCreateBill_Click(object sender, EventArgs e)
        {
            if (weekPlan == null)
            {
                MessageBox.Show("请选择要生成任务单的周计划");
                return;
            }

            var res = model.ProductionManagementSrv.CreateAssignWorkerOrderByPlan(weekPlan);
            if (res == -1)
            {
                MessageBox.Show("生成失败：未能获取到周计划数据");
            }
            else if (res == -2)
            {
                MessageBox.Show("生成失败：该周计划已经生成任务单");
            }
            else if (res == 1)
            {
                MessageBox.Show("生成成功");
            }
            else
            {
                MessageBox.Show("生成失败");
            }
        }

    }
}
