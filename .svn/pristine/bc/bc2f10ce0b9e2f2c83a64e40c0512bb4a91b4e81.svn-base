using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.MobileManage.OftenWords;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using IRPServiceModel.Domain.Document;
using VirtualMachine.Component.Util;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using System.IO;
using IRPServiceModel.Basic;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng
{
    public partial class VDocumentsModelAndTaskType : TBasicDataView
    {
        private MDocumentCategory model = null;
        private GWBSTree selectWBS = null;
        private IList resultList = null;

        public IList ResultList
        {
            get { return resultList; }
            set { resultList = value; }
        }
        public VDocumentsModelAndTaskType(GWBSTree wbs)
        {
            InitializeComponent();
            selectWBS = wbs;
            InitEvent();
            IntData();
        }
        private void InitEvent()
        {
            dgDocumentMast.CellClick += new DataGridViewCellEventHandler(dgDocumentMast_CellClick);
            btnDocumentDetailDownLoad.Click += new EventHandler(btnDocumentDetailDownLoad_Click);
            btnDocumentDetailShow.Click += new EventHandler(btnDocumentDetailShow_Click);
            lnkCheckAll.Click += new EventHandler(lnkCheckAll_Click);
            lnkCheckAllNot.Click += new EventHandler(lnkCheckAllNot_Click);

            btnOK.Click += new EventHandler(btnOK_Click);
            btnQuit.Click += new EventHandler(btnQuit_Click);
        }

        void IntData()
        {
            model = new MDocumentCategory();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", selectWBS.Id));
            oq.AddFetchMode("ProjectTaskTypeGUID", NHibernate.FetchMode.Eager);
            selectWBS = model.ObjectQuery(typeof(GWBSTree), oq)[0] as GWBSTree;
            txtTaskType.Text = GetFullPath(selectWBS.ProjectTaskTypeGUID);
            txtFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), selectWBS.Name, selectWBS.SysCode);
            InitDocumentMaster();
        }
        private string GetFullPath(ProjectTaskTypeTree taskType)
        {
            string path = string.Empty;

            path = taskType.Name;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", taskType.Id));
            oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof(ProjectTaskTypeTree), oq);

            taskType = list[0] as ProjectTaskTypeTree;

            CategoryNode parent = taskType.ParentNode;
            while (parent != null)
            {
                path = parent.Name + "\\" + path;

                oq.Criterions.Clear();
                oq.Criterions.Clear();
                oq.AddCriterion(Expression.Eq("Id", parent.Id));
                oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
                list = model.ObjectQuery(typeof(ProjectTaskTypeTree), oq);

                parent = (list[0] as CategoryNode).ParentNode;
            }

            return path;
        }
        void InitDocumentMaster()
        {
            //根据工程任务类型查找相应 工程任务类型关联文档模板
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProTaskType.Id", selectWBS.ProjectTaskTypeGUID.Id));
            IList list = model.ObjectQuery(typeof(ProTaskTypeDocumentStencil), oq);
            //根据工程任务类型关联文档模板上的文档模版ID查找相应的模版
            oq.Criterions.Clear();
            Disjunction dis = new Disjunction();
            foreach (ProTaskTypeDocumentStencil s in list)
            {
                dis.Add(Expression.Eq("Id", s.ProDocumentMasterID));
            }
            oq.AddCriterion(dis);
            IList documentMasterList = model.ObjectQuery(typeof(DocumentMaster), oq);

            if (documentMasterList != null && documentMasterList.Count > 0)
            {
                foreach (DocumentMaster m in documentMasterList)
                {
                    AddDgDocumentMastInfo(m);
                }
                dgDocumentMast.CurrentCell = dgDocumentMast[2, 0];
                dgDocumentMast_CellClick(dgDocumentMast, new DataGridViewCellEventArgs(2, 0));
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
            dgDocumentDetail.Rows[rowIndex].Tag = d;
        }
        #endregion

        //预览
        void btnDocumentDetailShow_Click(object sender, EventArgs e)
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
        //下载
        void btnDocumentDetailDownLoad_Click(object sender, EventArgs e)
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
                return;
            }
        }

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

        void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            IList listGWBSRelaTemplate = new ArrayList();
            foreach (DataGridViewRow row in dgDocumentMast.Rows)
            {
                if ((bool)row.Cells[DocumentSelect.Name].EditedFormattedValue)
                {
                    DocumentMaster doc = row.Tag as DocumentMaster;
                    ProjectDocumentVerify docVerify = new ProjectDocumentVerify();
                    docVerify.ProjectName = "知识库";
                    docVerify.ProjectCode = "KB";

                    docVerify.ProjectTask = selectWBS;
                    docVerify.ProjectTaskName = selectWBS.Name;

                    if (doc.IsInspectionLot)
                    {
                        docVerify.AssociateLevel = ProjectDocumentAssociateLevel.检验批;
                    }
                    else
                    {
                        docVerify.AssociateLevel = ProjectDocumentAssociateLevel.GWBS;
                    }

                    docVerify.DocuemntID = doc.Id;
                    docVerify.DocumentCode = doc.Code;
                    docVerify.DocumentName = doc.Name;
                    docVerify.DocumentDesc = doc.Description;
                    docVerify.FileSourceURl = null;
                    docVerify.DocumentCategoryCode = doc.CategoryCode;
                    docVerify.DocumentCategoryName = doc.CategoryName;
                    //docVerify.DocumentWorkflowName = docStencil.ControlWorkflowName;

                    docVerify.AlterMode = ProjectTaskTypeAlterMode.任务完成时触发验证;
                    docVerify.SubmitState = ProjectDocumentSubmitState.未提交;
                    docVerify.VerifySwitch = ProjectDocumentVerifySwitch.不验证;

                    listGWBSRelaTemplate.Add(docVerify);
                }
            }

            resultList = model.SaveOrUpdate(listGWBSRelaTemplate);
            this.Close();
        }
    }
}
