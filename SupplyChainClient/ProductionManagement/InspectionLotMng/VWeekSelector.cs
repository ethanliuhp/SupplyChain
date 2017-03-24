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
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionLotMng
{
    public partial class VWeekSelector : TBasicDataView
    {
        MProductionMng model = new MProductionMng();

        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }
        public VWeekSelector()
        {
            InitializeComponent();
            InitData();
            InitEvent();
        }
        private void InitData()
        {
            dtpDateBegin.Value = ConstObject.LoginDate.AddDays(-7);
            dtpDateEnd.Value = ConstObject.LoginDate;
        }
        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnCancle.Click += new EventHandler(btnCancle_Click);
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnCancle_Click(object sender, EventArgs e)
        {
            this.btnCancle.FindForm().Close();
        }
        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnOK_Click(object sender, EventArgs e)
        {
            if (dgDetail.Rows.Count > 0 && this.dgDetail.SelectedRows == null)
            {
                MessageBox.Show("请选择一条周进度计划明细！");
                return;
            }
            WeekScheduleDetail detail = dgDetail.SelectedRows[0].Tag as WeekScheduleDetail;
            result.Add(detail);

            this.btnOK.FindForm().Close();
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearch_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            IList list = new ArrayList();
            //CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            //oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            //oq.AddCriterion(Expression.Eq("ProjectName", projectInfo.Name));
            oq.AddCriterion(Expression.Ge("Master.CreateDate", dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("Master.CreateDate", dtpDateEnd.Value.AddDays(1).Date));
            if (txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Between("Master.Code", txtCodeBegin.Text, txtCodeEnd.Text));
            }

            if (txtCreatePerson.Text != "" && txtCreatePerson.Result.Count > 0)
            {
                oq.AddCriterion(Expression.Eq("Master.CreatePerson", txtCreatePerson.Result[0] as PersonInfo));
            }

            //if (txtGWBSName.Text != "")
            //{
            //    oq.AddCriterion(Expression.Like("GWBSTreeName", txtGWBSName.Text, MatchMode.Anywhere));
            //}
            oq.AddFetchMode("Master", NHibernate.FetchMode.Eager);
            try
            {
                //oq.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);
                //oq.AddFetchMode("PBSTree", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("WeekScheduleDetail", NHibernate.FetchMode.Eager);
                list = model.ProductionManagementSrv.GetWeekScheduleDetail(oq);
                ShowMasterList(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
        }

        /// <summary>
        /// 显示主表及明细信息
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList masterList)
        {
            dgDetail.Rows.Clear();
            if (masterList == null || masterList.Count == 0) return;
            foreach (WeekScheduleDetail detail in masterList)
            {
                int rowIndex = dgDetail.Rows.Add();
                dgDetail.Rows[rowIndex].Tag = detail;
                dgDetail[colWeekScheduleName.Name, rowIndex].Tag = detail.Id;
                dgDetail[colWeekScheduleName.Name, rowIndex].Value = detail.MainTaskContent;//主要工作内容
                dgDetail[colDgMasterCode.Name, rowIndex].Value = detail.Master.Code;
                dgDetail[colDgMasterGWBSName.Name, rowIndex].Value = detail.GWBSTreeName;
                dgDetail[colDgMasterPBSName.Name, rowIndex].Value = detail.PBSTreeName;
                dgDetail[colDgMasterPlannedBeginDate.Name, rowIndex].Value = detail.PlannedBeginDate.ToShortDateString();
                dgDetail[colDgMasterPlannedEndDate.Name, rowIndex].Value = detail.PlannedEndDate.ToShortDateString();
                dgDetail[colDgMasterDescript.Name, rowIndex].Value = detail.Descript;
            }
            if (dgDetail.Rows.Count == 0) return;
            dgDetail.CurrentCell = dgDetail[1, 0];
        }

    }
}
