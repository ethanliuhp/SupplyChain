using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using VirtualMachine.Component.WinMVC.generic;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.FinanceMng;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Service;
using Application.Business.Erp.SupplyChain.Client.Main;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost
{
    public partial class VBorrowedOrder : TMasterDetailView
    {
        private MIndirectCost service = new MIndirectCost();
        public BorrowedOrderMaster master;
        private Control[] lockClts;

        public VBorrowedOrder()
        {
            InitializeComponent();

            InitEvent();

            InitData();
        }

        public void InitData()
        {
            lockClts = new Control[]
                           {
                               ddlNature, dtpBorrowdate, txtMoney, txtPurpose, txtRemark,
                               txtCash, txtCheck, txtCheckNo, txtExchange, txtExchangeNo
                           };

            dtpDateEnd.Text = DateTime.Now.ToString();
        }

        private void InitEvent()
        {
            txtCash.tbTextChanged += new EventHandler(txtCash_tbTextChanged);
            txtCheck.tbTextChanged += new EventHandler(txtCheck_tbTextChanged);
            txtExchange.tbTextChanged += new EventHandler(txtExchange_tbTextChanged);
        }

        private void ComputeMoney()
        {
            decimal cash, check, exchange;

            decimal.TryParse(txtCash.Text.Trim(), out cash);
            decimal.TryParse(txtCheck.Text.Trim(), out check);
            decimal.TryParse(txtExchange.Text.Trim(), out exchange);

            var totalMoney = cash + check + exchange;
            txtMoney.Tag = totalMoney;
            txtMoney.Text = string.Format("{0}\t￥{1}", CommonUtil.ToChineseNumber(totalMoney), totalMoney.ToString("N2"));
        }

        private void txtExchange_tbTextChanged(object sender, EventArgs e)
        {
            decimal tmp;
            if (!decimal.TryParse(txtExchange.Text.Trim(), out tmp) || tmp < 0)
            {
                tmp = 0;
                txtExchange.Text = tmp.ToString();
            }

            txtExchange.Tag = tmp;

            ComputeMoney();
        }

        private void txtCheck_tbTextChanged(object sender, EventArgs e)
        {
            decimal tmp;
            if (!decimal.TryParse(txtCheck.Text.Trim(), out tmp) || tmp < 0)
            {
                tmp = 0;
                txtCheck.Text = tmp.ToString();
            }

            txtCheck.Tag = tmp;

            ComputeMoney();
        }

        private void txtCash_tbTextChanged(object sender, EventArgs e)
        {
            decimal tmp;
            if (!decimal.TryParse(txtCash.Text.Trim(), out tmp) || tmp < 0)
            {
                tmp = 0;
                txtCash.Text = tmp.ToString();
            }
            
            txtCash.Tag = tmp;

            ComputeMoney();
        }

        #region 固定代码

        /// <summary>
        /// 启动本窗体,(设置状态或重新加载已有的数据)
        /// </summary>
        /// <param name="code">窗体Caption</param>
        public void Start(string strID)
        {
            try
            {
                if (string.IsNullOrEmpty(strID) || string.Equals(strID, "空"))
                    RefreshState(MainViewState.Initialize);
                else
                {
                    master =
                        service.IndirectCostSvr.GetMasterByID(typeof (BorrowedOrderMaster), strID) as
                        BorrowedOrderMaster;
                    ModelToView();
                    RefreshState(MainViewState.Browser);

                    //判断是否为制单人
                    PersonInfo pi = this.txtCreateName.Tag as PersonInfo;
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
            switch (state)
            {
                case MainViewState.AddNew:
                case MainViewState.Modify:
                    txtRemark.Enabled = true;
                    txtPurpose.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    txtRemark.Enabled = false;
                    txtPurpose.Enabled = false;
                    break;
                default:
                    break;
            }
        }

        #endregion

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

                ObjectLock.Unlock(lockClts);
            }
            else
            {
                ObjectLock.Lock(lockClts);
            }
            //永久锁定
            object[] os = new object[] { txtCode, txtCreateName, dtpDateEnd, txtMoney, txtDocState };
            ObjectLock.Lock(os);
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
            else if (c is ComboBox)
            {
                c.Text = "";
            }
            else if (c is TextBox)
            {
                c.Text = "";
            }
            else if (c is CustomDateTimePicker)
            {
                ((CustomDateTimePicker) c).Value = ConstObject.LoginDate;
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
                base.NewView();

                ClearView();

                master = new BorrowedOrderMaster();
                master.CreatePerson = ConstObject.LoginPersonInfo;
                master.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                master.CreateDate = DateTime.Now;
                master.CreateYear = ConstObject.TheLogin.TheComponentPeriod.NowYear;
                master.CreateMonth = ConstObject.TheLogin.TheComponentPeriod.NowMonth;
                master.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                master.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;
                master.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                master.HandlePerson = ConstObject.LoginPersonInfo;
                master.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                master.DocState = DocumentState.Edit;

                //制单人
                txtCreateName.Tag = ConstObject.LoginPersonInfo;
                txtCreateName.Text = ConstObject.LoginPersonInfo.Name;
                //归属项目
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
                {
                    master.ProjectId = projectInfo.Id;
                    master.ProjectName = projectInfo.Name;
                }

                this.ModelToView();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "提示", MessageBoxButtons.OK,
                                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
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
            if (master.DocState == DocumentState.Edit || master.DocState == DocumentState.Valid)
            {
                base.ModifyView();
                master =
                    service.IndirectCostSvr.GetMasterByID(typeof (BorrowedOrderMaster), master.Id) as
                    BorrowedOrderMaster;
                ModelToView();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(master.DocState));
            MessageBox.Show(message);
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
                this.txtCode.Focus();
                if (!ViewToModel()) return false;

                if (master.Id == null)
                {
                    master.DocState = DocumentState.Edit;
                    master = service.IndirectCostSvr.SaveBorrowedOrderMaster(master);
                    //插入日志
                    LogData log = new LogData();
                    log.BillId = master.Id;
                    log.BillType = "借款单";
                    log.Code = master.Code;
                    log.OperType = "新增";
                    log.Descript = "";
                    log.OperPerson = ConstObject.LoginPersonInfo.Name;
                    log.ProjectName = master.ProjectName;
                    StaticMethod.InsertLogData(log);
                }
                else
                {
                    service.IndirectCostSvr.Update(master, null);
                    //插入日志
                    LogData log = new LogData();
                    log.BillId = master.Id;
                    log.BillType = "借款单";
                    log.Code = master.Code;
                    log.OperType = "修改";
                    log.Descript = "";
                    log.OperPerson = ConstObject.LoginPersonInfo.Name;
                    log.ProjectName = master.ProjectName;
                    StaticMethod.InsertLogData(log);
                }
                txtCode.Text = master.Code;
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
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
        /// 提交
        /// </summary>
        /// <returns></returns>
        public override bool SubmitView()
        {
            try
            {
                this.txtCode.Focus();
                if (!ViewToModel()) return false;
                master.DocState = DocumentState.InAudit;
                if (string.IsNullOrEmpty(master.Code))
                {
                    master = service.IndirectCostSvr.SaveBorrowedOrderMaster(master);
                }
                else
                {
                    service.IndirectCostSvr.Update(master, null);
                }
                txtCode.Text = master.Code;
                LogData log = new LogData();
                log.BillId = master.Id;
                log.BillType = "借款单";
                log.Code = master.Code;
                log.OperType = "提交";
                log.Descript = "";
                log.OperPerson = ConstObject.LoginPersonInfo.Name;
                log.ProjectName = master.ProjectName + string.Empty;
                StaticMethod.InsertLogData(log);
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                txtDocState.Text = ClientUtil.GetDocStateName(master.DocState);

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据提交错误：" + ExceptionUtil.ExceptionMessage(e));
                master.DocState = DocumentState.Edit;
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
                if (master.DocState == DocumentState.Valid || master.DocState == DocumentState.Edit)
                {
                    service.IndirectCostSvr.Delete(master);
                    //插入日志
                    LogData log = new LogData();
                    log.BillId = master.Id;
                    log.BillType = "借款单";
                    log.Code = master.Code;
                    log.OperType = "删除";
                    log.Descript = "";
                    log.OperPerson = ConstObject.LoginPersonInfo.Name;
                    log.ProjectName = master.ProjectName + string.Empty;
                    StaticMethod.InsertLogData(log);
                    ClearView();
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(master.DocState) + "】，不能删除！");
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
                        master =
                            service.IndirectCostSvr.GetMasterByID(typeof (BorrowedOrderMaster), master.Id) as
                            BorrowedOrderMaster;
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
                master =
                    service.IndirectCostSvr.GetMasterByID(typeof (BorrowedOrderMaster), master.Id) as
                    BorrowedOrderMaster;
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
            if (ddlNature.SelectedItem == null)
            {
                MessageBox.Show("请选择借款性质！");
                ddlNature.Focus();
                return false;
            }

            if (txtMoney.Tag == null)
            {
                MessageBox.Show("请输入借款金额");
                return false;
            }

            if (txtCheck.Tag != null && (decimal)txtCheck.Tag > 0 && string.IsNullOrEmpty(txtCheckNo.Text.Trim()))
            {
                MessageBox.Show("请输入支票号");
                txtCheckNo.Focus();
                return false;
            }

            if (txtExchange.Tag != null && (decimal)txtExchange.Tag > 0 && string.IsNullOrEmpty(txtExchangeNo.Text.Trim()))
            {
                MessageBox.Show("请输入汇票号");
                txtExchangeNo.Focus();
                return false;
            }

            if ((decimal)txtMoney.Tag == 0)
            {
                var result = MessageBox.Show("借款金额为0，确定保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    txtMoney.Focus();
                    return false;
                }
            }

            if (!CheckAccountLock(master.CreateDate))
            {
                return false;
            }
            return true;
        }

        //保存数据
        private bool ViewToModel()
        {
            try
            {
                if (!ValidView()) return false;
                master.CreateYear = ConstObject.LoginDate.Year;
                master.CreateMonth = ConstObject.LoginDate.Month;

                BorrowedOrderDetail detail = master.Details.FirstOrDefault() as BorrowedOrderDetail;
                if (detail == null)
                {
                    detail = new BorrowedOrderDetail();
                }

                detail.BorrowedType = ddlNature.SelectedItem == null ? "" : ddlNature.Text;
                detail.BorrowedDate = ClientUtil.ToDateTime(dtpBorrowdate.Value.ToShortDateString());
                master.CreateDate = detail.BorrowedDate;
                detail.Money = Convert.ToDecimal(txtMoney.Tag);
                detail.BorrowedPurpose = txtPurpose.Text;
                detail.Descript = txtRemark.Text;
                detail.Master = master;
                detail.RefundDate = dtpRefundDate.Value.Date;
                detail.CashMoney = txtCash.Tag == null ? 0 : (decimal) txtCash.Tag;
                detail.CheckMoney = txtCheck.Tag == null ? 0 : (decimal)txtCheck.Tag;
                detail.CheckNo = txtCheckNo.Text.Trim();
                detail.ExchangeMoney = txtExchange.Tag == null ? 0 : (decimal)txtExchange.Tag;
                detail.ExchangeNo = txtExchangeNo.Text.Trim();
                master.Details.Add(detail);

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
            txtCode.Text = master.Code;
            txtCreateName.Text = master.CreatePersonName;
            txtDocState.Text = ClientUtil.GetDocStateName(master.DocState);
            if (master.CreateDate > ClientUtil.ToDateTime("2000-01-01"))
            {
                dtpDateEnd.Text = master.CreateDate.ToShortDateString();
            }

            var detail = master.Details.FirstOrDefault() as BorrowedOrderDetail;
            if (detail != null)
            {
                ddlNature.Text = detail.BorrowedType;
                dtpBorrowdate.Text = ClientUtil.ToString(detail.BorrowedDate);
                dtpRefundDate.Value = detail.RefundDate ?? DateTime.Now;
                txtPurpose.Text = detail.BorrowedPurpose;
                txtRemark.Text = detail.Descript;
                txtCash.Text = detail.CashMoney.ToString();
                txtExchange.Text = detail.ExchangeMoney.ToString();
                txtExchangeNo.Text = detail.ExchangeNo;
                txtCheck.Text = detail.CheckMoney.ToString();
                txtCheckNo.Text = detail.CheckNo;
            }
            else
            {
                ddlNature.Text = "";
                txtMoney.Text = "";
                txtPurpose.Text = "";
                txtRemark.Text = "";
                txtCash.Text = string.Empty;
                txtCheck.Text = string.Empty;
                txtCheckNo.Text = string.Empty;
                txtExchange.Text = string.Empty;
                txtExchangeNo.Text = string.Empty;
            }
            return true;
        }

        #region 打印处理

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            return true;
        }

        public override bool Print()
        {
            return true;
        }

        #endregion

        private bool CheckAccountLock(DateTime businessDate)
        {
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
            {
                var errorMes =
                    new FinanceMultData.MFinanceMultData().FinanceMultDataSrv.IsAllowBusinessHappend(projectInfo.Id,
                                                                                                     businessDate);
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

