using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost
{
    public partial class VIndirectCostSearchCon : BasicUserControl
    {
        private MIndirectCost model = new MIndirectCost();
        private VIndirectCostSearchList searchList;


        public VIndirectCostSearchCon(VIndirectCostSearchList searchList )
        {
            this.searchList = searchList;
 
            InitializeComponent();            
            this.ViewType = VirtualMachine.Component.WinMVC.core.ViewType.Search;
            this.InitForm();
            this.InitEvent();
            btnOK.Focus();

        }

        private void InitForm()
        {
            this.dtpDateBegin.Value = ConstObject.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.LoginDate;
            //txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode;
        }

        private void InitEvent()
        {
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            if (this.txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Between("Code", this.txtCodeBegin.Text, this.txtCodeEnd.Text));
            }

            //查询自己的单据
            oq.AddCriterion(Expression.Eq("CreatePerson", ConstObject.LoginPersonInfo));
            oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));
            oq.AddCriterion(Expression.Eq("OperOrgInfo", ConstObject.TheLogin.TheOperationOrgInfo));
            oq.AddOrder(new Order("Code", false));
            IList list = model.IndirectCostSvr.QueryIndirectCostMaster(oq);  
            IList lst=new ArrayList();
            foreach (IndirectCostMaster oMaster in list)
            {
                if (oMaster.IndirectCost.Count>0)
                {

                }
                else
                {
                    lst.Add(oMaster);
                }
            }
            foreach (IndirectCostMaster oMaster in lst)
            {
                list.Remove(oMaster);
            }
            searchList.RefreshData(list);
            this.btnOK.FindForm().Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }
    }
}
