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
using Application.Business.Erp.SupplyChain.Client.AppMng.AppSolutionSetUI;


namespace Application.Business.Erp.SupplyChain.Client.PMCAndWarning
{
    public partial class VWarningPushModeMng : TBasicDataView
    {
        WarningPushMode optPushMode = null;

        CurrentProjectInfo projectInfo = null;

        public MPMCAndWarning model = new MPMCAndWarning();

        public VWarningPushModeMng()
        {
            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();

            ViewState = MainViewState.AddNew;

            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();

            foreach (string value in Enum.GetNames(typeof(WarningPushModeEnum)))
            {
                cbPushModeQuery.Items.Add(value);
                cbPushMode.Items.Add(value);
            }
            cbPushMode.SelectedIndex = 0;
            foreach (string value in Enum.GetNames(typeof(WarningLevelEnum)))
            {
                cbWarningLevelQuery.Items.Add(value);
                cbWarningLevel.Items.Add(value);
            }
            cbWarningLevel.SelectedIndex = 0;
        }

        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);

            btnSelectRoleQuery.Click += new EventHandler(btnSelectRoleQuery_Click);
            btnSelectTargetQuery.Click += new EventHandler(btnSelectTargetQuery_Click);

            btnSelectRole.Click += new EventHandler(btnSelectRole_Click);
            btnSelectTarget.Click += new EventHandler(btnSelectTarget_Click);

