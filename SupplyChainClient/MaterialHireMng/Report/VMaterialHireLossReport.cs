using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report
{
    public partial class VMaterialHireLossReport  : TBasicDataView
    {
        MMaterialHireMng model = new MMaterialHireMng();
        public VMaterialHireLossReport()
        {
            InitializeComponent();
            IntialData();
            IntialEvent();
        }
        public void IntialData()
        {
            for (int iYear = 2010; iYear <= DateTime.Now.Year+1; iYear++)
            {
                cmbYear.Items.Add(iYear);
                
            }
            cmbYear.SelectedIndex = cmbYear.Items.Count-2;
            cmbQuarter.Items.Insert(0, "第一季度");
            cmbQuarter.Items.Insert(1, "第二季度");
            cmbQuarter.Items.Insert(2, "第三季度");
            cmbQuarter.Items.Insert(3, "第四季度");
            cmbQuarter.SelectedIndex = ClientUtil.ToInt(ClientUtil.ToString((DateTime.Now.Month - 1)/3).Substring(0,1));  //Math.Floor( ((DateTime.Now.Month-1) / 3));
            IntialHead();
        }
        public void IntialEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
        }
        public void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                
                FlashScreen.Show("正在生成[在建项目使用情况表]报告...");
                fGridDetail.AutoRedraw = false;
                IntialHead();
                FillHead();
                FillData(GetDate());
                FillFoot();
                FlashScreen.Close();
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                System.Threading.Thread.Sleep(1);
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                FlexCell.Range oRange = fGridDetail.Range(1, 1, fGridDetail.Rows - 1, fGridDetail.Cols - 1);
                oRange.Locked = true;
                fGridDetail.BackColor1 = System.Drawing.SystemColors.ButtonFace;
                fGridDetail.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
                oRange.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
                oRange.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
                fGridDetail.AutoRedraw = true;
                fGridDetail.Refresh();
            }
        }
        public void btnExcel_Click(object sender, EventArgs e)
        {

            this.fGridDetail.ExportToExcel("在建项目料具使用情况表[{0}]", true, true, true);
           
        }
        public void IntialHead()
        {
            fGridDetail.DefaultRowHeight = 20;
            FlexCell.Range oRange = null;
            FlexCell.Cell oCell = null;
            fGridDetail.Rows = 1;
            fGridDetail.Cols = 1;
            fGridDetail.Rows = 6;
            fGridDetail.Cols = 16;
            fGridDetail.FixedRows = 5;
            oRange = fGridDetail.Range(1, 1, fGridDetail.Rows - 1, fGridDetail.Cols - 1); oRange.ClearAll(); oRange.Locked = false;
            oRange = fGridDetail.Range(1, 1, 1, fGridDetail.Cols - 1);
            oRange.Merge(); oRange.FontSize = 16;
            SetValue(1, 1, "在建项目料具使用情况表", FlexCell.AlignmentEnum.CenterCenter, true); fGridDetail.Row(1).Height = 40;
            oRange = fGridDetail.Range(2, 1, fGridDetail.Rows - 1, fGridDetail.Cols - 1); oRange.ClearText();
            oRange = fGridDetail.Range(2, 1, 2, fGridDetail.Cols - 1);
            oRange.Merge();
           SetValue(2,1, string.Format("日期:{0}年{1}", cmbYear.Text, cmbQuarter.Text),FlexCell.AlignmentEnum.LeftCenter);
            oRange = fGridDetail.Range(3, 1, 4, 1); oRange.Merge();
            SetValue(3, 1, "序号", FlexCell.AlignmentEnum.CenterCenter);
            oRange = fGridDetail.Range(3, 2, 4, 2); oRange.Merge();
            SetValue(3, 2, "项目名称", FlexCell.AlignmentEnum.CenterCenter);
            oRange = fGridDetail.Range(3, 3, 4, 3); oRange.Merge();
            SetValue(3, 3, "工期 至 自", FlexCell.AlignmentEnum.CenterCenter);
            oRange = fGridDetail.Range(3, 4, 4, 4); oRange.Merge();
            SetValue(3, 4, "料具类型", FlexCell.AlignmentEnum.CenterCenter);
            oRange = fGridDetail.Range(3, 5, 4, 5); oRange.Merge();
            SetValue(3, 5, "时长(月)", FlexCell.AlignmentEnum.CenterCenter);
            oRange = fGridDetail.Range(3, 6, 3, 13); oRange.Merge();
            SetValue(3, 6, "料具使用情况", FlexCell.AlignmentEnum.CenterCenter);
            SetValue(3, 14, "超标情况", FlexCell.AlignmentEnum.CenterCenter);
            oRange = fGridDetail.Range(3, 15, 4, 15); oRange.Merge();
            SetValue(3, 15, "备注", FlexCell.AlignmentEnum.CenterCenter);
            SetValue(4, 6, "使用量", FlexCell.AlignmentEnum.CenterCenter);
            SetValue(4, 7, "", FlexCell.AlignmentEnum.CenterCenter);
            SetValue(4, 8, "报损量", FlexCell.AlignmentEnum.CenterCenter);
            SetValue(4, 9, "报废量", FlexCell.AlignmentEnum.CenterCenter);
            SetValue(4, 10, "消耗量", FlexCell.AlignmentEnum.CenterCenter);
            SetValue(4, 11, "", FlexCell.AlignmentEnum.CenterCenter);
            SetValue(4, 12, "允许值", FlexCell.AlignmentEnum.CenterCenter);
            SetValue(4, 13, "年损耗率", FlexCell.AlignmentEnum.CenterCenter);
            oRange = fGridDetail.Range(3, 14, 4, 14); oRange.Merge();
            
            #region 列宽度
            fGridDetail.Column(1).Width = 50;
            fGridDetail.Column(2).Width = 150;
            fGridDetail.Column(3).Width = 150;
            fGridDetail.Column(4).Width = 80;
            fGridDetail.Column(5).Width = 80;
            fGridDetail.Column(6).Width = 80;
            fGridDetail.Column(7).Width = 80;
            fGridDetail.Column(8).Width = 80;
            fGridDetail.Column(9).Width = 80;
            fGridDetail.Column(10).Width = 80;
            fGridDetail.Column(11).Width = 80;
            fGridDetail.Column(12).Width = 80;
            fGridDetail.Column(13).Width = 80;
            fGridDetail.Column(14).Width = 80;
            fGridDetail.Column(15).Width = 80;
            oRange = fGridDetail.Range(1, 1, fGridDetail.Rows - 1, fGridDetail.Cols - 1);
            oRange.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            oRange.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            #endregion
        }
        public void FillHead()
        {
           
        }
        public void FillData(List<Data> lstData)
        {
            int iRow = 0;
            int iNum = 1;
            int iStart=0;
            string sProjectName = string.Empty;
            FlexCell.Range oRange = null;
            if (lstData != null)
            {
                foreach (Data oData in lstData.OrderBy(a=>a.ProjectName).ThenBy(a=>a.MinDate))
                {
                    fGridDetail.InsertRow(fGridDetail.Rows - 1, 1);
                  iRow = fGridDetail.Rows - 2;
                  if (oData.ProjectName != sProjectName)
                  {
                      if (iStart != iRow)
                      {
                          if (iStart != 0)
                          {
                              oRange = fGridDetail.Range(iStart, 1, iRow - 1, 1);
                              oRange.Merge();
                              oRange = fGridDetail.Range(iStart, 2, iRow - 1, 2);
                              oRange.Merge();
                             
                          }
                           iStart = iRow;
                              SetValue(iRow, 1, (iNum++).ToString());
                              SetValue(iRow, 2, oData.ProjectName);
                              sProjectName=oData.ProjectName;
                      }
                  }
                  SetValue(iRow, 3, oData.BeginToEnd);
                  SetValue(iRow, 4, oData.MaterialType);
                  SetValue(iRow, 5, oData.Month.ToString());
                  SetValue(iRow, 6, oData.EnterQuantity.ToString("N2"));
                  SetValue(iRow, 8, oData.LossQty.ToString("N2"));
                  SetValue(iRow, 9, oData.RejectQuantity.ToString("N2"));
                  SetValue(iRow, 10, oData.ConsumeQuantity.ToString("N2"));
                  SetValue(iRow, 12, oData.NeedRate.ToString()+"%");
                  SetValue(iRow, 13, oData.LossConsume.ToString() + "%");
                  SetValue(iRow, 14, oData.UpperRate.ToString() + "%");
                  SetValue(iRow, 15,oData. IsUpper );
                    fGridDetail.Row(iRow).AutoFit();
                }
                if (iStart != iRow)
                {
                    oRange = fGridDetail.Range(iStart, 1, iRow , 1);
                    oRange.Merge();
                    oRange = fGridDetail.Range(iStart, 2, iRow , 2);
                    oRange.Merge();
                }
            }
        }
        public void FillFoot()
        {
            int iCol = 0,iRow = 0;
            string sText = string.Empty;
            #region 备注
            fGridDetail.InsertRow(fGridDetail.Rows - 1, 1);
            iRow = fGridDetail.Rows - 2;
            FlexCell.Range oRange = fGridDetail.Range(iRow, 1, iRow, fGridDetail.Cols - 1); oRange.Merge();
            sText = "备注：表格中允许值为公司规定的年损耗值，年损耗率为（报损量+报废量）/使用量*100%/12*时长，报损量属于项目丢失、掩埋的数量；报废量属于项目退还料具时，料具站判断为报废的数量；消耗量是项目用于施工材料的数量。";
            iCol = 1; SetValue(iRow, iCol, sText, FlexCell.AlignmentEnum.LeftCenter);
            #endregion
            #region 
            fGridDetail.InsertRow(fGridDetail.Rows - 1, 1);
            iRow = fGridDetail.Rows - 2;
             oRange = fGridDetail.Range(iRow, 1, iRow, fGridDetail.Cols - 1); oRange.Merge();
             fGridDetail.Row(iRow).Height = 30;
            sText = "单位领导：                      分管领导：                        部门：                          制表：             ";
            iCol = 1; SetValue(iRow, iCol, sText, FlexCell.AlignmentEnum.LeftCenter);
            #endregion
        }
        public void SetValue(int iRow, int iCol, string sText, FlexCell.AlignmentEnum AlignmentEnum, bool isBold)
        {
            FlexCell.Cell oCell = fGridDetail.Cell(iRow, iCol);
            oCell.Text = sText;
            oCell.Alignment = AlignmentEnum;
            oCell.FontBold = isBold;
        }
        public void SetValue(int iRow, int iCol, string sText, FlexCell.AlignmentEnum AlignmentEnum)
        {
            FlexCell.Cell oCell = fGridDetail.Cell(iRow, iCol);
            oCell.Text = sText;
            oCell.Alignment = AlignmentEnum;
            oCell.FontBold = false;
        }
        public void SetValue(int iRow, int iCol, string sText)
        {
            FlexCell.Cell oCell = fGridDetail.Cell(iRow, iCol);
            oCell.Text = sText;
            oCell.FontBold = false;
        }
        public void ShowMessage(string sMessage)
        {
            MessageBox.Show(sMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public List<Data> GetDate()
        {
            List<Data> lstData = null;
            int iYear = ClientUtil.ToInt(cmbYear.Text);
            int iMonth =  (  cmbQuarter.SelectedIndex+1)*3;
            DataSet ds = model.MaterialHireMngSvr.GetMaterialHireBuilding(iYear, iMonth);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                lstData = ds.Tables[0].Rows.OfType<DataRow>().Select(row => new Data()
                {
                   // projectname, BEGINDATE
                    // tt.projectid,tt.materialname,  maxDate, minDate, enterQty ,exitQty,
                    // LOSSQTY,REJECTQUANTITY,CONSUMEQUANTITY
                    ProjectName = ClientUtil.ToString(row["projectname"]),
                    //StartDate = ClientUtil.ToDateTime(row["BEGINDATE"]),
                    MaterialType = ClientUtil.ToString(row["materialname"]),
                    MaxDate = ClientUtil.ToDateTime(row["maxDate"]),
                    MinDate = ClientUtil.ToDateTime(row["minDate"]),
                    EnterQuantity = ClientUtil.ToDecimal(row["enterQty"]),
                    ExitQuantity = ClientUtil.ToDecimal(row["exitQty"]),
                     LossQty = ClientUtil.ToDecimal(row["LOSSQTY"]),
                    RejectQuantity = ClientUtil.ToDecimal(row["REJECTQUANTITY"]),
                    ConsumeQuantity = ClientUtil.ToDecimal(row["CONSUMEQUANTITY"])
                }).ToList();
            }
            return lstData;
        }
        public class Data
        {
            /// <summary> 项目名称 </summary>
            public string ProjectName { get; set; }
            /// <summary> 项目开始 </summary>
            public DateTime StartDate { get; set; }
            /// <summary> 项目结束 </summary>
            public DateTime EndDate { get; set; }
            public DateTime MaxDate { get; set; }
            public DateTime MinDate { get; set; }
            /// <summary> 工期 </summary>
            public string BeginToEnd
            {
                get { return string.Format("{0}-{1}", MinDate.ToString("yyyy-MM-dd"), 
                    (ExitQuantity+EnterQuantity==0?MaxDate:DateTime.Now).ToString("yyyy-MM-dd")); }
            }
            public string MaterialType { get; set; }
            /// <summary>时长</summary>
            public int Month
            {
                get
                {
                    int iMonth = 0;
                    DateTime dEnd;
                    if (ExitQuantity + EnterQuantity == 0) dEnd = MaxDate;
                    else dEnd = DateTime.Now;
                    return (dEnd.Year - MinDate.Year) * 12 + (dEnd.Month - MinDate.Month);
                }
            }
            /// <summary>使用量</summary>
            public decimal EnterQuantity { get; set; }
            /// <summary>使用量</summary>
            public decimal ExitQuantity { get; set; }
            /// <summary> 报损量 </summary>
            public decimal LossQty { get; set; }
            /// <summary> 报废量 </summary>
            public decimal RejectQuantity { get; set; }
            /// <summary> 消耗量 </summary>
            public decimal ConsumeQuantity{ get; set; }
            /// <summary> 允许值 </summary>
            public decimal NeedRate
            {
                get
                {
                    decimal rate = 0;
                    if (this.MaterialType == "3.25钢管(米)") rate = 1;
                    else if (this.MaterialType == "3.0钢管(米)") rate = 1;
                    else if (this.MaterialType == "快拆头") rate = 1;
                    else if (this.MaterialType == "扣件") rate = 2;
                    else if (this.MaterialType == "碗扣(米)") rate = 1;
                    return rate;
                }
            }
            /// <summary> 年损耗率 年损耗率为（报损量+报废量）/使用量*100%/12*时长 </summary>
            public decimal LossConsume { get { return EnterQuantity == 0 ? 0 : Math.Round(((LossQty + RejectQuantity)*100*Month) / (EnterQuantity*12) ,2); } }
            /// <summary> 超标情况 </summary>
            public decimal UpperRate { get { return LossConsume - NeedRate; } }
            /// <summary> 备注 </summary>
            public string IsUpper
            {
                get { return UpperRate > 0 ? "超标" : "未超标"; }
            }
        }
    }
    
}

