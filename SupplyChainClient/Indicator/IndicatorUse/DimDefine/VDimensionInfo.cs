using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;
using Application.Business.Erp.SupplyChain.Client.BasicData;
using Application.Business.Erp.SupplyChain.Client.Indicator.BasicData;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimDefine
{
    public partial class VDimensionInfo : Form
    {
        private DimensionDefine tempDim = new DimensionDefine();
        private bool isBtnOkClicked;
        private MBasicData basicModel = new MBasicData();

        public VDimensionInfo(DimensionDefine obj)
        {
            InitializeComponent();
            tempDim = obj;
        }

        internal bool Open(IWin32Window owner, ref DimensionDefine obj, Application.Business.Erp.SupplyChain.Enums.Action action)
        {
            IniForm(action,obj);
            this.ShowDialog(owner);
            obj = tempDim;
            return isBtnOkClicked;
        }

        //internal bool Open(IWin32Window owner, Action action)
        //{
        //    IniForm(action);
        //    this.ShowDialog(owner);
        //    return isBtnOkClicked;
        //}

        private void IniForm(Application.Business.Erp.SupplyChain.Enums.Action action,DimensionDefine dim)
        {
            if (action == Application.Business.Erp.SupplyChain.Enums.Action.View)
            {
            }
            else if(action==Application.Business.Erp.SupplyChain.Enums.Action.Edit)
            {
                if (!string.IsNullOrEmpty(dim.Category.DimRegId))
                {
                    txtName.ReadOnly = true;
                }
            }

            basicModel.InitialBasicData("维度表-计算类型", cboCalType, true);

            txtName.Text = tempDim.Name;
            txtFactor.Text = tempDim.Factor.ToString();
            txtCalExp.Text = tempDim.CalExpression;
            if (tempDim.CalTypeCode != null)
            {
                cboCalType.SelectedValue = tempDim.CalTypeCode;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (Check())
            {
                tempDim.Name = txtName.Text;
                string strFactor = txtFactor.Text;
                if (strFactor == null || strFactor.Trim().Equals(""))
                {
                    tempDim.Factor = 0m;
                }
                else
                {
                    tempDim.Factor = ClientUtil.ToDecimal(strFactor);
                }
                tempDim.CalExpression = txtCalExp.Text;
                if (cboCalType.SelectedValue != null)
                {
                    tempDim.CalTypeCode = cboCalType.SelectedValue.ToString();
                    tempDim.CalTypeName = cboCalType.Text;
                }

                isBtnOkClicked = true;
                Close();
            }
        }

        private bool Check()
        {
            string name = txtName.Text;
            if (name == null || name.Trim().Equals(""))
            {
                KnowledgeMessageBox.InforMessage("维度名称不能为空。");
                txtName.Focus();
                return false;
            }

            string factor = txtFactor.Text;
            double df=0d;
            if (factor != null && !factor.Trim().Equals(""))
            {
                if (double.TryParse(factor, out df) == false)
                {
                    KnowledgeMessageBox.InforMessage("请输入正确的权重。");
                    txtFactor.Focus();
                    return false;
                }
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}