using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Resource.BasicData.Domain;
using System.Collections;
using VirtualMachine.Core;


namespace Application.Business.Erp.SupplyChain.Client.BasicData.ClassesMng
{
    public partial class VClassesDesing : Form
    {
        private Classes theCL = new Classes();
        private MClasses theMClasses = new MClasses();
        private bool isSuccess;
        public VClassesDesing()
        {
            InitializeComponent();
            this.InitEvent();
        }
        public Object Show(Classes obj)
        {
            this.theCL = obj;

            if (theCL.Id != "")
            {
                ModelToView();
            }
            this.ShowDialog();
            return theCL;
        }
        private void ModelToView()
        {
            this.txtName.Text = theCL.Name;
            this.txtCode.Text = theCL.Code;
        }
        virtual public Classes thC
        {
            get { return theCL; }
            set { theCL = value; }
        }
        virtual public bool IsSuccess
        {
            get { return isSuccess; }
            set { isSuccess = value; }
        }
        private void InitEvent()
        {
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
        }
        void btnOK_Click(object sender, EventArgs e)
        {
            if (!ValideView()) return;
            theCL = theMClasses.Save(theCL);
            isSuccess = true;
            this.btnOK.FindForm().Close();
        }
        void btnCancel_Click(object sender, EventArgs e)
        {
            this.isSuccess = false;
            this.btnCancel.FindForm().Close();
        }
        private bool ValideView()
        {
            if (this.txtName.Text.Equals(""))
            {
                MessageBox.Show("名称不能为空！");
                return false;
            }
            if (this.txtCode.Text.Equals(""))
            {
                MessageBox.Show("编码不能为空！");
                return false;
            }
            IList lst = new ArrayList();//集合，通过ArrayList类来实现IList接口。
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("Name", this.txtName.Text));
            lst = theMClasses.GetClasses(oq);
            if (lst.Count > 0)
            {
                if (theCL.Id == "")
                {
                    MessageBox.Show("该名称已经存在！");
                    return false;
                }
                else
                {
                    if (theCL.Id != (lst[0] as Classes).Id)
                    {
                        MessageBox.Show("该名称已经存在！");
                        return false;
                    }
                }
            }
            theCL.Name = this.txtName.Text;
            theCL.Code = this.txtCode.Text;
            return true;
        }

    }
}
