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
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Business.Erp.SupplyChain.Client.StockMng.StockCheckMng.StockInventoryMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS; 

 
 namespace Application.Business.Erp.SupplyChain.Client.StockMng.Report
{
    public partial class VStockReport : TBasicDataView
    {
        private MStockMng modelStock = new MStockMng();
        private MStockInventoryMng modelInventory = new MStockInventoryMng();
        //private string formatMoney = "################.##";
        private string formatMoney = "################.##";
        private string formatQuantity = "################.####";
        bool IsCheckProfessionCat = true;
        bool IsCheckUsedRank = true;
        bool IsCheckUsedPart = true;
        #region 报表名称
        private string NR_StoreReporter="月度盘点报表";
        private string NR_CostReporter = "成本分析汇总表";
        private string NR_StoreInventeryReporter = "结算消耗明细表报表";
        private string NR_StoreBalanceReporter = "结算耗用明细报表(安装)";
        private string NR_MaterialAccountReporter = "物资设备收发台帐(安装)";
        #endregion

        public VStockReport()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        private void InitData()
        {
            cmbReportName.Items.Clear();
            cmbReportName.Items.Add(NR_StoreReporter);
            cmbReportName.Items.Add(NR_CostReporter);
            cmbReportName.Items.Add(NR_StoreInventeryReporter);
            //cmbReportName.Items.Add(NR_StoreBalanceReporter);
            cmbReportName.Items.Add(NR_MaterialAccountReporter);
            cmbReportName.SelectedIndex = 0;
            CurrentProjectInfo pi = StaticMethod.GetProjectInfo();
            IList projectLst = new ArrayList();//这里可以查询所有的项目
            projectLst.Add(pi);
            cmbProject.DataSource = projectLst;
            cmbProject.DisplayMember = "Name";
            cmbProject.ValueMember = "Id";
            cmbProject.SelectedItem = pi;
            VBasicDataOptr.InitProfessionCategory(cboProfessionCat, true);
            IList list = modelStock.StockInSrv.GetFiscalYear();
            this.cmbYear.Items.Clear();
            this.cmbMonth.Items.Clear();
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
                this.cmbMonth.Items.Insert(this.cmbMonth.Items.Count, i);
                if (i == ConstObject.TheLogin.TheComponentPeriod.NowMonth)
                {
                    this.cmbMonth.SelectedItem = this.cmbMonth.Items[this.cmbMonth.Items.Count - 1];
                }
            }

        }

