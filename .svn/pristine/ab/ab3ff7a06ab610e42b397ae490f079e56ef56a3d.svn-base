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
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;

namespace Application.Business.Erp.SupplyChain.Client.StockMng.Report
{
    public partial class LJReport : TBasicDataView
    {
        private MStockMng model = new MStockMng();
        //private string formatMoney = "################.##";
        private string formatMoney = "###############0.##";
        private string formatQuantity = "###############0.####";
        private bool IsSelectParts = true ;
        #region 报表名称
        /// <summary>
        /// 料具租赁月报
        /// </summary>
        private string RN_clsfcybb = "料具租赁月报";
      
        #endregion

        public LJReport()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        private void InitData()
        {
            for (int i = 0; i < 13; i++)
            {
                this.cboFiscalYear.Items.Add(ConstObject.TheLogin.TheComponentPeriod.NowYear + (i - 3));
            }
            for (int i = 1; i < 13; i++)
            {
                this.cboFiscalMonth.Items.Add(i);
            }

            this.cboFiscalYear.Text = ConstObject.TheLogin.TheComponentPeriod.NowYear.ToString();
            this.cboFiscalMonth.Text = ConstObject.TheLogin.TheComponentPeriod.NowMonth.ToString();

            cmbReportName.Items.Clear();
            cmbReportName.Items.Add(RN_clsfcybb);
            cmbReportName.SelectedIndex = 0;

            CurrentProjectInfo pi = StaticMethod.GetProjectInfo();
            IList projectLst = new ArrayList();//这里可以查询所有的项目
            projectLst.Add(pi);
            cmbProject.DataSource = projectLst;
            cmbProject.DisplayMember = "Name";
            cmbProject.ValueMember = "Id";
            cmbProject.SelectedItem = pi;

            txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode1;
        }

        private void InitEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            cmbReportName.SelectedIndexChanged += new EventHandler(cmbReportName_SelectedIndexChanged);
            this.btnSelectUsePart.Click += new EventHandler(btnSelectPart_Click);
        }

        void cmbReportName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string reportName = cmbReportName.SelectedItem + "";
            if (reportName == RN_clsfcybb)
            {
                LoadTempleteFile(@"料具租赁月报.flx");
            }
            
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            string reportName = cmbReportName.SelectedItem + "";            

            if (reportName == this.RN_clsfcybb)
            {
                LoadTempleteFile(@"料具租赁月报.flx");
                FillReport_ljzlybb();
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
                    IList list = new ArrayList();
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
                            list.Add(task);
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

            tempUsedPartLst = model.StockInSrv.GetObjects(typeof(GWBSTree), oq);
            return tempUsedPartLst;
        }


