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

        #region 文档操作变量
        /// <summary>
        /// IRP集成类
        /// </summary>
        private IntergrationFrameWork cFuntions = null;

        /// <summary>
        /// 是否初始化本地化数据包
        /// </summary>
        private bool isInitLocalizeBag = false;

        private string language = "zhs";

        private ProObjectRelaDocument oprDocument = null;

        #endregion 文档操作

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
            this.comType.Items.AddRange(new object[] { "技术文档", "竣工文档" });
            this.comDocumentType.Items.AddRange(new object[] { "技术文档", "竣工文档" });
            
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

                        if (root.TypeLevel != ProjectTaskTypeLevel.项目 || root.Name != "建筑项目")//第一次加载  root.Name != projectInfo.Name || 
                        {
                            root.Name = "建筑项目";//固定为建筑项目
                            root.TypeLevel = ProjectTaskTypeLevel.项目;
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
                    root.Name = "建筑项目";//固定为建筑项目
                    root.Code = model.GetCode(typeof(ProjectTaskTypeTree));
                    root.TypeLevel = ProjectTaskTypeLevel.项目;
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
                MessageBox.Show("查询工程任务类型出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// 选择树节点
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
                MessageBox.Show("请选择要下载的文档！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show("下载失败，请检查与服务器连接是否正常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("未将对象引用设置到对象的实例") > -1)
                {
                    MessageBox.Show("下载失败，不存在的文档对象！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("下载失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void btnDelete_Click(object sender, EventArgs e)
        {
            if (gridFileList.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的文档！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gridFileList.Focus();
                return;
            }

            if (MessageBox.Show("确认要删除选择文档吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
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

                //删除IRP文档信息
                ErrorStack es = cFuntions.DeleteEntities("Document".ToUpper(), listDocId.ToArray());
                if (es != null)
                {
                    MessageBox.Show("删除IRP文档时出错，错误信息：" + es.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //删除MBP中对象关联文档信息
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

                //从列表中移除
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
                MessageBox.Show("删除失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        void btnExcel_Click(object sender, EventArgs e) { }

        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                #region 上传文件
                if (oprNode == null)
                {
                    MessageBox.Show("请先选择要关联文档节点！");
                    tvwCategory.Focus();
                    return;
                }

                DataTable sourceDataTable = new DataTable();
                sourceDataTable = PartConfigureLogic.CreatSourceDataTable(false, false, false);


                DataRow newRow = sourceDataTable.NewRow();
                //设置文件类型和文件路径
                newRow["TempFile"] = txtURL.Text.ToString();
                newRow["TempKey"] = txtURL.Text.ToString();
                newRow["FileType"] = "DOCUMENT";
                newRow["FileStructureType"] = "FILESTRUCTURE";
                //newRow["PartToFileType"] = "PARTTOFILELINK";

                //如果有必填字段加上必填字段
                newRow["FILENAME"] = txtDocumentName.Text.ToString();

                //设置文件类型和文件路径
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
                    if (string.IsNullOrEmpty(message) || message.IndexOf("未将对象引用设置到对象的实例") > -1)
                        MessageBox.Show("文件上传服务器失败，请检查IRP客户端是否启动且能正常连接到服务器！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                        MessageBox.Show("文件上传服务器失败，异常消息：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                #endregion 上传文件

                #region 保存MBP对象关联文档信息
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

                #endregion 保存MBP对象关联文档信息

                MessageBox.Show("文件保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        void btnScan_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "所有文件(*.*)|*.*";
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
                //MessageBox.Show("数据映射错误：" + StaticMethod.ExceptionMessage(e));
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

            BatchImportLocalize.Load(language);//初始化本地信息，集成包使用

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
