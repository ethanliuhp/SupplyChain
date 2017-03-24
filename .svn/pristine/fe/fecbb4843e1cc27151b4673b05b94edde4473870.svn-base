using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Linq;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;

using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Main;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonForm;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Threading;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Iesi.Collections.Generic;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppProcessUI;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng
{
    public partial class VQuantitiesAffirm : TMasterDetailView
    {
        public MProjectTaskAccount model = new MProjectTaskAccount();

        /// <summary>
        /// 核算节点集合
        /// </summary>
        private List<TreeNode> ListAccountGWBSNodes = new List<TreeNode>();

        /// <summary>
        /// 核算节点下的统计节点集合
        /// </summary>
        private IList ListStatGWBSNodes = new ArrayList();
        private ProjectTaskAccountBill curBillMaster = null;

        /// <summary>
        /// 当前单据
        /// </summary>
        public ProjectTaskAccountBill CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        /// <summary>
        /// 需要反写数据的确认明细集
        /// </summary>
        private List<GWBSTaskConfirm> listGWBSTaskConfirms = new List<GWBSTaskConfirm>();

        /// <summary>
        /// 当前项目
        /// </summary>
        private CurrentProjectInfo projectInfo = null;

        public VQuantitiesAffirm()
        {
            InitializeComponent();
            InitForm();
            RefreshControls(MainViewState.Browser);
        }

        private void InitForm()
        {
            InitEvents();
            projectInfo = StaticMethod.GetProjectInfo();
            DateTime serverTime = model.GetServerTime();
            dtAccountEndDate.Value = serverTime;
        }

        private void SetGridRowReadOnly(bool rowReadOnly)
        {
            //设置编辑的单元格状态
            foreach (DataGridViewRow row in gridDetail.Rows)
            {
                row.ReadOnly = rowReadOnly;
            }
        }

        private void InitEvents()
        {
            btnSelectAccountTaskRootNode.Click += new EventHandler(btnSelectAccountTaskRootNode_Click);
            btnGeneAccountBill.Click += new EventHandler(btnGeneAccountBill_Click);

            gridDetail.CellDoubleClick += new DataGridViewCellEventHandler(gridDetail_CellDoubleClick);
            gridDetail.CellValidating += new DataGridViewCellValidatingEventHandler(gridDetail_CellValidating);
            gridDetail.CellEndEdit += new DataGridViewCellEventHandler(gridDetail_CellEndEdit);
            btnCHDZH.Click += new EventHandler(btnCHDZH_Click);
            btnClear.Click += new EventHandler(btnClear_Click);
        }

        //清除选择的分包项目
        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtCHDZH.Tag = null;
            this.txtCHDZH.Text = "";
        }

        //选择分包项目
        private void btnCHDZH_Click(object sender, EventArgs e)
        {
            VContractExcuteSelector vmros = new VContractExcuteSelector();
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject engineerMaster = list[0] as SubContractProject;
            this.txtCHDZH.Tag = engineerMaster;
            this.txtCHDZH.Text = engineerMaster.BearerOrgName;
        }

        private void gridDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && gridDetail.Rows[e.RowIndex].ReadOnly == false)
            {
                object value = e.FormattedValue;
                if (value != null)
                {
                    try
                    {
                        string colName = gridDetail.Columns[e.ColumnIndex].Name;
                        if (colName == DtlAccountProjectAmount.Name)
                        {
                            if (value.ToString() != "")
                                ClientUtil.ToDecimal(value);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("输入格式不正确！");
                        e.Cancel = true;
                    }
                }
            }
        }

        private void gridDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {

                object tempValue = gridDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                string value = "";
                if (tempValue != null)
                    value = tempValue.ToString().Trim();

                DataGridViewRow currEditRow = gridDetail.Rows[e.RowIndex];
                ProjectTaskDetailAccount dtl = currEditRow.Tag as ProjectTaskDetailAccount;

                if (!string.IsNullOrEmpty(dtl.Id))
                {
                    dtl = GetAccountDtlByCurrAccountBill(dtl);
                }

                if (gridDetail.Columns[e.ColumnIndex].Name == DtlAccountProjectAmount.Name) //核算工程量
                {
                    decimal AccountProjectAmount = 0;

                    if (!string.IsNullOrEmpty(value))
                        AccountProjectAmount = ClientUtil.ToDecimal(value);

                    dtl.AccountProjectAmount = AccountProjectAmount;
                    dtl.AccountTotalPrice = AccountProjectAmount*dtl.AccountPrice;

                    // a）刷新{工程任务明细核算}的以下属性：
                    //【本次核算形象进度】：=【核算工程量】/【计划工程量】
                    //【本次核算挣值】：=【核算工程量】*【计划单价】
                    //【本次合同收入实现量】：=【本次核算形象进度】*【合同工程量】
                    //【本次合同收入合价】：=【本次合同收入实现量】*【合同单价】
                    //【本次责任成本实现量】：=【本次核算形象进度】*【责任工程量】
                    //【本次责任成本合价】：=【本次责任成本实现量】*【责任单价】
                    if (dtl.AccountProjectAmount == 0 || dtl.PlanQuantity == 0)
                        dtl.CurrAccFigureProgress = 0;
                    else
                        dtl.CurrAccFigureProgress = dtl.AccountProjectAmount/dtl.PlanQuantity;

                    dtl.CurrAccEV = dtl.AccountProjectAmount*dtl.PlanPrice;
                    dtl.CurrContractIncomeQny = dtl.CurrAccFigureProgress*dtl.ContractQuantity;
                    dtl.CurrContractIncomeTotal = dtl.CurrContractIncomeQny*dtl.ContractPrice;
                    dtl.CurrResponsibleCostQny = dtl.CurrAccFigureProgress*dtl.ResponsibleQuantity;
                    dtl.CurrResponsibleCostTotal = dtl.CurrResponsibleCostQny*dtl.ResponsiblePrice;
                        // ToDecimailString(dtl.CurrAccFigureProgress);
                    currEditRow.Cells[DtlAccountTotalPrice.Name].Value = ToDecimailString(dtl.AccountTotalPrice);

                    //b）刷新下属{工程资源耗用核算}的以下属性：
                    //【核算耗用数量】={工程任务明细核算}_【核算工程量】*【核算定额数量】；
                    //【本次实现合同收入量】：=<所属{工程任务明细核算}_【本次核算形象进度】>*【合同工程量】
                    //【本次实现合同收入合价】：=【本次实现合同工程量】*【合同数量单价】
                    //【本次责任成本数量】：=<所属{工程任务明细核算}_【本次核算形象进度】>*【责任耗用数量】
                    //【本次责任成本合价】：=【本次责任成本数量】*【责任数量量单价】
                    for (int i = 0; i < dtl.Details.Count; i++)
                    {
                        ProjectTaskDetailAccountSubject subject = dtl.Details.ElementAt(i);

                        subject.AccUsageQny = dtl.AccountProjectAmount*subject.AccountQuantity;
                        subject.CurrContractIncomeQny = dtl.CurrAccFigureProgress*subject.ResContractQuantity;
                        subject.CurrContractIncomeTotal = subject.CurrContractIncomeQny*subject.ContractQuantityPrice;
                        subject.CurrResponsibleCostQny = dtl.CurrAccFigureProgress*subject.ResponsibleUsageQny;
                        subject.CurrResponsibleCostTotal = subject.CurrResponsibleCostQny*subject.ResponsibleQnyPrice;
                    }

                    //c）修改对应的{工程任务明细核算汇总}_【核算工程量】：=相关{工程任务明细核算}_【核算工程量】的和。
                    decimal accountQuantityCount = 0;
                    foreach (ProjectTaskDetailAccount item in curBillMaster.Details)
                    {
                        if (item.ProjectTaskDtlGUID.Id == dtl.ProjectTaskDtlGUID.Id)
                        {
                            accountQuantityCount += item.AccountProjectAmount;
                        }
                    }
                }
                else if (gridDetail.Columns[e.ColumnIndex].Name == DtlRemark.Name)
                {
                    dtl.Remark = value;
                }

                gridDetail.Rows[e.RowIndex].Tag = dtl;

            }
        }

        private void gridDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                ProjectTaskDetailAccount dtl = gridDetail.Rows[e.RowIndex].Tag as ProjectTaskDetailAccount;

                if (!string.IsNullOrEmpty(dtl.Id))
                {
                    dtl = GetAccountDtlByCurrAccountBill(dtl);
                }

                VAccountDetailSubject frm = new VAccountDetailSubject(ref dtl);
                frm.OptionView = ViewState;
                frm.ShowDialog();
            }
        }

        private ProjectTaskDetailAccount GetAccountDtlByCurrAccountBill(ProjectTaskDetailAccount dtl)
        {
            for (int i = 0; i < curBillMaster.Details.Count; i++)
            {
                ProjectTaskDetailAccount item = curBillMaster.Details.ElementAt(i) as ProjectTaskDetailAccount;
                if (dtl.Id == item.Id)
                {
                    dtl = item;
                    break;
                }
            }
            return dtl;
        }

        private string ToDecimailString(decimal value)
        {
            return decimal.Round(value, 3).ToString();
        }

        #region 固定代码
        /// <summary>
        /// 启动本窗体,(设置状态或重新加载已有的数据)
        /// </summary>
        /// <param name="code"></param>
        /// <param name="GUID"></param>
        public void Start(string code, string GUID)
        {
            try
            {
                ObjectQuery oq = new ObjectQuery();
                
                if (!string.IsNullOrEmpty(GUID))
                {
                    oq.AddCriterion(Expression.Eq("Id", GUID));
                }
                else
                {
                    return;
                }

                IList list = model.ObjectQuery(typeof (ProjectTaskAccountBill), oq);
                if (list.Count > 0)
                {
                    curBillMaster = list[0] as ProjectTaskAccountBill;
                    ModelToView();
                    RefreshState(MainViewState.Browser);
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
        }

        #endregion

        /// <summary>
        /// 刷新控件(窗体中的控件)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshControls(MainViewState state)
        {
            base.RefreshControls(state);

            switch (state)
            {
                case MainViewState.AddNew:

                    dtAccountEndDate.Enabled = true;
                    txtRemark.ReadOnly = false;
                    txtCreateDate.Enabled = true;
                    btnSelectAccountTaskRootNode.Enabled = true;
                    btnCHDZH.Enabled = true;
                    btnClear.Enabled = true;
                    btnGeneAccountBill.Enabled = true;
                    SetGridRowReadOnly(false);

                    break;
                case MainViewState.Modify:

                    dtAccountEndDate.Enabled = false;
                    txtRemark.ReadOnly = false;
                    txtCreateDate.Enabled = false;
                    btnSelectAccountTaskRootNode.Enabled = false;
                    btnCHDZH.Enabled = false;
                    btnClear.Enabled = false;
                    btnGeneAccountBill.Enabled = false;
                    SetGridRowReadOnly(false);

                    break;
                case MainViewState.Browser:

                    dtAccountEndDate.Enabled = false;
                    txtRemark.ReadOnly = true;
                    txtCreateDate.Enabled = false;
                    btnSelectAccountTaskRootNode.Enabled = false;
                    btnCHDZH.Enabled = false;
                    btnClear.Enabled = false;
                    btnGeneAccountBill.Enabled = false;
                    SetGridRowReadOnly(true);
                    break;
            }

            txtBillCode.ReadOnly = true;
            txtResponsiblePerson.ReadOnly = true;
            txtTheProject.ReadOnly = true;
            txtCompleteProjectAmount.ReadOnly = true;

            ViewState = state;
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
            if (c is CustomEdit || c is TextBox)
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

                curBillMaster = new ProjectTaskAccountBill();
                curBillMaster.CreateDate = model.GetServerTime();
                curBillMaster.DocState = DocumentState.Edit;
                curBillMaster.AccountPersonGUID =
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;
                curBillMaster.AccountPersonName =
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                curBillMaster.OperOrgInfo =
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg;
                curBillMaster.OperOrgInfoName =
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.Name;
                curBillMaster.OpgSysCode =
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.SysCode;

                if (projectInfo != null)
                {
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
                }

                LoadAccountBillData(curBillMaster);
                RefreshControls(MainViewState.AddNew);
                if (string.IsNullOrEmpty(txtAccountRootNode.Text.Trim()))
                {
                    GetWbsRootNode();
                }
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
            if (curBillMaster != null && curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Suspend)
            {
                base.ModifyView();

                return true;
            }
            MessageBox.Show("此单状态为【" + StaticMethod.GetProjectTaskAccountBillStateText(curBillMaster.DocState) +
                            "】，不能修改！");
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
                return this.SaveOrSubmitBill(1);
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        public override bool SubmitView()
        {
            try
            {
                return this.SaveOrSubmitBill(2);
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }

        //[optrType=1 保存][optrType=2 提交]
        private bool SaveOrSubmitBill(int optrType)
        {
            if (!ValidView())
                return false;

            if (optrType == 2)
            {
                curBillMaster.DocState = DocumentState.InExecute;
            }

            LogData log = new LogData();
            if (string.IsNullOrEmpty(curBillMaster.Id))
            {
                if (optrType == 2)
                {
                    log.OperType = "新增提交";
                }
                else
                {
                    log.OperType = "新增保存";
                }
                curBillMaster.Temp1 = "1";//临时判断用
                curBillMaster = model.SaveAccBillAndSetCfmStateByVirCfmBill(curBillMaster, listGWBSTaskConfirms);
            }
            else
            {
                if (optrType == 2)
                {
                    log.OperType = "修改提交";
                }
                else
                {
                    log.OperType = "修改保存";
                }
                curBillMaster = model.SaveOrUpdateProjectTaskAccount(curBillMaster);
            }
            this.txtBillCode.Text = curBillMaster.Code;
            log.BillId = curBillMaster.Id;
            log.BillType = "工程任务确认单";
            log.Code = curBillMaster.Code;
            log.Descript = "";
            log.OperPerson =
                Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
            log.ProjectName = curBillMaster.ProjectName;
            StaticMethod.InsertLogData(log);

            this.ViewCaption = ViewName + "-" + txtBillCode.Text;

            MessageBox.Show(log.OperType + "成功");
            return true;
        }

        /// <summary>
        /// 保存数据前校验数据
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            if (gridDetail.CurrentCell != null)
            {
                gridDetail.EndEdit();
                gridDetail_CellEndEdit(gridDetail,
                                       new DataGridViewCellEventArgs(gridDetail.CurrentCell.ColumnIndex,
                                                                     gridDetail.CurrentCell.RowIndex));
            }
            curBillMaster.Remark = this.txtRemark.Text.Trim();
            curBillMaster.CreateDate = txtCreateDate.Value;
            return true;
        }

        //显示数据
        private bool ModelToView()
        {
            try
            {
                FlashScreen.Show("正在加载确认单信息,请稍候......");

                txtBillCode.Text = curBillMaster.Code;
                dtAccountEndDate.Value = curBillMaster.EndTime;
                txtCreateDate.Value = curBillMaster.CreateDate;
                txtResponsiblePerson.Text = curBillMaster.AccountPersonName;
                txtRemark.Text = curBillMaster.Remark;

                txtAccountRootNode.Text = curBillMaster.AccountTaskName;

                txtTheProject.Text = curBillMaster.ProjectName;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheAccountBill.Id", curBillMaster.Id));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("ProjectTaskDtlGUID.TheCostItem", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("BearerGUID", NHibernate.FetchMode.Eager);

                IList listDetail = model.ObjectQuery(typeof (ProjectTaskDetailAccount), oq);
                //curBillMaster.ListDetails.Clear();
                curBillMaster.Details = new HashedSet<BaseDetail>();
                curBillMaster.Details.AddAll(listDetail.OfType<ProjectTaskDetailAccount>().ToArray());

                gridDetail.Rows.Clear();

                decimal completeProjectQuantity = 0;
                decimal accountProjectQuantity = 0;
                foreach (ProjectTaskDetailAccount dtl in listDetail)
                {
                    AddAccountDetailInGrid(dtl, ref completeProjectQuantity);

                    accountProjectQuantity += dtl.AccountProjectAmount;
                }

                txtCompleteProjectAmount.Text = completeProjectQuantity.ToString();
                oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheAccountBill.Id", curBillMaster.Id));
                oq.AddFetchMode("ProjectTaskDtlGUID", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("ProjectTaskDtlGUID.TheCostItem", NHibernate.FetchMode.Eager);
                IList listSummary = model.ObjectQuery(typeof (ProjectTaskDetailAccountSummary), oq);

                curBillMaster.ListSummary = new HashedSet<ProjectTaskDetailAccountSummary>();
                curBillMaster.ListSummary.AddAll(listSummary.OfType<ProjectTaskDetailAccountSummary>().ToList());

                return true;
            }
            catch (Exception e)
            {
                FlashScreen.Close();

                //throw e;
                MessageBox.Show("加载数据失败，详细信息：" + StaticMethod.ExceptionMessage(e));
                return false;
            }
            finally
            {

                FlashScreen.Close();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                ProjectTaskAccountBill bill =
                    model.GetObjectById(typeof (ProjectTaskAccountBill), curBillMaster.Id) as ProjectTaskAccountBill;
                if (bill.DocState == DocumentState.Edit)
                {
                    IList list = new ArrayList();
                    list.Add(bill);
                    if (!model.DeleteProjectTaskAccount(list))
                        return false;

                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "工程任务确认单";
                    log.Code = curBillMaster.Code;
                    log.OperType = "删除";
                    log.Descript = "";
                    log.OperPerson =
                        Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                    log.ProjectName = curBillMaster.ProjectName;
                    StaticMethod.InsertLogData(log);

                    ClearView();

                    curBillMaster = null;

                    return true;
                }
                MessageBox.Show("此单状态为【" + StaticMethod.GetProjectTaskAccountBillStateText(bill.DocState) + "】，不能删除！");
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

                        if (!string.IsNullOrEmpty(curBillMaster.Id))
                        {
                            //重新查询数据

                            ObjectQuery oq = new ObjectQuery();
                            oq.AddCriterion(Expression.Eq("Id", curBillMaster.Id));
                            curBillMaster =
                                model.ObjectQuery(typeof (ProjectTaskAccountBill), oq)[0] as ProjectTaskAccountBill;

                            ModelToView();
                        }
                        else
                        {
                            ClearView();
                        }
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
                if (ViewState == MainViewState.Modify)
                {
                    if (
                        MessageBox.Show("当前确认单处于编辑状态，需要保存修改吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                        DialogResult.Yes)
                    {
                        if (!SaveView())
                        {
                            return;
                        }
                    }
                }

                //重新查询加载数据
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", curBillMaster.Id));

                curBillMaster = model.ObjectQuery(typeof (ProjectTaskAccountBill), oq)[0] as ProjectTaskAccountBill;

                ModelToView();

                RefreshControls(MainViewState.Browser);
            }
            catch (Exception e)
            {
                MessageBox.Show("数据刷新错误：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        private void btnGeneAccountBill_Click(object sender, EventArgs e)
        {
            DateTime accountEndTime = dtAccountEndDate.Value.Date.AddDays(1).AddSeconds(-1);

            if (txtAccountRootNode.Text == "")
            {
                MessageBox.Show("请选择确认根节点！");
                btnSelectAccountTaskRootNode.Focus();
                return;
            }

            #region 生成工程任务确认单

            FlashScreen.Show("正在生成确认单,请稍候......");

            #region 1.设置工程任务确认单主表数据

            if (curBillMaster == null)
            {
                curBillMaster = new ProjectTaskAccountBill();
                curBillMaster.CreateDate = model.GetServerTime();

                curBillMaster.DocState = DocumentState.Edit;
                curBillMaster.AccountPersonGUID =
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;
                curBillMaster.AccountPersonName =
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                curBillMaster.OperOrgInfo =
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg;
                curBillMaster.OperOrgInfoName =
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.Name;
                curBillMaster.OpgSysCode =
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.SysCode;

                if (projectInfo != null)
                {
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
                }
            }

            GWBSTree AccountRange = txtAccountRootNode.Tag as GWBSTree;
            curBillMaster.AccountRange = AccountRange;
            curBillMaster.AccountTaskSyscode = AccountRange.SysCode;
            curBillMaster.AccountTaskName = txtAccountRootNode.Text;
            curBillMaster.EndTime = accountEndTime;
            curBillMaster.Remark = txtRemark.Text.Trim();

            if (txtCHDZH.Tag != null)
            {
                curBillMaster.SubContractProjectID = (txtCHDZH.Tag as SubContractProject).Id;
            }

            #endregion

            IList listResult = model.GenAccountBillByVirConfirmBill(curBillMaster);

            string errMes = listResult[2] as string;
            if (!string.IsNullOrEmpty(errMes))
            {
                FlashScreen.Close();
                MessageBox.Show(errMes, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            curBillMaster = listResult[0] as ProjectTaskAccountBill;
            listGWBSTaskConfirms = listResult[1] as List<GWBSTaskConfirm>;
            #endregion

            try
            {                
                ClearAccountBillData();
                LoadAccountBillData(curBillMaster);
                RefreshControls(MainViewState.Modify);

                if (curBillMaster.Details.Count == 0 && curBillMaster.ListSummary.Count == 0)
                {
                    btnSelectAccountTaskRootNode.Enabled = true;
                    btnCHDZH.Enabled = true;
                    btnClear.Enabled = true;
                    btnGeneAccountBill.Enabled = true;
                    dtAccountEndDate.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex), "错误提示", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        private void ClearAccountBillData()
        {
            txtBillCode.Text = "";
            txtResponsiblePerson.Text = "";
            txtRemark.Text = "";
            txtTheProject.Text = "";
            gridDetail.Rows.Clear();
            txtCompleteProjectAmount.Text = "";
        }

        private void LoadAccountBillData(ProjectTaskAccountBill bill)
        {
            txtBillCode.Text = bill.Code;
            txtResponsiblePerson.Text = bill.AccountPersonName;
            txtRemark.Text = bill.Remark;
            txtAccountRootNode.Text = bill.AccountTaskName;
            txtTheProject.Text = bill.ProjectName;
            gridDetail.Rows.Clear();

            if (bill.Details.Count > 0)
            {
                decimal completeProjectQuantity = 0;
                decimal accountProjectQuantity = 0;
                foreach (ProjectTaskDetailAccount dtl in bill.Details)
                {
                    AddAccountDetailInGrid(dtl, ref completeProjectQuantity);

                    accountProjectQuantity += dtl.AccountProjectAmount;
                }

                txtCompleteProjectAmount.Text = completeProjectQuantity.ToString();
            }
        }

        private void AddAccountDetailInGrid(ProjectTaskDetailAccount dtl, ref decimal completeProjectQuantity)
        {
            int index = gridDetail.Rows.Add();
            DataGridViewRow row = gridDetail.Rows[index];

            row.Cells[DtlProjectTaskNode.Name].Value = dtl.AccountTaskNodeName;
            row.Cells[DtlProjectTaskNode.Name].ToolTipText =
                Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetCategorTreeFullPath(
                    typeof (GWBSTree), dtl.AccountTaskNodeName, dtl.AccountTaskNodeSyscode);
            //row.Cells[TaskName.Name].ToolTipText = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.TheGWBSTaskName, dtl.TheGWBSSysCode);

            row.Cells[DtlProjectTaskDetail.Name].Value = dtl.ProjectTaskDtlName;
            row.Cells[DtlTaskBearer.Name].Value = dtl.BearerName;
            if (dtl.BearerGUID != null)
                row.Cells[DtlContractName.Name].Value = dtl.BearerGUID.ContractGroupCode;
            row.Cells[DtlOwner.Name].Value = dtl.ResponsiblePersonName;
            row.Cells[DtlMatFeeBalanceFlag.Name].Value = dtl.MatFeeBlanceFlag.ToString();
            row.Cells[DtlPlanWorkAmount.Name].Value = dtl.PlanQuantity;
            row.Cells[DtlAddupAccQuantity.Name].Value = dtl.AddupAccountQuantity;
            row.Cells[DtlAddupAccProgress.Name].Value = Math.Round(dtl.AddupAccountProgress, 2);
            row.Cells[DtlConfirmProjectAmount.Name].Value = dtl.ConfirmQuantity;
            row.Cells[DtlAccountProjectAmount.Name].Value = dtl.AccountProjectAmount;
            row.Cells[DtlQuantityUnit.Name].Value = dtl.QuantityUnitName;
            row.Cells[DtlAccountPrice.Name].Value = dtl.AccountPrice;
            row.Cells[DtlPriceUnit.Name].Value = dtl.PriceUnitName;
            row.Cells[DtlAccountTotalPrice.Name].Value = dtl.AccountTotalPrice;
            row.Cells[DtlRemark.Name].Value = dtl.Remark;

            if (dtl.ProjectTaskDtlGUID != null)
            {
                row.Cells[DtlMainResourceSpec.Name].Value = dtl.ProjectTaskDtlGUID.MainResourceTypeSpec;
                row.Cells[DtlDigramNumber.Name].Value = dtl.ProjectTaskDtlGUID.DiagramNumber;
            }
            row.Tag = dtl;
            completeProjectQuantity += dtl.ConfirmQuantity;
        }
     
        private decimal FigureProgressRound(decimal value)
        {
            return decimal.Round(value, 3);
        }

        private void btnSelectAccountTaskRootNode_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(txtAccountRootNode.Tag as GWBSTree);
            frm.DefaultSelectedGWBS = txtAccountRootNode.Tag as GWBSTree;
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                TreeNode root = frm.SelectResult[0];

                GWBSTree task = root.Tag as GWBSTree;
                if (task != null)
                {
                    txtAccountRootNode.Text = task.Name;
                    txtAccountRootNode.Tag = task;
                }
            }
        }

        private void GetWbsRootNode()
        {
            if (projectInfo == null)
            {
                return;
            }
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
            objectQuery.AddCriterion(Expression.Eq("ParentNode", null));

            var list = model.ObjectQuery(typeof(GWBSTree), objectQuery).OfType<GWBSTree>().ToList();
            if (list == null || list.Count == 0)
            {
                txtAccountRootNode.Clear();
                txtAccountRootNode.Tag = null;
            }
            else
            {
                txtAccountRootNode.Text = list[0].Name;
                txtAccountRootNode.Tag = list[0];
            }
        }
    }
}
