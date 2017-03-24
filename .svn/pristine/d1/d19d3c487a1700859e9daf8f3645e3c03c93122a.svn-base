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


namespace Application.Business.Erp.SupplyChain.Client.PMCAndWarning
{
    public partial class VSelectWarningTarget : TBasicDataView
    {
        public bool IsOK = false;

        /// <summary>
        /// 选择的指标集
        /// </summary>
        public List<WarningTarget> SelectTargets = new List<WarningTarget>();

        public MPMCAndWarning model = new MPMCAndWarning();

        public VSelectWarningTarget()
        {

            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();
        }

        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);

            btnEnter.Click += new EventHandler(btnEnter_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            btnSelectTarget.Click += new EventHandler(btnSelectTarget_Click);
            btnSelectAllTarget.Click += new EventHandler(btnSelectAllTarget_Click);

            btnRemoveSelect.Click += new EventHandler(btnRemoveSelect_Click);
            btnRemoveAll.Click += new EventHandler(btnRemoveAll_Click);

            gridCheckAction.CellClick += new DataGridViewCellEventHandler(gridCheckAction_CellClick);
        }
        void gridCheckAction_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;

            StateCheckAction checkAction = gridCheckAction.Rows[e.RowIndex].Tag as StateCheckAction;

            LoadWarningTargetInGrid(checkAction);
        }

        void btnRemoveSelect_Click(object sender, EventArgs e)
        {

            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridTargetSelect.SelectedRows)
            {
                listRowIndex.Add(row.Index);
            }

            listRowIndex.Sort();
            for (int i = listRowIndex.Count - 1; i > -1; i--)
            {
                gridTargetSelect.Rows.RemoveAt(listRowIndex[i]);
            }
        }
        void btnRemoveAll_Click(object sender, EventArgs e)
        {
            gridTargetSelect.Rows.Clear();
        }

        void btnSelectTarget_Click(object sender, EventArgs e)
        {
            if (gridTarget.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in gridTarget.SelectedRows)
                {
                    WarningTarget target = row.Tag as WarningTarget;
                    if (IsExistsWarningTarget(target) == false)
                        LoadWarningTargetInSelectGrid(target);
                }
            }
        }
        void btnSelectAllTarget_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in gridTarget.Rows)
            {
                WarningTarget target = row.Tag as WarningTarget;
                if (IsExistsWarningTarget(target) == false)
                    LoadWarningTargetInSelectGrid(target);
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            SelectTargets.Clear();
            IsOK = false;
            this.Close();
        }

        void btnEnter_Click(object sender, EventArgs e)
        {
            if (gridTargetSelect.Rows.Count == 0)
            {
                MessageBox.Show("请选择预警指标！");
                return;
            }
            foreach (DataGridViewRow row in gridTargetSelect.Rows)
            {
                WarningTarget target = row.Tag as WarningTarget;
                SelectTargets.Add(target);
            }

            IsOK = true;
            this.Close();
        }

        private void LoadCheckActionInGrid(IList list)
        {
            gridTarget.Rows.Clear();
            gridCheckAction.Rows.Clear();

            if (list == null || list.Count == 0)
                return;

            foreach (StateCheckAction action in list)
            {
                int index = gridCheckAction.Rows.Add();
                DataGridViewRow rowAction = gridCheckAction.Rows[index];

                rowAction.Cells[colActionName.Name].Value = action.ActionName;
                rowAction.Cells[colActionDesc.Name].Value = action.ActionDesc;
                rowAction.Cells[colActionTriggerMode.Name].Value = action.TriggerMode.ToString();
                rowAction.Cells[colActionTriggerTerm1.Name].Value = StaticMethod.DecimelTrimEnd0(action.TriggerTerm1);
                rowAction.Cells[colActionTriggerTerm2.Name].Value = action.TriggerTerm2;
                rowAction.Cells[colActionTriggerTerm3.Name].Value = action.TriggerTerm3;
                rowAction.Cells[colActionStartMode.Name].Value = action.StartMethod;

                rowAction.Tag = action;
            }

            if (gridCheckAction.Rows.Count > 0)
            {
                gridCheckAction.CurrentCell = gridCheckAction.Rows[0].Cells[0];
                LoadWarningTargetInGrid(gridCheckAction.Rows[0].Tag as StateCheckAction);
            }
        }

        private void LoadWarningTargetInGrid(StateCheckAction action)
        {
            gridTarget.Rows.Clear();

            if (action == null || action.ListTargets.Count == 0)
                return;

            foreach (WarningTarget target in action.ListTargets)
            {
                int index1 = gridTarget.Rows.Add();
                DataGridViewRow rowTarget = gridTarget.Rows[index1];

                rowTarget.Cells[colTargetCode.Name].Value = target.TargetCode;
                rowTarget.Cells[colTargetName.Name].Value = target.TargetName;
                rowTarget.Cells[colTargetDesc.Name].Value = target.TargetDesc;
                rowTarget.Cells[colTargetCate.Name].Value = target.TargetCate;

                rowTarget.Tag = target;
            }
        }

        private bool IsExistsWarningTarget(WarningTarget target)
        {
            foreach (DataGridViewRow row in gridTargetSelect.Rows)
            {
                WarningTarget tempTarget = row.Tag as WarningTarget;
                if (target.Id == tempTarget.Id)
                {
                    return true;
                }
            }
            return false;
        }

        private void LoadWarningTargetInSelectGrid(IList list)
        {
            gridTargetSelect.Rows.Clear();

            if (list == null || list.Count == 0)
                return;

            foreach (WarningTarget target in list)
            {
                int index1 = gridTargetSelect.Rows.Add();
                DataGridViewRow rowTarget = gridTargetSelect.Rows[index1];

                rowTarget.Cells[colTargetCodeSelect.Name].Value = target.TargetCode;
                rowTarget.Cells[colTargetNameSelect.Name].Value = target.TargetName;
                rowTarget.Cells[colTargetDescSelect.Name].Value = target.TargetDesc;
                rowTarget.Cells[colTargetCateSelect.Name].Value = target.TargetCate;

                rowTarget.Tag = target;
            }
        }
        private void LoadWarningTargetInSelectGrid(WarningTarget target)
        {

            int index1 = gridTargetSelect.Rows.Add();
            DataGridViewRow rowTarget = gridTargetSelect.Rows[index1];

            rowTarget.Cells[colTargetCodeSelect.Name].Value = target.TargetCode;
            rowTarget.Cells[colTargetNameSelect.Name].Value = target.TargetName;
            rowTarget.Cells[colTargetDescSelect.Name].Value = target.TargetDesc;
            rowTarget.Cells[colTargetCateSelect.Name].Value = target.TargetCate;

            rowTarget.Tag = target;
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            if (txtActionName.Text.Trim() != "")
            {
                oq.AddCriterion(Expression.Like("ActionName", txtActionName.Text.Trim(), MatchMode.Anywhere));
            }
            if (txtActionDesc.Text.Trim() != "")
            {
                oq.AddCriterion(Expression.Like("ActionDesc", txtActionDesc.Text.Trim(), MatchMode.Anywhere));
            }
            oq.AddFetchMode("ListTargets", NHibernate.FetchMode.Eager);

            IList list = model.ObjectQuery(typeof(StateCheckAction), oq);

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
