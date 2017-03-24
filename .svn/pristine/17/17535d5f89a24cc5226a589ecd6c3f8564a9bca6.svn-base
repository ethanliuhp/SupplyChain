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
using Application.Business.Erp.SupplyChain.ImplementationPlan.Domain;
using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using System.IO;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using IRPServiceModel.Domain.Document;
using Application.Business.Erp.SupplyChain.Client.FileUpload;
using IRPServiceModel.Basic;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ImplementationPlan
{
    public partial class VImplementationMaintain : TMasterDetailView
    {
        private MImplementationPlan model = new MImplementationPlan();
        private ImplementationMaintain immaster;
        CurrentProjectInfo projectInfo;
        string userName = string.Empty;
        string jobId = string.Empty;
        string fileObjectType = string.Empty;
        IList booklist = new ArrayList();
        public VImplementationMaintain()
        {
            InitializeComponent();
            InitData();
            InitEvent();
        }
        public ImplementationMaintain Immaster
        {
            get { return immaster; }
            set { immaster = value; }
        }

        public void InitData()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
            this.ColUnitType.Items.AddRange(new object[] { "建设单位", "监理单位", "设计单位", "施工单位" });
            VBasicDataOptr.InitStructFrom(txtConstructionStyle, false);
            VBasicDataOptr.InitProjectLivel(cmoQualityObj, false);
            RefreshControls(MainViewState.Browser);
        }

        public void InitEvent()
        {
            txtCoveredArea.tbTextChanged += new EventHandler(txtCoveredArea_tbTextChanged);
            txtPeriodTarget.tbTextChanged += new EventHandler(txtPeriodTarget_tbTextChanged);
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

        #region 文档操作
        MDocumentCategory msrv = new MDocumentCategory();
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
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", immaster.Id));
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
            if (immaster.Id == null)
            {
                if (MessageBox.Show("当前业务对象还没保存，是否保存！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    if (!ValidView()) return;
                    try
                    {
                        if (!ViewToModel()) return;
                        immaster = model.saveImp(immaster);
                        this.ViewCaption = ViewName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(ex));
                    }
                }
            }
            if (immaster.Id != null)
            {
                VDocumentPublicUpload frm = new VDocumentPublicUpload(immaster.Id);
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
                btnStates(0);
                if (Id == "空")
                    RefreshState(MainViewState.Initialize);

                else
                {
                    btnDownLoadDocument.Enabled = true;
                    btnOpenDocument.Enabled = true;
                    Immaster = model.ImplementSrv.GetImpById(Id);
                    ModelToView();
                    RefreshState(MainViewState.Browser);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("视图启动出错：" + ExceptionUtil.ExceptionMessage(e));

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
                    txtConstructionStyle.Enabled = true;
                    cmoQualityObj.Enabled = true;
                    this.dgdetails.EditMode = DataGridViewEditMode.EditOnEnter;
                    //cmsDg.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    txtConstructionStyle.Enabled = false;
                    cmoQualityObj.Enabled = false;
                    this.dgdetails.EditMode = DataGridViewEditMode.EditProgrammatically;
                    //cmsDg.Enabled = false;
                    break;
                default:
                    break;
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
                ObjectLock.Unlock(pnlFloor, true);
                btnStates(1);
            }
            else
            {
                ObjectLock.Lock(pnlFloor, true);
                btnStates(0);
            }
            //永久锁定
            object[] os = new object[] { txtPlanName, txtPersonCharge };
            ObjectLock.Lock(os);

        }


        /// <summary>
        /// 验证
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            if (this.dgdetails.Rows.Count - 1 == 0)
            {
                MessageBox.Show("明细不能为空！");
                return false;
            }

            if (ClientUtil.ToString(this.txtTextName.Text) == "")
            {
                MessageBox.Show("文档名称不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.txtConstructionStyle.SelectedItem) == "")
            {
                MessageBox.Show("结构类型不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.txtCoveredArea.Text) == "")
            {
                MessageBox.Show("建筑面积不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.txtLvConstruction.Text) == "")
            {
                MessageBox.Show("层次结构不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.txtPeriodTarget.Text) == "")
            {
                MessageBox.Show("工期目标不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.txtEnGoal.Text) == "")
            {
                MessageBox.Show("环保目标不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.txtProfessionSafe.Text) == "")
            {
                MessageBox.Show("职业安全健康管理目标不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.txtCostObjective.Text) == "")
            {
                MessageBox.Show("成本目标不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.txtTechTarget.Text) == "")
            {
                MessageBox.Show("技术管理目标不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.txtObjectiveName.Text) == "")
            {
                MessageBox.Show("资金管理目标不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.txtCiTarget.Text) == "")
            {
                MessageBox.Show("CI目标不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.txtCashProject.Text) == "")
            {
                MessageBox.Show("项目预兑现不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.txtOwnerPayment.Text) == "")
            {
                MessageBox.Show("业主支付方式不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.txtDoteamPayStyle.Text) == "")
            {
                MessageBox.Show("劳务队伍支付方式不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.txtTeamPayStyle.Text) == "")
            {
                MessageBox.Show("分包队伍支付方式不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.txtMaterialPaystype.Text) == "")
            {
                MessageBox.Show("材料供应商支付方式不能为空！");
                return false;
            }

            if (ClientUtil.ToString(this.txtEngChoice.Text) == "")
            {
                MessageBox.Show("工程分包选择不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.txtThingsBuy.Text) == "")
            {
                MessageBox.Show("物资采购不能为空！");
                return false;
            }

            if (ClientUtil.ToString(this.txtMaterialMoney.Text) == "")
            {
                MessageBox.Show("材料款不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.txtContractChange.Text) == "")
            {
                MessageBox.Show("合同变跟不能为空！");
                return false;
            }

            if (ClientUtil.ToString(this.cmoQualityObj.SelectedItem) == "")
            {
                MessageBox.Show("质量目标不能为空！");
                return false;
            }
            foreach (DataGridViewRow dr in dgdetails.Rows)
            {

                if (dr.IsNewRow) break;

                if (dr.Cells[ColUnitType.Name].Value == null)
                {
                    MessageBox.Show("单位类型不允许为空！");
                    dgdetails.CurrentCell = dr.Cells[ColUnitType.Name];
                    return false;
                }

                if (dr.Cells[colUnitName.Name].Value == null)
                {
                    MessageBox.Show("单位名称不允许为空！");
                    dgdetails.CurrentCell = dr.Cells[colUnitName.Name];
                    return false;
                }

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
                Immaster.DocState = DocumentState.InExecute;
                //curBillMaster.AuditPerson = ConstObject.LoginPersonInfo;//制单人编号
                //curBillMaster.AuditPersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                //curBillMaster.AuditDate = ConstObject.LoginDate;//制单时间
                //curBillMaster.AuditYear = ConstObject.LoginDate.Year;//制单年
                //curBillMaster.AuditMonth = ConstObject.LoginDate.Month;//制单月
                Immaster = model.ImplementSrv.saveImp(Immaster);
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
        /// 显示数据
        /// </summary>
        public void ModelToView()
        {
            try
            {
                //主表
                this.txtPlanName.Text = ClientUtil.ToString(immaster.ProName);
                this.txtPlanName.Tag = immaster.ProjectId;
                this.txtPersonCharge.Text = ClientUtil.ToString(immaster.DutyOfficer);
                this.dtMadeBillStartDate.Text = immaster.CreateDate.ToShortDateString();
                this.txtTextName.Text = ClientUtil.ToString(immaster.FileName);
                this.txtConstructionStyle.Text = ClientUtil.ToString(immaster.StructType);
                this.txtCoveredArea.Text = ClientUtil.ToString(immaster.CoveredArea);
                this.txtLvConstruction.Text = ClientUtil.ToString(immaster.FloorStructure);
                //相关单位
                this.dgdetails.Rows.Clear();
                foreach (ImplementProjectUnit var in immaster.Details)
                {
                    int i = this.dgdetails.Rows.Add();
                    this.dgdetails[colUnitName.Name, i].Value = var.UnitName;
                    this.dgdetails[ColUnitType.Name, i].Value = GetUnitTypeStr(var.UnitStyle);
                    this.dgdetails.Rows[i].Tag = var;
                }
                //主要管理目标
                txtPeriodTarget.Text = ClientUtil.ToString(Immaster.PeriodTarget);
                this.cmoQualityObj.SelectedItem = ClientUtil.ToString(Immaster.QualityObj);
                txtEnGoal.Text = ClientUtil.ToString(Immaster.EnGoal);
                txtProfessionSafe.Text = ClientUtil.ToString(Immaster.ProfessionSafe);
                txtCostObjective.Text = ClientUtil.ToString(Immaster.CostObjective);
                txtTechTarget.Text = ClientUtil.ToString(Immaster.TechTarget);
                txtObjectiveName.Text = ClientUtil.ToString(Immaster.ObjectiveName);
                txtCiTarget.Text = ClientUtil.ToString(Immaster.CITarget);
                txtCashProject.Text = ClientUtil.ToString(Immaster.CashProject);
                //项目授权
                txtOwnerPayment.Text = ClientUtil.ToString(Immaster.Ownerpayment);
                txtDoteamPayStyle.Text = ClientUtil.ToString(Immaster.DoteamPayStyle);
                txtTeamPayStyle.Text = ClientUtil.ToString(Immaster.TeamPayStyle);
                txtMaterialPaystype.Text = ClientUtil.ToString(Immaster.MaterialPaystype);
                txtEngChoice.Text = ClientUtil.ToString(Immaster.EngChoice);
                txtThingsBuy.Text = ClientUtil.ToString(Immaster.ThingsBuy);
                txtMaterialMoney.Text = ClientUtil.ToString(Immaster.MaterialMoney);
                txtContractChange.Text = ClientUtil.ToString(Immaster.ContractChange);
                FillDoc();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string GetUnitTypeStr(decimal i)
        {
            if (i == 0)
                return "建设单位";
            else if (i == 1)
                return "监理单位";
            else if (i == 2)
                return "设计单位";
            else if (i == 3)
                return "施工单位";
            return "";
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
                //btnStates(1);
                this.immaster = new ImplementationMaintain();
                immaster.CreateDate = ConstObject.LoginDate;//制单时间              
                immaster.DofficerLevel = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;//业务组织编号、         
                immaster.DofficerName = ConstObject.LoginPersonInfo.Name;//负责人名称
                immaster.DocState = DocumentState.Edit;//状态
                //制单日期
                dtMadeBillStartDate.Text = ConstObject.LoginDate.ToShortDateString();
                //责任人
                txtPersonCharge.Tag = ConstObject.LoginPersonInfo;
                txtPersonCharge.Text = ConstObject.LoginPersonInfo.Name;
                //归属项目
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                if (projectInfo != null)
                {
                    txtPlanName.Tag = projectInfo;
                    txtPlanName.Text = projectInfo.Name;
                    immaster.ProjectId = projectInfo.Id;
                    immaster.ProName = projectInfo.Name;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public override bool SaveView()
        {
            if (!ValidView()) return false;
            try
            {
                if (!ViewToModel()) return false;
                bool flag = false;
                if (string.IsNullOrEmpty(immaster.Id))
                {
                    flag = true;
                }
                immaster = model.saveImp(immaster);

                this.ViewCaption = ViewName;
                MessageBox.Show("保存成功！");
                //btnStates(0);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                this.txtConstructionStyle.Focus();
                immaster.DutyOfficer = ClientUtil.ToString(this.txtPersonCharge.Text);
                immaster.CreateDate = ClientUtil.ToDateTime(this.dtMadeBillStartDate.Text);
                immaster.FileName = ClientUtil.ToString(this.txtTextName.Text);
                immaster.StructType = ClientUtil.ToString(this.txtConstructionStyle.Text);
                immaster.CoveredArea = ClientUtil.ToDecimal(this.txtCoveredArea.Text);
                immaster.FloorStructure = ClientUtil.ToString(this.txtLvConstruction.Text);
                foreach (DataGridViewRow var in this.dgdetails.Rows)
                {
                    if (var.IsNewRow) break;
                    ImplementProjectUnit curBillDtl = new ImplementProjectUnit();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as ImplementProjectUnit;
                        if (curBillDtl.Id == null)
                        {
                            immaster.Details.Remove(curBillDtl);
                        }
                    }
                    string str = ClientUtil.ToString(var.Cells[ColUnitType.Name].Value);
                    if (str != "")
                    {
                        if (str.Equals("建设单位"))
                        {
                            curBillDtl.UnitStyle = 0;
                        }
                        if (str.Equals("监理单位"))
                        {
                            curBillDtl.UnitStyle = 1;
                        }
                        if (str.Equals("设计单位"))
                        {
                            curBillDtl.UnitStyle = 2;
                        }
                        if (str.Equals("施工单位"))
                        {
                            curBillDtl.UnitStyle = 3;
                        }
                    }
                    curBillDtl.UnitName = ClientUtil.ToString(var.Cells[colUnitName.Name].Value);
                    immaster.AddDetail(curBillDtl);
                }

                immaster.PeriodTarget = ClientUtil.ToString(this.txtPeriodTarget.Text);
                if (this.cmoQualityObj.SelectedItem != null)
                {
                    immaster.QualityObj = ClientUtil.ToString(this.cmoQualityObj.SelectedItem);
                }
                immaster.EnGoal = ClientUtil.ToString(this.txtEnGoal.Text);
                immaster.ProfessionSafe = ClientUtil.ToString(this.txtProfessionSafe.Text);
                immaster.CostObjective = ClientUtil.ToString(this.txtCostObjective.Text);
                immaster.TechTarget = ClientUtil.ToString(this.txtTechTarget.Text);
                immaster.ObjectiveName = ClientUtil.ToString(this.txtObjectiveName.Text);
                immaster.CITarget = ClientUtil.ToString(this.txtCiTarget.Text);
                immaster.CashProject = ClientUtil.ToString(this.txtCashProject.Text);
                Immaster.Ownerpayment = ClientUtil.ToString(this.txtOwnerPayment.Text);
                Immaster.DoteamPayStyle = ClientUtil.ToString(this.txtDoteamPayStyle.Text);
                Immaster.TeamPayStyle = ClientUtil.ToString(this.txtTeamPayStyle.Text);
                Immaster.MaterialPaystype = ClientUtil.ToString(this.txtMaterialPaystype.Text);
                Immaster.EngChoice = ClientUtil.ToString(this.txtEngChoice.Text);
                Immaster.ThingsBuy = ClientUtil.ToString(this.txtThingsBuy.Text);
                Immaster.MaterialMoney = ClientUtil.ToString(this.txtPeriodTarget.Text);
                Immaster.ContractChange = ClientUtil.ToString(this.txtContractChange.Text);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }
        void txtCoveredArea_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string strDate = txtCoveredArea.Text.ToString();
            validity = CommonMethod.VeryValid(strDate);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                txtCoveredArea.Text = "";
            }
        }
        void txtPeriodTarget_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string strDate = txtPeriodTarget.Text.ToString();
            validity = CommonMethod.VeryValid(strDate);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                txtPeriodTarget.Text = "";
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
            immaster = model.ImplementSrv.GetImpById(immaster.Id);
            if (immaster.DocState == DocumentState.Valid || immaster.DocState == DocumentState.Edit)
            {
                base.ModifyView();
                //btnStates(1);
                ModelToView();
                return true; 
            }
            MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(immaster.DocState) + "】，不能修改！");
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
                immaster = model.ImplementSrv.GetImpById(immaster.Id);
                if (immaster.DocState == DocumentState.Valid || immaster.DocState == DocumentState.Edit)
                {
                    //if (!model.ImplementSrv.DeleteByDao(immaster)) return false;
                    if (!msrv.DeleteReceiptAndDocument(immaster, immaster.Id)) return false;
                    ClearView();
                    MessageBox.Show("删除成功！");
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(immaster.DocState) + "】，不能删除！");
                return false;
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
                        immaster = model.ImplementSrv.GetImpById(immaster.Id);
                        ModelToView();
                        break;
                    default:
                        ClearView();
                        break;
                }
                //btnStates(0);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据撤销错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
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
            //自定义控件清空
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
        //改变皮肤代码skin
        //private void VImplementationMaintain_Load(object sender, EventArgs e)
        //{
        //    this.skinEngine1.SkinFile = AppDomain.CurrentDomain.BaseDirectory + @"/Wave.ssk";
        //}




    }
}