        private void FillReport_ljzlybb()
        {
            CurrentProjectInfo cpi = cmbProject.SelectedItem as CurrentProjectInfo;
            string strFiscalYear = cboFiscalYear.Text;
            string strFiscalMonth = cboFiscalMonth.Text;
            int fiscalYear = 0, fiscalMonth = 0;
            try
            {
                fiscalYear = int.Parse(strFiscalYear);
                fiscalMonth = int.Parse(strFiscalMonth);
            }
            catch
            {
                MessageBox.Show("请输入正确的会计年月。");
                return;
            }
            //if (string.IsNullOrEmpty(txtSupplier.Text) || txtSupplier.Result == null || txtSupplier.Result.Count == 0)
            //{
            //    MessageBox.Show("请先选择一个料具供应商。");
            //    return;
            //}
            string supplierId = "";
            if (!string.IsNullOrEmpty(txtSupplier.Text) && txtSupplier.Result != null && txtSupplier.Result.Count != 0)
            {
                supplierId = (txtSupplier.Result[0] as SupplierRelationInfo).Id;
            }

            reportGrid.Cell(2, 1).Text = fiscalYear + "年" + fiscalMonth+"月料具租赁月报";
            reportGrid.Cell(4, 2).Text = cpi.Name;

            DataSet ds = model.StockInOutSrv.WZBB_Ljzlyb(cpi.Id, fiscalYear, fiscalMonth, supplierId);
            if (ds == null || ds.Tables.Count == 0) return;
            DataTable dt = ds.Tables[0];
            if (dt == null) return;

            int detailStartRowNumber = 8;//8为模板中的行号
            int detailCount = dt.Rows.Count;
            if (detailCount == 0) return;
            //插入报表行
            reportGrid.InsertRow(detailStartRowNumber, detailCount);

            decimal sumJcslby = 0M, sumJcsllj = 0M, sumZljeby = 0M, sumZljelj = 0M, sumTcslby = 0M, sumTcsllj = 0M, sumJcsl = 0M;
            decimal sum1 = 0M, sum2 = 0M, sum3 = 0M, sum4 = 0M, sum5 = 0M, sum6 = 0M, sum7 = 0M, sum8 = 0M, sum9 = 0M, sum10 = 0M, sum11 = 0M,sum12 = 0M;
            for (int i = 0; i < detailCount; i++)
            {
                DataRow dr = dt.Rows[i];
                string materialId = dr["material"] + "";
                string matcode = dr["materialcode"] + "";
                string matspec = dr["materialspec"] + "";
                string matname = dr["materialname"] + "";
                string unitname = dr["matstandardunitname"] + "";
                string endDate = dr["enddate"] + "";
                reportGrid.Cell(4,10).Text=endDate;
                /******/
                decimal jcslby=ClientUtil.ToDecimal(dr["jcslby"]);//进场数量
                decimal jcsllj=ClientUtil.ToDecimal(dr["jcsllj"]);
                decimal zljeby=ClientUtil.ToDecimal(dr["zljeby"]);//租赁金额
                decimal zljelj=ClientUtil.ToDecimal(dr["zljelj"]);
                decimal whslby=ClientUtil.ToDecimal(dr["whslby"]);//完好数量
                decimal whsllj=ClientUtil.ToDecimal(dr["whsllj"]);
                decimal wxslby=ClientUtil.ToDecimal(dr["wxslby"]);//维修数量
                decimal wxsllj=ClientUtil.ToDecimal(dr["wxsllj"]);
                decimal qtslby=ClientUtil.ToDecimal(dr["qtslby"]);//切头数量
                decimal qtsllj=ClientUtil.ToDecimal(dr["qtsllj"]);
                decimal bfslby=ClientUtil.ToDecimal(dr["bfslby"]);//报废数量
                decimal bfsllj=ClientUtil.ToDecimal(dr["bfsllj"]);

                decimal bsslby = ClientUtil.ToDecimal(dr["bsslby"]);//报损数量
                decimal bssllj = ClientUtil.ToDecimal(dr["bssllj"]);//
                decimal xhslby = ClientUtil.ToDecimal(dr["xhslby"]);//消耗数量
                decimal xhsllj = ClientUtil.ToDecimal(dr["xhsllj"]);//

                decimal tcslby=ClientUtil.ToDecimal(dr["tcslby"]);//退场数量
                decimal tcsllj=ClientUtil.ToDecimal(dr["tcsllj"]);
                decimal jcsl=ClientUtil.ToDecimal(dr["jcsl"]);//结存
                decimal sysj=ClientUtil.ToDecimal(dr["sysj"]);//使用时间

                sumJcslby += jcslby;
                sumJcsllj += jcsllj;
                sumZljeby += zljeby;
                sumZljelj += zljelj;
                sumTcslby += tcslby;
                sumTcsllj += tcsllj;
                sumJcsl += jcsl;

                sum1 += whslby;//完好
                sum2 += whsllj;
                sum3 += bsslby;//报废
                sum4 += bfsllj;
                sum5 += wxslby;//丢失
                sum6 += wxsllj;
                sum7 += qtslby;//消耗
                sum8 += qtsllj;
                sum9 += bsslby;//调拨
                sum10 += bssllj;
                sum11 += xhslby;//合计
                sum12 += xhsllj;

                reportGrid.Cell(detailStartRowNumber + i, 1).Text = matname;//物资名称
                reportGrid.Cell(detailStartRowNumber + i, 1).Tag = materialId;//物资名称
                reportGrid.Cell(detailStartRowNumber + i, 2).Text = matspec;//规格
                reportGrid.Cell(detailStartRowNumber + i, 3).Text = unitname;//单位
                reportGrid.Cell(detailStartRowNumber + i, 4).Text = CommonUtil.NumberToStringFormate(jcslby,4);//本月进场数量
                reportGrid.Cell(detailStartRowNumber + i, 5).Text = CommonUtil.NumberToStringFormate(jcsllj,4);//累计进场数量
                reportGrid.Cell(detailStartRowNumber + i, 6).Text = CommonUtil.NumberToStringFormate(zljeby,2);//本月租赁金额
                reportGrid.Cell(detailStartRowNumber + i, 7).Text = CommonUtil.NumberToStringFormate(zljelj,2);//累计租赁金额

                reportGrid.Cell(detailStartRowNumber + i, 8).Text = CommonUtil.NumberToStringFormate(whslby,4);//本月完好数量
                reportGrid.Cell(detailStartRowNumber + i, 9).Text = CommonUtil.NumberToStringFormate(whsllj,4);//累计完好数量
                reportGrid.Cell(detailStartRowNumber + i, 10).Text = CommonUtil.NumberToStringFormate(bfslby,4);//本月报废数量
                reportGrid.Cell(detailStartRowNumber + i, 11).Text = CommonUtil.NumberToStringFormate(bfsllj,4);//累计报废数量
                reportGrid.Cell(detailStartRowNumber + i, 12).Text = CommonUtil.NumberToStringFormate(wxslby,4);//本月维修数量
                reportGrid.Cell(detailStartRowNumber + i, 13).Text = CommonUtil.NumberToStringFormate(wxsllj,4);//累计维修数量
                reportGrid.Cell(detailStartRowNumber + i, 14).Text = CommonUtil.NumberToStringFormate(qtslby,4);//本月切头数量
                reportGrid.Cell(detailStartRowNumber + i, 15).Text = CommonUtil.NumberToStringFormate(qtsllj,4);//累计切头数量
                reportGrid.Cell(detailStartRowNumber + i, 16).Text = CommonUtil.NumberToStringFormate(bsslby,4);//本月报损数量
                reportGrid.Cell(detailStartRowNumber + i, 17).Text = CommonUtil.NumberToStringFormate(bssllj,4);//累计报损数量
                reportGrid.Cell(detailStartRowNumber + i, 18).Text = CommonUtil.NumberToStringFormate(bsslby,4);//本月消耗数量
                reportGrid.Cell(detailStartRowNumber + i, 19).Text = CommonUtil.NumberToStringFormate(bssllj,4);//累计消耗数量   
                reportGrid.Cell(detailStartRowNumber + i, 20).Text = CommonUtil.NumberToStringFormate(tcslby,4);//本月退场数量
                reportGrid.Cell(detailStartRowNumber + i, 21).Text = CommonUtil.NumberToStringFormate(tcsllj,4);//累计退场数量

                reportGrid.Cell(detailStartRowNumber + i, 22).Text = CommonUtil.NumberToStringFormate(jcsl,4);//结存数量 料具年损耗率
                decimal hsl = 0M;
                if (jcsllj != 0 && sysj != 0)
                {
                    hsl = (bfsllj + bssllj) / jcsllj / sysj * 12 * 100;
                }
                reportGrid.Cell(detailStartRowNumber + i, 23).Text = CommonUtil.NumberToStringFormate(hsl,2);//料具年损耗率
            }

            //其他费用
            DataSet dsFy = model.StockInOutSrv.WZBB_Ljzlyb_qtfy(cpi.Id, fiscalYear, fiscalMonth, supplierId);
            DataTable dtFy = dsFy.Tables[0];
            int fyRowCount=dtFy.Rows.Count;

            //插入报表费用行
            reportGrid.InsertRow(detailStartRowNumber + detailCount, fyRowCount);

            for (int j = 0; j < fyRowCount; j++)
            {
                DataRow drFy = dtFy.Rows[j];
                string costType = drFy["costType"] + "";
                decimal fyjeby = ClientUtil.ToDecimal(drFy["fyjeby"]);
                decimal fyjelj = ClientUtil.ToDecimal(drFy["fyjelj"]);

                reportGrid.Cell(detailStartRowNumber + detailCount + j, 1).Text = costType;
                reportGrid.Cell(detailStartRowNumber + detailCount + j, 1).Tag = costType;
                reportGrid.Cell(detailStartRowNumber + detailCount + j, 6).Text = CommonUtil.NumberToStringFormate(fyjeby,2);
                reportGrid.Cell(detailStartRowNumber + detailCount + j, 7).Text = CommonUtil.NumberToStringFormate(fyjelj,2);

                sumZljeby += fyjeby;
                sumZljelj += fyjelj;
            }
          
            //合计
            int summaryRowIndex = detailStartRowNumber + detailCount + fyRowCount;
            reportGrid.Cell(summaryRowIndex, 1).Text = "合计";
            reportGrid.Cell(summaryRowIndex, 4).Text = CommonUtil.NumberToStringFormate(sumJcslby,4);
            reportGrid.Cell(summaryRowIndex, 5).Text = CommonUtil.NumberToStringFormate(sumJcsllj,4);
            reportGrid.Cell(summaryRowIndex, 6).Text = CommonUtil.NumberToStringFormate(sumZljeby,2);
            reportGrid.Cell(summaryRowIndex, 7).Text = CommonUtil.NumberToStringFormate(sumZljelj,2);

            reportGrid.Cell(summaryRowIndex, 8).Text = CommonUtil.NumberToStringFormate(sum1,2);
            reportGrid.Cell(summaryRowIndex, 9).Text = CommonUtil.NumberToStringFormate(sum2,2);
            reportGrid.Cell(summaryRowIndex, 10).Text = CommonUtil.NumberToStringFormate(sum3,2);
            reportGrid.Cell(summaryRowIndex, 11).Text = CommonUtil.NumberToStringFormate(sum4,2);
            reportGrid.Cell(summaryRowIndex, 12).Text = CommonUtil.NumberToStringFormate(sum5,2);
            reportGrid.Cell(summaryRowIndex, 13).Text = CommonUtil.NumberToStringFormate(sum6,2);
            reportGrid.Cell(summaryRowIndex, 14).Text = CommonUtil.NumberToStringFormate(sum7,2);
            reportGrid.Cell(summaryRowIndex, 15).Text = CommonUtil.NumberToStringFormate(sum8,2);
            reportGrid.Cell(summaryRowIndex, 16).Text = CommonUtil.NumberToStringFormate(sum9,2);
            reportGrid.Cell(summaryRowIndex, 17).Text = CommonUtil.NumberToStringFormate(sum10,2);
            reportGrid.Cell(summaryRowIndex, 18).Text = CommonUtil.NumberToStringFormate(sum11,2);
            reportGrid.Cell(summaryRowIndex, 19).Text = CommonUtil.NumberToStringFormate(sum12,2);

            reportGrid.Cell(summaryRowIndex, 20).Text =CommonUtil.NumberToStringFormate(sumTcslby,4);
            reportGrid.Cell(summaryRowIndex, 21).Text = CommonUtil.NumberToStringFormate(sumTcsllj,4);
            reportGrid.Cell(summaryRowIndex, 22).Text = CommonUtil.NumberToStringFormate(sumJcsl, 4);
            if (this.IsSelectParts)
            {
                IList lstUsePart = this.txtUsePart.Tag as IList;
                //Xhmx_clsfchzb(beginDate, endDate, pi.Id, summaryRowIndex, lstUsePart, isSummary);
                //model.StockInOutSrv.WZBB_Ljzlyb(cpi.Id, fiscalYear, fiscalMonth, supplierId)
                Xhmx_clsfchzb(cpi.Id, fiscalYear, fiscalMonth, supplierId, lstUsePart, summaryRowIndex);
            }
            else
            {
                UsedPartMoney(fiscalYear, fiscalMonth, cpi.Id, supplierId, summaryRowIndex, detailStartRowNumber);
            }

            FlexCell.Range range = reportGrid.Range(detailStartRowNumber, 1, summaryRowIndex, 3);
            range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);

