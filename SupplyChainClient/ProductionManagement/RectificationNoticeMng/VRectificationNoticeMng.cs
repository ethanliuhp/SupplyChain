﻿using System;
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
using Application.Business.Erp.SupplyChain.ProductionManagement.RectificationNoticeMng.Domain;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionLotMng;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionRecordMng;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.AcceptanceInspectionMng;
using Application.Business.Erp.SupplyChain.ProductionManagement.AcceptanceInspectionMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.OBS;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Domain;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.ProInsRecordMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using System.IO;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using VirtualMachine.Core;
using System.Diagnostics;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.ProfessionInspectionRecordMng;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using IRPServiceModel.Domain.Document;
using Application.Business.Erp.SupplyChain.Client.FileUpload;
using IRPServiceModel.Basic;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.RectificationNoticeMng
{
    public partial class VRectificationNoticeMng : TMasterDetailView
    {
        private MRectificationNoticeMng model = new MRectificationNoticeMng();
        private RectificationNoticeMaster curBillMaster;
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        CurrentProjectInfo projectInfo = null;
        string userName = string.Empty;
        string jobId = string.Empty;
        string fileObjectType = string.Empty;

        /// <summary>
        /// 当前单据
        /// </summary>
        public RectificationNoticeMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VRectificationNoticeMng()
        {
            InitializeComponent();
            InitEvent();
            InitData();
            InitDate();
        }

        private void InitData()
        {
            txtInspectionPerson.IsAllLoad = false;
            txtDangerLevel.Items.AddRange(new object[] { "一般", "重要", "紧急" });
            VBasicDataOptr.InitDangerType(txtDangerType, false);
            VBasicDataOptr.InitDangerType(colDangerType, false);
            txtConclusion.Items.AddRange(new object[] { "整改中", "整改未通过", "整改通过" });
            txtAccepType.Items.AddRange(new object[] { "专业检查", "工程日常检查" });
            txtCompletDate.Value = DateTime.Now;
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
            List<string> listParams = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetUploadFileParamsByMBP_IRP();
            fileObjectType = listParams[0];
        }

        private void InitEvent()
        {
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            this.dgDetail.SelectionChanged += new EventHandler(dgDetail_SelectionChanged);
            btnSelect.Click += new EventHandler(btnSelect_Click);
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            txtAccepType.SelectedIndexChanged += new EventHandler(txtAccepType_SelectedIndexChanged);
            this.dgDetail.CellClick += new DataGridViewCellEventHandler(dgDetail_CellClick);
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
            checkBox1.Click += new EventHandler(checkBox1_Click);
            this.txtCompletDate.CloseUp += new EventHandler(txtCompletDate_CloseUp);
            this.btnType.Click += new EventHandler(btnType_Click);
            this.txtDangerLevel.TextChanged += new EventHandler(txtDangerLevel_TextChanged);
            this.txtDangerPart.TextChanged += new EventHandler(txtDangerPart_TextChanged);
            this.txtProblem.TextChanged += new EventHandler(txtProblem_TextChanged);
            this.txtRequired.TextChanged += new EventHandler(txtRequired_TextChanged);
            this.txtInspection.TextChanged += new EventHandler(txtInspection_TextChanged);
            this.txtConclusion.TextChanged += new EventHandler(txtConclusion_TextChanged);
            this.checkBox1.CheckedChanged += new EventHandler(checkBox1_CheckedChanged);
            this.txtCompletDate.CloseUp += new EventHandler(txtCompletDate_CloseUp);
            
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
            dgDocumentDetail.ReadOnly = false;
            FileSelect.ReadOnly = false;
        }
        //加载文档数据
        void FillDoc(string id)
        {
            dgDocumentMast.Rows.Clear();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", id));
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
                        curBillMaster = model.RectificationNoticeSrv.SaveRectificationNotice(curBillMaster);
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

        void txtCompletDate_CloseUp(object sender, EventArgs e)
        {
            if (dgDetail.CurrentRow != null)
            {
                if (txtCompletDate.Value != null)
                {
                    if (txtCompletDate.Value.Date < DateTime.Now)
                    {
                        MessageBox.Show("要求整改完成时间不得早于建立整改单时间！");
                        txtCompletDate.Value = DateTime.Now;
                        return;
                    }
                }
                dgDetail.CurrentRow.Cells[colRequiredDate.Name].Value = txtCompletDate.Value.ToShortDateString();
            }
        }

        void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (dgDetail.CurrentRow != null)
                {
                    dgDetail.CurrentRow.Cells[colRequiredDate.Name].Value = txtCompletDate.Value.ToShortDateString();
                }
            }
            else
            {
                if (dgDetail.CurrentRow != null)
                {
                    dgDetail.CurrentRow.Cells[colRequiredDate.Name].Value = "";
                }
            }
        }

        void txtConclusion_TextChanged(object sender, EventArgs e)
        {
            if (dgDetail.CurrentRow != null)
            {
                dgDetail.CurrentRow.Cells[colConclusion.Name].Value = txtConclusion.Text;
            }
        }

        void txtInspection_TextChanged(object sender, EventArgs e)
        {
            if (dgDetail.CurrentRow != null)
            {
                dgDetail.CurrentRow.Cells[colReqCSH.Name].Value = txtInspection.Text;
            }
        }

        void txtRequired_TextChanged(object sender, EventArgs e)
        {
            if (dgDetail.CurrentRow != null)
            {
                dgDetail.CurrentRow.Cells[colRectRequired.Name].Value = txtRequired.Text;
            }
        }

        void txtProblem_TextChanged(object sender, EventArgs e)
        {
            if (dgDetail.CurrentRow != null)
            {
                dgDetail.CurrentRow.Cells[colProblem.Name].Value = txtProblem.Text;
            }
        }

        void txtDangerPart_TextChanged(object sender, EventArgs e)
        {
            if (dgDetail.CurrentRow != null)
            {
                dgDetail.CurrentRow.Cells[colDangerPart.Name].Value = txtDangerPart.Text;
            }
        }

        void txtDangerLevel_TextChanged(object sender, EventArgs e)
        {
            if (dgDetail.CurrentRow != null)
            {
                dgDetail.CurrentRow.Cells[colDangerLevel.Name].Value = txtDangerLevel.Text;
            }
        }

        void btnType_Click(object sender, EventArgs e)
        {
            VDangerTypeSelector select = new VDangerTypeSelector(txtDangerType.Text.ToString());
            select.ShowDialog();
            txtDangerType.Text = select.Result;
            if (dgDetail.CurrentRow != null)
            {
                dgDetail.CurrentRow.Cells[colDangerType.Name].Value = txtDangerType.Text;
            }
        }

        void checkBox1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtCompletDate.Enabled = true;
            }
            else
            {
                txtCompletDate.Enabled = false;
            }
        }



        void dgDetail_SelectionChanged(object sender, EventArgs e)
        {

            if (dgDetail.Rows.Count > 0)
            {
                txtProblem.Text = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colProblem.Name].Value);
                txtRequired.Text = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colRectRequired.Name].Value);
                txtInspection.Text = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colReqCSH.Name].Value);
                if (dgDetail.CurrentRow.Cells[colRequiredDate.Name].Value != null)
                {
                    if (dgDetail.CurrentRow.Cells[colRequiredDate.Name].Value != "")
                    {
                        txtCompletDate.Value = Convert.ToDateTime(dgDetail.CurrentRow.Cells[colRequiredDate.Name].Value);
                    }
                    txtCompletDate.Enabled = true;
                    checkBox1.Enabled = true;
                    checkBox1.Checked = true;
                }
                else
                {
                    txtCompletDate.Value = DateTime.Now;
                    txtCompletDate.Enabled = false;
                    checkBox1.Enabled = false;
                    checkBox1.Checked = false;
                }
                if (dgDetail.CurrentRow.Cells[colProblem.Name].Tag != null)
                {
                    if (txtAccepType.Text == "工程日常检查")
                    {
                        txtDangerLevel.Enabled = false;
                        txtDangerPart.Enabled = false;
                        btnType.Enabled = false;
                    }
                    if (txtAccepType.Text == "专业检查")
                    {
                        ProfessionInspectionRecordDetail ProDetail = dgDetail.CurrentRow.Cells[colProblem.Name].Tag as ProfessionInspectionRecordDetail;
                        if (dgDetail.CurrentRow.Cells[colSep.Name].Value == "安全员检查")
                        {
                            txtDangerLevel.SelectedItem = ClientUtil.ToString(ProDetail.DangerLevel);
                            txtDangerPart.Text = ClientUtil.ToString(ProDetail.DangerPart);
                            txtDangerType.Text = ClientUtil.ToString(ProDetail.DangerType);
                            txtDangerLevel.Enabled = true;
                            txtDangerPart.Enabled = true;
                            btnType.Enabled = true;
                        }
                        else
                        {
                            txtDangerLevel.Enabled = false;
                            txtDangerPart.Enabled = false;
                            btnType.Enabled = false;
                        }
                    }
                }
                RectificationNoticeDetail rDtl = dgDetail.CurrentRow.Tag as RectificationNoticeDetail;
                if (rDtl != null && rDtl.Id != null)
                    FillDoc(rDtl.Id);
            }
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

        void dgDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgDetail.Rows.Count > 0)
            {
                txtProblem.Text = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colProblem.Name].Value);
                txtRequired.Text = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colRectRequired.Name].Value);
                txtInspection.Text = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colReqCSH.Name].Value);
                txtConclusion.Text = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colConclusion.Name].Value);
                if (dgDetail.CurrentRow.Cells[colRequiredDate.Name].Value != null)
                {
                    if (ClientUtil.ToString(dgDetail.CurrentRow.Cells[colRequiredDate.Name].Value) != "")
                    {
                        txtCompletDate.Value = Convert.ToDateTime(dgDetail.CurrentRow.Cells[colRequiredDate.Name].Value);
                        txtCompletDate.Enabled = true;
                        checkBox1.Enabled = true;
                        checkBox1.Checked = true;
                    }
                    else
                    {
                        txtCompletDate.Value = DateTime.Now;
                        txtCompletDate.Enabled = false;
                        checkBox1.Enabled = false;
                        checkBox1.Checked = false;
                    }
                }
                else
                {
                    txtCompletDate.Value = DateTime.Now;
                    txtCompletDate.Enabled = false;
                    checkBox1.Enabled = false;
                    checkBox1.Checked = false;
                }
                if (dgDetail.CurrentRow.Cells[colProblem.Name].Tag != null)
                {
                    if (txtAccepType.Text == "工程日常检查")
                    {
                        txtDangerLevel.Enabled = false;
                        txtDangerPart.Enabled = false;
                        btnType.Enabled = false;
                    }
                    if (txtAccepType.Text == "专业检查")
                    {
                        ProfessionInspectionRecordDetail ProDetail = dgDetail.CurrentRow.Cells[colProblem.Name].Tag as ProfessionInspectionRecordDetail;
                        if (dgDetail.CurrentRow.Cells[colSep.Name].Value.ToString().Trim() == "安全员检查")
                        {
                            txtDangerLevel.SelectedItem = ClientUtil.ToString(ProDetail.DangerLevel);
                            txtDangerPart.Text = ClientUtil.ToString(ProDetail.DangerPart);
                            txtDangerType.Text = ClientUtil.ToString(ProDetail.DangerType);
                            txtDangerLevel.Enabled = true;
                            txtDangerPart.Enabled = true;
                            btnType.Enabled = true;
                        }
                        else
                        {
                            txtDangerLevel.Enabled = false;
                            txtDangerPart.Enabled = false;
                            btnType.Enabled = false;
                        }
                    }
                }
                RectificationNoticeDetail rDtl = dgDetail.CurrentRow.Tag as RectificationNoticeDetail;
                if (rDtl != null && rDtl.Id != null)
                    FillDoc(rDtl.Id);
            }
        }

        void btnSelect_Click(object sender, EventArgs e)
        {
            DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
            VContractExcuteSelector vmros = new VContractExcuteSelector();
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            dgDetail.Rows.Clear();
            SubContractProject engineerMaster = list[0] as SubContractProject;

            txtSuppiler.Text = engineerMaster.BearerOrgName;
            txtSuppiler.Tag = engineerMaster;
        }

        void txtAccepType_SelectedIndexChanged(object sender, EventArgs e)
        {
            IList<DataGridViewRow> lstRow = new List<DataGridViewRow>();
            if (dgDetail.Rows.Count > 0)
            {
                foreach (DataGridViewRow oRow in dgDetail.Rows)
                {
                    if (!oRow.IsNewRow)
                    {
                        lstRow.Add(oRow);
                    }
                }
                foreach (DataGridViewRow oRow in lstRow)
                {
                    dgDetail.Rows.Remove(oRow);
                    if (oRow.Tag != null)
                    {
                        curBillMaster.Details.Remove(oRow.Tag as RectificationNoticeDetail);
                    }
                }
            }
            //string strType = ClientUtil.ToString(txtAccepType.SelectedItem);
            //if (strType != null || strType != "")
            //{
            //    dgDetail.Rows.Clear();
            //    if (strType.Equals("工程日常检查"))
            //    {
            //        colCode.Visible = false;
            //        colDangerType.Visible = false;
            //        colDangerLevel.Visible = false;
            //        colDangerPart.Visible = false;
            //        btnType.Enabled = false;
            //        customLabel2.Enabled = false;
            //        txtDangerLevel.Enabled = false;
            //        txtDangerPart.Enabled = false;
            //        txtDangerType.Enabled = false;
            //        customLabel9.Enabled = false;
            //        customLabel12.Enabled = false;
            //        colSep.Visible = true;
            //    }
            //    if (strType.Equals("专业检查"))
            //    {
            //        colCode.Visible = true;
            //        colDangerType.Visible = true;
            //        colDangerLevel.Visible = true;
            //        colDangerPart.Visible = true;
            //        txtDangerLevel.Enabled = true;
            //        txtDangerPart.Enabled = true;
            //        btnType.Visible = true;
            //        btnType.Enabled = true;
            //        customLabel2.Enabled = true;
            //        txtDangerLevel.Enabled = true;
            //        txtDangerPart.Enabled = true;
            //        txtDangerType.Enabled = true;
            //        customLabel9.Enabled = true;
            //        customLabel12.Enabled = true;
            //        colSep.Visible = true;
            //    }
            //    this.txtProblem.Text = "";
            //    this.txtRequired.Text = "";
            //    this.txtDangerPart.Text = "";
            //    this.txtInspection.Text = "";
            //    this.txtDangerType.Text = "";
            //if (txtSuppiler.Text != "")
            //{
            //    this.txtProblem.Enabled = true;
            //    this.txtRequired.Enabled = true;
            //    this.txtConclusion.Enabled = true;
            //    this.txtInspection.Enabled = true;
            //    this.checkBox1.Enabled = true;
            //}
            //}
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            PersonInfo PersonName = new PersonInfo();
            if (txtInspectionPerson.Result.Count > 0)
            {
                PersonName = txtInspectionPerson.Result[0] as PersonInfo;
            }
            SubContractProject subProject = txtSuppiler.Tag as SubContractProject;
            string strType = ClientUtil.ToString(txtAccepType.SelectedItem);
            if (txtSuppiler.Tag != null && txtInspectionPerson.Text != "" && strType != "")
            {
                if (strType.Equals("工程日常检查"))
                {
                    DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                    VInspectionSelector frm = new VInspectionSelector(PersonName, subProject);
                    frm.ShowDialog();
                    IList list = frm.Result;
                    if (list == null || list.Count == 0) return;
                    //this.txtProblem.TextChanged -= new EventHandler(txtProblem_TextChanged);
                    //this.txtRequired.TextChanged -= new EventHandler(txtRequired_TextChanged);
                    //this.txtInspection.TextChanged -= new EventHandler(txtInspection_TextChanged);
                    if (list != null && list.Count > 0)
                    {
                        if (dgDetail.Rows.Count <= 0)
                        {
                            foreach (InspectionRecord var in list)
                            {
                                int i = dgDetail.Rows.Add();
                                this.dgDetail[colProblem.Name, i].Tag = var;
                                //this.dgDetail[colProblem.Name, i].Value = var.InspectionContent;
                                //this.txtProblem.Text = var.InspectionContent;
                                this.dgDetail[colSep.Name, i].Value = var.InspectionSpecial;
                                this.dgDetail[colProblem.Name, i].Value = var.InspectionStatus;
                                //this.txtProblem.Text = var.InspectionStatus;
                                //this.txtRequired.Text = "";
                                //this.txtProblem.Text = ClientUtil.ToString(dgDetail[colProblem.Name, i].Value);
                            }
                        }
                        else
                        {
                            foreach (InspectionRecord var in list)
                            {
                                bool flag = true;
                                //判断表格中是否已经存在选中的信息
                                foreach (DataGridViewRow vardetail in this.dgDetail.Rows)
                                {
                                    if (var.Id == (vardetail.Cells[colProblem.Name].Tag as InspectionRecord).Id)
                                    {
                                        flag = false;
                                        break;
                                    }
                                }
                                if (flag)
                                {
                                    int i = dgDetail.Rows.Add();
                                    this.dgDetail[colProblem.Name, i].Tag = var;
                                    //this.dgDetail[colProblem.Name, i].Value = var.InspectionContent;
                                    //this.txtProblem.Text = var.InspectionContent;
                                    this.dgDetail[colSep.Name, i].Value = var.InspectionSpecial;
                                    this.dgDetail[colProblem.Name, i].Value = var.InspectionStatus;
                                    //this.txtProblem.Text = var.InspectionStatus;
                                    //this.txtRequired.Text = "";
                                }
                            }
                        }
                        this.txtDangerLevel.Enabled = false;
                        this.txtDangerPart.Enabled = false;
                        this.btnType.Enabled = false;
                    }
                    //this.txtProblem.TextChanged += new EventHandler(txtProblem_TextChanged);
                    //this.txtRequired.TextChanged += new EventHandler(txtRequired_TextChanged);
                    //this.txtInspection.TextChanged += new EventHandler(txtInspection_TextChanged);
                }
                if (strType.Equals("专业检查"))
                {
                    VProInsRecordSelector VProIns = new VProInsRecordSelector(PersonName, subProject);
                    VProIns.ShowDialog();
                    IList list = VProIns.Result;
                    if (list == null || list.Count == 0) return;
                    foreach (ProfessionInspectionRecordDetail detail in list)
                    {
                        bool flag = true;
                        //判断表格中是否已经存在选中的信息
                        foreach (DataGridViewRow vardetail in this.dgDetail.Rows)
                        {
                            if (vardetail.IsNewRow) break;
                            if (detail.Id == (vardetail.Cells[colProblem.Name].Tag as ProfessionInspectionRecordDetail).Id)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            int i = dgDetail.Rows.Add();
                            this.dgDetail[colProblem.Name, i].Tag = detail;
                            this.dgDetail[colCode.Name, i].Value = detail.Master.Code;
                            this.dgDetail[colRequiredDate.Name, i].Value = DateTime.Now;
                            this.dgDetail[colProblem.Name, i].Value = detail.InspectionContent;
                            this.dgDetail[colRectRequired.Name, i].Value = detail.MeasureRequired;
                            this.dgDetail[colDangerLevel.Name, i].Value = detail.DangerLevel;
                            this.dgDetail[colDangerPart.Name, i].Value = detail.DangerPart;
                            this.dgDetail[colDangerType.Name, i].Value = detail.DangerType;
                            ProfessionInspectionRecordMaster master = new ProfessionInspectionRecordMaster();
                            master = model.RectificationNoticeSrv.GetProfessionInspectionRecordById(detail.Master.Id);
                            this.dgDetail[colSep.Name, i].Value = master.InspectionSpecail;
                            this.dgDetail[colSep.Name, i].Tag = master;
                            this.txtProblem.Text = detail.InspectionContent;
                            this.txtRequired.Text = detail.MeasureRequired;
                            this.txtDangerLevel.Text = detail.DangerLevel;
                            this.txtDangerPart.Text = detail.DangerPart;
                            this.txtDangerType.Text = detail.DangerType;
                            this.dgDetail[colConclusion.Name, i].Value = txtConclusion.SelectedItem;
                        }
                    }
                    dgDetail.CurrentCell = dgDetail[1, 0];
                    dgDetail_SelectionChanged(dgDetail, new EventArgs());
                    if (dgDetail.CurrentRow.Cells[colSep.Name].Value.ToString().Trim() == "安全员检查")
                    {
                        this.txtDangerLevel.Enabled = true;
                        this.txtDangerPart.Enabled = true;
                        this.btnType.Enabled = true;
                    }
                }
                this.txtProblem.Enabled = true;
                this.txtRequired.Enabled = true;
                this.txtConclusion.Enabled = true;
                this.txtInspection.Enabled = true;
                this.checkBox1.Enabled = true;
            }
            else
            {
                if (txtSuppiler.Tag == null)
                {
                    MessageBox.Show("请选择受检承担单位！");
                }
                if (strType == "" || strType == null)
                {
                    MessageBox.Show("请选择检查类型！");
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
                    curBillMaster.Details.Remove(dr.Tag as RectificationNoticeDetail);
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
                    curBillMaster = model.RectificationNoticeSrv.GetRectificationNoticeById(Id);
                    ModelToView();
                    RefreshState(MainViewState.Browser);
                    btnDownLoadDocument.Enabled = true;
                    btnOpenDocument.Enabled = true;
                    //判断是否为制单人
                    string perid = ConstObject.LoginPersonInfo.Id;
                    if (curBillMaster.CreatePerson != null && !curBillMaster.CreatePerson.Id.Equals(perid))
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
                    this.txtAccepType.Enabled = true;
                    this.dgDetail.EditMode = DataGridViewEditMode.EditOnEnter;
                    if (this.dgDetail.CurrentRow != null)
                    {
                        if (this.dgDetail.CurrentRow.Cells[colSep.Name].Value == "安全员检查")
                        {
                            this.txtDangerLevel.Enabled = true;
                            this.txtDangerPart.Enabled = true;
                            this.btnType.Enabled = true;
                        }
                        else
                        {
                            this.txtDangerLevel.Enabled = false;
                            this.txtDangerPart.Enabled = false;
                            this.btnType.Enabled = false;
                        }
                    }
                    else
                    {
                        if (txtAccepType.Text != "")
                        {
                            this.txtDangerLevel.Enabled = false;
                            this.txtDangerPart.Enabled = false;
                            this.btnType.Enabled = false;
                        }
                        else
                        {
                            this.txtDangerLevel.Enabled = true;
                            this.txtDangerPart.Enabled = true;
                            this.btnType.Enabled = true;
                        }
                    }
                    if (txtSuppiler.Text != "")
                    {
                        this.txtProblem.Enabled = true;
                        this.txtRequired.Enabled = true;
                        this.txtConclusion.Enabled = true;
                        this.txtInspection.Enabled = true;
                        this.checkBox1.Enabled = true;
                        if (this.checkBox1.Checked)
                        {
                            txtCompletDate.Enabled = true;
                        }
                        else
                        {
                            txtCompletDate.Enabled = false;
                        }
                    }
                    else
                    {
                        this.txtProblem.Enabled = false;
                        this.txtRequired.Enabled = false;
                        this.txtConclusion.Enabled = false;
                        this.txtInspection.Enabled = false;
                        this.checkBox1.Enabled = false;
                        txtCompletDate.Enabled = false;
                    }
                    cmsDg.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.txtProblem.Enabled = false;
                    this.txtRequired.Enabled = false;
                    this.txtDangerLevel.Enabled = false;
                    this.txtDangerPart.Enabled = false;
                    this.txtConclusion.Enabled = false;
                    this.txtInspection.Enabled = false;
                    this.txtAccepType.Enabled = false;
                    this.checkBox1.Checked = false;
                    this.checkBox1.Enabled = false;
                    this.btnType.Enabled = false;
                    txtCompletDate.Enabled = false;
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
                    cmsDg.Enabled = false;
                    break;
                default:
                    break;
            }
            //txtCompletDate.Enabled = false;
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
            object[] os = new object[] { txtCode, txtDangerType, txtSuppiler };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colDangerPart.Name, colDangerLevel.Name, colDangerType.Name, colProblem.Name, colCode.Name, colRectRequired.Name, colRequiredDate.Name, colSep.Name, colReqCSH.Name, colConclusion.Name };
            dgDetail.SetColumnsReadOnly(lockCols);

            //dgDocumentDetail.ReadOnly = false;
            //FileSelect.ReadOnly = false;
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
                this.curBillMaster = new RectificationNoticeMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.CreateDate = ConstObject.LoginDate;
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                curBillMaster.HandOrgLevel = ConstObject.TheOperationOrg.Level;
                curBillMaster.DocState = DocumentState.Edit;
                txtAccepType.SelectedIndex = 0;
                txtConclusion.SelectedIndex = 0;
                txtCompletDate.Value = DateTime.Now;
                //审批人
                txtInspectionPerson.Result.Add(ConstObject.LoginPersonInfo);
                txtInspectionPerson.Text = ConstObject.LoginPersonInfo.Name;
                //开单日期
                txtCreateDate.Text = ConstObject.LoginDate.ToShortDateString();
                this.txtProblem.Text = "";
                this.txtRequired.Text = "";
                //归属项目
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                if (projectInfo != null)
                {
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
                curBillMaster = model.RectificationNoticeSrv.GetRectificationNoticeById(curBillMaster.Id);
                ModelToView();
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
                //if (flag)
                    curBillMaster = model.RectificationNoticeSrv.SaveRectificationNoticeOne(curBillMaster);
                //else
                    //curBillMaster = model.RectificationNoticeSrv.SaveRectificationNotice(curBillMaster);

                txtCode.Text = curBillMaster.Code;
                //更新Caption
                LogData log = new LogData();
                log.BillId = curBillMaster.Id;
                log.BillType = "整改通知单";
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
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                MessageBox.Show("保存成功！");
                //FillDoc();
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
        /// 提交
        /// </summary>
        /// <returns></returns>
        public override bool SubmitView()
        {
            try
            {
                if (!ViewToModel()) return false;
                curBillMaster.DocState = DocumentState.InAudit;
                curBillMaster.AuditPerson = ConstObject.LoginPersonInfo;//制单人编号
                curBillMaster.AuditPersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                curBillMaster.AuditDate = ConstObject.LoginDate;//制单时间
                curBillMaster.AuditYear = ConstObject.LoginDate.Year;//制单年
                curBillMaster.AuditMonth = ConstObject.LoginDate.Month;//制单月
                //将引用的检查记录的整改标志设置为2
                IList list = new ArrayList();
                if (txtAccepType.SelectedItem.Equals("工程日常检查"))
                {
                    foreach (DataGridViewRow var in this.dgDetail.Rows)
                    {
                        if (var.IsNewRow) break;
                        InspectionRecord record = new InspectionRecord();
                        if (var.Cells[colProblem.Name].Tag != null)
                        {
                            record = var.Cells[colProblem.Name].Tag as InspectionRecord;
                            record.CorrectiveSign = 2;
                            list.Add(record);
                        }
                    }
                }
                if (txtAccepType.SelectedItem.Equals("专业检查"))
                {
                    foreach (DataGridViewRow var in this.dgDetail.Rows)
                    {
                        if (var.IsNewRow) break;
                        ProfessionInspectionRecordDetail record = new ProfessionInspectionRecordDetail();
                        if (var.Cells[colProblem.Name].Tag != null)
                        {
                            record = var.Cells[colProblem.Name].Tag as ProfessionInspectionRecordDetail;
                            record.CorrectiveSign = 2;
                            list.Add(record);
                        }
                    }
                }
                //if (string.IsNullOrEmpty(curBillMaster.Id))
                    curBillMaster = model.RectificationNoticeSrv.SaveRectificationNoticeOne(curBillMaster, list, txtAccepType.SelectedIndex);
                //else
                    //curBillMaster = model.RectificationNoticeSrv.SaveRectificationNotice(curBillMaster, list, txtAccepType.SelectedIndex);
                //插入日志
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
                curBillMaster = model.RectificationNoticeSrv.GetRectificationNoticeById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    //if (!model.RectificationNoticeSrv.DeleteByDao(curBillMaster)) return false;
                    if (!msrv.DeleteReceiptAndDocument(curBillMaster, curBillMaster.Id)) return false;
                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "整改通知单";
                    log.Code = curBillMaster.Code;
                    log.OperType = "删除";
                    log.Descript = "";
                    log.OperPerson = ConstObject.LoginPersonInfo.Name;
                    log.ProjectName = curBillMaster.ProjectName;
                    StaticMethod.InsertLogData(log);
                    ClearView();
                    MessageBox.Show("删除成功！");
                    //FillDoc();
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
                        curBillMaster = model.RectificationNoticeSrv.GetRectificationNoticeById(curBillMaster.Id);
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
                curBillMaster = model.RectificationNoticeSrv.GetRectificationNoticeById(curBillMaster.Id);
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
            string validMessage = "";
            if (this.dgDetail.Rows.Count == 0)
            {
                MessageBox.Show("明细不能为空！");
                return false;
            }
            if (txtAccepType.SelectedItem == null)
            {
                MessageBox.Show("检查类型不能为空！");
                return false;
            }
            if (txtInspectionPerson.Result == null || txtInspectionPerson.Result.Count == 0 || this.txtInspectionPerson.Text == "")
            {
                MessageBox.Show("检查人不能为空,请选择！");
                return false;
            }

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

                if (dr.Cells[colProblem.Name].Value == null)
                {
                    MessageBox.Show("存在问题说明不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colProblem.Name];
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
                if (txtInspectionPerson.Result.Count > 0)
                {
                    curBillMaster.HandlePerson = txtInspectionPerson.Result[0] as PersonInfo;
                    curBillMaster.HandlePersonName = curBillMaster.HandlePerson.Name;
                }
                curBillMaster.SupplierUnitName = ClientUtil.ToString(txtSuppiler.Text);
                curBillMaster.SupplierUnit = txtSuppiler.Tag as SubContractProject;
                curBillMaster.Descript = ClientUtil.ToString(this.txtRemark.Text);
                curBillMaster.InspectionType = ClientUtil.ToInt(txtAccepType.SelectedIndex);
                curBillMaster.Descript = ClientUtil.ToString(txtRemark.Text);
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    RectificationNoticeDetail curBillDtl = new RectificationNoticeDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as RectificationNoticeDetail;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.QuestionState = ClientUtil.ToString(var.Cells[colProblem.Name].Value);//问题说明
                    curBillDtl.ForwordCode = ClientUtil.ToString(var.Cells[colCode.Name].Value);
                    if (txtAccepType.SelectedItem.Equals("工程日常检查"))
                    {
                        curBillDtl.ForwordInsLot = var.Cells[colProblem.Name].Tag as InspectionRecord;//日常检查记录
                    }
                    if (txtAccepType.SelectedItem.Equals("专业检查"))
                    {
                        curBillDtl.ProfessionDetail = var.Cells[colProblem.Name].Tag as ProfessionInspectionRecordDetail;
                    }
                    //curBillDtl.ProblemCode = ;//问题代码
                    if (var.Cells[colRequiredDate.Name].Value != "")
                    {
                        curBillDtl.RequiredDate = Convert.ToDateTime(var.Cells[colRequiredDate.Name].Value);//要求整改时间
                    }
                    else
                    {
                        curBillDtl.RequiredDate = ClientUtil.ToDateTime("1900-1-1");
                    }
                    curBillDtl.DangerPart = ClientUtil.ToString(var.Cells[colDangerPart.Name].Value);//隐患部位
                    curBillDtl.DangerLevel = ClientUtil.ToString(var.Cells[colDangerLevel.Name].Value);//隐患级别
                    curBillDtl.DangerType = ClientUtil.ToString(var.Cells[colDangerType.Name].Value);//隐患类型
                    curBillDtl.RectContent = ClientUtil.ToString(var.Cells[colReqCSH.Name].Value); //整改措施和整改说明
                    if (var.Cells[colRequiredDate.Name].Value != "")
                    {
                        curBillDtl.RectDate = Convert.ToDateTime(var.Cells[colRequiredDate.Name].Value); //整改结论的时间
                    }
                    curBillDtl.Rectrequired = ClientUtil.ToString(var.Cells[colRectRequired.Name].Value);//整改要求
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
                txtRemark.Text = curBillMaster.Descript;
                txtAccepType.SelectedIndex = curBillMaster.InspectionType;
                txtSuppiler.Text = curBillMaster.SupplierUnitName;
                txtSuppiler.Tag = curBillMaster.SupplierUnit;
                txtInspectionPerson.Result.Clear();
                this.txtInspectionPerson.Tag = curBillMaster.HandlePerson;
                txtInspectionPerson.Result.Add(curBillMaster.HandlePerson);
                this.txtInspectionPerson.Value = curBillMaster.HandlePersonName;
                this.txtCreateDate.Value = curBillMaster.CreateDate;
                this.txtRemark.Text = curBillMaster.Descript;
                this.dgDetail.Rows.Clear();
                foreach (RectificationNoticeDetail var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();
                    this.dgDetail[colProblem.Name, i].Value = var.QuestionState;
                    this.dgDetail[colCode.Name, i].Value = var.ForwordCode;
                    this.dgDetail[colRequiredDate.Name, i].Value = var.RequiredDate;
                    if (var.ForwordInsLot != null)
                    {
                        this.dgDetail[colProblem.Name, i].Tag = var.ForwordInsLot;
                    }
                    if (var.AccepIns != null)
                    {
                        this.dgDetail[colProblem.Name, i].Tag = var.AccepIns;
                    }
                    if (var.ProfessionDetail != null)
                    {
                        this.dgDetail[colProblem.Name, i].Tag = var.ProfessionDetail;

                    }
                    this.dgDetail[colDangerLevel.Name, i].Value = var.DangerLevel;
                    this.dgDetail[colDangerType.Name, i].Value = var.DangerType;
                    this.dgDetail[colDangerPart.Name, i].Value = var.DangerPart;
                    this.dgDetail[colDangerLevel.Name, i].Value = var.DangerLevel;
                    if (var.ProfessionDetail != null)
                    {
                        ProfessionInspectionRecordMaster mat = model.RectificationNoticeSrv.GetProfessionInspectionRecordById(var.ProfessionDetail.Master.Id);
                        this.dgDetail[colSep.Name, i].Value = mat.InspectionSpecail;
                    }
                    if (var.ForwordInsLot != null)
                    {
                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("Id", var.ForwordInsLot.Id));
                        InspectionRecord Record = (model.RectificationNoticeSrv.GetInspectionRecord(oq))[0] as InspectionRecord;
                        this.dgDetail[colSep.Name, i].Value = Record.InspectionSpecial;
                    }
                    int IntConcluded = Convert.ToInt32(var.RectConclusion);
                    string strName = "";
                    if (IntConcluded == 0)
                    {
                        this.dgDetail[colConclusion.Name, i].Value = "整改中";
                    }
                    if (IntConcluded == 1)
                    {
                        strName = "整改未通过";
                        this.dgDetail[colConclusion.Name, i].Value = strName;
                    }
                    if (IntConcluded == 2)
                    {
                        strName = "整改通过";
                        this.dgDetail[colConclusion.Name, i].Value = strName;
                    }
                    this.dgDetail[colReqCSH.Name, i].Value = var.RectContent;

                    if (var.RequiredDate <= Convert.ToDateTime("1900-1-1"))
                    {
                        this.dgDetail[colRequiredDate.Name, i].Value = "";
                    }
                    else
                    {
                        this.dgDetail[colRequiredDate.Name, i].Value = var.RequiredDate;
                    }
                    this.dgDetail[colRectRequired.Name, i].Value = var.Rectrequired;
                    this.dgDetail[colDangerLevel.Name, i].Value = var.DangerLevel;
                    this.dgDetail.Rows[i].Tag = var;
                }
                //FillDoc();
                if (dgDetail.Rows.Count > 0)
                {
                    dgDetail.CurrentCell = dgDetail.Rows[0].Cells[0];
                    dgDetail_CellClick(dgDetail, new DataGridViewCellEventArgs(0, 0));
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            if (LoadTempleteFile(@"整改通知单.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            return true;
        }

        public override bool Print()
        {
            if (LoadTempleteFile(@"整改通知单.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.Print();
            return true;
        }

        public override bool Export()
        {
            if (LoadTempleteFile(@"整改通知单.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.ExportToExcel("整改通知单【" + curBillMaster.Code + "】", false, false, true);
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

        private void FillFlex(RectificationNoticeMaster billMaster)
        {
            int detailStartRowNumber = 5;
            int detailCount = billMaster.Details.Count;

            //插入明细行
            flexGrid1.InsertRow(detailStartRowNumber, detailCount);

            //设置单元格的边框，对齐方式
            //FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
            //range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            //range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            //range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            //range.Mask = FlexCell.MaskEnum.Digital;
            FlexCell.PageSetup pageSetup = flexGrid1.PageSetup;
            pageSetup.LeftMargin = 0;
            pageSetup.RightMargin = 0;
            pageSetup.BottomMargin = 1;
            pageSetup.TopMargin = 1;
            //pageSetup.Landscape = true;
            pageSetup.CenterHorizontally = true;

            //主表数据
            flexGrid1.Cell(2, 2).Text = billMaster.ProjectName;  //项目名称
            flexGrid1.Cell(2, 2).WrapText = true;

            flexGrid1.Cell(2, 4).Text = projectInfo.HandlePersonName;//负责人
            flexGrid1.Cell(2, 4).WrapText = true;

            flexGrid1.Cell(3, 2).Text = billMaster.HandlePersonName;//检查人
            flexGrid1.Cell(3, 2).WrapText = true;

            flexGrid1.Cell(3, 4).Text = billMaster.CreateDate.ToShortDateString();//检查时间
            flexGrid1.Cell(3, 4).WrapText = true;

            string strQs = "";
            string strRq = "";
            string strRC = "";
            int tt = 0;
            foreach (RectificationNoticeDetail dtl in billMaster.Details)
            {
                FlexCell.Range range = flexGrid1.Range(detailStartRowNumber + tt, 1, detailStartRowNumber + tt, 4);
                range.Merge();
                flexGrid1.Cell(detailStartRowNumber + tt, 1).Text = dtl.QuestionState + "  整改要求:[" + dtl.Rectrequired + "]";
                flexGrid1.Cell(detailStartRowNumber + tt, 1).Alignment = FlexCell.AlignmentEnum.LeftCenter;
                flexGrid1.Cell(detailStartRowNumber + tt, 1).WrapText = true;
                flexGrid1.Row(detailStartRowNumber + tt).AutoFit();
                tt++;
                //strQs += dtl.QuestionState + ";";        //问题说明
                //strRq += dtl.Rectrequired + ";";      //整改要求
                strRC += dtl.RectContent + ";";//整改措施
            }
            //flexGrid1.Cell(5, 1).Text = strQs;//问题说明
            //flexGrid1.Cell(5, 1).WrapText = true;
            //flexGrid1.Row(5).AutoFit();
            //flexGrid1.Cell(5, 4).Text = strRq;//整改要求
            //flexGrid1.Cell(5, 4).WrapText = true;
            flexGrid1.Cell(detailCount + 7, 1).Text = strRC;//整改措施
            flexGrid1.Cell(detailCount + 7, 1).WrapText = true;
            pageSetup.RightFooter = " 第&P页/共&N页   ";

            //找出关联文档
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", billMaster.Id));
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
                oq.AddFetchMode("ListFiles.TheFileCabinet", NHibernate.FetchMode.Eager);
                IList docList = model.ObjectQuery(typeof(DocumentMaster), oq);
                //List<string> listFileFullPaths = new List<string>();
                Hashtable ht = new Hashtable();
                string fileFullPath1 = AppDomain.CurrentDomain.BaseDirectory + "TempFilePreview\\" + Guid.NewGuid().ToString().Replace("-", "");
                if (docList != null && docList.Count > 0)
                {
                    int i = 1;
                    foreach (DocumentMaster m in docList)
                    {
                        foreach (DocumentDetail docFile in m.ListFiles)
                        {
                            if (docFile.ExtendName == ".jpg" || docFile.ExtendName == ".bmp" || docFile.ExtendName == ".JPG" || docFile.ExtendName == ".BMP")
                            {
                                if (!Directory.Exists(fileFullPath1))
                                    Directory.CreateDirectory(fileFullPath1);
                                string tempFileFullPath = fileFullPath1 + @"\\" + docFile.FileName;
                                string baseAddress = @docFile.TheFileCabinet.TransportProtocal.ToString().ToLower() + "://" + docFile.TheFileCabinet.ServerName + "/" + docFile.TheFileCabinet.Path + "/";
                                string address = baseAddress + docFile.FilePartPath;
                                UtilityClass.WebClientObj.DownloadFile(address, tempFileFullPath);
                                FileStream fs = new FileStream(tempFileFullPath, FileMode.Open, FileAccess.Read);
                                byte[] infbytes = new byte[(int)fs.Length];
                                fs.Read(infbytes, 0, infbytes.Length);
                                fs.Close();
                                flexGrid1.InsertRow(flexGrid1.Rows - 1, 1);
                                //flexGrid1.Cell(flexGrid1.Rows-1, 1).Text = "xxxxxxxxxxxx";
                                FlexCell.Range ranges = new FlexCell.Range();
                                ranges = flexGrid1.Range(flexGrid1.Rows - 2, 1, flexGrid1.Rows - 2, flexGrid1.Cols - 1);
                                ranges.Merge();
                                flexGrid1.Cell(flexGrid1.Rows - 2, 2).Text = docFile.FileName;


                                Image image = showEPhote(infbytes);
                                int ih = image.Height;
                                short[] ic = convertToShort(ih);
                                this.flexGrid1.Row(flexGrid1.Rows - 2).Height = ic[0];
                                this.flexGrid1.Images.Add(image, i.ToString());
                                this.flexGrid1.Cell(flexGrid1.Rows - 2, 1).SetImage(i.ToString());
                                this.flexGrid1.Cell(flexGrid1.Rows - 2, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                                flexGrid1.Column(1).AutoFit();
                                i++;
                            }
                        }
                    }
                }
                int fr = flexGrid1.Rows;
            }
        }
        public static short[] convertToShort(int i)
        {
            short[] a = new short[2];
            a[0] = (short)(i & 0x0000ffff);          //将整型的低位取出,   
            a[1] = (short)(i >> 16);                     //将整型的高位取出.   
            return a;
        }
        public Image showEPhote(byte[] infbytes)
        {
            //DBSservice.FileSrvSoapClient dbs = new Application.Business.Erp.SupplyChain.Client.DBSservice.FileSrvSoapClient();
            //byte[] signBytes = dbs.GetUserSign(percode);

            Image img = null; ;
            if (infbytes != null)
            {
                img = ReturnPhoto(infbytes);
            }
            return img;
        }

        //将byte[]转换成图片
        public System.Drawing.Image ReturnPhoto(byte[] streamByte)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(streamByte);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            return img;
        }
    }
}