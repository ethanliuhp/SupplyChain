using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.FinanceMng;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using NHibernate.Criterion;
using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using NHibernate;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost
{
    public partial class VBorrowedOrderQuery : TBasicDataView
    {
        private MFinanceMultData fModel = new MFinanceMultData();
        private MIndirectCost model = new MIndirectCost();
        private EnumBorrowedOrder _ExecType;
        private CurrentProjectInfo projectInfo = null;
        IList opgList = new ArrayList();

        public VBorrowedOrderQuery(EnumBorrowedOrder execType)
        {
            InitializeComponent();
            this._ExecType = execType;
            InitEvent();
            InitData();
        }

        private void InitData()
        {
            this.dtpDateBegin.Value = new DateTime(ConstObject.TheLogin.LoginDate.Year, 1, 1);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
            {
                txtOrgName.Text = projectInfo.Name;
                txtOrgName.Tag = projectInfo;
                this.btnSelectOrgName.Visible = false;
            }
            //opgList = fModel.FinanceMultDataSrv.QuerySubAndCompanyOrgInfo();//分公司和总部信息
        }
        private void InitEvent()
        {
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_DoubleClick);

            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);

            this.btnSelectOrgName.Click += new EventHandler(btnSelectOrgName_Click);
        }
        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }

        void btnSelectOrgName_Click(object sender, EventArgs e)
        {
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect(true);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                OperationOrgInfo info = frm.Result[0] as OperationOrgInfo;
                txtOrgName.Tag = info;
                txtOrgName.Text = info.Name;
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        /// <summary>
        /// 明细表显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearch_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("正在查询信息......");

            ObjectQuery objectQuery = new ObjectQuery();
            IList list = new ArrayList();
            //单据
            if (txtCodeBegin.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("Master.Code", txtCodeBegin.Text, MatchMode.Anywhere));
            }
            if (txtCreatePerson.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("Master.CreatePersonName", txtCreatePerson.Text, MatchMode.Anywhere));
            }
            objectQuery.AddCriterion(Expression.Ge("BorrowedDate", this.dtpDateBegin.Value.Date));
            objectQuery.AddCriterion(Expression.Lt("BorrowedDate", this.dtpDateEnd.Value.AddDays(1).Date));
            if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
            {
                objectQuery.AddCriterion(Expression.Eq("Master.ProjectId", projectInfo.Id));
            }
            else if (txtOrgName.Tag != null)
            {
                OperationOrgInfo orgInfo = this.txtOrgName.Tag as OperationOrgInfo;
                objectQuery.AddCriterion(Expression.Like("Master.OpgSysCode", orgInfo.SysCode, MatchMode.Start));
            }

            if (btnSubmit.Checked)
            {
                objectQuery.AddCriterion(Expression.Sql("state = 5 "));
            }
            if (this.btnNoSubmit.Checked)
            {
                objectQuery.AddCriterion(Expression.Sql("state != 5 "));
            }
            
            string sBorrowedType = cmbBorrowedType.SelectedItem == null ? "" : cmbBorrowedType.SelectedItem.ToString();
            if (!string.IsNullOrEmpty(sBorrowedType))
            {
                objectQuery.AddCriterion(Expression.Eq("BorrowedType", sBorrowedType));
            }
            try
            {
                objectQuery.AddFetchMode("Master", FetchMode.Eager);
                list = model.IndirectCostSvr.Query(typeof(BorrowedOrderDetail), objectQuery);

                dgDetail.Rows.Clear();
                ShowDetailList(list);
                FlashScreen.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        public void ShowDetailList(IList lstDetail)
        {
            int iRowIndex = -1;
            this.dgDetail.Rows.Clear();
            if (lstDetail == null || lstDetail.Count == 0) return;
            foreach (BorrowedOrderDetail oDetail in lstDetail)
            {
                iRowIndex = this.dgDetail.Rows.Add();
                this.dgDetail[this.colCode.Name, iRowIndex].Value = oDetail.Master.Code;
                this.dgDetail[this.colCreatePersonName.Name, iRowIndex].Value = oDetail.Master.CreatePersonName;
                this.dgDetail[this.colRealOperateDate.Name, iRowIndex].Value = oDetail.Master.RealOperationDate.ToString("yyyy-MM-dd");
                this.dgDetail[this.colOrgName.Name, iRowIndex].Value = oDetail.Master.OperOrgInfoName;
                this.dgDetail[this.colBorrowData.Name, iRowIndex].Value = oDetail.BorrowedDate.ToString("yyyy-MM-dd");
                this.dgDetail[this.colBorrowedPurpose.Name, iRowIndex].Value = oDetail.BorrowedPurpose;
                this.dgDetail[this.colBorrowedType.Name, iRowIndex].Value = oDetail.BorrowedType;
                this.dgDetail[this.colMoney.Name, iRowIndex].Value = oDetail.Money;
                this.dgDetail[this.colDescript.Name, iRowIndex].Value = oDetail.Descript;
                this.dgDetail.Rows[iRowIndex].Tag = oDetail;
            }
            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }
        public void dgDetail_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.RowIndex < this.dgDetail.Rows.Count && this.dgDetail.CurrentRow.Tag != null)
            {
                BorrowedOrderDetail oDetail = this.dgDetail.CurrentRow.Tag as BorrowedOrderDetail;
                if (oDetail != null)
                {
                    VBorrowedOrder oVBorrowedOrder = new VBorrowedOrder();
                    oVBorrowedOrder.StartPosition = FormStartPosition.CenterParent;
                    oVBorrowedOrder.Start(oDetail.Master.Id);
                    oVBorrowedOrder.ShowDialog();
                }
            }
        }

    }
}
