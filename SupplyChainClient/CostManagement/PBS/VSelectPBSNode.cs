using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

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
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.PBS
{
    public partial class VSelectPBSNode : TBasicDataView
    {
        /// <summary>
        /// ��������ȣ�0��������
        /// </summary>
        private int SelectCopyLevel = 0;

        public List<TreeNode> SelectResult = new List<TreeNode>();

        private TreeNode currNode;

        private PBSTree oprNode = null;

        //��Ȩ�޵Ľڵ�
        private IList lstInstance;

        private Hashtable hashtableRules = new Hashtable();

        private Dictionary<string, TreeNode> listCheckedNode = new Dictionary<string, TreeNode>();
        private List<TreeNode> listCopyNode = new List<TreeNode>();

        private bool _IsSelectSingleNode = false;
        /// <summary>
        /// �Ƿ��ǵ����ڵ�ѡ��
        /// </summary>
        public bool IsSelectSingleNode
        {
            get { return _IsSelectSingleNode; }
            set { _IsSelectSingleNode = value; }
        }

        private SelectNodeMethod _SelectMethod = SelectNodeMethod.��״�ṹѡ��;
        /// <summary>
        /// ѡ��ʽ
        /// </summary>
        public SelectNodeMethod SelectMethod
        {
            get { return _SelectMethod; }
            set { _SelectMethod = value; }
        }

        /// <summary>
        ///ȱʡѡ���PBS
        /// </summary>
        public PBSTree DefaultSelectedPBS = null;

        public MPBSTree model = new MPBSTree();

        public VSelectPBSNode()
        {
            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();

            cbSelectMethod.Items.Add(SelectNodeMethod.��״�ṹѡ��.ToString());
            cbSelectMethod.Items.Add(SelectNodeMethod.��ɢ�ڵ�ѡ��.ToString());
            cbSelectMethod.SelectedIndex = 0;

            tvwCategory.CheckBoxes = false;

            txtCopyLevel.Text = "0";

            RefreshState(MainViewState.Browser);

            LoadPBSTreeTree();
        }

        private void InitEvents()
        {
            tvwCategory.AfterCheck += new TreeViewEventHandler(tvwCategory_AfterCheck);
            tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);

            btnEnter.Click += new EventHandler(btnEnter_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            cbSelectMethod.SelectedIndexChanged += new EventHandler(cbSelectMethod_SelectedIndexChanged);

            this.Load += new EventHandler(VSelectPBSNode_Load);
            this.FormClosing += new FormClosingEventHandler(VSelectPBSNode_FormClosing);
        }

        void VSelectPBSNode_Load(object sender, EventArgs e)
        {
            if (IsSelectSingleNode)
            {
                lblAlert.Visible = false;
                lblcopyLevel.Visible = false;
                lblCopyMethod.Visible = false;
                lblLevel.Visible = false;

                txtCopyLevel.Visible = false;
                cbSelectMethod.Visible = false;
            }
            else if (SelectMethod == SelectNodeMethod.��ɢ�ڵ�ѡ��)
            {
                cbSelectMethod.SelectedItem = SelectMethod.ToString();
                cbSelectMethod_SelectedIndexChanged(cbSelectMethod, new EventArgs());

                lblCopyMethod.Visible = false;
                cbSelectMethod.Visible = false;
            }

            if (DefaultSelectedPBS != null)
            {
                foreach (TreeNode tn in tvwCategory.Nodes)
                {
                    if (tn.Name == DefaultSelectedPBS.Id)
                    {
                        tvwCategory.SelectedNode = tn;
                        if (SelectMethod == SelectNodeMethod.��ɢ�ڵ�ѡ��)
                            tn.Checked = true;

                        TreeNode theParentNode = tn.Parent;
                        while (theParentNode != null)
                        {
                            theParentNode.Expand();
                            theParentNode = theParentNode.Parent;
                        }
                        break;
                    }
                    if (SetDefaultSelectedNode(tn))
                        break;
                }
            }
        }
        private bool SetDefaultSelectedNode(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Name == DefaultSelectedPBS.Id)
                {
                    tvwCategory.SelectedNode = tn;
                    if (SelectMethod == SelectNodeMethod.��ɢ�ڵ�ѡ��)
                        tn.Checked = true;

                    TreeNode theParentNode = tn.Parent;
                    while (theParentNode != null)
                    {
                        theParentNode.Expand();
                        theParentNode = theParentNode.Parent;
                    }
                    return true;
                }
                SetDefaultSelectedNode(tn);
            }

            return false;
        }

        public bool isOK = false;
        void btnEnter_Click(object sender, EventArgs e)
        {
            if (IsSelectSingleNode)//���ڵ�ѡ��
            {
                if (oprNode == null)
                {
                    MessageBox.Show("��ѡ��һ���ڵ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                TreeNode selectNode = tvwCategory.SelectedNode;
                if (selectNode.Nodes.Count > 0)
                    selectNode.Nodes.Clear();

                SelectResult.Add(selectNode);
            }
            else
            {
                if (cbSelectMethod.SelectedItem.ToString() == SelectNodeMethod.��״�ṹѡ��.ToString())
                {
                    if (oprNode == null)
                    {
                        MessageBox.Show("��ѡ��һ���ڵ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        if (txtCopyLevel.Text.Trim() == "")
                        {
                            MessageBox.Show("�����뿽�����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtCopyLevel.Focus();
                            return;
                        }
                        SelectCopyLevel = Convert.ToInt32(txtCopyLevel.Text);

                        if (SelectCopyLevel < 0)
                        {
                            MessageBox.Show("��������������ڻ����0��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("������������ʽ����ȷ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    TreeNode selectNode = tvwCategory.SelectedNode;
                    if (SelectCopyLevel > 0)
                    {
                        if (SelectCopyLevel == 1)
                            selectNode.Nodes.Clear();
                        else
                        {
                            foreach (TreeNode tn in selectNode.Nodes)
                            {
                                int level = 1;
                                SetCopyLevel(tn, level);
                            }
                        }
                    }

                    SelectResult.Add(selectNode);
                }
                else
                {
                    if (listCheckedNode.Count == 0)
                    {
                        MessageBox.Show("������ѡ��һ���ڵ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    foreach (var dic in listCheckedNode)
                    {
                        SelectResult.Add(dic.Value);
                    }
                }
            }
            isOK = true;
            this.Close();
        }

        private void SetCopyLevel(TreeNode parentNode, int level)
        {
            if (parentNode.Nodes.Count == 0)
                return;

            level += 1;
            if (level == SelectCopyLevel && parentNode.Nodes.Count > 0)
            {
                parentNode.Nodes.Clear();
            }

            foreach (TreeNode node in parentNode.Nodes)
            {
                SetCopyLevel(node, level);
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            SelectResult.Clear();
            this.Close();
        }

        void VSelectPBSNode_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isOK)
                SelectResult.Clear();
        }

        bool isSelectNodeInvoke = false;//�Ƿ���ѡ��(���)�ڵ�ʱ����
        bool startNodeCheckedState = false;//��shift��ѡ�ֵܽڵ�ʱ��ʼ�ڵ��ѡ��״̬
        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                #region ������ڵ�ʱʵ�ֶ�ѡ
                bool isMultiSelect = false;
                TreeNode preselectionNode;//Ԥѡ��ڵ�

                preselectionNode = e.Node;

                if (currNode != null && currNode.Name != preselectionNode.Name
                    && currNode.Parent != null && preselectionNode.Parent != null && currNode.Parent.Name == preselectionNode.Parent.Name)//Nameȡ�Ķ����ID
                    isMultiSelect = true;
                else
                    isMultiSelect = false;

                if (currNode != null)
                    startNodeCheckedState = currNode.Checked;

                if (isMultiSelect && (Control.ModifierKeys & Keys.Shift) != 0 && (Control.ModifierKeys & Keys.Control) != 0)//���ͬʱ������ctrl+shift
                {
                    int currNodeIndex = currNode.Index;
                    int preselectNodeIndex = preselectionNode.Index;

                    int startIndex = currNodeIndex < preselectNodeIndex ? currNodeIndex : preselectNodeIndex;
                    int endIndex = currNodeIndex < preselectNodeIndex ? preselectNodeIndex : currNodeIndex;

                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        TreeNode tn = currNode.Parent.Nodes[i];

                        isSelectNodeInvoke = true;//���ñ�־����check�¼��в��ٴ���

                        if (startNodeCheckedState)//�����ʼ�ڵ㵱ǰΪѡ�У���ȡ��ѡ��
                        {
                            TreeNode tempNode = new TreeNode();
                            tn.BackColor = tempNode.BackColor;
                            tn.ForeColor = tempNode.ForeColor;

                            if (listCheckedNode.ContainsKey(tn.Name))
                                listCheckedNode.Remove(tn.Name);

                            tn.Checked = false;
                        }
                        else//�����ʼ�ڵ㵱ǰΪδѡ�У�������ѡ��
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
                else if (isMultiSelect && (Control.ModifierKeys & Keys.Shift) != 0)//���ͬʱ������shift
                {


                    int currNodeIndex = currNode.Index;
                    int preselectNodeIndex = preselectionNode.Index;

                    int startIndex = currNodeIndex < preselectNodeIndex ? currNodeIndex : preselectNodeIndex;
                    int endIndex = currNodeIndex < preselectNodeIndex ? preselectNodeIndex : currNodeIndex;

                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        TreeNode tn = currNode.Parent.Nodes[i];

                        isSelectNodeInvoke = true;//���ñ�־����check�¼��в��ٴ���

                        if (startNodeCheckedState)//�����ʼ�ڵ㵱ǰΪѡ�У���ȡ��ѡ��
                        {
                            tn.Checked = false;

                            TreeNode tempNode = new TreeNode();
                            tn.BackColor = tempNode.BackColor;
                            tn.ForeColor = tempNode.ForeColor;

                            if (listCheckedNode.ContainsKey(tn.Name))
                                listCheckedNode.Remove(tn.Name);
                        }
                        else//�����ʼ�ڵ㵱ǰΪδѡ�У�������ѡ��
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

                oprNode = currNode.Tag as PBSTree;
                this.GetNodeDetail();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
            }
        }

        void tvwCategory_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (isSelectNodeInvoke)
            {
                isSelectNodeInvoke = false;
            }
            else
            {
                #region ������ѡ�����
                if ((Control.ModifierKeys & Keys.Shift) != 0 || (Control.ModifierKeys & Keys.Control) != 0)//���ͬʱ������Ctrl //(Control.ModifierKeys & Keys.Shift) != 0 || 
                {
                    if (e.Node.Checked)
                    {
                        e.Node.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                        e.Node.ForeColor = ColorTranslator.FromHtml("#000000");

                        //e.Node.BackColor = SystemColors.Control;
                        //e.Node.ForeColor = SystemColors.ControlText;

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

                        //e.Node.BackColor = SystemColors.Control;
                        //e.Node.ForeColor = SystemColors.ControlText;

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

                if (startNodeCheckedState)//�����ʼ�ڵ㵱ǰΪѡ�У���ȡ��ѡ��
                {
                    TreeNode tempNode = new TreeNode();
                    tn.BackColor = tempNode.BackColor;
                    tn.ForeColor = tempNode.ForeColor;

                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode.Remove(tn.Name);

                    tn.Checked = false;
                }
                else//�����ʼ�ڵ㵱ǰΪδѡ�У�������ѡ��
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

        private void GetNodeDetail()
        {
            try
            {
                ClearAll();

                this.txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath;

                this.txtName.Text = oprNode.Name;
                this.txtCode.Text = oprNode.Code;

                this.txtType.Text = oprNode.StructTypeName;
                this.txtDesc.Text = oprNode.Describe;

                RefreshControls(MainViewState.Browser);
            }
            catch (Exception exp)
            {
                MessageBox.Show("��ʾ������" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        private void ClearAll()
        {
            this.txtCurrentPath.Text = "";
            this.txtCode.Text = "";
            this.txtName.Text = "";
            this.txtType.Text = "";
            this.txtDesc.Text = "";
        }

        private void LoadPBSTreeTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                IList list = model.GetPBSTreesByInstance(projectInfo.Id);
                //lstInstance = listAll[1] as IList;
                //IList list = listAll[0] as IList;
                foreach (PBSTree childNode in list)
                {
                    if (childNode.State == 0)
                        continue;

                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = childNode.Name;
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
                MessageBox.Show("�������ݳ�����" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        public override void RefreshControls(MainViewState state)
        {
            switch (state)
            {
                case MainViewState.Modify:

                    break;

                case MainViewState.Browser:

                    break;
            }
        }

        private void GetCopyNode(TreeNode parentNode)
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
                else
                {
                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode.Remove(tn.Name);
                }
                GetCopyNode(tn);
            }
        }

        //������ʽ
        private void cbSelectMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSelectMethod.SelectedItem.ToString() == SelectNodeMethod.��ɢ�ڵ�ѡ��.ToString())
            {
                tvwCategory.CheckBoxes = true;
                SelectMethod = SelectNodeMethod.��ɢ�ڵ�ѡ��;


                lblAlert.Visible = false;
                lblcopyLevel.Visible = false;
                lblLevel.Visible = false;

                txtCopyLevel.Visible = false;
            }
            else
            {
                tvwCategory.CheckBoxes = false;
                SelectMethod = SelectNodeMethod.��״�ṹѡ��;


                lblAlert.Visible = true;
                lblcopyLevel.Visible = true;
                lblLevel.Visible = true;

                txtCopyLevel.Visible = true;
            }

            LoadPBSTreeTree();
        }
    }

    public enum SelectNodeMethod
    {
        ��״�ṹѡ�� = 1,
        ��ɢ�ڵ�ѡ�� = 2
    }
}