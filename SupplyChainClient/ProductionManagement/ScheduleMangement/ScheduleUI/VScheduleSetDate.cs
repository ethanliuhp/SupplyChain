using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI
{
    public partial class VScheduleSetDate : Form
    {
        private bool isUpdate = false;
        public bool IsUpdate
        {
            get { return isUpdate; }
            set { isUpdate = value; }
        }
        public DateTime StartDate
        {
            get { return this.dtpDateBegin.Value; }
        }
        public DateTime EndDate
        {
            get { return this.dtpDateEnd .Value; }
        }
        public VScheduleSetDate()
        {
            InitializeComponent();
            btnSetConfigDate.Click += new EventHandler(btnSetConfigDate_Click);
            btnCancel .Click +=new EventHandler(btnCancel_Click);
        }
        public void btnSetConfigDate_Click(object sender, EventArgs e)
        {
            if (DateTime.Parse(StartDate.ToShortDateString()) <= DateTime.Parse(EndDate.ToShortDateString()))
            {
                IsUpdate = true;
                this.Close ();
            }
            else
            {
                IsUpdate = false ;
                MessageBox.Show(string.Format("起始时间[{0}]应该小于等于结束时间[{1}]", StartDate.ToShortDateString(), EndDate.ToShortDateString()));
            }
        }
        public void btnCancel_Click(object sender, EventArgs e)
        {
            isUpdate = false;
            this.Close();
        }
    }
}
