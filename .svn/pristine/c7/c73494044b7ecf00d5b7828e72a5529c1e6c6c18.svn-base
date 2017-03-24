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
using Application.Business.Erp.ResourceManager.Client.Basic.Template;

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
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using System.Windows.Documents;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng;

using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using ImportIntegration;
using System.IO;
using System.Diagnostics;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Business.Erp.SupplyChain.PMCAndWarning.Domain;
using System.Data.OleDb;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI;


namespace Application.Business.Erp.SupplyChain.Client.PMCAndWarning
{
    public partial class VWarningInfoMng : TBasicDataView
    {

        CurrentProjectInfo projectInfo = null;

        public MPMCAndWarning model = new MPMCAndWarning();

        public VWarningInfoMng()
        {
            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();

            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();

            if (projectInfo != null)
            {
                txtProjectName.Text = projectInfo.Name;
                txtProjectName.Tag = projectInfo;
            }

            foreach (string mode in Enum.GetNames(typeof(WarningLevelEnum)))
            {
                cbWarningLevel.Items.Add(mode);
            }

            foreach (string state in Enum.GetNames(typeof(WarningInfoStateEnum)))
            {
                cbState.Items.Add(state);
            }

            dtSubmitStartTime.Value = DateTime.Now.AddDays(-7);
            dtSubmitEndTime.Value = DateTime.Now;
        }

        private void InitEvents()
        {
            btnSelectProject.Click += new EventHandler(btnSelectProject_Click);
            btnSelectWarningTarget.Click += new EventHandler(btnSelectWarningTarget_Click);

            btnQuery.Click += new EventHandler(btnQuery_Click);

            btnDelete.Click += new EventHandler(btnDelete_Click);

        }

        void btnSelectWarningTarget_Click(object sender, EventArgs e)
        {
            VSelectWarningTarget frm = new VSelectWarningTarget();
            frm.ShowDialog();

            if (frm.IsOK)
            {
                List<WarningTarget> listTarget = frm.SelectTargets;
                WarningTarget selectTarget = listTarget[0];

                if (selectTarget != null)
                {
                    txtWarningTargetName.Text = selectTarget.TargetName;
                    txtWarningTargetName.Tag = selectTarget;
                }
            }
        }

        void btnSelectProject_Click(object sender, EventArgs e)
        {
            VDepartSelector frm = new VDepartSelector("1");

            frm.ShowDialog();

            if (frm.Result != null && frm.Result.Count > 0)
            {
                CurrentProjectInfo selectProject = frm.Result[0] as CurrentProjectInfo;
                if (selectProject != null)
                {
                    txtProjectName.Text = selectProject.Name;
                    txtProjectName.Tag = selectProject;
                }
            }
        }


