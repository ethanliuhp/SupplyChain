using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Properties;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI
{
    public partial class VSelectSchdulePlan : Form
    {

        public bool isOK = false;

        /// <summary>
        /// 选择的总滚动计划
        /// </summary>
        public ProductionScheduleMaster SelectPlanMaster;

        private MProductionMng model = new MProductionMng();

        private string imageExpand = "imageExpand";
        private string imageCollapse = "imageCollapse";

        private EnumScheduleType enumScheduleType = EnumScheduleType.总进度计划;

        private CurrentProjectInfo projectInfo = null;

        private int startImageCol = 1, endImageCol = 19;


        public VSelectSchdulePlan()
        {
            InitializeComponent();

            InitData();
        }

        private void InitData()
        {
            
            btnEnter.Click += new EventHandler(btnEnter_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            gridPlanMaster.CellClick += new DataGridViewCellEventHandler(gridPlanMaster_CellClick);

            flexGrid.Click += new FlexCell.Grid.ClickEventHandler(flexGrid_Click);

            InitFlexGrid(5);

            //归属项目
            projectInfo = StaticMethod.GetProjectInfo();

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("ScheduleType", this.enumScheduleType));
            //oq.AddCriterion(Expression.Eq("DocState", VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute));

            oq.AddOrder(NHibernate.Criterion.Order.Asc("ScheduleTypeDetail"));
            oq.AddOrder(NHibernate.Criterion.Order.Asc("CreateDate"));

            IList listMaster = model.ProductionManagementSrv.ObjectQuery(typeof(ProductionScheduleMaster), oq);

            if (listMaster.Count > 0)
            {
                foreach (ProductionScheduleMaster master in listMaster)
                {
                    AddResourceRequirePlanInGrid(master);
                }
                gridPlanMaster_CellClick(gridPlanMaster, new DataGridViewCellEventArgs(0, 0));
            }
        }

        void gridPlanMaster_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
                return;


            try
            {
                ProductionScheduleMaster plan = gridPlanMaster.Rows[e.RowIndex].Tag as ProductionScheduleMaster;

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", plan.Id));

                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                IList listMaster = model.ProductionManagementSrv.ObjectQuery(typeof(ProductionScheduleMaster), oq);

                if (listMaster.Count > 0)
                    SelectPlanMaster = listMaster[0] as ProductionScheduleMaster;
                else
                {
                    SelectPlanMaster = null;
                    ClearData();
                    return;
                }

                FillFlex();
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询进度计划出错。\n" + ex.Message);
                return;
            }
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

            }
            else if (flexGrid.ActiveCell.ImageKey == imageCollapse)
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
        private void FillFlex()
        {
            flexGrid.AutoRedraw = false;
            flexGrid.Rows = 1;
            flexGrid.Column(endImageCol + 3).Locked = true;


            IList list = null;
            if (!string.IsNullOrEmpty(SelectPlanMaster.Id))
                list = model.ProductionManagementSrv.GetChilds(SelectPlanMaster);

            if (list != null && list.Count > 0)
            {
                flexGrid.Rows = list.Count + 1;
                int rowIndex = 1;
                foreach (ProductionScheduleDetail detail in list)
                {
                    //if (detail.State == EnumScheduleDetailState.失效)
                    //{
                    //    flexGrid.Rows = flexGrid.Rows - 1;
                    //    continue;
                    //}
                    
                    flexGrid.Cell(rowIndex, 0).Tag = detail.Id;

                    flexGrid.Cell(rowIndex, detail.Level).SetImage(imageCollapse);
                    FlexCell.Range rangeTemp = flexGrid.Range(rowIndex, detail.Level + 1, rowIndex, endImageCol);
                    rangeTemp.Merge();

                    //rangeTemp.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                    flexGrid.Cell(rowIndex, detail.Level + 1).Text = (detail.GWBSTreeName == null) ? "进度计划" : detail.GWBSTreeName;
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
                    //工期计量单位                    
                    //flexGrid.Cell(rowIndex, endImageCol + 3).Text =detail.ScheduleUnit; //"";//22
                    flexGrid.Cell(rowIndex, endImageCol + 3).Text = "天";


                    //计划工期
                    flexGrid.Cell(rowIndex, endImageCol + 4).Text = detail.PlannedDuration == 0 ? "" : detail.PlannedDuration.ToString(); //"计划工期";//23

                    //计划说明
                    flexGrid.Cell(rowIndex, endImageCol + 5).Text = detail.TaskDescript; //"计划说明";//27

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
        private void ClearData()
        {
            for (int i = flexGrid.Rows - 1; i > -1; i--)
            {
                flexGrid.RemoveItem(i);
            }
        }

        private void AddResourceRequirePlanInGrid(ProductionScheduleMaster plan)
        {
            int index = gridPlanMaster.Rows.Add();
            DataGridViewRow row = gridPlanMaster.Rows[index];

            row.Cells[colMasterPlanName.Name].Value = plan.ScheduleTypeDetail;
            row.Cells[colMasterVersion.Name].Value = plan.ScheduleName;
            row.Cells[colMasterCaliber.Name].Value = plan.ScheduleCaliber;
            row.Cells[colMasterState.Name].Value = ClientUtil.GetDocStateName(plan.DocState);
            row.Cells[colMasterResponsiblePerson.Name].Value = plan.HandlePersonName;
            row.Cells[colMasterDesc.Name].Value = plan.Descript;

            row.Tag = plan;
        }

        private void InitFlexGrid(int rows)
        {
            flexGrid.AutoRedraw = false;
            flexGrid.Column(0).Visible = false;

            flexGrid.Rows = rows;
            flexGrid.Cols = 25;//其中0列隐藏 1-19 为放置图片列 20-24为数据列

            flexGrid.SelectionMode = FlexCell.SelectionModeEnum.ByCell;
            flexGrid.ExtendLastCol = true;
            flexGrid.DisplayFocusRect = false;
            flexGrid.LockButton = true;
            flexGrid.ReadonlyFocusRect = FlexCell.FocusRectEnum.Solid;
            flexGrid.BorderStyle = FlexCell.BorderStyleEnum.FixedSingle;
            flexGrid.ScrollBars = FlexCell.ScrollBarsEnum.Both;
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

            flexGrid.Cell(0, endImageCol + 1).Text = "计划开始时间";//20
            flexGrid.Cell(0, endImageCol + 2).Text = "计划结束时间";//21
            flexGrid.Cell(0, endImageCol + 3).Text = "工期计量单位";//22
            flexGrid.Cell(0, endImageCol + 4).Text = "计划工期";//23

            flexGrid.Cell(0, endImageCol + 5).Text = "计划说明";//27

            flexGrid.Column(endImageCol + 1).CellType = FlexCell.CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 2).CellType = FlexCell.CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 3).CellType = FlexCell.CellTypeEnum.ComboBox;

            flexGrid.Column(endImageCol + 4).Mask = FlexCell.MaskEnum.Digital;


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
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取工期计量单位出错。");
            }

            // Refresh
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        void btnEnter_Click(object sender, EventArgs e)
        {
            if (SelectPlanMaster == null)
            {
                MessageBox.Show("请选择一个计划！");
                gridPlanMaster.Focus();
                return;
            }

            isOK = true;
            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            isOK = false;
            this.Close();
        }

    }
}
