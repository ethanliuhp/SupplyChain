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
using VirtualMachine.Component.Util;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using NHibernate.Cfg;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng
{
    public partial class VDocumentUploadList : TBasicDataView
    {
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();
        string filePath = string.Empty;
        string userName = string.Empty;
        string jobId = string.Empty;
        //string isIRPOrKB = string.Empty;
        string addOrUpdate = "add";
        int updateRowIndex = -1;
        public MPBSTree model = new MPBSTree();

        //IList upNewVersion = null;

        //操作的项目
        public CurrentProjectInfo OptProjectInfo = null;

        //工程对象名称 GUID
        string projectObjectName = string.Empty;
        string projectObjectGUID = string.Empty;

        private IList resultListDoc;
        public IList ResultListDoc
        {
            get { return resultListDoc; }
            set { resultListDoc = value; }
        }

        public VDocumentUploadList()
        {
            InitializeComponent();
            InitEvent();
            InitData();
            InitcomboBoxData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectInfo">操作的项目</param>
        /// <param name="proObjName">工程对象类型名称</param>
        /// <param name="proObjGUID">工程对象GUID</param>
        public VDocumentUploadList(CurrentProjectInfo projectInfo, object entityObj, string proObjGUID)
        {
            InitializeComponent();

            OptProjectInfo = projectInfo;
            projectObjectName = entityObj.GetType().Name;
            projectObjectGUID = proObjGUID;

            InitEvent();
            InitData();
            InitcomboBoxData();

            txtResideProject.Text = OptProjectInfo.Name;
            txtDocumentObjectType.Text = projectObjectName;

            //设置默认分类

            SetObjectDefaultDocCate(entityObj);

        }

        /// <summary>
        /// 设置对象缺省配置的文档分类
        /// </summary>
        private void SetObjectDefaultDocCate(object entityObj)
        {
            if (entityObj == null)
                return;

            Configuration cfg = new Configuration().AddAssembly("SupplyChain");

            NHibernate.Mapping.PersistentClass perClass = cfg.GetClassMapping(entityObj.GetType());

            string tableName = perClass.Table.Name.ToLower();//配置时统一转换为小写

            IEnumerable<NHibernate.Mapping.Property> dicProperty = perClass.ReferenceablePropertyIterator;
            Dictionary<string, string> dicColumnToProperty = new Dictionary<string, string>();
            //IEnumerable<NHibernate.Mapping.Column> dicColumn = perClass.Table.ColumnIterator;

            foreach (NHibernate.Mapping.Property p in dicProperty)
            {
                if (p.ColumnIterator.Count() > 0)
                {
                    string proName = p.Name;
                    string colName = p.ColumnIterator.ElementAt(0).Text.ToLower();

                    dicColumnToProperty.Add(colName, proName);
                }
            }

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ObjTypeName", tableName));
            IEnumerable<ObjTypeDefaultCateConfig> listConfig = model.ObjectQuery(typeof(ObjTypeDefaultCateConfig), oq).OfType<ObjTypeDefaultCateConfig>();

            //该业务对象类型配置了缺省文档分类
            if (listConfig.Count() > 0)
            {
                PLMWebServices.CategoryNode cate = null;

                Type entityType = entityObj.GetType();
                foreach (ObjTypeDefaultCateConfig config in listConfig)
                {
                    string colName = config.ObjTypeAttributeName;

                    if (string.IsNullOrEmpty(colName) == false
                        && dicColumnToProperty.ContainsKey(colName)
                        && entityType.GetProperty(dicColumnToProperty[colName]) != null)//包含指定列名
                    {
                        string propertyName = dicColumnToProperty[colName];

                        object value = entityObj.GetType().GetProperty(propertyName).GetValue(entityObj, null);

                        if ((value == null && string.IsNullOrEmpty(config.ObjTypeAttributeValue))
                            || (value != null && IsEqual(value, config.ObjTypeAttributeValue)))//如果匹配相等
                        {
                            if (cate == null)
                                cate = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.CategoryNode();

                            cate.CategoryCode = config.DocCateCode;
                            cate.CategoryName = config.DocCateName;

                            break;
                        }
                    }
                }

                if (cate == null)
                {
                    var queryCate = from c in listConfig
                                    where string.IsNullOrEmpty(c.ObjTypeAttributeName)
                                    select c;

                    if (queryCate.Count() > 0)
                    {
                        ObjTypeDefaultCateConfig config = queryCate.ElementAt(0);
                        cate = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.CategoryNode();
                        cate.CategoryCode = config.DocCateCode;
                        cate.CategoryName = config.DocCateName;
                    }
                }

                if (cate != null)
                {
                    txtDocumentSortEncode.Text = cate.CategoryCode;
                    txtDocumentSortEncode.Tag = cate;
                    txtDocumentSortName.Text = cate.CategoryName;
                    txtDocumentSortName.Tag = cate;
                }
            }
        }

        private bool IsEqual(object value, string configAttValue)
        {
            try
            {
                object value1 = null;
                object value2 = null;
                if (value.GetType().BaseType == typeof(Enum))
                {
                    value1 = (int)value;
                    value2 = Convert.ToInt32(configAttValue);
                }
                else if (string.IsNullOrEmpty(configAttValue) &&
                    (value.GetType() == typeof(int)
                    || value.GetType() == typeof(long)
                    || value.GetType() == typeof(float)
                    || value.GetType() == typeof(double)
                    || value.GetType() == typeof(decimal)))
                {
                    value1 = value;
                    value2 = Convert.ChangeType(0, value.GetType());
                }
                else
                {
                    value1 = value;
                    value2 = Convert.ChangeType(configAttValue, value.GetType());
                }
                return value1.Equals(value2);
            }
            catch
            {
                return false;
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
            this.btnAdd.Click += new EventHandler(btnAdd_Click);
            this.btnRemoveSelect.Click += new EventHandler(btnRemoveSelect_Click);
            this.btnEmpty.Click += new EventHandler(btnEmpty_Click);
            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
        }

        void InitData()
        {
            userName = "admin";
            jobId = "AAAA4DC34F5882C122C3D0FA863D";
            //userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            //jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
            OptProjectInfo = StaticMethod.GetProjectInfo();
            object[] os = new object[] { txtResideProject, txtDocumentObjectType, txtDocumentSortName, txtDocumentSortEncode };
            ObjectLock.Lock(os);
        }

        /// <summary>
        /// 移除选中项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnRemoveSelect_Click(object sender, EventArgs e)
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
        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnEmpty_Click(object sender, EventArgs e)
        {
            gridBrownFileList.Rows.Clear();
        }
        /// <summary>
        /// 文档分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearchSort_Click(object sender, EventArgs e)
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

        /// <summary>
        /// 向列表中添加数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAdd_Click(object sender, EventArgs e)
        {
            if (filePath == "")
            {
                MessageBox.Show("请选择文件！");
                return;
            }
            if (!Verify()) return;
            if (addOrUpdate == "update")
            {
                gridBrownFileList.Rows.RemoveAt(updateRowIndex);
            }
            int index = gridBrownFileList.Rows.Add();
            DataGridViewRow row = gridBrownFileList.Rows[index];
            row.Cells[BrownFileName.Name].Value = txtDocumentName.Text;
            row.Cells[BrownFileCateName.Name].Value = txtDocumentSortName.Text;
            row.Cells[BrownFileCateCode.Name].Value = txtDocumentSortEncode.Text;
            row.Cells[BrownFileCode.Name].Value = txtDocumentCode.Text;
            row.Cells[BrownFileAuthor.Name].Value = txtDocumentAuthor.Text;
            row.Cells[BrownFileDesc.Name].Value = txtDocumentExplain.Text;
            row.Cells[BrownFileInforType.Name].Value = cmbDocumentInforType.Text;
            row.Cells[BrownFileState.Name].Value = cmbDocumentStatus.Text;
            row.Cells[BrownFileKeyWord.Name].Value = txtDocumentKeywords.Text;
            row.Cells[BrownFilePath.Name].Value = filePath;
            row.Cells[BrownFileTitle.Name].Value = txtDocumentTitle.Text;
            FileInfo fileInfo = new FileInfo(filePath);
            row.Cells[BrownFileSize.Name].Value = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetFileAutoSizeString(fileInfo.Length, 3);
            addOrUpdate = "add";
            Empty();

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
        /// <summary>
        /// 修改选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnUpdate_Click(object sender, EventArgs e)
        {
            if (gridBrownFileList.SelectedRows.Count > 1)
            {
                MessageBox.Show("一次只能修改一行！");
                return;
            }
            if (gridBrownFileList.SelectedRows.Count < 1)
            {
                MessageBox.Show("请先选择数据！");
                return;
            }
            int rowIndex = gridBrownFileList.SelectedRows[0].Index;
            DataGridViewRow row = gridBrownFileList.Rows[rowIndex];
            txtDocumentName.Text = row.Cells[BrownFileName.Name].Value.ToString();
            txtDocumentSortName.Text = row.Cells[BrownFileCateName.Name].Value.ToString();
            txtDocumentSortEncode.Text = row.Cells[BrownFileCateCode.Name].Value.ToString();
            txtDocumentCode.Text = row.Cells[BrownFileCode.Name].Value.ToString();
            txtDocumentAuthor.Text = row.Cells[BrownFileAuthor.Name].Value.ToString();
            txtDocumentExplain.Text = row.Cells[BrownFileDesc.Name].Value.ToString();
            cmbDocumentInforType.Text = row.Cells[BrownFileInforType.Name].Value.ToString();
            cmbDocumentStatus.Text = row.Cells[BrownFileState.Name].Value.ToString();
            txtDocumentKeywords.Text = row.Cells[BrownFileKeyWord.Name].Value.ToString();
            filePath = txtFileURL.Text = row.Cells[BrownFilePath.Name].Value.ToString();
            txtDocumentTitle.Text = row.Cells[BrownFileTitle.Name].Value.ToString();
            updateRowIndex = rowIndex;
            addOrUpdate = "update";
        }

        /// <summary>
        /// 批量上传文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnUploadFile_Click(object sender, EventArgs e)
        {

            //if (txtFileURL.Text == "")
            //{
            //    MessageBox.Show("请选择文件！");
            //}
            //else
            //{
            //校验数据
            if (gridBrownFileList.Rows.Count == 0)
            {
                MessageBox.Show("列表里没有上传的数据！");
                return;
            }
            foreach (DataGridViewRow row in gridBrownFileList.Rows)
            {
                if (row.Cells[BrownFileName.Name].Value == null)
                {
                    MessageBox.Show("文件名不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    gridBrownFileList.CurrentCell = row.Cells[BrownFileName.Name];
                    gridBrownFileList.BeginEdit(false);
                    return;
                }
            }

            #region 批量上传

            if (OptProjectInfo.Code != "KB")
            {
                try
                {
                    int rowCount = gridBrownFileList.Rows.Count;

                    progressBarDocUpload.Minimum = 0;
                    progressBarDocUpload.Maximum = rowCount >= 10 ? rowCount + 1 : 11;
                    progressBarDocUpload.Value = 1;

                    //显示进度条，使用单个上传模式

                    IList addResultDocList = new List<ProObjectRelaDocument>();
                    for (int rowIndex = gridBrownFileList.Rows.Count - 1; rowIndex > -1; rowIndex--)
                    {
                        #region
                        DataGridViewRow row = gridBrownFileList.Rows[rowIndex];
                        IList listDoc = new List<ProObjectRelaDocument>();
                        List<PLMWebServices.ProjectDocument> docList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument>();
                        PLMWebServices.ProjectDocument[] resultList = null;
                        PLMWebServices.ProjectDocument doc = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument();
                        string filePath1 = row.Cells[BrownFilePath.Name].Value.ToString();

                        FileInfo file = new FileInfo(filePath1);
                        if (file.Exists)
                        {
                            #region 上传文件
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
                            doc.ExtendName = Path.GetExtension(filePath1); //文档扩展名*******************************
                            doc.FileDataByte = FileData; //文件二进制流
                            doc.FileName = file.Name;//文件名称

                            doc.ProjectCode = OptProjectInfo.Code; //所属项目代码*
                            doc.ProjectName = OptProjectInfo.Name; //所属项目名称*

                            List<string> listDocParam = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetUploadFileParamsByMBP_IRP();
                            string docObjTypeName = listDocParam[0];
                            string docCateLinkTypeName = listDocParam[2];

                            doc.CategoryRelaDocType = docCateLinkTypeName;//文档分类类型
                            doc.ObjectTypeName = docObjTypeName;//文档对象类型

                            doc.Category = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.CategoryNode();
                            doc.Category.CategoryCode = row.Cells[BrownFileCateCode.Name].Value.ToString();//"CSFL";//文档分类代码
                            doc.Category.CategoryName = row.Cells[BrownFileCateName.Name].Value.ToString();//"测试分类"; //文档分类名称

                            PLMWebServices.DocumentInfoType docInfoType = 0;
                            foreach (PLMWebServices.DocumentInfoType type in Enum.GetValues(typeof(PLMWebServices.DocumentInfoType)))
                            {
                                if (type.ToString() == row.Cells[BrownFileInforType.Name].Value.ToString())
                                {
                                    docInfoType = type;
                                    break;
                                }
                            }

                            PLMWebServices.DocumentState docState = 0;
                            foreach (PLMWebServices.DocumentState state in Enum.GetValues(typeof(PLMWebServices.DocumentState)))
                            {
                                if (state.ToString() == row.Cells[BrownFileState.Name].Value.ToString())
                                {
                                    docState = state;
                                    break;
                                }
                            }
                            doc.DocType = docInfoType;//文档信息类型
                            doc.State = docState;//文档状态

                            doc.Code = row.Cells[BrownFileCode.Name].Value.ToString();//文档代码
                            doc.Description = row.Cells[BrownFileDesc.Name].Value.ToString();//文档说明
                            doc.KeyWords = row.Cells[BrownFileKeyWord.Name].Value.ToString();//文档关键字
                            doc.Name = row.Cells[BrownFileName.Name].Value.ToString();//文档名称
                            doc.Title = row.Cells[BrownFileTitle.Name].Value.ToString(); ;//文档标题
                            doc.Author = row.Cells[BrownFileAuthor.Name].Value.ToString();//文档作者*
                            //doc.CategorySysCode = "";//文档分类层次码
                            //string docObjTypeName= StaticMethod.GetMBPUploadFileParams()[0]; 
                            //doc.OwnerID = "";//责任人
                            //doc.OwnerName = "";//责任人名称
                            //doc.OwnerOrgSysCode = "";// 责任人组织层次码
                            //doc.Revision = "";//文档版次 
                            //doc.Version = "";//文档版本
                            //doc.ExtendInfoNames = "";//扩展属性名
                            //doc.ExtendInfoValues = "";//扩展属性值
                            docList.Add(doc);

                            PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.AddDocumentByIRP(docList.ToArray(), Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentSaveMode.一个文件生成一个文档对象,
                                null, userName, jobId, null, out resultList);

                            fileStream.Close();
                            if (es != null)
                            {
                                MessageBox.Show("文件“" + row.Cells[BrownFileName.Name].Value.ToString() + "”上传到服务器失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                progressBarDocUpload.Value = 0;
                                return;
                            }
                            #endregion 上传文件

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
                                    rdoc.ProObjectName = OptProjectInfo.Name;
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
                                rdoc.FileURL = filePath1;

                                listDoc.Add(rdoc);
                                IList result = model.SaveOrUpdate(listDoc);
                                ProObjectRelaDocument pord = result[0] as ProObjectRelaDocument;
                                addResultDocList.Add(pord);
                            }
                            #endregion 保存MBP对象关联文档信息
                        }
                        #endregion

                        gridBrownFileList.Rows.RemoveAt(rowIndex);

                        if (gridBrownFileList.Rows.Count < 10)
                            progressBarDocUpload.Value += (int)Math.Floor((decimal)10 / rowCount);
                        else
                            progressBarDocUpload.Value += 1;
                    }

                    progressBarDocUpload.Value = progressBarDocUpload.Maximum;

                    MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    resultListDoc = addResultDocList;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
                }

                progressBarDocUpload.Value = 0;

            }
            else
            {
                try
                {
                    int rowCount = gridBrownFileList.Rows.Count;

                    progressBarDocUpload.Minimum = 0;
                    progressBarDocUpload.Maximum = rowCount >= 10 ? rowCount + 1 : 11;
                    progressBarDocUpload.Value = 1;

                    //显示进度条，使用单个上传模式
                    IList addResultDocList = new List<ProObjectRelaDocument>();
                    for (int rowIndex = gridBrownFileList.Rows.Count - 1; rowIndex > -1; rowIndex--)
                    {
                        #region
                        DataGridViewRow row = gridBrownFileList.Rows[rowIndex];
                        IList listDoc = new List<ProObjectRelaDocument>();
                        List<PLMWebServicesByKB.ProjectDocument> docList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.ProjectDocument>();
                        PLMWebServicesByKB.ProjectDocument[] resultList = null;
                        PLMWebServicesByKB.ProjectDocument doc = new Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.ProjectDocument();
                        string filePath1 = row.Cells[BrownFilePath.Name].Value.ToString();

                        FileInfo file = new FileInfo(filePath1);
                        if (file.Exists)
                        {
                            #region 上传文件
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
                            doc.ExtendName = Path.GetExtension(filePath1); //文档扩展名*******************************
                            doc.FileDataByte = FileData; //文件二进制流
                            doc.FileName = file.Name;//文件名称

                            doc.ProjectCode = OptProjectInfo.Code; //所属项目代码*
                            doc.ProjectName = OptProjectInfo.Name; //所属项目名称*

                            List<string> listDocParam = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetUploadFileParamsByKB();
                            string docObjTypeName = listDocParam[0];
                            string docCateLinkTypeName = listDocParam[2];

                            doc.CategoryRelaDocType = docCateLinkTypeName;//文档分类类型
                            doc.ObjectTypeName = docObjTypeName;//文档对象类型

                            doc.Category = new Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.CategoryNode();
                            doc.Category.CategoryCode = row.Cells[BrownFileCateCode.Name].Value.ToString();//"CSFL";//文档分类代码
                            doc.Category.CategoryName = row.Cells[BrownFileCateName.Name].Value.ToString();//"测试分类"; //文档分类名称

                            PLMWebServicesByKB.DocumentInfoType docInfoType = 0;
                            foreach (PLMWebServicesByKB.DocumentInfoType type in Enum.GetValues(typeof(PLMWebServicesByKB.DocumentInfoType)))
                            {
                                if (type.ToString() == row.Cells[BrownFileInforType.Name].Value.ToString())
                                {
                                    docInfoType = type;
                                    break;
                                }
                            }

                            PLMWebServicesByKB.DocumentState docState = 0;
                            foreach (PLMWebServicesByKB.DocumentState state in Enum.GetValues(typeof(PLMWebServicesByKB.DocumentState)))
                            {
                                if (state.ToString() == row.Cells[BrownFileState.Name].Value.ToString())
                                {
                                    docState = state;
                                    break;
                                }
                            }
                            doc.DocType = docInfoType;//文档信息类型
                            doc.State = docState;//文档状态

                            doc.Code = row.Cells[BrownFileCode.Name].Value.ToString();//文档代码
                            doc.Description = row.Cells[BrownFileDesc.Name].Value.ToString();//文档说明
                            doc.KeyWords = row.Cells[BrownFileKeyWord.Name].Value.ToString();//文档关键字
                            doc.Name = row.Cells[BrownFileName.Name].Value.ToString();//文档名称
                            doc.Title = row.Cells[BrownFileTitle.Name].Value.ToString(); ;//文档标题
                            doc.Author = row.Cells[BrownFileAuthor.Name].Value.ToString();//文档作者*
                            //doc.CategorySysCode = "";//文档分类层次码
                            //string docObjTypeName= StaticMethod.GetMBPUploadFileParams()[0]; 
                            //doc.OwnerID = "";//责任人
                            //doc.OwnerName = "";//责任人名称
                            //doc.OwnerOrgSysCode = "";// 责任人组织层次码
                            //doc.Revision = "";//文档版次 
                            //doc.Version = "";//文档版本
                            //doc.ExtendInfoNames = "";//扩展属性名
                            //doc.ExtendInfoValues = "";//扩展属性值
                            docList.Add(doc);

                            PLMWebServicesByKB.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.AddDocumentByKB(docList.ToArray(), Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.DocumentSaveMode.一个文件生成一个文档对象,
                                null, Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_UserName, Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_JobId,
                                null, out resultList);
                            if (es != null)
                            {
                                MessageBox.Show("文件“" + row.Cells[BrownFileName.Name].Value.ToString() + "”上传到服务器失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            #endregion 上传文件

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
                                    rdoc.ProObjectName = OptProjectInfo.Name;
                                    rdoc.ProObjectGUID = OptProjectInfo.Id;
                                }

                                rdoc.DocumentOwner = ConstObject.LoginPersonInfo;
                                rdoc.DocumentOwnerName = ConstObject.LoginPersonInfo.Name;
                                rdoc.DocumentGUID = resultList[0].EntityID;
                                rdoc.DocumentName = resultList[0].Name;
                                rdoc.DocumentDesc = resultList[0].Description;
                                rdoc.SubmitTime = resultList[0].UpdateTime;
                                if (resultList[0].Category != null)
                                {
                                    rdoc.DocumentCateCode = resultList[0].Category.CategoryCode;
                                    rdoc.DocumentCateName = resultList[0].Category.CategoryName;
                                }
                                rdoc.FileURL = filePath1;

                                listDoc.Add(rdoc);
                                IList result = model.SaveOrUpdate(listDoc);
                                ProObjectRelaDocument pord = result[0] as ProObjectRelaDocument;
                                addResultDocList.Add(pord);
                            }
                            #endregion 保存MBP对象关联文档信息
                        }
                        #endregion

                        gridBrownFileList.Rows.RemoveAt(rowIndex);

                        if (gridBrownFileList.Rows.Count < 10)
                            progressBarDocUpload.Value += (int)Math.Floor((decimal)10 / rowCount);
                        else
                            progressBarDocUpload.Value += 1;
                    }

                    progressBarDocUpload.Value = progressBarDocUpload.Maximum;

                    MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    resultListDoc = addResultDocList;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
                }

                progressBarDocUpload.Value = 0;

            }
            #endregion

            //}
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

        #region 选择文件
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
        #endregion

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
            if (txtDocumentName.Text.Trim() == "")
            {
                MessageBox.Show("文档名称不能为空！");
                return false;
            }
            return true;
        }
    }
}
