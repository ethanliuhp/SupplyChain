using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Linq;

using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Main;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonForm;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using ImportIntegration;
using System.IO;
using com.think3.PLM.Integration.DataTransfer;
using System.Diagnostics;
using com.think3.PLM.Integration.Client.WS;
using Application.Business.Erp.SupplyChain.CostManagement.QWBS.Domain;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.QWBS
{
    public partial class VQWBSSelect : TBasicDataView
    {
        private TreeNode currNode;
        private QWBSManage oprNode = null;
        private bool isNew = true;
        //有权限的业务组织
        private IList lstInstance;
        //唯一编码
        private string uniqueCode;
        private Hashtable hashtableRules = new Hashtable();
        private Dictionary<string, TreeNode> listCheckedNode = new Dictionary<string, TreeNode>();
        Dictionary<int, string> listTaskTypeLevel = new Dictionary<int, string>();
        /// <summary>
        /// 复制的顶级节点集合
        /// </summary>
        private List<TreeNode> listCopyNode = new List<TreeNode>();
        /// <summary>
        /// 复制的所有子节点集合，用于清除选择的节点时还能找到复制的节点
        /// </summary>
        Dictionary<string, TreeNode> listCopyNodeAll = new Dictionary<string, TreeNode>();
        CurrentProjectInfo projectInfo = null;
        /// <summary>
        /// 是否是插入的节点
        /// </summary>
        private bool IsInsertNode = false;
        private List<TreeNode> _SelectResult = new List<TreeNode>();
        /// <summary>
        /// 获取选择的节点集合
        /// </summary>
        public List<TreeNode> SelectResult
        {
            get { return _SelectResult; }
            set { _SelectResult = value; }
        }
        /// <summary>
        /// 是否是提交(还有保存)
        /// </summary>
        private bool IsSubmit = false;


        public MQWBSManagement model;

        public VQWBSSelect(MQWBSManagement mot)
        {
            model = mot;
            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();
            cbLevel.Items.Add("");
            foreach (string level in Enum.GetNames(typeof(QWBSLevel)))
            {
                cbLevel.Items.Add(level);

                listTaskTypeLevel.Add((int)VirtualMachine.Component.Util.EnumUtil<QWBSLevel>.FromDescription(level), level);
            }
            cbLevel.SelectedIndex = 0;
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            RefreshState(MainViewState.Browser);
            tvwCategory.CheckBoxes = false;
            LoadQWBSManage();
        }

        public override bool ModifyView()
        {
            ClearTaskLevelDropDownList(MainViewState.Modify);
            RefreshControls(MainViewState.Modify);
            return true;
        }

        private void ClearTaskLevelDropDownList(MainViewState state)
        {
            this.cbLevel.Items.Clear();
            if (state == MainViewState.Modify)
            {
                if (oprNode != null)
                {
                    cbLevel.Items.Add(oprNode.TaskLevel.ToString());
                    if (cbLevel.Items.Count > 0)
                        cbLevel.SelectedIndex = 0;
                }
            }
            else if (state == MainViewState.Browser)
            {
                foreach (var dic in listTaskTypeLevel)
                {
                    cbLevel.Items.Add(dic.Value);
                }

                if (cbLevel.Items.Count > 0)
                    cbLevel.SelectedIndex = 0;
            }
        }

        private void InitEvents()
        {
            tvwCategory.NodeMouseClick += new TreeNodeMouseClickEventHandler(tvwCategory_NodeMouseClick);
            tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);
            tvwCategory.MouseDown += new MouseEventHandler(tvwCategory_MouseDown);//处理没有根节点的情况
            this.btnOk.Click +=new EventHandler(btnOk_Click);
            this.btnCancel.Click +=new EventHandler(btnCancel_Click);

        }

        void btnOk_Click(object sender,EventArgs e)
        {
            if (oprNode == null)
            {
                MessageBox.Show("请选择一个节点", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TreeNode selectNode = tvwCategory.SelectedNode;
            if (selectNode.Nodes.Count > 0)
                selectNode.Nodes.Clear();
            SelectResult.Add(selectNode);
             this.Close();
        }


        void btnCancel_Click(object sender,EventArgs e)
        {
            this.Close();
 
        }

        void tvwCategory_MouseDown(object sender, MouseEventArgs e)
        {
            if (tvwCategory.Nodes.Count == 0 && e.Button == MouseButtons.Right)
            {
                modify();
            }
        }

        void tvwCategory_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)// && ConstMethod.Contains(lstInstance, tvwCategory.SelectedNode.Tag as CategoryNode)
            {
                tvwCategory.SelectedNode = e.Node;

                if (listCheckedNode.Count == 0)
                {
                    mnuTree.Items["复制勾选节点"].Enabled = false;
                    mnuTree.Items["删除勾选节点"].Enabled = false;
                    mnuTree.Items["清除勾选节点"].Enabled = false;
                }
                else
                {
                    mnuTree.Items["复制勾选节点"].Enabled = true;
                    mnuTree.Items["删除勾选节点"].Enabled = true;
                    mnuTree.Items["清除勾选节点"].Enabled = true;
                }

                if (listCopyNode.Count == 0)
                    mnuTree.Items["粘贴节点"].Enabled = false;
                else
                    mnuTree.Items["粘贴节点"].Enabled = true;

                if (e.Node.Parent == null)
                {
                    mnuTree.Items["删除节点"].Enabled = false;
                    mnuTree.Items["插入同级节点"].Enabled = false;
                }

                mnuTree.Show(tvwCategory, new Point(e.X, e.Y));
            }
        }

        public override bool SaveView()
        {
            bool isNew = false;
            try
            {
                if (!ValideSave())
                    return false;
                oprNode.PriceUnit = txtPriceUnit.Tag as StandardUnit;
                oprNode.PriceUnitName = txtPriceUnit.Text;
                oprNode.ProjectTaskName = txtQWBSName.Text;//清单任务名称
                oprNode.ProjectUnit = txtProjectUnit.Tag as StandardUnit;
                oprNode.ProjectUnitName = txtProjectUnit.Text;
                oprNode.TaskLevel = EnumUtil<QWBSLevel>.FromDescription(cbLevel.SelectedItem);
                oprNode.TaskName = txtQWBSName.Text;
                oprNode.TaskDigest = txtQWBSDesc.Text;
                oprNode.TaskCharacter = txtQWBSCharacter.Text;
                oprNode.ConProMoney = ClientUtil.ToDecimal(txtContractMoney.Text);
                oprNode.ConPorPrict = ClientUtil.ToDecimal(txtContractPrice.Text);
                oprNode.ConProQuantity = ClientUtil.ToDecimal(txtContractQuantity.Text);
                if (oprNode.Id == null)
                {
                    isNew = true;
                    oprNode.Code = model.GetCode(typeof(QWBSManage));
                    
                    if (IsInsertNode)
                    {
                        IList list = new ArrayList();
                        long orderNo = (currNode.Tag as QWBSManage).OrderNo;
                        oprNode.OrderNo = orderNo;
                        list.Add(oprNode);
                        TreeNode parentNode = currNode.Parent;
                        for (int i = currNode.Index; i < parentNode.Nodes.Count; i++)
                        {
                            QWBSManage pbs = parentNode.Nodes[i].Tag as QWBSManage;
                            pbs.OrderNo += 1;
                            list.Add(pbs);
                        }
                        list = model.InsertOrUpdateQWBSManages(list);
                        oprNode = list[0] as QWBSManage;
                        //插入子节点的父节点需要重新设置Tag
                        currNode.Parent.Tag = oprNode.ParentNode;
                        //更新tag
                        for (int i = currNode.Index; i < parentNode.Nodes.Count; i++)
                        {
                            QWBSManage taskType = parentNode.Nodes[i].Tag as QWBSManage;
                            foreach (QWBSManage ty in list)
                            {
                                if (ty.Id == taskType.Id)
                                {
                                    parentNode.Nodes[i].Tag = ty;
                                    break;
                                }
                            }
                        }
                        TreeNode tn = this.tvwCategory.SelectedNode.Parent.Nodes.Insert(currNode.Index, oprNode.TaskName.ToString());
                        tn.Name = oprNode.Id;
                        tn.Tag = oprNode;
                        this.tvwCategory.SelectedNode = tn;
                        tn.Expand();
                        //新增节点要有权限操作
                        //lstInstance.Add(oprNode);
                    }
                    else
                    {
                        
                        //oprNode.OrderNo = model.GetMaxOrderNo(oprNode.ParentNode) + 1;
                        oprNode = model.SaveQWBSManage(oprNode);
                    }
                }
                else
                {
                    isNew = false;
                    oprNode = model.SaveQWBSManage(oprNode);
                }
                if (isNew)
                {
                    if (!IsInsertNode)
                    {
                        //要添加子节点的节点以前没有子节点，需要重新设置Tag
                        if (tvwCategory.SelectedNode.Nodes.Count == 0)
                            tvwCategory.SelectedNode.Tag = oprNode.ParentNode;
                        TreeNode tn = this.tvwCategory.SelectedNode.Nodes.Add(oprNode.Id.ToString(), oprNode.TaskName.ToString());
                        //新增节点要有权限操作
                        //lstInstance.Add(oprNode);
                        tn.Tag = oprNode;
                        this.tvwCategory.SelectedNode = tn;
                        tn.Expand();
                    }
                }
                else
                {
                    this.tvwCategory.SelectedNode.Text = oprNode.TaskName.ToString();
                    this.tvwCategory.SelectedNode.Tag = oprNode;
                }

                this.RefreshControls(MainViewState.Browser);
                return true;
            }
            catch (Exception exp)
            {
                if (exp.InnerException != null && exp.InnerException.Message.Contains("违反唯一约束条件"))
                    MessageBox.Show("编码必须唯一！");
                else
                    MessageBox.Show("保存组织树错误：" + ExceptionUtil.ExceptionMessage(exp));
            }
            return false;
        }


        private bool ValideSave()
        {

            try
            {
                if (txtQWBSName.Text.Trim() == "")
                {
                    MessageBox.Show("清单任务名称不能为空!");
                    txtQWBSName.Focus();
                    return false;
                }
               
                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
                return false;
            }
        }

       
        private string getFileURL(FileToTransfer file)
        {
            if (file == null)
                return "";

            string fileURL = file.BaseUri;
            if (!String.IsNullOrEmpty(file.PartialUri))
            {
                if (file.PartialUri.IndexOf(".\\") == 0)
                    fileURL += file.PartialUri.Substring(2).Replace("\\", "/");
                else
                    fileURL += file.PartialUri.Replace("\\", "/");
            }

            return fileURL;
        }


        private void SetChildChecked(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                SetChildChecked(tn);
                tn.Checked = parentNode.Checked;

                if (tn.Checked)
                {
                    tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    tn.ForeColor = ColorTranslator.FromHtml("#000000");

                    //tn.BackColor = SystemColors.Control;
                    //tn.ForeColor = SystemColors.ControlText;

                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode[tn.Name] = tn;
                    else
                        listCheckedNode.Add(tn.Name, tn);
                }
                else
                {
                    TreeNode tempNode = new TreeNode();
                    tn.BackColor = tempNode.BackColor;
                    tn.ForeColor = tempNode.ForeColor;

                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode.Remove(tn.Name);
                }
            }
        }

        private void SetChildCheckedByMultiSel(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                SetChildCheckedByMultiSel(tn);

                isSelectNodeInvoke = true;

                if (startNodeCheckedState)//如果起始节点当前为选中，就取消选择
                {
                    TreeNode tempNode = new TreeNode();
                    tn.BackColor = tempNode.BackColor;
                    tn.ForeColor = tempNode.ForeColor;

                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode.Remove(tn.Name);

                    tn.Checked = false;
                }
                else//如果起始节点当前为未选中，就设置选择
                {
                    tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    tn.ForeColor = ColorTranslator.FromHtml("#000000");

                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode[tn.Name] = tn;
                    else
                        listCheckedNode.Add(tn.Name, tn);

                    tn.Checked = true;
                }
            }
        }

        private void GetChildChecked(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Checked)
                {
                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode[tn.Name] = tn;
                    else
                        listCheckedNode.Add(tn.Name, tn);
                }
                GetChildChecked(tn);
            }
        }

        private void RemoveChildChecked(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                RemoveChildChecked(tn);
                if (tn.Checked)
                {
                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode.Remove(tn.Name);
                }
            }
        }

        bool isSelectNodeInvoke = false;//是否在选择(点击)节点时调用
        bool startNodeCheckedState = false;//按shift多选兄弟节点时起始节点的选中状态
        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                currNode = tvwCategory.SelectedNode;
                oprNode = currNode.Tag as QWBSManage;
                ClearTaskLevelDropDownList(MainViewState.Modify);
                this.GetNodeDetail();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
            }
        }

        private void GetNodeDetail()
        {
            try
            {
                //ClearAll();
                modify();
                RefreshControls(MainViewState.Browser);
            }
            catch (Exception exp)
            {
                MessageBox.Show("显示出错：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

      

        private void tvwCategory_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                QWBSManage org = (e.Item as TreeNode).Tag as QWBSManage;
                //有权限的节点才允许拖动操作
                if (org != null)// && ConstMethod.Contains(lstInstance, org)
                {
                    DoDragDrop(e.Item, DragDropEffects.All);
                }
            }
        }

    
        private void ValideCode(TreeNode parentNode, string childCode, ref string errorMsg)
        {
            if (parentNode.Parent == null)//根节点编码不做校验
                return;

            errorMsg = string.Empty;

            QWBSManage parentType = parentNode.Tag as QWBSManage;
            if (parentType.Code.Length != 9)
            {
                errorMsg = "父节点编码不合法，请检查！";
                return;
            }
            else if (parentType.Code == childCode)
            {
                errorMsg = "编码不合法，请检查！";
                return;
            }

            string parentCode = parentType.Code;
            int level = parentNode.Level;
            if (level == 1)
            {
                if (childCode.IndexOf(parentCode.Substring(0, 2)) == -1)
                {
                    errorMsg = "编码不合法，请检查！";
                    return;
                }
            }
            else if (level == 2)
            {
                if (childCode.IndexOf(parentCode.Substring(0, 4)) == -1)
                {
                    errorMsg = "编码不合法，请检查！";
                    return;
                }
            }
            else if (level == 3)
            {
                if (childCode.IndexOf(parentCode.Substring(0, 6)) == -1)
                {
                    errorMsg = "编码不合法，请检查！";
                    return;
                }
            }
        }

       

        public void Start()
        {
            RefreshState(MainViewState.Browser);
            tvwCategory.ItemDrag += new ItemDragEventHandler(tvwCategory_ItemDrag);
            LoadQWBSManage();
        }

        private void LoadQWBSManage()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                //Controlfales();
                tvwCategory.Nodes.Clear();
                IList list = model.GetQWBSManageByInstance(projectInfo.Id);
                if (list.Count > 0)
                {
                    IEnumerable<QWBSManage> listTemp = from t in list.OfType<QWBSManage>()
                                                                where t.CategoryNodeType == NodeType.RootNode || t.ParentNode == null
                                                                select t;
                    if (listTemp != null && listTemp.Count() > 0 && projectInfo != null)
                    {
                        QWBSManage root = listTemp.ElementAt(0);
                        if (root.TaskLevel != QWBSLevel.项目 || root.Name != "建筑项目")//第一次加载  root.Name != projectInfo.Name || 
                        {
                            root.Name = "建筑项目";//固定为建筑项目
                            root.TaskLevel = QWBSLevel.项目;
                            root.SysCode = root.Id + ".";
                            root.ProjectId = projectInfo.Id;
                            root.ProjectName = projectInfo.Name;
                            if (string.IsNullOrEmpty(root.Code))
                                root.Code = model.GetCode(typeof(QWBSManage));
                            model.SaveQWBSManage(root);
                            list = model.GetQWBSManageByInstance(projectInfo.Id);
                        }
                    }
                }
                else
                {
                    IList listAdd = new List<QWBSManage>();
                    QWBSManage root = new QWBSManage();
                    root.Name = "建筑项目";//固定为建筑项目
                    root.Code = model.GetCode(typeof(QWBSManage));
                    root.TaskLevel = QWBSLevel.项目;
                    root.ProjectId = projectInfo.Id;
                    root.ProjectName = projectInfo.Name;
                    listAdd.Add(root);
                    model.SaveQWBSManageRootNode(listAdd);
                    list = model.GetQWBSManageByInstance(projectInfo.Id);
                }

                foreach (QWBSManage childNode in list)
                {
                    if (childNode.State == 0)
                        continue;

                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = childNode.TaskName;
                    tnTmp.Tag = childNode;
                    if (childNode.ParentNode != null)
                    {
                        TreeNode tnp = null;
                        tnp = hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                        if (tnp != null)
                            tnp.Nodes.Add(tnTmp);
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
                MessageBox.Show("查询工程任务类型出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }
     
        private void modify()
        {
            this.txtQWBSCharacter.Text = oprNode.TaskCharacter;
            this.txtQWBSDesc.Text = oprNode.TaskDigest;
            this.txtQWBSName.Text = oprNode.TaskName;
            this.txtQWBSPath.Text = tvwCategory.SelectedNode.FullPath + "\\";
            this.txtQWBSState.Text = ClientUtil.ToString(oprNode.TaskState);
            this.txtProjectUnit.Tag = oprNode.ProjectUnit;
            this.txtProjectUnit.Text = oprNode.ProjectUnitName;
            this.txtPriceUnit.Tag = oprNode.PriceUnit;
            this.txtPriceUnit.Text = oprNode.PriceUnitName;
            this.cbLevel.SelectedItem = EnumUtil<QWBSLevel>.GetDescription(oprNode.TaskLevel);
            this.txtContractQuantity.Text = ClientUtil.ToString(oprNode.ConProQuantity);
            this.txtContractPrice.Text = ClientUtil.ToString(oprNode.ConPorPrict);
            this.txtContractMoney.Text = ClientUtil.ToString(oprNode.ConProMoney);
        }


        ////刷新控件信息
        //public override void RefreshControls(MainViewState state)
        //{
        //    switch (state)
        //    {
        //        case MainViewState.Modify:

        //            this.mnuTree.Items["撤销"].Enabled = true;
        //            this.mnuTree.Items["保存节点"].Enabled = true;
        //            this.mnuTree.Items["增加子节点"].Enabled = false;
        //            this.mnuTree.Items["插入同级节点"].Enabled = false;
        //            this.mnuTree.Items["修改节点"].Enabled = false;
        //            this.mnuTree.Items["删除节点"].Enabled = false;
        //            modify();
        //            Controltrue();
        //            break;
        //        case MainViewState.Browser:
        //            this.mnuTree.Items["撤销"].Enabled = false;
        //            this.mnuTree.Items["保存节点"].Enabled = false;
        //            this.mnuTree.Items["增加子节点"].Enabled = true;
        //            this.mnuTree.Items["修改节点"].Enabled = true;
        //            if (currNode != null && currNode.Parent == null)
        //            {
        //                this.mnuTree.Items["插入同级节点"].Enabled = false;
        //                this.mnuTree.Items["删除节点"].Enabled = false;
        //            }
        //            else
        //            {
        //                this.mnuTree.Items["插入同级节点"].Enabled = true;
        //                this.mnuTree.Items["删除节点"].Enabled = true;
        //            }
        //            Controlfales();
        //            break;
        //    }
        //}
       

        private void GetCheckedNode(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Checked)//找到选择的每一个根节点
                {
                    listCopyNode.Add(tn);
                    continue;
                }
                GetCheckedNode(tn);
            }
        }
        /// <summary>
        /// 判断选择的节点及其子节点是否连续
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool SelectNodeIsSuccession(TreeNode parentNode)
        {
            //查询节点树
            var listLeafNode = from n in listCheckedNode
                               where (n.Value.Tag as QWBSManage).SysCode.IndexOf((parentNode.Tag as QWBSManage).SysCode) > -1
                               select n;
            foreach (var dic in listLeafNode)
            {
                if (listCopyNodeAll.Keys.Contains(dic.Key) == false)
                    listCopyNodeAll.Add(dic.Key, dic.Value);
                if (dic.Key != parentNode.Name)//此叶节点不是顶级节点
                {
                    TreeNode tempParent = dic.Value.Parent;
                    while (tempParent.Name != parentNode.Name)
                    {
                        if (tempParent.Checked == false)
                        {
                            return false;
                        }
                        if (listCopyNodeAll.Keys.Contains(tempParent.Name) == false)
                            listCopyNodeAll.Add(tempParent.Name, tempParent);
                        tempParent = tempParent.Parent;
                    }
                }
            }
            return true;
        }
     
      
        private void ClearSelectedNode(TreeNode parentNode)
        {
            TreeNode tempNode = new TreeNode();
            foreach (TreeNode tn in parentNode.Nodes)
            {
                tn.Checked = false;
                tn.BackColor = tempNode.BackColor;
                tn.ForeColor = tempNode.ForeColor;

                ClearSelectedNode(tn);
            }
        }
        private void AddCopyChildNode(TreeNode parentNode, QWBSManage parentPBS, IEnumerable<QWBSManage> listCopyPBS)
        {
            IEnumerable<QWBSManage> listCopyChild = from n in listCopyPBS
                                                    where n.ParentNode.Id == parentPBS.Id
                                                    select n;
            foreach (QWBSManage pbs in listCopyChild)
            {
                TreeNode tnTmp = new TreeNode();
                tnTmp.Name = pbs.Id.ToString();
                tnTmp.Text = pbs.Name;
                tnTmp.Tag = pbs;
                parentNode.Nodes.Add(tnTmp);
                AddCopyChildNode(tnTmp, pbs, listCopyPBS);
            }
        }
      
        private string getChildCode(TreeNode parentNode, string childCode)
        {
            QWBSManage taskType = parentNode.Tag as QWBSManage;

            if (parentNode.Level == 1)
            {
                childCode = taskType.Code.Substring(0, 2) + childCode.Substring(2);
            }
            if (parentNode.Level == 2)
            {
                childCode = taskType.Code.Substring(0, 4) + childCode.Substring(4);
            }
            if (parentNode.Level == 3)
            {
                childCode = taskType.Code.Substring(0, 6) + childCode.Substring(6);
            }

            return childCode;
        }



















    }
}
