﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.Properties;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using FlexCell;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using System.IO;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;
using IRPServiceModel.Domain.Document;
using NHibernate.Criterion;
using IRPServiceModel.Basic;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using Application.Business.Erp.SupplyChain.Client.FileUpload;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI
{
    public partial class VSchedule : TMasterDetailView
    {
        private MProductionMng model = new MProductionMng();
        private Hashtable detailHashtable = new Hashtable();
        private WeekScheduleMaster CurBillMaster;
        private string imageExpand = "imageExpand";
        private string imageCollapse = "imageCollapse";
        private EnumScheduleType enumScheduleType;
        private List<string> listDtlIds = new List<string>();
        private CurrentProjectInfo projectInfo;
        private WeekScheduleDetail findDetail;

        private const int startImageCol = 1, endImageCol = 19;
        private DateTime defaultTime = new DateTime(1900, 1, 1);
        private int CopyLevel = 0;
        private IList<WeekScheduleDetail> showDetail = null;
        //private List<WeekScheduleRalation> LstRelated = null;

        //private string flexGrid_detailId = "";
        private WSDOrderSet wsdorder = WSDOrderSet.WSD;
        /// <summary>
        /// 待删除的对象集合
        /// </summary>
        private List<WeekScheduleRalation> lstDelObj = new List<WeekScheduleRalation>();

        public VSchedule(EnumScheduleType enumScheduleType)
        {
            InitializeComponent();

            this.enumScheduleType = enumScheduleType;

            InitEvents();

            InitData();
        }

        public override void RefreshState(MainViewState state)
        {
            base.RefreshState(state);
             ViewCaption = ViewName+"-" + projectInfo.Name;;
        }
        private void InitEvents()
        {

            btnGWBS.Click += new EventHandler(btnGWBS_Click);
            btnFindNext.Click += new EventHandler(btnFindNext_Click);

            flexGrid.Click += new FlexCell.Grid.ClickEventHandler(flexGrid_Click);
            flexGrid.CellChange += new Grid.CellChangeEventHandler(flexGrid_CellChange);
            flexGrid.DoubleClick += new Grid.DoubleClickEventHandler(flexGrid_DoubleClick);
            flexGrid.MouseUp += new Grid.MouseUpEventHandler(flexGrid_MouseUp);


            //删除计划明细
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnSubmit.Click += new EventHandler(btnSubmit_Click);

            btnExportToMPP.Click += new EventHandler(btnExportToMPP_Click);

            txtFindKey.TextChanged += new EventHandler(txtFindKey_TextChanged);

            cmsGrid.ItemClicked += new ToolStripItemClickedEventHandler(cmsGrid_ItemClicked);

            this.btnFilter.Click += new EventHandler(btnFilter_Click);
            this.btnSync.Click += new EventHandler(btnSync_Click);

            this.txtWorkDays.LostFocus += new EventHandler(txtWorkDays_LostFocus);
            #region 相关文档

            btnDownLoadDocument.Click += new EventHandler(btnDownLoadDocument_Click);
            btnOpenDocument.Click += new EventHandler(btnOpenDocument_Click);
            btnUpdateDocument.Click += new EventHandler(btnUpdateDocument_Click);
            btnDeleteDocumentFile.Click += new EventHandler(btnDeleteDocumentFile_Click);
            btnUpFile.Click += new EventHandler(btnUpFile_Click);
            dgDocumentMast.CellClick += new DataGridViewCellEventHandler(dgDocumentMast_CellClick);
            btnDeleteDocumentMaster.Click += new EventHandler(btnDeleteDocumentMaster_Click);
            btnDocumentFileAdd.Click += new EventHandler(btnDocumentFileAdd_Click);
            btnDocumentFileUpdate.Click += new EventHandler(btnDocumentFileUpdate_Click);
            lnkCheckAll.Click += new EventHandler(lnkCheckAll_Click);
            lnkCheckAllNot.Click += new EventHandler(lnkCheckAllNot_Click);

            #endregion
            //tab页切换数据处理
            tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);

            this.btnUp.Click += new EventHandler(btnUp_Click);
            this.btnDown.Click += new EventHandler(btnDown_Click);

            this.rbtnGWBSOrder.CheckedChanged += new EventHandler(rbtnGWBSOrder_CheckedChanged);
            this.rbtnWSDOrder.CheckedChanged += new EventHandler(rbtnWSDOrder_CheckedChanged);

            this.btnPreViewCriPath.Click += new EventHandler(btnPreViewCriPath_Click);
            this.btnSetNodeAtrr.Click += new EventHandler(btnSetNodeAtrr_Click);
        }

        void btnSetNodeAtrr_Click(object sender, EventArgs e)
        {
            VScheduleSetNodeAtrr dlg = new VScheduleSetNodeAtrr(projectInfo, CurBillMaster);
            dlg.ShowDialog();
            if (dlg.IsChanged)
            {
                foreach (var item in dlg.RtnWSDs)
                {
                    
                    MaintainWorkDateInfoFollow(item);
                    //SetParentRowDateTime(item);
                }
                SetShowDetailParentDateTime();
                FillFlex();
            }

           
        }

        void btnPreViewCriPath_Click(object sender, EventArgs e)
        {
            VPreViewCriPath dlg = new VPreViewCriPath(projectInfo);
            dlg.ShowDialog();
        }

        void rbtnWSDOrder_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnWSDOrder.Checked)
            {
                wsdorder = WSDOrderSet.WSD;
                this.btnUp.Enabled = true;
                this.btnDown.Enabled = true;
            }
            FillFlex();
        }

        void rbtnGWBSOrder_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnGWBSOrder.Checked)
            {
                wsdorder = WSDOrderSet.GWBS;
                this.btnUp.Enabled = false;
                this.btnDown.Enabled = false;
            }
            FillFlex();
        }

        void btnDown_Click(object sender, EventArgs e)
        {
            string errorMsg = "";
            bool isCanUpDown = CheckCanUpAndDown(ref errorMsg);
            if (!isCanUpDown)
            {
                MessageBox.Show(errorMsg);
                return;
            }

            string detailId = flexGrid.Cell(flexGrid.ActiveCell.Row, 0).Tag;
            var detail = detailHashtable[detailId] as WeekScheduleDetail;

            int detail_wsdorder = (int)detail.WSDOrderNo;

            var parentWsd = detail.ParentNode;
            var wsd_SameLevel = showDetail.Where(p => (p.ParentNode != null && p.ParentNode.Id == parentWsd.Id));
            if (wsd_SameLevel == null || wsd_SameLevel.Count() < 1)
                return;

            var wsd_SameLevelDown = wsd_SameLevel.Where(p => p.WSDOrderNo > detail_wsdorder);

            if (wsd_SameLevelDown == null || wsd_SameLevelDown.Count() < 1)
                return;

            int detailnext_wsdorder = (int)wsd_SameLevelDown.Min(p => p.WSDOrderNo);
            WeekScheduleDetail detailnext = wsd_SameLevelDown.First(p => p.WSDOrderNo == detailnext_wsdorder);

            detail.WSDOrderNo = detailnext_wsdorder;
            detailnext.WSDOrderNo = detail_wsdorder;

            FillFlex();
           
        }

        void btnUp_Click(object sender, EventArgs e)
        {
            string errorMsg = "";
            bool isCanUpDown = CheckCanUpAndDown(ref errorMsg);
            if (!isCanUpDown)
            {
                MessageBox.Show(errorMsg);
                return;
            }
            string detailId = flexGrid.Cell(flexGrid.ActiveCell.Row, 0).Tag;
            var detail = detailHashtable[detailId] as WeekScheduleDetail;

            int detail_wsdorder = (int)detail.WSDOrderNo;

            var parentWsd = detail.ParentNode;
            var wsd_SameLevel = showDetail.Where(p => (p.ParentNode != null && p.ParentNode.Id == parentWsd.Id));
            if (wsd_SameLevel == null || wsd_SameLevel.Count() < 1)
                return;

            var wsd_SameLevelUp = wsd_SameLevel.Where(p => p.WSDOrderNo < detail_wsdorder);

            if (wsd_SameLevelUp == null || wsd_SameLevelUp.Count() < 1)
                return;

            int detailnext_wsdorder = (int)wsd_SameLevelUp.Max(p => p.WSDOrderNo);
            WeekScheduleDetail detailnext = wsd_SameLevelUp.First(p => p.WSDOrderNo == detailnext_wsdorder);

            detail.WSDOrderNo = detailnext_wsdorder;
            detailnext.WSDOrderNo = detail_wsdorder;

            FillFlex();
        }

        private bool CheckCanUpAndDown(ref string Msg)
        {
            bool b_rtn = true;
            if (b_rtn && (flexGrid.Selection == null || flexGrid.ActiveCell.Row <2))
            {
                Msg = "请选择您要调整的行！";
                b_rtn = false;
            }
            if (b_rtn && (flexGrid.Selection.FirstRow != flexGrid.Selection.LastRow))
            {
                Msg = "只能选择一行调整！";
                b_rtn = false;
            }
            if (b_rtn && (flexGrid.Selection.FirstRow == 1))
            {
                Msg = "根节点不能调整！";
                b_rtn = false;
            }
            return b_rtn;
        }

        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == tabPage1.Name)//如果是明细
            {

            }
            else if (tabControl1.SelectedTab.Name == tabPage2.Name)//相关文档
            {
                if (CurBillMaster != null && !string.IsNullOrEmpty(CurBillMaster.Id))
                {
                    FillDoc();
                }
            }
        }

        void flexGrid_DoubleClick(object Sender, EventArgs e)
        {
            if (flexGrid.ActiveCell.Col != endImageCol + 5)
            {
                return;
            }
            SetFrontTask(this.flexGrid, flexGrid.ActiveCell.Row);
        }

        void btnSync_Click(object sender, EventArgs e)
        {
            FlashScreen.Show(string.Format("正在同步总进度计划，请稍候..."));
            try
            {
                SyncGWBSToWeekScheduleDetail();
                MessageBox.Show("同步完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("同步失败.\n" + ex.Message);
            }
            finally
            {
                FlashScreen.Close();

            }
        }

        private void SyncGWBSToWeekScheduleDetail()
        {
            IList ilistWholeTree = model.ProductionManagementSrv.GetGWBSTreesByInstance(projectInfo.Id);
            List<GWBSTree> listWholeTree = ilistWholeTree as List<GWBSTree> ?? new List<GWBSTree>(ilistWholeTree.OfType<GWBSTree>());

            List<WeekScheduleDetail> list_Add = new List<WeekScheduleDetail>();

            List<WeekScheduleDetail> list_Del = new List<WeekScheduleDetail>();
            //维护 总进度计划的 ParentNode
            foreach (var item in CurBillMaster.Details)
            {
                //GWBSTree 为null 
                if (item.GWBSTree == null)
                {
                    list_Del.Add(item);
                    continue;
                }

                //
                GWBSTree gwbs_item = listWholeTree.Find(p => p.Id == item.GWBSTree.Id);
                if (gwbs_item == null)
                {
                    list_Del.Add(item);
                    continue;
                }

              
            }
            //同步GWBS节点至总进度计划明细
            foreach (var item in listWholeTree)
            {
                if (CurBillMaster.Details.Count > 0)
                {
                    var WSD_Find = CurBillMaster.Details.ToList<WeekScheduleDetail>().Find(p => p.GWBSTree.Id == item.Id);
                    if (WSD_Find != null)
                        continue;
                }

                WeekScheduleDetail detail = new WeekScheduleDetail();
                detail.GWBSTree = item as GWBSTree;
                if (detail.GWBSTree != null)
                {
                    detail.GWBSTreeName = detail.GWBSTree.Name;
                    detail.GWBSTreeSysCode = detail.GWBSTree.SysCode;
                    detail.NodeType = detail.GWBSTree.CategoryNodeType;
                    detail.OrderNo = (int)detail.GWBSTree.OrderNo;
                    detail.Level = detail.GWBSTree.Level;
                    detail.IsFixed = detail.GWBSTree.IsFixed;
         
                    if (detail.GWBSTree.ProductionCuringNode)
                    {
                        detail.ProductionCuringNode = detail.GWBSTree.ProductionCuringNode;
                        if (detail.GWBSTree.ProCurBeginDate != null && detail.GWBSTree.ProCurEndDate != null)
                        {
                            detail.PlannedBeginDate = ClientUtil.ToDateTime(detail.GWBSTree.ProCurBeginDate);
                            detail.PlannedEndDate = (DateTime)detail.GWBSTree.ProCurEndDate;
                            detail.PlannedDuration = (int)GetWorkDays(ClientUtil.ToDateTime(detail.PlannedBeginDate), ClientUtil.ToDateTime(detail.PlannedEndDate));
                        }
                    }
                }

                detail.Master = CurBillMaster;
                detail.State = DocumentState.Edit;
                detail.ScheduleUnit = "天";
                detail.ProjectId = CurBillMaster.ProjectId;
                detail.ProjectName = CurBillMaster.ProjectName;
                list_Add.Add(detail);
               
            }


            foreach (var item in list_Add)
            {
                CurBillMaster.Details.Add(item);
            }
            foreach (WeekScheduleDetail item in list_Add)
            {
                #region 维护父级节点
                GWBSTree gwbs_item = listWholeTree.Find(p => p.Id == item.GWBSTree.Id);
                if (gwbs_item.ParentNode == null)
                    continue;
                var WSD_Find = CurBillMaster.Details.ToList<WeekScheduleDetail>().Find(p => p.GWBSTree.Id == gwbs_item.ParentNode.Id);
                if (WSD_Find == null)
                {
                    continue;
                }
                item.ParentNode = WSD_Find;
                #endregion
            }

           
            foreach (var item in list_Del)
            {
                CurBillMaster.Details.Remove(item);
            }
            model.ProductionManagementSrv.SaveWeekPlanDtl(CurBillMaster.Details.ToList<WeekScheduleDetail>(),list_Add, list_Del);

            ReLoadMaster();


            ObjectQuery oq = new ObjectQuery();
 

            oq.AddCriterion(Expression.Eq("Master.Id", CurBillMaster.Id));
            oq.AddCriterion(Expression.Eq("Level",1));

            showDetail = model.ProductionManagementSrv.GetShowWeekScheduleDetails(oq).OfType<WeekScheduleDetail>().ToList<WeekScheduleDetail>();

            MaintainShowDetail();
            SetOrderShowDetail_Load();
            SetShowDetailParentDateTime();
            ModelToView();
            
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

        void btnFilter_Click(object sender, EventArgs e)
        {
            GetShowDetail();
        }

        /// <summary>
        /// 右键菜单点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmsGrid_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            cmsGrid.Close();
            if (e.ClickedItem.Name == 统一推迟.Name)
            {

                if (endImageCol >= flexGrid.Selection.FirstCol || flexGrid.Selection.LastCol > endImageCol + 2)
                {
                    MessageBox.Show("请选择叶节点的“计划开始时间”，“计划结束时间”列做此操作！");
                    return;
                }

                string msg = "";

                var rowindex = 0;
                var colIndex = 0;
                double delayDays = 0;
                if (!double.TryParse(txtDelay.Text.Trim(), out delayDays))
                {
                    MessageBox.Show("请输入有效的正，负数字！");
                    return;
                }
                int rowcount = flexGrid.Selection.LastRow - flexGrid.Selection.FirstRow + 1;
                int colCount = flexGrid.Selection.LastCol - flexGrid.Selection.FirstCol + 1;
                for (int i = 0; i < rowcount; i++)
                {
                    rowindex = flexGrid.Selection.FirstRow + i;
                    //如果是叶节点且是编辑状态时则修改数据
                    //if (flexGrid.Cell(rowindex, endImageCol + 1).Tag == "2" && flexGrid.Cell(rowindex, endImageCol + 2).Tag == "0")
                    if (flexGrid.Row(rowindex).Locked == false)
                    {
                        for (int j = 0; j < colCount; j++)
                        {
                            colIndex = flexGrid.Selection.FirstCol + j;
                            var curCellText = flexGrid.Cell(rowindex, colIndex).Text.Trim();
                            var curCellData = DateTime.Now;
                            if (!string.IsNullOrEmpty(curCellText) && DateTime.TryParse(curCellText, out curCellData))
                            {
                                flexGrid.Cell(rowindex, colIndex).Text = curCellData.AddDays(delayDays).ToShortDateString();
                            }
                        }
                    }
                    else
                    {
                        msg += rowindex.ToString() + "，";
                    }
                }
                if (!string.IsNullOrEmpty(msg))
                {
                    MessageBox.Show("第" + msg.TrimEnd(new char[] { '，' }) + "行由于不是叶节点或数据状态为非编辑状态而操作失败！");
                }
            }
            else if (e.ClickedItem.Name == 成本预算.Name)
            {
                var rowindex = flexGrid.ActiveCell.Row;
                string projectid = projectInfo.Id;
                string sysCode = flexGrid.Cell(rowindex, endImageCol + 3).Tag;
                VCostBudget vdetail = new VCostBudget(projectid, sysCode);
                vdetail.ShowDialog();
            }
            else if (e.ClickedItem.Name == 前置节点设置.Name)
            {
                SetFrontTask(this.flexGrid, flexGrid.ActiveCell.Row);
                #region cl update

                ////MessageBox.Show("该功能待开发中....");
                //var rowindex = flexGrid.ActiveCell.Row;
                //string detailId = flexGrid.Cell(rowindex, 0).Tag;
                //var detail = detailHashtable[detailId] as WeekScheduleDetail;
                //var curNodeRelated = detail.RalationDetails.ToList();
                //VSchedulePreNodeSet frm = new VSchedulePreNodeSet(flexGrid, detailId, showDetail.OfType<WeekScheduleDetail>().ToList(), curNodeRelated, CurBillMaster.Details.ToList<WeekScheduleDetail>());
                //frm.ShowDialog();

                ////如果不是点击的保存按钮,则退出操作
                //if (frm.IsSave == false)
                //{
                //    return;
                //}

                //detail.RalationDetails.Clear();
                //if (frm.RtnListRalated != null && frm.RtnListRalated.Count > 0)
                //{
                //    if (lstDelObj == null)
                //    {
                //        lstDelObj = new List<WeekScheduleRalation>();
                //    }
                //    var delobjs = curNodeRelated.Where(m => frm.RtnListRalated.Any(p => p.Id == m.Id) == false).ToList();
                //    if (delobjs != null)
                //    {
                //        lstDelObj.AddRange(delobjs);
                //    }
                //    detail.RalationDetails.AddAll(frm.RtnListRalated);
                //}

                ////根据判断类型FS
                ////时间，主线和延时时间                
                //var curNodeStartDate = ClientUtil.ToDateTime(flexGrid.Cell(rowindex, endImageCol + 1).Text.Trim());
                //var curNodeEndDate = ClientUtil.ToDateTime(flexGrid.Cell(rowindex, endImageCol + 2).Text.Trim());
                //int curNodeTimeSpan = (curNodeEndDate - curNodeStartDate).Days;
                ////当前节点最晚的开始时间
                //var maxStartDate = curNodeStartDate;
                //var maxEndDate = curNodeEndDate;
                //foreach (var item in frm.RtnListRalated)
                //{
                //    var preNodeStartDate = ClientUtil.ToDateTime(flexGrid.Cell(item.PreNodeRowIndex, endImageCol + 1).Text.Trim());
                //    var preNodeEndDate = ClientUtil.ToDateTime(flexGrid.Cell(item.PreNodeRowIndex, endImageCol + 2).Text.Trim());
                //    if (item.DelayRule == EnumDelayRule.FS)
                //    {//前置任务完成后，延时N天开始当前任务
                //        if (maxStartDate < preNodeEndDate.AddDays(item.DelayDays))
                //        {
                //            maxStartDate = preNodeEndDate.AddDays(item.DelayDays);
                //            maxEndDate = maxStartDate.AddDays(curNodeTimeSpan);
                //        }
                //    }
                //    else if (item.DelayRule == EnumDelayRule.SS)
                //    {//前置任务开始后，延时N天开始当前任务
                //        if (maxStartDate < preNodeStartDate.AddDays(item.DelayDays))
                //        {
                //            maxStartDate = preNodeStartDate.AddDays(item.DelayDays);
                //            maxEndDate = maxStartDate.AddDays(curNodeTimeSpan);
                //        }
                //    }
                //    else if (item.DelayRule == EnumDelayRule.FF)
                //    {//前置任务完成后，延时N天结束当前任务
                //        if (maxEndDate < preNodeEndDate.AddDays(item.DelayDays))
                //        {
                //            maxEndDate = preNodeEndDate.AddDays(item.DelayDays);
                //            maxStartDate = maxStartDate.AddDays(0 - curNodeTimeSpan);
                //        }
                //    }
                //}
                //flexGrid.Cell(rowindex, endImageCol + 5).Text = frm.StrRalation;
                //flexGrid.Cell(rowindex, endImageCol + 1).Text = maxStartDate.ToShortDateString();
                //flexGrid.Cell(rowindex, endImageCol + 2).Text = maxEndDate.ToShortDateString();
                #endregion
            }
            else if (e.ClickedItem.Name == 设置节点特性.Name)
            {
                string detailId = flexGrid.Cell(flexGrid.ActiveCell.Row, 0).Tag;
                if (detailId == "")
                {
                    MessageBox.Show("您当前没有选中节点，请先选中您要设置的节点！");
                    return;
                }
                var detail = detailHashtable[detailId] as WeekScheduleDetail;

                string str_gwbstree = detail.GWBSTree.Id;
                GWBSTree gwbs_setTree = model.ProductionManagementSrv.GetObjectById(typeof(GWBSTree), str_gwbstree) as GWBSTree;
                VGWBSTreeNodeEdit dlg = new VGWBSTreeNodeEdit(gwbs_setTree);
                dlg.SetNodeGWBSTree = gwbs_setTree;
                dlg.ShowDialog();
                if (dlg.IsChanged)
                {
                    if (dlg.SetNodeGWBSTree.ProductionCuringNode)
                    {
                        detail.PlannedBeginDate = (DateTime)dlg.SetNodeGWBSTree.ProCurBeginDate;
                        detail.PlannedEndDate = (DateTime)dlg.SetNodeGWBSTree.ProCurEndDate;
                        detail.PlannedDuration = (int)GetWorkDays(dlg.SetNodeGWBSTree.ProCurBeginDate, dlg.SetNodeGWBSTree.ProCurEndDate);
                        
                        MaintainWorkDateInfoFollow(detail);
                        //SetParentRowDateTime(detail);
                        FillFlex();
                    }
                }

            }

            else if (e.ClickedItem.Name == 设置工期.Name)
            {
                SetWorkDays();
                this.txtWorkDays.Text = "";
                FillFlex();
            }


        }

        void txtWorkDays_LostFocus(object sender, EventArgs e)
        {
            SetWorkDays();
            this.txtWorkDays.Text = "";
            FillFlex();
        }

        private void SetWorkDays()
        {
            string str_workdays = this.txtWorkDays.Text.Trim();
            int? workdays = ConvertToInt(str_workdays);
            if (workdays == null)
            {
                this.txtWorkDays.Text = "";
                return;
            }
            int rowcount = flexGrid.Selection.LastRow - flexGrid.Selection.FirstRow + 1;

            int rowindex = 0;
            for (int i = 0; i < rowcount; i++)
            {
                rowindex = flexGrid.Selection.FirstRow + i;
                //如果是叶节点且是编辑状态时则修改数据
                //if (flexGrid.Cell(rowindex, endImageCol + 1).Tag == "2" && flexGrid.Cell(rowindex, endImageCol + 2).Tag == "0")

                string detailId = flexGrid.Cell(rowindex, 0).Tag;
                var detail = detailHashtable[detailId] as WeekScheduleDetail;
                detail.PlannedDuration = (int)workdays;
                if (detail.PlannedBeginDate != defaultTime)
                    detail.PlannedEndDate = (DateTime)GetBenginOrEndDate(ClientUtil.ToDateTime(detail.PlannedBeginDate), ClientUtil.ToInt(detail.PlannedDuration), DateRel.GetEndDate);
                
                MaintainWorkDateInfoFollow(detail);
                //SetParentRowDateTime(detail);
            }


        }

        /// <summary>
        /// 是否所有的前置任务节点都包含在showDetail中
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool IsAllFrontTaskInShowDetail(ref string msg)
        {
            foreach (var wsd in showDetail)
            {
                foreach (var item in wsd.RalationDetails)
                {
                    WeekScheduleDetail wsd_ff = CurBillMaster.Details.ToList<WeekScheduleDetail>().Find(a => a.Id == item.FrontWeekScheduleDetail.Id);
                    if (wsd_ff == null)
                    {
                        msg = "Error!";
                        return false;
                    }

                    WeekScheduleDetail wsd_Fornt = showDetail.ToList<WeekScheduleDetail>().Find(a => a.Id == wsd_ff.Id);
                    if (wsd_Fornt == null)
                    {
                        msg = wsd.GWBSTreeName + " 的前置任务【" + wsd_ff.GWBSTreeName + "】不在此计划中，请先行添加！";
                        return false;
                    }
                }
            }
            msg = "";
            return true;
        }

        private void SetFrontTask(Grid fGrid, int rowindex)
        {
            string msg = "";
            bool is_allin = IsAllFrontTaskInShowDetail(ref msg);
            if (!is_allin)
            {
                MessageBox.Show(msg);
                return;
            }

            //MessageBox.Show("该功能待开发中....");
            //var rowindex = flexGrid.ActiveCell.Row;
            string detailId = fGrid.Cell(rowindex, 0).Tag;
            var detail = detailHashtable[detailId] as WeekScheduleDetail;
            var curNodeRelated = detail.RalationDetails.ToList();
            VSchedulePreNodeSet frm = new VSchedulePreNodeSet(fGrid, detailId, showDetail.OfType<WeekScheduleDetail>().ToList(), curNodeRelated, CurBillMaster.Details.ToList<WeekScheduleDetail>());
            frm.ShowDialog();

            //如果不是点击的保存按钮,则退出操作
            if (frm.IsSave == false)
            {
                return;
            }

            detail.RalationDetails.Clear();
            if (frm.RtnListRalated != null && frm.RtnListRalated.Count > 0)
            {
                if (lstDelObj == null)
                {
                    lstDelObj = new List<WeekScheduleRalation>();
                }
                var delobjs = curNodeRelated.Where(m => frm.RtnListRalated.Any(p => p.Id == m.Id) == false).ToList();
                if (delobjs != null)
                {
                    lstDelObj.AddRange(delobjs);
                }
                detail.RalationDetails.AddAll(frm.RtnListRalated);
            }
            detail.PlannedDuration = frm.RtnPlannedDuration;
            detail.PlannedBeginDate = frm.RtnBeginDate;
            var enddate = GetBenginOrEndDate(detail.PlannedBeginDate, (int)detail.PlannedDuration, DateRel.GetEndDate);
            detail.PlannedEndDate = enddate.HasValue ? enddate.Value : defaultTime;
           
            MaintainWorkDateInfoByFront(detail);
            MaintainWorkDateInfoFollow(detail);
            //SetShowDetailParentDateTime();
            //SetParentRowDateTime(detail);
            //FillFlex();
            FillFlex_CellChange();
        }

        /// <summary>
        /// 根据当前节点的前置节点 维护当前节点工期信息
        /// </summary>
        /// <param name="detail">当前节点</param>
        private void MaintainWorkDateInfoByFront(WeekScheduleDetail detail)
        {
            if (detail.RalationDetails == null)
                return;
            DateTime maxBegindate = defaultTime;

            foreach (var item in detail.RalationDetails)
            {
                //当前节点的前置节点
                WeekScheduleDetail wsd = showDetail.ToList<WeekScheduleDetail>().Find(a => a.Id == item.FrontWeekScheduleDetail.Id);
                if (item.DelayRule == EnumDelayRule.FS)
                {
                    if (wsd.PlannedEndDate == null || wsd.PlannedEndDate == defaultTime)
                        continue;
                    DateTime dtbegin_wsd = (DateTime)GetDateByDelay(wsd.PlannedEndDate, item.DelayDays);
                    maxBegindate = maxBegindate < dtbegin_wsd ? dtbegin_wsd : maxBegindate;
                }
                if (item.DelayRule == EnumDelayRule.SS)
                {
                    if (wsd.PlannedBeginDate == null || wsd.PlannedBeginDate == defaultTime)
                        continue;
                    DateTime dtbegin_wsd = (DateTime)GetDateByDelay(wsd.PlannedBeginDate, item.DelayDays);
                    maxBegindate = maxBegindate < dtbegin_wsd ? dtbegin_wsd : maxBegindate;
                }
                if (item.DelayRule == EnumDelayRule.FF)
                {
                    if (wsd.PlannedEndDate == null || wsd.PlannedEndDate == defaultTime)
                        continue;
                    DateTime dtend_wsd = (DateTime)GetDateByDelay(wsd.PlannedEndDate, item.DelayDays);
                    if (wsd.PlannedDuration == (decimal)0)
                        break;
                    DateTime dtbegin_wsd = (DateTime)GetBenginOrEndDate((DateTime?)dtend_wsd, (int?)wsd.PlannedDuration, DateRel.GetBeginDate);
                    maxBegindate = maxBegindate < dtbegin_wsd ? dtbegin_wsd : maxBegindate;
                }

            }
            if (maxBegindate != defaultTime)
            {
                detail.PlannedBeginDate = maxBegindate;
                if (detail.PlannedDuration != (decimal)0)
                    detail.PlannedEndDate = (DateTime)GetBenginOrEndDate((DateTime?)maxBegindate, (int?)detail.PlannedDuration, DateRel.GetEndDate);
            }

        }

        /// <summary>
        /// 根据当前节点 维护以当前节点为前置任务的节点的工期信息
        /// </summary>
        /// <param name="detail"></param>
        private void MaintainWorkDateInfoFollow(WeekScheduleDetail detail)
        {
            SetParentRowDateTime(detail);
            foreach (var item in showDetail)
            {
                if (item.RalationDetails == null)
                    continue;
                //以当前节点作为前置节点
                WeekScheduleRalation wsdr_frind = item.RalationDetails.ToList<WeekScheduleRalation>().Find(a => a.FrontWeekScheduleDetail.Id == detail.Id);
                if (wsdr_frind != null)
                {
                    WeekScheduleDetail wsd = wsdr_frind.Master;
                    DateTime date_wsdBgein = wsd.PlannedBeginDate;
                    MaintainWorkDateInfoByFront(wsd);
                    if (date_wsdBgein == wsd.PlannedBeginDate)
                        break;
                    MaintainWorkDateInfoFollow(wsd);
                }

            }
        }


        #region 维护父级节点的工期信息

        /// <summary>
        /// 根据当前节点 维护父级节点的工期信息
        /// </summary>
        /// <param name="curDetail"></param>
        private void SetParentRowDateTime(WeekScheduleDetail curDetail)
        {
            if (curDetail == null || curDetail.ParentNode == null)
            {
                return;
            }

            //var pNode = curDetail.ParentNode;
            var pNode = showDetail.First(a => a.Id == curDetail.ParentNode.Id);


            if ((curDetail.PlannedBeginDate != defaultTime) && (pNode.PlannedBeginDate == defaultTime || pNode.PlannedBeginDate > curDetail.PlannedBeginDate))
            {
                pNode.PlannedBeginDate = curDetail.PlannedBeginDate;
            }
            if (( curDetail.PlannedEndDate!=defaultTime ) &&(pNode.PlannedEndDate == defaultTime || pNode.PlannedEndDate < curDetail.PlannedEndDate))
            {
                pNode.PlannedEndDate = curDetail.PlannedEndDate;
            }

            pNode.PlannedDuration = ClientUtil.ToDecimal(GetWorkDays(pNode.PlannedBeginDate, pNode.PlannedEndDate));

            if (detailHashtable != null && detailHashtable.ContainsKey(pNode.Id))
            {
                detailHashtable[pNode.Id] = pNode;
            }

            SetParentRowDateTime(pNode);
        }

        private void SetShowDetailParentDateTime()
        {
            foreach (WeekScheduleDetail item in showDetail)
            {
                SetParentRowDateTime(item);
            }
        }

        #endregion


       

        /// <summary>
        /// 右键菜单事件
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        void flexGrid_MouseUp(object Sender, MouseEventArgs e)
        {
            //如果是在开始结束时间上点击右键时，显示右键菜单
            if (e.Button == MouseButtons.Right)
            {

                Range range_sel = flexGrid.Range(flexGrid.Selection.FirstRow, flexGrid.Selection.FirstCol, flexGrid.Selection.LastRow, flexGrid.Selection.LastCol);
                //MessageBox.Show("Row: " + flexGrid.ActiveCell.Row.ToString() + "|Col:" + flexGrid.ActiveCell.Col.ToString());

                成本预算.Enabled = false;
                统一推迟.Enabled = false;
                前置节点设置.Enabled = false;
                设置节点特性.Enabled = false;
                设置工期.Enabled = false;

                if (startImageCol <= flexGrid.Selection.FirstCol && flexGrid.Selection.FirstCol <= endImageCol && flexGrid.Selection.FirstRow == flexGrid.Selection.LastRow)//成本预算
                {
                    成本预算.Enabled = true;
                    前置节点设置.Enabled = true;
                    设置节点特性.Enabled = true;
                }
                if (endImageCol < flexGrid.Selection.FirstCol && flexGrid.Selection.LastCol < endImageCol + 3)//统一推迟
                {
                    统一推迟.Enabled = true;
                }
                if (endImageCol + 3 == flexGrid.Selection.FirstCol && flexGrid.Selection.LastCol == endImageCol + 3)//统一推迟
                {
                    设置工期.Enabled = true;
                }

                cmsGrid.Show(flexGrid, e.X, e.Y);
            }
        }

        #region 文档操作
        MDocumentCategory msrv = new MDocumentCategory();
        //文档按钮状态
        private void btnStates(int i)
        {
            if (i == 0)
            {
                //btnDownLoadDocument.Enabled = false;
                //btnOpenDocument.Enabled = false;
                btnUpdateDocument.Enabled = false;
                btnDeleteDocumentFile.Enabled = false;
                btnUpFile.Enabled = false;
                btnDeleteDocumentMaster.Enabled = false;
                btnDocumentFileAdd.Enabled = false;
                btnDocumentFileUpdate.Enabled = false;
                lnkCheckAll.Enabled = false;
                lnkCheckAllNot.Enabled = false;
            }
            if (i == 1)
            {
                //btnDownLoadDocument.Enabled = true;
                //btnOpenDocument.Enabled = true;
                btnUpdateDocument.Enabled = true;
                btnDeleteDocumentFile.Enabled = true;
                btnUpFile.Enabled = true;
                btnDeleteDocumentMaster.Enabled = true;
                btnDocumentFileAdd.Enabled = true;
                btnDocumentFileUpdate.Enabled = true;
                lnkCheckAll.Enabled = true;
                lnkCheckAllNot.Enabled = true;
            }
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
                    dgDocumentMast_CellClick(dgDocumentMast, new DataGridViewCellEventArgs(2, 0));
                }
            }
        }
        //添加文件
        void btnDocumentFileAdd_Click(object sender, EventArgs e)
        {
            if (dgDocumentMast.SelectedRows.Count == 0) return;
            DocumentMaster docMaster = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
            VDocumentFileUpload frm = new VDocumentFileUpload(docMaster, "add");
            frm.ShowDialog();
            IList resultList = frm.ResultList;
            if (resultList != null && resultList.Count > 0)
            {
                foreach (DocumentDetail dtl in resultList)
                {
                    AddDgDocumentDetailInfo(dtl);
                    docMaster.ListFiles.Add(dtl);
                }
            }
        }
        //修改文件
        void btnDocumentFileUpdate_Click(object sender, EventArgs e)
        {
            if (dgDocumentMast.SelectedRows.Count == 0 || dgDocumentDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要修改的文档件！");
                return;
            }
            DocumentMaster docMaster = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
            VDocumentFileModify frm = new VDocumentFileModify(docMaster);
            frm.ShowDialog();
            IList resultList = frm.ResultList;
            if (resultList != null && resultList.Count > 0)
            {
                DocumentDetail dtl = resultList[0] as DocumentDetail;
                AddDgDocumentDetailInfo(dtl, dgDocumentDetail.SelectedRows[0].Index);
                for (int i = 0; i < docMaster.ListFiles.Count; i++)
                {
                    DocumentDetail detail = docMaster.ListFiles.ElementAt(i);
                    if (detail.Id == dtl.Id)
                    {
                        detail = dtl;
                    }
                }
            }
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
        //删除文件
        void btnDeleteDocumentFile_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> rowList = new List<DataGridViewRow>();
            IList deleteFileList = new ArrayList();
            foreach (DataGridViewRow row in dgDocumentDetail.Rows)
            {
                if ((bool)row.Cells[FileSelect.Name].EditedFormattedValue)
                {
                    DocumentDetail dtl = row.Tag as DocumentDetail; ;
                    rowList.Add(row);
                    deleteFileList.Add(dtl);
                }
            }
            if (deleteFileList.Count == 0)
            {
                MessageBox.Show("请勾选要删除的数据！");
                return;
            }
            if (MessageBox.Show("要删除当前勾选文件吗！", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (msrv.Delete(deleteFileList))
                {
                    MessageBox.Show("删除成功！");
                    DocumentMaster master = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
                    foreach (DocumentDetail dtl in deleteFileList)
                    {
                        master.ListFiles.Remove(dtl);
                    }
                    dgDocumentMast.SelectedRows[0].Tag = master;
                }
                else
                {
                    MessageBox.Show("删除失败！");
                    return;
                }
                if (rowList != null && rowList.Count > 0)
                {
                    foreach (DataGridViewRow row in rowList)
                    {
                        dgDocumentDetail.Rows.Remove(row);
                    }
                }
            }
        }
        //添加文档（加文件）
        void btnUpFile_Click(object sender, EventArgs e)
        {
            if (CurBillMaster.Id == null)
            {
                if (MessageBox.Show("当前业务对象还没保存，是否保存！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    if (!ValidView()) return;
                    try
                    {
                        //if (!ViewToModel()) return;
                        CurBillMaster = model.ProductionManagementSrv.SaveOrUpdateByDao(CurBillMaster) as WeekScheduleMaster;
                        this.ViewCaption = ViewName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(ex));
                    }
                }
            }
            if (CurBillMaster.Id != null)
            {
                VDocumentPublicUpload frm = new VDocumentPublicUpload(CurBillMaster.Id);
                frm.ShowDialog();
                DocumentMaster resultDoc = frm.Result;
                if (resultDoc == null) return;
                AddDgDocumentMastInfo(resultDoc);
                dgDocumentMast.CurrentCell = dgDocumentMast[2, dgDocumentMast.Rows.Count - 1];
                dgDocumentDetail.Rows.Clear();
                if (resultDoc.ListFiles != null && resultDoc.ListFiles.Count > 0)
                {
                    foreach (DocumentDetail dtl in resultDoc.ListFiles)
                    {
                        AddDgDocumentDetailInfo(dtl);
                    }
                }
            }
        }
        //修改文档（加文件）
        void btnUpdateDocument_Click(object sender, EventArgs e)
        {
            if (dgDocumentMast.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要修改的文档！");
                return;
            }
            DocumentMaster master = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
            IList docFileList = new ArrayList();
            foreach (DataGridViewRow row in dgDocumentDetail.Rows)
            {
                DocumentDetail dtl = row.Tag as DocumentDetail;
                docFileList.Add(dtl);
            }
            VDocumentPublicModify frm = new VDocumentPublicModify(master, docFileList);
            frm.ShowDialog();
            DocumentMaster resultMaster = frm.Result;
            if (resultMaster == null) return;
            AddDgDocumentMastInfo(resultMaster, dgDocumentMast.SelectedRows[0].Index);
            dgDocumentMast_CellClick(dgDocumentMast, new DataGridViewCellEventArgs(2, dgDocumentMast.SelectedRows[0].Index));
        }
        //删除文档
        void btnDeleteDocumentMaster_Click(object sender, EventArgs e)
        {
            if (dgDocumentMast.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的文档！");
                return;
            }
            if (MessageBox.Show("要删除当前文档吗？该操作将连它的所有文件一同删除！", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DocumentMaster mas = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
                IList list = new ArrayList();
                list.Add(mas);

                if (msrv.Delete(list))
                {
                    MessageBox.Show("删除成功！");
                    dgDocumentMast.Rows.Remove(dgDocumentMast.SelectedRows[0]);
                    dgDocumentMast_CellClick(dgDocumentMast, new DataGridViewCellEventArgs(2, dgDocumentMast.SelectedRows[0].Index));
                }
                else
                {
                    MessageBox.Show("删除失败！");
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

        #region 列表里添加数据
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
        #endregion

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
        #endregion

        private void ShowPlanFactInfo(bool isShow)
        {
            flexGrid.Column(endImageCol + 7).Visible = isShow;
            flexGrid.Column(endImageCol + 8).Visible = isShow;
            flexGrid.Column(endImageCol + 9).Visible = isShow;
        }

        private void InitData()
        {
            //归属项目
            projectInfo = StaticMethod.GetProjectInfo();

            InitFlexGrid(10);

            ShowPlanFactInfo(false);

            MaintainPlan();

            LoadPlan();

        }

        private void ReLoadMaster()
        {
            IList listMaster = GetProductionSchedules();
            if (listMaster != null && listMaster.Count != 0)
            {
                CurBillMaster = listMaster[0] as WeekScheduleMaster;
                //this.ViewName = "总进度计划-" + projectInfo.Name;
            }

        }

        /// <summary>
        /// 维护总进度计划
        /// </summary>
        private void MaintainPlan()
        {
            IList listMaster = GetProductionSchedules();
            bool isExistMaster = true;
            if (listMaster == null || listMaster.Count == 0)
                isExistMaster = false;

            if (!isExistMaster)
            {
                WeekScheduleMaster master = NewMaster();

                CurBillMaster = model.ProductionManagementSrv.NewWeekSchedule(master);

                #region
                #region 递归保存计划明细 耗时太慢
                //WeekScheduleDetail rootnode = GetScheduleDetailRootNode();

                //IList listWholeTree = model.ProductionManagementSrv.GetGWBSTreesByInstance(projectInfo.Id);

                //if (listWholeTree == null || listWholeTree.Count == 0)
                //    return;



                //VirtualMachine.Component.WinControls.Controls.CustomTreeView tvwCategory = new VirtualMachine.Component.WinControls.Controls.CustomTreeView();

                //Hashtable hashtable = new Hashtable();
                //foreach (GWBSTree childNode in listWholeTree)
                //{
                //    //if (childNode.State == 0)
                //    //    continue;

                //    TreeNode tnTmp = new TreeNode();
                //    tnTmp.Name = childNode.Id.ToString();
                //    tnTmp.Text = childNode.Name;
                //    tnTmp.Tag = childNode;
                //    if (childNode.ParentNode != null)
                //    {
                //        TreeNode tnp = null;
                //        tnp = hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                //        if (tnp != null)
                //            tnp.Nodes.Add(tnTmp);
                //    }
                //    else
                //    {
                //        tvwCategory.Nodes.Add(tnTmp);
                //    }
                //    hashtable.Add(tnTmp.Name, tnTmp);
                //}

                //TreeNode tnRoot = tvwCategory.Nodes[0];

                //SavePlanDtl(tnRoot, rootnode, master);

                #endregion

                #region 非递归保存计划明细
                SyncGWBSToWeekScheduleDetail();
                #endregion
                #endregion
            }
            else
            {
                CurBillMaster = listMaster[0] as WeekScheduleMaster;

                //this.ViewName = "总进度计划-" + projectInfo.Name;
            }




        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gWBSTree"></param>
        /// <param name="uppergwsd">GWBS节点所定义的父级WeekScheduleDetail</param>
        /// <param name="master"></param>
        /// <returns>gWBSTree对应的WeekScheduleDetail </returns>
        private WeekScheduleDetail AddWeekScheduleDetail(GWBSTree gWBSTree, ref WeekScheduleDetail uppergwsd, WeekScheduleMaster master, ref IList detailList)
        {
            WeekScheduleDetail detail = master.Details.ToList().Find(a => (a.GWBSTree != null && a.GWBSTree.Id == gWBSTree.Id));
            if (detail != null)
                detailList.Add(detail);
            return detail;

        }

        private void GetShowDetail()
        {


            VScheduleFilter vFilter = new VScheduleFilter();
            vFilter.ShowDialog();

            ObjectQuery oq = vFilter.TempOQ;
            if (oq == null)
                return;

            oq.AddCriterion(Expression.Eq("Master.Id", CurBillMaster.Id));

            showDetail = model.ProductionManagementSrv.GetShowWeekScheduleDetails(oq).OfType<WeekScheduleDetail>().ToList<WeekScheduleDetail>();
            
            MaintainShowDetail();
            SetOrderShowDetail_Load();
            SetShowDetailParentDateTime();
            ModelToView();

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

        private bool showDetailContains(WeekScheduleDetail item)
        {
            var details = from p in showDetail
                          where p.Id == item.Id
                          select p;

            if (details != null && details.Count() > 0)
                return true;
            else
                return false ;

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
        private void LoadPlan()
        {

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", CurBillMaster.Id));

            Disjunction dis = new Disjunction();
            dis.Add(Expression.And(Expression.Eq("State", DocumentState.Edit), Expression.Not(Expression.Eq("PlannedBeginDate", defaultTime))));
            dis.Add(Expression.And(Expression.Eq("State", DocumentState.InExecute), Expression.Eq("Level", 1)));

            oq.AddCriterion(dis);

            showDetail = model.ProductionManagementSrv.GetShowWeekScheduleDetails(oq).OfType<WeekScheduleDetail>().ToList<WeekScheduleDetail>();

            SetOrderShowDetail_Load();

            MaintainShowDetail();

            this.rbtnWSDOrder.Checked = true;
            this.btnUp.Enabled = true;
            this.btnDown.Enabled = true;

            ModelToView();
            //加载明细

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
                           orderby (wsdorder == WSDOrderSet.GWBS ? d.OrderNo : d.WSDOrderNo) ascending
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

        private void getWeekChildDtl_Load(WeekScheduleDetail parentDtl, List<WeekScheduleDetail> listDtl, ref ArrayList listResult)
        {

            var queryDtl = from d in listDtl.OfType<WeekScheduleDetail>()
                           where (d.ParentNode != null && d.ParentNode.Id == parentDtl.Id)
                           orderby (wsdorder == WSDOrderSet.GWBS ? d.OrderNo : d.WSDOrderNo) ascending
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
                           orderby (wsdorder== WSDOrderSet.GWBS? d.OrderNo:d.WSDOrderNo) ascending
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
                           orderby (wsdorder == WSDOrderSet.GWBS ? d.OrderNo :d.WSDOrderNo) ascending
                           orderby d.Level ascending
                           select d;
            parentDtl.ChildCount = queryDtl.Count();

            foreach (WeekScheduleDetail dtl in queryDtl)
            {
                listResult.Add(dtl);

                getWeekChildDtl(dtl, listDtl, ref listResult);
            }
        }

        private IList GetProductionSchedules()
        {
            if (projectInfo == null)
            {
                return null;
            }

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("ExecScheduleType", EnumExecScheduleType.总体进度计划));
            oq.AddOrder(Order.Desc("CreateDate"));

            return model.ProductionManagementSrv.GetWeekScheduleMaster(oq);
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

        private WeekScheduleMaster NewMaster()
        {
            WeekScheduleMaster master = new WeekScheduleMaster();
            master.PlanName = projectInfo.Name + "总进度计划";
            master.ProjectId = projectInfo.Id;
            master.ProjectName = projectInfo.Name;
            master.ExecScheduleType = EnumExecScheduleType.总体进度计划;

            master.CreateDate = model.ProductionManagementSrv.GetServerTime();
            master.HandlePerson = ConstObject.LoginPersonInfo;
            master.HandlePersonName = ConstObject.LoginPersonInfo.Name;
            master.CreatePerson = ConstObject.LoginPersonInfo;
            master.CreatePersonName = ConstObject.LoginPersonInfo.Name;
            master.HandleOrg = ConstObject.TheOperationOrg;
            master.HandlePersonSyscode = ConstObject.TheOperationOrg.SysCode;

            master.DocState = DocumentState.Edit;


            return master;
        }

        /// <summary>
        /// 显示计划头信息到界面
        /// </summary>
        /// <param name="master"></param>
        private void FreshMasterInfo(WeekScheduleMaster master)
        {
            this.txtPlanName.Text = master.PlanName;
            this.txtRemark.Text = master.Descript;

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

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
            if (CurBillMaster.DocState == DocumentState.Edit)
            {
                base.ModifyView();

                return true;
            }
            string message = "此单状态为非制定状态，不能修改！";
            MessageBox.Show(message);
            return false;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public override bool SaveView()
        {
            try
            {
                return this.SaveOrSubmitBill(1);
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        public override bool SubmitView()
        {
            try
            {
                return this.SaveOrSubmitBill(2);
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }

        //[optrType=1 保存][optrType=2 提交]
        private bool SaveOrSubmitBill(int optrType)
        {
            try
            {
                if (!ValidView())
                {
                    return false;
                }

                LogData log = new LogData();
                if (string.IsNullOrEmpty(CurBillMaster.Id))
                {
                    if (optrType == 2)
                    {
                        log.OperType = "新增提交";
                    }
                    else
                    {
                        log.OperType = "新增保存";
                    }
                }
                else
                {
                    if (optrType == 2)
                    {
                        log.OperType = "修改提交";
                    }
                    else
                    {
                        log.OperType = "修改保存";
                    }

                    CurBillMaster = model.ProductionManagementSrv.GetWeekScheduleMasterById(CurBillMaster.Id);
                    //model.DeleteByIList((IList)lstDelObj);
                }

                ViewToMaster(false);

                ViewToDetails();

                var errorTimeItem =
                    CurBillMaster.Details.ToList().Find(a => a.PlannedBeginDate > a.PlannedEndDate);
                if (errorTimeItem != null)
                {
                    MessageBox.Show(string.Format("任务【{0}】的计划开始大于结束时间，请重新设置", errorTimeItem.GWBSTreeName));
                    return false;
                }

                if (optrType == 2)
                {
                    //var emptyTimeItem =
                    //    CurBillMaster.Details.ToList().Find(
                    //        a => a.PlannedBeginDate == defaultTime || a.PlannedEndDate == defaultTime);
                    //if (emptyTimeItem != null)
                    //{
                    //    MessageBox.Show(string.Format("任务【{0}】没有设置计划的开始或结束时间", emptyTimeItem.GWBSTreeName));
                    //    return false;
                    //}

                    //CurBillMaster.DocState = DocumentState.InAudit;
                    //CurBillMaster.SubmitDate = DateTime.Now;
                    foreach (var item in showDetail)
                    {
                        if (item.PlannedBeginDate != defaultTime && item.PlannedEndDate != defaultTime && item.PlannedDuration != (Decimal)0)
                            item.State = DocumentState.InExecute;
                    }
                }

                FlashScreen.Show(string.Format("正在{0}进度计划，请稍候...", optrType == 2 ? "提交" : "保存"));

                showDetail = model.ProductionManagementSrv.SaveUpdateWeekPlanDtl(showDetail as List<WeekScheduleDetail>, (IList)lstDelObj) as IList<WeekScheduleDetail>;
                this.lstDelObj.Clear();
                log.BillId = CurBillMaster.Id;
                log.BillType = "总进度计划";
                log.Code = "";
                log.Descript = CurBillMaster.Descript;
                log.OperPerson = ConstObject.LoginPersonInfo.Name;
                log.ProjectName = CurBillMaster.ProjectName;
                StaticMethod.InsertLogData(log);



                FlashScreen.Close();

                return true;
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("保存出错。\n" + ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 保存数据前校验数据
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            if (CurBillMaster == null)
            {
                MessageBox.Show("当前没有要操作的计划!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            //if (CurBillMaster.DocState != DocumentState.Edit)
            //{
            //    MessageBox.Show("当前计划状态为“" + ClientUtil.GetDocStateName(CurBillMaster.DocState) + "”,只能修改“编辑”状态的计划！");
            //    return false;
            //}

            return true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                CurBillMaster = model.ProductionManagementSrv.GetObjectById(typeof(WeekScheduleMaster), CurBillMaster.Id) as WeekScheduleMaster;

                if (CurBillMaster.DocState == DocumentState.Edit)
                {
                    if (!model.ProductionManagementSrv.DeleteByDao(CurBillMaster))
                        return false;

                    LogData log = new LogData();
                    log.BillId = CurBillMaster.Id;
                    log.BillType = "总进度计划";
                    log.Code = "";
                    log.OperType = "删除";
                    log.Descript = CurBillMaster.Descript;
                    log.OperPerson = ConstObject.LoginPersonInfo.Name;
                    log.ProjectName = CurBillMaster.ProjectName;
                    StaticMethod.InsertLogData(log);

                    CurBillMaster = null;


                    return true;
                }

                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(CurBillMaster.DocState) + "】，不能删除！");
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("删除失败，异常信息：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        private void ClearView()
        {
            flexGrid.Rows = 1;

            txtFindKey.Text = "";
            txtFindKey.Tag = null;
            txtRemark.Text = "";

        }

        /// <summary>
        /// 撤销
        /// </summary>
        /// <returns></returns>
        public override bool CancelView()
        {
            try
            {
                switch (ViewState)
                {
                    case MainViewState.Modify:

                        if (!string.IsNullOrEmpty(CurBillMaster.Id))
                        {
                            //重新查询数据
                            ObjectQuery oq = new ObjectQuery();
                            oq.AddCriterion(Expression.Eq("Id", CurBillMaster.Id));
                            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                            CurBillMaster = model.ObjectQuery(typeof(WeekScheduleMaster), oq)[0] as WeekScheduleMaster;

                            ModelToView();
                        }
                        else
                        {

                        }
                        break;
                    case MainViewState.AddNew:

                        if (CurBillMaster != null && CurBillMaster.DocState == DocumentState.Edit)
                        {
                            CurBillMaster = model.ProductionManagementSrv.GetObjectById(typeof(WeekScheduleMaster), CurBillMaster.Id) as WeekScheduleMaster;

                            if (!model.ProductionManagementSrv.DeleteByDao(CurBillMaster))
                                return false;

                            CurBillMaster = null;


                            return true;
                        }

                        break;
                    default:

                        ClearView();

                        break;
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据撤销错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public override void RefreshView()
        {
            try
            {
                if (ViewState == MainViewState.Modify)
                {
                    if (MessageBox.Show("当前核算单处于编辑状态，需要保存修改吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (!SaveView())
                        {
                            return;
                        }
                    }
                }

                //重新查询加载数据
                //重新查询数据
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", CurBillMaster.Id));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                CurBillMaster = model.ObjectQuery(typeof(WeekScheduleMaster), oq)[0] as WeekScheduleMaster;

                ModelToView();

                RefreshControls(MainViewState.Browser);
            }
            catch (Exception e)
            {
                MessageBox.Show("数据刷新错误：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        private void ModelToView()
        {
            FreshMasterInfo(CurBillMaster);

            FillFlex();
        }

        //创建新版本



        //复制计划


        //提交
        void btnSubmit_Click(object sender, EventArgs e)
        {
            //if (SubmitView())
            //    MessageBox.Show("提交成功！");
            VScheduleSubmit vss = new VScheduleSubmit(this.CurBillMaster);

            vss.ShowDialog();
            //ShowDialog(vss);
        }

        //保存
        void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveView())
                MessageBox.Show("保存成功！");
        }

        private void ViewToMaster(bool isNewVersion)
        {
            if (!isNewVersion)//修改版本名称
                CurBillMaster.PlanName = this.txtPlanName.Text.Trim();

            CurBillMaster.Descript = txtRemark.Text;
        }

        private void ViewToDetails()
        {
            if (string.IsNullOrEmpty(CurBillMaster.Id))
            {
                return;
            }

            for (int i = 1; i < flexGrid.Rows; i++)
            {
                string detailId = flexGrid.Cell(i, 0).Tag;

                if (detailId == null || detailId.Equals(""))
                    continue;

                WeekScheduleDetail detail = null;
                foreach (WeekScheduleDetail tempDetail in CurBillMaster.Details)
                {
                    if (detailId == tempDetail.Id)
                    {
                        detail = tempDetail;
                        break;
                    }
                }
                //计划开始时间
                string PlannedBeginDateStr = flexGrid.Cell(i, endImageCol + 1).Text;
                if (PlannedBeginDateStr != null && !PlannedBeginDateStr.Equals(""))
                {
                    detail.PlannedBeginDate = DateTime.Parse(PlannedBeginDateStr);
                }
                else
                {
                    detail.PlannedBeginDate = new DateTime(1900, 1, 1);
                }
                //计划结束时间
                string PlannedEndDateStr = flexGrid.Cell(i, endImageCol + 2).Text;
                if (PlannedEndDateStr != null && !PlannedEndDateStr.Equals(""))
                {
                    detail.PlannedEndDate = DateTime.Parse(PlannedEndDateStr);
                }
                else
                {
                    detail.PlannedEndDate = new DateTime(1900, 1, 1);
                }

                //计划工期
                detail.PlannedDuration = flexGrid.Cell(i, endImageCol + 3).IntegerValue;

                //工期计量单位
                detail.ScheduleUnit = flexGrid.Cell(i, endImageCol + 4).Text;

                //计划说明
                detail.Descript = flexGrid.Cell(i, endImageCol + 5).Text;

                CurBillMaster.AddDetail(detail);
            }
        }

        private void txtFindKey_TextChanged(object sender, EventArgs e)
        {
            findDetail = null;
            lbFindCount.Text = string.Empty;
        }

        //删除明细
        void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteDetailBatch();
        }

        private void DeleteDetailBatch()
        {
            int rowcount = flexGrid.Selection.LastRow - flexGrid.Selection.FirstRow + 1;
            int colCount = flexGrid.Selection.LastCol - flexGrid.Selection.FirstCol + 1;

            string str_GWBSTreeNames = "";
            string ErrMsg = "";

            if (!DeleteDetailBatchCheck(rowcount, ref  str_GWBSTreeNames, ref  ErrMsg))
            {
                MessageBox.Show(ErrMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult dlgRst = MessageBox.Show("确定要移除【\r\n" + str_GWBSTreeNames + "\r\n】及其下属进度计划吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlgRst != DialogResult.Yes)
                return;

            try
            {
                FlashScreen.Show("正在执行移除操作,请稍候......");
                ArrayList list_needDel = new ArrayList();
                int rowindex = 0;
                for (int i = 0; i < rowcount; i++)
                {
                    rowindex = flexGrid.Selection.FirstRow + i;
                    string detailId = flexGrid.Cell(rowindex, 0).Tag;
                    DeleteDetailRow(detailId, ref list_needDel);
                }
                foreach (WeekScheduleDetail item in list_needDel)
                {
                    showDetail.Remove(item);
                }
                //SetShowDetailParentDateTime();
                FlashScreen.Close();
                this.flexGrid.FindForm().Focus();
                FillFlex();
                MessageBox.Show("移除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                this.flexGrid.FindForm().Focus();
                MessageBox.Show("移除失败.\n" + ex.Message);
                return;
            }

        }

        private bool DeleteDetailBatchCheck(int rowcount, ref string str_GWBSTreeNames, ref string ErrMsg)
        {
            str_GWBSTreeNames = "";
            ErrMsg = "";
            int rowindex = 0;
            for (int i = 0; i < rowcount; i++)
            {
                rowindex = flexGrid.Selection.FirstRow + i;
                string detailId = flexGrid.Cell(rowindex, 0).Tag;

                if (detailId != null && !detailId.Equals(""))
                {
                    WeekScheduleDetail detail = (WeekScheduleDetail)detailHashtable[detailId];
                    if (detail == null)
                    {
                        ErrMsg = "选择计划明细不存在(或已被其他操作员删除),请重载该计划！";
                        return false;
                    }
                    else if (detail.Level == 1)
                    {
                        ErrMsg = "根节点不能删除！";
                        return false;
                    }

                    if (str_GWBSTreeNames != "")
                        str_GWBSTreeNames += "\r\n";
                    str_GWBSTreeNames += detail.GWBSTreeName;
                }
            }
            return true;
        }

        private void DeleteDetailRow(string detailId, ref ArrayList list_needDel)
        {
            WeekScheduleDetail detail = (WeekScheduleDetail)detailHashtable[detailId];
            var list = from p in showDetail
                       where p.SysCode.StartsWith(detail.SysCode)
                       select p;
            foreach (WeekScheduleDetail item in list)
            {
                string XX = item.GWBSTreeName;
                WeekScheduleDetail find_inNeedDel = list_needDel.OfType<WeekScheduleDetail>().ToList().Find(a => a.Id == item.Id);
                if (find_inNeedDel == null)
                {
                    list_needDel.Add(item);
                    //移除结点不删除前置任务
                    //AddRalationToList(item);
                }
            }
            //SetParentRowDateTime(detail);
        }

        private void AddRalationToList(WeekScheduleDetail wsd)
        {
            if (wsd.RalationDetails == null || wsd.RalationDetails.Count == 0)
                return;
            foreach (var item in wsd.RalationDetails)
            {
                WeekScheduleRalation wsdr_find = lstDelObj.Find(a => a == item);
                if (wsdr_find != null)
                    continue;
                lstDelObj.Add(item);
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
                detail.IsExpand = !isVisble;
                childs = (from a in detailHashtable.Values.OfType<WeekScheduleDetail>()
                          where a.SysCode.StartsWith(detail.SysCode) && a.Id != detailId
                          select a
                         ).Count();
            }

            SetRowVisible(activeRowIndex + 1, activeRowIndex + childs, isVisble);

            flexGrid.AutoRedraw = true;
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

        private bool CheckCanCellChange(WeekScheduleDetail detail,int col, string sStartTime, string sFinishTime, string sDuration, ref string msg)
        {
            bool b_rtn = true;
            msg = "";


            return b_rtn;
        }

        //计算工期
        private void flexGrid_CellChange(object sender, Grid.CellChangeEventArgs e)
        {
            if (e.Row <= 0 || e.Col <= endImageCol || e.Col >= 24)
            {
                return;
            }

            var detailId = flexGrid.Cell(e.Row, 0).Tag;
            if (!detailHashtable.ContainsKey(detailId))
                return;
            var detail = detailHashtable[detailId] as WeekScheduleDetail;
            //this.flexGrid_detailId = detail.Id;
        

            string sStartTime = flexGrid.Cell(e.Row, endImageCol + 1).Text;
            string sFinishTime = flexGrid.Cell(e.Row, endImageCol + 2).Text;
            string sDuration = flexGrid.Cell(e.Row, endImageCol + 3).Text;

            string msg = "";
            bool isCanChange = CheckCanCellChange(detail,e.Col, sStartTime, sFinishTime, sDuration, ref msg);

            if (!isCanChange)
            {
                MessageBox.Show(msg);
                return;
            }

            msg = "";
            if (e.Col == endImageCol + 1) //计划开始时间 ----->结束时间
            {

                if (!string.IsNullOrEmpty(sStartTime))
                {
                    detail.PlannedBeginDate = ClientUtil.ToDateTime(sStartTime);
                    if (!string.IsNullOrEmpty(sDuration))
                    {
                        if (IsHaveFrontTask(detail, ref msg))
                        {
                            MessageBox.Show("该节点已经设置前置任务，请通过以下任务修改：\r\n" + msg);
                            return;
                        }
                        detail.PlannedEndDate = (DateTime)GetBenginOrEndDate(ClientUtil.ToDateTime(sStartTime), ClientUtil.ToInt(sDuration), DateRel.GetEndDate);

                    }
                }
                else
                    detail.PlannedBeginDate = defaultTime;
            }
            else if (e.Col == endImageCol + 2)//计划结束时间 ---->工期
            {
                if (!string.IsNullOrEmpty(sFinishTime))
                {
                    detail.PlannedEndDate = ClientUtil.ToDateTime(sFinishTime);
                    if (!string.IsNullOrEmpty(sStartTime))
                        detail.PlannedDuration = (int)GetWorkDays(ClientUtil.ToDateTime(sStartTime), ClientUtil.ToDateTime(sFinishTime));
                }
                else
                    detail.PlannedEndDate = defaultTime;
            }
            else if (e.Col == endImageCol + 3)//计划工期 ------>结束时间
            {
                if (!string.IsNullOrEmpty(sDuration))
                {
                    detail.PlannedDuration = ClientUtil.ToInt(sDuration);
                    if (!string.IsNullOrEmpty(sStartTime))
                        detail.PlannedEndDate = (DateTime)GetBenginOrEndDate(ClientUtil.ToDateTime(sStartTime), ClientUtil.ToInt(sDuration), DateRel.GetEndDate);
                }
                else
                    detail.PlannedDuration = (decimal)0;
            }

            //SetParentRowDateTime(detail);
            MaintainWorkDateInfoFollow(detail);
            //SetShowDetailParentDateTime();
            //SetParentRowDateTime(detail);
            FillFlex_CellChange();

        }

        private bool IsHaveFrontTask(WeekScheduleDetail detail, ref string msg)
        {
            msg = "";
            if (detail.RalationDetails != null && detail.RalationDetails.Count > 0)
            {

                foreach (var item in detail.RalationDetails)
                {
                    if (msg != "")
                        msg += ",";
                    msg += GetGWBSTreeNameByWsdid(item.FrontWeekScheduleDetail.Id);
                }


            }
            return !(msg == "");
        }

        private string GetGWBSTreeNameByWsdid(string wsdid)
        {
            string str_Rtn = "";

            WeekScheduleDetail wsd_find = CurBillMaster.Details.ToList<WeekScheduleDetail>().Find(a => a.Id == wsdid);
            if (wsd_find != null)
            {
                str_Rtn += wsd_find.GWBSTreeName;
            }
            return str_Rtn;
        }

        //导出
        void btnExportToMPP_Click(object sender, EventArgs e)
        {

            if (CurBillMaster == null)
            {
                MessageBox.Show("当前没有要导出的计划!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "项目 (*.MPP)|*.MPP";
            sfd.RestoreDirectory = true;
            sfd.FileName = "总进度计划_" + CurBillMaster.PlanName + "_" + string.Format("{0:yyyy年MM月dd日HH点mm分}", DateTime.Now);
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string fileName = sfd.FileName;
                if (System.IO.File.Exists(fileName))
                {
                    IList list = model.ProductionManagementSrv.GetWeekChilds(CurBillMaster);
                    MSProjectUtil.UpdateProject(fileName, list, listDtlIds, this.Handle);
                }
                else
                {
                    IList list = model.ProductionManagementSrv.GetWeekChilds(CurBillMaster);
                    MSProjectUtil.CreateMPP(fileName, list, listDtlIds, this.Handle);
                }
            }


        }

        //选择GWBS
        void btnGWBS_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree(), true);
            frm.IsSelectSingleNode = true;
            frm.IsSelectTreeNodes = true; //获取的节点是 以选择的节点为树根的一棵树
            frm.ShowDialog();
            List<TreeNode> list = frm.SelectResult;
            CopyLevel = frm.RtnCopyLevel;//拷贝深度
            if (list != null && list.Count > 0)
            {
                try
                {
                  
                    FlashScreen.Show("正在生成进度计划......");

                    TreeNode selTreeNode = list[0];
                    GWBSTree selWBS = selTreeNode.Tag as GWBSTree;

                    WeekScheduleDetail wsd_selWBS = showDetail.ToList().Find(a => (a.GWBSTree != null && a.GWBSTree.Id == selWBS.Id));

                    WeekScheduleDetail parentNode = null;
                    //有父节点
                    if (selTreeNode.Parent != null)
                    {
                        //父节点
                        GWBSTree parentWBS = selTreeNode.Parent.Tag as GWBSTree;


                        WeekScheduleDetail wsd_parentWBS = showDetail.ToList().Find(a => (a.GWBSTree != null && a.GWBSTree.Id == parentWBS.Id));

                        //选择节点的父节点在当前计划明细中存在
                        if (wsd_parentWBS != null)
                        {
                            parentNode = wsd_parentWBS;
                        }
                        else//不存在
                        {
                            //所有的父系节点  第一个为detail中每个节点的子节点
                            List<GWBSTree> GWBSTree_recordFind = new List<GWBSTree>();

                            //detail中最近直系 赋值给parentNode
                            FindUpperLevelWeekScheduleDetail(ref parentNode, selWBS, ref GWBSTree_recordFind);


                            WeekScheduleDetail farther = null;
                            IList detailList = new ArrayList();
                            for (int i = 0; i < GWBSTree_recordFind.Count; i++)
                            {

                                if (i == 0)
                                {
                                    farther = parentNode;
                                }
                                if (i == GWBSTree_recordFind.Count - 1)
                                    parentNode = AddWeekScheduleDetail(GWBSTree_recordFind[i], ref  farther, CurBillMaster, ref detailList);
                                else
                                    AddWeekScheduleDetail(GWBSTree_recordFind[i], ref  farther, CurBillMaster, ref detailList);
                            }

                            //添加明细

                            foreach (WeekScheduleDetail item in detailList)
                            {
                                //因为事后添加，故有可能加重复，需要加测
                                WeekScheduleDetail wsd_find = showDetail.ToList<WeekScheduleDetail>().Find(a => a.Id == item.Id);
                                if (wsd_find != null)
                                    continue;
                                if (wsdorder == WSDOrderSet.WSD)
                                    item.WSDOrderNo = GetWSDOrderNoMax(item, showDetail.ToList<WeekScheduleDetail>());
                                showDetail.Add(item);
                            }

                        }
                    }
                    else //选择项目任务根节点
                    {
                        parentNode = GetScheduleDetailRootNode();
                    }

                    if (parentNode == null)
                    {
                        MessageBox.Show("在计划中未找到项目任务【" + selWBS.Name + "】的父节点，添加计划失败！");
                        return;
                    }

                    //?
                    SavePlanDtl(selTreeNode, parentNode, CurBillMaster);
                }
                catch (Exception ex)
                {
                    FlashScreen.Close();
                    MessageBox.Show("生成进度计划出错。\n" + ex.Message);
                    return;
                }
                finally
                {
                    FlashScreen.Close();
                }

                FillFlex();
            }
        }

        /// <summary>
        /// 找出选择节点 最近的一个在Detail中的父节点
        /// </summary>
        /// <param name="selWBS"></param>
        /// <returns></returns>
        private void FindUpperLevelWeekScheduleDetail(ref WeekScheduleDetail parentNode, GWBSTree selWBS, ref List<GWBSTree> GWBSTree_recordFind)
        {

            parentNode = CurrMsDtlHaveThisNode(selWBS);

            if (parentNode != null)
                return;
            else
            {
                if (selWBS.ParentNode == null)
                {
                    parentNode = showDetail.First(a => a.Level == 1 && a.ParentNode == null);
                    return;
                }

                string str_parentWBSId = selWBS.ParentNode.Id;

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
                oq.AddCriterion(Expression.Eq("Id", str_parentWBSId));
                GWBSTree parentWBS = null;
                IList listGWBSTree = model.ProductionManagementSrv.ObjectQuery(typeof(GWBSTree), oq);

                if (listGWBSTree != null && listGWBSTree.Count > 0)
                    parentWBS = listGWBSTree[0] as GWBSTree;

                if (parentWBS.ParentNode == null && parentWBS.Level == 1)
                {
                    parentNode = showDetail.ToList().Find(a => a.Level == 1 && a.ParentNode == null);
                    if (parentNode == null)
                    {
                        parentNode = CurBillMaster.Details.ToList<WeekScheduleDetail>().Find(a => a.Level == 1 && a.ParentNode == null);
                        //根节点不在showDetail ，需要添加
                        GWBSTree_recordFind.Insert(0, parentWBS);
                    }
                    return;
                }

                if (GWBSTree_recordFind == null || GWBSTree_recordFind.Count <= 0)
                    GWBSTree_recordFind.Add(parentWBS);
                else
                    GWBSTree_recordFind.Insert(0, parentWBS);

                FindUpperLevelWeekScheduleDetail(ref parentNode, parentWBS, ref GWBSTree_recordFind);

            }
        }

        /// <summary>
        /// 根据GWBSTree在CurBillMaster.Details匹配，匹配到返回匹配到的WeekScheduleDetail，否则返回null
        /// </summary>
        /// <param name="thisWBS">GWBSTree</param>
        /// <returns></returns>
        private WeekScheduleDetail CurrMsDtlHaveThisNode(GWBSTree thisWBS)
        {
            var details = from p in showDetail
                          where p.GWBSTree == thisWBS
                          select p;

            if (details != null && details.Count() > 0)
                return details.ElementAt(0);
            else
                return null;
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

                //恢复findDetail的前景背景色
                SetflexGridRowColor(findDetail.RowIndex, findDetail);

                //flexGrid.Cell(findDetail.RowIndex, findDetail.Level + 1).BackColor = SystemColors.Control;
                //flexGrid.Cell(findDetail.RowIndex, findDetail.Level + 1).ForeColor = Color.Black;
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

        private void SavePlanDtl(TreeNode treeNode, WeekScheduleDetail parentNode, WeekScheduleMaster master)
        {
            IList detailList = new ArrayList();
            int coypedLevel = 0;
            AddPlanDtl(treeNode, parentNode, ref detailList, master, ref coypedLevel);

            if (master.Details.Count > 1)
            {
                foreach (WeekScheduleDetail item in detailList)
                {
                    WeekScheduleDetail detail = showDetail == null ? null : showDetail.ToList<WeekScheduleDetail>().Find(a => (a.GWBSTree != null && a.GWBSTree.Id == item.GWBSTree.Id));
                    if (detail == null)
                    {
                        if (wsdorder == WSDOrderSet.WSD)
                            item.WSDOrderNo = GetWSDOrderNoMax(item, showDetail.ToList<WeekScheduleDetail>());
                        showDetail.Add(item);
                    }
                }
            }
            else
            {
                model.ProductionManagementSrv.SaveWeekPlanDtl(detailList);
            }
        }

        private void AddPlanDtl(TreeNode treeGWBSNode, WeekScheduleDetail parentPlanDtlNode, ref IList detailList, WeekScheduleMaster master, ref int coypedLevel)
        {
            //CopyLevel==0表示全部拷贝
            if (CopyLevel == 0 || ++coypedLevel <= CopyLevel)
            {
                GWBSTree Tree_treeGWBSNode = treeGWBSNode.Tag as GWBSTree;
                WeekScheduleDetail detail = null;
                #region
                if (master.Details.Count > 1)
                    detail = master.Details.ToList().Find(a => (a.GWBSTree != null && a.GWBSTree.Id == Tree_treeGWBSNode.Id));
                #endregion
                #region 递归方式同步GWBS  目前程序换做非递归方式将不会调用这里
                else
                {
                    detail = new WeekScheduleDetail();
                    detail.GWBSTree = treeGWBSNode.Tag as GWBSTree;
                    if (detail.GWBSTree != null)
                    {
                        detail.GWBSTreeName = detail.GWBSTree.Name;
                        detail.GWBSTreeSysCode = detail.GWBSTree.SysCode;
                        detail.NodeType = detail.GWBSTree.CategoryNodeType;
                        detail.OrderNo = (int)detail.GWBSTree.OrderNo;
                        detail.Level = detail.GWBSTree.Level;
                        detail.IsFixed = detail.GWBSTree.IsFixed;
                        if (detail.GWBSTree.ProductionCuringNode)
                        {
                            detail.ProductionCuringNode = detail.GWBSTree.ProductionCuringNode;
                            detail.PlannedBeginDate = (DateTime)detail.GWBSTree.ProCurBeginDate;
                            detail.PlannedEndDate = (DateTime)detail.GWBSTree.ProCurEndDate;
                        }
                    }
                    detail.ParentNode = parentPlanDtlNode;

                    detail.Master = master;
                    detail.State = DocumentState.Edit;
                    detail.ScheduleUnit = "天";
                    detail.ProjectId = master.ProjectId;
                    detail.ProjectName = master.ProjectName;
                }
                #endregion
                bool has_detailList = false;
                foreach (WeekScheduleDetail item in detailList)
                {
                    if (item.GWBSTree == null)
                        continue;

                    if (item.GWBSTree.Id == Tree_treeGWBSNode.Id)
                    {
                        has_detailList = true;
                        break;
                    }
                }

                if (detail != null && !has_detailList)
                    detailList.Add(detail);

                int level = coypedLevel;
                foreach (TreeNode tn in treeGWBSNode.Nodes)
                {
                    level = coypedLevel;//每个子节点的层级单独计算
                    AddPlanDtl(tn, detail, ref detailList, master, ref level);
                }
            }
        }

        private void SetRowBold(int rowIndex, bool isBold)
        {
            for (var i = 0; i < flexGrid.Cols; i++)
            {
                flexGrid.Cell(rowIndex, i).FontBold = isBold;
            }
        }

        private void FillFlex()
        {
            int toprow = this.flexGrid.TopRow;
            this.flexGrid.CellChange -= new Grid.CellChangeEventHandler(flexGrid_CellChange);
            detailHashtable.Clear();

            flexGrid.AutoRedraw = false;
            flexGrid.Rows = 1;
            //flexGrid.Column(endImageCol + 3).Locked = true;

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

                    ////节点类型【0：根节点；1：中间节点；2：叶节点】
                    //flexGrid.Cell(rowIndex, endImageCol + 1).Tag = detail.NodeType.GetHashCode().ToString();
                    ////当前行是否只读 【0：否；1：是】
                    //flexGrid.Cell(rowIndex, endImageCol + 2).Tag = (detail.ChildCount > 0 || detail.State != EnumScheduleDetailState.编辑).GetHashCode().ToString();
                    flexGrid.Cell(rowIndex, endImageCol + 3).Tag = detail.GWBSTreeSysCode;
                    SetRowBold(rowIndex, detail.ChildCount > 0);

                    Range rangeTemp = flexGrid.Range(rowIndex, detail.Level + 1, rowIndex, endImageCol);
                    rangeTemp.Merge();

                    flexGrid.Cell(rowIndex, detail.Level + 1).Text = (detail.GWBSTreeName ?? projectInfo.Name + "总进度计划") + (detail.IsFixed == 1 ? "【合】" : "") + (detail.ProductionCuringNode ? "【固】" : "");
                    //当前行任务名称所在列序号
                    flexGrid.Cell(rowIndex, endImageCol + 4).Tag = (detail.Level + 1).ToString();

                    //行是否只读
                    flexGrid.Row(rowIndex).Locked = detail.State != DocumentState.Edit;

                    //计划开始时间
                    if (detail.PlannedBeginDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 1).Text = ""; //"计划开始时间";//20
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 1).Text = detail.PlannedBeginDate.ToShortDateString(); //"计划开始时间";//20
                    }

                    //计划结束时间
                    if (detail.PlannedEndDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 2).Text = ""; //"计划结束时间";//21
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 2).Text = detail.PlannedEndDate.ToShortDateString();
                    }


                    #region 将计算甩给 flexGrid_CellChanged 时间处理，这里只对工期赋值
                    if (detail.PlannedDuration == (decimal)0)
                        flexGrid.Cell(rowIndex, endImageCol + 3).Text = "";
                    else
                        flexGrid.Cell(rowIndex, endImageCol + 3).Text = ClientUtil.ToString(detail.PlannedDuration);

                    #endregion




                    flexGrid.Cell(rowIndex, endImageCol + 3).Alignment = AlignmentEnum.CenterCenter;

                    //工期计量单位                    
                    flexGrid.Cell(rowIndex, endImageCol + 4).Text = detail.ScheduleUnit;
                    flexGrid.Cell(rowIndex, endImageCol + 4).Alignment = AlignmentEnum.CenterCenter;

                    //前置任务
                    flexGrid.Cell(rowIndex, endImageCol + 5).Text = GetFrontTask(detail, list); //"前置任务";//27


                    //计划说明
                    flexGrid.Cell(rowIndex, endImageCol + 6).Text = detail.Descript; //"计划说明";//28
                    flexGrid.Cell(rowIndex, endImageCol + 6).Locked = false;

                    //实际开始时间
                    if (detail.ActualBeginDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 7).Text = "";
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 7).Text = detail.ActualBeginDate.ToShortDateString();
                    }

                    //实际结束时间
                    if (detail.ActualEndDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 8).Text = "";
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 8).Text = detail.ActualEndDate.ToShortDateString();
                    }

                    //实际工期
                    flexGrid.Cell(rowIndex, endImageCol + 9).Text = detail.ActualDuration == 0 ? "" : detail.ActualDuration.ToString();

 
                    SetflexGridRowColor(rowIndex, detail);

                    rowIndex = rowIndex + 1;
                }
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

            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();

            lbRowCount.Text = string.Format("共计 {0} 行", flexGrid.Rows);
           
            this.flexGrid.CellChange += new Grid.CellChangeEventHandler(flexGrid_CellChange);

            this.flexGrid.TopRow = toprow;

        }

        private void FillFlex_CellChange()
        {
            int toprow = this.flexGrid.TopRow;
            this.flexGrid.CellChange -= new Grid.CellChangeEventHandler(flexGrid_CellChange);
            detailHashtable.Clear();

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

                    flexGrid.Cell(rowIndex, detail.Level + 1).Text = (detail.GWBSTreeName ?? projectInfo.Name + "总进度计划") + (detail.IsFixed == 1 ? "【合】" : "") + (detail.ProductionCuringNode ? "【固】" : "");
                    //当前行任务名称所在列序号
                    flexGrid.Cell(rowIndex, endImageCol + 4).Tag = (detail.Level + 1).ToString();

                    //行是否只读
                    flexGrid.Row(rowIndex).Locked = detail.State != DocumentState.Edit;

                    //计划开始时间
                    if (detail.PlannedBeginDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 1).Text = ""; //"计划开始时间";//20
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 1).Text = detail.PlannedBeginDate.ToShortDateString(); //"计划开始时间";//20
                    }

                    //计划结束时间
                    if (detail.PlannedEndDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 2).Text = ""; //"计划结束时间";//21
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 2).Text = detail.PlannedEndDate.ToShortDateString();
                    }

                    //计划工期

                    #region 将计算甩给 flexGrid_CellChanged 时间处理，这里只对工期赋值
                    if (detail.PlannedDuration == (decimal)0)
                        flexGrid.Cell(rowIndex, endImageCol + 3).Text = "";
                    else
                        flexGrid.Cell(rowIndex, endImageCol + 3).Text = ClientUtil.ToString(detail.PlannedDuration);

                    #endregion

                    flexGrid.Cell(rowIndex, endImageCol + 3).Alignment = AlignmentEnum.CenterCenter;

                    //工期计量单位                    
                    flexGrid.Cell(rowIndex, endImageCol + 4).Text = detail.ScheduleUnit;
                    flexGrid.Cell(rowIndex, endImageCol + 4).Alignment = AlignmentEnum.CenterCenter;

                    //前置任务
                    flexGrid.Cell(rowIndex, endImageCol + 5).Text = GetFrontTask(detail, list); //"前置任务";//27


                    //计划说明
                    flexGrid.Cell(rowIndex, endImageCol + 6).Text = detail.Descript; //"计划说明";//28
                    flexGrid.Cell(rowIndex, endImageCol + 6).Locked = false;

                    //实际开始时间
                    if (detail.ActualBeginDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 7).Text = "";
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 7).Text = detail.ActualBeginDate.ToShortDateString();
                    }

                    //实际结束时间
                    if (detail.ActualEndDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 8).Text = "";
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 8).Text = detail.ActualEndDate.ToShortDateString();
                    }

                    //实际工期
                    flexGrid.Cell(rowIndex, endImageCol + 9).Text = detail.ActualDuration == 0 ? "" : detail.ActualDuration.ToString();

                    SetflexGridRowColor(rowIndex, detail);

                    rowIndex = rowIndex + 1;
                }
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

            lbRowCount.Text = string.Format("共计 {0} 行", flexGrid.Rows);

            this.flexGrid.CellChange += new Grid.CellChangeEventHandler(flexGrid_CellChange);

            this.flexGrid.TopRow = toprow;

        }

        private void SetflexGridRowColor(int rowIndex, WeekScheduleDetail detail)
        {
            if (detail != null)
            {
                flexGrid.Cell(rowIndex, detail.Level + 1).BackColor = detail.State == DocumentState.InExecute ? Color.DarkOliveGreen : SystemColors.Control;
                flexGrid.Cell(rowIndex, detail.Level + 1).ForeColor = detail.State == DocumentState.InExecute ? Color.White : SystemColors.ControlText;
                flexGrid.Cell(rowIndex, detail.Level).Alignment = AlignmentEnum.LeftCenter;

                flexGrid.Cell(rowIndex, endImageCol + 1).BackColor = detail.ProductionCuringNode ? Color.DarkOrange : SystemColors.Control;
                flexGrid.Cell(rowIndex, endImageCol + 1).ForeColor = detail.ProductionCuringNode ? Color.White : SystemColors.ControlText;

                flexGrid.Cell(rowIndex, endImageCol + 2).BackColor = detail.ProductionCuringNode ? Color.DarkOrange : SystemColors.Control;
                flexGrid.Cell(rowIndex, endImageCol + 2).ForeColor = detail.ProductionCuringNode ? Color.White : SystemColors.ControlText;
            }
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

        private void InitFlexGrid(int rows)
        {
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
            flexGrid.Column(endImageCol + 3).Width = 60;
            flexGrid.Column(endImageCol + 4).Width = 60;
            flexGrid.Column(endImageCol + 5).Width = 300;//计划说明
            flexGrid.Column(endImageCol + 6).Width = 300;//计划说明

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

            flexGrid.Cell(0, endImageCol + 1).Text = "计划开始时间";//20
            flexGrid.Cell(0, endImageCol + 2).Text = "计划结束时间";//21
            flexGrid.Cell(0, endImageCol + 3).Text = "计划工期";//22
            flexGrid.Cell(0, endImageCol + 4).Text = "工期单位";//23
            flexGrid.Cell(0, endImageCol + 5).Text = "前置任务";//27
            flexGrid.Cell(0, endImageCol + 6).Text = "计划说明";//28
            flexGrid.Cell(0, endImageCol + 7).Text = "实际开始时间";//24
            flexGrid.Cell(0, endImageCol + 8).Text = "实际结束时间";//25
            flexGrid.Cell(0, endImageCol + 9).Text = "实际工期";//26

            flexGrid.Column(endImageCol + 1).CellType = CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 2).CellType = CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 3).Mask = MaskEnum.Digital;
            flexGrid.Column(endImageCol + 7).CellType = CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 8).CellType = CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 9).Mask = MaskEnum.Digital;

            flexGrid.Column(endImageCol + 6).Locked = true;
            flexGrid.Column(endImageCol + 7).Locked = true;
            flexGrid.Column(endImageCol + 8).Locked = true;
            flexGrid.Column(endImageCol + 9).Locked = true;

            // Refresh
            flexGrid.AutoRedraw = true;
            //flexGrid.VerticalScroll.Visible = true;
            flexGrid.Refresh();
        }

        private int? GetWorkDays(DateTime? dt_begin, DateTime? dt_end)
        {
            if (dt_begin == null || dt_end == null || dt_begin == defaultTime || dt_end == defaultTime)
                return null;
            if (dt_begin > dt_end)
                return null;
            return ((DateTime)dt_end - (DateTime)dt_begin).Days + 1;
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
                    date_Rtn = ((DateTime)dt_date1).AddDays(1-(int)i_workdays) ;
                    break;
                case DateRel.GetEndDate:
                    if (dt_date1 == null || dt_date1 == defaultTime || i_workdays == null)
                        break;
                    date_Rtn = ((DateTime)dt_date1).AddDays((int)i_workdays-1);
                    break;
            }
            return date_Rtn;
        }

        private void MaintainWorkDateInfo(ref DateTime? dt_begin, ref DateTime? dt_end, ref int? i_workdays)
        {
            if (dt_begin != null && dt_begin != defaultTime && dt_end != null && dt_end != defaultTime)
                i_workdays = (int)GetWorkDays(dt_begin, dt_end);

            else if (dt_begin != null && dt_begin != defaultTime && i_workdays != null)
                dt_end = (DateTime)GetBenginOrEndDate(dt_begin, i_workdays, DateRel.GetEndDate);

            else if (dt_end != null && dt_end != defaultTime && i_workdays != null)
                dt_begin = (DateTime)GetBenginOrEndDate(dt_end, i_workdays, DateRel.GetBeginDate);
        }

        private DateTime? ConvertToDateTime(string str)
        {
            DateTime dt = ClientUtil.ToDateTime(str);
            if (dt == null || dt == defaultTime)
                return null;
            else
                return dt;
        }

        private int? ConvertToInt(string str)
        {
            int i = ClientUtil.ToInt(str);
            if (i == 0)
                return null;
            else
                return i;
        }

        private DateTime? GetDateByDelay(DateTime? date1,int delaydays)
        {
            if (date1 != null && date1 != defaultTime)
                return ((DateTime)date1).AddDays(delaydays + 1);
            return null;
        }

    }
    enum DateRel
    {
        [Description("获取开始时间")]
        GetBeginDate = 1,
        [Description("获取结束时间")]
        GetEndDate = 2,
        [Description("获取工期")]
        GetWorkDays = 3
    }
    enum WSDOrderSet
    {
        [Description("按WBS排序")]
        GWBS = 1,
        [Description("按进度计划排序")]
        WSD = 2
    }

}
