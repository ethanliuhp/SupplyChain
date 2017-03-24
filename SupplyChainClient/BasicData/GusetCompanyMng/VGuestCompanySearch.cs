using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.GuestCompany.Domain;
using Application.Business.Erp.SupplyChain.GuestCompany.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.BasicData.GusetCompanyMng
{
    public partial class VGuestCompanySearch : Form
    {
        GuestCompanyMess theGuestCompanyMess = new GuestCompanyMess();
        private MGuestCompany theMGuestCompany = new MGuestCompany();
        private IGuestCompanySrv theIGuestCompanySrv = StaticMethod.GetService("GuestCompanySrv") as IGuestCompanySrv;
        private static IList result = new ArrayList();

        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VGuestCompanySearch()
        {
            InitializeComponent();
            this.InitData();
            this.InitEvent();
        }

        public void InitData()
        {
            try
            {
                //增加根节点
                this.treeView.Nodes.Clear();
                TreeNode root = new TreeNode("北京信源诚信息技术有限公司");
                root.ImageIndex = 0;
                root.SelectedImageIndex = 0;
                this.treeView.Nodes.Add(root);


                //增加子节点
                IList list_temp = theMGuestCompany.GetGuestComMess();
                foreach (GuestCompanyMess master in list_temp)
                {
                    TreeNode tn = new TreeNode();
                    tn.Text = master.Name;
                    tn.Tag = master.GusComId;
                    tn.ImageIndex = 1;
                    tn.SelectedImageIndex = 1;
                    root.Nodes.Add(tn);
                }

                this.treeView.DrawNode += new DrawTreeNodeEventHandler(treeView_DrawNode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnCancle.Click += new EventHandler(btnCancle_Click);
            this.treeView.AfterSelect += new TreeViewEventHandler(treeView_AfterSelect);
            this.treeView.DrawNode += new DrawTreeNodeEventHandler(treeView_DrawNode);
            this.grdDtl.CellDoubleClick += new DataGridViewCellEventHandler(grdDtl_CellDoubleClick);
            this.btnOK.Click += new EventHandler(btnOK_Click);
        }

        void btnCancle_Click(object sender, EventArgs e)
        {
            this.btnCancle.FindForm().Close();
        }

        void treeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                e.Graphics.DrawString(e.Node.Text, btnSearch.Font, Brushes.Red, e.Bounds);
            }
            else if (e.Node.Level == 1)
            {
                e.Graphics.DrawString(e.Node.Text, btnCancle.Font, Brushes.Blue, e.Bounds);
            }
        }

        void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.grdDtl.Rows.Clear();
            if (e.Node.Level == 0)
            {
                IList list = theIGuestCompanySrv.GetGuestComMess();

                foreach (GuestCompanyMess master in list)
                {
                    int newRowIndex = grdDtl.Rows.Add();
                    DataGridViewRow newRow = grdDtl.Rows[newRowIndex];
                    newRow.Cells["GusComId"].Value = master.GusComId;
                    newRow.Cells["GusComName"].Value = master.Name;
                    newRow.Tag = master;

                }
            }
            if (e.Node.Level == 1)
            {
                string gusComId = e.Node.Tag.ToString();
                string condition = "1=1";
                if (gusComId.Trim() != "")
                {
                    condition += "and GusComId=" + gusComId + "";
                }
                IList list = theIGuestCompanySrv.GetGuestComByCondition(condition);
                foreach (GuestCompanyMess master in list)
                {
                    int newRowIndex = grdDtl.Rows.Add();
                    DataGridViewRow newRow = grdDtl.Rows[newRowIndex];
                    newRow.Cells["GusComId"].Value = master.GusComId;
                    newRow.Cells["GusComName"].Value = master.Name;
                    newRow.Tag = master;

                }
            }
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            this.grdDtl.Rows.Clear();
            string condition = "1=1";
            if (this.txtGusComId.Text.Trim() != "")
            {
                condition += "and GusComId='" + this.txtGusComId.Text + "'";
            }
            if (this.txtName.Text.Trim() != "")
            {
                condition += "and Name like'%" + this.txtName.Text + "%'";
            }
            IList list = theIGuestCompanySrv.GetGuestComByCondition(condition);

            foreach (GuestCompanyMess master in list)
            {
                int newRowIndex = grdDtl.Rows.Add();
                DataGridViewRow newRow = grdDtl.Rows[newRowIndex];

                newRow.Tag = master;
                newRow.Cells["GusComId"].Value = master.GusComId;
                newRow.Cells["GusComName"].Value = master.Name;
            }
        }
        void grdDtl_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.btnOK_Click(this.btnOK, new EventArgs());
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            //记录数
            int count = 0;

            this.Result.Clear();
            foreach (DataGridViewRow row in this.grdDtl.Rows)
            {
                object selectValue = row.Cells["Selected"].Value;
                if (selectValue != null && (bool)selectValue == true)
                {
                    count++;
                    Result.Add(row.Tag);
                }
            }
            if (count == 0)
            {
                MessageBox.Show("请选择客户信息！");
                return;
            }
            if (count > 1)
            {
                MessageBox.Show("只能选择一个客户！");
                Result.Clear();
                return;
            }
            this.btnOK.FindForm().Close();
        }
    }
}