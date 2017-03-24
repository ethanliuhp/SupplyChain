﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Properties;
using VirtualMachine.Component.Util;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using VirtualMachine.Component.WinControls.Controls;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng;
using System.IO;
using FlexCell;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng;
using System.Reflection;
using VirtualMachine.Component.WinMVC.core;
using Application.Business.Erp.SupplyChain.Client.Main;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VGWBSAddByBatch : TBasicDataView
    {
        //对应列的Tag值说明
        //(endImageCol + 1)：0:任务明细数据；1：叶节点；2根节点、中间节点
        //(endImageCol + 2)：状态【1，2】;
        //(endImageCol + 3)：该节点下明细数量【仅节点有】;
        //(endImageCol + 4)：非明细时为节点下的最大序号；明细时为当前节点的序号，;
        //(endImageCol + 5)：当前ID;
        //(endImageCol + 6)：父节点ID;
        //(endImageCol + 7)：当前节点层次 ;
        //(endImageCol + 8)：名称的列号
        //(endImageCol + 9)：是否填写完整【0：否，1：是】
        //(endImageCol + 10)：父节点index
        CurrentProjectInfo projectInfo = null;
        public MGWBSTree model = new MGWBSTree();

        private int titleRows = 3, endImageCol = 19;
        private string imageExpand = "imageExpand";
        private string imageCollapse = "imageCollapse";

        /**/
        List<int> lstRowIndex = new List<int>();//新增行行号集合
        private GWBSTree OptGWBSTreeObj = null;

        /// <summary>
        /// 区域
        /// </summary>
        private IList zoningList = new ArrayList();
        /// <summary>
        /// 契约
        /// </summary>
        public ContractGroup contract = null;

        ///// <summary>
        ///// 当前tree对应的所有成本项信息
        ///// </summary>
        //List<CostItem> lstCostItem = null;
        ///// <summary>
        ///// 当前tree对应的所有资源耗用信息
        ///// </summary>
        //List<SubjectCostQuota> lstSubjectCostQuota = null;
        DataTable dtDetailCostSubject = null;
        /// <summary>
        /// Import表中当前项目下的所有记录
        /// </summary>
        List<GWBSDetailImport> lstImportAll = new List<GWBSDetailImport>();
        private List<GWBSTree> lstMGWBSTree = null;

        /// <summary>
        /// 
        /// </summary>
        IList<GWBSTreeTemp> lstTree = new List<GWBSTreeTemp>();

        /// <summary>
        /// 新增的详细信息数据提交后返回的数据
        /// </summary>
        //List<GWBSDetail> lstDBTreeDetailNew = new List<GWBSDetail>();

        List<GWBSDetailImport> lstDBDetailImportNew = new List<GWBSDetailImport>();

        /// <summary>
        /// 新增数据【以保存的和本次新增的】定额编号相关的资源耗用
        /// </summary>
        List<SubjectCostQuotaTemp> lstRelatedSubjectCostQuotaTemp = new List<SubjectCostQuotaTemp>();

        /// <summary>
        /// 赋值行的，
        /// </summary>
        int[] copyedSelection = null;
        /// <summary>
        /// 是否是复制的定额编号数据
        /// </summary>
        bool IsCopyedQuoteCode = false;

        /// <summary>
        /// 成本项集合
        /// </summary>
        List<CostItem> LstCostItem = null;
        /// <summary>
        /// 成本常用项grid的数据
        /// </summary>
        List<CostItem> LstQuoteGridCostItem = null;
        /**/

        public VGWBSAddByBatch()
        {
            InitializeComponent();
            InitData();
            InitEvents();
        }

        private void InitData()
        {
            projectInfo = StaticMethod.GetProjectInfo();

            InitFlexGrid(0, true);

            InitQuoteCodeGrid();
            QueryQuoteCodeData();
        }
        private void InitEvents()
        {
            btnQueryWBS.Click += new EventHandler(btnQuery_Click);
            btnSelectGWBSTreeWBS.Click += new EventHandler(btnSelectGWBSTreeWBS_Click);
            btnExcelWBS.Click += new EventHandler(btnExcel_Click);
            btnSelectContractGroup.Click += new EventHandler(btnSelectContractGroup_Click);
            btnAddRow.Click += new EventHandler(btnAddRow_Click);
            btnDel.Click += new EventHandler(btnDel_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnSubmit.Click += new EventHandler(btnSubmit_Click);
            flexGridWBS.Click += new FlexCell.Grid.ClickEventHandler(FlexGrid_Click);
            flexGridWBS.KeyDown += new Grid.KeyDownEventHandler(flexGridWBS_KeyDown);
            //flexGridWBS.DoubleClick += new Grid.DoubleClickEventHandler(flexGridWBS_DoubleClick);
            flexGridWBS.ButtonClick += new Grid.ButtonClickEventHandler(flexGridWBS_ButtonClick);
            flexGridWBS.SelChange += new Grid.SelChangeEventHandler(flexGridWBS_SelChange);
            flexGridWBS.MouseUp += new Grid.MouseUpEventHandler(flexGridWBS_MouseUp);
            cmsTreeRMenu.ItemClicked += new ToolStripItemClickedEventHandler(cmsTreeRMenu_ItemClicked);

            btnQueryCode.Click += new EventHandler(btnQueryCode_Click);
            grid1QuoteCode.KeyDown += new Grid.KeyDownEventHandler(grid1QuoteCode_KeyDown);

            chbShowAdded.CheckStateChanged += new EventHandler(chbShowAdded_CheckStateChanged);

            btnShowOrNot.Click += new EventHandler(btnShowOrNot_Click);
        }

        void btnShowOrNot_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = !splitContainer1.Panel2Collapsed;
        }

        void chbShowAdded_CheckStateChanged(object sender, EventArgs e)
        {
            btnQuery_Click(new object(), new EventArgs());
        }
        void grid1QuoteCode_KeyDown(object Sender, KeyEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.C)
            {
                IsCopyedQuoteCode = true;
                copyedSelection = new int[4] { grid1QuoteCode.Selection.FirstRow, grid1QuoteCode.Selection.FirstCol, 
                    grid1QuoteCode.Selection.LastRow, grid1QuoteCode.Selection.LastCol };
            }
        }

        void btnQueryCode_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtQuoteCode.Text.Trim()) && string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                if (MessageBox.Show("没有查询条件会降低查询速度，确定要继续吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    txtQuoteCode.Focus();
                    return;
                }
            }
            QueryQuoteCodeData();
        }

        void cmsTreeRMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            cmsTreeRMenu.Close();
            if (e.ClickedItem.Name == 复制.Name)
            {
                IsCopyedQuoteCode = false;
                copyedSelection = new int[4] { flexGridWBS.Selection.FirstRow, flexGridWBS.Selection.FirstCol, flexGridWBS.Selection.LastRow, flexGridWBS.Selection.LastCol };
            }
            else if (e.ClickedItem.Name == 粘贴.Name)
            {
                int selectedRow = flexGridWBS.Selection.FirstRow;
                //如果是叶节点，则直接新增行，然后粘贴数据
                if (flexGridWBS.Cell(flexGridWBS.Selection.FirstRow, endImageCol + 1).Tag == "0")//明细
                {
                    if (lstRowIndex.Any(p => p == selectedRow))
                    {
                        string errmsg = "";
                        if (IsCopyedQuoteCode)
                        {
                            errmsg = PasteDataFromQuoteGrid(selectedRow);
                        }
                        else
                        {
                            errmsg = PasteData(selectedRow);
                        }
                        if (!string.IsNullOrEmpty(errmsg))
                        {
                            WriteLog(errmsg);
                        }
                    }
                    else
                    {
                        MessageBox.Show("请选择叶节点，或是新插入的明细行中空的单元格进行粘贴");
                    }

                } //如果是明细节点，则直接粘贴
                else //if (flexGridWBS.Cell(flexGridWBS.Selection.FirstRow, endImageCol + 1).Tag == "1")//叶节点
                {
                    int newRowIndex = AddNewRow(selectedRow);
                    if (IsCopyedQuoteCode)
                    {
                        PasteDataFromQuoteGrid(newRowIndex);
                    }
                    else
                    {
                        PasteData(newRowIndex);
                    }
                }
                //else
                //{
                //    MessageBox.Show("请选择叶节点，或是新插入的明细行中空的单元格进行粘贴");
                //}
            }
        }

        void flexGridWBS_MouseUp(object Sender, MouseEventArgs e)
        {
            //return;//此功能，待验证
            //如果是在开始结束时间上点击右键时，显示右键菜单
            if (e.Button == MouseButtons.Right)
            {
                int rowindex = flexGridWBS.ActiveCell.Row;
                string namecolIndex = flexGridWBS.Cell(rowindex, endImageCol + 8).Tag;
                //如果是在叶节点上执行右键时显示右键菜单
                if (flexGridWBS.Cell(rowindex, endImageCol + 1).Tag != "0" && flexGridWBS.ActiveCell.Col.ToString() == namecolIndex)
                {
                    复制.Enabled = true;
                    粘贴.Enabled = true;
                }
                else if (lstRowIndex.Any(p => p == rowindex))//新增行
                {
                    复制.Enabled = true;
                    粘贴.Enabled = true;
                }
                else if (flexGridWBS.Row(rowindex).Locked)
                {
                    复制.Enabled = true;
                    粘贴.Enabled = false;
                }
                else
                {
                    复制.Enabled = true;
                    粘贴.Enabled = false;
                }

                cmsTreeRMenu.Show(flexGridWBS, new Point(e.X, e.Y));
            }
        }

        void flexGridWBS_SelChange(object Sender, Grid.SelChangeEventArgs e)
        {
            flexGridWBS.AutoRedraw = false;
            var bgcolor = Color.FromArgb(255, 228, 103);//参照excel的相同效果时的背景色
            //设置当前选择区域对应的表头，行的首列背景色
            Range range = null;
            range = flexGridWBS.Range(1, 0, 2, flexGridWBS.Cols - 1);
            range.FontBold = false;
            //range.BackColor = SystemColors.Control;
            range = flexGridWBS.Range(3, 0, flexGridWBS.Rows - 1, 0);
            //range.BackColor = SystemColors.Control;
            range.FontBold = false;
            range = flexGridWBS.Range(2, e.FirstCol, 2, e.LastCol);
            //range.BackColor = bgcolor;
            range.FontBold = true;
            range = flexGridWBS.Range(e.FirstRow, 0, e.LastRow, 0);
            //range.BackColor = bgcolor;
            range.FontBold = true;

            flexGridWBS.Refresh();
            flexGridWBS.AutoRedraw = true;
            //GridRefresh();
        }

        void flexGridWBS_ButtonClick(object Sender, Grid.ButtonClickEventArgs e)
        {
            string errMsg = "";
            int errRowIndex = 0;
            var rowIndex = flexGridWBS.ActiveCell.Row;
            var colIndex = flexGridWBS.ActiveCell.Col;
            if (rowIndex > titleRows && colIndex == (endImageCol + 1) && flexGridWBS.Row(rowIndex).Locked == false)
            {
                VSelectCostItem frm = new VSelectCostItem(new MCostItem(), false);
                frm.ShowDialog();
                var nameColIndex = ClientUtil.ToInt(flexGridWBS.Cell(rowIndex, endImageCol + 8).Tag);
                if (frm.SelectCostItemList == null)
                {
                    return;
                }
                var curNodeParentRowIndex = ClientUtil.ToInt(flexGridWBS.Cell(rowIndex, endImageCol + 10).Tag); ;
                //foreach (CostItem costitem in frm.SelectCostItemList)
                for (int i = 0; i < frm.SelectCostItemList.Count; i++)
                {
                    CostItem costitem = frm.SelectCostItemList[i] as CostItem;
                    if (costitem != null)
                    {
                        if (i > 0)
                        {
                            rowIndex = AddNewRow(curNodeParentRowIndex);
                        }
                        errRowIndex = rowIndex - 2;
                        flexGridWBS.Cell(rowIndex, nameColIndex).FontBold = false;
                        flexGridWBS.Cell(rowIndex, nameColIndex).Text = costitem.Name;
                        flexGridWBS.Cell(rowIndex, colIndex).Text = costitem.QuotaCode;

                        //if (!LstCostItem.Any(p => p.Id == costitem.Id))
                        //{
                        //    LstCostItem.Add(costitem);
                        //}
                        errMsg += QuoteCodeValid(costitem.QuotaCode, rowIndex);
                    }
                }

                if (!string.IsNullOrEmpty(errMsg))
                {
                    WriteLog(errMsg);
                }
            }
        }

        void flexGridWBS_KeyDown(object Sender, KeyEventArgs e)
        {
            //复制内容范围小于或是等于粘贴内容的有效范围
            //仅有当复制的内容行数小于或等于新增的连续空行数时，才可粘贴           
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.V)
            {
                e.Handled = true;//禁用掉flex默认的粘贴操作
                string errmsg = "";
                int selectedRow = flexGridWBS.Selection.FirstRow;
                //如果是叶节点，则直接新增行，然后粘贴数据
                if (flexGridWBS.Cell(flexGridWBS.Selection.FirstRow, endImageCol + 1).Tag == "0")//明细
                {
                    if (lstRowIndex.Any(p => p == selectedRow))
                    {
                        if (IsCopyedQuoteCode)
                        {
                            errmsg += PasteDataFromQuoteGrid(selectedRow);
                        }
                        else
                        {
                            errmsg += PasteData(selectedRow);
                        }
                    }
                    else
                    {
                        MessageBox.Show("请选择任务节点，或是新插入的明细行中空的单元格进行粘贴");
                    }

                } //如果是明细节点，则直接粘贴
                else //if (flexGridWBS.Cell(flexGridWBS.Selection.FirstRow, endImageCol + 1).Tag == "1")//叶节点
                {
                    int newRowIndex = AddNewRow(selectedRow);
                    if (IsCopyedQuoteCode)
                    {
                        errmsg += PasteDataFromQuoteGrid(newRowIndex);
                    }
                    else
                    {
                        errmsg += PasteData(newRowIndex);
                    }
                }

                if (!string.IsNullOrEmpty(errmsg))
                {
                    WriteLog(errmsg);
                }
                //else
                //{
                //    MessageBox.Show("请选择叶节点，或是新插入的明细行中空的单元格进行粘贴");
                //}
            }
            else if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.C)
            {
                IsCopyedQuoteCode = false;
                copyedSelection = new int[4] { flexGridWBS.Selection.FirstRow, flexGridWBS.Selection.FirstCol, flexGridWBS.Selection.LastRow, flexGridWBS.Selection.LastCol };
            }
        }

        string PasteData(int firstPasteRow)
        {
            //如果小于当前行第一列【明细：合并列】索引，或是大于第一列【明细：合并列】且小于等于endImageCol，则跳过
            //如果等于第一列【明细：合并列】，则赋值，
            //如果是大于等于endImageCol+1【定额编号】，则赋值 
            string errmsg = "";
            int copyedCcolindex = 0;
            int copyedRowindex = copyedSelection[0];
            int curCellRowIndex = firstPasteRow;//当前粘贴数据的行
            int curCellColIndex = 0;//当前粘贴数据的列
            int copyedRowsCount = copyedSelection[2] - copyedSelection[0] + 1;
            int copyedColsCount = copyedSelection[3] - copyedSelection[1] + 1;
            var curRowParentIndex = ClientUtil.ToInt(flexGridWBS.Cell(curCellRowIndex, endImageCol + 10).Tag);
            for (int i = 0; i < copyedRowsCount; i++)
            {
                copyedRowindex = copyedSelection[0] + i;
                copyedCcolindex = copyedSelection[1];
                curCellColIndex = flexGridWBS.Selection.FirstCol;
                var copyedRowNameColIndex = ClientUtil.ToInt(flexGridWBS.Cell(copyedRowindex, endImageCol + 8).Tag);
                var curRowNameColIndex = ClientUtil.ToInt(flexGridWBS.Cell(curCellRowIndex, endImageCol + 8).Tag);
                for (int j = 0; j < copyedColsCount; j++)
                {
                    if (copyedCcolindex == copyedRowNameColIndex || copyedCcolindex > endImageCol)
                    {
                        for (int m = curCellColIndex; m < flexGridWBS.Cols; m++)
                        {
                            curCellColIndex += 1;
                            if (m == curRowNameColIndex || m > endImageCol)
                            {
                                string val = flexGridWBS.Cell(copyedRowindex, copyedCcolindex).Text.Trim();
                                //如果是工程量【数量】则置为空
                                if (m == endImageCol + 3 || m == endImageCol + 3 + 3 || m == endImageCol + 3 + 3 + 3)
                                {
                                    flexGridWBS.Cell(curCellRowIndex, m).Text = "";
                                }
                                else
                                {
                                    flexGridWBS.Cell(curCellRowIndex, m).Text = val;
                                }

                                if (m == endImageCol + 1)//定额编号,则进行校验
                                {
                                    errmsg += QuoteCodeValid(val, curCellRowIndex);
                                }
                                flexGridWBS.Cell(curCellRowIndex, m).FontBold = false;
                                break;
                            }
                        }
                    }
                    copyedCcolindex += 1;
                }
                curCellRowIndex += 1;
                //如果还有待粘贴的内容，则插入行   
                if (i + 1 < copyedRowsCount)
                {
                    //插入行
                    AddNewRow(curRowParentIndex);
                }
            }
            return errmsg;
        }
        string PasteDataFromQuoteGrid(int firstPasteRow)
        {
            //如果小于当前行第一列【明细：合并列】索引，或是大于第一列【明细：合并列】且小于等于endImageCol，则跳过
            //如果等于第一列【明细：合并列】，则赋值，
            //如果是大于等于endImageCol+1【定额编号】，则赋值 
            string errmsg = "";
            int copyedColindex = 0;
            int copyedRowindex = copyedSelection[0];
            int copyedRowsCount = copyedSelection[2] - copyedSelection[0] + 1;
            int copyedColsCount = copyedSelection[3] - copyedSelection[1] + 1;

            int curCellRowIndex = firstPasteRow;// 当前粘贴数据的行
            int curCellColIndex = 0;// 当前粘贴数据的列
            var curRowNameColIndex = 0;//当前粘贴数据的行的明细名称列
            var curRowParentIndex = ClientUtil.ToInt(flexGridWBS.Cell(curCellRowIndex, endImageCol + 10).Tag);
            for (int i = 0; i < copyedRowsCount; i++)
            {
                copyedRowindex = copyedSelection[0] + i;
                copyedColindex = copyedSelection[1];
                curCellColIndex = flexGridWBS.Selection.FirstCol;
                curRowNameColIndex = ClientUtil.ToInt(flexGridWBS.Cell(curCellRowIndex, endImageCol + 8).Tag);
                for (int j = 0; j < copyedColsCount; j++)
                {
                    //右侧的定额编号gird中的列为固定死的1                    
                    if (copyedColindex == 1)//名称列
                    {
                        flexGridWBS.Cell(curCellRowIndex, curRowNameColIndex).Text = grid1QuoteCode.Cell(copyedRowindex, copyedColindex).Text.Trim();
                        flexGridWBS.Cell(curCellRowIndex, curRowNameColIndex).FontBold = false;
                    }
                    else if (copyedColindex == 2)//定额编号列
                    {
                        string quoteCode = grid1QuoteCode.Cell(copyedRowindex, copyedColindex).Text.Trim();
                        flexGridWBS.Cell(curCellRowIndex, endImageCol + 1).Text = quoteCode;
                        flexGridWBS.Cell(curCellRowIndex, endImageCol + 1).FontBold = false;
                        errmsg += QuoteCodeValid(quoteCode, curCellRowIndex);
                    }

                    copyedColindex += 1;
                }
                curCellRowIndex += 1;
                //如果还有待粘贴的内容，则插入行   
                if (i + 1 < copyedRowsCount)
                {
                    //插入行
                    AddNewRow(curRowParentIndex);
                }
            }
            return errmsg;
        }

        void btnDel_Click(object sender, EventArgs e)
        {
            IList lstDel = new ArrayList();
            //
            for (int i = flexGridWBS.Selection.FirstRow; i < flexGridWBS.Selection.LastRow + 1; i++)
            {
                //如果是最后一行空行【保留行】,则跳过不作删除处理
                if (i >= flexGridWBS.Rows - 1)
                {
                    continue;
                }
                if (!lstRowIndex.Any(p => p == i))
                {
                    MessageBox.Show("所选中的行不是新增的数据行，请重新选择！");
                    return;
                }
            }

            int selectionRowCount = flexGridWBS.Selection.LastRow - flexGridWBS.Selection.FirstRow + 1;

            for (int j = lstRowIndex.Count - 1; j >= 0; j--)//行
            {
                if (lstRowIndex[j] >= flexGridWBS.Selection.FirstRow && lstRowIndex[j] <= flexGridWBS.Selection.LastRow)
                {
                    string id = flexGridWBS.Cell(lstRowIndex[j], endImageCol + 5).Tag;
                    //如果选择的删除行是已经保存到了到表中时，则删除表中记录，否则不做删除操作
                    if (!string.IsNullOrEmpty(id))
                    {
                        var obj = lstImportAll.FirstOrDefault(p => p.State == Enum_ImportType.新建 && p.Id == id);
                        if (obj != null)
                        {
                            lstDel.Add(obj);
                        }
                    }
                    lstRowIndex.Remove(lstRowIndex[j]);
                }
                else if (lstRowIndex[j] > flexGridWBS.Selection.LastRow)
                {
                    lstRowIndex[j] = lstRowIndex[j] - selectionRowCount;
                }
            }
            if (lstDel.Count > 0)
            {
                //删除数据库中的行
                model.Delete(lstDel);

                foreach (var item in lstDel.OfType<GWBSDetailImport>().ToList())
                {
                    //删除lstTree集合中相应的的详情信息
                    var node = lstTree.FirstOrDefault(p => p.Id == item.ParentId);
                    node.Details.Remove(node.Details.FirstOrDefault(p => p.ID == item.Id));
                }
            }

            for (int i = flexGridWBS.Selection.FirstRow; i <= flexGridWBS.Selection.LastRow; i++)
            {
                //如果拷贝的是常用定额编号Grid中的数据时，则无需处理对应行，列索引号
                if (copyedSelection != null && IsCopyedQuoteCode == false)
                {
                    if (copyedSelection[0] <= i)
                    {
                        copyedSelection[0] -= 1;
                    }
                    if (copyedSelection[2] <= i)
                    {
                        copyedSelection[2] -= 1;
                    }
                }
                var iPrentIndex = ClientUtil.ToInt(flexGridWBS.Cell(i, endImageCol + 10).Tag);
                var iParentDetailCount = ClientUtil.ToInt(flexGridWBS.Cell(iPrentIndex, endImageCol + 3).Tag);//(endImageCol + 3)：该节点下明细数量【仅节点有】;
                var iParentMaxOrder = ClientUtil.ToInt(flexGridWBS.Cell(iPrentIndex, endImageCol + 4).Tag);//(endImageCol + 4)：非明细时为节点下的最大序号；明细时为当前节点的序号，;
                //更新父节点下的最大序号
                if (ClientUtil.ToInt(flexGridWBS.Cell(i, endImageCol + 4).Tag) == iParentMaxOrder)
                {
                    flexGridWBS.Cell(iPrentIndex, endImageCol + 3).Tag = (iParentMaxOrder - 1).ToString();
                }
                //更新父节点下的子节点数
                flexGridWBS.Cell(iPrentIndex, endImageCol + 3).Tag = (iParentDetailCount - 1).ToString();
                //父节点index调整
                for (int j = i + 1; j < flexGridWBS.Rows; j++)
                {
                    int parentNodeIndex = ClientUtil.ToInt(flexGridWBS.Cell(j, endImageCol + 10).Tag);
                    if (parentNodeIndex >= i)
                    {
                        flexGridWBS.Cell(j, endImageCol + 10).Tag = (parentNodeIndex - 1).ToString();
                    }
                }
            }

            flexGridWBS.Selection.DeleteByRow();
            GridRefresh();
        }

        //导出excel
        void btnExcel_Click(object sender, EventArgs e)
        {
            string sName = "01";
            flexGridWBS.ExportToExcel("", sName, true, true);
        }

        void btnAddRow_Click(object sender, EventArgs e)
        {
            int selectedRowIndex = flexGridWBS.Selection.FirstRow;
            if (flexGridWBS.Cell(selectedRowIndex, endImageCol + 1).Tag == "0")//0:任务明细数据；1：叶节点；2根节点、中间节点
            {
                MessageBox.Show("任务明细不能插入行操作，请选择任务树节点进行操作！");
                return;
            }

            int newRowIndex = AddNewRow(selectedRowIndex);

        }

        /// <summary>
        /// 新增行
        /// </summary>
        /// <param name="selectedRowIndex">新增行的父节点行所在index</param>
        /// <returns>新增行的行号</returns>
        int AddNewRow(int selectedRowIndex)
        {
            //叶节点下详细数据数量
            var detailCount = ClientUtil.ToInt(flexGridWBS.Cell(selectedRowIndex, endImageCol + 3).Tag);
            //叶节点下详细数据最大的排序序号
            var maxNo = ClientUtil.ToInt(flexGridWBS.Cell(selectedRowIndex, endImageCol + 4).Tag);
            //选中节点的父节点【叶节点】ID
            string parentID = flexGridWBS.Cell(selectedRowIndex, endImageCol + 5).Tag;
            int parentLevel = 0;
            int.TryParse(flexGridWBS.Cell(selectedRowIndex, endImageCol + 7).Tag, out parentLevel);
            int nameColIndex = parentLevel - lstTree[0].Level + 2;
            int newRowIndex = selectedRowIndex + detailCount + 1;
            //如果节点序号值小于明细数，则取明细数+1作为新插入行的序号
            string newOrderNo = maxNo < detailCount ? (detailCount + 1).ToString() : (maxNo + 1).ToString();
            flexGridWBS.AutoRedraw = false;
            flexGridWBS.InsertRow(newRowIndex, 1);
            flexGridWBS.Row(newRowIndex).Locked = false;
            flexGridWBS.Cell(selectedRowIndex, endImageCol + 3).Tag = (detailCount + 1).ToString();
            flexGridWBS.Cell(selectedRowIndex, endImageCol + 4).Tag = newOrderNo;
            flexGridWBS.Cell(newRowIndex, endImageCol + 5).Tag = "";
            flexGridWBS.Cell(newRowIndex, endImageCol + 6).Tag = parentID;//存放父节点ID
            flexGridWBS.Cell(newRowIndex, endImageCol + 7).Tag = (parentLevel + 1).ToString();//存放当前节点层次级别
            flexGridWBS.Cell(newRowIndex, endImageCol + 8).Tag = nameColIndex.ToString();//存放当前节点中名称所在列的index     
            flexGridWBS.Cell(newRowIndex, endImageCol + 1).Tag = "0";//叶节点
            flexGridWBS.Cell(newRowIndex, endImageCol + 2).Tag = "1";//状态，新建
            flexGridWBS.Cell(newRowIndex, endImageCol + 4).Tag = newOrderNo;
            flexGridWBS.Cell(newRowIndex, endImageCol + 10).Tag = selectedRowIndex.ToString();//如果节点序号值小于明细数，则取明细数+1作为新插入行的序号

            FlexCell.Range range = flexGridWBS.Range(newRowIndex, nameColIndex, newRowIndex, endImageCol);
            range.Merge();

            //var colIndex = endImageCol + 3;
            //flexGridWBS.Cell(newRowIndex, colIndex).Mask = MaskEnum.Numeric;
            //flexGridWBS.Cell(newRowIndex, ++colIndex).Mask = MaskEnum.Numeric;
            //flexGridWBS.Cell(newRowIndex, ++colIndex).Mask = MaskEnum.Numeric;
            //flexGridWBS.Cell(newRowIndex, ++colIndex).Mask = MaskEnum.Numeric;
            //flexGridWBS.Cell(newRowIndex, ++colIndex).Mask = MaskEnum.Numeric;
            //flexGridWBS.Cell(newRowIndex, ++colIndex).Mask = MaskEnum.Numeric;
            //flexGridWBS.Cell(newRowIndex, ++colIndex).Mask = MaskEnum.Numeric;
            //flexGridWBS.Cell(newRowIndex, ++colIndex).Mask = MaskEnum.Numeric;
            //flexGridWBS.Cell(newRowIndex, ++colIndex).Mask = MaskEnum.Numeric;

            for (int i = 0; i < lstRowIndex.Count; i++)
            {
                if (lstRowIndex[i] >= newRowIndex)
                {
                    lstRowIndex[i] = lstRowIndex[i] + 1;
                }
            }
            lstRowIndex.Add(newRowIndex);

            //如果拷贝的是常用定额编号Grid中的数据时，则无需处理对应行，列索引号
            if (copyedSelection != null && IsCopyedQuoteCode == false)
            {
                if (newRowIndex <= copyedSelection[0])
                {
                    copyedSelection[0] += 1;
                }
                if (newRowIndex <= copyedSelection[2])
                {
                    copyedSelection[2] += 1;
                }
            }
            //父节点index调整
            for (int i = newRowIndex + 1; i < flexGridWBS.Rows; i++)
            {
                int parentNodeIndex = ClientUtil.ToInt(flexGridWBS.Cell(i, endImageCol + 10).Tag);
                if (parentNodeIndex >= newRowIndex)
                {
                    flexGridWBS.Cell(i, endImageCol + 10).Tag = (parentNodeIndex + 1).ToString();
                }
            }

            GridRefresh();
            return newRowIndex;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Validdata())
                {
                    return;
                }
                FlashScreen.Show("正在保存数据,请稍候......");
                bool isSuccess = SaveDataToDrafts(Enum_ImportType.新建);
                FlashScreen.Close();
                if (isSuccess)
                {
                    FlashScreen.Close();
                    MessageBox.Show("数据保存成功");
                }
                else
                {
                    FlashScreen.Close();
                    MessageBox.Show("数据保存失败");
                }
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("数据保存失败");
                throw ex;
            }
            finally
            {
                FlashScreen.Close();
                //this.flexGridWBS.Focus();
            }
        }

        /// <summary>
        /// 将要导入的数据保存到草稿箱表
        /// </summary>
        /// <returns></returns>
        private bool SaveDataToDrafts(Enum_ImportType importType)
        {
            try
            {
                IList lst = GetNewRowData(importType);

                if (lst.Count > 0)
                {
                    lstDBDetailImportNew = model.SaveOrUpdate(lst).OfType<GWBSDetailImport>().ToList();
                    //lstTree数据处理，将GWBSDetailImport数据合并到lstTree中
                    if (lstTree != null && lstTree.Count > 0)
                    {
                        foreach (GWBSDetailImport item in lstDBDetailImportNew)
                        {
                            for (int i = 0; i < lstTree.Count; i++)
                            {
                                if (lstTree[i].Id == item.ParentId)//当前节点
                                {
                                    if (lstTree[i].Details != null && lstTree[i].Details.Count >= 0)
                                    {
                                        //如果存在则更新，否则直接添加
                                        if (lstTree[i].Details.Any(p => p.ID == item.Id))
                                        {
                                            for (int j = 0; j < lstTree[i].Details.Count; j++)
                                            {
                                                if (lstTree[i].Details[j].ID == item.Id)//节点下的明细。编辑
                                                {
                                                    SetValueToObj(lstTree[i].Details[j], lstTree[i], item);
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var obj = new GWBSDetailTemp();
                                            SetValueToObj(obj, lstTree[i], item);
                                            lstTree[i].Details.Add(obj);
                                        }
                                    }
                                    else
                                    {
                                        var obj = new GWBSDetailTemp();
                                        SetValueToObj(obj, lstTree[i], item);
                                        lstTree[i].Details = new List<GWBSDetailTemp>() { obj };

                                    }
                                }
                            }
                        }

                        //lstRowIndex.Clear();
                        //lstDBTreeDetailNew.Clear();
                        //lstDBDetailImportNew.Clear();
                        //FillFlex();

                        UpdateGridWithOutReload();
                    }
                    return true;
                }
                else
                {
                    MessageBox.Show("暂无记录可保存，请至少新增一条记录。");
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 无刷新更新表格数据
        /// </summary>
        private void UpdateGridWithOutReload()
        {
            var rowIndex = 0;
            for (int i = 0; i < lstRowIndex.Count; i++)
            {
                rowIndex = lstRowIndex[i];
                var parentID = ClientUtil.ToString(flexGridWBS.Cell(rowIndex, endImageCol + 6).Tag.Trim());
                var orderNo = ClientUtil.ToInt(flexGridWBS.Cell(rowIndex, endImageCol + 4).Tag.Trim());
                //父节点ID一定存在，即可在同一父节点下根据序号定位相应的对象与行数据
                var parentNode = lstTree.FirstOrDefault(p => p.Id == parentID);
                var detail = parentNode.Details.FirstOrDefault(p => p.OrderNo == orderNo);
                flexGridWBS.Cell(rowIndex, endImageCol + 5).Tag = detail == null ? "" : detail.ID;
            }
        }

        private void SetValueToObj(GWBSDetailTemp obj, GWBSTreeTemp theGWBS, GWBSDetailImport item)
        {
            obj.ID = item.Id;
            obj.Name = item.Name;
            obj.Code = item.Code;
            obj.ContractArtificialPrice = item.ContractArtificialPrice;
            obj.ContractMaterialPrice = item.ContractMaterialPrice;
            obj.ContractQuantity = item.ContractQuantity;
            obj.ResponsibilityArtificialPrice = item.ResponsibilityArtificialPrice;
            obj.ResponsibilityMaterialPrice = item.ResponsibilityMaterialPrice;
            obj.ResponsibilityQuantity = item.ResponsibilityQuantity;
            obj.PlanArtificialPrice = item.PlanArtificialPrice;
            obj.PlanMaterialPrice = item.PlanMaterialPrice;
            obj.PlanQuantity = item.PlanQuantity;
            obj.DiagramNumber = item.DiagramNumber;
            obj.OrderNo = item.OrderNo;
            obj.State = item.State.GetHashCode().ToString();
            obj.TheGWBS = theGWBS;
        }

        private IList GetNewRowData(Enum_ImportType importType)
        {
            IList lst = new ArrayList();
            //排除表头【2行】，数据内容从第三行开始
            foreach (var i in lstRowIndex)
            {
                GWBSDetailImport oprDtl = new GWBSDetailImport();
                oprDtl.ProjectID = projectInfo.Id;
                oprDtl.Id = string.IsNullOrEmpty(flexGridWBS.Cell(i, endImageCol + 5).Tag) ? null : flexGridWBS.Cell(i, endImageCol + 5).Tag;
                oprDtl.OrderNo = ClientUtil.ToInt(flexGridWBS.Cell(i, endImageCol + 4).Tag);

                oprDtl.ParentId = flexGridWBS.Cell(i, endImageCol + 6).Tag;
                oprDtl.Name = flexGridWBS.Cell(i, ClientUtil.ToInt(flexGridWBS.Cell(i, endImageCol + 8).Tag)).Text.Trim();
                oprDtl.State = importType;

                int indexCol = endImageCol + 1;
                oprDtl.Code = flexGridWBS.Cell(i, indexCol).Text.Trim();
                indexCol++;
                oprDtl.DiagramNumber = flexGridWBS.Cell(i, indexCol).Text.Trim();
                //合同收入
                indexCol++;
                oprDtl.ContractQuantity = ToDecimalOrNull(flexGridWBS.Cell(i, indexCol).Text.Trim());
                indexCol++;
                oprDtl.ContractArtificialPrice = ToDecimalOrNull(flexGridWBS.Cell(i, indexCol).Text.Trim());
                indexCol++;
                oprDtl.ContractMaterialPrice = ToDecimalOrNull(flexGridWBS.Cell(i, indexCol).Text.Trim());

                //责任成本
                indexCol++;
                oprDtl.ResponsibilityQuantity = ToDecimalOrNull(flexGridWBS.Cell(i, indexCol).Text.Trim());
                indexCol++;
                oprDtl.ResponsibilityArtificialPrice = ToDecimalOrNull(flexGridWBS.Cell(i, indexCol).Text.Trim());
                indexCol++;
                oprDtl.ResponsibilityMaterialPrice = ToDecimalOrNull(flexGridWBS.Cell(i, indexCol).Text.Trim());

                //计划成本
                indexCol++;
                oprDtl.PlanQuantity = ToDecimalOrNull(flexGridWBS.Cell(i, indexCol).Text.Trim());
                indexCol++;
                oprDtl.PlanArtificialPrice = ToDecimalOrNull(flexGridWBS.Cell(i, indexCol).Text.Trim());
                indexCol++;
                oprDtl.PlanMaterialPrice = ToDecimalOrNull(flexGridWBS.Cell(i, indexCol).Text.Trim());

                lst.Add(oprDtl);
            }
            return lst;
        }

        private bool SubmitData()
        {
            string errMsg = "";
            try
            {
                #region 验证  成本明细列表   成本项数据获取
                //成本项分类集合
                List<CostItemCategory> listCategory = new List<CostItemCategory>();
                //成本项集合
                List<CostItem> listCostItem = new List<CostItem>();
                //成本项编号
                List<string> quotaCodeList = new List<string>();

                ObjectQuery oqCate = new ObjectQuery();
                ObjectQuery oqCostItem = new ObjectQuery();
                Disjunction disCate = new Disjunction();
                Disjunction disCostItem = new Disjunction();
                int errRowIndex = 0;
                int curColIndex = 0;

                if (cbResponseAccountFlag.Checked == false && cbCostAccountFlag.Checked == false && cbProductConfirm.Checked == false)
                {
                    MessageBox.Show("请选择明细类型！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (cbCostAccountFlag.Checked == false && cbProductConfirm.Checked)
                {
                    MessageBox.Show("不能单独添加“生产确认明细”，请使用分摊方式从“成本核算明细”上分摊工程量，或将“成本核算明细”和“生产确认明细”做到一个明细上！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                //查询并校验基础数据
                foreach (var i in lstRowIndex)
                {
                    errRowIndex = i - titleRows + 1;
                    string quotaCode = flexGridWBS.Cell(i, endImageCol + 1).Text;
                    string name = flexGridWBS.Cell(i, ClientUtil.ToInt(flexGridWBS.Cell(i, endImageCol + 8).Tag)).Text;
                    if (string.IsNullOrEmpty(name))
                    {
                        errMsg += "第" + errRowIndex + "行明细名称为空" + Environment.NewLine;
                    }

                    if (string.IsNullOrEmpty(quotaCode))
                    {
                        errMsg += "第" + errRowIndex + "行定额编号为空" + Environment.NewLine;
                    }
                    else
                    {
                        quotaCode = quotaCode.Trim().ToUpper();
                        disCostItem.Add(Expression.Eq("QuotaCode", quotaCode));
                        quotaCodeList.Add(quotaCode);
                    }
                }

                if (errMsg == "")
                {
                    ObjectQuery oq1 = new ObjectQuery();
                    Disjunction dis1 = new Disjunction();
                    //层次码
                    string[] sysCodes = OptGWBSTreeObj.SysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < sysCodes.Length; i++)
                    {
                        string sysCode = "";
                        for (int j = 0; j <= i; j++)
                        {
                            sysCode += sysCodes[j] + ".";
                        }
                        dis1.Add(Expression.Eq("GwbsSyscode", sysCode));
                    }
                    oq1.AddCriterion(dis1);
                    oq1.AddCriterion(Expression.Eq("ProjectId", OptGWBSTreeObj.TheProjectGUID));
                    zoningList = model.ObjectQuery(typeof(CostItemsZoning), oq1);

                    oq1.Criterions.Clear();
                    dis1.Criteria.Clear();
                    if (zoningList.Count == 0)
                    {
                        if (quotaCodeList != null && quotaCodeList.Count > 0)
                        {
                            string sql = "";
                            for (int z = 0; z < quotaCodeList.Count - 1; z++)
                            {
                                sql += "'" + quotaCodeList[z] + "',";
                            }
                            sql += "'" + quotaCodeList[quotaCodeList.Count - 1] + "'";
                            if (model.CheckQutaCodeIsRepeat(sql))
                            {
                                errMsg += "这些定额编号里有些在成本项里有重复,请先选择一个区域";
                            }
                        }
                    }
                }
                else
                {
                    return false;
                }

                if (disCate.Criteria.Count > 0)
                {
                    oqCate.AddCriterion(disCate);
                    listCategory = model.ObjectQuery(typeof(CostItemCategory), oqCate).OfType<CostItemCategory>().ToList();
                }
                if (disCostItem.Criteria.Count > 0)
                {
                    oqCostItem.AddCriterion(disCostItem);
                    if (zoningList != null && zoningList.Count > 0)
                    {
                        CostItemsZoning z = zoningList[0] as CostItemsZoning;
                        oqCostItem.AddCriterion(Expression.Like("TheCostItemCateSyscode", z.CostItemsCateSyscode, MatchMode.Start));
                    }
                    oqCostItem.AddCriterion(Expression.Eq("ItemState", CostItemState.发布));
                    listCostItem = model.ObjectQuery(typeof(CostItem), oqCostItem).OfType<CostItem>().ToList();
                }
                var listQuotaByItemIDs = GetListQuotaByCostItemID(listCostItem).OfType<SubjectCostQuota>().ToList();
                #endregion

                DateTime serverTime = model.GetServerTime();
                IList listSaveDtl = new ArrayList();
                //变更涉及的tree节点
                List<GWBSTree> lstGWBSNodeChanged = new List<GWBSTree>();
                #region 循环遍历表格处理数据
                //排除表头【2行】，数据内容从第三行开始
                foreach (var i in lstRowIndex)
                {
                    errRowIndex = i - titleRows + 1;
                    //当前节点的父节点ID
                    string curNodeParentId = flexGridWBS.Cell(i, endImageCol + 6).Tag;
                    //当前节点的父节点
                    var curNodeParent = lstGWBSNodeChanged.FirstOrDefault(p => p.Id == curNodeParentId);
                    //如果该节点未添加到lstGWBSNodeChanged，则根据ID在lstMGWBSTree【所有节点结合】中取得相应的节点，同时添加进lstGWBSNodeChanged
                    if (curNodeParent == null)
                    {
                        curNodeParent = lstMGWBSTree.FirstOrDefault(p => p.Id == curNodeParentId);
                        lstGWBSNodeChanged.Add(curNodeParent);
                    }

                    curColIndex = endImageCol + 1;
                    GWBSDetail oprDtl = new GWBSDetail();
                    oprDtl.Name = flexGridWBS.Cell(i, ClientUtil.ToInt(flexGridWBS.Cell(i, endImageCol + 8).Tag)).Text.Trim();
                    oprDtl.Code = flexGridWBS.Cell(i, curColIndex).Text.Trim();
                    oprDtl.OrderNo = ClientUtil.ToInt(flexGridWBS.Cell(i, endImageCol + 4).Tag);

                    curColIndex++;
                    oprDtl.DiagramNumber = flexGridWBS.Cell(i, curColIndex).Text.Trim();
                    //合同收入
                    curColIndex++;
                    oprDtl.ContractProjectQuantity = ClientUtil.ToDecimal(flexGridWBS.Cell(i, curColIndex).Text.Trim());

                    //责任成本
                    curColIndex += 3;
                    if (cbResponseAccountFlag.Checked)
                    {
                        oprDtl.ResponseFlag = 1;
                        oprDtl.ResponsibilitilyWorkAmount = ClientUtil.ToDecimal(flexGridWBS.Cell(i, curColIndex).Text.Trim());
                    }

                    //计划成本   
                    curColIndex += 3;
                    if (cbCostAccountFlag.Checked)
                    {
                        oprDtl.CostingFlag = 1;
                        oprDtl.PlanWorkAmount = ClientUtil.ToDecimal(flexGridWBS.Cell(i, curColIndex).Text.Trim());
                    }
                    oprDtl.ProduceConfirmFlag = cbProductConfirm.Checked ? 1 : 0;
                    oprDtl.TheGWBS = curNodeParent;
                    oprDtl.TheGWBSSysCode = curNodeParent.SysCode;

                    if (curNodeParent.ProjectTaskTypeGUID != null)
                    {
                        oprDtl.ProjectTaskTypeCode = curNodeParent.ProjectTaskTypeGUID.Code;
                    }
                    oprDtl.TheProjectGUID = curNodeParent.TheProjectGUID;
                    oprDtl.TheProjectName = curNodeParent.TheProjectName;

                    oprDtl.ContractGroupCode = contract.Code;
                    oprDtl.ContractGroupGUID = contract.Id;
                    oprDtl.ContractGroupName = contract.ContractName;
                    oprDtl.ContractGroupType = contract.ContractGroupType;
                    int maxOrderNo = 1;
                    oprDtl.Code = curNodeParent.Code + "-" + GetChildNodeMaxCode(curNodeParent, out maxOrderNo).PadLeft(5, '0');
                    oprDtl.OrderNo = maxOrderNo + 1;
                    oprDtl.State = VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit;
                    oprDtl.CurrentStateTime = serverTime;

                    #region 获取成本项
                    string quotaCode = flexGridWBS.Cell(i, endImageCol + 1).Text;
                    quotaCode = string.IsNullOrEmpty(quotaCode) ? "" : quotaCode.ToUpper();

                    var queryQuota = from c in listCostItem
                                     where c.QuotaCode == quotaCode
                                     select c;

                    CostItem currItem = null;

                    if (queryQuota.Count() == 0)
                    {
                        errMsg += "第" + errRowIndex + "行根据定额编号“" + quotaCode + "”未找到相关成本项" + Environment.NewLine;
                        continue;
                    }
                    else
                    {
                        currItem = queryQuota.ElementAt(0);
                    }
                    if (errMsg != "")
                    {
                        return false;
                    }
                    #endregion

                    oprDtl.TheCostItem = currItem;
                    oprDtl.TheCostItemCateSyscode = currItem.TheCostItemCateSyscode;
                    #region 通过成本项获取资源耗用
                    CostItem item = oprDtl.TheCostItem;
                    if (item != null)
                    {
                        var listQuota = listQuotaByItemIDs.Where(p => p.TheCostItem.Id == item.Id).ToList();
                        /**
                        * 注：主资源标志【是：材料，否：人工】
                        * 1，用户填写的资源数和关联到的资源数一致，且都为两条时，【其中材料单价】写入主资源，【其中人工单价】写入另外一个资源
                        * 2，【其中材料单价】【其中人工单价】用户只填写了一项且关联到的资源数也为一项时，直接将相应的数据更新到资源上
                        * 3，关联到有两条以上或是多个是/否则不符合要求不导入等其他情况，提示报错误信息给用户，不予以导入
                        * **/
                        bool isOnly = false;//是否只有一项
                        errMsg += ValiddataResource(listQuota, i, out isOnly);
                        if (!string.IsNullOrEmpty(errMsg))
                        {
                            continue;
                        }

                        #region 加载资源耗用明细
                        ResourceGroup mainResource = null;

                        foreach (SubjectCostQuota quota in listQuota)
                        {
                            if (quota.MainResourceFlag && quota.ListResources.Count > 0)
                            {
                                var queryMainRes = from r in quota.ListResources
                                                   where r.ResourceTypeGUID == quota.ResourceTypeGUID
                                                   select r;

                                if (queryMainRes.Count() > 0)
                                {
                                    mainResource = queryMainRes.ElementAt(0);
                                }
                                else
                                {
                                    mainResource = quota.ListResources.ElementAt(0);
                                }
                            }
                            var query = from q in oprDtl.ListCostSubjectDetails
                                        where q.ResourceUsageQuota.Id == quota.Id
                                        select q;

                            if (query.Count() > 0)///??不知道什么意思，待确认
                            {
                                continue;
                            }

                            AddResourceUsageInTaskDetail(oprDtl, quota, i, isOnly);
                        }

                        if (mainResource != null)
                        {
                            oprDtl.MainResourceTypeId = mainResource.ResourceTypeGUID;
                            oprDtl.MainResourceTypeName = mainResource.ResourceTypeName;
                            oprDtl.MainResourceTypeQuality = mainResource.ResourceTypeQuality;
                            oprDtl.MainResourceTypeSpec = mainResource.ResourceTypeSpec;
                        }

                        oprDtl.WorkAmountUnitGUID = item.ProjectUnitGUID;
                        oprDtl.WorkAmountUnitName = item.ProjectUnitName;
                        oprDtl.PriceUnitGUID = item.PriceUnitGUID;
                        oprDtl.PriceUnitName = item.PriceUnitName;

                        #endregion
                    }
                    #endregion

                    listSaveDtl.Add(oprDtl);
                }
                #endregion

                if (!string.IsNullOrEmpty(errMsg))
                {
                    return false;
                }
                #region 循环处理节点数据，计算任务明细的量价，最后保存入库
                IList listGWBS = new ArrayList();
                foreach (var treeNodeChanged in lstGWBSNodeChanged)
                {
                    //根据明细设置任务对象的标记
                    bool taskResponsibleFlag = false;
                    bool taskCostAccFlag = false;

                    #region 计算任务明细的量价
                    ///获取当前节点的详细数据集合
                    var curNodeDetailList = listSaveDtl.OfType<GWBSDetail>().ToList().Where(p => p.TheGWBS.Id == treeNodeChanged.Id).ToList();
                    foreach (GWBSDetail oprDtl in curNodeDetailList)
                    {
                        decimal dtlUsageProjectAmountPriceByContract = 0;
                        decimal dtlUsageProjectAmountPriceByResponsible = 0;
                        decimal dtlUsageProjectAmountPriceByPlan = 0;
                        foreach (GWBSDetailCostSubject subject in oprDtl.ListCostSubjectDetails)
                        {
                            dtlUsageProjectAmountPriceByContract += subject.ContractPrice;
                            dtlUsageProjectAmountPriceByResponsible += subject.ResponsibleWorkPrice;
                            dtlUsageProjectAmountPriceByPlan += subject.PlanWorkPrice;
                        }

                        oprDtl.ContractPrice = dtlUsageProjectAmountPriceByContract;
                        oprDtl.ResponsibilitilyPrice = dtlUsageProjectAmountPriceByResponsible;
                        oprDtl.PlanPrice = dtlUsageProjectAmountPriceByPlan;

                        oprDtl.ContractTotalPrice = oprDtl.ContractProjectQuantity * oprDtl.ContractPrice;
                        oprDtl.ResponsibilitilyTotalPrice = oprDtl.ResponsibilitilyWorkAmount * oprDtl.ResponsibilitilyPrice;
                        oprDtl.PlanTotalPrice = oprDtl.PlanWorkAmount * oprDtl.PlanPrice;

                        ///??不知道什么意思，待确认 
                        if (oprDtl.ResponseFlag == 1)
                            taskResponsibleFlag = true;
                        if (oprDtl.CostingFlag == 1)
                            taskCostAccFlag = true;
                    }
                    #endregion

                    //计算项目任务的合价
                    GWBSTree tempNode = new GWBSTree();
                    tempNode.Id = treeNodeChanged.Id;
                    tempNode.SysCode = treeNodeChanged.SysCode;
                    tempNode = model.AccountTotalPrice(tempNode);

                    List<Decimal> listTotalPrice = StaticMethod.GetTaskDtlTotalPrice(curNodeDetailList);

                    treeNodeChanged.ContractTotalPrice = tempNode.ContractTotalPrice + listTotalPrice[0];
                    treeNodeChanged.ResponsibilityTotalPrice = tempNode.ResponsibilityTotalPrice + listTotalPrice[1];
                    treeNodeChanged.PlanTotalPrice = tempNode.PlanTotalPrice + listTotalPrice[2];

                    treeNodeChanged.ResponsibleAccFlag = taskResponsibleFlag;
                    treeNodeChanged.CostAccFlag = taskCostAccFlag;

                    listGWBS.Add(treeNodeChanged);
                }
                //数据的添加，父节点会随之变更，故只需判断父节点数即可
                if (listGWBS.Count > 0)
                {
                    ///保存，入库
                    var resultlst = model.SaveOrUpdateDetail(listGWBS, listSaveDtl, false);
                    //if (resultlst != null && resultlst.Count > 0)
                    //{
                    //    //lstDBTreeNew = resultlst[0];
                    //    //lstDBTreeDetailNew = (resultlst[1] as IList).OfType<GWBSDetail>().ToList();
                    //}
                    return true;
                }
                else
                {
                    MessageBox.Show("暂无记录可导入，请至少新增一条记录。");
                    return false;
                }
                #endregion
            }
            catch (Exception ex)
            {
                errMsg += ExceptionUtil.ExceptionMessage(ex);
                return false;
            }
            finally
            {
                if (errMsg != "")
                {
                    WriteLog(errMsg);
                }
            }
        }

        private string GetChildNodeMaxCode(GWBSTree parentNode, out int maxOrderNo)
        {
            maxOrderNo = 1;

            int code = parentNode.Details.Count + 1;

            foreach (GWBSDetail dtl in parentNode.Details)
            {
                if (dtl.OrderNo >= maxOrderNo)
                {
                    maxOrderNo = dtl.OrderNo;
                }

                if (!string.IsNullOrEmpty(dtl.Code) && dtl.Code.IndexOf("-") > -1)
                {
                    int tempCode = 0;
                    if (Int32.TryParse(dtl.Code.Substring(dtl.Code.LastIndexOf("-") + 1), out tempCode))
                    {
                        if (tempCode >= code)
                        {
                            code = tempCode + 1;
                        }
                    }
                }
            }
            return code.ToString();
        }

        /// <summary>
        /// 获取资源耗用
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private IList GetListQuotaByCostItemID(List<CostItem> listCostItem)
        {
            ObjectQuery oqQuota = new ObjectQuery();
            Disjunction or = new Disjunction();
            foreach (var item in listCostItem)
            {
                or.Add(Expression.Eq("TheCostItem.Id", item.Id));
            }
            oqQuota.AddCriterion(or);
            oqQuota.AddFetchMode("CostAccountSubjectGUID", NHibernate.FetchMode.Eager);
            oqQuota.AddFetchMode("ProjectAmountUnitGUID", NHibernate.FetchMode.Eager);
            oqQuota.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
            oqQuota.AddFetchMode("ListResources", NHibernate.FetchMode.Eager);

            IList listQuota = model.ObjectQuery(typeof(SubjectCostQuota), oqQuota);
            return listQuota;
        }

        void btnSubmit_Click(object sender, EventArgs e)
        {
            Timer t = new Timer();
            t.Start();
            Console.Write("开始时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            try
            {
                if (!ValidContract() || !Validdata())
                {
                    return;
                }
                FlashScreen.Show("正在保存数据,请稍候......");
                bool isSuccess = SubmitData();
                if (isSuccess)
                {
                    SaveDataToDrafts(Enum_ImportType.已提交);
                    //重置，以免多次操作时，上次数据对本次操作的影响
                    lstRowIndex.Clear();
                    //lstDBTreeDetailNew.Clear();
                    lstDBDetailImportNew.Clear();
                    contract = null;
                    //刷新列表，避免数据无ID导致重复添加
                    if (lstTree != null && lstTree.Count > 0)
                    {
                        FillFlex();
                    }
                    Console.Write("开始时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    t.Stop();
                    Console.Write("总共耗时：" + t.Interval.ToString());
                    FlashScreen.Close();
                    MessageBox.Show("数据导入成功");
                }
                else
                {
                    FlashScreen.Close();
                    MessageBox.Show("数据导入失败");
                }
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("数据导入失败");
                throw ex;
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        /// <summary>
        /// 选择契约组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSelectContractGroup_Click(object sender, EventArgs e)
        {
            VSelectWBSContractGroup frm = new VSelectWBSContractGroup(new MWBSContractGroup(), false);
            if (txtContractGroupName.Tag != null)
            {
                frm.DefaultSelectedContract = txtContractGroupName.Tag as ContractGroup;
            }
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                contract = frm.SelectResult[0] as ContractGroup;
                txtContractGroupName.Text = contract.ContractName;
                //txtContractGroupType.Text = contract.ContractGroupType;
                //txtContractGroupDesc.Text = contract.ContractDesc;
                txtContractGroupName.Tag = contract;
            }
        }

        /// <summary>
        /// 选择任务节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnSelectGWBSTreeWBS_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsSelectSingleNode = true;
            frm.ShowDialog();
            List<TreeNode> list = frm.SelectResult;
            if (list.Count > 0)
            {
                OptGWBSTreeObj = (list[0] as TreeNode).Tag as GWBSTree;
                txtGWBSTreeWBS.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), OptGWBSTreeObj.Name, OptGWBSTreeObj.SysCode);
                txtGWBSTreeWBS.Tag = OptGWBSTreeObj;
                //txtGWBSTreeWBS.Focus();                
                lstTree.Clear();
                //lstDBTreeDetailNew.Clear();
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnQuery_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("查询中......");
            DataTable oTable = null;
            lstRowIndex = new List<int>();
            List<GWBSDetailImport> lstImport = new List<GWBSDetailImport>();
            if (txtGWBSTreeWBS.Tag != null)
            {
                //获取工程WBS树
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
                oq.AddCriterion(Expression.Like("SysCode", OptGWBSTreeObj.SysCode, MatchMode.Start));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("ProjectTaskTypeGUID", NHibernate.FetchMode.Eager);
                lstMGWBSTree = model.ObjectQuery(typeof(GWBSTree), oq).OfType<GWBSTree>().ToList();

                GWBSTree oGWBSTree = txtGWBSTreeWBS.Tag as GWBSTree;
                oTable = model.SelectGWBSResourceCost(oGWBSTree, projectInfo);

                ObjectQuery oqeury = new ObjectQuery();
                oqeury.AddCriterion(Expression.Eq("ProjectID", projectInfo.Id));
                oqeury.AddCriterion(Expression.Eq("State", Enum_ImportType.新建)); //由于目前不用标识已提交数据，估值查询“新建”状态数据
                lstImportAll = model.ObjectQuery(typeof(GWBSDetailImport), oqeury).OfType<GWBSDetailImport>().ToList();
                //只显示未提交的数据
                lstImport = lstImportAll;// lstImportAll.Where(p => p.State == Enum_ImportType.新建).ToList();
                //string strcodes = string.Join("','", oTable.Select().Where(p => p["QUOTACODE"] != null && !string.IsNullOrEmpty(p["QUOTACODE"].ToString())).Select(p => p["QUOTACODE"].ToString()).Distinct().ToArray());
                //strcodes += "','" + string.Join("','", lstImport.Select(p => p.Code).Distinct().ToArray());
                //List<string> lstQuoteCode = oTable.Select().Where(p => p["QUOTACODE"] != null && !string.IsNullOrEmpty(p["QUOTACODE"].ToString())).Select(p => p["QUOTACODE"].ToString()).Distinct().ToList();
                //lstQuoteCode.AddRange(lstImport.Select(p => p.Code).Distinct());
                //string strcodes = "'" + string.Join("','", lstQuoteCode.Distinct().ToArray()) + "'";
                var dtTemp = model.GetSubjectCostQuotas(projectInfo.Id, oGWBSTree.SysCode);
                //lstRelatedSubjectCostQuotaTemp = ConvertToObject<SubjectCostQuotaTemp>(dtTemp);
                if (lstRelatedSubjectCostQuotaTemp == null)
                {
                    lstRelatedSubjectCostQuotaTemp = new List<SubjectCostQuotaTemp>();
                }
                else
                {
                    lstRelatedSubjectCostQuotaTemp.Clear();
                }
                //当前表格的中定额编号对应的资源耗用
                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    var obj = new SubjectCostQuotaTemp()
                    {
                        QuotaCode = ClientUtil.ToString(dtTemp.Rows[i]["QuotaCode"]),
                        Id = ClientUtil.ToString(dtTemp.Rows[i]["Id"]),
                        ItemID = ClientUtil.ToString(dtTemp.Rows[i]["ItemID"]),
                        MainResourceFlag = ClientUtil.ToBool(dtTemp.Rows[i]["MainResourceFlag"])
                    };
                    if (lstRelatedSubjectCostQuotaTemp.Any(p => p.Id == obj.Id) == false)
                    {
                        lstRelatedSubjectCostQuotaTemp.Add(obj);
                    }
                }

                ///常用项Grid中的定额编号对应的资源耗用
                ObjectQuery oqQuota = new ObjectQuery();
                oqQuota.AddCriterion(Expression.In("TheCostItem.Id", LstQuoteGridCostItem.Select(p => p.Id).Distinct().ToList()));
                var lstSubjectCostQuota = model.ObjectQuery(typeof(SubjectCostQuota), oqQuota).OfType<SubjectCostQuota>().ToList();
                foreach (var item in lstSubjectCostQuota)
                {
                    var costitem = LstQuoteGridCostItem.FirstOrDefault(p => p.Id == item.TheCostItem.Id);
                    var obj = new SubjectCostQuotaTemp()
                    {
                        QuotaCode = costitem.QuotaCode,
                        Id = item.Id,
                        ItemID = item.TheCostItem.Id,
                        MainResourceFlag = item.MainResourceFlag
                    };
                    if (lstRelatedSubjectCostQuotaTemp.Any(p => p.Id == obj.Id) == false)
                    {
                        lstRelatedSubjectCostQuotaTemp.Add(obj);
                    }
                }
            }
            else
            {
                MessageBox.Show("请选择一个任务节点");
            }

            if (lstTree != null && lstTree.Count > 0)
            {
                FillFlex();
            }
            else if ((oTable != null && oTable.Rows.Count > 0) || (lstImport != null && lstImport.Count > 0))
            {
                getTree(oTable, lstImport);
                FillFlex();
            }
            else
            {
                InitFlexGrid(0, true);
            }
            FlashScreen.Close();
        }


        public bool IsEmpty(int iRow)
        {
            bool bFlag = true;
            if (flexGridWBS.Cols > 1)
            {
                for (int i = 1; i < flexGridWBS.Cols; i++)
                {
                    if (!string.IsNullOrEmpty(flexGridWBS.Cell(iRow, i).Text.Trim()))
                    {
                        bFlag = false;
                        break;
                    }
                }
            }

            return bFlag;
        }

        private void AddResourceUsageInTaskDetail(GWBSDetail oprDtl, SubjectCostQuota quota, int rowIndex, bool isOnly)
        {
            int indexCol = 0;
            GWBSDetailCostSubject subject = new GWBSDetailCostSubject();
            subject.Name = quota.Name;
            subject.MainResTypeFlag = quota.MainResourceFlag;

            subject.ProjectAmountUnitGUID = quota.ProjectAmountUnitGUID;
            subject.ProjectAmountUnitName = quota.ProjectAmountUnitName;

            subject.PriceUnitGUID = quota.PriceUnitGUID;
            subject.PriceUnitName = quota.PriceUnitName;
            string val = string.Empty;
            //如果是表格里面只填写了一项，同时关联到的资源也只有一项，直接赋值【无需区分人工，材料】
            if (isOnly)
            {
                indexCol = endImageCol + 4;
                val = GetNonNullCellValue(rowIndex, indexCol);
                if (!string.IsNullOrEmpty(val))
                {
                    subject.ContractQuantityPrice = ClientUtil.ToDecimal(val);
                }
                indexCol = indexCol + 3;
                val = GetNonNullCellValue(rowIndex, indexCol);
                if (!string.IsNullOrEmpty(val))
                {
                    subject.ResponsibilitilyPrice = ClientUtil.ToDecimal(val);
                }
                indexCol = indexCol + 3;
                val = GetNonNullCellValue(rowIndex, indexCol);
                if (!string.IsNullOrEmpty(val))
                {
                    subject.PlanPrice = ClientUtil.ToDecimal(val);
                }
            }
            else if (quota.MainResourceFlag)//如果是主资源，即材料
            {
                indexCol = endImageCol + 5;
                val = GetCellValue(rowIndex, indexCol);
                if (!string.IsNullOrEmpty(val))
                {
                    subject.ContractQuantityPrice = ClientUtil.ToDecimal(flexGridWBS.Cell(rowIndex, indexCol).Text.Trim());
                }
                indexCol = indexCol + 3;
                val = GetCellValue(rowIndex, indexCol);
                if (!string.IsNullOrEmpty(val))
                {
                    subject.ResponsibilitilyPrice = ClientUtil.ToDecimal(flexGridWBS.Cell(rowIndex, indexCol).Text.Trim());
                }
                indexCol = indexCol + 3;
                val = GetCellValue(rowIndex, indexCol);
                if (!string.IsNullOrEmpty(val))
                {
                    subject.PlanPrice = ClientUtil.ToDecimal(flexGridWBS.Cell(rowIndex, indexCol).Text.Trim());
                }
            }
            else//人工
            {
                indexCol = endImageCol + 4;
                val = GetCellValue(rowIndex, indexCol);
                if (!string.IsNullOrEmpty(val))
                {
                    subject.ContractQuantityPrice = ClientUtil.ToDecimal(flexGridWBS.Cell(rowIndex, indexCol).Text.Trim());
                }
                indexCol = indexCol + 3;
                val = GetCellValue(rowIndex, indexCol);
                if (!string.IsNullOrEmpty(val))
                {
                    subject.ResponsibilitilyPrice = ClientUtil.ToDecimal(flexGridWBS.Cell(rowIndex, indexCol).Text.Trim());
                }
                indexCol = indexCol + 3;
                val = GetCellValue(rowIndex, indexCol);
                if (!string.IsNullOrEmpty(val))
                {
                    subject.PlanPrice = ClientUtil.ToDecimal(flexGridWBS.Cell(rowIndex, indexCol).Text.Trim());
                }
            }

            //合同
            subject.ContractQuotaQuantity = quota.QuotaProjectAmount;
            //subject.ContractQuantityPrice = quota.QuotaPrice;
            subject.ContractPricePercent = 1;
            subject.ContractBasePrice = subject.ContractQuantityPrice;
            subject.ContractPrice = subject.ContractQuotaQuantity * subject.ContractQuantityPrice;
            subject.ContractProjectAmount = oprDtl.ContractProjectQuantity * subject.ContractQuotaQuantity;
            subject.ContractTotalPrice = subject.ContractProjectAmount * subject.ContractQuantityPrice;

            //责任
            subject.ResponsibleQuotaNum = quota.QuotaProjectAmount;
            //subject.ResponsibilitilyPrice = quota.QuotaPrice;
            subject.ResponsiblePricePercent = 1;
            subject.ResponsibleBasePrice = subject.ResponsibilitilyPrice;
            subject.ResponsibleWorkPrice = subject.ResponsibleQuotaNum * subject.ResponsibilitilyPrice;
            subject.ResponsibilitilyWorkAmount = oprDtl.ResponsibilitilyWorkAmount * subject.ResponsibleQuotaNum;
            subject.ResponsibilitilyTotalPrice = subject.ResponsibilitilyWorkAmount * subject.ResponsibilitilyPrice;//责任包干单价未知

            //计划
            subject.PlanQuotaNum = quota.QuotaProjectAmount;
            //subject.PlanPrice = quota.QuotaPrice;
            subject.PlanPricePercent = 1;
            subject.PlanBasePrice = subject.PlanPrice;
            subject.PlanWorkPrice = subject.PlanQuotaNum * subject.PlanPrice;
            subject.PlanWorkAmount = oprDtl.PlanWorkAmount * subject.PlanQuotaNum;
            subject.PlanTotalPrice = subject.PlanWorkAmount * subject.PlanPrice;//计划包干单价未知


            subject.AssessmentRate = quota.AssessmentRate;

            subject.ResourceUsageQuota = quota;

            if (quota.ListResources.Count > 0)
            {
                ResourceGroup itemResource = quota.ListResources.ElementAt(0);

                subject.ResourceTypeGUID = itemResource.ResourceTypeGUID;
                subject.ResourceTypeCode = itemResource.ResourceTypeCode;
                subject.ResourceTypeName = itemResource.ResourceTypeName;
                subject.ResourceTypeQuality = itemResource.ResourceTypeQuality;
                subject.ResourceTypeSpec = itemResource.ResourceTypeSpec;
                subject.ResourceCateSyscode = itemResource.ResourceCateSyscode;
            }

            subject.CostAccountSubjectGUID = quota.CostAccountSubjectGUID;
            subject.CostAccountSubjectName = quota.CostAccountSubjectName;
            if (quota.CostAccountSubjectGUID != null)
            {
                subject.CostAccountSubjectSyscode = quota.CostAccountSubjectGUID.SysCode;
            }
            subject.ProjectAmountWasta = quota.Wastage;

            subject.State = GWBSDetailCostSubjectState.编制;

            subject.TheProjectGUID = oprDtl.TheProjectGUID;
            subject.TheProjectName = oprDtl.TheProjectName;

            subject.TheGWBSDetail = oprDtl;

            subject.TheGWBSTree = oprDtl.TheGWBS;
            subject.TheGWBSTreeName = oprDtl.TheGWBS.Name;
            subject.TheGWBSTreeSyscode = oprDtl.TheGWBS.SysCode;

            oprDtl.ListCostSubjectDetails.Add(subject);
        }

        /// <summary>
        /// 获取相邻两个单元格中的非空值
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        private string GetNonNullCellValue(int rowIndex, int colIndex)
        {
            var colValue = flexGridWBS.Cell(rowIndex, colIndex).Text.Trim();
            if (string.IsNullOrEmpty(colValue))
            {
                colValue = flexGridWBS.Cell(rowIndex, colIndex + 1).Text.Trim();
            }
            return colValue;
        }

        private string GetCellValue(int rowIndex, int colIndex)
        {
            var colValue = flexGridWBS.Cell(rowIndex, colIndex).Text.Trim();
            return colValue;
        }

        /// <summary>
        /// 资源数一致性验证
        /// </summary>
        /// <returns></returns>
        private string ValiddataResource(List<SubjectCostQuota> listQuota, int rowIndex, out bool isOnly)
        {
            /**
            * 注：主资源标志【是：材料，否：人工】
            * 1，用户填写的资源数和关联到的资源数一致，且都为两条时，【其中材料单价】写入主资源，【其中人工单价】写入另外一个资源
            * 2，【其中材料单价】【其中人工单价】用户只填写了一项且关联到的资源数也为一项时，直接将相应的数据更新到资源上
            * 3，关联到有两条以上或是多个是/否则不符合要求不导入等其他情况，提示报错误信息给用户，不予以导入
            * **/
            //用户填写的非空耗用数量            
            string errMsg = string.Empty;
            isOnly = false;//是否只有一项
            int curColIndex = endImageCol + 4;
            int errRowIndex = rowIndex - titleRows + 1;
            var quotaCode = flexGridWBS.Cell(rowIndex, endImageCol + 1).Text.Trim();
            for (int i = 0; i < 3; i++)
            {
                int inputedCount = 0;
                if (!string.IsNullOrEmpty(flexGridWBS.Cell(rowIndex, curColIndex).Text))
                {
                    inputedCount++;
                }

                curColIndex = curColIndex + 1;
                if (!string.IsNullOrEmpty(flexGridWBS.Cell(rowIndex, curColIndex).Text))
                {
                    inputedCount++;
                }
                curColIndex += 2;//进入下一个循环后取相应人工单价的列索引

                if (inputedCount == 0)//如果人工，材料单价均没填写，则跳过
                {
                    continue;
                }
                if (listQuota.Count == 0)
                {
                    errMsg += "第" + errRowIndex + "行根据定额编号“" + quotaCode + "”未找到相关资源耗用" + Environment.NewLine;
                    break;
                }
                else if (listQuota.Count == 1)
                {
                    if (inputedCount == 1)
                    {
                        isOnly = true;
                    }
                    else
                    {
                        errMsg += "第" + errRowIndex + "行填写的数据与根据定额编号“" + quotaCode + "”找到" + listQuota.Count + "条资源耗用数量不一致" + Environment.NewLine;
                        break;
                    }
                }
                else if (listQuota.Count > 2)
                {
                    errMsg += "第" + errRowIndex + "行根据定额编号“" + quotaCode + "”找到" + listQuota.Count + "条相关资源耗用" + Environment.NewLine;
                    break;
                }
                else if (listQuota.Count == 2)
                {
                    if (listQuota.Count(P => P.MainResourceFlag) != 1)
                    {
                        //如果两条记录中不符合： 一条主资源，一条人工，则给出提示
                        errMsg += "第" + errRowIndex + "行根据定额编号“" + quotaCode + "”找到相关" + listQuota.Count + "条资源耗用需符合要求：1条主资源，1条人工" + Environment.NewLine;
                        break;
                    }

                    if (inputedCount != listQuota.Count)
                    {
                        errMsg += "第" + errRowIndex + "行填写的数据与根据定额编号“" + quotaCode + "”找到" + listQuota.Count + "条资源耗用数量不一致" + Environment.NewLine;
                        break;
                    }
                }
            }
            return errMsg;
        }
        /// <summary>
        /// 数据验证
        /// </summary>
        /// <returns></returns>
        private bool Validdata()
        {
            //新增的+ 数据库lst数据对比即可
            string errMsg = string.Empty;
            try
            {
                int errRowIndex = 0;
                string txtCell = "";
                bool isEmptyRow = false;
                Dictionary<string, List<RowData>> dicTree = new Dictionary<string, List<RowData>>();
                for (int index = lstRowIndex.Count - 1; index >= 0; index--)
                {
                    var item = lstRowIndex[index];
                    isEmptyRow = true;
                    for (int i = 1; i < flexGridWBS.Cols - 1; i++)
                    {
                        txtCell = ClientUtil.ToString(flexGridWBS.Cell(item, i).Text.Trim());
                        if (!string.IsNullOrEmpty(txtCell))
                        {
                            isEmptyRow = false;
                        }
                    }
                    //跳过空行,并将空行index移除掉，便于保存，提交数据
                    if (isEmptyRow)
                    {
                        lstRowIndex.Remove(item);
                        continue;
                    }

                    var id = string.IsNullOrEmpty(flexGridWBS.Cell(item, endImageCol + 5).Tag) ? null : flexGridWBS.Cell(item, endImageCol + 5).Tag;
                    var parentid = flexGridWBS.Cell(item, endImageCol + 6).Tag;
                    var name = flexGridWBS.Cell(item, ClientUtil.ToInt(flexGridWBS.Cell(item, endImageCol + 8).Tag)).Text.Trim();
                    var quoteCode = flexGridWBS.Cell(item, endImageCol + 1).Text.Trim();
                    var diagramNumber = flexGridWBS.Cell(item, endImageCol + 2).Text.Trim();
                    var curNodeDBData = lstTree.FirstOrDefault(p => p.Id == parentid);
                    errRowIndex = item - titleRows + 1;
                    if (string.IsNullOrEmpty(name))
                    {
                        errMsg += "第" + errRowIndex + "行明细名称不能为空" + Environment.NewLine;
                    }

                    if (string.IsNullOrEmpty(quoteCode))
                    {
                        errMsg += "第" + errRowIndex + "行定额编号不能为空" + Environment.NewLine;
                    }
                    //验证新增的数据中是否存在重复
                    if (!dicTree.Keys.Contains(parentid))
                    {
                        RowData rowdata = new RowData()
                        {
                            ID = id,
                            ParentID = parentid,
                            Index = item,
                            DiagramNumber = diagramNumber,
                            Name = name,
                            QuoteCode = quoteCode
                        };
                        var lstobj = new List<RowData>() { rowdata };
                        dicTree.Add(parentid, lstobj);
                    }
                    else
                    {
                        RowData rowdata = new RowData()
                        {
                            ID = id,
                            ParentID = parentid,
                            Index = item,
                            DiagramNumber = diagramNumber,
                            Name = name,
                            QuoteCode = quoteCode
                        };
                        dicTree[parentid].Add(rowdata);
                    }
                    if (curNodeDBData.Details != null && curNodeDBData.Details.Any(p => p.ID == id))
                    {
                        var detail = curNodeDBData.Details.FirstOrDefault(p => p.ID == id);
                        detail.Code = quoteCode;
                        detail.DiagramNumber = diagramNumber;
                    }
                    //如果同一节点下的数据【数据库中的，新增的】存在相同定额编号且图号相同的数据，则不予保存，
                    if (dicTree[parentid].Any(p => p.QuoteCode == quoteCode && p.DiagramNumber == diagramNumber && p.ID != id)
                        || (curNodeDBData.Details != null && curNodeDBData.Details.Any(p => p.Code == quoteCode && p.DiagramNumber == diagramNumber && p.ID != id)))
                    {
                        errMsg += "第" + errRowIndex + "行定额编号、图号与当前节点下已存在的明细数据重复" + Environment.NewLine;
                    }
                }

                if (!string.IsNullOrEmpty(errMsg))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                errMsg += ExceptionUtil.ExceptionMessage(ex) + Environment.NewLine;
                return false;
            }
            finally
            {
                if (!string.IsNullOrEmpty(errMsg))
                {
                    WriteLog(errMsg);
                }
            }
        }
        /// <summary>
        /// 契约有效性验证
        /// </summary>
        /// <returns>true:有效【已选】，false：无效【未选】</returns>
        private bool ValidContract()
        {
            if (contract == null)
            {
                MessageBox.Show("请选择驱动契约组！");

                VSelectWBSContractGroup frm = new VSelectWBSContractGroup(new MWBSContractGroup(), false);
                frm.ShowDialog();
                if (frm.SelectResult.Count > 0)
                {
                    contract = frm.SelectResult[0] as ContractGroup;
                    txtContractGroupName.Text = contract.ContractName;
                    txtContractGroupName.Tag = contract;
                    return true;
                }
                return false;
            }
            return true;
        }
        private void WriteLog(string errMsg)
        {
            //写日志
            string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "importErrorLog" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            StreamWriter write = new StreamWriter(logFilePath, false, Encoding.Default);
            write.WriteLine("在执行操作时出现以下错误：" + Environment.NewLine + errMsg);
            write.Close();
            write.Dispose();

            FileInfo file = new FileInfo(logFilePath);
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
            catch
            {
            }
        }

        private string QuoteCodeValid(string strQuotaCode, int rowIndex)
        {
            string errMsg = "";
            int errRowIndex = rowIndex - 2; ;
            var costitem = LstQuoteGridCostItem.FirstOrDefault(p => p.QuotaCode == strQuotaCode);
            if (costitem != null)
            {
                ///目前只校验拷贝的数据，手动输入的修改的在导入时校验即可
                //根据定额编号获取资源耗用，从而控制各列是否写 
                //如果已存在该定额编号的记录，则不在重复添加
                //if (!lstRelatedSubjectCostQuotaTemp.Any(p => p.QuotaCode == costitem.QuotaCode))
                //{
                //    ObjectQuery oqQuota = new ObjectQuery();
                //    oqQuota.AddCriterion(Expression.Eq("TheCostItem.Id", costitem.Id));
                //    var lstSubjectCostQuota = model.ObjectQuery(typeof(SubjectCostQuota), oqQuota).OfType<SubjectCostQuota>().ToList();
                //    if (lstRelatedSubjectCostQuotaTemp == null)
                //    {
                //        lstRelatedSubjectCostQuotaTemp = new List<SubjectCostQuotaTemp>();
                //    }

                //    foreach (var item in lstSubjectCostQuota)
                //    {
                //        var obj = new SubjectCostQuotaTemp()
                //        {
                //            QuotaCode = costitem.QuotaCode,
                //            Id = item.Id,
                //            ItemID = item.TheCostItem.Id,
                //            MainResourceFlag = item.MainResourceFlag
                //        };
                //        lstRelatedSubjectCostQuotaTemp.Add(obj);
                //    }
                //}

                var lstCurQuotaSourse = lstRelatedSubjectCostQuotaTemp.Where(p => p.QuotaCode == costitem.QuotaCode).ToList();
                if (lstCurQuotaSourse == null || lstCurQuotaSourse.Count == 0)
                {
                    //MessageBox.Show("所选择的成本项暂无相关资源耗用，请重新选择！");
                    errMsg += "第" + errRowIndex + "行所选择的成本项暂无相关资源耗用，请重新选择！" + Environment.NewLine;
                    SetCellW(rowIndex);
                }
                else if (lstCurQuotaSourse.Count == 1)
                {
                    SetCellRW(rowIndex, lstCurQuotaSourse[0].MainResourceFlag);
                }
                else if (lstCurQuotaSourse.Count == 2)
                {
                    if (lstCurQuotaSourse.All(p => p.MainResourceFlag))
                    {
                        //MessageBox.Show("所选择的成本项相关的2条资源耗用，均为主资源，请重新选择！");
                        errMsg += "第" + errRowIndex + "行所选择的成本项相关的2条资源耗用，均为主资源，请重新选择！" + Environment.NewLine;
                    }

                    if (lstCurQuotaSourse.All(p => p.MainResourceFlag == false))
                    {
                        //MessageBox.Show("所选择的成本项相关的2条资源耗用，均为非主资源，请重新选择！");
                        errMsg += "第" + errRowIndex + "行所选择的成本项相关的2条资源耗用，均为非主资源，请重新选择！" + Environment.NewLine;
                    }
                    SetCellW(rowIndex);
                }
                else
                {
                    //MessageBox.Show("所选择的成本项存在" + lstCurQuotaSourse.Count + "相关资源耗用，请重新选择！");
                    errMsg += "第" + errRowIndex + "行所选择的成本项存在" + lstCurQuotaSourse.Count + "相关资源耗用，请重新选择！" + Environment.NewLine;
                    SetCellW(rowIndex);
                }
            }
            return errMsg;
        }

        #region 公共方法
        public void FlexGrid_Click(object Sender, EventArgs e)
        {

            int iRow = 0, iCol = 0;
            FlexCell.Cell oCell = null;

            if (!string.IsNullOrEmpty(flexGridWBS.ActiveCell.ImageKey))
            {
                flexGridWBS.AutoRedraw = false;
                bool bExpand = flexGridWBS.ActiveCell.ImageKey == imageExpand;//未展开imageExpand
                string sCurID = string.Empty;
                iRow = flexGridWBS.ActiveCell.Row;
                iCol = flexGridWBS.ActiveCell.Col;
                // flexGridWBS.Row(iRow).Visible = bExpand;
                flexGridWBS.ActiveCell.SetImage(bExpand ? imageCollapse : imageExpand);
                sCurID = flexGridWBS.Cell(iRow, endImageCol + 5).Tag;
                if (!string.IsNullOrEmpty(sCurID))
                {
                    SetImage(flexGridWBS, sCurID, bExpand, iRow + 1, iCol);
                }
                GridRefresh();
            }
        }
        public void SetImage(CustomFlexGrid flexGridWBS, string sParentID, bool bExpand, int iRow, int iCol)
        {
            Hashtable ht = new Hashtable();
            int iTotal = flexGridWBS.Rows - 1;//有一行便于插入行的隐藏的空行，故需-1
            FlexCell.Cell oCell = null;
            FlexCell.Row oRow = null;
            string sCurID = string.Empty, sCurParentID = string.Empty;
            if (!string.IsNullOrEmpty(sParentID) && iTotal > iRow && flexGridWBS.Cols > iCol)
            {
                ht.Add(sParentID, "");
                for (int iStart = iRow; iStart < iTotal; iStart++)
                {
                    oRow = flexGridWBS.Row(iStart);
                    sCurID = flexGridWBS.Cell(iStart, endImageCol + 5).Tag;
                    sCurParentID = flexGridWBS.Cell(iStart, endImageCol + 6).Tag;
                    if (ht.ContainsKey(sCurParentID))
                    {
                        if (string.Equals(sCurParentID, sParentID))
                        {
                            oRow.Visible = bExpand;
                            oCell = flexGridWBS.Cell(iStart, iCol + 1);
                            if (!string.IsNullOrEmpty(oCell.ImageKey))
                            {
                                flexGridWBS.Cell(iStart, iCol + 1).SetImage(imageExpand);
                            }
                        }
                        else if (!bExpand)
                        {
                            oRow.Visible = bExpand;
                        }
                        if (!string.IsNullOrEmpty(sCurID))
                        {
                            ht.Add(sCurID, "");
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        public void InitFlexGrid(int iRows, bool autoRedraw)
        {
            int iCol, iRow, endImageCol = 19;
            FlexCell.Cell oCell = null;
            FlexCell.Column oColumn = null;
            flexGridWBS.AutoRedraw = false;
            flexGridWBS.Rows = 1;
            flexGridWBS.Cols = 1;
            flexGridWBS.Locked = false;
            flexGridWBS.Cell(0, 0).Locked = true;
            flexGridWBS.Row(0).Locked = false;
            flexGridWBS.DisplayRowNumber = true;
            flexGridWBS.StartRowNumber = 1;
            flexGridWBS.Rows = titleRows + iRows;
            flexGridWBS.Cols = endImageCol + 1 + 11;//其中0列隐藏 1-19 为放置图片列 20之后的为数据列
            flexGridWBS.Column(0).Visible = true;
            flexGridWBS.Column(0).AutoFit();
            flexGridWBS.SelectionMode = FlexCell.SelectionModeEnum.Free;
            flexGridWBS.DisplayFocusRect = true;
            //flexGridWBS.LockButton = true;
            flexGridWBS.ReadonlyFocusRect = FlexCell.FocusRectEnum.Solid;
            flexGridWBS.BorderStyle = FlexCell.BorderStyleEnum.FixedSingle;
            flexGridWBS.ScrollBars = FlexCell.ScrollBarsEnum.Both;
            flexGridWBS.BackColorBkg = SystemColors.Control;
            flexGridWBS.BackColor1 = SystemColors.Control;
            flexGridWBS.BackColor2 = SystemColors.Control;
            flexGridWBS.DefaultFont = new Font("Tahoma", 8);
            //设置单元格小数位数
            //var colindex = endImageCol + 3;
            //flexGridWBS.Column(colindex).DecimalLength = 5;
            //flexGridWBS.Column(++colindex).DecimalLength = 5;
            //flexGridWBS.Column(++colindex).DecimalLength = 5;
            //flexGridWBS.Column(++colindex).DecimalLength = 5;
            //flexGridWBS.Column(++colindex).DecimalLength = 5;
            //flexGridWBS.Column(++colindex).DecimalLength = 5;
            //flexGridWBS.Column(++colindex).DecimalLength = 5;
            //flexGridWBS.Column(++colindex).DecimalLength = 5;
            //flexGridWBS.Column(++colindex).DecimalLength = 5;

            FlexCell.Range oRange;

            //项目名称
            iRow = 0; iCol = 1;
            oRange = flexGridWBS.Range(iRow, iCol, iRow, 5);
            oRange.Merge();
            oCell = flexGridWBS.Cell(iRow, iCol);
            oCell.Text = "项目名称:";

            iCol = 6;
            oRange = flexGridWBS.Range(iRow, iCol, iRow, flexGridWBS.Cols - 1);
            oRange.Merge();
            oCell = flexGridWBS.Cell(iRow, iCol);
            oCell.Text = projectInfo == null ? "" : projectInfo.Name;
            oCell.Alignment = FlexCell.AlignmentEnum.LeftCenter;

            //任务名称
            iRow += 1; iCol = 1;
            for (int i = iCol; i < endImageCol + iCol; i++)
            {
                //flexGridWBS.Column(i).TabStop = false;
                flexGridWBS.Column(i).Width = 20;
            }
            oRange = flexGridWBS.Range(iRow, iCol, iRow + 1, endImageCol + iCol - 1);
            oRange.Merge();
            flexGridWBS.Cell(iRow, iCol).Text = "任务名称";

            // 加载图片
            flexGridWBS.Images.Add(Resources.ImageExpend, imageExpand);
            flexGridWBS.Images.Add(Resources.ImageFold, imageCollapse);

            //成本项编码
            iCol = endImageCol + 1;
            oRange = flexGridWBS.Range(iRow, iCol, iRow + 1, iCol);
            oRange.Merge();
            flexGridWBS.Cell(iRow, iCol).Text = "定额编码";
            //oColumn = flexGridWBS.Column(iCol); oColumn.Width = 100; oColumn.CellType = FlexCell.CellTypeEnum.TextBox; oColumn.Mask = FlexCell.MaskEnum.None;

            //图号
            iCol = iCol + 1;
            oRange = flexGridWBS.Range(iRow, iCol, iRow + 1, iCol);
            oRange.Merge();
            flexGridWBS.Cell(iRow, iCol).Text = "图号";

            //合同收入
            iCol++;
            oRange = flexGridWBS.Range(iRow, iCol, iRow, iCol + 2);
            oRange.Merge();
            oCell = flexGridWBS.Cell(iRow, iCol);
            oCell.Text = "合同收入";

            //责任成本		
            iCol = iCol + 3;
            oRange = flexGridWBS.Range(iRow, iCol, iRow, iCol + 2);
            oRange.Merge();
            oCell = flexGridWBS.Cell(iRow, iCol);
            oCell.Text = "责任成本";

            //计划成本		
            iCol = iCol + 3;
            oRange = flexGridWBS.Range(iRow, iCol, iRow, iCol + 2);
            oRange.Merge();
            oCell = flexGridWBS.Cell(iRow, iCol);
            oCell.Text = "计划成本";

            //合同收入 工程量
            iRow += 1; iCol = endImageCol + 3;
            oCell = flexGridWBS.Cell(iRow, iCol);
            oCell.Text = "工程量";

            //合同收入 其中人工单价 
            iCol += 1;
            oCell = flexGridWBS.Cell(iRow, iCol);
            oCell.Text = "其中人工单价";

            //合同收入 其中材料单价
            iCol += 1;
            oCell = flexGridWBS.Cell(iRow, iCol);
            oCell.Text = "其中材料单价";

            //责任成本 工程量
            iCol += 1;
            oCell = flexGridWBS.Cell(iRow, iCol);
            oCell.Text = "工程量";

            //责任成本 其中人工单价 
            iCol += 1;
            oCell = flexGridWBS.Cell(iRow, iCol);
            oCell.Text = "其中人工单价";

            //责任成本 其中材料单价
            iCol += 1;
            oCell = flexGridWBS.Cell(iRow, iCol);
            oCell.Text = "其中材料单价";

            //计划成本  工程量
            iCol += 1;
            oCell = flexGridWBS.Cell(iRow, iCol);
            oCell.Text = "工程量";

            //计划成本  其中人工单价              
            iCol += 1;
            oCell = flexGridWBS.Cell(iRow, iCol);
            oCell.Text = "其中人工单价";

            //计划成本   其中材料单价
            iCol += 1;
            oCell = flexGridWBS.Cell(iRow, iCol);
            oCell.Text = "其中材料单价";

            SetGridTitleStyle();

            if (autoRedraw)
            {
                SetGridLockState();
                GridRefresh();
            }
        }
        public IList<GWBSTreeTemp> getTree(DataTable table, List<GWBSDetailImport> lstImport)
        {
            dtDetailCostSubject = model.GetGWBSDetailCostSubject(OptGWBSTreeObj.SysCode);
            getTree(null, table, new Hashtable(), lstImport);
            return lstTree;
        }
        public void getTree(GWBSTreeTemp oCurrNode, DataTable table, Hashtable ht, List<GWBSDetailImport> lstImport)
        {
            Hashtable htDetail = new Hashtable();
            int iCount = 0;
            string sID = string.Empty;
            DataRow[] oRows = null;
            DataRow[] oDetailRows = null;
            GWBSTreeTemp oChildNode = null;
            GWBSDetailTemp oGWBSDetailTemp = null;
            IList<GWBSDetailTemp> lstDetails = null;
            if (oCurrNode == null)//找第一个节点
            {
                oCurrNode = new GWBSTreeTemp()
                {
                    Id = ClientUtil.ToString(table.Rows[0]["id"]),
                    Name = ClientUtil.ToString(table.Rows[0]["name"]),
                    SysCode = ClientUtil.ToString(table.Rows[0]["syscode"]),
                    OrderNo = ClientUtil.ToInt(table.Rows[0]["orderno"]),
                    ParentNode = new GWBSTreeTemp()
                    {
                        Id = ClientUtil.ToString(table.Rows[0]["parentnodeid"])
                    },
                    CategoryNodeType = (NodeType)ClientUtil.ToInt(table.Rows[0]["categorynodetype"]),
                    Level = ClientUtil.ToInt(table.Rows[0]["tlevel"]),
                    ContractArtificialTotalPrice = 0,
                    ContractMaterialTotalPrice = 0,
                    ContractTotalQuantity = 0,

                    ResponsibilityArtificialTotalPrice = 0,
                    ResponsibilityMaterialTotalPrice = 0,
                    ResponsibilityTotalQuantity = 0,

                    PlanTotalQuantity = 0,
                    PlanArtificialTotalPrice = 0,
                    PlanMaterialTotalPrice = 0
                };
                if (ht.ContainsKey(oCurrNode.Id))
                {
                    return;
                }
                else
                {
                    ht.Add(oCurrNode.Id, "");
                    lstTree.Insert(lstTree.Count, oCurrNode);
                }
            }

            #region 查找子节点
            oRows = table.Select(string.Format("parentnodeid='{0}' ", oCurrNode.Id, (int)NodeType.MiddleNode), "OrderNo asc,TLevel asc ");
            foreach (DataRow dataRow in oRows)
            {
                sID = ClientUtil.ToString(dataRow["Id"]);
                if (!ht.Contains(sID))
                {
                    oChildNode = new GWBSTreeTemp()
                    {
                        Id = dataRow["Id"].ToString(),
                        Name = dataRow["Name"].ToString(),
                        SysCode = dataRow["SysCode"].ToString(),
                        OrderNo = ClientUtil.ToLong(dataRow["orderNo"]),
                        ParentNode = new GWBSTreeTemp()
                        {
                            Id = dataRow["parentnodeid"].ToString()
                        },
                        CategoryNodeType = (NodeType)ClientUtil.ToInt(dataRow["CategoryNodeType"]),
                        Level = ClientUtil.ToInt(dataRow["tlevel"]),
                        // ContractTotalPrice = 0,
                        // ContractProgress = 0,
                        //PlanTotalPrice = 0,
                        //PlanProgress = 0,
                        // ResponsibilityTotalPrice = 0,
                        // ResponsibilityProgress = 0
                    };
                    ht.Add(oChildNode.Id, "");
                    lstTree.Insert(lstTree.Count, oChildNode);
                    getTree(oChildNode, table, ht, lstImport);//获取当前节点的子节点下面的孩子                  
                }
            }
            #endregion

            #region 任务明细
            lstDetails = new List<GWBSDetailTemp>();
            Console.WriteLine(string.Format("第{0:D5}:{1}", ++iCount, oCurrNode.Id));
            oDetailRows = table.Select(string.Format("id='{0}' and dNAME IS NOT NULL  ", oCurrNode.Id), "dOrderno asc");
            foreach (DataRow oDetailRow in oDetailRows)
            {
                if (!htDetail.ContainsKey(ClientUtil.ToString(oDetailRow["detailID"])))
                {
                    oGWBSDetailTemp = new GWBSDetailTemp()
                    {
                        ID = ClientUtil.ToString(oDetailRow["detailID"]),
                        Name = ClientUtil.ToString(oDetailRow["dName"]),
                        Code = ClientUtil.ToString(oDetailRow["quotacode"]),
                        CostItemGuid = ClientUtil.ToString(oDetailRow["CostItemGuid"]),
                        DiagramNumber = ClientUtil.ToString(oDetailRow["DiagramNumber"]),
                        // dtl.TheGWBS = new GWBSTree();
                        TheGWBS = new GWBSTreeTemp()
                        {
                            Id = ClientUtil.ToString(oDetailRow["id"]),
                            SysCode = ClientUtil.ToString(oDetailRow["SysCode"]),
                            CategoryNodeType = (NodeType)ClientUtil.ToInt(oDetailRow["CategoryNodeType"])
                        },

                        //责任成本
                        ResponsibilityQuantity = ToIntOrNull(oDetailRow["responsibilitilyworkamount"].ToString()),
                        //合同收入
                        ContractQuantity = ToIntOrNull(oDetailRow["ContractProjectQuantity"].ToString()),
                        //计划成本
                        PlanQuantity = ToIntOrNull(oDetailRow["PlanWorkAmount"].ToString()),
                        OrderNo = ClientUtil.ToInt(oDetailRow["dOrderno"])
                    };

                    var subjectRows = dtDetailCostSubject.Select(string.Format(" GWBSDETAILID='{0}' ", oGWBSDetailTemp.ID));

                    foreach (var row in subjectRows)
                    {
                        if (row["MAINRESTYPEFLAG"].ToString() == "0")//
                        {
                            oGWBSDetailTemp.ContractArtificialPrice = ToDecimalOrNull(row["CONTRACTBASEPRICE"].ToString());
                            oGWBSDetailTemp.ResponsibilityArtificialPrice = ToDecimalOrNull(row["RESPONSIBLEBASEPRICE"].ToString());
                            oGWBSDetailTemp.PlanArtificialPrice = ToDecimalOrNull(row["PLANBASEPRICE"].ToString());
                        }
                        else
                        {
                            oGWBSDetailTemp.ContractMaterialPrice = ToDecimalOrNull(row["CONTRACTBASEPRICE"].ToString()); ;
                            oGWBSDetailTemp.ResponsibilityMaterialPrice = ToDecimalOrNull(row["RESPONSIBLEBASEPRICE"].ToString());
                            oGWBSDetailTemp.PlanMaterialPrice = ToDecimalOrNull(row["PLANBASEPRICE"].ToString());
                        }
                    }

                    htDetail.Add(oGWBSDetailTemp.ID, "");
                    lstDetails.Insert(lstDetails.Count, oGWBSDetailTemp);
                }
            }
            #endregion

            #region 处理任务明细导入表数据
            if (lstImport != null && lstImport.Count > 0)
            {
                var curNodeLstDetail = lstImport.Where(p => p.ParentId == oCurrNode.Id).ToList();
                foreach (var item in curNodeLstDetail)
                {
                    if (!htDetail.ContainsKey(item.Id))
                    {
                        oGWBSDetailTemp = new GWBSDetailTemp()
                        {
                            ID = item.Id,
                            //CostItemGuid =
                            Name = item.Name,
                            Code = item.Code,
                            State = item.State.GetHashCode().ToString(),
                            OrderNo = item.OrderNo,
                            DiagramNumber = item.DiagramNumber,
                            // dtl.TheGWBS = new GWBSTree();
                            TheGWBS = new GWBSTreeTemp()
                            {
                                Id = item.Id,
                                //SysCode = oCurrNode.SysCode,
                                CategoryNodeType = NodeType.LeafNode
                            },

                            //责任成本
                            ResponsibilityQuantity = item.ResponsibilityQuantity,
                            ResponsibilityMaterialPrice = item.ResponsibilityMaterialPrice,
                            ResponsibilityArtificialPrice = item.ResponsibilityArtificialPrice,
                            //合同收入
                            ContractArtificialPrice = item.ContractArtificialPrice,
                            ContractMaterialPrice = item.ContractMaterialPrice,
                            ContractQuantity = item.ContractQuantity,
                            //计划成本
                            PlanArtificialPrice = item.PlanArtificialPrice,
                            PlanMaterialPrice = item.PlanMaterialPrice,
                            PlanQuantity = item.PlanQuantity

                            //  OrderNo = lstDetails.Count
                        };
                        htDetail.Add(oGWBSDetailTemp.ID, "");
                        lstDetails.Insert(lstDetails.Count, oGWBSDetailTemp);
                    }
                }
            }
            #endregion

            oCurrNode.Details = (lstDetails == null || lstDetails.Count == 0) ? null : lstDetails.OrderBy(p => p.OrderNo).ToList();
        }
        private void FillFlex()
        {
            if (lstTree != null && lstTree.Count > 0)
            {
                flexGridWBS.AutoRedraw = false;
                InitFlexGrid(1, false);
                FlexCell.Cell oCell = null;
                int iExpandLevel = 1;
                //填充
                GWBSTreeTemp oParentTree = lstTree[0];
                int iRow = flexGridWBS.Rows - 1, iCol = 1, startLevel = 0, iLevel = 0;
                FlexCell.Range rangeTemp = null;
                startLevel = oParentTree.Level;
                foreach (GWBSTreeTemp gt in lstTree)
                {
                    #region WPS树处理
                    //加载工程任务
                    flexGridWBS.InsertRow(iRow, 1);
                    iCol = gt.Level - startLevel + 1;

                    var detailCount = 0;//当前节点下显示的明细数量
                    var maxNo = 0;//当前节点下所有明细中最大的序号
                    if (gt.Details == null)
                    {
                        detailCount = 0;
                        maxNo = 0;
                    }
                    else
                    {
                        maxNo = gt.Details.Max(p => p.OrderNo);
                        if (chbShowAdded.Checked)
                        {
                            detailCount = gt.Details.Count;
                        }
                        else
                        {
                            //State=2则表示是新增的数据
                            detailCount = gt.Details.Count(p => p.State == "1");
                        }
                    }

                    //如果是叶节点 并且后面的没任务明细不显示树形图片
                    if (gt.CategoryNodeType == NodeType.LeafNode && detailCount == 0)
                    {
                        flexGridWBS.Row(iRow).Visible = (gt.Level - startLevel < iExpandLevel + 1) ? true : false;
                    }
                    else
                    {
                        flexGridWBS.Cell(iRow, iCol).SetImage((gt.Level - startLevel < iExpandLevel) ? imageCollapse : imageExpand);
                        flexGridWBS.Row(iRow).Visible = (gt.Level - startLevel < iExpandLevel + 1) ? true : false;
                        iCol += 1;
                    }
                    rangeTemp = flexGridWBS.Range(iRow, iCol, iRow, endImageCol);
                    flexGridWBS.Row(iRow).Locked = false;
                    rangeTemp.Merge();
                    flexGridWBS.Cell(iRow, endImageCol + 5).Tag = gt.Id;//存放ID
                    flexGridWBS.Cell(iRow, endImageCol + 6).Tag = gt.ParentNode.Id;//存放父节点ID
                    flexGridWBS.Cell(iRow, endImageCol + 7).Tag = gt.Level.ToString();//存放当前节点层次                
                    flexGridWBS.Cell(iRow, endImageCol + 8).Tag = iCol.ToString();//存放当前节点中名称所在列的index                
                    flexGridWBS.Cell(iRow, endImageCol + 1).Tag = gt.CategoryNodeType == NodeType.LeafNode ? "1" : "2";//0:任务明细数据；1：叶节点；2根节点、中间节点
                    flexGridWBS.Cell(iRow, endImageCol + 2).Tag = ""; //状态
                    flexGridWBS.Cell(iRow, endImageCol + 3).Tag = ClientUtil.ToString(detailCount); //明细数量
                    flexGridWBS.Cell(iRow, endImageCol + 4).Tag = ClientUtil.ToString(maxNo); //明细数量

                    oCell = flexGridWBS.Cell(iRow, iCol);//存放wbs/明细任务名称
                    oCell.Text = "★" + gt.Name;
                    oCell.FontBold = true;
                    iCol = endImageCol;
                    int parentNodeIndex = iRow;
                    #endregion

                    #region 合同，计划，责任成本处理
                    if (gt.Details != null && gt.Details.Count > 0)
                    {
                        iLevel = gt.Level - startLevel + 2;
                        foreach (GWBSDetailTemp oDetail in gt.Details)
                        {
                            //如果未勾选，则不显示已提交的数据,State=2则表示是新增的提交数据，State=null则表示是已提交的数据
                            if (!chbShowAdded.Checked && (string.IsNullOrEmpty(oDetail.State) || oDetail.State == "2"))
                            {
                                continue;
                            }
                            iRow += 1; iCol = iLevel;
                            flexGridWBS.InsertRow(iRow, 1);
                            flexGridWBS.Cell(iRow, endImageCol + 5).Tag = string.IsNullOrEmpty(oDetail.ID) ? "" : oDetail.ID;
                            flexGridWBS.Cell(iRow, endImageCol + 6).Tag = gt.Id;
                            flexGridWBS.Cell(iRow, endImageCol + 7).Tag = (gt.Level + 1).ToString();//存放当前节点层次
                            flexGridWBS.Cell(iRow, endImageCol + 8).Tag = iCol.ToString();//存放当前节点中名称所在列的index     
                            flexGridWBS.Cell(iRow, endImageCol + 1).Tag = "0";//如果wbs节点=1  如果该节点为wbs任务明细=0
                            flexGridWBS.Cell(iRow, endImageCol + 2).Tag = oDetail.State; //状态
                            flexGridWBS.Cell(iRow, endImageCol + 4).Tag = ClientUtil.ToString(oDetail.OrderNo); //明细序号
                            flexGridWBS.Cell(iRow, endImageCol + 10).Tag = parentNodeIndex.ToString();//父节点index
                            flexGridWBS.Row(iRow).Visible = (gt.Level - startLevel < iExpandLevel) ? true : false; ;

                            oCell = flexGridWBS.Cell(iRow, iCol);
                            oCell.Text = oDetail.Name;
                            oCell.FontBold = false;
                            //oCell.BackColor = Color.Yellow;
                            rangeTemp = flexGridWBS.Range(iRow, iLevel, iRow, endImageCol);
                            flexGridWBS.Row(iRow).Locked = false;
                            rangeTemp.Merge();

                            iCol = endImageCol + 1;
                            //定额编号
                            oCell = flexGridWBS.Cell(iRow, iCol);
                            oCell.Text = oDetail.Code;
                            //oCell.BackColor = Color.Yellow;

                            iCol = iCol + 1;
                            //图号
                            oCell = flexGridWBS.Cell(iRow, iCol);
                            oCell.Text = oDetail.DiagramNumber;

                            //合同收入	数量  其中人工单价	其中材料单价
                            iCol++;
                            oCell = flexGridWBS.Cell(iRow, iCol);
                            oCell.Text = QuantityFormatter(oDetail.ContractQuantity);
                            //oCell.Mask = MaskEnum.Numeric;
                            iCol++;
                            oCell = flexGridWBS.Cell(iRow, iCol);
                            oCell.Text = PriceFormatter(oDetail.ContractArtificialPrice);
                            //oCell.Mask = MaskEnum.Numeric;
                            iCol++;
                            oCell = flexGridWBS.Cell(iRow, iCol);
                            oCell.Text = PriceFormatter(oDetail.ContractMaterialPrice);
                            //oCell.Mask = MaskEnum.Numeric;

                            //责任收入	数量  其中人工单价	其中材料单价
                            iCol++;
                            oCell = flexGridWBS.Cell(iRow, iCol);
                            oCell.Text = QuantityFormatter(oDetail.ResponsibilityQuantity);
                            //oCell.Mask = MaskEnum.Numeric;
                            iCol++;
                            oCell = flexGridWBS.Cell(iRow, iCol);
                            oCell.Text = PriceFormatter(oDetail.ResponsibilityArtificialPrice);
                            //oCell.Mask = MaskEnum.Numeric;
                            iCol++;
                            oCell = flexGridWBS.Cell(iRow, iCol);
                            oCell.Text = PriceFormatter(oDetail.ResponsibilityMaterialPrice);
                            //oCell.Mask = MaskEnum.Numeric;

                            //计划收入	数量  其中人工单价	其中材料单价
                            iCol++;
                            oCell = flexGridWBS.Cell(iRow, iCol);
                            oCell.Text = QuantityFormatter(oDetail.PlanQuantity);
                            //oCell.Mask = MaskEnum.Numeric;
                            iCol++;
                            oCell = flexGridWBS.Cell(iRow, iCol);
                            oCell.Text = PriceFormatter(oDetail.PlanArtificialPrice);
                            //oCell.Mask = MaskEnum.Numeric;
                            iCol++;
                            oCell = flexGridWBS.Cell(iRow, iCol);
                            oCell.Text = PriceFormatter(oDetail.PlanMaterialPrice);
                            //oCell.Mask = MaskEnum.Numeric;

                            //如果未填写完整，则标记颜色。已经保存的数据，最多只存在两条资源信息：人工，材料
                            var lstReateSourse = lstRelatedSubjectCostQuotaTemp.Where(p => p.QuotaCode == oDetail.Code && p.ItemID == oDetail.CostItemGuid).ToList();
                            if (lstReateSourse != null && string.IsNullOrEmpty(oDetail.State))
                            {
                                string isInputCompleted = "1";
                                if (lstReateSourse.Count == 1)
                                {
                                    if (lstReateSourse[0].MainResourceFlag)
                                    {
                                        if (ClientUtil.ToDecimal(oDetail.ContractMaterialPrice) == 0 || ClientUtil.ToDecimal(oDetail.ResponsibilityMaterialPrice) == 0 || ClientUtil.ToDecimal(oDetail.PlanMaterialPrice) == 0)
                                        {
                                            isInputCompleted = "0";
                                        }
                                    }
                                    else
                                    {
                                        if (ClientUtil.ToDecimal(oDetail.ContractArtificialPrice) == 0 || ClientUtil.ToDecimal(oDetail.ResponsibilityArtificialPrice) == 0 || ClientUtil.ToDecimal(oDetail.PlanArtificialPrice) == 0)
                                        {
                                            isInputCompleted = "0";
                                        }
                                    }

                                    if (ClientUtil.ToDecimal(oDetail.ContractQuantity) == 0 || ClientUtil.ToDecimal(oDetail.ResponsibilityQuantity) == 0 || ClientUtil.ToDecimal(oDetail.PlanQuantity) == 0)
                                    {
                                        isInputCompleted = "0";
                                    }
                                }
                                else if (lstReateSourse.Count == 2)
                                {
                                    if (ClientUtil.ToDecimal(oDetail.ContractMaterialPrice) == 0 || ClientUtil.ToDecimal(oDetail.ResponsibilityMaterialPrice) == 0 || ClientUtil.ToDecimal(oDetail.PlanMaterialPrice) == 0
                                        || ClientUtil.ToDecimal(oDetail.ContractArtificialPrice) == 0 || ClientUtil.ToDecimal(oDetail.ResponsibilityArtificialPrice) == 0 || ClientUtil.ToDecimal(oDetail.PlanArtificialPrice) == 0
                                        || ClientUtil.ToDecimal(oDetail.ContractQuantity) == 0 || ClientUtil.ToDecimal(oDetail.ResponsibilityQuantity) == 0 || ClientUtil.ToDecimal(oDetail.PlanQuantity) == 0)
                                    {
                                        isInputCompleted = "0";
                                    }
                                }
                                flexGridWBS.Cell(iRow, endImageCol + 9).Tag = isInputCompleted;
                            }

                        }
                    }
                    #endregion

                    iRow += 1;
                }

                //设置数据行样式
                Range range = flexGridWBS.Range(titleRows, 1, flexGridWBS.Rows - 1, endImageCol + 1);
                if (range != null)
                {
                    range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                    //range.BackColor = SystemColors.Control;
                }

                range = flexGridWBS.Range(titleRows, endImageCol + 2, flexGridWBS.Rows - 1, flexGridWBS.Cols - 1);
                if (range != null)
                {
                    range.Alignment = FlexCell.AlignmentEnum.RightCenter;
                    //range.BackColor = SystemColors.Control;
                }
                //保留最后一行空行，但不显示，以便可以插入空行
                flexGridWBS.Row(flexGridWBS.Rows - 1).Visible = false;
                SetGridLockState();
                GridRefresh();
            }
        }

        /// <summary>
        /// 设置表格样式
        /// </summary>
        /// <returns></returns>
        private void SetGridTitleStyle()
        {
            //2,3行标题背景色，对齐样式
            FlexCell.Range range = flexGridWBS.Range(1, endImageCol + 1, titleRows - 1, flexGridWBS.Cols - 1);
            if (range != null)
            {
                range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
                //range.BackColor = SystemColors.Control;
            }

            //设置标题第一行的背景色，对齐样式
            range = flexGridWBS.Range(1, 1, titleRows - 1, endImageCol);
            if (range != null)
            {
                range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                //range.BackColor = SystemColors.Control;
            }
        }

        private void SetGridLockState()
        {
            Range range = flexGridWBS.Range(0, 0, 2, flexGridWBS.Cols - 1);
            range.Locked = true;
            flexGridWBS.FixedRows = titleRows;
        }

        public void GridRefresh()
        {
            //排除掉最后一行隐藏的空行
            if (flexGridWBS.Rows - 1 <= titleRows)
            {
                flexGridWBS.AutoRedraw = true;
                flexGridWBS.Refresh();
                return;
            }

            //排除掉最后一行隐藏的空行
            for (int i = titleRows; i < flexGridWBS.Rows - 1; i++)
            {
                //非叶结点，直接跳过
                if (flexGridWBS.Row(i).Locked && flexGridWBS.Cell(i, endImageCol + 1).Tag != "0")
                {
                    continue;
                }
                Console.Write(flexGridWBS.Cell(i, 9).Tag);
                Color curColor = SystemColors.Control;
                //节点类型【0:任务明细数据；1：叶节点；2根节点、中间节点】；状态【1：新建，2：已提交】
                if (flexGridWBS.Cell(i, endImageCol + 9).Tag == "0")//未完整填写数据的，标识为红色
                {
                    Range curRowRange = flexGridWBS.Range(i, 1, i, flexGridWBS.Cols - 1);
                    curRowRange.BackColor = Color.Pink;
                    flexGridWBS.Row(i).Locked = true;
                }
                else if (flexGridWBS.Cell(i, endImageCol + 1).Tag == "0" && flexGridWBS.Cell(i, endImageCol + 2).Tag == "1")//如果是未提交的则标记为黄色
                {
                    curColor = Color.Yellow;
                    //如果是未提交的，但lstRowIndex【新增数据的行索引】不包含，则添加进lstRowIndex中
                    if (!lstRowIndex.Any(p => p == i))
                    {
                        lstRowIndex.Add(i);
                    }
                    flexGridWBS.Range(i, 1, i, flexGridWBS.Cols - 1).BackColor = curColor;
                    flexGridWBS.Cell(i, endImageCol + 1).CellType = CellTypeEnum.Button;

                    var quoteCode = flexGridWBS.Cell(i, endImageCol + 1).Text.Trim();
                    var lstResource = lstRelatedSubjectCostQuotaTemp.Where(p => p.QuotaCode == quoteCode).ToList();
                    if (lstResource.Count == 1)
                    {
                        SetCellRW(i, ClientUtil.ToString(lstResource[0].MainResourceFlag) == "1");
                    }
                    else
                    {
                        SetCellW(i);
                    }
                }
                else if (lstRowIndex.Any(p => p == i))//如果是未提交的则标记为黄色
                {
                    curColor = Color.Yellow;
                    flexGridWBS.Range(i, 1, i, flexGridWBS.Cols - 1).BackColor = curColor;
                    flexGridWBS.Cell(i, endImageCol + 1).CellType = CellTypeEnum.Button;

                    var quoteCode = flexGridWBS.Cell(i, endImageCol + 1).Text.Trim();
                    var lstResource = lstRelatedSubjectCostQuotaTemp.Where(p => p.QuotaCode == quoteCode).ToList();
                    if (lstResource.Count == 1)
                    {
                        SetCellRW(i, ClientUtil.ToString(lstResource[0].MainResourceFlag).ToString() == "1");
                    }
                    else
                    {
                        SetCellW(i);
                    }
                }
                else
                {
                    flexGridWBS.Row(i).Locked = true;
                }
            }
            flexGridWBS.Column(0).AutoFit(); ;
            flexGridWBS.AutoSize = false;
            flexGridWBS.Refresh();
            flexGridWBS.AutoRedraw = true;
        }

        public decimal? ToDecimalOrNull(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            else
            {
                return ClientUtil.ToDecimal(str);
            }
        }
        public decimal? ToIntOrNull(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            else
            {
                return ClientUtil.ToInt(str);
            }
        }

        public string PriceFormatter(decimal? val)
        {
            if (val.HasValue && val != 0)
            {
                return val.Value.ToString("N2");
            }
            else
            {
                return "";
            }
        }

        public string QuantityFormatter(decimal? val)
        {
            if (val.HasValue && val != 0)
            {
                return val.Value.ToString();
            }
            else
            {
                return "";
            }
        }

        public void PasteCopiedContent()
        {
            //剪贴板的行数
            var rowCount = 0;
            //剪贴板的列数
            var colCount = 0;
            IDataObject iData = Clipboard.GetDataObject();
            if (iData.GetDataPresent(DataFormats.Text))
            {
                string ClipboardData = (String)iData.GetData(DataFormats.Text);
                //获取复制【剪切板上】的行数据
                var rowDatas = ClipboardData.Split(new[] { "\r\n" }, StringSplitOptions.None);
                if (rowDatas != null && rowDatas.Length > 0)
                {
                    rowCount = rowDatas.Length;
                    var curRow = flexGridWBS.Selection.FirstRow;//当前选择行
                    var curCol = flexGridWBS.Selection.FirstCol;//当前选择列
                    for (int i = curRow; i < curRow + rowCount; i++)//行
                    {
                        //获取列数据
                        var colDatas = rowDatas[i - curRow].Split(new[] { "\t" }, StringSplitOptions.None);
                        if (colDatas != null && colDatas.Length > 0)
                        {
                            colCount = colDatas.Length;
                        }
                        //如果是新增的行，则粘贴
                        if (lstRowIndex.Any(p => p == i))
                        {
                            for (int j = curCol; j < curCol + colCount; j++)//列
                            {
                                //复制的内容不为空，则粘贴
                                if (!string.IsNullOrEmpty(colDatas[j - curCol]))
                                {
                                    flexGridWBS.Cell(i, j).Text = colDatas[j - curCol];
                                }
                            }
                        }
                    }
                }
            }


        }

        /// <summary>
        /// 设置单元格读写权限
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="lstSubjectCostQuota"></param>
        public void SetCellRW(int rowIndex, bool isMainResourceFlag)
        {
            //只读单元格的列索引
            var rColIndex = 0;
            //可写单元格的列索引
            var wColIndex = 0;
            //如果资源项是材料，则设置人工项为只读
            if (isMainResourceFlag)
            {
                rColIndex = endImageCol + 4;
                wColIndex = endImageCol + 5;
            }
            else//如果资源项不是材料，则设置材料项为只读
            {
                rColIndex = endImageCol + 5;
                wColIndex = endImageCol + 4;
            }
            flexGridWBS.Cell(rowIndex, rColIndex).BackColor = SystemColors.Control;// Color.Gainsboro;
            flexGridWBS.Cell(rowIndex, rColIndex).Locked = true;
            flexGridWBS.Cell(rowIndex, rColIndex).Text = "";//锁定时需清空数据
            flexGridWBS.Cell(rowIndex, wColIndex).BackColor = Color.Yellow;// Color.Gainsboro;
            flexGridWBS.Cell(rowIndex, wColIndex).Locked = false;
            rColIndex = rColIndex + 3;
            flexGridWBS.Cell(rowIndex, rColIndex).BackColor = SystemColors.Control;//Color.Gainsboro;
            flexGridWBS.Cell(rowIndex, rColIndex).Locked = true;
            flexGridWBS.Cell(rowIndex, rColIndex).Text = "";//锁定时需清空数据
            flexGridWBS.Cell(rowIndex, wColIndex).BackColor = Color.Yellow;// Color.Gainsboro;
            flexGridWBS.Cell(rowIndex, wColIndex).Locked = false;
            rColIndex = rColIndex + 3;
            flexGridWBS.Cell(rowIndex, rColIndex).BackColor = SystemColors.Control;// Color.Gainsboro;
            flexGridWBS.Cell(rowIndex, rColIndex).Locked = true;
            flexGridWBS.Cell(rowIndex, rColIndex).Text = "";//锁定时需清空数据
            flexGridWBS.Cell(rowIndex, wColIndex).BackColor = Color.Yellow;// Color.Gainsboro;
            flexGridWBS.Cell(rowIndex, wColIndex).Locked = false;
        }
        void SetCellW(int rowIndex)
        {
            //只读单元格的列索引
            var colIndex = endImageCol + 4;
            flexGridWBS.Cell(rowIndex, colIndex).BackColor = Color.Yellow;// Color.Gainsboro;
            flexGridWBS.Cell(rowIndex, colIndex).Locked = false;
            flexGridWBS.Cell(rowIndex, ++colIndex).BackColor = Color.Yellow;// Color.Gainsboro;
            flexGridWBS.Cell(rowIndex, colIndex).Locked = false;
            colIndex = colIndex + 2;
            flexGridWBS.Cell(rowIndex, colIndex).BackColor = Color.Yellow;//Color.Gainsboro;
            flexGridWBS.Cell(rowIndex, colIndex).Locked = false;
            flexGridWBS.Cell(rowIndex, ++colIndex).BackColor = Color.Yellow;// Color.Gainsboro;
            flexGridWBS.Cell(rowIndex, colIndex).Locked = false;
            colIndex = colIndex + 2;
            flexGridWBS.Cell(rowIndex, colIndex).BackColor = Color.Yellow;// Color.Gainsboro;
            flexGridWBS.Cell(rowIndex, colIndex).Locked = false;
            flexGridWBS.Cell(rowIndex, ++colIndex).BackColor = Color.Yellow;// Color.Gainsboro;
            flexGridWBS.Cell(rowIndex, colIndex).Locked = false;
        }

        #endregion

        #region   常用定额编号表格处理

        private void QueryQuoteCodeData()
        {
            var oq1 = new ObjectQuery();

            var strQuoteCode = txtQuoteCode.Text.Trim();
            var strName = txtName.Text.Trim();
            if (!string.IsNullOrEmpty(strQuoteCode))
            {
                oq1.AddCriterion(Expression.Like("QuotaCode", strQuoteCode, MatchMode.Anywhere));
            }

            if (!string.IsNullOrEmpty(strName))
            {
                oq1.AddCriterion(Expression.Like("Name", strName, MatchMode.Anywhere));
            }

            //页面初始化时,勾选框为勾选状态
            if (ckbCommonUsed.Checked)
            {
                oq1.AddCriterion(Expression.Eq("IsCommonlyUsed", true));
            }
            oq1.AddFetchMode("ListQuotas", NHibernate.FetchMode.Eager);
            LstQuoteGridCostItem = model.ObjectQuery(typeof(CostItem), oq1).OfType<CostItem>().ToList();
            if (lstRelatedSubjectCostQuotaTemp == null)
            {
                lstRelatedSubjectCostQuotaTemp = new List<SubjectCostQuotaTemp>();
            }
            foreach (CostItem item in LstQuoteGridCostItem)
            {
                foreach (SubjectCostQuota sub in item.ListQuotas)
                {
                    if (lstRelatedSubjectCostQuotaTemp.Any(p => p.Id == sub.Id) == false)
                    {
                        var obj = new SubjectCostQuotaTemp()
                        {
                            Id = sub.Id,
                            ItemID = item.Id,
                            MainResourceFlag = sub.MainResourceFlag,
                            QuotaCode = item.QuotaCode
                        };
                        lstRelatedSubjectCostQuotaTemp.Add(obj);
                    }
                }
            }
            FillQuoteCodeGrid(LstQuoteGridCostItem);
        }

        private void InitQuoteCodeGrid()
        {
            FlexCell.Cell oCell = null;
            FlexCell.Column oColumn = null;
            grid1QuoteCode.AutoRedraw = false;
            grid1QuoteCode.Locked = false;
            grid1QuoteCode.Cell(0, 0).Locked = true;
            grid1QuoteCode.Row(0).Locked = false;
            grid1QuoteCode.DisplayRowNumber = true;
            grid1QuoteCode.StartRowNumber = 1;
            grid1QuoteCode.Rows = 2;
            grid1QuoteCode.Cols = 3;
            grid1QuoteCode.Column(0).Visible = true;
            grid1QuoteCode.Column(0).AutoFit();
            grid1QuoteCode.SelectionMode = FlexCell.SelectionModeEnum.Free;
            grid1QuoteCode.DisplayFocusRect = true;
            grid1QuoteCode.ReadonlyFocusRect = FlexCell.FocusRectEnum.Solid;
            grid1QuoteCode.BorderStyle = FlexCell.BorderStyleEnum.FixedSingle;
            grid1QuoteCode.ScrollBars = FlexCell.ScrollBarsEnum.Both;
            grid1QuoteCode.BackColorBkg = SystemColors.Control;
            //grid1QuoteCode.BackColor1 = SystemColors.Control;
            //grid1QuoteCode.BackColor2 = SystemColors.Control;
            grid1QuoteCode.DefaultFont = new Font("Tahoma", 8);

            int iRow = 0, iCol = 1;
            oCell = grid1QuoteCode.Cell(iRow, iCol);
            oCell.Text = "成本项名称";

            iCol += 1;
            oCell = grid1QuoteCode.Cell(iRow, iCol);
            oCell.Text = "定额编号";

            grid1QuoteCode.FixedRows = 1;
            grid1QuoteCode.AutoRedraw = true;
            grid1QuoteCode.Refresh();
        }

        public void FillQuoteCodeGrid(List<CostItem> lstItem)
        {
            grid1QuoteCode.Rows = 1;
            grid1QuoteCode.Rows = 2;
            grid1QuoteCode.AutoRedraw = false;

            var newRowIndex = 1;
            foreach (CostItem item in lstItem)
            {
                newRowIndex = grid1QuoteCode.Rows - 1;
                grid1QuoteCode.InsertRow(newRowIndex, 1);
                grid1QuoteCode.Cell(newRowIndex, 1).Text = item.Name;
                grid1QuoteCode.Cell(newRowIndex, 2).Text = item.QuotaCode;
            }

            grid1QuoteCode.AutoRedraw = true;
            grid1QuoteCode.Refresh();
        }

        #endregion



        #region 临时类
        //[Serializable]
        public class GWBSTreeTemp : CategoryNode
        {
            //合同收入
            public decimal ContractArtificialTotalPrice { get; set; }
            public decimal ContractMaterialTotalPrice { get; set; }
            public decimal ContractTotalQuantity { get; set; }
            //责任成本 
            public decimal ResponsibilityArtificialTotalPrice { get; set; }
            public decimal ResponsibilityMaterialTotalPrice { get; set; }
            public decimal ResponsibilityTotalQuantity { get; set; }

            //计划成本 
            public decimal PlanArtificialTotalPrice { get; set; }
            public decimal PlanMaterialTotalPrice { get; set; }
            public decimal PlanTotalQuantity { get; set; }

            List<GWBSDetailImport> lstGWBSDetailImport { get; set; }
            IList<GWBSDetailTemp> lstDetails = null;
            public IList<GWBSDetailTemp> Details
            {
                get { return lstDetails; }
                set
                {
                    lstDetails = value;
                    this.ContractArtificialTotalPrice += (this.lstDetails == null ? 0 : this.lstDetails.Sum(Detail => Detail.ReadOnlyContractArtificialPrice));
                    this.ContractMaterialTotalPrice += (this.lstDetails == null ? 0 : this.lstDetails.Sum(Detail => Detail.ReadOnlyContractMaterialPrice));
                    this.ContractTotalQuantity += this.lstDetails == null ? 0 : this.lstDetails.Sum(Detail => Detail.ReadOnlyContractQuantity);

                    this.ResponsibilityArtificialTotalPrice += (this.lstDetails == null ? 0 : this.lstDetails.Sum(Detail => Detail.ReadOnlyResponsibilityArtificialPrice));
                    this.ResponsibilityMaterialTotalPrice += (this.lstDetails == null ? 0 : this.lstDetails.Sum(Detail => Detail.ReadOnlyResponsibilityMaterialPrice));
                    this.ResponsibilityTotalQuantity += (this.lstDetails == null ? 0 : this.lstDetails.Sum(Detail => Detail.ReadOnlyResponsibilityQuantity));

                    this.PlanArtificialTotalPrice += (this.lstDetails == null ? 0 : this.lstDetails.Sum(Detail => Detail.ReadOnlyPlanArtificialPrice));
                    this.PlanMaterialTotalPrice += (this.lstDetails == null ? 0 : this.lstDetails.Sum(Detail => Detail.ReadOnlyPlanMaterialPrice));
                    this.PlanTotalQuantity += (this.lstDetails == null ? 0 : this.lstDetails.Sum(Detail => Detail.ReadOnlyPlanQuantity));
                }
            }


        }
        // [Serializable]
        public class GWBSDetailTemp
        {
            public string ID { get; set; }
            public string CostItemGuid { get; set; }
            public string Code { get; set; }
            /// <summary>
            /// 图号
            /// </summary>
            public string DiagramNumber { get; set; }
            public string Name { get; set; }
            public string State { get; set; }
            public int OrderNo { get; set; }
            public GWBSTreeTemp TheGWBS { get; set; }

            public decimal ReadOnlyContractArtificialPrice { get { return ContractArtificialPrice.HasValue ? ContractArtificialPrice.Value : 0; } }
            public decimal ReadOnlyContractMaterialPrice { get { return ContractMaterialPrice.HasValue ? ContractMaterialPrice.Value : 0; } }
            public decimal ReadOnlyContractQuantity { get { return ContractQuantity.HasValue ? ContractQuantity.Value : 0; } }

            public decimal ReadOnlyResponsibilityArtificialPrice { get { return ResponsibilityArtificialPrice.HasValue ? ResponsibilityArtificialPrice.Value : 0; } }
            public decimal ReadOnlyResponsibilityMaterialPrice { get { return ResponsibilityMaterialPrice.HasValue ? ResponsibilityMaterialPrice.Value : 0; } }
            public decimal ReadOnlyResponsibilityQuantity { get { return ResponsibilityQuantity.HasValue ? ResponsibilityQuantity.Value : 0; } }

            public decimal ReadOnlyPlanArtificialPrice { get { return PlanArtificialPrice.HasValue ? PlanArtificialPrice.Value : 0; } }
            public decimal ReadOnlyPlanMaterialPrice { get { return PlanMaterialPrice.HasValue ? PlanMaterialPrice.Value : 0; } }
            public decimal ReadOnlyPlanQuantity { get { return PlanQuantity.HasValue ? PlanQuantity.Value : 0; } }

            /// <summary>
            /// 合同收入 人工单价
            /// </summary>
            public decimal? ContractArtificialPrice { get; set; }
            /// <summary>
            /// 合同收入 材料单价
            /// </summary>
            public decimal? ContractMaterialPrice { get; set; }
            /// <summary>
            /// 合同收入 数量
            /// </summary>
            public decimal? ContractQuantity { get; set; }

            /// <summary>
            /// 责任成本 人工单价
            /// </summary> 
            public decimal? ResponsibilityArtificialPrice { get; set; }
            /// <summary>
            /// 责任成本 材料单价
            /// </summary> 
            public decimal? ResponsibilityMaterialPrice { get; set; }
            /// <summary>
            /// 责任成本 数量
            /// </summary> 
            public decimal? ResponsibilityQuantity { get; set; }

            /// <summary>
            /// 计划成本 人工单价
            /// </summary> 
            public decimal? PlanArtificialPrice { get; set; }
            /// <summary>
            /// 计划成本 材料单价
            /// </summary> 
            public decimal? PlanMaterialPrice { get; set; }
            /// <summary>
            /// 计划成本 数量
            /// </summary> 
            public decimal? PlanQuantity { get; set; }
        }

        public class RowData
        {
            public string ID { set; get; }
            public string ParentID { set; get; }
            public int Index { set; get; }
            public string QuoteCode { set; get; }
            public string Name { set; get; }
            /// <summary>
            /// 图号
            /// </summary>
            public string DiagramNumber { get; set; }
        }

        public class SubjectCostQuotaTemp
        {
            public string QuotaCode { set; get; }
            public string ItemID { set; get; }
            public string Id { set; get; }
            public bool MainResourceFlag { set; get; }
        }
        #endregion
    }
}
