using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;
using System.Collections;
using C1.C1Excel;
using System.Diagnostics;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using NHibernate;
using VirtualMachine.Core.DataAccess;
using System.Runtime.Remoting.Messaging;
using Application.Business.Erp.SupplyChain.Client.Util;

namespace Application.Business.Erp.SupplyChain.Client.FinanceMng
{
    public delegate void ReturnClick(C1FlexGrid aFlex);
    public partial class VImprotData : Form
    {
        public event ReturnClick ReturnClick;
        IList lstCaption = new ArrayList();

        public VImprotData(IList aCaption)
        {
            InitializeComponent();
            this.btnExcel.Click += new EventHandler(button1_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
            lstCaption = aCaption;
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("是否导入数据？", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.No) return;

            if (ReturnClick != null)
                ReturnClick(this._tab.SelectedTab.Controls[0] as C1FlexGrid);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // choose file
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = "xls";
            dlg.FileName = "*.xls";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            // clear everything
            _book.Clear();
            _tab.TabPages.Clear();

            // load book
            _book.Load(dlg.FileName);

            // create one grid per sheet and add them to listbox
            foreach (XLSheet sheet in _book.Sheets)
            {
                // create a new grid for this sheet
                C1FlexGrid flex = new C1FlexGrid();
                flex.BorderStyle = _flex.BorderStyle;
                flex.AllowMerging = _flex.AllowMerging;
                flex.Dock = _flex.Dock;
                flex.AllowDelete = true;
                flex.SelectionMode = SelectionModeEnum.ListBox;
                // load sheet into new grid
                LoadSheet(flex, sheet, false);

                // add new grid to the list
                TabPage pg = new TabPage();
                pg.Text = sheet.Name;
                flex.Name = sheet.Name;
                pg.Controls.Add(flex);
                _tab.TabPages.Add(pg);
                flex.AutoSizeCols();
                AddDefaultCaption(flex);
                AddSerial(flex);
                flex.BeforeSort += new SortColEventHandler(flex_BeforeSort);
                break;
            }

            // select current sheet
            _tab.SelectedIndex = _book.Sheets.SelectedIndex;
        }

        void flex_BeforeSort(object sender, SortColEventArgs e)
        {
        }

        private void AddSerial(C1FlexGrid aFlex)
        {
            for (int i = 1; i < aFlex.Rows.Count; i++)
            {
                aFlex[i, 0] = i;
            }
        }


        private void AddDefaultCaption(C1FlexGrid aFlex)
        {

            for (int i = 1; i < aFlex.Cols.Count; i++)
            {
                if (i > lstCaption.Count) break;
                aFlex.Cols[i].Caption = lstCaption[i - 1].ToString();
                aFlex.Cols[i].Name = lstCaption[i - 1].ToString();
            }
        }


        #region ** Load an XLSheet into a C1FlexGrid

        Hashtable _styles;

        // load sheet into grid
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

        // convert Excel style into FlexGrid style
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
        #endregion

        //===========================================================================================
        #region ** Save a C1FlexGrid into an XLSheet
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
                xs.BackColor = style.BackColor;
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

    }
}
