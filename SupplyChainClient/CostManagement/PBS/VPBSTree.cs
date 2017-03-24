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
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using ImportIntegration;
using System.IO;
using com.think3.PLM.Integration.DataTransfer;
using System.Diagnostics;
using com.think3.PLM.Integration.Client.WS;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using IRPServiceModel.Domain.Document;
using IRPServiceModel.Basic;
using Application.Business.Erp.SupplyChain.Client.FileUpload;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
//using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;



namespace Application.Business.Erp.SupplyChain.Client.CostManagement.PBS
{
    public partial class VPBSTree : TBasicDataView
    {
        /// <summary>
        /// 结构类型
        /// </summary>
        private List<BasicDataOptr> listStructureType;

        private TreeNode currNode;
        private PBSTree oprNode = null;
        private bool isNew = true;
        //有权限的节点
        private IList lstInstance;
        //唯一编码
        private string uniqueCode;

        private Hashtable hashtableRules = new Hashtable();

        private Dictionary<string, TreeNode> listCheckedNode = new Dictionary<string, TreeNode>();

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

        private ProObjectRelaDocument oprDocument = null;

        string fileObjectType = string.Empty;
        string FileStructureType = string.Empty;

        string userName = string.Empty;
        string jobId = string.Empty;

        #endregion 文档操作

        public MPBSTree model;

