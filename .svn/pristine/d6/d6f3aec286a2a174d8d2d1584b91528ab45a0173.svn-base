using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Util;
using VirtualMachine.Component.Util;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using NHibernate.Criterion;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;

namespace Application.Business.Erp.SupplyChain.Client.StockMng.Report
{
    public partial class WZReport : TBasicDataView
    {
        private MStockMng model = new MStockMng();
        //private string formatMoney = "################.##";
        private string formatMoney = "################.##";
        private string formatQuantity = "################.####";
        private bool IsSelectParts = false;
        #region 报表名称
        /// <summary>
        /// 材料收发存月报表
        /// </summary>
        private string RN_clsfcybb = "材料收发存月报表";
        /// <summary>
        /// 材料收发存月报表(核算科目)
        /// </summary>
        private string RN_clsfcybb_Subject = "材料收发存月报表(核算科目)";
        /// <summary>
        /// 商品砼收发存月报表
        /// </summary>
        private string RN_sptsfcybb = "商品砼收发存月报表";
        /// <summary>
        /// 材料收发存本期汇总表
        /// </summary>
        private string RN_clsfchzb_bq = "材料收发存本期汇总表";
        /// <summary>
        /// 材料收发存累计汇总表
        /// </summary>
        private string RN_clsfchzb_lj = "材料收发存累计汇总表";

        /// <summary>
        /// 调拨材料统计表(出库)
        /// </summary>
        private string RN_dbcltjb_ck = "调拨材料统计表(出库)";

        /// <summary>
        /// 调拨材料统计表(入库)
        /// </summary>
        private string RN_dbcltjb_rk = "调拨材料统计表(入库)";

        /// <summary>
        /// 甲供材料汇总表
        /// </summary>
        private string RN_dbcltjb_rk_jg = "甲供材料汇总表";

        /// <summary>
        /// 材料收发存台帐记录卡
        /// </summary>
        private string RN_clsfctz = "材料收发存台帐记录卡";
        /// <summary>
        /// 核算科目物资统计表
        /// </summary>
        private string RN_hskmwztj = "核算科目物资统计表";
        #endregion

        public WZReport()
        {
            
            InitializeComponent();
            this.dtpDateBegin.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtpDateBegin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateEnd.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtpDateEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            InitEvent();
            InitData();
            
        }

        private void InitData()
        {
            this.txtMaterialCategory.rootCatCode = "01";
            this.txtMaterialCategory.rootLevel = "3";
            txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode;

            this.dtpDateBegin.Text = ConstObject.TheLogin.TheComponentPeriod.BeginDate.ToShortDateString();

            cmbReportName.Items.Clear();
            cmbReportName.Items.Add(RN_hskmwztj);
            cmbReportName.Items.Add(RN_clsfcybb);
            cmbReportName.Items.Add(RN_clsfcybb_Subject);
            cmbReportName.Items.Add(RN_clsfchzb_bq);
            cmbReportName.Items.Add(RN_clsfchzb_lj);
            cmbReportName.Items.Add(RN_sptsfcybb);

            cmbReportName.Items.Add(RN_dbcltjb_ck);
            cmbReportName.Items.Add(RN_dbcltjb_rk);
            cmbReportName.Items.Add(RN_dbcltjb_rk_jg);
            cmbReportName.Items.Add(RN_clsfctz);
           
            cmbReportName.SelectedIndex = 0;

            CurrentProjectInfo pi = StaticMethod.GetProjectInfo();
            IList projectLst = new ArrayList();//这里可以查询所有的项目
            projectLst.Add(pi);
            cmbProject.DataSource = projectLst;
            cmbProject.DisplayMember = "Name";
            cmbProject.ValueMember = "Id";
            cmbProject.SelectedItem = pi;

            this.txtMaterial.Visible = false;
            this.lblMaterial.Visible = false;
        }

        private void InitEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            cmbReportName.SelectedIndexChanged += new EventHandler(cmbReportName_SelectedIndexChanged);
           this.btnSelectCostSubject .Click +=new EventHandler(btnSelectSubject_Click);
            this.btnSelectUsePart.Click +=new EventHandler(btnSelectPart_Click);

        }

        private void DisplayControl(int controlType, bool ifDisplay)
        {
            if (controlType == 1)//时间
            {
                this.lblDate.Visible = ifDisplay;
                this.lblTo.Visible = ifDisplay;
                this.dtpDateBegin.Visible = ifDisplay;
                this.dtpDateEnd.Visible = ifDisplay;
            }

            if (controlType == 2)//分类
            {
                this.lblCat.Visible = ifDisplay;
                this.txtMaterialCategory.Visible = ifDisplay;
            }

            if (controlType == 3)//物资
            {
                this.lblMaterial.Visible = ifDisplay;
                this.txtMaterial.Visible = ifDisplay;
            }

            if (controlType == 4)
            {
                this.txtSupplier.Visible = ifDisplay;
                this.lblSupplier.Visible = ifDisplay;
            }
            if (controlType == 5)
            {
                this.lblCostSubject.Visible = ifDisplay;
                this.lblUsePart.Visible = ifDisplay;
                this.txtCostSubject.Visible = ifDisplay;
                this.txtUsePart.Visible = ifDisplay;
                this.btnSelectCostSubject.Visible = ifDisplay;
                this.btnSelectUsePart.Visible = ifDisplay;

            }
            if (controlType == 6)
            {
                
                this.lblUsePart.Visible = ifDisplay;
               
                this.txtUsePart.Visible = ifDisplay;
                
                this.btnSelectUsePart.Visible = ifDisplay;

            }
        }

        void cmbReportName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string reportName = cmbReportName.SelectedItem + "";
            
            txtMaterialCategory .Value = string.Empty;
            txtMaterialCategory.Tag = null;
            txtMaterialCategory.Result = null;
            if (reportName == RN_clsfcybb)
            {
                LoadTempleteFile(@"物资报表\材料收发存月报表.flx");
                this.DisplayControl(1, true);
                this.DisplayControl(2, true);
                this.DisplayControl(3, false);
                this.DisplayControl(4, false);
                this.DisplayControl(5, false);
                this.DisplayControl(6, true );
                this.txtMaterialCategory.rootLevel = "6";
                txtMaterialCategory.IsCheckBox = true;
                this.IsSelectParts = true;
              
            }
            else if (reportName == RN_clsfcybb_Subject)
            {
                LoadTempleteFile(@"物资报表\材料收发存月报表.flx");
                this.DisplayControl(1, true);
                this.DisplayControl(2, true);
                this.DisplayControl(3, false);
                this.DisplayControl(4, false);
                this.DisplayControl(5, false);
                this.DisplayControl(6, false);
                this.txtMaterialCategory.rootLevel = "6";
                txtMaterialCategory.IsCheckBox = true;
                this.IsSelectParts = false ;

            }
            else if (reportName == RN_sptsfcybb)
            {
                LoadTempleteFile(@"物资报表\商品砼收发存月报表.flx");
                this.DisplayControl(1, true);
                this.DisplayControl(2, true);
                this.DisplayControl(3, false);
                this.DisplayControl(4, true);
                this.DisplayControl(5, false);
                this.DisplayControl(6, true );
                this.txtMaterialCategory.rootLevel = "3";
                txtMaterialCategory.IsCheckBox = false ;
                this.IsSelectParts = true ;
            }
            else if (reportName == RN_clsfchzb_bq)
            {
                LoadTempleteFile(@"物资报表\材料收发存本期汇总表.flx");
                this.DisplayControl(1, true);
                this.DisplayControl(2, false);
                this.DisplayControl(3, false);
                this.DisplayControl(4, false);
                this.DisplayControl(5, false);
                this.DisplayControl(6, true );
                this.txtMaterialCategory.rootLevel = "3";
                txtMaterialCategory.IsCheckBox = false ;
                this.IsSelectParts = true ;
            }
            else if (reportName == RN_clsfchzb_lj)
            {
                LoadTempleteFile(@"物资报表\材料收发存累计汇总表.flx");
                this.DisplayControl(1, true);
                this.DisplayControl(2, false);
                this.DisplayControl(3, false);
                this.DisplayControl(4, false);
                this.DisplayControl(5, false);
                this.DisplayControl(6, true);
                this.txtMaterialCategory.rootLevel = "3";
                txtMaterialCategory.IsCheckBox = false ;
                this.IsSelectParts = true ;
            }
            else if (reportName==RN_dbcltjb_rk)
            {
                LoadTempleteFile(@"物资报表\调拨材料统计表.flx");
                this.DisplayControl(1, true);
                this.DisplayControl(2, false);
                this.DisplayControl(3, false);
                this.DisplayControl(4, false);
                this.DisplayControl(5, false);
                this.DisplayControl(6, false);
                this.txtMaterialCategory.rootLevel = "3";
                txtMaterialCategory.IsCheckBox = false ;
                this.IsSelectParts = false;
            }
            else if (reportName == RN_dbcltjb_ck)
            {
                LoadTempleteFile(@"物资报表\调拨材料统计表_出库.flx");
                this.DisplayControl(1, true);
                this.DisplayControl(2, false);
                this.DisplayControl(3, false);
                this.DisplayControl(4, false);
                this.DisplayControl(5, false);
                this.DisplayControl(6, false);
                this.txtMaterialCategory.rootLevel = "3";
                txtMaterialCategory.IsCheckBox = false ;
                this.IsSelectParts = false;
            }
            else if (reportName == RN_dbcltjb_rk_jg)
            {
                LoadTempleteFile(@"物资报表\甲供料汇总表.flx");
                this.DisplayControl(1, true);
                this.DisplayControl(2, false);
                this.DisplayControl(3, false);
                this.DisplayControl(4, false);
                this.DisplayControl(5, false);
                this.DisplayControl(6, false);
                this.txtMaterialCategory.rootLevel = "3";
                txtMaterialCategory.IsCheckBox = false;
                this.IsSelectParts = false;
            }
            else if (reportName == RN_clsfctz)
            {
                LoadTempleteFile(@"物资报表\材料收发存台帐记录卡.flx");
                this.DisplayControl(1, true);
                this.DisplayControl(2, false);
                this.DisplayControl(3, true);
                this.DisplayControl(4, false);
                this.DisplayControl(5, false);
                this.DisplayControl(6, false);
                this.txtMaterialCategory.rootLevel = "3";
                txtMaterialCategory.IsCheckBox = false;
                this.IsSelectParts = false;
            }
            else if (reportName == RN_hskmwztj)
            {
                LoadTempleteFile(@"物资报表\核算科目物资统计.flx");
                this.DisplayControl(1, true);
                this.DisplayControl(2, false);
                this.DisplayControl(3, true);
                this.DisplayControl(4, false);
                this.DisplayControl(5, true );
                this.DisplayControl(6, true );
                this.txtMaterialCategory.rootLevel = "3";
                txtMaterialCategory.IsCheckBox = false;
                this.IsSelectParts = false;
            }
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
             
            string reportName = cmbReportName.SelectedItem + "";
            if (ClientUtil.ToDateTime(this.dtpDateBegin.Text) > ClientUtil.ToDateTime(this.dtpDateEnd.Text))
            {
                MessageBox.Show("统计开始时间不能晚于结束时间！");
                return;
            }

