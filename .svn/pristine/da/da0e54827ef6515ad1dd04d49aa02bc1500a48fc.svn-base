using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialReturnMng.Domain;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialReturn
{
    public partial class VMaterialRepair : TBasicDataView
    {
        public IList Result;
        MaterialRepair materialRepair;
        public VMaterialRepair(Material material, IList list_RepairContent)
        {
            InitializeComponent();
            InitData(material, list_RepairContent);
            InitEvent();
        }

        private void InitData(Material material, IList list)
        {
            //VBasicDataOptr.InitMatRepairCon(colRepairContent, true, material);
            foreach (string CostSet in list)
            {
                colRepairContent.Items.Add(CostSet);
            }
        }

        private void InitEvent()
        {
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.dgMatRepair.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(dgMatRepair_RowHeaderMouseClick);
            this.dgDelete.Click += new EventHandler(dgDelete_Click);
        }

        void dgMatRepair_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point _Point = dgMatRepair.PointToClient(Cursor.Position);
                this.contextMenuStrip1.Show(dgMatRepair, _Point);
            }
        }

        void dgDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr = dgMatRepair.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                dgMatRepair.Rows.Remove(dr);
                if (dr.Tag != null)
                {
                    (dr.Tag as MaterialReturnDetail).TempData = "删除";
                }
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnCancel_Click(object sender, EventArgs e)
        {
            GetMaterialRepair();
            btnCancel.FindForm().Close();
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnOK_Click(object sender, EventArgs e)
        {
            GetMaterialRepair();
            btnOK.FindForm().Close();
        }

        public void OpenMaterialRepair(IList list)
        {
            this.dgMatRepair.Rows.Clear();
            try
            {
                if (list != null)
                {
                    //显示数据
                    foreach (MaterialRepair materialRepair in list)
                    {
                        int i = this.dgMatRepair.Rows.Add();
                        this.dgMatRepair[colRepairContent.Name, i].Value = materialRepair.WorkContent;
                        this.dgMatRepair[colRepairQuantity.Name, i].Value = materialRepair.Quantity;
                        this.dgMatRepair[colRepairDescript.Name, i].Value = materialRepair.Descript;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            ShowDialog();
        }

        private void GetMaterialRepair()
        {
            Result = new ArrayList();
            foreach (DataGridViewRow var in dgMatRepair.Rows)
            {
                if (var.IsNewRow) break;
                materialRepair = new MaterialRepair();
                materialRepair.WorkContent = ClientUtil.ToString(var.Cells[colRepairContent.Name].Value);
                materialRepair.Quantity = ClientUtil.ToDecimal(var.Cells[colRepairQuantity.Name].Value);
                materialRepair.Descript = ClientUtil.ToString(var.Cells[colRepairDescript.Name].Value);
                Result.Add(materialRepair);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.GetMaterialRepair();
        }
    }
}
