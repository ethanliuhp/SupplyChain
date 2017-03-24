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
using Application.Business.Erp.SupplyChain.CostManagement.OwnerQuantityMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VGatheringMng : TMasterDetailView
    {
        private MFinanceMultData model = new MFinanceMultData();
        private MAccountTitle titleModel = new MAccountTitle();
        private GatheringMaster curBillMaster;
        CurrentProjectInfo projectInfo;
        private int gatheringType;
        private IList acctTitleList = new ArrayList();
        //{备用金,投标保证金,履约保证金,预付款保证金,农民工工资保证金,安全保证金,质量保证金,诚信保证金,其他保证金,房租押金,水电押金,散装水泥押金,
        //其他押金,食堂押金,处理废材押金,风险抵押金,代业主垫款,代分包垫款,代职工垫款,调出租赁费,调出材料,配合费,罚款,其他应付其他}
        private string[] otherStrs = new string[] { "122101", "12210201", "12210202", "12210203", "12210204", "12210205", "12210206", "12210207", "12210299" 
            , "12210303", "12210304", "12210305", "12210310" ,"22410208","22410206","22410203","12210401","12210402","12210403","122194","122195","122196","122197","224199"};

        /// <summary>
        /// 收款类型,0:工程款,1:其他
        /// </summary>
        public int GatheringType
        {
            get { return gatheringType; }
            set { gatheringType = value; }
        }

        /// <summary>
        /// 当前单据
        /// </summary>
        public GatheringMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VGatheringMng(int toGatheringType)
        {
            gatheringType = toGatheringType;
            InitializeComponent();
            InitEvent();
            InitData();
        }

        public void InitData()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            VBasicDataOptr.InitBillType(this.colDtlBillType, true);
            this.dgOwnerQty.Columns["colOwnerClear"].DefaultCellStyle.NullValue = "清除";
            ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("BusinessFlag", "01"));//获取收款类别的会计科目
            oq.AddCriterion(Expression.Or(Expression.Eq("BusinessFlag", "02"), Expression.Eq("BusinessFlag", "01")));
            acctTitleList = titleModel.AccountTitleTreeSvr.GetAccountTitleTreeByQuery(oq);
            IList otherGatherList = new ArrayList();
            IList gatheringList = new ArrayList();
            AccountTitleTree titleTree = new AccountTitleTree();
            foreach (AccountTitleTree title in acctTitleList)
            {
                if (title.Code == "112201" || title.Code == "112206")
                {
                    gatheringList.Add(title);
                }
                else
                {
                    if (title.BusinessFlag == "01" && title.Code != "122199")
                    {
                        title.OrderNo = ((System.Collections.IList)otherStrs).IndexOf(title.Code);
                        otherGatherList.Add(title);
                    }
                    else
                    {
                        if (title.Code == "224199" || title.Code == "22410206" || title.Code == "22410203" || title.Code == "22410208")
                        {
                            title.OrderNo = ((System.Collections.IList)otherStrs).IndexOf(title.Code);
                            otherGatherList.Add(title);
                        }
                    }
                }
            }
            gatheringList.Insert(0, titleTree);
            var newOtherList = (from AccountTitleTree t in otherGatherList orderby t.OrderNo select t).ToList();
            newOtherList.Insert(0, titleTree);
            if (gatheringType == 0)
            {
                this.cmbGatheringType.DataSource = gatheringList;
            }
            else
            {
                this.cmbGatheringType.DataSource = newOtherList;
            }
            cmbGatheringType.DisplayMember = "Name";
            cmbGatheringType.ValueMember = "Id";
        }

        private void InitEvent()
        {
            this.dgOwnerQty.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgOwnerQty_CellClick);
            this.dgOwnerQty.CellDoubleClick += new DataGridViewCellEventHandler(dgOwnerQty_CellDoubleClick);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            this.btnSelRel.Click += new EventHandler(btnSelRel_Click);
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
            bool ifGathering = true;
            if (GatheringType == 1)
            {
                ifGathering = false;
                this.cmbGatheringType.DropDownHeight = 320;
            }
            else
            {
                this.cmbGatheringType.DropDownHeight = 100;
            }
            this.dgInvoice.Visible = ifGathering;
            //this.dgOwnerQty.Visible = ifGathering;
            this.colDtlBillDate.Visible = ifGathering;
            this.colDtlBillNo.Visible = ifGathering;
            this.colDtlBillType.Visible = ifGathering;
            this.colExpireDate.Visible = ifGathering;
            this.colAgreementMoney.Visible = ifGathering;
            this.colConcreteMoney.Visible = ifGathering;
            this.colOtherMoney.Visible = ifGathering;
            this.colOtherItemMoney.Visible = ifGathering;
            this.colPenaltyMoney.Visible = ifGathering;
            this.colWaterElecMoney.Visible = ifGathering;
            this.colWorkerMoney.Visible = ifGathering;
            this.colDtlSumMoney.Visible = ifGathering;
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
        }

        private void dgOwnerQty_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (this.dgOwnerQty.Columns[e.ColumnIndex].Name.Equals(this.colOwnerClear.Name))
            //{
            //    this.dgOwnerQty.CurrentRow.Cells[this.colOwnerBillCode.Name].Value = "";
            //    this.dgOwnerQty.CurrentRow.Tag = "";
            //    this.dgOwnerQty.CurrentRow.Cells[this.colOwnerDate.Name].Value = "";
            //    this.dgOwnerQty.CurrentRow.Cells[this.colOwnerMoney.Name].Value = "";
            //    this.dgOwnerQty.CurrentRow.Cells[this.colOwnerOkMoney.Name].Value = "";
            //    this.dgOwnerQty.CurrentRow.Cells[this.colQWBSName.Name].Value = "";
            //}
        }
        /// <summary>
        /// 关联报量列表，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgOwnerQty_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

            if (this.dgOwnerQty.Columns[e.ColumnIndex].Name.Equals(this.colOwnerBillCode.Name))
            {
                VOwnerQuantityDtlSelector vSelector = new VOwnerQuantityDtlSelector();
                vSelector.ShowDialog();
                IList list = vSelector.Result;
                if (list == null || list.Count == 0) return;
                DataDomain bill = list[0] as DataDomain;
                DataGridViewRow theCurrentRow = this.dgOwnerQty.CurrentRow;
                if (theCurrentRow.IsNewRow == true)
                {
                    int i = dgOwnerQty.Rows.Add();
                    this.dgOwnerQty[this.colOwnerBillCode.Name, i].Value = bill.Name2;
                    this.dgOwnerQty[this.colOwnerBillCode.Name, i].Tag = bill.Name1;
                    this.dgOwnerQty[this.colOwnerDate.Name, i].Value = bill.Name3;
                    this.dgOwnerQty[this.colOwnerMoney.Name, i].Value = bill.Name4;
                    this.dgOwnerQty[this.colOwnerOkMoney.Name, i].Value = bill.Name5;
                    this.dgOwnerQty[this.colQWBSName.Name, i].Value = bill.Name6;
                }
                else
                {
                    this.dgOwnerQty.CurrentRow.Cells[this.colOwnerBillCode.Name].Value = bill.Name2;
                    this.dgOwnerQty.CurrentRow.Cells[this.colOwnerBillCode.Name].Tag = bill.Name1;
                    this.dgOwnerQty.CurrentRow.Cells[this.colOwnerDate.Name].Value = bill.Name3;
                    this.dgOwnerQty.CurrentRow.Cells[this.colOwnerMoney.Name].Value = bill.Name4;
                    this.dgOwnerQty.CurrentRow.Cells[this.colOwnerOkMoney.Name].Value = bill.Name5;
                    this.dgOwnerQty.CurrentRow.Cells[this.colQWBSName.Name].Value = bill.Name6;
                }
                this.txtCode.Focus();
            }
        }
        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            if (colName == this.colDtlMoney.Name || colName == colAgreementMoney.Name || colName == colConcreteMoney.Name
                || colName == colOtherMoney.Name || colName == colPenaltyMoney.Name || colName == colWaterElecMoney.Name
                || colName == colWorkerMoney.Name || colName == this.colOtherItemMoney.Name)
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
                decimal agreementMoney = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colAgreementMoney.Name].Value);
                decimal concreteMoney = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colConcreteMoney.Name].Value);
                decimal otherMoney = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colOtherMoney.Name].Value);
                decimal otherItemMoney = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[this.colOtherItemMoney.Name].Value);
                decimal penaltyMoney = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colPenaltyMoney.Name].Value);
                decimal waterElecMoney = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colWaterElecMoney.Name].Value);
                decimal workerMoney = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colWorkerMoney.Name].Value);
                dgDetail.Rows[e.RowIndex].Cells[colDtlMoney.Name].Value = dtlMoney;
                dgDetail.Rows[e.RowIndex].Cells[colAgreementMoney.Name].Value = agreementMoney;
                dgDetail.Rows[e.RowIndex].Cells[colConcreteMoney.Name].Value = concreteMoney;
                dgDetail.Rows[e.RowIndex].Cells[colOtherMoney.Name].Value = otherMoney;
                dgDetail.Rows[e.RowIndex].Cells[colOtherItemMoney.Name].Value = otherItemMoney;
                dgDetail.Rows[e.RowIndex].Cells[colPenaltyMoney.Name].Value = penaltyMoney;
                dgDetail.Rows[e.RowIndex].Cells[colWaterElecMoney.Name].Value = waterElecMoney;
                dgDetail.Rows[e.RowIndex].Cells[colWorkerMoney.Name].Value = workerMoney;

                dgDetail.Rows[e.RowIndex].Cells[this.colDtlSumMoney.Name].Value = dtlMoney + agreementMoney + concreteMoney +
                    otherMoney + penaltyMoney + waterElecMoney + workerMoney + otherItemMoney;
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
                    curBillMaster = model.FinanceMultDataSrv.GetGatheringMasterById(Id);
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
            object[] os = new object[] { txtCode,txtOrgName, txtCreatePerson, txtProject, txtSumMoney };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colOwnerBillCode.Name, colOwnerDate.Name, colOwnerMoney.Name, colOwnerOkMoney.Name, colQWBSName.Name };
            this.dgOwnerQty.SetColumnsReadOnly(lockCols);
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
                this.curBillMaster = new GatheringMaster();
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
                curBillMaster = model.FinanceMultDataSrv.GetGatheringMasterById(curBillMaster.Id);
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
                curBillMaster = (GatheringMaster)model.FinanceMultDataSrv.SaveGatheringMaster(curBillMaster);
                txtCode.Text = curBillMaster.Code;
                //更新Caption
                LogData log = new LogData();
                log.BillId = curBillMaster.Id;
                log.BillType = "收款单";
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
                if (e.InnerException != null && e.InnerException.Message.ToUpper().Contains("U_GATHINVOICE"))
                    MessageBox.Show("收款发票号码必须唯一,不能重复！");
                else if (e.InnerException != null && e.InnerException.Message.ToUpper().Contains("U_ACCPTBILL_1"))
                    MessageBox.Show("票据号码必须唯一,不能重复！");
                else
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
                
                curBillMaster = model.FinanceMultDataSrv.SaveGatheringMaster(curBillMaster);
                txtCode.Text = curBillMaster.Code;
                LogData log = new LogData();
                log.BillId = curBillMaster.Id;
                log.BillType = "收款单";
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
                curBillMaster = model.FinanceMultDataSrv.GetGatheringMasterById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    if (!model.FinanceMultDataSrv.DeleteGatheringMaster(curBillMaster)) return false;
                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "收款单";
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
                        curBillMaster = model.FinanceMultDataSrv.GetGatheringMasterById(curBillMaster.Id);
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
                curBillMaster = model.FinanceMultDataSrv.GetGatheringMasterById(curBillMaster.Id);
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
            if (ClientUtil.ToString(this.cmbGatheringType.Text) == "")
            {
                MessageBox.Show("请选择收款类别！");
                cmbGatheringType.Focus();
                return false;
            }

            //结账检查
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
                //if (ClientUtil.ToDecimal(dr.Cells[this.colDtlMoney.Name].Value) == 0)
                //{
                //    MessageBox.Show("收款金额不能为0！");
                //    dgDetail.CurrentCell = dr.Cells[colDtlMoney.Name];
                //    return false;
                //}
                if (ClientUtil.ToString(dr.Cells[this.colDtlBillType.Name].Value) != "" || ClientUtil.ToString(dr.Cells[this.colDtlBillNo.Name].Value) != "")
                {
                    if (ClientUtil.ToString(dr.Cells[this.colDtlBillNo.Name].Value) == "")
                    {
                        MessageBox.Show("票据号码不能为空！");
                        dgDetail.CurrentCell = dr.Cells[colDtlBillNo.Name];
                        return false;
                    }
                    if (ClientUtil.ToDateTime(dr.Cells[this.colDtlBillDate.Name].Value) < ClientUtil.ToDateTime("2000-01-01"))
                    {
                        MessageBox.Show("请选择票据日期！");
                        dgDetail.CurrentCell = dr.Cells[colDtlBillDate.Name];
                        return false;
                    }
                    if (ClientUtil.ToString(dr.Cells[this.colDtlBillType.Name].Value) == "")
                    {
                        MessageBox.Show("票据类型不能为空！");
                        dgDetail.CurrentCell = dr.Cells[colDtlBillType.Name];
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

            //发票信息数据校验
            foreach (DataGridViewRow dr in this.dgInvoice.Rows)
            {
                //最后一行不进行校验
                if (dr.IsNewRow) break;
                if (ClientUtil.ToString(dr.Cells[this.colInvoiceCode.Name].Value) != "" || ClientUtil.ToString(dr.Cells[this.colInvoiceNo.Name].Value) != ""
                    || ClientUtil.ToDecimal(dr.Cells[this.colInvoiceMoney.Name].Value) != 0
                    || ClientUtil.ToDateTime(dr.Cells[this.colInvoiceDate.Name].Value) > ClientUtil.ToDateTime("2000-01-01"))
                {
                    if (ClientUtil.ToString(dr.Cells[this.colInvoiceCode.Name].Value) == "")
                    {
                        MessageBox.Show("票据代码不能为空！");
                        dgInvoice.CurrentCell = dr.Cells[colInvoiceCode.Name];
                        return false;
                    }
                    if (ClientUtil.ToString(dr.Cells[this.colInvoiceNo.Name].Value) == "")
                    {
                        MessageBox.Show("票据号码不能为空！");
                        dgInvoice.CurrentCell = dr.Cells[colInvoiceNo.Name];
                        return false;
                    }
                    if (ClientUtil.ToDateTime(dr.Cells[this.colInvoiceDate.Name].Value) < ClientUtil.ToDateTime("2000-01-01"))
                    {
                        MessageBox.Show("请选择发票日期！");
                        dgInvoice.CurrentCell = dr.Cells[colInvoiceDate.Name];
                        return false;
                    }
                    if (ClientUtil.ToDecimal(dr.Cells[this.colInvoiceMoney.Name].Value) == 0)
                    {
                        MessageBox.Show("发票金额不能为0！");
                        dgInvoice.CurrentCell = dr.Cells[colInvoiceMoney.Name];
                        return false;
                    }
                }
            }
            //报量数据校验
            int ownerCount = this.dgOwnerQty.RowCount - 1;
            decimal quantityMoney = 0;
            foreach (DataGridViewRow dr in this.dgOwnerQty.Rows)
            {
                if (dr.IsNewRow) break;
                if (ownerCount == 1)
                {
                    dr.Cells[this.colQGatherMoney.Name].Value = ClientUtil.ToDecimal(this.txtSumMoney.Text);
                }
                else
                {
                    if (ClientUtil.ToDecimal(dr.Cells[this.colQGatherMoney.Name].Value) == 0)
                    {
                        MessageBox.Show("报量信息对应的收款金额不能为0！");
                        dgOwnerQty.CurrentCell = dr.Cells[colQGatherMoney.Name];
                        return false;
                    }
                }
                quantityMoney += ClientUtil.ToDecimal(dr.Cells[this.colQGatherMoney.Name].Value);
            }
            if (gatheringType == 0 && ownerCount > 1 && quantityMoney != ClientUtil.ToDecimal(this.txtSumMoney.Text))
            {
                MessageBox.Show("报量对应收款金额之和[" + quantityMoney + "]与收款单总金额[" + this.txtSumMoney.Text + "]不相等！");
                return false;
            }
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
                curBillMaster.IfProjectMoney = gatheringType;
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
                AccountTitleTree currAccountTitle = this.cmbGatheringType.SelectedItem as AccountTitleTree;
                curBillMaster.AccountTitleID = currAccountTitle.Id;
                curBillMaster.AccountTitleName = currAccountTitle.Name;
                curBillMaster.AccountTitleCode = currAccountTitle.Code;
                curBillMaster.AccountTitleSyscode = currAccountTitle.SysCode;
                //string currAccountTitleID = this.cmbGatheringType.SelectedValue as string;
                //foreach (AccountTitleTree title in acctTitleList)
                //{
                //    if (title.Id == currAccountTitleID)
                //    {
                //        curBillMaster.AccountTitleID = title.Id;
                //        curBillMaster.AccountTitleName = title.Name;
                //        curBillMaster.AccountTitleCode = title.Code;
                //        curBillMaster.AccountTitleSyscode = title.SysCode;
                //    }
                //}
                //明细信息
                curBillMaster.Details.Clear();
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    GatheringDetail curBillDtl = new GatheringDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as GatheringDetail;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.GatheringMoney = ClientUtil.TransToDecimal(var.Cells[this.colDtlMoney.Name].Value);
                    curBillDtl.AgreementMoney = ClientUtil.TransToDecimal(var.Cells[this.colAgreementMoney.Name].Value);
                    curBillDtl.ConcreteMoney = ClientUtil.TransToDecimal(var.Cells[this.colConcreteMoney.Name].Value);
                    curBillDtl.OtherMoney = ClientUtil.TransToDecimal(var.Cells[this.colOtherMoney.Name].Value);
                    curBillDtl.OtherItemMoney = ClientUtil.TransToDecimal(var.Cells[this.colOtherItemMoney.Name].Value);
                    curBillDtl.PenaltyMoney = ClientUtil.TransToDecimal(var.Cells[this.colPenaltyMoney.Name].Value);
                    curBillDtl.WaterElecMoney = ClientUtil.TransToDecimal(var.Cells[this.colWaterElecMoney.Name].Value);
                    curBillDtl.WorkerMoney = ClientUtil.TransToDecimal(var.Cells[this.colWorkerMoney.Name].Value);
                    curBillDtl.Money = ClientUtil.TransToDecimal(var.Cells[this.colDtlSumMoney.Name].Value);
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);//备注

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
                        acceptBill.AcceptPerson = ClientUtil.ToString(txtOrgName.Text);
                        acceptBill.BillType = ClientUtil.ToString(var.Cells[this.colDtlBillType.Name].Value);
                        acceptBill.ExpireDate = ClientUtil.ToDateTime(ClientUtil.ToDateTime(var.Cells[this.colExpireDate.Name].Value).ToShortDateString());
                        acceptBill.SumMoney = curBillDtl.Money;
                        curBillDtl.AcceptBillID = acceptBill;
                    }
                    curBillMaster.AddDetail(curBillDtl);
                    var.Tag = curBillDtl;
                }
                //发票信息
                curBillMaster.ListInvoice.Clear();
                foreach (DataGridViewRow var in this.dgInvoice.Rows)
                {
                    if (var.IsNewRow) break;
                    GatheringInvoice curInvoice = new GatheringInvoice();
                    if (var.Tag != null)
                    {
                        curInvoice = var.Tag as GatheringInvoice;
                    }
                    curInvoice.InvoiceCode = ClientUtil.ToString(var.Cells[this.colInvoiceCode.Name].Value);
                    curInvoice.InvoiceNo = ClientUtil.ToString(var.Cells[this.colInvoiceNo.Name].Value);
                    curInvoice.SumMoney = ClientUtil.ToDecimal(var.Cells[this.colInvoiceMoney.Name].Value);
                    curInvoice.CreateDate = ClientUtil.ToDateTime(ClientUtil.ToDateTime(var.Cells[this.colInvoiceDate.Name].Value).ToShortDateString());
                    curInvoice.Descript = ClientUtil.ToString(var.Cells[this.colInvoiceDesc.Name].Value);
                    curInvoice.Master = curBillMaster;
                    curBillMaster.ListInvoice.Add(curInvoice);
                }
                //业主报量信息
                curBillMaster.ListRel.Clear();
                foreach (DataGridViewRow var in this.dgOwnerQty.Rows)
                {
                    if (var.IsNewRow) break;
                    GatheringAndQuantityRel curRel = new GatheringAndQuantityRel();
                    if (var.Tag != null)
                    {
                        curRel = var.Tag as GatheringAndQuantityRel;
                    }
                    string ownerQuantityMxID = ClientUtil.ToString(var.Cells[this.colOwnerBillCode.Name].Tag);
                    OwnerQuantityDetail qDetail = new OwnerQuantityDetail();
                    qDetail.Id = ownerQuantityMxID;
                    qDetail.Version = 0;
                    curRel.OwnerQuantityMxID = qDetail;
                    curRel.GatheringMoney = ClientUtil.ToDecimal(var.Cells[this.colQGatherMoney.Name].Value);
                    curRel.GatheringID = curBillMaster;
                    curBillMaster.ListRel.Add(curRel);
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
                if (curBillMaster.AccountTitleID != null)
                {
                    this.cmbGatheringType.SelectedValue = curBillMaster.AccountTitleID;
                }
                else
                {
                    this.cmbGatheringType.SelectedValue = "";
                }

                this.dgDetail.Rows.Clear();
                foreach (GatheringDetail var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();

                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    this.dgDetail[this.colDtlMoney.Name, i].Value = var.GatheringMoney;
                    this.dgDetail[this.colAgreementMoney.Name, i].Value = var.AgreementMoney;
                    this.dgDetail[this.colConcreteMoney.Name, i].Value = var.ConcreteMoney;
                    this.dgDetail[this.colOtherMoney.Name, i].Value = var.OtherMoney;
                    this.dgDetail[this.colOtherItemMoney.Name, i].Value = var.OtherItemMoney;
                    this.dgDetail[this.colPenaltyMoney.Name, i].Value = var.PenaltyMoney;
                    this.dgDetail[this.colWaterElecMoney.Name, i].Value = var.WaterElecMoney;
                    this.dgDetail[this.colWorkerMoney.Name, i].Value = var.WorkerMoney;
                    this.dgDetail[this.colDtlSumMoney.Name, i].Value = var.Money;
                    AcceptanceBill acceptBill = var.AcceptBillID;//票据信息
                    if (acceptBill != null)
                    {
                        this.dgDetail[this.colDtlBillNo.Name, i].Value = acceptBill.BillNo;
                        this.dgDetail[this.colDtlBillDate.Name, i].Value = acceptBill.CreateDate;
                        this.dgDetail[this.colDtlBillType.Name, i].Value = acceptBill.BillType;
                        this.dgDetail[this.colExpireDate.Name, i].Value = acceptBill.ExpireDate;
                    }

                    this.dgDetail.Rows[i].Tag = var;
                }
                dgInvoice.Rows.Clear();
                foreach (GatheringInvoice var in curBillMaster.ListInvoice)
                {
                    int i = this.dgInvoice.Rows.Add();
                    this.dgInvoice[this.colInvoiceCode.Name, i].Value = var.InvoiceCode;
                    this.dgInvoice[this.colInvoiceNo.Name, i].Value = var.InvoiceNo;
                    this.dgInvoice[this.colInvoiceMoney.Name, i].Value = var.SumMoney;
                    this.dgInvoice[this.colInvoiceDate.Name, i].Value = var.CreateDate;
                    this.dgInvoice[this.colInvoiceDesc.Name, i].Value = var.Descript;
                    this.dgInvoice.Rows[i].Tag = var;
                }
                this.dgOwnerQty.Rows.Clear();
                foreach (GatheringAndQuantityRel var in curBillMaster.ListRel)
                {
                    int i = this.dgOwnerQty.Rows.Add();
                    DataDomain bill = model.FinanceMultDataSrv.QueryOwnerQuantityInfoByMxID(var.OwnerQuantityMxID.Id);
                    this.dgOwnerQty[this.colOwnerBillCode.Name, i].Value = bill.Name2;
                    this.dgOwnerQty[this.colOwnerBillCode.Name, i].Tag = var.OwnerQuantityMxID.Id;
                    this.dgOwnerQty[this.colOwnerDate.Name, i].Value = ClientUtil.ToDateTime(bill.Name3).ToShortDateString();
                    this.dgOwnerQty[this.colOwnerMoney.Name, i].Value = bill.Name4;
                    this.dgOwnerQty[this.colOwnerOkMoney.Name, i].Value = bill.Name5;
                    this.dgOwnerQty[this.colQWBSName.Name, i].Value = bill.Name6;
                    this.dgOwnerQty[this.colQGatherMoney.Name, i].Value = var.GatheringMoney;
                    this.dgOwnerQty.Rows[i].Tag = var;
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
