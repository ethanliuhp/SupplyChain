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
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireLedger.Domain;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireLedger
{
    public partial class VMaterialHireLedgerSelector : TBasicDataView
    {
        private IList result = new ArrayList();
        //料具出租方
        private SupplierRelationInfo TheSupplier;
        CurrentProjectInfo ProjectInfo  ;
        EnumMatHireType MatHireType;
        bool IsProject = false;
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }
        MMaterialHireMng model = new MMaterialHireMng();
        public VMaterialHireLedgerSelector(SupplierRelationInfo theSupplier, CurrentProjectInfo ProjectInfo, EnumMatHireType matHireType)
        {
            InitializeComponent();
            this.TheSupplier = theSupplier;
            this.ProjectInfo = ProjectInfo;
            this.MatHireType = matHireType;
            InitEvent();
            this.chkAll.Visible = this.chkUnCheck.Visible = this.MatHireType == EnumMatHireType.普通料具;
            this.customLabel4.Visible = this.IsProject;
            this.txtBorrowUnit.Visible = this.IsProject;
            this.customLabel5.Visible = this.IsProject;
            this.txtPart.Visible = this.IsProject;
            this.colBorrowUnit.Visible = this.IsProject;
            this.colPart.Visible = this.IsProject;
            this.colSubject.Visible = this.IsProject;
        }

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.dgDetail.CellContentClick+=new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            this.chkUnCheck.CheckedChanged += new EventHandler(Check_CheckedChanged);
            this.chkAll.CheckedChanged += new EventHandler(Check_CheckedChanged);
        }
        public void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == this.colSelect.Index)
            {
                if (ClientUtil.ToBool(dgDetail[e.ColumnIndex, e.RowIndex].Value))
                {
                    if (this.MatHireType != EnumMatHireType.普通料具)
                    {
                        List<DataGridViewCell> lstCell = dgDetail.Rows.OfType<DataGridViewRow>().
                            Where(a => a.Index != e.RowIndex && ClientUtil.ToBool(a.Cells[colSelect.Name].Value))
                            .Select(a => a.Cells[colSelect.Name]).ToList();
                        foreach (DataGridViewCell oCell in lstCell)
                        {
                            oCell.Value = false;
                        }  
                    }
                }
                this.chkUnCheck.CheckedChanged -= new EventHandler(Check_CheckedChanged);
                this.chkAll.CheckedChanged -= new EventHandler(Check_CheckedChanged);
                chkAll.Checked = dgDetail.Rows.OfType<DataGridViewRow>().Where(a => ClientUtil.ToBool(a.Cells[colSelect.Name].Value)).Count() == dgDetail.Rows.Count;
                chkUnCheck.Checked = false;
                this.chkUnCheck.CheckedChanged += new EventHandler(Check_CheckedChanged);
                this.chkAll.CheckedChanged += new EventHandler(Check_CheckedChanged);
            }
        }
        public void Check_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox)
            {
                CheckBox check = sender as CheckBox;
                if (check == chkAll)
                {
                    foreach (DataGridViewRow oRow in dgDetail.Rows)
                    {
                        oRow.Cells[colSelect.Name].Value = true;
                    }
                }
                else if (check == chkUnCheck)
                {
                    foreach (DataGridViewRow oRow in dgDetail.Rows)
                    {
                        oRow.Cells[colSelect.Name].Value = !ClientUtil.ToBool(oRow.Cells[colSelect.Name].Value);
                    }
                }
            }
        }
        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnCancel.FindForm().Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.result.Clear();
            this.dgDetail.EndEdit();
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) break;
                bool IsChecked = Convert.ToBoolean(var.Cells[colSelect.Name].EditedFormattedValue);
                if (IsChecked)
                {
                    MatHireLedgerMaster master = var.Cells[colMaterialCode.Name].Tag as MatHireLedgerMaster;
                    result.Add(master);
                }
            }
            this.btnOK.FindForm().Close();
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgDetail.Rows.Clear();
            this.GetMaterialRenLed();
        }

        private void GetMaterialRenLed()
        {
            ObjectQuery oq = new ObjectQuery();
            IList list = new ArrayList();
            IList temp = new ArrayList();

            //oq.AddCriterion(Expression.Eq("WashType", 0));//收料单
            //if (this.MatHireType != EnumMatHireType.其他)
            //{
                oq.AddCriterion(Expression.Eq("MatHireType", this.MatHireType));
           //}
            oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", TheSupplier));//料具出租方
          
            oq.AddCriterion(Expression.Eq("ProjectId", this.ProjectInfo.Id));
            if (this.txtMaterialCode.Text != "")
            {
                oq.AddCriterion(Expression.Like("MaterialCode", "%" + txtMaterialCode.Text + "%"));
            }
            if (this.txtBorrowUnit.Text != "")
            {
                oq.AddCriterion(Expression.Like("TheRankName", "%" + txtBorrowUnit.Text + "%"));
            }
            if (this.txtPart.Text != "")
            {
                oq.AddCriterion(Expression.Like("UsedPartName", "%" + txtPart.Text + "%"));
            }
            if (this.txtMaterialName.Text != "")
            {
                oq.AddCriterion(Expression.Like("MaterialName", "%" + txtMaterialName.Text + "%"));
            }
            if (this.txtSpec.Text != "")
            {
                oq.AddCriterion(Expression.Like("MaterialSpec", "%" + txtSpec.Text + "%"));
            }
            list = model.MaterialHireMngSvr.GetMaterialHireLedgerMaster(oq);

            if (list.Count > 0)
            {
                ArrangeList(list);
            }
        }

        private void ArrangeList(IList list)
        {
            IList list_temp = new ArrayList();
            foreach (MatHireLedgerMaster master in list)
            {
                if (master.LeftQuantity == 0)
                    continue;
                if (list_temp.Count == 0)
                {
                    list_temp.Add(master);
                }
                else
                {
                    for (int i = 0; i < list_temp.Count; i++)
                    {
                        MatHireLedgerMaster theMatHireLedgerMaster = list_temp[i] as MatHireLedgerMaster;

                        if (master.MaterialResource == theMatHireLedgerMaster.MaterialResource && master.UsedPart == theMatHireLedgerMaster.UsedPart && master.TheRank == theMatHireLedgerMaster.TheRank)
                        {
                            theMatHireLedgerMaster.LeftQuantity += master.LeftQuantity;
                            break;
                        }
                        else if (i == list_temp.Count - 1)
                        {
                            list_temp.Add(master);
                            break;
                        }
                    }
                }
            }
            if (list_temp.Count > 0)
            {
                RefreshData(list_temp);
            }
        }

        private void RefreshData(IList list)
        {
            foreach (MatHireLedgerMaster master in list)
            {
                int i = this.dgDetail.Rows.Add();
                this.dgDetail[colMaterialCode.Name, i].Tag = master;
                this.dgDetail[colMaterialCode.Name, i].Value = master.MaterialCode;
                this.dgDetail[colMaterialName.Name, i].Value = master.MaterialName;
                this.dgDetail[colSpec.Name, i].Value = master.MaterialSpec;
                this.dgDetail[colStationQuantity.Name, i].Value = master.LeftQuantity;
                this.dgDetail[colUnit.Name, i].Value = master.MatStandardUnitName;
                this.dgDetail[this.colBorrowUnit.Name, i].Value = master.TheRankName;
                this.dgDetail[colPart.Name, i].Tag = master.UsedPart;
                this.dgDetail[colPart.Name, i].Value = master.UsedPartName;
                this.dgDetail[colSubject.Name, i].Tag = master.SubjectGUID;
                this.dgDetail[colSubject.Name, i].Value = master.SubjectName;
            }
        }

        public void OpenSelector()
        {
            this.dgDetail.Rows.Clear();
            this.GetMaterialRenLed();
         
            this.ShowDialog();
        }
    }
}
