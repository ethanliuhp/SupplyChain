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
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalLedgerMng
{
    public partial class VMaterialRentalLedgerQuery1 : TBasicDataView
    {
        MMatRentalMng model = new MMatRentalMng();
        public VMaterialRentalLedgerQuery1()
        {
            InitializeComponent();
            this.InitData();
            this.InitEvent();
        }

        private void InitData()
        {
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-30);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            this.txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode1;
            txtUsedBank.SupplierCatCode = CommonUtil.SupplierCatCode3;
            this.txtMaterial.materialCatCode = CommonUtil.TurnMaterialMatCode;
        }
        private void InitEvent()
        {
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
        }


        void btnSearch_Click(object sender, EventArgs e)
        {
            dgDetail.Rows.Clear();
            #region 定义查询条件
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            string condition = " and t1.ProjectId = '" + projectInfo.Id + "'";

            if (StaticMethod.IsUseSQLServer())
            {
                condition += " and  t1.RealOperationDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and  t1.RealOperationDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
            }
            else
            {
                condition += " and  t1.RealOperationDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and  t1.RealOperationDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
            }

            if (this.txtSupplier.Text != "" && this.txtSupplier.Result != null && this.txtSupplier.Result.Count != 0)
            {
                condition = condition + " and  t1.SupplierRelation='" + (this.txtSupplier.Result[0] as SupplierRelationInfo).Id.ToString() + "'";
            }

            if (this.txtUsedBank.Text != "" && this.txtUsedBank.Result != null && this.txtUsedBank.Result.Count != 0)
            {
                condition = condition + " and  t1.TheRank='" + (this.txtUsedBank.Result[0] as SupplierRelationInfo).Id.ToString() + "'";
            }

            if (txtOriContractNo.Text != "")
            {
                condition += " and  t1.OldContractNum like '" + txtOriContractNo.Text + "%'";
            }

            if (this.txtMaterial.Text != "")
            {
                condition = condition + " and t1.MaterialName like '%" + this.txtMaterial.Text + "%'";
            }
            if (this.txtSpec.Text != "")
            {
                condition = condition + " and t1.MaterialSpec like '%" + this.txtSpec.Text + "%'";
            }
            if (rbBal.Checked == true)
            {
                condition = condition + "and (t3.BalState=1 OR t4.BalState=1)";
            }
            if (rbNoBal.Checked == true)
            {
                condition = condition + "and (t3.BalState=0 OR t4.BalState=0)";
            }

            #endregion

            IList list = model.MatMngSrv.GetMaterialRentalLedger(condition, dtpDateBegin.Value.Date, projectInfo.Id);

            foreach (DataDomain domain in list)
            {
                int index = this.dgDetail.Rows.Add();
                dgDetail[colType.Name, index].Value = domain.Name1;
                string balState = "";
                if (domain.Name1 == "收料")
                {
                    balState = ClientUtil.ToString(domain.Name23);
                    if (balState != "")
                    {
                        if (balState == "0")
                        {
                            dgDetail[colBalState.Name, index].Value = "未结算";
                        }
                        if (balState == "1")
                        {
                            dgDetail[colBalState.Name, index].Value = "已结算";
                        }
                    }
                }
                if (domain.Name1 == "退料")
                {
                    balState = ClientUtil.ToString(domain.Name24);
                    if (balState != "")
                    {
                        if (balState == "0")
                        {
                            dgDetail[colBalState.Name, index].Value = "未结算";
                        }
                        if (balState == "1")
                        {
                            dgDetail[colBalState.Name, index].Value = "已结算";
                        }
                    }
                }
                dgDetail[colOriContractNo.Name, index].Value = domain.Name2;
                dgDetail[colCode.Name, index].Value = domain.Name3;
                dgDetail[colMaterialCode.Name, index].Value = domain.Name5;
                dgDetail[colMaterialName.Name, index].Value = domain.Name6;
                dgDetail[colSpec.Name, index].Value = domain.Name7;
                dgDetail[colCollQuantity.Name, index].Value = domain.Name8;
                dgDetail[colLeftQuantity.Name, index].Value = domain.Name9;
                dgDetail[colReturnQty.Name, index].Value = domain.Name10;
                dgDetail[colSupplyInfo.Name, index].Value = domain.Name11;
                dgDetail[colUnit.Name, index].Value = domain.Name12;
                dgDetail[colPrice.Name, index].Value = domain.Name15;
                dgDetail[colRankName.Name, index].Value = domain.Name16;
                dgDetail[colBroachQty.Name, index].Value = domain.Name17;
                dgDetail[colRejectQty.Name, index].Value = domain.Name18;
                dgDetail[colConsumeQty.Name, index].Value = domain.Name19;
                dgDetail[colDamageQty.Name, index].Value = domain.Name20;
                dgDetail[colDiscardQty.Name, index].Value = domain.Name21;
                dgDetail[colRepairQty.Name, index].Value = domain.Name22;
                if (domain.Name14 != null)
                    dgDetail[colOperDate.Name, index].Value = ClientUtil.ToDateTime(domain.Name14).ToString("yyyy-MM-dd");
                dgDetail[colSystemDate.Name, index].Value = domain.Name13;
            }
            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            MessageBox.Show("查询完毕！");
        }


        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }
    }
}
