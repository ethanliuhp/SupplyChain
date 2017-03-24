using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection
{
    public partial class VSelectMaterialLong : Form
    {
        public List<decimal> lstResult = null;
        public List<decimal> Result
        {
            get { return lstResult; }
        }
        public VSelectMaterialLong()
        {
            InitializeComponent();
            lstResult = new List<decimal>();
            IntialEvent();
        }
        public void IntialEvent()
        {
            this.btnCancel.Click+=new EventHandler(btnCancel_Click);
            this.btnSure.Click+=new EventHandler(btnSure_Click);
            SetCheck(null);
        }
        public void btnCancel_Click(object sender, EventArgs e)
        {
            this.Result.Clear();
            this.Close();
        }
        public void btnSure_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }
        public void SetCheck(Control oParentControl)
        {
            IList lstControl = oParentControl != null ? oParentControl.Controls : this.Controls;
 
            foreach (Control oControl in lstControl)
            {
                if (oControl is CheckBox)
                {
                    (oControl as CheckBox).CheckedChanged += new EventHandler(CheckBox_Change);
                }
                else
                {
                    SetCheck(oControl);
                }
            }
        }
        public void CheckBox_Change(object sender, EventArgs e)
        {
            if (sender is CheckBox)
            {
                CheckBox oCheckBox = sender as CheckBox;
                if (oCheckBox.Checked)
                {
                    lstResult.Insert(lstResult.Count,ClientUtil.ToDecimal(oCheckBox.Tag));
                }
                else
                {
                    lstResult.Remove(ClientUtil.ToDecimal(oCheckBox.Tag));
                }
            }
        }
    }
}
