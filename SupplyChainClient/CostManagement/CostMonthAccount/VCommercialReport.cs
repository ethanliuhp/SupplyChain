﻿using System;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Util;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using System.Collections;
using System.Collections.Generic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Globalization;
using Iesi.Collections.Generic;
using System.Windows.Documents;
using System.Linq;


using IRPServiceModel.Services.Common;
using VirtualMachine.Component.WinControls.Controls;
using System.Text;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    public partial class VCommercialReport : TBasicDataView
    {
        private string reportCBJCZB = "成本检查指标统计表";
        private string reportJCBBB = "局成本报表";
        private string reportFBZB = "专业分包指标统计表";//20160824 其实就是专业分包指标统计表，之前是  专业分包指标统计表 和  劳务分包指标统计表 统称为 分包指标统计表
        private string reportFBZY = "分包争议统计表";
        private string reportFBZJBS = "分包终结报审统计表";
        private string reportGSJSJZ = "公司结算进展月报";

        private string reportLWFBZB = "劳务分包指标统计表";//20160824  

        private string reportQZSPB = "签证索赔情况表";//20160826 
        private Hashtable htDescript = new Hashtable();


        private int endRow;
        private int lastRow;
        private int iCount;
        private int iStart;
        private CommercialReportMaster crMaster;
        private CurrentProjectInfo projectInfo;
        MCostMonthAccount model = new MCostMonthAccount();

        public VCommercialReport()
        {
            InitializeComponent();
            InitEvent();
            InitData();
            LoadData(reportCBJCZB, 0, 0);
        }

        private void InitEvent()
        {
            tabControl.SelectedIndexChanged += new EventHandler(tabControl_SelectedIndexChanged);
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnExl.Click += new EventHandler(btnExl_Click);

            btnAdd_bc.Click += new EventHandler(btnAdd_bc_Click);
            btnDelete_bc.Click += new EventHandler(btnDelete_bc_Click);
            btnSave_bc.Click += new EventHandler(btnSave_bc_Click);
            btnQuery_bc.Click += new EventHandler(btnQuery_bc_Click);
            btnExl_bc.Click += new EventHandler(btnExl_bc_Click);



            #region 专业分包指标统计  沿用的分包指标统计
            btnAdd_bi.Click += new EventHandler(btnAdd_bi_Click);
            btnDelete_bi.Click += new EventHandler(btnDelete_bi_Click);
            btnSave_bi.Click += new EventHandler(btnSave_bi_Click);
            btnQuery_bi.Click += new EventHandler(btnQuery_bi_Click);
            btnExl_bi.Click += new EventHandler(btnExl_bi_Click);
            #endregion

            #region 劳务分包指标统计 从 分包指标统计 中拆出来的 ，界面与分包指标统计一模一样 20160824  
            btnAdd_blw.Click += new EventHandler(btnAdd_blw_Click);
            btnDelete_blw.Click += new EventHandler(btnDelete_blw_Click);
            btnSave_blw.Click += new EventHandler(btnSave_blw_Click);
            btnQuery_blw.Click += new EventHandler(btnQuery_blw_Click);
            btnExl_blw.Click += new EventHandler(btnExl_blw_Click);
            #endregion






            btnAdd_dt.Click += new EventHandler(btnAdd_dt_Click);
            btnDelete_dt.Click += new EventHandler(btnDelete_dt_Click);
            btnSave_dt.Click += new EventHandler(btnSave_dt_Click);
            btnQuery_dt.Click += new EventHandler(btnQuery_dt_Click);
            btnExl_dt.Click += new EventHandler(btnExl_dt_Click);

            btnAdd_sa.Click += new EventHandler(btnAdd_sa_Click);
            btnDelete_sa.Click += new EventHandler(btnDelete_sa_Click);
            btnSave_sa.Click += new EventHandler(btnSave_sa_Click);
            btnQuery_sa.Click += new EventHandler(btnQuery_sa_Click);
            btnExl_sa.Click += new EventHandler(btnExl_sa_Click);

            btnAdd_spr.Click += new EventHandler(btnAdd_spr_Click);
            btnDelete_spr.Click += new EventHandler(btnDelete_spr_Click);
            btnSave_spr.Click += new EventHandler(btnSave_spr_Click);
            btnQuery_spr.Click += new EventHandler(btnQuery_spr_Click);
            btnExl_spr.Click += new EventHandler(btnExl_spr_Click);

            #region 签证索赔情况表
            btnAdd_vc.Click += new EventHandler(btnAdd_vc_Click);
            btnDelete_vc.Click += new EventHandler(btnDelete_vc_Click);
            btnSave_vc.Click += new EventHandler(btnSave_vc_Click);
            btnQuery_vc.Click += new EventHandler(btnQuery_vc_Click);
            btnExl_vc.Click += new EventHandler(btnExl_vc_Click);
            #endregion


        }

        private void InitData()
        {
            InitalDescript();
            projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
            {
                crMaster = new CommercialReportMaster();
                crMaster.ProjectId = projectInfo.Id;
                crMaster.ProjectName = projectInfo.Name;
                crMaster.OperOrgInfo = ConstObject.TheOperationOrg;
                crMaster.OpgSysCode = ConstObject.TheOperationOrg.SysCode;
                crMaster.OperOrgInfoName = ConstObject.TheOperationOrg.Name;
                crMaster.CreatePerson = ConstObject.LoginPersonInfo;
                crMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
            }
            this.LoadFLXFile(reportCBJCZB + ".flx", reportCBJCZB);
            this.LoadFLXFile(reportJCBBB + ".flx", reportJCBBB);
            this.LoadFLXFile(reportFBZB + ".flx", reportFBZB);
            this.LoadFLXFile(reportFBZY + ".flx", reportFBZY);
            this.LoadFLXFile(reportFBZJBS + ".flx", reportFBZJBS);
            this.LoadFLXFile(reportGSJSJZ + ".flx", reportGSJSJZ);

            this.LoadFLXFile(reportLWFBZB + ".flx", reportLWFBZB);//劳务分包指标 20160824

            this.LoadFLXFile(reportQZSPB + ".flx", reportQZSPB);//签证索赔情况表 20160826


            this.btnAdd.Visible = false;
            this.btnDelete.Visible = false;
            this.btnAdd_bc.Visible = false;
            this.btnDelete_bc.Visible = false;
            this.btnAdd_spr.Visible = false;
            this.btnDelete_spr.Visible = false;


            #region  签证和索赔不需要添加和删除  20160831 新需求
            btnAdd_vc.Visible = btnDelete_vc.Visible = false;
            #endregion




            if (projectInfo == null || projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
            {
                btnAdd.Visible = btnDelete.Visible = btnSave.Visible = false;
                btnAdd_bc.Visible = btnDelete_bc.Visible = btnSave_bc.Visible = false;
                btnAdd_dt.Visible = btnDelete_dt.Visible = btnSave_dt.Visible = false;
                btnAdd_sa.Visible = btnDelete_sa.Visible = btnSave_sa.Visible = false;
                //btnAdd_spr
                btnAdd_spr.Visible = btnDelete_spr.Visible = btnSave_spr.Visible = false;
                btnAdd_bi.Visible = false; btnDelete_bi.Visible = false; btnSave_bi.Visible = false;
                btnAdd_blw.Visible = btnDelete_blw.Visible = btnSave_blw.Visible = false;
                btnAdd_vc.Visible = btnDelete_vc.Visible = btnSave_vc.Visible = false;
            }
        }
        public CommercialReportMaster CreateMaster()
        {
            CommercialReportMaster oMaster = new CommercialReportMaster();
            oMaster.ProjectId = projectInfo.Id;
            oMaster.ProjectName = projectInfo.Name;
            oMaster.OperOrgInfo = ConstObject.TheOperationOrg;
            oMaster.OpgSysCode = ConstObject.TheOperationOrg.SysCode;
            oMaster.OperOrgInfoName = ConstObject.TheOperationOrg.Name;
            oMaster.CreatePerson = ConstObject.LoginPersonInfo;
            oMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
            return oMaster;
        }
        private void LoadData(string type, int year, int month)
        {


            try
            {
                #region 成本检查指标统计表
                if (type == reportCBJCZB)
                {
                    fGridDetail.AutoRedraw = false;
                    this.fGridDetail.InsertRow(7, 1);
                    
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
                    if (year == 0)
                    {
                        this.comYear.Text = DateTime.Now.Year.ToString();
                        year = ClientUtil.ToInt(this.comYear.Text);

                    }
                    if (month == 0)
                    {
                        this.comMonth.Text = DateTime.Now.Month.ToString();
                        month = ClientUtil.ToInt(this.comMonth.Text);
                    }

                    for (int i = 8; i <= fGridDetail.Rows - 1; i++)
                    {
                        fGridDetail.Row(i).Delete();
                        i--;
                    }

                    if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                    {
                        CommercialReportMaster crMaster = model.CostMonthAccSrv.GetCommercialMaster(projectInfo.Id, year, month, type);
                        if (!ClientUtil.isEmpty(crMaster) && crMaster.CciDtl.Count > 0)
                        {
                            this.ModelToView(crMaster, reportCBJCZB);
                        }
                        else
                        {
                            this.fGridDetail.InsertRow(7, 1);
                            this.fGridDetail.Cell(7, 29).Text = ConstObject.LoginDate.ToShortDateString();
                            this.fGridDetail.Cell(7, 30).Text = ConstObject.LoginDate.ToShortDateString();
                        }
                        fGridDetail.Cell(1, 1).Text = year + "年" + month + "月" + this.crMaster.ProjectName + reportCBJCZB;
                        this.fGridDetail.Cell(7, 1).Text = "1";
                        this.fGridDetail.Cell(7, 2).Text = projectInfo.Name;
                        AddDescript(type, fGridDetail);
                        
                    }

                }
                #endregion
                #region 局成本报表
                if (type == reportJCBBB)
                {
                    fGridDetail_bc.AutoRedraw = false;
                    this.fGridDetail_bc.InsertRow(8, 1);
                    this.comYear_bc.Items.Clear();
                    this.comMonth_bc.Items.Clear();
                    for (int i = 0; i < 13; i++)
                    {
                        this.comYear_bc.Items.Add(ConstObject.TheLogin.TheComponentPeriod.NowYear + (i - 6));
                    }
                    for (int i = 1; i < 13; i++)
                    {
                        this.comMonth_bc.Items.Add(i);
                    }
                    if (year == 0)
                    {
                        this.comYear_bc.Text = DateTime.Now.Year.ToString();
                        year = ClientUtil.ToInt(this.comYear_bc.Text);
                    }
                    if (month == 0)
                    {
                        this.comMonth_bc.Text = DateTime.Now.Month.ToString();
                        month = ClientUtil.ToInt(this.comMonth_bc.Text);
                    }

                    for (int i = 9; i <= fGridDetail_bc.Rows - 1; i++)
                    {
                        fGridDetail_bc.Row(i).Delete();
                        i--;
                    }

                    if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                    {
                        CommercialReportMaster crMaster = model.CostMonthAccSrv.GetCommercialMaster(projectInfo.Id, year, month, type);
                        if (!ClientUtil.isEmpty(crMaster) && crMaster.BcDtl.Count > 0)
                        {
                            this.ModelToView(crMaster, reportJCBBB);
                        }
                        else
                        {
                            this.fGridDetail_bc.Cell(8, 18).Text = ConstObject.LoginDate.ToShortDateString();
                            this.fGridDetail_bc.Cell(8, 24).Text = ConstObject.LoginDate.ToShortDateString();
                            this.fGridDetail_bc.Cell(8, 27).Text = ConstObject.LoginDate.ToShortDateString();
                            this.fGridDetail_bc.Cell(8, 48).Text = ConstObject.LoginDate.ToShortDateString();
                            this.fGridDetail_bc.Cell(8, 50).Text = ConstObject.LoginDate.ToShortDateString();
                            this.fGridDetail_bc.Cell(8, 73).Text = ConstObject.LoginDate.ToShortDateString();
                            this.fGridDetail_bc.Cell(8, 74).Text = ConstObject.LoginDate.ToShortDateString();
                            this.fGridDetail_bc.Cell(8, 75).Text = ConstObject.LoginDate.ToShortDateString();
                            this.fGridDetail_bc.Cell(8, 76).Text = ConstObject.LoginDate.ToShortDateString();
                        }
                        fGridDetail_bc.Cell(1, 1).Text = year + "年" + month + "月" + this.crMaster.ProjectName + reportJCBBB;
                        this.fGridDetail_bc.Cell(8, 1).Text = "1";
                        this.fGridDetail_bc.Cell(8, 2).Text = projectInfo.Name;
                        AddDescript(type, fGridDetail_bc);
                        
                    }

                }
                #endregion
                #region
                if (type == reportFBZB)
                {
                    fGridDetail_bi.AutoRedraw = false;
                    this.fGridDetail_bi.InsertRow(this.fGridDetail_bi.Rows - 1, 1);
                    this.fGridDetail_bi.Row(this.fGridDetail_bi.Rows - 1).Visible = false;

                    this.comYear_bi.Items.Clear();
                    this.comMonth_bi.Items.Clear();
                    for (int i = 0; i < 13; i++)
                    {
                        this.comYear_bi.Items.Add(ConstObject.TheLogin.TheComponentPeriod.NowYear + (i - 6));
                    }
                    for (int i = 1; i < 13; i++)
                    {
                        this.comMonth_bi.Items.Add(i);
                    }
                    if (year == 0)
                    {
                        this.comYear_bi.Text = DateTime.Now.Year.ToString();
                        year = ClientUtil.ToInt(this.comYear_bi.Text);
                    }
                    if (month == 0)
                    {
                        this.comMonth_bi.Text = DateTime.Now.Month.ToString();
                        month = ClientUtil.ToInt(this.comMonth_bi.Text);
                    }

                    for (int i = 7; i <= fGridDetail_bi.Rows - 2; i++)
                    {
                        fGridDetail_bi.Row(i).Delete();
                        i--;
                    }

                    if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                    {
                        CommercialReportMaster crMaster = model.CostMonthAccSrv.GetCommercialMaster(projectInfo.Id, year, month, type);

                        if (ClientUtil.isEmpty(crMaster))
                        {
                            crMaster = CreateMaster();
                            crMaster.CreateYear = year;
                            crMaster.CreateMonth = month;
                        }

                      
                        //if (!ClientUtil.isEmpty(crMaster) && crMaster.BiDtl.Count > 0)
                        //{
                            this.ModelToView(crMaster, reportFBZB);
                       // }
                        fGridDetail_bi.Cell(1, 1).Text = year + "年" + month + "月" + this.crMaster.ProjectName + reportFBZB;
                        AddDescript(type, fGridDetail_bi);
                    }
                }
                #endregion
                #region 劳务分包指标  20160824
                if (type == reportLWFBZB)
                {
                    fGridDetail_blw.AutoRedraw = false;
                    this.fGridDetail_blw.InsertRow(this.fGridDetail_blw.Rows - 1, 1);
                    this.fGridDetail_blw.Row(this.fGridDetail_blw.Rows - 1).Visible = false;

                    this.comYear_blw.Items.Clear();
                    this.comMonth_blw.Items.Clear();
                    for (int i = 0; i < 13; i++)
                    {
                        this.comYear_blw.Items.Add(ConstObject.TheLogin.TheComponentPeriod.NowYear + (i - 6));
                    }
                    for (int i = 1; i < 13; i++)
                    {
                        this.comMonth_blw.Items.Add(i);
                    }
                    if (year == 0)
                    {
                        this.comYear_blw.Text = DateTime.Now.Year.ToString();
                        year = ClientUtil.ToInt(this.comYear_blw.Text);
                    }
                    if (month == 0)
                    {
                        this.comMonth_blw.Text = DateTime.Now.Month.ToString();
                        month = ClientUtil.ToInt(this.comMonth_blw.Text);
                    }

                    for (int i = 7; i <= fGridDetail_blw.Rows - 2; i++)
                    {
                        fGridDetail_blw.Row(i).Delete();
                        i--;
                    }

                    if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                    {

                        CommercialReportMaster crMaster = model.CostMonthAccSrv.GetCommercialMaster(projectInfo.Id, year, month, type);

                        if (ClientUtil.isEmpty(crMaster))
                        {
                            crMaster = CreateMaster();
                            crMaster.CreateYear = year;
                            crMaster.CreateMonth = month;
                        }
                       // if (!ClientUtil.isEmpty(crMaster) && crMaster.BiDtl.Count > 0)
                       // {
                            this.ModelToView(crMaster, reportLWFBZB);
                        //}
                           // fGridDetail_blw.InsertRow(fGridDetail_blw.Rows-1,1);
                        fGridDetail_blw.Cell(1, 1).Text = year + "年" + month + "月" + this.crMaster.ProjectName + reportLWFBZB;
                        AddDescript(type, fGridDetail_blw);
                    }
                   
                }
                #endregion

                #region 签证索赔情况  20160826
                if (type == reportQZSPB)
                {
                   
                    fGridDetail_vc.AutoRedraw = false;
                    //this.fGridDetail_vc.InsertRow(this.fGridDetail_vc.Rows - 1, 1);
                    //this.fGridDetail_vc.InsertRow(3, 1);
                    //this.fGridDetail_vc.Row(this.fGridDetail_vc.Rows - 1).Visible = false;

                    this.comYear_vc.Items.Clear();
                    this.comMonth_vc.Items.Clear();
                    for (int i = 0; i < 13; i++)
                    {
                        this.comYear_vc.Items.Add(ConstObject.TheLogin.TheComponentPeriod.NowYear + (i - 6));
                    }
                    for (int i = 1; i < 13; i++)
                    {
                        this.comMonth_vc.Items.Add(i);
                    }
                    if (year == 0)
                    {
                        this.comYear_vc.Text = DateTime.Now.Year.ToString();
                        year = ClientUtil.ToInt(this.comYear_vc.Text);
                    }
                    if (month == 0)
                    {
                        this.comMonth_vc.Text = DateTime.Now.Month.ToString();
                        month = ClientUtil.ToInt(this.comMonth_vc.Text);
                    }

 

                    for (int iRow = 5;this.fGridDetail_vc.Rows>6 ; )
                    {
                        this.fGridDetail_vc.Row(iRow).Delete();
                    }
   
                        if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                        {
                            CommercialReportMaster crMaster = model.CostMonthAccSrv.GetCommercialMaster(projectInfo.Id, year, month, type);
                            if (ClientUtil.isEmpty(crMaster))
                            {
                                crMaster = CreateMaster();
                                crMaster.CreateMonth = month;
                                crMaster.CreateYear = year;
                            }


                            if (!ClientUtil.isEmpty(crMaster) && crMaster.VcDtl.Count > 0)
                            {
                                this.ModelToView(crMaster, reportQZSPB);
                               
                            }
                            else
                            {
                                this.fGridDetail_vc.InsertRow(5, 1);
                                this.fGridDetail_vc.Row(5).AutoFit();
                                this.fGridDetail_vc.Cell(5, 1).Text = "1";
                                this.fGridDetail_vc.Cell(5, 2).Text = projectInfo.Name;
                            }
                            fGridDetail_vc.Cell(1, 3).Text = year + "年" + month + "月" + this.crMaster.ProjectName + reportQZSPB;
                            this.fGridDetail_vc.Row(this.fGridDetail_vc.Rows - 1).Visible = false;

                            //this.fGridDetail_vc.Cell(4, 28).Text = ConstObject.LoginDate.ToShortDateString();
                            //this.fGridDetail_vc.Cell(4, 29).Text = ConstObject.LoginDate.ToShortDateString();
                            AddDescript(type, fGridDetail_vc);
                        }
                }
                #endregion

                #region 分包争议
                if (type == reportFBZY)
                {
                    fGridDetail_dt.AutoRedraw = false;
                    this.fGridDetail_dt.InsertRow(this.fGridDetail_dt.Rows - 1, 1);
                    this.fGridDetail_dt.Row(this.fGridDetail_dt.Rows - 1).Visible = false;

                    this.comYear_dt.Items.Clear();
                    this.comMonth_dt.Items.Clear();
                    for (int i = 0; i < 13; i++)
                    {
                        this.comYear_dt.Items.Add(ConstObject.TheLogin.TheComponentPeriod.NowYear + (i - 6));
                    }
                    for (int i = 1; i < 13; i++)
                    {
                        this.comMonth_dt.Items.Add(i);
                    }
                    if (year == 0)
                    {
                        this.comYear_dt.Text = DateTime.Now.Year.ToString();
                        year = ClientUtil.ToInt(this.comYear_dt.Text);
                    }
                    if (month == 0)
                    {
                        this.comMonth_dt.Text = DateTime.Now.Month.ToString();
                        month = ClientUtil.ToInt(this.comMonth_dt.Text);
                    }

                    for (int i = 5; i <= fGridDetail_dt.Rows - 2; i++)
                    {
                        fGridDetail_dt.Row(i).Delete();
                        i--;
                    }

                    if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                    {
                        CommercialReportMaster crMaster = model.CostMonthAccSrv.GetCommercialMaster(projectInfo.Id, year, month, type);

                        if (ClientUtil.isEmpty(crMaster))
                        {
                            crMaster = CreateMaster();
                            crMaster.CreateMonth = month;
                            crMaster.CreateYear = year;
                        }

                        //if (!ClientUtil.isEmpty(crMaster) && crMaster.DtDtl.Count > 0)
                        //{
                            this.ModelToView(crMaster, reportFBZY);
                        //}
                        
                        fGridDetail_dt.Cell(1, 1).Text = year + "年" + month + "月" + this.crMaster.ProjectName + reportFBZY;
                        AddDescript(type, fGridDetail_dt);
                    }
                }
                #endregion

                #region
                if (type == reportFBZJBS)
                {
                    fGridDetail_sa.AutoRedraw = false;
                    this.fGridDetail_sa.InsertRow(this.fGridDetail_sa.Rows - 1, 1);
                    this.fGridDetail_sa.Row(this.fGridDetail_sa.Rows - 1).Visible = false;

                    this.comYear_sa.Items.Clear();
                    this.comMonth_sa.Items.Clear();
                    for (int i = 0; i < 13; i++)
                    {
                        this.comYear_sa.Items.Add(ConstObject.TheLogin.TheComponentPeriod.NowYear + (i - 6));
                    }
                    for (int i = 1; i < 13; i++)
                    {
                        this.comMonth_sa.Items.Add(i);
                    }
                    if (year == 0)
                    {
                        this.comYear_sa.Text = DateTime.Now.Year.ToString();
                        year = ClientUtil.ToInt(this.comYear_sa.Text);
                    }
                    if (month == 0)
                    {
                        this.comMonth_sa.Text = DateTime.Now.Month.ToString();
                        month = ClientUtil.ToInt(this.comMonth_sa.Text);
                    }

                    for (int i = 5; i <= fGridDetail_sa.Rows - 2; i++)
                    {
                        fGridDetail_sa.Row(i).Delete();
                        i--;
                    }

                    if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                    {
                        CommercialReportMaster crMaster = model.CostMonthAccSrv.GetCommercialMaster(projectInfo.Id, year, month, type);
                        if (!ClientUtil.isEmpty(crMaster) && crMaster.SaDtl.Count > 0)
                        {
                            this.ModelToView(crMaster, reportFBZJBS);
                        }
                        fGridDetail_sa.Cell(1, 1).Text = year + "年" + month + "月" + this.crMaster.ProjectName + reportFBZJBS;
                        AddDescript(type, fGridDetail_sa);
                    }
                }
                #endregion
                #region
                if (type == reportGSJSJZ)
                {
                    fGridDetail_spr.AutoRedraw = false;
                    this.fGridDetail_spr.InsertRow(6, 1);

                    this.comYear_spr.Items.Clear();
                    this.comMonth_spr.Items.Clear();
                    for (int i = 0; i < 13; i++)
                    {
                        this.comYear_spr.Items.Add(ConstObject.TheLogin.TheComponentPeriod.NowYear + (i - 6));
                    }
                    for (int i = 1; i < 13; i++)
                    {
                        this.comMonth_spr.Items.Add(i);
                    }
                    if (year == 0)
                    {
                        this.comYear_spr.Text = DateTime.Now.Year.ToString();
                        year = ClientUtil.ToInt(this.comYear_spr.Text);
                    }
                    if (month == 0)
                    {
                        this.comMonth_spr.Text = DateTime.Now.Month.ToString();
                        month = ClientUtil.ToInt(this.comMonth_spr.Text);
                    }

                    for (int i = 7; i <= fGridDetail_spr.Rows - 1; i++)
                    {
                        fGridDetail_spr.Row(i).Delete();
                        i--;
                    }
                    if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                    {
                        CommercialReportMaster crMaster = model.CostMonthAccSrv.GetCommercialMaster(projectInfo.Id, year, month, type);
                        if (!ClientUtil.isEmpty(crMaster) && crMaster.SprDtl.Count > 0)
                        {
                            this.ModelToView(crMaster, reportGSJSJZ);
                        }
                        fGridDetail_spr.Cell(1, 1).Text = year + "年" + month + "月" + this.crMaster.ProjectName + reportGSJSJZ;
                        this.fGridDetail_spr.Cell(6, 1).Text = "1";
                        this.fGridDetail_spr.Cell(6, 2).Text = projectInfo.Name;
                        AddDescript(type, fGridDetail_spr);
                    }

                }
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                fGridDetail.AutoRedraw = true;
                fGridDetail.Refresh();
                fGridDetail_bc.AutoRedraw = true;
                fGridDetail_bc.Refresh();
                fGridDetail_bi.AutoRedraw = true;
                fGridDetail_bi.Refresh();

                #region 劳务分包指标
                fGridDetail_blw.AutoRedraw = true;
                int iCol=fGridDetail_blw.Cols;
                fGridDetail_blw.Column(iCol - 3).Locked = true;
                fGridDetail_blw.Column(iCol - 1).Locked = true;
                fGridDetail_blw.Refresh();
                #endregion

                #region 签证索赔
                fGridDetail_vc.AutoRedraw = true;
                fGridDetail_vc.Refresh();
                #endregion



                fGridDetail_dt.AutoRedraw = true;
                fGridDetail_dt.Refresh();
                fGridDetail_sa.AutoRedraw = true;
                fGridDetail_sa.Refresh();
                fGridDetail_spr.AutoRedraw = true;
                fGridDetail_spr.Refresh();
            }
        }
        string DescriptTag = "Descript";
        void AddDescript(string sType, CustomFlexGrid flexGrid)
        {
            FlexCell.Row oRow = null;
            FlexCell.Range oRange = null;
            FlexCell.Cell oCell = null;
            if (htDescript.Contains(sType))
            {
                int iRowStart = flexGrid.Rows - 1;
                flexGrid.InsertRow(iRowStart, 1);
                if (sType == reportFBZB || sType == reportLWFBZB)
                {
                    oRange = flexGrid.Range(iRowStart, 2, iRowStart, flexGrid.Cols - 4);
                }
                else
                {
                    oRange = flexGrid.Range(iRowStart, 2, iRowStart, flexGrid.Cols - 1);
                }
                oRange.Merge();
                oCell = flexGrid.Cell(iRowStart, 1);
                oCell.Tag = DescriptTag;
                oCell.Alignment = FlexCell.AlignmentEnum.RightTop;
                oCell.Text = "说明:";
                oCell.FontBold = true;
                oCell = flexGrid.Cell(iRowStart, 2);
                oCell.Alignment = FlexCell.AlignmentEnum.LeftTop;
                oCell.WrapText = true;
                oCell.Text = htDescript[sType] as string;
                oRow = flexGrid.Row(iRowStart);
                oRow.AutoFit();
                oRow.Locked = true;

            }

        }
        void InitalDescript()
        {
            #region 描述
            StringBuilder oBuilder = new StringBuilder();
            //pivate string reportCBJCZB = "成本检查指标统计表";
            //private string reportJCBBB = "局成本报表";
            //private string reportFBZB = "专业分包指标统计表";//20160824 其实就是专业分包指标统计表，之前是  专业分包指标统计表 和  劳务分包指标统计表 统称为 分包指标统计表
            //private string reportFBZY = "分包争议统计表";
            //private string reportFBZJBS = "分包终结报审统计表";
            //private string reportGSJSJZ = "公司结算进展月报";

            //private string reportLWFBZB = "劳务分包指标统计表";//20160824  

            //private string reportQZSPB = "签证索赔情况表";//20160826 
            #region 成本检查指标统计表
            oBuilder.Append("1、按季度填报，报表填报月份分别选择3、6、9、12（填报时注意修改月份）；"); oBuilder.Append("\n");
            oBuilder.Append("2、报表填报季度节点日期分别为截止至2.10/5.10/8.10/11.10；"); oBuilder.Append("\n");
            oBuilder.Append("3、填报时注意金额单位，填写百分比的部分需填入百分号；"); oBuilder.Append("\n");
            oBuilder.Append("4、强调现场经费为累计自营产值的比例，注意不要填为预计总产值比例；"); oBuilder.Append("\n");
            oBuilder.Append("5、临时设施所占比例为临时设施预计总成本占预计总产值的比例；"); oBuilder.Append("\n");
            oBuilder.Append("6、商品砼图纸计算量为预算量（不含损耗），钢筋翻样量不要与预算量混淆；"); oBuilder.Append("\n");
            oBuilder.Append("7、产值确认率是指业主书面确认的产值与收入的比例；"); oBuilder.Append("\n");
            oBuilder.Append("8、确权率统计中，确权值原则上为业主书面确认的产值金额；项目支出：指项目已经发生的实际成本（劳务费、材料费、机械费、分包工程费、措施费、现场经费、规费和税金）、已经验收入库但尚未消耗的材料、待摊销的临建及周转材料三者之和，按照成本分析成本与财务提供的项目支出取较高者进行确权率的计算。");
            htDescript.Add(reportCBJCZB, oBuilder.ToString());
            #endregion
            #region 专业分包指标统计表
            oBuilder.Length=0;
            oBuilder.Append("1、合同额=公司签订的合同金额+公司签订的补充协议金额；"); oBuilder.Append("\n");
            oBuilder.Append("2、代工金额为“+”，被代工金额为“-”；"); oBuilder.Append("\n");
            oBuilder.Append("3、项目自签协议归为合同外费用；"); oBuilder.Append("\n");
            oBuilder.Append("4、项目分包指标统计表，本期数是按月来统计的，要求每月办理完分包结算后进行填报。");   
            htDescript.Add(reportFBZB, oBuilder.ToString());
            #endregion
            #region 劳务分包指标统计表
            oBuilder.Length = 0;
            oBuilder.Append("1、合同额=公司签订的合同金额+公司签订的补充协议金额；"); oBuilder.Append("\n");
            oBuilder.Append("2、代工金额为“+”，被代工金额为“-”；"); oBuilder.Append("\n");
            oBuilder.Append("3、项目自签协议归为合同外费用；"); oBuilder.Append("\n");
            oBuilder.Append("4、项目分包指标统计表，本期数是按月来统计的，要求每月办理完分包结算后进行填报。");
            htDescript.Add(reportLWFBZB, oBuilder.ToString());
            #endregion
            #region 签证索赔商务策划统计表
            oBuilder.Length = 0;
            oBuilder.Append("1、按季度填报，报表填报月份分别选择3、6、9、12（填报时注意修改月份）。");
            htDescript.Add(reportQZSPB, oBuilder.ToString());
            #endregion 

            #region 分包争议统计表
            oBuilder.Length = 0;
            oBuilder.Append("1、按季度填报，报表填报月份分别选择3、6、9、12（填报时注意修改月份）。");
            htDescript.Add(reportFBZY, oBuilder.ToString());
            #endregion 
            #region 分包终结报审表
            oBuilder.Length = 0;
            oBuilder.Append("1、按季度填报，报表填报月份分别选择3、6、9、12（填报时注意修改月份）。");
            htDescript.Add(reportFBZJBS, oBuilder.ToString());
            #endregion 
            #endregion
        }
        void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {

           
            if (tabControl.SelectedTab.Text == reportCBJCZB)
            {
                this.LoadData(reportCBJCZB, 0, 0);
            }
            if (tabControl.SelectedTab.Text == reportJCBBB)
            {
                this.LoadData(reportJCBBB, 0, 0);
            }
            if (tabControl.SelectedTab.Text == reportFBZB)
            {
                //MessageBox.Show("劳务分包 直接取到的ID:" + projectInfo.Id.ToString());
                this.LoadData(reportFBZB, 0, 0);
            }
            #region 劳务分包指标
            if (tabControl.SelectedTab.Text == reportLWFBZB)
            {
                //MessageBox.Show("劳务分包 直接取到的ID:" + projectInfo.Id.ToString());
                this.LoadData(reportLWFBZB, 0, 0);
            }
            #endregion

            if (tabControl.SelectedTab.Text == reportFBZY)
            {
                this.LoadData(reportFBZY, 0, 0);
            }
            if (tabControl.SelectedTab.Text == reportFBZJBS)
            {
                this.LoadData(reportFBZJBS, 0, 0);
            }
            if (tabControl.SelectedTab.Text == reportGSJSJZ)
            {
                this.LoadData(reportGSJSJZ, 0, 0);
            }


            #region 签证索赔
            if (tabControl.SelectedTab.Text == reportQZSPB)
            {
                this.LoadData(reportQZSPB, 0, 0);
            }
            #endregion




        }

        #region 成本检查指标统计表
        void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (fGridDetail.Rows - 2 >= 7)
                {
                    MessageBox.Show("只允许添加一行!");
                    return;
                }
                lastRow = fGridDetail.Rows - 2;
                Boolean bl1 = this.CheckRow(lastRow, reportCBJCZB);
                if (bl1 || lastRow < 7)
                {
                    fGridDetail.AutoRedraw = false;
                    fGridDetail.InsertRow(lastRow + 1, 1);
                }
                else
                {
                    MessageBox.Show("请在最后一行输入数据!");
                }
            }
            catch (Exception e1)
            {
                throw new Exception(e1.Message);
            }
            finally
            {
                fGridDetail.AutoRedraw = true;
                fGridDetail.Refresh();
                fGridDetail.Cell(fGridDetail.Rows - 2, 1).Text = ClientUtil.ToString((ClientUtil.ToInt(fGridDetail.Cell(fGridDetail.Rows - 3, 1).Text) + 1));
                this.fGridDetail.Cell(this.fGridDetail.Rows - 2, 2).Text = projectInfo.Name;
                this.fGridDetail.Cell(this.fGridDetail.Rows - 2, 28).Text = ConstObject.LoginDate.ToShortDateString();
                this.fGridDetail.Cell(this.fGridDetail.Rows - 2, 29).Text = ConstObject.LoginDate.ToShortDateString();
            }
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (fGridDetail.Selection.FirstRow >= 7 && fGridDetail.Rows - 2 >= 6)
                {
                    //if (!this.CheckRow(fGridDetail.Selection.FirstRow, reportCBJCZB)) return;
                    if (MessageBox.Show("确认删除选中单元格所在行吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        fGridDetail.Selection.DeleteByRow();
                    }
                }
                else
                {
                    MessageBox.Show("该行不允许删除!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ExceptionUtil.ExceptionMessage(ex));
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

                bool b = this.CheckRow(7, reportCBJCZB);
                //lastRow = fGridDetail.Rows - 2;
                if (!b)
                {
                    MessageBox.Show("请先输入数据!");
                    return;
                }
                
                string type = reportCBJCZB;
                int year = ClientUtil.ToInt(this.comYear.Text);
                int month = ClientUtil.ToInt(this.comMonth.Text);
                Boolean b2 = this.ViewToModel(7, type);
                if (b2)
                {
                    model.CostMonthAccSrv.SaveMasterAndDetail(crMaster);
                }
                MessageBox.Show("保存成功!");
                this.LoadData(type, year, month);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" +ex.Message);//服务器上错误是无法准确捕捉到的
                MessageBox.Show("保存失败:" + ExceptionUtil.ExceptionMessage(ex));
                MessageBox.Show("保存失败:"+(ex.InnerException==null?ex:ex.InnerException).Message);
            }
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            int year = ClientUtil.ToInt(this.comYear.Text);
            int month = ClientUtil.ToInt(this.comMonth.Text);
            this.LoadData(reportCBJCZB, year, month);
        }

        void btnExl_Click(object sender, EventArgs e)
        {
            fGridDetail.ExportToExcel("商务填报", false, false, true);
        }
        #endregion

        #region 局成本报表
        void btnAdd_bc_Click(object sender, EventArgs e)
        {
            try
            {
                string ltext = fGridDetail_bc.Cell(fGridDetail_bc.BottomRow - 1, 1).Text;
                if (ltext == "合计")
                {
                    lastRow = fGridDetail_bc.Rows - 3;
                }
                else
                {
                    lastRow = fGridDetail_bc.Rows - 2;
                }

                if (lastRow >= 8)
                {
                    MessageBox.Show("只允许添加一行!");
                    return;
                }

                Boolean bl1 = this.CheckRow(lastRow, reportJCBBB);
                if (bl1 || lastRow < 8)
                {
                    fGridDetail_bc.AutoRedraw = false;
                    fGridDetail_bc.InsertRow(lastRow + 1, 1);
                    fGridDetail_bc.Cell(lastRow + 1, 1).Text = ClientUtil.ToString((ClientUtil.ToInt(fGridDetail_bc.Cell(lastRow, 1).Text) + 1));
                    this.fGridDetail_bc.Cell(this.lastRow + 1, 2).Text = projectInfo.Name;
                    this.fGridDetail_bc.Cell(this.lastRow + 1, 18).Text = ConstObject.LoginDate.ToShortDateString();
                    this.fGridDetail_bc.Cell(this.lastRow + 1, 24).Text = ConstObject.LoginDate.ToShortDateString();
                    this.fGridDetail_bc.Cell(this.lastRow + 1, 27).Text = ConstObject.LoginDate.ToShortDateString();
                    this.fGridDetail_bc.Cell(this.lastRow + 1, 48).Text = ConstObject.LoginDate.ToShortDateString();
                    this.fGridDetail_bc.Cell(this.lastRow + 1, 50).Text = ConstObject.LoginDate.ToShortDateString();
                    this.fGridDetail_bc.Cell(this.lastRow + 1, 73).Text = ConstObject.LoginDate.ToShortDateString();
                    this.fGridDetail_bc.Cell(this.lastRow + 1, 74).Text = ConstObject.LoginDate.ToShortDateString();
                    this.fGridDetail_bc.Cell(this.lastRow + 1, 75).Text = ConstObject.LoginDate.ToShortDateString();
                    this.fGridDetail_bc.Cell(this.lastRow + 1, 76).Text = ConstObject.LoginDate.ToShortDateString();
                }
                else
                {
                    MessageBox.Show("请在最后一行输入数据!");
                    return;
                }
            }
            catch (Exception e1)
            {
                throw new Exception(e1.Message);
            }
            finally
            {
                fGridDetail_bc.AutoRedraw = true;
                fGridDetail_bc.Refresh();
            }
        }

        void btnDelete_bc_Click(object sender, EventArgs e)
        {
            try
            {
                if (fGridDetail_bc.Selection.FirstRow >= 8 && fGridDetail_bc.Rows - 1 >= 8)
                {
                    //if (!this.CheckRow(fGridDetail_bc.Selection.FirstRow, reportJCBBB)) return;
                    if (MessageBox.Show("确认删除选中单元格所在行吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        fGridDetail_bc.Selection.DeleteByRow();
                    }
                }
                else
                {
                    MessageBox.Show("该行不允许删除!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        void btnSave_bc_Click(object sender, EventArgs e)
        {
            try
            {
                if (projectInfo == null || projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                {
                    MessageBox.Show("系统管理员不允许添加!");
                    return;
                }

                bool b = this.CheckRow(8, reportJCBBB);
                
                if (!b)
                {
                    MessageBox.Show("请先在最后一行输入数据!");
                    return;
                }
                
                string type = reportJCBBB;
                int year = ClientUtil.ToInt(this.comYear_bc.Text);
                int month = ClientUtil.ToInt(this.comMonth_bc.Text);
                Boolean b2 = this.ViewToModel(8, type);
                if (b2)
                {
                    model.CostMonthAccSrv.SaveMasterAndDetail(crMaster);
                }
                MessageBox.Show("保存成功!");
                this.LoadData(type, year, month);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:"+ ExceptionUtil.ExceptionMessage(ex));
            }
        }

        void btnQuery_bc_Click(object sender, EventArgs e)
        {
            int year = ClientUtil.ToInt(this.comYear_bc.Text);
            int month = ClientUtil.ToInt(this.comMonth_bc.Text);
            this.LoadData(reportJCBBB, year, month);
        }

        void btnExl_bc_Click(object sender, EventArgs e)
        {
            fGridDetail_bc.ExportToExcel("商务填报", false, false, true);
        }

        #endregion

        #region 专业分包指标统计表
        void btnAdd_bi_Click(object sender, EventArgs e)
        {
            try
            {
                lastRow = fGridDetail_bi.Rows - 3;
                //fGridDetail_bi.AutoRedraw = false;
                //fGridDetail_bi.InsertRow(lastRow + 1, 1);


#region 在合并之前插入列
                //得到合计之前的序号     
                int num = 0;
                for (int i = 7; i < lastRow; i++)
                {
                    if (fGridDetail_bi.Row(i).Visible)
                    {
                        num++;
                    }
                }
                // int num = ClientUtil.ToInt(fGridDetail_blw.Cell(fGridDetail_blw.Rows - 3, 1).Text);
                num++;
                fGridDetail_bi.InsertRow(lastRow, 1);//在合计之前插入行
                fGridDetail_bi.Cell(lastRow, 1).Text = ClientUtil.ToString(num);
                fGridDetail_bi.Cell(lastRow, 2).Text = projectInfo.Name;
                fGridDetail_bi.Row(lastRow).Visible = true;
#endregion

            }
            catch (Exception e1)
            {
                throw new Exception(e1.Message);
            }
            finally
            {
                fGridDetail_bi.AutoRedraw = true;
                fGridDetail_bi.Refresh();
                //fGridDetail_bi.Cell(this.lastRow + 1, 1).Text = ClientUtil.ToString((ClientUtil.ToInt(fGridDetail_bi.Cell(this.lastRow, 1).Text) + 1));
                //fGridDetail_bi.Cell(this.lastRow + 1, 2).Text = projectInfo.Name;
            }
        }

        void btnDelete_bi_Click(object sender, EventArgs e)
        {
            try
            {
                int iSelectRow = fGridDetail_bi.Selection.FirstRow;
                if (iSelectRow >= 7 && fGridDetail_bi.Rows - 3 > iSelectRow)
               // if (fGridDetail_bi.Selection.FirstRow >= 7 && fGridDetail_bi.Rows - 2 >= 7)
                {
                    if (MessageBox.Show("确认删除选中单元格所在行吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        //fGridDetail_bi.Selection.DeleteByRow();
                        fGridDetail_bi.Row(fGridDetail_bi.Selection.FirstRow).Visible = false;
                    }
                }
                else
                {
                    MessageBox.Show("该行不允许删除!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        void btnSave_bi_Click(object sender, EventArgs e)
        {
            try
            {
                if (projectInfo == null || projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                {
                    MessageBox.Show("系统管理员不允许添加!");
                    return;
                }
                lastRow = 0;

                

                        for (int i = 7; i <= fGridDetail_bi.Rows - 3; i++)
                        {
                            if (fGridDetail_bi.Row(i).Visible)
                            {
                                Boolean b1 = this.CheckRow(i, reportFBZB);
                                if (!b1)
                                {
                                    MessageBox.Show("请先删除空行!");
                                    return;
                                }
                            }
                            lastRow += 1;
                        }
                        int year = ClientUtil.ToInt(this.comYear_bi.Text);
                        int month = ClientUtil.ToInt(this.comMonth_bi.Text);
                Boolean b2 = this.ViewToModel(lastRow + 6, reportFBZB);
                if (b2)
                {
                    if (string.IsNullOrEmpty(crMaster.Id))
                    {
                        crMaster.CreateMonth = month;
                        crMaster.CreateYear = year;
                    }
                   
                    model.CostMonthAccSrv.SaveMasterAndDetail(crMaster);
                }
                MessageBox.Show("保存成功!");
               
                this.LoadData(reportFBZB, year, month);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        void btnQuery_bi_Click(object sender, EventArgs e)
        {
            int year = ClientUtil.ToInt(this.comYear_bi.Text);
            int month = ClientUtil.ToInt(this.comMonth_bi.Text);
            this.LoadData(reportFBZB, year, month);
        }

        void btnExl_bi_Click(object sender, EventArgs e)
        {
            fGridDetail_bi.ExportToExcel("商务填报", false, false, true);
        }

        #endregion


        #region 劳务分包指标统计表 20160824
        void btnAdd_blw_Click(object sender, EventArgs e)
        {
            try
            {
                lastRow = fGridDetail_blw.Rows - 3;
                //fGridDetail_blw.AutoRedraw = false;
                //fGridDetail_blw.InsertRow(lastRow + 1, 1);


                #region 在合并之前插入列
                //得到合计之前的序号
                int num =0;
                for (int i = 7; i < lastRow; i++)
                {
                    if (fGridDetail_blw.Row(i).Visible)
                    {
                        num++;
                    }
                }
                    // int num = ClientUtil.ToInt(fGridDetail_blw.Cell(fGridDetail_blw.Rows - 3, 1).Text);
                    num++;
                fGridDetail_blw.InsertRow(lastRow, 1);//在合计之前插入行
                fGridDetail_blw.Cell(lastRow, 1).Text = ClientUtil.ToString(num);
                fGridDetail_blw.Cell(lastRow, 2).Text = projectInfo.Name;
                fGridDetail_blw.Row(lastRow).Visible = true;
                #endregion

            }
            catch (Exception e1)
            {
                throw new Exception(e1.Message);
            }
            finally
            {
                fGridDetail_blw.AutoRedraw = true;
                fGridDetail_blw.Refresh();
                //fGridDetail_blw.Cell(this.lastRow + 1, 1).Text = ClientUtil.ToString((ClientUtil.ToInt(fGridDetail_blw.Cell(this.lastRow, 1).Text) + 1));
                //fGridDetail_blw.Cell(this.lastRow + 1, 2).Text = projectInfo.Name;
            }
        }

        void btnDelete_blw_Click(object sender, EventArgs e)
        {
            try
            {
                int iSelectRow = fGridDetail_blw.Selection.FirstRow;
                if (iSelectRow >= 7 && fGridDetail_blw.Rows - 3 > iSelectRow)
                {
                    if (MessageBox.Show("确认删除选中单元格所在行吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                       // fGridDetail_blw.Selection.DeleteByRow();
                        fGridDetail_blw.Row(fGridDetail_blw.Selection.FirstRow).Visible = false;
                    }
                }
                else
                {
                    MessageBox.Show("该行不允许删除!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        void btnSave_blw_Click(object sender, EventArgs e)
        {
            try
            {
                if (projectInfo == null || projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                {
                    MessageBox.Show("系统管理员不允许添加!");
                    return;
                }
                lastRow = 0;
                for (int i = 7; i < fGridDetail_blw.Rows - 3; i++)//加合并
                    //for (int i = 7; i <= fGridDetail_blw.Rows - 2; i++)//不加合并
                {
                    if (fGridDetail_blw.Row(i).Visible)
                    {
                        Boolean b1 = this.CheckRow(i, reportLWFBZB);
                        if (!b1)
                        {
                            MessageBox.Show("请先删除空行!");
                            return;
                        }
                    }
                    lastRow += 1;
                }
                Boolean b2 = this.ViewToModel(lastRow + 6, reportLWFBZB);
                if (b2)
                {
                    model.CostMonthAccSrv.SaveMasterAndDetail(crMaster);
                }
                MessageBox.Show("保存成功!");
                int year = ClientUtil.ToInt(this.comYear_blw.Text);
                int month = ClientUtil.ToInt(this.comMonth_blw.Text);
                this.LoadData(reportLWFBZB, year, month);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        void btnQuery_blw_Click(object sender, EventArgs e)
        {
            int year = ClientUtil.ToInt(this.comYear_blw.Text);
            int month = ClientUtil.ToInt(this.comMonth_blw.Text);
            this.LoadData(reportLWFBZB, year, month);
        }

        void btnExl_blw_Click(object sender, EventArgs e)
        {
            fGridDetail_blw.ExportToExcel("商务填报", false, false, true);
        }

        #endregion


        #region 签证索赔表 20160826
        void btnAdd_vc_Click(object sender, EventArgs e)
        {

           


            try
            {
                
                //if (fGridDetail_vc.Rows >= 7)
                //{
                //    MessageBox.Show("只允许添加一行!");
                //    return;
                //}
                lastRow = fGridDetail_vc.Rows - 2;
                fGridDetail_vc.AutoRedraw = false;
                //点添加的时候，添加的行应该加在合计之前，否则影响美观
                //得到合计之前的序号     
                int num = ClientUtil.ToInt(fGridDetail_vc.Cell(fGridDetail_vc.Rows - 3, 1).Text);
                num++;

                //fGridDetail_vc.InsertRow(lastRow + 1, 1);//在合计之后插入行
                fGridDetail_vc.InsertRow(lastRow, 1);//在合计之前插入行
                //fGridDetail_vc.Cell(this.lastRow, 1).Text = ClientUtil.ToString((ClientUtil.ToInt(fGridDetail_vc.Cell(fGridDetail_vc.Rows - 3, 1).Text) + 1));
                fGridDetail_vc.Cell(this.lastRow, 1).Text = ClientUtil.ToString(num);
                fGridDetail_vc.Cell(this.lastRow, 2).Text = projectInfo.Name;

                //fGridDetail_vc.Cell(this.lastRow + 1, 1).Text = ClientUtil.ToString((ClientUtil.ToInt(fGridDetail_vc.Cell(this.lastRow, 1).Text) + 1));
                //fGridDetail_vc.Cell(this.lastRow + 1, 2).Text = projectInfo.Name;
               

              
 
            }
            catch (Exception e1)
            {
                throw new Exception(e1.Message);
            }
            finally
            {
                fGridDetail_vc.AutoRedraw = true;
                fGridDetail_vc.Refresh();
                //fGridDetail_vc.Cell(this.lastRow, 1).Text = ClientUtil.ToString((ClientUtil.ToInt(fGridDetail_vc.Cell(fGridDetail_vc.Rows - 3, 1).Text) + 1));
                ////得到合计之前的序号     
                //int num = ClientUtil.ToInt(fGridDetail_vc.Cell(fGridDetail_vc.Rows - 3, 1).Text);
                //num++;

                //fGridDetail_vc.Cell(this.lastRow, 1).Text = ClientUtil.ToString(num);
                //fGridDetail_vc.Cell(this.lastRow, 2).Text = projectInfo.Name;

                //fGridDetail_vc.Cell(this.lastRow + 1, 1).Text = ClientUtil.ToString((ClientUtil.ToInt(fGridDetail_vc.Cell(this.lastRow, 1).Text) + 1));
                //fGridDetail_vc.Cell(this.lastRow + 1, 2).Text = projectInfo.Name;
            }
        }

        void btnDelete_vc_Click(object sender, EventArgs e)
        {
            try
            {
                //if (fGridDetail_vc.Selection.FirstRow >= 4 && fGridDetail_vc.Rows - 2 >= 4)
                if (fGridDetail_vc.Selection.FirstRow >= 4)
                {
                    if (MessageBox.Show("确认删除选中单元格所在行吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        fGridDetail_vc.Selection.DeleteByRow();
                    }
                }
                else
                {
                    MessageBox.Show("该行不允许删除!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        void btnSave_vc_Click(object sender, EventArgs e)
        {
            try
            {
                if (projectInfo == null || projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                {
                    MessageBox.Show("系统管理员不允许添加!");
                    return;
                }
                lastRow = 0;

                ////for (int i = 4; i < fGridDetail_vc.Rows - 3; i++)//加合并
                //for (int i = 4; i <= fGridDetail_vc.Rows - 3; i++)//不加合并
                //{
                //    Boolean b1 = this.CheckRow(i, reportQZSPB);
                //    if (!b1)
                //    {
                //        MessageBox.Show("请先删除空行!");
                //        return;
                //    }
                //    lastRow += 1;
                //}
                //Boolean b2 = this.ViewToModel(lastRow + 4, reportQZSPB);
                Boolean b2 = this.ViewToModel(5, reportQZSPB);
                if (b2)
                {

                    model.CostMonthAccSrv.SaveMasterAndDetail(crMaster);
                }
                MessageBox.Show("保存成功!");
                int year = ClientUtil.ToInt(this.comYear_vc.Text);
                int month = ClientUtil.ToInt(this.comMonth_vc.Text);
                this.LoadData(reportQZSPB, year, month);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        void btnQuery_vc_Click(object sender, EventArgs e)
        {
            int year = ClientUtil.ToInt(this.comYear_vc.Text);
            int month = ClientUtil.ToInt(this.comMonth_vc.Text);
            this.LoadData(reportQZSPB, year, month);
        }

        void btnExl_vc_Click(object sender, EventArgs e)
        {
            fGridDetail_vc.ExportToExcel("商务填报", false, false, true);
        }

        #endregion

        #region 分包争议统计表
        void btnAdd_dt_Click(object sender, EventArgs e)
        {
            try
            {
                lastRow = fGridDetail_dt.Rows - 3;
                Boolean bl1 = this.CheckRow(lastRow, reportFBZY);
                if (bl1 || lastRow < 5)
                {
                    #region  没有加合并之前的代码
                    //fGridDetail_dt.AutoRedraw = false;
                    //fGridDetail_dt.InsertRow(lastRow + 1, 1);
                    #endregion 

                    #region  加了合并之后的代码  20160830 
                    lastRow = fGridDetail_dt.Rows - 3;
                    fGridDetail_dt.AutoRedraw = false;
                    //点添加的时候，添加的行应该加在合计之前，否则影响美观
                    //得到合计之前的序号     
                    int num = ClientUtil.ToInt(fGridDetail_dt.Cell(fGridDetail_dt.Rows - 4, 1).Text);
                    num++;

                    fGridDetail_dt.InsertRow(lastRow, 1);//在合计之前插入行
                    fGridDetail_dt.Cell(this.lastRow, 1).Text = ClientUtil.ToString(num);
                    fGridDetail_dt.Cell(this.lastRow, 2).Text = projectInfo.Name;
                    #endregion

                }
                else
                {
                    MessageBox.Show("请在最后一行输入数据!");
                }
            }
            catch (Exception e1)
            {
                throw new Exception(e1.Message);
            }
            finally
            {
                fGridDetail_dt.AutoRedraw = true;
                fGridDetail_dt.Refresh();
                //fGridDetail_dt.Cell(fGridDetail_dt.Rows - 2, 1).Text = ClientUtil.ToString((ClientUtil.ToInt(fGridDetail_dt.Cell(fGridDetail_dt.Rows - 3, 1).Text) + 1));
                //this.fGridDetail_dt.Cell(this.fGridDetail_dt.Rows - 2, 2).Text = projectInfo.Name;
            }
        }

        void btnDelete_dt_Click(object sender, EventArgs e)
        {
            try
            {
                if (fGridDetail_dt.Selection.FirstRow >= 5 && fGridDetail_dt.Rows - 4 >= fGridDetail_dt.Selection.FirstRow)
                {
                    //if (!this.CheckRow(fGridDetail_dt.Selection.FirstRow, reportFBZY)) return;
                    if (MessageBox.Show("确认删除选中单元格所在行吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        fGridDetail_dt.Selection.DeleteByRow();
                    }
                }
                else
                {
                    MessageBox.Show("该行不允许删除!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        void btnSave_dt_Click(object sender, EventArgs e)
        {
            try
            {
                if (projectInfo == null || projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                {
                    MessageBox.Show("系统管理员不允许添加!");
                    return;
                }

                bool b = this.CheckRow(5, reportFBZY);
                lastRow = fGridDetail_dt.Rows - 3;
                if (!b)
                {
                    MessageBox.Show("请先在最后一行输入数据!");
                    return;
                }
                else
                {
                    Boolean b1 = this.CheckRow(lastRow, reportFBZY);
                    if (b1)
                    {
                        endRow = lastRow;
                    }
                    else
                    {
                        endRow = lastRow - 1;
                    }
                }
                Boolean b2 = this.ViewToModel(endRow, reportFBZY);
                if (b2)
                {
                    model.CostMonthAccSrv.SaveMasterAndDetail(crMaster);
                }
                MessageBox.Show("保存成功!");
                int year = ClientUtil.ToInt(this.comYear_dt.Text);
                int month = ClientUtil.ToInt(this.comMonth_dt.Text);
                this.LoadData(reportFBZY, year, month);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        void btnQuery_dt_Click(object sender, EventArgs e)
        {
            int year = ClientUtil.ToInt(this.comYear_dt.Text);
            int month = ClientUtil.ToInt(this.comMonth_dt.Text);
            this.LoadData(reportFBZY, year, month);
        }

        void btnExl_dt_Click(object sender, EventArgs e)
        {
            fGridDetail_dt.ExportToExcel("商务填报", false, false, true);
        }

        #endregion

        #region 分包终结报审统计表
        void btnAdd_sa_Click(object sender, EventArgs e)
        {
            try
            {
                lastRow = fGridDetail_sa.Rows - 3;
                Boolean bl1 = this.CheckRow(lastRow, reportFBZJBS);
                if (bl1 || lastRow < 5)
                {
                    fGridDetail_sa.AutoRedraw = false;
                    fGridDetail_sa.InsertRow(lastRow + 1, 1);
                }
                else
                {
                    MessageBox.Show("请在最后一行输入数据!");
                }
            }
            catch (Exception e1)
            {
                throw new Exception(e1.Message);
            }
            finally
            {
                this.fGridDetail_sa.AutoRedraw = true;
                this.fGridDetail_sa.Refresh();
                this.fGridDetail_sa.Cell(this.fGridDetail_sa.Rows - 3, 1).Text = ClientUtil.ToString((ClientUtil.ToInt(fGridDetail_sa.Cell(fGridDetail_sa.Rows - 4, 1).Text) + 1));
                this.fGridDetail_sa.Cell(this.fGridDetail_sa.Rows - 3, 2).Text = projectInfo.Name;
                this.fGridDetail_sa.Cell(this.fGridDetail_sa.Rows - 3, 7).Text = ConstObject.LoginDate.ToShortDateString();
                this.fGridDetail_sa.Cell(this.fGridDetail_sa.Rows - 3, 10).Text = ConstObject.LoginDate.ToShortDateString();
            }
        }

        void btnDelete_sa_Click(object sender, EventArgs e)
        {
            try
            {
                if (fGridDetail_sa.Selection.FirstRow >= 5 && fGridDetail_sa.Rows - 3 >= fGridDetail_sa.Selection.FirstRow)
                {
                    //if (!this.CheckRow(fGridDetail_sa.Selection.FirstRow, reportFBZJBS)) return;
                    if (MessageBox.Show("确认删除选中单元格所在行吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        fGridDetail_sa.Selection.DeleteByRow();
                    }
                }
                else
                {
                    MessageBox.Show("该行不允许删除!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        void btnSave_sa_Click(object sender, EventArgs e)
        {
            try
            {
                if (projectInfo == null || projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                {
                    MessageBox.Show("系统管理员不允许添加!");
                    return;
                }

                bool b = this.CheckRow(5, reportFBZJBS);
                lastRow = fGridDetail_sa.Rows - 3;
                if (!b)
                {
                    MessageBox.Show("请先在最后一行输入数据!");
                    return;
                }
                else
                {
                    Boolean b1 = this.CheckRow(lastRow, reportFBZJBS);
                    if (b1)
                    {
                        endRow = lastRow;
                    }
                    else
                    {
                        endRow = lastRow - 1;
                    }
                }
                Boolean b2 = this.ViewToModel(endRow, reportFBZJBS);
                if (b2)
                {
                    model.CostMonthAccSrv.SaveMasterAndDetail(crMaster);
                }
                MessageBox.Show("保存成功!");
                int year = ClientUtil.ToInt(this.comYear_sa.Text);
                int month = ClientUtil.ToInt(this.comMonth_sa.Text);
                this.LoadData(reportFBZJBS, year, month);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        void btnQuery_sa_Click(object sender, EventArgs e)
        {
            int year = ClientUtil.ToInt(this.comYear_sa.Text);
            int month = ClientUtil.ToInt(this.comMonth_sa.Text);
            this.LoadData(reportFBZJBS, year, month);
        }

        void btnExl_sa_Click(object sender, EventArgs e)
        {
            fGridDetail_sa.ExportToExcel("商务填报", false, false, true);
        }

        #endregion

        #region 公司结算进展月报
        void btnAdd_spr_Click(object sender, EventArgs e)
        {
            try
            {
                if (fGridDetail_spr.Rows - 2 >= 6)
                {
                    MessageBox.Show("只允许添加一行!");
                    return;
                }
                lastRow = fGridDetail_spr.Rows - 2;
                Boolean bl1 = this.CheckRow(lastRow, reportGSJSJZ);
                if (bl1 || lastRow < 6)
                {
                    fGridDetail_spr.AutoRedraw = false;
                    fGridDetail_spr.InsertRow(lastRow + 1, 1);
                }
                else
                {
                    MessageBox.Show("请在最后一行输入数据!");
                }
            }
            catch (Exception e1)
            {
                throw new Exception(e1.Message);
            }
            finally
            {
                this.fGridDetail_spr.AutoRedraw = true;
                this.fGridDetail_spr.Refresh();
                this.fGridDetail_spr.Cell(this.fGridDetail_spr.Rows - 2, 1).Text = ClientUtil.ToString((ClientUtil.ToInt(fGridDetail_spr.Cell(fGridDetail_spr.Rows - 3, 1).Text) + 1));
                this.fGridDetail_spr.Cell(this.fGridDetail_spr.Rows - 2, 2).Text = projectInfo.Name;
            }
        }

        void btnDelete_spr_Click(object sender, EventArgs e)
        {
            try
            {
                if (fGridDetail_spr.Selection.FirstRow >= 6 && fGridDetail_spr.Rows - 2 >= 5)
                {
                    //if (!this.CheckRow(fGridDetail_spr.Selection.FirstRow, reportGSJSJZ)) return;
                    if (MessageBox.Show("确认删除选中单元格所在行吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        fGridDetail_spr.Selection.DeleteByRow();
                    }
                }
                else
                {
                    MessageBox.Show("该行不允许删除!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        void btnSave_spr_Click(object sender, EventArgs e)
        {
            try
            {
                if (projectInfo == null || projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                {
                    MessageBox.Show("系统管理员不允许添加!");
                    return;
                }

                bool b = this.CheckRow(6, reportGSJSJZ);
                //lastRow = fGridDetail_spr.Rows - 2;
                if (!b)
                {
                    MessageBox.Show("请先在最后一行输入数据!");
                    return;
                }
                
                Boolean b2 = this.ViewToModel(6, reportGSJSJZ);
                if (b2)
                {
                    model.CostMonthAccSrv.SaveMasterAndDetail(crMaster);
                }
                MessageBox.Show("保存成功!");
                int year = ClientUtil.ToInt(this.comYear_spr.Text);
                int month = ClientUtil.ToInt(this.comMonth_spr.Text);
                this.LoadData(reportGSJSJZ, year, month);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        void btnQuery_spr_Click(object sender, EventArgs e)
        {
            int year = ClientUtil.ToInt(this.comYear_spr.Text);
            int month = ClientUtil.ToInt(this.comMonth_spr.Text);
            this.LoadData(reportGSJSJZ, year, month);
        }

        void btnExl_spr_Click(object sender, EventArgs e)
        {
            fGridDetail_spr.ExportToExcel("商务填报", false, false, true);
        }

        #endregion

        private bool CheckRow(int row, string type)
        {
            if (type == reportCBJCZB)
            {
                for (int i = 3; i < fGridDetail.Cols; i++)
                {
                    string value = fGridDetail.Cell(row, i).Text;
                    if (i == 29 || i == 30) continue;
                    if (!ClientUtil.isEmpty(value))
                    {
                        return true;
                    }
                    continue;
                }
            }
            if (type == reportJCBBB)
            {
                for (int i = 3; i < fGridDetail_bc.Cols; i++)
                {
                    string value = fGridDetail_bc.Cell(row, i).Text;
                    if (i == 18 || i == 24 || i == 27 || i == 48 || i == 50 || i == 73 || i == 74 || i == 75 || i == 76) continue;
                    if (!ClientUtil.isEmpty(value))
                    {
                        return true;
                    }
                    continue;
                }
            }
            if (type == reportFBZB)
            {
                for (int i = 3; i < fGridDetail_bi.Cols; i++)
                {
                    string value = fGridDetail_bi.Cell(row, i).Text;
                    if (!ClientUtil.isEmpty(value))
                    {
                        return true;
                    }
                    continue;
                }
            }
            #region 劳务分包指标
            if (type == reportLWFBZB)
            {
                for (int i = 3; i < fGridDetail_blw.Cols; i++)
                {
                    string value = fGridDetail_blw.Cell(row, i).Text;
                    if (!ClientUtil.isEmpty(value))
                    {
                        return true;
                    }
                    continue;
                }
            }
            #endregion

            #region 签证索赔情况
            if (type == reportQZSPB)
            {
                for (int i = 5; i < fGridDetail_vc.Cols; i++)
                {
                    string value = fGridDetail_vc.Cell(row, i).Text;
                    if (!ClientUtil.isEmpty(value))
                    {
                        return true;
                    }
                    continue;
                }
            }
            #endregion



            if (type == reportFBZY)
            {
                for (int i = 3; i < fGridDetail_dt.Cols; i++)
                {
                    string value = fGridDetail_dt.Cell(row, i).Text;
                    if (!ClientUtil.isEmpty(value))
                    {
                        return true;
                    }
                    continue;
                }
            }
            if (type == reportFBZJBS)
            {
                for (int i = 3; i < fGridDetail_sa.Cols; i++)
                {
                    string value = fGridDetail_sa.Cell(row, i).Text;
                    if (i == 7 || i == 10) continue;
                    if (!ClientUtil.isEmpty(value))
                    {
                        return true;
                    }
                    continue;
                }
            }
            if (type == reportGSJSJZ)
            {
                for (int i = 3; i < fGridDetail_spr.Cols; i++)
                {
                    string value = fGridDetail_spr.Cell(row, i).Text;
                    if (!ClientUtil.isEmpty(value))
                    {
                        return true;
                    }
                    continue;
                }
            }
            return false;
        }

        private void LoadFLXFile(string flxname, string type)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(flxname))
            {
                eFile.CreateTempleteFileFromServer(flxname);
                if (type == reportCBJCZB)
                {
                    fGridDetail.OpenFile(path + "\\" + flxname);//载入格式
                }
                if (type == reportJCBBB)
                {
                    fGridDetail_bc.OpenFile(path + "\\" + flxname);//载入格式
                }
                if (type == reportFBZB)
                {
                    fGridDetail_bi.OpenFile(path + "\\" + flxname);//载入格式
                }
                #region  劳务分包指标
                if (type == reportLWFBZB)
                {
                    fGridDetail_blw.OpenFile(path + "\\" + flxname);//载入格式
                }
                #endregion

                if (type == reportFBZY)
                {
                    fGridDetail_dt.OpenFile(path + "\\" + flxname);//载入格式
                }
                if (type == reportFBZJBS)
                {
                    fGridDetail_sa.OpenFile(path + "\\" + flxname);//载入格式
                }
                if (type == reportGSJSJZ)
                {
                    fGridDetail_spr.OpenFile(path + "\\" + flxname);//载入格式
                }

                #region  签证索赔
                if (type == reportQZSPB)
                {
                    fGridDetail_vc.OpenFile(path + "\\" + flxname);//载入格式
                }
                #endregion

            }
        }

        private Boolean ViewToModel(int endRow, string type)
        {
            int iOrderNo = 0;
            try
            {
                #region
                if (type == reportCBJCZB)
                {
                    int year = ClientUtil.ToInt(this.comYear.Text);
                    int month = ClientUtil.ToInt(this.comMonth.Text);
                    CommercialReportMaster cr = model.CostMonthAccSrv.GetCommercialMaster(crMaster.ProjectId, year, month, type);
                    if (!ClientUtil.isEmpty(cr))
                    {
                        crMaster = cr;
                        crMaster.CciDtl.Clear();
                    }
                    else
                       
                    {
                        crMaster = CreateMaster();
                        crMaster.CreateYear = year;
                        crMaster.CreateMonth = month;
                        crMaster.CreateDate = ConstObject.LoginDate;
                    }
                    for (int i = 7; i <= endRow; i++)
                    {
                        CostCheckIndicatorDtl cciDtl = new CostCheckIndicatorDtl();
                        cciDtl.Master = crMaster;
                        cciDtl.TenderCalculatePoint = ClientUtil.ToString(fGridDetail.Cell(i, 3).Text);
                        cciDtl.TenderCalculatePoint = GetRate(  cciDtl.TenderCalculatePoint  );
                        cciDtl.LiabilityPaid = ClientUtil.ToString(fGridDetail.Cell(i, 4).Text);
                        cciDtl.LiabilityPaid = GetRate( cciDtl.LiabilityPaid );
                        cciDtl.SelfPlanMoney = ClientUtil.ToDecimal(fGridDetail.Cell(i, 5).Text);
                        cciDtl.QuarterBenefitRate = ClientUtil.ToString(fGridDetail.Cell(i, 6).Text);
                        cciDtl.ConstructionContractIncome = ClientUtil.ToString(fGridDetail.Cell(i, 7).Text);
                        cciDtl.Income = ClientUtil.ToDecimal(fGridDetail.Cell(i, 8).Text);
                        cciDtl.Cost = ClientUtil.ToDecimal(fGridDetail.Cell(i, 9).Text);
                        cciDtl.BenefitAmount = ClientUtil.ToDecimal(fGridDetail.Cell(i, 10).Text);
                        cciDtl.BenefitRate = GetRate( ClientUtil.ToString(fGridDetail.Cell(i, 11).Text));
                        cciDtl.DutyCost = ClientUtil.ToDecimal(fGridDetail.Cell(i, 12).Text);
                        cciDtl.OverCostReduceRate =GetRate( ClientUtil.ToString(fGridDetail.Cell(i, 13).Text));
                        cciDtl.SiteFunds = ClientUtil.ToDecimal(fGridDetail.Cell(i, 14).Text);
                        cciDtl.TotalOutputValueAccount =GetRate( ClientUtil.ToString(fGridDetail.Cell(i, 15).Text));
                        cciDtl.OccurredMoney = ClientUtil.ToDecimal(fGridDetail.Cell(i, 16).Text);
                        cciDtl.ExpectOccurredMoney = ClientUtil.ToDecimal(fGridDetail.Cell(i, 17).Text);
                        cciDtl.TotalMoney = ClientUtil.ToDecimal(fGridDetail.Cell(i, 18).Text);
                        cciDtl.ExpectOutputValueAccount =GetRate( ClientUtil.ToString(fGridDetail.Cell(i, 19).Text));
                        cciDtl.ConcreteDrawingBudget = ClientUtil.ToString(fGridDetail.Cell(i, 20).Text);
                        cciDtl.ConcreteConsumption = ClientUtil.ToString(fGridDetail.Cell(i, 21).Text);
                        cciDtl.ConcreteSaveRate = ClientUtil.ToString(fGridDetail.Cell(i, 22).Text);
                        cciDtl.RebarDetailingAmount = ClientUtil.ToString(fGridDetail.Cell(i, 23).Text);
                        cciDtl.RebarConsumption = ClientUtil.ToString(fGridDetail.Cell(i, 24).Text);
                        cciDtl.RebarSaveRate = ClientUtil.ToString(fGridDetail.Cell(i, 25).Text);
                        cciDtl.WasteRebarAmount = ClientUtil.ToString(fGridDetail.Cell(i, 26).Text);
                        cciDtl.ScrapRate = ClientUtil.ToString(fGridDetail.Cell(i, 27).Text);
                        cciDtl.RightReportPoint = ClientUtil.ToString(fGridDetail.Cell(i, 28).Text);
                        cciDtl.ProjectSubmitTime = ClientUtil.ToDateTime(fGridDetail.Cell(i, 29).Text);
                        cciDtl.OwnerConfirmTime = ClientUtil.ToDateTime(fGridDetail.Cell(i,30).Text);
                        cciDtl.OwnerRightOutput = ClientUtil.ToDecimal(fGridDetail.Cell(i, 31).Text);
                        cciDtl.ContractorRightOutput = ClientUtil.ToDecimal(fGridDetail.Cell(i, 32).Text);
                        cciDtl.ProjectSelfPayment = ClientUtil.ToDecimal(fGridDetail.Cell(i, 33).Text);
                        cciDtl.ProjcetContractorPayment = ClientUtil.ToDecimal(fGridDetail.Cell(i, 34).Text);
                        cciDtl.SelfOutputRightRate =GetRate( ClientUtil.ToString(fGridDetail.Cell(i, 35).Text));
                        cciDtl.ContractorOutputRightRate =GetRate( ClientUtil.ToString(fGridDetail.Cell(i, 36).Text));
                        cciDtl.OutPutRightRate = ClientUtil.ToString(fGridDetail.Cell(i, 37).Text);
                        cciDtl.ReceivableAccount = ClientUtil.ToDecimal(fGridDetail.Cell(i, 38).Text);
                       
                        cciDtl.ActualAccount = ClientUtil.ToDecimal(fGridDetail.Cell(i, 39).Text);
                        cciDtl.OverallBusinessPlan = ClientUtil.ToString(fGridDetail.Cell(i, 40).Text);
                        cciDtl.ResponsibilitySigh = ClientUtil.ToString(fGridDetail.Cell(i, 41).Text);
                        cciDtl.ReceivableRiskMortgage = ClientUtil.ToDecimal(fGridDetail.Cell(i, 42).Text);
                        cciDtl.ActualRiskMortgage = ClientUtil.ToDecimal(fGridDetail.Cell(i, 43).Text);
                        cciDtl.RiskMortgageRate = ClientUtil.ToString(fGridDetail.Cell(i, 44).Text);
                        cciDtl.OccurredHourlyAccount = ClientUtil.ToDecimal(fGridDetail.Cell(i, 45).Text);
                        cciDtl.ProportionOfHourlyWork =GetRate( ClientUtil.ToString(fGridDetail.Cell(i, 46).Text));
                        cciDtl.OccurredOEMAccount = ClientUtil.ToDecimal(fGridDetail.Cell(i, 47).Text);
                        cciDtl.DeductedAccount = ClientUtil.ToDecimal(fGridDetail.Cell(i, 48).Text);
                        cciDtl.OuterContractAccount = ClientUtil.ToDecimal(fGridDetail.Cell(i, 49).Text);
                        cciDtl.SelfSignedAccount = ClientUtil.ToDecimal(fGridDetail.Cell(i, 50).Text);
                        cciDtl.ProportionOfOuterContractAccount =GetRate( ClientUtil.ToString(fGridDetail.Cell(i, 51).Text));
                        crMaster.CciDtl.Add(cciDtl);
                    }
                }
                #endregion


                #region
                if (type == reportJCBBB)
                {
                    int year = ClientUtil.ToInt(this.comYear_bc.Text);
                    int month = ClientUtil.ToInt(this.comMonth_bc.Text);
                    CommercialReportMaster cr = model.CostMonthAccSrv.GetCommercialMaster(crMaster.ProjectId, year, month, type);
                    if (!ClientUtil.isEmpty(cr))
                    {
                        crMaster = cr;
                        crMaster.BcDtl.Clear();
                    }
                    else
                    {
                        crMaster = CreateMaster();
                        crMaster.CreateYear = year;
                        crMaster.CreateMonth = month;
                        crMaster.CreateDate = ConstObject.LoginDate;
                    }
                    for (int i = 8; i <= endRow; i++)
                    {
                        BureauCostDtl bcDtl = new BureauCostDtl();
                        bcDtl.Master = crMaster;
                        bcDtl.ProjectType = ClientUtil.ToString(fGridDetail_bc.Cell(i, 3).Text);
                        bcDtl.ProjectScale = ClientUtil.ToString(fGridDetail_bc.Cell(i, 4).Text);
                        bcDtl.Location = ClientUtil.ToString(fGridDetail_bc.Cell(i, 5).Text);
                        bcDtl.OwnerName = ClientUtil.ToString(fGridDetail_bc.Cell(i, 6).Text);
                        bcDtl.GeneralContractName = ClientUtil.ToString(fGridDetail_bc.Cell(i, 7).Text);
                        bcDtl.OwnerProperty = ClientUtil.ToString(fGridDetail_bc.Cell(i, 8).Text);
                        bcDtl.IsStractegicCustomer = ClientUtil.ToString(fGridDetail_bc.Cell(i, 9).Text);
                        bcDtl.ProjectSite = ClientUtil.ToString(fGridDetail_bc.Cell(i, 10).Text);
                        bcDtl.ConstructionAcreage = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 11).Text);
                        bcDtl.ConstructionHeight = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 12).Text);
                        bcDtl.TotalContractAmount = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 13).Text);
                        bcDtl.SelfContractAmount = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 14).Text);
                        bcDtl.GeneralManageCost = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 15).Text);
                        bcDtl.ManageCostCalculateRate = ClientUtil.ToString(fGridDetail_bc.Cell(i, 16).Text);
                        bcDtl.SettlementType = ClientUtil.ToString(fGridDetail_bc.Cell(i, 17).Text);
                        bcDtl.SettlementTime = ClientUtil.ToDateTime(fGridDetail_bc.Cell(i, 18).Text);
                        bcDtl.InvokeSituation = ClientUtil.ToString(fGridDetail_bc.Cell(i, 19).Text);
                        bcDtl.ImprestRate = ClientUtil.ToString(fGridDetail_bc.Cell(i, 20).Text);
                        bcDtl.PaymentMode = ClientUtil.ToString(fGridDetail_bc.Cell(i, 21).Text);
                        bcDtl.PaymentRate = ClientUtil.ToString(fGridDetail_bc.Cell(i, 22).Text);
                        bcDtl.PaymentForm = ClientUtil.ToString(fGridDetail_bc.Cell(i, 23).Text);
                        bcDtl.MaintenancePayTime = ClientUtil.ToDateTime(fGridDetail_bc.Cell(i, 24).Text);
                        bcDtl.ManagementMode = ClientUtil.ToString(fGridDetail_bc.Cell(i, 25).Text);
                        bcDtl.TargetScopeMoney = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 26).Text);
                        bcDtl.DutyAgreementTime = ClientUtil.ToDateTime(fGridDetail_bc.Cell(i, 27).Text);
                        bcDtl.CheckNodeSetting = ClientUtil.ToString(fGridDetail_bc.Cell(i, 28).Text);
                        bcDtl.ForeBidEarningRate = ClientUtil.ToString(fGridDetail_bc.Cell(i, 29).Text);
                        bcDtl.CalculateCost = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 30).Text);
                        bcDtl.CalculateBraekevenRate = ClientUtil.ToString(fGridDetail_bc.Cell(i, 31).Text);
                        bcDtl.TargetCost = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 32).Text);
                        bcDtl.HandInRate = ClientUtil.ToString(fGridDetail_bc.Cell(i, 33).Text); //ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 33).Text);
                        bcDtl.TargetRemark = ClientUtil.ToString(fGridDetail_bc.Cell(i, 34).Text);
                        bcDtl.ShouldPaydeposit = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 35).Text);
                        bcDtl.ActualPayDeposit = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 36).Text);
                        bcDtl.QuarterFulfillTimes = ClientUtil.ToString(fGridDetail_bc.Cell(i, 37).Text);
                        bcDtl.QuarterShouldPayMoney = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 38).Text);
                        bcDtl.QuarterActualPayMoney = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 39).Text);
                        bcDtl.Quarter2015Money = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 40).Text);
                        bcDtl.NodeFulfillTimes = ClientUtil.ToString(fGridDetail_bc.Cell(i, 41).Text);
                        bcDtl.NodeShouldPayMoney = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 42).Text);
                        bcDtl.NodeActualPayMoney = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 43).Text);
                        bcDtl.Node2015Money = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 44).Text);
                        bcDtl.GeneralCompleteOutput = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 45).Text);
                        bcDtl.SelefCompleteOutput = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 46).Text);
                        bcDtl.ExpectConfirmAmount = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 47).Text);
                        bcDtl.ExpectConfirmDate = ClientUtil.ToDateTime(fGridDetail_bc.Cell(i, 48).Text);
                        bcDtl.ConfirmedAmount = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 49).Text);
                        bcDtl.ConfirmedDate = ClientUtil.ToDateTime(fGridDetail_bc.Cell(i, 50).Text);
                        bcDtl.EstimateAmount = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 51).Text);
                        bcDtl.ProjectActualCost = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 52).Text);
                        bcDtl.ProjectProfit = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 53).Text);
                        bcDtl.GatheringAmountAtRate = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 54).Text);
                        bcDtl.SelfAcceptAmount = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 55).Text);
                        bcDtl.SelfImprest = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 56).Text);
                        bcDtl.SelfProgressAmount = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 57).Text);
                        bcDtl.SelfOthers = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 58).Text);
                        bcDtl.PaymentAtRate = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 59).Text);
                        bcDtl.PayedAmount = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 60).Text);
                        bcDtl.ReportedQuantity = ClientUtil.ToString(fGridDetail_bc.Cell(i, 61).Text);
                        bcDtl.ReportedAmount = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 62).Text);
                        bcDtl.CorrespondingCost = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 63).Text);
                        bcDtl.OwnerConfirmQuantity = ClientUtil.ToString(fGridDetail_bc.Cell(i, 64).Text);
                        bcDtl.OwnerConfirmAmount = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 65).Text);
                        bcDtl.PlanReduceAmount = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 66).Text);
                        bcDtl.PlanIncreaseAmount = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 67).Text);
                        bcDtl.ActualReduceAmount = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 68).Text);
                        bcDtl.ActualIncreaseAmount = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 69).Text);
                        bcDtl.AccumulatePaidBonus = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 70).Text);
                        bcDtl.PaidBonus2015 = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 71).Text);
                        bcDtl.RebarDetailingAmount = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 72).Text);
                        bcDtl.ContractStartDate = ClientUtil.ToDateTime(fGridDetail_bc.Cell(i, 73).Text);
                        bcDtl.ContractEndDate = ClientUtil.ToDateTime(fGridDetail_bc.Cell(i, 74).Text);
                        bcDtl.ActualStartDate = ClientUtil.ToDateTime(fGridDetail_bc.Cell(i, 75).Text);
                        bcDtl.ExpectEendDate = ClientUtil.ToDateTime(fGridDetail_bc.Cell(i, 76).Text);
                        bcDtl.IsPerforming = ClientUtil.ToString(fGridDetail_bc.Cell(i, 77).Text);
                        bcDtl.DelayDays = ClientUtil.ToString(fGridDetail_bc.Cell(i, 78).Text);
                        bcDtl.CauseIncreaseCost = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 79).Text);
                        bcDtl.DurationCaption = ClientUtil.ToString(fGridDetail_bc.Cell(i, 80).Text);
                        bcDtl.OwnerSignDays = ClientUtil.ToString(fGridDetail_bc.Cell(i, 81).Text);
                        bcDtl.ConfirmExpense = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 82).Text);
                        bcDtl.ConfrontForfeit = ClientUtil.ToDecimal(fGridDetail_bc.Cell(i, 83).Text);
                        bcDtl.ProgramType = ClientUtil.ToString(fGridDetail_bc.Cell(i, 84).Text);
                        bcDtl.ProgramState = ClientUtil.ToString(fGridDetail_bc.Cell(i, 85).Text);
                        crMaster.BcDtl.Add(bcDtl);
                    }
                }
                #endregion

                #region  专业分包指标
                if (type == reportFBZB)
                {
                    iOrderNo = 0;
                    string sValue=string.Empty;
                    int year = ClientUtil.ToInt(this.comYear_bi.Text);
                    int month = ClientUtil.ToInt(this.comMonth_bi.Text);
                    CommercialReportMaster cr = model.CostMonthAccSrv.GetCommercialMaster(crMaster.ProjectId, year, month, type);
                    if (!ClientUtil.isEmpty(cr))
                    {
                        crMaster = cr;
                        //crMaster.BiDtl.Clear();
                    }
                    else
                    {
                        crMaster = CreateMaster();
                        crMaster.CreateYear = year;
                        crMaster.CreateMonth = month;
                        crMaster.CreateDate = ConstObject.LoginDate;
                    }
                    for (int i = 7; i <= endRow; i++)
                    {
                        if (fGridDetail_bi.Cell(i, 1).Text == "合计：")//合计列不需要添加进去  20160830 add
                        {

                        }
                        else
                        {
                            sValue=fGridDetail_bi.Cell(i, 1).Tag;
                            BearerIndicatorDtl biDtl = string.IsNullOrEmpty(sValue) ? new BearerIndicatorDtl() : crMaster.BiDtl.OfType<BearerIndicatorDtl>().Where(a=>string.Equals(a.Id,sValue)).First();
                            if (!fGridDetail_bi.Row(i).Visible )
                            {
                                if (!string.IsNullOrEmpty(biDtl.Id))
                                {
                                    crMaster.BiDtl.Remove(biDtl);
                                }
                                continue;
                            }
                            biDtl.OrderNo = iOrderNo++;
                            biDtl.Master = crMaster;
                            biDtl.ConstructionTeam = ClientUtil.ToString(fGridDetail_bi.Cell(i, 3).Text);
                            biDtl.ConstructionContent = ClientUtil.ToString(fGridDetail_bi.Cell(i, 4).Text);
                            biDtl.CurrenContractMoney = ClientUtil.ToDecimal(fGridDetail_bi.Cell(i, 5).Text);
                            biDtl.CurrentSettleMoney = ClientUtil.ToDecimal(fGridDetail_bi.Cell(i, 6).Text);
                            biDtl.CurrentOuterSettleMoney = ClientUtil.ToDecimal(fGridDetail_bi.Cell(i, 7).Text);
                            biDtl.IsConrresponding = ClientUtil.ToString(fGridDetail_bi.Cell(i, 8).Text);
                            biDtl.IncomeMoney = ClientUtil.ToDecimal(fGridDetail_bi.Cell(i, 9).Text);
                            biDtl.CurrentSelfSignMoney = ClientUtil.ToDecimal(fGridDetail_bi.Cell(i, 10).Text);
                            biDtl.Descript = ClientUtil.ToString(fGridDetail_bi.Cell(i, 11).Text);
                            biDtl.AccrueSettleMoney = ClientUtil.ToDecimal(fGridDetail_bi.Cell(i, 12).Text);
                            biDtl.AccrueOuterSettleMoney = ClientUtil.ToDecimal(fGridDetail_bi.Cell(i, 13).Text);
                            biDtl.AccrueSelfSignMoney = ClientUtil.ToDecimal(fGridDetail_bi.Cell(i, 14).Text);
                            biDtl.CurrentOemMoney = ClientUtil.ToDecimal(fGridDetail_bi.Cell(i, 15).Text);
                            biDtl.CurrentBeOemMoney = ClientUtil.ToDecimal(fGridDetail_bi.Cell(i, 16).Text);
                            biDtl.AccrueOemMoney = ClientUtil.ToDecimal(fGridDetail_bi.Cell(i, 17).Text);
                            biDtl.AccrueBeOemMoney = ClientUtil.ToDecimal(fGridDetail_bi.Cell(i, 18).Text);
                            biDtl.CurrentHourlyMoney = ClientUtil.ToDecimal(fGridDetail_bi.Cell(i, 19).Text);
                           // biDtl.CurrentHourlyRate = ClientUtil.ToString(fGridDetail_bi.Cell(i, 20).Text);
                            biDtl.AccrueHourlyMoney = ClientUtil.ToDecimal(fGridDetail_bi.Cell(i, 21).Text);
                           // biDtl.AccrueHourlyRate = ClientUtil.ToString(fGridDetail_bi.Cell(i, 22).Text);

                            biDtl.FLAG = ClientUtil.ToString("0");//专业分包为0
                            crMaster.BiDtl.Add(biDtl);
                        }
                    }
                }
