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
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.QWBS
{
    public partial class VQWBSTree : TBasicDataView
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

        /// <summary>
        /// 是否是提交(还有保存)
        /// </summary>
        private bool IsSubmit = false;


        public MQWBSManagement model;

        public VQWBSTree(MQWBSManagement mot)
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
            LoadQWBSManage();
        }

        public override bool ModifyView()
        {
            //ClearTaskLevelDropDownList(MainViewState.Modify);
            RefreshControls(MainViewState.Modify);
            return true;
        }

        private void ClearTaskLevelDropDownList(MainViewState state)
        {
            this.cbLevel.Items.Clear();
            if (state == MainViewState.AddNew)
            {
                if (oprNode != null)
                {
                    List<string> list = GetChildTypeLevel(oprNode.TaskLevel.ToString());
                    foreach (string s in list)
                    {
                        cbLevel.Items.Add(s);
                    }
                    if (cbLevel.Items.Count > 0)
                        cbLevel.SelectedIndex = 0;
                }
            }
            else if (state == MainViewState.Modify)
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

            //tvwCategory.AfterCheck += new TreeViewEventHandler(tvwCategory_AfterCheck);
            tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);
            tvwCategory.MouseDown += new MouseEventHandler(tvwCategory_MouseDown);//处理没有根节点的情况
            mnuTree.ItemClicked += new ToolStripItemClickedEventHandler(mnuTree_ItemClicked);

            ////tvwCategory.ItemDrag += new ItemDragEventHandler(tvwCategory_ItemDrag);
            //tvwCategory.DragEnter += new DragEventHandler(tvwCategory_DragEnter);
            //tvwCategory.DragDrop += new DragEventHandler(tvwCategory_DragDrop);
            //tvwCategory.DragOver += new DragEventHandler(tvwCategory_DragOver);

            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.txtPriceUnit.DoubleClick +=new EventHandler(txtPriceUnit_DoubleClick);
            this.txtProjectUnit.DoubleClick +=new EventHandler(txtProjectUnit_DoubleClick);
        }


        void txtPriceUnit_DoubleClick(object sender, EventArgs e)
        {
            StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
            if (su != null)
            {
                txtPriceUnit.Tag = su;
                txtPriceUnit.Text = su.Name;
                this.txtQWBSPath.Focus();
            }
        }

        void txtProjectUnit_DoubleClick(object sender, EventArgs e)
        {
            StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
            if (su != null)
            {
                txtProjectUnit.Tag = su;
                txtProjectUnit.Text = su.Name;
                txtQWBSPath.Focus();
            }
        }

        void btnSave_Click(object sender,EventArgs e)
        {
            IsSubmit = false;
            mnuTree.Hide();
            if (!SaveView()) return;
            this.RefreshControls(MainViewState.Browser); 
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
                if (txtContractMoney.Text != "")
                {
                    oprNode.ConProMoney = ClientUtil.ToDecimal(txtContractMoney.Text);
                }
                if (txtContractPrice.Text != "")
                {
                    oprNode.ConPorPrict = ClientUtil.ToDecimal(txtContractPrice.Text);

                }
                if (txtContractQuantity.Text != "")
                {
                    oprNode.ConProQuantity = ClientUtil.ToDecimal(txtContractQuantity.Text);
                }
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
                        currNode.Tag = oprNode.ParentNode;
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
                MessageBox.Show("保存成功！");
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

        private void InitIntegrationFramework()
        {
            //cFuntions = new IntergrationFrameWork();

            //DataPackage cadImpl = new DataPackage(this, language);
            //cFuntions.Init(cadImpl);

            //BatchImportLocalize.Load(language);//初始化本地信息，集成包使用

            //isInitLocalizeBag = true;
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
                //#region 点击树节点时实现多选
                //bool isMultiSelect = false;
                //TreeNode preselectionNode;//预选择节点

                //preselectionNode = e.Node;

                //if (currNode != null && currNode.Name != preselectionNode.Name
                //    && currNode.Parent != null && preselectionNode.Parent != null && currNode.Parent.Name == preselectionNode.Parent.Name)//Name取的对象的ID
                //    isMultiSelect = true;
                //else
                //    isMultiSelect = false;

                //if (currNode != null)
                //    startNodeCheckedState = currNode.Checked;

                //if (isMultiSelect && (Control.ModifierKeys & Keys.Shift) != 0 && (Control.ModifierKeys & Keys.Control) != 0)//如果同时按下了ctrl+shift
                //{
                //    int currNodeIndex = currNode.Index;
                //    int preselectNodeIndex = preselectionNode.Index;

                //    int startIndex = currNodeIndex < preselectNodeIndex ? currNodeIndex : preselectNodeIndex;
                //    int endIndex = currNodeIndex < preselectNodeIndex ? preselectNodeIndex : currNodeIndex;

                //    for (int i = startIndex; i <= endIndex; i++)
                //    {
                //        TreeNode tn = currNode.Parent.Nodes[i];

                //        isSelectNodeInvoke = true;//设置标志，在check事件中不再处理

                //        if (startNodeCheckedState)//如果起始节点当前为选中，就取消选择
                //        {
                //            TreeNode tempNode = new TreeNode();
                //            tn.BackColor = tempNode.BackColor;
                //            tn.ForeColor = tempNode.ForeColor;

                //            if (listCheckedNode.ContainsKey(tn.Name))
                //                listCheckedNode.Remove(tn.Name);

                //            tn.Checked = false;
                //        }
                //        else//如果起始节点当前为未选中，就设置选择
                //        {
                //            tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                //            tn.ForeColor = ColorTranslator.FromHtml("#000000");

                //            if (listCheckedNode.ContainsKey(tn.Name))
                //                listCheckedNode[tn.Name] = tn;
                //            else
                //                listCheckedNode.Add(tn.Name, tn);

                //            tn.Checked = true;
                //        }

                //        SetChildCheckedByMultiSel(tn);
                //    }
                //}
                //else if (isMultiSelect && (Control.ModifierKeys & Keys.Shift) != 0)//如果同时按下了shift
                //{


                //    int currNodeIndex = currNode.Index;
                //    int preselectNodeIndex = preselectionNode.Index;

                //    int startIndex = currNodeIndex < preselectNodeIndex ? currNodeIndex : preselectNodeIndex;
                //    int endIndex = currNodeIndex < preselectNodeIndex ? preselectNodeIndex : currNodeIndex;

                //    for (int i = startIndex; i <= endIndex; i++)
                //    {
                //        TreeNode tn = currNode.Parent.Nodes[i];

                //        isSelectNodeInvoke = true;//设置标志，在check事件中不再处理

                //        if (startNodeCheckedState)//如果起始节点当前为选中，就取消选择
                //        {
                //            tn.Checked = false;

                //            TreeNode tempNode = new TreeNode();
                //            tn.BackColor = tempNode.BackColor;
                //            tn.ForeColor = tempNode.ForeColor;

                //            if (listCheckedNode.ContainsKey(tn.Name))
                //                listCheckedNode.Remove(tn.Name);
                //        }
                //        else//如果起始节点当前为未选中，就设置选择
                //        {
                //            tn.Checked = true;

                //            tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                //            tn.ForeColor = ColorTranslator.FromHtml("#000000");

                //            if (listCheckedNode.ContainsKey(tn.Name))
                //                listCheckedNode[tn.Name] = tn;
                //            else
                //                listCheckedNode.Add(tn.Name, tn);
                //        }
                //    }
                //}
                //#endregion
                
                currNode = tvwCategory.SelectedNode;

                oprNode = currNode.Tag as QWBSManage;
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
                ClearAll();
                modify();
                //ClearTaskLevelDropDownList(MainViewState.Modify);
                RefreshControls(MainViewState.Browser);
            }
            catch (Exception exp)
            {
                MessageBox.Show("显示出错：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        /// <summary>
        /// 根据父类型级别获取子类型级别集合
        /// </summary>
        /// <param name="parentStructType"></param>
        /// <returns></returns>
        private List<string> GetChildTypeLevel(string parentTypeLevel)
        {
            List<string> list = new List<string>();
            switch (parentTypeLevel)
            {
                case "项目":
                    list.Add("单位工程");
                    break;
                case "单位工程":
                    list.Add("子单位工程");
                    list.Add("专业");
                    break;
                case "子单位工程":
                    list.Add("专业");
                    list.Add("分部工程");
                    break;
                case "专业":
                    list.Add("子单位工程");
                    list.Add("分部工程");
                    break;
                case "分部工程":
                    list.Add("子分部工程");
                    list.Add("分项工程");
                    break;
                case "子分部工程":
                    list.Add("分项工程");
                    break;
                default:
                    break;
            }
            return list;
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

        private void UpdateNode()
        {
            try
            {
                //QWBSManage currNode = tnCurrNode.Tag as QWBSManage;

                //if (currNode.ParentNode == null)
                //{
                //    currNode.Name = this.txtName.Text;
                //    tnCurrNode.Tag = currNode;
                //    return;
                //}
                //currNode.Name = this.txtName.Text;
                //tnCurrNode.Tag = currNode;

                //currNode.SysCode = currNode.ParentNode.SysCode;
            }
            catch (Exception exp)
            {
                MessageBox.Show("保存出错：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        private void DeleteNode()
        {
            try
            {
                if (!ValideDelete())
                    return;
                bool reset = false;
                //父节点只有这一个子节点，并且父节点有权限操作，删除后要重新设置父节点tag
                if (tvwCategory.SelectedNode.Parent.Nodes.Count == 1)// && ConstMethod.Contains(lstInstance, tvwCategory.SelectedNode.Parent.Tag as CategoryNode)
                {
                    reset = true;
                }
                model.DeleteQWBSManage(oprNode);
                //删除日志
                LogData log = new LogData();
                log.BillId = oprNode.Id;
                log.BillType = "清单任务";
                log.Code = "任务名称：" + oprNode.Name;
                log.OperType = "删除";
                log.Descript = "清单任务删除记录";
                log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                log.ProjectName = oprNode.ProjectName;
                StaticMethod.InsertLogData(log);

                if (reset)
                {
                    QWBSManage org = model.GetQWBSManageById((tvwCategory.SelectedNode.Parent.Tag as QWBSManage).Id);
                    if (org != null)
                        tvwCategory.SelectedNode.Parent.Tag = org;
                }

                //如果复制的节点有勾选的加入到选中集合
                if (tvwCategory.SelectedNode.Checked)
                {
                    if (listCheckedNode.ContainsKey(tvwCategory.SelectedNode.Name))
                        listCheckedNode.Remove(tvwCategory.SelectedNode.Name);

                    RemoveChildChecked(tvwCategory.SelectedNode);
                }

                this.tvwCategory.Nodes.Remove(this.tvwCategory.SelectedNode);
            }
            catch (Exception exp)
            {
                string message = exp.Message;
                Exception ex1 = exp.InnerException;
                while (ex1 != null && !string.IsNullOrEmpty(ex1.Message))
                {
                    message += ex1.Message;

                    ex1 = ex1.InnerException;
                }

                if (message.IndexOf("违反") > -1 && message.IndexOf("约束") > -1)
                {
                    MessageBox.Show("该节点被工程WBS或其它数据所引用，删除前请先删除引用的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("删除节点出错：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        private bool ValideDelete()
        {
            try
            {
                TreeNode tn = tvwCategory.SelectedNode;
                if (tn == null)
                {
                    MessageBox.Show("请先选择要删除的节点！");
                    return false;
                }
                if (tn.Parent == null)
                {
                    MessageBox.Show("根节点不允许删除！");
                    return false;
                }
                string text = "要删除当前选中的节点吗？该操作将连它的所有子节点一同删除！";
                if (MessageBox.Show(text, "删除节点", MessageBoxButtons.YesNo) == DialogResult.No)
                    return false;

                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
                return false;
            }
        }

        void add_Click(object sender, EventArgs e)
        {
            try
            {
                IsInsertNode = false;
                ClearAll();
                //ClearTaskLevelDropDownList(MainViewState.AddNew);
                oprNode = new QWBSManage();
                if (projectInfo != null)
                {
                    oprNode.ProjectId = projectInfo.Id;
                    oprNode.ProjectName = projectInfo.Name;
                }
                oprNode.ParentNode = this.tvwCategory.SelectedNode.Tag as QWBSManage;
                oprNode.Code = oprNode.ParentNode.Code; //model.GetCode(typeof(QWBSManage));
               
                oprNode.CreateDate = DateTime.Now;
                oprNode.OwnerGUID = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;
                oprNode.OwnerName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                oprNode.OwnerOrgSysCode = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.SysCode;//组织层次码
                oprNode.TaskState = QWBSState.制定;
               
                
                txtQWBSPath.Text = tvwCategory.SelectedNode.FullPath + "\\";
            }
            catch (Exception exp)
            {
                MessageBox.Show("增加节点出错：" + exp.Message);
            }
        }

        void delete_Click(object sender, EventArgs e)
        {
            DeleteNode();
        }

        void saveItem_Click(object sender, EventArgs e)
        {
            SaveView();
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

        private void btnApp_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateNode();
            }
            catch (Exception exp)
            {
                MessageBox.Show("保存组织树错误：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        public void Start()
        {
            RefreshState(MainViewState.Browser);

            tvwCategory.ItemDrag += new ItemDragEventHandler(tvwCategory_ItemDrag);
            //tvwCategory.DragEnter += new DragEventHandler(tvwCategory_DragEnter);
            //tvwCategory.DragDrop += new DragEventHandler(tvwCategory_DragDrop);
            //tvwCategory.DragOver += new DragEventHandler(tvwCategory_DragOver);

            LoadQWBSManage();
        }

        private void LoadQWBSManage()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                Controlfales();
                tvwCategory.Nodes.Clear();
                IList list = model.GetQWBSManageByInstance(projectInfo.Id);
                //lstInstance = listAll[1] as IList;
                //IList list = listAll[0] as IList;

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
                            //lstInstance = listAll[1] as IList;
                            //list = listAll[0] as IList;
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
                    //lstInstance = listAll[1] as IList;
                    //list = listAll[0] as IList;
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
                if (list.Count > 0 && this.tvwCategory.Nodes.Count > 0)
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
        //控件内容不可用
        private void Controlfales()
        {
            this.txtContractMoney.Enabled = false;
            this.txtContractPrice.Enabled = false;
            this.txtContractQuantity.Enabled = false;
            this.txtPriceUnit.Enabled = false;
            this.txtProjectUnit.Enabled = false;
            this.txtQWBSCharacter.Enabled = false;
            this.dtEndTime.Enabled = false;
            this.dtStartTime.Enabled = false;
            this.txtQWBSDesc.Enabled = false;
            this.cbLevel.Enabled = false;
            this.txtQWBSName.Enabled = false;
            this.txtQWBSPath.Enabled = false;
            this.txtQWBSState.Enabled = false;
        }
        //控件内容可用
        private void Controltrue()
        {
            this.txtContractMoney.Enabled = true;
            this.txtContractPrice.Enabled = true;
            this.txtContractQuantity.Enabled = true;
            this.txtPriceUnit.Enabled = true;
            this.txtProjectUnit.Enabled = true;
            this.dtStartTime.Enabled = true;
            this.dtEndTime.Enabled = true;
            this.txtQWBSCharacter.Enabled = true;
            this.txtQWBSDesc.Enabled = true;
            this.cbLevel.Enabled = true;
            this.txtQWBSName.Enabled = true;
            this.txtQWBSPath.Enabled = true;
            this.txtQWBSState.Enabled = true;
        }
        //清除信息
        private void ClearAll()
        {
            this.txtContractMoney.Text = "";
            this.txtContractPrice.Text = "";
            this.txtContractQuantity.Text = "";
            this.txtPriceUnit.Text = "";
            this.txtProjectUnit.Text = "";
            this.txtQWBSCharacter.Text = "";
            this.txtQWBSDesc.Text = "";
            this.txtQWBSName.Text = "";
            this.txtQWBSPath.Text = "";
            this.txtQWBSState.Text = "";
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


        //刷新控件信息
        public override void RefreshControls(MainViewState state)
        {
            switch (state)
            {
                case MainViewState.Modify:

                    this.mnuTree.Items["撤销"].Enabled = true;
                    this.mnuTree.Items["保存节点"].Enabled = true;
                    this.mnuTree.Items["增加子节点"].Enabled = false;
                    this.mnuTree.Items["插入同级节点"].Enabled = false;
                    this.mnuTree.Items["修改节点"].Enabled = false;
                    this.mnuTree.Items["删除节点"].Enabled = false;
                    modify();
                    Controltrue();
                    break;
                case MainViewState.Browser:
                    this.mnuTree.Items["撤销"].Enabled = false;
                    this.mnuTree.Items["保存节点"].Enabled = false;
                    this.mnuTree.Items["增加子节点"].Enabled = true;
                    this.mnuTree.Items["修改节点"].Enabled = true;
                    if (currNode != null && currNode.Parent == null)
                    {
                        this.mnuTree.Items["插入同级节点"].Enabled = false;
                        this.mnuTree.Items["删除节点"].Enabled = false;
                    }
                    else
                    {
                        this.mnuTree.Items["插入同级节点"].Enabled = true;
                        this.mnuTree.Items["删除节点"].Enabled = true;
                    }
                    Controlfales();
                    break;
            }
        }
        //针对树的增删改操作
        void mnuTree_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text.Trim() == "增加子节点")
            {
                RefreshControls(MainViewState.Modify);
                add_Click(null, new EventArgs());
            }
            if (e.ClickedItem.Text.Trim() == "插入同级节点")
            {
                RefreshControls(MainViewState.Modify);
                InsertBrotherNode();
            }
            else if (e.ClickedItem.Text.Trim() == "修改节点")
            {
                RefreshControls(MainViewState.Modify);
            }
            else if (e.ClickedItem.Text.Trim() == "删除节点")
            {
                mnuTree.Hide();
                delete_Click(null, new EventArgs());
                RefreshControls(MainViewState.Browser);
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
                SaveView();
            }
            else if (e.ClickedItem.Text.Trim() == "复制勾选节点")
            {
                mnuTree.Hide();

                listCopyNode.Clear();
                listCopyNodeAll.Clear();

                GetCheckedNode(tvwCategory.Nodes[0]);

                //检查选择的每个根节点下的子节点之间是否连续
                foreach (TreeNode tn in listCopyNode)
                {
                    if (SelectNodeIsSuccession(tn) == false)
                    {
                        MessageBox.Show("节点“" + tn.FullPath + "”下选择了不连续的子节点，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tn.ExpandAll();

                        listCopyNode.Clear();
                        listCopyNodeAll.Clear();

                        return;
                    }
                }

                //判断选择的每个根节点是否是同一个父节点
                for (int i = 0; i < listCopyNode.Count - 1; i++)
                {
                    TreeNode nodePrev = listCopyNode[i];
                    TreeNode nodeNext = listCopyNode[i + 1];

                    if (nodePrev.Parent.Name != nodeNext.Parent.Name)
                    {
                        MessageBox.Show("选择的多个顶级节点不归属同一父节点，这不符合拷贝规则，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        listCopyNode.Clear();
                        listCopyNodeAll.Clear();

                        return;
                    }
                }

                RefreshControls(MainViewState.Check);
            }
            else if (e.ClickedItem.Text.Trim() == "粘贴节点")
            {
                mnuTree.Hide();

                #region 校验拷贝规则

                //校验不能跨级别拷贝
                int levelValue = (int)oprNode.TaskLevel;
                QWBSManage copyObj = listCopyNode[0].Tag as QWBSManage;
                if ((levelValue + 1) != (int)copyObj.TaskLevel)
                {
                    MessageBox.Show("选择节点的级别和要粘贴的节点的级别的不符合父子关系！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                #endregion

                SaveCopyNode();

                RefreshControls(MainViewState.Check);
            }
            else if (e.ClickedItem.Text.Trim() == "删除勾选节点")
            {
                mnuTree.Hide();
                DeleteCheckedNode();

                RefreshControls(MainViewState.Check);
            }
            else if (e.ClickedItem.Text.Trim() == "清除勾选节点")
            {
                mnuTree.Hide();
                ClearSelectedNode(tvwCategory.Nodes[0]);

                listCheckedNode.Clear();

                RefreshControls(MainViewState.Check);
            }
        }


        private void InsertBrotherNode()
        {
            try
            {
                IsInsertNode = true;
                ClearAll();
                oprNode = new QWBSManage();
                oprNode.ParentNode = (currNode.Parent.Tag as QWBSManage);
                oprNode.Code = oprNode.ParentNode.Code;// model.GetCode(typeof(QWBSManage));

                if (projectInfo != null)
                {
                    oprNode.ProjectId = projectInfo.Id;
                    oprNode.ProjectName = projectInfo.Name;
                }
                txtQWBSName.Text = oprNode.TaskName;//树节点名称
                txtContractMoney.Text = ClientUtil.ToString(oprNode.CostSumMoney);//成本测算合价
                txtContractQuantity.Text = ClientUtil.ToString(oprNode.ConProQuantity);//合同签订工程量
                txtContractPrice.Text = ClientUtil.ToString(oprNode.ConPorPrict);//合同签订综合单价
                txtProjectUnit.Text = oprNode.ProjectUnitName;//工程量计量单位名称
                txtProjectUnit.Tag = oprNode.ProjectUnit;//工程量计量单位
                txtQWBSCharacter.Text = oprNode.TaskCharacter;//清单任务项目特征
                txtPriceUnit.Text = oprNode.ProjectUnitName;//价格计量单位名称
                txtPriceUnit.Tag = oprNode.PriceUnit;//价格计量单位
                txtQWBSDesc.Text = oprNode.TaskDigest;//清单任务摘要
                dtStartTime.Value = oprNode.RequiredStartDate;//任务要求开始时间
                dtEndTime.Value = oprNode.RequiredEndDate;//任务要求结束时间
                txtQWBSPath.Text = tvwCategory.SelectedNode.FullPath + "\\";
            }
            catch (Exception exp)
            {
                MessageBox.Show("增加节点出错：" + exp.Message);
            }
        }
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
        private void DeleteCheckedNode()
        {
            try
            {
                IList list = new ArrayList();
                foreach (var dic in listCheckedNode)
                {
                    if (dic.Value.Parent == null)
                    {
                        MessageBox.Show("根节点不允许删除！");
                        return;
                    }
                    list.Add(dic.Value.Tag as QWBSManage);
                }

                string text = "要删除勾选的所有节点吗？该操作将连它们的所有子节点一同删除！";
                if (MessageBox.Show(text, "删除节点", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

                if (model.DeleteQWBSManage(list))//删除成功
                {
                    //从PBS树上移除对应的节点
                    foreach (QWBSManage pbs in list)
                    {
                        foreach (TreeNode tn in tvwCategory.Nodes)
                        {
                            if (tn.Name == pbs.Id)
                            {
                                tvwCategory.Nodes.Remove(tn);
                                break;
                            }

                            if (tn.Nodes.Count > 0)
                            {
                                if (RemoveTreeNode(tn, pbs))
                                    break;
                            }
                        }
                    }
                    listCheckedNode.Clear();
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Exception ex1 = ex.InnerException;
                while (ex1 != null && !string.IsNullOrEmpty(ex1.Message))
                {
                    message += ex1.Message;

                    ex1 = ex1.InnerException;
                }

                if (message.IndexOf("违反") > -1 && message.IndexOf("约束") > -1)
                {
                    MessageBox.Show("勾选节点中有节点被工程WBS或其它数据所引用，删除前请先删除引用的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show(ExceptionUtil.ExceptionMessage(ex));
            }
        }
        private void SaveCopyNode()
        {
            if (listCopyNode.Count > 0)
            {
                IList lst = new ArrayList();
                foreach (TreeNode node in listCopyNode)
                {
                    QWBSManage catTmp = (node.Tag as QWBSManage).Clone();
                    if (projectInfo != null)
                    {
                        catTmp.ProjectId = projectInfo.Id;
                        catTmp.ProjectName = projectInfo.Name;
                    }
                    QWBSManage parentNode = oprNode;// tvwCategory.SelectedNode.Tag as QWBSManage
                    catTmp.ParentNode = parentNode;
                    catTmp.Code = getChildCode(tvwCategory.SelectedNode, (node.Tag as QWBSManage).Code); //model.GetCode(typeof(QWBSManage));
                    catTmp.OrderNo = model.GetMaxOrderNo(parentNode) + 1;
                    lst.Add(catTmp);
                    GetCopyNode(node, catTmp, ref lst);
                }
                //保存复制的节点
                lst = model.SaveQWBSManages(lst);
                //新增节点要有权限操作
                //(lstInstance as ArrayList).AddRange(lst);
                //给复制节点的新父节点tag设值
                oprNode = (lst[0] as QWBSManage).ParentNode as QWBSManage;
                tvwCategory.SelectedNode.Tag = oprNode;
                IEnumerable<QWBSManage> listCopyPBS = lst.OfType<QWBSManage>();
                IEnumerable<QWBSManage> listCopyRoot = from n in listCopyPBS
                                                       where n.ParentNode.Id == oprNode.Id
                                                       select n;
                foreach (QWBSManage pbs in listCopyRoot)
                {
                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = pbs.Id.ToString();
                    tnTmp.Text = pbs.Name;
                    tnTmp.Tag = pbs;
                    tvwCategory.SelectedNode.Nodes.Add(tnTmp);
                    AddCopyChildNode(tnTmp, pbs, listCopyPBS);
                }
                tvwCategory.SelectedNode.Expand();
                //listCopyNode.Clear();
            }
        }
        private bool RemoveTreeNode(TreeNode parentNode, QWBSManage pbs)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Name == pbs.Id)
                {
                    parentNode.Nodes.Remove(tn);
                    return true;
                }
                if (tn.Nodes.Count > 0)
                {
                    if (RemoveTreeNode(tn, pbs))
                        return true;
                }
            }
            return false;
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
        /// <summary>
        /// 获取要复制的节点
        /// </summary>
        private void GetCopyNode(TreeNode copyParentNode, QWBSManage saveParentNode, ref IList list)
        {
            foreach (TreeNode node in copyParentNode.Nodes)
            {
                if (listCopyNodeAll.Keys.Contains(node.Name))
                {
                    QWBSManage catTmp = (node.Tag as QWBSManage).Clone();
                    if (projectInfo != null)
                    {
                        catTmp.ProjectId = projectInfo.Id;
                        catTmp.ProjectName = projectInfo.Name;
                    }
                    catTmp.Code = model.GetCode(typeof(QWBSManage));
                    catTmp.ParentNode = saveParentNode;
                    //catTmp.OrderNo = model.GetMaxOrderNo(parentNode) + 1;//排序号就用复制节点的排序号
                    list.Add(catTmp);
                    GetCopyNode(node, catTmp, ref list);
                }
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
