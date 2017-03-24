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
    public partial class VViewDefReport : TBasicDataView
    {
        private MIndicatorUse model = new MIndicatorUse();
        private MBasicData bdModel = new MBasicData();

        public VViewDefReport()
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
            cboType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboType.Enabled = false;
            cboType.SelectedValue = "4";
        }

        private void EnableControls(bool enabled)
        {
            txtViewName.Enabled = enabled;
            btnSave.Enabled = enabled;
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
            model.InitialIndicatorCombox(KnowledgeUtil.ViewTypeCode, KnowledgeUtil.ViewTypeName, cboType, true);
        }

        private void InitialEvents()
        {
            cboCube.SelectedIndexChanged += new EventHandler(cboCube_SelectedIndexChanged);
            lstViewSel.SelectedIndexChanged += new EventHandler(lstViewSel_SelectedIndexChanged);
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnModify.Click += new EventHandler(btnModify_Click);
            btnDel.Click += new EventHandler(btnDel_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnStyle.Click += new EventHandler(btnStyle_Click);
        }

        /// <summary>
        /// 模板样式定义
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnStyle_Click(object sender, EventArgs e)
        {
            ViewMain obj = txtViewName.Tag as ViewMain;
            if (obj == null)
            {
                MessageBox.Show("请先定义统计模板！");
                return;
            }
            VReportDefine vrd = new VReportDefine();
            vrd.vm = obj;
            vrd.ShowDialog();            
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            if (CheckInput())
            {
                bool isNew = false;//判断是否是新增模板还是修改的模板
                
                ViewMain obj = txtViewName.Tag as ViewMain;
                if (obj == null) 
                {
                    obj = new ViewMain();
                }

                obj.ViewName = txtViewName.Text;
                obj.ViewTypeCode = cboType.SelectedValue.ToString();
                obj.ViewTypeName = cboType.Text;

                if (string.IsNullOrEmpty(obj.Id))
                {
                    isNew = true;

                    CubeRegister cubeReg = cboCube.SelectedItem as CubeRegister;
                    obj.CubeRegId = cubeReg;
                    obj.SystemId = ConstObject.TheSystemCode.ToString();

                    obj.CreatedDate = DateTime.Today;
                    obj.Author = ConstObject.LoginPersonInfo;
                    
                    obj.TheJob = ConstObject.TheSysRole;
                    obj.TheOperOrg = ConstObject.TheOperationOrg;
                }

                try
                {
                    //保存模板主表　
                    obj=model.ViewSrv.SaveViewMain(obj);
                    KnowledgeMessageBox.InforMessage("保存成功。");
                    if (isNew)
                    {
                        lstViewSel.Items.Add(obj);
                        lstViewSel.SelectedItem = obj;
                    }
                    else 
                    {
                        int curInx = lstViewSel.SelectedIndex;
                        lstViewSel.Items.Remove(obj);
                        lstViewSel.Items.Insert(curInx, obj);
                        lstViewSel.SelectedIndex = curInx;
                    }

                    EnableControls(false);
                }
                catch (Exception ex)
                {
                    KnowledgeMessageBox.InforMessage("保存模板出错。", ex);
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnDel_Click(object sender, EventArgs e)
        {
            ViewMain obj = lstViewSel.SelectedItem as ViewMain;
            if (obj == null)
            {
                KnowledgeMessageBox.InforMessage("请先选择一个模板。");
                return;
            }
            if (DialogResult.Yes == KnowledgeMessageBox.QuestionMessage("确定要删除当前模板吗？"))
            {
                try
                {
                    lstViewSel.Items.Remove(obj);
                    model.ViewSrv.DeleteViewMain(obj);
                    if (lstViewSel.Items.Count > 0)
                    {
                        lstViewSel.SelectedIndex = 0;
                    }
                    else
                    {
                        Clear();
                    }
                }
                catch (Exception ex)
                {
                    string exception = ex.ToString();
                    KnowledgeMessageBox.InforMessage("删除模板出错。", ex);                
                    lstViewSel.Items.Add(obj);
                    lstViewSel.SelectedItem = obj;
                }
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnModify_Click(object sender, EventArgs e)
        {
            ViewMain obj = lstViewSel.SelectedItem as ViewMain;
            if (obj == null)
            {
                KnowledgeMessageBox.InforMessage("请先选择一个模板。");
                return;
            }
            EnableControls(true);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAdd_Click(object sender, EventArgs e)
        {
            CubeRegister cubeReg = cboCube.SelectedItem as CubeRegister;
            if (cubeReg == null)
            {
                KnowledgeMessageBox.InforMessage("请先选择一个主题。");
                return;
            }

            EnableControls(true);

            //模板名称
            txtViewName.ReadOnly = false;
            txtViewName.Text = "";
            txtViewName.Tag = null;
            txtViewName.Focus();

            cboType.SelectedValue = "4";
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

        private void Clear()
        {
            txtViewName.Tag = null;
            txtViewName.Text = "";

            cboType.SelectedIndex = 0;
        }

        void lstViewSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewMain obj = lstViewSel.SelectedItem as ViewMain;
            if (obj == null) return;
            txtViewName.Text = obj.ViewName;
            txtViewName.Tag = obj;

            if (obj.ViewTypeCode != null)
            {
                cboType.SelectedValue = obj.ViewTypeCode;
            }

            CubeRegister cubeReg = cboCube.SelectedItem as CubeRegister;
            if (cubeReg == null)
            {
                KnowledgeMessageBox.InforMessage("请先选择一个主题。");
            }

            EnableControls(false);
        }

        void cboCube_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CubeRegister cubeReg = cboCube.SelectedItem as CubeRegister;
                if (cubeReg == null) return;

                //添加模板
                IList list = model.ViewSrv.GetViewMainByCubeIdAndType(cubeReg,"4");
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
                else
                {
                    Clear();
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


        private void customButton1_Click(object sender, EventArgs e)
        {
            VDimensionRegister vdr = new VDimensionRegister();
            vdr.ShowDialog();
        }
    }
}