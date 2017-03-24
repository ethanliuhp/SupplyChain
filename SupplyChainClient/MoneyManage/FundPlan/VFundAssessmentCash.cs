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
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using Application.Business.Erp.SupplyChain.Util;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using FlexCell;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Core;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan
{
    public partial class VFundAssessmentCash : TMasterDetailView
    {
        private const int REPORT_UNIT = 10000;
        private CurrentProjectInfo currentProject;
        private MFinanceMultData mOperate;
        private FundAssessmentMaster currentMaster;
        private Color editColor;

        public VFundAssessmentCash()
        {
            InitializeComponent();

            InitEvents();

            InitData();
        }

        private void InitData()
        {
            currentProject = StaticMethod.GetProjectInfo();
            if (currentProject != null && !currentProject.Code.Equals(CommonUtil.CompanyProjectCode))
            {
                txtOperationOrg.Text = ConstObject.TheOperationOrg.Name;
                txtOperationOrg.Tag = ConstObject.TheOperationOrg;
                btnOperationOrg.Visible = false;
            }
            else
            {
                currentProject = null;
            }

            mOperate = new MFinanceMultData();
            editColor = ColorTranslator.FromHtml("#D2FBD6");

            InitComboBox();

            InitGrid();
        }

        private void InitComboBox()
        {
            var startYear = 2006;
            var endYear = DateTime.Now.Year;
            var yearList = new List<int>();
            for (int i = endYear; i >= startYear; i--)
            {
                yearList.Add(i);
            }
            cmbYear.DataSource = yearList;

            var monList = new List<int>();
            for (var i = 1; i <= 12; i++)
            {
                monList.Add(i);
            }
            cmbMonth.DataSource = monList;
            cmbMonth.SelectedItem = DateTime.Now.Month;
        }

        private void InitGrid()
        {
            FundPlanOperate.LoadFlexFile(string.Concat(gdAssesscashDetail.Tag, ".flx"), gdAssesscashDetail);

            gdAssesscashDetail.DefaultRowHeight = 25;
            SetGridLocked(gdAssesscashDetail, 12);
        }

        private void InitMaster()
        {
            currentMaster = new FundAssessmentMaster();
            currentMaster.CreateDate = DateTime.Now;
            currentMaster.CreateMonth = TransUtil.ToInt(cmbMonth.Text);
            currentMaster.CreatePerson = ConstObject.TheLogin.ThePerson;
            currentMaster.CreatePersonName = currentMaster.CreatePerson.Name;
            currentMaster.CreateYear = TransUtil.ToInt(cmbYear.Text);
            currentMaster.QueryDate = new DateTime(currentMaster.CreateYear, currentMaster.CreateMonth, 1);
            currentMaster.DocState = DocumentState.Edit;
            currentMaster.OperOrgInfo = ConstObject.TheOperationOrg;
            currentMaster.OperOrgInfoName = currentMaster.OperOrgInfo.Name;
            currentMaster.OpgSysCode = currentMaster.OperOrgInfo.SysCode;
            if (currentProject != null)
            {
                SetProjectInfo();
            }
        }

        private void SetProjectInfo()
        {
            if (currentMaster == null)
            {
                return;
            }
            currentMaster.ProjectId = currentProject.Id;
            currentMaster.ProjectName = currentProject.Name;
            currentMaster.GatheringRate = currentProject.ContractCollectRatio / 100m;
            if (currentProject.ProjectCurrState == 2)
            {
                if ((DateTime.Now - currentProject.LastUpdateDate).Days > 180)
                {
                    currentMaster.ProjectState = "完工6个月以上未办结算";
                }
                else
                {
                    currentMaster.ProjectState = "完工6月内未办理结算";
                }
            }
            else
            {
                currentMaster.ProjectState = Enum.GetName(typeof(EnumProjectCurrState), currentProject.ProjectCurrState);
            }
        }

        private void SetGridLocked(CustomFlexGrid grid, int unLockedRow)
        {
            for (int i = 1; i < grid.Rows - 1 ; i++)
            {
                grid.Row(i).Locked = i != unLockedRow;
            }
        }

        private void AfterYearMonthChanged()
        {
            var ym = string.Format("计算期间：{0}年{1}月", cmbYear.Text, cmbMonth.Text);
            gdAssesscashDetail.Cell(2, 1).Text = ym;
            gdAssesscashDetail.Cell(5, 2).Text = string.Format("{0}月", cmbMonth.Text);

            if (currentMaster != null)
            {
                currentMaster.CreateYear = TransUtil.ToInt(cmbYear.Text);
                currentMaster.CreateMonth = TransUtil.ToInt(cmbMonth.Text);
                currentMaster.QueryDate = new DateTime(currentMaster.CreateYear, currentMaster.CreateMonth, 1);
            }
        }

        private bool SaveOrSubmit(int opType)
        {
            if (currentMaster == null || currentMaster.AssessCashDetails.Count == 0)
            {
                return false;
            }

            currentMaster.Temp1 = "考核兑现";
            currentMaster = mOperate.FinanceMultDataSrv.SaveFundAssessment(currentMaster);

            this.ViewCaption = ViewName + "-" + currentMaster.Code;
            txtCode.Text = currentMaster.Code;
            txtDocState.Text = ClientUtil.GetDocStateName(currentMaster.DocState);
            MessageBox.Show(string.Format("{0}成功", opType == 1 ? "保存" : "提交"));

            return true;
        }

        private void InitEvents()
        {
            btnOperationOrg.Click += btnOperationOrg_Click;
            btnGetData.Click += new EventHandler(btnGetData_Click);
            pnlBody.SizeChanged += new EventHandler(pnlBody_SizeChanged);

            cmbYear.SelectedIndexChanged += new EventHandler(cmbYear_SelectedIndexChanged);
            cmbMonth.SelectedIndexChanged += new EventHandler(cmbMonth_SelectedIndexChanged);

            gdAssesscashDetail.CellChange += new FlexCell.Grid.CellChangeEventHandler(gdAssesscashDetail_CellChange);
        }

        private void gdAssesscashDetail_CellChange(object sender, Grid.CellChangeEventArgs e)
        {
            if (currentMaster == null || e.Row != 12)
            {
                return;
            }

            var val = 0m;
            decimal.TryParse(gdAssesscashDetail.Cell(e.Row, e.Col).Text.Trim(), out val);
            var aDetail = currentMaster.AssessCashDetails.FirstOrDefault();
            if (aDetail == null)
            {
                return;
            }

            aDetail.OtherAdjust = val*REPORT_UNIT;
            aDetail.RealCashBalance = currentMaster.CurrentCashBalance
                                      - (aDetail.CentralPurchase + aDetail.InnerInstall
                                         + aDetail.OtherContractPay +
                                         aDetail.OtherAdjust);
            aDetail.AssessCardinal = currentMaster.CurrentCashBalance - currentMaster.CurrentSchemeTarget
                                     - aDetail.CentralPurchase - aDetail.InnerInstall
                                     - aDetail.OtherContractPay * 0.5m - aDetail.OtherAdjust;
            if (aDetail.AssessCardinal <= 0)
            {
                aDetail.CashMoney = aDetail.AssessCardinal * 0.0001m;
            }
            else if (aDetail.AssessCardinal < 500)
            {
                aDetail.CashMoney = aDetail.AssessCardinal * 0.0008m;
            }
            else if (aDetail.AssessCardinal < 1000)
            {
                aDetail.CashMoney = aDetail.AssessCardinal * 0.001m;
            }
            else if (aDetail.AssessCardinal < 2000)
            {
                aDetail.CashMoney = aDetail.AssessCardinal * 0.0012m;
            }
            else if (aDetail.AssessCardinal < 5000)
            {
                aDetail.CashMoney = aDetail.AssessCardinal * 0.00125m;
            }
            else if (aDetail.AssessCardinal < 10000)
            {
                aDetail.CashMoney = aDetail.AssessCardinal * 0.0013m;
            }
            else
            {
                aDetail.CashMoney = aDetail.AssessCardinal * 0.00135m;
            }
            aDetail.DeductionItem = aDetail.AssessCardinal <= 0
                                        ? 0
                                        : aDetail.CashMoney * aDetail.WarnRate > aDetail.ApprovalDeduction
                                              ? aDetail.WarnRate
                                              : aDetail.ApprovalDeduction;
            aDetail.AssessCashMoney = aDetail.AssessCardinal - aDetail.DeductionItem;

            DisplayAssesscashDetail(false);
        }

        private void pnlBody_SizeChanged(object sender, EventArgs e)
        {
            var wid = (pnlBody.Width - 24)/2;
            gdAssesscashDetail.Width = wid + 100;

            var pos = gdAssesscashDetail.Location;
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            if (currentMaster == null)
            {
                InitMaster();
            }

            currentMaster = mOperate.FinanceMultDataSrv.GetFundAssessmentData(currentMaster, 2);

            ModelToView();
        }

        private void btnOperationOrg_Click(object sender, EventArgs e)
        {
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect(true);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                var selOrg = frm.Result[0] as OperationOrgInfo;
                txtOperationOrg.Tag = selOrg;
                txtOperationOrg.Text = selOrg.Name;

                currentProject = mOperate.CurrentProjectSrv.GetProjectInfoByOwnOrg(selOrg.Id);
                if (currentProject != null)
                {
                    SetProjectInfo();
                }
            }
        }

        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            AfterYearMonthChanged();
        }

        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            AfterYearMonthChanged();
        }

        public void Start(string Id)
        {
            try
            {
                if (Id == "空")
                {
                    RefreshState(MainViewState.Initialize);
                }
                else
                {
                    currentMaster = mOperate.FinanceMultDataSrv.GetFundAssessmentById(Id);

                    InitGrid();

                    if (!ModelToView())
                    {
                        MessageBox.Show("加载数据失败");
                    }

                    RefreshState(MainViewState.Browser);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("视图启动出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        public override void RefreshState(MainViewState state)
        {
            base.RefreshState(state);
            //控制表格
            switch (state)
            {
                case MainViewState.AddNew:
                case MainViewState.Modify:
                    cmbYear.Enabled = cmbMonth.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    cmbYear.Enabled = cmbMonth.Enabled = false;
                    break;
                default:
                    break;
            }
        }

        public override void RefreshControls(MainViewState state)
        {
            base.RefreshControls(state);

            //控制自身控件
            if (ViewState == MainViewState.AddNew || ViewState == MainViewState.Modify)
            {
                ObjectLock.Unlock(pnlFloor, true);
            }
            else
            {
                ObjectLock.Lock(pnlFloor, true);
            }

            //永久锁定
            object[] os = new object[] {txtCode, txtCreateBy, txtCreateTime, txtDocState, txtOperationOrg};
            ObjectLock.Lock(os);
        }

        public override bool NewView()
        {
            base.NewView();

            ClearView();

            InitMaster();

            ModelToView();

            return true;
        }

        public override bool ModifyView()
        {
            if (currentMaster.DocState == DocumentState.Edit || currentMaster.DocState == DocumentState.Valid)
            {
                base.ModifyView();

                return true;
            }

            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(currentMaster.DocState));
            MessageBox.Show(message);

            return false;
        }

        public override bool SaveView()
        {
            return SaveOrSubmit(1);
        }

        public override bool SubmitView()
        {
            return SaveOrSubmit(2);
        }

        public override bool DeleteView()
        {
            if (currentMaster.DocState == DocumentState.Valid || currentMaster.DocState == DocumentState.Edit)
            {
                if (!mOperate.FinanceMultDataSrv.Delete(currentMaster)) return false;

                LogData log = new LogData();
                log.BillId = currentMaster.Id;
                log.BillType = "资金考核";
                log.Code = currentMaster.Code;
                log.OperType = "删除";
                log.Descript = "";
                log.OperPerson = ConstObject.LoginPersonInfo.Name;
                log.ProjectName = currentMaster.ProjectName;
                StaticMethod.InsertLogData(log);

                ClearView();

                MessageBox.Show("删除成功！");
                return true;
            }

            MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(currentMaster.DocState) + "】，不能删除！");
            return false;
        }

        public override void RefreshView()
        {
            ModelToView();
        }

        public override bool CancelView()
        {
            base.CancelView();
            return true;
        }

        private void DisplayAssesscashDetail(bool isShowOtherAdjust)
        {
            var rIndex = 3;
            gdAssesscashDetail.Cell(rIndex++, 2).Text = currentMaster.ProjectName;
            gdAssesscashDetail.Cell(rIndex++, 2).Text = currentMaster.ProjectState;
            gdAssesscashDetail.Cell(rIndex++, 2).Text = string.Format("{0}月", currentMaster.CreateMonth);
            gdAssesscashDetail.Cell(rIndex++, 2).Text = (currentMaster.CurrentSchemeTarget / REPORT_UNIT).ToString("N4");
            gdAssesscashDetail.Cell(rIndex++, 2).Text = (currentMaster.CurrentCashBalance / REPORT_UNIT).ToString("N4");

            var assDetail = currentMaster.AssessCashDetails.FirstOrDefault();
            if (assDetail != null)
            {
                gdAssesscashDetail.Cell(rIndex++, 2).Text =
                    ((assDetail.CentralPurchase + assDetail.InnerInstall
                      + assDetail.OtherContractPay + assDetail.OtherAdjust)/REPORT_UNIT).ToString("N4");
                gdAssesscashDetail.Cell(rIndex++, 2).Text = (assDetail.CentralPurchase / REPORT_UNIT).ToString("N4");
                gdAssesscashDetail.Cell(rIndex++, 2).Text = (assDetail.InnerInstall / REPORT_UNIT).ToString("N4");
                gdAssesscashDetail.Cell(rIndex++, 2).Text = (assDetail.OtherContractPay / REPORT_UNIT).ToString("N4");
                if (isShowOtherAdjust)
                {
                    var otherJustCell = gdAssesscashDetail.Cell(rIndex, 2);
                    otherJustCell.Text = (assDetail.OtherAdjust/REPORT_UNIT).ToString("N4");
                    otherJustCell.BackColor = editColor;
                    otherJustCell.Locked = false;
                }
                rIndex++;
                gdAssesscashDetail.Cell(rIndex++, 2).Text = (assDetail.RealCashBalance / REPORT_UNIT).ToString("N4");
                gdAssesscashDetail.Cell(rIndex++, 2).Text = (assDetail.AssessCardinal / REPORT_UNIT).ToString("N4");
                gdAssesscashDetail.Cell(rIndex++, 2).Text = (assDetail.CashMoney / REPORT_UNIT).ToString("N4");
                gdAssesscashDetail.Cell(rIndex++, 2).Text = (assDetail.DeductionItem / REPORT_UNIT).ToString("N4");
                gdAssesscashDetail.Cell(rIndex++, 2).Text = assDetail.WarnRate.ToString("P2");
                gdAssesscashDetail.Cell(rIndex++, 2).Text = assDetail.WarnLevel;
                gdAssesscashDetail.Cell(rIndex++, 2).Text = assDetail.ApprovalDeduction.ToString("P2");
                gdAssesscashDetail.Cell(rIndex++, 2).Text = assDetail.ApprovalRate.ToString("P2");
                gdAssesscashDetail.Cell(rIndex, 2).Text = (assDetail.AssessCashMoney / REPORT_UNIT).ToString("N4");
            }
        }

        private bool ModelToView()
        {
            if (currentMaster == null)
            {
                return false;
            }

            //主表
            cmbYear.Text = currentMaster.CreateYear.ToString();
            cmbMonth.Text = currentMaster.CreateMonth.ToString();
            txtCode.Text = currentMaster.Code;
            txtCreateBy.Text = currentMaster.CreatePersonName;
            txtCreateTime.Text = currentMaster.CreateDate.ToString();
            txtDocState.Text = ClientUtil.GetDocStateName(currentMaster.DocState);
            txtOperationOrg.Text = currentMaster.ProjectName;

            AfterYearMonthChanged();

            #region 资金考核兑现

            DisplayAssesscashDetail(true);

            #endregion

            return true;
        }

        private void ClearView()
        {
            ClearControl(pnlFloor);

            InitGrid();

            txtDocState.Text = ClientUtil.GetDocStateName(DocumentState.Edit);
        }

        private void ClearControl(Control c)
        {
            foreach (Control cd in c.Controls)
            {
                ClearControl(cd);
            }
            //自定义控件清空
            if (c is CustomEdit)
            {
                c.Tag = null;
                c.Text = "";
            }
            else if (c is CustomDataGridView)
            {
                (c as CustomDataGridView).Rows.Clear();
            }
        }
    }
}
