using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Util;
using VirtualMachine.Core;
using System.Collections;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.StockMng;

namespace Application.Business.Erp.SupplyChain.Client.Basic
{
    public partial class VBasicDataSelect : TBasicDataView
    {
        MStockMng mStockIn = new MStockMng();
        private BasicDataOptr basic = new BasicDataOptr();
        IList list = new ArrayList();
        private string basicId = "";
        private string tCode = "";//����
        private string tName = "";//����
        private int disFlag = 0;//�Ƿ���ʾѡ���

        public VBasicDataSelect()
        {
            InitializeComponent();
            dgvOptr.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        virtual public string TCode
        {
            get { return tCode; }
            set { tCode = value; }
        }

        virtual public string BasicId
        {
            get { return basicId; }
            set { basicId = value; }
        }

        virtual public int DisFlag
        {
            get { return disFlag; }
            set { disFlag = value; }
        }


        virtual public string TName
        {
            get { return tName; }
            set { tName = value; }
        }


        private void VBasicDataSelect_Load(object sender, EventArgs e)
        {
            InitialEvents();
            int t = 0;

            if (disFlag == 1)
            {
                dgvOptr.Columns["Selected"].Visible = true;
                this.lnkAll.Visible = true;
                this.lnkNone.Visible = true;
                t++;
            }

            if (basicId == "2")
            {
                this.lblBM.Text = "��������";
                this.lblName.Text = "������Ŀ";

                dgvOptr.Columns[t + 1].HeaderText = "��Ʊ����";
                dgvOptr.Columns[t + 2].HeaderText = "������Ŀ";
                dgvOptr.Columns[t + 3].HeaderText = "���ÿɵֿ���";
            } else if (basicId == "8")
            {
                this.Text = "��λ�˺Ų�ѯ";
                this.lblBM.Text = "��λ����";
                lblName.Text = "��������";
                dgvOptr.Columns[t + 2].HeaderText = "��λ����";
                dgvOptr.Columns[t + 3].HeaderText = "��������";
                dgvOptr.Columns[t + 4].HeaderText = "�����˺�";
            } else if (basicId == "9")
            {
                this.Text = "�����˺Ų�ѯ";
                this.lblBM.Text = "�����˺�";
                this.lblName.Text = "��������";

                dgvOptr.Columns[t + 2].HeaderText = "�����˺�";
                dgvOptr.Columns[t + 3].HeaderText = "��������";
            }

            LoadBasicInfo();
        }

        private void InitialEvents()
        {
            btnSave.Click += new EventHandler(btnSave_Click);
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnClose.Click += new EventHandler(btnClose_Click);
            this.lnkAll.Click += new EventHandler(lnkAll_Click);
            this.lnkNone.Click += new EventHandler(lnkNone_Click);
        }

        void lnkNone_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgvOptr.Rows)
            {
                var.Cells["Selected"].Value = false;
            }
        }

        void lnkAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgvOptr.Rows)
            {
                var.Cells["Selected"].Value = true;
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (this.dgvOptr.Columns["Selected"].Visible ==false && ( dgvOptr.Rows.Count == 0 || dgvOptr.CurrentRow == null))
            {
                MessageBox.Show("����ѡ���¼��");
                return;
            }

            if (this.dgvOptr.Columns["Selected"].Visible == true)
            {
                int s = 0;
                foreach (DataGridViewRow row in dgvOptr.Rows)
                {
                    object checkValue = row.Cells["Selected"].Value;
                    if (checkValue != null && (bool)checkValue == true)
                    {
                        s++;
                    }
                }

                if (s == 0)
                {
                    MessageBox.Show("����ѡ���¼��");
                    return;
                }
            }

            if (this.dgvOptr.Columns["Selected"].Visible == false)
            {
                DataGridViewRow row = dgvOptr.CurrentRow;
                string code = ClientUtil.ToString(row.Cells["BCode"].Value);
                string name = ClientUtil.ToString(row.Cells["BName"].Value);
                string remark = ClientUtil.ToString(row.Cells["Remark"].Value);
                basic.BasicCode = code;
                basic.BasicName = name;
                basic.Descript = remark;

                list.Add(basic);
            }
            else
            {
                foreach (DataGridViewRow row in dgvOptr.Rows)
                {
                    object checkValue = row.Cells["Selected"].Value;
                    if (checkValue != null && (bool)checkValue == true)
                    {
                        basic = new BasicDataOptr();
                        string code = ClientUtil.ToString(row.Cells["BCode"].Value);
                        string name = ClientUtil.ToString(row.Cells["BName"].Value);
                        string remark = ClientUtil.ToString(row.Cells["Remark"].Value);
                        basic.BasicCode = code;
                        basic.BasicName = name;
                        basic.Descript = remark;

                        list.Add(basic);
                    }
                }
            }
            
            this.Close();

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            dgvOptr.Rows.Clear();

            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("ParentId", basicId));
            if( tCode != "")
                objectQuery.AddCriterion(Expression.Eq("BasicCode", tCode));

            if (tName != "")
                objectQuery.AddCriterion(Expression.Eq("BasicName", tName));

            if (!this.txtCode.Text.Trim().Equals(""))
                objectQuery.AddCriterion(Expression.Like("BasicCode", txtCode.Text.Trim(), MatchMode.Anywhere));

            if (!this.txtName.Text.Trim().Equals(""))
                objectQuery.AddCriterion(Expression.Like("BasicName", txtName.Text.Trim(), MatchMode.Anywhere));

            objectQuery.AddOrder(Order.Asc("BasicName"));
            IList list =mStockIn.StockInSrv.GetBasicData(objectQuery);
            foreach (BasicDataOptr model in list)
            {
                int rowIndex = dgvOptr.Rows.Add();
                DataGridViewRow row = dgvOptr.Rows[rowIndex];
                row.Tag = model;
                row.Cells["BCode"].Value = model.BasicCode;
                row.Cells["BName"].Value = model.BasicName;
                row.Cells["Remark"].Value = model.Descript;
            }
        }

        private void LoadBasicInfo()
        {
            dgvOptr.Rows.Clear();

            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("ParentId", basicId));
            if (tCode != "")
                objectQuery.AddCriterion(Expression.Eq("BasicCode", tCode));

            if (tName != "")
                objectQuery.AddCriterion(Expression.Eq("BasicName", tName));

            objectQuery.AddOrder(Order.Asc("BasicName"));
            IList list = mStockIn.StockInSrv.GetBasicData(objectQuery);
            foreach (BasicDataOptr model in list)
            {
                int rowIndex = dgvOptr.Rows.Add();
                DataGridViewRow row = dgvOptr.Rows[rowIndex];
                row.Tag = model;
                row.Cells["BCode"].Value = model.BasicCode;
                row.Cells["BName"].Value = model.BasicName;
                row.Cells["Remark"].Value = model.Descript;
            }

            //dgvOptr.AutoSize = true;            
        }

        public IList VShowDialog()
        {
            this.ShowDialog();
            return list;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}