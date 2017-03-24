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
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalContractMng.Domain;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalSettlementMng.Domain;
using Application.Business.Erp.SupplyChain.Client.MaterialRentalMange;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using VirtualMachine.Core;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Business.Erp.SupplyChain.Client.MaterialRentalManage.MaterialRentalContractMng;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using IRPServiceModel.Domain.Document;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.Client.FileUpload;
using System.IO;
using IRPServiceModel.Basic;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement;

namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalManage.MaterialRentalContractMng
{
    public partial class VMaterialRentalContract : TMasterDetailView
    {
        private MMatRentalMng model = new MMatRentalMng();
        private MaterialRentalContractMaster curBillMaster;
        private MaterialRentalContractDetail curBillDetail;
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        /// <summary>
        /// 当前单据
        /// </summary>
        public MaterialRentalContractMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }
        /// <summary>
        /// 当前明细单据
        /// </summary>
        public MaterialRentalContractDetail CurBillDetail
        {
            get { return curBillDetail; }
            set { curBillDetail = value; }
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

        private string[] arrDateUnitNames = new string[] { "月", "天", "台班" };

        public VMaterialRentalContract()
        {
            InitializeComponent();
            InitEvent();
            InitDate();
            InitData();
        }

        public void InitData()
        {
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode3 + "-" + CommonUtil.SupplierCatCode4 + "-" + CommonUtil.SupplierCatCode2;
            colDateUnit.Items.AddRange(arrDateUnitNames);
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

            cmobalanceStyle.Items.AddRange(new object[] { "过程结算", "末次结算", "质保期" });
        }

        private void InitEvent()
        {
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            this.dgDetail.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgDetail_EditingControlShowing);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.dgDetail.CellEnter += new DataGridViewCellEventHandler(dgDetail_CellEnter);
            dgDetail.CellValueChanged += new DataGridViewCellEventHandler(dgDetail_CellValueChanged);

            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);

            //tab页切换数据处理
            tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);

            #region 相关文档
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
            #endregion
        }

        void dgDetail_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == colPrice.Index || e.ColumnIndex == colQuantity.Index)//如果是单价或是数量
            {
                var strPrice = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value;
                var strCount = dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value;
                var price = ClientUtil.ToDecimal(strPrice);
                var count = ClientUtil.ToDecimal(strCount);
                if (price == 0 || count == 0)
                {
                    dgDetail.Rows[e.RowIndex].Cells[colSettleMoney.Name].Value = string.Empty;
                }
                else
                {
                    dgDetail.Rows[e.RowIndex].Cells[colSettleMoney.Name].Value = price * count;
                }
            }
        }

        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == tabPage1.Name)//如果是明细
            {

            }
            else if (tabControl1.SelectedTab.Name == tabPage2.Name)//相关文档
            {
                if (curBillMaster != null && !string.IsNullOrEmpty(curBillMaster.Id))
                {
                    FillDoc();
                }
            }
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
                        curBillMaster = model.SaveMaterialRentalContract(curBillMaster);
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

        void dgDetail_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            object _DateControl = dgDetail.Controls["DataGridViewCalendarColumn"];
            if (_DateControl == null) return;
            DateTimePicker _DateTimePicker = (DateTimePicker)_DateControl;


            if (e.ColumnIndex == 4 || e.ColumnIndex == 5)//修改想要的列
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
                    curBillMaster.Details.Remove(dr.Tag as MaterialRentalContractDetail);
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
                if (Id == "空")
                    RefreshState(MainViewState.Initialize);
                else
                {
                    curBillMaster = model.MatMngSrv.GetMaterialRentalContractById(Id);
                    ModelToView();
                    RefreshState(MainViewState.Browser);

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
            }
            else
            {
                ObjectLock.Lock(pnlFloor, true);
            }

            //永久锁定
            object[] os = new object[] { txtCode, txtCreatePerson, txtHandlePerson, txtProject, txtSumQuantity, txtSumMoney };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colMaterialCode.Name, colMaterialName.Name, colMaterialSep.Name, colSettleMoney.Name, colUsedPart.Name, colQuantityUnit.Name };
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
                this.curBillMaster = new MaterialRentalContractMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;//制单人编号
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;//制单人名称

                curBillMaster.CreateYear = ConstObject.LoginDate.Year;//制单年
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;//制单月
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//登录业务组织
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//业务组织名称
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;//业务组织编号
                curBillMaster.HandOrgLevel = ConstObject.TheOperationOrg.Level;
                curBillMaster.DocState = DocumentState.Edit;//状态
                //curBillMaster.RealOperationDate = DateTime.Now;//业务日期默认为当前时间
                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //责任人
                txtHandlePerson.Tag = ConstObject.LoginPersonInfo;
                txtHandlePerson.Text = ConstObject.LoginPersonInfo.Name;
                //归属项目
                projectInfo = StaticMethod.GetProjectInfo();
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
            if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
            {
                base.ModifyView();
                curBillMaster = model.MatMngSrv.GetMaterialRentalContractById(curBillMaster.Id);
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
            curBillMaster = model.SaveMaterialRentalContract(curBillMaster);
            this.txtCode.Text = curBillMaster.Code;
            MessageBox.Show("保存成功！");
            log.BillId = curBillMaster.Id;
            log.BillType = "机械租赁合同单";
            log.Code = curBillMaster.Code;
            log.Descript = "";
            log.OperPerson = ConstObject.LoginPersonInfo.Name;
            log.ProjectName = curBillMaster.ProjectName;
            StaticMethod.InsertLogData(log);
            this.ViewCaption = ViewName + "-" + txtCode.Text;
            return true;
        }


        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        public override bool SubmitView()
        {
            try
            {
                if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
                {
                    return this.SaveOrSubmitBill(2);
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
        /// 保存
        /// </summary>
        /// <returns></returns>
        public override bool SaveView()
        {
            try
            {
                if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
                {
                    return this.SaveOrSubmitBill(1);
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能保存！");
                return false;
            }
            catch (Exception e)
            {
                if (e.Message != null && e.Message.IndexOf("SETTLESUBJECT") != -1)
                {
                    MessageBox.Show("数据保存错误：【核算科目不能为空！】");
                }
                else
                {
                    MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
                }
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
                MaterialRentalContractDetail ad = new MaterialRentalContractDetail();
                if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
                {
                    if (ad.RefQuantity > 0)
                    {
                        MessageBox.Show("此信息被引用，删除失败！");
                        return false;
                    }
                    else
                    {
                        curBillMaster = model.MatMngSrv.GetMaterialRentalContractById(curBillMaster.Id);
                        if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                        {
                            if (!model.MatMngSrv.DeleteByDao(curBillMaster)) return false;
                            LogData log = new LogData();
                            log.BillId = curBillMaster.Id;
                            log.BillType = "机械租赁结算单";
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
                }
                string message = "此单状态为【{0}】，不能删除！";
                message = string.Format(message, ClientUtil.GetDocStateName(curBillMaster.DocState));
                MessageBox.Show(message);
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
                        curBillMaster = model.MatMngSrv.GetMaterialRentalContractById(curBillMaster.Id);
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
                curBillMaster = model.MatMngSrv.GetMaterialRentalContractById(curBillMaster.Id);
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
            if (txtSupply.Text == "")
            {
                MessageBox.Show("供应商信息不能为空！");
                return false;
            }
            if (ClientUtil.ToString(cmobalanceStyle.Text) == "")
            {
                MessageBox.Show("结算完成情况不能为空！");
                return false;
            }

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

                if (dr.Cells[colMaterialCode.Name].Value == null)
                {
                    MessageBox.Show("材料编码不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colMaterialCode.Name];
                    return false;
                }
                if (ClientUtil.ToDateTime(dr.Cells[colStartDate.Name].Value) > ClientUtil.ToDateTime(dr.Cells[colEndDate.Name].Value))
                {
                    MessageBox.Show("租赁结束时间要大于开始时间！");
                    return false;
                }
                //if (dr.Cells[colSubject.Name].Value == null)
                //{
                //    MessageBox.Show("结算科目不能为空！");
                //    dgDetail.CurrentCell = dr.Cells[colSubject.Name];  //this block need fix
                //    return false;
                //}
                if (dr.Cells[this.colUsedPart.Name].Value == null)
                {
                    MessageBox.Show("使用部位不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colUsedPart.Name];
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
                curBillMaster.CreateDate = ConstObject.LoginDate;//业务时间
                curBillMaster.StartDate = dtpDateBegin.Value.Date;
                curBillMaster.EndDate = dtpDateEnd.Value.Date;
                curBillMaster.HandlePerson = this.txtHandlePerson.Tag as PersonInfo;
                curBillMaster.HandlePersonName = ClientUtil.ToString(this.txtHandlePerson.Text);
                if (txtSupply.Text != "")
                {
                    curBillMaster.TheSupplierRelationInfo = this.txtSupply.Result[0] as SupplierRelationInfo;
                    curBillMaster.SupplierName = ClientUtil.ToString(this.txtSupply.Text);
                }
                curBillMaster.Descript = ClientUtil.ToString(this.txtDescript.Text);

                curBillMaster.BalanceStyle = cmobalanceStyle.Text;
                if (!string.IsNullOrEmpty(txtProcessPayRate.Text.Trim()))
                {
                    curBillMaster.ProcessPayRate = ClientUtil.ToDecimal(txtProcessPayRate.Text) / 100;//过程付款比例
                }
                if (!string.IsNullOrEmpty(txtCompletePayRate.Text.Trim()))
                {
                    curBillMaster.CompletePayRate = ClientUtil.ToDecimal(txtCompletePayRate.Text) / 100;//完工结算付款比例
                }
                if (!string.IsNullOrEmpty(txtWarrantyPayRate.Text.Trim()))
                {
                    curBillMaster.WarrantyPayRate = ClientUtil.ToDecimal(txtWarrantyPayRate.Text) / 100;//质保期付款比例
                }

                var lstAllUnit = GetDateUnit();
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    MaterialRentalContractDetail curBillDtl = new MaterialRentalContractDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as MaterialRentalContractDetail;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescription.Name].Value);
                    //curBillDtl.MaterialSource = ClientUtil.ToString(var.Cells[colSource.Name].Value);
                    curBillDtl.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);
                    curBillDtl.MaterialName = ClientUtil.ToString(var.Cells[colMaterialName.Name].Value);
                    curBillDtl.MaterialResource = var.Cells[colMaterialCode.Name].Tag as Application.Resource.MaterialResource.Domain.Material;
                    //curBillDtl.MaterialStuff = (var.Cells[colMaterialCode.Name].Tag as Application.Resource.MaterialResource.Domain.Material).Stuff;
                    curBillDtl.MaterialSysCode = var.Cells[colMaterialName.Name].Tag as string;
                    curBillDtl.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSep.Name].Value);
                    curBillDtl.Money = ClientUtil.ToDecimal(var.Cells[colSettleMoney.Name].Value);

                    curBillDtl.Quantity = ClientUtil.ToDecimal(var.Cells[colQuantity.Name].Value);
                    curBillDtl.QuantityUnit = var.Cells[colQuantityUnit.Name].Tag as StandardUnit;
                    curBillDtl.QuantityUnitName = ClientUtil.ToString(var.Cells[colQuantityUnit.Name].Value);
                    curBillDtl.SettleDate = ClientUtil.ToDecimal(var.Cells[colSettleDate.Name].Value);
                    curBillDtl.SettleMoney = ClientUtil.ToDecimal(var.Cells[colSettleMoney.Name].Value);
                    curBillDtl.StartSettleDate = Convert.ToDateTime(var.Cells[colStartDate.Name].Value);
                    curBillDtl.EndSettleDate = Convert.ToDateTime(var.Cells[colEndDate.Name].Value);
                    //curBillDtl.UsedPart = ClientUtil.ToString(var.Cells[colUsedPart.Name].Tag as GWBSTree);
                    //curBillDtl.UsedPartName = ClientUtil.ToString(var.Cells[colUsedPart.Name].Value);
                    if (var.Cells[colUsedPart.Name].Value != null)
                    {
                        curBillDtl.UsedPart = var.Cells[colUsedPart.Name].Tag as GWBSTree;
                        curBillDtl.UsedPartName = ClientUtil.ToString(var.Cells[colUsedPart.Name].Value);
                        curBillDtl.UsedPartSysCode = curBillDtl.UsedPart.SysCode;
                    }
                    //时间单位分为日和月
                    //if (var.Cells[colDateUnit.Name].Value.Equals("天"))
                    //{
                    //    string strUnit1 = "天";
                    //    Application.Resource.MaterialResource.Domain.StandardUnit Unit1 = null;
                    //    ObjectQuery oq1 = new ObjectQuery();
                    //    oq1.AddCriterion(Expression.Eq("Name", strUnit1));
                    //    IList lists1 = model.MatMngSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq1);
                    //    if (lists1 != null && lists1.Count > 0)
                    //    {
                    //        Unit1 = lists1[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                    //        var.Cells[colDateUnit.Name].Value = Unit1.Name;
                    //        var.Cells[colDateUnit.Name].Tag = Unit1;
                    //    }
                    //}
                    //时间单位里面存在月，天，台班，为了避免循环里面访问数据库，故作此修改
                    string strUnit1 = ClientUtil.ToString(var.Cells[colDateUnit.Name].Value);
                    if (lstAllUnit != null && lstAllUnit.Count > 0)
                    {
                        var curDateUnit = lstAllUnit.FirstOrDefault(p => p.Name == strUnit1);
                        if (curDateUnit != null)
                        {
                            var.Cells[colDateUnit.Name].Value = curDateUnit.Name;
                            var.Cells[colDateUnit.Name].Tag = curDateUnit;
                            //curBillDtl.DateUnit = curDateUnit;
                        }
                    }
                    curBillDtl.DateUnit = var.Cells[colDateUnit.Name].Tag as StandardUnit;
                    curBillDtl.DateUnitName = ClientUtil.ToString(var.Cells[colDateUnit.Name].Value);
                    //价格单位默认为元
                    string strUnit = "元";
                    Application.Resource.MaterialResource.Domain.StandardUnit Unit = null;
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Name", strUnit));
                    IList lists = model.MatMngSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq);
                    if (lists != null && lists.Count > 0)
                    {
                        Unit = lists[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                        curBillDtl.PriceUnit = Unit;
                        curBillDtl.PriceUnitName = Unit.Name;
                    }
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
        /// 获取时间单位信息
        /// </summary>
        /// <returns></returns>
        private List<StandardUnit> GetDateUnit()
        {
            ObjectQuery oq1 = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (string name in arrDateUnitNames)
            {
                dis.Add(Expression.Eq("Name", name));
            }
            oq1.AddCriterion(dis);
            var lst = model.MatMngSrv.GetDomainByCondition(typeof(StandardUnit), oq1).OfType<StandardUnit>().ToList();
            return lst;
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
                    this.dgDetail.CurrentRow.Cells[colMaterialSep.Name].Value = selectedMaterial.Specification;
                    this.dgDetail.RefreshEdit();
                }
            }
        }

        /// <summary>
        /// 在设备编码列，敲击键盘时，取消原来已经选择的设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.dgDetail.CurrentCell.Tag = null;
        }

        /// <summary>
        /// 鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (this.dgDetail.EditMode == DataGridViewEditMode.EditOnEnter)
            {
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colMaterialCode.Name))
                {
                    DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                    CommonMaterial materialSelector = new CommonMaterial();
                    materialSelector.materialCatCode = "02" + "-" + "03";
                    DataGridViewCell cell = this.dgDetail[e.ColumnIndex, e.RowIndex];
                    object tempValue = cell.EditedFormattedValue;
                    if (tempValue != null && !tempValue.Equals(""))
                    {
                        materialSelector.OpenSelect();
                    }
                    else
                    {
                        materialSelector.OpenSelect();
                    }
                    IList list = materialSelector.Result;
                    foreach (Application.Resource.MaterialResource.Domain.Material theMaterial in list)
                    {
                        int i = dgDetail.Rows.Add();
                        this.dgDetail[colMaterialCode.Name, i].Tag = theMaterial;
                        this.dgDetail[colMaterialCode.Name, i].Value = theMaterial.Code;
                        this.dgDetail[colMaterialName.Name, i].Value = theMaterial.Name;
                        this.dgDetail[colMaterialName.Name, i].Tag = theMaterial.TheSyscode;
                        this.dgDetail[colMaterialSep.Name, i].Value = theMaterial.Specification;
                        this.dgDetail[colEndDate.Name, i].Value = DateTime.Now;
                        this.dgDetail[colStartDate.Name, i].Value = DateTime.Now;
                        //this.dgDetail[colSource.Name, i].Value = "内部租赁";
                        string strUnit = "台";
                        Application.Resource.MaterialResource.Domain.StandardUnit Unit = null;
                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("Name", strUnit));
                        IList lists = model.MatMngSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq);
                        if (lists != null && lists.Count > 0)
                        {
                            Unit = lists[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                            this.dgDetail[colQuantityUnit.Name, i].Value = Unit.Name;
                            this.dgDetail[colQuantityUnit.Name, i].Tag = Unit;
                        }
                        string strUnit1 = "月";
                        Application.Resource.MaterialResource.Domain.StandardUnit Unit1 = null;
                        ObjectQuery oq1 = new ObjectQuery();
                        oq1.AddCriterion(Expression.Eq("Name", strUnit1));
                        IList lists1 = model.MatMngSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq1);
                        if (lists1 != null && lists1.Count > 0)
                        {
                            Unit1 = lists1[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                            this.dgDetail[colDateUnit.Name, i].Value = Unit1.Name;
                            this.dgDetail[colDateUnit.Name, i].Tag = Unit1;
                        }
                        GWBSTree Part = null;
                        ObjectQuery oqTree = new ObjectQuery();
                        oqTree.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
                        oqTree.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
                        IList listTree = model.MatMngSrv.GetDomainByCondition(typeof(GWBSTree), oqTree);
                        if (listTree != null && listTree.Count > 0)
                        {
                            Part = listTree[0] as GWBSTree;
                            this.dgDetail[colUsedPart.Name, i].Value = Part.Name;
                            this.dgDetail[colUsedPart.Name, i].Tag = Part;
                        }
                        i++;
                    }
                    txtCode.Focus();
                }
                else
                {
                    if (this.dgDetail[colMaterialCode.Name, e.RowIndex].Value == null)
                    {
                        MessageBox.Show("请先选择设备信息");
                        return;
                    }
                }
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colUsedPart.Name))
                {
                    VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
                    frm.IsTreeSelect = true;
                    frm.ShowDialog();
                    List<TreeNode> list = frm.SelectResult;
                    if (list.Count > 0)
                    {
                        dgDetail[colUsedPart.Name, e.RowIndex].Tag = (list[0] as TreeNode).Tag as GWBSTree;
                        dgDetail[colUsedPart.Name, e.RowIndex].Value = (list[0] as TreeNode).Text;
                        txtCode.Focus();
                    }
                }
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colQuantityUnit.Name))
                {
                    StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                    if (su != null)
                    {
                        this.dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Tag = su;
                        this.dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Value = su.Name;
                        this.txtCode.Focus();
                    }
                }
            }
        }

        //显示数据
        private bool ModelToView()
        {
            try
            {
                this.dtpDateBegin.Text = curBillMaster.StartDate.ToShortDateString();
                this.dtpDateEnd.Text = curBillMaster.EndDate.ToShortDateString();
                this.txtCode.Text = curBillMaster.Code;
                this.txtHandlePerson.Tag = curBillMaster.HandlePerson;
                this.txtHandlePerson.Text = curBillMaster.HandlePersonName;
                if (curBillMaster.TheSupplierRelationInfo != null)
                {
                    txtSupply.Result.Clear();
                    txtSupply.Result.Add(curBillMaster.TheSupplierRelationInfo);
                }
                this.txtSupply.Value = curBillMaster.SupplierName;
                this.txtDescript.Text = curBillMaster.Descript;
                this.txtSumMoney.Text = ClientUtil.ToString(curBillMaster.SumMoney);
                this.txtSumQuantity.Text = ClientUtil.ToString(curBillMaster.SumQuantity);
                this.txtCreatePerson.Tag = curBillMaster.CreatePerson;
                this.txtCreatePerson.Text = curBillMaster.CreatePersonName;
                this.txtProject.Tag = curBillMaster.ProjectId;
                this.txtProject.Text = curBillMaster.ProjectName.ToString();

                cmobalanceStyle.Text = curBillMaster.BalanceStyle;
                txtProcessPayRate.Text = ClientUtil.ToString(curBillMaster.ProcessPayRate * 100);
                txtCompletePayRate.Text = ClientUtil.ToString(curBillMaster.CompletePayRate * 100);
                txtWarrantyPayRate.Text = ClientUtil.ToString(curBillMaster.WarrantyPayRate * 100);

                this.dgDetail.Rows.Clear();
                decimal sumMoney = 0;
                foreach (MaterialRentalContractDetail var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();
                    this.dgDetail[colUsedPart.Name, i].Tag = var.UsedPart as GWBSTree;
                    this.dgDetail[colUsedPart.Name, i].Value = var.UsedPartName;
                    this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                    this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                    this.dgDetail[colMaterialName.Name, i].Tag = var.MaterialSysCode;
                    this.dgDetail[colMaterialSep.Name, i].Value = var.MaterialSpec;
                    this.dgDetail[colQuantity.Name, i].Value = var.Quantity;
                    this.dgDetail[colQuantityUnit.Name, i].Value = var.QuantityUnit;
                    this.dgDetail[colQuantityUnit.Name, i].Value = var.QuantityUnitName;
                    this.dgDetail[colSettleDate.Name, i].Value = var.SettleDate;
                    this.dgDetail[colSettleMoney.Name, i].Value = var.SettleMoney;
                    object quantity = var.SettleMoney;
                    if (quantity != null)
                    {
                        sumMoney += decimal.Parse(quantity.ToString());
                    }
                    this.dgDetail[colStartDate.Name, i].Value = var.StartSettleDate;
                    this.dgDetail[colEndDate.Name, i].Value = var.EndSettleDate;
                    this.dgDetail[colDateUnit.Name, i].Tag = var.DateUnit;
                    this.dgDetail[colDescription.Name, i].Value = var.Descript;
                    this.dgDetail[colDateUnit.Name, i].Value = var.DateUnitName;
                    this.dgDetail[colUsedPart.Name, i].Tag = var.UsedPart;
                    this.dgDetail[colUsedPart.Name, i].Value = var.UsedPartName;
                    string strId = ClientUtil.ToString(var.Id);
                    list = model.MatMngSrv.GetMaterialSubjectByParentId(strId);
                    this.dgDetail.Rows[i].Tag = var;
                }
                this.txtSumMoney.Text = sumMoney.ToString("#,###.####");
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
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            decimal sumQuantity = 0;
            decimal sumMoney = 0;
            //for (int i = 0; i <= dgDetail.RowCount - 1; i++)
            //{
            //    if (colName == colSettleMoney.Name)//金额
            //    {
            //        string Money = ClientUtil.ToString(dgDetail.Rows[i].Cells[colSettleMoney.Name].Value);
            //        if (Money == null || Money == "")
            //        {
            //            Money = "0";
            //            return;
            //        }
            //        validity = CommonMethod.VeryValid(Money);
            //        if (validity == false)
            //        {
            //            MessageBox.Show("核算合价为数字！");
            //            this.dgDetail.Rows[i].Cells[colSettleMoney.Name].Value = "";
            //            return;
            //        }
            //        sumMoney += ClientUtil.ToDecimal(Money);
            //        txtSumMoney.Text = ClientUtil.ToString(sumMoney);
            //    }
            //    //string Moneys = ClientUtil.ToString(dgDetail.Rows[i].Cells[colSettleMoney.Name].Value);
            //    //if (Moneys == null || Moneys == "")
            //    //{
            //    //    Moneys = "0";
            //    //    sumMoney += ClientUtil.ToDecimal(Moneys);
            //    //    txtSumMoney.Text = ClientUtil.ToString(sumMoney);

            //    //}
            //    if (colName == colQuantity.Name)//数量
            //    {
            //        string quantity = ClientUtil.ToString(dgDetail.Rows[i].Cells[colQuantity.Name].Value);
            //        if (quantity == null || quantity == "")
            //        {
            //            quantity = "0";
            //            return;
            //        }
            //        validity = CommonMethod.VeryValid(quantity);
            //        if (validity == false)
            //        {
            //            MessageBox.Show("数量为数字！");
            //            this.dgDetail.Rows[i].Cells[colQuantity.Name].Value = "";
            //            return;
            //        }
            //        sumQuantity += ClientUtil.ToDecimal(quantity);
            //        txtSumQuantity.Text = ClientUtil.ToString(sumQuantity);
            //    }
            //}         //need fix
            decimal sumMoney1 = 0;
            for (int i = 0; i <= dgDetail.RowCount - 1; i++)
            {
                string Money = ClientUtil.ToString(dgDetail.Rows[i].Cells[colSettleMoney.Name].Value);
                if (Money == null || Money == "")
                {
                    Money = "0";
                }
                sumMoney1 += ClientUtil.ToDecimal(Money);

            }
            txtSumMoney.Text = ClientUtil.ToString(sumMoney1);

        }
    }
}
