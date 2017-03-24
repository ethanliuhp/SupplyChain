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
//测试
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using System.IO;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Diagnostics;
using Excel;


namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng
{
    public partial class VDocumentDownload : TBasicDataView
    {
        string userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
        string jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;

        string isIRPOrKB = string.Empty;

        public VDocumentDownload()
        {
            InitializeComponent();
            InitEvent();
        }
        /// <summary>
        /// 批量下载（传过来的是 文档对象）
        /// </summary>
        /// <param name="downList">文档对象列表</param>
        /// <param name="irpOrKb"></param>
        public VDocumentDownload(IList downList, string irpOrKb)
        {
            InitializeComponent();
            isIRPOrKB = irpOrKb;
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
            if (downList.Count > 0 && downList != null)
            {
                if (isIRPOrKB != "KB")
                {
                    foreach (PLMWebServices.ProjectDocument doc in downList)
                    {
                        int rowIndex = this.dgvDownList.Rows.Add();
                        dgvDownList[colDocumentName.Name, rowIndex].Value = doc.Name;
                        dgvDownList[colDownloadState.Name, rowIndex].Value = "等待下载";
                        this.dgvDownList.Rows[rowIndex].Tag = doc;
                    }
                }
                else
                {
                    foreach (PLMWebServicesByKB.ProjectDocument doc in downList)
                    {
                        int rowIndex = this.dgvDownList.Rows.Add();
                        dgvDownList[colDocumentName.Name, rowIndex].Value = doc.Name;
                        dgvDownList[colDownloadState.Name, rowIndex].Value = "等待下载";
                        this.dgvDownList.Rows[rowIndex].Tag = doc;
                    }
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
                //string s = txtFilesURL.Text;
                if (!Directory.Exists(txtFilesURL.Text))
                {
                    MessageBox.Show("文件路径不存在！请重新选择！");
                    return;
                }
                int i = 0;
                foreach (DataGridViewRow row in dgvDownList.Rows)
                {
                    dgvDownList[colDownloadState.Name, i].Value = "下载中";
                    if (isIRPOrKB != "KB")
                    {
                        List<PLMWebServices.ProjectDocument> listDocument = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument>();
                        PLMWebServices.ProjectDocument doc = row.Tag as PLMWebServices.ProjectDocument;
                        listDocument.Add(doc);
                        PLMWebServices.ProjectDocument[] listResult = null;
                        PLMWebServices.ErrorStack es = StaticMethod.DownLoadDocumentByIRP(listDocument.ToArray(), null, userName, jobId, null, out listResult);
                        if (es != null)
                        {
                            //MessageBox.Show("文件下载失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            dgvDownList[colDownloadState.Name, i].Value = "下载失败";
                            dgvDownList[colError.Name, i].Value = GetExceptionMessage(es);
                        }

                        if (listResult[0].FileDataByte != null)
                        {
                            string selectedDir = string.Empty;
                            string filename = txtFilesURL.Text + "\\" + listResult[0].Code + listResult[0].FileName;
                            CreateFileFromByteAarray(listResult[0].FileDataByte, filename);
                            dgvDownList[colDownloadState.Name, i].Value = "下载成功";
                        }
                        else
                        {
                            dgvDownList[colDownloadState.Name, i].Value = "下载失败";
                            dgvDownList[colError.Name, i].Value = "未找到文件";
                        }
                    }
                    else
                    {
                        List<PLMWebServicesByKB.ProjectDocument> listDocument = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.ProjectDocument>();
                        PLMWebServicesByKB.ProjectDocument doc = row.Tag as PLMWebServicesByKB.ProjectDocument;
                        listDocument.Add(doc);
                        PLMWebServicesByKB.ProjectDocument[] listResult = null;

                        PLMWebServicesByKB.ErrorStack es = StaticMethod.DownLoadDocumentByKB(listDocument.ToArray(), null,
                            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_UserName,
                            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_JobId, null, out listResult);
                        if (es != null)
                        {
                            //MessageBox.Show("文件下载失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            dgvDownList[colDownloadState.Name, i].Value = "下载失败";
                            dgvDownList[colError.Name, i].Value = GetExceptionMessage(es);
                        }

                        if (listResult[0].FileDataByte != null)
                        {
                            string selectedDir = string.Empty;
                            string filename = txtFilesURL.Text + "\\" + listResult[0].Code + listResult[0].FileName;
                            CreateFileFromByteAarray(listResult[0].FileDataByte, filename);
                            dgvDownList[colDownloadState.Name, i].Value = "下载成功";
                        }
                        else
                        {
                            dgvDownList[colDownloadState.Name, i].Value = "下载失败";
                            dgvDownList[colError.Name, i].Value = "未找到文件";
                        }
                    }
                    i++;
                }
                //if (HaseExcel())
                //{
                //    string downPath = txtFilesURL.Text + "\\批量下载文件" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                //    try
                //    {
                //        Excel(downPath);
                //    }
                //    catch
                //    {
                //        Txt();
                //    }
                //}
                //else
                //{
                //    Txt();
                //}

                //if (Directory.Exists(txtFilesURL.Text))
                //{
                //    Process.Start(txtFilesURL.Text);
                //}
            }
        }

        public static void CreateFileFromByteAarray(byte[] stream, string fileName)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                fs.Write(stream, 0, stream.Length);
                fs.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 错误
        private string GetExceptionMessage(PLMWebServicesByKB.ErrorStack es)
        {
            string msg = es.Message;
            PLMWebServicesByKB.ErrorStack esTemp = es.InnerErrorStack;
            while (esTemp != null && !string.IsNullOrEmpty(esTemp.Message))
            {
                msg += "；\n" + esTemp.Message;
                esTemp = esTemp.InnerErrorStack;
            }

            if ((msg.IndexOf("不能在具有唯一索引") > -1 && msg.IndexOf("插入重复键") > -1) || msg.IndexOf("违反唯一约束") > -1)
            {
                msg = "已存在同名文档，请重命名该文档名称.";
            }

            return msg;
        }

        private string GetExceptionMessage(PLMWebServices.ErrorStack es)
        {
            string msg = es.Message;
            PLMWebServices.ErrorStack esTemp = es.InnerErrorStack;
            while (esTemp != null && !string.IsNullOrEmpty(esTemp.Message))
            {
                msg += "；\n" + esTemp.Message;
                esTemp = esTemp.InnerErrorStack;
            }

            if ((msg.IndexOf("不能在具有唯一索引") > -1 && msg.IndexOf("插入重复键") > -1) || msg.IndexOf("违反唯一约束") > -1)
            {
                msg = "已存在同名文档，请重命名该文档名称.";
            }

            return msg;
        }
        #endregion

        /// <summary>
        /// Excel日志
        /// </summary>
        /// <param name="fileName">文件存放路径</param>
        void Excel(string fileName)
        {
            //string filename = "G:\\xx.xlsx";
            dgvDownList.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            Clipboard.Clear();
            dgvDownList.SelectAll();
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.DataGridViewToExcel(dgvDownList.GetClipboardContent(), fileName, false);
        }
        /// <summary>
        /// 文本
        /// </summary>
        void Txt()
        {
            string downPath = txtFilesURL.Text + "\\批量下载文件" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            StreamWriter write = new StreamWriter(downPath, true, Encoding.Default);
            foreach (DataGridViewRow row in dgvDownList.Rows)
            {
                string s = row.Cells[colDocumentName.Name].Value.ToString() + "    " + row.Cells[colDownloadState.Name].Value.ToString() + "    ";
                if (row.Cells[colError.Name].Value != null)
                {
                    s += row.Cells[colError.Name].Value.ToString();
                }
                write.WriteLine(s);
            }
            write.Close();
            write.Dispose();
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


        bool HaseExcel()
        {
            Excel.Application excelApplication = null;
            try
            {
                excelApplication = new ApplicationClass();
                return true;
            }
            catch
            {
                return false;//没有安装Excel
            }
            finally
            {
                if (excelApplication != null)
                {
                    //回收资源
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject((object)excelApplication);
                }
                excelApplication = null;
            }
        }

    }
}
