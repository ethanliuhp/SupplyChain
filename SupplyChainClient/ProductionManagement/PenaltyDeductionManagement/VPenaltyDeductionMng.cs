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

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement
{
    public partial class VPenaltyDeductionMng : TMasterDetailView
    {
        private MPenaltyDeductionMng model = new MPenaltyDeductionMng();
        private MStockMng stockModel = new MStockMng();
        private PenaltyDeductionMaster curBillMaster;
        CurrentProjectInfo projectInfo;
        string userName = string.Empty;
        string jobId = string.Empty;
        string fileObjectType = string.Empty;
        CostAccountSubject subject = new CostAccountSubject();
        /// <summary>
        /// 核算节点集合
        /// </summary>
        private List<TreeNode> ListAccountGWBSNodes = new List<TreeNode>();

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

        public VPenaltyDeductionMng()
        {
            InitializeComponent();
            InitEvent();
            InitDate();
            InitPlanType();
            InitDate();
        }

        private void InitPlanType()
        {
            //罚款类型
            colPenaltyType.Items.AddRange(new object[] { "安全施工", "文明施工", "物资专业", "质量专业" });
        }

        public void InitDate()
        {
            DateTimePicker dp = new DateTimePicker();
            DataGridViewCalendarColumn dc = new DataGridViewCalendarColumn();
            dp.CustomFormat = "yyyy-MM-dd";
            dp.Format = DateTimePickerFormat.Custom;
            dp.Visible = false;
            dgDetail.Controls.Add(dp);
            dp = new DateTimePicker();
        }

        private void InitEvent()
        {
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            flexGrid1.PrintPage += new FlexCell.Grid.PrintPageEventHandler(btnPrintPage_Click);
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
            //右键删除菜单
            this.cmsDg.ItemClicked += new ToolStripItemClickedEventHandler(cmsDg_ItemClicked);
            dgDetail.MouseClick += new MouseEventHandler(dgDetail_MouseClick);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
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
        void dgDetail_MouseClick(object sender, MouseEventArgs e)
        {
            if (!dgDetail.ReadOnly)
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (dgDetail.Enabled)
                    {
                        cmsDg.Items[tsmiDel.Name].Enabled = true;
                        cmsDg.Items[tsmiPaste.Name].Enabled = true;
                        cmsDg.Show(MousePosition.X, MousePosition.Y);
                    }
                }
            }
        }

        void cmsDg_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (this.dgDetail.Rows.Count - 1 == 0)
            {
                MessageBox.Show("明细为空,没有可复制粘贴的信息！");
                return;
            }
            if (e.ClickedItem.Text.Trim() == "复制科目")
            {
                cmsDg.Hide();
                subject = new CostAccountSubject();
                if (dgDetail.CurrentRow.Cells[colPenaltySubject.Name].Tag != null)
                {
                    subject = dgDetail.CurrentRow.Cells[colPenaltySubject.Name].Tag as CostAccountSubject;
                }
                else
                {
                    MessageBox.Show("未能复制任何信息！");
                }
            }
            if (e.ClickedItem.Text.Trim() == "粘贴科目")
            {
                cmsDg.Hide();
                if (subject.Id != null)
                {
                    dgDetail.CurrentRow.Cells[colPenaltySubject.Name].Tag = subject;
                    dgDetail.CurrentRow.Cells[colPenaltySubject.Name].Value = subject.Name;
                }
                else
                {
                    MessageBox.Show("没有可粘贴的信息！");
                }
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
                    curBillMaster.Details.Remove(dr.Tag as PenaltyDeductionDetail);
                }
            }
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            VContractExcuteSelector vmros = new VContractExcuteSelector();
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject engineerMaster = list[0] as SubContractProject;
            txtPenaltyRank.Text = engineerMaster.BearerOrgName;
            txtPenaltyRank.Tag = engineerMaster;

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
                    curBillMaster = model.PenaltyDeductionSrv.GetPenaltyDeductionById(Id);
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
                    //this.txtPenaltyReason.Enabled = true;
                    this.btnSearch.Enabled = true;
                    this.dgDetail.EditMode = DataGridViewEditMode.EditOnEnter;
                    cmsDg.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    //this.txtPenaltyReason.Enabled = false;
                    this.btnSearch.Enabled = false;
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
            object[] os = new object[] { txtPenaltyRank, txtCode, txtCreatePerson, txtHandlePerson, txtProject, txtSumMoney };
            ObjectLock.Lock(os);
            string[] lockCols = new string[] { colProjectType.Name, colProjectDetail.Name, colPenaltySubject.Name, colMaterialType.Name };
            dgDetail.SetColumnsReadOnly(lockCols);
        }

        //清空数据
        private void ClearView()
        {
            ClearControl(pnlContent);
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
                //归属项目
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                base.NewView();
                ClearView();
                this.curBillMaster = new PenaltyDeductionMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.CreateDate = ConstObject.LoginDate;
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                curBillMaster.HandOrgLevel = ConstObject.TheOperationOrg.Level;
                if (projectInfo.HandlePerson != null)
                {
                    curBillMaster.HandlePerson = projectInfo.HandlePerson;
                    curBillMaster.HandlePersonName = projectInfo.HandlePersonName;
                }
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
                //txtContractNo.Focus();
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
                curBillMaster.DocState = DocumentState.InAudit;
                curBillMaster.AuditPerson = ConstObject.LoginPersonInfo;//制单人编号
                curBillMaster.AuditPersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                curBillMaster.AuditDate = ConstObject.LoginDate;//制单时间
                curBillMaster.AuditYear = ConstObject.LoginDate.Year;//制单年
                curBillMaster.AuditMonth = ConstObject.LoginDate.Month;//制单月
            }
            curBillMaster = model.SavePenaltyDeduction(curBillMaster);

            //#region 短信
            if (optrType == 2)
            {
                MAppPlatMng appModel = new MAppPlatMng();
                appModel.SendMessage(curBillMaster.Id, "PenaltyDeductionMaster");
            }
            this.txtCode.Text = curBillMaster.Code;
            log.BillId = curBillMaster.Id;
            log.BillType = "罚款单维护";
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
                    //if (!model.PenaltyDeductionSrv.DeleteByDao(curBillMaster)) return false;
                    if (!msrv.DeleteReceiptAndDocument(curBillMaster, curBillMaster.Id)) return false;
                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "罚款单维护";
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
                MessageBox.Show("承担队伍不能为空！");
                return false;
            }
            if (this.dgDetail.Rows.Count - 1 == 0)
            {
                MessageBox.Show("明细不能为空！");
                return false;
            }
            dgDetail.EndEdit();
            dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));

            //明细表数据校验
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                if (ClientUtil.ToString(dr.Cells[colProjectType.Name].Value) != "" && dr.Cells[colPenaltyMoney.Name].Value == null)
                {
                    MessageBox.Show("实际用工量不能为空！");
                    return false;
                }
                //最后一行不进行校验
                if (dr.IsNewRow) break;
                if (dr.Cells[colProjectType.Name].Value == null)
                {
                    MessageBox.Show("工程任务不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colProjectType.Name];
                    return false;
                }
                if (dr.Cells[this.colPenaltySubject.Name].Value == null)
                {
                    MessageBox.Show("用工科目不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colPenaltySubject.Name];
                    return false;
                }
                if (dr.Cells[colPenaltyMoney.Name].Value == null)
                {
                    MessageBox.Show("罚款金额不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colPenaltyMoney.Name];
                    return false;
                }
            }
            //dgDetail.Update();
            return true;
        }

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                string strUnit = "项";
                Application.Resource.MaterialResource.Domain.StandardUnit Unit = null;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Name", strUnit));
                IList lists = model.PenaltyDeductionSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq);
                if (lists != null && lists.Count > 0)
                {
                    Unit = lists[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                }
                string strPriceUnit = "元";
                Application.Resource.MaterialResource.Domain.StandardUnit PriceUnit = null;
                ObjectQuery oq1 = new ObjectQuery();
                oq1.AddCriterion(Expression.Eq("Name", strPriceUnit));
                IList list = model.PenaltyDeductionSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq1);
                if (list != null && list.Count > 0)
                {
                    PriceUnit = list[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                }
                curBillMaster.Descript = ClientUtil.ToString(this.txtRemark.Text);//备注
                curBillMaster.PenaltyDeductionReason = "日常检查罚款";
                curBillMaster.PenaltyType = EnumUtil<PenaltyDeductionType>.FromDescription("日常检查罚款");
                curBillMaster.PenaltyDeductionRant = txtPenaltyRank.Tag as SubContractProject;
                curBillMaster.PenaltyDeductionRantName = ClientUtil.ToString(txtPenaltyRank.Text);//罚扣队伍名称
                curBillMaster.Details.Clear();
                if (txtSumMoney.Text.Equals(""))
                {
                    curBillMaster.SumMoney = 0;
                }
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    PenaltyDeductionDetail curBillDtl = new PenaltyDeductionDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as PenaltyDeductionDetail;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.BusinessDate = Convert.ToDateTime(var.Cells[colBusinessDate.Name].Value);
                    curBillDtl.PenaltyMoney = ClientUtil.ToDecimal(var.Cells[colPenaltyMoney.Name].Value);
                    curBillDtl.PenaltySubjectGUID = var.Cells[colPenaltySubject.Name].Tag as CostAccountSubject;
                    curBillDtl.PenaltySubject = ClientUtil.ToString(var.Cells[colPenaltySubject.Name].Value);//用工科目
                    curBillDtl.PenaltySysCode = (var.Cells[colPenaltySubject.Name].Tag as CostAccountSubject).SysCode;//用工科目层次码
                    curBillDtl.ProjectTaskSyscode = (var.Cells[colPenaltySubject.Name].Tag as CostAccountSubject).SysCode;
                    curBillDtl.ResourceSysCode = ClientUtil.ToString(var.Cells[colMaterialSysCode.Name].Value);
                    curBillDtl.ResourceType = var.Cells[colMaterialType.Name].Tag as Application.Resource.MaterialResource.Domain.Material;
                    curBillDtl.ResourceTypeName = ClientUtil.ToString(var.Cells[colMaterialType.Name].Value);
                    curBillDtl.ResourceTypeSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);
                    curBillDtl.ResourceTypeStuff = ClientUtil.ToString(var.Cells[colMaterialStuff.Name].Value);
                    curBillDtl.ProjectTaskDetail = var.Cells[colProjectDetail.Name].Tag as GWBSDetail;
                    curBillDtl.TaskDetailName = ClientUtil.ToString(var.Cells[colProjectDetail.Name].Value);//任务明细名称
                    curBillDtl.PenaltyType = ClientUtil.ToString(var.Cells[colPenaltyType.Name].Value);//罚款类型
                    curBillDtl.ProjectTask = var.Cells[colProjectType.Name].Tag as GWBSTree;
                    curBillDtl.ProjectTaskName = ClientUtil.ToString(var.Cells[colProjectType.Name].Value);
                    curBillDtl.ProjectTaskSyscode = (var.Cells[colProjectType.Name].Tag as GWBSTree).SysCode;
                    curBillDtl.ProjectTaskSyscode = curBillDtl.ProjectTask.SysCode;
                    curBillDtl.Cause = ClientUtil.ToString(var.Cells[this.colReason.Name].Value);
                    curBillDtl.ProductUnit = Unit;
                    curBillDtl.ProductUnitName = Unit.Name;
                    curBillDtl.MoneyUnit = PriceUnit;
                    curBillDtl.MoneyUnitName = PriceUnit.Name;
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

        //显示数据
        private bool ModelToView()
        {
            try
            {
                this.txtCode.Text = curBillMaster.Code;
                txtHandlePerson.Tag = curBillMaster.HandlePerson;
                txtHandlePerson.Text = curBillMaster.HandlePersonName;
                txtRemark.Text = curBillMaster.Descript;
                this.txtCreatePerson.Tag = curBillMaster.CreatePerson;
                txtCreatePerson.Text = curBillMaster.CreatePersonName;
                this.txtCreatePerson.Text = curBillMaster.CreatePersonName;
                txtSumMoney.Text = curBillMaster.SumMoney.ToString("#,###.####");
                txtProject.Text = curBillMaster.ProjectName.ToString();
                txtProject.Text = curBillMaster.ProjectName;
                this.dgDetail.Rows.Clear();
                txtPenaltyRank.Tag = curBillMaster.PenaltyDeductionRant;
                this.txtPenaltyRank.Text = curBillMaster.PenaltyDeductionRantName;
                foreach (PenaltyDeductionDetail var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();
                    string a = var.BusinessDate.ToString();
                    string[] aArray = a.Split(' ');
                    string strz = aArray[0];
                    this.dgDetail[colBusinessDate.Name, i].Value = strz;
                    this.dgDetail[colPenaltyMoney.Name, i].Value = var.PenaltyMoney;
                    this.dgDetail[colProjectDetail.Name, i].Value = var.TaskDetailName;
                    this.dgDetail[colProjectType.Name, i].Value = var.ProjectTaskName;
                    this.dgDetail[colProjectType.Name, i].Tag = var.ProjectTask;
                    this.dgDetail[colPenaltySubject.Name, i].Tag = var.PenaltySubjectGUID;
                    this.dgDetail[colPenaltySubject.Name, i].Value = var.PenaltySubject;
                    this.dgDetail[colMaterialType.Name, i].Tag = var.ResourceType;
                    this.dgDetail[colMaterialType.Name, i].Value = var.ResourceTypeName;
                    this.dgDetail[colMaterialSpec.Name, i].Value = var.ResourceTypeSpec;
                    this.dgDetail[colMaterialStuff.Name, i].Value = var.ResourceTypeStuff;
                    this.dgDetail[colMaterialSysCode.Name, i].Value = var.ResourceSysCode;
                    this.dgDetail[colPenaltyType.Name, i].Value = var.PenaltyType;
                    this.dgDetail[this.colReason.Name, i].Value = var.Cause;
                    this.dgDetail.Rows[i].Tag = var;
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            bool flag = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            if (colName == colPenaltyMoney.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colPenaltyMoney.Name].Value != null)
                {
                    string temp_price = dgDetail.Rows[e.RowIndex].Cells[colPenaltyMoney.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_price);
                    if (validity == false)
                    {
                        MessageBox.Show("罚扣金额为数字！");
                        this.dgDetail.Rows[e.RowIndex].Cells[colPenaltyMoney.Name].Value = "";
                        flag = false;
                    }
                }
                if (flag)
                {
                    object money = dgDetail.Rows[e.RowIndex].Cells[colPenaltyMoney.Name].Value;
                    decimal sumqty = 0;
                    decimal summoney = 0;
                    for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                    {
                        money = dgDetail.Rows[i].Cells[colPenaltyMoney.Name].Value;
                        summoney = summoney + ClientUtil.TransToDecimal(money);
                        if (money == null) money = 0;
                    }
                    txtSumMoney.Text = summoney.ToString();
                }
            }
        }

        /// <summary>
        /// 工程任务类型列，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (this.dgDetail.EditMode == DataGridViewEditMode.EditOnEnter)
            {
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colProjectDetail.Name))
                {
                    DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                    VSelectGWBSDetail frm = new VSelectGWBSDetail(new MGWBSTree());
                    frm.ShowDialog();
                    if (frm.IsOk)
                    {
                        List<GWBSDetail> list = frm.SelectGWBSDetail;
                        //dgDetail.Rows.Clear();
                        string strName = "劳务分包资源";
                        Application.Resource.MaterialResource.Domain.Material theMaterial = null;
                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("Name", strName));
                        IList lst = model.PenaltyDeductionSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.Material), oq);
                        if (lst != null && lst.Count > 0)
                        {
                            theMaterial = lst[0] as Application.Resource.MaterialResource.Domain.Material;
                        }
                        foreach (GWBSDetail gwbsTree in list)
                        {
                            if (dgDetail.CurrentRow.Cells[colMaterialType.Name].Value != null || dgDetail.CurrentRow.Cells[colPenaltyMoney.Name].Value != null || dgDetail.CurrentRow.Cells[colPenaltySubject.Name].Value != null || dgDetail.CurrentRow.Cells[colPenaltyType.Name].Value != null || dgDetail.CurrentRow.Cells[colProjectDetail.Name].Value != null || dgDetail.CurrentRow.Cells[colProjectType.Name].Value != null)
                            {
                                this.dgDetail.CurrentRow.Cells[colProjectDetail.Name].Value = gwbsTree.Name;
                                this.dgDetail.CurrentRow.Cells[colProjectDetail.Name].Tag = gwbsTree;
                                this.dgDetail.CurrentRow.Cells[colProjectType.Name].Value = gwbsTree.TheGWBS.Name;
                                this.dgDetail.CurrentRow.Cells[colProjectType.Name].Tag = gwbsTree.TheGWBS;
                                this.dgDetail.CurrentRow.Cells[colBusinessDate.Name].Value = DateTime.Now;
                                this.dgDetail.CurrentRow.Cells[colMaterialType.Name].Value = strName;
                                if (theMaterial != null)
                                {
                                    this.dgDetail.CurrentRow.Cells[colMaterialType.Name].Tag = theMaterial;
                                    this.dgDetail.CurrentRow.Cells[colMaterialSysCode.Name].Value = theMaterial.IfCatResource;
                                    this.dgDetail.CurrentRow.Cells[colMaterialStuff.Name].Value = theMaterial.Stuff;
                                    this.dgDetail.CurrentRow.Cells[colMaterialSpec.Name].Value = theMaterial.Specification;
                                }
                            }
                            else
                            {
                                int i = dgDetail.Rows.Add();
                                this.dgDetail[colProjectDetail.Name, i].Value = gwbsTree.Name;
                                this.dgDetail[colProjectDetail.Name, i].Tag = gwbsTree;
                                this.dgDetail[colProjectType.Name, i].Value = gwbsTree.TheGWBS.Name;
                                this.dgDetail[colProjectType.Name, i].Tag = gwbsTree.TheGWBS;
                                this.dgDetail[colBusinessDate.Name, i].Value = DateTime.Now;
                                this.dgDetail[colMaterialType.Name, i].Value = strName;
                                if (theMaterial != null)
                                {
                                    this.dgDetail[colMaterialType.Name, i].Tag = theMaterial;
                                    this.dgDetail[colMaterialSysCode.Name, i].Value = theMaterial.IfCatResource;
                                    this.dgDetail[colMaterialStuff.Name, i].Value = theMaterial.Stuff;
                                    this.dgDetail[colMaterialSpec.Name, i].Value = theMaterial.Specification;
                                }
                                i++;
                            }
                        }
                        this.txtCode.Focus();
                    }
                }
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colPenaltySubject.Name))
                {
                    //双击用工科目
                    VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
                    frm.ShowDialog();
                    CostAccountSubject cost = frm.SelectAccountSubject;
                    if (cost != null)
                    {
                        if (dgDetail.CurrentRow.Cells[colMaterialType.Name].Value != null || dgDetail.CurrentRow.Cells[colPenaltyMoney.Name].Value != null || dgDetail.CurrentRow.Cells[colPenaltySubject.Name].Value != null || dgDetail.CurrentRow.Cells[colPenaltyType.Name].Value != null || dgDetail.CurrentRow.Cells[colProjectDetail.Name].Value != null || dgDetail.CurrentRow.Cells[colProjectType.Name].Value != null)
                        {
                            this.dgDetail.CurrentRow.Cells[colPenaltySubject.Name].Tag = cost;
                            this.dgDetail.CurrentRow.Cells[colPenaltySubject.Name].Value = cost.Name;
                        }
                        else
                        {
                            int i = dgDetail.Rows.Add();
                            this.dgDetail[colPenaltySubject.Name, i].Value = cost.Name;
                            this.dgDetail[colPenaltySubject.Name, i].Tag = cost;
                        }
                        this.txtCode.Focus();
                    }
                }
            }
        }

        private void GetAccountGWBSNodes(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                GWBSTree accountGWBS = tn.Tag as GWBSTree;
                if (accountGWBS.IsAccountNode)//如果该节点是核算节点
                {
                    ListAccountGWBSNodes.Add(tn);
                }
                else
                {
                    GetAccountGWBSNodes(tn);
                }
            }
        }

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            if (LoadTempleteFile(@"罚款通知单.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            return true;
        }

        public override bool Print()
        {
            if (LoadTempleteFile(@"罚款通知单.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.Print();
            return true;
        }

        void btnPrintPage_Click(object sender, FlexCell.Grid.PrintPageEventArgs e)
        {
            if (e.Preview == false && e.PageNumber == 1)
            {
                stockModel.StockInSrv.UpdateBillPrintTimes(6, curBillMaster.Id);//回写次数
                //写打印日志
                StaticMethod.InsertLogData(curBillMaster.Id, "打印", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "罚款单", "", curBillMaster.ProjectName);
            }
        }

        public override bool Export()
        {
            if (LoadTempleteFile(@"罚款通知单.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.ExportToExcel("罚款通知单【" + curBillMaster.Code + "】", false, false, true);
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

        private void FillFlex(PenaltyDeductionMaster billMaster)
        {
            int detailStartRowNumber = 5;//7为模板中的行号
            int detailCount = billMaster.Details.Count;

            CommonUtil.SetFlexGridPrintFace(this.flexGrid1);

            //主表数据

            flexGrid1.Cell(3, 2).Text = billMaster.Code;
            flexGrid1.Cell(3, 5).Text = billMaster.CreateDate.Year + "年" + billMaster.CreateDate.Month + "月" + billMaster.CreateDate.Day + "日";
            
            flexGrid1.Cell(4, 2).Text = billMaster.PenaltyDeductionRantName;
            flexGrid1.Cell(4, 4).Text = billMaster.CreatePersonName;
            if (projectInfo == null)
            {
                projectInfo = StaticMethod.GetProjectInfo();
            }
            flexGrid1.Cell(4, 6).Text = projectInfo.HandlePersonName;

            StringBuilder cause = new StringBuilder();
            cause.AppendLine("处罚原因：");
            for (int i = 0; i < detailCount; i++)
            {
                PenaltyDeductionDetail detail = (PenaltyDeductionDetail)billMaster.Details.ElementAt(i);
                cause.AppendLine("    " + ClientUtil.ToString(i + 1) + "." + detail.Cause + ";");
            }
            cause.AppendLine("");
            flexGrid1.Cell(5, 1).Text = cause.ToString();
            flexGrid1.Cell(5, 1).Alignment = FlexCell.AlignmentEnum.LeftCenter;
            flexGrid1.Row(5).AutoFit();

            int maxRow = detailStartRowNumber +4;
            CommonUtil.SetFlexAuditPrint(flexGrid1, billMaster.Id, maxRow);

            //填写明细数据
            decimal sumMoney = billMaster.SumMoney;
            string Moneybig = CurrencyComUtil.GetMoneyChinese(sumMoney.ToString());
            flexGrid1.Cell(6, 2).Text = " ￥：" + sumMoney.ToString("N2") + "元" + "    大写：" + Moneybig;
            flexGrid1.Cell(6, 5).Text = "";

            flexGrid1.Cell(8, 5).Text = billMaster.ProjectName;
            flexGrid1.Row(8).AutoFit();

            //条形码
            string a = Application.Business.Erp.SupplyChain.Client.Util.DateUtil.GetStrMoneyInchar(ClientUtil.ToString(sumMoney));
            this.flexGrid1.Cell(1, 5).Text = billMaster.Code.Substring(billMaster.Code.Length - 11) + "-" + a;
            this.flexGrid1.Cell(1, 5).CellType = FlexCell.CellTypeEnum.BarCode;
            this.flexGrid1.Cell(1, 5).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;

            this.flexGrid1.Cell(2, 5).Text = "打印顺序号: " + CommonUtil.GetPrintTimesStr(billMaster.PrintTimes + 1);
            FlexCell.PageSetup pageSetup = flexGrid1.PageSetup;
            pageSetup.RightFooter = " 打印时间:[" + CommonMethod.GetServerDateTime() + " ],打印人:[" + ConstObject.LoginPersonInfo.Name + "]   " + "第&P页/共&N页  ";
        }

    }
}
