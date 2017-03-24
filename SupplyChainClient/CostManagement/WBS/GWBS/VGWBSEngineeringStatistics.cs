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
    public partial class VGWBSEngineeringStatistics : TBasicDataView
    {
        MGWBSTree model = new MGWBSTree();
        public VGWBSEngineeringStatistics()
        {
            InitializeComponent();
        }
        public VGWBSEngineeringStatistics(GWBSTree wbs)
        {
            InitializeComponent();
            //InitTextBoxEnabled();
            ShowData(wbs);
        }
        void InitTextBoxEnabled()
        {
            txtWBSName.Enabled = false;
            txtActualBeginTime.Enabled = txtActualDays.Enabled = txtActualEndTime.Enabled = false;
            txtPlanBeginTime.Enabled = txtPlanEndTime.Enabled = txtPlanDays.Enabled = false;
            txtCumulativeProject.Enabled = false;
        }

        void ShowData(GWBSTree wbs)
        {
            string fullPath = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), wbs.Name, wbs.SysCode);
            //dgWBSShow[colTaskName.Name, rowIndex].ToolTipText = fullPath;
            txtWBSName.Text = fullPath;

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

                    var queryChild = from p in queryLevel
                                     where p.ParentNode.Id == parentObj.parentId
                                     select p;

                    foreach (ProductionScheduleDetail planDtl in queryChild)
                    {
                        if (sumDuration != 0)
                            figureProgress += planDtl.AddupFigureProgress * (planDtl.PlannedDuration / sumDuration);
                    }

                    DateTime MaxEndDate = queryChild.Max(t => t.PlannedEndDate);

                    DateTime MinBeginDate = queryChild.Min(t => t.PlannedBeginDate);

                    int planDuration = 0;
                    if (MaxEndDate != DateTime.Parse("1900-1-1"))
                        planDuration = (MinBeginDate - MinBeginDate).Days + 1;


                    DateTime FactMaxEndDate = queryChild.Max(t => t.ActualEndDate);

                    DateTime FactMinBeginDate = queryChild.Min(t => t.ActualBeginDate);

                    int factDuration = 0;
                    if (FactMaxEndDate != DateTime.Parse("1900-1-1"))
                        factDuration = (FactMaxEndDate - FactMinBeginDate).Days + 1;

                    queryParent.ElementAt(0).PlannedBeginDate = MinBeginDate;
                    queryParent.ElementAt(0).PlannedEndDate = MaxEndDate;
                    queryParent.ElementAt(0).PlannedDuration = planDuration;

                    queryParent.ElementAt(0).ActualBeginDate = FactMinBeginDate;
                    queryParent.ElementAt(0).ActualEndDate = FactMaxEndDate;
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
    }
}
