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
    public partial class CommonMonth : CustomComboBox
    {
        public CommonMonth()
        {
            InitializeComponent();
            this.InitData();
        }
        private void InitData()
        {
            this.Items.Clear();
            if (ConstObject.TheLogin != null)
            {
                int aMonth = ConstObject.TheLogin.TheComponentPeriod.NowMonth;
                for (int i = 0; i < 12; i++)
                {
                    this.Items.Add(i+1);
                }
                this.Text = aMonth.ToString();
            }
        }
    }
}
