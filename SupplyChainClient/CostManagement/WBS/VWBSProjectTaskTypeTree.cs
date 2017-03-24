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
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using IRPServiceModel.Domain.Document;
using VirtualMachine.Core.Expression;
using VirtualMachine.Patterns.BusinessEssence.Domain;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS
{
    public partial class VWBSProjectTaskTypeTree : TBasicDataView
    {
        private TreeNode currNode;
        private ProjectTaskTypeTree oprNode = null;
        private bool isNew = true;
        //��Ȩ�޵�ҵ����֯
        private IList lstInstance;
        //Ψһ����
        private string uniqueCode;
        Color qibiaoColor = Color.Blue;
        Color guobiaoColor = Color.Black;
        Color InvalidColor = Color.Red;
        Color ValidColor = System.Drawing.Color.Black;
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

        private ProObjectRelaDocument oprDocument = null;

        string fileObjectType = string.Empty;
        string FileStructureType = string.Empty;

        string userName = string.Empty;
        string jobId = string.Empty;

        #endregion �ĵ�����

        public MWBSManagement model;

        IList saveDocStencilList = new List<ProTaskTypeDocumentStencil>();
        IList docStencillist = new List<ProTaskTypeDocumentStencil>();

        ProTaskTypeDocumentStencil updateDocStencil = new ProTaskTypeDocumentStencil();
        int updateRowIndex = -1;

        public VWBSProjectTaskTypeTree(MWBSManagement mot)
        {
            model = mot;
            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();
            txtGuoBiaoColorFlag.BackColor = guobiaoColor;
            lblGuobiao.ForeColor = guobiaoColor;
            txtQiBiaoColorFlag.BackColor = qibiaoColor;
            lblQibiao.ForeColor = qibiaoColor;
            //cbLevel.Items.Add("");
            foreach (string level in Enum.GetNames(typeof(ProjectTaskTypeLevel)))
            {
                cbLevel.Items.Add(level);

                listTaskTypeLevel.Add((int)VirtualMachine.Component.Util.EnumUtil<ProjectTaskTypeLevel>.FromDescription(level), level);
            }
            cbLevel.SelectedIndex = 0;

            //cbTypeStandard.Items.Add("");
            foreach (string stand in Enum.GetNames(typeof(ProjectTaskTypeStandard)))
            {
                cbTypeStandard.Items.Add(stand);
            }
            cbTypeStandard.SelectedIndex = 0;

            //���Ҫ��
            IList list = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetBasicDataByName(VBasicDataOptr.WBS_CheckRequire);
            if (list != null)
            {
                foreach (BasicDataOptr bdo in list)
                {
                    cbListCheckRequire.Items.Add(bdo.BasicName);
                }
            }

            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();

            RefreshState(MainViewState.Browser);

            LoadProjectTaskTypeTreeTree();

            List<string> listParams = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetUploadFileParamsByKB();
            fileObjectType = listParams[0];
            FileStructureType = listParams[1];

            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;

            foreach (string infoType in Enum.GetNames(typeof(ProjectTaskTypeCheckFlag)))
            {
                BrownCheckBatchFlag.Items.Add(infoType);
                cmbCheckFlag.Items.Add(infoType);
            }
            foreach (string infoType in Enum.GetNames(typeof(ProjectTaskTypeAlterMode)))
            {
                BrownAlterMode.Items.Add(infoType);
                cmbAlterMode.Items.Add(infoType);
            }

        }

        private void InitEvents()
        {
            tvwCategory.NodeMouseClick += new TreeNodeMouseClickEventHandler(tvwCategory_NodeMouseClick);

            tvwCategory.AfterCheck += new TreeViewEventHandler(tvwCategory_AfterCheck);
            tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);

            mnuTree.ItemClicked += new ToolStripItemClickedEventHandler(mnuTree_ItemClicked);

            tvwCategory.ItemDrag += new ItemDragEventHandler(tvwCategory_ItemDrag);
            tvwCategory.DragEnter += new DragEventHandler(tvwCategory_DragEnter);
            tvwCategory.DragDrop += new DragEventHandler(tvwCategory_DragDrop);
            tvwCategory.DragOver += new DragEventHandler(tvwCategory_DragOver);


            //�ĵ�����
            btnDownLoadDocument.Click += new EventHandler(btnDownLoadDocument_Click);
            //btnOpenDocument.Click += new EventHandler(btnOpenDocument_Click);

            btnUpdateDocument.Click += new EventHandler(btnUpdateDocument_Click);
            btnDeleteDocument.Click += new EventHandler(btnDeleteDocument_Click);

            btnChangeFile.Click += new EventHandler(btnChangeFile_Click);
            btnSaveUpdate.Click += new EventHandler(btnSaveDocument_Click);

            //btnBrownDocument.Click += new EventHandler(btnBrownDocument_Click);
            btnRemoveFile.Click += new EventHandler(btnRemoveFile_Click);
            btnClearAllFile.Click += new EventHandler(btnClearAllFile_Click);
            btnBatchSave.Click += new EventHandler(btnBatchSave_Click);

            gridDocument.CellClick += new DataGridViewCellEventHandler(gridDocument_CellClick);
            gridDocument.SelectionChanged += new EventHandler(gridDocument_SelectionChanged);

            btnSearchStencil.Click += new EventHandler(btnSearchStencil_Click);
            this.linkPublish.LinkClicked += new LinkLabelLinkClickedEventHandler(linkPublish_Click);
            this.linkInvalid.LinkClicked += new LinkLabelLinkClickedEventHandler(linkInvalid_Click);

        }



        #region  ѡ��ģ�� *******************************************************************************************************
        /// <summary>
        /// ѡ��ģ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearchStencil_Click(object sender, EventArgs e)
        {
            VDocumentsSelect vsdl = new VDocumentsSelect("add");
            vsdl.ShowDialog();
            IList resultDocumentList = vsdl.ResultList;
            if (resultDocumentList == null || resultDocumentList.Count == 0)
            {
                return;
            }
            if (resultDocumentList.Count > 0 && resultDocumentList != null)
            {
                gridBrownFileList.Rows.Clear();
                foreach (DocumentMaster pdoc in resultDocumentList)
                {
                    int rowIndex = gridBrownFileList.Rows.Add();
                    gridBrownFileList[BrownFileName.Name, rowIndex].Value = pdoc.Name;
                    gridBrownFileList[BrownFileDesc.Name, rowIndex].Value = pdoc.Description;
                    gridBrownFileList[BrownCheckBatchFlag.Name, rowIndex].Value = "�����Ŀ����ڵ�";
                    gridBrownFileList[BrownAlterMode.Name, rowIndex].Value = "������֤����";
                    gridBrownFileList[BrownDocumentCode.Name, rowIndex].Value = pdoc.Code;
                    gridBrownFileList.Rows[rowIndex].Tag = pdoc;
                }
            }
        }
        #endregion


        #region �ĵ�����***********************************************************************************************
        //ѡ���ĵ�
        void gridDocument_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (oprDocument == null && e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                ProObjectRelaDocument doc = gridDocument.SelectedRows[0].Tag as ProObjectRelaDocument;
                if (doc != null)
                {
                    txtDocumentName.Text = doc.DocumentName;
                    txtDocumentDesc.Text = doc.DocumentDesc;
                    //txtDocumentPath.Text = doc.FileURL;

                    txtDocumentName.ReadOnly = true;
                    txtDocumentDesc.ReadOnly = true;
                    //txtDocumentPath.ReadOnly = true;

                    btnChangeFile.Enabled = false;
                    btnSaveUpdate.Enabled = false;
                }

                oprDocument = doc;
            }
        }
        void gridDocument_SelectionChanged(object sender, EventArgs e)
        {
            if (gridDocument.SelectedRows.Count == 0)
                return;

            ProObjectRelaDocument doc = gridDocument.SelectedRows[0].Tag as ProObjectRelaDocument;
            if (doc != null)
            {
                txtDocumentName.Text = doc.DocumentName;
                txtDocumentDesc.Text = doc.DocumentDesc;
                //txtDocumentPath.Text = "";

                txtDocumentName.ReadOnly = true;
                txtDocumentDesc.ReadOnly = true;
                //txtDocumentPath.ReadOnly = true;

                btnChangeFile.Enabled = false;
                btnSaveUpdate.Enabled = false;
            }
            oprDocument = doc;
        }

        #region �����ĵ�***************************************************************************************************
        //�����ĵ�
        void btnDownLoadDocument_Click(object sender, EventArgs e)
        {
            if (gridDocument.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫ���ص��ĵ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gridDocument.Focus();
                return;
            }

            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (DataGridViewRow row in gridDocument.SelectedRows)
            {
                ProTaskTypeDocumentStencil stencilDoc = row.Tag as ProTaskTypeDocumentStencil;
                dis.Add(Expression.Eq("Id", stencilDoc.ProDocumentMasterID));
            }
            oq.AddCriterion(dis);
            IList docMasterList = model.ObjectQuery(typeof(DocumentMaster), oq);
            if (docMasterList != null && docMasterList.Count > 0)
            {
                VDocumentsDownloadOrOpen frm = new VDocumentsDownloadOrOpen(docMasterList);
                frm.ShowDialog();
            }
        }
        #endregion
        #region  Ԥ��************************************************************************************************************
        //Ԥ��
        void btnOpenDocument_Click(object sender, EventArgs e)
        {
            if (gridDocument.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫ�򿪵��ĵ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gridDocument.Focus();
                return;
            }

            //List<string> listFileIds = new List<string>();
            //foreach (DataGridViewRow row in gridDocument.SelectedRows)
            //{
            //    ProObjectRelaDocument doc = row.Tag as ProObjectRelaDocument;
            //    listFileIds.Add(doc.DocumentGUID);
            //}

            List<PLMWebServicesByKB.ProjectDocument> docList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.ProjectDocument>();
            PLMWebServicesByKB.ProjectDocument[] projectDocList = null;

            foreach (DataGridViewRow row in gridDocument.SelectedRows)
            {
                ProTaskTypeDocumentStencil docStencil = row.Tag as ProTaskTypeDocumentStencil;
                PLMWebServicesByKB.ProjectDocument doc = new Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.ProjectDocument();
                doc.EntityID = docStencil.ProDocumentMasterID;
                docList.Add(doc);
            }


            try
            {
                PLMWebServicesByKB.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.DownLoadDocumentByKB(docList.ToArray(), null,
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_UserName, Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_JobId,
                    null, out projectDocList);
                if (es != null)
                {
                    MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + GetExceptionMessage(es), "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                //           object[] listFileBytes = null;
                //           string[] listFileNames = null;

                //           PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.DownLoadDocumentByCustom(out listFileBytes, out listFileNames, listFileIds.ToArray(),
                //null, userName, jobId, null);

                //           if (es != null)
                //           {
                //               MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + GetExceptionMessage(es), "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //               return;
                //           }
                List<string> errorList = new List<string>();
                List<string> listFileFullPaths = new List<string>();
                if (projectDocList != null)
                {
                    string fileFullPath = AppDomain.CurrentDomain.BaseDirectory + "TempFilePreview\\" + Guid.NewGuid().ToString().Replace("-", "");
                    if (!Directory.Exists(fileFullPath))
                        Directory.CreateDirectory(fileFullPath);

                    for (int i = 0; i < projectDocList.Length; i++)
                    {
                        //byte[] by = listFileBytes[i] as byte[];
                        //if (by != null && by.Length > 0)
                        //{
                        string fileName = projectDocList[i].FileName;

                        if (projectDocList[i].FileDataByte == null || projectDocList[i].FileDataByte.Length <= 0 || fileName == null)
                        {
                            string strName = projectDocList[i].Code + projectDocList[i].Name;
                            errorList.Add(strName);
                            continue;
                        }

                        string tempFileFullPath = fileFullPath + @"\\" + fileName;

                        CreateFileFromByteAarray(projectDocList[i].FileDataByte, tempFileFullPath);

                        listFileFullPaths.Add(tempFileFullPath);
                        //}
                    }
                }

                foreach (string fileFullPath in listFileFullPaths)
                {
                    FileInfo file = new FileInfo(fileFullPath);

                    //����һ��ProcessStartInfoʵ��
                    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                    //�����������̵ĳ�ʼĿ¼
                    info.WorkingDirectory = file.DirectoryName;//Application.StartupPath; 
                    //�����������̵�Ӧ�ó�����ĵ���
                    info.FileName = file.Name;
                    //�����������̵Ĳ���
                    info.Arguments = "";
                    //�����ɰ�������������Ϣ�Ľ�����Դ
                    try
                    {
                        System.Diagnostics.Process.Start(info);
                    }
                    catch (System.ComponentModel.Win32Exception we)
                    {
                        MessageBox.Show(this, we.Message);
                    }
                }
                if (errorList != null && errorList.Count > 0)
                {
                    string str = "";
                    foreach (string s in errorList)
                    {
                        str += (s + ";");
                    }
                    MessageBox.Show(str + "��" + errorList.Count + "���ļ����޷�Ԥ�����ļ������ڻ�δָ����ʽ��");
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
        #endregion
        public static void CreateFileFromByteAarray(byte[] stream, string fileName)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                fs.Write(stream, 0, stream.Length);
                fs.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region �޸��ĵ�**********************************************************************************************
        //�޸��ĵ�
        void btnUpdateDocument_Click(object sender, EventArgs e)
        {
            if (gridDocument.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫ�޸ĵ��ĵ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gridDocument.Focus();
                return;
            }
            else if (gridDocument.SelectedRows.Count > 1)
            {
                MessageBox.Show("һ��ֻ���޸�һ�У�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gridDocument.Focus();
                return;
            }
            //oprDocument = gridDocument.SelectedRows[0].Tag as ProObjectRelaDocument;
            //ProTaskTypeDocumentStencil docStencil = gridDocument.SelectedRows[0].Tag as ProTaskTypeDocumentStencil;
            //updateDocStencil = docStencil;
            updateDocStencil = gridDocument.SelectedRows[0].Tag as ProTaskTypeDocumentStencil;
            updateRowIndex = gridDocument.SelectedRows[0].Index;
            txtDocumentName.ReadOnly = false;
            txtDocumentDesc.ReadOnly = false;
            txtDocumentName.Enabled = txtDocumentDesc.Enabled = true;
            cmbCheckFlag.Enabled = cmbAlterMode.Enabled = true;
            btnChangeFile.Enabled = true;
            btnSaveUpdate.Enabled = true;
            txtDocumentName.Text = updateDocStencil.StencilName;
            txtDocumentDesc.Text = updateDocStencil.StencilDescription;
            cmbCheckFlag.Text = updateDocStencil.InspectionMark.ToString();
            cmbAlterMode.Text = updateDocStencil.AlarmMode.ToString();
            txtDocumentName.Focus();
        }
        #endregion
        #region  ����ģ��************************************************************************************************
        //����ģ��               �����ļ�
        void btnChangeFile_Click(object sender, EventArgs e)
        {
            VDocumentsSelect vsdl = new VDocumentsSelect("update");
            vsdl.ShowDialog();
            IList resultUpdateDocStencil = vsdl.ResultList;
            if (resultUpdateDocStencil == null || resultUpdateDocStencil.Count == 0)
            {
                return;
            }
            if (resultUpdateDocStencil.Count > 0 && resultUpdateDocStencil != null)
            {
                foreach (DocumentMaster doc in resultUpdateDocStencil)
                {
                    txtDocumentName.Text = doc.Name;
                    txtDocumentName.Tag = doc;
                    txtDocumentDesc.Text = doc.Description;
                }
            }
        }
        #endregion
        #region �����޸�************************************************************************************************************************
        //�����޸�
        void btnSaveDocument_Click(object sender, EventArgs e)
        {
            updateDocStencil.StencilName = txtDocumentName.Text;
            updateDocStencil.StencilDescription = txtDocumentDesc.Text;
            updateDocStencil.InspectionMark = VirtualMachine.Component.Util.EnumUtil<ProjectTaskTypeCheckFlag>.FromDescription(cmbCheckFlag.Text);
            updateDocStencil.AlarmMode = VirtualMachine.Component.Util.EnumUtil<ProjectTaskTypeAlterMode>.FromDescription(cmbAlterMode.Text);
            if (txtDocumentName.Tag != null)
            {
                DocumentMaster doc = txtDocumentName.Tag as DocumentMaster;
                updateDocStencil.ProDocumentMasterID = doc.Id;

            }
            ProTaskTypeDocumentStencil docStenticl = model.SaveOrUpDateDocStencil(updateDocStencil);
            if (docStencillist != null)
            {
                MessageBox.Show("���³ɹ���");
                txtDocumentName.Enabled = txtDocumentDesc.Enabled = false;
                cmbCheckFlag.Enabled = cmbAlterMode.Enabled = false;
                txtDocumentName.Text = txtDocumentDesc.Text = "";
                txtDocumentName.Tag = null;
                cmbCheckFlag.Text = "";
                cmbAlterMode.Text = "";
                //gridBrownFileList[BrownFileName.Name, updateRowIndex].Value = docStenticl.StencilName;
                //gridBrownFileList[BrownFileDesc.Name, updateRowIndex].Value = docStenticl.StencilDescription;
                //gridBrownFileList[BrownCheckBatchFlag.Name, updateRowIndex].Value = docStenticl.InspectionMark.ToString();
                //gridBrownFileList[BrownAlterMode.Name, updateRowIndex].Value = docStenticl.AlarmMode.ToString();
                ////gridBrownFileList[BrownDocumentCode.Name, updateRowIndex].Value = docStenticl.Code;
                //gridBrownFileList.Rows[updateRowIndex].Tag = docStenticl;

                gridDocument[DocumentName.Name, updateRowIndex].Value = docStenticl.StencilName;
                gridDocument[DocumentDesc.Name, updateRowIndex].Value = docStenticl.StencilDescription;
                gridDocument[DocumentCheckBatchFlag.Name, updateRowIndex].Value = docStenticl.InspectionMark.ToString();
                gridDocument[DocumentAlterMode.Name, updateRowIndex].Value = docStenticl.AlarmMode.ToString();
                gridDocument.Rows[updateRowIndex].Tag = docStenticl;

                updateDocStencil = null;
                updateRowIndex = -1;
            }
            else
            {
                MessageBox.Show("ʧ�ܸ��£�");
            }
            txtDocumentName.Tag = null;
            #region  �޸ģ�ע�ͣ�
            //try
            //{
            //    if (!txtDocumentName.ReadOnly && oprDocument != null)
            //    {
            //        if (txtDocumentName.Text.Trim() == "")
            //        {
            //            MessageBox.Show("�ĵ����Ʋ���Ϊ�գ�");
            //            txtDocumentName.Focus();
            //            return;
            //        }
            //        List<string> listFileIds = new List<string>();
            //        listFileIds.Add(oprDocument.DocumentGUID);

            //        ObjectQuery oq = new ObjectQuery();
            //        oq.AddCriterion(NHibernate.Criterion.Expression.Eq("Id", oprDocument.Id));
            //        oprDocument = model.ObjectQuery(typeof(ProObjectRelaDocument), oq)[0] as ProObjectRelaDocument;

            //        oprDocument.DocumentName = txtDocumentName.Text.Trim();
            //        oprDocument.DocumentDesc = txtDocumentDesc.Text.Trim();

            //        if (txtDocumentPath.Text != "" && File.Exists(txtDocumentPath.Text))//�������ļ�
            //        {
            //            FileInfo file = new FileInfo(txtDocumentPath.Text);

            //            List<byte[]> listFileBytes = new List<byte[]>();
            //            List<string> listFileNames = new List<string>();
            //            List<PLMWebServices.DictionaryObjectInfo> listDicKeyValue = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.DictionaryObjectInfo>();

            //            FileStream fileStream = file.OpenRead();
            //            int FileLen = (int)file.Length;
            //            Byte[] FileData = new Byte[FileLen];
            //            //���ļ����ݷŵ�FileData�������ʵ���У�0��������ָ�����ʼλ��,FileLen����ָ��Ľ���λ��
            //            fileStream.Read(FileData, 0, FileLen);

            //            listFileBytes.Add(FileData);


            //            string fileName = txtDocumentName.Text.Trim();
            //            string fileDesc = txtDocumentDesc.Text.Trim();


            //            listFileNames.Add(fileName + Path.GetExtension(file.Name));


            //            List<string> listNames = new List<string>();
            //            List<object> listValues = new List<object>();

            //            listNames.Add("Name");
            //            listValues.Add(fileName);

            //            listNames.Add("DOCUMENTTITLE");
            //            listValues.Add(fileName);

            //            listNames.Add("DOCUMENTDESCRIPTION");
            //            listValues.Add(fileDesc);

            //            PLMWebServices.DictionaryObjectInfo dic = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.DictionaryObjectInfo();
            //            dic.InfoNames = listNames.ToArray();
            //            dic.InfoValues = listValues.ToArray();

            //            listDicKeyValue.Add(dic);


            //            PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.UpdateDocumentByCustom(listFileIds.ToArray(),
            //                 fileObjectType, listFileBytes.ToArray(), listFileNames.ToArray(), "1", listDicKeyValue.ToArray(), null, userName, jobId, null);

            //            if (es != null)
            //            {
            //                MessageBox.Show("�޸��ļ�ʧ�ܣ��쳣��Ϣ��" + GetExceptionMessage(es), "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                return;
            //            }
            //        }

            //        oprDocument = model.SaveOrUpdateProObjRelaDoc(oprDocument);

            //        UpdateDocument(oprDocument);

            //        txtDocumentName.ReadOnly = true;
            //        txtDocumentDesc.ReadOnly = true;

            //        btnChangeFile.Enabled = false;
            //        btnSaveUpdate.Enabled = false;

            //        MessageBox.Show("�޸ĳɹ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex));
            //}
            #endregion
        }
        #endregion
        #region ɾ���ĵ�*********************************************************************************************************
        //ɾ���ĵ�
        void btnDeleteDocument_Click(object sender, EventArgs e)
        {
            if (gridDocument.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫɾ�����ĵ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gridDocument.Focus();
                return;
            }

            if (MessageBox.Show("ȷ��Ҫɾ��ѡ���ĵ���", "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                IList deleteDocStencilList = new List<ProTaskTypeDocumentStencil>();
                foreach (DataGridViewRow row in gridDocument.SelectedRows)
                {
                    ProTaskTypeDocumentStencil docStencil = row.Tag as ProTaskTypeDocumentStencil;
                    deleteDocStencilList.Add(docStencil);
                }

                bool flag = model.DeleteDocStencilList(deleteDocStencilList);

                if (flag)
                {
                    foreach (DataGridViewRow row in gridDocument.SelectedRows)
                    {
                        gridDocument.Rows.Remove(row);
                    }
                    MessageBox.Show("ɾ���ɹ���");
                }
                else
                {
                    MessageBox.Show("ɾ��ʧ�ܣ�");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("ɾ��ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex), "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        //�Ƴ�ѡ���ļ�
        void btnRemoveFile_Click(object sender, EventArgs e)
        {
            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridBrownFileList.SelectedRows)
            {
                listRowIndex.Add(row.Index);
            }
            listRowIndex.Sort();
            for (int i = listRowIndex.Count - 1; i > -1; i--)
            {
                gridBrownFileList.Rows.RemoveAt(listRowIndex[i]);
            }
        }
        //ȫ�����
        void btnClearAllFile_Click(object sender, EventArgs e)
        {
            gridBrownFileList.Rows.Clear();
        }

        #region ��������********************************************************************************************************************
        //��������********************************************************************************************************************
        void btnBatchSave_Click(object sender, EventArgs e)
        {
            if (gridBrownFileList.Rows.Count > 0)
            {
                saveDocStencilList.Clear();
                foreach (DataGridViewRow row in gridBrownFileList.Rows)
                {
                    DocumentMaster doc = row.Tag as DocumentMaster;
                    ProTaskTypeDocumentStencil docStencil = new ProTaskTypeDocumentStencil();
                    ProjectTaskTypeTree type = tvwCategory.SelectedNode.Tag as ProjectTaskTypeTree;
                    docStencil.ProTaskType = type;
                    docStencil.ProTaskTypeName = type.Name;
                    docStencil.SysCode = type.SysCode;
                    docStencil.ProDocumentMasterID = doc.Id;
                    docStencil.InspectionMark = VirtualMachine.Component.Util.EnumUtil<ProjectTaskTypeCheckFlag>.FromDescription(row.Cells[BrownCheckBatchFlag.Name].Value);
                    docStencil.ProjectCode = "KB";
                    docStencil.AlarmMode = VirtualMachine.Component.Util.EnumUtil<ProjectTaskTypeAlterMode>.FromDescription(row.Cells[BrownAlterMode.Name].Value);
                    //docStencil.ControlWorkflowName = "";
                    docStencil.StencilName = doc.Name;
                    docStencil.StencilCode = doc.Code;
                    docStencil.StencilDescription = doc.Description;
                    docStencil.ProjectName = "֪ʶ��";
                    docStencil.DocumentCateCode = doc.CategoryCode;
                    docStencil.DocumentCateName = doc.CategoryName;
                    //model.SaveDocStencil(docStencil);
                    saveDocStencilList.Add(docStencil);
                }
                docStencillist = model.saveDaoStencilList(saveDocStencilList);

                if (docStencillist != null)
                {
                    MessageBox.Show("����ɹ���");
                    foreach (ProTaskTypeDocumentStencil docStencil in docStencillist)
                    {
                        int rowIndex = gridDocument.Rows.Add();
                        gridDocument[DocumentName.Name, rowIndex].Value = docStencil.StencilName;
                        gridDocument[DocumentCode.Name, rowIndex].Value = docStencil.StencilCode;
                        gridDocument[DocumentCheckBatchFlag.Name, rowIndex].Value = docStencil.InspectionMark;
                        gridDocument[DocumentAlterMode.Name, rowIndex].Value = docStencil.AlarmMode;
                        gridDocument[DocumentDesc.Name, rowIndex].Value = docStencil.StencilDescription;
                        gridDocument.Rows[rowIndex].Tag = docStencil;
                    }
                    gridBrownFileList.Rows.Clear();
                }
                else
                {
                    MessageBox.Show("����ʧ�ܣ�");
                }
            }
            else
            {
                MessageBox.Show("δ�����ݣ�");
            }
            #region  ��������(ע��)
            //    try
            //    {

            //        if (gridBrownFileList.Rows.Count == 0)
            //        {
            //            MessageBox.Show("����ѡ��Ҫ������ļ���");
            //            btnBrownDocument.Focus();
            //            return;
            //        }
            //        else if (oprNode == null)
            //        {
            //            MessageBox.Show("����ѡ��Ҫ�����ĵ�����ڵ㣡");
            //            tvwCategory.Focus();
            //            return;
            //        }

            //        //У������
            //        foreach (DataGridViewRow row in gridBrownFileList.Rows)
            //        {
            //            if (row.Cells[BrownFileName.Name].Value == null)
            //            {
            //                MessageBox.Show("�ļ�������Ϊ�գ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //                gridBrownFileList.CurrentCell = row.Cells[BrownFileName.Name];
            //                gridBrownFileList.BeginEdit(false);
            //                return;
            //            }
            //        }

            //        int rowCount = gridBrownFileList.Rows.Count;

            //        progressBarDocUpload.Minimum = 0;
            //        progressBarDocUpload.Maximum = rowCount >= 10 ? rowCount + 1 : 11;
            //        progressBarDocUpload.Value = 1;


            //        //��ʾ��������ʹ�õ����ϴ�ģʽ
            //        List<byte[]> listFileBytes = new List<byte[]>();
            //        List<string> listFileNames = new List<string>();
            //        List<PLMWebServices.DictionaryObjectInfo> listDicKeyValue = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.DictionaryObjectInfo>();

            //        IList listDoc = new List<ProObjectRelaDocument>();
            //        for (int rowIndex = gridBrownFileList.Rows.Count - 1; rowIndex > -1; rowIndex--)
            //        {
            //            DataGridViewRow row = gridBrownFileList.Rows[rowIndex];

            //            listFileBytes.Clear();
            //            listFileNames.Clear();
            //            listDicKeyValue.Clear();

            //            string filePath = row.Cells[BrownFilePath.Name].Value.ToString();

            //            FileInfo file = new FileInfo(filePath);
            //            if (file.Exists)
            //            {
            //                #region �ϴ��ļ�

            //                FileStream fileStream = file.OpenRead();
            //                int FileLen = (int)file.Length;
            //                Byte[] FileData = new Byte[FileLen];
            //                //���ļ����ݷŵ�FileData�������ʵ���У�0��������ָ�����ʼλ��,FileLen����ָ��Ľ���λ��
            //                fileStream.Read(FileData, 0, FileLen);

            //                listFileBytes.Add(FileData);


            //                string fileName = row.Cells[BrownFileName.Name].Value.ToString();
            //                listFileNames.Add(fileName + Path.GetExtension(file.Name));

            //                object fileDesc = row.Cells[BrownFileDesc.Name].Value;

            //                List<string> listNames = new List<string>();
            //                List<object> listValues = new List<object>();

            //                //listNames.Add("Code");
            //                //listValues.Add(fileName);

            //                //listNames.Add("DOCUMENTNUMBER");
            //                //listValues.Add(fileName);

            //                listNames.Add("Name");
            //                listValues.Add(fileName);

            //                listNames.Add("DOCUMENTTITLE");
            //                listValues.Add(fileName);

            //                listNames.Add("DOCUMENTDESCRIPTION");
            //                listValues.Add(fileDesc);

            //                PLMWebServices.DictionaryObjectInfo dic = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.DictionaryObjectInfo();
            //                dic.InfoNames = listNames.ToArray();
            //                dic.InfoValues = listValues.ToArray();

            //                listDicKeyValue.Add(dic);


            //                string[] listFileIds = null;
            //                PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.AddDocumentByCustom(out listFileIds,listFileBytes.ToArray(),
            //                    listFileNames.ToArray(), fileObjectType, "1", listDicKeyValue.ToArray(), null, userName, jobId, null);

            //                if (es != null)
            //                {
            //                    MessageBox.Show("�ļ���" + fileName + "���ϴ���������ʧ�ܣ��쳣��Ϣ��" + GetExceptionMessage(es), "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                    return;
            //                }
            //                #endregion �ϴ��ļ�

            //                #region ����MBP��������ĵ���Ϣ
            //                if (listFileIds != null)
            //                {
            //                    listDoc.Clear();

            //                    for (int i = 0; i < listFileIds.Length; i++)
            //                    {
            //                        string fileId = listFileIds[i];
            //                        ProObjectRelaDocument doc = new ProObjectRelaDocument();
            //                        doc.DocumentGUID = fileId;
            //                        doc.DocumentName = row.Cells[BrownFileName.Name].Value.ToString();
            //                        object desc = row.Cells[BrownFileDesc.Name].Value;
            //                        doc.DocumentDesc = desc == null ? "" : desc.ToString();

            //                        //doc.FileURL = getFileURL(file);//ʹ��WebService��ʽ���أ��˴������ļ�·��������ļ�����ܱ�Ǩ

            //                        doc.DocumentOwner = ConstObject.LoginPersonInfo;
            //                        doc.DocumentOwnerName = ConstObject.LoginPersonInfo.Name;
            //                        doc.ProObjectGUID = oprNode.Id;
            //                        doc.ProObjectName = oprNode.GetType().Name;
            //                        if (projectInfo != null)
            //                        {
            //                            doc.TheProjectGUID = projectInfo.Id;
            //                            doc.TheProjectName = projectInfo.Name;
            //                        }

            //                        listDoc.Add(doc);
            //                    }

            //                    listDoc = model.SaveOrUpdate(listDoc);

            //                    foreach (ProObjectRelaDocument doc in listDoc)
            //                    {
            //                        InsertIntoGridDocument(doc);
            //                    }
            //                }
            //                #endregion ����MBP��������ĵ���Ϣ
            //            }

            //            gridBrownFileList.Rows.RemoveAt(rowIndex);

            //            if (gridBrownFileList.Rows.Count < 10)
            //                progressBarDocUpload.Value += (int)Math.Floor((decimal)10 / rowCount);
            //            else
            //                progressBarDocUpload.Value += 1;
            //        }

            //        progressBarDocUpload.Value = progressBarDocUpload.Maximum;

            //        MessageBox.Show("����ɹ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex));
            //    }

            //    progressBarDocUpload.Value = 0;
            #endregion
        }
        #endregion



        private void InsertToFileList(string filePath)
        {
            int index = gridBrownFileList.Rows.Add();
            DataGridViewRow row = gridBrownFileList.Rows[index];

            string fileName = Path.GetFileName(filePath);
            row.Cells[BrownFileName.Name].Value = fileName.Substring(0, fileName.IndexOf("."));
            row.Cells[BrownFilePath.Name].Value = filePath;

            FileInfo fileInfo = new FileInfo(filePath);
            row.Cells[BrownFileSize.Name].Value = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetFileAutoSizeString(fileInfo.Length, 3);
        }

        private void InsertIntoGridDocument(ProObjectRelaDocument doc)
        {
            int index = gridDocument.Rows.Add();
            DataGridViewRow row = gridDocument.Rows[index];
            row.Cells[DocumentName.Name].Value = doc.DocumentName;
            row.Cells[UploadPerson.Name].Value = doc.DocumentOwnerName;
            row.Cells[UploadDate.Name].Value = doc.SubmitTime.ToString();
            row.Cells[DocumentDesc.Name].Value = doc.DocumentDesc;

            row.Tag = doc;
        }
        private void UpdateDocument(ProObjectRelaDocument doc)
        {
            foreach (DataGridViewRow row in gridDocument.Rows)
            {
                ProObjectRelaDocument docTemp = row.Tag as ProObjectRelaDocument;
                if (docTemp.Id == doc.Id)
                {
                    row.Cells[DocumentName.Name].Value = doc.DocumentName;
                    row.Cells[DocumentDesc.Name].Value = doc.DocumentDesc;

                    row.Tag = doc;

                    gridDocument.CurrentCell = row.Cells[1];
                    break;
                }
            }
        }

        private string GetExceptionMessage(PLMWebServices.ErrorStack es)
        {
            string msg = es.Message;
            PLMWebServices.ErrorStack esTemp = es.InnerErrorStack;
            while (esTemp != null && !string.IsNullOrEmpty(esTemp.Message))
            {
                msg += "��\n" + esTemp.Message;
                esTemp = esTemp.InnerErrorStack;
            }

            if (msg.IndexOf("�����ھ���Ψһ����") > -1 && msg.IndexOf("�����ظ���") > -1)
            {
                msg = "�Ѵ���ͬ���ĵ��������������ĵ�����.";
            }

            return msg;
        }
        #region ������ʾ**************************************************************************************
        /// <summary>
        /// ������ʾ
        /// </summary>
        /// <param name="es"></param>
        /// <returns></returns>
        private string GetExceptionMessage(PLMWebServicesByKB.ErrorStack es)
        {
            string msg = es.Message;
            PLMWebServicesByKB.ErrorStack esTemp = es.InnerErrorStack;
            while (esTemp != null && !string.IsNullOrEmpty(esTemp.Message))
            {
                msg += "��\n" + esTemp.Message;
                esTemp = esTemp.InnerErrorStack;
            }

            if ((msg.IndexOf("�����ھ���Ψһ����") > -1 && msg.IndexOf("�����ظ���") > -1) || msg.IndexOf("Υ��ΨһԼ��") > -1)
            {
                msg = "�Ѵ���ͬ���ĵ��������������ĵ�����.";
            }

            return msg;
        }
        #endregion

        #endregion �ĵ�����

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
        bool isSelectNodeInvoke = false;//�Ƿ���ѡ��(���)�ڵ�ʱ����
        bool startNodeCheckedState = false;//��shift��ѡ�ֵܽڵ�ʱ��ʼ�ڵ��ѡ��״̬
        #region  ѡ��ڵ㹤�����������ĵ�ģ��************************************************************************************************************
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

                oprNode = currNode.Tag as ProjectTaskTypeTree;
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
                ClearAll(MainViewState.Browser);

                this.txtName.Text = oprNode.Name;
                this.txtCode.Text = oprNode.Code;
                this.txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath;
                this.cbLevel.Text = oprNode.TypeLevel.ToString();
                this.cbTypeStandard.SelectedItem = oprNode.TypeStandard.ToString();
                txtState.Text = GetState(oprNode.State);
                if (!string.IsNullOrEmpty(oprNode.CheckRequire))
                {
                    char[] chs = oprNode.CheckRequire.ToCharArray();
                    for (int i = 0; i < chs.Length; i++)
                    {
                        Char c = chs[i];
                        if (c == '0')
                        {
                            if (cbListCheckRequire.Items.Count > i)
                                cbListCheckRequire.SetItemChecked(i, true);
                        }
                        else
                        {
                            if (cbListCheckRequire.Items.Count > i)
                                cbListCheckRequire.SetItemChecked(i, false);
                        }
                    }
                }

                this.txtSummary.Text = oprNode.TypeSummary;
                this.txtDesc.Text = oprNode.Summary;

                //��ѯ����ĵ�
                txtDocumentName.Text = "";
                txtDocumentDesc.Text = "";
                //txtDocumentPath.Text = "";

                gridDocument.Rows.Clear();
                ObjectQuery oq = new ObjectQuery();
                //oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", oprNode.Id));
                //ProTaskTypeDocumentStencil p = new ProTaskTypeDocumentStencil();
                oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProTaskType.Id", oprNode.Id));
                IList listDocument = model.ObjectQuery(typeof(ProTaskTypeDocumentStencil), oq);
                if (listDocument != null && listDocument.Count > 0)
                {
                    foreach (ProTaskTypeDocumentStencil docStencil in listDocument)
                    {
                        int rowIndex = gridDocument.Rows.Add();
                        gridDocument[DocumentName.Name, rowIndex].Value = docStencil.StencilName;
                        gridDocument[DocumentCode.Name, rowIndex].Value = docStencil.StencilCode;
                        gridDocument[DocumentCheckBatchFlag.Name, rowIndex].Value = docStencil.InspectionMark;
                        gridDocument[DocumentAlterMode.Name, rowIndex].Value = docStencil.AlarmMode;
                        gridDocument[DocumentDesc.Name, rowIndex].Value = docStencil.StencilDescription;
                        gridDocument.Rows[rowIndex].Tag = docStencil;
                        //InsertIntoGridDocument(doc);
                    }
                }
                gridDocument.ClearSelection();

                RefreshControls(MainViewState.Browser);
            }
            catch (Exception exp)
            {
                MessageBox.Show("��ʾ����" + ExceptionUtil.ExceptionMessage(exp));
            }
        }
        #endregion
        private void ClearAll(MainViewState state)
        {
            this.txtCurrentPath.Text = "";
            this.txtCode.Text = "";
            this.txtName.Text = "";
            this.cbTypeStandard.Text = "";
            //cbListCheckRequire.ClearSelected();
            for (int i = 0; i < cbListCheckRequire.Items.Count; i++)
            {
                cbListCheckRequire.SetItemChecked(i, false);
            }
            this.txtSummary.Text = "";
            this.txtDesc.Text = "";

            ClearTaskLevelDropDownList(state);
        }

        private void ClearTaskLevelDropDownList(MainViewState state)
        {
            this.cbLevel.Items.Clear();
            if (state == MainViewState.AddNew)
            {
                if (oprNode != null)
                {
                    List<string> list = GetChildTypeLevel(oprNode.TypeLevel.ToString());
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
                    cbLevel.Items.Add(oprNode.TypeLevel.ToString());
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

        /// <summary>
        /// ���ݸ����ͼ����ȡ�����ͼ��𼯺�
        /// </summary>
        /// <param name="parentStructType"></param>
        /// <returns></returns>
        private List<string> GetChildTypeLevel_Old(string parentTypeLevel)
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

        /// <summary>
        /// ���ݸ����ͼ����ȡ�����ͼ��𼯺�(ʹ�û�����ʩ��
        /// </summary>
        /// <param name="parentStructType"></param>
        /// <returns></returns>
        private List<string> GetChildTypeLevel(string parentTypeLevel)
        {
            List<string> list = new List<string>();
            switch (parentTypeLevel)
            {
                case "��Ŀ":
                    list.Add("רҵ");
                    list.Add("��λ����");
                    list.Add("�ӵ�λ����");
                    list.Add("�ֲ�����");
                    list.Add("�ӷֲ�����");
                    list.Add("�����");
                    break;
                case "רҵ":
                    list.Add("��λ����");
                    list.Add("�ӵ�λ����");
                    list.Add("�ֲ�����");
                    list.Add("�ӷֲ�����");
                    list.Add("�����");
                    break;
                case "��λ����":
                    list.Add("�ӵ�λ����");
                    list.Add("�ֲ�����");
                    list.Add("�ӷֲ�����");
                    list.Add("�����");
                    break;
                case "�ӵ�λ����":
                    list.Add("�ֲ�����");
                    list.Add("�ӷֲ�����");
                    list.Add("�����");
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

        private void UpdateNode()
        {
            try
            {
                //ProjectTaskTypeTree currNode = tnCurrNode.Tag as ProjectTaskTypeTree;

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
                model.DeleteProjectTaskTypeTree(oprNode);

                if (reset)
                {
                    ProjectTaskTypeTree org = model.GetProjectTaskTypeTreeById((tvwCategory.SelectedNode.Parent.Tag as ProjectTaskTypeTree).Id);
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

                ClearAll(MainViewState.AddNew);

                oprNode = new ProjectTaskTypeTree();

                //CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                if (projectInfo != null)
                {
                    oprNode.TheProjectGUID = projectInfo.Id;
                    oprNode.TheProjectName = projectInfo.Name;
                }
                oprNode.ParentNode = this.tvwCategory.SelectedNode.Tag as ProjectTaskTypeTree;
                //oprNode.Code = CreateChildCode(this.tvwCategory.SelectedNode);//oprNode.ParentNode.Code; //model.GetCode(typeof(ProjectTaskTypeTree));
                oprNode.Level = oprNode.ParentNode.Level + 1;
                txtCode.Text = oprNode.Code;
                txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath + "\\";

                for (int i = 0; i < cbListCheckRequire.Items.Count; i++)//Ĭ��ǰ����
                {
                    if (i < 3)
                        cbListCheckRequire.SetItemChecked(i, true);
                }
                gridDocument.Rows.Clear();
                txtName.Focus();
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

        private bool ValideSave()
        {

            try
            {
                if (txtCode.Text.Trim() == "")
                {
                    MessageBox.Show("���벻��Ϊ��!");
                    txtCode.Focus();
                    return false;
                }
                else if (txtCode.Text.Trim().Length > 13)
                {
                    MessageBox.Show("�����ʽ����ȷ�����Ȳ�����13λ!");
                    txtCode.Focus();
                    return false;
                }
                string errorMsg = string.Empty;
                ValideCode(tvwCategory.SelectedNode, txtCode.Text.Trim(), ref errorMsg);
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    MessageBox.Show(errorMsg);
                    txtCode.Focus();
                    return false;
                }

                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("���Ʋ���Ϊ��!");
                    txtName.Focus();
                    return false;
                }
                if (cbLevel.Text == "")
                {
                    MessageBox.Show("������Ϊ��!");
                    cbLevel.Focus();
                    return false;
                }
                if (cbTypeStandard.Text == "")
                {
                    MessageBox.Show("��ѭ��׼����Ϊ��!");
                    cbTypeStandard.Focus();
                    return false;
                }
                //if (cbListCheckRequire.CheckedItems.Count == 0)
                //{
                //    MessageBox.Show("��ѡ����Ҫ��!");
                //    cbListCheckRequire.Focus();
                //    return false;
                //}

                if ((string.IsNullOrEmpty(oprNode.TheProjectGUID) || string.IsNullOrEmpty(oprNode.TheProjectName)) && projectInfo != null)
                {
                    oprNode.TheProjectGUID = projectInfo.Id;
                    oprNode.TheProjectName = projectInfo.Name;
                }

                oprNode.Code = txtCode.Text.Trim();
                oprNode.Name = txtName.Text.Trim();
                oprNode.TypeLevel = VirtualMachine.Component.Util.EnumUtil<ProjectTaskTypeLevel>.FromDescription(cbLevel.SelectedItem.ToString());
                oprNode.TypeStandard = VirtualMachine.Component.Util.EnumUtil<ProjectTaskTypeStandard>.FromDescription(cbTypeStandard.SelectedItem.ToString());
                string checkRequire = string.Empty;
                for (int i = 0; i < cbListCheckRequire.Items.Count; i++)
                {
                    if (cbListCheckRequire.GetItemChecked(i))
                        checkRequire += "0";
                    else
                        checkRequire += "X";
                }
                oprNode.CheckRequire = checkRequire;
                oprNode.TypeSummary = txtSummary.Text.Trim();
                oprNode.Summary = txtDesc.Text.Trim();

                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
                return false;
            }
        }

        private void ValideCode(TreeNode parentNode, string childCode, ref string errorMsg)
        {
            if (parentNode.Parent == null)//���ڵ���벻��У��
                return;

            errorMsg = string.Empty;

            ProjectTaskTypeTree parentType = parentNode.Tag as ProjectTaskTypeTree;
            if (parentType.Code.Length > 13)
            {
                errorMsg = "���ڵ���벻�Ϸ������飡";
                return;
            }
            else if (parentType.Code == childCode && oprNode.Id == null)
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
            else if (level == 4)
            {
                if (childCode.IndexOf(parentCode.Substring(0, 8)) == -1)
                {
                    errorMsg = "���벻�Ϸ������飡";
                    return;
                }
            }
            else if (level == 5) 
            {
                if (childCode.IndexOf(parentCode.Substring(0, 10)) == -1)
                {
                    errorMsg = "���벻�Ϸ������飡";
                    return;
                }
 
            }

            if (IsCodeExists(childCode))
            {
                errorMsg = "�����ظ������飡";
                return;
            }

        }

        private bool IsCodeExists(string code)
        {
            var obj = model.GetTaskTypeByCode(code);

            if (obj != null)
            {
                if (oprNode.Id == null)
                {
                    return true;
                }
                else if ( obj.Id != oprNode.Id)
                {
                    return true;
                }
            }

            return false;
        }

        public string CreateChildCode(TreeNode parentNode)
        {

            string sParentCode = "";
            string sResultCode = "";
            int iStart = 0;
            int iMaxLen = 9;
            int iCode = 0;
            int iLevel = parentNode.FullPath.Split('\\').Count() - 1;
            ProjectTaskTypeTree parentType = parentNode.Tag as ProjectTaskTypeTree;
            sParentCode = parentType.Code;

            if (parentNode.Parent == null)
            {
                iStart = 2;
            }
            else if (iLevel == 1)
            {
                iStart = 4;
            }
            else if (iLevel == 2)
            {
                iStart = 6;
            }
            else
            {
                iStart = 9;
            }
            if (parentNode.Nodes.Count == 0)
            {
                iCode = ClientUtil.ToInt(parentType.Code.Substring(0, iStart)) + 1;
            }
            else
            {
                iCode = parentNode.Nodes.OfType<TreeNode>().Select(a => a.Tag as ProjectTaskTypeTree).Max(a => ClientUtil.ToInt(a.Code.Substring(0, iStart))) + 1;
            }
            sResultCode = ClientUtil.ToString(iCode).PadLeft(iStart, '0').PadRight(iMaxLen, '0');
            return sResultCode;
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
            tvwCategory.DragEnter += new DragEventHandler(tvwCategory_DragEnter);
            tvwCategory.DragDrop += new DragEventHandler(tvwCategory_DragDrop);
            tvwCategory.DragOver += new DragEventHandler(tvwCategory_DragOver);

            LoadProjectTaskTypeTreeTree();
        }

        private void LoadProjectTaskTypeTreeTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();
                IList list = model.GetProjectTaskTypeByInstance();
                //lstInstance = listAll[1] as IList;
                //IList list = listAll[0] as IList;

                list = list.OfType<ProjectTaskTypeTree>().Where(p => p.Level != 9999).ToList();

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
                    if (childNode.TypeStandard == ProjectTaskTypeStandard.���)
                        tnTmp.ForeColor = qibiaoColor;
                    if (childNode.State == (int)DocumentState.Invalid)
                    {
                        tnTmp.ForeColor = InvalidColor;
                    }
                    if (childNode.ParentNode != null)
                    {
                        TreeNode tnp = null;
                        tnp = hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                        if (tnp != null)
                        {
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
                MessageBox.Show("��ѯ�����������ͳ���" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        public override void RefreshControls(MainViewState state)
        {
            bool isUpdate = false;
            bool isDelete = false;
            switch (state)
            {
                case MainViewState.Modify:

                    this.mnuTree.Items["����"].Enabled = true;
                    this.mnuTree.Items["����ڵ�"].Enabled = true;
                    this.mnuTree.Items["�����ӽڵ�"].Enabled = false;
                    this.mnuTree.Items["����ͬ���ڵ�"].Enabled = false;
                    this.mnuTree.Items["�޸Ľڵ�"].Enabled = false;
                    this.mnuTree.Items["ɾ���ڵ�"].Enabled = false;

                    this.linkAdd.Enabled = false;
                    this.linkUpdate.Enabled = false;
                    this.linkDelete.Enabled = false;
                    this.linkCancel.Enabled = true;
                    this.linkSave.Enabled = true;

                    txtCode.ReadOnly = false;
                    txtName.ReadOnly = false;
                    cbLevel.Enabled = true;
                    cbTypeStandard.Enabled = true;
                    cbListCheckRequire.Enabled = true;
                    txtSummary.ReadOnly = false;
                    txtDesc.ReadOnly = false;
                    cmbCheckFlag.Enabled = cmbAlterMode.Enabled = false;
                    break;

                case MainViewState.Browser:
                    isUpdate = (oprNode != null && (oprNode.State == (int)DocumentState.InExecute || oprNode.State == (int)DocumentState.Invalid)) ? false : true;
                    isDelete = (oprNode != null && (oprNode.State == (int)DocumentState.InExecute || oprNode.State == (int)DocumentState.Invalid)) ? false : true;
                    this.linkPublish.Enabled = (oprNode != null && (oprNode.State == (int)DocumentState.Edit || oprNode.State == (int)DocumentState.InAudit || oprNode.State == (int)DocumentState.Valid || oprNode.State == (int)DocumentState.Invalid) ? true : false);
                    this.linkInvalid.Enabled = (oprNode != null && (oprNode.State == (int)DocumentState.InExecute) ? true : false);
                    this.mnuTree.Items["����"].Enabled = false;
                    this.mnuTree.Items["����ڵ�"].Enabled = false;
                    this.mnuTree.Items["�����ӽڵ�"].Enabled = true;
                    this.mnuTree.Items["�޸Ľڵ�"].Enabled = isUpdate;

                    this.linkCancel.Enabled = false;
                    this.linkSave.Enabled = false;
                    this.linkAdd.Enabled = true;
                    this.linkUpdate.Enabled = isUpdate;

                    if (currNode != null && currNode.Parent == null)
                    {
                        this.mnuTree.Items["����ͬ���ڵ�"].Enabled = false;
                        this.mnuTree.Items["ɾ���ڵ�"].Enabled = false;

                        this.linkDelete.Enabled = false;
                    }
                    else
                    {
                        this.mnuTree.Items["����ͬ���ڵ�"].Enabled = false;
                        this.mnuTree.Items["ɾ���ڵ�"].Enabled = isDelete;

                        this.linkDelete.Enabled = isDelete;
                    }

                    txtCode.ReadOnly = true;
                    txtName.ReadOnly = true;
                    cbLevel.Enabled = false;
                    cbTypeStandard.Enabled = false;
                    cbListCheckRequire.Enabled = false;
                    txtSummary.ReadOnly = true;
                    txtDesc.ReadOnly = true;
                    cmbCheckFlag.Enabled = cmbAlterMode.Enabled = false;

                    //�ĵ�����
                    btnUpdateDocument.Enabled = true;
                    btnDeleteDocument.Enabled = true;
                    txtDocumentName.ReadOnly = true;
                    txtDocumentDesc.ReadOnly = true;
                    //txtDocumentPath.ReadOnly = true;
                    btnChangeFile.Enabled = false;
                    btnSaveUpdate.Enabled = false;

                    //btnBrownDocument.Enabled = true;
                    btnRemoveFile.Enabled = true;
                    btnClearAllFile.Enabled = true;
                    btnBatchSave.Enabled = true;
                    break;
            }

            if (listCheckedNode.Count > 0)
            {
                linkCopy.Enabled = true;
                linkDeleteChecked.Enabled = true;
            }
            else
            {
                linkCopy.Enabled = false;
                linkDeleteChecked.Enabled = false;
            }

            if (listCopyNode.Count > 0 && tvwCategory.SelectedNode != null)
                linkPaste.Enabled = true;
            else
                linkPaste.Enabled = false;
        }

        public override bool ModifyView()
        {
            if (�޸Ľڵ�.Enabled)
            {
                mnuTree_ItemClicked(�޸Ľڵ�, new ToolStripItemClickedEventArgs(�޸Ľڵ�));
                return true;
            }

            return false;
        }

        public override bool CancelView()
        {
            try
            {
                if (����.Enabled)
                {
                    mnuTree_ItemClicked(����, new ToolStripItemClickedEventArgs(����));
                    return true;
                }

                return false;
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
                listCheckedNode.Clear();
                listCopyNode.Clear();

                RefreshState(MainViewState.Browser);

                LoadProjectTaskTypeTreeTree();
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
                if (!ValideSave())
                    return false;
                isNew = false;
                if (oprNode.Id == null)
                {
                    isNew = true;

                    //oprNode.Code = model.GetCode(typeof(ProjectTaskTypeTree));

                    if (IsInsertNode)
                    {
                        IList list = new ArrayList();

                        long orderNo = (currNode.Tag as ProjectTaskTypeTree).OrderNo;
                        oprNode.OrderNo = orderNo;

                        list.Add(oprNode);

                        TreeNode parentNode = currNode.Parent;
                        for (int i = currNode.Index; i < parentNode.Nodes.Count; i++)
                        {
                            ProjectTaskTypeTree pbs = parentNode.Nodes[i].Tag as ProjectTaskTypeTree;
                            pbs.OrderNo += 1;
                            list.Add(pbs);
                        }

                        list = model.InsertOrUpdateTaskTypeTrees(list);

                        oprNode = list[0] as ProjectTaskTypeTree;

                        //�����ӽڵ�ĸ��ڵ���Ҫ��������Tag
                        currNode.Parent.Tag = oprNode.ParentNode;


                        //����tag
                        for (int i = currNode.Index; i < parentNode.Nodes.Count; i++)
                        {
                            ProjectTaskTypeTree taskType = parentNode.Nodes[i].Tag as ProjectTaskTypeTree;

                            foreach (ProjectTaskTypeTree ty in list)
                            {
                                if (ty.Id == taskType.Id)
                                {
                                    parentNode.Nodes[i].Tag = ty;
                                    break;
                                }
                            }
                        }

                        TreeNode tn = this.tvwCategory.SelectedNode.Parent.Nodes.Insert(currNode.Index, oprNode.Name.ToString());
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
                        oprNode = model.SaveProjectTaskTypeTree(oprNode);
                    }
                }
                else
                {
                    isNew = false;
                    oprNode = model.SaveProjectTaskTypeTree(oprNode);
                }


                if (isNew)
                {
                    if (!IsInsertNode)
                    {
                        //Ҫ����ӽڵ�Ľڵ���ǰû���ӽڵ㣬��Ҫ��������Tag
                        if (tvwCategory.SelectedNode.Nodes.Count == 0)
                            tvwCategory.SelectedNode.Tag = oprNode.ParentNode;
                        TreeNode tn = this.tvwCategory.SelectedNode.Nodes.Add(oprNode.Id.ToString(), oprNode.Name.ToString());
                        //�����ڵ�Ҫ��Ȩ�޲���
                        //lstInstance.Add(oprNode);
                        tn.Tag = oprNode;
                        this.tvwCategory.SelectedNode = tn;
                        tn.Expand();
                    }
                }
                else
                {
                    this.tvwCategory.SelectedNode.Text = oprNode.Name.ToString();
                    this.tvwCategory.SelectedNode.Tag = oprNode;
                }

                this.RefreshControls(MainViewState.Browser);
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

        #region �ڵ���ק�ƶ�

        private void tvwCategory_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ProjectTaskTypeTree org = (e.Item as TreeNode).Tag as ProjectTaskTypeTree;
                //��Ȩ�޵Ľڵ�������϶�����
                if (org != null)// && ConstMethod.Contains(lstInstance, org)
                {
                    DoDragDrop(e.Item, DragDropEffects.All);
                }
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
                //Ŀ��ڵ�û��Ȩ�޲��������
                if (targetNode == null)//|| !ConstMethod.Contains(lstInstance, targetNode.Tag as ProjectTaskTypeTree)
                    return;
                TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

                if (TreeViewUtil.CanMoveNode(draggedNode, targetNode))
                {
                    //��ǰ�ĸ��ڵ�
                    TreeNode oldParentNode = draggedNode.Parent;

                    #region У�鿽������

                    //У�鲻�ܿ缶�𿽱�
                    int levelValue = (int)(targetNode.Tag as ProjectTaskTypeTree).TypeLevel;
                    ProjectTaskTypeTree copyObj = draggedNode.Tag as ProjectTaskTypeTree;
                    if ((levelValue + 1) != (int)copyObj.TypeLevel)
                    {
                        //����
                        if (draggedNode.Parent == targetNode.Parent)
                        {
                            if (MessageBox.Show("����Ҫִ�����������", "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                draggedNode.Remove();
                                targetNode.Parent.Nodes.Insert(targetNode.Index, draggedNode);

                                if (draggedNode.PrevNode != null)
                                {
                                    IList result = new ArrayList();
                                    ProjectTaskTypeTree prevOrg = draggedNode.PrevNode.Tag as ProjectTaskTypeTree;
                                    SetNextNodeOrder(draggedNode, result, prevOrg.OrderNo + 1);
                                    result = model.SaveProjectTaskTypeTrees(result);
                                    ResetTagAfterOrder(draggedNode, result, 0);
                                }
                                else
                                {
                                    ProjectTaskTypeTree fromOrg = draggedNode.Tag as ProjectTaskTypeTree;
                                    ProjectTaskTypeTree toOrg = targetNode.Tag as ProjectTaskTypeTree;
                                    fromOrg.OrderNo = toOrg.OrderNo - 1;
                                    draggedNode.Tag = model.SaveProjectTaskTypeTree(fromOrg);
                                }
                                //��֤�϶��������޸ı��治����
                                this.tvwCategory.SelectedNode = draggedNode;
                                return;
                            }
                        }

                        MessageBox.Show("Ŀ��ڵ�ļ������ק�Ľڵ㼶�𲻷��ϸ��ӹ�ϵ,����ִ�п������ƶ�������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    #endregion

                    bool reset = false;
                    //���ڵ�ֻ����һ���ӽڵ㣬���Ҹ��ڵ���Ȩ�޲������ƶ���Ҫ�������ø��ڵ�tag
                    if (oldParentNode.Nodes.Count == 1)// && ConstMethod.Contains(lstInstance, oldParentNode.Tag as CategoryNode)
                    {
                        reset = true;
                    }

                    frmTreeMoveCopy frmTmp = new frmTreeMoveCopy();
                    frmTmp.TargetNode = targetNode;
                    frmTmp.DraggedNode = draggedNode;
                    if (draggedNode.Parent == targetNode.Parent)
                        frmTmp.IsOrder = true;
                    frmTmp.ShowDialog();
                    if (frmTmp.IsOK == true)
                    {
                        //�������ڵ�
                        if (frmTmp.MoveOrCopy == enmMoveOrCopy.copy)
                        {
                            draggedNode = frmTmp.DraggedNode;
                            ProjectTaskTypeTree catTmp = (draggedNode.Tag as ProjectTaskTypeTree).Clone();
                            //ϵͳ����һ��Ψһ����
                            if (projectInfo != null)
                            {
                                catTmp.TheProjectGUID = projectInfo.Id;
                                catTmp.TheProjectName = projectInfo.Name;
                            }
                            catTmp.Code = model.GetCode(typeof(ProjectTaskTypeTree));


                            catTmp.ParentNode = targetNode.Tag as ProjectTaskTypeTree;
                            catTmp.OrderNo = model.GetMaxOrderNo(catTmp) + 1;
                            //draggedNode.Tag = catTmp;

                            IList lst = new ArrayList();
                            lst.Add(catTmp);
                            //��¡Ҫ���ƵĽڵ�����ӽڵ�Ķ���
                            PopulateList(draggedNode, lst, catTmp);
                            lst = model.SaveProjectTaskTypeTrees(lst);
                            //�����ڵ�Ҫ��Ȩ�޲���
                            //(lstInstance as ArrayList).AddRange(lst);
                            //�����ƽڵ���¸��ڵ�tag��ֵ
                            targetNode.Tag = (lst[0] as ProjectTaskTypeTree).ParentNode;
                            int i = 0;
                            CopyObjToTag(draggedNode, lst, ref i);

                            //������ƵĽڵ��й�ѡ�ļ��뵽ѡ�м���
                            foreach (TreeNode tn in targetNode.Nodes)
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
                        //�ƶ����ڵ�
                        else if (frmTmp.MoveOrCopy == enmMoveOrCopy.move)
                        {
                            ProjectTaskTypeTree toObj = targetNode.Tag as ProjectTaskTypeTree;
                            IDictionary dic = model.MoveNode(draggedNode.Tag as ProjectTaskTypeTree, toObj);
                            if (reset)
                            {
                                ProjectTaskTypeTree cat = model.GetProjectTaskTypeTreeById((oldParentNode.Tag as ProjectTaskTypeTree).Id);
                                oldParentNode.Tag = cat;
                            }
                            targetNode.Tag = dic[(targetNode.Tag as ProjectTaskTypeTree).Id.ToString()];
                            //���ݷ��ص����ݽ��нڵ�tag��ֵ
                            ResetTagAfterMove(draggedNode, dic);
                        }
                        //����
                        else if (draggedNode.Parent == targetNode.Parent)
                        {
                            if (draggedNode.PrevNode != null)
                            {
                                IList result = new ArrayList();
                                ProjectTaskTypeTree prevOrg = draggedNode.PrevNode.Tag as ProjectTaskTypeTree;
                                SetNextNodeOrder(draggedNode, result, prevOrg.OrderNo + 1);
                                result = model.SaveProjectTaskTypeTrees(result);
                                ResetTagAfterOrder(draggedNode, result, 0);
                            }
                            else
                            {
                                ProjectTaskTypeTree fromOrg = draggedNode.Tag as ProjectTaskTypeTree;
                                ProjectTaskTypeTree toOrg = targetNode.Tag as ProjectTaskTypeTree;
                                fromOrg.OrderNo = toOrg.OrderNo - 1;
                                draggedNode.Tag = model.SaveProjectTaskTypeTree(fromOrg);
                            }
                        }
                        //��֤�϶��������޸ı��治����
                        this.tvwCategory.SelectedNode = draggedNode;
                    }
                }//�û�����ѽڵ��Ƶ��հ�����ѡ�б��϶��ڵ�
                else if (targetNode == null)
                {
                    tvwCategory.SelectedNode = draggedNode;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("�ƶ��������" + ExceptionUtil.ExceptionMessage(ee));
            }
        }
        //���ú����ڵ�������
        private void SetNextNodeOrder(TreeNode node, IList list, long order)
        {
            ProjectTaskTypeTree org = node.Tag as ProjectTaskTypeTree;
            org.OrderNo = order;
            list.Add(org);
            if (node.NextNode != null)
            {
                SetNextNodeOrder(node.NextNode, list, order + 1);
            }
        }
        //������������ýڵ��Tag
        private void ResetTagAfterOrder(TreeNode node, IList lst, int i)
        {
            node.Tag = lst[i];
            if (node.NextNode != null)
                ResetTagAfterOrder(node.NextNode, lst, i + 1);
        }
        //�ƶ����������ýڵ��Tag
        private void ResetTagAfterMove(TreeNode node, IDictionary dic)
        {
            node.Tag = dic[(node.Tag as ProjectTaskTypeTree).Id.ToString()];
            if (node.Nodes.Count == 0)
                return;
            foreach (TreeNode var in node.Nodes)
            {
                ResetTagAfterMove(var, dic);
            }
        }
        //���ƺ��������ýڵ��Tag
        private void CopyObjToTag(TreeNode node, IList lst, ref int i)
        {
            node.Name = (lst[i] as ProjectTaskTypeTree).Id;
            node.Tag = lst[i];

            if (node.Nodes.Count == 0)
                return;
            foreach (TreeNode var in node.Nodes)
            {
                i++;
                CopyObjToTag(var, lst, ref i);
            }
        }

        private void PopulateList(TreeNode node, IList lst, ProjectTaskTypeTree parent)
        {
            if (node.Nodes.Count == 0)
                return;
            foreach (TreeNode var in node.Nodes)
            {
                ProjectTaskTypeTree matCatTmp = new ProjectTaskTypeTree();
                matCatTmp = (var.Tag as ProjectTaskTypeTree).Clone();

                //ϵͳ����һ��Ψһ����
                if (projectInfo != null)
                {
                    matCatTmp.TheProjectGUID = projectInfo.Id;
                    matCatTmp.TheProjectName = projectInfo.Name;
                }
                matCatTmp.Code = model.GetCode(typeof(ProjectTaskTypeTree));

                matCatTmp.ParentNode = parent;
                //var.Tag = matCatTmp;
                lst.Add(matCatTmp);
                PopulateList(var, lst, matCatTmp);
            }
        }

        private void tvwCategory_DragOver(object sender, DragEventArgs e)
        {
            //Point targetPoint = tvwCategory.PointToClient(new Point(e.X, e.Y));
            //tvwCategory.SelectedNode = tvwCategory.GetNodeAt(targetPoint);
        }

        #endregion

        void mnuTree_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var itemName = e.ClickedItem.Name.Trim();
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
                ClearTaskLevelDropDownList(MainViewState.Modify);
                RefreshControls(MainViewState.Modify);
                txtName.Focus();
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
                int levelValue = (int)oprNode.TypeLevel;
                ProjectTaskTypeTree copyObj = listCopyNode[0].Tag as ProjectTaskTypeTree;
                if ((levelValue + 1) != (int)copyObj.TypeLevel)
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
            else if (itemName == ����.Name)
            {
                this.mnuTree.Close();
                Move<ProjectTaskTypeTree>(true);
            }
            else if (itemName == ����.Name)
            {
                this.mnuTree.Close();
                Move<ProjectTaskTypeTree>(false);
            }
        }

        private void InsertBrotherNode()
        {
            try
            {
                IsInsertNode = true;

                ClearAll(MainViewState.Modify);

                oprNode = new ProjectTaskTypeTree();
                oprNode.ParentNode = (currNode.Parent.Tag as ProjectTaskTypeTree);
                //oprNode.Code = CreateChildCode(currNode.Parent); //oprNode.ParentNode.Code;// model.GetCode(typeof(ProjectTaskTypeTree));

                if (projectInfo != null)
                {
                    oprNode.TheProjectGUID = projectInfo.Id;
                    oprNode.TheProjectName = projectInfo.Name;
                }

                txtCode.Text = oprNode.Code;
                txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath + "\\";

                for (int i = 0; i < cbListCheckRequire.Items.Count; i++)
                {
                    if (i < 3)
                        cbListCheckRequire.SetItemChecked(i, true);
                }

                txtName.Focus();
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
                               where (n.Value.Tag as ProjectTaskTypeTree).SysCode.IndexOf((parentNode.Tag as ProjectTaskTypeTree).SysCode) > -1
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

        private void SaveCopyNode()
        {
            if (listCopyNode.Count > 0)
            {
                IList lst = new ArrayList();
                foreach (TreeNode node in listCopyNode)
                {
                    ProjectTaskTypeTree catTmp = (node.Tag as ProjectTaskTypeTree).Clone();

                    if (projectInfo != null)
                    {
                        catTmp.TheProjectGUID = projectInfo.Id;
                        catTmp.TheProjectName = projectInfo.Name;
                    }

                    ProjectTaskTypeTree parentNode = oprNode;// tvwCategory.SelectedNode.Tag as ProjectTaskTypeTree

                    catTmp.ParentNode = parentNode;
                    catTmp.Code = getChildCode(tvwCategory.SelectedNode, (node.Tag as ProjectTaskTypeTree).Code); //model.GetCode(typeof(ProjectTaskTypeTree));
                    catTmp.OrderNo = model.GetMaxOrderNo(parentNode) + 1;

                    lst.Add(catTmp);

                    GetCopyNode(node, catTmp, ref lst);
                }

                //���渴�ƵĽڵ�
                lst = model.SaveProjectTaskTypeTrees(lst);
                //�����ڵ�Ҫ��Ȩ�޲���
                //(lstInstance as ArrayList).AddRange(lst);
                //�����ƽڵ���¸��ڵ�tag��ֵ
                oprNode = (lst[0] as ProjectTaskTypeTree).ParentNode as ProjectTaskTypeTree;
                tvwCategory.SelectedNode.Tag = oprNode;

                IEnumerable<ProjectTaskTypeTree> listCopyPBS = lst.OfType<ProjectTaskTypeTree>();

                IEnumerable<ProjectTaskTypeTree> listCopyRoot = from n in listCopyPBS
                                                                where n.ParentNode.Id == oprNode.Id
                                                                select n;

                foreach (ProjectTaskTypeTree pbs in listCopyRoot)
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

        private string getChildCode(TreeNode parentNode, string childCode)
        {
            ProjectTaskTypeTree taskType = parentNode.Tag as ProjectTaskTypeTree;

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

        private void AddCopyChildNode(TreeNode parentNode, ProjectTaskTypeTree parentPBS, IEnumerable<ProjectTaskTypeTree> listCopyPBS)
        {
            IEnumerable<ProjectTaskTypeTree> listCopyChild = from n in listCopyPBS
                                                             where n.ParentNode.Id == parentPBS.Id
                                                             select n;

            foreach (ProjectTaskTypeTree pbs in listCopyChild)
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
        private void GetCopyNode(TreeNode copyParentNode, ProjectTaskTypeTree saveParentNode, ref IList list)
        {
            foreach (TreeNode node in copyParentNode.Nodes)
            {
                if (listCopyNodeAll.Keys.Contains(node.Name))
                {
                    ProjectTaskTypeTree catTmp = (node.Tag as ProjectTaskTypeTree).Clone();

                    if (projectInfo != null)
                    {
                        catTmp.TheProjectGUID = projectInfo.Id;
                        catTmp.TheProjectName = projectInfo.Name;
                    }

                    catTmp.Code = model.GetCode(typeof(ProjectTaskTypeTree));

                    catTmp.ParentNode = saveParentNode;
                    //catTmp.OrderNo = model.GetMaxOrderNo(parentNode) + 1;//����ž��ø��ƽڵ�������

                    list.Add(catTmp);

                    GetCopyNode(node, catTmp, ref list);
                }
            }
        }

        //private void SaveCopyNode()
        //{
        //    if (listCopyNode.Count > 0)
        //    {
        //        IList lst = new ArrayList();
        //        foreach (TreeNode draggedNode in listCopyNode)
        //        {
        //            ProjectTaskTypeTree catTmp = (draggedNode.Tag as ProjectTaskTypeTree).Clone();

        //            //ϵͳ����һ��Ψһ����
        //            catTmp.Code = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
        //            uniqueCode = catTmp.Code;
        //            catTmp.ParentNode = tvwCategory.SelectedNode.Tag as ProjectTaskTypeTree;
        //            catTmp.OrderNo = model.GetMaxOrderNo(catTmp) + 1;

        //            lst.Add(catTmp);
        //        }
        //        //���渴�ƵĽڵ�
        //        lst = model.SaveProjectTaskTypeTrees(lst);
        //        //�����ڵ�Ҫ��Ȩ�޲���
        //        (lstInstance as ArrayList).AddRange(lst);
        //        //�����ƽڵ���¸��ڵ�tag��ֵ
        //        tvwCategory.SelectedNode.Tag = (lst[0] as ProjectTaskTypeTree).ParentNode;

        //        foreach (ProjectTaskTypeTree pbs in lst)
        //        {
        //            TreeNode tnTmp = new TreeNode();
        //            tnTmp.Name = pbs.Id.ToString();
        //            tnTmp.Text = pbs.Name;
        //            tnTmp.Tag = pbs;

        //            tvwCategory.SelectedNode.Nodes.Add(tnTmp);
        //        }

        //        listCopyNode.Clear();
        //    }
        //}

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
                    list.Add(dic.Value.Tag as ProjectTaskTypeTree);
                }

                string text = "Ҫɾ����ѡ�����нڵ��𣿸ò����������ǵ������ӽڵ�һͬɾ����";
                if (MessageBox.Show(text, "ɾ���ڵ�", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

                if (model.DeleteProjectTaskTypeTree(list))//ɾ���ɹ�
                {
                    //��PBS�����Ƴ���Ӧ�Ľڵ�
                    foreach (ProjectTaskTypeTree pbs in list)
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
        private bool RemoveTreeNode(TreeNode parentNode, ProjectTaskTypeTree pbs)
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

        public void ReloadTreeNode()
        {
            if (isNew)
            {
                //Ҫ����ӽڵ�Ľڵ���ǰû���ӽڵ㣬��Ҫ��������Tag
                if (tvwCategory.SelectedNode.Nodes.Count == 0)
                    tvwCategory.SelectedNode.Tag = oprNode.ParentNode;
                TreeNode tn = this.tvwCategory.SelectedNode.Nodes.Add(oprNode.Id.ToString(), oprNode.Name.ToString());
                //�����ڵ�Ҫ��Ȩ�޲���
                //lstInstance.Add(oprNode);
                tn.Tag = oprNode;
                this.tvwCategory.SelectedNode = tn;
                tn.Expand();
            }
            else
            {
                this.tvwCategory.SelectedNode.Text = oprNode.Name.ToString();
            }
        }

        #region ������ť
        private void linkAdd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RefreshControls(MainViewState.Modify);
            add_Click(null, new EventArgs());
        }

        private void linkUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ClearTaskLevelDropDownList(MainViewState.Modify);
            RefreshControls(MainViewState.Modify);
            txtName.Focus();
        }

        private void linkDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mnuTree.Hide();
            delete_Click(null, new EventArgs());
            RefreshControls(MainViewState.Browser);
            this.Refresh();
        }

        private void linkCancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mnuTree.Hide();
            RefreshControls(MainViewState.Browser);
            this.tvwCategory_AfterSelect(this.tvwCategory, new TreeViewEventArgs(this.tvwCategory.SelectedNode));
        }

        private void linkSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mnuTree.Hide();
            SaveView();
        }
        private void linkPublish_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mnuTree.Hide();
            if (MessageBox.Show("ȷ��Ҫ������ǰ�ڵ㡰" + oprNode.Name + "�������������ӽڵ���", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            IList lst = model.PublisthTaskNodeAndChilds(oprNode.SysCode);
            if (lst != null && lst.Count > 0)
            {
                SetTag(null, lst.OfType<ProjectTaskTypeTree>());
            }
            //tvwCategory.SelectedNode.Tag = oprNode;
            RefreshControls(MainViewState.Browser);
        }
        private void linkInvalid_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mnuTree.Hide();
            if (MessageBox.Show("ȷ��Ҫ���ϵ�ǰ�ڵ㡰" + oprNode.Name + "�������������ӽڵ���", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            IList lst = model.InvalidTaskNodeAndChilds(oprNode.SysCode);
            //tvwCategory.SelectedNode.Tag = oprNode;
            if (lst != null && lst.Count > 0)
            {
                SetTag(null, lst.OfType<ProjectTaskTypeTree>());
            }
            RefreshControls(MainViewState.Browser);
        }
        private void SetTag(TreeNode oNode, IEnumerable<ProjectTaskTypeTree> lst)
        {
            int iState = (int)DocumentState.Invalid;
            if (lst != null && lst.Count() > 0)
            {
                ProjectTaskTypeTree oProjectTaskTypeTree = null;
                if (oNode == null)
                {
                    oNode = tvwCategory.SelectedNode;
                }
                oProjectTaskTypeTree = oNode.Tag as ProjectTaskTypeTree;
                if (oProjectTaskTypeTree != null)
                {
                    oProjectTaskTypeTree = lst.Where(a => a.Id == oProjectTaskTypeTree.Id).First();
                    if (oProjectTaskTypeTree != null)
                    {

                        oNode.Tag = oProjectTaskTypeTree;
                        if (oProjectTaskTypeTree.State == iState)
                        {
                            oNode.ForeColor = this.InvalidColor;
                        }
                        else
                        {
                            oNode.ForeColor = ValidColor;
                        }
                        if (oNode == tvwCategory.SelectedNode)
                        {
                            oprNode = oProjectTaskTypeTree;
                            txtState.Text = GetState(oprNode.State);
                        }
                        if (oNode.Nodes != null && oNode.Nodes.Count > 0)
                        {
                            foreach (TreeNode oChildNode in oNode.Nodes)
                            {
                                SetTag(oChildNode, lst);
                            }
                        }
                    }
                }
            }
        }
        private void linkCopy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mnuTree.Hide();

            listCopyNode.Clear();
            foreach (var dic in listCheckedNode)
            {
                listCopyNode.Add(dic.Value);
            }
            RefreshControls(MainViewState.Check);
        }

        private void linkPaste_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mnuTree.Hide();
            SaveCopyNode();
            RefreshControls(MainViewState.Check);
        }

        private void linkDeleteChecked_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mnuTree.Hide();
            DeleteCheckedNode();
            RefreshControls(MainViewState.Check);
        }
        public string GetState(int iState)
        {
            string sResult = string.Empty;
            if (iState == (int)DocumentState.InExecute)
            {
                sResult = "�ѷ���";
            }
            else if (iState == (int)DocumentState.Invalid)
            {
                sResult = "������";
            }
            else
            {
                sResult = ClientUtil.GetDocStateName((DocumentState)oprNode.State);
            }
            return sResult;
        }
        #endregion

        void SetStyle(TreeNode node)
        {
            ProjectTaskTypeTree tree = node.Tag as ProjectTaskTypeTree;
            if (tree.TypeStandard == ProjectTaskTypeStandard.���)
            {
                node.ForeColor = qibiaoColor;
            }

            if (tree.State == (int)DocumentState.Invalid)
            {
                node.ForeColor = InvalidColor;
            }
        }

        #region ���ƣ�����
        private void Move<T>(bool isMoveUp) where T : CategoryNode
        {
            List<T> lstTreeUpdate = new List<T>();
            T curTree = tvwCategory.SelectedNode.Tag as T;
            TreeNode curNode = tvwCategory.SelectedNode;//��ǰ�ڵ�
            int curr_index = curNode.Index;//��ǰ����λ��
            bool ifCheck = curNode.Checked;
            TreeNode parentNode = curNode.Parent;//���ڵ�
            if (parentNode != null)
            {
                int new_index = 0;//ǰһ����
                //����
                if (isMoveUp)
                {
                    if (curr_index > 0)
                    {
                        new_index = curr_index - 1;
                    }

                    if (curNode.PrevNode != null && curNode.PrevNode.Tag != null)
                    {
                        var preTree = curNode.PrevNode.Tag as T;
                        ////�����ȣ��򽫽���ǰ�ڵ����ż�һ���Լ���һ���ڵ�֮��������ֵܽڵ����ż�һ,����ֱ�ӽ������ߵ���ż���
                        if (preTree.OrderNo == curTree.OrderNo)
                        {
                            curTree.OrderNo = curTree.OrderNo - 1;
                            if (curNode.PrevNode.PrevNode != null && curNode.PrevNode.PrevNode.Tag != null)
                            {
                                SetOrderUp<T>(lstTreeUpdate, curTree.OrderNo, curNode.PrevNode.PrevNode);
                            }
                        }
                        else
                        {
                            var temp = curTree.OrderNo;
                            curTree.OrderNo = preTree.OrderNo;
                            preTree.OrderNo = temp;

                            curNode.PrevNode.Tag = preTree;
                            lstTreeUpdate.Add(preTree);
                            lstTreeUpdate.Add(curTree);
                        }
                    }
                }
                else//����
                {
                    new_index = curr_index + 1;

                    if (curNode.NextNode != null && curNode.NextNode.Tag != null)
                    {
                        var nextTree = curNode.NextNode.Tag as T;
                        //�����ȣ��򽫽���ǰ�ڵ����ż�һ���Լ���һ���ڵ�֮��������ֵܽڵ����ż�һ,����ֱ�ӽ������ߵ���ż���
                        if ((nextTree.OrderNo == curTree.OrderNo))
                        {
                            curTree.OrderNo = curTree.OrderNo + 1;
                            if (curNode.NextNode.NextNode != null && curNode.NextNode.NextNode.Tag != null)
                            {
                                SetOrderDown<T>(lstTreeUpdate, curTree.OrderNo, curNode.NextNode.NextNode);
                            }
                        }
                        else
                        {
                            var temp = curTree.OrderNo;
                            curTree.OrderNo = nextTree.OrderNo;
                            nextTree.OrderNo = temp;

                            curNode.NextNode.Tag = nextTree;
                            lstTreeUpdate.Add(nextTree);
                            lstTreeUpdate.Add(curTree);
                        }
                    }
                }

                //�����½ڵ�
                TreeNode tn_new = new TreeNode();
                tn_new.Name = curTree.Id.ToString();
                tn_new.Text = curTree.Name;
                tn_new.Tag = curTree;
                tn_new.Checked = ifCheck;
                if (curNode.IsExpanded)
                {
                    tn_new.Expand();
                }
                else
                {
                    tn_new.Collapse();
                }
                curNode.Remove();
                CreateNewTreeNodes(tn_new, curNode.Nodes);

                parentNode.Nodes.Insert(new_index, tn_new);//����ڵ�
                tvwCategory.SelectedNode = tn_new;
                SetStyle(tn_new);
                lstTreeUpdate.Add(curTree);
                try
                {
                    model.UpdateProjectTaskTypeTreeOrderNO(lstTreeUpdate as List<ProjectTaskTypeTree>);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("���������쳣��" + ExceptionUtil.ExceptionMessage(ex));
                }
            }
        }

        private void SetOrderUp<T>(List<T> lst, long preOrderNo, TreeNode tn) where T : CategoryNode
        {
            T curTree = tn.Tag as T;
            if (preOrderNo >= curTree.OrderNo)
            {
                curTree.OrderNo = curTree.OrderNo - 1;
                tn.Tag = curTree;
                lst.Add(curTree);
                if (tn.PrevNode != null && tn.PrevNode.Tag != null)
                {
                    TreeNode preNode = tn.PrevNode;
                    SetOrderUp(lst, curTree.OrderNo, preNode);
                }
            }
        }

        private void SetOrderDown<T>(List<T> lst, long preOrderNo, TreeNode tn) where T : CategoryNode
        {
            T curTree = tn.Tag as T;
            if (preOrderNo >= curTree.OrderNo)
            {
                curTree.OrderNo = preOrderNo + 1;
                tn.Tag = curTree;
                lst.Add(curTree);
                if (tn.NextNode != null && tn.NextNode.Tag != null)
                {
                    TreeNode nextNode = tn.NextNode;
                    SetOrderDown(lst, curTree.OrderNo, nextNode);
                }
            }
        }

        private void CreateNewTreeNodes(TreeNode node_new, TreeNodeCollection nodes)
        {
            if (nodes == null || nodes.Count == 0)
            {
                return;
            }
            foreach (TreeNode item in nodes)
            {
                node_new.Nodes.Add(item);
            }
        }
        #endregion
    }
}
