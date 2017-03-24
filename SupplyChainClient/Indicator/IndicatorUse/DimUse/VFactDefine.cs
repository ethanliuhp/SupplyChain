using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    public partial class VFactDefine : Form
    {
        private CubeRegister cubeRegister;
        private MCubeManager model = new MCubeManager();
        
        public VFactDefine(CubeRegister cubeRegister)
        {
            this.cubeRegister = cubeRegister;
            InitializeComponent();
            InitEvent();
        }

        private void InitEvent()
        {
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnModify.Click += new EventHandler(btnModify_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            Load += new EventHandler(VFactDefine_Load);
            dgFactDefine.CellDoubleClick += new DataGridViewCellEventHandler(dgFactDefine_CellDoubleClick);
            btnClose.Click += new EventHandler(btnClose_Click);
        }

        void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        void dgFactDefine_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell cell = dgFactDefine[e.ColumnIndex, e.RowIndex];
            if (cell.OwningColumn.Name == colUnit.Name)
            {
                StandardUnit unit = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                cell.Tag = unit;
                cell.Value = unit.Name;
                dgFactDefine.EndEdit();
            }
        }

        void VFactDefine_Load(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("CubeRegister.Id", cubeRegister.Id));
            try
            {
                IList list = model.CubeManagerSrv.GetFactDefine(oq);
                ShowData(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询事实出错。\n"+ex.Message);
            }
        }

        private void ShowData(IList list)
        {
            dgFactDefine.Rows.Clear();
            if (list != null && list.Count > 0)
            {
                foreach (FactDefine obj in list)
                {
                    int i = dgFactDefine.Rows.Add();
                    dgFactDefine.Rows[i].Tag = obj;
                    dgFactDefine[colFactname.Name, i].Value = obj.FactName;
                    dgFactDefine[colUnit.Name, i].Tag = obj.StandardUnit;
                    dgFactDefine[colUnit.Name, i].Value = obj.StandardUnitName;
                }
            }
        }

        private bool ValidView()
        {
            foreach (DataGridViewRow dr in dgFactDefine.Rows)
            {
                if (dr.IsNewRow) continue;
                object factName = dr.Cells[colFactname.Name].Value;
                if (factName == null || factName.ToString().Equals(""))
                {
                    MessageBox.Show("事实名称不能为空。");
                    return false;
                }
                object unitName = dr.Cells[colUnit.Name].Value;
                if (unitName == null || unitName.ToString().Equals(""))
                {
                    MessageBox.Show("计量单位不能为空,请双击选择。");
                    return false;
                }
            }
            return true;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if(!ValidView()) return;
            IList list = new ArrayList();
            foreach (DataGridViewRow dr in dgFactDefine.Rows)
            {
                if (dr.IsNewRow) continue;
                FactDefine obj = dr.Tag as FactDefine;
                if (obj == null)
                {
                    obj = new FactDefine();
                    obj.CubeRegister = this.cubeRegister;
                }
                obj.FactName = dr.Cells[colFactname.Name].Value.ToString();
                obj.StandardUnit = dr.Cells[colUnit.Name].Tag as StandardUnit;
                obj.StandardUnitName = dr.Cells[colUnit.Name].Value.ToString();
                list.Add(obj);
            }
            try
            {
                IList factDefineList=model.CubeManagerSrv.SaveFactDefine(list);
                ShowData(factDefineList);
                dgFactDefine.EditMode = DataGridViewEditMode.EditProgrammatically;
                //dgFactDefine.EndEdit();
                MessageBox.Show("保存成功。");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存事实出错。\n"+ex.Message);
            }
        }

        void btnModify_Click(object sender, EventArgs e)
        {
            dgFactDefine.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            //dgFactDefine.BeginEdit(false);
        }

        void btnAdd_Click(object sender, EventArgs e)
        {
            dgFactDefine.Rows.Add();
            dgFactDefine.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            //dgFactDefine.BeginEdit(false);
        }
    }
}
