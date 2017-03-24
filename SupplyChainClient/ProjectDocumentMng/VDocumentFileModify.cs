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
    public partial class VDocumentFileModify : TBasicDataView
    {
        IDocumentSrv model = null;
        private DocumentMaster master = null;
        private IList resultList;

        public IList ResultList
        {
            get { return resultList; }
            set { resultList = value; }
        }
        public VDocumentFileModify(DocumentMaster mas)
        {
            InitializeComponent();
            master = mas;
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
            btnBatchUpload.Click += new EventHandler(btnBatchUpload_Click);
            btnQuit.Click += new EventHandler(btnQuit_Click);
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "所有文件(*.*)|*.*";
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] strFiles = openFileDialog1.FileNames;
                txtFilePath.Text = strFiles[0];
            }
        }

        private void btnBatchUpload_Click(object sender, EventArgs e)
        {
            if (txtFilePath.Text == "")
            {
                MessageBox.Show("未选择修改文件！");
                return;
            }
            CurrentProjectInfo project = StaticMethod.GetProjectInfo();
            FileCabinet appFileCabinet = null;
            appFileCabinet = StaticMethod.GetDefaultFileCabinet();
            IList docFileList = new ArrayList();

            DocumentDetail dtl = new DocumentDetail();
            dtl.Master = master;

            string filePath = txtFilePath.Text; //row.Cells[FilePath.Name].Value.ToString();
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
            else
            {
                MessageBox.Show("文件不存在，请重新选择！");
                return;
            }

            dtl.ExtendName = Path.GetExtension(filePath);
            dtl.FileName = Path.GetFileName(filePath);
            dtl.TheFileCabinet = appFileCabinet;
            docFileList.Add(dtl);
            resultList = model.SaveFileList(master, docFileList);
            this.Close();
        }

        void btnQuit_Click(object sender, EventArgs e)
        {
            resultList = null;
            this.Close();
        }

    }
}
