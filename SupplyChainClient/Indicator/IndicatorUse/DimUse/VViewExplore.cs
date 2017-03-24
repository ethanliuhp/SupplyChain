using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NHibernate.Criterion;
using NHibernate;
using System.Collections;

using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;

using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Util;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    public partial class VViewExplore : Form
    {
        private MCubeManager mCube = new MCubeManager();
        private Hashtable ht_dim = new Hashtable();
        private bool ifScore = false;

        public VViewExplore(bool ifSaveScore)
        {
            ifScore = ifSaveScore;
            InitializeComponent();
            LoadUserViewList();
            InitDimension();

            btnExportExcel.Enabled = false;
            btnPrint.Enabled = false;
            btnPreview.Enabled = false;
        }

        //视图中数据的清除
        private void ClearView()
        {
            ClearFlexCell();
        }

        /// <summary>
        /// 激活控件的一些属性
        /// </summary>
        private void ActiveControls()
        {
            btnExportExcel.Enabled = true;
            btnPrint.Enabled = true;
            btnPreview.Enabled = true;
        }

        private void InitDimension()
        {
            IList list = mCube.DimManagerSrv.GetDimensionCategorys();
            foreach (DimensionCategory dc in list)
            {
                ht_dim.Add(dc.Id + "", dc);
            }
        }

        //FlexCell控件清除
        private void ClearFlexCell()
        {
            grid1.Rows = 1;
            grid1.Cols = 6;
            for (int i = 1; i < 6; i++)
            {
                grid1.Cell(0, i).Text = "";
                grid1.Column(i).Locked = false;
            }
        }

        // <summary>
        // 通过登录人的岗位查询视图列表
        // </summary>
        private void LoadUserViewList()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheJob.Id", ConstObject.TheSysRole.Id));
            ICriterion exp1 = Expression.Eq("StateCode", KnowledgeUtil.VIEWDIS_CREATE_CODE);
            ICriterion exp2 = Expression.Eq("StateCode", KnowledgeUtil.VIEWDIS_EDIT_CODE);
            oq.AddCriterion(Expression.Or(exp1, exp2));
            oq.AddCriterion(Expression.Eq("Main.ViewTypeCode", "2"));
            IList list = mCube.ViewService.GetViewDistributeByQuery(oq);
            lstView.DisplayMember = "ViewName";
            foreach (ViewDistribute vd in list)
            {
                lstView.Items.Add(vd);
            }
        }

        //缩进：根据维度树中维度所处的级别进行缩进
        private void lstView_MouseClick(object sender, MouseEventArgs e)
        {
            mCube.Clear();
            ViewDistribute vd = lstView.SelectedItem as ViewDistribute;
            if (vd == null)
            {
                KnowledgeMessageBox.InforMessage("请选择一个浏览的模板！");
                return;
            }
            //初始化
            ActiveControls();
            ClearView();

            edtDistriSerial.Text = vd.DistributeDate.ToShortDateString();
            edtDistriData.Text = vd.DistributeSerial + "";
            mCube.HtDimension = ht_dim;
            mCube.DisplayCustomFlexCell(grid1, vd.Main, ifScore, false, "");
        }

        private Hashtable transResult(IList result)
        {
            Hashtable ht_result = new Hashtable();
            foreach (CubeData cd in result)
            {
                IList list = cd.DimDataList;
                double value = cd.Result;
                string link = "_";
                foreach (string dimId in list)
                {
                    link += dimId + "_";
                }
                ht_result.Add(link, value);
            }
            return ht_result;
        }

        private void frmDemo2_Load(object sender, EventArgs e)
        {
            grid1.DisplayRowNumber = true;
            mnuFLXSample.Text = System.Windows.Forms.Application.StartupPath + "\\Sample.flx";
            mnuXMLSample.Text = System.Windows.Forms.Application.StartupPath + "\\Sample.xml";
        }

        private void frmDemo2_Resize(object sender, EventArgs e)
        {
            grid1.Size = this.ClientSize; 
        }

        private void mnuNewFile_Click(object sender, EventArgs e)
        {
            grid1.NewFile();
        }

        private void mnuOpenFile_Click(object sender, EventArgs e)
        {
            grid1.OpenFile("");
        }

        private void mnuSaveFile_Click(object sender, EventArgs e)
        {
            if (grid1.SaveFile(""))
            {
                MessageBox.Show("保存成功。", "演示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void mnuExportToExcel_Click(object sender, EventArgs e)
        {
            grid1.ExportToExcel("", true, true);
            MessageBox.Show("输出成功。", "演示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
        }

        private void mnuExportToCSV_Click(object sender, EventArgs e)
        {
            if (grid1.ExportToCSV("", false, false))
            {
                MessageBox.Show("输出成功。", "演示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void mnuExportToHTML_Click(object sender, EventArgs e)
        {
            if (grid1.ExportToHTML(""))
            {
                MessageBox.Show("输出成功。", "演示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void mnuExportToXML_Click(object sender, EventArgs e)
        {
            if (grid1.ExportToXML(""))
            {
                MessageBox.Show("输出成功。", "演示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void mnuLoadFromXML_Click(object sender, EventArgs e)
        {
            grid1.LoadFromXML("");
        }

        private void mnuPageSetup_Click(object sender, EventArgs e)
        {
            try
            {
                grid1.ShowPageSetupDialog();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "演示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void mnuPrintPreview_Click(object sender, EventArgs e)
        {
            try
            {
                mCube.GeneralPreview(grid1);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "演示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void mnuPrint_Click(object sender, EventArgs e)
        {
            try
            {
                grid1.Print();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "演示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void mnuFLXSample_Click(object sender, EventArgs e)
        {
            grid1.OpenFile(System.Windows.Forms.Application.StartupPath + "\\Sample.flx");
        }

        private void mnuXMLSample_Click(object sender, EventArgs e)
        {
            grid1.LoadFromXML(System.Windows.Forms.Application.StartupPath + "\\Sample.xml");
        }

        private void mnuQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnuCutData_Click(object sender, EventArgs e)
        {
            grid1.Selection.CutData();
        }

        private void mnuCopyData_Click(object sender, EventArgs e)
        {
            grid1.Selection.CopyData();
        }

        private void mnuPasteData_Click(object sender, EventArgs e)
        {
            grid1.Selection.PasteData();
        }

        private void mnuMerge_Click(object sender, EventArgs e)
        {
            try
            {
                grid1.Selection.Merge();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "操作", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void mnuUnmerge_Click(object sender, EventArgs e)
        {
            try
            {
                grid1.Selection.MergeCells = false;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "操作", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void mnuClearText_Click(object sender, EventArgs e)
        {
            grid1.Selection.ClearText();
        }

        private void mnuClearFormat_Click(object sender, EventArgs e)
        {
            grid1.Selection.ClearFormat();
        }

        private void mnuClearAll_Click(object sender, EventArgs e)
        {
            grid1.Selection.ClearAll();
        }

        private void mnuInsertRow_Click(object sender, EventArgs e)
        {
            grid1.Selection.InsertRows();
        }

        private void mnuInsertCol_Click(object sender, EventArgs e)
        {
            grid1.Selection.InsertCols();
        }

        private void mnuDeleteRow_Click(object sender, EventArgs e)
        {
            grid1.Selection.DeleteByRow();
        }

        private void mnuDeleteCol_Click(object sender, EventArgs e)
        {
            grid1.Selection.DeleteByCol();
        }

        private void mnuHideRow_Click(object sender, EventArgs e)
        {
            grid1.AutoRedraw = false;

            for (int i = grid1.Selection.FirstRow; i <= grid1.Selection.LastRow; i++)
            {
                grid1.Row(i).Visible = false;
            }

            grid1.AutoRedraw = true;
            grid1.Refresh();
        }

        private void mnuHideCol_Click(object sender, EventArgs e)
        {
            grid1.AutoRedraw = false;

            for (int i = grid1.Selection.FirstCol; i <= grid1.Selection.LastCol; i++)
            {
                grid1.Column(i).Visible = false;
            }

            grid1.AutoRedraw = true;
            grid1.Refresh();
        }
               
        private void mnuFont_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = grid1.ActiveCell.Font;

            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                grid1.Selection.Font = fontDialog.Font;
            }

            fontDialog.Dispose();
        }

        private void mnuBorder_None_Click(object sender, EventArgs e)
        {
            grid1.Selection.set_Borders(FlexCell.EdgeEnum.Inside | FlexCell.EdgeEnum.Outside | FlexCell.EdgeEnum.DiagonalUp | FlexCell.EdgeEnum.DiagonalDown, FlexCell.LineStyleEnum.None);
        }

        private void mnuBorder_Left_Click(object sender, EventArgs e)
        {
            grid1.Selection.set_Borders(FlexCell.EdgeEnum.Left, FlexCell.LineStyleEnum.Thin);
        }

        private void mnuBorder_Right_Click(object sender, EventArgs e)
        {
            grid1.Selection.set_Borders(FlexCell.EdgeEnum.Right, FlexCell.LineStyleEnum.Thin);
        }

        private void mnuBorder_Top_Click(object sender, EventArgs e)
        {
            grid1.Selection.set_Borders(FlexCell.EdgeEnum.Top, FlexCell.LineStyleEnum.Thin);
        }

        private void mnuBorder_Bottom_Click(object sender, EventArgs e)
        {
            grid1.Selection.set_Borders(FlexCell.EdgeEnum.Bottom, FlexCell.LineStyleEnum.Thin);
        }

        private void mnuBorder_DiagonalUp_Click(object sender, EventArgs e)
        {
            grid1.Selection.set_Borders(FlexCell.EdgeEnum.DiagonalUp, FlexCell.LineStyleEnum.Thin);
        }

        private void mnuBorder_DiagonalDown_Click(object sender, EventArgs e)
        {
            grid1.Selection.set_Borders(FlexCell.EdgeEnum.DiagonalDown, FlexCell.LineStyleEnum.Thin);
        }

        private void mnuBorder_Inside_Click(object sender, EventArgs e)
        {
            grid1.Selection.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
        }

        private void mnuBorder_OutsideThin_Click(object sender, EventArgs e)
        {
            grid1.Selection.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
        }

        private void mnuBorder_OutsideThick_Click(object sender, EventArgs e)
        {
            grid1.Selection.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thick);
        }

        private void mnuBorder_All_Click(object sender, EventArgs e)
        {
            grid1.Selection.set_Borders(FlexCell.EdgeEnum.Outside | FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
        }

        private void mnuType_TextBox_Click(object sender, EventArgs e)
        {
            grid1.Selection.CellType = FlexCell.CellTypeEnum.TextBox; 
        }

        private void mnuType_ComboBox_Click(object sender, EventArgs e)
        {
            grid1.Selection.CellType = FlexCell.CellTypeEnum.ComboBox;
        }

        private void mnuType_CheckBox_Click(object sender, EventArgs e)
        {
            grid1.Selection.CellType = FlexCell.CellTypeEnum.CheckBox;
        }

        private void mnuType_Calendar_Click(object sender, EventArgs e)
        {
            grid1.Selection.CellType = FlexCell.CellTypeEnum.Calendar;
        }

        private void mnuType_Button_Click(object sender, EventArgs e)
        {
            grid1.Selection.CellType = FlexCell.CellTypeEnum.Button;
        }

        private void mnuType_HyperLink_Click(object sender, EventArgs e)
        {
            grid1.Selection.CellType = FlexCell.CellTypeEnum.HyperLink;
        }

        private void mnuType_Default_Click(object sender, EventArgs e)
        {
            grid1.Selection.CellType = FlexCell.CellTypeEnum.DefaultType;
        }

        private void mnuMask_None_Click(object sender, EventArgs e)
        {
            grid1.Selection.Mask = FlexCell.MaskEnum.None;
        }

        private void mnuMask_Letter_Click(object sender, EventArgs e)
        {
            grid1.Selection.Mask = FlexCell.MaskEnum.Letter;
        }

        private void mnuMask_Upper_Click(object sender, EventArgs e)
        {
            grid1.Selection.Mask = FlexCell.MaskEnum.Upper;
        }

        private void mnuMask_Lower_Click(object sender, EventArgs e)
        {
            grid1.Selection.Mask = FlexCell.MaskEnum.Lower;
        }

        private void mnuMask_Digital_Click(object sender, EventArgs e)
        {
            grid1.Selection.Mask = FlexCell.MaskEnum.Digital;
        }

        private void mnuMask_Numeric_Click(object sender, EventArgs e)
        {
            grid1.Selection.Mask = FlexCell.MaskEnum.Numeric;
        }

        private void mnuMask_Default_Click(object sender, EventArgs e)
        {
            grid1.Selection.Mask = FlexCell.MaskEnum.DefaultMask;
        }

        private void mnuAlign_GeneralGeneral_Click(object sender, EventArgs e)
        {
            grid1.Selection.Alignment = FlexCell.AlignmentEnum.GeneralGeneral;
        }

        private void mnuAlign_GeneralTop_Click(object sender, EventArgs e)
        {
            grid1.Selection.Alignment = FlexCell.AlignmentEnum.GeneralTop;
        }

        private void mnuAlign_GeneralCenter_Click(object sender, EventArgs e)
        {
            grid1.Selection.Alignment = FlexCell.AlignmentEnum.GeneralCenter;
        }

        private void mnuAlign_GeneralBottom_Click(object sender, EventArgs e)
        {
            grid1.Selection.Alignment = FlexCell.AlignmentEnum.GeneralBottom;
        }

        private void mnuAlign_LeftGeneral_Click(object sender, EventArgs e)
        {
            grid1.Selection.Alignment = FlexCell.AlignmentEnum.LeftGeneral;
        }

        private void mnuAlign_LeftTop_Click(object sender, EventArgs e)
        {
            grid1.Selection.Alignment = FlexCell.AlignmentEnum.LeftTop;
        }

        private void mnuAlign_LeftCenter_Click(object sender, EventArgs e)
        {
            grid1.Selection.Alignment = FlexCell.AlignmentEnum.LeftCenter;
        }

        private void mnuAlign_LeftBottom_Click(object sender, EventArgs e)
        {
            grid1.Selection.Alignment = FlexCell.AlignmentEnum.LeftBottom;
        }

        private void mnuAlign_CenterGeneral_Click(object sender, EventArgs e)
        {
            grid1.Selection.Alignment = FlexCell.AlignmentEnum.CenterGeneral;
        }

        private void mnuAlign_CenterTop_Click(object sender, EventArgs e)
        {
            grid1.Selection.Alignment = FlexCell.AlignmentEnum.CenterTop;
        }

        private void mnuAlign_CenterCenter_Click(object sender, EventArgs e)
        {
            grid1.Selection.Alignment = FlexCell.AlignmentEnum.CenterCenter;
        }

        private void mnuAlign_CenterBottom_Click(object sender, EventArgs e)
        {
            grid1.Selection.Alignment = FlexCell.AlignmentEnum.CenterBottom;
        }

        private void mnuAlign_RightGeneral_Click(object sender, EventArgs e)
        {
            grid1.Selection.Alignment = FlexCell.AlignmentEnum.RightGeneral;
        }

        private void mnuAlign_RightTop_Click(object sender, EventArgs e)
        {
            grid1.Selection.Alignment = FlexCell.AlignmentEnum.RightTop;
        }

        private void mnuAlign_RightCenter_Click(object sender, EventArgs e)
        {
            grid1.Selection.Alignment = FlexCell.AlignmentEnum.RightCenter;
        }

        private void mnuAlign_RightBottom_Click(object sender, EventArgs e)
        {
            grid1.Selection.Alignment = FlexCell.AlignmentEnum.RightBottom;
        }

        private void mnuTextColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = grid1.ActiveCell.ForeColor;

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                grid1.Selection.ForeColor = colorDialog.Color;
            }

            colorDialog.Dispose();
        }

        private void mnuBackColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = grid1.ActiveCell.BackColor;

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                grid1.Selection.BackColor = colorDialog.Color;
            }

            colorDialog.Dispose();
        }

        private void mnuSetWrapText_Click(object sender, EventArgs e)
        {
            grid1.Selection.WrapText = true;
        }

        private void mnuCancelWrapText_Click(object sender, EventArgs e)
        {
            grid1.Selection.WrapText = false;
        }

        private void mnuLock_Click(object sender, EventArgs e)
        {
            grid1.Selection.Locked = true;
        }

        private void mnuUnlock_Click(object sender, EventArgs e)
        {
            grid1.Selection.Locked = false;
        }

        private void mnuPrintable_Click(object sender, EventArgs e)
        {
            grid1.Selection.Printable = true;
        }

        private void mnuUnprintable_Click(object sender, EventArgs e)
        {
            grid1.Selection.Printable = false;
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            /*frmProperties dlg = new frmProperties();
            dlg.SetGrid(grid1);
            dlg.ShowDialog();
            dlg.Dispose();*/
        }

        private void menuItem1_Click_1(object sender, EventArgs e)
        {
            grid1.PageSetup.DocumentName = "Sample Document";
            if (grid1.ExportToPDF(""))
            {
                MessageBox.Show("输出成功。", "演示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            mCube.GeneralPreview(grid1);
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            grid1.ExportToExcel("", true, true);
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}