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
using Application.Business.Erp.SupplyChain.ProductionManagement.PenaltyDeductionManagement.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Windows.Media.Media3D;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using System.Collections;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using System.IO;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using IRPServiceModel.Domain.Document;
using Application.Business.Erp.SupplyChain.Client.FileUpload;
using IRPServiceModel.Basic;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatMng;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionRecordMng;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.RectificationNoticeMng.Domain;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.ProfessionInspectionRecordMng;
using NHibernate;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement
{
    public partial class VTempDebitMng : TMasterDetailView
    {
        private MPenaltyDeductionMng model = new MPenaltyDeductionMng();
        private MStockMng stockModel = new MStockMng();
        private PenaltyDeductionMaster curBillMaster;
        private PenaltyDeductionDetail detail;
        CurrentProjectInfo projectInfo;
        string userName = string.Empty;
        string jobId = string.Empty;
        string fileObjectType = string.Empty;
        CostAccountSubject subject = new CostAccountSubject();
        /// <summary>
        /// 核算节点集合
        /// </summary>
        private List<TreeNode> ListAccountGWBSNodes = new List<TreeNode>();
        // 整改通知单主表与子表
        private RectificationNoticeMaster noticeMaster;
        private RectificationNoticeDetail noticeDetail;

        public PenaltyDeductionMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }
        public void InitData()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
        }

        public VTempDebitMng()
        {
            InitializeComponent();
            InitEvent();
            InitData();
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
            // 增加
            btnTeam.Click += new EventHandler(btnTeam_Click);
            btnSubject.Click += new EventHandler(btnSubject_Click);
            zgSelect.Click += new EventHandler(btnType_Click);
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
                        curBillMaster = model.SavePenaltyDeduction(curBillMaster);
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
                    curBillMaster = model.PenaltyDeductionSrv.GetPenaltyDeductionById(Id);
                    detail = curBillMaster.Details.FirstOrDefault() as PenaltyDeductionDetail;
                    noticeMaster = GetNotice(curBillMaster.Id);
                    noticeDetail = noticeMaster.Details.FirstOrDefault() as RectificationNoticeDetail;
                    ModelToView();
                    RefreshState(MainViewState.Browser);
                    btnDownLoadDocument.Enabled = true;
                    btnOpenDocument.Enabled = true;
                    //判断是否为制单人
                    PersonInfo pi = this.txtCreatePerson.Tag as PersonInfo;
                    string perid = ConstObject.LoginPersonInfo.Id;
                    if (pi != null && !pi.Id.Equals(perid))
                    {
                        RefreshStateByQuery();
                    }
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
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
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
                ObjectLock.Unlock(pnlFloor, true);
                btnStates(1);
            }
            else
            {
                ObjectLock.Lock(pnlFloor, true);
                btnStates(0);
            }
            //永久锁定
            object[] os = new object[] { txtPenaltyRank, txtCode, txtCreatePerson, txtHandlePerson, txtProject, txtWork, txtType, txtSubject, zgDangerArea, zgDangerLevel, zgDangerType, zgSelect };
            ObjectLock.Lock(os);
        }

        //清空数据
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
            else if (c is TextBox)
            {
                c.Tag = null;
                c.Text = "";
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
                //归属项目
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                base.NewView();
                ClearView();
                curBillMaster = new PenaltyDeductionMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.CreateDate = ConstObject.LoginDate;
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                curBillMaster.HandOrgLevel = ConstObject.TheOperationOrg.Level;
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.DocState = DocumentState.Edit;
                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //责任人
                txtHandlePerson.Tag = ConstObject.LoginPersonInfo;
                txtHandlePerson.Text = ConstObject.LoginPersonInfo.Name;

                if (projectInfo != null)
                {
                    txtProject.Tag = projectInfo;
                    txtProject.Text = projectInfo.Name;
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
                }

                detail = new PenaltyDeductionDetail();
                detail.Master = curBillMaster;
                curBillMaster.AddDetail(detail);
                // 创建整改通知单
                noticeMaster = new RectificationNoticeMaster();
                noticeDetail = new RectificationNoticeDetail();
                CreateNotice(noticeMaster, new RectificationNoticeDetail[] { noticeDetail });
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
            if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
            {
                base.ModifyView();
                curBillMaster = model.PenaltyDeductionSrv.GetPenaltyDeductionById(curBillMaster.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(curBillMaster.DocState));
            MessageBox.Show(message);
            return false;
        }

        //[optrType=1 保存][optrType=2 提交]
        private bool SaveOrSubmitBill(int optrType)
        {
            if (!ViewToModel())
                return false;

            LogData log = new LogData();
            if (string.IsNullOrEmpty(curBillMaster.Id))
            {
                if (optrType == 2)
                {
                    log.OperType = "新增提交";
                }
                else
                {
                    log.OperType = "新增保存";
                }
            }
            else
            {
                if (optrType == 2)
                {
                    log.OperType = "修改提交";
                }
                else
                {
                    log.OperType = "修改保存";
                }
            }
            if (optrType == 2)
            {
                curBillMaster.DocState = DocumentState.InExecute;
                curBillMaster.AuditPerson = ConstObject.LoginPersonInfo;//制单人编号
                curBillMaster.AuditPersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                curBillMaster.AuditDate = ConstObject.LoginDate;//制单时间
                curBillMaster.AuditYear = ConstObject.LoginDate.Year;//制单年
                curBillMaster.AuditMonth = ConstObject.LoginDate.Month;//制单月
            }
            curBillMaster.LastModifyDate = DateTime.Now.Date;
            EditParentState(2);         // 修改检查单的状态
            curBillMaster = model.SavePenaltyDeduction(curBillMaster);

            // 整改通知数据映射
            Notice_ViewToModel(noticeMaster, noticeDetail);
            if (optrType == 2)
            {
                SubmitNotice(noticeMaster);
            }
            else
            {
                SaveNotice(noticeMaster);
            }
            //#region 短信
            if (optrType == 2)
            {
                MAppPlatMng appModel = new MAppPlatMng();
                appModel.SendMessage(curBillMaster.Id, "PenaltyDeductionMaster");
            }
            this.txtCode.Text = curBillMaster.Code;
            log.BillId = curBillMaster.Id;
            log.BillType = "暂扣款单维护";
            log.Code = curBillMaster.Code;
            log.Descript = "";
            log.OperPerson = ConstObject.LoginPersonInfo.Name;
            log.ProjectName = curBillMaster.ProjectName;
            StaticMethod.InsertLogData(log);
            this.ViewCaption = ViewName + "-" + txtCode.Text;
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
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    if (SaveOrSubmitBill(1) == false) return false;
                    MessageBox.Show("保存成功！");
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能保存！");
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        public override bool SubmitView()
        {
            try
            {
                if (curBillMaster.DocState == DocumentState.Edit)
                {
                    if (SaveOrSubmitBill(2) == false) return false;
                    MessageBox.Show("提交成功！");
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能提交！");
                return false;
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
                curBillMaster = model.PenaltyDeductionSrv.GetPenaltyDeductionById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Edit)
                {
                    if (!msrv.DeleteReceiptAndDocument(curBillMaster, curBillMaster.Id)) return false;
                    EditParentState(1);         // 修改检查单的状态
                    DeleteNotice(noticeMaster); // 删除整改通知单
                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "暂扣款单维护";
                    log.Code = curBillMaster.Code;
                    log.OperType = "删除";
                    log.Descript = "";
                    log.OperPerson = ConstObject.LoginPersonInfo.Name;
                    log.ProjectName = curBillMaster.ProjectName;
                    StaticMethod.InsertLogData(log);
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
                        curBillMaster = model.PenaltyDeductionSrv.GetPenaltyDeductionById(curBillMaster.Id);
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

        /// <summary>
        /// 刷新
        /// </summary>
        public override void RefreshView()
        {
            try
            {
                //重新获得当前对象的值
                curBillMaster = model.PenaltyDeductionSrv.GetPenaltyDeductionById(curBillMaster.Id);
                //给界面赋值
                ModelToView();
            }
            catch (Exception e)
            {
                MessageBox.Show("数据刷新错误：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// 保存数据前校验数据
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            if (txtPenaltyRank.Text == "" || txtPenaltyRank.Tag == null)
            {
                MessageBox.Show("请选择检查单据！");
                return false;
            }
            if (string.IsNullOrEmpty(txtSubject.Text))
            {
                MessageBox.Show("请选择用工科目");
                return false;
            }
            if (string.IsNullOrEmpty(txtMoney.Text))
            {
                MessageBox.Show("请输入扣款金额");
                return false;
            }
            decimal money = 0;
            if (!decimal.TryParse(txtMoney.Text, out money))
            {
                MessageBox.Show("扣款金额必须为数字");
                return false;
            }
            if (string.IsNullOrEmpty(cbDeditType.Text))
            {
                MessageBox.Show("请选择扣款类型");
                return false;
            }
            if (string.IsNullOrEmpty(zgProblem.Text))
            {
                MessageBox.Show("请输入存在问题");
                return false;
            }
            if (string.IsNullOrEmpty(zgRequirement.Text))
            {
                MessageBox.Show("请输入整改要求");
                return false;
            }
            if (string.IsNullOrEmpty(zgNote.Text))
            {
                MessageBox.Show("请输入整改措施与结果说明");
                return false;
            }
            if (string.IsNullOrEmpty(zgResult.Text))
            {
                MessageBox.Show("请选择整改结论");
                return false;
            }
            return true;
        }

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                // 获取名称为“项”的标准单位对象
                string strUnit = "项";
                Application.Resource.MaterialResource.Domain.StandardUnit Unit = null;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Name", strUnit));
                IList lists = model.PenaltyDeductionSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq);
                if (lists != null && lists.Count > 0)
                {
                    Unit = lists[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                }
                // 获取名称为“元”的标准单位对象
                string strPriceUnit = "元";
                Application.Resource.MaterialResource.Domain.StandardUnit PriceUnit = null;
                ObjectQuery oq1 = new ObjectQuery();
                oq1.AddCriterion(Expression.Eq("Name", strPriceUnit));
                IList list = model.PenaltyDeductionSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq1);
                if (list != null && list.Count > 0)
                {
                    PriceUnit = list[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                }
                // 劳务分包资源
                string strResource = "劳务分包资源";
                Application.Resource.MaterialResource.Domain.Material theMaterial = null;
                ObjectQuery oq2 = new ObjectQuery();
                oq2.AddCriterion(Expression.Eq("Name", strResource));
                IList lst = model.PenaltyDeductionSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.Material), oq2);
                if (lst != null && lst.Count > 0)
                {
                    theMaterial = lst[0] as Application.Resource.MaterialResource.Domain.Material;
                }
                // 主表
                curBillMaster.Descript = ClientUtil.ToString(this.txtRemark.Text);//备注
                curBillMaster.PenaltyDeductionReason = "暂扣款";
                curBillMaster.PenaltyType = EnumUtil<PenaltyDeductionType>.FromDescription("暂扣款");
                curBillMaster.PenaltyDeductionRant = txtPenaltyRank.Tag as SubContractProject;
                curBillMaster.PenaltyDeductionRantName = txtPenaltyRank.Text;//罚扣队伍名称
                curBillMaster.CheckOrderId = parentId;               // 暂扣款单存储检查id
                // 子表
                detail.BusinessDate = dtTime.Value;
                detail.PenaltyMoney = ClientUtil.ToDecimal(txtMoney.Text);
                detail.PenaltySubjectGUID = txtSubject.Tag as CostAccountSubject;
                detail.PenaltySubject = txtSubject.Text;        // 用工科目名称
                detail.PenaltySysCode = detail.PenaltySubjectGUID.SysCode; //用工科目层次码
                detail.ResourceSysCode = theMaterial.IfCatResource.ToString();
                detail.ResourceType = theMaterial;
                detail.ResourceTypeName = theMaterial.Name;
                detail.ResourceTypeSpec = theMaterial.Specification;
                detail.ResourceTypeStuff = theMaterial.Stuff;
                detail.PenaltyType = cbDeditType.Text;
                detail.ProjectTask = txtWork.Tag as GWBSTree;
                detail.ProjectTaskSyscode = detail.ProjectTask.SysCode;
                detail.ProjectTaskName = txtWork.Text;
                detail.Cause = txtReason.Text;
                detail.ProductUnit = Unit;
                detail.ProductUnitName = Unit.Name;
                detail.MoneyUnit = PriceUnit;
                detail.MoneyUnitName = PriceUnit.Name;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        //显示数据
        private bool ModelToView()
        {
            try
            {
                // 主表信息
                txtCode.Text = curBillMaster.Code;
                txtHandlePerson.Tag = curBillMaster.HandlePerson;
                txtHandlePerson.Text = curBillMaster.HandlePersonName;
                txtRemark.Text = curBillMaster.Descript;
                txtCreatePerson.Tag = curBillMaster.CreatePerson;
                txtCreatePerson.Text = curBillMaster.CreatePersonName;
                txtCreatePerson.Text = curBillMaster.CreatePersonName;
                txtProject.Text = curBillMaster.ProjectName;
                txtPenaltyRank.Tag = curBillMaster.PenaltyDeductionRant;
                txtPenaltyRank.Text = curBillMaster.PenaltyDeductionRantName;
                parentId = curBillMaster.CheckOrderId;
                // 子表信息
                txtWork.Text = detail.ProjectTaskName;
                txtWork.Tag = detail.ProjectTask;
                txtType.Text = detail.ResourceTypeName;
                txtSubject.Text = detail.PenaltySubject;
                txtSubject.Tag = detail.PenaltySubjectGUID;
                txtMoney.Text = detail.PenaltyMoney.ToString();
                cbDeditType.Text = detail.PenaltyType;
                dtTime.Value = detail.BusinessDate;
                txtReason.Text = detail.Cause;
                parentCode = noticeDetail.ForwordCode;

                // 整改信息
                zgProblem.Text = noticeDetail.QuestionState;
                zgRequirement.Text = noticeDetail.Rectrequired;
                zgDangerArea.Text = noticeDetail.DangerPart;
                zgDangerType.Text = noticeDetail.DangerType;
                zgDangerLevel.Text = noticeDetail.DangerLevel;
                zgNote.Text = noticeDetail.RectContent;
                zgTime.Value = noticeDetail.RectDate;
                zgResult.SelectedIndex = noticeDetail.RectConclusion;

                // 加载文档
                FillDoc();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        #region 增加

        private string parentId;
        private string parentCode;
        /// <summary>
        /// 选择扣款队伍
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnTeam_Click(object sender, EventArgs e)
        {
            VInspectionSelector frm = new VInspectionSelector(new PersonInfo(), new SubContractProject());
            frm.ShowDialog();
            IList list = frm.Result;
            if (list.Count > 1)
            {
                MessageBox.Show("只能选择一条检查，请重新选择！");
                return;
            }
            else if (list.Count == 0) return;

            var item = list[0] as InspectionRecord;
            btnTeam.Tag = item;
            txtHandlePerson.Text = item.CreatePersonName;
            txtPenaltyRank.Text = item.BearTeamName;
            txtPenaltyRank.Tag = item.BearTeam;
            txtWork.Tag = item.GWBSTree;            // 工程任务
            txtWork.Text = item.GWBSTreeName;       // 工程任务名称
            txtType.Text = "劳务分包资源";
            txtReason.Text = item.InspectionStatus;
            parentId = item.Id;
            parentCode = item.Code;
        }
        /// <summary>
        /// 选择检查用工科目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSubject_Click(object sender, EventArgs e)
        {

            VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
            frm.IsSubBalanceFlag = true;
            frm.ShowDialog();
            CostAccountSubject cost = frm.SelectAccountSubject;
            if (cost != null)
            {
                txtSubject.Text = cost.Name;    // 用工科目节点名称
                txtSubject.Tag = cost;          // 用工科目
            }
        }
        /// <summary>
        /// 修改检查单的状态，如果是保存，标识码改为2；如果是删除，标识码改为1；
        /// </summary>
        private void EditParentState(int sign)
        {
            var oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", curBillMaster.CheckOrderId));
            var parentRecord = model.ObjectQuery(typeof(InspectionRecord), oq)[0] as InspectionRecord;
            parentRecord.CorrectiveSign = sign;
            model.PenaltyDeductionSrv.UpdateByDao(parentRecord);
        }

        #region 整改通知
        private MRectificationNoticeMng noticeService = new MRectificationNoticeMng();
        /// <summary>
        /// 选择隐患类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnType_Click(object sender, EventArgs e)
        {
            VDangerTypeSelector select = new VDangerTypeSelector(zgDangerType.Text);
            select.ShowDialog();
            zgDangerType.Text = select.Result;
        }
        /// <summary>
        /// 新建整改通知
        /// </summary>
        private void CreateNotice(RectificationNoticeMaster master, RectificationNoticeDetail[] detailArr)
        {
            // 主表对象初始化
            master.CreatePerson = ConstObject.LoginPersonInfo;
            master.CreatePersonName = ConstObject.LoginPersonInfo.Name;
            master.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;
            master.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
            master.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
            master.HandOrgLevel = ConstObject.TheOperationOrg.Level;
            master.HandlePerson = ConstObject.LoginPersonInfo;
            master.HandlePersonName = ConstObject.LoginPersonInfo.Name;
            master.DocState = DocumentState.Edit;
            master.ProjectId = projectInfo.Id;
            master.ProjectName = projectInfo.Name;

            // 将子表班定到主表上
            for (int i = 0; i < detailArr.Length; i++)
            {
                detailArr[i].Master = master;
                master.AddDetail(detailArr[i]);
            }
        }
        /// <summary>
        /// 将用户输入保存到单据对象中
        /// </summary>
        private void Notice_ViewToModel(RectificationNoticeMaster master, RectificationNoticeDetail detail)
        {
            // 主表数据映射
            master.SupplierUnitName = txtPenaltyRank.Text;
            master.SupplierUnit = txtPenaltyRank.Tag as SubContractProject;
            master.Descript = txtRemark.Text;
            master.InspectionType = 0;
            master.TempDebitId = curBillMaster.Id;
            // 子表数据映射
            detail.QuestionState = zgProblem.Text;
            detail.ForwordCode = parentCode;
            detail.RequiredDate = zgTime.Value;
            detail.DangerPart = zgDangerArea.Text;
            detail.DangerLevel = zgDangerLevel.Text;
            detail.DangerType = zgDangerType.Text;
            detail.RectContent = zgNote.Text;
            detail.RectDate = zgTime.Value;
            detail.Rectrequired = zgRequirement.Text;
            detail.RectConclusion = zgResult.SelectedIndex;
        }
        /// <summary>
        /// 保存整改通知
        /// </summary>
        /// <param name="master"></param>
        /// <returns></returns>
        private RectificationNoticeMaster SaveNotice(RectificationNoticeMaster master)
        {
            master = noticeService.RectificationNoticeSrv.SaveRectificationNotice(master);
            return master;
        }
        /// <summary>
        /// 提交整改通知
        /// </summary>
        /// <param name="master"></param>
        /// <returns></returns>
        private RectificationNoticeMaster SubmitNotice(RectificationNoticeMaster master)
        {
            master.DocState = DocumentState.InExecute;
            master.CreateDate = ConstObject.LoginDate;
            master.CreateYear = ConstObject.LoginDate.Year;
            master.CreateMonth = ConstObject.LoginDate.Month;
            master.AuditPerson = ConstObject.LoginPersonInfo;//制单人编号
            master.AuditPersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
            master.AuditDate = ConstObject.LoginDate;//制单时间
            master.AuditYear = ConstObject.LoginDate.Year;//制单年
            master.AuditMonth = ConstObject.LoginDate.Month;//制单月
            return SaveNotice(master);
        }
        /// <summary>
        /// 删除整改单
        /// </summary>
        /// <param name="master"></param>
        /// <returns></returns>
        private void DeleteNotice(RectificationNoticeMaster master)
        {
            msrv.DeleteReceiptAndDocument(master, master.Id);
        }
        /// <summary>
        /// 根据暂扣款id获取整改通知的信息
        /// </summary>
        /// <param name="id">暂扣款id</param>
        /// <returns></returns>
        public RectificationNoticeMaster GetNotice(string id)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("TempDebitId", id));
            oQuery.AddFetchMode("Details", FetchMode.Eager);
            IList lst = model.ObjectQuery(typeof(RectificationNoticeMaster), oQuery);
            return lst == null || lst.Count == 0 ? null : lst[0] as RectificationNoticeMaster;
        }
        #endregion

        #endregion

    }
}
