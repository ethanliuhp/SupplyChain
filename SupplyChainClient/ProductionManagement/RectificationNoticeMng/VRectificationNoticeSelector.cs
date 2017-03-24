using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NHibernate.Criterion;
using CustomServiceClient.Basic.Template;
using Application.Business.Erp.SupplyChain.ProductionManagement.RectificationNoticeMng.Domain;
using System.Collections;
using VirtualMachine.Core;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using System.IO;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.PenaltyDeductionManagement.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.RectificationNoticeMng
{
    public partial class VRectificationNoticeSelector : TBasicDataView
    {
        private MRectificationNoticeMng model = new MRectificationNoticeMng();
        private RectificationNoticeMaster curBillMaster;

        public RectificationNoticeMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }
        CurrentProjectInfo projectInfo = null;
        string userName = string.Empty;
        string jobId = string.Empty;
        string fileObjectType = string.Empty;


        public VRectificationNoticeSelector()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        private void InitData()
        {
            this.dtpDateBeginBill.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
            this.dtpDateBeginBill.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.txtConclusion.Items.AddRange(new object[] { "整改中", "整改未通过", "整改通过" });
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
            List<string> listParams = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetUploadFileParamsByMBP_IRP();
            fileObjectType = listParams[0];
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            if (dgMaster.RowCount == 0)
            {
                btnDeleteDocument.Enabled = false;
                btnDownLoadDocument.Enabled = false;
                btnOpenDocument.Enabled = false;
                btnUpFile.Enabled = false;
                btnUpdateDocument.Enabled = false;
                return;
            }

        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearchBill.Focus();
        }

        private void InitEvent()
        {
            this.btnSearchBill.Click += new EventHandler(btnSearchBill_Click);
            txtAccepTypeBill.Items.AddRange(new object[] { "工程日常检查", "专业检查" });
            btnSelectBill.Click += new EventHandler(btnSelectBill_Click);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            btnOK.Click += new EventHandler(btnOK_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            txtInspection.TextChanged += new EventHandler(txtInspection_TextChanged);
            txtConclusion.TextChanged += new EventHandler(txtConclusion_TextChanged);
            dgDetailBill.SelectionChanged += new EventHandler(dgDetailBill_SelectionChanged);
            //相关文档
            btnDownLoadDocument.Click += new EventHandler(btnDownLoadDocument_Click);
            btnOpenDocument.Click += new EventHandler(btnOpenDocument_Click);
            btnUpdateDocument.Click += new EventHandler(btnUpdateDocument_Click);
            btnDeleteDocument.Click += new EventHandler(btnDeleteDocument_Click);
            btnUpFile.Click += new EventHandler(btnUpFile_Click);
            //tab页切换数据处理
            tabControl2.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
        }
        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl2.SelectedTab.Name == 明细操作区.Name)//明细操作区
            {

            }
            else if (tabControl2.SelectedTab.Name == 相关附件.Name)//相关文档              
            {
                if (CurBillMaster != null && !string.IsNullOrEmpty(CurBillMaster.Id))
                {
                    FillDoc();
                }
            }
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
                        //if (!ViewToModel()) return;
                        bool flag = false;
                        if (string.IsNullOrEmpty(curBillMaster.Id))
                        {
                            flag = true;
                        }
                        curBillMaster = model.RectificationNoticeSrv.SaveRectificationNotice(curBillMaster);
                        //curBillMaster = model.DemandPlanSrv.SaveDemandMasterPlan(curBillMaster);
                        txtCodeBeginBill.Text = curBillMaster.Code;
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
                        this.ViewCaption = ViewName + "-" + txtCodeBeginBill.Text;
                        FillDoc();
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
        void txtInspection_TextChanged(object sender, EventArgs e)
        {
            dgDetailBill.CurrentRow.Cells[colInsResultBill.Name].Value = txtInspection.Text.ToString();
        }
        void txtConclusion_TextChanged(object sender, EventArgs e)
        {
            dgDetailBill.CurrentRow.Cells[colInsConclusionBill.Name].Value = txtConclusion.Text.ToString();
        }
        //单据选择队伍
        void btnSelectBill_Click(object sender, EventArgs e)
        {
            DataGridViewRow theCurrentRow = this.dgDetailBill.CurrentRow;
            VContractExcuteSelector vmros = new VContractExcuteSelector();
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject engineerMaster = list[0] as SubContractProject;

            txtSuppilerBill.Text = engineerMaster.BearerOrgName;
            txtSuppilerBill.Tag = engineerMaster;
        }

        /// <summary>
        /// 主从表显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearchBill_Click(object sender, EventArgs e)
        {
            if (dgMaster.RowCount != 0)
            {
                btnDeleteDocument.Enabled = true;
                btnDownLoadDocument.Enabled = true;
                btnOpenDocument.Enabled = true;
                btnUpFile.Enabled = true;
                btnUpdateDocument.Enabled = true;
            }
            ObjectQuery objectQuery = new ObjectQuery();
            IList list = new ArrayList();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));

            //单据
            if (txtCodeBeginBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("Code", txtCodeBeginBill.Text, NHibernate.Criterion.MatchMode.Anywhere));
            }
            //业务时间
            objectQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBeginBill.Value.Date));
            objectQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEndBill.Value.AddDays(1).Date));
            //创建人
            if (txtCreatePersonBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("CreatePersonName", txtCreatePersonBill.Text, MatchMode.Anywhere));
            }
            //检查类型
            if (this.txtAccepTypeBill.SelectedItem != null)
            {
                int i = 0;
                if (this.txtAccepTypeBill.Text == "专业检查")
                {
                    i = 1;
                }
                if (this.txtAccepTypeBill.Text == "工程日常检查")
                {
                    i = 0;
                }
                objectQuery.AddCriterion(Expression.Eq("InspectionType", i));
            }
            //供货商
            if (this.txtSuppilerBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("SupplierUnitName", txtSuppilerBill.Text, MatchMode.Anywhere)); ;
            }
            objectQuery.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            try
            {
                list = model.RectificationNoticeSrv.GetRectificationNotice(objectQuery);
                dgMaster.Rows.Clear();
                dgDetailBill.Rows.Clear();
                ShowMasterList(list);
                //MessageBox.Show("查询完毕");
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
        }
        /// <summary>
        /// 显示主表
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList masterList)
        {
            dgMaster.Rows.Clear();
            dgDetailBill.Rows.Clear();
            if (masterList == null || masterList.Count == 0) return;
            foreach (RectificationNoticeMaster master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCodeBill.Name, rowIndex].Value = master.Code;
                int strType = master.InspectionType;   //检查类型
                if (strType == 0)
                {
                    dgMaster[colInsTypeBill.Name, rowIndex].Value = "工程日常检查";
                }
                if (strType == 1)
                {
                    dgMaster[colInsTypeBill.Name, rowIndex].Value = "专业检查";
                }
                dgMaster[colInsSupplierBill.Name, rowIndex].Value = master.SupplierUnitName; //受检承担单位
                dgMaster[colCreateDateBill.Name, rowIndex].Value = master.CreateDate.ToShortDateString();   //业务日期
                dgMaster[colCreatePersonBill.Name, rowIndex].Value = master.CreatePersonName;  //制单人
                dgMaster[colStateBill.Name, rowIndex].Value = "执行中";
                //dgMaster[colStateBill.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);   //状态
            }
            dgMaster.CurrentCell = dgMaster[1, 0];
            dgMaster_SelectionChanged(dgMaster, new EventArgs());
        }
        /// <summary>
        /// 主表变化，明细同步变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            txtInspection.TextChanged -= new EventHandler(txtInspection_TextChanged);
            txtInspection.Enabled = true;
            txtConclusion.Enabled = true;
            dgDetailBill.Rows.Clear();
            curBillMaster = dgMaster.CurrentRow.Tag as RectificationNoticeMaster;
            if (curBillMaster == null) return;
            foreach (RectificationNoticeDetail dtl in curBillMaster.Details)
            {
                if (dtl.IsCreated == 0)
                {
                    int rowIndex = dgDetailBill.Rows.Add();
                    dgDetailBill[colInsHandlePersonBill.Name, rowIndex].Value = curBillMaster.HandlePersonName;                            //受检管理责任者
                    dgDetailBill[colInsQuestionCodeBill.Name, rowIndex].Value = dtl.ProblemCode;                          //问题代码
                    dgDetailBill[colInsQuestionBill.Name, rowIndex].Value = dtl.QuestionState;                               //存在问题说明
                    dgDetailBill[colInsRequiredBill.Name, rowIndex].Value = dtl.Rectrequired;                                   //整改要求
                    dgDetailBill[colInsResultBill.Name, rowIndex].Value = dtl.RectContent;                                     //整改措施和结果说明

                    txtInspection.Text = dtl.RectContent;
                    txtProblem.Text = dtl.QuestionState;
                    txtDangerLevel.Text = dtl.DangerLevel;
                    txtDangerType.Text = dtl.DangerType;
                    txtDangerPart.Text = dtl.DangerPart;
                    txtRequired.Text = dtl.Rectrequired;
                    if (dtl.RequiredDate > ClientUtil.ToDateTime("1900-1-1"))
                    {
                        dgDetailBill[colInsCompleteDateBill.Name, rowIndex].Value = dtl.RequiredDate.ToShortDateString();
                        txtCompletDate.Text = dtl.RequiredDate.ToShortDateString();                  //要求整改完成时间
                    }
                    if (dtl.RectSendDate.ToShortDateString() != "")
                    {
                        if (dtl.RectSendDate > ClientUtil.ToDateTime("1900-1-1"))
                        {
                            dgDetailBill[colInsSendDateBill.Name, rowIndex].Value = dtl.RectSendDate.ToShortDateString();                          //整改通知下发时间  
                        }
                    }
                    int rec = dtl.RectConclusion;
                    if (rec == 0)
                    {
                        dgDetailBill[colInsConclusionBill.Name, rowIndex].Value = "整改中";  //整改结论  
                        txtConclusion.Text = "整改中";
                    }
                    if (rec == 1)
                    {
                        dgDetailBill[colInsConclusionBill.Name, rowIndex].Value = "整改未通过"; //整改结论  
                        txtConclusion.Text = "整改未通过";
                    }
                    if (rec == 2)
                    {
                        dgDetailBill[colInsConclusionBill.Name, rowIndex].Value = "整改通过"; //整改结论  
                        txtConclusion.Text = "整改通过";
                    }
                    if (dtl.RectDate > ClientUtil.ToDateTime("1900-1-1"))
                    {
                        dgDetailBill[colInsConcluDateBill.Name, rowIndex].Value = dtl.RectDate.ToShortDateString();     //整改结论时间
                    }
                    dgDetailBill.Rows[rowIndex].Tag = dtl;
                    dgDetailBill[colSelect.Name, rowIndex].Value = false;
                }
            }
            if (dgDetailBill.Rows.Count > 0)
            {
                dgDetailBill.CurrentCell = dgDetailBill[1, 0];
                dgDetailBill_SelectionChanged(dgDetailBill, new EventArgs());
            }
            txtInspection.TextChanged += new EventHandler(txtInspection_TextChanged);
        }
        /// <summary>
        /// 明细变化，明细操作区跟随变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetailBill_SelectionChanged(object sender, EventArgs e)
        {
            if (dgDetailBill.CurrentRow == null)
                return;
            RectificationNoticeDetail proDetail = dgDetailBill.CurrentRow.Tag as RectificationNoticeDetail;
            if (proDetail != null)
            {
                txtDangerLevel.Text = proDetail.DangerLevel;
                txtDangerType.Text = proDetail.DangerType;
                txtDangerPart.Text = proDetail.DangerPart;
                txtProblem.Text = ClientUtil.ToString(dgDetailBill.CurrentRow.Cells[colInsQuestionBill.Name].Value); ;
                txtRequired.Text = ClientUtil.ToString(dgDetailBill.CurrentRow.Cells[colInsRequiredBill.Name].Value);
                txtInspection.Text = ClientUtil.ToString(dgDetailBill.CurrentRow.Cells[colInsResultBill.Name].Value);
                if (proDetail.RequiredDate > ClientUtil.ToDateTime("1900-1-1"))
                {
                    txtCompletDate.Text = ClientUtil.ToString(dgDetailBill.CurrentRow.Cells[colInsCompleteDateBill.Name].Value);//要求整改完成时间
                }
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            if (this.dgMaster.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择信息！");
                return;
            }
            bool flag = false;
            foreach (DataGridViewRow var in this.dgDetailBill.Rows)
            {
                if (var.IsNewRow) break;
                if ((bool)var.Cells[colSelect.Name].Value)
                {
                    RectificationNoticeDetail dtl = var.Tag as RectificationNoticeDetail;
                    dtl.RectContent = ClientUtil.ToString(var.Cells[colInsResultBill.Name].Value);
                    if (ClientUtil.ToString(var.Cells[colInsConclusionBill.Name].Value).Equals("整改未通过"))
                    {
                        dtl.RectConclusion = 1;
                    }
                    if (ClientUtil.ToString(var.Cells[colInsConclusionBill.Name].Value).Equals("整改通过"))
                    {
                        dtl.RectConclusion = 2;
                    }
                    curBillMaster.AddDetail(dtl);
                    flag = true;
                }
            }

            if (flag)
            {
                curBillMaster = model.RectificationNoticeSrv.SaveRectificationNotice(curBillMaster);
                btnSearchBill_Click(sender, e);
                MessageBox.Show("保存成功");
            }
            else
            {
                MessageBox.Show("未选中任何信息，请选择！");
            }
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnOK_Click(object sender, EventArgs e)
        {
            bool flag = false;
            bool flags = true;
            if (this.dgMaster.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择信息！");
                return;
            }
            foreach (DataGridViewRow var in this.dgDetailBill.Rows)
            {
                if ((bool)var.Cells[colSelect.Name].Value)
                {
                    RectificationNoticeDetail dtl = var.Tag as RectificationNoticeDetail;
                    dtl.RectContent = ClientUtil.ToString(var.Cells[colInsResultBill.Name].Value);
                    if (ClientUtil.ToString(var.Cells[colInsConclusionBill.Name].Value).Equals("整改未通过"))
                    {
                        dtl.RectConclusion = 1;
                    }
                    if (ClientUtil.ToString(var.Cells[colInsConclusionBill.Name].Value).Equals("整改通过"))
                    {
                        dtl.RectConclusion = 2;
                    }
                    if (ClientUtil.ToString(var.Cells[colInsConclusionBill.Name].Value).Equals("整改中"))
                    {
                        MessageBox.Show("请选择整改结论！");
                        return;
                    }
                    dtl.IsCreated = 1;
                    flag = true;
                }
                else
                {
                    flags = false;
                }
            }
            if (flags)
            {
                curBillMaster.DocState = DocumentState.Completed;
            }
            if (flag)
            {
                curBillMaster.AuditPerson = ConstObject.LoginPersonInfo;//保存审核信息时将审核人一并保存
                curBillMaster.AuditPersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.AuditDate = ConstObject.LoginDate;
                curBillMaster = model.RectificationNoticeSrv.SaveRectificationNotice(curBillMaster);
                btnSearchBill_Click(sender, e);

                CreateTempDebit(curBillMaster);         // 生成暂扣款单

                MessageBox.Show("提交成功！");
                return;
            }
            else
            {
                MessageBox.Show("未选中任何信息，请选择！");
            }
        }

        /// <summary>
        /// 由暂扣款单生成的整改单，复核完成后再生成暂扣款单
        /// </summary>
        /// <param name="curBillMaster"></param>
        private void CreateTempDebit(RectificationNoticeMaster notice)
        {
            if (!string.IsNullOrEmpty(notice.TempDebitId)) // 由暂扣款单生成
            {
                var noticeDetail = notice.Details.FirstOrDefault() as RectificationNoticeDetail;
                if (noticeDetail.RectConclusion == 2)       // 整改通过
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", notice.TempDebitId));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                    var oldMaster = model.ObjectQuery(typeof(PenaltyDeductionMaster), oq)[0] as PenaltyDeductionMaster;
                    var newMaster = CloneTempDebit(oldMaster);
                    new MPenaltyDeductionMng().SavePenaltyDeduction(newMaster);
                }
            }
        }

        /// <summary>
        /// 克隆原暂扣款单
        /// </summary>
        /// <param name="template"></param>
        private PenaltyDeductionMaster CloneTempDebit(PenaltyDeductionMaster template)
        {
            var oldDetail = template.Details.FirstOrDefault() as PenaltyDeductionDetail;
            // 主表
            var newMaster = Clone(template);
            // 子表
            var newDetail = Clone(oldDetail);
            // 关联主表与子表
            newMaster.AddDetail(newDetail);
            newDetail.Master = newMaster;
            return newMaster;
        }

        private PenaltyDeductionMaster Clone(PenaltyDeductionMaster master)
        {
            var newMaster = new PenaltyDeductionMaster();
            newMaster.DocState = VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute;
            newMaster.CreateDate = DateTime.Now.Date;
            newMaster.CreateYear = DateTime.Now.Year;
            newMaster.CreateMonth = DateTime.Now.Month;
            newMaster.CreatePerson = master.CreatePerson;
            newMaster.CreatePersonName = master.CreatePersonName;
            newMaster.AuditDate = DateTime.Now.Date;
            newMaster.AuditYear = DateTime.Now.Year;
            newMaster.AuditMonth = DateTime.Now.Month;
            newMaster.AuditPerson = master.AuditPerson;
            newMaster.AuditPersonName = master.AuditPersonName;
            newMaster.HandleOrg = master.HandleOrg;
            newMaster.HandlePerson = master.HandlePerson;
            newMaster.HandlePersonName = master.HandlePersonName;
            newMaster.HandOrgLevel = master.HandOrgLevel;
            newMaster.LastModifyDate = DateTime.Now;
            newMaster.OperOrgInfo = master.OperOrgInfo;
            newMaster.OperOrgInfoName = master.OperOrgInfoName;
            newMaster.OpgSysCode = master.OpgSysCode;
            newMaster.ProjectId = master.ProjectId;
            newMaster.ProjectName = master.ProjectName;
            newMaster.RealOperationDate = DateTime.Now;
            newMaster.SubmitDate = DateTime.Now;
            newMaster.PenaltyDeductionRant = master.PenaltyDeductionRant;
            newMaster.PenaltyDeductionRantName = master.PenaltyDeductionRantName;
            newMaster.Descript = "整改完成";
            newMaster.PenaltyDeductionReason = "暂扣款红单";
            newMaster.PenaltyType = EnumUtil<PenaltyDeductionType>.FromDescription("暂扣款红单");
            newMaster.CheckOrderId = master.CheckOrderId;
            return newMaster;
        }
        private PenaltyDeductionDetail Clone(PenaltyDeductionDetail detail)
        {
            var newDetail = new PenaltyDeductionDetail();
            newDetail.BusinessDate = detail.BusinessDate;
            newDetail.PenaltyMoney = -detail.PenaltyMoney;
            newDetail.PenaltySubjectGUID = detail.PenaltySubjectGUID;
            newDetail.PenaltySubject = detail.PenaltySubject;
            newDetail.PenaltySysCode = detail.PenaltySysCode;
            newDetail.ResourceSysCode = detail.ResourceSysCode;
            newDetail.ResourceType = detail.ResourceType;
            newDetail.ResourceTypeName = detail.ResourceTypeName;
            newDetail.ResourceTypeSpec = detail.ResourceTypeSpec;
            newDetail.ResourceTypeStuff = detail.ResourceTypeStuff;
            newDetail.PenaltyType = detail.PenaltyType;
            newDetail.ProjectTask = detail.ProjectTask;
            newDetail.ProjectTaskSyscode = detail.ProjectTaskSyscode;
            newDetail.ProjectTaskName = detail.ProjectTaskName;
            newDetail.Cause = detail.Cause;
            newDetail.ProductUnit = detail.ProductUnit;
            newDetail.ProductUnitName = detail.ProductUnitName;
            newDetail.MoneyUnit = detail.MoneyUnit;
            newDetail.MoneyUnitName = detail.MoneyUnitName;
            return newDetail;
        }

    }
}
