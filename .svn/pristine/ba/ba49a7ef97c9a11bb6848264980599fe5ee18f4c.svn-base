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
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatProcessMng.Domain;
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatApplyMng.Domain;
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
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using VirtualMachine.Core;
using System.IO;
using System.Diagnostics;
using IRPServiceModel.Domain.Document;
using NHibernate.Criterion;
using IRPServiceModel.Basic;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using Application.Business.Erp.SupplyChain.Client.FileUpload;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using Application.Business.Erp.SupplyChain.Util;

namespace Application.Business.Erp.SupplyChain.Client.WasteMaterialMng.WasteMaterialManage
{
    public partial class VWasteMaterialHandle : TMasterDetailView
    {
        private MWasteMaterialMng model = new MWasteMaterialMng();
        private WasteMatProcessMaster curBillMaster;
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空

        private ProObjectRelaDocument oprDocument = null;
        string userName = string.Empty;
        string jobId = string.Empty;
        string fileObjectType = string.Empty;
        CurrentProjectInfo projectInfo;

        /// <summary>
        /// 当前单据
        /// </summary>
        public WasteMatProcessMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VWasteMaterialHandle()
        {
            InitializeComponent();
            InitEvent();
            txtRecycleUnit.SupplierCatCode = CommonUtil.SupplierCatCode5 + "-" ;
        }


