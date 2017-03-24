using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Core;
using System.Collections;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalLedgerMng.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalLedgerMng
{
    public partial class VMaterialRenLedSelector : TBasicDataView
    {
        private IList result = new ArrayList();
        //料具出租方
        private SupplierRelationInfo TheSupplier;
        CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
        //队伍
        private SupplierRelationInfo TheRank;
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }
        MMatRentalMng model = new MMatRentalMng();
        public VMaterialRenLedSelector()
        {
            InitializeComponent();
            InitEvent();
        }

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
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
                    MaterialRentalLedger master = var.Cells[colMaterialCode.Name].Tag as MaterialRentalLedger;
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
            oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", TheSupplier));//料具出租方
            if (TheRank != null)
            {
                oq.AddCriterion(Expression.Eq("TheRank", TheRank));
            }
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
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
            list = model.MatMngSrv.GetMaterialRentalLedger(oq);

            if (list.Count > 0)
            {
                ArrangeList(list);
            }
        }

        private void ArrangeList(IList list)
        {
            IList list_temp = new ArrayList();
            foreach (MaterialRentalLedger master in list)
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
                        MaterialRentalLedger theMaterialRentalLedger = list_temp[i] as MaterialRentalLedger;

                        if (master.MaterialResource == theMaterialRentalLedger.MaterialResource && master.UsedPart == theMaterialRentalLedger.UsedPart && master.TheRank == theMaterialRentalLedger.TheRank)
                        {
                            theMaterialRentalLedger.LeftQuantity += master.LeftQuantity;
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
            foreach (MaterialRentalLedger master in list)
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

        public void OpenSelector(SupplierRelationInfo theSupplier, SupplierRelationInfo theRank)
        {
            this.dgDetail.Rows.Clear();
            this.GetMaterialRenLed();
            this.TheSupplier = theSupplier;
            this.TheRank = theRank;
            this.ShowDialog();
        }
    }
}