        private void InitEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            cmbReportName.SelectedIndexChanged += new EventHandler(cmbReportName_SelectedIndexChanged);
              btnSelectUsedRank .Click +=new EventHandler(btnSelectUsedRank_Click);
              btnSelectUsePart.Click += new EventHandler(btnSelectUserPart_Click);
        }
           void btnSelectUserPart_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                TreeNode root = frm.SelectResult[0];
                GWBSTree task = root.Tag as GWBSTree;
                this.txtUserPart.Text = task.Name;
                this.txtUserPart.Tag = task  ;
            }
        }
        void btnSelectUsedRank_Click(object sender, EventArgs e)
        { 
            CommonSupplier Supplier = new CommonSupplier();
            Supplier.SupplierCatCode = CommonUtil.SupplierCatCode3 + "-";
            Supplier.OpenSelect();
            IList list = Supplier.Result;
            foreach (SupplierRelationInfo Suppliers in list)
            {
                this.txtUsedRank.Tag = Suppliers;
                this.txtUsedRank.Text = Suppliers.SupplierInfo.Name;
                this.btnSearch.Focus();
            }
        }

        void cmbReportName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string reportName = cmbReportName.SelectedItem + "";
            if (reportName == NR_StoreReporter)
            {
                LoadTempleteFile(@"物资报表\月度盘点单(安装).flx");
                cmbProject.Visible = true;
                txtUsedRank.Visible = true;
                this.lblDate.Visible = true;
                cmbMonth.Visible = true;
                cmbYear.Visible = true;
                lblCat.Visible = true;
                cboProfessionCat.Visible = true;
                customLabel1.Visible = true;
                txtUsedRank.Visible = true;
                btnSelectUsedRank.Visible = true;
                lbUserPart.Visible = false;
                txtUserPart.Visible = false;
                
                btnSelectUsePart.Visible = false;

                this.lblBegin.Visible = false;
                this.lblEnd.Visible = false;
                this.dtpDateBegin.Visible = false;
                this.dtpDateEnd.Visible = false;

                this.IsCheckProfessionCat = false;
                this.IsCheckUsedRank = false;
                IsCheckUsedPart = false;
            }
            else if (reportName == NR_CostReporter)
            {
                LoadTempleteFile(@"物资报表\物资成本分析报表.flx");
                 
                cboProfessionCat.Visible = false ;
                txtUsedRank.Visible = false ;
                cmbMonth.Visible = true;
                cmbYear.Visible = true;
                lblCat.Visible = false ;
                this.lblDate.Visible = true;
                cboProfessionCat.Visible = false;
                customLabel1.Visible = false;
                txtUsedRank.Visible = false;
                btnSelectUsedRank.Visible = false;
                lbUserPart.Visible = false ;
                txtUserPart.Visible = false;
                btnSelectUsePart.Visible = false ;

                this.lblBegin.Visible = false;
                this.lblEnd.Visible = false;
                this.dtpDateBegin.Visible = false;
                this.dtpDateEnd.Visible = false;

                this.lblBegin.Visible = false;
                this.lblEnd.Visible = false;
                this.dtpDateBegin.Visible = false;
                this.dtpDateEnd.Visible = false;

                this.IsCheckProfessionCat = false;
                this.IsCheckUsedRank = false;
                IsCheckUsedPart = false;
            }
            else if (NR_StoreInventeryReporter == reportName)
            {
                LoadTempleteFile(@"物资报表\盘点结算消耗明细报表.flx");
              
                cmbProject.Visible = true;
                this.lblDate.Visible = true;
                txtUsedRank.Visible = true;
                cmbMonth.Visible = true;
                cmbYear.Visible = true;
                lblCat.Visible = true  ;
                cboProfessionCat.Visible = true  ;
                customLabel1.Visible = true  ;
                txtUsedRank.Visible = true;
                btnSelectUsedRank.Visible = true;
                lbUserPart.Visible = true;
                txtUserPart.Visible = true ;
                btnSelectUsePart.Visible = true;

                this.lblBegin.Visible = false;
                this.lblEnd.Visible = false;
                this.dtpDateBegin.Visible = false;
                this.dtpDateEnd.Visible = false;

                this.IsCheckProfessionCat = false;
                this.IsCheckUsedRank = false;
                IsCheckUsedPart = false;
            }
            else if (NR_StoreBalanceReporter == reportName)
            {
                LoadTempleteFile(@"物资报表\结算耗用明细报表(安装).flx");
                cmbProject.Visible = true;
                this.lblDate.Visible = true;
                txtUsedRank.Visible = true;
                cmbMonth.Visible = true;
                cmbYear.Visible = true;
                lblCat.Visible = false ;
                cboProfessionCat.Visible = false ;
                customLabel1.Visible = true  ;
                txtUsedRank.Visible = true;
                btnSelectUsedRank.Visible = true;
                lbUserPart.Visible = true;
                txtUserPart.Visible = true;
                btnSelectUsePart.Visible = true;

                this.lblBegin.Visible = false;
                this.lblEnd.Visible = false;
                this.dtpDateBegin.Visible = false;
                this.dtpDateEnd.Visible = false;

                this.IsCheckProfessionCat = false;
                this.IsCheckUsedRank = false;
                IsCheckUsedPart = false;
            }
            else if (this.NR_MaterialAccountReporter  == reportName)
            {
                LoadTempleteFile(@"物资报表\物资设备收发台帐(安装).flx");
                cmbProject.Visible = true;

                txtUsedRank.Visible = false ;
                cmbMonth.Visible = false;
                cmbYear.Visible = false;
                this.lblDate.Visible = false;
                lblCat.Visible = false;
                cboProfessionCat.Visible = false;
                customLabel1.Visible = false;
                txtUsedRank.Visible = false;
                btnSelectUsedRank.Visible = false;
                lbUserPart.Visible = false;
                txtUserPart.Visible = false;
                btnSelectUsePart.Visible = false;

                this.lblBegin.Visible = true ;
                this.lblEnd.Visible = true ;
                this.dtpDateBegin.Visible = true;
                this.dtpDateEnd.Visible = true;

                this.IsCheckProfessionCat = false;
                this.IsCheckUsedRank = false;
                IsCheckUsedPart = false;
                 
            }
        }
        
        void btnSearch_Click(object sender, EventArgs e)
        {
             
            string reportName = cmbReportName.SelectedItem + "";
            if (this.cmbProject.Visible ==true && this.cmbProject.SelectedItem == null)
            {
                MessageBox.Show("请选择项目名称！");
                return;
            }
            if (IsCheckProfessionCat && this.cboProfessionCat.Visible == true && this.cboProfessionCat.SelectedItem == null)
            {
                MessageBox.Show("请选择专业分类！");
                return;
            }
            if (IsCheckUsedRank && this.txtUsedRank.Visible == true && this.txtUsedRank.Tag == null)
            {
                MessageBox.Show("请选择劳务队伍！");
                return;
            }

            if (IsCheckUsedPart && this.txtUserPart.Visible && this.txtUserPart.Tag == null)
            {
                MessageBox.Show("请选择使用部位！");
                return;
            }
            if (reportName == this.NR_StoreReporter)
            {
                LoadTempleteFile(@"物资报表\月度盘点单(安装).flx");
                FillReport_StoreReporter();
            }
            else if (reportName == this.NR_CostReporter )
            {
                LoadTempleteFile(@"物资报表\物资成本分析报表.flx");
                FillReport_MaterialCostReporter();
            }
            else if (NR_StoreInventeryReporter == reportName)
            {
                LoadTempleteFile(@"物资报表\盘点结算消耗明细报表.flx");
              
                FillReport_StoreInventoryReporter();
            }
            else if (NR_StoreBalanceReporter == reportName)
            {
                LoadTempleteFile(@"物资报表\结算耗用明细报表(安装).flx");
                FillReport_StoreBalanceReporter();
            }
            else if (NR_MaterialAccountReporter == reportName)
            {
                LoadTempleteFile(@"物资报表\物资设备收发台帐(安装).flx");
                FillReport_MaterialAccountReporter();
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
        /// <summary>
        /// 物资成本分析报表
        /// </summary>
        private void FillReport_MaterialCostReporter()
        {//
 
            int iYear;
            int iMonth;
            CurrentProjectInfo oProject = cmbProject.SelectedItem as CurrentProjectInfo;
            reportGrid .AutoRedraw = false;
            //reportGrid.FixedCols = 2;
            //reportGrid.FixedRows  =3;
           string  sProjectId = oProject.Id;
           IList lstData = null;
           int iStartRow = 4;
           //DataTable oTableMoney1 = null;
           DataTable oTableMoney = null;
           DataTable oTableCount = null;
            iYear = int.Parse(this.cmbYear.SelectedItem.ToString());
            iMonth = int.Parse(this.cmbMonth.SelectedItem.ToString());
            reportGrid.Cell(1, 1).Text = string.Format("安装分公司{0}项目{1}年度{2}月物资成本分析汇总表", oProject.Name, iYear.ToString(), iMonth.ToString());
          
            #region 填充数据
           
            FlexCell.Cell oCell = null;
               lstData = modelStock.StockInSrv.QueryStockCost(sProjectId, iYear, iMonth);
               if (lstData == null || lstData.Count != 2)
            {
                return;
            }
            oTableMoney = lstData[0] as DataTable;
            oTableCount = lstData[1] as DataTable;
            if (oTableCount != null && oTableCount.Rows.Count > 0 && oTableMoney != null && oTableMoney.Rows.Count > 0)
            {
                //reportGrid.InsertRow(iStartRow, oTableMoney.Rows.Count);
                DataRow oRow = null;
                bool isAdd = true;
                int iCount = 0;
                int iCurrRow = 0;
                int Index = 0;
              decimal[,] arr=new decimal [2,16];
              
                for (int iRow = 0; iRow < oTableMoney.Rows.Count; )
                {
                    
                    iCurrRow = iStartRow + iCount;
                    reportGrid.InsertRow(iCurrRow, 1);
                    oRow = oTableMoney.Rows[iRow];
                    oCell = reportGrid.Cell(iCurrRow, 1);//物资名称
                    oCell.Text = ClientUtil.ToString(oRow["materialName"]);
                    if (string.IsNullOrEmpty(oCell.Text))
                    {
                        oCell.Text = "其他";
                    }
                    oCell.WrapText = true;
                    oCell = reportGrid.Cell(iCurrRow, 2);//本月
                    if (ClientUtil.ToDecimal(oRow["type"]) == 1 && iCount % 2 == 0)
                    {
                       
                        oCell.Text =  "本月" ;
                        oRow = oTableMoney.NewRow();
                        isAdd = false;
                    }
                    else
                    {
                        oCell.Text = (ClientUtil.ToDecimal(oRow["type"]) == 0 ? "本月" : "累计");
                        isAdd = true ;
                    }
                    oCell.WrapText = true;
                    iCount++;
                    //oCell = reportGrid.Cell(iRow + iStartRow, 2);//本月
                    //oCell.Text = (iRow % 2 == 0 ? "本月" : "累计");
                    //oCell.WrapText = true;
                    //验收认价合价
                    oCell = reportGrid.Cell(iCurrRow, 3);
                    oCell.Text = ClientUtil.ToDecimal(oRow["chkConfirmMoney"]).ToString();
                    oCell.WrapText = true;
                    //验收采购合价
                    oCell = reportGrid.Cell(iCurrRow, 4);
                    oCell.Text = ClientUtil.ToDecimal(oRow["chkOrderMoney"]).ToString();
                    oCell.WrapText = true;
                    //调入认价合价
                    oCell = reportGrid.Cell(iCurrRow, 5);
                    oCell.Text = ClientUtil.ToDecimal(oRow["mvInConfirmMoney"]).ToString();
                    oCell.WrapText = true;
                    //调入采购合价
                    oCell = reportGrid.Cell(iCurrRow, 6);
                    oCell.Text = ClientUtil.ToDecimal(oRow["mvInOrderMoney"]).ToString();
                    oCell.WrapText = true;
                    //调出 认价合价
                    oCell = reportGrid.Cell(iCurrRow, 7);
                    oCell.Text = ClientUtil.ToDecimal(oRow["mvOutConfirmMoney"]).ToString();
                    oCell.WrapText = true;
                    //调出 采购合价
                    oCell = reportGrid.Cell(iCurrRow, 8);
                    oCell.Text = ClientUtil.ToDecimal(oRow["mvOutOrderMoney"]).ToString();
                    oCell.WrapText = true;

                    //施工领料	 认价合价
                    oCell = reportGrid.Cell(iCurrRow, 9);
                    oCell.Text = ClientUtil.ToDecimal(oRow["stkOutConfirmMoney"]).ToString();
                    oCell.WrapText = true;
                    //施工领料	 采购合价
                    oCell = reportGrid.Cell(iCurrRow, 10);
                    oCell.Text = ClientUtil.ToDecimal(oRow["stkOutOrderMoney"]).ToString();
                    oCell.WrapText = true;


                    //劳务结算消耗	 认价合价
                    oCell = reportGrid.Cell(iCurrRow, 11);
                    oCell.Text = ClientUtil.ToDecimal(oRow["balConfirmMoney"]).ToString();
                    oCell.WrapText = true;
                    //劳务结算消耗		 采购合价
                    oCell = reportGrid.Cell(iCurrRow, 12);
                    oCell.Text = ClientUtil.ToDecimal(oRow["balOrderMoney"]).ToString();
                    oCell.WrapText = true;
                    reportGrid.Row(iCurrRow).AutoFit();

                    //库存盘点	认价合价
                    oCell = reportGrid.Cell(iCurrRow, 13);
                    oCell.Text = ClientUtil.ToDecimal(oRow["invConfirmMoney"]).ToString();
                    oCell.WrapText = true;
                    //库存盘点	采购合价
                    oCell = reportGrid.Cell(iCurrRow, 14);
                    oCell.Text = ClientUtil.ToDecimal(oRow["invOrderMoney"]).ToString();
                    oCell.WrapText = true;
                    reportGrid.Row(iCurrRow).AutoFit();

                    //验收+调入-施工领料		认价差
                    oCell = reportGrid.Cell(iCurrRow, 15);
                    oCell.Text = (ClientUtil.ToDecimal(oRow["chkConfirmMoney"]) + ClientUtil.ToDecimal(oRow["mvInConfirmMoney"]) - ClientUtil.ToDecimal(oRow["stkOutConfirmMoney"])).ToString();
                    oCell.WrapText = true;
                    //验收+调入-施工领料		采购价差
                    oCell = reportGrid.Cell(iCurrRow, 16);
                    oCell.Text = (ClientUtil.ToDecimal(oRow["chkOrderMoney"]) + ClientUtil.ToDecimal(oRow["mvInOrderMoney"]) - ClientUtil.ToDecimal(oRow["stkOutOrderMoney"])).ToString();
                    oCell.WrapText = true;
                    reportGrid.Row(iCurrRow).AutoFit();

                    //施工领料-结算消耗-库存盘点			认价差
                    oCell = reportGrid.Cell(iCurrRow, 17);
                    oCell.Text = ClientUtil.ToDecimal(oRow["stkOutConfirmMoney"])==0?"0":((ClientUtil.ToDecimal(oRow["stkOutConfirmMoney"]) - ClientUtil.ToDecimal(oRow["balConfirmMoney"]) - ClientUtil.ToDecimal(oRow["invConfirmMoney"])).ToString());
                    oCell.WrapText = true;
                    //施工领料-结算消耗-库存盘点			采购价差
                    oCell = reportGrid.Cell(iCurrRow, 18);
                    oCell.Text = ClientUtil.ToDecimal(oRow["stkOutOrderMoney"])==0?"0":(ClientUtil.ToDecimal(oRow["stkOutOrderMoney"]) - ClientUtil.ToDecimal(oRow["balOrderMoney"]) - ClientUtil.ToDecimal(oRow["invConfirmMoney"])).ToString();
                    oCell.WrapText = true;
                    reportGrid.Row(iCurrRow).AutoFit();

                     oCell = reportGrid.Cell(iCurrRow, 2);
                    Index = (string.Equals(oCell.Text, "本月") ? 0:1 );
                  
                         //验收认价合价      
                    arr[Index, 0] += ClientUtil.ToDecimal(oRow["chkConfirmMoney"]);
                         //验收采购合价
                    arr[Index, 1] += ClientUtil.ToDecimal(oRow["chkOrderMoney"]);
                         //调入认价合价
                    arr[Index, 2] += ClientUtil.ToDecimal(oRow["mvInConfirmMoney"]);
                         //调入采购合价
                    arr[Index, 3] += ClientUtil.ToDecimal(oRow["mvInOrderMoney"]);
                         //调出 认价合价
                    arr[Index, 4] += ClientUtil.ToDecimal(oRow["mvOutConfirmMoney"]);
                         //调出 采购合价
                    arr[Index, 5] += ClientUtil.ToDecimal(oRow["mvOutOrderMoney"]);
                         //施工领料	 认价合价
                    arr[Index, 6] += ClientUtil.ToDecimal(oRow["stkOutConfirmMoney"]);
                         //施工领料	 采购合价
                    arr[Index, 7] += ClientUtil.ToDecimal(oRow["stkOutOrderMoney"]);
                         //劳务结算消耗	 认价合价
                    arr[Index, 8] += ClientUtil.ToDecimal(oRow["balConfirmMoney"]);
                         //劳务结算消耗		 采购合价
                    arr[Index, 9] += ClientUtil.ToDecimal(oRow["balOrderMoney"]);
                         //库存盘点	认价合价
                    arr[Index, 10] += ClientUtil.ToDecimal(oRow["invConfirmMoney"]);
                         //库存盘点	采购合价
                    arr[Index, 11] += ClientUtil.ToDecimal(oRow["invOrderMoney"]);
                         //验收+调入-施工领料		认价差
                    arr[Index, 12] += (ClientUtil.ToDecimal(oRow["chkConfirmMoney"]) + ClientUtil.ToDecimal(oRow["mvInConfirmMoney"]) - ClientUtil.ToDecimal(oRow["stkOutConfirmMoney"]));
                         //验收+调入-施工领料		采购价差
                    arr[Index, 13] += (ClientUtil.ToDecimal(oRow["chkOrderMoney"]) + ClientUtil.ToDecimal(oRow["mvInOrderMoney"]) - ClientUtil.ToDecimal(oRow["stkOutOrderMoney"]));
                         //施工领料-结算消耗-库存盘点			认价差
                    arr[Index, 14] += ( ClientUtil.ToDecimal(oRow["stkOutConfirmMoney"])==0?0:   ClientUtil.ToDecimal(oRow["stkOutConfirmMoney"]) - ClientUtil.ToDecimal(oRow["balConfirmMoney"]) - ClientUtil.ToDecimal(oRow["invConfirmMoney"]));
                         //施工领料-结算消耗-库存盘点			采购价差
                    arr[Index, 15] += (ClientUtil.ToDecimal(oRow["stkOutOrderMoney"])==0?0:ClientUtil.ToDecimal(oRow["stkOutOrderMoney"]) - ClientUtil.ToDecimal(oRow["balOrderMoney"]) - ClientUtil.ToDecimal(oRow["invConfirmMoney"]));
                    if (isAdd)
                    {
                        iRow++;
                    }
                    oCell = reportGrid.Cell(iCurrRow, 2);
                    if(string.Equals (oCell.Text,"累计"))
                    {
                        FlexCell.Range oRange = reportGrid.Range(iCurrRow - 1, 1, iCurrRow, 1);
                        oRange.Merge();
                    }
                }
                for (int k = 1; k < 3; k++)
                {
                    reportGrid.InsertRow(iCurrRow+k, 1);
                  
                    if (k == 1)
                    {
                        oCell = reportGrid.Cell(iCurrRow + k, 1);
                        oCell.Text = "合计";
                        oCell = reportGrid.Cell(iCurrRow + k, 2);
                        oCell.Text = "本月";
                       

                    }
                    else
                    {
                        oCell = reportGrid.Cell(iCurrRow + k, 2);
                        oCell.Text = "累计";
                        FlexCell.Range oRange = reportGrid.Range(iCurrRow + k - 1, 1, iCurrRow + k, 1);
                        oRange.Merge();
                    }
                    for (int kk = 0; kk < 16; kk++)
                    {
                        oCell = reportGrid.Cell(iCurrRow + k, kk+3);
                        oCell.Text = arr[k-1,kk].ToString();
                        oCell.WrapText = true;
                       
                    }
                    reportGrid.Row(iCurrRow + k).AutoFit();

                }
                    if (oTableCount.Rows.Count == 1)
                    {
                        oRow = oTableCount.Rows[0];
                        //                    sum(chkCount)chkCount,sum(chkRedCount)chkRedCount,sum(stkOutCount)stkOutCount,sum(stkOutRedCount)stkOutRedCount,
                        //sum(stkInCount)stkInCount,sum(stkInRedCount)stkInRedCount,sum(mvInCount)mvInCount,sum(mvInRedCount)mvInRedCount,
                        //sum(mvOutCount)mvOutCount,sum(mvOutRedCount)mvOutRedCount,sum(sumTotal)sumTotal
                        string sSummaryInfo = string.Format("1.《验收单》 {0} 份    2.《验收单红单》 {1} 份    3.《领料单》 {2} 份    4.《领料红单》 {3} 份    5.《收料单》 {4} 份    6.《收料红单》 {5} 份   7.《调拨入库单》 {6} 份   8.《调拨入库单》 {7} 份   9.《调拨入库红单》 {8} 份  10.《调拨出库单》 {9} 份    11.《调拨出库红单》 {10} 份   共计 {11} 份", ClientUtil.ToDecimal(oRow["chkCount"]), ClientUtil.ToDecimal(oRow["chkRedCount"]), ClientUtil.ToDecimal(oRow["stkOutCount"]), ClientUtil.ToDecimal(oRow["stkOutRedCount"]), ClientUtil.ToDecimal(oRow["stkInCount"]), ClientUtil.ToDecimal(oRow["stkInRedCount"]), ClientUtil.ToDecimal(oRow["mvInCount"]), ClientUtil.ToDecimal(oRow["mvInCount"]), ClientUtil.ToDecimal(oRow["mvInRedCount"]), ClientUtil.ToDecimal(oRow["mvOutCount"]), ClientUtil.ToDecimal(oRow["mvOutRedCount"]), ClientUtil.ToDecimal(oRow["sumTotal"]));
                        oCell = reportGrid.Cell(iStartRow + iCount + 3, 3);
                        oCell.Text = sSummaryInfo;
                    }
                reportGrid.AutoRedraw = true ;
                reportGrid.Refresh();
            #endregion
            }
        }

        /// <summary>
        /// 月度盘点报表
        /// </summary>
        private void FillReport_StoreReporter()
        {//
            string sProfessionCategory = string.Empty;
            string sProjectId = string.Empty;
            string sUsedRankId = string.Empty;
            int iYear;
            int iMonth;
            CurrentProjectInfo oProject = cmbProject.SelectedItem as CurrentProjectInfo;
            sProjectId = oProject.Id;
         
            SupplierRelationInfo Supplier = this.txtUsedRank.Tag as SupplierRelationInfo;
            if (!string.IsNullOrEmpty (txtUsedRank.Text.Trim () )&& Supplier != null)
            {
                sUsedRankId = Supplier.Id;
            }
            if (this.cboProfessionCat.SelectedItem != null)
            {
                sProfessionCategory = this.cboProfessionCat.SelectedItem.ToString();
            }
            iYear = int.Parse(this.cmbYear.SelectedItem.ToString());
            iMonth = int.Parse(this.cmbMonth.SelectedItem.ToString());

            reportGrid.Cell(1, 1).Text = string.Format("(安装) {0}项目 {1}年{2}月 {3}专业物资盘点表", oProject.Name, iYear.ToString(), iMonth.ToString(), string.IsNullOrEmpty(sProfessionCategory) ? "全部" : sProfessionCategory);
            if (Supplier != null)
            {
                reportGrid.Cell(2, 2).Text = Supplier.SupplierInfo.Name;//劳务队伍
            }
            if (this.cboProfessionCat.SelectedItem != null)
            {
                reportGrid.Cell(2, 7).Text = sProfessionCategory;
            }

            #region 填充数据
            int iStartRow = 4;
            // t.materialname,t.MATERIALSPEC, t.MATSTANDARDUNITNAME ,t.INVENTORYQUANTITY,t.confirmprice,t.confirmmoney,t.price,t.money,t.descript
            string sMaterialName = string.Empty;
            string sMaterialSpec = string.Empty;
            string sMatStandardUnitName = string.Empty;
            string sDiagramnumber = string.Empty;
            int iNum = 0;
            decimal dInventoryQuantity = 0;
            decimal dConfirmPrice = 0;
            decimal dConfirmMoney = 0;
            decimal dPrice = 0;
            decimal dMoney = 0;
            decimal dSumMoney = 0;
            decimal dSumConfirnMoney = 0;
            DataRow oRow = null;
            DateTime oCreateDate;
            string sDescript = string.Empty;
            FlexCell.Cell oCell = null;
            DataTable oTable = modelInventory.StockInventorySrv.GetInventoryReport(sProfessionCategory, sProjectId, sUsedRankId, iYear, iMonth);
            if (oTable == null || oTable.Rows.Count == 0)
            {
                return;
            }
            oCreateDate = ClientUtil.ToDateTime(oTable.Rows[0]["createdate"]);
            reportGrid.Cell(5, 9).Text = string.Format("{0}年{1}月{2}日", oCreateDate.Year, oCreateDate.Month, oCreateDate.Day);
            reportGrid.Cell(5, 9).WrapText = true;
            reportGrid.InsertRow(iStartRow, oTable.Rows.Count);
            FlexCell.Range range = reportGrid.Range(iStartRow, 5, iStartRow + oTable.Rows.Count, 10);//序号格式
            range.Alignment = FlexCell.AlignmentEnum.RightCenter;
            for (int iRow = 0; iRow < oTable.Rows.Count; iRow++)
            {
                oRow = oTable.Rows[iRow];
                iNum = iRow + 1;
                sMaterialName = ClientUtil.ToString(oRow["materialname"]);
                sMaterialSpec = ClientUtil.ToString(oRow["MATERIALSPEC"]);
                sMatStandardUnitName = ClientUtil.ToString(oRow["MATSTANDARDUNITNAME"]);
                dInventoryQuantity = ClientUtil.ToDecimal(oRow["INVENTORYQUANTITY"]);
                dConfirmPrice = ClientUtil.ToDecimal(oRow["confirmprice"]);
                dConfirmMoney = ClientUtil.ToDecimal(oRow["confirmmoney"]);
                dSumConfirnMoney += dConfirmMoney;
                dPrice = ClientUtil.ToDecimal(oRow["price"]);
                dMoney = ClientUtil.ToDecimal(oRow["money"]);
                dSumMoney += dMoney;
                sDescript = ClientUtil.ToString(oRow["descript"]);
                sDiagramnumber = ClientUtil.ToString(oRow["diagramnumber"]);
                oCell = reportGrid.Cell(iRow + iStartRow, 1);
                oCell.Text = iNum.ToString();//序号
                oCell.WrapText = true;

                oCell = reportGrid.Cell(iRow + iStartRow, 2);
                oCell.Text = sMaterialName;//物资名称
                oCell.WrapText = true;
                oCell = reportGrid.Cell(iRow + iStartRow, 3);
                oCell.Text = sMaterialSpec;//规格
                oCell.WrapText = true;

                oCell = reportGrid.Cell(iRow + iStartRow, 4);
                oCell.Text = sMatStandardUnitName;//单位
                oCell.WrapText = true;

                oCell = reportGrid.Cell(iRow + iStartRow, 5);
                oCell.Text = sDiagramnumber;//图号
                oCell.WrapText = true;

                oCell = reportGrid.Cell(iRow + iStartRow, 6);
                oCell.Text = dInventoryQuantity.ToString(); //
                oCell.WrapText = true;

                oCell = reportGrid.Cell(iRow + iStartRow, 9);
                oCell.Text = dConfirmPrice.ToString();//认价单价
                oCell.WrapText = true;

                oCell = reportGrid.Cell(iRow + iStartRow, 10);
                oCell.Text = dConfirmMoney.ToString();//认价金额
                oCell.WrapText = true;

                oCell = reportGrid.Cell(iRow + iStartRow, 7);
                oCell.Text = dPrice.ToString();//采购单价
                oCell.WrapText = true;

                oCell = reportGrid.Cell(iRow + iStartRow, 8);
                oCell.Text = dMoney.ToString();//采购金额
                oCell.WrapText = true;

                oCell = reportGrid.Cell(iRow + iStartRow, 11);
                oCell.Text = sDescript;//备注
                oCell.WrapText = true;
                reportGrid.Row(iRow + iStartRow).AutoFit();
            }
            oCell = reportGrid.Cell(oTable.Rows.Count + iStartRow, 10);
            oCell.Text = dSumConfirnMoney.ToString();//认价总额
       
            oCell.WrapText = true;

            oCell = reportGrid.Cell(oTable.Rows.Count + iStartRow, 8);
            oCell.Text = dSumMoney.ToString();//采购总额
            oCell.WrapText = true;
            #endregion
        }
       
        private void FillReport_StoreInventoryReporter()
        {
           
            string sProjectID = string.Empty;
            string sUserPartID = string.Empty;
            string sUserRandID = string.Empty;
            string sProfessionCat=string.Empty ;
            string sAccountTaskSysCode = string.Empty;
            
            int iStart = 5;
            DataRow oRow=null;
            FlexCell.Cell oCell = null;
            
            int iYear = 0;
            int iMonth = 0;
            decimal dInvSumQuality=0;
            decimal dInvSumMoney = 0;
            decimal dInvSumConfirm = 0;
            decimal dJsSumQuality = 0;
            decimal dJsSumMoney = 0;
            decimal dJsSumConfirmMoney = 0;
            
              CurrentProjectInfo oProject = cmbProject.SelectedItem as CurrentProjectInfo;
            sProjectID =oProject .Id ;
            if( !string.IsNullOrEmpty (txtUserPart.Text )&& txtUserPart.Tag!=null)
            {
                GWBSTree oGWBSTree = txtUserPart.Tag as GWBSTree;
                sUserPartID = oGWBSTree.Id  as string;
                sAccountTaskSysCode = oGWBSTree.SysCode;
            }
            if (!string.IsNullOrEmpty(txtUsedRank.Text) && this.txtUsedRank.Tag != null)
            {
                SupplierRelationInfo Supplier = this.txtUsedRank.Tag as SupplierRelationInfo;
                sUserRandID = Supplier.Id;
            }
             
            sProfessionCat =cboProfessionCat.Text ;
            iYear = int.Parse(cmbYear.SelectedItem.ToString());
            iMonth = int.Parse(cmbMonth.SelectedItem.ToString());
           
            
            oCell = reportGrid.Cell(1, 1);
            oCell.Text = string.Format("(安装){0}项目{1}年{2}月{3}专业结算消耗明细表", oProject.Name, iYear, iMonth, string.IsNullOrEmpty(sProfessionCat) ? "全部" : sProfessionCat);
            oCell = reportGrid.Cell(2, 2);
            oCell.Text = txtUsedRank.Text;
            oCell = reportGrid.Cell(2, 6);
            oCell.Text = txtUserPart.Text;
             DataTable oTable = modelStock.StockInSrv.QueryStoreInventory(sProjectID, sUserPartID,sAccountTaskSysCode, sUserRandID, sProfessionCat,iYear, iMonth);
             if (oTable != null && oTable.Rows.Count > 0)
             {
                 reportGrid.InsertRow(iStart, oTable.Rows.Count);
                 for (int i = 0; i < oTable.Rows.Count; i++)
                 {
                     //select * from resperson t where t.pername='郑翔' and t.states=1
                     oRow=oTable.Rows[i];
                     //序号
                     oCell = reportGrid.Cell(i + iStart, 1);
                     oCell.Text = (i + 1).ToString();
                     oCell.WrapText = true;

                     //名称
                     oCell = reportGrid.Cell(i + iStart, 2);
                     oCell.Text =ClientUtil .ToString( oRow["materialname"]);
                     oCell.WrapText = true;
                     //规格号
                     oCell = reportGrid.Cell(i + iStart, 3);
                     oCell.Text = ClientUtil .ToString( oRow["materialspec"]);
                     oCell.WrapText = true;
                     //单位
                     oCell = reportGrid.Cell(i + iStart, 4);
                     oCell.Text = ClientUtil .ToString( oRow["matstandardunitname"]);
                     oCell.WrapText = true;
                     //diagramnumber  图号
                     oCell = reportGrid.Cell(i + iStart, 5);
                     oCell.Text = ClientUtil.ToString(oRow["diagramnumber"]);
                     oCell.WrapText = true;
                     //认价单价totalConfirmPrice
                     oCell = reportGrid.Cell(i + iStart, 6);
                     oCell.Text = ClientUtil.ToString(oRow["totalConfirmPrice"]);
                     oCell.WrapText = true;
                     //采购单价 totalPrice
                     oCell = reportGrid.Cell(i + iStart, 7);
                     oCell.Text = ClientUtil.ToString(oRow["totalPrice"]);
                     oCell.WrapText = true;
                     //盘点数量 quantity
                     oCell = reportGrid.Cell(i + iStart, 8);
                     oCell.Text = ClientUtil.ToString(oRow["quantity"]);
                     dInvSumQuality += ClientUtil.ToDecimal(oRow["quantity"]);
                     oCell.WrapText = true;
                     //盘点认价合价confirmmoney
                     oCell = reportGrid.Cell(i + iStart, 9);
                     oCell.Text = ClientUtil.ToString(oRow["confirmmoney"]);
                     dInvSumConfirm += ClientUtil.ToDecimal(oRow["confirmmoney"]);
                     oCell.WrapText = true;
                     //盘点合同合价 money
                     oCell = reportGrid.Cell(i + iStart, 10);
                     oCell.Text = ClientUtil.ToString(oRow["money"]);
                     dInvSumMoney += ClientUtil.ToDecimal(oRow["money"]);
                     oCell.WrapText = true;
                     //劳务数量
                     oCell = reportGrid.Cell(i + iStart, 11);
                     oCell.Text = ClientUtil.ToString(oRow["ACCUSAGEQNY"]);
                     dJsSumQuality += ClientUtil.ToDecimal(oRow["ACCUSAGEQNY"]);
                     oCell.WrapText = true;
                     //劳务认价单合价
                     oCell = reportGrid.Cell(i + iStart, 12);
                     oCell.Text = ClientUtil.ToString(oRow["jsConfirmMoney"]);
                     dJsSumConfirmMoney += ClientUtil.ToDecimal(oRow["jsConfirmMoney"]);
                     oCell.WrapText = true;
                     //劳务合同合价
                     oCell = reportGrid.Cell(i + iStart, 13);
                     dJsSumMoney += ClientUtil.ToDecimal(oRow["jsOrderMoney"]);
                     oCell.Text = ClientUtil.ToString(oRow["jsOrderMoney"]);
                     oCell.WrapText = true;
                     
                     reportGrid.Row(i + iStart).AutoFit();
                 }
                 oCell = reportGrid.Cell(oTable.Rows.Count + iStart, 8);
                 oCell.Text = ClientUtil.ToString(dInvSumQuality);
                 oCell = reportGrid.Cell(oTable.Rows.Count + iStart, 9);
                 oCell.Text = ClientUtil.ToString(dInvSumConfirm);
                 oCell = reportGrid.Cell(oTable.Rows.Count + iStart, 10);
                 oCell.Text = ClientUtil.ToString(dInvSumMoney);

                 oCell = reportGrid.Cell(oTable.Rows.Count + iStart, 11);
                 oCell.Text = ClientUtil.ToString(dJsSumQuality);
                 oCell = reportGrid.Cell(oTable.Rows.Count + iStart, 12);
                 oCell.Text = ClientUtil.ToString(dJsSumConfirmMoney);
                 oCell = reportGrid.Cell(oTable.Rows.Count + iStart, 13);
                 oCell.Text = ClientUtil.ToString(dJsSumMoney);
                 ///隐藏累计
               
             }
        }
        private void FillReport_StoreBalanceReporter()
        {
            string sProjectID = string.Empty;
            string sUserPartID = string.Empty;
            string sUserRandID = string.Empty;
            //string sProfessionCat = string.Empty;
            int iStart = 4;
            DataRow oRow = null;
            FlexCell.Cell oCell = null;

            int iYear = 0;
            int iMonth = 0;
            decimal dSumMoney = 0;
            decimal dSumConfirmMoney = 0;
            CurrentProjectInfo oProject = cmbProject.SelectedItem as CurrentProjectInfo;
            sProjectID = oProject.Id;
            if (txtUserPart.Tag != null)
            {
                GWBSTree oGWBSTree = txtUserPart.Tag as GWBSTree;
                sUserPartID = oGWBSTree.Id ;
            }
            SupplierRelationInfo Supplier = this.txtUsedRank.Tag as SupplierRelationInfo;
            sUserRandID = Supplier.Id;
            //sProfessionCat = cboProfessionCat.Text;
            iYear = int.Parse(cmbYear.SelectedItem.ToString());
            iMonth = int.Parse(cmbMonth.SelectedItem.ToString());


            oCell = reportGrid.Cell(1, 1);
            oCell.Text = string.Format("(安装){0}项目{1}年{2}月结算消耗明细表", oProject.Name, iYear, iMonth);
            oCell = reportGrid.Cell(2, 2);
            oCell.Text = txtUsedRank.Text;
            oCell = reportGrid.Cell(2, 7);
            oCell.Text = txtUserPart.Text;
            DataTable oTable = modelStock.StockInSrv.QueryStoreBalance(sProjectID, sUserPartID, sUserRandID,  iYear, iMonth);
            if (oTable != null && oTable.Rows.Count > 0)
            {
                reportGrid.InsertRow(iStart, oTable.Rows.Count);
                for (int i = 0; i < oTable.Rows.Count; i++)
                {
                    //   resourcetypeguid,RESOURCETYPENAME,RESOURCETYPESPEC,QUANTITYUNITNAME,ACCUSAGEQNY,ACCWORKQNYPrice, ACCWORKQNYMoney ,
                 //ttt2.price,ttt2.price*ACCUSAGEQNY money
                    oRow = oTable.Rows[i];
                    //序号
                    oCell = reportGrid.Cell(i + iStart, 1);
                    oCell.Text = (i + 1).ToString();
                    oCell.WrapText = true;

                    //名称
                    oCell = reportGrid.Cell(i + iStart, 2);
                    oCell.Text = ClientUtil.ToString(oRow["RESOURCETYPENAME"]);
                    oCell.WrapText = true;
                    //规格号
                    oCell = reportGrid.Cell(i + iStart, 3);
                    oCell.Text = ClientUtil.ToString(oRow["RESOURCETYPESPEC"]);
                    oCell.WrapText = true;
                    //单位
                    oCell = reportGrid.Cell(i + iStart, 4);
                    oCell.Text = ClientUtil.ToString(oRow["QUANTITYUNITNAME"]);
                    oCell.WrapText = true;
                    //结算消耗数量
                    oCell = reportGrid.Cell(i + iStart, 5);
                    oCell.Text = ClientUtil.ToString(oRow["ACCUSAGEQNY"]);
                    oCell.WrapText = true;
                    //认价单价
                    oCell = reportGrid.Cell(i + iStart, 6);
                    oCell.Text = ClientUtil.ToString(oRow["ACCWORKQNYPrice"]);
                    oCell.WrapText = true;
                    //认价合价
                    oCell = reportGrid.Cell(i + iStart, 7);
                    dSumMoney += ClientUtil.ToDecimal(oRow["ACCWORKQNYMoney"]);
                    oCell.Text = ClientUtil.ToString(oRow["ACCWORKQNYMoney"]);
                    oCell.WrapText = true;
                    //采购单价
                    oCell = reportGrid.Cell(i + iStart, 8);
                    oCell.Text = ClientUtil.ToString(oRow["price"]);
                    oCell.WrapText = true;
                    //采购合价
                    oCell = reportGrid.Cell(i + iStart, 9);
                    dSumConfirmMoney += ClientUtil.ToDecimal(oRow["money"]);
                    oCell.Text = ClientUtil.ToString(oRow["money"]);
                    oCell.WrapText = true;
                    reportGrid.Row(i + iStart).AutoFit();
                }
                oCell = reportGrid.Cell(oTable.Rows.Count + iStart, 7);
                oCell.Text = ClientUtil.ToString(dSumMoney);
                oCell = reportGrid.Cell(oTable.Rows.Count + iStart, 9);
                oCell.Text = ClientUtil.ToString(dSumConfirmMoney);
            }
        }

        private void FillReport_MaterialAccountReporter()
        {
            string sProjectID = string.Empty;
            string sUserPartID = string.Empty;
            string sUserRandID = string.Empty;
            //string sProfessionCat = string.Empty;
            int iStart = 5;
            DataRow oRow = null;
            FlexCell.Cell oCell = null;

            
            decimal dBalSumMoney = 0;
            decimal dMvInSumMoney = 0;
            decimal dMvOutSumMoney = 0;
            decimal dLossSumMoney = 0;
            decimal dStkOutSumMoney = 0;
            
            CurrentProjectInfo oProject = cmbProject.SelectedItem as CurrentProjectInfo;
            sProjectID = oProject.Id;
            reportGrid.AutoRedraw = false ;
            oCell = reportGrid.Cell(2, 2);
            oCell.Text = oProject.Name;
            oCell = reportGrid.Cell(2, 11);
            oCell.Text = this.dtpDateBegin .Value .ToShortDateString ();
            oCell = reportGrid.Cell(2, 15);
            oCell.Text = this.dtpDateEnd .Value.ToShortDateString();
            DataTable oTable = modelStock.StockInSrv.QueryMaterialAccount(sProjectID, dtpDateBegin.Value .ToShortDateString (), dtpDateEnd.Value .ToShortDateString ());

            string sStkInID = string.Empty;
            int  iStkInRow = 0;
            string sMvInID=string.Empty;
            int iMvInRow=0;
            string sMvOutID=string.Empty ;
            int iMvOutRow=0;
            string sStkOutID=string.Empty ;
            int iStkOutRow=0;
            if (oTable != null && oTable.Rows.Count > 0)
            {
                reportGrid.InsertRow(iStart, oTable.Rows.Count);
                for (int i = 0; i < oTable.Rows.Count; i++)
                {
                    //t.professioncategory ,nvl(t.supplierrelationname,'')supplierrelationname,
                    //       t1.materialname, t1.materialspec, t1.matstandardunitname ,t1.confirmprice stkinConfirmPrice,t1.descript stkinDescript,  ---入库单
                    //       nvl(t3.quantity,0.00) balQuantity ,nvl(t3.price,0.00)balPrice ,nvl(t3.money,0.00) balMoney ,t2.code balCode,t3.descript balDescript,
                    //       to_date(to_char(t2.createdate,'YYYY-MM-DD'),'YYYY-MM-DD') balCreatedate ,                ---验收单
                    //       nvl(t4.mvinQuantity,0.00)mvinQuantity,nvl( t4.mvinPrice,0.00)mvinPrice, nvl(t4.mvinMoney,0.00)mvinMoney,
                    //       nvl( t4.mvinCode,'')mvinCode, nvl(t4.mvinProject,'')mvinProject, nvl(t4.mvinDescript,'')mvinDescript, ---调入
                    //       nvl(t5.mvOutQuantity,0.00)mvOutQuantity,nvl(t5.mvOutPrice,0.00)mvOutPrice,nvl(t5.mvOutMoney,0.00)mvOutMoney,
                    //       nvl(t5.mvOutCode,'')mvOutCode,nvl(t5.mvOutProject,'')mvOutProject, nvl(t5.mvOutDescript,'')mvOutDescript,   ---调出
                    //       0 lostQuantity,0.00 lostPrice,0.00 lostMoney,'' lostCode ,'' lostDescript, ----报损
                    //       nvl(t6.stkOutQuantity,0.00)stkOutQuantity ,nvl(t6.stkOutMoney,0.00)stkOutMoney,
                    //       nvl(t6.stkOutCode,'')stkOutCode,nvl(t6.stkOutSupplierrelationname,'')stkOutSupplierrelationname   ---领料出库
                    oRow = oTable.Rows[i];
                    //序号
                    oCell = reportGrid.Cell(i + iStart, 1);
                    oCell.Text = (i + 1).ToString();
                    oCell.WrapText = true;

                    if (string.IsNullOrEmpty(sStkInID) || !string.Equals(ClientUtil.ToString(oRow["stkInID"]), sStkInID))
                    {
                        if (!string.IsNullOrEmpty(sStkInID))
                        {
                            sStkInID = ClientUtil.ToString(oRow["stkInID"]);
                            Merge(iStkInRow + iStart, 2, iStart + i - 1, 9);
                            iStkInRow = i;
                        }
                        else
                        {
                            sStkInID = ClientUtil.ToString(oRow["stkInID"]);
                            if (string.IsNullOrEmpty(sStkInID))
                            {
                                iStkInRow = i;
                            }
                            else
                            {
                                iStkInRow = i;
                            }
                        }
                       
                    }
                    //入库单据
                    FillCellNoReturn(i + iStart, 2, oRow, "professioncategory", FlexCell.AlignmentEnum.CenterCenter);//专业分类
                    FillCellNoReturn(i + iStart, 3, oRow, "supplierrelationname", FlexCell.AlignmentEnum.CenterCenter);//供应商
                    FillCellNoReturn(i + iStart, 4, oRow, "materialname", FlexCell.AlignmentEnum.CenterCenter);//物资名称
                    FillCellNoReturn(i + iStart, 5, oRow, "materialspec", FlexCell.AlignmentEnum.CenterCenter);//规格型号

                    FillCellNoReturn(i + iStart, 6, oRow, "diagramnumber", FlexCell.AlignmentEnum.CenterCenter);//图号

                    FillCellNoReturn(i + iStart, 7, oRow, "matstandardunitname", FlexCell.AlignmentEnum.CenterCenter);//单位
                    FillCellNoReturn(i + iStart, 8, oRow, "stkinConfirmPrice", FlexCell.AlignmentEnum.RightCenter);//业主确认价单价
                    FillCellNoReturn(i + iStart, 9, oRow, "stkinDescript", FlexCell.AlignmentEnum.RightCenter);//业主确认价备注
                    //验收
                    FillCellNoReturn(i + iStart, 10, oRow, "balQuantity", FlexCell.AlignmentEnum.RightCenter);//实收数量
                    FillCellNoReturn(i + iStart, 11, oRow, "balPrice", FlexCell.AlignmentEnum.RightCenter);//采购单价
                    dBalSumMoney += FillCellReturn(i + iStart, 12, oRow, "balMoney", FlexCell.AlignmentEnum.RightCenter);//采购合价
                    FillCellNoReturn(i + iStart, 13, oRow, "balCode", FlexCell.AlignmentEnum.CenterCenter);//验收单编号
                    FillCellNoReturn(i + iStart, 14, oRow, "balDescript", FlexCell.AlignmentEnum.CenterCenter);//备注
                    FillCellNoReturn(i + iStart, 15, oRow, "balCreatedate", FlexCell.AlignmentEnum.CenterCenter);//验收日期
                    //调入
                    if (string.IsNullOrEmpty(sMvInID) || !string.Equals(ClientUtil.ToString(oRow["mvInID"]), sMvInID))
                    {
                        if (!string.IsNullOrEmpty(sMvInID))
                        {
                            sMvInID = ClientUtil.ToString(oRow["mvInID"]);
                            Merge(iMvInRow + iStart, 16, iStart + i - 1, 21);
                            iMvInRow  = i;
                        }
                        else
                        {
                            sMvInID = ClientUtil.ToString(oRow["mvInID"]);
                            if (string.IsNullOrEmpty(sMvOutID))
                            {
                                iMvInRow = i;
                            }
                            else
                            {
                                iMvInRow = i;
                            }
                        }
                        FillCellNoReturn(i + iStart, 16, oRow, "mvinQuantity", FlexCell.AlignmentEnum.RightCenter);//调入数量
                        FillCellNoReturn(i + iStart, 17, oRow, "mvinPrice", FlexCell.AlignmentEnum.RightCenter);//调入单价
                        dMvInSumMoney += FillCellReturn(i + iStart, 18, oRow, "mvinMoney", FlexCell.AlignmentEnum.RightCenter);//调入金额
                        FillCellNoReturn(i + iStart, 19, oRow, "mvinCode", FlexCell.AlignmentEnum.CenterCenter);//调拨单编号
                        FillCellNoReturn(i + iStart, 20, oRow, "mvinProject", FlexCell.AlignmentEnum.CenterCenter);//调入项目
                        FillCellNoReturn(i + iStart, 21, oRow, "mvinDescript", FlexCell.AlignmentEnum.CenterCenter);//备注
                    }
                    //调出
                    if (string.IsNullOrEmpty(sMvOutID) || !string.Equals(ClientUtil.ToString(oRow["mvOutID"]), sMvOutID))
                    {
                        if (!string.IsNullOrEmpty(sMvOutID))
                        {
                            sMvOutID = ClientUtil.ToString(oRow["mvOutID"]);
                            Merge(iMvOutRow  + iStart, 22, iStart + i - 1, 27);
                            iMvOutRow = i;
                        }
                        else
                        {
                            sMvOutID = ClientUtil.ToString(oRow["mvOutID"]);
                            if (string.IsNullOrEmpty(sMvOutID))
                            {
                                iMvOutRow = i;
                            }
                            else
                            {
                                iMvOutRow = i;
                            }
                        }
                        FillCellNoReturn(i + iStart, 22, oRow, "mvOutQuantity", FlexCell.AlignmentEnum.RightCenter);//调出数量
                        FillCellNoReturn(i + iStart, 23, oRow, "mvOutPrice", FlexCell.AlignmentEnum.RightCenter);//调出单价
                        dMvOutSumMoney += FillCellReturn(i + iStart, 24, oRow, "mvOutMoney", FlexCell.AlignmentEnum.RightCenter);//调出金额
                        FillCellNoReturn(i + iStart, 25, oRow, "mvOutCode", FlexCell.AlignmentEnum.CenterCenter);//调拨单编号
                        FillCellNoReturn(i + iStart, 26, oRow, "mvOutProject", FlexCell.AlignmentEnum.CenterCenter);//调入单位名称
                        FillCellNoReturn(i + iStart, 27, oRow, "mvOutDescript", FlexCell.AlignmentEnum.CenterCenter);//备注
                    }
                    // ----报损
                    FillCellNoReturn(i + iStart, 28, oRow, "lostQuantity", FlexCell.AlignmentEnum.RightCenter );//报损数量
                    FillCellNoReturn(i + iStart, 29, oRow, "lostPrice", FlexCell.AlignmentEnum.RightCenter );//报损单价
                    dLossSumMoney += FillCellReturn(i + iStart, 30, oRow, "lostMoney", FlexCell.AlignmentEnum.RightCenter );//报损金额
                    FillCellNoReturn(i + iStart, 31, oRow, "lostCode", FlexCell.AlignmentEnum.CenterCenter);//报损单编号
                    FillCellNoReturn(i + iStart, 32, oRow, "lostDescript", FlexCell.AlignmentEnum.CenterCenter);//备注
                    //     ---领料出库
                    if (string.IsNullOrEmpty(sStkOutID) || !string.Equals(ClientUtil.ToString(oRow["stkOutID"]), sStkOutID))
                    {
                        if (!string.IsNullOrEmpty(sStkOutID))
                        {
                            sStkOutID = ClientUtil.ToString(oRow["stkOutID"]);
                            Merge(iStkOutRow + iStart, 33, iStart + i - 1, 37);
                            iStkOutRow  = i;
                        }
                        else
                        {
                            sStkOutID = ClientUtil.ToString(oRow["stkOutID"]);
                            if (string.IsNullOrEmpty(sStkOutID))
                            {
                                iStkOutRow = i;
                            }
                            else
                            {
                                iStkOutRow = i;
                            }
                        }
                        FillCellNoReturn(i + iStart, 33, oRow, "stkOutQuantity", FlexCell.AlignmentEnum.RightCenter);//领料数量
                        dStkOutSumMoney += FillCellReturn(i + iStart, 34, oRow, "stkOutMoney", FlexCell.AlignmentEnum.RightCenter);//领料金额
                        FillCellNoReturn(i + iStart, 35, oRow, "stkOutCode", FlexCell.AlignmentEnum.CenterCenter);//领料单编号
                        FillCellNoReturn(i + iStart, 36, oRow, "stkOutSupplierrelationname", FlexCell.AlignmentEnum.CenterCenter);//协作队伍
                        FillCellNoReturn(i + iStart, 37, oRow, "stkOutDescript", FlexCell.AlignmentEnum.CenterCenter);//备注 
                        reportGrid.Row(i + iStart).AutoFit();
                    }
                }
                if (iStkInRow + 1 != oTable.Rows.Count)
                {
                    //Merge(iStkInRow + iStart, 2, iStart + oTable.Rows.Count - 1, 15);
                    //Merge(iStkInRow + iStart, 2, iStart + oTable.Rows.Count - 1, 15);
                    Merge(iStkInRow + iStart, 2, iStart + oTable.Rows.Count - 1, 9);
                }
                if (iMvInRow + 1 != oTable.Rows.Count)
                {
                    Merge(iMvInRow + iStart, 16, iStart + oTable.Rows.Count - 1, 21);
                }
                if (iMvOutRow + 1 != oTable.Rows.Count)
                {
                    Merge(iMvOutRow + iStart, 22, iStart + oTable.Rows.Count - 1, 27);
                }
                if (iStkOutRow + 1 != oTable.Rows.Count)
                {
                    Merge(iStkOutRow + iStart, 33, iStart + oTable.Rows.Count - 1, 37);
                }
                oCell = reportGrid.Cell(oTable.Rows.Count + iStart, 12);
                oCell.Text = ClientUtil.ToString(dBalSumMoney);
                oCell.Alignment = FlexCell.AlignmentEnum.RightCenter;
                oCell = reportGrid.Cell(oTable.Rows.Count + iStart, 18);
                oCell.Text = ClientUtil.ToString(dMvInSumMoney);
                oCell.Alignment = FlexCell.AlignmentEnum.RightCenter;
                oCell = reportGrid.Cell(oTable.Rows.Count + iStart, 24);
                oCell.Text = ClientUtil.ToString(dMvOutSumMoney);
                oCell.Alignment = FlexCell.AlignmentEnum.RightCenter;
                oCell = reportGrid.Cell(oTable.Rows.Count + iStart, 30);
                oCell.Text = ClientUtil.ToString(dLossSumMoney);
                oCell.Alignment = FlexCell.AlignmentEnum.RightCenter;
                oCell = reportGrid.Cell(oTable.Rows.Count + iStart, 34);
                oCell.Text = ClientUtil.ToString(dStkOutSumMoney);
                oCell.Alignment = FlexCell.AlignmentEnum.RightCenter;
                reportGrid.Range(iStart, 1, oTable.Rows.Count + iStart, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                reportGrid.Range(iStart, 1, oTable.Rows.Count + iStart, 37).set_Borders(FlexCell.EdgeEnum.Inside  , FlexCell.LineStyleEnum.Thin);
            }
            reportGrid.AutoRedraw = true;
            reportGrid.Refresh();
        }
        public void FillCellNoReturn(int iRow, int iColumn, DataRow oRow, string sPropertyName,FlexCell .AlignmentEnum oAlignmentEnum)
        {
            if (oRow != null)
            {
                FlexCell.Cell oCell = null;
                oCell = reportGrid.Cell(iRow, iColumn);
                oCell.Text = ClientUtil.ToString(oRow[sPropertyName]);
                oCell.WrapText = true;
                oCell.Alignment = oAlignmentEnum;
            }
        }
        public decimal FillCellReturn(int iRow, int iColumn, DataRow oRow, string sPropertyName, FlexCell.AlignmentEnum oAlignmentEnum)
        {
            decimal dTemp = 0;
            if (oRow != null)
            {
                FlexCell.Cell oCell = null;
                oCell = reportGrid.Cell(iRow, iColumn);
                dTemp = ClientUtil.ToDecimal(oRow[sPropertyName]);
                oCell.Text = dTemp.ToString();
                oCell.WrapText = true;
                oCell.Alignment = oAlignmentEnum;
            }
            return dTemp;
        }
        public void Merge(int iStartRow, int iStartColumn, int iEndRow, int iEndColumn)
        {
            for (int i = iStartColumn; i <= iEndColumn; i++)
            {
                FlexCell.Range oRange = reportGrid.Range(iStartRow, i, iEndRow, i);
                oRange.Merge();
            }
        }
    }
}