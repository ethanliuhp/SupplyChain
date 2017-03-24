using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using System.Collections;
using VirtualMachine.Core;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.PBS
{
    public partial class VFeatureSet : TBasicDataView
    {
        MPBSTree model = new MPBSTree();
        public VFeatureSet()
        {
            InitializeComponent();
            InitForm();
        }
        private void InitForm()
        {
            dgFeatureSet.AllowUserToAddRows = true;
            dgFeature.AllowUserToAddRows = true;
            LoadFeatureSet();
            InitEvent();
        }
        private void InitEvent()
        {
            dgFeatureSet.RowEnter += new DataGridViewCellEventHandler(dgFeatureSet_RowEnter);
            dgFeatureSet.UserRowUpdating += new VirtualMachine.Component.WinControls.Controls.UserRowUpdatingHandle(dgFeatureSet_UserRowUpdating);
            dgFeature.UserRowUpdating += new VirtualMachine.Component.WinControls.Controls.UserRowUpdatingHandle(dgFeature_UserRowUpdating);

            btnSave.Click += new EventHandler(btnSave_Click);
            btnDeleteFeatureSet.Click += new EventHandler(btnDeleteFeatureSet_Click);
            btnDeleteFeature.Click += new EventHandler(btnDeleteFeature_Click);
        }

        void dgFeatureSet_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            FeatureSet set = dgFeatureSet.Rows[e.RowIndex].Tag as FeatureSet;
            if (set != null)
            {
                LoadFeature(set);
            }
            else
            {
                dgFeature.Rows.Clear();
            }
        }

        void dgFeatureSet_UserRowUpdating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dgFeatureSet.Rows[e.RowIndex].IsNewRow) return;
            DataGridViewRow row = dgFeatureSet.Rows[e.RowIndex];
            string name = Convert.ToString(row.Cells[FeatureSetName.Name].Value);
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("特性集名称不能为空！");
                e.Cancel = true;
                return;
            }
            else
            {
                foreach (DataGridViewRow r in dgFeatureSet.Rows)
                {
                    if (dgFeatureSet.Rows[e.RowIndex].IsNewRow) continue;
                    if (r.Index != e.RowIndex)
                    {
                        if (name == Convert.ToString(r.Cells[FeatureSetName.Name].Value))
                        {
                            MessageBox.Show("特性集名称出现重复请核实！");
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }
            try
            {
                if (row.Tag != null)
                {
                    FeatureSet set = row.Tag as FeatureSet;
                    set.Name = name;//Convert.ToString(row.Cells[FeatureSetName.Name].Value);
                    set.Description = Convert.ToString(row.Cells[FeatureSetDes.Name].Value);
                    row.Tag = set;
                }
                else
                {
                    FeatureSet set = new FeatureSet();
                    set.Name = name;//Convert.ToString(row.Cells[FeatureSetName.Name].Value);
                    set.Description = Convert.ToString(row.Cells[FeatureSetDes.Name].Value);
                    row.Tag = set;
                }
            }
            catch (Exception ee)
            {
                e.Cancel = true;
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
            }
            
        }

        void dgFeature_UserRowUpdating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dgFeatureSet.Rows.Count == 0 || dgFeatureSet.SelectedRows[0].Tag == null)
            {
                MessageBox.Show("请先填写特性集！");
                e.Cancel = true;
                if (!dgFeatureSet.Rows[e.RowIndex].IsNewRow)
                {
                    dgFeature.Rows.RemoveAt(e.RowIndex);
                }
                return;
            }
            if (dgFeature.Rows[e.RowIndex].IsNewRow) return;
            FeatureSet set = dgFeatureSet.SelectedRows[0].Tag as FeatureSet;
            DataGridViewRow row = dgFeature.Rows[e.RowIndex];
            string name = Convert.ToString(row.Cells[FeatureName.Name].Value);
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("特性名称不能为空！");
                e.Cancel = true;
                return;
            }
            if (row.Tag != null)
            {
                foreach (DataGridViewRow r in dgFeature.Rows)
                {
                    if (e.RowIndex != r.Index)
                    {
                        if (name == Convert.ToString(r.Cells[FeatureName.Name].Value))
                        {
                            MessageBox.Show("特性名称出现重复，请检查！");
                            e.Cancel = true;
                            return;
                        }
                    }
                }
                Feature f = row.Tag as Feature;
                //foreach (Feature ft in set.Details)
                //{
                //    if (!(f.Id == ft.Id || f.SnapValue == ft.SnapValue))
                //    {
                //        if(name == ft.Name
                        
                //        break;
                //    }
                //}
                f.Name = name;//Convert.ToString(row.Cells[FeatureName.Name].Value);
                f.Description = Convert.ToString(row.Cells[FeatureDes.Name].Value);
                row.Tag = f;
            }
            else
            {
                //VirtualMachine.Component.Util.IFCGuidGenerator genObj = new IFCGuidGenerator();
                Feature f = new Feature();
                f.TheFeatureSet = set;
                f.Name = name;//Convert.ToString(row.Cells[FeatureName.Name].Value);
                f.Description = Convert.ToString(row.Cells[FeatureDes.Name].Value);
                //f.SnapValue = genObj.GeneratorIFCGuid();
                row.Tag = f;
                set.Details.Add(f);
            }
            dgFeatureSet.SelectedRows[0].Tag = set;
        }

        //删除特性
        void btnDeleteFeature_Click(object sender, EventArgs e)
        {
            if (dgFeatureSet.Rows.Count == 0 || dgFeatureSet.SelectedRows[0].Tag == null) return;
            if (dgFeature.Rows.Count == 0 || dgFeature.SelectedRows.Count<=0 || dgFeature.SelectedRows[0].Tag == null) return;
            FeatureSet set = dgFeatureSet.SelectedRows[0].Tag as FeatureSet;
            Feature f = dgFeature.SelectedRows[0].Tag as Feature;
            set.Details.Remove(f);
            if (string.IsNullOrEmpty(f.Id))
            {
                model.Delete(f);
            }
        }
        //删除特性集
        void btnDeleteFeatureSet_Click(object sender, EventArgs e)
        {
            if (dgFeatureSet.Rows.Count > 0 && dgFeatureSet.SelectedRows[0].Tag != null)
            {
                FeatureSet set = dgFeatureSet.SelectedRows[0].Tag as FeatureSet;
                if (!string.IsNullOrEmpty(set.Id))
                {
                    model.Delete(set);
                }
                dgFeatureSet.Rows.Remove(dgFeatureSet.SelectedRows[0]);
            }
        }
        //保存
        void btnSave_Click(object sender, EventArgs e)
        {
            IList list = new ArrayList();
            foreach (DataGridViewRow row in dgFeatureSet.Rows)
            {
                if (row.Tag != null)
                {
                    FeatureSet set = row.Tag as FeatureSet;
                    list.Add(set);
                }
            }
            if (list != null && list.Count > 0)
            {
                model.SaveOrUpdateFeatureSet(list);
                MessageBox.Show("保存成功！");
            }
        }
        void LoadFeature(FeatureSet set)
        {
            dgFeature.Rows.Clear();
            foreach (Feature f in set.Details)
            {
                int index = dgFeature.Rows.Add();
                dgFeature[FeatureName.Name, index].Value = f.Name;
                dgFeature[FeatureDes.Name, index].Value = f.Description;
                dgFeature.Rows[index].Tag = f;
            }
        }
        void LoadFeatureSet()
        {
            dgFeatureSet.Rows.Clear();
            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof(FeatureSet), oq);
            if (list != null && list.Count > 0)
            {
                foreach (FeatureSet s in list)
                {
                    int index = dgFeatureSet.Rows.Add();
                    dgFeatureSet[FeatureSetName.Name, index].Value = s.Name;
                    dgFeatureSet[FeatureSetDes.Name, index].Value = s.Description;
                    dgFeatureSet.Rows[index].Tag = s;
                }
                dgFeatureSet.CurrentCell = dgFeatureSet[1, 0];
            }
        } 
    }
}