            btnRemoveRole.Click += new EventHandler(btnRemoveRole_Click);
            btnRemoveTarget.Click += new EventHandler(btnRemoveTarget_Click);

            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
        }

        void btnRemoveTarget_Click(object sender, EventArgs e)
        {
            WarningTarget item = listBoxTarget.SelectedItem as WarningTarget;

            if (item != null)
            {
                listBoxTarget.Items.Remove(item);

                if (listBoxTarget.Items.Count > 0)
                    listBoxTarget.SelectedIndex = 0;
            }
        }

        void btnRemoveRole_Click(object sender, EventArgs e)
        {
            OperationRole item = listBoxRole.SelectedItem as OperationRole;

            if (item != null)
            {
                listBoxRole.Items.Remove(item);

                if (listBoxRole.Items.Count > 0)
                    listBoxRole.SelectedIndex = 0;
            }
        }


        void btnSelectRoleQuery_Click(object sender, EventArgs e)
        {
            VSelectOperationRole frm = new VSelectOperationRole();
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                OperationRole role = frm.Result[0];
                if (role != null)
                {
                    txtRoleNameQuery.Text = role.RoleName;
                    txtRoleNameQuery.Tag = role;
                }
            }
        }
        void btnSelectTargetQuery_Click(object sender, EventArgs e)
        {
            VSelectWarningTarget frm = new VSelectWarningTarget();
            frm.ShowDialog();
            if (frm.IsOK)
            {
                List<WarningTarget> listTarget = frm.SelectTargets;
                if (listTarget != null && listTarget.Count > 0)
                {
                    WarningTarget target = listTarget.ElementAt(0);

                    txtTargetNameQuery.Text = target.TargetName;
                    txtTargetNameQuery.Tag = target;
                }
            }
        }

        void btnSelectRole_Click(object sender, EventArgs e)
        {
            VSelectOperationRole frm = new VSelectOperationRole();
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                foreach (OperationRole role in frm.Result)
                {
                    listBoxRole.Items.Add(role);
                }
                listBoxRole.DisplayMember = "RoleName";
            }
        }
        void btnSelectTarget_Click(object sender, EventArgs e)
        {
            VSelectWarningTarget frm = new VSelectWarningTarget();
            frm.ShowDialog();
            if (frm.IsOK)
            {
                List<WarningTarget> listTarget = frm.SelectTargets;
                foreach (WarningTarget target in listTarget)
                {
                    listBoxTarget.Items.Add(target);
                }
                listBoxTarget.DisplayMember = "TargetName";
            }
        }

        void btnAdd_Click(object sender, EventArgs e)
        {
            ClearEditData();

            optPushMode = null;

            ViewState = MainViewState.AddNew;
        }

        private void ClearEditData()
        {
            listBoxRole.Items.Clear();
            listBoxTarget.Items.Clear();

            cbPushMode.SelectedIndex = 0;
            cbWarningLevel.SelectedIndex = 0;

            btnSelectRole.Focus();
        }

        void btnUpdate_Click(object sender, EventArgs e)
        {
            if (gridWarningPush.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要修改的行！");
                return;
            }
            if (gridWarningPush.SelectedRows.Count > 1)
            {
                MessageBox.Show("一次只能修改一行！");
                return;
            }

            ViewState = MainViewState.Modify;

            optPushMode = gridWarningPush.SelectedRows[0].Tag as WarningPushMode;

            ClearEditData();

            listBoxRole.Items.Add(optPushMode.UserRole);
            listBoxRole.DisplayMember = "RoleName";

            listBoxTarget.Items.Add(optPushMode.TheTarget);
            listBoxTarget.DisplayMember = "TargetName";

            cbPushMode.Text = optPushMode.PushMode.ToString();
            cbWarningLevel.Text = optPushMode.WarningLevel.ToString();
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (gridWarningPush.Rows.Count == 0 || gridWarningPush.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的行！");
                return;
            }

            if (MessageBox.Show("您确认要删除选择行吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            List<int> listRowIndex = new List<int>();

            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (DataGridViewRow row in gridWarningPush.SelectedRows)
            {
                WarningPushMode item = row.Tag as WarningPushMode;
                if (!string.IsNullOrEmpty(item.Id))
                    dis.Add(Expression.Eq("Id", item.Id));

                listRowIndex.Add(row.Index);
            }
            oq.AddCriterion(dis);

            IList list = model.ObjectQuery(typeof(WarningPushMode), oq);
            if (list.Count > 0)
            {
                model.DeleteObjList(list);
            }

            listRowIndex.Sort();
            for (int i = listRowIndex.Count - 1; i > -1; i--)
            {
                gridWarningPush.Rows.RemoveAt(listRowIndex[i]);
            }

            ClearEditData();
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (listBoxRole.Items.Count == 0)
            {
                MessageBox.Show("请至少选择一个角色！");
                return;
            }
            else if (listBoxTarget.Items.Count == 0)
            {
                MessageBox.Show("请至少选择一个指标！");
                return;
            }

            WarningPushModeEnum pubshMode = VirtualMachine.Component.Util.EnumUtil<WarningPushModeEnum>.FromDescription(cbPushMode.Text);
            WarningLevelEnum warnLevel = VirtualMachine.Component.Util.EnumUtil<WarningLevelEnum>.FromDescription(cbWarningLevel.Text);

            if (ViewState == MainViewState.AddNew)
            {
                IList listPushMode = new ArrayList();

                foreach (OperationRole role in listBoxRole.Items)
                {
                    foreach (WarningTarget target in listBoxTarget.Items)
                    {
                        WarningPushMode item = new WarningPushMode();
                        item.PushMode = pubshMode;
                        item.WarningLevel = warnLevel;

                        item.UserRole = role;
                        item.UserRoleName = role.RoleName;
                        item.TheTarget = target;

                        listPushMode.Add(item);
                    }
                }

                listPushMode = model.SaveOrUpdate(listPushMode);

                LoadPushModeInGrid(listPushMode);

                MessageBox.Show("保存成功！");
            }
            else if (ViewState == MainViewState.Modify)
            {
                if (listBoxRole.Items.Count > 1)
                {
                    MessageBox.Show("修改时只能选择一个角色！");
                    return;
                }
                else if (listBoxTarget.Items.Count > 1)
                {
                    MessageBox.Show("修改时只能选择一个指标！");
                    return;
                }
                OperationRole role = listBoxRole.Items[0] as OperationRole;
                WarningTarget target = listBoxTarget.Items[0] as WarningTarget;

                optPushMode.PushMode = pubshMode;
                optPushMode.WarningLevel = warnLevel;

                optPushMode.UserRole = role;
                optPushMode.UserRoleName = role.RoleName;
                optPushMode.TheTarget = target;

                optPushMode = model.SaveOrUpdate(optPushMode) as WarningPushMode;

                foreach (DataGridViewRow row in gridWarningPush.Rows)
                {
                    WarningPushMode temp = row.Tag as WarningPushMode;
                    if (temp != null && temp.Id == optPushMode.Id)
                    {
                        row.Cells[colRole.Name].Value = optPushMode.UserRoleName;
                        row.Cells[colTarget.Name].Value = optPushMode.TheTarget.TargetName;
                        row.Cells[colPushMode.Name].Value = optPushMode.PushMode.ToString();
                        row.Cells[colWarningLevel.Name].Value = optPushMode.WarningLevel.ToString();

                        row.Tag = optPushMode;
                        break;
                    }
                }

                MessageBox.Show("保存成功！");
            }

            ClearEditData();
            optPushMode = null;
        }

        private void LoadPushModeInGrid(IList list)
        {
            gridWarningPush.Rows.Clear();

            if (list == null || list.Count == 0)
                return;

            foreach (WarningPushMode item in list)
            {
                int index = gridWarningPush.Rows.Add();
                DataGridViewRow row = gridWarningPush.Rows[index];

                row.Cells[colRole.Name].Value = item.UserRoleName;
                row.Cells[colTarget.Name].Value = item.TheTarget.TargetName;
                row.Cells[colPushMode.Name].Value = item.PushMode.ToString();
                row.Cells[colWarningLevel.Name].Value = item.WarningLevel.ToString();

                row.Tag = item;
            }

            if (gridWarningPush.Rows.Count > 0)
            {
                gridWarningPush.CurrentCell = gridWarningPush.Rows[0].Cells[0];
            }
        }

        void btnQuery_Click(object sender, EventArgs e)
        {

            ObjectQuery oq = new ObjectQuery();

            if (txtTargetNameQuery.Text.Trim() != "" && txtTargetNameQuery.Tag != null)
            {
                WarningTarget target = txtTargetNameQuery.Tag as WarningTarget;
                if (target != null)
                    oq.AddCriterion(Expression.Eq("TheTarget.Id", target.Id));
            }
            else if (txtTargetNameQuery.Text.Trim() != "" && txtTargetNameQuery.Tag == null)
            {
                oq.AddCriterion(Expression.Like("TheTarget.TargetName", txtTargetNameQuery.Text.Trim(), MatchMode.Anywhere));
            }

            if (txtRoleNameQuery.Text.Trim() != "" && txtRoleNameQuery.Tag != null)
            {
                OperationRole role = txtRoleNameQuery.Tag as OperationRole;
                if (role != null)
                    oq.AddCriterion(Expression.Eq("UserRole.Id", role.Id));
            }
            else if (txtRoleNameQuery.Text.Trim() != "" && txtRoleNameQuery.Tag == null)
            {
                oq.AddCriterion(Expression.Like("UserRoleName", txtRoleNameQuery.Text.Trim(), MatchMode.Anywhere));
            }

            if (cbPushModeQuery.Text.Trim() != "")
            {
                oq.AddCriterion(Expression.Eq("PushMode", VirtualMachine.Component.Util.EnumUtil<WarningPushModeEnum>.FromDescription(cbPushModeQuery.Text.Trim())));
            }
            if (cbWarningLevelQuery.Text.Trim() != "")
            {
                oq.AddCriterion(Expression.Eq("WarningLevel", VirtualMachine.Component.Util.EnumUtil<WarningLevelEnum>.FromDescription(cbWarningLevelQuery.Text.Trim())));
            }

            oq.AddFetchMode("TheTarget", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("UserRole", NHibernate.FetchMode.Eager);

            IList list = model.ObjectQuery(typeof(WarningPushMode), oq);

            LoadPushModeInGrid(list);

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
