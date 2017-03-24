using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Application.Resource.BasicData.Service;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Main;

using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.SupplyChain.Client.BasicData.MachineClassMng;
using Application.Resource.BasicData.Domain;
using VirtualMachine.Component.Util;


namespace Application.Business.Erp.SupplyChain.Client.Basic.Controls
{
    public partial class CommonMachineClass : CDropDown
    {
        public override event SelectBefore SelectBefore;
        public override event SelectFinish SelectFinish;

        private IList result = new ArrayList();
        virtual public event EventHandler MySelectedIndexChanged;
        virtual public IList Result
        {
            set { result = value; }
            get { return result; }
        }

        public void OpenSelect()
        {

            this.Text = "";
            result = new ArrayList();
            OpenView(false);
        }
        public void OpenSelect(string condition)
        {
            this.Text = condition;
            result.Clear();
            OpenView(true);
        }
        private static IMachineClassSrv theMachineClassSrv;
        public CommonMachineClass()
        {
            InitializeComponent();
            this.ShowDropDownButton = false;
            this.ShowModalButton = true;
            this.ShowUpDownButtons = false;
            this.Text = "";
            InitForm();
        }
        private void InitForm()
        {
            InitEvents();
        }
        private void InitEvents()
        {
            this.ModalButtonClick += new EventHandler(CommonMachineClass_ModalButtonClick);
            this.KeyDown += new KeyEventHandler(textBox_KeyDown);
            this.DoubleClick += new EventHandler(textBox_DoubleClick);
            this.ValueChanged+=new EventHandler(CommonMachineClass_ValueChanged);
            
        }

        void CommonMachineClass_ModalButtonClick(object sender, EventArgs e)
        {
            OpenView(false);
        }

        void textBox_DoubleClick(object sender, EventArgs e)
        {
            OpenView(true);
        }
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                bool isReadOnly = this.ReadOnly;
                bool isVisible = this.Visible;
                bool isEnable = this.Enabled;

                this.Visible = true;
                this.ReadOnly = false;
                this.Enabled = true;
                base.Text = value;

                this.Enabled = isEnable;
                this.Visible = isVisible;
                this.ReadOnly = isReadOnly;
            }
        }
        void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OpenView(true);
            }
        }
        private void OpenView(bool isFinding)
        {
            if (this.ReadOnly) return;

            if (theMachineClassSrv == null)
                theMachineClassSrv = StaticMethod.GetService(typeof(IMachineClassSrv)) as IMachineClassSrv;

            ObjectQuery oq = new ObjectQuery();
            object obj = new Object();
            if (isFinding)
            {
                if (this.Text.Trim() != "")
                {
                    NHibernate.Criterion.SimpleExpression exp1 = NHibernate.Criterion.Expression.Like("Name", this.Text.Trim() + "%");
                    NHibernate.Criterion.SimpleExpression exp2 = NHibernate.Criterion.Expression.Eq("Code", this.Text.Trim());
                    oq.AddCriterion(NHibernate.Criterion.Expression.Or(exp1, exp2));
                    IList list = theMachineClassSrv.GetObjects(typeof(MachineClass), oq);
                    if (list.Count == 1)
                    {
                        MachineClass aa = list[0] as MachineClass;
                        if (aa.State == 1)
                        {
                            this.Text = ClientUtil.ToString(aa.Name);
                            result = list;
                            return;
                        }
                    }
                }
            }
            obj = UCL.Locate("机组信息维护", MachineClassExcType.search, oq, this.FindForm());


            result = obj as IList;
            if (this.SelectFinish != null)
            {
                SelectFinish(result);
            }
            if (result == null) result = new ArrayList();
            if (result != null && result.Count == 1)
            {
                MachineClass bb = result[0] as MachineClass;
                this.Text = ClientUtil.ToString(bb.Name);
            }
        }
        void CommonMachineClass_ValueChanged(object sender, EventArgs e)
        {
            if (MySelectedIndexChanged != null)
            {
                MySelectedIndexChanged(sender, e);
            }
        }
    }
}
