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
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse
{
    public partial class VViewDefTechEco: TBasicDataView
    {
        private MIndicatorUse model = new MIndicatorUse();
        private MBasicData bdModel = new MBasicData();
        private CubeRegister cubeReg = new CubeRegister(); 
        private Hashtable checkedDim = new Hashtable();
        
        public VViewDefTechEco()
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
            cboIfDis.DropDownStyle = ComboBoxStyle.DropDownList;
            cboType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCollectType.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void EnableControls(bool enabled)
        {
            txtViewName.Enabled = enabled;
            cboIfDis.Enabled = enabled;
            cboType.Enabled = enabled;
            cboCollectType.Enabled = enabled;
            dgvDim.Enabled = enabled;
            btnSave.Enabled = enabled;
        }

        /// <summary>
        /// 获取主题
        /// </summary>
        private void GetCube()
        {
            try
            {
                cubeReg = model.CubeSrv.GetCubeRegisterById("1");
                if (cubeReg == null) return;

                //添加模板
                IList list = model.ViewSrv.GetViewMainByCubeId(cubeReg);
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
                KnowledgeMessageBox.InforMessage("查找主题出错。", ex);
            }
        }

        /// <summary>
        /// 初始化基础数据
        /// </summary>
        private void InitialBasicdata()
        {
            model.InitialIndicatorCombox(KnowledgeUtil.ViewTypeCode, KnowledgeUtil.ViewTypeName, cboType, true);
            model.InitialIndicatorCombox(KnowledgeUtil.CollectTypeCode, KnowledgeUtil.CollectTypeName, cboCollectType, true);
            model.InitialIndicatorCombox(KnowledgeUtil.IfDisplaySonMotherCode, KnowledgeUtil.IfDisplaySonMotherName, cboIfDis, false);
        }

        private void InitialEvents()
        {
            lstViewSel.SelectedIndexChanged += new EventHandler(lstViewSel_SelectedIndexChanged);
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnModify.Click += new EventHandler(btnModify_Click);
            btnDel.Click += new EventHandler(btnDel_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnPreview.Click += new EventHandler(btnPreview_Click);

            dgvDim.CellContentClick += new DataGridViewCellEventHandler(dgvDim_CellContentClick);
        }

        /// <summary>
        /// 模板预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnPreview_Click(object sender, EventArgs e)
        {
            VViewPreview vPreview = new VViewPreview();
            vPreview.Text = "模板预览--【" + txtViewName.Text + "】";
            IList vStyleList = new ArrayList();
            foreach (DataGridViewRow row in dgvDim.Rows)
            {
                ViewStyle vs = new ViewStyle();
                //方向
                object objDirection = row.Cells[colRCSelect.Name].Value;
                if (objDirection == null)
                {
                    vs.Direction = "";
                }
                else
                {
                    vs.Direction = objDirection.ToString();
                }

                //显示顺序
                object objRangeOrder = row.Cells[colOrder.Name].Value;
                int intOrder = 0;
                if (objRangeOrder == null || int.TryParse(objRangeOrder.ToString(), out intOrder) == false)
                {
                    vs.RangeOrder = 0;
                }
                else
                {
                    vs.RangeOrder = int.Parse(objRangeOrder.ToString());
                }

                //维度名称
                object objDimName = row.Cells[colDim.Name].Value;
                if (objDimName == null)
                {
                    vs.OldCatRootName = "";
                }
                else
                {
                    vs.OldCatRootName = objDimName.ToString();
                }

                //给ID赋值，传hashcode便于从对应的checkedDim中取维度值
                vs.Id = row.GetHashCode().ToString();

                vStyleList.Add(vs);
            }

            Sort(vStyleList);

            vPreview.checkedDim = checkedDim;
            vPreview.vStyleList = vStyleList;

            //视图主表信息
            ViewMain obj = txtViewName.Tag as ViewMain;
            if (obj == null)
            {
                obj = new ViewMain();
            }
            vPreview.vm = obj;

            vPreview.ShowDialog();
        }

        /// <summary>
        /// 选择维度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgvDim_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int colDimIndex = dgvDim.Columns.IndexOf(colDim);
            int currentColIndex = e.ColumnIndex;
            if (colDimIndex == currentColIndex)
            {
                IList dimArray = checkedDim[dgvDim.Rows[e.RowIndex].GetHashCode()] as IList;
                if (dimArray == null) dimArray = new ArrayList();
                VDimensionSelect dimSelect = new VDimensionSelect();
                CubeAttribute cubeAttr = dgvDim.Rows[e.RowIndex].Tag as CubeAttribute;
                dimSelect.DimCatId = cubeAttr.DimensionId;

                ViewStyle viewStyle = dgvDim.Rows[e.RowIndex].Cells[colSelect.Name].Tag as ViewStyle;
                if (viewStyle == null) viewStyle = new ViewStyle();

                dimSelect.VStyle = viewStyle;
                dimSelect.DimArray = dimArray;

                dimSelect.ShowDialog();
                if (dimSelect.IsOkClicked)
                {
                    dgvDim.Rows[e.RowIndex].Cells[colRCSelect.Name].Tag = true;
                    checkedDim.Remove(dgvDim.Rows[e.RowIndex].GetHashCode());
                    checkedDim.Add(dgvDim.Rows[e.RowIndex].GetHashCode(), dimSelect.DimArray);
                }
            }
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
                if ("1".Equals(cboIfDis.SelectedIndex.ToString()))
                {
                    obj.IfDisplaySonMother = cboIfDis.Text;
                }
                else {
                    obj.IfDisplaySonMother = "";
                }

                if (cboCollectType.SelectedValue != null)
                {
                    obj.CollectTypeCode = cboCollectType.SelectedValue.ToString();
                }
                obj.CollectTypeName = cboCollectType.Text;

                if (string.IsNullOrEmpty(obj.Id))
                {
                    isNew = true;

                    obj.CubeRegId = cubeReg;

                    obj.CreatedDate = DateTime.Today;
                    obj.Author = ConstObject.LoginPersonInfo;

                    obj.TheJob = ConstObject.TheSysRole;
                    obj.TheOperOrg = ConstObject.TheOperationOrg;
                }

                try
                {
                    foreach (DataGridViewRow row in dgvDim.Rows)
                    {
                        ViewStyle viewStyle = row.Cells[colSelect.Name].Tag as ViewStyle;

                        IList dimArray = checkedDim[row.GetHashCode()] as IList;
                        if (dimArray == null || dimArray.Count == 0)
                        {
                            KnowledgeMessageBox.InforMessage("请选择至少一个" + row.Cells[colDim.Name].Value + "维度");
                            return;
                        }

                        if (viewStyle == null || string.IsNullOrEmpty(viewStyle.Id)) viewStyle = new ViewStyle();
                        if (!isNew)
                        {
                            object dimChanged = row.Cells[colRCSelect.Name].Tag;
                            if (dimChanged != null && ((bool)dimChanged) == true)
                            {
                                foreach (ViewStyle vs in obj.ViewStyles)
                                {
                                    if (vs.Id == viewStyle.Id)
                                    {
                                        vs.Details.Clear();
                                        viewStyle = vs;
                                        foreach (ViewStyleDimension vsd in dimArray)
                                        {
                                            viewStyle.Details.Add(vsd);
                                        }
                                        break;
                                    }
                                }
                                row.Cells[colRCSelect.Name].Tag = false;
                            }
                        }

                        if (((bool)row.Cells[colSelect.Name].Value) == true)
                        {
                            object objRCValue = row.Cells[colRCSelect.Name].Value;
                            if (objRCValue == null)
                            {
                                KnowledgeMessageBox.InforMessage("请选择行或者列。");
                                row.Cells[colRCSelect.Name].Selected = true;
                                return;
                            }
                            object orderValue = row.Cells[colOrder.Name].Value;

                            viewStyle.Main = obj;
                            viewStyle.Direction = objRCValue.ToString();
                            viewStyle.CubeAttrId = ((CubeAttribute)row.Tag).Id;
                            viewStyle.OldCatRootId = ((CubeAttribute)row.Tag).DimensionId;
                            viewStyle.OldCatRootName = ((CubeAttribute)row.Tag).DimensionName;
                            if (orderValue == null)
                            {
                                viewStyle.RangeOrder = 0;
                            }
                            else
                            {
                                int intOrderValue = 0;
                                if (int.TryParse(orderValue.ToString(), out intOrderValue) == false)
                                {
                                    KnowledgeMessageBox.InforMessage("请输入正确的排序顺序");
                                    row.Cells[colOrder.Name].Selected = true;
                                    return;
                                }
                                viewStyle.RangeOrder = int.Parse(orderValue.ToString());
                            }

                            //维度树，重新选择维度时，维度树明细直接在类VDimensionSelect中生成集合
                            if (string.IsNullOrEmpty(viewStyle.Id))
                            {
                                foreach (ViewStyleDimension vsd in dimArray)
                                {
                                    ViewStyleDimension temp = vsd;
                                    temp.ViewStyleId = viewStyle;
                                    viewStyle.Details.Add(temp);
                                }
                            }

                            if (string.IsNullOrEmpty(viewStyle.Id))
                            {
                                obj.ViewStyles.Add(viewStyle);
                            }
                        }
                        else
                        {
                            //删除没有选中的列
                            foreach (ViewStyle temp in obj.ViewStyles)
                            {
                                if (temp.Id == viewStyle.Id)
                                {
                                    obj.ViewStyles.Remove(temp);
                                    break;
                                }
                            }
                        }
                    }

                    //保存模板主表　
                    obj = model.ViewSrv.SaveViewMain(obj);
                    ///保存主题数据
                    SaveCubeData(obj);
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
                    IList distribute_list = model.ViewSrv.GetViewDistributeByViewMain(obj);
                    if (distribute_list.Count > 0)
                    {
                        KnowledgeMessageBox.InforMessage("删除模板出错:该模板已经分发了岗位，不允许删除！");
                        return;
                    }
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
                    if (exception.IndexOf("违反完整约束条件") > 0)
                    {
                        KnowledgeMessageBox.InforMessage("删除模板出错:该模板已经分发了岗位，不允许删除！");
                    }
                    else
                    {
                        KnowledgeMessageBox.InforMessage("删除模板出错。", ex);
                    }
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
            EnableSelected();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAdd_Click(object sender, EventArgs e)
        {
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

            //模板类型
            cboType.ReadOnly = false;
            cboType.SelectedIndex = 0;

            //模板采集频率
            cboCollectType.ReadOnly = false;
            cboCollectType.SelectedIndex = 0;

            //模板采集频率
            cboIfDis.ReadOnly = false;
            cboIfDis.SelectedIndex = 0;

            //dgvDim.ReadOnly = false;
            EnableSelected();

            //添加维度列表
            try
            {
                IList list = model.CubeSrv.GetCubeAttrByCubeResgisterId(cubeReg);
                dgvDim.Rows.Clear();
                foreach (CubeAttribute cubeAttr in list)
                {
                    int i = dgvDim.Rows.Add();
                    DataGridViewRow row = dgvDim.Rows[i];
                    row.Tag = cubeAttr;
                    row.Cells[colSelect.Name].Value = true;
                    row.Cells[colRCSelect.Name].Value = "行";
                    row.Cells[colOrder.Name].Value = i;
                    row.Cells[colDim.Name].Value = cubeAttr.DimensionName;

                    ViewStyle viewStyle = new ViewStyle();
                    row.Cells[colSelect.Name].Tag = viewStyle;
                }

            }
            catch (Exception ex)
            {
                KnowledgeMessageBox.InforMessage("查找主题属性出错。", ex);
            }
        }

        /// <summary>
        /// 检查输入
        /// </summary>
        /// <returns></returns>
        private bool CheckInput()
        {
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
            cboIfDis.SelectedIndex = 0;
            cboCollectType.SelectedIndex = 0;

            dgvDim.Rows.Clear();
        }

        void lstViewSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewMain obj = lstViewSel.SelectedItem as ViewMain;
            if (obj == null) return;
            txtViewName.Text = obj.ViewName;
            txtViewName.Tag = obj;
            dgvDim.Rows.Clear();

            if (obj.ViewTypeCode != null)
            {
                cboType.SelectedValue = obj.ViewTypeCode;
            }

            if (obj.CollectTypeCode != null)
            {
                cboCollectType.SelectedValue = obj.CollectTypeCode;
            }

            if (obj.IfDisplaySonMother != null && !"".Equals(obj.IfDisplaySonMother))
            {
                cboIfDis.SelectedValue = "1";
            }

            if (cubeReg == null)
            {
                KnowledgeMessageBox.InforMessage("请先选择一个主题。");
            }

            //重新初始化模板维度集合哈希表
            checkedDim = new Hashtable();

            //添加维度列表
            try
            {
                IList list = model.CubeSrv.GetCubeAttrByCubeResgisterId(cubeReg);
                IList viewStyleList = obj.ViewStyles;
                foreach (CubeAttribute cubeAttr in list)
                {
                    int i = dgvDim.Rows.Add();
                    DataGridViewRow row = dgvDim.Rows[i];
                    row.Tag = cubeAttr;

                    bool hasChecked = false;
                    ViewStyle checkedViewStyle = new ViewStyle();

                    foreach (ViewStyle viewStyle in viewStyleList)
                    {
                        if (viewStyle.CubeAttrId == cubeAttr.Id)
                        {
                            hasChecked = true;
                            checkedViewStyle = viewStyle;
                            break;
                        }
                    }
                    if (hasChecked)
                    {
                        row.Cells[colSelect.Name].Value = true;
                        row.Cells[colSelect.Name].Tag = checkedViewStyle;
                        checkedDim.Add(row.GetHashCode(), checkedViewStyle.Details);
                        row.Cells[colRCSelect.Name].Value = checkedViewStyle.Direction;
                        row.Cells[colOrder.Name].Value = checkedViewStyle.RangeOrder;
                        row.Cells[colDim.Name].Value = cubeAttr.DimensionName;
                    }
                    else
                    {
                        row.Cells[colSelect.Name].Value = false;
                        row.Cells[colSelect.Name].Tag = checkedViewStyle;
                        checkedDim.Add(row.GetHashCode(), checkedViewStyle.Details);
                        row.Cells[colRCSelect.Name].Value = "行";
                        row.Cells[colOrder.Name].Value = i;
                        row.Cells[colDim.Name].Value = cubeAttr.DimensionName;
                    }
                }
            }
            catch (Exception ex)
            {
                KnowledgeMessageBox.InforMessage("查找主题属性出错。", ex);
            }

            EnableControls(false);
        }

        void cboCube_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cubeReg == null) return;

                //添加模板
                IList list = model.ViewSrv.GetViewMainByCubeId(cubeReg);
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

        /// <summary>
        /// 保存立方数据
        /// </summary>
        /// <param name="vMain"></param>
        private void SaveCubeData(ViewMain vMain)
        {
            try
            {

                //模板列表
                IList vStyleList = model.ViewSrv.GetViewStyleByViewMain(vMain, false);
                //立方属性，由于cubeReg.CubeAttribute没有正确的排序，导致插入数据
                //位置不正确，重新求cubeReg.CubeAttribute
                IList cubeAttribute = model.CubeSrv.GetCubeAttrByCubeResgisterId(cubeReg);
                cubeReg.CubeAttribute = cubeAttribute;

                //二维表值
                IList rowValueLists = model.Gen2DimTable(vStyleList, "Id");
                foreach (IList rowValue in rowValueLists)
                {
                    CubeData cubeData = new CubeData();
                    cubeData.DimDataList = rowValue;
                    model.CubeSrv.SetCubeData(cubeReg, cubeData);
                }
            }
            catch (Exception ex)
            {
                KnowledgeMessageBox.InforMessage("保存立方数据出错。");
            }
        }

        #region 生成新的树结构(暂时没用了)

        private CustomTreeView InitialTree(string dimCatId)
        {
            CustomTreeView tree = new CustomTreeView();
            tree.CheckBoxes = true;

            Hashtable table = new Hashtable();
            DimensionCategory cat = model.DimDefSrv.GetDimensionCategoryById(dimCatId);
            IList list = model.DimDefSrv.GetAllChildNodes(cat);
            tree.Nodes.Clear();

            //设置父节点
            TreeNode parent = new TreeNode();
            parent.Name = cat.Id.ToString();
            parent.Text = cat.Name;
            parent.Tag = cat;
            parent.Checked = true;
            tree.Nodes.Add(parent);
            table.Add(parent.Name, parent);


            foreach (DimensionCategory obj in list)
            {
                TreeNode tmpNode = new TreeNode();
                tmpNode.Name = obj.Id.ToString();
                tmpNode.Text = obj.Name;
                tmpNode.Tag = obj;
                tmpNode.Checked = true;
                if (obj.ParentNode != null)
                {
                    TreeNode node = table[obj.ParentNode.Id.ToString()] as TreeNode;
                    node.Nodes.Add(tmpNode);
                }
                else
                {
                    tree.Nodes.Add(tmpNode);
                }
                table.Add(tmpNode.Name, tmpNode);
            }

            return tree;
        }

        private ViewStyleDimension topParentTree = new ViewStyleDimension();
        bool hasTopParent = false;

        private void GenCheckedNodes(TreeNode treeNode, ViewStyleDimension parent, ViewStyle viewStyle, ref IList list)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                ViewStyleDimension viewStyleDim = new ViewStyleDimension();
                if (node.Checked)
                {
                    DimensionCategory dimCat = node.Tag as DimensionCategory;

                    viewStyleDim.DimCatId = dimCat.Id;
                    viewStyleDim.Name = dimCat.Name;
                    viewStyleDim.ParentId = parent;
                    viewStyleDim.ViewStyleId = viewStyle;
                    list.Add(viewStyleDim);
                    topParentTree = viewStyleDim;
                    hasTopParent = true;
                }
                if (node.Nodes.Count > 0)
                {
                    if (hasTopParent)
                    {
                        GenCheckedNodes(node, topParentTree, viewStyle, ref list);
                    }
                    else
                    {
                        GenCheckedNodes(node, null, viewStyle, ref list);
                    }
                }
            }
            hasTopParent = false;
            topParentTree = null;
        }
        #endregion

        /// <summary>
        /// 根据排列顺序重新生成IList
        /// </summary>
        /// <param name="vStyleList"></param>
        private void Sort(IList vStyleList)
        {
            IList ret = new ArrayList();
            int i, j;
            bool exchange;
            for (i = 0; i < vStyleList.Count; i++)
            {
                exchange = false;
                for (j = vStyleList.Count - 2; j >= i; j--)
                {
                    ViewStyle style1 = vStyleList[j + 1] as ViewStyle;
                    ViewStyle style2 = vStyleList[j] as ViewStyle;
                    if (style1.RangeOrder < style2.RangeOrder)
                    {
                        vStyleList.RemoveAt(j);
                        vStyleList.Insert(j, style1);
                        vStyleList.RemoveAt(j + 1);
                        vStyleList.Insert(j + 1, style2);
                        exchange = true;
                    }
                }
                if (!exchange) //本趟排序未发生交换，提前终止算法 
                {
                    break;
                }
            }
        }

        private void customButton1_Click(object sender, EventArgs e)
        {
            VDimensionRegister vdr = new VDimensionRegister();
            vdr.ShowDialog();
        }

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableSelected();
        }

        private void EnableSelected()
        {
            /*string viewType = cboType.SelectedValue.ToString();
            if ("1".Equals(viewType))
            {
                dgvDim.Columns["colRCSelect"].ReadOnly = true;
                dgvDim.Columns["colRCSelect"].DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
            }
            else
            {
                dgvDim.Columns["colRCSelect"].ReadOnly = false;
                dgvDim.Columns["colRCSelect"].DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
            }*/
        }
    }
}