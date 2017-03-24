using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ImportIntegration;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using System.Reflection;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using IRPServiceModel.Services.Document;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using IRPServiceModel.Domain.Document;
using VirtualMachine.Core;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Core.Expression;

namespace Application.Business.Erp.SupplyChain.Client.FileUpload
{
    public partial class VDocumentFileUpload : TBasicDataView
    {
        IDocumentSrv model = null;
        private DocumentMaster master = null;
        private string isAddOrUpdate = string.Empty;
        private IList resultList;

        public IList ResultList
        {
            get { return resultList; }
            set { resultList = value; }
        }
        public VDocumentFileUpload(DocumentMaster mas,string AddOrUpdate)
        {
            InitializeComponent();
            master = mas;
            isAddOrUpdate = AddOrUpdate;
            InitForm();
        }

        private void InitForm()
        {
            InitEvents();

            if (model == null)
                model = ConstMethod.GetService("DocumentSrv") as IDocumentSrv;
        }
        private void InitEvents()
        {
            btnSelectFile.Click += new EventHandler(btnSelectFile_Click);
            btnClearSelected.Click += new EventHandler(btnClearSelected_Click);
            btnClearAll.Click += new EventHandler(btnClearAll_Click);
            btnBatchUpload.Click += new EventHandler(btnBatchUpload_Click);
            btnQuit.Click += new EventHandler(btnQuit_Click);
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "所有文件(*.*)|*.*";
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] strFiles = openFileDialog1.FileNames;
                int iCount = strFiles.Length;
                for (int i = 0; i < iCount; i++)
                {
                    InsertToGrid(strFiles[i]);
                }

                gridFiles.AutoResizeColumns();
            }
        }
        private const double KBCount = 1024;
        private const double MBCount = KBCount * 1024;
        private const double GBCount = MBCount * 1024;
        private const double TBCount = GBCount * 1024;

        /// <summary>
        /// 得到适应的大小
        /// </summary>
        /// <param name="path"></param>
        /// <returns>string</returns>
        public static string GetAutoSizeString(double size, int roundCount)
        {
            if (KBCount > size)
            {
                return Math.Round(size, roundCount) + "B";
            }
            else if (MBCount > size)
            {
                return Math.Round(size / KBCount, roundCount) + "KB";
            }
            else if (GBCount > size)
            {
                return Math.Round(size / MBCount, roundCount) + "MB";
            }
            else if (TBCount > size)
            {
                return Math.Round(size / GBCount, roundCount) + "GB";
            }
            else
            {
                return Math.Round(size / TBCount, roundCount) + "TB";
            }
        }

        private void InsertToGrid(string filePath)
        {
            int index = gridFiles.Rows.Add();
            DataGridViewRow row = gridFiles.Rows[index];
            row.Cells[FileName.Name].Value = Path.GetFileName(filePath);
            row.Cells[FilePath.Name].Value = filePath;

            FileInfo fileInfo = new FileInfo(filePath);
            row.Cells[FileSize.Name].Value = GetAutoSizeString(fileInfo.Length, 3);
        }

        #region 变量

        private DataTable sourceDataTable = new DataTable();
        private IntergrationFrameWork cFuntions = null;
        private string language = "zhs";

        #endregion

        private void btnBatchUpload_Click(object sender, EventArgs e)
        {
            if (isAddOrUpdate == "update")
            {
                if (gridFiles.Rows.Count > 1)
                {
                    MessageBox.Show("修改时只能选择一个文件！");
                    return;
                }
            }
            CurrentProjectInfo project = StaticMethod.GetProjectInfo();
            FileCabinet appFileCabinet = null;
            appFileCabinet = StaticMethod.GetDefaultFileCabinet();
            IList docFileList = new ArrayList();
            foreach (DataGridViewRow row in gridFiles.Rows)
            {
                DocumentDetail dtl = new DocumentDetail();
                dtl.Master = master;

                string filePath = row.Cells[FilePath.Name].Value.ToString();
                FileInfo file = new FileInfo(filePath);
                if (file.Exists)
                {
                    FileStream fileStream = file.OpenRead();
                    int FileLen = (int)file.Length;
                    Byte[] FileData = new Byte[FileLen];
                    //将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
                    fileStream.Read(FileData, 0, FileLen);

                    dtl.FileDataByte = FileData;
                }

                dtl.ExtendName = Path.GetExtension(filePath);
                dtl.FileName = Path.GetFileName(filePath);
                dtl.TheFileCabinet = appFileCabinet;
                docFileList.Add(dtl);
            }
            resultList = model.SaveFileList(master, docFileList);
            this.Close();
        }

        private void btnClearSelected_Click(object sender, EventArgs e)
        {
            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridFiles.SelectedRows)
            {
                listRowIndex.Add(row.Index);
            }
            listRowIndex.Sort();
            for (int i = listRowIndex.Count - 1; i > -1; i--)
            {
                gridFiles.Rows.RemoveAt(listRowIndex[i]);
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            for (int i = gridFiles.Rows.Count - 1; i > -1; i--)
            {
                gridFiles.Rows.RemoveAt(i);
            }
        }


        void btnQuit_Click(object sender, EventArgs e)
        {
            resultList = null;
            this.Close();
        }

    }
}
