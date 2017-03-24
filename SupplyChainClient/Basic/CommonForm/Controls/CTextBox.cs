using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using C1.Win.C1Input;

namespace Application.Business.Erp.SupplyChain.Client.Basic.Controls
{
    public partial class CTextBox : C1TextBox
    {
        public CTextBox()
        {
            InitializeComponent();
        }

        public CTextBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