            if (reportName == this.RN_clsfcybb)
            {
                LoadTempleteFile(@"物资报表\材料收发存月报表.flx");
                FillReport_clsfcybb();
            }
            else if (reportName == this.RN_clsfcybb_Subject )
            {
                LoadTempleteFile(@"物资报表\材料收发存月报表.flx");
                FillReport_clsfcybb();
            }
            else if (reportName == this.RN_sptsfcybb)
            {
                LoadTempleteFile(@"物资报表\商品砼收发存月报表.flx");
                FillReport_sptsfcybb();
            }
            else if (reportName == this.RN_clsfchzb_bq)
            {
                LoadTempleteFile(@"物资报表\材料收发存本期汇总表.flx");
                FillReport_clsfchzb();
            }
            else if (reportName == this.RN_clsfchzb_lj)
            {
                LoadTempleteFile(@"物资报表\材料收发存累计汇总表.flx");
                FillReport_clsfchzb();
            }
            else if (reportName == RN_dbcltjb_rk)
            {
                LoadTempleteFile(@"物资报表\调拨材料统计表.flx");
                FillReport_db();
            }
            else if (reportName == RN_dbcltjb_ck)
            {
                LoadTempleteFile(@"物资报表\调拨材料统计表_出库.flx");
                FillReport_db();
            }
            else if (reportName == RN_dbcltjb_rk_jg)
            {
                LoadTempleteFile(@"物资报表\甲供料汇总表.flx");
                FillReport_db();
            }
            else if (reportName == RN_clsfctz)
            {
                LoadTempleteFile(@"物资报表\材料收发存台帐记录卡.flx");
                FillReport_clsfctz();
            }
            else if (reportName == RN_hskmwztj)
            {
                 LoadTempleteFile(@"物资报表\核算科目物资统计.flx");
                 FillReport_hskmwztj();
            }
        }
        void btnSelectSubject_Click(object sender, EventArgs e)
        {
            VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
            frm.IsLeafSelect = false;
            frm.ShowDialog();
            CostAccountSubject cost = frm.SelectAccountSubject;
            if (cost != null)
            {
                this.txtCostSubject .Text = cost.Name;
                this.txtCostSubject.Tag = cost;
            }
        }
        void btnSelectPart_Click(object sender, EventArgs e)
        { 
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsTreeSelect = true;

            frm.IsCheck = IsSelectParts;
            frm.IsRootNode = IsSelectParts;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                if (IsSelectParts)
                {
                    GWBSTree task = null;
                    string sUsePartName = string.Empty;
                    IList list =new ArrayList ();
                    foreach (TreeNode oNode in frm.SelectResult)
                    {
                        task = oNode.Tag as GWBSTree;
                        if (task != null)
                        {
                            if (!string.IsNullOrEmpty(sUsePartName))
                            {
                                sUsePartName += " | " + task.Name;

                            }
                            else
                            {
                                sUsePartName = task.Name;
                            }
                            list .Add (task);
                        }
                    }
                    this.txtUsePart.Text = sUsePartName;
                    this.txtUsePart.Tag = list;
                }
                else
                {
                    TreeNode root = frm.SelectResult[0];
                    GWBSTree task = root.Tag as GWBSTree;
                    if (task != null)
                    {
                        this.txtUsePart.Text = task.Name;
                        this.txtUsePart.Tag = task;
                        TextBox td = new TextBox();
                       

                    }
                }
                
            }
        }
       
        void btnExcel_Click(object sender, EventArgs e)
        {
            string reportNmae = cmbReportName.SelectedItem + "";
            reportGrid.ExportToExcel(reportNmae, false, false, true);
        }

        private bool LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式和数据
                reportGrid.OpenFile(path + "\\" + modelName);//载入格式
            }
            else
            {
                MessageBox.Show("未找到模板格式文件【" + modelName + "】");
                return false;
            }
            return true;
        }

        private int MachedRowIndex(string matchKey)
        {
            for (int i = 0; i < reportGrid.Rows; i++)
            {
                string tempMatchKey = reportGrid.Cell(i, 1).Tag;
                if (tempMatchKey == matchKey)
                {
                    return i;
                }
            }

            return int.MinValue;
        }

        private void InsertGridCol(int startCol,int tableHeaderStartRow, string usedPartName)
        {
            reportGrid.InsertCol(startCol, 4);
            FlexCell.Range range = reportGrid.Range(tableHeaderStartRow, startCol, tableHeaderStartRow, startCol + 3);
            range.MergeCells = true;
            reportGrid.Cell(tableHeaderStartRow, startCol).Text = "消耗明细";
            range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);

            range = reportGrid.Range(tableHeaderStartRow + 1, startCol, tableHeaderStartRow + 1, startCol + 3);
            range.MergeCells = true;
            reportGrid.Cell(tableHeaderStartRow + 1, startCol).Text = usedPartName;
            range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);

            range = reportGrid.Range(tableHeaderStartRow + 2, startCol, tableHeaderStartRow + 2, startCol + 3);
            range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            reportGrid.Cell(tableHeaderStartRow + 2, startCol).Text = "本期数量";
            reportGrid.Cell(tableHeaderStartRow + 2, startCol + 1).Text = "本期金额";
            reportGrid.Cell(tableHeaderStartRow + 2, startCol + 2).Text = "累计数量";
            reportGrid.Cell(tableHeaderStartRow + 2, startCol + 3).Text = "累计金额";
        }

        private void InsertGridCol4Clsfchzb(int startCol, int tableHeaderStartRow, string usedPartName)
        {
            reportGrid.InsertCol(startCol, 2);
            FlexCell.Range range = reportGrid.Range(tableHeaderStartRow, startCol, tableHeaderStartRow, startCol + 1);
            range.MergeCells = true;
            reportGrid.Cell(tableHeaderStartRow, startCol).Text = "消耗明细";
            range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);

            range = reportGrid.Range(tableHeaderStartRow + 1, startCol, tableHeaderStartRow + 1, startCol + 1);
            range.MergeCells = true;
            reportGrid.Cell(tableHeaderStartRow + 1, startCol).Text = usedPartName;
            range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);

            range = reportGrid.Range(tableHeaderStartRow + 2, startCol, tableHeaderStartRow + 2, startCol + 1);
            range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            reportGrid.Cell(tableHeaderStartRow + 2, startCol).Text = "数量";
            reportGrid.Cell(tableHeaderStartRow + 2, startCol + 1).Text = "金额";
        }

        private static IList tempUsedPartLst;
        private IList GetUsedPart(string projectId)
        {
            if (tempUsedPartLst != null && tempUsedPartLst.Count > 0)
            {
                return tempUsedPartLst;
            }

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", projectId));
            oq.AddCriterion(Expression.Eq("WarehouseFlag", true));
            oq.AddOrder(Order.Asc("Code"));

            tempUsedPartLst= model.StockInSrv.GetObjects(typeof(GWBSTree), oq);
            return tempUsedPartLst;
        }
        private void FillReport_hskmwztj()
        {

            CurrentProjectInfo pi = cmbProject.SelectedItem as CurrentProjectInfo;
            string beginDate = dtpDateBegin.Value.ToShortDateString();
            string endDate = dtpDateEnd.Value.ToShortDateString();
            if (string.IsNullOrEmpty(this.txtCostSubject.Text) || txtCostSubject.Tag == null)
            {
                MessageBox.Show("请先选择一个核算科目。");
                this.txtCostSubject.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.txtUsePart.Text) || txtUsePart.Tag == null)
            {
                MessageBox.Show("请先选择一个使用部位。");
                this.txtUsePart.Focus();
                return;
            }
            CostAccountSubject oCostAccountSubject = this.txtCostSubject .Tag as CostAccountSubject;
            GWBSTree oGWBSTree = this.txtUsePart .Tag as GWBSTree;
            DataSet ds = model.StockInOutSrv.WZBB_skmwztj(beginDate, endDate, pi.Id, oCostAccountSubject.SysCode, oGWBSTree.SysCode);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
            DataTable oTable=ds.Tables [0];
            string sMaterialTop = string.Empty;
            string sMaterialTemp=string.Empty ;
            string sMaterialName=string.Empty ;
            string sMaterialUnit=string.Empty ;
            string sMaterialSpec=string.Empty ;
            decimal dTotalQty=0;
            decimal dTotalMoney=0;
            decimal dSumQty = 0;
            decimal dSumMoney = 0;
            decimal dQty= 0;
            decimal dMoney=0;
            bool IsTotal = false;

            int iRowBasci=4;
            reportGrid.Cell(iRowBasci - 2, 3).Text = pi.Name;
            reportGrid.Cell(iRowBasci - 2,5).Text = oCostAccountSubject .Name ;
             reportGrid.Cell(iRowBasci - 2,7).Text=beginDate+" 至 "+ endDate;
            for (int i = 0; i < oTable.Rows.Count; i++)
            {
                
                 sMaterialTemp=oTable.Rows[i]["id"].ToString ();
                 sMaterialName = oTable.Rows[i]["materialname"].ToString();
                 sMaterialUnit = oTable.Rows[i]["matstandardunitname"].ToString();
sMaterialSpec = oTable.Rows[i]["materialspec"].ToString();
dQty = ClientUtil.ToDecimal(oTable.Rows[i]["qty"]);
dMoney = ClientUtil.ToDecimal(oTable.Rows[i]["money"].ToString());
               
                IsTotal = false;
                if (string.IsNullOrEmpty(sMaterialTop))
                {
                    sMaterialTop = sMaterialTemp;
                }
                else
                {
                    if (!string.Equals(sMaterialTop, sMaterialTemp))
                    {
                        sMaterialTop = sMaterialTemp;
                        IsTotal = true;
                    }
                }
                if (IsTotal)
                {
                    reportGrid.InsertRow(iRowBasci + i, 1);
                    reportGrid.Cell(iRowBasci + i, 1).Text = "合计";
                    reportGrid.Cell(iRowBasci + i, 5).Text = CommonUtil.NumberToStringFormate(dSumQty , 4); 
                    dTotalQty += dSumMoney;
                    reportGrid.Cell(iRowBasci + i, 6).Text = CommonUtil.NumberToStringFormate(dSumMoney, 2); 
                    dTotalMoney += dSumMoney;
                    iRowBasci++;
                }
                reportGrid.InsertRow(iRowBasci + i , 1);
                reportGrid.Cell(iRowBasci + i, 1).Text = (i+1).ToString ();
                reportGrid.Cell(iRowBasci + i, 2).Text = sMaterialName;
                reportGrid.Cell(iRowBasci + i, 3).Text = sMaterialUnit;
                reportGrid.Cell(iRowBasci + i, 4).Text = sMaterialSpec;
                reportGrid.Cell(iRowBasci + i, 5).Text = CommonUtil .NumberToStringFormate (dQty ,4);
                dSumQty  += dQty;
                reportGrid.Cell(iRowBasci + i, 6).Text = CommonUtil.NumberToStringFormate(dMoney, 2);
                dSumMoney  += dMoney;

                if (i+1 == oTable.Rows.Count)
                {
                    iRowBasci++;
                    reportGrid.InsertRow(iRowBasci + i, 1);
                    reportGrid.Cell(iRowBasci + i, 1).Text = "合计";
                    reportGrid.Cell(iRowBasci + i, 5).Text = CommonUtil.NumberToStringFormate(dSumQty, 4);
                    dTotalQty += dSumMoney;
                    reportGrid.Cell(iRowBasci + i, 6).Text = CommonUtil.NumberToStringFormate(dSumMoney, 2);
                    dTotalMoney += dSumMoney;
                    iRowBasci++;

                    reportGrid.InsertRow(iRowBasci + i, 1);
                    reportGrid.Cell(iRowBasci + i, 1).Text = "总合计";
                    reportGrid.Cell(iRowBasci + i, 5).Text = CommonUtil.NumberToStringFormate(dTotalQty, 4);
                    //dTotalQty += dSumMoney;
                    reportGrid.Cell(iRowBasci + i, 6).Text = CommonUtil.NumberToStringFormate(dTotalMoney, 2);
                    //dTotalMoney += dSumMoney;
                    //iRowBasci++;
                }
               

            }

            FlexCell.Range range = reportGrid.Range(iRowBasci, 1, iRowBasci + oTable.Rows.Count -1, 3);
            range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);

            range = reportGrid.Range(iRowBasci, 4, iRowBasci + oTable.Rows.Count - 1, 6);
            range.Alignment = FlexCell.AlignmentEnum.RightCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);


        }
        /// <summary>
        /// 材料收发存月报表
        /// </summary>
        private void FillReport_clsfcybb()
        {
            CurrentProjectInfo pi = cmbProject.SelectedItem as CurrentProjectInfo;
            string beginDate = dtpDateBegin.Value.ToShortDateString();
            string endDate = dtpDateEnd.Value.ToShortDateString();
            if (txtMaterialCategory.Text == "" || txtMaterialCategory.Result == null || txtMaterialCategory.Result.Count == 0)
            {
                MessageBox.Show("请先选择一个物资分类。");
                txtMaterialCategory.Focus();
                return;
            }
            IList lstMaterialCategory = txtMaterialCategory.Result ;
            DataSet ds = model.StockInOutSrv.WZBB_clsfcybb(beginDate, endDate, pi.Id, lstMaterialCategory);
            //reportGrid.Column(0).
            if (ds == null || ds.Tables.Count == 0) return;
            DataTable dt = ds.Tables[0];
            if (dt == null) return;

            reportGrid.Cell(2, 1).Text = "单位名称：" + pi.Name;
            reportGrid.Cell(2, 13).Text = "材料类别：" + txtMaterialCategory.Text ;
            reportGrid.Cell(2, 22).Text = "统计日期：" + beginDate + " 至 " + endDate;

            int detailStartRowNumber = 6;//6为模板中的行号
            int detailCount = dt.Rows.Count;
            if (detailCount == 0) return;
            //插入报表行
            reportGrid.InsertRow(detailStartRowNumber, detailCount);
            decimal sumLastQty = 0M, sumLastMoney = 0M, sumCgsl = 0M, sumCgje = 0M, sumLjcgsl = 0M, sumLjcgje = 0M, sumDbrksl = 0M, sumDbrkje = 0M, sumLjdbrksl = 0M, sumLjdbrkje = 0M,
                sumPysl = 0M, sumPyje = 0M, sumLjpysl = 0M, sumLjpyje = 0M, sumXhsl = 0M, sumXhje = 0M, sumLjxhsl = 0M, sumLjxhje = 0M, sumDbcksl = 0M, sumDbckje = 0M, sumLjdbcksl = 0M,
                sumLjdbckje = 0M, sumPksl = 0M, sumPkje = 0M, sumLjpksl = 0M, sumLjpkje = 0M, sumJcsl = 0M, sumJcje = 0M;
            for (int i = 0; i < detailCount; i++)
            {
                DataRow dr = dt.Rows[i];
                string materialId = dr["materialid"] + "";
                string matcode = dr["matcode"] + "";
                string matspec = dr["matspec"] + "";
                string matname = dr["matname"] + "";
                string unitname = dr["unitname"] + "";
                decimal lastQty = ClientUtil.ToDecimal(dr["lastQty"]);
                decimal lastMoney = ClientUtil.ToDecimal(dr["lastMoney"]);
                decimal cgsl = ClientUtil.ToDecimal(dr["cgsl"]);
                decimal cgje = ClientUtil.ToDecimal(dr["cgje"]);
                decimal ljcgsl = ClientUtil.ToDecimal(dr["ljcgsl"]);
                decimal ljcgje = ClientUtil.ToDecimal(dr["ljcgje"]);
                decimal dbrksl = ClientUtil.ToDecimal(dr["dbrksl"]);
                decimal dbrkje = ClientUtil.ToDecimal(dr["dbrkje"]);
                decimal ljdbrksl = ClientUtil.ToDecimal(dr["ljdbrksl"]);
                decimal ljdbrkje = ClientUtil.ToDecimal(dr["ljdbrkje"]);
                decimal pysl = ClientUtil.ToDecimal(dr["pysl"]);
                decimal pyje = ClientUtil.ToDecimal(dr["pyje"]);
                decimal ljpysl = ClientUtil.ToDecimal(dr["ljpysl"]);
                decimal ljpyje = ClientUtil.ToDecimal(dr["ljpyje"]);
                decimal xhsl = ClientUtil.ToDecimal(dr["xhsl"]);
                decimal xhje = ClientUtil.ToDecimal(dr["xhje"]);
                decimal ljxhsl = ClientUtil.ToDecimal(dr["ljxhsl"]);
                decimal ljxhje = ClientUtil.ToDecimal(dr["ljxhje"]);
                decimal dbcksl = ClientUtil.ToDecimal(dr["dbcksl"]);
                decimal dbckje = ClientUtil.ToDecimal(dr["dbckje"]);
                decimal ljdbcksl = ClientUtil.ToDecimal(dr["ljdbcksl"]);
                decimal ljdbckje = ClientUtil.ToDecimal(dr["ljdbckje"]);
                decimal pksl = ClientUtil.ToDecimal(dr["pksl"]);
                decimal pkje = ClientUtil.ToDecimal(dr["pkje"]);
                decimal ljpksl = ClientUtil.ToDecimal(dr["ljpksl"]);
                decimal ljpkje = ClientUtil.ToDecimal(dr["ljpkje"]);
                decimal jcsl = ClientUtil.ToDecimal(dr["jcsl"]);
                decimal jcje = ClientUtil.ToDecimal(dr["jcje"]);
                sumLastQty += lastQty;
                sumLastMoney += lastMoney;
                sumCgsl += cgsl;
                sumCgje += cgje;
                sumLjcgsl += ljcgsl;
                sumLjcgje += ljcgje;
                sumDbrksl += dbrksl;
                sumDbrkje += dbrkje;
                sumLjdbrksl += ljdbrksl;
                sumLjdbrkje += ljdbrkje;
                sumPysl += pysl;
                sumPyje += pyje;
                sumLjpysl += ljpysl;
                sumLjpyje += ljpyje;
                sumXhsl += xhsl;
                sumXhje += xhje;
                sumLjxhsl += ljxhsl;
                sumLjxhje += ljxhje;
                sumDbcksl += dbcksl;
                sumDbckje += dbckje;
                sumLjdbcksl += ljdbcksl;
                sumLjdbckje += ljdbckje;
                sumPksl += pksl;
                sumPkje += pkje;
                sumLjpksl += ljpksl;
                sumLjpkje += ljpkje;
                sumJcsl += jcsl;
                sumJcje += jcje;
                reportGrid.Cell(detailStartRowNumber + i, 1).Text = matname;//物资名称
                reportGrid.Cell(detailStartRowNumber + i, 1).Tag = materialId;//物资名称
                reportGrid.Cell(detailStartRowNumber + i, 2).Text = matspec;//规格
                reportGrid.Cell(detailStartRowNumber + i, 3).Text = unitname;//单位
                reportGrid.Cell(detailStartRowNumber + i, 4).Text = CommonUtil.NumberToStringFormate(lastQty,4);//
                reportGrid.Cell(detailStartRowNumber + i, 5).Text = CommonUtil.NumberToStringFormate(lastMoney,2);//
                reportGrid.Cell(detailStartRowNumber + i, 6).Text = CommonUtil.NumberToStringFormate(cgsl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 7).Text = CommonUtil.NumberToStringFormate(cgje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 8).Text = CommonUtil.NumberToStringFormate(ljcgsl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 9).Text = CommonUtil.NumberToStringFormate(ljcgje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 10).Text = CommonUtil.NumberToStringFormate(dbrksl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 11).Text = CommonUtil.NumberToStringFormate(dbrkje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 12).Text = CommonUtil.NumberToStringFormate(ljdbrksl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 13).Text = CommonUtil.NumberToStringFormate(ljdbrkje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 14).Text = CommonUtil.NumberToStringFormate(xhsl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 15).Text = CommonUtil.NumberToStringFormate(xhje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 16).Text = CommonUtil.NumberToStringFormate(ljxhsl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 17).Text = CommonUtil.NumberToStringFormate(ljxhje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 18).Text = CommonUtil.NumberToStringFormate(dbcksl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 19).Text = CommonUtil.NumberToStringFormate(dbckje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 20).Text = CommonUtil.NumberToStringFormate(ljdbcksl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 21).Text = CommonUtil.NumberToStringFormate(ljdbckje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 22).Text = "";//
                reportGrid.Cell(detailStartRowNumber + i, 23).Text = CommonUtil.NumberToStringFormate(pkje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 24).Text = "";//
                reportGrid.Cell(detailStartRowNumber + i, 25).Text = CommonUtil.NumberToStringFormate(ljpkje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 26).Text = CommonUtil.NumberToStringFormate(jcsl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 27).Text = CommonUtil.NumberToStringFormate(jcje,2);//
            }
            //插入合计
            int summaryRowIndex = detailStartRowNumber + detailCount;//合计行行号
            reportGrid.Cell(summaryRowIndex, 1).Text = "合计";//物资名称
            reportGrid.Cell(summaryRowIndex, 4).Text = CommonUtil.NumberToStringFormate(sumLastQty,4);//
            reportGrid.Cell(summaryRowIndex, 5).Text = CommonUtil.NumberToStringFormate(sumLastMoney,2);//
            reportGrid.Cell(summaryRowIndex, 6).Text = CommonUtil.NumberToStringFormate(sumCgsl,4);//
            reportGrid.Cell(summaryRowIndex, 7).Text = CommonUtil.NumberToStringFormate(sumCgje,2);//
            reportGrid.Cell(summaryRowIndex, 8).Text = CommonUtil.NumberToStringFormate(sumLjcgsl,4);//
            reportGrid.Cell(summaryRowIndex, 9).Text = CommonUtil.NumberToStringFormate(sumLjcgje,2);//
            reportGrid.Cell(summaryRowIndex, 10).Text = CommonUtil.NumberToStringFormate(sumDbrksl,4);//
            reportGrid.Cell(summaryRowIndex, 11).Text = CommonUtil.NumberToStringFormate(sumDbrkje,2);//
            reportGrid.Cell(summaryRowIndex, 12).Text = CommonUtil.NumberToStringFormate(sumLjdbrksl,4);//
            reportGrid.Cell(summaryRowIndex, 13).Text = CommonUtil.NumberToStringFormate(sumLjdbrkje,2);//
            reportGrid.Cell(summaryRowIndex, 14).Text = CommonUtil.NumberToStringFormate(sumXhsl,4);//
            reportGrid.Cell(summaryRowIndex, 15).Text = CommonUtil.NumberToStringFormate(sumXhje,2);//
            reportGrid.Cell(summaryRowIndex, 16).Text = CommonUtil.NumberToStringFormate(sumLjxhsl,4);//
            reportGrid.Cell(summaryRowIndex, 17).Text = CommonUtil.NumberToStringFormate(sumLjxhje,2);//
            reportGrid.Cell(summaryRowIndex, 18).Text = CommonUtil.NumberToStringFormate(sumDbcksl,4);//
            reportGrid.Cell(summaryRowIndex, 19).Text = CommonUtil.NumberToStringFormate(sumDbckje,2);//
            reportGrid.Cell(summaryRowIndex, 20).Text = CommonUtil.NumberToStringFormate(sumLjdbcksl,4);//
            reportGrid.Cell(summaryRowIndex, 21).Text = CommonUtil.NumberToStringFormate(sumLjdbckje,2);//
            //reportGrid.Cell(summaryRowIndex, 22).Text = (sumPysl - sumPksl),4);//
            reportGrid.Cell(summaryRowIndex, 23).Text = CommonUtil.NumberToStringFormate(sumPkje,2);//
            //reportGrid.Cell(summaryRowIndex, 24).Text = (sumLjpysl - sumLjpksl),4);//
            reportGrid.Cell(summaryRowIndex, 25).Text = CommonUtil.NumberToStringFormate(sumLjpkje,2);//
            reportGrid.Cell(summaryRowIndex, 26).Text = CommonUtil.NumberToStringFormate(sumJcsl,4);//
            reportGrid.Cell(summaryRowIndex, 27).Text = CommonUtil.NumberToStringFormate(sumJcje, 2);//

           // Xhmx_clsfcybb(beginDate, endDate, pi.Id, summaryRowIndex, lstMaterialCategory);
            
                if (IsSelectParts)
                {
                    IList lstUsePart = this.txtUsePart.Tag as IList;
                    if (this.RN_clsfcybb_Subject == cmbReportName.SelectedItem + "")
                    {
                        Xhmx_clsfcybb_Subject(beginDate, endDate, pi.Id, summaryRowIndex, lstMaterialCategory);
                    }
                    else
                    {
                        Xhmx_clsfcybb(beginDate, endDate, pi.Id, summaryRowIndex, lstMaterialCategory, lstUsePart);
                    }
                }
                else
                {
                    if (this.RN_clsfcybb_Subject == cmbReportName.SelectedItem + "")
                    {
                        Xhmx_clsfcybb_Subject(beginDate, endDate, pi.Id, summaryRowIndex, lstMaterialCategory);
                    }
                    else
                    {
                        Xhmx_clsfcybb(beginDate, endDate, pi.Id, summaryRowIndex, lstMaterialCategory);
                    }
                }
             
        
            FlexCell.Range range = reportGrid.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount, 3);
            range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);

            range = reportGrid.Range(detailStartRowNumber, 4, detailStartRowNumber + detailCount, reportGrid.Cols - 2);
            range.Alignment = FlexCell.AlignmentEnum.RightCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
        }

        /// <summary>
        /// 材料收发存月报表 消耗明细
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectId"></param>
        /// <param name="summaryRowIndex"></param>
        /// <param name="wzfl">物资分类code</param>
     //   private void Xhmx_clsfcybb(string beginDate, string endDate, string projectId, int summaryRowIndex,string wzfl)
        private void Xhmx_clsfcybb(string beginDate, string endDate, string projectId, int summaryRowIndex, IList lstReslut)
        {
            IList usedPartLst = GetUsedPart(projectId);
            if (usedPartLst == null || usedPartLst.Count == 0) return;
            int startCol = 0;
             string materialId;
              decimal xhsl = 0;
                    decimal xhje = 0;
                    decimal ljxhsl = 0;
                    decimal ljxhje = 0;
             int machedRowIndex=0;
             DataTable dt=null;
             DataSet ds =null;
             decimal sumXhsl = 0M, sumXhje = 0M, sumLjxhsl = 0M, sumLjxhje = 0M;
            foreach (GWBSTree usedPart in usedPartLst)
            {
                  startCol = reportGrid.Cols - 1;
                InsertGridCol(startCol, 3, usedPart.Name);//3为报表格式中的行号
                  ds = model.StockInOutSrv.WZBB_clsfcybb_xhmx(beginDate, endDate, projectId, usedPart.SysCode, lstReslut);
                if (ds == null || ds.Tables.Count == 0) continue;
                  dt = ds.Tables[0];
                if (dt == null || dt.Rows.Count == 0) continue;
                  sumXhsl = 0M;
                sumXhje = 0M;
                sumLjxhsl = 0M;
                sumLjxhje = 0M;
                foreach (DataRow dr in dt.Rows)
                {
                      materialId = dr["materialid"] + "";
                      xhsl = ClientUtil.ToDecimal(dr["xhsl"]);
                      xhje = ClientUtil.ToDecimal(dr["xhje"]);
                      ljxhsl = ClientUtil.ToDecimal(dr["ljxhsl"]);
                      ljxhje = ClientUtil.ToDecimal(dr["ljxhje"]);
                      machedRowIndex = MachedRowIndex(materialId);
                     
                    if (machedRowIndex == int.MinValue) continue;
                    reportGrid.Cell(machedRowIndex, startCol).Text = CommonUtil.NumberToStringFormate(xhsl,4);
                    reportGrid.Cell(machedRowIndex, startCol + 1).Text = CommonUtil.NumberToStringFormate(xhje,2);
                    reportGrid.Cell(machedRowIndex, startCol + 2).Text = CommonUtil.NumberToStringFormate(ljxhsl,4);
                    reportGrid.Cell(machedRowIndex, startCol + 3).Text = CommonUtil.NumberToStringFormate(ljxhje,2);
                    sumXhsl += xhsl;
                    sumXhje += xhje;
                    sumLjxhsl += ljxhsl;
                    sumLjxhje += ljxhje;
                }
                reportGrid.Cell(summaryRowIndex, startCol).Text = CommonUtil.NumberToStringFormate(sumXhsl,4);
                reportGrid.Cell(summaryRowIndex, startCol + 1).Text = CommonUtil.NumberToStringFormate(sumXhje,2);
                reportGrid.Cell(summaryRowIndex, startCol + 2).Text = CommonUtil.NumberToStringFormate(sumLjxhsl,4);
                reportGrid.Cell(summaryRowIndex, startCol + 3).Text = CommonUtil.NumberToStringFormate(sumLjxhje, 2);
            }

            #region
            string sNoKnow = "无使用部位";
              startCol = reportGrid.Cols - 1;
            startCol = reportGrid.Cols - 1;
            reportGrid.InsertCol(startCol, 4);
            int iEndCol = startCol;
            reportGrid.Cell(3, startCol).Text = sNoKnow;
            reportGrid.Cell(3, startCol).Tag = sNoKnow;
            FlexCell.Range oRange = reportGrid.Range(3, startCol, 4, startCol + 3);
            oRange.MergeCells = true;
            oRange.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            oRange.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            oRange.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            reportGrid.Cell(5, startCol).Text = "本期数量";
            reportGrid.Cell(5, startCol + 1).Text = "本期金额";
            reportGrid.Cell(5, startCol + 2).Text = "本期数量";
            reportGrid.Cell(5, startCol + 3).Text = "本期金额";

          //  startCol = reportGrid.Cols - 1;
           // InsertGridCol(startCol, 3, sNoKnow);//3为报表格式中的行号
            ds = model.StockInOutSrv.WZBB_clsfcybb_xhmx(beginDate, endDate, projectId,  lstReslut);
            if (ds == null || ds.Tables.Count == 0) return ;
            dt = ds.Tables[0];
            if (dt == null || dt.Rows.Count == 0) return ;
            sumXhsl = 0M;
            sumXhje = 0M;
            sumLjxhsl = 0M;
            sumLjxhje = 0M;
            foreach (DataRow dr in dt.Rows)
            {
                materialId = dr["materialid"] + "";
                xhsl = ClientUtil.ToDecimal(dr["xhsl"]);
                xhje = ClientUtil.ToDecimal(dr["xhje"]);
                ljxhsl = ClientUtil.ToDecimal(dr["ljxhsl"]);
                ljxhje = ClientUtil.ToDecimal(dr["ljxhje"]);
                machedRowIndex = MachedRowIndex(materialId);

                if (machedRowIndex == int.MinValue) continue;
                reportGrid.Cell(machedRowIndex, startCol).Text = CommonUtil.NumberToStringFormate(xhsl, 4);
                reportGrid.Cell(machedRowIndex, startCol + 1).Text = CommonUtil.NumberToStringFormate(xhje, 2);
                reportGrid.Cell(machedRowIndex, startCol + 2).Text = CommonUtil.NumberToStringFormate(ljxhsl, 4);
                reportGrid.Cell(machedRowIndex, startCol + 3).Text = CommonUtil.NumberToStringFormate(ljxhje, 2);
                sumXhsl += xhsl;
                sumXhje += xhje;
                sumLjxhsl += ljxhsl;
                sumLjxhje += ljxhje;
            }
            reportGrid.Cell(summaryRowIndex, startCol).Text = CommonUtil.NumberToStringFormate(sumXhsl, 4);
            reportGrid.Cell(summaryRowIndex, startCol + 1).Text = CommonUtil.NumberToStringFormate(sumXhje, 2);
            reportGrid.Cell(summaryRowIndex, startCol + 2).Text = CommonUtil.NumberToStringFormate(sumLjxhsl, 4);
            reportGrid.Cell(summaryRowIndex, startCol + 3).Text = CommonUtil.NumberToStringFormate(sumLjxhje, 2);
            #endregion


    }
        
        private void Xhmx_clsfcybb(string beginDate, string endDate, string projectId, int summaryRowIndex, IList lstReslut, IList lstUsePart)
        {
            string sNotKnow = "其他部位消耗";
            string sSubjectTempID = string.Empty;
            bool bFlag = false;
            QtyMoney oQtyMoney = null;
            if (lstUsePart != null && lstUsePart.Count > 0)
            {
              Hashtable hs=  SetHeadUsePart(lstUsePart, Headtype.SumAndCurrent,sNotKnow );
              if (hs.Count > 0)
              {
                  DataSet ds = model.StockInOutSrv.WZBB_clsfcybb_xhmx(beginDate, endDate, projectId, lstReslut, lstUsePart);
                  if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                  {
                      FillText(ds.Tables[0], hs, Headtype.SumAndCurrent, summaryRowIndex, sNotKnow);
                  }
                  
              }
           
              //for (int j = reportGrid.Cols - 1; j >= 0; j--)
              //{
              //    sSubjectTempID = reportGrid.Cell(4, j).Tag;
                 
              //    if (string.IsNullOrEmpty(sSubjectTempID)&& hs.ContainsKey(sSubjectTempID)) continue;
              //    bFlag = true;
              //    for (int i = 3; i < reportGrid.Rows; i++)
              //    {
              //        if(!(string.IsNullOrEmpty(reportGrid.Cell(3, j).Text) || string.IsNullOrEmpty(reportGrid.Cell(3, j+1).Text) || string.IsNullOrEmpty(reportGrid.Cell(5, j+2).Text) || string.IsNullOrEmpty(reportGrid.Cell(6, j+3).Text)))
              //        {
              //            bFlag =false ;
              //            break;
              //        }
                      
              //    }
              //    if (bFlag)
              //    {
              //        reportGrid.Column(j).Visible = false;
              //        reportGrid.Column(j+1).Visible = false;
              //        reportGrid.Column(j+2).Visible = false;
              //        reportGrid.Column(j +3).Visible = false;
              //    }

              //}
             
            }
        }
        public void FillText(DataTable tbSum, Hashtable ht, Headtype eHead, int summaryRowIndex, string sNotKnow)
        {

            string sSubjectName = string.Empty;
            string sSubjectTempName = string.Empty;
            int startCol = 0;
            IList lstRows = new ArrayList();
            
            DataRow[] oDataRows = null;
            string sMaterialID = string.Empty;
            string sHeadID = string.Empty;
            string sSubjectTempID = string.Empty;
            decimal SubjectSumNum = 0;
            decimal SubjectSunMoney = 0;
            decimal SubjectLstSumNum = 0;
            decimal SubjectLstSunMoney = 0;
            string sWhere = string.Empty;
            bool IsExistNoKnow = false;
            int iEndCol = 0;  
         int iColumn=0;
            string sOhter="调整金额";
            string sMaterial = "料具租赁";
            int iMaterialRow = 0;
         bool isFind = false;
            QtyMoney oQtyMoney = null;
            int iStep=eHead == Headtype.SumAndCurrent?5:3;
            QtyMoney oQtyMoneyTmp = null;
            iEndCol = reportGrid.Cols - iStep ;
            int iCount = 0;
            decimal dDyTotalMoney = 0;
            decimal dLjTotalMoney = 0;
            decimal dMoneyTmp = 0;
            int iCol=0;

            for (int i = 3; i < reportGrid.Cols; i++)
            {
                sSubjectTempID = reportGrid.Cell(4, i).Tag;

                if (string.IsNullOrEmpty(sSubjectTempID))
                {
                    sSubjectTempID = reportGrid.Cell(3, i).Tag;
                }

                if (string.IsNullOrEmpty(sSubjectTempID)) continue;
                reportGrid.Column(i + 1).DecimalLength = 2;
                if (eHead == Headtype.SumAndCurrent)
                {
                    reportGrid.Column(i + 3).DecimalLength = 2;
                }

            }
                for (int i = 3; i < reportGrid.Rows; i++)
                {
                    sMaterialID = reportGrid.Cell(i, 1).Tag;
                    if (!string.IsNullOrEmpty(sMaterialID))
                    {
                        sWhere = string.Format("materialid='{0}'", sMaterialID);
                        if (string.Equals(sMaterial, sMaterialID))
                        {
                            iMaterialRow = i;
                        }
                        oDataRows = tbSum.Select(sWhere);
                        if (oDataRows != null && oDataRows.Length > 0)
                        {
                            foreach (DataRow oRow in oDataRows)
                            {
                                isFind = false;
                                sHeadID = oRow["HeadID"].ToString();
                                SubjectSumNum = ClientUtil.ToDecimal(oRow["Quantity"]);
                                SubjectSunMoney = ClientUtil.ToDecimal(oRow["Money"]);
                                if (eHead == Headtype.SumAndCurrent)
                                {
                                    SubjectLstSumNum = ClientUtil.ToDecimal(oRow["lstquantity"]);
                                    SubjectLstSunMoney = ClientUtil.ToDecimal(oRow["lstmoney"]);
                                }
                                //if (string.Equals(sHeadID, "调整金额"))
                                //{
                                //    string s = "";
                                //}
                                if (string.IsNullOrEmpty(sHeadID))
                                {
                                    iColumn = iEndCol;
                                    sHeadID = sNotKnow;
                                    IsExistNoKnow = true;
                                }
                                else
                                {
                                    if (string.Equals(sHeadID, sOhter))
                                    {
                                        isFind = true;
                                    }
                                    for (int j = reportGrid.Cols - 1; j >= 0; j--)
                                    {
                                        sSubjectTempID = reportGrid.Cell(4, j).Tag;

                                        if (string.IsNullOrEmpty(sSubjectTempID))
                                        {
                                            sSubjectTempID = reportGrid.Cell(3, j).Tag;
                                        }

                                        if (string.IsNullOrEmpty(sSubjectTempID)) continue;
                                        if (string.Equals(sHeadID, sSubjectTempID))
                                        {
                                            iColumn = j;
                                            isFind = true;
                                            break;
                                        }
                                    }
                                    if (!isFind)
                                    {
                                        iColumn = iEndCol;
                                        sHeadID = sNotKnow;
                                        IsExistNoKnow = true;
                                    }
                                }
                                if (ht.ContainsKey(sHeadID))
                                {
                                    oQtyMoney = ht[sHeadID] as QtyMoney;
                                    oQtyMoney.Qty += SubjectSumNum;
                                    oQtyMoney.Money += SubjectSunMoney;
                                    if (eHead == Headtype.SumAndCurrent)
                                    {
                                        oQtyMoney.Ltqty += SubjectLstSumNum;
                                        oQtyMoney.Ltmoney += SubjectLstSunMoney;
                                    }
                                    ht[sHeadID] = oQtyMoney;
                                }
                                if (!string.Equals(sHeadID, sOhter))
                                {
                                    decimal dTemp = 0;
                                    dTemp = ClientUtil.ToDecimal(reportGrid.Cell(i, iColumn).Text) + SubjectSumNum;
                                    reportGrid.Cell(i, iColumn).Text = CommonUtil.NumberToStringFormate(dTemp, 4);
                                    dTemp = ClientUtil.ToDecimal(reportGrid.Cell(i, iColumn + 1).Text) + SubjectSunMoney;
                                    reportGrid.Cell(i, iColumn + 1).Text = CommonUtil.NumberToStringFormate(dTemp, 2);
                                    if (eHead == Headtype.SumAndCurrent)
                                    {
                                        dTemp = ClientUtil.ToDecimal(reportGrid.Cell(i, iColumn + 2).Text) + SubjectLstSumNum;
                                        reportGrid.Cell(i, iColumn + 2).Text = CommonUtil.NumberToStringFormate(dTemp, 4);
                                        dTemp = ClientUtil.ToDecimal(reportGrid.Cell(i, iColumn + 3).Text) + SubjectLstSunMoney;
                                        reportGrid.Cell(i, iColumn + 3).Text = CommonUtil.NumberToStringFormate(dTemp, 2);
                                    }
                                }
                            }


                        }
                    }
                }

            //分摊算法基本写完
            if (ht.ContainsKey(sOhter) && iMaterialRow > 0)
            {
                oQtyMoney = ht[sOhter] as QtyMoney;
                if (oQtyMoney.Ltmoney != 0 || oQtyMoney.Ltqty != 0 || oQtyMoney.Money != 0 || oQtyMoney.Qty != 0)
                {
                    foreach (string sKey in ht.Keys)
                    {
                        if (string.Equals(sOhter, sKey))
                        {
                        }
                        else
                        {
                            oQtyMoneyTmp = ht[sKey] as QtyMoney;
                            if (oQtyMoneyTmp.Ltmoney != 0 || oQtyMoneyTmp.Ltqty != 0 || oQtyMoneyTmp.Money != 0 || oQtyMoneyTmp.Qty != 0)
                            {

                                iCount++;
                            }
                        }
                    }
                    if (iCount > 0)
                    {
                        for (int j = reportGrid.Cols - 1; j >= 0; j--)
                        {
                            sSubjectTempID = reportGrid.Cell(4, j).Tag;

                            if (string.IsNullOrEmpty(sSubjectTempID))
                            {
                                sSubjectTempID = reportGrid.Cell(3, j).Tag;
                            }

                            if (string.IsNullOrEmpty(sSubjectTempID)) continue;
                            if (ht.ContainsKey(sSubjectTempID))
                            {
                                oQtyMoneyTmp = ht[sSubjectTempID] as QtyMoney;
                                if (oQtyMoneyTmp.Ltmoney != 0 || oQtyMoneyTmp.Ltqty != 0 || oQtyMoneyTmp.Money != 0 || oQtyMoneyTmp.Qty != 0)
                                {
                                    iCol = j;
                                    dMoneyTmp = Math.Round(oQtyMoney.Money / iCount, 2);
                                    dDyTotalMoney += dMoneyTmp;
                                    oQtyMoneyTmp.Money += dMoneyTmp;
                                    dMoneyTmp = ClientUtil.ToDecimal(reportGrid.Cell(iMaterialRow, j + 1).Text) + dMoneyTmp;
                                    reportGrid.Cell(iMaterialRow, j + 1).Text = CommonUtil.NumberToStringFormate(dMoneyTmp, 2);
                                    if (eHead == Headtype.SumAndCurrent)
                                    {
                                        dMoneyTmp = Math.Round(oQtyMoney.Ltmoney / iCount, 2);
                                        dLjTotalMoney += dMoneyTmp;
                                        oQtyMoneyTmp.Ltmoney += dMoneyTmp;
                                        dMoneyTmp = ClientUtil.ToDecimal(reportGrid.Cell(iMaterialRow, j + 3).Text) + dMoneyTmp;
                                        reportGrid.Cell(iMaterialRow, j + 3).Text = CommonUtil.NumberToStringFormate(dMoneyTmp, 2);
                                    }
                                    ht[sSubjectTempID] = oQtyMoneyTmp;
                                }
                            }
                        }
                        if (iCol > 0)
                        {
                            if (oQtyMoney.Money != dDyTotalMoney)
                            {
                                sSubjectTempID = reportGrid.Cell(4, iCol).Tag;

                                if (string.IsNullOrEmpty(sSubjectTempID))
                                {
                                    sSubjectTempID = reportGrid.Cell(3, iCol).Tag;
                                }
                                oQtyMoneyTmp = ht[sSubjectTempID] as QtyMoney;
                                dMoneyTmp = oQtyMoney.Money - dDyTotalMoney;
                                oQtyMoneyTmp.Money += dMoneyTmp;
                                dMoneyTmp = ClientUtil.ToDecimal(reportGrid.Cell(iMaterialRow, iCol + 1).Text) + dMoneyTmp;
                                reportGrid.Cell(iMaterialRow, iCol + 1).Text = CommonUtil.NumberToStringFormate(dMoneyTmp, 2);
                                if (eHead == Headtype.SumAndCurrent)
                                {
                                    if (oQtyMoney.Ltmoney != dLjTotalMoney)
                                    {
                                        dMoneyTmp = oQtyMoney.Ltmoney - dLjTotalMoney;
                                        oQtyMoneyTmp.Ltmoney += dMoneyTmp;
                                        dMoneyTmp = ClientUtil.ToDecimal(reportGrid.Cell(iMaterialRow, iCol + 3).Text) + dMoneyTmp;
                                        reportGrid.Cell(iMaterialRow, iCol + 3).Text = CommonUtil.NumberToStringFormate(dMoneyTmp, 2);
                                    }
                                }
                                ht[sSubjectTempID] = oQtyMoneyTmp;
                            }
                        }
                    }
                }
            }
            for (int j = reportGrid.Cols - 1; j >= 0; j--)
            {
                sSubjectTempID = reportGrid.Cell(4, j).Tag;
                if (string.IsNullOrEmpty(sSubjectTempID))
                {
                    sSubjectTempID = reportGrid.Cell(3, j).Tag;
                }
                if (!string.IsNullOrEmpty(sSubjectTempID) && ht.ContainsKey(sSubjectTempID))
                {
                    oQtyMoney = ht[sSubjectTempID] as QtyMoney;
                    if (oQtyMoney != null)
                    {
                        reportGrid.Cell(summaryRowIndex, j).Text = CommonUtil.NumberToStringFormate(oQtyMoney.Qty, 4);
                        reportGrid.Cell(summaryRowIndex, j + 1).Text = CommonUtil.NumberToStringFormate(oQtyMoney.Money, 2);
                        if (eHead == Headtype.SumAndCurrent)
                        {
                            reportGrid.Cell(summaryRowIndex, j+2).Text = CommonUtil.NumberToStringFormate(oQtyMoney.Ltqty, 4);
                            reportGrid.Cell(summaryRowIndex, j + 3).Text = CommonUtil.NumberToStringFormate(oQtyMoney.Ltmoney, 2);
                        }
                    }
                }

            }
            //if (ht.ContainsKey(sNotKnow))
            //{
            //    oQtyMoney = ht[sNotKnow] as QtyMoney;
            //    if (oQtyMoney != null)
            //    {
            //        reportGrid.Cell(summaryRowIndex, iEndCol).Text = CommonUtil.NumberToStringFormate(oQtyMoney.Qty, 4);
            //        reportGrid.Cell(summaryRowIndex, iEndCol + 1).Text = CommonUtil.NumberToStringFormate(oQtyMoney.Money, 2);
            //        if (eHead == Headtype.SumAndCurrent)
            //        {
            //            reportGrid.Cell(summaryRowIndex, iEndCol + 2).Text = CommonUtil.NumberToStringFormate(oQtyMoney.Ltqty, 4);
            //            reportGrid.Cell(summaryRowIndex, iEndCol + 3).Text = CommonUtil.NumberToStringFormate(oQtyMoney.Ltmoney, 2);
            //        }
            //    }
            //}
            //reportGrid.Column(iEndCol).Visible = IsExistNoKnow;
            //reportGrid.Column(iEndCol+1).Visible = IsExistNoKnow;
            //if (eHead == Headtype.SumAndCurrent)
            //{
            //    reportGrid.Column(iEndCol+2).Visible = IsExistNoKnow;
            //    reportGrid.Column(iEndCol + 3).Visible = IsExistNoKnow;
            //}
            
        }
        public Hashtable SetHeadUsePart(IList lstUsePart, Headtype eHeadType, string sNotKnow)
        {
        
            Hashtable ht = null;
            if (lstUsePart != null && lstUsePart.Count > 0)
            {
                int startCol = 0;
                ht = new Hashtable();
                foreach (GWBSTree oGWBSTree in lstUsePart)  //写部位
                {
                    startCol = reportGrid.Cols - 1;
                    if (eHeadType == Headtype.SumAndCurrent)
                    {
                       InsertGridCol(startCol, 3, oGWBSTree.Name);
                        reportGrid.Cell(4, startCol).Tag = oGWBSTree.Id;
                    }
                    else if (eHeadType == Headtype.Current )
                    {
                        InsertGridCol4Clsfchzb(startCol, 3, oGWBSTree.Name);
                        reportGrid.Cell(4, startCol).Tag = oGWBSTree.Id;
                    }
                    if (!ht.ContainsKey(oGWBSTree.Id))
                    {
                        ht.Add(oGWBSTree.Id, new QtyMoney()); 
                    }
                }
                if (eHeadType == Headtype.Current)
                {
                    startCol = reportGrid.Cols - 1;
                    reportGrid.InsertCol(startCol, 2);
                    reportGrid.Cell(3, startCol).Text = sNotKnow;
                    reportGrid.Cell(3, startCol).Tag = sNotKnow;
                    FlexCell.Range oRange = reportGrid.Range(3, startCol, 4, startCol + 1);
                    oRange.MergeCells = true;
                    oRange.Alignment = FlexCell.AlignmentEnum.CenterCenter;
                    oRange.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
                    oRange.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
                    reportGrid.Cell(5, startCol).Text = "数量";
                    reportGrid.Cell(5, startCol + 1).Text = "金额";
                }
                else if (eHeadType == Headtype.SumAndCurrent)
                {
                    startCol = reportGrid.Cols - 1;
                    reportGrid.InsertCol(startCol, 4);
                    reportGrid.Cell(3, startCol).Text = sNotKnow;
                    reportGrid.Cell(3, startCol).Tag = sNotKnow;
                    FlexCell.Range oRange = reportGrid.Range(3, startCol, 4, startCol + 3);
                    oRange.MergeCells = true;
                    oRange.Alignment = FlexCell.AlignmentEnum.CenterCenter;
                    oRange.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
                    oRange.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
                    reportGrid.Cell(5, startCol).Text = "本期数量";
                    reportGrid.Cell(5, startCol + 1).Text = "本期金额";
                    reportGrid.Cell(5, startCol+2).Text = "累计数量";
                    reportGrid.Cell(5, startCol + 3).Text = "累计金额";
                }
                if (!ht.ContainsKey(sNotKnow))
                {
                    ht.Add(sNotKnow, new QtyMoney());
                }

            }
            return ht;
        }
        private void Xhmx_clsfcybb_Subject(string beginDate, string endDate, string projectId ,int summaryRowIndex, IList lstReslut)
        {
            string sSubjectName =string.Empty ;
            string sSubjectTempName =string.Empty ;
            int startCol=0;
            IList lstRows= new ArrayList();
            DataSet dsSum = null;
            DataTable tbSum = null;
            DataRow[] oDataRows = null;
            string sMaterialID = string.Empty;
            string sSubjectID = string.Empty;
             string sSubjectTempID = string.Empty;
            decimal  SubjectSumNum=0 ;
            decimal  SubjectSunMoney=0;
            string sWhere =string.Empty ;
            Hashtable ht = new Hashtable(); 
            int sStartColSubject = reportGrid.Cols - 1;
            DataSet ds = model.StockInOutSrv.WZBB_GetCostAccountSubjectCat(beginDate, endDate, projectId, lstReslut);//获取核算科目类型
            int iEndCol = 0;
            string sNotKnow = "无核算科目";
            QtyMoney oQtyMoney = null;
            if(ds==null || ds.Tables.Count ==0 || ds.Tables [0].Rows .Count ==0) return ;
            foreach (DataRow oRow in ds.Tables[0].Rows)
            {
                sSubjectID = oRow["ID"].ToString();
                if (!ht.ContainsKey(sSubjectID))
                {
                    ht.Add(oRow["ID"].ToString(), new QtyMoney());
                }
                if (string.IsNullOrEmpty(sSubjectName))
                {
                    sSubjectName = oRow["ParentName"].ToString();
                    lstRows.Clear();
                    lstRows.Add(oRow);
                }
                else
                {
                    sSubjectTempName = oRow["ParentName"].ToString();
                    if (string.Equals(sSubjectTempName, sSubjectName))
                    {
                        lstRows.Add(oRow);
                    }
                    else
                    {
                        startCol = reportGrid.Cols - 1;
                        InsertGridCol(startCol, 3, lstRows);
                        sSubjectName = sSubjectTempName;
                        lstRows.Clear();
                        lstRows.Add(oRow);
                    }
                }

            }
            if (lstRows != null && lstRows.Count > 0)
            {
                startCol = reportGrid.Cols - 1;
                InsertGridCol(startCol, 3, lstRows);
            }
           
            startCol = reportGrid.Cols - 1;
            reportGrid.InsertCol(startCol, 2);
            iEndCol = startCol;
            reportGrid.Cell(3, startCol).Text = sNotKnow;
            reportGrid.Cell(3, startCol).Tag = sNotKnow;
            FlexCell.Range oRange = reportGrid.Range(3, startCol, 4, startCol+1);
            oRange.MergeCells = true;
            oRange.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            oRange.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            oRange.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            reportGrid.Cell(5, startCol).Text = "本期数量";
            reportGrid.Cell(5, startCol + 1).Text = "本期金额";
            ht.Add(sNotKnow, new QtyMoney());
           

            dsSum = model.StockInOutSrv.WZBB_GetCostAccountSubjectSum(beginDate, endDate, projectId, lstReslut);
            if (dsSum == null || dsSum.Tables.Count == 0 || dsSum.Tables[0].Rows.Count == 0) return;
            tbSum = dsSum.Tables[0];
                for (int i = 3; i < reportGrid.Rows; i++)
                {
                    sMaterialID = reportGrid.Cell(i, 1).Tag;
                    if (!string.IsNullOrEmpty(sMaterialID))
                    {
                        sWhere=string.Format ("materialid='{0}'",sMaterialID );
                        oDataRows = tbSum.Select(sWhere);
                        if (oDataRows != null && oDataRows.Length > 0)
                        {  
                            foreach (DataRow oRow in oDataRows)
                            {
                                sSubjectID =oRow["subjectguid"].ToString ();
                                if (string.Equals(sSubjectID, sNotKnow))
                                {
                                    SubjectSumNum = ClientUtil.ToDecimal(oRow["sumQuantity"]);
                                    SubjectSunMoney = ClientUtil.ToDecimal(oRow["sumMoney"]);
                                    if (ht.ContainsKey(sSubjectID) && ht[sSubjectID] != null)
                                    {
                                          oQtyMoney = ht[sSubjectID] as QtyMoney;
                                        oQtyMoney.Qty += SubjectSumNum;
                                        oQtyMoney.Money += SubjectSunMoney;
                                        ht[sSubjectID] = oQtyMoney;
                                    }
                                    //reportGrid.Cell(i, j).Text = reportGrid.Cell(4, j).Text  ;
                                    //reportGrid.Cell(i, j + 1).Text = reportGrid.Cell(4, j).Text  ;
                                    reportGrid.Cell(i, iEndCol).Text = CommonUtil.NumberToStringFormate(SubjectSumNum, 4);
                                    reportGrid.Cell(i, iEndCol + 1).Text = CommonUtil.NumberToStringFormate(SubjectSunMoney, 2);
                                }
                                else
                                {
                                    for (int j = reportGrid.Cols - 1; j >= sStartColSubject; j--)
                                    {
                                        sSubjectTempID = reportGrid.Cell(4, j).Tag;
                                        if (string.IsNullOrEmpty(sSubjectTempID)) continue;
                                        if (string.Equals(sSubjectID, sSubjectTempID))
                                        {
                                            SubjectSumNum = ClientUtil.ToDecimal(oRow["sumQuantity"]);
                                            SubjectSunMoney = ClientUtil.ToDecimal(oRow["sumMoney"]);
                                            if (ht.ContainsKey(sSubjectID) && ht[sSubjectID] != null)
                                            {
                                                  oQtyMoney = ht[sSubjectID] as QtyMoney;
                                                oQtyMoney.Qty += SubjectSumNum;
                                                oQtyMoney.Money += SubjectSunMoney;
                                                ht[sSubjectID] = oQtyMoney;
                                            }
                                            //reportGrid.Cell(i, j).Text = reportGrid.Cell(4, j).Text  ;
                                            //reportGrid.Cell(i, j + 1).Text = reportGrid.Cell(4, j).Text  ;
                                            reportGrid.Cell(i, j).Text = CommonUtil.NumberToStringFormate(SubjectSumNum, 4);
                                            reportGrid.Cell(i, j + 1).Text = CommonUtil.NumberToStringFormate(SubjectSunMoney, 2);
                                            break;
                                        }
                                    }
                                }
                            }
                             
                        }
                    }
                }
                for (int j = reportGrid.Cols - 1; j >= sStartColSubject; j--)
                {
                    sSubjectTempID = reportGrid.Cell(4, j).Tag;
                    
                    if (!string.IsNullOrEmpty(sSubjectTempID) && ht.ContainsKey(sSubjectTempID))
                    {
                          oQtyMoney = ht[sSubjectTempID] as QtyMoney;
                        if(oQtyMoney !=null)
                        {
                            reportGrid.Cell(summaryRowIndex, j).Text = CommonUtil.NumberToStringFormate(oQtyMoney.Qty, 4);
                            reportGrid.Cell(summaryRowIndex, j+1).Text = CommonUtil.NumberToStringFormate(oQtyMoney.Money , 2);
                        }
                    }
                    
                }
                if (ht.ContainsKey(sNotKnow))
                {
                    oQtyMoney = ht[sNotKnow] as QtyMoney;
                    if (oQtyMoney != null)
                    {
                        reportGrid.Cell(summaryRowIndex, iEndCol).Text = CommonUtil.NumberToStringFormate(oQtyMoney.Qty, 4);
                        reportGrid.Cell(summaryRowIndex, iEndCol + 1).Text = CommonUtil.NumberToStringFormate(oQtyMoney.Money, 2);
                    }
                }
        }

        
      
        private void InsertGridCol(int startCol, int tableHeaderStartRow, IList lstRows)
        {
            if (lstRows != null && lstRows.Count  > 0)
            {
                int iLength = lstRows.Count ;
                int iColStart = 0;
                int iColEnd = 0;
                int iRowNum=0;
                string sName = string.Empty;
                string sID = string.Empty;
                DataRow oRow = null;
                reportGrid.InsertCol(startCol, iLength*2);
                FlexCell.Range range = reportGrid.Range(tableHeaderStartRow, startCol, tableHeaderStartRow, startCol + iLength*2 - 1);
                range.MergeCells = true;
                oRow = lstRows[0] as DataRow;
                sName = oRow["ParentName"].ToString();
                sID = oRow["ParentID"].ToString();
                reportGrid.Cell(tableHeaderStartRow, startCol).Text = sName;
                reportGrid.Cell(tableHeaderStartRow, startCol).Tag = sID;
                range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
                range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
                range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
                iRowNum=tableHeaderStartRow+1;
                for (int i = 0; i < lstRows.Count ; i++)
                {
                    oRow = lstRows[i] as DataRow;
                    iColStart = startCol + i * 2;
                    iColEnd = iColStart + 1;
                    range = reportGrid.Range(iRowNum, iColStart, iRowNum, iColEnd);
                    range.MergeCells = true;
                    sName = oRow["Name"].ToString();
                    sID = oRow["ID"].ToString();
                    reportGrid.Cell(iRowNum, iColStart).Text = sName;
                    reportGrid.Cell(iRowNum, iColStart).Tag = sID;
                    range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
                    range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
                    range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);


                    reportGrid.Cell(iRowNum + 1, iColStart).Text = "本期数量";
                    reportGrid.Cell(iRowNum + 1, iColEnd).Text = "本期金额";
                    range = reportGrid.Range(iRowNum+1, iColStart, iRowNum+1, iColEnd);
                    range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
                    range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
                    range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
                }
            }

            //reportGrid.InsertCol(startCol, 4);
            //FlexCell.Range range = reportGrid.Range(tableHeaderStartRow, startCol, tableHeaderStartRow, startCol + 3);
            //range.MergeCells = true;
            //reportGrid.Cell(tableHeaderStartRow, startCol).Text = "消耗明细";
            //range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            //range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            //range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);

            //range = reportGrid.Range(tableHeaderStartRow + 1, startCol, tableHeaderStartRow + 1, startCol + 3);
            //range.MergeCells = true;
            //reportGrid.Cell(tableHeaderStartRow + 1, startCol).Text = usedPartName;
            //range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            //range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            //range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);

            //range = reportGrid.Range(tableHeaderStartRow + 2, startCol, tableHeaderStartRow + 2, startCol + 3);
            //range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            //range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            //range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            //reportGrid.Cell(tableHeaderStartRow + 2, startCol).Text = "本期数量";
            //reportGrid.Cell(tableHeaderStartRow + 2, startCol + 1).Text = "本期金额";
            //reportGrid.Cell(tableHeaderStartRow + 2, startCol + 2).Text = "累计数量";
            //reportGrid.Cell(tableHeaderStartRow + 2, startCol + 3).Text = "累计金额";
        }
        /// <summary>
        /// 一次添加4列
        /// </summary>
        /// <param name="startCol"></param>
        /// <param name="tableHeaderStartRow"></param>
        /// <param name="lstRows"></param>
        private void InsertGridCol4(int startCol, int tableHeaderStartRow, IList lstRows)
        {
            if (lstRows != null && lstRows.Count > 0)
            {
                int iLength = lstRows.Count;
                int iColStart = 0;
                int iColEnd = 0;
                int iRowNum = 0;
                string sName = string.Empty;
                string sID = string.Empty;
                DataRow oRow = null;
                reportGrid.InsertCol(startCol, iLength * 4);
                FlexCell.Range range = reportGrid.Range(tableHeaderStartRow, startCol, tableHeaderStartRow, startCol + iLength * 4 - 1);
                range.MergeCells = true;
                oRow = lstRows[0] as DataRow;
                sName = oRow["ParentName"].ToString();
                sID = oRow["ParentID"].ToString();
                reportGrid.Cell(tableHeaderStartRow, startCol).Text = sName;
                reportGrid.Cell(tableHeaderStartRow, startCol).Tag = sID;
                range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
                range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
                range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
                iRowNum = tableHeaderStartRow + 1;
                for (int i = 0; i < lstRows.Count; i++)
                {
                    oRow = lstRows[i] as DataRow;
                    iColStart = startCol + i * 4;
                    iColEnd = iColStart +3;
                    range = reportGrid.Range(iRowNum, iColStart, iRowNum, iColEnd);
                    range.MergeCells = true;
                    sName = oRow["Name"].ToString();
                    sID = oRow["ID"].ToString();
                    reportGrid.Cell(iRowNum, iColStart).Text = sName;
                    reportGrid.Cell(iRowNum, iColStart).Tag = sID;
                    range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
                    range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
                    range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);


                    reportGrid.Cell(iRowNum + 1, iColStart).Text = "本期数量";
                    reportGrid.Cell(iRowNum + 1, iColStart + 1).Text = "本期金额";
                    reportGrid.Cell(iRowNum + 1, iColStart + 2).Text = "累计数量";
                    reportGrid.Cell(iRowNum + 1, iColStart + 3).Text = "累计金额";
                    range = reportGrid.Range(iRowNum + 1, iColStart, iRowNum + 1, iColEnd);
                    range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
                    range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
                    range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
                }
            }

            //reportGrid.InsertCol(startCol, 4);
            //FlexCell.Range range = reportGrid.Range(tableHeaderStartRow, startCol, tableHeaderStartRow, startCol + 3);
            //range.MergeCells = true;
            //reportGrid.Cell(tableHeaderStartRow, startCol).Text = "消耗明细";
            //range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            //range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            //range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);

            //range = reportGrid.Range(tableHeaderStartRow + 1, startCol, tableHeaderStartRow + 1, startCol + 3);
            //range.MergeCells = true;
            //reportGrid.Cell(tableHeaderStartRow + 1, startCol).Text = usedPartName;
            //range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            //range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            //range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);

            //range = reportGrid.Range(tableHeaderStartRow + 2, startCol, tableHeaderStartRow + 2, startCol + 3);
            //range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            //range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            //range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            //reportGrid.Cell(tableHeaderStartRow + 2, startCol).Text = "本期数量";
            //reportGrid.Cell(tableHeaderStartRow + 2, startCol + 1).Text = "本期金额";
            //reportGrid.Cell(tableHeaderStartRow + 2, startCol + 2).Text = "累计数量";
            //reportGrid.Cell(tableHeaderStartRow + 2, startCol + 3).Text = "累计金额";
        }
        /// <summary>
        /// 商品砼收发存月报表
        /// </summary>
        private void FillReport_sptsfcybb()
        {
            CurrentProjectInfo pi = cmbProject.SelectedItem as CurrentProjectInfo;
            string beginDate = dtpDateBegin.Value.ToShortDateString();
            string endDate = dtpDateEnd.Value.ToShortDateString();

            string wzfl = "I112";//商品砼分类
            if (!string.IsNullOrEmpty(txtMaterialCategory.Text) && txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
            {
                MaterialCategory mc = txtMaterialCategory.Result[0] as MaterialCategory;
                if (!mc.Code.StartsWith(wzfl))
                {
                    MessageBox.Show("请选择物资分类【商品砼】或其下属分类。");
                    txtMaterialCategory.Focus();
                    return;
                }
                wzfl = mc.Code;
            }

            SupplierRelationInfo supplier = null;
            if (!string.IsNullOrEmpty(txtSupplier.Text) && txtSupplier.Result != null && txtSupplier.Result.Count > 0)
            {
                supplier = txtSupplier.Result[0] as SupplierRelationInfo;
            }

            DataSet ds = model.StockInOutSrv.WZBB_sptsfcybb(beginDate, endDate, pi.Id,wzfl,supplier);
            //reportGrid.Column(0).
            if (ds == null || ds.Tables.Count == 0) return;
            DataTable dt = ds.Tables[0];
            if (dt == null) return;

            reportGrid.Cell(2, 1).Text = "单位名称：" + pi.Name;
            reportGrid.Cell(2, 22).Text = "统计日期：" + beginDate + " 至 " + endDate;

            int detailStartRowNumber = 6;//6为模板中的行号
            int detailCount = dt.Rows.Count;
            if (detailCount == 0) return;
            //插入报表行
            reportGrid.InsertRow(detailStartRowNumber, detailCount);
            decimal sumCgsl = 0M, sumCgje = 0M, sumLjcgsl = 0M, sumLjcgje = 0M, sumDbrksl = 0M, sumDbrkje = 0M, sumLjdbrksl = 0M, sumLjdbrkje = 0M,
                sumPysl = 0M, sumPyje = 0M, sumLjpysl = 0M, sumLjpyje = 0M, sumXhsl = 0M, sumXhje = 0M, sumLjxhsl = 0M, sumLjxhje = 0M, sumDbcksl = 0M, sumDbckje = 0M, sumLjdbcksl = 0M,
                sumLjdbckje = 0M, sumPksl = 0M, sumPkje = 0M, sumLjpksl = 0M, sumLjpkje = 0M;
            for (int i = 0; i < detailCount; i++)
            {
                DataRow dr = dt.Rows[i];
                string materialId = dr["materialid"] + "";
                string matcode = dr["matcode"] + "";
                string matspec = dr["materialspec"] + "";
                string matname = dr["materialname"] + "";
                string unitname = dr["matstandardunitname"] + "";
                decimal cgsl = ClientUtil.ToDecimal(dr["cgsl"]);
                decimal cgje = ClientUtil.ToDecimal(dr["cgje"]);
                decimal ljcgsl = ClientUtil.ToDecimal(dr["ljcgsl"]);
                decimal ljcgje = ClientUtil.ToDecimal(dr["ljcgje"]);
                decimal dbrksl = ClientUtil.ToDecimal(dr["dbrksl"]);
                decimal dbrkje = ClientUtil.ToDecimal(dr["dbrkje"]);
                decimal ljdbrksl = ClientUtil.ToDecimal(dr["ljdbrksl"]);
                decimal ljdbrkje = ClientUtil.ToDecimal(dr["ljdbrkje"]);
                decimal pysl = ClientUtil.ToDecimal(dr["pysl"]);
                decimal pyje = ClientUtil.ToDecimal(dr["pyje"]);
                decimal ljpysl = ClientUtil.ToDecimal(dr["ljpysl"]);
                decimal ljpyje = ClientUtil.ToDecimal(dr["ljpyje"]);
                decimal xhsl = ClientUtil.ToDecimal(dr["xhsl"]);
                decimal xhje = ClientUtil.ToDecimal(dr["xhje"]);
                decimal ljxhsl = ClientUtil.ToDecimal(dr["ljxhsl"]);
                decimal ljxhje = ClientUtil.ToDecimal(dr["ljxhje"]);
                decimal dbcksl = ClientUtil.ToDecimal(dr["dbcksl"]);
                decimal dbckje = ClientUtil.ToDecimal(dr["dbckje"]);
                decimal ljdbcksl = ClientUtil.ToDecimal(dr["ljdbcksl"]);
                decimal ljdbckje = ClientUtil.ToDecimal(dr["ljdbckje"]);
                decimal pksl = ClientUtil.ToDecimal(dr["pksl"]);
                decimal pkje = ClientUtil.ToDecimal(dr["pkje"]);
                decimal ljpksl = ClientUtil.ToDecimal(dr["ljpksl"]);
                decimal ljpkje = ClientUtil.ToDecimal(dr["ljpkje"]);
                sumCgsl += cgsl;
                sumCgje += cgje;
                sumLjcgsl += ljcgsl;
                sumLjcgje += ljcgje;
                sumDbrksl += dbrksl;
                sumDbrkje += dbrkje;
                sumLjdbrksl += ljdbrksl;
                sumLjdbrkje += ljdbrkje;
                sumPysl += pysl;
                sumPyje += pyje;
                sumLjpysl += ljpysl;
                sumLjpyje += ljpyje;
                sumXhsl += xhsl;
                sumXhje += xhje;
                sumLjxhsl += ljxhsl;
                sumLjxhje += ljxhje;
                sumDbcksl += dbcksl;
                sumDbckje += dbckje;
                sumLjdbcksl += ljdbcksl;
                sumLjdbckje += ljdbckje;
                sumPksl += pksl;
                sumPkje += pkje;
                sumLjpksl += ljpksl;
                sumLjpkje += ljpkje;
                reportGrid.Cell(detailStartRowNumber + i, 1).Text = matname;//物资名称
                reportGrid.Cell(detailStartRowNumber + i, 1).Tag = materialId;//物资名称
                reportGrid.Cell(detailStartRowNumber + i, 2).Text = matspec;//规格
                reportGrid.Cell(detailStartRowNumber + i, 3).Text = unitname;//单位
                reportGrid.Cell(detailStartRowNumber + i, 4).Text = CommonUtil.NumberToStringFormate(cgsl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 5).Text = CommonUtil.NumberToStringFormate(cgje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 6).Text = CommonUtil.NumberToStringFormate(ljcgsl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 7).Text = CommonUtil.NumberToStringFormate(ljcgje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 8).Text = CommonUtil.NumberToStringFormate(dbrksl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 9).Text = CommonUtil.NumberToStringFormate(dbrkje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 10).Text = CommonUtil.NumberToStringFormate(ljdbrksl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 11).Text = CommonUtil.NumberToStringFormate(ljdbrkje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 12).Text = CommonUtil.NumberToStringFormate(xhsl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 13).Text = CommonUtil.NumberToStringFormate(xhje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 14).Text = CommonUtil.NumberToStringFormate(ljxhsl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 15).Text = CommonUtil.NumberToStringFormate(ljxhje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 16).Text = CommonUtil.NumberToStringFormate(dbcksl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 17).Text = CommonUtil.NumberToStringFormate(dbckje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 18).Text = CommonUtil.NumberToStringFormate(ljdbcksl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 19).Text = CommonUtil.NumberToStringFormate(ljdbckje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 20).Text = "";//
                reportGrid.Cell(detailStartRowNumber + i, 21).Text = CommonUtil.NumberToStringFormate(pkje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 22).Text = "";//
                reportGrid.Cell(detailStartRowNumber + i, 23).Text = CommonUtil.NumberToStringFormate(ljpkje,2);//
            }
            //插入合计
            int summaryRowIndex = detailStartRowNumber + detailCount;//合计行行号
            reportGrid.Cell(summaryRowIndex, 1).Text = "合计";//物资名称
            reportGrid.Cell(summaryRowIndex, 4).Text = CommonUtil.NumberToStringFormate(sumCgsl,4);//
            reportGrid.Cell(summaryRowIndex, 5).Text = CommonUtil.NumberToStringFormate(sumCgje,2);//
            reportGrid.Cell(summaryRowIndex, 6).Text = CommonUtil.NumberToStringFormate(sumLjcgsl,4);//
            reportGrid.Cell(summaryRowIndex, 7).Text = CommonUtil.NumberToStringFormate(sumLjcgje,2);//
            reportGrid.Cell(summaryRowIndex, 8).Text = CommonUtil.NumberToStringFormate(sumDbrksl,4);//
            reportGrid.Cell(summaryRowIndex, 9).Text = CommonUtil.NumberToStringFormate(sumDbrkje,2);//
            reportGrid.Cell(summaryRowIndex, 10).Text = CommonUtil.NumberToStringFormate(sumLjdbrksl,4);//
            reportGrid.Cell(summaryRowIndex, 11).Text = CommonUtil.NumberToStringFormate(sumLjdbrkje,2);//
            reportGrid.Cell(summaryRowIndex, 12).Text = CommonUtil.NumberToStringFormate(sumXhsl,4);//
            reportGrid.Cell(summaryRowIndex, 13).Text = CommonUtil.NumberToStringFormate(sumXhje,2);//
            reportGrid.Cell(summaryRowIndex, 14).Text = CommonUtil.NumberToStringFormate(sumLjxhsl,4);//
            reportGrid.Cell(summaryRowIndex, 15).Text = CommonUtil.NumberToStringFormate(sumLjxhje,2);//
            reportGrid.Cell(summaryRowIndex, 16).Text = CommonUtil.NumberToStringFormate(sumDbcksl,4);//
            reportGrid.Cell(summaryRowIndex, 17).Text = CommonUtil.NumberToStringFormate(sumDbckje,2);//
            reportGrid.Cell(summaryRowIndex, 18).Text = CommonUtil.NumberToStringFormate(sumLjdbcksl,4);//
            reportGrid.Cell(summaryRowIndex, 19).Text = CommonUtil.NumberToStringFormate(sumLjdbckje,2);//
            reportGrid.Cell(summaryRowIndex, 20).Text = "";//
            reportGrid.Cell(summaryRowIndex, 21).Text = CommonUtil.NumberToStringFormate(sumPkje,2);//
            reportGrid.Cell(summaryRowIndex, 22).Text = "";//
            reportGrid.Cell(summaryRowIndex, 23).Text = CommonUtil.NumberToStringFormate(sumLjpkje, 2);//

            //消耗明细
            //Xhmx_sptsfcybb(beginDate, endDate, pi.Id, summaryRowIndex, wzfl, supplier);//old
            if (this.IsSelectParts)
            {
                IList lstUsePart = this.txtUsePart.Tag as IList;
                Xhmx_sptsfcybb(beginDate, endDate, pi.Id, summaryRowIndex, wzfl, supplier, lstUsePart);
            }
            else
            {
                DataSet dsUserPart = model.StockInOutSrv.WZBB_sptsfcybb_xhmx_UserPart(beginDate, endDate, pi.Id, wzfl, supplier);
                DataSet dsSum = model.StockInOutSrv.WZBB_sptsfcybb_xhmx(beginDate, endDate, pi.Id, wzfl, supplier);
                Xhmx_Active_Subject4(summaryRowIndex, dsUserPart, dsSum, "未知部位");

            }
            
           

            FlexCell.Range range = reportGrid.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount, 3);
            range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);

            range = reportGrid.Range(detailStartRowNumber, 4, detailStartRowNumber + detailCount, reportGrid.Cols - 2);
            range.Alignment = FlexCell.AlignmentEnum.RightCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
        }
        private void Xhmx_sptsfcybb(string beginDate, string endDate, string projectId, int summaryRowIndex,string wzfl,SupplierRelationInfo supplier  , IList lstUsePart)
        {
            string sNotKnow = "其他部位消耗";
            if (lstUsePart != null && lstUsePart.Count > 0)
            {
                Hashtable hs = SetHeadUsePart(lstUsePart, Headtype.SumAndCurrent, sNotKnow);
                if (hs.Count > 0)
                {
                    DataSet ds = model.StockInOutSrv.WZBB_sptsfcybb_xhmx(beginDate, endDate, projectId, wzfl, supplier, lstUsePart);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        FillText(ds.Tables[0], hs, Headtype.SumAndCurrent, summaryRowIndex, sNotKnow);
                    }
                }
            }
        }
        /// <summary>
        /// 商品砼收发存月报表 消耗明细
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectId"></param>
        /// <param name="summaryRowIndex"></param>
        private void Xhmx_sptsfcybb(string beginDate, string endDate, string projectId, int summaryRowIndex, string categoryCode, SupplierRelationInfo supplier)
        {
            IList usedPartLst = GetUsedPart(projectId);
            if (usedPartLst == null || usedPartLst.Count == 0) return;
            foreach (GWBSTree usedPart in usedPartLst)
            {
                int startCol = reportGrid.Cols - 1;
                InsertGridCol(startCol, 3, usedPart.Name);//3为报表格式中的行号
                DataSet ds = model.StockInOutSrv.WZBB_sptsfcybb_xhmx(beginDate, endDate, projectId, usedPart.SysCode,categoryCode,supplier);
                if (ds == null || ds.Tables.Count == 0) continue;
                DataTable dt = ds.Tables[0];
                if (dt == null || dt.Rows.Count == 0) continue;
                decimal sumXhsl = 0M, sumXhje = 0M, sumLjxhsl = 0M, sumLjxhje = 0M;
                foreach (DataRow dr in dt.Rows)
                {
                    string materialId = dr["materialid"] + "";
                    decimal xhsl = ClientUtil.ToDecimal(dr["xhsl"]);
                    decimal xhje = ClientUtil.ToDecimal(dr["xhje"]);
                    decimal ljxhsl = ClientUtil.ToDecimal(dr["ljxhsl"]);
                    decimal ljxhje = ClientUtil.ToDecimal(dr["ljxhje"]);
                    int machedRowIndex = MachedRowIndex(materialId);
                    if (machedRowIndex == int.MinValue) continue;
                    reportGrid.Cell(machedRowIndex, startCol).Text = CommonUtil.NumberToStringFormate(xhsl,4);
                    reportGrid.Cell(machedRowIndex, startCol + 1).Text = CommonUtil.NumberToStringFormate(xhje,2);
                    reportGrid.Cell(machedRowIndex, startCol + 2).Text = CommonUtil.NumberToStringFormate(ljxhsl,4);
                    reportGrid.Cell(machedRowIndex, startCol + 3).Text = CommonUtil.NumberToStringFormate(ljxhje,2);
                    sumXhsl += xhsl;
                    sumXhje += xhje;
                    sumLjxhsl += ljxhsl;
                    sumLjxhje += ljxhje;
                }
                reportGrid.Cell(summaryRowIndex, startCol).Text = CommonUtil.NumberToStringFormate(sumXhsl,4);
                reportGrid.Cell(summaryRowIndex, startCol + 1).Text = CommonUtil.NumberToStringFormate(sumXhje,2);
                reportGrid.Cell(summaryRowIndex, startCol + 2).Text = CommonUtil.NumberToStringFormate(sumLjxhsl,4);
                reportGrid.Cell(summaryRowIndex, startCol + 3).Text = CommonUtil.NumberToStringFormate(sumLjxhje, 2);
            }
        }
        private void Xhmx_Active_Subject4(int summaryRowIndex, DataSet dsSubject, DataSet dsSum, string sNotKnow)
        {
            string sSubjectName = string.Empty;
            string sSubjectTempName = string.Empty;
            int startCol = 0;
            IList lstRows = new ArrayList();
            bool bFlag = false;
            DataTable tbSum = null;
            DataRow[] oDataRows = null;
            string sMaterialID = string.Empty;
            string sSubjectID = string.Empty;
            string sSubjectTempID = string.Empty;
            decimal SubjectSumNum = 0;
            decimal SubjectLastSumNum = 0;
            decimal SubjectLastSumMoney = 0;
            decimal SubjectSunMoney = 0;
            string sWhere = string.Empty;
            Hashtable ht = new Hashtable();
            int sStartColSubject = reportGrid.Cols - 1;
            //DataSet ds = model.StockInOutSrv.WZBB_GetCostAccountSubjectCat(beginDate, endDate, projectId, lstReslut);//获取核算科目类型
            int iEndCol = 0;
            //string sNotKnow = "无核算科目";
            QtyMoney oQtyMoney = null;
            if (dsSubject == null || dsSubject.Tables.Count == 0 || dsSubject.Tables[0].Rows.Count == 0) return;
            foreach (DataRow oRow in dsSubject.Tables[0].Rows)
            {
                sSubjectID = oRow["ID"].ToString();
                if (!ht.ContainsKey(sSubjectID))
                {
                    ht.Add(oRow["ID"].ToString(), new QtyMoney());
                }
                if (string.IsNullOrEmpty(sSubjectName))
                {
                    sSubjectName = oRow["ParentName"].ToString();
                    lstRows.Clear();
                    lstRows.Add(oRow);
                }
                else
                {
                    sSubjectTempName = oRow["ParentName"].ToString();
                    if (string.Equals(sSubjectTempName, sSubjectName))
                    {
                        lstRows.Add(oRow);
                    }
                    else
                    {
                        startCol = reportGrid.Cols - 1;
                        InsertGridCol4(startCol, 3, lstRows);
                        sSubjectName = sSubjectTempName;
                        lstRows.Clear();
                        lstRows.Add(oRow);
                    }
                }

            }
            if (lstRows != null && lstRows.Count > 0)
            {
                startCol = reportGrid.Cols - 1;
                InsertGridCol4(startCol, 3, lstRows);
            }

            startCol = reportGrid.Cols - 1;
            reportGrid.InsertCol(startCol, 4);
            iEndCol = startCol;
            reportGrid.Cell(3, startCol).Text = sNotKnow;
            reportGrid.Cell(3, startCol).Tag = sNotKnow;
            FlexCell.Range oRange = reportGrid.Range(3, startCol, 4, startCol + 3);
            oRange.MergeCells = true;
            oRange.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            oRange.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            oRange.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            reportGrid.Cell(5, startCol).Text = "本期数量";
            reportGrid.Cell(5, startCol + 1).Text = "本期金额";
            reportGrid.Cell(5, startCol+2).Text = "累计数量";
            reportGrid.Cell(5, startCol + 3).Text = "累计金额";
            ht.Add(sNotKnow, new QtyMoney());


            //dsSum = model.StockInOutSrv.WZBB_GetCostAccountSubjectSum(beginDate, endDate, projectId, lstReslut);
            if (dsSum == null || dsSum.Tables.Count == 0 || dsSum.Tables[0].Rows.Count == 0) return;
            tbSum = dsSum.Tables[0];
            for (int i = 3; i < reportGrid.Rows; i++)
            {
                sMaterialID = reportGrid.Cell(i, 1).Tag;
                if (!string.IsNullOrEmpty(sMaterialID))
                {
                    sWhere = string.Format("materialid='{0}'", sMaterialID);
                    oDataRows = tbSum.Select(sWhere);
                    if (oDataRows != null && oDataRows.Length > 0)
                    {
                        foreach (DataRow oRow in oDataRows)
                        {
                            SubjectSumNum = ClientUtil.ToDecimal(oRow["sumQuantity"]);
                            SubjectSunMoney = ClientUtil.ToDecimal(oRow["sumMoney"]);
                            SubjectLastSumNum = ClientUtil.ToDecimal(oRow["sumLstQuantity"]);
                            SubjectLastSumMoney = ClientUtil.ToDecimal(oRow["sumLstMoney"]);
                            sSubjectID = oRow["id"].ToString();
                            if (string.Equals(sSubjectID, sNotKnow))
                            {

                                if (ht.ContainsKey(sSubjectID) && ht[sSubjectID] != null)
                                {
                                    oQtyMoney = ht[sSubjectID] as QtyMoney;
                                    oQtyMoney.Qty += SubjectSumNum;
                                    oQtyMoney.Money += SubjectSunMoney;
                                    oQtyMoney.Ltmoney += SubjectLastSumMoney;
                                    oQtyMoney.Ltqty += SubjectLastSumNum;
                                    ht[sSubjectID] = oQtyMoney;
                                }
                                //reportGrid.Cell(i, j).Text = reportGrid.Cell(4, j).Text  ;
                                //reportGrid.Cell(i, j + 1).Text = reportGrid.Cell(4, j).Text  ;
                                reportGrid.Cell(i, iEndCol).Text = CommonUtil.NumberToStringFormate(SubjectSumNum, 4);
                                reportGrid.Cell(i, iEndCol + 1).Text = CommonUtil.NumberToStringFormate(SubjectSunMoney, 2);
                                reportGrid.Cell(i, iEndCol + 2).Text = CommonUtil.NumberToStringFormate(SubjectLastSumNum, 4);
                                reportGrid.Cell(i, iEndCol + 3).Text = CommonUtil.NumberToStringFormate(SubjectLastSumMoney, 2);
                            }
                            else
                            {
                                bFlag = false;
                                for (int j = reportGrid.Cols - 1; j >= sStartColSubject; j--)
                                {
                                    sSubjectTempID = reportGrid.Cell(4, j).Tag;
                                    if (string.IsNullOrEmpty(sSubjectTempID)) continue;
                                    if (string.Equals(sSubjectID, sSubjectTempID))
                                    {
                                        //SubjectSumNum = ClientUtil.ToDecimal(oRow["sumQuantity"]);
                                        //SubjectSunMoney = ClientUtil.ToDecimal(oRow["sumMoney"]);

                                        if (ht.ContainsKey(sSubjectID) && ht[sSubjectID] != null)
                                        {
                                            oQtyMoney = ht[sSubjectID] as QtyMoney;
                                            oQtyMoney.Qty += SubjectSumNum;
                                            oQtyMoney.Money += SubjectSunMoney;
                                            oQtyMoney.Ltmoney += SubjectLastSumMoney;
                                            oQtyMoney.Ltqty += SubjectLastSumNum;
                                            ht[sSubjectID] = oQtyMoney;
                                        }
                                        //reportGrid.Cell(i, j).Text = reportGrid.Cell(4, j).Text  ;
                                        //reportGrid.Cell(i, j + 1).Text = reportGrid.Cell(4, j).Text  ;
                                        reportGrid.Cell(i, j).Text = CommonUtil.NumberToStringFormate(SubjectSumNum, 4);
                                        reportGrid.Cell(i, j + 1).Text = CommonUtil.NumberToStringFormate(SubjectSunMoney, 2);
                                        reportGrid.Cell(i, j + 2).Text = CommonUtil.NumberToStringFormate(SubjectLastSumNum, 4);
                                        reportGrid.Cell(i, j + 3).Text = CommonUtil.NumberToStringFormate(SubjectLastSumMoney, 2);
                                        bFlag = true;
                                        break;
                                    }
                                }
                                if (!bFlag)
                                {
                                    //SubjectSumNum = ClientUtil.ToDecimal(oRow["sumQuantity"]);
                                    //SubjectSunMoney = ClientUtil.ToDecimal(oRow["sumMoney"]);
                                    if (ht.ContainsKey(sSubjectID) && ht[sSubjectID] != null)
                                    {
                                        oQtyMoney = ht[sSubjectID] as QtyMoney;
                                        oQtyMoney.Qty += SubjectSumNum;
                                        oQtyMoney.Money += SubjectSunMoney;
                                        oQtyMoney.Ltmoney += SubjectLastSumMoney;
                                        oQtyMoney.Ltqty += SubjectLastSumNum;
                                        ht[sSubjectID] = oQtyMoney;
                                    }
                                    //reportGrid.Cell(i, j).Text = reportGrid.Cell(4, j).Text  ;
                                    //reportGrid.Cell(i, j + 1).Text = reportGrid.Cell(4, j).Text  ;
                                    reportGrid.Cell(i, iEndCol).Text = CommonUtil.NumberToStringFormate(SubjectSumNum, 4);
                                    reportGrid.Cell(i, iEndCol + 1).Text = CommonUtil.NumberToStringFormate(SubjectSunMoney, 2);
                                    reportGrid.Cell(i, iEndCol + 2).Text = CommonUtil.NumberToStringFormate(SubjectLastSumNum, 4);
                                    reportGrid.Cell(i, iEndCol + 3).Text = CommonUtil.NumberToStringFormate(SubjectLastSumMoney, 2);
                                }
                            }
                        }

                    }
                }
            }
            for (int j = reportGrid.Cols - 1; j >= sStartColSubject; j--)
            {
                sSubjectTempID = reportGrid.Cell(4, j).Tag;

                if (!string.IsNullOrEmpty(sSubjectTempID) && ht.ContainsKey(sSubjectTempID))
                {
                    oQtyMoney = ht[sSubjectTempID] as QtyMoney;
                    if (oQtyMoney != null)
                    {
                        reportGrid.Cell(summaryRowIndex, j).Text = CommonUtil.NumberToStringFormate(oQtyMoney.Qty, 4);
                        reportGrid.Cell(summaryRowIndex, j + 1).Text = CommonUtil.NumberToStringFormate(oQtyMoney.Money, 2);
                        reportGrid.Cell(summaryRowIndex, j + 2).Text = CommonUtil.NumberToStringFormate(oQtyMoney.Ltqty, 4);
                        reportGrid.Cell(summaryRowIndex, j + 3).Text = CommonUtil.NumberToStringFormate(oQtyMoney.Ltmoney, 2);
                    }
                }

            }
            if (ht.ContainsKey(sNotKnow))
            {
                oQtyMoney = ht[sNotKnow] as QtyMoney;
                if (oQtyMoney != null)
                {
                    reportGrid.Cell(summaryRowIndex, iEndCol).Text = CommonUtil.NumberToStringFormate(oQtyMoney.Qty, 4);
                    reportGrid.Cell(summaryRowIndex, iEndCol + 1).Text = CommonUtil.NumberToStringFormate(oQtyMoney.Money, 2);
                    reportGrid.Cell(summaryRowIndex, iEndCol + 2).Text = CommonUtil.NumberToStringFormate(oQtyMoney.Ltqty, 4);
                    reportGrid.Cell(summaryRowIndex, iEndCol + 3).Text = CommonUtil.NumberToStringFormate(oQtyMoney.Ltmoney, 2);
                }
            }
        }
        /// <summary>
        /// 材料收发存汇总表
        /// </summary>
        private void FillReport_clsfchzb()
        {
            CurrentProjectInfo pi = cmbProject.SelectedItem as CurrentProjectInfo;
            string beginDate = dtpDateBegin.Value.ToShortDateString();
            string endDate = dtpDateEnd.Value.ToShortDateString();

            string reportName = cmbReportName.SelectedItem + "";
            bool isSummary = false;
            if (reportName == RN_clsfchzb_lj)
            {
                isSummary = true;
            }

            DataSet ds = model.StockInOutSrv.WZBB_clsfchzb(beginDate, endDate, pi.Id, isSummary);
            Hashtable cjclcb = model.StockInOutSrv.WZBB_cjclcbje(beginDate, endDate, pi.Id, isSummary);
            decimal sumMoney = 0;
            //reportGrid.Column(0).
            if (ds == null || ds.Tables.Count == 0) return;
            DataTable dt = ds.Tables[0];
            if (dt == null) return;

            reportGrid.Cell(2, 1).Text = "单位名称：" + pi.Name;
            
            reportGrid.Cell(2, 14).Text = "期次：" + beginDate + " 至 " + endDate;
            if (isSummary)
            {
                reportGrid.Cell(2, 14).Text = "期次：统计截止时间" + endDate;
            }

            int detailStartRowNumber = 6;//6为模板中的行号
            int detailCount = dt.Rows.Count;
            if (detailCount == 0) return;
            //插入报表行
            reportGrid.InsertRow(detailStartRowNumber, detailCount);
            decimal sumLastQty = 0M, sumLastMoney = 0M, sumCgsl = 0M, sumCgje = 0M, sumLjcgsl = 0M, sumLjcgje = 0M, sumDbrksl = 0M, sumDbrkje = 0M, sumLjdbrksl = 0M, sumLjdbrkje = 0M,
                sumPysl = 0M, sumPyje = 0M, sumLjpysl = 0M, sumLjpyje = 0M, sumXhsl = 0M, sumXhje = 0M, sumLjxhsl = 0M, sumLjxhje = 0M, sumDbcksl = 0M, sumDbckje = 0M, sumLjdbcksl = 0M,
                sumLjdbckje = 0M, sumPksl = 0M, sumPkje = 0M, sumLjpksl = 0M, sumLjpkje = 0M, sumJcsl = 0M, sumJcje = 0M;
            for (int i = 0; i < detailCount; i++)
            {
                DataRow dr = dt.Rows[i];
                string matname = dr["name"] + "";

                decimal lastQty = ClientUtil.ToDecimal(dr["lastQty"]);
                decimal lastMoney = ClientUtil.ToDecimal(dr["lastMoney"]);
                decimal cgsl = ClientUtil.ToDecimal(dr["cgsl"]);
                decimal cgje = ClientUtil.ToDecimal(dr["cgje"]);
                decimal dbrksl = ClientUtil.ToDecimal(dr["dbrksl"]);
                decimal dbrkje = ClientUtil.ToDecimal(dr["dbrkje"]);

                decimal pysl = ClientUtil.ToDecimal(dr["pysl"]);
                decimal pyje = ClientUtil.ToDecimal(dr["pyje"]);

                decimal xhsl = ClientUtil.ToDecimal(dr["xhsl"]);
                decimal xhje = ClientUtil.ToDecimal(dr["xhje"]);
      
                decimal dbcksl = ClientUtil.ToDecimal(dr["dbcksl"]);
                decimal dbckje = ClientUtil.ToDecimal(dr["dbckje"]);
       
                decimal pksl = ClientUtil.ToDecimal(dr["pksl"]);
                decimal pkje = ClientUtil.ToDecimal(dr["pkje"]);
           
                decimal jcsl = ClientUtil.ToDecimal(dr["jcsl"]);
                decimal jcje = ClientUtil.ToDecimal(dr["jcje"]);
                sumLastQty += lastQty;
                sumLastMoney += lastMoney;
                sumCgsl += cgsl;
                sumCgje += cgje;
          
                sumDbrksl += dbrksl;
                sumDbrkje += dbrkje;
              
                sumPysl += pysl;
                sumPyje += pyje;
             
                sumXhsl += xhsl;
                sumXhje += xhje;
               
                sumDbcksl += dbcksl;
                sumDbckje += dbckje;
               
                sumPksl += pksl;
                sumPkje += pkje;
                
                sumJcsl += jcsl;
                sumJcje += jcje;
                reportGrid.Cell(detailStartRowNumber + i, 1).Text = matname;//物资名称
                reportGrid.Cell(detailStartRowNumber + i, 1).Tag = matname;//物资名称
    
                reportGrid.Cell(detailStartRowNumber + i, 4).Text = CommonUtil.NumberToStringFormate(lastQty,4);//
                reportGrid.Cell(detailStartRowNumber + i, 5).Text = CommonUtil.NumberToStringFormate(lastMoney,2);//
                reportGrid.Cell(detailStartRowNumber + i, 6).Text = CommonUtil.NumberToStringFormate(cgsl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 7).Text = CommonUtil.NumberToStringFormate(cgje,2);//
           
                reportGrid.Cell(detailStartRowNumber + i, 8).Text = CommonUtil.NumberToStringFormate(dbrksl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 9).Text = CommonUtil.NumberToStringFormate(dbrkje,2);//
     
                reportGrid.Cell(detailStartRowNumber + i, 10).Text = CommonUtil.NumberToStringFormate(xhsl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 11).Text = CommonUtil.NumberToStringFormate(xhje,2);//
           
                reportGrid.Cell(detailStartRowNumber + i, 12).Text = CommonUtil.NumberToStringFormate(dbcksl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 13).Text = CommonUtil.NumberToStringFormate(dbckje,2);//
            
                //reportGrid.Cell(detailStartRowNumber + i, 14).Text = (pysl - pksl),4);//
                reportGrid.Cell(detailStartRowNumber + i, 15).Text = CommonUtil.NumberToStringFormate(pkje,2);//
             
                reportGrid.Cell(detailStartRowNumber + i, 16).Text = CommonUtil.NumberToStringFormate(jcsl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 17).Text = CommonUtil.NumberToStringFormate(jcje,2);//
            }

            //冲减材料成本行
            //reportGrid.Cell(detailStartRowNumber + detailCount, 1).Text = "冲减材料成本";
            //if (cjclcb.Count != 0)
            //{
            //    decimal strmatMoney = 0;
                 
            //    //获得名称及其对应的层次码
            //    Hashtable hashtableSysCode = new Hashtable();
            //    string strSearchCode = "select usedPartName,usedPartSysCode from THD_WasteMatProcessDetail";
            //    DataTable strCodedt = model.ExcelImportSrv.SearchSql(strSearchCode);
            //    for (int k = 0; k < strCodedt.Rows.Count; k++)
            //    {
            //        string strName = strCodedt.Rows[k][0].ToString();
            //        string strSysCode = strCodedt.Rows[k][1].ToString();
            //        if (strName != "" && strSysCode != "")
            //        {
            //            if (!hashtableSysCode.Contains(strName))
            //            {
            //                hashtableSysCode.Add(strName, strSysCode);
            //            }
            //        }
            //    }
            //    foreach (System.Collections.DictionaryEntry objCode in cjclcb)
            //    {
            //        strmatMoney = ClientUtil.ToDecimal(objCode.Value);
            //        sumMoney += strmatMoney;
            //    }
            //    reportGrid.Cell(detailStartRowNumber + detailCount, 11).Text = ClientUtil.ToString(-sumMoney);
            //}

            //插入合计
            int summaryRowIndex = detailStartRowNumber + detailCount;//合计行行号
            reportGrid.Cell(summaryRowIndex, 1).Text = "合计";//物资名称
            reportGrid.Cell(summaryRowIndex, 4).Text = CommonUtil.NumberToStringFormate(sumLastQty,4);//
            reportGrid.Cell(summaryRowIndex, 5).Text = CommonUtil.NumberToStringFormate(sumLastMoney,2);//
            reportGrid.Cell(summaryRowIndex, 6).Text = CommonUtil.NumberToStringFormate(sumCgsl,4);//
            reportGrid.Cell(summaryRowIndex, 7).Text = CommonUtil.NumberToStringFormate(sumCgje,2);//
    
            reportGrid.Cell(summaryRowIndex, 8).Text = CommonUtil.NumberToStringFormate(sumDbrksl,4);//
            reportGrid.Cell(summaryRowIndex, 9).Text = CommonUtil.NumberToStringFormate(sumDbrkje,2);//
        
            reportGrid.Cell(summaryRowIndex, 10).Text = CommonUtil.NumberToStringFormate(sumXhsl,4);//
            reportGrid.Cell(summaryRowIndex, 11).Text = CommonUtil.NumberToStringFormate((sumXhje - sumMoney),2);//
       
            reportGrid.Cell(summaryRowIndex, 12).Text = CommonUtil.NumberToStringFormate(sumDbcksl,4);//
            reportGrid.Cell(summaryRowIndex, 13).Text = CommonUtil.NumberToStringFormate(sumDbckje,2);//
           
            //reportGrid.Cell(summaryRowIndex, 14).Text = (sumPysl - sumPksl),4);//
            reportGrid.Cell(summaryRowIndex, 15).Text = CommonUtil.NumberToStringFormate(sumPkje,2);//
          
            reportGrid.Cell(summaryRowIndex, 16).Text = CommonUtil.NumberToStringFormate(sumJcsl,4);//
            reportGrid.Cell(summaryRowIndex, 17).Text = CommonUtil.NumberToStringFormate(sumJcje, 2);//
           
            //消耗明细
             
            //    Xhmx_clsfchzb(beginDate, endDate, pi.Id, summaryRowIndex, isSummary);//old

            if (this.IsSelectParts)
            {
                IList lstUsePart = this.txtUsePart.Tag as IList;
                Xhmx_clsfchzb(beginDate, endDate, pi.Id, summaryRowIndex, lstUsePart, isSummary);
            }
            else
            {
                DataSet dsUserPart = model.StockInOutSrv.WZBB_clsfchzb_UserPart(beginDate, endDate, pi.Id, isSummary);//获取材料收发存汇总表 部位信息
                DataSet dsXHSum = model.StockInOutSrv.WZBB_clsfchzb_xhmx(beginDate, endDate, pi.Id, isSummary);
                Xhmx_Active_Subject(summaryRowIndex, dsUserPart, dsXHSum, "未知部位");
            }

            //Hashtable hashtableSysCode1 = new Hashtable();
            //string strSearchCode1 = "select usedPartName,usedPartSysCode from THD_WasteMatProcessDetail";
            //DataTable strCodedt1 = model.ExcelImportSrv.SearchSql(strSearchCode1);
            //for (int k = 0; k < strCodedt1.Rows.Count; k++)
            //{
            //    string strName = strCodedt1.Rows[k][0].ToString();
            //    string strSysCode = strCodedt1.Rows[k][1].ToString();
            //    if (strName != "" && strSysCode != "")
            //    {
            //        if (!hashtableSysCode1.Contains(strName))
            //        {
            //            hashtableSysCode1.Add(strName, strSysCode);
            //        }
            //    }
            //}
            //IList usedPartLst = GetUsedPart(pi.Id);
            //if (usedPartLst == null || usedPartLst.Count == 0) return;
            //foreach (GWBSTree usedPart in usedPartLst)
            //{
            //    int startCol = reportGrid.Cols - 1;
            //    InsertGridCol4Clsfchzb(startCol, 3, usedPart.Name);//3为报表格式中的行号
            //    decimal sumXhMoney = 0M;
            //    foreach (System.Collections.DictionaryEntry objCode in hashtableSysCode1)
            //    {

            //        if (objCode.Key.ToString().Equals(usedPart.Name))
            //        {
            //            string strMatCode = ClientUtil.ToString(objCode.Value);
            //            foreach (System.Collections.DictionaryEntry objMoney in cjclcb)
            //            {
            //                if (objMoney.Key.ToString().Equals(strMatCode))
            //                {
            //                    //获得金额
            //                    decimal strMatMoney = ClientUtil.ToDecimal(objMoney.Value);
            //                    //int machedRowIndex = MachedRowIndex(usedPart.Name);
            //                    //if (machedRowIndex == int.MinValue) continue;
            //                    reportGrid.Cell(detailStartRowNumber + detailCount, startCol + 1).Text = (-strMatMoney),2);
            //                    sumXhMoney +=(-strMatMoney);
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //    reportGrid.Cell(summaryRowIndex, startCol + 1).Text = sumXhMoney,2);
            //}

            FlexCell.Range range = reportGrid.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount, 3);
            range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);

            range = reportGrid.Range(detailStartRowNumber, 4, detailStartRowNumber + detailCount, reportGrid.Cols - 2);
            range.Alignment = FlexCell.AlignmentEnum.RightCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);

            ///累计汇总表隐藏期初数据
            if (cmbReportName.SelectedItem + "" == RN_clsfchzb_lj)
            {
                reportGrid.Column(4).Visible = false;
                reportGrid.Column(5).Visible = false;
            }
        }
        private void Xhmx_clsfchzb(string beginDate, string endDate, string projectId, int summaryRowIndex, IList lstUsePart, bool isSummary)
        {
            string sNotKnow = "其他部位消耗";
            string sSubjectTempID = string.Empty;
            QtyMoney oQtyMoney=null;
            if (lstUsePart != null && lstUsePart.Count > 0)
            {
                Hashtable hs = SetHeadUsePart(lstUsePart, Headtype.Current, sNotKnow);
                 
                string sOther = "调整金额";
                if (!hs.ContainsKey(sOther))
                {
                    hs.Add(sOther, new QtyMoney());
                   
                    //int startCol = reportGrid.Cols - 1;
                    //reportGrid.InsertCol(startCol, 2);
                    //reportGrid.Cell(3, startCol).Text = sOther;
                    //reportGrid.Cell(3, startCol+1).Text = sOther;
                    //reportGrid.Cell(3, startCol+1).Tag = sOther;
                    //reportGrid.Cell(3, startCol ).Tag = sOther;

                    //reportGrid.Cell(5, startCol).Text = "金额";//金额
                    //reportGrid.Cell(5, startCol + 1).Text = "金额";
                    //FlexCell.Range oRange = reportGrid.Range(3, startCol, 4, startCol + 1);
                    //oRange.MergeCells = true;
                    //oRange.Alignment = FlexCell.AlignmentEnum.CenterCenter;
                    //oRange.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
                    //oRange.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
                    //reportGrid.Column(startCol ).Visible = false;
                }
                if (hs.Count > 0)
                {
                    DataSet ds = model.StockInOutSrv.WZBB_clsfchzb_xhmx(beginDate, endDate, projectId, lstUsePart, isSummary);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        FillText(ds.Tables[0], hs, Headtype.Current, summaryRowIndex, sNotKnow);
                    }
                    for (int j = reportGrid.Cols - 1; j >= 0; j--)
                    {
                        sSubjectTempID = reportGrid.Cell(4, j).Tag;
                        if (string.IsNullOrEmpty(sSubjectTempID))
                        {
                            sSubjectTempID = reportGrid.Cell(3, j).Tag;
                        }
                        if (string.IsNullOrEmpty(sSubjectTempID)) { continue; }
                        if (hs.ContainsKey(sSubjectTempID))
                        {
                            oQtyMoney = hs[sSubjectTempID] as QtyMoney;
                            if (oQtyMoney.Ltmoney == 0 && oQtyMoney.Ltqty == 0 && oQtyMoney.Money == 0 && oQtyMoney.Qty == 0)
                            {
                                reportGrid.Column(j).Visible = false;
                                reportGrid.Column(j + 1).Visible = false; 
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 为何后添加动态列  2列
        /// </summary>
        /// <param name="summaryRowIndex"></param>
        /// <param name="lstReslut"></param>
        /// <param name="dsSubject"></param>
        /// <param name="dsSum"></param>
        private void Xhmx_Active_Subject(int summaryRowIndex,  DataSet dsSubject, DataSet dsSum, string sNotKnow)
        {
            string sSubjectName = string.Empty;
            string sSubjectTempName = string.Empty;
            int startCol = 0;
            IList lstRows = new ArrayList();
            bool bFlag = false;
            DataTable tbSum = null;
            DataRow[] oDataRows = null;
            string sMaterialID = string.Empty;
            string sSubjectID = string.Empty;
            string sSubjectTempID = string.Empty;
            decimal SubjectSumNum = 0;
            decimal SubjectSunMoney = 0;
            string sWhere = string.Empty;
            Hashtable ht = new Hashtable();
            int sStartColSubject = reportGrid.Cols - 1;
            //DataSet ds = model.StockInOutSrv.WZBB_GetCostAccountSubjectCat(beginDate, endDate, projectId, lstReslut);//获取核算科目类型
            int iEndCol = 0;
            //string sNotKnow = "无核算科目";
            QtyMoney oQtyMoney = null;
            if (dsSubject == null || dsSubject.Tables.Count == 0 || dsSubject.Tables[0].Rows.Count == 0) return;
            foreach (DataRow oRow in dsSubject.Tables[0].Rows)
            {
                sSubjectID = oRow["ID"].ToString();
                if (!ht.ContainsKey(sSubjectID))
                {
                    ht.Add(oRow["ID"].ToString(), new QtyMoney());
                }
                if (string.IsNullOrEmpty(sSubjectName))
                {
                    sSubjectName = oRow["ParentName"].ToString();
                    lstRows.Clear();
                    lstRows.Add(oRow);
                }
                else
                {
                    sSubjectTempName = oRow["ParentName"].ToString();
                    if (string.Equals(sSubjectTempName, sSubjectName))
                    {
                        lstRows.Add(oRow);
                    }
                    else
                    {
                        startCol = reportGrid.Cols - 1;
                        InsertGridCol(startCol, 3, lstRows);
                        sSubjectName = sSubjectTempName;
                        lstRows.Clear();
                        lstRows.Add(oRow);
                    }
                }

            }
            if (lstRows != null && lstRows.Count > 0)
            {
                startCol = reportGrid.Cols - 1;
                InsertGridCol(startCol, 3, lstRows);
            }

            startCol = reportGrid.Cols - 1;
            reportGrid.InsertCol(startCol, 2);
            iEndCol = startCol;
            reportGrid.Cell(3, startCol).Text = sNotKnow;
            reportGrid.Cell(3, startCol).Tag = sNotKnow;
            FlexCell.Range oRange = reportGrid.Range(3, startCol, 4, startCol + 1);
            oRange.MergeCells = true;
            oRange.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            oRange.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            oRange.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            reportGrid.Cell(5, startCol).Text = "本期数量";
            reportGrid.Cell(5, startCol + 1).Text = "本期金额";
            ht.Add(sNotKnow, new QtyMoney());


            //dsSum = model.StockInOutSrv.WZBB_GetCostAccountSubjectSum(beginDate, endDate, projectId, lstReslut);
            if (dsSum == null || dsSum.Tables.Count == 0 || dsSum.Tables[0].Rows.Count == 0) return;
            tbSum = dsSum.Tables[0];
            for (int i = 3; i < reportGrid.Rows; i++)
            {
                sMaterialID = reportGrid.Cell(i, 1).Tag;
                if (string.Equals(sMaterialID, "料具租赁"))
                {
                    string sss = "";
                }
                if (!string.IsNullOrEmpty(sMaterialID))
                {
                    sWhere = string.Format("materialid='{0}'", sMaterialID);
                    oDataRows = tbSum.Select(sWhere);
                    if (oDataRows != null && oDataRows.Length > 0)
                    {
                        foreach (DataRow oRow in oDataRows)
                        {
                            sSubjectID = oRow["id"].ToString();
                            if (string.Equals(sSubjectID, sNotKnow))
                            {
                                SubjectSumNum = ClientUtil.ToDecimal(oRow["sumQuantity"]);
                                SubjectSunMoney = ClientUtil.ToDecimal(oRow["sumMoney"]);
                                if (ht.ContainsKey(sSubjectID) && ht[sSubjectID] != null)
                                {
                                    oQtyMoney = ht[sSubjectID] as QtyMoney;
                                    oQtyMoney.Qty += SubjectSumNum;
                                    oQtyMoney.Money += SubjectSunMoney;
                                    ht[sSubjectID] = oQtyMoney;
                                }
                                //reportGrid.Cell(i, j).Text = reportGrid.Cell(4, j).Text  ;
                                //reportGrid.Cell(i, j + 1).Text = reportGrid.Cell(4, j).Text  ;
                                reportGrid.Cell(i, iEndCol).Text =CommonUtil .NumberToStringFormate ( ClientUtil.ToDecimal(reportGrid.Cell(i, iEndCol).Text) + SubjectSumNum,4) ;
                                reportGrid.Cell(i, iEndCol + 1).Text = CommonUtil.NumberToStringFormate(ClientUtil.ToDecimal(reportGrid.Cell(i, iEndCol + 1).Text) + SubjectSunMoney,2);  
                            }
                            else
                            {
                                bFlag = false;
                                for (int j = reportGrid.Cols - 1; j >= sStartColSubject; j--)
                                {
                                    sSubjectTempID = reportGrid.Cell(4, j).Tag;
                                    if (string.IsNullOrEmpty(sSubjectTempID)) continue;
                                    if (string.Equals(sSubjectID, sSubjectTempID))
                                    {
                                        SubjectSumNum = ClientUtil.ToDecimal(oRow["sumQuantity"]);
                                        SubjectSunMoney = ClientUtil.ToDecimal(oRow["sumMoney"]);
                                        if (ht.ContainsKey(sSubjectID) && ht[sSubjectID] != null)
                                        {
                                            oQtyMoney = ht[sSubjectID] as QtyMoney;
                                            oQtyMoney.Qty += SubjectSumNum;
                                            oQtyMoney.Money += SubjectSunMoney;
                                            ht[sSubjectID] = oQtyMoney;
                                        }
                                        //reportGrid.Cell(i, j).Text = reportGrid.Cell(4, j).Text  ;
                                        //reportGrid.Cell(i, j + 1).Text = reportGrid.Cell(4, j).Text  ;
                                        reportGrid.Cell(i, j).Text = CommonUtil.NumberToStringFormate(SubjectSumNum, 4);
                                        reportGrid.Cell(i, j + 1).Text = CommonUtil.NumberToStringFormate(SubjectSunMoney, 2);
                                        bFlag = true ;
                                        break;
                                    }
                                }
                                if (!bFlag)
                                {
                                    SubjectSumNum = ClientUtil.ToDecimal(oRow["sumQuantity"]);
                                    SubjectSunMoney = ClientUtil.ToDecimal(oRow["sumMoney"]);
                                    sSubjectID = sNotKnow;
                                    if (ht.ContainsKey(sSubjectID) && ht[sSubjectID] != null)
                                    {
                                        oQtyMoney = ht[sSubjectID] as QtyMoney;
                                        oQtyMoney.Qty += SubjectSumNum;
                                        oQtyMoney.Money += SubjectSunMoney;
                                        ht[sSubjectID] = oQtyMoney;
                                    }
                                    //reportGrid.Cell(i, j).Text = reportGrid.Cell(4, j).Text  ;
                                    //reportGrid.Cell(i, j + 1).Text = reportGrid.Cell(4, j).Text  ;
                                    //reportGrid.Cell(i, iEndCol).Text = CommonUtil.NumberToStringFormate(SubjectSumNum, 4);
                                    //reportGrid.Cell(i, iEndCol + 1).Text = CommonUtil.NumberToStringFormate(SubjectSunMoney, 2);
                                    reportGrid.Cell(i, iEndCol).Text = CommonUtil.NumberToStringFormate(ClientUtil.ToDecimal(reportGrid.Cell(i, iEndCol).Text) + SubjectSumNum, 4);
                                    reportGrid.Cell(i, iEndCol + 1).Text = CommonUtil.NumberToStringFormate(ClientUtil.ToDecimal(reportGrid.Cell(i, iEndCol + 1).Text) + SubjectSunMoney, 2);  
                                }
                            }
                        }

                    }
                }
            }
            for (int j = reportGrid.Cols - 1; j >= sStartColSubject; j--)
            {
                sSubjectTempID = reportGrid.Cell(4, j).Tag;

                if (!string.IsNullOrEmpty(sSubjectTempID) && ht.ContainsKey(sSubjectTempID))
                {
                    oQtyMoney = ht[sSubjectTempID] as QtyMoney;
                    if (oQtyMoney != null)
                    {
                        reportGrid.Cell(summaryRowIndex, j).Text = CommonUtil.NumberToStringFormate(oQtyMoney.Qty, 4);
                        reportGrid.Cell(summaryRowIndex, j + 1).Text = CommonUtil.NumberToStringFormate(oQtyMoney.Money, 2);
                    }
                }

            }
            if (ht.ContainsKey(sNotKnow))
            {
                oQtyMoney = ht[sNotKnow] as QtyMoney;
                if (oQtyMoney != null)
                {
                    reportGrid.Cell(summaryRowIndex, iEndCol).Text = CommonUtil.NumberToStringFormate(oQtyMoney.Qty, 4);
                    reportGrid.Cell(summaryRowIndex, iEndCol + 1).Text = CommonUtil.NumberToStringFormate(oQtyMoney.Money, 2);
                }
            }
        }
        /// <summary>
        /// 材料收发存汇总表 消耗明细
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="projectId"></param>
        /// <param name="summaryRowIndex"></param>
        /// <param name="isSummary">是否汇总表 true 汇总表</param>
        private void Xhmx_clsfchzb(string beginDate, string endDate, string projectId, int summaryRowIndex, bool isSummary)
        {
            IList usedPartLst = GetUsedPart(projectId);
            if (usedPartLst == null || usedPartLst.Count == 0) return;
            foreach (GWBSTree usedPart in usedPartLst)
            {
                int startCol = reportGrid.Cols - 1;
                InsertGridCol4Clsfchzb(startCol, 3, usedPart.Name);//3为报表格式中的行号
                DataSet ds = model.StockInOutSrv.WZBB_clsfchzb_xhmx(beginDate, endDate, projectId, usedPart.SysCode, isSummary);
                if (ds == null || ds.Tables.Count == 0) continue;
                DataTable dt = ds.Tables[0];
                if (dt == null || dt.Rows.Count == 0) continue;
                decimal sumXhsl = 0M, sumXhje = 0M, sumLjxhsl = 0M, sumLjxhje = 0M;
                foreach (DataRow dr in dt.Rows)
                {
                    string matname = dr["name"] + "";
                    decimal xhsl = ClientUtil.ToDecimal(dr["xhsl"]);
                    decimal xhje = ClientUtil.ToDecimal(dr["xhje"]);

                    int machedRowIndex = MachedRowIndex(matname);
                    if (machedRowIndex == int.MinValue) continue;
                    reportGrid.Cell(machedRowIndex, startCol).Text = CommonUtil.NumberToStringFormate(xhsl,4);
                    reportGrid.Cell(machedRowIndex, startCol + 1).Text = CommonUtil.NumberToStringFormate(xhje,2);
                    sumXhsl += xhsl;
                    sumXhje += xhje;
                }
                reportGrid.Cell(summaryRowIndex, startCol).Text = CommonUtil.NumberToStringFormate(sumXhsl,4);
                reportGrid.Cell(summaryRowIndex, startCol + 1).Text = CommonUtil.NumberToStringFormate(sumXhje, 2);
            }
        }

        /// <summary>
        /// 调拨报表
        /// </summary>
        private void FillReport_db()
        {
            CurrentProjectInfo pi = cmbProject.SelectedItem as CurrentProjectInfo;
            string beginDate = dtpDateBegin.Value.ToShortDateString();
            string endDate = dtpDateEnd.Value.ToShortDateString();

            DataSet ds = new DataSet();
            string reportName = cmbReportName.SelectedItem + "";
            bool isDbck = false;
            if (reportName == RN_dbcltjb_ck)
            {
                ds = model.StockInOutSrv.WZBB_dbck(beginDate, endDate, pi.Id);
                isDbck = true;
            }
            else if (reportName == RN_dbcltjb_rk)
            {
                ds = model.StockInOutSrv.WZBB_dbrk(beginDate, endDate, pi.Id,false);
            }
            else if (reportName == RN_dbcltjb_rk_jg)
            {
                ds = model.StockInOutSrv.WZBB_dbrk(beginDate, endDate, pi.Id, true);                
            }

            //reportGrid.Column(0).
            if (ds == null || ds.Tables.Count == 0) return;
            DataTable dt = ds.Tables[0];
            if (dt == null) return;

            reportGrid.Cell(2, 1).Text = "期次：" + beginDate + " 至 " + endDate;

            int detailStartRowNumber = 5;//5为模板中的行号
            int detailCount = dt.Rows.Count;
            if (detailCount == 0) return;
            //插入报表行
            reportGrid.InsertRow(detailStartRowNumber, detailCount);

            decimal sumBqsl = 0M, sumBqje = 0M, sumLjsl = 0M, sumLjje = 0M,sumBqykje=0M,sumLjykje=0M;
            for (int i = 0; i < detailCount; i++)
            {
                DataRow dr = dt.Rows[i];
                string moveoutprojectname = dr["moveoutprojectname"] + "";
                string materialname = dr["materialname"] + "";
                string materialspec = dr["materialspec"] + "";
                string matstandardunitname = dr["matstandardunitname"] + "";

                decimal bqsl = ClientUtil.ToDecimal(dr["bqsl"]);
                decimal bqje = ClientUtil.ToDecimal(dr["bqje"]);
                decimal ljsl = ClientUtil.ToDecimal(dr["ljsl"]);
                decimal ljje = ClientUtil.ToDecimal(dr["ljje"]);
                decimal bqykje = 0M;
                decimal ljykje = 0M;
                if (isDbck)
                {
                    bqykje = ClientUtil.ToDecimal(dr["bqykje"]);
                    ljykje = ClientUtil.ToDecimal(dr["ljykje"]);
                }

                sumBqsl += bqsl;
                sumBqje += bqje;
                sumLjsl += ljsl;
                sumLjje += ljje;
                sumBqykje += bqykje;
                sumLjykje += ljykje;

                reportGrid.Cell(detailStartRowNumber + i, 1).Text = moveoutprojectname;//调拨单位

                reportGrid.Cell(detailStartRowNumber + i, 2).Text = materialname;//
                reportGrid.Cell(detailStartRowNumber + i, 3).Text = materialspec;//
                reportGrid.Cell(detailStartRowNumber + i, 4).Text = matstandardunitname;//

                reportGrid.Cell(detailStartRowNumber + i, 5).Text = CommonUtil.NumberToStringFormate(bqsl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 6).Text = CommonUtil.NumberToStringFormate(bqje,2);//

                reportGrid.Cell(detailStartRowNumber + i, 7).Text = CommonUtil.NumberToStringFormate(ljsl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 8).Text = CommonUtil.NumberToStringFormate(ljje,2);//

                if (isDbck)
                { 
                    //调拨出库增加盈亏列
                    reportGrid.Cell(detailStartRowNumber + i, 10).Text = CommonUtil.NumberToStringFormate(bqykje,2);//
                    reportGrid.Cell(detailStartRowNumber + i, 12).Text = CommonUtil.NumberToStringFormate(ljykje,2);//
                }
                
            }
            //插入合计
            int summaryRowIndex = detailStartRowNumber + detailCount;//合计行行号
            reportGrid.Cell(summaryRowIndex, 1).Text = "合计";//物资名称

            reportGrid.Cell(summaryRowIndex, 5).Text = CommonUtil.NumberToStringFormate(sumBqsl,4);//
            reportGrid.Cell(summaryRowIndex, 6).Text = CommonUtil.NumberToStringFormate(sumBqje,2);//

            reportGrid.Cell(summaryRowIndex, 7).Text = CommonUtil.NumberToStringFormate(sumLjsl,4);//
            reportGrid.Cell(summaryRowIndex, 8).Text = CommonUtil.NumberToStringFormate(sumLjje,2);//   

            if (isDbck)
            {
                //调拨出库增加盈亏列
                reportGrid.Cell(summaryRowIndex, 10).Text = CommonUtil.NumberToStringFormate(sumBqykje,2);//
                reportGrid.Cell(summaryRowIndex, 12).Text = CommonUtil.NumberToStringFormate(sumLjykje, 2);//
            }

            FlexCell.Range range = reportGrid.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount, 4);
            range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);

            range = reportGrid.Range(detailStartRowNumber, 5, detailStartRowNumber + detailCount, reportGrid.Cols - 2);
            range.Alignment = FlexCell.AlignmentEnum.RightCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
        }

        /// <summary>
        /// 材料收发存台帐记录卡
        /// </summary>
        private void FillReport_clsfctz()
        {
            CurrentProjectInfo pi = cmbProject.SelectedItem as CurrentProjectInfo;
            string beginDate = dtpDateBegin.Value.ToShortDateString();
            string endDate = dtpDateEnd.Value.ToShortDateString();
            if (txtMaterial.Text == "" || txtMaterial.Result == null || txtMaterial.Result.Count == 0)
            {
                MessageBox.Show("请先选择一个物资。");
                txtMaterial.Focus();
                return;
            }
            Material material = txtMaterial.Result[0] as Material;
            DataSet ds = model.StockInOutSrv.WZBB_clsfctz(pi.Id, material.Id, beginDate, endDate);

            reportGrid.Cell(2, 2).Text = material.Code;
            if (material.Stuff == null)
            {
                reportGrid.Cell(2, 3).Text = "名称 "+material.Name;
            }
            else
            {
                reportGrid.Cell(2, 3).Text = "名称 " + material.Name+"("+material.Stuff+")";
            }
            reportGrid.Cell(3,2).Text = material.Specification;
            reportGrid.Cell(3, 3).Text = "单位 " + material.BasicUnit.Name;
            reportGrid.Cell(4, 1).Text = "单位名称：" + pi.Name;

            //reportGrid.Column(0).
            if (ds == null || ds.Tables.Count == 0) return;
            DataTable dt = ds.Tables[0];
            if (dt == null) return;
           

            int detailStartRowNumber = 8;//8为模板中的行号
            int detailCount = dt.Rows.Count;
            if (detailCount == 0) return;
            
            //插入报表行
            reportGrid.InsertRow(detailStartRowNumber, detailCount);

            ///消耗明细起始列
            int startCols = reportGrid.Cols - 1; 

            //插入使用部位列
            WZBB_clsfctz_addColsOfUsedPart(pi.Id);

            int summaryRowIndex = detailStartRowNumber + detailCount;//合计行行号

            decimal jcsl = 0M, jcje = 0M ,sumCgsl=0M,sumCgje=0M,sumJssl=0M,sumJsje=0M,sumDbrksl=0M,sumDbrkje=0M,sumRksl=0M,sumRkje=0M,
                sumLlcksl=0M,sumLlckje=0M,sumDbcksl=0M,sumDbckje=0M,sumCksl=0M,sumCkje=0M,sumYkje=0M;
            for (int i = 0; i < detailCount; i++)
            {
                DataRow dr = dt.Rows[i];
                string djlx = dr["djlx"] + "";
                string createdate = dr["createdate"] + "";
                string code = dr["code"] + "";
                string supplierrelationname = dr["supplierrelationname"] + "";

                decimal price = ClientUtil.ToDecimal(dr["price"]);
                decimal cgsl = ClientUtil.ToDecimal(dr["cgsl"]);
                decimal cgje = ClientUtil.ToDecimal(dr["cgje"]);
                decimal jssl = ClientUtil.ToDecimal(dr["jssl"]);
                decimal  jsje= ClientUtil.ToDecimal(dr["jsje"]);
                decimal dbrksl = ClientUtil.ToDecimal(dr["dbrksl"]);
                decimal dbrkje = ClientUtil.ToDecimal(dr["dbrkje"]);
                decimal rksl = ClientUtil.ToDecimal(dr["rksl"]);
                decimal rkje = ClientUtil.ToDecimal(dr["rkje"]);
                decimal llcksl = ClientUtil.ToDecimal(dr["llcksl"]);
                decimal llckje = ClientUtil.ToDecimal(dr["llckje"]);
                decimal dbcksl = ClientUtil.ToDecimal(dr["dbcksl"]);
                decimal dbckje = ClientUtil.ToDecimal(dr["dbckje"]);
                decimal cksl = ClientUtil.ToDecimal(dr["cksl"]);
                decimal ckje = ClientUtil.ToDecimal(dr["ckje"]);
                decimal ykje = ClientUtil.ToDecimal(dr["ykje"]);
                decimal tempckje = ClientUtil.ToDecimal(dr["tempckje"]);
                jcsl = jcsl + rksl - cksl;
                jcje = jcje + rkje - tempckje;
                //jcje = jcje + rkje - ckje + ykje;
                //jcje = jcje + rkje - ckje;
                if (i == detailCount - 1 && jcje <= decimal.Parse("0.05") && decimal.Parse("-0.05") <= jcje)
                {
                    jcje = 0;
                }

                sumCgsl += cgsl;
                sumCgje += cgje;
                sumJssl += jssl;
                sumJsje += jsje;
                sumDbrksl += dbrksl;
                sumDbrkje += dbrkje;
                sumRksl += rksl;
                sumRkje += rkje;
                sumLlcksl += llcksl;
                sumLlckje += llckje;
                sumDbcksl += dbcksl;
                sumDbckje += dbckje;
                sumCksl += cksl;
                sumCkje += ckje;
                sumYkje += ykje;

                reportGrid.Cell(detailStartRowNumber + i, 1).Text = createdate;//日期
                reportGrid.Cell(detailStartRowNumber + i, 2).Text = code;//
                reportGrid.Cell(detailStartRowNumber + i, 3).Text = supplierrelationname;//
                reportGrid.Cell(detailStartRowNumber + i, 4).Text = price.ToString();//

                reportGrid.Cell(detailStartRowNumber + i, 5).Text = CommonUtil.NumberToStringFormate(cgsl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 6).Text = CommonUtil.NumberToStringFormate(cgje,2);//                
                reportGrid.Cell(detailStartRowNumber + i, 7).Text = CommonUtil.NumberToStringFormate(dbrksl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 8).Text = CommonUtil.NumberToStringFormate(dbrkje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 9).Text = CommonUtil.NumberToStringFormate(rksl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 10).Text = CommonUtil.NumberToStringFormate(rkje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 11).Text = CommonUtil.NumberToStringFormate(jssl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 12).Text = CommonUtil.NumberToStringFormate(jsje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 13).Text = CommonUtil.NumberToStringFormate(llcksl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 14).Text = CommonUtil.NumberToStringFormate(llckje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 15).Text = CommonUtil.NumberToStringFormate(dbcksl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 16).Text = CommonUtil.NumberToStringFormate(dbckje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 17).Text = CommonUtil.NumberToStringFormate(cksl,4);//
                reportGrid.Cell(detailStartRowNumber + i, 18).Text = CommonUtil.NumberToStringFormate(ckje,2);//
                reportGrid.Cell(detailStartRowNumber + i, 19).Text = "";//
                reportGrid.Cell(detailStartRowNumber + i, 20).Text = CommonUtil.NumberToStringFormate(ykje,2);//盈亏金额
                if (jcsl == 0)
                {
                    reportGrid.Cell(detailStartRowNumber + i, 21).Text = "";
                }
                else
                {
                    reportGrid.Cell(detailStartRowNumber + i, 21).Text = CommonUtil.NumberToStringFormate(jcsl,4);//
                }

                if (jcje == 0)
                {
                    reportGrid.Cell(detailStartRowNumber + i, 22).Text = "";
                }
                else
                {
                    reportGrid.Cell(detailStartRowNumber + i, 22).Text = CommonUtil.NumberToStringFormate(jcje,2);//
                }
               
                
                //消耗明细
                if (djlx.Equals("llck", StringComparison.OrdinalIgnoreCase))
                {
                    string gwbsTreeSyscode = dr["gwbsTreeSyscode"] + "";
                    if (!string.IsNullOrEmpty(gwbsTreeSyscode))
                    {
                        WZBB_clsfctz_xhmx(pi.Id, detailStartRowNumber + i, startCols, llcksl, llckje, gwbsTreeSyscode,summaryRowIndex);
                    }
                }
            }
            //插入合计
            
            reportGrid.Cell(summaryRowIndex, 1).Text = "合计";//

            reportGrid.Cell(summaryRowIndex, 5).Text = CommonUtil.NumberToStringFormate(sumCgsl,4);//
            reportGrid.Cell(summaryRowIndex, 6).Text = CommonUtil.NumberToStringFormate(sumCgje,2);//
            
            reportGrid.Cell(summaryRowIndex, 7).Text = CommonUtil.NumberToStringFormate(sumDbrksl,4);//
            reportGrid.Cell(summaryRowIndex, 8).Text = CommonUtil.NumberToStringFormate(sumDbrkje,2);//
            reportGrid.Cell(summaryRowIndex, 9).Text = CommonUtil.NumberToStringFormate(sumRksl,4);//
            reportGrid.Cell(summaryRowIndex, 10).Text = CommonUtil.NumberToStringFormate(sumRkje,2);//

            reportGrid.Cell(summaryRowIndex, 11).Text = CommonUtil.NumberToStringFormate(sumJssl,4);//
            reportGrid.Cell(summaryRowIndex, 12).Text = CommonUtil.NumberToStringFormate(sumJsje, 2);//

            reportGrid.Cell(summaryRowIndex, 13).Text = CommonUtil.NumberToStringFormate(sumLlcksl,4);//
            reportGrid.Cell(summaryRowIndex, 14).Text = CommonUtil.NumberToStringFormate(sumLlckje,2);//
            reportGrid.Cell(summaryRowIndex, 15).Text = CommonUtil.NumberToStringFormate(sumDbcksl,4);//
            reportGrid.Cell(summaryRowIndex, 16).Text = CommonUtil.NumberToStringFormate(sumDbckje,2);//
            reportGrid.Cell(summaryRowIndex, 17).Text = CommonUtil.NumberToStringFormate(sumCksl,4);//
            reportGrid.Cell(summaryRowIndex, 18).Text = CommonUtil.NumberToStringFormate(sumCkje,2);//
  
            reportGrid.Cell(summaryRowIndex, 20).Text = CommonUtil.NumberToStringFormate(sumYkje,2);//

            FlexCell.Range range = reportGrid.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount, 3);
            range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);

            range = reportGrid.Range(detailStartRowNumber, 4, detailStartRowNumber + detailCount, reportGrid.Cols - 2);
            range.Alignment = FlexCell.AlignmentEnum.RightCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
        }

        /// <summary>
        /// 材料收发存台帐插入消耗明细列
        /// </summary>
        /// <param name="projectId"></param>
        private void WZBB_clsfctz_addColsOfUsedPart(string projectId)
        {
            IList usedPartLst = GetUsedPart(projectId);
            if (usedPartLst == null || usedPartLst.Count == 0) return;
            foreach (GWBSTree usedPart in usedPartLst)
            {
                int startCol = reportGrid.Cols - 1;
                InsertGridCol4Clsfchzb(startCol,5,usedPart.Name);//5为报表格式中的行号
            }
        }

        /// <summary>
        /// 材料收发存台帐 消耗明细
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="rowIndex">需要插入消耗明细的行</param>
        /// <param name="oldStartCols">插入消耗明细列前的列索引</param>
        /// <param name="quantity">数量</param>
        /// <param name="money">金额</param>
        /// <param name="gwbsTreeSyscode">领料出库明细使用部位的层次码</param>
        /// <param name="summaryRowIndex">汇总行所在的行索引</param>
        private void WZBB_clsfctz_xhmx(string projectId,int rowIndex,int oldStartCols,decimal quantity,decimal money,string gwbsTreeSyscode,int summaryRowIndex)
        {
            IList usedPartLst = GetUsedPart(projectId);
            if (usedPartLst == null || usedPartLst.Count == 0) return;
            foreach (GWBSTree usedPart in usedPartLst)
            {
                if(gwbsTreeSyscode.IndexOf(usedPart.Id)>=0)
                {
                    reportGrid.Cell(rowIndex, oldStartCols).Text = CommonUtil.NumberToStringFormate(quantity,4);
                    reportGrid.Cell(rowIndex, oldStartCols + 1).Text = CommonUtil.NumberToStringFormate(money,4);

                    //合计消耗数量、金额
                    decimal sumXhmxSl =ClientUtil.ToDecimal(reportGrid.Cell(summaryRowIndex, oldStartCols).Text);
                    decimal sumXhmxJe = ClientUtil.ToDecimal(reportGrid.Cell(summaryRowIndex, oldStartCols+1).Text);
                    reportGrid.Cell(summaryRowIndex, oldStartCols).Text = CommonUtil.NumberToStringFormate((sumXhmxSl + quantity),4);
                    reportGrid.Cell(summaryRowIndex, oldStartCols + 1).Text = CommonUtil.NumberToStringFormate((sumXhmxJe + money), 2);

                    break;
                }
                oldStartCols = oldStartCols + 2;
            }
        }

         
    }
  public   class    QtyMoney
    {
      decimal qty=0;
      decimal money=0;
      decimal ltqty = 0;
      decimal ltmoney = 0;
      public   decimal Qty 
      {
          get{return qty ;}
          set{qty =value ;}
      }
      public decimal Money
      {
          get { return money; }
          set { money = value; }
      }
      public decimal Ltqty
      {
          get { return ltqty; }
          set { ltqty = value; }
      }
      public decimal Ltmoney
      {
          get { return ltmoney; }
          set { ltmoney = value; }
      }
    }
 public  enum Headtype
  {
      SumAndCurrent=0,
      Current=1
  }
}
