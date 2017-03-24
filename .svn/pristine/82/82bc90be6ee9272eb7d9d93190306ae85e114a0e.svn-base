using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
//测试
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using System.IO;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using IRPServiceModel.Domain.Document;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.Util;
using ImportIntegration;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng
{
    public partial class VDocumentMasterInfoAdd : TBasicDataView
    {

        public MDocumentCategory model = new MDocumentCategory();

        //所属项目
        public CurrentProjectInfo projectInfo = null;
        public PersonInfo person = null;
        private DocumentMaster master = null;
        private DocumentCategory cate = null;
        private DocumentMaster result;

        public DocumentMaster Result
        {
            get { return result; }
            set { result = value; }
        }
        public VDocumentMasterInfoAdd()
        {
            InitializeComponent();
        }

        public VDocumentMasterInfoAdd(DocumentMaster m, DocumentCategory c)
        {
            InitializeComponent();
            master = m;
            cate = c;
            InitEvent();
            InitData();
            InitcomboBoxData();
            if (m != null)
            {
                InitUpdateData();
            }
        }
        void InitcomboBoxData()
        {
            //文档信息类型
            foreach (string infoType in Enum.GetNames(typeof(DocumentInfoTypeEnum)))
            {
                cmbDocumentInforType.Items.Add(infoType);
            }
            //文档状态
            foreach (DocumentState state in Enum.GetValues(typeof(DocumentState)))
            {
                cmbDocumentStatus.Items.Add(ClientUtil.GetDocStateName(state));
            }
            this.cmbDocumentInforType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbDocumentStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDocumentInforType.SelectedIndex = 0;
            cmbDocumentStatus.SelectedIndex = 0;
        }

        void InitData()
        {
            object[] os = new object[] { txtResideProject };
            ObjectLock.Lock(os);

            //归属项目
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            if (projectInfo != null)
            {
                txtResideProject.Tag = projectInfo;
                txtResideProject.Text = projectInfo.Name;
            }
            person = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;
        }

        void InitUpdateData()
        {
            txtDocumentName.Text = master.Name;
            txtDocumentAuthor.Text = master.Author;
            txtDocumentCode.Text = master.Code;
            txtDocumentKeywords.Text = master.KeyWords;
            txtDocumentExplain.Text = master.Description;
            txtDocumentTitle.Text = master.Title;
            cmbDocumentInforType.Text = master.DocType.ToString();
            cmbDocumentStatus.Text = ClientUtil.GetDocStateName(master.State);
        }

        void InitEvent()
        {
            btnSave.Click += new EventHandler(btnSave_Click);
            btnQuit.Click += new EventHandler(btnQuit_Click);
            #region 文件
            btnSelectFile.Click += new EventHandler(btnSelectFile_Click);
            btnClearSelected.Click += new EventHandler(btnClearSelected_Click);
            btnClearAll.Click += new EventHandler(btnClearAll_Click);
            #endregion
        }

        //保存
        void btnSave_Click(object sender, EventArgs e)
        {
            if (!Verify()) return;

            DocumentInfoTypeEnum docInfoType = 0;
            foreach (DocumentInfoTypeEnum type in Enum.GetValues(typeof(DocumentInfoTypeEnum)))
            {
                if (type.ToString() == cmbDocumentInforType.Text.Trim())
                {
                    docInfoType = type;
                    break;
                }
            }
            DocumentState docState = 0;
            foreach (DocumentState state in Enum.GetValues(typeof(DocumentState)))
            {
                if (ClientUtil.GetDocStateName(state) == cmbDocumentStatus.Text.Trim())
                {
                    docState = state;
                    break;
                }
            }

            try
            {
                #region 文档（主表）
                if (master == null)
                {
                    master = new DocumentMaster();
                    if (cate != null)
                    {
                        master.Category = cate;
                        master.CategoryCode = cate.Code;
                        master.CategoryName = cate.Name;
                        master.CategorySysCode = cate.SysCode;
                    }
                    master.ProjectCode = projectInfo.Code;
                    master.ProjectId = projectInfo.Id;
                    master.ProjectName = projectInfo.Name;

                    master.CreateTime = DateTime.Now;
                }

                master.Author = txtDocumentAuthor.Text;

                master.OwnerID = person;
                master.OwnerName = person.Name;
                master.OwnerOrgSysCode = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.SysCode;


                //master.CheckoutState = DocumentCheckOutStateEnum.
                master.Code = txtDocumentCode.Text;
                master.Description = txtDocumentExplain.Text;
                master.DocType = docInfoType;
                //master.IsInspectionLot
                master.KeyWords = txtDocumentKeywords.Text;
                master.Name = txtDocumentName.Text;
                //master.NGUID
                //master.Revision = 
                //master.SecurityLevel = 
                //master.SetNewRevision
                //master.SetNewVersion
                master.State = docState;
                master.Title = txtDocumentTitle.Text;
                master.UpdateTime = DateTime.Now;
                //master.Version
                //master.VersionMajor
                #endregion

                #region 文件（明细）

                FileCabinet appFileCabinet = null;
                appFileCabinet = StaticMethod.GetDefaultFileCabinet();

                foreach (DataGridViewRow row in gridFiles.Rows)
                {
                    DocumentDetail dtl = new DocumentDetail();
                    dtl.Master = master;
                    master.ListFiles.Add(dtl);

                    string filePath = row.Cells[FilePath.Name].Value.ToString();
                    FileInfo file = new FileInfo(filePath);
                    if (file.Exists)
                    {
                        FileStream fileStream = file.OpenRead();
                        int FileLen = (int)file.Length;
                        Byte[] FileData = new Byte[FileLen];
                        //将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
                        fileStream.Read(FileData, 0, FileLen);

                        dtl.FileDataByte = FileData;
                    }

                    dtl.ExtendName = Path.GetExtension(filePath);
                    dtl.FileName = Path.GetFileName(filePath);
                    dtl.TheFileCabinet = appFileCabinet;
                }
                result = model.SaveDocumentAndFile(master);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            this.Close();
        }

        //保存前验证
        bool Verify()
        {
            return true;
        }

        // 放弃
        void btnQuit_Click(object sender, EventArgs e)
        {
            result = null;
            this.Close();
        }

        #region 文件
        private void btnSelectFile_Click(object sender, EventArgs e)
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
                    InsertToGrid(strFiles[i]);
                }

                gridFiles.AutoResizeColumns();
            }
        }
        private const double KBCount = 1024;
        private const double MBCount = KBCount * 1024;
        private const double GBCount = MBCount * 1024;
        private const double TBCount = GBCount * 1024;

        /// <summary>
        /// 得到适应的大小
        /// </summary>
        /// <param name="path"></param>
        /// <returns>string</returns>
        public static string GetAutoSizeString(double size, int roundCount)
        {
            if (KBCount > size)
            {
                return Math.Round(size, roundCount) + "B";
            }
            else if (MBCount > size)
            {
                return Math.Round(size / KBCount, roundCount) + "KB";
            }
            else if (GBCount > size)
            {
                return Math.Round(size / MBCount, roundCount) + "MB";
            }
            else if (TBCount > size)
            {
                return Math.Round(size / GBCount, roundCount) + "GB";
            }
            else
            {
                return Math.Round(size / TBCount, roundCount) + "TB";
            }
        }

        private void InsertToGrid(string filePath)
        {
            int index = gridFiles.Rows.Add();
            DataGridViewRow row = gridFiles.Rows[index];
            row.Cells[FileName.Name].Value = Path.GetFileName(filePath);
            row.Cells[FilePath.Name].Value = filePath;

            FileInfo fileInfo = new FileInfo(filePath);
            row.Cells[FileSize.Name].Value = GetAutoSizeString(fileInfo.Length, 3);
        }

        #region 变量

        private DataTable sourceDataTable = new DataTable();
        private IntergrationFrameWork cFuntions = null;
        private string language = "zhs";

        #endregion

        private void btnBatchUpload_Click(object sender, EventArgs e)
        {
            CurrentProjectInfo project = StaticMethod.GetProjectInfo();

            FileCabinet appFileCabinet = null;

            appFileCabinet = StaticMethod.GetDefaultFileCabinet();
            List<DocumentMaster> listDoc = new List<DocumentMaster>();

            //DocumentMaster master = new DocumentMaster();
            //master.ProjectId = project.Id;
            //master.ProjectCode = project.Code;
            //master.ProjectName = project.Name;

            //ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("CategoryNodeType", VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.RootNode));
            //IList list = model.ObjectQuery(typeof(DocumentCategory), oq);
            //DocumentCategory cate = list[0] as DocumentCategory;

            //master.Category = cate;
            //master.CategoryCode = cate.Code;
            //master.CategoryName = cate.Name;
            //master.CategorySysCode = cate.SysCode;

            //master.DocType = DocumentInfoTypeEnum.普通文档;
            //master.Name = "测试文档";

            //listDoc.Add(master);

            foreach (DataGridViewRow row in gridFiles.Rows)
            {
                DocumentDetail dtl = new DocumentDetail();
                dtl.Master = master;
                master.ListFiles.Add(dtl);

                string filePath = row.Cells[FilePath.Name].Value.ToString();
                FileInfo file = new FileInfo(filePath);
                if (file.Exists)
                {
                    FileStream fileStream = file.OpenRead();
                    int FileLen = (int)file.Length;
                    Byte[] FileData = new Byte[FileLen];
                    //将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
                    fileStream.Read(FileData, 0, FileLen);

                    dtl.FileDataByte = FileData;
                }

                dtl.ExtendName = Path.GetExtension(filePath);
                dtl.FileName = Path.GetFileName(filePath);
                dtl.TheFileCabinet = appFileCabinet;
            }

            listDoc = model.AddDocumentByCustomExtend(listDoc);
        }

        private void btnClearSelected_Click(object sender, EventArgs e)
        {
            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridFiles.SelectedRows)
            {
                listRowIndex.Add(row.Index);
            }
            listRowIndex.Sort();
            for (int i = listRowIndex.Count - 1; i > -1; i--)
            {
                gridFiles.Rows.RemoveAt(listRowIndex[i]);
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            for (int i = gridFiles.Rows.Count - 1; i > -1; i--)
            {
                gridFiles.Rows.RemoveAt(i);
            }
        }
        #endregion
    }
}
