using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan
{
    public partial class VSelectServiceType : Form
    {
        private List<string> _selectServiceType = new List<string>();
        private string selectType = "";
        /// <summary>
        /// 选择的劳务类型
        /// </summary>
        public List<string> SelectServiceType
        {
            get { return _selectServiceType; }
            set { _selectServiceType = value; }
        }

        public VSelectServiceType()
        {
            InitializeComponent();
            InitialForm();
        }
        public VSelectServiceType(string type)
        {
            InitializeComponent();
            InitialForm();


        }
        private void InitialForm()
        {
            InitialEvents();

            foreach (string type in Enum.GetNames(typeof(ResourceRequirePlanDetailServiceType)))
            {
                cbServiceType.Items.Add(type);
                if (type == selectType)
                    cbServiceType.SetSelected(cbServiceType.Items.Count - 1, true);
            }
        }
        private void InitialEvents()
        {
            btnEnter.Click += new EventHandler(btnEnter_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            SelectServiceType.Clear();
            this.Close();
        }

        void btnEnter_Click(object sender, EventArgs e)
        {
            if (cbServiceType.CheckedItems.Count == 0)
            {
                MessageBox.Show("请选择劳务类型！");
                return;
            }

            foreach (string type in cbServiceType.CheckedItems)
            {
                SelectServiceType.Add(type);
            }
            this.Close();
        }
    }
}
