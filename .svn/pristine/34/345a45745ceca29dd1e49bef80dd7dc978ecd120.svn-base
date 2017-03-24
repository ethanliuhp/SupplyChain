using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using VirtualMachine.Core;
using System.Collections;

using VirtualMachine.Core.Expression;

using NHibernate;
using VirtualMachine.Component.Util;
using Microsoft.Office.Interop.Excel;
using System.IO;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.StockMng;
namespace Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng
{
    public partial class VEndAccountAuditReport : Form
    {

        private MStockMng stockModel = new MStockMng();
        public MSubContractBalance subContractOperate = new MSubContractBalance();
        public EndAccountAuditBill subBill = null;
        string mainStr = "分包终结结算报表";
        string detailStr = "分包终结结算明细";

        public VEndAccountAuditReport(EndAccountAuditBill bill)
        {
            subBill = bill;
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
            fGridDetail.PrintPage += new FlexCell.Grid.PrintPageEventHandler(btnPrintPage_Click);
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
                this.fGridDetail.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            }
        }



        void btnPrintPage_Click(object sender, FlexCell.Grid.PrintPageEventArgs e)
        {
            if (e.Preview == false && e.PageNumber == 1)
            {
                stockModel.StockInSrv.UpdateBillPrintTimes(5, subBill.Id);//回写次数
                //写打印日志
                StaticMethod.InsertLogData(subBill.Id, "打印", subBill.Code, ConstObject.LoginPersonInfo.Name, "分包终结结算单", "", subBill.ProjectName);
            }
        }
        void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
            mySheet.Name = "分包终结结算封面";

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
                MessageBox.Show("导出分包终结结算报表成功！");
            }
            catch (Exception e1)
            {
                throw new Exception("导出分包终结结算报表出错！");
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
        void btnPrint_Click(object sender, EventArgs e)
        {
            FlexCell.PageSetup pageSetup = fGridMain.PageSetup;
            pageSetup.Landscape = true;
            this.fGridMain.Print(false);
            pageSetup = fGridDetail.PageSetup;
            pageSetup.Landscape = true;
            this.fGridDetail.Print(false);       
        }
        private void InitData()
        {
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            LoadTempleteFile(mainStr + ".flx");
            LoadTempleteFile(detailStr + ".flx");
            #region 构造主表数据
            string monthStr = subBill.CreateDate.Month.ToString();
            ////构造主表数据
            this.fGridMain.Cell(1, 1).Text = " 分 包 终 结 结 算 单";
            this.fGridMain.Cell(2, 1).Text = "制单日期： " + subBill.CreateDate.ToShortDateString() + "   结算日期： " + subBill.CreateDate.ToShortDateString().Substring(0, 4) + " 年 " + monthStr + " 月 ";
            fGridMain.Cell(2, 1).WrapText = true;
            this.fGridMain.Cell(3, 2).Text = "编号： " + subBill.Code;
            this.fGridMain.Cell(4, 2).Text = projectInfo.Name;
            fGridMain.Cell(4, 2).WrapText = true;
            this.fGridMain.Cell(5, 2).Text = Decimal.Round(subBill.BalanceMoney, 2, MidpointRounding.AwayFromZero) + "";  //本次结算金额
            this.fGridMain.Cell(6, 2).Text = CurrencyComUtil.GetMoneyChinese(Decimal.Round(subBill.BalanceMoney, 2, MidpointRounding.AwayFromZero));
            decimal addUpBalMoney = Decimal.Round(subBill.TheSubContractProject.AddupBalanceMoney, 2, MidpointRounding.AwayFromZero);
            if (subBill.DocState != VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute)
            {
                addUpBalMoney += Decimal.Round(subBill.BalanceMoney, 2, MidpointRounding.AwayFromZero);
            }
            this.fGridMain.Cell(5, 1).Text = "结算金额(元):";
            this.fGridMain.Cell(6, 1).Text = "结算金额大写:";
            this.fGridMain.Cell(7, 1).Text = "";//累计结算金额，不需要显示，直接清空
            this.fGridMain.Cell(7, 2).Text = "";//累计结算金额，不需要显示，直接清空
            this.fGridMain.Cell(8, 1).Text = "";//累计结算金额，不需要显示，直接清空
            this.fGridMain.Cell(8, 2).Text = "";//累计结算金额，不需要显示，直接清空 
            this.fGridMain.Cell(9, 2).Text = "分包终结结算";//备注
            fGridMain.Cell(9, 2).WrapText = true;
            this.fGridMain.Cell(11, 2).Text = projectInfo.ManagerDepart;
            this.fGridMain.Cell(11, 2).WrapText = true;
            this.fGridMain.Cell(12, 4).Text = subBill.SubContractUnitName;
            fGridMain.Cell(12, 4).WrapText = true;
           
            //条形码
            string a = Application.Business.Erp.SupplyChain.Client.Util.DateUtil.GetStrMoneyInchar(ClientUtil.ToString(subBill.BalanceMoney));
            this.fGridMain.Cell(1, 4).Text = subBill.Code.Substring(subBill.Code.Length - 11) + "-" + a;
            this.fGridMain.Cell(1, 4).CellType = FlexCell.CellTypeEnum.BarCode;
            this.fGridMain.Cell(1, 4).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;
            this.fGridMain.Cell(3, 4).Text = "—";
            this.fGridMain.Cell(4, 4).Text = "—";//劳务费
            this.fGridMain.Cell(5, 4).Text = "—";
            this.fGridMain.Cell(6, 4).Text = "—";
            this.fGridMain.Cell(8, 4).Text = "—";
            this.fGridMain.Cell(9, 4).Text = "—";
            this.fGridMain.Cell(7, 4).Text = "—";
            this.fGridMain.Cell(10, 4).Text = "—";
            #endregion

            #region 构造明细数据
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", subBill.Id));
            oq.AddFetchMode("BalanceTaskDtl", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("BalanceTaskDtl.TheCostItem", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            IList subContractBalanceDetailList = subContractOperate.ObjectQuery(typeof(EndAccountAuditDetail), oq);
         
            //分包结算明细,显示在flexcell表格控件当中
            List<EndAccountAuditDetail> htn = new List<EndAccountAuditDetail>();//合同内     合同清单范围内部分
            List<EndAccountAuditDetail> sjbg = new List<EndAccountAuditDetail>();//设计变更  设计变更增减部分
            List<EndAccountAuditDetail> qzsp = new List<EndAccountAuditDetail>();//签证索赔  签证及索赔部分
            List<EndAccountAuditDetail> jsg = new List<EndAccountAuditDetail>();//计时工
            List<EndAccountAuditDetail> fbqz = new List<EndAccountAuditDetail>();//分包签证
            List<EndAccountAuditDetail> dgkk = new List<EndAccountAuditDetail>();//代工扣款
            List<EndAccountAuditDetail> fk = new List<EndAccountAuditDetail>();//罚款
            List<EndAccountAuditDetail> sdf = new List<EndAccountAuditDetail>();//水电费，建管费
            List<EndAccountAuditDetail> qt = new List<EndAccountAuditDetail>();//其它

            Decimal BalanceTotalPrice = 0.00M;//结算合价汇总用
            Decimal LittleBalanceTotalPrice = 0.00M;//结算合价小计用
            foreach (EndAccountAuditDetail dt in subContractBalanceDetailList)
            {
               //需要得到的值： 工程量 对应 结算数量，结算单价，结算合价，结算单位，结算依据，结算月份，备注
                #region 1：合同内
                if (dt.BalanceBase == "合同内")
                { 
                    htn.Add(dt);
                }
                #endregion
                #region 2：设计变更
                else if (dt.BalanceBase == "设计变更")
                {  
                    sjbg.Add(dt);       
                }
                #endregion
                #region  3：签证索赔
                else if (dt.BalanceBase == "签证索赔")
                { 
                    qzsp.Add(dt);
                }
                #endregion
                #region 4：计时工
                else if(dt.BalanceBase == "计时工")
                {
                    jsg.Add(dt);
                }
                #endregion
                #region 5：分包签证
                else if(dt.BalanceBase == "分包签证") 
                {
                    fbqz.Add(dt);
                }
                #endregion
                #region 6：代工扣款
                else if(dt.BalanceBase == "代工扣款")
                {
                    dgkk.Add(dt);
                }
                #endregion
                #region 7：罚款
                else if (dt.BalanceBase == "罚款")
                {
                    fk.Add(dt);
                }
                #endregion
                #region 8：水电费，建管费
                else if ((dt.BalanceBase == "水电费") || (dt.BalanceBase == "建管费"))
                {
                    sdf.Add(dt);
                }
                #endregion
                #region 9:其它
                else
                {
                    qt.Add(dt);
                }
                #endregion
            }

            #region 填充数据
            int i = 0; 
            int num = 4;
            #region 合同内
            //MessageBox.Show("合同内明细的个数：" + htn.Count.ToString());
            //int Count = 2;//模拟赋值,假设有2列
            //fGridMain.InsertRow(4, 2);//测试，插入2列，2代表数据的行数
            //num = 4;//测试
            //fGridMain.Cell(num, 1).Text = "1." + (i + 1).ToString();//测试

            int Count = htn.Count;
            fGridDetail.InsertRow(num, Count);
            num = 4; 
            foreach (EndAccountAuditDetail dt in htn)
            {
                //结算月份？
                fGridDetail.Cell(num, 1).Text = "1." + (i + 1).ToString();//序号
                fGridDetail.Cell(num, 2).Text = ClientUtil.ToString(dt.BalanceTaskName);//分包工作内容
                fGridDetail.Cell(num, 3).Text = ClientUtil.ToString(dt.BalacneQuantity);// 工程量 对应 结算数量
                fGridDetail.Cell(num, 4).Text = ClientUtil.ToString(dt.BalancePrice);//结算单价
                fGridDetail.Cell(num, 5).Text = ClientUtil.ToString(dt.BalanceTotalPrice);//结算合价
                fGridDetail.Cell(num, 6).Text = ClientUtil.ToString(dt.QuantityUnitName);//结算单位
                fGridDetail.Cell(num, 7).Text = ClientUtil.ToString(dt.BalanceBase);// 结算依据
                fGridDetail.Cell(num, 9).Text = ClientUtil.ToString(dt.Descript);// 备注
                fGridDetail.Cell(num, 10).Text = ClientUtil.ToString(dt.Descript);// 审减工程量
                fGridDetail.Cell(num, 11).Text = ClientUtil.ToString(dt.Descript);// 审减单价
                fGridDetail.Cell(num, 12).Text = ClientUtil.ToString(dt.Descript);// 审减金额
                fGridDetail.Cell(num, 13).Text = ClientUtil.ToString(dt.Descript);// 审核意见
                i++;
                num++;
                BalanceTotalPrice += dt.BalanceTotalPrice;
                LittleBalanceTotalPrice += dt.BalanceTotalPrice;
            }
            #endregion

            #region  设计变更

            //MessageBox.Show("设计变更明细的个数：" + sjbg.Count.ToString());
            //i = 0;
            //Count = 2;//假设htn.Count为2
            //fGridMain.InsertRow(5 + Count, 2);//测试，插入2列，2代表数据的行数
            //num = 5 + Count;//测试
            //fGridMain.Cell(num, 1).Text = "2." + (i + 1).ToString();//测试


            i = 0;
            Count = sjbg.Count;
            fGridDetail.InsertRow(5 + Count, sjbg.Count);
            num = 5 + Count;
            foreach (EndAccountAuditDetail dt in sjbg)
            {
                //结算月份？
                fGridDetail.Cell(num, 1).Text = "2." + (i + 1).ToString();//序号
                fGridDetail.Cell(num, 2).Text = ClientUtil.ToString(dt.BalanceTaskName);//分包工作内容
                fGridDetail.Cell(num, 3).Text = ClientUtil.ToString(dt.BalacneQuantity);// 工程量 对应 结算数量
                fGridDetail.Cell(num, 4).Text = ClientUtil.ToString(dt.BalancePrice);//结算单价
                fGridDetail.Cell(num, 5).Text = ClientUtil.ToString(dt.BalanceTotalPrice);//结算合价
                fGridDetail.Cell(num, 6).Text = ClientUtil.ToString(dt.QuantityUnitName);//结算单位
                fGridDetail.Cell(num, 7).Text = ClientUtil.ToString(dt.BalanceBase);// 结算依据
                fGridDetail.Cell(num, 9).Text = ClientUtil.ToString(dt.Descript);// 备注

                fGridDetail.Cell(num, 10).Text = ClientUtil.ToString(dt.Descript);// 审减工程量
                fGridDetail.Cell(num, 11).Text = ClientUtil.ToString(dt.Descript);// 审减单价
                fGridDetail.Cell(num, 12).Text = ClientUtil.ToString(dt.Descript);// 审减金额
                fGridDetail.Cell(num, 13).Text = ClientUtil.ToString(dt.Descript);// 审核意见
                i++;
                num++;
                BalanceTotalPrice += dt.BalanceTotalPrice;
                LittleBalanceTotalPrice += dt.BalanceTotalPrice;
            }
            #endregion

            #region  签证索赔
            //MessageBox.Show("签证索赔明细的个数：" + qzsp.Count.ToString());
            //i = 0;
            //Count = 2 + 2;//假设htn.Count为2，sjbg.Count为2
            //fGridMain.InsertRow(6 + Count, 2);//测试，插入2列，2代表数据的行数
            //num = 6 + Count;//测试
            //fGridMain.Cell(num, 1).Text = "3." + (i + 1).ToString();//测试

            i = 0;
            Count = htn.Count + sjbg.Count;
            fGridDetail.InsertRow(6 + Count, qzsp.Count);
            num = 6 + Count;
            foreach (EndAccountAuditDetail dt in qzsp)
            {
                //结算月份？
                fGridDetail.Cell(num, 1).Text = "3." + (i + 1).ToString();//序号
                fGridDetail.Cell(num, 2).Text = ClientUtil.ToString(dt.BalanceTaskName);//分包工作内容
                fGridDetail.Cell(num, 3).Text = ClientUtil.ToString(dt.BalacneQuantity);// 工程量 对应 结算数量
                fGridDetail.Cell(num, 4).Text = ClientUtil.ToString(dt.BalancePrice);//结算单价
                fGridDetail.Cell(num, 5).Text = ClientUtil.ToString(dt.BalanceTotalPrice);//结算合价
                fGridDetail.Cell(num, 6).Text = ClientUtil.ToString(dt.QuantityUnitName);//结算单位
                fGridDetail.Cell(num, 7).Text = ClientUtil.ToString(dt.BalanceBase);// 结算依据
                fGridDetail.Cell(num, 9).Text = ClientUtil.ToString(dt.Descript);// 备注

                fGridDetail.Cell(num, 10).Text = ClientUtil.ToString(dt.Descript);// 审减工程量
                fGridDetail.Cell(num, 11).Text = ClientUtil.ToString(dt.Descript);// 审减单价
                fGridDetail.Cell(num, 12).Text = ClientUtil.ToString(dt.Descript);// 审减金额
                fGridDetail.Cell(num, 13).Text = ClientUtil.ToString(dt.Descript);// 审核意见
                i++;
                num++;
                BalanceTotalPrice += dt.BalanceTotalPrice;
                LittleBalanceTotalPrice += dt.BalanceTotalPrice;
            }

            #endregion

            #region  计时工
            //MessageBox.Show("计时工明细的个数：" + jsg.Count.ToString());

            //i = 0;
            //Count = 2 + 2 + 2;//假设htn.Count为2，sjbg.Count为2，qzsp.Count为2
            //fGridMain.InsertRow(7 + Count, 2);//测试，插入2列，2代表数据的行数
            //num = 7 + Count;//测试
            //fGridMain.Cell(num, 1).Text = "4." + (i + 1).ToString();//测试

            i = 0;
            Count = htn.Count + sjbg.Count + qzsp.Count;
            fGridDetail.InsertRow(7 + Count, jsg.Count);
            num = 7 + Count;
            foreach (EndAccountAuditDetail dt in jsg)
            {
                //结算月份？
                fGridDetail.Cell(num, 1).Text = "4." + (i + 1).ToString();//序号
                fGridDetail.Cell(num, 2).Text = ClientUtil.ToString(dt.BalanceTaskName);//分包工作内容
                fGridDetail.Cell(num, 3).Text = ClientUtil.ToString(dt.BalacneQuantity);// 工程量 对应 结算数量
                fGridDetail.Cell(num, 4).Text = ClientUtil.ToString(dt.BalancePrice);//结算单价
                fGridDetail.Cell(num, 5).Text = ClientUtil.ToString(dt.BalanceTotalPrice);//结算合价
                fGridDetail.Cell(num, 6).Text = ClientUtil.ToString(dt.QuantityUnitName);//结算单位
                fGridDetail.Cell(num, 7).Text = ClientUtil.ToString(dt.BalanceBase);// 结算依据
                fGridDetail.Cell(num, 9).Text = ClientUtil.ToString(dt.Descript);// 备注

                fGridDetail.Cell(num, 10).Text = ClientUtil.ToString(dt.Descript);// 审减工程量
                fGridDetail.Cell(num, 11).Text = ClientUtil.ToString(dt.Descript);// 审减单价
                fGridDetail.Cell(num, 12).Text = ClientUtil.ToString(dt.Descript);// 审减金额
                fGridDetail.Cell(num, 13).Text = ClientUtil.ToString(dt.Descript);// 审核意见
                i++;
                num++;
                BalanceTotalPrice += dt.BalanceTotalPrice;
                LittleBalanceTotalPrice += dt.BalanceTotalPrice;
            }

            #endregion

            #region  分包签证
            //MessageBox.Show("分包签证明细的个数：" + fbqz.Count.ToString());
            //i = 0;
            //Count = 2 + 2 + 2 + 2;//假设htn.Count为2，sjbg.Count为2，qzsp.Count为2，jsg.Count
            //fGridMain.InsertRow(8 + Count, 2);//测试，插入2列，2代表数据的行数
            //num = 8 + Count;//测试
            //fGridMain.Cell(num, 1).Text = "5." + (i + 1).ToString();//测试

            i = 0;
            Count = htn.Count + sjbg.Count + qzsp.Count + jsg.Count;
            fGridDetail.InsertRow(8 + Count, fbqz.Count);
            num = 8 + Count;
            foreach (EndAccountAuditDetail dt in fbqz)
            {
                //结算月份？
                fGridDetail.Cell(num, 1).Text = "5." + (i + 1).ToString();//序号
                fGridDetail.Cell(num, 2).Text = ClientUtil.ToString(dt.BalanceTaskName);//分包工作内容
                fGridDetail.Cell(num, 3).Text = ClientUtil.ToString(dt.BalacneQuantity);// 工程量 对应 结算数量
                fGridDetail.Cell(num, 4).Text = ClientUtil.ToString(dt.BalancePrice);//结算单价
                fGridDetail.Cell(num, 5).Text = ClientUtil.ToString(dt.BalanceTotalPrice);//结算合价
                fGridDetail.Cell(num, 6).Text = ClientUtil.ToString(dt.QuantityUnitName);//结算单位
                fGridDetail.Cell(num, 7).Text = ClientUtil.ToString(dt.BalanceBase);// 结算依据
                fGridDetail.Cell(num, 9).Text = ClientUtil.ToString(dt.Descript);// 备注

                fGridDetail.Cell(num, 10).Text = ClientUtil.ToString(dt.Descript);// 审减工程量
                fGridDetail.Cell(num, 11).Text = ClientUtil.ToString(dt.Descript);// 审减单价
                fGridDetail.Cell(num, 12).Text = ClientUtil.ToString(dt.Descript);// 审减金额
                fGridDetail.Cell(num, 13).Text = ClientUtil.ToString(dt.Descript);// 审核意见
                i++;
                num++;
                BalanceTotalPrice += dt.BalanceTotalPrice;
                LittleBalanceTotalPrice += dt.BalanceTotalPrice;
            }

            #endregion

            #region  代工扣款
            //MessageBox.Show(" 代工扣款明细的个数：" + dgkk.Count.ToString());
            //i = 0;
            //Count = 2 + 2 + 2 + 2 + 2;//假设htn.Count为2，sjbg.Count为2，qzsp.Count为2，jsg.Count为2，fbqz.Count为2
            //fGridMain.InsertRow(9 + Count, 2);//测试，插入2列，2代表数据的行数
            //num = 9 + Count;//测试
            //fGridMain.Cell(num, 1).Text = "6." + (i + 1).ToString();//测试


            i = 0;
            Count = htn.Count + sjbg.Count + qzsp.Count + jsg.Count + fbqz.Count;
            fGridDetail.InsertRow(9 + Count, dgkk.Count);
            num = 9 + Count;
            foreach (EndAccountAuditDetail dt in dgkk)
            {
                //结算月份？
                fGridDetail.Cell(num, 1).Text = "6." + (i + 1).ToString();//序号
                fGridDetail.Cell(num, 2).Text = ClientUtil.ToString(dt.BalanceTaskName);//分包工作内容
                fGridDetail.Cell(num, 3).Text = ClientUtil.ToString(dt.BalacneQuantity);// 工程量 对应 结算数量
                fGridDetail.Cell(num, 4).Text = ClientUtil.ToString(dt.BalancePrice);//结算单价
                fGridDetail.Cell(num, 5).Text = ClientUtil.ToString(dt.BalanceTotalPrice);//结算合价
                fGridDetail.Cell(num, 6).Text = ClientUtil.ToString(dt.QuantityUnitName);//结算单位
                fGridDetail.Cell(num, 7).Text = ClientUtil.ToString(dt.BalanceBase);// 结算依据
                fGridDetail.Cell(num, 9).Text = ClientUtil.ToString(dt.Descript);// 备注

                fGridDetail.Cell(num, 10).Text = ClientUtil.ToString(dt.Descript);// 审减工程量
                fGridDetail.Cell(num, 11).Text = ClientUtil.ToString(dt.Descript);// 审减单价
                fGridDetail.Cell(num, 12).Text = ClientUtil.ToString(dt.Descript);// 审减金额
                fGridDetail.Cell(num, 13).Text = ClientUtil.ToString(dt.Descript);// 审核意见
                i++;
                num++;
                BalanceTotalPrice += dt.BalanceTotalPrice;
                LittleBalanceTotalPrice += dt.BalanceTotalPrice;
            }

            #endregion

            #region  罚款
            //MessageBox.Show(" 罚款明细的个数：" + fk.Count.ToString());

            //i = 0;
            //Count = 2 + 2 + 2 + 2 + 2 + 2;//假设htn.Count为2，sjbg.Count为2，qzsp.Count为2，jsg.Count为2，fbqz.Count为2，dgkk.Count为2
            //fGridMain.InsertRow(10 + Count, 2);//测试，插入2列，2代表数据的行数
            //num = 10 + Count;//测试
            //fGridMain.Cell(num, 1).Text = "7." + (i + 1).ToString();//测试

            i = 0;
            Count = htn.Count + sjbg.Count + qzsp.Count + jsg.Count + fbqz.Count + dgkk.Count; 
            fGridDetail.InsertRow(10 + Count, fk.Count); 
            num = 10 + Count; 
            foreach (EndAccountAuditDetail dt in fk)
            {
                //结算月份？
                fGridDetail.Cell(num, 1).Text = "7." + (i + 1).ToString();//序号
                fGridDetail.Cell(num, 2).Text = ClientUtil.ToString(dt.BalanceTaskName);//分包工作内容
                fGridDetail.Cell(num, 3).Text = ClientUtil.ToString(dt.BalacneQuantity);// 工程量 对应 结算数量
                fGridDetail.Cell(num, 4).Text = ClientUtil.ToString(dt.BalancePrice);//结算单价
                fGridDetail.Cell(num, 5).Text = ClientUtil.ToString(dt.BalanceTotalPrice);//结算合价
                fGridDetail.Cell(num, 6).Text = ClientUtil.ToString(dt.QuantityUnitName);//结算单位
                fGridDetail.Cell(num, 7).Text = ClientUtil.ToString(dt.BalanceBase);// 结算依据
                fGridDetail.Cell(num, 9).Text = ClientUtil.ToString(dt.Descript);// 备注

                fGridDetail.Cell(num, 10).Text = ClientUtil.ToString(dt.Descript);// 审减工程量
                fGridDetail.Cell(num, 11).Text = ClientUtil.ToString(dt.Descript);// 审减单价
                fGridDetail.Cell(num, 12).Text = ClientUtil.ToString(dt.Descript);// 审减金额
                fGridDetail.Cell(num, 13).Text = ClientUtil.ToString(dt.Descript);// 审核意见
                i++;
                num++;
                BalanceTotalPrice += dt.BalanceTotalPrice;
                LittleBalanceTotalPrice += dt.BalanceTotalPrice;
            }
            #endregion

           

            #region 其它
            //MessageBox.Show(" 其它明细的个数：" + qt.Count.ToString());

            //i = 0;
            //Count = 2 + 2 + 2 + 2 + 2 + 2 + 2;//假设htn.Count为2，sjbg.Count为2，qzsp.Count为2，jsg.Count为2，fbqz.Count为2，dgkk.Count为2，fk.Count为2
            //fGridMain.InsertRow(10 + Count, 1);//测试，插入2列，2代表数据的行数
            //num = 10 + Count;//测试
            //fGridMain.Cell(num, 1).Text = "八";//测试
            //fGridMain.Cell(num, 2).Text = "其它";//测试

            //i = 0;
            //Count = 2 + 2 + 2 + 2 + 2 + 2 + 2 + 1;//假设htn.Count为2，sjbg.Count为2，qzsp.Count为2，jsg.Count为2，fbqz.Count为2，dgkk.Count为2，fk.Count为2
            //fGridMain.InsertRow(10 + Count, 2);//测试，插入2列，2代表数据的行数
            //num = 10 + Count;//测试
            //fGridMain.Cell(num, 1).Text = "8." + (i + 1).ToString();//测试


            i = 0;
            Count = htn.Count + sjbg.Count + qzsp.Count + jsg.Count + fbqz.Count + dgkk.Count + fk.Count;
            fGridDetail.InsertRow(10 + Count, 1); 
            num = 10 + Count; 
            fGridDetail.Cell(num, 1).Text = "八"; 
            fGridDetail.Cell(num, 2).Text = "其它"; 

            i = 0;
            Count = htn.Count + sjbg.Count + qzsp.Count + jsg.Count + fbqz.Count + dgkk.Count + fk.Count +1; 
            fGridDetail.InsertRow(10 + Count, qt.Count); 
            num = 10 + Count;
            foreach (EndAccountAuditDetail dt in qt)
            {
                //结算月份？
                fGridDetail.Cell(num, 1).Text = "8." + (i + 1).ToString();//序号
                fGridDetail.Cell(num, 2).Text = ClientUtil.ToString(dt.BalanceTaskName);//分包工作内容
                fGridDetail.Cell(num, 3).Text = ClientUtil.ToString(dt.BalacneQuantity);// 工程量 对应 结算数量
                fGridDetail.Cell(num, 4).Text = ClientUtil.ToString(dt.BalancePrice);//结算单价
                fGridDetail.Cell(num, 5).Text = ClientUtil.ToString(dt.BalanceTotalPrice);//结算合价
                fGridDetail.Cell(num, 6).Text = ClientUtil.ToString(dt.QuantityUnitName);//结算单位
                fGridDetail.Cell(num, 7).Text = ClientUtil.ToString(dt.BalanceBase);// 结算依据
                fGridDetail.Cell(num, 9).Text = ClientUtil.ToString(dt.Descript);// 备注

                fGridDetail.Cell(num, 10).Text = ClientUtil.ToString(dt.Descript);// 审减工程量
                fGridDetail.Cell(num, 11).Text = ClientUtil.ToString(dt.Descript);// 审减单价
                fGridDetail.Cell(num, 12).Text = ClientUtil.ToString(dt.Descript);// 审减金额
                fGridDetail.Cell(num, 13).Text = ClientUtil.ToString(dt.Descript);// 审核意见
                i++;
                num++;
                BalanceTotalPrice += dt.BalanceTotalPrice;
                LittleBalanceTotalPrice += dt.BalanceTotalPrice;
            }
            #endregion
            #region 给小计赋值
            int col = 10 + htn.Count + sjbg.Count + qzsp.Count + jsg.Count + fbqz.Count + dgkk.Count + fk.Count + 1 + qt.Count;//假设htn.Count为2，sjbg.Count为2，qzsp.Count为2，jsg.Count为2，fbqz.Count为2，dgkk.Count为2，fk.Count为2
            fGridDetail.Cell(col, 5).Text = ClientUtil.ToString(LittleBalanceTotalPrice);//小计的结算合价
            #endregion
            #region  水电费，建管费
            //MessageBox.Show(" 水电费，建管费明细的个数：" + sdf.Count.ToString());
            //i = 0;
            //Count = 2 + 2 + 2 + 2 + 2 + 2 + 2 + 1+2+2;//假设htn.Count为2，sjbg.Count为2，qzsp.Count为2，jsg.Count为2，fbqz.Count为2，dgkk.Count为2，fk.Count为2,qt.Count为2
            //fGridMain.InsertRow(10 + Count, 2);//测试，插入2列，2代表数据的行数
            //num = 10 + Count;//测试
            //fGridMain.Cell(num, 1).Text = "9." + (i + 1).ToString();//测试

            i = 0;
            Count = htn.Count + sjbg.Count + qzsp.Count + jsg.Count + fbqz.Count + dgkk.Count + fk.Count + 1 +qt.Count +2 ; 
            fGridDetail.InsertRow(10 + Count, sdf.Count); 
            num = 10 + Count; 
            foreach (EndAccountAuditDetail dt in sdf)
            {
                //结算月份？
                fGridDetail.Cell(num, 1).Text = (i + 1).ToString();//序号
                fGridDetail.Cell(num, 2).Text = ClientUtil.ToString(dt.BalanceTaskName);//分包工作内容
                fGridDetail.Cell(num, 3).Text = ClientUtil.ToString(dt.BalacneQuantity);// 工程量 对应 结算数量
                fGridDetail.Cell(num, 4).Text = ClientUtil.ToString(dt.BalancePrice);//结算单价
                fGridDetail.Cell(num, 5).Text = ClientUtil.ToString(dt.BalanceTotalPrice);//结算合价
                fGridDetail.Cell(num, 6).Text = ClientUtil.ToString(dt.QuantityUnitName);//结算单位
                fGridDetail.Cell(num, 7).Text = ClientUtil.ToString(dt.BalanceBase);// 结算依据
                fGridDetail.Cell(num, 9).Text = ClientUtil.ToString(dt.Descript);// 备注

                fGridDetail.Cell(num, 10).Text = ClientUtil.ToString(dt.Descript);// 审减工程量
                fGridDetail.Cell(num, 11).Text = ClientUtil.ToString(dt.Descript);// 审减单价
                fGridDetail.Cell(num, 12).Text = ClientUtil.ToString(dt.Descript);// 审减金额
                fGridDetail.Cell(num, 13).Text = ClientUtil.ToString(dt.Descript);// 审核意见
                i++;
                num++;
                BalanceTotalPrice += dt.BalanceTotalPrice;
            }
            #endregion


            #region 给合计赋值
            int colTotal = 10 + htn.Count + sjbg.Count + qzsp.Count + jsg.Count + fbqz.Count + dgkk.Count + fk.Count + 1 + qt.Count + 2 + sdf.Count;//假设htn.Count为2，sjbg.Count为2，qzsp.Count为2，jsg.Count为2，fbqz.Count为2，dgkk.Count为2，fk.Count为2
            fGridDetail.Cell(colTotal, 5).Text = ClientUtil.ToString(BalanceTotalPrice);//小计的结算合价
            #endregion

            #endregion
            #endregion


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
