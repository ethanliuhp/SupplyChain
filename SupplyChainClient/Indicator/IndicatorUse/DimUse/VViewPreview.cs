using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;
using System.Collections;
using Oracle.DataAccess.Client;
using VirtualMachine.Component.WinControls.Controls;
using FlexCell;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    public partial class VViewPreview : Form
    {
        private MIndicatorUse model = new MIndicatorUse();
        private MCubeManager mCube = new MCubeManager();

        /// <summary>
        /// 选中的维度
        /// </summary>
        public Hashtable checkedDim = new Hashtable();
        /// <summary>
        /// 视图格式集合
        /// </summary>
        public IList vStyleList = new ArrayList();
        /// <summary>
        /// 视图主表信息
        /// </summary>
        public ViewMain vm = new ViewMain();
        
        public VViewPreview()
        {
            InitializeComponent();
        }

        private void VViewPreview_Load(object sender, EventArgs e)
        {
            if (vm == null)
            {
                MessageBox.Show("请先保存该模板！");
                return;
            }
            if (vm.CubeRegId.Id == "1")
            {
                InitialDataGridViewByJJZB();
            }
            else
            {
                InitialDataGridView();
            }
            //InitialDataGridView2(customFlexGrid1);
        }

        /// <summary>
        /// 生成预览格式
        /// </summary>
        private void InitialDataGridView()
        {
            try
            {
                string ifDisplaySonMother = vm.IfDisplaySonMother;//是否显示子项母项
                grid1.AutoRedraw = false;//修改自动重画属性
                grid1.AllowUserReorderColumn = true;

                IList style_l_list = new ArrayList();//列格式集合
                IList style_h_list = new ArrayList();//行格式集合

                int rowDims = 0;//行的维数
                int colDims = 0;//列的维数
                int rows = 1;//行维的总行数
                int cols = 1;//列维的总列数

                //取得行列维数
                if (vStyleList != null && vStyleList.Count > 0)
                {
                    foreach (ViewStyle vs in vStyleList)
                    {
                        IList styleMxList = checkedDim[vs.Id] as IList;

                        vs.Details = styleMxList;
                        if (vs.Direction.Equals("行"))
                        {
                            style_h_list.Add(vs);
                            rows = rows * styleMxList.Count;
                            rowDims++;
                        }
                        else
                        {
                            style_l_list.Add(vs);
                            cols = cols * styleMxList.Count;
                            colDims++;
                        }
                    }
                }

                //写入行列的数据
                //1：初始化表格的单元格
                int totalRows = colDims + rows + 1;//表格的总行数
                int totalCols = rowDims + cols + 1;//表格的总列数，1：为记录数显示
                grid1.Rows = totalRows;
                grid1.Cols = totalCols;

                //写入行标题
                /*int k = 1;
                foreach (ViewStyle vs in vStyleList)
                {
                    if (vs.Direction.Equals("行"))
                    {
                        grid1.Cell(colDims, k).Text = vs.OldCatRootName;
                        grid1.Cell(colDims, k).BackColor = System.Drawing.Color.LightGray;
                        grid1.Cell(colDims, k).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                        k++;
                    }
                }*/

                if (rowDims > 0 && colDims > 0)
                {
                    //2：写入固定区（空白部分）
                    FlexCell.Range range = new FlexCell.Range();
                    range = grid1.Range(1, 1, colDims - 1, rowDims);//表格左上角的空白区
                    range.Merge();
                }


                //写入行列维度值   
                int startColLocation = rowDims + 1;//起始列标题位置
                int startRowLocation = colDims + 1;//起始行标题位置
                mCube.FillGridCell(grid1, 1, 1, startColLocation, style_l_list);//填充列标题
                mCube.FillGridCell(grid1, 2, startRowLocation, 1, style_h_list);//填充行标题

                //合并相同的行列
                mCube.MergeCell(grid1, 1, 1, startColLocation, colDims, startColLocation + cols);//合并列标题
                mCube.MergeCell(grid1, 2, startRowLocation, 1, startRowLocation + rows, rowDims);//合并行标题

                grid1.Refresh();
                grid1.AutoRedraw = true;//重画属性复原
            }
            catch (Exception ex)
            {
                KnowledgeMessageBox.InforMessage("生成视图出错。", ex);
            }
        }

        /// <summary>
        /// 生成预览格式-技经指标模板
        /// </summary>
        private void InitialDataGridViewByJJZB()
        {
            try
            {
                string ifDisplaySonMother = vm.IfDisplaySonMother;//是否显示子项母项
                grid1.AutoRedraw = false;//修改自动重画属性
                grid1.AllowUserReorderColumn = true;

                IList style_l_list = new ArrayList();//列格式集合
                IList style_h_list = new ArrayList();//行格式集合

                int rowDims = 0;//行的维数
                int colDims = 0;//列的维数
                int rows = 1;//行维的总行数
                int cols = 1;//列维的总列数

                IList style_mx_list = new ArrayList();
                if ("是".Equals(vm.IfDisplaySonMother))
                {
                    mCube.AddViewStyleBySonMother(vm, vStyleList);
                }

                //取得行列维数
                if (vStyleList != null && vStyleList.Count > 0)
                {
                    foreach (ViewStyle vs in vStyleList)
                    {
                        IList styleMxList = checkedDim[vs.Id] as IList;
                        if (vs.OldCatRootName.IndexOf("子母项") != -1)
                        {
                            styleMxList = vs.Details;
                        }

                        vs.Details = styleMxList;
                        if (vs.Direction.Equals("行"))
                        {
                            style_h_list.Add(vs);
                            rows = rows * styleMxList.Count;
                            rowDims++;
                        }
                        else
                        {
                            style_l_list.Add(vs);
                            cols = cols * styleMxList.Count;
                            colDims++;
                        }
                    }
                }

                
                

                //写入行列的数据
                //1：初始化表格的单元格
                int totalRows = colDims + rows + 1;//表格的总行数
                int totalCols = rowDims + cols + 1 + 1;//表格的总列数，1：为记录数显示,还有1为：计量单位
                grid1.Rows = totalRows;
                grid1.Cols = totalCols;

                if (rowDims > 0 && colDims > 0)
                {
                    //2：写入固定区（空白部分）
                    FlexCell.Range range = new FlexCell.Range();
                    range = grid1.Range(1, 1, colDims - 1, rowDims);//表格左上角的空白区
                    range.Merge();
                }


                //写入行列维度值   
                int startColLocation = rowDims + 1 + 1;//起始列标题位置
                int startRowLocation = colDims + 1;//起始行标题位置
                
                mCube.FillGridCellByJJZB(grid1, 1, 1, startColLocation, style_l_list);//填充列标题
                mCube.FillGridCellByJJZB(grid1, 2, startRowLocation, 1, style_h_list);//填充行标题

                //合并相同的行列
                mCube.MergeCell(grid1, 1, 1, startColLocation, colDims, startColLocation + cols);//合并列标题
                mCube.MergeCell(grid1, 2, startRowLocation, 1, startRowLocation + rows, rowDims);//合并行标题

                grid1.Refresh();
                grid1.AutoRedraw = true;//重画属性复原
            }
            catch (Exception ex)
            {
                KnowledgeMessageBox.InforMessage("生成视图出错。", ex);
            }
        }

        private void InitialDataGridView2(CustomFlexGrid grid)
        {
            try
            {
                grid.AutoRedraw = false;//修改自动重画属性
                int gridCols = 1;

                grid.Images.Add(Properties.Resources.ImageExpend, "ImageExpend");
                grid.Images.Add(Properties.Resources.ImageFold, "ImageFold");

                //生成表格列
                foreach (ViewStyle vs in vStyleList)
                {
                    if (vs.Direction.Equals("行"))
                    {
                        grid.Cell(0, gridCols).Text = vs.OldCatRootName;
                        //grid1.Column(gridCols).Locked = true;
                        gridCols++;
                    }
                }

                grid.Cols = gridCols;
                grid.DisplayRowNumber = true;

                IList rowValueLists = model.Gen2DimTable(vStyleList, checkedDim);
                grid.Rows = rowValueLists.Count + 1;

                for (int i = 1; i <= rowValueLists.Count; i++)
                {
                    IList rowValueList = rowValueLists[i - 1] as IList;
                    for (int j = 1; j <= rowValueList.Count; j++)
                    {
                        object cellValue = rowValueList[j - 1];
                        if (cellValue == null)
                        {
                            grid1.Cell(i, j).Text = "";
                        }
                        else
                        {
                            grid.Cell(i, j).Text = cellValue.ToString();
                            //grid.Cell(i, j).CellType=FlexCell.CellTypeEnum. SetImage("ImgAdd");
                        }
                        grid.Cell(i, j).BackColor = System.Drawing.Color.LightGray;
                    }
                }

                //合并相同的行
                for (int colJ = 1; colJ < grid.Cols; colJ++)
                {
                    FlexCell.Range range = new FlexCell.Range();

                    for (int i = 1; i < grid.Rows; i++)
                    {
                        if (range.FirstRow == 0)
                        {
                            range = grid1.Range(i, colJ, i, colJ);
                        }

                        string currCellValue = grid.Cell(i, colJ).Text;
                        string nextCellValue = "";
                        if (i + 1 <= grid1.Rows)
                        {
                            nextCellValue = grid.Cell(i + 1, colJ).Text;
                        }
                        if (currCellValue == nextCellValue)
                        {
                            range = grid.Range(range.FirstRow, colJ, i, colJ);
                        }
                        else
                        {
                            range = grid.Range(range.FirstRow, colJ, i, colJ);
                            range.Merge();
                            range = grid.Range(0, colJ, 0, colJ);
                        }
                    }

                }

                grid.AutoRedraw = true;//重画属性复原
                grid.Refresh();
            }
            catch (Exception ex)
            {
                KnowledgeMessageBox.InforMessage("生成视图出错。", ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}