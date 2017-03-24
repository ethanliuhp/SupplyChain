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
        //��Ȩ�޵�ҵ����֯
        private IList lstInstance;
        //Ψһ����
        private string uniqueCode;
        private Hashtable hashtableRules = new Hashtable();
        private Dictionary<string, TreeNode> listCheckedNode = new Dictionary<string, TreeNode>();
        Dictionary<int, string> listTaskTypeLevel = new Dictionary<int, string>();
        /// <summary>
        /// ���ƵĶ����ڵ㼯��
        /// </summary>
        private List<TreeNode> listCopyNode = new List<TreeNode>();
        /// <summary>
        /// ���Ƶ������ӽڵ㼯�ϣ��������ѡ��Ľڵ�ʱ�����ҵ����ƵĽڵ�
        /// </summary>
        Dictionary<string, TreeNode> listCopyNodeAll = new Dictionary<string, TreeNode>();
        CurrentProjectInfo projectInfo = null;
        /// <summary>
        /// �Ƿ��ǲ���Ľڵ�
        /// </summary>
        private bool IsInsertNode = false;

        /// <summary>
        /// �Ƿ����ύ(���б���)
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
            tvwCategory.MouseDown += new MouseEventHandler(tvwCategory_MouseDown);//����û�и��ڵ�����
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
            StandardUnit su = UCL.Locate("������λά��", StandardUnitExcuteType.OpenSelect) as StandardUnit;
            if (su != null)
            {
                txtPriceUnit.Tag = su;
                txtPriceUnit.Text = su.Name;
                this.txtQWBSPath.Focus();
            }
        }

        void txtProjectUnit_DoubleClick(object sender, EventArgs e)
        {
            StandardUnit su = UCL.Locate("������λά��", StandardUnitExcuteType.OpenSelect) as StandardUnit;
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
                    mnuTree.Items["���ƹ�ѡ�ڵ�"].Enabled = false;
                    mnuTree.Items["ɾ����ѡ�ڵ�"].Enabled = false;
                    mnuTree.Items["�����ѡ�ڵ�"].Enabled = false;
                }
                else
                {
                    mnuTree.Items["���ƹ�ѡ�ڵ�"].Enabled = true;
                    mnuTree.Items["ɾ����ѡ�ڵ�"].Enabled = true;
                    mnuTree.Items["�����ѡ�ڵ�"].Enabled = true;
                }

                if (listCopyNode.Count == 0)
                    mnuTree.Items["ճ���ڵ�"].Enabled = false;
                else
                    mnuTree.Items["ճ���ڵ�"].Enabled = true;

                if (e.Node.Parent == null)
                {
                    mnuTree.Items["ɾ���ڵ�"].Enabled = false;
                    mnuTree.Items["����ͬ���ڵ�"].Enabled = false;
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
                oprNode.ProjectTaskName = txtQWBSName.Text;//�嵥��������
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
                        //�����ӽڵ�ĸ��ڵ���Ҫ��������Tag
                        currNode.Parent.Tag = oprNode.ParentNode;
                        //����tag
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
                        //�����ڵ�Ҫ��Ȩ�޲���
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
                        //Ҫ����ӽڵ�Ľڵ���ǰû���ӽڵ㣬��Ҫ��������Tag
                        if (tvwCategory.SelectedNode.Nodes.Count == 0)
                            tvwCategory.SelectedNode.Tag = oprNode.ParentNode;
                        TreeNode tn = this.tvwCategory.SelectedNode.Nodes.Add(oprNode.Id.ToString(), oprNode.TaskName.ToString());
                        //�����ڵ�Ҫ��Ȩ�޲���
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
                MessageBox.Show("����ɹ���");
                return true;
            }
            catch (Exception exp)
            {
                if (exp.InnerException != null && exp.InnerException.Message.Contains("Υ��ΨһԼ������"))
                    MessageBox.Show("�������Ψһ��");
                else
                    MessageBox.Show("������֯������" + ExceptionUtil.ExceptionMessage(exp));
            }
            return false;
        }


        private bool ValideSave()
        {

            try
            {
                if (txtQWBSName.Text.Trim() == "")
                {
                    MessageBox.Show("�嵥�������Ʋ���Ϊ��!");
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

            //BatchImportLocalize.Load(language);//��ʼ��������Ϣ�����ɰ�ʹ��

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

        bool isSelectNodeInvoke = false;//�Ƿ���ѡ��(���)�ڵ�ʱ����
        bool startNodeCheckedState = false;//��shift��ѡ�ֵܽڵ�ʱ��ʼ�ڵ��ѡ��״̬
        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                //#region ������ڵ�ʱʵ�ֶ�ѡ
                //bool isMultiSelect = false;
                //TreeNode preselectionNode;//Ԥѡ��ڵ�

                //preselectionNode = e.Node;

                //if (currNode != null && currNode.Name != preselectionNode.Name
                //    && currNode.Parent != null && preselectionNode.Parent != null && currNode.Parent.Name == preselectionNode.Parent.Name)//Nameȡ�Ķ����ID
                //    isMultiSelect = true;
                //else
                //    isMultiSelect = false;

                //if (currNode != null)
                //    startNodeCheckedState = currNode.Checked;

                //if (isMultiSelect && (Control.ModifierKeys & Keys.Shift) != 0 && (Control.ModifierKeys & Keys.Control) != 0)//���ͬʱ������ctrl+shift
                //{
                //    int currNodeIndex = currNode.Index;
                //    int preselectNodeIndex = preselectionNode.Index;

                //    int startIndex = currNodeIndex < preselectNodeIndex ? currNodeIndex : preselectNodeIndex;
                //    int endIndex = currNodeIndex < preselectNodeIndex ? preselectNodeIndex : currNodeIndex;

                //    for (int i = startIndex; i <= endIndex; i++)
                //    {
                //        TreeNode tn = currNode.Parent.Nodes[i];

                //        isSelectNodeInvoke = true;//���ñ�־����check�¼��в��ٴ���

                //        if (startNodeCheckedState)//�����ʼ�ڵ㵱ǰΪѡ�У���ȡ��ѡ��
                //        {
                //            TreeNode tempNode = new TreeNode();
                //            tn.BackColor = tempNode.BackColor;
                //            tn.ForeColor = tempNode.ForeColor;

                //            if (listCheckedNode.ContainsKey(tn.Name))
                //                listCheckedNode.Remove(tn.Name);

                //            tn.Checked = false;
                //        }
                //        else//�����ʼ�ڵ㵱ǰΪδѡ�У�������ѡ��
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
                //else if (isMultiSelect && (Control.ModifierKeys & Keys.Shift) != 0)//���ͬʱ������shift
                //{


                //    int currNodeIndex = currNode.Index;
                //    int preselectNodeIndex = preselectionNode.Index;

                //    int startIndex = currNodeIndex < preselectNodeIndex ? currNodeIndex : preselectNodeIndex;
                //    int endIndex = currNodeIndex < preselectNodeIndex ? preselectNodeIndex : currNodeIndex;

                //    for (int i = startIndex; i <= endIndex; i++)
                //    {
                //        TreeNode tn = currNode.Parent.Nodes[i];

                //        isSelectNodeInvoke = true;//���ñ�־����check�¼��в��ٴ���

                //        if (startNodeCheckedState)//�����ʼ�ڵ㵱ǰΪѡ�У���ȡ��ѡ��
                //        {
                //            tn.Checked = false;

                //            TreeNode tempNode = new TreeNode();
                //            tn.BackColor = tempNode.BackColor;
                //            tn.ForeColor = tempNode.ForeColor;

                //            if (listCheckedNode.ContainsKey(tn.Name))
                //                listCheckedNode.Remove(tn.Name);
                //        }
                //        else//�����ʼ�ڵ㵱ǰΪδѡ�У�������ѡ��
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
                MessageBox.Show("��ʾ����" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        /// <summary>
        /// ���ݸ����ͼ����ȡ�����ͼ��𼯺�
        /// </summary>
        /// <param name="parentStructType"></param>
        /// <returns></returns>
        private List<string> GetChildTypeLevel(string parentTypeLevel)
        {
            List<string> list = new List<string>();
            switch (parentTypeLevel)
            {
                case "��Ŀ":
                    list.Add("��λ����");
                    break;
                case "��λ����":
                    list.Add("�ӵ�λ����");
                    list.Add("רҵ");
                    break;
                case "�ӵ�λ����":
                    list.Add("רҵ");
                    list.Add("�ֲ�����");
                    break;
                case "רҵ":
                    list.Add("�ӵ�λ����");
                    list.Add("�ֲ�����");
                    break;
                case "�ֲ�����":
                    list.Add("�ӷֲ�����");
                    list.Add("�����");
                    break;
                case "�ӷֲ�����":
                    list.Add("�����");
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
                //��Ȩ�޵Ľڵ�������϶�����
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
                MessageBox.Show("�������" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        private void DeleteNode()
        {
            try
            {
                if (!ValideDelete())
                    return;
                bool reset = false;
                //���ڵ�ֻ����һ���ӽڵ㣬���Ҹ��ڵ���Ȩ�޲�����ɾ����Ҫ�������ø��ڵ�tag
                if (tvwCategory.SelectedNode.Parent.Nodes.Count == 1)// && ConstMethod.Contains(lstInstance, tvwCategory.SelectedNode.Parent.Tag as CategoryNode)
                {
                    reset = true;
                }
                model.DeleteQWBSManage(oprNode);
                //ɾ����־
                LogData log = new LogData();
                log.BillId = oprNode.Id;
                log.BillType = "�嵥����";
                log.Code = "�������ƣ�" + oprNode.Name;
                log.OperType = "ɾ��";
                log.Descript = "�嵥����ɾ����¼";
                log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                log.ProjectName = oprNode.ProjectName;
                StaticMethod.InsertLogData(log);

                if (reset)
                {
                    QWBSManage org = model.GetQWBSManageById((tvwCategory.SelectedNode.Parent.Tag as QWBSManage).Id);
                    if (org != null)
                        tvwCategory.SelectedNode.Parent.Tag = org;
                }

                //������ƵĽڵ��й�ѡ�ļ��뵽ѡ�м���
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

                if (message.IndexOf("Υ��") > -1 && message.IndexOf("Լ��") > -1)
                {
                    MessageBox.Show("�ýڵ㱻����WBS���������������ã�ɾ��ǰ����ɾ�����õ����ݣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("ɾ���ڵ����" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        private bool ValideDelete()
        {
            try
            {
                TreeNode tn = tvwCategory.SelectedNode;
                if (tn == null)
                {
                    MessageBox.Show("����ѡ��Ҫɾ���Ľڵ㣡");
                    return false;
                }
                if (tn.Parent == null)
                {
                    MessageBox.Show("���ڵ㲻����ɾ����");
                    return false;
                }
                string text = "Ҫɾ����ǰѡ�еĽڵ��𣿸ò����������������ӽڵ�һͬɾ����";
                if (MessageBox.Show(text, "ɾ���ڵ�", MessageBoxButtons.YesNo) == DialogResult.No)
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
                oprNode.OwnerOrgSysCode = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.SysCode;//��֯�����
                oprNode.TaskState = QWBSState.�ƶ�;
               
                
                txtQWBSPath.Text = tvwCategory.SelectedNode.FullPath + "\\";
            }
            catch (Exception exp)
            {
                MessageBox.Show("���ӽڵ����" + exp.Message);
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
            if (parentNode.Parent == null)//���ڵ���벻��У��
                return;

            errorMsg = string.Empty;

            QWBSManage parentType = parentNode.Tag as QWBSManage;
            if (parentType.Code.Length != 9)
            {
                errorMsg = "���ڵ���벻�Ϸ������飡";
                return;
            }
            else if (parentType.Code == childCode)
            {
                errorMsg = "���벻�Ϸ������飡";
                return;
            }

            string parentCode = parentType.Code;
            int level = parentNode.Level;
            if (level == 1)
            {
                if (childCode.IndexOf(parentCode.Substring(0, 2)) == -1)
                {
                    errorMsg = "���벻�Ϸ������飡";
                    return;
                }
            }
            else if (level == 2)
            {
                if (childCode.IndexOf(parentCode.Substring(0, 4)) == -1)
                {
                    errorMsg = "���벻�Ϸ������飡";
                    return;
                }
            }
            else if (level == 3)
            {
                if (childCode.IndexOf(parentCode.Substring(0, 6)) == -1)
                {
                    errorMsg = "���벻�Ϸ������飡";
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
                MessageBox.Show("������֯������" + ExceptionUtil.ExceptionMessage(exp));
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

                        if (root.TaskLevel != QWBSLevel.��Ŀ || root.Name != "������Ŀ")//��һ�μ���  root.Name != projectInfo.Name || 
                        {
                            root.Name = "������Ŀ";//�̶�Ϊ������Ŀ
                            root.TaskLevel = QWBSLevel.��Ŀ;
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
                    root.Name = "������Ŀ";//�̶�Ϊ������Ŀ
                    root.Code = model.GetCode(typeof(QWBSManage));
                    root.TaskLevel = QWBSLevel.��Ŀ;
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
                MessageBox.Show("��ѯ�����������ͳ���" + ExceptionUtil.ExceptionMessage(e));
            }
        }
        //�ؼ����ݲ�����
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
        //�ؼ����ݿ���
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
        //�����Ϣ
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


        //ˢ�¿ؼ���Ϣ
        public override void RefreshControls(MainViewState state)
        {
            switch (state)
            {
                case MainViewState.Modify:

                    this.mnuTree.Items["����"].Enabled = true;
                    this.mnuTree.Items["����ڵ�"].Enabled = true;
                    this.mnuTree.Items["�����ӽڵ�"].Enabled = false;
                    this.mnuTree.Items["����ͬ���ڵ�"].Enabled = false;
                    this.mnuTree.Items["�޸Ľڵ�"].Enabled = false;
                    this.mnuTree.Items["ɾ���ڵ�"].Enabled = false;
                    modify();
                    Controltrue();
                    break;
                case MainViewState.Browser:
                    this.mnuTree.Items["����"].Enabled = false;
                    this.mnuTree.Items["����ڵ�"].Enabled = false;
                    this.mnuTree.Items["�����ӽڵ�"].Enabled = true;
                    this.mnuTree.Items["�޸Ľڵ�"].Enabled = true;
                    if (currNode != null && currNode.Parent == null)
                    {
                        this.mnuTree.Items["����ͬ���ڵ�"].Enabled = false;
                        this.mnuTree.Items["ɾ���ڵ�"].Enabled = false;
                    }
                    else
                    {
                        this.mnuTree.Items["����ͬ���ڵ�"].Enabled = true;
                        this.mnuTree.Items["ɾ���ڵ�"].Enabled = true;
                    }
                    Controlfales();
                    break;
            }
        }
        //���������ɾ�Ĳ���
        void mnuTree_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text.Trim() == "�����ӽڵ�")
            {
                RefreshControls(MainViewState.Modify);
                add_Click(null, new EventArgs());
            }
            if (e.ClickedItem.Text.Trim() == "����ͬ���ڵ�")
            {
                RefreshControls(MainViewState.Modify);
                InsertBrotherNode();
            }
            else if (e.ClickedItem.Text.Trim() == "�޸Ľڵ�")
            {
                RefreshControls(MainViewState.Modify);
            }
            else if (e.ClickedItem.Text.Trim() == "ɾ���ڵ�")
            {
                mnuTree.Hide();
                delete_Click(null, new EventArgs());
                RefreshControls(MainViewState.Browser);
                this.Refresh();
            }
            else if (e.ClickedItem.Text.Trim() == "����")
            {
                mnuTree.Hide();
                RefreshControls(MainViewState.Browser);
                this.tvwCategory_AfterSelect(this.tvwCategory, new TreeViewEventArgs(this.tvwCategory.SelectedNode));
            }
            else if (e.ClickedItem.Text.Trim() == "����ڵ�")
            {
                mnuTree.Hide();
                SaveView();
            }
            else if (e.ClickedItem.Text.Trim() == "���ƹ�ѡ�ڵ�")
            {
                mnuTree.Hide();

                listCopyNode.Clear();
                listCopyNodeAll.Clear();

                GetCheckedNode(tvwCategory.Nodes[0]);

                //���ѡ���ÿ�����ڵ��µ��ӽڵ�֮���Ƿ�����
                foreach (TreeNode tn in listCopyNode)
                {
                    if (SelectNodeIsSuccession(tn) == false)
                    {
                        MessageBox.Show("�ڵ㡰" + tn.FullPath + "����ѡ���˲��������ӽڵ㣬���飡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tn.ExpandAll();

                        listCopyNode.Clear();
                        listCopyNodeAll.Clear();

                        return;
                    }
                }

                //�ж�ѡ���ÿ�����ڵ��Ƿ���ͬһ�����ڵ�
                for (int i = 0; i < listCopyNode.Count - 1; i++)
                {
                    TreeNode nodePrev = listCopyNode[i];
                    TreeNode nodeNext = listCopyNode[i + 1];

                    if (nodePrev.Parent.Name != nodeNext.Parent.Name)
                    {
                        MessageBox.Show("ѡ��Ķ�������ڵ㲻����ͬһ���ڵ㣬�ⲻ���Ͽ����������飡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        listCopyNode.Clear();
                        listCopyNodeAll.Clear();

                        return;
                    }
                }

                RefreshControls(MainViewState.Check);
            }
            else if (e.ClickedItem.Text.Trim() == "ճ���ڵ�")
            {
                mnuTree.Hide();

                #region У�鿽������

                //У�鲻�ܿ缶�𿽱�
                int levelValue = (int)oprNode.TaskLevel;
                QWBSManage copyObj = listCopyNode[0].Tag as QWBSManage;
                if ((levelValue + 1) != (int)copyObj.TaskLevel)
                {
                    MessageBox.Show("ѡ��ڵ�ļ����Ҫճ���Ľڵ�ļ���Ĳ����ϸ��ӹ�ϵ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                #endregion

                SaveCopyNode();

                RefreshControls(MainViewState.Check);
            }
            else if (e.ClickedItem.Text.Trim() == "ɾ����ѡ�ڵ�")
            {
                mnuTree.Hide();
                DeleteCheckedNode();

                RefreshControls(MainViewState.Check);
            }
            else if (e.ClickedItem.Text.Trim() == "�����ѡ�ڵ�")
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
                txtQWBSName.Text = oprNode.TaskName;//���ڵ�����
                txtContractMoney.Text = ClientUtil.ToString(oprNode.CostSumMoney);//�ɱ�����ϼ�
                txtContractQuantity.Text = ClientUtil.ToString(oprNode.ConProQuantity);//��ͬǩ��������
                txtContractPrice.Text = ClientUtil.ToString(oprNode.ConPorPrict);//��ͬǩ���ۺϵ���
                txtProjectUnit.Text = oprNode.ProjectUnitName;//������������λ����
                txtProjectUnit.Tag = oprNode.ProjectUnit;//������������λ
                txtQWBSCharacter.Text = oprNode.TaskCharacter;//�嵥������Ŀ����
                txtPriceUnit.Text = oprNode.ProjectUnitName;//�۸������λ����
                txtPriceUnit.Tag = oprNode.PriceUnit;//�۸������λ
                txtQWBSDesc.Text = oprNode.TaskDigest;//�嵥����ժҪ
                dtStartTime.Value = oprNode.RequiredStartDate;//����Ҫ��ʼʱ��
                dtEndTime.Value = oprNode.RequiredEndDate;//����Ҫ�����ʱ��
                txtQWBSPath.Text = tvwCategory.SelectedNode.FullPath + "\\";
            }
            catch (Exception exp)
            {
                MessageBox.Show("���ӽڵ����" + exp.Message);
            }
        }
        private void GetCheckedNode(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Checked)//�ҵ�ѡ���ÿһ�����ڵ�
                {
                    listCopyNode.Add(tn);
                    continue;
                }
                GetCheckedNode(tn);
            }
        }
        /// <summary>
        /// �ж�ѡ��Ľڵ㼰���ӽڵ��Ƿ�����
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool SelectNodeIsSuccession(TreeNode parentNode)
        {
            //��ѯ�ڵ���
            var listLeafNode = from n in listCheckedNode
                               where (n.Value.Tag as QWBSManage).SysCode.IndexOf((parentNode.Tag as QWBSManage).SysCode) > -1
                               select n;
            foreach (var dic in listLeafNode)
            {
                if (listCopyNodeAll.Keys.Contains(dic.Key) == false)
                    listCopyNodeAll.Add(dic.Key, dic.Value);
                if (dic.Key != parentNode.Name)//��Ҷ�ڵ㲻�Ƕ����ڵ�
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
                        MessageBox.Show("���ڵ㲻����ɾ����");
                        return;
                    }
                    list.Add(dic.Value.Tag as QWBSManage);
                }

                string text = "Ҫɾ����ѡ�����нڵ��𣿸ò����������ǵ������ӽڵ�һͬɾ����";
                if (MessageBox.Show(text, "ɾ���ڵ�", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

                if (model.DeleteQWBSManage(list))//ɾ���ɹ�
                {
                    //��PBS�����Ƴ���Ӧ�Ľڵ�
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

                if (message.IndexOf("Υ��") > -1 && message.IndexOf("Լ��") > -1)
                {
                    MessageBox.Show("��ѡ�ڵ����нڵ㱻����WBS���������������ã�ɾ��ǰ����ɾ�����õ����ݣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                //���渴�ƵĽڵ�
                lst = model.SaveQWBSManages(lst);
                //�����ڵ�Ҫ��Ȩ�޲���
                //(lstInstance as ArrayList).AddRange(lst);
                //�����ƽڵ���¸��ڵ�tag��ֵ
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
        /// ��ȡҪ���ƵĽڵ�
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
                    //catTmp.OrderNo = model.GetMaxOrderNo(parentNode) + 1;//����ž��ø��ƽڵ�������
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
