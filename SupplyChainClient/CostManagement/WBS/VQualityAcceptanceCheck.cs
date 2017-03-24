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

using Application.Business.Erp.ResourceManager.Client.Main;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonForm;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
//using VirtualMachine.Core.Expression;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using System.Windows.Documents;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng;

using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using ImportIntegration;
using System.IO;
using System.Diagnostics;
using NHibernate.Criterion;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using IRPServiceModel.Domain.Document;
using Application.Business.Erp.SupplyChain.Client.FileUpload;
using IRPServiceModel.Basic;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS
{
    public partial class VQualityAcceptanceCheck : TBasicDataView
    {
        private TreeNode currNode = null;

        private GWBSTree oprNode = null;

        #region 查找节点
        private List<TreeNode> listFindNodes = new List<TreeNode>();
        private int showFindNodeIndex = 0;
        private TreeNode mouseSelectNode = null;

        private Dictionary<string, TreeNode> listCheckedNode = new Dictionary<string, TreeNode>();
        #endregion

        Hashtable SpecialHt = new Hashtable();

        private bool isDistribute = true;
        public bool IsDistribute
        {
            get { return isDistribute; }
            set { isDistribute = value; }
        }
        CurrentProjectInfo projectInfo = null;


        #region 文档操作变量


        string fileObjectType = string.Empty;
        string FileStructureType = string.Empty;

        string userName = string.Empty;
        string jobId = string.Empty;

        #endregion 文档操作

        public MGWBSTree model;

        string filePath = string.Empty;
        string objecIsGWBS = string.Empty;
        string addOrUpDate = string.Empty;

        private bool isCanAdd = false;

        private GWBSTree selTree = null;
        #region ctor
        public VQualityAcceptanceCheck(MGWBSTree mot)
        {
            model = mot;
            InitializeComponent();
            InitForm();
            Loaddgdetail();
        }

        #endregion

        void Loaddgdetail()
        {
            TreeNode node = this.tvwCategory.SelectedNode;
            if (node != null)
            {
                GWBSTree tree = node.Tag as GWBSTree;

                FilldgDtail(tree);
            }
        }

        private IList GetInsRecord(GWBSTree tree)
        {
            ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("GWBSTree", tree));
            oq.AddCriterion(Expression.Like("GWBSTreeSysCode",tree.SysCode + "%"));
            oq.AddCriterion(Expression.Eq("InspectType", 2));
            IList temp_list = model.GetInsRecord(oq);
            if (temp_list == null)
                return null;
            return temp_list;
        }

        void FilldgDtail(GWBSTree tree)
        {
            dgDetail.Rows.Clear();
            IList temp_list = GetInsRecord(tree);
            if (temp_list != null && temp_list.Count > 0)
            {
                foreach (InspectionRecord Record in temp_list)
                {
                    int rowIndex = dgDetail.Rows.Add();
                    dgDetail[colGWBSTreeName.Name, rowIndex].Value = Record.GWBSTreeName;
                    dgDetail[colCorrectiveSign.Name, rowIndex].Value = Record.CorrectiveSign;
                    dgDetail[colCheckSpecial.Name, rowIndex].Value = Record.InspectionSpecial;
                    dgDetail[colCheckConclusion.Name, rowIndex].Value = Record.InspectionConclusion;
                    dgDetail[colContent.Name, rowIndex].Value = Record.InspectionStatus;
                    dgDetail[colCheckPerson.Name, rowIndex].Value = Record.CreatePersonName;
                    dgDetail[colCheckPerson.Name, rowIndex].Tag = Record.CreatePerson;
                    dgDetail[colBearName.Name, rowIndex].Value = Record.BearTeamName;
                    dgDetail[colBearName.Name, rowIndex].Tag = Record.BearTeam;
                    dgDetail[colInspectionDate.Name, rowIndex].Value = Record.CreateDate.ToShortDateString();
                    if (Record.Id != null)
                    {
                        if (Record.DocState == DocumentState.InExecute || Record.DocState == DocumentState.InAudit)
                        {
                            dgDetail[colRecordState.Name, rowIndex].Value = "有效";
                        }
                        else
                        {
                            dgDetail[colRecordState.Name, rowIndex].Value = "编辑";
                        }
                    }
                    else
                    {
                        dgDetail[colRecordState.Name, rowIndex].Value = "编辑";
                    }
                    if (Record.InspectType == 1)
                    {
                        dgDetail[colInsType.Name, rowIndex].Value = "日常检查";
                    }
                    if (Record.InspectType == 2)
                    {
                        dgDetail[colInsType.Name, rowIndex].Value = "质量验收";
                    }
                    string strDeductionSign = ClientUtil.ToString(Record.DeductionSign);
                    string strCorrectiveSign = ClientUtil.ToString(Record.CorrectiveSign);
                    if (strCorrectiveSign.Equals("0"))
                    {
                        dgDetail[colCorrectiveSign.Name, rowIndex].Value = "不需整改";
                    }
                    if (strCorrectiveSign.Equals("1"))
                    {
                        dgDetail[colCorrectiveSign.Name, rowIndex].Value = "需要整改，未进行处理";
                    }
                    if (strCorrectiveSign.Equals("2"))
                    {
                        dgDetail[colCorrectiveSign.Name, rowIndex].Value = "需要整改，已进行处理";
                    }
                    dgDetail.Rows[rowIndex].Tag = Record;
                }

            }
            
            if (dgDetail.Rows.Count > 0)
            {
                dgDetail.CurrentCell = dgDetail.Rows[0].Cells[0];
                dgDetail_CellClick(dgDetail, new DataGridViewCellEventArgs(0, 0));
            }
            else
            {
                RefreshControls(MainViewState.Browser);
            }
        }
        private void InitForm()
        {
            try
            {

                txtNoDtlColorFlag.BackColor = ColorTranslator.FromHtml("#D7E8FE");

                tvwCategory.CheckBoxes = false;
                projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();

                InitEvents();

                IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.WBS_CheckRequire);

                cbCheckConclusion1.Items.AddRange(new object[] { "通过", "不通过" });
                cbWBSCheckRequir1.Items.Clear();
                foreach (BasicDataOptr b in list)
                {
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                    li.Text = b.BasicName;
                    li.Value = b.BasicCode;
                    cbWBSCheckRequir1.Items.Add(li);
                }

                //检查要求
              

                if (tvwCategory.Nodes.Count == 0)
                    RefreshControls(MainViewState.Initialize);
                else
                    RefreshControls(MainViewState.Browser);
                if (IsDistribute)
                {
                    NewLoadGWBSTreeTree();
                }
                else
                {
                    LoadGWBSTreeTree();
                }
                List<string> listParams = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetUploadFileParamsByMBP_IRP();
                fileObjectType = listParams[0];
                FileStructureType = listParams[1];
                userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
                jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
                //生产检查报表

                foreach (BasicDataOptr b in list)
                {
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                    li.Text = b.BasicName;
                    li.Value = b.BasicCode;
                    //  txtInspectionSpecial.Items.Add(li);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("初始化失败，详细信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                dgDocumentDetail.ReadOnly = false;
                FileSelect.ReadOnly = false;


            }
        }

        private void InitEvents()
        {
            #region GWBS树
            tvwCategory.MouseDown += new MouseEventHandler(tvwCategory_MouseDown);//处理没有根节点的情况
            tvwCategory.AfterCheck += new TreeViewEventHandler(tvwCategory_AfterCheck);
            tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);
            tvwCategory.AfterExpand += new TreeViewEventHandler(tvwCategory_AfterExpand);

            txtKeyWord.KeyDown += new KeyEventHandler(txtKeyWord_KeyDown);
            txtKeyWord.TextChanged += new EventHandler(txtKeyWord_TextChanged);
            btnFindTaskNode.Click += new EventHandler(btnFindTaskNode_Click);

            #endregion

            #region  质量验收检查
            //承担着
            this.btnSearch1.Click += new EventHandler(btnSearch1_Click);

            this.btnSave1.Click += new EventHandler(btnSave1_Click);
            this.btnSubmit1.Click += new EventHandler(btnSubmit1_Click);
            this.btnDelete1.Click += new EventHandler(btnDelete1_Click);

            this.btnAddNew.Click += new EventHandler(btnAddNew_Click);

            #endregion

            #region 节点信息区域 控件注册事件
            this.dgDetail.CellClick += new DataGridViewCellEventHandler(dgDetail_CellClick);
            this.dgDetail.MouseClick += new MouseEventHandler(dgDetail_MouseClick);
            #endregion

            this.conMenu.ItemClicked += new ToolStripItemClickedEventHandler(conMenu_ItemClicked);

            #region 相关文档
            btnDownLoadDocument.Click += new EventHandler(btnDownLoadDocument_Click);
            btnOpenDocument.Click += new EventHandler(btnOpenDocument_Click);
            btnUpdateDocument.Click += new EventHandler(btnUpdateDocument_Click);
            btnDeleteDocumentFile.Click += new EventHandler(btnDeleteDocumentFile_Click);
            btnUpFile.Click += new EventHandler(btnUpFile_Click);
            dgDocumentMast.CellClick += new DataGridViewCellEventHandler(dgDocumentMast_CellClick);
            btnDeleteDocumentMaster.Click += new EventHandler(btnDeleteDocumentMaster_Click);
            btnDocumentFileAdd.Click += new EventHandler(btnDocumentFileAdd_Click);
            btnDocumentFileUpdate.Click += new EventHandler(btnDocumentFileUpdate_Click);
            lnkCheckAll.Click += new EventHandler(lnkCheckAll_Click);
            lnkCheckAllNot.Click += new EventHandler(lnkCheckAllNot_Click);
            #endregion

            //tab页切换数据处理
            tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
        }

        

        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == 相关附件.Name)//相关文档
            {
                FillDoc();
            }
        }

        #region  GWBS树 相关事件

        #region tvwCategory_MouseDown
        void tvwCategory_MouseDown(object sender, MouseEventArgs e)
        {
            if (tvwCategory.Nodes.Count == 0 && e.Button == MouseButtons.Right)
            {
                RefreshControls(MainViewState.Initialize);
            }
        }
        #endregion

        #region tvwCategory_AfterCheck
        void tvwCategory_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (isSelectNodeInvoke)
            {
                isSelectNodeInvoke = false;
            }
            else
            {
                #region 单击复选框操作
                if ((Control.ModifierKeys & Keys.Shift) != 0 || (Control.ModifierKeys & Keys.Control) != 0)//如果同时按下了Ctrl //(Control.ModifierKeys & Keys.Shift) != 0 || 
                {
                    if (e.Node.Checked)
                    {
                        e.Node.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                        e.Node.ForeColor = ColorTranslator.FromHtml("#000000");

                        if (listCheckedNode.ContainsKey(e.Node.Name))
                            listCheckedNode[e.Node.Name] = e.Node;
                        else
                            listCheckedNode.Add(e.Node.Name, e.Node);
                    }
                    else
                    {
                        TreeNode tempNode = new TreeNode();
                        e.Node.BackColor = tempNode.BackColor;
                        e.Node.ForeColor = tempNode.ForeColor;

                        if (listCheckedNode.ContainsKey(e.Node.Name))
                            listCheckedNode.Remove(e.Node.Name);
                    }

                    SetChildChecked(e.Node);
                }
                else
                {
                    if (e.Node.Checked)
                    {
                        e.Node.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                        e.Node.ForeColor = ColorTranslator.FromHtml("#000000");


                        if (listCheckedNode.ContainsKey(e.Node.Name))
                            listCheckedNode[e.Node.Name] = e.Node;
                        else
                            listCheckedNode.Add(e.Node.Name, e.Node);
                    }
                    else
                    {
                        TreeNode tempNode = new TreeNode();
                        e.Node.BackColor = tempNode.BackColor;
                        e.Node.ForeColor = tempNode.ForeColor;

                        if (listCheckedNode.ContainsKey(e.Node.Name))
                            listCheckedNode.Remove(e.Node.Name);
                    }
                }

                #endregion
            }
            RefreshControls(MainViewState.Check);
        }
        #endregion

        #region tvwCategory_AfterSelect
        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                //this.btnUpdate.Enabled = true;
                ClearIns();
                #region 点击树节点时实现多选
                bool isMultiSelect = false;
                TreeNode preselectionNode;//预选择节点

                preselectionNode = e.Node;

                if (currNode != null && currNode.Name != preselectionNode.Name
                    && currNode.Parent != null && preselectionNode.Parent != null && currNode.Parent.Name == preselectionNode.Parent.Name)//Name取的对象的ID
                    isMultiSelect = true;
                else
                    isMultiSelect = false;

                if (currNode != null)
                    startNodeCheckedState = currNode.Checked;

                if (isMultiSelect && (Control.ModifierKeys & Keys.Shift) != 0 && (Control.ModifierKeys & Keys.Control) != 0)//如果同时按下了ctrl+shift
                {
                    int currNodeIndex = currNode.Index;
                    int preselectNodeIndex = preselectionNode.Index;

                    int startIndex = currNodeIndex < preselectNodeIndex ? currNodeIndex : preselectNodeIndex;
                    int endIndex = currNodeIndex < preselectNodeIndex ? preselectNodeIndex : currNodeIndex;

                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        TreeNode tn = currNode.Parent.Nodes[i];

                        isSelectNodeInvoke = true;//设置标志，在check事件中不再处理

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

                        SetChildCheckedByMultiSel(tn);
                    }
                }
                else if (isMultiSelect && (Control.ModifierKeys & Keys.Shift) != 0)//如果同时按下了shift
                {


                    int currNodeIndex = currNode.Index;
                    int preselectNodeIndex = preselectionNode.Index;

                    int startIndex = currNodeIndex < preselectNodeIndex ? currNodeIndex : preselectNodeIndex;
                    int endIndex = currNodeIndex < preselectNodeIndex ? preselectNodeIndex : currNodeIndex;

                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        TreeNode tn = currNode.Parent.Nodes[i];

                        isSelectNodeInvoke = true;//设置标志，在check事件中不再处理

                        if (startNodeCheckedState)//如果起始节点当前为选中，就取消选择
                        {
                            tn.Checked = false;

                            TreeNode tempNode = new TreeNode();
                            tn.BackColor = tempNode.BackColor;
                            tn.ForeColor = tempNode.ForeColor;

                            if (listCheckedNode.ContainsKey(tn.Name))
                                listCheckedNode.Remove(tn.Name);
                        }
                        else//如果起始节点当前为未选中，就设置选择
                        {
                            tn.Checked = true;

                            tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                            tn.ForeColor = ColorTranslator.FromHtml("#000000");

                            if (listCheckedNode.ContainsKey(tn.Name))
                                listCheckedNode[tn.Name] = tn;
                            else
                                listCheckedNode.Add(tn.Name, tn);
                        }
                    }
                }
                #endregion
                currNode = tvwCategory.SelectedNode;
                oprNode = tvwCategory.SelectedNode.Tag as GWBSTree;
                oprNode = LoadRelaAttribute(oprNode);
                this.selTree = oprNode;
           
                FilldgDtail(oprNode);

                SetCanAddQAC(oprNode);

            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
            }
        }

        private void SetCanAddQAC(GWBSTree oprNode)
        {
            IList list = GetInsRecord(oprNode);
            SetColGWBSTreeNameVis(oprNode);
            bool flag = true;
            if (list != null && list.Count > 0)
                flag = false;

           

            if (flag && oprNode.CategoryNodeType == NodeType.LeafNode)
            {
                this.isCanAdd = true;
                if (dgDetail.Rows.Count == 0 )
                {
                    this.Clear();
                }
                else
                {
                    InspectionRecord master = dgDetail.CurrentRow.Tag as InspectionRecord;
                    if( master.InspectType !=2)
                        this.Clear();
                }
                RefreshControls(MainViewState.Browser);
                this.btnAddNew.Enabled = true;
            }
            else
            {
                this.isCanAdd = false;
                if (dgDetail.Rows.Count == 0)
                    this.Clear();
                else
                {
                    InspectionRecord master = dgDetail.CurrentRow.Tag as InspectionRecord;
                    if (master.InspectType != 2)
                        this.Clear();
                }
                RefreshControls(MainViewState.Browser);
                this.btnAddNew.Enabled = false;
            }
           
        }

        private void SetColGWBSTreeNameVis(GWBSTree oprNode)
        {
            if (oprNode.CategoryNodeType == NodeType.LeafNode)
                this.colGWBSTreeName.Visible = false;
            else
                this.colGWBSTreeName.Visible = true;
        }
        #endregion

        #region tvwCategory_AfterExpand
        public void tvwCategory_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                TreeNode oTreeNode = e.Node;
                DataTable tbGwbsTress = null;
                GWBSTree oGWBSTree = null;

                if (oTreeNode.Nodes != null && oTreeNode.Nodes.Count > 0 && oTreeNode.Nodes[0].Tag == null)
                {
                    oTreeNode.Nodes.Clear();
                    oGWBSTree = oTreeNode.Tag as GWBSTree;
                    tbGwbsTress = model.GetGWBSTree(projectInfo.Id, oGWBSTree.Id, oGWBSTree.Level + 1);
                    if (tbGwbsTress != null && tbGwbsTress.Rows.Count > 0)
                    {
                        AddChildNodes(oTreeNode, tbGwbsTress);
                    }
                }
            }
        }
        #endregion

        private GWBSTree LoadRelaAttribute(GWBSTree wbs)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", wbs.Id));
            oq.AddFetchMode("ListRelaPBS", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ProjectTaskTypeGUID", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("PriceAmountUnitGUID", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof(GWBSTree), oq);
            if (list.Count > 0)
            {
                wbs = list[0] as GWBSTree;
            }
            return wbs;
        }

        public void AddChildNodes(TreeNode oParentNode, DataTable tbGWBSTress)
        {
            if (oParentNode != null)
            {
                oParentNode.Nodes.Clear();
            }
            if (tbGWBSTress != null && tbGWBSTress.Rows.Count > 0)
            {
                GWBSTree oGWBSTree = null;
                TreeNode oTreeNode = null;
                foreach (DataRow oRow in tbGWBSTress.Rows)
                {
                    oGWBSTree = CreateGWBSTree(oRow);
                    string sName = oGWBSTree.Name;
                    if (oGWBSTree != null)
                    {
                        oTreeNode = CreateNode(oGWBSTree);
                        if (oTreeNode != null)
                        {
                            if (oParentNode == null)
                            {
                                this.tvwCategory.Nodes.Add(oTreeNode);
                            }
                            else
                            {
                                oParentNode.Nodes.Add(oTreeNode);
                            }
                        }
                    }
                }

            }
        }

        void txtKeyWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyData == Keys.Enter)
            {
                btnFindTaskNode_Click(btnFindTaskNode, new EventArgs());
            }
        }

        void txtKeyWord_TextChanged(object sender, EventArgs e)
        {
            listFindNodes.Clear();
            showFindNodeIndex = 0;
        }

        //查找/下一个
        void btnFindTaskNode_Click(object sender, EventArgs e)
        {
            if (txtKeyWord.Text.Trim() == "")
                return;

            if (listFindNodes.Count > 0)
            {
                showFindNodeIndex += 1;
                if (showFindNodeIndex > listFindNodes.Count - 1)
                    showFindNodeIndex = 0;

                ShowFindNode(listFindNodes[showFindNodeIndex]);
            }
            else
            {
                string keyWord = txtKeyWord.Text.Trim();
                if (mouseSelectNode != null)
                {
                    foreach (TreeNode tn in mouseSelectNode.Nodes)
                    {
                        if (tn.Text.IndexOf(keyWord) > -1)
                        {
                            listFindNodes.Add(tn);
                        }

                        QueryCheckedTreeNode(tn, keyWord);
                    }
                }
                else
                {
                    foreach (TreeNode tn in tvwCategory.Nodes)
                    {
                        if (tn.Text.IndexOf(keyWord) > -1)
                        {
                            listFindNodes.Add(tn);
                        }

                        QueryCheckedTreeNode(tn, keyWord);
                    }
                }

                if (listFindNodes.Count > 0)
                {
                    showFindNodeIndex = 0;
                    ShowFindNode(listFindNodes[showFindNodeIndex]);
                }
            }
        }

        #endregion

        void ClearIns()
        {
            txtSupply1.Text = "";
            txtSupply1.Tag = null;
        }

        void Clear()
        {
            txtCheckPerson1.Value = ConstObject.LoginPersonInfo.Name;
            txtCheckPerson1.Result.Add(ConstObject.LoginPersonInfo);
            dtpCheckDate1.Value = DateTime.Now;
            txtCheckStatus1.Text = "";
            cbCheckConclusion1.Text = "";
            txtSupply1.Text = "";
            cbWBSCheckRequir1.Text = "";
        }

        public override void RefreshControls(MainViewState state)
        {
            switch (state)
            {
               case MainViewState.Modify:

                    //按钮
                    //界面输入控件

                    txtCheckPerson1.Enabled = true;
                    cbWBSCheckRequir1.Enabled = true;
                    cbCheckConclusion1.Enabled = true;
                    dtpCheckDate1.Enabled = true;
                    txtCheckStatus1.Enabled = true;
                    btnSubmit1.Enabled = true;
                    radioButton3.Enabled = true;
                    radioButton4.Enabled = true;
                    txtSupply1.Enabled = true;
                    this.btnSave1.Enabled = true;
                    this.btnDelete1.Enabled = true;
                    dgDocumentDetail.ReadOnly = false;
                    FileSelect.ReadOnly = false;
                    break;

                case MainViewState.Browser:

                    //按钮
                    //界面输入控件
                    txtCheckPerson1.Enabled = false;
                    cbWBSCheckRequir1.Enabled = false;
                    cbCheckConclusion1.Enabled = false;
                    dtpCheckDate1.Enabled = false;
                    txtCheckStatus1.Enabled = false;
                    btnSubmit1.Enabled = false;
                    radioButton3.Enabled = false;
                    radioButton4.Enabled = false;
                    txtSupply1.Enabled = false;
                    this.btnSave1.Enabled = false;
                    this.btnDelete1.Enabled = false;
                    dgDocumentDetail.ReadOnly = false;
                    FileSelect.ReadOnly = false;
                    break;
            }

            ViewState = state;
        }

        #region 检查记录列表相关事件
        void dgDetail_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                if (this.isCanAdd)
                {
                    conMenu.Items[ConAddIns.Name].Enabled = true;
                    conMenu.Show(MousePosition.X, MousePosition.Y);
                    this.btnAddNew.Enabled = true;
                }
                else
                    this.btnAddNew.Enabled = false;
            }

        }

        void dgDetail_CellClick(object sender, EventArgs e)
        {
            if (dgDetail.CurrentRow != null)
            {
                InspectionRecord master = dgDetail.CurrentRow.Tag as InspectionRecord;
                if (master != null)
                {
                    if (master.InspectType == 2)
                    {
                        //质量验收
                        tabPage2.Parent = tabControl1;
                        相关附件.Parent = null;
                        tabControl1.SelectedTab = tabPage2;
                        相关附件.Parent = tabControl1;
                        this.EditInspectionRecord();

                        if (master.DocState == DocumentState.InExecute)
                            RefreshControls(MainViewState.Browser);
                        else
                            RefreshControls(MainViewState.Modify);
                        this.btnAddNew.Enabled = false;
                    }
                    else
                    {
                        Clear();
                        MessageBox.Show("此检查记录不是质量验收检查！");

                        RefreshControls(MainViewState.Browser);
                    }

                    FillDoc();

                }
            }
            else
            {
                RefreshControls(MainViewState.Browser);
            }
        }

        #endregion 

        /// <summary>
        /// 在编辑框显示当前检查记录详细信息
        /// </summary>
        private void EditInspectionRecord()
        {

            if (tabControl1.SelectedTab.Name.Equals(tabPage2.Name))
            {
                if (dgDetail.CurrentRow != null)
                {
                    DataGridViewRow theCurrentRow = dgDetail.CurrentRow;
                    if (theCurrentRow.Tag != null)
                    {
                        InspectionRecord master = theCurrentRow.Tag as InspectionRecord;
                        if (master.CreatePersonName != null)
                        {
                            txtCheckPerson1.Result.Clear();
                            txtCheckPerson1.Result.Add(master.CreatePerson);
                            txtCheckPerson1.Value = master.CreatePersonName;
                        }
                        else
                        {
                            this.txtCheckPerson1.Result.Clear();
                            this.txtCheckPerson1.Result.Add(ConstObject.LoginPersonInfo);
                            this.txtCheckPerson1.Value = ConstObject.LoginPersonInfo.Name;
                        }
                        if (master.BearTeam != null)
                        {
                            txtSupply1.Tag = null;
                            txtSupply1.Text = master.BearTeamName;
                        }
                        else
                        {
                            txtSupply1.Text = "";
                            txtSupply1.Tag = null;
                        }
                        cbWBSCheckRequir1.Text = master.InspectionSpecial;
                        cbCheckConclusion1.Text = master.InspectionConclusion;
                        if (master.CreateDate <= Convert.ToDateTime("1900-1-1 00:00:00"))
                        {
                            dtpCheckDate1.Value = DateTime.Now;
                        }
                        else
                        {
                            dtpCheckDate1.Value = master.CreateDate;
                        }
                        string strDeductionSign = ClientUtil.ToString(master.DeductionSign);
                        string strCorrectiveSign = ClientUtil.ToString(master.CorrectiveSign);
                        if (strCorrectiveSign.Equals("1") || strCorrectiveSign.Equals("2"))
                        {
                            radioButton3.Checked = true;
                        }
                        else
                        {
                            radioButton4.Checked = true;
                        }
                        txtCheckStatus1.Text = master.InspectionStatus;
                        if (master.Id != null)
                        {
                            if ((master.DocState == DocumentState.InAudit || master.DocState == DocumentState.InExecute) || master.CreatePerson.Id != ConstObject.LoginPersonInfo.Id)
                            {
                                //信息为提交状态，信息不可编辑
                                RefreshControls(MainViewState.Browser);
                            }
                            else
                            {
                                RefreshControls(MainViewState.Modify);
                            }
                        }
                        else
                        {
                            RefreshControls(MainViewState.Modify);
                        }
                    }
                }
            }
        }

        #region 质量验收检查 相关控件事件
        void btnSearch1_Click(object sender, EventArgs e)
        {
            DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
            VContractExcuteSelector vmros = new VContractExcuteSelector();
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject engineerMaster = list[0] as SubContractProject;
            txtSupply1.Text = engineerMaster.BearerOrgName;
            txtSupply1.Tag = engineerMaster;
        }

        void btnSave1_Click(object sender, EventArgs e)
        {
            //校验数据
            if (!SetMessage1()) return;
            try
            {
                this.SaveInsRecord("保存");
                FilldgDtail(tvwCategory.SelectedNode.Tag as GWBSTree);
            }
            catch (Exception err)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(err));
                return;
            }
        }

        void btnDelete1_Click(object sender, EventArgs e)
        {
            bool flag = false;
            if (dgDetail.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    for (int i = 0; i < dgDetail.SelectedRows.Count; i++)
                    {
                        if ((dgDetail.SelectedRows[i].Tag as InspectionRecord).Id != null)
                        {
                            InspectionRecord record = dgDetail.SelectedRows[i].Tag as InspectionRecord;
                            if (record.CreatePerson != null)
                            {
                                if (record.CreatePerson.Id != ConstObject.LoginPersonInfo.Id)
                                {
                                    MessageBox.Show("非登录人检查的信息，不可删除！");
                                    return;
                                }

                                if (record.DocState == DocumentState.InAudit || record.DocState == DocumentState.InExecute)
                                {
                                    MessageBox.Show("信息已经提交，不可删除！");
                                    return;
                                }
                                //删除信息
                                if (!model.DeleteInspectionRecord(dgDetail.SelectedRows[i].Tag as InspectionRecord)) return;
                            }
                        }
                        else
                        {
                            dgDetail.Rows.Remove(dgDetail.SelectedRows[i]);
                        }
                        flag = true;
                    }
                    if (flag)
                    {
                        MessageBox.Show("删除成功！");
                        FilldgDtail(tvwCategory.SelectedNode.Tag as GWBSTree);
                    }
                }
            }
        }

        void btnSubmit1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.Selected)
                {
                    if (var.Cells[colCheckPerson.Name].Value == null)
                    {
                        if (!SetMessage()) return;
                        //SaveInspectionRecord("提交");
                        FilldgDtail(tvwCategory.SelectedNode.Tag as GWBSTree);
                    }
                    else
                    {
                        InspectionRecord record = dgDetail.CurrentRow.Tag as InspectionRecord;
                        if (record.Id != null)
                        {
                            if (record.DocState == DocumentState.InExecute)
                            {
                                MessageBox.Show("信息已提交！");
                                return;
                            }
                            record.DocState = DocumentState.InExecute;
                            record = model.SaveInspectialRecordMaster(record,true);
                            MessageBox.Show("提交成功！");
                            RefreshControls(MainViewState.Browser);
                            FilldgDtail(tvwCategory.SelectedNode.Tag as GWBSTree);
                        }
                    }
                }
            }
        }

        void conMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (dgDetail.Rows.Count == 0)
            {
                AddRow(e);
            }
            else
            {
                if (dgDetail[colCheckPerson.Name, dgDetail.Rows.Count - 1].Value == null)
                {
                    conMenu.Hide();
                    MessageBox.Show("有新建的信息尚未保存！");
                }
                else
                {
                    AddRow(e);
                }
            }
        }

        void btnAddNew_Click(object sender, EventArgs e)
        {
            if (dgDetail.Rows.Count == 0)
            {
                AddRow(null);
            }
            else
            {
                if (dgDetail[colCheckPerson.Name, dgDetail.Rows.Count - 1].Value == null)
                {
                    conMenu.Hide();
                    MessageBox.Show("有新建的信息尚未保存！");
                }
                else
                {
                    AddRow(null);
                }
            }
            

        }

        void AddRow(ToolStripItemClickedEventArgs e)
        {
            conMenu.Hide();
            if (tvwCategory.SelectedNode == null)
            {
                MessageBox.Show("至少选择一个工程任务节点！");
                return;
            }
            InspectionRecord red = new InspectionRecord();
            red.DocState = DocumentState.Edit;
            red.CreateDate = ConstObject.LoginDate;
            int i = dgDetail.Rows.Add();
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                for (int j = 0; j < dgDetail.Rows.Count - 1; j++)
                {
                    dgDetail.Rows[j].Selected = false;
                }
            }
            dgDetail.Rows[i].Cells[0].Selected = true;
            dgDetail.Rows[i].Tag = red;
            txtCheckPerson1.Value = ConstObject.LoginPersonInfo.Name;
            txtCheckPerson1.Result.Add(ConstObject.LoginPersonInfo);
            dtpCheckDate1.Value = ConstObject.LoginDate;
            //通过GBWS节点查询服务obs信息的承担队伍
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Sql(" instr('" + oprNode.SysCode + "',{alias}.ProjectTaskCode)>0"));
            IList list = model.ObjectQuery(typeof(OBSService), oq);
            if (list.Count > 0)
            {
                OBSService service = list[0] as OBSService;
                txtSupply1.Text = service.SupplierName;
                txtSupply1.Tag = service.SupplierId;
            }
            else
            {
                txtSupply1.Text = "";
                txtSupply1.Tag = null;
            }
            txtCheckStatus1.Text = "";
            cbCheckConclusion1.Text = "";
            
            tabPage2.Parent = tabControl1;
            相关附件.Parent = null;
            tabControl1.SelectedTab = tabPage2;
            相关附件.Parent = tabControl1;
            this.radioButton4.Checked = true;
            this.cbWBSCheckRequir1.Text = "质检员检查";
            RefreshControls(MainViewState.Modify);
            this.btnAddNew.Enabled = false;
            this.btnDelete1.Enabled = false;

        }
        #endregion

        bool SetMessage()
        {
            if (dgDetail.CurrentRow == null)
            {
                MessageBox.Show("表格中没有信息，不可保存！");
                return false;
            }

            return true;
        }

        bool SetMessage1()
        {
            if (cbWBSCheckRequir1.Text == "")
            {
                MessageBox.Show("请选择检查专业！");
                return false;
            }
            if (txtCheckPerson1.Text == "")
            {
                MessageBox.Show("请选择检查人！");
                return false;
            }
            if (cbCheckConclusion1.Text == "")
            {
                MessageBox.Show("请选择检查结论！");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 保存更新质量检验记录
        /// </summary>
        /// <returns></returns>
        private void SaveInsRecord(string strName)
        {
            try
            {
                InspectionRecord master = dgDetail.CurrentRow.Tag as InspectionRecord;
                if (master.Id == null)
                    master = new InspectionRecord();
                if (strName == "保存")
                {
                    master.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                    master.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;
                    master.OpgSysCode = ConstObject.TheOperationOrg.SysCode;
                    master.HandOrgLevel = ConstObject.TheOperationOrg.Level;
                    master.HandlePerson = ConstObject.LoginPersonInfo;
                    master.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                    CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                    master.ProjectId = projectInfo.Id;
                    master.ProjectName = projectInfo.Name;
                    master.DocState = DocumentState.Edit;
                    master.InspectType = 2;
                }
                else
                {
                    master.DocState = DocumentState.InExecute;
                }
                //检查人
                if (txtCheckPerson1.Result.Count > 0)
                {
                    master.CreatePerson = txtCheckPerson1.Result[0] as PersonInfo;
                    master.CreatePersonName = txtCheckPerson1.Text;
                }
                master.InspectionSpecial = cbWBSCheckRequir1.Text;//检查专业
                System.Web.UI.WebControls.ListItem li = cbWBSCheckRequir1.SelectedItem as System.Web.UI.WebControls.ListItem;

                master.GWBSTree = oprNode;
                master.GWBSTreeName = oprNode.Name;
                master.GWBSTreeSysCode = oprNode.SysCode;
                if (txtSupply1.Tag != null)
                {
                    master.BearTeam = txtSupply1.Tag as SubContractProject;
                    master.BearTeamName = ClientUtil.ToString(txtSupply1.Text);
                }
                else
                {
                    master.BearTeam = null;
                    master.BearTeamName = "";
                }
                master.InspectionSpecialCode = li.Value;
                master.InspectionConclusion = cbCheckConclusion1.Text;//检查结论
                master.InspectionStatus = txtCheckStatus1.Text;//检查内容
                master.CreateDate = dtpCheckDate1.Value.Date;
                if (master.Id != null)
                {
                    if (master.DeductionSign != 2)
                    {
                        if (radioButton4.Checked)
                        {
                            master.CorrectiveSign = 0;
                        }
                        else
                        {
                            master.CorrectiveSign = 1;
                        }
                    }
                }
                else
                {
                    if (radioButton4.Checked)
                    {
                        master.CorrectiveSign = 0;
                    }
                    else
                    {
                        master.CorrectiveSign = 1;
                    }
                }
                //保存日常检查记录
                master = model.SaveInspectialRecordMaster(master);
                this.dgDetail.Rows[dgDetail.Rows.Count - 1].Tag = master;
                MessageBox.Show(strName + "成功！");
                RefreshControls(MainViewState.Browser);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存数据错误" + ex.ToString());
                return;
            }
        }

        #region 文档操作
        MDocumentCategory msrv = new MDocumentCategory();
        //文档按钮状态
        private void btnStates(int i)
        {
            if (i == 0)
            {
                //btnDownLoadDocument.Enabled = false;
                //btnOpenDocument.Enabled = false;
                btnUpdateDocument.Enabled = false;
                btnDeleteDocumentFile.Enabled = false;
                btnUpFile.Enabled = false;
                btnDeleteDocumentMaster.Enabled = false;
                btnDocumentFileAdd.Enabled = false;
                btnDocumentFileUpdate.Enabled = false;
                lnkCheckAll.Enabled = false;
                lnkCheckAllNot.Enabled = false;
            }
            if (i == 1)
            {
                //btnDownLoadDocument.Enabled = true;
                //btnOpenDocument.Enabled = true;
                btnUpdateDocument.Enabled = true;
                btnDeleteDocumentFile.Enabled = true;
                btnUpFile.Enabled = true;
                btnDeleteDocumentMaster.Enabled = true;
                btnDocumentFileAdd.Enabled = true;
                btnDocumentFileUpdate.Enabled = true;
                lnkCheckAll.Enabled = true;
                lnkCheckAllNot.Enabled = true;
            }
            dgDocumentDetail.ReadOnly = false;
            FileSelect.ReadOnly = false;

        }
        //加载文档数据
        void FillDoc()
        {
            if (dgDetail.CurrentRow == null || dgDetail.CurrentRow.Tag == null)
                return;
            InspectionRecord curBillMaster = dgDetail.CurrentRow.Tag as InspectionRecord;
            dgDocumentMast.Rows.Clear();
            dgDocumentDetail.Rows.Clear();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", curBillMaster.Id));
            IList listObj = model.ObjectQuery(typeof(ProObjectRelaDocument), oq);
            if (listObj != null && listObj.Count > 0)
            {
                oq.Criterions.Clear();
                Disjunction dis = new Disjunction();
                foreach (ProObjectRelaDocument obj in listObj)
                {
                    dis.Add(Expression.Eq("Id", obj.DocumentGUID));
                }
                oq.AddCriterion(dis);
                oq.AddFetchMode("ListFiles", NHibernate.FetchMode.Eager);
                IList docList = model.ObjectQuery(typeof(DocumentMaster), oq);
                if (docList != null && docList.Count > 0)
                {
                    foreach (DocumentMaster m in docList)
                    {
                        AddDgDocumentMastInfo(m);
                    }
                    dgDocumentMast.CurrentCell = dgDocumentMast[2, 0];
                    dgDocumentMast_CellClick(dgDocumentMast, new DataGridViewCellEventArgs(2, 0));
                }
            }
        }
        //添加文件
        void btnDocumentFileAdd_Click(object sender, EventArgs e)
        {
            if (dgDocumentMast.SelectedRows.Count == 0) return;
            DocumentMaster docMaster = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
            VDocumentFileUpload frm = new VDocumentFileUpload(docMaster, "add");
            frm.ShowDialog();
            IList resultList = frm.ResultList;
            if (resultList != null && resultList.Count > 0)
            {
                foreach (DocumentDetail dtl in resultList)
                {
                    AddDgDocumentDetailInfo(dtl);
                    docMaster.ListFiles.Add(dtl);
                }
            }
        }
        //修改文件
        void btnDocumentFileUpdate_Click(object sender, EventArgs e)
        {
            if (dgDocumentMast.SelectedRows.Count == 0 || dgDocumentDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要修改的文档件！");
                return;
            }
            DocumentMaster docMaster = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
            VDocumentFileModify frm = new VDocumentFileModify(docMaster);
            frm.ShowDialog();
            IList resultList = frm.ResultList;
            if (resultList != null && resultList.Count > 0)
            {
                DocumentDetail dtl = resultList[0] as DocumentDetail;
                AddDgDocumentDetailInfo(dtl, dgDocumentDetail.SelectedRows[0].Index);
                for (int i = 0; i < docMaster.ListFiles.Count; i++)
                {
                    DocumentDetail detail = docMaster.ListFiles.ElementAt(i);
                    if (detail.Id == dtl.Id)
                    {
                        detail = dtl;
                    }
                }
            }
        }
        //下载
        void btnDownLoadDocument_Click(object sender, EventArgs e)
        {
            IList downList = new ArrayList();
            foreach (DataGridViewRow row in dgDocumentDetail.Rows)
            {
                if ((bool)row.Cells[FileSelect.Name].EditedFormattedValue)
                {
                    DocumentDetail dtl = row.Tag as DocumentDetail;
                    downList.Add(dtl);
                }
            }
            if (downList != null && downList.Count > 0)
            {
                VDocumentPublicDownload frm = new VDocumentPublicDownload(downList);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("请勾选要下载的文件！");
            }
        }
        //预览
        void btnOpenDocument_Click(object sender, EventArgs e)
        {
            IList list = new ArrayList();
            foreach (DataGridViewRow row in dgDocumentDetail.Rows)
            {
                if ((bool)row.Cells[FileSelect.Name].EditedFormattedValue)
                {
                    DocumentDetail dtl = row.Tag as DocumentDetail;
                    list.Add(dtl);
                }
            }

            if (list.Count == 0)
            {
                MessageBox.Show("勾选文件才能预览，请选择要预览的文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {

                List<string> listFileFullPaths = new List<string>();
                string fileFullPath1 = AppDomain.CurrentDomain.BaseDirectory + "TempFilePreview\\" + Guid.NewGuid().ToString().Replace("-", "");
                foreach (DocumentDetail docFile in list)
                {
                    //DocumentDetail docFile = dgDocumentDetail.SelectedRows[0].Tag as DocumentDetail;
                    if (!Directory.Exists(fileFullPath1))
                        Directory.CreateDirectory(fileFullPath1);

                    string tempFileFullPath = fileFullPath1 + @"\\" + docFile.FileName;

                    //string address = "http://10.70.18.203//TestFile//Files//0001//0001//0001.0001.000027V_特效.jpg";
                    string baseAddress = @docFile.TheFileCabinet.TransportProtocal.ToString().ToLower() + "://" + docFile.TheFileCabinet.ServerName + "/" + docFile.TheFileCabinet.Path + "/";

                    string address = baseAddress + docFile.FilePartPath;
                    UtilityClass.WebClientObj.DownloadFile(address, tempFileFullPath);
                    listFileFullPaths.Add(tempFileFullPath);
                }
                foreach (string fileFullPath in listFileFullPaths)
                {
                    FileInfo file = new FileInfo(fileFullPath);

                    //定义一个ProcessStartInfo实例
                    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                    //设置启动进程的初始目录
                    info.WorkingDirectory = file.DirectoryName;//Application.StartupPath; 
                    //设置启动进程的应用程序或文档名
                    info.FileName = file.Name;
                    //设置启动进程的参数
                    info.Arguments = "";
                    //启动由包含进程启动信息的进程资源
                    try
                    {
                        System.Diagnostics.Process.Start(info);
                    }
                    catch (System.ComponentModel.Win32Exception we)
                    {
                        MessageBox.Show(this, we.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("未将对象引用设置到对象的实例") > -1)
                {
                    MessageBox.Show("操作失败，不存在的文档对象！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        //删除文件
        void btnDeleteDocumentFile_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> rowList = new List<DataGridViewRow>();
            IList deleteFileList = new ArrayList();
            foreach (DataGridViewRow row in dgDocumentDetail.Rows)
            {
                if ((bool)row.Cells[FileSelect.Name].EditedFormattedValue)
                {
                    DocumentDetail dtl = row.Tag as DocumentDetail; ;
                    rowList.Add(row);
                    deleteFileList.Add(dtl);
                }
            }
            if (deleteFileList.Count == 0)
            {
                MessageBox.Show("请勾选要删除的数据！");
                return;
            }
            if (MessageBox.Show("要删除当前勾选文件吗！", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (msrv.Delete(deleteFileList))
                {
                    MessageBox.Show("删除成功！");
                    DocumentMaster master = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
                    foreach (DocumentDetail dtl in deleteFileList)
                    {
                        master.ListFiles.Remove(dtl);
                    }
                    dgDocumentMast.SelectedRows[0].Tag = master;
                }
                else
                {
                    MessageBox.Show("删除失败！");
                    return;
                }
                if (rowList != null && rowList.Count > 0)
                {
                    foreach (DataGridViewRow row in rowList)
                    {
                        dgDocumentDetail.Rows.Remove(row);
                    }
                }
            }
        }
        //添加文档（加文件）
        void btnUpFile_Click(object sender, EventArgs e)
        {
            if (dgDetail.CurrentRow == null)
            {
                MessageBox.Show("没有选中的信息");
                return;
            }
            InspectionRecord curBillMaster = dgDetail.CurrentRow.Tag as InspectionRecord;

            if (curBillMaster.Id != null)
            {
                VDocumentPublicUpload frm = new VDocumentPublicUpload(curBillMaster.Id);
                frm.ShowDialog();
                DocumentMaster resultDoc = frm.Result;
                if (resultDoc == null) return;
                AddDgDocumentMastInfo(resultDoc);
                dgDocumentMast.CurrentCell = dgDocumentMast[2, dgDocumentMast.Rows.Count - 1];
                dgDocumentDetail.Rows.Clear();
                if (resultDoc.ListFiles != null && resultDoc.ListFiles.Count > 0)
                {
                    foreach (DocumentDetail dtl in resultDoc.ListFiles)
                    {
                        AddDgDocumentDetailInfo(dtl);
                    }
                }
            }
        }
        //修改文档（加文件）
        void btnUpdateDocument_Click(object sender, EventArgs e)
        {
            if (dgDocumentMast.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要修改的文档！");
                return;
            }
            DocumentMaster master = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
            IList docFileList = new ArrayList();
            foreach (DataGridViewRow row in dgDocumentDetail.Rows)
            {
                DocumentDetail dtl = row.Tag as DocumentDetail;
                docFileList.Add(dtl);
            }
            VDocumentPublicModify frm = new VDocumentPublicModify(master, docFileList);
            frm.ShowDialog();
            DocumentMaster resultMaster = frm.Result;
            if (resultMaster == null) return;
            AddDgDocumentMastInfo(resultMaster, dgDocumentMast.SelectedRows[0].Index);
            dgDocumentMast_CellClick(dgDocumentMast, new DataGridViewCellEventArgs(2, dgDocumentMast.SelectedRows[0].Index));
        }
        //删除文档
        void btnDeleteDocumentMaster_Click(object sender, EventArgs e)
        {
            if (dgDocumentMast.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的文档！");
                return;
            }
            if (MessageBox.Show("要删除当前文档吗？该操作将连它的所有文件一同删除！", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DocumentMaster mas = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
                IList list = new ArrayList();
                list.Add(mas);

                if (msrv.Delete(list))
                {
                    MessageBox.Show("删除成功！");
                    dgDocumentMast.Rows.Remove(dgDocumentMast.SelectedRows[0]);
                    if (dgDocumentMast.Rows.Count > 0)
                        dgDocumentMast_CellClick(dgDocumentMast, new DataGridViewCellEventArgs(2, dgDocumentMast.SelectedRows[0].Index));
                    else
                        dgDocumentDetail.Rows.Clear();
                }
                else
                {
                    MessageBox.Show("删除失败！");
                }
            }
        }
        void dgDocumentMast_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            dgDocumentDetail.Rows.Clear();
            DocumentMaster docMaster = dgDocumentMast.Rows[e.RowIndex].Tag as DocumentMaster;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", docMaster.Id));
            oq.AddFetchMode("TheFileCabinet", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof(DocumentDetail), oq);
            if (list != null && list.Count > 0)
            {
                foreach (DocumentDetail docDetail in list)
                {
                    AddDgDocumentDetailInfo(docDetail);
                }
            }
        }

        #region 列表里添加数据
        void AddDgDocumentMastInfo(DocumentMaster m)
        {
            int rowIndex = dgDocumentMast.Rows.Add();
            AddDgDocumentMastInfo(m, rowIndex);
        }
        void AddDgDocumentMastInfo(DocumentMaster m, int rowIndex)
        {
            dgDocumentMast[colDocumentName.Name, rowIndex].Value = m.Name;
            dgDocumentMast[colCreateTime.Name, rowIndex].Value = m.CreateTime;
            dgDocumentMast[colDocumentCode.Name, rowIndex].Value = m.Code;
            dgDocumentMast[colDocumentInforType.Name, rowIndex].Value = m.DocType.ToString();
            dgDocumentMast[colDocumentState.Name, rowIndex].Value = ClientUtil.GetDocStateName(m.State);
            dgDocumentMast[colOwnerName.Name, rowIndex].Value = m.OwnerName;
            dgDocumentMast.Rows[rowIndex].Tag = m;
        }

        void AddDgDocumentDetailInfo(DocumentDetail d)
        {
            int rowIndex = dgDocumentDetail.Rows.Add();
            AddDgDocumentDetailInfo(d, rowIndex);
        }
        void AddDgDocumentDetailInfo(DocumentDetail d, int rowIndex)
        {
            dgDocumentDetail[FileName.Name, rowIndex].Value = d.FileName;
            dgDocumentDetail.Rows[rowIndex].Tag = d;
        }
        #endregion

        //反选
        void lnkCheckAllNot_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgDocumentDetail.Rows)
            {
                row.Cells[FileSelect.Name].Value = false;
            }
        }
        //全选
        void lnkCheckAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgDocumentDetail.Rows)
            {
                row.Cells[FileSelect.Name].Value = true;
            }
        }
        #endregion

        #region 生产检查打印

        void btnPrint_Click(object sender, EventArgs e)
        {
            if (LoadTempleteFile(@"日常检查记录.flx") == false) return;
            //FillFlex();
            //flexGrid1.Print();
            //curBillMaster.PrintTimes = curBillMaster.PrintTimes + 1;
            //curBillMaster = model.saveWasteMatApply(curBillMaster);
            return;

        }
        void btnPrintPreview_Click(object sender, EventArgs e)
        {

            if (LoadTempleteFile(@"日常检查记录.flx") == false) return;
            //FillFlex();
            //flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            return;
        }

        private bool LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式和数据
                //flexGrid1.OpenFile(path + "\\" + modelName);//载入格式
            }
            else
            {
                MessageBox.Show("未找到模板格式文件【" + modelName + "】");
                return false;
            }
            return true;
        }

        #endregion

        bool isSelectNodeInvoke = false;//是否在选择(点击)节点时调用
        bool startNodeCheckedState = false;//按shift多选兄弟节点时起始节点的选中状态

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

        private void LoadGWBSChildNode(TreeNode parentNode, GWBSTree parentObj, IEnumerable<GWBSTree> listGWBS)
        {
            var query = from wbs in listGWBS
                        where wbs.ParentNode.Id == parentObj.Id
                        select wbs;

            foreach (GWBSTree wbs in query)
            {
                TreeNode childNode = new TreeNode();
                childNode.Tag = wbs;
                childNode.Name = wbs.Id;
                childNode.Text = wbs.Name;
                parentNode.Nodes.Add(childNode);

                LoadGWBSChildNode(childNode, wbs, listGWBS);
            }
        }

        public override bool SaveView()
        {
            if (!ValideSave()) return false;
            return model.SaveAndWritebackScrollPlanState(oprNode);

        }

        private bool ValideSave()
        {
            return true;
        }

        public void Start()
        {
            RefreshState(MainViewState.Browser);
            if (IsDistribute)
            {
                NewLoadGWBSTreeTree();
            }
            else
            {
                LoadGWBSTreeTree();
            }
        }

        private void LoadGWBSTreeTree1(TreeNode oNode)
        {
            Hashtable hashtable = new Hashtable();
            int iLevel = 1;
            string sSysCode = string.Empty;
            IList list = null;
            try
            {
                if (oNode != null)
                {
                    GWBSTree oGWBSTree = oNode.Tag as GWBSTree;
                    if (oGWBSTree != null)
                    {

                        if (oGWBSTree != null)
                        {
                            iLevel = oGWBSTree.Level + 1;
                            sSysCode = oGWBSTree.SysCode;
                        }
                    }
                }
                list = model.GetGWBSTreesByInstance(projectInfo.Id, sSysCode, iLevel);
                if (oNode != null)
                {
                    oNode.Nodes.Clear();
                }

                if (list == null || list.Count == 0)
                {
                    PBSTree pbs = null;
                    ProjectTaskTypeTree taskType = null;

                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));

                    //if (oNode == null)
                    //{
                    oq.AddCriterion(Expression.IsNull("ParentNode"));
                    oq.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
                    //}
                    //else
                    //{

                    //}
                    IList listPBS = model.ObjectQuery(typeof(PBSTree), oq);

                    if (listPBS == null || listPBS.Count == 0)
                        return;
                    pbs = listPBS[0] as PBSTree;


                    oq.Criterions.Clear();
                    oq.AddCriterion(Expression.IsNull("ParentNode"));
                    oq.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
                    IList listTaskType = model.ObjectQuery(typeof(ProjectTaskTypeTree), oq);

                    if (listTaskType == null || listTaskType.Count == 0)
                        return;
                    taskType = listTaskType[0] as ProjectTaskTypeTree;


                    IList listAdd = new ArrayList();

                    GWBSTree root = new GWBSTree();

                    DateTime serverTime = model.GetServerTime();

                    root.TheProjectGUID = projectInfo.Id;
                    root.TheProjectName = projectInfo.Name;
                    root.Name = projectInfo.Name;

                    root.Code = projectInfo.Code.PadLeft(4, '0') + "-" + model.GetCode(typeof(GWBSTree));
                    root.CheckRequire = string.IsNullOrEmpty(taskType.CheckRequire) ? ("X".PadRight(11, 'X') + "0") : (taskType.CheckRequire.PadRight(11, 'X') + "0");

                    GWBSRelaPBS relaPBS = new GWBSRelaPBS();
                    relaPBS.ThePBS = pbs;

                    relaPBS.TheGWBSTree = root;
                    root.ListRelaPBS.Add(relaPBS);

                    root.ProjectTaskTypeGUID = taskType;
                    root.ProjectTaskTypeName = taskType.Name;

                    root.TaskState = DocumentState.Edit;
                    root.TaskStateTime = serverTime;
                    root.OwnerGUID = ConstObject.LoginPersonInfo.Id;
                    root.OwnerName = ConstObject.LoginPersonInfo.Name;
                    root.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;
                    root.OrderNo = 0;

                    listAdd.Add(root);

                    model.SaveGWBSTreeRootNode(listAdd);

                    list = model.GetGWBSTreesByInstance(projectInfo.Id, sSysCode, iLevel);
                }

                if (list != null && list.Count > 0)
                {
                    foreach (GWBSTree childNode in list)
                    {
                        CreateChildNode(oNode, childNode);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        public void CreateChildNode(TreeNode oParentNode, GWBSTree oGWBSTree)
        {

            if (oGWBSTree.State != 0)
            {
                TreeNode oChild = new TreeNode();
                oChild.Name = oGWBSTree.Id;
                oChild.Text = oGWBSTree.Name;
                oChild.Tag = oGWBSTree;
                if (oGWBSTree.CategoryNodeType != NodeType.LeafNode)
                {
                    TreeNode oEmptyNode = new TreeNode();
                    oEmptyNode.Name = "Test";
                    oEmptyNode.Text = "Test";
                    oEmptyNode.Tag = null;
                    oChild.Nodes.Add(oEmptyNode);
                }
                if (oParentNode == null)
                {
                    tvwCategory.Nodes.Add(oChild);
                }
                else
                {
                    oParentNode.Nodes.Add(oChild);
                }
                if ((oChild.Tag as GWBSTree).ProductConfirmFlag)
                {
                    //生产节点
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("GWBSDetail.TheGWBS", oChild.Tag as GWBSTree));
                    IList list = model.ObjectQuery(typeof(GWBSTaskConfirm), oq);
                    if (list.Count > 0)
                    {
                        oParentNode.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                        oParentNode.ForeColor = ColorTranslator.FromHtml("#000000");
                    }

                }
            }
        }

        private void LoadGWBSTreeTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();
                IList list = model.GetGWBSTreesByInstance(projectInfo.Id);


                if (list == null || list.Count == 0)
                {
                    PBSTree pbs = null;
                    ProjectTaskTypeTree taskType = null;

                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
                    oq.AddCriterion(Expression.IsNull("ParentNode"));
                    oq.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
                    IList listPBS = model.ObjectQuery(typeof(PBSTree), oq);

                    if (listPBS == null || listPBS.Count == 0)
                        return;
                    pbs = listPBS[0] as PBSTree;


                    oq.Criterions.Clear();
                    oq.AddCriterion(Expression.IsNull("ParentNode"));
                    oq.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
                    IList listTaskType = model.ObjectQuery(typeof(ProjectTaskTypeTree), oq);

                    if (listTaskType == null || listTaskType.Count == 0)
                        return;
                    taskType = listTaskType[0] as ProjectTaskTypeTree;


                    IList listAdd = new ArrayList();

                    GWBSTree root = new GWBSTree();

                    DateTime serverTime = model.GetServerTime();

                    root.TheProjectGUID = projectInfo.Id;
                    root.TheProjectName = projectInfo.Name;
                    root.Name = projectInfo.Name;

                    root.Code = projectInfo.Code.PadLeft(4, '0') + "-" + model.GetCode(typeof(GWBSTree));
                    root.CheckRequire = string.IsNullOrEmpty(taskType.CheckRequire) ? ("X".PadRight(11, 'X') + "0") : (taskType.CheckRequire.PadRight(11, 'X') + "0");

                    GWBSRelaPBS relaPBS = new GWBSRelaPBS();
                    relaPBS.ThePBS = pbs;

                    relaPBS.TheGWBSTree = root;
                    root.ListRelaPBS.Add(relaPBS);

                    root.ProjectTaskTypeGUID = taskType;
                    root.ProjectTaskTypeName = taskType.Name;

                    root.TaskState = DocumentState.Edit;
                    root.TaskStateTime = serverTime;
                    root.OwnerGUID = ConstObject.LoginPersonInfo.Id;
                    root.OwnerName = ConstObject.LoginPersonInfo.Name;
                    root.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;
                    root.OrderNo = 0;

                    listAdd.Add(root);

                    model.SaveGWBSTreeRootNode(listAdd);

                    list = model.GetGWBSTreesByInstance(projectInfo.Id);
                }

                if (list != null && list.Count > 0)
                {
                    foreach (GWBSTree childNode in list)
                    {
                        if (childNode.State == 0)
                            continue;

                        TreeNode tnTmp = new TreeNode();
                        tnTmp.Name = childNode.Id.ToString();
                        tnTmp.Text = childNode.Name;
                        tnTmp.Tag = childNode;
                        //ObjectQuery oq = new ObjectQuery();
                        //oq.AddCriterion(Expression.Eq("GWBSDetail.TheGWBS.Id", childNode.Id));
                        //IList listTmp = model.ObjectQuery(typeof(GWBSTaskConfirm), oq);
                        if (childNode.ProductConfirmFlag)
                        {
                            tnTmp.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                            tnTmp.ForeColor = ColorTranslator.FromHtml("#000000");
                        }
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
                        if ((tnTmp.Tag as GWBSTree).SpecialType != null)
                        {
                            SpecialHt.Add((tnTmp.Tag as GWBSTree).SysCode, (tnTmp.Tag as GWBSTree).SpecialType);
                        }
                    }
                    this.tvwCategory.SelectedNode = this.tvwCategory.Nodes[0];
                    this.tvwCategory.SelectedNode.Expand();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        private void NewLoadGWBSTreeTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();
                //IList list = model.GetGWBSTreesByInstance(projectInfo.Id);
                DataTable dtGwbsTress = model.GetGWBSTree(projectInfo.Id, "", 1);

                if (dtGwbsTress == null || dtGwbsTress.Rows.Count == 0)
                {
                    PBSTree pbs = null;
                    ProjectTaskTypeTree taskType = null;

                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
                    oq.AddCriterion(Expression.IsNull("ParentNode"));
                    oq.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
                    IList listPBS = model.ObjectQuery(typeof(PBSTree), oq);

                    if (listPBS == null || listPBS.Count == 0)
                        return;
                    pbs = listPBS[0] as PBSTree;


                    oq.Criterions.Clear();
                    oq.AddCriterion(Expression.IsNull("ParentNode"));
                    oq.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
                    IList listTaskType = model.ObjectQuery(typeof(ProjectTaskTypeTree), oq);

                    if (listTaskType == null || listTaskType.Count == 0)
                        return;
                    taskType = listTaskType[0] as ProjectTaskTypeTree;


                    IList listAdd = new ArrayList();

                    GWBSTree root = new GWBSTree();

                    DateTime serverTime = model.GetServerTime();

                    root.TheProjectGUID = projectInfo.Id;
                    root.TheProjectName = projectInfo.Name;
                    root.Name = projectInfo.Name;

                    root.Code = projectInfo.Code.PadLeft(4, '0') + "-" + model.GetCode(typeof(GWBSTree));
                    root.CheckRequire = string.IsNullOrEmpty(taskType.CheckRequire) ? ("X".PadRight(11, 'X') + "0") : (taskType.CheckRequire.PadRight(11, 'X') + "0");

                    GWBSRelaPBS relaPBS = new GWBSRelaPBS();
                    relaPBS.ThePBS = pbs;

                    relaPBS.TheGWBSTree = root;
                    root.ListRelaPBS.Add(relaPBS);

                    root.ProjectTaskTypeGUID = taskType;
                    root.ProjectTaskTypeName = taskType.Name;

                    root.TaskState = DocumentState.Edit;
                    root.TaskStateTime = serverTime;
                    root.OwnerGUID = ConstObject.LoginPersonInfo.Id;
                    root.OwnerName = ConstObject.LoginPersonInfo.Name;
                    root.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;
                    root.OrderNo = 0;

                    listAdd.Add(root);

                    model.SaveGWBSTreeRootNode(listAdd);

                    //  list = model.GetGWBSTreesByInstance(projectInfo.Id);
                    dtGwbsTress = model.GetGWBSTree(projectInfo.Id, "", 1);
                }

                if (dtGwbsTress != null && dtGwbsTress.Rows.Count > 0)
                {
                    AddChildNodes(null, dtGwbsTress);
                    this.tvwCategory.SelectedNode = this.tvwCategory.Nodes[0];
                    this.tvwCategory.SelectedNode.Expand();

                }
            }
            catch (Exception e)
            {
                MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        public TreeNode CreateNode(GWBSTree oGWBSTree)
        {
            TreeNode tnTmp = new TreeNode();
            tnTmp.Name = oGWBSTree.Id.ToString();
            tnTmp.Text = oGWBSTree.Name;
            tnTmp.Tag = oGWBSTree;

            if (oGWBSTree.ProductConfirmFlag)
            {
                tnTmp.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                tnTmp.ForeColor = ColorTranslator.FromHtml("#000000");
            }

            if ((tnTmp.Tag as GWBSTree).SpecialType != null)
            {
                SpecialHt.Add((tnTmp.Tag as GWBSTree).SysCode, (tnTmp.Tag as GWBSTree).SpecialType);
            }
            if (oGWBSTree.CategoryNodeType != NodeType.LeafNode)
            {
                tnTmp.Nodes.Add("test");
            }
            return tnTmp;
        }

        public GWBSTree CreateGWBSTree(DataRow oRow)
        {
            GWBSTree oGWBSTree = null;
            if (oRow != null)
            {
                oGWBSTree = new GWBSTree();
                oGWBSTree.Name = ClientUtil.ToString(oRow["Name"]);
                oGWBSTree.Id = ClientUtil.ToString(oRow["ID"]);
                oGWBSTree.Version = 0;
                oGWBSTree.CategoryNodeType = (NodeType)ClientUtil.ToInt(oRow["CategoryNodeType"]);
                if (oGWBSTree.CategoryNodeType == NodeType.LeafNode)
                {
                    oGWBSTree.CategoryNodeType = ClientUtil.ToInt(oRow["childCount"]) == 0 ? NodeType.LeafNode : NodeType.MiddleNode;
                }
                oGWBSTree.Code = ClientUtil.ToString(oRow["Code"]);
                oGWBSTree.CreateDate = ClientUtil.ToDateTime(oRow["CreateDate"]);
                oGWBSTree.SysCode = ClientUtil.ToString(oRow["SysCode"]);
                oGWBSTree.State = ClientUtil.ToInt(oRow["State"]);
                oGWBSTree.Level = ClientUtil.ToInt(oRow["TLevel"]);
                oGWBSTree.Describe = ClientUtil.ToString(oRow["Describe"]);
                if (string.IsNullOrEmpty(ClientUtil.ToString(oRow["PARENTNODEID"])))
                {
                    oGWBSTree.ParentNode = null;
                }
                else
                {
                    oGWBSTree.ParentNode = new GWBSTree();
                    oGWBSTree.ParentNode.Id = ClientUtil.ToString(oRow["PARENTNODEID"]);
                    oGWBSTree.ParentNode.Version = 0;
                }
                //oGWBSTree .Author =new 
                //TheTree
                oGWBSTree.OrderNo = ClientUtil.ToLong(oRow["OrderNo"]);
                if (string.IsNullOrEmpty(ClientUtil.ToString(oRow["projecttasktypeguid"])))
                {
                    oGWBSTree.ProjectTaskTypeGUID = null;
                }
                else
                {
                    oGWBSTree.ProjectTaskTypeGUID = new ProjectTaskTypeTree();
                    oGWBSTree.ProjectTaskTypeGUID.Id = ClientUtil.ToString(oRow["projecttasktypeguid"]);
                    oGWBSTree.ProjectTaskTypeGUID.Name = ClientUtil.ToString(oRow["ProjectTaskTypeName"]);
                    oGWBSTree.ProjectTaskTypeGUID.Version = 0;
                }
                oGWBSTree.ProjectTaskTypeName = ClientUtil.ToString(oRow["ProjectTaskTypeName"]);
                oGWBSTree.Summary = ClientUtil.ToString(oRow["Summary"]);
                oGWBSTree.TaskState = (DocumentState)ClientUtil.ToInt(oRow["TaskState"]);
                oGWBSTree.TaskStateTime = ClientUtil.ToDateTime(oRow["TaskStateTime"]);
                oGWBSTree.ContractTotalPrice = ClientUtil.ToDecimal(oRow["ContractTotalPrice"]);
                oGWBSTree.OwnerGUID = ClientUtil.ToString(oRow["OwnerGUID"]);
                oGWBSTree.OwnerName = ClientUtil.ToString(oRow["OwnerName"]);
                oGWBSTree.OwnerOrgSysCode = ClientUtil.ToString(oRow["OwnerOrgSysCode"]);
                if (string.IsNullOrEmpty(ClientUtil.ToString(oRow["PriceAmountUnitGUID"])))
                {
                    oGWBSTree.PriceAmountUnitGUID = null;
                }
                else
                {
                    oGWBSTree.PriceAmountUnitGUID = new StandardUnit();
                    oGWBSTree.PriceAmountUnitGUID.Id = ClientUtil.ToString(oRow["PriceAmountUnitGUID"]);
                    oGWBSTree.PriceAmountUnitGUID.Name = ClientUtil.ToString(oRow["PriceAmountUnitName"]);
                    oGWBSTree.PriceAmountUnitGUID.Version = 0;
                }
                oGWBSTree.PriceAmountUnitName = ClientUtil.ToString(oRow["PriceAmountUnitName"]);
                oGWBSTree.ResponsibilityTotalPrice = ClientUtil.ToDecimal(oRow["ResponsibilityTotalPrice"]);
                oGWBSTree.PlanTotalPrice = ClientUtil.ToDecimal(oRow["PlanTotalPrice"]);
                oGWBSTree.TaskPlanStartTime = ClientUtil.ToDateTime(oRow["TaskPlanStartTime"]);
                oGWBSTree.TaskPlanEndTime = ClientUtil.ToDateTime(oRow["TaskPlanEndTime"]);
                oGWBSTree.CheckRequire = ClientUtil.ToString(oRow["CheckRequire"]);
                oGWBSTree.NodeType = (WBSNodeType)ClientUtil.ToInt(oRow["CheckRequire"]);
                oGWBSTree.PalnTime = ClientUtil.ToInt(oRow["PalnTime"]);
                oGWBSTree.RealTime = ClientUtil.ToInt(oRow["RealTime"]);
                oGWBSTree.RealTime = ClientUtil.ToInt(oRow["RealTime"]);
                oGWBSTree.RealStartDate = ClientUtil.ToDateTime(oRow["RealStartDate"]);
                oGWBSTree.RealEndDate = ClientUtil.ToDateTime(oRow["RealEndDate"]);
                oGWBSTree.NGUID = ClientUtil.ToString(oRow["NGUID"]);
                oGWBSTree.AddUpFigureProgress = ClientUtil.ToDecimal(oRow["AddUpFigureProgress"]);
                oGWBSTree.ResponsibleAccFlag = ClientUtil.ToBool(oRow["ResponsibleAccFlag"]);
                oGWBSTree.CostAccFlag = ClientUtil.ToBool(oRow["CostAccFlag"]);
                oGWBSTree.ProductConfirmFlag = ClientUtil.ToBool(oRow["ProductConfirmFlag"]);
                oGWBSTree.SubContractFeeFlag = ClientUtil.ToBool(oRow["SubContractFeeFlag"]);
                oGWBSTree.CheckBatchNumber = ClientUtil.ToInt(oRow["CheckBatchNumber"]);
                oGWBSTree.OverOrUndergroundFlag = (OverOrUnderGroundFlagEnum)ClientUtil.ToInt(oRow["OverOrUndergroundFlag"]);
                oGWBSTree.WarehouseFlag = ClientUtil.ToBool(oRow["WarehouseFlag"]);
                oGWBSTree.AcceptanceCheckState = (AcceptanceCheckStateEnum)ClientUtil.ToInt(oRow["AcceptanceCheckState"]);
                oGWBSTree.SuperiorCheckState = (SuperiorCheckStateEnum)ClientUtil.ToInt(oRow["SuperiorCheckState"]);
                oGWBSTree.DailyCheckState = ClientUtil.ToString(oRow["DailyCheckState"]);
                oGWBSTree.SpecialType = ClientUtil.ToString(oRow["SpecialType"]);

            }

            return oGWBSTree;
        }
        
        private void ShowFindNode(TreeNode tn)
        {
            TreeNode theParentNode = tn.Parent;
            while (theParentNode != null)
            {
                theParentNode.Expand();
                theParentNode = theParentNode.Parent;
            }

            tvwCategory.Select();
            tvwCategory.SelectedNode = tn;
        }

        private void QueryCheckedTreeNode(TreeNode parentNode, string keyWord)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Text.IndexOf(keyWord) > -1)
                {
                    listFindNodes.Add(tn);
                }

                QueryCheckedTreeNode(tn, keyWord);
            }
        }


    }

}