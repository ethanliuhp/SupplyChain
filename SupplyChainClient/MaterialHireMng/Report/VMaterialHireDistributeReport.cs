using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireBalance.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report
{
    public partial class VMaterialHireDistributeReport : TBasicDataView
    {
        MatHireOrderMaster OrderMaster = null;
        MMaterialHireMng model = new MMaterialHireMng();
        List<decimal> lstSumQty = null;
        List<decimal> lstStockQty = null;
        List<ColumnInfo> lstMaterialInfo = new List<ColumnInfo>();
        public VMaterialHireDistributeReport()
        {
            InitializeComponent();
            IntialData();
            IntialEvent();
        }
        public void IntialData()
        {
           // dtpDateBegin.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
 
            //IntialHead();
            //FillFoot();
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
                FlashScreen.Show("正在生成[料具分布报表]报告...");
                fGridDetail.AutoRedraw = false;
               
                List<Data> lstData = GetDate();
                if (lstData != null && lstData.Count > 0 && lstMaterialInfo != null && lstMaterialInfo.Count > 0)
                {
                    IntialHead();
                    FillData(lstData);
                    FillFoot();
                }
                else
                {
                    ShowMessage("该时间区间内没有找到对应的进退料信息");
                }
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
            if (this.fGridDetail.Rows > 5)
            {
                this.fGridDetail.ExportToExcel(string.Format("料具分布报表[{0}]", dtpDateBegin.Value.ToString("yyyy-MM-dd")), true, true, true);
            }
        }
        public void IntialHead()
        {
            int iRow = 1, iCol = 1;
            fGridDetail.DefaultRowHeight = 25;
            FlexCell.Range oRange = null;
            FlexCell.Cell oCell = null;
            fGridDetail.Rows = 1;
            fGridDetail.Cols = 1;
            fGridDetail.Rows = 5;
            fGridDetail.Cols = 2+lstMaterialInfo.Count;//
            fGridDetail.FixedRows = 4;
            fGridDetail.Column(0).Visible = false;
            oRange = fGridDetail.Range(iRow, iCol, fGridDetail.Rows - 1, fGridDetail.Cols - 1); oRange.ClearText(); oRange.ClearAll(); oRange.Locked = false;
            oRange = fGridDetail.Range(iRow, iCol, iRow, fGridDetail.Cols - 1); oRange.Merge();
            fGridDetail.Row(iRow).Height = 40;
            oCell = fGridDetail.Cell(iRow, iCol); oCell.FontSize = 18; SetValue(iRow, iCol, "料具支撑体系分布表", FlexCell.AlignmentEnum.CenterCenter, true);
            iRow+=1;
            iCol = 1; SetValue(iRow, iCol, "日期:", FlexCell.AlignmentEnum.RightCenter, true);
            iCol += 1; oRange = fGridDetail.Range(iRow, iCol, iRow, fGridDetail.Cols - 1); oRange.Merge(); oRange.Alignment = FlexCell.AlignmentEnum.LeftCenter;
            SetValue(iRow,iCol,dtpDateBegin.Value.ToString("yyyy年MM月dd日"),FlexCell.AlignmentEnum.LeftCenter);
            iRow += 1;
            iCol = 1; SetValue(iRow, iCol, "项目", FlexCell.AlignmentEnum.CenterCenter, true);
            foreach (ColumnInfo oItem in lstMaterialInfo)
            {
                iCol += 1; SetValue(iRow, iCol, oItem.ColumnsName, FlexCell.AlignmentEnum.CenterCenter, true);
                oItem.ColumnIndex = iCol;
                fGridDetail.Column(iCol).AutoFit();
            }
         
        }
        public void FillData(List<Data> lstData)
        {
            int iCol = 0;
            int iRow = 0;
            FlexCell.Range oRange = null;
            List<string> lstProjectName = null;
            IEnumerable<Data> lstDataTemp=null;
            //Data oData = null;
            ColumnInfo oColumnInfo = null;
            lstSumQty = new List<decimal>();
            for(int i=0;i<fGridDetail.Cols;i++)
            {
                lstSumQty.Insert(i, 0);
            }
            if (lstData != null && lstData.Count > 0)
            {
                 
                lstProjectName = lstData.Select(a => a.ProjectName).Distinct().ToList();
                foreach (string sProjectName in lstProjectName)
                {
                    fGridDetail.InsertRow(fGridDetail.Rows-1,1);
                    iRow=fGridDetail.Rows-2;
                    iCol=1;SetValue(iRow,iCol,sProjectName);
                    lstDataTemp=lstData.Where(a=>a.ProjectName==sProjectName);
                    foreach (Data oData in lstDataTemp)
                    {
                        oColumnInfo=lstMaterialInfo.FirstOrDefault(a => a.MaterialCode == oData.MaterialCode);
                        if (oColumnInfo != null)
                        {
                            lstSumQty[oColumnInfo.ColumnIndex] += oData.Quanity;
                            SetValue(iRow, oColumnInfo.ColumnIndex, GetValue(oData.Quanity, 2), FlexCell.AlignmentEnum.RightCenter);
                        }
                    }
                    //foreach (ColumnInfo oItem in lstMaterialInfo)
                    //{
                    //    iCol += 1;
                    //    oData = lstDataTemp.FirstOrDefault(a => a.MaterialCode == oItem.MaterialCode);
                    //    if (oData != null)
                    //    {
                    //        lstSumQty[iCol] += oData.Quanity;
                    //        SetValue(iRow, iCol, GetValue(oData.Quanity, 2),FlexCell.AlignmentEnum.RightCenter);
                    //    }
                    //}
                }
                #region 在用
                fGridDetail.InsertRow(fGridDetail.Rows - 1, 1);
                iRow = fGridDetail.Rows - 2;
                iCol = 1; SetValue(iRow, iCol, "在用", FlexCell.AlignmentEnum.CenterCenter);
                if (lstSumQty != null)
                {
                    for (iCol = 2; iCol < fGridDetail.Cols; iCol++)
                    {
                        SetValue(iRow, iCol, GetValue(lstSumQty[iCol],2), FlexCell.AlignmentEnum.RightCenter);
                    }
                }
                #endregion
                #region 库存
                fGridDetail.InsertRow(fGridDetail.Rows - 1, 1);
                iRow = fGridDetail.Rows - 2;
                iCol = 1; SetValue(iRow, iCol, "库存", FlexCell.AlignmentEnum.CenterCenter);
                if (lstStockQty != null)
                {
                    for (iCol = 2; iCol < fGridDetail.Cols; iCol++)
                    {
                        SetValue(iRow, iCol, GetValue(lstStockQty[iCol],2), FlexCell.AlignmentEnum.RightCenter);
                    }
                }
                #endregion

            }
        }
        public void FillFoot()
        {
            int iRow = 0, iCol = 0;
            string sText = "主管领导:                    分管领导：                     财务:                  经营管理部：             制表:      ";
            fGridDetail.InsertRow(fGridDetail.Rows - 1, 1);
            iRow = fGridDetail.Rows - 2;
            FlexCell.Range oRange = fGridDetail.Range(iRow, 1, iRow, fGridDetail.Cols-1); oRange.Merge();
            iCol = 1; SetValue(iRow, iCol, sText, FlexCell.AlignmentEnum.LeftCenter);

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
        public string GetValue(decimal dValue, int iDigit)
        {
            if (dValue == 0)
            {
                return "";
            }
            else
            {
                return dValue.ToString(string.Format("N{0}", iDigit));
            }
        }
        public void ShowMessage(string sMessage)
        {
            MessageBox.Show(sMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public List<Data> GetDate()
        {
            List<Data> lstData;
            Hashtable hs = new Hashtable();
            //string sMaterialCodes =string.Join(",", lstMaterialInfo.Select(a=>string.Format("'{0}'",a.Descript)).ToArray());
            //if (string.IsNullOrEmpty(sMaterialCodes))
            //{
            //    ShowMessage("后台没配置需要查询显示的列");
            //    return null;
            //}
            //t.projectid,t.projectname,t.material,t.materialcode,MATERIALNAME,MATERIALSPEC,MATSTANDARDUNITNAME,
            //sum(leftquantity) qty
            DataSet ds = model.MaterialHireMngSvr.QueryMaterialDistributeReport(dtpDateBegin.Value);
            lstData = ds.Tables[0].Rows.OfType<DataRow>().Select(row => new Data {
                Material = ClientUtil.ToString(row["material"]),
                MaterialCode = ClientUtil.ToString(row["materialcode"]),
                MaterialName = ClientUtil.ToString(row["MATERIALNAME"]),
                MaterialSpec = ClientUtil.ToString(row["MATERIALSPEC"]),
                MaterialUnitName = ClientUtil.ToString(row["MATSTANDARDUNITNAME"]),
                ProjectName = ClientUtil.ToString(row["projectname"]),
                Quanity = ClientUtil.ToDecimal(row["qty"])
            }).ToList();
            lstMaterialInfo.Clear();
            foreach (Data data in lstData.OrderBy(A=>A.MaterialCode))
            {
                if (!hs.ContainsKey(data.Material))
                {
                    hs.Add(data.Material, null);
                    lstMaterialInfo.Add(new ColumnInfo()
                    {
                        Material = data.Material,
                        MaterialCode = data.MaterialCode,
                        MaterialName = data.MaterialName,
                        MaterialSpec = data.MaterialSpec,
                        MaterialUnitName = data.MaterialUnitName
                    });
                }
            }
            return lstData;
        }
        public class Data
        {
            /// <summary> 项目名称 </summary>
            public string ProjectName { get; set; }
            /// <summary> 编码 </summary>
            public string MaterialCode { get; set; }
            public string MaterialName { get; set; }
            public string Material { get; set; }
            public string MaterialSpec { get; set; }
            public string MaterialUnitName { get; set; }
            /// <summary>
            /// 账面余量
            /// </summary>
            public decimal Quanity { get; set; }

        }
        public class ColumnInfo
        {
            public string Material;
            public string MaterialName;
            public string MaterialUnitName;
            public string MaterialSpec;
            public string MaterialCode;
            public string ColumnsName{get{return string.Format("{0}{1}({2})",MaterialName,MaterialSpec,MaterialUnitName);}}
            public int ColumnIndex;
        }

    }
    
}

