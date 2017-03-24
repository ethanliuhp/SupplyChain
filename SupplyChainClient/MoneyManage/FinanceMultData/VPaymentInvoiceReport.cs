using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Component.Util;
namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VPaymentInvoiceReport : TBasicDataView
    {
        private string rptName = "付款发票台账";
        private readonly MFinanceMultData mOperate = new MFinanceMultData();
        private List<OperationOrgInfo> subCompanys = null;           

        public VPaymentInvoiceReport()
        {
            InitializeComponent();

            InitEvents();

            InitData();
        }

        private void InitData()
        {
            datePeriodPicker1.BeginValue = DateTime.Now.AddDays(-7).Date;
            datePeriodPicker1.EndValue = DateTime.Now.Date;

            CurrentProjectInfo proj = StaticMethod.GetProjectInfo();
            if (proj != null && !proj.Code.Equals(CommonUtil.CompanyProjectCode))
            {
                txtOperationOrg.Text = proj.Name;
                txtOperationOrg.Tag = proj;
                btnOperationOrg.Visible = false;

                LoadTempleteFile(rptName + "-项目.flx");
            }
        }

        private void InitEvents()
        {
            btnExcel.Click += btnExcel_Click;
            btnOperationOrg.Click += btnOperationOrg_Click;
            btnQuery.Click += btnQuery_Click;
        }

        private void LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                gdResult.OpenFile(path + "\\" + modelName);//载入格式
            }
            else
            {
                MessageBox.Show("未找到模板格式文件" + modelName);
            }
        }

        private void ShowQueryResult(List<DataDomain> rList, int offSet)
        {
            if (rList == null)
            {
                return;
            }

            var rowCount = rList.Count;
            var colCount = gdResult.Cols;
            var startRow = 3;

            gdResult.AutoRedraw = false;
            gdResult.InsertRow(startRow, rowCount);

            FlexCell.Range oRange = gdResult.Range(startRow, 1, startRow + rowCount, colCount - 1);
            CommonUtil.SetFlexGridDetailCenter(oRange);

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 1; j < colCount; j++)
                {
                    var pvIndex = j;
                    if (offSet > 0 && j > 3)
                    {
                        pvIndex = j + offSet;
                    }
                    var vl = rList[i].GetType().GetProperty(string.Concat("Name", pvIndex)).GetValue(rList[i], null);

                    gdResult.Cell(startRow + i, j).Text = vl == null ? "" : vl.ToString();
                }
            }
            gdResult.Cell(startRow + rowCount, 2).Text = "合计";
            gdResult.Cell(startRow + rowCount, colCount - 1).Text =
                rList.Sum(r => ClientUtil.ToDecimal(r.Name9)).ToString();
            gdResult.Cell(startRow + rowCount, colCount - 2).Text =
                rList.Sum(r => ClientUtil.ToDecimal(r.Name8)).ToString();
            gdResult.Cell(startRow + rowCount, colCount - 3).Text =
                rList.Sum(r => ClientUtil.ToDecimal(r.Name7)).ToString();
            gdResult.Cell(startRow + rowCount, colCount - 4).Text =
                rList.Sum(r => ClientUtil.ToDecimal(r.Name6)).ToString();

            gdResult.AutoRedraw = true;
            gdResult.Refresh();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                string projId = string.Empty;
                string orgSysCode = string.Empty;
                var isSubCompany = false;                   
                if (!btnOperationOrg.Visible)
                {
                    var proj = txtOperationOrg.Tag as CurrentProjectInfo;
                    if (proj != null)
                    {
                        projId = proj.Id;
                    }
                }
                else
                {
                    var org = txtOperationOrg.Tag as OperationOrgInfo;
                    if (org == null)
                    {
                        MessageBox.Show("请选择查询范围");
                        return;
                    }

                    projId = mOperate.IndirectCostSvr.GetProjectIDByOperationOrg(org.Id);
                    if (string.IsNullOrEmpty(projId))
                    {
                        orgSysCode = org.SysCode;
                    }

                    if (subCompanys == null)
                    {
                        subCompanys =
                            mOperate.FinanceMultDataSrv.QuerySubAndCompanyOrgInfo().OfType<OperationOrgInfo>().ToList();
                    }
                    isSubCompany = subCompanys != null && subCompanys.Exists(s => orgSysCode.StartsWith(s.SysCode));
                }

                FlashScreen.Show("正在查询信息......");
                var list = mOperate.FinanceMultDataSrv.GetPaymentInvoiceReport(datePeriodPicker1.BeginValue,
                                                                               datePeriodPicker1.EndValue.AddDays(1),
                                                                               projId, orgSysCode, isSubCompany);

                if (!string.IsNullOrEmpty(projId))
                {
                    rptName = "付款发票台账-项目";
                    LoadTempleteFile(rptName + ".flx");
                    ShowQueryResult(list, 0);
                }
                else if (isSubCompany)
                {
                    rptName = "付款发票台账-分公司";
                    LoadTempleteFile(rptName + ".flx");
                    ShowQueryResult(list, 2);
                }else
                {
                    rptName = "付款发票台账-公司";
                    LoadTempleteFile(rptName + ".flx");
                    ShowQueryResult(list, 2);
                }
                gdResult.Cell(1, 1).Text = string.Format("{1}-{2}{0}付款发票台账"
                                             , txtOperationOrg.Text
                                             , datePeriodPicker1.BeginValue.ToString("yyyy年MM月dd日")
                                             , datePeriodPicker1.EndValue.ToString("yyyy年MM月dd日"));

                FlashScreen.Close();
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show(string.Format("查询失败：{0}", ex.Message));
            }
        }

        private void btnOperationOrg_Click(object sender, EventArgs e)
        {
            var frm = new VCommonOperationOrgSelect(true);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                var info = frm.Result[0] as OperationOrgInfo;
                txtOperationOrg.Tag = info;
                txtOperationOrg.Text = info.Name;
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            gdResult.ExportToExcel(rptName, false, false, true);
        }
    }
}
