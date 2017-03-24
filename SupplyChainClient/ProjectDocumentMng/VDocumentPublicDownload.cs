using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using System.IO;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Diagnostics;
using Excel;
using IRPServiceModel.Domain.Document;
using IRPServiceModel.Basic;
using VirtualMachine.Component.Util;


namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng
{
    public partial class VDocumentPublicDownload : TBasicDataView
    {
        public VDocumentPublicDownload()
        {
            InitializeComponent();
            InitEvent();
        }
        /// <summary>
        /// 批量下载（传过来的是 文档对象）
        /// </summary>
        /// <param name="downList">文档对象列表</param>
        /// <param name="irpOrKb"></param>
        public VDocumentPublicDownload(IList downList)
        {
            InitializeComponent();
            InitEvent();
            ShowDownList(downList);
        }

        private void InitEvent()
        {
            this.btnBrowse.Click += new EventHandler(btnBrowse_Click);
            this.btnBeginDownload.Click += new EventHandler(btnBeginDownload_Click);
            this.btnQuit.Click += new EventHandler(btnQuit_Click);
            this.btnOpenFolder.Click += new EventHandler(btnOpenFolder_Click);
        }

        void btnOpenFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = txtFilesURL.Text.Trim();
                if (filePath != "" && Directory.Exists(filePath))
                    System.Diagnostics.Process.Start("Explorer.exe", filePath);
            }
            catch { }
        }
        /// <summary>
        /// 显示下载文档列表
        /// </summary>
        /// <param name="downList"></param>
        void ShowDownList(IList downList)
        {
            dgvDownList.Rows.Clear();
            if (downList != null && downList.Count > 0)
            {
                foreach (DocumentDetail dtl in downList)
                {
                    int rowIndex = this.dgvDownList.Rows.Add();
                    dgvDownList[colDocumentName.Name, rowIndex].Value = dtl.FileName;
                    dgvDownList[colDownloadState.Name, rowIndex].Value = "等待下载";
                    this.dgvDownList.Rows[rowIndex].Tag = dtl;
                }

            }
        }

        //开始下载
        void btnBeginDownload_Click(object sender, EventArgs e)
        {
            if (txtFilesURL.Text == "")
            {
                MessageBox.Show("目标文件夹路径不能为空！");
            }
            else
            {
                if (!Directory.Exists(txtFilesURL.Text))
                {
                    MessageBox.Show("文件路径不存在！请重新选择！");
                    return;
                }
                foreach (DataGridViewRow row in dgvDownList.Rows)
                {
                    try
                    {
                        DocumentDetail docFile = row.Tag as DocumentDetail;
                        //string address = "http://10.70.18.203//TestFile//Files//0001//0001//0001.0001.000027V_特效.jpg";
                        string baseAddress = @docFile.TheFileCabinet.TransportProtocal.ToString().ToLower() + "://" + docFile.TheFileCabinet.ServerName + "/" + docFile.TheFileCabinet.Path + "/";
                        string address = baseAddress + docFile.FilePartPath;
                        int statrIndex = docFile.FilePartPath.LastIndexOf("/");
                        int length = docFile.FilePartPath.Length - statrIndex;
                        string downloadPath = txtFilesURL.Text + docFile.FilePartPath.Substring(statrIndex, length);
                        UtilityClass.WebClientObj.DownloadFile(address, downloadPath);
                        row.Cells[colDownloadState.Name].Value = "成功";
                    }
                    catch (Exception ex)
                    {
                        row.Cells[colDownloadState.Name].Value = "失败";
                        row.Cells[colError.Name].Value = ExceptionUtil.ExceptionMessage(ex);
                    }
                }
            }
        }


        #region 选择目标文件夹
        //选择目标文件夹
        void btnBrowse_Click(object sender, EventArgs e)
        {
            string path = GetfilePath();
            if (path != "")
            {
                txtFilesURL.Text = path;
            }
        }

        /// <summary>
        /// 选择目标文件夹
        /// </summary>
        /// <returns></returns>
        string GetfilePath()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            return fbd.SelectedPath;
        }
        #endregion

        //放弃
        void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
