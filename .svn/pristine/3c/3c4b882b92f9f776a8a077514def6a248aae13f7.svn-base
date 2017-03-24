using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.Financial.FIUtils;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Domain;

namespace Application.Business.Erp.Financial.Client.FinanceUI.InitialSetting.AccountTitleUI
{
    public partial class VAccTitleCondiction : Application.Business.Erp.Financial.Client.Basic.CommonClass.BaseForm
    {
        public VAccTitleCondiction()
        {
            InitializeComponent();
            this.Load +=new EventHandler(VAccTitleCondiction_Load);
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DictionaryId", typeof(Int32));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Describe", typeof(string));
            dt.Rows.Clear();

            foreach (int i in Enum.GetValues(typeof(AccountType)))
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = Enum.GetName(typeof(AccountType), i);
                dr[2] = FinanceUtil.GetAccountTypeByValue(i);
                dt.Rows.Add(dr);

            }
            this.cbbAccType.DataSource = dt;
            this.cbbAccType.DisplayMember = "Describe";
            this.cbbAccType.ValueMember = "DictionaryId";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFrom_Load(object sender, EventArgs e)
        {

        }

        private void txtTo_Load(object sender, EventArgs e)
        {

        }

        private void pnMain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void VAccTitleCondiction_Load(object sender, EventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {

        }

        private void txtFrom_ArrowClick(object sender, EventArgs e)
        {
            VAccTitleSelect vSelect = new VAccTitleSelect();
            vSelect.ShowDialog();
        }

        private void txtTo_ArrowClick(object sender, EventArgs e)
        {
            VAccTitleSelect vSelect = new VAccTitleSelect();
            vSelect.ShowDialog();
        }

    }
}

