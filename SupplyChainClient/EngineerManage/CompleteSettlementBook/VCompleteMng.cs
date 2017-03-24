using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component.Util;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.CompleteSettlementBook.Domain;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.CompleteSettlementBook;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using CustomServiceClient.CustomWebSrv;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using System.IO;

namespace Application.Business.Erp.SupplyChain.Client.CompleteSettlementBook
{
    public partial class VCompleteMng : TMasterDetailView
    {
        private MCompleteMng model = new MCompleteMng();
        private CompleteInfo curBillMaster;
        CurrentProjectInfo projectInfo;
        string userName = string.Empty;
        string jobId = string.Empty;
        string fileObjectType = string.Empty;
        public VCompleteMng()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }
        public void InitEvents()
        {
            //相关文档
            btnDownLoadDocument.Click += new EventHandler(btnDownLoadDocument_Click);
            btnOpenDocument.Click += new EventHandler(btnOpenDocument_Click);
            btnUpdateDocument.Click += new EventHandler(btnUpdateDocument_Click);
            btnDeleteDocument.Click += new EventHandler(btnDeleteDocument_Click);
            btnUpFile.Click += new EventHandler(btnUpFile_Click);

        }
        public void InitData()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;   
        }
        #region 文档操作
        //文档上传按钮状态
        private void btnStates(int i)
        {
            if (i == 0)
            {
                btnDownLoadDocument.Enabled = false;
                btnOpenDocument.Enabled = false;
                btnUpdateDocument.Enabled = false;
                btnDeleteDocument.Enabled = false;
                btnUpFile.Enabled = false;
            }
            if (i == 1)
            {
                btnDownLoadDocument.Enabled = true;
                btnOpenDocument.Enabled = true;
                btnUpdateDocument.Enabled = true;
                btnDeleteDocument.Enabled = true;
                btnUpFile.Enabled = true;
            }
        }
        void FillDoc()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", curBillMaster.Id));
            IList listDocument = model.ObjectQuery(typeof(ProObjectRelaDocument), oq);
            if (listDocument != null && listDocument.Count > 0)
            {
                gridDocument.Rows.Clear();
                foreach (ProObjectRelaDocument doc in listDocument)
                {
                    InsertIntoGridDocument(doc);
                }
            }
        }
        private void InsertIntoGridDocument(ProObjectRelaDocument doc)
        {
            int index = gridDocument.Rows.Add();
            DataGridViewRow row = gridDocument.Rows[index];
            row.Cells[DocumentName.Name].Value = doc.DocumentName;
            row.Cells[DocumentCode.Name].Value = doc.DocumentCode;
            row.Cells[DocumentCateCode.Name].Value = doc.DocumentCateCode;
            row.Cells[DocumentDesc.Name].Value = doc.DocumentDesc;

            row.Tag = doc;
        }

        void btnDownLoadDocument_Click(object sender, EventArgs e)
        {
            if (gridDocument.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要下载的文档！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gridDocument.Focus();
                return;
            }
            IList relaDocList = new List<ProObjectRelaDocument>();
            foreach (DataGridViewRow row in gridDocument.SelectedRows)
            {
                ProObjectRelaDocument relaDoc = row.Tag as ProObjectRelaDocument;
                relaDocList.Add(relaDoc);
            }
            VDocumentDownloadByID vdd = new VDocumentDownloadByID(relaDocList);
            vdd.ShowDialog();
        }

        void btnOpenDocument_Click(object sender, EventArgs e)
        {
            if (gridDocument.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要打开的文档！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gridDocument.Focus();
                return;
            }

            List<PLMWebServices.ProjectDocument> docList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument>();
            PLMWebServices.ProjectDocument[] projectDocList = null;

            foreach (DataGridViewRow row in gridDocument.SelectedRows)
            {
                ProObjectRelaDocument relaDoc = row.Tag as ProObjectRelaDocument;
                PLMWebServices.ProjectDocument doc = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument();
                doc.EntityID = relaDoc.DocumentGUID;
                docList.Add(doc);
            }


            try
            {
                PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.DownLoadDocumentByIRP(docList.ToArray(), null, userName, jobId, null, out projectDocList);
                if (es != null)
                {
                    MessageBox.Show("下载失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                List<string> errorList = new List<string>();
                List<string> listFileFullPaths = new List<string>();
                if (projectDocList != null)
                {
                    string fileFullPath = AppDomain.CurrentDomain.BaseDirectory + "TempFilePreview\\" + Guid.NewGuid().ToString().Replace("-", "");
                    if (!Directory.Exists(fileFullPath))
                        Directory.CreateDirectory(fileFullPath);

                    for (int i = 0; i < projectDocList.Length; i++)
                    {
                        //byte[] by = listFileBytes[i] as byte[];
                        //if (by != null && by.Length > 0)
                        //{
                        string fileName = projectDocList[i].FileName;


                        if (projectDocList[i].FileDataByte == null || projectDocList[i].FileDataByte.Length <= 0 || fileName == null)
                        {
                            string strName = projectDocList[i].Code + projectDocList[i].Name;
                            errorList.Add(strName);
                            continue;
                        }

                        string tempFileFullPath = fileFullPath + @"\\" + fileName;

                        CreateFileFromByteAarray(projectDocList[i].FileDataByte, tempFileFullPath);

                        listFileFullPaths.Add(tempFileFullPath);
                        //}
                    }
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
                if (errorList != null && errorList.Count > 0)
                {
                    string str = "";
                    foreach (string s in errorList)
                    {
                        str += (s + ";");
                    }
                    MessageBox.Show(str + "这" + errorList.Count + "个文件，无法预览，文件不存在或未指定格式！");
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


        //修改文档
        void btnUpdateDocument_Click(object sender, EventArgs e)
        {
            if (gridDocument.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要修改的文档！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gridDocument.Focus();
                return;
            }
            IList relaDocList = new List<ProObjectRelaDocument>();
            foreach (DataGridViewRow row in gridDocument.SelectedRows)
            {
                ProObjectRelaDocument relaDoc = row.Tag as ProObjectRelaDocument;
                relaDocList.Add(relaDoc);
            }
            VDocumentListUpdate vdlu = new VDocumentListUpdate(projectInfo, relaDocList);
            vdlu.ShowDialog();
            IList resultRelaDocList = vdlu.ResultListDoc;

            foreach (DataGridViewRow row in gridDocument.SelectedRows)
            {
                gridDocument.Rows.RemoveAt(row.Index);
            }
            if (resultRelaDocList == null) return;
            foreach (ProObjectRelaDocument doc in resultRelaDocList)
            {
                int rowIndex = gridDocument.Rows.Add();
                gridDocument[DocumentName.Name, rowIndex].Value = doc.DocumentName;
                gridDocument[DocumentCateCode.Name, rowIndex].Value = doc.DocumentCateCode;
                gridDocument[DocumentCode.Name, rowIndex].Value = doc.DocumentCode;
                gridDocument[DocumentDesc.Name, rowIndex].Value = doc.DocumentDesc;
                gridDocument.Rows[rowIndex].Tag = doc;
            }
        }
        //删除文档
        void btnDeleteDocument_Click(object sender, EventArgs e)
        {
            if (gridDocument.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的文档！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gridDocument.Focus();
                return;
            }

            if (MessageBox.Show("确认要删除选择文档吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                IList relaDocList = new List<ProObjectRelaDocument>();
                List<string> docIds = new List<string>();
                List<PLMWebServices.ProjectDocument> proDocList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument>();
                PLMWebServices.ProjectDocument[] reultProdocList = null;
                PLMWebServices.ProjectDocument[] resultProDoc = null;

                foreach (DataGridViewRow row in gridDocument.SelectedRows)
                {
                    ProObjectRelaDocument relaDoc = row.Tag as ProObjectRelaDocument;
                    relaDocList.Add(relaDoc);
                    docIds.Add(relaDoc.DocumentGUID);
                }

                PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectDocumentByIRP(docIds.ToArray(), Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentQueryVersion.所有版本,
                    null, userName, jobId, null, out resultProDoc);
                if (es != null)
                {
                    MessageBox.Show("操作失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                for (int i = 0; i < resultProDoc.Length; i++)
                {
                    PLMWebServices.ProjectDocument doc = resultProDoc[i];
                    doc.State = Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentState.作废;
                    proDocList.Add(doc);
                }

                PLMWebServices.ErrorStack es1 = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.UpdateDocumentByIRP(proDocList.ToArray(), Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentUpdateMode.添加一个新版次文件,
                    null, userName, jobId, null, out reultProdocList);
                if (es1 != null)
                {
                    MessageBox.Show("操作失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                bool flag = model.DeleteProObjRelaDoc(relaDocList);
                if (flag)
                {
                    MessageBox.Show("删除成功！");
                    foreach (DataGridViewRow row in gridDocument.SelectedRows)
                    {
                        gridDocument.Rows.RemoveAt(row.Index);
                    }
                }
                else
                {
                    MessageBox.Show("删除失败！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //批量上传
        void btnUpFile_Click(object sender, EventArgs e)
        {
            if (curBillMaster.Id == null)
            {
                if (MessageBox.Show("当前业务对象还没保存，是否保存！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    try
                    {
                        if (!ViewToModel()) return;

                        bool flag = false;
                        if (string.IsNullOrEmpty(curBillMaster.Id))
                        {
                            flag = true;
                        }
                        curBillMaster = model.CompleteSrv.SaveComplete(curBillMaster);
                        LogData log = new LogData();
                        log.BillId = curBillMaster.Id;
                        log.BillType = "竣工结算表";
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
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(ex));
                    }
                }
            }
            if (curBillMaster.Id != null)
            {
                VDocumentUploadList vdul = new VDocumentUploadList(projectInfo, curBillMaster, curBillMaster.Id);
                vdul.ShowDialog();
                IList resultDocumentList = vdul.ResultListDoc;
                if (resultDocumentList == null) return;
                //gridDocument.Rows.Clear();
                foreach (ProObjectRelaDocument doc in resultDocumentList)
                {
                    int rowIndex = gridDocument.Rows.Add();
                    gridDocument[DocumentName.Name, rowIndex].Value = doc.DocumentName;
                    gridDocument[DocumentCode.Name, rowIndex].Value = doc.DocumentCode;
                    gridDocument[DocumentCateCode.Name, rowIndex].Value = doc.DocumentCateCode;
                    gridDocument[DocumentDesc.Name, rowIndex].Value = doc.DocumentDesc;
                    gridDocument.Rows[rowIndex].Tag = doc;
                }
            }
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

            if (msg.IndexOf("不能在具有唯一索引") > -1 && msg.IndexOf("插入重复键") > -1)
            {
                msg = "已存在同名文档，请重命名该文档名称.";
            }

            return msg;
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


        #endregion
        
        public void InitEvent()
        {
            txtSubmitMoney.tbTextChanged += new EventHandler(txtSubmitMoney_tbTextChanged);
            //txtContractDocName.tbTextChanged += new EventHandler(txtContractDocName_tbTextChanged);
            txtEndMoney.tbTextChanged+=new EventHandler(txtEndMoney_tbTextChanged);
            txtSureMoney.tbTextChanged+=new EventHandler(txtSureMoney_tbTextChanged);
            txtBeginMoney.tbTextChanged += new EventHandler(txtBeginMoney_tbTextChanged);
            txtMoney.tbTextChanged+=new EventHandler(txtMoney_tbTextChanged);
            txtRealCost.tbTextChanged+=new EventHandler(txtRealCost_tbTextChanged);
            txtbenefit.tbTextChanged+=new EventHandler(txtbenefit_tbTextChanged);
            txtbenefit1.tbTextChanged+=new EventHandler(txtbenefit1_tbTextChanged);
            //相关文档
            btnDownLoadDocument.Click += new EventHandler(btnDownLoadDocument_Click);
            btnOpenDocument.Click += new EventHandler(btnOpenDocument_Click);
            btnUpdateDocument.Click += new EventHandler(btnUpdateDocument_Click);
            btnDeleteDocument.Click += new EventHandler(btnDeleteDocument_Click);
            btnUpFile.Click += new EventHandler(btnUpFile_Click);
        }
        /// <summary>
        /// 保存数据前校验数据
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            string validMessage = "";
            bool validity = true;
            if (txtContractDocName.Text.Equals(""))
            {
                MessageBox.Show("文档名称不能为空！");
                return false;
            }
            if (txtSubmitMoney.Text.Equals(""))
            {
                MessageBox.Show("报送总金额不能为空！");
                return false;
            }
            decimal EndMoney=ClientUtil.ToDecimal(txtSubmitMoney.Text);
           
            if (txtEndMoney.Text.Equals(""))
            {
                MessageBox.Show("争取结算金额不能为空！");
                return false;
            }
            if (txtSureMoney.Text.Equals(""))
            {
                MessageBox.Show("确保结算金额不能为空！");
                return false;
            }
            if (txtBeginMoney.Text.Equals(""))
            {
                MessageBox.Show("初次审定结算金额不能为空！");
                return false;
            }
            if (txtMoney.Text.Equals(""))
            {
                MessageBox.Show("审定总金额不能为空！");
                return false;
            }
            if (txtRealCost.Text.Equals(""))
            {
                MessageBox.Show("实际成本不能为空！");
                return false;
            }
            if (txtbenefit.Text.Equals(""))
            {
                MessageBox.Show("效益额不能为空！");
                return false;
            }
            if (txtbenefit1.Text.Equals(""))
            {
                MessageBox.Show("效益率不能为空！");
                return false;
            }
            return true;
        }

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
                    curBillMaster = model.CompleteSrv.GetCompleteById(Id);
                    ModelToView();
                    RefreshState(MainViewState.Browser);
                    btnDownLoadDocument.Enabled = true;
                    btnOpenDocument.Enabled = true;
                    //判断是否为制单人
                    PersonInfo pi = this.txtHandlePerson.Tag as PersonInfo;
                    string perid = ConstObject.LoginPersonInfo.Id;
                    if (pi != null && !pi.Id.Equals(perid))
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

        ///数据验证
        /// <summary>
        /// 报送总金额
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtSubmitMoney_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string strDate = txtSubmitMoney.Text.ToString();
            validity = CommonMethod.VeryValid(strDate);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                txtSubmitMoney.Text = "";
            }
            else if (!(ClientUtil.TransToDecimal(this.txtSubmitMoney.Text) >= 0))
            {
                MessageBox.Show("报送总金额：大于等于0！");
                txtSubmitMoney.Text = "";
            }
        }
        
       //争取结算金额
        void txtEndMoney_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string strDate = txtEndMoney.Text.ToString();
            validity = CommonMethod.VeryValid(strDate);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                txtEndMoney.Text = "";
            }
            else if (!(ClientUtil.TransToDecimal(this.txtEndMoney.Text) >= 0))
            {
                MessageBox.Show("争取结算金额：大于等于0！");
                txtEndMoney.Text = "";
            }
        }
        //确保结算金额
        void txtSureMoney_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string strDate = txtSureMoney.Text.ToString();
            validity = CommonMethod.VeryValid(strDate);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                txtSureMoney.Text = "";
            }
            else if (!(ClientUtil.TransToDecimal(this.txtSureMoney.Text) >= 0))
            {
                MessageBox.Show("确保结算金额：大于等于0！");
                txtSureMoney.Text = "";
            }
        }
        //初次审定金额
        void txtBeginMoney_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string strDate = txtBeginMoney.Text.ToString();
            validity = CommonMethod.VeryValid(strDate);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                txtBeginMoney.Text = "";
            }
            else if (!(ClientUtil.TransToDecimal(this.txtBeginMoney.Text) >= 0))
            {
                MessageBox.Show("初次审定金额：大于等于0！");
                txtBeginMoney.Text = "";
            }
        }
        //审定总金额
        void txtMoney_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string strDate = txtMoney.Text.ToString();
            validity = CommonMethod.VeryValid(strDate);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                txtMoney.Text = "";
            }
            else if (!(ClientUtil.TransToDecimal(this.txtMoney.Text) >= 0))
            {
                MessageBox.Show("审定总金额：大于等于0！");
                txtMoney.Text = "";
            }
            //计算效益额的值
            decimal Money = ClientUtil.TransToDecimal(this.txtMoney.Text);
            if(txtRealCost.Text.Trim()!="")
            {
                decimal realcost = ClientUtil.ToDecimal(txtRealCost.Text.Trim());
                txtbenefit.Text = decimal.Round(Money-realcost).ToString();
            }
           //计算效益率的值
            if(txtbenefit.Text.Trim()!="")
            {
                decimal benefit = ClientUtil.ToDecimal(txtbenefit.Text.Trim());
                if (Money != 0)
                {
                    txtbenefit1.Text = decimal.Round(benefit / Money*100).ToString();
                }
            }
        }
        //实际成本
        void txtRealCost_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string strDate = txtRealCost.Text.ToString();
            validity = CommonMethod.VeryValid(strDate);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                txtRealCost.Text = "";
                return;
            }
            else if (!(ClientUtil.TransToDecimal(this.txtRealCost.Text) >= 0))
            {
                MessageBox.Show("实际成本：大于等于0！");
                txtRealCost.Text = "";
                return;
            }


            Decimal realCost= ClientUtil.TransToDecimal(this.txtRealCost.Text);

            //if(txtMoney.Text.Trim()!="")
            //{
            //    try { 
            //        decimal money= ClientUtil.ToDecimal(txtMoney.Text.Trim());
                    
            //        if(realCost!=0)
            //        txtbenefit1.Text =decimal.Round(money / realCost,3).ToString();
            //    }
            //    catch { }
            //}

           
            if (txtMoney.Text.Trim() != "")
            {
                try
                {
                    decimal Money = ClientUtil.ToDecimal(txtMoney.Text.Trim());
                    txtbenefit.Text = decimal.Round(Money - realCost).ToString();
                }
                catch
                {

                }

            }
        }
        //效益额
        void txtbenefit_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string strDate = txtbenefit.Text.ToString();
            validity = CommonMethod.VeryValid(strDate);
            //if (validity == false)
            //{
            //    MessageBox.Show("请输入数字！");
            //    txtbenefit.Text = "";
            //    return;
            //}
            //else if (!(ClientUtil.TransToDecimal(this.txtbenefit.Text) >= 0) )
            //{
            //    MessageBox.Show("效益额：大于等于0！");
            //    txtbenefit.Text = "";
            //    return;
            //}
            decimal benefit = ClientUtil.TransToDecimal(this.txtbenefit.Text);
            if(txtMoney.Text.Trim()!="")
            {
                try {
                    decimal Money = ClientUtil.ToDecimal(txtMoney.Text.Trim());
                    if (Money != 0)
                    {
                        txtbenefit1.Text = decimal.Round(benefit/Money,3).ToString();
                    }
                }
                catch { }
            }
           
        }
        //效益率
        void txtbenefit1_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string strDate = txtbenefit1.Text.ToString();
            validity = CommonMethod.VeryValid(strDate);
          
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                txtbenefit1.Text = "";
            }
            else if (!(ClientUtil.TransToDecimal(this.txtbenefit1.Text) >= 0 ))
            {
                //MessageBox.Show("效益率：大于等于0！");
                txtbenefit1.Text = "";
                return;
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
                //btnStates(0);
                curBillMaster = new CompleteInfo();
                txtHandlePerson.Tag = ConstObject.LoginPersonInfo;
                txtHandlePerson.Text = ConstObject.LoginPersonInfo.Name;
                //归属项目
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                if (projectInfo != null)
                {
                    txtProjectName.Tag = projectInfo;
                    txtProjectName.Text = projectInfo.Name;
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
                }
                //txtDate.Value = DateTime.Now;
                //DateTime strDate = Convert.ToDateTime(txtDate.Value.Date);
                //SearchCompleteManage(strDate);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            return true;
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
                //curBillMaster.AuditPerson = ConstObject.LoginPersonInfo;//制单人编号
                //curBillMaster.AuditPersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                //curBillMaster.AuditDate = ConstObject.LoginDate;//制单时间
                //curBillMaster.AuditYear = ConstObject.LoginDate.Year;//制单年
                //curBillMaster.AuditMonth = ConstObject.LoginDate.Month;//制单月
                curBillMaster = model.CompleteSrv.SaveComplete(curBillMaster);
                ////插入日志
                //txtCode.Text = curBillMaster.Code;
                ////更新Caption
                //this.ViewCaption = ViewName + "-" + txtCode.Text;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
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
                curBillMaster = model.CompleteSrv.SaveComplete(curBillMaster);
                LogData log = new LogData();
                log.BillId = curBillMaster.Id;
                log.BillType = "竣工结算表";
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
                //this.ViewCaption = ViewName + "-" + txtDate.Value.Date.ToShortDateString();
                MessageBox.Show("保存成功！");
                //btnStates(0);
                //ClearView();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                curBillMaster.HandlePersonName=ClientUtil.ToString(txtHandlePerson.Text);
                curBillMaster.CreateDate = ClientUtil.ToDateTime(dtpcreatetime.Text);
                curBillMaster.PlanTime =ClientUtil.ToDateTime(dtpPlanEndTime.Text);
                curBillMaster.EndTime =ClientUtil.ToDateTime(dtpRealEndTime.Text);

                decimal submitMoney = ClientUtil.ToDecimal(txtSubmitMoney.Text);
                if (submitMoney > 1000000000000)
                {
                    MessageBox.Show("你输入的报送总金额太大了，请重新输入！");
                    return false;
                }
                submitMoney = submitMoney * 10000;
                curBillMaster.SubmitMoney = ClientUtil.ToDecimal(submitMoney);

                decimal EndMoney = ClientUtil.ToDecimal(txtEndMoney.Text);
                if (EndMoney > 1000000000000)
                {
                    MessageBox.Show("你输入的争取结算金额太大了，请重新输入！");
                    return false;
                }
                EndMoney = EndMoney * 10000;
                curBillMaster.ZhenquMoney = ClientUtil.ToDecimal(EndMoney);

                decimal sureMoney = ClientUtil.ToDecimal(txtSureMoney.Text);
                if (sureMoney > 1000000000000)
                {
                    MessageBox.Show("你输入的金额太大了，请重新输入！");
                    return false;
                }
                sureMoney = sureMoney * 10000;
                curBillMaster.SureMoney = ClientUtil.ToDecimal(sureMoney);

                decimal beginMoney = ClientUtil.ToDecimal(txtBeginMoney.Text);
                if (beginMoney > 1000000000000)
                {
                    MessageBox.Show("你输入的初次审定总金额太大了！");
                    return false;
                }
                beginMoney = beginMoney * 10000;
                curBillMaster.BeginMoney = ClientUtil.ToDecimal(beginMoney);

                decimal Money = ClientUtil.ToDecimal(txtMoney.Text);
                if (Money > 1000000000000)
                {
                    MessageBox.Show("你输入的金额太大了，请重新输入");
                    return false;
                }
                Money = Money * 10000;
                curBillMaster.ShendingMoney = ClientUtil.ToDecimal(Money);

                decimal realCost = ClientUtil.ToDecimal(txtRealCost.Text);
                if (realCost > 1000000000000)
                {
                    MessageBox.Show("你输入的金额太大了，请重新输入！");
                    return false;
                }
                realCost = realCost * 10000;
                curBillMaster.RealMoney = ClientUtil.ToDecimal(realCost);

                decimal benefit = ClientUtil.ToDecimal(txtbenefit.Text);
                if (benefit > 1000000000000)
                {
                    MessageBox.Show("你输入的金额太大了，请重新输入！");
                    return false;
                }
                benefit = benefit * 10000;
                curBillMaster.Benefit = ClientUtil.ToDecimal(benefit);


                curBillMaster.Bennefitlv = ClientUtil.ToDecimal(txtbenefit1.Text);

                curBillMaster.ContractDocName = ClientUtil.ToString(txtContractDocName.Text);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }
        
        //显示数据
        private bool ModelToView()
        {
            try
            {
                this.txtProjectName.Text = ClientUtil.ToString(curBillMaster.ProjectName);
                this.txtProjectName.Tag = curBillMaster.ProjectId;
                this.txtHandlePerson.Text = ClientUtil.ToString(curBillMaster.HandlePersonName);
                this.dtpcreatetime.Text = curBillMaster.CreateDate.ToShortDateString();
                this.dtpPlanEndTime.Text = curBillMaster.PlanTime.ToShortDateString();
                this.dtpRealEndTime.Text = curBillMaster.EndTime.ToShortDateString();

                decimal submitMoney = ClientUtil.ToDecimal(curBillMaster.SubmitMoney);
                submitMoney = submitMoney / 10000;
                this.txtSubmitMoney.Text = ClientUtil.ToString(submitMoney);

                decimal zhengquMoney = ClientUtil.ToDecimal(curBillMaster.ZhenquMoney);
                zhengquMoney = zhengquMoney / 10000;
                this.txtEndMoney.Text = ClientUtil.ToString(zhengquMoney);

                decimal sureMoney = ClientUtil.ToDecimal(curBillMaster.SureMoney);
                sureMoney = sureMoney / 10000;
                this.txtSureMoney.Text = ClientUtil.ToString(sureMoney);

                decimal beginMoney = ClientUtil.ToDecimal(curBillMaster.BeginMoney);
                beginMoney = beginMoney / 10000;
                this.txtBeginMoney.Text = ClientUtil.ToString(beginMoney);

                decimal shenDingMoney = ClientUtil.ToDecimal(curBillMaster.ShendingMoney);
                shenDingMoney = shenDingMoney / 10000;
                this.txtMoney.Text = ClientUtil.ToString(shenDingMoney);

                decimal realCost = ClientUtil.ToDecimal(curBillMaster.RealMoney);
                realCost = realCost / 10000;
                this.txtRealCost.Text = ClientUtil.ToString(realCost);

                decimal benefit = ClientUtil.ToDecimal(curBillMaster.Benefit);
                benefit = benefit / 10000;
                this.txtbenefit.Text = ClientUtil.ToString(benefit);

                this.txtbenefit1.Text = ClientUtil.ToString(curBillMaster.Bennefitlv);
                this.txtContractDocName.Text = ClientUtil.ToString(curBillMaster.ContractDocName);
                FillDoc();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        ////[optrType=1 保存][optrType=2 提交]
        //private bool SaveOrSubmitBill(int optrType)
        //{
        //    if (!ViewToModel())
        //        return false;

        //    LogData log = new LogData();
        //    if (string.IsNullOrEmpty(curBillMaster.Id))
        //    {
        //        if (optrType == 2)
        //        {
        //            log.OperType = "新增提交";
        //        }
        //        else
        //        {
        //            log.OperType = "新增保存";
        //        }

        //    }
        //    else
        //    {
        //        if (optrType == 2)
        //        {
        //            log.OperType = "修改提交";
        //        }
        //        else
        //        {
        //            log.OperType = "修改保存";
        //        }
        //    }
        //    if (optrType == 2)
        //    {
        //        curBillMaster.DocState = DocumentState.InAudit;
        //        curBillMaster.DocState = DocumentState.InExecute;
        //        curBillMaster.AuditPerson = ConstObject.LoginPersonInfo;//制单人编号
        //        curBillMaster.AuditPersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
        //        curBillMaster.AuditDate = ConstObject.LoginDate;//制单时间
        //        curBillMaster.AuditYear = ConstObject.LoginDate.Year;//制单年
        //        curBillMaster.AuditMonth = ConstObject.LoginDate.Month;//制单月
        //    }
        //    curBillMaster = model.StockInventorySrv.SaveStockInventory(curBillMaster, movedDtlList);
        //    this.txtCode.Text = curBillMaster.Code;
        //    log.BillId = curBillMaster.Id;
        //    log.BillType = "月度盘点单";
        //    log.Code = curBillMaster.Code;
        //    log.Descript = "";
        //    log.OperPerson = ConstObject.LoginPersonInfo.Name;
        //    log.ProjectName = curBillMaster.ProjectName;
        //    StaticMethod.InsertLogData(log);
        //    this.ViewCaption = ViewName + "-" + txtCode.Text;
        //    return true;
        //}


        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
            if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
            {
                base.ModifyView();
                curBillMaster = model.CompleteSrv.GetCompleteById(curBillMaster.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为非编辑状态，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(curBillMaster.DocState));
            MessageBox.Show(message);
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
                curBillMaster = model.CompleteSrv.GetCompleteById(curBillMaster.Id);


                if (model.CompleteSrv.DeleteByDao(curBillMaster))
                {
                    MessageBox.Show("删除成功！");
                    ClearView();
                }
                return true;
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
                        curBillMaster = model.CompleteSrv.GetCompleteById(curBillMaster.Id);
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
                //ObjectLock.Lock(pnlContent1, true);
            }

            ////永久锁定
            //object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate, txtHandlePerson, txtSumQuantity, txtProject };
            //ObjectLock.Lock(os);

            object[] lockCols = new object[] { txtProjectName, txtHandlePerson, dtpcreatetime};
            ObjectLock.Lock(lockCols);
        }
        /// <summary>
        /// 刷新
        /// </summary>
        public override void RefreshView()
        {
            try
            {
                //重新获得当前对象的值
                curBillMaster = model.CompleteSrv.GetCompleteById(curBillMaster.Id);
                //给界面赋值
                ModelToView();
            }
            catch (Exception e)
            {
                MessageBox.Show("数据刷新错误：" + ExceptionUtil.ExceptionMessage(e));
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
                    txtbenefit.Text = "";
                    txtbenefit1.Text = "";
                    dtpPlanEndTime.Text = ConstObject.LoginDate.ToString();
                    dtpRealEndTime.Text = ConstObject.LoginDate.ToString();
                    break;
                case MainViewState.Browser:
                    this.txtProjectName.ReadOnly = true;
                    this.txtHandlePerson.ReadOnly = true;
                    this.txtContractDocName.ReadOnly = true;
                    this.txtSubmitMoney.ReadOnly = true;
                    this.txtRealCost.ReadOnly = true;
                    this.txtEndMoney.ReadOnly = true;
                    this.txtSureMoney.ReadOnly = true;
                    this.txtBeginMoney.ReadOnly = true;
                    this.txtMoney.ReadOnly = true;
                    this.txtbenefit.ReadOnly = true;
                    this.txtbenefit1.ReadOnly = true;
                    this.dtpPlanEndTime.Enabled = false;
                    this.dtpcreatetime.Enabled = false;
                    this.dtpRealEndTime.Enabled = false;
                    break;
                 
                default:
                    break;
            }
        }
    }
}
