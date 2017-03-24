using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.Util;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.StockMng;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveInRedUI
{
    public partial class VStockMoveInRedSearchCon : BasicUserControl
    {
        private MStockMng model = new MStockMng();
        private VStockMoveInRedSearchList searchList;
        private EnumStockExecType execType;

        public VStockMoveInRedSearchCon(VStockMoveInRedSearchList searchList,EnumStockExecType execType)
        {
            this.searchList = searchList;
            this.execType = execType;
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

            if(txtMoveOutProject.Text!="")
            {
                //oq.AddCriterion(Expression.Eq("OriginalContractNo", txtOriConNo.Text));
                oq.AddCriterion(Expression.Like("MoveOutProjectName", txtMoveOutProject.Text, MatchMode.Anywhere));
            }
            //if (txtSupplier.Text != "" && txtSupplier.Result.Count > 0)
            //{
            //    oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo",txtSupplier.Result[0] as SupplierRelationInfo));
            //}
            oq.AddCriterion(Expression.Eq("Special", Enum.GetName(typeof(EnumStockExecType), execType)));
            //区分项目
            oq.AddCriterion(Expression.Eq("ProjectId", StaticMethod.GetProjectInfo().Id));
            oq.AddOrder(new Order("Code", false));
            IList list = model.StockMoveSrv.GetStockMoveInRed(oq);
            searchList.RefreshData(list);
            this.btnOK.FindForm().Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }
    }
}

