using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorDefine.Domain;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorDefine
{
    public partial class VIndicatorInfo : Form
    {
        private IndicatorDefinition tempIndicator = new IndicatorDefinition();
        private bool isBtnOkClicked;
        
        public VIndicatorInfo(IndicatorDefinition indicator)
        {
            InitializeComponent();
            tempIndicator = indicator;
        }

        internal bool Open(IWin32Window owner,ref IndicatorDefinition indicator,Application.Business.Erp.SupplyChain.Enums.Action action)
        {
            IniForm(action);
            this.ShowDialog(owner);
            indicator = tempIndicator;
            return isBtnOkClicked;
        }

        internal bool Open(IWin32Window owner, Application.Business.Erp.SupplyChain.Enums.Action action)
        {
            IniForm(action);
            this.ShowDialog(owner);
            return isBtnOkClicked;
        }

        private void IniForm(Application.Business.Erp.SupplyChain.Enums.Action action)
        {
            if (action == Application.Business.Erp.SupplyChain.Enums.Action.View)
            {
                txtIndicatorCode.ReadOnly = true;
                txtIndicatorName.ReadOnly = true;
                txtIndicatorRemark.ReadOnly = true;
                txtIndicatorUnit.ReadOnly = true;
            }
            txtIndicatorCode.Text = tempIndicator.Code;
            txtIndicatorName.Text = tempIndicator.Name;
            txtIndicatorUnit.Text = tempIndicator.UnitId;
            txtIndicatorRemark.Text = tempIndicator.Description;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (Check())
            {
                isBtnOkClicked = true;
                Close();
            }
        }

        private bool Check()
        {
            string code = txtIndicatorCode.Text;
            if (code == null || code.Trim().Equals(""))
            {
                KnowledgeMessageBox.InforMessage("指标编号不能为空。");
                txtIndicatorCode.Focus();
                return false;
            }

            string name = txtIndicatorName.Text;
            if (name == null || name.Trim().Equals(""))
            {
                KnowledgeMessageBox.InforMessage("指标名称不能为空。");
                txtIndicatorName.Focus();
                return false;
            }

            string indicatorUnit=txtIndicatorUnit.Text;
            string remark=txtIndicatorRemark.Text;

            tempIndicator.Code = code;
            tempIndicator.Name = name;
            tempIndicator.UnitId = indicatorUnit;
            tempIndicator.Description = remark;

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}