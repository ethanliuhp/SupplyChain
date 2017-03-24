using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Resource.FinancialResource.RelateClass;
using System.Collections;
using VirtualMachine.Core.Expression;
using VirtualMachine.Core;
using C1.Win.C1FlexGrid;


namespace Application.Business.Erp.SupplyChain.Client.FinanceMng
{
    public partial class VVoucherTypeSearch : Form
    {
        private MVoucherType theMVoucherType = new MVoucherType();
        public IList Result = new ArrayList();
        private bool isSuccess;
        private VoucherTypeInfo DataView = new VoucherTypeInfo();
        public Object Show(VoucherTypeInfo dataRetern)
        {
            this.DataView = dataRetern;
            this.ShowDialog();
            return DataView;
        }
        virtual public bool IsSuccess
        {
            get { return isSuccess; }
            set { isSuccess = value; }
        }
        public VVoucherTypeSearch()
        {
            InitializeComponent();
            this.InitEvent();
        }
        public void InitEvent()
        {
            this.btnok.Click += new EventHandler(btnok_Click);
            this.btnCancle.Click += new EventHandler(btnCancle_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.Load += new EventHandler(VVoucherTypeSearch_Load);
            this.grdVoucherType.DoubleClick += new EventHandler(grdVoucherType_DoubleClick);
        }

        void grdVoucherType_DoubleClick(object sender, EventArgs e)
        {
            this.btnok_Click(this.btnok, new EventArgs());
        }

        void VVoucherTypeSearch_Load(object sender, EventArgs e)
        {

            ObjectQuery objectQuery = new ObjectQuery();
             IList lst = new ArrayList();
            objectQuery.AddOrder(new NHibernate.Criterion.Order("Code", true));

            lst = theMVoucherType.getVouType(objectQuery);

            foreach (VoucherTypeInfo var in lst)
            {
                Row row = this.grdVoucherType.Rows.Add();
                row["colTypeName"] = var.TypeName;
                row["colTypeMark"] = var.TypeMark;
                row["colCode"] = var.Code;
                row["colState"] = var.State == 1 ? true : false;
                row.UserData = var;
            }
            if (lst.Count > 0)
            {
                this.grdVoucherType.Rows[1].Selected = true;
            }
            else
            {
                MessageBox.Show("数据为空！");
            }

        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            this.grdVoucherType.Rows.Count = 1;
            if (this.comTerm1.Text == "")
            {
                MessageBox.Show("请选择查询条件");
                
                return;
            }
            IList lst = new ArrayList();
            ObjectQuery oq = new ObjectQuery();
            oq.AddOrder(new NHibernate.Criterion.Order("Code", true));
            switch (comTerm1.SelectedIndex)
            {
                case 0:
                    ObjectQuery objectQuery = new ObjectQuery();
                    lst = theMVoucherType.getVouType(objectQuery);
                    this.PassToTerm();
                    break;
                case 1:
                    oq.AddCriterion(Expression.Eq("Code", this.txtContent1.Text));
                    break;
                case 2:
                    oq.AddCriterion(Expression.Eq("TypeName", this.txtContent1.Text));
                    break;
                case 3:
               
                    oq.AddCriterion(Expression.Eq("TypeMark", this.txtContent1.Text));
                    break;
 
                default:
                    break;

            }
            lst = theMVoucherType.getVouType(oq);
            foreach (VoucherTypeInfo var in lst)
            {
                Row row = this.grdVoucherType.Rows.Add();
                row["colTypeName"] = var.TypeName;
                row["colCode"] = var.Code;
                row["colTypeMark"] = var.TypeMark;
                row["colState"] = var.State == 1 ? true : false;
                row.UserData = var;
            }
            if (lst.Count > 0)
            {
                this.grdVoucherType.Rows[1].Selected = true;
            }
            else
            {
                MessageBox.Show("没有要查询的数据！");
            }

        }

        void btnCancle_Click(object sender, EventArgs e)
        {
            this.btnCancle.FindForm().Close();

        }

        void btnok_Click(object sender, EventArgs e)
        {
            this.Result.Clear();
            foreach (Row var in this.grdVoucherType.Rows)
            {
                if (var.Selected)
                {
                    Result.Add(var.UserData);
                }
            }
            this.btnok.FindForm().Close();

        }

        private void PassToTerm()
        {
            IList lst = new ArrayList();
            if (lst.Count > 0)
            {

                VVoucherTypeSearch Search1 = new VVoucherTypeSearch();
                Search1.grdVoucherType.Rows.Count = 1;
                foreach (VoucherTypeInfo var in lst)
                {
                    Row row = this.grdVoucherType.Rows.Add();
                    row["colTypeName"] = var.TypeName;
                    row["colCode"] = var.Code;
                    row["colTypeMark"] = var.TypeMark;
                    row["colState"] = var.State == 1 ? true : false;
                    row.UserData = var;
                }

            }
        }

    }
}