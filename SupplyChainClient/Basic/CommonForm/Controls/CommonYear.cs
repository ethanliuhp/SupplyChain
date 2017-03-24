using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.Basic.Controls
{
    public partial class CommonYear : CustomComboBox
    {
        public CommonYear()
        {
            InitializeComponent();
            this.InitData();
        }
        private void InitData()
        {
            this.Items.Clear();
            if (ConstObject.TheLogin != null)
            {
                int aYear = ConstObject.TheLogin.TheComponentPeriod.NowYear;
                //int aMonth = ConstObject.TheLogin.TheComponentPeriod.NowMonth;
                for (int i = 0; i < 10; i++)
                {
                    this.Items.Add(aYear - 5 + i);
                }
                this.Text = aYear.ToString();
            }
        }
    }
}
