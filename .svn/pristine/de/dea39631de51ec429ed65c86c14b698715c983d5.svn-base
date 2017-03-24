using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.FinanceMng;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Base.Domain;
using CustomServiceClient.CustomWebSrv;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Basic.Domain;

using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Core;
using NPOI.SS.UserModel;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost
{
    public partial class VAccountMng : TMasterDetailView
    {
        private string sIndirectCostAccountTitleCode = "6602";
        EnumAccountMng _ExcuteType;
        private MIndirectCost model = new MIndirectCost();
        private IndirectCostMaster curBillMaster;
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        // private Hashtable htNextCostBudgetMoney = null;
        /// <summary>
        /// 当前单据
        /// </summary>
        public IndirectCostMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VAccountMng(EnumAccountMng _ExcuteType)
        {
            InitializeComponent();
            this._ExcuteType = _ExcuteType;
            InitEvent();
            InitData();
        }

        public void InitData()
        {
            //this.cmbYear.Items.Clear();
            //for (int iYear = 2012; iYear <= DateTime.Now.Year; iYear++)
            //{
            //    this.cmbYear.Items.Add(iYear);
            //}
            //this.cmbYear.SelectedItem = DateTime.Now.Year;
            //this.cmbMonth.Items.Clear();
            //for (int iMonth = 1; iMonth < 13; iMonth++)
            //{
            //    this.cmbMonth.Items.Add(iMonth);
            //}
            //this.cmbMonth.SelectedItem = DateTime.Now.Month;
            customLabel12.Visible = this._ExcuteType != EnumAccountMng.分公司财务账面维护;
            txtBorrowSymbolMoney.Visible = this._ExcuteType != EnumAccountMng.分公司财务账面维护;
        }

        private void InitEvent()
        {
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.dgDetail.UserDeletingRow += new DataGridViewRowCancelEventHandler(dgDetail_UserDeletingRow);
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
            this.txtBorrowSymbolMoney.tbTextChanged += new EventHandler(Money_Changed);//借款
            this.txtBorrowSymbolMoney.Leave += new EventHandler(TextLeave);
            this.txtFinanceCostSymbolMoney.tbTextChanged += new EventHandler(Money_Changed);//财务
            this.txtFinanceCostSymbolMoney.Leave += new EventHandler(TextLeave);
            this.txtHandOnSymbolMoney.tbTextChanged += new EventHandler(Money_Changed);//上交
            this.txtHandOnSymbolMoney.Leave += new EventHandler(TextLeave);
            this.btnExcelImport.Click += new EventHandler(this.btnImportExcel_Click);
        }
        void TextLeave(object sender, EventArgs e)
        {
            bool validity = true;
            if (sender is CustomEdit)
            {
                CustomEdit oControl = sender as CustomEdit;
                string value = oControl.Text;
                if (!string.IsNullOrEmpty(value))
                {
                    string temp_quantity = value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        oControl.Focus();


                        validity = false;
                    }
                }
            }
            if (validity)
            {
                (sender as CustomEdit).Text = ClientUtil.ToDecimal((sender as CustomEdit).Text).ToString("N2");
                //this.CalculateMoney();
            }
        }
        void Money_Changed(object sender, EventArgs e)
        {

            bool validity = true;
            if (sender is TextBox)
            {
                TextBox oControl = sender as TextBox;
                string value = oControl.Text;
                if (!string.IsNullOrEmpty(value))
                {
                    string temp_quantity = value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        oControl.Focus();


                        validity = false;
                    }
                }
            }
            if (validity)
            {
                //(sender as TextBox).Text = ClientUtil.ToDecimal((sender as TextBox).Text).ToString("N2");
                this.CalculateMoney();
            }
        }
        void tsmiDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr = dgDetail.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                dgDetail.Rows.Remove(dr);
                if (dr.Tag != null)
                {
                    curBillMaster.Details.Remove(dr.Tag as IndirectCostDetail);
                    movedDtlList.Add(dr.Tag as IndirectCostDetail);
                }
            }
        }

        void dgDetail_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            curBillMaster.Details.Remove(e.Row.Tag as BaseDetail);
            movedDtlList.Add(e.Row.Tag as IndirectCostDetail);
        }
        public void btnImportExcel_Click(object sender, EventArgs e)
        {
            string sPathFile = "";
            OpenFileDialog oOpenFileDialog = new OpenFileDialog();
            oOpenFileDialog.Filter = "xls文件|*.xls;*.xlsx";
            oOpenFileDialog.FilterIndex = 0;
            oOpenFileDialog.RestoreDirectory = true;
            oOpenFileDialog.Title = "请选择导入管理费文件";

            if (oOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                sPathFile = oOpenFileDialog.FileName;
                OpenExcel(sPathFile);
                this.CalculateMoney();
            }
        }
        private void OpenExcel(string strFileName)
        {

            //IList<IndirectCostDetail> lstDetail = null;
            try
            {
                string sFlag = string.Empty;
                string sAccountTitleCode = string.Empty;
                string sOrgName = string.Empty;
                string sMoney = string.Empty;
                double dMoney = 0;
                int iRow = -1;
                IRow oRow = null;
                AccountTitleTree oAccountTitleTree = null;
                IList lstAccount = model.AccountTitleTreeSvr.GetAccountTitleTreeByInstance(this.sIndirectCostAccountTitleCode);
                using (ExcelHelper oExcelHelper = new ExcelHelper(strFileName))
                {
                    ISheet oSheet = oExcelHelper.GetSheet("");
                    //lstDetail = new List<IndirectCostDetail>();
                    IndirectCostDetail oDetail = null;
                    for (int iStartRow = 10; iStartRow <= oSheet.LastRowNum; iStartRow++)
                    {
                        oRow = oSheet.GetRow(iStartRow);
                        sFlag = oRow.GetCell(0).StringCellValue.Trim();
                        if (string.IsNullOrEmpty(sFlag))
                        {
                            iRow = this.dgDetail.Rows.Add();
                            //oDetail = new IndirectCostDetail();
                            //oDetail.Master = this.curBillMaster;
                            ////oDetail.Id = Guid.NewGuid().ToString();
                            //oDetail.CostType = EnumCostType.管理费用;
                            //oDetail.AccountSymbol = EnumAccountSymbol.其他;
                            sAccountTitleCode = oRow.GetCell(1).StringCellValue;
                            if (!string.IsNullOrEmpty(sAccountTitleCode) && sAccountTitleCode.IndexOf('\\') > 0)
                            {
                                sAccountTitleCode = sAccountTitleCode.Substring(0, sAccountTitleCode.IndexOf("\\"));
                                oAccountTitleTree = GetAccountTitleTree(lstAccount, sAccountTitleCode);
                            }
                            else
                            {
                                oAccountTitleTree = null;
                            }

                            sOrgName = oRow.GetCell(3).StringCellValue.Trim();
                            dMoney = oRow.GetCell(6).NumericCellValue;
                            this.dgDetail.Rows[iRow].Cells[colAccountTitle.Name].Tag = oAccountTitleTree;
                            this.dgDetail.Rows[iRow].Cells[colAccountTitle.Name].Value = oAccountTitleTree == null ? "" : oAccountTitleTree.Name;
                            this.dgDetail.Rows[iRow].Cells[colActualMoney.Name].Value = dMoney.ToString("N2");
                            this.dgDetail.Rows[iRow].Cells[this.colOrgName.Name].Value = sOrgName;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            catch
            {

            }
            //return lstDetail;
        }
        public AccountTitleTree GetAccountTitleTree(IList lstAccountTitle, string sCode)
        {
            AccountTitleTree oAccountTitleTree = null;
            foreach (AccountTitleTree oAccount in lstAccountTitle)
            {
                if (string.Equals(oAccount.Code, sCode))
                {
                    oAccountTitleTree = oAccount;
                    break;
                }
            }
            return oAccountTitleTree;
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
                    curBillMaster = model.IndirectCostSvr.GetIndirectCostMasterByID(strID);
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
                    cmsDg.Enabled = true;
                    this.dgDetail.Enabled = true;
                    this.btnExcelImport.Enabled = true;
                    //htNextCostBudgetMoney = model.IndirectCostSvr.GetNextCostBudgetMoney(ConstObject.TheLogin.TheOperationOrgInfo.SysCode);
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
                    cmsDg.Enabled = false;
                    this.dgDetail.Enabled = false;
                    this.btnExcelImport.Enabled = false;
                    break;
                default:
                    break;
            }
            //this.dgDetail.VScrollOffset = int.MaxValue;
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
                ObjectLock.Unlock(pnlFloor, true);
            }
            else
            {
                ObjectLock.Lock(pnlFloor, true);
            }

            //永久锁定
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate, txtSumMoney, txtBudgetSumMoney, txtIndircSumMoney };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { this.colRate.Name, this.colOrgName.Name };
            dgDetail.SetColumnsReadOnly(lockCols);
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
                base.NewView();
                ClearView();

                movedDtlList = new ArrayList();

                this.curBillMaster = new IndirectCostMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.CreateYear = ConstObject.TheLogin.TheComponentPeriod.NowYear;
                curBillMaster.CreateMonth = ConstObject.TheLogin.TheComponentPeriod.NowMonth;
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.DocState = DocumentState.Edit;
                curBillMaster.IsSubCompany = (int)this._ExcuteType;
                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //归属项目
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
                {
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
                }
                Hashtable ht = this.model.AccountTitleTreeSvr.GetBasicAccountTitleTree();
                //借款
                curBillMaster.BorrowSymbol = new IndirectCostDetail() { Master = curBillMaster, AccountSymbol = EnumAccountSymbol.借款标志, CostType = EnumCostType.其他 };
                if (ht["短期借款"] != null)
                {
                    curBillMaster.BorrowSymbol.AccountTitle = ht["短期借款"] as AccountTitleTree;
                    curBillMaster.BorrowSymbol.AccountTitleCode = curBillMaster.BorrowSymbol.AccountTitle.Code;
                    curBillMaster.BorrowSymbol.AccountTitleID = curBillMaster.BorrowSymbol.AccountTitle.Id;
                    curBillMaster.BorrowSymbol.AccountTitleName = curBillMaster.BorrowSymbol.AccountTitle.Name;
                }

                //利润
                //curBillMaster.ProfitSymbol = new IndirectCostDetail() { Master = curBillMaster, AccountSymbol = EnumAccountSymbol.利润标志, CostType = EnumCostType.其他 };
                //if (ht["利润"] != null)
                //{
                //    curBillMaster.ProfitSymbol.AccountTitle = ht["利润"] as AccountTitleTree;
                //    curBillMaster.ProfitSymbol.AccountTitleCode = curBillMaster.ProfitSymbol.AccountTitle.Code;
                //    curBillMaster.ProfitSymbol.AccountTitleID = curBillMaster.ProfitSymbol.AccountTitle.Id;
                //    curBillMaster.ProfitSymbol.AccountTitleName = curBillMaster.ProfitSymbol.AccountTitle.Name;
                //}
                //财务
                curBillMaster.FinanceCostSymbol = new IndirectCostDetail() { Master = curBillMaster, AccountSymbol = EnumAccountSymbol.财务费用标志, CostType = EnumCostType.其他 };
                if (ht["财务费用"] != null)
                {
                    curBillMaster.FinanceCostSymbol.AccountTitle = ht["财务费用"] as AccountTitleTree;
                    curBillMaster.FinanceCostSymbol.AccountTitleCode = curBillMaster.FinanceCostSymbol.AccountTitle.Code;
                    curBillMaster.FinanceCostSymbol.AccountTitleID = curBillMaster.FinanceCostSymbol.AccountTitle.Id;
                    curBillMaster.FinanceCostSymbol.AccountTitleName = curBillMaster.FinanceCostSymbol.AccountTitle.Name;
                }
                //上交
                curBillMaster.HandOnSymbol = new IndirectCostDetail() { Master = curBillMaster, AccountSymbol = EnumAccountSymbol.上交标志, CostType = EnumCostType.其他 };
                if (ht["货币上交"] != null)
                {
                    curBillMaster.HandOnSymbol.AccountTitle = ht["货币上交"] as AccountTitleTree;
                    curBillMaster.HandOnSymbol.AccountTitleCode = curBillMaster.HandOnSymbol.AccountTitle.Code;
                    curBillMaster.HandOnSymbol.AccountTitleID = curBillMaster.HandOnSymbol.AccountTitle.Id;
                    curBillMaster.HandOnSymbol.AccountTitleName = curBillMaster.HandOnSymbol.AccountTitle.Name;
                }
                this.ModelToView();
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
                movedDtlList.Clear();
                curBillMaster = model.IndirectCostSvr.GetIndirectCostMasterByID(curBillMaster.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(curBillMaster.DocState));
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
                if (curBillMaster.Id == null)
                {
                    curBillMaster.DocState = DocumentState.InExecute;
                    curBillMaster = model.IndirectCostSvr.Save(curBillMaster) as IndirectCostMaster;
                    //插入日志
                    StaticMethod.InsertLogData(curBillMaster.Id, "新增", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, this._ExcuteType.ToString(), "", string.IsNullOrEmpty(curBillMaster.ProjectName) ? "" : curBillMaster.ProjectName);
                }
                else
                {
                    curBillMaster = model.IndirectCostSvr.Update(curBillMaster, movedDtlList) as IndirectCostMaster;
                    //插入日志
                    StaticMethod.InsertLogData(curBillMaster.Id, "修改", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, this._ExcuteType.ToString(), "", string.IsNullOrEmpty(curBillMaster.ProjectName) ? "" : curBillMaster.ProjectName);
                }
                movedDtlList.Clear();
                txtCode.Text = curBillMaster.Code;
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
                curBillMaster.DocState = DocumentState.InExecute;
                if (string.IsNullOrEmpty(curBillMaster.Id))
                {
                    curBillMaster = model.IndirectCostSvr.Save(curBillMaster) as IndirectCostMaster;
                }
                else
                {
                    curBillMaster = model.IndirectCostSvr.Update(curBillMaster, null) as IndirectCostMaster;
                }
                txtCode.Text = curBillMaster.Code;
                LogData log = new LogData();
                log.BillId = curBillMaster.Id;
                log.BillType = "费用信息维护";
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
                MessageBox.Show("数据提交错误：" + ExceptionUtil.ExceptionMessage(e));
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
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    curBillMaster = model.IndirectCostSvr.GetIndirectCostMasterByID(curBillMaster.Id);
                    model.IndirectCostSvr.Delete(curBillMaster);
                    //插入日志
                    StaticMethod.InsertLogData(curBillMaster.Id, "删除", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "费用信息维护", "", string.IsNullOrEmpty(curBillMaster.ProjectName) ? "" : curBillMaster.ProjectName);
                    ClearView();
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
                        curBillMaster = model.IndirectCostSvr.GetIndirectCostMasterByID(curBillMaster.Id);
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
                curBillMaster = model.IndirectCostSvr.GetIndirectCostMasterByID(curBillMaster.Id);
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
            //if (this.dgDetail.Rows.Count - 1 == 0)
            //{
            //    MessageBox.Show("明细不能为空！");
            //    return false;
            //}
            if (this.dgDetail.Rows.Count - 1 > 0)
            {
                dgDetail.EndEdit();
                dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));
                for (int iRow = 0; iRow < dgDetail.Rows.Count; iRow++)
                {
                    DataGridViewRow dr = dgDetail.Rows[iRow];
                    //最后一行不进行校验
                    if (dr.IsNewRow) break;
                    ValiditeCell(dr.Cells[colActualMoney.Name]);
                    ValiditeCell(dr.Cells[this.colBudgetMoney.Name]);
                    if (dr.Cells[colOrgName.Name].Tag == null && string.IsNullOrEmpty( ClientUtil.ToString( dr.Cells[colOrgName.Name].Value)))
                    {
                        MessageBox.Show("所属组织不能为空！");
                        dr.Cells[colOrgName.Name].Selected = true;
                        return false;
                    }
                    if (dr.Cells[this.colAccountTitle.Name].Tag == null && string.IsNullOrEmpty(ClientUtil.ToString(dr.Cells[colAccountTitle.Name].Value)))
                    {
                        MessageBox.Show("费用类型不能为空！");
                        dr.Cells[colAccountTitle.Name].Selected = true;
                        return false;
                    }
                }
                dgDetail.Update();
            }
            return CheckAccountLock(dtpDateEnd.Value.Date);
        }

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                curBillMaster.CreateDate = ClientUtil.ToDateTime(dtpDateEnd.Value.ToShortDateString());
                curBillMaster.Year = ConstObject.LoginDate.Year;
                curBillMaster.Month = ConstObject.LoginDate.Month;
                curBillMaster.Descript = this.txtDescript.Text;
                IndirectCostDetail curBillDtl;
                AccountTitleTree oAccountTitle = null;
                OperationOrgInfo oOrgInfo = null;
                curBillMaster.FinanceCostSymbol.Money = ClientUtil.ToDecimal(this.txtFinanceCostSymbolMoney.Text);
                curBillMaster.BorrowSymbol.Money = ClientUtil.ToDecimal(this.txtBorrowSymbolMoney.Text);
                curBillMaster.HandOnSymbol.Money = ClientUtil.ToDecimal(this.txtHandOnSymbolMoney.Text);
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as IndirectCostDetail;
                        if (curBillDtl != null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    else
                    {
                        curBillDtl = new IndirectCostDetail();
                    }
                    curBillDtl.Master = curBillMaster;
                    oAccountTitle = var.Cells[this.colAccountTitle.Name].Tag as AccountTitleTree;
                    curBillDtl.AccountTitle = oAccountTitle;
                    curBillDtl.AccountTitleCode = oAccountTitle == null ? "" : oAccountTitle.Code;
                    curBillDtl.AccountTitleName = oAccountTitle == null ? "" : oAccountTitle.Name;
                    curBillDtl.AccountTitleSyscode = oAccountTitle == null ? "" : oAccountTitle.SysCode;
                    oOrgInfo = var.Cells[this.colOrgName.Name].Tag as OperationOrgInfo;
                    curBillDtl.OrgInfo = oOrgInfo;
                    curBillDtl.OrgInfoID = oOrgInfo == null ? "" : oOrgInfo.Id;
                    curBillDtl.OrgInfoName = oOrgInfo == null ? ClientUtil.ToString(var.Cells[this.colOrgName.Name].Value) : oOrgInfo.Name;
                    curBillDtl.OrgInfoSysCode = oOrgInfo == null ? "" : oOrgInfo.SysCode;
                    curBillDtl.Money = ClientUtil.ToDecimal(var.Cells[this.colActualMoney.Name].Value);
                    curBillDtl.BudgetMoney = ClientUtil.ToDecimal(var.Cells[this.colBudgetMoney.Name].Value);
                    curBillDtl.CostType = EnumCostType.管理费用;
                    curBillDtl.AccountSymbol = EnumAccountSymbol.其他;
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[this.colDescript.Name].Value);
                    curBillMaster.AddDetail(curBillDtl);
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }
        /// <summary>
        /// 物料编码列，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            bool IsNeedCaculate = false;
            AccountTitleTree oAccountTitleTree = null;
            int iRow = 0;
            int iRowIndex = e.RowIndex;
            if (e.ColumnIndex > -1)
            {
                if (dgDetail.Columns[e.ColumnIndex] == colAccountTitle)
                {
                    VAccountTitleTreeSelect oAccountTitleTreeSelect = new VAccountTitleTreeSelect(sIndirectCostAccountTitleCode);
                    oAccountTitleTreeSelect.IsAllowMulti = dgDetail.Rows[e.RowIndex].Tag == null ? true : false;
                    oAccountTitleTreeSelect.ShowDialog();
                    if (oAccountTitleTreeSelect.SelectedNodes != null && oAccountTitleTreeSelect.SelectedNodes.Count > 0)
                    {
                        oAccountTitleTree = oAccountTitleTreeSelect.SelectedNodes[0];
                        dgDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = oAccountTitleTree;
                        dgDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = oAccountTitleTree.Name;
                        IndirectCostDetail oDetail = dgDetail.Rows[e.RowIndex].Tag as IndirectCostDetail;
                        if (oDetail != null)
                        {
                            oDetail.AccountTitle = oAccountTitleTree;
                            oDetail.AccountTitleCode = oAccountTitleTree.Code;
                            oDetail.AccountTitleName = oAccountTitleTree.Name;
                            IsNeedCaculate = true;
                        }
                    }

                    for (int i = 1; i < oAccountTitleTreeSelect.SelectedNodes.Count; i++)
                    {
                        iRow = iRowIndex + i - 1;
                        dgDetail.Rows.Insert(iRow, 1);
                        oAccountTitleTree = oAccountTitleTreeSelect.SelectedNodes[i];
                        dgDetail.Rows[iRow].Cells[e.ColumnIndex].Tag = oAccountTitleTree;
                        dgDetail.Rows[iRow].Cells[e.ColumnIndex].Value = oAccountTitleTree.Name;
                    }
                }
                else if (dgDetail.Columns[e.ColumnIndex] == this.colOrgName)
                {
                    //CommonOrg org = new CommonOrg();
                    //org.ObjectType = ORGType.OperationOrg;
                    //org.OpenSelect();
                    //IList resourceList = org.Result;
                    VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect(true);
                    frm.ShowDialog();
                    IList resourceList = frm.Result;
                    OperationOrgInfo orgInfo = resourceList == null || resourceList.Count == 0 ? null : resourceList[0] as OperationOrgInfo;
                    dgDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = orgInfo == null ? "" : orgInfo.Name;
                    dgDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = orgInfo;
                    IsNeedCaculate = true;
                }
                //if (IsNeedCaculate)
                //{
                //     oAccountTitleTree = dgDetail.Rows[e.RowIndex].Cells[colAccountTitle.Name].Tag == null ? null : dgDetail.Rows[e.RowIndex].Cells[colAccountTitle.Name].Tag as AccountTitleTree;
                //    string sOrgName = ClientUtil.ToString(dgDetail.Rows[e.RowIndex].Cells[colOrgName.Name].Value);
                //    decimal dMoney = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colBudgetMoney.Name].Value);
                //    if (oAccountTitleTree != null && !string.IsNullOrEmpty(sOrgName) && dMoney==0)
                //    {
                //         dMoney = this.GetNextCostBudgetMoney(oAccountTitleTree.Id, sOrgName);

                //    }
                //}
            }

        }

        //显示数据
        private bool ModelToView()
        {
            AccountTitleTree oAccountTitleTree = null;
            OperationOrgInfo oOperationOrgInfo = null;
            object objTemp = null;
            try
            {
                this.txtCode.Text = curBillMaster.Code;
                if (curBillMaster.RealOperationDate > ClientUtil.ToDateTime("2000-01-01"))
                {
                    this.txtCreateDate.Text = curBillMaster.RealOperationDate.ToShortDateString();
                }
                if (curBillMaster.CreateDate > ClientUtil.ToDateTime("2000-01-01"))
                {
                    dtpDateEnd.Value = curBillMaster.CreateDate;
                }
                
                this.txtDescript.Text = curBillMaster.Descript;
                txtCreatePerson.Text = curBillMaster.CreatePersonName;
                //上交
                IndirectCostDetail oHandleCostSymbol = curBillMaster.HandOnSymbol;
                this.txtHandOnSymbolMoney.Text = (oHandleCostSymbol == null ? 0 : oHandleCostSymbol.Money).ToString("N2");
                //dIndircSumMoney += (oHandleCostSymbol == null ? 0 : oHandleCostSymbol.Money);
                //借款
                IndirectCostDetail oBorrowSymbol = curBillMaster.BorrowSymbol;
                this.txtBorrowSymbolMoney.Text = (oBorrowSymbol == null ? 0 : oBorrowSymbol.Money).ToString("N2");
                //dIndircSumMoney += (oBorrowSymbol == null ? 0 : oBorrowSymbol.Money);
                //财务
                IndirectCostDetail oFinanceCostSymbol = curBillMaster.FinanceCostSymbol;
                this.txtFinanceCostSymbolMoney.Text = (oFinanceCostSymbol == null ? 0 : oFinanceCostSymbol.Money).ToString("N2");
                //dIndircSumMoney += (oFinanceCostSymbol == null ? 0 : oFinanceCostSymbol.Money);
                this.dgDetail.Rows.Clear();
                foreach (IndirectCostDetail oDetail in curBillMaster.Details)
                {
                    if (oDetail == oFinanceCostSymbol || oDetail == oHandleCostSymbol || oDetail == oBorrowSymbol) continue;
                    int i = this.dgDetail.Rows.Add();
                    if (oDetail.AccountTitle != null)
                    {
                        oAccountTitleTree = model.AccountTitleTreeSvr.GetAccountTitleTreeById(oDetail.AccountTitle.Id);
                        this.dgDetail[colAccountTitle.Name, i].Tag = oAccountTitleTree;
                        this.dgDetail[colAccountTitle.Name, i].Value = oAccountTitleTree == null ? null : oAccountTitleTree.Name;
                    }
                    if (oDetail.OrgInfo != null && !string.IsNullOrEmpty(oDetail.OrgInfo.Id))
                    {
                        objTemp = model.IndirectCostSvr.Get(typeof(OperationOrgInfo), oDetail.OrgInfo.Id);
                        oOperationOrgInfo = objTemp == null ? null : objTemp as OperationOrgInfo;
                    }
                    this.dgDetail[colOrgName.Name, i].Tag = oOperationOrgInfo;
                    this.dgDetail[colOrgName.Name, i].Value = oOperationOrgInfo == null ? oDetail.OrgInfoName : oOperationOrgInfo.Name;//对象获取不到就读保存名称 防止excel导入
                    this.dgDetail[this.colBudgetMoney.Name, i].Value = oDetail.BudgetMoney;
                    //dBudgetSumMoney += oDetail.BudgetMoney;
                    this.dgDetail[this.colActualMoney.Name, i].Value = oDetail.Money;
                    //dIndirectSumMoney += oDetail.Money;
                    this.dgDetail[this.colRate.Name, i].Value = oDetail.Rate.ToString("N2");
                    this.dgDetail[this.colDescript.Name, i].Value = oDetail.Descript;
                    // dSumMoney += oDetail.Money;
                    this.dgDetail.Rows[i].Tag = oDetail;
                }
                //dIndircSumMoney += curBillMaster.SumMoney;
                //txtIndircSumMoney.Text = dIndircSumMoney.ToString("#,###.##");
                //txtSumMoney.Text = curBillMaster.SumMoney.ToString("#,###.##");
                //txtBudgetSumMoney.Text = dBudgetSumMoney.ToString("#,###.##");
                CalculateMoney();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            if (colName == colBudgetMoney.Name || colName == colActualMoney.Name)
            {
                if (ValiditeCell(dgDetail.Rows[e.RowIndex].Cells[e.ColumnIndex]))
                {
                    if (colName == colActualMoney.Name || colName == this.colBudgetMoney.Name)
                    {
                        CalculateMoney();
                        decimal dActualMoney = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[this.colActualMoney.Name].Value);
                        dgDetail.Rows[e.RowIndex].Cells[this.colActualMoney.Name].Value = dActualMoney.ToString("N2");
                        decimal dBudgetMoney = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[this.colBudgetMoney.Name].Value);
                        dgDetail.Rows[e.RowIndex].Cells[this.colBudgetMoney.Name].Value = dBudgetMoney.ToString("N2");
                        dgDetail.Rows[e.RowIndex].Cells[colRate.Name].Value = (dBudgetMoney == 0 ? 0 : (dActualMoney / dBudgetMoney) * 100).ToString("N2");
                    }

                }
            }
        }
        public void CalculateMoney()
        {
            //根据单价和数量计算金额                
            decimal sumMoney = ClientUtil.ToDecimal(this.txtFinanceCostSymbolMoney.Text) +
                                ClientUtil.ToDecimal(this.txtBorrowSymbolMoney.Text) +
                                ClientUtil.ToDecimal(this.txtHandOnSymbolMoney.Text);

            decimal dBudgetSumMoney = 0;
            decimal dIndirectSumMoney = 0;
            for (int i = 0; i <= dgDetail.RowCount - 1; i++)
            {
                dBudgetSumMoney += ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[this.colBudgetMoney.Name].Value);
                sumMoney += ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colActualMoney.Name].Value);
                dIndirectSumMoney += ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colActualMoney.Name].Value);
            }
            txtSumMoney.Text = sumMoney.ToString("N2");
            txtIndircSumMoney.Text = dIndirectSumMoney.ToString("N2");
            txtBudgetSumMoney.Text = dBudgetSumMoney.ToString("N2");

        }
        public bool ValiditeCell(DataGridViewCell oCell)
        {
            bool validity = true;
            object value = oCell.Value;
            if (value != null)
            {
                string temp_quantity = value.ToString();
                validity = CommonMethod.VeryValid(temp_quantity);
                if (validity == false)
                {
                    MessageBox.Show("请输入数字！");
                    oCell.Selected = true;

                    dgDetail.BeginEdit(false);
                    validity = false;
                }
            }
            return validity;
        }

        public AccountTitleTree GetFinanceAccountTitle()
        {
            string sCode = "6603";
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(NHibernate.Criterion.Expression.Eq("Code", sCode));
            IList lst = model.AccountTitleTreeSvr.GetAccountTitleTreeByQuery(oQuery);
            return lst == null || lst.Count == 0 ? null : lst[0] as AccountTitleTree;
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

        public override bool Export()
        {
            return true;
        }

        private bool LoadTempleteFile(string modelName)
        {
            return true;
        }
        #endregion
        //public decimal GetNextCostBudgetMoney(string sAccountTitleID, string sOrgName)
        //{
        //    decimal dMoney = 0;
        //    string sKey = string.Empty;
        //    if (this.htNextCostBudgetMoney != null)
        //    {
        //        sKey = string.Format("{0}-{1}", sAccountTitleID, sOrgName);
        //        if (this.htNextCostBudgetMoney.ContainsKey(sKey))
        //        {
        //            dMoney = ClientUtil.ToDecimal(this.htNextCostBudgetMoney[sKey]);
        //        }
        //    }
        //    return dMoney;
        //}

        private bool CheckAccountLock(DateTime businessDate)
        {
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
            {
                var errorMes = new FinanceMultData.MFinanceMultData().FinanceMultDataSrv.IsAllowBusinessHappend(projectInfo.Id, businessDate);
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
