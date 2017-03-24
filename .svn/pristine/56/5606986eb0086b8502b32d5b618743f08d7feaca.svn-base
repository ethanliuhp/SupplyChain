using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.CorporateResource.Domain;
using System.Collections;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CorporateResource.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using C1.Win.C1FlexGrid;

namespace Application.Business.Erp.SupplyChain.Client.BasicData.PersonInforMng
{
    public partial class VPersonSearch : Form
    {
        PersonInfor thePersonInfor = new PersonInfor();
        private MPersonInfor theMPersonInfor = new MPersonInfor();
        private ICorporateResourceSrv theCorporateResourceSrv = StaticMethod.GetService("CorporateResourceSrv") as ICorporateResourceSrv;
        public IList Result = new ArrayList();
        public VPersonSearch()
        {
            InitializeComponent();
            this.InitData();
            this.InitEvent();
        }

        private void InitData()
        {
            try
            {
                //增加根结点
                this.treeView.Nodes.Clear();
                TreeNode root = new TreeNode("信源诚信息技术有限公司");
                //ICON
                root.ImageIndex = 0;
                root.SelectedImageIndex = 0;
                this.treeView.Nodes.Add(root);

                //增加子结点
                DataSet ds = theMPersonInfor.GetCorporateBantch();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    TreeNode tn = new TreeNode();
                    tn.Text = dr["BantchName"].ToString();
                    tn.Tag = dr["OrgId"];
                    tn.ImageIndex = 1;
                    tn.SelectedImageIndex = 1;
                    root.Nodes.Add(tn);
                    AddTreeNode(tn);
                }
                this.treeView.DrawNode += new DrawTreeNodeEventHandler(treeView_DrawNode);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void InitEvent()
        {
            this.treeView.DrawNode += new DrawTreeNodeEventHandler(treeView_DrawNode);
            this.treeView.AfterSelect += new TreeViewEventHandler(treeView_AfterSelect);
            this.btnCancle.Click += new EventHandler(btnCancle_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.Result.Clear();
            foreach (DataGridViewRow row in this.dtGrid.Rows)
            {
                object selectValue = row.Cells["Selected"].Value;
                if (selectValue != null && (bool)selectValue == true)
                {
                    Result.Add(row.Tag);
                }
                this.btnOK.FindForm().Close();
            }
        }

        void btnCancle_Click(object sender, EventArgs e)
        {
            this.btnCancle.FindForm().Close();
        }

        void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string name = e.Node.Text;
            TreeNode node = e.Node;
            int level = node.Level;
            this.dtGrid.Rows.Clear();
            DataTable table = new DataTable();
            if (level == 1)
            {
                long orgId = ClientUtil.ToLong(e.Node.Tag);
                table = theMPersonInfor.GetPersonInfor(orgId);
            }
            else if (level == 2)
            {
                string perCode = ClientUtil.ToString(e.Node.Tag);
                table = theMPersonInfor.GetPersonInforByCode(perCode);
            }
            if (table.Rows.Count > 0)
            {
                IList list_temp = new ArrayList();
                foreach (DataRow var in table.Rows)
                {
                    PersonInfor master = new PersonInfor();
                    master.PerCode = ClientUtil.ToString(var["PerCode"]);
                    master.PerId = ClientUtil.ToLong(var["PerId"]);
                    master.OrgId = ClientUtil.ToLong(var["OrgId"]);
                    master.Name = ClientUtil.ToString(var["Name"]);

                    list_temp.Add(master);
                }


                foreach (PersonInfor master in list_temp)
                {
                    int newRowIndex = dtGrid.Rows.Add();
                    DataGridViewRow newRow = dtGrid.Rows[newRowIndex];

                    newRow.Cells["PerName"].Value = master.Name;
                    newRow.Cells["PerCode"].Value = master.PerCode;
                    newRow.Cells["PerId"].Value = master.PerId;
                    CorporateBantch corporateBantch = theMPersonInfor.GetCorporateBantchById(master.OrgId);
                    newRow.Cells["BantchName"].Value = corporateBantch.BantchName;
                    newRow.Tag = master;

                }
            }
            else
            {
                this.dtGrid.Rows.Clear();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.dtGrid.Rows.Clear();
            string condition = "1=1";
            if (this.txtPerCode.Text.Trim() != "")
            {
                condition += "and perCode ='" + this.txtPerCode.Text + "'";
            }
            if (this.txtName.Text.Trim() != "")
            {
                condition += "and name like '%" + this.txtName.Text + "%'";
            }
            IList list = theCorporateResourceSrv.GetPersonInfor(condition);


            foreach (PersonInfor master in list)
            {
                int newRowIndex = dtGrid.Rows.Add();
                DataGridViewRow newRow = dtGrid.Rows[newRowIndex];

                newRow.Cells["PerName"].Value = master.Name;
                newRow.Cells["PerId"].Value = master.PerId;
                newRow.Cells["PerCode"].Value = master.PerCode;
                CorporateBantch corporateBantch = theMPersonInfor.GetCorporateBantchById(master.OrgId);
                newRow.Cells["BantchName"].Value = corporateBantch.BantchName;
                newRow.Tag = master;
            }
        }

        private void AddTreeNode(TreeNode treeNode)
        {
            DataTable table = new DataTable();
            if (treeNode != null)
            {
                table = theMPersonInfor.GetPersonInfor(ClientUtil.ToLong(treeNode.Tag));
                foreach (DataRow row in table.Rows)
                {
                    TreeNode tn = new TreeNode();
                    tn.Text = row["Name"].ToString();
                    tn.Tag = row["PerCode"];
                    tn.ImageIndex = 2;
                    tn.SelectedImageIndex = 2;
                    treeNode.Nodes.Add(tn);
                }
            }
        }

        void treeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                e.Graphics.DrawString(e.Node.Text, button1.Font, Brushes.Red, e.Bounds);
            }
            else if (e.Node.Level == 1)
            {
                e.Graphics.DrawString(e.Node.Text, btnCancle.Font, Brushes.Blue, e.Bounds);
            }
        }

        private void grdDtl_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.btnOK_Click(this.btnOK, new EventArgs());
        }
    }
}
