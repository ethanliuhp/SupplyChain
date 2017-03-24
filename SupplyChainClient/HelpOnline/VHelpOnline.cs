using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using AuthManagerLib.AuthMng.MenusMng.Domain;
using VirtualMachine.Core;
using AuthManager.AuthMng.AuthConfigMng;
using VirtualMachine.Component.Util;
using System.Collections;
using NHibernate.Criterion;
using AuthManagerLib.AuthMng.AuthConfigMng.Domain;
using IRPServiceModel.Domain.Document;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.HelpOnlineManage.Domain;
using Application.Business.Erp.SupplyChain.Client.FileUpload;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using System.IO;
using IRPServiceModel.Basic;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.HelpOnline
{
    public partial class VHelpOnline : TBasicDataView
    {
        private HelpOnlineMng hOMng;
        public bool isDistribute = true ;
        /// <summary>
        /// 设置是否是分层加载
        /// </summary>
        public bool IsDistribute
        {
            get { return isDistribute; }
            set { isDistribute = value; }
        }
        public HelpOnlineMng HOMng
        {
            get { return hOMng; }
            set { hOMng = value; }
        }
        private List<TreeNode> listFindNodes = new List<TreeNode>();
        private int showFindNodeIndex = 0;
        private MHelpOnline model = new MHelpOnline();
        private IList cachedMenuList = new ArrayList();
        private SortedList lstNode = new SortedList();
        private TreeNode mouseSelectNode = null;
        private SortedList lstMenuNode = new SortedList();
        private MAuthConfig MAuthConfig = new MAuthConfig();
        public VHelpOnline()
        {
            InitializeComponent();
            if (IsDistribute)
            {
                InitEvent();
                InitData();
                
            }
            else
            {
                InitData();
                InitEvent();
            }
        }
        public void InitData()
        {
            if (IsDistribute)
            {
                InitMenusNew();
            }
            else
            {
                InitMenus();
            }
            if (tvwCategory.Nodes.Count > 0)
                tvwCategory.SelectedNode = tvwCategory.Nodes[0];
        }
        private void InitEvent()
        {
            this.tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);
            this.btnFindTaskNode.Click += new EventHandler(btnFindTaskNode_Click);
            this.btnSave.Click += new EventHandler(btnSave_Click);

            this.txtKeyWord.TextChanged += new EventHandler(txtKeyWord_TextChanged);
            txtKeyWord.KeyDown += new KeyEventHandler(txtKeyWord_KeyDown);
            //相关文档
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
            if (IsDistribute)
            {
                tvwCategory.AfterExpand +=new TreeViewEventHandler(tvwCategory_AfterExpand);
            }

        }
        void txtKeyWord_TextChanged(object sender, EventArgs e)
        {
            listFindNodes.Clear();
            showFindNodeIndex = 0;
        }
        void txtKeyWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyData == Keys.Enter)
            {
                btnFindTaskNode_Click(btnFindTaskNode, new EventArgs());
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
        }
        //加载文档数据
        void FillDoc()
        {
            dgDocumentMast.Rows.Clear();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", hOMng.Id));
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

            string id = (this.tvwCategory.SelectedNode.Tag as Menus).Id;

            if (id != null)
            {
                VDocumentPublicUpload frm = new VDocumentPublicUpload(id);
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
        /// <summary>
        /// 加载功能树
        /// </summary>
        private void InitMenus()
        {
            lstMenuNode.Clear();
            this.tvwCategory.Nodes.Clear();
            TreeNode RootNode = new TreeNode();
            RootNode.Name = "Root";
            RootNode.Text = "功能菜单";
            RootNode.ImageKey = "Menu";
            RootNode.SelectedImageKey = "Select";
            this.tvwCategory.Nodes.Add(RootNode);

            lstMenuNode.Add(RootNode.Name, RootNode);

            ObjectQuery oq = new ObjectQuery();
            try
            {
                cachedMenuList = MAuthConfig.AuthConfigSrv.GetMenus(oq);
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载菜单出错。\n" + ExceptionUtil.ExceptionMessage(ex));
                return;
            }

            foreach (Menus tmpMenus in cachedMenuList)
            {
                TreeNode parentNode = null;
                if (tmpMenus.Parent == null)
                {
                    parentNode = lstMenuNode["Root"] as TreeNode;
                }
                else
                {
                    parentNode = lstMenuNode["Menu" + tmpMenus.Parent.Id] as TreeNode;
                }
                TreeNode NowNode = new TreeNode();
                if (tmpMenus.Code == "" || tmpMenus.Code == null)
                {
                    NowNode.Text = tmpMenus.Name;
                }
                else
                {
                    NowNode.Text = "[" + tmpMenus.Code + "]" + tmpMenus.Name;
                }
                NowNode.Name = "Menu" + tmpMenus.Id;
                if (tmpMenus.MenusKind == MenusKindEnm.Menu)
                {
                    if (!tmpMenus.IsValide)
                    {
                        NowNode.ForeColor = Color.Red;
                    }
                    NowNode.ImageKey = "Menu";
                    NowNode.SelectedImageKey = "Select";
                }
                else
                {
                    if (!tmpMenus.IsValide)
                    {
                        NowNode.ForeColor = Color.Red;
                    }
                    NowNode.ImageKey = "Button";
                    NowNode.SelectedImageKey = "Select";
                }

                NowNode.Tag = tmpMenus;

                parentNode.Nodes.Add(NowNode);
                lstMenuNode.Add(NowNode.Name, NowNode);

                //DBAuth
                if (tmpMenus.DataAuth.Count > 0)
                {
                    foreach (MenuDataAuth tmpMenuDataAuth in tmpMenus.DataAuth)
                    {
                        TreeNode tnDBAuth = new TreeNode();
                        tnDBAuth.Text = tmpMenuDataAuth.KindFlag;
                        tnDBAuth.Name = "DB" + tmpMenuDataAuth.Id.ToString() + "";

                        tnDBAuth.ImageKey = "DB";
                        tnDBAuth.SelectedImageKey = "Select";

                        tnDBAuth.Tag = tmpMenuDataAuth;
                        NowNode.Nodes.Add(tnDBAuth);
                    }
                }
            }
            this.tvwCategory.SelectedNode = RootNode;
            RootNode.Expand();
        }
        /// <summary>
        /// 查找/下一个
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ViewToModel();
                   hOMng = model.saveImp(hOMng);
                MessageBox.Show("保存成功！");
            }
            catch (Exception ev)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(ev));
            }
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        void ViewToModel()
        {
            try
            {
                Menus mda = tvwCategory.SelectedNode.Tag as Menus;
                hOMng.MenuName = mda.Name;
                hOMng.MenuId = mda.Id;
                hOMng.MenuDescript = ClientUtil.ToString(this.txtMenuDescript.Text);
                hOMng.CreatePersonName = ClientUtil.ToString(ConstObject.LoginPersonInfo.Name);
                hOMng.CreatePerson = ConstObject.LoginPersonInfo;
               if( hOMng.CreateDate.ToShortDateString()=="1990-01-01")
                {
                    hOMng.CreateDate = ClientUtil.ToDateTime(DateTime.Now);
                }
                hOMng.LastUpdateDate = ClientUtil.ToDateTime(DateTime.Now);
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// 选中节点 显示数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            dgDocumentMast.Rows.Clear();
            dgDocumentDetail.Rows.Clear();
            Menus mda = tvwCategory.SelectedNode.Tag as Menus;
            if (mda == null) return;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("MenuId", mda.Id));
            IList listMenu = model.IHelpOnlineSrv.ObjectQuery(typeof(HelpOnlineMng), oq);
            if (listMenu != null && listMenu.Count > 0)
            {
                foreach (HelpOnlineMng ho in listMenu)
                {
                    hOMng = ho;
                    hOMng.Id = ho.Id;
                    this.txtMenuDescript.Text = ho.MenuDescript;
                    FillDoc(mda.Id);    
                }
            }
            else
            {
                hOMng = new HelpOnlineMng();
                this.txtMenuDescript.Text = "";
            }
        }
        //加载文档数据
        void FillDoc(string id)
        {
            dgDocumentMast.Rows.Clear();
            dgDocumentDetail.Rows.Clear();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", id));
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


        #region 分层加载方法
        private void InitMenusNew()
        {
            this.tvwCategory.AfterExpand -= new TreeViewEventHandler(tvwCategory_AfterExpand);
            this.tvwCategory.AfterExpand += new TreeViewEventHandler(tvwCategory_AfterExpand);

            this.tvwCategory.Nodes.Clear();
          
            
                TreeNode RootNode = new TreeNode();
                TreeNode tempNode = new TreeNode();
                tempNode.Text = "test";
                tempNode.Name = "test";
                RootNode.Nodes.Add(tempNode);
                RootNode.Name = "Root";
                    RootNode.Text = "功能菜单";
            

                RootNode.ImageKey = "Menu";
                RootNode.SelectedImageKey = "Menu";

                this.tvwCategory.Nodes.Add(RootNode);

              

                this.tvwCategory.SelectedNode = RootNode;

                RootNode.Expand();

             
        }
        void tvwCategory_AfterExpand(object sender, TreeViewEventArgs e)
        {
            Menus oMenus = null;
            string sID = string.Empty;
            int iLevel = 0;
            DataTable tbMenu = null;

            if (e.Node != null)
            {
                if (e.Node.Nodes != null && e.Node.Nodes.Count > 0)
                {
                    oMenus = e.Node.Nodes[0].Tag as Menus;
                    if (oMenus != null)
                    {
                    }
                    else
                    {
                        e.Node.Nodes.Clear();
                        oMenus = e.Node.Tag as Menus;
                        if (oMenus != null)
                        {
                            sID = oMenus.Id;
                            iLevel = (int)oMenus.Level + 1;
                        }
                        else
                        {
                            sID = string.Empty;
                            iLevel = 1;
                        }

                        tbMenu = MAuthConfig.AuthConfigSrv.GetAuthConfig("", "", sID, iLevel, 0);
                        AddNode(e.Node, tbMenu);
                    }
                }

            }
        }


        public void AddNode(TreeNode oParentNode, DataTable tbChildNode)
        {
            string sCode = string.Empty;
            string sName = string.Empty;
            string sPicPath = string.Empty;
            string sID = string.Empty;
            int iLevel = 0;
            int iChildCount = 0;
            TreeNode NowNode = null;
            TreeNode tempNode = null;
            Menus menus = null;
            if (oParentNode != null)
            {
                if (tbChildNode != null && tbChildNode.Rows.Count > 0)
                {
                    foreach (DataRow oRow in tbChildNode.Rows) 
                    {
                        sCode = oRow["code"].ToString();
                        sName = oRow["NAME"].ToString();
                        sPicPath = oRow["PICPATH"].ToString();
                        sID = oRow["ID"].ToString();
                        iChildCount = int.Parse(oRow["childcount"].ToString());
                        iLevel = int.Parse(oRow["tlevel"].ToString());
                        NowNode = new TreeNode();
                        NowNode.Text = sName;
                        menus = new Menus();
                        menus.Id = sID;
                        menus.Code = sCode;
                        menus.PicPath = sPicPath;
                        menus.Name = sName;
                        menus.Level = iLevel;
                        menus.Version = 0;
                        NowNode.Tag = menus;
                        NowNode.Name = "Menu" + sID;




                        if (string.IsNullOrEmpty (menus.Code) )
                        {
                            NowNode.Text = menus.Name;
                        }
                        else
                        {
                            NowNode.Text = "[" + menus.Code + "]" + menus.Name;
                        }
                        NowNode.Name = "Menu" + menus.Id;
                        if (menus.MenusKind == MenusKindEnm.Menu)
                        {
                            if (!menus.IsValide)
                            {
                                NowNode.ForeColor = Color.Red;
                            }
                            NowNode.ImageKey = "Menu";
                            NowNode.SelectedImageKey = "Select";
                        }
                        else
                        {
                            if (!menus.IsValide)
                            {
                                NowNode.ForeColor = Color.Red;
                            }
                            NowNode.ImageKey = "Button";
                            NowNode.SelectedImageKey = "Select";
                        }

                        NowNode.Tag = menus;

                        oParentNode.Nodes.Add(NowNode);

                         if (iChildCount > 0)//有子节点
                        {
                            tempNode = new TreeNode("temp");
                            NowNode.Nodes.Add(tempNode);
                        }
                        
                      //  lstMenuNode.Add(NowNode.Name, NowNode);

                        ////DBAuth
                        //if (tmpMenus.DataAuth.Count > 0)
                        //{
                        //    foreach (MenuDataAuth tmpMenuDataAuth in tmpMenus.DataAuth)
                        //    {
                        //        TreeNode tnDBAuth = new TreeNode();
                        //        tnDBAuth.Text = tmpMenuDataAuth.KindFlag;
                        //        tnDBAuth.Name = "DB" + tmpMenuDataAuth.Id.ToString() + "";

                        //        tnDBAuth.ImageKey = "DB";
                        //        tnDBAuth.SelectedImageKey = "Select";

                        //        tnDBAuth.Tag = tmpMenuDataAuth;
                        //        NowNode.Nodes.Add(tnDBAuth);
                        //    }
                        //}
                    }
                    //this.MenuView.SelectedNode = RootNode;
                    //RootNode.Expand();
                   
                }

            }
        }
        #endregion
    }
}
