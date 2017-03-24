using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
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

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng
{
    public partial class VSelectWBSContractGroup : TBasicDataView
    {
        /// <summary>
        /// 是否单选
        /// </summary>
        private bool IsMultiSelect = false;
        private string[] _arrContractGroupType;
        public List<ContractGroup> SelectResult = new List<ContractGroup>();

        /// <summary>
        ///缺省选择的契约组
        /// </summary>
        public ContractGroup DefaultSelectedContract = null;

        public MWBSContractGroup model;

        public VSelectWBSContractGroup(MWBSContractGroup mot)
        {
            model = mot;

            InitializeComponent();

            InitForm();
        }
        public VSelectWBSContractGroup(MWBSContractGroup mot, bool isMultiSel)
        {
            model = mot;

            InitializeComponent();

            InitForm();

            IsMultiSelect = isMultiSel;

            if (IsMultiSelect)
            {
                gridContractGroup.MultiSelect = isMultiSel;
            }
        }
        public VSelectWBSContractGroup(MWBSContractGroup mot, bool isMultiSel, string[] arrContractGroupType)
        {
            _arrContractGroupType = arrContractGroupType;
            model = mot;

            InitializeComponent();

            InitForm();

            IsMultiSelect = isMultiSel;

            if (IsMultiSelect)
            {
                gridContractGroup.MultiSelect = isMultiSel;
            }
        }
        private void InitForm()
        {
            IList list = null;
            InitEvents();

            //契约组类型
            cbContractTypeQuery.Items.Add("");
            //xl20160912 工程成本维护中签证变更 需要传递 契约类型:工程签证索赔 设计变更单
            if (this._arrContractGroupType != null && this._arrContractGroupType.Length != 0)
            {
                list = new List<BasicDataOptr>();
                foreach (string sName in this._arrContractGroupType)
                {
                    list.Add(new BasicDataOptr() { BasicName = sName });
                }
            }
            else
            {
                list = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetBasicDataByName(VBasicDataOptr.WBS_ContractGroupType);
            }
            //list = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetBasicDataByName(VBasicDataOptr.WBS_ContractGroupType);
            if (list != null)
            {
                foreach (BasicDataOptr bdo in list)
                {
                    cbContractTypeQuery.Items.Add(bdo.BasicName);
                }
            }
            cbContractTypeQuery.SelectedIndex = 0;

            dtStartDate.Value = DateTime.Now.AddMonths(-1);
            dtEndDate.Value = DateTime.Now;
        }

        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);

            gridContractGroup.CellMouseClick += new DataGridViewCellMouseEventHandler(gridContractGroup_CellMouseClick);
            gridContractGroup.CellDoubleClick += new DataGridViewCellEventHandler(gridContractGroup_CellDoubleClick);

            cbContractTypeQuery.SelectedIndexChanged +=new EventHandler(cbContractTypeQuery_SelectedIndexChanged);

            btnSelect.Click += new EventHandler(btnSelect_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            this.Load += new EventHandler(VSelectWBSContractGroup_Load);
            this.FormClosing += new FormClosingEventHandler(VSelectWBSContractGroup_FormClosing);
        }

        void cbContractTypeQuery_SelectedIndexChanged(object sender,EventArgs e)
        {
            string strContractType = cbContractTypeQuery.SelectedItem.ToString();
            if (strContractType == "分包合同" || strContractType == "分包合同补充协议" || strContractType == "分包签证" || strContractType == "")
            {
                colBearRange.Visible = true;
                colSettleType.Visible = true;
                colSingDate.Visible = true;
            }
            else
            {
                colSingDate.Visible = false;
                colSettleType.Visible = false;
                colBearRange.Visible = false;
            }
        }

        void VSelectWBSContractGroup_Load(object sender, EventArgs e)
        {
            if (DefaultSelectedContract != null)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", DefaultSelectedContract.Id));
                oq.AddFetchMode("ChangeContract", NHibernate.FetchMode.Eager);
                //xl20160912
                if (this._arrContractGroupType != null && this._arrContractGroupType.Length>0)
                oq.AddCriterion(Expression.In("ContractGroupType", this._arrContractGroupType));
                IList listContract = model.ObjectQuery(typeof(ContractGroup), oq);

                if (listContract != null && listContract.Count > 0)
                {
                    foreach (ContractGroup cg in listContract)
                    {
                        int index = gridContractGroup.Rows.Add();
                        DataGridViewRow row = gridContractGroup.Rows[index];
                        row.Cells[ContractName.Name].Value = cg.ContractName;
                        row.Cells[ContractCode.Name].Value = cg.Code;
                        row.Cells[ContractBusinessCode.Name].Value = cg.ContractNumber;
                        if (cg.ChangeContract != null)
                            row.Cells[ChangeContract.Name].Value = cg.ChangeContract.Code;
                        row.Cells[ContractVersion.Name].Value = cg.ContractVersion;
                        row.Cells[ContractType.Name].Value = cg.ContractGroupType;
                        row.Cells[ContractState.Name].Value = cg.State.ToString();
                        row.Cells[ContractCreatePerson.Name].Value = cg.CreatePersonName;
                        row.Cells[ContractCreateDate.Name].Value = cg.CreateDate.ToString();
                        row.Cells[ContractDesc.Name].Value = cg.ContractDesc;
                        row.Tag = cg;
                    }
                    gridContractGroup.CurrentCell = gridContractGroup.Rows[0].Cells[0];

                    gridContractGroup_CellMouseClick(gridContractGroup.Rows[0].Cells[0], new DataGridViewCellMouseEventArgs(0, 0, 0, 0, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0)));
                }
            }
        }

        public bool isOK = false;
        void btnSelect_Click(object sender, EventArgs e)
        {
            if (gridContractGroup.SelectedRows.Count == 0)
            {
                MessageBox.Show("请至少选择一条契约组！");
                return;
            }

            foreach (DataGridViewRow row in gridContractGroup.SelectedRows)
            {
                SelectResult.Add(row.Tag as ContractGroup);
            }

            isOK = true;
            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            SelectResult.Clear();
            this.Close();
        }

        void VSelectWBSContractGroup_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isOK)
                SelectResult.Clear();
        }

        void gridContractGroup_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                //加载明细
                ContractGroup cg = gridContractGroup.Rows[e.RowIndex].Tag as ContractGroup;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", cg.Id));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                IList list = model.ObjectQuery(typeof(ContractGroup), oq);

                gridContractGroupDetail.Rows.Clear();
                if (list.Count > 0)
                {
                    ContractGroup parent = list[0] as ContractGroup;
                    foreach (ContractGroupDetail dtl in parent.Details)
                    {
                        int index = gridContractGroupDetail.Rows.Add();
                        DataGridViewRow row = gridContractGroupDetail.Rows[index];
                        row.Cells[ContractDetailDocName.Name].Value = dtl.ContractDocName;
                        row.Cells[ContractDetailDocDesc.Name].Value = dtl.ContractDocDesc;
                        row.Cells[ContractDetailRemark.Name].Value = dtl.Remark;
                        row.Cells[ContractDetailOwner.Name].Value = dtl.CreatePersonName;

                        row.Cells[ContractDetailCreateDate.Name].Value = dtl.CreateTime.ToString();
                        row.Tag = dtl;
                        row.ReadOnly = true;
                    }
                }
            }
        }

        void gridContractGroup_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                //加载明细
                ContractGroup cg = gridContractGroup.Rows[e.RowIndex].Tag as ContractGroup;

                SelectResult.Add(cg);

                isOK = true;
                this.Close();
            }
        }

        public void Start()
        {
            RefreshState(MainViewState.Browser);
        }

        public override void RefreshControls(MainViewState state)
        {
            switch (state)
            {
                case MainViewState.Modify:

                    break;

                case MainViewState.Browser:

                    break;
            }
        }

        //查询
        private void btnQuery_Click(object sender, EventArgs e)
        {

            ObjectQuery oq = new ObjectQuery();

            CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));

            string name = txtContractName.Text.Trim();
            string startCode = txtStartCode.Text.Trim();
            string endCode = txtEndCode.Text.Trim();
            DateTime startTime = dtStartDate.Value;
            DateTime endTime = dtEndDate.Value;
            string desc = txtContractDesc.Text.Trim();

            if (!string.IsNullOrEmpty(name))
                oq.AddCriterion(Expression.Like("ContractName", name, MatchMode.Anywhere));

            if (!string.IsNullOrEmpty(startCode))
                oq.AddCriterion(Expression.Ge("Code", startCode));

            if (!string.IsNullOrEmpty(endCode))
                oq.AddCriterion(Expression.Le("Code", endCode));

            oq.AddCriterion(Expression.Ge("CreateDate", startTime));
            oq.AddCriterion(Expression.Le("CreateDate", endTime.AddDays(1).AddSeconds(-1)));

            if (!string.IsNullOrEmpty(desc))
                oq.AddCriterion(Expression.Like("ContractDesc", desc, MatchMode.Anywhere));

            if (cbContractTypeQuery.SelectedItem != null && !string.IsNullOrEmpty(cbContractTypeQuery.SelectedItem.ToString()))
                oq.AddCriterion(Expression.Eq("ContractGroupType", cbContractTypeQuery.SelectedItem.ToString()));
            else if(this._arrContractGroupType!=null && this._arrContractGroupType.Length>0)//xl 20160912
            {
                oq.AddCriterion(Expression.In("ContractGroupType", this._arrContractGroupType));
            }
               

            if (txtHandlePerson.Text != "" && txtHandlePerson.Result != null && txtHandlePerson.Result.Count > 0)
            {
                oq.AddCriterion(Expression.Eq("CreatePersonGUID", (txtHandlePerson.Result[0] as PersonInfo).Id));
            }
            //oq.AddCriterion(Expression.Eq("CreatePersonGUID", txtHandlePerson.Tag));

            oq.AddCriterion(Expression.Eq("State", ContractGroupState.发布));
            oq.AddOrder(NHibernate.Criterion.Order.Desc("ContractVersion"));
            oq.AddOrder(NHibernate.Criterion.Order.Asc("CreateDate"));
            oq.AddFetchMode("ChangeContract", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof(ContractGroup), oq);

            gridContractGroup.Rows.Clear();
            foreach (ContractGroup cg in list)
            {
                int index = gridContractGroup.Rows.Add();
                DataGridViewRow row = gridContractGroup.Rows[index];
                row.Cells[ContractName.Name].Value = cg.ContractName;
                row.Cells[ContractCode.Name].Value = cg.Code;
                row.Cells[ContractBusinessCode.Name].Value = cg.ContractNumber;
                if (cg.ChangeContract != null)
                    row.Cells[ChangeContract.Name].Value = cg.ChangeContract.Code;
                row.Cells[ContractVersion.Name].Value = cg.ContractVersion;
                row.Cells[ContractType.Name].Value = cg.ContractGroupType;
                row.Cells[ContractState.Name].Value = cg.State.ToString();
                row.Cells[ContractCreatePerson.Name].Value = cg.CreatePersonName;
                row.Cells[ContractCreateDate.Name].Value = cg.CreateDate.ToString();
                row.Cells[ContractDesc.Name].Value = cg.ContractDesc;
                row.Cells[colBearRange.Name].Value = cg.BearRange;
                row.Cells[colSettleType.Name].Value = cg.SettleType;
                row.Cells[colSingDate.Name].Value = cg.SingDate;
                row.Tag = cg;

                row.Selected = false;
            }
            gridContractGroup.ClearSelection();

            gridContractGroupDetail.Rows.Clear();
        }

    }
}
