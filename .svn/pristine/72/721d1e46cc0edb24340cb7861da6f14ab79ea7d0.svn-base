using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Util;
using System.IO;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount;
using Application.Business.Erp.SupplyChain.StockManage.StockBalManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost
{
    public partial class VDayCashReport : TBasicDataView
    {
        MIndirectCost model = new MIndirectCost();
        MProjectDepartment projectModel = new MProjectDepartment();
        private CurrentProjectInfo projectInfo;
        string mainExptr = "日现金流";
        string dtlExptr = "日现金流详细";

        IList list = new ArrayList();
        IList dtlList = new ArrayList();
        DateTime startDate = new DateTime();

        public VDayCashReport()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }

        private void InitData()
        {
            dtpDateBegin.Value = ConstObject.TheLogin.LoginDate;

            projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
            {

                this.lblPSelect.Visible = false;
                this.cbIfSelf.Visible = false;
                this.cbIfDtl.Visible = false;
                this.txtOperationOrg.Visible = false;
                this.btnOperationOrg.Visible = false;
                this.tabCost.TabPages.Remove(tabCost.TabPages["tabDetail"]);
            }
            this.fGridMain.Rows = 1;
            this.fGridDetail.Rows = 1;
        }

        private void InitEvents()
        {
            btnQuery.Click+=new EventHandler(btnQuery_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnOperationOrg.Click += new EventHandler(btnOperationOrg_Click);
        }

        void btnOperationOrg_Click(object sender, EventArgs e)
        {
            string opgId = "";
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect(true);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                OperationOrgInfo info = frm.Result[0] as OperationOrgInfo;
                txtOperationOrg.Tag = info;
                txtOperationOrg.Text = info.Name;
                opgId = info.Id;
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            if (this.tabCost.SelectedTab.Name.Equals("tabMain"))
            {
                fGridMain.ExportToExcel(mainExptr, false, false, true);
            }
            else if (tabCost.SelectedTab.Name.Equals("tabDetail"))
            {
                fGridDetail.ExportToExcel(mainExptr, false, false, true);
            }
            
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            OperationOrgInfo info = txtOperationOrg.Tag as OperationOrgInfo;
            if (info == null && txtOperationOrg.Visible == true)
            {
                MessageBox.Show("[范围选择]不能为空，请选择！");
                return;
            }
            list.Clear();
            dtlList.Clear();
            LoadTempleteFile(mainExptr + ".flx");
            LoadTempleteFile(dtlExptr + ".flx");

            //载入数据
            this.LoadMainFile();

            //设置外观
            fGridMain.BackColor1 = System.Drawing.SystemColors.ButtonFace;
            fGridMain.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            //设置外观
            fGridDetail.BackColor1 = System.Drawing.SystemColors.ButtonFace;
            fGridDetail.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
        }

        private void LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式
                if (modelName == "日现金流.flx")
                {                   
                    fGridMain.OpenFile(path + "\\" + modelName);//载入格式
                }
                if (modelName == "日现金流详细.flx")
                {
                    fGridDetail.OpenFile(path + "\\" + modelName);//载入格式
                }
            } else {
                MessageBox.Show("未找到模板格式文件" + modelName);
                return;
            }
        }

        #region 日现金流统计报表

        private void LoadMainFile()
        {
            OperationOrgInfo orgInfo = this.txtOperationOrg.Tag as OperationOrgInfo;
            FlashScreen.Show("正在生成[日现金流统计]报告...");

            OperationOrgInfo info = txtOperationOrg.Tag as OperationOrgInfo;         
            try
            {
                startDate = dtpDateBegin.Value.Date;
                if (info == null)
                {
                    list = model.IndirectCostSvr.QueryDayCashFlowReport(startDate, projectInfo.Id, "", cbIfSelf.Checked);
                    list = this.TransToMoneyUnit(list);
                }
                else
                {
                    string selProjectID = model.IndirectCostSvr.GetProjectIDByOperationOrg(info.Id);
                    if (ClientUtil.ToString(selProjectID) != "")
                    {
                        list = model.IndirectCostSvr.QueryDayCashFlowReport(startDate, selProjectID, "", cbIfSelf.Checked);
                    }
                    else
                    {
                        list = model.IndirectCostSvr.QueryDayCashFlowReport(startDate, "", info.SysCode, cbIfSelf.Checked);
                    }
                    list = this.TransToMoneyUnit(list);
                    if (cbIfDtl.Checked == true)
                    {
                        dtlList = model.IndirectCostSvr.QueryDayCashDetailReport(startDate, info.SysCode);
                        dtlList = this.TransToMoneyUnit(dtlList);
                    }
                }
                
                LoadTotalFile();
                LoadDetailFile();
            }
            catch (Exception e1)
            {
                throw new Exception("生成[日现金流统计]报告异常[" + e1.Message + "]");
            }
            finally
            {
                FlashScreen.Close();
            }
            
        }
        //转换计量单位
        private IList TransToMoneyUnit(IList list)
        {
            decimal unitRate = 10000;//万元
            //string opgSyscode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
            //belongOrgId = projectModel.CurrentSrv.GetBelongOperationOrg(opgSyscode);
            //if (belongOrgId == "" && projectInfo != null && projectInfo.Code == CommonUtil.CompanyProjectCode && cbIfSelf.Checked == false)//公司
            //{
            //    unitRate = 100000000;//亿元
            //}
            foreach (DataDomain domain in list)
            {
                domain.Name2 = decimal.Round(ClientUtil.ToDecimal(domain.Name2) / unitRate, 2);
                domain.Name3 = decimal.Round(ClientUtil.ToDecimal(domain.Name3) / unitRate, 2);
                domain.Name4 = decimal.Round(ClientUtil.ToDecimal(domain.Name4) / unitRate, 2);
                domain.Name5 = decimal.Round(ClientUtil.ToDecimal(domain.Name5) / unitRate, 2);
                domain.Name6 = decimal.Round(ClientUtil.ToDecimal(domain.Name6) / unitRate, 2);
                domain.Name7 = decimal.Round(ClientUtil.ToDecimal(domain.Name7) / unitRate, 2);
                domain.Name8 = decimal.Round(ClientUtil.ToDecimal(domain.Name8) / unitRate, 2);
                domain.Name9 = decimal.Round(ClientUtil.ToDecimal(domain.Name9) / unitRate, 2);
                domain.Name10 = decimal.Round(ClientUtil.ToDecimal(domain.Name10) / unitRate, 2);
                domain.Name12 = decimal.Round(ClientUtil.ToDecimal(domain.Name12) / unitRate, 2);
                domain.Name14 = decimal.Round(ClientUtil.ToDecimal(domain.Name14) / unitRate, 2);
                
            }
            return list;
        }

        #endregion

        #region 日现金流统计表

        private void LoadTotalFile()
        {
            fGridMain.AutoRedraw = false;
            int dtlStartRowNum = 4;//模板中的行号
            int dtlCount = list.Count;
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGridMain.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGridMain.Cols - 1);
            CommonUtil.SetFlexGridDetailCenter(range);
            //if (belongOrgId == "" && projectInfo != null && projectInfo.Code == CommonUtil.CompanyProjectCode && cbIfSelf.Checked == false)//公司
            //{
            //    this.fGridDetail.Cell(1, 1).Text = "单位: 亿元";
            //}
            //else
            //{
            //    this.fGridDetail.Cell(1, 1).Text = "单位: 万元";
            //}
            this.fGridMain.Cell(1, 1).Text = "单位: 万元";
            OperationOrgInfo info = txtOperationOrg.Tag as OperationOrgInfo;
            if (info != null)
            {
                this.fGridMain.Cell(1, 2).Text = dtpDateBegin.Value.Date.ToShortDateString() + " [" + info.Name + "] 日现金流统计报告";
            }
            else
            {
                this.fGridMain.Cell(1, 2).Text = dtpDateBegin.Value.Date.ToShortDateString() + " [" + projectInfo.Name + "] 日现金流统计报告";
            }
            
            int i = 0;
            IList subList = new ArrayList();
            IList yearList = new ArrayList();
            decimal dayInMoney = 0;
            decimal monthInMoney = 0;
            decimal yearInMoney = 0;
            decimal addInMoney = 0;
            decimal dayOutMoney = 0;
            decimal monthOutMoney = 0;
            decimal yearOutMoney = 0;
            decimal addOutMoney = 0;
            decimal initMoney = 0;
            decimal avgDayMoney = 0;
            decimal avgMonthMoney = 0;
            foreach (DataDomain domain in list)
            {
                //fGridDetail.Cell(dtlStartRowNum + i, 1).Text = ClientUtil.ToString(domain.Name1);//款项类别
                if (ClientUtil.ToDecimal(domain.Name2) != 0)
                {
                    initMoney = ClientUtil.ToDecimal(domain.Name2);//期初
                }
                if (ClientUtil.ToDecimal(domain.Name12) != 0)
                {
                    avgDayMoney = ClientUtil.ToDecimal(domain.Name12);//日均
                }
                if (ClientUtil.ToDecimal(domain.Name14) != 0)
                {
                    avgMonthMoney = ClientUtil.ToDecimal(domain.Name14);//月均
                }
                fGridMain.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(domain.Name3);//资金流入(当日)
                dayInMoney += ClientUtil.ToDecimal(domain.Name3);
                fGridMain.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(domain.Name4);//资金流入(当月)
                monthInMoney += ClientUtil.ToDecimal(domain.Name4);
                fGridMain.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(domain.Name5);//资金流入(本年)
                yearInMoney += ClientUtil.ToDecimal(domain.Name5);
                fGridMain.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(domain.Name6);//资金流入(累计)
                addInMoney += ClientUtil.ToDecimal(domain.Name6);
                fGridMain.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(domain.Name7);//资金流出(当日)
                dayOutMoney += ClientUtil.ToDecimal(domain.Name7);
                fGridMain.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(domain.Name8);//资金流出(当月)
                monthOutMoney += ClientUtil.ToDecimal(domain.Name8);
                fGridMain.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(domain.Name9);//资金流出(本年)
                yearOutMoney += ClientUtil.ToDecimal(domain.Name9);
                fGridMain.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(domain.Name10);//资金流出(累计)
                addOutMoney += ClientUtil.ToDecimal(domain.Name10);
                i++;
            }
            //写入合计和结余
            fGridMain.Cell(4, 2).Text = ClientUtil.ToString(initMoney);
            fGridMain.Cell(4, 11).Text = ClientUtil.ToString(dayInMoney - dayOutMoney);
            fGridMain.Cell(4, 12).Text = ClientUtil.ToString(avgDayMoney);
            fGridMain.Cell(4, 13).Text = ClientUtil.ToString(monthInMoney - monthOutMoney);
            fGridMain.Cell(4, 14).Text = ClientUtil.ToString(avgMonthMoney);
            fGridMain.Cell(4, 15).Text = ClientUtil.ToString(yearInMoney - yearOutMoney);
            fGridMain.Cell(4, 16).Text = ClientUtil.ToString(initMoney + yearInMoney - yearOutMoney);
            //合计
            fGridMain.Cell(24, 2).Text = ClientUtil.ToString(initMoney);
            fGridMain.Cell(24, 3).Text = ClientUtil.ToString(dayInMoney);
            fGridMain.Cell(24, 4).Text = ClientUtil.ToString(monthInMoney);
            fGridMain.Cell(24, 5).Text = ClientUtil.ToString(yearInMoney);
            fGridMain.Cell(24, 6).Text = ClientUtil.ToString(addInMoney);
            fGridMain.Cell(24, 7).Text = ClientUtil.ToString(dayOutMoney);
            fGridMain.Cell(24, 8).Text = ClientUtil.ToString(monthOutMoney);
            fGridMain.Cell(24, 9).Text = ClientUtil.ToString(yearOutMoney);
            fGridMain.Cell(24, 10).Text = ClientUtil.ToString(addOutMoney);
            fGridMain.Cell(24, 11).Text = ClientUtil.ToString(dayInMoney - dayOutMoney);
            fGridMain.Cell(24, 12).Text = ClientUtil.ToString(avgDayMoney);
            fGridMain.Cell(24, 13).Text = ClientUtil.ToString(monthInMoney - monthOutMoney);
            fGridMain.Cell(24, 14).Text = ClientUtil.ToString(avgMonthMoney);
            fGridMain.Cell(24, 15).Text = ClientUtil.ToString(yearInMoney - yearOutMoney);
            fGridMain.Cell(24, 16).Text = ClientUtil.ToString(initMoney + yearInMoney - yearOutMoney);
            for (int k = 4; k < fGridMain.Rows; k++)
            {
                for (int j = 2; j < fGridMain.Cols; j++)
                {
                    if (ClientUtil.ToDecimal(fGridMain.Cell(k, j).Text) == 0)
                    {
                        fGridMain.Cell(k, j).Text = "";
                    }
                }
            }

            fGridMain.AutoRedraw = true;
            fGridMain.Refresh();
        }
        #endregion

        #region 日现金流详细表

        private void LoadDetailFile()
        {
            fGridDetail.AutoRedraw = false;
            int dtlStartRowNum = 4;//模板中的行号
            int dtlCount = dtlList.Count;
            //插入明细行
            fGridDetail.InsertRow(dtlStartRowNum, dtlCount);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = fGridDetail.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGridDetail.Cols - 1);
            CommonUtil.SetFlexGridDetailCenter(range);
            this.fGridDetail.Cell(1, 1).Text = "单位: 万元";
            OperationOrgInfo info = txtOperationOrg.Tag as OperationOrgInfo;
            if (info != null)
            {
                this.fGridDetail.Cell(1, 2).Text = dtpDateBegin.Value.Date.ToShortDateString() + " [" + info.Name + "] 日现金流详细报告";
            }
            else
            {
                this.fGridDetail.Cell(1, 2).Text = dtpDateBegin.Value.Date.ToShortDateString() + " [" + projectInfo.Name + "] 日现金流详细报告";
            }
            decimal initMoney = 0;
            decimal dayInMoney = 0;
            decimal monthInMoney = 0;
            decimal yearInMoney = 0;
            decimal addInMoney = 0;
            decimal dayOutMoney = 0;
            decimal monthOutMoney = 0;
            decimal yearOutMoney = 0;
            decimal addOutMoney = 0;
            decimal dayLeftMoney = 0;
            decimal monthLeftMoney = 0;
            decimal yearLeftMoney = 0;
            decimal addLeftMoney = 0;
            for (int i = 0; i < dtlCount; i++)
            {
                DataDomain domain = (DataDomain)dtlList[i];
                fGridDetail.Cell(dtlStartRowNum + i, 1).Text = ClientUtil.ToString(domain.Name1);//款项类别
                fGridDetail.Cell(dtlStartRowNum + i, 2).Text = ClientUtil.ToString(domain.Name2);//期初
                initMoney += ClientUtil.ToDecimal(domain.Name2);
                fGridDetail.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(domain.Name3);//资金流入(当日)
                dayInMoney += ClientUtil.ToDecimal(domain.Name3);
                fGridDetail.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(domain.Name4);//资金流入(当月)
                monthInMoney += ClientUtil.ToDecimal(domain.Name4);
                fGridDetail.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(domain.Name5);//资金流入(本年)
                yearInMoney += ClientUtil.ToDecimal(domain.Name5);
                fGridDetail.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(domain.Name6);//资金流入(累计)
                addInMoney += ClientUtil.ToDecimal(domain.Name6);
                fGridDetail.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(domain.Name7);//资金流出(当日)
                dayOutMoney += ClientUtil.ToDecimal(domain.Name7);
                fGridDetail.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(domain.Name8);//资金流出(当月)
                monthOutMoney += ClientUtil.ToDecimal(domain.Name8);
                fGridDetail.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(domain.Name9);//资金流出(本年)
                yearOutMoney += ClientUtil.ToDecimal(domain.Name9);
                fGridDetail.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(domain.Name10);//资金流出(累计)
                addOutMoney += ClientUtil.ToDecimal(domain.Name10);
                fGridDetail.Cell(dtlStartRowNum + i, 11).Text = (ClientUtil.ToDecimal(domain.Name3) - ClientUtil.ToDecimal(domain.Name7)) + "";//当日
                dayLeftMoney += ClientUtil.ToDecimal(fGridDetail.Cell(dtlStartRowNum + i, 11).Text);
                fGridDetail.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(domain.Name12);//日均
                fGridDetail.Cell(dtlStartRowNum + i, 13).Text = (ClientUtil.ToDecimal(domain.Name4) - ClientUtil.ToDecimal(domain.Name8)) + "";//本月
                monthLeftMoney += ClientUtil.ToDecimal(fGridDetail.Cell(dtlStartRowNum + i, 13).Text);
                fGridDetail.Cell(dtlStartRowNum + i, 14).Text = ClientUtil.ToString(domain.Name14);//月均
                fGridDetail.Cell(dtlStartRowNum + i, 15).Text = (ClientUtil.ToDecimal(domain.Name5) - ClientUtil.ToDecimal(domain.Name9)) + "";//本年
                yearLeftMoney += ClientUtil.ToDecimal(fGridDetail.Cell(dtlStartRowNum + i, 15).Text);
                fGridDetail.Cell(dtlStartRowNum + i, 16).Text = (ClientUtil.ToDecimal(domain.Name2) + ClientUtil.ToDecimal(domain.Name5)
                                                                    - ClientUtil.ToDecimal(domain.Name9)) + "";//累计
                addLeftMoney += ClientUtil.ToDecimal(fGridDetail.Cell(dtlStartRowNum + i, 16).Text);
            }

            fGridDetail.Cell(dtlStartRowNum + dtlCount, 1).Text = "合  计";
            fGridDetail.Cell(dtlStartRowNum + dtlCount, 2).Text = ClientUtil.ToString(initMoney);
            fGridDetail.Cell(dtlStartRowNum + dtlCount, 3).Text = ClientUtil.ToString(dayInMoney);
            fGridDetail.Cell(dtlStartRowNum + dtlCount, 4).Text = ClientUtil.ToString(monthInMoney);
            fGridDetail.Cell(dtlStartRowNum + dtlCount, 5).Text = ClientUtil.ToString(yearInMoney);
            fGridDetail.Cell(dtlStartRowNum + dtlCount, 6).Text = ClientUtil.ToString(addInMoney);
            fGridDetail.Cell(dtlStartRowNum + dtlCount, 7).Text = ClientUtil.ToString(dayOutMoney);
            fGridDetail.Cell(dtlStartRowNum + dtlCount, 8).Text = ClientUtil.ToString(monthOutMoney);
            fGridDetail.Cell(dtlStartRowNum + dtlCount, 9).Text = ClientUtil.ToString(yearOutMoney);
            fGridDetail.Cell(dtlStartRowNum + dtlCount, 10).Text = ClientUtil.ToString(addOutMoney);
            fGridDetail.Cell(dtlStartRowNum + dtlCount, 11).Text = ClientUtil.ToString(dayLeftMoney);
            fGridDetail.Cell(dtlStartRowNum + dtlCount, 13).Text = ClientUtil.ToString(monthLeftMoney);
            fGridDetail.Cell(dtlStartRowNum + dtlCount, 15).Text = ClientUtil.ToString(yearLeftMoney);
            fGridDetail.Cell(dtlStartRowNum + dtlCount, 16).Text = ClientUtil.ToString(addLeftMoney);

            for (int k = 4; k < fGridDetail.Rows; k++)
            {
                for (int j = 2; j < fGridDetail.Cols; j++)
                {
                    if (ClientUtil.ToDecimal(fGridDetail.Cell(k, j).Text) == 0)
                    {
                        fGridDetail.Cell(k, j).Text = "";
                    }
                }
            }
            fGridDetail.Column(1).AutoFit();
            fGridDetail.AutoRedraw = true;
            fGridDetail.Refresh();
        }
        #endregion
    }
}