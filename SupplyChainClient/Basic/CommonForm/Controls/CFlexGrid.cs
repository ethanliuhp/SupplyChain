using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using C1.Win.C1FlexGrid;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Reflection;
using C1.C1Excel;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.Basic.Controls
{
    [Description("二维表格")]
    public partial class CFlexGrid : C1FlexGrid
    {
        private IList<int> _LockColumnsIndex = new List<int>();
        public CFlexGrid()
        {
            InitializeComponent();
            InitDefaultProperty();
            //初始化表头居中
            foreach (Column tmpColumn in this.Cols)
            {
                tmpColumn.TextAlignFixed = TextAlignEnum.CenterCenter;
            }
        }

        /// <summary>
        /// 初始化默认属性
        /// </summary>
        private void InitDefaultProperty()
        {
            this.SelectionMode = SelectionModeEnum.ListBox;
            this.ShowButtons = ShowButtonsEnum.WhenEditing;
            this.KeyActionEnter = KeyActionEnum.MoveAcross;
            this.KeyActionTab = KeyActionEnum.MoveAcross;
        }

        public CFlexGrid(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            InitDefaultProperty();
        }
        #region 保存至Excel
        /// <summary>
        /// 保存到Excel文件
        /// </summary>
        /// <param name="isOpen">True 打开，False 不打开</param>
        public void SaveToExcelN(bool isOpen)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excel|*.XLS";
            DialogResult result = saveFileDialog1.ShowDialog(this);
            if (result != DialogResult.OK) return;

            this.SaveExcel(saveFileDialog1.FileName, "", FileFlags.IncludeFixedCells);
            if (isOpen)
            {
                Process process = new Process();
                process.StartInfo.FileName = saveFileDialog1.FileName;
                process.Start();
                process.Close();
            }
        }
        /// <summary>
        /// 保存到Excel文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="isOpen">True 打开，False 不打开</param>
        public void SaveToExcel(string fileName, bool isOpen)
        {
            this.SaveExcel(fileName, "", FileFlags.IncludeFixedCells);
            if (isOpen)
            {
                Process process = new Process();
                process.StartInfo.FileName = fileName;
                process.Start();
                process.Close();
            }
        }
        C1.C1Excel.C1XLBook _book = new C1XLBook();

        public void SaveToExcel(bool isOpen)
        {
            ToExcel(isOpen, true);
        }
        #region 保存Excel
        private void ToExcel(bool isOpen, bool fixedCells)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excel|*.XLS";
            saveFileDialog1.DefaultExt = "xls";
            saveFileDialog1.FileName = "*.xls";
            DialogResult result = saveFileDialog1.ShowDialog(this);
            if (result != DialogResult.OK) return;

            // clear book
            _book.Clear();
            _book.Sheets.Clear();

            XLSheet sheet = _book.Sheets.Add("Sheet1");
            SaveSheet(this, sheet, fixedCells);
            // save the book
            _book.Save(saveFileDialog1.FileName);
            if (isOpen)
            {
                Process process = new Process();
                process.StartInfo.FileName = saveFileDialog1.FileName;
                process.Start();
                process.Close();
            }
        }
        int delRow = 0;
        protected override void OnBeforeDeleteRow(RowColEventArgs e)
        {
            base.OnBeforeDeleteRow(e);
            delRow = e.Row;
        }

        protected override void OnAfterDeleteRow(RowColEventArgs e)
        {
            base.OnAfterDeleteRow(e);
            //选择上一行
            if (this.Rows.Count > 1)
            {
                this.Rows[delRow - 1].Selected = true;
            }
        }

        private void SaveSheet(C1FlexGrid flex, XLSheet sheet, bool fixedCells)
        {
            // account for fixed cells
            int frows = flex.Rows.Fixed;
            int fcols = flex.Cols.Fixed;
            if (fixedCells) frows = fcols = 0;

            // copy dimensions
            int lastRow = flex.Rows.Count - frows - 1;
            int lastCol = flex.Cols.Count - fcols - 1;
            if (lastRow < 0 || lastCol < 0) return;
            XLCell cell = sheet[lastRow, lastCol];

            // set default properties
            sheet.Book.DefaultFont = flex.Font;
            sheet.DefaultRowHeight = C1XLBook.PixelsToTwips(flex.Rows.DefaultSize);
            sheet.DefaultColumnWidth = C1XLBook.PixelsToTwips(flex.Cols.DefaultSize);

            // prepare to convert styles
            _styles = new Hashtable();

            // set row/column properties
            for (int r = frows; r < flex.Rows.Count; r++)
            {
                // size/visibility
                Row fr = flex.Rows[r];
                XLRow xr = sheet.Rows[r - frows];
                if (fr.Height >= 0)
                    xr.Height = C1XLBook.PixelsToTwips(fr.Height);
                xr.Visible = fr.Visible;

                // style
                XLStyle xs = StyleFromFlex(fr.Style);
                if (xs != null)
                    xr.Style = xs;
            }
            for (int c = fcols; c < flex.Cols.Count; c++)
            {
                // size/visibility
                Column fc = flex.Cols[c];
                XLColumn xc = sheet.Columns[c - fcols];
                if (fc.Width >= 0)
                    xc.Width = C1XLBook.PixelsToTwips(fc.Width);
                xc.Visible = fc.Visible;

                // style
                XLStyle xs = StyleFromFlex(fc.Style);
                if (xs != null)
                    xc.Style = xs;
            }

            // load cells
            for (int r = frows; r < flex.Rows.Count; r++)
            {
                for (int c = fcols; c < flex.Cols.Count; c++)
                {
                    // get cell
                    cell = sheet[r - frows, c - fcols];

                    // apply content
                    cell.Value = flex[r, c];

                    // apply style
                    XLStyle xs = StyleFromFlex(flex.GetCellStyle(r, c));
                    if (xs != null)
                        cell.Style = xs;
                }
            }
        }

        // convert FlexGrid style into Excel style
        private XLStyle StyleFromFlex(CellStyle style)
        {
            // sanity
            if (style == null)
                return null;

            // look it up on list
            if (_styles.Contains(style))
                return _styles[style] as XLStyle;

            // create new Excel style
            XLStyle xs = new XLStyle(_book);

            // set up new style
            xs.Font = style.Font;
            if (style.BackColor.ToArgb() != SystemColors.Window.ToArgb())
            {
                //xs.BackColor = style.BackColor;
            }
            xs.WordWrap = style.WordWrap;
            xs.Format = XLStyle.FormatDotNetToXL(style.Format);
            switch (style.TextDirection)
            {
                case TextDirectionEnum.Up:
                    xs.Rotation = 90;
                    break;
                case TextDirectionEnum.Down:
                    xs.Rotation = 180;
                    break;
            }
            switch (style.TextAlign)
            {
                case TextAlignEnum.CenterBottom:
                    xs.AlignHorz = XLAlignHorzEnum.Center;
                    xs.AlignVert = XLAlignVertEnum.Bottom;
                    break;
                case TextAlignEnum.CenterCenter:
                    xs.AlignHorz = XLAlignHorzEnum.Center;
                    xs.AlignVert = XLAlignVertEnum.Center;
                    break;
                case TextAlignEnum.CenterTop:
                    xs.AlignHorz = XLAlignHorzEnum.Center;
                    xs.AlignVert = XLAlignVertEnum.Top;
                    break;
                case TextAlignEnum.GeneralBottom:
                    xs.AlignHorz = XLAlignHorzEnum.General;
                    xs.AlignVert = XLAlignVertEnum.Bottom;
                    break;
                case TextAlignEnum.GeneralCenter:
                    xs.AlignHorz = XLAlignHorzEnum.General;
                    xs.AlignVert = XLAlignVertEnum.Center;
                    break;
                case TextAlignEnum.GeneralTop:
                    xs.AlignHorz = XLAlignHorzEnum.General;
                    xs.AlignVert = XLAlignVertEnum.Top;
                    break;
                case TextAlignEnum.LeftBottom:
                    xs.AlignHorz = XLAlignHorzEnum.Left;
                    xs.AlignVert = XLAlignVertEnum.Bottom;
                    break;
                case TextAlignEnum.LeftCenter:
                    xs.AlignHorz = XLAlignHorzEnum.Left;
                    xs.AlignVert = XLAlignVertEnum.Center;
                    break;
                case TextAlignEnum.LeftTop:
                    xs.AlignHorz = XLAlignHorzEnum.Left;
                    xs.AlignVert = XLAlignVertEnum.Top;
                    break;
                case TextAlignEnum.RightBottom:
                    xs.AlignHorz = XLAlignHorzEnum.Right;
                    xs.AlignVert = XLAlignVertEnum.Bottom;
                    break;
                case TextAlignEnum.RightCenter:
                    xs.AlignHorz = XLAlignHorzEnum.Right;
                    xs.AlignVert = XLAlignVertEnum.Center;
                    break;
                case TextAlignEnum.RightTop:
                    xs.AlignHorz = XLAlignHorzEnum.Right;
                    xs.AlignVert = XLAlignVertEnum.Top;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            // save it
            _styles.Add(style, xs);

            // return it
            return xs;
        }
        #endregion

        #endregion
        #region 读取Excel
        public void ReadExcel()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = "xls";
            dlg.FileName = "*.xls";
            dlg.Filter = "Excel|*.XLS";

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            _book.Clear();
            _book.Load(dlg.FileName);

            foreach (XLSheet sheet in _book.Sheets)
            {
                LoadSheet(this, sheet, true);
            }
        }
        private void LoadSheet(C1FlexGrid flex, XLSheet sheet, bool fixedCells)
        {
            // account for fixed cells
            int frows = flex.Rows.Fixed;
            int fcols = flex.Cols.Fixed;

            // copy dimensions
            flex.Rows.Count = sheet.Rows.Count + frows;
            flex.Cols.Count = sheet.Columns.Count + fcols;

            // initialize fixed cells
            if (fixedCells && frows > 0 && fcols > 0)
            {
                flex.Styles.Fixed.TextAlign = TextAlignEnum.CenterCenter;
                for (int r = 1; r < flex.Rows.Count; r++)
                {
                    flex[r, 0] = r;
                }
                for (int c = 1; c < flex.Cols.Count; c++)
                {
                    string hdr = string.Format("{0}", (char)('A' + c - 1));
                    flex[0, c] = hdr;
                }
            }

            // set default properties
            flex.Font = sheet.Book.DefaultFont;
            flex.Rows.DefaultSize = C1XLBook.TwipsToPixels(sheet.DefaultRowHeight);
            flex.Cols.DefaultSize = C1XLBook.TwipsToPixels(sheet.DefaultColumnWidth);

            // prepare to convert styles
            _styles = new Hashtable();

            // set row/column properties
            for (int r = 0; r < sheet.Rows.Count; r++)
            {
                // size/visibility
                Row fr = flex.Rows[r + frows];
                XLRow xr = sheet.Rows[r];
                if (xr.Height >= 0)
                    fr.Height = C1XLBook.TwipsToPixels(xr.Height);
                fr.Visible = xr.Visible;

                // style
                CellStyle cs = StyleFromExcel(flex, xr.Style);
                if (cs != null)
                {
                    //cs.DefinedElements &= ~StyleElementFlags.TextAlign; // << need to fix the grid
                    fr.Style = cs;
                }
            }
            for (int c = 0; c < sheet.Columns.Count; c++)
            {
                // size/visibility
                Column fc = flex.Cols[c + fcols];
                XLColumn xc = sheet.Columns[c];
                if (xc.Width >= 0)
                    fc.Width = C1XLBook.TwipsToPixels(xc.Width);
                fc.Visible = xc.Visible;

                // style
                CellStyle cs = StyleFromExcel(flex, xc.Style);
                if (cs != null)
                {
                    //cs.DefinedElements &= ~StyleElementFlags.TextAlign; // << need to fix the grid
                    fc.Style = cs;
                }
            }

            // load cells
            for (int r = 0; r < sheet.Rows.Count; r++)
            {
                for (int c = 0; c < sheet.Columns.Count; c++)
                {
                    // get cell
                    XLCell cell = sheet.GetCell(r, c);
                    if (cell == null) continue;

                    // apply content
                    flex[r + frows, c + fcols] = cell.Value;

                    // apply style
                    CellStyle cs = StyleFromExcel(flex, cell.Style);
                    if (cs != null)
                        flex.SetCellStyle(r + frows, c + fcols, cs);
                }
            }
        }
        private CellStyle StyleFromExcel(C1FlexGrid flex, XLStyle style)
        {
            // sanity
            if (style == null)
                return null;

            // look it up on list
            if (_styles.Contains(style))
                return _styles[style] as CellStyle;

            // create new flex style
            CellStyle cs = flex.Styles.Add(_styles.Count.ToString());

            // set up new style
            if (style.Font != null) cs.Font = style.Font;
            if (style.ForeColor != Color.Transparent) cs.ForeColor = style.ForeColor;
            if (style.BackColor != Color.Transparent) cs.BackColor = style.BackColor;
            if (style.Rotation == 90) cs.TextDirection = TextDirectionEnum.Up;
            if (style.Rotation == 180) cs.TextDirection = TextDirectionEnum.Down;
            if (style.Format != null && style.Format.Length > 0)
                cs.Format = XLStyle.FormatXLToDotNet(style.Format);
            switch (style.AlignHorz)
            {
                case XLAlignHorzEnum.Center:
                    cs.WordWrap = style.WordWrap;
                    switch (style.AlignVert)
                    {
                        case XLAlignVertEnum.Top:
                            cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterTop;
                            break;
                        case XLAlignVertEnum.Center:
                            cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
                            break;
                        default:
                            cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterBottom;
                            break;
                    }
                    break;
                case XLAlignHorzEnum.Right:
                    cs.WordWrap = style.WordWrap;
                    switch (style.AlignVert)
                    {
                        case XLAlignVertEnum.Top:
                            cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightTop;
                            break;
                        case XLAlignVertEnum.Center:
                            cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter;
                            break;
                        default:
                            cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightBottom;
                            break;
                    }
                    break;
                case XLAlignHorzEnum.Left:
                    cs.WordWrap = style.WordWrap;
                    switch (style.AlignVert)
                    {
                        case XLAlignVertEnum.Top:
                            cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftTop;
                            break;
                        case XLAlignVertEnum.Center:
                            cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter;
                            break;
                        default:
                            cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftBottom;
                            break;
                    }
                    break;
                default:
                    cs.WordWrap = style.WordWrap;
                    switch (style.AlignVert)
                    {
                        case XLAlignVertEnum.Top:
                            cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.GeneralTop;
                            break;
                        case XLAlignVertEnum.Center:
                            cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.GeneralCenter;
                            break;
                        default:
                            cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.GeneralBottom;
                            break;
                    }
                    break;
            }

            // save it
            _styles.Add(style, cs);

            // return it
            return cs;
        }
        Hashtable _styles;
        #endregion

        #region 冻结末尾行
        private int freezeBottom;
        /// <summary>
        /// 冻结的末尾行数
        /// </summary>
        [Description("冻结的末尾行数"), DefaultValue(0), Category("Custom")]
        virtual public int FreezeBottom
        {
            get { return freezeBottom; }
            set
            {
                freezeBottom = value;
                FlexFreezeBottom ffb = new FlexFreezeBottom(this, value);
            }
        }
        #endregion

        #region 锁定列
        private Color lockColumnBackColor = Color.White;
        /// <summary>
        /// 锁定列的背景颜色
        /// </summary>
        virtual public Color LockColumnBackColor
        {
            get { return lockColumnBackColor; }
            set { lockColumnBackColor = value; }
        }


        #region 锁定列的Index
        private IList<int> lockColumnsIndex;
        /// <summary>
        /// 锁定列Index
        /// </summary>
        virtual public IList<int> LockColumnsIndex
        {
            get { return lockColumnsIndex; }
            set
            {
                lockColumnsIndex = value;
                if (lockColumnsIndex != null)
                {
                    //统一转换成_LockColumnsIndex
                    foreach (int var in value)
                    {
                        if (!_LockColumnsIndex.Contains(var))
                        {
                            _LockColumnsIndex.Add(var);
                        }
                    }
                    SetLockColumnBackColor();
                }
            }
        }
        private int lockColumnIndex;
        /// <summary>
        /// 锁定单个列的Index
        /// </summary>
        virtual public int LockColumnIndex
        {
            get { return lockColumnIndex; }
            set
            {
                lockColumnIndex = value;
                if (lockColumnIndex != null)
                {
                    if (!_LockColumnsIndex.Contains(value))
                    {
                        _LockColumnsIndex.Add(value);
                    }
                    SetLockColumnBackColor();
                }
            }
        }
        #endregion

        #region 锁定列的Name
        private IList<string> lockColumnsName;
        /// <summary>
        /// 锁定列Name
        /// </summary>
        virtual public IList<string> LockColumnsName
        {
            get { return lockColumnsName; }
            set
            {
                lockColumnsName = value;
                if (value != null)
                {
                    //统一转换成_LockColumnsIndex
                    foreach (string var in value)
                    {
                        if (!_LockColumnsIndex.Contains(this.Cols[var].Index))
                        {
                            _LockColumnsIndex.Add(this.Cols[var].Index);
                        }
                    }
                    SetLockColumnBackColor();
                }
            }
        }
        private string lockColumnName;

        virtual public string LockColumnName
        {
            get { return lockColumnName; }
            set
            {
                lockColumnName = value;
                if (value != null && !_LockColumnsIndex.Contains(this.Cols[value].Index))
                {
                    _LockColumnsIndex.Add(this.Cols[value].Index);
                }
                SetLockColumnBackColor();
            }
        }

        #endregion

        private void SetLockColumnBackColor()
        {
            CellStyle LockColumn = this.Styles.Add("LockColumn");
            LockColumn.BackColor = lockColumnBackColor;
            foreach (int c in _LockColumnsIndex)
            {
                this.Cols[c].Style = LockColumn;
            }
        }

        protected override void OnBeforeSelChange(RangeEventArgs e)
        {

            // check if the new selection contains any bad columns
            int badCol = -1;
            for (int c = e.NewRange.LeftCol; c <= e.NewRange.RightCol; c++)
            {
                if (_LockColumnsIndex.Contains(c))
                {
                    badCol = c;
                    break;
                }
            }

            // if so, cancel the selection and skip over the locked columns
            if (badCol > -1)
            {
                // cancel the selection
                e.Cancel = true;

                // try skipping over to the right
                if (this.Col < badCol)
                {
                    for (int c = badCol + 1; c < this.Cols.Count; c++)
                    {
                        if (!_LockColumnsIndex.Contains(c))
                        {
                            this.Select(e.NewRange.TopRow, c);
                            break;
                        }
                    }
                    return;
                }

                // try skipping to the left
                if (this.Col > badCol)
                {
                    for (int c = badCol - 1; c >= this.Cols.Fixed; c--)
                    {
                        if (!_LockColumnsIndex.Contains(c))
                        {
                            this.Select(e.NewRange.TopRow, c);
                            break;
                        }
                    }
                    return;
                }
            }
            base.OnBeforeSelChange(null);

        }
        protected override void OnBeforeDoubleClick(BeforeMouseDownEventArgs e)
        {

            if (_LockColumnsIndex.Contains(this.MouseCol - 1))
            {
                e.Cancel = true;
            }
            base.OnBeforeDoubleClick(null);
        }
        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            foreach (HostedControl hosted in HostedCtl)
                hosted.UpdatePosition();
            base.OnPaint(e);
        }

        private ArrayList HostedCtl = new ArrayList();

    }
    public class HostedControl
    {
        internal C1FlexGrid _flex;
        internal Control _ctl;
        internal Row _row;
        internal Column _col;

        internal HostedControl(C1FlexGrid flex, Control hosted, int row, int col)
        {
            // save info
            _flex = flex;
            _ctl = hosted;
            _row = flex.Rows[row];
            _col = flex.Cols[col];

            // insert hosted control into grid
            _flex.Controls.Add(_ctl);
        }
        internal bool UpdatePosition()
        {
            // get row/col indices
            int r = _row.Index;
            int c = _col.Index;
            if (r < 0 || c < 0) return false;

            // get cell rect
            Rectangle rc = _flex.GetCellRect(r, c, false);

            // hide control if out of range
            if (rc.Width <= 0 || rc.Height <= 0 || !rc.IntersectsWith(_flex.ClientRectangle))
            {
                _ctl.Visible = false;
                return true;
            }

            // move the control and show it
            _ctl.Bounds = rc;
            _ctl.Visible = true;
            // done
            return true;
        }
    }
}
