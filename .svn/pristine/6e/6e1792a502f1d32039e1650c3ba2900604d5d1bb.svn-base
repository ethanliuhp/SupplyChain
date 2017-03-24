using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.Properties;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using FlexCell;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using System.IO;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using IRPServiceModel.Domain.Document;
using NHibernate.Criterion;
using IRPServiceModel.Basic;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using Application.Business.Erp.SupplyChain.Client.FileUpload;


namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI
{
    public partial class VScrollSchedulePlan : TMasterDetailView
    {
        private MProductionMng model = new MProductionMng();
        private Hashtable detailHashtable = new Hashtable();
        private ProductionScheduleMaster CurBillMaster;
        private ProductionScheduleDetail ChildRootNode;
        private string imageExpand = "imageExpand";
        private string imageCollapse = "imageCollapse";
        private EnumScheduleType enumScheduleType;
        private List<string> listDtlIds = new List<string>();
        private CurrentProjectInfo projectInfo = null;
        string userName = string.Empty;
        string jobId = string.Empty;

        private int startImageCol = 1, endImageCol = 19;

        public VScrollSchedulePlan(EnumScheduleType enumScheduleType)
        {
            InitializeComponent();
            this.enumScheduleType = enumScheduleType;

            InitEvents();
            InitData();
        }

        private void InitEvents()
        {
            btnSetConfigDate.Click += new EventHandler(btnSetConfigDate_Click);
            btnCreateNewVersion.Click += new EventHandler(btnCreateNewVersion_Click);

            btnGWBS.Click += new EventHandler(btnGWBS_Click);
            btnSelectChildTask.Click += new EventHandler(btnSelectChildTask_Click);

            cbShowAllPlanDtl.Click += new EventHandler(cbShowAllPlanDtl_Click);


            cboScheduleName.SelectedIndexChanged += new EventHandler(cboScheduleType_SelectedIndexChanged);
            cbScheduleVersion.SelectedIndexChanged += new EventHandler(cbScheduleVersion_SelectedIndexChanged);

            flexGrid.Click += new FlexCell.Grid.ClickEventHandler(flexGrid_Click);
            flexGrid.LeaveCell += new Grid.LeaveCellEventHandler(flexGrid_LeaveCell);

            btnDeletePlanMaster.Click += new EventHandler(btnDeletePlanMaster_Click);

            btnPublish.Click += new EventHandler(btnPublish_Click);
            btnInvalid.Click += new EventHandler(btnInvalid_Click);

            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnInvalidPlanDtl.Click += new EventHandler(btnInvalidPlanDtl_Click);
            btnEffectPlanDtl.Click += new EventHandler(btnEffectPlanDtl_Click);
            btnSave.Click += new EventHandler(btnSave_Click);

            btnCopyAndNew.Click += new EventHandler(btnCopyAndNew_Click);

            btnExportToMPP.Click += new EventHandler(btnExportToMPP_Click);
            btnImportFromMPP.Click += new EventHandler(btnImportFromMPP_Click);

            btnImportScrollPlan.Click += new EventHandler(btnImportScrollPlan_Click);

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
            //tab页切换数据处理
            tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
        }
        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == tabPage1.Name)//计划明细
            {

            }
            else if (tabControl1.SelectedTab.Name == tabPage2.Name)//相关文档
            {
                if (CurBillMaster != null && !string.IsNullOrEmpty(CurBillMaster.Id))
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
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", CurBillMaster.Id));
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
            if (CurBillMaster.Id == null)
            {
                if (MessageBox.Show("当前业务对象还没保存，是否保存！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    //if (!ValidView()) return;
                    try
                    {
                        //if (!ViewToModel()) return;
                        CurBillMaster = model.ProductionManagementSrv.NewSchedule(CurBillMaster) as ProductionScheduleMaster; ;
                        this.ViewCaption = ViewName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(ex));
                    }
                }
            }
            if (CurBillMaster.Id != null)
            {
                VDocumentPublicUpload frm = new VDocumentPublicUpload(CurBillMaster.Id);
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
            if (e.RowIndex < 1)//点击标题时无需加载
            {
                return;
            }
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
        public void btnSetConfigDate_Click(object sender, EventArgs e)
        {
            VScheduleSetDate oVScheduleSetDate = new VScheduleSetDate();
            oVScheduleSetDate.ShowDialog();
            if (oVScheduleSetDate.IsUpdate)
            {
                DateTime StartDate = DateTime.Parse(oVScheduleSetDate.StartDate.ToShortDateString());
                DateTime EndDate = DateTime.Parse(oVScheduleSetDate.EndDate.ToShortDateString());
                string sMsg = string.Format("确定将[{0}] 至 [{1}] 区间的任务日期设置为空：修改请点击[确认] ,撤销点击[取消]", StartDate.ToShortDateString(), EndDate.ToShortDateString());
                if (MessageBox.Show(sMsg, "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    DateTime StartDateTemp;
                    DateTime EndDateTemp;

                    FlexCell.Cell oStartCell = null;
                    FlexCell.Cell oEndCell = null;
                    FlexCell.Cell oDaysCell = null;
                    if (StartDate <= EndDate)
                    {
                        for (int rowIndex = 1; rowIndex < flexGrid.Rows; rowIndex++)
                        {
                            try
                            {
                                oStartCell = flexGrid.Cell(rowIndex, endImageCol + 2);
                                oEndCell = flexGrid.Cell(rowIndex, endImageCol + 3);
                                oDaysCell = flexGrid.Cell(rowIndex, endImageCol + 4);
                                if (!string.IsNullOrEmpty(oStartCell.Text.Trim()) && !string.IsNullOrEmpty(oEndCell.Text.Trim()))
                                {
                                    StartDateTemp = DateTime.Parse(oStartCell.Text);
                                    EndDateTemp = DateTime.Parse(oEndCell.Text);
                                    if (StartDateTemp <= EndDateTemp && (StartDateTemp >= StartDate && StartDateTemp <= EndDate) && (EndDateTemp <= EndDate && EndDateTemp >= StartDate))
                                    {
                                        oStartCell.Text = "";
                                        oEndCell.Text = "";
                                        oDaysCell.Text = "";
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(string.Format("起始时间[{0}]应该小于等于结束时间[{1}]", StartDate.ToShortDateString(), EndDate.ToShortDateString()));
                    }
                }
            }

        }
        public void flexGrid_LeaveCell(object sender, Grid.LeaveCellEventArgs e)
        {
            if (e.Row > 0 && e.Col > 19 && e.Col < 24)
            {
                string sStartTime = flexGrid.Cell(e.Row, endImageCol + 2).Text;
                string sFinishTime = flexGrid.Cell(e.Row, endImageCol + 3).Text;
                if (!string.IsNullOrEmpty(sStartTime) && !string.IsNullOrEmpty(sFinishTime))
                {
                    try
                    {
                        DateTime StartTime = DateTime.Parse(sStartTime);
                        DateTime FinnishTime = DateTime.Parse(sFinishTime);
                        if (StartTime <= FinnishTime)
                        {
                            flexGrid.Cell(e.Row, endImageCol + 4).Text = ((FinnishTime - StartTime).Days + 1).ToString();
                        }
                        else
                        {
                            flexGrid.Cell(e.Row, endImageCol + 4).Text = string.Empty;
                        }
                    }
                    catch
                    {
                    }
                }


            }
        }

        private void ShowPlanFactInfo(bool isShow)
        {
            flexGrid.Column(endImageCol + 6).Visible = isShow;
            flexGrid.Column(endImageCol + 7).Visible = isShow;
            flexGrid.Column(endImageCol + 8).Visible = isShow;
        }


        private void InitData()
        {
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;

            InitFlexGrid(5);

            //ShowPlanFactInfo(true);

            //归属项目
            projectInfo = StaticMethod.GetProjectInfo();

            //进度计划口径
            VBasicDataOptr.InitScheduleCaliber(cboScheduleCaliber, false);
            if (cboScheduleCaliber.Items.Count > 0)
            {
                cboScheduleCaliber.SelectedIndex = 0;
            }

            if (enumScheduleType == EnumScheduleType.总进度计划)
            {
                //总进度计划类型
                VBasicDataOptr.InitScheduleType(cboScheduleName, false);

                if (cboScheduleName.Items.Count > 0)
                {
                    cboScheduleName.SelectedIndex = 0;
                }

                btnPubSchedule.Visible = false;
                btnImportScrollPlan.Visible = false;

                ShowPlanFactInfo(false);
            }
            else if (enumScheduleType == EnumScheduleType.总滚动进度计划)
            {
                //总滚动进度计划类型
                VBasicDataOptr.InitScheduleTypeRolling(cboScheduleName, false);


                if (cboScheduleName.Items.Count > 0)
                {
                    cboScheduleName.SelectedIndex = 0;
                }

                btnPubSchedule.Visible = false;
                btnImportScrollPlan.Visible = false;

                ShowPlanFactInfo(true);
            }
        }

        void btnImportFromMPP_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "项目 (*.MPP)|*.MPP";
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string fileName = ofd.FileName;
                List<ProductionScheduleDetail> list = MSProjectUtil.ReadMPP(fileName);
                updateSchedule(list);

                if (CurBillMaster != null)
                {
                    //UpLoadFile(CurBillMaster, fileName);
                }
            }
        }
        //public void UpLoadFile(ProductionScheduleMaster oMaster, string sPathFile)
        //{

        //    ProObjectRelaDocument pord = MSProjectUtil.UpLoadFile(this.projectInfo, oMaster.Id, oMaster.GetType().Name, this.userName, this.jobId, sPathFile);
        //    if (pord != null)
        //    {
        //        int rowIndex = gridDocument.Rows.Add();
        //        gridDocument[DocumentName.Name, rowIndex].Value = pord.DocumentName;
        //        gridDocument[DocumentCateCode.Name, rowIndex].Value = pord.DocumentCateCode;
        //        gridDocument[DocumentCateName.Name, rowIndex].Value = pord.DocumentCateName;
        //        gridDocument[DocumentCode.Name, rowIndex].Value = pord.DocumentCode;
        //        gridDocument[DocumentDesc.Name, rowIndex].Value = pord.DocumentDesc;
        //        gridDocument.Rows[rowIndex].Tag = pord;
        //    }
        //}
        private void updateSchedule(List<ProductionScheduleDetail> list)
        {
            if (list == null || list.Count == 0)
                return;

            foreach (ProductionScheduleDetail detail in list)
            {
                for (int i = 1; i <= flexGrid.Rows; i++)
                {
                    if (flexGrid.Cell(i, 0) == null)
                        continue;

                    string detailId = flexGrid.Cell(i, 0).Tag;
                    if (detail.Id == detailId)
                    {
                        //计划开始日期
                        //  flexGrid.Cell(i, endImageCol + 1).Text = detail.PlannedBeginDate.ToShortDateString();
                        flexGrid.Cell(i, endImageCol + 2).Text = detail.PlannedBeginDate.ToShortDateString();
                        //计划结束日期
                        flexGrid.Cell(i, endImageCol + 3).Text = detail.PlannedEndDate.ToShortDateString();
                        //工期
                        flexGrid.Cell(i, endImageCol + 4).Text = detail.PlannedDuration.ToString();
                        break;
                    }
                }
            }
        }

        //导出
        void btnExportToMPP_Click(object sender, EventArgs e)
        {

            if (CurBillMaster == null)
            {
                MessageBox.Show("当前没有要导出的计划!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //OpenFileDialog ofg = new OpenFileDialog();
            SaveFileDialog sfd = new SaveFileDialog();
            //openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            sfd.Filter = "项目 (*.MPP)|*.MPP";
            sfd.RestoreDirectory = true;
            sfd.FileName = CurBillMaster.ScheduleTypeDetail + "_" + CurBillMaster.ScheduleName + "_" + string.Format("{0:yyyy年MM月dd日HH点mm分}", DateTime.Now) + ".mpp";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string fileName = sfd.FileName;
                if (System.IO.File.Exists(fileName))
                {
                    IList list = model.ProductionManagementSrv.GetChilds(CurBillMaster);
                    // MSProjectUtil mSProjectUtil = new MSProjectUtil();
                    //mSProjectUtil.UpdateMPP(fileName, list);
                    MSProjectUtil.UpdateProject(fileName, list, listDtlIds, this.Handle);
                }
                else
                {

                    IList list = model.ProductionManagementSrv.GetChilds(CurBillMaster);//这是全部树
                    //string dtlId = flexGrid.Cell(i, 0).Tag;
                    //if (listDtlIds.Contains(dtlId))
                    //    flexGrid.Row(i).Visible = true;
                    //else
                    //    flexGrid.Row(i).Visible = false;
                    MSProjectUtil.CreateMPP(fileName, list, listDtlIds, this.Handle);


                    //IList list = model.ProductionManagementSrv.GetChilds(CurBillMaster);
                    //MSProjectUtil.CreateMPP(fileName, list, this.Handle);


                }
            }


        }

        //创建新版本
        void btnCreateNewVersion_Click(object sender, EventArgs e)
        {
            if (cboScheduleName.Text.Trim() == "")
            {
                MessageBox.Show("请选择一个计划名称!");
                cboScheduleName.Focus();
                return;
            }

            VSchduleVersionEdit frm = new VSchduleVersionEdit();
            foreach (string v in cbScheduleVersion.Items)
            {
                frm.ListHasVersions.Add(v);
            }

            if (frm.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show("您已经取消了新版本的创建！", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //if (MessageBox.Show("是否要作废当前有效版本的计划？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //{
            //    if (CancellationAvailabilityPlan() == false)
            //        return;
            //}

            string planVersion = frm.NewVersion;

            CurBillMaster = new ProductionScheduleMaster();
            NewMaster(CurBillMaster);
            CurBillMaster.ScheduleName = planVersion;

            CurBillMaster = model.ProductionManagementSrv.NewSchedule(CurBillMaster) as ProductionScheduleMaster;
            ChildRootNode = CurBillMaster.GetChildRootNode();

            cbScheduleVersion.Items.Add(CurBillMaster.ScheduleName);
            cbScheduleVersion.SelectedIndex = cbScheduleVersion.Items.Count - 1;

            txtRemark.Text = "";
            txtPlanState.Text = ClientUtil.GetDocStateName(CurBillMaster.DocState);

            txtChildTask.Text = "";

            FillFlex();
        }

        //作废有效的计划
        private bool CancellationAvailabilityPlan()
        {
            try
            {
                string scheduleName = cboScheduleName.SelectedItem as string;

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                oq.AddCriterion(Expression.Eq("ScheduleType", this.enumScheduleType));
                oq.AddCriterion(Expression.Eq("ScheduleTypeDetail", scheduleName));
                oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));

                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                IList listMaster = model.ProductionManagementSrv.ObjectQuery(typeof(ProductionScheduleMaster), oq);

                for (int i = 0; i < listMaster.Count; i++)
                {
                    ProductionScheduleMaster master = listMaster[i] as ProductionScheduleMaster;
                    master.DocState = DocumentState.Invalid;

                    for (int j = 0; j < master.Details.Count; j++)
                    {
                        ProductionScheduleDetail dtl = master.Details.ElementAt(j);
                        dtl.State = EnumScheduleDetailState.失效;
                    }

                    model.ProductionManagementSrv.SaveOrUpdateByDao(listMaster);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            return false;
        }

        //作废整个计划
        private bool CancellationPlan(ProductionScheduleMaster plan)
        {
            try
            {
                CurBillMaster = model.ProductionManagementSrv.GetSchedulesById(CurBillMaster.Id);

                CurBillMaster.DocState = DocumentState.Invalid;

                for (int i = 0; i < CurBillMaster.Details.Count; i++)
                {
                    ProductionScheduleDetail dtl = CurBillMaster.Details.ElementAt(i);
                    dtl.State = EnumScheduleDetailState.失效;
                }

                CurBillMaster = model.ProductionManagementSrv.SaveOrUpdateByDao(CurBillMaster) as ProductionScheduleMaster;

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            return false;
        }

        //复制计划
        void btnCopyAndNew_Click(object sender, EventArgs e)
        {
            if (CurBillMaster == null)
            {
                MessageBox.Show("请选择一个计划。");
                cboScheduleName.Focus();
                return;
            }


            VSchduleVersionEdit frm = new VSchduleVersionEdit();
            foreach (string v in cbScheduleVersion.Items)
            {
                frm.ListHasVersions.Add(v);
            }

            if (frm.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show("您已经取消了计划的复制！", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            //if (MessageBox.Show("是否要作废当前有效版本的计划？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //{
            //    if (CancellationAvailabilityPlan() == false)
            //        return;
            //}

            CurBillMaster.ScheduleName = frm.NewVersion;
            CurBillMaster = model.ProductionManagementSrv.CopyNewSchdulePlan(CurBillMaster);

            ChildRootNode = CurBillMaster.GetChildRootNode();

            cbScheduleVersion.Items.Add(CurBillMaster.ScheduleName);
            cbScheduleVersion.SelectedIndex = cbScheduleVersion.Items.Count - 1;

            txtChildTask.Text = "";

            txtRemark.Text = "";
            txtPlanState.Text = ClientUtil.GetDocStateName(CurBillMaster.DocState);
            cboScheduleCaliber.Text = CurBillMaster.ScheduleCaliber;

            FillFlex();

            MessageBox.Show("复制成功！", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //发布计划
        void btnPublish_Click(object sender, EventArgs e)
        {
            //发布整个计划
            if (CurBillMaster == null)
            {
                MessageBox.Show("当前没有要发布的计划!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (CurBillMaster.DocState == DocumentState.Invalid)
            {
                MessageBox.Show("当前计划已作废，不能发布!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //if (MessageBox.Show("发布该计划前是否要作废其他所有有效版本的计划？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //{
            //    if (CancellationAvailabilityPlan() == false)
            //        return;
            //}
            //else
            //{

            //}

            if (PublishSchedule(false) == false)
                return;

            txtPlanState.Text = ClientUtil.GetDocStateName(CurBillMaster.DocState);

            FillFlex();

            MessageBox.Show("发布成功!", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);

            #region 发布明细
            //int activeRowIndex = flexGrid.ActiveCell.Row;
            //string detailId = flexGrid.Cell(activeRowIndex, 0).Tag;
            //if (detailId != null && !detailId.Equals(""))
            //{
            //    ProductionScheduleDetail detail = (ProductionScheduleDetail)detailHashtable[detailId];
            //    int childs = model.ProductionManagementSrv.CountAllChildNodes(detailId);
            //    if (detail == null)
            //    {
            //        MessageBox.Show("没有找到进度计划。");
            //        return;
            //    }
            //    if (MessageBox.Show("确定要发布【" + detail.GWBSTreeName + "】和其下的进度计划吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            //    {
            //        try
            //        {
            //            model.ProductionManagementSrv.PublishScheduleDetail(detailId);
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show("发布出错。\n" + ex.Message);
            //        }
            //    }
            //}
            #endregion
        }

        //作废整个计划
        void btnInvalid_Click(object sender, EventArgs e)
        {
            //作废整个计划
            if (CurBillMaster == null)
            {
                MessageBox.Show("当前没有要作废的计划!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (CurBillMaster.DocState == DocumentState.Invalid)
            {
                MessageBox.Show("当前计划已作废!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ////作废整个计划
            //if (CurBillMaster == null)
            //{
            //    string planName = cboScheduleName.SelectedItem.ToString();
            //    string planVersion = cbScheduleVersion.Text.Trim();

            //    ObjectQuery oq = new ObjectQuery();
            //    oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            //    oq.AddCriterion(Expression.Eq("ScheduleType", this.enumScheduleType));
            //    oq.AddCriterion(Expression.Eq("ScheduleTypeDetail", planName));
            //    oq.AddCriterion(Expression.Eq("ScheduleName", planVersion));

            //    IList listMaster = model.ProductionManagementSrv.ObjectQuery(typeof(ProductionScheduleMaster), oq);
            //    if (listMaster.Count > 0)//至少有一个计划
            //    {
            //        CurBillMaster = listMaster[0] as ProductionScheduleMaster;
            //    }
            //}

            //if (CurBillMaster == null)
            //{
            //    MessageBox.Show("当前没有要设置的计划，请检查!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            CancellationPlan(CurBillMaster);

            txtPlanState.Text = ClientUtil.GetDocStateName(CurBillMaster.DocState);

            FillFlex();

            MessageBox.Show("作废设置成功!", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);

            #region 作废明细
            //int activeRowIndex = flexGrid.ActiveCell.Row;
            //string detailId = flexGrid.Cell(activeRowIndex, 0).Tag;
            //if (detailId != null && !detailId.Equals(""))
            //{
            //    ProductionScheduleDetail detail = (ProductionScheduleDetail)detailHashtable[detailId];
            //    int childs = model.ProductionManagementSrv.CountAllChildNodes(detailId);
            //    if (detail == null)
            //    {
            //        MessageBox.Show("没有找到进度计划。");
            //        return;
            //    }
            //    if (MessageBox.Show("确定要作废【" + detail.GWBSTreeName + "】和其下的进度计划吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            //    {
            //        try
            //        {
            //            model.ProductionManagementSrv.InvalidScheduleDetail(detailId);
            //            for (int i = 0; i <= childs; i++)
            //            {
            //                flexGrid.Row(activeRowIndex).Delete();
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show("作废出错。\n" + ex.Message);
            //        }
            //    }
            //}
            #endregion
        }

        //保存
        void btnSave_Click(object sender, EventArgs e)
        {
            if (CurBillMaster == null)
            {
                MessageBox.Show("当前没有要保存的计划!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //if (CurBillMaster.DocState != DocumentState.Edit)
            //{
            //    MessageBox.Show("当前计划状态为“" + ClientUtil.GetDocStateName(CurBillMaster.DocState) + "”,只能修改“编辑”状态的计划！");
            //    return;
            //}

            if (cboScheduleName.SelectedItem == null)
            {
                MessageBox.Show("请选择一个计划名称。");
                cboScheduleName.Focus();
                return;
            }
            if (cbScheduleVersion.Text.Trim() == "")
            {
                MessageBox.Show("请输入计划版本。");
                cbScheduleVersion.Focus();
                return;
            }

            if (SaveSchedule(false))
            {
                MessageBox.Show("保存成功。");
            }
        }

        //导入滚动计划
        void btnImportScrollPlan_Click(object sender, EventArgs e)
        {
            if (cboScheduleName.Text.Trim() == "")
            {
                MessageBox.Show("请选择一个计划名称!");
                cboScheduleName.Focus();
                return;
            }


            VSelectSchdulePlan frm = new VSelectSchdulePlan();
            frm.ShowDialog();

            if (frm.isOK == false)
                return;

            ProductionScheduleMaster master = frm.SelectPlanMaster;


            VSchduleVersionEdit frmVersion = new VSchduleVersionEdit();
            foreach (string v in cbScheduleVersion.Items)
            {
                frmVersion.ListHasVersions.Add(v);
            }

            if (frmVersion.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show("您已经取消了滚动计划的导入！", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            CurBillMaster = master;

            CurBillMaster.ScheduleType = EnumScheduleType.总滚动进度计划;
            CurBillMaster.ScheduleTypeDetail = cboScheduleName.SelectedItem.ToString();
            CurBillMaster.ScheduleName = frmVersion.NewVersion;

            CurBillMaster = model.ProductionManagementSrv.CopyNewScrollSchdulePlan(CurBillMaster);

            ChildRootNode = CurBillMaster.GetChildRootNode();

            cbScheduleVersion.Items.Add(CurBillMaster.ScheduleName);
            cbScheduleVersion.SelectedIndex = cbScheduleVersion.Items.Count - 1;

            txtRemark.Text = "";
            txtPlanState.Text = ClientUtil.GetDocStateName(CurBillMaster.DocState);
            cboScheduleCaliber.Text = CurBillMaster.ScheduleCaliber;

            FillFlex();

            MessageBox.Show("复制成功！", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool SaveSchedule(bool isNewVersion)
        {
            try
            {
                if (CurBillMaster == null)
                {
                    CurBillMaster = new ProductionScheduleMaster();
                    NewMaster(CurBillMaster);
                    CurBillMaster = model.ProductionManagementSrv.NewSchedule(CurBillMaster) as ProductionScheduleMaster;
                    ChildRootNode = CurBillMaster.GetChildRootNode();
                }
                else
                    CurBillMaster = model.ProductionManagementSrv.GetSchedulesById(CurBillMaster.Id);

                string oldVersion = CurBillMaster.ScheduleName;

                ViewToMaster(isNewVersion);

                string currVersion = cbScheduleVersion.Text.Trim();
                if (currVersion != oldVersion)
                {
                    for (int i = 0; i < cbScheduleVersion.Items.Count; i++)
                    {
                        string value = cbScheduleVersion.Items[i] as string;
                        if (value == oldVersion)
                        {
                            cbScheduleVersion.Items[i] = currVersion;
                            break;
                        }
                    }
                }

                ViewToDetails();
                CurBillMaster = model.ProductionManagementSrv.SaveOrUpdateByDao(CurBillMaster) as ProductionScheduleMaster;

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存出错。\n" + ex.Message);
            }
            return false;
        }

        private bool PublishSchedule(bool isNewVersion)
        {
            try
            {

                FlashScreen.Show("正在执行发布操作,请稍候......");

                CurBillMaster = model.ProductionManagementSrv.GetSchedulesById(CurBillMaster.Id);

                string oldVersion = CurBillMaster.ScheduleName;

                ViewToMaster(isNewVersion);

                string currVersion = cbScheduleVersion.Text.Trim();
                if (currVersion != oldVersion)
                {
                    for (int i = 0; i < cbScheduleVersion.Items.Count; i++)
                    {
                        string value = cbScheduleVersion.Items[i] as string;
                        if (value == oldVersion)
                        {
                            cbScheduleVersion.Items[i] = currVersion;
                            break;
                        }
                    }
                }

                ViewToDetails();


                CurBillMaster.DocState = DocumentState.InExecute;

                for (int i = 0; i < CurBillMaster.Details.Count; i++)
                {
                    ProductionScheduleDetail dtl = CurBillMaster.Details.ElementAt(i);
                    dtl.State = EnumScheduleDetailState.有效;
                }

                CurBillMaster = model.ProductionManagementSrv.SaveOrUpdateByDao(CurBillMaster) as ProductionScheduleMaster;

                return true;
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("发布操作失败。\n" + ex.Message);
            }
            finally
            {
                FlashScreen.Close();
            }
            return false;
        }

        private void ViewToMaster(bool isNewVersion)
        {
            CurBillMaster.ScheduleTypeDetail = cboScheduleName.SelectedItem + "";
            CurBillMaster.ScheduleCaliber = cboScheduleCaliber.SelectedItem + "";
            if (!isNewVersion)//修改版本名称
                CurBillMaster.ScheduleName = cbScheduleVersion.Text.Trim();
            CurBillMaster.Descript = txtRemark.Text;
        }

        private IList ViewToDetails()
        {
            IList list = new ArrayList();

            if (!string.IsNullOrEmpty(CurBillMaster.Id))
            {
                for (int i = 1; i < flexGrid.Rows; i++)
                {
                    string detailId = flexGrid.Cell(i, 0).Tag;

                    if (detailId == null || detailId.Equals(""))
                        continue;

                    ProductionScheduleDetail detail = null;
                    foreach (ProductionScheduleDetail tempDetail in CurBillMaster.Details)
                    {
                        if (detailId == tempDetail.Id)
                        {
                            detail = tempDetail;
                            break;
                        }
                    }

                    //if (detail.State == EnumScheduleDetailState.编辑)
                    //{
                    //计划开始时间
                    string PlannedBeginDateStr = flexGrid.Cell(i, endImageCol + 2).Text;
                    if (PlannedBeginDateStr != null && !PlannedBeginDateStr.Equals(""))
                    {
                        detail.PlannedBeginDate = DateTime.Parse(PlannedBeginDateStr);
                    }
                    else
                    {
                        detail.PlannedBeginDate = new DateTime(1900, 1, 1);
                    }
                    //计划结束时间
                    string PlannedEndDateStr = flexGrid.Cell(i, endImageCol + 3).Text;
                    if (PlannedEndDateStr != null && !PlannedEndDateStr.Equals(""))
                    {
                        detail.PlannedEndDate = DateTime.Parse(PlannedEndDateStr);
                    }
                    else
                    {
                        detail.PlannedEndDate = new DateTime(1900, 1, 1);
                    }
                    //计划工期
                    detail.PlannedDuration = flexGrid.Cell(i, endImageCol + 4).IntegerValue;
                    //工期计量单位
                    detail.ScheduleUnit = flexGrid.Cell(i, endImageCol + 5).Text;


                    //计划说明
                    detail.TaskDescript = flexGrid.Cell(i, endImageCol + 6).Text;

                    CurBillMaster.AddDetail(detail);
                    //}
                }
            }
            return list;
        }

        private void ClearView()
        {
            flexGrid.Rows = 1;

            txtChildTask.Text = "";
            txtChildTask.Tag = null;

            cbShowAllPlanDtl.Checked = false;

            txtRemark.Text = "";
            txtPlanState.Text = "";
            cbScheduleVersion.Items.Clear();
            cboScheduleCaliber.SelectedIndex = 0;
        }

        private void ClearVersionData(bool removeCurrVersion)
        {
            flexGrid.Rows = 1;

            txtChildTask.Text = "";
            txtChildTask.Tag = null;

            cbShowAllPlanDtl.Checked = false;

            txtRemark.Text = "";
            txtPlanState.Text = "";

            cboScheduleCaliber.SelectedIndex = 0;

            if (removeCurrVersion && cbScheduleVersion.Items.Count > 0 && cbScheduleVersion.Text.Trim() != "")
            {
                cbScheduleVersion.Items.Remove(cbScheduleVersion.Text.Trim());

                if (cbScheduleVersion.Items.Count > 0)
                    cbScheduleVersion.SelectedIndex = 0;
            }
        }

        //删除主表
        void btnDeletePlanMaster_Click(object sender, EventArgs e)
        {
            if (CurBillMaster == null)
            {
                MessageBox.Show("请选择一个计划!");
                cboScheduleName.Focus();
                return;
            }
            else if (CurBillMaster.DocState != DocumentState.Edit)
            {
                MessageBox.Show("当前计划的状态为【" + ClientUtil.GetDocStateName(CurBillMaster.DocState) + "】，只能删除“编辑”状态的计划!");
                return;
            }

            if (MessageBox.Show("您确认要删除当前版本的计划吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    CurBillMaster = model.ProductionManagementSrv.GetObjectById(typeof(ProductionScheduleMaster), CurBillMaster.Id) as ProductionScheduleMaster;

                    if (CurBillMaster.DocState == DocumentState.Edit)
                    {
                        if (!model.ProductionManagementSrv.DeleteByDao(CurBillMaster))
                            return;

                        CurBillMaster = null;

                        ClearVersionData(true);

                    }
                    else
                        MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(CurBillMaster.DocState) + "】，不能删除！");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
                }
            }
        }

        //删除明细
        void btnDelete_Click(object sender, EventArgs e)
        {
            if (CurBillMaster == null)
            {
                MessageBox.Show("当前没有要操作的计划!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //if (CurBillMaster.DocState != DocumentState.Edit)
            //{
            //    MessageBox.Show("当前计划状态为“" + ClientUtil.GetDocStateName(CurBillMaster.DocState) + "”,只能删除“编辑”状态的计划！");
            //    return;
            //}

            int activeRowIndex = flexGrid.ActiveCell.Row;
            string detailId = flexGrid.Cell(activeRowIndex, 0).Tag;
            if (detailId != null && !detailId.Equals(""))
            {
                ProductionScheduleDetail detail = (ProductionScheduleDetail)detailHashtable[detailId];
                if (detail == null)
                {
                    MessageBox.Show("选择计划明细不存在(或已被其他操作员删除),请重载该计划！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (detail.Level == 1)
                {
                    MessageBox.Show("根节点不能删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("确定要删除【" + detail.GWBSTreeName + "】及其下属滚动计划吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        FlashScreen.Show("正在执行删除操作,请稍候......");

                        int childs = 0;
                        string errMsg = "";

                        IList list = model.ProductionManagementSrv.DeleteScheduleDetail(detail, childs, errMsg);
                        errMsg = list[0] as string;
                        childs = Convert.ToInt32(list[1].ToString());

                        if (!string.IsNullOrEmpty(errMsg))
                        {
                            FlashScreen.Close();
                            MessageBox.Show(errMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        //flexGrid.AutoRedraw = false;

                        int rowIndexs = activeRowIndex + childs;
                        for (int i = rowIndexs; i >= activeRowIndex; i--)
                        {
                            flexGrid.Row(i).Delete();
                        }
                        //flexGrid.AutoRedraw = true;

                    }
                    catch (Exception ex)
                    {
                        FlashScreen.Close();
                        MessageBox.Show("删除失败.\n" + ex.Message);
                        return;
                    }
                    finally
                    {
                        FlashScreen.Close();
                    }

                    MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        //作废明细
        void btnInvalidPlanDtl_Click(object sender, EventArgs e)
        {
            if (CurBillMaster == null)
            {
                MessageBox.Show("当前没有要操作的计划!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int activeRowIndex = flexGrid.ActiveCell.Row;
            string detailId = flexGrid.Cell(activeRowIndex, 0).Tag;
            if (detailId != null && !detailId.Equals(""))
            {
                ProductionScheduleDetail detail = (ProductionScheduleDetail)detailHashtable[detailId];
                if (detail == null)
                {
                    MessageBox.Show("没有找到计划明细。");
                    return;
                }

                if (detail.Level == 1)
                {
                    MessageBox.Show("根节点不能作废！");
                    return;
                }

                if (MessageBox.Show("确定要作废【" + detail.GWBSTreeName + "】及其下属“有效”的滚动计划吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    try
                    {
                        FlashScreen.Show("正在执行作废操作,请稍候......");

                        IList listChilds = null;

                        model.ProductionManagementSrv.InvalidScrollSchdulePlanDtl(detail, out listChilds);

                        if (listChilds != null && listChilds.Count > 0)
                        {
                            IEnumerable<ProductionScheduleDetail> queryChilds = listChilds.OfType<ProductionScheduleDetail>();
                            for (int i = 0; i < listChilds.Count; i++)
                            {
                                detailId = flexGrid.Cell(activeRowIndex + i, 0).Tag;

                                var query = from d in queryChilds
                                            where d.Id == detailId
                                            select d;

                                if (query.Count() > 0)
                                {
                                    detail = query.ElementAt(0);
                                    if (detailHashtable.ContainsKey(detail.Id))
                                        detailHashtable[detail.Id] = detail;
                                    else
                                        detailHashtable.Add(detail.Id, detail);

                                    if (detail.State == EnumScheduleDetailState.失效)
                                    {
                                        flexGrid.Cell(activeRowIndex + i, endImageCol + 1).Text = EnumScheduleDetailState.失效.ToString();
                                        flexGrid.Cell(activeRowIndex + i, endImageCol + 1).ForeColor = Color.Red;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        FlashScreen.Close();
                        MessageBox.Show("作废操作失败.\n" + ex.Message);
                        return;
                    }
                    finally
                    {
                        FlashScreen.Close();
                    }
                    //MessageBox.Show("作废成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        //发布明细
        void btnEffectPlanDtl_Click(object sender, EventArgs e)
        {
            if (CurBillMaster == null)
            {
                MessageBox.Show("当前没有要操作的计划!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int activeRowIndex = flexGrid.ActiveCell.Row;
            string detailId = flexGrid.Cell(activeRowIndex, 0).Tag;
            if (detailId != null && !detailId.Equals(""))
            {
                ProductionScheduleDetail detail = (ProductionScheduleDetail)detailHashtable[detailId];
                if (detail == null)
                {
                    MessageBox.Show("没有找到计划明细。");
                    return;
                }

                //if (detail.Level == 1)
                //{
                //    MessageBox.Show("此节点不能作废！");
                //    return;
                //}

                if (MessageBox.Show("确定要发布【" + detail.GWBSTreeName + "】及其下属“编辑、失效”的滚动计划节点吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {

                        FlashScreen.Show("正在执行发布操作,请稍候......");

                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("Master.Id", CurBillMaster.Id));
                        oq.AddCriterion(Expression.Like("SysCode", detail.SysCode, MatchMode.Start));
                        oq.AddCriterion(Expression.Eq("State", EnumScheduleDetailState.编辑));

                        IEnumerable<ProductionScheduleDetail> listDtl = model.ProductionManagementSrv.ObjectQuery(typeof(ProductionScheduleDetail), oq).OfType<ProductionScheduleDetail>();

                        string sql = "select count(*) from thd_productionscheduledetail t1 where t1.parentid='" + CurBillMaster.Id + "' and t1.syscode like '" + detail.SysCode + "%'";
                        int childCount = ClientUtil.ToInt(model.ProductionManagementSrv.SearchSQL(sql).Tables[0].Rows[0][0]);
                        for (int j = 0; j < listDtl.Count(); j++)
                        {
                            ProductionScheduleDetail dtl = listDtl.ElementAt(j) as ProductionScheduleDetail;

                            for (int i = activeRowIndex; i < activeRowIndex + childCount; i++)
                            {
                                detailId = flexGrid.Cell(i, 0).Tag;

                                if (detailId != dtl.Id)
                                    continue;

                                //计划开始时间
                                string PlannedBeginDateStr = flexGrid.Cell(i, endImageCol + 2).Text;
                                if (PlannedBeginDateStr != null && !PlannedBeginDateStr.Equals(""))
                                {
                                    dtl.PlannedBeginDate = DateTime.Parse(PlannedBeginDateStr);
                                }
                                else
                                {
                                    dtl.PlannedBeginDate = new DateTime(1900, 1, 1);
                                }

                                //计划结束时间
                                string PlannedEndDateStr = flexGrid.Cell(i, endImageCol + 3).Text;
                                if (PlannedEndDateStr != null && !PlannedEndDateStr.Equals(""))
                                {
                                    dtl.PlannedEndDate = DateTime.Parse(PlannedEndDateStr);
                                }
                                else
                                {
                                    dtl.PlannedEndDate = new DateTime(1900, 1, 1);
                                }

                                //计划工期
                                dtl.PlannedDuration = flexGrid.Cell(i, endImageCol + 4).IntegerValue;

                                //工期计量单位
                                dtl.ScheduleUnit = flexGrid.Cell(i, endImageCol + 5).Text;

                                //计划说明
                                dtl.TaskDescript = flexGrid.Cell(i, endImageCol + 6).Text;

                                break;
                            }
                        }


                        IList listChilds = null;

                        model.ProductionManagementSrv.EffectScrollSchdulePlanDtl(detail, listDtl.ToList(), out listChilds);

                        if (listChilds != null && listChilds.Count > 0)
                        {
                            IEnumerable<ProductionScheduleDetail> queryChilds = listChilds.OfType<ProductionScheduleDetail>();
                            for (int i = 0; i < childCount; i++)
                            {
                                detailId = flexGrid.Cell(activeRowIndex + i, 0).Tag;

                                var query = from d in queryChilds
                                            where d.Id == detailId
                                            select d;

                                if (query.Count() > 0)
                                {
                                    detail = query.ElementAt(0);
                                    if (detailHashtable.ContainsKey(detail.Id))
                                        detailHashtable[detail.Id] = detail;
                                    else
                                        detailHashtable.Add(detail.Id, detail);

                                    if (detail.State == EnumScheduleDetailState.有效)
                                    {
                                        flexGrid.Cell(activeRowIndex + i, endImageCol + 1).Text = EnumScheduleDetailState.有效.ToString();
                                        flexGrid.Cell(activeRowIndex + i, endImageCol + 1).ForeColor = Color.Blue;
                                    }
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        FlashScreen.Close();
                        MessageBox.Show("发布操作失败.\n" + ex.Message);
                        return;
                    }
                    finally
                    {
                        FlashScreen.Close();
                    }
                    //MessageBox.Show("发布成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        void flexGrid_Click(object Sender, EventArgs e)
        {
            flexGrid.AutoRedraw = false;

            if (flexGrid.ActiveCell.ImageKey == imageExpand)
            {
                flexGrid.ActiveCell.SetImage(imageCollapse);
                int activeRowIndex = flexGrid.ActiveCell.Row;

                string detailId = flexGrid.Cell(activeRowIndex, 0).Tag;
                int childs = model.ProductionManagementSrv.CountAllChildNodes(detailId);
                if (childs > 0)
                {
                    SetRowVisible(activeRowIndex + 1, activeRowIndex + childs, true);
                }

            }
            else if (flexGrid.ActiveCell.ImageKey == imageCollapse)
            {
                flexGrid.ActiveCell.SetImage(imageExpand);
                int activeRowIndex = flexGrid.ActiveCell.Row;

                string detailId = flexGrid.Cell(activeRowIndex, 0).Tag;
                int childs = model.ProductionManagementSrv.CountAllChildNodes(detailId);
                if (childs > 0)
                {
                    SetRowVisible(activeRowIndex + 1, activeRowIndex + childs, false);
                }
            }

            flexGrid.AutoRedraw = true;
        }

        private void SetRowVisible(int row1, int row2, bool value)
        {
            flexGrid.AutoRedraw = false;
            for (int i = row1; i <= row2; i++)
            {
                flexGrid.Row(i).Visible = value;
                if (value)
                {
                    for (int j = startImageCol; j <= endImageCol; j++)
                    {
                        if (flexGrid.Cell(i, j).ImageKey != null && !flexGrid.Cell(i, j).ImageKey.Equals(""))
                        {
                            flexGrid.Cell(i, j).SetImage(imageCollapse);
                            break;
                        }
                    }
                }
            }
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        void cboScheduleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string scheduleName = cboScheduleName.SelectedItem as string;
            try
            {
                FlashScreen.Show("正在加载滚动进度计划......");

                CurBillMaster = null;

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                oq.AddCriterion(Expression.Eq("ScheduleType", this.enumScheduleType));
                oq.AddCriterion(Expression.Eq("ScheduleTypeDetail", scheduleName));
                //oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                //oq.AddOrder(Order.Asc("Details.Level"));
                //oq.AddOrder(Order.Asc("Details.OrderNo"));
                IList listMaster = model.ProductionManagementSrv.ObjectQuery(typeof(ProductionScheduleMaster), oq);

                cbScheduleVersion.SelectedIndexChanged -= new EventHandler(cbScheduleVersion_SelectedIndexChanged);
                cbScheduleVersion.Items.Clear();
                if (listMaster.Count > 0)
                {
                    for (int i = 0; i < listMaster.Count; i++)
                    {
                        ProductionScheduleMaster item = listMaster[i] as ProductionScheduleMaster;

                        if (!string.IsNullOrEmpty(item.ScheduleName))
                        {
                            cbScheduleVersion.Items.Add(item.ScheduleName);
                        }
                    }

                    //默认显示执行状态的计划，如果没有则显示编辑状态的计划，否则显示第一个
                    IEnumerable<ProductionScheduleMaster> listPlanMaster = listMaster.OfType<ProductionScheduleMaster>();
                    var queryMaster = from m in listPlanMaster
                                      where m.DocState == DocumentState.InExecute
                                      select m;

                    if (queryMaster.Count() == 0)
                    {
                        queryMaster = from m in listPlanMaster
                                      where m.DocState == DocumentState.Edit
                                      select m;

                        if (queryMaster.Count() == 0)
                            queryMaster = listPlanMaster;
                    }

                    CurBillMaster = queryMaster.ElementAt(0);

                    cbScheduleVersion.Text = CurBillMaster.ScheduleName;

                    oq.Criterions.Clear();
                    oq.AddCriterion(Expression.Eq("Id", CurBillMaster.Id));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                    //oq.AddOrder(Order.Asc("Details.Level"));
                    //oq.AddOrder(Order.Asc("Details.OrderNo"));
                    IList listPlan = model.ProductionManagementSrv.ObjectQuery(typeof(ProductionScheduleMaster), oq);

                    CurBillMaster = listPlan[0] as ProductionScheduleMaster;
                }
                cbScheduleVersion.SelectedIndexChanged += new EventHandler(cbScheduleVersion_SelectedIndexChanged);

                if (CurBillMaster == null)
                {
                    //CurBillMaster = new ProductionScheduleMaster();
                    //NewMaster(CurBillMaster);
                    //CurBillMaster = model.ProductionManagementSrv.NewSchedule(CurBillMaster) as ProductionScheduleMaster;
                    //ChildRootNode = CurBillMaster.GetChildRootNode();

                    ClearView();
                    return;
                }
                else
                {
                    ChildRootNode = CurBillMaster.GetChildRootNode();
                    FreshMasterInfo(CurBillMaster);
                }

                txtPlanState.Text = ClientUtil.GetDocStateName(CurBillMaster.DocState);

                FillFlex();
                //ShowDocument();
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("查询滚动进度计划出错。\n" + ex.Message);
                return;
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        void cbScheduleVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            string planName = cboScheduleName.SelectedItem as string;
            string planVersion = cbScheduleVersion.SelectedItem as string;
            try
            {
                FlashScreen.Show("正在加载滚动进度计划......");

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                oq.AddCriterion(Expression.Eq("ScheduleType", this.enumScheduleType));
                oq.AddCriterion(Expression.Eq("ScheduleTypeDetail", planName));
                oq.AddCriterion(Expression.Eq("ScheduleName", planVersion));

                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                IList listMaster = model.ProductionManagementSrv.ObjectQuery(typeof(ProductionScheduleMaster), oq);

                if (listMaster.Count > 0)
                    CurBillMaster = listMaster[0] as ProductionScheduleMaster;
                else
                {
                    CurBillMaster = null;
                    ClearData();
                    return;
                }

                ChildRootNode = CurBillMaster.GetChildRootNode();
                FreshMasterInfo(CurBillMaster);

                txtPlanState.Text = ClientUtil.GetDocStateName(CurBillMaster.DocState);

                FillFlex();
                //ShowDocument();
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("查询滚动进度计划出错。\n" + ex.Message);
                return;
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        private void ClearData()
        {
            cboScheduleCaliber.SelectedIndex = 0;
            txtPlanState.Text = "";
            txtRemark.Text = "";
            for (int i = flexGrid.Rows - 1; i > -1; i--)
            {
                flexGrid.RemoveItem(i);
            }
        }

        private void NewMaster(ProductionScheduleMaster master)
        {
            master.ProjectId = projectInfo.Id;
            master.ProjectName = projectInfo.Name;

            master.CreateDate = model.ProductionManagementSrv.GetServerTime();
            master.HandlePerson = ConstObject.LoginPersonInfo;
            master.HandlePersonName = ConstObject.LoginPersonInfo.Name;
            master.OperOrgInfo = ConstObject.TheOperationOrg;
            master.OperOrgInfoName = ConstObject.TheOperationOrg.Name;
            master.OpgSysCode = ConstObject.TheOperationOrg.SysCode;

            master.DocState = DocumentState.Edit;
            master.ScheduleType = this.enumScheduleType;
            master.ScheduleCaliber = cboScheduleCaliber.SelectedItem.ToString();
            master.ScheduleTypeDetail = cboScheduleName.SelectedItem + "";
            master.ScheduleName = cbScheduleVersion.Text.Trim();

            //ProductionScheduleDetail detail = new ProductionScheduleDetail();
            //detail.Master = master;
            //master.Details.Add(detail);

            //detail.Level = 1;
            //detail.State = EnumScheduleDetailState.有效;
            //detail.OrderNo = 0;
        }

        /// <summary>
        /// 显示计划头信息到界面
        /// </summary>
        /// <param name="master"></param>
        private void FreshMasterInfo(ProductionScheduleMaster master)
        {
            cboScheduleCaliber.SelectedItem = master.ScheduleCaliber;
            cbScheduleVersion.Text = string.IsNullOrEmpty(master.ScheduleName) ? "" : master.ScheduleName;
            txtRemark.Text = string.IsNullOrEmpty(master.Descript) ? "" : master.Descript;
        }

        //选择GWBS
        void btnGWBS_Click(object sender, EventArgs e)
        {
            ProductionScheduleDetail parentNode = null;
            if (CurBillMaster == null)
            {
                MessageBox.Show("请选择一个计划！");
                cboScheduleName.Focus();
                return;
            }

            //if (CurBillMaster.DocState != DocumentState.Edit)
            //{
            //    MessageBox.Show("当前计划状态为“" + ClientUtil.GetDocStateName(CurBillMaster.DocState) + "”,只能修改“编辑”状态的计划！");
            //    return;
            //}

            // VSelectGWBSTree frm = new VSelectGWBSTree(new MGWBSTree());
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsSelectSingleNode = true;
            frm.IsSelectTreeNodes = true;//获取的节点是 以选择的节点为树根的一棵树

            frm.IsTreeSelect = true;
            frm.ShowDialog();
            List<TreeNode> list = frm.SelectResult;

            if (list != null && list.Count > 0)
            {
                try
                {
                    FlashScreen.Show("正在生成滚动进度计划......");

                    TreeNode selTreeNode = list[0];

                    //if (CurBillMaster != null && !string.IsNullOrEmpty(CurBillMaster.Id))
                    //{
                    //    CurBillMaster = model.ProductionManagementSrv.GetSchedulesById(CurBillMaster.Id);
                    //}

                    //判断工程任务节点是否可以添加(是否能找到父节点或)

                    //if (!CanAddChild(treeNode, CurBillMaster))
                    //    return;

                    //ProductionScheduleDetail parentNode = FindParentNode(treeNode, CurBillMaster);
                    //if (parentNode == null)
                    //{
                    //    if (CurBillMaster.Details.Count == 1 && CurBillMaster.Details.ElementAt(0).Level == 1)
                    //    {
                    //        parentNode = ChildRootNode;
                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("未找到选择任务节点的父节点，请检查！");
                    //        return;
                    //    }
                    //}

                    GWBSTree selWBS = selTreeNode.Tag as GWBSTree;

                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Master.Id", CurBillMaster.Id));
                    oq.AddCriterion(Expression.Eq("GWBSTree.Id", selWBS.Id));
                    oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
                    IList listDtl = model.ProductionManagementSrv.ObjectQuery(typeof(ProductionScheduleDetail), oq);

                    if (listDtl != null && listDtl.Count > 0)
                    {
                        //构建将更新的总滚动计划节点
                        //删除老的需要更新的节点 保存  将更新的节点保存

                        #region 获取父节点
                        ProductionScheduleDetail oCurPlan = listDtl[0] as ProductionScheduleDetail;

                        if (oCurPlan.ParentNode != null)
                        {
                            //构建将更新的总滚动计划节点
                            IList detailList = new ArrayList();
                            CreateUpdatePlanDtl(selTreeNode, oCurPlan.ParentNode, ref detailList, CurBillMaster);
                            if (detailList.Count > 0)
                            {
                                var query11 = from d in detailList.OfType<ProductionScheduleDetail>()
                                              where d.Id == "3_6RIOZa9Egf5ikMOzQ1Pa"
                                              select d;

                                var query12 = from d in detailList.OfType<ProductionScheduleDetail>()
                                              where d.ParentNode.Id == "3_6RIOZa9Egf5ikMOzQ1Pa"
                                              select d;

                                CurBillMaster = model.ProductionManagementSrv.UpdateScrollSchedule(CurBillMaster, detailList, oCurPlan);
                            }
                        }
                        else
                        {
                            MessageBox.Show("项目任务“" + selWBS.Name + "”的计划上一级节点不存在，建议先更新上一级节点！");
                            FlashScreen.Close();
                            return;
                        }
                        #endregion
                        //MessageBox.Show("项目任务“" + selWBS.Name + "”的计划已存在！");
                        //return;
                    }
                    else
                    {
                        if (selTreeNode.Parent != null)
                        {
                            GWBSTree parentWBS = selTreeNode.Parent.Tag as GWBSTree;
                            oq.Criterions.Clear();
                            oq.AddCriterion(Expression.Eq("Master.Id", CurBillMaster.Id));
                            oq.AddCriterion(Expression.Eq("GWBSTree.Id", parentWBS.Id));
                            listDtl = model.ProductionManagementSrv.ObjectQuery(typeof(ProductionScheduleDetail), oq);
                            if (listDtl != null && listDtl.Count > 0)
                            {
                                parentNode = listDtl[0] as ProductionScheduleDetail;
                            }
                            else
                            {
                                oq.Criterions.Clear();
                                oq.AddCriterion(Expression.Eq("Master.Id", CurBillMaster.Id));
                                oq.AddCriterion(Expression.Eq("Level", 1));
                                //oq.AddCriterion(Expression.IsNull("ParentNode"));

                                listDtl = model.ProductionManagementSrv.ObjectQuery(typeof(ProductionScheduleDetail), oq);
                                if (listDtl != null && listDtl.Count > 0)
                                {
                                    parentNode = listDtl[0] as ProductionScheduleDetail;
                                }
                                else
                                {
                                    FlashScreen.Close();
                                    MessageBox.Show("在计划中未找到项目任务“" + selWBS.Name + "”的父节点，添加计划失败！");
                                    return;
                                }
                            }
                        }
                        else//选择项目任务根节点
                        {
                            oq.Criterions.Clear();
                            oq.AddCriterion(Expression.Eq("Master.Id", CurBillMaster.Id));
                            oq.AddCriterion(Expression.Eq("Level", 1));
                            //oq.AddCriterion(Expression.IsNull("ParentNode"));

                            listDtl = model.ProductionManagementSrv.ObjectQuery(typeof(ProductionScheduleDetail), oq);
                            if (listDtl != null && listDtl.Count > 0)
                            {
                                parentNode = listDtl[0] as ProductionScheduleDetail;
                            }
                        }

                        CurBillMaster = SavePlanDtl(selTreeNode, parentNode, CurBillMaster);
                    }

                    var query = from d in CurBillMaster.Details
                                where d.Level == 1 && d.ParentNode == null
                                select d;

                    if (query.Count() > 0)
                        ChildRootNode = query.ElementAt(0);
                }
                catch (Exception ex)
                {
                    FlashScreen.Close();
                    MessageBox.Show("生成滚动进度计划出错。\n" + ExceptionUtil.ExceptionMessage(ex));
                    return;
                }
                finally
                {
                    FlashScreen.Close();
                }

                //AddRow(startRow, treeNode, ChildRootNode, treeNode.Level + 1);
                FillFlex();
            }
        }

        private void CreateUpdatePlanDtl(TreeNode treeGWBSNode, ProductionScheduleDetail parentPlanDtlNode, ref IList detailList, ProductionScheduleMaster master)
        {
            ProductionScheduleDetail detail = new ProductionScheduleDetail();

            detail.GWBSTree = treeGWBSNode.Tag as GWBSTree;
            if (detail.GWBSTree != null)
            {
                detail.GWBSTreeName = detail.GWBSTree.Name;
                detail.GWBSTreeSysCode = detail.GWBSTree.SysCode;
                detail.GWBSNodeType = detail.GWBSTree.CategoryNodeType;
                detail.OrderNo = (int)detail.GWBSTree.OrderNo;
            }
            if (parentPlanDtlNode != null)
            {
                detail.ParentNode = parentPlanDtlNode;
                detail.Level = parentPlanDtlNode.Level + 1;
                //detail.SysCode = parentPlanDtlNode.SysCode + parentPlanDtlNode.Id + ".";
            }

            #region 查找久的节点 并复制
            detail.Master = master;
            detail.State = EnumScheduleDetailState.编辑;
            ProductionScheduleDetail oldPlanNode = null;

            // foreach ( ProductionScheduleDetail oldPlanNode in master .Details  )
            //foreach (ProductionScheduleDetail oldPlanNode1 in master.Details)
            //{
            //    string ss = oldPlanNode1.GWBSTree.Id;
            //}
            var query = from d in master.Details
                        where d.GWBSTreeSysCode == detail.GWBSTreeSysCode
                        select d;

            if (query.Count() > 0)
            {
                for (int i = 0; i < query.Count(); i++)
                {

                    oldPlanNode = query.ElementAt(i);
                    if (string.Equals(oldPlanNode.GWBSTree.Id, detail.GWBSTree.Id))
                    {
                        detail.ActualBeginDate = oldPlanNode.ActualBeginDate;
                        detail.ActualDuration = oldPlanNode.ActualDuration;
                        detail.ActualEndDate = oldPlanNode.ActualEndDate;
                        detail.AddupFigureProgress = oldPlanNode.AddupFigureProgress;
                        detail.GwbsFullPath = oldPlanNode.GwbsFullPath;
                        // detail.GWBSNodeType = oldPlanNode.GWBSNodeType;
                        detail.IsSelected = oldPlanNode.IsSelected;
                        //detail.OrderNo = oldPlanNode.OrderNo;
                        detail.PlannedBeginDate = oldPlanNode.PlannedBeginDate;
                        detail.PlannedDuration = oldPlanNode.PlannedDuration;
                        detail.PlannedEndDate = oldPlanNode.PlannedEndDate;
                        detail.ScheduleUnit = oldPlanNode.ScheduleUnit;
                        detail.State = oldPlanNode.State;
                        detail.TaskDescript = oldPlanNode.TaskDescript;
                        detail.TaskRequirements = oldPlanNode.TaskRequirements;
                        break;
                    }
                }
            }
            #endregion
            detailList.Add(detail);
            foreach (TreeNode tn in treeGWBSNode.Nodes)
            {
                CreateUpdatePlanDtl(tn, detail, ref detailList, master);
            }
        }

        //选择子任务
        void btnSelectChildTask_Click(object sender, EventArgs e)
        {
            if (CurBillMaster == null)
            {
                MessageBox.Show("当前没有要操作的计划！");
                return;
            }
            else if (CurBillMaster.Details.Count == 0)
            {
                MessageBox.Show("当前计划尚没有计划明细！");
                return;
            }

            VSelectGWBSTree frm = new VSelectGWBSTree(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            List<TreeNode> list = frm.SelectResult;

            if (list != null && list.Count > 0)
            {
                GWBSTree selectTaskNode = list[0].Tag as GWBSTree;
                txtChildTask.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), selectTaskNode);

                ProductionScheduleDetail selectPlanDtl = null;
                if (!IsContainTaskInPlanDtl(selectTaskNode, ref selectPlanDtl))
                {
                    MessageBox.Show("计划中不包含该任务！");
                    btnSelectChildTask.Focus();
                    return;
                }

                ShowPlanDtlInFlex(selectPlanDtl);

                cbShowAllPlanDtl.Checked = false;
            }
        }

        //显示全部计划
        void cbShowAllPlanDtl_Click(object sender, EventArgs e)
        {
            if (cbShowAllPlanDtl.Checked)
            {
                txtChildTask.Text = "";

                for (int i = 0; i < flexGrid.Rows; i++)
                {
                    flexGrid.Row(i).Visible = true;
                }
            }
        }


        private void ShowPlanDtlInFlex(ProductionScheduleDetail planDtl)
        {
            var queryChild = from d in CurBillMaster.Details
                             where d.ParentNode == null || d.SysCode.IndexOf(planDtl.SysCode) > -1
                             select d;

            listDtlIds.Clear();
            foreach (ProductionScheduleDetail dtl in queryChild)
            {
                listDtlIds.Add(dtl.Id);
            }

            for (int i = 1; i < flexGrid.Rows; i++)
            {
                string dtlId = flexGrid.Cell(i, 0).Tag;
                if (listDtlIds.Contains(dtlId))
                    flexGrid.Row(i).Visible = true;
                else
                    flexGrid.Row(i).Visible = false;
            }
        }

        private bool IsContainTaskInPlanDtl(GWBSTree theGWBSNode, ref ProductionScheduleDetail selectPlanDtl)
        {
            if (CurBillMaster == null)
                return false;

            foreach (ProductionScheduleDetail dtl in CurBillMaster.Details)
            {
                if (dtl.GWBSTree != null && dtl.GWBSTree.Id == theGWBSNode.Id)
                {
                    selectPlanDtl = dtl;
                    return true;
                }
            }

            return false;
        }

        private void AddPlanDetail(TreeNode treeGWBSNode, ProductionScheduleDetail parentPlanDtlNode)
        {
            ProductionScheduleDetail detail = new ProductionScheduleDetail();
            detail.GWBSTree = treeGWBSNode.Tag as GWBSTree;
            if (detail.GWBSTree != null)
            {
                detail.GWBSTreeName = detail.GWBSTree.Name;
                detail.GWBSTreeSysCode = detail.GWBSTree.SysCode;
                detail.GWBSNodeType = detail.GWBSTree.CategoryNodeType;
                detail.OrderNo = (int)detail.GWBSTree.OrderNo;
            }
            detail.ParentNode = parentPlanDtlNode;
            detail.Level = parentPlanDtlNode.Level + 1;
            detail.State = EnumScheduleDetailState.编辑;

            //detail.SysCode = parentPlanDtlNode.SysCode + detail.Id + ".";

            detail.Master = CurBillMaster;
            CurBillMaster.Details.Add(detail);

            foreach (TreeNode tn in treeGWBSNode.Nodes)
            {
                AddPlanDetail(tn, detail);
            }
        }

        private ProductionScheduleMaster SavePlanDtl(TreeNode treeNode, ProductionScheduleDetail parentNode, ProductionScheduleMaster master)
        {
            IList detailList = new ArrayList();

            AddPlanDtl(treeNode, parentNode, ref detailList, master);

            return model.ProductionManagementSrv.SaveScrollPlanDtl(detailList);

        }

        private void AddPlanDtl(TreeNode treeGWBSNode, ProductionScheduleDetail parentPlanDtlNode, ref IList detailList, ProductionScheduleMaster master)
        {
            ProductionScheduleDetail detail = new ProductionScheduleDetail();
            detail.GWBSTree = treeGWBSNode.Tag as GWBSTree;
            if (detail.GWBSTree != null)
            {
                detail.GWBSTreeName = detail.GWBSTree.Name;
                detail.GWBSTreeSysCode = detail.GWBSTree.SysCode;
                detail.GWBSNodeType = detail.GWBSTree.CategoryNodeType;
                detail.OrderNo = (int)detail.GWBSTree.OrderNo;
            }
            detail.ParentNode = parentPlanDtlNode;
            detail.Level = parentPlanDtlNode.Level + 1;
            detail.Master = master;
            detail.State = EnumScheduleDetailState.编辑;

            detailList.Add(detail);
            foreach (TreeNode tn in treeGWBSNode.Nodes)
            {
                AddPlanDtl(tn, detail, ref detailList, master);
            }
        }

        private ProductionScheduleMaster SaveChilds(TreeNode treeNode, ProductionScheduleDetail parentNode, ProductionScheduleMaster master)
        {
            IList detailList = new ArrayList();
            try
            {
                AddChild(treeNode, parentNode, ref detailList, master);
            }
            catch (Exception ex)
            {
                model.ProductionManagementSrv.DeleteByDao(detailList);
                detailList.Clear();
                MessageBox.Show("生成进度计划明细出错,异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            if (detailList.Count > 0)
            {
                return model.ProductionManagementSrv.SaveSchedule(master, detailList);
            }
            else
            {
                return master;
            }
        }

        private void AddChild(TreeNode treeGWBSNode, ProductionScheduleDetail parentPlanDtlNode, ref IList detailList, ProductionScheduleMaster master)
        {
            ProductionScheduleDetail detail = new ProductionScheduleDetail();
            detail.GWBSTree = treeGWBSNode.Tag as GWBSTree;
            if (detail.GWBSTree != null)
            {
                detail.GWBSTreeName = detail.GWBSTree.Name;
                detail.GWBSTreeSysCode = detail.GWBSTree.SysCode;
                detail.GWBSNodeType = detail.GWBSTree.CategoryNodeType;
                detail.OrderNo = (int)detail.GWBSTree.OrderNo;
            }
            detail.ParentNode = parentPlanDtlNode;
            detail.Level = parentPlanDtlNode.Level + 1;
            detail.Master = master;
            detail.State = EnumScheduleDetailState.编辑;
            detail = model.ProductionManagementSrv.SaveOrUpdateByDao(detail) as ProductionScheduleDetail;
            detail.SysCode = parentPlanDtlNode.SysCode + detail.Id + ".";

            //detail = model.ProductionManagementSrv.SaveOrUpdateByDao(detail) as ProductionScheduleDetail;
            //master.AddDetail(detail);

            detailList.Add(detail);
            foreach (TreeNode tn in treeGWBSNode.Nodes)
            {
                AddChild(tn, detail, ref detailList, master);
            }
        }

        /// <summary>
        /// 判断工程任务节点是否可以添加
        /// </summary>
        /// <param name="treeNode"></param>
        /// <param name="master"></param>
        /// <returns></returns>
        private bool CanAddChild(TreeNode treeNode, ProductionScheduleMaster master)
        {
            GWBSTree gwbsTree = treeNode.Tag as GWBSTree;
            foreach (ProductionScheduleDetail detail in master.Details)
            {
                if (detail.GWBSTree == null)
                    continue;
                if (gwbsTree.Id == detail.GWBSTree.Id)
                {
                    MessageBox.Show("【" + gwbsTree.Name + "】已经存在，不能添加！");
                    return false;
                }
            }
            foreach (TreeNode tn in treeNode.Nodes)
            {
                return CanAddChild(tn, master);
            }
            return true;
        }

        /// <summary>
        /// 查找当前工程任务的父节点
        /// </summary>
        /// <param name="treeNode"></param>
        /// <param name="master"></param>
        /// <returns></returns>
        private ProductionScheduleDetail FindParentNode(TreeNode treeNode, ProductionScheduleMaster master)
        {
            TreeNode tn = treeNode.Parent;
            if (tn == null)
                return null;
            GWBSTree gwbsTree = tn.Tag as GWBSTree;
            if (gwbsTree == null)
                return null;

            foreach (ProductionScheduleDetail detail in master.Details)
            {
                if (detail.GWBSTree == null)
                    continue;
                if (gwbsTree.Id == detail.GWBSTree.Id)
                {
                    return detail;
                }
            }
            return null;
        }

        private void InitFlexGrid(int rows)
        {
            flexGrid.AutoRedraw = false;
            flexGrid.Column(0).Visible = false;

            flexGrid.Rows = rows;
            flexGrid.Cols = 30;//其中0列隐藏 1-19 为放置图片列 20-24为数据列

            flexGrid.SelectionMode = FlexCell.SelectionModeEnum.ByCell;
            flexGrid.ExtendLastCol = true;
            flexGrid.DisplayFocusRect = false;
            flexGrid.LockButton = true;
            flexGrid.ReadonlyFocusRect = FlexCell.FocusRectEnum.Solid;
            flexGrid.BorderStyle = FlexCell.BorderStyleEnum.FixedSingle;
            flexGrid.ScrollBars = FlexCell.ScrollBarsEnum.Both;
            flexGrid.BackColorBkg = SystemColors.Control;
            flexGrid.DefaultFont = new Font("Tahoma", 8);

            flexGrid.DisplayRowArrow = true;
            flexGrid.DisplayRowNumber = true;

            FlexCell.Range range;

            for (int i = startImageCol; i <= endImageCol; i++)
            {
                flexGrid.Column(i).TabStop = false;
                flexGrid.Column(i).Width = 20;
            }
            flexGrid.Column(endImageCol + 1).Width = 30;//状态
            flexGrid.Column(endImageCol + 4).Width = 55;//计划工期
            flexGrid.Column(endImageCol + 9).Width = 55;//实际工期
            flexGrid.Column(endImageCol + 10).Width = 130;//任务累积形象进度百分比

            range = flexGrid.Range(0, startImageCol, 0, endImageCol);
            range.Merge();
            flexGrid.Cell(0, 1).Text = "任务名称";
            flexGrid.Cell(0, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;

            // 加载图片
            flexGrid.Images.Add(Resources.ImageExpend, imageExpand);
            flexGrid.Images.Add(Resources.ImageFold, imageCollapse);

            flexGrid.Cell(0, endImageCol + 1).Text = "状态";

            flexGrid.Cell(0, endImageCol + 2).Text = "计划开始时间";
            flexGrid.Cell(0, endImageCol + 3).Text = "计划结束时间";
            flexGrid.Cell(0, endImageCol + 4).Text = "计划工期";
            flexGrid.Cell(0, endImageCol + 5).Text = "工期计量单位";
            flexGrid.Cell(0, endImageCol + 6).Text = "计划说明";

            flexGrid.Cell(0, endImageCol + 7).Text = "实际开始时间";
            flexGrid.Cell(0, endImageCol + 8).Text = "实际结束时间";
            flexGrid.Cell(0, endImageCol + 9).Text = "实际工期";
            flexGrid.Cell(0, endImageCol + 10).Text = "任务累积形象进度百分比";


            flexGrid.Column(endImageCol + 1).CellType = FlexCell.CellTypeEnum.TextBox;

            flexGrid.Column(endImageCol + 2).CellType = FlexCell.CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 3).CellType = FlexCell.CellTypeEnum.Calendar;

            flexGrid.Column(endImageCol + 4).Mask = FlexCell.MaskEnum.Digital;

            flexGrid.Column(endImageCol + 5).CellType = FlexCell.CellTypeEnum.ComboBox;

            flexGrid.Column(endImageCol + 7).CellType = FlexCell.CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 8).CellType = FlexCell.CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 9).Mask = FlexCell.MaskEnum.Digital;
            flexGrid.Column(endImageCol + 10).Mask = FlexCell.MaskEnum.Digital;

            flexGrid.Column(endImageCol + 7).Locked = true;
            flexGrid.Column(endImageCol + 8).Locked = true;
            flexGrid.Column(endImageCol + 9).Locked = true;
            flexGrid.Column(endImageCol + 10).Locked = true;

            FlexCell.ComboBox cb = flexGrid.ComboBox(endImageCol + 5);
            flexGrid.ComboBox(endImageCol + 5).Locked = true;
            try
            {
                IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.PLAN_ScheduleUnit);
                if (list != null && list.Count > 0)
                {
                    foreach (BasicDataOptr bd in list)
                    {
                        cb.Items.Add(bd.BasicName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取工期计量单位出错。");
            }

            // Refresh
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        private void FillFlex()
        {
            flexGrid.AutoRedraw = false;
            flexGrid.Rows = 1;
            flexGrid.Column(endImageCol + 5).Locked = true;

            detailHashtable.Clear();


            IList list = null;
            if (!string.IsNullOrEmpty(CurBillMaster.Id))
                list = model.ProductionManagementSrv.GetChilds(CurBillMaster);

            if (list != null && list.Count > 0)
            {
                flexGrid.Rows = list.Count + 1;
                int rowIndex = 1;
                listDtlIds.Clear();
                foreach (ProductionScheduleDetail detail in list)
                {
                    //if (detail.State == EnumScheduleDetailState.失效)
                    //{
                    //    flexGrid.Rows = flexGrid.Rows - 1;
                    //    continue;
                    //}

                    flexGrid.Cell(rowIndex, 0).Tag = detail.Id;
                    listDtlIds.Add(detail.Id);
                    detailHashtable.Add(detail.Id, detail);

                    flexGrid.Cell(rowIndex, detail.Level).SetImage(imageCollapse);
                    FlexCell.Range rangeTemp = flexGrid.Range(rowIndex, detail.Level + 1, rowIndex, endImageCol);
                    rangeTemp.Merge();

                    //rangeTemp.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                    flexGrid.Cell(rowIndex, detail.Level + 1).Text = (detail.GWBSTreeName == null) ? "进度计划" : detail.GWBSTreeName;

                    if (detail.State == EnumScheduleDetailState.失效)
                        flexGrid.Cell(rowIndex, endImageCol + 1).ForeColor = Color.Red;
                    else if (detail.State == EnumScheduleDetailState.有效)
                        flexGrid.Cell(rowIndex, endImageCol + 1).ForeColor = Color.Blue;

                    flexGrid.Cell(rowIndex, endImageCol + 1).Text = detail.State.ToString();

                    //计划开始时间
                    if (detail.PlannedBeginDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 2).Text = ""; //"计划开始时间";//20
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 2).Text = detail.PlannedBeginDate.ToShortDateString(); //"计划开始时间";//20
                    }
                    //计划结束时间
                    if (detail.PlannedEndDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 3).Text = ""; //"计划结束时间";//21
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 3).Text = detail.PlannedEndDate.ToShortDateString();
                    }

                    //计划工期
                    // flexGrid.Cell(rowIndex, endImageCol + 4).Text = detail.PlannedDuration == 0 ? "" : detail.PlannedDuration.ToString(); //"计划工期";//23
                    if (detail.PlannedBeginDate != (new DateTime(1900, 1, 1)) && detail.PlannedEndDate != (new DateTime(1900, 1, 1)) && detail.PlannedBeginDate <= detail.PlannedEndDate)
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 4).Text = ((detail.PlannedEndDate - detail.PlannedBeginDate).Days + 1).ToString();
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 4).Text = "";
                    }

                    //工期计量单位                    
                    //flexGrid.Cell(rowIndex, endImageCol + 5).Text =detail.ScheduleUnit; //"";//22
                    flexGrid.Cell(rowIndex, endImageCol + 5).Text = "天";

                    //计划说明
                    flexGrid.Cell(rowIndex, endImageCol + 6).Text = detail.TaskDescript; //"计划说明";//27


                    //实际开始时间
                    if (detail.ActualBeginDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 7).Text = "";
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 7).Text = detail.ActualBeginDate.ToShortDateString();
                    }
                    //实际结束时间
                    if (detail.ActualEndDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 8).Text = "";
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 8).Text = detail.ActualEndDate.ToShortDateString();
                    }
                    //实际工期
                    flexGrid.Cell(rowIndex, endImageCol + 9).Text = detail.ActualDuration == 0 ? "" : detail.ActualDuration.ToString();

                    //任务累积形象进度百分比
                    flexGrid.Cell(rowIndex, endImageCol + 10).Text = StaticMethod.DecimelTrimEnd0(detail.AddupFigureProgress);

                    rowIndex = rowIndex + 1;
                }
            }
            //1-19列的背景色
            FlexCell.Range range = flexGrid.Range(1, startImageCol, flexGrid.Rows - 1, endImageCol);
            if (range != null)
            {
                range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                range.BackColor = SystemColors.Control;
            }

            range = flexGrid.Range(1, endImageCol + 1, flexGrid.Rows - 1, endImageCol + 1);
            if (range != null)
            {
                range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                range.BackColor = SystemColors.Control;
            }

            //设置计划实际信息列的背景色
            range = flexGrid.Range(1, endImageCol + 7, flexGrid.Rows - 1, endImageCol + 10);
            if (range != null)
            {
                range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                range.BackColor = SystemColors.Control;
            }

            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        private IList GetShowDetails()
        {
            IList listDtl = new ArrayList();
            var queryDtl = from d in CurBillMaster.Details
                           where d.Level == 1
                           orderby d.OrderNo ascending
                           select d;


            foreach (ProductionScheduleDetail dtl in queryDtl)
            {
                listDtl.Add(dtl);
                GetShowDetailsByParentDtl(listDtl, dtl);
            }

            return listDtl;
        }

        private void GetShowDetailsByParentDtl(IList listDtl, ProductionScheduleDetail parentDtl)
        {
            var queryDtl = from d in CurBillMaster.Details
                           where d.ParentNode == parentDtl
                           orderby d.OrderNo ascending
                           orderby d.Level ascending
                           select d;

            foreach (ProductionScheduleDetail dtl in queryDtl)
            {
                listDtl.Add(dtl);
                GetShowDetailsByParentDtl(listDtl, dtl);
            }
        }

        private int NodesCount(TreeNode treeNode)
        {
            int result = 0;
            if (treeNode != null)
            {
                result = treeNode.Nodes.Count;
                foreach (TreeNode tn in treeNode.Nodes)
                {
                    result += NodesCount(tn);
                }
            }
            return result;
        }

    }
}
