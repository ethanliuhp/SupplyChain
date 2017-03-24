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
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using System.IO;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng
{
    public partial class VContractExcuteMng : TMasterDetailView
    {
        private MContractExcuteMng model = new MContractExcuteMng();
        private SubContractProject curBillMaster;
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        //SubContractType subContractType = SubContractType.总承包;
        ////劳务分包价格明细
        //LaborSubContractPriceItem theLaborItem = null;
        ////专业分包价格明细
        //ProfessionalSubcontractPriceItem theProfessionalItem = null;
        CurrentProjectInfo projectInfo = null;
        string userName = string.Empty;
        string jobId = string.Empty;
        string fileObjectType = string.Empty;
        /// <summary>
        /// 当前单据
        /// </summary>
        public SubContractProject CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VContractExcuteMng()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        private void InitData()
        {
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            //userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            //jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
            userName = "admin";
            jobId = "AAAA4DC34F5882C122C3D0FA863D";
            ((ComboBox)comUtilities).Items.AddRange(Enum.GetNames(typeof(UtilitiesRememberMethod)));
            ((ComboBox)comConstractType).Items.AddRange(Enum.GetNames(typeof(SubContractType)));
            ((ComboBox)comConstruct).Items.AddRange(Enum.GetNames(typeof(ManagementRememberMethod)));
            ((ComboBox)txtLaborMoneyType).Items.AddRange(Enum.GetNames(typeof(ManagementRememberMethod)));
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode3 + "-" + CommonUtil.SupplierCatCode4;
            VBasicDataOptr.InitContractType(colConsType, false);
            VBasicDataOptr.InitProjectLivel(colQualityLevel, false);
            VBasicDataOptr.InitSubContractProject(colJobType, false);
            this.cmobalanceStyle.Items.AddRange(new object[] {"过程结算", "末次结算", "质保期"});
        }

        private void InitEvent()
        {
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            this.dgDetail.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgDetail_EditingControlShowing);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.txtMainMoney.tbTextChanged +=new EventHandler(txtMainMoney_tbTextChanged);
            this.dgDetail.SelectionChanged +=new EventHandler(dgDetail_SelectionChanged);
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.dgDetail.UserDeletingRow += new DataGridViewRowCancelEventHandler(dgDetail_UserDeletingRow);
            this.dgLaborDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgLaborDetail_CellDoubleClick);
            this.dgLaborDetail.CellEndEdit += new DataGridViewCellEventHandler(dgLaborDetail_CellEndEdit);
            this.dgProfessDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgProfessDetail_CellDoubleClick);
            this.dgProfessDetail.CellEndEdit += new DataGridViewCellEventHandler(dgProfessDetail_CellEndEdit);
            this.tpLaborSubCon.Parent = null;
            this.tpProSubCon.Parent = null;
            this.comConstractType.SelectedIndexChanged += new EventHandler(comConstractType_SelectedIndexChanged);
            //相关文档
            btnDownLoadDocument.Click += new EventHandler(btnDownLoadDocument_Click);
            btnOpenDocument.Click += new EventHandler(btnOpenDocument_Click);
            btnUpdateDocument.Click += new EventHandler(btnUpdateDocument_Click);
            btnDeleteDocument.Click += new EventHandler(btnDeleteDocument_Click);
            btnUpFile.Click += new EventHandler(btnUpFile_Click);
            this.comUtilities.SelectedIndexChanged +=new EventHandler(comUtilities_SelectedIndexChanged);
            this.comConstruct.SelectedIndexChanged +=new EventHandler(comConstruct_SelectedIndexChanged);
            this.txtLaborMoneyType.SelectedIndexChanged +=new EventHandler(txtLaborMoneyType_SelectedIndexChanged);
        }

        void dgDetail_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (dgDetail.CurrentRow.IsNewRow) return;
            DataGridViewRow dr = dgDetail.CurrentRow;
            if (dr == null || dr.IsNewRow) return;
            if (dr.Tag != null)
            {
                curBillMaster.Details.Remove(dr.Tag as SubContractChangeItem);
            }
        }

        void dgDetail_SelectionChanged(object sender,EventArgs e)
        {
            decimal summoney = 0;
            for (int i = 0; i <= dgDetail.RowCount - 1; i++)
            {
                object money = dgDetail.Rows[i].Cells[colUpdateMoney.Name].Value;
                summoney = summoney + ClientUtil.TransToDecimal(money);
            }
            txtSumMoney.Text = (summoney + ClientUtil.ToDecimal(txtMainMoney.Text)).ToString();
        }


        void comUtilities_SelectedIndexChanged(object sender,EventArgs e)
        {
            if (comUtilities.Text.ToString() == "按费率计取")
            {
                txtUtilitiesRate.ReadOnly = false;
            }
            else
            {
                txtUtilitiesRate.Text = "";
                txtUtilitiesRate.ReadOnly = true;
            }
        }

        void comConstruct_SelectedIndexChanged(object sender,EventArgs e)
        {
            if (comConstruct.Text.ToString() == "按费率计取")
            {
                txtConstructRate.ReadOnly = false;
            }
            else
            {
                txtConstructRate.Text = "";
                txtConstructRate.ReadOnly = true;
            }
        }

        void txtLaborMoneyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtLaborMoneyType.Text.ToString() == "按费率计取")
            {
                txtLaobrRace.ReadOnly = false;
            }
            else
            {
                txtLaobrRace.Text = "";
                txtLaobrRace.ReadOnly = true;
            }
        }

        void txtMainMoney_tbTextChanged(object sender, EventArgs e)
        {
            decimal summoney = 0;
            for (int i = 0; i <= dgDetail.RowCount - 1; i++)
            {
                object money = dgDetail.Rows[i].Cells[colUpdateMoney.Name].Value;
                summoney = summoney + ClientUtil.TransToDecimal(money);
            }
            txtSumMoney.Text = (summoney + ClientUtil.ToDecimal(txtMainMoney.Text)).ToString();
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
                    foreach(string s in errorList)
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
                        if (curBillMaster.DocState == DocumentState.Edit)
                        {
                            if (SaveOrSubmitBill(1) == false) return ;
                        }
                        MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能保存！");
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

        void comConstractType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comConstractType.SelectedItem.Equals(""))
            {
                this.tpLaborSubCon.Parent = null;
                this.tpProSubCon.Parent = null;
            }
            if (comConstractType.SelectedItem.Equals("劳务分包"))
            {
                //this.tpLaborSubCon.Parent = this.tabControl1;
                //this.tpProSubCon.Parent = null;
            }
            else if (comConstractType.SelectedItem.Equals("专业分包"))
            {
                //this.tpLaborSubCon.Parent = null;
                //this.tpProSubCon.Parent = this.tabControl1;
            }
            else
            {
                this.tpLaborSubCon.Parent = null;
                this.tpProSubCon.Parent = null;
            }
        }

        void btnSearch_Click(object sender,EventArgs e)
        {
            VSelectWBSContractGroup vmros = new VSelectWBSContractGroup(new MWBSContractGroup());
            vmros.ShowDialog();
            IList list = vmros.SelectResult;
            if (list == null || list.Count == 0) return;
            ContractGroup engineerMaster = list[0] as ContractGroup;
            txtConstract.Tag = engineerMaster;
            txtConstract.Text = engineerMaster.ContractName;
            txtsubpackage.Tag = engineerMaster;
            txtsubpackage.Text = engineerMaster.BearRange;
        }

        void tsmiDel_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            //{
            //    DataGridViewRow dr = dgDetail.CurrentRow;
            //    if (dr == null || dr.IsNewRow) return;
            //    dgDetail.Rows.Remove(dr);
            //    if (dr.Tag != null)
            //    {
            //        curBillMaster.Details.Remove(dr.Tag as SubContractChangeItem);
            //    }
            //}
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
                    curBillMaster = model.ContractExcuteSrv.GetContractExcuteById(Id);
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
                    this.comUtilities.Enabled = true;
                    this.comConstractType.Enabled = true;
                    this.comConstruct.Enabled = true;
                    colConsType.ReadOnly = false;
                    this.colProjectAmountUnit.ReadOnly = false;
                    txtUtilitiesRate.ReadOnly = false;
                    this.btnSearch.Enabled = true;
                    this.dgDetail.EditMode = DataGridViewEditMode.EditOnEnter;
                    cmsDg.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.comUtilities.Enabled = false;
                    this.comConstruct.Enabled = false;
                    colConsType.ReadOnly = true;
                    this.colProjectAmountUnit.ReadOnly = true;
                    this.comConstractType.Enabled = false;
                    this.btnSearch.Enabled = false;
                    txtUtilitiesRate.ReadOnly = true;
                    this.txtPercentage.ReadOnly = true;
                    this.txtConstructRate.ReadOnly = true;
                    this.txtMainMoney.ReadOnly = true;
                    this.txtLaobrRace.ReadOnly = true;
                    this.txtLaborMoneyType.Enabled = false;
                    this.colConstractType.ReadOnly = true;
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
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
            }
            else
            {
                ObjectLock.Lock(pnlFloor, true);
                btnStates(0);
            }

            //永久锁定
            object[] os = new object[] { txtCode, txtHandlePerson, txtProject, txtSumMoney, txtSumSettleMoney };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colPriceUnit.Name,colConstractName.Name,colConstractType.Name };
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
                base.NewView();
                ClearView();
                //btnStates(0);
                this.curBillMaster = new SubContractProject();
                //curBillMaster.CreateDate = ConstObject.LoginDate;
                curBillMaster.OwnerOrgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                curBillMaster.Owner = ConstObject.LoginPersonInfo;
                curBillMaster.OwnerName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.DocState = DocumentState.Edit;
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
                //txtContractNo.Focus();
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
                curBillMaster = model.ContractExcuteSrv.GetContractExcuteById(curBillMaster.Id);
                ModelToView();
                //btnStates(1);
                
                return true;
            }
            MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能修改！");
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
            {
                curBillMaster.DocState = DocumentState.InExecute;
            }
            curBillMaster = model.ContractExcuteSrv.SaveContractExcute(curBillMaster);
            this.txtCode.Text = curBillMaster.Code;

            log.BillId = curBillMaster.Id;
            log.BillType = "分包执行状态维护";
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
                    //btnStates(0);
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能保存！");
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
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
                if (curBillMaster.DocState == DocumentState.Edit)
                {
                    //if (SaveOrSubmitBill(2) == false) return false;
                    //MessageBox.Show("提交成功！");
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
                curBillMaster = model.ContractExcuteSrv.GetContractExcuteById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Edit)
                {
                   
                    if (!model.ContractExcuteSrv.DeleteByDao(curBillMaster)) return false;
                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "分包执行状态维护";
                    log.Code = curBillMaster.Code;
                    log.OperType = "删除";
                    log.Descript = "";
                    log.OperPerson = ConstObject.LoginPersonInfo.Name;
                    log.ProjectName = curBillMaster.ProjectName;
                    StaticMethod.InsertLogData(log);
                    ClearView();
                    FillDoc();
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
                        curBillMaster = model.ContractExcuteSrv.GetContractExcuteById(curBillMaster.Id);
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

        /// <summary> shi 
        /// 刷新
        /// </summary>
        public override void RefreshView()
        {
            try
            {
                //重新获得当前对象的值
                curBillMaster = model.ContractExcuteSrv.GetContractExcuteById(curBillMaster.Id);
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
            string validMessage = "";
            if (comUtilities.SelectedItem == null)
            {
                MessageBox.Show("请选择代缴水电费计取方式！");
                return false;
            }
            if (comConstruct.SelectedItem == null)
            {
                MessageBox.Show("请选择建设管理费计取方式！");
                return false;
            }
            if (txtLaborMoneyType.SelectedItem == null)
            {
                MessageBox.Show("请选择劳务税金计取方式！");
                return false;
            }
            if (txtSupply.Result.Count == 0)
            {
                MessageBox.Show("请选择承担队伍！");
                txtSupply.Focus();
                return false;
            }
            if (comUtilities.SelectedItem.Equals("按费率计取"))
            {
                if (txtUtilitiesRate.Text != null && ClientUtil.TransToDecimal(txtUtilitiesRate.Text) < 0)
                {
                    MessageBox.Show("代缴水电费费率不能小于0！");
                    txtUtilitiesRate.Focus();
                    return false;
                }
            }
            if (comConstruct.SelectedItem.Equals("按费率计取"))
            {
                if (txtConstructRate.Text != null && ClientUtil.TransToDecimal(txtConstructRate.Text) <= 0 || ClientUtil.TransToDecimal(txtConstructRate.Text) >= 100)
                {
                    MessageBox.Show("建设管理费费率介于0到100之间！");
                    txtUtilitiesRate.Focus();
                    return false;
                }
            }
            if (txtPercentage.Text != null && ClientUtil.TransToDecimal(txtPercentage.Text) <= 0 || ClientUtil.TransToDecimal(txtPercentage.Text) >= 100)
            {
                MessageBox.Show("允许超结百分比介于0到100之间！");
                txtUtilitiesRate.Focus();
                return false;
            }
            if (dgDetail.Rows.Count - 1 > 0)
            {
                dgDetail.EndEdit();
                dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));
            }
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
                if (dr.Cells[colConstractName.Name].Value == null)
                {
                    MessageBox.Show("契约名称不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colConstractName.Name];
                    return false;
                }
                
                if (dr.Cells[colUpdateMoney.Name].Value == null)
                {
                    MessageBox.Show("变更金额不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colUpdateMoney.Name];
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
                if (txtPercentage.Text != "")
                {
                    curBillMaster.AllowExceedPercent = ClientUtil.ToDecimal(txtPercentage.Text)/100;//允许超结百分比
                }
                if (txtSupply.Result != null && txtSupply.Result.Count > 0)
                {
                    curBillMaster.BearerOrg = txtSupply.Result[0] as SupplierRelationInfo;
                }
                curBillMaster.CreateDate = ClientUtil.ToDateTime(this.dtCreateDate.Text);
                curBillMaster.BearerOrgName = ClientUtil.ToString(txtSupply.Text);//承担组织
                //curBillMaster.ContractType = comConstractType.SelectedItem;//分包合同类型
                curBillMaster.ContractSumMoney = ClientUtil.ToDecimal(txtMainMoney.Text);
                curBillMaster.ContractType = EnumUtil<SubContractType>.FromDescription(comConstractType.SelectedItem);
                curBillMaster.BalanceStyle = this.cmobalanceStyle.Text.ToString();
                curBillMaster.SubPackage = this.txtsubpackage.Text.ToString();
                if (comConstruct.SelectedItem.Equals("按费率计取"))
                {
                    if (txtConstructRate.Text != "")
                    {
                        curBillMaster.ManagementRate = ClientUtil.ToDecimal(txtConstructRate.Text)/100;//建设管理费率
                    }
                }
                //curBillMaster.ManagementRemMethod = comConstruct.SelectedValue as ManagementRememberMethod;//建设管理计取方式
                curBillMaster.ManagementRemMethod = EnumUtil<ManagementRememberMethod>.FromDescription(comConstruct.SelectedItem);
                //curBillMaster.ContractInterimMoney = ClientUtil.ToDecimal(txtSumMoney.Text);//暂定金额
                curBillMaster.TheContractGroup = txtConstract.Tag as ContractGroup;//分包合同契约
                curBillMaster.ContractGroupCode = ClientUtil.ToString(txtConstract.Text);//编号
                curBillMaster.ContractInterimMoney = ClientUtil.ToDecimal(txtSumMoney.Text);//合同暂定金额
                if (comUtilities.SelectedItem.Equals("按费率计取"))
                {
                    if (txtUtilitiesRate.Text != "")
                    {
                        curBillMaster.UtilitiesRate = ClientUtil.ToDecimal(txtUtilitiesRate.Text)/100;//代缴水电费率
                    }
                }
                //curBillMaster.UtilitiesRemMethod = comUtilities.SelectedValue as UtilitiesRememberMethod;//代缴水电费计取方式
                curBillMaster.UtilitiesRemMethod = EnumUtil<UtilitiesRememberMethod>.FromDescription(comUtilities.SelectedItem);

                if (txtLaborMoneyType.SelectedItem.Equals("按费率计取"))
                {
                    if (txtLaobrRace.Text != "")
                    {
                        curBillMaster.LaobrRace = ClientUtil.ToDecimal(txtLaobrRace.Text) / 100;//分包劳务税金费率
                    }
                }
                //分包劳务税金计取方式
                curBillMaster.LaborMoneyType = EnumUtil<ManagementRememberMethod>.FromDescription(txtLaborMoneyType.SelectedItem);

                if (!string.IsNullOrEmpty(txtProcessPayRate.Text.Trim()))
                {
                    curBillMaster.ProcessPayRate = ClientUtil.ToDecimal(txtProcessPayRate.Text) / 100;//过程付款比例
                }
                if (!string.IsNullOrEmpty(txtCompletePayRate.Text.Trim()))
                {
                    curBillMaster.CompletePayRate = ClientUtil.ToDecimal(txtCompletePayRate.Text) / 100;//完工结算付款比例
                }
                if (!string.IsNullOrEmpty(txtWarrantyPayRate.Text.Trim()))
                {
                    curBillMaster.WarrantyPayRate = ClientUtil.ToDecimal(txtWarrantyPayRate.Text) / 100;//质保期付款比例
                }

                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    SubContractChangeItem curBillDtl = new SubContractChangeItem();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as SubContractChangeItem;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.ChangeDesc = ClientUtil.ToString(var.Cells[colUpdateDescript.Name].Value);//变更说明
                    curBillDtl.ChangeMoney = ClientUtil.ToDecimal(var.Cells[colUpdateMoney.Name].Value);//变更金额
                    curBillDtl.TheContractGroup = var.Cells[colConstractName.Name].Tag as ContractGroup;//契约组GUID
                    curBillDtl.ContractName = ClientUtil.ToString(var.Cells[colConstractName.Name].Value);//契约名称
                    curBillDtl.ContractType = ClientUtil.ToString(var.Cells[colConstractType.Name].Value);//契约类型
                    curBillDtl.PriceUnit = var.Cells[colPriceUnit.Name].Tag as StandardUnit;//价格计量单位
                    curBillDtl.PriceUnitName = ClientUtil.ToString(var.Cells[colPriceUnit.Name].Value);//价格计量单位名称
                    //curBillDtl.TheContractGroup = var.Cells[colConstractCode.Name].Tag as ContractGroup;//依据契约
                    //curBillDtl.TheProject = var.Cells[colConstractCode];//分包项目
                    curBillMaster.AddDetail(curBillDtl);
                    var.Tag = curBillDtl;
                }
                foreach (DataGridViewRow var in this.dgLaborDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    LaborSubContractPriceItem curBillLabor = new LaborSubContractPriceItem();
                    if (var.Tag != null)
                    {
                        curBillLabor = var.Tag as LaborSubContractPriceItem;
                        if (curBillLabor.Id == null)
                        {
                            curBillMaster.LaborDetails.Remove(curBillLabor);
                        }
                    }
                    curBillLabor.JobContent = ClientUtil.ToString(var.Cells[colWorkContent.Name].Value);//工作内容
                    curBillLabor.ConstractType = ClientUtil.ToString(var.Cells[colConsType.Name].Value);//承包方式
                    curBillLabor.UnitPrice = ClientUtil.ToDecimal(var.Cells[colUnitPrice.Name].Value);//价格计量单位
                    curBillLabor.Descript = ClientUtil.ToString(var.Cells[colRemark.Name].Value);//备注
                    string strPriceUnit = "元";
                    Application.Resource.MaterialResource.Domain.StandardUnit PriceUnit = null;
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Name", strPriceUnit));
                    IList lists = model.ContractExcuteSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq);
                    if (lists != null && lists.Count > 0)
                    {
                        PriceUnit = lists[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                    }
                    curBillLabor.PriceUnit = PriceUnit;
                    curBillLabor.PriceUnitName = strPriceUnit;
                    curBillLabor.ProjectAmountUnit = var.Cells[colProjectAmountUnit.Name].Tag as StandardUnit;//工程量单位GUID
                    curBillLabor.ProjectAmountName = ClientUtil.ToString(var.Cells[colProjectAmountUnit.Name].Value);//工程量单位
                    curBillLabor.SubConProject = curBillMaster;
                    curBillMaster.AddLaborDetail(curBillLabor);
                    var.Tag = curBillLabor;
                }
                foreach (DataGridViewRow var in this.dgProfessDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    ProfessionalSubcontractPriceItem curBillProfess = new ProfessionalSubcontractPriceItem();
                    if (var.Tag != null)
                    {
                        curBillProfess = var.Tag as ProfessionalSubcontractPriceItem;
                        if (curBillProfess.Id == null)
                        {
                            curBillMaster.ProfessDetails.Remove(curBillProfess);
                        }
                    }
                    curBillProfess.JobType = ClientUtil.ToString(var.Cells[colJobType.Name].Value);
                    curBillProfess.JobContent = ClientUtil.ToString(var.Cells[colJobContent.Name].Value);//工作内容
                    curBillProfess.ProvisionalPrice = ClientUtil.ToDecimal(var.Cells[colPrice.Name].Value);//暂定单价
                    curBillProfess.ProvisionalTotalPrice = ClientUtil.ToDecimal(var.Cells[colTotalPrice.Name].Value);//暂定总价
                    curBillProfess.ProjectAmount = ClientUtil.ToDecimal(var.Cells[colProjectAmount.Name].Value);//工程量
                    curBillProfess.ProjectAmountUnit = var.Cells[colProjectAmountName.Name].Tag as StandardUnit;//工程量单位GUID
                    curBillProfess.ProjectAmountName = ClientUtil.ToString(var.Cells[colProjectAmountName.Name].Value);//工程量单位
                    string strPriceUnit = "元";
                    Application.Resource.MaterialResource.Domain.StandardUnit PriceUnit = null;
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Name", strPriceUnit));
                    IList lists = model.ContractExcuteSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq);
                    if (lists != null && lists.Count > 0)
                    {
                        PriceUnit = lists[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                    }
                    curBillProfess.PriceUnit = PriceUnit;
                    curBillProfess.PriceUnitName = strPriceUnit;
                    curBillProfess.ProductModel = ClientUtil.ToString(var.Cells[colProductModel.Name].Value);//规格型号
                    curBillProfess.QualityLevel = ClientUtil.ToString(var.Cells[colQualityLevel.Name].Value);//质量标准
                    curBillProfess.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);//备注
                    curBillProfess.SubConProject = curBillMaster;
                    curBillMaster.AddProDetail(curBillProfess);
                    var.Tag = curBillProfess;
                }
                //curBillMaster.ContractInterimMoney = ClientUtil.ToDecimal(txtMainMoney.Text) + sumMoney;//暂定金额
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
        /// 物料编码列增加事件监听，支持处理键盘回车查询操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox textBox = e.Control as TextBox;
            if (textBox != null)
            {
                textBox.PreviewKeyDown -= new PreviewKeyDownEventHandler(textBox_PreviewKeyDown);
                textBox.KeyPress -= new KeyPressEventHandler(textBox_KeyPress);

                if (this.dgDetail.Columns[this.dgDetail.CurrentCell.ColumnIndex].Name.Equals("MaterialCode"))
                {
                    textBox.PreviewKeyDown += new PreviewKeyDownEventHandler(textBox_PreviewKeyDown);
                    textBox.KeyPress += new KeyPressEventHandler(textBox_KeyPress);
                }

            }
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
            //        this.dgDetail.CurrentRow.Cells[colMaterialCode.Name].Tag = selectedMaterial;
            //        this.dgDetail.CurrentCell.Value = selectedMaterial.Code;
            //        this.dgDetail.CurrentRow.Cells[colMaterialName.Name].Value = selectedMaterial.Name;
            //        this.dgDetail.CurrentRow.Cells[colMaterialSpec.Name].Value = selectedMaterial.Specification;
            //        this.dgDetail.CurrentRow.Cells[colStuff.Name].Value = selectedMaterial.Stuff;

            //        //动态分类复合单位                    
            //        DataGridViewComboBoxCell cbo = this.dgDetail.CurrentRow.Cells[colUnit.Name] as DataGridViewComboBoxCell;
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

        /// <summary>
        /// 在物料编码列，敲击键盘时，取消原来已经选择的物料，
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.dgDetail.CurrentCell.Tag = null;
        }

        /// <summary>
        /// 物料编码列，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colConstractName.Name))
            {
                if (this.dgDetail.EditMode == DataGridViewEditMode.EditOnEnter)
                {
                    VSelectWBSContractGroup vmros = new VSelectWBSContractGroup(new MWBSContractGroup());
                    vmros.ShowDialog();
                    IList list = vmros.SelectResult;
                    if (list == null || list.Count == 0) return;
                    ContractGroup engineerMaster = list[0] as ContractGroup;
                    if (dgDetail.CurrentRow.Cells[colConstractType.Name].Value != null || dgDetail.CurrentRow.Cells[colConstractName.Name].Value != null || dgDetail.CurrentRow.Cells[colPriceUnit.Name].Value != null || dgDetail.CurrentRow.Cells[colUpdateMoney.Name].Value != null)
                    {
                        this.dgDetail.CurrentRow.Cells[colConstractName.Name].Value = engineerMaster.ContractName;
                        this.dgDetail.CurrentRow.Cells[colConstractType.Name].Value = engineerMaster.ContractGroupType;
                        this.dgDetail.CurrentRow.Cells[colConstractName.Name].Tag = engineerMaster;

                        string strUnit = "元";
                        Application.Resource.MaterialResource.Domain.StandardUnit Unit = null;
                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("Name", strUnit));
                        IList lists = model.ContractExcuteSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq);
                        if (lists != null && lists.Count > 0)
                        {
                            Unit = lists[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                        }
                        this.dgDetail.CurrentRow.Cells[colPriceUnit.Name].Tag = Unit;
                        this.dgDetail.CurrentRow.Cells[colPriceUnit.Name].Value = Unit.Name;
                    }
                    else
                    {
                        int i = dgDetail.Rows.Add();
                        this.dgDetail[colConstractName.Name,i].Value = engineerMaster.ContractName;
                        this.dgDetail[colConstractType.Name,i].Value = engineerMaster.ContractGroupType;
                        this.dgDetail[colConstractName.Name,i].Tag = engineerMaster;

                        string strUnit = "元";
                        Application.Resource.MaterialResource.Domain.StandardUnit Unit = null;
                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("Name", strUnit));
                        IList lists = model.ContractExcuteSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq);
                        if (lists != null && lists.Count > 0)
                        {
                            Unit = lists[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                        }
                        this.dgDetail[colPriceUnit.Name,i].Tag = Unit;
                        this.dgDetail[colPriceUnit.Name,i].Value = Unit.Name;
                        i++;
                    }
                    this.txtCode.Focus();
                }
            }

            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colPriceUnit.Name))
            {
                if (this.dgDetail.EditMode == DataGridViewEditMode.EditOnEnter)
                {
                    StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                    if (su != null)
                    {
                        this.dgDetail.CurrentRow.Cells[colPriceUnit.Name].Tag = su;
                        this.dgDetail.CurrentRow.Cells[colPriceUnit.Name].Value = su.Name;
                        this.txtCode.Focus();
                    }
                    this.txtCode.Focus();
                }
            }

        }

        /// <summary>
        /// 计量单位，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgLaborDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgLaborDetail.Columns[e.ColumnIndex].Name.Equals(colProjectAmountUnit.Name))
            {              
                StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                if (su != null)
                {
                    this.dgLaborDetail.CurrentRow.Cells[colProjectAmountUnit.Name].Tag = su;
                    this.dgLaborDetail.CurrentRow.Cells[colProjectAmountUnit.Name].Value = su.Name;
                    this.txtCode.Focus();
                }
                this.txtCode.Focus();
            }
        }

        /// <summary>
        /// 计量单位，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgProfessDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgProfessDetail.Columns[e.ColumnIndex].Name.Equals(colProjectAmountName.Name))
            { 
                StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                if (su != null)
                {
                    this.dgProfessDetail.CurrentRow.Cells[colProjectAmountName.Name].Tag = su;
                    this.dgProfessDetail.CurrentRow.Cells[colProjectAmountName.Name].Value = su.Name;
                    this.txtCode.Focus();
                }
                this.txtCode.Focus();
            }
        }

        //显示数据
        private bool ModelToView()
        {
            try
            {
                this.txtCode.Text = curBillMaster.Code;
                txtHandlePerson.Tag = curBillMaster.Owner;
                txtHandlePerson.Text = curBillMaster.OwnerName;
                txtMainMoney.Text = curBillMaster.ContractSumMoney.ToString();
                txtSumSettleMoney.Text = curBillMaster.AddupBalanceMoney.ToString();
                cmobalanceStyle.Text = curBillMaster.BalanceStyle;
                txtsubpackage.Text = curBillMaster.SubPackage;
                txtProject.Text = curBillMaster.ProjectName;
                txtConstract.Text = ClientUtil.ToString(curBillMaster.ContractGroupCode);
                txtConstract.Tag = curBillMaster.TheContractGroup;
                txtConstructRate.Text = ClientUtil.ToString(curBillMaster.ManagementRate * 100);
                txtPercentage.Text = ClientUtil.ToString(curBillMaster.AllowExceedPercent * 100);
                txtLaobrRace.Text = ClientUtil.ToString(curBillMaster.LaobrRace * 100);
                dtCreateDate.Text = curBillMaster.CreateDate.ToShortDateString();

                txtProcessPayRate.Text = ClientUtil.ToString(curBillMaster.ProcessPayRate * 100);
                txtCompletePayRate.Text = ClientUtil.ToString(curBillMaster.CompletePayRate * 100);
                txtWarrantyPayRate.Text = ClientUtil.ToString(curBillMaster.WarrantyPayRate * 100);

                if (curBillMaster.BearerOrg != null)
                {
                    txtSupply.Result.Clear();
                    txtSupply.Tag = curBillMaster.BearerOrg;
                    txtSupply.Result.Add(curBillMaster.BearerOrg);
                    txtSupply.Value = curBillMaster.BearerOrgName;
                }

                txtUtilitiesRate.Text = ClientUtil.ToString(curBillMaster.UtilitiesRate * 100);
                //comUtilities.SelectedValue = curBillMaster.UtilitiesRemMethod;
                comUtilities.SelectedItem = EnumUtil<UtilitiesRememberMethod>.GetDescription(curBillMaster.UtilitiesRemMethod);
                if (comUtilities.Text.ToString() == "按费率计取")
                {
                    txtUtilitiesRate.ReadOnly = false;
                }
                else
                {
                    txtUtilitiesRate.ReadOnly = true;
                }
                comConstractType.SelectedItem = EnumUtil<SubContractType>.GetDescription(curBillMaster.ContractType);
                txtLaborMoneyType.SelectedItem = EnumUtil<ManagementRememberMethod>.GetDescription(curBillMaster.LaborMoneyType);
                if (txtLaborMoneyType.Text.ToString() == "按费率计取")
                {
                    txtLaobrRace.ReadOnly = false;
                }
                else
                {
                    txtLaobrRace.ReadOnly = true;
                }
                comConstruct.SelectedItem = EnumUtil<ManagementRememberMethod>.GetDescription(curBillMaster.ManagementRemMethod);
                if (comConstruct.Text.ToString() == "按费率计取")
                {
                    txtConstructRate.ReadOnly = false;
                }
                else
                {
                    txtConstructRate.ReadOnly = true;
                }
               
                this.dgDetail.Rows.Clear();
                //IList ConstractDetail = model.ContractExcuteSrv.GetContractDetailById(curBillMaster.Id);
                //foreach (SubContractChangeItem var in ConstractDetail)

                foreach (SubContractChangeItem var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();
                    this.dgDetail[colConstractName.Name, i].Value = var.ContractName;
                    this.dgDetail[colConstractType.Name, i].Value = var.ContractType;
                    this.dgDetail[colConstractName.Name, i].Tag = var.TheContractGroup;
                    this.dgDetail[colPriceUnit.Name, i].Value = var.PriceUnitName;
                    this.dgDetail[colPriceUnit.Name, i].Tag = var.PriceUnit;
                    this.dgDetail[colUpdateDescript.Name, i].Value = var.ChangeDesc;
                    this.dgDetail[colUpdateMoney.Name, i].Value = var.ChangeMoney;
                    this.dgDetail.Rows[i].Tag = var;
                }

                this.dgLaborDetail.Rows.Clear();
                foreach (LaborSubContractPriceItem var in curBillMaster.LaborDetails)
                {
                    int i = this.dgLaborDetail.Rows.Add();
                    this.dgLaborDetail[colWorkContent.Name,i].Value = var.JobContent;
                    this.dgLaborDetail[colConsType.Name,i].Value = var.ConstractType;
                    this.dgLaborDetail[colUnitPrice.Name,i].Value = var.UnitPrice;
                    this.dgLaborDetail[colProjectAmountUnit.Name, i].Value = var.ProjectAmountName;
                    this.dgLaborDetail[colRemark.Name,i].Value = var.Descript;
                    this.dgLaborDetail.Rows[i].Tag = var;
                }

                this.dgProfessDetail.Rows.Clear();
                foreach (ProfessionalSubcontractPriceItem var in curBillMaster.ProfessDetails)
                {
                    int i = this.dgProfessDetail.Rows.Add();
                    this.dgProfessDetail[colJobType.Name, i].Value = var.JobType;
                    this.dgProfessDetail[colJobContent.Name, i].Value = var.JobContent;
                    this.dgProfessDetail[colPrice.Name, i].Value = var.ProvisionalPrice;
                    this.dgProfessDetail[colTotalPrice.Name, i].Value = var.ProvisionalTotalPrice;
                    this.dgProfessDetail[colProjectAmount.Name, i].Value = var.ProjectAmount;
                    this.dgProfessDetail[colProjectAmountName.Name, i].Value = var.ProjectAmountName;
                    this.dgProfessDetail[colProductModel.Name, i].Value = var.ProductModel;
                    this.dgProfessDetail[colQualityLevel.Name, i].Value = var.QualityLevel;
                    this.dgProfessDetail[colDescript.Name, i].Value = var.Descript;
                    this.dgProfessDetail.Rows[i].Tag = var;
                }
                FillDoc();
                txtSumMoney.Text = curBillMaster.ContractInterimMoney.ToString();
                return true;
            }
            catch (Exception e)
            {
                throw e;
                //MessageBox.Show("数据映射错误：" + StaticMethod.ExceptionMessage(e));
                //return false;
            }
        }

        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            bool flag = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            if (colName == colUpdateMoney.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colUpdateMoney.Name].Value != null)
                {
                    string temp_price = dgDetail.Rows[e.RowIndex].Cells[colUpdateMoney.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_price);
                    if (validity == false)
                    {
                        MessageBox.Show("变更金额为数字！");
                        this.dgDetail.Rows[e.RowIndex].Cells[colUpdateMoney.Name].Value = "";
                        flag = false;
                    }
                }
                if (flag)
                {
                    decimal summoney = 0;
                    for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                    {
                        object money = dgDetail.Rows[i].Cells[colUpdateMoney.Name].Value;
                        summoney = summoney + ClientUtil.TransToDecimal(money);
                    }
                    txtSumMoney.Text = (summoney + ClientUtil.ToDecimal(txtMainMoney.Text)).ToString();
                }
            }
        }

        private void dgLaborDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            string colName = dgLaborDetail.Columns[e.ColumnIndex].Name;
            if (colName == colUnitPrice.Name)
            {
                if (dgLaborDetail.Rows[e.RowIndex].Cells[colUnitPrice.Name].Value != null)
                {
                    string temp_quantity = dgLaborDetail.Rows[e.RowIndex].Cells[colUnitPrice.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        dgLaborDetail.Rows[e.RowIndex].Cells[colUnitPrice.Name].Value = "";
                    }
                }
                object quantity = dgLaborDetail.Rows[e.RowIndex].Cells[colUnitPrice.Name].Value;
            }
        }

        private void dgProfessDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            string colName = dgProfessDetail.Columns[e.ColumnIndex].Name;
            if (colName == colPrice.Name)
            {
                if (dgProfessDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value != null)
                {
                    string temp_quantity = dgProfessDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        dgProfessDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value = "";
                    }
                }
                object quantity = dgProfessDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value;
            }
            if (colName == colTotalPrice.Name)
            {
                if (dgProfessDetail.Rows[e.RowIndex].Cells[colTotalPrice.Name].Value != null)
                {
                    string temp_quantity = dgProfessDetail.Rows[e.RowIndex].Cells[colTotalPrice.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        dgProfessDetail.Rows[e.RowIndex].Cells[colTotalPrice.Name].Value = "";
                    }
                }
                object quantity = dgProfessDetail.Rows[e.RowIndex].Cells[colTotalPrice.Name].Value;
            }
            if (colName == colProjectAmount.Name)
            {
                if (dgProfessDetail.Rows[e.RowIndex].Cells[colProjectAmount.Name].Value != null)
                {
                    string temp_quantity = dgProfessDetail.Rows[e.RowIndex].Cells[colProjectAmount.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        dgProfessDetail.Rows[e.RowIndex].Cells[colProjectAmount.Name].Value = "";
                    }
                }
                object quantity = dgProfessDetail.Rows[e.RowIndex].Cells[colProjectAmount.Name].Value;
            }
        }

        ///// <summary>
        ///// 打印预览
        ///// </summary>
        ///// <returns></returns>
        //public override bool Preview()
        //{
        //    if (LoadTempleteFile(@"物资需求总计划.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.PrintPreview(true,true,true,0,0,0,0,0);
        //    return true;
        //}

        //public override bool Print()
        //{
        //    if (LoadTempleteFile(@"物资需求总计划.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.Print();
        //    return true;
        //}

        //public override bool Export()
        //{
        //    if (LoadTempleteFile(@"物资需求总计划.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.ExportToExcel("物资需求总计划【" + curBillMaster.Code + "】", false, false, true);
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

        //private void FillFlex(DemandMasterPlanMaster billMaster)
        //{
        //    int detailStartRowNumber = 5;//5为模板中的行号
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
        //    flexGrid1.Cell(3, 2).Text = billMaster.Code;
        //    flexGrid1.Cell(3, 4).Text = billMaster.MaterialCategoryName;
        //    flexGrid1.Cell(3, 6).Text = billMaster.RealOperationDate.ToShortDateString();
        //    flexGrid1.Cell(3, 7).Text = "编制依据：" + billMaster.Compilation;

        //    FlexCell.PageSetup pageSetup = flexGrid1.PageSetup;
        //    pageSetup.RightFooter = "第 &P 页/共 &N 页      ";

        //    System.Drawing.Printing.PaperSize paperSize = new System.Drawing.Printing.PaperSize("自定义纸张", 910, 470);
        //    pageSetup.PaperSize = paperSize;

        //    //填写明细数据
        //    decimal sumQuantity = 0;
        //    for (int i = 0; i < detailCount; i++)
        //    {
        //        DemandMasterPlanDetail detail = (DemandMasterPlanDetail)billMaster.Details.ElementAt(i);
        //        //物资名称
        //        flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialResource.Name;

        //        //规格型号
        //        flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialResource.Specification;

        //        //计量单位
        //        flexGrid1.Cell(detailStartRowNumber + i, 3).Text = detail.MatStandardUnitName;

        //        //需用计划
        //        flexGrid1.Cell(detailStartRowNumber + i, 4).Text = ClientUtil.ToString(detail.Quantity);
        //        sumQuantity += detail.Quantity;

        //        //质量标准
        //        flexGrid1.Cell(detailStartRowNumber + i, 5).Text = ClientUtil.ToString(detail.QualityStandard);

        //        //生产厂家
        //        flexGrid1.Cell(detailStartRowNumber + i, 6).Text = ClientUtil.ToString(detail.Manufacturer);

        //        //备注
        //        flexGrid1.Cell(detailStartRowNumber + i, 7).Text = ClientUtil.ToString(detail.Descript);
        //        if (i == detailCount - 1)
        //        {
        //            flexGrid1.Cell(detailStartRowNumber + i + 1, 4).Text = ClientUtil.ToString(sumQuantity);
        //        }
        //    }
        //    flexGrid1.Cell(6 + detailCount, 2).Text = billMaster.ProjectName;
        //    flexGrid1.Cell(6 + detailCount, 5).Text = billMaster.CreateDate.ToShortDateString();
        //    flexGrid1.Cell(6 + detailCount, 7).Text = billMaster.CreatePersonName;
        //}
    }
}
