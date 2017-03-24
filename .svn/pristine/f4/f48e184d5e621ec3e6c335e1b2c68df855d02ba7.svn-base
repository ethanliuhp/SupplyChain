using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorDefine.Domain;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorDefine
{
    public partial class VIndicatorCategoryInfo : Form
    {
        private MIndicatorDefine mIndicatorDefine = new MIndicatorDefine();
        
        public VIndicatorCategoryInfo()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool SaveCategory()
        {
            string code = this.customEditCode.Text;
            if (code == null || code.Trim().Equals(""))
            {
                KnowledgeMessageBox.InforMessage("请输入分类编码。");
                customEditCode.Focus();
                return false;
            }

            string name = this.customEditName.Text;
            if (name == null || name.Trim().Equals(""))
            {
                KnowledgeMessageBox.InforMessage("请输入分类名称。");
                customEditName.Focus();
                return false;                
            }

            string remark = richTextBoxRemark.Text;

            IndicatorCategory category = new IndicatorCategory();

            return true;
        }
    }
}