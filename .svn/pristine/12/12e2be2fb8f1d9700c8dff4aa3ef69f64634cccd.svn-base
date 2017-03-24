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
using Application.Resource.BasicData.Domain;


namespace Application.Business.Erp.SupplyChain.Client.BasicData.ClassTeamMng
{
    public partial class VClassTeamDesing : Form
    {
        private ClassTeam theCT = new ClassTeam();
        private MClassTeam theMClassTeam = new MClassTeam();
        private bool isSuccess;
        public VClassTeamDesing()
        {
            InitializeComponent();
            this.InitEvent();
        }
        public Object Show(ClassTeam obj)
        {
            this.theCT = obj;

            if (theCT.Id != "")
            {
                ModelToView();
            }
            this.ShowDialog();
            return theCT;
        }

        private void ModelToView()
        {
            this.txtName.Text = theCT.Name;
            this.txtCode.Text = theCT.Code;
            this.Check0.Checked = theCT.State == 1 ? true : false;
            this.txtDescript.Text = theCT.Descript;
        }
        virtual public ClassTeam thC
        {
            get { return theCT; }
            set { theCT = value; }
        }
        virtual public bool IsSuccess
        {
            get { return isSuccess; }
            set { isSuccess = value; }
        }
        private void InitEvent()
        {
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        void btnCancel_Click(object sender, EventArgs e)
        {

            this.isSuccess = false;
            this.btnCancel.FindForm().Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            if (!ValideView()) return;
            theCT = theMClassTeam.Save(theCT);
            isSuccess = true;
            this.btnOK.FindForm().Close();
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

            oq.AddCriterion(Expression.Eq("Name", this.txtName.Text));

            lst = theMClassTeam.GetClassTeam(oq);
            if (lst.Count > 0)
            {
                if (theCT.Id == "")
                {
                    MessageBox.Show("该名称已经存在！");
                    return false;
                }
                else
                {
                    if (theCT.Id != (lst[0] as ClassTeam).Id)
                    {
                        MessageBox.Show("该名称已经存在！");
                        return false;
                    }
                }
            }


            theCT.Name = this.txtName.Text;
            theCT.Code = this.txtCode.Text;
            theCT.Descript = this.txtDescript.Text;
            theCT.State = this.Check0.Checked ? 1 : 0;

            return true;
        }

    }
}