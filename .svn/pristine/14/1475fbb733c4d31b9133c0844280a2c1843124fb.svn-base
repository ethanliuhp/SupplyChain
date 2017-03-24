using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using Application.Business.Erp.SupplyChain.Client.Properties;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using FlexCell;
using IRPServiceModel.Basic;
using IRPServiceModel.Domain.Document;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Core;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI
{
    public partial class UcScheduleDetailViewer : UserControl
    {
        private MProductionMng model = null;
        private WeekScheduleMaster CurBillMaster;
        private Hashtable detailHashtable = new Hashtable();
        private WeekScheduleDetail findDetail;

        private const int startImageCol = 1, endImageCol = 19;
        private const string imageExpand = "imageExpand";
        private const string imageCollapse = "imageCollapse";
        private DateTime defaultTime = new DateTime(1900, 1, 1);
        private IList<WeekScheduleDetail> showDetail = null;
        private WSDOrderSet wsdorder = WSDOrderSet.WSD;
        private List<string> listDtlIds = new List<string>();

        public UcScheduleDetailViewer()
        {
            InitializeComponent();

            InitEvents();
        }

  

        public void LoadScheduleDetails(WeekScheduleMaster schedule)
        {
            CurBillMaster = schedule;
            InitFlexGrid(1);
            FlashScreen.Show("正在加载进度计划，请稍稍候...");
            
            if (model == null)
            {
                model = new MProductionMng();
            }

            LoadDetail();

            FillFlexAndDoc();

            FlashScreen.Close();
        }

        private void FillFlexAndDoc()
        {
            FillFlex();

            FillDoc(); 
        }

        private void LoadDetail()
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis2 = new Disjunction();
            dis2.Add(Expression.And(Expression.Not(Expression.Eq("PlannedBeginDate", defaultTime)), Expression.Not(Expression.Eq("PlannedEndDate", defaultTime))));
            oq.AddCriterion(dis2);
            oq.AddCriterion(Expression.Or(Expression.Eq("State", DocumentState.InAudit), Expression.Eq("Level", 1)));
            oq.AddCriterion(Expression.Eq("Master.Id", CurBillMaster.Id));

            showDetail = model.ProductionManagementSrv.GetShowWeekScheduleDetails(oq).OfType<WeekScheduleDetail>().ToList<WeekScheduleDetail>();

            MaintainShowDetail();
            SetOrderShowDetail_Load();
            FillFlex();
        }

        private void SetOrderShowDetail_Load()
        {
            if (showDetail == null || showDetail.Count == 0)
                return;
            List<WeekScheduleDetail> temp_showDetail = new List<WeekScheduleDetail>();
            foreach (WeekScheduleDetail item in showDetail)
            {
                temp_showDetail.Add(item);
            }
            WeekScheduleDetail detail_root = temp_showDetail.Find(a => (a.ParentNode == null && a.Level == 1));
            if (detail_root == null)
                return;
            showDetail.Clear();
            showDetail.Add(detail_root);

            var queryDtl = from d in temp_showDetail
                           orderby d.OrderNo ascending
                           orderby d.Level ascending
                           select d;

            foreach (WeekScheduleDetail item in queryDtl)
            {
                ArrayList list = new ArrayList();
                getWeekChildDtl_Load(item, temp_showDetail, ref list);
                foreach (WeekScheduleDetail lst in list)
                {
                    if (!showDetailContains(lst))
                    {
                        showDetail.Add(lst);
                    }
                }
            }

        }

        private bool showDetailContains(WeekScheduleDetail item)
        {
            var details = from p in showDetail
                          where p.Id == item.Id
                          select p;

            if (details != null && details.Count() > 0)
                return true;
            else
                return false;

        }

        private void getWeekChildDtl_Load(WeekScheduleDetail parentDtl, List<WeekScheduleDetail> listDtl, ref ArrayList listResult)
        {

            var queryDtl = from d in listDtl.OfType<WeekScheduleDetail>()
                           where (d.ParentNode != null && d.ParentNode.Id == parentDtl.Id)
                           orderby d.OrderNo ascending
                           orderby d.Level ascending
                           select d;
            parentDtl.ChildCount = queryDtl.Count();

            int i = 1;
            foreach (WeekScheduleDetail dtl in queryDtl)
            {
                if (!listResult.Contains(dtl))
                {
                    dtl.WSDOrderNo = i;
                    listResult.Add(dtl);
                    i++;
                    getWeekChildDtl_Load(dtl, listDtl, ref listResult);
                }
            }
        }

        private void MaintainShowDetail()
        {
            WeekScheduleDetail rootnode = GetScheduleDetailRootNode();

            var pNodes = new List<WeekScheduleDetail>();
            foreach (var dt in showDetail)
            {
                if (dt.ParentNode == null)
                    continue;
                var pNode = FindWeekScheduleDetailById(dt.ParentNode.Id);
                while (pNode != null)
                {
                    if (!showDetailContains(pNode))
                    {
                        pNodes.Add(pNode);
                    }

                    if (pNode.Id == rootnode.Id)
                        pNode = null;
                    else
                    {
                        //pNode = pNode.ParentNode;
                        string strid = pNode.ParentNode.Id;
                        WeekScheduleDetail pNode_parent = FindWeekScheduleDetailById(strid);

                        pNode = pNode_parent;
                    }
                }
            }

            foreach (var item in pNodes)
            {
                if (!showDetailContains(item))
                {
                    if (wsdorder == WSDOrderSet.WSD)
                        item.WSDOrderNo = GetWSDOrderNoMax(item, showDetail.ToList<WeekScheduleDetail>());
                    showDetail.Add(item);
                }
            }
        }

        private WeekScheduleDetail GetScheduleDetailRootNode()
        {
            if (CurBillMaster == null)
            {
                return null;
            }

            var oq = new ObjectQuery();
            oq.Criterions.Clear();
            oq.AddCriterion(Expression.Eq("Master.Id", CurBillMaster.Id));
            oq.AddCriterion(Expression.Eq("Level", 1));

            var listDtl = model.ProductionManagementSrv.ObjectQuery(typeof(WeekScheduleDetail), oq);
            if (listDtl != null && listDtl.Count > 0)
            {
                return listDtl[0] as WeekScheduleDetail;
            }

            return null;
        }

        private WeekScheduleDetail FindWeekScheduleDetailById(string strid)
        {
            var details = from p in CurBillMaster.Details
                          where p.Id == strid
                          select p;

            if (details != null && details.Count() > 0)
                return details.ElementAt(0);
            else
                return null;
        }

        private int GetWSDOrderNoMax(WeekScheduleDetail detail, List<WeekScheduleDetail> list_details)
        {
            if (detail.ParentNode == null)
                return 1;

            if (list_details == null || list_details.Count < 1)
                return 1;

            WeekScheduleDetail ParentWsd = detail.ParentNode;

            int level = ParentWsd.Level + 1;

            var list = from p in list_details
                       where p.SysCode.StartsWith(ParentWsd.SysCode) && p.Level == (ParentWsd.Level + 1)
                       select p;

            if (list == null || list.Count() < 1)
                return 1;

            var listwhere = list.Where(p => p.WSDOrderNo != null);
            if (listwhere == null || listwhere.Count() < 1)
                return 1;
            int cl = (int)listwhere.Max(p => p.WSDOrderNo);

            return cl + 1;
        }

        private void InitEvents()
        {
            btnFindNext.Click += btnFindNext_Click;
            btnDownLoadDocument.Click += btnDownLoadDocument_Click;
            btnOpenDocument.Click += btnOpenDocument_Click;
            lnkCheckAll.Click += new EventHandler(lnkCheckAll_Click);
            lnkCheckAllNot.Click += new EventHandler(lnkCheckAllNot_Click);
            dgDocumentMast.CellClick += new DataGridViewCellEventHandler(dgDocumentMast_CellClick);

            flexGrid.Click += new FlexCell.Grid.ClickEventHandler(flexGrid_Click);
            flexGrid.CellChange += new Grid.CellChangeEventHandler(flexGrid_CellChange);
            this.cbxAllSel.CheckedChanged += new EventHandler(cbxAllSel_CheckedChanged);
        }

        void flexGrid_CellChange(object Sender, Grid.CellChangeEventArgs e)
        {
            if (e.Row <= 0 || e.Col < endImageCol + 1 || e.Col > endImageCol + 1)
            {
                return;
            }
            var detailId = flexGrid.Cell(e.Row, 0).Tag;
            if (!detailHashtable.ContainsKey(detailId))
                return;
            var detail = detailHashtable[detailId] as WeekScheduleDetail;


            detail.IsSubmitCheck = ClientUtil.ToBool(flexGrid.Cell(e.Row, endImageCol + 1).Text);
        }

        void cbxAllSel_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var item in showDetail)
            {
                if (item.State == DocumentState.InAudit)
                    item.IsSubmitCheck = this.cbxAllSel.Checked;
            }
            FillFlex();
        }

        private void InitFlexGrid(int rows)
        {
            LockFlexGridColumn(false);
            flexGrid.AutoRedraw = false;
            flexGrid.Column(0).Visible = true;

            flexGrid.Rows = rows;
            flexGrid.Cols = 29;//其中0列隐藏 1-19 为放置图片列 20-24为数据列

            flexGrid.SelectionMode = SelectionModeEnum.Free;
            flexGrid.ExtendLastCol = true;
            flexGrid.DisplayFocusRect = true;
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
            flexGrid.Column(endImageCol + 4).Width = 60;
            flexGrid.Column(endImageCol + 5).Width = 60;
            flexGrid.Column(endImageCol + 6).Width = 300;//计划说明
            flexGrid.Column(endImageCol + 7).Width = 300;//计划说明

            for (int i = 0; i < rows; i++)
            {
                range = flexGrid.Range(i, startImageCol, i, endImageCol);
                range.Merge();

                flexGrid.Cell(i, endImageCol + 3).Text = "天";
            }

            // 加载图片
            flexGrid.Images.Add(Resources.ImageExpend, imageExpand);
            flexGrid.Images.Add(Resources.ImageFold, imageCollapse);

            //flexGrid.Cell(0, 1).CellType = CellTypeEnum.CheckBox;
            //flexGrid.Cell(0, 1).Text = "选择";
            //flexGrid.Cell(0, 1).Alignment = AlignmentEnum.CenterCenter;


            flexGrid.Cell(0, 1).Text = "任务名称";
            flexGrid.Cell(0, 1).Alignment = AlignmentEnum.CenterCenter;

            flexGrid.Cell(0, endImageCol + 1).Text = "选择";
            flexGrid.Cell(0, endImageCol + 2).Text = "计划开始时间";//20
            flexGrid.Cell(0, endImageCol + 3).Text = "计划结束时间";//21
            flexGrid.Cell(0, endImageCol + 4).Text = "计划工期";//22
            flexGrid.Cell(0, endImageCol + 5).Text = "工期单位";//23
            flexGrid.Cell(0, endImageCol + 6).Text = "前置任务";//27
            flexGrid.Cell(0, endImageCol + 7).Text = "计划说明";//28
            flexGrid.Cell(0, endImageCol + 8).Text = "提交人";//24
            flexGrid.Cell(0, endImageCol + 9).Text = "提交时间";//25


            flexGrid.Column(endImageCol + 1).CellType = CellTypeEnum.CheckBox;
            flexGrid.Column(endImageCol + 2).CellType = CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 3).CellType = CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 4).Mask = MaskEnum.Digital;
            flexGrid.Column(endImageCol + 5).CellType = CellTypeEnum.TextBox;
            flexGrid.Column(endImageCol + 6).CellType = CellTypeEnum.TextBox;
            flexGrid.Column(endImageCol + 7).CellType = CellTypeEnum.TextBox;
            flexGrid.Column(endImageCol + 8).CellType = CellTypeEnum.TextBox;
            flexGrid.Column(endImageCol + 9).CellType = CellTypeEnum.Calendar;


            LockFlexGridColumn(true);
            // Refresh
            flexGrid.AutoRedraw = true;
            //flexGrid.VerticalScroll.Visible = true;
            flexGrid.Refresh();
        }

        private void FillFlex()
        {
            LockFlexGridColumn(false);
            this.flexGrid.CellChange -= new Grid.CellChangeEventHandler(flexGrid_CellChange);
            detailHashtable.Clear();

            flexGrid.AutoRedraw = false;
            flexGrid.Rows = 1;

            SetChildCountShowDetail();
            SetOrderShowDetail();
            IList list = showDetail.ToList();

            if (list != null && list.Count > 0)
            {
                flexGrid.Rows = list.Count + 1;
                int rowIndex = 1;
                listDtlIds.Clear();
                foreach (WeekScheduleDetail detail in list)
                {
                    detail.RowIndex = rowIndex;

                    listDtlIds.Add(detail.Id);
                    detailHashtable.Add(detail.Id, detail);

                    flexGrid.Cell(rowIndex, 0).Tag = detail.Id;

                    if (detail.ChildCount > 0)
                    {
                        flexGrid.Cell(rowIndex, detail.Level).SetImage(imageCollapse);
                    }

                    flexGrid.Cell(rowIndex, endImageCol + 4).Tag = detail.GWBSTreeSysCode;
                    SetRowBold(rowIndex, detail.ChildCount > 0);

                    Range rangeTemp = flexGrid.Range(rowIndex, detail.Level + 1, rowIndex, endImageCol);
                    rangeTemp.Merge();

                    flexGrid.Cell(rowIndex, detail.Level + 1).Text = (detail.GWBSTreeName) + (detail.IsFixed == 1 ? "【合】" : "") + (detail.ProductionCuringNode ? "【固】" : "");
                    //当前行任务名称所在列序号
                    flexGrid.Cell(rowIndex, endImageCol + 5).Tag = (detail.Level + 1).ToString();

                    ////行是否只读
                    //flexGrid.Row(rowIndex).Locked = detail.State != DocumentState.Edit;

                    if (detail.State != DocumentState.InAudit)
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 1).CellType = CellTypeEnum.TextBox;
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 1).CellType = CellTypeEnum.CheckBox;
                        flexGrid.Cell(rowIndex, endImageCol + 1).Text = detail.IsSubmitCheck.ToString();

                    }

                    //计划开始时间
                    if (detail.PlannedBeginDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 2).Text = ""; //"计划开始时间";//20
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 2).Text = detail.PlannedBeginDate.ToShortDateString(); //"计划开始时间";//20
                    }

                    //计划结束时间
                    if (detail.PlannedEndDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 3).Text = ""; //"计划结束时间";//21
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 3).Text = detail.PlannedEndDate.ToShortDateString();
                    }

                    #region 将计算甩给 flexGrid_CellChanged 时间处理，这里只对工期赋值
                    if (detail.PlannedDuration == (decimal)0)
                        flexGrid.Cell(rowIndex, endImageCol + 4).Text = "";
                    else
                        flexGrid.Cell(rowIndex, endImageCol + 4).Text = ClientUtil.ToString(detail.PlannedDuration);

                    #endregion

                    flexGrid.Cell(rowIndex, endImageCol + 5).Alignment = AlignmentEnum.CenterCenter;

                    //工期计量单位                    
                    flexGrid.Cell(rowIndex, endImageCol + 5).Text = detail.ScheduleUnit;
                    flexGrid.Cell(rowIndex, endImageCol + 5).Alignment = AlignmentEnum.CenterCenter;

                    //前置任务
                    flexGrid.Cell(rowIndex, endImageCol + 6).Text = GetFrontTask(detail, list); //"前置任务";//27


                    //计划说明
                    flexGrid.Cell(rowIndex, endImageCol + 7).Text = detail.Descript; //"计划说明";//28
                    flexGrid.Cell(rowIndex, endImageCol + 7).Locked = true;

                    flexGrid.Cell(rowIndex, endImageCol + 8).Text = detail.SubmitPersonName ;
                    flexGrid.Cell(rowIndex, endImageCol + 8).Locked = true;

                    flexGrid.Cell(rowIndex, endImageCol + 9).Text = ConvertDatetimeToString(detail.SubmitDate);
                    flexGrid.Cell(rowIndex, endImageCol + 9).Locked = true;

                    SetflexGridRowColor(rowIndex, detail);

                    rowIndex = rowIndex + 1;
                }
                this.lbRowCount.Text = string.Format("共计 {0} 行", rowIndex -1);
            }

            //设置计划实际信息列的背景色
            Range range = flexGrid.Range(1, endImageCol + 7, flexGrid.Rows - 1, endImageCol + 9);
            if (range != null)
            {
                range.Alignment = AlignmentEnum.LeftCenter;
                range.BackColor = SystemColors.Control;
            }

            range = flexGrid.Range(1, endImageCol + 5, flexGrid.Rows - 1, endImageCol + 5);
            if (range != null)
            {
                range.Alignment = AlignmentEnum.LeftCenter;
                range.BackColor = SystemColors.Control;
            }

            foreach (WeekScheduleDetail item in list)
            {
                MaintainExpandCollapse(item);
            }
            LockFlexGridColumn(true);
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
            
            this.flexGrid.CellChange += new Grid.CellChangeEventHandler(flexGrid_CellChange);
        }
        private string ConvertDatetimeToString(DateTime? dt)
        {
            if (dt == null || dt == defaultTime || dt == new DateTime(1,1,1))
                return "";
            else
                return ((DateTime)dt).ToShortDateString();
        }
        private void SetOrderShowDetail()
        {
            if (showDetail == null || showDetail.Count == 0)
                return;
            List<WeekScheduleDetail> temp_showDetail = new List<WeekScheduleDetail>();
            foreach (WeekScheduleDetail item in showDetail)
            {
                temp_showDetail.Add(item);
            }
            WeekScheduleDetail detail_root = temp_showDetail.Find(a => (a.ParentNode == null && a.Level == 1));
            if (detail_root == null)
                return;


            showDetail.Clear();

            showDetail.Add(detail_root);


            var queryDtl = from d in temp_showDetail
                           orderby (wsdorder == WSDOrderSet.GWBS ? d.OrderNo : d.WSDOrderNo) ascending
                           orderby d.Level ascending
                           select d;

            foreach (WeekScheduleDetail item in queryDtl)
            {
                ArrayList list = new ArrayList();
                getWeekChildDtl(item, temp_showDetail, ref list);
                foreach (WeekScheduleDetail lst in list)
                {
                    if (!showDetailContains(lst))
                    {
                        showDetail.Add(lst);
                    }
                }
            }

        }

        private void getWeekChildDtl(WeekScheduleDetail parentDtl, List<WeekScheduleDetail> listDtl, ref ArrayList listResult)
        {

            var queryDtl = from d in listDtl.OfType<WeekScheduleDetail>()
                           where (d.ParentNode != null && d.ParentNode.Id == parentDtl.Id)
                           orderby (wsdorder == WSDOrderSet.GWBS ? d.OrderNo : d.WSDOrderNo) ascending
                           orderby d.Level ascending
                           select d;
            parentDtl.ChildCount = queryDtl.Count();

            foreach (WeekScheduleDetail dtl in queryDtl)
            {
                listResult.Add(dtl);

                getWeekChildDtl(dtl, listDtl, ref listResult);
            }
        }

        private void MaintainExpandCollapse(WeekScheduleDetail detail)
        {
            if (detail == null)
                return;

            if (detail.IsExpand == null)
                return;

            flexGrid.AutoRedraw = false;
            bool isVisble = false;

            int activeRowIndex = FindInflexGrid(detail);
            if ((bool)detail.IsExpand)
            {
                flexGrid.Cell(activeRowIndex, detail.Level).SetImage(imageExpand);
                isVisble = false;
            }
            else
            {
                flexGrid.Cell(activeRowIndex, detail.Level).SetImage(imageCollapse);
                isVisble = true;
            }
            string detailId = flexGrid.Cell(activeRowIndex, 0).Tag;
            int childs = 0;
            if (detailHashtable.ContainsKey(detailId))
            {
                //var detail = detailHashtable[detailId] as WeekScheduleDetail;
                childs = (from a in detailHashtable.Values.OfType<WeekScheduleDetail>()
                          where a.SysCode.StartsWith(detail.SysCode) && a.Id != detailId
                          select a
                         ).Count();
            }

            SetRowVisible(activeRowIndex + 1, activeRowIndex + childs, isVisble);

            flexGrid.AutoRedraw = true;
        }

        private int FindInflexGrid(WeekScheduleDetail detail)
        {
            if (detail == null)
                return 0;
            for (int i = 1; i < flexGrid.Rows; i++)
            {
                if (flexGrid.Cell(i, 0).Tag == detail.Id)
                {
                    return i;
                }
            }
            return 0;
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

        private void LockFlexGridColumn(bool isSet)
        {
            for (int i = 0; i < flexGrid.Cols; i++)
            {
                flexGrid.Column(i).Locked = isSet ? true : false;
            }
            if (flexGrid.Cols >= endImageCol + 1)
                flexGrid.Column(endImageCol + 1).Locked = false;
        }

        private string GetFrontTask(WeekScheduleDetail detail, IList list)
        {
            string str_Rtn = "";
            foreach (var item in detail.RalationDetails)
            {
                if (str_Rtn != "")
                    str_Rtn += " ,";

                int index_preWsd = -1;
                WeekScheduleDetail wsd_find = ((List<WeekScheduleDetail>)list).Find(a => a.Id == item.FrontWeekScheduleDetail.Id);
                if (wsd_find != null)
                    index_preWsd = list.IndexOf(wsd_find);
                if (index_preWsd < 0)
                {
                    WeekScheduleDetail wsd_ff = CurBillMaster.Details.ToList<WeekScheduleDetail>().Find(a => a.Id == item.FrontWeekScheduleDetail.Id);
                    if (wsd_ff != null)
                        str_Rtn += wsd_ff.GWBSTreeName;
                }
                else
                    str_Rtn += ClientUtil.ToString(index_preWsd + 1);

                str_Rtn += Enum.GetName(typeof(EnumDelayRule), item.DelayRule) + "+";

                str_Rtn += ClientUtil.ToString(item.DelayDays) + "天";


            }
            return str_Rtn;
        }

        private void SetRowBold(int rowIndex, bool isBold)
        {
            for (var i = 0; i < flexGrid.Cols; i++)
            {
                flexGrid.Cell(rowIndex, i).FontBold = isBold;
            }
        }

        private void SetflexGridRowColor(int rowIndex, WeekScheduleDetail detail)
        {
            if (detail != null)
            {
                flexGrid.Cell(rowIndex, detail.Level + 1).BackColor = detail.State == DocumentState.InExecute ? Color.DarkOliveGreen : SystemColors.Control;
                flexGrid.Cell(rowIndex, detail.Level + 1).ForeColor = detail.State == DocumentState.InExecute ? Color.White : SystemColors.ControlText;
                flexGrid.Cell(rowIndex, detail.Level).Alignment = AlignmentEnum.LeftCenter;

                flexGrid.Cell(rowIndex, endImageCol + 2).BackColor = detail.ProductionCuringNode ? Color.DarkOrange : SystemColors.Control;
                flexGrid.Cell(rowIndex, endImageCol + 2).ForeColor = detail.ProductionCuringNode ? Color.White : SystemColors.ControlText;

                flexGrid.Cell(rowIndex, endImageCol + 3).BackColor = detail.ProductionCuringNode ? Color.DarkOrange : SystemColors.Control;
                flexGrid.Cell(rowIndex, endImageCol + 3).ForeColor = detail.ProductionCuringNode ? Color.White : SystemColors.ControlText;
            }
        }

        private void SetChildCountShowDetail()
        {
            foreach (WeekScheduleDetail item in showDetail)
            {
                var queryDtl = from d in showDetail.OfType<WeekScheduleDetail>()
                               where (d.ParentNode != null && d.ParentNode.Id == item.Id)
                               orderby d.OrderNo ascending
                               orderby d.Level ascending
                               select d;
                item.ChildCount = queryDtl.Count();
            }


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
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", CurBillMaster.Id));
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

            if (CurBillMaster == null)
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


        public bool OnSave(bool isAgree)
        {
            try
            {
                List<WeekScheduleDetail> listWsd = new List<WeekScheduleDetail>();
                foreach (var item in showDetail)
                {
                    if (item.IsSubmitCheck)
                    {
                        item.State = isAgree ? DocumentState.InExecute : DocumentState.Edit;
                        item.SubmitDate = DateTime.Now;
                        item.SubmitPerson = ConstObject.LoginPersonInfo;
                        item.SubmitPersonName = ConstObject.LoginPersonInfo.Name;
                        listWsd.Add(item);
                    }
                }
                if (listWsd == null || listWsd.Count < 1)
                    return false;
                model.ProductionManagementSrv.SaveUpdateWeekPlanDtl(listWsd);
                return true;
            }
            catch
            {

                return false;
            }
        }
    }
}
