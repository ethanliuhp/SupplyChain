﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborSporadicManagement.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Windows.Media.Media3D;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using System.Collections;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.PenaltyDeductionManagement.Domain;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using System.IO;
using IRPServiceModel.Domain.Document;
using Application.Business.Erp.SupplyChain.Client.FileUpload;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using IRPServiceModel.Basic;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatformUI;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatMng;
using System.Text.RegularExpressions;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng
{
    public partial class VSubPackageVisa : TMasterDetailView
    {
        private MLaborSporadicMng model = new MLaborSporadicMng();
        private LaborSporadicMaster curBillMaster;
        CurrentProjectInfo projectInfo = null;
        CostAccountSubject subject = new CostAccountSubject();
        /// <summary>
        /// 核算节点集合
        /// </summary>
        private List<TreeNode> ListAccountGWBSNodes = new List<TreeNode>();

        /// <summary>
        /// 当前单据
        /// </summary>
        public LaborSporadicMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }
        string userName = string.Empty;
        string jobId = string.Empty;
        string fileObjectType = string.Empty;
        public VSubPackageVisa()
        {
            InitializeComponent();
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            InitEvent();
            InitDate();
            InitSporadicType();
        }

        private void InitSporadicType()
        {
            //派工类型
            txtSporadicType.Items.AddRange(new object[] { "计时派工", "零星用工" ,"分包签证"});
            txtSporadicType.Text = "分包签证";
        }

        private void InitEvent()
        {
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            //右键删除菜单
            this.cmsDg.ItemClicked += new ToolStripItemClickedEventHandler(cmsDg_ItemClicked);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            txtAccountPerson.LostFocus += new EventHandler(txtAccountPerson_LostFocus);
            btnSelectOwner.Click += new EventHandler(btnSelectOwner_Click);
            this.txtSporadicType.SelectedIndexChanged += new EventHandler(txtSporadicType_SelectedIndexChanged);
            dgDetail.MouseClick += new MouseEventHandler(dgDetail_MouseClick);
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
            this.tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
            this.btnQuantityUnit.Click += new EventHandler(btnQuantityUnit_Click);

            //dgDetail.CellLeave += new DataGridViewCellEventHandler(DgDetail_CellLeave);
        }


        void txtAccountPerson_LostFocus(object sender, EventArgs e)
        {
            if (txtAccountPerson.Text.Trim() == "")
                txtAccountPerson.Tag = null;
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
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", curBillMaster.Id));
            IList listObj = msrv.ObjectQuery(typeof(ProObjectRelaDocument), oq);
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
                IList docList = msrv.ObjectQuery(typeof(DocumentMaster), oq);
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
            if (curBillMaster.Id == null)
            {
                if (MessageBox.Show("当前业务对象还没保存，是否保存！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    if (!ValidView()) return;
                    try
                    {
                        if (!ViewToModel()) return;
                        curBillMaster = model.LaborSporadicSrv.SaveLaborSporadic(curBillMaster);
                        this.ViewCaption = ViewName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(ex));
                    }
                }
            }
            if (curBillMaster.Id != null)
            {
                VDocumentPublicUpload frm = new VDocumentPublicUpload(curBillMaster.Id);
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
            if (dgDocumentMast.Rows.Count == 0) return;
            dgDocumentDetail.Rows.Clear();

            DocumentMaster docMaster = dgDocumentMast.Rows[e.RowIndex].Tag as DocumentMaster;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", docMaster.Id));
            oq.AddFetchMode("TheFileCabinet", NHibernate.FetchMode.Eager);
            IList list = msrv.ObjectQuery(typeof(DocumentDetail), oq);
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
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name.Equals(tabPage2.Name))
            {
                //dgDocumentMast.Rows.Clear();
                //dgDocumentDetail.Rows.Clear();
                if (curBillMaster != null)
                {
                    if (curBillMaster.Id != null)
                    {
                        if (dgDocumentMast.Rows.Count > 0)
                        {
                            dgDocumentMast_CellClick(dgDocumentMast, new DataGridViewCellEventArgs(2, dgDocumentMast.SelectedRows[0].Index));
                        }
                    }
                }
            }
        }

        void dgDetail_MouseClick(object sender, MouseEventArgs e)
        {
            if (!dgDetail.ReadOnly)
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (dgDetail.Enabled)
                    {
                        cmsDg.Items[tsmiDel.Name].Enabled = true;
                        cmsDg.Items[tsmiPaste.Name].Enabled = true;
                        cmsDg.Show(MousePosition.X, MousePosition.Y);
                    }
                }
            }
        }

        void cmsDg_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (this.dgDetail.Rows.Count - 1 == 0)
            {
                MessageBox.Show("明细为空,没有可复制粘贴的信息！");
                return;
            }
            if (e.ClickedItem.Text.Trim() == "复制科目")
            {
                cmsDg.Hide();
                subject = new CostAccountSubject();
                if (dgDetail.CurrentRow.Cells[colLaborSubject.Name].Tag != null)
                {
                    subject = dgDetail.CurrentRow.Cells[colLaborSubject.Name].Tag as CostAccountSubject;
                }
                else
                {
                    MessageBox.Show("未能复制任何信息！");
                }
            }
            if (e.ClickedItem.Text.Trim() == "粘贴科目")
            {
                cmsDg.Hide();
                if (subject.Id != null)
                {
                    dgDetail.CurrentRow.Cells[colLaborSubject.Name].Tag = subject;
                    dgDetail.CurrentRow.Cells[colLaborSubject.Name].Value = subject.Name;
                }
                else
                {
                    MessageBox.Show("没有可粘贴的信息！");
                }
            }

        }

        void txtSporadicType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtSporadicType.SelectedItem.Equals("零星用工") || txtSporadicType.SelectedItem.Equals("分包签证"))
            {
                colRealLaborNum.HeaderText = "实际工程量";
            }
            if (txtSporadicType.SelectedItem.Equals("计时派工"))
            {
                colRealLaborNum.HeaderText = "实际用工量";
            }
        }

        public void InitDate()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
            DateTimePicker dp = new DateTimePicker();
            DataGridViewCalendarColumn dc = new DataGridViewCalendarColumn();
            dp.CustomFormat = "yyyy-MM-dd";
            dp.Format = DateTimePickerFormat.Custom;
            dp.Visible = false;
            dgDetail.Controls.Add(dp);
            dp = new DateTimePicker();
            btnStates(0);
            this.colPriceUnit.Visible = false;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            if (btnSearch.Enabled = true)
            {
                VContractExcuteSelector vmros = new VContractExcuteSelector();
                vmros.ShowDialog();
                IList list = vmros.Result;
                if (list == null || list.Count == 0) return;
                SubContractProject engineerMaster = list[0] as SubContractProject;
                txtSupply.Text = engineerMaster.BearerOrgName;
                txtSupply.Tag = engineerMaster;
            }
        }

        void btnSelectOwner_Click(object sender, EventArgs e)
        {
            VSelectPersonInfo frm = new VSelectPersonInfo(ConstObject.TheOperationOrg);
            frm.ShowDialog();
            if (frm.ListResult.Count > 0)
            {
                PersonInfo p = frm.ListResult[0];
                txtAccountPerson.Text = p.Name;
                txtAccountPerson.Tag = p;
            }
        }

        void btnQuantityUnit_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in dgDetail.Rows)
            {
                if (var.IsNewRow) break;
                if (dgDetail[colQuantityUnit.Name, 0].Tag != null)
                {
                    var.Cells[colQuantityUnit.Name].Tag = dgDetail[colQuantityUnit.Name, 0].Tag as StandardUnit;
                    var.Cells[colQuantityUnit.Name].Value = dgDetail[colQuantityUnit.Name, 0].Value;
                }
            }
        }

        #region 固定代码
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
                    curBillMaster = model.LaborSporadicSrv.GetLaborSporadicById(Id);
                    ModelToView();
                    RefreshState(MainViewState.Browser);

                    //判断是否为制单人
                    PersonInfo pi = this.txtCreatePerson.Tag as PersonInfo;
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
                    this.dgDetail.EditMode = DataGridViewEditMode.EditOnEnter;
                    this.btnSearch.Enabled = true;
                    this.txtSporadicType.Enabled = true;
                    cmsDg.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
                    this.btnSearch.Enabled = false;
                    this.txtSporadicType.Enabled = false;
                    cmsDg.Enabled = false;
                    break;
                default:
                    break;
            }
        }
        #endregion

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
            }
            else
            {
                ObjectLock.Lock(pnlFloor, true);
            }

            //永久锁定
            object[] os = new object[] { txtSupply, txtCode, txtCreatePerson, txtPredictProjectTast, txtRealProjectTast, txtProject };
            ObjectLock.Lock(os);
            string[] lockCols = new string[] { colProjectTastDetail.Name, colProjectTastType.Name, colMaterialCode.Name, colQuantityUnit.Name, colLaborSubject.Name, colInsteadTeam.Name };
            dgDetail.SetColumnsReadOnly(lockCols);
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
        /// 新建
        /// </summary>
        /// <returns></returns>
        public override bool NewView()
        {
            try
            {
                btnStates(1);
                base.NewView();
                ClearView();
                this.curBillMaster = new LaborSporadicMaster();
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.CreateDate = ConstObject.LoginDate;
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                curBillMaster.HandOrgLevel = ConstObject.TheOperationOrg.Level;
                curBillMaster.DocState = DocumentState.Edit;
                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //责任人
                txtAccountPerson.Tag = ConstObject.LoginPersonInfo;
                txtAccountPerson.Text = ConstObject.LoginPersonInfo.Name;
                //归属项目
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                if (projectInfo != null)
                {
                    txtProject.Tag = projectInfo;
                    txtProject.Text = projectInfo.Name;
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
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
        /// 修改
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
            if (curBillMaster.DocState == DocumentState.Edit)
            {
                base.ModifyView();
                curBillMaster = model.LaborSporadicSrv.GetLaborSporadicById(curBillMaster.Id);
                ModelToView();
                btnStates(1);
                return true;
            }
            string message = "此单状态为非编辑状态，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(curBillMaster.DocState));
            MessageBox.Show(message);
            return false;
        }

        //[optrType=1 保存][optrType=2 提交]
        private bool SaveOrSubmitBill(int optrType)
        {
            if (!ViewToModel())
                return false;
            LogData log = new LogData();
            if (string.IsNullOrEmpty(curBillMaster.Id))
            {
                if (optrType == 2)
                {
                    log.OperType = "新增提交";
                }
                else
                {
                    log.OperType = "新增保存";
                }
            }
            else
            {
                if (optrType == 2)
                {
                    log.OperType = "修改提交";
                }
                else
                {
                    log.OperType = "修改保存";
                }
            }
            if (optrType == 2)
            {//不需要走审批平台和分包签证单审核  直接进行结算 所以分包签证提交后直接将状态改为5 提交状态
                curBillMaster.DocState = DocumentState.InExecute; //DocumentState.InAudit;
                curBillMaster.AuditPerson = ConstObject.LoginPersonInfo;//制单人编号
                curBillMaster.AuditPersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                curBillMaster.AuditDate = ConstObject.LoginDate;//制单时间
                curBillMaster.AuditYear = ConstObject.LoginDate.Year;//制单年
                curBillMaster.AuditMonth = ConstObject.LoginDate.Month;//制单月
                curBillMaster.SubmitDate = DateTime.Now;
            }
            curBillMaster = model.LaborSporadicSrv.SaveLaborSporadic1(curBillMaster);
            this.txtCode.Text = curBillMaster.Code;

            //#region 短信
            if (optrType == 2)
            {
                MAppPlatMng appModel = new MAppPlatMng();
                appModel.SendMessage(curBillMaster.Id, "LaborSporadicMaster");
            }

            log.BillId = curBillMaster.Id;
            log.BillType = "分包签证单维护";
            log.Code = curBillMaster.Code;
            log.Descript = "";
            log.OperPerson = ConstObject.LoginPersonInfo.Name;
            log.ProjectName = curBillMaster.ProjectName;
            StaticMethod.InsertLogData(log);
            this.ViewCaption = ViewName + "-" + txtCode.Text;
            return true;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public override bool SaveView()
        {
            try
            {
                if (curBillMaster.DocState == DocumentState.Edit)
                {
                    if (SaveOrSubmitBill(1) == false) return false;
                    MessageBox.Show("保存成功！");
                    btnStates(0);
                    return true;
                }
                else
                {
                    MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能保存！");
                    return false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
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
                if (curBillMaster.DocState == DocumentState.Edit)
                {
                    if (SaveOrSubmitBill(2) == false) return false;
                    MessageBox.Show("提交成功！");
                    //#region 短信
                    //MAppPlatform AppModel = new MAppPlatform();
                    //DataSet ds = AppModel.Service.GetSubmitBillPerson(curBillMaster.Id, "LaborSporadicMaster");
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
                    btnStates(0);
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能提交！");
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
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
                curBillMaster = model.LaborSporadicSrv.GetLaborSporadicById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Edit)
                {
                    //if (!model.LaborSporadicSrv.DeleteByDao(curBillMaster)) return false;
                    if (!msrv.DeleteReceiptAndDocument(curBillMaster, curBillMaster.Id)) return false;
                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "零星用工单维护";
                    log.Code = curBillMaster.Code;
                    log.OperType = "删除";
                    log.Descript = "";
                    log.OperPerson = ConstObject.LoginPersonInfo.Name;
                    log.ProjectName = curBillMaster.ProjectName;
                    StaticMethod.InsertLogData(log);
                    ClearView();
                    MessageBox.Show("删除成功！");
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能删除！");
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
                        curBillMaster = model.LaborSporadicSrv.GetLaborSporadicById(curBillMaster.Id);
                        ModelToView();
                        break;
                    default:
                        ClearView();
                        break;
                }
                btnStates(0);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据撤销错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public override void RefreshView()
        {
            try
            {
                //重新获得当前对象的值
                curBillMaster = model.LaborSporadicSrv.GetLaborSporadicById(curBillMaster.Id);
                //给界面赋值
                ModelToView();
            }
            catch (Exception e)
            {
                MessageBox.Show("数据刷新错误：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// 保存数据前校验数据
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            if (txtSupply.Text == "" || txtSupply.Tag == null)
            {
                MessageBox.Show("承担队伍不能为空！");
                return false;
            }
            if (this.dgDetail.Rows.Count - 1 == 0)
            {
                MessageBox.Show("明细不能为空！");
                return false;
            }
            dgDetail.EndEdit();
            dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));
            //明细表数据校验
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                //if (ClientUtil.ToString(dr.Cells[colProjectTastType.Name].Value) != "" && dr.Cells[colRealLaborNum.Name].Value == null)
                //{
                //    MessageBox.Show("实际用工量不能为空！");
                //    return false;
                //}
                //最后一行不进行校验
                if (dr.IsNewRow) break;
                if (dr.Cells[colProjectTastType.Name].Value == null)
                {
                    MessageBox.Show("工程任务类型不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colProjectTastType.Name];
                    return false;
                }
                if (dr.Cells[colLaborSubject.Name].Value == null)
                {
                    MessageBox.Show("用工科目不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colLaborSubject.Name];
                    return false;
                }
                //if (dr.Cells[colRealLaborNum.Name].Value == null)
                //{
                //    MessageBox.Show("实际用工量不能为空！");
                //    dgDetail.CurrentCell = dr.Cells[colRealLaborNum.Name];
                //    return false;
                //}
                if (dr.Cells[colPrice.Name].Value == null)
                {
                    MessageBox.Show("核算单价不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colPrice.Name];
                    return false;
                }
                //if (dr.Cells[colPriceUnit.Name].Value == null)
                //{
                //    MessageBox.Show("价格单位不能为空！");
                //    dgDetail.CurrentCell = dr.Cells[colPriceUnit.Name];
                //    return false;
                //}
                if (dr.Cells[colAmount.Name].Value == null)
                {
                    MessageBox.Show("核算合价不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colAmount.Name];
                    return false;
                }
                if (dr.Cells[this.colAccountQuantity.Name].Value == null)
                {
                    MessageBox.Show("核算量不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colAmount.Name];
                    return false;
                }
                if (dr.Cells[colInsteadTeam.Name].Value != null && (dr.Cells[colInsteadTeam.Name].Tag as SubContractProject).Id == (txtSupply.Tag as SubContractProject).Id)
                {
                    MessageBox.Show("承担队伍应与被代工队伍不能相同！");
                    dgDetail.CurrentCell = dr.Cells[colInsteadTeam.Name];
                    return false;
                }

            }
            return true;
        }
        StandardUnit oPriceUnit=null;
        public StandardUnit PriceUnit{
            get
            {
                if (oPriceUnit == null)
                {
                    string strPriceUnit = "元";
                    StandardUnit PriceUnit = null;
                    ObjectQuery oq1 = new ObjectQuery();
                    oq1.AddCriterion(Expression.Eq("Name", strPriceUnit));
                    IList list = model.LaborSporadicSrv.GetDomainByCondition(typeof(StandardUnit), oq1);
                    if (list != null && list.Count > 0)
                    {
                        oPriceUnit = list[0] as StandardUnit;
                    }
                }
                return oPriceUnit;
            }
        }
        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                this.txtCode.Focus();
                curBillMaster.BearTeamName = ClientUtil.ToString(txtSupply.Text);//承担队伍
                curBillMaster.BearTeam = txtSupply.Tag as SubContractProject;
               // curBillMaster.HandlePersonName = "分包签证单";      // 使用工长的栏位存放该单据的类型
                curBillMaster.LaborState = this.txtSporadicType.Text;
                curBillMaster.CreateDate = dtpBusinessDate.Value.Date;
                curBillMaster.Details.Clear();
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    LaborSporadicDetail curBillDtl = new LaborSporadicDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as LaborSporadicDetail;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    //if (var.Cells[colRealLaborNum.Name].Value != null && ClientUtil.ToString(var.Cells[colRealLaborNum.Name].Value) != "")
                    if (var.Cells[colAccountQuantity.Name].Value != null && ClientUtil.ToString(var.Cells[colAccountQuantity.Name].Value) != "")//
                    {
                        curBillDtl.InsteadTeam =  var.Cells[colInsteadTeam.Name].Tag as SubContractProject;//被代工队伍
                        curBillDtl.InsteadTeamName =  ClientUtil.ToString(var.Cells[colInsteadTeam.Name].Value);//被代工队伍 
                    }
                    curBillDtl.RealLaborNum = ClientUtil.ToDecimal(var.Cells[colAccountQuantity.Name].Value);// ClientUtil.ToDecimal(var.Cells[colRealLaborNum.Name].Value);//实际用工量
                    curBillDtl.AccountLaborNum = curBillDtl.RealLaborNum;
                    curBillDtl.EndDate = Convert.ToDateTime(var.Cells[colEndDate.Name].Value);//结束时间
                    curBillDtl.CompleteDate = Convert.ToDateTime(var.Cells[colCompleteDate.Name].Value);//完成时间
                    curBillDtl.LaborSubject = var.Cells[colLaborSubject.Name].Tag as CostAccountSubject;//用工科目
                    curBillDtl.LaborSubjectSysCode = (var.Cells[colLaborSubject.Name].Tag as CostAccountSubject).SysCode;//用工科目层次码
                    curBillDtl.LaborSubjectName = ClientUtil.ToString(var.Cells[colLaborSubject.Name].Value);//用工科目
                    curBillDtl.ProjectTastDetailName = ClientUtil.ToString(var.Cells[colProjectTastDetail.Name].Value);//工程任务明细名称
                    curBillDtl.ProjectTastName = ClientUtil.ToString(var.Cells[colProjectTastType.Name].Value);//工程任务名称

                    /*
                        新增加的三列
                    */
                    curBillDtl.AccountPrice = ClientUtil.ToDecimal(var.Cells[colPrice.Name].Value);
                    curBillDtl.PriceUnit = var.Cells[colPriceUnit.Name].Tag as StandardUnit;
                    if (curBillDtl.PriceUnit == null)
                    {
                        curBillDtl.PriceUnit = PriceUnit;
                        curBillDtl.PriceUnitName = curBillDtl.PriceUnit.Name;
                    }
                   
                    curBillDtl.AccountLaborNum = ClientUtil.ToDecimal(var.Cells[colAccountQuantity.Name].Value);
                    curBillDtl.AccountSumMoney = ClientUtil.ToDecimal(var.Cells[colAmount.Name].Value);

                    

                    curBillDtl.QuantityUnit = var.Cells[colQuantityUnit.Name].Tag as StandardUnit;//数量单位
                    curBillDtl.QuantityUnitName = ClientUtil.ToString(var.Cells[colQuantityUnit.Name].Value);//数量单位名称
                    curBillDtl.RealLaborNum = ClientUtil.ToDecimal(var.Cells[colAccountQuantity.Name].Value);// ClientUtil.ToDecimal(var.Cells[colRealLaborNum.Name].Value);//实际用工量
                    curBillDtl.ResourceType = var.Cells[colMaterialCode.Name].Tag as Application.Resource.MaterialResource.Domain.Material;
                    curBillDtl.ResourceTypeName = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);//资源类型名称
                    curBillDtl.ResourceTypeSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);//资源规格
                    curBillDtl.ResourceTypeStuff = ClientUtil.ToString(var.Cells[colMaterialStuff.Name].Value);//资源材质
                    curBillDtl.ResourceSysCode = ClientUtil.ToString(var.Cells[colMaterialSysCode.Name].Value);//资源层次码
                    curBillDtl.StartDate = Convert.ToDateTime(var.Cells[colStartDate.Name].Value);//起始时间
                    curBillDtl.ProjectTastDetail = var.Cells[colProjectTastDetail.Name].Tag as GWBSDetail;//工程任务明细
                    curBillDtl.ProjectTast = var.Cells[colProjectTastType.Name].Tag as GWBSTree;//工程任务
                    curBillDtl.ProjectTaskSyscode = (var.Cells[colProjectTastType.Name].Tag as GWBSTree).SysCode;//工程任务层次码
                    curBillDtl.LaborDescript = ClientUtil.ToString(var.Cells[colLaborDescript.Name].Value);//备注
                    curBillDtl.DetailNumber = var.Index + 1;
                    curBillMaster.AddDetail(curBillDtl);
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 在光标跳自动转到下一列时，首先执行校验，确定是否跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
        }


        /// <summary>
        /// 键盘回车查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //if (base.ViewState == MainViewState.Browser || base.ViewState == MainViewState.Initialize) return;

            //if (e.KeyValue == 13)
            //{
            //    CommonMaterial materialSelector = new CommonMaterial();

            //    TextBox textBox = sender as TextBox;
            //    if (textBox.Text != null && !textBox.Text.Equals(""))
            //    {
            //        materialSelector.OpenSelect(textBox.Text);
            //    }
            //    else
            //    {
            //        materialSelector.OpenSelect();
            //    }
            //    IList list = materialSelector.Result;

            //    if (list != null && list.Count > 0)
            //    {
            //        Application.Resource.MaterialResource.Domain.Material selectedMaterial = list[0] as Application.Resource.MaterialResource.Domain.Material;
            //        //this.dgDetail.CurrentRow.Cells[colMaterialCode.Name].Tag = selectedMaterial;
            //        this.dgDetail.CurrentCell.Value = selectedMaterial.Code;
            //        //this.dgDetail.CurrentRow.Cells[colMaterialName.Name].Value = selectedMaterial.Name;
            //        //this.dgDetail.CurrentRow.Cells[colMaterialSpec.Name].Value = selectedMaterial.Specification;

            //        //动态分类复合单位                    
            //        //DataGridViewComboBoxCell cbo = this.dgDetail.CurrentRow.Cells[colUnit.Name] as DataGridViewComboBoxCell;
            //        cbo.Items.Clear();

            //        StandardUnit first = null;
            //        foreach (StandardUnit cu in selectedMaterial.GetMaterialAllUnit())
            //        {
            //            cbo.Items.Add(cu.Name);
            //        }
            //        first = selectedMaterial.BasicUnit;
            //        this.dgDetail.CurrentRow.Cells[colUnit.Name].Tag = first;
            //        cbo.Value = first.Name;
            //        this.dgDetail.RefreshEdit();
            //    }
            //}
        }


        //显示数据
        private bool ModelToView()
        {
            try
            {
                this.txtCode.Text = curBillMaster.Code;
                txtAccountPerson.Tag = curBillMaster.HandlePerson;
                txtAccountPerson.Text = curBillMaster.HandlePersonName;
                //txtCreateDate.Text = curBillMaster.CreateDate.ToShortDateString();
                dtpBusinessDate.Value = curBillMaster.CreateDate;
                this.txtCreatePerson.Tag = curBillMaster.CreatePerson;
                txtCreatePerson.Text = curBillMaster.CreatePersonName;
                this.txtCreatePerson.Text = curBillMaster.CreatePersonName;
                this.txtProject.Text = curBillMaster.ProjectName.ToString();
                this.txtSupply.Text = ClientUtil.ToString(curBillMaster.BearTeamName);//承担队伍
                txtSupply.Tag = curBillMaster.BearTeam;
                this.txtPredictProjectTast.Text = ClientUtil.ToString(curBillMaster.SumPredictProjectNum);
                this.txtRealProjectTast.Text = ClientUtil.ToString(curBillMaster.SumRealProjectNum);
                this.txtSporadicType.Text = ClientUtil.ToString(curBillMaster.LaborState);
                this.dgDetail.Rows.Clear();
                foreach (LaborSporadicDetail var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();
                    if (var.InsteadTeam != null)
                    {
                        this.dgDetail[colInsteadTeam.Name, i].Tag = var.InsteadTeam;
                        this.dgDetail[colInsteadTeam.Name, i].Value = var.InsteadTeamName;
                    }
                    this.dgDetail[colCompleteDate.Name, i].Value = var.CompleteDate;
                    this.dgDetail[colEndDate.Name, i].Value = var.EndDate;
                    this.dgDetail[colLaborSubject.Name, i].Value = var.LaborSubjectName;
                    this.dgDetail[colLaborSubject.Name, i].Tag = var.LaborSubject;
                    this.dgDetail[colMaterialStuff.Name, i].Value = var.ResourceTypeStuff;
                    this.dgDetail[colMaterialSpec.Name, i].Value = var.ResourceTypeSpec;
                    this.dgDetail[colMaterialCode.Name, i].Value = var.ResourceTypeName;
                    this.dgDetail[colMaterialCode.Name, i].Tag = var.ResourceType;
                    this.dgDetail[colMaterialSysCode.Name, i].Value = var.ResourceSysCode;//层次码
                    this.dgDetail[colProjectTastDetail.Name, i].Value = var.ProjectTastDetailName;
                    this.dgDetail[colProjectTastDetail.Name, i].Tag = var.ProjectTastDetail;
                    this.dgDetail[colProjectTastType.Name, i].Value = var.ProjectTastName;
                    this.dgDetail[colProjectTastType.Name, i].Tag = var.ProjectTast;
                    this.dgDetail[colQuantityUnit.Name, i].Value = var.QuantityUnitName;
                    this.dgDetail[colQuantityUnit.Name, i].Tag = var.QuantityUnit;
                    this.dgDetail[colRealLaborNum.Name, i].Value = var.RealLaborNum;
                    this.dgDetail[colStartDate.Name, i].Value = var.StartDate;
                    this.dgDetail[colLaborDescript.Name, i].Value = var.LaborDescript;
                    
                    /*
                        新增的三列
                    */
                    this.dgDetail[colAccountQuantity.Name, i].Value = var.AccountLaborNum;
                    this.dgDetail[colPrice.Name, i].Value = var.AccountPrice;
                    this.dgDetail[colPriceUnit.Name, i].Tag = var.PriceUnit;
                    this.dgDetail[colPriceUnit.Name, i].Value = var.PriceUnitName;
                    this.dgDetail[colAmount.Name, i].Value = var.AccountSumMoney;


                    this.dgDetail.Rows[i].Tag = var;
                }
                FillDoc();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            bool flag = true;
            decimal sumRealQuan = 0;
            decimal sumPredictQuan = 0;
            for (int i = 0; i <= dgDetail.RowCount - 1; i++)
            {
                if (colName == colRealLaborNum.Name)//实际工程量
                {
                    string quantity = ClientUtil.ToString(dgDetail.Rows[i].Cells[colRealLaborNum.Name].Value);
                    if (quantity == null)
                    {
                        quantity = "0";
                        return;
                    }
                    validity = CommonMethod.VeryValid(quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("实际用工量为数字！");
                        this.dgDetail.Rows[i].Cells[colRealLaborNum.Name].Value = "";
                        flag = false;
                    }
                    sumRealQuan += ClientUtil.ToDecimal(quantity);
                    txtRealProjectTast.Text = ClientUtil.ToString(sumRealQuan);
                }
            }

            // 计算合价
            var table = (CustomDataGridView)sender;
            var cell = table.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (cell.Value + "" == "") return;
            string columnName = table.Columns[e.ColumnIndex].Name;
            if (columnName == colPrice.Name || columnName == colRealLaborNum.Name || columnName == colAccountQuantity.Name)
            {
                decimal temp = 0;
                var isNum = decimal.TryParse(cell.Value.ToString(), out temp);
                if (!isNum)
                {
                    MessageBox.Show("请输入一个数字...");
                    cell.Selected = true;
                    cell.Value = null;
                }
                else
                {
                    CalcAmount(table, e.RowIndex);
                }
            }

        }
        /// <summary>
        /// 计算合价
        /// </summary>
        /// <param name="table"></param>
        /// <param name="rowIndex"></param>
        private void CalcAmount(DataGridView table, int rowIndex)
        {
            decimal price = 0;
            decimal num = 0;
            decimal.TryParse(table.Rows[rowIndex].Cells[colPrice.Name].Value + "", out price);
            decimal.TryParse(table.Rows[rowIndex].Cells[colAccountQuantity.Name].Value + "", out num);
            table.Rows[rowIndex].Cells[colAmount.Name].Value = price * num;
        }

        /// <summary>
        /// 工程任务类型列，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
                if (this.dgDetail.EditMode == DataGridViewEditMode.EditOnEnter)
                {
                    if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colProjectTastDetail.Name))
                    {
                        string strType = ClientUtil.ToString(txtSporadicType.SelectedItem);
                        if (strType != "")
                        {
                            DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                            VSelectGWBSDetail frm = new VSelectGWBSDetail(new MGWBSTree());
                            frm.ShowDialog();

                            if (frm.IsOk)
                            {
                                List<GWBSDetail> list = frm.SelectGWBSDetail;
                                string strUnit = "工日";
                                Application.Resource.MaterialResource.Domain.StandardUnit Unit = null;
                                ObjectQuery oq = new ObjectQuery();
                                oq.AddCriterion(Expression.Eq("Name", strUnit));
                                IList lists = model.LaborSporadicSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq);
                                if (lists != null && lists.Count > 0)
                                {
                                    Unit = lists[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                                }
                                string strName = "劳务分包资源";
                                Application.Resource.MaterialResource.Domain.Material theMaterial = null;
                                ObjectQuery oqt = new ObjectQuery();
                                oqt.AddCriterion(Expression.Eq("Name", strName));
                                IList lst = model.LaborSporadicSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.Material), oqt);
                                if (lst != null && lst.Count > 0)
                                {
                                    theMaterial = lst[0] as Application.Resource.MaterialResource.Domain.Material;
                                }
                                foreach (GWBSDetail gwbsTree in list)
                                {
                                    if (dgDetail.CurrentRow.Cells[colLaborDescript.Name].Value != null || dgDetail.CurrentRow.Cells[colLaborSubject.Name].Value != null || dgDetail.CurrentRow.Cells[colMaterialCode.Name].Value != null || dgDetail.CurrentRow.Cells[colProjectTastDetail.Name].Value != null || dgDetail.CurrentRow.Cells[colProjectTastType.Name].Value != null || dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Value != null || dgDetail.CurrentRow.Cells[colRealLaborNum.Name].Value != null)
                                    {
                                        this.dgDetail.CurrentRow.Cells[colProjectTastDetail.Name].Value = gwbsTree.Name;
                                        this.dgDetail.CurrentRow.Cells[colProjectTastDetail.Name].Tag = gwbsTree;
                                        this.dgDetail.CurrentRow.Cells[colProjectTastType.Name].Value = gwbsTree.TheGWBS.Name;
                                        this.dgDetail.CurrentRow.Cells[colProjectTastType.Name].Tag = gwbsTree.TheGWBS;
                                        this.dgDetail.CurrentRow.Cells[colCompleteDate.Name].Value = DateTime.Now;
                                        this.dgDetail.CurrentRow.Cells[colEndDate.Name].Value = DateTime.Now;
                                        this.dgDetail.CurrentRow.Cells[colStartDate.Name].Value = DateTime.Now;
                                        if (txtSporadicType.SelectedItem.Equals("计时派工"))
                                        {
                                            this.dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Value = strUnit;
                                            this.dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Tag = Unit;
                                        }
                                        else
                                        {
                                            this.dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Value = gwbsTree.WorkAmountUnitName;
                                            this.dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Tag = gwbsTree.WorkAmountUnitGUID;
                                        }
                                        this.dgDetail.CurrentRow.Cells[colMaterialCode.Name].Value = "劳务分包资源";
                                        if (theMaterial != null)
                                        {
                                            this.dgDetail.CurrentRow.Cells[colMaterialCode.Name].Tag = theMaterial;
                                            this.dgDetail.CurrentRow.Cells[colMaterialSysCode.Name].Value = theMaterial.TheSyscode;
                                            this.dgDetail.CurrentRow.Cells[colMaterialStuff.Name].Value = theMaterial.Stuff;
                                            this.dgDetail.CurrentRow.Cells[colMaterialSpec.Name].Value = theMaterial.Specification;
                                        }
                                    }
                                    else
                                    {
                                        int i = dgDetail.Rows.Add();
                                        this.dgDetail[colProjectTastDetail.Name, i].Value = gwbsTree.Name;
                                        this.dgDetail[colProjectTastDetail.Name, i].Tag = gwbsTree;
                                        this.dgDetail[colProjectTastType.Name, i].Value = gwbsTree.TheGWBS.Name;
                                        this.dgDetail[colProjectTastType.Name, i].Tag = gwbsTree.TheGWBS;
                                        this.dgDetail[colCompleteDate.Name, i].Value = DateTime.Now;
                                        this.dgDetail[colEndDate.Name, i].Value = DateTime.Now;
                                        this.dgDetail[colStartDate.Name, i].Value = DateTime.Now;
                                        if (txtSporadicType.SelectedItem.Equals("计时派工"))
                                        {
                                            this.dgDetail[colQuantityUnit.Name, i].Value = strUnit;
                                            this.dgDetail[colQuantityUnit.Name, i].Tag = Unit;
                                        }
                                        else
                                        {
                                            this.dgDetail[colQuantityUnit.Name, i].Value = gwbsTree.WorkAmountUnitName;
                                            this.dgDetail[colQuantityUnit.Name, i].Tag = gwbsTree.WorkAmountUnitGUID;
                                        }
                                        this.dgDetail[colMaterialCode.Name, i].Value = "劳务分包资源";
                                        if (theMaterial != null)
                                        {
                                            this.dgDetail[colMaterialCode.Name, i].Tag = theMaterial;
                                            this.dgDetail[colMaterialSysCode.Name, i].Value = theMaterial.TheSyscode;
                                            this.dgDetail[colMaterialStuff.Name, i].Value = theMaterial.Stuff;
                                            this.dgDetail[colMaterialSpec.Name, i].Value = theMaterial.Specification;
                                        }
                                        i++;
                                    }
                                }
                                this.txtCode.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("请先选择派工类型！");
                            return;
                        }
                    }
                    if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colQuantityUnit.Name) || this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colPriceUnit.Name))
                    {
                        string colName = this.dgDetail.Columns[e.ColumnIndex].Name;
                        //双击数量单位
                        StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                        if (su != null)
                        {
                            if (dgDetail.CurrentRow.Cells[colLaborDescript.Name].Value != null || dgDetail.CurrentRow.Cells[colLaborSubject.Name].Value != null || dgDetail.CurrentRow.Cells[colMaterialCode.Name].Value != null || dgDetail.CurrentRow.Cells[colProjectTastDetail.Name].Value != null || dgDetail.CurrentRow.Cells[colProjectTastType.Name].Value != null || dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Value != null || dgDetail.CurrentRow.Cells[colRealLaborNum.Name].Value != null)
                            {
                                this.dgDetail.CurrentRow.Cells[colName].Tag = su;
                                this.dgDetail.CurrentRow.Cells[colName].Value = su.Name;
                            }
                            else
                            {
                                int i = dgDetail.Rows.Add();
                                this.dgDetail[colName, i].Value = su.Name;
                                this.dgDetail[colName, i].Tag = su;
                            }
                            this.txtCode.Focus();
                        }
                    }

                    if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colLaborSubject.Name))
                    {
                        //双击用工科目
                        VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
                        frm.IsSubBalanceFlag = true;
                        frm.ShowDialog();
                        CostAccountSubject cost = frm.SelectAccountSubject;
                        if (cost != null)
                        {
                            if (dgDetail.CurrentRow.Cells[colLaborDescript.Name].Value != null || dgDetail.CurrentRow.Cells[colLaborSubject.Name].Value != null || dgDetail.CurrentRow.Cells[colMaterialCode.Name].Value != null || dgDetail.CurrentRow.Cells[colProjectTastDetail.Name].Value != null || dgDetail.CurrentRow.Cells[colProjectTastType.Name].Value != null || dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Value != null || dgDetail.CurrentRow.Cells[colRealLaborNum.Name].Value != null)
                            {
                                this.dgDetail.CurrentRow.Cells[colLaborSubject.Name].Tag = cost;
                                this.dgDetail.CurrentRow.Cells[colLaborSubject.Name].Value = cost.Name;
                            }
                            else
                            {
                                int i = dgDetail.Rows.Add();
                                this.dgDetail[colLaborSubject.Name, i].Value = cost.Name;
                                this.dgDetail[colLaborSubject.Name, i].Tag = cost;
                            }
                        }
                        this.txtCode.Focus();
                    }
                    if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colInsteadTeam.Name))
                    {
                        VContractExcuteSelector vmros = new VContractExcuteSelector();
                        vmros.ShowDialog();
                        IList list = vmros.Result;
                        if (list == null || list.Count == 0) return;
                        SubContractProject engineerMaster = list[0] as SubContractProject;
                        if (engineerMaster != null)
                        {
                            if (dgDetail.CurrentRow.Cells[colInsteadTeam.Name].Value != null || dgDetail.CurrentRow.Cells[colLaborDescript.Name].Value != null || dgDetail.CurrentRow.Cells[colLaborSubject.Name].Value != null || dgDetail.CurrentRow.Cells[colMaterialCode.Name].Value != null || dgDetail.CurrentRow.Cells[colProjectTastDetail.Name].Value != null || dgDetail.CurrentRow.Cells[colProjectTastType.Name].Value != null || dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Value != null || dgDetail.CurrentRow.Cells[colRealLaborNum.Name].Value != null)
                            {
                                this.dgDetail.CurrentRow.Cells[colInsteadTeam.Name].Tag = engineerMaster;
                                this.dgDetail.CurrentRow.Cells[colInsteadTeam.Name].Value = engineerMaster.BearerOrgName;
                            }
                            else
                            {
                                int i = dgDetail.Rows.Add();
                                this.dgDetail[colInsteadTeam.Name, i].Value = engineerMaster.BearerOrgName;
                                this.dgDetail[colInsteadTeam.Name, i].Tag = engineerMaster;
                            }
                        }
                        this.txtCode.Focus();
                    }
                }
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        private void GetAccountGWBSNodes(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                GWBSTree accountGWBS = tn.Tag as GWBSTree;
                if (accountGWBS.IsAccountNode)//如果该节点是核算节点
                {
                    ListAccountGWBSNodes.Add(tn);
                }
                else
                {
                    GetAccountGWBSNodes(tn);
                }
            }
        }

     

        ///// <summary>
        ///// 打印预览
        ///// </summary>
        ///// <returns></returns>
        //public override bool Preview()
        //{
        //    if (LoadTempleteFile(@"料具租赁管理\料具租赁合同打印.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.PrintPreview(true,true,true,0,0,0,0,0);
        //    return true;
        //}

        //public override bool Print()
        //{
        //    if (LoadTempleteFile(@"料具租赁管理\料具租赁合同打印.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.Print();
        //    return true;
        //}

        //public override bool Export()
        //{
        //    if (LoadTempleteFile(@"料具租赁管理\料具租赁合同打印.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.ExportToExcel("料具租赁合同【" + curBillMaster.Code + "】", false, false, true);
        //    return true;
        //}

        //private bool LoadTempleteFile(string modelName)
        //{
        //    ExploreFile eFile = new ExploreFile();
        //    string path = eFile.Path;
        //    if (eFile.IfExistFileInServer(modelName))
        //    {
        //        eFile.CreateTempleteFileFromServer(modelName);
        //        //载入格式和数据
        //        flexGrid1.OpenFile(path + "\\" + modelName);//载入格式
        //    }
        //    else
        //    {
        //        MessageBox.Show("未找到模板格式文件【" + modelName + "】");
        //        return false;
        //    }
        //    return true;
        //}

        //private void FillFlex(MaterialRentalOrderMaster billMaster)
        //{
        //    int detailStartRowNumber = 7;//7为模板中的行号
        //    int detailCount = billMaster.Details.Count;

        //    //插入明细行
        //    flexGrid1.InsertRow(detailStartRowNumber, detailCount);

        //    //设置单元格的边框，对齐方式
        //    FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
        //    range.Alignment = FlexCell.AlignmentEnum.RightCenter;
        //    range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
        //    range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
        //    range.Mask = FlexCell.MaskEnum.Digital;

        //    //主表数据

        //    flexGrid1.Cell(2, 1).Text = "使用单位：";
        //    flexGrid1.Cell(2, 4).Text = "登记时间：" + DateTime.Now.ToShortDateString();
        //    flexGrid1.Cell(2, 7).Text = "制单编号：" + billMaster.Code;
        //    flexGrid1.Cell(4, 2).Text = billMaster.OriginalContractNo;
        //    flexGrid1.Cell(4, 5).Text = "";//合同名称
        //    flexGrid1.Cell(4, 7).Text = "";//材料分类
        //    flexGrid1.Cell(5, 2).Text = "";//租赁单位
        //    flexGrid1.Cell(5, 2).WrapText = true;
        //    flexGrid1.Cell(5, 5).Text = "";//承租单位
        //    flexGrid1.Row(5).AutoFit();
        //    flexGrid1.Cell(5, 7).Text = billMaster.RealOperationDate.ToShortDateString();//签订日期

        //    FlexCell.PageSetup pageSetup = flexGrid1.PageSetup;
        //    pageSetup.LeftFooter = "   制单人：" + billMaster.CreatePersonName;
        //    pageSetup.RightFooter = "第 &P 页/共 &N 页      ";

        //    System.Drawing.Printing.PaperSize paperSize = new System.Drawing.Printing.PaperSize("自定义纸张", 910, 470);
        //    pageSetup.PaperSize = paperSize;

        //    //填写明细数据
        //    for (int i = 0; i < detailCount; i++)
        //    {
        //        MaterialRentalOrderDetail detail = (MaterialRentalOrderDetail)billMaster.Details.ElementAt(i);
        //        //物资名称
        //        flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialResource.Name;
        //        flexGrid1.Cell(detailStartRowNumber + i, 1).Alignment = FlexCell.AlignmentEnum.LeftCenter;

        //        //规格型号
        //        flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialResource.Specification;
        //        flexGrid1.Cell(detailStartRowNumber + i, 2).Alignment = FlexCell.AlignmentEnum.LeftCenter;

        //        //结算规则
        //        flexGrid1.Cell(detailStartRowNumber + i, 3).Text = Enum.GetName(typeof(EnumMaterialMngBalRule), detail.BalRule);
        //        flexGrid1.Cell(detailStartRowNumber + i, 3).Alignment = FlexCell.AlignmentEnum.LeftCenter;

        //        //数量
        //        flexGrid1.Cell(detailStartRowNumber + i, 4).Text = detail.Quantity.ToString();

        //        //日租金
        //        flexGrid1.Cell(detailStartRowNumber + i, 5).Text = "";

        //        //金额
        //        flexGrid1.Cell(detailStartRowNumber + i, 6).Text = "";

        //        //备注
        //        flexGrid1.Cell(detailStartRowNumber + i, 7).Text = detail.Descript;
        //        flexGrid1.Cell(detailStartRowNumber + i, 7).Alignment = FlexCell.AlignmentEnum.LeftCenter;
        //    }
        //}
    }
}
