using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.HumanResource;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.StockMng;

namespace Application.Business.Erp.SupplyChain.Client.Basic
{
    public partial class VBasicDataQuery : TBasicDataView
    {
        MStockMng mStockIn = new MStockMng();
        BasicDataOptr currBasicData = null;

        public VBasicDataQuery()
        {
            InitializeComponent();
            InitialEvents();
            LoadBasicDefine();
            LoadBasicDetail();            
        }

        internal void Start()
        {
            
        }

        private void InitialEvents()
        {
            listBoxType.SelectedIndexChanged += new EventHandler(listBoxType_SelectedIndexChanged);
            listBoxType.MouseClick +=new MouseEventHandler(listBoxType_MouseClick);
        }

        private void LoadBasicDefine()
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("State", -1));

            IList list = mStockIn.StockInSrv.GetBasicData(objectQuery);
            foreach (BasicDataOptr obj in list)
            {
                listBoxType.Items.Add(obj);
            }

            listBoxType.DataSource = list;
            listBoxType.DisplayMember = "BasicName";

            if (listBoxType.Items.Count > 0)
            {
                listBoxType.SelectedIndex = 0;
            }
        }

        private void LoadBasicDetail()
        {
            dgvOptr.Rows.Clear();
            currBasicData = listBoxType.SelectedItem as BasicDataOptr;

            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("ParentId", currBasicData.Id));
            objectQuery.AddOrder(Order.Asc("Id"));
            IList list = mStockIn.StockInSrv.GetBasicData(objectQuery);
            foreach (BasicDataOptr model in list)
            {
                int rowIndex = dgvOptr.Rows.Add();
                DataGridViewRow row = dgvOptr.Rows[rowIndex];
                row.Tag = model;
                row.Cells["BasicCode"].Value = model.BasicCode;
                row.Cells["BasicName"].Value = model.BasicName;
                row.Cells["Remark"].Value = model.Descript;
            }
        }

        void listBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvOptr.Rows.Clear();
            currBasicData = listBoxType.SelectedItem as BasicDataOptr;

            //��ӻ���������ϸ�б�
            try
            {
                ObjectQuery objectQuery = new ObjectQuery();
                objectQuery.AddCriterion(Expression.Eq("ParentId", currBasicData.Id));
                objectQuery.AddOrder(Order.Asc("Id"));
                IList list = mStockIn.StockInSrv.GetBasicData(objectQuery);

                foreach (BasicDataOptr mx in list)
                {
                    int i = this.dgvOptr.Rows.Add();
                    DataGridViewRow row = dgvOptr.Rows[i];
                    row.Tag = mx;
                    row.Cells["BasicCode"].Value = mx.BasicCode;
                    row.Cells["BasicName"].Value = mx.BasicName;
                    row.Cells["Remark"].Value = mx.Descript;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("���һ���������ϸ����", ExceptionUtil.ExceptionMessage(ex));
            }

        }

        private void listBoxType_MouseClick(object sender, MouseEventArgs e)
        {
            currBasicData = listBoxType.SelectedItem as BasicDataOptr;
            if (currBasicData != null && currBasicData.Id == "4")
            {
                dgvOptr.Columns[1].HeaderText = "����Ȩ��";
                dgvOptr.Columns[2].HeaderText = "��Ա����";
                dgvOptr.Columns[3].HeaderText = "��ע(����������[¼��]��[���])";
            }

            if (currBasicData != null && currBasicData.Id == "3")
            {
                dgvOptr.Columns[1].HeaderText = "ϵͳ����";
                dgvOptr.Columns[2].HeaderText = "��Ʊ����";
                dgvOptr.Columns[3].HeaderText = "��ע";
            }

            if (currBasicData != null && currBasicData.Id == "2")
            {
                dgvOptr.Columns[1].HeaderText = "��Ʊ����";
                dgvOptr.Columns[2].HeaderText = "������Ŀ";
                dgvOptr.Columns[3].HeaderText = "����˰��";
            }

            if (currBasicData != null && currBasicData.Id == "1")
            {
                dgvOptr.Columns[1].HeaderText = "����";
                dgvOptr.Columns[2].HeaderText = "����";
                dgvOptr.Columns[3].HeaderText = "��ע";
            }

            if (currBasicData != null && currBasicData.Id == "6")
            {
                dgvOptr.Columns[1].HeaderText = "����";
                dgvOptr.Columns[2].HeaderText = "Ʒ��";
                dgvOptr.Columns[3].HeaderText = "��ע";
            }

        }

    }
}