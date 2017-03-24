using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using C1.Win.C1Input;
using System.Collections;
using VirtualMachine.Core;

namespace Application.Business.Erp.SupplyChain.Client.Basic.Controls
{
    public delegate void SelectFinish(IList aResult);
    public delegate void SelectBefore(ObjectQuery aObjectQuery);
    public partial class CDropDown : VirtualMachine.Component.WinControls.Controls.CustomCDropDown
    {
        virtual public event SelectFinish SelectFinish = null;
        virtual public event SelectBefore SelectBefore = null;
        public CDropDown()
        {
            InitializeComponent();
        }
    }
}
