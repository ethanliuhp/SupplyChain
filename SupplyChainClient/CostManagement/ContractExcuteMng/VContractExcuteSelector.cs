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
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng
{
    public partial class VContractExcuteSelector : TBasicDataView
    {
        MContractExcuteMng model = new MContractExcuteMng();
        private int totalRecords = 0;
        private decimal sumQuantity = 0;

        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VContractExcuteSelector()
        {
            InitializeComponent();
            InitEvent();
            InitForm();
        }

        private void InitForm()
        {
            //dtpDateBegin.Value = ConstObject.LoginDate.AddMonths(-1);
            //dtpDateEnd.Value = ConstObject.LoginDate;

            ((ComboBox)comConstractType).Items.AddRange(Enum.GetNames(typeof(SubContractType)));
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode3;
        }

        private void InitEvent()
        {
            dgMaster.CellDoubleClick += new DataGridViewCellEventHandler(dgMaster_CellDoubleClick);

            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

        }

        void dgMaster_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
                btnOK_Click(btnOK, new EventArgs());
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.result.Clear();
            if (this.dgMaster.SelectedRows.Count == 0)
            {
                MessageBox.Show("没有可以引用的分包项目！");
                return;
            }
            DataGridViewRow theCurrentRow = this.dgMaster.CurrentRow;
            result.Add(this.dgMaster.CurrentRow.Tag);
            this.btnOK.FindForm().Close();
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            IList list = new ArrayList();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            //oq.AddCriterion(Expression.Eq("TheContractGroup.State", ContractGroupState.发布));
            oq.AddCriterion(Expression.Or(Expression.Eq("TheContractGroup.State", ContractGroupState.制定), Expression.Eq("TheContractGroup.State", ContractGroupState.发布)));
            //oq.AddCriterion(Expression.Ge("CreateTime", dtpDateBegin.Value.Date));
            //oq.AddCriterion(Expression.Lt("CreateTime", dtpDateEnd.Value.AddDays(1).Date));
            if (txtSupply.Text != "" && txtSupply.Result.Count > 0)
            {
                oq.AddCriterion(Expression.Eq("BearerOrgName", txtSupply.Text));
            }
            if (comConstractType.SelectedItem != null)
            {
                oq.AddCriterion(Expression.Eq("ContractType", EnumUtil<SubContractType>.FromDescription(comConstractType.SelectedItem)));
            }

            try
            {
                list = model.ContractExcuteSrv.GetContractExcute(oq);
                ShowMasterList(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
        }

        /// <summary>
        /// 显示主表
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList masterList)
        {
            dgMaster.Rows.Clear();
            if (masterList == null || masterList.Count == 0) return;
            foreach (SubContractProject master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCode.Name, rowIndex].Value = master.Code;
                dgMaster[colConstructRace.Name, rowIndex].Value = master.ManagementRate;
                dgMaster[colContractType.Name, rowIndex].Value = master.ContractType;
                dgMaster[colCreateDate.Name, rowIndex].Value = master.CreateDate.ToShortDateString();
                dgMaster[colRace.Name, rowIndex].Value = master.AllowExceedPercent;
                dgMaster[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);
                dgMaster[colSumMoney.Name, rowIndex].Value = master.ContractInterimMoney;
                dgMaster[colTearTeam.Name, rowIndex].Value = master.BearerOrgName;
                dgMaster[colUtilitiesRace.Name, rowIndex].Value = master.UtilitiesRate;
                dgMaster[colCreatePerson.Name, rowIndex].Value = master.CreatePersonName;
                dgMaster[colLaborMoneyType.Name, rowIndex].Value = master.LaborMoneyType;
                dgMaster[colLaborRace.Name, rowIndex].Value = master.LaobrRace;
                if (master.TheContractGroup != null)
                {
                    dgMaster[colConstractName.Name, rowIndex].Value = master.TheContractGroup.ContractName;
                    dgMaster[colMainMoney.Name, rowIndex].Value = master.ContractInterimMoney;//暂定金额
                    dgMaster[colRange.Name, rowIndex].Value = master.TheContractGroup.BearRange;
                    dgMaster[colSettleWay.Name, rowIndex].Value = master.TheContractGroup.SettleType;
                }
            }
            this.lblRecordTotal.Text = "共【" + masterList.Count + "】条记录";
        }

    }
}
