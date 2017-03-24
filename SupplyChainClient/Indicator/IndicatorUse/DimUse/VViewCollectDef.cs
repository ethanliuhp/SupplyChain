using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.BasicData;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

using Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.Client.Indicator.BasicData;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse
{
    public partial class VViewCollectDef : TBasicDataView
    {
        private MIndicatorUse model = new MIndicatorUse();
        private MBasicData bdModel = new MBasicData();

        public VViewCollectDef()
        {
            InitializeComponent();
        }

        internal void Start()
        {
            InitialEvents();
            InitialBasicdata();
            InitialControls();
            EnableControls(false);
            GetCube();
        }

        /// <summary>
        /// 设置控件的一些属性
        /// </summary>
        private void InitialControls()
        {
            cboCube.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCollectRule.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCollectType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCollectRule.SelectedValue = "1";
            cboCollectRule.Enabled = false;
        }

        private void EnableControls(bool enabled)
        {
            txtViewName.Enabled = enabled;
            cboCollectType.Enabled = enabled;
        }

        /// <summary>
        /// 获取主题
        /// </summary>
        private void GetCube()
        {
            try
            {
                SystemRegister sr = new SystemRegister();
                sr.Id = ConstObject.TheSystemCode.ToString();
                IList list = model.CubeSrv.GetPartSystemCubeRegister(sr);
                foreach (CubeRegister obj in list)
                {
                    cboCube.Items.Add(obj);
                }
                cboCube.DisplayMember = "CubeName";

                if (cboCube.Items.Count > 0)
                {
                    cboCube.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                KnowledgeMessageBox.InforMessage("查找主题出错。",ex);
            }
        }

        /// <summary>
        /// 初始化基础数据
        /// </summary>
        private void InitialBasicdata()
        {
            model.InitialIndicatorCombox(KnowledgeUtil.CalculateTypeCode, KnowledgeUtil.CalculateTypeName, cboCollectRule, true);
            model.InitialIndicatorCombox(KnowledgeUtil.CollectTypeCode, KnowledgeUtil.CollectTypeName, cboCollectType, true);
        }

        private void InitialEvents()
        {
            cboCube.SelectedIndexChanged += new EventHandler(cboCube_SelectedIndexChanged);
            lstViewSel.SelectedIndexChanged += new EventHandler(lstViewSel_SelectedIndexChanged);
            btnSave.Click += new EventHandler(btnSave_Click);
        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            ViewMain vm = lstViewSel.SelectedItem as ViewMain;
            string ywzz_str = "_";
            foreach (DataGridViewRow row in dgvYwzz.Rows)
            {
                ViewStyleDimension vsd = row.Tag as ViewStyleDimension;
                object ifSelect = row.Cells["ifselect"].Value;
                if (ifSelect == null || int.Parse(ifSelect.ToString()) != 1) continue;

                ywzz_str += vsd.DimCatId + "_";
            }
            vm.CollectYwzz = ywzz_str;
            model.ViewSrv.SaveViewMain(vm);
            MessageBox.Show("保存数据完成！");
        }

        /// <summary>
        /// 检查输入
        /// </summary>
        /// <returns></returns>
        private bool CheckInput()
        {
            CubeRegister cubeReg = cboCube.SelectedItem as CubeRegister;
            if (cubeReg == null)
            {
                KnowledgeMessageBox.InforMessage("请选择一个主题。");
                return false;
            }
            
            if (txtViewName.Text == null || txtViewName.Text.Trim().Equals(""))
            {
                KnowledgeMessageBox.InforMessage("请输入模板名称。");
                txtViewName.Focus();
                return false;
            }

            return true;
        }


        void lstViewSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewMain obj = lstViewSel.SelectedItem as ViewMain;
            if (obj == null) return;
            txtViewName.Text = obj.ViewName;
            txtViewName.Tag = obj;

            cboCollectRule.SelectedValue = "1";

            if (obj.CollectTypeCode != null)
            {
                cboCollectType.SelectedValue = obj.CollectTypeCode;
            }

            CubeRegister cubeReg = cboCube.SelectedItem as CubeRegister;
            if (cubeReg == null)
            {
                KnowledgeMessageBox.InforMessage("请先选择一个主题。");
            }

            //初始化业务部门的选择列表
            string collectYwzz = obj.CollectYwzz;
            IList style_list = obj.ViewStyles;
            ViewStyle vs_ywzz = new ViewStyle();
            foreach (ViewStyle vs in style_list)
            {
                if (vs.OldCatRootName.IndexOf("业务组织") != -1) {
                    vs_ywzz = vs;
                    break;
                }
            }
            IList styleMx_list = vs_ywzz.Details;
            if (styleMx_list.Count > 0)
            {
                dgvYwzz.Rows.Clear();
                foreach (ViewStyleDimension vsd in styleMx_list)
                {
                    int i = dgvYwzz.Rows.Add();
                    DataGridViewRow row = dgvYwzz.Rows[i];
                    row.Tag = vsd;
                    if (collectYwzz.IndexOf("_" + vsd.DimCatId + "_") != -1)
                    {
                        row.Cells["ifselect"].Value = 1;
                    }
                    else {
                        row.Cells["ifselect"].Value = 0;
                    }
                    row.Cells["ywzz"].Value = vsd.Name;
                   
                }
            }

        }

        void cboCube_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CubeRegister cubeReg = cboCube.SelectedItem as CubeRegister;
                if (cubeReg == null) return;

                //添加模板
                IList list = model.ViewSrv.GetViewMainByCubeIdAndType(cubeReg,"3");
                lstViewSel.Items.Clear();
                foreach (ViewMain obj in list)
                {
                    lstViewSel.Items.Add(obj);
                }
                lstViewSel.DisplayMember = "ViewName";

                if (lstViewSel.Items.Count > 0)
                {
                    lstViewSel.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                KnowledgeMessageBox.InforMessage("查找模板出错。", ex);
            }
        }

        public override void RefreshState(MainViewState state)
        {
            base.RefreshState(state);
            switch (state)
            { 
                case MainViewState.Initialize:
                    ToolMenu.LockItem(ToolMenuItem.AddNew);
                    break;
            }
        }
       

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

    }
}