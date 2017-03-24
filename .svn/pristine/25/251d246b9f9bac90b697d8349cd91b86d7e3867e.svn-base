using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.ConcreteManage.ConcreteBalanceMng.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Util;
using Microsoft.Office.Interop.Excel;
using System.IO;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.ConcreteManage.ConcreteCheckingMng.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConBalanceMng
{
    public partial class VConBalReport : Form
    {
        public ConcreteBalanceMaster conMaster = null;
        private ConcreteCheckingMaster conCheckMaster;
        MConcreteMng model = new MConcreteMng();
        private MStockMng stockModel = new MStockMng();
        string mainStr = "商品砼结算单";
        string detailStr = "商品砼结算单明细";
        public VConBalReport(ConcreteBalanceMaster master)
        {
            conMaster = master;
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

        void btnPrintPage_Click(object sender, FlexCell.Grid.PrintPageEventArgs e)
        {
            if (e.Preview == false && e.PageNumber == 1)
            {
                stockModel.StockInSrv.UpdateBillPrintTimes(4, conMaster.Id);//回写次数
                //写打印日志
                StaticMethod.InsertLogData(conMaster.Id, "打印", conMaster.Code, ConstObject.LoginPersonInfo.Name, "商品砼结算单", "", conMaster.ProjectName);
            }
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

            #region 构造数据
            //通过前驱对账单,构造结算明细数据
            decimal otherMoney = 0;//其他费用
            decimal lessPumpMoney = 0;//扣磅罚款
            decimal ticketVolume = 0;//小票数量
            string checkId = conMaster.ForwardBillId;
            Hashtable checkHt = new Hashtable();
            conCheckMaster = model.ConcreteMngSrv.GetConCheckingMasterById(checkId);
            foreach (ConcreteCheckingDetail checkDetail in conCheckMaster.Details)
            {
                string checkdtlid = checkDetail.Id;
                DataDomain domain = new DataDomain();
                domain.Name1 = checkDetail.TicketVolume + "";
                domain.Name2 = checkDetail.LessPumpVolume + "";
                checkHt.Add(checkdtlid, domain);
            }
            //补上小票量、抽磅减量
            foreach (ConcreteBalanceDetail detail in conMaster.Details)
            {
                string checkdtlid = detail.ForwardDetailId;
                if (checkHt.Contains(checkdtlid))
                {
                    DataDomain domain = (DataDomain)checkHt[checkdtlid];
                    detail.TempData1 = ClientUtil.ToString(domain.Name1);//小票方量
                    detail.TempData2 = ClientUtil.ToString(domain.Name2);//扣磅方量
                }
                if (detail.Quantity == 0 && detail.Price == 0)
                {
                    otherMoney += detail.Money;
                }
                if (ClientUtil.ToDecimal(detail.TempData2) != 0)
                {
                    lessPumpMoney += decimal.Round(ClientUtil.ToDecimal(detail.TempData2) * detail.Price, 2);
                }
                ticketVolume += ClientUtil.ToDecimal(detail.TempData1);
            }
            //构造分部位+规格型号明细信息
            var queryGwbs = from c in conMaster.Details orderby c.UsedPartName select c;

            //构造规格型号明细信息
            //Hashtable material_ht = new Hashtable();
            Dictionary<string, DataDomain> material_d = new Dictionary<string, DataDomain>();
            var queryMaterial = from c in conMaster.Details orderby c.MaterialName,c.MaterialSpec select c;
            foreach (ConcreteBalanceDetail detail in queryMaterial)
            {
                string str = detail.MaterialResource.Id + "-" + detail.IsPump;
                if (material_d.ContainsKey(str))
                {
                    DataDomain domain = (DataDomain)material_d[str];
                    domain.Name4 = ClientUtil.ToDecimal(domain.Name4) + ClientUtil.ToDecimal(detail.TempData1) + "";
                    domain.Name5 = ClientUtil.ToDecimal(domain.Name5) + ClientUtil.ToDecimal(detail.BalanceVolume) + "";
                    domain.Name7 = ClientUtil.ToDecimal(domain.Name7) + ClientUtil.ToDecimal(detail.Money) + "";
                    if (ClientUtil.ToDecimal(domain.Name5) != 0)
                    {
                        domain.Name6 = decimal.Round(ClientUtil.ToDecimal(domain.Name7) / ClientUtil.ToDecimal(domain.Name5), 2);
                    }
                    else
                    {
                        domain.Name6 = detail.Price;
                    }
                }
                else
                {
                    DataDomain domain = new DataDomain();
                    domain.Name1 = detail.MaterialSpec + detail.MaterialName;
                    if (detail.IsPump == true)
                    {
                        domain.Name2 = "泵送";
                    }
                    else
                    {
                        domain.Name2 = "自卸";
                    }
                    domain.Name3 = detail.MatStandardUnitName;
                    domain.Name4 = detail.TempData1;
                    domain.Name5 = detail.BalanceVolume;
                    domain.Name6 = detail.Price;
                    domain.Name7 = detail.Money;
                    material_d.Add(str, domain);
                }
            }

            #endregion
            //构造主表数据
            fGridDetail.AutoRedraw = false;
            this.fGridMain.Cell(1, 1).Text = "中建三局集团有限公司工程总承包公司 " + projectInfo.Name + " 项目";
            this.fGridMain.Cell(4, 4).Text = conMaster.Code;
            fGridMain.Cell(4, 4).WrapText = true;
            this.fGridMain.Cell(5, 2).Text = projectInfo.Name;
            fGridMain.Cell(5, 2).WrapText = true;

            //构造明细表头数据
            this.fGridDetail.Cell(2, 7).Text = "项目名称：" + projectInfo.Name;
            this.fGridDetail.Cell(2, 1).Text = "供应单位：" + conMaster.SupplierName;

            //构造明细数据
            int dtlStartRowNum = 4;//模板中的行号
            int dtlCount = conMaster.Details.Count;//明细数
            int specCount = material_d.Count;//分规格汇总数
            //插入明细行
            
            this.fGridDetail.InsertRow(dtlStartRowNum, dtlCount);
            
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGridDetail.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGridDetail.Cols - 1);
            this.SetFlexGridDetailFormat(range);

            //写入明细数据
            int i = 0;
            decimal sumAcctTotalPrice = conMaster.SumMoney;
            foreach (ConcreteBalanceDetail detail in queryGwbs)
            {
                fGridDetail.Cell(dtlStartRowNum + i, 1).Text = (i + 1) + "";
                fGridDetail.Cell(dtlStartRowNum + i, 2).Text = detail.UsedPartName + "";
                fGridDetail.Cell(dtlStartRowNum + i, 2).WrapText = true;
                fGridDetail.Cell(dtlStartRowNum + i, 3).Text = detail.MaterialSpec + detail.MaterialName;
                fGridDetail.Cell(dtlStartRowNum + i, 3).WrapText = true;
                if (detail.IsPump)
                {
                    fGridDetail.Cell(dtlStartRowNum + i, 4).Text = "泵送";
                    fGridDetail.Cell(dtlStartRowNum + i, 4).WrapText = true;
                }
                else
                {
                    fGridDetail.Cell(dtlStartRowNum + i, 4).Text = "自卸";
                    fGridDetail.Cell(dtlStartRowNum + i, 4).WrapText = true;
                }
                fGridDetail.Cell(dtlStartRowNum + i, 5).Text = detail.MatStandardUnitName + "";
                fGridDetail.Cell(dtlStartRowNum + i, 6).Text = detail.TempData1 + "";
                fGridDetail.Cell(dtlStartRowNum + i, 7).Text = detail.BalanceVolume + "";
                
                fGridDetail.Cell(dtlStartRowNum + i, 8).Text = detail.Price + "";
                fGridDetail.Cell(dtlStartRowNum + i, 9).Text = detail.Money + "";
                fGridDetail.Cell(dtlStartRowNum + i, 9).WrapText = true;
               
                fGridDetail.Row(dtlStartRowNum + i).AutoFit();
                i++;
            }

            //写入汇总规格数据
            int startSpecCount = dtlStartRowNum + dtlCount + 2;//汇总规格数据插入起始位置
            fGridDetail.Cell(startSpecCount - 2, 9).Text = conMaster.SumMoney + "";//小计 = 
            int materialCount = specCount - 1;
            this.fGridDetail.InsertRow(startSpecCount, materialCount); 
            int j = -1;
            foreach (DataDomain domain in material_d.Values)
            {
                fGridDetail.Cell(startSpecCount + j, 3).Text = domain.Name1 + "";
                fGridDetail.Cell(startSpecCount + j, 4).Text = domain.Name2 + "";
                fGridDetail.Cell(startSpecCount + j, 5).Text = domain.Name3 + "";
                fGridDetail.Cell(startSpecCount + j, 6).Text = domain.Name4 + "";
                fGridDetail.Cell(startSpecCount + j, 7).Text = domain.Name5 + "";
                fGridDetail.Cell(startSpecCount + j, 8).Text = domain.Name6 + "";
                fGridDetail.Cell(startSpecCount + j, 9).Text = domain.Name7 + "";
                j++;
            }
            int startSumIndex = startSpecCount + specCount - 1;
            //加入合计
            fGridDetail.Cell(startSumIndex, 6).Text = ticketVolume + "";
            fGridDetail.Cell(startSumIndex, 7).Text = conMaster.SumVolumeQuantity + "";
            fGridDetail.Cell(startSumIndex, 9).Text = conMaster.SumMoney + "";
            fGridDetail.Row(startSumIndex).AutoFit();

            fGridDetail.Cell(startSumIndex + 1, 9).Text = otherMoney + "";
            fGridDetail.Cell(startSumIndex + 2, 9).Text = lessPumpMoney + "";
            fGridDetail.Cell(startSumIndex + 4, 3).Text = conMaster.SumVolumeQuantity + "";
            fGridDetail.Cell(startSumIndex + 5, 3).Text = conMaster.SumMoney + "";
            fGridDetail.Cell(startSumIndex + 6, 3).Text = CurrencyComUtil.GetMoneyChinese(conMaster.SumMoney);

            //int maxRow = dtlStartRowNum + dtlCount + 4;
            //string signStr = CommonUtil.SetFlexAuditPrint(fGridDetail, conMaster.Id, maxRow);//电子签名
            //条形码
            string a = Application.Business.Erp.SupplyChain.Client.Util.DateUtil.GetStrMoneyInchar(ClientUtil.ToString(sumAcctTotalPrice));
            this.fGridMain.Cell(2, 4).Text = conMaster.Code.Substring(conMaster.Code.Length - 11) + "-" + a;
            this.fGridMain.Cell(2, 4).CellType = FlexCell.CellTypeEnum.BarCode;
            this.fGridMain.Cell(2, 4).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;

            //获取本会计期开始和结束日期
            IList dateList = model.ConcreteMngSrv.GetFiscalInfo(conCheckMaster.CreateDate);
            if (dateList != null && dateList.Count > 0)
            {
                if (dateList[0] != null && dateList[1] != null)
                {
                    this.fGridMain.Cell(3, 1).Text = ClientUtil.ToDateTime(dateList[0]).ToShortDateString() + " 至 " + ClientUtil.ToDateTime(dateList[1]).ToShortDateString();
                }
            }
            int printTimes = stockModel.StockInSrv.QueryBillPrintTimes(4, conMaster.Id);//取打印次数
            this.fGridMain.Cell(3, 3).Text = "打印顺序号: " + CommonUtil.GetPrintTimesStr(printTimes + 1);

            //Image img = showEPhote("SANGZIMIN");
            //fGridDetail.Images.Add(img, "sample");
            //fGridDetail.Cell(dtlStartRowNum + dtlCount + 3, 9).SetImage("sample");
            //fGridDetail.Cell(dtlStartRowNum + dtlCount + 3, 9).Alignment = FlexCell.AlignmentEnum.LeftGeneral;

            fGridMain.Cell(5, 2).Text = sumAcctTotalPrice + "元";
            fGridMain.Cell(6, 2).Text = CurrencyComUtil.GetMoneyChinese(sumAcctTotalPrice);
            if (conMaster.AddSumMoney != 0)
            {
                fGridMain.Cell(7, 2).Text = conMaster.AddSumMoney + "元";
            }
            else
            {
                decimal getAddMoney = model.ConcreteMngSrv.GetAddSumMoney(conMaster.ProjectId, conMaster.TheSupplierRelationInfo.Id);
                if (getAddMoney != 0)
                {
                    fGridMain.Cell(7, 2).Text = getAddMoney + "元";
                }
            }
            
            if (lessPumpMoney != 0)
            {
                fGridMain.Cell(6, 4).Text = lessPumpMoney + "元";
            }
            if (otherMoney != 0)
            {
                fGridMain.Cell(7, 4).Text = otherMoney + "元";
            }
            fGridMain.Cell(10, 4).Text = conMaster.SupplierName;
            fGridMain.Cell(10, 4).WrapText = true;
            this.fGridMain.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            fGridDetail.BackColorBkg = System.Drawing.SystemColors.ButtonFace;

            FlexCell.PageSetup pageSetup = fGridDetail.PageSetup;
            pageSetup.BottomMargin = 2;
            pageSetup.RightFooter = "打印顺序号:[" + CommonUtil.GetPrintTimesStr(printTimes + 1) + "],打印时间:[" + CommonMethod.GetServerDateTime() + " ],打印人:[" + ConstObject.LoginPersonInfo.Name + "]   第&P页/共&N页   ";

            fGridMain.Locked = true;
            fGridDetail.Locked = true;
            fGridDetail.AutoRedraw = true;
            fGridDetail.Refresh();
        }


         /// <summary>
        /// 写文件到本地目录
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileByte"></param>
        private void WriteBinaryToFile(string filePath, byte[] fileByte)
        {
            string extName = Path.GetExtension(filePath).ToLower();

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                if (FilePropertyUtility.ListCommonFile.Contains(extName))
                {
                    //写普通文件方式
                    fs.Write(fileByte, 0, fileByte.GetUpperBound(0));
                }
                else if (FilePropertyUtility.ListOfficeFile.Contains(extName))
                {
                    //写office文件方式
                    BinaryWriter w = new BinaryWriter(fs);
                    w.Write(fileByte);
                    w.Close();
                }
                else
                {
                    fs.Write(fileByte, 0, fileByte.GetUpperBound(0));
                }

                fs.Close();
                fs.Dispose();
            }
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

        private Image showEPhote(string name)
        {
            DBSservice.FileSrvSoapClient dbs = new Application.Business.Erp.SupplyChain.Client.DBSservice.FileSrvSoapClient();
            byte[] signBytes = dbs.GetUserSign(name);
            //WriteBinaryToFile("E:\\xxx\\x.jpg", signBytes);
            Image img = null;
            if (signBytes != null)
            {
                img = ReturnPhoto(signBytes);
            }
            return img;
        }

        //将byte[]转换成图片
        public System.Drawing.Image ReturnPhoto(byte[] streamByte)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(streamByte);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            return img;
        } 
    }
}
