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
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery
{
    public partial class VContractExcute : TBasicToolBarByMobile
    {
        MContractExcuteMng model = new MContractExcuteMng();
        private AutomaticSize automaticSize = new AutomaticSize();
        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VContractExcute()
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            InitEvent();
            InitForm();
        }

        private void InitForm()
        {
            dtpDateBegin.Value = ConstObject.LoginDate.AddMonths(-1);
            dtpDateEnd.Value = ConstObject.LoginDate;

            comConstractType.Text = "劳务分包";

            ((ComboBox)comConstractType).Items.AddRange(Enum.GetNames(typeof(SubContractType)));
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode3;
        }

        private void InitEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

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
            oq.AddCriterion(Expression.Ge("CreateTime", dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateTime", dtpDateEnd.Value.AddDays(1).Date));
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
                dgMaster[colContractType.Name, rowIndex].Value = master.ContractType;
                dgMaster[colTearTeam.Name, rowIndex].Value = master.BearerOrgName;
            }
            this.lblRecordTotal.Text = "共【" + masterList.Count + "】条记录";
        }

        private void VContractExcute_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
    }
}
