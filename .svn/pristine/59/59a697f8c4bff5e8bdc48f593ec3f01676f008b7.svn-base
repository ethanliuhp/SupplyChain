using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VGWBSBusinessStatistics : TBasicDataView
    {
        MGWBSTree model = new MGWBSTree();
        decimal contractRevenue = 0;
        decimal responsibilityCost = 0;
        decimal plannedCost = 0;

        public VGWBSBusinessStatistics()
        {
            InitializeComponent();
        }
        public VGWBSBusinessStatistics(GWBSTree wbs)
        {
            InitializeComponent();
            //InitTextBoxEnabled();
            ShowData(wbs);
        }

        void InitTextBoxEnabled()
        {
            txtWBSName.Enabled = false;
            txtContractRevenue.Enabled = txtPlannedCost.Enabled = txtResponsibilityCost.Enabled = false;
            txtActualBeginTime.Enabled = txtActualDays.Enabled = txtActualEndTime.Enabled = false;
            txtPlanBeginTime.Enabled = txtPlanEndTime.Enabled = txtPlanDays.Enabled = false;
            txtCumulativeProject.Enabled = txtCumulativeAccounting.Enabled = false;
        }

        void ShowData(GWBSTree wbs)
        {
            string fullPath = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), wbs.Name, wbs.SysCode);
            txtWBSName.Text = fullPath;

            AggregatePrice(wbs);

            if (wbs.CategoryNodeType == VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("GWBSTree.Id", wbs.Id));
                oq.AddCriterion(Expression.Eq("State", EnumScheduleDetailState.有效));
                oq.AddCriterion(Expression.Eq("Master.ScheduleType", EnumScheduleType.总滚动进度计划));
                IList schedule = model.ObjectQuery(typeof(ProductionScheduleDetail), oq);
                if (schedule != null && schedule.Count > 0)
                {
                    ProductionScheduleDetail dtl = schedule[0] as ProductionScheduleDetail;
                    ShowData(dtl);
                }
            }
            else
            {
                ProductionScheduleDetail dtl = GetAddupFigureProgressByGWBS(wbs);
                ShowData(dtl);
            }
        }

        void ShowData(ProductionScheduleDetail dtl)
        {
            if (dtl != null)
            {
                if (dtl.PlannedBeginDate != DateTime.Parse("1900-1-1"))
                    txtPlanBeginTime.Text = dtl.PlannedBeginDate.ToString();
                if (dtl.PlannedEndDate != DateTime.Parse("1900-1-1"))
                    txtPlanEndTime.Text = dtl.PlannedEndDate.ToString();
                if (dtl.PlannedBeginDate != DateTime.Parse("1900-1-1") && dtl.PlannedEndDate != DateTime.Parse("1900-1-1"))
                    txtPlanDays.Text = dtl.PlannedDuration.ToString();

                if (dtl.ActualBeginDate != DateTime.Parse("1900-1-1"))
                    txtActualBeginTime.Text = dtl.ActualBeginDate.ToString();
                if (dtl.ActualEndDate != DateTime.Parse("1900-1-1"))
                    txtActualEndTime.Text = dtl.ActualEndDate.ToString();
                if (dtl.ActualBeginDate != DateTime.Parse("1900-1-1") && dtl.ActualEndDate != DateTime.Parse("1900-1-1"))
                    txtActualDays.Text = dtl.ActualDuration.ToString();

                txtCumulativeProject.Text = dtl.AddupFigureProgress.ToString();
            }
        }

        /// <summary>
        /// 获取项目任务的累计工程形象进度
        /// </summary>
        /// <param name="wbs"></param>
        /// <returns></returns>
        private ProductionScheduleDetail GetAddupFigureProgressByGWBS(GWBSTree wbs)
        {
            ObjectQuery oq = new ObjectQuery();

            oq.AddCriterion(Expression.Like("GWBSTreeSysCode", wbs.SysCode, MatchMode.Start));
            oq.AddCriterion(Expression.Eq("State", EnumScheduleDetailState.有效));
            oq.AddCriterion(Expression.Eq("Master.ScheduleType", EnumScheduleType.总滚动进度计划));

            IEnumerable<ProductionScheduleDetail> listChildSchduleNode = model.ObjectQuery(typeof(ProductionScheduleDetail), oq).OfType<ProductionScheduleDetail>();
            if (listChildSchduleNode == null || listChildSchduleNode.Count() == 0)
                return null;
            int maxLevel = 0;
            int minLevel = 0;

            maxLevel = (from s in listChildSchduleNode
                        where s.Level == listChildSchduleNode.Max(t => t.Level)
                        select s).ElementAt(0).Level;


            minLevel = (from s in listChildSchduleNode
                        where s.Level == listChildSchduleNode.Min(t => t.Level)
                        select s).ElementAt(0).Level;


            //节点累计工程形象进度X=子节点形象进度Xi按子节点计划工期Di进行加权平均：X= ∑(Xi*(Di/∑Di)) 
            //从最深级别开始往上一层层汇集
            for (int i = maxLevel; i > minLevel; i--)
            {
                var queryLevel = from s in listChildSchduleNode
                                 where s.Level == i
                                 select s;

                int sumDuration = queryLevel.Sum(t => t.PlannedDuration);

                //根据父节点分组，以便汇集工期和形象进度到每个父节点上
                var queryGroupParent = from p in queryLevel
                                       group p by new { parentId = p.ParentNode.Id }
                                           into g
                                           select new { g.Key.parentId };

                //汇集上一层节点的形象进度和工期
                foreach (var parentObj in queryGroupParent)
                {
                    decimal figureProgress = 0;

                    var queryParent = from p in listChildSchduleNode
                                      where p.Id == parentObj.parentId
                                      select p;

                    if (queryParent.Count() == 0)
                        continue;

                    var queryChild = from p in queryLevel
                                     where p.ParentNode.Id == parentObj.parentId
                                     select p;

                    foreach (ProductionScheduleDetail planDtl in queryChild)
                    {
                        if (sumDuration != 0)
                            figureProgress += planDtl.AddupFigureProgress * (planDtl.PlannedDuration / sumDuration);
                    }

                    var queryMaxEndDate = from p in queryChild
                                          where p.PlannedEndDate == queryChild.Max(t => t.PlannedEndDate)
                                          select p;

                    var queryMinBeginDate = from p in queryChild
                                            where p.PlannedBeginDate == queryChild.Min(t => t.PlannedBeginDate)
                                            select p;

                    int planDuration = 0;
                    if (queryMaxEndDate.ElementAt(0).PlannedEndDate != DateTime.Parse("1900-1-1"))
                        planDuration = (queryMaxEndDate.ElementAt(0).PlannedEndDate - queryMinBeginDate.ElementAt(0).PlannedBeginDate).Days + 1;


                    var querFactMaxEndDate = from p in queryChild
                                             where p.ActualEndDate == queryChild.Max(t => t.ActualEndDate)
                                             select p;

                    var queryFactMinBeginDate = from p in queryChild
                                                where p.ActualBeginDate == queryChild.Min(t => t.ActualBeginDate)
                                                select p;

                    int factDuration = 0;
                    if (querFactMaxEndDate.ElementAt(0).ActualEndDate != DateTime.Parse("1900-1-1"))
                        factDuration = (querFactMaxEndDate.ElementAt(0).ActualEndDate - queryFactMinBeginDate.ElementAt(0).ActualBeginDate).Days + 1;

                    queryParent.ElementAt(0).PlannedBeginDate = queryMinBeginDate.ElementAt(0).PlannedBeginDate;
                    queryParent.ElementAt(0).PlannedEndDate = queryMaxEndDate.ElementAt(0).PlannedEndDate;
                    queryParent.ElementAt(0).PlannedDuration = planDuration;

                    queryParent.ElementAt(0).ActualBeginDate = queryFactMinBeginDate.ElementAt(0).ActualBeginDate;
                    queryParent.ElementAt(0).ActualEndDate = querFactMaxEndDate.ElementAt(0).ActualEndDate;
                    queryParent.ElementAt(0).ActualDuration = factDuration;

                    queryParent.ElementAt(0).AddupFigureProgress = figureProgress;

                }
            }

            ProductionScheduleDetail returnPlanDtl = null;

            returnPlanDtl = (from s in listChildSchduleNode
                             where s.Level == minLevel
                             select s).First();

            //returnPlanDtl = (from s in listChildSchduleNode
            //                 where s.GWBSTree.Id == wbs.Id
            //                 select s).First();

            return returnPlanDtl;
        }

        /// <summary>
        /// 获取项目任务的核算形象进度
        /// </summary>
        /// <returns></returns>
        private decimal GetAccountFigureProgress(GWBSTree wbs)
        {
            decimal figureProgress = 0;


            return figureProgress;
        }

        void AggregatePrice(GWBSTree wbs)
        {
            //IList updateGWBSList = new ArrayList();
            //ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Like("SysCode", wbs.SysCode, MatchMode.Start));
            //oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            //IList list = model.ObjectQuery(typeof(GWBSTree), oq);
            //if (list != null && list.Count > 0)
            //{
            //    foreach (GWBSTree w in list)
            //    {
            //        List<Decimal> listTotalPrice = StaticMethod.GetTaskDtlTotalPrice(w);

            //        decimal contractTotalPrice = listTotalPrice[0];
            //        decimal planTotalPrice = listTotalPrice[1];
            //        decimal responsibilitilyTotalPrice = listTotalPrice[2];

            //        bool flag = false;
            //        if (contractTotalPrice != w.ContractTotalPrice)
            //        {
            //            w.ContractTotalPrice = contractTotalPrice;
            //            flag = true;
            //        }
            //        if (planTotalPrice != w.PlanTotalPrice)
            //        {
            //            w.PlanTotalPrice = planTotalPrice;
            //            flag = true;
            //        }
            //        if (responsibilitilyTotalPrice != w.ResponsibilityTotalPrice)
            //        {
            //            w.ResponsibilityTotalPrice = responsibilitilyTotalPrice;
            //            flag = true;
            //        }
            //        if (flag)
            //            updateGWBSList.Add(w);

            //        contractRevenue += w.ContractTotalPrice;
            //        plannedCost += w.PlanTotalPrice;
            //        responsibilityCost += w.ResponsibilityTotalPrice;
            //    }
            //}

            wbs = model.GetObjectById(typeof(GWBSTree), wbs.Id) as GWBSTree;

            wbs = model.AccountTotalPrice(wbs);

            txtContractRevenue.Text = wbs.ContractTotalPrice.ToString();
            txtResponsibilityCost.Text = wbs.ResponsibilityTotalPrice.ToString();
            txtPlannedCost.Text = wbs.PlanTotalPrice.ToString();

            model.SaveGWBSTree(wbs);

        }
    }
}
