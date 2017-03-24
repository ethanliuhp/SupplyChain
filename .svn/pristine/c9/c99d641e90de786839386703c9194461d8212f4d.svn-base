using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Domain;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalSettlementMng.Domain;
using Microsoft.Office.Interop.Excel;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Core;

namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalSettlementMng
{
      public partial class VMaterialRentalReport : Form
    {
        MMatRentalMng model = new MMatRentalMng();
        public MaterialRentalSettlementMaster mrsm = null;
        public bool ifSign = true;
        DateTime lastdate=DateTime.Now;
        string mainStr = "机械租赁结算报表";
        string detailStr = "机械租赁结算明细";
        public VMaterialRentalReport(MaterialRentalSettlementMaster m,DateTime dt)
        {
            mrsm = m;
            lastdate = dt;
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

        void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void btnPreview_Click(object sender, EventArgs e)
        {
            if (tabMaterialRental.SelectedTab.Name.Equals("tabMain"))
            {
                FlexCell.PageSetup pageSetup = fGridMain.PageSetup;
                pageSetup.Landscape = true;
                this.fGridMain.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            }
            else if (tabMaterialRental.SelectedTab.Name.Equals("tabDetail"))
            {
                FlexCell.PageSetup pageSetup = fGridDetail.PageSetup;
                pageSetup.Landscape = true;
                //System.Drawing.Printing.PaperSize paperSize = new System.Drawing.Printing.PaperSize("自定义纸张", 800, 470);
                //pageSetup.PaperSize = paperSize;
                this.fGridDetail.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            }
        }
        void btnPrint_Click(object sender, EventArgs e)
        {
            FlexCell.PageSetup pageSetup = fGridMain.PageSetup;
            pageSetup.Landscape = true;
            this.fGridMain.Print(false);

            pageSetup = fGridDetail.PageSetup;
            pageSetup.Landscape = true;
            this.fGridDetail.Print(false);

            //if (this.tabSubBill.SelectedTab.Name.Equals("tabMain"))
            //{
            //    FlexCell.PageSetup pageSetup = fGridMain.PageSetup;
            //    pageSetup.Landscape = true;
            //    this.fGridMain.Print();
            //}
            //else if (tabSubBill.SelectedTab.Name.Equals("tabDetail"))
            //{
            //    FlexCell.PageSetup pageSetup = fGridDetail.PageSetup;
            //    pageSetup.Landscape = true;
            //    //System.Drawing.Printing.PaperSize paperSize = new System.Drawing.Printing.PaperSize("自定义纸张", 800, 470);
            //    //pageSetup.PaperSize = paperSize;
            //    this.fGridDetail.Print();
            //}
        }
        void btnPrintPage_Click(object sender, FlexCell.Grid.PrintPageEventArgs e)
        {
            if (e.Preview == false && e.PageNumber == 1)
            {
                model.MatMngSrv.UpdatePrintTimes(1, mrsm.Id);//回写次数
            }
        }
        void btnExcel_Click(object sender, EventArgs e)
        {
            string fileName = this.fGridMain.ExportToExcel(mainStr, false, false, true);
            if (fileName == "")
                return;

            ApplicationClass excel = new ApplicationClass();  // 1. 创建Excel应用程序对象的一个实例，相当于我们从开始菜单打开Excel应用程序
            int startIndex = fileName.LastIndexOf("\\") + 1;
            int endIndex = fileName.IndexOf(".x");
            mainStr = fileName.Substring(startIndex, endIndex - startIndex);

            //主文件对象
            Workbook workbook = excel.Workbooks.Open(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet = workbook.Sheets[1] as Worksheet;
            mySheet.Name = mainStr;

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
                MessageBox.Show("导出租赁结算报表成功！");
            }
            catch (Exception e1)
            {
                throw new Exception("导出租赁结算报表出错！");
            }
            finally {
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
        private string getYMD(int i, DateTime dt)
        {
            string dateString=dt.ToShortDateString();
            string str = dateString.Substring(6, 1);
            switch (i)
            {
                case 1:
                    return dateString.Substring(0, 4);
                    break;
                case 2:                 
                    if(string.Equals(str,"/")||string.Equals(str,"-"))
                    {
                        return dateString.Substring(5,1);
                    }
                    else
                    {
                        return dateString.Substring(5,2);
                    }
                    break;
                case 3:
                    if (string.Equals(str, "/") || string.Equals(str, "-"))
                    {
                        return dateString.Substring(7);
                    }
                    else
                    {
                        return dateString.Substring(8);
                    }
                    break;
                default:
                    return " ";
            }
        }
        private void InitData()
        {
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            LoadTempleteFile(mainStr + ".flx");
            LoadTempleteFile(detailStr + ".flx");
            //构造主表数据
            this.fGridMain.Cell(1, 1).Text = "机 械 租 赁 结 算 书";
            this.fGridMain.Cell(2, 2).Text = getYMD(1, mrsm.StartDate) + "年" + getYMD(2, mrsm.StartDate) + "月" + getYMD(3, mrsm.StartDate) + "日" + " 到 " + getYMD(1, mrsm.EndDate) + "年" + getYMD(2, mrsm.EndDate) + "月" + getYMD(3, mrsm.EndDate) + "日";
            fGridMain.Cell(2, 2).WrapText = true;
            fGridMain.Cell(3, 2).Text = mrsm.ProjectName;
            fGridMain.Cell(3, 2).WrapText = true; 
            this.fGridMain.Cell(4, 2).Text = Decimal.Round(mrsm.SumMoney, 2, MidpointRounding.AwayFromZero).ToString("N2");  //本次结算金额      
            this.fGridMain.Cell(5, 2).Text = CurrencyComUtil.GetMoneyChinese(Decimal.Round(mrsm.SumMoney, 2, MidpointRounding.AwayFromZero));
            fGridMain.Cell(5, 2).WrapText = true;        
            //decimal addUpBalMoney = Decimal.Round(mrsm .TheSubContractProject.AddupBalanceMoney, 2, MidpointRounding.AwayFromZero);
            //if (mrsm.DocState != VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute)
            //{
            //    addUpBalMoney += Decimal.Round(mrsm.BalanceMoney, 2, MidpointRounding.AwayFromZero);
            //}
            decimal accumulativeTotalMoney=Decimal.Round(model.MatMngSrv.GetAccumulativeTotalMoney(mrsm.ProjectId,mrsm.SupplierName,lastdate), 2, MidpointRounding.AwayFromZero);
            this.fGridMain.Cell(6, 2).Text = accumulativeTotalMoney.ToString("N2") + "";//累计结算金额
            this.fGridMain.Cell(7, 2).Text = CurrencyComUtil.GetMoneyChinese(accumulativeTotalMoney);
            fGridMain.Cell(8, 2).Text = mrsm.Descript;
            fGridMain.Cell(8, 2).WrapText = true;
            fGridMain.Cell(10, 2).Text = mrsm.ProjectName;
            fGridMain.Cell(10, 2).WrapText = true;
            this.fGridMain.Cell(11, 4).Text = mrsm.SupplierName;
            fGridMain.Cell(11, 4).WrapText = true;
     
   
            //条形码
            string a = Application.Business.Erp.SupplyChain.Client.Util.DateUtil.GetStrMoneyInchar(ClientUtil.ToString(mrsm.SumMoney));
            this.fGridMain.Cell(1, 4).Text = mrsm.Code.Substring(mrsm.Code.Length - 11) + "-" + a;
            this.fGridMain.Cell(1, 4).CellType = FlexCell.CellTypeEnum.BarCode;
            this.fGridMain.Cell(1, 4).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;
            //打印顺序号
            int printTimes = model.MatMngSrv.QueryPrintTimes(1,mrsm.Id);//取打印次数
            this.fGridMain.Cell(2, 4).Text = "打印顺序号: " + CommonUtil.GetPrintTimesStr(printTimes + 1);
            //fGridMain.Column(2).Alignment = FlexCell.AlignmentEnum.LeftCenter;
            //fGridMain.Column(4).Alignment = FlexCell.AlignmentEnum.LeftCenter;
            //条形码
            this.fGridDetail.Cell(1, 8).Text = mrsm.Code.Substring(mrsm.Code.Length - 11) + "-" + a;
            this.fGridDetail.Cell(1, 8).CellType = FlexCell.CellTypeEnum.BarCode;
            this.fGridDetail.Cell(1, 8).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;
            //构造明细表头数据 
            this.fGridDetail.Cell(2, 1).Text = "工程名称： " + mrsm.ProjectName;
            this.fGridDetail.Cell(2, 1).WrapText = true;
            this.fGridDetail.Cell(2, 7).Text = mrsm.StartDate.ToShortDateString();
            this.fGridDetail.Cell(2, 9).Text = mrsm.EndDate.ToShortDateString();
            if (mrsm.Details == null || mrsm.Details.Count == 0)
            {
                return;
            }
            int subdtlCount = 0;
            foreach (MaterialRentalSettlementDetail mrsd in mrsm.Details)
            {
                if(mrsd!=null)
                {
                       subdtlCount += mrsd.MaterialSubjectDetails.Count;
                }           
            }
            int totalCount = mrsm.Details.Count + subdtlCount;
            int dtlStartRowNum = 3;
            int dtlCount = 1;
            int sequenceNumber = 1;
            int subSequenceNumber = 1;
            decimal totalMoney = 0;
            this.fGridDetail.InsertRow(dtlStartRowNum+1, totalCount+1);  //插入空行
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGridDetail.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGridDetail.Cols-1);
            this.SetFlexGridDetailFormat(range);
            fGridDetail.PageSetup.LeftMargin = 0;
            fGridDetail.PageSetup.RightMargin = 0;
            fGridDetail.PageSetup.BottomMargin = 1;
            fGridDetail.PageSetup.TopMargin = 1;
            fGridDetail.PageSetup.CenterHorizontally = true;  //打印内容水平居中
            foreach (MaterialRentalSettlementDetail mrsd in mrsm.Details)
            {
                if(mrsd==null||mrsd.MaterialSubjectDetails.Count==0){
                    continue;
                }
                fGridDetail.Cell(dtlStartRowNum + dtlCount, 1).Text = sequenceNumber + "";
                fGridDetail.Cell(dtlStartRowNum + dtlCount, 2).Text = mrsd.MaterialName;
                fGridDetail.Cell(dtlStartRowNum + dtlCount, 4).Text = mrsd.Quantity.ToString("N2");
                fGridDetail.Cell(dtlStartRowNum + dtlCount, 5).Text = mrsd.QuantityUnitName;
                fGridDetail.Cell(dtlStartRowNum + dtlCount, 6).Text = mrsd.SettleDate.ToString("N2");
                fGridDetail.Cell(dtlStartRowNum + dtlCount, 7).Text = mrsd.DateUnitName;
                fGridDetail.Cell(dtlStartRowNum + dtlCount, 9).Text = mrsd.SettleMoney.ToString("N2");
                totalMoney += mrsd.SettleMoney;
                fGridDetail.Cell(dtlStartRowNum + dtlCount, 10).Text = mrsd.Descript;
                fGridDetail.Cell(dtlStartRowNum + dtlCount, 10).WrapText = true;
                dtlCount++;           
                subSequenceNumber=1;
                foreach(MaterialSubjectDetail msd in mrsd.MaterialSubjectDetails)
                {
                    if(msd==null)
                    {
                        continue;
                    }
                    fGridDetail.Cell(dtlStartRowNum + dtlCount, 1).Text = sequenceNumber + "." + subSequenceNumber + "";
                    fGridDetail.Cell(dtlStartRowNum + dtlCount, 3).Text = msd.SettleSubjectName;
                    fGridDetail.Cell(dtlStartRowNum + dtlCount, 4).Text = msd.Quantity.ToString("N2");
                    fGridDetail.Cell(dtlStartRowNum + dtlCount, 5).Text = msd.QuantityUnitName;
                    fGridDetail.Cell(dtlStartRowNum + dtlCount, 6).Text = msd.SettleDate.ToString("N2");
                    fGridDetail.Cell(dtlStartRowNum + dtlCount, 7).Text = msd.DateUnitName;
                    fGridDetail.Cell(dtlStartRowNum + dtlCount, 8).Text = msd.SettlePrice.ToString("N2");
                    fGridDetail.Cell(dtlStartRowNum + dtlCount, 10).Text = msd.Descript;
                    fGridDetail.Cell(dtlStartRowNum + dtlCount, 10).WrapText = true;
                    dtlCount++;
                    subSequenceNumber++;
                }
                sequenceNumber++;
            }
            fGridDetail.Column(2).AutoFit();
            fGridDetail.Column(3).AutoFit();
            fGridDetail.Cell(dtlStartRowNum + dtlCount, 1).Text = "合计";
            fGridDetail.Cell(dtlStartRowNum + dtlCount, 9).Text = totalMoney.ToString("N2");

            //审批信息    
            string signStr = CommonUtil.SetFlexAuditPrint(fGridDetail, mrsm.Id, dtlStartRowNum + totalCount + 2);
            if (signStr != "")
            {
                MessageBox.Show("该单据相关审核人" + signStr + "尚未设置电子签名图片，不能打印！请联系相关人员进入系统后，在系统状态栏双击并上传！", "警告");
                ifSign = false;
                return;
            }
            this.fGridMain.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            fGridDetail.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            fGridMain.Locked = true;
            fGridDetail.Locked = true;
      
        }

        private void SetFlexGridDetailFormat(FlexCell.Range range)
        {
            range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
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
