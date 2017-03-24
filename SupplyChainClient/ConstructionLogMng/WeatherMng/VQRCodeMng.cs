using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.WeatherManage.Domain;
using VirtualMachine.Core;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.Basic;
using System.IO;
using Application.Business.Erp.SupplyChain.Client.StockMng;

namespace Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.WeatherMng
{
    public partial class VQRCodeMng : TBasicDataView
    {
        private MWeatherMng model = new MWeatherMng();
        private MStockMng stockModel = new MStockMng();
        string flxTitle = "二维码打印";
        CurrentProjectInfo projectInfo;
        QRCodeBill curBillMaster = new QRCodeBill();

        public VQRCodeMng()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }
        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnFile.Click += new EventHandler(btnFile_Click);
            this.btnNew.Click += new EventHandler(btnNew_Click);
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.btnDel.Click += new EventHandler(btnDel_Click);
            this.btnPreview.Click += new EventHandler(btnPreview_Click);
            this.btnPrint.Click += new EventHandler(btnPrint_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.fGridDetail.PrintPage += new FlexCell.Grid.PrintPageEventHandler(btnPrintPage_Click);
            this.dgDetail.SelectionChanged += new EventHandler(dgDetail_SelectionChanged);
        }

        private void InitData()
        {
            this.dtpDateBegin.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null)
            {
                txtProject.Tag = projectInfo;
                txtProject.Text = projectInfo.Name;
            }
            txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
            txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;           
        }
        void btnPrintPage_Click(object sender, FlexCell.Grid.PrintPageEventArgs e)
        {
            if (e.Preview == false && e.PageNumber == 1)
            {
                stockModel.StockInSrv.UpdateBillPrintTimes(7, curBillMaster.Id);//回写次数                
            }
        }
        void dgDetail_SelectionChanged(object sender, EventArgs e)
        {
            curBillMaster = dgDetail.CurrentRow.Tag as QRCodeBill;
            if (curBillMaster == null) return;
            
            this.txtDescript.Text = curBillMaster.Descript;
            this.txtFile.Text = curBillMaster.FileName;
            this.txtType.Text = curBillMaster.BillType;
            this.txtCodeTitle.Text = curBillMaster.CodeTitle;
            this.txtCode.Text = curBillMaster.Code;
            this.txtCreatePerson.Text = curBillMaster.CreatePersonName;
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        void btnFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog.FileName;
                txtFile.Text = path;
                string fileName = Path.GetFileName(path);
                txtCode.Text = fileName.Substring(0,fileName.IndexOf('.'));
            }
        }

        void btnNew_Click(object sender, EventArgs e)
        {
            curBillMaster = new QRCodeBill();
            //创建人
            txtDescript.Text = "";
            txtCode.Text = "";
            txtFile.Text = "";
        }
        void btnSave_Click(object sender, EventArgs e)
        {          
            if (this.txtType.Text.Equals(""))
            {
                MessageBox.Show("标题不能为空！");
                return;
            }
            if (this.txtCodeTitle.Text.Equals(""))
            {
                MessageBox.Show("标识名称不能为空！");
                return;
            }
            if (this.txtCode.Text.Equals(""))
            {
                MessageBox.Show("标识内容不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(this.txtFile.Text))
            {
                MessageBox.Show("上传文件不能为空！");
                return;
            }
            curBillMaster.ProjectId = projectInfo.Id;
            curBillMaster.ProjectName = projectInfo.Name;
            curBillMaster.CreateDate = ConstObject.LoginDate;
            curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
            curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
            curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
            curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
            curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
            curBillMaster.DocState = DocumentState.Edit;//状态

            string filePath = this.txtFile.Text;
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                string fileName = Path.GetFileName(filePath);
                if (ClientUtil.ToString(curBillMaster.Id) == "" || curBillMaster.FileName != fileName
                    || (curBillMaster.FileName == fileName && !curBillMaster.FileLastTime.ToString().Equals(file.LastWriteTime.ToString())))
                {                   
                    FileStream fileStream = file.OpenRead();
                    int FileLen = (int)file.Length;
                    if (FileLen > 1024 * 1024 * 5)
                    {
                        MessageBox.Show("上传文件大小不能大于[5M]！");
                        return;
                    }
                    Byte[] FileData = new Byte[FileLen];
                    //将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
                    fileStream.Read(FileData, 0, FileLen);
                    string sysFileName = ClientUtil.ToString(curBillMaster.SysFileName);
                    if (sysFileName == "")
                    {
                        IFCGuidGenerator g = new IFCGuidGenerator();
                        sysFileName = g.GeneratorIFCGuid() + file.Extension;
                        curBillMaster.SysFileName = sysFileName;
                    }
                    else { 
                        //判断文件后缀是否相同
                        string currSuffix = file.Extension;
                        string billFileSuffix = sysFileName.Substring(sysFileName.IndexOf('.'));
                        if (currSuffix != billFileSuffix)
                        {
                            MessageBox.Show("上传文件类型不能改变,必须为[" + billFileSuffix + "]类型！");
                            this.txtFile.Text = curBillMaster.FileName;
                            txtCode.Text = curBillMaster.Code;
                            return;
                        }
                    }
                    Hashtable ht = model.UploadSingleFile(sysFileName, FileData, "PersonPicture");
                    //Hashtable ht = model.UploadPicture(sysFileName, FileData, "QRCode");
                    fileStream.Close();
                    curBillMaster.FileLastTime = file.LastWriteTime;
                    curBillMaster.FileName = fileName;
                    if (ht != null)
                    {
                        foreach (System.Collections.DictionaryEntry h in ht)
                        {
                            curBillMaster.FilecabinetId = h.Key.ToString();
                            curBillMaster.FileUrl = h.Value.ToString();
                        }
                    }
                }
            }
            curBillMaster.Code = txtCode.Text;
            curBillMaster.BillType = this.txtType.Text;
            curBillMaster.CodeTitle = txtCodeTitle.Text;
            curBillMaster.Descript = txtDescript.Text;
            curBillMaster = model.WeatherSrv.SaveOrUpdateByDao(curBillMaster) as QRCodeBill;
            LogData log = new LogData();
            log.BillId = curBillMaster.Id;
            log.BillType = "二维码";
            log.Code = curBillMaster.Code;
            log.OperType = "保存";
            log.Descript = "";
            log.OperPerson = ConstObject.LoginPersonInfo.Name;
            log.ProjectName = curBillMaster.ProjectName;
            StaticMethod.InsertLogData(log);
            MessageBox.Show("保存成功！");
        }

        void btnDel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(curBillMaster.Id))
            {
                MessageBox.Show("未保存的二维码信息不需删除！");
                return;
            }
            DialogResult dr = MessageBox.Show("是否删除此二维码信息？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (dr == DialogResult.No) return;
            if (ClientUtil.ToInt(curBillMaster.PrintTimes) > 0)
            {
                curBillMaster.DocState = DocumentState.Invalid;
                model.WeatherSrv.SaveOrUpdateByDao(curBillMaster);
            }
            else
            {
                model.WeatherSrv.DeleteByDao(curBillMaster);
            }
            LogData log = new LogData();
            log.BillId = curBillMaster.Id;
            log.BillType = "二维码";
            log.Code = curBillMaster.Code;
            log.OperType = "删除";
            log.Descript = "";
            log.OperPerson = ConstObject.LoginPersonInfo.Name;
            log.ProjectName = curBillMaster.ProjectName;
            StaticMethod.InsertLogData(log);
            txtDescript.Text = "";
            txtCode.Text = "";
            txtFile.Text = "";
            curBillMaster.Id = "";
            MessageBox.Show("删除成功！");
            QueryData();
        }

        void btnPreview_Click(object sender, EventArgs e)
        {
            
            bool isOk = LoadTempleteFile(flxTitle + ".flx");
            if (isOk == true)
            {
                this.fGridDetail.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            }
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            bool isOk = LoadTempleteFile(flxTitle + ".flx");
            if (isOk == true)
            {
                fGridDetail.Print();
            }
        }

        private bool LoadTempleteFile(string modelName)
        {
            if (string.IsNullOrEmpty(curBillMaster.Id))
            {
                MessageBox.Show("请先保存二维码相关信息！");
                return false;
            }
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式
                fGridDetail.OpenFile(path + "\\" + modelName);//载入格式
            }
            else
            {
                MessageBox.Show("未找到模板格式文件" + modelName);
                return false;
            }

            this.fGridDetail.Row(0).Visible = false;
            this.fGridDetail.Column(0).Visible = false;

            string qrcodestr = curBillMaster.FileUrl;

            //生成二维码
            Bitmap qrcodebmp = CommonUtil.GenDimensionalCode(qrcodestr);
            Bitmap log = this.fGridDetail.Images.Item("Image2").Picture;//小log
            //合并图片
            Graphics g = Graphics.FromImage(qrcodebmp);
            g.DrawImage(qrcodebmp, 0, 0, qrcodebmp.Width, qrcodebmp.Height);
            g.DrawImage(log, qrcodebmp.Width / 2 - log.Width / 2, qrcodebmp.Width / 2 - log.Width / 2, log.Width, log.Height);
            GC.Collect(); 

            this.fGridDetail.Images.Add(qrcodebmp, "qrcode");
            fGridDetail.Cell(3, 2).SetImage("qrcode");
            fGridDetail.Cell(3, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
            fGridDetail.Cell(3, 4).Text = curBillMaster.ProjectName;
            fGridDetail.Cell(4, 3).Text = curBillMaster.CodeTitle+": ";
            fGridDetail.Cell(4, 4).Text = curBillMaster.Code;
            fGridDetail.Cell(2, 3).Text = curBillMaster.BillType;

            fGridDetail.PageSetup.TopMargin = ClientUtil.Tofloat("0.1");
            fGridDetail.PageSetup.HeaderMargin = ClientUtil.Tofloat("0");
            fGridDetail.PageSetup.RightMargin = ClientUtil.Tofloat("0.1");
            fGridDetail.PageSetup.LeftMargin = ClientUtil.Tofloat("0.1");
            return true;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            QueryData();
        }

        private void QueryData()
        {
            try
            {
                ObjectQuery objectQuery = new ObjectQuery();
                objectQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value.Date));
                objectQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));
                objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                objectQuery.AddCriterion(Expression.Eq("DocState", DocumentState.Edit));
                if (projectInfo == null || projectInfo.Code == CommonUtil.CompanyProjectCode)
                {
                    objectQuery.AddCriterion(Expression.Eq("CreatePersonName", ConstObject.LoginPersonInfo.Name));
                }
                if (!string.IsNullOrEmpty(this.txtQCode.Text))
                {
                    objectQuery.AddCriterion(Expression.Like("Code", "%"+this.txtQCode.Text + "%"));
                }
                if (!string.IsNullOrEmpty(this.txtQType.Text))
                {
                    objectQuery.AddCriterion(Expression.Like("BillType", "%" + this.txtQType.Text + "%"));
                }
                objectQuery.AddOrder(NHibernate.Criterion.Order.Desc("RealOperationDate"));
                IList SearchResult = model.WeatherSrv.GetDomainByCondition(typeof(QRCodeBill), objectQuery);
                this.dgDetail.Rows.Clear();
                if (SearchResult.Count > 0 && SearchResult != null)
                {
                    foreach (QRCodeBill var in SearchResult)
                    {
                        int rowIndex = this.dgDetail.Rows.Add();
                        dgDetail.Rows[rowIndex].Tag = var;
                        dgDetail[colCreateDate.Name, rowIndex].Value = var.RealOperationDate;
                        dgDetail[this.colCode.Name, rowIndex].Value = var.Code;
                        dgDetail[this.colType.Name, rowIndex].Value = var.BillType;
                        dgDetail[this.colCostTile.Name, rowIndex].Value = var.CodeTitle;
                        dgDetail[this.colFileName.Name, rowIndex].Value = var.FileName;
                        dgDetail[this.colCreatePerson.Name, rowIndex].Value = var.CreatePersonName;
                        dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName((int)var.DocState);
                        dgDetail[this.colRemark.Name, rowIndex].Value = var.Descript;
                    }
                    dgDetail.CurrentCell = dgDetail[1, 0];
                    dgDetail_SelectionChanged(dgDetail, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
            finally
            {
                FlashScreen.Close();
            }
        }
    }
}
