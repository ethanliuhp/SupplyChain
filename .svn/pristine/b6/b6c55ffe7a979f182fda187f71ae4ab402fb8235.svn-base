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
                MessageBox.Show("编码不能为空");
                return false;
            }
            if (this.txtTypeName.Text.Equals(""))
            {
                MessageBox.Show("类别名称不能为空！");
                return false;
            }
            if (this.comTypeMark.Text.Equals(""))
            {
                MessageBox.Show("类别字不能为空！");
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
                    MessageBox.Show("所增加的类别字已存在");
                    this.comTypeMark.SelectedItem = "";
                    return false;

                }
                else
                {
                    if (theVoucherTypeInfo.Id != (lst[0] as VoucherTypeInfo).Id)
                    {
                        MessageBox.Show("类别字已经存在！");
                        this.comTypeMark.SelectedItem = "";
                        return false;

                    }
                }
            }
            this.PassToObject();
            return true;
        }
        /// <summary>
        /// 将文本框的赋值到对象
        /// </summary>
        private void PassToObject()
        {
            theVoucherTypeInfo.Code = this.txtCode.Text;
            theVoucherTypeInfo.TypeName = this.txtTypeName.Text;
            switch (comTypeMark.SelectedIndex)
            {
                case 0:
                    theVoucherTypeInfo.TypeMark = "转";
                    break;
                case 1:
                    theVoucherTypeInfo.TypeMark = "记";
                    break;
                case 2:
                    theVoucherTypeInfo.TypeMark = "现";
                    break;
                case 3:
                    theVoucherTypeInfo.TypeMark = "银";
                    break;
                case 4:
                    theVoucherTypeInfo.TypeMark = "现收";
                    break;
                case 5:
                    theVoucherTypeInfo.TypeMark = "现付";
                    break;
                case 6:
                    theVoucherTypeInfo.TypeMark = "银收";
                    break;
                case 7:
                    theVoucherTypeInfo.TypeMark = "银付";
                    break;
                case 8:
                    theVoucherTypeInfo.TypeMark = "收";
                    break;
                case 9:
                    theVoucherTypeInfo.TypeMark = "付";
                    break;

                default:
                    break;
            }
            theVoucherTypeInfo.State = this.checkBox1.Checked ? 1 : 0;
            //ClientUtil.ToInt(this.checkBox1.Checked == true ? true : false);
        }


    }
}