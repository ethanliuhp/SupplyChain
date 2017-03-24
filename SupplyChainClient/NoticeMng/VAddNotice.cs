using System;
using System.Windows.Forms;
using VirtualMachine.Component.Util;
using System.Collections;
using VirtualMachine.Notice.Domain;
using Application.Resource.CommonClass.Domain;
using System.Runtime.Remoting.Messaging;

namespace Application.Business.Erp.SupplyChain.Client.NoticeMng
{
    public partial class VAddNotice : Form
    {
        private MNotice theMEdiDept = new MNotice();
        private bool isSuccess;
        private Notice theEdiDept = new Notice();
        private IList lst = new ArrayList();
        public Object Show(Notice dataRetern)
        {
            this.theEdiDept = dataRetern;

            this.ShowDialog();
            return theEdiDept;
        }
        virtual public bool IsSuccess
        {
            get { return isSuccess; }
            set { isSuccess = value; }
        }

        /// <summary>
        /// 方法初始化
        /// </summary>

        public VAddNotice()
        {
            InitializeComponent();
            this.InitEvent();
        }
        private void InitEvent()
        {
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.btnCancle.Click += new EventHandler(btnCancle_Click);
            this.Load += new EventHandler(VAddEdiDept_Load);
        }

        void VAddEdiDept_Load(object sender, EventArgs e)
        {
            if (theEdiDept.Id != "")
            {
                this.cboxKind.SelectedText = theEdiDept.Kind;
                this.cboxKind.Text = theEdiDept.Kind;

                switch (theEdiDept.Level)
                {
                    case 1:
                        this.cboxLevel.SelectedText = "优";
                        this.cboxLevel.Text = "优";
                        break;
                    case 2:
                        this.cboxLevel.SelectedText = "中";
                        this.cboxLevel.Text = "中";
                        break;
                    case 3:
                        this.cboxLevel.SelectedText = "低";
                        this.cboxLevel.Text = "低";
                        break;
                    default:
                        break;
                }
                this.tboxContent.Text = theEdiDept.Content;
                this.dtpBeginDate.Value = theEdiDept.BeginDate;
                this.tboxFbzq.Text = theEdiDept.Cycle.ToString();
                this.dtpcreatedate.Value = theEdiDept.CreateDate;
                this.tboxCreatePerson.Text = theEdiDept.Createperson;
                this.checkBox1.Checked = theEdiDept.State == 0 ? false : true;
            }
            else
            {

                Login login = CallContextUtil.LogicalGetData<Login>("LoginInformation");
                this.dtpcreatedate.Value = login.LoginDate;
                this.tboxCreatePerson.Text = login.ThePerson.Name;
                this.tboxFbzq.Text = "3";
                this.cboxKind.SelectedText = "系统公告";
                this.cboxLevel.SelectedText = "优";
            }
        }
        void btnCancle_Click(object sender, EventArgs e)
        {
            this.btnCancle.FindForm().Close();
        }
        void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValideView()) return;
            theEdiDept = theMEdiDept.Save(theEdiDept);
            isSuccess = true;
            this.btnSave.FindForm().Close();
        }
        /// <summary>
        /// 清空文本框
        /// </summary>
        private void ClearText()
        {
            this.tboxContent.Text = "";
        }

        /// <summary>
        /// 为空检验
        /// </summary>
        /// <returns></returns>
        private bool ValideView()
        {
            if (this.tboxContent.Text.Equals(""))
            {
                MessageBox.Show("公告内容不能为空！");
                return false;
            }
            this.PassToObject();
            return true;
        }
        /// <summary>
        /// 将文本框的赋值到对象
        /// </summary>
        private void PassToObject()
        {
            theEdiDept.Kind = this.cboxKind.Text;
            switch (this.cboxLevel.Text)
            {
                case "优":
                    theEdiDept.Level = 1;
                    break;
                case "中":
                    theEdiDept.Level = 2;
                    break;
                case "低":
                    theEdiDept.Level = 3;
                    break;
                default:
                    break;
            }
            theEdiDept.Content = this.tboxContent.Text;
            theEdiDept.BeginDate = ClientUtil.ToDateTime(this.dtpBeginDate.Text);
            theEdiDept.Cycle = ClientUtil.ToInt(this.tboxFbzq.Text);
            theEdiDept.CreateDate = dtpcreatedate.Value;
            theEdiDept.Createperson = this.tboxCreatePerson.Text;
            theEdiDept.State = checkBox1.Checked == false ? 0 : 1;
        }
    }
}


