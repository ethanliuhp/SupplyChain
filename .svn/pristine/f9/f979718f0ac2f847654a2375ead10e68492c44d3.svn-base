using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Util;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    public partial class VTimeSelect : Form
    {
        public bool isOkClicked = false;
        public string timeName;

        public VTimeSelect()
        {
            InitializeComponent();
            InitialEvents();
        }

        private void InitialEvents()
        {
            btnOk.Click += new EventHandler(btnOk_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        private void VTimeSelect_Load(object sender, EventArgs e)
        {
            LoadTimeList();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        void btnOk_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvTime.Rows)
            {
                object checkValue = row.Cells["selected"].Value;
                if (checkValue != null)
                {
                    timeName = row.Cells["name"].Value.ToString();
                    isOkClicked = true;
                    break;
                }              
            }    
            Close();
        }

        private void LoadTimeList()
        {
            string[] times = KnowledgeUtil.TimeSpan;
            for (int i = 0; i < times.Length; i++)
            {
                dgvTime.Rows.Add();
                DataGridViewRow row = dgvTime.Rows[i];
                string name = times[i].ToString();
                row.Cells["name"].Value = name;
            }
        }
    }
}