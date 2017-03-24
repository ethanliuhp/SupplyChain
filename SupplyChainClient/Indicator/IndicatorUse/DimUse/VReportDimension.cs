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
    public partial class VReportDimension : Form
    {
        //private long dimCatId = 0;

        private MIndicatorUse model = new MIndicatorUse();
        private MCubeManager mCube = new MCubeManager();
        private DimensionCategory selectedCat = new DimensionCategory();
        public bool isOkClicked = false;
        public string selectDimId;
        public string selectDimName;
        public string selectDimCode;
        public DimensionCategory main_cat;

        public VReportDimension()
        {
            InitializeComponent();
            InitialEvents();
        }

        private void InitialEvents()
        {
            this.Load += new EventHandler(VDimensionSelect_Load);
            ///分类树事件
            TVCategory.AfterSelect += new TreeViewEventHandler(TVCategory_AfterSelect);
            btnOk.Click += new EventHandler(btnOk_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        void VDimensionSelect_Load(object sender, EventArgs e)
        {
            LoadIndicatorCategoryTree();
        }


        void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TVCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            if (node == null) return;
            selectedCat = node.Tag as DimensionCategory;
        }

        void btnOk_Click(object sender, EventArgs e)
        {
            if (selectedCat.Id == "")
            {
                KnowledgeMessageBox.InforMessage("请选择一个维度！");
                return;
            }
            selectDimId = selectedCat.Id;
            selectDimName = selectedCat.Name;
            selectDimCode = selectedCat.Code;
            isOkClicked = true;
            Close();
        }

        /// <summary>
        /// 加载维度树
        /// </summary>
        private void LoadIndicatorCategoryTree()
        {
            try
            {
                Hashtable table = new Hashtable();
                TVCategory.Nodes.Clear();

                if (main_cat == null || "".Equals(main_cat))
                {
                    KnowledgeMessageBox.InforMessage("主维度为空！");
                    return;
                }
                
                IList list = model.DimDefSrv.GetAllChildNodes(main_cat);
                //设置父节点
                TreeNode parent = new TreeNode();
                parent.Name = main_cat.Id.ToString();
                parent.Text = main_cat.Name;
                parent.Tag = main_cat;
                TVCategory.Nodes.Add(parent);
                table.Add(parent.Name, parent);

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
                KnowledgeMessageBox.InforMessage("装载维度分类树出错。", ex);
            }
        }
        
        
    }
}