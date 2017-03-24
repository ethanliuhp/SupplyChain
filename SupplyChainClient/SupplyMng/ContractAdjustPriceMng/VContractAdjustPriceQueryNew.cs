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
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.SupplyManage.ContractAdjustPriceManage.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.ContractAdjustPriceMng
{
    public partial class VContractAdjustPriceQueryNew : TBasicDataView
    {
        private MContractAdjustPriceMng model = new MContractAdjustPriceMng();

        public VContractAdjustPriceQueryNew()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        private void InitData()
        {
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode;
            txtSupplier.Enabled = true;
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }


        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        void btnSearch_Click(object sender, EventArgs e)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));

            if (!string.IsNullOrEmpty(txtCodeBegin.Text))
            {
                if (txtCodeBegin.Text == txtCodeEnd.Text)
                {
                    objectQuery.AddCriterion(Expression.Like("Code", string.Format("%{0}%",txtCodeBegin.Text)));
                }
                else
                {
                    objectQuery.AddCriterion(Expression.Between("Code", txtCodeBegin.Text, txtCodeEnd.Text));
                }
            }

            objectQuery.AddCriterion(Expression.Between("CreateDate", dtpDateBegin.Value.Date, dtpDateEnd.Value.Date));

            if (!string.IsNullOrEmpty(txtCreatePerson.Text) && txtCreatePerson.Result != null && txtCreatePerson.Result.Count > 0)
            {
                objectQuery.AddCriterion(Expression.Eq("CreatePerson", txtCreatePerson.Result[0] as PersonInfo));
            }

            if (this.txtSupplier.Text != "" && this.txtSupplier.Result != null && this.txtSupplier.Result.Count != 0)
            {
                objectQuery.AddCriterion(Expression.Eq("SupplierName", txtSupplier.Text));
            }

            if (!string.IsNullOrEmpty(this.txtSupplyNum.Text.Trim()))
            {
                objectQuery.AddCriterion(Expression.Eq("ContractNum", txtSupplyNum.Text));
            }

            if (!string.IsNullOrEmpty(txtMaterial.Text.Trim()))
            {
                objectQuery.AddCriterion(Expression.Eq("MaterialName", txtMaterial.Text));
            }

            if (!string.IsNullOrEmpty(this.txtMaterialSpec.Text.Trim()))
            {
                objectQuery.AddCriterion(Expression.Eq("MaterialSpec", txtMaterialSpec.Text));
            }

            objectQuery.AddFetchMode("CreatePerson", NHibernate.FetchMode.Eager);

            var adjustPriceList = model.ContractAdjustPriceSrv.GetContractAdjustPrice(objectQuery);
            BindData(adjustPriceList);
        }

        private void BindData(IList adjustPriceList)
        {
            dgExtDetail.Rows.Clear();
            foreach (var item in adjustPriceList)
            {
                var obj = item as ContractAdjustPrice;

                int rowIndex = this.dgExtDetail.Rows.Add();
                dgExtDetail[colCGContractDate.Name, rowIndex].Value = obj.AvailabilityDate;//调价日期
                dgExtDetail[colCGCode.Name, rowIndex].Value = obj.Code;//单号
                dgExtDetail[colCGContractNo.Name, rowIndex].Value = obj.SupplyOrderCode;//采购合同号
                //dgExtDetail[colCGContractPrice.Name, rowIndex].Value = obj.SupplyOrderDetail.Price;
                //dgExtDetail[colCGQuantity.Name, rowIndex].Value = obj.SupplyOrderDetail.Quantity;
                dgExtDetail[colCGContractPerson.Name, rowIndex].Value = obj.CreatePerson.Name;
                dgExtDetail[colCGSupply.Name, rowIndex].Value = obj.SupplierName;
                dgExtDetail[colCGMaterialName.Name, rowIndex].Value = obj.MaterialName;//物资名称
                dgExtDetail[colCGMaterialSpec.Name, rowIndex].Value = obj.MaterialSpec;//规格型号
                dgExtDetail[colCGMaterialCode.Name, rowIndex].Value = obj.MaterialCode;//物资编号
                dgExtDetail[colCGMaterialCode.Name, rowIndex].Tag = obj.MaterialResource;//物资
                dgExtDetail[colCGNewPrice.Name, rowIndex].Value = obj.ModifyPrice;//调后价格，新价格
                dgExtDetail[colCGOldPrice.Name, rowIndex].Value =obj.PrePrice;//调前价格
                dgExtDetail[colCGContractReason.Name, rowIndex].Value = obj.ModifyPriceReason;//调价原因
                dgExtDetail[colCGState.Name, rowIndex].Value = ClientUtil.GetDocStateName(obj.DocState);
            }

            this.dgExtDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }
    }
}