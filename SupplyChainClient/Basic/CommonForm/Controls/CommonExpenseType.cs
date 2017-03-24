using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Collections;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Service;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Domain;
using NHibernate.Criterion;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.FinanceMng.CostProjectUI;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.Basic.Controls
{
    public partial class CommonExpenseType : CDropDown
    {
        public override event SelectBefore SelectBefore;
        public override event SelectFinish SelectFinish;
        private IList result = new ArrayList();
        private static ICostProjectSrv theCosProjectSrv;

        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public void OpenSelect()
        {
            this.Text = "";
            result = new ArrayList();
            OpenView(false);
        }
        public void OpenSelect(string condition)
        {
            this.Text = "";
            result.Clear();
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

        private void OpenView(bool isFinding)
        {
            if (this.ReadOnly) return;
            if (theCosProjectSrv == null)
            {
                theCosProjectSrv = StaticMethod.GetService("CostProjectSrv") as ICostProjectSrv;
            }
            ObjectQuery oq = new ObjectQuery();
            object obj = new object();
            if (isFinding)
            {
                if (this.Text.Trim() != "")
                {
                    oq.AddCriterion(Expression.Eq("Name", this.Text.Trim()));
                    IList list = theCosProjectSrv.GetObjects(typeof(CostProject), oq);
                    if (list.Count == 1)
                    {
                        CostProject master = list[0] as CostProject;
                        this.Text = ClientUtil.ToString(master.Name);
                        result = list;
                        return;
                    }
                }
            }
            obj = UCL.Locate("费用项目选择", CostPtojectType.costProjectSelect, oq, this.FindForm());
            result = obj as IList;
            if (this.SelectFinish != null)
            {
                SelectFinish(result);
            }
            if (result == null) result = new ArrayList();
            if (result != null && result.Count == 1)
            {
                CostProject master = result[0] as CostProject;
                this.Text = ClientUtil.ToString(master.Name);
            }
        }

        public CommonExpenseType()
        {
            InitializeComponent();
        }

        public CommonExpenseType(IContainer container)
        {
            //container.Add(this);

            InitializeComponent();
            this.ShowDropDownButton = false;
            this.ShowModalButton = true;
            this.ShowUpDownButtons = false;
            this.Text = "";
            InitEvent();
        }
        private void InitEvent()
        {
            this.ModalButtonClick += new EventHandler(CommonExpenseType_ModalButtonClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(textBox_KeyDown);
            this.DoubleClick += new EventHandler(CommonExpenseType_DoubleClick);
        }

        void CommonExpenseType_DoubleClick(object sender, EventArgs e)
        {
            OpenView(true);
        }

        void CommonExpenseType_ModalButtonClick(object sender, EventArgs e)
        {
            OpenView(false);
        }
        void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OpenView(true);
            }
        }
    }
}
