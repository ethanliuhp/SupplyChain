using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.Client.Properties;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    public partial class VCostMonthCompareReport :  TBasicDataView
    {
        private int startImageCol = 1, endImageCol = 0, defaultImageCol = 5, startRow = 4, defaultCollapse=1;
        private string imageExpand = "imageExpand";
        private string imageCollapse = "imageCollapse";
        MCostMonthAccount model = new MCostMonthAccount();
        MStockMng stockModel = new MStockMng();
        private CurrentProjectInfo ProjectInfo;
        public VCostMonthCompareReport()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }
        private void InitData()
        {
            ProjectInfo = StaticMethod.GetProjectInfo();
            this.cmbProject.Enabled = false;
            cmbProject.Text = ProjectInfo.Name;
            IList list = stockModel.StockInSrv.GetFiscalYear();
            this.cmbYear.Items.Clear();
            foreach (int iYear in list)
            {
                this.cmbYear.Items.Insert(this.cmbYear.Items.Count, iYear);
                if (iYear == ConstObject.TheLogin.TheComponentPeriod.NowYear)
                {
                    this.cmbYear.SelectedItem = this.cmbYear.Items[this.cmbYear.Items.Count - 1];
                }
            }

            for (int i = 1; i < 13; i++)
            {
                this.cboFiscalMonth.Items.Add(i);
            }
            this.cboFiscalMonth.Text = ConstObject.TheLogin.TheComponentPeriod.NowMonth.ToString();
            flexGrid.Images.Add(Resources.ImageExpend, imageExpand);
            flexGrid.Images.Add(Resources.ImageFold, imageCollapse);
           
            endImageCol = defaultImageCol;
            InitalFlexHead(true);

           
        }
        List<TempColumn> lstHideColumns = new List<TempColumn>();
        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnSelectGWBS.Click += new EventHandler(btSelectGWBS_Click);
            this.flexGrid.Click += new FlexCell.Grid.ClickEventHandler(FlexGrid_Click);
            linkSetColumn.Click+=new EventHandler(linkSetColumn_Click);
        }
        void linkSetColumn_Click(object sender, EventArgs e)
        {
            int iStartRowIndex=startRow - 1; 
            int iStartColumnIndex= endImageCol + 1;
            VCostMonthCompareReportSetColumn oVCostMonthCompareReportSetColumn = new VCostMonthCompareReportSetColumn(flexGrid, iStartRowIndex,iStartColumnIndex);
            oVCostMonthCompareReportSetColumn.ShowDialog();
            lstHideColumns.Clear();
            if (flexGrid != null && flexGrid.Cols > iStartColumnIndex && flexGrid.Rows > iStartRowIndex)
            {
                for (int i = iStartColumnIndex; i < flexGrid.Cols; i++)
                {
                    if (flexGrid.Column(i).Visible == false)
                    {
                        lstHideColumns.Add(new TempColumn() { ColumnName = flexGrid.Cell(iStartRowIndex, i).Text, index = i - iStartColumnIndex });
                    }
                }
            }
        }
        void btnQuery_Click(object sender, EventArgs e)
        {
            if (ClientUtil.ToString(this.cmbYear.Text) == "")
            {
                MessageBox.Show("请输入会计年！");
                return;
            }
            if (ClientUtil.ToString(this.cboFiscalMonth.Text) == "")
            {
                MessageBox.Show("请输入会计月！");
                return;
            }
            if (ClientUtil.ToString(this.txtAccountRootNode.Text) == "")
            {
                MessageBox.Show("请选择核算节点！");
                return;
            }
            FlashScreen.Show("正在统计月度成本核算报表...");
            try
            {
                //this.flexGrid.AutoRedraw = false;
                //InitalFlexHead(false);
                //Redraw();
                ////设置外观
                //CommonUtil.SetFlexGridFace(this.flexGrid);
                GetData();
                FlashScreen.Close();
            }
            catch (Exception e1)
            {
                FlashScreen.Close();
               System.Threading.Thread.Sleep(1);
                MessageBox.Show("统计月度成本核算分析套表出错！"+ExceptionUtil.ExceptionMessage(e1));
            }
            finally
            {
               
            }

        }
        void btnExcel_Click(object sender, EventArgs e)
        {
            flexGrid.ExportToExcel("月度成本分析对比报表", false, false, true);
        }
        void btSelectGWBS_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());

            if (txtAccountRootNode.Tag != null)
            {
                frm.DefaultSelectedGWBS = txtAccountRootNode.Tag as GWBSTree;
            }
            frm.IsCheck = false;//是否有checkbox

            frm.IsRootNode = true;//这个参数是否只选择隶属关系的根节点
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                IList taskList = new ArrayList();
                string taskStr = "";
                GWBSTree task = frm.SelectResult[0].Tag as GWBSTree;
                txtAccountRootNode.Text = task.Name;
                txtAccountRootNode.Tag = task;
            }
        }
        void InitalFlexHead(bool IsRedraw )
        {
            FlexCell.Range oRange = null;
            FlexCell.Cell oCell = null;
            FlexCell.Column oColumn=null;
            int iRow=0,iCol=0;
            flexGrid.AutoRedraw = false;
    
            #region 填充表头
            flexGrid.Range(0, 0, flexGrid.Rows - 1, flexGrid.Cols - 1).ClearAll();
            flexGrid.Rows = 5;
            flexGrid.Cols = this.endImageCol + 35 + 1;
            
          //  flexGrid.Column(0).Visible = 
            flexGrid.Row(0).Visible = false;
            for (int i = 1; i <= this.endImageCol; i++)
            {
                oColumn = flexGrid.Column(i); oColumn.Width = 20; oColumn.Alignment = FlexCell.AlignmentEnum.LeftCenter;
            }
                iRow = 1;
            iCol = 1;
           // oRange = flexGrid.Range(iRow, iCol, iRow, endImageCol); oRange.Merge();
            //iCol = endImageCol + 1;//表格标题
            oRange = flexGrid.Range(iRow, 1, iRow, flexGrid.Cols - 1); oRange.Merge();
            oCell = flexGrid.Cell(iRow, iCol); 
            oCell.Text = GetTitle(); 
            oCell.FontBold = true; 
            oCell.FontSize = 16; 
            oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            flexGrid.Row(iRow).Height = 40;
            iRow = 2;
            iCol = 1;//明细名称
            oRange = flexGrid.Range(iRow, iCol, iRow + 1, endImageCol); oRange.Merge();
            oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "明细名称"; oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += endImageCol;//工程成本维护情况1+=5
            oRange = flexGrid.Range(iRow, iCol, iRow, iCol + 2); oRange.Merge();
            oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "工程成本维护情况"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter; oCell.FontBold = true;
            iCol += 3;//本期实际成本6+=3
            oRange = flexGrid.Range(iRow, iCol, iRow, iCol + 5); oRange.Merge();
            oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "本期实际成本"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter; oCell.FontBold = true;
            iCol += 6;//本期责任成本
            oRange = flexGrid.Range(iRow, iCol, iRow, iCol +2); oRange.Merge();
            oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "本期责任成本"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter; oCell.FontBold = true;
            iCol += 3;//本期合同收入
            oRange = flexGrid.Range(iRow, iCol, iRow, iCol + 2); oRange.Merge();
            oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "本期合同收入"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter; oCell.FontBold = true;
            iCol += 3;//本期收支对比
            oRange = flexGrid.Range(iRow, iCol, iRow, iCol + 1); oRange.Merge();
            oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "本期收支对比"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter; oCell.FontBold = true;
            iCol += 2;//本期节超对比
            oRange = flexGrid.Range(iRow, iCol, iRow, iCol + 1); oRange.Merge();
            oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "本期节超对比"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter; oCell.FontBold = true;
            iCol += 2;//累计实际成本
            oRange = flexGrid.Range(iRow, iCol, iRow, iCol + 5); oRange.Merge();
            oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "累计实际成本"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter; oCell.FontBold = true;
            iCol += 6;//累计责任成本
            oRange = flexGrid.Range(iRow, iCol, iRow, iCol + 2); oRange.Merge();
            oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "累计责任成本"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter; oCell.FontBold = true;
            iCol += 3;//累计合同收入
            oRange = flexGrid.Range(iRow, iCol, iRow, iCol + 2); oRange.Merge();
            oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "累计合同收入"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter; oCell.FontBold = true;
            iCol += 3;//累计收支对比
            oRange = flexGrid.Range(iRow, iCol, iRow, iCol + 1); oRange.Merge();
            oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "累计收支对比"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter; oCell.FontBold = true;
            iCol += 2;//累计节超对比
            oRange = flexGrid.Range(iRow, iCol, iRow, iCol + 1); oRange.Merge();
            oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "累计节超对比"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter; oCell.FontBold = true;

            iRow = 3;
            iCol = endImageCol + 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "计划工程量(1)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "责任成本工程量(2)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "合同收入工程量(3)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "本期分包结算工程量(4)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "分包结算单价(5)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "本期实际成本均价(5.1=6/4)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "本期实际成本金额（含材料费等）(6)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "其中：本期额外支出金额(6.1)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "其中：本期材料费金额(6.2)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "本期责任工程量(7=4/1*2)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "责任单价(8)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "本期责任成本金额(9=7*8)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "本期合同收入量(10=4/1*3)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "合同收入单价(11)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "本期合同收入金额(12=10*11)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "本期利润(13=12-6)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "本期利润率(14=13/12)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "本期节超额(15=9-6)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "本期节超例(16=15/9)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "累计分包结算工程量(17)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "分包结算单价(5)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "累计实际成本平均单价(18)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "累计实际成本金额(19)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "其中：累计额外支出金额(6.1)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "其中：累计材料费金额(6.2)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "累计责任工程量(20=17/1*2)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "累计责任单价(8)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "累计责任成本金额(21=20*8)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "累计合同收入量(22=17/1*3)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "累计合同收入单价(11)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "累计合同收入金额(23=22*11)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "累计利润(24=23-19)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "累计利润率(25=24/23)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "累计节超额(26=21-19)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iCol += 1; oCell = flexGrid.Cell(iRow, iCol);
            oCell.Text = "累计节超例(27=26/21)"; //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            iRow += 1;
            flexGrid.Row(iRow).Visible = false;
            if (lstHideColumns != null && lstHideColumns.Count > 0)
            {
                string sCellText=string.Empty;
                for (int i = endImageCol + 1; i < flexGrid.Cols; i++)
                {

                    if (lstHideColumns.Exists(a => a.index + endImageCol+1==i))
                    {
                        flexGrid.Column(i).Visible = false;
                    }
                }

            }
            #endregion
            if (IsRedraw)
            {
                Redraw();
            }
            flexGrid.DisplayRowNumber = true;
            //flexGrid.FixedCols = iImgCol + 1;
           flexGrid.FixedRows = 4;

        }
       
        string GetTitle()
        {
            string sResult = string.Empty;
            sResult = "可追溯性对比表";
            return sResult;
        }
        public void Redraw()
        {
            for (int i = 1; i <= endImageCol; i++)
            {
                flexGrid.Column(i).Width = 20;

            }
            for (int i = endImageCol + 1; i < flexGrid.Cols; i++)
            {
                flexGrid.Column(i).AutoFit();
            }
           SetBk();
           for (int i = 5; i < flexGrid.Rows; i++)
           {
               flexGrid.Row(i).Locked = true;
           }
               flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }
        public void SetBk()
        {
            flexGrid.SelectionMode = FlexCell.SelectionModeEnum.ByCell;
            //flexGrid.ExtendLastCol = true;
            flexGrid.DisplayFocusRect = false;
            flexGrid.LockButton = true;
            flexGrid.ReadonlyFocusRect = FlexCell.FocusRectEnum.Solid;
            flexGrid.BorderStyle = FlexCell.BorderStyleEnum.FixedSingle;
            flexGrid.ScrollBars = FlexCell.ScrollBarsEnum.Both;
            flexGrid.BackColorBkg = SystemColors.Control;
            flexGrid.DefaultFont = new Font("Tahoma", 8);

            flexGrid.ForeColor=flexGrid.BackColor = flexGrid.BackColor1 = flexGrid.BackColor2 = System.Drawing.SystemColors.Control;
            FlexCell.Range oRange = flexGrid.Range(4, endImageCol + 1, flexGrid.Rows - 1, flexGrid.Cols - 1);

            oRange.Locked = true;
        }
        #region flex
        public void FlexGrid_Click(object sender, EventArgs e)
        {
            FlexCell.Cell oCell = flexGrid.ActiveCell;
            bool isOpened = false;
            string sCurrID = string.Empty;
            int iCol, iRow;
            if (oCell != null && !string.IsNullOrEmpty(oCell.ImageKey))
            {
                flexGrid.AutoRedraw = false;
                isOpened = oCell.ImageKey == imageCollapse;
                oCell.SetImage(isOpened ? imageExpand : imageCollapse);
                iCol =  oCell.Col;
                iRow = oCell.Row;
                sCurrID = flexGrid.Cell(iRow, 0).Tag;
                SetImage(sCurrID, isOpened, iRow + 1, iCol);
                flexGrid.AutoRedraw = true;
                flexGrid.Refresh();
            }
        }
        public void SetImage(string sParentID, bool isOpened, int iRow, int iCol)
        {
            Hashtable ht = new Hashtable();
            int iTotal = flexGrid.Rows;
            FlexCell.Cell oCell = null;
            FlexCell.Row oRow = null;
            string sCurID = string.Empty, sCurParentID = string.Empty;
            if (!string.IsNullOrEmpty(sParentID) && iTotal > iRow && flexGrid.Cols > iCol)
            {
                ht.Add(sParentID, "");
                for (int iStart = iRow; iStart < iTotal; iStart++)
                {
                    oRow = flexGrid.Row(iStart);
                    sCurID = flexGrid.Cell(iStart, 0).Tag;
                    sCurParentID = flexGrid.Cell(iStart, 1).Tag;
                    if (ht.ContainsKey(sCurParentID))
                    {
                        if (string.Equals(sCurParentID, sParentID))
                        {
                            oRow.Visible = !isOpened;
                            oCell = flexGrid.Cell(iStart, iCol + 1);
                            if (!string.IsNullOrEmpty(oCell.ImageKey))
                            {
                                flexGrid.Cell(iStart, iCol + 1).SetImage(imageExpand);
                            }
                        }
                        else 
                        {
                            oRow.Visible = false;
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
        public int FillFlex(TempGWBSTree oTempGWBSTree, int iStartRowIndex, int iColIndex)
        {
            int iColumn = 0;
            int iRowIndex = iStartRowIndex;
            FlexCell.Row oRow = null;
            FlexCell.Cell oCell = null, oCellImage = null;
            FlexCell.Range oRange = null;
            if (oTempGWBSTree != null)
            {
                this.flexGrid.InsertRow(iRowIndex, 1);
                oRow = this.flexGrid.Row(iRowIndex);
                oRow.Visible = iColIndex <= defaultCollapse + 1 ? true : false;
                if (startImageCol < iColIndex)
                {
                    oRange = flexGrid.Range(iRowIndex, startImageCol, iRowIndex, iColIndex - 1);
                    oRange.Merge();
                }
                if (oTempGWBSTree.Detail.Count > 0 || oTempGWBSTree.ChildNode.Count > 0)
                {
                    oCellImage = flexGrid.Cell(iRowIndex, iColIndex);
                    oCellImage.SetImage(iColIndex<=defaultCollapse? imageCollapse:imageExpand);
                    iColIndex += 1;
                }
               
                oRange = flexGrid.Range(iRowIndex, iColIndex, iRowIndex, endImageCol);
                oRange.Merge();
                #region 行中单元填充
                oCell = flexGrid.Cell(iRowIndex, iColIndex); oCell.Text ="★"+ oTempGWBSTree.Name; oCell.FontBold = true;
                oCell = flexGrid.Cell(iRowIndex, 0); oCell.Tag = oTempGWBSTree.ID;
                oCell = flexGrid.Cell(iRowIndex, 1); oCell.Tag = oTempGWBSTree.ParentID;
                iColumn = endImageCol;
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.PlanQty.ToString("N2");//计划工程量（1）
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.ResponsibilitilQty.ToString("N2");//责任成本工程量
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.ContractQty.ToString("N2");//合同收入工程量(3)
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.CurrSubContractQty.ToString("N2");//本期分包结算工程量（4）
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.CurrSubContractPrice.ToString("N2");//分包结算单价（5）
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.CurrRealPrice.ToString("N2");//本期实际成本均价（5.1）5.1=6/4
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.CurrRealTotalPrice.ToString("N2");//本期实际成本金额（含材料费等）（6）
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.CurrOtherOutPutTotalPrice.ToString("N2");//其中：本期额外支出金额（6.1）
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.CurrMaterialTotalPrice.ToString("N2");//其中：本期材料费金额（6.2）
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.CurrResponsibilitilQty.ToString("N2");//本期责任工程量（7）7=4/1*2
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.CurrResponsibilitilPrice.ToString("N2");//责任单价（8）
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.CurrResponsibilitilTotalPrice.ToString("N2");//本期责任成本金额（9）9=7*8
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.CurrContractQty.ToString("N2");//本期合同收入量（10）10=4/1*3
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.CurrContractPrice.ToString("N2");//合同收入单价（11）
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.CurrContractTotalPrice.ToString("N2");//本期合同收入金额（12）12=10*11
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.CurrProfit.ToString("N2");//本期利润(13)=合同收入金额-实际成本金额13=12-6
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.CurrProfitRate.ToString("N2");//本期利润率（14）=利润/合同收入金额14=13/12
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.CurrSave.ToString("N2");//本期节超额（15）=责任成本-实际成本15=9-6
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.CurrSaveRate.ToString("N2");//本期节超例（16）=节超额/责任成本16=15/9
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.SumSubContractQty.ToString("N2");//累计分包结算工程量（17）
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.SumSubContractPrice.ToString("N2");//分包结算单价（5）
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.SumRealPrice.ToString("N2");//累计实际成本平均单价（18）
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.SumRealTotalPrice.ToString("N2");//累计实际成本金额（19）
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.SumOtherPutOutTotalPrice.ToString("N2");//其中：累计额外支出金额（6.1）
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.SumMaterialTotalPrice.ToString("N2");//其中：累计材料费金额（6.2）
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.SumResponsibilitilQty.ToString("N2");//累计责任工程量（20）20=17/1*2
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.SumResponsibilitilPrice.ToString("N2");//累计责任单价（8）
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.SumResponsibilitilTotalPrice.ToString("N2");//累计责任成本金额（21）21=20*8
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.SumContractQty.ToString("N2");//累计合同收入量（22）22=17/1*3
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.SumContractPrice.ToString("N2");//累计合同收入单价（11）
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.SumContractTotalPrice.ToString("N2");//累计合同收入金额（23）23=22*11
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.SumProfit.ToString("N2");//累计利润(24)=累计合同收入金额-累计实际成本金额24=23-19
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.SumProfitRate.ToString("N2");//累计利润率（25）=累计利润/累计合同收入金额25=24/23
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.SumSave.ToString("N2");//累计节超额（26）=累计责任成本-累计实际成本26=21-19
                iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSTree.SumSaveRate.ToString("N2");//累计节超例（27）=累计节超额/累计责任成本27=26/21
                #endregion
                iRowIndex += 1;

                foreach (TempGWBSTree child in oTempGWBSTree.ChildNode.OrderBy(a => a.OrderNo))
                {
                    iRowIndex = FillFlex(child, iRowIndex, iColIndex);
                }
                foreach (TempGWBSDetail oTempGWBSDetail in oTempGWBSTree.Detail.OrderBy(a => a.OrderNo))
                {
                    flexGrid.InsertRow(iRowIndex, 1);
                    oRow = flexGrid.Row(iRowIndex);
                    oRow.Visible = iColIndex <= defaultCollapse + 1 ? true : false;
                    if (startImageCol < iColIndex)
                    {
                        oRange = flexGrid.Range(iRowIndex, startImageCol, iRowIndex, iColIndex - 1);
                        oRange.Merge();
                    }
                    oRange = flexGrid.Range(iRowIndex, iColIndex, iRowIndex,  endImageCol);
                    oRange.Merge();
                    #region 行中单元填充
                    oCell = flexGrid.Cell(iRowIndex, iColIndex); oCell.Text = oTempGWBSDetail.Name; oCell.FontBold = false;  ;
                    oCell = flexGrid.Cell(iRowIndex, 0); oCell.Tag = oTempGWBSDetail.ID;
                    oCell = flexGrid.Cell(iRowIndex,1); oCell.Tag = oTempGWBSDetail.ParentID;
                    iColumn = endImageCol;
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.PlanQty.ToString("N2");//计划工程量（1）
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.ResponsibilitilQty.ToString("N2");//责任成本工程量
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.ContractQty.ToString("N2");//合同收入工程量(3)
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.CurrSubContractQty.ToString("N2");//本期分包结算工程量（4）
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.CurrSubContractPrice.ToString("N2");//分包结算单价（5）
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.CurrRealPrice.ToString("N2");//本期实际成本均价（5.1）5.1=6/4
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.CurrRealTotalPrice.ToString("N2");//本期实际成本金额（含材料费等）（6）
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.CurrOtherOutPutTotalPrice.ToString("N2");//其中：本期额外支出金额（6.1）
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.CurrMaterialTotalPrice.ToString("N2");//其中：本期材料费金额（6.2）
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.CurrResponsibilitilQty.ToString("N2");//本期责任工程量（7）7=4/1*2
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.CurrResponsibilitilPrice.ToString("N2");//责任单价（8）
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.CurrResponsibilitilTotalPrice.ToString("N2");//本期责任成本金额（9）9=7*8
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.CurrContractQty.ToString("N2");//本期合同收入量（10）10=4/1*3
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.CurrContractPrice.ToString("N2");//合同收入单价（11）
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.CurrContractTotalPrice.ToString("N2");//本期合同收入金额（12）12=10*11
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.CurrProfit.ToString("N2");//本期利润(13)=合同收入金额-实际成本金额13=12-6
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.CurrProfitRate.ToString("N2");//本期利润率（14）=利润/合同收入金额14=13/12
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.CurrSave.ToString("N2");//本期节超额（15）=责任成本-实际成本15=9-6
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.CurrSaveRate.ToString("N2");//本期节超例（16）=节超额/责任成本16=15/9
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.SumSubContractQty.ToString("N2");//累计分包结算工程量（17）
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.SumSubContractPrice.ToString("N2");//分包结算单价（5）
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.SumRealPrice.ToString("N2");//累计实际成本平均单价（18）
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.SumRealTotalPrice.ToString("N2");//累计实际成本金额（19）
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.SumOtherPutOutTotalPrice.ToString("N2");//其中：累计额外支出金额（6.1）
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.SumMaterialTotalPrice.ToString("N2");//其中：累计材料费金额（6.2）
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.SumResponsibilitilQty.ToString("N2");//累计责任工程量（20）20=17/1*2
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.SumResponsibilitilPrice.ToString("N2");//累计责任单价（8）
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.SumResponsibilitilTotalPrice.ToString("N2");//累计责任成本金额（21）21=20*8
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.SumContractQty.ToString("N2");//累计合同收入量（22）22=17/1*3
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.SumContractPrice.ToString("N2");//累计合同收入单价（11）
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.SumContractTotalPrice.ToString("N2");//累计合同收入金额（23）23=22*11
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.SumProfit.ToString("N2");//累计利润(24)=累计合同收入金额-累计实际成本金额24=23-19
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.SumProfitRate.ToString("N2");//累计利润率（25）=累计利润/累计合同收入金额25=24/23
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.SumSave.ToString("N2");//累计节超额（26）=累计责任成本-累计实际成本26=21-19
                    iColumn += 1; oCell = flexGrid.Cell(iRowIndex, iColumn); oCell.Text = oTempGWBSDetail.SumSaveRate.ToString("N2");//累计节超例（27）=累计节超额/累计责任成本27=26/21
                
                    #endregion
                    iRowIndex += 1;
                }
                if (oCellImage != null)
                {
                    oCellImage.Tag = (iRowIndex - iStartRowIndex - 1).ToString();
                    oCellImage.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                }
            }
            return iRowIndex;
        }
        #endregion
        #region 将分包结算单汇总到月度成本上
        public void GetData()
        {
            int iLevel = defaultImageCol;
            int iYear = ClientUtil.ToInt(cmbYear.SelectedItem);
            int iMonth = ClientUtil.ToInt(cboFiscalMonth.SelectedItem);
            string sProjectId = ProjectInfo.Id;
            string sGWBSTreeID = (txtAccountRootNode.Tag as GWBSTree).Id;
            IList  lstResult = model.CostMonthAccSrv.QueryCostCompareReportData(sProjectId, sGWBSTreeID, iYear, iMonth);
            TempGWBSTree oTempGWBSTree = GetTempGWBSTreeRoot(lstResult,ref iLevel);
            if (oTempGWBSTree != null)
            {
                endImageCol = iLevel - oTempGWBSTree.TLevel+2;
                if (endImageCol < defaultImageCol)
                {
                    endImageCol = defaultImageCol;
                }
            }
            flexGrid.AutoRedraw = false;
            InitalFlexHead(false);
            

            FillFlex(oTempGWBSTree,startRow,startImageCol);
            Redraw();
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }
       
        public TempGWBSTree GetTempGWBSTreeRoot(IList lstResult,ref int iLevel)
        {
         
            DataTable oCostMonthTable=null, oSubTable=null;
            Hashtable htGWBSTree = new Hashtable();//key：工程wbs ID value：TempGWBSTree
            Hashtable htGWBSDetail = new Hashtable();
            IList<TempGWBSTree> lstTempGWBSTree = new List<TempGWBSTree>();
            IList<TempGWBSDetail> lstTempGWBSDetail = new List<TempGWBSDetail>();
            IList<TempGWBSConsume> lstTempGWBSConsume = new List<TempGWBSConsume>();
            //tt.syscode,tt.name gwbsTreeName, tt1.name dtlName tt.id,tt1.id dtlid,
            string sParentID = string.Empty;
            string sGWBSTreeID = string.Empty;
            string sGWBSDetailID = string.Empty;
            string sConsumeID = string.Empty;
            TempGWBSTree oTempGWBSTree, oTempGWBSTreeParent;
            TempGWBSDetail oTempGWBSDetail = null;
            TempGWBSConsume oTempGWBSConsume = null;
            if (lstResult != null)
            {
                oCostMonthTable = lstResult.Count > 0 ? (lstResult[0] as DataSet).Tables[0] : null;
                oSubTable = lstResult.Count == 2 ? (lstResult[1] as DataSet).Tables[0] : null;
            }
            if (oCostMonthTable != null)
            {
                #region 构造树
                foreach (DataRow oRow in oCostMonthTable.Rows)
                {
                    sGWBSTreeID = ClientUtil.ToString(oRow["id"]);
                    sParentID = ClientUtil.ToString(oRow["parentid"]);
                    if (htGWBSTree.Contains(sGWBSTreeID))
                    {
                        oTempGWBSTree = htGWBSTree[sGWBSTreeID] as TempGWBSTree;
                    }
                    else
                    {//tt.syscode,tt.id,tt1.id dtlid,tt.name gwbsTreeName, tt1.name dtlName,tt.tlevel
                        oTempGWBSTree = new TempGWBSTree()
                        {
                            ID = sGWBSTreeID,
                            Name = ClientUtil.ToString(oRow["gwbsTreeName"]),
                            SysCode = ClientUtil.ToString(oRow["syscode"]),
                            TLevel = ClientUtil.ToInt(oRow["tlevel"]),
                            ParentID = sParentID,
                            OrderNo = ClientUtil.ToInt(oRow["treeOrderNo"])
                        };
                        htGWBSTree.Add(sGWBSTreeID, oTempGWBSTree);
                        if (htGWBSTree.Contains(sParentID))
                        {
                            oTempGWBSTreeParent = htGWBSTree[sParentID] as TempGWBSTree;
                            oTempGWBSTreeParent.ChildNode.Add(oTempGWBSTree);
                            oTempGWBSTree.ParentNode = oTempGWBSTreeParent;
                        }
                        lstTempGWBSTree.Insert(lstTempGWBSTree.Count, oTempGWBSTree);
                        if (iLevel < oTempGWBSTree.TLevel)
                        {
                            iLevel = oTempGWBSTree.TLevel;
                        }
                    }
                    sGWBSDetailID = ClientUtil.ToString(oRow["dtlid"]);
                    if (!string.IsNullOrEmpty(sGWBSDetailID))
                    {
                        if (htGWBSDetail.Contains(sGWBSDetailID))
                        {
                            oTempGWBSDetail = htGWBSDetail[sGWBSDetailID] as TempGWBSDetail;
                        }
                        else
                        {
                            oTempGWBSDetail = new TempGWBSDetail()
                            {
                                ID = sGWBSDetailID,
                                Parent = oTempGWBSTree,
                                ParentID = sGWBSTreeID,
                                PlanQty = ClientUtil.ToDecimal(oRow["dudgetplanquantity"]),
                                ContractQty = ClientUtil.ToDecimal(oRow["dudgetcontractquantity"]),
                                ResponsibilitilQty = ClientUtil.ToDecimal(oRow["dudgetrespquantity"]),
                                SumResponsibilitilPrice = ClientUtil.ToDecimal(oRow["responsibilitilyprice"]),
                                CurrResponsibilitilPrice = ClientUtil.ToDecimal(oRow["responsibilitilyprice"]),
                                SumContractPrice = ClientUtil.ToDecimal(oRow["contractprice"]),
                                CurrContractPrice = ClientUtil.ToDecimal(oRow["contractprice"]),
                                Name = ClientUtil.ToString(oRow["dtlName"]),
                                OrderNo = ClientUtil.ToInt(oRow["detailOrderNo"])
                            };
                            htGWBSDetail.Add(sGWBSDetailID, oTempGWBSDetail);

                            oTempGWBSTree.Detail.Add(oTempGWBSDetail);
                            lstTempGWBSDetail.Insert(lstTempGWBSDetail.Count, oTempGWBSDetail);
                        }
                    }
                    sConsumeID = ClientUtil.ToString(oRow["consumeID"]);
                    if (!string.IsNullOrEmpty(sConsumeID))
                    {
                        oTempGWBSConsume = new TempGWBSConsume()
                        {
                            ID = sConsumeID,
                            Parent = oTempGWBSDetail,
                            ResourceTypeGuid = ClientUtil.ToString(oRow["resourcetypeguid"]),
                            SubjectGuid = ClientUtil.ToString(oRow["costingsubjectguid"]),
                            CurrRealTotalPrice = ClientUtil.ToDecimal(oRow["currrealconsumetotalprice"]),
                            CurrMaterialTotalPrice = ClientUtil.ToDecimal(oRow["currmaterialTotalPrice"]),
                           // CurrContractQty = ClientUtil.ToDecimal(oRow["currincomequantity"]),
                            CurrContractTotalPrice = ClientUtil.ToDecimal(oRow["currincometotalprice"]),
                            //CurrResponsibilitilQty = ClientUtil.ToDecimal(oRow["currresponsiconsumequantity"]),
                            CurrResponsibilitilTotalPrice = ClientUtil.ToDecimal(oRow["currresponsiconsumetotalprice"]),
                            SumRealTotalPrice = ClientUtil.ToDecimal(oRow["sumrealconsumetotalprice"]),
                            SumMaterialTotalPrice = ClientUtil.ToDecimal(oRow["sumrealconsumetotalprice"]),
                            //SumContractQty = ClientUtil.ToDecimal(oRow["sumincomequantity"]),
                            SumContractTotalPrice = ClientUtil.ToDecimal(oRow["summaterialTotalPrice"]),
                            //SumResponsibilitilQty = ClientUtil.ToDecimal(oRow["sumresponsiconsumequantity"]),
                            SumResponsibilitilTotalPrice = ClientUtil.ToDecimal(oRow["sumresponsiconsumetotalprice"])
                        };
                        oTempGWBSDetail.Detail.Add(oTempGWBSConsume);
                        lstTempGWBSConsume.Add(oTempGWBSConsume);
                    }
                }
                #endregion
                #region 将分包结算汇总到月度成本耗用上
                CalSubToCostMonth(lstTempGWBSConsume, oSubTable);
                #endregion
            }
             oTempGWBSTree = lstTempGWBSTree.Count > 0 ? lstTempGWBSTree[0] : null;
            if (oTempGWBSTree != null)
            {
                oTempGWBSTree.Calculation();
            }
            return oTempGWBSTree;
        }
        private void  CalSubToCostMonth(IList<TempGWBSConsume> lstTempGWBSConsume, DataTable oSubTable)
        {
            string sTempGWBSTreeID, sTempGWBSDetailID, sSysCode, sSubjectID,sResuourceTypeID;
            DataRow oRow;
            TempGWBSConsume oTempGWBSConsume = null;
            IEnumerable<TempGWBSConsume> lstTemp, lstTemp1;
            //t2.balancetaskguid gwbtreeid,t2.balancetaskdtlguid dtlid,t2.balancetasksyscode syscode,t3.resourcetypeguid,t3.balancesubjectname,t3.balancesubjectguid,
            //curTotalPrice curTotalQty curOtherTotalPrice sumTotalPrice sumQty sumOtherTotalPrice
            if (lstTempGWBSConsume != null && lstTempGWBSConsume.Count > 0 && oSubTable!=null &&oSubTable.Rows.Count>0)
            {
                for (int i=0;i<oSubTable.Rows.Count; i++)
                {
                    oRow=oSubTable.Rows[i];
                    sTempGWBSTreeID = ClientUtil.ToString(oRow["gwbtreeid"]);
                    sTempGWBSDetailID = ClientUtil.ToString(oRow["dtlid"]);
                    sSysCode = ClientUtil.ToString(oRow["syscode"]);
                    sResuourceTypeID = ClientUtil.ToString(oRow["resourcetypeguid"]);
                    sSubjectID = ClientUtil.ToString(oRow["balancesubjectguid"]);
                    oTempGWBSConsume = null;

                    //用资源和核算科目进行过滤
                    lstTemp = lstTempGWBSConsume.Where(a=>string.Equals(a.SubjectGuid, sSubjectID) && string.Equals(a.ResourceTypeGuid, sResuourceTypeID));
                    if (lstTemp != null && lstTemp.Count() > 0)
                    {
                        if (!string.IsNullOrEmpty(sTempGWBSDetailID))
                        {
                            //根据任务ID 找到该节点的本节点或者最近父节点
                            lstTemp1 = lstTemp.Where(a => string.Equals(a.Parent.ID, sTempGWBSDetailID) && sSysCode.Contains(a.Parent.Parent.SysCode)).OrderByDescending(a => a.Parent.Parent.SysCode.Length);
                            if (lstTemp1 != null && lstTemp1.Count() > 0)
                            {
                                oTempGWBSConsume = lstTemp1.ElementAt(0);
                            }
                        }
                        if (oTempGWBSConsume == null)
                        { //  找到该节点的本节点或者最近父节点
                            lstTemp1 = lstTemp.Where(a => sSysCode.Contains(a.Parent.Parent.SysCode)).OrderByDescending(a => a.Parent.Parent.SysCode.Length);
                            if (lstTemp1 != null && lstTemp1.Count() > 0)
                            {
                                oTempGWBSConsume = lstTemp1.ElementAt(0);
                            }
                        }
                        if (oTempGWBSConsume == null && !string.IsNullOrEmpty(sTempGWBSDetailID))
                        {
                            //  找到该节点的最近子节点
                            lstTemp1 = lstTemp.Where(a => string.Equals(a.Parent.ID, sTempGWBSDetailID) && a.Parent.Parent.SysCode.Contains(sSysCode)).OrderByDescending(a => a.Parent.Parent.SysCode.Length);
                            if (lstTemp1 != null && lstTemp1.Count() > 0)
                            {
                                oTempGWBSConsume = lstTemp1.ElementAt(0);
                            }
                        }
                        if (oTempGWBSConsume == null)
                        {
                            lstTemp1 = lstTemp.Where(a => a.Parent.Parent.SysCode.Contains(sSysCode)).OrderByDescending(a => a.Parent.Parent.SysCode.Length);
                            if (lstTemp1 != null && lstTemp1.Count() > 0)
                            {
                                oTempGWBSConsume = lstTemp1.ElementAt(0);
                            }
                        }
                        if (oTempGWBSConsume != null)
                        {
                            oTempGWBSConsume.CurrOtherOutPutTotalPrice += ClientUtil.ToDecimal(oRow["curOtherTotalPrice"]);
                            oTempGWBSConsume.CurrSubContractQty += ClientUtil.ToDecimal(oRow["curTotalQty"]);
                            oTempGWBSConsume.CurrSubContractTotalPrice += ClientUtil.ToDecimal(oRow["curTotalPrice"]);
                            oTempGWBSConsume.SumOtherPutOutTotalPrice += ClientUtil.ToDecimal(oRow["sumOtherTotalPrice"]);
                            oTempGWBSConsume.SumSubContractQty += ClientUtil.ToDecimal(oRow["sumQty"]);
                            oTempGWBSConsume.SumSubContractTotalPrice += ClientUtil.ToDecimal(oRow["sumTotalPrice"]);
                        }
                    }
                }
            }
        }
        #endregion
    }
    public class TempGWBSTree 
    {
        public int TLevel { get; set; }
        public string ID{get;set;}
        public string SysCode{get;set;}
        public string Name{get;set;}
        public string ParentID;
        public TempGWBSTree ParentNode;
        IList<TempGWBSTree> childNode = new List<TempGWBSTree>();
        public IList<TempGWBSTree> ChildNode { get { return childNode; } }
        IList<TempGWBSDetail> detail = new List<TempGWBSDetail>();
        public IList<TempGWBSDetail> Detail { get { return detail; } }
        public int OrderNo { get; set; }
        #region
        /// <summary>
        /// 计划工程量
        /// </summary>
        public decimal PlanQty { get; set; }
        /// <summary>
        /// 责任成本工程量
        /// </summary>
        public decimal ResponsibilitilQty { get; set; }
        /// <summary>
        /// 合同收入工程量
        /// </summary>
        public decimal ContractQty { get; set; }
        /// <summary>
        /// 本期分包结算工程量
        /// </summary>
        public decimal CurrSubContractQty { get; set; }
        /// <summary>
        /// 本期分包结算工程合价
        /// </summary>
        public decimal CurrSubContractTotalPrice { get; set; }
        /// <summary>
        /// 分包结算单价
        /// </summary>
        public decimal CurrSubContractPrice { get { return CurrSubContractQty == 0 ? 0 : Math.Round(CurrSubContractTotalPrice / CurrSubContractQty, 2); } }
        /// <summary>
        /// 本期实际成本均价
        /// </summary>
        public decimal CurrRealPrice { get { return CurrSubContractQty == 0 ? 0 : Math.Round(CurrRealTotalPrice / CurrSubContractQty, 2); } }
        /// <summary>
        /// 本期实际成本金额（含材料费等）
        /// </summary>
        public decimal CurrRealTotalPrice { get; set; }
        /// <summary>
        /// 其中：本期额外支出金额
        /// </summary>
        public decimal CurrOtherOutPutTotalPrice { get; set; }
        /// <summary>
        /// 其中：本期材料费金额
        /// </summary>
        public decimal CurrMaterialTotalPrice { get; set; }
        /// <summary>
        /// 本期责任工程量 本期责任工程量（7）7=本期分包结算工程量/计划工程量*责任成本工程量
        /// </summary>
        public decimal CurrResponsibilitilQty { get; set; }
        /// <summary>
        /// 责任单价
        /// </summary>
        public decimal CurrResponsibilitilPrice { get { return CurrResponsibilitilQty == 0 ? 0 : Math.Round(CurrResponsibilitilTotalPrice / CurrResponsibilitilQty, 2); } }
        /// <summary>
        /// 责任金额
        /// </summary>
        public decimal CurrResponsibilitilTotalPrice { get; set; }
        /// <summary>
        /// 本期合同量 本期合同收入量（10）10=本期分包结算工程量/计划工程量*合同收入工程量
        /// </summary>
        public decimal CurrContractQty { get; set; }
        /// <summary>
        /// 合同单价
        /// </summary>
        public decimal CurrContractPrice { get { return CurrContractQty == 0 ? 0 : CurrContractQty == 0 ? 0 : Math.Round(CurrContractTotalPrice / CurrContractQty, 2); } }
        /// <summary>
        /// 合同金额
        /// </summary>
        public decimal CurrContractTotalPrice { get; set; }
        /// <summary>
        /// 本期利润 合同收入金额-实际成本金额13=12-6
        /// </summary>
        public decimal CurrProfit { get { return this.CurrContractTotalPrice - this.CurrRealTotalPrice; } }
        /// <summary>
        /// 本期利润率 本期利润率（14）=利润/合同收入金额14=13/12
        /// </summary>
        public decimal CurrProfitRate { get { return CurrContractTotalPrice == 0 ? 0 : Math.Round(CurrProfit * 100 / CurrContractTotalPrice, 2); } }
        /// <summary>
        /// 本期节超额  本期节超额（15）=责任成本-实际成本15=9-6
        /// </summary>
        public decimal CurrSave { get { return this.CurrResponsibilitilTotalPrice - this.CurrRealTotalPrice; } }
        /// <summary>
        /// 本期节超额比例  本期节超例（16）=节超额/责任成本16=15/9
        /// </summary>
        public decimal CurrSaveRate { get { return CurrResponsibilitilTotalPrice == 0 ? 0 : Math.Round(CurrSave * 100 / CurrResponsibilitilTotalPrice, 2); } }
        /// <summary>
        /// 累计分包结算工程量
        /// </summary>
        public decimal SumSubContractQty { get; set; }
        /// <summary>
        /// 累计分包结算单价
        /// </summary>
        public decimal SumSubContractPrice { get { return SumSubContractQty == 0 ? 0 : Math.Round(SumSubContractTotalPrice / SumSubContractQty, 2); } }
        /// <summary>
        /// 累计分包结算金额 
        /// </summary>
        public decimal SumSubContractTotalPrice { get; set; }
        /// <summary>
        /// 累计实际成本平均单价
        /// </summary>
        public decimal SumRealPrice { get { return SumSubContractQty == 0 ? 0 : Math.Round(SumSubContractTotalPrice / SumSubContractQty, 2); } }
        /// <summary>
        /// 累计实际成本金额
        /// </summary>
        public decimal SumRealTotalPrice { get; set; }
        /// <summary>
        /// 其中：累计额外支出金额
        /// </summary>
        public decimal SumOtherPutOutTotalPrice { get; set; }
        /// <summary>
        /// 其中：累计材料费金额
        /// </summary>
        public decimal SumMaterialTotalPrice { get; set; }
        /// <summary>
        /// 累计责任工程量  累计责任工程量（20）20=17/1*2
        /// </summary>
        public decimal SumResponsibilitilQty { get; set; }
        /// <summary>
        /// 累计责任工程价格
        /// </summary>
        public decimal SumResponsibilitilPrice { get { return SumResponsibilitilQty == 0 ? 0 : Math.Round(SumResponsibilitilTotalPrice / SumResponsibilitilQty, 2); } }
        /// <summary>
        /// 累计责任成本金额
        /// </summary>
        public decimal SumResponsibilitilTotalPrice { get; set; }
        /// <summary>
        /// 累计合同工程量  累计合同收入量（22）22=17/1*3
        /// </summary>
        public decimal SumContractQty { get; set; }
        /// <summary>
        /// 累计合同工程价格
        /// </summary>
        public decimal SumContractPrice { get { return SumContractQty == 0 ? 0 : Math.Round(SumContractTotalPrice / SumContractQty,2); } }
        /// <summary>
        /// 累计合同成本金额
        /// </summary>
        public decimal SumContractTotalPrice { get; set; }
        /// <summary>
        /// 累计利润 累计利润(24)=累计合同收入金额-累计实际成本金额24=23-19
        /// </summary>
        public decimal SumProfit { get { return SumContractTotalPrice - SumRealTotalPrice; } }
        /// <summary>
        /// 累计利润率 累计利润率（25）=累计利润/累计合同收入金额25=24/23
        /// </summary>
        public decimal SumProfitRate { get { return SumContractTotalPrice == 0 ? 0 : Math.Round(SumProfit * 100 / SumContractTotalPrice, 2); } }
        /// <summary>
        /// 累计节超额  累计节超额（26）=累计责任成本-累计实际成本26=21-19
        /// </summary>
        public decimal SumSave { get { return SumResponsibilitilTotalPrice - SumRealTotalPrice; } }
        /// <summary>
        /// 累计节超额比例  累计节超例（27）=累计节超额/累计责任成本27=26/21
        /// </summary>
        public decimal SumSaveRate { get { return SumResponsibilitilTotalPrice == 0 ? 0 : Math.Round(SumSave * 100 / SumResponsibilitilTotalPrice, 2); } }

        #endregion
        public void Calculation()
        {
            #region 计算子节点
            if (ChildNode != null)
            {
                foreach (TempGWBSTree oTempGWBSTree in ChildNode)
                {
                    oTempGWBSTree.Calculation();
                    this.PlanQty += oTempGWBSTree.PlanQty;
                    this.ResponsibilitilQty += oTempGWBSTree.ResponsibilitilQty;
                    this.ContractQty += oTempGWBSTree.ContractQty;
                    this.CurrContractQty += oTempGWBSTree.CurrContractQty;
                    this.CurrContractTotalPrice += oTempGWBSTree.CurrContractTotalPrice;
                    this.CurrMaterialTotalPrice += oTempGWBSTree.CurrMaterialTotalPrice;
                    this.CurrOtherOutPutTotalPrice += oTempGWBSTree.CurrOtherOutPutTotalPrice;//
                    this.CurrRealTotalPrice += oTempGWBSTree.CurrRealTotalPrice;
                    this.CurrResponsibilitilQty += oTempGWBSTree.CurrResponsibilitilQty;
                    this.CurrResponsibilitilTotalPrice += oTempGWBSTree.CurrResponsibilitilTotalPrice;
                    this.CurrSubContractQty += oTempGWBSTree.CurrSubContractQty;//
                    this.CurrSubContractTotalPrice += oTempGWBSTree.CurrSubContractTotalPrice;//
                    this.SumContractQty += oTempGWBSTree.SumContractQty;
                    this.SumContractTotalPrice += oTempGWBSTree.SumContractTotalPrice;
                    this.SumMaterialTotalPrice += oTempGWBSTree.SumMaterialTotalPrice;
                    this.SumOtherPutOutTotalPrice += oTempGWBSTree.SumOtherPutOutTotalPrice;//
                    this.SumRealTotalPrice += oTempGWBSTree.SumRealTotalPrice;
                    this.SumResponsibilitilQty += oTempGWBSTree.SumResponsibilitilQty;
                    this.SumResponsibilitilTotalPrice += oTempGWBSTree.SumResponsibilitilTotalPrice;
                    this.SumSubContractQty += oTempGWBSTree.SumSubContractQty;//
                    this.SumSubContractTotalPrice += oTempGWBSTree.SumSubContractTotalPrice;
                }
            }
            #endregion
            #region 计算任务明细
            if (this.Detail != null)
            {
                foreach (TempGWBSDetail oDetail in Detail)
                {
                    oDetail.Calculation();
                    this.PlanQty += oDetail.PlanQty;
                    this.ResponsibilitilQty += oDetail.ResponsibilitilQty;
                    this.ContractQty += oDetail.ContractQty;
                    this.CurrContractQty += oDetail.CurrContractQty;
                    this.CurrContractTotalPrice += oDetail.CurrContractTotalPrice;
                    this.CurrMaterialTotalPrice += oDetail.CurrMaterialTotalPrice;
                    this.CurrOtherOutPutTotalPrice += oDetail.CurrOtherOutPutTotalPrice;
                    this.CurrRealTotalPrice += oDetail.CurrRealTotalPrice;
                    this.CurrResponsibilitilQty += oDetail.CurrResponsibilitilQty;
                    this.CurrResponsibilitilTotalPrice += oDetail.CurrResponsibilitilTotalPrice;
                    this.CurrSubContractQty += oDetail.CurrSubContractQty;
                    this.CurrSubContractTotalPrice += oDetail.CurrSubContractTotalPrice;
                    this.SumContractQty += oDetail.SumContractQty;
                    this.SumContractTotalPrice += oDetail.SumContractTotalPrice;
                    this.SumMaterialTotalPrice += oDetail.SumMaterialTotalPrice;
                    this.SumOtherPutOutTotalPrice += oDetail.SumOtherPutOutTotalPrice;
                    this.SumRealTotalPrice += oDetail.SumRealTotalPrice;
                    this.SumResponsibilitilQty += oDetail.SumResponsibilitilQty;
                    this.SumResponsibilitilTotalPrice += oDetail.SumResponsibilitilTotalPrice;
                    this.SumSubContractQty += oDetail.SumSubContractQty;
                    this.SumSubContractTotalPrice += oDetail.SumSubContractTotalPrice;
                }
            }
            #endregion
        }
    }
    public class TempGWBSDetail  
    {
        public string ID { get; set; }
        public string SysCode { get; set; }
        public string Name { get; set; }
         public string ParentID;
        /// <summary>
        /// 工程wbs
        /// </summary>
        public TempGWBSTree Parent;
        IList<TempGWBSConsume> detail = new List<TempGWBSConsume>();
        /// <summary>
        /// 消耗明细
        /// </summary>
        public IList<TempGWBSConsume> Detail { get { return detail; } }
        public int OrderNo { get; set; }
        #region
        /// <summary>
        /// 计划工程量
        /// </summary>
        public decimal PlanQty { get; set; }
        /// <summary>
        /// 责任成本工程量
        /// </summary>
        public decimal ResponsibilitilQty { get; set; }
        /// <summary>
        /// 合同收入工程量
        /// </summary>
        public decimal ContractQty { get; set; }
        /// <summary>
        /// 本期分包结算工程量
        /// </summary>
        public decimal CurrSubContractQty { get; set; }
        /// <summary>
        /// 本期分包结算工程合价
        /// </summary>
        public decimal CurrSubContractTotalPrice { get; set; }
        /// <summary>
        /// 分包结算单价
        /// </summary>
        public decimal CurrSubContractPrice { get { return CurrSubContractQty == 0 ? 0 : Math.Round(CurrSubContractTotalPrice / CurrSubContractQty, 2); } }
        /// <summary>
        /// 本期实际成本均价
        /// </summary>
        public decimal CurrRealPrice { get { return CurrSubContractQty == 0 ? 0 : Math.Round(CurrRealTotalPrice / CurrSubContractQty, 2); } }
        /// <summary>
        /// 本期实际成本金额（含材料费等）
        /// </summary>
        public decimal CurrRealTotalPrice { get; set; }
        /// <summary>
        /// 其中：本期额外支出金额
        /// </summary>
        public decimal CurrOtherOutPutTotalPrice { get; set; }
        /// <summary>
        /// 其中：本期材料费金额
        /// </summary>
        public decimal CurrMaterialTotalPrice { get; set; }
        /// <summary>
        /// 本期责任工程量 本期责任工程量（7）7=本期分包结算工程量/计划工程量*责任成本工程量
        /// </summary>
        public decimal CurrResponsibilitilQty { get { return PlanQty == 0 ? 0 : Math.Round((CurrSubContractQty * ResponsibilitilQty / PlanQty), 2); } }
        /// <summary>
        /// 责任单价
        /// </summary>
        public decimal CurrResponsibilitilPrice { get; set; }
        /// <summary>
        /// 责任金额
        /// </summary>
        public decimal CurrResponsibilitilTotalPrice { get { return CurrResponsibilitilQty * CurrResponsibilitilPrice; } }
        /// <summary>
        /// 本期合同量 本期合同收入量（10）10=本期分包结算工程量/计划工程量*合同收入工程量
        /// </summary>
        public decimal CurrContractQty { get { return this.PlanQty == 0 ? 0 : Math.Round(this.CurrSubContractQty * this.ContractQty / this.PlanQty); } }
        /// <summary>
        /// 合同单价
        /// </summary>
        public decimal CurrContractPrice { get; set; }
        /// <summary>
        /// 合同金额
        /// </summary>
        public decimal CurrContractTotalPrice { get { return CurrContractQty * CurrContractPrice; } }
        /// <summary>
        /// 本期利润 合同收入金额-实际成本金额13=12-6
        /// </summary>
        public decimal CurrProfit { get { return this.CurrContractTotalPrice - this.CurrRealTotalPrice; } }
        /// <summary>
        /// 本期利润率 本期利润率（14）=利润/合同收入金额14=13/12
        /// </summary>
        public decimal CurrProfitRate { get { return CurrContractTotalPrice == 0 ? 0 : Math.Round(CurrProfit * 100 / CurrContractTotalPrice, 2); } }
        /// <summary>
        /// 本期节超额  本期节超额（15）=责任成本-实际成本15=9-6
        /// </summary>
        public decimal CurrSave { get { return this.CurrResponsibilitilTotalPrice - this.CurrRealTotalPrice; } }
        /// <summary>
        /// 本期节超额比例  本期节超例（16）=节超额/责任成本16=15/9
        /// </summary>
        public decimal CurrSaveRate { get { return CurrResponsibilitilTotalPrice == 0 ? 0 : Math.Round(CurrSave * 100 / CurrResponsibilitilTotalPrice, 2); } }
        /// <summary>
        /// 累计分包结算工程量
        /// </summary>
        public decimal SumSubContractQty { get; set; }
        /// <summary>
        /// 累计分包结算单价
        /// </summary>
        public decimal SumSubContractPrice { get { return SumSubContractQty == 0 ? 0 : Math.Round(SumSubContractTotalPrice / SumSubContractQty, 2); } }
        /// <summary>
        /// 累计分包结算金额 
        /// </summary>
        public decimal SumSubContractTotalPrice { get; set; }
        /// <summary>
        /// 累计实际成本平均单价
        /// </summary>
        public decimal SumRealPrice { get { return SumSubContractQty == 0 ? 0 : Math.Round(SumSubContractTotalPrice / SumSubContractQty, 2); } }
        /// <summary>
        /// 累计实际成本金额
        /// </summary>
        public decimal SumRealTotalPrice { get; set; }
        /// <summary>
        /// 其中：累计额外支出金额
        /// </summary>
        public decimal SumOtherPutOutTotalPrice { get; set; }
        /// <summary>
        /// 其中：累计材料费金额
        /// </summary>
        public decimal SumMaterialTotalPrice { get; set; }
        /// <summary>
        /// 累计责任工程量  累计责任工程量（20）20=17/1*2
        /// </summary>
        public decimal SumResponsibilitilQty { get { return PlanQty == 0 ? 0 : Math.Round(ResponsibilitilQty * SumSubContractQty / PlanQty, 2); } }
        /// <summary>
        /// 累计责任工程价格
        /// </summary>
        public decimal SumResponsibilitilPrice { get; set; }
        /// <summary>
        /// 累计责任成本金额
        /// </summary>
        public decimal SumResponsibilitilTotalPrice { get { return SumResponsibilitilPrice * SumResponsibilitilQty; } }
        /// <summary>
        /// 累计合同工程量  累计合同收入量（22）22=17/1*3
        /// </summary>
        public decimal SumContractQty { get { return PlanQty == 0 ? 0 : Math.Round(SumSubContractQty * ContractQty / PlanQty); } }
        /// <summary>
        /// 累计合同工程价格
        /// </summary>
        public decimal SumContractPrice { get; set; }
        /// <summary>
        /// 累计合同成本金额
        /// </summary>
        public decimal SumContractTotalPrice { get { return SumContractQty * SumContractPrice; } }
        /// <summary>
        /// 累计利润 累计利润(24)=累计合同收入金额-累计实际成本金额24=23-19
        /// </summary>
        public decimal SumProfit { get { return SumContractTotalPrice - SumRealTotalPrice; } }
        /// <summary>
        /// 累计利润率 累计利润率（25）=累计利润/累计合同收入金额25=24/23
        /// </summary>
        public decimal SumProfitRate { get { return SumContractTotalPrice == 0 ? 0 : Math.Round(SumProfit * 100 / SumContractTotalPrice, 2); } }
        /// <summary>
        /// 累计节超额  累计节超额（26）=累计责任成本-累计实际成本26=21-19
        /// </summary>
        public decimal SumSave { get { return SumResponsibilitilTotalPrice - SumRealTotalPrice; } }
        /// <summary>
        /// 累计节超额比例  累计节超例（27）=累计节超额/累计责任成本27=26/21
        /// </summary>
        public decimal SumSaveRate { get { return SumResponsibilitilTotalPrice == 0 ? 0 : Math.Round(SumSave * 100 / SumResponsibilitilTotalPrice, 2); } }
       
        #endregion
        public void Calculation()
        {
            if (this.Detail != null)
            {
                foreach (TempGWBSConsume oConsume in Detail)
                {

                   // this.CurrContractQty += oConsume.CurrContractQty;
                    //this.CurrContractTotalPrice += oConsume.CurrContractTotalPrice;
                    this.CurrMaterialTotalPrice += oConsume.CurrMaterialTotalPrice;
                    this.CurrOtherOutPutTotalPrice += oConsume.CurrOtherOutPutTotalPrice;
                    this.CurrRealTotalPrice += oConsume.CurrRealTotalPrice;
                    //this.CurrResponsibilitilQty += oConsume.CurrResponsibilitilQty;
                    //this.CurrResponsibilitilTotalPrice += oConsume.CurrResponsibilitilTotalPrice;
                    this.CurrSubContractQty += oConsume.CurrSubContractQty;
                    this.CurrSubContractTotalPrice += oConsume.CurrSubContractTotalPrice;
                   // this.SumContractQty += oConsume.SumContractQty;
                    //this.SumContractTotalPrice += oConsume.SumContractTotalPrice;
                    this.SumMaterialTotalPrice += oConsume.SumMaterialTotalPrice;
                    this.SumOtherPutOutTotalPrice += oConsume.SumOtherPutOutTotalPrice;
                    this.SumRealTotalPrice += oConsume.SumRealTotalPrice;
                    //this.SumResponsibilitilQty += oConsume.SumResponsibilitilQty;
                    //this.SumResponsibilitilTotalPrice += oConsume.SumResponsibilitilTotalPrice;
                    this.SumSubContractQty += oConsume.SumSubContractQty;
                    this.SumSubContractTotalPrice += oConsume.SumSubContractTotalPrice;
                }
            }
        }
    }
    public class TempGWBSConsume
    {
    
        public string ID { get; set; }
        public TempGWBSDetail Parent{get;set;}
        /// <summary>
        /// 资源类型ID
        /// </summary>
        public string ResourceTypeGuid { get; set; }
        /// <summary>
        /// 核算科目ID
        /// </summary>
        public string SubjectGuid { get; set; }
        
        /// <summary>
        /// 本期分包结算工程量
        /// </summary>
        public decimal CurrSubContractQty { get; set; }
        /// <summary>
        /// 本期分包结算工程合价
        /// </summary>
        public decimal CurrSubContractTotalPrice { get; set; }
        /// <summary>
        /// 分包结算单价
        /// </summary>
       // public decimal CurrSubContractPrice { get { return CurrSubContractQty ==0 ? 0 :Math.Round( CurrSubContractTotalPrice / CurrSubContractQty,2); } }
        /// <summary>
        /// 本期实际成本均价（5.1）5.1=6/4
        /// </summary>
       // public decimal CurrRealPrice { get { return CurrSubContractQty == 0 ? 0 : Math.Round(CurrRealTotalPrice / CurrSubContractQty,2); } }
        /// <summary>
        /// 本期实际成本金额（含材料费等）
        /// </summary>
        public decimal CurrRealTotalPrice { get; set; }
        /// <summary>
        /// 其中：本期额外支出金额
        /// </summary>
        public decimal CurrOtherOutPutTotalPrice { get; set; }
        /// <summary>
        /// 其中：本期材料费金额
        /// </summary>
        public decimal CurrMaterialTotalPrice { get; set; }
       
        /// <summary>
        /// 责任金额
        /// </summary>
        public decimal CurrResponsibilitilTotalPrice { get; set; }
        
        /// <summary>
        /// 合同金额
        /// </summary>
        public decimal CurrContractTotalPrice { get; set; }
     
        /// <summary>
        /// 累计分包结算工程量
        /// </summary>
        public decimal SumSubContractQty { get; set; }
        /// <summary>
        /// 累计分包结算合价价
        /// </summary>
        public decimal SumSubContractTotalPrice { get; set; }
        /// <summary>
        /// 累计分包结算单价
        /// </summary>
       // public decimal SumSubContractPrice { get; set; }
        /// <summary>
        /// 累计实际成本平均单价
        /// </summary>
      //  public decimal SumRealPrice { get; set; }
        /// <summary>
        /// 累计实际成本金额
        /// </summary>
        public decimal SumRealTotalPrice { get; set; }
        /// <summary>
        /// 其中：累计额外支出金额
        /// </summary>
        public decimal SumOtherPutOutTotalPrice { get; set; }
        /// <summary>
        /// 其中：累计材料费金额
        /// </summary>
        public decimal SumMaterialTotalPrice { get; set; }
        
        /// <summary>
        /// 累计责任成本金额
        /// </summary>
        public decimal SumResponsibilitilTotalPrice { get; set; }
        
        /// <summary>
        /// 累计合同成本金额
        /// </summary>
        public decimal SumContractTotalPrice { get; set; }
    }
   
}
