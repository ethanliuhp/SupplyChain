using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Collections;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using C1.Win.C1FlexGrid;
using VirtualMachine.Component.Util;
using Application.Resource.BasicData.Domain;
using NHibernate.Mapping;

namespace Application.Business.Erp.SupplyChain.Client.BasicData.MachineClassMng
{
    public partial class VMachineClassSerach : Form
    {
        MachineClass theMC = new MachineClass();
        private MMachineClass theMMachineClass = new MMachineClass();
        private VMachineClass theVMachineClass = new VMachineClass();

        public IList Result = new ArrayList();
        public VMachineClassSerach()
        {
            InitializeComponent();
            this.InitEvent();
            Data();
           
            if (this.grdDtl.Rows.Count > 1)
            {
                this.grdDtl.Rows[1].Selected =true;
            }
        }

        private void Data()
        {
           IList lst = theMMachineClass.GetMachineClass();
            grdDtl.Rows.Count = 1;
            foreach (MachineClass var in lst)
            {
                Row row = this.grdDtl.Rows.Add();
                row["colName"] = var.Name;
                row["colCode"] = var.Code;
                row["colDescript"] = var.Descript;
                row["colState"] = var.State == 1 ? true : false;
                row["WorkKind"] = EnumUtil<enmWorkKind>.GetDescription(var.WorkKind);
                row.UserData = var;

            }
        }
       
       
        virtual public MachineClass theM
        {
            get { return theMC; }
            set { theMC = value; }
        }
        private void InitEvent()
        {
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
           
            this.grdDtl.DoubleClick += new EventHandler(grdDtl_DoubleClick);
        }

        void grdDtl_DoubleClick(object sender, EventArgs e)
        {
            this.btnOK_Click(this.btnOK, new EventArgs());
            
        }

      

        //void btnSearch_Click(object sender, EventArgs e)
        //{
        //    ObjectQuery oq = new ObjectQuery();


        //    IList lst = new ArrayList();//集合，通过ArrayList类来实现IList接口。
        //    if (this.txtName.Text != "")
        //    {
        //        oq.AddCriterion(Expression.Eq("Name", this.txtName.Text));
        //    }
        //    if (this.txtCode.Text != "")
        //    {
        //        oq.AddCriterion(Expression.Eq("Code", this.txtCode.Text));
        //    }
        //    if (this.txtDescript.Text != "")
        //    {
        //        oq.AddCriterion(Expression.Eq("Descript", this.txtDescript.Text));
        //    }
        //    if (this.Check0.Checked)
        //    {
        //        oq.AddCriterion(Expression.Eq("State", this.Check0.Checked));
        //    }
        //    lst = theMMachineClass.GetMachineClass(oq);
        //    if (lst.Count > 0)
        //    {
        //        grdDtl.Rows.Count = 1;
        //        foreach (MachineClass var in lst)
        //        {
        //            Row row = this.grdDtl.Rows.Add();
        //            row["colName"] = var.Name;
        //            row["colCode"] = var.Code;
        //            row["colDescript"] = var.Descript;
        //            row["colState"] = var.State == 1 ? true : false;
        //            row.UserData = var;

        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("对不起，您所查找的数据信息不存在！");
        //    }
        //}

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnCancel.FindForm().Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.Result.Clear();
            foreach (Row var in this.grdDtl.Rows)
            {
                if (var.Selected)
                {
                    Result.Add(var.UserData);//放到集合中
                }
            }
            this.btnOK.FindForm().Close();
        }

      
    }
}