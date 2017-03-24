using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng;
using Application.Business.Erp.SupplyChain.Client.FileUpload;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using IRPServiceModel.Basic;
using IRPServiceModel.Domain.Document;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Core;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    public partial class VContractDisclosure : TMasterDetailView
    {
        private MCostMonthAccount model = new MCostMonthAccount();
        private MDocumentCategory msrv = new MDocumentCategory();
        private DisclosureMaster disMaster;
        private DisclosureDetail disDetail;
        private CurrentProjectInfo projectInfo;

        public VContractDisclosure()
        {
            InitializeComponent();
            InitData();
            InitEvent();
        }
        public void InitData()
        {
            ((ComboBox)comConstractType).Items.AddRange(Enum.GetNames(typeof(SubContractType)));
            VisualOperationOrg();
        }
        private void InitEvent()
        {
            this.btnSelect.Click += new EventHandler(btnSelect_Click);
            //相关文档
            btnDownLoadDocument.Click += new EventHandler(btnDownLoadDocument_Click);
            btnOpenDocument.Click += new EventHandler(btnOpenDocument_Click);
            btnUpdateDocument.Click += new EventHandler(btnUpdateDocument_Click);
            btnDeleteDocumentFile.Click += new EventHandler(btnDeleteDocumentFile_Click);
            btnUpFile.Click += new EventHandler(btnUpFile_Click);
            dgDocumentMast.CellClick += new DataGridViewCellEventHandler(dgDocumentMast_CellClick);
            btnDeleteDocumentMaster.Click += new EventHandler(btnDeleteDocumentMaster_Click);
            btnDocumentFileAdd.Click += new EventHandler(btnDocumentFileAdd_Click);
            btnDocumentFileUpdate.Click += new EventHandler(btnDocumentFileUpdate_Click);
            lnkCheckAll.Click += new EventHandler(lnkCheckAll_Click);
            lnkCheckAllNot.Click += new EventHandler(lnkCheckAllNot_Click);
        }

        private void VisualOperationOrg()
        {
            bool flag = false;
            projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
            {
                flag = true;
            }
            else
            {
                txtProName.Tag = projectInfo;
                txtProName.Text = projectInfo.Name;
                flag = false;
            }
            this.btnSelect.Visible = flag;
        }

        void btnSelect_Click(object sender, EventArgs e)
        {
            VSelectProjectInfo project = new VSelectProjectInfo();
            CurrentProjectInfo extProject = new CurrentProjectInfo();
            project.ListExtendProject.Add(extProject);
            project.ShowDialog();

            if (project.Result != null && project.Result.Count > 0)
            {
                CurrentProjectInfo selectProject = project.Result[0] as CurrentProjectInfo;
                if (selectProject != null)
                {
                    txtProName.Text = selectProject.Name;
                    txtProName.Tag = selectProject;
                }
            }
        }

        #region 文档操作
        //文档按钮状态
        private void btnStates(int i)
        {
            if (i == 0)
            {
                //btnDownLoadDocument.Enabled = false;
                //btnOpenDocument.Enabled = false;
                btnUpdateDocument.Enabled = false;
                btnDeleteDocumentFile.Enabled = false;
                btnUpFile.Enabled = false;
                btnDeleteDocumentMaster.Enabled = false;
                btnDocumentFileAdd.Enabled = false;
                btnDocumentFileUpdate.Enabled = false;
                lnkCheckAll.Enabled = false;
                lnkCheckAllNot.Enabled = false;
            }
            if (i == 1)
            {
                //btnDownLoadDocument.Enabled = true;
                //btnOpenDocument.Enabled = true;
                btnUpdateDocument.Enabled = true;
                btnDeleteDocumentFile.Enabled = true;
                btnUpFile.Enabled = true;
                btnDeleteDocumentMaster.Enabled = true;
                btnDocumentFileAdd.Enabled = true;
                btnDocumentFileUpdate.Enabled = true;
                lnkCheckAll.Enabled = true;
                lnkCheckAllNot.Enabled = true;
            }
        }
        //加载文档数据
        void FillDoc()
        {
            dgDocumentMast.Rows.Clear();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", disMaster.Id));
            IList listObj = model.ObjectQuery(typeof(ProObjectRelaDocument), oq);
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
                IList docList = model.ObjectQuery(typeof(DocumentMaster), oq);
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
        //添加文件
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
        //修改文件
        void btnDocumentFileUpdate_Click(object sender, EventArgs e)
        {
            if (dgDocumentMast.SelectedRows.Count == 0 || dgDocumentDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要修改的文档件！");
                return;
            }
            DocumentMaster docMaster = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
            VDocumentFileModify frm = new VDocumentFileModify(docMaster);
            frm.ShowDialog();
            IList resultList = frm.ResultList;
            if (resultList != null && resultList.Count > 0)
            {
                DocumentDetail dtl = resultList[0] as DocumentDetail;
                AddDgDocumentDetailInfo(dtl, dgDocumentDetail.SelectedRows[0].Index);
                for (int i = 0; i < docMaster.ListFiles.Count; i++)
                {
                    DocumentDetail detail = docMaster.ListFiles.ElementAt(i);
                    if (detail.Id == dtl.Id)
                    {
                        detail = dtl;
                    }
                }
            }
        }
        //下载
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
        //预览
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
        //删除文件
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
                MessageBox.Show("请勾选要删除的数据！");
                return;
            }
            if (MessageBox.Show("要删除当前勾选文件吗！", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (msrv.Delete(deleteFileList))
                {
                    MessageBox.Show("删除成功！");
                    DocumentMaster master = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
                    foreach (DocumentDetail dtl in deleteFileList)
                    {
                        master.ListFiles.Remove(dtl);
                    }
                    dgDocumentMast.SelectedRows[0].Tag = master;
                }
                else
                {
                    MessageBox.Show("删除失败！");
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
        //添加文档（加文件）
        void btnUpFile_Click(object sender, EventArgs e)
        {
            if (disMaster.Id == null)
            {
                if (MessageBox.Show("当前业务对象还没保存，是否保存！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    if (!ValidView()) return;
                    try
                    {
                        if (!ViewToModel()) return;
                        disMaster = model.CostMonthAccSrv.SaveContractDisclosure(disMaster);
                        this.ViewCaption = ViewName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(ex));
                    }
                }
            }
            if (disMaster.Id != null)
            {
                VDocumentPublicUpload frm = new VDocumentPublicUpload(disMaster.Id);
                frm.ShowDialog();
                DocumentMaster resultDoc = frm.Result;
                if (resultDoc == null) return;
                AddDgDocumentMastInfo(resultDoc);
                dgDocumentMast.CurrentCell = dgDocumentMast[2, dgDocumentMast.Rows.Count - 1];
                dgDocumentDetail.Rows.Clear();
                if (resultDoc.ListFiles != null && resultDoc.ListFiles.Count > 0)
                {
                    foreach (DocumentDetail dtl in resultDoc.ListFiles)
                    {
                        AddDgDocumentDetailInfo(dtl);
                    }
                }
            }
        }
        //修改文档（加文件）
        void btnUpdateDocument_Click(object sender, EventArgs e)
        {
            if (dgDocumentMast.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要修改的文档！");
                return;
            }
            DocumentMaster master = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
            IList docFileList = new ArrayList();
            foreach (DataGridViewRow row in dgDocumentDetail.Rows)
            {
                DocumentDetail dtl = row.Tag as DocumentDetail;
                docFileList.Add(dtl);
            }
            VDocumentPublicModify frm = new VDocumentPublicModify(master, docFileList);
            frm.ShowDialog();
            DocumentMaster resultMaster = frm.Result;
            if (resultMaster == null) return;
            AddDgDocumentMastInfo(resultMaster, dgDocumentMast.SelectedRows[0].Index);
            dgDocumentMast_CellClick(dgDocumentMast, new DataGridViewCellEventArgs(2, dgDocumentMast.SelectedRows[0].Index));
        }
        //删除文档
        void btnDeleteDocumentMaster_Click(object sender, EventArgs e)
        {
            if (dgDocumentMast.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的文档！");
                return;
            }
            if (MessageBox.Show("要删除当前文档吗？该操作将连它的所有文件一同删除！", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DocumentMaster mas = dgDocumentMast.SelectedRows[0].Tag as DocumentMaster;
                IList list = new ArrayList();
                list.Add(mas);

                if (msrv.Delete(list))
                {
                    MessageBox.Show("删除成功！");
                    dgDocumentMast.Rows.Remove(dgDocumentMast.SelectedRows[0]);
                    if (dgDocumentMast.Rows.Count > 0)
                        dgDocumentMast_CellClick(dgDocumentMast, new DataGridViewCellEventArgs(2, dgDocumentMast.SelectedRows[0].Index));
                    else
                        dgDocumentDetail.Rows.Clear();
                }
                else
                {
                    MessageBox.Show("删除失败！");
                }
            }
        }
        void dgDocumentMast_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            dgDocumentDetail.Rows.Clear();
            DocumentMaster docMaster = dgDocumentMast.Rows[e.RowIndex].Tag as DocumentMaster;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", docMaster.Id));
            oq.AddFetchMode("TheFileCabinet", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof(DocumentDetail), oq);
            if (list != null && list.Count > 0)
            {
                foreach (DocumentDetail docDetail in list)
                {
                    AddDgDocumentDetailInfo(docDetail);
                }
            }
        }

        #region 列表里添加数据
        void AddDgDocumentMastInfo(DocumentMaster m)
        {
            int rowIndex = dgDocumentMast.Rows.Add();
            AddDgDocumentMastInfo(m, rowIndex);
        }
        void AddDgDocumentMastInfo(DocumentMaster m, int rowIndex)
        {
            dgDocumentMast[colDocumentName.Name, rowIndex].Value = m.Name;
            dgDocumentMast[colCreateTime.Name, rowIndex].Value = m.CreateTime;
            dgDocumentMast[colDocumentCode.Name, rowIndex].Value = m.Code;
            dgDocumentMast[colDocumentInforType.Name, rowIndex].Value = m.DocType.ToString();
            dgDocumentMast[colDocumentState.Name, rowIndex].Value = ClientUtil.GetDocStateName(m.State);
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
            //dgDocumentDetail[FileExtension.Name, rowIndex].Value = d.ExtendName;
            //dgDocumentDetail[FilePath.Name, rowIndex].Value = d.FilePartPath;
            dgDocumentDetail.Rows[rowIndex].Tag = d;
        }
        #endregion

        //反选
        void lnkCheckAllNot_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgDocumentDetail.Rows)
            {
                row.Cells[FileSelect.Name].Value = false;
            }
        }
        //全选
        void lnkCheckAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgDocumentDetail.Rows)
            {
                row.Cells[FileSelect.Name].Value = true;
            }
        }
        #endregion

        public void Start(string Id)
        {
            try
            {
                if (Id == "空")
                {
                    RefreshState(MainViewState.Initialize);
                }

                else
                {
                    btnDownLoadDocument.Enabled = true;
                    btnOpenDocument.Enabled = true;
                    disMaster = model.CostMonthAccSrv.GetContractDisclosureById(Id);
                    ModelToView();
                    RefreshState(MainViewState.Browser);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("视图启动出错：" + ExceptionUtil.ExceptionMessage(e));

            }
        }

        public override void RefreshState(MainViewState state)
        {
            base.RefreshState(state);
            switch (state)
            {
                case MainViewState.AddNew:
                case MainViewState.Modify:
                    txtDesc.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    txtDesc.Enabled = false;
                    this.txtPersonCreate.ReadOnly = true;
                    this.txtProName.ReadOnly = true;
                    break;
                default:
                    break;
            }
        }

        public override void RefreshControls(MainViewState state)
        {
            base.RefreshControls(state);
            if (ViewState == MainViewState.AddNew || ViewState == MainViewState.Modify)
            {
                ObjectLock.Unlock(pnlFloor, true);
            }
            else
            {
                ObjectLock.Lock(pnlFloor, true);
            }
            object[] os = new object[] { txtCode, txtProName, txtPersonCreate };
            ObjectLock.Lock(os);
        }

        private bool ValidView()
        {
            if (ClientUtil.isEmpty(ClientUtil.ToString(this.txtProName.Text)))
            {
                MessageBox.Show("项目名称不能为空!");
                return false;
            }
            if (ClientUtil.isEmpty(ClientUtil.ToString(this.txtContName.Text)))
            {
                MessageBox.Show("合同名称不能为空!");
                return false;
            }
            if (ClientUtil.isEmpty(ClientUtil.ToString(this.txtBearerName.Text)))
            {
                MessageBox.Show("分包单位名称不能为空!");
                return false;
            }
            if (Convert.ToDateTime(this.dtDisclosureDate.Value) <= Convert.ToDateTime(DateTime.Now.ToString()))
            {
                this.dtDisclosureDate.Value = Convert.ToDateTime(this.dtDisclosureDate.Value);
            }
            else
            {
                MessageBox.Show("交底时间不可选择当天之后的日期!");
                return false;
            }

            return true;
        }

        public override bool NewView()
        {
            try
            {
                base.NewView();
                ClearView();
                this.disMaster = new DisclosureMaster();
                this.disDetail = new DisclosureDetail();                      
                disMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                disMaster.CreatePerson = ConstObject.LoginPersonInfo;
                
                txtPersonCreate.Tag = ConstObject.LoginPersonInfo;
                txtPersonCreate.Text = ConstObject.LoginPersonInfo.Name;
                this.ModelToView();

            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            return true;
        }

        public override bool SubmitView()
        {
            try
            {
                this.txtCode.Focus();
                if (!ViewToModel()) return false;
                disMaster.DocState = DocumentState.InAudit;
                disMaster = model.CostMonthAccSrv.SaveContractDisclosure(disMaster);
                this.txtCode.Text = disMaster.Code;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }


        public override bool SaveView()
        {
            this.txtCode.Focus();
            try
            {
                if (!ViewToModel()) return false;
               
                disMaster = model.CostMonthAccSrv.SaveContractDisclosure(disMaster);
                txtCode.Text = disMaster.Code;
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                MessageBox.Show("保存成功！");
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }

        public override bool ModifyView()
        {
            if (disMaster.DocState == DocumentState.Edit || disMaster.DocState == DocumentState.Valid)
            {
                base.ModifyView();
                disMaster = model.CostMonthAccSrv.GetContractDisclosureById(disMaster.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(disMaster.DocState));
            MessageBox.Show(message);
            return false;
        }

        public override bool DeleteView()
        {
            try
            {
                disMaster = model.CostMonthAccSrv.GetContractDisclosureById(disMaster.Id);
                if (disMaster.DocState == DocumentState.Valid || disMaster.DocState == DocumentState.Edit)
                {
                    if (!msrv.DeleteReceiptAndDocument(disMaster, disMaster.Id)) return false;
                    ClearView();
                    MessageBox.Show("删除成功！");
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(disMaster.DocState) + "】，不能删除！");
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据删除错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }


        public override bool CancelView()
        {
            try
            {
                switch (ViewState)
                {
                    case MainViewState.Modify:
                        disMaster = model.CostMonthAccSrv.GetContractDisclosureById(disMaster.Id);
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



        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
                {
                    disMaster.ProjectId = projectInfo.Id;
                    disMaster.ProjectName = projectInfo.Name;
                    disMaster.OperOrgInfo = ConstObject.TheOperationOrg;
                    disMaster.OpgSysCode = ConstObject.TheOperationOrg.SysCode;
                    disMaster.OperOrgInfoName = ConstObject.TheOperationOrg.Name;
                }
                else
                {
                    if (!ClientUtil.isEmpty(this.txtProName.Tag)) 
                    {
                        CurrentProjectInfo selectProInfo = this.txtProName.Tag as CurrentProjectInfo;
                        disMaster.ProjectId = selectProInfo.Id;
                        disMaster.ProjectName = selectProInfo.Name;
                        OperationOrgInfo orginfo = model.CommonMethodSrv.GetOperationOrgInfo(selectProInfo.OwnerOrg.Id);
                        disMaster.OperOrgInfo = orginfo;
                        disMaster.OpgSysCode = orginfo.SysCode;
                        disMaster.OperOrgInfoName = orginfo.Name;
                    }
                }
                disMaster.ProjectName = ClientUtil.ToString(this.txtProName.Text);
                disMaster.CreateDate = ClientUtil.ToDateTime(this.dtDisclosureDate.Text);
                disMaster.ContractName = ClientUtil.ToString(this.txtContName.Text);
                disMaster.BearerOrgName = ClientUtil.ToString(this.txtBearerName.Text);
                disDetail.SubPackage = ClientUtil.ToString(this.txtSubPackage.Text);
                disDetail.ContractType = ClientUtil.ToString(comConstractType.SelectedItem);
                disDetail.ContractInterimMoney = ClientUtil.ToDecimal(this.txtContractMoney.Text);
                disDetail.QualityBreachDuty = ClientUtil.ToString(this.txtQuality.Text);
                disDetail.SafetyBreachDuty = ClientUtil.ToString(this.txtSafe.Text);
                disDetail.CivilizedBreachDuty = ClientUtil.ToString(this.txtCivil.Text);
                disDetail.DurationBreachDuty = ClientUtil.ToString(this.txtDuration.Text);
                disDetail.LaborDemand = ClientUtil.ToString(this.txtLabor.Text);
                disDetail.MaterialDemand = ClientUtil.ToString(this.txtMaterial.Text);
                disDetail.PaymentType = ClientUtil.ToString(this.txtPayment.Text);
                disDetail.WarrantyDateMoney = ClientUtil.ToString(this.txtWarranty.Text);
                disDetail.OtherDesc = ClientUtil.ToString(this.txtDesc.Text);
                disMaster.AddDetail(disDetail);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        public void ModelToView()
        {
            try
            {
                disDetail = (DisclosureDetail)disMaster.Details.ElementAtOrDefault(0);
                if (disDetail == null)
                {
                    disDetail = new DisclosureDetail();
                }
                this.txtCode.Text = disMaster.Code;
                this.txtProName.Text = ClientUtil.ToString(disMaster.ProjectName);
                this.txtPersonCreate.Text = ClientUtil.ToString(disMaster.CreatePersonName);
                if (disMaster.CreateDate > ClientUtil.ToDateTime("2000-01-01"))
                {
                    this.dtDisclosureDate.Text = disMaster.CreateDate.ToShortDateString();
                }
                this.txtContName.Text = ClientUtil.ToString(disMaster.ContractName);
                this.txtBearerName.Text = ClientUtil.ToString(disMaster.BearerOrgName);

                this.txtSubPackage.Text = ClientUtil.ToString(disDetail.SubPackage);
                this.comConstractType.SelectedItem = ClientUtil.ToString(disDetail.ContractType);
                this.txtContractMoney.Text = ClientUtil.ToString(disDetail.ContractInterimMoney);
                this.txtQuality.Text = ClientUtil.ToString(disDetail.QualityBreachDuty);
                this.txtSafe.Text = ClientUtil.ToString(disDetail.SafetyBreachDuty);
                this.txtCivil.Text = ClientUtil.ToString(disDetail.CivilizedBreachDuty);
                this.txtDuration.Text = ClientUtil.ToString(disDetail.DurationBreachDuty);
                this.txtLabor.Text = ClientUtil.ToString(disDetail.LaborDemand);
                this.txtMaterial.Text = ClientUtil.ToString(disDetail.MaterialDemand);
                this.txtPayment.Text = ClientUtil.ToString(disDetail.PaymentType);
                this.txtWarranty.Text = ClientUtil.ToString(disDetail.WarrantyDateMoney);
                this.txtDesc.Text = ClientUtil.ToString(disDetail.OtherDesc);
                FillDoc();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

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
            this.txtDesc.Text = "";
            this.txtCode.Text = "";
            this.txtProName.Name = "";
            this.txtProName.Tag = null;
            if(c is TextBox)
            {
                c.Text = "";
            }
            if (c is CustomEdit)
            {
                c.Tag = null;
                c.Text = "";
            }
            else if (c is VirtualMachine.Component.WinControls.Controls.CustomDataGridView)
            {
                (c as CustomDataGridView).Rows.Clear();
            }
        }

    }
}

