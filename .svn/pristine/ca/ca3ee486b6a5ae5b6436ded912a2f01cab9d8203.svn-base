using System;
using System.Windows.Forms;
using VirtualMachine.Component.Util;
using C1.Win.C1FlexGrid;
using System.Collections;
using Application.Resource.BasicData.Domain;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.Basic.Controls;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.SupplyChain.Approval.AppTableSetMng.Domain;
using Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Domain;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;
using Application.Business.Erp.SupplyChain.Approval.AppProcessMng.Domain;
using Application.Resource.CommonClass.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppSolutionSetUI;
using Application.Business.Erp.Secure.GlobalInfo;
using Application.Business.Erp.SupplyChain.ApprovalMng.AppTableSetMng.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.IO;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using IRPServiceModel.Domain.Document;
using IRPServiceModel.Basic;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppProcessUI;

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatformUI
{
    public partial class VAppStatusQuery : Application.Business.Erp.SupplyChain.Client.Basic.Template.TBasicDataView
    {
        private SysRole CurrRole = null;
        private MAppPlatform Model = new MAppPlatform();
        private AppTableSet CurrTableSet = null;
        IList List = new ArrayList();
        private AppTableSet CurTableSet = new AppTableSet();
        Hashtable hashNoAppBill = new Hashtable();
        public static int ConstMaxCellLength = 4;
        CurrentProjectInfo projectInfo;
        string userName = string.Empty;
        string jobId = string.Empty;
        string fileObjectType = string.Empty;

        private IList MasterMostAppPropertyList = new ArrayList();
        private IList DetailMostAppPropertyList = new ArrayList();
        public VAppStatusQuery()
        {
            InitializeComponent();
            this.InitEvent();
            InitData();
            Search();
        }

        void InitData()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
            DateBeg.Value = LoginInfomation.LoginInfo.LoginDate.AddMonths(-1);
            DateEnd.Value = LoginInfomation.LoginInfo.LoginDate;
            LoginInfomation.LoginInfo = AppDomain.CurrentDomain.GetData("TheLogin") as Login;
            CurrRole = LoginInfomation.LoginInfo.TheSysRole;
        }

        private void InitEvent()
        {
            this.BtnQuery.Click += new EventHandler(BtnQuery_Click);
            this.dgBill.SelectionChanged += new EventHandler(dgBill_SelectionChanged);
            //this.dgBill.CellDoubleClick += new DataGridViewCellEventHandler(DataGridView_CellContentDoubleClick);

            // this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(DataGridView_CellContentDoubleClick);
            this.dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            dgDocumentMast.CellClick += new DataGridViewCellEventHandler(dgDocumentMast_CellClick);
            btnDownLoadDocument.Click += new EventHandler(btnDownLoadDocument_Click);
            btnOpenDocument.Click += new EventHandler(btnOpenDocument_Click);

        }
        void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            string sTitle = string.Empty;
            string sContent = string.Empty;
            CustomDataGridView oGird = sender as CustomDataGridView;
            if (oGird != null && e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                sContent = ClientUtil.ToString(oGird[e.ColumnIndex, e.RowIndex].Value);
                if (!string.IsNullOrEmpty(sContent) && sContent.Length > VAppStatusQuery.ConstMaxCellLength)
                {
                    // MessageBox.Show(sValue);
                    sTitle = string.Format("【{0}】---内容", oGird.Columns[e.ColumnIndex].HeaderText);
                    VShowContent oContent = new VShowContent(sTitle, sContent);
                    // Point p = new Point(e.X, e.Y);

                    oContent.StartPosition = FormStartPosition.CenterParent;
                    // oContent.Text = string.Format("X:{0};Y:{1}", e.X, e.Y);
                    oContent.ShowDialog();
                }
            }
        }
        #region 文档

        //加载文档数据
        void FillDoc(string billId)
        {
            dgDocumentMast.Rows.Clear();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", billId));
            IList listObj = Model.ObjectQuery(typeof(ProObjectRelaDocument), oq);
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
                IList docList = Model.ObjectQuery(typeof(DocumentMaster), oq);
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

        void AddDgDocumentMastInfo(DocumentMaster m)
        {
            int rowIndex = dgDocumentMast.Rows.Add();
            AddDgDocumentMastInfo(m, rowIndex);
        }

        void AddDgDocumentMastInfo(DocumentMaster m, int rowIndex)
        {
            dgDocumentMast[colDocumentName.Name, rowIndex].Value = m.Name;
            dgDocumentMast[colCreateTime.Name, rowIndex].Value = m.CreateTime;
            dgDocumentMast[colDocumentInforType.Name, rowIndex].Value = m.DocType.ToString();
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
            dgDocumentDetail.Rows[rowIndex].Tag = d;
        }

        void dgDocumentMast_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            dgDocumentDetail.Rows.Clear();
            DocumentMaster docMaster = dgDocumentMast.Rows[e.RowIndex].Tag as DocumentMaster;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", docMaster.Id));
            oq.AddFetchMode("TheFileCabinet", NHibernate.FetchMode.Eager);
            IList list = Model.ObjectQuery(typeof(DocumentDetail), oq);
            if (list != null && list.Count > 0)
            {
                foreach (DocumentDetail docDetail in list)
                {
                    AddDgDocumentDetailInfo(docDetail);
                }
            }
        }

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
        #endregion

        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            dgDetail.Rows.Clear();
            FgAppSetpsInfo.Rows.Clear();
            if (dgMaster.SelectedRows.Count > 0 || dgMaster.SelectedCells.Count > 0)
            {
                DataGridViewRow CurrRow = dgMaster.CurrentRow;
                string BillId = "";
                if (CurrRow != null)
                {
                    if (CurrRow.Tag != null)
                    {
                        if (IsNotAppBill())
                        {
                            BindDetialNoAppBill();
                        }
                        else
                        {
                            string linkStr = CurrRow.Tag as string;
                            //单据Id
                            BillId = linkStr.Split(',')[0];
                            //审批日期(时间戳)
                            string AppDate = linkStr.Split(',')[1];

                            #region 审批明细属性信息和数据
                            Hashtable hs_table = GetDetailData(BillId, AppDate);

                            if (hs_table != null)
                            {
                                foreach (string keys in hs_table.Keys)
                                {
                                    DataDomain domain = hs_table[keys] as DataDomain;

                                    int rowIndex = dgDetail.Rows.Add();
                                    if (dgDetail.Columns.Contains(domain.Name1.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name1.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name1.ToString().Split(',')[1];
                                    }
                                    if (dgDetail.Columns.Contains(domain.Name2.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name2.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name2.ToString().Split(',')[1];
                                    }
                                    if (dgDetail.Columns.Contains(domain.Name3.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name3.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name3.ToString().Split(',')[1]; ;
                                    }
                                    if (dgDetail.Columns.Contains(domain.Name4.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name4.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name4.ToString().Split(',')[1]; ;
                                    }

                                    if (dgDetail.Columns.Contains(domain.Name5.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name5.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name5.ToString().Split(',')[1];
                                    }
                                    if (dgDetail.Columns.Contains(domain.Name6.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name6.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name6.ToString().Split(',')[1];
                                    }
                                    if (dgDetail.Columns.Contains(domain.Name7.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name7.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name7.ToString().Split(',')[1]; ;
                                    }
                                    if (dgDetail.Columns.Contains(domain.Name8.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name8.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name8.ToString().Split(',')[1]; ;
                                    }

                                    if (dgDetail.Columns.Contains(domain.Name9.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name9.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name9.ToString().Split(',')[1];
                                    }
                                    if (dgDetail.Columns.Contains(domain.Name10.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name10.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name10.ToString().Split(',')[1];
                                    }
                                    if (dgDetail.Columns.Contains(domain.Name11.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name11.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name11.ToString().Split(',')[1]; ;
                                    }
                                    if (dgDetail.Columns.Contains(domain.Name12.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name12.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name12.ToString().Split(',')[1]; ;
                                    }
                                    if (dgDetail.Columns.Contains(domain.Name12.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name12.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name12.ToString().Split(',')[1]; ;
                                    }
                                    if (dgDetail.Columns.Contains(domain.Name13.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name13.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name13.ToString().Split(',')[1]; ;
                                    }
                                    if (dgDetail.Columns.Contains(domain.Name14.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name14.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name14.ToString().Split(',')[1]; ;
                                    }
                                    if (dgDetail.Columns.Contains(domain.Name15.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name15.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name15.ToString().Split(',')[1]; ;
                                    }
                                    if (dgDetail.Columns.Contains(domain.Name16.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name16.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name16.ToString().Split(',')[1]; ;
                                    }
                                    if (dgDetail.Columns.Contains(domain.Name17.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name17.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name17.ToString().Split(',')[1]; ;
                                    }
                                    if (dgDetail.Columns.Contains(domain.Name18.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name18.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name18.ToString().Split(',')[1]; ;
                                    }
                                    if (dgDetail.Columns.Contains(domain.Name19.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name19.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name19.ToString().Split(',')[1]; ;
                                    }
                                    if (dgDetail.Columns.Contains(domain.Name20.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name20.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name20.ToString().Split(',')[1]; ;
                                    }
                                    if (dgDetail.Columns.Contains(domain.Name21.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name21.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name21.ToString().Split(',')[1]; ;
                                    }
                                    if (dgDetail.Columns.Contains(domain.Name22.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name22.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name22.ToString().Split(',')[1]; ;
                                    }
                                    if (dgDetail.Columns.Contains(domain.Name23.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name23.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name23.ToString().Split(',')[1]; ;
                                    }
                                    if (dgDetail.Columns.Contains(domain.Name24.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name24.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name24.ToString().Split(',')[1]; ;
                                    }
                                    if (dgDetail.Columns.Contains(domain.Name25.ToString().Split(',')[0]))
                                    {
                                        string columnName = domain.Name25.ToString().Split(',')[0];
                                        dgDetail[columnName, rowIndex].Value = domain.Name25.ToString().Split(',')[1]; ;
                                    }
                                }
                            }
                            #endregion

                            #region 挂入文档
                            FillDoc(BillId);
                            #endregion

                            #region 审批记录
                            IList list_AppStepsInfo = GetAppStepsInfo(BillId, ClientUtil.ToDateTime(AppDate));
                            foreach (AppStepsInfo theAppStepsInfo in list_AppStepsInfo)
                            {
                                int rowIndex = FgAppSetpsInfo.Rows.Add();
                                FgAppSetpsInfo[StepOrder.Name, rowIndex].Value = theAppStepsInfo.StepOrder;
                                FgAppSetpsInfo[StepName.Name, rowIndex].Value = theAppStepsInfo.StepsName;
                                if (theAppStepsInfo.AppRelations == 0)
                                {
                                    FgAppSetpsInfo[AppRelations.Name, rowIndex].Value = "或";
                                }
                                else if (theAppStepsInfo.AppRelations == 1)
                                {
                                    FgAppSetpsInfo[AppRelations.Name, rowIndex].Value = "并";
                                }
                                FgAppSetpsInfo[AppRole.Name, rowIndex].Value = theAppStepsInfo.RoleName;
                                FgAppSetpsInfo[AppPerson.Name, rowIndex].Value = theAppStepsInfo.AuditPerson.Name;
                                FgAppSetpsInfo[AppComments.Name, rowIndex].Value = theAppStepsInfo.AppComments;
                                FgAppSetpsInfo[AppDateTime.Name, rowIndex].Value = theAppStepsInfo.AppDate;
                                switch (theAppStepsInfo.AppStatus)
                                {
                                    case -1:
                                        FgAppSetpsInfo[AppStatus.Name, rowIndex].Value = "已撤单";
                                        break;
                                    case 0:
                                        FgAppSetpsInfo[AppStatus.Name, rowIndex].Value = "审批中";
                                        break;
                                    case 1:
                                        FgAppSetpsInfo[AppStatus.Name, rowIndex].Value = "未通过";
                                        break;
                                    case 2:
                                        FgAppSetpsInfo[AppStatus.Name, rowIndex].Value = "已通过";
                                        break;
                                    default:
                                        break;
                                }
                            }
                            #endregion
                            if (list_AppStepsInfo != null && list_AppStepsInfo.Count > 0 )
                            {
                                //IList lst = Model.Service.GetAppBillPerNameByProc(BillId);
                                //if (lst != null)
                                //{
                                //    int rowIndex = FgAppSetpsInfo.Rows.Add();
                                //   // FgAppSetpsInfo[StepName.Name, rowIndex].Value = lst[0].ToString();
                                //    FgAppSetpsInfo[StepName.Name, rowIndex].Value =  "审批下一步";

                                //    FgAppSetpsInfo[StepName.Name, rowIndex].Style.ForeColor = Color.Blue;

                                //    FgAppSetpsInfo[AppPerson.Name, rowIndex].Value = lst[1].ToString();
                                //    FgAppSetpsInfo[AppPerson.Name, rowIndex].ToolTipText = lst[1].ToString();
                                //}
                                if ((list_AppStepsInfo[0] as AppStepsInfo).State == 1)//只有审批中的才会计算下一步
                                {
                                    BindNextStep(BillId);
                                }
                            }
                            #region
                            #endregion
                        }
                    }
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

        void dgBill_SelectionChanged(object sender, EventArgs e)
        {
            this.dgMaster.SelectionChanged -= new EventHandler(dgMaster_SelectionChanged);
            //DateTime startTime = DateTime.Now;
            //DateTime midTime = DateTime.Now;
            //DateTime midTime1 = DateTime.Now;
            //DateTime midTime2 = DateTime.Now;
            //DateTime endTime = DateTime.Now;

            dgMaster.Columns.Clear();
            try
            {
                dgDetail.Columns.Clear();
            }
            catch (Exception ex)
            {

            }
            MasterMostAppPropertyList = new ArrayList();
            DetailMostAppPropertyList = new ArrayList();

            FgAppSetpsInfo.Rows.Clear();

            DataGridViewRow thdCurrentRow = dgBill.SelectedRows[0];
            if (thdCurrentRow != null)
            {
                AppTableSet master = thdCurrentRow.Tag as AppTableSet;
                if (master != null)
                {
                    /*
                    ///
                    // 根据当前审批单据的定义查询当前审批记录,确定当前审批单据审批属性最多的情况
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Ge("AppDate", DateBeg.Value.Date));
                    oq.AddCriterion(Expression.Lt("AppDate", this.DateEnd.Value.AddDays(1).Date));
                    oq.AddCriterion(Expression.Eq("AppTableSet", master.Id));


                    IList ListMaster = Model.Service.GetAppData(typeof(AppMasterData), oq);
                    IList ListDetail = Model.Service.GetAppData(typeof(AppDetailData), oq);

                    //1: 如果存在审批记录，则根据审批记录的属性确定列表

                    if (ListMaster != null && ListMaster.Count > 0)
                    {
                        MasterMostAppPropertyList = GetMostAppProperty(ListMaster, "Master");

                        DetailMostAppPropertyList = GetMostAppProperty(ListDetail, "Detail");


                        if (MasterMostAppPropertyList != null)
                        {
                            int masterColumnIndex = 0;
                            foreach (AppMasterData MasterProperty in MasterMostAppPropertyList)
                            {
                                //按照属性的排序号排序

                                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                                column.HeaderText = MasterProperty.PropertyChineseName;
                                column.Name = MasterProperty.PropertyName;

                                column.ReadOnly = true;
                                dgMaster.Columns.Insert(masterColumnIndex, column);
                                masterColumnIndex++;
                                //dgMaster.Columns.Add(column);
                            }
                           

                            DataGridViewTextBoxColumn MasterIdColumn = new DataGridViewTextBoxColumn();
                            MasterIdColumn.HeaderText = "主表ID";
                            MasterIdColumn.Name = "ColMasterId";
                            MasterIdColumn.Visible = false;
                            dgMaster.Columns.Add(MasterIdColumn);
                        }
                        if (DetailMostAppPropertyList != null)
                        {
                            int detailColumnIndex = 0;
                            foreach (AppDetailData DetailProperty in DetailMostAppPropertyList)
                            {
                                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                                column.HeaderText = DetailProperty.PropertyChineseName;
                                column.Name = DetailProperty.PropertyName;

                                column.ReadOnly = true;
                                dgDetail.Columns.Insert(detailColumnIndex, column);
                                detailColumnIndex++;
                                //dgDetail.Columns.Add(column);
                            }
                            DataGridViewTextBoxColumn DetailIdColumn = new DataGridViewTextBoxColumn();
                            DetailIdColumn.HeaderText = "明细ID";
                            DetailIdColumn.Name = "ColDetailId";
                            DetailIdColumn.Visible = false;
                            dgDetail.Columns.Add(DetailIdColumn);
                        }
                    }

                    // 2：如果不存审批记录，则根据当前审批属性的定义确定列表
                    else
                    {   */
                    MasterMostAppPropertyList = Model.GetAppMasterProperties(master.Id);
                    DetailMostAppPropertyList = Model.GetAppDetailProperties(master.Id);
                    //IList List_TempMaster = new ArrayList();

                    if (MasterMostAppPropertyList != null)
                    {
                        int masterColumnIndex = 0;
                        foreach (AppMasterPropertySet MasterProperty in MasterMostAppPropertyList)
                        {
                            //按照属性的排序号排序
                            if (MasterProperty.MasterPropertyVisible == true)
                            {
                                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                                column.HeaderText = MasterProperty.MasterPpropertyChineseName;
                                column.Name = MasterProperty.MasterPropertyName;
                                if (MasterProperty.MasterPropertyReadOnly == true)
                                {
                                    column.ReadOnly = true;
                                }
                                else
                                {
                                    column.ReadOnly = false;
                                }
                                column.Width = 100;

                                dgMaster.Columns.Insert(masterColumnIndex, column);
                                masterColumnIndex++;
                            }
                        }
                        DataGridViewTextBoxColumn MasterIdColumn = new DataGridViewTextBoxColumn();
                        MasterIdColumn.HeaderText = "主表ID";
                        MasterIdColumn.Name = "ColMasterId";
                        MasterIdColumn.Visible = false;
                        dgMaster.Columns.Add(MasterIdColumn);
                    }

                    if (DetailMostAppPropertyList != null)
                    {
                        int detailColumnIndex = 0;
                        foreach (AppDetailPropertySet DetailProperty in DetailMostAppPropertyList)
                        {
                            if (DetailProperty.DetailPropertyVisible == true)
                            {
                                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                                column.HeaderText = DetailProperty.DetailPropertyChineseName;
                                column.Name = DetailProperty.DetailPropertyName;
                                if (DetailProperty.DetailPropertyReadOnly == true)
                                {
                                    column.ReadOnly = true;
                                }
                                else
                                {
                                    column.ReadOnly = false;
                                }
                                column.Width = 100;
                                dgDetail.Columns.Insert(detailColumnIndex, column);
                                detailColumnIndex++;
                            }
                        }
                        DataGridViewTextBoxColumn DetailIdColumn = new DataGridViewTextBoxColumn();
                        DetailIdColumn.HeaderText = "明细ID";
                        DetailIdColumn.Name = "ColDetailId";
                        DetailIdColumn.Visible = false;
                        dgDetail.Columns.Add(DetailIdColumn);
                    }
                    //}
                    // midTime = DateTime.Now;
                    if (dgMaster.Columns.Count > 1)
                    {
                        DataGridViewTextBoxColumn colAppTime = new DataGridViewTextBoxColumn();
                        colAppTime.HeaderText = "审批时间";
                        colAppTime.Name = "appDate";
                        colAppTime.ReadOnly = true;
                        colAppTime.Visible = true;
                        dgMaster.Columns.Insert(dgMaster.Columns.Count, colAppTime);
                        DataGridViewTextBoxColumn colAppSate = new DataGridViewTextBoxColumn();
                        colAppSate.HeaderText = "审批状态";
                        colAppSate.Name = "appSate";
                        colAppSate.ReadOnly = true;
                        colAppSate.Visible = true;
                        dgMaster.Columns.Insert(dgMaster.Columns.Count, colAppSate);
                    }
                    // midTime1 = DateTime.Now;
                }
            }
            BtnQuery_Click(sender, new EventArgs());
            this.dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);

            dgMaster.CellDoubleClick -= new DataGridViewCellEventHandler(DataGridView_CellDoubleClick);
            dgMaster.CellDoubleClick += new DataGridViewCellEventHandler(DataGridView_CellDoubleClick);
            dgDetail.CellDoubleClick -= new DataGridViewCellEventHandler(DataGridView_CellDoubleClick);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(DataGridView_CellDoubleClick);
            //endTime = DateTime.Now;
            //TimeSpan  pertime = midTime - startTime;
            //TimeSpan mid = midTime1 - midTime;
            //TimeSpan end = endTime - midTime1;
        }

        /// <summary>
        /// 获取单据审批属性最多的情况
        /// </summary>
        /// <param name="list">主表的审批信息</param>
        /// <param name="type">主表/明细</param>
        /// <returns></returns>
        private IList GetMostAppProperty(IList list, string type)
        {
            if (type == "Master")
            {
                IList temp_list = new ArrayList();

                foreach (AppMasterData master in list)
                {
                    if (temp_list.Count == 0)
                    {
                        temp_list.Add(master);
                    }
                    else
                    {
                        for (int i = 0; i < temp_list.Count; i++)
                        {
                            AppMasterData theAppMasterData = temp_list[i] as AppMasterData;
                            if (master.PropertyName == theAppMasterData.PropertyName)
                            {
                                break;
                            }
                            else if (i == temp_list.Count - 1)
                            {
                                temp_list.Add(master);
                                break;
                            }
                        }
                    }
                }
                //MasterMostAppPropertyList = temp_list;
                MasterMostAppPropertyList = SortMostMasterAppProperty(temp_list);
                return MasterMostAppPropertyList;
            }
            else if (type == "Detail" && list != null)
            {
                IList temp_list = new ArrayList();

                foreach (AppDetailData detail in list)
                {
                    if (temp_list.Count == 0)
                    {
                        temp_list.Add(detail);
                    }
                    else
                    {
                        for (int i = 0; i < temp_list.Count; i++)
                        {
                            AppDetailData theAppDetailData = temp_list[i] as AppDetailData;
                            if (detail.PropertyName == theAppDetailData.PropertyName)
                            {
                                break;
                            }
                            else if (i == temp_list.Count - 1)
                            {
                                temp_list.Add(detail);
                            }
                        }
                    }
                }
                //DetailMostAppPropertyList = temp_list;
                DetailMostAppPropertyList = SortMostDetailAppProperty(temp_list);
                return DetailMostAppPropertyList;
            }
            else
            {
                return null;
            }
        }

        private IList SortMostMasterAppProperty(IList list)
        {
            IList list_temp = new ArrayList();
            int i = 0;
            AppMasterData temp = new AppMasterData();
            for (int j = 0; j < list.Count; j++)
            {
                for (i = list.Count - 1; i > j; i--)
                {
                    if ((list[j] as AppMasterData).PropertySerialNumber > (list[i] as AppMasterData).PropertySerialNumber)
                    {
                        temp = list[j] as AppMasterData;
                        list[j] = list[i];
                        list[i] = temp;
                    }
                }
            }
            return list;
        }

        private IList SortMostDetailAppProperty(IList list)
        {
            IList list_temp = new ArrayList();
            int i = 0;
            AppDetailData temp = new AppDetailData();
            for (int j = 0; j < list.Count; j++)
            {
                for (i = list.Count - 1; i > j; i--)
                {
                    if ((list[j] as AppDetailData).PropertySerialNumber > (list[i] as AppDetailData).PropertySerialNumber)
                    {
                        temp = list[j] as AppDetailData;
                        list[j] = list[i];
                        list[i] = temp;
                    }
                }
            }
            return list;
        }


        private void Search()
        {
            CurrentProjectInfo ProjectInfo = StaticMethod.GetProjectInfo();

            ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));
            oq.AddCriterion(Expression.Eq("Status", 0));

            List = Model.GetObjects(typeof(AppTableSet), oq);
            foreach (AppTableSet item in List)
            {
                int rowIndex = dgBill.Rows.Add();
                dgBill[collBillName.Name, rowIndex].Value = item.TableName;
                dgBill[colClassName.Name, rowIndex].Value = item.ClassName;
                dgBill[colPhysicsName.Name, rowIndex].Value = item.PhysicsName;
                dgBill[colDetailClassName.Name, rowIndex].Value = item.DetailClassName;
                dgBill[colDetailPhysicsName.Name, rowIndex].Value = item.DetailPhysicsName;
                dgBill[colExcuCode.Name, rowIndex].Value = item.ExecCode;
                dgBill[colStatusName.Name, rowIndex].Value = item.StatusName;
                dgBill[colStatusValue.Name, rowIndex].Value = item.StatusValueAgr;
                dgBill[colDescript.Name, rowIndex].Value = item.Remark;
                if (item.Status == 0)
                {
                    dgBill[colStatus.Name, rowIndex].Value = "启用";
                }
                else
                {
                    dgBill[colStatus.Name, rowIndex].Value = "停用";
                }
                dgBill.Rows[rowIndex].Tag = item;
            }
        }

        void BtnQuery_Click(object sender, EventArgs e)
        {
            // this.dgMaster.SelectionChanged -= new EventHandler(dgMaster_SelectionChanged);
            dgMaster.Rows.Clear();
            dgDetail.Rows.Clear();
            FgAppSetpsInfo.Rows.Clear();
            Hashtable hs_table = GetMasterData();



            DataRow[] Rows = null;
            string sWhere = string.Empty;
            if (hs_table != null)
            {
                DataTable oDataTable = GetPassBillIDs();
                foreach (string keys in hs_table.Keys)
                {
                    DataDomain domain = hs_table[keys] as DataDomain;

                    int rowIndex = dgMaster.Rows.Add();
                    if (dgMaster.Columns.Contains(domain.Name1.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name1.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name1.ToString().Split(',')[1];
                    }
                    if (dgMaster.Columns.Contains(domain.Name2.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name2.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name2.ToString().Split(',')[1];
                    }
                    if (dgMaster.Columns.Contains(domain.Name3.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name3.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name3.ToString().Split(',')[1];
                    }
                    if (dgMaster.Columns.Contains(domain.Name4.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name4.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name4.ToString().Split(',')[1];
                    }

                    if (dgMaster.Columns.Contains(domain.Name5.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name5.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name5.ToString().Split(',')[1];
                    }
                    if (dgMaster.Columns.Contains(domain.Name6.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name6.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name6.ToString().Split(',')[1];
                    }
                    if (dgMaster.Columns.Contains(domain.Name7.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name7.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name7.ToString().Split(',')[1];
                    }
                    if (dgMaster.Columns.Contains(domain.Name8.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name8.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name8.ToString().Split(',')[1];
                    }

                    if (dgMaster.Columns.Contains(domain.Name9.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name9.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name9.ToString().Split(',')[1];
                    }
                    if (dgMaster.Columns.Contains(domain.Name10.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name10.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name10.ToString().Split(',')[1];
                    }
                    if (dgMaster.Columns.Contains(domain.Name11.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name11.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name11.ToString().Split(',')[1];
                    }
                    if (dgMaster.Columns.Contains(domain.Name12.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name12.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name12.ToString().Split(',')[1];
                    }
                    if (dgMaster.Columns.Contains(domain.Name12.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name12.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name12.ToString().Split(',')[1];
                    }
                    if (dgMaster.Columns.Contains(domain.Name13.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name13.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name13.ToString().Split(',')[1];
                    }
                    if (dgMaster.Columns.Contains(domain.Name14.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name14.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name14.ToString().Split(',')[1];
                    }
                    if (dgMaster.Columns.Contains(domain.Name15.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name15.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name15.ToString().Split(',')[1];
                    }
                    if (dgMaster.Columns.Contains(domain.Name16.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name16.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name16.ToString().Split(',')[1];
                    }
                    if (dgMaster.Columns.Contains(domain.Name17.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name17.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name17.ToString().Split(',')[1];
                    }
                    if (dgMaster.Columns.Contains(domain.Name18.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name18.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name18.ToString().Split(',')[1];
                    }
                    if (dgMaster.Columns.Contains(domain.Name19.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name19.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name19.ToString().Split(',')[1];
                    }
                    if (dgMaster.Columns.Contains(domain.Name20.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name20.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name20.ToString().Split(',')[1];
                    }
                    if (dgMaster.Columns.Contains(domain.Name21.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name21.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name21.ToString().Split(',')[1];
                    }
                    if (dgMaster.Columns.Contains(domain.Name22.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name22.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name22.ToString().Split(',')[1];
                    }
                    if (dgMaster.Columns.Contains(domain.Name23.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name23.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name23.ToString().Split(',')[1];
                    }
                    if (dgMaster.Columns.Contains(domain.Name24.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name24.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name24.ToString().Split(',')[1];
                    }
                    if (dgMaster.Columns.Contains(domain.Name25.ToString().Split(',')[0]))
                    {
                        string columnName = domain.Name25.ToString().Split(',')[0];
                        dgMaster[columnName, rowIndex].Value = domain.Name25.ToString().Split(',')[1];
                    }
                    dgMaster.Rows[rowIndex].Tag = keys;
                    dgMaster.Rows[rowIndex].Cells["appDate"].Value = keys.Split(',')[1];
                    dgMaster.Rows[rowIndex].Cells["appSate"].Value = (keys.Split(',')[2] == "2" ? "审批中..." : "已退回");
                    if (keys.Split(',')[2] == "2" && oDataTable != null)
                    {
                        sWhere = string.Format("id='{0}'", keys.Split(',')[0]);
                        Rows = oDataTable.Select(sWhere);
                        if (Rows != null && Rows.Length > 0)
                        {
                            dgMaster.Rows[rowIndex].Cells["appSate"].Value = "已通过";
                        }
                    }
                }
            }
            BindMasterNoAppBill();

            //  this.dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            dgMaster_SelectionChanged(sender, new EventArgs());
        }
        private DataTable GetPassBillIDs()
        {
            DataTable oDataTable = null;
            DataGridViewRow thdCurrentRow = dgBill.SelectedRows[0];
            if (thdCurrentRow != null && thdCurrentRow.Tag != null)
            {
                AppTableSet master = thdCurrentRow.Tag as AppTableSet;
                if (master != null)
                {
                    string sStartTime = DateBeg.Value.Date.ToShortDateString();
                    string sEndTime = this.DateEnd.Value.Date.ToShortDateString();
                    string sProjectID = StaticMethod.GetProjectInfo().Id;
                    string sTableSetID = master.Id;
                    DataSet ds = Model.Service.GetPassBillID(sTableSetID, sStartTime, sEndTime, sProjectID);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        oDataTable = ds.Tables[0];
                    }
                }
            }
            return oDataTable;
        }
        private Hashtable GetMasterData()
        {
            if (dgBill.Rows.Count == 0)
                return null;
            DataGridViewRow thdCurrentRow = dgBill.SelectedRows[0];
            Hashtable hs_table = null;
            if (thdCurrentRow != null)
            {
                AppTableSet master = thdCurrentRow.Tag as AppTableSet;
                if (master != null)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Ge("AppDate", DateBeg.Value.Date));
                    oq.AddCriterion(Expression.Lt("AppDate", this.DateEnd.Value.AddDays(1).Date));
                    oq.AddCriterion(Expression.Eq("AppTableSet", master.Id));
                    oq.AddCriterion(Expression.Eq("ProjectId", StaticMethod.GetProjectInfo().Id));

                    IList ListMasterData = Model.Service.GetAppData(typeof(AppMasterData), oq);
                    if (ListMasterData != null && ListMasterData.Count > 0)
                        hs_table = GetHorizontalMasterData(ListMasterData);
                }
            }
            return hs_table;
        }

        private Hashtable GetDetailData(string BillId, string AppDate)
        {
            Hashtable hs_table = null;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BillId", BillId));
            oq.AddCriterion(Expression.Eq("AppDate", ClientUtil.ToDateTime(AppDate)));
            IList ListDetailData = Model.Service.GetAppData(typeof(AppDetailData), oq);
            if (ListDetailData != null && ListDetailData.Count > 0)
            {
                hs_table = GetHorizontalDetailData(ListDetailData);
            }
            return hs_table;
        }
        //将主表存储的信息竖表数据根据条件转成横表存在DataDomain中
        private Hashtable GetHorizontalMasterData1(IList list)
        {
            Hashtable ht = new Hashtable();
            foreach (AppMasterData master in list)
            {
                string linkStr = master.BillId + "," + master.AppDate;
                if (!ht.Contains(linkStr))
                {
                    DataDomain domain = GetDoamin();
                    domain.Name1 = master.PropertyName + "," + master.PropertyValue;
                    ht.Add(linkStr, domain);
                }
                else
                {
                    DataDomain domain = (DataDomain)ht[linkStr];
                    domain = FillMasterDomain(domain, master);
                    ht.Remove(linkStr);
                    ht.Add(linkStr, domain);
                }
            }
            return ht;
        }
        //将主表存储的信息竖表数据根据条件转成横表存在DataDomain中
        private Hashtable GetHorizontalMasterData(IList list)
        {
            Hashtable ht = new Hashtable();
            foreach (AppMasterData master in list)
            {
                string linkStr = master.BillId + "," + master.AppDate + "," + master.AppStatus;
                if (!ht.Contains(linkStr))
                {
                    DataDomain domain = GetDoamin();
                    domain.Name1 = master.PropertyName + "," + master.PropertyValue;
                    ht.Add(linkStr, domain);
                }
                else
                {
                    DataDomain domain = (DataDomain)ht[linkStr];
                    domain = FillMasterDomain(domain, master);
                    ht.Remove(linkStr);
                    ht.Add(linkStr, domain);
                }
            }
            return ht;
        }
        //将明细存储的信息竖表数据根据条件转成横表存在在DataDomain中
        private Hashtable GetHorizontalDetailData(IList list)
        {
            Hashtable ht = new Hashtable();
            foreach (AppDetailData master in list)
            {
                string linkStr = master.BillDtlId + "," + master.AppDate;
                if (!ht.Contains(linkStr))
                {
                    DataDomain domain = GetDoamin();
                    domain.Name1 = master.PropertyName + "," + master.PropertyValue;
                    ht.Add(linkStr, domain);
                }
                else
                {
                    DataDomain domain = (DataDomain)ht[linkStr];
                    domain = FillDetailDomain(domain, master);
                    ht.Remove(linkStr);
                    ht.Add(linkStr, domain);
                }
            }
            return ht;
        }

        private DataDomain FillMasterDomain(DataDomain domain, AppMasterData master)
        {
            if (domain.Name1 == "**")
            {
                domain.Name1 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name2 == "**")
            {
                domain.Name2 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name3 == "**")
            {
                domain.Name3 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name4 == "**")
            {
                domain.Name4 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name5 == "**")
            {
                domain.Name5 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name6 == "**")
            {
                domain.Name6 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name7 == "**")
            {
                domain.Name7 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name8 == "**")
            {
                domain.Name8 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name9 == "**")
            {
                domain.Name9 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name10 == "**")
            {
                domain.Name10 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name11 == "**")
            {
                domain.Name11 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name12 == "**")
            {
                domain.Name12 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name13 == "**")
            {
                domain.Name13 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name14 == "**")
            {
                domain.Name14 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name15 == "**")
            {
                domain.Name15 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name16 == "**")
            {
                domain.Name16 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name17 == "**")
            {
                domain.Name17 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name18 == "**")
            {
                domain.Name18 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name19 == "**")
            {
                domain.Name19 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name20 == "**")
            {
                domain.Name20 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name21 == "**")
            {
                domain.Name21 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name22 == "**")
            {
                domain.Name22 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name23 == "**")
            {
                domain.Name23 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name24 == "**")
            {
                domain.Name24 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name25 == "**")
            {
                domain.Name25 = master.PropertyName + "," + master.PropertyValue;
            }
            else
            {

            }
            return domain;
        }

        private DataDomain FillDetailDomain(DataDomain domain, AppDetailData master)
        {
            if (domain.Name1 == "**")
            {
                domain.Name1 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name2 == "**")
            {
                domain.Name2 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name3 == "**")
            {
                domain.Name3 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name4 == "**")
            {
                domain.Name4 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name5 == "**")
            {
                domain.Name5 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name6 == "**")
            {
                domain.Name6 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name7 == "**")
            {
                domain.Name7 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name8 == "**")
            {
                domain.Name8 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name9 == "**")
            {
                domain.Name9 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name10 == "**")
            {
                domain.Name10 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name11 == "**")
            {
                domain.Name11 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name12 == "**")
            {
                domain.Name12 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name13 == "**")
            {
                domain.Name13 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name14 == "**")
            {
                domain.Name14 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name15 == "**")
            {
                domain.Name15 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name16 == "**")
            {
                domain.Name16 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name17 == "**")
            {
                domain.Name17 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name18 == "**")
            {
                domain.Name18 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name19 == "**")
            {
                domain.Name19 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name20 == "**")
            {
                domain.Name20 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name21 == "**")
            {
                domain.Name21 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name22 == "**")
            {
                domain.Name22 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name23 == "**")
            {
                domain.Name23 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name24 == "**")
            {
                domain.Name24 = master.PropertyName + "," + master.PropertyValue;
            }
            else if (domain.Name25 == "**")
            {
                domain.Name25 = master.PropertyName + "," + master.PropertyValue;
            }
            else
            {

            }
            return domain;
        }

        private DataDomain GetDoamin()
        {
            DataDomain domain = new DataDomain();
            domain.Name1 = "**";
            domain.Name2 = "**";
            domain.Name3 = "**";
            domain.Name4 = "**";
            domain.Name5 = "**";
            domain.Name6 = "**";
            domain.Name7 = "**";
            domain.Name8 = "**";
            domain.Name9 = "**";
            domain.Name10 = "**";
            domain.Name11 = "**";
            domain.Name12 = "**";
            domain.Name13 = "**";
            domain.Name14 = "**";
            domain.Name15 = "**";
            domain.Name16 = "**";
            domain.Name17 = "**";
            domain.Name18 = "**";
            domain.Name19 = "**";
            domain.Name20 = "**";
            domain.Name21 = "**";
            domain.Name22 = "**";
            domain.Name23 = "**";
            domain.Name24 = "**";
            domain.Name25 = "**";
            return domain;
        }

        private IList GetAppStepsInfo(string BillId, DateTime AppDate)
        {
            IList list = new ArrayList();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BillId", BillId));
            oq.AddCriterion(Expression.Eq("BillAppDate", AppDate));
            oq.AddFetchMode("AppRole", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("AuditPerson", NHibernate.FetchMode.Eager);
            oq.AddOrder(NHibernate.Criterion.Order.Asc("AppDate"));
            list = Model.Service.GetAppStepsInfo(oq);
            return list;
        }
        public Hashtable GetMasterID()
        {
            Hashtable hs = new Hashtable();
            string sKey = string.Empty;
            string[] arr;
            foreach (DataGridViewRow oRow in this.dgMaster.Rows)
            {
                sKey = oRow.Tag as string;
                if (!string.IsNullOrEmpty(sKey))
                {
                    arr = sKey.Split(',');
                    if (arr.Length > 2)
                    {
                        if (!hs.ContainsKey(arr[0]) && arr[2] == "2")
                        {
                            hs.Add(arr[0], "");
                        }
                    }
                }
            }
            return hs;
        }
        private void BindMasterNoAppBill()
        {
            IList lstBills = null;
            if (dgBill.Rows.Count == 0)
                return;
            DataGridViewRow thdCurrentRow = dgBill.CurrentRow;
            if (thdCurrentRow == null) return;
            AppTableSet master = thdCurrentRow.Tag as AppTableSet;
            //xuleitest
            //  lstBills = Model.Service.GetBill(master,StaticMethod .GetProjectInfo ().Id );
            lstBills = Model.Service.GetBill(master, StaticMethod.GetProjectInfo().Id, DateBeg.Value, DateEnd.Value);
            int rowIndex = 0;
            string sColumnName = string.Empty;
            string sValue = string.Empty;
            if (lstBills == null || lstBills.Count == 0) return;
            Hashtable htAppBillID = GetMasterID();
            foreach (object obj in lstBills)
            {
                string Id = ClientUtil.ToString(obj.GetType().GetProperty("Id").GetValue(obj, null));
                if (!htAppBillID.ContainsKey(Id))
                {
                    rowIndex = dgMaster.Rows.Add();


                    dgMaster["ColMasterId", rowIndex].Value = Id;
                    dgMaster["ColMasterId", rowIndex].Tag = Id;

                    dgMaster.Rows[rowIndex].Tag = obj;
                    dgMaster.Rows[rowIndex].Cells["appSate"].Value = "未审批";
                    dgMaster.Rows[rowIndex].Cells["appSate"].Tag = 3;
                    foreach (System.Reflection.PropertyInfo pi in obj.GetType().GetProperties())
                    {

                        if (dgMaster.Columns.Contains(pi.Name))
                        {

                            dgMaster[pi.Name, rowIndex].Value = ClientUtil.ToString(pi.GetValue(obj, null));
                            dgMaster[pi.Name, rowIndex].Tag = ClientUtil.ToString(pi.GetValue(obj, null));
                            if (dgMaster.Columns[pi.Name].ReadOnly == true)
                            {
                                dgMaster[pi.Name, rowIndex].ReadOnly = true;
                            }
                            else
                            {
                                dgMaster[pi.Name, rowIndex].ReadOnly = false;
                                dgMaster[pi.Name, rowIndex].Style.ForeColor = Color.Red;
                            }
                        }

                    }
                }
            }
            // dgMaster.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }

        private bool IsNotAppBill()
        {
            bool bFlag = false;
            if (this.dgMaster.Rows.Count == 0) return bFlag;
            DataGridViewRow oCurrentRow = this.dgMaster.CurrentRow;
            if (oCurrentRow == null) return bFlag;
            if (oCurrentRow.Cells["appSate"].Tag == null) return bFlag;
            if (string.Equals(oCurrentRow.Cells["appSate"].Tag.ToString(), "3"))
            {
                bFlag = true;
            }
            else
            {
                bFlag = false;
            }
            return bFlag;
        }
        private void BindDetialNoAppBill()
        {
            if (this.dgMaster.Rows.Count == 0) return;
            DataGridViewRow oCurrentRow = this.dgMaster.CurrentRow;
            if (oCurrentRow == null) return;
            if (oCurrentRow.Cells["appSate"].Tag == null) return;
            if (string.Equals(oCurrentRow.Cells["appSate"].Tag.ToString(), "3"))
            {
                object billMaster = oCurrentRow.Tag;
                string sBillID = ClientUtil.ToString(billMaster.GetType().GetProperty("Id").GetValue(billMaster, null));
                object detailsO = billMaster.GetType().GetProperty("Details").GetValue(billMaster, null);
                this.dgDetail.Rows.Clear();
                if (detailsO == null) return;
                IEnumerable set = detailsO as IEnumerable;
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
                        }
                    }
                }
                //dgDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                FillDoc(sBillID);

                BindNotAppBillNextStep(sBillID);
            }

            //ShowAppSetpSet(sAppTableSetID, sSolutionID, masterId, sNextStepID);
            //BillDocumentDisplay(masterId);
        }
        private void BindNextStep(string sBillId)
        {
            IList lst = Model.Service.GetAppBillPerNameByProc(sBillId);
            if (lst != null)
            {
                int rowIndex = FgAppSetpsInfo.Rows.Add();
                // FgAppSetpsInfo[StepName.Name, rowIndex].Value = lst[0].ToString();
                FgAppSetpsInfo[StepName.Name, rowIndex].Value = "审批下一步";

                FgAppSetpsInfo[StepName.Name, rowIndex].Style.ForeColor = Color.Blue;

                FgAppSetpsInfo[AppPerson.Name, rowIndex].Value = lst[1].ToString();
                FgAppSetpsInfo[AppPerson.Name, rowIndex].ToolTipText = lst[1].ToString();
            }
        }
        private void BindNotAppBillNextStep(string sBillId)
        {
            FgAppSetpsInfo.Rows.Clear();
            if (this.dgBill.Rows.Count == 0) return;
            DataGridViewRow oCurrentRow = this.dgBill.CurrentRow;
            AppTableSet oAppTableSet = oCurrentRow.Tag as AppTableSet;
            if (oAppTableSet != null)
            {
                string sPersonNames = Model.Service.GetSubmitBillPersonByName(sBillId, oAppTableSet.ClassName);
                int rowIndex = FgAppSetpsInfo.Rows.Add();
                // FgAppSetpsInfo[StepName.Name, rowIndex].Value = lst[0].ToString();
                FgAppSetpsInfo[StepName.Name, rowIndex].Value = "审批下一步";

                FgAppSetpsInfo[StepName.Name, rowIndex].Style.ForeColor = Color.Blue;

                FgAppSetpsInfo[AppPerson.Name, rowIndex].Value = sPersonNames;
                FgAppSetpsInfo[AppPerson.Name, rowIndex].ToolTipText = sPersonNames;
            }
        }
    }
}