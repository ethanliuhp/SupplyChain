using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI
{
    /// <summary>
    /// 周进度计划确认
    /// </summary>
    public partial class VWeekScheduleConfirm : TBasicDataView
    {
        WeekScheduleDetail optDtl = null;

        MProductionMng model = new MProductionMng();

        public VWeekScheduleConfirm()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        private void InitData()
        {
            dtpDateBeginByQuery.Value = DateTime.Now.AddDays(-10);

            dtFactEndTime.Value = DateTime.Now;
        }

        private void InitEvent()
        {
            btnSelBearByQuery.Click += new EventHandler(btnSelBearByQuery_Click);
            btnQuery.Click += new EventHandler(btnQuery_Click);

            dgMaster.CellClick += new DataGridViewCellEventHandler(dgMaster_CellClick);
            dgDetail.CellClick += new DataGridViewCellEventHandler(dgDetail_CellClick);

            btnConfirmSubmit.Click += new EventHandler(btnConfirm_Click);
            btnSave.Click += new EventHandler(btnSave_Click);

            gbQuery.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
        }

        //选择任务承担者
        void btnSelBearByQuery_Click(object sender, EventArgs e)
        {
            VContractExcuteSelector vces = new VContractExcuteSelector();
            vces.StartPosition = FormStartPosition.CenterScreen;
            vces.ShowDialog();
            if (vces.Result != null && vces.Result.Count > 0)
            {
                SubContractProject scp = vces.Result[0] as SubContractProject;

                txtTaskBearByQuery.Text = scp.BearerOrgName;
                txtTaskBearByQuery.Tag = scp;
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgDetail.CurrentRow == null)
                {
                    MessageBox.Show("当前没有要保存的周进度计划明细！");
                    return;
                }

                if (!Valid())
                    return;

                DateTime serverTime = model.ProductionManagementSrv.GetServerTime();

                //optDtl.ScheduleConfirmFlag = 1;//形象进度确认标志
                optDtl.ScheduleConfirmDate = serverTime;
                //optDtl = model.ProductionManagementSrv.UpdateWeekScheduleDetail(optDtl);
                IList list = new ArrayList();
                list.Add(optDtl);
                optDtl = model.ProductionManagementSrv.SaveOrUpdate(list)[0] as WeekScheduleDetail;
                MessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {

                MessageBox.Show("保存失败，错误信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            
        }


        //提交
        void btnConfirm_Click(object sender, EventArgs e)
        {
            if (dgDetail.CurrentRow == null)
            {
                MessageBox.Show("当前没有要提交的周进度计划明细！");
                return;
            }

            if (!Valid())
                return;

            DateTime serverTime = model.ProductionManagementSrv.GetServerTime();

            //optDtl.GWBSConfirmFlag = 1;   //工程量确认标志,在提交确认单时处理
            //optDtl.GWBSConfirmDate = serverTime;

            optDtl.ScheduleConfirmFlag = 1;//形象进度确认标志
            optDtl.ScheduleConfirmDate = serverTime;


            try
            {
                optDtl = model.ProductionManagementSrv.UpdateWeekScheduleDetailAndScrollPlan(optDtl);
                dgDetail.Rows[dgDetail.CurrentRow.Index].Tag = optDtl;

                gbWeekPlanDtlInfo.Enabled = false;

                //dgDetail.Rows.RemoveAt(dgDetail.CurrentRow.Index);
                //if (dgDetail.Rows.Count > 0)
                //{
                //    dgDetail.CurrentCell = dgDetail.Rows[0].Cells[0];
                //    dgDetail_CellClick(dgDetail, new DataGridViewCellEventArgs(0, 0));
                //}
                //else
                //    ClearChildControlData(gbWeekPlanDtlInfo);

                MessageBox.Show("确认完成!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        private bool Valid()
        {

            if (dtFactBeginTime.Value == DateTime.Parse("1900-1-1"))
            {
                MessageBox.Show("请选择实际开始时间！");
                dtFactBeginTime.Focus();
                return false;
            }
            if (dtFactEndTime.Value == DateTime.Parse("1900-1-1"))
            {
                MessageBox.Show("请选择实际结束时间！");
                dtFactEndTime.Focus();
                return false;
            }
            if (dtFactEndTime.Value < dtFactBeginTime.Value)
            {
                MessageBox.Show("实际结束时间不能小于实际开始时间！");
                dtFactEndTime.Focus();
                return false;
            }

            decimal FigureProgress = 0;
            decimal factProjectQuantity = 0;

            try
            {
                if (txtFigureProgress.Text.Trim() == "")
                {
                    MessageBox.Show("请填写【本次确认累积形象进度】！");
                    txtFigureProgress.Focus();
                    return false;
                }
                FigureProgress = ClientUtil.ToDecimal(txtFigureProgress.Text);
                if (FigureProgress < 0)
                {
                    MessageBox.Show("请输入正确的【本次确认累积形象进度】！");
                    txtFigureProgress.Focus();
                    return false;
                }

                decimal addupFigureProgress = ClientUtil.ToDecimal(txtAddupFigureProgress.Text);
                if (FigureProgress <= addupFigureProgress)
                {
                    MessageBox.Show("【本次确认累积形象进度】需大于【已确认累积形象进度】！");
                    txtFigureProgress.Focus();
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("【本次确认累积形象进度】输入格式不正确！");
                txtFigureProgress.Focus();
                return false;
            }

            try
            {
                if (txtFactProjectQuantity.Text.Trim() == "")
                {
                    MessageBox.Show("请填写实际工程量！");
                    txtFactProjectQuantity.Focus();
                    return false;
                }

                factProjectQuantity = ClientUtil.ToDecimal(txtFactProjectQuantity.Text);
            }
            catch
            {
                MessageBox.Show("实际工程量输入格式不正确！");
                txtFactProjectQuantity.Focus();
                return false;
            }

            optDtl = dgDetail.CurrentRow.Tag as WeekScheduleDetail;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", optDtl.Id));
            IList list = model.ProductionManagementSrv.ObjectQuery(typeof(WeekScheduleDetail), oq);
            optDtl = list[0] as WeekScheduleDetail;

            optDtl.ActualBeginDate = dtFactBeginTime.Value.Date;
            optDtl.ActualEndDate = dtFactEndTime.Value.Date;
            optDtl.TaskCompletedPercent = FigureProgress;
            optDtl.ActualWorklaod = factProjectQuantity;
            optDtl.CompletionAnalysis = txtCompletionAnalysis.Text.Trim();

            optDtl.ActualDuration = (optDtl.ActualEndDate - optDtl.ActualBeginDate).Days + 1;

            return true;
        }

        void dgMaster_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dr = dgMaster.CurrentRow;
            if (dr == null)
                return;

            WeekScheduleMaster master = dr.Tag as WeekScheduleMaster;
            if (master == null)
                return;

            FillDgDetail(master);
        }
        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow dr = dgMaster.CurrentRow;
            if (dr == null) return;
            WeekScheduleMaster master = dr.Tag as WeekScheduleMaster;
            if (master == null) return;
            FillDgDetail(master);
        }

        void dgDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //ClearChildControlData(gbWeekPlanDtlInfo);

            DataGridViewRow dr = dgDetail.CurrentRow;
            if (dr == null) return;

            WeekScheduleDetail dtl = dr.Tag as WeekScheduleDetail;
            if (dtl == null) return;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", dtl.Id));
            //oq.AddCriterion(Expression.Eq("GWBSConfirmFlag", 0));
            oq.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("SubContractProject", NHibernate.FetchMode.Eager);

            IList list = model.ProductionManagementSrv.ObjectQuery(typeof(WeekScheduleDetail), oq);
            dtl = list[0] as WeekScheduleDetail;

            FillDgDetail(dtl);
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            ClearControlData();

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", StaticMethod.GetProjectInfo().Id));
            oq.AddCriterion(Expression.Eq("ExecScheduleType", EnumExecScheduleType.周进度计划));
            oq.AddCriterion(Expression.Eq("SummaryStatus", EnumSummaryStatus.汇总生成));

            if (txtResponsiblePersonByQuery.Text.Trim() != "" && txtResponsiblePersonByQuery.Result != null && txtResponsiblePersonByQuery.Result.Count > 0)
            {
                PersonInfo responsiblePerson = txtResponsiblePersonByQuery.Result[0] as PersonInfo;
                oq.AddCriterion(Expression.Eq("HandlePerson.Id", responsiblePerson.Id));
            }

            oq.AddCriterion(Expression.Ge("CreateDate", dtpDateBeginByQuery.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", dtpDateEndByQuery.Value.AddDays(1).Date));


            ObjectQuery oqChild = new ObjectQuery();
            //oqChild.AddCriterion(Expression.Eq("GWBSConfirmFlag", 0));
            //oqChild.AddCriterion(Expression.Eq("ScheduleConfirmFlag", 0));//能查出已确认形象进度的周进度计划但是不能修改

            if (txtTaskBearByQuery.Text.Trim() != "" && txtTaskBearByQuery.Tag != null)
            {
                SubContractProject project = txtTaskBearByQuery.Tag as SubContractProject;
                oqChild.AddCriterion(Expression.Eq("SubContractProject.Id", project.Id));
            }
            if (txtTaskNameByQuery.Text.Trim() != "")
            {
                oqChild.AddCriterion(Expression.Like("GWBSTree.Name", txtTaskNameByQuery.Text.Trim(), MatchMode.Anywhere));
            }

            IList listDtl = model.ProductionManagementSrv.ObjectQuery(typeof(WeekScheduleDetail), oqChild);
            Disjunction dis = new Disjunction();
            if (listDtl.Count > 0)
            {
                IEnumerable<WeekScheduleDetail> queryDtl = listDtl.OfType<WeekScheduleDetail>();
                var queryMaster = from d in queryDtl
                                  group d by new { d.Master.Id }
                                      into g
                                      select new
                                      {
                                          g.Key.Id
                                      };

                foreach (var parent in queryMaster)
                {
                    dis.Add(Expression.Eq("Id", parent.Id));
                }
            }
            oq.AddCriterion(dis);


            try
            {
                IList list = model.ProductionManagementSrv.ObjectQuery(typeof(WeekScheduleMaster), oq);
                FillMaster(list);

            }
            catch (Exception ex)
            {
                MessageBox.Show("查询周进度计划出错。\n" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        private void FillMaster(IList masterList)
        {
            dgMaster.Rows.Clear();
            if (masterList == null || masterList.Count == 0) return;
            foreach (WeekScheduleMaster master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                DataGridViewRow dr = dgMaster.Rows[rowIndex];
                dr.Tag = master;

                dr.Cells[colMasterProjectName.Name].Value = master.ProjectName;
                dr.Cells[colMasterPlanName.Name].Value = master.PlanName;
                dr.Cells[colMasterCreateDate.Name].Value = master.CreateDate.ToShortDateString();
                dr.Cells[colMasterPlannedBeginDate.Name].Value = master.PlannedBeginDate.ToShortDateString();
                dr.Cells[colMasterPlannedDateEnd.Name].Value = master.PlannedEndDate.ToShortDateString();
                dr.Cells[colMasterHandlePerson.Name].Value = master.HandlePersonName;
                dr.Cells[colMasterDescript.Name].Value = master.Descript;
            }
            dgMaster.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            if (dgMaster.Rows.Count > 0)
            {
                dgMaster.CurrentCell = dgMaster.Rows[0].Cells[0];
                dgMaster_CellClick(dgMaster, new DataGridViewCellEventArgs(0, 0));
            }
        }
        private void FillDgDetail(WeekScheduleMaster master)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", master.Id));
            //oq.AddCriterion(Expression.Eq("GWBSConfirmFlag", 0));
            //oq.AddCriterion(Expression.Eq("ScheduleConfirmFlag", 0));
            oq.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("SubContractProject", NHibernate.FetchMode.Eager);

            IList listDtl = model.ProductionManagementSrv.ObjectQuery(typeof(WeekScheduleDetail), oq);

            dgDetail.Rows.Clear();
            foreach (WeekScheduleDetail dtl in listDtl)
            {
                int i = this.dgDetail.Rows.Add();
                DataGridViewRow row = dgDetail.Rows[i];

                if (dtl.GWBSTree != null)
                {
                    row.Cells[colGWBSTree.Name].Value = dtl.GWBSTree.Name;
                    row.Cells[colGWBSTree.Name].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.GWBSTree);
                }

                row.Cells[colMainTaskContent.Name].Value = dtl.MainTaskContent;
                if (dtl.SubContractProject != null)
                    row.Cells[colBear.Name].Value = dtl.SubContractProject.BearerOrgName;

                row.Cells[colPlanBeginTime.Name].Value = dtl.PlannedBeginDate.ToShortDateString();
                row.Cells[colPlanOverTime.Name].Value = dtl.PlannedEndDate.ToShortDateString();
                row.Cells[colPlannedDuration.Name].Value = dtl.PlannedDuration;
                row.Cells[colPlannedWrokload.Name].Value = dtl.PlannedWrokload;

                row.Cells[colDescript.Name].Value = dtl.Descript;

                row.Tag = dtl;
            }

            if (dgDetail.Rows.Count > 0)
                FillDgDetail(dgDetail.Rows[0].Tag as WeekScheduleDetail);
        }
        private void FillDgDetail(WeekScheduleDetail detail)
        {
            if (detail == null)
                return;
            if (detail.ScheduleConfirmFlag == 1)
            {
                gbWeekPlanDtlInfo.Enabled = false;
            }
            else
            {
                gbWeekPlanDtlInfo.Enabled = true;
            }

            if (detail.GWBSTree != null)
            {
                txtTaskName.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), detail.GWBSTree);
            }

            txtPlanBeginTime.Text = detail.PlannedBeginDate.ToLongDateString();
            txtPlanOverTime.Text = detail.PlannedEndDate.ToLongDateString();

            txtPlanProjectTime.Text = detail.PlannedDuration.ToString();

            if (detail.SubContractProject != null)
                txtTaskBearer.Text = detail.SubContractProject.BearerOrgName;

            txtCheckState.Text = StaticMethod.GetCheckStatePassStr(detail.TaskCheckState);
            txtWorkContent.Text = detail.MainTaskContent;


            dtFactBeginTime.Value = detail.ActualBeginDate;
            if (detail.ActualEndDate == DateTime.Parse("1900-1-1") && detail.ActualBeginDate != DateTime.Parse("1900-1-1"))
                dtFactEndTime.Value = detail.ActualBeginDate.AddDays(6);
            else
                dtFactEndTime.Value = detail.ActualEndDate;

            if (detail.GWBSTree != null)
                txtAddupFigureProgress.Text = detail.GWBSTree.AddUpFigureProgress.ToString();

            txtFigureProgress.Text = detail.TaskCompletedPercent.ToString();
            txtFactProjectQuantity.Text = detail.ActualWorklaod.ToString();

            txtCompletionAnalysis.Text = detail.CompletionAnalysis;

            //panelDtlEdit.Enabled = StaticMethod.GetCheckStatePassStr(detail.TaskCheckState) == "通过" ? true : false;
        }

        private void ClearControlData()
        {
            dgMaster.Rows.Clear();
            dgDetail.Rows.Clear();

            ClearChildControlData(gbWeekPlanDtlInfo);
        }
        private void ClearChildControlData(Control parentControl)
        {
            foreach (Control c in parentControl.Controls)
            {
                if (c is TextBox)
                {
                    (c as TextBox).Text = "";
                }
                else if (c is CustomEdit)
                {
                    (c as CustomEdit).Text = "";
                }
                else if (c is DateTimePicker)
                {
                    (c as DateTimePicker).Value = DateTime.Now;
                }
                else if (c.Controls != null && c.Controls.Count > 0)
                {
                    ClearChildControlData(c);
                }
            }
        }
    }
}
