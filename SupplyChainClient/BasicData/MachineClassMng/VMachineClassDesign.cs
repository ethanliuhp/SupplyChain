using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using VirtualMachine.Component.Util;
using System.Collections;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Resource.BasicData.Domain;

namespace Application.Business.Erp.SupplyChain.Client.BasicData.MachineClassMng
{
    public partial class VMachineClassDesign : Form
    {
        private MachineClass theMC = new MachineClass();
        private MMachineClass theMMachineClass = new MMachineClass();
        private bool isSuccess;
        public VMachineClassDesign()
        {
            InitializeComponent();
            this.InitEvent();
            this.WorkKind.DataSource = EnumUtil<enmWorkKind>.GetDescriptions();
        }
        public Object Show(MachineClass obj)
        {
            this.theMC = obj;

            if (theMC.Id != "")
            {
                ModelToView();
            }
            this.ShowDialog();
            return theMC;
        }

        private void ModelToView()
        {
            this.txtName.Text = theMC.Name;
            this.txtCode.Text = theMC.Code;
            this.Check0.Checked = theMC.State == 1 ? true : false;
            this.txtDescript.Text = theMC.Descript;
            this.WorkKind.Text = EnumUtil<enmWorkKind>.GetDescription(theMC.WorkKind);
        }
        virtual public MachineClass theM
        {
            get { return theMC; }
            set { theMC = value; }
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
            theMC = theMMachineClass.Save(theMC);
            isSuccess = true;
            this.btnOK.FindForm().Close();
        }

        private bool ValideView()
        {
            if (this.txtName.Text.Equals(""))
            {
                MessageBox.Show("���Ʋ���Ϊ�գ�");
                return false;
            }
            if (this.txtCode.Text.Equals(""))
            {
                MessageBox.Show("���벻��Ϊ�գ�");
                return false;
            }

            IList lst = new ArrayList();
            ObjectQuery oq = new ObjectQuery();

            oq.AddCriterion(Expression.Eq("Name", this.txtName.Text));

            lst = theMMachineClass.GetMachineClass(oq);
            if (lst.Count > 0)//��lstlst.Count > 0ʱ��ֱ����ʾ�������Ѿ����ڣ�
            {
                if (theMC.Id == "")
                {
                    MessageBox.Show("�������Ѿ����ڣ�");
                    return false;
                }
                else
                {
                    if (theMC.Id != (lst[0] as MachineClass).Id)//������thePM.Id��ʾ��ǰҪ�޸ĵ��м�¼��(lst[0] as PayManner).Id��ʾ�������м�¼��
                    {
                        MessageBox.Show("�������Ѿ����ڣ�");
                        return false;
                    }
                }
            }


            theMC.Name = this.txtName.Text;
            theMC.Code = this.txtCode.Text;
            theMC.Descript = this.txtDescript.Text;
            theMC.State = this.Check0.Checked ? 1 : 0;
            theMC.WorkKind = EnumUtil<enmWorkKind>.FromDescription(ClientUtil.ToString(this.WorkKind.Text));
            return true;
        }



    }
}