        private void InitEvent()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            this.dgDetail.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgDetail_EditingControlShowing);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.btnSameDate.Click+=new EventHandler(btnSameDate_Click);
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
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
            this.tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
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
                        curBillMaster = model.WasteMatSrv.saveWasteMatProcess(curBillMaster, movedDtlList);
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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name.Equals(tabPage2.Name))
            {
                dgDocumentMast.Rows.Clear();
                dgDocumentDetail.Rows.Clear();
                if (curBillMaster != null)
                {
                    if (curBillMaster.Id != null)
                    {
                        dgDocumentMast_CellClick(dgDocumentMast, new DataGridViewCellEventArgs(2, dgDocumentMast.SelectedRows[0].Index));
                    }
                }
            }
        }

        void dgDetail_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            object _DateControl = dgDetail.Controls["DataGridViewCalendarColumn"];
            if (_DateControl == null) return;
            DateTimePicker _DateTimePicker = (DateTimePicker)_DateControl;
            if (e.ColumnIndex == 6)//修改想要的列
            {
                Rectangle _Rectangle = dgDetail.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                _DateTimePicker.Size = new Size(_Rectangle.Width, _Rectangle.Height);
                _DateTimePicker.Location = new Point(_Rectangle.X, _Rectangle.Y);
                _DateTimePicker.Visible = true;
            }
            else
            {
                _DateTimePicker.Visible = false;
            }
        }

        void tsmiDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr = dgDetail.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                dgDetail.Rows.Remove(dr);
                if (dr.Tag != null)
                {
                    curBillMaster.Details.Remove(dr.Tag as WasteMatProcessDetail);
                }
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
                    curBillMaster = model.WasteMatSrv.GetWasteMatHandleById(Id);
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
                    this.dgDetail.EditMode = DataGridViewEditMode.EditOnEnter;
                    cmsDg.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
                    cmsDg.Enabled = false;
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
            object[] os = new object[] { txtCode, txtCreatePerson, txtHandlePerson, txtSumQuantity, txtProject, txtSumMoney, txtApplyCode };
            ObjectLock.Lock(os);
            string[] lockCols = new string[] { colMaterialName.Name, colMaterialSpec.Name, colUnit.Name, colRefQuantity.Name, colTotalValue.Name, colNetWeight.Name, colUserPart.Name};
            dgDetail.SetColumnsReadOnly(lockCols);
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
            else if (c is CustomDataGridView)
            {
                (c as CustomDataGridView).Rows.Clear();
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
                movedDtlList = new ArrayList();
                this.curBillMaster = new WasteMatProcessMaster();   
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;//制单人编号
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;//制单年
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;//制单月
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//登录业务组织
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录组织名称
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;//业务组织编号
                curBillMaster.HandOrgLevel = ConstObject.TheOperationOrg.Level;
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;//负责人
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;//负责人名称
                curBillMaster.DocState = DocumentState.Edit;//状态
                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //责任人
                txtHandlePerson.Tag = ConstObject.LoginPersonInfo;
                txtHandlePerson.Text = ConstObject.LoginPersonInfo.Name;
                //归属项目
                if (projectInfo != null)
                {
                    txtProject.Tag = projectInfo;
                    txtProject.Text = projectInfo.Name;
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
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
        /// 修改
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
            movedDtlList = new ArrayList();
            if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
            {
                base.ModifyView();
                curBillMaster = model.WasteMatSrv.GetWasteMatHandleById(curBillMaster.Id);
                ModelToView();
                FillDoc();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(curBillMaster.DocState));
            MessageBox.Show(message);
            return false;
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
                curBillMaster = model.WasteMatSrv.saveWasteMatProcess(curBillMaster, movedDtlList);
                txtCode.Text = curBillMaster.Code;
                LogData log = new LogData();
                log.BillId = curBillMaster.Id;
                log.BillType = "废旧物资处理单";
                log.Code = curBillMaster.Code;
                log.Descript = "";
                log.OperPerson = ConstObject.LoginPersonInfo.Name;
                log.ProjectName = curBillMaster.ProjectName;
                if (flag)
                {
                    log.OperType = "新增";
                }
                else
                {
                    log.OperType = "修改";
                }
                StaticMethod.InsertLogData(log);
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
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
        /// 提交
        /// </summary>
        /// <returns></returns>
        public override bool SubmitView()
        {
            try
            {
                curBillMaster.DocState = DocumentState.InExecute;
                if (!ViewToModel()) return false;               
                curBillMaster.AuditPerson = ConstObject.LoginPersonInfo;//制单人编号
                curBillMaster.AuditPersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                curBillMaster.AuditDate = ConstObject.LoginDate;//制单时间
                curBillMaster.AuditYear = ConstObject.LoginDate.Year;//制单年
                curBillMaster.AuditMonth = ConstObject.LoginDate.Month;//制单月
                curBillMaster.SubmitDate = ConstObject.LoginDate;//提交后修改业务日期
                curBillMaster = model.WasteMatSrv.saveWasteMatProcess(curBillMaster, movedDtlList);
                txtCode.Text = curBillMaster.Code;
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
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
                curBillMaster = model.WasteMatSrv.GetWasteMatHandleById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    if (!msrv.docSrv.DeleteDocument(curBillMaster.Id)) return false;
                    if (!model.WasteMatSrv.DeleteWasteMaterialProcessMaster(curBillMaster)) return false;
                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "废旧物资处理单";
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
                        curBillMaster = model.WasteMatSrv.GetWasteMatHandleById(curBillMaster.Id);
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
                curBillMaster = model.WasteMatSrv.GetWasteMatHandleById(curBillMaster.Id);
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

            //取得前驱单据的制单日期
            DateTime preCreateDate = model.WasteMatSrv.GetWasterSQCreateDate(curBillMaster.ForwardBillId);
            string validMessage = "";
            if (this.dgDetail.Rows.Count - 1 == 0)
            {
                MessageBox.Show("明细不能为空！");
                return false;
            }
            dgDetail.EndEdit();
            dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));
            if (validMessage != "")
            {
                MessageBox.Show(validMessage);
                return false;
            }
            //明细表数据校验
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                //最后一行不进行校验
                if (dr.IsNewRow) break;

                if (dr.Cells[colMaterialCode.Name].Tag == null)
                {
                    MessageBox.Show("物料不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colMaterialCode.Name];
                    return false;
                }
                if (dr.Cells[colUnit.Name].Tag == null)
                {
                    MessageBox.Show("计量单位不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colUnit.Name];
                    return false;
                }
                object forwardDtlId = dr.Cells[colForwardDtlId.Name].Value;
                WasteMatApplyDetail forwardDetail = model.WasteMatSrv.GetWasteProcessDetailById(forwardDtlId.ToString());
                if (forwardDetail == null)
                {
                    MessageBox.Show("未找到前续出库单明细,请重新引用。");
                    dgDetail.CurrentCell = dr.Cells[colRefQuantity.Name];
                    return false;
                }
                else
                {
                    decimal canUseQty = forwardDetail.Quantity - forwardDetail.RefQuantity;//可利用数量
                    decimal currentQty = ClientUtil.ToDecimal(dr.Cells[colNetWeight.Name].Value);//净重
                    object qtyTempObj = dr.Cells[colQueryTemp.Name].Value;//临时数量
                    decimal qtyTemp = 0;
                    if (qtyTempObj != null && !qtyTempObj.ToString().Equals(""))
                    {
                        qtyTemp = decimal.Parse(qtyTempObj.ToString());
                    }
                }
                
                if (ClientUtil.ToDateTime(dr.Cells[this.colProcessDate.Name].Value) < preCreateDate)
                {
                    MessageBox.Show("处理时间不能早于申请处理单的业务日期！");
                    dgDetail.CurrentCell = dr.Cells[colProcessDate.Name];
                    return false;
                }
                if (ClientUtil.ToDateTime(dr.Cells[this.colProcessDate.Name].Value) > ConstObject.LoginDate)
                {
                    MessageBox.Show("处理时间不能晚于当前日期！");
                    dgDetail.CurrentCell = dr.Cells[colProcessDate.Name];
                    return false;
                }
                if (dr.Cells[colRefQuantity.Name].Value == null)
                {
                    MessageBox.Show("计划数量不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colRefQuantity.Name];
                    return false;
                }

                if (dr.Cells[colPlaceNumber.Name].Value == null)
                {
                    MessageBox.Show("车牌号不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colPlaceNumber.Name];
                    return false;
                }
                
                if (dr.Cells[colTareWeight.Name].Value == null)
                {
                    MessageBox.Show("皮重不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colTareWeight.Name];
                    return false;
                }
                if (dr.Cells[colGrossWeight.Name].Value == null)
                {
                    MessageBox.Show("毛重不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colGrossWeight.Name];
                    return false;
                }
                if (dr.Cells[colProcessPrice.Name].Value == null || ClientUtil.TransToDecimal(dr.Cells[colProcessPrice.Name].Value) == 0)
                {
                    MessageBox.Show("单价不能为空或为0！");
                    dgDetail.CurrentCell = dr.Cells[colProcessPrice.Name];
                    return false;
                }
                if (curBillMaster.DocState == DocumentState.InExecute && dr.Cells[colReceiptCode.Name].Value == null)
                {
                    MessageBox.Show("收据编号不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colReceiptCode.Name];
                    return false;
                }
            }
            dgDetail.Update();
            return true;
        }

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                txtCode.Focus();
                curBillMaster.CreateDate = ClientUtil.ToDateTime(this.dtpDateBegin.Text);//业务时间
                curBillMaster.Descript = ClientUtil.ToString(this.txtDescript.Text);
                curBillMaster.ForwardBillCode = ClientUtil.ToString(this.txtApplyCode.Text);
                curBillMaster.ForwardBillId = ClientUtil.ToString(this.txtApplyCode.Tag);
                if (txtRecycleUnit.Result.Count > 0 && txtRecycleUnit.Text != "")
                {
                    curBillMaster.RecycleUnit = this.txtRecycleUnit.Result[0] as SupplierRelationInfo;
                    curBillMaster.RecycleUnitName = ClientUtil.ToString(this.txtRecycleUnit.Text);
                }
                else 
                {
                    MessageBox.Show("回收单位不能为空！");
                    return false;
                }
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    WasteMatProcessDetail curBillDtl = new WasteMatProcessDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as WasteMatProcessDetail;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.MaterialResource = var.Cells[colMaterialCode.Name].Tag as Application.Resource.MaterialResource.Domain.Material;
                    curBillDtl.MaterialName = ClientUtil.ToString(var.Cells[colMaterialName.Name].Value);
                    curBillDtl.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);
                    curBillDtl.MaterialStuff = ClientUtil.ToString(var.Cells[colStuff.Name].Value);//材质
                    curBillDtl.MatStandardUnitName = ClientUtil.ToString(var.Cells[colUnit.Name].Value);
                    curBillDtl.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);
                    curBillDtl.MatStandardUnit = var.Cells[colUnit.Name].Tag as StandardUnit;//计量单位
                    curBillDtl.PlateNumber = ClientUtil.ToString(var.Cells[colPlaceNumber.Name].Value);
                    curBillDtl.ReceiptCode = ClientUtil.ToString(var.Cells[colReceiptCode.Name].Value);
                    curBillDtl.TareWeight = ClientUtil.ToDecimal(var.Cells[colTareWeight.Name].Value);
                    curBillDtl.NetWeight = ClientUtil.ToDecimal(var.Cells[colNetWeight.Name].Value);
                    curBillDtl.GrossWeight = ClientUtil.ToDecimal(var.Cells[colGrossWeight.Name].Value);
                    curBillDtl.TotalValue = ClientUtil.ToDecimal(var.Cells[colTotalValue.Name].Value);
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                    curBillDtl.ProcessPrice = ClientUtil.ToDecimal(var.Cells[colProcessPrice.Name].Value);
                    curBillDtl.Quantity = ClientUtil.ToDecimal(var.Cells[colRefQuantity.Name].Value);//计划数量
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                    curBillDtl.Price = ClientUtil.TransToDecimal(var.Cells[colProcessPrice.Name].Value);
                    curBillDtl.ProcessDate = ClientUtil.ToDateTime(var.Cells[colProcessDate.Name].Value);//处理时间
                    //临时数量
                    curBillDtl.QuantityTemp = ClientUtil.TransToDecimal(var.Cells[colQueryTemp.Name].Value);//临时数量
                    //前驱明细Id
                    curBillDtl.UsedPart = var.Cells[colUserPart.Name].Tag as GWBSTree;
                    curBillDtl.UsedPartName = ClientUtil.ToString(var.Cells[colUserPart.Name].Value);
                    curBillDtl.UsedPartSysCode = ClientUtil.ToString((var.Cells[colUserPart.Name].Tag as GWBSTree).SysCode);
                    curBillDtl.ForwardDetailId = ClientUtil.ToString(var.Cells[colForwardDtlId.Name].Value);
                    curBillDtl.DiagramNumber = ClientUtil.ToString(var.Cells[colDiagramNum.Name].Value);
                    curBillMaster.AddDetail(curBillDtl);
                    var.Tag = curBillDtl;
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 在光标跳自动转到下一列时，首先执行校验，确定是否跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
        }

        /// <summary>
        /// 物料编码列增加事件监听，支持处理键盘回车查询操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox textBox = e.Control as TextBox;
            if (textBox != null)
            {
                textBox.PreviewKeyDown -= new PreviewKeyDownEventHandler(textBox_PreviewKeyDown);
                textBox.KeyPress -= new KeyPressEventHandler(textBox_KeyPress);

                if (this.dgDetail.Columns[this.dgDetail.CurrentCell.ColumnIndex].Name.Equals("MaterialCode"))
                {
                    textBox.PreviewKeyDown += new PreviewKeyDownEventHandler(textBox_PreviewKeyDown);
                    textBox.KeyPress += new KeyPressEventHandler(textBox_KeyPress);
                }
            }
        }

        /// <summary>
        /// 键盘回车查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (base.ViewState == MainViewState.Browser || base.ViewState == MainViewState.Initialize) return;

            if (e.KeyValue == 13)
            {
                CommonMaterial materialSelector = new CommonMaterial();
                TextBox textBox = sender as TextBox;
                if (textBox.Text != null && !textBox.Text.Equals(""))
                {
                    materialSelector.OpenSelect(textBox.Text);
                }
                else
                {
                    materialSelector.OpenSelect();
                }
                IList list = materialSelector.Result;

                if (list != null && list.Count > 0)
                {
                    Application.Resource.MaterialResource.Domain.Material selectedMaterial = list[0] as Application.Resource.MaterialResource.Domain.Material;
                    this.dgDetail.CurrentRow.Cells[colMaterialCode.Name].Tag = selectedMaterial;
                    this.dgDetail.CurrentCell.Value = selectedMaterial.Code;
                    this.dgDetail.CurrentRow.Cells[colMaterialName.Name].Value = selectedMaterial.Name;
                    this.dgDetail.CurrentRow.Cells[colMaterialSpec.Name].Value = selectedMaterial.Specification;
                    //动态分类复合单位                    
                    DataGridViewComboBoxCell cbo = this.dgDetail.CurrentRow.Cells[colUnit.Name] as DataGridViewComboBoxCell;
                    cbo.Items.Clear();
                    StandardUnit first = null;
                    foreach (StandardUnit cu in selectedMaterial.GetMaterialAllUnit())
                    {
                        cbo.Items.Add(cu.Name);
                    }
                    first = selectedMaterial.BasicUnit;
                    this.dgDetail.CurrentRow.Cells[colUnit.Name].Tag = first;
                    cbo.Value = first.Name;
                    this.dgDetail.RefreshEdit();
                }
            }
        }

        /// <summary>
        /// 在物料编码列，敲击键盘时，取消原来已经选择的物料，
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.dgDetail.CurrentCell.Tag = null;
            this.dgDetail.Rows[this.dgDetail.CurrentCell.RowIndex].Cells[colMaterialName.Name].Value = "";
            this.dgDetail.Rows[this.dgDetail.CurrentCell.RowIndex].Cells[colMaterialSpec.Name].Value = "";
        }

        /// <summary>
        /// 物料编码列，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (this.dgDetail.EditMode == DataGridViewEditMode.EditOnEnter)
            {
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colUserPart.Name))
                {
                    VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
                    frm.IsTreeSelect = true;
                    frm.ShowDialog();
                    List<TreeNode> list = frm.SelectResult;
                    if (list.Count > 0)
                    {
                        dgDetail.CurrentRow.Cells[colUserPart.Name].Tag = (list[0] as TreeNode).Tag as GWBSTree;
                        dgDetail.CurrentRow.Cells[colUserPart.Name].Value = (list[0] as TreeNode).Text;
                    }
                    this.txtCode.Focus();
                }
            }
        }

        //显示数据
        private bool ModelToView()
        {
            try
            {
                this.txtCode.Text = curBillMaster.Code;
                this.txtCode.Tag = curBillMaster;
                txtHandlePerson.Tag = curBillMaster.HandlePerson;
                txtHandlePerson.Text = curBillMaster.HandlePersonName;
                txtDescript.Text = curBillMaster.Descript;
                txtSumQuantity.Text = curBillMaster.SumQuantity.ToString();//总数量
                this.txtCreatePerson.Tag = curBillMaster.CreatePerson;
                txtCreatePerson.Text = curBillMaster.CreatePersonName;
                this.dtpDateBegin.Text = curBillMaster.CreateDate.ToShortDateString();
                if (curBillMaster.RecycleUnit != null)
                {
                    txtRecycleUnit.Result.Clear();
                    txtRecycleUnit.Result.Add(curBillMaster.RecycleUnit);
                }
                txtRecycleUnit.Value = curBillMaster.RecycleUnitName;
                txtApplyCode.Text = curBillMaster.ForwardBillCode;//前驱单号编号
                txtApplyCode.Tag = curBillMaster.ForwardBillId;//前驱单号Id
                txtProject.Text = curBillMaster.ProjectName;
                txtSumMoney.Text = curBillMaster.SumMoney.ToString("#,###.####");
                this.txtCreatePerson.Text = curBillMaster.CreatePersonName;
                this.dgDetail.Rows.Clear();
                foreach (WasteMatProcessDetail var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();
                    this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialResource.Code;
                    this.dgDetail[colMaterialName.Name, i].Value = var.MaterialResource.Name;
                    this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialResource.Specification;
                    this.dgDetail[colStuff.Name, i].Value = var.MaterialResource.Stuff;
                    this.dgDetail[colGrossWeight.Name, i].Value = var.GrossWeight;
                    this.dgDetail[colTareWeight.Name, i].Value = var.TareWeight;
                    this.dgDetail[colNetWeight.Name, i].Value = var.NetWeight;
                    this.dgDetail[colProcessPrice.Name, i].Value = var.Price;
                    this.dgDetail[colPlaceNumber.Name, i].Value = var.PlateNumber;
                    this.dgDetail[colReceiptCode.Name, i].Value = var.ReceiptCode;
                    this.dgDetail[colTotalValue.Name, i].Value = var.TotalValue;
                    this.dgDetail[colProcessPrice.Name, i].Value = var.ProcessPrice;
                    this.dgDetail[colRefQuantity.Name, i].Value = var.Quantity;
                    this.dgDetail[colForwardDtlId.Name, i].Value = var.ForwardDetailId;//将前驱编号保存到表中的不可视的列中
                    this.dgDetail[colProcessDate.Name, i].Value = var.ProcessDate;
                    this.dgDetail[colQueryTemp.Name, i].Value = var.NetWeight;//选中信息时将净重保存到临时数量中
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    this.dgDetail[colUserPart.Name,i].Value = var.UsedPartName;
                    this.dgDetail[colUserPart.Name, i].Tag = var.UsedPart;
                    //设置该物料的计量单位
                    this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
                    this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnit.Name;
                    this.dgDetail[colDiagramNum.Name, i].Value = var.DiagramNumber;
                    this.dgDetail.Rows[i].Tag = var;
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
        /// 修改表格信息操作
        /// </summary>
        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            bool flag = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            if (colName == colGrossWeight.Name)//修改毛重
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colGrossWeight.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colGrossWeight.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        this.dgDetail.Rows[e.RowIndex].Cells[colGrossWeight.Name].Value = "";
                        flag = false;
                    }
                }
                //皮重也不为空
                if (dgDetail.Rows[e.RowIndex].Cells[colTareWeight.Name].Value != null)
                {
                    decimal QuanGross = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colGrossWeight.Name].Value);
                    decimal QuanTare = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colTareWeight.Name].Value);
                    if(QuanGross < QuanTare)
                    {
                        MessageBox.Show("毛重不可大于皮重！");
                        return;
                    }
                    decimal quantity = QuanGross - QuanTare;
                    dgDetail.Rows[e.RowIndex].Cells[colNetWeight.Name].Value = quantity;
                    decimal sumqty = 0;
                    for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                    {
                        if (quantity == null) quantity = 0;
                        sumqty = sumqty + ClientUtil.TransToDecimal(quantity);
                    }
                    txtSumQuantity.Text = sumqty.ToString();
                }
            }
            if (colName == colTareWeight.Name)//修改皮重
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colTareWeight.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colTareWeight.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        this.dgDetail.Rows[e.RowIndex].Cells[colTareWeight.Name].Value = "";
                        flag = false;
                    }
                }
                //毛重也不为空
                if (dgDetail.Rows[e.RowIndex].Cells[colGrossWeight.Name].Value != null)
                {
                    decimal QuanGross = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colGrossWeight.Name].Value);
                    decimal QuanTare = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colTareWeight.Name].Value);
                    if (QuanGross < QuanTare)
                    {
                        MessageBox.Show("皮重不可大于毛重！");
                        return;
                    }
                    decimal quantity = 0;
                    if (QuanGross < 0)
                    {
                        quantity = -(QuanGross - QuanTare);
                    }
                    else
                    {
                        quantity = QuanGross - QuanTare;
                    }
                    dgDetail.Rows[e.RowIndex].Cells[colNetWeight.Name].Value = quantity;
                    decimal sumqty = 0;
                    for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                    {
                        quantity = ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colNetWeight.Name].Value);
                        if (quantity == null) quantity = 0;
                        sumqty = sumqty + ClientUtil.TransToDecimal(quantity);
                    }
                    txtSumQuantity.Text = sumqty.ToString();
                }
            }
            if (colName == colProcessPrice.Name)//修改单价
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colProcessPrice.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colProcessPrice.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        this.dgDetail.Rows[e.RowIndex].Cells[colProcessPrice.Name].Value = "";
                        flag = false;
                    }
                }
            }
            if (flag)
            {
                string strTotalValue = null;
                if (ClientUtil.ToString(dgDetail.Rows[e.RowIndex].Cells[colNetWeight.Name].Value) != "" && ClientUtil.ToString(dgDetail.Rows[e.RowIndex].Cells[colProcessPrice.Name].Value) != "")
                {
                    string strNetWeight = ClientUtil.ToString(dgDetail.Rows[e.RowIndex].Cells[colNetWeight.Name].Value);
                    string strProcessPrice = ClientUtil.ToString(dgDetail.Rows[e.RowIndex].Cells[colProcessPrice.Name].Value);
                    strTotalValue = ClientUtil.ToString(ClientUtil.ToDecimal(strNetWeight) * ClientUtil.ToDecimal(strProcessPrice));
                    dgDetail.Rows[e.RowIndex].Cells[colTotalValue.Name].Value = strTotalValue;
                }
                else
                {
                    strTotalValue = "0";
                    dgDetail.Rows[e.RowIndex].Cells[colTotalValue.Name].Value = strTotalValue;
                }
                object totalvalue = dgDetail.Rows[e.RowIndex].Cells[colTotalValue.Name].Value;
                decimal sumMoney = 0;
                for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                {
                    totalvalue = dgDetail.Rows[i].Cells[colTotalValue.Name].Value;
                    if (totalvalue == null) totalvalue = 0;
                    sumMoney = sumMoney + ClientUtil.TransToDecimal(totalvalue);
                }
                txtSumMoney.Text = sumMoney.ToString();
            }
        }

        public void btnSearch_Click(object sender, EventArgs e)
        {
            VWastematerialApplySelector vmros = new VWastematerialApplySelector();
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            WasteMatApplyMaster wasteMaster = list[0] as WasteMatApplyMaster;
            txtApplyCode.Tag = wasteMaster.Id;
            txtApplyCode.Text = wasteMaster.Code;
            ////处理旧明细
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                WasteMatApplyDetail dtl = dr.Tag as WasteMatApplyDetail;
                if (dtl != null)
                {
                    if (dtl.IsSelect == true)
                    {
                        if (CurBillMaster != null)
                        {
                            CurBillMaster.Details.Remove(dtl);
                            if (dtl.Id != null)
                            {
                                movedDtlList.Add(dtl);
                            }
                        }
                    }
                }
            }
            ////显示引用的明细
            this.dgDetail.Rows.Clear();
            foreach (WasteMatApplyDetail var in wasteMaster.Details)
            {
                if (var.IsSelect == false) continue;
                int i = this.dgDetail.Rows.Add();
                this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;
                this.dgDetail[colStuff.Name, i].Value = var.MaterialStuff;
                this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
                this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnitName;
                this.dgDetail[colDescript.Name, i].Value = var.Descript;
                this.dgDetail[colTotalValue.Name, i].Value = (var.Quantity - var.RefQuantity) * var.Price;
                this.dgDetail[colForwardDtlId.Name, i].Value = var.Id;
                this.dgDetail[colProcessDate.Name, i].Value = DateTime.Now;
                this.dgDetail[colRefQuantity.Name, i].Value = var.Quantity;
                this.dgDetail[colUserPart.Name, i].Value = var.UsedPartName;
                this.dgDetail[colUserPart.Name, i].Tag = var.UsedPart;
                this.dgDetail[colDiagramNum.Name, i].Value = var.DiagramNumber;
            }

        }

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            if (LoadTempleteFile(@"废旧物资处理单.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.PrintPreview(true,true,true,0,0,0,0,0);
            return true;
        }

        public override bool Print()
        {
            if (LoadTempleteFile(@"废旧物资处理单.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.Print();
            curBillMaster.PrintTimes = curBillMaster.PrintTimes + 1;
            curBillMaster = model.WasteMatSrv.saveWasteMatProcess(curBillMaster, movedDtlList);
            return true;
        }

        public override bool Export()
        {
            if (LoadTempleteFile(@"废旧物资处理单.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.ExportToExcel("废旧物资处理单【" + curBillMaster.Code + "】", false, false, true);
            return true;
        }

        private bool LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式和数据
                flexGrid1.OpenFile(path + "\\" + modelName);//载入格式
            }
            else
            {
                MessageBox.Show("未找到模板格式文件【" + modelName + "】");
                return false;
            }
            return true;
        }

        private void FillFlex(WasteMatProcessMaster billMaster)
        {
            int detailStartRowNumber = 6;//6为模板中的行号
            int detailCount = billMaster.Details.Count;

            //插入明细行
            flexGrid1.InsertRow(detailStartRowNumber, detailCount);

            //设置单元格的边框，对齐方式
            FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
            range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            range.Mask = FlexCell.MaskEnum.Digital;
            CommonUtil.SetFlexGridPrintFace(this.flexGrid1);
            //主表数据

            flexGrid1.Cell(3, 2).Text = billMaster.Code;
            flexGrid1.Cell(3, 6).Text = billMaster.ForwardBillCode;
            flexGrid1.Cell(3, 11).Text = billMaster.OperOrgInfoName;
            flexGrid1.Cell(4, 2).Text = billMaster.RealOperationDate.ToShortDateString();
            flexGrid1.Cell(4,6).Text = billMaster.RecycleUnitName;

            //填写明细数据
            decimal sumQuantity = 0;
            decimal sumTareWeight = 0;
            decimal sumGrossWeight = 0;
            decimal sumNetWeight = 0;
            decimal sumMoney = 0;
            for (int i = 0; i < detailCount; i++)
            {
                WasteMatProcessDetail detail = (WasteMatProcessDetail)billMaster.Details.ElementAt(i);
                //物资名称
                //string a1 = detail.MaterialResource.CreatedDate.ToString();
                flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialName;//detail.MaterialResource.Name;
                flexGrid1.Cell(detailStartRowNumber + i, 1).WrapText = true;
                //规格型号
                flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialSpec;//detail.MaterialResource.Specification;

                //计量单位
                flexGrid1.Cell(detailStartRowNumber + i, 3).Text = detail.MatStandardUnitName;

                //计划数量
                flexGrid1.Cell(detailStartRowNumber + i, 4).Text = detail.Quantity.ToString();
                sumQuantity += detail.Quantity;

                //皮重
                flexGrid1.Cell(detailStartRowNumber + i, 5).Text = detail.TareWeight.ToString();
                sumTareWeight += detail.TareWeight;
                //毛重
                flexGrid1.Cell(detailStartRowNumber + i, 6).Text = detail.GrossWeight.ToString();
                sumGrossWeight += detail.GrossWeight;

                //净重
                flexGrid1.Cell(detailStartRowNumber + i, 7).Text = detail.NetWeight.ToString();
                sumNetWeight += detail.NetWeight;

                //单价
                flexGrid1.Cell(detailStartRowNumber + i, 8).Text = detail.ProcessPrice.ToString();
                flexGrid1.Cell(detailStartRowNumber + i, 8).WrapText = true;

                //金额
                flexGrid1.Cell(detailStartRowNumber + i, 9).Text = detail.TotalValue.ToString();
                sumMoney += detail.TotalValue;
                flexGrid1.Cell(detailStartRowNumber + i, 9).WrapText = true;

                //车牌号
                flexGrid1.Cell(detailStartRowNumber + i, 10).Text = detail.PlateNumber.ToString();

                //收据号
                if (!string.IsNullOrEmpty(detail.ReceiptCode))
                {
                    flexGrid1.Cell(detailStartRowNumber + i, 11).Text = detail.ReceiptCode.ToString();
                }

                //备注
                flexGrid1.Cell(detailStartRowNumber + i, 12).Text = ClientUtil.ToString(detail.Descript);
                flexGrid1.Cell(detailStartRowNumber + i, 12).WrapText = true;
                if (i == detailCount - 1)
                {
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 4).Text = ClientUtil.ToString(sumQuantity);
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 5).Text = ClientUtil.ToString(sumTareWeight);
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 6).Text = ClientUtil.ToString(sumGrossWeight);
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 7).Text = ClientUtil.ToString(sumNetWeight);
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 9).Text = ClientUtil.ToString(sumMoney);
                    flexGrid1.Cell(detailStartRowNumber + i+1, 9).WrapText = true;
                    string Moneybig = CurrencyComUtil.GetMoneyChinese(sumMoney.ToString());
                    flexGrid1.Cell(detailStartRowNumber + i + 2, 2).Text = Moneybig;
                }
                string a = Application.Business.Erp.SupplyChain.Client.Util.DateUtil.GetStrMoneyInchar(ClientUtil.ToString(sumMoney));
                this.flexGrid1.Cell(1, 10).Text = billMaster.Code.Substring(billMaster.Code.Length - 11) + "-" + a;
                this.flexGrid1.Cell(1, 10).CellType = FlexCell.CellTypeEnum.BarCode;
                this.flexGrid1.Cell(1, 10).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;
                flexGrid1.Row(detailStartRowNumber + i).AutoFit();
            }
            flexGrid1.Cell(8 + detailCount, 2).Text = billMaster.ProjectName;
            flexGrid1.Cell(8 + detailCount, 8).Text = billMaster.CreateDate.ToShortDateString();
            flexGrid1.Cell(8 + detailCount, 11).Text = billMaster.CreatePersonName;
        }

        private void btnSameDate_Click(object sender, EventArgs e)
        {
            string sValue = string.Empty;
            if (dgDetail.Rows.Count > 0 && !this.dgDetail.Rows[0].IsNewRow)
            {
                sValue = (this.dgDetail.Rows[0].Cells[colProcessDate.Name].Value == null ? string.Empty : this.dgDetail.Rows[0].Cells[colProcessDate.Name].Value.ToString());
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    var.Cells[colProcessDate.Name].Value = ClientUtil.ToDateTime(sValue).ToShortDateString();
                }
            }

        }
    }
}
