using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using System.Collections;
using VirtualMachine.Component.Util;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.PBS
{
    public partial class VElementFeature : TBasicDataView
    {
        private Elements opElement = null;
        MPBSTree model = new MPBSTree();
        public VElementFeature()
        {
            InitializeComponent();
        }

        public VElementFeature(Elements e)
        {
            InitializeComponent();
            opElement = e;
            InitEvents();
            InitData();
        }
        private void InitData()
        {
            gbFeature.Enabled = false;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", opElement.Id));
            IList list = model.ObjectQuery(typeof(ElementFeature), oq);
            dgElementFeature.Rows.Clear();
            foreach (ElementFeature ef in list)
            {
                int index = dgElementFeature.Rows.Add();
                dgElementFeature[FeatureSet.Name, index].Value = ef.FeatureSet;
                dgElementFeature[FeatureName.Name, index].Value = ef.FeatureName;
                dgElementFeature[FeatureUnit.Name, index].Value = ef.FeatureSet;
                dgElementFeature[FeatureLable.Name, index].Value = ef.Lable;
                dgElementFeature[FeatureValue.Name, index].Value = ef.Value;
                dgElementFeature[FeatureValueFormat.Name, index].Value = ef.ValueFormat;
                dgElementFeature[FeatureDes.Name, index].Value = ef.Description;
                dgElementFeature.Rows[index].Tag = ef;
            }
            if (list != null && list.Count > 0)
            {
                dgElementFeature_SelectionChanged(dgElementFeature, new EventArgs());
            }
        }
        private void InitEvents()
        {
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            dgElementFeature.SelectionChanged += new EventHandler(dgElementFeature_SelectionChanged);
            btnUnit.Click += new EventHandler(btnUnit_Click);
        }
        //选择计量单位
        void btnUnit_Click(object sender, EventArgs e)
        {
            StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
            if (su != null)
            {
                txtFUnit.Tag = su;
                txtFUnit.Text = su.Name;
                txtFUnit.Focus();
            }
        }

        //取消
        void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            gbFeature.Enabled = false;
        }
        //保存
        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool isNew = true;
                ElementFeature feature = new ElementFeature();
                if (txtFName.Tag != null)
                {
                    feature = txtFName.Tag as ElementFeature;
                    isNew = false;
                }
                else
                {
                    feature.Master = opElement;
                }
                feature.FeatureSet = txtFSet.Text;
                feature.FeatureName = txtFName.Text;
                feature.Lable = txtFLable.Text;
                if (txtFUnit.Tag != null)
                {
                    feature.Unit = txtFUnit.Tag as StandardUnit;
                    feature.UnitName = txtFUnit.Text;
                }

                feature.Value = txtFValue.Text;
                feature.ValueFormat = txtFValueFormat.Text;
                feature.Description = txtFDes.Text;

                IList list = new ArrayList();
                list.Add(feature);
                feature = model.SaveOrUpdate(list)[0] as ElementFeature;
                int index = -1;
                if (isNew)
                {
                    index = dgElementFeature.Rows.Add();
                    dgElementFeature_SelectionChanged(dgElementFeature, new EventArgs());
                }
                else
                {
                    index = dgElementFeature.CurrentRow.Index;
                }
                dgElementFeature[FeatureSet.Name, index].Value = feature.FeatureSet;
                dgElementFeature[FeatureName.Name, index].Value = feature.FeatureName;
                dgElementFeature[FeatureUnit.Name, index].Value = feature.FeatureSet;
                dgElementFeature[FeatureLable.Name, index].Value = feature.Lable;
                dgElementFeature[FeatureValue.Name, index].Value = feature.Value;
                dgElementFeature[FeatureValueFormat.Name, index].Value = feature.ValueFormat;
                dgElementFeature[FeatureDes.Name, index].Value = feature.Description;
                dgElementFeature.Rows[index].Tag = feature;
                gbFeature.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存出错：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        //修改
        void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgElementFeature.Rows.Count == 0 || dgElementFeature.CurrentRow == null || dgElementFeature.CurrentRow.Tag == null) return;
            gbFeature.Enabled = true;
            ElementFeature feature = dgElementFeature.CurrentRow.Tag as ElementFeature;
            txtFName.Tag = feature;
        }
        //删除
        void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgElementFeature.Rows.Count == 0 || dgElementFeature.CurrentRow == null || dgElementFeature.CurrentRow.Tag == null) return;
            try
            {
                if (MessageBox.Show("确定删除？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ElementFeature feature = dgElementFeature.CurrentRow.Tag as ElementFeature;
                    model.Delete(feature);
                    dgElementFeature.Rows.Remove(dgElementFeature.CurrentRow);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除出错：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        //新增
        void btnAdd_Click(object sender, EventArgs e)
        {
            Clear();
            gbFeature.Enabled = true;
        }
        void Clear()
        {
            txtFDes.Text = "";
            txtFLable.Text = "";
            txtFName.Text = "";
            txtFName.Tag = null;
            txtFSet.Text = "";
            txtFUnit.Text = "";
            txtFUnit.Tag = null;
            txtFValue.Text = "";
            txtFValueFormat.Text = "";
        }

        void dgElementFeature_SelectionChanged(object sender, EventArgs e)
        {
            gbFeature.Enabled = false;
            if (dgElementFeature.Rows.Count == 0 || dgElementFeature.CurrentRow == null || dgElementFeature.CurrentRow.Tag == null)
            {
                Clear();
                return;
            }
            ElementFeature feature = dgElementFeature.CurrentRow.Tag as ElementFeature;
            txtFDes.Text = feature.Description;
            txtFLable.Text = feature.Lable;
            txtFName.Text = feature.FeatureName;
            txtFName.Tag = feature;
            txtFSet.Text = feature.FeatureSet;
            if (feature.Unit != null)
            {
                txtFUnit.Text = feature.UnitName;
                txtFUnit.Tag = feature.Unit;
            }
            txtFValue.Text = feature.Value;
            txtFValueFormat.Text = feature.ValueFormat;
        }
    }
}