        void btnDelete_Click(object sender, EventArgs e)
        {
            if (gridWarningInfo.Rows.Count == 0 || gridWarningInfo.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的行！");
                return;
            }

            if (MessageBox.Show("您确认要删除吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            List<int> listRowIndex = new List<int>();

            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (DataGridViewRow row in gridWarningInfo.SelectedRows)
            {
                WarningInfo warnInfo = row.Tag as WarningInfo;
                if (!string.IsNullOrEmpty(warnInfo.Id))
                    dis.Add(Expression.Eq("Id", warnInfo.Id));

                listRowIndex.Add(row.Index);
            }
            oq.AddCriterion(dis);

            IList list = model.ObjectQuery(typeof(WarningInfo), oq);
            if (list.Count > 0)
            {
                model.DeleteObjList(list);
            }

            listRowIndex.Sort();
            for (int i = listRowIndex.Count - 1; i > -1; i--)
            {
                gridWarningInfo.Rows.RemoveAt(listRowIndex[i]);
            }

        }

        private void LoadCheckActionInGrid(IEnumerable<WarningInfo> list)
        {
            gridWarningInfo.Rows.Clear();

            if (list != null && list.Count() > 0)
            {
                //按所属项目、预警级别、预警指标 排序
                var queryWarnInfo = from w in list
                                    orderby w.TheTargetName ascending
                                    orderby (int)w.Level descending
                                    orderby w.ProjectName ascending
                                    select w;

                foreach (Application.Business.Erp.SupplyChain.PMCAndWarning.Domain.WarningInfo warn in queryWarnInfo)
                {
                    int index = gridWarningInfo.Rows.Add();
                    DataGridViewRow row = gridWarningInfo.Rows[index];
                    row.Cells[colWarnProjectName.Name].Value = warn.ProjectName;
                    row.Cells[colWarnLevel.Name].Value = warn.Level.ToString();
                    row.Cells[colWarnTargetName.Name].Value = warn.TheTargetName;
                    row.Cells[colState.Name].Value = warn.State;
                    row.Cells[colWarnContent.Name].Value = warn.WarningContent;
                    if (warn.SubmitTime != null)
                        row.Cells[colWarnSubmitTime.Name].Value = warn.SubmitTime.Value;

                    row.Tag = warn;
                }
            }
        }


        void btnQuery_Click(object sender, EventArgs e)
        {

            ObjectQuery oq = new ObjectQuery();
            if (txtProjectName.Text.Trim() != "" && txtProjectName.Tag != null)
            {
                CurrentProjectInfo selProject = txtProjectName.Tag as CurrentProjectInfo;
                if (selProject != null)
                    oq.AddCriterion(Expression.Eq("ProjectId", selProject.Id));
            }
            else if (txtProjectName.Text.Trim() != "")
            {
                oq.AddCriterion(Expression.Like("ProjectName", txtProjectName.Text.Trim(), MatchMode.Anywhere));
            }

            if (txtWarningTargetName.Text.Trim() != "" && txtWarningTargetName.Tag != null)
            {
                WarningTarget selTarget = txtWarningTargetName.Tag as WarningTarget;
                if (selTarget != null)
                    oq.AddCriterion(Expression.Eq("TheTarget.Id", selTarget.Id));
            }
            else if (txtWarningTargetName.Text.Trim() != "")
            {
                oq.AddCriterion(Expression.Like("TheTargetName", txtWarningTargetName.Text.Trim(), MatchMode.Anywhere));
            }

            if (cbWarningLevel.Text.Trim() != "")
            {
                oq.AddCriterion(Expression.Eq("Level", VirtualMachine.Component.Util.EnumUtil<WarningLevelEnum>.FromDescription(cbWarningLevel.Text.Trim())));
            }

            if (cbState.Text.Trim() != "")
                oq.AddCriterion(Expression.Eq("State", VirtualMachine.Component.Util.EnumUtil<WarningInfoStateEnum>.FromDescription(cbState.Text.Trim())));

            oq.AddCriterion(Expression.Ge("SubmitTime", dtSubmitStartTime.Value.Date));
            oq.AddCriterion(Expression.Lt("SubmitTime", dtSubmitEndTime.Value.Date.AddDays(1)));


            IEnumerable<WarningInfo> list = model.ObjectQuery(typeof(WarningInfo), oq).OfType<WarningInfo>();

            LoadCheckActionInGrid(list);

        }

        public override void RefreshControls(MainViewState state)
        {
            switch (state)
            {
                case MainViewState.AddNew:


                    break;

                case MainViewState.Modify:


                    break;

                case MainViewState.Browser:


                    break;

                case MainViewState.Initialize://添加根节点


                    break;
            }

            ViewState = state;
        }

        public override bool ModifyView()
        {

            return false;
        }

        public override bool CancelView()
        {
            try
            {

                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        public override void RefreshView()
        {
            try
            {

            }
            catch (Exception e)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(e));
            }
        }

        public override bool SaveView()
        {
            bool isNew = false;
            try
            {
                //if (!ValideSave())
                //    return false;



                RefreshControls(MainViewState.Browser);

                return true;
            }
            catch (Exception exp)
            {
                if (exp.InnerException != null && exp.InnerException.Message.Contains("违反唯一约束条件"))
                    MessageBox.Show("编码必须唯一！");
                else
                    MessageBox.Show("保存失败，异常信息：" + ExceptionUtil.ExceptionMessage(exp));
            }
            return false;
        }

    }
}
