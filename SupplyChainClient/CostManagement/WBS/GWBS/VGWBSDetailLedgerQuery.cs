using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VGWBSDetailLedgerQuery : TBasicDataView
    {
        public VGWBSDetailLedgerQuery()
        {
            InitializeComponent();

            InitForm();
        }

        MGWBSTree model = new MGWBSTree();
        CurrentProjectInfo projectInfo = null;
        private void InitForm()
        {
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();

            dtLogStartTime.Value = DateTime.Now.AddMonths(-1);
            dtLogEndDate.Value = DateTime.Now;

            InitEvents();
        }
        private void InitEvents()
        {
            btnSelectProjectTask.Click += new EventHandler(btnSelectProjectTask_Click);
            btnSelectTaskDetail.Click += new EventHandler(btnSelectTaskDetail_Click);
            btnSelectContractGroup.Click += new EventHandler(btnSelectContractGroup_Click);
            btnQuery.Click += new EventHandler(btnQuery_Click);
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                FlashScreen.Show("正在查询，请稍候........");

                string startTime = "";
                string endTime = "";

                string sqlCondition = " where t1.theprojectguid='" + projectInfo.Id + "'";

                if (txtProjectTask.Text.Trim() != "" && txtProjectTask.Tag != null)
                    sqlCondition += " and t1.theprojecttasksyscode like '" + (txtProjectTask.Tag as GWBSTree).SysCode + "%'";
                if (txtProjectTaskDetail.Text.Trim() != "" && txtProjectTaskDetail.Tag != null)
                    sqlCondition += " and t1.projecttaskdtlid = '" + (txtProjectTaskDetail.Tag as GWBSDetail).Id + "'";
                if (txtContract.Text.Trim() != "" && txtContract.Tag != null)
                    sqlCondition += " and t1.contractgroupid = '" + (txtContract.Tag as ContractGroup).Id + "'";
                if (dtLogStartTime.IsHasValue)
                {
                    startTime = dtLogStartTime.Value.Date.ToString();
                    sqlCondition += " and t1.createtime>=to_date('" + startTime + "','yyyy-mm-dd hh24:mi:ss')";
                }
                if (dtLogEndDate.IsHasValue)
                {
                    endTime = dtLogEndDate.Value.Date.AddDays(1).AddSeconds(-1).ToString();
                    sqlCondition += " and t1.createtime<=to_date('" + endTime + "','yyyy-mm-dd hh24:mi:ss')";
                }

                List<GWBSDetailLedger> list = model.GWBSDtlLedgerQuery(sqlCondition, startTime, endTime);

                #region ObjectQuery
                //ObjectQuery oq = new ObjectQuery();
                //oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));

                //if (txtProjectTask.Text.Trim() != "" && txtProjectTask.Tag != null)
                //    oq.AddCriterion(Expression.Like("TheProjectTaskSysCode", (txtProjectTask.Tag as GWBSTree).SysCode, MatchMode.Start));

                //if (txtProjectTaskDetail.Text.Trim() != "" && txtProjectTaskDetail.Tag != null)
                //    oq.AddCriterion(Expression.Eq("ProjectTaskDtlID", (txtProjectTaskDetail.Tag as GWBSDetail).Id));

                //if (txtContract.Text.Trim() != "" && txtContract.Tag != null)
                //    oq.AddCriterion(Expression.Eq("TheContractGroup.Id", (txtContract.Tag as ContractGroup).Id));

                //if (dtLogStartTime.IsHasValue)
                //    oq.AddCriterion(Expression.Ge("CreateTime", dtLogStartTime.Value.Date));
                //if (dtLogEndDate.IsHasValue)
                //    oq.AddCriterion(Expression.Le("CreateTime", dtLogEndDate.Value.Date.AddDays(1).AddSeconds(-1)));

                //oq.AddFetchMode("TheContractGroup", NHibernate.FetchMode.Eager);

                //IList list = model.ObjectQuery(typeof(GWBSDetailLedger), oq);

                //if (list.Count == 0)
                //    return;

                ////1.添加变更初始值台帐
                //if (dtLogStartTime.IsHasValue || dtLogEndDate.IsHasValue)//带查询时间条件的情况可能只查询出任务明细的部分变更，统计时数据不完整，需再做二次查询
                //{
                //    var queryGroupTaskDtl = from l in list.OfType<GWBSDetailLedger>()
                //                            group l by new { l.ProjectTaskDtlID } into g
                //                            select new { g.Key.ProjectTaskDtlID };

                //    ObjectQuery oqSum = new ObjectQuery();
                //    Disjunction disSum = new Disjunction();
                //    foreach (var obj in queryGroupTaskDtl)
                //    {
                //        disSum.Add(Expression.Eq("ProjectTaskDtlID", obj.ProjectTaskDtlID));
                //    }
                //    oqSum.AddCriterion(disSum);
                //    list = model.ObjectQuery(typeof(GWBSDetailLedger), oqSum);
                //}

                //加载明细和成本项信息
                //ObjectQuery oqDtl = new ObjectQuery();
                //Disjunction disDtl = new Disjunction();
                //var queryDtlGroup = from d in list.OfType<GWBSDetailLedger>()
                //                    group d by new { d.ProjectTaskDtlID } into g
                //                    select new { g.Key.ProjectTaskDtlID };
                //foreach (var item in queryDtlGroup)
                //{
                //    disDtl.Add(Expression.Eq("Id", item.ProjectTaskDtlID));
                //}
                //oqDtl.AddCriterion(disDtl);
                //oqDtl.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
                //IEnumerable<GWBSDetail> listDtl = model.ObjectQuery(typeof(GWBSDetail), oqDtl).OfType<GWBSDetail>();

                #endregion


                //显示数据
                gridLedger.Rows.Clear();
                foreach (GWBSDetailLedger item in list)
                {
                    InsertIntoGridByDtl(item);
                }

                #region 显示汇总信息

                gridLedgerSum.Rows.Clear();

                List<GWBSDetailLedger> listLedgerSum = new List<GWBSDetailLedger>();

                var query = from l in list.OfType<GWBSDetailLedger>()
                            where l.ContractChangeMode == ContractIncomeChangeModeEnum.合同初始值
                            select l;
                if (query.Count() > 0)
                    listLedgerSum.AddRange(query.ToList());

                //2.汇总量、价的变更
                var queryGroupQny = from l in list.OfType<GWBSDetailLedger>()
                                    where l.ContractChangeMode != ContractIncomeChangeModeEnum.合同初始值
                                    group l by new { l.ProjectTaskID, l.ProjectTaskName, l.ProjectTaskDtlID, l.ProjectTaskDtlName } into g
                                    select new
                                    {
                                        g.Key.ProjectTaskID,
                                        g.Key.ProjectTaskName,
                                        g.Key.ProjectTaskDtlID,
                                        g.Key.ProjectTaskDtlName,
                                    };
                foreach (var obj in queryGroupQny)
                {
                    decimal contractQnySum = (from l in list.OfType<GWBSDetailLedger>()
                                              where l.ProjectTaskDtlID == obj.ProjectTaskDtlID && l.ContractChangeMode == ContractIncomeChangeModeEnum.合同收入工程量变化
                                              select l).Sum(o => o.ContractWorkAmount);

                    decimal responsibleQnySum = (from l in list.OfType<GWBSDetailLedger>()
                                                 where l.ProjectTaskDtlID == obj.ProjectTaskDtlID && l.ResponsibleCostChangeMode == ResponsibleCostChangeModeEnum.责任工程量变化
                                                 select l).Sum(o => o.ResponsibleWorkAmount);

                    decimal planQnySum = (from l in list.OfType<GWBSDetailLedger>()
                                          where l.ProjectTaskDtlID == obj.ProjectTaskDtlID && l.PlanCostChangeMode == PlanCostChangeModeEnum.计划工程量变化
                                          select l).Sum(o => o.PlanWorkAmount);

                    if (contractQnySum > 0 || responsibleQnySum > 0 || planQnySum > 0)
                    {
                        GWBSDetailLedger led = new GWBSDetailLedger();
                        led.ProjectTaskID = obj.ProjectTaskID;
                        led.ProjectTaskName = obj.ProjectTaskName;
                        led.ProjectTaskDtlID = obj.ProjectTaskDtlID;
                        led.ProjectTaskDtlName = obj.ProjectTaskDtlName;
                        led.ContractWorkAmount = contractQnySum;
                        led.ResponsibleWorkAmount = responsibleQnySum;
                        led.PlanWorkAmount = planQnySum;

                        if (contractQnySum > 0)
                        {
                            led.ContractChangeMode = ContractIncomeChangeModeEnum.合同收入工程量变化;

                            var query2 = from l in list.OfType<GWBSDetailLedger>()
                                         where l.ProjectTaskDtlID == obj.ProjectTaskDtlID && l.ContractChangeMode == ContractIncomeChangeModeEnum.合同初始值
                                         select l;
                            led.ContractPrice = query2.ElementAt(0).ContractPrice;
                            led.ContractTotalPrice = led.ContractWorkAmount * led.ContractPrice;
                        }
                        else
                            led.ContractChangeMode = ContractIncomeChangeModeEnum.合同收入无变化;

                        if (responsibleQnySum > 0)
                        {
                            led.ResponsibleCostChangeMode = ResponsibleCostChangeModeEnum.责任工程量变化;
                            var query2 = from l in list.OfType<GWBSDetailLedger>()
                                         where l.ProjectTaskDtlID == obj.ProjectTaskDtlID && l.ResponsibleCostChangeMode == ResponsibleCostChangeModeEnum.责任成本初始值
                                         select l;

                            led.ResponsiblePrice = query2.ElementAt(0).ResponsiblePrice;
                            led.ResponsibleTotalPrice = led.ResponsibleWorkAmount * led.ResponsiblePrice;
                        }
                        else
                            led.ResponsibleCostChangeMode = ResponsibleCostChangeModeEnum.责任成本无变化;

                        if (planQnySum > 0)
                        {
                            led.PlanCostChangeMode = PlanCostChangeModeEnum.计划工程量变化;
                            var query2 = from l in list.OfType<GWBSDetailLedger>()
                                         where l.ProjectTaskDtlID == obj.ProjectTaskDtlID && l.PlanCostChangeMode == PlanCostChangeModeEnum.计划成本初始值
                                         select l;
                            led.PlanPrice = query2.ElementAt(0).PlanPrice;
                            led.PlanTotalPrice = led.PlanWorkAmount * led.PlanPrice;
                        }
                        else
                            led.PlanCostChangeMode = PlanCostChangeModeEnum.计划成本无变化;

                        listLedgerSum.Add(led);
                    }


                    decimal contractPriceSum = (from l in list.OfType<GWBSDetailLedger>()
                                                where l.ProjectTaskDtlID == obj.ProjectTaskDtlID && l.ContractChangeMode == ContractIncomeChangeModeEnum.合同单价变化
                                                select l).Sum(o => o.ContractPrice);

                    decimal responsiblePriceSum = (from l in list.OfType<GWBSDetailLedger>()
                                                   where l.ProjectTaskDtlID == obj.ProjectTaskDtlID && l.ResponsibleCostChangeMode == ResponsibleCostChangeModeEnum.责任单价变化
                                                   select l).Sum(o => o.ResponsiblePrice);

                    decimal planPriceSum = (from l in list.OfType<GWBSDetailLedger>()
                                            where l.ProjectTaskDtlID == obj.ProjectTaskDtlID && l.PlanCostChangeMode == PlanCostChangeModeEnum.计划单价变化
                                            select l).Sum(o => o.PlanPrice);

                    if (contractPriceSum > 0 || responsiblePriceSum > 0 || planPriceSum > 0)
                    {
                        GWBSDetailLedger led = new GWBSDetailLedger();
                        led.ProjectTaskID = obj.ProjectTaskID;
                        led.ProjectTaskName = obj.ProjectTaskName;
                        led.ProjectTaskDtlID = obj.ProjectTaskDtlID;
                        led.ProjectTaskDtlName = obj.ProjectTaskDtlName;
                        led.ContractPrice = contractPriceSum;
                        led.ResponsiblePrice = responsiblePriceSum;
                        led.PlanPrice = planPriceSum;

                        if (contractPriceSum > 0)
                        {
                            led.ContractChangeMode = ContractIncomeChangeModeEnum.合同单价变化;

                            decimal contractWorkAmount = (from l in listLedgerSum
                                                          where l.ProjectTaskDtlID == obj.ProjectTaskDtlID &&
                                                          (l.ContractChangeMode == ContractIncomeChangeModeEnum.合同收入工程量变化 || l.ContractChangeMode == ContractIncomeChangeModeEnum.合同初始值)
                                                          select l).Sum(o => o.ContractWorkAmount);

                            led.ContractWorkAmount = contractWorkAmount;

                            led.ContractTotalPrice = led.ContractWorkAmount * led.ContractPrice;
                        }
                        else
                            led.ContractChangeMode = ContractIncomeChangeModeEnum.合同收入无变化;

                        if (responsiblePriceSum > 0)
                        {
                            led.ResponsibleCostChangeMode = ResponsibleCostChangeModeEnum.责任单价变化;

                            decimal responsibleWorkAmount = (from l in listLedgerSum
                                                             where l.ProjectTaskDtlID == obj.ProjectTaskDtlID &&
                                                             (l.ResponsibleCostChangeMode == ResponsibleCostChangeModeEnum.责任工程量变化 || l.ResponsibleCostChangeMode == ResponsibleCostChangeModeEnum.责任成本初始值)
                                                             select l).Sum(o => o.ResponsibleWorkAmount);

                            led.ResponsibleWorkAmount = responsibleWorkAmount;
                            led.ResponsibleTotalPrice = led.ResponsibleWorkAmount * led.ResponsiblePrice;
                        }
                        else
                            led.ResponsibleCostChangeMode = ResponsibleCostChangeModeEnum.责任成本无变化;

                        if (planPriceSum > 0)
                        {
                            led.PlanCostChangeMode = PlanCostChangeModeEnum.计划单价变化;

                            decimal planWorkAmount = (from l in listLedgerSum
                                                      where l.ProjectTaskDtlID == obj.ProjectTaskDtlID &&
                                                      (l.PlanCostChangeMode == PlanCostChangeModeEnum.计划工程量变化 || l.PlanCostChangeMode == PlanCostChangeModeEnum.计划成本初始值)
                                                      select l).Sum(o => o.PlanWorkAmount);

                            led.PlanWorkAmount = planWorkAmount;
                            led.PlanTotalPrice = led.PlanWorkAmount * led.PlanPrice;
                        }
                        else
                            led.PlanCostChangeMode = PlanCostChangeModeEnum.计划成本无变化;

                        listLedgerSum.Add(led);
                    }
                }

                foreach (GWBSDetailLedger item in listLedgerSum)
                {
                    InsertIntoGridBySum(item);
                }

                #endregion

            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("查询异常，请重试！详细信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        private void InsertIntoGridByDtl(GWBSDetailLedger item)
        {
            int index = gridLedger.Rows.Add();
            DataGridViewRow row = gridLedger.Rows[index];
            row.Cells[ProjectTaskName.Name].Value = item.ProjectTaskName;
            row.Cells[ProjectTaskName.Name].ToolTipText = item.Temp_WBSFullPath;
            row.Cells[ProjectTaskDetail.Name].Value = item.ProjectTaskDtlName;


            if (item.Temp_WBSDtl != null)
            {
                if (item.Temp_WBSDtl.TheCostItem != null)
                    row.Cells[CostItemQuotaCode.Name].Value = item.Temp_WBSDtl.TheCostItem.QuotaCode;
                row.Cells[MainResourceTypeName.Name].Value = item.Temp_WBSDtl.MainResourceTypeName;
                row.Cells[MainResourceSpec.Name].Value = item.Temp_WBSDtl.MainResourceTypeSpec;
                row.Cells[DrawNumber.Name].Value = item.Temp_WBSDtl.DiagramNumber;
            }

            if (item.TheContractGroup != null)
            {
                row.Cells[ContractGroup.Name].Value = item.TheContractGroup.Code;
                row.Cells[ContractGroupName.Name].Value = item.TheContractGroup.ContractName;
            }
            row.Cells[CreateTime.Name].Value = item.CreateTime.ToString();

            row.Cells[ContractChangeType.Name].Value = item.ContractChangeMode;
            row.Cells[ContractProjectAmount.Name].Value = item.ContractWorkAmount;
            row.Cells[ContractPrice.Name].Value = item.ContractPrice;
            row.Cells[ContractTotalPrice.Name].Value = item.ContractTotalPrice;

            row.Cells[ResponsibleChangeType.Name].Value = item.ResponsibleCostChangeMode;
            row.Cells[ResponsibleProjectAmount.Name].Value = item.ResponsibleWorkAmount;
            row.Cells[ResponsiblePrice.Name].Value = item.ResponsiblePrice;
            row.Cells[ResponsibleTotalPrice.Name].Value = item.ResponsibleTotalPrice;

            row.Cells[PlanChangeType.Name].Value = item.PlanCostChangeMode;
            row.Cells[PlanProjectAmount.Name].Value = item.PlanWorkAmount;
            row.Cells[PlanPrice.Name].Value = item.PlanPrice;
            row.Cells[PlanTotalPrice.Name].Value = item.PlanTotalPrice;

            row.Cells[ProjectAmountUnit.Name].Value = item.WorkAmountUnitName;
            row.Cells[PriceUnit.Name].Value = item.PriceUnitName;
        }
        private void InsertIntoGridBySum(GWBSDetailLedger item)
        {
            int index = gridLedgerSum.Rows.Add();
            DataGridViewRow row = gridLedgerSum.Rows[index];
            row.Cells[ProjectTaskNameSum.Name].Value = item.ProjectTaskName;
            row.Cells[ProjectTaskNameSum.Name].ToolTipText = item.Temp_WBSFullPath;
            row.Cells[ProjectTaskDetailSum.Name].Value = item.ProjectTaskDtlName;

            if (item.Temp_WBSDtl != null)
            {
                if (item.Temp_WBSDtl.TheCostItem != null)
                    row.Cells[CostItemQuotaCodeSum.Name].Value = item.Temp_WBSDtl.TheCostItem.QuotaCode;
                row.Cells[MainResourceTypeNameSum.Name].Value = item.Temp_WBSDtl.MainResourceTypeName;
                row.Cells[MainResourceSpecSum.Name].Value = item.Temp_WBSDtl.MainResourceTypeSpec;
                row.Cells[DrawNumberSum.Name].Value = item.Temp_WBSDtl.DiagramNumber;
            }

            row.Cells[ContractChangeTypeSum.Name].Value = item.ContractChangeMode;
            row.Cells[ContractProjectAmountSum.Name].Value = item.ContractWorkAmount;
            row.Cells[ContractPriceSum.Name].Value = item.ContractPrice;
            row.Cells[ContractTotalPriceSum.Name].Value = item.ContractTotalPrice;

            row.Cells[ResponsibleChangeTypeSum.Name].Value = item.ResponsibleCostChangeMode;
            row.Cells[ResponsibleProjectAmountSum.Name].Value = item.ResponsibleWorkAmount;
            row.Cells[ResponsiblePriceSum.Name].Value = item.ResponsiblePrice;
            row.Cells[ResponsibleTotalPriceSum.Name].Value = item.ResponsibleTotalPrice;

            row.Cells[PlanChangeTypeSum.Name].Value = item.PlanCostChangeMode;
            row.Cells[PlanProjectAmountSum.Name].Value = item.PlanWorkAmount;
            row.Cells[PlanPriceSum.Name].Value = item.PlanPrice;
            row.Cells[PlanTotalPriceSum.Name].Value = item.PlanTotalPrice;

        }
        void btnSelectContractGroup_Click(object sender, EventArgs e)
        {
            VSelectWBSContractGroup frm = new VSelectWBSContractGroup(new MWBSContractGroup(), false);
            if (txtContract.Text.Trim() != "" && txtContract.Tag != null)
                frm.DefaultSelectedContract = txtContract.Tag as ContractGroup;
            frm.ShowDialog();

            if (frm.SelectResult != null && frm.SelectResult.Count > 0)
            {
                ContractGroup cg = frm.SelectResult[0];
                txtContract.Text = cg.Code;
                txtContract.Tag = cg;
            }
        }

        void btnSelectTaskDetail_Click(object sender, EventArgs e)
        {
            VSelectGWBSDetail frm = new VSelectGWBSDetail(new MGWBSTree());
            frm.ShowDialog();

            if (frm.SelectGWBSDetail != null && frm.SelectGWBSDetail.Count > 0)
            {
                GWBSDetail dtl = frm.SelectGWBSDetail[0];
                txtProjectTaskDetail.Text = dtl.Name;
                txtProjectTaskDetail.Tag = dtl;
            }
        }

        void btnSelectProjectTask_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            if (txtProjectTask.Text.Trim() != "" && txtProjectTask.Tag != null)
                frm.DefaultSelectedGWBS = txtProjectTask.Tag as GWBSTree;
            frm.ShowDialog();

            if (frm.SelectResult != null && frm.SelectResult.Count > 0)
            {
                TreeNode node = frm.SelectResult[0] as TreeNode;
                GWBSTree wbs = node.Tag as GWBSTree;
                txtProjectTask.Text = wbs.Name;
                txtProjectTask.Tag = wbs;
            }
        }
    }
}
