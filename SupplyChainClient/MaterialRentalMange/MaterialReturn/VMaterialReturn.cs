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
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialReturnMng.Domain;
using VirtualMachine.Component.WinControls.Controls;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalLedgerMng;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalLedgerMng.Domain;
using Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialCollection;
using Application.Business.Erp.SupplyChain.Util;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalOrder;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using System.IO;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.ClientManagement;
using IRPServiceModel.Domain.Document;
using NHibernate.Criterion;
using IRPServiceModel.Basic;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using Application.Business.Erp.SupplyChain.Client.FileUpload;

namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialReturn
{
    public partial class VMaterialReturn : TMasterDetailView
    {
        private MMatRentalMng model = new MMatRentalMng();
        private MaterialReturnMaster matReturnMaster;
        /// <summary>
        /// 当前单据
        /// </summary>
        public MaterialReturnMaster MatReturnMaster
        {
            get { return matReturnMaster; }
            set { matReturnMaster = value; }
        }
        CurrentProjectInfo projectInfo;
        string userName = string.Empty;
        string jobId = string.Empty;
        string fileObjectType = string.Empty;
        private MaterialRentalOrderMaster matRentalOrderMaster;
        /// <summary>
        /// 当前单据
        /// </summary>
        public MaterialRentalOrderMaster MatRentalOrderMaster
        {
            get { return matRentalOrderMaster; }
            set { matRentalOrderMaster = value; }
        }

        
        IList MatRenLed_list = null;
        IList MatReturnDetSeq_list = null;
        IList list_MatReturnDtlSeq = null;
        //非数量计费
        IList list_index_detail = new ArrayList();
        IList list_costType_detail = new ArrayList();
        //按数量计费
        IList list_index_costDetail = new ArrayList();
        IList list_costType_costDetail = new ArrayList();

        public int returnType = 1;        //type  1:料具退料单、 2：料具退料单(损耗)

        public VMaterialReturn(int type)
        {
            InitializeComponent();
            this.InitData(type);
            this.InitEvent();
        }

        private void InitData(int type)
        {
            returnType = type;
            txtBalRule.DataSource = (Enum.GetNames(typeof(EnumMaterialMngBalRule)));
            if (type == 1)
            {
                this.tabPage2.Parent = null;
                this.dgDetail.Columns[colLossQty.Name].Visible = false;
                this.dgDetail.Columns[colConsumeQuantity.Name].Visible = false;
            }
            else if (type == 2)
            {
                this.tabPage2.Parent = tabControl1;
                this.dgDetail.Columns[colBroachQuantity.Name].Visible = false;
                this.dgDetail.Columns[colDisCardQty.Name].Visible = false;
                this.dgDetail.Columns[colRepairQty.Name].Visible = false;
                this.dgDetail.Columns[colRejectQuantity.Name].Visible = false;
            }
            projectInfo = StaticMethod.GetProjectInfo();
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode1;
            txtRank.SupplierCatCode = CommonUtil.SupplierCatCode3;
        }

        private void InitEvent()
        {
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.dgDetail.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(dgDetail_RowHeaderMouseClick);
            this.dgDelete.Click += new EventHandler(dgDelete_Click);
            txtSupply.TextChanged += new EventHandler(txtSupply_TextChanged);
            txtTransChagre.tbTextChanged += new EventHandler(txtTransChagre_tbTextChanged);
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
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", matReturnMaster.Id));
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
            if (matReturnMaster.Id == null)
            {
                if (MessageBox.Show("当前业务对象还没保存，是否保存！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    if (!ValidView()) return;
                    try
                    {
                        if (!ViewToModel()) return;
                        matReturnMaster = model.MatMngSrv.SaveMaterialReturnMaster(matReturnMaster);
                        this.ViewCaption = ViewName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(ex));
                    }
                }
            }
            if (matReturnMaster.Id != null)
            {
                VDocumentPublicUpload frm = new VDocumentPublicUpload(matReturnMaster.Id);
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
                    dgDocumentMast_CellClick(dgDocumentMast, new DataGridViewCellEventArgs(2, dgDocumentMast.SelectedRows[0].Index));
                }
                else
                {
                    MessageBox.Show("删除失败！");
                }
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

        void txtTransChagre_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string TransCharge = this.txtTransChagre.Text;
            validity = CommonMethod.VeryValid(TransCharge);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                return;
            }
        }

        void dgDetail_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point _Point = dgDetail.PointToClient(Cursor.Position);
                this.contextMenuStrip1.Show(dgDetail, _Point);
            }
        }

        void dgDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr = dgDetail.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                dgDetail.Rows.Remove(dr);
                if (dr.Tag != null)
                {
                    (dr.Tag as MaterialReturnDetail).TempData = "删除";
                }
            }
        }

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (this.txtSupply.Text == "" && this.txtSupply.Tag == null)
            {
                MessageBox.Show("请选择料具出租方！");
                return;
            }

            //查询当前料具供应商合同租赁单价和合同号
            MaterialRentalOrderMaster theMaterialRentalOrderMaster = new MaterialRentalOrderMaster();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", this.txtSupply.Result[0] as SupplierRelationInfo));
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            IList lst = model.MatMngSrv.GetMaterialRentalOrder(oq) as IList;
            if (lst.Count > 0)
            {
                theMaterialRentalOrderMaster = (model.MatMngSrv.GetMaterialRentalOrder(oq) as IList)[0] as MaterialRentalOrderMaster;
                this.txtContractNo.Text = theMaterialRentalOrderMaster.OriginalContractNo;
            }
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colMaterialCode.Name))
            {

                DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                VMaterialRenLedSelector VMaterialRenLedSelector = new VMaterialRenLedSelector();
                VMaterialRenLedSelector.OpenSelector(this.txtSupply.Result[0] as SupplierRelationInfo, null);

                IList list = VMaterialRenLedSelector.Result;
                foreach (MaterialRentalLedger master in list)
                {
                    int i = this.dgDetail.Rows.Add();
                    this.dgDetail[colMaterialCode.Name, i].Value = master.MaterialCode;
                    this.dgDetail[colMaterialCode.Name, i].Tag = master.MaterialResource;
                    this.dgDetail[colMaterialName.Name, i].Value = master.MaterialName;
                    this.dgDetail[colMaterialSpec.Name, i].Value = master.MaterialSpec;
                    this.dgDetail[colCategoryQuantity.Name, i].Value = master.LeftQuantity;
                    this.dgDetail[colUnit.Name, i].Value = master.MatStandardUnitName;
                    this.dgDetail[colUnit.Name, i].Tag = master.MatStandardUnit;
                    this.dgDetail[colPrice.Name, i].Value = master.RentalPrice;
                    this.dgDetail[colSubject.Name, i].Tag = master.SubjectGUID;
                    this.dgDetail[colSubject.Name, i].Value = master.SubjectName;
                    this.dgDetail[colUsedPart.Name, i].Tag = master.UsedPart;
                    this.dgDetail[colUsedPart.Name, i].Value = master.UsedPartName;
                    this.dgDetail[this.colBorrowUnit.Name, i].Tag = master.TheRank;
                    this.dgDetail[colBorrowUnit.Name, i].Value = master.TheRankName;
                }
            }
            else if (this.dgDetail.Columns[e.ColumnIndex].Name.Contains("维修费"))
            {
                DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;

                if (theCurrentRow.Cells[colMaterialCode.Name].Tag == null)
                {
                    MessageBox.Show("请先选择料具信息！");
                    return;
                }
                else
                {
                    ////如果填写了维修数量，必须设置维修内容和对应的维修数量
                    //if (theCurrentRow.Cells[colRepairQty.Name].Value != null)
                    //{

                    //}
                    //查询当前合同的维修内容(根据料具区分)
                    IList list_RepairCon = new ArrayList();
                    Material theMaterial = theCurrentRow.Cells[colMaterialCode.Name].Tag as Material;
                    foreach (MaterialRentalOrderDetail theMaterialRentalOrderDetail in theMaterialRentalOrderMaster.Details)
                    {
                        if (theMaterialRentalOrderDetail.MaterialResource.Id == theMaterial.Id)
                        {
                            foreach (BasicDtlCostSet theBasicDtlCostSet in theMaterialRentalOrderDetail.BasicDtlCostSets)
                            {
                                if (theBasicDtlCostSet.SetType == SetType.维修设置.ToString())
                                {
                                    list_RepairCon.Add(theBasicDtlCostSet.WorkContent);
                                }
                            }
                        }
                    }
                    if (list_RepairCon.Count < 0)
                    {
                        MessageBox.Show("当前料具供应商的合同未设置维修内容，请先设置维修内容和单价！");
                        return;
                    }
                    else
                    {
                        VMaterialRepair vMaterialRepair = new VMaterialRepair(theMaterial, list_RepairCon);
                        vMaterialRepair.OpenMaterialRepair(theCurrentRow.Cells["维修费用"].Tag as IList);
                        IList list_repairContent = vMaterialRepair.Result;
                        theCurrentRow.Cells["维修费用"].Tag = vMaterialRepair.Result;

                        //查询并设置维修单价
                        decimal repairMoney = 0;
                        foreach (MaterialRepair repair in list_repairContent)
                        {
                            int temp_count = 0;
                            foreach (MaterialRentalOrderDetail detail in matRentalOrderMaster.Details)
                            {
                                Material currentMaterial = theCurrentRow.Cells[colMaterialCode.Name].Tag as Material;
                                if (detail.MaterialResource.Id == currentMaterial.Id)
                                {
                                    foreach (BasicDtlCostSet costSetDtl in detail.BasicDtlCostSets)
                                    {
                                        if (costSetDtl.SetType == "维修设置")
                                        {
                                            if (costSetDtl.WorkContent == repair.WorkContent)
                                            {
                                                repair.Price = costSetDtl.Price;
                                                temp_count++;
                                            }
                                        }
                                    }
                                }
                                //如果未找到价格定义，则赋值 0
                                if (temp_count == 0)
                                {
                                    repair.Price = 0;
                                }
                            }
                            repairMoney = repairMoney + repair.Quantity * repair.Price;
                        }
                        theCurrentRow.Cells["维修费用"].Value = repairMoney.ToString();
                    }
                }
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
                matReturnMaster = new MaterialReturnMaster();
                matReturnMaster.CreatePerson = ConstObject.LoginPersonInfo;
                matReturnMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                matReturnMaster.CreateDate = ConstObject.LoginDate;
                matReturnMaster.CreateYear = ConstObject.LoginDate.Year;
                matReturnMaster.CreateMonth = ConstObject.LoginDate.Month;
                matReturnMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                matReturnMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                matReturnMaster.HandlePerson = ConstObject.LoginPersonInfo;
                matReturnMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                matReturnMaster.DocState = DocumentState.Edit;
                matReturnMaster.BalState = 0;//结算状态 0：未结算  1; 已结算

                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //制单日期
                txtCreateDate.Text = ConstObject.LoginDate.ToShortDateString();
                //责任人
                txtHandlePerson.Tag = ConstObject.LoginPersonInfo;
                txtHandlePerson.Text = ConstObject.LoginPersonInfo.Name;
                //归属项目
                //btnStates(0);
                if (projectInfo != null)
                {
                    txtProject.Tag = projectInfo;
                    txtProject.Text = projectInfo.Name;
                    matReturnMaster.ProjectId = projectInfo.Id;
                    matReturnMaster.ProjectName = projectInfo.Name;
                }
                txtContractNo.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("新建单据错误：" + ExceptionUtil.ExceptionMessage(ex));
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
                if (!ValidKC()) return false;
                if (matReturnMaster.Id == null)
                {
                    matReturnMaster.DocState = DocumentState.InExecute;
                    matReturnMaster = model.MatMngSrv.SaveMaterialReturnMaster(matReturnMaster);
                }
                else
                {
                    matReturnMaster.DocState = DocumentState.InExecute;
                    matReturnMaster = model.MatMngSrv.UpdateMaterialReturnMaster(matReturnMaster);
                }

                //插入日志
                //MStockIn.InsertLogData(matReturnMaster.Id, "保存", matReturnMaster.Code, matReturnMaster.CreatePerson.Name, "料具退料单","");
                txtCode.Text = matReturnMaster.Code;
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                FillDoc();
                //btnStates(0);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存数据出错：" + ExceptionUtil.ExceptionMessage(ex));
                return false;
            }
            return true;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <returns></returns>
        public override bool CancelView()
        {

            return true;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
            if (matReturnMaster.DocState == DocumentState.Edit || matReturnMaster.DocState == DocumentState.Valid)
            {
                base.ModifyView();
                matReturnMaster = model.MatMngSrv.GetMaterialReturnById(matReturnMaster.Id);
                ModelToView();
                FillDoc();
                //btnStates(1);
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(matReturnMaster.DocState));
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
                matReturnMaster = model.MatMngSrv.GetMaterialReturnById(matReturnMaster.Id);
                if (matReturnMaster.DocState == DocumentState.Valid || matReturnMaster.DocState == DocumentState.Edit)
                {
                    if (!model.MatMngSrv.DeleteMaterialReturn(matReturnMaster)) return false;
                    ClearView();
                    FillDoc();
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(matReturnMaster.DocState) + "】，不能删除！");
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据删除错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public override void RefreshView()
        {
            base.RefreshView();
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns></returns>
        private bool ModelToView()
        {
            try
            {
                this.txtCode.Text = matReturnMaster.Code;
                operDate.Value = matReturnMaster.CreateDate ;
                this.txtContractNo.Text = matReturnMaster.OldContractNum;
                if (matReturnMaster.HandlePerson != null)
                {
                    txtHandlePerson.Tag = matReturnMaster.HandlePerson;
                    txtHandlePerson.Text = matReturnMaster.HandlePersonName;
                }
                if (matReturnMaster.TheSupplierRelationInfo != null)
                {
                    txtSupply.Result.Clear();
                    txtSupply.Tag = matReturnMaster.TheSupplierRelationInfo;
                    txtSupply.Result.Add(matReturnMaster.TheSupplierRelationInfo);
                    txtSupply.Value = matReturnMaster.SupplierName;
                }
                if (matReturnMaster.TheRank != null)
                {
                    txtRank.Result.Clear();
                    txtRank.Tag = matReturnMaster.TheRank;
                    txtRank.Result.Add(matReturnMaster.TheRank);
                    txtRank.Value = matReturnMaster.TheRankName;
                }
                if (matReturnMaster.CreatePerson != null)
                {
                    txtCreatePerson.Tag = matReturnMaster.CreatePerson;
                    txtCreatePerson.Text = matReturnMaster.CreatePersonName;
                }
                txtRemark.Text = matReturnMaster.Descript;
                txtCreateDate.Text = matReturnMaster.CreateDate.ToShortDateString();
                txtSumQuantity.Text = matReturnMaster.SumExitQuantity.ToString();
                txtProject.Text = matReturnMaster.ProjectName;
                txtProject.Tag = matReturnMaster.ProjectId;
                txtTransChagre.Text = ClientUtil.ToString(matReturnMaster.TransportCharge);
                txtBalRule.Text = matReturnMaster.BalRule;
                FillDoc();
                //显示收料单明细
                this.dgDetail.Rows.Clear();

                //查询当前料具供应商合同租赁单价
                MaterialRentalOrderMaster theMaterialRentalOrderMaster = new MaterialRentalOrderMaster();
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", this.txtSupply.Result[0] as SupplierRelationInfo));
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                IList list = model.MatMngSrv.GetMaterialRentalOrder(oq) as IList;
                if (list.Count > 0)
                {
                    theMaterialRentalOrderMaster = list[0] as MaterialRentalOrderMaster;
                }

                foreach (MaterialReturnDetail var in matReturnMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();
                    //设置物料
                    this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                    this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                    this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;
                    //设置该物料的计量单位
                    this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
                    this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnitName;


                    this.dgDetail[colPrice.Name, i].Value = var.Price;
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;

                    //设置使用部位
                    this.dgDetail[colUsedPart.Name, i].Tag = var.UsedPart;
                    this.dgDetail[colUsedPart.Name, i].Value = var.UsedPartName;

                    //设置使用队伍
                    this.dgDetail[this.colBorrowUnit.Name, i].Tag = var.BorrowUnit;
                    this.dgDetail[colBorrowUnit.Name, i].Value = var.BorrowUnitName;

                    this.dgDetail[colExitQuantity.Name, i].Value = var.ExitQuantity;
                    this.dgDetail[colExitQuantity.Name, i].Tag = var.ExitQuantity;
                    this.dgDetail[colTempQty.Name, i].Value = var.ExitQuantity;
                    this.dgDetail[colBroachQuantity.Name, i].Value = var.BroachQuantity;
                    this.dgDetail[colRejectQuantity.Name, i].Value = var.RejectQuantity;
                    this.dgDetail[colConsumeQuantity.Name, i].Value = var.ConsumeQuantity;
                    this.dgDetail[colDisCardQty.Name, i].Value = var.DisCardQty;
                    this.dgDetail[colRepairQty.Name, i].Value = var.RepairQty;
                    this.dgDetail[colLossQty.Name, i].Value = var.LossQty;

                    //查询当前出租方，队伍料具库存
                    decimal stockQty = model.MatMngSrv.GetMatStockQty(this.txtSupply.Result[0] as SupplierRelationInfo, var.BorrowUnit, var.MaterialResource, projectInfo.Id);

                    this.dgDetail[colCategoryQuantity.Name, i].Value = stockQty;

                    foreach (MaterialRentalOrderDetail theDetail in theMaterialRentalOrderMaster.Details)
                    {
                        if (theDetail.MaterialResource.Id == var.MaterialResource.Id)
                        {
                            this.dgDetail[colPrice.Name, i].Value = theDetail.Price;
                        }
                    }
                    foreach (MaterialReturnCostDtl costDtl in var.MatReturnCostDtls)
                    {
                        IList list_temp = new ArrayList();
                        if (costDtl.CostType == "维修费用")
                        {
                            foreach (MaterialRepair materialRepait in var.MatRepairs)
                            {
                                list_temp.Add(materialRepait);
                            }
                            this.dgDetail[costDtl.CostType, i].Tag = list_temp;
                        }
                        this.dgDetail[costDtl.CostType, i].Value = costDtl.Money;
                    }
                    this.dgDetail.Rows[i].Tag = var;
                }
                
            }
            catch (Exception ex)
            {
                throw;
            }
            return true;
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
                decimal sumMoney = 0;
                if (this.txtSupply.Result.Count > 0)
                {
                    matReturnMaster.TheSupplierRelationInfo = this.txtSupply.Result[0] as SupplierRelationInfo;
                    matReturnMaster.SupplierName = this.txtSupply.Text;
                }
                if (this.txtRank.Result.Count > 0)
                {
                    matReturnMaster.TheRank = this.txtRank.Result[0] as SupplierRelationInfo;
                    matReturnMaster.TheRankName = this.txtRank.Text;
                }
                matReturnMaster.CreateDate  = operDate.Value.Date;
                matReturnMaster.OldContractNum = this.txtContractNo.Text;
                matReturnMaster.Descript = this.txtRemark.Text;
                matReturnMaster.SumExitQuantity = ClientUtil.ToDecimal(txtSumQuantity.Text);
                matReturnMaster.BalRule = txtBalRule.Text;
                if (txtTransChagre.Text != "")
                {
                    matReturnMaster.TransportCharge = TransUtil.ToInt(this.txtTransChagre.Text);
                }
                if (returnType == 1)
                {
                    matReturnMaster.ReturnType = 1;
                }
                else if (returnType == 2)
                {
                    matReturnMaster.ReturnType = 2;
                }

                //退料明细
                MatRenLed_list = new ArrayList();
                matReturnMaster.Details.Clear();
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    MaterialReturnDetail theMaterialReturnDetail = new MaterialReturnDetail();
                    if (var.Tag != null)
                    {
                        theMaterialReturnDetail = var.Tag as MaterialReturnDetail;
                        if (theMaterialReturnDetail.Id == null)
                        {
                            matReturnMaster.Details.Remove(theMaterialReturnDetail);
                        }
                    }
                    //材料
                    theMaterialReturnDetail.MaterialResource = var.Cells[colMaterialCode.Name].Tag as Material;
                    theMaterialReturnDetail.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);
                    theMaterialReturnDetail.MaterialName = ClientUtil.ToString(var.Cells[colMaterialName.Name].Value);
                    theMaterialReturnDetail.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);
                    //计量单位
                    theMaterialReturnDetail.MatStandardUnit = var.Cells[colUnit.Name].Tag as StandardUnit;
                    theMaterialReturnDetail.MatStandardUnitName = ClientUtil.ToString(var.Cells[colUnit.Name].Value);

                    theMaterialReturnDetail.ExitQuantity = ClientUtil.ToDecimal(var.Cells[colExitQuantity.Name].Value);//退场数量
                    decimal tempQty = ClientUtil.ToDecimal(var.Cells[colTempQty.Name].Value);
                    theMaterialReturnDetail.TempData = tempQty.ToString();
                    theMaterialReturnDetail.RejectQuantity = ClientUtil.ToDecimal(var.Cells[colRejectQuantity.Name].Value);//报废数量
                    theMaterialReturnDetail.BroachQuantity = ClientUtil.ToDecimal(var.Cells[colBroachQuantity.Name].Value);//完好数量
                    theMaterialReturnDetail.ConsumeQuantity = ClientUtil.ToDecimal(var.Cells[colConsumeQuantity.Name].Value);//消耗数量
                    theMaterialReturnDetail.LossQty = ClientUtil.ToDecimal(var.Cells[colLossQty.Name].Value);//报损数量
                    theMaterialReturnDetail.DisCardQty = ClientUtil.ToDecimal(var.Cells[colDisCardQty.Name].Value);//切头数量
                    theMaterialReturnDetail.RepairQty = ClientUtil.ToDecimal(var.Cells[colRepairQty.Name].Value);//维修数量
                    //使用部位
                    if (var.Cells[colUsedPart.Name].Tag != null)
                    {
                        theMaterialReturnDetail.UsedPart = var.Cells[colUsedPart.Name].Tag as GWBSTree;
                        theMaterialReturnDetail.UsedPartName = ClientUtil.ToString(var.Cells[colUsedPart.Name].Value);
                        theMaterialReturnDetail.UsedPartSysCode = ClientUtil.ToString((var.Cells[colUsedPart.Name].Tag as GWBSTree).SysCode);
                    }
                    if (var.Cells[colSubject.Name].Tag != null)
                    {
                        theMaterialReturnDetail.SubjectGUID = var.Cells[colSubject.Name].Tag as CostAccountSubject;
                        theMaterialReturnDetail.SubjectName = ClientUtil.ToString(var.Cells[colSubject.Name].Value);
                        theMaterialReturnDetail.SubjectSysCode = ClientUtil.ToString((var.Cells[colSubject.Name].Tag as CostAccountSubject).SysCode);
                    }
                    if (var.Cells[this.colBorrowUnit.Name].Tag != null)
                    {
                        theMaterialReturnDetail.BorrowUnit = var.Cells[colBorrowUnit.Name].Tag as SupplierRelationInfo;
                        theMaterialReturnDetail.BorrowUnitName = ClientUtil.ToString(var.Cells[colBorrowUnit.Name].Value);
                    }
                    theMaterialReturnDetail.RentalPrice = ClientUtil.ToDecimal(var.Cells[colPrice.Name].Value);
                    theMaterialReturnDetail.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                    //处理费用明细
                    //先清除已有费用明细
                    IList list_temp = new ArrayList();
                    foreach (MaterialReturnCostDtl returnCostDtl in theMaterialReturnDetail.MatReturnCostDtls)
                    {
                        list_temp.Add(returnCostDtl);
                    }
                    foreach (MaterialReturnCostDtl detail in list_temp)
                    {
                        theMaterialReturnDetail.MatReturnCostDtls.Remove(detail);
                    }
                    //然后新增
                    foreach (string costType in list_costType_detail)
                    {
                        MaterialReturnCostDtl matCost = new MaterialReturnCostDtl();
                        if (costType == "维修费用")
                        {
                            if (var.Cells[costType].Tag != null)
                            {
                                foreach (MaterialRepair materialRepair in var.Cells[costType].Tag as IList)
                                {
                                    theMaterialReturnDetail.AddMatRepairs(materialRepair);
                                }
                            }
                            matCost.CostType = costType;
                            matCost.Money = ClientUtil.ToDecimal(var.Cells[costType].Value);
                            sumMoney += matCost.Money;
                        }
                        else
                        {
                            matCost.CostType = costType;
                            matCost.Money = ClientUtil.ToDecimal(var.Cells[costType].Value);
                            sumMoney += matCost.Money;
                        }

                        theMaterialReturnDetail.AddMatReturnCostDtls(matCost);
                    }

                    matReturnMaster.AddDetail(theMaterialReturnDetail);
                }

                matReturnMaster.SumExtMoney = sumMoney;

                //运输费
                decimal TransChagre = 0;
                if (txtTransChagre.Text == "")
                {
                    TransChagre = 0;
                }
                else
                {
                    TransChagre = ClientUtil.ToDecimal(txtTransChagre.Text);
                }
                matReturnMaster.SumExtMoney += TransChagre;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存数据出错：" + ExceptionUtil.ExceptionMessage(ex));
                return false;
            }
            return true;
        }
        /// <summary>
        /// 保存前校验库存
        /// </summary>
        /// <returns></returns>
        private bool ValidKC()
        {
            //校验当前退料单是否是插入的单据
            bool value = model.MatMngSrv.VerifyReturnMatBusinessDate(operDate.Value.Date, projectInfo.Id);
            //校验库存情况
            DataDomain domain = model.MatMngSrv.VerifyReturnMatKC(matReturnMaster, value);
            if (ClientUtil.ToInt(domain.Name1) == 0)
            {
                //通过
                return true;
            }
            else if (ClientUtil.ToInt(domain.Name1) == 1)
            {
                //当前库存不足
                MessageBox.Show(domain.Name2.ToString());
                return false;
            }
            else if (ClientUtil.ToInt(domain.Name1) == 2)
            {
                //插入业务日期的库存不足
                MessageBox.Show(domain.Name2.ToString());
                return false;
            }
            else if (ClientUtil.ToInt(domain.Name1) == 3)
            {
                //插入该笔退料后，业务日期[yyyy-MM-dd]的库存为负[yyyy-MM-dd]是计算中的第一笔负数的日期
                MessageBox.Show(domain.Name2.ToString());
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 保存数据前校验数据
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            string validMessage = "";
            if (this.dgDetail.Rows.Count - 1 == 0)
            {
                MessageBox.Show("收料单明细不能为空");
                return false;
            }

            dgDetail.EndEdit();
            dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));

            //业务日期不得大于服务器日期
            DateTime ServerDate = CommonMethod.GetServerDateTime();
            DateTime OperDate = this.operDate.Value.Date;
            if (DateTime.Compare(OperDate, ServerDate) > 0)
            {
                validMessage += "业务日期不得大于服务器日期！";
            }
            if (txtContractNo.Text == "")
            {
                validMessage += "原始合同号不能为空！\n";
            }
            if (txtSupply.Result.Count == 0)
            {
                validMessage += "出租方不能为空！\n";
            }

            if (validMessage != "")
            {
                MessageBox.Show(validMessage);
                return false;
            }
            //收料单明细表数据校验
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

                //输入维修数量时必须选择维修内容和维修数量
                if (dr.Cells[colRepairQty.Name].Value != null && dr.Cells[colRepairQty.Name].Value.ToString() != "")
                {
                    decimal repairQty = ClientUtil.ToDecimal(dr.Cells[colRepairQty.Name].Value);
                    if (repairQty != 0)
                    {
                        //查找是否有维修费列
                        int count = 0;
                        foreach (DataGridViewColumn theDataGridViewColumn in dr.DataGridView.Columns)
                        {
                            if (theDataGridViewColumn.HeaderText == "维修费用")
                            {
                                count++;
                            }
                        }
                        if (count > 0)
                        {
                            DataGridViewCell theDataGridViewCell = dr.Cells["维修费用"] as DataGridViewCell;
                            if (theDataGridViewCell != null)
                            {
                                IList lst = dr.Cells["维修费用"].Tag as IList;
                                if (lst == null || lst.Count == 0)
                                {
                                    MessageBox.Show("请双击 [维修费用] 单元格设置维修内容和数量，系统自动计算维修费！");
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("该料具商合同中未定义维修费用，请修改合同，此单不能保存！");
                            return false;
                        }
                    }
                }

                if (dr.Cells[colPrice.Name].Value == null || dr.Cells[colPrice.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colPrice.Name].Value) <= 0)
                {
                    MessageBox.Show("单价不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colPrice.Name];
                    return false;
                }

                //string materialName = dr.Cells[colMaterialName.Name].Value.ToString();
                //string materialSpec = dr.Cells[colMaterialSpec.Name].Value.ToString();

                //if (dr.Cells[colExitQuantity.Name].Tag != null)
                //{
                //    decimal oldQuantity = ClientUtil.ToDecimal(dr.Cells[colExitQuantity.Name].Tag);
                //    decimal stockQuantity = ClientUtil.ToDecimal(dr.Cells[colCategoryQuantity.Name].Value);
                //    decimal sumQuantity = oldQuantity + stockQuantity;
                //    if (ClientUtil.ToDecimal(dr.Cells[colExitQuantity.Name].Value) - oldQuantity - stockQuantity > 0)
                //    {
                //        MessageBox.Show("[" + materialName + "]" + "[" + materialSpec + "]" + "库存量为[" + sumQuantity + "],退料数量大于库存，请修改！");
                //        return false;
                //    }
                //}
                //else
                //{
                //    decimal stockQty = ClientUtil.ToDecimal(dr.Cells[colCategoryQuantity.Name].Value);
                //    decimal exitQuantity = ClientUtil.ToDecimal(dr.Cells[colExitQuantity.Name].Value);
                //    if (exitQuantity > stockQty)
                //    {
                //        MessageBox.Show("[" + materialName + "]" + "[" + materialSpec + "]" + "退料数量大于库存数量，请修改！");
                //        return false;
                //    }
                //}
            }

            dgDetail.Update();
            return true;
        }

        void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            if (colName == colBroachQuantity.Name || colName == colRepairQty.Name || colName == colDisCardQty.Name ||
                 colName == colRejectQuantity.Name || colName == colConsumeQuantity.Name || colName == colLossQty.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colBroachQuantity.Name].Value != null)
                {
                    string temp_qty = dgDetail.Rows[e.RowIndex].Cells[colBroachQuantity.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_qty);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
                if (dgDetail.Rows[e.RowIndex].Cells[colRejectQuantity.Name].Value != null)
                {
                    string temp_qty = dgDetail.Rows[e.RowIndex].Cells[colRejectQuantity.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_qty);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
                if (dgDetail.Rows[e.RowIndex].Cells[colConsumeQuantity.Name].Value != null)
                {
                    string temp_qty = dgDetail.Rows[e.RowIndex].Cells[colConsumeQuantity.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_qty);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
                if (dgDetail.Rows[e.RowIndex].Cells[colLossQty.Name].Value != null)
                {
                    string temp_qty = dgDetail.Rows[e.RowIndex].Cells[colLossQty.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_qty);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
                if (dgDetail.Rows[e.RowIndex].Cells[colDisCardQty.Name].Value != null)
                {
                    string temp_qty = dgDetail.Rows[e.RowIndex].Cells[colDisCardQty.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_qty);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
                if (dgDetail.Rows[e.RowIndex].Cells[colRepairQty.Name].Value != null)
                {
                    string temp_qty = dgDetail.Rows[e.RowIndex].Cells[colRepairQty.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_qty);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
                //完整数量
                decimal broachQuantity = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colBroachQuantity.Name].Value);
                //报废数量
                decimal rejectQuantity = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colRejectQuantity.Name].Value);
                //消耗数量
                decimal consumeQuantity = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colConsumeQuantity.Name].Value);
                //报损数量
                decimal lossQuantity = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colLossQty.Name].Value);
                //切头数量
                decimal disCardQuantity = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colDisCardQty.Name].Value);
                //维修数量
                decimal repairQuantity = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colRepairQty.Name].Value);

                this.dgDetail.Rows[e.RowIndex].Cells[colExitQuantity.Name].Value = broachQuantity + rejectQuantity + consumeQuantity + lossQuantity + disCardQuantity + repairQuantity;

                //计算退料总数量
                object quantity = dgDetail.Rows[e.RowIndex].Cells[colExitQuantity.Name].Value;
                decimal sumqty = 0;

                for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                {
                    quantity = dgDetail.Rows[i].Cells[colExitQuantity.Name].Value;
                    if (quantity == null) quantity = 0;
                    sumqty = sumqty + ClientUtil.TransToDecimal(quantity);
                }

                txtSumQuantity.Text = sumqty.ToString();

                //计算当前全部费用金额
                foreach (string costType in list_costType_detail)
                {
                    if (!costType.ToString().Contains("维修费"))
                    {
                        string expression = GetCostExpression(costType, this.dgDetail.CurrentRow.Cells[colMaterialCode.Name].Tag as Material);
                        if (expression != "" && expression != null)
                        {
                            MaterialReturnDetail detail = new MaterialReturnDetail();
                            detail.BroachQuantity = broachQuantity;
                            detail.RejectQuantity = rejectQuantity;
                            detail.ConsumeQuantity = consumeQuantity;
                            detail.LossQty = lossQuantity;
                            detail.DisCardQty = disCardQuantity;
                            detail.RepairQty = repairQuantity;

                            this.dgDetail.CurrentRow.Cells[costType].Value = GetCalResult(detail, expression);
                        }
                        else
                        {
                            this.dgDetail.CurrentRow.Cells[costType].Value = 0;
                        }
                    }
                }
            }
        }

        void txtSupply_TextChanged(object sender, EventArgs e)
        {
            if (txtSupply.Result.Count > 0)
            {
                //清除动态加载列
                foreach (int i in list_index_detail)
                {
                    dgDetail.Columns.RemoveAt(i);
                }

                list_index_detail = new ArrayList();
                list_costType_detail = new ArrayList();
                list_index_costDetail = new ArrayList();
                list_costType_costDetail = new ArrayList();

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", this.txtSupply.Result[0] as SupplierRelationInfo));
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                IList list = model.MatMngSrv.GetMaterialRentalOrder(oq) as IList;
                if (list.Count > 0)
                {
                    IList list_temp = new ArrayList();
                    matRentalOrderMaster = list[0] as MaterialRentalOrderMaster;

                    txtBalRule.Text = matRentalOrderMaster.BalRule;
                    txtContractNo.Text = matRentalOrderMaster.OriginalContractNo;
                    if (matReturnMaster.Id == null)
                    {
                        if (returnType == 1)
                        {
                            foreach (BasicCostSet costSet in matRentalOrderMaster.BasiCostSets)
                            {
                                if (costSet.MatCostType.Contains("报损") || costSet.MatCostType.Contains("消耗"))
                                {
                                    list_temp.Add(costSet);
                                }
                            }
                            foreach (BasicCostSet CostSet in list_temp)
                            {
                                matRentalOrderMaster.BasiCostSets.Remove(CostSet);
                            }

                            foreach (BasicCostSet theBasicCostSet in matRentalOrderMaster.BasiCostSets)
                            {
                                if (theBasicCostSet.ExitCalculation == 1)
                                {
                                    DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                                    column.HeaderText = theBasicCostSet.MatCostType;
                                    column.Name = theBasicCostSet.MatCostType;
                                    if (theBasicCostSet.MatCostType.Contains("维修费"))
                                    {
                                        column.ReadOnly = true;
                                    }
                                    else
                                    {
                                        column.ReadOnly = false;
                                    }
                                    dgDetail.Columns.Insert(15, column);
                                    list_index_detail.Add(dgDetail.Columns[theBasicCostSet.MatCostType].Index);
                                    list_costType_detail.Add(theBasicCostSet.MatCostType);
                                }
                            }
                        }
                        else if (returnType == 2)
                        {
                            foreach (BasicCostSet theBasicCostSet in matRentalOrderMaster.BasiCostSets)
                            {
                                if (theBasicCostSet.MatCostType.Contains("报损") || theBasicCostSet.MatCostType.Contains("消耗"))
                                {
                                    DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                                    column.HeaderText = theBasicCostSet.MatCostType;
                                    column.Name = theBasicCostSet.MatCostType;
                                    column.ReadOnly = true;
                                    dgDetail.Columns.Insert(15, column);
                                    list_index_detail.Add(dgDetail.Columns[theBasicCostSet.MatCostType].Index);
                                    list_costType_detail.Add(theBasicCostSet.MatCostType);
                                }
                            }
                        }
                    }
                    else
                    {
                        Hashtable hs = new Hashtable();
                        int index = 1;
                        foreach (MaterialReturnDetail theMaterialReturnDetail in matReturnMaster.Details)
                        {
                            foreach (MaterialReturnCostDtl theMaterialReturnCostDtl in theMaterialReturnDetail.MatReturnCostDtls)
                            {
                                if (!hs.ContainsValue(theMaterialReturnCostDtl.CostType))
                                {
                                    hs.Add(index, theMaterialReturnCostDtl.CostType);
                                    index++;
                                }
                            }
                        }
                        foreach (int keys in hs.Keys)
                        {
                            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                            column.HeaderText = hs[keys].ToString();
                            column.Name = hs[keys].ToString();
                            if (hs[keys].ToString().Contains("维修费"))
                            {
                                column.ReadOnly = true;
                            }
                            else
                            {
                                column.ReadOnly = false;
                            }
                            dgDetail.Columns.Insert(15, column);
                            list_index_detail.Add(dgDetail.Columns[hs[keys].ToString()].Index);
                            list_costType_detail.Add(hs[keys].ToString());
                        }
                    }
                }
            }
        }

        #region 固定代码
        /// <summary>
        /// 启动本窗体,(设置状态或重新加载已有的数据)
        /// </summary>
        /// <param name="code">窗体Caption</param>
        public void Start(string id)
        {
            try
            {
                btnStates(0);
                if (id == "空")
                    RefreshState(MainViewState.Initialize);
                else
                {
                    matReturnMaster = model.MatMngSrv.GetMaterialReturnById(id);
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
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
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
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate, txtHandlePerson, txtSumQuantity, txtProject, txtContractNo, txtBalRule };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colMaterialName.Name, colMaterialSpec.Name, colUnit.Name, colBorrowUnit.Name, colUsedPart.Name, colSubject.Name };
            dgDetail.SetColumnsReadOnly(lockCols);
        }

        //清空数据
        private void ClearView()
        {
            ClearControl(pnlFloor);
            txtSupply.Result.Clear();
            txtRank.Result.Clear();
            txtSupply.Text = "";
            txtRank.Text = "";
        }

        private void ClearControl(Control c)
        {
            try
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
            catch (Exception)
            {
                throw;
            }

        }

        #region 解析表达式，计算费用金额
        //通过计算表达式，计算本单元格的计算值
        private decimal GetCalResult(MaterialReturnDetail detail, string express)
        {
            decimal result = 0;

            if (express.IndexOf("完好") != -1)
            {
                express = express.Replace("完好", detail.BroachQuantity.ToString());
            }
            if (express.IndexOf("报损") != -1)
            {
                express = express.Replace("报损", detail.LossQty.ToString());
            }
            if (express.IndexOf("报废") != -1)
            {
                express = express.Replace("报废", detail.RejectQuantity.ToString());
            }
            if (express.IndexOf("消耗") != -1)
            {
                express = express.Replace("消耗", detail.ConsumeQuantity.ToString());
            }
            if (express.IndexOf("切头") != -1)
            {
                express = express.Replace("切头", detail.DisCardQty.ToString());
            }
            if (express.IndexOf("维修") != -1)
            {
                express = express.Replace("维修", detail.RepairQty.ToString());
            }
            result = Decimal.Parse(CommonUtil.CalculateExpression(express, 2));
            return result;
        }
        //获取表达式
        private string GetCostExpression(string costType, Material material)
        {
            string expression = "";
            foreach (MaterialRentalOrderDetail detail in matRentalOrderMaster.Details)
            {
                if (material != null && detail.MaterialResource.Id == material.Id)
                {
                    foreach (BasicDtlCostSet theBasicDtlCostSet in detail.BasicDtlCostSets)
                    {
                        if (theBasicDtlCostSet.CostType == costType && theBasicDtlCostSet.SetType == "公式定义")
                        {
                            expression = theBasicDtlCostSet.ExitExpression;
                            break;
                        }
                    }
                }
            }

            //获取全部价格类型
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATENAME_PRICETYPE);
            foreach (MaterialRentalOrderDetail detail in matRentalOrderMaster.Details)
            {
                if (material != null && detail.MaterialResource.Id == material.Id)
                {
                    foreach (BasicDataOptr theBasicDataOptr in list)
                    {
                        string priceType = "";
                        if (expression != null && expression.IndexOf(theBasicDataOptr.BasicName) != -1)
                        {
                            priceType = theBasicDataOptr.BasicName;
                        }
                        if (priceType != "")
                        {
                            int count = 0;
                            foreach (BasicDtlCostSet theBasicDtlCostSet in detail.BasicDtlCostSets)
                            {
                                //取基本费用明细中的价格类型
                                if (theBasicDtlCostSet.SetType == "价格定义")
                                {
                                    if (theBasicDtlCostSet.CostType == priceType)
                                    {
                                        string price = theBasicDtlCostSet.Price.ToString();
                                        expression = expression.Replace(priceType, price);
                                        count++;
                                    }
                                }
                            }
                            //count=0：用户未定义当前价格类型，此时赋值0
                            if (count == 0)
                            {
                                expression = expression.Replace(priceType, ClientUtil.ToString(0));
                            }
                        }
                    }
                }
            }
            return expression;
        }
        #endregion
        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            if (LoadTempleteFile(@"料具退料单.flx") == false) return false;
            FillFlex(matReturnMaster);
            flexGrid1.PrintPreview(true,true,true,0,0,0,0,0);
            return true;
        }

        public override bool Print()
        {
            if (LoadTempleteFile(@"料具退料单.flx") == false) return false;
            FillFlex(matReturnMaster);
            flexGrid1.Print();
            matReturnMaster.PrintTimes = matReturnMaster.PrintTimes + 1;
            matReturnMaster = model.MatMngSrv.UpdateMaterialReturnMaster(matReturnMaster);
            return true;
        }

        public override bool Export()
        {
            if (LoadTempleteFile(@"料具退料单.flx") == false) return false;
            FillFlex(matReturnMaster);
            flexGrid1.ExportToExcel("料具退料单【" + matReturnMaster.Code + "】", false, false, true);
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

        private void FillFlex(MaterialReturnMaster billMaster)
        {
            int detailStartRowNumber = 7;//7为模板中的行号
            int detailCount = billMaster.Details.Count;

            //插入明细行
            flexGrid1.InsertRow(detailStartRowNumber, detailCount);

            //设置单元格的边框，对齐方式
            FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1,

flexGrid1.Cols - 1);
            range.Alignment = FlexCell.AlignmentEnum.RightCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            range.Mask = FlexCell.MaskEnum.Digital;
            CommonUtil.SetFlexGridPrintFace(this.flexGrid1);
            //主表数据

            flexGrid1.Cell(3, 3).Text = billMaster.Code;
            flexGrid1.Cell(3, 10).Text = billMaster.ForwardBillCode;
            flexGrid1.Cell(4, 12).Text = billMaster.SupplierName;
            flexGrid1.Cell(4, 16).Text = billMaster.RealOperationDate.ToShortDateString();
            flexGrid1.Cell(4, 3).Text = billMaster.TheRankName;
            flexGrid1.Cell(4, 8).Text = ClientUtil.ToString(billMaster.TransportCharge);
            flexGrid1.Cell(3, 11).Text = ClientUtil.ToString(billMaster.BalRule);
          
            //填写明细数据
            decimal sumQuantity = 0;
            decimal suMoney = 0;
            for (int i = 0; i < detailCount; i++)
            {
                MaterialReturnDetail detail = (MaterialReturnDetail)billMaster.Details.ElementAt(i);
                //物资名称
                flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialName;//detail.MaterialResource.Name;
                flexGrid1.Cell(detailStartRowNumber + i, 1).WrapText = true;
                //规格型号
                flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialSpec;//detail.MaterialResource.Specification;

                //计量单位
                flexGrid1.Cell(detailStartRowNumber + i, 3).Text = detail.MatStandardUnitName;

                //租赁单价
                flexGrid1.Cell(detailStartRowNumber + i, 4).Text = ClientUtil.ToString(detail.RentalPrice);

                //使用部位
                flexGrid1.Cell(detailStartRowNumber + i, 5).Text = ClientUtil.ToString(detail.UsedPartName);
                flexGrid1.Cell(detailStartRowNumber + i, 5).WrapText = true;
                //库存数量
                flexGrid1.Cell(detailStartRowNumber + i, 6).Text = ClientUtil.ToString(detail.Quantity);
                //退场数量
                flexGrid1.Cell(detailStartRowNumber + i, 7).Text = ClientUtil.ToString(detail.ExitQuantity);
                //flexGrid1.Cell(detailStartRowNumber + i, 8).Text = ClientUtil.ToString(detail.ExitQuantity);
                sumQuantity += detail.ExitQuantity;
                //完好数量
                flexGrid1.Cell(detailStartRowNumber + i, 8).Text = ClientUtil.ToString(detail.BroachQuantity);
                //flexGrid1.Cell(detailStartRowNumber + i, 9).Text = ClientUtil.ToString(detail.BroachQuantity);
                //维修数量
                flexGrid1.Cell(detailStartRowNumber + i, 9).Text = ClientUtil.ToString(detail.RepairQty);
                //切头数量
                flexGrid1.Cell(detailStartRowNumber + i, 10).Text = ClientUtil.ToString(detail.DisCardQty);
                //flexGrid1.Cell(detailStartRowNumber + i, 11).Text = ClientUtil.ToString(detail.DisCardQty);
                //报废数量
                flexGrid1.Cell(detailStartRowNumber + i, 11).Text = ClientUtil.ToString(detail.RejectQuantity);
                //保养费
                flexGrid1.Cell(detailStartRowNumber + i, 12).Text = "";
                //维修费
                flexGrid1.Cell(detailStartRowNumber + i, 13).Text = "";
                //补偿费
                flexGrid1.Cell(detailStartRowNumber + i, 14).Text = "";
                //力资费
                flexGrid1.Cell(detailStartRowNumber + i, 15).Text = "";
                //备注
                flexGrid1.Cell(detailStartRowNumber + i, 16).Text = ClientUtil.ToString(detail.Descript);
                flexGrid1.Cell(detailStartRowNumber + i, 16).WrapText = true;
               
                flexGrid1.Row(detailStartRowNumber + i).AutoFit();
            }
            flexGrid1.Cell(7 + detailCount, 3).Text = billMaster.ProjectName;
            flexGrid1.Cell(7 + detailCount, 9).Text = billMaster.CreateDate.ToShortDateString();
            flexGrid1.Cell(7 + detailCount, 12).Text = ClientUtil.ToString(sumQuantity);
            flexGrid1.Cell(7 + detailCount, 14).Text = ClientUtil.ToString(suMoney);
            flexGrid1.Cell(7 + detailCount, 16).Text = billMaster.CreatePersonName;
            //条形码
            string a = Application.Business.Erp.SupplyChain.Client.Util.DateUtil.GetStrMoneyInchar(ClientUtil.ToString(suMoney));
            this.flexGrid1.Cell(1, 14).Text = billMaster.Code.Substring(billMaster.Code.Length - 11) + "-" + a;
            this.flexGrid1.Cell(1, 14).CellType = FlexCell.CellTypeEnum.BarCode;
            this.flexGrid1.Cell(1, 14).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;

        }

   


    }
}
