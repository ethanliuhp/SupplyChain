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
    public partial class VAcceptanceBillReport : TBasicDataView
    {
        MIndirectCost model = new MIndirectCost();
        MProjectDepartment projectModel = new MProjectDepartment();
        private CurrentProjectInfo projectInfo;
        string detailExptr = "票据台账";
        DataSet ds = null;
        DataTable dt = null;
        DateTime startDate = new DateTime();
        DateTime endDate = new DateTime();
        public VAcceptanceBillReport()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }
        private void InitData()
        {
            dtpDateBegin.Value = new DateTime(ConstObject.TheLogin.LoginDate.Year, 1, 1);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;

            projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
            {

                this.lblPSelect.Visible = false;
                this.txtOperationOrg.Visible = false;
                this.btnOperationOrg.Visible = false;
            }
            this.fGridDetail.Rows = 1;
        }

        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
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
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            fGridDetail.ExportToExcel(detailExptr, false, false, true);
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            OperationOrgInfo info = txtOperationOrg.Tag as OperationOrgInfo;
            if (info == null && txtOperationOrg.Visible == true)
            {
                MessageBox.Show("[范围选择]不能为空，请选择！");
                return;
            }
            LoadTempleteFile(detailExptr + ".flx");

            //载入数据
            this.LoadDetailFile();

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
                if (modelName == "票据台账.flx")
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

        private void LoadDetailFile()
        {
            //OperationOrgInfo orgInfo = this.txtOperationOrg.Tag as OperationOrgInfo;
            FlashScreen.Show("正在生成[票据台账]...");

            OperationOrgInfo info = txtOperationOrg.Tag as OperationOrgInfo;
            try
            {
                startDate = dtpDateBegin.Value.Date;
                endDate=dtpDateEnd.Value.Date;
                if (info == null)
                {
                    ds = model.IndirectCostSvr.QueryAcceptanceBillReport(startDate, endDate, projectInfo.Id, "");
                }
                else
                {
                    string selProjectID = model.IndirectCostSvr.GetProjectIDByOperationOrg(info.Id);
                    if (ClientUtil.ToString(selProjectID) != "")
                    {
                        ds = model.IndirectCostSvr.QueryAcceptanceBillReport(startDate, endDate, selProjectID, "");
                    }
                    else
                    {
                        ds = model.IndirectCostSvr.QueryAcceptanceBillReport(startDate, endDate, "", info.SysCode);
                    }
                }
                dt = ds == null || ds.Tables.Count == 0 ? null : ds.Tables[0];
                LoadTotalFile();
            }
            catch (Exception e1)
            {
                throw new Exception("生成[票据台账]报告异常[" + e1.Message + "]");
            }
            finally
            {
                FlashScreen.Close();
            }

        }

        private void LoadTotalFile()
        {
            if (dt != null)
            {
                int iCount = dt.Rows.Count;
                decimal dSumGathering = 0, dSumPayment = 0;
                int iStart = 3;
                fGridDetail.AutoRedraw = false;
                fGridDetail.InsertRow(iStart, iCount);
                FlexCell.Range range = fGridDetail.Range(iStart, 1, iCount, fGridDetail.Cols - 1);
                CommonUtil.SetFlexGridDetailCenter(range);
                //fGridDetail.Column(8).Mask = FlexCell.MaskEnum.None;
                iCount = 0;
                int iRowIndex = 0;
                OperationOrgInfo info = txtOperationOrg.Tag as OperationOrgInfo;
                 if (info!=null)
                 {
                    this.fGridDetail.Cell(1, 1).Text = dtpDateBegin.Value.Date.ToShortDateString() +"到"+dtpDateEnd.Value.Date.ToShortDateString()+ " [" + info.Name + "] 票据台账";
                 }
                else
                 {
                    this.fGridDetail.Cell(1, 1).Text = dtpDateBegin.Value.Date.ToShortDateString() +"到"+dtpDateEnd.Value.Date.ToShortDateString()+ " [" + projectInfo.Name + "] 票据台账";
                 }
                 string billCharacter = "";
                foreach (DataRow dr in dt.Rows)
                {
                    iRowIndex = iStart + iCount;
                    fGridDetail.Cell(iRowIndex, 1).Text = (iCount + 1).ToString();      
                    fGridDetail.Cell(iRowIndex, 2).Text = ClientUtil.ToString(dr["ORGNAME"]);
                    fGridDetail.Cell(iRowIndex, 3).Text = ClientUtil.ToString(dr["PROJECTNAME"]);
                    if (string.IsNullOrEmpty(ClientUtil.ToString(dr["GATHERINGMXID"])) && !string.IsNullOrEmpty(ClientUtil.ToString(dr["PAYMENTMXID"])))
                    {
                        billCharacter = "付款"; dSumPayment += ClientUtil.ToDecimal(dr["SUMMONEY"]);
                    }
                    else if (!string.IsNullOrEmpty(ClientUtil.ToString(dr["GATHERINGMXID"])) && string.IsNullOrEmpty(ClientUtil.ToString(dr["PAYMENTMXID"])))
                    {
                        billCharacter = "收款"; dSumGathering += ClientUtil.ToDecimal(dr["SUMMONEY"]);
                    }
                    else
                    {
                        billCharacter = "异常";
                    }
                    fGridDetail.Cell(iRowIndex, 4).Text = billCharacter;
                    fGridDetail.Cell(iRowIndex, 5).Text = ClientUtil.ToString(dr["ACCEPTPERSON"]);
                    fGridDetail.Cell(iRowIndex, 6).Text = ClientUtil.ToString(dr["BILLTYPE"]);
                    fGridDetail.Cell(iRowIndex, 7).Text = ClientUtil.ToString(dr["SUMMONEY"]);
                    fGridDetail.Cell(iRowIndex, 8).Mask = FlexCell.MaskEnum.None;
                    fGridDetail.Cell(iRowIndex, 8).Text = ClientUtil.ToString(dr["BILLNO"]);
                    fGridDetail.Cell(iRowIndex, 9).Text = ClientUtil.ToDateTime(dr["CREATEDATE"]).ToShortDateString();
                    fGridDetail.Cell(iRowIndex, 10).Text = ClientUtil.ToDateTime(dr["EXPIREDATE"]).ToShortDateString();
                    iCount++;
                }
                fGridDetail.Column(3).AutoFit();
                fGridDetail.Column(5).AutoFit();
                fGridDetail.Column(7).AutoFit();
                fGridDetail.Column(8).AutoFit();
                fGridDetail.AutoRedraw = true;
                fGridDetail.Refresh();
                txtSumGathering.Text = dSumGathering.ToString("N2");
                txtSumPayment.Text = dSumPayment.ToString("N2");
                txtDiffere.Text = (dSumGathering - dSumPayment).ToString("N2");
               //txtDiffere.RightToLeft= txtSumPayment.RightToLeft=txtSumGathering.RightToLeft = RightToLeft.Yes;
            }

        }



    
    }
}
