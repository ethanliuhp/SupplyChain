﻿using System;
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
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection
{
    public partial class VMaterialHireSelector : TBasicDataView
    {
        MMaterialHireMng model = new MMaterialHireMng();

        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VMaterialHireSelector(SupplierRelationInfo Supplier)
        {
            InitializeComponent();
            InitData(Supplier);
            InitEvent();
        }

        private void InitData(SupplierRelationInfo Supplier)
        {
            //根据料具供应商查询合同
            CurrentProjectInfo ProjectInfo = StaticMethod.GetProjectInfo();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));
            oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", Supplier));
            IList lst = model.MaterialHireMngSvr.GetMaterialHireOrder(oq) as IList;
            MatHireOrderMaster theMaterialRentalOrderMaster = new MatHireOrderMaster();
            if (lst.Count > 0)
            {
                theMaterialRentalOrderMaster = (model.MaterialHireMngSvr.GetMaterialHireOrder(oq) as IList)[0] as MatHireOrderMaster;
                foreach (MatHireOrderDetail theMaterialRentalOrderDetail in theMaterialRentalOrderMaster.Details)
                {
                    int index = grdMaterial.Rows.Add();

                    grdMaterial[colMaterialCode.Name, index].Tag = theMaterialRentalOrderDetail.MaterialResource;
                    grdMaterial[colMaterialCode.Name, index].Value = theMaterialRentalOrderDetail.MaterialResource.Code;
                    grdMaterial[colMaterialName.Name, index].Value = theMaterialRentalOrderDetail.MaterialResource.Name;
                    grdMaterial[colSpec.Name, index].Value = theMaterialRentalOrderDetail.MaterialResource.Specification;
                    grdMaterial[colUnit.Name, index].Tag = theMaterialRentalOrderDetail.MatStandardUnit;
                    grdMaterial[colUnit.Name, index].Value = theMaterialRentalOrderDetail.MatStandardUnit.Name;
                    grdMaterial[colPrice.Name, index].Value = theMaterialRentalOrderDetail.Price;
                    grdMaterial.Rows[index].Tag = theMaterialRentalOrderDetail;
                }
            }
        }
        private void InitEvent()
        {
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnCancel_Click(object sender, EventArgs e)
        {
            btnCancel.FindForm().Close();
        }
        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnOK_Click(object sender, EventArgs e)
        {
            result.Clear();
            foreach (DataGridViewRow var in this.grdMaterial.Rows)
            {
                if (var.IsNewRow) break;
                if (var.Cells[colSelect.Name].Value != null)
                {
                    if ((bool)var.Cells[colSelect.Name].Value)
                    {
                        MatHireOrderDetail dtl = var.Tag as MatHireOrderDetail;
                        result.Add(dtl);
                    }
                }
            }
            this.btnOK.FindForm().Close();
        }

         
    }
}
