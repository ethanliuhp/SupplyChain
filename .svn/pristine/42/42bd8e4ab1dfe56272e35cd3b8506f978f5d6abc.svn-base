using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Microsoft.Office.Interop.Excel;
using System.IO;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.SpecialAccount
{
    public partial class VSpecialAccountReport : Form
    {
        public SpeCostSettlementMaster speMaster = null;
        string mainStr = "专项费用结算书";
        string detailStr = "专项费用结算单明细";
        public VSpecialAccountReport(SpeCostSettlementMaster master)
        {
            speMaster = master;
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
            this.btnPreview.Click += new EventHandler(btnPreview_Click);
        }

        void btnPreview_Click(object sender, EventArgs e)
        {
            if (this.tabSubBill.SelectedTab.Name.Equals("tabMain"))
            {
                FlexCell.PageSetup pageSetup = fGridMain.PageSetup;
                pageSetup.Landscape = true;
                this.fGridMain.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            }
            else if (tabSubBill.SelectedTab.Name.Equals("tabDetail"))
            {
                FlexCell.PageSetup pageSetup = fGridDetail.PageSetup;
                pageSetup.Landscape = true;
                //System.Drawing.Printing.PaperSize paperSize = new System.Drawing.Printing.PaperSize("自定义纸张", 800, 470);
                //pageSetup.PaperSize = paperSize;
                this.fGridDetail.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            }
        }

        void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            if (this.tabSubBill.SelectedTab.Name.Equals("tabMain"))
            {
                FlexCell.PageSetup pageSetup = fGridMain.PageSetup;
                pageSetup.Landscape = true;
                this.fGridMain.Print();
            }
            else if (tabSubBill.SelectedTab.Name.Equals("tabDetail"))
            {
                FlexCell.PageSetup pageSetup = fGridDetail.PageSetup;
                pageSetup.Landscape = true;
                //System.Drawing.Printing.PaperSize paperSize = new System.Drawing.Printing.PaperSize("自定义纸张", 800, 470);
                //pageSetup.PaperSize = paperSize;
                this.fGridDetail.Print();
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            string fileName = this.fGridMain.ExportToExcel(mainStr, false, false, true);
            ApplicationClass excel = new ApplicationClass();  // 1. 创建Excel应用程序对象的一个实例，相当于我们从开始菜单打开Excel应用程序

            //主文件对象
            Workbook workbook = excel.Workbooks.Open(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet = workbook.Sheets[1] as Worksheet;
            mySheet.Name = "分包结算封面";

            string tempName = fileName.Replace(mainStr, detailStr);
            this.fGridDetail.ExportToExcel(tempName, false, false, false);
            Workbook workbook1 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet1 = workbook1.Sheets[1] as Worksheet;
            mySheet1.Name = detailStr;

            try
            {
                mySheet1.Copy(Type.Missing, mySheet);

                workbook.Save();
                MessageBox.Show("导出分包结算报表成功！");
            }
            catch (Exception e1)
            {
                throw new Exception("导出分包结算报表出错！");
            }
            finally
            {
                //关闭工作表和退出Excel
                workbook.Close(false, Type.Missing, Type.Missing);
                workbook1.Close(false, Type.Missing, Type.Missing);
                //如果报表文件存在，先删除
                if (File.Exists(fileName.Replace(mainStr, detailStr)))
                {
                    File.Delete(fileName.Replace(mainStr, detailStr));
                }
                excel.Quit();
                excel = null;
            }
        }

        private void InitData()
        {
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            LoadTempleteFile(mainStr + ".flx");
            LoadTempleteFile(detailStr + ".flx");

            //构造主表数据
            this.fGridMain.Cell(2, 2).Text = speMaster.CreateDate.ToShortDateString();
            fGridMain.Cell(2, 2).WrapText = true;
            this.fGridMain.Cell(2, 4).Text = speMaster.RealOperationDate.ToShortDateString();
            fGridMain.Cell(2, 4).WrapText = true;
            this.fGridMain.Cell(3, 4).Text = speMaster.Code;
            fGridMain.Cell(3, 4).WrapText = true;
            this.fGridMain.Cell(4, 2).Text = projectInfo.Name;
            fGridMain.Cell(4, 2).WrapText = true;
            this.fGridMain.Cell(10, 4).Text = speMaster.SubcontractUnitName;
            fGridMain.Cell(10, 4).WrapText = true;
            this.fGridMain.Cell(11, 4).Text = speMaster.SubcontractProjectId.OwnerName;
            fGridMain.Cell(11, 4).WrapText = true;
            this.fGridMain.Cell(12, 4).Text = DateTime.Now.ToShortDateString();
            fGridMain.Cell(12, 4).WrapText = true;

            //构造明细表头数据
            this.fGridDetail.Cell(2, 2).Text = projectInfo.Name;
            this.fGridDetail.Cell(2, 5).Text = speMaster.CreateDate.ToShortDateString();
            this.fGridDetail.Cell(2, 8).Text = speMaster.RealOperationDate.ToShortDateString();

            //构造明细数据
            int dtlStartRowNum = 4;//模板中的行号
            int dtlCount = speMaster.Details.Count;

            //插入明细行
            this.fGridDetail.InsertRow(dtlStartRowNum, dtlCount + 1);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGridDetail.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGridDetail.Cols - 1);
            this.SetFlexGridDetailFormat(range);

            //写入明细数据
            int i = 0;
            decimal money = 0;
            decimal manageMoney = 0;
            decimal electMoney = 0;
            decimal payFees = 0;
            decimal otherMoney = 0;
            decimal settleMoney = 0;
            decimal manageRace = 0;
            decimal electRace = 0;
            foreach (SpeCostSettlementDetail detail in speMaster.Details)
            {
                fGridDetail.Cell(dtlStartRowNum + i, 1).Text = (i + 1) + "";
                fGridDetail.Cell(dtlStartRowNum + i, 2).Text = detail.EngTaskName;
                fGridDetail.Cell(dtlStartRowNum + i, 2).WrapText = true;
                fGridDetail.Cell(dtlStartRowNum + i, 3).Text = detail.Money + "";
                fGridDetail.Cell(dtlStartRowNum + i, 4).Text = detail.ManageMoney + "";
                fGridDetail.Cell(dtlStartRowNum + i, 5).Text = detail.ElectMoney + "";
                fGridDetail.Cell(dtlStartRowNum + i, 6).Text = detail.PayMentFees + "";
                fGridDetail.Cell(dtlStartRowNum + i, 7).Text = detail.OtherAccruals + "";
                fGridDetail.Cell(dtlStartRowNum + i, 8).Text = detail.SettlementMoney + "";
                fGridDetail.Row(dtlStartRowNum + i).AutoFit();
                money += detail.Money;
                manageMoney += detail.ManageMoney;
                electMoney += detail.ElectMoney;
                payFees += detail.PayMentFees;
                otherMoney += detail.OtherAccruals;
                settleMoney += detail.SettlementMoney;
                manageRace = detail.ManageAcer;
                electRace = detail.ElectAcer;
                i++;
            }
            decimal eleMoney = ClientUtil.ToDecimal(fGridDetail.Cell(7 + dtlCount, 3).Text);
            fGridDetail.Cell(5 + dtlCount, 3).Text = money + "";
            fGridDetail.Cell(5 + dtlCount, 4).Text = manageMoney + "";
            fGridDetail.Cell(5 + dtlCount, 5).Text = electMoney + "";
            fGridDetail.Cell(5 + dtlCount, 6).Text = payFees + "";
            fGridDetail.Cell(5 + dtlCount, 7).Text = otherMoney + "";
            fGridDetail.Cell(5 + dtlCount, 8).Text = settleMoney + "";
            fGridDetail.Cell(6 + dtlCount, 3).Text = manageMoney + "";
            fGridDetail.Cell(6 + dtlCount, 7).Text = manageRace * 100 + " %";
            fGridDetail.Cell(7 + dtlCount, 3).Text = electMoney + "";
            fGridDetail.Cell(7 + dtlCount, 7).Text = electRace * 100 + " %";
            fGridDetail.Cell(8 + dtlCount, 3).Text = payFees + "";
            fGridDetail.Cell(9 + dtlCount, 3).Text = otherMoney + "";
            fGridDetail.Cell(10 + dtlCount, 3).Text = settleMoney + "";
            fGridDetail.Cell(11 + dtlCount, 3).Text = CurrencyComUtil.GetMoneyChinese(settleMoney);
            fGridDetail.Cell(11 + dtlCount, 3).WrapText = true;
            fGridDetail.Cell(13 + dtlCount, 2).Text = speMaster.SubcontractUnitName;
            fGridDetail.Cell(13 + dtlCount, 3).WrapText = true;
            fGridDetail.Cell(14 + dtlCount, 6).Text = speMaster.CreatePersonName;
            this.fGridMain.Cell(5, 2).Text = settleMoney + "";
            this.fGridMain.Cell(6, 2).Text = CurrencyComUtil.GetMoneyChinese(settleMoney);
            fGridMain.Cell(6, 2).WrapText = true;
            this.fGridMain.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            fGridDetail.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
        }

        private void SetFlexGridDetailFormat(FlexCell.Range range)
        {
            range.Alignment = FlexCell.AlignmentEnum.RightCenter;
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
                if (modelName == (mainStr + ".flx"))
                {
                    this.fGridMain.OpenFile(path + "\\" + modelName);//载入格式
                }
                else if (modelName == (detailStr + ".flx"))
                {
                    this.fGridDetail.OpenFile(path + "\\" + modelName);//载入格式
                }
            }
            else
            {
                MessageBox.Show("未找到模板格式文件" + modelName);
                return;
            }
        }
    }
}
