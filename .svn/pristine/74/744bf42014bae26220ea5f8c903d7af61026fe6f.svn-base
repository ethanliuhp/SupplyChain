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
using Application.Business.Erp.SupplyChain.CostManagement.OwnerQuantityMng.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.QWBS.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.OwnerQuantityMng
{
    public partial class VOwnerQuantityDisplayInfo : TBasicDataView
    {
        /// <summary>
        /// 业主保量状态对象
        /// </summary>
        public OwnerQuantity OwnerQny = null;

        private MOwnerQuantityMng model = new MOwnerQuantityMng();

        public VOwnerQuantityDisplayInfo()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        private void InitEvent()
        {
            this.btnExit.Click += new EventHandler(btnExit_Click);
            this.Load += new EventHandler(VOwnerQuantityDisplayInfo_Load);
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void VOwnerQuantityDisplayInfo_Load(object sender, EventArgs e)
        {
            if (OwnerQny != null)
            {
                int rowIndex = this.dgDetail.Rows.Add();
                DataGridViewRow row = dgDetail.Rows[rowIndex];

                row.Cells[colProjectName.Name].Value = OwnerQny.ProjectName;

                row.Cells[colQWBSName.Name].Value = OwnerQny.QWBSName;
                row.Cells[colQWBSName.Name].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(QWBSManage), OwnerQny.QWBSName, OwnerQny.QWBSSysCode);

                row.Cells[colSumContractMoney.Name].Value = OwnerQny.SumContractMoney;
                row.Cells[colRenderMoney.Name].Value = OwnerQny.SumSubmitMoney;

                row.Cells[colConfirmMoney.Name].Value = OwnerQny.SumConfirmMoney;
                row.Cells[colFactReceiveMoney.Name].Value = OwnerQny.RealCollectionMoney;
                row.Cells[colPriceUnit.Name].Value = OwnerQny.UnitPriceName;

                row.Cells[colUpdatetime.Name].Value = OwnerQny.LastUpdateDate.ToString();
            }
        }

        private void InitData()
        {

        }


    }
}
