using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Linq;

using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Main;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonForm;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using ImportIntegration;
using System.IO;
using com.think3.PLM.Integration.DataTransfer;
using System.Diagnostics;
using com.think3.PLM.Integration.Client.WS;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using IRPServiceModel.Domain.Document;
using IRPServiceModel.Basic;
using Application.Business.Erp.SupplyChain.Client.FileUpload;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
//using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;



namespace Application.Business.Erp.SupplyChain.Client.CostManagement.PBS
{
    public partial class VPBSMaintain : TBasicDataView
    {
        public MPBSTree model = new MPBSTree();

        public VPBSMaintain()
        {
            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();
        }

        private void InitEvents()
        {
            //文档操作
            //btnDownLoadDocument.Click += new EventHandler(btnDownLoadDocument_Click);
            //btnOpenDocument.Click += new EventHandler(btnOpenDocument_Click);
            //btnUpdateDocument.Click += new EventHandler(btnUpdateDocument_Click);
            //btnDeleteDocumentFile.Click += new EventHandler(btnDeleteDocumentFile_Click);
            //btnUpFile.Click += new EventHandler(btnUpFile_Click);
            //dgDocumentMast.CellClick += new DataGridViewCellEventHandler(dgDocumentMast_CellClick);
            //btnDeleteDocumentMaster.Click += new EventHandler(btnDeleteDocumentMaster_Click);
            //btnDocumentFileAdd.Click += new EventHandler(btnDocumentFileAdd_Click);
            //btnDocumentFileUpdate.Click += new EventHandler(btnDocumentFileUpdate_Click);
            //lnkCheckAll.Click += new EventHandler(lnkCheckAll_Click);
            //lnkCheckAllNot.Click += new EventHandler(lnkCheckAllNot_Click);
        }

        

