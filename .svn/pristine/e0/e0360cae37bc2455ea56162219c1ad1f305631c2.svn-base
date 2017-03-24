using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;
using Application.Business.Erp.SupplyChain.Util;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    public partial class VCubeDefine : TBasicDataView
    {
        private MCubeManager mCube = new MCubeManager();
        public VCubeDefine()
        {
            InitializeComponent();
            btnFactDefine.Click += new EventHandler(btnFactDefine_Click);
        }

        void btnFactDefine_Click(object sender, EventArgs e)
        {
            CubeRegister cr = lstCubeSel.SelectedItem as CubeRegister;
            if (cr == null)
            {
                MessageBox.Show("请先选择一个主题。");
                return;
            }
            VFactDefine vfd = new VFactDefine(cr);
            vfd.ShowDialog();
        }

        internal void Start()
        {
            LoadCubeList();
            LoadDimensionList();
        }

        private void LoadDimensionList()
        {
            IList list = mCube.DimManagerSrv.GetDimensionRegisterByRights(ConstObject.TheSystemCode+"");
            lstSourDim.DisplayMember = "Name";
            if( list.Count > 0 ){
                foreach (DimensionRegister res in list)
                {
                    lstSourDim.Items.Add(res);
                }
            }
        }

        private void LoadCubeList()
        {
            SystemRegister sr = new SystemRegister();
            sr.Id = ConstObject.TheSystemCode.ToString();
            IList list = mCube.CubeManagerSrv.GetPartSystemCubeRegister(sr);
            lstCubeSel.DisplayMember = "CubeName";
            if (list.Count > 0)
            {
                foreach (CubeRegister res in list)
                {
                    lstCubeSel.Items.Add(res);
                }
            }
        }

        private void lstCubeSel_MouseClick(object sender, MouseEventArgs e)
        {
            ClearView();
            if( lstCubeSel.SelectedIndex != -1 ){
                CubeRegister reg = lstCubeSel.SelectedItem as CubeRegister;
                edtCubeName.Text = reg.CubeName;
                edtCubeName.Tag = reg;
                IList list = mCube.CubeManagerSrv.GetCubeAttrByCubeResgisterId(reg);
                lstTarDim.DisplayMember = "DimensionName";
                foreach (CubeAttribute attr in list)
                {
                    lstTarDim.Items.Add(attr);
                }

                if (reg.Id == "1")
                {
                    btnRemove.Enabled = false;
                    btnAdd.Enabled = false;
                    btnDel.Enabled = false;
                    btnSubmit.Enabled = false;

                }
                else {
                    btnRemove.Enabled = true;
                    btnAdd.Enabled = true;
                    btnDel.Enabled = true;
                    btnSubmit.Enabled = true;
                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            CubeRegister reg = new CubeRegister();
            if(edtCubeName.Tag != null)
            {
                reg = edtCubeName.Tag as CubeRegister;
            }
            if (KnowledgeUtil.isEmpty(edtCubeName.Text))
            {
                MessageBox.Show("主题名称不能为空！");
                edtCubeName.Focus();
                return;
            }
            if ( lstTarDim.Items.Count == 0 )
            {
                MessageBox.Show("目标属性的选择不能为空！");
                lstTarDim.Focus();
                return;
            }
            try
            {
                reg.CubeName = edtCubeName.Text;
                SystemRegister sr = mCube.CubeManagerSrv.GetSystemRegisterById(ConstObject.TheSystemCode);
                reg.SysRegister = sr;
                reg = mCube.CubeManagerSrv.SaveCubeRegister(reg);//保存主题信息
                //保存主题属性信息（先删除再新增）
                IList old_list = mCube.CubeManagerSrv.GetCubeAttrByCubeResgisterId(reg);
                foreach (CubeAttribute old_attr in old_list)
                {
                    mCube.CubeManagerSrv.DeleteCubeAttribute(old_attr);
                }
                //删除主题数据表
                mCube.CubeManagerSrv.CallDynamicDelCubeData(reg);
                //新增属性信息
                IList new_list = lstTarDim.Items;
                foreach (CubeAttribute new_attr in new_list)
                {
                    new_attr.Id ="";
                    new_attr.Version = -1;
                    new_attr.CubeRegis = reg;
                    mCube.CubeManagerSrv.SaveCubeAttribute(new_attr);
                }

                //调用动态生成主题数据表
                mCube.CubeManagerSrv.CallDynamicCreateCubeData(reg);

                lstCubeSel.Items.Clear();
                LoadCubeList();
            }
            catch (Exception ex)
            {
                KnowledgeMessageBox.InforMessage("主题定义出错：", ex);
                return;
            }
            MessageBox.Show("主题定义成功！");
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            ClearView();
            btnAdd.Enabled = true;
            btnDel.Enabled = true;
            btnSubmit.Enabled = true;

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (edtCubeName.Tag != null)
            {
                if (DialogResult.Yes == KnowledgeMessageBox.QuestionMessage("确定要删除当前主题吗？"))
                {
                    try
                    {
                        CubeRegister obj = edtCubeName.Tag as CubeRegister;
                        mCube.CubeManagerSrv.DeleteCubeRegister(obj);
                        lstCubeSel.Items.Clear();
                        LoadCubeList();
                        ClearView();
                    }
                    catch (Exception ex)
                    {
                        string exception = ex.ToString();
                        if (exception.IndexOf("违反完整约束条件") > 0)
                        {
                            KnowledgeMessageBox.InforMessage("删除主题出错:该主题已存在视图，不允许删除！");
                        }
                        else {
                            KnowledgeMessageBox.InforMessage("删除主题出错", ex);
                        }
                    }
                }             
            }
            else {
                MessageBox.Show("请先选择要删除的主题！");
            }
        }

        private void ClearView(){
            edtCubeName.Tag = null;
            edtCubeName.Text = null;
            lstTarDim.Items.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (lstSourDim.SelectedIndex != -1)
            {
                //取得现有的目标框的维度名称的集合
                ArrayList al_name = new ArrayList();
                foreach (CubeAttribute obj in lstTarDim.Items)
                {
                    al_name.Add(obj.DimensionName);
                }

                lstTarDim.DisplayMember = "DimensionName";
                DimensionRegister dimReg = lstSourDim.SelectedItem as DimensionRegister;
                string selectedItem = dimReg.Name;
                if (al_name.Contains(selectedItem))
                {
                    MessageBox.Show("目标列表已经包括此属性！");
                }
                else
                {
                    CubeAttribute attr = new CubeAttribute();
                    attr.CubeRegis = edtCubeName.Tag as CubeRegister;
                    attr.DimensionId = dimReg.Id;
                    attr.DimensionName = dimReg.Name;
                    attr.DimensionCode = dimReg.Code;
                    lstTarDim.Items.Add(attr);
                }
            }
            else {
                MessageBox.Show("请先选择源列表要增加的属性！");
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (lstTarDim.SelectedIndex != -1)
            {
                lstTarDim.Items.Remove(lstTarDim.SelectedItem);
            }
            else
            {
                MessageBox.Show("请先选择目标列表要移除的属性！");
            }
        }

    }
}