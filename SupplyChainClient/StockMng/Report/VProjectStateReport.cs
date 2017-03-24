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

namespace Application.Business.Erp.SupplyChain.Client.StockMng.Report
{
    public partial class VProjectStateReport : TBasicDataView
    {
        MProjectDepartment model = new MProjectDepartment();

        string orgSyscode = "";
        private CurrentProjectInfo ProjectInfo;
        public bool ifCall = false;
        public string sProjectID = "";
        public string sProjectName = "";
        public DateTime startDate = new DateTime();
        public DateTime endDate = new DateTime();
        #region 基本数据
        private string loginPersonName = ConstObject.LoginPersonInfo.Name;
        private string loginDate = ConstObject.LoginDate.ToShortDateString();
        string detailExptr = "项目使用状态明细表";
        #endregion
        #region 结果数据
        Hashtable htData = new Hashtable();
        
        #endregion

        public VProjectStateReport()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }

        public VProjectStateReport(bool call,string projectID, string projectName, DateTime sDate, DateTime eDate)
        {
            InitializeComponent();
            InitEvents();
            ifCall = call;
            sProjectID = projectID;
            sProjectName = projectName;
            startDate = sDate;
            endDate = eDate;
            InitData();
        }

        private void InitData()
        {
            dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-30);
            dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;

            ProjectInfo = StaticMethod.GetProjectInfo();
            if (ProjectInfo != null && ProjectInfo.Code != CommonUtil.CompanyProjectCode)
            {
                this.txtProject.Text = ProjectInfo.Name;
                this.txtProject.Tag = ProjectInfo.Id;

                this.lblPSelect.Visible = false;
                this.txtOperationOrg.Visible = false;
                this.btnOperationOrg.Visible = false;
            }

            this.fGridDetail.Rows = 1;

            if (ifCall == true)
            {
                this.lblPSelect.Visible = false;
                this.txtOperationOrg.Visible = false;
                this.btnOperationOrg.Visible = false;
                this.lblDate.Visible = false;
                this.dtpDateBegin.Visible = false;
                this.lblTo.Visible = false;
                this.dtpDateEnd.Visible = false;
                this.btnQuery.Visible = false;
                this.btnExcel.Visible = false;
                this.txtProject.Text = sProjectName;
                query();
            }
            
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
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect();
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                OperationOrgInfo info = frm.Result[0] as OperationOrgInfo;
                txtOperationOrg.Tag = info;
                txtOperationOrg.Text = info.Name;
                opgId = info.Id;
            }
            DataDomain domain = model.CurrentSrv.GetProjectInfoByOpgId(opgId);
            if (TransUtil.ToString(domain.Name1) != "")
            {
                this.txtProject.Text = TransUtil.ToString(domain.Name2);
                this.txtProject.Tag = TransUtil.ToString(domain.Name1);
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            fGridDetail.ExportToExcel(detailExptr, false, false, true);
        }

        private void query()
        {
            LoadTempleteFile(detailExptr + ".flx");

            //载入数据
            this.LoadDetailFile();
            this.LoadTotalFile();

            //设置外观
            fGridDetail.BackColor1 = System.Drawing.SystemColors.ButtonFace;
            fGridDetail.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            query();
        }

        private void LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式
                if (modelName == "项目使用状态明细表.flx")
                {                   
                    fGridDetail.OpenFile(path + "\\" + modelName);//载入格式
                }
            } else {
                MessageBox.Show("未找到模板格式文件" + modelName);
                return;
            }
        }

        #region 项目使用状态明细表
       
        private void LoadDetailFile()
        {
            string projectID = this.txtProject.Tag as string;
            if (projectID == null && ifCall == false)
            {
                MessageBox.Show("请选择项目！");
                return;
            }
            FlashScreen.Show("正在生成[项目使用状态]报告...");
            
            try
            {
                if (ifCall == false)
                {
                    htData = model.CurrentSrv.QueryProjectStateInfo(projectID, dtpDateBegin.Value.Date, dtpDateEnd.Value.Date);
                }
                else
                {
                    htData = model.CurrentSrv.QueryProjectStateInfo(sProjectID, startDate, endDate);
                }
            }
            catch (Exception e1)
            {
                throw new Exception("生成[项目使用状态]报告异常[" + e1.Message + "]");
            }
            finally
            {
                FlashScreen.Close();
            }
            this.fGridDetail.Cell(2, 4).Text = ClientUtil.ToString(htData["basinInfo"]);
            this.fGridDetail.Cell(3, 4).Text = ClientUtil.ToString(htData["perandjobinfo"]);
            this.fGridDetail.Cell(4, 4).Text = ClientUtil.ToString(htData["pbsinfo"]);
            this.fGridDetail.Cell(5, 4).Text = ClientUtil.ToString(htData["gwbsinfo"]);
            this.fGridDetail.Cell(6, 4).Text = ClientUtil.ToString(htData["costbudgetinfo"]);
            this.fGridDetail.Cell(7, 4).Text = ClientUtil.ToString(htData["produceinfo"]);
            this.fGridDetail.Cell(8, 4).Text = ClientUtil.ToString(htData["safecheckinfo"]);
            this.fGridDetail.Cell(9, 4).Text = ClientUtil.ToString(htData["materialinfo"]);
            this.fGridDetail.Cell(10, 4).Text = ClientUtil.ToString(htData["subbalinfo"]);
            this.fGridDetail.Cell(11, 4).Text = ClientUtil.ToString(htData["costinfo"]);
            this.fGridDetail.Cell(12, 4).Text = ClientUtil.ToString(htData["useinfo"]);
        }

        #endregion

        #region 项目使用状态综合表

        private void LoadTotalFile()
        {
            
        }
        #endregion
    }
}