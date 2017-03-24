using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using System.Collections;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireLedger.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection
{
    public partial class VMaterialHireRankQuantity : Form
    {
        public IList Result;
        public VMaterialHireRankQuantity()
        {
            InitializeComponent();
            InitData();
            InitEvent();
        }
        public int detailId;

        public void InitData()
        {

        }
        public void InitEvent()
        {
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.dgRankQuantity.CellDoubleClick += new DataGridViewCellEventHandler(dgRankQuantity_CellDoubleClick);
        }

        void dgRankQuantity_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (this.dgRankQuantity.Columns[e.ColumnIndex].Name.Equals(colRank.Name))
            {
                DataGridViewRow theCurrowRow = this.dgRankQuantity.CurrentRow;
                CommonSupplier theSupplierSelect = new CommonSupplier();
                DataGridViewCell cell = this.dgRankQuantity[e.ColumnIndex, e.RowIndex];
                object tempValue = cell.EditedFormattedValue;
                if (tempValue != null && !tempValue.Equals(""))
                {
                    theSupplierSelect.OpenSelect();
                }
                else
                {
                    theSupplierSelect.OpenSelect();
                }
                IList list = theSupplierSelect.Result;
                foreach (SupplierRelationInfo theSupplier in list)
                {
                    int i = dgRankQuantity.Rows.Add();
                    this.dgRankQuantity[colRank.Name, i].Tag = theSupplier;
                    this.dgRankQuantity[colRank.Name, i].Value = theSupplier.SupplierInfo.Name;

                    i++;
                }
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            //校验数据
            foreach (DataGridViewRow dr in dgRankQuantity.Rows)
            {
                //最后一行不进行校验
                if (dr.IsNewRow) break;

                if (dr.Cells[colRankQuantity.Name].Value == null)
                {
                    MessageBox.Show("数量不能为空,请输入数量！");
                    dgRankQuantity.CurrentCell = dr.Cells[colRankQuantity.Name];
                    return;
                }
            }

            Result = new ArrayList();
            foreach (DataGridViewRow var in dgRankQuantity.Rows)
            {
                //if (var.IsNewRow) break;
                //MaterialRentalLedgerDetail theDetail = new MaterialRentalLedgerDetail();
                //theDetail.Id = ClientUtil.ToString(var.Cells[colDtlId.Name].Value);
                //theDetail.TheRank = var.Cells[colRank.Name].Tag as SupplierRelationInfo;
                //theDetail.TheRankName = (var.Cells[colRank.Name].Tag as SupplierRelationInfo).SupplierInfo.Name;
                //theDetail.Quantity = ClientUtil.ToDecimal(var.Cells[colRankQuantity.Name].Value);
                //theDetail.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                ////区分明细的标识
                //theDetail.TempIndex = detailId;
                //Result.Add(theDetail);
            }
            this.Close();
        }
        /// <summary>
        /// OpenMaterialRank
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="tagId">用来区分明细对应的多条队伍信息</param>
        public void OpenMaterialRank(IList lst, int Index)
        {
            detailId = Index;
            Clear();
            if (lst != null)
            {
                foreach (MatHireLedgerDetail detail in lst)
                {
                    int i = this.dgRankQuantity.Rows.Add();
                    this.dgRankQuantity[colRank.Name, i].Tag = detail.TheRank;
                    this.dgRankQuantity[colRank.Name, i].Value = detail.TheRankName;
                    this.dgRankQuantity[colRankQuantity.Name, i].Value = detail.Quantity;
                    this.dgRankQuantity[colDescript.Name, i].Value = detail.TempIndex;
                    this.dgRankQuantity[colDtlId.Name, i].Value = detail.Id;
                }
            }
            this.ShowDialog();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (this.dgRankQuantity.Rows.Count - 1 > 0)
            {
                Result = new ArrayList();
                foreach (DataGridViewRow var in dgRankQuantity.Rows)
                {
                    if (var.IsNewRow) break;

                    MatHireLedgerDetail theDetail = new MatHireLedgerDetail();
                    theDetail.Id = ClientUtil.ToString(var.Cells[colDtlId.Name].Value);
                    theDetail.TheRank = var.Cells[colRank.Name].Tag as SupplierRelationInfo;
                    theDetail.TheRankName = (var.Cells[colRank.Name].Tag as SupplierRelationInfo).SupplierInfo.Name;
                    theDetail.Quantity = ClientUtil.ToDecimal(var.Cells[colRankQuantity.Name].Value);
                    theDetail.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                    //区分明细的标识
                    theDetail.TempIndex = detailId;
                    Result.Add(theDetail);
                }
            }
        }

        private void Clear()
        {
            this.dgRankQuantity.Rows.Clear();
        }
    }
}
