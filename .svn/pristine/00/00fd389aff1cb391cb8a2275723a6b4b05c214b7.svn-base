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
using Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain;
using System.Collections;
using NHibernate.Criterion;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireReturn.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireReturn
{
    public partial class VMaterialHireReturnGGLessOneQuery : TBasicDataView
    {
        private MMaterialHireMng model = new MMaterialHireMng();
        public VMaterialHireReturnGGLessOneQuery()
        {
            InitializeComponent();
            InitData();
            InitEvent();
        }
        private void InitData()
        {

            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
        
        }
        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnForward.Click+=new EventHandler(btnForward_Click);
           // this.txtContract.tbKeyPress += new KeyPressEventHandler(txtContract_tbKeyPress);
            this.txtContract.tbKeyDown += new KeyEventHandler(txtContract_tbKeyDown);
        }
        void txtContract_tbKeyDown(object sender, KeyEventArgs e)
        {
            int i = 0;
            string [] arrKey={"Delete","Back"};
            if (arrKey.Contains(e.KeyCode.ToString()))
            {
                txtSupply.Text = "";
                txtSupply.Tag = null;
                txtContract.Tag = null;
                txtContract.Text = "";
                txtProjectName.Text = "";
                txtProjectName.Tag = null;
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        //void txtContract_tbKeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == '\b')
        //    {
        //        txtSupply.Text = "";
        //        txtSupply.Tag = null;
        //        txtContract.Tag = null;
        //        txtContract.Text = "";
        //        txtProjectName.Text = "";
        //        txtProjectName.Tag = null;
        //        e.Handled = false;
        //    }
        //    else
        //    {
        //        e.Handled = true;
        //    }
        //}
        void btnForward_Click(object sender, EventArgs e)
        {
            VMaterialHireOrderSelector oVMaterialHireOrderSelector = new VMaterialHireOrderSelector(EnumMatHireType.其他);
            oVMaterialHireOrderSelector.ShowDialog();
            if (oVMaterialHireOrderSelector.Result != null && oVMaterialHireOrderSelector.Result.Count > 0)
            {
                MatHireOrderMaster OrderMaster = oVMaterialHireOrderSelector.Result[0] as MatHireOrderMaster;
                #region 数据填充
                txtSupply.Text = OrderMaster.SupplierName;
                txtSupply.Tag = OrderMaster.TheSupplierRelationInfo;
                txtContract.Tag = OrderMaster;
                txtContract.Text = OrderMaster.Code;
                txtProjectName.Text = OrderMaster.ProjectName;
                txtProjectName.Tag = OrderMaster.ProjectId;
                #endregion
            }
        }
        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                dgMaster.Rows.Clear();
                txtSumLessOneQty.Text = string.Empty;
                ObjectQuery oq = new ObjectQuery();
                //CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                //oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                oq.AddCriterion(Expression.Eq("MatHireType", EnumMatHireType.钢管));
                oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value.Date));
                oq.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));
                if (this.txtCreatePersonBill.Text != "")
                {
                    oq.AddCriterion(Expression.Eq("CreatePersonName", this.txtCreatePersonBill.Text));
                }
                if (!string.IsNullOrEmpty(txtOldCodeBill.Text))
                {
                    oq.AddCriterion(Expression.Like("BillCode", txtOldCodeBill.Text, MatchMode.Anywhere));
                }
                if (txtContract.Tag != null)
                {
                    oq.AddCriterion(Expression.Eq("Contract.Id", (txtContract.Tag as MatHireOrderMaster).Id));
                }
                // oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                IList list = model.MaterialHireMngSvr.ObjectQuery(typeof(MatHireReturnMaster), oq);
               

                ShowMasterList(list);
            }
            catch (Exception ex)
            {

            }
        }
        void ShowMasterList(IList list)
        {
            if (list == null || list.Count == 0) return;
            foreach (MatHireReturnMaster master in list)
            {
                int i = this.dgMaster.Rows.Add();
                this.dgMaster[colCodeMaster.Name, i].Value = master.Code;
                this.dgMaster[colStateMaster.Name, i].Value = ClientUtil.GetDocStateName(master.DocState);
                this.dgMaster[colCreateDateMaster.Name, i].Value = master.CreateDate.ToShortDateString();
                this.dgMaster[colBusinessDateMaster.Name, i].Value = master.RealOperationDate.ToShortDateString();
                this.dgMaster[colSupplyInfoMaster.Name, i].Tag = master.TheSupplierRelationInfo;
                this.dgMaster[colSupplyInfoMaster.Name, i].Value = master.SupplierName;
                this.dgMaster[colOriContractNoMaster.Name, i].Value = master.OldContractNum;
                this.dgMaster[colPrintTimesMaster.Name, i].Value = master.PrintTimes;
                this.dgMaster[colCreatePersonMaster.Name, i].Value = master.CreatePersonName;
                this.dgMaster[colDescriptMaster.Name, i].Value = master.Descript;
                this.dgMaster[colBalRuleMaster.Name, i].Value = master.BalRule;
                //this.dgMaster[colTransportChargeMaster.Name, i].Value = master.TransportCharge;
                this.dgMaster[this.colProjectNameMaster.Name, i].Value = master.ProjectName;
                this.dgMaster[this.colMasterQtMoney.Name, i].Value = master.SumExtMoney;
                this.dgMaster[this.colBillCodeMaster.Name, i].Value = master.BillCode;
                this.dgMaster[this.colLessOneQty.Name, i].Value = master.LessOneQuanity;
                this.dgMaster.Rows[i].Tag = master;
            }
            this.dgMaster.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            this.txtSumLessOneQty.Text = list.OfType<MatHireReturnMaster>().Sum(a => a.LessOneQuanity).ToString("N2");
        }

        
    }
}
