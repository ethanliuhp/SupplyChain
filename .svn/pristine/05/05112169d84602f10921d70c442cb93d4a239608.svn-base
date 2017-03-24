using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report
{
    public partial class VMaterialHireReport : TBasicDataView
    {
        MMaterialHireMng model = new MMaterialHireMng();
        EnumMatHireType matHireType;
        MatHireOrderMaster OrderMaster = null;
        public VMaterialHireReport()
        {
            InitializeComponent();
            IntialData();
            IntialEvent();
        }
        public void IntialData()
        {
            txtMaterial.materialCatCode = CommonUtil.TurnStationMaterialMatCode;
            txtMaterial.IsCheckBox = false;
            IntialHead();
        }
        public void IntialEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            btnForward.Click+=new EventHandler(btnForward_Click);
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
        public void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtpDateBegin.Value > dtpDateEnd.Value)
                {
                    ShowMessage("时间范围错误:[开始时间]应该小于[结束时间]");
                    return;
                }
                if (txtContract.Tag == null)
                {
                    ShowMessage("请选择租赁方");
                    return;
                }
                if (txtMaterial.Result == null || txtMaterial.Result.Count == 0)
                {
                    ShowMessage("请选物资");
                    return;
                }
                FlashScreen.Show("正在生成[料具租赁台账]报告...");
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
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ex) );
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
            Material oMaterial = (txtMaterial.Result == null || txtMaterial.Result.Count == 0 ? null : txtMaterial.Result[0] as Material);
            if (oMaterial!=null)
            {
                this.fGridDetail.ExportToExcel(string.Format("料具租赁台账[{0}]", oMaterial.Name), true , true, true);
            }
        }
        public void IntialHead()
        {
            FlexCell.Range oRange = null;
            fGridDetail.Rows = 6;
            fGridDetail.Cols = 12;
            oRange = fGridDetail.Range(1, 1, fGridDetail.Rows-1, fGridDetail.Cols - 1);
            oRange.Locked = false;
            SetValue(1, 1, "租赁单位名称:", FlexCell.AlignmentEnum.RightCenter, true);
            oRange = fGridDetail.Range(1, 2, 1,6);
            oRange.Merge();
            SetValue(1, 7, "统计时间:", FlexCell.AlignmentEnum.RightCenter, true);
            oRange = fGridDetail.Range(1, 8, 1, fGridDetail.Cols-1);
            oRange.Merge();
            SetValue(2, 1, "材料编号:", FlexCell.AlignmentEnum.RightCenter, true);
            oRange = fGridDetail.Range(2, 2, 2, 4);
            oRange.Merge();
            SetValue(2, 5, "材料名称:", FlexCell.AlignmentEnum.RightCenter, true);
            SetValue(2, 7, "规格:", FlexCell.AlignmentEnum.RightCenter, true);
            SetValue(2, 9, "单位:", FlexCell.AlignmentEnum.RightCenter, true);
            oRange = fGridDetail.Range(3, 1, 4, 1); oRange.Merge();
            SetValue(3, 1, "时间", FlexCell.AlignmentEnum.CenterCenter, true);
            oRange = fGridDetail.Range(3, 2, 4, 2); oRange.Merge();
            SetValue(3, 2, "单据编号", FlexCell.AlignmentEnum.CenterCenter, true);
            oRange = fGridDetail.Range(3, 3, 4, 3); oRange.Merge();
            SetValue(3, 3, "型号", FlexCell.AlignmentEnum.CenterCenter, true);
            oRange = fGridDetail.Range(3, 4, 4, 4); oRange.Merge();
            SetValue(3, 4, "长度", FlexCell.AlignmentEnum.CenterCenter, true);
            oRange = fGridDetail.Range(3, 5, 4, 5); oRange.Merge();
            SetValue(3, 5, "进场", FlexCell.AlignmentEnum.CenterCenter, true);
            //时间 单据编号 型号 长度 完好 维修 切头 报废 报损 消耗
            oRange = fGridDetail.Range(3, 6, 3, 11); oRange.Merge();
            SetValue(3, 6, "发出退料", FlexCell.AlignmentEnum.CenterCenter, true);
            SetValue(4, 6, "完好", FlexCell.AlignmentEnum.CenterCenter, true);
            SetValue(4, 7, "维修", FlexCell.AlignmentEnum.CenterCenter, true);
            SetValue(4, 8, "切头", FlexCell.AlignmentEnum.CenterCenter, true);
            SetValue(4, 9, "报废", FlexCell.AlignmentEnum.CenterCenter, true);
            SetValue(4, 10, "报损", FlexCell.AlignmentEnum.CenterCenter, true);
            SetValue(4, 11, "消耗", FlexCell.AlignmentEnum.CenterCenter, true);
           // SetValue(4, 12, "丢失", FlexCell.AlignmentEnum.CenterCenter, true);
            
            #region 列宽度
            fGridDetail.Column(1).Width = 100;
            fGridDetail.Column(2).Width = 150;
            fGridDetail.Column(3).Width = 80;
            fGridDetail.Column(4).Width = 80;
            fGridDetail.Column(5).Width = 80;
            fGridDetail.Column(6).Width = 80;
            fGridDetail.Column(7).Width = 80;
            fGridDetail.Column(8).Width = 80;
            fGridDetail.Column(9).Width = 80;
            fGridDetail.Column(10).Width = 80;
            fGridDetail.Column(11).Width = 80;
           // fGridDetail.Column(12).Width = 80;
            #endregion
            oRange = fGridDetail.Range(1, 1, fGridDetail.Rows - 1, fGridDetail.Cols - 1);
            oRange.set_Borders(FlexCell.EdgeEnum.Inside , FlexCell.LineStyleEnum.Thin);
            oRange.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
        }
        public void FillHead()
        {
            Material oMaterial = txtMaterial.Result[0] as Material;
            SetValue(1, 2, OrderMaster.ProjectName, FlexCell.AlignmentEnum.LeftCenter);
            SetValue(1, 8, string.Format("{0}至{1}", dtpDateBegin.Value.ToString("yyyy-MM-dd"), this.dtpDateEnd.Value.ToString("yyyy-MM-dd")), FlexCell.AlignmentEnum.LeftCenter);
            SetValue(2, 2, oMaterial.Code, FlexCell.AlignmentEnum.LeftCenter);
            SetValue(2, 6, oMaterial.Name, FlexCell.AlignmentEnum.LeftCenter);
            SetValue(2, 8, oMaterial.Specification, FlexCell.AlignmentEnum.LeftCenter);
            SetValue(2, 10, oMaterial.BasicUnit.Name, FlexCell.AlignmentEnum.LeftCenter);
            if (this.matHireType == EnumMatHireType.普通料具)
            {
                fGridDetail.Column(3).Visible = false;
                fGridDetail.Column(4).Visible = false;
            }
            else if (this.matHireType == EnumMatHireType.钢管)
            {
                fGridDetail.Column(3).Visible = false;
                fGridDetail.Column(4).Visible = true;
            }
            else if (this.matHireType == EnumMatHireType.碗扣)
            {
                fGridDetail.Column(3).Visible = true;
                fGridDetail.Column(4).Visible = false;
            }
        }
        public void FillData(List<Data> lstData)
        {
            int iRow = 0;
            if (lstData != null)
            {//时间 单据编号 型号 长度 进场 完好 维修 切头 报废 报损 消耗 丢失
                foreach (Data oData in lstData)
                {
                  fGridDetail.InsertRow(fGridDetail.Rows-1,1);
                  iRow = fGridDetail.Rows - 2;
                    SetValue(iRow, 1, oData.CreateDate.ToString("yyyy-MM-dd"), FlexCell.AlignmentEnum.RightCenter);
                    SetValue(iRow, 2, oData.Code, FlexCell.AlignmentEnum.RightCenter);//单据编号
                    if (this.matHireType == EnumMatHireType.钢管)
                    {
                        SetValue(iRow, 4, oData.MaterialLength.ToString(), FlexCell.AlignmentEnum.RightCenter);//长度
                    }
                    else if (this.matHireType == EnumMatHireType.碗扣)
                    {
                        SetValue(iRow, 3, oData.MaterialType, FlexCell.AlignmentEnum.RightCenter);//型号
                    }
                    if (oData.BillType == 1)
                    {
                        SetValue(iRow, 5, oData.EnterQuantity.ToString("N2"), FlexCell.AlignmentEnum.RightCenter);
                    }
                    else
                    {//完好 维修 切头 报废 报损 消耗
                        SetValue(iRow, 6, oData.BroachQuantity.ToString("N2"), FlexCell.AlignmentEnum.RightCenter);
                        SetValue(iRow, 7, oData.RepairQty.ToString("N2"), FlexCell.AlignmentEnum.RightCenter);
                        SetValue(iRow, 8, oData.DisCardQty.ToString("N2"), FlexCell.AlignmentEnum.RightCenter);
                        SetValue(iRow, 9, oData.RejectQuantity.ToString("N2"), FlexCell.AlignmentEnum.RightCenter);
                        SetValue(iRow, 10, oData.LossQty.ToString("N2"), FlexCell.AlignmentEnum.RightCenter);
                        SetValue(iRow, 11, oData.ConsumeQuantity.ToString("N2"), FlexCell.AlignmentEnum.RightCenter);
                       // SetValue(iRow, 12, oData.LoseQuantity, FlexCell.AlignmentEnum.RightCenter);
                    }
                   
                    fGridDetail.Row(iRow).AutoFit();
                }
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
        public void ShowMessage(string sMessage)
        {
            MessageBox.Show(sMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public List<Data> GetDate()
        {
            List<Data> lstData = null;
            //this.IntialHead();
            //FillHead();
            Material oMaterial=txtMaterial.Result[0] as Material;
            matHireType=oMaterial.Code.StartsWith(CommonUtil.MaterialCateGGCode)?EnumMatHireType.钢管:
                (oMaterial.Code.StartsWith(CommonUtil.MaterialCateWKCode) ? EnumMatHireType.碗扣 : EnumMatHireType.普通料具);
            DataSet ds = model.MaterialHireMngSvr.GetMaterialHireReport(
                dtpDateBegin.Value,
                dtpDateEnd.Value,
                OrderMaster.TheSupplierRelationInfo.Id,
               OrderMaster.ProjectId,
                oMaterial.Id
                );
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                lstData = ds.Tables[0].Rows.OfType<DataRow>().Select(row => new Data()
                {
                    BillType = ClientUtil.ToInt(row["BillType"]),
                    CreateDate = ClientUtil.ToDateTime(row["createdate"]),
                    Code = ClientUtil.ToString(row["code"]),
                    MaterialType = ClientUtil.ToString(row["materialtype"]),
                    MaterialLength = ClientUtil.ToDecimal(row["materiallength"]),
                    EnterQuantity = ClientUtil.ToDecimal(row["Quantity"]),
                    ExitQuantity = ClientUtil.ToDecimal(row["ExitQuantity"]),
                    RejectQuantity = ClientUtil.ToDecimal(row["RejectQuantity"]),
                    LoseQuantity = ClientUtil.ToDecimal(row["LoseQuantity"]),
                    BroachQuantity = ClientUtil.ToDecimal(row["BroachQuantity"]),
                    ConsumeQuantity = ClientUtil.ToDecimal(row["ConsumeQuantity"]),
                    DisCardQty = ClientUtil.ToDecimal(row["DisCardQty"]),
                    RepairQty = ClientUtil.ToDecimal(row["RepairQty"]),
                    LossQty = ClientUtil.ToDecimal(row["LossQty"])
                }).ToList();
            }
            return lstData;
        }
        public class Data
        {
            public int BillType { get; set; }
            public DateTime CreateDate { get; set; }
            public string Code { get; set; }
            public string MaterialType { get; set; }
            public decimal  MaterialLength { get; set; }
            /// <summary>进场数量 </summary>
            public decimal EnterQuantity { get; set; }
            /// <summary>退料数量 </summary>
            public decimal ExitQuantity { get; set; }
            /// <summary>报废数量 </summary>
            public decimal RejectQuantity { get; set; }
            /// <summary>丢失数量 </summary>
            public decimal LoseQuantity { get; set; }
            /// <summary>完好数量 </summary>
            public decimal BroachQuantity { get; set; }
            /// <summary>消耗数量 </summary>
            public decimal ConsumeQuantity { get; set; }
            /// <summary>切头数量 </summary>
            public decimal DisCardQty { get; set; }
            /// <summary>维修数量 </summary>
            public decimal RepairQty { get; set; }
            /// <summary>报损数量 </summary>
            public decimal LossQty { get; set; }


        }

        

        
    }
    
}