            range = reportGrid.Range(detailStartRowNumber, 4, summaryRowIndex, reportGrid.Cols - 2);
            range.Alignment = FlexCell.AlignmentEnum.RightCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);


           

        }
        #region 加载部位
        //model.StockInOutSrv.WZBB_Ljzlyb(cpi.Id, fiscalYear, fiscalMonth, supplierId)
        private void Xhmx_clsfchzb(string projectId, int fiscalYear, int fiscalMonth, string supplierId, IList lstUsePart, int summaryRowIndex)
        {
            string sNotKnow = "其他部位退场";
            string sSubjectTempID = string.Empty;
            QtyMoney oQtyMoney;
            
            if (lstUsePart != null && lstUsePart.Count > 0)
            {
                Hashtable hs = SetHeadUsePart(lstUsePart , sNotKnow);
                string sOther = "调整费用";
                if (!hs.ContainsKey(sOther))
                {
                    hs.Add(sOther, new QtyMoney());
                }
               
                //if (!hs.ContainsKey(sOther))
                //{
                //    hs.Add(sOther, new QtyMoney());

                //    int startCol = reportGrid.Cols - 1;
                //    reportGrid.InsertCol(startCol, 4);
                //    reportGrid.Cell(5, startCol).Text = sOther;
                //    reportGrid.Cell(5, startCol).Tag = sOther;
                //    reportGrid.Cell(5, startCol + 2).Text = sOther;
                //    reportGrid.Cell(5, startCol).Text = sOther;
                //    reportGrid.Cell(7, startCol).Text = "本月数量";
                //    reportGrid.Cell(7, startCol + 1).Text = "本月金额";
                //    reportGrid.Cell(7, startCol + 2).Text = "累计数量";
                //    reportGrid.Cell(7, startCol + 3).Text = "累计金额";
                //    FlexCell.Range oRange = reportGrid.Range(5, startCol , 6, startCol + 3);
                //    oRange.MergeCells = true;
                //    oRange.Alignment = FlexCell.AlignmentEnum.CenterCenter;
                //    oRange.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
                //    oRange.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
                //    reportGrid.Column(startCol).Visible = false;

                //    reportGrid.Column(startCol + 2).Visible = false;
                //}
                if (hs.Count > 0)
                {
                    DataSet ds = model.StockInOutSrv.WZBB_LjzlybByUsePart(projectId, fiscalYear, fiscalMonth, supplierId, lstUsePart);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        FillText(ds.Tables[0], hs, summaryRowIndex, sNotKnow);
                    }
                    for (int j = reportGrid.Cols - 1; j >= 0; j--)
                    {
                        sSubjectTempID = reportGrid.Cell(6, j).Tag;
                        if (string.IsNullOrEmpty(sSubjectTempID)) { continue; }
                        if (hs.ContainsKey(sSubjectTempID))
                        {
                            oQtyMoney = hs[sSubjectTempID] as QtyMoney;
                            if (oQtyMoney.Ltmoney == 0 && oQtyMoney.Ltqty == 0 && oQtyMoney.Money == 0 && oQtyMoney.Qty == 0)
                            {
                                reportGrid.Column(j).Visible = false;
                                reportGrid.Column(j + 1).Visible = false;
                                reportGrid.Column(j+2).Visible = false;
                                reportGrid.Column(j + 3).Visible = false;
                            }
                        }
                    }
                }
               
            }
        }
        public void FillText(DataTable tbSum, Hashtable ht,  int summaryRowIndex, string sNotKnow)
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
            //decimal SubjectSunMoney = 0;
            decimal SubjectLstSumNum = 0;
            //decimal SubjectLstSunMoney = 0;
            decimal SubjectSumMoney = 0;
            decimal SubjectLstMoney = 0;
            string sWhere = string.Empty;
            int iEndCol = 0;
            int iColumn = 0;
            bool isFind = false;
            QtyMoney oQtyMoney = null;
            bool IsExistNoKnow = false;
            string sOther = "调整费用";
           // iEndCol = reportGrid.Cols-5 ;
            iEndCol = reportGrid.Cols - 5 ;
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
                            isFind = false;
                            sHeadID = oRow["HeadID"].ToString();
                            SubjectSumNum = ClientUtil.ToDecimal(oRow["Quantity"]);
                            SubjectLstSumNum = ClientUtil.ToDecimal(oRow["lstQuantity"]);
                            SubjectSumMoney = ClientUtil.ToDecimal(oRow["moneyby"]);
                            SubjectLstMoney = ClientUtil.ToDecimal(oRow["moneylj"]);
                           
                            if (string.IsNullOrEmpty(sHeadID))
                            {
                                iColumn = iEndCol;
                                sHeadID = sNotKnow;
                                IsExistNoKnow = true;
                            }
                            else
                            {
                                if (string.Equals(sHeadID, "调整费用"))
                                {
                                    isFind = true;
                                }
                                else
                                {
                                    for (int j = reportGrid.Cols - 1; j >= 0; j--)
                                    {
                                        sSubjectTempID = reportGrid.Cell(6, j).Tag;
                                        if (string.IsNullOrEmpty(sSubjectTempID))
                                        {
                                            sSubjectTempID = reportGrid.Cell(5, j).Tag;
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
                                 
                            }
                            if (ht.ContainsKey(sHeadID))
                            {
                                oQtyMoney = ht[sHeadID] as QtyMoney;
                                oQtyMoney.Qty += SubjectSumNum;
                                oQtyMoney.Ltqty += SubjectLstSumNum;
                                oQtyMoney.Money += SubjectSumMoney;
                                oQtyMoney.Ltmoney += SubjectLstMoney;
                                ht[sHeadID] = oQtyMoney;
                            }
                            if (string.Equals(sHeadID, "调整费用"))
                            {
                            }
                            else{
                                decimal dTemp = 0;
                                dTemp = ClientUtil.ToDecimal(reportGrid.Cell(i, iColumn).Text) + SubjectSumNum;
                                reportGrid.Cell(i, iColumn).Text = CommonUtil.NumberToStringFormate(dTemp, 4);
                                dTemp = ClientUtil.ToDecimal(reportGrid.Cell(i, iColumn + 1).Text) + SubjectSumMoney;
                                reportGrid.Cell(i, iColumn + 1).Text = CommonUtil.NumberToStringFormate(dTemp, 2);
                                dTemp = ClientUtil.ToDecimal(reportGrid.Cell(i, iColumn + 2).Text) + SubjectLstSumNum;
                                reportGrid.Cell(i, iColumn + 2).Text = CommonUtil.NumberToStringFormate(dTemp, 4);
                                dTemp = ClientUtil.ToDecimal(reportGrid.Cell(i, iColumn + 3).Text) + SubjectLstMoney;
                                reportGrid.Cell(i, iColumn + 3).Text = CommonUtil.NumberToStringFormate(dTemp, 2);
                            }
                           
                        }

                    }
                }
            }
            
            if (ht.ContainsKey(sOther))
            {
                decimal dDyMoney=0;
                decimal dLjMoney=0;
                decimal dDyMoneyTmp = 0;
                decimal dLjMoneyTemp = 0;
                decimal dMoney=0;
                int iCount = 0;
              
                int iDyCol = 0;
                oQtyMoney = ht[sOther] as QtyMoney;
                QtyMoney QtyMoneyTmp = null;
                if (oQtyMoney != null && (oQtyMoney.Ltmoney != 0  || oQtyMoney.Money != 0))
                {
                    foreach (string sKey in ht.Keys)
                    {
                        if (string.Equals(sOther, sKey))
                        {
                        }
                        else
                        {
                            QtyMoneyTmp = ht[sKey] as QtyMoney;
                            if (QtyMoneyTmp != null)
                            {
                                if (QtyMoneyTmp.Ltmoney != 0 || QtyMoneyTmp.Ltqty != 0 || QtyMoneyTmp.Money != 0 || QtyMoneyTmp.Qty != 0)
                                {
                                    iCount++;
                                }
                            }
                        }
                    }
                    if(iCount>0)
                    {
                    for (int i = 3; i < reportGrid.Rows; i++)
                    {
                        sMaterialID = reportGrid.Cell(i, 1).Tag;
                        if (string.Equals(sMaterialID, sOther))
                        {
                            for (int j = 0; j< reportGrid.Cols ; j++)
                            {
                                sSubjectTempID = reportGrid.Cell(6, j).Tag;
                                if (string.IsNullOrEmpty(sSubjectTempID))
                                {
                                    sSubjectTempID = reportGrid.Cell(5, j).Tag;
                                }
                                if (!string.IsNullOrEmpty(sSubjectTempID) && ht.ContainsKey(sSubjectTempID))
                                {
                                    QtyMoneyTmp = ht[sSubjectTempID] as QtyMoney;
                                    if (QtyMoneyTmp.Ltmoney != 0 || QtyMoneyTmp.Ltqty != 0 || QtyMoneyTmp.Money != 0 || QtyMoneyTmp.Qty != 0)
                                    {
                                        iDyCol = j;
                                        dMoney = Math.Round( oQtyMoney.Money/iCount, 2);
                                        reportGrid.Cell(i, j + 1).Text = CommonUtil.NumberToStringFormate(dMoney, 2);
                                        dDyMoneyTmp += dMoney;
                                        QtyMoneyTmp.Money += dMoney;
                                        dMoney = Math.Round(oQtyMoney.Ltmoney / iCount, 2);
                                        reportGrid.Cell(i, j + 3).Text = CommonUtil.NumberToStringFormate(dMoney, 2);
                                        dLjMoneyTemp += dMoney;
                                        QtyMoneyTmp.Ltmoney += dMoney;
                                        ht[sSubjectTempID] = QtyMoneyTmp;
                                    }
                                    
                                }
                            }
                            if (iDyCol != 0)
                            {
                                if (oQtyMoney.Money != dDyMoneyTmp)
                                {
                                    dMoney = ClientUtil.ToDecimal(reportGrid.Cell(i, iDyCol + 1).Text);
                                    dMoney += (oQtyMoney.Money - dDyMoneyTmp);
                                    reportGrid.Cell(i, iDyCol + 1).Text = CommonUtil.NumberToStringFormate(dMoney, 2);
                                    sSubjectTempID = reportGrid.Cell(6, iDyCol).Tag;
                                    if (string.IsNullOrEmpty(sSubjectTempID))
                                    {
                                        sSubjectTempID = reportGrid.Cell(5, iDyCol).Tag;
                                    }
                                    if (!string.IsNullOrEmpty(sSubjectTempID) && ht.ContainsKey(sSubjectTempID))
                                    {
                                        QtyMoneyTmp = ht[sSubjectTempID] as QtyMoney;
                                        QtyMoneyTmp.Money += (oQtyMoney.Money - dDyMoneyTmp);
                                        ht[sSubjectTempID] = QtyMoneyTmp;
                                    }
                                }
                                if (oQtyMoney.Ltmoney != dLjMoneyTemp)
                                {
                                    dMoney = ClientUtil.ToDecimal(reportGrid.Cell(i, iDyCol + 3).Text);
                                    dMoney += (oQtyMoney.Ltmoney - dLjMoneyTemp);
                                    reportGrid.Cell(i, iDyCol + 3).Text = CommonUtil.NumberToStringFormate(dMoney, 2);
                                    sSubjectTempID = reportGrid.Cell(6, iDyCol).Tag;
                                    if (string.IsNullOrEmpty(sSubjectTempID))
                                    {
                                        sSubjectTempID = reportGrid.Cell(5, iDyCol).Tag;
                                    }
                                    if (!string.IsNullOrEmpty(sSubjectTempID) && ht.ContainsKey(sSubjectTempID))
                                    {
                                        QtyMoneyTmp = ht[sSubjectTempID] as QtyMoney;
                                        QtyMoneyTmp.Ltmoney += (oQtyMoney.Ltmoney - dLjMoneyTemp);
                                        ht[sSubjectTempID] = QtyMoneyTmp;
                                    }
                                }
                                
                            }
                             
                            break ;
                        }
                    }
                    }
                }
            }
            for (int j = reportGrid.Cols - 1; j >= 0; j--)
            {
                sSubjectTempID = reportGrid.Cell(6, j).Tag;
                if (string.IsNullOrEmpty(sSubjectTempID))
                {
                    sSubjectTempID = reportGrid.Cell(5, j).Tag;
                }
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

            reportGrid.Column(iEndCol).Visible = IsExistNoKnow;
            reportGrid.Column(iEndCol + 1).Visible = IsExistNoKnow;
            reportGrid.Column(iEndCol + 2).Visible = IsExistNoKnow;
            reportGrid.Column(iEndCol + 3).Visible = IsExistNoKnow;
        }
        public Hashtable SetHeadUsePart(IList lstUsePart , string sNotKnow)
        {

            Hashtable ht = null;
            if (lstUsePart != null && lstUsePart.Count > 0)
            {
                int startCol = 0;
                ht = new Hashtable();
                foreach (GWBSTree oGWBSTree in lstUsePart)  //写部位
                {
                    startCol = reportGrid.Cols - 1;
                    InsertGridCol4Clsfchzb(startCol, 5, oGWBSTree.Name);
                    reportGrid.Cell(6, startCol).Tag = oGWBSTree.Id;
                    if (!ht.ContainsKey(oGWBSTree.Id))
                    {
                        ht.Add(oGWBSTree.Id, new QtyMoney());
                    }
                }
                 
                    startCol = reportGrid.Cols - 1;
                    reportGrid.InsertCol(startCol, 4);
                    reportGrid.Cell(5, startCol).Text = sNotKnow;
                    reportGrid.Cell(5, startCol).Tag = sNotKnow;
                    FlexCell.Range oRange = reportGrid.Range(5, startCol, 6, startCol + 3);
                    oRange.MergeCells = true;
                    oRange.Alignment = FlexCell.AlignmentEnum.CenterCenter;
                    oRange.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
                    oRange.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
                    reportGrid.Cell(7, startCol).Text = "本月数量";
                    reportGrid.Cell(7, startCol+1).Text = "本月金额";
                    reportGrid.Cell(7, startCol + 2).Text = "累计数量";
                    reportGrid.Cell(7, startCol + 3).Text = "累计金额";
              
                if (!ht.ContainsKey(sNotKnow))
                {
                    ht.Add(sNotKnow, new QtyMoney());
                }

            }
            return ht;
        }
        private void InsertGridCol(int startCol, int tableHeaderStartRow, string usedPartName)
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
            reportGrid.InsertCol(startCol, 4);
            FlexCell.Range range = reportGrid.Range(tableHeaderStartRow, startCol, tableHeaderStartRow, startCol + 3);
            range.MergeCells = true;
            reportGrid.Cell(tableHeaderStartRow, startCol).Text = "退场明细";
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
        #endregion
        /// <summary>
        /// 料具租赁月报 消耗明细
        /// </summary>
        /// <param name="fiscalYear"></param>
        /// <param name="fiscalMonth"></param>
        /// <param name="projectId"></param>
        /// <param name="supplierId"></param>
        /// <param name="summaryRowIndex">合计行索引</param>
        /// <param name="detailStartRowNumber">明细开始行号</param>
        private void UsedPartMoney(int fiscalYear, int fiscalMonth, string projectId, string supplierId, int summaryRowIndex,int detailStartRowNumber)
        {
            IList usedPartLst = GetUsedPart(projectId);
            if (usedPartLst == null || usedPartLst.Count == 0) return;
            int addedCols = 0;
            int startRowIndex=5;//5为报表中行数

            int firstCol=reportGrid.Cols - 1;//保存插入使用部位列前的列索引

            foreach (GWBSTree usedPart in usedPartLst)
            {
                int startCol = reportGrid.Cols - 1;

                reportGrid.InsertCol(startCol, 1);
                reportGrid.Cell(startRowIndex + 2, startCol).Text = usedPart.Name;
                addedCols = addedCols + 1;

                DataSet ds = model.StockInOutSrv.WZBB_Ljzlyb_xhmx(projectId, fiscalYear,fiscalMonth,supplierId,usedPart.SysCode);
                if (ds == null || ds.Tables.Count == 0) continue;
                DataTable dt = ds.Tables[0];
                if (dt == null || dt.Rows.Count == 0) continue;
                decimal sumXhje = 0M;
                foreach (DataRow dr in dt.Rows)
                {
                    string materialId = dr["material"] + "";
                    decimal xhje = ClientUtil.ToDecimal(dr["money"]);
                    int machedRowIndex = MachedRowIndex(materialId);
                    if (machedRowIndex == int.MinValue) continue;
                    reportGrid.Cell(machedRowIndex, startCol).Text = CommonUtil.NumberToStringFormate(xhje,2);
                    
                    sumXhje += xhje;
                }
                reportGrid.Cell(summaryRowIndex, startCol).Text = CommonUtil.NumberToStringFormate(sumXhje,4);
            }

            //插入合计列
            int startCol2 = reportGrid.Cols - 1;

            reportGrid.InsertCol(startCol2, 1);
            reportGrid.Cell(startRowIndex + 2, startCol2).Text = "合计";
            addedCols = addedCols + 1;

            //使用部位列 行头格式化
            FlexCell.Range range = reportGrid.Range(startRowIndex, firstCol, startRowIndex + 1, firstCol + addedCols-1);
            range.MergeCells = true;
            range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            reportGrid.Cell(startRowIndex, firstCol).Text = "本月发出";

            range = reportGrid.Range(startRowIndex + 2, firstCol, startRowIndex + 2, firstCol + addedCols-1);
            range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);

            //使用部位横排合计
            for (int i = detailStartRowNumber; i <= summaryRowIndex; i++)
            {
                decimal sumMoney = 0M;
                for (int colIndex = firstCol; colIndex < firstCol + addedCols - 1; colIndex++)
                {
                    decimal tempMoney = ClientUtil.ToDecimal(reportGrid.Cell(i,colIndex).Text);
                    sumMoney += tempMoney;
                }
                reportGrid.Cell(i, firstCol + addedCols - 1).Text = CommonUtil.NumberToStringFormate(sumMoney, 2);
            }
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
    }
}
