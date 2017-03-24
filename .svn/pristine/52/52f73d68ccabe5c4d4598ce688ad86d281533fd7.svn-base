using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

using Application.Business.Erp.SupplyChain.Client.BasicData;
using Application.Business.Erp.SupplyChain.Util;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Indicator.BasicData;
using Application.Business.Erp.SupplyChain.Client.Basic;


namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimDefine
{
    public partial class VDimensionDefine : TBasicDataView
    {
        private MIndicatorUse model = new MIndicatorUse();
        private MBasicData basicModel = new MBasicData();
        IList priDimList = new ArrayList();
        string dimType = "";

        /// <summary>
        /// 计算表达式分隔符
        /// </summary>
        public const char CalExpSplit = '@';

        public VDimensionDefine()
        {
            InitializeComponent();
            //priDimList = model.DimDefSrv.GetDimensionRegisterByRights(ConstObject.TheSystemCode+"");
        }

        
        #region 初始化数据
        internal void Start()
        {
            LoadIndicatorCategoryTree();
            InitialBasicdata();
            InitialControls();
            InitialEvents();
            btnScope.Visible = false;
        }

        private void InitialEvents()
        {
            ///分类树事件
            TVCategory.AfterSelect += new TreeViewEventHandler(TVCategory_AfterSelect);

            //按钮事件
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnModify.Click += new EventHandler(btnModify_Click);
            btnDel.Click += new EventHandler(btnDel_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnGenCalExp.Click += new EventHandler(btnGenCalExp_Click);
        }

        private void InitialBasicdata()
        {
            model.InitialIndicatorCombox(KnowledgeUtil.CalculateTypeCode,KnowledgeUtil.CalculateTypeName,cboCalType, false);
            VBasicDataOptr.InitIndicatorUnit(cboUnit, true);
        }        

        /// <summary>
        /// 加载维度树
        /// </summary>
        [TransManager]
        private void LoadIndicatorCategoryTree()
        {
            try
            {
                Hashtable table = new Hashtable();
                TVCategory.Nodes.Clear();
                IList list = model.DimDefSrv.GetPartDimensionCategorys(ConstObject.TheSystemCode.ToString());
                foreach (DimensionCategory cat in list)
                {
                    TreeNode tmpNode = new TreeNode();
                    tmpNode.Name = cat.Id.ToString();
                    tmpNode.Text = cat.Name;
                    tmpNode.Tag = cat;
                    if (cat.ParentNode != null)
                    {
                        TreeNode node = table[cat.ParentNode.Id.ToString()] as TreeNode;
                        node.Nodes.Add(tmpNode);
                        //tmpNode.ContextMenuStrip = this.contextMenuStripCategory;
                    }
                    else
                    {
                        TVCategory.Nodes.Add(tmpNode);
                    }
                    table.Add(tmpNode.Name, tmpNode);
                }
                //选中顶级节点，并且展开一级子节点
                if (list.Count > 0)
                {
                    TVCategory.SelectedNode = TVCategory.Nodes[0];
                    TVCategory.SelectedNode.Expand();
                }
            }
            catch (Exception ex)
            {
                KnowledgeMessageBox.InforMessage("装载维度分类树出错。", ex);
            }
        }

        #endregion

        #region 界面事件的响应
        private void TVCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            txtDimName.ReadOnly = true;
            txtCalExp.ReadOnly = true;
            txtRemark.ReadOnly = true;
            cboCalType.ReadOnly = true;
            cboUnit.ReadOnly = true;
            btnGenCalExp.Enabled = false;

            TreeNode node = e.Node;
            if (node == null) return;
            DimensionCategory category = node.Tag as DimensionCategory;
            dimType = this.GetSelectDimType(category);

            if (category == null) return;

            if (node.Level == 0 || "1".Equals(dimType))
            {
                btnAdd.Enabled = true;
                btnDel.Enabled = false;
                btnModify.Enabled = false;
                btnSave.Enabled = true;
            }
            else if (node.Level == 1)
            {
                btnAdd.Enabled = true;
                btnDel.Enabled = false;
                btnModify.Enabled = false;
                btnSave.Enabled = true;
            }
            else
            {
                btnAdd.Enabled = true;
                btnDel.Enabled = true;
                btnModify.Enabled = true;
                btnSave.Enabled = true;
            }

            if (category != null)
            {
                LoadDimensions(category);
            }
        }

        //构造表达式的选择按钮
        void btnGenCalExp_Click(object sender, EventArgs e)
        {
            TreeNode node = TVCategory.SelectedNode;
            DimensionCategory cat = node.Tag as DimensionCategory;
            VCalExpressionDefine vced = new VCalExpressionDefine();
            vced.dimSysCode = cat.SysCode;
            vced.calExp = txtCalExp.Text + VDimensionDefine.CalExpSplit + txtCalExp.Tag;
            vced.ShowDialog();
            if (vced.isOkClicked)
            {
                string[] calExp = vced.calExp.Split(VDimensionDefine.CalExpSplit);
                txtCalExp.Text = calExp[0];
                txtCalExp.Tag = calExp[1];
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            bool isNew = false;
            DimensionCategory cat = txtDimName.Tag as DimensionCategory;
            if (cat == null)
            {
                KnowledgeMessageBox.InforMessage("请先选择一个维度。");
                return;
            }

            DimensionCategory parentCat = TVCategory.SelectedNode.Tag as DimensionCategory;
            if (parentCat == null)
            {
                KnowledgeMessageBox.InforMessage("请选择一个父节点。");
                return;
            }

            if (string.IsNullOrEmpty(cat.Id))
            {
                cat.ParentNode = parentCat;
                isNew = true;
            }

            if (Check())
            {
                cat.Name = txtDimName.Text;
                cat.Code = cat.Name;
                //cat.Factor=decimal.Parse(txtFactor.Text);

                //计算表达式
                cat.CalExpression = txtCalExp.Text;
                cat.CalExpression += VDimensionDefine.CalExpSplit.ToString();
                if (txtCalExp.Tag != null)
                {
                    cat.CalExpression += txtCalExp.Tag.ToString();
                }

                if (cboCalType.SelectedValue != null)
                {
                    cat.CalTypeCode = cboCalType.SelectedValue.ToString();
                    if ("".Equals(cboCalType.SelectedValue.ToString()))
                    {
                        cat.CalTypeName = "";
                    }
                    else {
                        cat.CalTypeName = cboCalType.Text;
                    }                    
                }
                //计量单位
                if (cboUnit.SelectedValue != null && !"".Equals(cboUnit.SelectedValue.ToString()))
                {
                    cat.DimUnit = cboUnit.SelectedValue.ToString();
                    cat.DimUnitName = cboUnit.Text;
                }
                else {
                    cat.DimUnit = "";
                    cat.DimUnitName = "";
                }

                cat.Code = txtAlias.Text;
                cat.Describe = txtRemark.Text;

                try
                {
                    cat = model.DimDefSrv.SaveDimensionCategory(cat);
                    KnowledgeMessageBox.InforMessage("保存成功。");
                    txtDimName.Tag = cat;

                    if (isNew)
                    {
                        TreeNode node = new TreeNode();
                        node.Name = cat.Id.ToString();
                        node.Text = cat.Name;
                        node.Tag = cat;

                        TVCategory.SelectedNode.Nodes.Add(node);
                        TVCategory.SelectedNode = node;
                    }
                    else
                    {
                        TVCategory.SelectedNode.Tag = cat;
                        TVCategory.SelectedNode.Text = cat.Name;
                    }
                    InitialControls();
                }
                catch (Exception ex)
                {
                    KnowledgeMessageBox.InforMessage("保存维度出错。", ex);
                }
            }            
        }

        void btnDel_Click(object sender, EventArgs e)
        {
            DeleteChildNodes();
        }

        void btnModify_Click(object sender, EventArgs e)
        {
            DimensionCategory cat = TVCategory.SelectedNode.Tag as DimensionCategory;
            if (cat == null)
            {
                KnowledgeMessageBox.InforMessage("请先选择一个维度。");
                return;
            }
            this.ControlElements(dimType);
        }

        void btnAdd_Click(object sender, EventArgs e)
        {
            btnGenCalExp.Enabled = true;
            AddCategory();
        }

        /// <summary>
        /// 添加维度（分类）
        /// </summary>
        private void AddCategory()
        {            
            DimensionCategory newCat = new DimensionCategory();
            this.ControlElements(dimType);
            btnScope.Enabled = false;
            txtDimName.Tag = newCat;
            txtAlias.Text = "";
            txtDimName.Text = "";
            txtCalExp.Text = "";
            txtCalExp.Tag = "";
            //txtFactor.Text = "0";
            txtRemark.Text = "";
            cboCalType.SelectedIndex = 0;
            cboUnit.SelectedIndex = 0;

            txtDimName.Focus();

            TreeNode node = TVCategory.SelectedNode;
            DimensionCategory cat = node.Tag as DimensionCategory;
            if (node == null || cat == null)
            {
                KnowledgeMessageBox.InforMessage("请先选择一个分类。");
                return;
            }

            if (!string.IsNullOrEmpty(cat.DimRegId))
            {
                //外部来源
                IList resourceList = new ArrayList();
                if (!string.IsNullOrEmpty(cat.DimRegId))
                {
                    DimensionRegister dimReg = model.DimDefSrv.GetDimensionRegisterById(cat.DimRegId);
                    if (dimReg == null) return;
                    if (dimReg.Name.Equals("业务组织"))
                    {
                        CommonOrg org = new CommonOrg();
                        org.ObjectType = ORGType.OperationOrg;
                        org.OpenSelect();
                        resourceList = org.Result;
                        if (resourceList == null || resourceList.Count == 0) return;
                        foreach (OperationOrgInfo orgInfo in resourceList)
                        {
                            txtDimName.Text = orgInfo.Name;
                            txtDimName.ReadOnly = true;
                            newCat.DimRegId = cat.DimRegId;
                            newCat.ResourceId = orgInfo.Id;
                            btnSave.Focus();
                            break;
                        }
                    }
                    else if (dimReg.Name.Equals("物料资源"))
                    {
                        CommonMaterial c = new CommonMaterial();
                        c.ObjectType = MaterialSelectType.MaterialCatView;
                        c.OpenSelect();
                        resourceList = c.Result;
                        if (resourceList == null || resourceList.Count == 0) return;
                        foreach (MaterialCategory obj in resourceList)
                        {
                            txtDimName.Text = obj.Name;
                            txtDimName.ReadOnly = true;
                            newCat.DimRegId = cat.DimRegId;
                            newCat.ResourceId = obj.Id;
                            btnSave.Focus();
                            break;
                        }
                    }
                }
                else
                {
                }
            }
        }

        /// <summary>
        /// 删除下级类别
        /// </summary>
        private void DeleteChildNodes()
        {
            TreeNode node = TVCategory.SelectedNode;
            if (node == null)
            {
                KnowledgeMessageBox.InforMessage("请先选择一个分类。");
                return;
            }
            if (node != null)
            {
                DialogResult dr = KnowledgeMessageBox.QuestionMessage("确定要删除当前节点及其所有子节点吗？");
                if (dr == DialogResult.Yes)
                {
                    DimensionCategory category = node.Tag as DimensionCategory;
                    bool canRemove = true;
                    try
                    {
                        string syscode = category.ParentNode.SysCode;
                        string parentId = category.ParentNode.Id + "";
                        model.DimDefSrv.DeleteDimensionCategory(category);

                        //当此节点的父节点不存在子节点时,修改其父节点的nodetype为叶节点为"2"
                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Like("SysCode", syscode + "%"));
                        IList list = model.DimDefSrv.GetDimensionCategoryByQuery(oq);
                        if (list.Count == 1)
                        {
                            model.DimDefSrv.UpdateDimNodeType(parentId, 2);
                        }
                    }
                    catch (Exception ex)
                    {
                        string exception = ex.ToString();
                        if (exception.IndexOf("FK_STYLE_DIM") > 0)
                        {
                            KnowledgeMessageBox.InforMessage("该维度值已经被后续模板所引用，不允许删除！");
                        }
                        else
                        {
                            KnowledgeMessageBox.InforMessage("删除节点出错。", ex);
                        }
                        canRemove = false;
                    }
                    if (canRemove)
                    {
                        node.Remove();
                    }
                }
            }
        }

        void btnScope_Click(object sender, EventArgs e)
        {
            DimensionCategory catgory = txtDimName.Tag as DimensionCategory;
            VDimScopeDefine vScopeDefine = new VDimScopeDefine(catgory);
            vScopeDefine.Open(pnlFloor.FindForm(), ref catgory);
        }
        #endregion

        #region 界面元素可读写控制

        private void InitialControls()
        {
            txtDimName.ReadOnly = true;
            //txtFactor.ReadOnly = true;
            txtCalExp.ReadOnly = true;
            txtRemark.ReadOnly = true;
            txtAlias.ReadOnly = true;
            cboCalType.ReadOnly = true;
            cboUnit.ReadOnly = true;

            btnGenCalExp.Enabled = false;
            btnScope.Enabled = true;
            btnSave.Enabled = true;
            btnDel.Enabled = true;
            btnModify.Enabled = true;
            btnAdd.Enabled = true;
        }

        /// <summary>
        ///　刷新状态使菜单不可用
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshState(VirtualMachine.Component.WinMVC.generic.MainViewState state)
        {
            base.RefreshState(state);
            switch (state)
            {
                case MainViewState.Initialize:
                    ToolMenu.LockItem(ToolMenuItem.AddNew);
                    break;
            }
        }

        //根据主维度来控制界面元素的读写
        private void ControlElements(string type) 
        {
            txtDimName.ReadOnly = false;
            txtCalExp.ReadOnly = false;
            txtRemark.ReadOnly = false;
            txtAlias.ReadOnly = false;
            cboCalType.ReadOnly = false;
            cboUnit.ReadOnly = false;
            btnGenCalExp.Enabled = true;
            btnScope.Enabled = false;

            //switch (type) {
            //    case "3":
            //        txtDimName.ReadOnly = false;
            //        txtRemark.ReadOnly = false;
            //        cboCalType.ReadOnly = false;
            //        cboUnit.ReadOnly = false;
            //        btnGenCalExp.Enabled = true;
            //        btnScope.Enabled = true;
            //        return;
            //    case "4":
            //        txtDimName.ReadOnly = false;
            //        return;
            //}
            
        }

        #endregion

        #region 函数支持

        //装载维度列表
        private void LoadDimensions(DimensionCategory cat)
        {
            txtDimName.Text = cat.Name;
            txtDimName.Tag = cat;
            //txtFactor.Text = cat.Factor.ToString();
            if (cat.CalExpression != null && cat.CalExpression.IndexOf(VDimensionDefine.CalExpSplit) >= 0)
            {
                string[] calExp = cat.CalExpression.Split(VDimensionDefine.CalExpSplit);
                txtCalExp.Text = calExp[0];
                txtCalExp.Tag = calExp[1];
            }
            else
            {
                txtCalExp.Text = cat.CalExpression;
            }

            txtRemark.Text = cat.Describe;
            txtAlias.Text = cat.Code;//别名
            if (cat.CalTypeCode != null && !cat.CalTypeCode.Trim().Equals(""))
            {
                cboCalType.SelectedValue = cat.CalTypeCode;
            }
            else {
                cboCalType.SelectedValue = "";
            }
            //计量单位
            if (cat.DimUnit != null && !cat.DimUnit.Trim().Equals("0") && !cat.DimUnit.Trim().Equals(""))
            {
                cboUnit.SelectedValue = cat.DimUnit;
            }
            else
            {
                cboUnit.SelectedValue = "0";
            }

            
        }

        private bool Check()
        {
            if (txtDimName.Text == null || txtDimName.Text.Trim().Equals(""))
            {
                KnowledgeMessageBox.InforMessage("请输入维度名称！");
                txtDimName.Focus();
                return false;
            }

            /*if (txtAlias.Text == null || txtAlias.Text.Trim().Equals(""))
            {
                KnowledgeMessageBox.InforMessage("请输入维度别名！");
                txtAlias.Focus();
                return false;
            }*/

            /*string factor = txtFactor.Text;
            decimal decFactor=0m;
            if (decimal.TryParse(factor, out decFactor) == false)
            {
                KnowledgeMessageBox.InforMessage("请输入正确的权重。");
                txtFactor.Focus();
                return false;
            }*/
            return true;
        }

        //通过选中的维度树中的节点来判断此维度所归属的主维度的类型(1:系统生成维度；2:外部来源维度；3:度量维度；4:其他手工维度)
        private string GetSelectDimType(DimensionCategory dc) 
        {
            string type = "0";
            string sysCode = dc.SysCode;
            foreach (DimensionRegister dr in priDimList)
            {
                string priSysCode = "100." + dr.Id + ".";
                if (sysCode.StartsWith(priSysCode))
                {
                    type = dr.OriginTypeCode;
                    //3为手工维度
                    if ("3".Equals(type))
                    {
                        if (dr.IfMeasure == 0)
                        {
                            type = "4";
                        }
                    }
                }
            }
            return type;
        }

        #endregion    

        
    }
}