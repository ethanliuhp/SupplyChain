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
using Application.Business.Erp.SupplyChain.CostManagement.ConstructionDataMng.Domain;
using System.Collections;
using VirtualMachine.Core;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ConstructionDataMng
{
    public partial class VConstructionSelect : TBasicDataView
    {
        private MConstructionData model = new MConstructionData();
        private ConstructionData curBillMaster;
        /// <summary>
        /// 当前单据
        /// </summary>
        public ConstructionData CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
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


        public VConstructionSelect()
        {
            InitializeComponent();
            InitEvent();
            ModelToView();
        }

       
        private void InitEvent()
        {
            btnOK.Click +=new EventHandler(btnOK_Click);
            btnCancel.Click +=new EventHandler(btnCancel_Click);
        }

        void btnOK_Click(object sender,EventArgs e)
        {
            DataGridViewRow var = dgDetail.CurrentRow;
            if (var.IsNewRow)
            {
                MessageBox.Show("请选择施工专业基础信息");
                return;
            }
            result.Add(this.dgDetail.CurrentRow.Tag);
            curBillMaster.InspectionContent = ClientUtil.ToString(var.Cells[colInspectionContent.Name].Value);
            curBillMaster.Specail = ClientUtil.ToString(var.Cells[colInspectionSpecail.Name].Value);
            curBillMaster.InspectionType = ClientUtil.ToString(var.Cells[colInspectionType.Name].Value);
            curBillMaster.SerailNum = ClientUtil.ToInt(var.Cells[colNum.Name].Value);
            this.Close();

        }
        void btnCancel_Click(object sender,EventArgs e)
        {
            this.Close();
        }

     
        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <returns></returns>
        private bool ModelToView()
        {
            try
            {
                curBillMaster = new ConstructionData();
                ObjectQuery oq = new ObjectQuery();
                IList lists = model.ConstructionDataSrv.GetConstructionData(oq);
                if (lists.Count <= 0 || lists == null)
                {
                    dgDetail.Rows.Clear();
                    return false;
                }
                dgDetail.Rows.Clear();
                foreach (ConstructionData var in lists)
                {
                    int i = this.dgDetail.Rows.Add();
                    this.dgDetail.Rows[i].Tag = var;
                    this.dgDetail[colInspectionContent.Name, i].Value = var.InspectionContent;//检查内容
                    this.dgDetail[colInspectionSpecail.Name, i].Value = var.Specail;//专业
                    this.dgDetail[colInspectionType.Name, i].Value = var.InspectionType;//检查类型
                    this.dgDetail[colNum.Name, i].Value = var.SerailNum;//序号
                }

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

      
    }
}
