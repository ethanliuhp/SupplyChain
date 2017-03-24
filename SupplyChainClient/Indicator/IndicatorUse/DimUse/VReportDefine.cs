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

using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;

using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.Client.Util;



namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    public partial class VReportDefine : TBasicDataView
    {
        private MCubeManager mCube = new MCubeManager();
        private MIndicatorUse model = new MIndicatorUse();
        public ViewMain vm = new ViewMain();
        public Hashtable ht_dim = new Hashtable();
        private Hashtable ht_express = new Hashtable();//报表内存在表达式的HT

        public VReportDefine()
        {
            InitializeComponent();
            InitialEvents();
        }

        #region 界面属性
        private void InitialEvents()
        {
            btnInsertRow.Click += new EventHandler(btnInsertRow_Click);
            btnDelRow.Click += new EventHandler(btnDelRow_Click);
            btnInsertCol.Click += new EventHandler(btnInsertCol_Click);
            btnDelCol.Click += new EventHandler(btnDelCol_Click);
        }

        private void frmDemo2_Load(object sender, EventArgs e)
        {
            LoadData();
            ht_dim = mCube.DimManagerSrv.GetDimensionCategorysBySql();
        }
        #endregion

        #region 私有函数

        private void ControlButton(bool ifEnabled)
        {
            mnuInsertRow.Enabled = ifEnabled;
            mnuInsertCol.Enabled = ifEnabled;
            mnuDeleteRow.Enabled = ifEnabled;
            mnuDeleteCol.Enabled = ifEnabled;
        }

        private void LoadData()
        {
            grid1.Rows = 1;
            grid1.DisplayRowNumber = true;
            LoadFlexFile();
            LoadRuleDefData();
            SetColNumber();
        }

        //引导表格设计器的格式文件
        private void LoadFlexFile()
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;

            if (eFile.IfExistFileInServer(vm.ViewName + ".flx"))
            {
                //eFile.CreateEnergyTempletFile(vm.ViewName + ".flx");
                //载入格式和数据
                grid1.OpenFile(path + "\\" + vm.ViewName + ".flx");//载入格式
            }
            grid1.DisplayRowNumber = true;
        }

        private void SetColNumber() {
            int col = grid1.Cols;
            for (int i = 1; i < col; i++)
            {
                grid1.Cell(0, i).Text = KnowledgeUtil.Init_str[i].ToString();
            }
        }

        private void ClearFlexDef() {

            grid1.AutoRedraw = false;
            int rows = grid1.Rows;
            int cols = grid1.Cols;
            //去除单元格属性
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (grid1.Cell(i, j).CellType == FlexCell.CellTypeEnum.Button)
                    {
                        grid1.Cell(i, j).Text = "";
                        grid1.Cell(i, j).ClearBackColor();
                    }
                    grid1.Cell(i, j).CellType = FlexCell.CellTypeEnum.DefaultType;
                    grid1.Cell(i, j).Tag = "";
                }
            }
            grid1.AutoRedraw = true;
            grid1.Refresh();
        }

        /// <summary>
        /// 引导规则数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadRuleDefData()
        {
            ht_express.Clear();
            if (vm == null)
            {
                return;
            }
            IList rule_list = mCube.ViewService.GetViewRuleDefByViewMain(vm);

            //控制菜单的按钮
            if (rule_list.Count > 0)
            {
                ControlButton(false);
            }
            else
            {
                ControlButton(true);
            }

            grid1.AutoRedraw = false;
            foreach (ViewRuleDef vrd in rule_list)
            {
                string sign = vrd.CellSign;
                //取出列字符
                int col = KnowledgeUtil.GetSignIndex(KnowledgeUtil.SplitStr(sign, 2));
                int row = int.Parse(KnowledgeUtil.SplitStr(sign, 1));
                grid1.Cell(row, col).Tag = vrd.Id+"";
                grid1.Cell(row, col).CellType = FlexCell.CellTypeEnum.Button;
                grid1.Cell(row, col).BackColor = System.Drawing.Color.AliceBlue;

                if (vrd.CalExpress != null && !"".Equals(vrd.CalExpress))
                {                   
                    grid1.Cell(row, col).Text = vrd.CalExpress;
                }
                else
                {
                    grid1.Cell(row, col).Text = vrd.DisplayRule;
                }

                bool ifLetter = KnowledgeUtil.IfHaveUpperLetter(vrd.CalExpress);
                if (vrd.CalExpress != null && !"".Equals(vrd.CalExpress) && ifLetter == true)
                {
                    ht_express.Add(vrd.Id + "", vrd.CalExpress);
                }
            }
            grid1.AutoRedraw = true;
            grid1.Refresh();
        }

        //重新显示单元格计算表达式
        private void ReDisplayCellExpressByRow(int curr_row)
        {
            int rows = grid1.Rows;
            int cols = grid1.Cols;

            for (int i = curr_row; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    string id = grid1.Cell(i, j).Tag;
                    if (id != null && !"".Equals(id))
                    {
                        if (ht_express[id] != null)
                        {
                            grid1.Cell(i, j).Text = ht_express[id].ToString();
                        }
                    }
                }
            }
        }

        //重新显示单元格计算表达式
        private void ReDisplayCellExpressByCol(int curr_col)
        {
            int rows = grid1.Rows;
            int cols = grid1.Cols;
            for (int j = curr_col; j <= cols; j++)
            {
                for (int i = 0; i < rows; i++)
                {
                    string id = grid1.Cell(i, j).Tag;
                    if (id != null && !"".Equals(id))
                    {
                        if (ht_express[id] != null)
                        {
                            string express = ht_express[id].ToString();
                            grid1.Cell(i, j).Text = express;
                        }
                    }
                }
            }
        }

        private void SaveData()
        {
            IList save_list = new ArrayList();
            //保存规则定义，保存grid中的单元格属性为“按钮”的规则定义
            int rows = grid1.Rows;
            int cols = grid1.Cols;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    string id = grid1.Cell(i, j).Tag;
                    if (grid1.Cell(i, j).CellType == FlexCell.CellTypeEnum.Button && !"".Equals(id))
                    {

                        string sign = KnowledgeUtil.Init_str[j].ToString() + i;
                        string express = grid1.Cell(i, j).Text;
                        ViewRuleDef vrd = new ViewRuleDef();
                        vrd.Id = id;
                        vrd.CellSign = sign;
                        if (express != null && !"".Equals(express))
                        {
                            if (express.IndexOf("[") != -1)
                            {
                                vrd.CalExpress = express;
                            }
                            else
                            {
                                vrd.DisplayRule = express;
                            }
                        }
                        else
                        {
                            vrd.CalExpress = "";
                            vrd.DisplayRule = "";
                        }
                        save_list.Add(vrd);
                    }
                }
            }
            mCube.ViewService.BatchUpdateViewRuleDefById(save_list);

            ClearFlexDef();

            //保存文件格式
            string fileName = "c:\\windows\\temp\\techtemp\\" + vm.ViewName + ".flx";
            grid1.SaveFile(fileName);
            ExploreFile eFile = new ExploreFile();
            byte[] result = eFile.GetBytes(fileName);
            mCube.CubeManagerSrv.SaveFileToServer(vm.ViewName + ".flx", result);

            LoadData();
        }
        #endregion

        #region 界面事件

        private void btnCancle_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
            KnowledgeMessageBox.InforMessage("保存成功。");
        }

        private void grid1_ButtonClick(object Sender, FlexCell.Grid.ButtonClickEventArgs e)
        {
            VReportExpressionDefine vred = new VReportExpressionDefine();

            FlexCell.Cell curr_cell = grid1.ActiveCell;            
            vred.vMain = vm;
            vred.ht_dims = ht_dim;
            vred.calExp_id = curr_cell.Text;
            vred.ShowDialog();

            if (vred.isOkClicked)
            {
                curr_cell.Text = vred.calExp_id;
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            IList set_list = new ArrayList();

            grid1.Selection.CellType = FlexCell.CellTypeEnum.Button;
            int firstRow = grid1.Selection.FirstRow;
            int firstCol = grid1.Selection.FirstCol;
            int lastRow = grid1.Selection.LastRow;
            int lastCol = grid1.Selection.LastCol;
            
            for (int i = firstRow; i <= lastRow; i++)
            {
                for (int j = firstCol; j <= lastCol; j++)
                {
                    string id = grid1.Cell(i, j).Tag;
                    if ("".Equals(id) || id == null)
                    {
                        ViewRuleDef vrd = new ViewRuleDef();
                        vrd.Main = vm;
                        string sign = KnowledgeUtil.Init_str[j].ToString() + i;
                        vrd.CellSign = sign;
                        set_list.Add(vrd);
                    }
                }
            }

            if(set_list.Count > 0)
            {
                mCube.ViewService.SaveViewRuleDefs(set_list);
                MessageBox.Show("设置定义区完成！");
            }
            
            LoadData();
        }

        private void btnCancelSet_Click(object sender, EventArgs e)
        {
            IList set_list = new ArrayList();

            int firstRow = grid1.Selection.FirstRow;
            int firstCol = grid1.Selection.FirstCol;
            int lastRow = grid1.Selection.LastRow;
            int lastCol = grid1.Selection.LastCol;

            for (int i = firstRow; i <= lastRow; i++)
            {
                for (int j = firstCol; j <= lastCol; j++)
                {
                    string id = grid1.Cell(i, j).Tag;
                    if (!"".Equals(id) && id != null)
                    {
                        set_list.Add(id);
                    }
                }
            }

            if (set_list.Count > 0)
            {
                mCube.ViewService.DeleteViewRuleDefs(set_list);
            }
            MessageBox.Show("取消设置定义区完成！");
            LoadData();
        }

        private void grid1_MouseUp(object Sender, MouseEventArgs e)
        {
            string id_link = grid1.ActiveCell.Text;
            if (id_link != null && id_link.IndexOf("_") != -1)
            {
                string name = model.TransIdToName(id_link, ht_dim);
                txtExpress.Text = name;
            }
            else if (id_link != null && id_link.IndexOf("[") != -1)
            {
                txtExpress.Text = id_link;
            }
            else
            {
                txtExpress.Text = "";
            }
        }

        private void btnSetTime_Click(object sender, EventArgs e)
        {
            ViewRuleDef vrd = new ViewRuleDef();
            vrd.Main = vm;

            FlexCell.Cell curr_cell = grid1.ActiveCell;
            string id = curr_cell.Tag;
            if ("".Equals(id) || id == null)
            {
                int curr_row = curr_cell.Row;
                int curr_col = curr_cell.Col;
                string sign = KnowledgeUtil.Init_str[curr_col].ToString() + curr_row;

                vrd.CellSign = sign;
                vrd.DisplayRule = "{定义时间}";

                mCube.ViewService.SaveViewRuleDef(vrd);
                MessageBox.Show("设置显示时间完成！");
                LoadData();
            }
            else {
                MessageBox.Show("设置显示时间完成！");
            }
        }

        private void btnCancelTime_Click(object sender, EventArgs e)
        {
            IList set_list = new ArrayList();
            FlexCell.Cell curr_cell = grid1.ActiveCell;
            string id = curr_cell.Tag;
            if (!"".Equals(id) && id != null)
            {
                set_list.Add(id);
                mCube.ViewService.DeleteViewRuleDefs(set_list);
                MessageBox.Show("取消显示时间完成！");
                LoadData();
            }
            else {
                MessageBox.Show("取消显示时间完成！");
            }           
        }

        private void btnInsertRow_Click(object sender, EventArgs e)
        {
            grid1.AutoRedraw = false;
            int row = grid1.Selection.FirstRow;
            grid1.InsertRow(row, 1);
            grid1.Range(row, 0, row, grid1.Cols - 1).ClearBackColor();

            if (ht_express.Count > 0)
            {
                ht_express = KnowledgeUtil.ReSetExpress(row + "", (row + 1) + "", ht_express);
            }
            ReDisplayCellExpressByRow(row);
            SetColNumber();
            grid1.AutoRedraw = true;
            grid1.Refresh();
        }

        private void btnDelRow_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == KnowledgeMessageBox.QuestionMessage("是否删除所选行？"))
            {
                grid1.AutoRedraw = false;
                int row = grid1.Selection.FirstRow;
                
                if (ht_express.Count > 0)
                {
                    ht_express = KnowledgeUtil.ReSetExpress(row + "", (row - 1) + "", ht_express);
                }
                ReDisplayCellExpressByRow(row);

                //删除行所在的表达式
                IList del_list = new ArrayList();
                for (int col = 1; col < grid1.Cols; col++)
                {
                    string id = grid1.Cell(row, col).Tag;
                    if (id != null && !"".Equals(id))
                    {
                        del_list.Add(id);
                    }
                }
                if (del_list.Count > 0)
                {
                    mCube.ViewService.DeleteViewRuleDefs(del_list);
                }

                grid1.Range(row, 0, row, 0).DeleteByRow();

                SetColNumber();
                grid1.AutoRedraw = true;
                grid1.Refresh();

                SaveData();
            }
        }

        private void btnInsertCol_Click(object sender, EventArgs e)
        {
            grid1.AutoRedraw = false;
            int col = grid1.Selection.FirstCol;
            grid1.InsertCol(col, 1);

            for (int i = 1; i < grid1.Rows; i++)
            {
                grid1.Cell(i, col).ClearBackColor();
            }

            if (ht_express.Count > 0)
            {
                ht_express = KnowledgeUtil.ReSetExpress(KnowledgeUtil.Init_str[col], KnowledgeUtil.Init_str[col + 1], ht_express);
            }

            ReDisplayCellExpressByCol(col);
            SetColNumber();
            grid1.AutoRedraw = true;
            grid1.Refresh();
        }

        private void btnDelCol_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == KnowledgeMessageBox.QuestionMessage("是否删除所选列？"))
            {
                grid1.AutoRedraw = false;
                int col = grid1.Selection.FirstCol;

                if (ht_express.Count > 0)
                {
                    ht_express = KnowledgeUtil.ReSetExpress(KnowledgeUtil.Init_str[col], KnowledgeUtil.Init_str[col - 1], ht_express);
                }

                ReDisplayCellExpressByCol(col);

                //删除列所在的表达式
                IList del_list = new ArrayList();
                for (int row = 1; row < grid1.Rows; row++)
                {
                    string id = grid1.Cell(row, col).Tag;
                    if (id != null && !"".Equals(id))
                    {
                        del_list.Add(id);
                    }
                }

                if (del_list.Count > 0)
                {
                    mCube.ViewService.DeleteViewRuleDefs(del_list);
                }
                grid1.Range(0, col, 0, col).DeleteByCol();
                SetColNumber();
                grid1.AutoRedraw = true;
                grid1.Refresh();

                SaveData();
            }
        }

        #endregion

        #region 编辑菜单事件
        private void mnuInsertRow_Click(object sender, EventArgs e)
        {
            grid1.Selection.InsertRows();
        }

        private void mnuInsertCol_Click(object sender, EventArgs e)
        {
            grid1.Selection.InsertCols();
            int col = grid1.Cols;
            for (int i = 1; i < col; i++)
            {
                grid1.Cell(0, i).Text = KnowledgeUtil.Init_str[i].ToString();
            }
        }

        private void mnuDeleteRow_Click(object sender, EventArgs e)
        {
            grid1.Selection.DeleteByRow();  
        }

        private void mnuDeleteCol_Click(object sender, EventArgs e)
        {
            grid1.Selection.DeleteByCol();
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
            ClearFlexDef();
            grid1.ExportToExcel("", true, true);
            //MessageBox.Show("输出成功。", "演示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
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
                ClearFlexDef();
                mCube.GeneralPreviewWithNoFix(grid1);
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

        private void mnuHideRow_Click(object sender, EventArgs e)
        {
            /*grid1.AutoRedraw = false;

            for (int i = grid1.Selection.FirstRow; i <= grid1.Selection.LastRow; i++)
            {
                grid1.Row(i).Visible = false;
            }

            grid1.AutoRedraw = true;
            grid1.Refresh();*/
        }

        private void mnuHideCol_Click(object sender, EventArgs e)
        {
            /*grid1.AutoRedraw = false;

            for (int i = grid1.Selection.FirstCol; i <= grid1.Selection.LastCol; i++)
            {
                grid1.Column(i).Visible = false;
            }

            grid1.AutoRedraw = true;
            grid1.Refresh();*/
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
        #endregion

    }
}