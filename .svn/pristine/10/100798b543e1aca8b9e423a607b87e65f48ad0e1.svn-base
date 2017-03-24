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
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyPlanMng;
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using VirtualMachine.Core;
using System.IO;
using System.Diagnostics;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using IRPServiceModel.Domain.Document;
using NHibernate.Criterion;
using IRPServiceModel.Basic;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using Application.Business.Erp.SupplyChain.Client.FileUpload;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyOrderMng
{
    public partial class VSupplyOrder : TMasterDetailView
    {
        private MSupplyOrderMng model = new MSupplyOrderMng();
        private MSupplyPlanMng Supplymodle = new MSupplyPlanMng();
        private MStockMng modelStockIn = new MStockMng();
        private SupplyPlanMaster supplyMaster;
        private SupplyOrderMaster curBillMaster;
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        IList list_price = new ArrayList();

        private ProObjectRelaDocument oprDocument = null;
        string userName = string.Empty;
        string jobId = string.Empty;
        string fileObjectType = string.Empty;
        private string buttonStr = "";
        
        public string[] arrMustRefDemandPlan = new string[] { "I1100112","I1100113","I1100102","I1100101","I1100111","I1100110","I11201"};
        private EnumSupplyType supplyType;
        /// <summary>
        /// 用来区分专业
        /// </summary>
        public EnumSupplyType SupplyType
        {
            get { return supplyType; }
            set { supplyType = value;
            
            }
        }

        /// <summary>
        /// 当前单据
        /// </summary>
        public SupplyOrderMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VSupplyOrder()
        {
            InitializeComponent();
            InitEvent();
            InitZYFL();
            InitProjectStatus();
        }

        private void InitProjectStatus()
        {
            //添加工程状态下拉框
            VBasicDataOptr.InitProjectStatus(colProjectStatus, false);
            dgDetail.ContextMenuStrip = cmsDg;
            VBasicDataOptr.InitProfessionCategory(cboProfessionCat, false);
        }

        private void InitZYFL()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            this.txtMaterialCategory.rootCatCode = "01";
            this.txtMaterialCategory.rootLevel = "3";
            //添加专业分类下拉框
            //VBasicDataOptr.InitProfessionCategory(colzyfl, false);
            dgDetail.ContextMenuStrip = cmsDg;
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode + "-";
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
            this.tabControl1.TabPages.RemoveByKey("tabPage2");
            list_price.Add("I1100101");
            list_price.Add("I1100102");
            list_price.Add("I1100110");
            list_price.Add("I1100111");
            list_price.Add("I1100112");
            list_price.Add("I1100113");
            this.colwzfl.Visible = false;

            cmobalanceStyle.Items.AddRange(new object[] {"过程结算", "末次结算", "质保期"});
        }

        private void InitEvent()
        {
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            this.dgDetail.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgDetail_EditingControlShowing);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            //this.dgDetail.UserDeletingRow += new DataGridViewRowCancelEventHandler(dgDetail_UserDeletingRow);
            //按钮事件
            this.btnDemandSearch.Click += new EventHandler(btnDemandSearch_Click);//需求总计划
            //this.btnSupplySearch.Click += new EventHandler(btnSupplySearch_Click);//采购计划
            this.btnSet.Click += new EventHandler(btnSet_Click);
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            txtMaterialCategory.Leave += new EventHandler(txtMaterialCategory_Leave);
            this.txtSupply.Leave += new EventHandler(txtSupply_Leave);
            cboProfessionCat.SelectedIndexChanged += new EventHandler(cboProfessionCat_SelectedIndexChanged);
            btnSetPP.Click +=new EventHandler(btnSetPP_Click);

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
            this.btnExcel.Click+=new EventHandler(btnExcel_Click);
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
                        curBillMaster =model.SupplyOrderSrv.AddSupplyOrder(curBillMaster, movedDtlList);
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
        //EXCEL导入
        
        void btnExcel_Click(object sender, EventArgs e)
        {
            VImportSupplyOrder oVImportSupplyOrder = new VImportSupplyOrder(this.dgDetail .Rows .Count );
            oVImportSupplyOrder.ShowDialog();
            Hashtable  htSupply = oVImportSupplyOrder.htResult ;
            SupplyOrderDetail oSupplyOrderDetail =null;
            string sCode=string.Empty ;
            DataGridViewRow oRow = null;

            if (htSupply != null && htSupply.Count > 0)
            {
                for (int i = 0; i < this.dgDetail.Rows.Count;i++ )
                {
                    oRow = this.dgDetail.Rows[i];
                    sCode = ClientUtil.ToString(oRow.Cells[colMaterialCode.Name].Value);
                    if (htSupply.ContainsKey(sCode))
                    {
                        oSupplyOrderDetail=htSupply[sCode] as SupplyOrderDetail;
                        if(oSupplyOrderDetail!=null)
                        {
                            oRow.Cells[colSupplyPrice.Name].Value = oSupplyOrderDetail.Price;
                            oRow.Cells[colSumMoney.Name].Value = oSupplyOrderDetail.Price*ClientUtil .ToDecimal (oRow.Cells[colQuantity.Name].Value );
                            oRow.Cells[colDescript.Name].Value = oSupplyOrderDetail.Descript;
                        }
                    }
                }
            }
            //string strFilePash = SearchExcel();
            //string extFile = Path.GetExtension(strFilePash).ToUpper();//获得Fielupload的值
            //if (extFile == ".XLS" || extFile == ".XLSX")//如果文件是.XLS格式的
            //{
            //    string str = System.IO.Path.GetFileName(strFilePash);
            //    DataSet ds = ExploreFile.ReadDataFromExcel(strFilePash);
            //    if (ds.Tables[0].Rows.Count != 0)
            //    {
            //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)//循环读取临时表的行
            //        {
            //            string strCode = ds.Tables[0].Rows[i][1].ToString();
            //            string strTH = ds.Tables[0].Rows[i][4].ToString();
            //            string strQuantity = ds.Tables[0].Rows[i][8].ToString();
            //            string strPrice = ds.Tables[0].Rows[i][9].ToString();
            //            string strMoney = ds.Tables[0].Rows[i][10].ToString();
            //            foreach (DataGridViewRow var in this.dgDetail.Rows)
            //            {
            //                if (var.IsNewRow) break;
            //                if (var.Cells[colMaterialCode.Name].Value.ToString().Trim().Equals(strCode) && var.Cells[colDiagramNum.Name].Value.ToString().Trim().Equals(strTH))
            //                {
            //                    var.Cells[colSupplyPrice.Name].Value = strPrice;
            //                    var.Cells[colSumMoney.Name].Value = strMoney;
            //                    var.Cells[colQuantity.Name].Value = strQuantity;
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}            
        }

        protected string SearchExcel()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "所有文件(*.*)|*.*";
            openFile.ShowDialog();
            string fileName = openFile.FileName;
            if (fileName.Equals("") || !System.IO.File.Exists(fileName))
            {
            }
            else
            {
                FileInfo finfo = new FileInfo(fileName);
                if (finfo.Length > int.MaxValue)
                {
                    MessageBox.Show("文件太大！系统加载失败！", "系统提示", MessageBoxButtons.OK);
                }
            }
            return fileName;
        }
        void cboProfessionCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pc = cboProfessionCat.SelectedItem as string;
            if (pc != null)
            {
                foreach (DataGridViewRow dr in dgDetail.Rows)
                {
                    if (dr.IsNewRow) break;
                    dr.Cells[colzyfl.Name].Value = pc;
                }
            }
        }

        void btnSetPP_Click(object sender,EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) break;
                if (dgDetail[colBrand.Name, 0].Value != "")
                {
                    var.Cells[colBrand.Name].Value = dgDetail[colBrand.Name, 0].Value;
                }
            }
        }

        void txtSupply_Leave(object sender, EventArgs e)
        {
            if (txtSupply.Result != null && txtSupply.Result.Count > 0)
            {
                SupplierRelationInfo supRel = txtSupply.Result[0] as SupplierRelationInfo;
                if (supRel != null)
                {
                    this.txtContactPerson.Text = supRel.LinkMan;
                    this.txtContactPhone.Text = supRel.Telphone;
                }
            }
        }

        void txtMaterialCategory_Leave(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                if (dr.IsNewRow) break;
                dr.Cells[colwzfl.Name].Value = txtMaterialCategory.Text;
                dr.Cells[colwzfl.Name].Tag = txtMaterialCategory.Result[0] as MaterialCategory;
            }
        }

        /// <summary>
        /// 查找一级分类
        /// </summary>
        /// <param name="mc"></param>
        /// <returns></returns>
        private MaterialCategory FindFirstCategory(MaterialCategory mc)
        {
            //if (mc.Level == 2) return mc;
            return FindFirstCategory((MaterialCategory)mc.ParentNode);
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            InitControls();
        }
        /// <summary>
        /// 根据土建、安装 界面不同显示
        /// </summary>
        private void InitControls()
        {
            if (supplyType == EnumSupplyType.土建)
            {
                colzyfl.Visible = false;
                colPrice.Visible = false;
                btnSet.Visible = false;
                colRJMoney.Visible = false;
                txtRJMoney.Visible = false;
                customLabel14.Visible = false;
                customLabel8.Text = "需求总计划:";
                lblCat.Text = "物资分类";
                txtMaterialCategory.Visible = true;
                cboProfessionCat.Visible = false;
                colzyfl.Visible = false;
                colTelchPara.Visible = false;
                this.btnExcel.Visible = false;
            }
            if (SupplyType == EnumSupplyType.安装)
            {
                customLabel8.Text = "专业需求计划:";
                lblCat.Visible = false;
                cboProfessionCat.Visible = false;
                txtMaterialCategory.Visible = false;
                this.lblPump.Visible = false;
                this.txtPump.Visible = false;
                colzyfl.Visible =false ;
                this.btnExcel.Visible = true;
            }
        }

        void btnDemandSearch_Click(object sender, EventArgs e)
        {
            if (customLabel8.Text.Equals("需求总计划:"))
            {
                VDemandMasterPlanSelector vmros = new VDemandMasterPlanSelector(this.SupplyType);
                vmros.ShowDialog();
                IList list = vmros.Result;
                if (list == null || list.Count == 0) return;
                DemandMasterPlanMaster supplyMaster = list[0] as DemandMasterPlanMaster;
                txtDemandPlanCode.Tag = supplyMaster.Id;
                txtDemandPlanCode.Text = supplyMaster.Code;

                if (this.txtMaterialCategory.Result != null)
                {
                    txtMaterialCategory.Result.Clear();
                    
                }
                IList result = new ArrayList();
                result.Add(supplyMaster.MaterialCategory);
                this.txtMaterialCategory.Text = supplyMaster.MaterialCategoryName;
                this.txtMaterialCategory.Tag = supplyMaster.MaterialCategory;
                txtMaterialCategory.Result = result;

                ////处理旧明细
                CurBillMaster.Details.Clear();
                //foreach (DataGridViewRow dr in dgDetail.Rows)
                //{
                //    DemandMasterPlanDetail dtl = dr.Tag as DemandMasterPlanDetail;
                //    if (dtl != null)
                //    {
                //        if (CurBillMaster != null)
                //        {
                //            CurBillMaster.Details.Remove(dtl);
                //            if (dtl.Id != null)
                //            {
                //                movedDtlList.Add(dtl);
                //            }
                //        }
                //    }
                //}

                ////显示引用的明细
                this.dgDetail.Rows.Clear();
                decimal summoney = 0;
                
                foreach (DemandMasterPlanDetail var in supplyMaster.Details)
                {
                    if (var.IsSelect == false) continue;
                    int i = this.dgDetail.Rows.Add();
                    this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                    this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                    this.dgDetail[colMaterialName.Name, i].Tag = false ; 
                    this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;
                    this.dgDetail[colStuff.Name, i].Value = var.MaterialStuff;
                    this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
                    this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnitName;
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    this.dgDetail[colForwardDtlId.Name, i].Value = var.Id;
                    this.dgDetail[colQuantity.Name, i].Value = var.SupplyLeftQuantity;
                    this.dgDetail[this.colDiagramNum.Name, i].Value = var.DiagramNumber;
                    this.dgDetail[colPrice.Name, i].Value = var.Price;
                    this.dgDetail[colwzfl.Name, i].Value = supplyMaster.MaterialCategoryName;
                    this.dgDetail[colwzfl.Name, i].Tag = supplyMaster.MaterialCategory;
                    decimal money = ClientUtil.ToDecimal(this.dgDetail[colQuantity.Name, i].Value) * ClientUtil.ToDecimal(this.dgDetail[colPrice.Name, i].Value);
                    this.dgDetail[colRJMoney.Name, i].Value = ClientUtil.ToString(money);
                    this.dgDetail[colzyfl.Name, i].Value = var.SpecialType;
                    this.dgDetail[colTelchPara.Name, i].Value = var.TechnologyParameter;
                    
                    summoney += money;
                    //this.dgDetail[colQuantityTemp.Name, i].Value = var.Quantity;
                }
                this.txtRJMoney.Text = summoney.ToString("#,###.####");
            }
            if (customLabel8.Text.Equals("专业需求计划:"))
            {
                VSupplyPlanSelector vmros = new VSupplyPlanSelector();
                vmros.ShowDialog();
                IList list = vmros.Result;
                if (list == null || list.Count == 0) return;
                SupplyPlanMaster supplyMaster = list[0] as SupplyPlanMaster;
                txtDemandPlanCode.Tag = supplyMaster.Id;
                txtDemandPlanCode.Text = supplyMaster.Code;
               
                ////处理旧明细
                //foreach (DataGridViewRow dr in dgDetail.Rows)
                //{
                //    SupplyPlanDetail dtl = dr.Tag as SupplyPlanDetail;
                //    if (dtl != null)
                //    {
                //        if (CurBillMaster != null)
                //        {
                //            CurBillMaster.Details.Remove(dtl);
                //            if (dtl.Id != null)
                //            {
                //                movedDtlList.Add(dtl);
                //            }
                //        }
                //    }
                //}

                ////显示引用的明细
                //this.dgDetail.Rows.Clear();
                decimal summoney = 0;
                if (string.IsNullOrEmpty(this.txtRJMoney.Text))
                {
                    summoney = 0;
                }
                else
                {
                    summoney = ClientUtil.ToDecimal(this.txtRJMoney.Text);
                }
                string sProjectID = StaticMethod.GetProjectInfo().Id;
                string sAccountSysCode = string.Empty;
                string sDiagramNumber = string.Empty;
                string sMaterialID = string.Empty;
                if (!string.IsNullOrEmpty(ConstObject.TheLogin.TheAccountOrgInfo.Id))
                {
                    sAccountSysCode = ConstObject.TheLogin.TheAccountOrgInfo.SysCode;
                }
                foreach (SupplyPlanDetail var in supplyMaster.Details)
                {
                    if (var.IsSelect == false) continue;
                    int i = this.dgDetail.Rows.Add();
                    this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                    this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                    this.dgDetail[colMaterialName.Name, i].Tag = false ; 
                    this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;
                    this.dgDetail[colStuff.Name, i].Value = var.MaterialStuff;
                    this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
                    this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnitName;
                    this.dgDetail[colDescript.Name, i].Value = var.Descript + "[" + supplyMaster.Code + "]";
                    this.dgDetail[colForwardDtlId.Name, i].Value = var.Id;
                    this.dgDetail[colzyfl.Name, i].Value = var.SpecialType;
                    this.dgDetail[colPrice.Name, i].Value = var.Price;
                    this.dgDetail[colDiagramNum .Name , i].Value= var.DiagramNumber;
                    //this.dgDetail[colzyfl.Name, i].Value = cboProfessionCat.SelectedItem as string;
                    //this.dgDetail[colQuantity.Name, i].Value = var.Quantity;
                    this.dgDetail[colQuantity.Name, i].Value =ClientUtil.ToDecimal (  var.TempData2) ;
                    decimal money = ClientUtil.ToDecimal(this.dgDetail[colQuantity.Name, i].Value) * ClientUtil.ToDecimal(this.dgDetail[colPrice.Name, i].Value);
                    summoney += money;
                    this.dgDetail[colRJMoney.Name, i].Value = ClientUtil.ToString(money);
                    this.dgDetail[colTelchPara.Name, i].Value = var.TechnologyParameter;
                    sDiagramNumber = (var.DiagramNumber == null ? string.Empty : var.DiagramNumber);
                    sMaterialID = (var.MaterialResource == null ? "" : var.MaterialResource.Id);
                    if (this.SupplyType == EnumSupplyType.安装)
                    {
                        //this.dgDetail[colPrice.Name, i].Value = model.SupplyOrderSrv.GetConfirmPrice(var.ForwardDetailId);
                        //this.dgDetail[colRJMoney.Name, i].Value = ClientUtil.ToDecimal(this.dgDetail[colPrice.Name, i].Value) * ClientUtil.ToDecimal(this.dgDetail[colQuantity.Name, i].Value);
                        this.dgDetail[colRJMoney.Name, i].Value = ClientUtil.ToString(var.Price);
                    }
                    else if (!string.IsNullOrEmpty(ConstObject.TheLogin.TheAccountOrgInfo.Id))
                    {
                        this.dgDetail[colPrice.Name, i].Value = model.SupplyOrderSrv.GetConfirmPrice(sAccountSysCode, sProjectID, sDiagramNumber, sMaterialID);
                        this.dgDetail[colRJMoney.Name, i].Value = ClientUtil.ToDecimal(this.dgDetail[colPrice.Name, i].Value) * ClientUtil.ToDecimal(this.dgDetail[colQuantity.Name, i].Value);
                    }
                   
                    //this.dgDetail[colQuantity.Name, i].Value = var.DemandLeftQuantity;
                    //this.dgDetail[colQuantityTemp.Name, i].Value = var.Quantity;
                }
                this.txtRJMoney.Text = summoney.ToString("##,###.####");
            }
        }

        void btnSet_Click(object sender, EventArgs e)
        {
            //获得第一行专业分类的信息，将其他行的专业分类信息都改成和第一行相同的信息
            string strSpecialType = null;
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) break;
                strSpecialType = this.dgDetail.Rows[0].Cells["colzyfl"].Value.ToString();
                var.Cells[colzyfl.Name].Value = strSpecialType;

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
                    curBillMaster.Details.Remove(dr.Tag as SupplyOrderDetail);
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
                    curBillMaster = model.SupplyOrderSrv.GetSupplyOrderById(Id);
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
            object[] os = new object[] { txtCode, txtCreatePerson, txtHandlePerson, txtSumQuantity, txtProject, txtSumMoney, txtDemandPlanCode, txtRJMoney };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colMaterialName.Name, colMaterialSpec.Name, colUnit.Name, colSumMoney.Name, colRJMoney.Name, colDiagramNum.Name, colwzfl.Name };
            dgDetail.SetColumnsReadOnly(lockCols);
        }

        //清空数据
        private void ClearView()
        {
            if (this.txtMaterialCategory.Visible == true)
            {
                this.txtMaterialCategory.Text = "";
                this.txtMaterialCategory.Result = null;
            }
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

                this.curBillMaster = new SupplyOrderMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.SignDate = ConstObject.LoginDate;
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                curBillMaster.HandOrgLevel = ConstObject.TheOperationOrg.Level;
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.DocState = DocumentState.Edit;

                curBillMaster.Special = Enum.GetName(typeof(EnumSupplyType), SupplyType);

                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //责任人
                txtHandlePerson.Tag = ConstObject.LoginPersonInfo;
                txtHandlePerson.Text = ConstObject.LoginPersonInfo.Name;
                //归属项目
                //btnStates(0);
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
                curBillMaster = model.SupplyOrderSrv.GetSupplyOrderById(curBillMaster.Id);
                ModelToView();
                FillDoc();
                //btnStates(1);
                return true;
            }
            string message = "此单状态为非编辑状态，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(curBillMaster.DocState));
            MessageBox.Show(message);

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
                buttonStr = "提交";
               
                if (!ViewToModel()) return false;
                if (!CheckConfrimPrice()) return false;
                bool IsNew=(string.IsNullOrEmpty (curBillMaster.Id)?true :false );
                DataTable oTable = modelStockIn.StockInSrv.GetFiscaDate(curBillMaster.CreateDate);
                if (oTable != null && oTable.Rows.Count > 0)
                {
                    curBillMaster.CreateYear = int.Parse(oTable.Rows[0]["year"].ToString());
                    curBillMaster.CreateMonth = int.Parse(oTable.Rows[0]["month"].ToString());
                }
                curBillMaster.DocState = DocumentState.InExecute;
                curBillMaster = model.SupplyOrderSrv.SaveSupplyOrder(curBillMaster);
                txtCode.Text = curBillMaster.Code;
                WriteLog(IsNew);
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
                curBillMaster.DocState = DocumentState.Edit;
            }
            return false;
        }
        public bool CheckConfrimPrice()
        {
            bool bFlag = true;
            decimal dConfrimPrice = 0;
            if (this.SupplyType == EnumSupplyType.安装)
            {
                foreach (DataGridViewRow dr in dgDetail.Rows)
                {
                    //最后一行不进行校验
                    if (dr.IsNewRow) break;
                    else
                    {
                        dConfrimPrice = ClientUtil.ToDecimal(dr.Cells[this.colPrice.Name].Value);
                        if (dConfrimPrice == 0)
                        {
                            MessageBox.Show(string.Format("[{0}]物资认价为0，请输入正确单价", ClientUtil.ToString(dr.Cells[this.colMaterialName.Name].Value)));
                            dr.Cells[this.colPrice.Name].Selected = true;
                            bFlag = false;
                            break;
                        }
                    }
                }
            }
            return bFlag;

        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public override bool SaveView()
        {
            try
            {
                buttonStr = "保存";
                if (!ViewToModel()) return false;
                bool flag = false;
                if (string.IsNullOrEmpty(curBillMaster.Id))
                {
                    flag = true;
                }
                DataTable oTable = modelStockIn.StockInSrv.GetFiscaDate(curBillMaster.CreateDate);
                if (oTable != null && oTable.Rows.Count > 0)
                {
                    curBillMaster.CreateYear = int.Parse(oTable.Rows[0]["year"].ToString());
                    curBillMaster.CreateMonth = int.Parse(oTable.Rows[0]["month"].ToString());
                }
                curBillMaster = model.SupplyOrderSrv.AddSupplyOrder(curBillMaster, movedDtlList);
                txtCode.Text = curBillMaster.Code;
                //更新Caption

                WriteLog(flag);
                this.ViewCaption = ViewName + "-" + txtCode.Text;
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

        private void WriteLog(bool flag)
        {
            LogData log = new LogData();
            log.BillId = curBillMaster.Id;
            log.BillType = "采购合同单";
            log.Code = curBillMaster.Code;
            log.Descript = "";
            log.OperPerson = ConstObject.LoginPersonInfo.Name;
            log.ProjectName = curBillMaster.ProjectName;
            if (flag)
            {
                if (curBillMaster.DocState == DocumentState.InExecute)
                {
                    if (supplyType == EnumSupplyType.安装)
                    {
                        log.OperType = "新增提交 - 安装";
                    }
                    if (supplyType == EnumSupplyType.土建)
                    {
                        log.OperType = "新增提交 - 土建";
                    }
                }
                else
                {
                    if (supplyType == EnumSupplyType.安装)
                    {
                        log.OperType = "新增 - 安装";
                    }
                    if (supplyType == EnumSupplyType.土建)
                    {
                        log.OperType = "新增 - 土建";
                    }
                }
            }
            else
            {
                if (curBillMaster.DocState == DocumentState.InExecute)
                {
                    if (supplyType == EnumSupplyType.安装)
                    {
                        log.OperType = "修改提交 - 安装";
                    }
                    if (supplyType == EnumSupplyType.土建)
                    {
                        log.OperType = "修改提交 - 土建";
                    }
                }
                else
                {
                    if (supplyType == EnumSupplyType.安装)
                    {
                        log.OperType = "修改 - 安装";
                    }
                    if (supplyType == EnumSupplyType.土建)
                    {
                        log.OperType = "修改 - 土建";
                    }
                }
            }
            StaticMethod.InsertLogData(log);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                curBillMaster = model.SupplyOrderSrv.GetSupplyOrderById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    if (!msrv.docSrv.DeleteDocument(curBillMaster.Id)) return false;
                    if (!model.SupplyOrderSrv.DeleteSupplyOrder(curBillMaster)) return false;
                    
                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "采购合同单";
                    log.Code = curBillMaster.Code;
                    if (supplyType == EnumSupplyType.安装)
                    {
                        log.OperType = "删除 - 安装";
                    }
                    if (supplyType == EnumSupplyType.土建)
                    {
                        log.OperType = "删除 - 土建";
                    }
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
                        curBillMaster = model.SupplyOrderSrv.GetSupplyOrderById(curBillMaster.Id);
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
                curBillMaster = model.SupplyOrderSrv.GetSupplyOrderById(curBillMaster.Id);
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
            if (supplyType == EnumSupplyType.安装 && buttonStr == "保存")
            {
               
            }
            else
            {
                if (txtSupply.Text == "" || txtSupply.Result.Count == 0)
                {
                    MessageBox.Show("供应商不能为空。");
                    return false;
                }
                if (supplyType == EnumSupplyType.土建)
                {
                    if (txtMaterialCategory.Text.Equals("") || txtMaterialCategory.Text.Equals(null))
                    {
                        MessageBox.Show("物资分类为必选项！");
                        return false;
                    }
                }
                //没有必要判断了
                //if (SupplyType == EnumSupplyType.安装)
                //{
                //    if (this.cboProfessionCat.SelectedItem != null)
                //    { }else
                //    {
                //        MessageBox.Show("专业分类为必选项！");
                //        return false;
                //    }
                //}
                if (ClientUtil.ToString(this.txtContratcMaterial.Text) == "")
                {
                    MessageBox.Show("合同物资不能为空！");
                    return false;
                }

                if (ClientUtil.ToString(this.txtContactPerson.Text) == "")
                {
                    MessageBox.Show("联系人不能为空！");
                    return false;
                }

                if (ClientUtil.ToString(this.txtContactPhone.Text) == "")
                {
                    MessageBox.Show("联系电话不能为空！");
                    return false;
                }

                if (ClientUtil.ToString(this.txtSupplyContract.Text) == "")
                {
                    MessageBox.Show("采购合同号不能为空！");
                    return false;
                }

                if (ClientUtil.ToString(cmobalanceStyle.Text) == "")
                {
                    MessageBox.Show("结算完成情况不能为空！");
                    return false;
                }
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
            string sMsg = CheckMaterial();
            if (!string.IsNullOrEmpty(sMsg))
            {
                MessageBox.Show(sMsg);
                return false;
            }
            if (supplyType == EnumSupplyType.安装 && buttonStr == "保存")
            {
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
                }
            }
            else
            {

                //取得该供应商，该项目的合同物资信息
                Hashtable ht_material = model.SupplyOrderSrv.GetOrderMaterialInfo(projectInfo.Id, (txtSupply.Result[0] as SupplierRelationInfo).Id);
                IList currList = new ArrayList();
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
                    else
                    {
                        string materialId = (dr.Cells[colMaterialCode.Name].Tag as Application.Resource.MaterialResource.Domain.Material).Id;
                        if (currList.Contains(materialId))
                        {
                            MessageBox.Show("物料编码[" + dr.Cells[colMaterialCode.Name].Value + "]规格[" + dr.Cells[this.colMaterialSpec.Name].Value + "]重复！");
                            return false;
                        }
                        else
                        {
                            currList.Add(materialId);
                        }

                        if (supplyType == EnumSupplyType.土建)
                        {
                            if (curBillMaster.Id == null)
                            {
                                if (ht_material.Contains(materialId))
                                {
                                    MessageBox.Show("物料编码[" + dr.Cells[colMaterialCode.Name].Value + "]规格[" + dr.Cells[this.colMaterialSpec.Name].Value + "]在该项目中已经签订合同！");
                                    return false;
                                }
                            }
                            else
                            {
                                if (ht_material.Contains(materialId))
                                {
                                    IList list_order = (ArrayList)ht_material[materialId];
                                    if (list_order.Count > 1)
                                    {
                                        MessageBox.Show("物料编码[" + dr.Cells[colMaterialCode.Name].Value + "]规格[" + dr.Cells[this.colMaterialSpec.Name].Value + "]在该项目中已经签订合同！");
                                        return false;
                                    }
                                    else
                                    {
                                        if (!list_order.Contains(curBillMaster.Id))
                                        {
                                            MessageBox.Show("物料编码[" + dr.Cells[colMaterialCode.Name].Value + "]规格[" + dr.Cells[this.colMaterialSpec.Name].Value + "]在该项目中已经签订合同！");
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (dr.Cells[colUnit.Name].Value == null)
                    {
                        MessageBox.Show("计量单位不允许为空！");
                        dgDetail.CurrentCell = dr.Cells[colUnit.Name];
                        return false;
                    }

                    if (dr.Cells[colQuantity.Name].Value == null || dr.Cells[colQuantity.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colQuantity.Name].Value) < 0)
                    {
                        MessageBox.Show("数量不允许为空或小于0！");
                        dgDetail.CurrentCell = dr.Cells[colQuantity.Name];
                        return false;
                    }

                    if (dr.Cells[this.colBrand.Name].Value == null || dr.Cells[colBrand.Name].Value.ToString() == "")
                    {
                        MessageBox.Show("品牌不能为空！");
                        dgDetail.CurrentCell = dr.Cells[colBrand.Name];
                        return false;
                    }

                    if (SupplyType == EnumSupplyType.安装)
                    {
                        if (dr.Cells[colPrice.Name].Value == null || dr.Cells[colPrice.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colPrice.Name].Value) < 0)
                        {
                            MessageBox.Show("认价单价不允许为空！");
                            dgDetail.CurrentCell = dr.Cells[colPrice.Name];
                            return false;
                        }
                    }
                    //foreach (string matCat in list_price)
                    //{
                    //    if (ClientUtil.ToString(dr.Cells[colMaterialCode.Name].Value).IndexOf(matCat) != -1 && ClientUtil.ToDecimal(dr.Cells[colSupplyPrice.Name].Value) <= 0)
                    //    {
                    //        MessageBox.Show("编码为[" + dr.Cells[colMaterialCode.Name].Value + "]名称[" + dr.Cells[this.colMaterialName.Name].Value + "]规格[" 
                    //            + dr.Cells[this.colMaterialSpec.Name].Value + "]的物资采购单价不允许为空或小于等于0！");
                    //        dgDetail.CurrentCell = dr.Cells[colSupplyPrice.Name];
                    //        return false;
                    //    }
                    //}

                    if (dr.Cells[colSupplyPrice.Name].Value == null || dr.Cells[colSupplyPrice.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colSupplyPrice.Name].Value) < 0)
                    {
                        MessageBox.Show("采购单价不允许为空或小于0！");
                        dgDetail.CurrentCell = dr.Cells[colSupplyPrice.Name];
                        return false;
                    }

                    if (btnDemandSearch.Enabled == false)
                    {
                        object forwardDtlId = dr.Cells[colForwardDtlId.Name].Value;
                        SupplyPlanDetail forwardDetail = model.SupplyOrderSrv.GetSupplyDetailById(forwardDtlId.ToString());
                        //SupplyPlanDetail forwardDetail = Supplymodle.SupplyPlanSrv.GetSupplyDetailById(forwardDtlId.ToString());
                        if (forwardDetail == null)
                        {
                            MessageBox.Show("未找到前续单据明细,请重新引用。");
                            //dgDetail.CurrentCell = dr.Cells[colQuantityTemp.Name];
                            return false;
                        }
                        else
                        {
                            decimal canUseQty = forwardDetail.Quantity - forwardDetail.RefQuantity;//可利用数量
                            decimal currentQty = decimal.Parse(dr.Cells[colQuantity.Name].Value.ToString());
                            object qtyTempObj = dr.Cells[colQuantityTemp.Name].Value;//临时数量
                            decimal qtyTemp = 0;
                            if (qtyTempObj != null && !qtyTempObj.ToString().Equals(""))
                            {
                                qtyTemp = decimal.Parse(qtyTempObj.ToString());
                            }

                            if (currentQty - qtyTemp - canUseQty > 0)
                            {
                                MessageBox.Show("输入数量【" + currentQty + "】大于可引用数量【" + (canUseQty + qtyTemp) + "】。");
                                dgDetail.CurrentCell = dr.Cells[colQuantity.Name];
                                return false;
                            }
                        }
                    }

                }

                //付款方式明细校验
                foreach (DataGridViewRow dr in dgExtDetail.Rows)
                {
                    //最后一行不进行校验
                    if (dr.IsNewRow) break;
                    if (dr.Cells[colProjectStatus.Name].Value == null || dr.Cells[colProjectStatus.Name].Value.ToString() == "")
                    {
                        MessageBox.Show("工程状态不能为空！");
                        return false;
                    }
                    if (dr.Cells[colProportion.Name].Value == null || dr.Cells[colProportion.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colProportion.Name].Value) <= 0 || ClientUtil.TransToDecimal(dr.Cells[colProportion.Name].Value) >= 100)
                    {
                        MessageBox.Show("付款比例不允许为空或小于等于0大于等于100！");
                        //dgDetail.CurrentCell = dr.Cells[colProportion.Name];
                        return false;
                    }
                }
                dgExtDetail.Update();
                dgDetail.Update();
            }
            return true;
        }

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                this.txtCode.Focus();
                curBillMaster.CreateDate = dtpSignDate.Value.Date;
                curBillMaster.SignDate = dtpSignDate.Value.Date;
                //物资分类
                if (txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
                {
                    curBillMaster.MaterialCategory = txtMaterialCategory.Result[0] as MaterialCategory;
                    curBillMaster.MaterialCategoryName = txtMaterialCategory.Text;
                    curBillMaster.MaterialCategoryCode = curBillMaster.MaterialCategory.Code;
                }
                curBillMaster.SpecialType = cboProfessionCat.SelectedItem == null ? null : cboProfessionCat.SelectedItem.ToString();
                curBillMaster.ContractMoney = ClientUtil.ToDecimal(this.txtSumMoney.Text);
                curBillMaster.ContractMatDes = ClientUtil.ToString(this.txtContratcMaterial.Text);//合同物资
                curBillMaster.OldContractNum = ClientUtil.ToString(this.txtSupplyContract.Text);//采购合同号
                curBillMaster.ContactPerson = ClientUtil.ToString(this.txtContactPerson.Text);//联系人
                curBillMaster.Telephone = ClientUtil.ToString(this.txtContactPhone.Text);//联系电话
                //curBillMaster.AttachmentDocPath = ClientUtil.ToString(this.txtAttachment.Text);//附件上传
                curBillMaster.Descript = ClientUtil.ToString(this.txtContractDescript.Text);//合同描述
                if (txtSupply.Result.Count > 0)
                {
                    curBillMaster.TheSupplierRelationInfo = txtSupply.Result[0] as SupplierRelationInfo;
                }
                curBillMaster.RJSumMoney = ClientUtil.ToDecimal(this.txtRJMoney.Text);//认价总金额
                curBillMaster.SupplierName = txtSupply.Text;//供应商
                curBillMaster.Descript = this.txtContractDescript.Text;
                curBillMaster.ForwardBillCode = this.txtDemandPlanCode.Text;
                curBillMaster.PumpMoney = ClientUtil.ToDecimal(this.txtPump.Text);//泵送费

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

                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    SupplyOrderDetail curBillDtl = new SupplyOrderDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as SupplyOrderDetail;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.MaterialResource = var.Cells[colMaterialCode.Name].Tag as Application.Resource.MaterialResource.Domain.Material;
                    curBillDtl.MaterialName = ClientUtil.ToString(var.Cells[colMaterialName.Name].Value);//物资名称
                    curBillDtl.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);//规格型号
                    curBillDtl.MaterialStuff = ClientUtil.ToString(var.Cells[colStuff.Name].Value);//材质
                    curBillDtl.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);//物资编码
                    curBillDtl.MatStandardUnit = var.Cells[colUnit.Name].Tag as StandardUnit;//计量单位
                    curBillDtl.MatStandardUnitName = ClientUtil.ToString(var.Cells[colUnit.Name].Value);//计量单位
                    curBillDtl.Quantity = ClientUtil.TransToDecimal(var.Cells[colQuantity.Name].Value);//数量
                    curBillDtl.QuantityTemp = ClientUtil.ToDecimal(var.Cells[colQuantityTemp.Name].Value);//临时数量
                    curBillDtl.Money = ClientUtil.TransToDecimal(var.Cells[colSumMoney.Name].Value);//金额
                    curBillDtl.ConfirmPrice = ClientUtil.TransToDecimal(var.Cells[colPrice.Name].Value);//认价单价
                    curBillDtl.RJMoney = ClientUtil.ToDecimal(var.Cells[colRJMoney.Name].Value);//认价金额
                    curBillDtl.SupplyPrice = ClientUtil.TransToDecimal(var.Cells[colSupplyPrice.Name].Value);//采购单价
                    curBillDtl.ModifyPrice = ClientUtil.TransToDecimal(var.Cells[colSupplyPrice.Name].Value);//调后价格先暂定为采购价
                    curBillDtl.SpecialType = ClientUtil.ToString(var.Cells[colzyfl.Name].Value);//专业分类
                    curBillDtl.MaterialCategory = var.Cells[colwzfl.Name].Tag as MaterialCategory;
                    curBillDtl.MaterialCategoryName = ClientUtil.ToString(var.Cells[colwzfl.Name].Value);//材料类别
                    curBillDtl.Grade = ClientUtil.ToString(var.Cells[colGrade.Name].Value);//档次
                    curBillDtl.Brand = ClientUtil.ToString(var.Cells[colBrand.Name].Value);//品牌
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);//备注
                    curBillDtl.DiagramNumber = ClientUtil.ToString(var.Cells[this.colDiagramNum.Name].Value);//图号
                    curBillMaster.ForwardBillId = ClientUtil.ToString(var.Cells[colForwardDtlId.Name].Value);//前驱明细
                    curBillDtl.ForwardDetailId = ClientUtil.ToString(var.Cells[colForwardDtlId.Name].Value);
                    curBillMaster.AddDetail(curBillDtl);
                    var.Tag = curBillDtl;
                    curBillDtl.TechnologyParameter = ClientUtil.ToString(var.Cells[colTelchPara.Name].Value);
                }
                //保存付款方式
                foreach (DataGridViewRow var in this.dgExtDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    SupplyOrderPayment curBillPaymant = new SupplyOrderPayment();
                    if (var.Tag != null)
                    {
                        curBillPaymant = var.Tag as SupplyOrderPayment;
                        if (curBillPaymant.Id == null)
                        {
                            curBillMaster.PaymentDetails.Remove(curBillPaymant);
                        }
                    }
                    curBillPaymant.PaymentProportion = ClientUtil.ToDecimal(var.Cells[colProportion.Name].Value);//付款比例
                    curBillPaymant.ProjectState = ClientUtil.ToString(var.Cells[colProjectStatus.Name].Value);//工程状态
                    curBillPaymant.Descript = ClientUtil.ToString(var.Cells[colRemark.Name].Value);//备注
                    curBillMaster.AddPaymentDetail(curBillPaymant);
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
                    this.dgDetail.CurrentRow.Cells[colStuff.Name].Value = selectedMaterial.Stuff;

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
        public bool CheckMaterialCategory(string sMatCateCode)
        {
            bool bFlag = true ;
            if (!string.IsNullOrEmpty(sMatCateCode))
            {
                foreach (string sCode in arrMustRefDemandPlan)
                {
                    if (sMatCateCode .StartsWith (sCode ))
                    {
                        bFlag = false;
                        break;
                    }
                }
                if (!bFlag)
                {
                    MessageBox.Show("请关联需求总计划单获取物资明细！");
                }
            }
            return bFlag;
        }
        public  string   CheckMaterial( )
        {
            bool bFlag = true;
            string sMsg = string.Empty;
            IList lstCodes = new ArrayList();
           
             string sTempCode =string.Empty ;
            string sMaterialName=string.Empty ;
            bool IsSelectMaterial = false;
            foreach (DataGridViewRow  oRow in dgDetail.Rows )
            {
                IsSelectMaterial=ClientUtil .ToBool ( oRow.Cells [colMaterialName.Name].Tag);
                if (IsSelectMaterial)
                {
                    sTempCode = ClientUtil.ToString(oRow.Cells[colMaterialCode.Name].Value);
                    foreach (string sCode in arrMustRefDemandPlan)
                    {
                        if (sTempCode.StartsWith(sCode))
                        {
                            sMaterialName = ClientUtil.ToString(oRow.Cells[colMaterialName.Name].Value);
                            sMsg += string.Format("第{0}行 物资名称【{1}】 编号【{2}】\n", oRow.Index + 1, sMaterialName, sTempCode);
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(sMsg))
            {
                sMsg += "这些物资必须引用总需求计划";
            }
            return sMsg;
        }
        /// <summary>
        /// 物料编码列，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (SupplyType == EnumSupplyType.安装)
            {
                return;
            }
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colMaterialCode.Name))
            {
                if (this.txtMaterialCategory.Visible == true && (this.txtMaterialCategory.Result == null || this.txtMaterialCategory.Result.Count == 0))
                {
                    MessageBox.Show("请先选择物资分类！");
                    return;
                }
                MaterialCategory cat = this.txtMaterialCategory.Result[0] as MaterialCategory;
                if (cat.Level != 3)
                {
                    MessageBox.Show("请选择一级物资分类！");
                    return;
                }

                CommonMaterial materialSelector = new CommonMaterial();
                if (this.txtMaterialCategory.Visible == true)
                {
                    string catCode = (txtMaterialCategory.Result[0] as MaterialCategory).Code;
                    //if (catCode.StartsWith("I1100112") || catCode.StartsWith("I1100113") || catCode.StartsWith("I1100102") || catCode.StartsWith("I1100101")
                    //    || catCode.StartsWith("I1100111") || catCode.StartsWith("I1100110") || catCode.StartsWith("I11201"))
                    //{
                    //    MessageBox.Show("请关联需求总计划单获取物资明细！");
                    //    return;
                    //}
                    if (!CheckMaterialCategory(catCode))
                    {
                        return;
                    }
                    if (txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
                    {
                        materialSelector.materialCatCode = (txtMaterialCategory.Result[0] as MaterialCategory).Code;
                    }
                }
                DataGridViewCell cell = this.dgDetail[e.ColumnIndex, e.RowIndex];
                object tempValue = cell.EditedFormattedValue;
                materialSelector.OpenSelect();

                IList list = materialSelector.Result;
                foreach (Application.Resource.MaterialResource.Domain.Material theMaterial in list)
                {
                    int i = dgDetail.Rows.Add();
                    this.dgDetail[colMaterialCode.Name, i].Tag = theMaterial;
                    this.dgDetail[colMaterialCode.Name, i].Value = theMaterial.Code;
                    this.dgDetail[colMaterialName.Name, i].Value = theMaterial.Name;
                    this.dgDetail[colMaterialName.Name, i].Tag = true; 
                    this.dgDetail[colMaterialSpec.Name, i].Value = theMaterial.Specification;
                    this.dgDetail[colStuff.Name, i].Value = theMaterial.Stuff;
                    this.dgDetail[colUnit.Name, i].Tag = theMaterial.BasicUnit;
                    if (theMaterial.BasicUnit != null)
                        this.dgDetail[colUnit.Name, i].Value = theMaterial.BasicUnit.Name;
                    if (txtMaterialCategory.Result != null)
                    {
                        dgDetail[colwzfl.Name, i].Value = txtMaterialCategory.Value;
                        dgDetail[colzyfl.Name, i].Tag = txtMaterialCategory.Result[0] as MaterialCategory;
                    }
                    i++;
                }
                string sMsg = CheckMaterial();
                if (!string.IsNullOrEmpty(sMsg))
                {
                    MessageBox.Show(sMsg);
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
                dtpSignDate.Value = curBillMaster.SignDate;//业务时间
                txtHandlePerson.Tag = curBillMaster.HandlePerson;
                txtHandlePerson.Text = curBillMaster.HandlePersonName;
                txtContractDescript.Text = curBillMaster.Descript;
                this.txtCreatePerson.Tag = curBillMaster.CreatePerson;
                txtCreatePerson.Text = curBillMaster.CreatePersonName;
                this.txtCreatePerson.Text = curBillMaster.CreatePersonName;

                cmobalanceStyle.Text = curBillMaster.BalanceStyle;
                txtProcessPayRate.Text = ClientUtil.ToString(curBillMaster.ProcessPayRate * 100);
                txtCompletePayRate.Text = ClientUtil.ToString(curBillMaster.CompletePayRate * 100);
                txtWarrantyPayRate.Text = ClientUtil.ToString(curBillMaster.WarrantyPayRate * 100);

                //专业分类
                if (curBillMaster.SpecialType != null)
                {
                    cboProfessionCat.SelectedItem = curBillMaster.SpecialType;
                }

                //物资分类
                if (curBillMaster.MaterialCategory != null)
                {
                    txtMaterialCategory.Result.Clear();
                    txtMaterialCategory.Result.Add(curBillMaster.MaterialCategory);
                    txtMaterialCategory.Value = curBillMaster.MaterialCategoryName;
                }
                txtSumQuantity.Text = ClientUtil.ToString(curBillMaster.SumQuantity.ToString("#,###.####"));
                txtSumMoney.Text = ClientUtil.ToString(curBillMaster.SumMoney.ToString("#,###.####"));
                this.txtPump.Text = ClientUtil.ToString(curBillMaster.PumpMoney.ToString("#,###.##"));
                this.txtProject.Text = curBillMaster.ProjectName.ToString();
                this.txtContractDescript.Text = ClientUtil.ToString(curBillMaster.Descript);
                this.txtSupplyContract.Text = ClientUtil.ToString(curBillMaster.OldContractNum);//原始合同号
                if (curBillMaster.TheSupplierRelationInfo != null)
                {
                    txtSupply.Result.Clear();
                    txtSupply.Result.Add(curBillMaster.TheSupplierRelationInfo);
                    this.txtSupply.Value = curBillMaster.SupplierName;//供应商
                }
                this.txtRJMoney.Text = curBillMaster.RJSumMoney.ToString("##,###,#####");
                //this.txtAttachment.Text = ClientUtil.ToString(curBillMaster.AttachmentDocPath);//附件上传
                this.txtContactPerson.Text = ClientUtil.ToString(curBillMaster.ContactPerson);//联系人
                this.txtContactPhone.Text = ClientUtil.ToString(curBillMaster.Telephone);//联系电话
                this.txtContratcMaterial.Text = ClientUtil.ToString(curBillMaster.ContractMatDes);//合同物资
                this.dgDetail.Rows.Clear();
                //获得前驱单号，查询单号属于哪个前驱，相应的将信息保存
                string strForCode = ClientUtil.ToString(curBillMaster.ForwardBillCode);
                supplyMaster = Supplymodle.SupplyPlanSrv.GetSupplyPlanByCode(strForCode);
                this.txtDemandPlanCode.Text = ClientUtil.ToString(curBillMaster.ForwardBillCode);
                this.dgDetail.Rows.Clear();
                foreach (SupplyOrderDetail var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();
                    
                    this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialResource.Code;
                    this.dgDetail[colMaterialName.Name, i].Value = var.MaterialResource.Name;
                    this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialResource.Specification;
                    this.dgDetail[colStuff.Name, i].Value = var.MaterialResource.Stuff;
                    //设置该物料的计量单位
                    this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
                    this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnitName;
                    this.dgDetail[colBrand.Name, i].Value = var.Brand;
                    this.dgDetail[colGrade.Name, i].Value = var.Grade;//商标牌子
                    this.dgDetail[colPrice.Name, i].Value = var.ConfirmPrice;//认价单价
                    this.dgDetail[colSupplyPrice.Name, i].Value = var.SupplyPrice;//采购价格

                    this.dgDetail[colQuantity.Name, i].Value = var.Quantity-var.RefQuantity;//数量
                    this.dgDetail[colQuantityTemp.Name, i].Value = var.Quantity;
                    this.dgDetail[colSumMoney.Name, i].Value = var.Money;
                    this.dgDetail[colzyfl.Name, i].Value = var.SpecialType;
                    this.dgDetail[colwzfl.Name, i].Tag = var.MaterialCategory;
                    this.dgDetail[colwzfl.Name, i].Value = var.MaterialCategoryName;
                    this.dgDetail[this.colDiagramNum.Name, i].Value = var.DiagramNumber;//图号
                    this.dgDetail[colForwardDtlId.Name, i].Value = var.ForwardDetailId;
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    this.dgDetail[colRJMoney.Name, i].Value = var.RJMoney;
                    this.dgDetail.Rows[i].Tag = var;
                    this.dgDetail[colTelchPara.Name, i].Value = var.TechnologyParameter;
                }
                //显示付款方式
                dgExtDetail.Rows.Clear();
                foreach (SupplyOrderPayment var in curBillMaster.PaymentDetails)
                {
                    int i = this.dgExtDetail.Rows.Add();
                    this.dgExtDetail[colRemark.Name, i].Value = var.Descript;//备注
                    this.dgExtDetail[colProportion.Name, i].Value = var.PaymentProportion;//比例
                    this.dgExtDetail[colProjectStatus.Name, i].Value = var.ProjectState;//工程状态
                    this.dgExtDetail.Rows[i].Tag = var;
                }
                FillDoc();

                return true;
            }
            catch (Exception e)
            {
                throw e;
                //MessageBox.Show("数据映射错误：" + StaticMethod.ExceptionMessage(e));
                //return false;
            }
        }

        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            bool flag = true;
            bool flag1 = true;
            if (colName == colSupplyPrice.Name || colName == colQuantity.Name || colName ==colPrice .Name )
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("数量为数字！");
                        this.dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value = "";
                        flag = false;
                       
                    }
                }

                if (dgDetail.Rows[e.RowIndex].Cells[colSupplyPrice.Name].Value != null)
                {
                    string temp_price = dgDetail.Rows[e.RowIndex].Cells[colSupplyPrice.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_price);
                    if (validity == false)
                    {
                        MessageBox.Show("采购单价为数字！");
                        this.dgDetail.Rows[e.RowIndex].Cells[colSupplyPrice.Name].Value = "";
                        flag = false;
                    }
                }
                if (dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value != null)
                {
                    string temp_price = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_price);
                    if (validity == false)
                    {
                        MessageBox.Show("认价单价为数字！");
                        this.dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value = "";
                        flag = false;  
                    }
                }
                if (flag)
                {
                    //根据单价和数量计算金额  
                    object price = dgDetail.Rows[e.RowIndex].Cells[colSupplyPrice.Name].Value;
                    object quantity = dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value;

                    decimal sumqty = 0;
                    decimal money = 0;
                    decimal summoney = 0;
                    for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                    {
                        quantity = dgDetail.Rows[i].Cells[colQuantity.Name].Value;
                        if (quantity == null) quantity = 0;
                        //if (SupplyType == EnumSupplyType.安装)
                        //{
                        dgDetail.Rows[i].Cells[colQuantity.Name].Value = ClientUtil.ToString(dgDetail.Rows[i].Cells[colQuantity.Name].Value).Trim () == "" ? 0 : dgDetail.Rows[i].Cells[colQuantity.Name].Value;
                        dgDetail.Rows[i].Cells[colSupplyPrice.Name].Value = ClientUtil.ToString(dgDetail.Rows[i].Cells[colSupplyPrice.Name].Value).Trim () == "" ? 0 : dgDetail.Rows[i].Cells[colSupplyPrice.Name].Value;
                        dgDetail.Rows[i].Cells[colPrice.Name].Value = ClientUtil.ToString(dgDetail.Rows[i].Cells[colPrice.Name].Value).Trim() == "" ? 0 : dgDetail.Rows[i].Cells[colPrice.Name].Value;
                            money = ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colQuantity.Name].Value) * ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colSupplyPrice.Name].Value);
                            dgDetail.Rows[i].Cells[colRJMoney.Name].Value = ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colQuantity.Name].Value) * ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colPrice.Name  ].Value);
                            dgDetail.Rows[i].Cells[colSumMoney.Name].Value = ClientUtil.ToString(money);
                       
                        //}
                        sumqty = sumqty + ClientUtil.TransToDecimal(quantity);
                        summoney += money;
                    }

                    txtSumQuantity.Text = sumqty.ToString();
                    txtSumMoney.Text = summoney.ToString();
                }
                
                
            }
        }

        ///// <summary>
        ///// 打印预览
        ///// </summary>
        ///// <returns></returns>
        //public override bool Preview()
        //{
        //    if (LoadTempleteFile(@"料具租赁管理\料具租赁合同打印.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.PrintPreview(true,true,true,0,0,0,0,0);
        //    return true;
        //}

        //public override bool Print()
        //{
        //    if (LoadTempleteFile(@"料具租赁管理\料具租赁合同打印.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.Print();
        //    return true;
        //}

        //public override bool Export()
        //{
        //    if (LoadTempleteFile(@"料具租赁管理\料具租赁合同打印.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.ExportToExcel("料具租赁合同【" + curBillMaster.Code + "】", false, false, true);
        //    return true;
        //}

        //private bool LoadTempleteFile(string modelName)
        //{
        //    ExploreFile eFile = new ExploreFile();
        //    string path = eFile.Path;
        //    if (eFile.IfExistFileInServer(modelName))
        //    {
        //        eFile.CreateTempleteFileFromServer(modelName);
        //        //载入格式和数据
        //        flexGrid1.OpenFile(path + "\\" + modelName);//载入格式
        //    }
        //    else
        //    {
        //        MessageBox.Show("未找到模板格式文件【" + modelName + "】");
        //        return false;
        //    }
        //    return true;
        //}

        //private void FillFlex(MaterialRentalOrderMaster billMaster)
        //{
        //    int detailStartRowNumber = 7;//7为模板中的行号
        //    int detailCount = billMaster.Details.Count;

        //    //插入明细行
        //    flexGrid1.InsertRow(detailStartRowNumber, detailCount);

        //    //设置单元格的边框，对齐方式
        //    FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
        //    range.Alignment = FlexCell.AlignmentEnum.RightCenter;
        //    range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
        //    range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
        //    range.Mask = FlexCell.MaskEnum.Digital;

        //    //主表数据

        //    flexGrid1.Cell(2, 1).Text = "使用单位：";
        //    flexGrid1.Cell(2, 4).Text = "登记时间：" + DateTime.Now.ToShortDateString();
        //    flexGrid1.Cell(2, 7).Text = "制单编号：" + billMaster.Code;
        //    flexGrid1.Cell(4, 2).Text = billMaster.OriginalContractNo;
        //    flexGrid1.Cell(4, 5).Text = "";//合同名称
        //    flexGrid1.Cell(4, 7).Text = "";//材料分类
        //    flexGrid1.Cell(5, 2).Text = "";//租赁单位
        //    flexGrid1.Cell(5, 2).WrapText = true;
        //    flexGrid1.Cell(5, 5).Text = "";//承租单位
        //    flexGrid1.Row(5).AutoFit();
        //    flexGrid1.Cell(5, 7).Text = billMaster.RealOperationDate.ToShortDateString();//签订日期

        //    FlexCell.PageSetup pageSetup = flexGrid1.PageSetup;
        //    pageSetup.LeftFooter = "   制单人：" + billMaster.CreatePersonName;
        //    pageSetup.RightFooter = "第 &P 页/共 &N 页      ";

        //    System.Drawing.Printing.PaperSize paperSize = new System.Drawing.Printing.PaperSize("自定义纸张", 910, 470);
        //    pageSetup.PaperSize = paperSize;

        //    //填写明细数据
        //    for (int i = 0; i < detailCount; i++)
        //    {
        //        MaterialRentalOrderDetail detail = (MaterialRentalOrderDetail)billMaster.Details.ElementAt(i);
        //        //物资名称
        //        flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialResource.Name;
        //        flexGrid1.Cell(detailStartRowNumber + i, 1).Alignment = FlexCell.AlignmentEnum.LeftCenter;

        //        //规格型号
        //        flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialResource.Specification;
        //        flexGrid1.Cell(detailStartRowNumber + i, 2).Alignment = FlexCell.AlignmentEnum.LeftCenter;

        //        //结算规则
        //        flexGrid1.Cell(detailStartRowNumber + i, 3).Text = Enum.GetName(typeof(EnumMaterialMngBalRule), detail.BalRule);
        //        flexGrid1.Cell(detailStartRowNumber + i, 3).Alignment = FlexCell.AlignmentEnum.LeftCenter;

        //        //数量
        //        flexGrid1.Cell(detailStartRowNumber + i, 4).Text = detail.Quantity.ToString();

        //        //日租金
        //        flexGrid1.Cell(detailStartRowNumber + i, 5).Text = "";

        //        //金额
        //        flexGrid1.Cell(detailStartRowNumber + i, 6).Text = "";

        //        //备注
        //        flexGrid1.Cell(detailStartRowNumber + i, 7).Text = detail.Descript;
        //        flexGrid1.Cell(detailStartRowNumber + i, 7).Alignment = FlexCell.AlignmentEnum.LeftCenter;
        //    }
        //}
    }
}
