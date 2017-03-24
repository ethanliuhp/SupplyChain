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
    public partial class VMechanicalCostComparisonRpt : TBasicDataView
    {
        ICommonMethodSrv service = CommonMethod.CommonMethodSrv;
        string detailExptr = "机械费成本分析对比表";
        string flexName = "机械费成本分析对比表.flx";
        CurrentProjectInfo projectInfo;

        public VMechanicalCostComparisonRpt()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }

        private void InitData()
        {
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

            LoadTempleteFile(flexName);
            projectInfo = StaticMethod.GetProjectInfo();
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

        private void btnQuery_Click(object sender, EventArgs e)
        {
            LoadTempleteFile(flexName);
            LoadDetailFile();
        }

        private string GetRptSql()
        {
            #region old code
//            return @"select 
//                            t3.COSTINGSUBJECTNAME as CostName --费用名称
//                            ,sum(nvl(t3.CURRRESPONSICONSUMEQUANTITY,0)) as CurrZRAmount          --本期责任成本数量
//                            ,case sum(nvl(t3.CURRRESPONSICONSUMEQUANTITY,0)) 
//                                                         when 0 then 0 
//                                                         else round( (sum(nvl(t3.CURRRESPONSICONSUMETOTALPRICE,0))/sum(nvl(t3.CURRRESPONSICONSUMEQUANTITY,0))),2) 
//                                                    end  as CurrZRPrice           --本期责任成本单价
//                            ,sum(nvl(t3.CURRRESPONSICONSUMETOTALPRICE,0)) as CurrZRMoney           --本期责任成本合价
//                            ,sum(nvl(t3.CURRREALCONSUMEQUANTITY,0)) as CurrSJAmount          --本期实际成本数量
//                            ,sum(nvl(t3.CURRREALCONSUMEPRICE,0)) as CurrSJPrice           --本期实际成本单价
//                            ,sum(nvl(t3.CURRREALCONSUMETOTALPRICE,0)) as CurrSJMoney           --本期实际成本合价
//                            ,sum(nvl(t3.CURRREALCONSUMETOTALPRICE,0)) - sum(nvl(t3.CURRRESPONSICONSUMETOTALPRICE,0)) as CurrJC                --本期节超( 实际金额 - 责任金额）
//                            ,case sum(nvl(t3.CURRRESPONSICONSUMETOTALPRICE,0)) 
//                                                                               when  0 then 0
//                                                                               else round((sum(nvl(t3.CURRREALCONSUMETOTALPRICE,0)) - sum(nvl(t3.CURRRESPONSICONSUMETOTALPRICE,0)))/sum(nvl(t3.CURRRESPONSICONSUMETOTALPRICE,0)),4)
//                                                                               end as CurrJCRate            --本期节超比例（实际金额-责任金额）/责任金额
//                            ,sum(nvl(t3.SUMRESPONSICONSUMEQUANTITY,0)) as LJZRAmount            --累计责任成本数量
//                            ,case sum(nvl(t3.SUMRESPONSICONSUMEQUANTITY,0))
//                                                           when 0 then 0
//                                                           else round( sum(nvl(t3.SUMRESPONSICONSUMETOTALPRICE,0)) /sum(nvl(t3.SUMRESPONSICONSUMEQUANTITY,0)),2)
//                                                           end as LJZRPrice             --累计责任成本单价
//                            ,sum(nvl(t3.SUMRESPONSICONSUMETOTALPRICE,0)) as LJZRMoney             --累计责任成本合价
//                            ,sum(nvl(t3.SUMREALCONSUMEQUANTITY,0)) as LJSJAmount            --累计实际成本数量
//                            ,case sum(nvl(t3.SUMREALCONSUMEQUANTITY,0))
//                                                                       when 0 then 0 
//                                                                       else round(sum(nvl(t3.SUMREALCONSUMETOTALPRICE,0))/ sum(nvl(t3.SUMREALCONSUMEQUANTITY,0)) ,2)
//                                                                       end as LJSJPrice             --累计实际成本单价
//                            ,sum(nvl(t3.SUMREALCONSUMETOTALPRICE,0)) as LJSJMoney             --累计实际成本合价
//                            ,sum(nvl(t3.SUMREALCONSUMETOTALPRICE,0)) - sum(nvl(t3.SUMRESPONSICONSUMETOTALPRICE,0))  as LJJC                  --累计节超
//                            ,case sum(nvl(t3.SUMRESPONSICONSUMETOTALPRICE,0)) 
//                                                                              when 0 then 0
//                                                                              else round( (sum(nvl(t3.SUMREALCONSUMETOTALPRICE,0)) - sum(nvl(t3.SUMRESPONSICONSUMETOTALPRICE,0)))/sum(nvl(t3.SUMRESPONSICONSUMETOTALPRICE,0)) ,4)
//                                                                              end as LJJCRate              --累计节超比例
//                      from thd_costmonthaccount t1
//                                 ,thd_costmonthaccountdtl t2
//                                 ,thd_costmonthaccdtlconsume t3
//                      where t1.id=t2.parentid 
//                             and t2.id=t3.parentid ";

            #endregion

            return @"select 
                            t3.COSTINGSUBJECTNAME as CostName --费用名称
                            
                            ,sum(t3.CURRRESPONSICONSUMEQUANTITY) as CurrZRAmount          --本期责任成本数量
                            ,sum(t3.CURRRESPONSICONSUMETOTALPRICE) as CurrZRMoney           --本期责任成本合价
 
                            ,sum(t3.CURRREALCONSUMEQUANTITY) as CurrSJAmount          --本期实际成本数量
                            ,sum(t3.CURRREALCONSUMEPRICE) as CurrSJPrice           --本期实际成本单价
                            ,sum(t3.CURRREALCONSUMETOTALPRICE) as CurrSJMoney           --本期实际成本合价
                                    
                            ,sum(t3.SUMRESPONSICONSUMEQUANTITY) as LJZRAmount            --累计责任成本数量
                            ,sum(t3.SUMRESPONSICONSUMETOTALPRICE) as LJZRMoney             --累计责任成本合价
                            
                            ,sum(t3.SUMREALCONSUMEQUANTITY) as LJSJAmount            --累计实际成本数量
                            ,sum(t3.SUMREALCONSUMETOTALPRICE) as LJSJMoney             --累计实际成本合价
                      from thd_costmonthaccount t1
                                 ,thd_costmonthaccountdtl t2
                                 ,thd_costmonthaccdtlconsume t3
                      where t1.id=t2.parentid 
                             and t2.id=t3.parentid ";
        }

        private string GetRptSqlCondition()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" and t1.kjn = {0}", this.comYear.Text.Trim());
            sb.AppendFormat(" and t1.kjy = {0}", this.comMonth.Text.Trim());
            sb.AppendFormat(" and t1.theprojectguid = '{0}'", projectInfo.Id);
            sb.Append(" and t3.COSTSUBJECTCODE like 'C51103%'");
            sb.Append(" group by t3.COSTINGSUBJECTNAME");

            return sb.ToString();
        }
        private void LoadDetailFile()
        {
            FlashScreen.Show("正在生成[" + detailExptr + "]报告...");
            try
            {
                fGridDetail.AutoRedraw = false;
                string sql = GetRptSql() + GetRptSqlCondition();
                var dt = service.GetData(sql).Tables[0];

                if (dt == null && dt.Rows.Count == 0) return;

                #region 处理表头
                string sTitle = string.Format("{0}年{1}月 机械费用对比表", comYear.Text, comMonth.Text);
                fGridDetail.Cell(1, 1).Text = sTitle;

                fGridDetail.Cell(2, 1).Text = "项目名称：" + projectInfo.Name;
                fGridDetail.Cell(2, 8).Text = string.Format(@"日  期： {0}年 {1}月  {2}日	", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                #endregion

                #region 处理表体
                fGridDetail.InsertRow(5, dt.Rows.Count - 1);          // 插入用于存储所有单位的行数

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var dr = dt.Rows[i];
                    var curRow = 5 + i;
                    #region old code
                    //fGridDetail.Cell(curRow, 1).Text = (i + 1).ToString();//序号
                    //fGridDetail.Cell(curRow, 2).Text = dr["CostName"].ToString();//费用名称

                    //fGridDetail.Cell(curRow, 3).Text = dr["CurrZRAmount"].ToString();//本期责任成本数量
                    //fGridDetail.Cell(curRow, 4).Text = dr["CurrZRPrice"].ToString();//本期责任成本单价
                    //fGridDetail.Cell(curRow, 5).Text = dr["CurrZRMoney"].ToString();//本期责任成本合价

                    //fGridDetail.Cell(curRow, 6).Text = dr["CurrSJAmount"].ToString();//本期实际成本数量
                    //fGridDetail.Cell(curRow, 7).Text = dr["CurrSJPrice"].ToString();//本期实际成本单价
                    //fGridDetail.Cell(curRow, 8).Text = dr["CurrSJMoney"].ToString();//本期实际成本合价

                    //fGridDetail.Cell(curRow, 9).Text = dr["CurrJC"].ToString();//本期节超
                    //fGridDetail.Cell(curRow, 10).Text = dr["CurrJCRate"].ToString();//本期节超比例


                    //fGridDetail.Cell(curRow, 11).Text = dr["LJZRAmount"].ToString();//累计责任成本数量
                    //fGridDetail.Cell(curRow, 12).Text = dr["LJZRPrice"].ToString();//累计责任成本单价
                    //fGridDetail.Cell(curRow, 13).Text = dr["LJZRMoney"].ToString();//累计责任成本合价


                    //fGridDetail.Cell(curRow, 14).Text = dr["LJSJAmount"].ToString();//累计实际成本数量
                    //fGridDetail.Cell(curRow, 15).Text = dr["LJSJPrice"].ToString();//累计实际成本单价
                    //fGridDetail.Cell(curRow, 16).Text = dr["LJSJMoney"].ToString();//累计实际成本合价

                    //fGridDetail.Cell(curRow, 17).Text = dr["LJJC"].ToString();//累计节超
                    //fGridDetail.Cell(curRow, 18).Text = dr["LJJCRate"].ToString();//累计节超比例
                    #endregion

                    fGridDetail.Cell(curRow, 1).Text = (i + 1).ToString();//序号
                    fGridDetail.Cell(curRow, 2).Text = dr["CostName"].ToString();//费用名称

                    fGridDetail.Cell(curRow, 3).Text = dr["CurrZRAmount"].ToString();//本期责任成本数量
                    decimal currZRPrice = 0;
                    if (dr["CurrZRAmount"] != null && dr["CurrZRAmount"] != DBNull.Value && dr["CurrZRMoney"] != null && dr["CurrZRMoney"]!=DBNull.Value)
                    {
                        currZRPrice = Convert.ToDecimal(dr["CurrZRAmount"]) == 0 ? 0 : Convert.ToDecimal(dr["CurrZRMoney"]) / Convert.ToDecimal(dr["CurrZRAmount"]);
                    }
                    fGridDetail.Cell(curRow, 4).Text = currZRPrice.ToString("0.00");//本期责任成本单价

                    fGridDetail.Cell(curRow, 5).Text = dr["CurrZRMoney"].ToString();//本期责任成本合价

                    fGridDetail.Cell(curRow, 6).Text = dr["CurrSJAmount"].ToString();//本期实际成本数量
                    fGridDetail.Cell(curRow, 7).Text = dr["CurrSJPrice"].ToString();//本期实际成本单价
                    fGridDetail.Cell(curRow, 8).Text = dr["CurrSJMoney"].ToString();//本期实际成本合价

                    decimal currjc = 0;
                    if (dr["CurrSJMoney"] != null && dr["CurrSJMoney"] != DBNull.Value && dr["CurrZRMoney"] != null && dr["CurrZRMoney"] != DBNull.Value)
                        currjc = Convert.ToDecimal(dr["CurrZRMoney"]) - Convert.ToDecimal(dr["CurrSJMoney"]);//Convert.ToDecimal(dr["CurrSJMoney"]) - Convert.ToDecimal(dr["CurrZRMoney"]);

                    fGridDetail.Cell(curRow, 9).Text = currjc.ToString("0.00");//本期节超
                    if (currjc < 0) fGridDetail.Cell(curRow, 9).BackColor = System.Drawing.Color.Red;
                    decimal currjcrate = 0;
                    if (dr["CurrZRMoney"] != null && dr["CurrZRMoney"] != DBNull.Value)
                        currjcrate = Convert.ToDecimal(dr["CurrZRMoney"]) == 0 ? 0 : currjc / Convert.ToDecimal(dr["CurrZRMoney"]);
                    fGridDetail.Cell(curRow, 10).Text = currjcrate.ToString("0.0000");//本期节超比例


                    fGridDetail.Cell(curRow, 11).Text = dr["LJZRAmount"].ToString();//累计责任成本数量

                    decimal ljzrprice = 0;
                    if (dr["LJZRAmount"] != null && dr["LJZRAmount"] != DBNull.Value && dr["LJZRMoney"] != null && dr["LJZRMoney"] != DBNull.Value)
                    {
                        ljzrprice = Convert.ToDecimal(dr["LJZRAmount"]) == 0 ? 0 : Convert.ToDecimal(dr["LJZRMoney"]) / Convert.ToDecimal(dr["LJZRAmount"]);
                    }
                    fGridDetail.Cell(curRow, 12).Text = ljzrprice.ToString("0.00");//累计责任成本单价
                    fGridDetail.Cell(curRow, 13).Text = dr["LJZRMoney"].ToString();//累计责任成本合价


                    fGridDetail.Cell(curRow, 14).Text = dr["LJSJAmount"].ToString();//累计实际成本数量
                    decimal ljsjprice = 0;
                    if (dr["LJSJAmount"] != null && dr["LJSJAmount"] != DBNull.Value && dr["LJSJMoney"] != null && dr["LJSJMoney"] != DBNull.Value)
                    {
                        ljsjprice = Convert.ToDecimal(dr["LJSJAmount"]) == 0 ? 0 : Convert.ToDecimal(dr["LJSJMoney"]) / Convert.ToDecimal(dr["LJSJAmount"]);
                    }
                    fGridDetail.Cell(curRow, 15).Text = ljsjprice.ToString("0.00");//累计实际成本单价
                    fGridDetail.Cell(curRow, 16).Text = dr["LJSJMoney"].ToString();//累计实际成本合价

                    decimal ljjc = 0;
                    if (dr["LJSJMoney"] != null && dr["LJSJMoney"] != DBNull.Value && dr["LJZRMoney"] != null && dr["LJZRMoney"] != DBNull.Value)
                        ljjc = Convert.ToDecimal(dr["LJZRMoney"]) - Convert.ToDecimal(dr["LJSJMoney"]);//Convert.ToDecimal(dr["LJSJMoney"]) - Convert.ToDecimal(dr["LJZRMoney"]);

                    fGridDetail.Cell(curRow, 17).Text = ljjc.ToString("0.00");//累计节超
                    if (ljjc < 0) { fGridDetail.Cell(curRow, 17).BackColor = System.Drawing.Color.Red; }

                    decimal ljjcrate = 0;
                    if (dr["LJZRMoney"] != null && dr["LJZRMoney"] != DBNull.Value)
                        ljjcrate = Convert.ToDecimal(dr["LJZRMoney"]) == 0 ? 0 : ljjc / Convert.ToDecimal(dr["LJZRMoney"]);

                    fGridDetail.Cell(curRow, 18).Text = ljjcrate.ToString("0.0000");//累计节超比例

                    fGridDetail.Row(curRow).AutoFit();
                }
                #endregion


                #region 处理表尾

                int bwRow = dt.Rows.Count + 5;
                fGridDetail.InsertRow(bwRow, 1);

                FlexCell.Range rag = fGridDetail.Range(bwRow, 1, bwRow, 2);
                rag.Merge();//合并单元格

                fGridDetail.Cell(bwRow, 1).Text = "项目经理：";

                FlexCell.Range rag1 = fGridDetail.Range(bwRow, 6, bwRow, 9);
                rag1.Merge();//合并单元格
                fGridDetail.Cell(bwRow, 6).Text = "项目预算员（商务经理）：";

                FlexCell.Range rag2 = fGridDetail.Range(bwRow, 11, bwRow, 13);
                rag2.Merge();//合并单元格

                fGridDetail.Cell(bwRow, 11).Text = "项目机电管理员：";


                FlexCell.Range rag3 = fGridDetail.Range(bwRow, 16, bwRow, 17);
                rag3.Merge();//合并单元格

                fGridDetail.Cell(bwRow, 16).Text = "项目成本员：";


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

                fGridDetail.AutoRedraw = true;
                fGridDetail.Refresh();
                FlashScreen.Close();
            }

        }
    }
}
