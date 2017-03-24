using System;
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

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI
{
    public partial class VScheduleQueryBak : TBasicDataView
    {
        private MProductionMng model = new MProductionMng();
        private Hashtable detailHashtable = new Hashtable();
        private ProductionScheduleMaster CurBillMaster;
        //private ProductionScheduleDetail ChildRootNode;
        private string imageExpand = "imageExpand";
        private string imageCollapse = "imageCollapse";

        private int startImageCol = 1, endImageCol = 19;

        public VScheduleQueryBak()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }

        private void InitEvents()
        {
            cboScheduleType.SelectedIndexChanged += new EventHandler(cboScheduleType_SelectedIndexChanged);
            flexGrid.Click += new FlexCell.Grid.ClickEventHandler(flexGrid_Click);
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnExportToMPP.Click += new EventHandler(btnExportToMPP_Click);
        }

        void btnExportToMPP_Click(object sender, EventArgs e)
        {
            //OpenFileDialog ofg = new OpenFileDialog();
            if (CurBillMaster == null) {
                btnQuery_Click(sender, new EventArgs());
            }
            SaveFileDialog sfd = new SaveFileDialog();
            //openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            sfd.Filter = "项目 (*.MPP)|*.MPP";
            sfd.RestoreDirectory = true;
            sfd.FileName = CurBillMaster.ScheduleName;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string fileName = sfd.FileName;
                if (System.IO.File.Exists(fileName))
                {
                    IList list = model.ProductionManagementSrv.GetChilds(CurBillMaster);
                    MSProjectUtil mSProjectUtil = new MSProjectUtil();
                    mSProjectUtil.UpdateMPP(fileName, list);
                }
                else
                {
                    IList list = model.ProductionManagementSrv.GetChilds(CurBillMaster);
                    MSProjectUtil.CreateMPP(fileName, list, this.Handle);
                }
            }
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            EnumScheduleType enumScheduleType = (EnumScheduleType)Enum.Parse(typeof(EnumScheduleType), cboScheduleType.SelectedItem + "");
            oq.AddCriterion(Expression.Eq("ScheduleType", enumScheduleType));
            oq.AddCriterion(Expression.Eq("ProjectId", StaticMethod.GetProjectInfo().Id));
            oq.AddCriterion(Expression.Eq("ScheduleTypeDetail", cboScheduleTypeDetail.SelectedItem+""));

            if (cboScheduleCaliber.SelectedItem != null && !string.IsNullOrEmpty(cboScheduleCaliber.SelectedItem.ToString()))
            {
                oq.AddCriterion(Expression.Eq("ScheduleCaliber", cboScheduleCaliber.SelectedItem + ""));
            }

            CurBillMaster = model.ProductionManagementSrv.GetSchedulesByType(oq);
            if (CurBillMaster == null)
            {
                MessageBox.Show("未找到进度计划。");
                return;
            }
            FillFlex();
        }

        private IList ViewToDetails()
        {
            IList list = new ArrayList();
            for (int i = 1; i < flexGrid.Rows; i++)
            {
                string detailId = flexGrid.Cell(i, 0).Tag;
                if (detailId == null || detailId.Equals("")) continue;
                ProductionScheduleDetail detail = null;
                foreach (ProductionScheduleDetail tempDetail in CurBillMaster.Details)
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
                } else
                {
                    detail.PlannedBeginDate = new DateTime(1900, 1, 1);
                }
                //计划结束时间
                string PlannedEndDateStr = flexGrid.Cell(i, endImageCol + 2).Text;
                if (PlannedEndDateStr != null && !PlannedEndDateStr.Equals(""))
                {
                    detail.PlannedEndDate = DateTime.Parse(PlannedEndDateStr);
                } else
                {
                    detail.PlannedEndDate = new DateTime(1900, 1, 1);
                }
                //工期计量单位
                detail.ScheduleUnit = flexGrid.Cell(i, endImageCol + 3).Text;
                //计划工期
                detail.PlannedDuration = flexGrid.Cell(i, endImageCol + 4).IntegerValue;
                //实际开始时间
                string actualBeginDate = flexGrid.Cell(i, endImageCol + 5).Text;
                if (actualBeginDate != null && !actualBeginDate.Equals(""))
                {
                    detail.ActualBeginDate = DateTime.Parse(actualBeginDate);
                } else
                {
                    detail.ActualBeginDate = new DateTime(1900, 1, 1);
                }

                //实际结束时间
                string actualEndDate = flexGrid.Cell(i, endImageCol + 6).Text;
                if (actualEndDate != null && !actualEndDate.Equals(""))
                {
                    detail.ActualEndDate = DateTime.Parse(actualEndDate);
                } else
                {
                    detail.ActualEndDate = new DateTime(1900, 1, 1);
                }
                //实际工期
                detail.ActualDuration = flexGrid.Cell(i, endImageCol + 7).IntegerValue;
                //计划说明
                detail.TaskDescript = flexGrid.Cell(i, endImageCol + 8).Text;
                CurBillMaster.AddDetail(detail);
            }
            return list;
        }

        void flexGrid_Click(object Sender, EventArgs e)
        {
            if (flexGrid.ActiveCell.ImageKey == imageExpand)
            {
                flexGrid.ActiveCell.SetImage(imageCollapse);
                int activeRowIndex = flexGrid.ActiveCell.Row;

                string detailId = flexGrid.Cell(activeRowIndex, 0).Tag;
                int childs = model.ProductionManagementSrv.CountAllChildNodes(detailId);
                if (childs > 0)
                {
                    SetRowVisible(activeRowIndex + 1, activeRowIndex + childs, true);
                }

            } else if (flexGrid.ActiveCell.ImageKey == imageCollapse)
            {
                flexGrid.ActiveCell.SetImage(imageExpand);
                int activeRowIndex = flexGrid.ActiveCell.Row;

                string detailId = flexGrid.Cell(activeRowIndex, 0).Tag;
                int childs = model.ProductionManagementSrv.CountAllChildNodes(detailId);
                if (childs > 0)
                {
                    SetRowVisible(activeRowIndex + 1, activeRowIndex + childs, false);
                }
            }
        }

        private void SetRowVisible(int row1, int row2, bool value)
        {
            flexGrid.AutoRedraw = false;
            for (int i = row1; i <= row2; i++)
            {
                flexGrid.Row(i).Visible = value;
                if (value)
                {
                    for (int j = startImageCol; j <= endImageCol; j++)
                    {
                        if (flexGrid.Cell(i, j).ImageKey != null && !flexGrid.Cell(i, j).ImageKey.Equals(""))
                        {
                            flexGrid.Cell(i, j).SetImage(imageCollapse);
                            break;
                        }
                    }
                }
            }
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        void cboScheduleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string scheduleType = cboScheduleType.SelectedItem+"";
            if (scheduleType == "总进度计划")
            {
                //总进度计划类型
                cboScheduleTypeDetail.Items.Clear();
                VBasicDataOptr.InitScheduleType(cboScheduleTypeDetail, false);
                if (cboScheduleTypeDetail.Items.Count > 0)
                {
                    cboScheduleTypeDetail.SelectedIndex = 0;
                }
            }
            else
            {
                //总滚动进度计划类型
                cboScheduleTypeDetail.Items.Clear();
                VBasicDataOptr.InitScheduleTypeRolling(cboScheduleTypeDetail, false);
                if (cboScheduleTypeDetail.Items.Count > 0)
                {
                    cboScheduleTypeDetail.SelectedIndex = 0;
                }
            }
        }

        private void InitData()
        {
            InitFlexGrid(5);
            
            //进度计划口径
            VBasicDataOptr.InitScheduleCaliber(cboScheduleCaliber, true);
            if (cboScheduleCaliber.Items.Count > 0)
            {
                cboScheduleCaliber.SelectedIndex = 0;
            }

            if (cboScheduleType.Items.Count > 0)
            {
                cboScheduleType.SelectedIndex = 0;
            }
        }


        private void FillFlex()
        {
            flexGrid.AutoRedraw = false;
            flexGrid.Rows = 1;
            flexGrid.Column(endImageCol + 3).Locked = true;
            detailHashtable.Clear();
            IList list = model.ProductionManagementSrv.GetChilds(CurBillMaster);
            if (list != null && list.Count > 0)
            {
                flexGrid.Rows = list.Count+1;
                int rowIndex = 1;
                foreach (ProductionScheduleDetail detail in list)
                {
                    if (detail.State == EnumScheduleDetailState.失效)
                    {
                        flexGrid.Rows = flexGrid.Rows - 1;
                        continue;
                    }
                    flexGrid.Cell(rowIndex, 0).Tag = detail.Id;
                    detailHashtable.Add(detail.Id, detail);
                    flexGrid.Cell(rowIndex, detail.Level).SetImage(imageCollapse);
                    FlexCell.Range rangeTemp = flexGrid.Range(rowIndex, detail.Level + 1, rowIndex, endImageCol);
                    rangeTemp.Merge();
                    //rangeTemp.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                    flexGrid.Cell(rowIndex, detail.Level + 1).Text = (detail.GWBSTreeName == null) ? "进度计划" : detail.GWBSTreeName;
                    //计划开始时间
                    if(detail.PlannedBeginDate==(new DateTime(1900,1,1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 1).Text =""; //"计划开始时间";//20
                    }else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 1).Text = detail.PlannedBeginDate.ToShortDateString(); //"计划开始时间";//20
                    }
                    //计划结束时间
                    if (detail.PlannedEndDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 2).Text = ""; //"计划结束时间";//21
                    } else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 2).Text = detail.PlannedEndDate.ToShortDateString();
                    }
                    //工期计量单位                    
                    //flexGrid.Cell(rowIndex, endImageCol + 3).Text =detail.ScheduleUnit; //"";//22
                    flexGrid.Cell(rowIndex, endImageCol + 3).Text = "天";


                    //计划工期
                    flexGrid.Cell(rowIndex, endImageCol + 4).Text = detail.PlannedDuration == 0 ? "" : detail.PlannedDuration.ToString(); //"计划工期";//23
                    //实际开始时间
                    if (detail.ActualBeginDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 5).Text = "";
                    } else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 5).Text = detail.ActualBeginDate.ToShortDateString();
                    }
                    //实际结束时间
                    if (detail.ActualEndDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 6).Text = "";
                    } else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 6).Text = detail.ActualEndDate.ToShortDateString();
                    }
                    //实际工期
                    flexGrid.Cell(rowIndex, endImageCol + 7).Text = detail.ActualDuration == 0 ? "" : detail.ActualDuration.ToString();
                    //计划说明
                    flexGrid.Cell(rowIndex, endImageCol + 8).Text = detail.TaskDescript; //"计划说明";//27

                    rowIndex = rowIndex + 1;
                }
            }
            //1-19列的背景色
            FlexCell.Range range = flexGrid.Range(1, startImageCol, flexGrid.Rows - 1, endImageCol);
            if (range != null)
            {
                range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                range.BackColor = SystemColors.Control;
            }
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        private int NodesCount(TreeNode treeNode)
        {
            int result = 0;
            if (treeNode != null)
            {
                result=treeNode.Nodes.Count;
                foreach (TreeNode tn in treeNode.Nodes)
                {
                    result += NodesCount(tn);
                }
            }
            return result;
        }

        private void InitFlexGrid(int rows)
        {
            flexGrid.AutoRedraw = false;
            flexGrid.Column(0).Visible = false;

            flexGrid.Rows=rows;
            flexGrid.Cols = 28;//其中0列隐藏 1-19 为放置图片列 20-27为数据列

            flexGrid.SelectionMode = FlexCell.SelectionModeEnum.ByCell;
            flexGrid.ExtendLastCol = true;
            flexGrid.DisplayFocusRect = false;
            flexGrid.LockButton = true;
            flexGrid.ReadonlyFocusRect = FlexCell.FocusRectEnum.Solid;
            flexGrid.BorderStyle = FlexCell.BorderStyleEnum.FixedSingle;
            flexGrid.ScrollBars = FlexCell.ScrollBarsEnum.Vertical;
            flexGrid.BackColorBkg = SystemColors.Control;
            flexGrid.DefaultFont = new Font("Tahoma", 8);

            FlexCell.Range range;

            for (int i = startImageCol; i <= endImageCol; i++)
            {
                flexGrid.Column(i).TabStop = false;
                flexGrid.Column(i).Width = 20;
            }

            range = flexGrid.Range(0, startImageCol, 0, endImageCol);
            range.Merge();
            flexGrid.Cell(0, 1).Text = "任务名称";
            flexGrid.Cell(0, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;

            // 加载图片
            flexGrid.Images.Add(Resources.ImageExpend, imageExpand);
            flexGrid.Images.Add(Resources.ImageFold, imageCollapse);
            
            flexGrid.Cell(0, endImageCol+1).Text = "计划开始时间";//20
            flexGrid.Cell(0, endImageCol+2).Text = "计划结束时间";//21
            flexGrid.Cell(0, endImageCol+3).Text = "工期计量单位";//22
            flexGrid.Cell(0, endImageCol+4).Text = "计划工期";//23
            flexGrid.Cell(0, endImageCol+5).Text = "实际开始时间";//24
            flexGrid.Cell(0, endImageCol+6).Text = "实际结束时间";//25
            flexGrid.Cell(0, endImageCol+7).Text = "实际工期";//26
            flexGrid.Cell(0, endImageCol+8).Text = "计划说明";//27

            flexGrid.Column(endImageCol + 1).CellType = FlexCell.CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 2).CellType = FlexCell.CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 3).CellType = FlexCell.CellTypeEnum.ComboBox;
            flexGrid.Column(endImageCol + 5).CellType = FlexCell.CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 6).CellType = FlexCell.CellTypeEnum.Calendar;

            flexGrid.Column(endImageCol + 4).Mask = FlexCell.MaskEnum.Digital;
            flexGrid.Column(endImageCol + 7).Mask = FlexCell.MaskEnum.Digital;

            FlexCell.ComboBox cb = flexGrid.ComboBox(endImageCol + 3);
            flexGrid.ComboBox(endImageCol + 3).Locked = true;
            try
            {
                IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.PLAN_ScheduleUnit);
                if (list != null && list.Count > 0)
                {
                    foreach (BasicDataOptr bd in list)
                    {
                        cb.Items.Add(bd.BasicName);
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show("获取工期计量单位出错。");
            }

            // Refresh
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }
    }
}
