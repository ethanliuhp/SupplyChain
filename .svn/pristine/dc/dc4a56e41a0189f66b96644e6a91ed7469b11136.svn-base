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
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Client.MaterialRentalMange;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalSettlementMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalManage.MaterialRentalSettlementMng
{
    public partial class VMaterialSubjectSelector : TBasicDataView
    {
        MMatRentalMng model = new MMatRentalMng();
        private int totalRecords = 0;
        private MaterialRentalSettlementDetail curBillDetail;
        /// <summary>
        /// 当前明细单据
        /// </summary>
        public MaterialRentalSettlementDetail CurBillDetail
        {
            get { return curBillDetail; }
            set { curBillDetail = value; }
        }

        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VMaterialSubjectSelector()
        {
            InitializeComponent();
            InitEvent();
            InitForm();
            InitData();
        }

        public void InitData()
        {
            //给费用添加下拉列表框的信息InitCostType
            //((DataGridViewComboBoxColumn)colCostName).Items.AddRange(Enum.GetNames(typeof(MaterialResource)));
            VBasicDataOptr.InitMaterialRental(colCostName, false);
        }

        private void InitForm()
        {
            this.Title = "结算分科目费用明细";
        }

        private void InitEvent()
        {
            btnOK.Click += new EventHandler(btnOK_Click);
            Load += new EventHandler(VMaterialSubjectSelector_Load);
        }

        void VMaterialSubjectSelector_Load(object sender, EventArgs e)
        {
            foreach (MaterialSubjectDetail obj in Result)
            {
                int rowIndex = dgDetail.Rows.Add();
                dgDetail.Rows[rowIndex].Tag = obj;
                dgDetail[colCostName.Name, rowIndex].Value = obj.CostName;//成本名称
                dgDetail[colDescription.Name, rowIndex].Value = obj.Descript;//备注
                dgDetail[colDateUnit.Name, rowIndex].Tag = obj.DateUnit;
                dgDetail[colDateUnit.Name, rowIndex].Value = obj.DateUnitName;//时间单位
                dgDetail[colMoney.Name, rowIndex].Value = obj.SettleMoney;//结算合价
                dgDetail[colPriceUnit.Name, rowIndex].Tag = obj.PriceUnit;
                dgDetail[colPriceUnit.Name, rowIndex].Value = obj.PriceUnitName;//价格单位
                dgDetail[colQuantity.Name, rowIndex].Value = obj.SettleQuantity;//数量
                dgDetail[colQuantityUnit.Name, rowIndex].Tag = obj.QuantityUnit;
                dgDetail[colQuantityUnit.Name, rowIndex].Value = obj.QuantityUnitName;//数量单位
                dgDetail[colRentalDate.Name, rowIndex].Value = obj.SettleDate;//租赁时间
                dgDetail[colRentalPrice.Name, rowIndex].Value = obj.SettlePrice;//结算单价
                dgDetail[colSettleSubject.Name, rowIndex].Value = obj.SettleSubjectName;//结算科目
                dgDetail[colResource.Name, rowIndex].Value = obj.MaterialTypeName;
            }
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }
    }
}