        public VPBSTree(MPBSTree mot)
        {
            model = mot;
            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();

            cbType.Items.Add("");
            //结构类型
            listStructureType = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetBasicDataByName(VBasicDataOptr.PBS_StructType).OfType<BasicDataOptr>().ToList();
            if (listStructureType != null && listStructureType.Count > 0)
            {
                foreach (BasicDataOptr type in listStructureType)
                {
                    cbType.Items.Add(type);
                }
                cbType.DisplayMember = "BasicName";
                cbType.ValueMember = "Id";

                cbType.SelectedIndex = 0;
            }

            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();

            RefreshState(MainViewState.Browser);

            LoadPBSTreeTreeBySql();

            List<string> listParams = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetUploadFileParamsByMBP_IRP();
            fileObjectType = listParams[0];
            FileStructureType = listParams[1];

            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;

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

            //文档操作
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

            #region 元素
            btnElementAdd.Click += new EventHandler(btnElementAdd_Click);
            btnElementUpdate.Click += new EventHandler(btnElementUpdate_Click);
            btnElementDelete.Click += new EventHandler(btnElementDelete_Click);
            btnElementSave.Click += new EventHandler(btnElementSave_Click);
            btnElementCancel.Click += new EventHandler(btnElementCancel_Click);

            dgElements.SelectionChanged += new EventHandler(dgElements_SelectionChanged);

            btnElementFeature.Click += new EventHandler(btnElementFeature_Click);
            btnUnit.Click += new EventHandler(btnUnit_Click);
            btnSelectRes.Click += new EventHandler(btnSelectRes_Click);
            btnRelation.Click += new EventHandler(btnRelation_Click);
            #endregion
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
        }
        //加载文档数据
        void FillDoc()
        {
            dgDocumentMast.Rows.Clear();
            dgDocumentDetail.Rows.Clear();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", oprNode.Id));
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
            if (oprNode.Id == null)
            {
                if (MessageBox.Show("当前业务对象还没保存，是否保存！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    try
                    {
                        SaveView();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(ex));
                    }
                }
            }
            if (oprNode.Id != null)
            {
                VDocumentPublicUpload frm = new VDocumentPublicUpload(oprNode.Id);
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
                    {
                        dgDocumentMast_CellClick(dgDocumentMast, new DataGridViewCellEventArgs(2, dgDocumentMast.SelectedRows[0].Index));
                    }
                    else
                    {
                        dgDocumentDetail.Rows.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("删除失败！");
                }
            }
        }
        void dgDocumentMast_CellClick(object sender, DataGridViewCellEventArgs e)
        {
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
            //dgDocumentDetail[FileExtension.Name, rowIndex].Value = d.ExtendName;
            //dgDocumentDetail[FilePath.Name, rowIndex].Value = d.FilePartPath;
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

        #region 文档操作（ 已注释）
        ////选择文档
        ////void gridDocument_CellClick(object sender, DataGridViewCellEventArgs e)
        ////{
        ////    if (oprDocument == null && e.ColumnIndex > -1 && e.RowIndex > -1)
        ////    {
        ////        ProObjectRelaDocument doc = gridDocument.SelectedRows[0].Tag as ProObjectRelaDocument;
        ////        if (doc != null)
        ////        {
        ////            txtDocumentName.Text = doc.DocumentName;
        ////            txtDocumentDesc.Text = doc.DocumentDesc;
        ////            txtDocumentPath.Text = doc.FileURL;

        ////            txtDocumentName.ReadOnly = true;
        ////            txtDocumentDesc.ReadOnly = true;
        ////            txtDocumentPath.ReadOnly = true;

        ////            btnChangeFile.Enabled = false;
        ////            btnSaveUpdate.Enabled = false;
        ////        }

        ////        oprDocument = doc;
        ////    }
        ////}
        ////void gridDocument_SelectionChanged(object sender, EventArgs e)
        ////{
        ////    if (gridDocument.SelectedRows.Count == 0)
        ////        return;

        ////    ProObjectRelaDocument doc = gridDocument.SelectedRows[0].Tag as ProObjectRelaDocument;
        ////    if (doc != null)
        ////    {
        ////        txtDocumentName.Text = doc.DocumentName;
        ////        txtDocumentDesc.Text = doc.DocumentDesc;
        ////        txtDocumentPath.Text = "";

        ////        txtDocumentName.ReadOnly = true;
        ////        txtDocumentDesc.ReadOnly = true;
        ////        txtDocumentPath.ReadOnly = true;

        ////        btnChangeFile.Enabled = false;
        ////        btnSaveUpdate.Enabled = false;
        ////    }
        ////    oprDocument = doc;
        ////}

        ////下载文档

        //void btnDownLoadDocument_Click(object sender, EventArgs e)
        //{
        //    if (gridDocument.SelectedRows.Count == 0)
        //    {
        //        MessageBox.Show("请选择要下载的文档！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        gridDocument.Focus();
        //        return;
        //    }
        //    IList relaDocList = new List<ProObjectRelaDocument>();
        //    foreach (DataGridViewRow row in gridDocument.SelectedRows)
        //    {
        //        ProObjectRelaDocument relaDoc = row.Tag as ProObjectRelaDocument;
        //        relaDocList.Add(relaDoc);
        //    }
        //    VDocumentDownloadByID vdd = new VDocumentDownloadByID(relaDocList);
        //    vdd.ShowDialog();

        //    #region 注释
        //    //List<string> listFileIds = new List<string>();
        //    //foreach (DataGridViewRow row in gridDocument.SelectedRows)
        //    //{
        //    //    ProObjectRelaDocument doc = row.Tag as ProObjectRelaDocument;
        //    //    listFileIds.Add(doc.DocumentGUID);
        //    //}

        //    //object[] listFileBytes = null;
        //    //string[] listFileNames = null;

        //    //PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.DownLoadDocumentByCustom(out listFileBytes, out listFileNames, listFileIds.ToArray(),
        //    //    null, userName, jobId, null);

        //    //if (es != null)
        //    //{
        //    //    MessageBox.Show("文件下载失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    //    return;
        //    //}

        //    //if (listFileBytes != null)
        //    //{
        //    //    string selectedDir = string.Empty;
        //    //    for (int i = 0; i < listFileBytes.Length; i++)
        //    //    {
        //    //        byte[] by = listFileBytes[i] as byte[];
        //    //        if (by != null && by.Length > 0)
        //    //        {
        //    //            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

        //    //            saveFileDialog1.Filter = "All files(*.*)|*.*";
        //    //            saveFileDialog1.RestoreDirectory = true;
        //    //            saveFileDialog1.FileName = listFileNames[i];

        //    //            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        //    //            {
        //    //                //进行赋值 
        //    //                string filename = saveFileDialog1.FileName;
        //    //                CreateFileFromByteAarray(by, filename);

        //    //                selectedDir = saveFileDialog1.FileName.Substring(0, saveFileDialog1.FileName.LastIndexOf("\\"));
        //    //            }
        //    //        }
        //    //    }
        //    //    if (Directory.Exists(selectedDir))
        //    //    {
        //    //        Process.Start(selectedDir);
        //    //    }
        //    //}
        //    #endregion
        //}
        //void btnOpenDocument_Click(object sender, EventArgs e)
        //{
        //    if (gridDocument.SelectedRows.Count == 0)
        //    {
        //        MessageBox.Show("请选择要打开的文档！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        gridDocument.Focus();
        //        return;
        //    }

        //    List<PLMWebServices.ProjectDocument> docList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument>();
        //    PLMWebServices.ProjectDocument[] projectDocList = null;

        //    foreach (DataGridViewRow row in gridDocument.SelectedRows)
        //    {
        //        ProObjectRelaDocument relaDoc = row.Tag as ProObjectRelaDocument;
        //        PLMWebServices.ProjectDocument doc = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument();
        //        doc.EntityID = relaDoc.DocumentGUID;
        //        docList.Add(doc);
        //    }


        //    try
        //    {
        //        PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.DownLoadDocumentByIRP(docList.ToArray(), null, userName, jobId, null, out projectDocList);
        //        if (es != null)
        //        {
        //            MessageBox.Show("下载失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //        List<string> errorList = new List<string>();
        //        List<string> listFileFullPaths = new List<string>();
        //        if (projectDocList != null)
        //        {
        //            string fileFullPath = AppDomain.CurrentDomain.BaseDirectory + "TempFilePreview\\" + Guid.NewGuid().ToString().Replace("-", "");
        //            if (!Directory.Exists(fileFullPath))
        //                Directory.CreateDirectory(fileFullPath);

        //            for (int i = 0; i < projectDocList.Length; i++)
        //            {
        //                //byte[] by = listFileBytes[i] as byte[];
        //                //if (by != null && by.Length > 0)
        //                //{
        //                string fileName = projectDocList[i].FileName;

        //                if (projectDocList[i].FileDataByte == null || projectDocList[i].FileDataByte.Length <= 0 || fileName == null)
        //                {
        //                    string strName = projectDocList[i].Code + projectDocList[i].Name;
        //                    errorList.Add(strName);
        //                    continue;
        //                }

        //                string tempFileFullPath = fileFullPath + @"\\" + fileName;

        //                CreateFileFromByteAarray(projectDocList[i].FileDataByte, tempFileFullPath);

        //                listFileFullPaths.Add(tempFileFullPath);
        //                //}
        //            }
        //        }

        //        foreach (string fileFullPath in listFileFullPaths)
        //        {
        //            FileInfo file = new FileInfo(fileFullPath);

        //            //定义一个ProcessStartInfo实例
        //            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
        //            //设置启动进程的初始目录
        //            info.WorkingDirectory = file.DirectoryName;//Application.StartupPath; 
        //            //设置启动进程的应用程序或文档名
        //            info.FileName = file.Name;
        //            //设置启动进程的参数
        //            info.Arguments = "";
        //            //启动由包含进程启动信息的进程资源
        //            try
        //            {
        //                System.Diagnostics.Process.Start(info);
        //            }
        //            catch (System.ComponentModel.Win32Exception we)
        //            {
        //                MessageBox.Show(this, we.Message);
        //            }
        //        }
        //        if (errorList != null && errorList.Count > 0)
        //        {
        //            string str = "";
        //            foreach (string s in errorList)
        //            {
        //                str += (s + ";");
        //            }
        //            MessageBox.Show(str + "这" + errorList.Count + "个文件，无法预览，文件不存在或未指定格式！");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.IndexOf("未将对象引用设置到对象的实例") > -1)
        //        {
        //            MessageBox.Show("操作失败，不存在的文档对象！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        }
        //        else
        //            MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }

        //    #region 注释
        //    //       List<string> listFileIds = new List<string>();
        //    //       foreach (DataGridViewRow row in gridDocument.SelectedRows)
        //    //       {
        //    //           ProObjectRelaDocument doc = row.Tag as ProObjectRelaDocument;
        //    //           listFileIds.Add(doc.DocumentGUID);
        //    //       }

        //    //       try
        //    //       {

        //    //           object[] listFileBytes = null;
        //    //           string[] listFileNames = null;

        //    //           PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.DownLoadDocumentByCustom(out listFileBytes, out listFileNames, listFileIds.ToArray(),
        //    //null, userName, jobId, null);

        //    //           if (es != null)
        //    //           {
        //    //               MessageBox.Show("操作失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    //               return;
        //    //           }

        //    //           List<string> listFileFullPaths = new List<string>();
        //    //           if (listFileBytes != null)
        //    //           {
        //    //               string fileFullPath = AppDomain.CurrentDomain.BaseDirectory + "TempFilePreview\\" + Guid.NewGuid().ToString().Replace("-", "");
        //    //               if (!Directory.Exists(fileFullPath))
        //    //                   Directory.CreateDirectory(fileFullPath);

        //    //               for (int i = 0; i < listFileBytes.Length; i++)
        //    //               {
        //    //                   byte[] by = listFileBytes[i] as byte[];
        //    //                   if (by != null && by.Length > 0)
        //    //                   {
        //    //                       string fileName = listFileNames[i];
        //    //                       string tempFileFullPath = fileFullPath + @"\\" + fileName;

        //    //                       CreateFileFromByteAarray(by, tempFileFullPath);

        //    //                       listFileFullPaths.Add(tempFileFullPath);
        //    //                   }
        //    //               }
        //    //           }

        //    //           foreach (string fileFullPath in listFileFullPaths)
        //    //           {
        //    //               FileInfo file = new FileInfo(fileFullPath);

        //    //               //定义一个ProcessStartInfo实例
        //    //               System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
        //    //               //设置启动进程的初始目录
        //    //               info.WorkingDirectory = file.DirectoryName;//Application.StartupPath; 
        //    //               //设置启动进程的应用程序或文档名
        //    //               info.FileName = file.Name;
        //    //               //设置启动进程的参数
        //    //               info.Arguments = "";
        //    //               //启动由包含进程启动信息的进程资源
        //    //               try
        //    //               {
        //    //                   System.Diagnostics.Process.Start(info);
        //    //               }
        //    //               catch (System.ComponentModel.Win32Exception we)
        //    //               {
        //    //                   MessageBox.Show(this, we.Message);
        //    //               }
        //    //           }
        //    //       }
        //    //       catch (Exception ex)
        //    //       {
        //    //           if (ex.Message.IndexOf("未将对象引用设置到对象的实例") > -1)
        //    //           {
        //    //               MessageBox.Show("操作失败，不存在的文档对象！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    //           }
        //    //           else
        //    //               MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    //       }
        //    #endregion

        //}
        //public static void CreateFileFromByteAarray(byte[] stream, string fileName)
        //{
        //    try
        //    {
        //        FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
        //        fs.Write(stream, 0, stream.Length);
        //        fs.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        ////修改文档
        //void btnUpdateDocument_Click(object sender, EventArgs e)
        //{
        //    if (gridDocument.SelectedRows.Count == 0)
        //    {
        //        MessageBox.Show("请选择要修改的文档！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        gridDocument.Focus();
        //        return;
        //    }
        //    IList relaDocList = new List<ProObjectRelaDocument>();
        //    foreach (DataGridViewRow row in gridDocument.SelectedRows)
        //    {
        //        ProObjectRelaDocument relaDoc = row.Tag as ProObjectRelaDocument;
        //        relaDocList.Add(relaDoc);
        //    }
        //    VDocumentListUpdate vdlu = new VDocumentListUpdate(projectInfo, relaDocList);
        //    vdlu.ShowDialog();
        //    IList resultRelaDocList = vdlu.ResultListDoc;
        //    if (resultRelaDocList == null) return;
        //    foreach (DataGridViewRow row in gridDocument.SelectedRows)
        //    {
        //        gridDocument.Rows.RemoveAt(row.Index);
        //    }

        //    foreach (ProObjectRelaDocument doc in resultRelaDocList)
        //    {
        //        int rowIndex = gridDocument.Rows.Add();
        //        gridDocument[DocumentName.Name, rowIndex].Value = doc.DocumentName;
        //        gridDocument[DocumentCateCode.Name, rowIndex].Value = doc.DocumentCateCode;
        //        gridDocument[DocumentCateName.Name, rowIndex].Value = doc.DocumentCateName;
        //        gridDocument[DocumentCode.Name, rowIndex].Value = doc.DocumentCode;
        //        gridDocument[DocumentDesc.Name, rowIndex].Value = doc.DocumentDesc;
        //        gridDocument.Rows[rowIndex].Tag = doc;
        //    }

        //    //else if (gridDocument.SelectedRows.Count > 1)
        //    //{
        //    //    MessageBox.Show("一次只能修改一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    //    gridDocument.Focus();
        //    //    return;
        //    //}
        //    //oprDocument = gridDocument.SelectedRows[0].Tag as ProObjectRelaDocument;

        //    //txtDocumentName.ReadOnly = false;
        //    //txtDocumentDesc.ReadOnly = false;

        //    //btnChangeFile.Enabled = true;
        //    //btnSaveUpdate.Enabled = true;

        //    //txtDocumentName.Focus();
        //}
        //#region 注释
        //////更换文件
        ////void btnChangeFile_Click(object sender, EventArgs e)
        ////{
        ////    OpenFileDialog openFileDialog1 = new OpenFileDialog();
        ////    openFileDialog1.Filter = "所有文件(*.*)|*.*";
        ////    openFileDialog1.Multiselect = false;

        ////    if (openFileDialog1.ShowDialog() == DialogResult.OK)
        ////    {
        ////        txtDocumentPath.Text = openFileDialog1.FileNames[0];
        ////    }
        ////}
        //////保存修改
        ////void btnSaveDocument_Click(object sender, EventArgs e)
        ////{
        ////    try
        ////    {
        ////        if (!txtDocumentName.ReadOnly && oprDocument != null)
        ////        {
        ////            if (txtDocumentName.Text.Trim() == "")
        ////            {
        ////                MessageBox.Show("文档名称不能为空！");
        ////                txtDocumentName.Focus();
        ////                return;
        ////            }
        ////            List<string> listFileIds = new List<string>();
        ////            listFileIds.Add(oprDocument.DocumentGUID);

        ////            ObjectQuery oq = new ObjectQuery();
        ////            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("Id", oprDocument.Id));
        ////            oprDocument = model.ObjectQuery(typeof(ProObjectRelaDocument), oq)[0] as ProObjectRelaDocument;

        ////            oprDocument.DocumentName = txtDocumentName.Text.Trim();
        ////            oprDocument.DocumentDesc = txtDocumentDesc.Text.Trim();

        ////            if (txtDocumentPath.Text != "" && File.Exists(txtDocumentPath.Text))//更换了文件
        ////            {
        ////                FileInfo file = new FileInfo(txtDocumentPath.Text);

        ////                List<byte[]> listFileBytes = new List<byte[]>();
        ////                List<string> listFileNames = new List<string>();
        ////                List<PLMWebServices.DictionaryObjectInfo> listDicKeyValue = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.DictionaryObjectInfo>();

        ////                FileStream fileStream = file.OpenRead();
        ////                int FileLen = (int)file.Length;
        ////                Byte[] FileData = new Byte[FileLen];
        ////                //将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
        ////                fileStream.Read(FileData, 0, FileLen);

        ////                listFileBytes.Add(FileData);


        ////                string fileName = txtDocumentName.Text.Trim();
        ////                string fileDesc = txtDocumentDesc.Text.Trim();


        ////                listFileNames.Add(fileName + Path.GetExtension(file.Name));


        ////                List<string> listNames = new List<string>();
        ////                List<object> listValues = new List<object>();

        ////                listNames.Add("Name");
        ////                listValues.Add(fileName);

        ////                listNames.Add("DOCUMENTTITLE");
        ////                listValues.Add(fileName);

        ////                listNames.Add("DOCUMENTDESCRIPTION");
        ////                listValues.Add(fileDesc);

        ////                PLMWebServices.DictionaryObjectInfo dic = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.DictionaryObjectInfo();
        ////                dic.InfoNames = listNames.ToArray();
        ////                dic.InfoValues = listValues.ToArray();

        ////                listDicKeyValue.Add(dic);


        ////                PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.UpdateDocumentByCustom(listFileIds.ToArray(),
        ////                     fileObjectType, listFileBytes.ToArray(), listFileNames.ToArray(), "1", listDicKeyValue.ToArray(), null, userName, jobId, null);

        ////                if (es != null)
        ////                {
        ////                    MessageBox.Show("修改文件失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        ////                    return;
        ////                }
        ////            }

        ////            oprDocument = model.SaveOrUpdateProObjRelaDoc(oprDocument);

        ////            UpdateDocument(oprDocument);

        ////            txtDocumentName.ReadOnly = true;
        ////            txtDocumentDesc.ReadOnly = true;

        ////            btnChangeFile.Enabled = false;
        ////            btnSaveUpdate.Enabled = false;

        ////            MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        MessageBox.Show("保存失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
        ////    }
        ////}
        //#endregion
        ////删除文档
        //void btnDeleteDocument_Click(object sender, EventArgs e)
        //{
        //    if (gridDocument.SelectedRows.Count == 0)
        //    {
        //        MessageBox.Show("请选择要删除的文档！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        gridDocument.Focus();
        //        return;
        //    }

        //    if (MessageBox.Show("确认要删除选择文档吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        //        return;

        //    try
        //    {
        //        IList relaDocList = new List<ProObjectRelaDocument>();
        //        List<string> docIds = new List<string>();
        //        List<PLMWebServices.ProjectDocument> proDocList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument>();
        //        PLMWebServices.ProjectDocument[] reultProdocList = null;
        //        PLMWebServices.ProjectDocument[] resultProDoc = null;

        //        foreach (DataGridViewRow row in gridDocument.SelectedRows)
        //        {
        //            ProObjectRelaDocument relaDoc = row.Tag as ProObjectRelaDocument;
        //            relaDocList.Add(relaDoc);
        //            docIds.Add(relaDoc.DocumentGUID);
        //        }

        //        PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectDocumentByIRP(docIds.ToArray(), Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentQueryVersion.所有版本,
        //            null, userName, jobId, null, out resultProDoc);
        //        if (es != null)
        //        {
        //            MessageBox.Show("操作失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }

        //        for (int i = 0; i < resultProDoc.Length; i++)
        //        {
        //            PLMWebServices.ProjectDocument doc = resultProDoc[i];
        //            doc.State = Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentState.作废;
        //            proDocList.Add(doc);
        //        }

        //        PLMWebServices.ErrorStack es1 = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.UpdateDocumentByIRP(proDocList.ToArray(), Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentUpdateMode.添加一个新版次文件,
        //            null, userName, jobId, null, out reultProdocList);
        //        if (es1 != null)
        //        {
        //            MessageBox.Show("操作失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //        bool flag = model.DeleteProObjRelaDoc(relaDocList);
        //        if (flag)
        //        {
        //            MessageBox.Show("删除成功！");
        //            foreach (DataGridViewRow row in gridDocument.SelectedRows)
        //            {
        //                gridDocument.Rows.RemoveAt(row.Index);
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("删除失败！");
        //        }
        //        #region 注释
        //        //List<ProObjectRelaDocument> listDoc = new List<ProObjectRelaDocument>();
        //        //List<string> listDocId = new List<string>();
        //        //foreach (DataGridViewRow row in gridDocument.SelectedRows)
        //        //{
        //        //    ProObjectRelaDocument doc = row.Tag as ProObjectRelaDocument;
        //        //    listDoc.Add(doc);
        //        //    listDocId.Add(doc.DocumentGUID);
        //        //}

        //        ////删除IRP文档信息
        //        //PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.DeleteDocumentByCustom(listDocId.ToArray()
        //        //    , "1", null, userName, jobId, null);
        //        //if (es != null)
        //        //{

        //        //    MessageBox.Show("删除IRP文档时出错，错误信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        //    return;
        //        //}

        //        ////删除MBP中对象关联文档信息
        //        //ObjectQuery oq = new ObjectQuery();
        //        //NHibernate.Criterion.Disjunction dis = new NHibernate.Criterion.Disjunction();
        //        //foreach (ProObjectRelaDocument doc in listDoc)
        //        //{
        //        //    dis.Add(NHibernate.Criterion.Expression.And(
        //        //        NHibernate.Criterion.Expression.Eq("ProObjectGUID", doc.ProObjectGUID),
        //        //        NHibernate.Criterion.Expression.Eq("DocumentGUID", doc.DocumentGUID)));
        //        //}
        //        //oq.AddCriterion(dis);

        //        //IList list = model.ObjectQuery(typeof(ProObjectRelaDocument), oq);

        //        //model.DeleteProObjRelaDoc(list);

        //        ////从列表中移除
        //        //List<int> listRowIndex = new List<int>();
        //        //foreach (DataGridViewRow row in gridDocument.SelectedRows)
        //        //{
        //        //    listRowIndex.Add(row.Index);
        //        //}
        //        //listRowIndex.Sort();
        //        //for (int i = listRowIndex.Count - 1; i > -1; i--)
        //        //{
        //        //    gridDocument.Rows.RemoveAt(listRowIndex[i]);
        //        //}

        //        //txtDocumentName.Text = "";
        //        //txtDocumentDesc.Text = "";
        //        //txtDocumentPath.Text = "";
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("删除失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //#region 注释
        //////浏览选择文档
        ////void btnBrownDocument_Click(object sender, EventArgs e)
        ////{
        ////    OpenFileDialog openFileDialog1 = new OpenFileDialog();
        ////    openFileDialog1.Filter = "所有文件(*.*)|*.*";
        ////    openFileDialog1.Multiselect = true;

        ////    if (openFileDialog1.ShowDialog() == DialogResult.OK)
        ////    {
        ////        string[] strFiles = openFileDialog1.FileNames;
        ////        int iCount = strFiles.Length;
        ////        for (int i = 0; i < iCount; i++)
        ////        {
        ////            InsertToFileList(strFiles[i]);
        ////        }
        ////        gridBrownFileList.AutoResizeColumns();
        ////    }
        ////}
        //////移除选择文件
        ////void btnRemoveFile_Click(object sender, EventArgs e)
        ////{
        ////    List<int> listRowIndex = new List<int>();
        ////    foreach (DataGridViewRow row in gridBrownFileList.SelectedRows)
        ////    {
        ////        listRowIndex.Add(row.Index);
        ////    }
        ////    listRowIndex.Sort();
        ////    for (int i = listRowIndex.Count - 1; i > -1; i--)
        ////    {
        ////        gridBrownFileList.Rows.RemoveAt(listRowIndex[i]);
        ////    }
        ////}
        //////全部清除
        ////void btnClearAllFile_Click(object sender, EventArgs e)
        ////{
        ////    gridBrownFileList.Rows.Clear();
        ////}
        //#endregion
        ////批量保存
        ////void btnBatchSave_Click(object sender, EventArgs e)
        ////{
        ////    try
        ////    {

        ////        if (gridBrownFileList.Rows.Count == 0)
        ////        {
        ////            MessageBox.Show("请先选择要保存的文件！");
        ////            btnBrownDocument.Focus();
        ////            return;
        ////        }
        ////        else if (oprNode == null)
        ////        {
        ////            MessageBox.Show("请先选择要关联文档任务节点！");
        ////            tvwCategory.Focus();
        ////            return;
        ////        }

        ////        //校验数据
        ////        foreach (DataGridViewRow row in gridBrownFileList.Rows)
        ////        {
        ////            if (row.Cells[BrownFileName.Name].Value == null)
        ////            {
        ////                MessageBox.Show("文件名不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        ////                gridBrownFileList.CurrentCell = row.Cells[BrownFileName.Name];
        ////                gridBrownFileList.BeginEdit(false);
        ////                return;
        ////            }
        ////        }

        ////        int rowCount = gridBrownFileList.Rows.Count;

        ////        progressBarDocUpload.Minimum = 0;
        ////        progressBarDocUpload.Maximum = rowCount >= 10 ? rowCount + 1 : 11;
        ////        progressBarDocUpload.Value = 1;

        ////        //显示进度条，使用单个上传模式
        ////        //List<byte[]> listFileBytes = new List<byte[]>();
        ////        //List<string> listFileNames = new List<string>();
        ////        //List<PLMWebServices.DictionaryObjectInfo> listDicKeyValue = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.DictionaryObjectInfo>();

        ////        IList listDoc = new List<ProObjectRelaDocument>();
        ////        IList addResultDocList = new List<ProObjectRelaDocument>();
        ////        for (int rowIndex = gridBrownFileList.Rows.Count - 1; rowIndex > -1; rowIndex--)
        ////        {
        ////            #region
        ////            DataGridViewRow row = gridBrownFileList.Rows[rowIndex];

        ////            //listFileBytes.Clear();
        ////            //listFileNames.Clear();
        ////            //listDicKeyValue.Clear();
        ////            List<PLMWebServices.ProjectDocument> docList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument>();
        ////            PLMWebServices.ProjectDocument[] resultList = null;
        ////            PLMWebServices.ProjectDocument doc = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument();
        ////            string filePath = row.Cells[BrownFilePath.Name].Value.ToString();

        ////            FileInfo file = new FileInfo(filePath);
        ////            if (file.Exists)
        ////            {
        ////                #region 上传文件
        ////                FileStream fileStream = file.OpenRead();
        ////                int FileLen = (int)file.Length;
        ////                Byte[] FileData = new Byte[FileLen];
        ////                ////将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
        ////                fileStream.Read(FileData, 0, FileLen);
        ////                if (FileData.Length == 0)
        ////                {
        ////                    MessageBox.Show("该文件长度为0,请检查!");
        ////                    return;
        ////                }
        ////                doc.ExtendName = Path.GetExtension(filePath); //文档扩展名*******************************
        ////                doc.FileDataByte = FileData; //文件二进制流
        ////                doc.FileName = file.Name;//文件名称

        ////                doc.ProjectCode = projectInfo.Code; //所属项目代码*
        ////                doc.ProjectName = projectInfo.Name; //所属项目名称*

        ////                doc.Author = row.Cells[BrownFileAuthor.Name].Value.ToString();//文档作者*

        ////                PLMWebServicesByKB.CategoryNode cateNode = row.Cells[BrownFileCateName.Name].Tag as PLMWebServicesByKB.CategoryNode;
        ////                doc.Category = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.CategoryNode();
        ////                doc.Category.CategoryCode = cateNode.CategoryCode;//"CSFL";//文档分类代码
        ////                doc.Category.CategoryName = row.Cells[BrownFileCateName.Name].Value.ToString();//"测试分类"; //文档分类名称

        ////                List<string> listDocParam = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetUploadFileParamsByMBP_IRP();
        ////                string docObjTypeName = listDocParam[0];
        ////                string docCateLinkTypeName = listDocParam[2];

        ////                doc.CategoryRelaDocType = docCateLinkTypeName;//文档分类类型
        ////                doc.ObjectTypeName = docObjTypeName;//文档对象类型
        ////                //doc.CategorySysCode = "";//文档分类层次码
        ////                doc.Code = row.Cells[BrownFileCode.Name].Value.ToString();//文档代码
        ////                doc.Description = row.Cells[BrownFileDesc.Name].Value.ToString();//文档说明

        ////                PLMWebServices.DocumentInfoType docInfoType = 0;
        ////                foreach (PLMWebServices.DocumentInfoType type in Enum.GetValues(typeof(PLMWebServices.DocumentInfoType)))
        ////                {
        ////                    if (type.ToString() == row.Cells[BrownFileInforType.Name].Value.ToString())
        ////                    {
        ////                        docInfoType = type;
        ////                        break;
        ////                    }
        ////                }

        ////                PLMWebServices.DocumentState docState = 0;
        ////                foreach (PLMWebServices.DocumentState state in Enum.GetValues(typeof(PLMWebServices.DocumentState)))
        ////                {
        ////                    if (state.ToString() == row.Cells[BrownFileState.Name].Value.ToString())
        ////                    {
        ////                        docState = state;
        ////                        break;
        ////                    }
        ////                }
        ////                doc.DocType = docInfoType;//文档信息类型
        ////                doc.State = docState;//文档状态
        ////                doc.KeyWords = row.Cells[BrownFileKeyWord.Name].Value.ToString();//文档关键字
        ////                doc.Name = row.Cells[BrownFileName.Name].Value.ToString();//文档名称
        ////                doc.Title = row.Cells[BrownFileName.Name].Value.ToString();//文档标题

        ////                //string docObjTypeName= StaticMethod.GetMBPUploadFileParams()[0]; 
        ////                //doc.OwnerID = "";//责任人
        ////                //doc.OwnerName = "";//责任人名称
        ////                //doc.OwnerOrgSysCode = "";// 责任人组织层次码
        ////                //doc.Revision = "";//文档版次 
        ////                //doc.Version = "";//文档版本
        ////                //doc.ExtendInfoNames = "";//扩展属性名
        ////                //doc.ExtendInfoValues = "";//扩展属性值
        ////                docList.Add(doc);

        ////                PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.AddDocumentByIRP(docList.ToArray(), Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentSaveMode.一个文件生成一个文档对象,
        ////                    null, userName, jobId, null, out resultList);
        ////                if (es != null)
        ////                {
        ////                    MessageBox.Show("文件“" + row.Cells[BrownFileName.Name].Value.ToString() + "”上传到服务器失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        ////                    return;
        ////                }

        ////                #region 上传    注释
        ////                //FileStream fileStream = file.OpenRead();
        ////                //int FileLen = (int)file.Length;
        ////                //Byte[] FileData = new Byte[FileLen];
        ////                ////将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
        ////                //fileStream.Read(FileData, 0, FileLen);

        ////                //listFileBytes.Add(FileData);


        ////                //string fileName = row.Cells[BrownFileName.Name].Value.ToString();
        ////                //listFileNames.Add(fileName + Path.GetExtension(file.Name));

        ////                //object fileDesc = row.Cells[BrownFileDesc.Name].Value;

        ////                //List<string> listNames = new List<string>();
        ////                //List<object> listValues = new List<object>();

        ////                ////listNames.Add("Code");
        ////                ////listValues.Add("");

        ////                ////listNames.Add("DOCUMENTNUMBER");
        ////                ////listValues.Add("");

        ////                //listNames.Add("Name");
        ////                //listValues.Add(fileName);

        ////                //listNames.Add("DOCUMENTTITLE");
        ////                //listValues.Add(fileName);

        ////                //listNames.Add("DOCUMENTDESCRIPTION");
        ////                //listValues.Add(fileDesc);

        ////                //PLMWebServices.DictionaryObjectInfo dic = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.DictionaryObjectInfo();
        ////                //dic.InfoNames = listNames.ToArray();
        ////                //dic.InfoValues = listValues.ToArray();

        ////                //listDicKeyValue.Add(dic);


        ////                //string[] listFileIds = null;

        ////                //PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.AddDocumentByCustom
        ////                //    (out listFileIds, listFileBytes.ToArray(),
        ////                //    listFileNames.ToArray(), fileObjectType, "1", listDicKeyValue.ToArray(), null, userName, jobId, null);
        ////                #endregion

        ////                #endregion 上传文件

        ////                #region 保存MBP对象关联文档信息
        ////                if (resultList != null)
        ////                {
        ////                    listDoc.Clear();
        ////                    string fileId = resultList[0].ID;
        ////                    ProObjectRelaDocument rdoc = new ProObjectRelaDocument();

        ////                    rdoc.TheProjectGUID = projectInfo.Id;
        ////                    rdoc.TheProjectName = projectInfo.Name;
        ////                    rdoc.TheProjectCode = projectInfo.Code;

        ////                    rdoc.ProObjectName = oprNode.Name;
        ////                    rdoc.ProObjectGUID = oprNode.Id;

        ////                    rdoc.DocumentOwner = ConstObject.LoginPersonInfo;
        ////                    rdoc.DocumentOwnerName = ConstObject.LoginPersonInfo.Name;
        ////                    rdoc.DocumentGUID = resultList[0].EntityID;
        ////                    rdoc.DocumentName = resultList[0].FileName;
        ////                    rdoc.DocumentDesc = resultList[0].Description;
        ////                    rdoc.SubmitTime = resultList[0].UpdateTime;
        ////                    if (resultList[0].Category != null)
        ////                        rdoc.DocumentCateCode = resultList[0].Category.CategoryCode;
        ////                    rdoc.FileURL = filePath;

        ////                    listDoc.Add(rdoc);
        ////                    listDoc = model.SaveOrUpdate(listDoc);
        ////                }
        ////                #region 保存MBP对象关联文档信息 （注释）
        ////                //if (listFileIds != null)
        ////                //{
        ////                //    listDoc.Clear();

        ////                //    for (int i = 0; i < listFileIds.Length; i++)
        ////                //    {
        ////                //        string fileId = listFileIds[i];
        ////                //        ProObjectRelaDocument doc = new ProObjectRelaDocument();
        ////                //        doc.DocumentGUID = fileId;
        ////                //        doc.DocumentName = row.Cells[BrownFileName.Name].Value.ToString();
        ////                //        object desc = row.Cells[BrownFileDesc.Name].Value;
        ////                //        doc.DocumentDesc = desc == null ? "" : desc.ToString();

        ////                //        //doc.FileURL = getFileURL(file);//使用WebService方式下载，此处不存文件路径，其次文件柜可能变迁

        ////                //        doc.DocumentOwner = ConstObject.LoginPersonInfo;
        ////                //        doc.DocumentOwnerName = ConstObject.LoginPersonInfo.Name;
        ////                //        doc.ProObjectGUID = oprNode.Id;
        ////                //        doc.ProObjectName = oprNode.GetType().Name;
        ////                //        if (projectInfo != null)
        ////                //        {
        ////                //            doc.TheProjectGUID = projectInfo.Id;
        ////                //            doc.TheProjectName = projectInfo.Name;
        ////                //        }

        ////                //        listDoc.Add(doc);
        ////                //    }

        ////                //    listDoc = model.SaveOrUpdate(listDoc);

        ////                //    foreach (ProObjectRelaDocument doc in listDoc)
        ////                //    {
        ////                //       InsertIntoGridDocument(doc);
        ////                //    }
        ////                //}
        ////                #endregion
        ////                #endregion 保存MBP对象关联文档信息
        ////            }
        ////            #endregion
        ////            addResultDocList = model.SaveOrUpdate(listDoc);

        ////            gridBrownFileList.Rows.RemoveAt(rowIndex);

        ////            if (gridBrownFileList.Rows.Count < 10)
        ////                progressBarDocUpload.Value += (int)Math.Floor((decimal)10 / rowCount);
        ////            else
        ////                progressBarDocUpload.Value += 1;
        ////        }

        ////        progressBarDocUpload.Value = progressBarDocUpload.Maximum;

        ////        MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
        ////    }

        ////    progressBarDocUpload.Value = 0;
        ////}

        ////private void InsertToFileList(string filePath)
        ////{
        ////    int index = gridBrownFileList.Rows.Add();
        ////    DataGridViewRow row = gridBrownFileList.Rows[index];

        ////    string fileName = Path.GetFileName(filePath);
        ////    row.Cells[BrownFileName.Name].Value = fileName.Substring(0, fileName.IndexOf("."));
        ////    row.Cells[BrownFilePath.Name].Value = filePath;
        ////    row.Cells[BrownFileInforType.Name].Value = "文本";
        ////    row.Cells[BrownFileState.Name].Value = "编制";
        ////    FileInfo fileInfo = new FileInfo(filePath);
        ////    row.Cells[BrownFileSize.Name].Value = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetFileAutoSizeString(fileInfo.Length, 3);
        ////}

        //private void InsertIntoGridDocument(ProObjectRelaDocument doc)
        //{
        //    int index = gridDocument.Rows.Add();
        //    DataGridViewRow row = gridDocument.Rows[index];
        //    row.Cells[DocumentName.Name].Value = doc.DocumentName;
        //    row.Cells[DocumentCode.Name].Value = doc.DocumentCode;
        //    row.Cells[DocumentCateCode.Name].Value = doc.DocumentCateCode;
        //    row.Cells[DocumentCateName.Name].Value = doc.DocumentCateName;
        //    //row.Cells[UploadPerson.Name].Value = doc.DocumentOwnerName;
        //    //row.Cells[UploadDate.Name].Value = doc.SubmitTime.ToString();
        //    row.Cells[DocumentDesc.Name].Value = doc.DocumentDesc;

        //    row.Tag = doc;
        //}
        //private void UpdateDocument(ProObjectRelaDocument doc)
        //{
        //    foreach (DataGridViewRow row in gridDocument.Rows)
        //    {
        //        ProObjectRelaDocument docTemp = row.Tag as ProObjectRelaDocument;
        //        if (docTemp.Id == doc.Id)
        //        {
        //            row.Cells[DocumentName.Name].Value = doc.DocumentName;
        //            row.Cells[DocumentDesc.Name].Value = doc.DocumentDesc;

        //            row.Tag = doc;

        //            gridDocument.CurrentCell = row.Cells[1];
        //            break;
        //        }
        //    }
        //}

        //private string GetExceptionMessage(PLMWebServices.ErrorStack es)
        //{
        //    string msg = es.Message;
        //    PLMWebServices.ErrorStack esTemp = es.InnerErrorStack;
        //    while (esTemp != null && !string.IsNullOrEmpty(esTemp.Message))
        //    {
        //        msg += "；\n" + esTemp.Message;
        //        esTemp = esTemp.InnerErrorStack;
        //    }

        //    if (msg.IndexOf("不能在具有唯一索引") > -1 && msg.IndexOf("插入重复键") > -1)
        //    {
        //        msg = "已存在同名文档，请重命名该文档名称.";
        //    }

        //    return msg;
        //}
        //批量上传
        //void btnUpFile_Click(object sender, EventArgs e)
        //{
        //    VDocumentUploadList vdul = new VDocumentUploadList(projectInfo, oprNode, oprNode.Id);
        //    vdul.ShowDialog();
        //    IList resultDocumentList = vdul.ResultListDoc;
        //    if (resultDocumentList == null) return;
        //    //gridDocument.Rows.Clear();
        //    foreach (ProObjectRelaDocument doc in resultDocumentList)
        //    {
        //        int rowIndex = gridDocument.Rows.Add();
        //        gridDocument[DocumentName.Name, rowIndex].Value = doc.DocumentName;
        //        gridDocument[DocumentCode.Name, rowIndex].Value = doc.DocumentCode;
        //        gridDocument[DocumentCateCode.Name, rowIndex].Value = doc.DocumentCateCode;
        //        gridDocument[DocumentCateName.Name, rowIndex].Value = doc.DocumentCateName;
        //        gridDocument[DocumentDesc.Name, rowIndex].Value = doc.DocumentDesc;
        //        gridDocument.Rows[rowIndex].Tag = doc;
        //    }
        //}


        //双击单元格事件 选文档分类编码
        //void gridBrownFileList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex == gridBrownFileList.Rows[e.RowIndex].Cells[BrownFileCateName.Name].ColumnIndex)
        //    {
        //        VDocumentSortSelect vdss = new VDocumentSortSelect();
        //        vdss.ShowDialog();
        //        PLMWebServicesByKB.CategoryNode cate = vdss.ResultCate;
        //        if (cate != null)
        //        {
        //            gridBrownFileList.Rows[e.RowIndex].Cells[BrownFileCateName.Name].Tag = cate;
        //            gridBrownFileList.Rows[e.RowIndex].Cells[BrownFileCateName.Name].Value = cate.CategoryName;
        //        }
        //    }
        //}
        #endregion 文档操作

        void tvwCategory_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (isSelectNodeInvoke)
            {
                isSelectNodeInvoke = false;
            }
            else
            {
                #region 单击复选框操作
                if ((Control.ModifierKeys & Keys.Shift) != 0 || (Control.ModifierKeys & Keys.Control) != 0)//如果同时按下了Shift或Ctrl
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

                oprNode = currNode.Tag as PBSTree;

                oprNode = model.GetPBSTreeById(oprNode.Id) as PBSTree;

                currNode.Tag = oprNode;

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
                this.cbType.Text = oprNode.StructTypeName;
                //this.cbType.Text = oprNode.StructTypeName;

                this.txtDesc.Text = oprNode.Describe;

                //查询相关文档
                ////txtDocumentName.Text = "";
                ////txtDocumentDesc.Text = "";
                ////txtDocumentPath.Text = "";

                //gridDocument.Rows.Clear();
                //ObjectQuery oq = new ObjectQuery();
                //oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", oprNode.Id));
                //IList listDocument = model.ObjectQuery(typeof(ProObjectRelaDocument), oq);
                //if (listDocument != null && listDocument.Count > 0)
                //{
                //    foreach (ProObjectRelaDocument doc in listDocument)
                //    {
                //        InsertIntoGridDocument(doc);
                //    }
                //}
                //gridDocument.ClearSelection();
                FillDoc();
                ShowElements(oprNode);
                RefreshControls(MainViewState.Browser);
            }
            catch (Exception exp)
            {
                MessageBox.Show("显示出错：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        private void ClearAll(MainViewState state)
        {
            this.txtCurrentPath.Text = "";
            this.txtCode.Text = "";
            this.txtName.Text = "";
            this.txtDesc.Text = "";

            //this.gridDocument.Rows.Clear();
            //this.txtDocumentName.Text = "";
            //this.txtDocumentDesc.Text = "";
            //this.txtDocumentPath.Text = "";

            ClearStructTypeDropDownList(state);
        }

        private void ClearStructTypeDropDownList(MainViewState state)
        {
            this.cbType.Items.Clear();
            if (state == MainViewState.AddNew || state == MainViewState.Modify)
            {
                if (oprNode != null)
                {
                    IList list = null;
                    if (state == MainViewState.AddNew)
                    {
                        list = GetChildStructTypeObject(oprNode.StructTypeName);

                        if (list != null && list.Count > 0)
                        {
                            foreach (BasicDataOptr type in list)
                            {
                                cbType.Items.Add(type);
                            }
                            cbType.DisplayMember = "BasicName";
                            cbType.ValueMember = "Id";
                            cbType.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        list = GetChildStructTypeObject((currNode.Parent.Tag as PBSTree).StructTypeName);

                        if (list != null && list.Count > 0)
                        {
                            foreach (BasicDataOptr type in list)
                            {
                                cbType.Items.Add(type);
                                if (type.Id == (currNode.Tag as PBSTree).StructTypeGUID)
                                    cbType.SelectedIndex = cbType.Items.Count - 1;
                            }
                            cbType.DisplayMember = "BasicName";
                            cbType.ValueMember = "Id";
                        }
                    }
                }
            }
            else if (state == MainViewState.Browser)
            {
                if (listStructureType.Count > 0)
                {
                    foreach (BasicDataOptr type in listStructureType)
                    {
                        cbType.Items.Add(type);
                    }
                    cbType.DisplayMember = "BasicName";
                    cbType.ValueMember = "Id";

                    cbType.SelectedIndex = 0;
                }
            }
        }

        private List<BasicDataOptr> GetCurrNodeStructType(string typeId, MainViewState state)
        {
            try
            {
                var query1 = from t in listStructureType
                             where t.Id == typeId
                             select t;

                BasicDataOptr currType = query1.ElementAt(0);


                List<BasicDataOptr> list = null;
                if (state == MainViewState.AddNew)
                {
                    var query2 = from t in listStructureType
                                 where (Convert.ToInt32(t.BasicCode) > Convert.ToInt32(currType.BasicCode))
                                 && t.BasicCode.IndexOf(currType.BasicCode.Substring(0, 1)) > -1
                                 select t;

                    list = query2.ToList();
                }
                else if (state == MainViewState.Modify)
                {
                    //var query2 = from t in listStructureType
                    //             where t.BasicCode.IndexOf(currType.BasicCode.Substring(0, 1)) > -1
                    //             && t.BasicCode != currType.BasicCode.Substring(0, 1) + '0'
                    //             select t;

                    var query2 = from t in listStructureType
                                 where (Convert.ToInt32(t.BasicCode) >= Convert.ToInt32(currType.BasicCode))
                                 && t.BasicCode.IndexOf(currType.BasicCode.Substring(0, 1)) > -1
                                 select t;

                    list = query2.ToList();
                }

                return list;
            }
            catch { }

            return null;
        }

        private void UpdateNode()
        {
            try
            {
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

                IList list = new ArrayList();
                list.Add(oprNode);
                model.DeletePBSTree(list);

                if (reset)
                {
                    PBSTree org = model.GetPBSTreeById((tvwCategory.SelectedNode.Parent.Tag as PBSTree).Id);
                    if (org != null)
                        tvwCategory.SelectedNode.Parent.Tag = org;
                }

                //如果复制的节点有勾选的从选中集合中移除
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

                ClearAll(MainViewState.AddNew);
                oprNode = new PBSTree();

                oprNode.ParentNode = this.tvwCategory.SelectedNode.Tag as PBSTree;

                //CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                if (projectInfo != null)
                {
                    oprNode.TheProjectGUID = projectInfo.Id;
                    oprNode.TheProjectName = projectInfo.Name;
                }

                txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath + "\\";
                txtName.Focus();
            }
            catch (Exception exp)
            {
                MessageBox.Show("增加节点出错：" + exp.Message);
            }
        }

        private void InsertBrotherNode()
        {
            try
            {
                IsInsertNode = true;

                ClearAll(MainViewState.Modify);

                oprNode = new PBSTree();

                oprNode.ParentNode = (currNode.Parent.Tag as PBSTree);

                if (projectInfo != null)
                {
                    oprNode.TheProjectGUID = projectInfo.Id;
                    oprNode.TheProjectName = projectInfo.Name;
                }

                txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath + "\\";
                txtName.Focus();
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

        private bool ValideSave()
        {

            try
            {
                if (oprNode == null)
                {
                    oprNode = new PBSTree();
                }
                if (!string.IsNullOrEmpty(oprNode.Id))
                {
                    string currentPath = currNode.Parent == null ? "" : currNode.Parent.FullPath;
                    string name = txtName.Text.Trim();
                    oprNode.FullPath = currentPath == "" ? name : currentPath + @"\" + name;
                }
                else if (string.IsNullOrEmpty(oprNode.Id))
                {
                    oprNode.FullPath = txtCurrentPath.Text + txtName.Text.Trim();
                }
                if (string.IsNullOrEmpty(oprNode.TheProjectGUID) || string.IsNullOrEmpty(oprNode.TheProjectName))
                {
                    if (projectInfo != null)
                    {
                        oprNode.TheProjectGUID = projectInfo.Id;
                        oprNode.TheProjectName = projectInfo.Name;
                    }
                }

                //if (txtCode.Text.Trim() == "")
                //    throw new Exception("编码不能为空!");
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("名称不能为空!");
                    txtName.Focus();
                    return false;
                }
                if (cbType.Text == "")
                {
                    MessageBox.Show("结构类型不能为空!");
                    cbType.Focus();
                    return false;
                }

                oprNode.Name = txtName.Text.Trim();
                oprNode.Code = txtCode.Text.Trim();
                if (cbType.SelectedItem != null)
                {
                    BasicDataOptr basic = cbType.SelectedItem as BasicDataOptr;
                    oprNode.StructTypeGUID = basic.Id;
                    oprNode.StructTypeName = basic.BasicName;
                }
                oprNode.Describe = txtDesc.Text.Trim();
                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
                return false;
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
            tvwCategory.DragEnter += new DragEventHandler(tvwCategory_DragEnter);
            tvwCategory.DragDrop += new DragEventHandler(tvwCategory_DragDrop);
            tvwCategory.DragOver += new DragEventHandler(tvwCategory_DragOver);

            LoadPBSTreeTree();
        }

        private void LoadPBSTreeTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();
                IList list = model.GetPBSTreesByInstance(projectInfo.Id);

                if (list.Count == 0)
                {
                    IList listUpdate = new ArrayList();

                    PBSTree root = new PBSTree();

                    if (projectInfo != null)
                    {
                        root.TheProjectGUID = projectInfo.Id;
                        root.TheProjectName = projectInfo.Name;

                        root.Name = projectInfo.Name;
                    }

                    var listBasicData = from t in listStructureType
                                        where t.BasicName == "项目"
                                        select t;

                    if (listBasicData.Count() > 0)
                    {
                        BasicDataOptr baseData = listBasicData.ElementAt(0);

                        root.StructTypeGUID = baseData.Id;
                        root.StructTypeName = baseData.BasicName;

                        root.Code = projectInfo.Code.PadLeft(4, '0') + "-" + baseData.BasicCode + "-" + model.GetCode(typeof(PBSTree));
                    }

                    listUpdate.Add(root);
                    listBasicData = from t in listStructureType
                                    where t.BasicName == "建筑空间结构" || t.BasicName == "服务系统结构"
                                    orderby t.BasicCode ascending
                                    select t;

                    if (listBasicData.Count() > 0)
                    {
                        foreach (BasicDataOptr baseData in listBasicData)
                        {
                            PBSTree node = new PBSTree();
                            node.Name = baseData.BasicName;
                            node.StructTypeGUID = baseData.Id;
                            node.StructTypeName = baseData.BasicName;
                            node.Code = projectInfo.Code.PadLeft(4, '0') + "-" + baseData.BasicCode + "-" + model.GetCode(typeof(PBSTree));

                            if (projectInfo != null)
                            {
                                node.TheProjectGUID = projectInfo.Id;
                                node.TheProjectName = projectInfo.Name;
                            }

                            node.ParentNode = root;

                            listUpdate.Add(node);
                            //root.ChildNodes.Add(node);
                        }
                    }

                    model.SavePBSTreeRootNode(listUpdate);

                    list = model.GetPBSTreesByInstance(projectInfo.Id);
                    //lstInstance = listAll[1] as IList;
                    //list = listAll[0] as IList;
                }
                else if (list.Count == 1)//只有根节点
                {
                    IList listUpdate = new ArrayList();

                    PBSTree root = list[0] as PBSTree;
                    if (projectInfo != null)
                    {
                        root.TheProjectGUID = projectInfo.Id;
                        root.TheProjectName = projectInfo.Name;

                        root.Name = projectInfo.Name;
                        root.SysCode = root.Id + ".";
                    }

                    var listBasicData = from t in listStructureType
                                        where t.BasicName == "项目"
                                        select t;

                    if (listBasicData.Count() > 0)
                    {
                        BasicDataOptr baseData = listBasicData.ElementAt(0);

                        root.StructTypeGUID = baseData.Id;
                        root.StructTypeName = baseData.BasicName;


                        root.Code = projectInfo.Code.PadLeft(4, '0') + "-" + baseData.BasicCode + "-" + model.GetCode(typeof(PBSTree));
                    }

                    listUpdate.Add(root);


                    listBasicData = from t in listStructureType
                                    where t.BasicName == "建筑空间结构" || t.BasicName == "服务系统结构"
                                    orderby t.BasicCode ascending
                                    select t;

                    if (listBasicData.Count() > 0)
                    {
                        foreach (BasicDataOptr baseData in listBasicData)
                        {
                            PBSTree node = new PBSTree();
                            node.Name = baseData.BasicName;
                            node.StructTypeGUID = baseData.Id;
                            node.StructTypeName = baseData.BasicName;
                            node.Code = projectInfo.Code.PadLeft(4, '0') + "-" + baseData.BasicCode + "-" + model.GetCode(typeof(PBSTree));

                            if (projectInfo != null)
                            {
                                node.TheProjectGUID = projectInfo.Id;
                                node.TheProjectName = projectInfo.Name;
                            }

                            node.ParentNode = root;

                            listUpdate.Add(node);
                            //root.ChildNodes.Add(node);
                        }
                    }

                    model.SavePBSTrees(listUpdate);

                    list = model.GetPBSTreesByInstance(projectInfo.Id);
                }

                foreach (PBSTree childNode in list)
                {
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
                MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        private void LoadPBSTreeTreeBySql()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();

                IList list = new ArrayList();

                DataTable dt = model.GetPBSTreesInstanceBySql(projectInfo.Id);
                foreach (DataRow dataRow in dt.Rows)
                {
                    PBSTree pbs = new PBSTree();
                    pbs.Id = dataRow["Id"].ToString();
                    pbs.Name = dataRow["Name"].ToString();
                    pbs.SysCode = dataRow["SysCode"].ToString();
                    pbs.OrderNo = ClientUtil.ToLong(dataRow["OrderNo"]);
                    pbs.ParentNode = new PBSTree();
                    pbs.ParentNode.Id = dataRow["parentnodeid"].ToString();
                    pbs.StructTypeGUID = dataRow["structtypeguid"].ToString();
                    pbs.StructTypeName = dataRow["structtypename"].ToString();
                    pbs.TheProjectGUID = dataRow["theProjectGUID"].ToString();
                    pbs.TheProjectName = dataRow["theProjectName"].ToString();
                    list.Add(pbs);
                }

                if (list.Count == 0)
                {
                    IList listUpdate = new ArrayList();

                    PBSTree root = new PBSTree();

                    if (projectInfo != null)
                    {
                        root.TheProjectGUID = projectInfo.Id;
                        root.TheProjectName = projectInfo.Name;

                        root.Name = projectInfo.Name;
                        root.FullPath = root.Name;
                    }

                    var listBasicData = from t in listStructureType
                                        where t.BasicName == "项目"
                                        select t;

                    if (listBasicData.Count() > 0)
                    {
                        BasicDataOptr baseData = listBasicData.ElementAt(0);

                        root.StructTypeGUID = baseData.Id;
                        root.StructTypeName = baseData.BasicName;

                        root.Code = projectInfo.Code.PadLeft(4, '0') + "-" + baseData.BasicCode + "-" + model.GetCode(typeof(PBSTree));
                    }

                    listUpdate.Add(root);


                    listBasicData = from t in listStructureType
                                    where t.BasicName == "建筑空间结构" || t.BasicName == "服务系统结构"
                                    orderby t.BasicCode ascending
                                    select t;

                    if (listBasicData.Count() > 0)
                    {
                        foreach (BasicDataOptr baseData in listBasicData)
                        {
                            PBSTree node = new PBSTree();
                            node.Name = baseData.BasicName;
                            node.StructTypeGUID = baseData.Id;
                            node.StructTypeName = baseData.BasicName;
                            node.Code = projectInfo.Code.PadLeft(4, '0') + "-" + baseData.BasicCode + "-" + model.GetCode(typeof(PBSTree));

                            if (projectInfo != null)
                            {
                                node.TheProjectGUID = projectInfo.Id;
                                node.TheProjectName = projectInfo.Name;
                            }

                            node.ParentNode = root;

                            node.FullPath = root.FullPath + @"\" + node.Name;

                            listUpdate.Add(node);
                        }
                    }

                    model.SavePBSTreeRootNode(listUpdate);

                    list = model.GetPBSTreesByInstance(projectInfo.Id);

                }
                else if (list.Count == 1)//只有根节点
                {
                    IList listUpdate = new ArrayList();

                    PBSTree root = list[0] as PBSTree;

                    if (projectInfo != null)
                    {
                        root.TheProjectGUID = projectInfo.Id;
                        root.TheProjectName = projectInfo.Name;

                        root.Name = projectInfo.Name;
                        root.SysCode = root.Id + ".";
                        root.FullPath = root.Name;
                    }

                    var listBasicData = from t in listStructureType
                                        where t.BasicName == "项目"
                                        select t;

                    if (listBasicData.Count() > 0)
                    {
                        BasicDataOptr baseData = listBasicData.ElementAt(0);

                        root.StructTypeGUID = baseData.Id;
                        root.StructTypeName = baseData.BasicName;

                        root.Code = projectInfo.Code.PadLeft(4, '0') + "-" + baseData.BasicCode + "-" + model.GetCode(typeof(PBSTree));
                    }

                    listUpdate.Add(root);


                    listBasicData = from t in listStructureType
                                    where t.BasicName == "建筑空间结构" || t.BasicName == "服务系统结构"
                                    orderby t.BasicCode ascending
                                    select t;

                    if (listBasicData.Count() > 0)
                    {
                        foreach (BasicDataOptr baseData in listBasicData)
                        {
                            PBSTree node = new PBSTree();
                            node.Name = baseData.BasicName;
                            node.StructTypeGUID = baseData.Id;
                            node.StructTypeName = baseData.BasicName;
                            node.Code = projectInfo.Code.PadLeft(4, '0') + "-" + baseData.BasicCode + "-" + model.GetCode(typeof(PBSTree));

                            if (projectInfo != null)
                            {
                                node.TheProjectGUID = projectInfo.Id;
                                node.TheProjectName = projectInfo.Name;
                            }

                            node.ParentNode = root;
                            node.FullPath = root.FullPath + @"\" + node.Name;

                            listUpdate.Add(node);
                        }
                    }

                    model.SavePBSTrees(listUpdate);

                    list = model.GetPBSTreesByInstance(projectInfo.Id);
                }

                foreach (PBSTree childNode in list)
                {
                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = childNode.Name;
                    tnTmp.Tag = childNode;
                    if (childNode.ParentNode != null && !string.IsNullOrEmpty(childNode.ParentNode.Id))
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
                MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

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

                    this.linkCancel.Enabled = true;
                    this.linkSave.Enabled = true;
                    this.linkAdd.Enabled = false;
                    this.linkUpdate.Enabled = false;
                    this.linkDelete.Enabled = false;

                    txtCode.ReadOnly = true;
                    txtName.Enabled = true;
                    cbType.Enabled = true;
                    txtDesc.Enabled = true;
                    break;

                case MainViewState.Browser:

                    this.mnuTree.Items["撤销"].Enabled = false;
                    this.mnuTree.Items["保存节点"].Enabled = false;

                    if (currNode != null && currNode.Parent == null)
                    {
                        this.mnuTree.Items["增加子节点"].Enabled = false;
                        this.mnuTree.Items["插入同级节点"].Enabled = false;
                        this.mnuTree.Items["修改节点"].Enabled = false;
                        this.mnuTree.Items["删除节点"].Enabled = false;

                        this.linkAdd.Enabled = false;
                        this.linkUpdate.Enabled = false;
                        this.linkDelete.Enabled = false;
                    }
                    else if (oprNode != null && (oprNode.StructTypeName == "建筑空间结构" || oprNode.StructTypeName == "服务系统结构"))
                    {
                        this.mnuTree.Items["增加子节点"].Enabled = true;
                        this.mnuTree.Items["插入同级节点"].Enabled = false;
                        this.mnuTree.Items["修改节点"].Enabled = false;
                        this.mnuTree.Items["删除节点"].Enabled = false;

                        this.linkAdd.Enabled = true;
                        this.linkUpdate.Enabled = false;
                        this.linkDelete.Enabled = false;
                    }
                    else
                    {
                        this.mnuTree.Items["增加子节点"].Enabled = true;
                        this.mnuTree.Items["插入同级节点"].Enabled = true;
                        this.mnuTree.Items["修改节点"].Enabled = true;
                        this.mnuTree.Items["删除节点"].Enabled = true;

                        this.linkAdd.Enabled = true;
                        this.linkUpdate.Enabled = true;
                        this.linkDelete.Enabled = true;
                    }

                    this.linkCancel.Enabled = false;
                    this.linkSave.Enabled = false;

                    txtCode.ReadOnly = true;
                    txtName.Enabled = false;
                    cbType.Enabled = false;
                    txtDesc.Enabled = false;

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
            if (修改节点.Enabled)
            {
                ClearStructTypeDropDownList(MainViewState.Modify);
                RefreshControls(MainViewState.Modify);
                txtName.Focus();

                return true;
            }
            return false;
        }

        public override bool CancelView()
        {
            try
            {
                if (撤销.Enabled)
                {
                    mnuTree_ItemClicked(撤销, new ToolStripItemClickedEventArgs(撤销));
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

                LoadPBSTreeTree();
            }
            catch (Exception e)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(e));
            }
        }

        public override bool SaveView()
        {
            bool isNew = false;
            try
            {
                if (!ValideSave())
                    return false;

                if (oprNode.Id == null)
                {
                    isNew = true;

                    var query = from t in listStructureType
                                where t.Id == oprNode.StructTypeGUID
                                select t;

                    if (projectInfo != null && query.Count() > 0)
                    {
                        oprNode.Code = projectInfo.Code.PadLeft(4, '0') + "-" + query.ElementAt(0).BasicCode + "-" + model.GetCode(typeof(PBSTree));
                    }

                    if (IsInsertNode)
                    {
                        IList list = new ArrayList();

                        long orderNo = (currNode.Tag as PBSTree).OrderNo;
                        oprNode.OrderNo = orderNo;

                        list.Add(oprNode);

                        TreeNode parentNode = currNode.Parent;
                        for (int i = currNode.Index; i < parentNode.Nodes.Count; i++)
                        {
                            PBSTree pbs = parentNode.Nodes[i].Tag as PBSTree;
                            pbs.OrderNo += 1;
                            list.Add(pbs);
                        }

                        list = model.InsertOrUpdatePBSTrees(list);

                        oprNode = list[0] as PBSTree;

                        //插入子节点的父节点需要重新设置Tag
                        currNode.Parent.Tag = oprNode.ParentNode;

                        //更新tag
                        for (int i = currNode.Index; i < parentNode.Nodes.Count; i++)
                        {
                            PBSTree pbs = parentNode.Nodes[i].Tag as PBSTree;

                            foreach (PBSTree p in list)
                            {
                                if (p.Id == pbs.Id)
                                {
                                    parentNode.Nodes[i].Tag = p;
                                    break;
                                }
                            }
                        }

                        TreeNode tn = this.tvwCategory.SelectedNode.Parent.Nodes.Insert(currNode.Index, oprNode.Name.ToString());
                        tn.Name = oprNode.Id;
                        tn.Tag = oprNode;

                        this.tvwCategory.SelectedNode = tn;
                        tn.Expand();

                        //新增节点要有权限操作
                        //lstInstance.Add(oprNode);
                    }
                    else
                    {
                        if (oprNode.OrderNo == -1)
                        {
                            oprNode.OrderNo = currNode.Nodes.Count + 1;
                        }
                        oprNode = model.SavePBSTree(oprNode);
                    }
                }
                else
                {
                    isNew = false;

                    oprNode = model.SavePBSTree(oprNode);
                }



                if (isNew)
                {
                    if (!IsInsertNode)
                    {
                        //要添加子节点的节点以前没有子节点，需要重新设置Tag
                        if (tvwCategory.SelectedNode.Nodes.Count == 0)
                            tvwCategory.SelectedNode.Tag = oprNode.ParentNode;

                        TreeNode tn = this.tvwCategory.SelectedNode.Nodes.Add(oprNode.Id.ToString(), oprNode.Name.ToString());
                        tn.Name = oprNode.Id;

                        tn.Tag = oprNode;
                        this.tvwCategory.SelectedNode = tn;
                        tn.Expand();

                        //新增节点要有权限操作
                        //lstInstance.Add(oprNode);
                    }
                }
                else
                {
                    this.tvwCategory.SelectedNode.Text = oprNode.Name;
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

        #region 节点拖拽移动

        private void tvwCategory_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PBSTree org = (e.Item as TreeNode).Tag as PBSTree;
                //有权限的节点才允许拖动操作
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
                //目标节点没有权限不允许操作
                if (targetNode == null)// || !ConstMethod.Contains(lstInstance, targetNode.Tag as PBSTree)
                    return;
                TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

                if (TreeViewUtil.CanMoveNode(draggedNode, targetNode))
                {
                    //以前的父节点
                    TreeNode oldParentNode = draggedNode.Parent;

                    #region 校验拷贝或移动规则
                    string serviceSystemSysCode = "";//服务系统结构层次码
                    string buildSpaceSysCode = "";//建筑空间结构层次码
                    foreach (TreeNode tn in tvwCategory.Nodes[0].Nodes)
                    {
                        PBSTree pbs = tn.Tag as PBSTree;
                        if (pbs.StructTypeName == "建筑空间结构")
                        {
                            buildSpaceSysCode = pbs.SysCode;
                        }
                        else if (pbs.StructTypeName == "服务系统结构")
                        {
                            serviceSystemSysCode = pbs.SysCode;
                        }
                    }

                    if (string.IsNullOrEmpty(serviceSystemSysCode) || string.IsNullOrEmpty(buildSpaceSysCode))
                    {
                        MessageBox.Show("当前PBS结构不正确，请先在项目根节点下添加“建筑空间结构”和“服务系统结构”节点。");
                        return;
                    }


                    bool isBuildSpaceType = (targetNode.Tag as PBSTree).SysCode.IndexOf(buildSpaceSysCode) > -1;//是否属于建筑空间结构类型

                    //校验建筑空间结构和服务系统结构下的节点不允许互相拷贝
                    PBSTree copyOrMovePbsObj = draggedNode.Tag as PBSTree;
                    if (isBuildSpaceType && copyOrMovePbsObj.SysCode.IndexOf(buildSpaceSysCode) == -1)
                    {
                        MessageBox.Show("服务系统结构下的节点不支持到建筑空间结构的操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else if (isBuildSpaceType == false && copyOrMovePbsObj.SysCode.IndexOf(serviceSystemSysCode) == -1)
                    {
                        MessageBox.Show("建筑空间结构下的节点不支持到服务系统结构的操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }


                    //校验不能跨结构类型拷贝
                    List<string> listChildStructType = GetChildStructType((targetNode.Tag as PBSTree).StructTypeName);
                    if (listChildStructType.Contains(copyOrMovePbsObj.StructTypeName) == false)
                    {
                        //排序
                        if (draggedNode.Parent == targetNode.Parent)
                        {
                            if (MessageBox.Show("您是要执行排序操作吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                draggedNode.Remove();
                                targetNode.Parent.Nodes.Insert(targetNode.Index, draggedNode);

                                if (draggedNode.PrevNode != null)
                                {
                                    IList result = new ArrayList();
                                    PBSTree prevOrg = draggedNode.PrevNode.Tag as PBSTree;
                                    SetNextNodeOrder(draggedNode, result, prevOrg.OrderNo + 1);
                                    result = model.PBSTreeOrder(result);
                                    ResetTagAfterOrder(draggedNode, result, 0);
                                }
                                else
                                {
                                    PBSTree fromOrg = draggedNode.Tag as PBSTree;
                                    PBSTree toOrg = targetNode.Tag as PBSTree;
                                    fromOrg.OrderNo = toOrg.OrderNo - 1;
                                    IList list = new ArrayList();
                                    list.Add(fromOrg);
                                    draggedNode.Tag = model.PBSTreeOrder(list)[0];
                                }

                                //保证拖动后马上修改保存不出错
                                this.tvwCategory.SelectedNode = draggedNode;
                                return;
                            }
                        }

                        MessageBox.Show("目标节点的结构类型和拖拽的节点结构类型不符合父子关系，不能执行拷贝或移动操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        return;
                    }

                    #endregion

                    bool reset = false;
                    //父节点只有这一个子节点，并且父节点有权限操作，移动后要重新设置父节点tag
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
                        //复制树节点
                        if (frmTmp.MoveOrCopy == enmMoveOrCopy.copy)
                        {
                            draggedNode = frmTmp.DraggedNode;
                            PBSTree catTmp = (draggedNode.Tag as PBSTree).Clone();

                            //系统生成编码
                            var query = from t in listStructureType
                                        where t.Id == catTmp.StructTypeGUID
                                        select t;

                            if (projectInfo != null && query.Count() > 0)
                            {
                                catTmp.TheProjectGUID = projectInfo.Id;
                                catTmp.TheProjectName = projectInfo.Name;
                                //catTmp.Code = projectInfo.Code.PadLeft(4, '0') + "-" + query.ElementAt(0).BasicCode + "-"+model.GetCode(typeof(PBSTree));
                            }

                            PBSTree parentNode = targetNode.Tag as PBSTree;
                            parentNode = model.GetPBSTreeById(parentNode.Id) as PBSTree;

                            catTmp.ParentNode = parentNode;
                            catTmp.OrderNo = model.GetMaxOrderNo(catTmp) + 1;
                            catTmp.TheTree = parentNode.TheTree;

                            catTmp.OwnerGUID = ConstObject.LoginPersonInfo.Id;
                            catTmp.OwnerName = ConstObject.LoginPersonInfo.Name;
                            catTmp.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;

                            catTmp.FullPath = targetNode.FullPath + @"\" + catTmp.Name;
                            catTmp.Level = catTmp.ParentNode.Level + 1;
                            catTmp.CategoryNodeType = NodeType.LeafNode;

                            IList lst = new ArrayList();
                            if (parentNode.CategoryNodeType != NodeType.RootNode)
                                parentNode.CategoryNodeType = NodeType.MiddleNode;

                            lst.Add(parentNode);
                            lst.Add(catTmp);

                            PopulateList(draggedNode, lst, catTmp);

                            lst = model.SavePBSTrees(lst);

                            //给复制节点的新父节点tag设值
                            targetNode.Tag = lst[0] as PBSTree;
                            int i = 1;
                            CopyObjToTag(draggedNode, lst, ref i);

                            //如果复制的节点有勾选的加入到选中集合
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
                        //移动树节点
                        else if (frmTmp.MoveOrCopy == enmMoveOrCopy.move)
                        {
                            PBSTree draggedObj = draggedNode.Tag as PBSTree;
                            draggedObj.OwnerGUID = ConstObject.LoginPersonInfo.Id;
                            draggedObj.OwnerName = ConstObject.LoginPersonInfo.Name;
                            draggedObj.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;

                            PBSTree toObj = targetNode.Tag as PBSTree;
                            Hashtable dic = model.MoveNode(draggedObj, toObj);
                            if (reset)
                            {
                                PBSTree cat = null;
                                PBSTree oldParentObj = oldParentNode.Tag as PBSTree;
                                if (dic.ContainsKey(oldParentObj.Id))
                                    cat = dic[oldParentObj.Id] as PBSTree;
                                else
                                    cat = model.GetPBSTreeById(oldParentObj.Id) as PBSTree;

                                oldParentNode.Tag = cat;
                            }
                            targetNode.Tag = dic[(targetNode.Tag as PBSTree).Id.ToString()];
                            //根据返回的数据进行节点tag赋值
                            ResetTagAfterMove(draggedNode, dic);

                        }
                        //排序
                        else if (draggedNode.Parent == targetNode.Parent)
                        {
                            if (draggedNode.PrevNode != null)
                            {
                                IList result = new ArrayList();
                                PBSTree prevOrg = draggedNode.PrevNode.Tag as PBSTree;
                                SetNextNodeOrder(draggedNode, result, prevOrg.OrderNo + 1);
                                result = model.PBSTreeOrder(result);
                                ResetTagAfterOrder(draggedNode, result, 0);
                            }
                            else
                            {
                                PBSTree fromOrg = draggedNode.Tag as PBSTree;
                                PBSTree toOrg = targetNode.Tag as PBSTree;
                                fromOrg.OrderNo = toOrg.OrderNo - 1;
                                IList listPBS = new ArrayList();
                                listPBS.Add(fromOrg);
                                draggedNode.Tag = model.PBSTreeOrder(listPBS)[0];
                            }
                        }
                        //保证拖动后马上修改保存不出错
                        this.tvwCategory.SelectedNode = draggedNode;
                    }
                }//用户如果把节点移到空白区再选中被拖动节点
                else if (targetNode == null)
                {
                    tvwCategory.SelectedNode = draggedNode;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("移动分类出错：" + ExceptionUtil.ExceptionMessage(ee));
            }
        }
        //设置后续节点的排序号
        private void SetNextNodeOrder(TreeNode node, IList list, long order)
        {
            PBSTree org = node.Tag as PBSTree;
            org.OrderNo = order;
            list.Add(org);
            if (node.NextNode != null)
            {
                SetNextNodeOrder(node.NextNode, list, order + 1);
            }
        }
        //排序后重新设置节点的Tag
        private void ResetTagAfterOrder(TreeNode node, IList lst, int i)
        {
            node.Tag = lst[i];
            if (node.NextNode != null)
                ResetTagAfterOrder(node.NextNode, lst, i + 1);
        }
        //移动后重新设置节点的Tag
        private void ResetTagAfterMove(TreeNode node, IDictionary dic)
        {
            node.Tag = dic[(node.Tag as PBSTree).Id.ToString()];
            if (node.Nodes.Count == 0)
                return;
            foreach (TreeNode var in node.Nodes)
            {
                ResetTagAfterMove(var, dic);
            }
        }
        //复制后重新设置节点的Tag
        private void CopyObjToTag(TreeNode node, IList lst, ref int i)
        {
            node.Name = (lst[i] as PBSTree).Id;
            node.Tag = lst[i];

            if (node.Nodes.Count == 0)
                return;
            foreach (TreeNode var in node.Nodes)
            {
                i++;
                CopyObjToTag(var, lst, ref i);
            }
        }

        private void PopulateList(TreeNode node, IList lst, PBSTree parent)
        {
            if (node.Nodes.Count == 0)
                return;

            for (int i = 0; i < node.Nodes.Count; i++)
            {
                TreeNode var = node.Nodes[i];
                PBSTree matCatTmp = (var.Tag as PBSTree).Clone();

                //系统生成编码
                var query = from t in listStructureType
                            where t.Id == matCatTmp.StructTypeGUID
                            select t;

                if (projectInfo != null && query.Count() > 0)
                {
                    matCatTmp.TheProjectGUID = projectInfo.Id;
                    matCatTmp.TheProjectName = projectInfo.Name;
                    //matCatTmp.Code = projectInfo.Code.PadLeft(4, '0') + "-" + query.ElementAt(0).BasicCode + "-" + model.GetCode(typeof(PBSTree));
                }

                matCatTmp.ParentNode = parent;
                matCatTmp.OrderNo = i + 1;


                matCatTmp.TheTree = parent.TheTree;

                matCatTmp.OwnerGUID = ConstObject.LoginPersonInfo.Id;
                matCatTmp.OwnerName = ConstObject.LoginPersonInfo.Name;
                matCatTmp.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;
                matCatTmp.Author = parent.Author;
                matCatTmp.FullPath = ((PBSTree)matCatTmp.ParentNode).FullPath + @"\" + matCatTmp.Name;
                matCatTmp.Level = matCatTmp.ParentNode.Level + 1;
                matCatTmp.CategoryNodeType = NodeType.LeafNode;

                if (parent.CategoryNodeType != NodeType.RootNode)
                    parent.CategoryNodeType = NodeType.MiddleNode;
                matCatTmp.CategoryNodeType = NodeType.LeafNode;


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
            if (e.ClickedItem.Name == 增加子节点.Name)
            {
                RefreshControls(MainViewState.Modify);
                add_Click(null, new EventArgs());
            }
            else if (e.ClickedItem.Name == 插入同级节点.Name)
            {
                RefreshControls(MainViewState.Modify);
                InsertBrotherNode();
            }
            else if (e.ClickedItem.Name == 修改节点.Name)
            {
                mnuTree.Hide();
                ModifyView();
            }
            else if (e.ClickedItem.Name == 删除节点.Name)
            {
                mnuTree.Hide();
                delete_Click(null, new EventArgs());
                RefreshControls(MainViewState.Browser);
                this.Refresh();
            }
            else if (e.ClickedItem.Name == 撤销.Name)
            {
                mnuTree.Hide();
                RefreshControls(MainViewState.Browser);
                this.tvwCategory_AfterSelect(this.tvwCategory, new TreeViewEventArgs(this.tvwCategory.SelectedNode));
            }
            else if (e.ClickedItem.Name == 保存节点.Name)
            {
                mnuTree.Hide();
                SaveView();
            }
            else if (e.ClickedItem.Name == 复制勾选节点.Name)
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
            else if (e.ClickedItem.Name == 粘贴节点.Name)
            {
                mnuTree.Hide();

                #region 校验拷贝规则
                string serviceSystemSysCode = "";//服务系统结构层次码
                string buildSpaceSysCode = "";//建筑空间结构层次码
                foreach (TreeNode tn in tvwCategory.Nodes[0].Nodes)
                {
                    PBSTree pbs = tn.Tag as PBSTree;
                    if (pbs.StructTypeName == "建筑空间结构")
                    {
                        buildSpaceSysCode = pbs.SysCode;
                    }
                    else if (pbs.StructTypeName == "服务系统结构")
                    {
                        serviceSystemSysCode = pbs.SysCode;
                    }
                }

                if (serviceSystemSysCode == "" || buildSpaceSysCode == "")
                {
                    MessageBox.Show("当前PBS结构不正确,请先在项目根节点下添加“建筑空间结构”和“服务系统结构”！");
                    return;
                }

                bool isBuildSpaceType = oprNode.SysCode.IndexOf(buildSpaceSysCode) > -1;//是否属于建筑空间结构类型

                //校验建筑空间结构和服务系统结构下的节点不允许互相拷贝
                PBSTree copyPbsObj = listCopyNode[0].Tag as PBSTree;
                if (isBuildSpaceType && copyPbsObj.SysCode.IndexOf(buildSpaceSysCode) == -1)
                {
                    MessageBox.Show("服务系统结构下的节点不支持到建筑空间结构的拷贝！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (isBuildSpaceType == false && copyPbsObj.SysCode.IndexOf(serviceSystemSysCode) == -1)
                {
                    MessageBox.Show("建筑空间结构下的节点不支持到服务系统结构的拷贝！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                //校验不能跨结构类型拷贝
                List<string> listChildStructType = GetChildStructType(oprNode.StructTypeName);
                if (listChildStructType.Contains(copyPbsObj.StructTypeName) == false)
                {
                    MessageBox.Show("选择节点的结构类型和要粘贴的节点的结构类型的不符合父子关系！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                #endregion

                SaveCopyNode();

                RefreshControls(MainViewState.Check);
            }
            else if (e.ClickedItem.Name == 删除勾选节点.Name)
            {
                mnuTree.Hide();
                DeleteCheckedNode();

                RefreshControls(MainViewState.Check);
            }
            else if (e.ClickedItem.Name == 清除勾选节点.Name)
            {
                mnuTree.Hide();
                ClearSelectedNode(tvwCategory.Nodes[0]);

                listCheckedNode.Clear();

                RefreshControls(MainViewState.Check);
            }
        }

        /// <summary>
        /// 根据父结构类型获取子结构类型集合
        /// </summary>
        /// <param name="parentStructType"></param>
        /// <returns></returns>
        private List<string> GetChildStructType(string parentStructType)
        {
            List<string> list = new List<string>();

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("StructType", parentStructType));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

            IList listParentStructType = model.ObjectQuery(typeof(PBSStructTypeRuleMaster), oq);

            if (listParentStructType != null && listParentStructType.Count > 0)
            {
                PBSStructTypeRuleMaster master = listParentStructType[0] as PBSStructTypeRuleMaster;
                foreach (PBSStructTypeRuleDetail dtl in master.Details)
                {
                    list.Add(dtl.StructType);
                }
            }

            //switch (parentStructType)
            //{
            //    case "项目":
            //        list.Add("建筑空间结构");
            //        list.Add("服务系统结构");
            //        break;
            //    case "建筑空间结构":
            //        list.Add("场所");
            //        list.Add("单体建筑");
            //        break;
            //    case "场所":
            //        list.Add("单体建筑");
            //        break;
            //    case "单体建筑":
            //        list.Add("单体建筑段");
            //        list.Add("建筑层组");
            //        list.Add("建筑层");
            //        list.Add("建筑区组");
            //        list.Add("建筑区");
            //        break;
            //    case "单体建筑段":
            //        list.Add("建筑层组");
            //        list.Add("建筑层");
            //        list.Add("建筑区组");
            //        list.Add("建筑区");
            //        break;
            //    case "建筑层组":
            //        list.Add("建筑层");
            //        break;
            //    case "建筑层":
            //        list.Add("建筑区组");
            //        list.Add("建筑区");
            //        break;
            //    case "建筑区组":
            //        list.Add("建筑区");
            //        break;
            //    case "建筑区":
            //        list.Add("建筑层组");
            //        list.Add("建筑层");
            //        break;


            //    case "服务系统结构":
            //        list.Add("单体建筑服务系统");
            //        break;
            //    case "单体建筑服务系统":
            //        list.Add("建筑段服务系统");
            //        list.Add("系统");
            //        break;
            //    case "建筑段服务系统":
            //        list.Add("系统");
            //        list.Add("子系统");
            //        break;
            //    case "系统":
            //        list.Add("建筑段服务系统");
            //        list.Add("子系统");
            //        list.Add("系统层组");
            //        list.Add("系统层");
            //        list.Add("系统区");
            //        break;
            //    case "系统层组":
            //        list.Add("系统层");
            //        list.Add("系统区");
            //        break;
            //    case "系统层":
            //        list.Add("系统区");
            //        break;
            //    case "系统区":
            //        list.Add("系统层");
            //        break;
            //    default:
            //        break;
            //}

            return list;
        }
        /// <summary>
        /// 根据父结构类型获取子结构类型集合
        /// </summary>
        /// <param name="parentStructType"></param>
        /// <returns></returns>
        private List<BasicDataOptr> GetChildStructTypeObject(string parentStructType)
        {
            List<string> listTemp = GetChildStructType(parentStructType);

            List<BasicDataOptr> list = (from t in listStructureType
                                        where listTemp.Contains(t.BasicName)
                                        select t).ToList();

            //switch (parentStructType)
            //{
            //    case "项目":
            //        list = (from t in listStructureType
            //                where t.BasicName == "建筑空间结构"
            //                || t.BasicName == "服务系统结构"
            //                select t).ToList();
            //        break;
            //    case "建筑空间结构":
            //        list = (from t in listStructureType
            //                where t.BasicName == "场所"
            //                || t.BasicName == "单体建筑"
            //                select t).ToList();
            //        break;
            //    case "场所":
            //        list = (from t in listStructureType
            //                where t.BasicName == "单体建筑"
            //                select t).ToList();
            //        break;
            //    case "单体建筑":
            //        list = (from t in listStructureType
            //                where t.BasicName == "单体建筑段"
            //                || t.BasicName == "建筑层组"
            //                || t.BasicName == "建筑层"
            //                || t.BasicName == "建筑区组"
            //                || t.BasicName == "建筑区"
            //                select t).ToList();
            //        break;
            //    case "单体建筑段":
            //        list = (from t in listStructureType
            //                where t.BasicName == "建筑区组"
            //                || t.BasicName == "建筑区"
            //                || t.BasicName == "建筑层组"
            //                || t.BasicName == "建筑层"
            //                select t).ToList();
            //        break;
            //    case "建筑层组":
            //        list = (from t in listStructureType
            //                where t.BasicName == "建筑层"
            //                select t).ToList();
            //        break;
            //    case "建筑层":
            //        list = (from t in listStructureType
            //                where t.BasicName == "建筑区组"
            //                || t.BasicName == "建筑区"
            //                select t).ToList();
            //        break;
            //    case "建筑区组":
            //        list = (from t in listStructureType
            //                where t.BasicName == "建筑区"
            //                select t).ToList();
            //        break;
            //    case "建筑区":
            //        list = (from t in listStructureType
            //                where t.BasicName == "建筑层组"
            //                || t.BasicName == "建筑层"
            //                select t).ToList();
            //        break;


            //    case "服务系统结构":
            //        list = (from t in listStructureType
            //                where t.BasicName == "单体建筑服务系统"
            //                select t).ToList();
            //        break;
            //    case "单体建筑服务系统":
            //        list = (from t in listStructureType
            //                where t.BasicName == "建筑段服务系统"
            //                || t.BasicName == "系统"
            //                select t).ToList();
            //        break;
            //    case "建筑段服务系统":
            //        list = (from t in listStructureType
            //                where t.BasicName == "系统"
            //                || t.BasicName == "子系统"
            //                select t).ToList();
            //        break;
            //    case "系统":
            //        list = (from t in listStructureType
            //                where t.BasicName == "建筑段服务系统"
            //                || t.BasicName == "子系统"
            //                || t.BasicName == "系统层组"
            //                || t.BasicName == "系统层"
            //                || t.BasicName == "系统区"
            //                select t).ToList();
            //        break;
            //    case "子系统":
            //        list = (from t in listStructureType
            //                where t.BasicName == "系统层组"
            //                || t.BasicName == "系统层"
            //                || t.BasicName == "系统区"
            //                select t).ToList();
            //        break;
            //    case "系统层组":
            //        list = (from t in listStructureType
            //                where t.BasicName == "系统层"
            //                || t.BasicName == "系统区"
            //                select t).ToList();
            //        break;
            //    case "系统层":
            //        list = (from t in listStructureType
            //                where t.BasicName == "系统区"
            //                select t).ToList();
            //        break;
            //    case "系统区":
            //        list = (from t in listStructureType
            //                where t.BasicName == "系统层"
            //                select t).ToList();
            //        break;
            //    default:
            //        break;
            //}
            return list;
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
                               where (n.Value.Tag as PBSTree).SysCode.IndexOf((parentNode.Tag as PBSTree).SysCode) > -1
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

        private void SaveCopyNode()
        {
            if (listCopyNode.Count > 0)
            {
                IList lst = new ArrayList();
                foreach (TreeNode node in listCopyNode)
                {
                    PBSTree catTmp = (node.Tag as PBSTree).Clone();

                    catTmp.OwnerGUID = ConstObject.LoginPersonInfo.Id;
                    catTmp.OwnerName = ConstObject.LoginPersonInfo.Name;
                    catTmp.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;

                    PBSTree parentNode = oprNode;// tvwCategory.SelectedNode.Tag as PBSTree

                    catTmp.ParentNode = parentNode;
                    catTmp.TheTree = parentNode.TheTree;
                    catTmp.OrderNo = model.GetMaxOrderNo(catTmp) + 1;

                    catTmp.FullPath = ((PBSTree)catTmp.ParentNode).FullPath + @"\" + catTmp.Name;
                    catTmp.Level = catTmp.ParentNode.Level + 1;
                    catTmp.CategoryNodeType = NodeType.LeafNode;

                    lst.Add(catTmp);

                    GetCopyNode(node, catTmp, ref lst);
                }

                if (oprNode.CategoryNodeType != NodeType.RootNode)
                    oprNode.CategoryNodeType = NodeType.MiddleNode;

                lst.Insert(0, oprNode);

                //保存复制的节点
                lst = model.SavePBSTrees(lst);

                //给复制节点的新父节点tag设值
                oprNode = lst[0] as PBSTree;
                tvwCategory.SelectedNode.Tag = oprNode;

                IEnumerable<PBSTree> listCopyPBS = lst.OfType<PBSTree>();

                IEnumerable<PBSTree> listCopyRoot = from n in listCopyPBS
                                                    where n.ParentNode.Id == oprNode.Id
                                                    select n;

                foreach (PBSTree pbs in listCopyRoot)
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

        private void AddCopyChildNode(TreeNode parentNode, PBSTree parentPBS, IEnumerable<PBSTree> listCopyPBS)
        {
            IEnumerable<PBSTree> listCopyChild = from n in listCopyPBS
                                                 where n.ParentNode.Id == parentPBS.Id
                                                 select n;

            foreach (PBSTree pbs in listCopyChild)
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
        private void GetCopyNode(TreeNode copyParentNode, PBSTree saveParentNode, ref IList list)
        {
            foreach (TreeNode node in copyParentNode.Nodes)
            {
                if (listCopyNodeAll.Keys.Contains(node.Name))
                {
                    PBSTree catTmp = (node.Tag as PBSTree).Clone();

                    catTmp.ParentNode = saveParentNode;

                    catTmp.OwnerGUID = ConstObject.LoginPersonInfo.Id;
                    catTmp.OwnerName = ConstObject.LoginPersonInfo.Name;
                    catTmp.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;
                    catTmp.Author = saveParentNode.Author;
                    catTmp.TheTree = saveParentNode.TheTree;

                    catTmp.FullPath = ((PBSTree)catTmp.ParentNode).FullPath + @"\" + catTmp.Name;
                    catTmp.Level = catTmp.ParentNode.Level + 1;
                    if (saveParentNode.CategoryNodeType != NodeType.RootNode)
                        saveParentNode.CategoryNodeType = NodeType.MiddleNode;
                    catTmp.CategoryNodeType = NodeType.LeafNode;

                    list.Add(catTmp);

                    GetCopyNode(node, catTmp, ref list);
                }
            }
        }

        private void DeleteCheckedNode()
        {
            try
            {
                IList list = new ArrayList();
                foreach (var dic in listCheckedNode)
                {
                    PBSTree pbs = dic.Value.Tag as PBSTree;
                    if (dic.Value.Parent == null)
                    {
                        MessageBox.Show("根节点不允许删除！");
                        return;
                    }
                    else if (pbs.StructTypeName == "建筑空间结构" || pbs.StructTypeName == "服务系统结构")
                    {
                        MessageBox.Show("“" + pbs.StructTypeName + "”类型节点不允许删除！");
                        return;
                    }

                    list.Add(pbs);
                }

                string text = "要删除勾选的所有节点吗？该操作将连它们的所有子节点一同删除！";
                if (MessageBox.Show(text, "删除节点", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

                if (model.DeletePBSTree(list))//删除成功
                {
                    //从PBS树上移除对应的节点
                    foreach (PBSTree pbs in list)
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
        private bool RemoveTreeNode(TreeNode parentNode, PBSTree pbs)
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


                if (listCopyNode.Count == 0)
                    mnuTree.Items["粘贴节点"].Enabled = false;
                else
                    mnuTree.Items["粘贴节点"].Enabled = true;


                if (e.Node.Parent == null)
                {
                    mnuTree.Items["删除节点"].Enabled = false;
                    mnuTree.Items["粘贴节点"].Enabled = false;
                    mnuTree.Items["插入同级节点"].Enabled = false;
                }

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

                mnuTree.Show(tvwCategory, new Point(e.X, e.Y));
            }
        }

        public void ReloadTreeNode()
        {
            if (isNew)
            {
                //要添加子节点的节点以前没有子节点，需要重新设置Tag
                if (tvwCategory.SelectedNode.Nodes.Count == 0)
                    tvwCategory.SelectedNode.Tag = oprNode.ParentNode;
                TreeNode tn = this.tvwCategory.SelectedNode.Nodes.Add(oprNode.Id.ToString(), oprNode.Name.ToString());
                //新增节点要有权限操作
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

        #region 操作按钮
        private void linkAdd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RefreshControls(MainViewState.Modify);
            add_Click(null, new EventArgs());
        }

        private void linkUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mnuTree.Hide();
            ModifyView();
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
        #endregion


        #region 元素
        //取消
        void btnElementCancel_Click(object sender, EventArgs e)
        {
            Clear();
            gbElement.Enabled = false;
        }
        //保存
        void btnElementSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool isNew = true;
                decimal workAmount = 0;
                try
                {
                    workAmount = Convert.ToDecimal(txtElWorkAmount.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("工程量输入有误,请检查！");
                    return;
                }

                Elements element = new Elements();
                if (txtElName.Tag != null)
                {
                    element = txtElName.Tag as Elements;
                    isNew = false;
                }
                else
                {
                    element.PbsId = oprNode.Id;
                    element.PbsName = oprNode.Name;
                }
                element.Name = txtElName.Text;
                element.Code = txtElCode.Text;
                if (txtElUnit.Tag != null)
                {
                    element.Unit = txtElUnit.Tag as StandardUnit;
                    element.UnitName = txtElUnit.Text;
                }
                element.WorkAmount = workAmount;
                element.Type = txtElType.Text;
                if (txtElRes.Tag != null)
                {
                    element.Resources = txtElRes.Text;
                    element.ResourcesName = txtElRes.Tag.ToString();
                }
                element.Description = txtElDes.Text;
                IList list = new ArrayList();
                list.Add(element);
                element = model.SaveOrUpdate(list)[0] as Elements;
                int index = -1;
                if (isNew)
                {
                    index = dgElements.Rows.Add();
                    dgElements_SelectionChanged(dgElements, new EventArgs());
                }
                else
                {
                    index = dgElements.CurrentRow.Index;
                }
                dgElements[ElementName.Name, index].Value = element.Name;
                dgElements[ElementCode.Name, index].Value = element.Code;
                dgElements[ElementUnit.Name, index].Value = element.UnitName;
                dgElements[ElementWorkAmount.Name, index].Value = element.WorkAmount;
                dgElements[ElementType.Name, index].Value = element.Type;
                dgElements[ElementDes.Name, index].Value = element.Description;
                dgElements[ElementRes.Name, index].Value = element.Resources;
                dgElements.Rows[index].Tag = element;
                gbElement.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存出错：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        //删除
        void btnElementDelete_Click(object sender, EventArgs e)
        {
            if (dgElements.Rows.Count == 0 || dgElements.CurrentRow.Tag == null) return;
            try
            {
                if (MessageBox.Show("其特性集也一起删除，确定删除？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Elements element = dgElements.CurrentRow.Tag as Elements;
                    model.DeleteElement(element);
                    dgElements.Rows.Remove(dgElements.CurrentRow);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除出错：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        //修改
        void btnElementUpdate_Click(object sender, EventArgs e)
        {
            if (dgElements.Rows.Count == 0 || dgElements.CurrentRow.Tag == null) return;
            gbElement.Enabled = true;
            Elements element = dgElements.CurrentRow.Tag as Elements;
            txtElName.Tag = element;
        }
        //新增
        void btnElementAdd_Click(object sender, EventArgs e)
        {
            Clear();
            gbElement.Enabled = true;
        }
        //显示数据列表
        void ShowElements(PBSTree pbs)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("PbsId", pbs.Id));
            IList list = model.ObjectQuery(typeof(Elements), oq);
            dgElements.Rows.Clear();
            foreach (Elements e in list)
            {
                int index = dgElements.Rows.Add();
                dgElements[ElementName.Name, index].Value = e.Name;
                dgElements[ElementCode.Name, index].Value = e.Code;
                dgElements[ElementUnit.Name, index].Value = e.UnitName;
                dgElements[ElementWorkAmount.Name, index].Value = e.WorkAmount;
                dgElements[ElementType.Name, index].Value = e.Type;
                dgElements[ElementDes.Name, index].Value = e.Description;
                dgElements[ElementRes.Name, index].Value = e.Resources;
                dgElements.Rows[index].Tag = e;
            }
            if (list != null && list.Count > 0)
            {
                dgElements_SelectionChanged(dgElements, new EventArgs());
            }
        }
        //清空数据
        void Clear()
        {
            txtElCode.Text = "";
            txtElDes.Text = "";
            txtElName.Text = "";
            txtElName.Tag = null;
            txtElRes.Text = "";
            txtElRes.Tag = null;
            txtElType.Text = "";
            txtElUnit.Text = "";
            txtElUnit.Tag = null;
            txtElWorkAmount.Text = "";
        }

        void dgElements_SelectionChanged(object sender, EventArgs e)
        {
            gbElement.Enabled = false;
            if (dgElements.Rows.Count == 0 || dgElements.CurrentRow == null || dgElements.CurrentRow.Tag == null)
            {
                Clear();
                return;
            }
            Elements element = dgElements.CurrentRow.Tag as Elements;
            txtElCode.Text = element.Code;
            txtElDes.Text = element.Description;
            txtElName.Text = element.Name;
            txtElName.Tag = element;
            txtElRes.Text = element.Resources;
            txtElRes.Tag = element.ResourcesName;
            txtElType.Text = element.Type;
            txtElUnit.Text = element.UnitName;
            if (element.Unit != null)
                txtElUnit.Tag = element.Unit; ;
            txtElWorkAmount.Text = element.WorkAmount.ToString();
        }

        //元素特性
        void btnElementFeature_Click(object sender, EventArgs e)
        {
            if (dgElements.Rows.Count == 0 || dgElements.CurrentRow.Tag == null) return;

            Elements element = dgElements.CurrentRow.Tag as Elements;
            VElementFeature frm = new VElementFeature(element);
            frm.ShowDialog();
        }
        //选择计量单位
        void btnUnit_Click(object sender, EventArgs e)
        {
            StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
            if (su != null)
            {
                txtElUnit.Tag = su;
                txtElUnit.Text = su.Name;
                txtElUnit.Focus();
            }
        }
        //选择资源
        void btnSelectRes_Click(object sender, EventArgs e)
        {
            CommonMaterial materialSelector = new CommonMaterial();
            materialSelector.OpenSelect();
            IList list = materialSelector.Result;
            if (list.Count > 0)
            {
                Material mat = list[0] as Material;
                txtElRes.Text = mat.Name;
                txtElRes.Tag = mat.Id;
            }
        }
        //关系维护
        void btnRelation_Click(object sender, EventArgs e)
        {
            if (dgElements.Rows.Count == 0 || dgElements.CurrentRow.Tag == null) return;
            Elements element = dgElements.CurrentRow.Tag as Elements;
            VElementAndPBSRelation frm = new VElementAndPBSRelation(element);
            frm.ShowDialog();
        }
        #endregion
    }
}
