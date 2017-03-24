using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.EngineerManage.CollectionManage.Domain;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;

using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using VirtualMachine.Core;
using System.IO;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using IRPServiceModel.Domain.Document;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.FileUpload;
using IRPServiceModel.Basic;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.CollectionManage
{
    public partial class VCollectionManage : TMasterDetailView
    {

        private MCollectionManage model = new MCollectionManage();
        private ColletionManage cmaser;
        CurrentProjectInfo projectInfo;
        string userName = string.Empty;
        string jobId = string.Empty;
        string fileObjectType = string.Empty;
        public ColletionManage Cmaser
        {
            get { return cmaser; }
            set { cmaser = value; }
        }
        IList booklist = new ArrayList();
        public VCollectionManage()
        {
            InitializeComponent();
            InitData();
            InitEvent();
        }
        public void InitData()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
            VBasicDataOptr.SendStyleType(cmoSendStyle, true);
            VBasicDataOptr.LettersStyleType(cmoLettersStyle, true);
        }
        private void InitEvent()
        {
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
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", cmaser.Id));
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
            if (cmaser.Id == null)
            {
                if (MessageBox.Show("当前业务对象还没保存，是否保存！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    if (!ValidView()) return;
                    try
                    {
                        if (!ViewToModel()) return;
                        cmaser = model.saveImp(cmaser);
                        this.ViewCaption = ViewName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(ex));
                    }
                }
            }
            if (cmaser.Id != null)
            {
                VDocumentPublicUpload frm = new VDocumentPublicUpload(cmaser.Id);
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
            if (e.RowIndex < 0) return;
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


        public void Start(string Id)
        {
            try
            {
                btnStates(0);
                if (Id == "空")
                    RefreshState(MainViewState.Initialize);

                else
                {
                    btnDownLoadDocument.Enabled = true;
                    btnOpenDocument.Enabled = true;
                    cmaser = model.ICollectionMngSrv.GetImpById(Id);
                    ModelToView();
                    RefreshState(MainViewState.Browser);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("视图启动出错：" + ExceptionUtil.ExceptionMessage(e));

            }
        }
        /// <summary>
        /// 刷新状态(按钮状态)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshState(MainViewState state)
        {
            base.RefreshState(state);
            //控制表格
            switch (state)
            {
                case MainViewState.AddNew:
                case MainViewState.Modify:
                    cmoSendStyle.Enabled = true;
                    cmoLettersStyle.Enabled = true;
                    txtRemark.Enabled = true;
                    dtSendLettersDate.Enabled = true;
                    txtSendUnits.ReadOnly = false;
                    txtLettersName.ReadOnly = false;
                    txtLettersId.ReadOnly = false;
                    cmoLettersStyle.Text = "";
                    cmoSendStyle.Text = "";
                    txtPersonCharge.Enabled = false;
                    txtPlanName.Enabled = false;
                    dtSendLettersDate.Text = ConstObject.LoginDate.ToString();
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    cmoSendStyle.Enabled = false;
                    cmoLettersStyle.Enabled = false;
                    txtRemark.Enabled = false;
                    dtSendLettersDate.Enabled = false;
                    txtSendUnits.ReadOnly = true;
                    txtLettersName.ReadOnly = true; ;
                    txtLettersId.ReadOnly = true;
                    txtPersonCharge.ReadOnly = true; ;
                    txtPlanName.ReadOnly = true;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 刷新控件(窗体中的控件)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshControls(MainViewState state)
        {
            base.RefreshControls(state);

            //控制自身控件
            if (ViewState == MainViewState.AddNew || ViewState == MainViewState.Modify)
            {
                ObjectLock.Unlock(pnlFloor, true);
                btnStates(1);
            }
            else
            {
                ObjectLock.Lock(pnlFloor, true);
                btnStates(0);
            }

            //永久锁定
            object[] os = new object[] { txtPlanName, txtPersonCharge };
            ObjectLock.Lock(os);

        }


        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        public override bool SubmitView()
        {
            try
            {
                if (!ViewToModel()) return false;
                Cmaser.DocState = DocumentState.InExecute;
                //curBillMaster.AuditPerson = ConstObject.LoginPersonInfo;//制单人编号
                //curBillMaster.AuditPersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                //curBillMaster.AuditDate = ConstObject.LoginDate;//制单时间
                //curBillMaster.AuditYear = ConstObject.LoginDate.Year;//制单年
                //curBillMaster.AuditMonth = ConstObject.LoginDate.Month;//制单月
                Cmaser = model.ICollectionMngSrv.saveImp(Cmaser);
                ////插入日志
                //txtCode.Text = curBillMaster.Code;
                ////更新Caption
                //this.ViewCaption = ViewName + "-" + txtCode.Text;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {

            if (ClientUtil.ToString(this.cmoSendStyle.SelectedItem) == "")
            {
                MessageBox.Show("收发类型不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.cmoLettersStyle.SelectedItem) == "")
            {
                MessageBox.Show("函件类型不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.txtSendUnits.Text) == "")
            {
                MessageBox.Show("发函单位不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.txtGetUnits.Text) == "")
            {
                MessageBox.Show("收函单位不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.txtLettersName.Text) == "")
            {
                MessageBox.Show("函件名称不能为空！");
                return false;
            }
  
            return true;

        }
        /// <summary>
        /// 显示数据
        /// </summary>
        public void ModelToView()
        {
            try
            {
                //主表
                 this.txtPlanName.Text = ClientUtil.ToString(Cmaser.ProjectName);
                 this.txtPlanName.Tag = Cmaser.ProjectId;       
                this.txtPersonCharge.Text = ClientUtil.ToString(Cmaser.CreatePersonName);
                this.dtMadeBillStartDate.Text = Cmaser.CreateDate.ToShortDateString();
                this.cmoSendStyle.Text = ClientUtil.ToString(Cmaser.SendStyle);

                //函件信息
                txtSendUnits.Text = ClientUtil.ToString(Cmaser.SendUnits);
                txtGetUnits.Text = ClientUtil.ToString(Cmaser.GetUnits);
                txtLettersName.Text = ClientUtil.ToString(Cmaser.LettersName);
                cmoLettersStyle.Text = ClientUtil.ToString(Cmaser.LettersStyle);
                txtLettersId.Text = ClientUtil.ToString(Cmaser.LettersId);
                dtSendLettersDate.Text = Cmaser.SendLettersDate.ToShortDateString();
                txtRemark.Text = ClientUtil.ToString(Cmaser.Remark);
                FillDoc();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 新建
        /// </summary>
        /// <returns></returns>
        public override bool NewView()
        {
            try
            {
                base.NewView();
                ClearView();
                this.Cmaser = new ColletionManage();
                Cmaser.CreateDate = ConstObject.LoginDate;//制单时间                            
                Cmaser.CreatePersonName = ConstObject.LoginPersonInfo.Name;//负责人名称
                Cmaser.CreatePersonId = ConstObject.LoginPersonInfo.Id;
                //制单日期
                dtMadeBillStartDate.Text = ConstObject.LoginDate.ToShortDateString();
                //责任人
                txtPersonCharge.Tag = ConstObject.LoginPersonInfo;
                txtPersonCharge.Text = ConstObject.LoginPersonInfo.Name;
                //归属项目
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                if (projectInfo != null)
                {
                    txtPlanName.Tag = projectInfo;
                    txtPlanName.Text = projectInfo.Name;
                    cmaser.ProjectId = projectInfo.Id;
                    cmaser.ProjectName = projectInfo.Name;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            return true;
        }



        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public override bool SaveView()
        {
            if (!ValidView()) return false;
            try
            {
                if (!ViewToModel()) return false;
                bool flag = false;
                if (string.IsNullOrEmpty(cmaser.Id))
                {
                    flag = true;
                }
                cmaser = model.saveImp(cmaser);

                this.ViewCaption = ViewName;
                MessageBox.Show("保存成功！");
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }


        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                cmaser.ProjectName = ClientUtil.ToString(this.txtPlanName.Text);
                cmaser.CreateDate = ClientUtil.ToDateTime(this.dtMadeBillStartDate.Text);
                cmaser.CreatePersonName = ClientUtil.ToString(this.txtPersonCharge.Text);
                cmaser.SendStyle = ClientUtil.ToString(this.cmoSendStyle.Text);
                //函件信息
                cmaser.SendUnits = ClientUtil.ToString(this.txtSendUnits.Text);
                cmaser.GetUnits = ClientUtil.ToString(this.txtGetUnits.Text);
                cmaser.LettersName = ClientUtil.ToString(this.txtLettersName.Text);
                if (this.cmoLettersStyle.SelectedItem != null)
                {
                    cmaser.LettersStyle = ClientUtil.ToString(this.cmoLettersStyle.Text);
                }
                cmaser.LettersId = ClientUtil.ToString(this.txtLettersId.Text);
                cmaser.SendLettersDate = ClientUtil.ToDateTime(this.dtSendLettersDate.Text);
                cmaser.Remark = ClientUtil.ToString(this.txtRemark.Text);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
             if (cmaser.DocState == DocumentState.Edit || cmaser.DocState == DocumentState.Valid)
            {
                base.ModifyView();
                cmaser = model.ICollectionMngSrv.GetImpById(cmaser.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(cmaser.DocState));
            MessageBox.Show(message);
            return false;
        
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                cmaser = model.ICollectionMngSrv.GetImpById(cmaser.Id);
                  //if (!model.ICollectionMngSrv.DeleteByDao(cmaser)) return false;
                  if (!msrv.DeleteReceiptAndDocument(cmaser, cmaser.Id)) return false;
                    ClearView();
                    MessageBox.Show("删除成功！");
                    return true;
                    MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(cmaser.DocState) + "】，不能删除！");
                    return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据删除错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 撤销
        /// </summary>
        /// <returns></returns>
        public override bool CancelView()
        {
            try
            {
                switch (ViewState)
                {
                    case MainViewState.Modify:
                        //重新查询数据
                        cmaser = model.ICollectionMngSrv.GetImpById(cmaser.Id);
                        ModelToView();
                        break;
                    default:
                        ClearView();
                        break;
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据撤销错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        private void ClearView()
        {
            ClearControl(pnlFloor);
        }

        private void ClearControl(Control c)
        {
            foreach (Control cd in c.Controls)
            {
                ClearControl(cd);
            }
            this.txtRemark.Text = "";
            //自定义控件清空
            if (c is CustomEdit)
            {
                c.Tag = null;
                c.Text = "";
            }
            else if (c is VirtualMachine.Component.WinControls.Controls.CustomDataGridView)
            {
                (c as CustomDataGridView).Rows.Clear();
            }
        }
    }
}
