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
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Windows.Media.Media3D;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using System.Windows.Documents;
using Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.OBS;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.ProInsRecordMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using System.IO;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using IRPServiceModel.Domain.Document;
using Application.Business.Erp.SupplyChain.Client.FileUpload;
using NHibernate.Criterion;
using IRPServiceModel.Basic;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ProfessionInspectionRecordMng
{
    public partial class VProInsRecord : TMasterDetailView
    {
        private MProInsRecordMng model = new MProInsRecordMng();
        private ProfessionInspectionRecordMaster curBillMaster;
        CurrentProjectInfo projectInfo;
        string userName = string.Empty;
        string jobId = string.Empty;
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        /// <summary>
        /// 当前单据
        /// </summary>
        public ProfessionInspectionRecordMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VProInsRecord()
        {
            InitializeComponent();
            this.tabControl2.TabPages.Remove(this.明细操作区);
            InitData();
            InitEvent();
            InitDate();
        }

        public void InitData()
        {

            colInspectionConclusion.DataSource = Enum.GetNames(typeof(EnumConclusionType));
           
            projectInfo = StaticMethod.GetProjectInfo();
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
            RefreshControls(MainViewState.Browser);
        }

        public void InitDate()
        {
            //DateTimePicker dp = new DateTimePicker();
            //DataGridViewCalendarColumn dc = new DataGridViewCalendarColumn();
            //dp.CustomFormat = "yyyy-MM-dd";
            //dp.Format = DateTimePickerFormat.Custom;
            //dp.Visible = false;
            //dgDetail.Controls.Add(dp);
            //dp = new DateTimePicker();
            txtInspectionSpecail.Items.Clear();
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.WBS_CheckRequire);
            foreach (BasicDataOptr b in list)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                li.Text = b.BasicName;
                li.Value = b.BasicCode;
                txtInspectionSpecail.Items.Add(li);
            }

            txtConclusion.DataSource = (Enum.GetNames(typeof(EnumConclusionType)));
            VBasicDataOptr.InitDangerType(colDangerType, false);
            txtDangerLevel.Items.AddRange(new object[] { "一般", "重要", "紧急" });
            
        }

        private void InitEvent()
        {
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.SelectionChanged += new EventHandler(dgDetail_SelectionChanged);
            this.dgDetail.MouseClick += new MouseEventHandler(dgDetail_MouseClick);
            //右键删除菜单
            this.txtInspectionSpecail.SelectedIndexChanged += new EventHandler(txtInspectionSpecail_SelectedIndexChanged);
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.txtConclusion.SelectedIndexChanged += new EventHandler(txtConclusion_SelectedIndexChanged);

            this.btnSearch.Click += new EventHandler(btnSearch_Click);
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

            this.btnSelSJCDDW.Click += new EventHandler(btnSelSJCDDW_Click);
            this.btnSelZRGLZ.Click += new EventHandler(btnSelZRGLZ_Click);

            this.btnAdd.Click += new EventHandler(btnAdd_Click);
            this.btnDel.Click += new EventHandler(btnDel_Click);

        }

        void dgDetail_MouseClick(object sender, MouseEventArgs e)
        {
            if (!dgDetail.ReadOnly)
            {
                if (e.Button == MouseButtons.Right)
                {
                    bool isNotNewRow = this.dgDetail.CurrentRow.Cells[colInspectionSupplier.Name].Value != null         //受检承担单位
                              || this.dgDetail.CurrentRow.Cells[colInspectionPerson.Name].Value != null       //受检管理者

                              || this.dgDetail.CurrentRow.Cells[colInspectionContent.Name].Value != null      //检查内容
                              || this.dgDetail.CurrentRow.Cells[colInspectionConclusion.Name].Value != null   //检查结论

                              || this.dgDetail.CurrentRow.Cells[colCorrectiveSign.Name].Value != null    //是否整改


                              //|| this.dgDetail.CurrentRow.Cells[colInspectionRequireDate.Name].Value != null  //要求整改时间
                              || (this.dgDetail.CurrentRow.Cells[colInspectionRequire.Name].Value != null      //整改措施
                                  && ClientUtil.ToString(this.dgDetail.CurrentRow.Cells[colInspectionRequire.Name].Value).Trim().Length > 0)

                              || (this.dgDetail.CurrentRow.Cells[colDangerType.Name].Value != null             //隐患类型
                                  && ClientUtil.ToString(this.dgDetail.CurrentRow.Cells[colDangerType.Name].Value).Trim().Length > 0)
                              || (this.dgDetail.CurrentRow.Cells[colDangerLevel.Name].Value != null             //隐患级别
                                  && ClientUtil.ToString(this.dgDetail.CurrentRow.Cells[colDangerLevel.Name].Value).Trim().Length > 0)
                              || (this.dgDetail.CurrentRow.Cells[colDangerPart.Name].Value != null             //隐患部位
                                  && ClientUtil.ToString(this.dgDetail.CurrentRow.Cells[colDangerPart.Name].Value).Trim().Length > 0)

                              || (this.dgDetail.CurrentRow.Cells[colDescription.Name].Value != null            //备注
                                  && ClientUtil.ToString(this.dgDetail.CurrentRow.Cells[colDescription.Name].Value).Trim().Length > 0)
                              ;
                    if (dgDetail.Enabled && isNotNewRow)
                    {
                        cmsDg.Items[tsmiDel.Name].Enabled = true;
                        cmsDg.Show(MousePosition.X, MousePosition.Y);
                    }
                }
            }
        }

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (ViewState == MainViewState.Browser || ViewState== MainViewState.Initialize) return;

            bool isNotNewRow = this.dgDetail.CurrentRow.Cells[colInspectionSupplier.Name].Value != null         //受检承担单位
                                || this.dgDetail.CurrentRow.Cells[colInspectionPerson.Name].Value != null       //受检管理者
                              
                                || this.dgDetail.CurrentRow.Cells[colInspectionContent.Name].Value != null      //检查内容
                                || this.dgDetail.CurrentRow.Cells[colInspectionConclusion.Name].Value != null   //检查结论

                                || this.dgDetail.CurrentRow.Cells[colCorrectiveSign.Name].Value != null    //是否整改


                                //|| this.dgDetail.CurrentRow.Cells[colInspectionRequireDate.Name].Value != null  //要求整改时间
                                || (this.dgDetail.CurrentRow.Cells[colInspectionRequire.Name].Value != null      //整改措施
                                    && ClientUtil.ToString(this.dgDetail.CurrentRow.Cells[colInspectionRequire.Name].Value).Trim().Length > 0)

                                || (this.dgDetail.CurrentRow.Cells[colDangerType.Name].Value != null             //隐患类型
                                    && ClientUtil.ToString(this.dgDetail.CurrentRow.Cells[colDangerType.Name].Value).Trim().Length > 0)
                                || (this.dgDetail.CurrentRow.Cells[colDangerLevel.Name].Value != null             //隐患级别
                                    && ClientUtil.ToString(this.dgDetail.CurrentRow.Cells[colDangerLevel.Name].Value).Trim().Length > 0)
                                || (this.dgDetail.CurrentRow.Cells[colDangerPart.Name].Value != null             //隐患部位
                                    && ClientUtil.ToString(this.dgDetail.CurrentRow.Cells[colDangerPart.Name].Value).Trim().Length > 0)

                                || (this.dgDetail.CurrentRow.Cells[colDescription.Name].Value != null            //备注
                                    && ClientUtil.ToString(this.dgDetail.CurrentRow.Cells[colDescription.Name].Value).Trim().Length > 0)
                                ;

            IList list = null;
            switch (this.dgDetail.Columns[e.ColumnIndex].Name)  
            {
                case "colInspectionSupplier":
                    if (txtInspectionSpecail.Text == "")
                    {
                        MessageBox.Show("请选择检查专业");
                        return;
                    }
                    VContractExcuteSelector vmros = new VContractExcuteSelector();
                    vmros.ShowDialog();
                    list = vmros.Result;
                    if (list == null || list.Count == 0) return;
                    SubContractProject engineerMaster = list[0] as SubContractProject;
                    if (isNotNewRow)
                    {
                        this.dgDetail.CurrentRow.Cells[colInspectionSupplier.Name].Value = engineerMaster.BearerOrgName;
                        this.dgDetail.CurrentRow.Cells[colInspectionSupplier.Name].Tag = engineerMaster;
                    }
                    else
                    {
                        int i = this.dgDetail.Rows.Add();
                        this.dgDetail[colInspectionSupplier.Name, i].Value = engineerMaster.BearerOrgName;
                        this.dgDetail[colInspectionSupplier.Name, i].Tag = engineerMaster;

                        if (this.dgDetail[this.colInspectionRequireDate.Name, i].Value == null)
                            this.dgDetail[this.colInspectionRequireDate.Name, i].Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    }
                    break;
                #region old code
                case "colInspectionPerson":
                    //CommonPerson personSelector = new CommonPerson();
                    //personSelector.OpenSelect();

                    //list = personSelector.Result;
                    //if (list != null && list.Count > 0)
                    //{
                    //    PersonInfo thePerson = list[0] as PersonInfo;
                    //    if (isNotNewRow)
                    //    {
                    //        this.dgDetail.CurrentRow.Cells[colInspectionPerson.Name].Value = thePerson.Name;
                    //        this.dgDetail.CurrentRow.Cells[colInspectionPerson.Name].Tag = thePerson;
                    //    }
                    //    else
                    //    {
                    //        int i = this.dgDetail.Rows.Add();
                    //        this.dgDetail[colInspectionPerson.Name, i].Value = thePerson.Name;
                    //        this.dgDetail[colInspectionPerson.Name, i].Tag = thePerson.Name;
                    //        if (dgDetail[this.colInspectionRequireDate.Name, i].Value == null)
                    //            dgDetail[this.colInspectionRequireDate.Name, i].Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    //    }
                    //}

                    break;
                #endregion
                case "colDangerType":
                    VDangerTypeSelector select = new VDangerTypeSelector(txtDangerType.Text.ToString());
                    select.ShowDialog();

                    if (isNotNewRow)
                    {
                        this.dgDetail.CurrentRow.Cells[colDangerType.Name].Value = select.Result;

                    }
                    else
                    {
                        int i = this.dgDetail.Rows.Add();
                        this.dgDetail[colDangerType.Name, i].Value = select.Result;
                        if (dgDetail[this.colInspectionRequireDate.Name, i].Value == null)
                            dgDetail[this.colInspectionRequireDate.Name, i].Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    }


                    break;
                case "colInspectionContent"://检查内容                
  
                case "colInspectionRequire"://整改措施
                case "colDangerPart"://隐患部位  
                case "colDescription"://备注
                    if (isNotNewRow)
                    {
                        VTextSetDlg dlg = new VTextSetDlg();
                        dlg.StrInput = ClientUtil.ToString(this.dgDetail.CurrentRow.Cells[this.dgDetail.Columns[e.ColumnIndex].Name].Value);
                        dlg.ShowDialog();
                        this.dgDetail.CurrentRow.Cells[this.dgDetail.Columns[e.ColumnIndex].Name].Value = dlg.StrInput;
                    }
                    else
                    {
                        VTextSetDlg dlg = new VTextSetDlg();
                        dlg.ShowDialog();
                        if (dlg.StrInput !=null && dlg.StrInput.Trim().Length > 0)
                        {
                            int i = this.dgDetail.Rows.Add();
                            this.dgDetail[this.dgDetail.Columns[e.ColumnIndex].Name, i].Value = dlg.StrInput;

                            if (dgDetail[this.colInspectionRequireDate.Name, i].Value == null)
                                dgDetail[this.colInspectionRequireDate.Name, i].Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        }
                    }
                    break;
                default:
                    break;
            }
           

        }

        void btnDel_Click(object sender, EventArgs e)
        {
            if (this.dgDetail.CurrentRow == null)
            {
                MessageBox.Show("请选择您要删除的行！");
                return;
            }
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr = dgDetail.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                dgDetail.Rows.Remove(dr);
                if (dr.Tag != null)
                {
                    curBillMaster.Details.Remove(dr.Tag as ProfessionInspectionRecordDetail);
                }
            }
            

        }

        //private void RefreshControls(string  str)
        //{
        //    bool hasRows = this.dgDetail.Rows.Count > 0;
        //    switch (str)
        //    {
        //        case "btnAddClicked":
        //            this.btnSave.Enabled = true;
        //            this.btnDel.Enabled = false;
        //            this.btnAdd.Enabled = false;
        //            break;
        //        case "btnSaveClicked":
        //            this.btnAdd.Enabled = true;
        //            this.btnSave.Enabled = false;
        //            this.btnDel.Enabled = hasRows;
        //            break;
        //        default:
        //            break;
        //    }
        //    //this.btnSave.Enabled = 
        //}
        void btnAdd_Click(object sender, EventArgs e)
        {
            ViewState = MainViewState.Modify;
            RefreshControls(MainViewState.Modify);
            //RefreshControls("btnAddClicked");
            ClearCtlContent();      
        }

       

       

        void btnSelZRGLZ_Click(object sender, EventArgs e)
        {
            if (this.txtSJCDDW.Tag != null)
            {
                CommonPerson personSelector = new CommonPerson();
                personSelector.OpenSelect();
               

                IList list = personSelector.Result;
                if (list != null && list.Count > 0)
                {
                    PersonInfo thePerson = list[0] as PersonInfo;
                    this.txtSJZRGLZ.Text = thePerson.Name;
                    this.txtSJZRGLZ.Tag = thePerson;


                    txtCode.Focus();
                }
            }
            else
            {
                MessageBox.Show("请先选择承担单位信息");
            }
        }

        void btnSelSJCDDW_Click(object sender, EventArgs e)
        {
            if (txtInspectionSpecail.Text == "")
            {
                MessageBox.Show("请选择检查专业");
                return;
            }

            VContractExcuteSelector vmros = new VContractExcuteSelector();
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject engineerMaster = list[0] as SubContractProject;

            this.txtSJCDDW.Text = engineerMaster.BearerOrgName;
            this.txtSJCDDW.Tag = engineerMaster;


            txtContent.Focus();

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
            if (curBillMaster.Id == null)
            {
                if (MessageBox.Show("当前业务对象还没保存，是否保存！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    if (!ValidView()) return;
                    try
                    {
                        if (!ViewToModel()) return;
                        curBillMaster = model.ProfessionInspectionSrv.SaveProfessionInspectionRecordPlan(curBillMaster);
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

        void btnSearch_Click(object sender, EventArgs e)
        {
            VDangerTypeSelector select = new VDangerTypeSelector(txtDangerType.Text.ToString());
            select.ShowDialog();
            txtDangerType.Text = select.Result;
            //dgDetail.CurrentRow.Cells[colDangerType.Name].Value = txtDangerType.Text;
        }

        void txtConclusion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.txtContent.Enabled)
            {
                if (dgDetail.CurrentRow != null)
                {
                    if (valid())
                    {
                        if (dgDetail.CurrentRow.IsNewRow) return;
    
                        if (txtConclusion.SelectedItem.Equals("检查通过"))
                        {
                            radioButton1.Checked = true;
                            radioButton1.Enabled = false;
                            radioButton2.Enabled = false;
                            txtCompletDate.Enabled = false;
                            //if (dgDetail.CurrentRow != null)
                            //{
                            //    dgDetail.CurrentRow.Cells[colInspectionConclusion.Name].Value = txtConclusion.Text;
                            //    dgDetail.CurrentRow.Cells[colInspectionRequireDate.Name].Value = "";
                            //}
                        }
                        if (txtConclusion.SelectedItem.Equals("检查不通过"))
                        {
                            radioButton2.Checked = true;
                            radioButton1.Enabled = true;
                            radioButton2.Enabled = true;
                            txtCompletDate.Enabled = true;
                            //if (dgDetail.CurrentRow != null)
                            //{
                            //    dgDetail.CurrentRow.Cells[colInspectionConclusion.Name].Value = txtConclusion.Text;
                            //    dgDetail.CurrentRow.Cells[colInspectionRequireDate.Name].Value = txtCompletDate.Value.ToShortDateString();
                            //}
                        }
                    }
                }
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (!valid()) return;
            int i = dgDetail.Rows.Add();
            dgDetail[colInspectionContent.Name,i].Value = txtContent.Text;
            dgDetail[colInspectionConclusion.Name, i].Value = txtConclusion.Text;
            dgDetail[colDescription.Name, i].Value = txtDescription.Text;
            dgDetail[colDangerLevel.Name, i].Value = txtDangerLevel.Text;
            dgDetail[colDangerPart.Name, i].Value = txtDangerPart.Text;
            dgDetail[colDangerType.Name, i].Value = txtDangerType.Text;
            dgDetail[colInspectionRequire.Name, i].Value = txtRequired.Text;
            dgDetail[colInspectionRequireDate.Name, i].Value = txtCompletDate.Value.Date.ToShortDateString();
            dgDetail[colInspectionSupplier.Name, i].Value = this.txtSJCDDW.Text;
            dgDetail[colInspectionSupplier.Name, i].Tag = this.txtSJCDDW.Tag as SubContractProject;
            dgDetail[colInspectionPerson.Name, i].Value = this.txtSJZRGLZ.Text;
            dgDetail[colInspectionPerson.Name, i].Tag = this.txtSJZRGLZ.Tag as PersonInfo;

            //if (radioButton1.Checked)
            //{
            //    dgDetail[colRectification.Name,i].Value = "0";
            //}
            //else
            //{
            //    dgDetail[colRectification.Name,i].Value = "1";
            //}
            //RefreshControls("btnSaveClicked");
                 
        }
        bool valid()
        {
            bool flag = true;
            if (txtInspectionSpecail.Text == "")
            {
                MessageBox.Show("请先选择检查专业！");
                flag = false;
                return flag;
            }
            //if (this.txtSJCDDW.Tag == null || this.txtSJCDDW.Text == "")
            //{
            //    MessageBox.Show("请先选择受检承担单位！");
            //    flag = false;
            //    return flag;
            //}
            //if (this.txtSJZRGLZ.Tag == null || this.txtSJZRGLZ.Text == "")
            //{
            //    MessageBox.Show("请先选择受检责任管理者！");
            //    flag = false;
            //    return flag;
            //}

            foreach (DataGridViewRow  dr in this.dgDetail.Rows)
            {
                if (dr.Index == this.dgDetail.Rows.Count - 1)
                    continue;
                if(dr.Cells[this.colInspectionSupplier.Name].Value == null || ClientUtil.ToString(dr.Cells[this.colInspectionSupplier.Name].Value)== "")
                {
                    MessageBox.Show("请先选择受检承担单位！");
                    flag = false;
                    return flag;
                }
                if (dr.Cells[this.colInspectionPerson.Name].Value == null || ClientUtil.ToString(dr.Cells[this.colInspectionPerson.Name].Value) == "")
                {
                    MessageBox.Show("请先选择受检责任管理者！");
                    flag = false;
                    return flag;
                }

            }
            return flag;
        }

        private void dgDetail_SelectionChanged(object sender, EventArgs e)
        {
            //dgdetailChange();
        }

        void dgdetailChange()
        {
            if (dgDetail.CurrentRow.IsNewRow)
            {
                txtRequired.Text = "";
                txtContent.Text = "";
                txtDescription.Text = "";
                txtDangerPart.Text = "";
                return;
            }
            if (dgDetail.Rows.Count  != 0)
            {

                txtDescription.Text = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colDescription.Name].Value);
                txtContent.Text = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colInspectionContent.Name].Value);
                txtRequired.Text = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colInspectionRequire.Name].Value);
                txtConclusion.Text = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colInspectionConclusion.Name].Value);
                System.Web.UI.WebControls.ListItem li = txtInspectionSpecail.SelectedItem as System.Web.UI.WebControls.ListItem;
                if (li != null)
                {
                    if (li.Value.Equals("003"))
                    {
                        txtDangerType.Text = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colDangerType.Name].Value);
                        txtDangerPart.Text = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colDangerPart.Name].Value);
                        txtDangerLevel.SelectedItem = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colDangerLevel.Name].Value);
                        txtDangerLevel.Enabled = true;
                        txtDangerPart.Enabled = true;
                        btnSearch.Enabled = true;
                    }
                    else
                    {
                        txtDangerType.Text = "";
                        txtDangerPart.Text = "";
                        txtDangerLevel.SelectedItem = "";
                        txtDangerPart.Enabled = false;
                        txtDangerLevel.Enabled = false;
                        btnSearch.Enabled = false;
                    }
                }
                //if (ClientUtil.ToString(dgDetail.CurrentRow.Cells[colRectification.Name].Value) == "0")
                //{
                //    radioButton1.Checked = true;
                //}
                //else
                //{
                //    radioButton2.Checked = true;
                //}
                if (ClientUtil.ToDateTime(dgDetail.CurrentRow.Cells[colInspectionRequireDate.Name].Value) > ClientUtil.ToDateTime("1900-01-01"))
                {
                    if (txtCompletDate.Enabled == true)
                    {
                        txtCompletDate.Value = ClientUtil.ToDateTime(dgDetail.CurrentRow.Cells[colInspectionRequireDate.Name].Value);
                    }
                }

            }
            else
            {
                System.Web.UI.WebControls.ListItem li = txtInspectionSpecail.SelectedItem as System.Web.UI.WebControls.ListItem;
                if (li == null)
                {
                    if (txtInspectionSpecail.Enabled == true)
                    {
                        MessageBox.Show("请先选择检查专业");
                        return;
                    }
                }
            }
        }

        private void txtInspectionSpecail_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtInspectionSpecailIndexChange();
        }

        void txtInspectionSpecailIndexChange()
        {
            System.Web.UI.WebControls.ListItem li = txtInspectionSpecail.SelectedItem as System.Web.UI.WebControls.ListItem;
            if (li != null)
            {
                if (li.Value.Equals("003"))
                {
                    colDangerPart.Visible = true;
                    colDangerType.Visible = true;
                    colDangerLevel.Visible = true;
                    txtDangerLevel.Enabled = true;
                    txtDangerPart.Enabled = true;
                    txtDangerType.Enabled = true;
                    btnSearch.Enabled = true;
                }
                else
                {
                    colDangerPart.Visible = false;
                    colDangerType.Visible = false;
                    colDangerLevel.Visible = false;
                    txtDangerLevel.Enabled = false;
                    txtDangerPart.Enabled = false;
                    btnSearch.Enabled = false;
                    if (dgDetail.CurrentRow != null)
                    {
                        dgDetail.CurrentRow.Cells[colDangerLevel.Name].Value = "";
                        dgDetail.CurrentRow.Cells[colDangerPart.Name].Value = "";
                        dgDetail.CurrentRow.Cells[colDangerType.Name].Value = "";
                    }
                    txtDangerLevel.Text = "";
                    txtDangerPart.Text = "";
                    txtDangerType.Text = "";
                }
                this.txtConclusion.Enabled = true;
                this.txtContent.Enabled = true;
                this.txtRequired.Enabled = true;
                this.txtDescription.Enabled = true;
            }
        }


        void tsmiDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr = dgDetail.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                dgDetail.Rows.Remove(dr);
                if (dr.Tag != null)
                {
                    curBillMaster.Details.Remove(dr.Tag as ProfessionInspectionRecordDetail);
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
                    curBillMaster = model.ProfessionInspectionSrv.GetProfessionInspectionRecordById(Id);
                    ModelToView();
                    RefreshState(MainViewState.Browser);
                    btnDownLoadDocument.Enabled = true;
                    btnOpenDocument.Enabled = true;
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

            if (state == MainViewState.Modify || state == MainViewState.AddNew)
                cmsDg.Enabled = true;
    
            return;
            //控制表格
            switch (state)
            {
                case MainViewState.AddNew:
                    RefreshState(MainViewState.Browser);
                    //RefreshControls("btnSaveClicked");
                    ViewState = MainViewState.AddNew;
                    ToolMenu.LockItem(ToolMenuItem.AddNew);
                    ToolMenu.UnlockItem(ToolMenuItem.Save);
                    ToolMenu.LockItem(ToolMenuItem.Delete);
                    ToolMenu.LockItem(ToolMenuItem.Modify);
                    ToolMenu.LockItem(ToolMenuItem.Check);
                    break;
                case MainViewState.Modify:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditOnEnter;
                    System.Web.UI.WebControls.ListItem li = txtInspectionSpecail.SelectedItem as System.Web.UI.WebControls.ListItem;
                    if (li != null)
                    {
                        if (li.Value.Equals("003"))
                        {
                            this.btnSearch.Enabled = true;
                            this.txtDangerLevel.Enabled = true;
                            this.txtDangerPart.Enabled = true;
                        }
                        else
                        {
                            this.btnSearch.Enabled = false;
                            this.txtDangerLevel.Enabled = false;
                            this.txtDangerPart.Enabled = false;
                        }
                        this.txtConclusion.Enabled = true;
                        this.txtInspectionSpecail.Enabled = true;
                        this.txtContent.Enabled = true;
                        this.txtRequired.Enabled = true;
                        this.txtDescription.Enabled = true;
                        if (txtConclusion.SelectedItem.ToString().Trim() == "检查通过")
                        {
                            radioButton1.Enabled = false;
                            radioButton2.Enabled = false;
                            this.txtCompletDate.Enabled = false;
                        }
                        else
                        {
                            radioButton1.Enabled = true;
                            radioButton2.Enabled = true;
                            this.txtCompletDate.Enabled = true;
                        }
                        this.btnSave.Enabled = true;
                        cmsDg.Enabled = true;
                    }
                    else
                    {
                        radioButton1.Enabled = false;
                        radioButton2.Enabled = false;
                        this.btnSave.Enabled = false;
                        this.txtContent.Enabled = false;
                        this.txtRequired.Enabled = false;
                        this.txtDangerPart.Enabled = false;
                        this.txtDescription.Enabled = false;
                        this.txtConclusion.Enabled = false;
                        this.btnSearch.Enabled = false;
                        this.txtDangerLevel.Enabled = false;
                        this.txtCompletDate.Enabled = false;
                        cmsDg.Enabled = false;
                    }
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
                    this.txtInspectionSpecail.Enabled = false;
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    this.btnSave.Enabled = false;
                    this.txtContent.Enabled = false;
                    this.txtRequired.Enabled = false;
                    this.txtDangerPart.Enabled = false;
                    this.txtDescription.Enabled = false;
                    this.txtConclusion.Enabled = false;
                    this.btnSearch.Enabled = false;
                    this.txtDangerLevel.Enabled = false;
                    this.txtCompletDate.Enabled = false;
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
                btnStates(1);
                if (txtInspectionSpecail.Text != "安全员检查")
                {
                    txtDangerType.Enabled = false;
                    txtDangerLevel.Enabled = false;
                    btnSearch.Enabled = true;
                    btnSave.Enabled = true;
                }
            }
            else
            {
                ObjectLock.Lock(pnlFloor, true);
                btnSearch.Enabled = false;
                btnSave.Enabled = false;
                btnStates(0);
            }

            //永久锁定
            object[] os = new object[] { txtCode, txtCreatePerson, txtHandlePerson, txtProject, txtDangerType };
            ObjectLock.Lock(os);

            //string[] lockCols = new string[] { colInspectionSupplier.Name, colInspectionPerson.Name, colRectification.Name, colInspectionConclusion.Name, colInspectionRequireDate.Name, colDangerType.Name, colDangerLevel.Name, colDangerPart.Name, colInspectionRequire.Name, colInspectionContent.Name, colDescription.Name };
            string[] lockCols = new string[] { colInspectionSupplier.Name, colDangerType.Name, colDangerPart.Name, colInspectionRequire.Name, colInspectionContent.Name, colDescription.Name };
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
        private void ClearCtlContent()
        { 
            ClearContent(pnlFloor);
        }

        private void ClearContent(Control c)
        {
            foreach (Control cd in c.Controls)
            {
                ClearContent(cd);
            }
            if (c is CustomEdit || c is TextBox)
            {
                c.Tag = null;
                c.Text = "";
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
                if (dgDetail.Rows.Count > 1)
                {
                    ClearView();
                }
                movedDtlList = new ArrayList();
                this.curBillMaster = new ProfessionInspectionRecordMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;//制单人编号
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;//制单年
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;//制单月
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//登录业务组织
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//业务组织名称
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;//业务组织编号
                curBillMaster.HandOrgLevel = ConstObject.TheOperationOrg.Level;
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;//负责人
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;//负责人名称
                curBillMaster.DocState = DocumentState.Edit;//状态
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;//负责人
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;//负责人名称
                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //责任人
                txtHandlePerson.Tag = ConstObject.LoginPersonInfo;
                txtHandlePerson.Text = ConstObject.LoginPersonInfo.Name;
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
            if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
            {
                base.ModifyView();
                curBillMaster = model.ProfessionInspectionSrv.GetProfessionInspectionRecordById(curBillMaster.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(curBillMaster.DocState));
            MessageBox.Show(message);
            return false;
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
                if (!valid()) return false;
                curBillMaster.DocState = DocumentState.InExecute;
                curBillMaster.AuditPerson = ConstObject.LoginPersonInfo;//制单人编号
                curBillMaster.AuditPersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                curBillMaster.AuditDate = ConstObject.LoginDate;//制单时间
                curBillMaster.AuditYear = ConstObject.LoginDate.Year;//制单年
                curBillMaster.AuditMonth = ConstObject.LoginDate.Month;//制单月
                curBillMaster = model.ProfessionInspectionSrv.SaveProfessionInspectionRecordPlan(curBillMaster);
                //插入日志
                txtCode.Text = curBillMaster.Code;
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
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
                if (!valid()) return false;
                bool flag = false;
                if (string.IsNullOrEmpty(curBillMaster.Id))
                {
                    flag = true;
                }
                curBillMaster = model.ProfessionInspectionSrv.SaveProfessionInspectionRecordPlan(curBillMaster);

                //插入日志
                txtCode.Text = curBillMaster.Code;
                //更新Caption
                LogData log = new LogData();
                log.BillId = curBillMaster.Id;
                log.BillType = "检查记录单";
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

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                ProfessionInspectionRecordDetail ad = new ProfessionInspectionRecordDetail();
                if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
                {
                    if (ad.RefQuantity > 0)
                    {
                        MessageBox.Show("此信息被引用，删除失败！");
                        return false;
                    }
                    else
                    {
                        curBillMaster = model.ProfessionInspectionSrv.GetProfessionInspectionRecordById(curBillMaster.Id);
                        if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                        {
                            //if (!model.ProfessionInspectionSrv.DeleteByDao(curBillMaster)) return false;
                            if (!msrv.DeleteReceiptAndDocument(curBillMaster, curBillMaster.Id)) return false;
                            LogData log = new LogData();
                            log.BillId = curBillMaster.Id;
                            log.BillType = "检查记录单";
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
                }
                string message = "此单状态为【{0}】，不能删除！";
                message = string.Format(message, ClientUtil.GetDocStateName(curBillMaster.DocState));
                MessageBox.Show(message);
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
                        curBillMaster = model.ProfessionInspectionSrv.GetProfessionInspectionRecordById(curBillMaster.Id);
                        ModelToView();
                        break;
                    default:
                        if (dgDetail.Rows.Count > 1)
                        {
                            ClearView();
                        }
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

        /// <summary>
        /// 刷新
        /// </summary>
        public override void RefreshView()
        {
            try
            {
                //重新获得当前对象的值
                curBillMaster = model.ProfessionInspectionSrv.GetProfessionInspectionRecordById(curBillMaster.Id);
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
            if (ClientUtil.ToString(this.txtInspectionSpecail.Text) == "")
            {
                MessageBox.Show("检查专业不能为空！");
                return false;
            }
            string validMessage = "";
            if (this.dgDetail.Rows.Count == 0)
            {
                MessageBox.Show("明细不能为空！");
                return false;
            }
            dgDetail.EndEdit();
            if (validMessage != "")
            {
                MessageBox.Show(validMessage);
                return false;
            }

            //明细表数据校验
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                //最后一行不进行校验
                if (dr.IsNewRow) break;

                if (dr.Cells[colInspectionSupplier.Name].Value == null)
                {
                    MessageBox.Show("受检承担单位不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colInspectionSupplier.Name];
                    return false;
                }
            }
            dgDetail.Update();
            return true;
        }

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                curBillMaster.Descript = ClientUtil.ToString(this.txtDescript.Text);
                curBillMaster.InspectionSpecail = ClientUtil.ToString(this.txtInspectionSpecail.SelectedItem);
                curBillMaster.CreateDate = Convert.ToDateTime(this.dtpDateBegin.Value);
                curBillMaster.HandlePerson = this.txtHandlePerson.Tag as PersonInfo;
                curBillMaster.HandlePersonName = ClientUtil.ToString(this.txtHandlePerson.Value);
                curBillMaster.Details.Clear();
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    ProfessionInspectionRecordDetail curBillDtl = new ProfessionInspectionRecordDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as ProfessionInspectionRecordDetail;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    if (curBillMaster.Id != null)
                    {
                        if (curBillDtl.CorrectiveSign != 2)
                        {
                            if (radioButton1.Checked)
                            {
                                curBillDtl.CorrectiveSign = 0;//整改标志
                            }
                            else
                            {
                                curBillDtl.CorrectiveSign = 1;
                            }
                        }
                    }

                    curBillDtl.DangerLevel = ClientUtil.ToString(var.Cells[colDangerLevel.Name].Value);//隐患级别
                    curBillDtl.DangerPart = ClientUtil.ToString(var.Cells[colDangerPart.Name].Value);//隐患部位
                    curBillDtl.DangerType = ClientUtil.ToString(var.Cells[colDangerType.Name].Value);//隐患类型
                    curBillDtl.MeasureRequired = ClientUtil.ToString(var.Cells[colInspectionRequire.Name].Value);//整改措施要求
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescription.Name].Value);//备注
                    string strName = ClientUtil.ToString(var.Cells[colInspectionConclusion.Name].Value);
                    if (strName.Equals("检查通过"))
                    {
                        curBillDtl.InspectionConclusion = 0;
                        curBillDtl.InspectionDate = ClientUtil.ToDateTime("1900-1-1");
                    }
                    if (strName.Equals("检查不通过"))
                    {
                        curBillDtl.InspectionConclusion = 1;
                        curBillDtl.InspectionDate = ClientUtil.ToDateTime(var.Cells[colInspectionRequireDate.Name].Value);//要求整改时间
                    }
                    ////整改标志
                    //if (radioButton1.Checked)
                    //{
                    //    //不需要整改
                    //    curBillDtl.CorrectiveSign = 0;
                    //}
                    //if (radioButton2.Checked)
                    //{
                    //    //需要整改
                    //    curBillDtl.CorrectiveSign = 1;
                    //}


                    switch (ClientUtil.ToString(var.Cells[colCorrectiveSign.Name].Value).Trim())
                    {
                        case "不需整改":
                            curBillDtl.CorrectiveSign = 0;
                            break;
                        case"需要整改":
                            curBillDtl.CorrectiveSign = 1;
                            break;
                    }

                    curBillDtl.InspectionContent = ClientUtil.ToString(var.Cells[colInspectionContent.Name].Value);//检查内容
                    curBillDtl.InspectionPerson = var.Cells[colInspectionPerson.Name].Tag as PersonInfo;//受检管理负责人
                    curBillDtl.InspectionPersonName = ClientUtil.ToString(var.Cells[colInspectionPerson.Name].Value);//受检管理负责人名称
                    curBillDtl.InspectionSupplier = var.Cells[colInspectionSupplier.Name].Tag as SubContractProject;//受检承担单位
                    curBillDtl.InspectionSupplierName = ClientUtil.ToString(var.Cells[colInspectionSupplier.Name].Value);//受检承担单位名称                   
                    curBillMaster.AddDetails(curBillDtl);
                    var.Tag = curBillDtl;
                }
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
                this.txtCode.Text = curBillMaster.Code;
                dtpDateBegin.Value = curBillMaster.CreateDate;
                this.txtHandlePerson.Result.Clear();
                this.txtHandlePerson.Result.Add(curBillMaster.HandlePerson);
                txtHandlePerson.Value = curBillMaster.HandlePersonName;
                txtDescript.Text = curBillMaster.Descript;
                this.txtCreatePerson.Tag = curBillMaster.CreatePerson;
                txtCreatePerson.Text = curBillMaster.CreatePersonName;
                this.txtInspectionSpecail.Text = curBillMaster.InspectionSpecail;//检查专业
                IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.WBS_CheckRequire);
                foreach (BasicDataOptr b in list)
                {
                    if (b.BasicName == ClientUtil.ToString(curBillMaster.InspectionSpecail))
                    {
                        System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                        li.Text = b.BasicName;
                        li.Value = b.BasicCode;
                        txtInspectionSpecail.SelectedItem = b.BasicName;
                        break;
                    }
                }
                this.txtCreatePerson.Text = curBillMaster.CreatePersonName;
                txtProject.Text = curBillMaster.ProjectName.ToString();
                this.dgDetail.Rows.Clear();
                foreach (ProfessionInspectionRecordDetail var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();
                    this.dgDetail[colInspectionSupplier.Name, i].Value = var.InspectionSupplierName;
                    this.dgDetail[colInspectionSupplier.Name, i].Tag = var.InspectionSupplier;
                    this.dgDetail[colInspectionRequire.Name, i].Value = var.MeasureRequired;
                    this.dgDetail[colInspectionPerson.Name, i].Tag = var.InspectionPerson;
                    this.dgDetail[colInspectionPerson.Name, i].Value = var.InspectionPersonName;
                    this.dgDetail[colInspectionContent.Name, i].Value = var.InspectionContent;
                    //this.dgDetail[colRectification.Name, i].Value = var.CorrectiveSign;
                    this.dgDetail[colDangerType.Name, i].Value = var.DangerType;
                    this.dgDetail[colDangerPart.Name, i].Value = var.DangerPart;
                    this.dgDetail[colDangerLevel.Name, i].Value = var.DangerLevel;
                    this.dgDetail[colDescription.Name, i].Value = var.Descript;
                    string strCorr = ClientUtil.ToString(var.CorrectiveSign);
                    if (strCorr.Equals("0"))
                    {
                        this.radioButton1.Checked = true;
                        this.dgDetail[colCorrectiveSign.Name, i].Value = "不需整改";
                    }
                    if (strCorr.Equals("1"))
                    {
                        this.radioButton2.Checked = true;
                        this.dgDetail[colCorrectiveSign.Name, i].Value = "需要整改";
                    }
                    if (var.InspectionConclusion.Equals(0))
                    {
                        this.dgDetail[colInspectionConclusion.Name, i].Value = "检查通过";
                        this.dgDetail[colInspectionRequireDate.Name, i].Value = "";
                    }
                    if (var.InspectionConclusion.Equals(1))
                    {
                        this.dgDetail[colInspectionConclusion.Name, i].Value = "检查不通过";
                      
                    }
                    if (var.InspectionDate.Date > ClientUtil.ToDateTime("1900-01-01"))
                    {
                        this.dgDetail[colInspectionRequireDate.Name, i].Value = var.InspectionDate.ToShortDateString();
                    }
                    this.dgDetail.Rows[i].Tag = var;
                }
                //dgdetailChange();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

 

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            if (LoadTempleteFile(@"专业检查记录.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            return true;
        }

        public override bool Print()
        {
            if (LoadTempleteFile(@"专业检查记录.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.Print();
            return true;
        }

        public override bool Export()
        {
            if (LoadTempleteFile(@"专业检查记录.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.ExportToExcel("专业检查记录【" + curBillMaster.Code + "】", false, false, true);
            return true;
        }

        private bool LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式和数据
                flexGrid1.OpenFile(path + "\\" + modelName);//载入格式
            }
            else
            {
                MessageBox.Show("未找到模板格式文件【" + modelName + "】");
                return false;
            }
            return true;
        }

        private void FillFlex(ProfessionInspectionRecordMaster billMaster)
        {
            int detailCount = billMaster.Details.Count;

            //插入明细行
            //flexGrid1.InsertRow(detailStartRowNumber, detailCount);

            //设置单元格的边框，对齐方式
            //FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
            ////range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            ////range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            ////range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            ////range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            //range.Mask = FlexCell.MaskEnum.Digital;
            FlexCell.PageSetup pageSetup = flexGrid1.PageSetup;
            pageSetup.LeftMargin = 0;
            pageSetup.RightMargin = 0;
            pageSetup.BottomMargin = 1;
            pageSetup.TopMargin = 1;
            pageSetup.CenterHorizontally = true;

            //主表数据
            flexGrid1.Cell(2, 2).Text = billMaster.ProjectName;
            flexGrid1.Cell(3, 2).Text = billMaster.OperOrgInfoName;
            flexGrid1.Cell(3, 4).Text = billMaster.CreateDate.ToShortDateString();
            flexGrid1.Cell(4, 2).Text = billMaster.InspectionSpecail;
            flexGrid1.Cell(4, 4).Text = billMaster.CreatePersonName;
            //FlexCell.PageSetup pageSetup = flexGrid1.PageSetup;
            //pageSetup.RightFooter = "第 &P 页/共 &N 页      ";

            //System.Drawing.Printing.PaperSize paperSize = new System.Drawing.Printing.PaperSize("自定义纸张", 910, 470);
            //pageSetup.PaperSize = paperSize;

            //填写明细数据
            //for (int i = 0; i < detailCount; i++)
            //{
            //    ProfessionInspectionRecordDetail detail = (ProfessionInspectionRecordDetail)billMaster.Details.ElementAt(i);
            //    //检查记录
            //    string pro
            //}
            string strProOrg = "";
            string strIContent = "";
            int i = 1;
            if (i <= detailCount)
            {
                foreach (ProfessionInspectionRecordDetail dtl in billMaster.Details)
                {
                    strProOrg = dtl.InspectionSupplierName + ":";        //检查单位
                    strIContent = dtl.InspectionContent + ";";      //检查内容说明
                    if (i == 1)
                    {

                        flexGrid1.Cell(5, 1).Text += "检 查 记 录：\r\n  " + i + "、" + strProOrg + "\r\n           " + strIContent + "\r\n";
                    }
                    else
                    {
                        flexGrid1.Cell(5, 1).Text += "  "+i + "、" + strProOrg + "\r\n           " + strIContent + "\r\n";
                    }
                    flexGrid1.Cell(5, 1).WrapText = true;
                    i++;
                }
           

            }
        }
    }
}
