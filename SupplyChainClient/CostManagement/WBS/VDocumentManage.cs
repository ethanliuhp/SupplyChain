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
using VirtualMachine.Core.Expression;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppSolutionSetUI;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using ImportIntegration;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using System.IO;
using com.think3.PLM.Integration.Client.WS;
using System.Diagnostics;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS
{
    public partial class VDocumentManage : TBasicDataView
    {
        private TreeNode currNode;
        private ProjectTaskTypeTree oprNode = null;
        private ProjectTaskTypeTree curBillManage;
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

        #region �ĵ���������
        /// <summary>
        /// IRP������
        /// </summary>
        private IntergrationFrameWork cFuntions = null;

        /// <summary>
        /// �Ƿ��ʼ�����ػ����ݰ�
        /// </summary>
        private bool isInitLocalizeBag = false;

        private string language = "zhs";

        private ProObjectRelaDocument oprDocument = null;

        #endregion �ĵ�����

        public MWBSManagement model;
        public VDocumentManage(MWBSManagement mot)
        {
            model = mot;
            InitializeComponent();
            InitBegion();
            InitForm();
            tvwCategory.CheckBoxes = false;
            InitData();
        }

        private void InitForm()
        {
            InitEvents();
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            LoadGWBSTreeTree();
        }
        private void InitData()
        {
            this.comType.Items.AddRange(new object[] { "�����ĵ�", "�����ĵ�" });
            this.comDocumentType.Items.AddRange(new object[] { "�����ĵ�", "�����ĵ�" });
            
        }

        private void InitBegion()
        {
            txtDocumentDescript.Enabled = false;
            txtDocumentName.Enabled = false;
            comDocumentType.Enabled = false;
            btnScan.Enabled = false;
            btnSave.Enabled = false;
        }

        private void AllAvailable()
        {
            txtDocumentName.Enabled = true;
            txtDocumentDescript.Enabled = true;
            comDocumentType.Enabled = true;
            btnSave.Enabled = true;
            btnScan.Enabled = true;
            txtPerson.Tag = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;
            txtPerson.Text = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
        }

        private void InitEvents()
        {
            tvwCategory.NodeMouseClick += new TreeNodeMouseClickEventHandler(tvwCategory_NodeMouseClick);
            tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);
            this.btnAddDocument.Click += new EventHandler(btnAddDocument_Click);
            this.btnDownLoadFile.Click += new EventHandler(btnDownLoadFile_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.btnScan.Click += new EventHandler(btnScan_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnUpdateFile.Click += new EventHandler(btnUpdateFile_Click);
        }

        private void LoadGWBSTreeTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();
                IList list = model.GetProjectTaskTypeByInstance();
                if (list.Count > 0)
                {
                    IEnumerable<ProjectTaskTypeTree> listTemp = from t in list.OfType<ProjectTaskTypeTree>()
                                                                where t.CategoryNodeType == NodeType.RootNode || t.ParentNode == null
                                                                select t;

                    if (listTemp != null && listTemp.Count() > 0 && projectInfo != null)
                    {
                        ProjectTaskTypeTree root = listTemp.ElementAt(0);

                        if (root.TypeLevel != ProjectTaskTypeLevel.��Ŀ || root.Name != "������Ŀ")//��һ�μ���  root.Name != projectInfo.Name || 
                        {
                            root.Name = "������Ŀ";//�̶�Ϊ������Ŀ
                            root.TypeLevel = ProjectTaskTypeLevel.��Ŀ;
                            root.SysCode = root.Id + ".";

                            root.TheProjectGUID = projectInfo.Id;
                            root.TheProjectName = projectInfo.Name;

                            if (string.IsNullOrEmpty(root.Code))
                                root.Code = model.GetCode(typeof(ProjectTaskTypeTree));

                            model.SaveProjectTaskTypeTree(root);

                            list = model.GetProjectTaskTypeByInstance();
                            //lstInstance = listAll[1] as IList;
                            //list = listAll[0] as IList;
                        }
                    }
                }
                else
                {
                    IList listAdd = new List<ProjectTaskTypeTree>();

                    ProjectTaskTypeTree root = new ProjectTaskTypeTree();
                    root.Name = "������Ŀ";//�̶�Ϊ������Ŀ
                    root.Code = model.GetCode(typeof(ProjectTaskTypeTree));
                    root.TypeLevel = ProjectTaskTypeLevel.��Ŀ;
                    root.TheProjectGUID = projectInfo.Id;
                    root.TheProjectName = projectInfo.Name;

                    listAdd.Add(root);
                    model.SaveProjectTaskTypeRootNode(listAdd);

                    list = model.GetProjectTaskTypeByInstance();
                    //lstInstance = listAll[1] as IList;
                    //list = listAll[0] as IList;
                }

                foreach (ProjectTaskTypeTree childNode in list)
                {
                    //if (childNode.State == 0)
                    //    continue;

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
                MessageBox.Show("��ѯ�����������ͳ���" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// ѡ�����ڵ�
        /// </summary>
        /// <returns></returns>
        void tvwCategory_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)// && ConstMethod.Contains(lstInstance, e.Node.Tag as CategoryNode)
            {
                tvwCategory.SelectedNode = e.Node;
                oprNode = e.Node.Tag as ProjectTaskTypeTree;
            }
        }

        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                oprNode = tvwCategory.SelectedNode.Tag as ProjectTaskTypeTree;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
            }
        }

        void btnAddDocument_Click(object sender, EventArgs e)
        {
            AllAvailable();
        }
        void btnDownLoadFile_Click(object sender, EventArgs e)
        {
            if (gridFileList.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫ���ص��ĵ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gridFileList.Focus();
                return;
            }

            List<string> listFileIds = new List<string>();
            foreach (DataGridViewRow row in gridFileList.SelectedRows)
            {
                ProObjectRelaDocument doc = row.Tag as ProObjectRelaDocument;
                listFileIds.Add(doc.DocumentGUID);
            }

            try
            {
                if (cFuntions == null)
                {
                    InitIntegrationFramework();
                }

                com.think3.PLM.Integration.Client.BatchDownload bd = new com.think3.PLM.Integration.Client.BatchDownload();
                bd.CreateDownloadSession(listFileIds.ToArray());
                bool downFlag = bd.Download();
                if (downFlag)
                {
                    string privateWorkspacePath = cFuntions.GetPrivateWorkspacePath();
                    if (Directory.Exists(privateWorkspacePath))
                    {
                        Process.Start(privateWorkspacePath.Trim());
                    }
                }
                else
                {
                    MessageBox.Show("����ʧ�ܣ�����������������Ƿ�������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("δ�������������õ������ʵ��") > -1)
                {
                    MessageBox.Show("����ʧ�ܣ������ڵ��ĵ�����", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex), "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void btnDelete_Click(object sender, EventArgs e)
        {
            if (gridFileList.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫɾ�����ĵ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gridFileList.Focus();
                return;
            }

            if (MessageBox.Show("ȷ��Ҫɾ��ѡ���ĵ���", "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                List<ProObjectRelaDocument> listDoc = new List<ProObjectRelaDocument>();
                List<string> listDocId = new List<string>();
                foreach (DataGridViewRow row in gridFileList.SelectedRows)
                {
                    ProObjectRelaDocument doc = row.Tag as ProObjectRelaDocument;
                    listDoc.Add(doc);
                    listDocId.Add(doc.DocumentGUID);
                }
                if (cFuntions == null)
                {
                    InitIntegrationFramework();
                }

                //ɾ��IRP�ĵ���Ϣ
                ErrorStack es = cFuntions.DeleteEntities("Document".ToUpper(), listDocId.ToArray());
                if (es != null)
                {
                    MessageBox.Show("ɾ��IRP�ĵ�ʱ����������Ϣ��" + es.Message, "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //ɾ��MBP�ж�������ĵ���Ϣ
                ObjectQuery oq = new ObjectQuery();
                NHibernate.Criterion.Disjunction dis = new NHibernate.Criterion.Disjunction();
                foreach (ProObjectRelaDocument doc in listDoc)
                {
                    dis.Add(NHibernate.Criterion.Expression.And(
                        NHibernate.Criterion.Expression.Eq("ProObjectGUID", doc.ProObjectGUID),
                        NHibernate.Criterion.Expression.Eq("DocumentGUID", doc.DocumentGUID)));
                }
                oq.AddCriterion(dis);

                IList list = model.ObjectQuery(typeof(ProObjectRelaDocument), oq);

                model.DeleteProObjRelaDoc(list);

                //���б����Ƴ�
                List<int> listRowIndex = new List<int>();
                foreach (DataGridViewRow row in gridFileList.SelectedRows)
                {
                    listRowIndex.Add(row.Index);
                }
                listRowIndex.Sort();
                for (int i = listRowIndex.Count - 1; i > -1; i--)
                {
                    gridFileList.Rows.RemoveAt(listRowIndex[i]);
                }

                txtDocumentName.Text = "";
                txtDocumentDescript.Text = "";
                txtURL.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("ɾ��ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex), "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        void btnExcel_Click(object sender, EventArgs e) { }

        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                #region �ϴ��ļ�
                if (oprNode == null)
                {
                    MessageBox.Show("����ѡ��Ҫ�����ĵ��ڵ㣡");
                    tvwCategory.Focus();
                    return;
                }

                DataTable sourceDataTable = new DataTable();
                sourceDataTable = PartConfigureLogic.CreatSourceDataTable(false, false, false);


                DataRow newRow = sourceDataTable.NewRow();
                //�����ļ����ͺ��ļ�·��
                newRow["TempFile"] = txtURL.Text.ToString();
                newRow["TempKey"] = txtURL.Text.ToString();
                newRow["FileType"] = "DOCUMENT";
                newRow["FileStructureType"] = "FILESTRUCTURE";
                //newRow["PartToFileType"] = "PARTTOFILELINK";

                //����б����ֶμ��ϱ����ֶ�
                newRow["FILENAME"] = txtDocumentName.Text.ToString();

                //�����ļ����ͺ��ļ�·��
                //PartConfigureLogic.AnalyseDataRow(newRow, "C:\\temp\\PWS\\file\\PartFile2\\1016.txt", "FILE", "FILESTRUCTURE", "PARTTOFILELINK");//this.FileStructureType.SelectedValue.ToString()

                sourceDataTable.Rows.Add(newRow);
                

                if (cFuntions == null)
                {
                    InitIntegrationFramework();
                }

                //initional intergration frameWork
                //initional dataPackage
                DataPackage cadImpl = new DataPackage(this, language);

                //cadImpl.SetType("PART", "DOCUMENT", "PARTEBOM", "FILESTRUCTURE", "PARTTOFILELINK");
                cadImpl.SetType("", "DOCUMENT", "", "FILESTRUCTURE", "");

                //fill data into package
                cadImpl.SetSourceDataTable(sourceDataTable);

                //set option item 
                cadImpl.bOnlySaveFile = true;

                //cFuntions = new IntergrationFrameWork();
                cFuntions.Init(cadImpl);

                string message = string.Empty;
                FilesToTransfer fileTransfer = cFuntions.SaveFiles(ref message);

                if (fileTransfer == null || fileTransfer.Files == null || fileTransfer.Files.Length == 0)
                {
                    if (string.IsNullOrEmpty(message) || message.IndexOf("δ�������������õ������ʵ��") > -1)
                        MessageBox.Show("�ļ��ϴ�������ʧ�ܣ�����IRP�ͻ����Ƿ����������������ӵ���������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                        MessageBox.Show("�ļ��ϴ�������ʧ�ܣ��쳣��Ϣ��" + message, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                #endregion �ϴ��ļ�

                #region ����MBP��������ĵ���Ϣ
                IList listDoc = new List<ProObjectRelaDocument>();

                FileToTransfer[] files = fileTransfer.Files;
                for (int i = 0; i < files.Length; i++)
                {
                    FileToTransfer file = files[i];

                    ProObjectRelaDocument doc = new ProObjectRelaDocument();
                    doc.DocumentGUID = file.ObjectID;
                    doc.DocumentName = txtDocumentName.Text.ToString();
                    object desc = txtDocumentDescript.Text.ToString();
                    doc.DocumentDesc = desc == null ? "" : desc.ToString();

                    doc.FileURL = getFileURL(file);

                    doc.DocumentOwner = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;
                    doc.DocumentOwnerName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                    doc.ProObjectGUID = oprNode.Id;
                    doc.ProObjectName = oprNode.GetType().Name;
                    if (projectInfo != null)
                    {
                        doc.TheProjectGUID = projectInfo.Id;
                        doc.TheProjectName = projectInfo.Name;
                    }

                    listDoc.Add(doc);
                }

                listDoc = model.SaveOrUpdate(listDoc);

                foreach (ProObjectRelaDocument doc in listDoc)
                {
                    InsertIntoGridDocument(doc);
                }

                //gridFileList.Rows.Clear();

                #endregion ����MBP��������ĵ���Ϣ

                MessageBox.Show("�ļ�����ɹ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        void btnScan_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "�����ļ�(*.*)|*.*";
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] strFiles = openFileDialog1.FileNames;
                int iCount = strFiles.Length;

                for (int i = 0; i < iCount; i++)
                {
                    txtDocumentName.Text = Path.GetFileName(strFiles[i]);
                    txtURL.Text = strFiles[i];
                    //    InsertToFileList(strFiles[i]);
                }
                gridFileList.AutoResizeColumns();
            }
        }
        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                InitBegion();
                curBillManage = new ProjectTaskTypeTree();
                curBillManage.TheProjectGUID = oprNode.Id;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("ProObjectGUID", curBillManage.TheProjectGUID));
                IList lists = model.ObjectQuery(typeof(ProObjectRelaDocument), oq);
                if (lists.Count <= 0 || lists == null)
                {
                    gridFileList.Rows.Clear();
                    return;
                }
                gridFileList.Rows.Clear();
                foreach (ProObjectRelaDocument var in lists)
                {
                    int i = this.gridFileList.Rows.Add();
                    this.gridFileList[DocumentName.Name, i].Value = var.DocumentName;
                    this.gridFileList[DocumentPath.Name, i].Value = var.FileURL;
                    this.gridFileList[DocumentDesc.Name, i].Value = var.DocumentDesc;
                    //this.gridFileList[DocumentType.Name,i].Value = var.
                    this.gridFileList[UploadDate.Name, i].Value = var.SubmitTime;
                    this.gridFileList[UploadPerson.Name, i].Value = var.DocumentOwnerName;
                    this.gridFileList[UploadPerson.Name, i].Tag = var.DocumentOwner;
                    this.gridFileList.Rows[i].Tag = var;
                }

                return;
            }
            catch (Exception err)
            {
                throw err;
                //MessageBox.Show("����ӳ�����" + StaticMethod.ExceptionMessage(e));
                //return false;
            }
        }
        void btnUpdateFile_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = gridFileList.CurrentRow;
            if (dr == null || dr.IsNewRow) return;
            txtDocumentDescript.Text = ClientUtil.ToString(gridFileList.CurrentRow.Cells[DocumentDesc.Name].Value);
            txtDocumentName.Text = ClientUtil.ToString(gridFileList.CurrentRow.Cells[DocumentName.Name].Value);
            txtURL.Text = ClientUtil.ToString(gridFileList.CurrentRow.Cells[DocumentPath.Name].Value);
            comDocumentType.SelectedItem = ClientUtil.ToString(gridFileList.CurrentRow.Cells[DocumentType.Name].Value);
            AllAvailable();
        }

        private void InitIntegrationFramework()
        {
            string userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
            string orgId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
            cFuntions = new IntergrationFrameWork(true, userName, orgId);

            DataPackage cadImpl = new DataPackage(this, language);
            cFuntions.Init(cadImpl);

            BatchImportLocalize.Load(language);//��ʼ��������Ϣ�����ɰ�ʹ��

            isInitLocalizeBag = true;
        }
        private void InsertIntoGridDocument(ProObjectRelaDocument doc)
        {
            int index = gridFileList.Rows.Add();
            DataGridViewRow row = gridFileList.Rows[index];
            row.Cells[DocumentName.Name].Value = doc.DocumentName;
            row.Cells[UploadPerson.Name].Value = doc.DocumentOwnerName;
            row.Cells[UploadDate.Name].Value = doc.SubmitTime.ToString();
            row.Cells[DocumentDesc.Name].Value = doc.DocumentDesc;

            row.Tag = doc;
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
    }
}