        //#region 文档操作
        //MDocumentCategory msrv = new MDocumentCategory();
        ////文档按钮状态
        //private void btnStates(int i)
        //{
        //    if (i == 0)
        //    {
        //        //btnDownLoadDocument.Enabled = false;
        //        //btnOpenDocument.Enabled = false;
        //        btnUpdateDocument.Enabled = false;
        //        btnDeleteDocumentFile.Enabled = false;
        //        btnUpFile.Enabled = false;
        //        btnDeleteDocumentMaster.Enabled = false;
        //        btnDocumentFileAdd.Enabled = false;
        //        btnDocumentFileUpdate.Enabled = false;
        //        lnkCheckAll.Enabled = false;
        //        lnkCheckAllNot.Enabled = false;
        //    }
        //    if (i == 1)
        //    {
        //        //btnDownLoadDocument.Enabled = true;
        //        //btnOpenDocument.Enabled = true;
        //        btnUpdateDocument.Enabled = true;
        //        btnDeleteDocumentFile.Enabled = true;
        //        btnUpFile.Enabled = true;
        //        btnDeleteDocumentMaster.Enabled = true;
        //        btnDocumentFileAdd.Enabled = true;
        //        btnDocumentFileUpdate.Enabled = true;
        //        lnkCheckAll.Enabled = true;
        //        lnkCheckAllNot.Enabled = true;
        //    }
        //}
        ////加载文档数据
        //void FillDoc()
        //{
        //    dgDocumentMast.Rows.Clear();
        //    dgDocumentDetail.Rows.Clear();
        //    ObjectQuery oq = new ObjectQuery();
        //    oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", oprNode.Id));
        //    IList listObj = model.ObjectQuery(typeof(ProObjectRelaDocument), oq);
        //    if (listObj != null && listObj.Count > 0)
        //    {
        //        oq.Criterions.Clear();
        //        Disjunction dis = new Disjunction();
        //        foreach (ProObjectRelaDocument obj in listObj)
        //        {
        //            dis.Add(Expression.Eq("Id", obj.DocumentGUID));
        //        }
        //        oq.AddCriterion(dis);
        //        oq.AddFetchMode("ListFiles", NHibernate.FetchMode.Eager);
        //        IList docList = model.ObjectQuery(typeof(DocumentMaster), oq);
        //        if (docList != null && docList.Count > 0)
        //        {
        //            foreach (DocumentMaster m in docList)
        //            {
        //                AddDgDocumentMastInfo(m);
        //            }
        //            dgDocumentMast.CurrentCell = dgDocumentMast[2, 0];
        //            dgDocumentMast_CellClick(dgDocumentMast, new DataGridViewCellEventArgs(2, 0));
        //        }
        //    }
        //}
        ////添加文件
        //void btnDocumentFileAdd_Click(object sender, EventArgs e)
        //{
        //    if (dgDocumentMast.SelectedRows.Count == 0) return;
        //    DocumentMaster docMaster = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
        //    VDocumentFileUpload frm = new VDocumentFileUpload(docMaster, "add");
        //    frm.ShowDialog();
        //    IList resultList = frm.ResultList;
        //    if (resultList != null && resultList.Count > 0)
        //    {
        //        foreach (DocumentDetail dtl in resultList)
        //        {
        //            AddDgDocumentDetailInfo(dtl);
        //            docMaster.ListFiles.Add(dtl);
        //        }
        //    }
        //}
        ////修改文件
        //void btnDocumentFileUpdate_Click(object sender, EventArgs e)
        //{
        //    if (dgDocumentMast.SelectedRows.Count == 0 || dgDocumentDetail.SelectedRows.Count == 0)
        //    {
        //        MessageBox.Show("请选择要修改的文档件！");
        //        return;
        //    }
        //    DocumentMaster docMaster = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
        //    VDocumentFileModify frm = new VDocumentFileModify(docMaster);
        //    frm.ShowDialog();
        //    IList resultList = frm.ResultList;
        //    if (resultList != null && resultList.Count > 0)
        //    {
        //        DocumentDetail dtl = resultList[0] as DocumentDetail;
        //        AddDgDocumentDetailInfo(dtl, dgDocumentDetail.SelectedRows[0].Index);
        //        for (int i = 0; i < docMaster.ListFiles.Count; i++)
        //        {
        //            DocumentDetail detail = docMaster.ListFiles.ElementAt(i);
        //            if (detail.Id == dtl.Id)
        //            {
        //                detail = dtl;
        //            }
        //        }
        //    }
        //}
        ////下载
        //void btnDownLoadDocument_Click(object sender, EventArgs e)
        //{
        //    IList downList = new ArrayList();
        //    foreach (DataGridViewRow row in dgDocumentDetail.Rows)
        //    {
        //        if ((bool)row.Cells[FileSelect.Name].EditedFormattedValue)
        //        {
        //            DocumentDetail dtl = row.Tag as DocumentDetail;
        //            downList.Add(dtl);
        //        }
        //    }
        //    if (downList != null && downList.Count > 0)
        //    {
        //        VDocumentPublicDownload frm = new VDocumentPublicDownload(downList);
        //        frm.ShowDialog();
        //    }
        //    else
        //    {
        //        MessageBox.Show("请勾选要下载的文件！");
        //    }
        //}
        ////预览
        //void btnOpenDocument_Click(object sender, EventArgs e)
        //{
        //    IList list = new ArrayList();
        //    foreach (DataGridViewRow row in dgDocumentDetail.Rows)
        //    {
        //        if ((bool)row.Cells[FileSelect.Name].EditedFormattedValue)
        //        {
        //            DocumentDetail dtl = row.Tag as DocumentDetail;
        //            list.Add(dtl);
        //        }
        //    }

        //    if (list.Count == 0)
        //    {
        //        MessageBox.Show("勾选文件才能预览，请选择要预览的文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    try
        //    {

        //        List<string> listFileFullPaths = new List<string>();
        //        string fileFullPath1 = AppDomain.CurrentDomain.BaseDirectory + "TempFilePreview\\" + Guid.NewGuid().ToString().Replace("-", "");
        //        foreach (DocumentDetail docFile in list)
        //        {
        //            //DocumentDetail docFile = dgDocumentDetail.SelectedRows[0].Tag as DocumentDetail;
        //            if (!Directory.Exists(fileFullPath1))
        //                Directory.CreateDirectory(fileFullPath1);

        //            string tempFileFullPath = fileFullPath1 + @"\\" + docFile.FileName;

        //            //string address = "http://10.70.18.203//TestFile//Files//0001//0001//0001.0001.000027V_特效.jpg";
        //            string baseAddress = @docFile.TheFileCabinet.TransportProtocal.ToString().ToLower() + "://" + docFile.TheFileCabinet.ServerName + "/" + docFile.TheFileCabinet.Path + "/";

        //            string address = baseAddress + docFile.FilePartPath;
        //            UtilityClass.WebClientObj.DownloadFile(address, tempFileFullPath);
        //            listFileFullPaths.Add(tempFileFullPath);
        //        }
        //        foreach (string fileFullPath in listFileFullPaths)
        //        {
        //            FileInfo file = new FileInfo(fileFullPath);

