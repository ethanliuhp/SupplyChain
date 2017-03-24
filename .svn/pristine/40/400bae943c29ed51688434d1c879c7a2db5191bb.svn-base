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
    public partial class VTaskAccountByVirtualConfirmBill : TMasterDetailView
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
        CurrentProjectInfo projectInfo = null;

        public VTaskAccountByVirtualConfirmBill()
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

            //默认上期月度核算的轧帐时间
            //ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            //oq.AddOrder(NHibernate.Criterion.Order.Desc("BillingTime"));
            //oq.FirstResult = 0;
            //oq.MaxResults = 1;

            //IList list = model.ObjectQuery(typeof(ProjectTaskAccountBill), oq);
            //if (list.Count > 0)
            //{
            //    ProjectTaskAccountBill bill = list[0] as ProjectTaskAccountBill;
            //    dtAccountStartTime.Value = bill.EndTime.AddDays(1);
            //    dtAccountEndDate.Value = bill.EndTime.AddMonths(1);
            //}
            //else
            //{
            dtAccountStartTime.Value = serverTime.AddMonths(-1);
            dtAccountEndDate.Value = serverTime;
            //}
        }

        private void SetGridRowReadOnly(bool rowReadOnly)
        {
            //设置编辑的单元格状态
            foreach (DataGridViewRow row in gridDetail.Rows)
            {
                row.ReadOnly = rowReadOnly;
            }

            //设置编辑的单元格状态
            foreach (DataGridViewRow row in gridDetailSummary.Rows)
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
            gridDetail.CellContentClick += new DataGridViewCellEventHandler(gridDetail_CellContentClick);

            gridDetailSummary.CellValidating += new DataGridViewCellValidatingEventHandler(gridDetailSummary_CellValidating);
            gridDetailSummary.CellEndEdit += new DataGridViewCellEventHandler(gridDetailSummary_CellEndEdit);

            btnEditUsageAcc.Click += new EventHandler(btnEditUsageAcc_Click);
            btnAccAccountCost.Click += new EventHandler(btnAccAccountCost_Click);
            btnCHDZH.Click += new EventHandler(btnCHDZH_Click);
            btnClear.Click += new EventHandler(btnClear_Click);
            btnBatchSetBalance.Click+=new EventHandler(btnBatchSetBalance_Click);
        }
        void btnBatchSetBalance_Click( object sender, EventArgs e)
        {
            if (CurBillMaster != null && CurBillMaster.Details.Count > 0)
            {
                VAccountDetailSubjectSetBalState oVAccountDetailSubjectSetBalState = new VAccountDetailSubjectSetBalState(CurBillMaster.Details.OfType<ProjectTaskDetailAccount>().ToList());
                oVAccountDetailSubjectSetBalState.ShowDialog();
                if (oVAccountDetailSubjectSetBalState.IsSave)
                {
                    btnAccAccountCost_Click(null, null);
                }
            }
        }
        //清除选择的分包项目
        void btnClear_Click(object sender, EventArgs e)
        {
            this.txtCHDZH.Tag = null;
            this.txtCHDZH.Text = "";
        }
        //选择分包项目
        void btnCHDZH_Click(object sender, EventArgs e)
        {
            VContractExcuteSelector vmros = new VContractExcuteSelector();
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject engineerMaster = list[0] as SubContractProject;
            this.txtCHDZH.Tag = engineerMaster;
            this.txtCHDZH.Text = engineerMaster.BearerOrgName;
        }

        void gridDetailSummary_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && gridDetailSummary.Rows[e.RowIndex].ReadOnly == false)
            {
                object value = e.FormattedValue;
                if (value != null)
                {
                    try
                    {
                        string colName = gridDetailSummary.Columns[e.ColumnIndex].Name;
                        if (colName == SumAddupAccFigureProgress.Name)//累计核算形象进度
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

        void gridDetailSummary_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                object tempValue = gridDetailSummary.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                string value = "";
                if (tempValue != null)
                    value = tempValue.ToString().Trim();


                ProjectTaskDetailAccountSummary dtl = gridDetailSummary.Rows[e.RowIndex].Tag as ProjectTaskDetailAccountSummary;

                if (!string.IsNullOrEmpty(dtl.Id))
                {
                    for (int i = 0; i < curBillMaster.ListSummary.Count; i++)
                    {
                        ProjectTaskDetailAccountSummary item = curBillMaster.ListSummary.ElementAt(i);
                        if (item.Id == dtl.Id)
                        {
                            dtl = item;
                            break;
                        }
                    }
                }

                if (gridDetailSummary.Columns[e.ColumnIndex].Name == SumAddupAccFigureProgress.Name)//累计核算形象进度
                {
                    if (value == "")
                    {
                        dtl.AddupAccFigureProgress = 0;
                    }
                    else
                    {
                        dtl.AddupAccFigureProgress = ClientUtil.ToDecimal(value);
                    }
                }
                else if (gridDetailSummary.Columns[e.ColumnIndex].Name == SumRemark.Name)//备注
                {
                    dtl.Remark = value;
                }

                gridDetailSummary.Rows[e.RowIndex].Tag = dtl;

            }
        }

        void gridDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
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

        void gridDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
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

                if (gridDetail.Columns[e.ColumnIndex].Name == DtlAccountProjectAmount.Name)//核算工程量
                {
                    decimal AccountProjectAmount = 0;

                    if (!string.IsNullOrEmpty(value))
                        AccountProjectAmount = ClientUtil.ToDecimal(value);

                    dtl.AccountProjectAmount = AccountProjectAmount;
                    dtl.AccountTotalPrice = AccountProjectAmount * dtl.AccountPrice;

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
                        dtl.CurrAccFigureProgress = dtl.AccountProjectAmount / dtl.PlanQuantity;

                    dtl.CurrAccEV = dtl.AccountProjectAmount * dtl.PlanPrice;
                    dtl.CurrContractIncomeQny = dtl.CurrAccFigureProgress * dtl.ContractQuantity;
                    dtl.CurrContractIncomeTotal = dtl.CurrContractIncomeQny * dtl.ContractPrice;
                    dtl.CurrResponsibleCostQny = dtl.CurrAccFigureProgress * dtl.ResponsibleQuantity;
                    dtl.CurrResponsibleCostTotal = dtl.CurrResponsibleCostQny * dtl.ResponsiblePrice;

                    currEditRow.Cells[DtlCurrAccFigureProgress.Name].Value = StaticMethod.DecimelTrimEnd0(FigureProgressRound(dtl.CurrAccFigureProgress * 100));// ToDecimailString(dtl.CurrAccFigureProgress);
                    currEditRow.Cells[DtlAccountTotalPrice.Name].Value = ToDecimailString(dtl.AccountTotalPrice);

                    //currEditRow.Cells[DtlCurrAccEV.Name].Value = ToDecimailString(dtl.CurrAccEV);
                    //currEditRow.Cells[DtlCurrContractIncomeQny.Name].Value = ToDecimailString(dtl.CurrContractIncomeQny);
                    //currEditRow.Cells[DtlCurrContractIncomeTotal.Name].Value = ToDecimailString(dtl.CurrContractIncomeTotal);
                    //currEditRow.Cells[DtlCurrResponsibleCostQny.Name].Value = ToDecimailString(dtl.CurrResponsibleCostQny);
                    //currEditRow.Cells[DtlCurrResponsibleTotal.Name].Value = ToDecimailString(dtl.CurrResponsibleCostTotal);

                    //b）刷新下属{工程资源耗用核算}的以下属性：
                    //【核算耗用数量】={工程任务明细核算}_【核算工程量】*【核算定额数量】；
                    //【本次实现合同收入量】：=<所属{工程任务明细核算}_【本次核算形象进度】>*【合同工程量】
                    //【本次实现合同收入合价】：=【本次实现合同工程量】*【合同数量单价】
                    //【本次责任成本数量】：=<所属{工程任务明细核算}_【本次核算形象进度】>*【责任耗用数量】
                    //【本次责任成本合价】：=【本次责任成本数量】*【责任数量量单价】
                    for (int i = 0; i < dtl.Details.Count; i++)
                    {
                        ProjectTaskDetailAccountSubject subject = dtl.Details.ElementAt(i);

                        subject.AccUsageQny = dtl.AccountProjectAmount * subject.AccountQuantity;
                        subject.CurrContractIncomeQny = dtl.CurrAccFigureProgress * subject.ResContractQuantity;
                        subject.CurrContractIncomeTotal = subject.CurrContractIncomeQny * subject.ContractQuantityPrice;
                        subject.CurrResponsibleCostQny = dtl.CurrAccFigureProgress * subject.ResponsibleUsageQny;
                        subject.CurrResponsibleCostTotal = subject.CurrResponsibleCostQny * subject.ResponsibleQnyPrice;
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
                    for (int i = 0; i < curBillMaster.ListSummary.Count; i++)
                    {
                        ProjectTaskDetailAccountSummary sum = curBillMaster.ListSummary.ElementAt(i);
                        if (sum.ProjectTaskDtlGUID.Id == dtl.ProjectTaskDtlGUID.Id)
                        {
                            sum.AccountProjectAmount = accountQuantityCount;
                            sum.AddupAccQuantity = sum.ProjectTaskDtlGUID.AddupAccQuantity + sum.AccountProjectAmount;

                            foreach (DataGridViewRow sumRow in gridDetailSummary.Rows)
                            {
                                ProjectTaskDetailAccountSummary tempSum = sumRow.Tag as ProjectTaskDetailAccountSummary;
                                if (!string.IsNullOrEmpty(sum.Id) && tempSum.Id == sum.Id)
                                {
                                    sumRow.Cells[SumCurrAccountProjectAmount.Name].Value = ToDecimailString(sum.AccountProjectAmount);
                                    sumRow.Cells[SumAddupAccProjectAmount.Name].Value = ToDecimailString(sum.AddupAccQuantity);
                                    break;
                                }
                                else if (tempSum.ProjectTaskDtlGUID.Id == sum.ProjectTaskDtlGUID.Id)
                                {
                                    sumRow.Cells[SumCurrAccountProjectAmount.Name].Value = ToDecimailString(sum.AccountProjectAmount);
                                    sumRow.Cells[SumAddupAccProjectAmount.Name].Value = ToDecimailString(sum.AddupAccQuantity);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    txtAccountProjectAmount.Text = accountQuantityCount.ToString();
                }
                else if (gridDetail.Columns[e.ColumnIndex].Name == DtlRemark.Name)
                {
                    dtl.Remark = value;
                }

                gridDetail.Rows[e.RowIndex].Tag = dtl;

            }
        }

        void gridDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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

                //计算核算成本
                btnAccAccountCost_Click(btnAccAccountCost, new EventArgs());

                //ProjectTaskDetailAccount tempDtl = frm.optAccountDtl;

                ////统计合价
                //decimal accountTotalPrice = 0;
                //foreach (ProjectTaskDetailAccountSubject subject in tempDtl.Details)
                //{
                //    accountTotalPrice += subject.AccountTotalPrice;
                //}
                //dtl.AccountTotalPrice = accountTotalPrice;

                //if (dtl.AccountTotalPrice != 0 && dtl.AccountProjectAmount != 0)
                //    dtl.AccountPrice = decimal.Round(dtl.AccountTotalPrice / dtl.AccountProjectAmount, 3);


                //gridDetail.Rows[e.RowIndex].Cells[DtlAccountTotalPrice.Name].Value = Decimal.Round(dtl.AccountTotalPrice, 3);
                //gridDetail.Rows[e.RowIndex].Cells[DtlAccountPrice.Name].Value = dtl.AccountPrice;
                //gridDetail.Rows[e.RowIndex].Tag = dtl;


                ////汇总合价
                //decimal totalPriceCount = 0;
                //foreach (ProjectTaskDetailAccount item in curBillMaster.Details)
                //{
                //    if (item.ProjectTaskDtlGUID == dtl.ProjectTaskDtlGUID)
                //    {
                //        totalPriceCount += item.AccountTotalPrice;
                //    }
                //}

                //ProjectTaskDetailAccountSummary optSum = null;
                //foreach (ProjectTaskDetailAccountSummary sum in curBillMaster.ListSummary)
                //{
                //    if (sum.ProjectTaskDtlGUID.Id == dtl.ProjectTaskDtlGUID.Id)
                //    {
                //        sum.AccountTotalPrice = totalPriceCount;
                //        optSum = sum;
                //        break;
                //    }
                //}
                //foreach (DataGridViewRow row in gridDetailSummary.Rows)
                //{
                //    ProjectTaskDetailAccountSummary sum = row.Tag as ProjectTaskDetailAccountSummary;
                //    if (sum.ProjectTaskDtlGUID.Id == optSum.ProjectTaskDtlGUID.Id)
                //    {
                //        row.Cells[SumAccountTotalPrice.Name].Value = decimal.Round(optSum.AccountTotalPrice, 3);
                //        row.Tag = optSum;
                //        break;
                //    }
                //}
            }
        }
        void gridDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                if (e.ColumnIndex == gridDetail.Rows[e.RowIndex].Cells[DtlAppOpinion.Name].ColumnIndex)
                {
                    ProjectTaskDetailAccount accDtl = gridDetail.SelectedRows[0].Tag as ProjectTaskDetailAccount;
                    ObjectQuery oq = new ObjectQuery();
                    List<GWBSTaskConfirmMaster> list = new List<GWBSTaskConfirmMaster>();
                    //GWBSTaskConfirm g = new GWBSTaskConfirm();
                    //g.TaskHandler
                    if (string.IsNullOrEmpty(curBillMaster.Id))
                    {
                        oq.Criterions.Clear();
                        oq.AddCriterion(Expression.Eq("AccountingState", EnumGWBSTaskConfirmAccountingState.未核算));
                        oq.AddCriterion(Expression.Eq("CostItem.Id", accDtl.ProjectTaskDtlGUID.TheCostItem.Id));
                        oq.AddCriterion(Expression.Eq("GWBSDetail.MainResourceTypeId", accDtl.ProjectTaskDtlGUID.MainResourceTypeId));
                        oq.AddCriterion(Expression.Eq("GWBSDetail.DiagramNumber", accDtl.ProjectTaskDtlGUID.DiagramNumber));
                        oq.AddCriterion(Expression.Eq("TaskHandler.Id", accDtl.BearerGUID.Id));
                        oq.AddCriterion(Expression.Eq("MaterialFeeSettlementFlag", accDtl.MatFeeBlanceFlag));
                        oq.AddCriterion(Expression.Eq("Master.ConfirmHandlePerson.Id", accDtl.ResponsiblePerson.Id));
                        oq.AddCriterion(Expression.Like("GwbsSysCode", accDtl.AccountTaskNodeSyscode, MatchMode.Start));
                    }
                    else
                    {
                        oq.Criterions.Clear();
                        oq.AddCriterion(Expression.Eq("AccountingDetailId", accDtl.Id));
                        oq.AddCriterion(Expression.Eq("AccountingState", EnumGWBSTaskConfirmAccountingState.己核算));

                    }
                    oq.AddFetchMode("Master.Details", NHibernate.FetchMode.Eager);
                    IList conList = model.ObjectQuery(typeof(GWBSTaskConfirm), oq);
                    foreach (GWBSTaskConfirm c in conList)
                    {
                        list.Add(c.Master);
                    }
                    list = list.Distinct().ToList<GWBSTaskConfirmMaster>();
                    VAppOpinionGWBSConfirm frm = new VAppOpinionGWBSConfirm((IList)list);
                    frm.ShowDialog();
                }
            }
        }


        void btnEditUsageAcc_Click(object sender, EventArgs e)
        {
            if (gridDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择一条核算明细信息！");
                return;
            }

            ProjectTaskDetailAccount dtl = gridDetail.SelectedRows[0].Tag as ProjectTaskDetailAccount;

            if (!string.IsNullOrEmpty(dtl.Id))
            {
                for (int i = 0; i < curBillMaster.Details.Count; i++)
                {
                    ProjectTaskDetailAccount item = curBillMaster.Details.ElementAt(0) as ProjectTaskDetailAccount;
                    if (dtl.Id == item.Id)
                    {
                        dtl = item;
                        break;
                    }
                }
            }

            VAccountDetailSubject frm = new VAccountDetailSubject(ref dtl);
            frm.ShowDialog();

            btnAccAccountCost_Click(btnAccAccountCost, new EventArgs());
        }

        //计算核算成本
        void btnAccAccountCost_Click(object sender, EventArgs e)
        {
            AccountCostInfo();
        }

        private void AccountCostInfo()
        {
            //i> <所属{工程任务明细核算}>_【核算单价】=下属{工程资源耗用核算}_【核算工程量单价】之和；
            //ii> <所属{工程任务明细核算}>_【核算合价】=<所属{工程任务明细核算}>_【核算工程量】*<所属{工程任务明细核算}>_【核算单价】
            //iii> 针对<所属{工程任务明细核算}>下属{工程资源耗用核算}中每个对象进行以下计算：
            //{工程资源耗用核算}_【核算耗用量】=<所属{工程任务明细核算}>_【核算工程量】*{工程资源耗用核算}_【核算定额数量】
            //{工程资源耗用核算}_【核算耗用合价】={工程资源耗用核算}_【核算耗用量】*{工程资源耗用核算}_【核算数量单价】

            for (int i = 0; i < gridDetail.Rows.Count; i++)
            {
                DataGridViewRow dtlRow = gridDetail.Rows[i];
                ProjectTaskDetailAccount dtl = dtlRow.Tag as ProjectTaskDetailAccount;

                if (!string.IsNullOrEmpty(dtl.Id))
                {
                    dtl = GetAccountDtlByCurrAccountBill(dtl);
                }

                //i
                decimal accountPrice = 0;
                for (int j = 0; j < dtl.Details.Count; j++)
                {
                    ProjectTaskDetailAccountSubject subject = dtl.Details.ElementAt(j);
                    if (subject.IsBalance)
                        accountPrice += subject.AccWorkQnyPrice;
                }
                dtl.AccountPrice = accountPrice;
                dtlRow.Cells[DtlAccountPrice.Name].Value = ToDecimailString(accountPrice);

                //ii
                dtl.AccountTotalPrice = dtl.AccountProjectAmount * dtl.AccountPrice;
                dtlRow.Cells[DtlAccountTotalPrice.Name].Value = ToDecimailString(dtl.AccountTotalPrice);

                //iii
                for (int j = 0; j < dtl.Details.Count; j++)
                {
                    ProjectTaskDetailAccountSubject subject = dtl.Details.ElementAt(j);
                    subject.AccUsageQny = dtl.AccountProjectAmount * subject.AccountQuantity;
                    subject.AccountTotalPrice = subject.AccUsageQny * subject.AccountPrice;
                }

              //  dtlRow.Cells[DtlMatFeeBalanceFlag.Name].Value = GetIsBalance(dtl);
                dtlRow.Tag = dtl;
            }

            //汇总合价
            foreach (DataGridViewRow row in gridDetailSummary.Rows)
            {
                ProjectTaskDetailAccountSummary optSum = row.Tag as ProjectTaskDetailAccountSummary;
                for (int i = 0; i < curBillMaster.ListSummary.Count; i++)
                {
                    ProjectTaskDetailAccountSummary item = curBillMaster.ListSummary.ElementAt(i);
                    if (optSum.Id == item.Id && optSum.ProjectTaskDtlGUID.Id == item.ProjectTaskDtlGUID.Id)//当optSum的id为null时第二个条件起作用
                    {
                        optSum = item;
                        break;
                    }
                }

                decimal totalPriceSum = 0;
                foreach (ProjectTaskDetailAccount dtl in curBillMaster.Details)
                {
                    if (dtl.ProjectTaskDtlGUID.Id == optSum.ProjectTaskDtlGUID.Id)
                    {
                        totalPriceSum += dtl.AccountTotalPrice;
                    }
                }

                optSum.AccountTotalPrice = totalPriceSum;

                row.Cells[SumAccountTotalPrice.Name].Value = ToDecimailString(optSum.AccountTotalPrice);
                row.Tag = optSum;
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
                }else if (!string.IsNullOrEmpty(code))
                {
                    oq.AddCriterion(Expression.Eq("Code", code));
                }
                else
                {
                    return;
                }

                IList list = model.ObjectQuery(typeof(ProjectTaskAccountBill), oq);
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

                    dtAccountStartTime.Enabled = true;
                    dtAccountEndDate.Enabled = true;
                    txtRemark.ReadOnly = false;
                    txtCreateDate.Enabled = true;
                    btnSelectAccountTaskRootNode.Enabled = true;
                    btnCHDZH.Enabled = true;
                    btnClear.Enabled = true;
                    btnGeneAccountBill.Enabled = true;

                    btnEditUsageAcc.Enabled = true;
                    btnAccAccountCost.Enabled = true;
                    btnBatchSetBalance.Enabled = true;
                    SetGridRowReadOnly(false);

                    break;
                case MainViewState.Modify:

                    dtAccountStartTime.Enabled = false;
                    dtAccountEndDate.Enabled = false;
                    txtRemark.ReadOnly = false;
                    txtCreateDate.Enabled = false;
                    btnSelectAccountTaskRootNode.Enabled = false;
                    btnCHDZH.Enabled = false;
                    btnClear.Enabled = false;
                    btnGeneAccountBill.Enabled = false;

                    btnEditUsageAcc.Enabled = true;
                    btnAccAccountCost.Enabled = true;
                    btnBatchSetBalance.Enabled = true;
                    SetGridRowReadOnly(false);

                    break;
                case MainViewState.Browser:

                    dtAccountStartTime.Enabled = false;
                    dtAccountEndDate.Enabled = false;
                    txtRemark.ReadOnly = true;
                    txtCreateDate.Enabled = false;
                    btnSelectAccountTaskRootNode.Enabled = false;
                    btnCHDZH.Enabled = false;
                    btnClear.Enabled = false;
                    btnGeneAccountBill.Enabled = false;

                    btnEditUsageAcc.Enabled = false;
                    btnAccAccountCost.Enabled = false;
                    btnBatchSetBalance.Enabled = false;
                    SetGridRowReadOnly(true);
                    break;
            }

            txtBillCode.ReadOnly = true;
            txtResponsiblePerson.ReadOnly = true;
            txtTheProject.ReadOnly = true;
            txtCompleteProjectAmount.ReadOnly = true;
            txtAccountProjectAmount.ReadOnly = true;

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
                curBillMaster.AccountPersonGUID = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;
                curBillMaster.AccountPersonName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                curBillMaster.OperOrgInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg;
                curBillMaster.OperOrgInfoName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.Name;
                curBillMaster.OpgSysCode = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.SysCode;

                if (projectInfo != null)
                {
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
                }

                LoadAccountBillData(curBillMaster);
                RefreshControls(MainViewState.AddNew);
                dtAccountStartTime.Focus();
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
            if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Suspend)
            {
                base.ModifyView();

                //ObjectQuery oq = new ObjectQuery();
                //oq.AddCriterion(Expression.Eq("Id", curBillMaster.Id));
                ////oq.AddFetchMode("ListDetails", NHibernate.FetchMode.Eager);
                ////oq.AddFetchMode("ListSummary", NHibernate.FetchMode.Eager);

                //curBillMaster = model.ObjectQuery(typeof(ProjectTaskAccountBill), oq)[0] as ProjectTaskAccountBill;

                //ModelToView();

                return true;
            }
            MessageBox.Show("此单状态为【" + StaticMethod.GetProjectTaskAccountBillStateText(curBillMaster.DocState) + "】，不能修改！");
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
                if (curBillMaster.DocState == DocumentState.Edit)
                    curBillMaster.DocState = DocumentState.InAudit;
                else if (curBillMaster.DocState == DocumentState.Suspend)
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
            log.BillType = "工程任务核算单";
            log.Code = curBillMaster.Code;
            log.Descript = "";
            log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
            log.ProjectName = curBillMaster.ProjectName;
            StaticMethod.InsertLogData(log);

            this.ViewCaption = ViewName + "-" + txtBillCode.Text;

            return true;
        }

        /// <summary>
        /// 保存数据前校验数据
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            btnAccAccountCost_Click(btnAccAccountCost, new EventArgs());

            if (gridDetail.CurrentCell != null)
            {
                gridDetail.EndEdit();
                gridDetail_CellEndEdit(gridDetail, new DataGridViewCellEventArgs(gridDetail.CurrentCell.ColumnIndex, gridDetail.CurrentCell.RowIndex));
            }
            if (gridDetailSummary.CurrentCell != null)
            {
                gridDetailSummary.EndEdit();
                gridDetailSummary_CellEndEdit(gridDetailSummary, new DataGridViewCellEventArgs(gridDetailSummary.CurrentCell.ColumnIndex, gridDetailSummary.CurrentCell.RowIndex));
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
                FlashScreen.Show("正在加载核算单信息,请稍候......");

                txtBillCode.Text = curBillMaster.Code;
                dtAccountStartTime.Value = curBillMaster.BeginTime;
                dtAccountEndDate.Value = curBillMaster.EndTime;
                txtCreateDate.Value = curBillMaster.CreateDate;
                txtResponsiblePerson.Text = curBillMaster.AccountPersonName;
                txtRemark.Text = curBillMaster.Remark;

                txtAccountRootNode.Text = curBillMaster.AccountTaskName;

                txtTheProject.Text = curBillMaster.ProjectName;

                //if (curBillMaster.ListDetails.Count > 0)
                //{
                ObjectQuery oq = new ObjectQuery();
                //Disjunction dis = new Disjunction();
                //foreach (ProjectTaskDetailAccount dtl in curBillMaster.ListDetails)
                //{
                //    dis.Add(Expression.Eq("Id", dtl.Id));
                //}
                //oq.AddCriterion(dis);
                oq.AddCriterion(Expression.Eq("TheAccountBill.Id", curBillMaster.Id));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("ProjectTaskDtlGUID.TheCostItem", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("BearerGUID", NHibernate.FetchMode.Eager);

                IList listDetail = model.ObjectQuery(typeof(ProjectTaskDetailAccount), oq);


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
                txtAccountProjectAmount.Text = accountProjectQuantity.ToString();
                //}

                //if (curBillMaster.ListSummary.Count > 0)
                //{
                oq = new ObjectQuery();
                //Disjunction dis = new Disjunction();
                //foreach (ProjectTaskDetailAccountSummary summary in curBillMaster.ListSummary)
                //{
                //    dis.Add(Expression.Eq("Id", summary.Id));
                //}
                //oq.AddCriterion(dis);
                oq.AddCriterion(Expression.Eq("TheAccountBill.Id", curBillMaster.Id));
                oq.AddFetchMode("ProjectTaskDtlGUID", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("ProjectTaskDtlGUID.TheCostItem", NHibernate.FetchMode.Eager);
                IList listSummary = model.ObjectQuery(typeof(ProjectTaskDetailAccountSummary), oq);

                //curBillMaster.ListSummary.Clear();
                curBillMaster.ListSummary = new HashedSet<ProjectTaskDetailAccountSummary>();
                curBillMaster.ListSummary.AddAll(listSummary.OfType<ProjectTaskDetailAccountSummary>().ToList());

                gridDetailSummary.Rows.Clear();
                foreach (ProjectTaskDetailAccountSummary summary in listSummary)
                {
                    AddAccountDetailSummaryInGrid(summary);
                }
                //}

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
                ProjectTaskAccountBill bill = model.GetObjectById(typeof(ProjectTaskAccountBill), curBillMaster.Id) as ProjectTaskAccountBill;
                if (bill.DocState == DocumentState.Edit)
                {
                    IList list = new ArrayList();
                    list.Add(bill);
                    if (!model.DeleteProjectTaskAccount(list))
                        return false;

                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "工程任务核算单";
                    log.Code = curBillMaster.Code;
                    log.OperType = "删除";
                    log.Descript = "";
                    log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
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
                            //oq.AddFetchMode("ListDetails", NHibernate.FetchMode.Eager);
                            //oq.AddFetchMode("ListSummary", NHibernate.FetchMode.Eager);

                            curBillMaster = model.ObjectQuery(typeof(ProjectTaskAccountBill), oq)[0] as ProjectTaskAccountBill;

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
                    if (MessageBox.Show("当前核算单处于编辑状态，需要保存修改吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                //oq.AddFetchMode("ListDetails", NHibernate.FetchMode.Eager);
                //oq.AddFetchMode("ListSummary", NHibernate.FetchMode.Eager);

                curBillMaster = model.ObjectQuery(typeof(ProjectTaskAccountBill), oq)[0] as ProjectTaskAccountBill;

                ModelToView();

                RefreshControls(MainViewState.Browser);
            }
            catch (Exception e)
            {
                MessageBox.Show("数据刷新错误：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        void btnGeneAccountBill_Click(object sender, EventArgs e)
        {
            DateTime accountStartTime = dtAccountStartTime.Value.Date;
            DateTime accountEndTime = dtAccountEndDate.Value.Date.AddDays(1).AddSeconds(-1);

            if (accountStartTime > accountEndTime)
            {
                MessageBox.Show("核算起始时间不能大于结束时间！");
                dtAccountEndDate.Focus();
                return;
            }
            if (txtAccountRootNode.Text == "")
            {
                MessageBox.Show("请选择核算根节点！");
                btnSelectAccountTaskRootNode.Focus();
                return;
            }

            #region 生成工程任务核算单

            FlashScreen.Show("正在生成核算单,请稍候......");

            #region 1.设置工程任务核算单主表数据

            if (curBillMaster == null)
            {
                curBillMaster = new ProjectTaskAccountBill();
                curBillMaster.CreateDate = model.GetServerTime();

                curBillMaster.DocState = DocumentState.Edit;
                curBillMaster.AccountPersonGUID = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;
                curBillMaster.AccountPersonName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                curBillMaster.OperOrgInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg;
                curBillMaster.OperOrgInfoName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.Name;
                curBillMaster.OpgSysCode = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.SysCode;

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
            curBillMaster.BeginTime = accountStartTime;
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
                //保存数据
                //bill = model.SaveOrUpdateProjectTaskAccount(bill);

                ClearAccountBillData();
                LoadAccountBillData(curBillMaster);
                RefreshControls(MainViewState.Modify);

                if (curBillMaster.Details.Count == 0 && curBillMaster.ListSummary.Count == 0)
                {
                    btnSelectAccountTaskRootNode.Enabled = true;
                    btnCHDZH.Enabled = true;
                    btnClear.Enabled = true;
                    btnGeneAccountBill.Enabled = true;
                    dtAccountStartTime.Enabled = true;
                    dtAccountEndDate.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            gridDetailSummary.Rows.Clear();

            txtCompleteProjectAmount.Text = "";
            txtAccountProjectAmount.Text = "";
        }

        private void LoadAccountBillData(ProjectTaskAccountBill bill)
        {
            txtBillCode.Text = bill.Code;
            txtResponsiblePerson.Text = bill.AccountPersonName;
            txtRemark.Text = bill.Remark;
            txtAccountRootNode.Text = bill.AccountTaskName;

            txtTheProject.Text = bill.ProjectName;

            gridDetail.Rows.Clear();
            gridDetailSummary.Rows.Clear();

            if (bill.Details.Count > 0)
            {
                //IList listDetail = bill.ListDetails.ToList();

                //if (!string.IsNullOrEmpty(bill.Id))
                //{
                //    ObjectQuery oq = new ObjectQuery();
                //    Disjunction dis = new Disjunction();
                //    foreach (ProjectTaskDetailAccount dtl in bill.ListDetails)
                //    {
                //        dis.Add(Expression.Eq("Id", dtl.Id));
                //    }
                //    oq.AddCriterion(dis);
                //    oq.AddFetchMode("ProjectTaskDtlGUID", NHibernate.FetchMode.Eager);
                //    //oq.AddFetchMode("BearerGUID", NHibernate.FetchMode.Eager);

                //    listDetail = model.ObjectQuery(typeof(ProjectTaskDetailAccount), oq);
                //}



                decimal completeProjectQuantity = 0;
                decimal accountProjectQuantity = 0;
                foreach (ProjectTaskDetailAccount dtl in bill.Details)
                {
                    AddAccountDetailInGrid(dtl, ref completeProjectQuantity);

                    accountProjectQuantity += dtl.AccountProjectAmount;
                }

                txtCompleteProjectAmount.Text = completeProjectQuantity.ToString();
                txtAccountProjectAmount.Text = accountProjectQuantity.ToString();
            }

            if (bill.ListSummary.Count > 0)
            {
                //IList listSummary = bill.ListSummary.ToList();
                //if (!string.IsNullOrEmpty(bill.Id))
                //{
                //    ObjectQuery oq = new ObjectQuery();
                //    Disjunction dis = new Disjunction();
                //    foreach (ProjectTaskDetailAccountSummary summary in bill.ListSummary)
                //    {
                //        dis.Add(Expression.Eq("Id", summary.Id));
                //    }
                //    oq.AddCriterion(dis);
                //    oq.AddFetchMode("ProjectTaskDtlGUID", NHibernate.FetchMode.Eager);

                //    listSummary = model.ObjectQuery(typeof(ProjectTaskDetailAccountSummary), oq);
                //}


                foreach (ProjectTaskDetailAccountSummary summary in bill.ListSummary)
                {
                    AddAccountDetailSummaryInGrid(summary);
                }
            }
        }
        private void AddAccountDetailInGrid(ProjectTaskDetailAccount dtl, ref decimal completeProjectQuantity)
        {
            int index = gridDetail.Rows.Add();
            DataGridViewRow row = gridDetail.Rows[index];

            row.Cells[DtlProjectTaskNode.Name].Value = dtl.AccountTaskNodeName;
            row.Cells[DtlProjectTaskNode.Name].ToolTipText = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.AccountTaskNodeName, dtl.AccountTaskNodeSyscode);
            //row.Cells[TaskName.Name].ToolTipText = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.TheGWBSTaskName, dtl.TheGWBSSysCode);

            row.Cells[DtlProjectTaskDetail.Name].Value = dtl.ProjectTaskDtlName;

            row.Cells[DtlTaskBearer.Name].Value = dtl.BearerName;
            if (dtl.BearerGUID != null)
                row.Cells[DtlContractName.Name].Value = dtl.BearerGUID.ContractGroupCode;
            row.Cells[DtlOwner.Name].Value = dtl.ResponsiblePersonName;

            row.Cells[DtlMatFeeBalanceFlag.Name].Value = dtl.MatFeeBlanceFlag.ToString();
           // row.Cells[DtlMatFeeBalanceFlag.Name].Value = GetIsBalance(dtl);
            //row.Cells[DtlContractProjectAmount.Name].Value = ToDecimailString(dtl.ContractQuantity);
            //row.Cells[DtlContractPrice.Name].Value = ToDecimailString(dtl.ContractPrice);
            //row.Cells[DtlContractTotalPrice.Name].Value = ToDecimailString(dtl.ContractTotalPrice);

            //row.Cells[DtlResponsibleProjectAmount.Name].Value = ToDecimailString(dtl.ResponsibleQuantity);
            //row.Cells[DtlResponsiblePrice.Name].Value = ToDecimailString(dtl.ResponsiblePrice);
            //row.Cells[DtlResponsibleTotalPrice.Name].Value = ToDecimailString(dtl.ResponsibleTotalPrice);

            //row.Cells[DtlPlanProjectAmount.Name].Value = ToDecimailString(dtl.PlanQuantity);
            //row.Cells[DtlPlanPrice.Name].Value = ToDecimailString(dtl.PlanPrice);
            //row.Cells[DtlPlanTotalPrice.Name].Value = ToDecimailString(dtl.PlanTotalPrice);
            row.Cells[DtlPlanWorkAmount.Name].Value = dtl.PlanQuantity;
            row.Cells[DtlAddupAccQuantity.Name].Value = dtl.AddupAccountQuantity;
            row.Cells[DtlAddupAccProgress.Name].Value = dtl.AddupAccountProgress;
            row.Cells[DtlConfirmProjectAmount.Name].Value = dtl.ConfirmQuantity;
            row.Cells[DtlAccountProjectAmount.Name].Value = dtl.AccountProjectAmount;
            row.Cells[DtlQuantityUnit.Name].Value = dtl.QuantityUnitName;

            row.Cells[DtlAccountPrice.Name].Value = dtl.AccountPrice;
            row.Cells[DtlPriceUnit.Name].Value = dtl.PriceUnitName;
            row.Cells[DtlAccountTotalPrice.Name].Value = dtl.AccountTotalPrice;

            row.Cells[DtlCurrAccFigureProgress.Name].Value = StaticMethod.DecimelTrimEnd0(FigureProgressRound(dtl.CurrAccFigureProgress * 100));
            //row.Cells[DtlCurrAccEV.Name].Value = ToDecimailString(dtl.CurrAccEV);

            //row.Cells[DtlCurrContractIncomeQny.Name].Value = ToDecimailString(dtl.CurrContractIncomeQny);
            //row.Cells[DtlCurrContractIncomeTotal.Name].Value = ToDecimailString(dtl.CurrContractIncomeTotal);

            //row.Cells[DtlCurrResponsibleCostQny.Name].Value = ToDecimailString(dtl.CurrResponsibleCostQny);
            //row.Cells[DtlCurrResponsibleTotal.Name].Value = ToDecimailString(dtl.CurrResponsibleCostTotal);

            row.Cells[DtlRemark.Name].Value = dtl.Remark;
            row.Cells[DtlAppOpinion.Name].Value = "审批意见";

            if (dtl.ProjectTaskDtlGUID != null)
            {
                row.Cells[DtlMainResourceName.Name].Value = dtl.ProjectTaskDtlGUID.MainResourceTypeName;
                row.Cells[DtlMainResourceSpec.Name].Value = dtl.ProjectTaskDtlGUID.MainResourceTypeSpec;
                row.Cells[DtlDigramNumber.Name].Value = dtl.ProjectTaskDtlGUID.DiagramNumber;
                if (dtl.ProjectTaskDtlGUID.TheCostItem != null)
                    row.Cells[DtlCostItemQuotaCode.Name].Value = dtl.ProjectTaskDtlGUID.TheCostItem.QuotaCode;
            }

            row.Tag = dtl;

            completeProjectQuantity += dtl.ConfirmQuantity;
        }

        private void AddAccountDetailSummaryInGrid(ProjectTaskDetailAccountSummary dtl)
        {
            int index = gridDetailSummary.Rows.Add();
            DataGridViewRow row = gridDetailSummary.Rows[index];

            row.Cells[SumProjectTaskNode.Name].Value = dtl.AccountNodeName;
            row.Cells[SumProjectTaskNode.Name].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.AccountNodeName, dtl.AccountNodeSysCode);

            row.Cells[SumProjectTaskDetail.Name].Value = dtl.ProjectTaskDtlName;
            row.Cells[SumPlanQuantity.Name].Value = dtl.ProjectTaskDtlGUID.PlanWorkAmount;

            row.Cells[SumCurrAccountProjectAmount.Name].Value = dtl.AccountProjectAmount;
            row.Cells[SumCurrAccFigureProgress.Name].Value = StaticMethod.DecimelTrimEnd0(FigureProgressRound(dtl.CurrAccFigureProgress));

            row.Cells[SumAddupAccProjectAmount.Name].Value = dtl.AddupAccQuantity;
            row.Cells[SumAddupAccFigureProgress.Name].Value = StaticMethod.DecimelTrimEnd0(FigureProgressRound(dtl.AddupAccFigureProgress));

            row.Cells[SumAccountTotalPrice.Name].Value = dtl.AccountTotalPrice;

            row.Cells[SumProjectAmountUnit.Name].Value = dtl.ProjectAmountUnitName;
            row.Cells[SumPriceUnit.Name].Value = dtl.PriceUnitName;

            row.Cells[SumState.Name].Value = dtl.State.ToString();
            row.Cells[SumRemark.Name].Value = dtl.Remark;

            if (dtl.ProjectTaskDtlGUID != null)
            {
                row.Cells[SumMainResourceName.Name].Value = dtl.ProjectTaskDtlGUID.MainResourceTypeName;
                row.Cells[SumMainResourceSpec.Name].Value = dtl.ProjectTaskDtlGUID.MainResourceTypeSpec;
                row.Cells[SumDigramNumber.Name].Value = dtl.ProjectTaskDtlGUID.DiagramNumber;

                if (dtl.ProjectTaskDtlGUID.TheCostItem != null)
                    row.Cells[SumCostItemQuotaCode.Name].Value = dtl.ProjectTaskDtlGUID.TheCostItem.QuotaCode;
            }

            row.Tag = dtl;
        }

        private decimal FigureProgressRound(decimal value)
        {
            return decimal.Round(value, 3);
        }

        void btnSelectAccountTaskRootNode_Click(object sender, EventArgs e)
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

        private void GetAccountGWBSNodes(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                GWBSTree accountGWBS = tn.Tag as GWBSTree;
                if (accountGWBS.ResponsibleAccFlag)//如果该节点是核算节点
                {
                    ListAccountGWBSNodes.Add(tn);
                }
                else
                {
                    GetAccountGWBSNodes(tn);
                }
            }
        }

        private void GetStatGWBSNodesByAccountNode(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                ListStatGWBSNodes.Add(tn.Tag as GWBSTree);

                GetStatGWBSNodesByAccountNode(tn);
            }
        }
        private string GetIsBalance(ProjectTaskDetailAccount oDtl)
        {
            string sResult = "未结算";
            if (oDtl != null)
            {
                if (oDtl.Details != null && oDtl.Details.Count > 0)
                {
                    sResult = oDtl.Details.FirstOrDefault(a => a.IsBalance == false) == null ? "结算" : "未结算";
                }
                else
                {
                    sResult = oDtl.MatFeeBlanceFlag == EnumMaterialFeeSettlementFlag .结算? "结算" : "未结算";
                }
            }
            return sResult;
        }
    }
}
