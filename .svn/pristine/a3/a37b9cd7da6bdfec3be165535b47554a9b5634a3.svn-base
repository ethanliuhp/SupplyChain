using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass; 
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Service;
using Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData;
using System.Collections;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.Util;
using FlexCell;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FundScheme
{
    public partial class VFundSchemeQuery : TBasicDataView
    {
        private FundPlanningMaster selectFundScheme;
        private MFinanceMultData mOperate;
        private OperationOrgInfo subCompany;

        public VFundSchemeQuery()
        {
            InitializeComponent();

            InitEvents();

            InitData();

            ucFundSchemeDetail1.InitReport();

            ucProjectSelector1.InitData();
        }

        public void InitData()
        {
            this.dtpBeginCreateDate.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpEndCreateDate.Value = ConstObject.TheLogin.LoginDate;

            mOperate = new MFinanceMultData();
            subCompany = StaticMethod.GetSubCompanyOrgInfo();
        }

        private void InitEvents()
        {
            dgMaster.CellDoubleClick += new DataGridViewCellEventHandler(dgMaster_CellDoubleClick);
       }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("正在查询信息......");

            ObjectQuery objectQuery = new ObjectQuery();
            if (!string.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                objectQuery.AddCriterion(Expression.Like("Code", txtCode.Text.Trim(), MatchMode.Anywhere));
            }
            objectQuery.AddCriterion(Expression.Ge("CreateDate", dtpBeginCreateDate.Value.Date));
            objectQuery.AddCriterion(Expression.Le("CreateDate", dtpEndCreateDate.Value.Date.AddDays(1)));

            var proj = ucProjectSelector1.SelectedProject;
            if (proj != null)
            {
                objectQuery.AddCriterion(Expression.Like("OpgSysCode", proj.OwnerOrgSysCode, MatchMode.Start));
            }
            else if (subCompany != null)
            {
                objectQuery.AddCriterion(Expression.Like("OpgSysCode", subCompany.SysCode, MatchMode.Start));
            }
            objectQuery.AddOrder(new Order("Code", true));

            var fundMasterDataList = mOperate.FinanceMultDataSrv.Query(typeof(FundPlanningMaster), objectQuery)
                .OfType<FundPlanningMaster>().ToList();

            dgMaster.Rows.Clear();
            foreach (var oneDataMaster in fundMasterDataList)
            {
                var index = dgMaster.Rows.Add(1);
                dgMaster.Rows[index].Tag = oneDataMaster;
                dgMaster.Rows[index].Cells[0].Value = oneDataMaster.Code;
                dgMaster.Rows[index].Cells[1].Value = oneDataMaster.ProjectName;
                dgMaster.Rows[index].Cells[2].Value = oneDataMaster.SchemeBeginDate.ToShortDateString() + "-" + oneDataMaster.SchemeEndDate.ToShortDateString();
                dgMaster.Rows[index].Cells[3].Value = ClientUtil.GetDocStateName(oneDataMaster.DocState);
                dgMaster.Rows[index].Cells[4].Value = oneDataMaster.CreateDate.ToShortDateString();
                dgMaster.Rows[index].Cells[5].Value = oneDataMaster.CreatePersonName;
            }

            FlashScreen.Close();
        }

        private void dgMaster_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex > dgMaster.RowCount)
            {
                return;
            }

            selectFundScheme = dgMaster.Rows[e.RowIndex].Tag as FundPlanningMaster;
            if (selectFundScheme != null)
            {
                ucFundSchemeDetail1.LoadFundScheme(selectFundScheme);
            }
            else
            {
                MessageBox.Show("获取策划表信息失败");
            }
        }
    }
}