        //            //定义一个ProcessStartInfo实例
        //            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
        //            //设置启动进程的初始目录
        //            info.WorkingDirectory = file.DirectoryName;//Application.StartupPath; 
        //            //设置启动进程的应用程序或文档名
        //            info.FileName = file.Name;
        //            //设置启动进程的参数
        //            info.Arguments = "";
        //            //启动由包含进程启动信息的进程资源
        //            try
        //            {
        //                System.Diagnostics.Process.Start(info);
        //            }
        //            catch (System.ComponentModel.Win32Exception we)
        //            {
        //                MessageBox.Show(this, we.Message);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.IndexOf("未将对象引用设置到对象的实例") > -1)
        //        {
        //            MessageBox.Show("操作失败，不存在的文档对象！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        }
        //        else
        //            MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }

        //}
        ////删除文件
        //void btnDeleteDocumentFile_Click(object sender, EventArgs e)
        //{
        //    List<DataGridViewRow> rowList = new List<DataGridViewRow>();
        //    IList deleteFileList = new ArrayList();
        //    foreach (DataGridViewRow row in dgDocumentDetail.Rows)
        //    {
        //        if ((bool)row.Cells[FileSelect.Name].EditedFormattedValue)
        //        {
        //            DocumentDetail dtl = row.Tag as DocumentDetail; ;
        //            rowList.Add(row);
        //            deleteFileList.Add(dtl);
        //        }
        //    }
        //    if (deleteFileList.Count == 0)
        //    {
        //        MessageBox.Show("请勾选要删除的数据！");
        //        return;
        //    }
        //    if (MessageBox.Show("要删除当前勾选文件吗！", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //    {
        //        if (msrv.Delete(deleteFileList))
        //        {
        //            MessageBox.Show("删除成功！");
        //            DocumentMaster master = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
        //            foreach (DocumentDetail dtl in deleteFileList)
        //            {
        //                master.ListFiles.Remove(dtl);
        //            }
        //            dgDocumentMast.SelectedRows[0].Tag = master;
        //        }
        //        else
        //        {
        //            MessageBox.Show("删除失败！");
        //            return;
        //        }
        //        if (rowList != null && rowList.Count > 0)
        //        {
        //            foreach (DataGridViewRow row in rowList)
        //            {
        //                dgDocumentDetail.Rows.Remove(row);
        //            }
        //        }
        //    }
        //}
        ////添加文档（加文件）
        //void btnUpFile_Click(object sender, EventArgs e)
        //{
        //    if (oprNode.Id == null)
        //    {
        //        if (MessageBox.Show("当前业务对象还没保存，是否保存！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
        //        {
        //            try
        //            {
        //                SaveView();
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(ex));
        //            }
        //        }
        //    }
        //    if (oprNode.Id != null)
        //    {
        //        VDocumentPublicUpload frm = new VDocumentPublicUpload(oprNode.Id);
        //        frm.ShowDialog();
        //        DocumentMaster resultDoc = frm.Result;
        //        if (resultDoc == null) return;
        //        AddDgDocumentMastInfo(resultDoc);
        //        dgDocumentMast.CurrentCell = dgDocumentMast[2, dgDocumentMast.Rows.Count - 1];
        //        dgDocumentDetail.Rows.Clear();
        //        if (resultDoc.ListFiles != null && resultDoc.ListFiles.Count > 0)
        //        {
        //            foreach (DocumentDetail dtl in resultDoc.ListFiles)
        //            {
        //                AddDgDocumentDetailInfo(dtl);
        //            }
        //        }
        //    }
        //}
        ////修改文档（加文件）
        //void btnUpdateDocument_Click(object sender, EventArgs e)
        //{
        //    if (dgDocumentMast.SelectedRows.Count == 0)
        //    {
        //        MessageBox.Show("请选择要修改的文档！");
        //        return;
        //    }
        //    DocumentMaster master = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
        //    IList docFileList = new ArrayList();
        //    foreach (DataGridViewRow row in dgDocumentDetail.Rows)
        //    {
        //        DocumentDetail dtl = row.Tag as DocumentDetail;
        //        docFileList.Add(dtl);
        //    }
        //    VDocumentPublicModify frm = new VDocumentPublicModify(master, docFileList);
        //    frm.ShowDialog();
        //    DocumentMaster resultMaster = frm.Result;
        //    if (resultMaster == null) return;
        //    AddDgDocumentMastInfo(resultMaster, dgDocumentMast.SelectedRows[0].Index);
        //    dgDocumentMast_CellClick(dgDocumentMast, new DataGridViewCellEventArgs(2, dgDocumentMast.SelectedRows[0].Index));
        //}
        ////删除文档
        //void btnDeleteDocumentMaster_Click(object sender, EventArgs e)
        //{
        //    if (dgDocumentMast.SelectedRows.Count == 0)
        //    {
        //        MessageBox.Show("请选择要删除的文档！");
        //        return;
        //    }
        //    if (MessageBox.Show("要删除当前文档吗？该操作将连它的所有文件一同删除！", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //    {
        //        DocumentMaster mas = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
        //        IList list = new ArrayList();
        //        list.Add(mas);

