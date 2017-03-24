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
using Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatMng;
using IRPServiceModel.Domain.Document;
using IRPServiceModel.Basic;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppProcessUI;
using Application.Business.Erp.SupplyChain.Client.FileUpload;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatformUI
{
    public partial class VAppPlatformNew : Application.Business.Erp.SupplyChain.Client.Basic.Template.TBasicDataView
    {
        private MAppPlatform Model = new MAppPlatform();
        private IList List = new ArrayList();
        private AppStepsInfo curAppStepsInfo = null;
        private IList list_AppSolution = new ArrayList ();
        IList list_AppStepsInfo = new ArrayList();
        CurrentProjectInfo projectInfo;
        string userName = string.Empty;
        string jobId = string.Empty;
        string fileObjectType = string.Empty;
        /// <summary>
        /// ��Ҫ�����ĵ���
        /// </summary>
        private Hashtable neededAuditBillHash = new Hashtable();
        private Hashtable neededAuditSolution = new Hashtable();
        private Hashtable neededAuditTableSet = new Hashtable();
        /// <summary>
        /// ��ǰ���ݶ�Ӧ����������
        /// </summary>
        private Hashtable BillIdAppStepsSetHash = new Hashtable();

        public VAppPlatformNew()
        {
            InitializeComponent();
            this.InitEvent();
            InitData();
        }
        void InitData()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
            DateBeg.Value = LoginInfomation.LoginInfo.LoginDate.AddMonths(-1);
            DateEnd.Value = LoginInfomation.LoginInfo.LoginDate;
            LoginInfomation.LoginInfo = AppDomain.CurrentDomain.GetData("TheLogin") as Login;
            this.label1.Text = "��ǰ��λ: " + LoginInfomation.LoginInfo.TheSysRole.RoleName;
            this.FgAppSetpsInfo.Size = new System.Drawing.Size(800, 150);
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
            dgDocumentMast.CellClick += new DataGridViewCellEventHandler(dgDocumentMast_CellClick);

            btnDownLoadDocument.Click += new EventHandler(btnDownLoadDocument_Click);
            btnOpenDocument.Click += new EventHandler(btnOpenDocument_Click);
            btnDocumentFileAdd.Click += new EventHandler(btnDocumentFileAdd_Click);
            btnDeleteDocumentFile.Click += new EventHandler(btnDeleteDocumentFile_Click);
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
                    sTitle = string.Format("��{0}��---����", oGird.Columns[e.ColumnIndex].HeaderText);
                    VShowContent oContent = new VShowContent(sTitle, sContent);
                    // Point p = new Point(e.X, e.Y);

                    oContent.StartPosition = FormStartPosition.CenterParent;
                    // oContent.Text = string.Format("X:{0};Y:{1}", e.X, e.Y);
                    oContent.ShowDialog();
                }
            }
        }

        #region �ĵ�
        MDocumentCategory msrv = new MDocumentCategory();
        //�����ĵ�����
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
        //����ļ�
        void btnDocumentFileAdd_Click(object sender, EventArgs e)
        {
            if (dgDocumentMast.SelectedRows.Count == 0) return;
            DocumentMaster docMaster = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
            VDocumentFileUpload frm = new VDocumentFileUpload(docMaster, "add");
            frm.ShowDialog();
            IList resultList = frm.ResultList;
            if (resultList != null && resultList.Count > 0)
            {
                foreach (DocumentDetail dtl in resultList)
                {
                    AddDgDocumentDetailInfo(dtl);
                    docMaster.ListFiles.Add(dtl);
                }
            }
        }
        //ɾ���ļ�
        void btnDeleteDocumentFile_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> rowList = new List<DataGridViewRow>();
            IList deleteFileList = new ArrayList();
            foreach (DataGridViewRow row in dgDocumentDetail.Rows)
            {
                if ((bool)row.Cells[FileSelect.Name].EditedFormattedValue)
                {
                    DocumentDetail dtl = row.Tag as DocumentDetail; ;
                    rowList.Add(row);
                    deleteFileList.Add(dtl);
                }
            }
            if (deleteFileList.Count == 0)
            {
                MessageBox.Show("�빴ѡҪɾ�������ݣ�");
                return;
            }
            if (MessageBox.Show("Ҫɾ����ǰ��ѡ�ļ���", "��ʾ", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (msrv.Delete(deleteFileList))
                {
                    MessageBox.Show("ɾ���ɹ���");
                    DocumentMaster master = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
                    foreach (DocumentDetail dtl in deleteFileList)
                    {
                        master.ListFiles.Remove(dtl);
                    }
                    dgDocumentMast.SelectedRows[0].Tag = master;
                }
                else
                {
                    MessageBox.Show("ɾ��ʧ�ܣ�");
                    return;
                }
                if (rowList != null && rowList.Count > 0)
                {
                    foreach (DataGridViewRow row in rowList)
                    {
                        dgDocumentDetail.Rows.Remove(row);
                    }
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
                MessageBox.Show("�빴ѡҪ���ص��ļ���");
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
                MessageBox.Show("��ѡ�ļ�����Ԥ������ѡ��ҪԤ�����ļ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    //string address = "http://10.70.18.203//TestFile//Files//0001//0001//0001.0001.000027V_��Ч.jpg";
                    string baseAddress = @docFile.TheFileCabinet.TransportProtocal.ToString().ToLower() + "://" + docFile.TheFileCabinet.ServerName + "/" + docFile.TheFileCabinet.Path + "/";

                    string address = baseAddress + docFile.FilePartPath;
                    UtilityClass.WebClientObj.DownloadFile(address, tempFileFullPath);
                    listFileFullPaths.Add(tempFileFullPath);
                }
                foreach (string fileFullPath in listFileFullPaths)
                {
                    FileInfo file = new FileInfo(fileFullPath);

                    //����һ��ProcessStartInfoʵ��
                    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                    //�����������̵ĳ�ʼĿ¼
                    info.WorkingDirectory = file.DirectoryName;//Application.StartupPath; 
                    //�����������̵�Ӧ�ó�����ĵ���
                    info.FileName = file.Name;
                    //�����������̵Ĳ���
                    info.Arguments = "";
                    //�����ɰ�������������Ϣ�Ľ�����Դ
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
                if (ex.Message.IndexOf("δ�������������õ������ʵ��") > -1)
                {
                    MessageBox.Show("����ʧ�ܣ������ڵ��ĵ�����", "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex), "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region �¼�
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
            string sAppTableSetID = string.Empty;
            string sSolutionID = string.Empty;
            string sNextStepID=string.Empty ;
            if (dgMaster.Rows.Count > 0)
            {
                if (dgMaster.SelectedRows.Count > 0 || dgMaster.SelectedCells.Count > 0)
                {
                    SelectRow = dgMaster.CurrentRow;
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
           // AppTableSet theAppTableSet = new AppTableSet();
            string masterId = "";
            if (BillRow != null)
            {
               // theAppTableSet = BillRow.Tag as AppTableSet;
                sAppTableSetID = BillRow.Tag as string;
                sSolutionID = BillRow.Cells[collBillName.Name].Tag as string;
                sNextStepID=BillRow.Cells[colCreatePerson.Name].Tag  as string;
                if (SelectRow != null)
                {
                    dgDetail.Rows.Clear();
                    masterId = ClientUtil.ToString(BillRow.Cells[colCode.Name].Tag);
                    if (masterId == null || masterId.Equals("")) return;

                    object billMaster = neededAuditBillHash[masterId];
                    if (billMaster == null)
                    {
                        IList list = Model.Service.GetBill(sAppTableSetID, masterId);
                        if (list == null && list.Count !=2)
                        {
                            return;
                        }
                        else
                        {
                            if (!neededAuditTableSet.ContainsKey(masterId))
                            {
                                neededAuditTableSet.Add(masterId, list[0]);
                            }
                            billMaster = list[1];
                            neededAuditBillHash.Add (masterId, billMaster);
                        }
                    }

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
                                //theAppDetailData.AppTableSet = theAppTableSet.Id;
                                theAppDetailData.AppTableSet = sAppTableSetID;
                                dgDetail[pi.Name, rowIndex].Tag = theAppDetailData;

                                //detaiList.Add(theAppDetailData);
                            }
                        }
                    }
                }
                //dgDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                ShowAppSetpSet(sAppTableSetID, sSolutionID, masterId, sNextStepID);
                this.FillDoc(masterId);
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
                string sAppTableSetID = thdCurrentRow.Tag as string;
                string sSolutionID = thdCurrentRow.Cells[collBillName.Name].Tag as string;
                if (string.IsNullOrEmpty(sAppTableSetID) || string.IsNullOrEmpty(sSolutionID)) return;
                IList List_MasterProperty = Model.GetAppMasterProperties(sAppTableSetID);
                IList List_DetailProperty = Model.GetAppDetailProperties(sAppTableSetID);
                IList List_TempMaster = new ArrayList();
                if (List_MasterProperty != null)
                {
                    int columnIndex = 0;
                    foreach (AppMasterPropertySet MasterProperty in List_MasterProperty)
                    {
                        //�������Ե����������
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
                            column.Width = 100;
                            column.Tag = MasterProperty;
                            dgMaster.Columns.Insert(columnIndex, column);
                            columnIndex++;
                        }
                    }
                    DataGridViewTextBoxColumn MasterIdColumn = new DataGridViewTextBoxColumn();
                    MasterIdColumn.HeaderText = "����ID";
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
                            column.Width = 100;
                            column.Tag = DetailProperty;
                            dgDetail.Columns.Insert(columnIndex, column);
                            columnIndex++;
                        }
                    }
                    DataGridViewTextBoxColumn DetailIdColumn = new DataGridViewTextBoxColumn();
                    DetailIdColumn.HeaderText = "��ϸID";
                    DetailIdColumn.Name = "ColDetailId";
                    DetailIdColumn.Visible = false;
                    dgDetail.Columns.Add(DetailIdColumn);
                }
                dgMaster.CellDoubleClick -= new DataGridViewCellEventHandler(DataGridView_CellDoubleClick);
                dgMaster.CellDoubleClick += new DataGridViewCellEventHandler(DataGridView_CellDoubleClick);
                dgDetail.CellDoubleClick -= new DataGridViewCellEventHandler(DataGridView_CellDoubleClick);
                dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(DataGridView_CellDoubleClick);

                string auditBillId = (string)thdCurrentRow.Cells[colCode.Name].Tag;
                DgMasterShowData(sAppTableSetID, sSolutionID, auditBillId);
            }
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
                        //��ͬ�����ĵ�����
                        DataGridViewRow thdCurrentRow = dgBill.SelectedRows[0];
                        if (thdCurrentRow != null)
                        {
                            string billCode = (string)thdCurrentRow.Cells[colCode.Name].Value;
                            if (ClientUtil.ToString(billCode).Contains("��ͬ����") && curAppStepsInfo.RoleName.Contains("������"))
                            {
                                this.btnDeleteDocumentFile.Visible = true;
                                this.btnDocumentFileAdd.Visible = true;
                            }
                            else
                            {
                                this.btnDeleteDocumentFile.Visible = false;
                                this.btnDocumentFileAdd.Visible = false;
                            }
                        }
                        this.BtnAppAgree.Enabled = true;
                        this.BtnDisagree.Enabled = true;
                    }
                }
            }
        }

        private void BtnDisagree_Click(object sender, EventArgs e)
        {
            string opinion = ClientUtil.ToString(textBox1.Text);
            if (opinion == "")
            {
                MessageBox.Show("����[��ͨ��]������д���������");
                return;
            }
            if (MessageBox.Show("ȷ��Ҫ������ͨ����", "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            IList AppMasterDataList = new ArrayList();
            IList AppDetailDataList = new ArrayList();

            IList AppMasterDataModify = new ArrayList();
            IList AppDetailDataModify = new ArrayList();


            //����û��Ƿ��޸��˵�����Ϣ(�������ϸ)
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

            AppMasterDataList = GetBillMasterMess(1);
            AppDetailDataList = GetBillDetailMess();

            DataGridViewRow thdCurrentRow = dgBill.SelectedRows[0];
            //AppTableSet tableSet = thdCurrentRow.Tag as AppTableSet;
            string sTableSet = thdCurrentRow.Tag as string;
            AppTableSet tableSet = Model.Service.GetAppTableSetByID(sTableSet);

            string BillId = dgBill.SelectedRows[0].Cells[colCode.Name].Tag + "";
            Model.Service.AppDisAgree(tableSet, FgAppSetpsInfo.SelectedRows[0].Tag as AppStepsInfo, textBox1.Text, BillId, AppMasterDataList, AppDetailDataList, AppMasterDataModify, AppDetailDataModify,null);

            MessageBox.Show("������ɣ�");
            Clear();
        }

        private void BtnAppAgree_Click(object sender, EventArgs e)
        {
            string opinion = ClientUtil.ToString(textBox1.Text);
            DataGridViewRow theCurrentRow = dgBill.SelectedRows[0];
            if (theCurrentRow != null)
            {
                string billCode = (string)theCurrentRow.Cells[colCode.Name].Value;
                if (ClientUtil.ToString(billCode).Contains("��ͬ����"))
                {
                    if (opinion == "")
                    {
                        MessageBox.Show("����ͬ���׻�ǩ��������д��ǩ�����");
                        return;
                    }
                }
            }

            IList AppMasterDataList = new ArrayList();
            IList AppDetailDataList = new ArrayList();
            IList AppMasterDataModify = new ArrayList();
            IList AppDetailDataModify = new ArrayList();

            //�жϵ�ǰ���������Ƿ������һ��
            bool IsFinish = true;
            AppSolutionSet theAppSolutionSet = list_AppSolution[0] as AppSolutionSet;
            //��ȡ���������һ��
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
            string BillId = dgBill.SelectedRows[0].Cells[colCode.Name].Tag + "";

            OperationRole currentRole = CurrentAppRole(currentSteps.AppStepsSet, BillId);
            currentSteps.AppRole = currentRole;
            currentSteps.RoleName = currentRole.RoleName;
            currentSteps.BillId = BillId;

            //2014-05-19
            /*if (currentSteps.AppRelations == 0)
            {
                //���ϵ
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
                //���ϵ ���������н�ɫ������ɺ���ܴ������
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
            }*/

            AppMasterDataList = GetBillMasterMess(2);
            AppDetailDataList = GetBillDetailMess();

            DataGridViewRow thdCurrentRow = dgBill.SelectedRows[0];
            //AppTableSet tableSet = thdCurrentRow.Tag as AppTableSet;
            AppTableSet tableSet = neededAuditTableSet[BillId] as AppTableSet;
            if (tableSet == null)
            {
                string sTableSet = thdCurrentRow.Tag as string;
                tableSet = Model.Service.GetAppTableSetByID(sTableSet);
                if (tableSet != null)
                {
                    neededAuditTableSet.Add(BillId, tableSet);
                }
            }
            Model.Service.AppAgree(tableSet,theAppSolutionSet, currentSteps, textBox1.Text, BillId, AppMasterDataList, AppDetailDataList, AppMasterDataModify, AppDetailDataModify,null);
            MAppPlatMng appModel = new MAppPlatMng();

            appModel.SendMessage(BillId, "");
            //#region ����
            //DataSet ds = Model.Service.GetAppingBillPerson(BillId);
            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    string sMsg = string.Empty;
            //    foreach (DataRow oRow in ds.Tables[0].Rows)
            //    {
            //        sMsg += "PERNAME:" + ClientUtil.ToString(oRow["PERNAME"]).PadLeft(20, ' ') + "  PERCODE:" + ClientUtil.ToString(oRow["PERCODE"]).PadLeft(20, ' ') + "\n";
            //    }
            //    MessageBox.Show(sMsg);
            //}
            //#endregion
            MessageBox.Show("������ɣ�");
            Clear();
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            Binding1();
        }
        #endregion

        #region ֧�ŷ���
        private void DgMasterShowData(string sAppTableSetID,string sSolutionID,string auditBillId)
        {
            if (string.IsNullOrEmpty(auditBillId) || string.IsNullOrEmpty(sSolutionID)) return;
            object obj = neededAuditBillHash[auditBillId];
            IList list = null;
            if (obj == null)
            {
                list = Model.Service.GetBill(sAppTableSetID, auditBillId);
                if (list == null || list .Count !=2)
               {
                   return;
               }
               else
               {
                   if (!neededAuditTableSet.ContainsKey(auditBillId))
                   {
                       neededAuditTableSet.Add(auditBillId, list[0]);
                   }
                   obj = list[1];
                   
                   neededAuditBillHash.Add(auditBillId, obj);
               }
            }
           

            int rowIndex = dgMaster.Rows.Add();
            DateTime currServerDateTime = Model.Service.GetServerDateTime(); ;
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
                    AppMasterData theAppMasterData = new AppMasterData();
                    theAppMasterData.PropertyName = pi.Name;
                    theAppMasterData.PropertyValue = ClientUtil.ToString(pi.GetValue(obj, null));
                    theAppMasterData.BillId = auditBillId;
                    theAppMasterData.PropertyChineseName = dgMaster.Columns[pi.Name].HeaderText;
                    theAppMasterData.PropertySerialNumber = dgMaster.Columns[pi.Name].Index;
                    theAppMasterData.AppDate = currServerDateTime;
                    theAppMasterData.ProjectId = StaticMethod.GetProjectInfo().Id;
                    theAppMasterData.AppStatus = 2L;
                    //theAppMasterData.AppTableSet = (dgBill.SelectedRows[0].Tag as AppTableSet).Id;
                    theAppMasterData.AppTableSet =  dgBill.SelectedRows[0].Tag as string  ;
                    dgMaster[pi.Name, rowIndex].Tag = theAppMasterData;

                    //masterData.Add(theAppMasterData);
                }
            }
            //dgMaster.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }

        void ShowAppSetpSet(string sAppTableSetID ,string sSolutionID, string BillId,string sNextStepID)
        {
            FgAppSetpsInfo.Rows.Clear();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BillId", BillId));
            oq.AddCriterion(Expression.Eq("AppStatus", ClientUtil.ToLong(2)));
            oq.AddCriterion(Expression.Eq("State", 1));
            oq.AddFetchMode("AuditPerson", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("AppRole", NHibernate.FetchMode.Eager);
            //string sWhere = string.Format("Id in (select t.id  from view_vaildAppStepInfo t where t.billid='{0}')", BillId);
            //oq.AddCriterion(Expression.Sql(sWhere));

            //��ǰ���ݵ�������Ϣ
            list_AppStepsInfo = Model.Service.GetAppStepsInfo(oq);
            object oAppSolutionSet = neededAuditSolution[sSolutionID];
            if (oAppSolutionSet == null)
            {
                oAppSolutionSet = Model.Service.GetAppSolution(sSolutionID);
                if (oAppSolutionSet == null)
                {
                    return;
                }
                else
                {
                    neededAuditSolution.Add(sSolutionID, oAppSolutionSet);
                    
                }
            }
            if (list_AppSolution != null)
            {
                list_AppSolution.Clear();
            }
            list_AppSolution.Add(oAppSolutionSet);

            #region ������ͨ��������������Ϣ
            foreach (AppStepsInfo master in list_AppStepsInfo)
            {
                int index = FgAppSetpsInfo.Rows.Add();

                FgAppSetpsInfo[StepOrder.Name, index].Value = ClientUtil.ToLong(master.StepOrder);
                FgAppSetpsInfo[StepName.Name, index].Value = ClientUtil.ToString(master.StepsName);
                if (master.AppRelations == 0)
                {
                    FgAppSetpsInfo[AppRelations.Name, index].Value = "��";
                }
                else
                {
                    FgAppSetpsInfo[AppRelations.Name, index].Value = "��";
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
                        FgAppSetpsInfo[AppStatus.Name, index].Value = "�ѳ���";
                        break;
                    case 0:
                        FgAppSetpsInfo[AppStatus.Name, index].Value = "������";
                        break;
                    case 1:
                        FgAppSetpsInfo[AppStatus.Name, index].Value = "δͨ��";
                        break;
                    case 2:
                        FgAppSetpsInfo[AppStatus.Name, index].Value = "��ͨ��";
                        break;
                    default:
                        break;
                }
            }
            #endregion

            #region �������Ĳ����ȱʡ��Ϣ

            int rowIndex = FgAppSetpsInfo.Rows.Add();
            AppStepsSet ReadyAppStepsSet = BillIdAppStepsSetHash[BillId] as AppStepsSet;
            if (ReadyAppStepsSet == null)
            {
               ReadyAppStepsSet= Model.Service.GetAppStepSetByStepID(sNextStepID);
               if (ReadyAppStepsSet != null)
               {
                   BillIdAppStepsSetHash.Add(BillId, ReadyAppStepsSet);
               }
            }
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
                FgAppSetpsInfo[AppRelations.Name, rowIndex].Value = "��";
            }
            else
            {
                FgAppSetpsInfo[AppRelations.Name, rowIndex].Value = "��";
            }
            FgAppSetpsInfo[AppRole.Name, rowIndex].Value = currentRole.RoleName;
            FgAppSetpsInfo[AppRole.Name, rowIndex].Tag = currentRole;
            FgAppSetpsInfo[AppDateTime.Name, rowIndex].Value = Model.Service.GetServerDateTime();
            FgAppSetpsInfo.Rows[rowIndex].Tag = theAppStepsInfo;

            this.BtnAppAgree.Enabled = true;
            this.BtnDisagree.Enabled = true;
           
            #endregion

            FgAppSetpsInfo_SelectionChanged(null, new EventArgs());
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

        /// <summary>
        /// ����������Ĺ�ϵΪ�루1��ʱ �ж���������Ľ�ɫ�Ƿ��Ѿ�ȫ��������� ȫ����ɷ���true
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
        /// ��������ͨ��ʱ�ж� �Ƿ����
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

            //��ǰ���ݵ�������Ϣ
            list_AppStepsInfo = Model.Service.GetAppStepsInfo(oq);
            if (list_AppStepsInfo == null)
            {
                list_AppStepsInfo = new ArrayList();
            }
            list_AppStepsInfo.Add(currentSteps);

            return AllRolePassed(list_AppStepsInfo, currentSteps.AppStepsSet);
        }

        /// <summary>
        /// ��ǰ�����Ľ�ɫ
        /// </summary>
        /// <param name="appStepsSet"></param>
        /// <returns></returns>
        private OperationRole CurrentAppRole(AppStepsSet appStepsSet, string BillId)
        {
            //��ǰ��¼�û��Ľ�ɫ
            if (appStepsSet != null)
            {
                IList userRolelst = GetOperationRoleByJobId(ConstObject.TheSysRole.Id);
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("BillId", BillId));
                oq.AddCriterion(Expression.Eq("AppStatus", ClientUtil.ToLong(2)));
                oq.AddCriterion(Expression.Eq("State", 1));
                oq.AddCriterion(Expression.Eq("StepOrder", appStepsSet.StepOrder));
                oq.AddFetchMode("AuditPerson", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("AppRole", NHibernate.FetchMode.Eager);
                //��ǰ���ݵ�������Ϣ
                list_AppStepsInfo = Model.Service.GetAppStepsInfo(oq);
                foreach (OperationRole role in userRolelst)
                {
                    foreach (AppRoleSet appRoleSet in appStepsSet.AppRoleSets)
                    {
                        if (role.Id == appRoleSet.AppRole.Id)
                        {
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
            }
            return null;
        }

        //OptrType�� 1Ϊ��ͨ�� 2Ϊͨ��
        private IList GetBillMasterMess(long OptrType)
        {
            IList list = new ArrayList();
            foreach (DataGridViewRow row in dgMaster.SelectedRows)
            {
                foreach (DataGridViewCell Column in row.Cells)
                {
                    if (Column.Visible == true)
                    {
                        AppMasterData master = Column.Tag as AppMasterData;
                        master.AppStatus = OptrType;
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
        #endregion

        #region ����������(����б�)
        private IList CurrentUserAppTableSet()
        {           
            //��ǰ��¼�û��Ľ�ɫ
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
            Binding1();
        }

        public void Binding1()
        {
            dgBill.SelectionChanged -= new EventHandler(dgBill_SelectionChanged);
            dgMaster.SelectionChanged -= new EventHandler(dgMaster_SelectionChanged);
            string sBillCode = string.Empty;
            string sBillCreateDate = string.Empty;
            string sBillCreatePerson = string.Empty;
            string sSolutionID = string.Empty;
            string sSolutionName = string.Empty;
            string sBillID = string.Empty;
            string sTableID = string.Empty;
            string sNextStepID = string.Empty;
            string sErrMsg = string.Empty;
            int i = 0;
            DataGridViewRow dr = null;
            DateTime dtBegin;
            DateTime dtEnd;
            dgBill.Rows.Clear();
            bool IsProce = true    ;
            //DataTable oTable1 = Model.Service.GetProc(ConstObject.TheSysRole.Id);
            //"billCode", "billSysCode", "billCreateDate", "OrgSysCodes", "billCreatePerson", "solutionID", "SolutionName" 
            if (CBoxDate.Checked)
            {
                dtBegin = DateBeg.Value;
                dtEnd = this.DateEnd.Value;
            }
            else
            {
                dtBegin = DateBeg.Value.AddMonths (-12);
                dtEnd = this.DateEnd.Value;
            }
            DataTable oTable = null;
            if (!IsProce)
            {
                oTable = Model.Service.GetAppBill(ConstObject.TheSysRole.Id, ConstObject.TheOperationOrg.SysCode, StaticMethod.GetProjectInfo().Id, dtBegin, dtEnd);
            }
            else
            {
                oTable = Model.Service.GetAppBillByProc(ConstObject.TheSysRole.Id, ConstObject.TheOperationOrg.SysCode, StaticMethod.GetProjectInfo().Id, dtBegin, dtEnd,ref sErrMsg);
                if (!string.IsNullOrEmpty(sErrMsg))
                {
                    MessageBox.Show(sErrMsg );
                }
            }
            if (oTable != null)
            {
                foreach (DataRow oRow in oTable.Rows)
                {
                    sBillCode = oRow["billCode"].ToString();
                    sBillCreateDate = oRow["billCreateDate"].ToString();
                    sBillCreatePerson = oRow["billCreatePerson"].ToString();
                    sSolutionID = oRow["solutionID"].ToString();
                    sSolutionName = oRow["SolutionName"].ToString();
                    sBillID = oRow["billID"].ToString();
                    sTableID = oRow["TableID"].ToString();
                    sNextStepID = oRow["OrgSysCodes"].ToString();
                    if (!IsProce)
                    {
                        sNextStepID = sNextStepID.Substring(0, sNextStepID.IndexOf("-"));
                    }
                    i = dgBill.Rows.Add();
                    dr = dgBill.Rows[i];

                    dr.Tag = sTableID;//���������
                    dr.Cells[colCode.Name].Value = sBillCode;//����code
                    dr.Cells[colCode.Name].Tag = sBillID;//����ID
                    dr.Cells[colCreatePerson.Name].Value = sBillCreatePerson;//���ݴ�����
                    dr.Cells[colCreatePerson.Name].Tag = sNextStepID;
                    dr.Cells[colCreateDate.Name].Value = sBillCreateDate;//���ݴ���ʱ��
                    dr.Cells[collBillName.Name].Value = sSolutionName;//������������
                    dr.Cells[collBillName.Name].Tag = sSolutionID;//��������ID
                }
            }
            dgBill.SelectionChanged += new EventHandler(dgBill_SelectionChanged);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            if (dgBill.Rows.Count > 0)
            {
                //dgBill.CurrentCell = dgBill.Rows[0].Cells[collBillName.Name];
                dgBill_SelectionChanged(dgBill, new EventArgs());
            }

        }
        #endregion   
    }
}