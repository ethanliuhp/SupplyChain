using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Application.Resource.BasicData.Domain;
using C1.Win.C1FlexGrid;

namespace Application.Business.Erp.SupplyChain.Client.BasicData.ClassesMng
{
    public partial class VClassesSerach : Form
    {
        Classes theCL = new Classes();
        private MClasses theMClasses = new MClasses();
        public IList Result = new ArrayList();
        public VClassesSerach()
        {
            InitializeComponent();
            this.InitEvent();
            InitData();
            if (this.grdDtl.Rows.Count > 1)
            {
                this.grdDtl.Row = 1;
            }
        }
        private void InitEvent()
        {
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.grdDtl.DoubleClick += new EventHandler(grdDtl_DoubleClick);
        }

        void grdDtl_DoubleClick(object sender, EventArgs e)
        {
            this.btnOK_Click(this.btnOK, new EventArgs());
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnCancel.FindForm().Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.Result.Clear();
            foreach (Row var in this.grdDtl.Rows)
            {
                if (var.Selected)
                {
                    Result.Add(var.UserData);
                }
            }
            this.btnOK.FindForm().Close();
        }
        private bool ValdieView()
        {

            //IList lst = theMClasses.GetClasses();
            //if (lst.Count == 0)
            //{
            //    MessageBox.Show("数据库的记录为空，没有可查询和编辑的数据！");
            //    return false;
            //}
            return true;
        }
        private void InitData()
        {
            this.Data();
        }

        private void Data()
        {

            IList lst = theMClasses.GetClasses();

            grdDtl.Rows.Count = 1;
            foreach (Classes var in lst)
            {
                Row row = this.grdDtl.Rows.Add();
                row["colName"] = var.Name;
                row["colCode"] = var.Code;
                row.UserData = var;
            }

        }
    }
}
