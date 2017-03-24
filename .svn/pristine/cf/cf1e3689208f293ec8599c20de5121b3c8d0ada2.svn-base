using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng;

namespace Application.Business.Erp.SupplyChain.Client.StockMng.StockQuery
{
    public partial class VStockSporadicQuery : TBasicDataView
    {
        private MStockMng model = new MStockMng();
        MProjectDepartment proModel = new MProjectDepartment();
        private string projectID = "";

        public VStockSporadicQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        private void InitData()
        {
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);

            this.dtpDateBeginMv.Value = ConstObject.TheLogin.LoginDate.AddDays(-30);
            this.dtpDateEndMv.Value = ConstObject.TheLogin.LoginDate;

            CurrentProjectInfo ProjectInfo = StaticMethod.GetProjectInfo();
            if (ProjectInfo != null && ProjectInfo.Code != CommonUtil.CompanyProjectCode)
            {
                this.txtProject.Text = ProjectInfo.Name;
                this.txtProject.Tag = ProjectInfo.Id;
                this.lblPSelect.Visible = false;
                this.txtOperationOrg.Visible = false;
                this.btnOperationOrg.Visible = false;
            }
            projectID = ProjectInfo.Id;
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
        }

        void btnOperationOrg_Click(object sender, EventArgs e)
        {
            string opgId = "";
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect();
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                OperationOrgInfo info = frm.Result[0] as OperationOrgInfo;
                txtOperationOrg.Tag = info;
                txtOperationOrg.Text = info.Name;
                opgId = info.Id;
            }
            DataDomain domain = proModel.CurrentSrv.GetProjectInfoByOpgId(opgId);
            if (TransUtil.ToString(domain.Name1) != "")
            {
                this.txtProject.Text = TransUtil.ToString(domain.Name2);
                this.txtProject.Tag = TransUtil.ToString(domain.Name1);
                projectID = TransUtil.ToString(domain.Name1);
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            IList list = model.StockOutSrv.GetSporadicMoney(projectID, dtpDateBeginMv.Value.Date, dtpDateEndMv.Value.Date);
            this.dgDetail.Rows.Clear();
            decimal totalMoney = ClientUtil.ToDecimal(list[0]);
            decimal sporadicMoney = ClientUtil.ToDecimal(list[1]);
            decimal netMoney = ClientUtil.ToDecimal(list[2]);
            decimal elecMoney = ClientUtil.ToDecimal(list[3]);
            decimal sporadicRate = 0;
            decimal netRate = 0;
            decimal elecRate = 0;
            if (totalMoney != 0)
            {
                sporadicRate = Decimal.Round(sporadicMoney * 100 / totalMoney, 2);
                netRate = Decimal.Round(netMoney * 100 / totalMoney, 2);
                elecRate = Decimal.Round(elecMoney * 100 / totalMoney, 2);
            }

            int rowIndex = this.dgDetail.Rows.Add();
            dgDetail[this.colTotalMoney.Name, rowIndex].Value = totalMoney;
            dgDetail[this.colSporadicMoney.Name, rowIndex].Value = sporadicMoney;
            dgDetail[this.colNetMoney.Name, rowIndex].Value = netMoney;
            dgDetail[this.colElectMaterial.Name, rowIndex].Value = elecMoney;
            dgDetail[this.colSporadicRate.Name, rowIndex].Value = sporadicRate;
            dgDetail[this.colNetRate.Name, rowIndex].Value = netRate;
            dgDetail[this.colElecRate.Name, rowIndex].Value = elecRate;

            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            MessageBox.Show("查询完毕！");
        }
    }
}
