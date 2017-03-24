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

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.PBS
{
    public partial class VPBSTreeBakNew : TBasicDataView
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
        /// 记录修改之前的根节点编码
        /// </summary>
        private string rootCode;

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
        public VPBSTreeBakNew(MPBSTree mot)
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
        }

        #region 文档操作
        MDocumentCategory msrv = new MDocumentCategory();
        //文档按钮状态
        private void btnStates(int i)
        {
            if (i == 0)
            {
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

                this.txtDesc.Text = oprNode.Describe;
                FillDoc();
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

            ClearStructTypeDropDownList(state);
        }

        private void ClearStructTypeDropDownList(MainViewState state)
        {
            this.cbType.Items.Clear();
            if (state == MainViewState.AddNew || state == MainViewState.Modify)
            {
                if (currNode.Parent == null) return;
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
                    if (currNode.Parent != null)
                    {
                        MessageBox.Show("结构类型不能为空!");
                        cbType.Focus();
                        return false;
                    }
                    else
                    {
                        rootCode = oprNode.Code;
                    }
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
                        }
                    }

                    model.SavePBSTreeRootNode(listUpdate);
                    list = model.GetPBSTreesByInstance(projectInfo.Id);
                }
                else if (list.Count == 1)//只有根节点
                {
                    IList listUpdate = new ArrayList();

                    PBSTree root = list[0] as PBSTree;
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
                    // 新项目非根节点修改
                    if (projectInfo.ProjectInfoState == EnumProjectInfoState.新项目 && currNode.Nodes.Count == 0 && currNode.Parent != null)
                    {
                        txtCode.ReadOnly = false;
                    }
                    if (currNode.Parent == null)
                    {
                        txtCode.ReadOnly = false;
                        txtName.Enabled = false;
                        cbType.Enabled = false;
                        txtDesc.Enabled = false;
                    }
                    break;
                case MainViewState.Browser:
                    this.mnuTree.Items["撤销"].Enabled = false;
                    this.mnuTree.Items["保存节点"].Enabled = false;

                    if (currNode != null && currNode.Parent == null)
                    {
                        if (currNode.Nodes.Count == 2 && currNode.Nodes[0].Nodes.Count == 0 && currNode.Nodes[1].Nodes.Count == 0 && projectInfo.ProjectInfoState == EnumProjectInfoState.新项目)
                        {
                            this.mnuTree.Items["修改节点"].Enabled = true;
                            this.linkUpdate.Enabled = true;
                        }
                        else
                        {
                            this.mnuTree.Items["修改节点"].Enabled = false;
                            this.linkUpdate.Enabled = false;
                        }
                        this.mnuTree.Items["增加子节点"].Enabled = false;
                        this.mnuTree.Items["插入同级节点"].Enabled = false;
                        this.mnuTree.Items["删除节点"].Enabled = false;

                        this.linkAdd.Enabled = false;
                        this.linkDelete.Enabled = false;
                    }
                    //else if (oprNode != null && currNode.Nodes.Count != 0)
                    //{
                    //    this.mnuTree.Items["增加子节点"].Enabled = true;
                    //    this.mnuTree.Items["插入同级节点"].Enabled = false;
                    //    this.mnuTree.Items["修改节点"].Enabled = false;
                    //    this.mnuTree.Items["删除节点"].Enabled = false;

                    //    this.linkAdd.Enabled = true;
                    //    this.linkUpdate.Enabled = false;
                    //    this.linkDelete.Enabled = false;
                    //}
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
                        if (projectInfo.ProjectInfoState == EnumProjectInfoState.新项目)
                        {
                            var temp = currNode.Tag as PBSTree;
                            // 确定当前节点下子节点的最大序号
                            int max = 0;
                            if (currNode.Nodes.Count > 0)
                            {
                                List<int> list = new List<int>();
                                foreach (TreeNode item in currNode.Nodes)
                                {
                                    // 取子节点中的编码后两位，如果能转化为数字，则存储在list中
                                    var pbs = item.Tag as PBSTree;
                                    if (pbs.Code == null || pbs.Code.Length < 3) continue;
                                    var code = pbs.Code.Substring(pbs.Code.Length - 2);
                                    int number = 0;
                                    if (int.TryParse(code, out number))
                                    {
                                        list.Add(number);
                                    }
                                }
                                // 取list的最大值
                                max = list.Max();
                            }
                            oprNode.Code = temp.Code + (max + 1).ToString().PadLeft(2, '0');
                        }
                        else
                        {
                            oprNode.Code = projectInfo.Code.PadLeft(4, '0') + "-" + query.ElementAt(0).BasicCode + "-" + model.GetCode(typeof(PBSTree));
                        }
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
                    if (currNode.Parent == null)
                    {
                        // 根节点编码修改，需要修改下面子节点的编码
                        foreach (TreeNode item in currNode.Nodes)
                        {
                            var pbs = item.Tag as PBSTree;
                            if (pbs.Code == null) continue;
                            pbs.Code = pbs.Code.Replace(rootCode, oprNode.Code);
                            model.SavePBSTree(pbs);
                        }
                    }
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
    }
}
