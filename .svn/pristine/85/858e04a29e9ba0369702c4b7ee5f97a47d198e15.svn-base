using System;
using System.Windows.Forms;
using VirtualMachine.Component.Util;
using C1.Win.C1FlexGrid;
using System.Collections;
using Application.Resource.BasicData.Domain;
//using Application.Business.Erp.SupplyChain.CostingMng.InitData.Domain;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.Basic.Controls;
using Application.Business.Erp.SupplyChain.Approval.AppTableSetMng.Domain;
using Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Domain;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;
using Application.Business.Erp.SupplyChain.Approval.AppProcessMng.Domain;
using Application.Resource.CommonClass.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppSolutionSetUI;
using Application.Business.Erp.Secure.GlobalInfo;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.ApprovalMng.AppTableSetMng.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Drawing;
using System.Collections.Generic;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using System.Linq;
using System.Reflection;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using System.IO;
using Iesi.Collections;
using Application.Business.Erp.SupplyChain.Base.Domain;
using System.Data;

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatformUI
{
    public partial class VAppPlatform : Application.Business.Erp.SupplyChain.Client.Basic.Template.TBasicDataView
    {
        private MAppPlatform Model = new MAppPlatform();
        private IList List = new ArrayList();
        private AppStepsInfo curAppStepsInfo = null;
        private IList list_AppSolution = null;
        IList list_AppStepsInfo = null;
        CurrentProjectInfo projectInfo;
        string userName = string.Empty;
        string jobId = string.Empty;
        string fileObjectType = string.Empty;
        /// <summary>
        /// 需要审批的单据
        /// </summary>
        private Hashtable neededAuditBillHash = new Hashtable();

        /// <summary>
        /// 当前单据对应的审批步骤
        /// </summary>
        private Hashtable BillIdAppStepsSetHash = new Hashtable();

        public VAppPlatform()
        {
            InitializeComponent();
            this.InitEvent();
            InitData();
        }

        private void InitEvent()
        {
            dgBill.SelectionChanged += new EventHandler(dgBill_SelectionChanged);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            FgAppSetpsInfo.SelectionChanged += new EventHandler(FgAppSetpsInfo_SelectionChanged);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            dgMaster.CellDoubleClick += new DataGridViewCellEventHandler(dgMaster_CellDoubleClick);
            this.BtnAppAgree.Click += new System.EventHandler(BtnAppAgree_Click);
            this.BtnDisagree.Click += new System.EventHandler(BtnDisagree_Click);

            btnDownLoadDocument.Click += new EventHandler(btnDownLoadDocument_Click);
            btnOpenDocument.Click += new EventHandler(btnOpenDocument_Click);
        }

        void dgMaster_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell theCurrentCell = dgMaster.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (theCurrentCell.ReadOnly == false)
            {
                dgMaster.BeginEdit(true);
            }
        }

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell theCurrentCell = dgDetail.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (theCurrentCell.ReadOnly == false)
            {
                dgDetail.BeginEdit(true);
            }
        }

        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow SelectRow = null;
            if (dgMaster.Rows.Count > 0)
            {
                if (dgMaster.SelectedRows.Count > 0)
                {
                    SelectRow = dgMaster.SelectedRows[0];
                }
            }
            else
            {
                return;
            }
            if (SelectRow==null)
            {
                return;
            }

            DataGridViewRow BillRow = dgBill.SelectedRows[0];
            AppTableSet theAppTableSet = new AppTableSet();
            string masterId = "";
            if (BillRow != null)
            {
                theAppTableSet = BillRow.Tag as AppTableSet;
                if (SelectRow != null)
                {
                    dgDetail.Rows.Clear();
                    masterId = ClientUtil.ToString(BillRow.Cells[colCode.Name].Tag);
                    if (masterId == null || masterId.Equals("")) return;

                    object billMaster = neededAuditBillHash[masterId];

                    object detailsO=billMaster.GetType().GetProperty("Details").GetValue(billMaster, null);
                    IEnumerable set = detailsO as IEnumerable;//Iesi.Collections.Generic.ISet<BaseDetail>;

                    foreach (object detail in set)
                    {
                        int rowIndex = dgDetail.Rows.Add();
                        string Id = ClientUtil.ToString(detail.GetType().GetProperty("Id").GetValue(detail, null));
                        dgDetail["colDetailId", rowIndex].Value = Id;
                        dgDetail["colDetailId", rowIndex].Tag = Id;
                        IList detaiList = new ArrayList();
                        foreach (System.Reflection.PropertyInfo pi in detail.GetType().GetProperties())
                        {
                            if (dgDetail.Columns.Contains(pi.Name))
                            {
                                dgDetail[pi.Name, rowIndex].Value = ClientUtil.ToString(pi.GetValue(detail, null));
                                if (dgDetail.Columns[pi.Name].ReadOnly == true)
                                {
                                    dgDetail[pi.Name, rowIndex].ReadOnly = true;
                                }
                                else
                                {
                                    dgDetail[pi.Name, rowIndex].ReadOnly = false;
                                    dgDetail[pi.Name, rowIndex].Style.ForeColor = Color.Red;
                                }
                                AppDetailData theAppDetailData = new AppDetailData();
                                theAppDetailData.PropertyName = pi.Name;
                                theAppDetailData.PropertyValue = ClientUtil.ToString(pi.GetValue(detail, null));
                                theAppDetailData.BillId = masterId;
                                theAppDetailData.BillDtlId = Id;
                                theAppDetailData.PropertyChineseName = dgDetail.Columns[pi.Name].HeaderText;
                                theAppDetailData.PropertySerialNumber = dgDetail.Columns[pi.Name].Index;
                                theAppDetailData.AppStatus = 2L;
                                theAppDetailData.AppTableSet = theAppTableSet.Id;
                                dgDetail[pi.Name, rowIndex].Tag = theAppDetailData;

                                //detaiList.Add(theAppDetailData);
                            }
                        }
                    }
                }
                dgDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                ShowAppSetpSet(theAppTableSet, masterId);
                BillDocumentDisplay(masterId);
            }
        }        

        //挂入文档
        private void BillDocumentDisplay(string masterId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", masterId));
            IList listDocument = Model.ObjectQuery(typeof(ProObjectRelaDocument), oq);
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

        void dgBill_SelectionChanged(object sender, EventArgs e)
        {
            dgMaster.Columns.Clear();
            dgDetail.Columns.Clear();
            FgAppSetpsInfo.Rows.Clear();

            if (dgBill.Rows.Count == 0) return;
            if (dgBill.SelectedRows.Count == 0) return;

            DataGridViewRow thdCurrentRow = dgBill.SelectedRows[0];
            if (thdCurrentRow != null)
            {
                AppTableSet master = thdCurrentRow.Tag as AppTableSet;
                if (master == null) return;

                IList List_MasterProperty = Model.GetAppMasterProperties(master.Id);
                IList List_DetailProperty = Model.GetAppDetailProperties(master.Id);
                IList List_TempMaster = new ArrayList();
                if (List_MasterProperty != null)
                {
                    int columnIndex = 0;
                    foreach (AppMasterPropertySet MasterProperty in List_MasterProperty)
                    {                        
                        //按照属性的排序号排序
                        if (MasterProperty.MasterPropertyVisible == true)
                        {
                            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                            column.HeaderText = MasterProperty.MasterPpropertyChineseName;
                            column.Name = MasterProperty.MasterPropertyName;
                            if (MasterProperty.MasterPropertyReadOnly == true)
                            {
                                column.ReadOnly = false;
                            }
                            else
                            {
                                column.ReadOnly = true;
                            }
                            column.Tag = MasterProperty;
                            dgMaster.Columns.Insert(columnIndex, column);
                            columnIndex++;
                        }
                    }
                    DataGridViewTextBoxColumn MasterIdColumn = new DataGridViewTextBoxColumn();
                    MasterIdColumn.HeaderText = "主表ID";
                    MasterIdColumn.Name = "ColMasterId";
                    MasterIdColumn.Visible = false;
                    dgMaster.Columns.Add(MasterIdColumn);
                }
                if (List_DetailProperty != null)
                {
                    int columnIndex = 0;
                    foreach (AppDetailPropertySet DetailProperty in List_DetailProperty)
                    {
                        if (DetailProperty.DetailPropertyVisible == true)
                        {
                            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                            column.HeaderText = DetailProperty.DetailPropertyChineseName;
                            column.Name = DetailProperty.DetailPropertyName;
                            if (DetailProperty.DetailPropertyReadOnly == true)
                            {
                                column.ReadOnly = false;
                            }
                            else
                            {
                                column.ReadOnly = true;
                            }
                            column.Tag = DetailProperty;
                            dgDetail.Columns.Insert(columnIndex, column);
                            columnIndex++;
                        }
                    }
                    DataGridViewTextBoxColumn DetailIdColumn = new DataGridViewTextBoxColumn();
                    DetailIdColumn.HeaderText = "明细ID";
                    DetailIdColumn.Name = "ColDetailId";
                    DetailIdColumn.Visible = false;
                    dgDetail.Columns.Add(DetailIdColumn);
                }

                string auditBillId = (string)thdCurrentRow.Cells[colCode.Name].Tag;
                DgMasterShowData(auditBillId);
            }
        }

        private void DgMasterShowData(string auditBillId)
        {
            if (string.IsNullOrEmpty(auditBillId)) return;
            object obj = neededAuditBillHash[auditBillId];
            if (obj == null) return;

            int rowIndex = dgMaster.Rows.Add();

            foreach (System.Reflection.PropertyInfo pi in obj.GetType().GetProperties())
            {
                if (dgMaster.Columns.Contains(pi.Name))
                {
                    string ss = pi.Name;
                    string sValue = string.Empty; 
                    if (string.Equals(pi.Name, "DocState"))
                    {
                        sValue = ClientUtil.ToString(pi.GetValue(obj, null));
                        DocumentState enumDocState = (DocumentState)Enum.Parse(typeof(DocumentState), sValue);
                        sValue = ConstObject.BillState(enumDocState);
                        
                        
                    }
                    else
                    {
                        sValue = ClientUtil.ToString(pi.GetValue(obj, null));  
                    }
                    dgMaster[pi.Name, rowIndex].Value = sValue;
                    dgMaster[pi.Name, rowIndex].Tag = sValue;
                    //dgMaster[pi.Name, rowIndex].Value = ClientUtil.ToString(pi.GetValue(obj, null));
                    //dgMaster[pi.Name, rowIndex].Tag = ClientUtil.ToString(pi.GetValue(obj, null));
                    if (dgMaster.Columns[pi.Name].ReadOnly == true)
                    {
                        dgMaster[pi.Name, rowIndex].ReadOnly = true;
                    }
                    else
                    {
                        dgMaster[pi.Name, rowIndex].ReadOnly = false;
                        dgMaster[pi.Name, rowIndex].Style.ForeColor = Color.Red;
                    }
                    AppMasterData theAppMasterData = new AppMasterData();
                    theAppMasterData.PropertyName = pi.Name;
                    theAppMasterData.PropertyValue = ClientUtil.ToString(pi.GetValue(obj, null));
                    theAppMasterData.BillId = auditBillId;
                    theAppMasterData.PropertyChineseName = dgMaster.Columns[pi.Name].HeaderText;
                    theAppMasterData.PropertySerialNumber = dgMaster.Columns[pi.Name].Index;
                    theAppMasterData.AppDate = DateTime.Now;
                    theAppMasterData.ProjectId = StaticMethod.GetProjectInfo().Id;
                    theAppMasterData.AppStatus = 2L;
                    theAppMasterData.AppTableSet = (dgBill.SelectedRows[0].Tag as AppTableSet).Id;
                    dgMaster[pi.Name, rowIndex].Tag = theAppMasterData;

                    //masterData.Add(theAppMasterData);
                }
            }
            dgMaster.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }

        void FgAppSetpsInfo_SelectionChanged(object sender, EventArgs e)
        {
            if (FgAppSetpsInfo.Rows.Count <= 0)
            {
                this.BtnAppAgree.Enabled = false;
                this.BtnDisagree.Enabled = false;
                return;
            }
            if (FgAppSetpsInfo.SelectedRows.Count > 0)
            {
                curAppStepsInfo = FgAppSetpsInfo.SelectedRows[0].Tag as AppStepsInfo;
                if (curAppStepsInfo != null)
                {
                    if (curAppStepsInfo.Id != null)
                    {
                        this.BtnAppAgree.Enabled = false;
                        this.BtnDisagree.Enabled = false;
                    }
                    else
                    {
                        this.BtnAppAgree.Enabled = true;
                        this.BtnDisagree.Enabled = true;
                    }
                }
            }
        }

        void InitData()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
            DateBeg.Value = LoginInfomation.LoginInfo.LoginDate.AddMonths(-1);
            DateEnd.Value = LoginInfomation.LoginInfo.LoginDate;
            LoginInfomation.LoginInfo = AppDomain.CurrentDomain.GetData("TheLogin") as Login;
            this.label1.Text = "当前岗位: "+LoginInfomation.LoginInfo.TheSysRole.RoleName;
            this.FgAppSetpsInfo.Size = new System.Drawing.Size(800, 150);
        }

        void ShowAppSetpSet(AppTableSet theAppTableSet, string BillId)
        {
            FgAppSetpsInfo.Rows.Clear();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BillId", BillId));
            oq.AddCriterion(Expression.Eq("AppStatus", ClientUtil.ToLong(2)));
            oq.AddCriterion(Expression.Eq("State", 1));
            oq.AddFetchMode("AuditPerson", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("AppRole", NHibernate.FetchMode.Eager);

            //当前单据的审批信息
            list_AppStepsInfo = Model.Service.GetAppStepsInfo(oq);
            //获取当前单据定义的审批方案和审批步骤
            list_AppSolution = Model.Service.GetAppSolution(theAppTableSet);
            AppSolutionSet oAppSolutionSet = null;
            if (list_AppSolution.Count > 1)
            {
                for (int i = list_AppSolution.Count - 1; i >= 0; i--)
                {
                    oAppSolutionSet = list_AppSolution[i] as AppSolutionSet;
                    if (oAppSolutionSet != null)
                    {
                        if (SuitedCondition(theAppTableSet, BillId, oAppSolutionSet))
                        {
                            break;
                        }
                    }
                }
                list_AppSolution.Clear();
                list_AppSolution.Add(oAppSolutionSet);
            }

            #region 已审批通过的审批步骤信息
            foreach (AppStepsInfo master in list_AppStepsInfo)
            {
                int index = FgAppSetpsInfo.Rows.Add();

                FgAppSetpsInfo[StepOrder.Name, index].Value = ClientUtil.ToLong(master.StepOrder);
                FgAppSetpsInfo[StepName.Name, index].Value = ClientUtil.ToString(master.StepsName);
                if (master.AppRelations == 0)
                {
                    FgAppSetpsInfo[AppRelations.Name, index].Value = "或";
                }
                else
                {
                    FgAppSetpsInfo[AppRelations.Name, index].Value = "与";
                }
                FgAppSetpsInfo[AppRole.Name, index].Value = master.AppRole.RoleName;
                FgAppSetpsInfo[AppRole.Name, index].Tag = master.AppRole;
               
                FgAppSetpsInfo[AppComments.Name, index].Value = master.AppComments;
                FgAppSetpsInfo[AppDateTime.Name, index].Value = master.AppDate;
                FgAppSetpsInfo[AppPerson.Name, index].Value = master.AuditPerson.Name;

                FgAppSetpsInfo.Rows[index].Tag = master;
                switch (master.AppStatus)
                {
                    case -1:
                        FgAppSetpsInfo[AppStatus.Name, index].Value = "已撤单";
                        break;
                    case 0:
                        FgAppSetpsInfo[AppStatus.Name, index].Value = "审批中";
                        break;
                    case 1:
                        FgAppSetpsInfo[AppStatus.Name, index].Value = "未通过";
                        break;
                    case 2:
                        FgAppSetpsInfo[AppStatus.Name, index].Value = "已通过";
                        break;
                    default:
                        break;
                }
            }
            #endregion

            #region 待审批的步骤的缺省信息

            int rowIndex = FgAppSetpsInfo.Rows.Add();
            AppStepsSet ReadyAppStepsSet = BillIdAppStepsSetHash[BillId] as AppStepsSet;
            OperationRole currentRole = CurrentAppRole(ReadyAppStepsSet,BillId);
            
            AppStepsInfo theAppStepsInfo = new AppStepsInfo();
            theAppStepsInfo.StepOrder = ReadyAppStepsSet.StepOrder;
            theAppStepsInfo.StepsName = ReadyAppStepsSet.StepsName;
            theAppStepsInfo.AppRelations = ReadyAppStepsSet.AppRelations;
            theAppStepsInfo.AppTableSet = ReadyAppStepsSet.AppTableSet;
            theAppStepsInfo.AppRole = currentRole;
            theAppStepsInfo.RoleName = currentRole.RoleName;
            theAppStepsInfo.AppStepsSet = ReadyAppStepsSet;

            FgAppSetpsInfo[StepOrder.Name, rowIndex].Value = ClientUtil.ToLong(ReadyAppStepsSet.StepOrder);
            FgAppSetpsInfo[StepName.Name, rowIndex].Value = ClientUtil.ToString(ReadyAppStepsSet.StepsName);
            if (ReadyAppStepsSet.AppRelations == 0)
            {
                FgAppSetpsInfo[AppRelations.Name, rowIndex].Value = "或";
            }
            else
            {
                FgAppSetpsInfo[AppRelations.Name, rowIndex].Value = "与";
            }
            FgAppSetpsInfo[AppRole.Name, rowIndex].Value = currentRole.RoleName;
            FgAppSetpsInfo[AppRole.Name, rowIndex].Tag = currentRole;
            FgAppSetpsInfo[AppDateTime.Name, rowIndex].Value = DateTime.Now.Date;
            FgAppSetpsInfo.Rows[rowIndex].Tag = theAppStepsInfo;

            this.BtnAppAgree.Enabled = true;
            this.BtnDisagree.Enabled = true;
           
            #endregion

            FgAppSetpsInfo_SelectionChanged(null, new EventArgs());
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            dgBill.SelectionChanged -= new EventHandler(dgBill_SelectionChanged);
            dgMaster.SelectionChanged -= new EventHandler(dgMaster_SelectionChanged);
            QueryData();
            dgBill.SelectionChanged += new EventHandler(dgBill_SelectionChanged);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            if (dgBill.Rows.Count > 0)
            {
                //dgBill.CurrentCell = dgBill.Rows[0].Cells[collBillName.Name];
                dgBill_SelectionChanged(dgBill,new EventArgs());
            }
        }

        private void QueryData()
        {
            dgBill.Rows.Clear();
            IList appTableSetLst = CurrentUserAppTableSet();
            BillIdAppStepsSetHash.Clear();
            foreach (AppTableSet obj in appTableSetLst)
            {
                Query(obj);
                //dgMaster_SelectionChanged(dgMaster, new EventArgs());
            }
        }

        private void Query(AppTableSet master)
        {
            dgMaster.Rows.Clear();
            dgDetail.Rows.Clear();
            FgAppSetpsInfo.Rows.Clear();
            
            IList ListMaster = new ArrayList();
            IList ListDetail = new ArrayList();

            ObjectQuery oq = new ObjectQuery();
            if (CBoxDate.Checked == true)
            {
                oq.AddCriterion(Expression.Ge("CreateDate", this.DateBeg.Value.Date));
                oq.AddCriterion(Expression.Lt("CreateDate", this.DateEnd.Value.AddDays(1).Date));
            }
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InAudit));
            oq.AddCriterion(Expression.Eq("ProjectId", StaticMethod.GetProjectInfo().Id));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

            ListMaster = Model.GetDomainByCondition(master.MasterNameSpace, oq);
            ListMaster = FilterData(ListMaster, master);
            IList masterData = new ArrayList();
            IList detailData = new ArrayList();
            foreach (object obj in ListMaster)
            {
                //int rowIndex = dgMaster.Rows.Add();
                //masterData = new ArrayList();
                string Id = ClientUtil.ToString(obj.GetType().GetProperty("Id").GetValue(obj, null));

                //把当前单据放到hashtable中
                if (!neededAuditBillHash.ContainsKey(Id))
                {
                    neededAuditBillHash.Add(Id, obj);
                }

                //dgMaster["colMasterId", rowIndex].Value = Id;
                //dgMaster["colMasterId", rowIndex].Tag = Id;
                //parentIdStr = parentIdStr + Id + ",";
                string dgBillCode = "";
                string dgBillCreatePerson = "";
                string dgBillCreateDate = "";
                foreach (System.Reflection.PropertyInfo pi in obj.GetType().GetProperties())
                {
                    if (pi.Name == "CreatePersonName" || pi.Name == "ConfirmHandlePersonName")
                    {
                        dgBillCreatePerson = ClientUtil.ToString(pi.GetValue(obj, null));
                    }
                    if (pi.Name == "Code")
                    {
                        dgBillCode=ClientUtil.ToString(pi.GetValue(obj, null));
                    }
                    if (pi.Name == "CreateDate")
                    {
                        dgBillCreateDate = ClientUtil.ToString(pi.GetValue(obj, null));
                    }
                }
                DgBillAddRow(master, dgBillCode, dgBillCreatePerson, dgBillCreateDate,Id);
            }
            dgBill.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }

        private void DgBillAddRow(AppTableSet theAppTableSet,string code,string createPerson,string createDate,string id)
        {
            int i=dgBill.Rows.Add();
            DataGridViewRow dr = dgBill.Rows[i];
            dr.Tag = theAppTableSet;
            dr.Cells[colCode.Name].Value = code;
            dr.Cells[colCode.Name].Tag = id;
            dr.Cells[colCreatePerson.Name].Value = createPerson;
            dr.Cells[colCreateDate.Name].Value = createDate;
            dr.Cells[collBillName.Name].Value = theAppTableSet.TableName;
        }

        private IList FilterData(IList lstMaster, AppTableSet theAppTableSet)
        {
            IList retLst=new ArrayList();
            if (lstMaster == null || lstMaster.Count == 0) return retLst;
            

            //获取当前单据定义的审批方案
            IList lstAppSolution = Model.Service.GetAppSolution(theAppTableSet);
            if (lstAppSolution == null || lstAppSolution.Count == 0) return retLst;

            #region 判断审批方案配置是否完整
            IList lstSuitedAppSolution = new ArrayList();
            foreach (AppSolutionSet appSolutionSet in lstAppSolution)
            {
                if (appSolutionSet.AppStepsSets == null || appSolutionSet.AppStepsSets.Count == 0)
                {
                    continue;
                }
                bool roleConfiged = false;//是否配置审批角色
                foreach (AppStepsSet appStepsSet in appSolutionSet.AppStepsSets)
                {
                    if (appStepsSet.AppRoleSets.Count == 0)
                    {
                        continue;
                    }
                    else
                    {
                        roleConfiged = true;
                        break;
                    }
                }
                if (roleConfiged)
                {
                    lstSuitedAppSolution.Add(appSolutionSet);
                }
            }
            if (lstSuitedAppSolution.Count == 0)
            {
                //审批方案配置不完整 直接返回
                return retLst;
            }
            #endregion


            foreach (object obj in lstMaster)
            {
                string Id = ClientUtil.ToString(obj.GetType().GetProperty("Id").GetValue(obj, null));
                //判断这条数据是否有符合的审批方案
                IList tempAppSolutionSetLst = GetSuitedAppSolutionSet(theAppTableSet, Id, lstSuitedAppSolution);
                if (tempAppSolutionSetLst == null || tempAppSolutionSetLst.Count == 0)
                {
                    //没有合适的审批方案，这条数据不显示
                    continue;
                }
                AppSolutionSet currentAppSolutionSet = tempAppSolutionSetLst[0] as AppSolutionSet;
                if (!SuitedRole(theAppTableSet, obj, currentAppSolutionSet))
                {
                    continue;
                }
                retLst.Add(obj);
            }

            return retLst;
        }

        private bool CheckRoleAndOpg(string billOpgSysCode,List<OperationRole> appRoleLst)
        {
            //ConstObject..TheOperationOrg
            
            //当前登录用户的角色
            IList userRolelst = GetOperationRoleByJobId(ConstObject.TheSysRole.Id);
            //一、判断本部门 角色、组织是否相同
            if (ConstObject.TheOperationOrg.SysCode == billOpgSysCode && CheckRole(appRoleLst, userRolelst))
            {
                return true;
            }
            //二、最近上级部门和兄弟部门
            if (CheckOpg(billOpgSysCode, userRolelst, appRoleLst))
            {
                return true;
            }

            //三、审批角色只有对应一个岗位时 如果与登录岗位匹配则返回（无的情况）
            if (appRoleLst.Count>0)
            {
            OperationRole appRole = appRoleLst[0] as OperationRole;
            IList appJobLst = GetOperationJobByRoleId(appRole.Id);
            if (appJobLst != null && appJobLst.Count > 0)
            {
                if ((appJobLst[0] as OperationJob).Id == ConstObject.TheSysRole.Id)
                {
                    return true;
                }
            }
            }
            return false;
        }

        private bool CheckOpg(string billOpgSysCode,IList userRolelst,List<OperationRole> appRoleLst)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            string[] opgIds = billOpgSysCode.Split('.');
            if (opgIds == null || opgIds.Length == 0) return false;

            string tempId = "";
            List<string> parentSysCodes = new List<string>();
            foreach (string id in opgIds)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    tempId = tempId + id + ".";
                    parentSysCodes.Add(tempId);
                    dis.Add(Expression.Eq("SysCode", tempId.Trim()));
                }
            }
            if (opgIds.Length > 2)
            {
                dis.Add(Expression.Eq("ParentNode.Id", opgIds[opgIds.Length-3]));
            }
            
            oq.AddCriterion(dis);
            oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
            //查询单据对应业务部门的所有上层同级业务部门
            IList lstOpg=Model.Service.GetDomainByCondition(typeof(OperationOrg), oq);
            //最近上级部门的情况
            for (int j = parentSysCodes.Count - 1; j >= 0; j--)
            {
                string parentSysCode = parentSysCodes[j];
                if (parentSysCode == billOpgSysCode) continue;
                if (ConstObject.TheOperationOrg.SysCode == parentSysCode && CheckRole(appRoleLst, userRolelst))
                {
                    return true;
                }
            }
            /*****
             * 最近的兄弟部门
             * **/
            if (opgIds.Length > 1)
            {
                for (int k = opgIds.Length - 2; k >= 0; k--)
                {
                    string parentId = opgIds[k];
                    //List<OperationOrg> childs = GetChildOpg(parentId, lstOpg);//子业务部门
                    foreach (OperationOrg org in lstOpg)
                    {
                        string aa = ConstObject.TheOperationOrg.SysCode;
                        if (ConstObject.TheOperationOrg.SysCode == org.SysCode && CheckRole(appRoleLst, userRolelst))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private List<OperationOrg> GetChildOpg(string parentId, IList lstOpg)
        {
            //可以在本方法中把检索过的数据清除 以避免后续再循环 未处理
            List<OperationOrg> retLst = new List<OperationOrg>();
            if (lstOpg == null) return retLst;
            foreach (OperationOrg org in lstOpg)
            {
                if (org.ParentNode == null) continue;
                if (org.ParentNode.Id == parentId)
                {
                    retLst.Add(org);
                }
            }
            return retLst;
        }

        private bool CheckRole(List<OperationRole> appRoleLst, IList userRolelst)
        {
            foreach (OperationRole appRole in appRoleLst)
            {
                foreach (OperationRole userRole in userRolelst)
                {
                    if (appRole.Id == userRole.Id)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private IList GetOperationRoleByJobId(string jobId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("OperationJob.Id", jobId));
            oq.AddFetchMode("OperationRole", NHibernate.FetchMode.Eager);
            IList tempLst=Model.Service.GetDomainByCondition(typeof(OperationJobWithRole), oq);
            IList retLst = new ArrayList();
            foreach (OperationJobWithRole obj in tempLst)
            {
                retLst.Add(obj.OperationRole);
            }
            return retLst;
        }

        private IList GetOperationJobByRoleId(string roleId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("OperationRole.Id", roleId));
            oq.AddFetchMode("OperationJob", NHibernate.FetchMode.Eager);
            IList tempLst = Model.Service.GetDomainByCondition(typeof(OperationJobWithRole), oq);
            IList retLst = new ArrayList();
            foreach (OperationJobWithRole obj in tempLst)
            {
                retLst.Add(obj.OperationJob);
            }
            return retLst;
        }

        private bool SuitedRole(AppTableSet theAppTableSet, object objBillInfo, AppSolutionSet currentAppSolutionSet)
        {
            bool result = false;
            int CurrStepOrder = 1;
            int ReadyStepOrder = 1;
            AppStepsSet ReadyAppStepsSet = null;

            string BillId = ClientUtil.ToString(objBillInfo.GetType().GetProperty("Id").GetValue(objBillInfo, null));
            string opgSysCode = objBillInfo.GetType().GetProperty("OpgSysCode").GetValue(objBillInfo, null)+"";
            //ConstObject.TheOperationOrg

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BillId", BillId));
            oq.AddCriterion(Expression.Eq("AppStatus", ClientUtil.ToLong(2)));
            oq.AddCriterion(Expression.Eq("State", 1));
            oq.AddFetchMode("AuditPerson", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("AppRole", NHibernate.FetchMode.Eager);

            //当前单据的审批信息
            list_AppStepsInfo = Model.Service.GetAppStepsInfo(oq);

            /* 1：如果存在审批信息，然后确定待审步骤的岗位和当前登录人岗位对比
             * (1)如果能匹配到则显示审批步骤的缺省信息和之前审批通过的信息
             * (2)如果不匹配到则不显示任何信息
             * 
             * 2：如果不存审批信息，说明待审步骤是第一步
             * (1)如果当前登录人的岗位和第一步审批岗位匹配就显示该审批的缺省信息
             * (2)如果不匹配到则不显示任何信息
             */

            if (list_AppStepsInfo.Count > 0)
            {
                
                //确定待审批的步骤
                foreach (AppStepsInfo StepsInfo in list_AppStepsInfo)
                {
                    if (StepsInfo.StepOrder > CurrStepOrder)
                    {
                        CurrStepOrder = (int)StepsInfo.StepOrder;
                    }
                }
                ReadyStepOrder = CurrStepOrder;//待审步骤
                foreach (AppStepsSet item in currentAppSolutionSet.AppStepsSets)
                {
                    if (item.StepOrder < CurrStepOrder) continue;
                    if (item.StepOrder == CurrStepOrder)
                    {
                        if (item.AppRelations == 0)
                        {
                            //或关系 跳到下一步骤
                            ReadyStepOrder = ReadyStepOrder + 1;
                            break;
                        }
                        else
                        { 
                            //与关系
                            if (AllRolePassed(list_AppStepsInfo, item))
                            {
                                //所有与关系角色已经审批 跳到下一步骤
                                ReadyStepOrder = ReadyStepOrder + 1;
                                break;
                            }
                            else
                            { 
                                //还是执行当前步骤
                                ReadyStepOrder = CurrStepOrder;
                                break;
                            }
                        }
                    }
                }
                ReadyAppStepsSet = GetAppStepsSet(ReadyStepOrder, currentAppSolutionSet.AppStepsSets.ToList());                

                if (ReadyAppStepsSet != null)
                {
                    bool SFPP = false;//是否匹配标志
                    
                    List<OperationRole> appRoleLst=new List<OperationRole>();
                    foreach (AppRoleSet RoleSet in ReadyAppStepsSet.AppRoleSets)
                    {
                        //或关系把审批步骤的所角色作对比，与关系则排除已经作了审批的角色
                        if (ReadyAppStepsSet.AppRelations == 0)
                        {
                            appRoleLst.Add(RoleSet.AppRole);
                        }
                        else
                        {
                            bool hasAudit = false;
                            foreach (AppStepsInfo stepInfo in list_AppStepsInfo)
                            {
                                if (stepInfo.AppRole.Id == RoleSet.AppRole.Id)
                                {
                                    hasAudit = true;
                                    break;
                                }
                            }
                            if (hasAudit == false)
                            {
                                appRoleLst.Add(RoleSet.AppRole);
                            }
                        }
                        //
                    }
                    SFPP=CheckRoleAndOpg(opgSysCode, appRoleLst);
                    //待审批步骤的定义的岗位和当前登录人的岗位匹配
                    if (SFPP == true)
                    {
                        BillIdAppStepsSetHash.Add(BillId, ReadyAppStepsSet);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (currentAppSolutionSet != null)
                {
                    //确定待审步骤(第一步)
                    foreach (AppStepsSet item in currentAppSolutionSet.AppStepsSets)
                    {
                        //待审批步骤
                        if (item.StepOrder == 1)
                        {
                            ReadyAppStepsSet = item;
                        }
                    }
                    if (ReadyAppStepsSet != null)
                    {
                        bool SFPP = false;//是否匹配标志
                        List<OperationRole> appRoleLst = new List<OperationRole>();
                        foreach (AppRoleSet RoleSet in ReadyAppStepsSet.AppRoleSets)
                        {
                            appRoleLst.Add(RoleSet.AppRole);
                        }
                        SFPP = CheckRoleAndOpg(opgSysCode, appRoleLst);
                        //待审批步骤的定义的岗位和当前登录人的岗位匹配
                        if (SFPP == true)
                        {
                            BillIdAppStepsSetHash.Add(BillId, ReadyAppStepsSet);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 根据审批顺序确定
        /// </summary>
        /// <param name="stepOrder"></param>
        /// <param name="AppStepsSets"></param>
        /// <returns></returns>
        private AppStepsSet GetAppStepsSet(int stepOrder, IList AppStepsSets)
        {
            AppStepsSet appStepsSet = null;
            foreach (AppStepsSet item in AppStepsSets)
            {
                if (item.StepOrder == stepOrder)
                {
                    appStepsSet = item;
                    break;
                }
            }
            return appStepsSet;
        }

        /// <summary>
        /// 当审批步骤的关系为与（1）时 判断审批步骤的角色是否已经全部完成审批 全部完成返回true
        /// </summary>
        /// <param name="list_AppStepsInfo"></param>
        /// <param name="appStepsSet"></param>
        /// <returns></returns>
        private bool AllRolePassed(IList list_AppStepsInfo, AppStepsSet appStepsSet)
        {
            bool allPassed = true;

            foreach (AppRoleSet appRoleSet in appStepsSet.AppRoleSets)
            {
                bool currentPassed = false;
                foreach (AppStepsInfo stepInfo in list_AppStepsInfo)
                {
                    if (stepInfo.AppRole.Id == appRoleSet.AppRole.Id)
                    {
                        currentPassed = true;
                        break;
                    }
                }
                if (currentPassed == false)
                {
                    return false;
                }
            }
            return allPassed;
        }

        private List<AppSolutionSet> GetSuitedAppSolutionSet(AppTableSet theAppTableSet, string masterId, IList lstSuitedAppSolution)
        {
            List<AppSolutionSet> ret = new List<AppSolutionSet>();
            foreach (AppSolutionSet appSolutionSet in lstSuitedAppSolution)
            {
                if (appSolutionSet.Conditions == null || appSolutionSet.Conditions.Trim().Equals(""))
                {
                    //执行条件为空时，直接返回当前这个解决方案
                    ret.Add(appSolutionSet);
                    break;
                }

                if (SuitedCondition(theAppTableSet, masterId, appSolutionSet))
                {
                    ret.Add(appSolutionSet);
                    break;
                }
            }

            return ret;
        }

        /// <summary>
        /// 有执行条件时，判断这个数据是否符合条件
        /// </summary>
        /// <param name="theAppTableSet"></param>
        /// <param name="masterId"></param>
        /// <param name="appSolutionSet"></param>
        /// <returns></returns>
        private bool SuitedCondition(AppTableSet theAppTableSet, string masterId, AppSolutionSet appSolutionSet)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", masterId));
            oq.AddCriterion(Expression.Sql(appSolutionSet.Conditions));
            IList lst = null;
            try
            {
                lst = Model.Service.GetDomainByCondition(theAppTableSet.MasterNameSpace, oq);
            }
            catch (Exception ex)
            { 
            
            }
            if (lst != null && lst.Count > 0)
            {
                return true;
            }

            return false;
        }

        private void BtnAppAgree_Click(object sender, EventArgs e)
        {
            IList AppMasterDataList = new ArrayList();
            IList AppDetailDataList = new ArrayList();
            IList AppMasterDataModify = new ArrayList();
            IList AppDetailDataModify = new ArrayList();

            //判断当前审批步骤是否是最后一步
            bool IsFinish = true;
            AppSolutionSet theAppSolutionSet = list_AppSolution[0] as AppSolutionSet;
            //获取审批的最后一步
            long MaxStepOrder = 1;
            foreach (AppStepsSet item in theAppSolutionSet.AppStepsSets)
            {
                if (item.StepOrder >= MaxStepOrder)
                {
                    MaxStepOrder = item.StepOrder;
                }                
            }
            if (this.FgAppSetpsInfo.CurrentRow == null) return;
            AppStepsInfo currentSteps = this.FgAppSetpsInfo.CurrentRow.Tag as AppStepsInfo;
            if (currentSteps == null) return;

            //string BillId = ClientUtil.ToString(dgMaster.SelectedRows[0].Cells["ColMasterId"].Value);
            string BillId = dgBill.SelectedRows[0].Cells[colCode.Name].Tag+"";

            OperationRole currentRole = CurrentAppRole(currentSteps.AppStepsSet,BillId);
            currentSteps.AppRole = currentRole;
            currentSteps.RoleName = currentRole.RoleName;
            currentSteps.BillId = BillId;
            
            if (currentSteps.AppRelations == 0)
            {
                //或关系
                if (currentSteps.StepOrder >= MaxStepOrder)
                {
                    IsFinish = true;
                }
                else
                {
                    IsFinish = false;
                }
            }
            else if (currentSteps.AppRelations == 1)
            { 
                //与关系 必须是所有角色审批完成后才能代表完成
                if (AllRolePassed(currentSteps))
                {
                    if (currentSteps.StepOrder >= MaxStepOrder)
                    {
                        IsFinish = true;
                    }
                    else
                    {
                        IsFinish = false;
                    }
                }
                else
                {
                    IsFinish = false;
                }
            }


            //检查用户是否修改了单据信息(主表和明细)
            foreach (DataGridViewRow row in dgMaster.SelectedRows)
            {
                foreach (DataGridViewCell theCurrentCell in row.Cells)
                {
                    if (theCurrentCell.Visible == true)
                    {
                        string newValue = ClientUtil.ToString(theCurrentCell.Value);
                        string oldValue = ClientUtil.ToString((theCurrentCell.Tag as AppMasterData).PropertyValue);
                        if (newValue != oldValue)
                        {
                            AppMasterPropertySet theAppMasterProperty = dgMaster.Columns[theCurrentCell.ColumnIndex].Tag as AppMasterPropertySet;

                            AppMasterPropertySet theCurrMasterProperty = new AppMasterPropertySet();
                            theCurrMasterProperty.MasterPropertyName = theAppMasterProperty.MasterPropertyName;
                            theCurrMasterProperty.DataType = theAppMasterProperty.DataType;
                            theCurrMasterProperty.DBFieldName = theAppMasterProperty.DBFieldName;
                            theCurrMasterProperty.TempValue = newValue;
                            theCurrMasterProperty.TempBillId = ClientUtil.ToString(row.Cells["ColMasterId"].Value);
                            AppMasterDataModify.Add(theCurrMasterProperty);
                        }
                    }
                }
            }
            foreach (DataGridViewRow row in dgDetail.Rows)
            {
                foreach (DataGridViewCell theCurrentCell in row.Cells)
                {
                    if (theCurrentCell.Visible == true)
                    {
                        string newValue = ClientUtil.ToString(theCurrentCell.Value);
                        string oldValue = ClientUtil.ToString((theCurrentCell.Tag as AppDetailData).PropertyValue);
                        if (newValue != oldValue)
                        {
                            AppDetailPropertySet theAppDetailProperty = dgDetail.Columns[theCurrentCell.ColumnIndex].Tag as AppDetailPropertySet;

                            AppDetailPropertySet theCurrDetailProperty = new AppDetailPropertySet();
                            theCurrDetailProperty.DetailPropertyName = theAppDetailProperty.DetailPropertyName;
                            theCurrDetailProperty.DBFieldName = theAppDetailProperty.DBFieldName;
                            theCurrDetailProperty.DataType = theAppDetailProperty.DataType;
                            theCurrDetailProperty.TempValue = newValue;
                            theCurrDetailProperty.TempBillId = ClientUtil.ToString(row.Cells["ColDetailId"].Value);
                            AppDetailDataModify.Add(theCurrDetailProperty);
                        }
                    }
                }
            }

            AppMasterDataList = GetBillMasterMess();
            AppDetailDataList = GetBillDetailMess();

            DataGridViewRow thdCurrentRow = dgBill.SelectedRows[0];
            AppTableSet tableSet = thdCurrentRow.Tag as AppTableSet;
            //Model.Service.AppAgree(tableSet,currentSteps, textBox1.Text, BillId, IsFinish, AppMasterDataList, AppDetailDataList, AppMasterDataModify, AppDetailDataModify);
            MessageBox.Show("审批完成！");
            //if (IsFinish)
            //{
            //    object obj = neededAuditBillHash[BillId];
            //    if (obj != null)
            //    {
            //        string str = Model.Service.AppCommitBusiness(obj);
            //        if (!string.IsNullOrEmpty(str))
            //        {
            //            MessageBox.Show(str);
            //        }
            //    }
            //}
            Clear();
            //ShowAppSetpSet(dgBill.CurrentRow.Tag as AppTableSet, BillId);
        }

        private void Clear()
        {
            BtnAppAgree.Enabled = false;
            BtnDisagree.Enabled = false;
            dgDetail.Rows.Clear();
            FgAppSetpsInfo.Rows.Clear();
            dgMaster.Rows.Remove(dgMaster.CurrentRow);
            DataGridViewRow drAppTableSetBillRow = dgBill.CurrentRow;
            if (drAppTableSetBillRow != null)
            {
                dgBill.Rows.Remove(drAppTableSetBillRow);
            }
        }

        /// <summary>
        /// 用于审批通过时判断 是否己完成
        /// </summary>
        /// <param name="currentSteps"></param>
        /// <returns></returns>
        private bool AllRolePassed(AppStepsInfo currentSteps)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BillId", currentSteps.BillId));
            oq.AddCriterion(Expression.Eq("AppStatus", ClientUtil.ToLong(2)));
            oq.AddCriterion(Expression.Eq("State", 1));
            oq.AddFetchMode("AuditPerson", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("AppRole", NHibernate.FetchMode.Eager);

            //当前单据的审批信息
            list_AppStepsInfo = Model.Service.GetAppStepsInfo(oq);
            if (list_AppStepsInfo == null)
            {
                list_AppStepsInfo = new ArrayList();
            }
            list_AppStepsInfo.Add(currentSteps);

            return AllRolePassed(list_AppStepsInfo, currentSteps.AppStepsSet);
        }

        /// <summary>
        /// 当前审批的角色
        /// </summary>
        /// <param name="appStepsSet"></param>
        /// <returns></returns>
        private OperationRole CurrentAppRole(AppStepsSet appStepsSet, string BillId)
        {
            //当前登录用户的角色
            IList userRolelst = GetOperationRoleByJobId(ConstObject.TheSysRole.Id);
            
            foreach (OperationRole role in userRolelst)
            {
                //bool audited = false;
                //foreach (DataGridViewRow row in FgAppSetpsInfo.Rows)
                //{
                //    OperationRole roleAudited = row.Cells[AppRole.Name].Tag as OperationRole;
                //    if (roleAudited == null) continue;
                //    string appPerson = row.Cells[AppPerson.Name].Value + "";
                //    if (roleAudited.Id == role.Id && !string.IsNullOrEmpty(appPerson))
                //    {
                //        audited = true;
                //        break;
                //    }
                //}
                //if (audited) continue;
                
                foreach (AppRoleSet appRoleSet in appStepsSet.AppRoleSets)
                {
                    if (role.Id == appRoleSet.AppRole.Id)
                    {
                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("BillId", BillId));
                        oq.AddCriterion(Expression.Eq("AppStatus", ClientUtil.ToLong(2)));
                        oq.AddCriterion(Expression.Eq("State", 1));
                        oq.AddFetchMode("AuditPerson", NHibernate.FetchMode.Eager);
                        oq.AddFetchMode("AppRole", NHibernate.FetchMode.Eager);
                        //当前单据的审批信息
                        list_AppStepsInfo = Model.Service.GetAppStepsInfo(oq);
                        bool audited = false;
                        if (list_AppStepsInfo.Count > 0)
                        {
                            foreach (AppStepsInfo master in list_AppStepsInfo)
                            {
                                if (master.AppRole.Id == role.Id)
                                {
                                    audited = true;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        if (audited) continue;
                       return role;
                    }
                       
                }
            }
            return null;
        }

        private void BtnDisagree_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认要审批不通过吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            IList AppMasterDataList = new ArrayList();
            IList AppDetailDataList = new ArrayList();

            IList AppMasterDataModify = new ArrayList();
            IList AppDetailDataModify = new ArrayList();


            //检查用户是否修改了单据信息(主表和明细)
            foreach (DataGridViewRow row in dgMaster.SelectedRows)
            {
                foreach (DataGridViewCell theCurrentCell in row.Cells)
                {
                    if (theCurrentCell.Visible == true)
                    {
                        string newValue = ClientUtil.ToString(theCurrentCell.Value);
                        string oldValue = ClientUtil.ToString((theCurrentCell.Tag as AppMasterData).PropertyValue);
                        if (newValue != oldValue)
                        {
                            AppMasterPropertySet theAppMasterProperty = dgMaster.Columns[theCurrentCell.ColumnIndex].Tag as AppMasterPropertySet;

                            AppMasterPropertySet theCurrMasterProperty = new AppMasterPropertySet();
                            theCurrMasterProperty.MasterPropertyName = theAppMasterProperty.MasterPropertyName;
                            theCurrMasterProperty.DataType = theAppMasterProperty.DataType;
                            theCurrMasterProperty.DBFieldName = theAppMasterProperty.DBFieldName;
                            theCurrMasterProperty.TempValue = newValue;
                            theCurrMasterProperty.TempBillId = ClientUtil.ToString(row.Cells["ColMasterId"].Value);
                            AppMasterDataModify.Add(theCurrMasterProperty);
                        }
                    }
                }
            }
            foreach (DataGridViewRow row in dgDetail.Rows)
            {
                foreach (DataGridViewCell theCurrentCell in row.Cells)
                {
                    if (theCurrentCell.Visible == true)
                    {
                        string newValue = ClientUtil.ToString(theCurrentCell.Value);
                        string oldValue = ClientUtil.ToString((theCurrentCell.Tag as AppDetailData).PropertyValue);

                        if (newValue != oldValue)
                        {
                            AppDetailPropertySet theAppDetailProperty = dgDetail.Columns[theCurrentCell.ColumnIndex].Tag as AppDetailPropertySet;

                            AppDetailPropertySet theCurrDetailProperty = new AppDetailPropertySet();
                            theCurrDetailProperty.DetailPropertyName = theAppDetailProperty.DetailPropertyName;
                            theCurrDetailProperty.DBFieldName = theAppDetailProperty.DBFieldName;
                            theCurrDetailProperty.DataType = theAppDetailProperty.DataType;
                            theCurrDetailProperty.TempValue = newValue;
                            theCurrDetailProperty.TempBillId = ClientUtil.ToString(row.Cells["ColDetailId"].Value);
                            AppDetailDataModify.Add(theCurrDetailProperty);
                        }
                    }
                }
            }

            AppMasterDataList = GetBillMasterMess();
            AppDetailDataList = GetBillDetailMess();

            DataGridViewRow thdCurrentRow = dgBill.SelectedRows[0];
            AppTableSet tableSet = thdCurrentRow.Tag as AppTableSet;

            string BillId = dgBill.SelectedRows[0].Cells[colCode.Name].Tag + "";
            Model.Service.AppDisAgree(tableSet,FgAppSetpsInfo.SelectedRows[0].Tag as AppStepsInfo, textBox1.Text, BillId, AppMasterDataList, AppDetailDataList, AppMasterDataModify, AppDetailDataModify,null);

            MessageBox.Show("审批完成！");
            Clear();
            //ShowAppSetpSet(dgBill.CurrentRow.Tag as AppTableSet, BillId);
        }

        private IList GetBillMasterMess()
        {
            IList list = new ArrayList();
            foreach (DataGridViewRow row in dgMaster.SelectedRows)
            {
                foreach (DataGridViewCell Column in row.Cells)
                {
                    if (Column.Visible == true)
                    {
                        AppMasterData master = Column.Tag as AppMasterData;
                        master.AppStatus = 1;
                        master.PropertyValue = ClientUtil.ToString(Column.Value);
                        if (master != null)
                        {
                            list.Add(master);
                        }
                    }
                }
            }
            return list;
        }

        private IList GetBillDetailMess()
        {
            IList list = new ArrayList();
            foreach (DataGridViewRow row in dgDetail.Rows)
            {
                foreach (DataGridViewCell Column in row.Cells)
                {
                    if (Column.Visible == true)
                    {
                        AppDetailData detail = Column.Tag as AppDetailData;
                        detail.PropertyValue = ClientUtil.ToString(Column.Value);
                        if (detail != null)
                        {
                            list.Add(detail);
                        }
                    }
                }
                //IList list_temp = row.Tag as IList;
                //foreach (AppDetailData detailData in list_temp)
                //{
                //    list.Add(detailData);
                //}
            }
            return list;
        }

        #region 新
        private IList CurrentUserAppTableSet()
        {           
            //当前登录用户的角色
            IList userRolelst = GetOperationRoleByJobId(ConstObject.TheSysRole.Id);
            string roleConStr = " and c.appRole in('1'";
            foreach (OperationRole userRole in userRolelst)
            {
                roleConStr = roleConStr + ",'" + userRole.Id + "'";
            }
            roleConStr += ")";

            string sql = @"select distinct a.parentid appTableSetId from thd_appsolutionset a join thd_appstepsset b on b.parentid=a.id
                join thd_approleset c on c.parentid=b.id where 1=1 " + roleConStr;
            IList lst=Model.Service.CurrentUserTableSet(sql);
            return lst;
        }

        public override void ViewShow()
        {
            base.ViewShow();
            dgBill.SelectionChanged -= new EventHandler(dgBill_SelectionChanged);
            dgMaster.SelectionChanged -= new EventHandler(dgMaster_SelectionChanged);
             QueryData();

           // Binding1();
            dgBill.SelectionChanged += new EventHandler(dgBill_SelectionChanged);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            if (dgBill.Rows.Count > 0)
            {
                //dgBill.CurrentCell = dgBill.Rows[0].Cells[collBillName.Name];
                dgBill_SelectionChanged(dgBill, new EventArgs());
            }
        }
        #endregion

        public void Binding1()
        {
            dgBill.Rows.Clear();
            //"billCode", "billSysCode", "billCreateDate", "OrgSysCodes", "billCreatePerson", "solutionID", "SolutionName" 
            DataTable oTable = Model.Service.GetAppBill(ConstObject.TheSysRole.Id, ConstObject.TheOperationOrg.SysCode, StaticMethod.GetProjectInfo().Id, DateBeg.Value, this.DateEnd.Value);
            foreach (DataRow oRow in oTable.Rows)
            {
                string sBillCode = oRow["billCode"].ToString ();
                string sBillCreateDate = oRow["billCreateDate"].ToString();
                string sBillCreatePerson = oRow["billCreatePerson"].ToString();
                string sSolutionID = oRow["solutionID"].ToString();
                string sSolutionName = oRow["SolutionName"].ToString();
                string sBillID = oRow["billID"].ToString();
                int i = dgBill.Rows.Add();
                DataGridViewRow dr = dgBill.Rows[i];

                dr.Tag = sSolutionID;
                dr.Cells[colCode.Name].Value = sBillCode;
                dr.Cells[colCode.Name].Tag = sBillID;
                dr.Cells[colCreatePerson.Name].Value = sBillCreatePerson;
                dr.Cells[colCreateDate.Name].Value = sBillCreateDate;
                dr.Cells[collBillName.Name].Value = sSolutionName;
            }

        }
    }
}