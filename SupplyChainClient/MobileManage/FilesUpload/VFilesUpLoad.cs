using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using System.IO;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.MobileManage.OftenWords;
//using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.FilesUpload
{
    public partial class VFilesUpLoad : TBasicDataViewByMobile
    {
        private AutomaticSize automaticSize = new AutomaticSize(); 

        string fileObjectType = string.Empty;
        string FileStructureType = string.Empty;
        string userName = string.Empty;
        string jobId = string.Empty;

        string objectID = string.Empty;
        string projectID = string.Empty;
        string documentSort = string.Empty;
        string documentWorkflow = string.Empty;
        //private PBSTree oprNode = null;
        //CurrentProjectInfo projectInfo = null;
        public MPBSTree model = new MPBSTree();

        public VFilesUpLoad()
        {
            InitializeComponent();
            init();
            automaticSize.SetTag(this);
        }
        /// 对象类型  对象GUID  所属项目GUID  文档分类  文档工作流
        public VFilesUpLoad(string _objectType, string _objectID, string _projectID,string _documentSort,string _documentWorkflow)
        {
            InitializeComponent();
            automaticSize.SetTag(this);//（可以修改）
            init();
            fileObjectType = _objectType;//
            objectID = _objectID;
            projectID = _projectID;
            documentSort = _documentSort;
            documentWorkflow = _documentWorkflow;
            txtSort.Text = _documentSort;
            //txtEncode.Text = "";
            //（下面两行可以修改）
            List<string> listParams = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetUploadFileParamsByMBP_IRP();
            fileObjectType = listParams[0];//文件对象类型
        }
        void init()
        {
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
            List<string> listParams = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetUploadFileParamsByMBP_IRP();
            fileObjectType = listParams[0];//文件对象类型
            FileStructureType = listParams[1];//结构类型
        }
        //private void pnlFloor_Paint(object sender, PaintEventArgs e)
        //{

        //}

        //浏览选择文档
        string filePath = string.Empty;
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            filePath = GetfilePath();
            if (filePath != "")
            {
                txtFilesURL.Text = filePath;
                FileInfo fileInfo = new FileInfo(filePath);
                txtName.Text = fileInfo.Name.Substring(0, fileInfo.Name.LastIndexOf("."));
                //txtName.Text = Path.GetFileName(filePath);
            }
        }
        /// <summary>
        /// 得到上传文件路径
        /// </summary>
        /// <returns></returns>
        string GetfilePath()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "所有文件(*.*)|*.*";
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog();
            return openFileDialog1.FileName;
        }

        private void btnUpLoad_Click(object sender, EventArgs e)
        {
            #region 上传文件
            if (txtFilesURL.Text == "" || txtName.Text == "")
            {
                MessageBox.Show("文件路径或名字不能为空！");
                return;
            }

            List<byte[]> listFileBytes = new List<byte[]>();
            List<string> listFileNames = new List<string>();
            List<PLMWebServices.DictionaryObjectInfo> listDicKeyValue = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.DictionaryObjectInfo>();
            IList listDoc = new List<ProObjectRelaDocument>();

            //string filePath = GetfilePath();
            FileInfo file = new FileInfo(filePath);
            string fileName = file.Name.Substring(0, file.Name.LastIndexOf("."));
            FileStream fileStream = file.OpenRead();
            int FileLen = (int)file.Length;
            Byte[] FileData = new Byte[FileLen];
            //将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
            fileStream.Read(FileData, 0, FileLen);

            if (FileData.Length == 0)
            {
                MessageBox.Show("该文件长度为0,请检查!");
                return;
            }

            listFileBytes.Add(FileData);


            //string fileName = row.Cells[BrownFileName.Name].Value.ToString();
            //listFileNames.Add(fileName + Path.GetExtension(file.Name));
            listFileNames.Add(file.Name);

            object fileDesc = txtExplain.Text;

            List<string> listNames = new List<string>();
            List<object> listValues = new List<object>();

            listNames.Add("Code");
            listValues.Add(txtEncode.Text);

            //listNames.Add("DOCUMENTNUMBER");
            //listValues.Add(fileName);

            listNames.Add("Name");
            listValues.Add(txtName.Text);

            //listNames.Add("DOCUMENTTITLE");
            //listValues.Add(fileName);

            listNames.Add("DOCUMENTDESCRIPTION");
            listValues.Add(fileDesc);

            PLMWebServices.DictionaryObjectInfo dic = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.DictionaryObjectInfo();
            dic.InfoNames = listNames.ToArray();
            dic.InfoValues = listValues.ToArray();

            listDicKeyValue.Add(dic);

            //List<string> listParam= StaticMethod.GetMBPUploadFileParams();
            //string fileObjectType = listParam[0];

            string[] listFileIds = null;
            PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.AddDocumentByCustom
                (out listFileIds, listFileBytes.ToArray(),
                listFileNames.ToArray(), fileObjectType, "1", listDicKeyValue.ToArray(), null, userName, jobId, null);

            if (es != null)
            {
                MessageBox.Show("文件“" + fileName + "”上传到服务器失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                MessageBox.Show("已经上传至服务器！");
            }
            #endregion

            #region 保存MBP对象关联文档信息
            if (listFileIds != null)
            {
                listDoc.Clear();

                //for (int i = 0; i < listFileIds.Length; i++)
                //{
                string fileId = listFileIds[0];
                ProObjectRelaDocument doc = new ProObjectRelaDocument();
                doc.DocumentGUID = fileId;

                doc.ProObjectName = fileObjectType;
                doc.ProObjectGUID = objectID;
                doc.DocumentOwnerName = ConstObject.LoginPersonInfo.Name;

                doc.DocumentName = txtName.Text;
                doc.DocumentCateCode = txtSort.Text;
                object desc = txtExplain.Text;
                doc.DocumentDesc = desc == null ? "" : desc.ToString();
                doc.FileURL = txtFilesURL.Text;
                //doc.FileURL = getFileURL(file);//使用WebService方式下载，此处不存文件路径，其次文件柜可能变迁
                //doc.
                doc.DocumentOwner = ConstObject.LoginPersonInfo;
                doc.TheProjectGUID = projectID;
                //doc.ProObjectGUID = oprNode.Id;
                //doc.ProObjectName = oprNode.GetType().Name;
                //if (projectInfo != null)
                //{
                //    doc.TheProjectGUID = projectInfo.Id;
                //    doc.TheProjectName = projectInfo.Name;
                //}

                listDoc.Add(doc);
                //}

                model.SaveOrUpdate(listDoc);
                //model.SaveOrUpdate(
                //listDoc = model.SaveOrUpdate(listDoc);

                //foreach (ProObjectRelaDocument doc in listDoc)
                //{
                //    InsertIntoGridDocument(doc);
                //}
            }
            #endregion 保存MBP对象关联文档信息

        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            //txtFilesURL.Text = "";
            //txtName.Text = "";
            //txtEncode.Text = "";
            //txtExplain.Text = "";
            //txtSort.Text = "";
            this.Close();
        }


        private string GetExceptionMessage(PLMWebServices.ErrorStack es)
        {
            string msg = es.Message;
            PLMWebServices.ErrorStack esTemp = es.InnerErrorStack;
            while (esTemp != null && !string.IsNullOrEmpty(esTemp.Message))
            {
                msg += "；\n" + esTemp.Message;
                esTemp = esTemp.InnerErrorStack;
            }

            if (msg.IndexOf("不能在具有唯一索引") > -1 && msg.IndexOf("插入重复键") > -1)
            {
                msg = "已存在同名文档，请重命名该文档名称.";
            }

            return msg;
        }

        private void txtExplain_DoubleClick(object sender, EventArgs e)
        {
            string userID = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            string interphaseID =this.Name ;//txtInterphaseID.Text;
            string controlID = "txtExplain";//txtControlID.Text;
            string oftenWord = string.Empty;
            VOftenWords vow = new VOftenWords(userID, interphaseID, controlID, oftenWord);
            vow.ShowDialog();
            string result = vow.Result;
            if (result != null)
            {
                txtExplain.Text += vow.Result;
            }
        }
    }
}
