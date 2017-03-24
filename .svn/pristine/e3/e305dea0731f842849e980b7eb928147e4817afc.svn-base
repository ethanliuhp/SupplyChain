using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Util;
using IRPServiceModel.Services.Common;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using System.Linq;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement
{
    public partial class VSceneManagementReport : TBasicDataView
    {
        ICommonMethodSrv service = CommonMethod.CommonMethodSrv;
        string detailExptr = "现场管理费分析对比表";
        string flexName = "现场管理费分析对比表.flx";

        string detailExptrMeasures = "措施费成本分析对比表";
        string flexNameMeasures = "措施费成本分析对比表.flx";


        CurrentProjectInfo projectInfo;

        public VSceneManagementReport()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }

        private void InitData()
        {
            #region   给年份和月份的下拉列表框赋值
            this.comYear.Items.Clear();
            this.comMonth.Items.Clear();
            for (int i = 0; i < 13; i++)
            {
                this.comYear.Items.Add(ConstObject.TheLogin.TheComponentPeriod.NowYear + (i - 6));
            }
            for (int i = 1; i < 13; i++)
            {
                this.comMonth.Items.Add(i);
            }
            this.comYear.Text = DateTime.Now.Year.ToString();
            this.comMonth.Text = DateTime.Now.Month.ToString();
            #endregion

            this.fGridDetail.Rows = 1;

            this.fGridMeasures.Rows = 1;//措施费的列表展示
           
            LoadTempleteFile(flexName);//现场管理费分析对比表
            LoadTempleteFileMeasures(flexNameMeasures);//措施费成本分析对比表

            projectInfo = StaticMethod.GetProjectInfo();//项目相关信息
        }

        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            VContractExcuteSelector vmros = new VContractExcuteSelector();
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject engineerMaster = list[0] as SubContractProject;
        }


        void btnExcel_Click(object sender, EventArgs e)
        {
            fGridDetail.ExportToExcel(detailExptr, false, false, true);

            fGridMeasures.ExportToExcel(detailExptrMeasures, false, false, true);//20160822
        }

        private void LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式
                if (modelName == flexName)
                {
                    fGridDetail.OpenFile(path + "\\" + modelName);//载入格式
                }
            }
            else
            {
                MessageBox.Show("未找到模板格式文件" + modelName);
                return;
            }
        }


        private void LoadTempleteFileMeasures(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式
                if (modelName == flexNameMeasures)
                {
                    fGridMeasures.OpenFile(path + "\\" + modelName);//载入格式
                }
            }
            else
            {
                MessageBox.Show("未找到模板格式文件" + modelName);
                return;
            }
        }




        private void btnQuery_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("正在生成[" + detailExptr + "]报告...");
            LoadTempleteFile(flexName);
            LoadDetailFile();

            LoadTempleteFileMeasures(flexNameMeasures);//措施费
            LoadDetailFileMeasures();//措施费
            FlashScreen.Close();
        }


        private void LoadDetailFileMeasures()
        {
           // FlashScreen.Show("正在生成[" + detailExptrMeasures + "]报告...");
            try
            {
                fGridMeasures.AutoRedraw = false;


                #region  组装查询条件 （现场管理费分析对比表）
                var condition = " and t1.theprojectguid='" + projectInfo.Id + "'  and t3.COSTSUBJECTCODE LIKE  'C512%'          ";//措施费C512
                if (!string.IsNullOrEmpty(this.comYear.Text))//年份
                {
                    condition += " and t1.kjn='" + this.comYear.Text + "'";
                }
                if (!string.IsNullOrEmpty(this.comMonth.Text))//月份
                {
                    condition += " and t1.kjy='" + this.comMonth.Text + "'";
                }

                #endregion

                #region 需要取到的值（现场管理费分析对比表）


                //项目名称(对应 projectInfo.Name)     日期(当天的日期)
                //序号（自己编号）    费用项目        (本期数的)责任成本  (本期数的)实际成本  (本期数的)节超额       (累计数的)责任成本  (累计数的)实际成本 (累计数的)节超额   (累计数的)节超比例
                //项目经理          项目预算员（商务经理）   项目成本员

                // string sql = "select t3.*  from thd_costmonthaccount t1,thd_costmonthaccountdtl t2,thd_costmonthaccdtlconsume t3 where t1.id=t2.parentid and t2.id=t3.parentid  " + condition + " order by t3.costsubjectcode";//不分组的查询
                decimal dJCMoney = 0, dSumRespon = 0;
                string sql = "select t3.COSTINGSUBJECTNAME as  COSTINGSUBJECTNAME, SUM(t3.CURRRESPONSICONSUMETOTALPRICE) as  CURRRESPONSICONSUMETOTALPRICE, SUM(t3.CURRREALCONSUMETOTALPRICE) as  CURRREALCONSUMETOTALPRICE, SUM(t3.SUMRESPONSICONSUMETOTALPRICE) as  SUMRESPONSICONSUMETOTALPRICE,  SUM(t3.SUMREALCONSUMETOTALPRICE) as  SUMREALCONSUMETOTALPRICE    from thd_costmonthaccount t1,thd_costmonthaccountdtl t2,thd_costmonthaccdtlconsume t3 where t1.id=t2.parentid and t2.id=t3.parentid  " + condition + " GROUP BY  t3.COSTINGSUBJECTNAME,t3.costsubjectcode ";//分组的查询

                var dt = service.GetData(sql).Tables[0];
                if (dt == null && dt.Rows.Count == 0) return;
                fGridMeasures.Cell(3, 1).Text = "项目名称：" + projectInfo.Name;//项目名称,位于模板的第3行第1列
                fGridMeasures.Cell(3, 6).Text = "日期：" + DateTime.Now.Year + "年  " + DateTime.Now.Month + "月  " + DateTime.Now.Day + "日";//日  期：  年  月  日,位于模板的第3行第6列


                string SumAccountMoney = "";//需要从查询结果中取值
                fGridMeasures.Cell(3, 9).Text = "单位：" + SumAccountMoney + "元";//单位：元,位于模板的第2行第8列

                fGridMeasures.InsertRow(6, dt.Rows.Count - 1);          // 插入用于存储所有单位的行数   
                int num = 6;//模板从第6行开始赋值
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    //int i = 45;
                    fGridMeasures.Cell(num, 1).Text = (i + 1).ToString();//序号
                    fGridMeasures.Cell(num, 2).Text = dt.Rows[i]["COSTINGSUBJECTNAME"].ToString();//费用项目

                    fGridMeasures.Cell(num, 3).Text = dt.Rows[i]["CURRRESPONSICONSUMETOTALPRICE"].ToString(); //(本期数的)责任成本  
                    fGridMeasures.Cell(num, 4).Text = dt.Rows[i]["CURRREALCONSUMETOTALPRICE"].ToString();  //(本期数的)实际成本
                    //节超=责任-实际
                    dJCMoney = ClientUtil.ToDecimal(dt.Rows[i]["CURRRESPONSICONSUMETOTALPRICE"]) - ClientUtil.ToDecimal(dt.Rows[i]["CURRREALCONSUMETOTALPRICE"]);
                    fGridMeasures.Cell(num, 5).Text = dJCMoney.ToString();// (decimal.Parse(dt.Rows[i]["CURRREALCONSUMETOTALPRICE"].ToString()) - decimal.Parse(dt.Rows[i]["CURRRESPONSICONSUMETOTALPRICE"].ToString())).ToString();  //(本期数的)节超额 
                    if (dJCMoney < 0) fGridMeasures.Cell(num, 5).BackColor =   System.Drawing.Color.Red ;

                    fGridMeasures.Cell(num, 6).Text = dt.Rows[i]["SUMRESPONSICONSUMETOTALPRICE"].ToString();//(累计数的)责任成本 
                    fGridMeasures.Cell(num, 7).Text = dt.Rows[i]["SUMREALCONSUMETOTALPRICE"].ToString();//(累计数的)实际成本 
                    //节超=责任-实际
                    dJCMoney = ClientUtil.ToDecimal(dt.Rows[i]["SUMRESPONSICONSUMETOTALPRICE"]) - ClientUtil.ToDecimal(dt.Rows[i]["SUMREALCONSUMETOTALPRICE"]);
                    //string otm = (decimal.Parse(dt.Rows[i]["SUMREALCONSUMETOTALPRICE"].ToString()) - decimal.Parse(dt.Rows[i]["SUMRESPONSICONSUMETOTALPRICE"].ToString())).ToString();//(累计数的)节超额  也就是  实际成本 - 责任成本
                    fGridMeasures.Cell(num, 8).Text = dJCMoney.ToString();//(累计数的)节超额
                   if(dJCMoney < 0 ) fGridMeasures.Cell(num, 8).BackColor = System.Drawing.Color.Red ;

                    //string ljzrcb = dt.Rows[i]["SUMRESPONSICONSUMETOTALPRICE"].ToString(); //(累计数的)责任成本 
                    dSumRespon = ClientUtil.ToDecimal(dt.Rows[i]["SUMRESPONSICONSUMETOTALPRICE"]);
                    //if ((ljzrcb != "0") && (ljzrcb.Trim().Length > 0))
                    if (dSumRespon!=0)
                    {
                        //（实际成本 - 责任成本）/ 责任成本
                        //(责任-实际)/责任

                       // Decimal m = Convert.ToDecimal(decimal.Parse(otm) / decimal.Parse(ljzrcb));

                        //fGridDetail.Cell(num, 9).Text = Convert.ToDecimal(decimal.Parse(otm) / decimal.Parse(ljzrcb)).ToString();//(累计数的)节超比例 ,用(累计数的)节超额 除以 (累计数的)责任成本
                        fGridMeasures.Cell(num, 9).Text =Math.Round((dJCMoney * 100) / dSumRespon,2).ToString("N2") + "%";  //精确到小数点后2位
                    }
                    else
                    {
                        fGridMeasures.Cell(num, 9).Text = "0";
                    }

                    num++;

                }

                #region  这3个暂时不需要管 ,最后在模板后面插入一行即可
                fGridMeasures.Cell(num, 1).Text = "项目经理:";
                fGridMeasures.Cell(num, 4).Text = "项目预算员（商务经理):";
                fGridMeasures.Cell(num, 7).Text = "项目成本员:";
                //项目经理          
                //项目预算员（商务经理）   
                //项目成本员
                #endregion

                #endregion

                
            }
            catch (Exception e1)
            {
                throw new Exception("生成[" + detailExptrMeasures + "]报告异常[" + e1.Message + "]");
            }
            finally
            {
                fGridMeasures.BackColor1 = System.Drawing.SystemColors.ButtonFace;
                fGridMeasures.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
                FlexCell.Range oRange = fGridMeasures.Range(1, 1, fGridMeasures.Rows - 1, fGridMeasures.Cols - 1);
                oRange.Locked = true;
                fGridMeasures.SelectionMode = FlexCell.SelectionModeEnum.Free;
                fGridMeasures.AutoRedraw = true;
                fGridMeasures.Refresh();
               
            }

        }

        private void LoadDetailFile()
        {
           
            try
            {
                fGridDetail.AutoRedraw = false;
               

                #region  组装查询条件 （现场管理费分析对比表）
                var condition = " and t1.theprojectguid='" + projectInfo.Id + "'  and t3.COSTSUBJECTCODE LIKE  'C513%'          ";//现场管理费C513
                if (!string.IsNullOrEmpty(this.comYear.Text))//年份
                {
                    condition += " and t1.kjn='" + this.comYear.Text + "'";
                }
                if (!string.IsNullOrEmpty(this.comMonth.Text))//月份
                {
                    condition += " and t1.kjy='" + this.comMonth.Text + "'";
                }

                #endregion

                #region 需要取到的值（现场管理费分析对比表）


                //项目名称(对应 projectInfo.Name)     日期(当天的日期)
                //序号（自己编号）    费用项目        (本期数的)责任成本  (本期数的)实际成本  (本期数的)节超额       (累计数的)责任成本  (累计数的)实际成本 (累计数的)节超额   (累计数的)节超比例
                //项目经理          项目预算员（商务经理）   项目成本员

               // string sql = "select t3.*  from thd_costmonthaccount t1,thd_costmonthaccountdtl t2,thd_costmonthaccdtlconsume t3 where t1.id=t2.parentid and t2.id=t3.parentid  " + condition + " order by t3.costsubjectcode";//不分组的查询

                string sql = "select t3.COSTINGSUBJECTNAME as  COSTINGSUBJECTNAME, SUM(t3.CURRRESPONSICONSUMETOTALPRICE) as  CURRRESPONSICONSUMETOTALPRICE, SUM(t3.CURRREALCONSUMETOTALPRICE) as  CURRREALCONSUMETOTALPRICE, SUM(t3.SUMRESPONSICONSUMETOTALPRICE) as  SUMRESPONSICONSUMETOTALPRICE,  SUM(t3.SUMREALCONSUMETOTALPRICE) as  SUMREALCONSUMETOTALPRICE    from thd_costmonthaccount t1,thd_costmonthaccountdtl t2,thd_costmonthaccdtlconsume t3 where t1.id=t2.parentid and t2.id=t3.parentid  " + condition + " GROUP BY  t3.COSTINGSUBJECTNAME,t3.costsubjectcode ";//分组的查询
                decimal dJCMoney = 0,dSumRepso=0;
                var dt = service.GetData(sql).Tables[0];
                if (dt == null && dt.Rows.Count == 0) return;
                fGridDetail.Cell(3, 1).Text = "项目名称：" +  projectInfo.Name;//项目名称,位于模板的第3行第1列
                fGridDetail.Cell(3, 6).Text = "日期：" + DateTime.Now.Year + "年  " + DateTime.Now.Month + "月  " + DateTime.Now.Day + "日";//日  期：  年  月  日,位于模板的第3行第6列


                string SumAccountMoney = "";//需要从查询结果中取值
                fGridDetail.Cell(3, 9).Text = "单位：" + SumAccountMoney + "元";//单位：元,位于模板的第2行第8列

                fGridDetail.InsertRow(6, dt.Rows.Count - 1);          // 插入用于存储所有单位的行数   
                int num = 6;//模板从第6行开始赋值
                for (int i = 0; i < dt.Rows.Count;i++ )
                {

                    //int i = 45;
                    fGridDetail.Cell(num, 1).Text = (i + 1).ToString();//序号
                    fGridDetail.Cell(num, 2).Text = dt.Rows[i]["COSTINGSUBJECTNAME"].ToString();//费用项目

                    fGridDetail.Cell(num, 3).Text = dt.Rows[i]["CURRRESPONSICONSUMETOTALPRICE"].ToString(); //(本期数的)责任成本  
                    fGridDetail.Cell(num, 4).Text = dt.Rows[i]["CURRREALCONSUMETOTALPRICE"].ToString();  //(本期数的)实际成本
                    //责任-实际成本
                    dJCMoney =ClientUtil.ToDecimal(dt.Rows[i]["CURRRESPONSICONSUMETOTALPRICE"])- ClientUtil.ToDecimal(dt.Rows[i]["CURRREALCONSUMETOTALPRICE"] ) ;
                    fGridDetail.Cell(num, 5).Text = dJCMoney.ToString();  //(本期数的)节超额 
                    if (dJCMoney < 0) fGridDetail.Cell(num, 5).BackColor =   System.Drawing.Color.Red  ;
                 


                    fGridDetail.Cell(num, 6).Text = dt.Rows[i]["SUMRESPONSICONSUMETOTALPRICE"].ToString();//(累计数的)责任成本 
                    fGridDetail.Cell(num, 7).Text = dt.Rows[i]["SUMREALCONSUMETOTALPRICE"].ToString();//(累计数的)实际成本 
                    //责任-实际成本
                    dJCMoney = ClientUtil.ToDecimal(dt.Rows[i]["SUMRESPONSICONSUMETOTALPRICE"]) - ClientUtil.ToDecimal(dt.Rows[i]["SUMREALCONSUMETOTALPRICE"]);//(累计数的)节超额  也就是   
                    fGridDetail.Cell(num, 8).Text = dJCMoney.ToString();//(累计数的)节超额
                    if (dJCMoney < 0) fGridDetail.Cell(num, 8).BackColor =   System.Drawing.Color.Red  ;
                    dSumRepso =ClientUtil.ToDecimal( dt.Rows[i]["SUMRESPONSICONSUMETOTALPRICE"] ); //(累计数的)责任成本 

                    if (dSumRepso!=0)
                    {
                        //（实际成本 - 责任成本）/ 责任成本


                       // Decimal m = Convert.ToDecimal(decimal.Parse(otm) / decimal.Parse(ljzrcb));

                        //fGridDetail.Cell(num, 9).Text = Convert.ToDecimal(decimal.Parse(otm) / decimal.Parse(ljzrcb)).ToString();//(累计数的)节超比例 ,用(累计数的)节超额 除以 (累计数的)责任成本
                        fGridDetail.Cell(num, 9).Text = Math.Round( (dJCMoney / dSumRepso * 100),2).ToString("N2") + "%";  //精确到小数点后2位
                    }
                    else
                    {
                        fGridDetail.Cell(num, 9).Text = "0";
                    }
                  
                    num++;

                }

                #region  这3个暂时不需要管 ,最后在模板后面插入一行即可
                fGridDetail.Cell(num, 1).Text = "项目经理:";
                fGridDetail.Cell(num, 4).Text = "项目预算员（商务经理):";
                fGridDetail.Cell(num, 7).Text = "项目成本员:";
                //项目经理          
                    //项目预算员（商务经理）   
                //项目成本员
                #endregion

                #endregion

                #region   DEMO

                ////// 首先取出统计的时间段
                //TimeSlice ts = new TimeSlice(Convert.ToInt32(cbYear.Text));

                //var startTime = ts.Slice[0].Value[0];
                //var endTime = ts.Slice[11].Value[1];

                //// 其次查询出所有的统计数据
                //var condition = string.Format(" and submitdate<=to_date('{0}','yyyy-mm-dd')", endTime.ToString("yyyy-MM-dd"));
                //if (!string.IsNullOrEmpty(txtSupply.Text))
                //{
                //    condition += " and supplierrelation='" + (this.txtSupply.Result[0] as SupplierRelationInfo).Id + "'";
                //}
                //string sql = "select suppliername name, supplierrelation id,summoney money,submitdate time from thd_materialrentelsetmaster where state=5 and projectid='" + projectInfo.Id + "'" + condition + " order by createdate desc";
                //var dt = service.GetData(sql).Tables[0];

          
                //if (dt == null && dt.Rows.Count == 0) return;
                //var result = dt.Select().Select(a => new { CreateTime = Convert.ToDateTime(a["time"]), Name = a["name"].ToString(), Money = Convert.ToDecimal(a["money"]), Id = a["id"].ToString() });
                //var resultGroup = result.GroupBy(a => a.Id);                // 根据单位id分组
                //fGridDetail.InsertRow(6, resultGroup.Count() - 1);          // 插入用于存储所有单位的行数

                //int num = 0;
                //foreach (var item in resultGroup)
                //{
                //    var departmentResult = result.Where(a => a.Id == item.Key).ToList();
                //    var tempResult = departmentResult.Where(a => a.CreateTime >= startTime && a.CreateTime <= endTime).ToList();
                //    decimal total = departmentResult.Where(a => a.CreateTime < startTime).Sum(a => a.Money);
                //    fGridDetail.Cell(5 + num, 1).Text = (num + 1).ToString();
                //    fGridDetail.Cell(5 + num, 2).Text = departmentResult[0].Name;
                //    foreach (var time in ts.Slice)
                //    {
                //        if (time.Value[0] > DateTime.Now) break;
                //        var summary = tempResult.Where(a => a.CreateTime >= time.Value[0] && a.CreateTime <= time.Value[1]).Sum(a => a.Money);
                //        total += summary;
                //        fGridDetail.Cell(5 + num, time.Key * 2 + 1).Text = summary.ToString();
                //        fGridDetail.Cell(5 + num, time.Key * 2 + 2).Text = total.ToString();
                //    }
                //    num++;
                //}

                //string sTitle = string.Format("{0}年 {1}项目机械租赁结算台帐", cbYear.Text, projectInfo.Name);
                //fGridDetail.Cell(1, 1).Text = sTitle;
                //fGridDetail.Cell(2, 3).Text = cbYear.Text + "年每月结算金额（元）";

                //for (int tt = 0; tt < fGridDetail.Cols; tt++)
                //{
                //    fGridDetail.Column(tt).AutoFit();
                //}
                #endregion
            }
            catch (Exception e1)
            {
                throw new Exception("生成[" + detailExptr + "]报告异常[" + e1.Message + "]");
            }
            finally
            {
                fGridDetail.BackColor1 = System.Drawing.SystemColors.ButtonFace;
                fGridDetail.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
                FlexCell.Range oRange = fGridDetail.Range(1, 1, fGridDetail.Rows - 1, fGridDetail.Cols - 1);
                oRange.Locked = true;
                fGridDetail.SelectionMode = FlexCell.SelectionModeEnum.Free;
                fGridDetail.AutoRedraw = true;
                fGridDetail.Refresh();
                //FlashScreen.Close();
            }

        }

    }
}