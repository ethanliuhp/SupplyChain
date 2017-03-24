using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.Util;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    public partial class VContractDisclosurePrint : Form
    {
        private MCostMonthAccount model = new MCostMonthAccount();
        public bool ifSign = true;
        DisclosureMaster oMaster = null;
        public VContractDisclosurePrint(DisclosureMaster bill)
        {
            oMaster = bill;
            InitializeComponent();
            InitForm();
        }

        private void InitForm()
        {
            InitEvents();
            InitData();
        }

        private void InitEvents()
        {
            btnClose.Click += new EventHandler(btnClose_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            btnPrint.Click += new EventHandler(btnPrint_Click);
            fGridMain.PrintPage += new FlexCell.Grid.PrintPageEventHandler(btnPrintPage_Click);
            this.btnPreview.Click += new EventHandler(btnPreview_Click);
        }

        void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void btnPrintPage_Click(object sender, FlexCell.Grid.PrintPageEventArgs e)
        {
            //if (e.Preview == false && e.PageNumber == 1)
            //{
            //    //stockModel.StockInSrv.UpdateBillPrintTimes(5, subBill.Id);//回写次数
            //    //写打印日志
            //    StaticMethod.InsertLogData(oMaster.Id, "打印", oMaster.Code, ConstObject.LoginPersonInfo.Name, "合同交底单", "", oMaster.ProjectName);
            //}
        }
        void btnPreview_Click(object sender, EventArgs e)
        {
                FlexCell.PageSetup pageSetup = fGridMain.PageSetup;
                pageSetup.Landscape = true;
                this.fGridMain.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            FlexCell.PageSetup pageSetup = fGridMain.PageSetup;
            pageSetup.Landscape = true;
            this.fGridMain.Print(false);

            pageSetup = this.fGridMain.PageSetup;
            pageSetup.Landscape = true;
            this.fGridMain.Print(false);
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            string fileName = this.fGridMain.ExportToExcel("合同交底单", false, false, true);
            
        }

        private void InitData()
        {
            if (oMaster != null)
            {
                if (oMaster.Details.Count > 0)
                {
                    DisclosureDetail oDetail = oMaster.Details.ElementAt(0) as DisclosureDetail;
                    int iRow = 0, iCol = 0;
                    LoadTempleteFile("合同交底样稿.flx");
                    iRow = 2; iCol = 2;//项目
                    FlexCell.Cell oCell = fGridMain.Cell(iRow, iCol);
                    oCell.Text = oMaster.ProjectName;
                    oCell.WrapText = true;
                    iRow = 2; iCol = 5;//交底时间:
                    oCell = fGridMain.Cell(iRow, iCol);
                    oCell.Text = oMaster.CreateDate.ToString("交底时间:yyyy-MM-dd");
                    iRow = 3; iCol = 2;//合同名称
                    oCell = fGridMain.Cell(iRow, iCol);
                    oCell.Text = oMaster.ContractName;
                    oCell.WrapText = true;
                    iRow = 4; iCol = 2;//分包单位
                    oCell = fGridMain.Cell(iRow, iCol);
                    oCell.Text = oMaster.BearerOrgName;
                    oCell.WrapText = true;
                    iRow = 5; iCol = 2;//分包方承包范围
                    oCell = fGridMain.Cell(iRow, iCol);
                    oCell.Text = oDetail.SubPackage;
                    oCell.WrapText = true;
                    iRow = 6; iCol = 2;//分包方承包方式
                    oCell = fGridMain.Cell(iRow, iCol);
                    oCell.Text = oDetail.ContractType;
                    oCell.WrapText = true;
                    iRow = 7; iCol = 2;//分包合同价格
                    oCell = fGridMain.Cell(iRow, iCol);
                    oCell.Text = oDetail.ContractInterimMoney.ToString("N2");
                    iRow = 8; iCol = 2;//质量目标及违约责任
                    oCell = fGridMain.Cell(iRow, iCol);
                    oCell.Text = oDetail.QualityBreachDuty;
                    oCell.WrapText = true;
                    iRow = 9; iCol = 2;//工期目标及违约责任
                    oCell = fGridMain.Cell(iRow, iCol);
                    oCell.Text = oDetail.DurationBreachDuty;
                    oCell.WrapText = true;
                    iRow = 10; iCol = 2;//安全目标及违约责任
                    oCell = fGridMain.Cell(iRow, iCol);
                    oCell.Text = oDetail.SafetyBreachDuty;
                    oCell.WrapText = true;
                    iRow = 11; iCol = 2;//文明施工要求及违约责任
                    oCell = fGridMain.Cell(iRow, iCol);
                    oCell.Text = oDetail.CivilizedBreachDuty;
                    oCell.WrapText = true;
                    iRow = 12; iCol = 2;//劳动力要求
                    oCell = fGridMain.Cell(iRow, iCol);
                    oCell.Text = oDetail.LaborDemand;
                    oCell.WrapText = true;
                    iRow = 13; iCol = 2;//主要材料要求
                    oCell = fGridMain.Cell(iRow, iCol);
                    oCell.Text = oDetail.MaterialDemand;
                    oCell.WrapText = true;
                    iRow = 14; iCol = 2;//付款方式
                    oCell = fGridMain.Cell(iRow, iCol);
                    oCell.Text = oDetail.PaymentType;
                    oCell.WrapText = true;
                    iRow = 15; iCol = 2;//保修期及保修金
                    oCell = fGridMain.Cell(iRow, iCol);
                    oCell.Text = oDetail.WarrantyDateMoney;
                    oCell.WrapText = true;
                    iRow = 16; iCol = 2;//其他要说明事项
                    oCell = fGridMain.Cell(iRow, iCol);
                    oCell.Text = oDetail.OtherDesc;
                    oCell.WrapText = true;
                    
                    FlexCell.Range range = fGridMain.Range(3, 2, 16, 5);
                    for (iRow = 2; iRow <= 16; iRow++)
                    {
                        fGridMain.Row(iRow).AutoFit();
                    }
                        this.SetFlexGridDetailFormat(range);
                    fGridMain.PageSetup.LeftMargin = 0;
                    fGridMain.PageSetup.RightMargin = 0;
                    fGridMain.PageSetup.BottomMargin = 1;
                    fGridMain.PageSetup.TopMargin = 1;
                    fGridMain.PageSetup.CenterHorizontally = true;  //打印内容水平居中

                    string signStr = CommonUtil.SetFlexAuditPrint(fGridMain, oMaster.Id, 18);

                    if (signStr != "")
                    {
                        MessageBox.Show("该单据相关审核人" + signStr + "尚未设置电子签名图片，不能打印！请联系相关人员进入系统后，在系统状态栏双击并上传！", "警告");
                        ifSign = false;
                     
                    }
                    //this.fGridMain.Row(17).Visible = false;
                    this.fGridMain.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
                    fGridMain.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
                    fGridMain.SelectionMode =  FlexCell.SelectionModeEnum.ByCell;
                    fGridMain.CellBorderColor = System.Drawing.Color.Black;
                    fGridMain.CellBorderColorFixed = System.Drawing.Color.Black;
                    fGridMain.SelectionBorderColor = System.Drawing.Color.Black;
                    fGridMain.Locked = true;
                }
                else
                {
                    MessageBox.Show("主表信息中没明细");
                }
            }
            else
            {
                MessageBox.Show("主表信息为空");
            }
        }

        private void SetFlexGridDetailFormat(FlexCell.Range range)
        {
            range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            range.FontBold = false;
        }

        private void LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式

                this.fGridMain.OpenFile(path + "\\" + modelName);//载入格式

            }
            else
            {
                MessageBox.Show("未找到模板格式文件" + modelName);
                return;
            }
        }
    }
}
