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

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI
{
    public partial class VCostBudget : TBasicDataView
    {
        public VCostBudget()
        {
            InitializeComponent();
        }

        public VCostBudget(string projectid, string sysCode)
            : this()
        {
            InitData(projectid, sysCode);
        }

        private void InitData(string projectid, string sysCode)
        {
            InitFlexGrid();
            MProductionMng model = new MProductionMng();
            DataTable dt = model.GetCostBudget(projectid, sysCode);
            FillFlex(dt);
        }


        private void InitFlexGrid()
        {
            flexGrid.AutoRedraw = false;
            flexGrid.Column(0).Visible = true;

            flexGrid.Rows = 1;
            flexGrid.Cols = 5;

            flexGrid.SelectionMode = SelectionModeEnum.Free;
            flexGrid.ExtendLastCol = false;
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

            flexGrid.Cell(0, 1).Alignment = AlignmentEnum.CenterCenter;
            flexGrid.Cell(0, 1).Text = "资源名称";
            flexGrid.Cell(0, 2).Text = "资源规格";
            flexGrid.Cell(0, 3).Text = "数量";
            flexGrid.Cell(0, 4).Text = "工程量单位";
            //Refresh
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        private void FillFlex(DataTable dt)
        {
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
            {
                return;
            }
            flexGrid.AutoRedraw = false;
            flexGrid.Rows = dt.Rows.Count + 1;
            int rowIndex = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                rowIndex = i + 1;
                flexGrid.Cell(rowIndex, 1).Text = ClientUtil.ToString(dt.Rows[i]["RESOURCETYPENAME"]);// "资源名称";
                flexGrid.Cell(rowIndex, 2).Text = ClientUtil.ToString(dt.Rows[i]["ResourceTypeSpec"]);// "资源规格";
                flexGrid.Cell(rowIndex, 3).Text = ClientUtil.ToString(dt.Rows[i]["totalamount"]);//数量
                flexGrid.Cell(rowIndex, 3).Alignment = AlignmentEnum.RightCenter;
                flexGrid.Cell(rowIndex, 4).Text = ClientUtil.ToString(dt.Rows[i]["PROJECTAMOUNTUNITNAME"]);// 工程量单位
                flexGrid.Cell(rowIndex, 4).Alignment = AlignmentEnum.CenterCenter;
            }
            for (int i = 1; i < flexGrid.Cols; i++)
            {
                flexGrid.Column(i).AutoFit();
            }
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }
    }
}
