using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Resource.FinancialResource.RelateClass;
using VirtualMachine.Component.Util;
using System.Collections;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;

namespace Application.Business.Erp.SupplyChain.Client.FinanceMng
{
    public partial class VAddNewVoucherType : Form
    {
        private MVoucherType theMVoucherType = new MVoucherType();
        private bool isSuccess;
        private VoucherTypeInfo theVoucherTypeInfo = new VoucherTypeInfo();
        private IList lst = new ArrayList();
        public Object Show(VoucherTypeInfo dataRetern)
        {
            this.theVoucherTypeInfo = dataRetern;
            if (theVoucherTypeInfo.Id != "")
            {
                this.txtCode.Text = theVoucherTypeInfo.Code;
                this.comTypeMark.SelectedItem = theVoucherTypeInfo.TypeMark;
                this.txtTypeName.Text = theVoucherTypeInfo.TypeName;
                this.checkBox1.Checked = ClientUtil.ToBool(theVoucherTypeInfo.State);
            }
            this.ShowDialog();
            return theVoucherTypeInfo;
        }
        virtual public bool IsSuccess
        {
            get { return isSuccess; }
            set { isSuccess = value; }
        }
        public VAddNewVoucherType()
        {
            InitializeComponent();
            this.InitEvent();

        }
        public void InitEvent()
        {
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.btnCancle.Click += new EventHandler(btnCancle_Click);

        }

        void btnCancle_Click(object sender, EventArgs e)
        {
            this.btnCancle.FindForm().Close();
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValideView()) return;
            theVoucherTypeInfo = theMVoucherType.Save(theVoucherTypeInfo);
            isSuccess = true;
            this.btnSave.FindForm().Close();
        }
        private void ClearText()
        {
            this.txtTypeName.Text = "";
        }
        private bool ValideView()
        {
            if (this.txtCode.Text.Equals(""))
            {
                MessageBox.Show("���벻��Ϊ��");
                return false;
            }
            if (this.txtTypeName.Text.Equals(""))
            {
                MessageBox.Show("������Ʋ���Ϊ�գ�");
                return false;
            }
            if (this.comTypeMark.Text.Equals(""))
            {
                MessageBox.Show("����ֲ���Ϊ�գ�");
                return false;
            }

            IList lst = new ArrayList();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TypeMark", this.comTypeMark.SelectedItem));
            lst = theMVoucherType.getVouType(oq);
            if (lst.Count > 0)
            {
                if (theVoucherTypeInfo.Id == "")
                {
                    MessageBox.Show("�����ӵ�������Ѵ���");
                    this.comTypeMark.SelectedItem = "";
                    return false;

                }
                else
                {
                    if (theVoucherTypeInfo.Id != (lst[0] as VoucherTypeInfo).Id)
                    {
                        MessageBox.Show("������Ѿ����ڣ�");
                        this.comTypeMark.SelectedItem = "";
                        return false;

                    }
                }
            }
            this.PassToObject();
            return true;
        }
        /// <summary>
        /// ���ı���ĸ�ֵ������
        /// </summary>
        private void PassToObject()
        {
            theVoucherTypeInfo.Code = this.txtCode.Text;
            theVoucherTypeInfo.TypeName = this.txtTypeName.Text;
            switch (comTypeMark.SelectedIndex)
            {
                case 0:
                    theVoucherTypeInfo.TypeMark = "ת";
                    break;
                case 1:
                    theVoucherTypeInfo.TypeMark = "��";
                    break;
                case 2:
                    theVoucherTypeInfo.TypeMark = "��";
                    break;
                case 3:
                    theVoucherTypeInfo.TypeMark = "��";
                    break;
                case 4:
                    theVoucherTypeInfo.TypeMark = "����";
                    break;
                case 5:
                    theVoucherTypeInfo.TypeMark = "�ָ�";
                    break;
                case 6:
                    theVoucherTypeInfo.TypeMark = "����";
                    break;
                case 7:
                    theVoucherTypeInfo.TypeMark = "����";
                    break;
                case 8:
                    theVoucherTypeInfo.TypeMark = "��";
                    break;
                case 9:
                    theVoucherTypeInfo.TypeMark = "��";
                    break;

                default:
                    break;
            }
            theVoucherTypeInfo.State = this.checkBox1.Checked ? 1 : 0;
            //ClientUtil.ToInt(this.checkBox1.Checked == true ? true : false);
        }


    }
}