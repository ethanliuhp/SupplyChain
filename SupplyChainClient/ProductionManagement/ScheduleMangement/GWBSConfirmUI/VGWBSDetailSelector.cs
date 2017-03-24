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
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.GWBSConfirmUI
{
    public partial class VGWBSDetailSelector : TBasicDataView
    {
        public bool isOK = false;

        MProductionMng model = new MProductionMng();

        private int totalRecords = 0;

        private List<WeekScheduleDetail> selectedWeekPlanDtl = new List<WeekScheduleDetail>();
        /// <summary>
        /// 选择的周进度计划明细
        /// </summary>
        virtual public List<WeekScheduleDetail> SelectedWeekPlanDtl
        {
            get { return selectedWeekPlanDtl; }
            set { selectedWeekPlanDtl = value; }
        }

        public VGWBSDetailSelector()
        {
            InitializeComponent();
            InitEvent();
            InitForm();
        }

        private void InitForm()
        {
            dtpDateBegin.Value = ConstObject.LoginDate.AddMonths(-1);
            dtpDateEnd.Value = ConstObject.LoginDate;

            dtPlanBeginDate.Text = "";
            dtPlanEndDate.Text = "";


        }

        private void InitEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);

            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            lnkAll.Click += new EventHandler(lnkAll_Click);
            lnkNone.Click += new EventHandler(lnkNone_Click);

            txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);

            Load += new EventHandler(VGWBSDetailSelector_Load);

            //pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
        }

        void btnSearchGWBS_Click(object sender, EventArgs e)
        {
            bool rowSelected = false;
            foreach (DataGridViewRow dr in dgMaster.Rows)
            {
                if ((bool)dr.Cells[colDgMasterSelect.Name].Value)
                {
                    rowSelected = true;
                    break;
                }
            }
            if (rowSelected == false)
            {
                MessageBox.Show("请先选择周进度计划。");
                return;
            }

            try
            {
                //foreach (DataGridViewRow dr in dgMaster.Rows)
                //{
                //    if ((bool)dr.Cells[colDgMasterSelect.Name].Value)
                //    {
                //        WeekScheduleDetail detail = dr.Tag as WeekScheduleDetail;
                //        IList treeNodes = model.ProductionManagementSrv.GetGWBSTree(detail.GWBSTree);
                //        IList GWBSDetailList = GetGwbsDetail(treeNodes);
                //        AddGWBSDetail(GWBSDetailList,detail);
                //    }
                //}

                foreach (DataGridViewRow dr in dgMaster.Rows)
                {
                    if ((bool)dr.Cells[colDgMasterSelect.Name].Value)
                    {
                        WeekScheduleDetail detail = dr.Tag as WeekScheduleDetail;

                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("ProduceConfirmFlag", 1));
                        oq.AddCriterion(Expression.Like("TheGWBSSysCode", detail.GWBSTree.SysCode, MatchMode.Start));
                        oq.AddFetchMode("TheGWBS", NHibernate.FetchMode.Eager);

                        IList GWBSDetailList = model.ProductionManagementSrv.GetGWBSDetail(oq);

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("查询工程任务明细出错。\n" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        private IList GetGwbsDetail(IList gwbsTrees)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProduceConfirmFlag", 1));
            Disjunction dis = Expression.Disjunction();
            foreach (GWBSTree tree in gwbsTrees)
            {
                dis.Add(Expression.Eq("TheGWBS.Id", tree.Id));
            }
            oq.AddCriterion(dis);
            oq.AddFetchMode("TheGWBS", NHibernate.FetchMode.Eager);
            return model.ProductionManagementSrv.GetGWBSDetail(oq);
        }

        void VGWBSDetailSelector_Load(object sender, EventArgs e)
        {
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        /// <summary>
        /// 反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkNone_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgMaster.Rows)
            {
                if ((bool)var.Cells[colDgMasterSelect.Name].Value)
                {
                    var.Cells[colDgMasterSelect.Name].Value = false;
                }
                else
                {
                    var.Cells[colDgMasterSelect.Name].Value = true;
                }
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgMaster.Rows)
            {
                var.Cells[colDgMasterSelect.Name].Value = true;
            }
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgMaster.Rows)
            {
                if (row.Cells[colDgMasterSelect.Name].Value != null && (bool)row.Cells[colDgMasterSelect.Name].Value)
                {
                    WeekScheduleDetail dtl = row.Tag as WeekScheduleDetail;

                    selectedWeekPlanDtl.Add(dtl);
                }
            }
            if (selectedWeekPlanDtl.Count == 0)
            {
                MessageBox.Show("请选择周进度计划明细！");
                return;
            }


            isOK = true;
            this.Close();
        }
        void btnCancel_Click(object sender, EventArgs e)
        {
            isOK = false;
            this.Close();
        }

        //查询
        void btnSearch_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();

            if (dtpDateBegin.Text.Trim() != "")
                oq.AddCriterion(Expression.Ge("CreateTime", dtpDateBegin.Value));
            if (dtpDateEnd.Text.Trim() != "")
                oq.AddCriterion(Expression.Lt("CreateTime", dtpDateEnd.Value.AddDays(1).Date));

            if (dtPlanBeginDate.Text.Trim() != "")
                oq.AddCriterion(Expression.Ge("PlannedBeginDate", dtPlanBeginDate.Value));
            if (dtPlanEndDate.Text.Trim() != "")
                oq.AddCriterion(Expression.Lt("PlannedBeginDate", dtPlanEndDate.Value.AddDays(1).Date));

            //oq.AddCriterion(Expression.Eq("GWBSConfirmFlag", 0));//工程任务确认标志

            if (txtGWBSName.Text != "")
                oq.AddCriterion(Expression.Like("GWBSTreeName", txtGWBSName.Text, MatchMode.Anywhere));


            oq.AddCriterion(Expression.Eq("Master.ProjectId", StaticMethod.GetProjectInfo().Id));//当前操作项目
            oq.AddCriterion(Expression.Eq("Master.ExecScheduleType", EnumExecScheduleType.周进度计划));
            oq.AddCriterion(Expression.Eq("Master.SummaryStatus", EnumSummaryStatus.汇总生成));



            if (txtCodeBegin.Text != "")
                oq.AddCriterion(Expression.Between("Master.Code", txtCodeBegin.Text, txtCodeEnd.Text));

            if (txtWeekPlanName.Text.Trim() != "")
                oq.AddCriterion(Expression.Like("Master.PlanName", txtWeekPlanName.Text.Trim(), MatchMode.Anywhere));

            oq.AddCriterion(Expression.Eq("GWBSTree.ProductConfirmFlag", true));//生产确认节点

            //oq.AddCriterion(Expression.Eq("ScheduleConfirmFlag", 1));//形象进度确认标志

            oq.AddOrder(Order.Asc("Master.Code"));
            oq.AddOrder(Order.Asc("PlannedBeginDate"));

            try
            {
                IList list = model.ProductionManagementSrv.ObjectQuery(typeof(WeekScheduleDetail), oq);

                //IEnumerable<WeekScheduleDetail> listddd = list.OfType<WeekScheduleDetail>();

                //var query = from d in listddd
                //            where d.CreateTime > Convert.ToDateTime("2012-11-28")
                //            select d;

                int dtlCount = 0;
                ShowMasterList(list, ref dtlCount);
                lblRecordCount.Text = "共【" + dtlCount + "】条记录";
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        /// <summary>
        /// 显示主表
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList masterList, ref int dtlCount)
        {
            dgMaster.Rows.Clear();

            if (masterList == null || masterList.Count == 0)
                return;

            //根据项目任务分组
            IEnumerable<WeekScheduleDetail> queryWeekDtl = masterList.OfType<WeekScheduleDetail>();

            var queryGroup = from w in queryWeekDtl
                             where w.GWBSTree != null
                             group w by new
                             {
                                 w.GWBSTree.Id
                             }
                                 into g
                                 select new
                                 {
                                     g.Key.Id
                                 }
                           ;

            dtlCount = queryGroup.Count();

            foreach (var gwbs in queryGroup)
            {
                //取每个项目任务下累计工程形象进度为最大的周进度计划明细显示
                //var queryDtl = from w in queryWeekDtl
                //               where w.GWBSTree.Id == gwbs.Id && w.TaskCompletedPercent == queryWeekDtl.Max(t => w.TaskCompletedPercent)
                //               select w;
                //WeekScheduleDetail detail = queryDtl.ElementAt(0);

                var queryDtl = from w in queryWeekDtl
                               where w.GWBSTree != null && w.GWBSTree.Id == gwbs.Id
                               select w;

                //WeekScheduleDetail detail = null;
                decimal maxPercent= queryDtl.Max(p => p.TaskCompletedPercent);

                queryDtl = from d in queryDtl
                           where d.TaskCompletedPercent==maxPercent
                           select d;

                foreach (WeekScheduleDetail detail in queryDtl)
                {
                    //if (detail == null)
                    //    detail = dtl;

                    //if (detail.TaskCompletedPercent < dtl.TaskCompletedPercent)
                    //{
                    //    detail = dtl;
                    //}

                    int rowIndex = dgMaster.Rows.Add();
                    dgMaster.Rows[rowIndex].Tag = detail;

                    dgMaster[colDgMasterSelect.Name, rowIndex].Value = true;

                    dgMaster[colDgMasterWeekPlanCode.Name, rowIndex].Value = detail.Master.Code;
                    dgMaster[colDgMasterWeekPlanName.Name, rowIndex].Value = detail.Master.PlanName;

                    dgMaster[colDgMasterGWBSName.Name, rowIndex].Value = detail.GWBSTreeName;
                    dgMaster[colDgMasterGWBSName.Name, rowIndex].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), detail.GWBSTreeName, detail.GWBSTreeSysCode);

                    dgMaster[colProductConfirmFlag.Name, rowIndex].Value = "是";//直观

                    dgMaster[colCreateDate.Name, rowIndex].Value = detail.CreateTime.ToShortDateString();

                    dgMaster[colDgMasterPlanBeginDate.Name, rowIndex].Value = detail.PlannedBeginDate.ToShortDateString();
                    dgMaster[colDgMasterPlanEndDate.Name, rowIndex].Value = StaticMethod.GetShowDateTimeStr(detail.PlannedEndDate, false);

                    dgMaster[colDgMasterTaskContent.Name, rowIndex].Value = detail.MainTaskContent;
                    dgMaster[colDgMasterFinishPercent.Name, rowIndex].Value = detail.TaskCompletedPercent;
                    dgMaster[colDgMasterAnalysis.Name, rowIndex].Value = detail.CompletionAnalysis;
                }
            }
        }

        /// <summary>
        /// 判断主表是否有可引用数量
        /// </summary>
        /// <param name="master"></param>
        /// <returns></returns>
        private bool HasRefQuantity(BaseMaster master)
        {
            if (master == null || master.Details.Count == 0)
                return false;
            foreach (BaseDetail dtl in master.Details)
            {
                if (decimal.Subtract(dtl.Quantity, dtl.RefQuantity) > 0)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
