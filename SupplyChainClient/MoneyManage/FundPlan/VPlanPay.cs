﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using FlexCell;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Core;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using IRPServiceModel.Domain.Document;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.Client.MaterialRentalMange;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan
{
    public partial class VPlanPay : TMasterDetailView
    {
        private MMatRentalMng model = new MMatRentalMng();
        private CurrentProjectInfo currentProject;
        private ProjectFundPlanDetail selectPlan;
        private PaymentMaster curBillMaster;
        private PaymentDetail curBillDetail;
        private AccountTitleTree selectAccount;
        private DataDomain selectSupplier;
        private PaymentMaster selectPayment;
        private Color editColor;
        private MFinanceMultData mOperate;
        private Dictionary<string, AcceptanceBill> selectBills;
        private bool isProjectOrg = true;

        public PaymentMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }
        /// <summary>
        /// 当前明细单据
        /// </summary>
        public PaymentDetail CurBillDetail
        {
            get { return curBillDetail; }
            set { curBillDetail = value; }
        }

        public VPlanPay()
        {
            InitializeComponent();

            InitData();

            InitGrid(gdPlanPay);

            InitGrid(gdPlanPay2);

            InitEvents();
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
                    selectPayment = mOperate.FinanceMultDataSrv.GetPaymentMasterById(Id);

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
                    gdPlanPay.Locked = false;
                    gdPlanPay2.Locked = false;

                    InitHeader();

                    InitHeader2();

                    SetButtonsEnable(true);
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    gdPlanPay.Locked = true;
                    gdPlanPay2.Locked = true;
                    SetButtonsEnable(false);
                    break;
                default:
                    break;
            }
        }

        public override bool NewView()
        {
            base.NewView();

            ClearView();

            return true;
        }

        public override bool ModifyView()
        {
            if (selectPayment.DocState == DocumentState.Edit || selectPayment.DocState == DocumentState.Valid)
            {
                base.ModifyView();

                return true;
            }

            var message = string.Format("此单状态为【{0}】，不能修改！", ClientUtil.GetDocStateName(selectPayment.DocState));
            MessageBox.Show(message);
            return false;
        }

        public override bool DeleteView()
        {
            try
            {
                selectPayment = mOperate.FinanceMultDataSrv.GetPaymentMasterById(selectPayment.Id);
                if (selectPayment.DocState == DocumentState.Valid || selectPayment.DocState == DocumentState.Edit)
                {
                    if (!mOperate.FinanceMultDataSrv.DeletePaymentMaster(selectPayment)) return false;

                    LogData log = new LogData();
                    log.BillId = selectPayment.Id;
                    log.BillType = "资金支付单";
                    log.Code = selectPayment.Code;
                    log.OperType = "删除";
                    log.Descript = "";
                    log.OperPerson = ConstObject.LoginPersonInfo.Name;
                    log.ProjectName = selectPayment.ProjectName;
                    StaticMethod.InsertLogData(log);

                    ClearView();

                    MessageBox.Show("删除成功！");
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(selectPayment.DocState) + "】，不能删除！");
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据删除错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        public override bool SaveView()
        {
            base.SaveView();

            return SaveOrSubmit(1);
        }

        public override bool SubmitView()
        {
            base.SubmitView();

            return SaveOrSubmit(2);
        }

        public override bool CancelView()
        {
            base.CancelView();
            return true;
        }

        public override void RefreshView()
        {
            try
            {
                //重新获得当前对象的值
                selectPayment = mOperate.FinanceMultDataSrv.GetPaymentMasterById(selectPayment.Id);

                //给界面赋值
                ModelToView();
            }
            catch (Exception e)
            {
                MessageBox.Show("数据刷新错误：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        private bool ValidateModel()
        {
            if (selectPayment == null)
            {
                return false;
            }

            var isSimple = tabControl1.SelectedTab == tabPage2;

            if (currentProject == null)
            {
                MessageBox.Show("请选择项目");
                return false;
            }

            if (!isSimple && string.IsNullOrEmpty(selectPayment.FundPlanCode))
            {
                MessageBox.Show("请选择关联的资金计划");
                return false;
            }

            if (string.IsNullOrEmpty(selectPayment.TheCustomerRelationInfo)
                && string.IsNullOrEmpty(selectPayment.TheSupplierRelationInfo))
            {
                MessageBox.Show("请选择收款单位");
                return false;
            }

            if (string.IsNullOrEmpty(selectPayment.BankAccountNo))
            {
                MessageBox.Show("请输入银行账号");
                return false;
            }

            if (string.IsNullOrEmpty(selectPayment.BankName))
            {
                MessageBox.Show("请输入开户行");
                return false;
            }

            if (!isSimple && string.IsNullOrEmpty(selectPayment.AccountTitleID))
            {
                MessageBox.Show("请选择支付用途");
                return false;
            }
            else if (isSimple && string.IsNullOrEmpty(selectPayment.AccountTitleName))
            {
                MessageBox.Show("请输入支付用途");
                return false;
            }

            if (selectPayment.SumMoney == 0)
            {
                MessageBox.Show("请输入支付金额");
                return false;
            }

            if (selectPayment.Details.Count == 0)
            {
                MessageBox.Show("支付明细不能为空");
                return false;
            }

            if (selectPayment.Details.OfType<PaymentDetail>().Any(
                    a => a.Descript.Contains("汇票") && a.AcceptBillID == null))
            {
                MessageBox.Show("请选择票据");
                return false;
            }

            var bills = selectPayment.Details.OfType<PaymentDetail>().ToList()
                .FindAll(a => a.Descript.Contains("汇票") && a.AcceptBillID != null)
                .GroupBy(b => b.AcceptBillID.Id)
                .Select(a => new { BillId = a.Key, Counter = a.Count() });
            if (bills.Any(a => a.Counter > 1))
            {
                MessageBox.Show("选择的票据不允许重复，请重新选择");
                return false;
            }

            if (!isSimple)
            {
                var hasPay = Convert.ToDecimal(gdPlanPay.Cell(11, 4).Tag);
                if ((hasPay + selectPayment.SumMoney) > selectPayment.FundPlan.Quota)
                {
                    MessageBox.Show(string.Format("本次支付金额不能大于{0}，请重新输入支付金额", selectPayment.FundPlan.Quota - hasPay));
                    return false;
                }
            }
            else if (selectPayment.RefundDate == DateTime.MinValue)
            {
                MessageBox.Show("请选择报销（还款）时间");
                return false;
            }

            //检查资金是否结账
            if (!CheckAccountLock(selectPayment.CreateDate))
            {
                return false;
            }

            return true;
        }

        private void ViewToModel(CustomFlexGrid grid)
        {
            if (selectPayment == null)
            {
                selectPayment = new PaymentMaster();
                selectPayment.CreatePerson = ConstObject.LoginPersonInfo;
                selectPayment.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                var dt = Convert.ToDateTime(grid.Cell(3, 5).Text);
                selectPayment.CreateDate = dt;
                selectPayment.CreateYear = dt.Year;
                selectPayment.CreateMonth = dt.Month;
                selectPayment.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                selectPayment.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                selectPayment.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                selectPayment.HandOrgLevel = ConstObject.TheOperationOrg.Level;
                selectPayment.HandlePerson = ConstObject.LoginPersonInfo;
                selectPayment.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                selectPayment.DocState = DocumentState.Edit;
                selectPayment.Temp1 = "资金支付";
            }

            if (currentProject != null)
            {
                selectPayment.ProjectId = currentProject.Id;
                selectPayment.ProjectName = currentProject.Name;
            }

            if (selectPlan != null)
            {
                selectPayment.FundPlan = selectPlan;
                selectPayment.FundPlanCode = selectPlan.Master.Code;
                selectPayment.AddBalMoney = selectPlan.CumulativeSettlement;
                selectPayment.AddPayMoney = selectPlan.CumulativePayment;
            }

            if (selectAccount != null)
            {
                selectPayment.AccountTitleCode = selectAccount.Code;
                selectPayment.AccountTitleID = selectAccount.Id;
                selectPayment.AccountTitleName = selectAccount.Name;
                selectPayment.AccountTitleSyscode = selectAccount.SysCode;
            }

            if (selectSupplier != null)
            {
                if (ClientUtil.ToInt(selectSupplier.Name1) == 1)
                {
                    selectPayment.TheSupplierRelationInfo = ClientUtil.ToString(selectSupplier.Name2);
                    selectPayment.TheSupplierName = ClientUtil.ToString(selectSupplier.Name4);
                    selectPayment.TheCustomerRelationInfo = "";
                    selectPayment.TheCustomerName = "";
                }
                else
                {
                    selectPayment.TheCustomerRelationInfo = ClientUtil.ToString(selectSupplier.Name2);
                    selectPayment.TheCustomerName = ClientUtil.ToString(selectSupplier.Name4);
                    selectPayment.TheSupplierRelationInfo = "";
                    selectPayment.TheSupplierName = "";
                }

                selectPayment.BankAddress = ClientUtil.ToString(selectSupplier.Name7);
            }

            selectPayment.BankAccountNo = grid.Cell(5, 4).Text;
            selectPayment.BankName = grid.Cell(6, 4).Text;

            var isSimple = tabControl1.SelectedTab == tabPage2;
            if (isSimple)
            {
                selectPayment.AccountTitleName = grid.Cell(4, 6).Text;
                DateTime tmpDt = DateTime.MinValue;
                DateTime.TryParse(grid.Cell(6, 6).Text, out tmpDt);
                selectPayment.RefundDate = tmpDt;
                selectPayment.SumMoney = Convert.ToDecimal(grid.Cell(8, 4).Tag);
            }
            else
            {
                selectPayment.OriginalCollectionUnit = grid.Cell(4, 6).Text;
                selectPayment.PaymentClause = grid.Cell(6, 6).Text;
                selectPayment.AddInvoiceMoney = Convert.ToDecimal(grid.Cell(9, 6).DoubleValue);
                selectPayment.SumMoney = Convert.ToDecimal(grid.Cell(13, 4).Tag);
            }

            //明细
            selectPayment.Details.Clear();
            var startRow = isSimple ? 10 : 15;
            for (int i = startRow; i < grid.Rows - 1; i++)
            {
                var detail = new PaymentDetail();
                detail.Master = selectPayment;
                var id = grid.Cell(i, 1).Tag;
                if (!string.IsNullOrEmpty(id))
                {
                    detail.Id = id;
                }

                detail.Descript = grid.Cell(i, 2).Text;
                if (string.IsNullOrEmpty(detail.Descript))
                {
                    continue;
                }

                var acpBillId = grid.Cell(i, 4).Tag;
                if (string.IsNullOrEmpty(acpBillId))
                {
                    detail.Money = detail.PaymentMoney = Convert.ToDecimal(grid.Cell(i, 3).DoubleValue);
                }
                else
                {
                    detail.AcceptBillID = selectBills[acpBillId];
                    detail.Money = detail.PaymentMoney = Convert.ToDecimal(grid.Cell(i, 6).DoubleValue);
                }

                selectPayment.Details.Add(detail);
            }
        }

        private bool ModelToView()
        {
            if (selectPayment == null)
            {
                return false;
            }

            selectBills.Clear();

            var isSimple = string.IsNullOrEmpty(selectPayment.FundPlanCode);
            var grid = isSimple ? gdPlanPay2 : gdPlanPay;
            tabControl1.SelectedTab = isSimple ? tabPage2 : tabPage1;

            InitGrid(grid);

            #region 项目
            currentProject = mOperate.CurrentProjectSrv.GetProjectById(selectPayment.ProjectId);
            DisplayProjectInfo(grid);

            #endregion

            #region 收款单位
            selectSupplier = new DataDomain();
            if (string.IsNullOrEmpty(selectPayment.TheSupplierRelationInfo))
            {
                selectSupplier.Name1 = 2;
                selectSupplier.Name2 = selectPayment.TheCustomerRelationInfo;
                selectSupplier.Name4 = selectPayment.TheCustomerName;
            }
            else
            {
                selectSupplier.Name1 = 1;
                selectSupplier.Name2 = selectPayment.TheSupplierRelationInfo;
                selectSupplier.Name4 = selectPayment.TheSupplierName;
            }

            selectSupplier.Name5 = selectPayment.BankAccountNo;
            selectSupplier.Name6 = selectPayment.BankName;
            selectSupplier.Name7 = selectPayment.BankAddress;

            DisplaySupplierInfo(grid);
            #endregion

            #region 资金计划

            selectPlan = selectPayment.FundPlan;
            DisPlayPlanInfo();

            #endregion

            #region 支付用途
            selectAccount = new AccountTitleTree();
            selectAccount.Id = selectPayment.AccountTitleID;
            selectAccount.Code = selectPayment.AccountTitleCode;
            selectAccount.Name = selectPayment.AccountTitleName;
            selectAccount.SysCode = selectPayment.AccountTitleSyscode;

            if (isSimple)
            {
                grid.Cell(4, 6).Text = selectAccount.Name;
            }
            else
            {
                DisplayAccountInfo();
            }

            #endregion

            #region  其他信息

            grid.Cell(3, 5).Text = selectPayment.CreateDate.ToString("yyyy-MM-dd");
            grid.Cell(grid.Rows - 1, 4).Text = selectPayment.CreatePersonName;
            grid.Cell(grid.Rows - 1, 6).Text = selectPayment.CreateDate.ToString();

            if (!isSimple)
            {
                grid.Cell(4, 6).Text = selectPayment.OriginalCollectionUnit;
                grid.Cell(6, 6).Text = selectPayment.PaymentClause;
            }
            else
            {
                grid.Cell(6, 6).Text = selectPayment.RefundDate.ToString("yyyy-MM-dd");
            }

            #endregion

            #region 明细
            var rIndex = 0;
            var startRow = isSimple ? 9 : 14;
            foreach (PaymentDetail pay in selectPayment.Details)
            {
                rIndex++;

                AddNewDetailRow(grid);

                grid.Cell(startRow + rIndex, 1).Text = rIndex.ToString();
                grid.Cell(startRow + rIndex, 1).Tag = pay.Id;
                grid.Cell(startRow + rIndex, 2).Text = pay.Descript;

                SetPayDetailType(startRow + rIndex, 2, grid);
                if (pay.AcceptBillID != null)
                {
                    if (!selectBills.ContainsKey(pay.AcceptBillID.Id))
                    {
                        selectBills.Add(pay.AcceptBillID.Id, pay.AcceptBillID);
                    }
                    grid.Cell(startRow + rIndex, 3).Text = "票据号码";
                    grid.Cell(startRow + rIndex, 4).Tag = pay.AcceptBillID.Id;
                    grid.Cell(startRow + rIndex, 4).Text = pay.AcceptBillID.BillNo;
                    grid.Cell(startRow + rIndex, 5).Text = "票据金额";
                    grid.Cell(startRow + rIndex, 6).Text = pay.AcceptBillID.SumMoney.ToString("N2");
                }
                else
                {
                    grid.Cell(startRow + rIndex, 3).Text = pay.Money.ToString("N2");
                }
            }

            #endregion

            ComputeThisPay(grid);

            if (!isSimple)
            {
                ComputePayRate();
            }

            return true;
        }

        private void ClearView()
        {
            selectAccount = null;
            selectPlan = null;
            selectBills = new Dictionary<string, AcceptanceBill>();
            selectSupplier = null;
            selectPayment = null;

            InitGrid(gdPlanPay);

            InitGrid(gdPlanPay2);

            SetButtonsEnable(false);
        }

        private void SetButtonsEnable(bool bl)
        {
            btnDeleteDocumentMaster.Enabled = btnDeleteDocumentFile.Enabled = bl;
            btnDocumentFileAdd.Enabled = btnDocumentFileUpdate.Enabled = bl;

            btnDownLoadDocument.Enabled = btnOpenDocument.Enabled = bl;
            btnUpFile.Enabled = btnUpdateDocument.Enabled = bl;
        }

        private bool SaveOrSubmit(int opType)
        {
            try
            {
                ViewToModel(tabControl1.SelectedTab == tabPage1 ? gdPlanPay : gdPlanPay2);

                if (!ValidateModel())
                {
                    return false;
                }

                selectPayment.DocState = opType == 1 ? DocumentState.Edit : DocumentState.InAudit;

                selectPayment = mOperate.FinanceMultDataSrv.SavePaymentMaster(selectPayment);

                this.ViewCaption = ViewName + "-" + selectPayment.Code;

                LogData log = new LogData();
                log.BillId = selectPayment.Id;
                log.BillType = "资金支付单";
                log.Code = selectPayment.Code;
                log.OperType = opType == 1 ? "保存" : "提交";
                log.Descript = "";
                log.OperPerson = ConstObject.LoginPersonInfo.Name;
                log.ProjectName = selectPayment.ProjectName;
                StaticMethod.InsertLogData(log);

                MessageBox.Show("保存成功");

                ModelToView();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败：" + ex.Message);
                return false;
            }
        }

        private void InitGrid(CustomFlexGrid grid)
        {
            FundPlanOperate.LoadFlexFile(string.Concat(grid.Tag, ".flx"), grid);

            if (currentProject != null)
            {
                grid.Cell(3, 1).Text = string.Format("项目：{0}", currentProject.Name);
                grid.Cell(3, 1).Locked = true;
            }

            grid.Cell(3, 5).Text = DateTime.Now.ToString("yyyy-MM-dd");
            grid.Cell(grid.Rows - 1, 4).Text = ConstObject.TheLogin.ThePerson.Name;
            grid.Cell(grid.Rows - 1, 6).Text = DateTime.Now.ToString();
        }

        private void InitData()
        {
            currentProject = StaticMethod.GetProjectInfo();
            if (currentProject != null && currentProject.Code.Equals(CommonUtil.CompanyProjectCode)) //项目
            {
                currentProject = null;
                isProjectOrg = false;
            }

            editColor = ColorTranslator.FromHtml("#D2FBD6");

            mOperate = new MFinanceMultData();
            selectBills = new Dictionary<string, AcceptanceBill>();
        }

        private void InitEvents()
        {
            gdPlanPay.Click -= new Grid.ClickEventHandler(gdPlanPay_Click);
            gdPlanPay.Click += new Grid.ClickEventHandler(gdPlanPay_Click);
            gdPlanPay.ButtonClick -= new Grid.ButtonClickEventHandler(gdPlanPay_ButtonClick);
            gdPlanPay.ButtonClick += new Grid.ButtonClickEventHandler(gdPlanPay_ButtonClick);
            gdPlanPay.ComboDropDown -= new Grid.ComboDropDownEventHandler(gdPlanPay_ComboDropDown);
            gdPlanPay.ComboDropDown += new Grid.ComboDropDownEventHandler(gdPlanPay_ComboDropDown);
            gdPlanPay.CellChange -= new Grid.CellChangeEventHandler(gdPlanPay_CellChange);
            gdPlanPay.CellChange += new Grid.CellChangeEventHandler(gdPlanPay_CellChange);

            gdPlanPay2.Click -= new Grid.ClickEventHandler(gdPlanPay2_Click);
            gdPlanPay2.Click += new Grid.ClickEventHandler(gdPlanPay2_Click);
            gdPlanPay2.ButtonClick -= new Grid.ButtonClickEventHandler(gdPlanPay2_ButtonClick);
            gdPlanPay2.ButtonClick += new Grid.ButtonClickEventHandler(gdPlanPay2_ButtonClick);
            gdPlanPay2.ComboDropDown -= new Grid.ComboDropDownEventHandler(gdPlanPay_ComboDropDown);
            gdPlanPay2.ComboDropDown += new Grid.ComboDropDownEventHandler(gdPlanPay_ComboDropDown);
            gdPlanPay2.CellChange -= new Grid.CellChangeEventHandler(gdPlanPay2_CellChange);
            gdPlanPay2.CellChange += new Grid.CellChangeEventHandler(gdPlanPay2_CellChange);

            btnUpFile.Click += new EventHandler(btnUpFile_Click);
            dgDocumentMast.CellClick += new DataGridViewCellEventHandler(dgDocumentMast_CellClick);
        }

        private void InitHeader()
        {
            var range = gdPlanPay.Range(4, 1, 13, 6);
            range.Locked = true;

            gdPlanPay.Cell(4, 4).Locked = false;
            gdPlanPay.Cell(4, 6).Locked = false;
            gdPlanPay.Cell(5, 4).Locked = false;
            gdPlanPay.Cell(5, 6).Locked = false;
            gdPlanPay.Cell(6, 4).Locked = false;
            gdPlanPay.Cell(6, 6).Locked = false;

            gdPlanPay.Range(3, 1, 3, 6).BackColor = editColor;
            gdPlanPay.Cell(4, 4).BackColor = editColor;
            gdPlanPay.Cell(4, 6).BackColor = editColor;
            gdPlanPay.Cell(5, 4).BackColor = editColor;
            gdPlanPay.Cell(5, 6).BackColor = editColor;
            gdPlanPay.Cell(6, 4).BackColor = editColor;
            gdPlanPay.Cell(6, 6).BackColor = editColor;
        }

        private void InitHeader2()
        {
            var range = gdPlanPay2.Range(4, 1, 8, 6);
            range.Locked = true;

            gdPlanPay2.Cell(4, 4).Locked = false;
            gdPlanPay2.Cell(4, 6).Locked = false;
            gdPlanPay2.Cell(5, 4).Locked = false;
            gdPlanPay2.Cell(6, 4).Locked = false;
            gdPlanPay2.Cell(6, 6).Locked = false;

            gdPlanPay2.Range(3, 1, 3, 6).BackColor = editColor;
            gdPlanPay2.Cell(4, 4).BackColor = editColor;
            gdPlanPay2.Cell(4, 6).BackColor = editColor;
            gdPlanPay2.Cell(5, 4).BackColor = editColor;
            gdPlanPay2.Cell(6, 4).BackColor = editColor;
            gdPlanPay2.Cell(6, 6).BackColor = editColor;
        }

        private bool CheckAccountLock(DateTime businessDate)
        {
            if (currentProject != null && currentProject.Code != CommonUtil.CompanyProjectCode)
            {
                var errorMes = mOperate.FinanceMultDataSrv.IsAllowBusinessHappend(currentProject.Id, businessDate);
                if (string.IsNullOrEmpty(errorMes))
                {
                    return true;
                }

                MessageBox.Show(errorMes);
                return false;
            }

            return true;
        }

        private void OpenProjectSelector()
        {
            if (isProjectOrg)
            {
                return;
            }

            VProjectSelectDialog projDialog = new VProjectSelectDialog(string.Empty);
            if (projDialog.ShowDialog() == DialogResult.OK)
            {
                currentProject = projDialog.SelectedProject;

                DisplayProjectInfo(gdPlanPay);

                DisplayProjectInfo(gdPlanPay2);
            }
        }

        private void DisplayProjectInfo(CustomFlexGrid grid)
        {
            if (currentProject != null)
            {
                grid.Cell(3, 1).Text = string.Format("项目：{0}", currentProject.Name);
                grid.Cell(3, 1).Tag = currentProject.Id;
            }
            else
            {
                grid.Cell(3, 1).Text = string.Empty;
                grid.Cell(3, 1).Tag = string.Empty;
            }
        }

        private void OpenPlanSelector()
        {
            var dtTxt = gdPlanPay.Cell(3, 5).Text;
            var spName = gdPlanPay.Cell(4, 4).Text;
            if (string.IsNullOrEmpty(spName))
            {
                MessageBox.Show("请先选择收款单位");
                return;
            }

            DateTime dt;
            DateTime.TryParse(dtTxt, out dt);

            VFundPlanSelector dPlanSelector = new VFundPlanSelector(currentProject, dt.Year, dt.Month, spName);
            if (dPlanSelector.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            selectPlan = dPlanSelector.SelectedPlan;

            DisPlayPlanInfo();
        }

        private void DisPlayPlanInfo()
        {
            if (selectPlan != null)
            {
                gdPlanPay.Cell(3, 6).Text = selectPlan.Master.Code;
                gdPlanPay.Cell(7, 4).Text = selectPlan.ContractAmount.ToString("N2");
                gdPlanPay.Cell(7, 6).Text = selectPlan.CumulativeSettlement.ToString("N2");
                gdPlanPay.Cell(8, 4).Text = selectPlan.CumulativePayment.ToString("N2");
                gdPlanPay.Cell(8, 6).Text = selectPlan.PaymentRatio.ToString("N2");
                gdPlanPay.Cell(9, 4).Text = selectPlan.Quota.ToString("N2");

                if (currentProject != null && selectSupplier != null)
                {
                    var values =
                        mOperate.FinanceMultDataSrv.GetPayValues(
                            currentProject.Id,
                            selectSupplier.Name2.ToString(),
                            selectPlan.Master.Code,
                            selectPayment == null ? DateTime.Now : selectPayment.RealOperationDate);

                    if (values.Count >= 2)
                    {
                        gdPlanPay.Cell(9, 6).Text = values[0].ToString("N2");
                        gdPlanPay.Cell(11, 4).Tag = values[1].ToString();
                        gdPlanPay.Cell(11, 4).Text = string.Format(" ¥{0:N2}", values[1]);
                        gdPlanPay.Cell(10, 4).Text = string.Format("（大写）{0}", CommonUtil.ToChineseNumber(values[1]));
                    }
                }

                ComputePayRate();
            }
            else
            {
                gdPlanPay.Cell(3, 6).Text = string.Empty;
                gdPlanPay.Cell(7, 4).Text = string.Empty;
                gdPlanPay.Cell(7, 6).Text = string.Empty;
                gdPlanPay.Cell(8, 4).Text = string.Empty;
                gdPlanPay.Cell(8, 6).Text = string.Empty;
                gdPlanPay.Cell(9, 4).Text = string.Empty;
            }
        }

        private void OpenSupplerSelector()
        {
            VCusAndSupRelSelector vSelector = new VCusAndSupRelSelector();
            vSelector.ShowDialog();
            IList list = vSelector.Result;
            if (list == null || list.Count == 0)
            {
                return;
            }

            selectSupplier = list[0] as DataDomain;
            DisplaySupplierInfo(gdPlanPay);
            DisplaySupplierInfo(gdPlanPay2);
        }

        private void DisplaySupplierInfo(CustomFlexGrid grid)
        {
            if (selectSupplier != null)
            {
                grid.Cell(4, 4).Text = ClientUtil.ToString(selectSupplier.Name4);
                grid.Cell(5, 4).Text = ClientUtil.ToString(selectSupplier.Name5);
                grid.Cell(6, 4).Text = ClientUtil.ToString(selectSupplier.Name6);
            }
            else
            {
                grid.Cell(4, 4).Text = string.Empty;
                grid.Cell(5, 4).Text = string.Empty;
                grid.Cell(6, 4).Text = string.Empty;
            }
        }

        private void OpenAccountSelector()
        {
            VAccountSelector accountSelector = new VAccountSelector();
            if (accountSelector.ShowDialog() == DialogResult.OK)
            {
                selectAccount = accountSelector.SelectedAccount;
                DisplayAccountInfo();
            }
        }

        private void DisplayAccountInfo()
        {
            if (selectAccount != null)
            {
                gdPlanPay.Cell(5, 6).Text = selectAccount.Name;
            }
            else
            {
                gdPlanPay.Cell(5, 6).Text = string.Empty;
            }
        }

        private void OpenAcceptanceBill(CustomFlexGrid grid)
        {
            VAcceptBillSelector vSelector = new VAcceptBillSelector();
            vSelector.ShowDialog();
            IList list = vSelector.Result;
            if (list == null || list.Count == 0)
            {
                return;
            }

            AcceptanceBill bill = list[0] as AcceptanceBill;
            if (bill == null)
            {
                MessageBox.Show("没有获取到票据信息");
                return;
            }

            if (bill.PaymentMxId != null)
            {
                MessageBox.Show(string.Format("票据[{0}]已经被付款，不能再使用", bill.BillNo));
                return;
            }

            if (!selectBills.ContainsKey(bill.Id))
            {
                selectBills.Add(bill.Id, bill);
            }

            grid.ActiveCell.Tag = bill.Id;
            grid.ActiveCell.Text = bill.BillNo;
            grid.Cell(grid.ActiveCell.Row, 6).Text = bill.SumMoney.ToString("N2");
        }

        private void AddNewDetailRow(CustomFlexGrid grid)
        {
            grid.InsertRow(grid.Rows - 1, 1);
            var range = grid.Range(grid.Rows - 2, 1, grid.Rows - 2, grid.Cols - 1);
            range.set_Borders(EdgeEnum.Inside, LineStyleEnum.Thin);
            range.set_Borders(EdgeEnum.Bottom, LineStyleEnum.Thin);
            range.BackColor = editColor;
            range.FontSize = 10;

            var indexCell = grid.Cell(grid.Rows - 2, 1);
            indexCell.Text = (indexCell.Row - (grid == gdPlanPay ? 14 : 9)).ToString();
            indexCell.FontSize = 10;
            indexCell.Locked = true;
            indexCell.FontBold = true;
            indexCell.Alignment = AlignmentEnum.CenterCenter;

            var cmbCell = grid.Cell(grid.Rows - 2, 2);
            cmbCell.CellType = CellTypeEnum.ComboBox;
            cmbCell.FontSize = 10;

            grid.ComboBox(0).DropDownFont = grid.DefaultFont;
            grid.ComboBox(0).DropDownWidth = 120;
        }

        private List<string> GetSelectedPayType(CustomFlexGrid grid)
        {
            var itemList = new List<string>();
            var startRow = grid == gdPlanPay ? 15 : 10;
            for (int i = startRow; i < grid.Rows - 1; i++)
            {
                itemList.Add(grid.Cell(i, 2).Text);
            }
            return itemList;
        }

        private void FillComboBoxItems(CustomFlexGrid grid)
        {
            FlexCell.ComboBox cb = grid.ComboBox(0);
            if (cb == null)
            {
                return;
            }

            var selectedItems = GetSelectedPayType(grid);
            var items = new List<string>()
                                {
                                    "银行金额",
                                    "商业汇票",
                                    "银行汇票",
                                    "以房(物)抵款",
                                    "履约保证金",
                                    "调拔材料",
                                    "代付生活费",
                                    "代付水电费",
                                    "其他代付、扣款"
                                };

            items.FindAll(i => !selectedItems.Exists(s => s == i)).ForEach(s => cb.Items.Add(s));
        }

        private void ComputePayRate()
        {
            var payed = Convert.ToDecimal(gdPlanPay.Cell(8, 4).DoubleValue);

            var currPay = 0m;
            decimal.TryParse(gdPlanPay.Cell(11, 4).Tag, out currPay);

            var thisPay = 0m;
            decimal.TryParse(gdPlanPay.Cell(13, 4).Tag, out thisPay);

            var settlement = Convert.ToDecimal(gdPlanPay.Cell(7, 6).DoubleValue);

            var rate = 0m;
            if (settlement != 0)
            {
                rate = (payed + currPay + thisPay) / settlement;
            }

            gdPlanPay.Cell(13, 6).Text = rate.ToString("P2");
        }

        private void ComputeThisPay(CustomFlexGrid grid)
        {
            var result = 0m;
            var startRow = grid == gdPlanPay ? 15 : 10;
            for (int i = startRow; i < grid.Rows - 1; i++)
            {
                var payType = grid.Cell(i, 2).Text;
                var tmp = 0m;
                if (payType.Contains("汇票"))
                {
                    decimal.TryParse(grid.Cell(i, 6).DoubleValue.ToString(), out tmp);
                }
                else
                {
                    decimal.TryParse(grid.Cell(i, 3).DoubleValue.ToString(), out tmp);
                }

                result += tmp;
            }

            if (grid == gdPlanPay)
            {
                grid.Cell(13, 4).Tag = result.ToString();
                grid.Cell(13, 4).Text = string.Format(" ¥{0:N2}", result);
                grid.Cell(12, 4).Text = string.Format("（大写）{0}", CommonUtil.ToChineseNumber(result));
            }
            else
            {
                grid.Cell(8, 4).Tag = result.ToString();
                grid.Cell(8, 4).Text = string.Format(" ¥{0:N2}", result);
                grid.Cell(7, 4).Text = string.Format("（大写）{0}", CommonUtil.ToChineseNumber(result));
            }

            grid.Refresh();
        }

        private void SetPayDetailType(int rIndex, int cIndex, CustomFlexGrid grid)
        {
            var txt = grid.Cell(rIndex, cIndex).Text;
            var range = grid.Range(rIndex, cIndex + 1, rIndex, grid.Cols - 1);
            range.Locked = false;
            range.MergeCells = false;
            range.BackColor = editColor;
            if (txt.Contains("汇票"))
            {
                grid.Cell(rIndex, cIndex + 1).Text = "票据号码";
                grid.Cell(rIndex, cIndex + 3).Text = "票据金额";

                range.set_Borders(EdgeEnum.Inside, LineStyleEnum.Thin);
                range.set_Borders(EdgeEnum.Bottom, LineStyleEnum.Thin);

                range.Locked = true;
                grid.Cell(rIndex, cIndex + 2).CellType = CellTypeEnum.Button;
                grid.Cell(rIndex, cIndex + 2).Locked = false;
            }
            else
            {
                range.Merge();
                range.ClearText();
            }
        }

        private void gdPlanPay_Click(object sender, EventArgs e)
        {
            var actCell = gdPlanPay.ActiveCell;
            if (actCell == null || gdPlanPay.Locked)
            {
                return;
            }

            if (actCell.Row == 14 && actCell.Col == 1)
            {
                AddNewDetailRow(gdPlanPay);
            }
        }

        private void gdPlanPay_ButtonClick(object Sender, Grid.ButtonClickEventArgs e)
        {
            if (e.Row == 3 && e.Col == 1)
            {
                OpenProjectSelector();
            }
            else if (e.Row == 3 && e.Col == 6)
            {
                OpenPlanSelector();
            }
            else if (e.Row == 4 && e.Col == 4)
            {
                OpenSupplerSelector();
            }
            else if (e.Row == 5 && e.Col == 6)
            {
                OpenAccountSelector();
            }
            else if (e.Col == 4 && e.Row > 14)
            {
                OpenAcceptanceBill(gdPlanPay);
            }
        }

        private void gdPlanPay_ComboDropDown(object sender, Grid.ComboDropDownEventArgs e)
        {
            var grid = sender as CustomFlexGrid;
            if (e.Col == 2)
            {
                FillComboBoxItems(grid);
            }
        }

        private void gdPlanPay_CellChange(object sender, Grid.CellChangeEventArgs e)
        {
            if (e.Row <= 14 || e.Row == gdPlanPay.Rows - 1)
            {
                return;
            }

            if (e.Col == 2)
            {
                SetPayDetailType(e.Row, e.Col, gdPlanPay);
            }
            else if (e.Col == 3 || e.Col == 6)
            {
                ComputeThisPay(gdPlanPay);

                ComputePayRate();
            }
        }

        private void gdPlanPay2_Click(object sender, EventArgs e)
        {
            var actCell = gdPlanPay2.ActiveCell;
            if (actCell == null || gdPlanPay2.Locked)
            {
                return;
            }

            if (actCell.Row == 9 && actCell.Col == 1)
            {
                AddNewDetailRow(gdPlanPay2);
            }
        }

        private void gdPlanPay2_ButtonClick(object sender, Grid.ButtonClickEventArgs e)
        {
            if (e.Row == 3 && e.Col == 1)
            {
                OpenProjectSelector();
            }
            else if (e.Row == 4 && e.Col == 4)
            {
                OpenSupplerSelector();
            }
            else if (e.Col == 4 && e.Row > 9)
            {
                OpenAcceptanceBill(gdPlanPay2);
            }
        }

        private void gdPlanPay2_CellChange(object sender, Grid.CellChangeEventArgs e)
        {
            if (e.Row <= 9 || e.Row == gdPlanPay2.Rows - 1)
            {
                return;
            }

            if (e.Col == 2)
            {
                SetPayDetailType(e.Row, e.Col, gdPlanPay2);
            }
            else if (e.Col == 3 || e.Col == 6)
            {
                ComputeThisPay(gdPlanPay2);
            }
        }

        private void btnUpFile_Click(object sender, EventArgs e)
        {
            if (selectPayment == null || selectPayment.Id == null)
            {
                if (MessageBox.Show("当前业务对象还没保存，是否保存！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    if(!SaveOrSubmit(1))
                    {
                        return;
                    }
                }
            }

            if (selectPayment != null && selectPayment.Id != null)
            {
                FillDoc();

                VDocumentPublicUpload frm = new VDocumentPublicUpload(selectPayment.Id);
                frm.ShowDialog();
                DocumentMaster resultDoc = frm.Result;
                if (resultDoc == null) return;
                AddDgDocumentMastInfo(resultDoc);
                dgDocumentMast.CurrentCell = dgDocumentMast[2, dgDocumentMast.Rows.Count - 1];
                dgDocumentDetail.Rows.Clear();
                if (resultDoc.ListFiles != null && resultDoc.ListFiles.Count > 0)
                {
                    foreach (DocumentDetail dtl in resultDoc.ListFiles)
                    {
                        AddDgDocumentDetailInfo(dtl);
                    }
                }
            }
        }

        void AddDgDocumentDetailInfo(DocumentDetail d)
        {
            int rowIndex = dgDocumentDetail.Rows.Add();
            AddDgDocumentDetailInfo(d, rowIndex);
        }

        void AddDgDocumentDetailInfo(DocumentDetail d, int rowIndex)
        {
            dgDocumentDetail[FileName.Name, rowIndex].Value = d.FileName;
            dgDocumentDetail.Rows[rowIndex].Tag = d;
        }

        //加载文档数据
        void FillDoc()
        {
            dgDocumentMast.Rows.Clear();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", selectPayment.Id));
            IList listObj = model.ObjectQuery(typeof(ProObjectRelaDocument), oq);
            if (listObj != null && listObj.Count > 0)
            {
                oq.Criterions.Clear();
                Disjunction dis = new Disjunction();
                foreach (ProObjectRelaDocument obj in listObj)
                {
                    dis.Add(Expression.Eq("Id", obj.DocumentGUID));
                }
                oq.AddCriterion(dis);
                oq.AddFetchMode("ListFiles", NHibernate.FetchMode.Eager);
                IList docList = model.ObjectQuery(typeof(DocumentMaster), oq);
                if (docList != null && docList.Count > 0)
                {
                    foreach (DocumentMaster m in docList)
                    {
                        AddDgDocumentMastInfo(m);
                    }
                    dgDocumentMast.CurrentCell = dgDocumentMast[2, 0];
                    dgDocumentMast_CellClick(dgDocumentMast, new DataGridViewCellEventArgs(2, 0));
                }
            }
        }

        void AddDgDocumentMastInfo(DocumentMaster m)
        {
            int rowIndex = dgDocumentMast.Rows.Add();
            AddDgDocumentMastInfo(m, rowIndex);
        }

        void AddDgDocumentMastInfo(DocumentMaster m, int rowIndex)
        {
            dgDocumentMast[colDocumentName.Name, rowIndex].Value = m.Name;
            dgDocumentMast[colCreateTime.Name, rowIndex].Value = m.CreateTime;
            dgDocumentMast[colDocumentCode.Name, rowIndex].Value = m.Code;
            dgDocumentMast[colDocumentInforType.Name, rowIndex].Value = m.DocType.ToString();
            dgDocumentMast[colDocumentState.Name, rowIndex].Value = ClientUtil.GetDocStateName(m.State);
            dgDocumentMast[colOwnerName.Name, rowIndex].Value = m.OwnerName;
            dgDocumentMast.Rows[rowIndex].Tag = m;
        }

        private void dgDocumentMast_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            dgDocumentDetail.Rows.Clear();
            DocumentMaster docMaster = dgDocumentMast.Rows[e.RowIndex].Tag as DocumentMaster;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", docMaster.Id));
            oq.AddFetchMode("TheFileCabinet", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof(DocumentDetail), oq);
            if (list != null && list.Count > 0)
            {
                foreach (DocumentDetail docDetail in list)
                {
                    AddDgDocumentDetailInfo(docDetail);
                }
            }
        }

       
    }
}