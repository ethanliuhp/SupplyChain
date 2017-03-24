using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.IO;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.ProductionManagement.PenaltyDeductionManagement.Domain;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using System.Diagnostics;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using IRPServiceModel.Domain.Document;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using Application.Business.Erp.SupplyChain.Client.FileUpload;
using IRPServiceModel.Basic;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionRecordMng
{
    public partial class VInspectionRecord : TBasicDataView
    {
        public MProductionMng model = new MProductionMng();
        private InspectionRecord inspectionRecord;
        CurrentProjectInfo projectInfo = null;
        string userName = string.Empty;
        string jobId = string.Empty;
        string fileObjectType = string.Empty;
        private ProObjectRelaDocument oprDocument = null;
        private WeekScheduleMaster  weekMaster;
        private WeekScheduleDetail weekDetail;

        public VInspectionRecord()
        {
            InitializeComponent();
            InitData();
            InitEvent();
        }

        private void InitData()
        {
            txtCreateDate.Text = "";
            txtCreateDate1.Text = "";
            cbCheckConclusion.Items.AddRange(new object[] { "通过", "不通过" });
            cbWBSCheckRequir.Items.Clear();
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.WBS_CheckRequire);
            foreach (BasicDataOptr b in list)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                li.Text = b.BasicName;
                li.Value = b.BasicCode;
                cbWBSCheckRequir.Items.Add(li);
            }
            RefreshControls(MainViewState.Browser);
            txtCheckPerson.Value = ConstObject.LoginPersonInfo.Name;
            dtpCheckDate.Value = ConstObject.TheLogin.LoginDate;
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
            List<string> listParams = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetUploadFileParamsByMBP_IRP();
            fileObjectType = listParams[0];
            btnStates(0);
        }
        
        private void InitEvent()
        {
            //周进度计划
            this.btnSearchWeek.Click += new EventHandler(btnSearchWeek_Click);
            this.dgMaster.CellClick +=new DataGridViewCellEventHandler(dgMaster_CellClick);
            dgWeekDetail.CellClick += new DataGridViewCellEventHandler(dgWeekDetail_CellClick);
            this.btnSelBearByQuery.Click += new EventHandler(btnSelBearByQuery_Click);
            this.tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
            //检查记录
            btnAddInspectionRecord.Click += new EventHandler(btnAddInspectionRecord_Click);
            btnSaveInspectionRecord.Click += new EventHandler(btnSaveInspectionRecord_Click);
            this.btnSubmit.Click +=new EventHandler(btnSubmit_Click);
            this.btnSelect.Click += new EventHandler(btnSelect_Click);
            this.btnDelete.Click +=new EventHandler(btnDelete_Click);
            dgDetail.CellClick += new DataGridViewCellEventHandler(dgDetail_CellClick);
            cbCheckConclusion.SelectedIndexChanged += new EventHandler(cbCheckConclusion_SelectedIndexChanged);
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
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", inspectionRecord.Id));
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
            if (inspectionRecord.Id == null)
            {
                if (MessageBox.Show("当前业务对象还没保存，是否保存！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    //if (!ValidView()) return;
                    try
                    {
                        //if (!ViewToModel()) return;
                        inspectionRecord = model.ProductionManagementSrv.SaveInspectialRecordMaster(inspectionRecord, weekDetail);
                        this.ViewCaption = ViewName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(ex));
                    }
                }
            }
            if (inspectionRecord.Id != null)
            {
                VDocumentPublicUpload frm = new VDocumentPublicUpload(inspectionRecord.Id);
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
        void btnSelect_Click(object sender, EventArgs e)
        {
            DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
            VContractExcuteSelector vmros = new VContractExcuteSelector();
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject engineerMaster = list[0] as SubContractProject;
            txtSuppiler.Text = engineerMaster.BearerOrgName;
            txtSuppiler.Tag = engineerMaster;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnDelete_Click(object sender, EventArgs e)
        {
            bool flag = false;
            if (dgDetail.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    for (int i = 0; i < dgDetail.SelectedRows.Count; i++)
                    {
                        if ((dgDetail.SelectedRows[i].Tag as InspectionRecord).Id != null)
                        {
                            InspectionRecord record = dgDetail.SelectedRows[i].Tag as InspectionRecord;
                            if (record.CreatePerson != null)
                            {
                                if (record.CreatePerson.Id != ConstObject.LoginPersonInfo.Id)
                                {
                                    MessageBox.Show("非登录人检查的信息，不可删除！");
                                    return;
                                }

                                if (record.DocState == DocumentState.InAudit || record.DocState == DocumentState.InExecute)
                                {
                                    MessageBox.Show("信息已经提交，不可删除！");
                                    return;
                                }
                                //删除信息
                                if (!model.ProductionManagementSrv.DeleteInspectionRecord(dgDetail.SelectedRows[i].Tag as InspectionRecord)) return;
                            }
                        }
                        else
                        {
                            dgDetail.Rows.Remove(dgDetail.SelectedRows[i]);
                        }
                        flag = true;
                    }
                    if (flag)
                    {
                        MessageBox.Show("删除成功！");
                        FilldgDtail();
                    }
                }
            }
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSubmit_Click(object sender,EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.Selected)
                {
                    if (var.Cells[colCheckPerson.Name].Value == null)
                    {
                        if (!SetMessage()) return;
                        SaveInspectionRecord("提交");
                        FilldgDtail();
                    }
                    else
                    {
                        InspectionRecord record = dgDetail.CurrentRow.Tag as InspectionRecord;
                        if (record.Id != null)
                        {
                            if (record.DocState == DocumentState.InExecute)
                            {
                                MessageBox.Show("信息已提交！");
                                return;
                            }
                            record.DocState = DocumentState.InExecute;
                            WeekScheduleDetail weekDetail = dgWeekDetail.CurrentRow.Tag as WeekScheduleDetail;
                            record = model.ProductionManagementSrv.SaveInspectialRecordMaster(record, weekDetail);
                            MessageBox.Show("提交成功！");
                            RefreshControls(MainViewState.Browser);
                            FilldgDtail();
                        }
                    }
                }
            }
        }
        //检查结论下拉框
        void cbCheckConclusion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCheckConclusion.SelectedItem == "通过")
            {
                radioButton1.Checked = true;
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
            }
            if (cbCheckConclusion.SelectedItem == "不通过")
            {
                radioButton2.Checked = true;
                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
            }
        }

        void btnSelBearByQuery_Click(object sender, EventArgs e)
        {
            VContractExcuteSelector vces = new VContractExcuteSelector();
            vces.StartPosition = FormStartPosition.CenterScreen;
            vces.ShowDialog();
            if (vces.Result != null && vces.Result.Count > 0)
            {
                SubContractProject scp = vces.Result[0] as SubContractProject;

                txtTaskBearByQuery.Text = scp.BearerOrgName;
                txtTaskBearByQuery.Tag = scp;
            }
        }

        void btnSearchWeek_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", StaticMethod.GetProjectInfo().Id));
            if (txtCreateDate.IsHasValue)
                oq.AddCriterion(Expression.Ge("CreateTime", txtCreateDate.Value.Date));
            if (txtCreateDate1.IsHasValue)
                oq.AddCriterion(Expression.Lt("CreateTime", txtCreateDate1.Value.AddDays(1).Date));
            if (txtTaskBearByQuery.Text.Trim() != "" && txtTaskBearByQuery.Tag != null)
            {
                SubContractProject project = txtTaskBearByQuery.Tag as SubContractProject;
                oq.AddCriterion(Expression.Eq("SubContractProject.Id", project.Id));
            }
            if (txtProjectName.Text.Trim() != "")
            {
                oq.AddCriterion(Expression.Like("GWBSTreeName", txtProjectName.Text.Trim(), MatchMode.Anywhere));
            }
            if (txtMainWork.Text.Trim() != "")
            {
                oq.AddCriterion(Expression.Like("MainTaskContent", txtMainWork.Text.Trim(), MatchMode.Anywhere));
            }
            oq.AddCriterion(Expression.Eq("Master.ExecScheduleType", EnumExecScheduleType.周进度计划));
            oq.AddCriterion(Expression.Eq("Master.SummaryStatus", EnumSummaryStatus.汇总生成));

            if (txtWeekPlanName.Text.Trim() != "")
                oq.AddCriterion(Expression.Like("Master.PlanName", txtWeekPlanName.Text.Trim(), MatchMode.Anywhere));

            if (txtCreatePerson.Text.Trim() != "" && txtCreatePerson.Result != null && txtCreatePerson.Result.Count > 0)
            {
                PersonInfo responsiblePerson = txtCreatePerson.Result[0] as PersonInfo;
                oq.AddCriterion(Expression.Eq("Master.HandlePerson.Id", responsiblePerson.Id));
            }
            IList listDtl = model.ProductionManagementSrv.ObjectQuery(typeof(WeekScheduleDetail), oq);
            List<WeekScheduleMaster> list = new List<WeekScheduleMaster>();
            if (listDtl.Count > 0)
            {
                IEnumerable<WeekScheduleDetail> queryDtl = listDtl.OfType<WeekScheduleDetail>();

                var queryMaster = from d in queryDtl
                                  group d by new { d.Master.Id }
                                      into g
                                      select new
                                      {
                                          g.Key.Id
                                      };
                foreach (var parent in queryMaster)
                {
                    var query = from d in queryDtl
                                where d.Master.Id == parent.Id
                                select d;
                    list.Add(query.ElementAt(0).Master);
                }
                var query11 = from m in list
                              orderby m.Code ascending
                              select m;

                list = query11.ToList();
            }
            try
            {
                FillMaster(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询周计划出错。\n" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        /// <summary>
        /// 显示主表
        /// </summary>
        /// <param name="masterList"></param>
        private void FillMaster(IList masterList)
        {
            dgMaster.Rows.Clear();
            dgWeekDetail.Rows.Clear();
            dgDetail.Rows.Clear();
            if (masterList == null || masterList.Count == 0)
                return;
            foreach (WeekScheduleMaster master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                DataGridViewRow dr = dgMaster.Rows[rowIndex];
                dr.Tag = master;
                dr.Cells[colWeekPlanMaster.Name].Value = master.PlanName;
                dr.Cells[colCreateDateMaster.Name].Value = master.CreateDate.ToShortDateString();
                dr.Cells[colPlanStartDate.Name].Value = master.PlannedBeginDate.ToShortDateString();
                dr.Cells[colPlanEndDate.Name].Value = master.PlannedEndDate.ToShortDateString();
                dr.Cells[colCreatePersonMaster.Name].Value = master.HandlePersonName;
                dr.Cells[colPlanDescript.Name].Value = master.Descript;
            }
            dgMaster.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            if (dgMaster.Rows.Count > 0)
            {
                dgMaster.CurrentCell = dgMaster.Rows[0].Cells[0];
                weekMaster = dgMaster.CurrentRow.Tag as WeekScheduleMaster;
                dgMaster_CellClick(dgMaster, new DataGridViewCellEventArgs(0, 0));
            }
        }

        void dgMaster_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgDetail.Rows.Count != 0)
            {
                if (!saveInspection())
                {
                    //修改时还原选中的修改行
                    foreach (DataGridViewRow row in dgMaster.Rows)
                    {
                        WeekScheduleMaster temp = row.Tag as WeekScheduleMaster;
                        if (temp.Id == weekMaster.Id)
                        {
                            dgMaster.CurrentCell = row.Cells[0];
                            break;
                        }
                    }
                    return;
                }
            }
            DataGridViewRow dr = dgMaster.CurrentRow;
            if (dr == null) return;
            weekMaster = dr.Tag as WeekScheduleMaster;
            if (weekMaster == null) return;
            FillDgDetail(weekMaster);
        }

        private void FillDgDetail(WeekScheduleMaster master)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", master.Id));
            oq.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("SubContractProject", NHibernate.FetchMode.Eager);
            IList listDtl = model.ProductionManagementSrv.ObjectQuery(typeof(WeekScheduleDetail), oq);
            dgWeekDetail.Rows.Clear();
            foreach (WeekScheduleDetail dtl in listDtl)
            {
                int i = this.dgWeekDetail.Rows.Add();
                DataGridViewRow row = dgWeekDetail.Rows[i];
                if (dtl.GWBSTree != null)
                {
                    row.Cells[colProjectDetail.Name].Value = dtl.GWBSTree.Name;
                    row.Cells[colProjectDetail.Name].Tag = dtl.GWBSTree;
                }
                row.Cells[colMainWork.Name].Value = dtl.MainTaskContent;
                if (dtl.SubContractProject != null)
                {
                    row.Cells[colBear.Name].Value = dtl.SubContractProject.BearerOrgName;
                    row.Cells[colBear.Name].Tag = dtl.SubContractProject.BearerOrg;
                }
                row.Cells[colStartDate.Name].Value = dtl.PlannedBeginDate.ToShortDateString();
                row.Cells[colEndDate.Name].Value = dtl.PlannedEndDate.ToShortDateString();
                row.Cells[colPlanTime.Name].Value = dtl.PlannedDuration;
                row.Cells[colDescription.Name].Value = dtl.Descript;
                row.Tag = dtl;
            }
            if (dgWeekDetail.Rows.Count > 0)
            {
                dgWeekDetail.CurrentCell = dgWeekDetail.Rows[0].Cells[0];
                weekDetail = dgWeekDetail.CurrentRow.Tag as WeekScheduleDetail;
                dgWeekDetail_CellClick(dgMaster, new DataGridViewCellEventArgs(0, 0));
            }
        }
        bool saveInspection()
        {
            if (dgDetail.Rows.Count > 0)
            {
                if ((dgDetail.Rows[dgDetail.Rows.Count - 1].Tag != null))
                {
                    if ((dgDetail.Rows[dgDetail.Rows.Count - 1].Tag as InspectionRecord).Id == null)
                    {
                        if ((dgDetail.CurrentRow.Tag as InspectionRecord).Id != null)
                        {
                            if (MessageBox.Show("记录未保存，是否要保存记录？", "保存记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                            {
                                if (!SetMessage())
                                {
                                    return false;
                                }
                                try
                                {
                                    this.SaveInspectionRecord("保存");
                                    FilldgDtail();
                                    return true;
                                }
                                catch (Exception err)
                                {
                                    MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(err));
                                    return false;
                                }
                            }
                            else
                            {
                                dgDetail.Rows.Remove(dgDetail.Rows[dgDetail.Rows.Count - 1]);
                                return true;
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }

        void dgWeekDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgDetail.Rows.Count != 0)
            {
                if (!saveInspection())
                {
                    //修改时还原选中的修改行
                    foreach (DataGridViewRow row in dgWeekDetail.Rows)
                    {
                        WeekScheduleDetail temp = row.Tag as WeekScheduleDetail;
                        if (temp.Id == weekDetail.Id)
                        {
                            dgWeekDetail.CurrentCell = row.Cells[0];
                            break;
                        }
                    }
                    return;
                }
            }
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            Clear();
            FilldgDtail();
        }

        void FilldgDtail()
        {
            dgDetail.Rows.Clear();
            weekDetail = dgWeekDetail.CurrentRow.Tag as WeekScheduleDetail;
            if (weekDetail != null)
            {
                txtCheckPerson.Value = ConstObject.LoginPersonInfo.Name;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("WeekScheduleDetail.Id", weekDetail.Id));
                IList temp_list = model.ProductionManagementSrv.GetInspectionRecord(oq);
                if (temp_list.Count > 0)
                {
                    btnStates(1);
                    foreach (InspectionRecord Record in temp_list)
                    {
                        int rowIndex = dgDetail.Rows.Add();
                        dgDetail[colCorrectiveSign.Name, rowIndex].Value = Record.CorrectiveSign;
                        dgDetail[colCheckSpecial.Name, rowIndex].Value = Record.InspectionSpecial;
                        dgDetail[colCheckConclusion.Name, rowIndex].Value = Record.InspectionConclusion;
                        dgDetail[colContent.Name, rowIndex].Value = Record.InspectionStatus;
                        dgDetail[colCheckPerson.Name, rowIndex].Value = Record.CreatePersonName;
                        dgDetail[colCheckPerson.Name, rowIndex].Tag = Record.CreatePerson;
                        dgDetail[colBearName.Name, rowIndex].Value = Record.BearTeamName;
                        dgDetail[colBearName.Name, rowIndex].Tag = Record.BearTeam;
                        dgDetail[colInspectionDate.Name, rowIndex].Value = Record.CreateDate.ToShortDateString();
                        if (Record.Id != null)
                        {
                            if (Record.DocState == DocumentState.InExecute || Record.DocState == DocumentState.InAudit)
                            {
                                dgDetail[colRecordState.Name, rowIndex].Value = "有效";
                            }
                            else
                            {
                                dgDetail[colRecordState.Name, rowIndex].Value = "编辑";
                            }
                        }
                        else
                        {
                            dgDetail[colRecordState.Name, rowIndex].Value = "编辑";
                        }
                        string strDeductionSign = ClientUtil.ToString(Record.DeductionSign);
                        string strCorrectiveSign = ClientUtil.ToString(Record.CorrectiveSign);
                        if (strCorrectiveSign.Equals("0"))
                        {
                            dgDetail[colCorrectiveSign.Name, rowIndex].Value = "不需整改";
                        }
                        if (strCorrectiveSign.Equals("1"))
                        {
                            dgDetail[colCorrectiveSign.Name, rowIndex].Value = "需要整改，未进行处理";
                        }
                        if (strCorrectiveSign.Equals("2"))
                        {
                            dgDetail[colCorrectiveSign.Name, rowIndex].Value = "需要整改，已进行处理";
                        }
                        txtWeekScheduleDetail.Tag = null;
                        txtWeekScheduleDetail.Tag = Record.WeekScheduleDetail;
                        dgDetail.Rows[rowIndex].Tag = Record;
                    }

                }
                else
                {
                    btnStates(0);
                    txtWeekScheduleDetail.Tag = null;
                    txtWeekScheduleDetail.Tag = weekDetail;
                    this.txtCheckPerson.Result.Clear();
                    this.txtCheckPerson.Result.Add(ConstObject.LoginPersonInfo);
                    this.txtCheckPerson.Value = ConstObject.LoginPersonInfo.Name;
                }
                if (dgDetail.Rows.Count > 0)
                {
                    dgDetail.CurrentCell = dgDetail.Rows[0].Cells[0];
                    inspectionRecord = dgDetail.CurrentRow.Tag as InspectionRecord;
                    dgDetail_CellClick(dgDetail, new DataGridViewCellEventArgs(0, 0));
                }
                else
                {
                    Clear();
                    RefreshControls(MainViewState.Browser);
                }
            }
        }

        #region 检查记录操作事件及方法
        void dgDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnStates(1);
            if (!saveInspection())
            {
                //修改时还原选中的修改行
                foreach (DataGridViewRow row in dgDetail.Rows)
                {
                    InspectionRecord temp = row.Tag as InspectionRecord;
                    if (temp.Id == inspectionRecord.Id)
                    {
                        dgDetail.CurrentCell = row.Cells[0];
                        break;
                    }
                }
                return;
            }
            if (dgDetail.Rows.Count > 0)
            {
                if ((dgDetail.CurrentRow.Tag as InspectionRecord).Id != null)
                {
                    inspectionRecord = dgDetail.CurrentRow.Tag as InspectionRecord;
                    inspectionRecord = model.ProductionManagementSrv.GetInspectionRecordById(inspectionRecord.Id);
                    this.btnSaveInspectionRecord.Enabled = true;
                    this.btnSubmit.Enabled = true;
                    if (inspectionRecord.CreatePerson.Id != ConstObject.LoginPersonInfo.Id)
                    {
                        this.btnSaveInspectionRecord.Enabled = false;
                        this.btnSubmit.Enabled = false;
                        this.EditInspectionRecord();
                    }
                    else
                    {
                        if (tabControl1.SelectedTab.Name.Equals(检查记录明细信息.Name))
                        {
                            if (dgDetail.Rows.Count > 0 && dgDetail.CurrentRow != null)
                            {
                                this.EditInspectionRecord();
                            }
                        }

                        else if (tabControl1.SelectedTab.Name.Equals(相关附件.Name))
                        {
                            if (dgDetail.Rows.Count > 0 && dgDetail.CurrentRow != null)
                            {
                                dgDocumentMast.Rows.Clear();
                                dgDocumentDetail.Rows.Clear();
                                if (dgDetail.CurrentRow.Tag != null)
                                {
                                    InspectionRecord insdetail = dgDetail.CurrentRow.Tag as InspectionRecord;
                                    if (insdetail.Id == null)
                                    {
                                        btnStates(0);
                                    }
                                    else
                                    {
                                        btnStates(1);
                                    }
                                    dgDocumentMast_CellClick(dgDocumentMast, new DataGridViewCellEventArgs(2, dgDocumentMast.SelectedRows[0].Index));
                                    
                                }
                            }
                        }
                    }
                }
                else
                {
                    Clear();
                }
            }
        }

        bool SetMessage()
        {
            if (cbWBSCheckRequir.Text == "")
            {
                MessageBox.Show("请选择检查专业！");
                return false;
            }
            if (txtCheckPerson.Text == "")
            {
                MessageBox.Show("请选择检查人！");
                return false;
            }
            if (cbCheckConclusion.Text == "")
            {
                MessageBox.Show("请选择检查结论！");
                return false;
            }
            return true;
        }

        void btnSaveInspectionRecord_Click(object sender, EventArgs e)
        {
            //校验数据
            if(!SetMessage()) return;
            try
            {
                this.SaveInspectionRecord("保存");
                FilldgDtail();
            }
            catch (Exception err)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(err));
                return;
            }
        }

        //新增检查记录(根据工程项目任务新增:检查专业不同)
        void btnAddInspectionRecord_Click(object sender, EventArgs e)
        {
            btnStates(0);
            if (dgWeekDetail.Rows.Count <= 0)
            {
                MessageBox.Show("至少选择一条周计划明细！");
                return;
            }
            InspectionRecord red = new InspectionRecord();
            red.DocState = DocumentState.Edit;
            red.CreateDate = ConstObject.LoginDate;
            int i = dgDetail.Rows.Add();
            dgDetail.ClearSelection();
            dgDetail.CurrentCell = dgDetail.Rows[i].Cells[colCheckPerson.Name];
            //dgDetail.Rows[i].Selected = true;
            txtWeekScheduleDetail.Tag = null;
            txtWeekScheduleDetail.Tag = dgWeekDetail.CurrentRow.Tag as WeekScheduleDetail;
            dgDetail.Rows[i].Tag = red;
            txtInspectionRecord.Tag = red;
            inspectionRecord = red;
            txtSuppiler.Text = ClientUtil.ToString((dgWeekDetail.CurrentRow.Tag as WeekScheduleDetail).SupplierName);
            txtSuppiler.Tag = (dgWeekDetail.CurrentRow.Tag as WeekScheduleDetail).SubContractProject;
            txtCheckPerson.Value = ConstObject.LoginPersonInfo.Name;
            txtCheckPerson.Result.Add(ConstObject.LoginPersonInfo);
            dtpCheckDate.Value = ConstObject.LoginDate;
            txtCheckStatus.Text = "";
            cbCheckConclusion.Text = "";
            cbWBSCheckRequir.Text = "";
            RefreshControls(MainViewState.Modify);
        }

        /// <summary>
        /// 在编辑框显示当前检查记录详细信息
        /// </summary>
        private void EditInspectionRecord()
        {
            if (tabControl1.SelectedTab.Name.Equals(检查记录明细信息.Name))
            {
                if (dgDetail.CurrentRow != null)
                {
                    DataGridViewRow theCurrentRow = dgDetail.CurrentRow;
                    if (theCurrentRow.Tag != null)
                    {
                        InspectionRecord master = theCurrentRow.Tag as InspectionRecord;
                        txtInspectionRecord.Tag = master;
                        if (master.WeekScheduleDetail != null)
                        {
                            txtWeekScheduleDetail.Tag = null;
                            txtWeekScheduleDetail.Tag = master.WeekScheduleDetail;
                        }
                        if (master.CreatePersonName != null)
                        {
                            txtCheckPerson.Result.Clear();
                            txtCheckPerson.Result.Add(master.CreatePerson);
                            txtCheckPerson.Value = master.CreatePersonName;
                        }
                        else
                        {
                            this.txtCheckPerson.Result.Clear();
                            this.txtCheckPerson.Result.Add(ConstObject.LoginPersonInfo);
                            this.txtCheckPerson.Value = ConstObject.LoginPersonInfo.Name;
                        }
                        if (master.BearTeam != null)
                        {
                            txtSuppiler.Tag = master.BearTeam;
                            txtSuppiler.Text = master.BearTeamName;
                        }
                        else
                        {
                            txtSuppiler.Text = "";
                            txtSuppiler.Tag = null;
                        }
                        cbWBSCheckRequir.Text = master.InspectionSpecial;
                        cbCheckConclusion.Text = master.InspectionConclusion;
                        if (master.CreateDate <= Convert.ToDateTime("1900-1-1 00:00:00"))
                        {
                            dtpCheckDate.Value = DateTime.Now;
                        }
                        else
                        {
                            dtpCheckDate.Value = master.CreateDate;
                        }
                        string strDeductionSign = ClientUtil.ToString(master.DeductionSign);
                        string strCorrectiveSign = ClientUtil.ToString(master.CorrectiveSign);
                        if (strCorrectiveSign.Equals("1") || strCorrectiveSign.Equals("2"))
                        {
                            radioButton2.Checked = true;
                        }
                        else
                        {
                            radioButton1.Checked = true;
                        }
                        txtCheckStatus.Text = master.InspectionStatus;
                        if (master.Id != null)
                        {
                            if ((master.DocState == DocumentState.InAudit || master.DocState == DocumentState.InExecute) || master.HandlePerson.Id != ConstObject.LoginPersonInfo.Id)
                            {
                                //信息为提交状态，信息不可编辑
                                RefreshControls(MainViewState.Browser);
                            }
                            else
                            {
                                if (inspectionRecord.CreatePerson.Id == ConstObject.LoginPersonInfo.Id)
                                {
                                    RefreshControls(MainViewState.Modify);
                                }
                                else
                                {
                                    RefreshControls(MainViewState.Browser);
                                }
                            }
                        }
                        else
                        {
                            RefreshControls(MainViewState.Modify);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 保存更新检查记录
        /// </summary>
        /// <returns></returns>
        private void SaveInspectionRecord(string strName)
        {
            try
            {
                InspectionRecord master = txtInspectionRecord.Tag as InspectionRecord;
                if (master.Id == null)
                    master = new InspectionRecord();
                master.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                master.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;
                master.OpgSysCode = ConstObject.TheOperationOrg.SysCode;
                master.HandOrgLevel = ConstObject.TheOperationOrg.Level;
                master.HandlePerson = ConstObject.LoginPersonInfo;
                master.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                master.ProjectId = projectInfo.Id;
                master.ProjectName = projectInfo.Name;
                master.InspectType = 1;//1代表日常检查，2代表质量验收
                if (strName == "保存")
                {
                    master.DocState = DocumentState.Edit;
                }
                else
                {
                    master.DocState = DocumentState.InExecute;
                }
                //检查人
                if (txtCheckPerson.Result.Count > 0)
                {
                    master.CreatePerson = txtCheckPerson.Result[0] as PersonInfo;
                    master.CreatePersonName = txtCheckPerson.Text;
                }
                master.InspectionSpecial = cbWBSCheckRequir.Text;//检查专业
                System.Web.UI.WebControls.ListItem li = cbWBSCheckRequir.SelectedItem as System.Web.UI.WebControls.ListItem;

                master.GWBSTree = dgWeekDetail.CurrentRow.Cells[colProjectDetail.Name].Tag as GWBSTree;
                master.GWBSTreeName = ClientUtil.ToString(dgWeekDetail.CurrentRow.Cells[colProjectDetail.Name].Value);
                if (txtSuppiler.Text != "")
                {
                    master.BearTeam = txtSuppiler.Tag as SubContractProject;
                    master.BearTeamName = ClientUtil.ToString(txtSuppiler.Text);
                }
                else
                {
                    master.BearTeam = null;
                    master.BearTeamName = "";
                }
                master.InspectionSpecialCode = li.Value;
                master.InspectionConclusion = cbCheckConclusion.Text;//检查结论
                master.InspectionStatus = txtCheckStatus.Text;//检查内容
                master.CreateDate = dtpCheckDate.Value.Date;
                master.WeekScheduleDetail = txtWeekScheduleDetail.Tag as WeekScheduleDetail;
                if (master.Id != null)
                {
                    if (master.DeductionSign != 2)
                    {
                        if (radioButton1.Checked)
                        {
                            master.CorrectiveSign = 0;
                        }
                        else
                        {
                            master.CorrectiveSign = 1;
                        }
                    }
                }
                else
                {
                    if (radioButton1.Checked)
                    {
                        master.CorrectiveSign = 0;
                    }
                    else
                    {
                        master.CorrectiveSign = 1;
                    }
                }
                //保存日常检查记录
                WeekScheduleDetail weekDetail = dgWeekDetail.CurrentRow.Tag as WeekScheduleDetail;
                InspectionRecord record = model.ProductionManagementSrv.SaveInspectialRecordMaster(master, weekDetail);
                this.dgDetail.Rows[dgDetail.Rows.Count - 1].Tag = record;
                MessageBox.Show(strName +"成功！");
                RefreshControls(MainViewState.Browser);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存数据错误" + ex.ToString());
                return;
            }
        }
        private void UpdateCurrentRow(InspectionRecord obj)
        {
            txtInspectionRecord.Tag = null;
            txtInspectionRecord.Tag = obj;
            DataGridViewRow theCurrentRow = dgDetail.CurrentRow;
            theCurrentRow.Cells[colCheckSpecial.Name].Value = obj.InspectionSpecial;
            theCurrentRow.Cells[colCheckConclusion.Name].Value = obj.InspectionConclusion;
            theCurrentRow.Cells[colContent.Name].Value = obj.InspectionStatus;
            theCurrentRow.Cells[colCorrectiveSign.Name].Value = obj.CorrectiveSign;
            if (obj.CorrectiveSign.Equals(1))
            {
                theCurrentRow.Cells[colCorrectiveSign.Name].Value = "需要整改，未进行处理";
            }
            if (obj.CorrectiveSign.Equals(2))
            {
                theCurrentRow.Cells[colCorrectiveSign.Name].Value = "需要整改，已进行处理";
            }
            if (obj.CorrectiveSign.Equals(0))
            {
                theCurrentRow.Cells[colCorrectiveSign.Name].Value = "不需整改";
            }
            if (obj.CreatePerson != null)
            {
                theCurrentRow.Cells[colCheckPerson.Name].Tag = obj.CreatePerson;
                theCurrentRow.Cells[colCheckPerson.Name].Value = obj.CreatePersonName;
            }
            theCurrentRow.Tag = obj;
        }
        #endregion

        void btnSelectGWBSDetail_Click(object sender, EventArgs e)
        {
            VGWBSDetailSelector vss = new VGWBSDetailSelector();
            vss.ShowDialog();
            IList list = vss.Result;
            if (list == null || list.Count == 0) return;

            dgDetail.Rows.Clear();
            foreach (WeekScheduleDetail detail in list)
            {
                txtCheckPerson.Value = ConstObject.LoginPersonInfo.Name;
                //查询该工程任务下的检查记录
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("WeekScheduleDetail.Id", detail.Id));
                IList temp_list = model.ProductionManagementSrv.GetInspectionRecord(oq);
                if (temp_list.Count > 0)
                {
                    foreach (InspectionRecord Record in temp_list)
                    {
                        int rowIndex = dgDetail.Rows.Add();
                        dgDetail[colCorrectiveSign.Name, rowIndex].Value = Record.CorrectiveSign;
                        dgDetail[colCheckSpecial.Name, rowIndex].Value = Record.InspectionSpecial;
                        dgDetail[colCheckConclusion.Name, rowIndex].Value = Record.InspectionConclusion;
                        dgDetail[colContent.Name, rowIndex].Value = Record.InspectionStatus;
                        dgDetail[colCheckPerson.Name, rowIndex].Value = Record.CreatePersonName;
                        string strCorrectiveSign = ClientUtil.ToString(Record.CorrectiveSign);
                        if (strCorrectiveSign.Equals("0"))
                        {
                            dgDetail[colCorrectiveSign.Name, rowIndex].Value = "不需整改";
                        }
                        if (strCorrectiveSign.Equals("1"))
                        {
                            dgDetail[colCorrectiveSign.Name, rowIndex].Value = "需要整改，未进行处理";
                        }
                        if (strCorrectiveSign.Equals("2"))
                        {
                            dgDetail[colCorrectiveSign.Name, rowIndex].Value = "需要整改，已进行处理";
                        }
                        txtWeekScheduleDetail.Tag = null;
                        txtWeekScheduleDetail.Tag = Record.WeekScheduleDetail;
                        dgDetail.Rows[rowIndex].Tag = Record;
                    }
                    dgDetail_CellClick(sender, new DataGridViewCellEventArgs(0, 0));
                }
                else
                {
                    int rowIndex = dgDetail.Rows.Add();
                    //构造检查记录
                    InspectionRecord record = new InspectionRecord();
                    record.GWBSTree = detail.GWBSTree;
                    record.GWBSTreeName = detail.GWBSTreeName;
                    record.PBSTree = detail.PBSTree;
                    record.PBSTreeName = detail.PBSTreeName;
                    record.GWBSDescription = detail.Descript;
                    dgDetail[colCorrectiveSign.Name, rowIndex].Value = record.CorrectiveSign;
                    txtWeekScheduleDetail.Tag = null;
                    txtWeekScheduleDetail.Tag = detail;
                    dgDetail.Rows[rowIndex].Tag = record;
                }
            }
        }

        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgDetail_CellClick(dgDetail, new DataGridViewCellEventArgs(0, 0));
        }

        private void Clear()
        {
            txtCheckStatus.Text = "";
            txtCheckPerson.Result.Clear();
            txtCheckPerson.Tag = ConstObject.LoginPersonInfo;
            txtCheckPerson.Value = ConstObject.LoginPersonInfo.Name;
            dtpCheckDate.Value = ConstObject.LoginDate;
            cbCheckConclusion.SelectedItem = null;
            cbWBSCheckRequir.SelectedItem = null;
        }

        public override void RefreshControls(MainViewState state)
        {
            switch (state)
            {
                case MainViewState.Modify:
                    //按钮
                    //界面输入控件
                    btnSaveInspectionRecord.Enabled = true;
                    txtCheckPerson.Enabled = true;
                    cbWBSCheckRequir.Enabled = true;
                    cbCheckConclusion.Enabled = true;
                    dtpCheckDate.Enabled = true;
                    txtCheckStatus.Enabled = true;
                    btnSubmit.Enabled = true;
                    radioButton1.Enabled = true;
                    radioButton2.Enabled = true;
                    txtSuppiler.Enabled = true;
                    btnSelect.Enabled = true;
                    break;
                case MainViewState.Browser:
                    //按钮
                    //界面输入控件
                    btnSaveInspectionRecord.Enabled = false;
                    txtCheckPerson.Enabled = false;
                    cbWBSCheckRequir.Enabled = false;
                    cbCheckConclusion.Enabled = false;
                    dtpCheckDate.Enabled = false;
                    txtCheckStatus.Enabled = false;
                    btnSubmit.Enabled = false;
                    txtSuppiler.Enabled = false;
                    btnSelect.Enabled = false;
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    break;
            }
        }
    }
}
