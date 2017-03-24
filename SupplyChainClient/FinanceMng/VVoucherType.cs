using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Application.Resource.FinancialResource.RelateClass;
using VirtualMachine.Component.Util;
using C1.Win.C1FlexGrid;
using VirtualMachine.Core;

namespace Application.Business.Erp.SupplyChain.Client.FinanceMng
{
    public partial class VVoucherType : Form
    {
        private MVoucherType theMVoucherType = new MVoucherType();
        private VoucherTypeInfo theVoucherTypeInfo = new VoucherTypeInfo();
        private IList lst = new ArrayList();
        public VVoucherType()
        {
            InitializeComponent();
            this.InitEvent();
            this.InitData();
        }
        private void InitData()
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddOrder(new NHibernate.Criterion.Order("Code", true));

            lst = theMVoucherType.getVouType(objectQuery);

            foreach (VoucherTypeInfo var in lst)
            {
                Row row = this.grdVoucherType.Rows.Add();
                row["colTypeMark"] = var.TypeMark;
                row["colCode"] = var.Code;
                row["colTypeName"] = var.TypeName;
                row["colState"] = var.State == 1 ? true : false;
                row.UserData = var;
            }
            
            if (lst.Count > 0)
            {
                this.grdVoucherType.Rows[1].Selected = true;
            }
        }
        private void InitEvent()
        {
            this.lnkAdd.Click += new EventHandler(lnkAdd_Click);
            this.lnkEdit.Click += new EventHandler(lnkEdit_Click);
            this.lnkSearch.Click += new EventHandler(lnkSearch_Click);
            this.lnkDelete.Click += new EventHandler(lnkDelete_Click);
        }

        void lnkDelete_Click(object sender, EventArgs e)
        {
            if (this.grdVoucherType.Rows.Selected.Count == 0)
            {
                MessageBox.Show("请选择要删除的凭证类别！");
                return;
            }
            VoucherTypeInfo obj = this.grdVoucherType.Rows[this.grdVoucherType.Row].UserData as VoucherTypeInfo;
            DialogResult dr = MessageBox.Show("是否删除？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (dr == DialogResult.No)
                return;
            bool result = false;
            result = theMVoucherType.Delete(obj);
            this.grdVoucherType.RemoveItem(this.grdVoucherType.Row); 
        }

        void lnkSearch_Click(object sender, EventArgs e)
        {
            VoucherTypeInfo Search = new VoucherTypeInfo();
            VVoucherTypeSearch theVVoucherTypeSearch = new VVoucherTypeSearch();
            Search = theVVoucherTypeSearch.Show(Search) as VoucherTypeInfo;
        }

        void lnkEdit_Click(object sender, EventArgs e)
        {
            if (this.grdVoucherType.Rows.Selected.Count == 0)
            {
                MessageBox.Show("请选择要编辑的凭证类别");
                return;
            }
            VoucherTypeInfo EditDate = this.grdVoucherType.Rows[this.grdVoucherType.Row].UserData as VoucherTypeInfo;
            Row row = this.grdVoucherType.Rows[this.grdVoucherType.Row];
            VAddNewVoucherType theUpdate = new VAddNewVoucherType();
            lst = this.theMVoucherType.getobj();
            EditDate.TypeMark = ClientUtil.ToString(row["colTypeMark"]);
            EditDate.Code = ClientUtil.ToString(row["colCode"]);
            EditDate.TypeName = ClientUtil.ToString(row["colTypeName"]);
            EditDate.State = ClientUtil.ToInt(row["colState"]);
            EditDate = theUpdate.Show(EditDate) as VoucherTypeInfo;
            row.UserData = EditDate;
            if (theUpdate.IsSuccess)
            {
                row["colTypeName"] = EditDate.TypeName;
                row["colCode"] = EditDate.Code;
                row["colTypeMark"] = EditDate.TypeMark;
                row["colState"] = EditDate.State == 1 ? true : false;
                row.UserData = EditDate;
            } 
        }

        void lnkAdd_Click(object sender, EventArgs e)
        {
            VoucherTypeInfo theVoucherTypeInfo = new VoucherTypeInfo();
            VAddNewVoucherType theVAddNewVoucherType = new VAddNewVoucherType();
            theVoucherTypeInfo = theVAddNewVoucherType.Show(theVoucherTypeInfo) as VoucherTypeInfo;
            if (theVAddNewVoucherType.IsSuccess)
            {
                Row row = this.grdVoucherType.Rows.Add();
                this.grdVoucherType.Row = this.grdVoucherType.Rows.Count - 2;//当前行指向最后一行
                row["colTypeName"] = theVoucherTypeInfo.TypeName;
                row["colCode"] = theVoucherTypeInfo.Code;
                row["colTypeMark"] = theVoucherTypeInfo.TypeMark;
                row["colState"] = theVoucherTypeInfo.State == 1 ? true : false;
                row.UserData = theVoucherTypeInfo;
            }             
 
        }
      

    }
}