        //        if (msrv.Delete(list))
        //        {
        //            MessageBox.Show("删除成功！");
        //            dgDocumentMast.Rows.Remove(dgDocumentMast.SelectedRows[0]);
        //            if (dgDocumentMast.Rows.Count > 0)
        //            {
        //                dgDocumentMast_CellClick(dgDocumentMast, new DataGridViewCellEventArgs(2, dgDocumentMast.SelectedRows[0].Index));
        //            }
        //            else
        //            {
        //                dgDocumentDetail.Rows.Clear();
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("删除失败！");
        //        }
        //    }
        //}
        //void dgDocumentMast_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    dgDocumentDetail.Rows.Clear();
        //    DocumentMaster docMaster = dgDocumentMast.Rows[e.RowIndex].Tag as DocumentMaster;
        //    ObjectQuery oq = new ObjectQuery();
        //    oq.AddCriterion(Expression.Eq("Master.Id", docMaster.Id));
        //    oq.AddFetchMode("TheFileCabinet", NHibernate.FetchMode.Eager);
        //    IList list = model.ObjectQuery(typeof(DocumentDetail), oq);
        //    if (list != null && list.Count > 0)
        //    {
        //        foreach (DocumentDetail docDetail in list)
        //        {
        //            AddDgDocumentDetailInfo(docDetail);
        //        }
        //    }
        //}

        //#region 列表里添加数据
        //void AddDgDocumentMastInfo(DocumentMaster m)
        //{
        //    int rowIndex = dgDocumentMast.Rows.Add();
        //    AddDgDocumentMastInfo(m, rowIndex);
        //}
        //void AddDgDocumentMastInfo(DocumentMaster m, int rowIndex)
        //{
        //    dgDocumentMast[colDocumentName.Name, rowIndex].Value = m.Name;
        //    dgDocumentMast[colCreateTime.Name, rowIndex].Value = m.CreateTime;
        //    dgDocumentMast[colDocumentCode.Name, rowIndex].Value = m.Code;
        //    dgDocumentMast[colDocumentInforType.Name, rowIndex].Value = m.DocType.ToString();
        //    dgDocumentMast[colDocumentState.Name, rowIndex].Value = ClientUtil.GetDocStateName(m.State);
        //    dgDocumentMast[colOwnerName.Name, rowIndex].Value = m.OwnerName;
        //    dgDocumentMast.Rows[rowIndex].Tag = m;
        //}

        //void AddDgDocumentDetailInfo(DocumentDetail d)
        //{
        //    int rowIndex = dgDocumentDetail.Rows.Add();
        //    AddDgDocumentDetailInfo(d, rowIndex);
        //}
        //void AddDgDocumentDetailInfo(DocumentDetail d, int rowIndex)
        //{
        //    dgDocumentDetail[FileName.Name, rowIndex].Value = d.FileName;
        //    //dgDocumentDetail[FileExtension.Name, rowIndex].Value = d.ExtendName;
        //    //dgDocumentDetail[FilePath.Name, rowIndex].Value = d.FilePartPath;
        //    dgDocumentDetail.Rows[rowIndex].Tag = d;
        //}
        //#endregion

        ////反选
        //void lnkCheckAllNot_Click(object sender, EventArgs e)
        //{
        //    foreach (DataGridViewRow row in dgDocumentDetail.Rows)
        //    {
        //        row.Cells[FileSelect.Name].Value = false;
        //    }
        //}
        ////全选
        //void lnkCheckAll_Click(object sender, EventArgs e)
        //{
        //    foreach (DataGridViewRow row in dgDocumentDetail.Rows)
        //    {
        //        row.Cells[FileSelect.Name].Value = true;
        //    }
        //}
        //#endregion
        
    }
}
