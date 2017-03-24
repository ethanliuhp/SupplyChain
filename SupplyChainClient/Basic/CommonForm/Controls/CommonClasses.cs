using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Main;

using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Resource.BasicData.Service;
using Application.Resource.BasicData.Domain;
using Application.Business.Erp.SupplyChain.Client.BasicData.ClassesMng;
using VirtualMachine.Component.Util;


namespace Application.Business.Erp.SupplyChain.Client.Basic.Controls
{
    public partial class CommonClasses : CDropDown
    {
        public override event SelectBefore SelectBefore;
        public override event SelectFinish SelectFinish;
        private IList result = new ArrayList();

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
        private static IClassesSrv theClassesSrv;
        public CommonClasses()
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
            this.ModalButtonClick += new EventHandler(CommonClasses_ModalButtonClick);
            this.KeyDown += new KeyEventHandler(textBox_KeyDown);
            this.DoubleClick += new EventHandler(textBox_DoubleClick);
        }

        void CommonClasses_ModalButtonClick(object sender, EventArgs e)
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
                //this.Value = value;
                //this.ResetText();

                //if (this.Visible)
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

            if (theClassesSrv == null)
                theClassesSrv = StaticMethod.GetService(typeof(IClassesSrv)) as IClassesSrv;

            ObjectQuery oq = new ObjectQuery();
            object obj = new Object();
            if (isFinding)
            {
                if (this.Text.Trim() != "")
                {
                    NHibernate.Criterion.SimpleExpression exp1 = NHibernate.Criterion.Expression.Like("Name", this.Text.Trim() + "%");
                    NHibernate.Criterion.SimpleExpression exp2 = NHibernate.Criterion.Expression.Eq("Code", this.Text.Trim());
                    oq.AddCriterion(NHibernate.Criterion.Expression.Or(exp1, exp2));
                    IList list = theClassesSrv.GetObjects(typeof(Classes), oq);
                    if (list.Count == 1)
                    {
                        Classes aa = list[0] as Classes;
                        if (aa.State == 1)
                        {
                            this.Text = ClientUtil.ToString(aa.Name);
                            result = list;
                            return;
                        }
                    }
                }
            }
            obj = UCL.Locate("°à±ðÎ¬»¤", ClassesExcType.search, oq, this.FindForm());

            result = obj as IList;
            if (this.SelectFinish != null)
            {
                SelectFinish(result);
            }
            if (result == null) result = new ArrayList();
            if (result != null && result.Count == 1)
            {
                Classes bb = result[0] as Classes;
                this.Text = ClientUtil.ToString(bb.Name);
            }
        }
    }
}
