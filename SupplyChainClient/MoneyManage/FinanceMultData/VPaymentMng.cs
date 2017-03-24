using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Component.WinMVC.generic;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Windows.Media.Media3D;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.ApprovalMng.AppSolutionMng.Service;
using Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Service;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.ClientManagement;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatMng;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Application.Resource.PersonAndOrganization.ClientManagement.RelateClass;
using Iesi.Collections.Generic;
using Application.Business.Erp.Financial.Client.FinanceUI.InitialSetting.AccountTitleUI;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;
using Application.Business.Erp.SupplyChain.Client.StockMng;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VPaymentMng : TMasterDetailView
    {
        private MFinanceMultData model = new MFinanceMultData();
        private MStockMng stockModel = new MStockMng();
        private PaymentMaster curBillMaster;
        private MAccountTitle titleModel = new MAccountTitle();
        private IList acctTitleList = new ArrayList();
        CurrentProjectInfo projectInfo;
        //{备用金,应交税费,投标保证金,履约保证金,安全保证金,质量保证金,诚信保证金,其他保证金,预付款保证金,农民工工资保证金,风险抵押金,,房租押金,水电押金,处理废材押金,
        //食堂押金,其他押金,散装水泥押金,代业主垫款,代分包垫款,代职工垫款,调入租赁费,调入材料,保险费,政府规费,罚款,其他应收款其他}
        private string[] otherStrs = new string[] { "122101","2221", "22410101", "22410102", "22410106", "22410104", "22410107", "22410199", "12210203", "12210204",
            "22410203", "22410204", "22410205", "22410206", "22410208", "22410220", "12210305", "12210401", "12210402", "12210403", "224194", "224195", "224196", 
            "224197", "224198", "122199" };
        private int paymentType;
        /// <summary>
        /// 付款类型,0:工程款,1:其他
        /// </summary>
        public int PaymentType
        {
            get { return paymentType; }
            set { paymentType = value; }
        }

        /// <summary>
        /// 当前单据
        /// </summary>
        public PaymentMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VPaymentMng(int toPaymentType)
        {
            paymentType = toPaymentType;
            InitializeComponent();
            InitEvent();
            InitData();
        }

        public void InitData()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            VBasicDataOptr.InitBillType(this.colDtlBillType, true);

            //获取付款类别的会计科目
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Or(Expression.Eq("BusinessFlag", "02"), Expression.Eq("BusinessFlag", "01")));
            acctTitleList = titleModel.AccountTitleTreeSvr.GetAccountTitleTreeByQuery(oq);
            IList otherPaymentList = new ArrayList();
            IList paymentList = new ArrayList();
            AccountTitleTree titleTree = new AccountTitleTree();
            foreach (AccountTitleTree title in acctTitleList)
            {
                if (title.Code == "220202" || title.Code == "220203" || title.Code == "220204" || title.Code == "220205" || title.Code == "220299"
                    || title.Code == "22020801" || title.Code == "22020802" || title.Code == "22020803" || title.Code == "22020804")
                {
                    paymentList.Add(title);
                }
                else
                {
                    if (title.BusinessFlag == "02" && title.Code != "224199")
                    {
                        title.OrderNo = ((System.Collections.IList)otherStrs).IndexOf(title.Code);
                        otherPaymentList.Add(title);
                    }
                    else
                    {
                        if (title.Code == "122101" || title.Code == "12210203" || title.Code == "12210204" || title.Code == "12210305"
                            || title.Code == "12210401" || title.Code == "12210402" || title.Code == "12210403" || title.Code == "122199")
                        {
                            title.OrderNo = ((System.Collections.IList)otherStrs).IndexOf(title.Code);
                            otherPaymentList.Add(title);
                        }
                    }
                }
            }
            paymentList.Insert(0, titleTree);
            var newOtherList = (from AccountTitleTree t in otherPaymentList orderby t.OrderNo select t).ToList();
            newOtherList.Insert(0, titleTree);
            if (paymentType == 0)
            {
                this.cmbPaymentType.DataSource = paymentList;
            }
            else
            {
                this.cmbPaymentType.DataSource = newOtherList;
            }
            cmbPaymentType.DisplayMember = "Name";
            cmbPaymentType.ValueMember = "Id";
        }

        private void InitEvent()
        {
            this.dgMoneyPlan.CellDoubleClick += new DataGridViewCellEventHandler(dgMoneyPlan_CellDoubleClick);
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.flexGrid1.PrintPage += new FlexCell.Grid.PrintPageEventHandler(flexGrid1_PrintPage);
            this.btnSelRel.Click += new EventHandler(btnSelRel_Click);
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
        }
        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            InitControls();
        }

        /// <summary>
        /// 根据收款类别界面不同显示
        /// </summary>
        private void InitControls()
        {
            bool ifPayment = true;
            if (paymentType == 1)
            {
                ifPayment = false;
                this.cmbPaymentType.DropDownHeight = 340;
            }
            else
            {
                this.cmbPaymentType.DropDownHeight = 150;
            }
            this.dgMoneyPlan.Visible = false;
            this.colDtlBillDate.Visible = ifPayment;
            this.colDtlBillNo.Visible = ifPayment;
            this.colDtlAcceptPerson.Visible = ifPayment;
            this.colDtlBillType.Visible = ifPayment;
            this.colExpireDate.Visible = ifPayment;
            this.colDtlInMaterMoney.Visible = ifPayment;
            this.colDtlLiveMoney.Visible = ifPayment;
            this.colDtlOtherMoney.Visible = ifPayment;
            this.colDtlSumMoney.Visible = ifPayment;
            
        }

        #region 事件
        void btnSelRel_Click(object sender, EventArgs e)
        {
            VCusAndSupRelSelector vSelector = new VCusAndSupRelSelector();
            vSelector.ShowDialog();
            IList list = vSelector.Result;
            if (list == null || list.Count == 0) return;
            DataDomain domain = list[0] as DataDomain;
            this.txtOrgName.Tag = domain;
            this.txtOrgName.Text = ClientUtil.ToString(domain.Name4);
            txtBankAcctNo.Text =  ClientUtil.ToString(domain.Name5);
            this.txtBankNo.Text =  ClientUtil.ToString(domain.Name6);//开户行/行号
            this.txtBankNo.Tag =  ClientUtil.ToString(domain.Name7);//开户地址
        }

        /// <summary>
        /// 关联资金计划列表，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgMoneyPlan_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

            if (this.dgDetail.EditMode == DataGridViewEditMode.EditOnEnter)
            {
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(this.colPlanCode.Name))
                {

                }
            }
        }
        /// <summary>
        /// 票据选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

            if (this.dgDetail.EditMode == DataGridViewEditMode.EditOnEnter)
            {
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(this.colDtlBillNo.Name))
                {
                    VAcceptBillSelector vSelector = new VAcceptBillSelector();
                    vSelector.ShowDialog();
                    IList list = vSelector.Result;
                    if (list == null || list.Count == 0) return;
                    AcceptanceBill bill = list[0] as AcceptanceBill;
                    DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                    if (theCurrentRow.IsNewRow == true)
                    {
                        int i = dgDetail.Rows.Add();
                        this.dgDetail[this.colDtlBillDate.Name,i].Value = bill.CreateDate;
                        this.dgDetail[this.colDtlBillNo.Name, i].Value = bill.BillNo;
                        this.dgDetail[this.colDtlAcceptPerson.Name, i].Value = bill.AcceptPerson;
                        this.dgDetail[this.colDtlBillNo.Name, i].Tag = bill;
                        this.dgDetail[this.colDtlBillType.Name, i].Value = bill.BillType;
                        this.dgDetail[this.colExpireDate.Name, i].Value = bill.ExpireDate;
                        this.dgDetail[this.colDtlMoney.Name, i].Value = bill.SumMoney;
                        this.dgDetail[this.colDtlSumMoney.Name, i].Value = bill.SumMoney;
                        this.dgDetail[this.colDtlAcceptPerson.Name, i].ReadOnly = true;
                        this.dgDetail[this.colDtlAcceptPerson.Name, i].Style.BackColor = System.Drawing.SystemColors.Control;
                        this.dgDetail[this.colDtlBillNo.Name, i].ReadOnly = true;
                        this.dgDetail[this.colDtlBillNo.Name, i].Style.BackColor = System.Drawing.SystemColors.Control;
                        this.dgDetail[this.colDtlBillDate.Name, i].ReadOnly = true;
                        this.dgDetail[this.colDtlBillDate.Name, i].Style.BackColor = System.Drawing.SystemColors.Control;
                        this.dgDetail[this.colDtlBillType.Name, i].ReadOnly = true;
                        this.dgDetail[this.colDtlBillType.Name, i].Style.BackColor = System.Drawing.SystemColors.Control;
                        this.dgDetail[this.colDtlMoney.Name, i].ReadOnly = true;
                        this.dgDetail[this.colDtlMoney.Name, i].Style.BackColor = System.Drawing.SystemColors.Control;
                        this.dgDetail[this.colExpireDate.Name, i].ReadOnly = true;
                        this.dgDetail[this.colExpireDate.Name, i].Style.BackColor = System.Drawing.SystemColors.Control;
                    }
                    else
                    {
                        this.dgDetail.CurrentRow.Cells[this.colDtlBillDate.Name].Value = bill.CreateDate;
                        this.dgDetail.CurrentRow.Cells[this.colDtlBillNo.Name].Value = bill.BillNo;
                        this.dgDetail.CurrentRow.Cells[this.colDtlBillNo.Name].Tag = bill;
                        this.dgDetail.CurrentRow.Cells[this.colDtlAcceptPerson.Name].Value = bill.AcceptPerson;
                        this.dgDetail.CurrentRow.Cells[this.colDtlBillType.Name].Value = bill.BillType;
                        this.dgDetail.CurrentRow.Cells[this.colExpireDate.Name].Value = bill.ExpireDate;
                        this.dgDetail.CurrentRow.Cells[this.colDtlMoney.Name].Value = bill.SumMoney;
                        this.dgDetail.CurrentRow.Cells[this.colDtlSumMoney.Name].Value = bill.SumMoney;
                        this.dgDetail.CurrentRow.Cells[this.colDtlBillNo.Name].ReadOnly = true;
                        this.dgDetail.CurrentRow.Cells[this.colDtlBillNo.Name].Style.BackColor = System.Drawing.SystemColors.Control;
                        this.dgDetail.CurrentRow.Cells[this.colDtlAcceptPerson.Name].ReadOnly = true;
                        this.dgDetail.CurrentRow.Cells[this.colDtlAcceptPerson.Name].Style.BackColor = System.Drawing.SystemColors.Control;
                        this.dgDetail.CurrentRow.Cells[this.colDtlBillDate.Name].ReadOnly = true;
                        this.dgDetail.CurrentRow.Cells[this.colDtlBillDate.Name].Style.BackColor = System.Drawing.SystemColors.Control;
                        this.dgDetail.CurrentRow.Cells[this.colDtlBillType.Name].ReadOnly = true;
                        this.dgDetail.CurrentRow.Cells[this.colDtlBillType.Name].Style.BackColor = System.Drawing.SystemColors.Control;
                        this.dgDetail.CurrentRow.Cells[this.colDtlMoney.Name].ReadOnly = true;
                        this.dgDetail.CurrentRow.Cells[this.colDtlMoney.Name].Style.BackColor = System.Drawing.SystemColors.Control;
                        this.dgDetail.CurrentRow.Cells[this.colExpireDate.Name].ReadOnly = true;
                        this.dgDetail.CurrentRow.Cells[this.colExpireDate.Name].Style.BackColor = System.Drawing.SystemColors.Control;
                    }
                    this.txtCode.Focus();
                }
            }
        }
        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            if (colName == this.colDtlMoney.Name || colName == this.colDtlOtherMoney.Name || colName == this.colDtlInMaterMoney.Name
                || colName == this.colDtlLiveMoney.Name )
            {
                string temp_quantity = ClientUtil.ToString(dgDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                bool validity = CommonMethod.VeryValid(temp_quantity);
                if (validity == false)
                {
                    MessageBox.Show("请输入数字！");
                    this.dgDetail.Rows[e.RowIndex].Cells[colName].Value = "";
                    dgDetail.Rows[e.RowIndex].Cells[colName].Selected = true;
                    dgDetail.BeginEdit(false);
                }
                decimal sumMoney = 0;
                decimal dtlMoney = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colDtlMoney.Name].Value);
                decimal dtlLiveMoney = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[this.colDtlLiveMoney.Name].Value);
                decimal inMaterMoney = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[this.colDtlInMaterMoney.Name].Value);
                decimal otherMoney = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[this.colDtlOtherMoney.Name].Value);
                dgDetail.Rows[e.RowIndex].Cells[colDtlMoney.Name].Value = dtlMoney;
                dgDetail.Rows[e.RowIndex].Cells[colDtlLiveMoney.Name].Value = dtlLiveMoney;
                dgDetail.Rows[e.RowIndex].Cells[colDtlInMaterMoney.Name].Value = inMaterMoney;
                dgDetail.Rows[e.RowIndex].Cells[colDtlOtherMoney.Name].Value = otherMoney;

                dgDetail.Rows[e.RowIndex].Cells[this.colDtlSumMoney.Name].Value = dtlMoney + dtlLiveMoney + inMaterMoney +
                    otherMoney;
                for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                {
                    sumMoney += ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[this.colDtlSumMoney.Name].Value);
                }
                txtSumMoney.Text = sumMoney.ToString("#,###.####");
            }
        }

        #endregion

        #region 固定代码
        /// <summary>
        /// 启动本窗体,(设置状态或重新加载已有的数据)
        /// </summary>
        /// <param name="code">窗体Caption</param>
        public void Start(string Id)
        {
            try
            {
                if (Id == "空")
                    RefreshState(MainViewState.Initialize);
                else
                {
                    curBillMaster = model.FinanceMultDataSrv.GetPaymentMasterById(Id);
                    ModelToView();
                    RefreshState(MainViewState.Browser);

                    //判断是否为制单人
                    PersonInfo pi = this.txtCreatePerson.Tag as PersonInfo;
                    string perid = ConstObject.LoginPersonInfo.Id;
                    if (pi != null && !pi.Id.Equals(perid))
                    {
                        RefreshStateByQuery();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("视图启动出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// 刷新状态(按钮状态)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshState(MainViewState state)
        {
            base.RefreshState(state);
            //控制表格
            switch (state)
            {
                case MainViewState.AddNew:
                case MainViewState.Modify:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditOnEnter;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
                    break;
                default:
                    break;
            }
        }
        

        /// <summary>
        /// 刷新控件(窗体中的控件)
        /// </summary>
        /// <param name="state"></param>
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
            object[] os = new object[] { txtCode, txtOrgName, txtCreatePerson, txtProject, txtSumMoney, txtAddInvoice, txtAddPayMoney };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { this.colPlanCode.Name, this.colPlanOrderMoney.Name, this.colPlanAddBalMoney.Name, this.colPlanAddPayMoney.Name,this.colPlanRate.Name };
            this.dgMoneyPlan.SetColumnsReadOnly(lockCols);

            string[] lockColsDtl = new string[] { this.colDtlSumMoney.Name };
            this.dgDetail.SetColumnsReadOnly(lockColsDtl);
        }

        //清空数据
        private void ClearView()
        {
            ClearControl(pnlFloor);
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

        /// <summary>
        /// 新建
        /// </summary>
        /// <returns></returns>
        public override bool NewView()
        {
            try
            {
                //判断是否存在未提交的付款
                if (projectInfo != null)
                {
                    //int noSubmitCount = model.FinanceMultDataSrv.GetNoSubmitBillCount(projectInfo.Id, 1);
                    //if (noSubmitCount > 0)
                    //{
                    //    MessageBox.Show("该项目存在未提交的付款单，请先提交！");
                    //    return false;
                    //}
                }
                base.NewView();
                ClearView();
                this.curBillMaster = new PaymentMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                curBillMaster.HandOrgLevel = ConstObject.TheOperationOrg.Level;
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.DocState = DocumentState.Edit;

                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //归属项目
                if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
                {
                    txtProject.Tag = projectInfo;
                    txtProject.Text = projectInfo.Name;
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
                }
                this.ModelToView();
                //txtContractNo.Focus();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
            if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
            {
                base.ModifyView();
                curBillMaster = model.FinanceMultDataSrv.GetPaymentMasterById(curBillMaster.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(curBillMaster.DocState));
            MessageBox.Show(message);
            return false;
        }
        #endregion

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public override bool SaveView()
        {
            try
            {
                this.txtCode.Focus();
                if (!ViewToModel()) return false;
                curBillMaster.DocState = DocumentState.InExecute;
                curBillMaster = (PaymentMaster)model.FinanceMultDataSrv.SavePaymentMaster(curBillMaster);
                txtCode.Text = curBillMaster.Code;
                //更新Caption
                LogData log = new LogData();
                log.BillId = curBillMaster.Id;
                log.BillType = "付款单";
                log.Code = curBillMaster.Code;
                log.OperType = "保存";
                log.Descript = "";
                log.OperPerson = ConstObject.LoginPersonInfo.Name;
                log.ProjectName = curBillMaster.ProjectName;
                StaticMethod.InsertLogData(log);
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                MessageBox.Show("保存成功！");
                return true;
            }
            catch (Exception e)
            {
                if (e.InnerException != null && e.InnerException.Message.ToUpper().Contains("U_ACCPTBILL_1"))
                {
                    MessageBox.Show("票据号码必须唯一,不能重复！");
                }
                else
                {
                    MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
                }
            }
            return false;
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        public override bool SubmitView()
        {
            try
            {
                this.txtCode.Focus();
                if (!ViewToModel()) return false;
                bool IsNew = (string.IsNullOrEmpty(curBillMaster.Id) ? true : false);
                curBillMaster.DocState = DocumentState.InExecute;
                curBillMaster = model.FinanceMultDataSrv.SavePaymentMaster(curBillMaster);
                txtCode.Text = curBillMaster.Code;
                LogData log = new LogData();
                log.BillId = curBillMaster.Id;
                log.BillType = "付款单";
                log.Code = curBillMaster.Code;
                log.OperType = "提交";
                log.Descript = "";
                log.OperPerson = ConstObject.LoginPersonInfo.Name;
                log.ProjectName = curBillMaster.ProjectName;
                StaticMethod.InsertLogData(log);
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
                curBillMaster.DocState = DocumentState.Edit;
            }
            return false;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                curBillMaster = model.FinanceMultDataSrv.GetPaymentMasterById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    if (!model.FinanceMultDataSrv.DeletePaymentMaster(curBillMaster)) return false;
                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "付款单";
                    log.Code = curBillMaster.Code;
                    log.OperType = "删除";
                    log.Descript = "";
                    log.OperPerson = ConstObject.LoginPersonInfo.Name;
                    log.ProjectName = curBillMaster.ProjectName;
                    StaticMethod.InsertLogData(log);
                    ClearView();
                    MessageBox.Show("删除成功！");
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能删除！");
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据删除错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 撤销
        /// </summary>
        /// <returns></returns>
        public override bool CancelView()
        {
            try
            {
                switch (ViewState)
                {
                    case MainViewState.Modify:
                        //重新查询数据
                        curBillMaster = model.FinanceMultDataSrv.GetPaymentMasterById(curBillMaster.Id);
                        ModelToView();
                        break;
                    default:
                        ClearView();
                        break;
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据撤销错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public override void RefreshView()
        {
            try
            {
                //重新获得当前对象的值
                curBillMaster = model.FinanceMultDataSrv.GetPaymentMasterById(curBillMaster.Id);
                //给界面赋值
                ModelToView();
            }
            catch (Exception e)
            {
                MessageBox.Show("数据刷新错误：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// 保存数据前校验数据
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            string validMessage = "";
            if (this.dgDetail.Rows.Count - 1 == 0)
            {
                MessageBox.Show("明细不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.txtOrgName.Text) == "")
            {
                MessageBox.Show("单位不能为空！");
                txtOrgName.Focus();
                return false;
            }
            if (ClientUtil.ToString(this.cmbPaymentType.Text) == "")
            {
                MessageBox.Show("请选择付款类别！");
                cmbPaymentType.Focus();
                return false;
            }

            //检查资金是否结账
            if (!CheckAccountLock(dtpDateBegin.Value.Date))
            {
                return false;
            }

            dgDetail.EndEdit();
            dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));
            if (validMessage != "")
            {
                MessageBox.Show(validMessage);
                return false;
            }

            //明细表数据校验
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                //最后一行不进行校验
                if (dr.IsNewRow) break;

                if (ClientUtil.ToString(dr.Cells[this.colDtlBillType.Name].Value) != "" || ClientUtil.ToString(dr.Cells[this.colDtlBillNo.Name].Value) != "")
                {
                    if (ClientUtil.ToString(dr.Cells[this.colDtlBillNo.Name].Value) == "")
                    {
                        MessageBox.Show("票据号码不能为空！");
                        dgDetail.CurrentCell = dr.Cells[colDtlBillNo.Name];
                        return false;
                    }
                    if (ClientUtil.ToString(dr.Cells[this.colDtlAcceptPerson.Name].Value) == "")
                    {
                        MessageBox.Show("出票人不能为空！");
                        dgDetail.CurrentCell = dr.Cells[colDtlAcceptPerson.Name];
                        return false;
                    }
                    if (ClientUtil.ToString(dr.Cells[this.colDtlBillType.Name].Value) == "")
                    {
                        MessageBox.Show("票据类型不能为空！");
                        dgDetail.CurrentCell = dr.Cells[colDtlBillType.Name];
                        return false;
                    }
                    AcceptanceBill currBill = dr.Cells[this.colDtlBillNo.Name].Tag as AcceptanceBill;
                    if (currBill != null && currBill.PaymentMxId != null && ClientUtil.ToString(currBill.PaymentMxId.Id) != "")
                    {
                        MessageBox.Show("此票据已被付款，不能再使用！");
                        dgDetail.CurrentCell = dr.Cells[colDtlBillNo.Name];
                        return false;
                    }
                    if (ClientUtil.ToDateTime(dr.Cells[this.colDtlBillDate.Name].Value) < ClientUtil.ToDateTime("2000-01-01"))
                    {
                        MessageBox.Show("请选择票据日期！");
                        dgDetail.CurrentCell = dr.Cells[colDtlBillDate.Name];
                        return false;
                    }
                    if (ClientUtil.ToDateTime(dr.Cells[this.colExpireDate.Name].Value) < ClientUtil.ToDateTime("2000-01-01"))
                    {
                        MessageBox.Show("请选择票据到期日！");
                        dgDetail.CurrentCell = dr.Cells[colExpireDate.Name];
                        return false;
                    }
                    if (ClientUtil.ToDateTime(dr.Cells[this.colExpireDate.Name].Value) < ClientUtil.ToDateTime(dr.Cells[this.colDtlBillDate.Name].Value))
                    {
                        MessageBox.Show("票据到期日小于票据日期！");
                        dgDetail.CurrentCell = dr.Cells[colExpireDate.Name];
                        return false;
                    }
                }
               
            }
            dgDetail.Update();
            return true;
        }

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                curBillMaster.CreateDate = ClientUtil.ToDateTime(dtpDateBegin.Value.ToShortDateString());
                curBillMaster.SumMoney = ClientUtil.ToDecimal(this.txtSumMoney.Text);
                curBillMaster.Descript = ClientUtil.ToString(this.txtRemark.Text);
                curBillMaster.BankAccountNo = ClientUtil.ToString(this.txtBankAcctNo.Text);
                curBillMaster.BankName = ClientUtil.ToString(this.txtBankNo.Text);
                curBillMaster.BankAddress = ClientUtil.ToString(this.txtBankNo.Tag);
                curBillMaster.IfProjectMoney = paymentType;
                if (this.txtOrgName.Tag != null)
                {
                    DataDomain currDomain = this.txtOrgName.Tag as DataDomain;
                    if (ClientUtil.ToInt(currDomain.Name1) == 1)
                    {
                        curBillMaster.TheSupplierRelationInfo = ClientUtil.ToString(currDomain.Name2);
                        curBillMaster.TheSupplierName = txtOrgName.Text;
                        curBillMaster.TheCustomerRelationInfo = "";
                        curBillMaster.TheCustomerName = "";
                    }
                    else
                    {
                        curBillMaster.TheCustomerRelationInfo = ClientUtil.ToString(currDomain.Name2);
                        curBillMaster.TheCustomerName = txtOrgName.Text;
                        curBillMaster.TheSupplierRelationInfo = "";
                        curBillMaster.TheSupplierName = "";
                    }
                }
                else
                {
                    MessageBox.Show("单位不能为空！");
                    txtOrgName.Focus();
                    return false;
                }

                AccountTitleTree currAccountTitle = this.cmbPaymentType.SelectedItem as AccountTitleTree;
                curBillMaster.AccountTitleID = currAccountTitle.Id;
                curBillMaster.AccountTitleName = currAccountTitle.Name;
                curBillMaster.AccountTitleCode = currAccountTitle.Code;
                curBillMaster.AccountTitleSyscode = currAccountTitle.SysCode;

                //明细信息
                curBillMaster.Details.Clear();
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    PaymentDetail curBillDtl = new PaymentDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as PaymentDetail;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }

                    curBillDtl.PaymentMoney = ClientUtil.TransToDecimal(var.Cells[this.colDtlMoney.Name].Value);
                    curBillDtl.InMaterialMoney = ClientUtil.TransToDecimal(var.Cells[this.colDtlInMaterMoney.Name].Value);
                    curBillDtl.LiveMoney = ClientUtil.TransToDecimal(var.Cells[this.colDtlLiveMoney.Name].Value);
                    curBillDtl.OtherMoney = ClientUtil.TransToDecimal(var.Cells[this.colDtlOtherMoney.Name].Value);
                    curBillDtl.Money = ClientUtil.TransToDecimal(var.Cells[this.colDtlSumMoney.Name].Value);
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);//备注

                    AcceptanceBill selectBill = var.Cells[this.colDtlBillNo.Name].Tag as AcceptanceBill;
                    if (selectBill != null && ClientUtil.ToString(selectBill.Id) != "")
                    {
                        curBillDtl.AcceptBillID = selectBill;
                    }
                    else
                    {
                        if ((curBillDtl.AcceptBillID != null && ClientUtil.ToString(curBillDtl.AcceptBillID.Id) != "") || ClientUtil.ToString(var.Cells[this.colDtlBillNo.Name].Value) != "")
                        {
                            //票据信息
                            AcceptanceBill acceptBill = curBillDtl.AcceptBillID;
                            if (acceptBill == null)
                            {
                                acceptBill = new AcceptanceBill();
                            }
                            acceptBill.CreateDate = ClientUtil.ToDateTime(ClientUtil.ToDateTime(var.Cells[this.colDtlBillDate.Name].Value).ToShortDateString());
                            acceptBill.BillNo = ClientUtil.ToString(var.Cells[this.colDtlBillNo.Name].Value);
                            acceptBill.AcceptPerson = ClientUtil.ToString(var.Cells[this.colDtlAcceptPerson.Name].Value);
                            acceptBill.BillType = ClientUtil.ToString(var.Cells[this.colDtlBillType.Name].Value);
                            acceptBill.ExpireDate = ClientUtil.ToDateTime(ClientUtil.ToDateTime(var.Cells[this.colExpireDate.Name].Value).ToShortDateString());
                            acceptBill.SumMoney = curBillDtl.Money;
                            curBillDtl.AcceptBillID = acceptBill;
                        }
                    }
                    curBillMaster.AddDetail(curBillDtl);
                    var.Tag = curBillDtl;
                }
                if (curBillMaster.Details == null || (curBillMaster.Details != null && curBillMaster.Details.Count == 0))
                {
                    MessageBox.Show("明细不能为空！");
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }  

        //显示数据
        private bool ModelToView()
        {
            try
            {
                if (curBillMaster.CreateDate > ClientUtil.ToDateTime("2000-01-01"))
                {
                    dtpDateBegin.Value = curBillMaster.CreateDate;
                }
                this.txtCode.Text = curBillMaster.Code;
                txtRemark.Text = curBillMaster.Descript;
                this.txtCreatePerson.Tag = curBillMaster.CreatePerson;
                txtCreatePerson.Text = curBillMaster.CreatePersonName;
                this.txtSumMoney.Text = curBillMaster.SumMoney.ToString("#,###.####");
                txtProject.Text = curBillMaster.ProjectName;
                txtAddInvoice.Text = curBillMaster.AddInvoiceMoney + "";
                txtAddPayMoney.Text = curBillMaster.AddPayMoney + "";

                if (curBillMaster.AccountTitleID != null)
                {
                    this.cmbPaymentType.SelectedValue = curBillMaster.AccountTitleID;
                }
                else
                {
                    this.cmbPaymentType.SelectedValue = "";
                }

                DataDomain currDomain = new DataDomain();
                if (ClientUtil.ToString(curBillMaster.TheSupplierRelationInfo) != "")
                {
                    currDomain.Name1 = "1";
                    currDomain.Name2 = curBillMaster.TheSupplierRelationInfo;
                    this.txtOrgName.Text = curBillMaster.TheSupplierName;
                }
                else
                {
                    currDomain.Name1 = "2";
                    currDomain.Name2 = curBillMaster.TheCustomerRelationInfo;
                    this.txtOrgName.Text = curBillMaster.TheCustomerName;
                }
                txtBankAcctNo.Text = curBillMaster.BankAccountNo;
                txtBankNo.Text = curBillMaster.BankName;
                txtBankNo.Tag = curBillMaster.BankAddress;
                this.dgDetail.Rows.Clear();
                foreach (PaymentDetail var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();

                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    this.dgDetail[this.colDtlMoney.Name, i].Value = var.PaymentMoney;
                    this.dgDetail[this.colDtlInMaterMoney.Name, i].Value = var.InMaterialMoney;
                    this.dgDetail[this.colDtlLiveMoney.Name, i].Value = var.LiveMoney;
                    this.dgDetail[this.colDtlOtherMoney.Name, i].Value = var.OtherMoney;
                    this.dgDetail[this.colDtlSumMoney.Name, i].Value = var.Money;
                    AcceptanceBill acceptBill = var.AcceptBillID;//票据信息
                    if (acceptBill != null)
                    {
                        this.dgDetail[this.colDtlBillNo.Name, i].Value = acceptBill.BillNo;
                        this.dgDetail[this.colDtlAcceptPerson.Name, i].Value = acceptBill.AcceptPerson;
                        this.dgDetail[this.colDtlBillDate.Name, i].Value = acceptBill.CreateDate;
                        this.dgDetail[this.colDtlBillType.Name, i].Value = acceptBill.BillType;
                        this.dgDetail[this.colExpireDate.Name, i].Value = acceptBill.ExpireDate;
                    }

                    this.dgDetail.Rows[i].Tag = var;
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
                //MessageBox.Show("数据映射错误：" + StaticMethod.ExceptionMessage(e));
                //return false;
            }
        }
     
        #region 打印
        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            if (LoadTempleteFile(@"付款单.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            return true;
        }
        //打印事件
        void flexGrid1_PrintPage(object Sender, FlexCell.Grid.PrintPageEventArgs e)
        {
            if (e.Preview == false && e.PageNumber == 1)
            {
                stockModel.StockInSrv.UpdateBillPrintTimes(0, curBillMaster.Id);//回写次数
                //写打印日志
                StaticMethod.InsertLogData(curBillMaster.Id, "打印", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "付款单", "", curBillMaster.ProjectName);
            }
        }

        public override bool Print()
        {
            if (LoadTempleteFile(@"付款单.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.Print();
            curBillMaster.PrintTimes = curBillMaster.PrintTimes + 1;
            //curBillMaster = model.PaymentSrv.SavePayment(curBillMaster);

            return true;
        }

        public override bool Export()
        {

            if (LoadTempleteFile(@"付款单.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.ExportToExcel("付款单【" + curBillMaster.Code + "】", false, false, true);

            return true;
        }

        private bool LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式和数据
                flexGrid1.OpenFile(path + "\\" + modelName);//载入格式
            }
            else
            {
                MessageBox.Show("未找到模板格式文件【" + modelName + "】");
                return false;
            }
            return true;
        }

        private void FillFlex(PaymentMaster billMaster)
        {
            CommonUtil.SetFlexGridPrintByA4(this.flexGrid1);
            flexGrid1.PageSetup.TopMargin = ClientUtil.Tofloat("1");
            flexGrid1.PageSetup.Landscape = true;
            //主表数据
            flexGrid1.Cell(3, 2).Text = billMaster.AccountTitleName;
            flexGrid1.Cell(3, 4).Text = billMaster.TheSupplierName;
            flexGrid1.Cell(3, 4).WrapText = true;
            flexGrid1.Cell(3, 6).Text = billMaster.BankAddress;
            flexGrid1.Cell(4, 2).Text = "";
            flexGrid1.Cell(4, 4).Text = billMaster.BankName;
            flexGrid1.Cell(4, 6).Text = billMaster.BankAccountNo;
            flexGrid1.Cell(5, 2).Text = "";
            flexGrid1.Cell(5, 4).Text = billMaster.AddInvoiceMoney.ToString("#,###.####");

            flexGrid1.Cell(6, 2).Text = billMaster.AddBalMoney.ToString("#,###.####");
            flexGrid1.Cell(6, 4).Text = billMaster.AddPayMoney.ToString("#,###.####");
            if (billMaster.AddBalMoney != 0)
            {
                flexGrid1.Cell(6, 6).Text = decimal.Round(billMaster.AddPayMoney * 100 / billMaster.AddBalMoney, 2).ToString("#,###.####"); ;
            }
            string acceptBillCodeStr = "";
            string acceptBillTypeStr = "";
            foreach (PaymentDetail var in curBillMaster.Details)
            {
                if (var.AcceptBillID != null && ClientUtil.ToString(var.AcceptBillID.Id) != "")
                {
                    if (acceptBillCodeStr == "")
                    {
                        acceptBillCodeStr += var.AcceptBillID.BillNo;
                    }
                    else
                    {
                        acceptBillCodeStr += "/" + var.AcceptBillID.BillNo;
                    }
                    if (acceptBillTypeStr == "")
                    {
                        acceptBillTypeStr += var.AcceptBillID.BillType;
                    }
                    else
                    {
                        if (!acceptBillCodeStr.Contains(var.AcceptBillID.BillType))
                        {
                            acceptBillTypeStr += "/" + var.AcceptBillID.BillType;
                        }
                    }
                }
            }
            
            if (acceptBillCodeStr != "")
            {
                flexGrid1.Cell(7, 2).Text = acceptBillTypeStr;
            }
            else
            {
                flexGrid1.Cell(7, 2).Text = "现金";
            }
            flexGrid1.Cell(7, 2).WrapText = true;
            flexGrid1.Cell(7, 4).Text = billMaster.SumMoney.ToString("#,###.####");
            flexGrid1.Cell(7, 6).Text = acceptBillCodeStr;
            flexGrid1.Cell(7, 6).WrapText = true;
            flexGrid1.Cell(8, 2).Text = billMaster.Descript;
            flexGrid1.Cell(9, 2).Text = billMaster.ProjectName;
            flexGrid1.Cell(9, 4).Text = "制单日期: "+billMaster.CreateDate.ToShortDateString();
            flexGrid1.Cell(10, 4).Text = "制单人: " + billMaster.CreatePersonName;
            
            //条形码
            string a = Application.Business.Erp.SupplyChain.Client.Util.DateUtil.GetStrMoneyInchar(ClientUtil.ToString(billMaster.SumMoney));
            this.flexGrid1.Cell(1, 6).Text = billMaster.Code.Substring(billMaster.Code.Length - 11) + "-" + a;
            this.flexGrid1.Cell(1, 6).CellType = FlexCell.CellTypeEnum.BarCode;
            this.flexGrid1.Cell(1, 6).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;
            this.flexGrid1.Cell(2, 6).Text = "打印顺序号: " + CommonUtil.GetPrintTimesStr(billMaster.PrintTimes + 1);

            this.flexGrid1.FrozenBottomRows = 2;
            //审批信息打印
            int maxRow =  10;
            //CommonUtil.SetFlexAuditPrint(flexGrid1, billMaster.Id, maxRow);
        }
        #endregion

        private bool CheckAccountLock(DateTime businessDate)
        {
            if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
            {
                var errorMes = model.FinanceMultDataSrv.IsAllowBusinessHappend(projectInfo.Id, businessDate);
                if (string.IsNullOrEmpty(errorMes))
                {
                    return true;
                }

                MessageBox.Show(errorMes);
                return false;
            }

            return true;
        }
    }
}
