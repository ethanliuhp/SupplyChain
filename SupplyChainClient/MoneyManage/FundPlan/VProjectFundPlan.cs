using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using VirtualMachine.Component.Util;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost
{
    public partial class VProjectFundPlan : TBasicDataView
    {
        private string reportGSZG = "公司直管项目汇总";
        private string reportXMZJ = "项目资金使用计划表";

        private CurrentProjectInfo projectInfo;
        private ProjectFundPlanMaster fpMaster;

        MIndirectCost model = new MIndirectCost();

        public VProjectFundPlan()
        {
            InitializeComponent();
            InitEvent();
            InitData();
            LoadData(reportGSZG);
        }

        private void InitEvent()
        {
            tabControl.SelectedIndexChanged += new EventHandler(tabControl_SelectedIndexChanged);
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnCreate.Click += new EventHandler(btnCreate_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnSubmit.Click += new EventHandler(btnSubmit_Click);
            btnQuery1.Click += new EventHandler(btnQuery1_Click);
            btnCreate1.Click += new EventHandler(btnCreate1_Click);
            btnSave1.Click += new EventHandler(btnSave1_Click);
            btnSubmit1.Click += new EventHandler(btnSubmit1_Click);
        }

        private void InitData()
        {
            this.LoadFLXFile(reportGSZG + ".flx", reportGSZG);
            this.LoadFLXFile(reportXMZJ + ".flx", reportXMZJ);
            projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
            {
                fpMaster = new ProjectFundPlanMaster();
                fpMaster.ProjectId = projectInfo.Id;
                fpMaster.ProjectName = projectInfo.Name;
                fpMaster.OperOrgInfo = ConstObject.TheOperationOrg;
                fpMaster.OpgSysCode = ConstObject.TheOperationOrg.SysCode;
                fpMaster.OperOrgInfoName = ConstObject.TheOperationOrg.Name;
               // fpMaster.CreatePerson = ConstObject.LoginPersonInfo;
                fpMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
            }
           
        }

        void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab.Text == reportGSZG)
            {
                this.LoadData(reportGSZG);
            }
            if (tabControl.SelectedTab.Text == reportXMZJ)
            {
                this.LoadData(reportXMZJ);
            }
        }

        #region 公司直管项目汇总

        void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                int year = ClientUtil.ToInt(this.comYear.Text);
                int month = ClientUtil.ToInt(this.comMonth.Text);
                ProjectFundPlanMaster fp = model.IndirectCostSvr.GetProjectFundPlanMaster(projectInfo.Id, year, month, reportGSZG);
                if (!ClientUtil.isEmpty(fp) && fp.Details.Count > 0)
                {
                    this.LoadDetail(fp, reportGSZG);
                }
                else
                {
                    MessageBox.Show("没有数据!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询失败:" + ex.Message);
            }
        }

        void btnCreate_Click(object sender, EventArgs e)
        {
            try 
            {
                int year = ClientUtil.ToInt(this.comYear.Text);
                int month = ClientUtil.ToInt(this.comMonth.Text);
                ProjectFundPlanMaster fp = model.IndirectCostSvr.GetProjectFundPlanMaster(projectInfo.Id, year, month, reportGSZG);
                if (!ClientUtil.isEmpty(fp))
                {
                    this.LoadMaster(fp, reportGSZG);
                }
                else
                {
                    MessageBox.Show("没有数据!");
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("生成失败:" + ex.Message);
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            try 
            {
                if (projectInfo == null || projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                {
                    MessageBox.Show("系统管理员不允许添加!");
                    return;
                }
                this.ViewToModel(reportGSZG);
                model.IndirectCostSvr.SaveProjectFundPlan(fpMaster);
                MessageBox.Show("保存成功!");
                this.LoadMaster(fpMaster, reportGSZG);
                this.LoadDetail(fpMaster, reportGSZG);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ex.Message);
            }
        }

   
        void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (projectInfo == null || projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                {
                    MessageBox.Show("系统管理员不允许添加!");
                    return;
                }
                this.ViewToModel(reportGSZG);
                fpMaster.DocState = DocumentState.InAudit;
                model.IndirectCostSvr.SaveProjectFundPlan(fpMaster);
                MessageBox.Show("保存成功!");
                this.LoadMaster(fpMaster, reportGSZG);
                this.LoadDetail(fpMaster, reportGSZG);
                this.btnSave.Visible = false;
                this.btnSubmit.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ex.Message);
            }
        }

        #endregion

        #region 项目资金使用计划表

        void btnQuery1_Click(object sender, EventArgs e)
        {
            try
            {
                int year = ClientUtil.ToInt(this.comYear1.Text);
                int month = ClientUtil.ToInt(this.comMonth1.Text);
                ProjectFundPlanMaster fp = model.IndirectCostSvr.GetProjectFundPlanMaster(projectInfo.Id, year, month, reportXMZJ);
                if (!ClientUtil.isEmpty(fp) && fp.Details.Count > 0)
                {
                    this.LoadDetail(fp, reportXMZJ);
                }
                else
                {
                    MessageBox.Show("没有数据!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询失败:" + ex.Message);
            }
        }

        void btnCreate1_Click(object sender, EventArgs e)
        {
            try
            {
                int year = ClientUtil.ToInt(this.comYear1.Text);
                int month = ClientUtil.ToInt(this.comMonth1.Text);
                ProjectFundPlanMaster fp = model.IndirectCostSvr.GetProjectFundPlanMaster(projectInfo.Id, year, month, reportXMZJ);
                if (!ClientUtil.isEmpty(fp))
                {
                    this.LoadMaster(fp, reportXMZJ);
                }
                else
                {
                    MessageBox.Show("没有数据!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("生成失败:" + ex.Message);
            }
        }

        void btnSave1_Click(object sender, EventArgs e)
        {
            try
            {
                if (projectInfo == null || projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                {
                    MessageBox.Show("系统管理员不允许添加!");
                    return;
                }
                this.ViewToModel(reportXMZJ);
                model.IndirectCostSvr.SaveProjectFundPlan(fpMaster);
                MessageBox.Show("保存成功!");
                this.LoadMaster(fpMaster, reportXMZJ);
                this.LoadDetail(fpMaster, reportXMZJ);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ex.Message);
            }
        }

        void btnSubmit1_Click(object sender, EventArgs e)
        {
            try
            {
                if (projectInfo == null || projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                {
                    MessageBox.Show("系统管理员不允许添加!");
                    return;
                }
                this.ViewToModel(reportXMZJ);
                fpMaster.DocState = DocumentState.InAudit;
                model.IndirectCostSvr.SaveProjectFundPlan(fpMaster);
                MessageBox.Show("保存成功!");
                this.LoadMaster(fpMaster, reportXMZJ);
                this.LoadDetail(fpMaster, reportXMZJ);
                this.btnSave1.Visible = false;
                this.btnSubmit1.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ex.Message);
            }
        }

        #endregion

        private void ViewToModel(string type)
        {
            if (type == reportGSZG) 
            {
                int year = ClientUtil.ToInt(this.comYear.Text);
                int month = ClientUtil.ToInt(this.comMonth.Text);
                ProjectFundPlanMaster fp = model.IndirectCostSvr.GetProjectFundPlanMaster(fpMaster.ProjectId, year, month, reportGSZG);
                if (!ClientUtil.isEmpty(fp))
                {
                    fpMaster = fp;
                    fp.Details.Clear();
                }
                else 
                {
                    fpMaster.CreateYear = year;
                    fpMaster.CreateMonth = month;
                    fpMaster.CreateDate = ConstObject.LoginDate;
                }
               // fpMaster.ProjectType = reportGSZG;
              //  fpMaster.CumulativeConfirmIncome = ClientUtil.ToDecimal(fGridDetail.Cell(4, 1).Text);
               // fpMaster.CumulativeOwnerReport = ClientUtil.ToDecimal(fGridDetail.Cell(4, 2).Text);
                fpMaster.CumulativeGathering = ClientUtil.ToDecimal(fGridDetail.Cell(4, 3).Text);
                fpMaster.CumulativePayment = ClientUtil.ToDecimal(fGridDetail.Cell(4, 4).Text);
               // fpMaster.PlanGathering = ClientUtil.ToDecimal(fGridDetail.Cell(4, 6).Text);
              //  fpMaster.PlanPayment = ClientUtil.ToDecimal(fGridDetail.Cell(4, 7).Text);
              //  fpMaster.ContractGatheringProportion = ClientUtil.ToDecimal(fGridDetail.Cell(5, 2).Text);
                for (int i = 9; i < 15; i++)
                {
                    ProjectFundPlanDetail fpDetail = new ProjectFundPlanDetail();
                    fpDetail.Master = fpMaster;
                    //fpDetail.ExpensesName = ClientUtil.ToString(fGridDetail.Cell(i, 1).Text);
                  //  fpDetail.CumulativeSettlementAmount = ClientUtil.ToDecimal(fGridDetail.Cell(i, 2).Text);
                    fpDetail.CumulativePayment = ClientUtil.ToDecimal(fGridDetail.Cell(i, 3).Text);
                  //  fpDetail.ExpiredPayableAmount = ClientUtil.ToDecimal(fGridDetail.Cell(i, 5).Text);
                  //  fpDetail.PlanPayment = ClientUtil.ToDecimal(fGridDetail.Cell(i, 6).Text);
                   // fpDetail.PaidAmount = ClientUtil.ToDecimal(fGridDetail.Cell(i,8));
                    fpMaster.AddDetail(fpDetail);
                }
            }
        }

        private void LoadDetail(ProjectFundPlanMaster fpMaster, string type)
        {
            if (!ClientUtil.isEmpty(fpMaster)) 
            {
                if (type == reportGSZG)
                {
                    foreach (ProjectFundPlanDetail fpDetail in fpMaster.Details)
                    {
                        for (int i = 9; i < 15; i++)
                        {
                            //if (fpDetail.ExpensesName == ClientUtil.ToString(fGridDetail.Cell(i, 1).Text))
                            //{
                            //    fGridDetail.Cell(i, 2).Text = fpDetail.CumulativeSettlementAmount.ToString("N2");
                            //    fGridDetail.Cell(i, 3).Text = fpDetail.CumulativePayment.ToString("N2");
                            //    fGridDetail.Cell(i, 5).Text = fpDetail.ExpiredPayableAmount.ToString("N2");
                            //    fGridDetail.Cell(i, 6).Text = fpDetail.PlanPayment.ToString("N2");
                            //    fGridDetail.Cell(i, 8).Text = fpDetail.PaidAmount.ToString("N2");
                            //}
                        }
                    }
                }
            }
        }

        private void LoadMaster(ProjectFundPlanMaster fpMaster, string type)
        {
            if (!ClientUtil.isEmpty(fpMaster)) 
            {
                if (type == reportGSZG)
                {
                    //fGridDetail.Cell(4, 1).Text = ClientUtil.ToString(fpMaster.CumulativeConfirmIncome);
                    //fGridDetail.Cell(4, 2).Text = ClientUtil.ToString(fpMaster.CumulativeOwnerReport);
                    //fGridDetail.Cell(4, 3).Text = ClientUtil.ToString(fpMaster.CumulativeGathering);
                    //fGridDetail.Cell(4, 4).Text = ClientUtil.ToString(fpMaster.CumulativePayment);
                    //fGridDetail.Cell(4, 5).Text = "";
                    //fGridDetail.Cell(4, 6).Text = ClientUtil.ToString(fpMaster.PlanGathering);
                    //fGridDetail.Cell(4, 7).Text = ClientUtil.ToString(fpMaster.PlanPayment);
                    //fGridDetail.Cell(4, 8).Text = "";
                   // fGridDetail.Cell(5, 2).Text = ClientUtil.ToString(fpMaster.ContractGatheringProportion);
                    fGridDetail.Cell(5, 4).Text = "";
                }
            }
        }

        private void LoadData(string type)
        {
            try
            {
                if (type == reportGSZG)
                {
                    fGridDetail.AutoRedraw = false;

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

                    if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                    {
                        fGridDetail.Cell(1, 1).Text = projectInfo.Name + this.comMonth.Text + "月资金使用计划申报表";
                    }
                }
                if (type == reportXMZJ)
                {
                    fGridDetail1.AutoRedraw = false;

                    this.comYear1.Items.Clear();
                    this.comMonth1.Items.Clear();

                    for (int i = 0; i < 13; i++)
                    {
                        this.comYear1.Items.Add(ConstObject.TheLogin.TheComponentPeriod.NowYear + (i - 6));
                    }
                    for (int i = 1; i < 13; i++)
                    {
                        this.comMonth1.Items.Add(i);
                    }

                    this.comYear1.Text = DateTime.Now.Year.ToString();
                    this.comMonth1.Text = DateTime.Now.Month.ToString();

                    if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                    {
                        fGridDetail1.Cell(2, 2).Text = projectInfo.Name;
                    }
                    fGridDetail1.Cell(2, 6).Text = this.dtpDate1.Value.Year + "年" + this.dtpDate1.Value.Month + "月" + this.dtpDate1.Value.Day + "日";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                fGridDetail.AutoRedraw = true;
                fGridDetail.Refresh();
                fGridDetail1.AutoRedraw = true;
                fGridDetail1.Refresh();
            }
        }

        private void LoadFLXFile(string flxname, string type)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(flxname))
            {
                eFile.CreateTempleteFileFromServer(flxname);
                if (type == reportGSZG)
                {
                    fGridDetail.OpenFile(path + "\\" + flxname);//载入格式
                }
                if (type == reportXMZJ)
                {
                    fGridDetail1.OpenFile(path + "\\" + flxname);//载入格式
                }
            }
        }
    }
}
