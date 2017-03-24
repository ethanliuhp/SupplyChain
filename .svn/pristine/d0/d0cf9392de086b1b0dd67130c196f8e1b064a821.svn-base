using System;
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

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VSelectGWBSValence : TBasicDataView
    {
        private GWBSTree wbs = null;
        CurrentProjectInfo projectInfo = null;
        public MGWBSTree model = new MGWBSTree();
        //private Hashtable detailHashtable = new Hashtable();
        private int startImageCol = 1, endImageCol = 19;
        private string imageExpand = "imageExpand";
        private string imageCollapse = "imageCollapse";

        List<GWBSTree> listResult = new List<GWBSTree>();//工程任务集合
        List<GWBSDetail> dtlList = new List<GWBSDetail>(); //任务明细集合

        bool isShowDtl = false;

        public VSelectGWBSValence()
        {
            InitializeComponent();
            Data();
            InitEvents();
        }

        private void Data()
        {
            tabQuery.TabPages.Remove(tabPage1);
            txtMaterialCategory.IsCheckBox = false;//多选
            cmbValenceType.Items.AddRange(new object[] { "合同收入", "责任成本", "计划成本" });
            cmbValenceType.SelectedIndex = 1;
            projectInfo = StaticMethod.GetProjectInfo();
            InitFlexGrid(1);
            InitFlexGrid(flexGridWBS, 0,string.Format( "[{0}]工程成本维护收支对比分析表",projectInfo.Name), true);
            InitFlexGrid(flexGridResource, 0, "根据资源分类进行合同收入和责任成本对比",true );
        }
        private void InitEvents()
        {
            btnSelect.Click += new EventHandler(btnSelect_Click);
            btnSelectGWBSTree.Click += new EventHandler(btnSelectGWBSTree_Click);
            flexGrid.Click += new FlexCell.Grid.ClickEventHandler(flexGrid_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);

            btnQueryResource.Click += new EventHandler(btnQuery_Click);
            btnQueryWBS.Click += new EventHandler(btnQuery_Click);
            btnSelectGWBSTreeWBS.Click += new EventHandler(btnSelectGWBSTreeWBS_Click);
            btnExcelResource.Click += new EventHandler(btnExcel_Click);
            btnExcelWBS.Click += new EventHandler(btnExcel_Click);
            flexGridResource.Click += new FlexCell.Grid.ClickEventHandler(flexGrid_Click);
             this.flexGridWBS.Click += new FlexCell.Grid.ClickEventHandler(flexGrid_Click);
        }
        public void btnQuery_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("查询中......");
            DataTable oTable = null;
            if (sender == btnQueryWBS)
            {
                if (txtGWBSTreeWBS.Tag != null)
                {
                    GWBSTree oGWBSTree = txtGWBSTreeWBS.Tag as GWBSTree;
                    oTable = model.SelectGWBSValence(oGWBSTree, projectInfo);
                }
                else
                {
                    MessageBox.Show("请选择一个任务节点");
                }
            }
            else if(sender==btnQueryResource)
            {
                if (txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
                {
                    string sSysCode = (txtMaterialCategory.Result[0]as MaterialCategory ).SysCode;
                    oTable = model.SelectGWBSValence(sSysCode, projectInfo.Id);
                }
                else
                {
                    MessageBox.Show("请选择物资分类");
                }
            }
            if (oTable != null && oTable.Rows.Count>0)
            {
                IList<GWBSTreeTemp> lstTemp = getTree(oTable);
                FillFlex(lstTemp);
            }
            else
            {
                InitFlexGrid(GetFlexGrid(), 0, GetTitle(),true);
            }
            FlashScreen.Close();
        }
        #region 工程任务合价查询
        private void InitFlexGrid(int rows)
        {
            flexGrid.AutoRedraw = false;
            flexGrid.Column(0).Visible = false;

            flexGrid.Rows = rows;
            flexGrid.Cols = 25;//其中0列隐藏 1-19 为放置图片列 20-23为数据列

            flexGrid.SelectionMode = FlexCell.SelectionModeEnum.ByCell;
            //flexGrid.ExtendLastCol = true;
            flexGrid.DisplayFocusRect = false;
            flexGrid.LockButton = true;
            flexGrid.ReadonlyFocusRect = FlexCell.FocusRectEnum.Solid;
            flexGrid.BorderStyle = FlexCell.BorderStyleEnum.FixedSingle;
            flexGrid.ScrollBars = FlexCell.ScrollBarsEnum.Both;
            flexGrid.BackColorBkg = SystemColors.Control;
            flexGrid.DefaultFont = new Font("Tahoma", 8);
           
            FlexCell.Range range;
            //int startImageCol = 1, endImageCol = 19;

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
            //单位 任务明细 价格 数量 合价
            flexGrid.Cell(0, endImageCol + 1).Text = "任务明细";//20
            flexGrid.Column(endImageCol + 1).Visible = false; //"任务明细";//21
            flexGrid.Cell(0, endImageCol + 2).Text = "单位";//21
            flexGrid.Cell(0, endImageCol + 3).Text = "价格";//23
            flexGrid.Cell(0, endImageCol + 4).Text = "数量";//24
            flexGrid.Cell(0, endImageCol + 5).Text = "合价";//22
          

            flexGrid.Column(endImageCol + 1).CellType = FlexCell.CellTypeEnum.TextBox;
            flexGrid.Column(endImageCol + 2).CellType = FlexCell.CellTypeEnum.TextBox;
            flexGrid.Column(endImageCol + 3).CellType = FlexCell.CellTypeEnum.TextBox;
            flexGrid.Column(endImageCol + 4).CellType = FlexCell.CellTypeEnum.TextBox;
            flexGrid.Column(endImageCol + 5).CellType = FlexCell.CellTypeEnum.TextBox;

            flexGrid.Column(endImageCol + 1).Width =200;
            flexGrid.Column(endImageCol + 2).Width = 100;
            flexGrid.Column(endImageCol + 3).Width = 100;
            flexGrid.Column(endImageCol + 4).Width = 100;
            flexGrid.Column(endImageCol + 5).Width = 200;


            for (int i = endImageCol + 1; i < flexGrid.Cols; i++)
            {
                flexGrid.Column(i).Locked = true;
            }

            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        //选择工程任务
        void btnSelectGWBSTree_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsSelectSingleNode = true;
            frm.ShowDialog();
            List<TreeNode> list = frm.SelectResult;
            if (list.Count > 0)
            {
                wbs = (list[0] as TreeNode).Tag as GWBSTree;
                txtGWBSTree.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), wbs.Name, wbs.SysCode);
                txtGWBSTree.Tag = wbs;
                txtGWBSTree.Focus();
            }
        }
        //查询
        void btnSelect_Click(object sender, EventArgs e)
        {
            if (wbs == null || string.IsNullOrEmpty(wbs.Id))
            {
                MessageBox.Show("请选择一个任务节点");
                return;
            }
            FlashScreen.Show("查询中......");
            string type = cmbValenceType.Text;
            DataTable table = model.SelectGWBSValence(wbs, projectInfo);

            listResult = new List<GWBSTree>();
            dtlList = new List<GWBSDetail>();
            List<GWBSTree> list = new List<GWBSTree>();

            //list.Add(wbs);
            //listResult.Add(wbs);

            //工程任务明细集合

            foreach (DataRow dataRow in table.Rows)
            {
                if (dataRow["dName"].ToString() == "") continue;
                GWBSDetail dtl = new GWBSDetail();
                dtl.Name = dataRow["dName"].ToString();
                dtl.TheGWBS = new GWBSTree();
                dtl.TheGWBS.Id = dataRow["id"].ToString();
                dtl.TheGWBS.SysCode = dataRow["SysCode"].ToString();
                dtl.TheGWBS.CategoryNodeType = (NodeType)ClientUtil.ToInt(dataRow["CategoryNodeType"]);
                dtl.WorkAmountUnitName = dataRow["workamountunitname"].ToString();
                //责任成本
                dtl.ResponsibilitilyWorkAmount = ClientUtil.ToDecimal(dataRow["responsibilitilyworkamount"]);
                dtl.ResponsibilitilyPrice = ClientUtil.ToDecimal(dataRow["responsibilitilyprice"]);
                dtl.ResponsibilitilyTotalPrice = ClientUtil.ToDecimal(dataRow["responsibilitilytotalprice"]);
                //合同收入
                dtl.ContractProjectQuantity = ClientUtil.ToDecimal(dataRow["ContractProjectQuantity"]);
                dtl.ContractPrice = ClientUtil.ToDecimal(dataRow["ContractPrice"]);
                dtl.ContractTotalPrice = ClientUtil.ToDecimal(dataRow["ContractTotalPrice"]);
                //计划成本
                dtl.PlanWorkAmount = ClientUtil.ToDecimal(dataRow["PlanWorkAmount"]);
                dtl.PlanPrice = ClientUtil.ToDecimal(dataRow["PlanPrice"]);
                dtl.PlanTotalPrice = ClientUtil.ToDecimal(dataRow["PlanTotalPrice"]);

                dtl.OrderNo = ClientUtil.ToInt(dataRow["dOrderno"]);
                dtlList.Add(dtl);
            }
            //wbs.ResponsibilityTotalPrice = 0;
            //wbs.ContractTotalPrice = 0;
            //wbs.PlanTotalPrice = 0;
            //foreach (GWBSDetail detail in dtlList)
            //{
            //    if (wbs.Id == detail.TheGWBS.Id)
            //    {
            //        wbs.ContractTotalPrice += detail.ContractTotalPrice;
            //        wbs.PlanTotalPrice += detail.PlanTotalPrice;
            //        wbs.ResponsibilityTotalPrice += detail.ResponsibilitilyTotalPrice;
            //    }
            //}
            foreach (DataRow dataRow in table.Rows)
            {
                bool flag = true;
                if (list.Count > 0)
                {
                    foreach (GWBSTree gt in list)
                    {
                        if (gt.Id == dataRow["Id"].ToString())
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                else
                {
                    flag = true;
                }
                if (flag)
                {
                    GWBSTree wbst = new GWBSTree();
                    wbst.Id = dataRow["Id"].ToString();
                    wbst.Name = dataRow["Name"].ToString();
                    wbst.SysCode = dataRow["SysCode"].ToString();
                    wbst.OrderNo = ClientUtil.ToLong(dataRow["orderNo"]);
                    wbst.ParentNode = new GWBSTree();
                    wbst.ParentNode.Id = dataRow["parentnodeid"].ToString();
                    wbst.CategoryNodeType = (NodeType)ClientUtil.ToInt(dataRow["CategoryNodeType"]);
                    wbst.Level = ClientUtil.ToInt(dataRow["tlevel"]);
                    wbst.ContractTotalPrice = 0;
                    wbst.PlanTotalPrice = 0;
                    wbst.ResponsibilityTotalPrice = 0;
                    wbst.AddUpFigureProgress = 0;
                    wbst.ResponsibilityQuantity = 0;
                    wbst.ContractQuantity = 0;
                    wbst.ResponsibilityQuantity = 0;
                    wbst.PlanQuantity = 0;
                    foreach (GWBSDetail detail in dtlList)
                    {
                        if (wbst.Id == detail.TheGWBS.Id)
                        {
                            if (cmbValenceType.Text == "责任成本")
                            {
                                wbst.AddUpFigureProgress += detail.ResponsibilitilyTotalPrice;
                               // wbst.ResponsibilityQuantity += detail.ResponsibilitilyWorkAmount;
                            }
                            else if (cmbValenceType.Text == "合同收入")
                            {
                                wbst.AddUpFigureProgress += detail.ContractTotalPrice;
                                //wbst.ContractQuantity += detail.ContractProjectQuantity;
                            }
                            else
                            {//计划收入
                                wbst.AddUpFigureProgress += detail.PlanTotalPrice;
                               // wbst.PlanQuantity+=detail.PlanWorkAmount;
                            }
                        }
                        if (detail.TheGWBS.SysCode.Contains(wbst.SysCode) && detail.TheGWBS.CategoryNodeType == NodeType.LeafNode)
                        {
                            //wbst.AddUpFigureProgress += detail.ResponsibilitilyTotalPrice;
                            wbst.ContractTotalPrice += detail.ContractTotalPrice;
                            wbst.PlanTotalPrice += detail.PlanTotalPrice;
                            wbst.ResponsibilityTotalPrice += detail.ResponsibilitilyTotalPrice;
                        }
                    }
                    list.Add(wbst);
                    if (wbst.Id == wbs.Id)
                    {
                        wbs.ResponsibilityTotalPrice = wbst.ResponsibilityTotalPrice;
                        wbs.ContractTotalPrice = wbst.ContractTotalPrice;
                        wbs.PlanTotalPrice = wbst.PlanTotalPrice;
                        wbs.AddUpFigureProgress = wbst.AddUpFigureProgress;
                    }
                }
            }
            list.Remove(wbs);
            listResult.Add(wbs);
            //ArrayList listResult = new ArrayList();//排序后的工程任务集合
            getChild(wbs, list, ref listResult);//对工程任务排序

            flexGrid.Cell(0, endImageCol + 1).Text = cmbValenceType.Text;
            //if (cbIsShow.Checked)
            //{
            //    isShowDtl = true;
                FillFlex(table);
            //}
            //else
            //{
            //    isShowDtl = false;
            //    FillFlexWBS(table);

            //}
                FlashScreen.Close();
        }

        void flexGrid_Click(object Sender, EventArgs e)
        {
            //if (tabQuery.SelectedTab == tabPage2 || tabQuery.SelectedTab == tabPage3)
            //{
                FlexGrid_Click(Sender,e);
                return;
           // }
            #region 不需要
            //展开
            //if (flexGrid.ActiveCell.ImageKey == imageExpand)
            //{
            //    int activeRowIndex = flexGrid.ActiveCell.Row;
            //    if (flexGrid.Cell(activeRowIndex, endImageCol + 1).Tag == "2") return;

            //    flexGrid.ActiveCell.SetImage(imageCollapse);


            //    //int childs = 0;
            //    if (isShowDtl)
            //    {
            //        string syscode = flexGrid.Cell(activeRowIndex, 0).Tag;
            //        int level = wbs.Level + flexGrid.ActiveCell.Col;
            //        var query = from d in dtlList
            //                    where d.TheGWBS.SysCode == syscode && d.Name != ""
            //                    select d;
            //        for (int i = activeRowIndex + query.Count() + 1; i < flexGrid.Rows; i++)
            //        {
            //            int l = ClientUtil.ToInt(flexGrid.Cell(i, 1).Tag);
            //            if (l == level)
            //            {
            //                flexGrid.Row(i).Visible = true;
            //            }
            //            else if (l < level) break;
            //        }
            //    }
            //    else
            //    {
            //        int level = wbs.Level + flexGrid.ActiveCell.Col;
            //        for (int i = activeRowIndex + 1; i < flexGrid.Rows; i++)
            //        {
            //            int l = ClientUtil.ToInt(flexGrid.Cell(i, 1).Tag);
            //            if (l == level)
            //            {
            //                flexGrid.Row(i).Visible = true;
            //            }
            //            else if (l < level) break;
            //        }
            //    }
            //}
            //else if (flexGrid.ActiveCell.ImageKey == imageCollapse)//收起
            //{
            //    int activeRowIndex = flexGrid.ActiveCell.Row;
            //    if (flexGrid.Cell(activeRowIndex, endImageCol + 1).Tag == "2") return;

            //    flexGrid.ActiveCell.SetImage(imageExpand);

            //    string syscode = flexGrid.Cell(activeRowIndex, 0).Tag;
            //    if (isShowDtl)
            //    {
            //        int dtlListCount = 0;
            //        int childs = getChildCount(syscode, ref dtlListCount);
            //        if (childs > 0)
            //        {
            //            SetRowVisible(activeRowIndex + dtlListCount + 1, activeRowIndex + dtlListCount + childs, false);
            //        }
            //    }
            //    else
            //    {
            //        int childs = getWBSChildCount(syscode) - 1;
            //        if (childs > 0)
            //        {
            //            SetRowVisible(activeRowIndex + 1, activeRowIndex + childs, false);
            //        }
            //    }
            //}
            #endregion
        }

        private void SetRowVisible(int row1, int row2, bool value)
        {
            flexGrid.AutoRedraw = false;
            for (int i = row1; i <= row2; i++)
            {
                flexGrid.Row(i).Visible = value;
                if (!value)
                {
                    for (int j = startImageCol; j <= endImageCol; j++)
                    {
                        if (flexGrid.Cell(i, j).ImageKey != null && !flexGrid.Cell(i, j).ImageKey.Equals("") && flexGrid.Cell(i, endImageCol + 1).Tag != "2")
                        {
                            flexGrid.Cell(i, j).SetImage(imageExpand);
                            break;
                        }
                    }
                }
            }
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        private void FillFlex(DataTable table)
        {
            flexGrid.AutoRedraw = false;
            flexGrid.Rows = 2;
            int rowIndex = 1;
            flexGrid.Column(endImageCol + 5).Locked = true;
            int startLevel = wbs.Level, iExpandLevel = 1, iCol=0;
            IList<GWBSDetail> query = null;
            
            //填充
            foreach (GWBSTree gt in listResult)
            {
                bool dtlIsHide = false;
                //加载工程任务
                flexGrid.InsertRow(rowIndex, 1);
                //获取任务明细
                if (cbIsShow.Checked)
                {
                    query = dtlList.Where(a => (a.TheGWBS.Id == gt.Id && !string.IsNullOrEmpty(a.Name))).OrderBy(a => a.OrderNo).ToList();

                }
                //from d in dtlList  where d.TheGWBS.Id == gt.Id && d.Name != "" orderby d.OrderNo ascending select d;
                //if (gt.Level == wbs.Level)
                //{
                //    flexGrid.Cell(rowIndex, gt.Level - startLevel).SetImage(imageCollapse);
                //}
                //else
                //{
                //    if (gt.CategoryNodeType == NodeType.LeafNode &&  query.)
                //    {
                //        flexGrid.Cell(rowIndex, gt.Level - startLevel).SetImage(imageCollapse);
                //        flexGrid.Cell(rowIndex, endImageCol + 1).Tag = "2";
                //    }
                //    else
                //    {
                //        flexGrid.Cell(rowIndex, gt.Level - startLevel).SetImage(imageExpand);
                //    }
                //}

                //if (gt.Level > wbs.Level + 1)
                //{
                //    flexGrid.Row(rowIndex).Visible = false;
                //    dtlIsHide = false;
                //}
                //else
                //{
                //    flexGrid.Row(rowIndex).Visible = true;
                //    dtlIsHide = true;
                //}
                 iCol = gt.Level - startLevel + 1;
                 if (gt.CategoryNodeType == NodeType.LeafNode && ((query == null || query.Count == 0)))//如果是叶节点 并且后面的没任务明细不显示树形图片
                 {
                     flexGrid.Row(rowIndex).Visible = (gt.Level - startLevel < iExpandLevel + 1) ? true : false;
                 }
                 else
                 {
                     flexGrid.Cell(rowIndex, iCol).SetImage((gt.Level - startLevel < iExpandLevel) ? imageCollapse : imageExpand);
                     flexGrid.Row(rowIndex).Visible = (gt.Level - startLevel < iExpandLevel + 1) ? true : false;
                     iCol += 1;
                 }
                //flexGrid.Cell(rowIndex, gt.Level - startLevel).SetImage(imageCollapse);
                 FlexCell.Range rangeTemp = flexGrid.Range(rowIndex, iCol, rowIndex, endImageCol);
                rangeTemp.Merge();
                //flexGrid.Cell(rowIndex, 0).Tag = gt.SysCode;
                //flexGrid.Cell(rowIndex, 1).Tag = gt.Level.ToString();
                flexGrid.Cell(rowIndex, 0).Tag = gt.Id;
                flexGrid.Cell(rowIndex, 1).Tag = gt.ParentNode==null?"":gt.ParentNode.Id;
                flexGrid.Cell(rowIndex, iCol).Text = gt.Name;
                //单位 价格 数量 合价
                if (cmbValenceType.Text == "责任成本")
                {
                   // flexGrid.Cell(rowIndex, endImageCol + 3).Text =Math.Round( gt.ResponsibilityQuantity==0?0: gt.ResponsibilityTotalPrice / gt.ResponsibilityQuantity,2).ToString("N2");//24 单价
                    flexGrid.Cell(rowIndex, endImageCol + 4).Text = gt.ResponsibilityQuantity.ToString("N2");//23 工程量
                    flexGrid.Cell(rowIndex, endImageCol + 5).Text = gt.ResponsibilityTotalPrice == 0?gt.AddUpFigureProgress.ToString():gt.ResponsibilityTotalPrice.ToString("N2") ;
                }
                else if (cmbValenceType.Text == "合同收入")
                {
                    //flexGrid.Cell(rowIndex, endImageCol + 3).Text = Math.Round(gt.ContractQuantity == 0 ? 0 : gt.ContractTotalPrice / gt.ContractQuantity, 2).ToString("N2");//24 单价
                    flexGrid.Cell(rowIndex, endImageCol + 4).Text = gt.ContractQuantity.ToString("N2");//23 工程量
                    flexGrid.Cell(rowIndex, endImageCol + 5).Text = gt.ContractTotalPrice == 0 ? gt.AddUpFigureProgress.ToString() : gt.ContractTotalPrice.ToString("N2");
                }
                else
                {//计划收入
                    //flexGrid.Cell(rowIndex, endImageCol + 3).Text = Math.Round(gt.PlanQuantity == 0 ? 0 : gt.PlanTotalPrice / gt.PlanQuantity, 2).ToString("N2");//24 单价
                    flexGrid.Cell(rowIndex, endImageCol + 4).Text = gt.PlanQuantity.ToString("N2");//23 工程量
                    flexGrid.Cell(rowIndex, endImageCol + 5).Text = gt.PlanTotalPrice == 0 ? gt.AddUpFigureProgress.ToString() : gt.PlanTotalPrice.ToString("N2");
                }

                //加载节点上的明细
                //if (gt.CategoryNodeType == VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode)
                //{
                rowIndex = rowIndex + 1;
                #region 添加明细
                if (cbIsShow.Checked)
                {
                    flexGrid.InsertRow(rowIndex, query.Count());
                    foreach (GWBSDetail gd in query)
                    {    //单位 价格 数量 合价
                        FlexCell.Range rangeTemp2 = flexGrid.Range(rowIndex, iCol, rowIndex, endImageCol);
                        rangeTemp2.Merge();
                        //flexGrid.Row(rowIndex).Visible = dtlIsHide;
                        // flexGrid.Cell(rowIndex, 0).Tag = gd.Id;
                        flexGrid.Cell(rowIndex, 1).Tag = gt.Id;
                        flexGrid.Cell(rowIndex, iCol).Text = gd.Name;
                        //flexGrid.Cell(rowIndex, endImageCol + 2).Text = gd.Name;//21 任务明细
                        flexGrid.Cell(rowIndex, endImageCol + 2).Text = gd.WorkAmountUnitName;//22 单位
                        if (cmbValenceType.Text == "责任成本")
                        {
                            flexGrid.Cell(rowIndex, endImageCol + 3).Text = gd.ResponsibilitilyPrice.ToString("N2");//24 单价
                            flexGrid.Cell(rowIndex, endImageCol + 4).Text = gd.ResponsibilitilyWorkAmount.ToString("N2");//23 工程量
                            flexGrid.Cell(rowIndex, endImageCol + 5).Text = gd.ResponsibilitilyTotalPrice.ToString("N2");//20 合价
                        
                           
                        }
                        else if (cmbValenceType.Text == "合同收入")
                        {
                            flexGrid.Cell(rowIndex, endImageCol + 5).Text = gd.ContractTotalPrice.ToString("N2");//20 合价
                            flexGrid.Cell(rowIndex, endImageCol + 4).Text = gd.ContractProjectQuantity.ToString("N2");//23 工程量
                            flexGrid.Cell(rowIndex, endImageCol + 3).Text = gd.ContractPrice.ToString("N2");//24 单价
                        }
                        else
                        {//计划收入
                            flexGrid.Cell(rowIndex, endImageCol + 5).Text = gd.PlanTotalPrice.ToString("N2");//20 合价
                            flexGrid.Cell(rowIndex, endImageCol + 4).Text = gd.PlanWorkAmount.ToString("N2");//23 工程量
                            flexGrid.Cell(rowIndex, endImageCol + 3).Text = gd.PlanPrice.ToString("N2");//24 单价
                        }
                        flexGrid.Row(rowIndex).Visible = (gt.Level - startLevel < iExpandLevel) ? true : false;
                        rowIndex = rowIndex + 1;



                    }
                }
                #endregion
                //}
                //else
                //{
                //    rowIndex = rowIndex + 1;
                //}
            }

            //1-19列的背景色
            FlexCell.Range range = flexGrid.Range(1, startImageCol, flexGrid.Rows - 1, endImageCol);
            if (range != null)
            {
                range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                range.BackColor = SystemColors.Control;
            }
            range = flexGrid.Range(1, endImageCol + 1, flexGrid.Rows - 1, flexGrid.Cols-1);
            if (range != null)
            {
                range.Alignment = FlexCell.AlignmentEnum.RightCenter;
            }
            flexGrid.Row(flexGrid.Rows - 1).Delete();
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        private void getChild(GWBSTree parentNode, List<GWBSTree> list, ref List<GWBSTree> listResult)
        {
            var query = from d in list
                        where d.ParentNode.Id == parentNode.Id
                        orderby d.OrderNo ascending
                        orderby d.Level ascending
                        select d;
            parentNode.PlanQuantity = 0;
            parentNode.ResponsibilityQuantity = 0;
            parentNode.ContractQuantity = 0;
            foreach (GWBSTree gt in query)
            {
                listResult.Add(gt);

                getChild(gt, list, ref listResult);
                if (cmbValenceType.Text == "责任成本")
                {
                     parentNode.ResponsibilityQuantity += gt.ResponsibilityQuantity;
                }
                else if (cmbValenceType.Text == "合同收入")
                {
                    parentNode.ContractQuantity += gt.ContractQuantity;
                }
                else
                {
                    parentNode.PlanQuantity += gt.PlanQuantity;
                }

            }
            if (cmbValenceType.Text == "责任成本")
            {
                parentNode.ResponsibilityQuantity += dtlList.Where(a => a.TheGWBS.Id == parentNode.Id).Sum(a => a.ResponsibilitilyWorkAmount);
            }
            else if (cmbValenceType.Text == "合同收入")
            {
                parentNode.ContractQuantity += dtlList.Where(a => a.TheGWBS.Id == parentNode.Id).Sum(a => a.ContractProjectQuantity);
            }
            else
            {
                parentNode.PlanQuantity += dtlList.Where(a => a.TheGWBS.Id == parentNode.Id).Sum(a => a.PlanWorkAmount);
            } 
        }

        private int getChildCount(string syscode, ref int dtlListCount)
        {
            int count = 0;
            dtlListCount = 0;
            foreach (GWBSTree gt in listResult)
            {
                if (gt.SysCode.Contains(syscode) && gt.SysCode != syscode)
                    count += 1;
            }
            foreach (GWBSDetail dtl in dtlList)
            {
                if (dtl.TheGWBS.SysCode.Contains(syscode))//dtl.TheGWBS.CategoryNodeType == NodeType.LeafNode &&
                {
                    if (dtl.TheGWBS.SysCode == syscode)
                    {
                        dtlListCount += 1;
                    }
                    else
                    {
                        count += 1;
                    }
                }
            }
            return count;
        }

        //导出excel
        void btnExcel_Click(object sender, EventArgs e)
        {
            CustomFlexGrid oFlexGrid=null;
            string sName=string.Empty ;
            if(tabQuery.SelectedTab==tabPage1)
            {
                oFlexGrid=flexGrid;
                sName="工程任务合价";
            }
            else if(tabQuery.SelectedTab==tabPage2){
                 oFlexGrid=flexGridWBS;
                sName="根据WBS节点进行合同收入和责任成本对比";
            }
            else if(tabQuery.SelectedTab==tabPage3){
                 oFlexGrid=flexGridResource;
                sName="根据资源分类进行合同收入和责任成本对比";
            }
            if(oFlexGrid!=null){
            oFlexGrid.ExportToExcel("", sName, true, true);
            }
        }

        private void FillFlexWBS(DataTable table)
        {
            flexGrid.AutoRedraw = false;
            flexGrid.Rows = 2;
            int rowIndex = 1;
            flexGrid.Column(endImageCol + 5).Locked = true;
            int startLevel = wbs.Level - 1;

            //填充
            foreach (GWBSTree gt in listResult)
            {
                //加载工程任务
                flexGrid.InsertRow(rowIndex, 1);
                if (gt.Level == wbs.Level)
                {
                    flexGrid.Cell(rowIndex, gt.Level - startLevel).SetImage(imageCollapse);
                }
                else
                {
                    if (gt.CategoryNodeType == NodeType.LeafNode)
                    {
                        flexGrid.Cell(rowIndex, gt.Level - startLevel).SetImage(imageCollapse);
                        flexGrid.Cell(rowIndex, endImageCol + 1).Tag = "2";
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, gt.Level - startLevel).SetImage(imageExpand);
                    }
                }

                if (gt.Level > wbs.Level + 1)
                {
                    flexGrid.Row(rowIndex).Visible = false;
                }
                else
                {
                    flexGrid.Row(rowIndex).Visible = true;
                }
                FlexCell.Range rangeTemp = flexGrid.Range(rowIndex, gt.Level - startLevel + 1, rowIndex, endImageCol);
                rangeTemp.Merge();
                //flexGrid.Cell(rowIndex, 0).Tag = gt.SysCode;
                //flexGrid.Cell(rowIndex, 1).Tag = gt.Level.ToString();
                flexGrid.Cell(rowIndex, 0).Tag = gt.Id;
                flexGrid.Cell(rowIndex, 1).Tag = gt.ParentNode==null?"":gt.ParentNode.Id;
                flexGrid.Cell(rowIndex, gt.Level - startLevel + 1).Text = gt.Name;


                if (cmbValenceType.Text == "责任成本")
                {
                    //flexGrid.Cell(rowIndex, endImageCol + 1).Text = gt.ResponsibilityTotalPrice.ToString();
                    flexGrid.Cell(rowIndex, endImageCol + 1).Text = gt.ResponsibilityTotalPrice == 0 ? gt.AddUpFigureProgress.ToString() : gt.ResponsibilityTotalPrice.ToString();
                }
                else if (cmbValenceType.Text == "合同收入")
                {
                    flexGrid.Cell(rowIndex, endImageCol + 1).Text = gt.ContractTotalPrice == 0 ? gt.AddUpFigureProgress.ToString() : gt.ContractTotalPrice.ToString();
                }
                else
                {//计划收入
                    flexGrid.Cell(rowIndex, endImageCol + 1).Text = gt.PlanTotalPrice == 0 ? gt.AddUpFigureProgress.ToString() : gt.PlanTotalPrice.ToString();
                }

                rowIndex = rowIndex + 1;
            }

            //1-19列的背景色
            FlexCell.Range range = flexGrid.Range(1, startImageCol, flexGrid.Rows - 1, endImageCol);
            if (range != null)
            {
                range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                range.BackColor = SystemColors.Control;
            }
            flexGrid.Row(flexGrid.Rows - 1).Delete();
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        private int getWBSChildCount(string syscode)
        {
            int count = 0;
            foreach (GWBSTree gt in listResult)
            {
                if (gt.SysCode.Contains(syscode))
                    count += 1;
            }
            return count;
        }
        #endregion
        #region 根据WBS节点进行合同收入和责任成本对比
        public void btnSelectGWBSTreeWBS_Click(object sender, EventArgs e)
        {
            GWBSTree oGWBSTree = null;
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsSelectSingleNode = true;
            frm.ShowDialog();
            List<TreeNode> list = frm.SelectResult;
            if (list.Count > 0)
            {
                oGWBSTree = (list[0] as TreeNode).Tag as GWBSTree;
                txtGWBSTreeWBS.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), oGWBSTree.Name, oGWBSTree.SysCode);
                txtGWBSTreeWBS.Tag = oGWBSTree;
                txtGWBSTreeWBS.Focus();
            }
        }
       
        #endregion
        #region 根据资源分类节点进行合同收入和责任成本对比
        #endregion
        #region 公共方法
        public void FlexGrid_Click(object Sender, EventArgs e)
        {
            
            int iRow = 0,iCol = 0;
            FlexCell.Cell oCell = null;
            CustomFlexGrid oFlexGrid=GetFlexGrid();
            
            if (!string.IsNullOrEmpty(oFlexGrid.ActiveCell.ImageKey))
            {
                oFlexGrid.AutoRedraw = false;
                bool bExpand = oFlexGrid.ActiveCell.ImageKey == imageExpand;//未展开imageExpand
                string sCurID = string.Empty;
                iRow = oFlexGrid.ActiveCell.Row;
                iCol = oFlexGrid.ActiveCell.Col;
               // oFlexGrid.Row(iRow).Visible = bExpand;
                oFlexGrid.ActiveCell.SetImage(bExpand ? imageCollapse : imageExpand);
                sCurID = oFlexGrid.Cell(iRow, 0).Tag;
                if (!string.IsNullOrEmpty(sCurID))
                {
                    SetImage(oFlexGrid, sCurID, bExpand, iRow + 1, iCol);
                }
                oFlexGrid.AutoRedraw = true;
                oFlexGrid.Refresh();
            }
        }
        public void SetImage(CustomFlexGrid oFlexGrid, string sParentID, bool bExpand, int iRow, int iCol)
        {
            Hashtable ht = new Hashtable();
            int iTotal = oFlexGrid.Rows;
            FlexCell.Cell oCell = null;
            FlexCell.Row oRow = null;
            string sCurID = string.Empty, sCurParentID = string.Empty;
            if (!string.IsNullOrEmpty(sParentID) && iTotal > iRow && oFlexGrid.Cols > iCol)
            {
                ht.Add(sParentID, "");
                for (int iStart = iRow; iStart < iTotal; iStart++)
                {
                    oRow = oFlexGrid.Row(iStart);
                    sCurID = oFlexGrid.Cell(iStart, 0).Tag;
                    sCurParentID = oFlexGrid.Cell(iStart, 1).Tag;
                    if (ht.ContainsKey(sCurParentID))
                    {
                        if (string.Equals(sCurParentID, sParentID))
                        {
                            oRow.Visible = bExpand;
                            oCell= oFlexGrid.Cell(iStart, iCol + 1);
                            if (!string.IsNullOrEmpty(oCell.ImageKey))
                            {
                                oFlexGrid.Cell(iStart, iCol + 1).SetImage( imageExpand);
                            }
                        }
                        else if (!bExpand)
                        {
                            oRow.Visible = bExpand;
                        }
                        if (!string.IsNullOrEmpty(sCurID))
                        {
                            ht.Add(sCurID,"");
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        public void InitFlexGrid(CustomFlexGrid oFlex, int iRows, string sTitle, bool autoRedraw)
        {
            if (oFlex == flexGrid)
            {
                InitFlexGridWBS(oFlex, iRows, sTitle, autoRedraw);
            }
            else if (oFlex == flexGridWBS)
            {
             InitFlexGridWBS(oFlex, iRows, sTitle, autoRedraw);
            }
            else if (oFlex == flexGridResource)
            {
                InitFlexGridResource(oFlex, iRows, sTitle, autoRedraw);
            }
        }
        private void FillFlex(IList<GWBSTreeTemp> lstTree)
        {
            if (tabQuery.SelectedTab == tabPage1)
            {
                FillFlexWBS(lstTree);
            }
            else if (tabQuery.SelectedTab == tabPage2)
            {
                FillFlexWBS(lstTree);
            }
            else if (tabQuery.SelectedTab == tabPage3)
            {
                FillFlexResource(lstTree);
            }
        }
        #region WBS报表
        public void InitFlexGridWBS(CustomFlexGrid oFlex, int iRows, string sTitle,bool autoRedraw )
        {
            int iCol, iRow, iImageCol = 19;
            FlexCell.Cell oCell = null;
            FlexCell.Column oColumn = null;
            if (autoRedraw)
            {
                oFlex.AutoRedraw = false;
            }
           
            oFlex.Rows = 1;
            oFlex.Cols = 1;
            oFlex.Locked = false;
            oFlex.Cell(0, 0).Locked = false;
            oFlex.Row(0).Locked = false;
            
            oFlex.Rows = 4 + iRows;
            oFlex.Cols = iImageCol + 1 + 9+5;//其中0列隐藏 1-19 为放置图片列 20-23为数据列
            oFlex.Column(0).Visible = false;
            oFlex.SelectionMode = FlexCell.SelectionModeEnum.ByCell;
            //oFlex.ExtendLastCol = true;
            oFlex.DisplayFocusRect = false;
            oFlex.LockButton = true;
            oFlex.ReadonlyFocusRect = FlexCell.FocusRectEnum.Solid;
            oFlex.BorderStyle = FlexCell.BorderStyleEnum.FixedSingle;
            oFlex.ScrollBars = FlexCell.ScrollBarsEnum.Both;
            oFlex.BackColorBkg = SystemColors.Control;
            oFlex.DefaultFont = new Font("Tahoma", 8);

            FlexCell.Range oRange;
            //标题
            iRow = 0; iCol = 1;
            oFlex.Row(iRow).Height = 30;
            oRange = oFlex.Range(iRow, iCol, iRow, oFlex.Cols - 1);
            oRange.Merge();
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = sTitle;
            oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            oCell.FontBold = true;
            oCell.FontSize = 16;
            //项目名称
            iRow += 1; iCol = 1;
            oRange = oFlex.Range(iRow, iCol, iRow, 5);
            oRange.Merge();
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "项目名称:";
            iCol = 5 + 1;
            oRange = oFlex.Range(iRow, iCol, iRow, oFlex.Cols - 1);
            oRange.Merge();
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = projectInfo == null ? "" : projectInfo.Name;
            oCell.Alignment = FlexCell.AlignmentEnum.LeftCenter;
            //任务名称
            iRow += 1; iCol = 1;
            for (int i = iCol; i < iImageCol + iCol; i++)
            {
                oFlex.Column(i).TabStop = false;
                oFlex.Column(i).Width = 20;
            }
            oRange = oFlex.Range(iRow, iCol, iRow + 1, iImageCol + iCol - 1);
            oRange.Merge();
            oFlex.Cell(iRow, iCol).Text = "任务名称";
            //oFlex.Cell(iRow, iCol).Alignment = FlexCell.AlignmentEnum.CenterCenter;
            // 加载图片
            oFlex.Images.Add(Resources.ImageExpend, imageExpand);
            oFlex.Images.Add(Resources.ImageFold, imageCollapse);
            #region 表头第二行设置
           
            //合同成本
            iCol = iImageCol + iCol + 1; //iCol + 3;
            oRange = oFlex.Range(iRow, iCol, iRow, iCol + 2);
            oRange.Merge();
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "合同收入";
            //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            //责任成本		
            iCol = iCol + 3;
            oRange = oFlex.Range(iRow, iCol, iRow, iCol + 2);
            oRange.Merge();
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "责任成本";
            //计划成本
            iCol = iCol + 3;
            oRange = oFlex.Range(iRow, iCol, iRow, iCol + 2);
            oRange.Merge();
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "计划成本";
            //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            //收支对比		
            iCol = iCol + 3;
            oRange = oFlex.Range(iRow, iCol, iRow, iCol + 1);
            oRange.Merge();
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "合同收入与计划对比";
            //责任与计划对比		
            iCol = iCol + 2;
            oRange = oFlex.Range(iRow, iCol, iRow, iCol + 1);
            oRange.Merge();
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "责任与计划对比";
            #endregion
            //单位
            iRow += 1; iCol = iImageCol + 1;
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "单位";
            //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            oColumn = oFlex.Column(iCol); oColumn.Width = 60; oColumn.CellType = FlexCell.CellTypeEnum.TextBox;
       
            #region 合同收入
             iCol += 1;            // 数量
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "工程量";
            oColumn = oFlex.Column(iCol); oColumn.Width = 60; oColumn.CellType = FlexCell.CellTypeEnum.TextBox; oColumn.Mask = FlexCell.MaskEnum.None; ;
            iCol += 1;            // 单价
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "单价";
            oColumn = oFlex.Column(iCol); oColumn.Width = 60; oColumn.CellType = FlexCell.CellTypeEnum.TextBox; oColumn.Mask = FlexCell.MaskEnum.None;
            iCol += 1;// 合价
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "合价(元)";
            oColumn = oFlex.Column(iCol); oColumn.Width = 100; oColumn.CellType = FlexCell.CellTypeEnum.TextBox; oColumn.Mask = FlexCell.MaskEnum.None; ;
            #endregion
            #region 责任成本
             iCol += 1;// 数量
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "工程量";
            oColumn = oFlex.Column(iCol); oColumn.Width = 60; oColumn.CellType = FlexCell.CellTypeEnum.TextBox; oColumn.Mask = FlexCell.MaskEnum.None; ;
            iCol += 1;// 单价
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "单价";
            oColumn = oFlex.Column(iCol); oColumn.Width = 60; oColumn.CellType = FlexCell.CellTypeEnum.TextBox; oColumn.Mask = FlexCell.MaskEnum.None; ;
            iCol += 1;     // 合价
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "合价(元)";
            oColumn = oFlex.Column(iCol); oColumn.Width = 100; oColumn.CellType = FlexCell.CellTypeEnum.TextBox; oColumn.Mask = FlexCell.MaskEnum.None; ;
            #endregion
            #region 计划成本
            iCol += 1;  // 数量
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "工程量";
            oColumn = oFlex.Column(iCol); oColumn.Width = 60; oColumn.CellType = FlexCell.CellTypeEnum.TextBox; oColumn.Mask = FlexCell.MaskEnum.None; ;
            iCol += 1; // 单价
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "单价";
            oColumn = oFlex.Column(iCol); oColumn.Width = 60; oColumn.CellType = FlexCell.CellTypeEnum.TextBox; oColumn.Mask = FlexCell.MaskEnum.None; ;
            iCol += 1; // 合价
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "合价(元)";
            oColumn = oFlex.Column(iCol); oColumn.Width = 100; oColumn.CellType = FlexCell.CellTypeEnum.TextBox; oColumn.Mask = FlexCell.MaskEnum.None; ;
            #endregion
            #region 利润
            iCol += 1;//利润
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "利润";
            oColumn = oFlex.Column(iCol); oColumn.Width = 80; oColumn.CellType = FlexCell.CellTypeEnum.TextBox; oColumn.Mask = FlexCell.MaskEnum.None;
            iCol += 1;//利润比例
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "利润比例(%)";
            oColumn = oFlex.Column(iCol); oColumn.Width = 80; oColumn.CellType = FlexCell.CellTypeEnum.TextBox; oColumn.Mask = FlexCell.MaskEnum.None;
            #endregion
            #region 节超
            iCol += 1; // 节超额
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "节超额";
            oColumn = oFlex.Column(iCol); oColumn.Width = 80; oColumn.CellType = FlexCell.CellTypeEnum.TextBox; oColumn.Mask = FlexCell.MaskEnum.None;
            iCol += 1;      //节超比例
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "节超比例(%)";
            oColumn = oFlex.Column(iCol); oColumn.Width = 80; oColumn.CellType = FlexCell.CellTypeEnum.TextBox; oColumn.Mask = FlexCell.MaskEnum.None;
            #endregion
            //oFlex.Locked = true;
            oRange = oFlex.Range(0, 0, oFlex.Rows - 1, oFlex.Cols - 1);
            oRange.set_Borders(FlexCell.EdgeEnum.Inside | FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            if (autoRedraw)
            {
                //1-19列的背景色
                FlexCell.Range range = oFlex.Range(1, 1, oFlex.Rows - 1,  iImageCol);
                if (range != null)
                {
                    range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                    range.BackColor = SystemColors.Control;
                }
                for (int i = iImageCol + 1; i < oFlex.Cols; i++)
                {
                    oFlex.Column(i).Locked = true;
                }
                for (iRow = 0; iRow < 4; iRow++)
                {
                    oFlex.Row(iRow).Locked = true;
                }
                range = oFlex.Range(2, iImageCol + 1, 3, oFlex.Cols - 1);
                if (range != null)
                {
                    range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
                }
                oFlex.AutoRedraw = true;
                oFlex.Refresh();
            }
        }
        private void FillFlexWBS(IList<GWBSTreeTemp> lstTree)
        {
            if (lstTree != null && lstTree.Count > 0)
            {

                CustomFlexGrid oFlexGrid = GetFlexGrid();
                oFlexGrid.AutoRedraw = false;
                InitFlexGrid(oFlexGrid, 1, GetTitle(), false);

                FlexCell.Cell oCell = null;
                int iExpandLevel = 1;
                //填充
                GWBSTreeTemp oParentTree = lstTree[0];
                int iRow = oFlexGrid.Rows - 1, iCol = 1, startLevel = 0, iImageCol = 19, iLevel = 0;
                FlexCell.Range rangeTemp = null;
                startLevel = oParentTree.Level;
                foreach (GWBSTreeTemp gt in lstTree)
                {

                    //加载工程任务
                    oFlexGrid.InsertRow(iRow, 1);
                    iCol = gt.Level - startLevel + 1;
                    if (gt.CategoryNodeType == NodeType.LeafNode && (gt.Details == null || gt.Details.Count == 0))//如果是叶节点 并且后面的没任务明细不显示树形图片
                    {
                        oFlexGrid.Row(iRow).Visible = (gt.Level - startLevel < iExpandLevel + 1) ? true : false;
                    }
                    else
                    {
                        oFlexGrid.Cell(iRow, iCol).SetImage((gt.Level - startLevel < iExpandLevel) ? imageCollapse : imageExpand);
                        oFlexGrid.Row(iRow).Visible = (gt.Level - startLevel < iExpandLevel + 1) ? true : false;
                        iCol += 1;
                    }
                    rangeTemp = oFlexGrid.Range(iRow, iCol, iRow, iImageCol);
                    rangeTemp.Merge();
                    oFlexGrid.Cell(iRow, 0).Tag = gt.Id;//存放ID
                    oFlexGrid.Cell(iRow, 1).Tag = gt.ParentNode.Id;//存放父节点ID
                    oCell = oFlexGrid.Cell(iRow, iCol);//存放wbs/明细任务名称
                    oCell.Text = "★" + gt.Name; oCell.FontBold = true;
                    iCol = iImageCol;
                    iCol += 1;//单位

                   
                    #region 合同收入
                   
                    iCol += 1;// 数量
                    oCell = oFlexGrid.Cell(iRow, iCol);
                    oCell.Text = gt.ContractQuantity.ToString("N2");
                    iCol += 1;// 单价
                    oCell = oFlexGrid.Cell(iRow, iCol);
                    iCol += 1;//合价
                    oCell = oFlexGrid.Cell(iRow, iCol);
                    oCell.Text = gt.ContractTotalPrice.ToString("N2");
                    #endregion
                    #region 责任成本
                   
                    iCol += 1; // 数量
                    oCell = oFlexGrid.Cell(iRow, iCol);
                    oCell.Text = gt.ResponsibilityQuantity.ToString("N2");
                    iCol += 1; // 单价
                    oCell = oFlexGrid.Cell(iRow, iCol);
                    iCol += 1;//合价
                    oCell = oFlexGrid.Cell(iRow, iCol);
                    oCell.Text = gt.ResponsibilityTotalPrice.ToString("N2");
                    #endregion
                    #region 计划收入
                   
                    iCol += 1; // 数量
                    oCell = oFlexGrid.Cell(iRow, iCol);
                    oCell.Text = gt.PlanQuantity.ToString("N2");
                    iCol += 1;// 单价
                    oCell = oFlexGrid.Cell(iRow, iCol);
                    iCol += 1;//合价
                    oCell = oFlexGrid.Cell(iRow, iCol);
                    oCell.Text = gt.PlanTotalPrice.ToString("N2");
                    #endregion
                    #region 利润
                    iCol += 1;//利润
                    oCell = oFlexGrid.Cell(iRow, iCol);
                    oCell.Text = gt.Profit.ToString("N2");
                    iCol += 1;
                    oCell = oFlexGrid.Cell(iRow, iCol);
                    oCell.Text = gt.ProfitRate.ToString("N2");//利润比例
                    oCell.Tag = "1";//如果wbs节点=1  如果该节点为wbs任务明细=0
                    #endregion

                    #region 节超额
                    iCol += 1;//节超额
                    oCell = oFlexGrid.Cell(iRow, iCol);
                    oCell.Text = gt.SaveMoney.ToString("N2");
                    iCol += 1;
                    oCell = oFlexGrid.Cell(iRow, iCol);
                    oCell.Text = gt.SaveRate.ToString("N2");//节超额比例
                    #endregion
                    if (gt.Details != null && gt.Details.Count > 0)
                    {
                        iLevel = gt.Level - startLevel + 2;
                        foreach (GWBSDetailTemp oDetail in gt.Details)
                        {
                            iRow += 1;
                            oFlexGrid.InsertRow(iRow, 1);
                            oFlexGrid.Cell(iRow, 0).Tag = "";
                            oFlexGrid.Cell(iRow, 1).Tag = gt.Id;
                            oFlexGrid.Row(iRow).Visible = (gt.Level - startLevel < iExpandLevel) ? true : false; ;
                            iCol = iLevel;
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            oCell.FontBold = false;
                            oCell.Text = oDetail.Name;
                            rangeTemp = oFlexGrid.Range(iRow, iLevel, iRow, iImageCol);
                            rangeTemp.Merge();
                            iCol = iImageCol + 1;
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            oCell.Text = oDetail.Unit;//单位
                         
                            #region 合同收入
                           
                            iCol += 1;// 数量
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            oCell.Text = oDetail.ContractQuantity.ToString("N2");
                            iCol += 1;// 单价
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            oCell.Text = oDetail.ContractPrice.ToString("N2");
                            iCol += 1;// 合价
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            oCell.Text = oDetail.ContractTotalPrice.ToString("N2");
                            #endregion
                            #region 责任成本
                           
                            iCol += 1;// 数量
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            oCell.Text = oDetail.ResponsibilityQuantity.ToString("N2");
                            iCol += 1;// 单价
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            oCell.Text = oDetail.ResponsibilityPrice.ToString("N2"); ;
                            iCol += 1;// 合价
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            oCell.Text = oDetail.ResponsibilityTotalPrice.ToString("N2");
                            #endregion
                            #region 计划
                           
                            iCol += 1;// 数量
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            oCell.Text = oDetail.PlanQuantity.ToString("N2");
                            iCol += 1;// 单价
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            oCell.Text = oDetail.PlanPrice.ToString("N2");
                            iCol += 1;// 合价
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            oCell.Text = oDetail.PlanTotalPrice.ToString("N2");
                            #endregion
                            #region 利润
                            iCol += 1;//利润
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            oCell.Text = oDetail.Profit.ToString("N2");
                            iCol += 1;//利润比例
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            oCell.Text = oDetail.ProfitRate.ToString("N2");
                            #endregion

                            #region 节超
                            iCol += 1;// 节超额
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            oCell.Text = oDetail.SaveMoney.ToString("N2");
                            iCol += 1;// 节超比例
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            oCell.Text = oDetail.SaveRate.ToString("N2");
                            #endregion
                        }
                    }
                    oParentTree = gt;
                    iRow += 1;
                }

                //1-19列的背景色
                FlexCell.Range range = oFlexGrid.Range(1, 1, oFlexGrid.Rows - 1, iImageCol);
                if (range != null)
                {
                    range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                    range.BackColor = SystemColors.Control;
                }
               
                range = oFlexGrid.Range(4, iImageCol + 1, oFlexGrid.Rows - 1, oFlexGrid.Cols - 1);
                if (range != null)
                {

                    range.Alignment = FlexCell.AlignmentEnum.RightCenter;
                    range.BackColor = Color.White;
                }
                oFlexGrid.Row(oFlexGrid.Rows - 1).Delete();
                //for (int i = iImageCol + 1; i < oFlexGrid.Cols; i++)
                //{
                //    oFlexGrid.Column(i).Locked = true;
                //}
                //for (iRow = 0; iRow < 4; iRow++)
                //{
                //    oFlexGrid.Row(iRow).Locked = true;
                //}
                for (int i = iImageCol+1; i < oFlexGrid.Cols; i++)
                {
                    oFlexGrid.Column(i).AutoFit();
                }
                oFlexGrid.Locked = true;
                oFlexGrid.FixedRows = 4;
                oFlexGrid.AutoRedraw = true;
                oFlexGrid.Refresh();
            }
        }
        #endregion
        #region Resource报表
        public void InitFlexGridResource(CustomFlexGrid oFlex, int iRows, string sTitle, bool autoRedraw)
        {
            int iCol, iRow, iImageCol = 19;
            FlexCell.Cell oCell = null;
            FlexCell.Column oColumn = null;
            if (autoRedraw)
            {
                oFlex.AutoRedraw = false;
            }

            oFlex.Rows = 1;
            oFlex.Cols = 1;
            oFlex.Locked = false;
            oFlex.Cell(0, 0).Locked = false;
            oFlex.Row(0).Locked = false;

            oFlex.Rows = 4 + iRows;
            oFlex.Cols = iImageCol + 1 + 9;//其中0列隐藏 1-19 为放置图片列 20-23为数据列
            oFlex.Column(0).Visible = false;
            oFlex.SelectionMode = FlexCell.SelectionModeEnum.ByCell;
            //oFlex.ExtendLastCol = true;
            oFlex.DisplayFocusRect = false;
            oFlex.LockButton = true;
            oFlex.ReadonlyFocusRect = FlexCell.FocusRectEnum.Solid;
            oFlex.BorderStyle = FlexCell.BorderStyleEnum.FixedSingle;
            oFlex.ScrollBars = FlexCell.ScrollBarsEnum.Both;
            oFlex.BackColorBkg = SystemColors.Control;
            oFlex.DefaultFont = new Font("Tahoma", 8);

            FlexCell.Range oRange;
            //标题
            iRow = 0; iCol = 1;
            oFlex.Row(iRow).Height = 30;
            oRange = oFlex.Range(iRow, iCol, iRow, oFlex.Cols - 1);
            oRange.Merge();
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = sTitle;
            oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            oCell.FontBold = true;
            oCell.FontSize = 16;
            //项目名称
            iRow += 1; iCol = 1;
            oRange = oFlex.Range(iRow, iCol, iRow, 5);
            oRange.Merge();
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "项目名称:";
            iCol = 5 + 1;
            oRange = oFlex.Range(iRow, iCol, iRow, oFlex.Cols - 1);
            oRange.Merge();
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = projectInfo == null ? "" : projectInfo.Name;
            oCell.Alignment = FlexCell.AlignmentEnum.LeftCenter;
            //任务名称
            iRow += 1; iCol = 1;
            for (int i = iCol; i < iImageCol + iCol; i++)
            {
                oFlex.Column(i).TabStop = false;
                oFlex.Column(i).Width = 20;
            }
            oRange = oFlex.Range(iRow, iCol, iRow + 1, iImageCol + iCol - 1);
            oRange.Merge();
            oFlex.Cell(iRow, iCol).Text = "任务名称";
            //oFlex.Cell(iRow, iCol).Alignment = FlexCell.AlignmentEnum.CenterCenter;
            // 加载图片
            oFlex.Images.Add(Resources.ImageExpend, imageExpand);
            oFlex.Images.Add(Resources.ImageFold, imageCollapse);
            //合同收入
            iCol = iImageCol + iCol + 1;
            oRange = oFlex.Range(iRow, iCol, iRow, iCol + 2);
            oRange.Merge();
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "合同收入";
            //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            //责任成本		
            iCol = iCol + 3;
            oRange = oFlex.Range(iRow, iCol, iRow, iCol + 2);
            oRange.Merge();
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "责任成本";
            //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;

            //单位
            iRow += 1; iCol = iImageCol + 1;
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "单位";
            //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            oColumn = oFlex.Column(iCol); oColumn.Width = 60; oColumn.CellType = FlexCell.CellTypeEnum.TextBox;
            //合同收入 单价
            iCol += 1;
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "单价";
            // oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            oColumn = oFlex.Column(iCol); oColumn.Width = 60; oColumn.CellType = FlexCell.CellTypeEnum.TextBox; oColumn.Mask = FlexCell.MaskEnum.None; ;
            //合同收入 数量
            iCol += 1;
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "数量";
            //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            oColumn = oFlex.Column(iCol); oColumn.Width = 60; oColumn.CellType = FlexCell.CellTypeEnum.TextBox; oColumn.Mask = FlexCell.MaskEnum.None; ;
            //合同收入 合价
            iCol += 1;
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "合价(元)";
            //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            oColumn = oFlex.Column(iCol); oColumn.Width = 100; oColumn.CellType = FlexCell.CellTypeEnum.TextBox; oColumn.Mask = FlexCell.MaskEnum.None; ;

            //责任成本 单价
            iCol += 1;
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "单价";
            //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            oColumn = oFlex.Column(iCol); oColumn.Width = 60; oColumn.CellType = FlexCell.CellTypeEnum.TextBox; oColumn.Mask = FlexCell.MaskEnum.None; ;
            //责任成本 数量
            iCol += 1;
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "数量";
            //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            oColumn = oFlex.Column(iCol); oColumn.Width = 60; oColumn.CellType = FlexCell.CellTypeEnum.TextBox; oColumn.Mask = FlexCell.MaskEnum.None; ;
            //责任成本 合价
            iCol += 1;
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "合价(元)";
            //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            oColumn = oFlex.Column(iCol); oColumn.Width = 100; oColumn.CellType = FlexCell.CellTypeEnum.TextBox; oColumn.Mask = FlexCell.MaskEnum.None; ;

            // 节超额
            iCol += 1;
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "节超额";
            //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            oColumn = oFlex.Column(iCol); oColumn.Width = 80; oColumn.CellType = FlexCell.CellTypeEnum.TextBox; oColumn.Mask = FlexCell.MaskEnum.None;
            //节超比例
            iCol += 1;
            oCell = oFlex.Cell(iRow, iCol);
            oCell.Text = "节超比例(%)";
            //oCell.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            oColumn = oFlex.Column(iCol); oColumn.Width = 80; oColumn.CellType = FlexCell.CellTypeEnum.TextBox; oColumn.Mask = FlexCell.MaskEnum.None;

            //oFlex.Locked = true;
            oRange = oFlex.Range(0, 0, oFlex.Rows - 1, oFlex.Cols - 1);
            oRange.set_Borders(FlexCell.EdgeEnum.Inside | FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            if (autoRedraw)
            {
                //1-19列的背景色
                FlexCell.Range range = oFlex.Range(1, 1, oFlex.Rows - 1, iImageCol);
                if (range != null)
                {
                    range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                    range.BackColor = SystemColors.Control;
                }
                for (int i = iImageCol + 1; i < oFlex.Cols; i++)
                {
                    oFlex.Column(i).Locked = true;
                }
                for (iRow = 0; iRow < 4; iRow++)
                {
                    oFlex.Row(iRow).Locked = true;
                }
                oFlex.AutoRedraw = true;
                oFlex.Refresh();
            }
        }
        private void FillFlexResource(IList<GWBSTreeTemp> lstTree)
        {
            if (lstTree != null && lstTree.Count > 0)
            {

                CustomFlexGrid oFlexGrid = GetFlexGrid();
                oFlexGrid.AutoRedraw = false;
                InitFlexGrid(oFlexGrid, 1, GetTitle(), false);

                FlexCell.Cell oCell = null;
                int iExpandLevel = 1;
                //填充
                GWBSTreeTemp oParentTree = lstTree[0];
                int iRow = oFlexGrid.Rows - 1, iCol = 1, startLevel = 0, iImageCol = 19, iLevel = 0;
                FlexCell.Range rangeTemp = null;
                startLevel = oParentTree.Level;
                foreach (GWBSTreeTemp gt in lstTree)
                {

                    //加载工程任务
                    oFlexGrid.InsertRow(iRow, 1);
                    iCol = gt.Level - startLevel + 1;
                    if (gt.CategoryNodeType == NodeType.LeafNode && (gt.Details == null || gt.Details.Count == 0))//如果是叶节点 并且后面的没任务明细不显示树形图片
                    {
                        oFlexGrid.Row(iRow).Visible = (gt.Level - startLevel < iExpandLevel + 1) ? true : false;
                    }
                    else
                    {
                        oFlexGrid.Cell(iRow, iCol).SetImage((gt.Level - startLevel < iExpandLevel) ? imageCollapse : imageExpand);
                        oFlexGrid.Row(iRow).Visible = (gt.Level - startLevel < iExpandLevel + 1) ? true : false;
                        iCol += 1;
                    }
                    rangeTemp = oFlexGrid.Range(iRow, iCol, iRow, iImageCol);
                    rangeTemp.Merge();
                    oFlexGrid.Cell(iRow, 0).Tag = gt.Id;//存放ID
                    oFlexGrid.Cell(iRow, 1).Tag = gt.ParentNode.Id;//存放父节点ID
                    oCell = oFlexGrid.Cell(iRow, iCol);//存放wbs/明细任务名称
                    oCell.Text = gt.Name;
                    iCol = iImageCol;
                    iCol += 1;//单位

                    iCol += 1;//合同收入 单价
                    oCell = oFlexGrid.Cell(iRow, iCol);
                    //oCell.Text = gt.ContractPrice.ToString("N2");
                    //合同收入 数量
                    iCol += 1;
                    oCell = oFlexGrid.Cell(iRow, iCol);
                    oCell.Text = gt.ContractQuantity.ToString("N2");
                    iCol += 1;
                    oCell = oFlexGrid.Cell(iRow, iCol);
                    oCell.Text = gt.ContractTotalPrice.ToString("N2");//合同合价
                    //oCell.Alignment = FlexCell.AlignmentEnum.RightGeneral;
                    // oCell.Mask = FlexCell.MaskEnum.Numeric;
                    //责任成本 单价
                    iCol += 1;
                    oCell = oFlexGrid.Cell(iRow, iCol);
                    // oCell.Text = gt.ResponsibilityPrice.ToString("N2");
                    //责任成本 数量
                    iCol += 1;
                    oCell = oFlexGrid.Cell(iRow, iCol);
                    oCell.Text = gt.ResponsibilityQuantity.ToString("N2");
                    iCol += 1;
                    oCell = oFlexGrid.Cell(iRow, iCol);
                    oCell.Text = gt.ResponsibilityTotalPrice.ToString("N2");//责任合价
                    //oCell.Alignment = FlexCell.AlignmentEnum.RightCenter;
                    // oCell.Mask = FlexCell.MaskEnum.Numeric;
                    iCol += 1;
                    oCell = oFlexGrid.Cell(iRow, iCol);
                    oCell.Text = gt.SaveMoney.ToString("N2");//节超额
                    //oCell.Alignment = FlexCell.AlignmentEnum.RightCenter;
                    // oCell.Mask = FlexCell.MaskEnum.Numeric;
                    iCol += 1;
                    oCell = oFlexGrid.Cell(iRow, iCol);
                    oCell.Text = gt.SaveRate.ToString("N2");//节超额比例
                    //oCell.Alignment = FlexCell.AlignmentEnum.RightCenter;
                    oCell.Tag = "1";//如果wbs节点=1  如果该节点为wbs任务明细=0
                    // oCell.Mask = FlexCell.MaskEnum.Numeric;
                    if (gt.Details != null && gt.Details.Count > 0)
                    {
                        iLevel = gt.Level - startLevel + 2;
                        foreach (GWBSDetailTemp oDetail in gt.Details)
                        {
                            iRow += 1;
                            oFlexGrid.InsertRow(iRow, 1);
                            oFlexGrid.Cell(iRow, 0).Tag = "";
                            oFlexGrid.Cell(iRow, 1).Tag = gt.Id;
                            oFlexGrid.Row(iRow).Visible = (gt.Level - startLevel < iExpandLevel) ? true : false; ;
                            iCol = iLevel;
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            oCell.Text = oDetail.Name;
                            rangeTemp = oFlexGrid.Range(iRow, iLevel, iRow, iImageCol);
                            rangeTemp.Merge();
                            iCol = iImageCol + 1;
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            oCell.Text = oDetail.Unit;//单位

                            iCol += 1;
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            oCell.Text = oDetail.ContractPrice.ToString("N2");//合同收入 单价
                            // oCell.Alignment = FlexCell.AlignmentEnum.RightCenter;
                            //  oCell.Mask = FlexCell.MaskEnum.Numeric;
                            iCol += 1;
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            oCell.Text = oDetail.ContractQuantity.ToString("N2");//合同收入 数量
                            //oCell.Alignment = FlexCell.AlignmentEnum.RightCenter;
                            //oCell.Mask = FlexCell.MaskEnum.Numeric;
                            iCol += 1;
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            oCell.Text = oDetail.ContractTotalPrice.ToString("N2");//合同收入 合价
                            // oCell.Alignment = FlexCell.AlignmentEnum.RightCenter;
                            // oCell.Mask = FlexCell.MaskEnum.Numeric;
                            iCol += 1;
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            oCell.Text = oDetail.ResponsibilityPrice.ToString("N2");//责任成本 单价
                            // oCell.Alignment = FlexCell.AlignmentEnum.RightCenter;
                            //oCell.Mask = FlexCell.MaskEnum.Numeric;
                            iCol += 1;
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            oCell.Text = oDetail.ResponsibilityQuantity.ToString("N2");//责任成本 数量
                            // oCell.Alignment = FlexCell.AlignmentEnum.RightCenter;
                            //oCell.Mask = FlexCell.MaskEnum.Numeric;
                            iCol += 1;
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            oCell.Text = oDetail.ResponsibilityTotalPrice.ToString("N2");//责任成本 合价
                            // oCell.Alignment = FlexCell.AlignmentEnum.RightCenter;
                            //oCell.Mask = FlexCell.MaskEnum.Numeric;
                            iCol += 1;
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            oCell.Text = oDetail.SaveMoney.ToString("N2");//责任成本 节超额
                            //oCell.Alignment = FlexCell.AlignmentEnum.RightCenter;
                            //oCell.Mask = FlexCell.MaskEnum.Numeric;
                            iCol += 1;
                            oCell = oFlexGrid.Cell(iRow, iCol);
                            // oCell.Mask = FlexCell.MaskEnum.Numeric;
                            oCell.Text = oDetail.SaveRate.ToString("N2");//责任成本 节超比例
                            //oCell.Alignment = FlexCell.AlignmentEnum.RightCenter;
                            // oCell.Mask = FlexCell.MaskEnum.Numeric;
                        }
                    }
                    oParentTree = gt;
                    iRow += 1;
                }

                //1-19列的背景色
                FlexCell.Range range = oFlexGrid.Range(1, 1, oFlexGrid.Rows - 1, iImageCol);
                if (range != null)
                {
                    range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                    range.BackColor = SystemColors.Control;
                }
                range = oFlexGrid.Range(2, iImageCol + 1, 3, oFlexGrid.Cols - 1);
                if (range != null)
                {
                    range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
                }
                range = oFlexGrid.Range(4, iImageCol + 1, oFlexGrid.Rows - 1, oFlexGrid.Cols - 1);
                if (range != null)
                {

                    range.Alignment = FlexCell.AlignmentEnum.RightCenter;
                    range.BackColor = Color.White;
                }
                oFlexGrid.Row(oFlexGrid.Rows - 1).Delete();
                //for (int i = iImageCol + 1; i < oFlexGrid.Cols; i++)
                //{
                //    oFlexGrid.Column(i).Locked = true;
                //}
                //for (iRow = 0; iRow < 4; iRow++)
                //{
                //    oFlexGrid.Row(iRow).Locked = true;
                //}
                oFlexGrid.Locked = true;
                oFlexGrid.FixedRows = 4;
                oFlexGrid.AutoRedraw = true;
                oFlexGrid.Refresh();
            }
        }
        #endregion
        public IList<GWBSTreeTemp> getTree(DataTable table)
        {
           
            IList<GWBSTreeTemp> lstTree = new List<GWBSTreeTemp>();
            Console.WriteLine(string.Format("查询结果:{0}",table.Rows.Count));
            getTree(null, lstTree, table,new Hashtable());
            Console.WriteLine(string.Format("最后结果:{0}", lstTree.Count));
            return lstTree;
        }
        public void getTree(GWBSTreeTemp oCurrNode, IList<GWBSTreeTemp> lstTree, DataTable table, Hashtable ht)
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
                {//t1.name,t1.id,t1.syscode,t1.parentnodeid,t1.categorynodetype,t1.tlevel,t1.orderno
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
                    ContractTotalPrice = 0,
                    ContractQuantity = 0,
                    ResponsibilityTotalPrice = 0,
                    ResponsibilityQuantity=0,
                    PlanTotalPrice=0,
                    PlanQuantity=0

                };
                if (ht.ContainsKey(oCurrNode.Id))
                {
                    return;
                }
                else
                {
                    ht.Add(oCurrNode.Id,"");
                    lstTree.Insert(lstTree.Count, oCurrNode);
                }
               
            }
           
            #region 查找子节点
            oRows = table.Select(string.Format("parentnodeid='{0}' ", oCurrNode.Id,(int)NodeType.MiddleNode), "OrderNo asc,TLevel asc ");
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
                    getTree(oChildNode, lstTree, table,ht);//获取当前节点的子节点下面的孩子
                    #region 求和算法
                    
                    oCurrNode.ContractTotalPrice += oChildNode.ContractTotalPrice;
                    oCurrNode.ContractQuantity += oChildNode.ContractQuantity;
                   // oCurrNode.ContractProgress += oChildNode.ContractProgress;
                   // oCurrNode.ResponsibilityProgress += oChildNode.ResponsibilityProgress;
                    oCurrNode.ResponsibilityTotalPrice += oChildNode.ResponsibilityTotalPrice;
                    oCurrNode.ResponsibilityQuantity += oChildNode.ResponsibilityQuantity;
                    oCurrNode.PlanTotalPrice += oChildNode.PlanTotalPrice;
                    oCurrNode.PlanQuantity += oChildNode.PlanQuantity;
                    //oCurrNode.PlanProgress += oChildNode.PlanProgress;
                    //oCurrNode.PlanTotalPrice += oChildNode.PlanTotalPrice;
                    #endregion
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
                        ID=ClientUtil.ToString(oDetailRow["detailID"]),
                        Name = ClientUtil.ToString(oDetailRow["dName"]),
                        // dtl.TheGWBS = new GWBSTree();
                        TheGWBS = new GWBSTreeTemp()
                        {
                            Id = ClientUtil.ToString(oDetailRow["id"]),
                            SysCode = ClientUtil.ToString(oDetailRow["SysCode"]),
                            CategoryNodeType = (NodeType)ClientUtil.ToInt(oDetailRow["CategoryNodeType"])
                        },
                        //dtl.WorkAmountUnitName = dataRow["workamountunitname"].ToString();
                        Unit = ClientUtil.ToString(oDetailRow["workamountunitname"]),
                        //责任成本
                        ResponsibilityQuantity = ClientUtil.ToDecimal(oDetailRow["responsibilitilyworkamount"]),
                        ResponsibilityPrice = ClientUtil.ToDecimal(oDetailRow["responsibilitilyprice"]),
                        ResponsibilityTotalPrice = ClientUtil.ToDecimal(oDetailRow["responsibilitilytotalprice"]),
                        //合同收入
                        ContractQuantity = ClientUtil.ToDecimal(oDetailRow["ContractProjectQuantity"]),
                        ContractPrice = ClientUtil.ToDecimal(oDetailRow["ContractPrice"]),
                        ContractTotalPrice = ClientUtil.ToDecimal(oDetailRow["ContractTotalPrice"]),
                        //计划成本
                        PlanQuantity = ClientUtil.ToDecimal(oDetailRow["PlanWorkAmount"]),
                        PlanPrice = ClientUtil.ToDecimal(oDetailRow["PlanPrice"]),
                        PlanTotalPrice = ClientUtil.ToDecimal(oDetailRow["PlanTotalPrice"]),

                        OrderNo = ClientUtil.ToInt(oDetailRow["dOrderno"])
                    };
                    htDetail.Add(oGWBSDetailTemp.ID,"");
                    lstDetails.Insert(lstDetails.Count, oGWBSDetailTemp);
                }
            }
            oCurrNode.Details = (lstDetails == null || lstDetails.Count == 0) ? null : lstDetails;
            #endregion

        }
       
       
        
        public CustomFlexGrid GetFlexGrid(){
             CustomFlexGrid oFlexGrid=null;
            if(tabQuery.SelectedTab==tabPage1)
            {
                oFlexGrid=flexGrid;
            }
            else if(tabQuery.SelectedTab==tabPage2){
                 oFlexGrid=flexGridWBS;
            }
            else if(tabQuery.SelectedTab==tabPage3){
                 oFlexGrid=flexGridResource;
            }
            return oFlexGrid;
        }
        public string GetTitle()
        {
            string sResult = null;
            if (tabQuery.SelectedTab == tabPage1)
            {
                sResult = string.Format("{0}汇总表", cmbValenceType.SelectedItem.ToString());
            }
            else if (tabQuery.SelectedTab == tabPage2)
            {
                sResult = string.Format("[{0}]工程成本维护收支对比分析表", projectInfo.Name);
               // sResult = string.Format("在[{0}]节点合同收入和责任成本对比表", txtGWBSTreeWBS.Tag == null ? "" : (txtGWBSTreeWBS.Tag as GWBSTree).Name);
            }
            else if (tabQuery.SelectedTab == tabPage3)
            {
                string sTemp = string.Empty;
                if (txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
                {
                    sTemp = string.Join("|",txtMaterialCategory.Result.OfType<MaterialCategory>().Select(cat => string.Format("{0}", cat.Name)).ToArray());
                    
                    sResult = string.Format("在[{0}]节点合同收入和责任成本对比表", sTemp);
                }
            }
            return sResult;
        }
        #endregion
    }
    #region 临时类
    public class GWBSTreeTemp : CategoryNode
    {
        IList<GWBSDetailTemp> lstDetails = null;
        public decimal ContractTotalPrice { get; set; }
        public decimal ContractPrice { get { return ContractQuantity == 0 ? 0 : Math.Round(ContractTotalPrice / ContractQuantity, 2); } }
        public decimal ContractQuantity { get; set; }
       // public decimal ContractMoney { get; set; }
       // public decimal ContractProgress { get; set; }
        //责任成本
        // public string ResponsibilityUnit { get; set; }
        public decimal ResponsibilityTotalPrice { get; set; }
        public decimal ResponsibilityPrice { get { return ResponsibilityQuantity == 0 ? 0 : Math.Round(ResponsibilityTotalPrice / ResponsibilityQuantity,2); } }
        public decimal ResponsibilityQuantity { get; set; }
       // public decimal ResponsibilityMoney { get; set; }
       // public decimal ResponsibilityProgress { get; set; }

        //计划成本
        // public string PlanUnit { get; set; }
        public decimal PlanTotalPrice { get; set; }
        public decimal PlanPrice { get { return PlanQuantity == 0 ? 0 : Math.Round(PlanTotalPrice / PlanQuantity, 2); } }
        public decimal PlanQuantity { get; set; }
      //  public decimal PlanMoney { get; set; }
       // public decimal PlanProgress { get; set; }
        public IList<GWBSDetailTemp> Details
        {
            get { return lstDetails; }
            set
            {
                lstDetails = value;
                this.ContractTotalPrice += (this.lstDetails == null ? 0 : this.lstDetails.Sum(Detail => Detail.ContractTotalPrice));
                this.ContractQuantity += this.lstDetails == null ? 0 : this.lstDetails.Sum(Detail => Detail.ContractQuantity);
                this.ResponsibilityTotalPrice += (this.lstDetails == null ? 0 : this.lstDetails.Sum(Detail => Detail.ResponsibilityTotalPrice));
                this.ResponsibilityQuantity += (this.lstDetails == null ? 0 : this.lstDetails.Sum(Detail => Detail.ResponsibilityQuantity));
                this.PlanTotalPrice += (this.lstDetails == null ? 0 : this.lstDetails.Sum(Detail => Detail.PlanTotalPrice));
                this.PlanQuantity += (this.lstDetails == null ? 0 : this.lstDetails.Sum(Detail => Detail.PlanQuantity));
            }
        }

        /// <summary>
        ///节超额=责任成本-计划成本
        /// </summary>
        public decimal SaveMoney
        {
            get
            {
                return  ResponsibilityTotalPrice-PlanTotalPrice;
            }
        }
        /// <summary>
        /// 节超例=节超额/责任成本
        /// </summary>
        public decimal SaveRate
        {
            get
            {
                return ResponsibilityTotalPrice == 0 ? 0 : decimal.Round(((SaveMoney * 100) / ResponsibilityTotalPrice), 2);
            }
        }
        /// <summary>
        /// 利润=合同收入金额-计划金额
        /// </summary>
        public decimal Profit
        {
            get { return this.ContractTotalPrice - this.PlanTotalPrice; }
        }
        /// <summary>
        /// 利润率=利润/合同收入金额
        /// </summary>
        public decimal ProfitRate
        {
            get { return this.ContractTotalPrice == 0 ? 0 : Math.Round((Profit * 100) / this.ContractTotalPrice, 2); }
        }
    }
    public class GWBSDetailTemp
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int OrderNo { get; set; }
        public GWBSTreeTemp TheGWBS { get; set; }
        public string Unit { get; set; }
        //合同收入
       //public string ContractUnit { get; set; }
        public decimal ContractTotalPrice { get; set; }
        public decimal ContractPrice { get; set; }
        public decimal ContractQuantity { get; set; }
        public decimal ContractMoney { get; set; }
        public decimal ContractProgress { get; set; }
        //责任成本
       // public string ResponsibilityUnit { get; set; }
        public decimal ResponsibilityTotalPrice { get; set; }
        public decimal ResponsibilityPrice { get; set; }
        public decimal ResponsibilityQuantity { get; set; }
        public decimal ResponsibilityMoney { get; set; }
        public decimal ResponsibilityProgress { get; set; }

        //计划成本
       // public string PlanUnit { get; set; }
        public decimal PlanTotalPrice { get; set; }
        public decimal PlanPrice { get; set; }
        public decimal PlanQuantity { get; set; }
        public decimal PlanMoney { get; set; }
        public decimal PlanProgress { get; set; }
        /// <summary>
        ///节超额=责任成本-计划成本
        /// </summary>
        public decimal SaveMoney
        {
            get
            {
                return ResponsibilityTotalPrice - PlanTotalPrice;
            }
        }
        /// <summary>
        /// 节超例=节超额/责任成本
        /// </summary>
        public decimal SaveRate
        {
            get
            {
                return ContractTotalPrice == 0 ? 0 : decimal.Round(((SaveMoney * 100) / ContractTotalPrice), 2);
            }
        }
        /// <summary>
        /// 利润=合同收入金额-计划金额
        /// </summary>
        public decimal Profit
        {
            get { return this.ContractTotalPrice - this.PlanTotalPrice; }
        }
        /// <summary>
        /// 利润率=利润/合同收入金额
        /// </summary>
        public decimal ProfitRate
        {
            get { return this.ContractTotalPrice == 0 ? 0 : Math.Round((Profit * 100) / this.ContractTotalPrice, 2); }
        }
    }
    #endregion
}
