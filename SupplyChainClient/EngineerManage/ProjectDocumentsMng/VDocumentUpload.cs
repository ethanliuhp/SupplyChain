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
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng
{
    public partial class VDocumentUpload : TBasicDataView
    {
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();
        string filePath = string.Empty;
        string userName = string.Empty;
        string jobId = string.Empty;
        string isIRPOrKB = string.Empty;
        //string fileObjectType = string.Empty;
        public MPBSTree model = new MPBSTree();

        IList upNewVersion = null;
        //操作的项目
        public CurrentProjectInfo OptProjectInfo = null;

        //工程对象名称 GUID
        string projectObjectName = string.Empty;
        string projectObjectGUID = string.Empty;

        //private PLMWebServices.ProjectDocument resultDocument;

        //public PLMWebServices.ProjectDocument ResultDocument
        //{
        //    get { return resultDocument; }
        //    set { resultDocument = value; }
        //}

        private IList resultListDoc;
        public IList ResultListDoc
        {
            get { return resultListDoc; }
            set { resultListDoc = value; }
        }

        public VDocumentUpload()
        {
            InitializeComponent();
            InitEvent();
            InitData();
            InitcomboBoxData();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectName">项目名称</param>
        /// <param name="projectGUID">项目GUID</param>
        /// <param name="poName">工程对象名称</param>
        /// <param name="poGUID">工程对象GUID</param>
        public VDocumentUpload(CurrentProjectInfo projectInfo, string proObjName, string proObjGUID)
        {
            InitializeComponent();

            OptProjectInfo = projectInfo;
            projectObjectName = proObjName;
            projectObjectGUID = proObjGUID;

            InitEvent();
            InitData();
            InitcomboBoxData();

            txtResideProject.Text = OptProjectInfo.Name;
        }

        public VDocumentUpload(IList newVersionList, string irpOrKb)
        {
            InitializeComponent();
            upNewVersion = newVersionList;
            isIRPOrKB = irpOrKb;
            InitEvent();
            InitData();
            InitcomboBoxData();
            if (upNewVersion.Count > 0 && upNewVersion != null)
            {
                object[] os = new Object[]{txtDocumentAuthor,txtDocumentCode,txtDocumentExplain,txtDocumentKeywords,txtDocumentObjectType,
                txtDocumentSortEncode,txtDocumentSortName,txtDocumentTitle,txtResideProject};
                cmbDocumentInforType.Enabled = false;
                btnSearchSort.Enabled = false;
                ObjectLock.Lock(os);
                if (isIRPOrKB != "KB")
                {
                    PLMWebServices.ProjectDocument doc = upNewVersion[0] as PLMWebServices.ProjectDocument;
                    HandleNewVision(doc);
                }
                else
                {
                    PLMWebServicesByKB.ProjectDocument doc = upNewVersion[0] as PLMWebServicesByKB.ProjectDocument;
                    HandleNewVision(doc);
                }
            }
        }
        void InitcomboBoxData()
        {
            //文档信息类型
            foreach (string infoType in Enum.GetNames(typeof(PLMWebServices.DocumentInfoType)))
            {
                cmbDocumentInforType.Items.Add(infoType);
            }
            //文档状态
            foreach (string infoType in Enum.GetNames(typeof(PLMWebServices.DocumentState)))
            {
                cmbDocumentStatus.Items.Add(infoType);
            }
            this.cmbDocumentInforType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbDocumentStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDocumentInforType.SelectedIndex = 0;
            cmbDocumentStatus.SelectedIndex = 0;
        }
        void InitEvent()
        {
            this.btnSearchSort.Click += new EventHandler(btnSearchSort_Click);
            this.btnQuit.Click += new EventHandler(btnQuit_Click);
            this.btnUploadFile.Click += new EventHandler(btnUploadFile_Click);
            this.btnBrowse.Click += new EventHandler(btnBrowse_Click);
        }
        void InitData()
        {
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;

            object[] os = new object[] { txtResideProject, txtDocumentObjectType, txtDocumentSortName, txtDocumentSortEncode };
            ObjectLock.Lock(os);

            //归属项目
            //projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            //if (projectInfo != null)
            //{
            //    txtResideProject.Tag = projectInfo;
            //    txtResideProject.Text = projectInfo.Name;
            //}
            //txtDocumentAuthor.Text = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
            //txtDocumentAuthor.Tag = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;
        }

        void HandleNewVision(PLMWebServices.ProjectDocument upDoc)
        {
            txtResideProject.Text = upDoc.ProjectName;
            txtDocumentName.Text = upDoc.Name;
            txtDocumentAuthor.Text = upDoc.Author;
            txtDocumentCode.Text = upDoc.Code;
            txtDocumentExplain.Text = upDoc.Description;
            txtDocumentKeywords.Text = upDoc.KeyWords;
            txtDocumentObjectType.Text = upDoc.ObjectTypeName;
            if (upDoc.Category != null)
            {
                txtDocumentSortEncode.Text = upDoc.Category.CategoryCode;
                txtDocumentSortName.Text = upDoc.Category.CategoryName;
            }
            txtDocumentTitle.Text = upDoc.Title;
            cmbDocumentInforType.Text = upDoc.DocType.ToString();
            cmbDocumentStatus.Text = upDoc.State.ToString();
        }
        void HandleNewVision(PLMWebServicesByKB.ProjectDocument upDoc)
        {
            txtResideProject.Text = upDoc.ProjectName;
            txtDocumentName.Text = upDoc.Name;
            txtDocumentAuthor.Text = upDoc.Author;
            txtDocumentCode.Text = upDoc.Code;
            txtDocumentExplain.Text = upDoc.Description;
            txtDocumentKeywords.Text = upDoc.KeyWords;
            txtDocumentObjectType.Text = upDoc.ObjectTypeName;
            if (upDoc.Category != null)
            {
                txtDocumentSortEncode.Text = upDoc.Category.CategoryCode;
                txtDocumentSortName.Text = upDoc.Category.CategoryName;
            }
            txtDocumentTitle.Text = upDoc.Title;
            cmbDocumentInforType.Text = upDoc.DocType.ToString();
            cmbDocumentStatus.Text = upDoc.State.ToString();
        }

        /// <summary>
        /// 文档分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearchSort_Click(object sender, EventArgs e)
        {
            if (OptProjectInfo.Code != "KB")
            {
                VDocumentSortSelectByIRP vdss = new VDocumentSortSelectByIRP();
                vdss.ShowDialog();
                PLMWebServices.CategoryNode cate = vdss.ResultCate;
                if (cate != null)
                {
                    txtDocumentSortEncode.Text = cate.CategoryCode;
                    txtDocumentSortEncode.Tag = cate;
                    txtDocumentSortName.Text = cate.CategoryName;
                    txtDocumentSortName.Tag = cate;
                }
            }
            else
            {
                VDocumentSortSelect vdss = new VDocumentSortSelect();
                vdss.ShowDialog();
                PLMWebServicesByKB.CategoryNode cate = vdss.ResultCate;
                if (cate != null)
                {
                    txtDocumentSortEncode.Text = cate.CategoryCode;
                    txtDocumentSortEncode.Tag = cate;
                    txtDocumentSortName.Text = cate.CategoryName;
                    txtDocumentSortName.Tag = cate;
                }
            }
        }

        /// <summary>
        /// 上传文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnUploadFile_Click(object sender, EventArgs e)
        {

            if (!Verify())
            {
                return;
            }
            else
            {
                if (upNewVersion == null)
                {
                    #region 上传新文档
                    if (OptProjectInfo.Code != "KB")
                    {
                        #region
                        IList listDoc = new List<ProObjectRelaDocument>();

                        PLMWebServices.DocumentInfoType docInfoType = 0;
                        foreach (PLMWebServices.DocumentInfoType type in Enum.GetValues(typeof(PLMWebServices.DocumentInfoType)))
                        {
                            if (type.ToString() == cmbDocumentInforType.Text.Trim())
                            {
                                docInfoType = type;
                                break;
                            }
                        }
                        PLMWebServices.DocumentState docState = 0;
                        foreach (PLMWebServices.DocumentState state in Enum.GetValues(typeof(PLMWebServices.DocumentState)))
                        {
                            if (state.ToString() == cmbDocumentStatus.Text.Trim())
                            {
                                docState = state;
                                break;
                            }
                        }

                        List<PLMWebServices.ProjectDocument> listDocument = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument>();

                        PLMWebServices.ProjectDocument doc = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument();

                        FileInfo file = new FileInfo(filePath);
                        FileStream fileStream = file.OpenRead();
                        int FileLen = (int)file.Length;
                        Byte[] FileData = new Byte[FileLen];
                        ////将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
                        fileStream.Read(FileData, 0, FileLen);
                        if (FileData.Length == 0)
                        {
                            MessageBox.Show("该文件长度为0,请检查!");
                            return;
                        }
                        doc.ExtendName = Path.GetExtension(filePath); //文档扩展名*******************************
                        doc.FileDataByte = FileData; //文件二进制流
                        doc.FileName = file.Name;//文件名称

                        doc.ProjectCode = OptProjectInfo.Code; //所属项目代码*
                        doc.ProjectName = txtResideProject.Text; //所属项目名称*

                        doc.Author = txtDocumentAuthor.Text;//文档作者*

                        doc.Category = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.CategoryNode();
                        doc.Category.CategoryCode = txtDocumentSortEncode.Text;//"CSFL";//文档分类代码
                        doc.Category.CategoryName = txtDocumentSortName.Text;//"测试分类"; //文档分类名称

                        List<string> listDocParam = StaticMethod.GetUploadFileParamsByMBP_IRP();
                        string docObjTypeName = listDocParam[0];
                        string docCateLinkTypeName = listDocParam[2];

                        doc.CategoryRelaDocType = docCateLinkTypeName;//文档分类类型
                        //doc.CategorySysCode = "";//文档分类层次码
                        doc.Code = txtDocumentCode.Text;//文档代码
                        doc.Description = txtDocumentExplain.Text;//文档说明
                        doc.DocType = docInfoType;//文档信息类型
                        doc.KeyWords = txtDocumentKeywords.Text;//文档关键字
                        doc.Name = txtDocumentName.Text;//文档名称
                        //string docObjTypeName= StaticMethod.GetMBPUploadFileParams()[0];
                        doc.ObjectTypeName = docObjTypeName;//文档对象类型
                        //doc.OwnerID = "";//责任人
                        //doc.OwnerName = "";//责任人名称
                        //doc.OwnerOrgSysCode = "";// 责任人组织层次码
                        //doc.Revision = "";//文档版次
                        doc.State = docState;//文档状态
                        doc.Title = txtDocumentTitle.Text;//文档标题
                        //doc.Version = "";//文档版本
                        //doc.ExtendInfoNames = "";//扩展属性名
                        //doc.ExtendInfoValues = "";//扩展属性值

                        listDocument.Add(doc);
                        PLMWebServices.ProjectDocument[] resultList = null;
                        PLMWebServices.ErrorStack es = StaticMethod.AddDocumentByIRP(listDocument.ToArray(), Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentSaveMode.一个文件生成一个文档对象, null, userName, jobId, null, out resultList);
                        fileStream.Close();
                        if (es != null)
                        {
                            MessageBox.Show("文件“" + file.Name + "”上传到服务器失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("上传成功！");
                            Empty();
                        }
                        #endregion
                        #region 保存MBP对象关联文档信息
                        if (resultList != null)
                        {
                            listDoc.Clear();
                            string fileId = resultList[0].ID;
                            ProObjectRelaDocument rdoc = new ProObjectRelaDocument();

                            rdoc.TheProjectGUID = OptProjectInfo.Id;
                            rdoc.TheProjectName = OptProjectInfo.Name;
                            rdoc.TheProjectCode = OptProjectInfo.Code;

                            if (projectObjectName != null && projectObjectName != "")
                            {
                                rdoc.ProObjectName = projectObjectName;
                                rdoc.ProObjectGUID = projectObjectGUID;
                            }
                            else
                            {
                                rdoc.ProObjectName = OptProjectInfo.GetType().Name;
                                rdoc.ProObjectGUID = OptProjectInfo.Id;
                            }

                            rdoc.DocumentOwner = ConstObject.LoginPersonInfo;
                            rdoc.DocumentOwnerName = ConstObject.LoginPersonInfo.Name;
                            rdoc.DocumentGUID = resultList[0].EntityID;
                            rdoc.DocumentName = resultList[0].Name;
                            rdoc.DocumentDesc = resultList[0].Description;
                            rdoc.DocumentCode = resultList[0].Code;
                            rdoc.SubmitTime = resultList[0].UpdateTime;
                            if (resultList[0].Category != null)
                            {
                                rdoc.DocumentCateCode = resultList[0].Category.CategoryCode;
                                rdoc.DocumentCateName = resultList[0].Category.CategoryName;
                            }
                            rdoc.FileURL = filePath;

                            listDoc.Add(rdoc);
                            model.SaveOrUpdate(listDoc);
                        }
                        #endregion 保存MBP对象关联文档信息
                    }
                    else
                    {
                        #region
                        IList listDoc = new List<ProObjectRelaDocument>();

                        PLMWebServicesByKB.DocumentInfoType docInfoType = 0;
                        foreach (PLMWebServicesByKB.DocumentInfoType type in Enum.GetValues(typeof(PLMWebServicesByKB.DocumentInfoType)))
                        {
                            if (type.ToString() == cmbDocumentInforType.Text.Trim())
                            {
                                docInfoType = type;
                                break;
                            }
                        }
                        PLMWebServicesByKB.DocumentState docState = 0;
                        foreach (PLMWebServicesByKB.DocumentState state in Enum.GetValues(typeof(PLMWebServicesByKB.DocumentState)))
                        {
                            if (state.ToString() == cmbDocumentStatus.Text.Trim())
                            {
                                docState = state;
                                break;
                            }
                        }

                        List<PLMWebServicesByKB.ProjectDocument> listDocument = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.ProjectDocument>();

                        PLMWebServicesByKB.ProjectDocument doc = new Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.ProjectDocument();

                        FileInfo file = new FileInfo(filePath);
                        FileStream fileStream = file.OpenRead();
                        int FileLen = (int)file.Length;
                        Byte[] FileData = new Byte[FileLen];
                        ////将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
                        fileStream.Read(FileData, 0, FileLen);
                        if (FileData.Length == 0)
                        {
                            MessageBox.Show("该文件长度为0,请检查!");
                            return;
                        }
                        doc.ExtendName = Path.GetExtension(filePath); //文档扩展名*******************************
                        doc.FileDataByte = FileData; //文件二进制流
                        doc.FileName = file.Name;//文件名称

                        doc.ProjectCode = OptProjectInfo.Code; //所属项目代码*
                        doc.ProjectName = txtResideProject.Text; //所属项目名称*

                        doc.Author = txtDocumentAuthor.Text;//文档作者*

                        doc.Category = new Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.CategoryNode();
                        doc.Category.CategoryCode = txtDocumentSortEncode.Text;//"CSFL";//文档分类代码
                        doc.Category.CategoryName = txtDocumentSortName.Text;//"测试分类"; //文档分类名称

                        List<string> listDocParam = StaticMethod.GetUploadFileParamsByKB();
                        string docObjTypeName = listDocParam[0];
                        string docCateLinkTypeName = listDocParam[2];

                        doc.CategoryRelaDocType = docCateLinkTypeName;//文档分类类型
                        //doc.CategorySysCode = "";//文档分类层次码
                        doc.Code = txtDocumentCode.Text;//文档代码
                        doc.Description = txtDocumentExplain.Text;//文档说明
                        doc.DocType = docInfoType;//文档信息类型
                        doc.KeyWords = txtDocumentKeywords.Text;//文档关键字
                        doc.Name = txtDocumentName.Text;//文档名称
                        //string docObjTypeName= StaticMethod.GetMBPUploadFileParams()[0];
                        doc.ObjectTypeName = docObjTypeName;//文档对象类型
                        //doc.OwnerID = "";//责任人
                        //doc.OwnerName = "";//责任人名称
                        //doc.OwnerOrgSysCode = "";// 责任人组织层次码
                        //doc.Revision = "";//文档版次
                        doc.State = docState;//文档状态
                        doc.Title = txtDocumentTitle.Text;//文档标题
                        //doc.Version = "";//文档版本
                        //doc.ExtendInfoNames = "";//扩展属性名
                        //doc.ExtendInfoValues = "";//扩展属性值

                        listDocument.Add(doc);
                        PLMWebServicesByKB.ProjectDocument[] resultList = null;

                        PLMWebServicesByKB.ErrorStack es = StaticMethod.AddDocumentByKB(listDocument.ToArray(),
                            Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.DocumentSaveMode.一个文件生成一个文档对象, null,
                            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_UserName,
                            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_JobId, null, out resultList);

                        if (es != null)
                        {
                            MessageBox.Show("文件“" + file.Name + "”上传到服务器失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("上传成功！");
                            Empty();
                        }

                        #region 保存MBP对象关联文档信息
                        if (resultList != null)
                        {
                            listDoc.Clear();
                            string fileId = resultList[0].ID;
                            ProObjectRelaDocument rdoc = new ProObjectRelaDocument();

                            rdoc.TheProjectGUID = OptProjectInfo.Id;
                            rdoc.TheProjectName = OptProjectInfo.Name;
                            rdoc.TheProjectCode = OptProjectInfo.Code;

                            if (projectObjectName != null && projectObjectName != "")
                            {
                                rdoc.ProObjectName = projectObjectName;
                                rdoc.ProObjectGUID = projectObjectGUID;
                            }
                            else
                            {
                                rdoc.ProObjectName = OptProjectInfo.GetType().Name;
                                rdoc.ProObjectGUID = OptProjectInfo.Id;
                            }

                            rdoc.DocumentOwner = ConstObject.LoginPersonInfo;
                            rdoc.DocumentOwnerName = ConstObject.LoginPersonInfo.Name;
                            rdoc.DocumentGUID = resultList[0].EntityID;
                            rdoc.DocumentName = resultList[0].Name;
                            rdoc.DocumentDesc = resultList[0].Description;
                            rdoc.DocumentCode = resultList[0].Code;
                            rdoc.SubmitTime = resultList[0].UpdateTime;
                            if (resultList[0].Category != null)
                            {
                                rdoc.DocumentCateCode = resultList[0].Category.CategoryCode;
                                rdoc.DocumentCateName = resultList[0].Category.CategoryName;
                            }
                            rdoc.FileURL = filePath;

                            listDoc.Add(rdoc);
                            model.SaveOrUpdate(listDoc);
                        }
                        #endregion 保存MBP对象关联文档信息
                        #endregion
                    }
                    #endregion
                }

                else
                {
                    #region 上传新版本
                    if (isIRPOrKB != "KB")
                    {
                        #region
                        PLMWebServices.DocumentState docState = 0;
                        foreach (PLMWebServices.DocumentState state in Enum.GetValues(typeof(PLMWebServices.DocumentState)))
                        {
                            if (state.ToString() == cmbDocumentStatus.Text.Trim())
                            {
                                docState = state;
                                break;
                            }
                        }

                        PLMWebServices.ProjectDocument updatedDoc = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument();
                        PLMWebServices.ProjectDocument[] updateResultList = null;
                        List<PLMWebServices.ProjectDocument> updateDocumentList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument>();

                        updatedDoc = upNewVersion[0] as PLMWebServices.ProjectDocument;

                        FileInfo file = new FileInfo(filePath);
                        FileStream fileStream = file.OpenRead();
                        int FileLen = (int)file.Length;
                        Byte[] FileData = new Byte[FileLen];
                        ////将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
                        fileStream.Read(FileData, 0, FileLen);
                        if (FileData.Length == 0)
                        {
                            MessageBox.Show("该文件长度为0,请检查!");
                            return;
                        }
                        updatedDoc.ExtendName = Path.GetExtension(filePath); //文档扩展名*******************************
                        updatedDoc.FileDataByte = FileData; //文件二进制流
                        updatedDoc.Name = txtDocumentName.Text;
                        updatedDoc.State = docState;

                        //List<string> listDocParam = StaticMethod.GetUploadFileParamsByMBP_IRP();
                        //string docObjTypeName = listDocParam[0];
                        ////string docCateLinkTypeName = listDocParam[2];
                        //updatedDoc.ObjectTypeName = docObjTypeName;
                        updateDocumentList.Add(updatedDoc);

                        PLMWebServices.ErrorStack es = StaticMethod.UpdateDocumentByIRP(updateDocumentList.ToArray(), Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentUpdateMode.添加一个新版次文件,
                            null, userName, jobId, null, out updateResultList);
                        if (es != null)
                        {
                            MessageBox.Show("文件“" + file.Name + "”上传到服务器失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("更新最新版本成功！");
                            IList updateResultDoc = new List<PLMWebServices.ProjectDocument>();
                            updateResultDoc.Add(updateResultList[0]);
                            resultListDoc = updateResultDoc;
                            this.Close();
                        }
                        #endregion
                    }
                    else
                    {
                        #region
                        PLMWebServicesByKB.DocumentState docState = 0;
                        foreach (PLMWebServicesByKB.DocumentState state in Enum.GetValues(typeof(PLMWebServicesByKB.DocumentState)))
                        {
                            if (state.ToString() == cmbDocumentStatus.Text.Trim())
                            {
                                docState = state;
                                break;
                            }
                        }

                        PLMWebServicesByKB.ProjectDocument updatedDoc = new Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.ProjectDocument();
                        PLMWebServicesByKB.ProjectDocument[] updateResultList = null;
                        List<PLMWebServicesByKB.ProjectDocument> updateDocumentList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.ProjectDocument>();

                        updatedDoc = upNewVersion[0] as PLMWebServicesByKB.ProjectDocument;

                        FileInfo file = new FileInfo(filePath);
                        FileStream fileStream = file.OpenRead();
                        int FileLen = (int)file.Length;
                        Byte[] FileData = new Byte[FileLen];
                        ////将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
                        fileStream.Read(FileData, 0, FileLen);
                        if (FileData.Length == 0)
                        {
                            MessageBox.Show("该文件长度为0,请检查!");
                            return;
                        }
                        updatedDoc.ExtendName = Path.GetExtension(filePath); //文档扩展名*******************************
                        updatedDoc.FileDataByte = FileData; //文件二进制流
                        updatedDoc.Name = txtDocumentName.Text;
                        updatedDoc.State = docState;

                        //List<string> listDocParam = StaticMethod.GetUploadFileParamsByMBP_IRP();
                        //string docObjTypeName = listDocParam[0];
                        ////string docCateLinkTypeName = listDocParam[2];
                        //updatedDoc.ObjectTypeName = docObjTypeName;
                        updateDocumentList.Add(updatedDoc);

                        PLMWebServicesByKB.ErrorStack es = StaticMethod.UpdateDocumentByKB(updateDocumentList.ToArray(), Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.DocumentUpdateMode.添加一个新版次文件,
                            null, Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_UserName,
                            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_JobId, null, out updateResultList);

                        if (es != null)
                        {
                            MessageBox.Show("文件“" + file.Name + "”上传到服务器失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("更新最新版本成功！");
                            IList updateResultDoc = new List<PLMWebServicesByKB.ProjectDocument>();
                            updateResultDoc.Add(updateResultList[0]);
                            resultListDoc = updateResultDoc;
                            this.Close();
                        }
                        #endregion
                    }
                    #endregion
                }
            }
        }
        /// <summary>
        /// 清空填写数据
        /// </summary>
        void Empty()
        {
            txtDocumentAuthor.Text = "";
            txtDocumentCode.Text = "";
            txtDocumentExplain.Text = "";
            txtDocumentKeywords.Text = "";
            txtDocumentName.Text = "";
            txtDocumentSortEncode.Text = "";
            txtDocumentSortName.Text = "";
            txtDocumentTitle.Text = "";
            txtFileURL.Text = "";
            cmbDocumentInforType.SelectedIndex = 0;
            cmbDocumentStatus.SelectedIndex = 0;
            filePath = "";
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

        private string GetExceptionMessage(PLMWebServicesByKB.ErrorStack es)
        {
            string msg = es.Message;
            PLMWebServicesByKB.ErrorStack esTemp = es.InnerErrorStack;
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


        void btnBrowse_Click(object sender, EventArgs e)
        {
            filePath = GetfilePath();
            if (filePath != "")
            {
                txtFileURL.Text = filePath;
                FileInfo file = new FileInfo(filePath);
                string s = file.Name.Substring(0, file.Name.LastIndexOf("."));
                txtDocumentName.Text = s;
                txtDocumentTitle.Text = s;
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

        #region  放弃
        /// <summary>
        /// 放弃
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        bool Verify()
        {
            if (txtFileURL.Text.Trim() == "")
            {
                MessageBox.Show("请选择文件！");
                return false;
            }
            if (txtDocumentName.Text.Trim() == "")
            {
                MessageBox.Show("文档名称不能为空！");
                return false;
            }
            return true;
        }
    }
}
