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

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report
{
    public partial class VMaterialHireBalanceReport : TBasicDataView
    {
        MatHireOrderMaster OrderMaster = null;
        MMaterialHireMng model = new MMaterialHireMng();
        public VMaterialHireBalanceReport()
        {
            InitializeComponent();
            IntialData();
            IntialEvent();
        }
        public void IntialData()
        {
            dtpDateBegin.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpDateEnd.Value = DateTime.Now;
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
                
                FlashScreen.Show("正在生成[租赁结算]报告...");
                fGridDetail.AutoRedraw = false;
                IntialHead();
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

            this.fGridDetail.ExportToExcel(string.Format("租赁结算台账[{0}]", txtProjectName.Text), true, true, true);
        
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
            fGridDetail.Cols = 11;
            fGridDetail.Column(0).Visible = false;
            fGridDetail.FixedRows = 4;
            oRange = fGridDetail.Range(iRow, iCol, fGridDetail.Rows - 1, fGridDetail.Cols - 1); oRange.ClearText(); oRange.ClearAll(); oRange.Locked = false;
            oRange = fGridDetail.Range(iRow, iCol, iRow, fGridDetail.Cols - 1); oRange.Merge();
            fGridDetail.Row(iRow).Height = 40;
            oCell = fGridDetail.Cell(iRow, iCol); oCell.FontSize = 18; SetValue(iRow, iCol, "租赁结算", FlexCell.AlignmentEnum.CenterCenter, true);
            iRow+=1;
            iCol = 1; SetValue(iRow, iCol, "单位名称:", FlexCell.AlignmentEnum.RightCenter, true);
            iCol += 1; oRange = fGridDetail.Range(iRow, iCol, iRow, iCol + 2); oRange.Merge();
            SetValue(iRow, iCol, txtProjectName.Text,FlexCell.AlignmentEnum.LeftCenter);
            iCol += 3; SetValue(iRow, iCol, "统计时间:", FlexCell.AlignmentEnum.RightCenter, true);
            iCol += 1; oRange = fGridDetail.Range(iRow, iCol, iRow, fGridDetail.Cols - 1); oRange.Merge();
            SetValue(iRow, iCol, string.Format("{0}至{1}", dtpDateBegin.Value.ToString("yyyy-MM-dd"), dtpDateEnd.Value.ToString("yyyy-MM-dd")), FlexCell.AlignmentEnum.LeftCenter);
            iRow += 1;
            //出租单位	材料名称	单位	租赁单价	进退场日期	进场数量	退场数量	天数	租赁费用	结存在租量
            iCol = 1; SetValue(iRow, iCol, "出租单位", FlexCell.AlignmentEnum.CenterCenter,true);
            iCol += 1; SetValue(iRow, iCol, "材料名称", FlexCell.AlignmentEnum.CenterCenter, true);
            iCol += 1; SetValue(iRow, iCol, "单位", FlexCell.AlignmentEnum.CenterCenter, true);
            iCol += 1; SetValue(iRow, iCol, "租赁单价", FlexCell.AlignmentEnum.CenterCenter, true);
            iCol += 1; SetValue(iRow, iCol, "进退场日期", FlexCell.AlignmentEnum.CenterCenter, true);
            iCol += 1; SetValue(iRow, iCol, "进场数量", FlexCell.AlignmentEnum.CenterCenter, true);
            iCol += 1; SetValue(iRow, iCol, "退场数量", FlexCell.AlignmentEnum.CenterCenter, true);
            iCol += 1; SetValue(iRow, iCol, "天数", FlexCell.AlignmentEnum.CenterCenter, true);
            iCol += 1; SetValue(iRow, iCol, "租赁费用", FlexCell.AlignmentEnum.CenterCenter, true);
            iCol += 1; SetValue(iRow, iCol, "结存在租量", FlexCell.AlignmentEnum.CenterCenter, true);


            #region 列宽度
            fGridDetail.Column(1).Width = 150;
            fGridDetail.Column(2).Width = 100;
            fGridDetail.Column(3).Width = 50;
            fGridDetail.Column(4).Width = 80;
            fGridDetail.Column(5).Width = 80;
            fGridDetail.Column(6).Width = 80;
            fGridDetail.Column(7).Width = 80;
            fGridDetail.Column(8).Width = 80;
            fGridDetail.Column(9).Width = 80;
            fGridDetail.Column(10).Width = 80;
      
            oRange = fGridDetail.Range(1, 1, fGridDetail.Rows - 1, fGridDetail.Cols - 1);
            oRange.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            oRange.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            #endregion
        }
        public void FillData(List<MasterData> lstData)
        {
            int iCol = 0;
            int iRow = 0;
            int iNum = 1;
            int iStart=0;
            string sProjectName = string.Empty;
            FlexCell.Range oRange = null;
            
            if (lstData != null && lstData.Count>0)
            {//出租单位	材料名称	单位	租赁单价	进退场日期	进场数量	退场数量	天数	租赁费用	结存在租量
                foreach (MasterData oData in lstData)
                {
                
                  #region 结存
                    if (oData.LeftDetail != null)
                    {
                        HireDetail oHireDetail = oData.LeftDetail;
                        fGridDetail.InsertRow(fGridDetail.Rows - 1, 1);
                        iRow = fGridDetail.Rows - 2;
                        iCol = 1; SetValue(iRow, iCol, oData.ProjectName,FlexCell.AlignmentEnum.LeftCenter);//出租单位
                        iCol += 1; SetValue(iRow, iCol, oHireDetail.MaterialName, FlexCell.AlignmentEnum.LeftCenter);//	材料名称
                        iCol += 1; SetValue(iRow, iCol, oHireDetail.UnitName, FlexCell.AlignmentEnum.CenterCenter);//	单位
                        iCol += 1; SetValue(iRow, iCol, GetValue(oHireDetail.HirePrice, 4),FlexCell.AlignmentEnum.RightCenter);//租赁单价
                        iCol += 1; SetValue(iRow, iCol, oHireDetail.CreateDate.ToString("yyyy-MM-dd"), FlexCell.AlignmentEnum.RightCenter);//进退场日期
                        iCol += 1; SetValue(iRow, iCol, GetValue(oHireDetail.EnterQuanity, 4),  FlexCell.AlignmentEnum.RightCenter);//进场数量
                        iCol += 1; SetValue(iRow, iCol, GetValue(oHireDetail.ExitQuanity,4), FlexCell.AlignmentEnum.RightCenter);//退场数量
                        iCol += 1; SetValue(iRow, iCol, oHireDetail.Day.ToString(),  FlexCell.AlignmentEnum.RightCenter);//天数
                        iCol += 1; SetValue(iRow, iCol, GetValue(oHireDetail.HireMoney, 6),  FlexCell.AlignmentEnum.RightCenter);//租赁费用
                        iCol += 1; SetValue(iRow, iCol, GetValue(oHireDetail.LeftQuanity, 4), FlexCell.AlignmentEnum.RightCenter);//租赁费用
                        fGridDetail.Row(iRow).AutoFit();
                    }
                  #endregion
                    #region 发退料
                    foreach (HireDetail oHireDetail in oData.lstHireDetail)
                    {
                        fGridDetail.InsertRow(fGridDetail.Rows - 1, 1);
                        iRow = fGridDetail.Rows - 2;
                        iCol = 1; SetValue(iRow, iCol, oData.ProjectName, FlexCell.AlignmentEnum.LeftCenter);//出租单位
                        iCol += 1; SetValue(iRow, iCol, oHireDetail.MaterialName,  FlexCell.AlignmentEnum.LeftCenter);//	材料名称
                        iCol += 1; SetValue(iRow, iCol, oHireDetail.UnitName, FlexCell.AlignmentEnum.CenterCenter);//	单位
                        iCol += 1; SetValue(iRow, iCol, GetValue(oHireDetail.HirePrice, 4),FlexCell.AlignmentEnum.RightCenter);//租赁单价
                        iCol += 1; SetValue(iRow, iCol, oHireDetail.CreateDate.ToString("yyyy-MM-dd"),FlexCell.AlignmentEnum.RightCenter);//进退场日期
                        iCol += 1; SetValue(iRow, iCol, GetValue(oHireDetail.EnterQuanity, 4),FlexCell.AlignmentEnum.RightCenter);//进场数量
                        iCol += 1; SetValue(iRow, iCol, GetValue(oHireDetail.ExitQuanity, 4),FlexCell.AlignmentEnum.RightCenter);//退场数量
                        iCol += 1; SetValue(iRow, iCol, oHireDetail.Day.ToString(),FlexCell.AlignmentEnum.RightCenter);//天数
                        iCol += 1; SetValue(iRow, iCol, GetValue(oHireDetail.HireMoney, 6),FlexCell.AlignmentEnum.RightCenter);//租赁费用
                        iCol += 1; SetValue(iRow, iCol, GetValue(oHireDetail.LeftQuanity, 4), FlexCell.AlignmentEnum.RightCenter);//租赁费用
                        fGridDetail.Row(iRow).AutoFit();
                    }
                    #endregion
                    #region 小计
                    if (oData.TotelDetail != null)
                    {
                        HireDetail oHireDetail = oData.TotelDetail;
                        fGridDetail.InsertRow(fGridDetail.Rows - 1, 1);
                        iRow = fGridDetail.Rows - 2;
                        iCol = 1; SetValue(iRow, iCol, oData.ProjectName,FlexCell.AlignmentEnum.LeftCenter);//出租单位
                        iCol += 1; SetValue(iRow, iCol, oHireDetail.MaterialName, FlexCell.AlignmentEnum.LeftCenter);//	材料名称
                        iCol += 1; SetValue(iRow, iCol, oHireDetail.UnitName,FlexCell.AlignmentEnum.CenterCenter);//	单位
                        //iCol += 1; SetValue(iRow, iCol, oHireDetail.HirePrice.ToString("N4"));//租赁单价
                        //iCol += 1; SetValue(iRow, iCol, oHireDetail.CreateDate.ToString("yyyy-MM-dd"));//进退场日期
                        iCol += 1; SetValue(iRow, iCol, GetValue(oHireDetail.EnterQuanity, 4), FlexCell.AlignmentEnum.RightCenter);//进场数量
                        iCol += 1; SetValue(iRow, iCol, GetValue(oHireDetail.ExitQuanity, 4), FlexCell.AlignmentEnum.RightCenter);//退场数量
                        //iCol += 1; SetValue(iRow, iCol, oHireDetail.Day.ToString());//天数
                        iCol += 1; SetValue(iRow, iCol, GetValue(oHireDetail.HireMoney, 6), FlexCell.AlignmentEnum.RightCenter);//租赁费用
                        iCol += 1; SetValue(iRow, iCol, GetValue(oHireDetail.LeftQuanity, 4), FlexCell.AlignmentEnum.RightCenter);//租赁费用
                        fGridDetail.Row(iRow).AutoFit();
                    }
                    #endregion
                    #region 费用
                    foreach (HireCostDetail oCostDetail in oData.lstHireCostDetail.OrderBy(A=>A.CostType))
                    {
                        fGridDetail.InsertRow(fGridDetail.Rows - 1, 1);
                        iRow = fGridDetail.Rows - 2;
                        iCol = 1; //SetValue(iRow, iCol, oData.ProjectName);//出租单位
                        iCol += 1; SetValue(iRow, iCol, oCostDetail.CostType, FlexCell.AlignmentEnum.LeftCenter);//	费用类型
                        iCol += 1; SetValue(iRow, iCol, oCostDetail.UnitName, FlexCell.AlignmentEnum.CenterCenter);//	单位
                        iCol += 1; SetValue(iRow, iCol, GetValue(oCostDetail.Price, 4), FlexCell.AlignmentEnum.RightCenter);//租赁单价
                        iCol += 1; //SetValue(iRow, iCol, oCostDetail.CreateDate.ToString("yyyy-MM-dd"));//进退场日期
                        iCol += 1; SetValue(iRow, iCol, GetValue(oCostDetail.EnterQuanity, 4), FlexCell.AlignmentEnum.RightCenter);//进场数量
                        iCol += 1; SetValue(iRow, iCol, GetValue(oCostDetail.ExitQuanity, 4),FlexCell.AlignmentEnum.RightCenter);//退场数量
                        iCol += 1; SetValue(iRow, iCol, GetValue(oCostDetail.ConstValue, 6), FlexCell.AlignmentEnum.RightCenter);//理论值
                        iCol += 1; SetValue(iRow, iCol, GetValue(oCostDetail.Money, 6), FlexCell.AlignmentEnum.RightCenter);//租赁费用
                        iCol += 1; //SetValue(iRow, iCol, GetValue(oCostDetail.LeftQuanity, 4));//租赁费用
                        fGridDetail.Row(iRow).AutoFit();
                    }
                    #endregion
                }
                #region 合计
                fGridDetail.InsertRow(fGridDetail.Rows - 1, 1);
                iRow = fGridDetail.Rows - 2;
                iCol = 1; SetValue(iRow, iCol, txtProjectName.Text,FlexCell.AlignmentEnum.LeftCenter);//出租单位
                iCol += 1; SetValue(iRow, iCol,"合计");//	费用类型
                iCol += 1; //SetValue(iRow, iCol, oCostDetail.UnitName);//	单位
                iCol += 1;// SetValue(iRow, iCol, GetValue(oCostDetail.Price, 4));//租赁单价
                iCol += 1; //SetValue(iRow, iCol, oCostDetail.CreateDate.ToString("yyyy-MM-dd"));//进退场日期
                iCol += 1; //SetValue(iRow, iCol, GetValue(oCostDetail.EnterQuanity, 4));//进场数量
                iCol += 1; //SetValue(iRow, iCol, GetValue(oCostDetail.ExitQuanity, 4));//退场数量
                iCol += 1; //SetValue(iRow, iCol, oCostDetail.Day.ToString());//天数
                iCol += 1; SetValue(iRow, iCol, GetValue(lstData.Sum(a => a.SumMoney), 6), FlexCell.AlignmentEnum.RightCenter);//租赁费用
                iCol += 1; //SetValue(iRow, iCol, GetValue(oCostDetail.LeftQuanity, 4));//租赁费用
                fGridDetail.Row(iRow).AutoFit();
                FlexCell.Column oColumn = null;
                for (int iColumn = 1; iColumn < fGridDetail.Cols; iColumn++)
                {
                   oColumn= fGridDetail.Column(iColumn);
                   oColumn.AutoFit();
                }
                #endregion
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
        public List<MasterData> GetDate()
        {
            MasterData oMasterData = null;
            HireDetail oHireDetail = null;
            List<MasterData> lstData = new List<MasterData> ();
            List<string> lstMatCode = null;
            DataRow oRow = null;
            decimal dLeftQty = 0;
            decimal dSumMoney = 0;
            if (OrderMaster == null)
            {
                ShowMessage("请选择租赁合同");
                return null;
            }
            IList  lst = model.MaterialHireMngSvr.QueryMatHireBalanceReport(dtpDateBegin.Value, dtpDateEnd.Value, OrderMaster.ProjectId, OrderMaster.TheSupplierRelationInfo.Id);
            if (lst.Count == 2)
            {
                DataTable dtDetial =(lst[0] as DataSet).Tables[0];
                DataTable dtCost = (lst[1] as DataSet).Tables[0];
                lstMatCode = dtDetial.Rows.OfType<DataRow>().Select(a => ClientUtil.ToString(a["materialcode"])).Where(a => !string.IsNullOrEmpty(a)).Distinct().ToList();
                if (lstMatCode == null || lstMatCode.Count == 0)
                {
                    lstMatCode.AddRange(dtCost.Rows.OfType<DataRow>().Select(a => ClientUtil.ToString(a["materialcode"])).Where(a => !string.IsNullOrEmpty(a)).Distinct());
                }
                else
                {
                    lstMatCode.AddRange(dtCost.Rows.OfType<DataRow>().Select(a => ClientUtil.ToString(a["materialcode"])).Where(a => !string.IsNullOrEmpty(a) && !lstMatCode.Contains(a)).Distinct());
                }
                //tt.billCode,tt.materialcode,tt.materialname,tt.matstandardunitname,tt.billtype,tt.rentalprice,
                //tt.startdate,tt.days,sum(tt.matcolldtlqty) enterQty,sum(tt.matreturndtlqty)exitQty,sum(tt.money)money
                foreach (string sMatCode in lstMatCode)
                {
                    dLeftQty = 0;
                    dSumMoney = 0;
                    oMasterData = new MasterData() { ProjectName = txtProjectName.Text, MaterialCode = sMatCode };
                    #region 结存量
                    oRow = dtDetial.Rows.OfType<DataRow>().FirstOrDefault(a => ClientUtil.ToInt(a["billtype"]) == 2 && a["materialcode"] == sMatCode);//获取结存
                    if (oRow != null)
                    {
                        oMasterData.LeftDetail = new HireDetail()
                        {
                            MaterialName = string.Format("{0}  结存", ClientUtil.ToString(oRow["materialname"])),
                            UnitName = ClientUtil.ToString(oRow["matstandardunitname"]),
                            BillType = "结存",
                            HirePrice = ClientUtil.ToDecimal(oRow["rentalprice"]),
                            HireMoney = ClientUtil.ToDecimal(oRow["money"]),
                            CreateDate = ClientUtil.ToDateTime(oRow["startdate"]),
                            ExitQuanity = ClientUtil.ToDecimal(oRow["exitQty"]),
                            EnterQuanity = ClientUtil.ToDecimal(oRow["enterQty"]),
                            LeftQuanity = ClientUtil.ToDecimal(oRow["enterQty"]),
                            Day = ClientUtil.ToInt(oRow["days"])
                        };
                        dLeftQty = oMasterData.LeftDetail.LeftQuanity;
                        dSumMoney = oMasterData.LeftDetail.HireMoney;
                    }
                    #endregion
                    #region 获取收发料租赁结算单
                    //获取收退料
                    oMasterData.lstHireDetail = dtDetial.Rows.OfType<DataRow>().Where(a => ClientUtil.ToInt(a["billtype"]) != 2 && ClientUtil.ToString(a["materialcode"]) == sMatCode).OrderBy(a => ClientUtil.ToDateTime(a["startdate"]))
                        .Select(row => new HireDetail()
                        {
                            MaterialName = ClientUtil.ToString(row["materialname"]),
                            UnitName = ClientUtil.ToString(row["matstandardunitname"]),
                            BillType = ClientUtil.ToInt(row["billtype"]) == 0 ? "发料" : "退料",
                            HirePrice = ClientUtil.ToDecimal(row["rentalprice"]),
                            HireMoney = ClientUtil.ToDecimal(row["money"]),
                            CreateDate = ClientUtil.ToDateTime(row["startdate"]),
                            ExitQuanity = -ClientUtil.ToDecimal(row["exitQty"]),
                            EnterQuanity = ClientUtil.ToDecimal(row["enterQty"]),
                            Day = ClientUtil.ToInt(row["days"]),
                            LeftQuanity = 0
                        }).ToList();
                    #endregion
                    #region 汇总
                    oHireDetail = null;
                    if (oMasterData.TotelDetail != null) { oHireDetail = oMasterData.TotelDetail; }
                    else if (oMasterData.lstHireDetail != null && oMasterData.lstHireDetail.Count > 0) { oHireDetail = oMasterData.lstHireDetail[0]; }
                    if (oHireDetail != null)
                    {
                        oMasterData.TotelDetail = new HireDetail()
                        {
                            MaterialName = string.Format("{0}  小计", oHireDetail.MaterialName),
                            UnitName = oHireDetail.UnitName,
                            BillType = "小计",
                            //HirePrice = oHireDetail.HirePrice,
                            HireMoney = dSumMoney,
                           // CreateDate = oHireDetail.CreateDate,
                            ExitQuanity = 0,
                            EnterQuanity = dLeftQty,
                            LeftQuanity = dLeftQty,
                            Day=0
                        };
                        foreach (HireDetail oDetail in oMasterData.lstHireDetail)//
                        {
                            //发退料数据往小计上汇
                            oMasterData.TotelDetail.EnterQuanity += oDetail.EnterQuanity;
                            oMasterData.TotelDetail.ExitQuanity += oDetail.ExitQuanity;
                            oMasterData.TotelDetail.HireMoney += oDetail.HireMoney;
                            //结存量累加
                            dLeftQty += oDetail.EnterQuanity - oDetail.ExitQuanity;
                            oDetail.LeftQuanity = dLeftQty;
                        }

                    }
                    #endregion
                    #region  获取发退料费用信息 及运输费 安装费
                    //tt.businesscode,tt.materialcode,tt.materialname,tt.matstandardunitname,tt.businesstype,
                    //tt.costtype,tt.price,tt.constvalue,enterQty,exitQty,costmoney
                    oMasterData.lstHireCostDetail = dtCost.Rows.OfType<DataRow>().Where(a=>ClientUtil.ToString( a["materialcode"]) == sMatCode)
                        .Select(row => new HireCostDetail()
                        {
                            MaterialName = ClientUtil.ToString(row["materialname"]),
                            UnitName = ClientUtil.ToString(row["matstandardunitname"]),
                            CostType = ClientUtil.ToString(row["costtype"]),
                            BillType = ClientUtil.ToString(row["businesstype"]),
                            Price = ClientUtil.ToDecimal(row["price"]),
                            ExitQuanity = ClientUtil.ToDecimal(row["exitQty"]),
                            EnterQuanity = ClientUtil.ToDecimal(row["enterQty"]),
                            ConstValue = ClientUtil.ToDecimal(row["constvalue"]),
                            Money = ClientUtil.ToDecimal(row["costmoney"]),
                        }).ToList();

                    #endregion
                    lstData.Insert(lstData.Count, oMasterData);
                }
            }
            return lstData;
        }
        public class MasterData
        {
            /// <summary> 项目名称 </summary>
            public string ProjectName { get; set; }
            /// <summary> 编码 </summary>
            public string MaterialCode { get; set; }
            /// <summary>
            ///结存量
            /// </summary>
            public HireDetail LeftDetail;
            public HireDetail TotelDetail;
            public IList<HireCostDetail> lstHireCostDetail = new List<HireCostDetail>();
            public IList<HireDetail> lstHireDetail = new List<HireDetail>();
            /// <summary>
            /// 合计金额
            /// </summary>
            public decimal SumMoney{
                get { return (TotelDetail == null ? 0 : TotelDetail.HireMoney) + (lstHireCostDetail == null ? 0 : lstHireCostDetail.Sum(a => a.Money)); }
            }

        }
        public class HireDetail
        {
       
            /// <summary> 材料名称 </summary>
            public string MaterialName { get; set; }
            /// <summary> 单位 </summary>
            public string UnitName { get; set; }
            /// <summary> 单据类型 </summary>
            public string  BillType { get; set; }
            /// <summary> 价格 </summary>
            public decimal HirePrice { get; set; }
            /// <summary> 租赁费用 </summary>
            public decimal HireMoney { get; set; }
            /// <summary> 进退场日期 </summary>
            public DateTime CreateDate { get; set; }
            /// <summary>退数量 </summary>
            public decimal ExitQuanity { get; set; }
            /// <summary>进场数量 </summary>
            public decimal EnterQuanity { get; set; }
             /// <summary>天数 </summary>
            public int Day { get; set; }
            public decimal LeftQuanity { get; set; }
           
        }
        public class HireCostDetail
        {
            /// <summary> 材料名称 </summary>
            public string MaterialName { get; set; }
            /// <summary> 单位 </summary>
            public string UnitName { get; set; }
            public string CostType { get; set; }
            public string BillType { get; set; }
            public decimal Price { get; set; }
            public decimal ExitQuanity { get; set; }
            public decimal EnterQuanity { get; set; }
            public decimal ConstValue { get; set; }
            public decimal Money { get; set; }
        }
    }
    
}