#endregion
                #region 劳务分包指标  20160824
                if (type == reportLWFBZB)
                {
                    iOrderNo = 0;
                    string sValue = string.Empty;

                    int year = ClientUtil.ToInt(this.comYear_blw.Text);
                    int month = ClientUtil.ToInt(this.comMonth_blw.Text);
                    CommercialReportMaster cr = model.CostMonthAccSrv.GetCommercialMaster(crMaster.ProjectId, year, month, type);
                    
                    if (!ClientUtil.isEmpty(cr))
                    {
                        crMaster = cr;
                       
                    }
                    else
                    {
                        crMaster.CreateYear = year;
                        crMaster.CreateMonth = month;
                        crMaster.CreateDate = ConstObject.LoginDate;
                    }
                    for (int i = 7; i <= endRow; i++)
                    {

                        if (fGridDetail_blw.Cell(i, 1).Text == "合计：")//合计列不需要添加进去  20160830 add
                        {

                        }
                        else
                        {
                            sValue = fGridDetail_blw.Cell(i, 1).Tag;
                            BearerIndicatorDtl biDtl = string.IsNullOrEmpty(sValue) ? new BearerIndicatorDtl() : crMaster.BiDtl.OfType<BearerIndicatorDtl>().Where(a => string.Equals(a.Id, sValue)).First();
                            if (!fGridDetail_blw.Row(i).Visible  )
                            {
                                if (!string.IsNullOrEmpty(biDtl.Id))
                                {
                                    crMaster.BiDtl.Remove(biDtl);
                                }
                                continue;
                            }
                            //BearerIndicatorDtl biDtl = new BearerIndicatorDtl();
                            biDtl.OrderNo = iOrderNo++;
                            biDtl.Master = crMaster;
                            biDtl.ConstructionTeam = ClientUtil.ToString(fGridDetail_blw.Cell(i, 3).Text);
                            biDtl.ConstructionContent = ClientUtil.ToString(fGridDetail_blw.Cell(i, 4).Text);
                            biDtl.CurrenContractMoney = ClientUtil.ToDecimal(fGridDetail_blw.Cell(i, 5).Text);
                            biDtl.CurrentSettleMoney = ClientUtil.ToDecimal(fGridDetail_blw.Cell(i, 6).Text);
                            biDtl.CurrentOuterSettleMoney = ClientUtil.ToDecimal(fGridDetail_blw.Cell(i, 7).Text);
                            biDtl.IsConrresponding = ClientUtil.ToString(fGridDetail_blw.Cell(i, 8).Text);
                            biDtl.IncomeMoney = ClientUtil.ToDecimal(fGridDetail_blw.Cell(i, 9).Text);
                            biDtl.CurrentSelfSignMoney = ClientUtil.ToDecimal(fGridDetail_blw.Cell(i, 10).Text);
                            biDtl.Descript = ClientUtil.ToString(fGridDetail_blw.Cell(i, 11).Text);
                            biDtl.AccrueSettleMoney = ClientUtil.ToDecimal(fGridDetail_blw.Cell(i, 12).Text);
                            biDtl.AccrueOuterSettleMoney = ClientUtil.ToDecimal(fGridDetail_blw.Cell(i, 13).Text);
                            biDtl.AccrueSelfSignMoney = ClientUtil.ToDecimal(fGridDetail_blw.Cell(i, 14).Text);
                            biDtl.CurrentOemMoney = ClientUtil.ToDecimal(fGridDetail_blw.Cell(i, 15).Text);
                            biDtl.CurrentBeOemMoney = ClientUtil.ToDecimal(fGridDetail_blw.Cell(i, 16).Text);
                            biDtl.AccrueOemMoney = ClientUtil.ToDecimal(fGridDetail_blw.Cell(i, 17).Text);
                            biDtl.AccrueBeOemMoney = ClientUtil.ToDecimal(fGridDetail_blw.Cell(i, 18).Text);
                            biDtl.CurrentHourlyMoney = ClientUtil.ToDecimal(fGridDetail_blw.Cell(i, 19).Text);
                            //biDtl.CurrentHourlyRate = ClientUtil.ToString(fGridDetail_blw.Cell(i, 20).Text);
                            biDtl.AccrueHourlyMoney = ClientUtil.ToDecimal(fGridDetail_blw.Cell(i, 21).Text);
                            //biDtl.AccrueHourlyRate = ClientUtil.ToString(fGridDetail_blw.Cell(i, 22).Text);

                            biDtl.FLAG = ClientUtil.ToString("1");//劳务分包为1
                            crMaster.BiDtl.Add(biDtl);
                          
                        }
                    }
                }
                #endregion

                #region 签证索赔  20160826
                if (type == reportQZSPB )
                {
                    int year = ClientUtil.ToInt(this.comYear_vc.Text);
                    int month = ClientUtil.ToInt(this.comMonth_vc.Text);
                    CommercialReportMaster cr = model.CostMonthAccSrv.GetCommercialMaster(crMaster.ProjectId, year, month, type);
                    if (!ClientUtil.isEmpty(cr))
                    {
                        crMaster = cr;
                        crMaster.VcDtl.Clear();
                    }
                    else
                    {
                        crMaster = CreateMaster();
                        crMaster.CreateYear = year;
                        crMaster.CreateMonth = month;
                        crMaster.CreateDate = ConstObject.LoginDate;
                    }
                    for (int i = 5; i <= endRow; i++)//
                    {


                        //fGridDetail_vc.Cell(index, 1).Text = (iCount + 1).ToString();
                        //fGridDetail_vc.Cell(index, 2).WrapText = true;
                        //fGridDetail_vc.Cell(index, 2).Text = ClientUtil.ToString(crMaster.ProjectName);
                        //fGridDetail_vc.Cell(index, 3).WrapText = true;
                        //fGridDetail_vc.Cell(index, 3).Text = ClientUtil.ToString(bi.YBSQZFS);
                        //fGridDetail_vc.Cell(index, 4).WrapText = true;
                        //fGridDetail_vc.Cell(index, 5).Text = bi.QZLJBSJE.ToString("N2");
                        //fGridDetail_vc.Cell(index, 6).Text = bi.LJQRSR.ToString("N2");
                        //fGridDetail_vc.Cell(index, 7).Text = bi.LSSJSR.ToString("N2");
                        //fGridDetail_vc.Cell(index, 8).Text = bi.LJQRQZBFSYL.ToString("N2");
                        //fGridDetail_vc.Cell(index, 9).Text = bi.LJBSJE.ToString("N2");
                        //fGridDetail_vc.Cell(index, 10).Text = bi.LJSJCB.ToString("N2");
                        //fGridDetail_vc.Cell(index, 11).Text = bi.QZLJSJCB.ToString("N2");
                        //fGridDetail_vc.Cell(index, 12).Text = bi.QZXYL.ToString("N2");
                        //fGridDetail_vc.Cell(index, 13).Text = bi.JZLJZXSGCZ.ToString("N2");
                        //fGridDetail_vc.Cell(index, 14).Text = bi.QZZEDCZGXL.ToString("N2");
                        //fGridDetail_vc.Cell(index, 15).Text = bi.QZXYCZGXL.ToString("N2");

                        if (fGridDetail_vc.Cell(i, 1).Text == "合计：")//合计列不需要添加进去  20160830 add
                        {

                        }
                        else
                        {
                            VisaClamisDtl biDtl = new VisaClamisDtl();
                            biDtl.Master = crMaster;
                            biDtl.YBSQZFS = ClientUtil.ToInt(fGridDetail_vc.Cell(i, 3).Text);

                            biDtl.YQRQZFS = ClientUtil.ToInt(fGridDetail_vc.Cell(i, 4).Text);


                            biDtl.QZLJBSJE = ClientUtil.ToDecimal(fGridDetail_vc.Cell(i, 5).Text);
                            biDtl.LJQRSR = ClientUtil.ToDecimal(fGridDetail_vc.Cell(i, 6).Text);


                            biDtl.LSSJSR = ClientUtil.ToDecimal(fGridDetail_vc.Cell(i, 7).Text);
                            biDtl.LJQRQZBFSYL = ClientUtil.ToString(fGridDetail_vc.Cell(i, 8).Text);

                            biDtl.LJBSJE = ClientUtil.ToDecimal(fGridDetail_vc.Cell(i, 9).Text);
                            biDtl.LJSJCB = ClientUtil.ToDecimal(fGridDetail_vc.Cell(i, 10).Text);

                            biDtl.QZLJSJCB = ClientUtil.ToDecimal(fGridDetail_vc.Cell(i, 11).Text);
                            biDtl.QZXYL = ClientUtil.ToString(fGridDetail_vc.Cell(i, 12).Text);
                            biDtl.JZLJZXSGCZ = ClientUtil.ToDecimal(fGridDetail_vc.Cell(i, 13).Text);
                            biDtl.QZZEDCZGXL = ClientUtil.ToString(fGridDetail_vc.Cell(i, 14).Text);
                            biDtl.QZXYCZGXL = ClientUtil.ToString(fGridDetail_vc.Cell(i, 15).Text);
                            biDtl.ProjectDeclareCount = ClientUtil.ToInt(fGridDetail_vc.Cell(i, 16).Text);
                            biDtl.ProjectRiskSolution = ClientUtil.ToDecimal(fGridDetail_vc.Cell(i, 17).Text);
                            biDtl.ProjectAddBenefit = ClientUtil.ToDecimal(fGridDetail_vc.Cell(i, 18).Text);
                            biDtl.RewardDeclareCount = ClientUtil.ToInt(fGridDetail_vc.Cell(i, 19).Text);
                            biDtl.RewardRiskSolution = ClientUtil.ToDecimal(fGridDetail_vc.Cell(i, 20).Text);
                            biDtl.RewardAddBenefit = ClientUtil.ToDecimal(fGridDetail_vc.Cell(i, 21).Text);
                            biDtl.OutPutMoney = ClientUtil.ToDecimal(fGridDetail_vc.Cell(i, 22).Text);
                            biDtl.SelfMoneyRate = ClientUtil.ToString(fGridDetail_vc.Cell(i, 23).Text);
                            crMaster.VcDtl.Add(biDtl);
                        }
                    }
                }
                #endregion

                #region  分包争议
                if (type == reportFBZY)
                {
                    iOrderNo = 0;
                    int year = ClientUtil.ToInt(this.comYear_dt.Text);
                    int month = ClientUtil.ToInt(this.comMonth_dt.Text);
                    CommercialReportMaster cr = model.CostMonthAccSrv.GetCommercialMaster(crMaster.ProjectId, year, month, type);
                    if (!ClientUtil.isEmpty(cr))
                    {
                        crMaster = cr;
                        crMaster.DtDtl.Clear();
                    }
                    else
                    {
                        crMaster = CreateMaster();
                        crMaster.CreateYear = year;
                        crMaster.CreateMonth = month;
                        crMaster.CreateDate = ConstObject.LoginDate;
                    }

                   

                    for (int i = 5; i <= endRow; i++)
                    {

                        if (fGridDetail_dt.Cell(i, 1).Text == "合计：")//合计列不需要添加进去  20160830 add
                        {

                        }
                        else
                        {

                            DisputeTrackDtl dtDtl = new DisputeTrackDtl();
                            dtDtl.OrderNo = iOrderNo++;
                            dtDtl.Master = crMaster;
                            dtDtl.BearerUnitName = ClientUtil.ToString(fGridDetail_dt.Cell(i, 3).Text);
                            dtDtl.DisputeContent = ClientUtil.ToString(fGridDetail_dt.Cell(i, 4).Text);
                            dtDtl.BearerSuggestion = ClientUtil.ToString(fGridDetail_dt.Cell(i, 5).Text);
                            dtDtl.ProjectSuggestion = ClientUtil.ToString(fGridDetail_dt.Cell(i, 6).Text);
                            dtDtl.InvolveMoney = ClientUtil.ToDecimal(fGridDetail_dt.Cell(i, 7).Text);
                            dtDtl.CurrentProgress = ClientUtil.ToString(fGridDetail_dt.Cell(i, 8).Text);
                            dtDtl.Descript = ClientUtil.ToString(fGridDetail_dt.Cell(i, 9).Text);
                            crMaster.DtDtl.Add(dtDtl);
                        }
                    }
                }
                #endregion

                #region
                if (type == reportFBZJBS)
                {
                    iOrderNo = 0;
                    int year = ClientUtil.ToInt(this.comYear_sa.Text);
                    int month = ClientUtil.ToInt(this.comMonth_sa.Text);
                    CommercialReportMaster cr = model.CostMonthAccSrv.GetCommercialMaster(crMaster.ProjectId, year, month, type);
                    if (!ClientUtil.isEmpty(cr))
                    {
                        crMaster = cr;
                        crMaster.SaDtl.Clear();
                    }
                    else
                    {
                        crMaster = CreateMaster();
                        crMaster.CreateYear = year;
                        crMaster.CreateMonth = month;
                        crMaster.CreateDate = ConstObject.LoginDate;
                    }
                    for (int i = 5; i <= endRow; i++)
                    {
                        SubcontractAuditDtl saDtl = new SubcontractAuditDtl();
                        saDtl.OrderNo = iOrderNo++;
                        saDtl.Master = crMaster;
                        saDtl.SubentryProjectName = ClientUtil.ToString(fGridDetail_sa.Cell(i, 3).Text);
                        saDtl.BearerOrgName = ClientUtil.ToString(fGridDetail_sa.Cell(i, 4).Text);
                        saDtl.SubcontractAmount = ClientUtil.ToDecimal(fGridDetail_sa.Cell(i, 5).Text);
                        saDtl.AccumulateAmount = ClientUtil.ToDecimal(fGridDetail_sa.Cell(i, 6).Text);
                        saDtl.Makespan = ClientUtil.ToDateTime(fGridDetail_sa.Cell(i, 7).Text);
                        saDtl.ExpectSubcontractAmount = ClientUtil.ToDecimal(fGridDetail_sa.Cell(i, 8).Text);
                        saDtl.IsAudit = ClientUtil.ToString(fGridDetail_sa.Cell(i, 9).Text);
                        saDtl.ExpectAuditTime = ClientUtil.ToDateTime(fGridDetail_sa.Cell(i, 10).Text);
                        saDtl.Descript = ClientUtil.ToString(fGridDetail_sa.Cell(i, 11).Text);
                        crMaster.SaDtl.Add(saDtl);
                    }
                }
                #endregion


                #region
                if (type == reportGSJSJZ)
                {
                    int year = ClientUtil.ToInt(this.comYear_spr.Text);
                    int month = ClientUtil.ToInt(this.comMonth_spr.Text);
                    CommercialReportMaster cr = model.CostMonthAccSrv.GetCommercialMaster(crMaster.ProjectId, year, month, type);
                    if (!ClientUtil.isEmpty(cr))
                    {
                        crMaster = cr;
                        crMaster.SprDtl.Clear();
                    }
                    else
                    {
                        crMaster = CreateMaster();
                        crMaster.CreateYear = year;
                        crMaster.CreateMonth = month;
                        crMaster.CreateDate = ConstObject.LoginDate;
                    }
                    for (int i = 6; i <= endRow; i++)
                    {
                        SettlementProgressReportDtl sprDtl = new SettlementProgressReportDtl();
                        sprDtl.Master = crMaster;
                        sprDtl.ThisMonth = ClientUtil.ToString(fGridDetail_spr.Cell(i, 3).Text);
                        sprDtl.SettlementProgress = ClientUtil.ToString(fGridDetail_spr.Cell(i, 4).Text);
                        sprDtl.ImportantFactor = ClientUtil.ToString(fGridDetail_spr.Cell(i, 5).Text);
                        sprDtl.NextMonthPlan = ClientUtil.ToString(fGridDetail_spr.Cell(i, 6).Text);
                        sprDtl.Descript = ClientUtil.ToString(fGridDetail_spr.Cell(i, 7).Text);
                        crMaster.SprDtl.Add(sprDtl);
                    }
                }
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void ModelToView(CommercialReportMaster crMaster, string type)
        {
            try
            {
                fGridDetail.AutoRedraw = false;
                ISet<CostCheckIndicatorDtl> cciDtls = crMaster.CciDtl;
                ISet<BureauCostDtl> bcDtls = crMaster.BcDtl;
                ISet<BearerIndicatorDtl> biDtls = crMaster.BiDtl;



                ISet<DisputeTrackDtl> dtDtls = crMaster.DtDtl;
                ISet<SubcontractAuditDtl> saDtls = crMaster.SaDtl;
                ISet<SettlementProgressReportDtl> sprDtls = crMaster.SprDtl;

                ISet<VisaClamisDtl> vcDtls = crMaster.VcDtl;//签证索赔

                #region
                if (type == reportCBJCZB)
                {
                    iCount = cciDtls.Count;
                    iStart = 7;
                    fGridDetail.InsertRow(iStart, iCount);
                    FlexCell.Range range = fGridDetail.Range(iStart, 1, iCount + iStart, fGridDetail.Cols - 1);

                    iCount = 0;
                    int index = 0;
                    fGridDetail.Column(28).CellType = fGridDetail.Column(29).CellType = FlexCell.CellTypeEnum.Calendar;
                    foreach (CostCheckIndicatorDtl cci in cciDtls)
                    {
                        index = iStart + iCount;
                        fGridDetail.Cell(index, 1).Text = (iCount + 1).ToString();
                        fGridDetail.Cell(index, 2).WrapText = true;fGridDetail.Cell(index, 2).Text = ClientUtil.ToString(crMaster.ProjectName);
                        fGridDetail.Cell(index, 3).Text = ShowRate(ClientUtil.ToString(cci.TenderCalculatePoint)); fGridDetail.Cell(index, 3).WrapText = true;
                        fGridDetail.Cell(index, 4).WrapText = true; fGridDetail.Cell(index, 4).Text = ShowRate(ClientUtil.ToString(cci.LiabilityPaid));
                        fGridDetail.Cell(index, 5).Text = cci.SelfPlanMoney.ToString("N2");
                        fGridDetail.Cell(index, 6).WrapText = true; fGridDetail.Cell(index, 6).Text = ClientUtil.ToString(cci.QuarterBenefitRate);
                        fGridDetail.Cell(index, 7).WrapText = true; fGridDetail.Cell(index,7).Text = ClientUtil.ToString(cci.ConstructionContractIncome);
                        fGridDetail.Cell(index, 8).Text = cci.Income.ToString("N2");
                        fGridDetail.Cell(index, 9).Text = cci.Cost.ToString("N2");
                        fGridDetail.Cell(index, 10).Text = cci.BenefitAmount.ToString("N2");
                        fGridDetail.Cell(index, 11).WrapText = true; fGridDetail.Cell(index, 11).Text = ShowRate(ClientUtil.ToString(cci.BenefitRate));
                        fGridDetail.Cell(index, 12).Text = cci.DutyCost.ToString("N2");
                        fGridDetail.Cell(index, 13).WrapText = true; fGridDetail.Cell(index, 13).Text = ShowRate(ClientUtil.ToString(cci.OverCostReduceRate));
                        fGridDetail.Cell(index, 14).Text = cci.SiteFunds.ToString("N2");
                        fGridDetail.Cell(index, 15).WrapText = true; fGridDetail.Cell(index, 15).Text = ShowRate(ClientUtil.ToString(cci.TotalOutputValueAccount));
                        fGridDetail.Cell(index, 16).Text = cci.OccurredMoney.ToString("N2");
                        fGridDetail.Cell(index, 17).Text = cci.ExpectOccurredMoney.ToString("N2");
                        fGridDetail.Cell(index, 18).Text = cci.TotalMoney.ToString("N2");
                        fGridDetail.Cell(index, 19).WrapText = true; fGridDetail.Cell(index, 19).Text = ShowRate(ClientUtil.ToString(cci.ExpectOutputValueAccount));
                        fGridDetail.Cell(index, 20).WrapText = true; fGridDetail.Cell(index, 20).Text = ClientUtil.ToString(cci.ConcreteDrawingBudget);
                        fGridDetail.Cell(index, 21).WrapText = true; fGridDetail.Cell(index, 21).Text = ClientUtil.ToString(cci.ConcreteConsumption);
                        fGridDetail.Cell(index, 22).WrapText = true; fGridDetail.Cell(index, 22).Text = ClientUtil.ToString(cci.ConcreteSaveRate);
                        fGridDetail.Cell(index, 23).WrapText = true; fGridDetail.Cell(index, 23).Text = ClientUtil.ToString(cci.RebarDetailingAmount);
                        fGridDetail.Cell(index, 24).WrapText = true; fGridDetail.Cell(index, 24).Text = ClientUtil.ToString(cci.RebarConsumption);
                        fGridDetail.Cell(index, 25).WrapText = true; fGridDetail.Cell(index, 25).Text = ClientUtil.ToString(cci.RebarSaveRate);
                        fGridDetail.Cell(index, 26).WrapText = true; fGridDetail.Cell(index, 26).Text = ClientUtil.ToString(cci.WasteRebarAmount);
                        fGridDetail.Cell(index, 27).WrapText = true; fGridDetail.Cell(index, 27).Text = ClientUtil.ToString(cci.ScrapRate);
                        fGridDetail.Cell(index, 28).WrapText = true; fGridDetail.Cell(index, 28).Text = ClientUtil.ToString(cci.RightReportPoint); fGridDetail.Cell(index, 28).WrapText = true;
                       // fGridDetail.Cell(index, 29).CellType = fGridDetail.Cell(index, 28).CellType = FlexCell.CellTypeEnum.Calendar;
                        fGridDetail.Cell(index, 29).Text = cci.ProjectSubmitTime.ToShortDateString();
                      
                        fGridDetail.Cell(index, 30).Text = cci.OwnerConfirmTime.ToShortDateString();
                        fGridDetail.Cell(index, 31).Text = cci.OwnerRightOutput.ToString("N2");
                        fGridDetail.Cell(index, 32).Text = cci.ContractorRightOutput.ToString("N2");
                        fGridDetail.Cell(index, 33).Text = cci.ProjectSelfPayment.ToString("N2");
                        fGridDetail.Cell(index, 34).Text = cci.ProjcetContractorPayment.ToString("N2");
                        fGridDetail.Cell(index, 35).WrapText = true; fGridDetail.Cell(index, 35).Text = ShowRate(ClientUtil.ToString(cci.SelfOutputRightRate));
                        fGridDetail.Cell(index, 36).WrapText = true; fGridDetail.Cell(index, 36).Text = ShowRate(ClientUtil.ToString(cci.ContractorOutputRightRate));
                        fGridDetail.Cell(index, 37).WrapText = true; fGridDetail.Cell(index, 37).Text = ShowRate(ClientUtil.ToString(cci.OutPutRightRate));
                        fGridDetail.Cell(index, 38).WrapText = true; fGridDetail.Cell(index, 38).Text = cci.ReceivableAccount.ToString("N2");
                        fGridDetail.Cell(index, 39).WrapText = true; fGridDetail.Cell(index, 39).Text = cci.ActualAccount.ToString("N2");
                        fGridDetail.Cell(index,40).WrapText = true; fGridDetail.Cell(index, 40).Text = ClientUtil.ToString(cci.OverallBusinessPlan);
                        fGridDetail.Cell(index, 41).WrapText = true; fGridDetail.Cell(index, 41).Text = ClientUtil.ToString(cci.ResponsibilitySigh);
                         fGridDetail.Cell(index, 42).Text = cci.ReceivableRiskMortgage.ToString("N2");
                         fGridDetail.Cell(index, 43).Text = cci.ActualRiskMortgage.ToString("N2");
                         fGridDetail.Cell(index, 44).WrapText = true; fGridDetail.Cell(index, 44).Text = ClientUtil.ToString(cci.RiskMortgageRate);
                        fGridDetail.Cell(index, 45).Text = cci.OccurredHourlyAccount.ToString("N2");
                        fGridDetail.Cell(index, 46).WrapText = true; fGridDetail.Cell(index, 46).Text = ShowRate(ClientUtil.ToString(cci.ProportionOfHourlyWork));
                        fGridDetail.Cell(index, 47).Text = cci.OccurredOEMAccount.ToString("N2");
                        fGridDetail.Cell(index, 48).Text = cci.DeductedAccount.ToString("N2");
                        fGridDetail.Cell(index, 49).Text = cci.OuterContractAccount.ToString("N2");
                        fGridDetail.Cell(index, 50).Text = cci.SelfSignedAccount.ToString("N2");
                        fGridDetail.Cell(index, 51).WrapText = true; fGridDetail.Cell(index, 51).Text = ShowRate(ClientUtil.ToString(cci.ProportionOfOuterContractAccount));
                        fGridDetail.Row(index).AutoFit();
                        iCount++;
                    }
                }
                #endregion

                #region  局成本报表
                if (type == reportJCBBB)
                {
                    iCount = bcDtls.Count;
                    iStart = 8;
                    fGridDetail_bc.InsertRow(iStart, iCount);
                    FlexCell.Range range = fGridDetail_bc.Range(iStart, 1, iCount + iStart, fGridDetail_bc.Cols - 1);
                    iCount = 0;
                    int index = 0;
                    //                    List<decimal> sum = new List<decimal>();
                    //                    decimal[] d = new decimal[34];
                    //                    for (int i = 0; i < 34; i++)
                    //                    {
                    //                        d[i] = new decimal(0);
                    //                        sum.Add(d[i]);
                    //                    }
                    //                    int sum34 = 0;
                    //                    int sum35 = 0;
                    foreach (BureauCostDtl bc in bcDtls )
                    {
                        index = iStart + iCount;
                        fGridDetail_bc.Cell(index, 1).Text = (iCount + 1).ToString();
                        fGridDetail_bc.Cell(index, 2).WrapText = true;
                        fGridDetail_bc.Cell(index, 2).Text = ClientUtil.ToString(crMaster.ProjectName);
                        fGridDetail_bc.Cell(index, 3).WrapText = true; fGridDetail_bc.Cell(index, 3).Text = ClientUtil.ToString(bc.ProjectType);
                        fGridDetail_bc.Cell(index, 4).WrapText = true; fGridDetail_bc.Cell(index, 4).Text = ClientUtil.ToString(bc.ProjectScale);
                        fGridDetail_bc.Cell(index, 5).WrapText = true; fGridDetail_bc.Cell(index, 5).Text = ClientUtil.ToString(bc.Location);
                        fGridDetail_bc.Cell(index, 6).WrapText = true;
                        fGridDetail_bc.Cell(index, 6).Text = ClientUtil.ToString(bc.OwnerName);
                        fGridDetail_bc.Cell(index, 7).WrapText = true;
                        fGridDetail_bc.Cell(index, 7).Text = ClientUtil.ToString(bc.GeneralContractName);
                        fGridDetail_bc.Cell(index, 8).WrapText = true;
                        fGridDetail_bc.Cell(index, 8).Text = ClientUtil.ToString(bc.OwnerProperty);
                        fGridDetail_bc.Cell(index, 9).WrapText = true;
                        fGridDetail_bc.Cell(index, 9).Text = ClientUtil.ToString(bc.IsStractegicCustomer);
                        fGridDetail_bc.Cell(index, 10).WrapText = true;
                        fGridDetail_bc.Cell(index, 10).Text = ClientUtil.ToString(bc.ProjectSite);
                        fGridDetail_bc.Cell(index, 11).Text = bc.ConstructionAcreage.ToString("N2");
                        fGridDetail_bc.Cell(index, 12).Text = bc.ConstructionHeight.ToString("N2"); ;
                        fGridDetail_bc.Cell(index, 13).Text = bc.TotalContractAmount.ToString("N2");
                        fGridDetail_bc.Cell(index, 14).Text = bc.SelfContractAmount.ToString("N2");
                        fGridDetail_bc.Cell(index, 15).Text = bc.GeneralManageCost.ToString("N2");
                        fGridDetail_bc.Cell(index, 16).WrapText = true; fGridDetail_bc.Cell(index, 16).Text = ClientUtil.ToString(bc.ManageCostCalculateRate);
                        fGridDetail_bc.Cell(index, 17).WrapText = true;
                        fGridDetail_bc.Cell(index, 17).Text = ClientUtil.ToString(bc.SettlementType);
                        fGridDetail_bc.Cell(index, 18).Text = ClientUtil.ToDateTime(bc.SettlementTime).ToShortDateString();
                        fGridDetail_bc.Cell(index, 19).WrapText = true;
                        fGridDetail_bc.Cell(index, 19).Text = ClientUtil.ToString(bc.InvokeSituation);
                        fGridDetail_bc.Cell(index, 20).WrapText = true; fGridDetail_bc.Cell(index, 20).Text = ClientUtil.ToString(bc.ImprestRate);
                        fGridDetail_bc.Cell(index, 21).WrapText = true;
                        fGridDetail_bc.Cell(index, 21).Text = ClientUtil.ToString(bc.PaymentMode);
                        fGridDetail_bc.Cell(index, 22).WrapText = true; fGridDetail_bc.Cell(index, 22).Text = ClientUtil.ToString(bc.PaymentRate);
                        fGridDetail_bc.Cell(index, 23).WrapText = true;
                        fGridDetail_bc.Cell(index, 23).Text = ClientUtil.ToString(bc.PaymentForm);
                        fGridDetail_bc.Cell(index, 24).Text = ClientUtil.ToDateTime(bc.MaintenancePayTime).ToShortDateString();
                        fGridDetail_bc.Cell(index, 25).WrapText = true; fGridDetail_bc.Cell(index, 25).Text = ClientUtil.ToString(bc.ManagementMode);
                        fGridDetail_bc.Cell(index, 26).Text = bc.TargetScopeMoney.ToString("N2");
                        fGridDetail_bc.Cell(index, 27).Text = ClientUtil.ToDateTime(bc.DutyAgreementTime).ToShortDateString();
                        fGridDetail_bc.Cell(index, 28).WrapText = true; fGridDetail_bc.Cell(index, 28).Text = ClientUtil.ToString(bc.CheckNodeSetting);
                        fGridDetail_bc.Cell(index, 29).WrapText = true; fGridDetail_bc.Cell(index, 29).Text = ClientUtil.ToString(bc.ForeBidEarningRate);
                        fGridDetail_bc.Cell(index, 30).Text = bc.CalculateCost.ToString("N2");
                        fGridDetail_bc.Cell(index, 31).WrapText = true; fGridDetail_bc.Cell(index, 31).Text = ClientUtil.ToString(bc.CalculateBraekevenRate);
                        fGridDetail_bc.Cell(index, 32).Text = bc.TargetCost.ToString("N2");
                        fGridDetail_bc.Cell(index, 33).WrapText = true; fGridDetail_bc.Cell(index, 33).Text = ClientUtil.ToString(bc.HandInRate);//bc.HandInRate.ToString("N2");
                        fGridDetail_bc.Cell(index, 34).WrapText = true;
                        fGridDetail_bc.Cell(index, 34).Text = ClientUtil.ToString(bc.TargetRemark);
                        fGridDetail_bc.Cell(index, 35).Text = bc.ShouldPaydeposit.ToString("N2");
                        fGridDetail_bc.Cell(index, 36).Text = bc.ActualPayDeposit.ToString("N2");
                        fGridDetail_bc.Cell(index, 37).WrapText = true; fGridDetail_bc.Cell(index, 37).Text = ClientUtil.ToString(bc.QuarterFulfillTimes);
                        fGridDetail_bc.Cell(index, 38).Text = bc.QuarterShouldPayMoney.ToString("N2");
                        fGridDetail_bc.Cell(index, 39).Text = bc.QuarterActualPayMoney.ToString("N2");
                        fGridDetail_bc.Cell(index, 40).Text = bc.Quarter2015Money.ToString("N2");
                        fGridDetail_bc.Cell(index, 41).WrapText = true; fGridDetail_bc.Cell(index, 41).Text = ClientUtil.ToString(bc.NodeFulfillTimes);
                        fGridDetail_bc.Cell(index, 42).Text = bc.NodeShouldPayMoney.ToString("N2");
                        fGridDetail_bc.Cell(index, 43).Text = bc.NodeActualPayMoney.ToString("N2");
                        fGridDetail_bc.Cell(index, 44).Text = bc.Node2015Money.ToString("N2");
                        fGridDetail_bc.Cell(index, 45).Text = bc.GeneralCompleteOutput.ToString("N2");
                        fGridDetail_bc.Cell(index, 46).Text = bc.SelefCompleteOutput.ToString("N2");
                        fGridDetail_bc.Cell(index, 47).Text = bc.ExpectConfirmAmount.ToString("N2");
                        fGridDetail_bc.Cell(index, 48).Text = ClientUtil.ToDateTime(bc.ExpectConfirmDate).ToShortDateString();
                        fGridDetail_bc.Cell(index, 49).Text = bc.ConfirmedAmount.ToString("N2");
                        fGridDetail_bc.Cell(index, 50).Text = ClientUtil.ToDateTime(bc.ConfirmedDate).ToShortDateString();
                        fGridDetail_bc.Cell(index, 51).Text = bc.EstimateAmount.ToString("N2");
                        fGridDetail_bc.Cell(index, 52).Text = bc.ProjectActualCost.ToString("N2");
                        fGridDetail_bc.Cell(index, 53).Text = bc.ProjectProfit.ToString("N2");
                        fGridDetail_bc.Cell(index, 54).Text = bc.GatheringAmountAtRate.ToString("N2");
                        fGridDetail_bc.Cell(index, 55).Text = bc.SelfAcceptAmount.ToString("N2");
                        fGridDetail_bc.Cell(index, 56).Text = bc.SelfImprest.ToString("N2");
                        fGridDetail_bc.Cell(index, 57).Text = bc.SelfProgressAmount.ToString("N2");
                        fGridDetail_bc.Cell(index, 58).Text = bc.SelfOthers.ToString("N2");
                        fGridDetail_bc.Cell(index, 59).Text = bc.PaymentAtRate.ToString("N2");
                        fGridDetail_bc.Cell(index, 60).Text = bc.PayedAmount.ToString("N2");
                        fGridDetail_bc.Cell(index, 61).WrapText = true; fGridDetail_bc.Cell(index, 61).Text = ClientUtil.ToString(bc.ReportedQuantity);
                        fGridDetail_bc.Cell(index, 62).Text = bc.ReportedAmount.ToString("N2");
                        fGridDetail_bc.Cell(index, 63).Text = bc.CorrespondingCost.ToString("N2");
                        fGridDetail_bc.Cell(index, 64).WrapText = true; fGridDetail_bc.Cell(index, 64).Text = ClientUtil.ToString(bc.OwnerConfirmQuantity);
                        fGridDetail_bc.Cell(index, 65).Text = bc.OwnerConfirmAmount.ToString("N2");
                        fGridDetail_bc.Cell(index, 66).Text = bc.PlanReduceAmount.ToString("N2");
                        fGridDetail_bc.Cell(index, 67).Text = bc.PlanIncreaseAmount.ToString("N2");
                        fGridDetail_bc.Cell(index, 68).Text = bc.ActualReduceAmount.ToString("N2");
                        fGridDetail_bc.Cell(index, 69).Text = bc.ActualIncreaseAmount.ToString("N2");
                        fGridDetail_bc.Cell(index, 70).Text = bc.AccumulatePaidBonus.ToString("N2");
                        fGridDetail_bc.Cell(index, 71).Text = bc.PaidBonus2015.ToString("N2");
                        fGridDetail_bc.Cell(index, 72).Text = bc.RebarDetailingAmount.ToString("N2");
                        fGridDetail_bc.Cell(index, 73).Text = ClientUtil.ToDateTime(bc.ContractStartDate).ToShortDateString();
                        fGridDetail_bc.Cell(index, 74).Text = ClientUtil.ToDateTime(bc.ContractEndDate).ToShortDateString();
                        fGridDetail_bc.Cell(index, 75).Text = ClientUtil.ToDateTime(bc.ActualStartDate).ToShortDateString();
                        fGridDetail_bc.Cell(index, 76).Text = ClientUtil.ToDateTime(bc.ExpectEendDate).ToShortDateString();
                        fGridDetail_bc.Cell(index, 77).WrapText = true; fGridDetail_bc.Cell(index, 77).Text = ClientUtil.ToString(bc.IsPerforming);
                        fGridDetail_bc.Cell(index, 78).WrapText = true; fGridDetail_bc.Cell(index, 78).Text = ClientUtil.ToString(bc.DelayDays);
                        fGridDetail_bc.Cell(index, 79).Text = bc.CauseIncreaseCost.ToString("N2");
                        fGridDetail_bc.Cell(index, 80).WrapText = true;
                        fGridDetail_bc.Cell(index, 80).Text = ClientUtil.ToString(bc.DurationCaption);
                        fGridDetail_bc.Cell(index, 81).WrapText = true; fGridDetail_bc.Cell(index, 81).Text = ClientUtil.ToString(bc.OwnerSignDays);
                      fGridDetail_bc.Cell(index, 82).Text = bc.ConfirmExpense.ToString("N2");
                        fGridDetail_bc.Cell(index, 83).Text = bc.ConfrontForfeit.ToString("N2");
                        fGridDetail_bc.Cell(index, 84).WrapText = true; fGridDetail_bc.Cell(index, 84).Text = ClientUtil.ToString(bc.ProgramType);
                        fGridDetail_bc.Cell(index, 85).WrapText = true; fGridDetail_bc.Cell(index, 85).Text = ClientUtil.ToString(bc.ProgramState);
                        fGridDetail_bc.Row(index).AutoFit();
                        iCount++;
                        //                        sum[0] += ClientUtil.ToDecimal(bc.ConstructionAcreage);
                        //                        sum[1] += ClientUtil.ToDecimal(bc.TotalContractAmount);
                        //                        sum[2] += ClientUtil.ToDecimal(bc.SelfContractAmount);
                        //                        sum[3] += ClientUtil.ToDecimal(bc.GeneralManageCost);
                        //                        sum[4] += ClientUtil.ToDecimal(bc.TargetScopeMoney);
                        //                        sum[5] += ClientUtil.ToDecimal(bc.CalculateCost);
                        //                        sum[6] += ClientUtil.ToDecimal(bc.HandInRate);
                        //                        sum[7] += ClientUtil.ToDecimal(bc.ShouldPaydeposit);
                        //                        sum[8] += ClientUtil.ToDecimal(bc.ActualPayDeposit);
                        //                        sum[9] += ClientUtil.ToDecimal(bc.QuarterShouldPayMoney);
                        //                        sum[10] += ClientUtil.ToDecimal(bc.QuarterActualPayMoney);
                        //                        sum[11] += ClientUtil.ToDecimal(bc.Quarter2015Money);
                        //                        sum[12] += ClientUtil.ToDecimal(bc.NodeShouldPayMoney);
                        //                        sum[13] += ClientUtil.ToDecimal(bc.NodeActualPayMoney);
                        //                        sum[14] += ClientUtil.ToDecimal(bc.Node2015Money);
                        //                        sum[15] += ClientUtil.ToDecimal(bc.GeneralCompleteOutput);
                        //                        sum[16] += ClientUtil.ToDecimal(bc.SelefCompleteOutput);
                        //                        sum[17] += ClientUtil.ToDecimal(bc.ExpectConfirmAmount);
                        //                        sum[18] += ClientUtil.ToDecimal(bc.ConfirmedAmount);
                        //                        sum[19] += ClientUtil.ToDecimal(bc.EstimateAmount);
                        //                        sum[20] += ClientUtil.ToDecimal(bc.ProjectActualCost);
                        //                        sum[21] += ClientUtil.ToDecimal(bc.ProjectProfit);
                        //                        sum[22] += ClientUtil.ToDecimal(bc.SelfAcceptAmount);
                        //                        sum[23] += ClientUtil.ToDecimal(bc.SelfOthers);
                        //                        sum[24] += ClientUtil.ToDecimal(bc.PaymentAtRate);
                        //                        sum[25] += ClientUtil.ToDecimal(bc.PayedAmount);
                        //                        sum[26] += ClientUtil.ToDecimal(bc.ReportedAmount);
                        //                        sum[27] += ClientUtil.ToDecimal(bc.OwnerConfirmAmount);
                        //                        sum[28] += ClientUtil.ToDecimal(bc.PlanReduceAmount);
                        //                        sum[29] += ClientUtil.ToDecimal(bc.PlanIncreaseAmount);
                        //                        sum[30] += ClientUtil.ToDecimal(bc.ActualReduceAmount);
                        //                        sum[31] += ClientUtil.ToDecimal(bc.ActualIncreaseAmount);
                        //                        sum[32] += ClientUtil.ToDecimal(bc.AccumulatePaidBonus);
                        //                        sum[33] += ClientUtil.ToDecimal(bc.PaidBonus2015);
                        //                        sum34 += ClientUtil.ToInt(bc.ReportedQuantity);
                        //                        sum35 += ClientUtil.ToInt(bc.OwnerConfirmQuantity);
                    }
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 1).Text = ClientUtil.ToString("合计");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 11).Text = ClientUtil.ToDecimal(sum[0]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 13).Text = ClientUtil.ToDecimal(sum[1]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 14).Text = ClientUtil.ToDecimal(sum[2]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 15).Text = ClientUtil.ToDecimal(sum[3]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 26).Text = ClientUtil.ToDecimal(sum[4]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 30).Text = ClientUtil.ToDecimal(sum[5]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 33).Text = ClientUtil.ToDecimal(sum[6]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 35).Text = ClientUtil.ToDecimal(sum[7]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 36).Text = ClientUtil.ToDecimal(sum[8]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 38).Text = ClientUtil.ToDecimal(sum[9]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 39).Text = ClientUtil.ToDecimal(sum[10]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 40).Text = ClientUtil.ToDecimal(sum[11]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 42).Text = ClientUtil.ToDecimal(sum[12]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 43).Text = ClientUtil.ToDecimal(sum[13]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 44).Text = ClientUtil.ToDecimal(sum[14]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 45).Text = ClientUtil.ToDecimal(sum[15]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 46).Text = ClientUtil.ToDecimal(sum[16]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 47).Text = ClientUtil.ToDecimal(sum[17]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 49).Text = ClientUtil.ToDecimal(sum[18]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 51).Text = ClientUtil.ToDecimal(sum[19]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 52).Text = ClientUtil.ToDecimal(sum[20]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 53).Text = ClientUtil.ToDecimal(sum[21]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 55).Text = ClientUtil.ToDecimal(sum[22]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 58).Text = ClientUtil.ToDecimal(sum[23]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 59).Text = ClientUtil.ToDecimal(sum[24]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 60).Text = ClientUtil.ToDecimal(sum[25]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 61).Text = ClientUtil.ToDecimal(sum34).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 62).Text = ClientUtil.ToDecimal(sum[26]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 64).Text = ClientUtil.ToDecimal(sum35).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 65).Text = ClientUtil.ToDecimal(sum[27]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 66).Text = ClientUtil.ToDecimal(sum[28]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 67).Text = ClientUtil.ToDecimal(sum[29]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 68).Text = ClientUtil.ToDecimal(sum[30]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 69).Text = ClientUtil.ToDecimal(sum[31]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 70).Text = ClientUtil.ToDecimal(sum[32]).ToString("N2");
                    //                    fGridDetail_bc.Cell(fGridDetail_bc.Rows - 2, 71).Text = ClientUtil.ToDecimal(sum[33]).ToString("N2");
                }
                #endregion
                #region 专业分包
                if (type == reportFBZB)
                {
                    int count = 0;
                    foreach (BearerIndicatorDtl bi in biDtls)
                    {
                        if (bi.FLAG == "0")//0代表专业分包,前台过滤数据
                        {
                            count++;
                        }
                    }
                    iCount = count;//过滤查询结果的

                    //iCount = biDtls.Count;//之前的没过滤查询结果的



                    iStart = 7;
                    fGridDetail_bi.InsertRow(iStart, iCount + 1); //合计需要再多加一行,没有合计之前是fGridDetail_bi.InsertRow(iStart, iCount)
                    FlexCell.Range range = fGridDetail_bi.Range(iStart, 1, iCount + iStart, fGridDetail.Cols - 1);

                    iCount = 0;
                    int index = 0;

                    #region  合计 20160829 add
                    decimal SumCurrenContractMoney = 0.00M;//对应的列：5
                    decimal SumCurrentSettleMoney = 0.00M;//对应的列：6
                    decimal SumCurrentOuterSettleMoney = 0.00M;//对应的列：7
                    decimal SumIncomeMoney = 0.00M;//对应的列：9
                    decimal SumCurrentSelfSignMoney = 0.00M;//对应的列：10
                    decimal SumAccrueSettleMoney = 0.00M;//对应的列：12
                    decimal SumAccrueOuterSettleMoney = 0.00M;//对应的列：13
                    decimal SumAccrueSelfSignMoney = 0.00M;//对应的列：14
                    decimal SumCurrentOemMoney = 0.00M;//对应的列：15
                    decimal SumCurrentBeOemMoney = 0.00M;//对应的列：16
                    decimal SumAccrueOemMoney = 0.00M;//对应的列：17
                    decimal SumAccrueBeOemMoney = 0.00M;//对应的列：18
                    decimal SumCurrentHourlyMoney = 0.00M;//对应的列：19
                    decimal SumAccrueHourlyMoney = 0.00M;//对应的列：21
                    #endregion
                    foreach (BearerIndicatorDtl bi in biDtls.OrderBy(a => a.OrderNo))//之前的没过滤查询结果的
                    //foreach (BearerIndicatorDtl bi in CM.BiDtl)//20160825 add 直接通过SQL操作取值
                    {

                        if (bi.FLAG != "0")//0代表专业分包,前台过滤数据
                        {

                        }
                        else
                        {

                            index = iStart + iCount;
                            fGridDetail_bi.Cell(index, 1).Tag = bi.Id;
                            fGridDetail_bi.Cell(index, 1).Text = (iCount + 1).ToString();
                            fGridDetail_bi.Cell(index, 2).WrapText = true;
                            fGridDetail_bi.Cell(index, 2).Text = ClientUtil.ToString(crMaster.ProjectName);
                            fGridDetail_bi.Cell(index, 3).WrapText = true;
                            fGridDetail_bi.Cell(index, 3).Text = ClientUtil.ToString(bi.ConstructionTeam);
                            fGridDetail_bi.Cell(index, 4).WrapText = true;
                            fGridDetail_bi.Cell(index, 4).Text = ClientUtil.ToString(bi.ConstructionContent);
                            fGridDetail_bi.Cell(index, 5).Text = bi.CurrenContractMoney.ToString("N2");
                            fGridDetail_bi.Cell(index, 6).Text = bi.CurrentSettleMoney.ToString("N2");
                            fGridDetail_bi.Cell(index, 7).Text = bi.CurrentOuterSettleMoney.ToString("N2"); fGridDetail_bi.Cell(index, 7).WrapText = true;
                            fGridDetail_bi.Cell(index, 8).Text = ClientUtil.ToString(bi.IsConrresponding); fGridDetail_bi.Cell(index, 8).WrapText = true;
                            fGridDetail_bi.Cell(index, 9).Text = bi.IncomeMoney.ToString("N2");
                            fGridDetail_bi.Cell(index, 10).Text = bi.CurrentSelfSignMoney.ToString("N2");
                            fGridDetail_bi.Cell(index, 11).WrapText = true;
                            fGridDetail_bi.Cell(index, 11).Text = ClientUtil.ToString(bi.Descript);
                            fGridDetail_bi.Cell(index, 12).Text = bi.AccrueSettleMoney.ToString("N2");
                            fGridDetail_bi.Cell(index, 13).Text = bi.AccrueOuterSettleMoney.ToString("N2");
                            fGridDetail_bi.Cell(index, 14).Text = bi.AccrueSelfSignMoney.ToString("N2");
                            fGridDetail_bi.Cell(index, 15).Text = bi.CurrentOemMoney.ToString("N2");
                            fGridDetail_bi.Cell(index, 16).Text = bi.CurrentBeOemMoney.ToString("N2");
                            fGridDetail_bi.Cell(index, 17).Text = bi.AccrueOemMoney.ToString("N2");
                            fGridDetail_bi.Cell(index, 18).Text = bi.AccrueBeOemMoney.ToString("N2");
                            fGridDetail_bi.Cell(index, 19).Text = bi.CurrentHourlyMoney.ToString("N2");
                            //fGridDetail_bi.Cell(index, 20).Text = ClientUtil.ToString(bi.CurrentHourlyRate);
                            fGridDetail_bi.Cell(index, 21).Text = bi.AccrueHourlyMoney.ToString("N2");
                            //fGridDetail_bi.Cell(index, 22).Text = ClientUtil.ToString(bi.AccrueHourlyRate);


                            #region  合计  20160829 add
                            SumCurrenContractMoney += bi.CurrenContractMoney;//对应的列：5
                            SumCurrentSettleMoney += bi.CurrentSettleMoney;//对应的列：6
                            SumCurrentOuterSettleMoney += bi.CurrentOuterSettleMoney;//对应的列：7
                            SumIncomeMoney += bi.IncomeMoney;//对应的列：9
                            SumCurrentSelfSignMoney += bi.CurrentSelfSignMoney;//对应的列：10
                            SumAccrueSettleMoney += bi.AccrueSettleMoney;//对应的列：12
                            SumAccrueOuterSettleMoney += bi.AccrueOuterSettleMoney;//对应的列：13
                            SumAccrueSelfSignMoney += bi.AccrueSelfSignMoney;//对应的列：14
                            SumCurrentOemMoney += bi.CurrentOemMoney;//对应的列：15
                            SumCurrentBeOemMoney += bi.CurrentBeOemMoney;//对应的列：16
                            SumAccrueOemMoney += bi.AccrueOemMoney;//对应的列：17
                            SumAccrueBeOemMoney += bi.AccrueBeOemMoney;//对应的列：18
                            SumCurrentHourlyMoney += bi.CurrentHourlyMoney;//对应的列：19
                            SumAccrueHourlyMoney += bi.AccrueHourlyMoney;//对应的列：21
                            #endregion


                            //fGridDetail_bi.Row(index).AutoFit();
                            iCount++;
                        }

                    }
                    #region 在最后一行加入合计 20160829

                    index = iStart + iCount;
                    fGridDetail_bi.Cell(index, 1).Text = "合计：";
                    //fGridDetail_bi.Cell(index, 1).Text = (iCount + 1).ToString();
                    //fGridDetail_bi.Cell(index, 2).WrapText = true;
                    //fGridDetail_bi.Cell(index, 2).Text = ClientUtil.ToString(crMaster.ProjectName);
                    //fGridDetail_bi.Cell(index, 3).WrapText = true;
                    //fGridDetail_bi.Cell(index, 3).Text = ClientUtil.ToString(bi.ConstructionTeam);
                    //fGridDetail_bi.Cell(index, 4).WrapText = true;
                    //fGridDetail_bi.Cell(index, 4).Text = ClientUtil.ToString(bi.ConstructionContent);
                    fGridDetail_bi.Cell(index, 5).Text = SumCurrenContractMoney.ToString("N2");
                    fGridDetail_bi.Cell(index, 6).Text = SumCurrentSettleMoney.ToString("N2");
                    fGridDetail_bi.Cell(index, 7).Text = SumCurrentOuterSettleMoney.ToString("N2");
                    //fGridDetail_bi.Cell(index, 8).Text = ClientUtil.ToString(bi.IsConrresponding);
                    fGridDetail_bi.Cell(index, 9).Text = SumIncomeMoney.ToString("N2");
                    fGridDetail_bi.Cell(index, 10).Text = SumCurrentSelfSignMoney.ToString("N2");
                    //fGridDetail_bi.Cell(index, 11).WrapText = true;
                    //fGridDetail_bi.Cell(index, 11).Text = ClientUtil.ToString(bi.Descript);
                    fGridDetail_bi.Cell(index, 12).Text = SumAccrueSettleMoney.ToString("N2");
                    fGridDetail_bi.Cell(index, 13).Text = SumAccrueOuterSettleMoney.ToString("N2");
                    fGridDetail_bi.Cell(index, 14).Text = SumAccrueSelfSignMoney.ToString("N2");
                    fGridDetail_bi.Cell(index, 15).Text = SumCurrentOemMoney.ToString("N2");
                    fGridDetail_bi.Cell(index, 16).Text = SumCurrentBeOemMoney.ToString("N2");
                    fGridDetail_bi.Cell(index, 17).Text = SumAccrueOemMoney.ToString("N2");
                    fGridDetail_bi.Cell(index, 18).Text = SumAccrueBeOemMoney.ToString("N2");
                    fGridDetail_bi.Cell(index, 19).Text = SumCurrentHourlyMoney.ToString("N2");
                    //fGridDetail_bi.Cell(index, 20).Text = ClientUtil.ToString(bi.CurrentHourlyRate);
                    fGridDetail_bi.Cell(index, 21).Text = SumAccrueHourlyMoney.ToString("N2");
                    //fGridDetail_bi.Cell(index, 22).Text = ClientUtil.ToString(bi.AccrueHourlyRate);


                    fGridDetail_bi.Row(index).AutoFit();

                    #endregion





                }
                #endregion
                #region 劳务分包指标  20160824
                if (type == reportLWFBZB)
                {


                    int count = 0;
                    foreach (BearerIndicatorDtl bi in biDtls)
                    {
                        if (bi.FLAG == "1")//1代表劳务分包,前台过滤数据
                        {
                            count++;
                        }
                    }
                    iCount = count;//过滤查询结果的

                    //iCount = biDtls.Count;
                    iStart = 7;
                    fGridDetail_blw.InsertRow(iStart, iCount + 1);//合计，需要多加一行，没合计之前是：fGridDetail_blw.InsertRow(iStart, iCount)
                    FlexCell.Range range = fGridDetail_blw.Range(iStart, 1, iCount + iStart, fGridDetail.Cols - 1);

                    iCount = 0;
                    int index = 0;

                    #region  合计 20160829 add
                    decimal SumCurrenContractMoney = 0.00M;//对应的列：5
                    decimal SumCurrentSettleMoney = 0.00M;//对应的列：6
                    decimal SumCurrentOuterSettleMoney = 0.00M;//对应的列：7
                    decimal SumIncomeMoney = 0.00M;//对应的列：9
                    decimal SumCurrentSelfSignMoney = 0.00M;//对应的列：10
                    decimal SumAccrueSettleMoney = 0.00M;//对应的列：12
                    decimal SumAccrueOuterSettleMoney = 0.00M;//对应的列：13
                    decimal SumAccrueSelfSignMoney = 0.00M;//对应的列：14
                    decimal SumCurrentOemMoney = 0.00M;//对应的列：15
                    decimal SumCurrentBeOemMoney = 0.00M;//对应的列：16
                    decimal SumAccrueOemMoney = 0.00M;//对应的列：17
                    decimal SumAccrueBeOemMoney = 0.00M;//对应的列：18
                    decimal SumCurrentHourlyMoney = 0.00M;//对应的列：19
                    decimal SumAccrueHourlyMoney = 0.00M;//对应的列：21
                    #endregion




                    foreach (BearerIndicatorDtl bi in biDtls.OrderBy(A=>A.OrderNo))
                    //foreach (BearerIndicatorDtl bi in blwDtls)
                    {
                        if (bi.FLAG != "1")//1代表劳务分包,前台过滤数据
                        {

                        }
                        else
                        {

                            index = iStart + iCount;
                            fGridDetail_blw.Cell(index, 1).Tag = bi.Id;//20160830 add

                            fGridDetail_blw.Cell(index, 1).Text = (iCount + 1).ToString();
                            fGridDetail_blw.Cell(index, 2).WrapText = true;
                            fGridDetail_blw.Cell(index, 2).Text = ClientUtil.ToString(crMaster.ProjectName);
                            fGridDetail_blw.Cell(index, 3).WrapText = true;
                            fGridDetail_blw.Cell(index, 3).Text = ClientUtil.ToString(bi.ConstructionTeam);
                            fGridDetail_blw.Cell(index, 4).WrapText = true;
                            fGridDetail_blw.Cell(index, 4).Text = ClientUtil.ToString(bi.ConstructionContent);
                            fGridDetail_blw.Cell(index, 5).Text = bi.CurrenContractMoney.ToString("N2");
                            fGridDetail_blw.Cell(index, 6).Text = bi.CurrentSettleMoney.ToString("N2");
                            fGridDetail_blw.Cell(index, 7).Text = bi.CurrentOuterSettleMoney.ToString("N2");
                            fGridDetail_blw.Cell(index, 8).Text = ClientUtil.ToString(bi.IsConrresponding); fGridDetail_blw.Cell(index, 8).WrapText = true;
                            fGridDetail_blw.Cell(index, 9).Text = bi.IncomeMoney.ToString("N2");
                            fGridDetail_blw.Cell(index, 10).Text = bi.CurrentSelfSignMoney.ToString("N2");
                            fGridDetail_blw.Cell(index, 11).WrapText = true;
                            fGridDetail_blw.Cell(index, 11).Text = ClientUtil.ToString(bi.Descript);
                            fGridDetail_blw.Cell(index, 12).Text = bi.AccrueSettleMoney.ToString("N2");
                            fGridDetail_blw.Cell(index, 13).Text = bi.AccrueOuterSettleMoney.ToString("N2");
                            fGridDetail_blw.Cell(index, 14).Text = bi.AccrueSelfSignMoney.ToString("N2");
                            fGridDetail_blw.Cell(index, 15).Text = bi.CurrentOemMoney.ToString("N2");
                            fGridDetail_blw.Cell(index, 16).Text = bi.CurrentBeOemMoney.ToString("N2");
                            fGridDetail_blw.Cell(index, 17).Text = bi.AccrueOemMoney.ToString("N2");
                            fGridDetail_blw.Cell(index, 18).Text = bi.AccrueBeOemMoney.ToString("N2");
                            fGridDetail_blw.Cell(index, 19).Text = bi.CurrentHourlyMoney.ToString("N2");
                            //fGridDetail_blw.Cell(index, 20).Text = ClientUtil.ToString(bi.CurrentHourlyRate);
                            fGridDetail_blw.Cell(index, 21).Text = bi.AccrueHourlyMoney.ToString("N2");
                            //fGridDetail_blw.Cell(index, 22).Text = ClientUtil.ToString(bi.AccrueHourlyRate);

                            #region  合计  20160829 add
                            SumCurrenContractMoney += bi.CurrenContractMoney;//对应的列：5
                            SumCurrentSettleMoney += bi.CurrentSettleMoney;//对应的列：6
                            SumCurrentOuterSettleMoney += bi.CurrentOuterSettleMoney;//对应的列：7
                            SumIncomeMoney += bi.IncomeMoney;//对应的列：9
                            SumCurrentSelfSignMoney += bi.CurrentSelfSignMoney;//对应的列：10
                            SumAccrueSettleMoney += bi.AccrueSettleMoney;//对应的列：12
                            SumAccrueOuterSettleMoney += bi.AccrueOuterSettleMoney;//对应的列：13
                            SumAccrueSelfSignMoney += bi.AccrueSelfSignMoney;//对应的列：14
                            SumCurrentOemMoney += bi.CurrentOemMoney;//对应的列：15
                            SumCurrentBeOemMoney += bi.CurrentBeOemMoney;//对应的列：16
                            SumAccrueOemMoney += bi.AccrueOemMoney;//对应的列：17
                            SumAccrueBeOemMoney += bi.AccrueBeOemMoney;//对应的列：18
                            SumCurrentHourlyMoney += bi.CurrentHourlyMoney;//对应的列：19
                            SumAccrueHourlyMoney += bi.AccrueHourlyMoney;//对应的列：21
                            #endregion




                            //fGridDetail_blw.Row(index).AutoFit();
                            iCount++;
                        }

                    }

                    #region 在最后一行加入合计 20160829
                    index = iStart + iCount;
                    fGridDetail_blw.Cell(index, 1).Text = "合计：";
                    //fGridDetail_bi.Cell(index, 1).Text = (iCount + 1).ToString();
                    //fGridDetail_bi.Cell(index, 2).WrapText = true;
                    //fGridDetail_bi.Cell(index, 2).Text = ClientUtil.ToString(crMaster.ProjectName);
                    //fGridDetail_bi.Cell(index, 3).WrapText = true;
                    //fGridDetail_bi.Cell(index, 3).Text = ClientUtil.ToString(bi.ConstructionTeam);
                    //fGridDetail_bi.Cell(index, 4).WrapText = true;
                    //fGridDetail_bi.Cell(index, 4).Text = ClientUtil.ToString(bi.ConstructionContent);
                    fGridDetail_blw.Cell(index, 5).Text = SumCurrenContractMoney.ToString("N2");
                    fGridDetail_blw.Cell(index, 6).Text = SumCurrentSettleMoney.ToString("N2");
                    fGridDetail_blw.Cell(index, 7).Text = SumCurrentOuterSettleMoney.ToString("N2");
                    //fGridDetail_bi.Cell(index, 8).Text = ClientUtil.ToString(bi.IsConrresponding);
                    fGridDetail_blw.Cell(index, 9).Text = SumIncomeMoney.ToString("N2");
                    fGridDetail_blw.Cell(index, 10).Text = SumCurrentSelfSignMoney.ToString("N2");
                    //fGridDetail_bi.Cell(index, 11).WrapText = true;
                    //fGridDetail_bi.Cell(index, 11).Text = ClientUtil.ToString(bi.Descript);
                    fGridDetail_blw.Cell(index, 12).Text = SumAccrueSettleMoney.ToString("N2");
                    fGridDetail_blw.Cell(index, 13).Text = SumAccrueOuterSettleMoney.ToString("N2");
                    fGridDetail_blw.Cell(index, 14).Text = SumAccrueSelfSignMoney.ToString("N2");
                    fGridDetail_blw.Cell(index, 15).Text = SumCurrentOemMoney.ToString("N2");
                    fGridDetail_blw.Cell(index, 16).Text = SumCurrentBeOemMoney.ToString("N2");
                    fGridDetail_blw.Cell(index, 17).Text = SumAccrueOemMoney.ToString("N2");
                    fGridDetail_blw.Cell(index, 18).Text = SumAccrueBeOemMoney.ToString("N2");
                    fGridDetail_blw.Cell(index, 19).Text = SumCurrentHourlyMoney.ToString("N2");
                    //fGridDetail_bi.Cell(index, 20).Text = ClientUtil.ToString(bi.CurrentHourlyRate);
                    fGridDetail_blw.Cell(index, 21).Text = SumAccrueHourlyMoney.ToString("N2");
                    //fGridDetail_bi.Cell(index, 22).Text = ClientUtil.ToString(bi.AccrueHourlyRate);


                    fGridDetail_blw.Row(index).AutoFit();

                    #endregion



                }
                #endregion



                #region 签证索赔  20160826
                if (type == reportQZSPB)
                {



                    iCount = vcDtls.Count;
                    iStart = 5;
                    fGridDetail_vc.InsertRow(iStart, iCount);//此表格只有一行，这里注释 20160831 新需求

                    FlexCell.Range range = fGridDetail_vc.Range(iStart, 1, iCount + iStart, fGridDetail.Cols - 1);

                    iCount = 0;
                    int index = 0;



                    foreach (VisaClamisDtl bi in vcDtls)
                    {
                        index = iStart + iCount;
                        fGridDetail_vc.Cell(index, 1).Text = (iCount + 1).ToString();
                        fGridDetail_vc.Cell(index, 2).WrapText = true;
                        fGridDetail_vc.Cell(index, 2).Text = ClientUtil.ToString(crMaster.ProjectName);
                        fGridDetail_vc.Cell(index, 3).WrapText = true;
                        fGridDetail_vc.Cell(index, 3).Text = ClientUtil.ToString(bi.YBSQZFS);
                        fGridDetail_vc.Cell(index, 4).WrapText = true;
                        fGridDetail_vc.Cell(index, 4).Text = ClientUtil.ToString(bi.YQRQZFS);
                        fGridDetail_vc.Cell(index, 5).Text = bi.QZLJBSJE.ToString("N2");
                        fGridDetail_vc.Cell(index, 6).Text = bi.LJQRSR.ToString("N2");
                        fGridDetail_vc.Cell(index, 7).Text = bi.LSSJSR.ToString("N2");
                        fGridDetail_vc.Cell(index, 8).Text = ClientUtil.ToString(bi.LJQRQZBFSYL);
                        fGridDetail_vc.Cell(index, 9).Text = bi.LJBSJE.ToString("N2");
                        fGridDetail_vc.Cell(index, 10).Text = bi.LJSJCB.ToString("N2");
                        fGridDetail_vc.Cell(index, 11).Text = bi.QZLJSJCB.ToString("N2");
                        fGridDetail_vc.Cell(index, 12).Text = ClientUtil.ToString(bi.QZXYL);
                        fGridDetail_vc.Cell(index, 13).Text = bi.JZLJZXSGCZ.ToString("N2");

                        fGridDetail_vc.Cell(index, 14).Text = ClientUtil.ToString(bi.QZZEDCZGXL);
                        fGridDetail_vc.Cell(index, 15).Text = ClientUtil.ToString(bi.QZXYCZGXL);

                        fGridDetail_vc.Cell(index, 16).Text = ClientUtil.ToString(bi.ProjectDeclareCount);
                        fGridDetail_vc.Cell(index, 17).Text = ClientUtil.ToString(bi.ProjectRiskSolution);
                        fGridDetail_vc.Cell(index, 18).Text = ClientUtil.ToString(bi.ProjectAddBenefit);
                        fGridDetail_vc.Cell(index, 19).Text = ClientUtil.ToString(bi.RewardDeclareCount);
                        fGridDetail_vc.Cell(index, 20).Text = ClientUtil.ToString(bi.RewardRiskSolution);
                        fGridDetail_vc.Cell(index, 21).Text = ClientUtil.ToString(bi.RewardAddBenefit);
                        fGridDetail_vc.Cell(index, 22).Text = ClientUtil.ToString(bi.OutPutMoney);
                        fGridDetail_vc.Cell(index, 23).Text = ClientUtil.ToString(bi.SelfMoneyRate);
                        fGridDetail_vc.Row(index).AutoFit();
                        iCount++;

                    }


                }
                #endregion

                #region  分包争议统计表
                if (type == reportFBZY)
                {
                    iCount = dtDtls.Count;
                    iStart = 5;
                    //fGridDetail_dt.InsertRow(iStart, iCount);
                    fGridDetail_dt.InsertRow(iStart, iCount + 1);//合计，需要多加一行，没合计之前是：fGridDetail_dt.InsertRow(iStart, iCount)


                    FlexCell.Range range = fGridDetail_dt.Range(iStart, 1, iCount + iStart, fGridDetail.Cols - 1);

                    iCount = 0;
                    int index = 0;


                    #region  合计
                    decimal SumInvolveMoney = 0.00M;//对应的列：7
                    #endregion


                    foreach (DisputeTrackDtl dt in dtDtls.OrderBy(a=>a.OrderNo))
                    {
                        index = iStart + iCount;
                        fGridDetail_dt.Cell(index, 1).Text = (iCount + 1).ToString();
                        fGridDetail_dt.Cell(index, 2).WrapText = true;
                        fGridDetail_dt.Cell(index, 2).Text = ClientUtil.ToString(crMaster.ProjectName);
                        fGridDetail_dt.Cell(index, 3).WrapText = true;
                        fGridDetail_dt.Cell(index, 3).Text = ClientUtil.ToString(dt.BearerUnitName);
                        fGridDetail_dt.Cell(index, 4).WrapText = true;
                        fGridDetail_dt.Cell(index, 4).Text = ClientUtil.ToString(dt.DisputeContent);
                        fGridDetail_dt.Cell(index, 5).WrapText = true;
                        fGridDetail_dt.Cell(index, 5).Text = ClientUtil.ToString(dt.BearerSuggestion);
                        fGridDetail_dt.Cell(index, 6).WrapText = true;
                        fGridDetail_dt.Cell(index, 6).Text = ClientUtil.ToString(dt.ProjectSuggestion);
                        fGridDetail_dt.Cell(index, 7).Text = dt.InvolveMoney.ToString("N2");
                        fGridDetail_dt.Cell(index, 8).WrapText = true;
                        fGridDetail_dt.Cell(index, 8).Text = ClientUtil.ToString(dt.CurrentProgress);
                        fGridDetail_dt.Cell(index, 9).WrapText = true;
                        fGridDetail_dt.Cell(index, 9).Text = ClientUtil.ToString(dt.Descript);

                        #region  合计
                        SumInvolveMoney += dt.InvolveMoney;//对应的列：7
                        #endregion
                        //fGridDetail_dt.Row(index).AutoFit();
                        iCount++;
                    }
                    #region 在最后一行加入合计
                    index = iStart + iCount;
                    fGridDetail_dt.Cell(index, 1).Text = "合计：";
                    fGridDetail_dt.Cell(index, 7).Text = SumInvolveMoney.ToString("N2");
                    fGridDetail_dt.Row(index).AutoFit();
                    #endregion
                }
                #endregion

                #region
                if (type == reportFBZJBS)
                {
                    iCount = saDtls.Count;
                    iStart = 5;
                    fGridDetail_sa.InsertRow(iStart, iCount);
                    FlexCell.Range range = fGridDetail_sa.Range(iStart, 1, iCount + iStart, fGridDetail.Cols - 1);

                    iCount = 0;
                    int index = 0;
                    foreach (SubcontractAuditDtl sa in saDtls.OrderBy(a=>a.OrderNo))
                    {
                        index = iStart + iCount;
                        fGridDetail_sa.Cell(index, 1).Text = (iCount + 1).ToString();
                        fGridDetail_sa.Cell(index, 2).WrapText = true;
                        fGridDetail_sa.Cell(index, 2).Text = ClientUtil.ToString(crMaster.ProjectName);
                        fGridDetail_sa.Cell(index, 3).WrapText = true;
                        fGridDetail_sa.Cell(index, 3).Text = ClientUtil.ToString(sa.SubentryProjectName);
                        fGridDetail_sa.Cell(index, 4).WrapText = true;
                        fGridDetail_sa.Cell(index, 4).Text = ClientUtil.ToString(sa.BearerOrgName);
                        fGridDetail_sa.Cell(index, 5).Text = sa.SubcontractAmount.ToString("N2");
                        fGridDetail_sa.Cell(index, 6).Text = sa.AccumulateAmount.ToString("N2");
                        fGridDetail_sa.Cell(index, 7).Text = ClientUtil.ToDateTime(sa.Makespan).ToShortDateString();
                        fGridDetail_sa.Cell(index, 8).Text = sa.ExpectSubcontractAmount.ToString("N2"); fGridDetail_sa.Cell(index, 8).WrapText = true;
                        fGridDetail_sa.Cell(index, 9).Text = ClientUtil.ToString(sa.IsAudit); fGridDetail_sa.Cell(index, 9).WrapText = true;
                        fGridDetail_sa.Cell(index, 10).Text = ClientUtil.ToDateTime(sa.ExpectAuditTime).ToShortDateString();
                        fGridDetail_sa.Cell(index, 11).Text = ClientUtil.ToString(sa.Descript); fGridDetail_sa.Cell(index, 11).WrapText = true;
                        fGridDetail_sa.Row(index).AutoFit();
                        iCount++;
                    }
                }
                #endregion
                #region
                if (type == reportGSJSJZ)
                {
                    iCount = sprDtls.Count;
                    iStart = 6;
                    fGridDetail_spr.InsertRow(iStart, iCount);
                    FlexCell.Range range = fGridDetail_spr.Range(iStart, 1, iCount + iStart, fGridDetail.Cols - 1);

                    iCount = 0;
                    int index = 0;
                    foreach (SettlementProgressReportDtl spr in sprDtls )
                    {
                        index = iStart + iCount;
                        fGridDetail_spr.Cell(index, 1).Text = (iCount + 1).ToString();
                        fGridDetail_spr.Cell(index, 2).WrapText = true;
                        fGridDetail_spr.Cell(index, 2).Text = ClientUtil.ToString(crMaster.ProjectName);
                        fGridDetail_spr.Cell(index, 3).WrapText = true;
                        fGridDetail_spr.Cell(index, 3).Text = ClientUtil.ToString(spr.ThisMonth);
                        fGridDetail_spr.Cell(index, 4).WrapText = true;
                        fGridDetail_spr.Cell(index, 4).Text = ClientUtil.ToString(spr.SettlementProgress);
                        fGridDetail_spr.Cell(index, 5).WrapText = true;
                        fGridDetail_spr.Cell(index, 5).Text = ClientUtil.ToString(spr.ImportantFactor);
                        fGridDetail_spr.Cell(index, 6).WrapText = true;
                        fGridDetail_spr.Cell(index, 6).Text = ClientUtil.ToString(spr.NextMonthPlan);
                        fGridDetail_spr.Cell(index, 7).WrapText = true;
                        fGridDetail_spr.Cell(index, 7).Text = ClientUtil.ToString(spr.Descript);
                        fGridDetail_spr.Row(index).AutoFit();
                        iCount++;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                fGridDetail.AutoRedraw = true;
                fGridDetail.Refresh();
            }
        }
        public string  ShowRate(string sValue)
        {
            return sValue;

            if (!string.IsNullOrEmpty(sValue))
            {
                sValue = sValue.Trim();
                if (ValidateUtil.isNumeric(sValue))
                {

                    sValue = sValue + "%";

                }
            }
            return sValue;
        }
        public string GetRate(string sValue)
        {
            return sValue;
            if (!string.IsNullOrEmpty(sValue)  )
            {
            //this.projectInfo
                sValue = sValue.Trim();
                if (sValue[sValue.Length - 1] == '%')
                {
                    sValue = sValue.Substring(0, sValue.Length - 1);
                }
            }
            return sValue;
        }
       
       
    }
}