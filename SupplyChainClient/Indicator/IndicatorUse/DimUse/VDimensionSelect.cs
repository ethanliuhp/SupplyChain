using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;
using Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse
{
    public partial class VDimensionSelect : Form
    {
        private string dimCatId = "";

        private MIndicatorUse model = new MIndicatorUse();
        private MCubeManager mCube = new MCubeManager();

        private IList dimArray = new ArrayList();
        private bool isOkClicked = false;
        private ViewStyle vStyle;
        int order = 1;//维度值的排列顺序
        //private Hashtable bb = new Hashtable();

        /// <summary>
        /// 视图维度行列顺序明细
        /// </summary>
        public ViewStyle VStyle
        {
            get { return vStyle; }
            set { vStyle = value; }
        }

        /// <summary>
        /// 新的维度树结构数组
        /// </summary>
        public IList DimArray
        {
            get { return dimArray; }
            set { dimArray = value; }
        }

        /// <summary>
        /// 是否点击确定按钮
        /// </summary>
        public bool IsOkClicked
        {
            get { return isOkClicked; }
            //set { isOkClicked = value; }
        }

        /// <summary>
        /// 维度分类ID
        /// </summary>
        public string DimCatId
        {
            get { return dimCatId; }
            set { dimCatId = value; }
        }
        
        public VDimensionSelect()
        {
            InitializeComponent();
            InitialEvents();
        }

        private void InitialEvents()
        {
            this.Load += new EventHandler(VDimensionSelect_Load);
            btnSelectAll.Click += new EventHandler(btnSelectAll_Click);
            btnUnSelectAll.Click += new EventHandler(btnUnSelectAll_Click);
            btnOk.Click += new EventHandler(btnOk_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            cmsStyleMx.ItemClicked += new ToolStripItemClickedEventHandler(cmsStyleMx_ItemClicked);
        }

        void cmsStyleMx_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "上移")
            {
                DimensionCategory dc = TVCategory.SelectedNode.Tag as DimensionCategory;              
                
                string nodeType = dc.CategoryNodeType.ToString();
                if (!"LeafNode".Equals(nodeType))
                {
                    MessageBox.Show("此节点还存在子节点，不允许移动！");
                    return;
                }

                TreeNode tn = TVCategory.SelectedNode;//当前节点
                int curr_index = tn.Index;//当前索引位置
                bool ifCheck = tn.Checked;
                TreeNode parentNode = tn.Parent;//父节点
                if (parentNode != null)
                {
                    tn.Remove();
                    //构造新节点
                    TreeNode tn_new = new TreeNode();
                    tn_new.Name = dc.Id.ToString();
                    tn_new.Text = dc.Name;
                    tn_new.Tag = dc;
                    tn_new.Checked = ifCheck;

                    int pre_index = 0;//前一索引
                    if (curr_index > 0)
                    {
                        pre_index = curr_index - 1;
                    }
                    
                    parentNode.Nodes.Insert(pre_index, tn_new);//插入节点
                    TVCategory.SelectedNode = tn_new;
                }
                
            }
            else if (e.ClickedItem.Text == "下移")
            {
                DimensionCategory dc = TVCategory.SelectedNode.Tag as DimensionCategory;

                string nodeType = dc.CategoryNodeType.ToString();
                if (!"LeafNode".Equals(nodeType))
                {
                    MessageBox.Show("此节点还存在子节点，不允许移动！");
                    return;
                }

                TreeNode tn = TVCategory.SelectedNode;//当前节点
                int curr_index = tn.Index;//当前索引位置
                bool ifCheck = tn.Checked;
                TreeNode parentNode = tn.Parent;//父节点
                if (parentNode != null)
                {
                    tn.Remove();
                    //构造新节点
                    TreeNode tn_new = new TreeNode();
                    tn_new.Name = dc.Id.ToString();
                    tn_new.Text = dc.Name;
                    tn_new.Tag = dc;
                    tn_new.Checked = ifCheck;

                    int next_index = 0;//前一索引
                    next_index = curr_index + 1;

                    parentNode.Nodes.Insert(next_index, tn_new);//插入节点
                    TVCategory.SelectedNode = tn_new;
                }
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        void btnOk_Click(object sender, EventArgs e)
        {
            dimArray = new ArrayList();
            GenCheckedNodes(TVCategory.Nodes[0], null, VStyle, ref dimArray);
            if (dimArray.Count == 0)
            {
                KnowledgeMessageBox.InforMessage("请至少选择一个维度。");
                return;
            }

            isOkClicked = true;
            vStyle = null;
            Close();
        }

        private void GetCheckedNodes(TreeNode treeNode,ref IList list)
        {
            if (treeNode.Checked)
            {
                list.Add(treeNode.Tag);
            }

            foreach (TreeNode node in treeNode.Nodes)
            {
                if (node.Checked)
                {
                    list.Add(node.Tag);
                }
                if (node.Nodes.Count > 0)
                {
                    GetCheckedNodes(node, ref list);
                }
            }
        }

        #region 生成新的树结构

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
                    viewStyleDim.DimUnit = dimCat.DimUnitName;
                    viewStyleDim.Name = dimCat.Name;
                    viewStyleDim.ParentId = parent;
                    viewStyleDim.ViewStyleId = viewStyle;
                    viewStyleDim.OrderNo = order;
                    list.Add(viewStyleDim);
                    topParentTree = viewStyleDim;
                    hasTopParent = true;
                    order++;
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
        }
        #endregion

        void btnUnSelectAll_Click(object sender, EventArgs e)
        {
            CheckNodes(TVCategory.Nodes[0], false);
        }

        void btnSelectAll_Click(object sender, EventArgs e)
        {
            CheckNodes(TVCategory.Nodes[0], true);
        }

        private void CheckNodes(TreeNode treeNode, bool nodeChecked)
        {
            treeNode.Checked = nodeChecked;
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    CheckNodes(node, nodeChecked);
                }
            }
        }

        void VDimensionSelect_Load(object sender, EventArgs e)
        {
            InitialTree();
        }

        /// <summary>
        /// 生成树结构
        /// </summary>
        private void InitialTree()
        {
            try
            {
                if (string.IsNullOrEmpty(dimCatId))
                {
                    return;
                }
                else
                {
                    Hashtable table = new Hashtable();
                    DimensionCategory cat = model.DimDefSrv.GetDimensionCategoryById(dimCatId);
                    IList list = model.DimDefSrv.GetAllChildNodes(cat);
                    TVCategory.Nodes.Clear();

                    //设置父节点
                    TreeNode parent = new TreeNode();
                    parent.Name = cat.Id.ToString();
                    parent.Text = cat.Name;
                    parent.Tag = cat;
                    parent.Checked = true;
                    TVCategory.Nodes.Add(parent);
                    table.Add(parent.Name, parent);

                    //重新排列顺序
                    list = this.OrderStyleMx(list, dimArray);

                    foreach (DimensionCategory obj in list)
                    {
                        TreeNode tmpNode = new TreeNode();
                        tmpNode.Name = obj.Id.ToString();
                        tmpNode.Text = obj.Name;
                        tmpNode.Tag = obj;

                        //选中已经保存的维度节点
                        if (dimArray== null || dimArray.Count == 0)
                        { }
                        else
                        {
                            foreach (ViewStyleDimension vsd in dimArray)
                            {
                                if (vsd.DimCatId+"" == obj.Id)
                                {
                                    tmpNode.Checked = true;
                                    break;
                                }
                            }
                        }

                        if (obj.ParentNode != null)
                        {
                            TreeNode node = table[obj.ParentNode.Id.ToString()] as TreeNode;
                            node.Nodes.Add(tmpNode);
                            //展开己选中的节点
                            if (tmpNode.Checked)
                            {
                                TVCategory.SelectedNode = tmpNode;
                            }
                        }
                        else
                        {
                            TVCategory.Nodes.Add(tmpNode);
                            //展开己选中的节点
                            if (tmpNode.Checked)
                            {
                                TVCategory.SelectedNode = tmpNode;
                            }
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
            }
            catch (Exception ex)
            {
                KnowledgeMessageBox.InforMessage("查找维度分类树出错。",ex);
            }
        }

        /// <summary>
        /// 把维度值List重新排序,先排已经选中的维度值(并按顺序排列)
        /// </summary>
        /// <param name="dimCatList">维度值的所有集合</param>
        /// <param name="checkedStyleMx">已经选中的维度值集合</param>
        /// <returns>重新排序的维度值集合</returns>
        private IList OrderStyleMx(IList dimCatList,IList checkedStyleMx)
        {
            IList newDimCatList = new ArrayList();
            IList transList = new ArrayList();
            IList temp = new ArrayList();
            //按树级别和父节点ID重新组合
            string currlinkstr = "";
            string nextlinkstr = "";
            foreach (DimensionCategory obj in dimCatList)
            {
                //第一个取值
                if ("".Equals(currlinkstr))
                {
                    currlinkstr = obj.Level + "_" + obj.ParentNode.Id;
                    temp.Add(obj);
                }
                else {
                    nextlinkstr = obj.Level + "_" + obj.ParentNode.Id;
                    if (nextlinkstr.Equals(currlinkstr))
                    {
                        temp.Add(obj);
                    }
                    else {
                        IList tempx = new ArrayList();
                        foreach (Object o in temp) tempx.Add(o);
                        transList.Add(tempx);
                        temp.Clear();
                        currlinkstr = obj.Level + "_" + obj.ParentNode.Id;
                        temp.Add(obj);
                    }
                }               
            }
            transList.Add(temp);//加上最后一组树级别和父节点ID相同的集合

            dimCatList.Clear();
            //同一树级别和父节点ID的集合重新排序
            foreach (IList list in transList)
            {
                IList tempx = this.ReOrder(list, checkedStyleMx);
                foreach (DimensionCategory dc in tempx)
                {
                    dimCatList.Add(dc);
                }
            }

            return dimCatList;
        }

        // <summary>
        ///重新排序已选中的格式明细
        /// </summary>
        /// <param name="list">同一树级别和父节点ID的集合</param>
        /// <param name="checkedStyleMx">已选中的格式明细集合</param>
        private IList ReOrder(IList list, IList checkedStyleMx)
        {
            IList newDimCatList = new ArrayList();
            Hashtable ht_order = new Hashtable();
            //先载入已经选中的维度值集合
            foreach (ViewStyleDimension vsd in checkedStyleMx)
            {
                foreach (DimensionCategory obj in list)
                {
                    if (vsd.DimCatId+"" == obj.Id)
                    {
                        newDimCatList.Add(obj);
                        ht_order.Add(obj.Id, vsd.OrderNo);
                        break;
                    }
                }
            }

            //重新按顺序排列已经选中的维度对象
            IList temp = new ArrayList();
            foreach (DimensionCategory obj in newDimCatList)
            {
                this.BubbleOrder(obj, temp, ht_order);
            }
            newDimCatList.Clear();
            foreach (object o in temp) newDimCatList.Add(o);

            //再载入其他的维度值集合
            foreach (DimensionCategory obj in list)
            {
                if (!newDimCatList.Contains(obj))
                {
                    newDimCatList.Add(obj);
                }
            }
            ht_order.Clear();
            return newDimCatList;
        }

        //冒泡算法，插入一个vsd
        private IList BubbleOrder(DimensionCategory dc, IList existDcList, Hashtable ht_order)
        {
            if (existDcList.Count == 0)
            {
                existDcList.Add(dc);
                return existDcList;
            }
            bool ifMax = true;//判断是否在目前最大的值
            foreach (DimensionCategory obj in existDcList)
            {
                if (Int16.Parse(ht_order[dc.Id].ToString()) < Int16.Parse(ht_order[obj.Id].ToString()))
                {
                    existDcList.Insert(existDcList.IndexOf(obj), dc);
                    ifMax = false;
                    break;
                }
            }
            if (ifMax == true)
            {
                existDcList.Add(dc);
            }

            return existDcList;
        }

        
    }
}