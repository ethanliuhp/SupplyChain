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
    public partial class VMaterialHireSizeReport : TBasicDataView
    {
        MatHireOrderMaster OrderMaster = null;
        MMaterialHireMng model = new MMaterialHireMng();
        IList lstMaterialSize = VBasicDataOptr.GetStationMaterialSize();
        public VMaterialHireSizeReport()
        {
            InitializeComponent();
            IntialData();
            IntialEvent();
        }
        public void IntialData()
        {
            dtpDateBegin.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpDateEnd.Value = DateTime.Now;
            cmbMaterial.Items.Clear();
            foreach (BasicDataOptr oItem in lstMaterialSize)
            {
                cmbMaterial.Items.Add(oItem.BasicName);
            }
            IntialHead();
        }
       
        public void IntialEvent()
        {
            btnForward.Click+=new EventHandler(btnForward_Click);
            dtpDateBegin.ValueChanged +=new EventHandler(dtpDate_ValueChanged);
            dtpDateEnd.ValueChanged += new EventHandler(dtpDate_ValueChanged);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
        }
        void btnForward_Click(object sender, EventArgs e)
        {

            VMaterialHireOrderSelector oVMaterialHireOrderSelector = new VMaterialHireOrderSelector(EnumMatHireType.其他);
            oVMaterialHireOrderSelector.ShowDialog();
            if (oVMaterialHireOrderSelector.Result != null && oVMaterialHireOrderSelector.Result.Count > 0)
            {
                OrderMaster = oVMaterialHireOrderSelector.Result[0] as MatHireOrderMaster;
                #region 数据填充
                txtSupply.Text = OrderMaster.SupplierName;
                txtSupply.Tag = OrderMaster.TheSupplierRelationInfo;
                txtContract.Tag = OrderMaster;
                txtContract.Text = OrderMaster.Code;
                txtProjectName.Text = OrderMaster.ProjectName;
                txtProjectName.Tag = OrderMaster.ProjectId;
                #endregion
            }
        }
        public void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            if (dtpDateEnd.Value < dtpDateBegin.Value)
            {
                dtpDateBegin.Value = dtpDateEnd.Value.AddDays(1);
            }
        }
        public void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {

                FlashScreen.Show("正在生成[尺寸分段统计表]报告...");
                fGridDetail.AutoRedraw = false;
                IntialHead();
                FillHead();
                FillData(GetDate());
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
            if (this.fGridDetail.Rows > 6)
            {
                this.fGridDetail.ExportToExcel(string.Format("尺寸分段统计表[{0}-{1}]", txtProjectName.Text, cmbMaterial.Text), true, true, true);
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
            fGridDetail.Cols = 7;
            fGridDetail.FixedRows = 4;
            fGridDetail.Column(0).Visible = false; fGridDetail.Row(0).Visible = false;
            oRange = fGridDetail.Range(iRow, iCol, fGridDetail.Rows - 1, fGridDetail.Cols - 1); oRange.ClearText(); oRange.ClearAll(); oRange.Locked = false;
            oRange = fGridDetail.Range(iRow, iCol, iRow, fGridDetail.Cols - 1); oRange.Merge();
            fGridDetail.Row(iRow).Height = 40;
            oCell = fGridDetail.Cell(iRow, iCol); oCell.FontSize = 18; SetValue(iRow, iCol, "尺寸分段统计表", FlexCell.AlignmentEnum.CenterCenter, true);
            iRow += 1;
            iCol = 1; SetValue(iRow, iCol, "项目名称:", FlexCell.AlignmentEnum.RightCenter, true);
            iCol += 1; oRange = fGridDetail.Range(iRow, iCol, iRow, iCol + 1); oRange.Merge();
            iCol += 2; SetValue(iRow, iCol, "时间:", FlexCell.AlignmentEnum.RightCenter, true);
            iCol += 1; oRange = fGridDetail.Range(iRow, iCol, iRow, iCol + 1); oRange.Merge();
            iRow+=1;
            iCol = 1; SetValue(iRow, iCol, "序号", FlexCell.AlignmentEnum.CenterCenter, true);
            iCol += 1; SetValue(iRow, iCol, "料具名称", FlexCell.AlignmentEnum.CenterCenter, true);
            iCol += 1; SetValue(iRow, iCol,  "规格尺寸", FlexCell.AlignmentEnum.CenterCenter, true);
            iCol += 1; SetValue(iRow, iCol, "进场根数", FlexCell.AlignmentEnum.CenterCenter, true);
            iCol += 1; SetValue(iRow, iCol, "退场根数", FlexCell.AlignmentEnum.CenterCenter, true);

            #region 列宽度
            fGridDetail.Column(1).Width = 50;
            fGridDetail.Column(2).Width = 100;
            fGridDetail.Column(3).Width = 50;
            fGridDetail.Column(4).Width = 80;
            fGridDetail.Column(5).Width = 80;
            fGridDetail.Column(6).Width = 80;
            
      
            oRange = fGridDetail.Range(1, 1, fGridDetail.Rows - 1, fGridDetail.Cols - 1);
            oRange.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            oRange.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            #endregion
        }
        public void FillHead()
        {
           int  iRow =2;
           int  iCol = 2; SetValue(iRow, iCol, txtProjectName.Text, FlexCell.AlignmentEnum.LeftCenter);
           iCol += 3; SetValue(iRow, iCol, 
               string.Format("{0}-{1}", dtpDateBegin.Value.ToString("yyyy-MM-dd"), dtpDateEnd.Value.ToString("yyyy-MM-dd")), 
               FlexCell.AlignmentEnum.LeftCenter);
           iRow = 3; iCol = 3;
           if (cmbMaterial.Text.Contains("钢管"))
           {
               SetValue(iRow, iCol, "钢管长度",FlexCell.AlignmentEnum.CenterCenter,true);
           }
           else if (cmbMaterial.Text.Contains("碗扣"))
           {
               SetValue(iRow, iCol, "碗扣规格", FlexCell.AlignmentEnum.CenterCenter, true);
           }
               

        }
        public void FillData(List<Data> lstData)
        {
            int iCol = 0;
            int iRow = 0;
            int iNum = 1;
            FlexCell.Range oRange = null;
            
            if (lstData != null && lstData.Count>0)
            {//出租单位	材料名称	单位	租赁单价	进退场日期	进场数量	退场数量	天数	租赁费用	结存在租量
                foreach (Data oData in lstData)
                {
                    fGridDetail.InsertRow(fGridDetail.Rows - 1, 1);
                    iRow = fGridDetail.Rows - 2;
                    iCol = 1; SetValue(iRow, iCol, (iNum++).ToString(), FlexCell.AlignmentEnum.CenterCenter);
                    iCol += 1; SetValue(iRow, iCol, oData.MaterialName, FlexCell.AlignmentEnum.RightCenter);
                    iCol += 1; SetValue(iRow, iCol,  oData.SizeType, FlexCell.AlignmentEnum.RightCenter);
                    iCol += 1; SetValue(iRow, iCol, GetValue(oData.EnterQuanity,4), FlexCell.AlignmentEnum.RightCenter);
                    iCol += 1; SetValue(iRow, iCol, GetValue(oData.ExitQuanity, 4), FlexCell.AlignmentEnum.RightCenter);
                    fGridDetail.Row(iRow).AutoFit();
                }
               
            }
            for (int iColumn = 1; iColumn < fGridDetail.Cols; iColumn++)
            {
                fGridDetail.Column(iColumn).AutoFit();
            }
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
            
            List<Data> lstData = new List<Data> ();
            List<string> lstMatCode = null;
            DataRow oRow = null;
            decimal dLeftQty = 0;
            decimal dSumMoney = 0;
            if (OrderMaster == null)
            {
                ShowMessage("请选择租赁合同");
                this.btnForward.Focus();
                return null;
            }
            if (string.IsNullOrEmpty(this.cmbMaterial.Text))
            {
                ShowMessage("请选择需要查询物资");
                cmbMaterial.Focus();
                return null;
            }
            string sMaterialCode=lstMaterialSize.OfType<BasicDataOptr>().First(a=>a.BasicName== cmbMaterial.Text).Descript;
            DataSet ds = model.MaterialHireMngSvr.QueryMaterialSizeReport(dtpDateBegin.Value, dtpDateEnd.Value, OrderMaster.Id, sMaterialCode);
            //materialname,materialcode,materiallength,materialtype,enterQuanity,exitQuanity
            lstData = ds.Tables[0].Rows.OfType<DataRow>().Select(row => new Data() {
                MaterialName =cmbMaterial.Text,
                MaterialCode=ClientUtil.ToString(row["materialcode"]),
                Len = ClientUtil.ToDecimal(row["materiallength"]),
                TypeName = ClientUtil.ToString(row["materialtype"]),
                EnterQuanity = ClientUtil.ToDecimal(row["enterQuanity"]),
                ExitQuanity = ClientUtil.ToDecimal(row["exitQuanity"]),
            }).ToList();
            return lstData;
        }
        public class Data
        {
            public string MaterialName;
            public string MaterialCode;
            public decimal Len;
            public string TypeName;
            public decimal EnterQuanity;
            public decimal ExitQuanity;
            public string SizeType
            {
                get
                {
                    return MaterialName.Contains("钢管") ? Len.ToString("N1") : this.TypeName;
                }
            }

        }
    
    }
    
}

