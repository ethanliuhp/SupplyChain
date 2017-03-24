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
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.Util;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using VirtualMachine.Component.WinControls.Controls;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VFinanceMultDataAZ : TMasterDetailView
    {
        private MFinanceMultData model = new MFinanceMultData();
        private FinanceMultDataMaster curBillMaster;
        private CurrentProjectInfo projectInfo;
        Control[] arrControl;
        private AccountType accountType;
        public AccountType AccountType
        {
            get { return this.accountType; }

        }
        public VFinanceMultDataAZ(AccountType accountType)
        {
            InitializeComponent();
            arrControl = new Control[]  {
                 this.txtContractGrossProfit,this.txtMainBusinessTax,this.txtMaterialCost,
                this.txtMaterialRemain,this.txtMechanicalCost,this.txtOtherDirectCost,this.txtPersonCost,this.txtSubProjectPayout
            };
            InitDate();
            InitEvent();
            this.accountType = accountType;
        }
        public PersonInfo PersonInfo
        {
            get { return ConstObject.LoginPersonInfo; }
        }
        public OperationOrgInfo TheOperationOrgInfo
        {
            get { return ConstObject.TheLogin.TheOperationOrgInfo; }
        }
        public CurrentProjectInfo ProjectInfo
        {
            get
            {
                if (projectInfo == null)
                {
                    projectInfo = StaticMethod.GetProjectInfo();
                }
                return projectInfo;
            }
        }
        public FinanceMultDataMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }
        public void InitDate()
        {
            cmbYear.Items.Clear();
            for (int iYear = 2012; iYear <= DateTime.Now.Year; iYear++)
            {
                cmbYear.Items.Add(iYear);
            }
            cmbYear.SelectedItem = DateTime.Now.Year;

            cmbMonth.Items.Clear();
            for (int iMonth = 1; iMonth <= 12; iMonth++)
            {
                cmbMonth.Items.Add(iMonth);
            }
            cmbMonth.SelectedItem = DateTime.Now.Month;
            IntialTextBox();
            projectInfo = StaticMethod.GetProjectInfo();

        }
        void IntialTextBox()
        {
            foreach (Control oControl in arrControl)
            {
                if (oControl is CustomEdit)
                {
                    (oControl as CustomEdit).Text = ShowMoney(0);
                }
            }
            this.txtSumMoney.Text = ShowMoney(0);
        }
        void InitEvent()
        {
            if (arrControl != null && arrControl.Length > 0)
            {
                this.KeyPreview = true;
            }
            foreach (Control oControl in arrControl)
            {
                if (oControl is CustomEdit)
                {
                    (oControl as CustomEdit).Leave += new EventHandler(Leave);
                    (oControl as CustomEdit).EnterToTab = true;
                    //(oControl as CustomEdit).KeyPress += new KeyPressEventHandler(Control_KeyPress);
                }
            }

        }

        void Leave(object sender, EventArgs e)
        {
            if (sender is CustomEdit)
            {
                if (!this.ValidView(sender as CustomEdit))
                {
                    MessageBox.Show("请输入数值类型的数据");
                    (sender as CustomEdit).Focus();
                }
                else
                {
                    (sender as CustomEdit).Text = ClientUtil.ToDecimal((sender as CustomEdit).Text).ToString("N2");
                    SumMoneyTotal();
                }

            }
        }
        void SumMoneyTotal()
        {
            decimal dTotal = 0;
            for (int i = 0; i < arrControl.Length; i++)
            {
                if (arrControl[i] is CustomEdit)
                {
                    dTotal += ClientUtil.ToDecimal((arrControl[i] as CustomEdit).Text);
                }
            }
            this.txtSumMoney.Text = ShowMoney(dTotal);
        }
        private void ModelToView()
        {
            try
            {
                if (curBillMaster != null)
                {
                    txtCode.Text = curBillMaster.Code;
                    txtCreateDate.Text = curBillMaster.RealOperationDate.ToShortDateString();
                    txtDescript.Text = curBillMaster.Descript;
                    txtCreatePerson.Tag = curBillMaster.CreatePerson;
                    txtCreatePerson.Text = curBillMaster.CreatePersonName;

                    cmbYear.SelectedItem = curBillMaster.Year;
                    cmbMonth.SelectedItem = curBillMaster.Month;

                    if (curBillMaster.Details != null && curBillMaster.Details.Count > 0)
                    {
                        FinanceMultDataDetail oDetail = curBillMaster.Details.ToList()[0] as FinanceMultDataDetail;
                        txtMaterialRemain.Text = ShowMoney(oDetail.MaterialRemain);
                        //txtExchangeMaterialRemain.Text = ShowMoney(oDetail.ExchangeMaterialRemain);
                        //txtLowValueConsumableRemain.Text = ShowMoney(oDetail.LowValueConsumableRemain);
                        //txtTempDeviceRemain.Text = ShowMoney(oDetail.TempDeviceRemain);
                        //txtCivilProjectBalance.Text = ShowMoney(oDetail.CivilProjectBalance);
                        //txtSetUpProjectBuild.Text = ShowMoney(oDetail.SetUpProjectBuild);
                        //txtCivilAndSetUpBalance.Text = ShowMoney(oDetail.CivilAndSetUpBalance);
                        //txtCivilAndSetUpPayout.Text = ShowMoney(oDetail.CivilAndSetUpPayout);
                        //txtSetUpPayout.Text = ShowMoney(oDetail.SetUpPayout);
                        txtSubProjectPayout.Text = ShowMoney(oDetail.SubProjectPayout);
                        txtPersonCost.Text = ShowMoney(oDetail.PersonCost);
                        txtMaterialCost.Text = ShowMoney(oDetail.MaterialCost);
                        txtMechanicalCost.Text = ShowMoney(oDetail.MechanicalCost);
                        txtOtherDirectCost.Text = ShowMoney(oDetail.OtherDirectCost);
                        txtContractGrossProfit.Text = ShowMoney(oDetail.ContractGrossProfit);
                        txtMainBusinessTax.Text = ShowMoney(oDetail.MainBusinessTax);
                    }
                    this.SumMoneyTotal();
                }
                else
                {
                    throw new Exception("当前主表不存在");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private bool ViewToModel()
        {
            if (!ValidView(null)) return false;
            try
            {
                curBillMaster.Code = ClientUtil.ToString(this.txtCode.Text);
                curBillMaster.Year = ClientUtil.ToInt(this.cmbYear.SelectedItem);
                curBillMaster.Month = ClientUtil.ToInt(this.cmbMonth.SelectedItem);
                curBillMaster.Descript = ClientUtil.ToString(this.txtDescript.Text);
                curBillMaster.SumMoney = ClientUtil.ToDecimal(this.txtSumMoney.Text);

                if (txtSumMoney.Visible == true)
                {
                    curBillMaster.SumMoney = ClientUtil.ToDecimal(this.txtSumMoney.Text);
                }

                FinanceMultDataDetail oDetail = curBillMaster.Details.Count > 0 ? curBillMaster.Details.ToList()[0] as FinanceMultDataDetail : new FinanceMultDataDetail();
                oDetail.MaterialRemain = ClientUtil.ToDecimal(txtMaterialRemain.Text);
                //oDetail.ExchangeMaterialRemain = ClientUtil.ToDecimal(txtExchangeMaterialRemain.Text);
                //oDetail.LowValueConsumableRemain = ClientUtil.ToDecimal(txtLowValueConsumableRemain.Text);
                //oDetail.TempDeviceRemain = ClientUtil.ToDecimal(txtTempDeviceRemain.Text);
                //oDetail.CivilProjectBalance = ClientUtil.ToDecimal(txtCivilProjectBalance.Text);
                //oDetail.SetUpProjectBuild = ClientUtil.ToDecimal(txtSetUpProjectBuild.Text);
                //oDetail.CivilAndSetUpBalance = ClientUtil.ToDecimal(txtCivilAndSetUpBalance.Text);
                //oDetail.CivilAndSetUpPayout = ClientUtil.ToDecimal(txtCivilAndSetUpPayout.Text);
                //oDetail.SetUpPayout = ClientUtil.ToDecimal(txtSetUpPayout.Text);
                oDetail.SubProjectPayout = ClientUtil.ToDecimal(txtSubProjectPayout.Text);
                oDetail.PersonCost = ClientUtil.ToDecimal(txtPersonCost.Text);
                oDetail.MaterialCost = ClientUtil.ToDecimal(txtMaterialCost.Text);
                oDetail.MechanicalCost = ClientUtil.ToDecimal(txtMechanicalCost.Text);
                oDetail.OtherDirectCost = ClientUtil.ToDecimal(txtOtherDirectCost.Text);
                oDetail.ContractGrossProfit = ClientUtil.ToDecimal(txtContractGrossProfit.Text);
                oDetail.MainBusinessTax = ClientUtil.ToDecimal(txtMainBusinessTax.Text);
                oDetail.Master = curBillMaster;
                oDetail.Money = oDetail.MaterialRemain +
                    oDetail.ExchangeMaterialRemain +
                    oDetail.LowValueConsumableRemain +
                    oDetail.TempDeviceRemain +
                    oDetail.CivilProjectBalance +
                    oDetail.SetUpProjectBuild +
                    oDetail.CivilAndSetUpBalance +
                    oDetail.CivilAndSetUpPayout +
                    oDetail.SetUpPayout +
                    oDetail.SubProjectPayout +
                    oDetail.PersonCost +
                    oDetail.MaterialCost +
                    oDetail.MechanicalCost +
                    oDetail.OtherDirectCost +
                    oDetail.ContractGrossProfit +
                    oDetail.MainBusinessTax;
                curBillMaster.Details.Clear();
                curBillMaster.Details.Add(oDetail);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }
        public bool ValidView(CustomEdit oControl)
        {
            bool bReturn = true;
            Control[] arr = null;

            decimal dValue = 0;
            if (oControl == null)
            {
                arr = arrControl;
            }
            else
            {
                arr = new Control[] { oControl };
            }
            foreach (Control o in arr)
            {
                if (o is CustomEdit)
                {
                    if (!string.IsNullOrEmpty((o as CustomEdit).Text))
                    {
                        if (Decimal.TryParse((o as CustomEdit).Text, out dValue))
                        {

                        }
                        else
                        {
                            bReturn = false;
                            break;
                        }
                    }
                }
            }
            if (bReturn == false)
            {
                MessageBox.Show("请输入数字！");
                return bReturn;
            }
            return bReturn;
        }
        public string ShowMoney(decimal value)
        {
            return value == 0 ? "" : value.ToString("N2");
        }

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
                    curBillMaster = model.FinanceMultDataSrv.GetMasterByID(Id);
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
                    cmbYear.Enabled = true;
                    cmbMonth.Enabled = true;
                    txtDescript.ReadOnly = false;
                    foreach (Control oControl in arrControl)
                    {
                        if (oControl is CustomEdit)
                        {
                            (oControl as CustomEdit).ReadOnly = false;
                            (oControl as CustomEdit).Text = ShowMoney(0);
                        }
                    }
                    cmbMonth.SelectedItem = DateTime.Now.Month;
                    cmbYear.SelectedItem = DateTime.Now.Year;
                    break;
                case MainViewState.Modify:
                    cmbYear.Enabled = true;
                    cmbMonth.Enabled = true;
                    txtDescript.ReadOnly = false;
                    foreach (Control oControl in arrControl)
                    {
                        if (oControl is CustomEdit)
                        {
                            (oControl as CustomEdit).ReadOnly = false;
                        }
                    }
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    cmbYear.Enabled = false;
                    cmbMonth.Enabled = false;
                    txtDescript.ReadOnly = true;
                    foreach (Control oControl in arrControl)
                    {
                        if (oControl is CustomEdit)
                        {
                            (oControl as CustomEdit).ReadOnly = true;
                            //(oControl as TextBox).Text = ShowMoney(0);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region 增删改查
        /// <summary>
        /// 新建
        /// </summary>
        /// <returns></returns>
        public override bool NewView()
        {
            try
            {
                base.NewView();
                ClearView();

                this.curBillMaster = new FinanceMultDataMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名

                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                curBillMaster.HandOrgLevel = ConstObject.TheOperationOrg.Level;
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.DocState = DocumentState.Edit;
                curBillMaster.CreateDate = ClientUtil.ToDateTime(DateTime.Now.ToShortDateString());
                curBillMaster.AccountType = this.AccountType;
                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //责任人
                txtCreateDate.Text = DateTime.Now.ToShortDateString();
                if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
                {

                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
                }
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
                curBillMaster = model.FinanceMultDataSrv.GetMasterByID(curBillMaster.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(curBillMaster.DocState));
            MessageBox.Show(message);
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
                curBillMaster.DocState = DocumentState.InExecute;
                bool IsNew = (string.IsNullOrEmpty(curBillMaster.Id) ? true : false);
                curBillMaster = model.FinanceMultDataSrv.Save(curBillMaster) as FinanceMultDataMaster;
                //StaticMethod.InsertLogData(curBillMaster.Id, "提交", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, this._ExcuteType.ToString(), "", string.IsNullOrEmpty(curBillMaster.ProjectName) ? "" : curBillMaster.ProjectName);
                txtCode.Text = curBillMaster.Code;
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                WriteLog(IsNew);
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
        /// 保存
        /// </summary>
        /// <returns></returns>
        public override bool SaveView()
        {
            try
            {
                if (!ViewToModel()) return false;
                bool flag = false;
                if (string.IsNullOrEmpty(curBillMaster.Id))
                {
                    flag = true;
                }
                if (flag)
                {
                    curBillMaster.DocState = DocumentState.InExecute;
                    curBillMaster = model.FinanceMultDataSrv.Save(curBillMaster) as FinanceMultDataMaster;
                    //StaticMethod.InsertLogData(curBillMaster.Id, "新增", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, this._ExcuteType.ToString(), "", string.IsNullOrEmpty(curBillMaster.ProjectName) ? "" : curBillMaster.ProjectName);
                }
                else
                {
                    curBillMaster = model.FinanceMultDataSrv.Update(curBillMaster) as FinanceMultDataMaster;
                    // StaticMethod.InsertLogData(curBillMaster.Id, "修改", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, this._ExcuteType.ToString(), "", string.IsNullOrEmpty(curBillMaster.ProjectName) ? "" : curBillMaster.ProjectName);
                }
                txtCode.Text = curBillMaster.Code;
                //更新Caption
                WriteLog(flag);

                this.ViewCaption = ViewName + "-" + txtCode.Text;
                MessageBox.Show("保存成功！");
                ModelToView();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
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
                curBillMaster = model.FinanceMultDataSrv.GetMasterByID(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    if (!model.FinanceMultDataSrv.DeleteFinanceMultDataMaster(curBillMaster)) return false;
                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "项目财务账面维护";
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
                        curBillMaster = model.FinanceMultDataSrv.GetMasterByID(curBillMaster.Id);
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
                curBillMaster = model.FinanceMultDataSrv.GetMasterByID(curBillMaster.Id);
                //给界面赋值
                ModelToView();
            }
            catch (Exception e)
            {
                MessageBox.Show("数据刷新错误：" + ExceptionUtil.ExceptionMessage(e));
            }
        }
        private void WriteLog(bool IsNew)
        {
            LogData log = new LogData();
            log.BillId = curBillMaster.Id;
            log.BillType = "项目财务账面维护(安装)";
            log.Code = curBillMaster.Code;
            log.Descript = "";
            log.OperPerson = ConstObject.LoginPersonInfo.Name;
            log.ProjectName = curBillMaster.ProjectName;
            if (IsNew)
            {
                log.OperType = "新增 - " + Enum.GetName(typeof(AccountType), AccountType);
            }
            else
            {
                if (curBillMaster.DocState == DocumentState.InExecute)
                {
                    log.OperType = "提交 - " + Enum.GetName(typeof(AccountType), AccountType);

                }
                else
                {
                    log.OperType = "修改 - " + Enum.GetName(typeof(AccountType), AccountType);

                }
            }
            StaticMethod.InsertLogData(log);
        }
        #endregion
















    }
}
