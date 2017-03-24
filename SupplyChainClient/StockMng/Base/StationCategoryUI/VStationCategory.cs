using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Core;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component.WinControls.Controls;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Main;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonForm;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Util;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.Base.StationCategoryUI
{
    public partial class VStationCategory : TBasicDataView
    {
        //public IMStationCategory theMStationCategory = null;
        public CStationCategory CStationCategory = null;
        private StationCategory theStationCategory = null;
        private Hashtable hashtableStationKind = new Hashtable();
        private MStationCategory theMStationCategory = new MStationCategory();

        public VStationCategory(CStationCategory cmc)
        {
            //if (this.theMStationCategory == null)
            //    this.theMStationCategory = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetRefModule(typeof(MStationCategory)) as IMStationCategory;
            this.CStationCategory = cmc;

            InitializeComponent();
            InitForm();
        }

        private void InitForm()
        {
            hashtableStationKind.Add("仓库", 0);
            hashtableStationKind.Add("货位", 1);

            hashtableStationKind.Add(0, "仓库");
            hashtableStationKind.Add(1, "货位");

            this.KeyPreview = true;
            InitEvents();

            //this.cboBusiType.DataSource = ClientUtil.StationTypeList();
        }

        private void InitEvents()
        {
            lnkAddCategory.Click += new EventHandler(lnkAddCategory_Click);
            lnkDeleteCategory.Click += new EventHandler(lnkDeleteCategory_Click);
            tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);
            tvwCategory.ItemDrag += new ItemDragEventHandler(tvwCategory_ItemDrag);
            tvwCategory.DragEnter += new DragEventHandler(tvwCategory_DragEnter);
            tvwCategory.DragDrop += new DragEventHandler(tvwCategory_DragDrop);
            tvwCategory.DragOver += new DragEventHandler(tvwCategory_DragOver);
            this.tvwCategory.NodeMouseClick += new TreeNodeMouseClickEventHandler(tvwCategory_NodeMouseClick);
            this.mnuTree.ItemClicked += new ToolStripItemClickedEventHandler(mnuTree_ItemClicked);
        }
        void mnuTree_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            if (e.ClickedItem.Text.Trim() == "增加子节点")
            {
                RefreshControls(MainViewState.Modify);
                lnkAddCategory_Click(lnkAddCategory, new EventArgs());
            }
            else if (e.ClickedItem.Text.Trim() == "修改节点")
            {
                RefreshControls(MainViewState.Modify);
            }
            else if (e.ClickedItem.Text.Trim() == "删除节点")
            {
                this.Refresh();
                mnuTree.Hide();
                lnkDeleteCategory_Click(lnkDeleteCategory, new EventArgs());
                this.Refresh();
            }
            else if (e.ClickedItem.Text.Trim() == "撤销")
            {
                mnuTree.Hide();
                RefreshControls(MainViewState.Browser);
                this.tvwCategory_AfterSelect(this.tvwCategory, new TreeViewEventArgs(this.tvwCategory.SelectedNode));

            }
            else if (e.ClickedItem.Text.Trim() == "保存节点")
            {
                mnuTree.Hide();
                if (this.SaveView())
                    this.RefreshControls(MainViewState.Browser);
            }
        }

        void tvwCategory_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                mnuTree.Show(tvwCategory, new Point(e.X, e.Y));
            }
        }


        public void Start()
        {
            RefreshState(MainViewState.Browser);

            this.tvwCategory.AllowDrop = true;

            LoadStationCategoryTree();

        }

        private void LoadStationCategoryTree()
        {
            int i = 0;
            Hashtable old_hashtable = new Hashtable();
            Hashtable hashtable = new Hashtable();
            try
            {
                i = 0;
                tvwCategory.Nodes.Clear();

                IList list = theMStationCategory.GetStationCategory();
                foreach (object o in list)
                {
                    StationCategory node = o as StationCategory;
                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = node.Id.ToString();
                    tnTmp.Text = "[" + node.Code + "]" + node.Name;
                    tnTmp.Tag = node;
                    old_hashtable.Add(tnTmp.Name, tnTmp);
                }

                foreach (object o in list)
                {
                    i++;
                    StationCategory childNode = o as StationCategory;
                    //if (childNode.State == 0) continue;

                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = "[" + childNode.Code + "]" + childNode.Name;
                    tnTmp.Tag = childNode;
                    if (childNode.ParentNode != null)
                    {
                        TreeNode tnp = null;
                        if (hashtable[childNode.ParentNode.Id.ToString()] != null)
                        {
                            tnp = hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                            tnp.Nodes.Add(tnTmp);
                        }
                        else
                        {
                            tnp = old_hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                            tnp.Nodes.Add(tnTmp);
                        }
                    }
                    else
                    {
                        tvwCategory.Nodes.Add(tnTmp);
                    }
                    hashtable.Add(tnTmp.Name, tnTmp);
                }
                if (list.Count > 0)
                {
                    this.tvwCategory.SelectedNode = this.tvwCategory.Nodes[0];
                    this.tvwCategory.SelectedNode.Expand();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("装载分类树出错：" + i.ToString() + ExceptionUtil.ExceptionMessage(e));
            }
        }


        private void ViewStationCategory()
        {
            try
            {
                ClearAll();

                txtCurrentPath.Text = this.tvwCategory.SelectedNode.FullPath.ToString();
                txtSysCode.Text = theStationCategory.SysCode;
                txtCategoryCode.Text = theStationCategory.Code;
                txtCategoryName.Text = theStationCategory.Name;
                txtCategoryDescript.Text = theStationCategory.Describe;
                if (this.theStationCategory.OperOrgInfo != null)
                {
                    this.txtOrg.Text = this.theStationCategory.OperOrgInfo.Name;
                    this.txtOrg.Tag = this.theStationCategory.OperOrgInfo;
                }
                else
                {
                    this.txtOrg.Text = "";
                    this.txtOrg.Tag = null;
                }

                this.txtAddress.Text = theStationCategory.Address;
                this.txtCapability.Text = theStationCategory.Capability.ToString();
                this.cboStationKind.Text = ClientUtil.ToString(hashtableStationKind[theStationCategory.StationKind]);
                //this.cboBusiType.SelectedIndex = theStationCategory.BusinessType;
                this.txtUsableCapability.Text = theStationCategory.UsableCapability.ToString();
                this.txtUsedPrice.Text = theStationCategory.UsedPrice.ToString();
                this.chkState.Checked = theStationCategory.State == 0 ? false : true;
            }
            catch (Exception e)
            {
                MessageBox.Show("显示分类出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }
        private bool ValideView()
        {
            string errMsg = "";
            try
            {
                if (theStationCategory == null)
                    errMsg += "请选择一个节点！";
                if (theStationCategory.ParentNode == null)
                    errMsg += "根节点不能修改！";

                theStationCategory.Code = txtCategoryCode.Text;
                theStationCategory.Name = txtCategoryName.Text;
                theStationCategory.Describe = txtCategoryDescript.Text;
                //if (this.txtOrg.Result.Count > 0)
                //{
                //    OperationOrgInfo theOperationOrg = this.txtOrg.Result[0] as OperationOrgInfo;
                //    this.theStationCategory.OperOrgInfo = theOperationOrg.SysCode;
                //}
                //else
                //{
                //    this.theStationCategory.OperOrgInfo = null;
                //}

                if (this.txtOrg.Result.Count > 0)
                {
                    OperationOrgInfo theOperationOrg = this.txtOrg.Result[0] as OperationOrgInfo;
                    theStationCategory.OperOrgInfo = theOperationOrg;
                    this.theStationCategory.OpgSysCode = theOperationOrg.SysCode;
                }
                else
                {
                    this.theStationCategory.OperOrgInfo = ConstObject.TheOperationOrg;
                    this.theStationCategory.OpgSysCode = ConstObject.TheOperationOrg.SysCode;
                }
                theStationCategory.Author = ConstObject.LoginPersonInfo;
                theStationCategory.Address = ClientUtil.ToString(this.txtAddress.Text);
                theStationCategory.Capability = StringUtil.StrToDecimal(this.txtCapability.Text);
                theStationCategory.StationKind = StringUtil.StrToInt(ClientUtil.ToString(hashtableStationKind[this.cboStationKind.Text]));
                theStationCategory.UsableCapability = StringUtil.StrToDecimal(this.txtUsableCapability.Text);
                theStationCategory.UsedPrice = StringUtil.StrToDecimal(this.txtUsedPrice.Text);
                theStationCategory.State = this.chkState.Checked == false ? 0 : 1;
                //theStationCategory.BusinessType = this.cboBusiType.SelectedIndex;
                if (theStationCategory.Id !="")
                {
                    theStationCategory.ModifyPerson = ConstObject.LoginPersonInfo;
                }

                if (theStationCategory.ParentNode != null)
                {
                    if (theStationCategory.ParentNode.Level == 1)
                    {
                        if (theStationCategory.StationKind == 1)
                            errMsg += "此节点的类型必须是仓库！";
                    }
                    else
                    {
                        if (theStationCategory.StationKind == 0)
                        {
                            if ((theStationCategory.ParentNode as StationCategory).StationKind == 1)
                            {
                                errMsg += "不能在仓位下面添加仓库！";
                            }
                        }
                    }
                }

                if (errMsg != "")
                    throw new Exception(errMsg);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("保存分类出错：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }


        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //oldSelectNode = e.Node;
            if (e.Node.Tag == null) return;//根节点不显示

            theStationCategory = e.Node.Tag as StationCategory;
            ViewStationCategory();
        }


        private void lnkAddCategory_Click(object sender, EventArgs e)
        {
            theStationCategory = new StationCategory();
            theStationCategory.ParentNode = this.tvwCategory.SelectedNode.Tag as StationCategory;
            ClearAll();
        }

        private void ClearAll()
        {
            this.txtCurrentPath.Text = "";
            this.txtSysCode.Text = "";
            this.txtCategoryCode.Text = "";
            this.txtCategoryName.Text = "";
            this.txtCategoryDescript.Text = "";

            this.txtAddress.Text = "";
            this.txtCapability.Text = "";
            this.txtUsableCapability.Text = "";
            this.txtUsedPrice.Text = "";
            //this.chkState.Checked = true;
        }

        private void lnkDeleteCategory_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValideDelete()) return;
                this.Refresh();
                theMStationCategory.Delete(theStationCategory);
                this.Refresh();
                if (this.tvwCategory.SelectedNode.Parent.Nodes.Count == 1 && (this.tvwCategory.SelectedNode.Parent.Tag as StationCategory).Id != "")
                {
                    StationCategory a = theMStationCategory.GetStationCategory((this.tvwCategory.SelectedNode.Parent.Tag as StationCategory).Id);
                    this.tvwCategory.SelectedNode.Parent.Tag = a;
                }
                this.tvwCategory.Nodes.Remove(this.tvwCategory.SelectedNode);

                this.Refresh();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }
        private bool ValideDelete()
        {
            string errMsg = "";
            try
            {
                TreeNode tn = tvwCategory.SelectedNode;
                if (tn == null || theStationCategory == null)
                    errMsg = "请选择要删除的记录!" + "\n";
                if (tn.Parent == null)
                    errMsg = "根节点不允许删除！!" + "\n";
                if (MessageBox.Show("真的要删除当前选中的分类吗？该操作将连它的所有子分类一同删除。", "分类删除", MessageBoxButtons.YesNo) == DialogResult.No)
                    return false;
                if (errMsg != "")
                    throw new Exception(errMsg);
                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }

        }


        #region 物料分类移动
        private void tvwCategory_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DoDragDrop(e.Item, DragDropEffects.All);
            }
        }

        private void tvwCategory_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;

        }

        private void tvwCategory_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                Point targetPoint = tvwCategory.PointToClient(new Point(e.X, e.Y));
                TreeNode targetNode = tvwCategory.GetNodeAt(targetPoint);

                TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
                TreeNode draggedNodeParent = draggedNode.Parent;

                if (draggedNode.Equals(targetNode) || ContainsNode(draggedNode, targetNode))
                    return;
                //如果被移动的节点是仓库必须移动仓库的节点或根节点下
                if ((draggedNode.Tag as StationCategory).StationKind == 0)
                {
                    if ((targetNode.Tag as StationCategory).StationKind == 1)
                    {
                        MessageBox.Show("节点移动不正确,原节点是[仓库]，目标节点不能是[仓位]!");
                        return;
                    }
                }
                else
                {
                    //仓位不能移动到父节点是根节点下
                    if ((targetNode.Tag as StationCategory).Level == 1)
                    {
                        MessageBox.Show("节点移动不正确,仓位节点必须移动到相应的[仓库]节点下！");
                        return;
                    }
                }


                enmMoveOrCopy copyOrMove = enmMoveOrCopy.copy;
                frmTreeMoveCopy frmTmp = new frmTreeMoveCopy();
                frmTmp.MoveOrCopy = enmMoveOrCopy.copy;
                frmTmp.TargetNode = targetNode;
                frmTmp.DraggedNode = draggedNode;
                frmTmp.ShowDialog();
                if (frmTmp.IsOK == false)
                {
                    return;
                }
                this.Refresh();
                copyOrMove = frmTmp.MoveOrCopy;
                if (copyOrMove == enmMoveOrCopy.move)
                {

                    IList list = new ArrayList();
                    list = theMStationCategory.Move(draggedNode.Tag as StationCategory, targetNode.Tag as StationCategory);
                    draggedNode.Parent.Tag = (list[0] as StationCategory).ParentNode;
                    foreach (StationCategory var in list)
                    {
                        MoveAfterToTreeTag(draggedNode, var);
                    }

                    if ((draggedNodeParent.Tag as StationCategory).Id != "" && draggedNodeParent.Nodes.Count == 0) //源父节点无子节点
                    {
                        StationCategory a = theMStationCategory.GetStationCategory((draggedNodeParent.Tag as StationCategory).Id);
                        draggedNodeParent.Tag = a;
                    }
                    tvwCategory_AfterSelect(this.tvwCategory, new TreeViewEventArgs(this.tvwCategory.SelectedNode));
                }
                else
                {
                    StationCategory matCatTmp = new StationCategory();
                    matCatTmp = (frmTmp.DraggedNode.Tag as StationCategory).Clone();
                    matCatTmp.ParentNode = frmTmp.DraggedNode.Parent.Tag as StationCategory;
                    frmTmp.DraggedNode.Tag = matCatTmp;

                    IList lst = new ArrayList();
                    int level = (int)matCatTmp.Level;
                    SaveCopyNode(frmTmp.DraggedNode, lst, ref level);
                    lst = theMStationCategory.Save(lst);

                    int i = 0;
                    CopyObjToTag(frmTmp.DraggedNode, lst, ref i);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
            }
        }

        private void CopyObjToTag(TreeNode draggedNode, IList lst, ref int i)
        {
            if (i == 0)
                draggedNode.Parent.Tag = (lst[i] as StationCategory).ParentNode;
            draggedNode.Tag = lst[i];

            if (draggedNode.Nodes.Count == 0) return;
            foreach (TreeNode var in draggedNode.Nodes)
            {
                i++;
                CopyObjToTag(var, lst, ref i);
            }
        }

        private void SaveCopyNode(TreeNode draggedNode, IList lst, ref int level)
        {

            lst.Add(draggedNode.Tag as StationCategory);
            level++;
            if (draggedNode.Nodes.Count == 0)
            {
                //StationCategory matCatTmp = new StationCategory();
                //matCatTmp = (var.Tag as StationCategory).Clone();
                //matCatTmp.ParentNode = var.Parent.Tag as StationCategory;
                //var.Tag = matCatTmp;

                return;
            }
            foreach (TreeNode var in draggedNode.Nodes)
            {
                StationCategory matCatTmp = new StationCategory();
                matCatTmp = (var.Tag as StationCategory).Clone();
                matCatTmp.ParentNode = var.Parent.Tag as StationCategory;
                var.Tag = matCatTmp;

                //matCatTmp = theMStationCategory.Save(matCatTmp);

                SaveCopyNode(var, lst, ref level);
            }
            level--;
        }

        private void MoveAfterToTreeTag(TreeNode node, StationCategory matCat)
        {
            if (node.Nodes.Count == 0 || node.Name == matCat.Id.ToString())
            {
                if (node.Name == matCat.Id.ToString())
                {
                    node.Tag = matCat;
                }
                return;
            }
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                if (node.Nodes[i].Name == matCat.Id.ToString())
                {
                    node.Nodes[i].Tag = matCat;
                    return;
                }
                else
                {
                    MoveAfterToTreeTag(node.Nodes[i], matCat);
                }
            }
        }

        private bool ContainsNode(TreeNode node1, TreeNode node2)
        {
            if (node2.Parent == null) return false;
            if (node2.Parent.Equals(node1)) return true;
            return ContainsNode(node1, node2.Parent);
        }

        private void CopyNodeTagObject(TreeNode tn)
        {
            if (tn.Nodes.Count == 0) return;
            foreach (TreeNode varTn in tn.Nodes)
            {
                StationCategory mc = varTn.Tag as StationCategory;

            }
        }


        private void tvwCategory_DragOver(object sender, DragEventArgs e)
        {
            Point targetPoint = tvwCategory.PointToClient(new Point(e.X, e.Y));
            tvwCategory.SelectedNode = tvwCategory.GetNodeAt(targetPoint);
        }

        #endregion

        public override void RefreshControls(MainViewState state)
        {
            //base.RefreshControls(state);
            ToolMenu.LockItem(ToolMenuItem.AddNew);
            ToolMenu.LockItem(ToolMenuItem.Modify);
            ToolMenu.LockItem(ToolMenuItem.Cancel);
            ToolMenu.LockItem(ToolMenuItem.Refresh);

            switch (state)
            {
                case MainViewState.Modify:
                    ObjectLock.Unlock(pnlFloor, true);
                    this.mnuTree.Items["保存节点"].Enabled = true;
                    this.mnuTree.Items["增加子节点"].Enabled = false;
                    this.mnuTree.Items["修改节点"].Enabled = false;
                    this.mnuTree.Items["删除节点"].Enabled = false;
                    break;
                case MainViewState.Browser:
                    ObjectLock.Lock(pnlFloor, true);
                    this.mnuTree.Items["保存节点"].Enabled = false;
                    this.mnuTree.Items["增加子节点"].Enabled = true;
                    this.mnuTree.Items["修改节点"].Enabled = true;
                    this.mnuTree.Items["删除节点"].Enabled = true;
                    break;
            }

            object[] fixedOS = new object[] { txtCurrentPath, txtSysCode };
            ObjectLock.Lock(fixedOS);

        }

        public override bool ModifyView()
        {
            return true;
        }

        public override bool CancelView()
        {
            try
            {
                ClearAll();

                //theMStationCategory.ReloadStationCategoryTree();
                //LoadStationCategoryTree();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        public override void RefreshView()
        {
            try
            {
                //theMStationCategory.ReloadStationCategoryTree();
                LoadStationCategoryTree();
            }
            catch (Exception e)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(e));
            }
        }

        public override bool SaveView()
        {
            try
            {
                bool isNew = false;

                if (!ValideView()) return false;
                if (theStationCategory.Id == "")
                    isNew = true;
                else
                    isNew = false;
                theStationCategory = theMStationCategory.Save(theStationCategory);
                TreeNode tn;
                if (isNew)
                {

                    tn = this.tvwCategory.SelectedNode.Nodes.Add(theStationCategory.Id.ToString(), "[" + theStationCategory.Code + "]" + theStationCategory.Name);
                    tn.Parent.Tag = theStationCategory.ParentNode;

                }
                else
                {
                    tn = this.tvwCategory.SelectedNode;
                    tn.Text = "[" + theStationCategory.Code + "]" + theStationCategory.Name;
                }
                tn.Tag = theStationCategory;

                tvwCategory.SelectedNode = tn;
                tvwCategory_AfterSelect(tvwCategory, new TreeViewEventArgs(tn));
                MessageBox.Show("保存成功！");
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }
    }
}
