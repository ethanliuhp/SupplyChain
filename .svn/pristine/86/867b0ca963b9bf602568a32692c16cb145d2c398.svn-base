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

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng
{
    public partial class VDocumentListUpdate : TBasicDataView
    {
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();
        string filePath = string.Empty;
        string userName = string.Empty;
        string jobId = string.Empty;
        int updateRowIndex = -1;
        public MPBSTree model = new MPBSTree();

        //IList upNewVersion = null;

        //操作的项目
        public CurrentProjectInfo OptProjectInfo = null;

        ////工程对象名称 GUID
        //string projectObjectName = string.Empty;
        //string projectObjectGUID = string.Empty;
        IList updateList = null;

        private IList resultListDoc;
        public IList ResultListDoc
        {
            get { return resultListDoc; }
            set { resultListDoc = value; }
        }

        public VDocumentListUpdate()
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
        /// <param name="list">修改的对象集合</param>
        public VDocumentListUpdate(CurrentProjectInfo projectInfo, IList list)
        {
            InitializeComponent();

            OptProjectInfo = projectInfo;
            updateList = list;

            InitEvent();
            InitData();
            InitcomboBoxData();

            //txtResideProject.Text = OptProjectInfo.Name;
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
            //cmbDocumentInforType.SelectedIndex = 0;
            //cmbDocumentStatus.SelectedIndex = 0;
        }
        void InitEvent()
        {
            this.btnQuit.Click += new EventHandler(btnQuit_Click);
            this.btnBrowse.Click += new EventHandler(btnBrowse_Click);
            this.btnUpdateSelect.Click += new EventHandler(btnUpdateSelect_Click);
            this.btnSaveUpdate.Click += new EventHandler(btnSaveUpdate_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
        }

        void InitData()
        {
            //userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            //jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
            userName = "admin";
            jobId = "AAAA4DC34F5882C122C3D0FA863D";
            gbUpdate.Enabled = false;
            // 向列表中添加数据
            gridBrownFileList.Rows.Clear();
            foreach (ProObjectRelaDocument doc in updateList)
            {
                int index = gridBrownFileList.Rows.Add();
                DataGridViewRow row = gridBrownFileList.Rows[index];
                row.Cells[BrownFileName.Name].Value = doc.DocumentName;
                row.Cells[BrownFileCateCode.Name].Value = doc.DocumentCateCode;
                row.Cells[BrownFileCateName.Name].Value = doc.DocumentCateName;
                row.Cells[BrownFileCode.Name].Value = doc.DocumentCode;
                row.Cells[BrownFileDesc.Name].Value = doc.DocumentDesc;
                gridBrownFileList.Rows[index].Tag = doc;
            }
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnOK_Click(object sender, EventArgs e)
        {
            IList resultList = new List<ProObjectRelaDocument>();
            foreach (DataGridViewRow row in gridBrownFileList.Rows)
            {
                ProObjectRelaDocument relaDoc = new ProObjectRelaDocument();
                relaDoc = row.Tag as ProObjectRelaDocument;
                resultList.Add(relaDoc);
            }
            resultListDoc = resultList;
            this.Close();
        }

        /// <summary>
        /// 修改选中行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnUpdateSelect_Click(object sender, EventArgs e)
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
            gbUpdate.Enabled = true;
            int rowIndex = gridBrownFileList.SelectedRows[0].Index;
            updateRowIndex = rowIndex;
            if (OptProjectInfo.Code != "KB")
            {
                ProObjectRelaDocument relaDocument = gridBrownFileList.Rows[rowIndex].Tag as ProObjectRelaDocument;
                PLMWebServices.ProjectDocument[] resultDoc = null;
                List<string> fileIds = new List<string>();
                fileIds.Add(relaDocument.DocumentGUID);

                PLMWebServices.ErrorStack es = StaticMethod.GetProjectDocumentByIRP(fileIds.ToArray(), Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentQueryVersion.最新版本,
                    null, userName, jobId, null, out resultDoc);
                if (es != null)
                {
                    MessageBox.Show("操作失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (resultDoc != null)
                {
                    DataGridViewRow row = gridBrownFileList.Rows[rowIndex];
                    txtDocumentName.Text = relaDocument.DocumentName;
                    txtDocumentExplain.Text = relaDocument.DocumentDesc;
                    //txtDocumentCode.Text = relaDocument;
                    txtDocumentAuthor.Text = resultDoc[0].Author;
                    cmbDocumentInforType.Text = resultDoc[0].DocType.ToString();
                    cmbDocumentStatus.Text = resultDoc[0].State.ToString();
                    txtDocumentKeywords.Text = resultDoc[0].KeyWords;
                    txtDocumentKeywords.Tag = resultDoc[0];
                    txtDocumentTitle.Text = resultDoc[0].Title;
                }
            }
            else
            {
                ProObjectRelaDocument relaDocument = gridBrownFileList.Rows[rowIndex].Tag as ProObjectRelaDocument;
                PLMWebServicesByKB.ProjectDocument[] resultDoc = null;
                List<string> fileIds = new List<string>();
                fileIds.Add(relaDocument.DocumentGUID);

                PLMWebServicesByKB.ErrorStack es = StaticMethod.GetProjectDocumentByKB(fileIds.ToArray(), Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.DocumentQueryVersion.最新版本,
                    null, Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_UserName,
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_JobId, null, out resultDoc);
                if (es != null)
                {
                    MessageBox.Show("操作失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (resultDoc != null)
                {
                    DataGridViewRow row = gridBrownFileList.Rows[rowIndex];
                    txtDocumentName.Text = relaDocument.DocumentName;
                    txtDocumentExplain.Text = relaDocument.DocumentDesc;
                    //txtDocumentCode.Text = relaDocument;
                    txtDocumentAuthor.Text = resultDoc[0].Author;
                    cmbDocumentInforType.Text = resultDoc[0].DocType.ToString();
                    cmbDocumentStatus.Text = resultDoc[0].State.ToString();
                    txtDocumentKeywords.Text = resultDoc[0].KeyWords;
                    txtDocumentKeywords.Tag = resultDoc[0];
                    txtDocumentTitle.Text = resultDoc[0].Title;
                }
            }

        }

        /// <summary>
        ///保存修改 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSaveUpdate_Click(object sender, EventArgs e)
        {
            
            if (OptProjectInfo.Code != "KB")
            {
                List<PLMWebServices.ProjectDocument> updateList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument>();
                PLMWebServices.ProjectDocument[] resultUpdateDoc = null;
                ProObjectRelaDocument relaDoc = gridBrownFileList.Rows[updateRowIndex].Tag as ProObjectRelaDocument;
                PLMWebServices.ProjectDocument doc = txtDocumentKeywords.Tag as PLMWebServices.ProjectDocument;

                if (filePath != "")
                {
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
                }

                relaDoc.DocumentName = doc.Name = txtDocumentName.Text;
                relaDoc.DocumentDesc = doc.Description = txtDocumentExplain.Text;
                doc.Author = txtDocumentAuthor.Text;
                doc.KeyWords = txtDocumentKeywords.Text;
                doc.Title = txtDocumentTitle.Text;

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
                doc.DocType = docInfoType;
                doc.State = docState;
                updateList.Add(doc);
                PLMWebServices.ErrorStack es = StaticMethod.UpdateDocumentByIRP(updateList.ToArray(), Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentUpdateMode.添加一个新版次文件,
                    null, userName, jobId, null, out resultUpdateDoc);
                if (es != null)
                {
                    MessageBox.Show("操作失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (resultUpdateDoc != null)
                {
                    IList updateRelaDocList = new List<ProObjectRelaDocument>();
                    updateRelaDocList.Add(relaDoc);
                    IList resultRelaDocList = model.SaveOrUpdate(updateRelaDocList);
                    if (resultRelaDocList != null && resultRelaDocList.Count > 0)
                    {
                        MessageBox.Show("修改成功！");
                        ProObjectRelaDocument rd = resultRelaDocList[0] as ProObjectRelaDocument;
                        gridBrownFileList[BrownFileName.Name, updateRowIndex].Value = rd.DocumentName;
                        gridBrownFileList[BrownFileDesc.Name, updateRowIndex].Value = rd.DocumentDesc;
                        gridBrownFileList.Rows[updateRowIndex].Tag = rd;
                        Empty();
                    }
                    else
                    {
                        MessageBox.Show("修改失败！");
                    }
                }
            }
            else
            {
                List<PLMWebServicesByKB.ProjectDocument> updateList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.ProjectDocument>();
                PLMWebServicesByKB.ProjectDocument[] resultUpdateDoc = null;
                ProObjectRelaDocument relaDoc = gridBrownFileList.Rows[updateRowIndex].Tag as ProObjectRelaDocument;
                PLMWebServicesByKB.ProjectDocument doc = txtDocumentKeywords.Tag as PLMWebServicesByKB.ProjectDocument;

                if (filePath != null && txtFileURL.Text.Trim() != "")
                {
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
                }

                relaDoc.DocumentName = doc.Name = txtDocumentName.Text;
                relaDoc.DocumentDesc = doc.Description = txtDocumentExplain.Text;
                doc.Author = txtDocumentAuthor.Text;
                doc.KeyWords = txtDocumentKeywords.Text;
                doc.Title = txtDocumentTitle.Text;

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
                doc.DocType = docInfoType;
                doc.State = docState;
                updateList.Add(doc);
                PLMWebServicesByKB.ErrorStack es = StaticMethod.UpdateDocumentByKB(updateList.ToArray(), Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.DocumentUpdateMode.添加一个新版次文件,
                    null, Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_UserName, 
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_JobId, null, out resultUpdateDoc);
                if (es != null)
                {
                    MessageBox.Show("操作失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (resultUpdateDoc != null)
                {
                    IList updateRelaDocList = new List<ProObjectRelaDocument>();
                    updateRelaDocList.Add(relaDoc);
                    IList resultRelaDocList = model.SaveOrUpdate(updateRelaDocList);
                    if (resultRelaDocList != null && resultRelaDocList.Count > 0)
                    {
                        MessageBox.Show("修改成功！");
                        ProObjectRelaDocument rd = resultRelaDocList[0] as ProObjectRelaDocument;
                        gridBrownFileList[BrownFileName.Name, updateRowIndex].Value = rd.DocumentName;
                        gridBrownFileList[BrownFileDesc.Name, updateRowIndex].Value = rd.DocumentDesc;
                        gridBrownFileList.Rows[updateRowIndex].Tag = rd;
                        Empty();
                    }
                    else
                    {
                        MessageBox.Show("修改失败！");
                    }
                }
            }
            filePath = string.Empty;
        }
        /// <summary>
        /// 清空数据
        /// </summary>
        void Empty()
        {
            txtDocumentAuthor.Text = "";
            txtDocumentExplain.Text = "";
            txtDocumentKeywords.Text = "";
            txtDocumentName.Text = "";
            txtDocumentTitle.Text = "";
            txtFileURL.Text = "";
            cmbDocumentInforType.SelectedIndex = 0;
            cmbDocumentStatus.SelectedIndex = 0;
            filePath = "";
            gbUpdate.Enabled = false;
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
    }
}
