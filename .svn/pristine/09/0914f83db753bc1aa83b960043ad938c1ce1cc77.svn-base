using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.ProjectPlanningMange.Domain;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.Util;
using System.Collections;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using System.IO;
using IRPServiceModel.Domain.Document;
using Application.Business.Erp.SupplyChain.Client.FileUpload;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using IRPServiceModel.Basic;

namespace Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.BusinessProposalManage
{
    public partial class VBusinessProposal : TMasterDetailView
    {
        private MProjectPlanningMng model = new MProjectPlanningMng();
        private BusinessProposal curBillMaster;

        public VBusinessProposal()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }
        CurrentProjectInfo projectInfo;
        string userName = string.Empty;
        string jobId = string.Empty;
        string fileObjectType = string.Empty;
        /// <summary>
        /// 当前主表数据
        /// </summary>
        public BusinessProposal CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public void InitData()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
            this.colImplementResult.Items.AddRange(new object[] { "未完成", "中止", "完成" });
            dgMaster.ContextMenuStrip = cmsDg;
            RefreshControls(MainViewState.Browser);
        }

        public void InitEvents()
        {
            btnLogSave.Click += new EventHandler(btnLogSave_Click);
            btnLogDelete.Click += new EventHandler(btnLogDelete_Click);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            dgMaster.CellDoubleClick += new DataGridViewCellEventHandler(dgMaster_CellDoubleClick);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
            dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
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

        /// <summary>
        /// 商务策划点，双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgMaster_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            BusinessProposalItem curBillItem = dgMaster.CurrentRow.Tag as BusinessProposalItem;
            VBusinessProposalItem vitem = new VBusinessProposalItem(curBillItem);
            vitem.ShowDialog();
            curBillItem = vitem.CurBillItem;
            if (curBillItem != null)
            {
                if (dgMaster.CurrentRow.Cells[colImplementState.Name].Value == null)
                {
                    int i = dgMaster.Rows.Add();
                    this.dgMaster[colPlanningTheme.Name, i].Value = ClientUtil.ToString(curBillItem.PlanningTheme);
                    this.dgMaster[colPlanningItemType.Name, i].Value = ClientUtil.ToString(curBillItem.PlanningItemType);
                    this.dgMaster[colPlanningDateEnd.Name, i].Value = curBillItem.PlanningDateEnd.ToShortDateString();
                    this.dgMaster[colImplementState.Name, i].Value = ClientUtil.ToString(curBillItem.PlanningState);
                    this.dgMaster[colPlanningDateStart.Name, i].Value = curBillItem.PlanningDateStart.ToShortDateString();
                    this.dgMaster[colProposalStartTime.Name, i].Value = curBillItem.PlanningImplementStartDate.ToShortDateString();
                    this.dgMaster[colProposalEndDate.Name, i].Value = curBillItem.PlanningImplementDate.ToShortDateString();
                    this.dgMaster[colDescript.Name,i].Value = ClientUtil.ToString(curBillItem.Descript);
                    this.dgMaster.Rows[i].Tag = curBillItem;
                }
                else
                {
                    this.dgMaster.CurrentRow.Cells[colPlanningTheme.Name].Value = ClientUtil.ToString(curBillItem.PlanningTheme);
                    this.dgMaster.CurrentRow.Cells[colPlanningItemType.Name].Value = ClientUtil.ToString(curBillItem.PlanningItemType);
                    this.dgMaster.CurrentRow.Cells[colPlanningDateEnd.Name].Value = curBillItem.PlanningDateEnd.ToShortDateString();
                    this.dgMaster.CurrentRow.Cells[colImplementState.Name].Value = ClientUtil.ToString(curBillItem.PlanningState);
                    this.dgMaster.CurrentRow.Cells[colPlanningDateStart.Name].Value = curBillItem.PlanningDateStart.ToShortDateString();
                    this.dgMaster.CurrentRow.Cells[colProposalStartTime.Name].Value = curBillItem.PlanningImplementStartDate.ToShortDateString();
                    this.dgMaster.CurrentRow.Cells[colProposalEndDate.Name].Value = curBillItem.PlanningImplementDate.ToShortDateString();
                    this.dgMaster.CurrentRow.Cells[colDescript.Name].Value = ClientUtil.ToString(curBillItem.Descript);
                    this.dgMaster.CurrentRow.Tag = curBillItem;
                }
            }      
        }

        /// <summary>
        /// 商务策划点右键删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tsmiDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr = dgMaster.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                dgMaster.Rows.Remove(dr);
                if (dr.Tag != null)
                {
                    curBillMaster.Details.Remove(dr.Tag as BusinessProposalItem);
                }
            }
        }

        /// <summary>
        /// 商务策划点实施记录保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnLogSave_Click(object sender, EventArgs e)
        {
            if (dgMaster.CurrentRow == null) return;
            if (dgDetail.CurrentRow == null) return;
            BusinessProposalItem curBillItem = this.dgMaster.CurrentRow.Tag as BusinessProposalItem;
            //curBillItem.IrpDetails.Clear();
            if (curBillItem != null)
            {
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    IrpBusinessPlanManageLog curBillLog = new IrpBusinessPlanManageLog();
                    if (var.Tag != null)
                    {
                        curBillLog = var.Tag as IrpBusinessPlanManageLog;
                        if (curBillLog.Id == null)
                        {
                            curBillItem.IrpDetails.Remove(curBillLog);
                        }
                    }
                    curBillLog.ImplementCondition = ClientUtil.ToString(var.Cells[colImplementCondition.Name].Value);
                    curBillLog.ImplementPerson = ConstObject.LoginPersonInfo;
                    curBillLog.ImplementPersonName = ConstObject.LoginPersonInfo.Name;
                    curBillLog.ImplementResult = ClientUtil.ToString(var.Cells[colImplementResult.Name].Value);
                    curBillLog.ImplementDate = ClientUtil.ToDateTime(var.Cells[colImplementDate.Name].Value);
                    curBillItem.AddIrpDetail(curBillLog);

                    string strResult = ClientUtil.ToString(curBillLog.ImplementResult);
                    if(strResult == "未完成")
                    {
                        dgMaster.CurrentRow.Cells[colImplementState.Name].Value = "实施";
                        curBillItem.PlanningState = EnumItemPlanningState.实施;
                        
                    }
                    if (strResult == "中止")
                    {
                        dgMaster.CurrentRow.Cells[colImplementState.Name].Value = "中止";
                        curBillItem.PlanningState = EnumItemPlanningState.中止;
                    }
                    if (strResult == "完成")
                    {
                        dgMaster.CurrentRow.Cells[colImplementState.Name].Value = "完成";
                        curBillItem.PlanningState = EnumItemPlanningState.完成;
                    }
                    curBillItem.PlanningImplementDate = curBillLog.ImplementDate;
                }
                MessageBox.Show("商务策划点实施记录保存成功！");
                 
            }
            else
            {
                MessageBox.Show("商务策划点为空，保存错误！");
                dgDetail.Rows.Clear();
            }
        }

        /// <summary>
        /// 商务策划点实施记录删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnLogDelete_Click(object sender, EventArgs e)
        {
            if (dgDetail.CurrentRow == null) return;
            IrpBusinessPlanManageLog curBillLog = this.dgDetail.CurrentRow.Tag as IrpBusinessPlanManageLog;
            BusinessProposalItem curBillItem = this.dgMaster.CurrentRow.Tag as BusinessProposalItem;
            if (curBillLog == null) return;
            if (model.ProjectPlanningSrv.DeleteByDao(curBillLog))
            {
                MessageBox.Show("商务策划点实施记录删除成功！");
                curBillItem.IrpDetails.Remove(curBillLog);
                this.dgDetail.Rows.Clear();
                RefreshState(MainViewState.Browser);
                
            }
        }

        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            dgDetail.Rows.Clear();
            BusinessProposalItem master = dgMaster.CurrentRow.Tag as BusinessProposalItem;
            if (master == null) return;
            foreach (IrpBusinessPlanManageLog log in master.IrpDetails)
            {
                int rowIndex = dgDetail.Rows.Add();
                dgDetail.Rows[rowIndex].Tag = log;
                this.dgDetail[colImplementCondition.Name, rowIndex].Value = log.ImplementCondition;
                this.dgDetail[colImplementPerson.Name, rowIndex].Value = log.ImplementPersonName;
                this.dgDetail[colImplementDate.Name, rowIndex].Value = log.ImplementDate.ToShortDateString();
                this.dgDetail[colImplementResult.Name, rowIndex].Value = log.ImplementResult;
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
                        curBillMaster = model.ProjectPlanningSrv.SaveBusinessProposal(curBillMaster);
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
                    btnDownLoadDocument.Enabled = true;
                    btnOpenDocument.Enabled = true;
                    curBillMaster = model.ProjectPlanningSrv.GetBusinessProposalById(Id);
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
                     this.btnLogDelete.Enabled = false;
                     break;
                case MainViewState.Modify:
                    cmsDg.Enabled = true;
                    this.btnLogDelete.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
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
                ObjectLock.Unlock(pnlContent, true);
                btnStates(1);
            }
            else
            {
                ObjectLock.Lock(pnlContent, true);
                btnStates(0);
            }
            //永久锁定
            object[] os = new object[] { txtCreatePerson, dtpCreateDate, txtProject, txtState };
            ObjectLock.Lock(os);
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

                this.curBillMaster = new BusinessProposal();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.CreateDate = ConstObject.LoginDate;
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.DocState = DocumentState.Edit;
                this.txtState.Text = ClientUtil.GetDocStateName(curBillMaster.DocState);

                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //归属项目
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                if (projectInfo != null)
                {
                    txtProject.Tag = projectInfo;
                    txtProject.Text = projectInfo.Name;
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
                    curBillMaster.ProjectCost = projectInfo.ProjectCost;
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
                curBillMaster = model.ProjectPlanningSrv.GetBusinessProposalById(curBillMaster.Id);
                ModelToView();
                //btnStates(1);
                return true;
            }
            this.btnLogDelete.Enabled = true;
            string message = "此单状态为【{0}】，不能修改！";
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
                if (!ViewToModel()) return false;
                curBillMaster.DocState = DocumentState.InExecute;
                curBillMaster = model.ProjectPlanningSrv.SaveBusinessProposal(curBillMaster);
                this.txtState.Text = ClientUtil.GetDocStateName(curBillMaster.DocState);
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
        /// 保存数据前校验数据
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            if (ClientUtil.ToString(this.txtEnginnerName.Text) == "")
            {
                MessageBox.Show("策划名称不能为空！");
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
                curBillMaster = model.ProjectPlanningSrv.SaveBusinessProposal(curBillMaster);
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
                curBillMaster = model.ProjectPlanningSrv.GetBusinessProposalById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    //if (!model.ProjectPlanningSrv.DeleteByDao(curBillMaster)) return false;
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
                        curBillMaster = model.ProjectPlanningSrv.GetBusinessProposalById(curBillMaster.Id);
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

        private bool ModelToView()
        {
            try
            {
                this.txtProject.Text = ClientUtil.ToString(curBillMaster.ProjectName);
                this.txtProject.Tag = curBillMaster.ProjectId; 
                this.txtCreatePerson.Value = ClientUtil.ToString(curBillMaster.CreatePersonName);
                this.dtpCreateDate.Value = Convert.ToDateTime(curBillMaster.CreateDate);
                this.txtEnginnerName.Text = ClientUtil.ToString(curBillMaster.EnginnerName);
                this.txtState.Text = ClientUtil.GetDocStateName(curBillMaster.DocState);

                this.dgMaster.Rows.Clear();
                foreach (BusinessProposalItem item in curBillMaster.Details)
                {
                    int rowIndex = this.dgMaster.Rows.Add();
                    this.dgMaster[colPlanningTheme.Name, rowIndex].Value = ClientUtil.ToString(item.PlanningTheme);
                    this.dgMaster[colPlanningItemType.Name, rowIndex].Value = ClientUtil.ToString(item.PlanningItemType);
                    this.dgMaster[colPlanningDateEnd.Name, rowIndex].Value = item.PlanningDateEnd.ToShortDateString();
                    this.dgMaster[colImplementState.Name, rowIndex].Value = ClientUtil.ToString(item.PlanningState);
                    this.dgMaster[colPlanningDateStart.Name, rowIndex].Value = item.PlanningDateStart.ToShortDateString();
                    this.dgMaster[colProposalStartTime.Name, rowIndex].Value = item.PlanningImplementStartDate.ToShortDateString();
                    this.dgMaster[colProposalEndDate.Name, rowIndex].Value = item.PlanningImplementDate.ToShortDateString();
                    this.dgMaster[colDescript.Name,rowIndex].Value = ClientUtil.ToString(item.Descript);
                    this.dgMaster.Rows[rowIndex].Tag = item;

                    if (dgMaster.Rows.Count > 0)
                    {
                        dgMaster.CurrentCell = dgMaster.Rows[0].Cells[0];
                        dgMaster_CellClick(dgMaster, new DataGridViewCellEventArgs(0, 0));
                    }
                }
                FillDoc();
                return true;
            }
            catch (Exception e)
            {
               throw e;
            }
        }

        void dgMaster_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dr = dgMaster.CurrentRow;
            if (dr == null) return;
            BusinessProposalItem master = dr.Tag as BusinessProposalItem;
            if (master == null) return;
            dgDetail.Rows.Clear();
            foreach (IrpBusinessPlanManageLog log in master.IrpDetails)
             {
                 int i = this.dgDetail.Rows.Add();
                 this.dgDetail[colImplementCondition.Name, i].Value = ClientUtil.ToString(log.ImplementCondition);
                 this.dgDetail[colImplementPerson.Name, i].Value = ClientUtil.ToString(log.ImplementPersonName);
                 this.dgDetail[colImplementDate.Name, i].Value = log.ImplementDate.ToShortDateString();
                 this.dgDetail[colImplementResult.Name, i].Value = ClientUtil.ToString(log.ImplementResult);
                 this.dgDetail.Rows[i].Tag = log;
             }
        }

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colImplementPerson.Name))
            {
                CommonPerson personSelector = new CommonPerson();
                DataGridViewCell cell = this.dgDetail[e.ColumnIndex, e.RowIndex];
                object tempValue = cell.EditedFormattedValue;
                if (tempValue != null && !tempValue.Equals(""))
                {
                    personSelector.OpenSelect();
                }
                else
                {
                    personSelector.OpenSelect();
                }

                IList list = personSelector.Result;
                if (list != null && list.Count > 0)
                {
                    PersonInfo thePerson = list[0] as PersonInfo;
                    dgDetail.CurrentRow.Cells[colImplementPerson.Name].Value = thePerson.Name;
                    dgDetail.CurrentRow.Cells[colImplementPerson.Name].Tag = thePerson;
                }
            }
        }

        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //bool validity = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;

            if (dgDetail.Rows[e.RowIndex].Cells[colImplementPerson.Name].Value == null)
            {
                dgDetail.Rows[e.RowIndex].Cells[colImplementPerson.Name].Value = ConstObject.LoginPersonInfo.Name;
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
                curBillMaster.EnginnerName = ClientUtil.ToString(this.txtEnginnerName.Text);
                curBillMaster.SubmitDate = DateTime.Now;
                //curBillMaster.State = EnumUtil<EnumDocState>.FromDescription(cmbDocState.SelectedItem);
                string strUnit = "元";
                Application.Resource.MaterialResource.Domain.StandardUnit Unit = null;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Name", strUnit));
                IList lists = model.ProjectPlanningSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq);
                if (lists != null && lists.Count > 0)
                {
                    Unit = lists[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                }
                curBillMaster.PriceUnit = Unit;
                //curBillMaster.Details.Clear();
                foreach (DataGridViewRow var in this.dgMaster.Rows)
                {
                    if (var.IsNewRow) break;
                    BusinessProposalItem curBillItem = new BusinessProposalItem();
                    if (var.Tag != null)
                    {
                        curBillItem = var.Tag as BusinessProposalItem;
                        if (curBillItem.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillItem);
                        }
                    }
                    curBillItem = var.Tag as BusinessProposalItem;
                    curBillMaster.AddDetail(curBillItem);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
                
            return true;
        }

     
    }
}
