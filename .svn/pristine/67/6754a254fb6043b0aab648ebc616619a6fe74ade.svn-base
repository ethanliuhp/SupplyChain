using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.EngineerManage.TargetRespBookManage.Domain;
using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.EngineerManage.Client.TargetRespBookMng;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using System.IO;
using IRPServiceModel.Domain.Document;
using IRPServiceModel.Basic;
using Application.Business.Erp.SupplyChain.Client.FileUpload;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng
{
    public partial class VTargetRespBook : TMasterDetailView
    {
        private MTargetRespBookMng model = new MTargetRespBookMng();
        private TargetRespBook curBillMaster;
        CurrentProjectInfo projectInfo;
        string userName = string.Empty;
        string jobId = string.Empty;
        string fileObjectType = string.Empty;
        //CurrentProjectInfo projectInfo = new CurrentProjectInfo();
        IList bookList = new ArrayList();//存储删除的明细，新增，保存，修改时清空

        public VTargetRespBook()
        {
            InitializeComponent();
            InitData();
            InitEvent();
        }
        /// <summary>
        /// 当前表数据
        /// </summary>
        public TargetRespBook CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        private IList list = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList List
        {
            get { return list; }
            set { list = value; }
        }

        public void InitData()
        {
            this.cbProjectSeale.Items.AddRange(new object[] { "特大", "大型", "中型", "小型", "特小" });
            this.cbSignedWhether.Items.AddRange(new object[] { "未签订", "已签订" });
            this.cbRiskPayed.Items.AddRange(new object[] { "未缴纳", "全部缴纳", "部分缴纳" });
            VBasicDataOptr.InitProjectLivel(cbEnsureLevel, false);
            projectInfo = StaticMethod.GetProjectInfo();
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
        }




        public void InitEvent()
        {
            btnSelectProject.Click += new EventHandler(btnSelectProject_Click);
            txtProjectDate.tbTextChanged += new EventHandler(txtProjectDate_tbTextChanged);
            txtCostcontrolTarget.tbTextChanged += new EventHandler(txtCostcontrolTarget_tbTextChanged);
            txtRiskDissolvesTarget.tbTextChanged += new EventHandler(txtRiskDissolvesTarget_tbTextChanged);
            txtResponsibilityTrunedTarget.tbTextChanged += new EventHandler(txtResponsibilityTrunedTarget_tbTextChanged);
            txtCostcontrolRewardTatio.tbTextChanged += new EventHandler(txtCostcontrolRewardTatio_tbTextChanged);
            txtRiskRewardRatio.tbTextChanged += new EventHandler(txtRiskRewardRatio_tbTextChanged);
            txtResponsibilityRewardTatio.tbTextChanged += new EventHandler(txtResponsibilityRewardTatio_tbTextChanged);
            txtResponsibilityRatio.tbTextChanged += new EventHandler(txtResponsibilityRatio_tbTextChanged);
            txtInstallationfreeRate.tbTextChanged += new EventHandler(txtInstallationfreeRate_tbTextChanged);
            dgIrpRiskDepositPayRecord.CellEndEdit += new DataGridViewCellEventHandler(dgIrpRiskDepositPayRecord_CellEndEdit);
            dgTargetProgressNode.CellEndEdit += new DataGridViewCellEventHandler(dgTargetProgressNode_CellEndEdit);
            dtpPlanBeginDate.CloseUp += new EventHandler(dtpPlanBeginDate_CloseUp);
            dtpPlanEndDate.CloseUp += new EventHandler(dtpPlanBeginDate_CloseUp);
            btnSearchPerson.Click += new EventHandler(btnSearchPerson_Click);
            //txtProjectName.tbTextChanged += new EventHandler(txtProjectName_tbTextChanged);
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
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", curBillMaster.Id));
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
            if (curBillMaster.Id == null)
            {
                if (MessageBox.Show("当前业务对象还没保存，是否保存！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    if (!ValidView()) return;
                    try
                    {
                        if (!ViewToModel()) return;
                        curBillMaster = model.TargetRespBookSrc.saveTargetRespBook(curBillMaster);
                        this.ViewCaption = ViewName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(ex));
                    }
                }
            }
            if (curBillMaster.Id != null)
            {
                VDocumentPublicUpload frm = new VDocumentPublicUpload(curBillMaster.Id);
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

        private void dtpPlanBeginDate_CloseUp(object sender, EventArgs e)
        {
            showDate();
        }

        void showDate()
        {
            DateTime planBeginDate = dtpPlanBeginDate.Value.Date;
            DateTime plannEndDate = dtpPlanEndDate.Value.Date;
            if (planBeginDate == plannEndDate)
            {
                txtProjectDate.Text = "1";
            }
            else
            {
                TimeSpan date = plannEndDate - planBeginDate;
                string[] date1 = date.ToString().Split('.');
                int date2 = Convert.ToInt32(date1[0]) + 1;
                string date3 = date2.ToString();
                txtProjectDate.Text = date3;
            }
        }

        void btnSearchPerson_Click(object sender, EventArgs e)
        {
            if (this.txtProjectName.Text == "")
            {
                MessageBox.Show("请选择项目......");
            }
            else
            {
                VSeclectCommonPerson vscp = new VSeclectCommonPerson(txtProjectName.Tag as CurrentProjectInfo);
                vscp.ShowDialog();
                list = vscp.Result;
                if (list.Count == 0) return;
                for (int i = 0; i < list.Count; i++)
                {
                    IrpRiskDepositPayRecord record = list[i] as IrpRiskDepositPayRecord;
                    bool flag = false;
                    foreach (DataGridViewRow var in this.dgIrpRiskDepositPayRecord.Rows)
                    {
                        if (var.IsNewRow) break;
                        string personname = ClientUtil.ToString(var.Cells[colName.Name].Value);
                        string job = ClientUtil.ToString(var.Cells[colProjectPosition.Name].Value);
                        if (personname == record.Name && job == record.ProjectPosition)
                        {
                            flag = true;
                            MessageBox.Show("列表中已存在选中的人员名称......");
                            break;
                        }
                    }
                    if (!flag)
                    {
                        int j = dgIrpRiskDepositPayRecord.Rows.Add();
                        this.dgIrpRiskDepositPayRecord[colName.Name, j].Value = record.Name;
                        this.dgIrpRiskDepositPayRecord[colProjectPosition.Name, j].Value = record.ProjectPosition;
                        this.dgIrpRiskDepositPayRecord[colPaidinDate.Name, j].Value = DateTime.Now;
                    }
                }
            }

        }

        /// <summary>
        /// 项目名称从文档库中选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSelectProject_Click(object sender, EventArgs e)
        {
            txtProjectName.tbTextChanged += new EventHandler(txtProjectName_tbTextChanged);
            VSelectProjectInfo frm = new VSelectProjectInfo();
            CurrentProjectInfo extProject = new CurrentProjectInfo();
            frm.ListExtendProject.Add(extProject);
            frm.ShowDialog();

            if (frm.Result != null && frm.Result.Count > 0)
            {
                CurrentProjectInfo selectProject = frm.Result[0] as CurrentProjectInfo;
                if (selectProject != null)
                {
                    txtProjectName.Text = selectProject.Name;
                    txtProjectName.Tag = selectProject;
                }
            }
            txtProjectName.tbTextChanged -= new EventHandler(txtProjectName_tbTextChanged);
        }

        private void txtProjectName_tbTextChanged(object sender, EventArgs e)
        {
            string projectName = this.txtProjectName.Text;
            SearchProjectManage(projectName);
        }

        public void SearchProjectManage(string projectName)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("ProjectName", projectName));
            IList alists = model.TargetRespBookSrc.GetTargetRespBook(objectQuery);
            if (alists != null && alists.Count > 0)
            {
                if (projectName == txtProjectName.Text)
                {
                    MessageBox.Show("该项目下的目标责任书已经存在");
                }
                this.txtProjectName.Text = "";
            }

        }

        #region 固定代码
        /// <summary>
        /// 启动本窗体,(设置状态或重新加载已有的数据)
        /// </summary>
        /// <param name="code">窗体Caption</param>
        public void Start(string Id)
        {
            try
            {
                btnStates(0);
                if (Id == "空")
                    RefreshState(MainViewState.Initialize);
                else
                {
                    curBillMaster = model.TargetRespBookSrc.GetTargetRespBookById(Id);
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
                    //this.dgDetail.EditMode = DataGridViewEditMode.EditOnEnter;
                    //cmsDg.Enabled = true;
                    btnSelectProject.Enabled = true;
                    txtProjectManager.Enabled = true;
                    btnSearchPerson.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    //this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
                    //cmsDg.Enabled = false;
                    btnSelectProject.Enabled = false;
                    txtProjectManager.Enabled = false;
                    btnSearchPerson.Enabled = false;
                    break;
                default:
                    break;
            }
        }
        #endregion

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
                ObjectLock.Unlock(pnlContent, true);
                btnStates(1);
            }
            else
            {
                ObjectLock.Lock(pnlContent, true);
                btnStates(0);
            }

            object[] lockCols = new object[] { txtProjectName, txtHandlePerson, dtpCreateDate, txtProjectDate };
            ObjectLock.Lock(lockCols);
        }

        /// <summary>
        /// 清空数据
        /// </summary>
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
            //自定义控件清空
            if (c is CustomEdit)
            {
                c.Tag = null;
                c.Text = "";

            }
            else if (c is CustomDataGridView)
            {
                (c as CustomDataGridView).Rows.Clear();
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
                        curBillMaster = model.TargetRespBookSrc.GetTargetRespBookById(curBillMaster.Id);
                        ModelToView();
                        break;
                    default:
                        ClearView();
                        break;
                }
                //btnStates(0);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据撤销错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public override void RefreshView()
        {
            try
            {
                //重新获得当前对象的值
                curBillMaster = model.TargetRespBookSrc.GetTargetRespBookById(curBillMaster.Id);
                //给界面赋值
                ModelToView();
            }
            catch (Exception e)
            {
                MessageBox.Show("数据刷新错误：" + ExceptionUtil.ExceptionMessage(e));
            }
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
                CurBillMaster.DocState = DocumentState.InExecute;
                //curBillMaster.AuditPerson = ConstObject.LoginPersonInfo;//制单人编号
                //curBillMaster.AuditPersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                //curBillMaster.AuditDate = ConstObject.LoginDate;//制单时间
                //curBillMaster.AuditYear = ConstObject.LoginDate.Year;//制单年
                //curBillMaster.AuditMonth = ConstObject.LoginDate.Month;//制单月
                CurBillMaster = model.TargetRespBookSrc.saveTargetRespBook(CurBillMaster);
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
        /// 新建
        /// </summary>
        /// <returns></returns>
        public override bool NewView()
        {
            try
            {
                base.NewView();
                ClearView();
                //btnStates(1);
                bookList = new ArrayList();

                this.curBillMaster = new TargetRespBook();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.CreateDate = ConstObject.LoginDate;
                curBillMaster.RealOperationDate = ConstObject.LoginDate;
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.DocState = DocumentState.Edit;

                //制单日期
                dtpCreateDate.Text = ConstObject.LoginDate.ToShortDateString();
                //责任人
                txtHandlePerson.Tag = ConstObject.LoginPersonInfo;
                txtHandlePerson.Text = ConstObject.LoginPersonInfo.Name;
                //txtProjectManager.Text = curBillMaster.HandlePersonName;

                this.cbProjectSeale.SelectedItem = "特小";
                this.cbSignedWhether.SelectedItem = "已签订";
                this.cbRiskPayed.SelectedItem = "未缴纳";
                this.cbEnsureLevel.SelectedItem = "合格";
                showDate();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 保存数据前校验数据
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            if (ClientUtil.ToString(this.txtProjectName.Text) == "")
            {
                MessageBox.Show("项目名称不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.txtDocumentName.Text) == "")
            {
                MessageBox.Show("文档名称不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.txtProjectManager.Text) == "")
            {
                MessageBox.Show("项目经理不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.cbProjectSeale.Text) == "")
            {
                MessageBox.Show("项目规模不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.cbSignedWhether.SelectedItem) == "")
            {
                MessageBox.Show("签订状态不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.cbRiskPayed.SelectedItem) == "")
            {
                MessageBox.Show("风险抵押金缴纳情况不能为空！");
                return false;
            }
            if (ClientUtil.ToDateTime(this.dtpPlanBeginDate.Text) > ClientUtil.ToDateTime(this.dtpPlanEndDate.Text))
            {
                MessageBox.Show("计划开始时间应小于等于计划竣工时间！");
                return false;
            }
            if (ClientUtil.ToDecimal(this.txtCostcontrolTarget.Text.Length) >= 13 || ClientUtil.ToDecimal(this.txtRiskDissolvesTarget.Text.Length) >= 13 || ClientUtil.ToDecimal(this.txtResponsibilityTrunedTarget.Text.Length) >= 13 || ClientUtil.ToDecimal(this.txtCashRewardNodeNumber.Text.Length) >= 13 || ClientUtil.ToDecimal(this.txtCostcontrolRewardTatio.Text.Length) >= 13 || ClientUtil.ToDecimal(this.txtRiskRewardRatio.Text.Length) >= 13 || ClientUtil.ToDecimal(this.txtResponsibilityRewardTatio.Text.Length) >= 13 || ClientUtil.ToDecimal(this.txtInstallationfreeRate.Text.Length) >= 13)
            {
                MessageBox.Show("责任目标中，你输入的字符太长，无法保存！");
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
            try
            {
                if (!ViewToModel()) return false;
                bool flag = false;
                if (string.IsNullOrEmpty(curBillMaster.Id))
                {
                    flag = true;
                }
                curBillMaster = model.TargetRespBookSrc.saveTargetRespBook(curBillMaster);
                MessageBox.Show("保存成功！");
                //btnStates(0);
                return true;

            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }

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
                curBillMaster = model.TargetRespBookSrc.GetTargetRespBookById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    //if (!model.TargetRespBookSrc.DeleteByDao(curBillMaster)) return false;
                    if (!msrv.DeleteReceiptAndDocument(curBillMaster, curBillMaster.Id)) return false;
                    ClearView();
                    MessageBox.Show("删除成功！");
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能删除！");
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据删除错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
            curBillMaster = model.TargetRespBookSrc.GetTargetRespBookById(curBillMaster.Id);
            if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
            {
                base.ModifyView();
                ModelToView();
                return true;
            }
            MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能修改！");
            return false;

        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns></returns>
        private bool ModelToView()
        {
            try
            {
                this.txtProjectName.Text = ClientUtil.ToString(curBillMaster.ProjectName);
                this.txtProjectName.Tag = curBillMaster.ProjectId;
                this.txtHandlePerson.Value = ClientUtil.ToString(curBillMaster.HandlePerson);
                this.dtpCreateDate.Value = curBillMaster.CreateDate;
                this.txtDocumentName.Text = ClientUtil.ToString(curBillMaster.DocumentName);
                this.txtProjectManager.Value = ClientUtil.ToString(curBillMaster.ProjectManagerName);
                this.cbProjectSeale.SelectedItem = ClientUtil.ToString(curBillMaster.ProjectScale);
                this.cbSignedWhether.SelectedItem = ClientUtil.ToString(curBillMaster.SignedWhether);
                //this.dtpSignedDate.Text = curBillMaster.SignDate.ToShortDateString();
                this.cbRiskPayed.SelectedItem = ClientUtil.ToString(curBillMaster.RiskPaymentState);

                txtProjectDate.Text = ClientUtil.ToString(curBillMaster.ProjectDate);
                dtpPlanBeginDate.Text = curBillMaster.PlanBeginDate.ToShortDateString();
                dtpPlanEndDate.Text = curBillMaster.PlanEndDate.ToShortDateString();
                cbEnsureLevel.SelectedItem = ClientUtil.ToString(curBillMaster.EnsureLevel);
                txtCashRewardNodeNumber.Text = ClientUtil.ToString(curBillMaster.CashRewardNodeNumber);
                decimal cosTarget = ClientUtil.ToDecimal(curBillMaster.CostcontrolTarget);
                cosTarget = cosTarget / 10000;
                txtCostcontrolTarget.Text = ClientUtil.ToString(cosTarget);
                txtCostcontrolRewardTatio.Text = ClientUtil.ToString(curBillMaster.CostcontrolRewardtatio);
                decimal riskTarget = ClientUtil.ToDecimal(curBillMaster.RiskDissolvesTarget);
                riskTarget = riskTarget / 10000;
                txtRiskDissolvesTarget.Text = ClientUtil.ToString(riskTarget);
                txtRiskRewardRatio.Text = ClientUtil.ToString(curBillMaster.RiskrewardRatio);
                decimal resTarget = ClientUtil.ToDecimal(curBillMaster.ResponsibilityTurnedTarget);
                resTarget = resTarget / 10000;
                txtResponsibilityTrunedTarget.Text = ClientUtil.ToString(resTarget);
                txtResponsibilityRewardTatio.Text = ClientUtil.ToString(curBillMaster.ResponsibilityRewardTatio);
                txtResponsibilityRatio.Text = ClientUtil.ToString(curBillMaster.ResponsibilityRatio);
                txtInstallationfreeRate.Text = ClientUtil.ToString(curBillMaster.InstallationFreeRate);
                txtSafetyCivilizedSign.Text = ClientUtil.ToString(curBillMaster.SafetyCivilizedSign);
                txtEconomicGoalEnginner.Text = ClientUtil.ToString(curBillMaster.EconomicgoalEnginner);

                this.dgIrpRiskDepositPayRecord.Rows.Clear();
                foreach (IrpRiskDepositPayRecord record in curBillMaster.RecordDetails)
                {
                    int rowIndex = this.dgIrpRiskDepositPayRecord.Rows.Add();

                    this.dgIrpRiskDepositPayRecord[colName.Name, rowIndex].Value = ClientUtil.ToString(record.Name);
                    this.dgIrpRiskDepositPayRecord[colProjectPosition.Name, rowIndex].Value = ClientUtil.ToString(record.ProjectPosition);
                    this.dgIrpRiskDepositPayRecord[colPayAble.Name, rowIndex].Value = ClientUtil.ToString(record.PayAble);
                    this.dgIrpRiskDepositPayRecord[colPaidinAmount.Name, rowIndex].Value = ClientUtil.ToString(record.PaidinAmount);
                    this.dgIrpRiskDepositPayRecord[colPaidinDate.Name, rowIndex].Value = record.PaidinDate.ToShortDateString();
                    this.dgIrpRiskDepositPayRecord.Rows[rowIndex].Tag = record;
                }

                this.dgTargetProgressNode.Rows.Clear();
                foreach (TargetProgressNode node in curBillMaster.NodeDetails)
                {
                    int rowIndex = this.dgTargetProgressNode.Rows.Add();

                    this.dgTargetProgressNode[colNodeName.Name, rowIndex].Value = ClientUtil.ToString(node.NodeNameId);
                    this.dgTargetProgressNode[colFigurativeProgress.Name, rowIndex].Value = ClientUtil.ToString(node.FigurativeProgress);
                    decimal predictvalue = ClientUtil.ToDecimal(node.PredictValue);
                    predictvalue = predictvalue / 10000;
                    this.dgTargetProgressNode[colPredictValue.Name, rowIndex].Value = ClientUtil.ToString(predictvalue);
                    decimal benefitGoal = ClientUtil.ToDecimal(node.BenefitGoal);
                    benefitGoal = benefitGoal / 10000;
                    this.dgTargetProgressNode[colBenefitGoal.Name, rowIndex].Value = ClientUtil.ToString(benefitGoal);
                    this.dgTargetProgressNode[colPlanCompleteDate.Name, rowIndex].Value = node.PlanCompleteDate.ToShortDateString();
                    this.dgTargetProgressNode.Rows[rowIndex].Tag = node;
                }
                FillDoc();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
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
                //this.txtCode.Focus();
                curBillMaster.ProjectId = ClientUtil.ToString(txtProjectName.Tag);
                curBillMaster.ProjectName = ClientUtil.ToString(this.txtProjectName.Text);
                curBillMaster.HandlePerson = ClientUtil.ToString(this.txtHandlePerson.Text);
                curBillMaster.CreateDate = DateTime.Now;
                curBillMaster.DocumentName = ClientUtil.ToString(this.txtDocumentName.Text);
                curBillMaster.ProjectManagerId = this.txtProjectManager.Result[0] as PersonInfo;
                curBillMaster.ProjectManagerName = ClientUtil.ToString(this.txtProjectManager.Value);
                curBillMaster.ProjectScale = ClientUtil.ToString(this.cbProjectSeale.SelectedItem);
                curBillMaster.SignedWhether = ClientUtil.ToString(this.cbSignedWhether.SelectedItem);
                //curBillMaster.SignDate = ClientUtil.ToDateTime(this.dtpSignedDate.Text);
                curBillMaster.RiskPaymentState = ClientUtil.ToString(this.cbRiskPayed.SelectedItem);

                curBillMaster.ProjectDate = ClientUtil.ToString(txtProjectDate.Text);
                curBillMaster.PlanBeginDate = ClientUtil.ToDateTime(this.dtpPlanBeginDate.Text);
                curBillMaster.PlanEndDate = ClientUtil.ToDateTime(this.dtpPlanEndDate.Text);
                curBillMaster.EnsureLevel = ClientUtil.ToString(this.cbEnsureLevel.SelectedItem);
                curBillMaster.CashRewardNodeNumber = ClientUtil.ToDecimal(this.txtCashRewardNodeNumber.Text);
                decimal cosTarget = ClientUtil.ToDecimal(this.txtCostcontrolTarget.Text);
                cosTarget = cosTarget * 10000;
                curBillMaster.CostcontrolTarget = ClientUtil.ToDecimal(cosTarget);
                curBillMaster.CostcontrolRewardtatio = ClientUtil.ToDecimal(this.txtCostcontrolRewardTatio.Text);
                decimal riskTarget = ClientUtil.ToDecimal(this.txtRiskDissolvesTarget.Text);
                riskTarget = riskTarget * 10000;
                curBillMaster.RiskDissolvesTarget = ClientUtil.ToDecimal(riskTarget);
                curBillMaster.RiskrewardRatio = ClientUtil.ToDecimal(this.txtRiskRewardRatio.Text);
                decimal resTarget = ClientUtil.ToDecimal(this.txtResponsibilityTrunedTarget.Text);
                resTarget = resTarget * 10000;
                curBillMaster.ResponsibilityTurnedTarget = ClientUtil.ToDecimal(resTarget);
                curBillMaster.ResponsibilityRewardTatio = ClientUtil.ToDecimal(this.txtResponsibilityRewardTatio.Text);
                curBillMaster.ResponsibilityRatio = ClientUtil.ToDecimal(this.txtResponsibilityRatio.Text);
                curBillMaster.InstallationFreeRate = ClientUtil.ToDecimal(this.txtInstallationfreeRate.Text);
                curBillMaster.SafetyCivilizedSign = ClientUtil.ToString(this.txtSafetyCivilizedSign.Text);
                curBillMaster.EconomicgoalEnginner = ClientUtil.ToString(this.txtEconomicGoalEnginner.Text);
                string strUnit = "元";
                Application.Resource.MaterialResource.Domain.StandardUnit Unit = null;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Name", strUnit));
                IList lists = model.TargetRespBookSrc.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq);
                if (lists != null && lists.Count > 0)
                {
                    Unit = lists[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                }
                curBillMaster.PrickleName = strUnit;
                curBillMaster.PricePrickle = Unit;

                foreach (DataGridViewRow var in dgIrpRiskDepositPayRecord.Rows)
                {
                    if (var.IsNewRow) break;
                    IrpRiskDepositPayRecord curBillRecord = new IrpRiskDepositPayRecord();
                    if (var.Tag != null)
                    {
                        curBillRecord = var.Tag as IrpRiskDepositPayRecord;
                        if (curBillRecord.Id == null)
                        {
                            curBillMaster.RecordDetails.Remove(curBillRecord);

                        }
                    }
                    curBillRecord.Name = ClientUtil.ToString(var.Cells[colName.Name].Value);
                    curBillRecord.ProjectPosition = ClientUtil.ToString(var.Cells[colProjectPosition.Name].Value);
                    curBillRecord.PayAble = ClientUtil.ToDecimal(var.Cells[colPayAble.Name].Value);
                    curBillRecord.PaidinAmount = ClientUtil.ToDecimal(var.Cells[colPaidinAmount.Name].Value);
                    curBillRecord.PaidinDate = ClientUtil.ToDateTime(var.Cells[colPaidinDate.Name].Value);
                    curBillRecord.PrickleName = strUnit;
                    curBillRecord.PricePrickle = Unit;
                    curBillRecord.ResponsibleName = ClientUtil.ToString(txtHandlePerson.Text);
                    curBillRecord.TargetRespBookGuid = curBillMaster;
                    curBillMaster.AddRecordDetail(curBillRecord);
                }

                foreach (DataGridViewRow var in dgTargetProgressNode.Rows)
                {
                    if (var.IsNewRow) break;
                    TargetProgressNode curBillNode = new TargetProgressNode();
                    if (var.Tag != null)
                    {
                        curBillNode = var.Tag as TargetProgressNode;
                        if (curBillNode.Id == null)
                        {
                            curBillMaster.NodeDetails.Remove(curBillNode);
                        }
                    }
                    curBillNode.NodeNameId = ClientUtil.ToString(var.Cells[colNodeName.Name].Value);
                    curBillNode.FigurativeProgress = ClientUtil.ToString(var.Cells[colFigurativeProgress.Name].Value);
                    decimal predictvalue = ClientUtil.ToDecimal(var.Cells[colPredictValue.Name].Value);
                    predictvalue = predictvalue * 10000;
                    curBillNode.PredictValue = ClientUtil.ToDecimal(predictvalue);
                    decimal benefitGoal = ClientUtil.ToDecimal(var.Cells[colBenefitGoal.Name].Value);
                    benefitGoal = benefitGoal * 10000;
                    curBillNode.BenefitGoal = ClientUtil.ToDecimal(benefitGoal);
                    curBillNode.PlanCompleteDate = ClientUtil.ToDateTime(var.Cells[colPlanCompleteDate.Name].Value);
                    curBillNode.UnitId = Unit;
                    curBillNode.UnitName = strUnit;
                    curBillNode.TargetRespBookGuid = curBillMaster;
                    curBillMaster.AddDetail(curBillNode);
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        void txtProjectDate_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string strDate = txtProjectDate.Text.ToString();
            validity = CommonMethod.VeryValid(strDate);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                txtProjectDate.Text = "";
            }
        }
        void txtCostcontrolTarget_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string strDate = txtCostcontrolTarget.Text.ToString();
            validity = CommonMethod.VeryValid(strDate);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                txtCostcontrolTarget.Text = "";
            }
        }
        void txtRiskDissolvesTarget_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string strDate = txtRiskDissolvesTarget.Text.ToString();
            validity = CommonMethod.VeryValid(strDate);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                txtRiskDissolvesTarget.Text = "";
            }
        }
        void txtResponsibilityTrunedTarget_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string strDate = txtResponsibilityTrunedTarget.Text.ToString();
            validity = CommonMethod.VeryValid(strDate);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                txtResponsibilityTrunedTarget.Text = "";
            }
        }
        void txtCostcontrolRewardTatio_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string strDate = txtCostcontrolRewardTatio.Text.ToString();
            validity = CommonMethod.VeryValid(strDate);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                txtCostcontrolRewardTatio.Text = "";
            }
            else if (!(ClientUtil.TransToDecimal(this.txtCostcontrolRewardTatio.Text) >= 0))
            {
                MessageBox.Show("成本控制奖励比率：大于等于0小于等于100！");
                txtCostcontrolRewardTatio.Text = "";
            }
        }
        void txtRiskRewardRatio_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string strDate = txtRiskRewardRatio.Text.ToString();
            validity = CommonMethod.VeryValid(strDate);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                txtRiskRewardRatio.Text = "";
            }
            else if (!(ClientUtil.TransToDecimal(this.txtRiskRewardRatio.Text) >= 0))
            {
                MessageBox.Show("风险化解奖励比率：大于等于0！");
                txtRiskRewardRatio.Text = "";
            }
        }
        void txtResponsibilityRewardTatio_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string strDate = txtResponsibilityRewardTatio.Text.ToString();
            validity = CommonMethod.VeryValid(strDate);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                txtResponsibilityRewardTatio.Text = "";
            }
            else if (!(ClientUtil.TransToDecimal(this.txtResponsibilityRewardTatio.Text) >= 0))
            {
                MessageBox.Show("责任上缴奖励比率：大于等于0！");
                txtResponsibilityRewardTatio.Text = "";
            }
        }
        void txtResponsibilityRatio_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string strDate = txtResponsibilityRatio.Text.ToString();
            validity = CommonMethod.VeryValid(strDate);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                txtResponsibilityRatio.Text = "";
            }
            else if (!(ClientUtil.TransToDecimal(this.txtResponsibilityRatio.Text) >= 0))
            {
                MessageBox.Show("责任范围外分包工程利润自提比率：大于等于0！");
                txtResponsibilityRatio.Text = "";
            }
        }
        void txtInstallationfreeRate_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string strDate = txtInstallationfreeRate.Text.ToString();
            validity = CommonMethod.VeryValid(strDate);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                txtInstallationfreeRate.Text = "";
            }
            else if (!(ClientUtil.TransToDecimal(this.txtInstallationfreeRate.Text) >= 0))
            {
                MessageBox.Show("安装施工部分配合费比率：大于等于0！");
                txtInstallationfreeRate.Text = "";
            }
        }

        private void dgIrpRiskDepositPayRecord_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            string colName = dgIrpRiskDepositPayRecord.Columns[e.ColumnIndex].Name;
            if (colName == colPayAble.Name)
            {
                if (dgIrpRiskDepositPayRecord.Rows[e.RowIndex].Cells[colPayAble.Name].Value != null)
                {
                    string temp_quantity = dgIrpRiskDepositPayRecord.Rows[e.RowIndex].Cells[colPayAble.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        dgIrpRiskDepositPayRecord.Rows[e.RowIndex].Cells[colPayAble.Name].Value = "";
                    }
                }

                object quantity = dgIrpRiskDepositPayRecord.Rows[e.RowIndex].Cells[colPayAble.Name].Value;
            }
            if (colName == colPaidinAmount.Name)
            {
                if (dgIrpRiskDepositPayRecord.Rows[e.RowIndex].Cells[colPaidinAmount.Name].Value != null)
                {
                    string temp_quantity = dgIrpRiskDepositPayRecord.Rows[e.RowIndex].Cells[colPaidinAmount.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        dgIrpRiskDepositPayRecord.Rows[e.RowIndex].Cells[colPayAble.Name].Value = "";
                    }
                }
                object quantity = dgIrpRiskDepositPayRecord.Rows[e.RowIndex].Cells[colPaidinAmount.Name].Value;
            }
        }

        private void dgTargetProgressNode_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            string colName = dgTargetProgressNode.Columns[e.ColumnIndex].Name;
            if (colName == colPredictValue.Name)
            {
                if (dgTargetProgressNode.Rows[e.RowIndex].Cells[colPredictValue.Name].Value != null)
                {
                    string temp_quantity = dgTargetProgressNode.Rows[e.RowIndex].Cells[colPredictValue.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        dgTargetProgressNode.Rows[e.RowIndex].Cells[colPredictValue.Name].Value = "";
                    }
                }

                object quantity = dgTargetProgressNode.Rows[e.RowIndex].Cells[colPredictValue.Name].Value;
            }
            if (colName == colBenefitGoal.Name)
            {
                if (dgTargetProgressNode.Rows[e.RowIndex].Cells[colBenefitGoal.Name].Value != null)
                {
                    string temp_quantity = dgTargetProgressNode.Rows[e.RowIndex].Cells[colBenefitGoal.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        dgTargetProgressNode.Rows[e.RowIndex].Cells[colBenefitGoal.Name].Value = "";
                    }
                }
                object quantity = dgTargetProgressNode.Rows[e.RowIndex].Cells[colBenefitGoal.Name].Value;
            }
            if (colName == colFigurativeProgress.Name)
            {
                if (dgTargetProgressNode.Rows[e.RowIndex].Cells[colFigurativeProgress.Name].Value != null)
                {
                    string temp_quantity = dgTargetProgressNode.Rows[e.RowIndex].Cells[colFigurativeProgress.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        dgTargetProgressNode.Rows[e.RowIndex].Cells[colFigurativeProgress.Name].Value = "";
                    }
                }
                object quantity = dgTargetProgressNode.Rows[e.RowIndex].Cells[colFigurativeProgress.Name].Value;
            }
        }


    }
}
