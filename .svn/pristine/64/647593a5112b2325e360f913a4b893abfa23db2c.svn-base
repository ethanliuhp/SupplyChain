using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using FlexCell;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI
{
    public partial class VSchedulePreNodeSet : TBasicDataView
    {
        private FlexCell.Grid ParentFlexgrid = null;
        private string DetailId = string.Empty;
        private List<WeekScheduleDetail> LstDetail = null;
        private int endImageCol = 19;
        private List<WeekScheduleRalation> LstRelated = null;
        List<WeekScheduleDetail> _currMstDtl = null;

        /// <summary>
        /// 是否点击的保存按钮
        /// </summary>
        public bool IsSave { private set; get; }

        public decimal RtnPlannedDuration { set; get; }
        public DateTime RtnBeginDate { get; set; }

        private List<WeekScheduleRalation> _rtnListRalated;

        ///<summary>
        ///返回的关系结果集
        ///</summary>
        public List<WeekScheduleRalation> RtnListRalated
        {
            get { return this._rtnListRalated; }
        }
        private string _strRalation;


        ///<summary>
        /// 返回的前置任务文本
        ///</summary>
        public string StrRalation
        {
            get { return this._strRalation; }
        }

        public VSchedulePreNodeSet(Grid parentGrid, string detailID, List<WeekScheduleDetail> lstDetail, List<WeekScheduleRalation> curNodeLstRelated, List<WeekScheduleDetail> CurrMstDtl)
        {
            DetailId = detailID;
            ParentFlexgrid = parentGrid;
            LstDetail = lstDetail;
            LstRelated = curNodeLstRelated;
            _currMstDtl = CurrMstDtl;
            InitializeComponent();
            InitData(curNodeLstRelated);
            InitEvents();
        }

        private void InitData(List<WeekScheduleRalation> curNodeLstRelated)
        {
            InitFlexGrid();
            grid.AutoRedraw = false;
            int rowIndex = 1;
            var detail = LstDetail.FirstOrDefault(p => p.Id == DetailId);
            lblCurNode.Text = detail.GWBSTreeName;
            dtpBeginDate.Text = detail.PlannedBeginDate == new DateTime(1900, 1, 1) ? "" : detail.PlannedBeginDate.ToShortDateString();
            txtPlannedDuration.Text = ClientUtil.ToString(detail.PlannedDuration == 0 ? 1 : detail.PlannedDuration);
            foreach (var item in curNodeLstRelated)
            {
                var preDetail = LstDetail.FirstOrDefault(p => p.Id == item.FrontWeekScheduleDetail.Id);
                if (preDetail == null)
                {
                    WeekScheduleDetail wsd_ff = _currMstDtl.Find(a => a.Id == item.FrontWeekScheduleDetail.Id);
                    if (wsd_ff != null)
                        grid.Cell(rowIndex, 1).Text = wsd_ff.GWBSTreeName;
                }
                else
                {
                    grid.Cell(rowIndex, 1).Text = ClientUtil.ToString(preDetail.RowIndex);//行号
                }
                grid.Cell(rowIndex, 0).Tag = item.Id;

                //grid.Cell(rowIndex, 2).Text = detail.GWBSTreeName;//节点名称
                WeekScheduleDetail wsd_ff1 = _currMstDtl.Find(a => a.Id == item.FrontWeekScheduleDetail.Id);
                if (wsd_ff1 != null)
                    grid.Cell(rowIndex, 2).Text = wsd_ff1.GWBSTreeName;//节点名称

                //grid.Cell(rowIndex, 3).Text = GetEnumDelayRuleDesc(item.DelayRule);
                grid.Cell(rowIndex, 3).Text = EnumUtil<EnumDelayRule>.GetDescription(item.DelayRule);
                grid.Cell(rowIndex++, 4).Text = ClientUtil.ToString(item.DelayDays);
            }
            grid.AutoRedraw = true;
            grid.Refresh();
            grid.Cell(1, 1).SetFocus();
        }

        private void InitEvents()
        {
            grid.CellChange += new FlexCell.Grid.CellChangeEventHandler(grid_CellChange);

            btnSave.Click += new EventHandler(btnSave_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        #region 事件
        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            IsSave = true;
            this._strRalation = "";
            List<WeekScheduleRalation> lst = new List<WeekScheduleRalation>();
            for (int i = 1; i < grid.Rows; i++)
            {
                int preNodeRowIndex = ClientUtil.ToInt(grid.Cell(i, 1).Text.Trim());
                //去根节点
                if (preNodeRowIndex < 2)
                {
                    break;
                }
                if (this._strRalation != "")
                    this._strRalation += " ,";

                string preNodeID = ParentFlexgrid.Cell(preNodeRowIndex, 0).Tag;

                this._strRalation += ClientUtil.ToString(preNodeRowIndex);
                this._strRalation += Enum.GetName(typeof(EnumDelayRule), EnumUtil<EnumDelayRule>.FromDescription(grid.Cell(i, 3).Text.Trim())) + "+";
                this._strRalation += ClientUtil.ToInt(grid.Cell(i, 4).Text.Trim()) + "天";

                string id = grid.Cell(i, 0).Tag;
                WeekScheduleRalation model = LstRelated.FirstOrDefault(p => p.Id == id) ?? new WeekScheduleRalation();
                model.PreNodeRowIndex = preNodeRowIndex;
                model.Master = LstDetail.FirstOrDefault(p => p.Id == DetailId);
                model.FrontWeekScheduleDetail = LstDetail.FirstOrDefault(p => p.Id == preNodeID);
                model.DelayRule = EnumUtil<EnumDelayRule>.FromDescription(grid.Cell(i, 3).Text.Trim());
                model.DelayDays = ClientUtil.ToInt(grid.Cell(i, 4).Text.Trim());
                lst.Add(model);
            }
            this._rtnListRalated = lst;
            this.RtnPlannedDuration = ClientUtil.ToDecimal(txtPlannedDuration.Text.Trim());
            this.RtnBeginDate = dtpBeginDate.Value;
            this.Close();
        }

        void grid_CellChange(object Sender, FlexCell.Grid.CellChangeEventArgs e)
        {
            /**待添加验证
             * 改变，加进来，然后便于做重复数据验证，
             * 当前节点 否掉
             * 父节点， 否掉  
             */
            //如果是非表头，第一列（前置任务行号），
            if (e.Row > 0 && e.Col == 1)
            {
                var strFirstCol = grid.Cell(e.Row, e.Col).Text.Trim();
                if (string.IsNullOrEmpty(strFirstCol))
                {
                    grid.Cell(e.Row, 0).Tag = null;
                    grid.Cell(e.Row, 1).Text = "";
                    grid.Cell(e.Row, 2).Text = "";
                    grid.Cell(e.Row, 3).Text = "";
                    grid.Cell(e.Row, 4).Text = "";
                    return;
                }
                int preNodeRowIndex = ClientUtil.ToInt(strFirstCol);
                if (preNodeRowIndex > 1 && preNodeRowIndex <= LstDetail.Count)
                {
                    int parentRowNameColIndex = ClientUtil.ToInt(ParentFlexgrid.Cell(preNodeRowIndex, endImageCol + 4).Tag);
                    var preNodeId = ParentFlexgrid.Cell(preNodeRowIndex, 0).Tag;
                    var preNode = LstDetail.FirstOrDefault(p => p.Id == preNodeId);
                    if (IsEndlessLoop(preNode))//判断是否会形成死循环
                    {
                        grid.Cell(e.Row, 0).Tag = null;
                        grid.Cell(e.Row, 1).Text = "";
                        grid.Cell(e.Row, 2).Text = "";
                        grid.Cell(e.Row, 3).Text = "";
                        grid.Cell(e.Row, 4).Text = "";
                        MessageBox.Show("你正试图将一个任务连接到另一个任务，而后者包含一系列链接的任务，最终又链接回第一个任务。无法再这些任务之间创建链接，这样将产生循环任务关联。请重新填写");
                        return;
                    }
                    if (IsAncestorNode(preNodeId, LstDetail.FirstOrDefault(p => p.Id == DetailId)))//判断是否是父级|祖先级任务节点
                    {
                        grid.Cell(e.Row, 0).Tag = null;
                        grid.Cell(e.Row, 1).Text = "";
                        grid.Cell(e.Row, 2).Text = "";
                        grid.Cell(e.Row, 3).Text = "";
                        grid.Cell(e.Row, 4).Text = "";
                        MessageBox.Show("无法将摘要任务连接到它本身的子任务之一");
                        return;
                    }

                    grid.Cell(e.Row, 2).Text = ParentFlexgrid.Cell(preNodeRowIndex, parentRowNameColIndex).Text.Trim();
                    grid.Cell(e.Row, 3).Text = EnumUtil<EnumDelayRule>.GetDescription(EnumDelayRule.FS);
                    grid.Cell(e.Row, 4).Text = "0";
                }
                else if (preNodeRowIndex < 2)//如果填写的为空或是1时【第一行为项目名称不能作为前置节点】
                {
                    grid.Cell(e.Row, 1).Text = "";
                    grid.Cell(e.Row, 2).Text = "";
                    grid.Cell(e.Row, 3).Text = "";
                    grid.Cell(e.Row, 4).Text = "";
                    MessageBox.Show("第一行为项目名称不能作为前置节点,请重新填写");
                }
                else if (preNodeRowIndex > LstDetail.Count)//如果填写的为空或是大于最大行数时，
                {
                    grid.Cell(e.Row, 1).Text = "";
                    grid.Cell(e.Row, 2).Text = "";
                    grid.Cell(e.Row, 3).Text = "";
                    grid.Cell(e.Row, 4).Text = "";
                    MessageBox.Show("在第" + preNodeRowIndex + "行没有任务节点，请重新填写");
                }
            }
        }
        #endregion


        private void InitFlexGrid()
        {
            int iCol, iRow;
            FlexCell.Cell oCell = null;
            FlexCell.Column oColumn = null;
            grid.AutoRedraw = false;
            grid.Rows = 30;
            grid.Cols = 5;
            grid.Locked = false;
            grid.Cell(0, 0).Locked = true;
            grid.Row(0).Locked = false;
            grid.DisplayRowNumber = true;
            grid.StartRowNumber = 1;
            grid.Column(0).Visible = true;
            grid.Column(0).AutoFit();
            grid.SelectionMode = FlexCell.SelectionModeEnum.Free;
            grid.DisplayFocusRect = true;
            grid.ReadonlyFocusRect = FlexCell.FocusRectEnum.Solid;
            grid.BorderStyle = FlexCell.BorderStyleEnum.FixedSingle;
            grid.ScrollBars = FlexCell.ScrollBarsEnum.Both;
            grid.DefaultFont = new Font("Tahoma", 8);

            grid.Column(1).Width = 90;
            grid.Column(2).Width = 230;
            grid.Column(3).Width = 130;
            grid.Column(4).Width = 100;

            //前置节点行号
            iRow = 0; iCol = 1;
            oCell = grid.Cell(iRow, iCol);
            oCell.Text = "前置节点行号";

            //前置任务名称
            iCol++;
            oCell = grid.Cell(iRow, iCol);
            oCell.Text = "前置任务名称";
            grid.Column(iCol).Locked = true;

            //规则
            iCol++;
            oCell = grid.Cell(iRow, iCol);
            oCell.Text = "规则";
            grid.Column(iCol).CellType = CellTypeEnum.ComboBox;

            foreach (var item in EnumUtil<EnumDelayRule>.GetDescriptions())
            {
                grid.ComboBox(iCol).Items.Add(item);
            }

            //延时时间
            iCol++;
            oCell = grid.Cell(iRow, iCol);
            oCell.Text = "延时时间";

            grid.AutoRedraw = true;
            grid.Refresh();
        }

        /// <summary>
        /// 判断前置节点是否是死循环【此处找到自己节点即为死循环，如：A-->B-->C-->A】
        /// </summary>
        /// <param name="curNode">当前节点</param>
        /// <param name="curPreNode">前置节点</param>
        /// <returns>true:是死循环；false：不是</returns>
        private bool IsEndlessLoop(WeekScheduleDetail curPreNode)
        {
            //由于此处是环路，故只需要递归判断前置节点即可，无需判断以此节点为前置节点的相关节点
            if (DetailId == curPreNode.Id)
            {
                return true;
            }

            foreach (WeekScheduleRalation itemRelation in curPreNode.RalationDetails.ToList())
            {
                var itemNode = LstDetail.FirstOrDefault(p => p.Id == itemRelation.FrontWeekScheduleDetail.Id);
                if (IsEndlessLoop(itemNode))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断是否是父级|祖先级任务节点
        /// </summary>
        /// <param name="curPreNodeID">当前前置节点ID</param>
        /// <param name="curParentNode">当前节点的父任务节点</param>
        /// <returns>true:是父级|祖先级；false：不是</returns>
        private bool IsAncestorNode(string curPreNodeID, WeekScheduleDetail curParentNode)
        {
            if (curParentNode == null)
            {
                return false;
            }
            if (curPreNodeID == curParentNode.Id)
            {
                return true;
            }

            return IsAncestorNode(curPreNodeID, curParentNode.ParentNode);
        }
    }
}
