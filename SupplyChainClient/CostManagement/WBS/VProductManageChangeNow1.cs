using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Linq;

using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;

using Application.Business.Erp.ResourceManager.Client.Main;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonForm;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
//using VirtualMachine.Core.Expression;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using System.Windows.Documents;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng;

using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using ImportIntegration;
using System.IO;
using System.Diagnostics;
using NHibernate.Criterion;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using IRPServiceModel.Domain.Document;
using Application.Business.Erp.SupplyChain.Client.FileUpload;
using IRPServiceModel.Basic;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS
{
    public partial class VProductManageChangeNow1 : TBasicDataView
    {
        private TreeNode currNode = null;

        private GWBSTree oprNode = null;

        //有权限的业务组织
        private IList lstInstance;
        //唯一编码
        private string uniqueCode;

        private Hashtable hashtableRules = new Hashtable();
        private List<TreeNode> listFindNodes = new List<TreeNode>();
        private int showFindNodeIndex = 0;
        private TreeNode mouseSelectNode = null;

        Hashtable SpecialHt = new Hashtable();

        private bool isDistribute = true;
        public bool IsDistribute
        {
            get { return isDistribute; }
            set { isDistribute = value; }
        }

        private Dictionary<string, TreeNode> listCheckedNode = new Dictionary<string, TreeNode>();

        DateTime serverTime = new DateTime();
        GWBSTaskConfirm confirm = new GWBSTaskConfirm();
        GWBSDetail SavedGWBSDetail = null;
        GWBSTaskConfirmMaster conMaster = new GWBSTaskConfirmMaster();

        /// <summary>
        /// 复制的顶级节点集合
        /// </summary>
        private List<TreeNode> listCopyNode = new List<TreeNode>();
        /// <summary>
        /// 复制的所有子节点集合，用于清除选择的节点时还能找到复制的节点
        /// </summary>
        Dictionary<string, TreeNode> listCopyNodeAll = new Dictionary<string, TreeNode>();

        CurrentProjectInfo projectInfo = null;

        /// <summary>
        /// 是否是插入的节点
        /// </summary>
        private bool IsInsertNode = false;

        /// <summary>
        /// 是否是提交(还有保存)
        /// </summary>
        private bool IsSubmit = false;

        #region 文档操作变量

        private ProObjectRelaDocument oprDocument = null;

        string fileObjectType = string.Empty;
        string FileStructureType = string.Empty;

        string userName = string.Empty;
        string jobId = string.Empty;

        #endregion 文档操作

        public MGWBSTree model;


        string filePath = string.Empty;
        string objecIsGWBS = string.Empty;
        string addOrUpDate = string.Empty;
        //CurrentProjectInfo projectInfo = new CurrentProjectInfo();


        public VProductManageChangeNow1(MGWBSTree mot)
        {
            model = mot;
            InitializeComponent();
            InitForm();
            Loaddgdetail();
        }

        private void InitForm()
        {
            try
            {



                VBasicDataOptr.InitProfessionCategory(cbSpecialtyClassify, false);
                txtNoDtlColorFlag.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                //  colMaterialSinge.DataSource = EnumUtil<EnumMaterialFeeSettlementFlag>.GetDescriptions();
                txtStartDate.Value = DateTime.Now.AddMonths(-1);
                tvwCategory.CheckBoxes = false;
                projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                //查询工程任务确认单
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                //oq.AddCriterion(Expression.Eq("OperOrgInfo", ConstObject.TheLogin.TheOperationOrgInfo));
                oq.AddCriterion(Expression.Eq("BillType", EnumConfirmBillType.虚拟工单));
                IList temp_list = model.ObjectQuery(typeof(GWBSTaskConfirmMaster), oq);
                if (temp_list.Count == 0)
                {
                    conMaster = new GWBSTaskConfirmMaster();
                    conMaster.ConfirmHandlePerson = ConstObject.LoginPersonInfo;
                    conMaster.ConfirmHandlePersonName = ConstObject.LoginPersonInfo.Name;
                    conMaster.OperOrgInfo = ConstObject.TheOperationOrg;
                    conMaster.OperOrgInfoName = ConstObject.TheOperationOrg.Name;
                    conMaster.OpgSysCode = ConstObject.TheOperationOrg.SysCode;
                    conMaster.OpgSysCode = ConstObject.TheOperationOrg.SysCode;
                    conMaster.DocState = DocumentState.Edit;
                    conMaster.ProjectId = projectInfo.Id;
                    conMaster.ProjectName = projectInfo.Name;
                    conMaster.ConfirmDate = DateTime.Now;
                    conMaster.CreatePerson = ConstObject.LoginPersonInfo;//制单人编号
                    conMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                    conMaster.CreateYear = ConstObject.LoginDate.Year;//制单年
                    conMaster.CreateMonth = ConstObject.LoginDate.Month;//制单月
                    conMaster.CreateDate = ConstObject.LoginDate;
                    conMaster.RealOperationDate = ConstObject.LoginDate;
                    conMaster.BillType = EnumConfirmBillType.虚拟工单;
                    conMaster = model.SaveGWBSTaskConfirmMaster(conMaster);
                }
                else
                {
                    conMaster = temp_list[0] as GWBSTaskConfirmMaster;
                }
                InitEvents();
                cbCheckConclusion.Items.AddRange(new object[] { "通过", "不通过" });
                cbWBSCheckRequir.Items.Clear();
                IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.WBS_CheckRequire);
                foreach (BasicDataOptr b in list)
                {
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                    li.Text = b.BasicName;
                    li.Value = b.BasicCode;
                    cbWBSCheckRequir.Items.Add(li);
                }
                this.cbWBSCheckRequir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                this.cbCheckConclusion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                cbCheckConclusion1.Items.AddRange(new object[] { "通过", "不通过" });
                cbWBSCheckRequir1.Items.Clear();
                foreach (BasicDataOptr b in list)
                {
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                    li.Text = b.BasicName;
                    li.Value = b.BasicCode;
                    cbWBSCheckRequir1.Items.Add(li);
                }

                //检查要求
                IList lst = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetBasicDataByName(VBasicDataOptr.WBS_CheckRequire);
                if (lst != null)
                {
                    foreach (BasicDataOptr bdo in lst)
                    {
                        cbListCheckRequire.Items.Add(bdo.BasicName);
                    }
                }
                foreach (string flag in Enum.GetNames(typeof(OverOrUnderGroundFlagEnum)))
                {
                    cbOverOrUnderGroundFlag.Items.Add(flag);
                }
                //serverTime = model.GetServerTime();
                //dtStartTime.Value = serverTime.Date;
                //dtEndTime.Value = serverTime.Date.AddDays(7);
                //realStartDate.Value = serverTime.Date;
                //realEndDate.Value = serverTime.Date.AddDays(7);

                dtStartTime.Text = "";
                dtEndTime.Text = "";
                realStartDate.Text = "";
                realEndDate.Text = "";

                if (tvwCategory.Nodes.Count == 0)
                    RefreshControls(MainViewState.Initialize);
                else
                    RefreshControls(MainViewState.Browser);
                if (IsDistribute)
                {
                    NewLoadGWBSTreeTree();
                }
                else
                {
                    LoadGWBSTreeTree();
                }
                List<string> listParams = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetUploadFileParamsByMBP_IRP();
                fileObjectType = listParams[0];
                FileStructureType = listParams[1];
                userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
                jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
                //生产检查报表
                this.txtInspectionType.Items.AddRange(new object[] { "日常检查", "质量验收" });
                this.txtInspectionCnclusion.Items.AddRange(new object[] { "通过", "不通过" });
                this.txtPrintSign.Items.AddRange(new object[] { "已打印", "未打印" });
                this.txtRectReq.Items.AddRange(new object[] { "不需整改", "需要整改，未进行处理", "需要整改，已进行处理" });
                txtInspectionSpecial.Items.Clear();
                foreach (BasicDataOptr b in list)
                {
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                    li.Text = b.BasicName;
                    li.Value = b.BasicCode;
                    txtInspectionSpecial.Items.Add(li);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("初始化失败，详细信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                dgDocumentDetail.ReadOnly = false;
                FileSelect.ReadOnly = false;


            }
        }

        private void InitEvents()
        {
            tvwCategory.MouseDown += new MouseEventHandler(tvwCategory_MouseDown);//处理没有根节点的情况
            tvwCategory.AfterCheck += new TreeViewEventHandler(tvwCategory_AfterCheck);
            tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);
            tvwCategory.AfterExpand += new TreeViewEventHandler(tvwCategory_AfterExpand);
            btnSelectWBSType.Click += new EventHandler(btnSelectWBSType_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            //btnRemovePBS.Click += new EventHandler(btnRemovePBS_Click);
            this.dgDetail.CellClick += new DataGridViewCellEventHandler(dgDetail_CellClick);
            this.btnSaveInspectionRecord.Click += new EventHandler(btnSaveInspectionRecord_Click);
            this.btnSave1.Click += new EventHandler(btnSave1_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            this.btnDelete1.Click += new EventHandler(btnDelete1_Click);
            this.btnSubmit.Click += new EventHandler(btnSubmit_Click);
            this.btnSubmit1.Click += new EventHandler(btnSubmit1_Click);
            this.dgDetail.MouseClick += new MouseEventHandler(dgDetail_MouseClick);
            this.conMenu.ItemClicked += new ToolStripItemClickedEventHandler(conMenu_ItemClicked);
            this.cbCheckConclusion.SelectedIndexChanged += new EventHandler(cbCheckConclusion_SelectedIndexChanged);

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

            //生产检查报表
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnPrintPreview.Click += new EventHandler(btnPrintPreview_Click);
            this.btnPrint.Click += new EventHandler(btnPrint_Click);
            lnkAll.Click += new EventHandler(lnkAll_Click);
            lnkNone.Click += new EventHandler(lnkNone_Click);
            this.dgSearch.CellDoubleClick += new DataGridViewCellEventHandler(dgSearch_CellDoubleClick);
            //工程量提报
            // this.btnSubmitProject.Click += new EventHandler(btnSubmitProject_Click);
            // this.dgBear.CellEndEdit += new DataGridViewCellEventHandler(dgBear_CellEndEdit);
            //this.dgBear.CellValueChanged += new DataGridViewCellEventHandler(dgBear_CellValueChanged);
            //this.dgBear.SelectionChanged += new EventHandler(dgBear_SelectionChanged);
            //this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            // this.dgProject.SelectionChanged += new EventHandler(dgProject_SelectionChanged);
            // this.btnDeleteProject.Click += new EventHandler(btnDeleteProject_Click);
            //this.btnProject.Click += new EventHandler(btnProject_Click);
            this.btnSelect.Click += new EventHandler(btnSelect_Click);
            this.btnSearch1.Click += new EventHandler(btnSearch1_Click);
            this.btnProjectQuery.Click += new EventHandler(btnQueryProject_Click);
            // dgBear.CellDoubleClick += new DataGridViewCellEventHandler(dgBear_CellDoubleClick);
            dtStartTime.Leave += new EventHandler(dtStartTime_Leave);
            dtEndTime.Leave += new EventHandler(dtEndTime_Leave);
            realStartDate.Leave += new EventHandler(realStartDate_CloseUp);
            realEndDate.Leave += new EventHandler(realEndDate_CloseUp);
            this.btnSupplySearch1.Click += new EventHandler(btnSupplySearch1_Click);
            //tvwCategory.AfterCollapse += new TreeViewEventHandler(tvwCategory_AfterCollapse);
            //tvwCategory.BeforeExpand += new TreeViewCancelEventHandler(tvwCategory_BeforeExpand);
            txtKeyWord.KeyDown += new KeyEventHandler(txtKeyWord_KeyDown);
            txtKeyWord.TextChanged += new EventHandler(txtKeyWord_TextChanged);
            btnFindTaskNode.Click += new EventHandler(btnFindTaskNode_Click);
            // btnCHDZH.Click += new EventHandler(btnCHDZH_Click);
            this.btnProjectBatchSave.Click += new EventHandler(btnProjectBatchSave_Click);
            this.btnProjectBatchSet.Click += new EventHandler(btnProjectBatchSet_Click);
            this.chkProjectAll.CheckedChanged += new EventHandler(chkProjectAll_CheckedChanged);
            this.chkProjectNot.CheckedChanged += new EventHandler(chkProjectNot_CheckedChanged);
            this.dgProject.CellEndEdit += new DataGridViewCellEventHandler(dgProject_CellEndEdit);
            // this.dgProject.CellValueChanged += new DataGridViewCellEventHandler(dgProject_CellValueChanged);
            btnSelectContract.Click += new EventHandler(btnSelectContract_Click);
            //tab页切换数据处理
            tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
        }
        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == 检查记录明细信息.Name)//如果是明细
            {

            }
            else if (tabControl1.SelectedTab.Name == tabPage2.Name)//质量验收信息
            {

            }
            else if (tabControl1.SelectedTab.Name == 相关附件.Name)//相关文档
            {
                //if (curBillMaster != null && !string.IsNullOrEmpty(curBillMaster.Id))
                {
                    FillDoc();
                }
            }
        }

        void cbCheckConclusion_SelectedIndexChanged(object sendr, EventArgs e)
        {
            if (cbCheckConclusion.SelectedItem.ToString() == "通过")
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
            else
            {
                radioButton1.Checked = false;
                radioButton2.Checked = true;
            }

        }

        //void dgProject_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        //{
        //    try
        //    {
        //        //this.dgProject.CellValueChanged -= new DataGridViewCellEventHandler(dgProject_CellValueChanged);
        //        string colName = dgProject.Columns[e.ColumnIndex].Name;
        //        if (colName == colQuantityBC.Name)
        //        {
        //            //本次提报
        //            this.dgProject.CurrentRow.Cells[colGZHTBL.Name].Value = ClientUtil.ToString(ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colGZHTBL.Name].Value) + ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colQuantityBC.Name].Value) + ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colQuantityBefore.Name].Value));
        //            //this.dgProject.CurrentRow.Cells[colXXJDBefore.Name].Value = ClientUtil.ToString(ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colQuantityBefore.Name].Value) / ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) * 100);
        //            this.dgProject.CurrentRow.Cells[colBCXXJD.Name].Value = (ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colQuantityBC.Name].Value) / ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) * 100).ToString("0.00");
        //            this.dgProject.CurrentRow.Cells[colBCTotalXXJD.Name].Value = (ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colBCXXJD.Name].Value) + ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colXXJDBefore.Name].Value)).ToString("0.00");
        //        }
        //        if (colName == colGZHTBL.Name)
        //        {
        //            //工长累计提报工程量
        //            //this.dgProject.CurrentRow.Cells[colQuantityBC.Name].Value = ClientUtil.ToString(ClientUtil.ToDecimal(this.dgProject.CurrentRow.Cells[colGZHTBL.Name].Value) - ClientUtil.ToDecimal(this.dgProject.CurrentRow.Cells[colQuantityBefore.Name].Value));
        //            //this.dgProject.CurrentRow.Cells[colXXJDBefore.Name].Value = ClientUtil.ToString(ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colGZHTBL.Name].Value) / ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) * 100);
        //            this.dgProject.CurrentRow.Cells[colBCXXJD.Name].Value = ((ClientUtil.ToDecimal(this.dgProject.CurrentRow.Cells[colGZHTBL.Name].Value) - ClientUtil.ToDecimal(this.dgProject.CurrentRow.Cells[colQuantityBefore.Name].Value)) / ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) * 100).ToString("0.00");
        //            this.dgProject.CurrentRow.Cells[colBCTotalXXJD.Name].Value = (ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colBCXXJD.Name].Value) + ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colXXJDBefore.Name].Value)).ToString("0.00");
        //        }
        //        if (colName == colBCXXJD.Name)
        //        {
        //            //本次提报形象进度
        //            //this.dgProject.CurrentRow.Cells[colQuantityBC.Name].Value 
        //            decimal num = ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colBCXXJD.Name].Value) / 100 * ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value);
        //            this.dgProject.CurrentRow.Cells[colGZHTBL.Name].Value = ClientUtil.ToString(num + ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colQuantityBefore.Name].Value));
        //            //this.dgProject.CurrentRow.Cells[colXXJDBefore.Name].Value = ClientUtil.ToString(ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colQuantityBefore.Name].Value) / ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value)*100);
        //            this.dgProject.CurrentRow.Cells[colBCTotalXXJD.Name].Value = (ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colBCXXJD.Name].Value) + ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colXXJDBefore.Name].Value)).ToString("0.00");
        //        }
        //        if (colName == colBCTotalXXJD.Name)
        //        {
        //            //本次累计提报形象进度
        //            this.dgProject.CurrentRow.Cells[colBCXXJD.Name].Value = (ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colBCTotalXXJD.Name].Value) - ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colXXJDBefore.Name].Value)).ToString("0.00");
        //            //this.dgProject.CurrentRow.Cells[colQuantityBC.Name].Value = (ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) * ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colBCXXJD.Name].Value) / 100).ToString("0.00");
        //            decimal num = ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) * ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colBCXXJD.Name].Value) / (decimal)100;
        //            this.dgProject.CurrentRow.Cells[colGZHTBL.Name].Value = (num + ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colQuantityBefore.Name].Value)).ToString("0.00");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        //this.dgProject.CellValueChanged += new DataGridViewCellEventHandler(dgProject_CellValueChanged);
        //    }
        //}

        void txtKeyWord_TextChanged(object sender, EventArgs e)
        {
            listFindNodes.Clear();
            showFindNodeIndex = 0;
        }





        void btnSupplySearch1_Click(object sender, EventArgs e)
        {
            DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
            VContractExcuteSelector vmros = new VContractExcuteSelector();
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject engineerMaster = list[0] as SubContractProject;
            txtBearTeam.Text = engineerMaster.BearerOrgName;
            txtBearTeam.Tag = engineerMaster;
        }


        void dtStartTime_Leave(object sender, EventArgs e)
        {
            int planDuration = 0;
            if (dtStartTime.IsHasValue && dtEndTime.IsHasValue)
            {
                if (dtStartTime.Value > dtEndTime.Value)
                {
                    MessageBox.Show("计划开始时间不能晚于计划结束时间!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //realStartDate.Focus();
                }
                else
                {
                    planDuration = (dtEndTime.Value - dtStartTime.Value).Days + 1;
                }
            }
            txtPlanTime.Text = planDuration.ToString();
        }

        void dtEndTime_Leave(object sender, EventArgs e)
        {
            int planDuration = 0;
            if (dtStartTime.IsHasValue && dtEndTime.IsHasValue)
            {
                if (dtStartTime.Value > dtEndTime.Value)
                {
                    MessageBox.Show("计划开始时间不能晚于计划结束时间!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //realStartDate.Focus();
                }
                else
                {
                    planDuration = (dtEndTime.Value - dtStartTime.Value).Days + 1;
                }
            }
            txtPlanTime.Text = planDuration.ToString();
        }

        private void realStartDate_CloseUp(object sender, EventArgs e)
        {
            int planDuration = 0;
            if (realStartDate.IsHasValue && realEndDate.IsHasValue)
            {
                if (realStartDate.Value > realEndDate.Value)
                {
                    MessageBox.Show("实际开始时间不能晚于实际结束时间!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //realStartDate.Focus();
                }
                else
                {
                    planDuration = (realEndDate.Value - realStartDate.Value).Days + 1;
                }
            }
            txtRealTime.Text = planDuration.ToString();
        }

        private void realEndDate_CloseUp(object sender, EventArgs e)
        {
            int planDuration = 0;
            if (realStartDate.IsHasValue && realEndDate.IsHasValue)
            {
                if (realStartDate.Value > realEndDate.Value)
                {
                    MessageBox.Show("实际结束时间不能早于实际开始时间", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //realEndDate.Focus();
                }
                else
                {
                    planDuration = (realEndDate.Value - realStartDate.Value).Days + 1;
                }
            }
            txtRealTime.Text = planDuration.ToString();
        }

        void btnSelect_Click(object sender, EventArgs e)
        {
            DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
            VContractExcuteSelector vmros = new VContractExcuteSelector();
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject engineerMaster = list[0] as SubContractProject;
            txtSupply.Text = engineerMaster.BearerOrgName;
            txtSupply.Tag = engineerMaster;
        }
        void btnSearch1_Click(object sender, EventArgs e)
        {
            DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
            VContractExcuteSelector vmros = new VContractExcuteSelector();
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject engineerMaster = list[0] as SubContractProject;
            txtSupply1.Text = engineerMaster.BearerOrgName;
            txtSupply1.Tag = engineerMaster;
        }

        private PBSTree selectRelaPBS = null;

        void tvwCategory_MouseDown(object sender, MouseEventArgs e)
        {
            if (tvwCategory.Nodes.Count == 0 && e.Button == MouseButtons.Right)
            {
                RefreshControls(MainViewState.Initialize);
                mnuTree.Show(tvwCategory, new Point(e.X, e.Y));
            }
        }
        bool isSelectNodeInvoke = false;//是否在选择(点击)节点时调用
        bool startNodeCheckedState = false;//按shift多选兄弟节点时起始节点的选中状态

        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                //this.btnUpdate.Enabled = true;
                ClearIns();
                #region 点击树节点时实现多选
                bool isMultiSelect = false;
                TreeNode preselectionNode;//预选择节点

                preselectionNode = e.Node;

                if (currNode != null && currNode.Name != preselectionNode.Name
                    && currNode.Parent != null && preselectionNode.Parent != null && currNode.Parent.Name == preselectionNode.Parent.Name)//Name取的对象的ID
                    isMultiSelect = true;
                else
                    isMultiSelect = false;

                if (currNode != null)
                    startNodeCheckedState = currNode.Checked;

                if (isMultiSelect && (Control.ModifierKeys & Keys.Shift) != 0 && (Control.ModifierKeys & Keys.Control) != 0)//如果同时按下了ctrl+shift
                {
                    int currNodeIndex = currNode.Index;
                    int preselectNodeIndex = preselectionNode.Index;

                    int startIndex = currNodeIndex < preselectNodeIndex ? currNodeIndex : preselectNodeIndex;
                    int endIndex = currNodeIndex < preselectNodeIndex ? preselectNodeIndex : currNodeIndex;

                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        TreeNode tn = currNode.Parent.Nodes[i];

                        isSelectNodeInvoke = true;//设置标志，在check事件中不再处理

                        if (startNodeCheckedState)//如果起始节点当前为选中，就取消选择
                        {
                            TreeNode tempNode = new TreeNode();
                            tn.BackColor = tempNode.BackColor;
                            tn.ForeColor = tempNode.ForeColor;

                            if (listCheckedNode.ContainsKey(tn.Name))
                                listCheckedNode.Remove(tn.Name);

                            tn.Checked = false;
                        }
                        else//如果起始节点当前为未选中，就设置选择
                        {
                            tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                            tn.ForeColor = ColorTranslator.FromHtml("#000000");

                            if (listCheckedNode.ContainsKey(tn.Name))
                                listCheckedNode[tn.Name] = tn;
                            else
                                listCheckedNode.Add(tn.Name, tn);

                            tn.Checked = true;
                        }

                        SetChildCheckedByMultiSel(tn);
                    }
                }
                else if (isMultiSelect && (Control.ModifierKeys & Keys.Shift) != 0)//如果同时按下了shift
                {


                    int currNodeIndex = currNode.Index;
                    int preselectNodeIndex = preselectionNode.Index;

                    int startIndex = currNodeIndex < preselectNodeIndex ? currNodeIndex : preselectNodeIndex;
                    int endIndex = currNodeIndex < preselectNodeIndex ? preselectNodeIndex : currNodeIndex;

                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        TreeNode tn = currNode.Parent.Nodes[i];

                        isSelectNodeInvoke = true;//设置标志，在check事件中不再处理

                        if (startNodeCheckedState)//如果起始节点当前为选中，就取消选择
                        {
                            tn.Checked = false;

                            TreeNode tempNode = new TreeNode();
                            tn.BackColor = tempNode.BackColor;
                            tn.ForeColor = tempNode.ForeColor;

                            if (listCheckedNode.ContainsKey(tn.Name))
                                listCheckedNode.Remove(tn.Name);
                        }
                        else//如果起始节点当前为未选中，就设置选择
                        {
                            tn.Checked = true;

                            tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                            tn.ForeColor = ColorTranslator.FromHtml("#000000");

                            if (listCheckedNode.ContainsKey(tn.Name))
                                listCheckedNode[tn.Name] = tn;
                            else
                                listCheckedNode.Add(tn.Name, tn);
                        }
                    }
                }
                #endregion
                currNode = tvwCategory.SelectedNode;
                oprNode = tvwCategory.SelectedNode.Tag as GWBSTree;
                oprNode = LoadRelaAttribute(oprNode);
                FilldgDtail(oprNode);
                #region 旧代码
                //if (oprNode.ProductConfirmFlag)
                //{
                //    colProjectDetail.Visible = true;
                //    colUnit.Visible = true;
                //    colDtlState.Visible = true;
                //    //生产节点
                //    FillProject(oprNode);
                //    //realStartDate.Enabled = false;
                //    //realStartDate.Enabled = false;
                //}
                //else
                //{
                //    //realStartDate.Enabled = true;
                //    //realStartDate.Enabled = true;
                //    //非生产节点
                //    IList dtlList = new ArrayList();
                //    if (oprNode.Level != 1)
                //    {
                //        dtlList = model.SearchGWBSDetail(oprNode.SysCode);
                //    }
                //    dgProject.Rows.Clear();
                //    dgBear.Rows.Clear();
                //    if (dtlList.Count > 0)
                //    {
                //        //colProjectDetail.Visible = false;
                //        colUnit.Visible = false;
                //        colDtlState.Visible = false;
                //        foreach (GWBSDetail detail in dtlList)
                //        {
                //            int rowIndex = dgProject.Rows.Add();
                //            dgProject[colProjectDetail.Name, rowIndex].Value = detail.Name;
                //            if (detail.TheCostItem != null)
                //            {
                //                dgProject[colDECode.Name, rowIndex].Value = detail.TheCostItem.QuotaCode;//定额编号
                //            }
                //            dgProject[colProjectDetail.Name, rowIndex].Value = detail.Name;//汇总时取汇总数据的第一条明细名称
                //            dgProject[colMainResType.Name, rowIndex].Value = detail.MainResourceTypeName;
                //            dgProject[colSpe.Name, rowIndex].Value = detail.MainResourceTypeSpec;
                //            dgProject[colTH.Name, rowIndex].Value = detail.DiagramNumber;
                //            dgProject[colPlanProject.Name, rowIndex].Value = detail.PlanWorkAmount;
                //            dgProject[colSumProject.Name, rowIndex].Value = detail.QuantityConfirmed;
                //            if (detail.PlanWorkAmount != 0)
                //            {
                //                dgProject[colSumXXJD.Name, rowIndex].Value = (detail.QuantityConfirmed / detail.PlanWorkAmount * 100).ToString("0.00");
                //            }
                //            dgProject.Rows[rowIndex].Tag = detail;
                //        }
                //    }
                //}
                FillProjectNew(oprNode);
                #endregion

                this.GetNodeDetail();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
            }
        }

        private string GetFullPath(PBSTree pbs)
        {
            string path = string.Empty;
            path = pbs.Name;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", pbs.Id));
            oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof(PBSTree), oq);
            pbs = list[0] as PBSTree;
            CategoryNode parent = pbs.ParentNode;
            while (parent != null)
            {
                path = parent.Name + "\\" + path;
                oq.Criterions.Clear();
                oq.Criterions.Clear();
                oq.AddCriterion(Expression.Eq("Id", parent.Id));
                oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
                list = model.ObjectQuery(typeof(PBSTree), oq);
                parent = (list[0] as CategoryNode).ParentNode;
            }
            return path;
        }
        private string GetFullPath(ProjectTaskTypeTree taskType)
        {
            string path = string.Empty;
            path = taskType.Name;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", taskType.Id));
            oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof(ProjectTaskTypeTree), oq);
            taskType = list[0] as ProjectTaskTypeTree;
            CategoryNode parent = taskType.ParentNode;
            while (parent != null)
            {
                path = parent.Name + "\\" + path;
                oq.Criterions.Clear();
                oq.Criterions.Clear();
                oq.AddCriterion(Expression.Eq("Id", parent.Id));
                oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
                list = model.ObjectQuery(typeof(ProjectTaskTypeTree), oq);
                parent = (list[0] as CategoryNode).ParentNode;
            }
            return path;
        }

        private GWBSTree LoadRelaAttribute(GWBSTree wbs)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", wbs.Id));
            oq.AddFetchMode("ListRelaPBS", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ProjectTaskTypeGUID", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("PriceAmountUnitGUID", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof(GWBSTree), oq);
            if (list.Count > 0)
            {
                wbs = list[0] as GWBSTree;
            }
            return wbs;
        }

        void tvwCategory_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (isSelectNodeInvoke)
            {
                isSelectNodeInvoke = false;
            }
            else
            {
                #region 单击复选框操作
                if ((Control.ModifierKeys & Keys.Shift) != 0 || (Control.ModifierKeys & Keys.Control) != 0)//如果同时按下了Ctrl //(Control.ModifierKeys & Keys.Shift) != 0 || 
                {
                    if (e.Node.Checked)
                    {
                        e.Node.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                        e.Node.ForeColor = ColorTranslator.FromHtml("#000000");

                        //e.Node.BackColor = SystemColors.Control;
                        //e.Node.ForeColor = SystemColors.ControlText;

                        if (listCheckedNode.ContainsKey(e.Node.Name))
                            listCheckedNode[e.Node.Name] = e.Node;
                        else
                            listCheckedNode.Add(e.Node.Name, e.Node);
                    }
                    else
                    {
                        TreeNode tempNode = new TreeNode();
                        e.Node.BackColor = tempNode.BackColor;
                        e.Node.ForeColor = tempNode.ForeColor;

                        if (listCheckedNode.ContainsKey(e.Node.Name))
                            listCheckedNode.Remove(e.Node.Name);
                    }

                    SetChildChecked(e.Node);
                }
                else
                {
                    if (e.Node.Checked)
                    {
                        e.Node.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                        e.Node.ForeColor = ColorTranslator.FromHtml("#000000");

                        //e.Node.BackColor = SystemColors.Control;
                        //e.Node.ForeColor = SystemColors.ControlText;

                        if (listCheckedNode.ContainsKey(e.Node.Name))
                            listCheckedNode[e.Node.Name] = e.Node;
                        else
                            listCheckedNode.Add(e.Node.Name, e.Node);
                    }
                    else
                    {
                        TreeNode tempNode = new TreeNode();
                        e.Node.BackColor = tempNode.BackColor;
                        e.Node.ForeColor = tempNode.ForeColor;

                        if (listCheckedNode.ContainsKey(e.Node.Name))
                            listCheckedNode.Remove(e.Node.Name);
                    }
                }

                #endregion
            }
            RefreshControls(MainViewState.Check);
        }

        private void SetChildChecked(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                SetChildChecked(tn);
                tn.Checked = parentNode.Checked;
                if (tn.Checked)
                {
                    tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    tn.ForeColor = ColorTranslator.FromHtml("#000000");
                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode[tn.Name] = tn;
                    else
                        listCheckedNode.Add(tn.Name, tn);
                }
                else
                {
                    TreeNode tempNode = new TreeNode();
                    tn.BackColor = tempNode.BackColor;
                    tn.ForeColor = tempNode.ForeColor;
                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode.Remove(tn.Name);
                }
            }
        }

        private void SetChildCheckedByMultiSel(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                SetChildCheckedByMultiSel(tn);
                isSelectNodeInvoke = true;
                if (startNodeCheckedState)//如果起始节点当前为选中，就取消选择
                {
                    TreeNode tempNode = new TreeNode();
                    tn.BackColor = tempNode.BackColor;
                    tn.ForeColor = tempNode.ForeColor;
                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode.Remove(tn.Name);
                    tn.Checked = false;
                }
                else//如果起始节点当前为未选中，就设置选择
                {
                    tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    tn.ForeColor = ColorTranslator.FromHtml("#000000");
                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode[tn.Name] = tn;
                    else
                        listCheckedNode.Add(tn.Name, tn);
                    tn.Checked = true;
                }
            }
        }

        private void GetChildChecked(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Checked)
                {
                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode[tn.Name] = tn;
                    else
                        listCheckedNode.Add(tn.Name, tn);
                }
                GetChildChecked(tn);
            }
        }

        private void RemoveChildChecked(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                RemoveChildChecked(tn);
                if (tn.Checked)
                {
                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode.Remove(tn.Name);
                }
            }
        }

        private void GetNodeDetail()
        {
            try
            {
                ClearAll();
                //基本信息
                this.txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath;
                this.txtFullPath.Text = tvwCategory.SelectedNode.FullPath;
                this.txtTaskState.Text = StaticMethod.GetWBSTaskStateText(oprNode.TaskState);
                this.txtTaskCode.Text = oprNode.Code;
                this.txtTaskName.Text = oprNode.Name;
                if (oprNode.ListRelaPBS.Count > 0)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddFetchMode("ThePBS", NHibernate.FetchMode.Eager);
                    Disjunction dis = new Disjunction();
                    foreach (GWBSRelaPBS rela in oprNode.ListRelaPBS)
                    {
                        dis.Add(Expression.Eq("Id", rela.Id));
                    }
                    oq.AddCriterion(dis);
                    IList listRela = model.ObjectQuery(typeof(GWBSRelaPBS), oq);
                    List<PBSTree> listPBS = new List<PBSTree>();
                    for (int i = 0; i < listRela.Count; i++)
                    {
                        PBSTree pbs = (listRela[i] as GWBSRelaPBS).ThePBS;
                        pbs.FullPath = GetFullPath(pbs);
                        cbRelaPBS.Items.Add(pbs);
                        listPBS.Add(pbs);
                    }
                    cbRelaPBS.DisplayMember = "FullPath";
                    cbRelaPBS.ValueMember = "Id";
                    cbRelaPBS.Tag = listPBS;
                }
                if (oprNode.ProjectTaskTypeGUID != null)
                {
                    this.txtTaskWBSType.Text = GetFullPath(oprNode.ProjectTaskTypeGUID);
                    this.txtTaskWBSType.Tag = oprNode.ProjectTaskTypeGUID;
                }
                this.txtTaskDesc.Text = oprNode.Describe;
                this.txtOwner.Text = oprNode.OwnerName;
                if (oprNode.TaskPlanEndTime != null && oprNode.TaskPlanEndTime > Convert.ToDateTime("0001-01-01 00:00:00"))
                {
                    this.dtEndTime.Value = oprNode.TaskPlanEndTime.Value;
                }
                //if (oprNode.TaskStateTime != null)
                if (oprNode.TaskPlanStartTime != null && oprNode.TaskPlanStartTime > Convert.ToDateTime("0001-01-01 00:00:00"))
                {
                    this.dtStartTime.Value = oprNode.TaskPlanStartTime.Value;
                }
                if (oprNode.TaskPlanEndTime != null && oprNode.TaskPlanStartTime != null && oprNode.TaskPlanEndTime > Convert.ToDateTime("0001-01-01 00:00:00") && oprNode.TaskPlanStartTime > Convert.ToDateTime("0001-01-01 00:00:00"))
                {
                    txtPlanTime.Text = ((oprNode.TaskPlanEndTime.Value - oprNode.TaskPlanStartTime.Value).Days + 1).ToString();
                }
                //检查要求
                if (!string.IsNullOrEmpty(oprNode.CheckRequire))
                {
                    char[] chs = oprNode.CheckRequire.ToCharArray();
                    for (int i = 0; i < chs.Length; i++)
                    {
                        Char c = chs[i];
                        if (c == '0')
                        {
                            if (cbListCheckRequire.Items.Count > i)
                                cbListCheckRequire.SetItemChecked(i, true);
                        }
                        else
                        {
                            if (cbListCheckRequire.Items.Count > i)
                                cbListCheckRequire.SetItemChecked(i, false);
                        }
                    }
                }
                cbResponseAccount.Checked = oprNode.ResponsibleAccFlag;
                cbCostAccount.Checked = oprNode.CostAccFlag;
                cbProductConfirm.Checked = oprNode.ProductConfirmFlag;
                cbSubContractFee.Checked = oprNode.SubContractFeeFlag;
                cbWarehouseFlag.Checked = oprNode.WarehouseFlag;
                txtFigureProgress.Text = oprNode.AddUpFigureProgress.ToString();
                cbOverOrUnderGroundFlag.Text = oprNode.OverOrUndergroundFlag.ToString();
                if (oprNode.RealEndDate != null && oprNode.RealEndDate > Convert.ToDateTime("0001-01-01 00:00:00"))
                {
                    realEndDate.Value = oprNode.RealEndDate.Value;
                }
                if (oprNode.RealStartDate != null && oprNode.RealStartDate > Convert.ToDateTime("0001-01-01 00:00:00"))
                {
                    realStartDate.Value = oprNode.RealStartDate.Value;
                }
                if (oprNode.RealEndDate != null && oprNode.RealStartDate != null && oprNode.RealEndDate > Convert.ToDateTime("0001-01-01 00:00:00") && oprNode.RealStartDate > Convert.ToDateTime("0001-01-01 00:00:00"))
                {
                    txtRealTime.Text = ClientUtil.ToString((oprNode.RealEndDate.Value - oprNode.RealStartDate.Value).Days + 1);
                }
                txtComplateDescript.Text = oprNode.CompleteDescription;
                //日常检查要求
                txtDayCheckState.Text = StaticMethod.GetCheckStateShowText(oprNode.DailyCheckState);
                txtAcceptanceCheckState.Text = oprNode.AcceptanceCheckState == 0 ? "" : oprNode.AcceptanceCheckState.ToString();
                txtSuperiorCheckState.Text = oprNode.SuperiorCheckState == 0 ? "" : oprNode.SuperiorCheckState.ToString();

                FillSpecial();
                //RefreshControls(MainViewState.Browser);
            }
            catch (Exception exp)
            {
                MessageBox.Show("显示出错：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        void FillSpecial()
        {
            string strSysCode = "";
            string SpecialName = "";
            foreach (System.Collections.DictionaryEntry objDE in SpecialHt)
            {
                string strCode = objDE.Key.ToString();
                if (oprNode.SysCode.StartsWith(strCode))
                {
                    if (strSysCode != "")
                    {
                        if (strSysCode.Length > strCode.Length)
                        {
                            strSysCode = strCode;
                            SpecialName = objDE.Value.ToString();
                        }
                    }
                    else
                    {
                        strSysCode = strCode;
                        SpecialName = objDE.Value.ToString();
                    }
                }
            }
            cbSpecialtyClassify.Text = SpecialName;
        }

        private void ClearAll()
        {
            //基本信息
            this.txtCurrentPath.Text = "";
            this.txtTaskState.Text = "";
            this.txtTaskCode.Text = "";
            this.txtTaskName.Text = "";
            this.cbRelaPBS.Items.Clear();
            this.cbRelaPBS.Tag = null;
            this.txtTaskWBSType.Text = "";
            this.txtTaskWBSType.Tag = null;

            this.txtTaskDesc.Text = "";

            this.txtOwner.Text = "";
            this.txtPlanTime.Text = "";
            this.txtRealTime.Text = "";
            this.txtComplateDescript.Text = "";

            //DateTime serverTime = model.GetServerTime();
            //this.dtStartTime.Value = serverTime.Date;
            //this.dtEndTime.Value = serverTime.Date.AddDays(7);

            this.dtStartTime.Text = "";
            this.dtEndTime.Text = "";
            realStartDate.Text = "";
            realEndDate.Text = "";

            cbResponseAccount.Checked = false;
            cbCostAccount.Checked = false;
            cbProductConfirm.Checked = false;
            cbSubContractFee.Checked = false;
            cbWarehouseFlag.Checked = false;

            for (int i = 0; i < cbListCheckRequire.Items.Count; i++)
            {
                cbListCheckRequire.SetItemChecked(i, false);
            }

            txtFigureProgress.Text = "";

        }

        private void LoadGWBSChildNode(TreeNode parentNode, GWBSTree parentObj, IEnumerable<GWBSTree> listGWBS)
        {
            var query = from wbs in listGWBS
                        where wbs.ParentNode.Id == parentObj.Id
                        select wbs;

            foreach (GWBSTree wbs in query)
            {
                TreeNode childNode = new TreeNode();
                childNode.Tag = wbs;
                childNode.Name = wbs.Id;
                childNode.Text = wbs.Name;
                parentNode.Nodes.Add(childNode);

                LoadGWBSChildNode(childNode, wbs, listGWBS);
            }
        }

        public override bool SaveView()
        {
            if (!ValideSave()) return false;
            return model.SaveAndWritebackScrollPlanState(oprNode);
            //IList list = new ArrayList();
            //list.Add(oprNode);
            //IList resultList = model.SaveOrUpdate(list);
            //if (resultList != null && resultList.Count > 0)
            //{
            //    return true;
            //}
            //return false;
        }

        private bool ValideSave()
        {
            try
            {
                if (txtPlanTime.Text == "")
                {
                    //MessageBox.Show("计划工期不能为空");
                    //return false;
                }
                else
                {
                    bool validity = true;
                    string temp_quantity = txtPlanTime.Text.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("计划工期输入数字！");
                        return false;
                    }
                }
                if (txtRealTime.Text == "")
                {
                    //MessageBox.Show("实际工期不能为空");
                    //return false;
                }
                else
                {
                    bool validity = true;
                    string temp_quantity = txtRealTime.Text.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("实际工期输入数字！");
                        return false;
                    }
                }
                //基本信息
                if (dtStartTime.IsHasValue)
                    oprNode.TaskPlanStartTime = dtStartTime.Value;
                else
                    oprNode.TaskPlanStartTime = null;

                if (dtEndTime.IsHasValue)
                    oprNode.TaskPlanEndTime = dtEndTime.Value;
                else
                    oprNode.TaskPlanEndTime = null;

                if (realStartDate.IsHasValue)
                    oprNode.RealStartDate = realStartDate.Value;
                else
                    oprNode.RealStartDate = null;

                if (realEndDate.IsHasValue)
                    oprNode.RealEndDate = realEndDate.Value;
                else
                    oprNode.RealEndDate = null;


                oprNode.RealTime = ClientUtil.ToInt(txtRealTime.Text);
                oprNode.PalnTime = ClientUtil.ToInt(txtPlanTime.Text);
                oprNode.CompleteDescription = ClientUtil.ToString(txtComplateDescript.Text);
                try
                {
                    decimal AddUpFigureProgress = 0;
                    if (txtFigureProgress.Text.Trim() != "")
                        AddUpFigureProgress = Convert.ToDecimal(txtFigureProgress.Text);

                    oprNode.AddUpFigureProgress = AddUpFigureProgress;
                }
                catch
                {
                    MessageBox.Show("形象进度格式填写不正确！");
                    txtFigureProgress.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
                return false;
            }
        }
        /// <summary>
        /// 验证仓库标识是否符合规范（从当前节点到根节点的所有节点,或所有子节点上只能设置一个此标志）
        /// </summary>
        /// <param name="currWBSNode">验证的GWBS节点</param>
        /// <param name="validType">验证类型（1.新增，2.修改）</param>
        /// <param name="fullPath">存在此标志节点的完整路径</param>
        /// <returns>true表示可以添加，false则不能</returns>
        private bool ValidateWareFlag(GWBSTree currWBSNode, int validType, ref string msg)
        {
            if (currWBSNode == null)
                return false;

            //注：检查时依据数据库里的数据（防止并发界面数据和实际不一致问题）

            ObjectQuery oq = new ObjectQuery();


            //检查所有父节点的此标志
            string[] sysCodes = currWBSNode.SysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            oq.AddCriterion(Expression.Eq("WarehouseFlag", true));

            Disjunction dis = new Disjunction();
            for (int i = 0; i < sysCodes.Length; i++)
            {
                string sysCode = "";
                for (int j = 0; j <= i; j++)
                {
                    sysCode += sysCodes[j] + ".";

                }

                dis.Add(Expression.Eq("SysCode", sysCode));
            }
            oq.AddCriterion(dis);

            IList listParent = model.ObjectQuery(typeof(GWBSTree), oq);
            if (listParent.Count > 0)
            {
                GWBSTree parentNode = listParent[0] as GWBSTree;

                msg = "在父节点“" + StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), parentNode.Name, parentNode.SysCode) + "”上已经设置了“仓库标志”属性.";

                return false;
            }

            if (validType == 2)
            {
                //检查所有子节点的此标志
                oq.Criterions.Clear();
                oq.AddCriterion(Expression.Eq("WarehouseFlag", true));
                oq.AddCriterion(Expression.Like("SysCode", currWBSNode.SysCode, MatchMode.Start));
                oq.AddCriterion(Expression.Not(Expression.Eq("SysCode", currWBSNode.SysCode)));

                IList listChild = model.ObjectQuery(typeof(GWBSTree), oq);
                if (listChild.Count > 0)
                {
                    GWBSTree childNode = listChild[0] as GWBSTree;

                    msg = "在子节点“" + StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), childNode.Name, childNode.SysCode) + "”上已经设置了“仓库标志”属性.";

                    return false;
                }
            }
            return true;
        }

        public void Start()
        {
            RefreshState(MainViewState.Browser);
            if (IsDistribute)
            {
                NewLoadGWBSTreeTree();
            }
            else
            {
                LoadGWBSTreeTree();
            }
        }

        private void LoadGWBSTreeTree1(TreeNode oNode)
        {
            Hashtable hashtable = new Hashtable();
            int iLevel = 1;
            string sSysCode = string.Empty;
            IList list = null;
            try
            {
                if (oNode != null)
                {
                    GWBSTree oGWBSTree = oNode.Tag as GWBSTree;
                    if (oGWBSTree != null)
                    {

                        if (oGWBSTree != null)
                        {
                            iLevel = oGWBSTree.Level + 1;
                            sSysCode = oGWBSTree.SysCode;
                        }
                    }
                }
                list = model.GetGWBSTreesByInstance(projectInfo.Id, sSysCode, iLevel);
                if (oNode != null)
                {
                    oNode.Nodes.Clear();
                }

                if (list == null || list.Count == 0)
                {
                    PBSTree pbs = null;
                    ProjectTaskTypeTree taskType = null;

                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));

                    //if (oNode == null)
                    //{
                    oq.AddCriterion(Expression.IsNull("ParentNode"));
                    oq.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
                    //}
                    //else
                    //{

                    //}
                    IList listPBS = model.ObjectQuery(typeof(PBSTree), oq);

                    if (listPBS == null || listPBS.Count == 0)
                        return;
                    pbs = listPBS[0] as PBSTree;


                    oq.Criterions.Clear();
                    oq.AddCriterion(Expression.IsNull("ParentNode"));
                    oq.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
                    IList listTaskType = model.ObjectQuery(typeof(ProjectTaskTypeTree), oq);

                    if (listTaskType == null || listTaskType.Count == 0)
                        return;
                    taskType = listTaskType[0] as ProjectTaskTypeTree;


                    IList listAdd = new ArrayList();

                    GWBSTree root = new GWBSTree();

                    DateTime serverTime = model.GetServerTime();

                    root.TheProjectGUID = projectInfo.Id;
                    root.TheProjectName = projectInfo.Name;
                    root.Name = projectInfo.Name;

                    root.Code = projectInfo.Code.PadLeft(4, '0') + "-" + model.GetCode(typeof(GWBSTree));
                    root.CheckRequire = string.IsNullOrEmpty(taskType.CheckRequire) ? ("X".PadRight(11, 'X') + "0") : (taskType.CheckRequire.PadRight(11, 'X') + "0");

                    GWBSRelaPBS relaPBS = new GWBSRelaPBS();
                    relaPBS.ThePBS = pbs;

                    relaPBS.TheGWBSTree = root;
                    root.ListRelaPBS.Add(relaPBS);

                    root.ProjectTaskTypeGUID = taskType;
                    root.ProjectTaskTypeName = taskType.Name;

                    root.TaskState = DocumentState.Edit;
                    root.TaskStateTime = serverTime;
                    root.OwnerGUID = ConstObject.LoginPersonInfo.Id;
                    root.OwnerName = ConstObject.LoginPersonInfo.Name;
                    root.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;
                    root.OrderNo = 0;

                    listAdd.Add(root);

                    model.SaveGWBSTreeRootNode(listAdd);

                    list = model.GetGWBSTreesByInstance(projectInfo.Id, sSysCode, iLevel);
                }

                if (list != null && list.Count > 0)
                {
                    foreach (GWBSTree childNode in list)
                    {
                        CreateChildNode(oNode, childNode);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }
        public void CreateChildNode(TreeNode oParentNode, GWBSTree oGWBSTree)
        {

            if (oGWBSTree.State != 0)
            {
                TreeNode oChild = new TreeNode();
                oChild.Name = oGWBSTree.Id;
                oChild.Text = oGWBSTree.Name;
                oChild.Tag = oGWBSTree;
                if (oGWBSTree.CategoryNodeType != NodeType.LeafNode)
                {
                    TreeNode oEmptyNode = new TreeNode();
                    oEmptyNode.Name = "Test";
                    oEmptyNode.Text = "Test";
                    oEmptyNode.Tag = null;
                    oChild.Nodes.Add(oEmptyNode);
                }
                if (oParentNode == null)
                {
                    tvwCategory.Nodes.Add(oChild);
                }
                else
                {
                    oParentNode.Nodes.Add(oChild);
                }
                if ((oChild.Tag as GWBSTree).ProductConfirmFlag)
                {
                    //生产节点
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("GWBSDetail.TheGWBS", oChild.Tag as GWBSTree));
                    IList list = model.ObjectQuery(typeof(GWBSTaskConfirm), oq);
                    if (list.Count > 0)
                    {
                        oParentNode.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                        oParentNode.ForeColor = ColorTranslator.FromHtml("#000000");
                    }

                }
            }
        }
        private void LoadGWBSTreeTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();
                IList list = model.GetGWBSTreesByInstance(projectInfo.Id);


                if (list == null || list.Count == 0)
                {
                    PBSTree pbs = null;
                    ProjectTaskTypeTree taskType = null;

                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
                    oq.AddCriterion(Expression.IsNull("ParentNode"));
                    oq.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
                    IList listPBS = model.ObjectQuery(typeof(PBSTree), oq);

                    if (listPBS == null || listPBS.Count == 0)
                        return;
                    pbs = listPBS[0] as PBSTree;


                    oq.Criterions.Clear();
                    oq.AddCriterion(Expression.IsNull("ParentNode"));
                    oq.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
                    IList listTaskType = model.ObjectQuery(typeof(ProjectTaskTypeTree), oq);

                    if (listTaskType == null || listTaskType.Count == 0)
                        return;
                    taskType = listTaskType[0] as ProjectTaskTypeTree;


                    IList listAdd = new ArrayList();

                    GWBSTree root = new GWBSTree();

                    DateTime serverTime = model.GetServerTime();

                    root.TheProjectGUID = projectInfo.Id;
                    root.TheProjectName = projectInfo.Name;
                    root.Name = projectInfo.Name;

                    root.Code = projectInfo.Code.PadLeft(4, '0') + "-" + model.GetCode(typeof(GWBSTree));
                    root.CheckRequire = string.IsNullOrEmpty(taskType.CheckRequire) ? ("X".PadRight(11, 'X') + "0") : (taskType.CheckRequire.PadRight(11, 'X') + "0");

                    GWBSRelaPBS relaPBS = new GWBSRelaPBS();
                    relaPBS.ThePBS = pbs;

                    relaPBS.TheGWBSTree = root;
                    root.ListRelaPBS.Add(relaPBS);

                    root.ProjectTaskTypeGUID = taskType;
                    root.ProjectTaskTypeName = taskType.Name;

                    root.TaskState = DocumentState.Edit;
                    root.TaskStateTime = serverTime;
                    root.OwnerGUID = ConstObject.LoginPersonInfo.Id;
                    root.OwnerName = ConstObject.LoginPersonInfo.Name;
                    root.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;
                    root.OrderNo = 0;

                    listAdd.Add(root);

                    model.SaveGWBSTreeRootNode(listAdd);

                    list = model.GetGWBSTreesByInstance(projectInfo.Id);
                }

                if (list != null && list.Count > 0)
                {
                    foreach (GWBSTree childNode in list)
                    {
                        if (childNode.State == 0)
                            continue;

                        TreeNode tnTmp = new TreeNode();
                        tnTmp.Name = childNode.Id.ToString();
                        tnTmp.Text = childNode.Name;
                        tnTmp.Tag = childNode;
                        //ObjectQuery oq = new ObjectQuery();
                        //oq.AddCriterion(Expression.Eq("GWBSDetail.TheGWBS.Id", childNode.Id));
                        //IList listTmp = model.ObjectQuery(typeof(GWBSTaskConfirm), oq);
                        if (childNode.ProductConfirmFlag)
                        {
                            tnTmp.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                            tnTmp.ForeColor = ColorTranslator.FromHtml("#000000");
                        }
                        if (childNode.ParentNode != null)
                        {
                            TreeNode tnp = null;
                            tnp = hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                            if (tnp != null)
                                tnp.Nodes.Add(tnTmp);
                        }
                        else
                        {
                            tvwCategory.Nodes.Add(tnTmp);
                        }
                        hashtable.Add(tnTmp.Name, tnTmp);
                        if ((tnTmp.Tag as GWBSTree).SpecialType != null)
                        {
                            SpecialHt.Add((tnTmp.Tag as GWBSTree).SysCode, (tnTmp.Tag as GWBSTree).SpecialType);
                        }
                    }
                    this.tvwCategory.SelectedNode = this.tvwCategory.Nodes[0];
                    this.tvwCategory.SelectedNode.Expand();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        private void NewLoadGWBSTreeTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();
                //IList list = model.GetGWBSTreesByInstance(projectInfo.Id);
                DataTable dtGwbsTress = model.GetGWBSTree(projectInfo.Id, "", 1);

                if (dtGwbsTress == null || dtGwbsTress.Rows.Count == 0)
                {
                    PBSTree pbs = null;
                    ProjectTaskTypeTree taskType = null;

                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
                    oq.AddCriterion(Expression.IsNull("ParentNode"));
                    oq.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
                    IList listPBS = model.ObjectQuery(typeof(PBSTree), oq);

                    if (listPBS == null || listPBS.Count == 0)
                        return;
                    pbs = listPBS[0] as PBSTree;


                    oq.Criterions.Clear();
                    oq.AddCriterion(Expression.IsNull("ParentNode"));
                    oq.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
                    IList listTaskType = model.ObjectQuery(typeof(ProjectTaskTypeTree), oq);

                    if (listTaskType == null || listTaskType.Count == 0)
                        return;
                    taskType = listTaskType[0] as ProjectTaskTypeTree;


                    IList listAdd = new ArrayList();

                    GWBSTree root = new GWBSTree();

                    DateTime serverTime = model.GetServerTime();

                    root.TheProjectGUID = projectInfo.Id;
                    root.TheProjectName = projectInfo.Name;
                    root.Name = projectInfo.Name;

                    root.Code = projectInfo.Code.PadLeft(4, '0') + "-" + model.GetCode(typeof(GWBSTree));
                    root.CheckRequire = string.IsNullOrEmpty(taskType.CheckRequire) ? ("X".PadRight(11, 'X') + "0") : (taskType.CheckRequire.PadRight(11, 'X') + "0");

                    GWBSRelaPBS relaPBS = new GWBSRelaPBS();
                    relaPBS.ThePBS = pbs;

                    relaPBS.TheGWBSTree = root;
                    root.ListRelaPBS.Add(relaPBS);

                    root.ProjectTaskTypeGUID = taskType;
                    root.ProjectTaskTypeName = taskType.Name;

                    root.TaskState = DocumentState.Edit;
                    root.TaskStateTime = serverTime;
                    root.OwnerGUID = ConstObject.LoginPersonInfo.Id;
                    root.OwnerName = ConstObject.LoginPersonInfo.Name;
                    root.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;
                    root.OrderNo = 0;

                    listAdd.Add(root);

                    model.SaveGWBSTreeRootNode(listAdd);

                    //  list = model.GetGWBSTreesByInstance(projectInfo.Id);
                    dtGwbsTress = model.GetGWBSTree(projectInfo.Id, "", 1);
                }

                if (dtGwbsTress != null && dtGwbsTress.Rows.Count > 0)
                {
                    AddChildNodes(null, dtGwbsTress);
                    this.tvwCategory.SelectedNode = this.tvwCategory.Nodes[0];
                    this.tvwCategory.SelectedNode.Expand();
                    //foreach (GWBSTree childNode in list)
                    //{
                    //    if (childNode.State == 0)
                    //        continue;

                    //    TreeNode tnTmp = new TreeNode();
                    //    tnTmp.Name = childNode.Id.ToString();
                    //    tnTmp.Text = childNode.Name;
                    //    tnTmp.Tag = childNode;
                    //    //ObjectQuery oq = new ObjectQuery();
                    //    //oq.AddCriterion(Expression.Eq("GWBSDetail.TheGWBS.Id", childNode.Id));
                    //    //IList listTmp = model.ObjectQuery(typeof(GWBSTaskConfirm), oq);
                    //    if (childNode.ProductConfirmFlag)
                    //    {
                    //        tnTmp.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    //        tnTmp.ForeColor = ColorTranslator.FromHtml("#000000");
                    //    }
                    //    if (childNode.ParentNode != null)
                    //    {
                    //        TreeNode tnp = null;
                    //        tnp = hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                    //        if (tnp != null)
                    //            tnp.Nodes.Add(tnTmp);
                    //    }
                    //    else
                    //    {
                    //        tvwCategory.Nodes.Add(tnTmp);
                    //    }
                    //    hashtable.Add(tnTmp.Name, tnTmp);
                    //    if ((tnTmp.Tag as GWBSTree).SpecialType != null)
                    //    {
                    //        SpecialHt.Add((tnTmp.Tag as GWBSTree).SysCode, (tnTmp.Tag as GWBSTree).SpecialType);
                    //    }
                    //}
                    //this.tvwCategory.SelectedNode = this.tvwCategory.Nodes[0];
                    //this.tvwCategory.SelectedNode.Expand();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        public void AddChildNodes(TreeNode oParentNode, DataTable tbGWBSTress)
        {
            if (oParentNode != null)
            {
                oParentNode.Nodes.Clear();
            }
            if (tbGWBSTress != null && tbGWBSTress.Rows.Count > 0)
            {
                GWBSTree oGWBSTree = null;
                TreeNode oTreeNode = null;
                foreach (DataRow oRow in tbGWBSTress.Rows)
                {
                    oGWBSTree = CreateGWBSTree(oRow);
                    string sName = oGWBSTree.Name;
                    if (oGWBSTree != null)
                    {
                        oTreeNode = CreateNode(oGWBSTree);
                        if (oTreeNode != null)
                        {
                            if (oParentNode == null)
                            {
                                this.tvwCategory.Nodes.Add(oTreeNode);
                            }
                            else
                            {
                                oParentNode.Nodes.Add(oTreeNode);
                            }
                        }
                    }
                }

            }
        }
        public TreeNode CreateNode(GWBSTree oGWBSTree)
        {
            TreeNode tnTmp = new TreeNode();
            tnTmp.Name = oGWBSTree.Id.ToString();
            tnTmp.Text = oGWBSTree.Name;
            tnTmp.Tag = oGWBSTree;
            //ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("GWBSDetail.TheGWBS.Id", childNode.Id));
            //IList listTmp = model.ObjectQuery(typeof(GWBSTaskConfirm), oq);
            if (oGWBSTree.ProductConfirmFlag)
            {
                tnTmp.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                tnTmp.ForeColor = ColorTranslator.FromHtml("#000000");
            }
            //需要查看 hashtable SpecialHt
            //if (oGWBSTree.ParentNode != null)
            //{
            //    TreeNode tnp = null;
            //    tnp = hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
            //    if (tnp != null)
            //        tnp.Nodes.Add(tnTmp);
            //}
            //else
            //{
            //    tvwCategory.Nodes.Add(tnTmp);
            //}
            //hashtable.Add(tnTmp.Name, tnTmp);
            if ((tnTmp.Tag as GWBSTree).SpecialType != null)
            {
                SpecialHt.Add((tnTmp.Tag as GWBSTree).SysCode, (tnTmp.Tag as GWBSTree).SpecialType);
            }
            if (oGWBSTree.CategoryNodeType != NodeType.LeafNode)
            {
                tnTmp.Nodes.Add("test");
            }
            return tnTmp;
        }
        public void tvwCategory_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                TreeNode oTreeNode = e.Node;
                DataTable tbGwbsTress = null;
                GWBSTree oGWBSTree = null;

                if (oTreeNode.Nodes != null && oTreeNode.Nodes.Count > 0 && oTreeNode.Nodes[0].Tag == null)
                {
                    oTreeNode.Nodes.Clear();
                    oGWBSTree = oTreeNode.Tag as GWBSTree;
                    tbGwbsTress = model.GetGWBSTree(projectInfo.Id, oGWBSTree.Id, oGWBSTree.Level + 1);
                    if (tbGwbsTress != null && tbGwbsTress.Rows.Count > 0)
                    {
                        AddChildNodes(oTreeNode, tbGwbsTress);
                    }
                }
            }
        }
        public GWBSTree CreateGWBSTree(DataRow oRow)
        {
            GWBSTree oGWBSTree = null;
            if (oRow != null)
            {
                oGWBSTree = new GWBSTree();
                oGWBSTree.Name = ClientUtil.ToString(oRow["Name"]);
                oGWBSTree.Id = ClientUtil.ToString(oRow["ID"]);
                oGWBSTree.Version = 0;
                oGWBSTree.CategoryNodeType = (NodeType)ClientUtil.ToInt(oRow["CategoryNodeType"]);
                if (oGWBSTree.CategoryNodeType == NodeType.LeafNode)
                {
                    oGWBSTree.CategoryNodeType = ClientUtil.ToInt(oRow["childCount"]) == 0 ? NodeType.LeafNode : NodeType.MiddleNode;
                }
                oGWBSTree.Code = ClientUtil.ToString(oRow["Code"]);
                oGWBSTree.CreateDate = ClientUtil.ToDateTime(oRow["CreateDate"]);
                oGWBSTree.SysCode = ClientUtil.ToString(oRow["SysCode"]);
                oGWBSTree.State = ClientUtil.ToInt(oRow["State"]);
                oGWBSTree.Level = ClientUtil.ToInt(oRow["TLevel"]);
                oGWBSTree.Describe = ClientUtil.ToString(oRow["Describe"]);
                if (string.IsNullOrEmpty(ClientUtil.ToString(oRow["PARENTNODEID"])))
                {
                    oGWBSTree.ParentNode = null;
                }
                else
                {
                    oGWBSTree.ParentNode = new GWBSTree();
                    oGWBSTree.ParentNode.Id = ClientUtil.ToString(oRow["PARENTNODEID"]);
                    oGWBSTree.ParentNode.Version = 0;
                }
                //oGWBSTree .Author =new 
                //TheTree
                oGWBSTree.OrderNo = ClientUtil.ToLong(oRow["OrderNo"]);
                if (string.IsNullOrEmpty(ClientUtil.ToString(oRow["projecttasktypeguid"])))
                {
                    oGWBSTree.ProjectTaskTypeGUID = null;
                }
                else
                {
                    oGWBSTree.ProjectTaskTypeGUID = new ProjectTaskTypeTree();
                    oGWBSTree.ProjectTaskTypeGUID.Id = ClientUtil.ToString(oRow["projecttasktypeguid"]);
                    oGWBSTree.ProjectTaskTypeGUID.Name = ClientUtil.ToString(oRow["ProjectTaskTypeName"]);
                    oGWBSTree.ProjectTaskTypeGUID.Version = 0;
                }
                oGWBSTree.ProjectTaskTypeName = ClientUtil.ToString(oRow["ProjectTaskTypeName"]);
                oGWBSTree.Summary = ClientUtil.ToString(oRow["Summary"]);
                oGWBSTree.TaskState = (DocumentState)ClientUtil.ToInt(oRow["TaskState"]);
                oGWBSTree.TaskStateTime = ClientUtil.ToDateTime(oRow["TaskStateTime"]);
                oGWBSTree.ContractTotalPrice = ClientUtil.ToDecimal(oRow["ContractTotalPrice"]);
                oGWBSTree.OwnerGUID = ClientUtil.ToString(oRow["OwnerGUID"]);
                oGWBSTree.OwnerName = ClientUtil.ToString(oRow["OwnerName"]);
                oGWBSTree.OwnerOrgSysCode = ClientUtil.ToString(oRow["OwnerOrgSysCode"]);
                if (string.IsNullOrEmpty(ClientUtil.ToString(oRow["PriceAmountUnitGUID"])))
                {
                    oGWBSTree.PriceAmountUnitGUID = null;
                }
                else
                {
                    oGWBSTree.PriceAmountUnitGUID = new StandardUnit();
                    oGWBSTree.PriceAmountUnitGUID.Id = ClientUtil.ToString(oRow["PriceAmountUnitGUID"]);
                    oGWBSTree.PriceAmountUnitGUID.Name = ClientUtil.ToString(oRow["PriceAmountUnitName"]);
                    oGWBSTree.PriceAmountUnitGUID.Version = 0;
                }
                oGWBSTree.PriceAmountUnitName = ClientUtil.ToString(oRow["PriceAmountUnitName"]);
                oGWBSTree.ResponsibilityTotalPrice = ClientUtil.ToDecimal(oRow["ResponsibilityTotalPrice"]);
                oGWBSTree.PlanTotalPrice = ClientUtil.ToDecimal(oRow["PlanTotalPrice"]);
                oGWBSTree.TaskPlanStartTime = ClientUtil.ToDateTime(oRow["TaskPlanStartTime"]);
                oGWBSTree.TaskPlanEndTime = ClientUtil.ToDateTime(oRow["TaskPlanEndTime"]);
                oGWBSTree.CheckRequire = ClientUtil.ToString(oRow["CheckRequire"]);
                oGWBSTree.NodeType = (WBSNodeType)ClientUtil.ToInt(oRow["CheckRequire"]);
                oGWBSTree.PalnTime = ClientUtil.ToInt(oRow["PalnTime"]);
                oGWBSTree.RealTime = ClientUtil.ToInt(oRow["RealTime"]);
                oGWBSTree.RealTime = ClientUtil.ToInt(oRow["RealTime"]);
                oGWBSTree.RealStartDate = ClientUtil.ToDateTime(oRow["RealStartDate"]);
                oGWBSTree.RealEndDate = ClientUtil.ToDateTime(oRow["RealEndDate"]);
                oGWBSTree.NGUID = ClientUtil.ToString(oRow["NGUID"]);
                oGWBSTree.AddUpFigureProgress = ClientUtil.ToDecimal(oRow["AddUpFigureProgress"]);
                oGWBSTree.ResponsibleAccFlag = ClientUtil.ToBool(oRow["ResponsibleAccFlag"]);
                oGWBSTree.CostAccFlag = ClientUtil.ToBool(oRow["CostAccFlag"]);
                oGWBSTree.ProductConfirmFlag = ClientUtil.ToBool(oRow["ProductConfirmFlag"]);
                oGWBSTree.SubContractFeeFlag = ClientUtil.ToBool(oRow["SubContractFeeFlag"]);
                oGWBSTree.CheckBatchNumber = ClientUtil.ToInt(oRow["CheckBatchNumber"]);
                oGWBSTree.OverOrUndergroundFlag = (OverOrUnderGroundFlagEnum)ClientUtil.ToInt(oRow["OverOrUndergroundFlag"]);
                oGWBSTree.WarehouseFlag = ClientUtil.ToBool(oRow["WarehouseFlag"]);
                oGWBSTree.AcceptanceCheckState = (AcceptanceCheckStateEnum)ClientUtil.ToInt(oRow["AcceptanceCheckState"]);
                oGWBSTree.SuperiorCheckState = (SuperiorCheckStateEnum)ClientUtil.ToInt(oRow["SuperiorCheckState"]);
                oGWBSTree.DailyCheckState = ClientUtil.ToString(oRow["DailyCheckState"]);
                oGWBSTree.SpecialType = ClientUtil.ToString(oRow["SpecialType"]);

            }

            return oGWBSTree;
        }
        public override void RefreshControls(MainViewState state)
        {
            switch (state)
            {

                case MainViewState.Modify:

                    //按钮
                    //界面输入控件
                    btnSaveInspectionRecord.Enabled = true;
                    txtCheckPerson.Enabled = true;
                    cbWBSCheckRequir.Enabled = true;
                    cbCheckConclusion.Enabled = true;
                    dtpCheckDate.Enabled = true;
                    txtCheckStatus.Enabled = true;
                    btnSubmit.Enabled = true;
                    radioButton1.Enabled = true;
                    radioButton2.Enabled = true;
                    txtSupply.Enabled = true;
                    txtCheckPerson1.Enabled = true;
                    cbWBSCheckRequir1.Enabled = true;
                    cbCheckConclusion1.Enabled = true;
                    dtpCheckDate1.Enabled = true;
                    txtCheckStatus1.Enabled = true;
                    btnSubmit1.Enabled = true;
                    radioButton3.Enabled = true;
                    radioButton4.Enabled = true;
                    txtSupply1.Enabled = true;
                    //this.btnSave.Enabled = true;
                    this.btnSave1.Enabled = true;
                    this.btnDelete.Enabled = true;
                    this.btnDelete1.Enabled = true;
                    dgDocumentDetail.ReadOnly = false;
                    FileSelect.ReadOnly = false;
                    break;

                case MainViewState.Browser:

                    //按钮
                    //界面输入控件
                    btnSaveInspectionRecord.Enabled = false;
                    txtCheckPerson.Enabled = false;
                    cbWBSCheckRequir.Enabled = false;
                    cbCheckConclusion.Enabled = false;
                    dtpCheckDate.Enabled = false;
                    txtCheckStatus.Enabled = false;
                    btnSubmit.Enabled = false;
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    txtSupply.Enabled = false;
                    this.btnDelete.Enabled = true;
                    txtCheckPerson1.Enabled = false;
                    cbWBSCheckRequir1.Enabled = false;
                    cbCheckConclusion1.Enabled = false;
                    dtpCheckDate1.Enabled = false;
                    txtCheckStatus1.Enabled = false;
                    btnSubmit1.Enabled = false;
                    radioButton3.Enabled = false;
                    radioButton4.Enabled = false;
                    txtSupply1.Enabled = false;
                    //this.btnSave.Enabled = false;
                    this.btnSave1.Enabled = false;
                    this.btnDelete.Enabled = false;
                    this.btnDelete1.Enabled = false;
                    dgDocumentDetail.ReadOnly = false;
                    FileSelect.ReadOnly = false;
                    break;
            }

            ViewState = state;
        }

        #region 操作按钮
        //保存
        void btnSave_Click(object sender, EventArgs e)
        {
            IsSubmit = false;
            mnuTree.Hide();
            if (!SaveView()) return;
            MessageBox.Show("保存成功！");
            this.RefreshControls(MainViewState.Browser);
        }

        //选择工程任务类型
        void btnSelectWBSType_Click(object sender, EventArgs e)
        {
            VSelectPBSAndTaskType frm = new VSelectPBSAndTaskType();
            frm.IsSingleSelectTaskType = true;

            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("ThePBS", NHibernate.FetchMode.Eager);
            Disjunction dis = new Disjunction();

            TreeNode parentTreeNode = null;
            GWBSTree parentNode = null;

            if (IsInsertNode || ViewState == MainViewState.Modify)//修改节点
            {
                parentTreeNode = currNode.Parent;
                if (parentTreeNode != null)
                {
                    parentNode = parentTreeNode.Tag as GWBSTree;
                    parentNode = LoadRelaAttribute(parentNode);
                    parentTreeNode.Tag = parentNode;
                }

            }
            else
            {
                parentTreeNode = currNode;
                parentNode = parentTreeNode.Tag as GWBSTree;
                parentNode = LoadRelaAttribute(parentNode);
                parentTreeNode.Tag = parentNode;
            }
            if (parentNode != null)
            {
                foreach (GWBSRelaPBS rela in parentNode.ListRelaPBS)
                {
                    dis.Add(Expression.Eq("Id", rela.Id));
                }

                oq.AddCriterion(dis);

                IList listRela = model.ObjectQuery(typeof(GWBSRelaPBS), oq);
                foreach (GWBSRelaPBS rela in listRela)
                {
                    frm.ParentPBSType.Add(rela.ThePBS.StructTypeName);
                }

                frm.ParentTaskType = parentNode.ProjectTaskTypeGUID.TypeLevel.ToString();
                frm.ParentNode = parentTreeNode;
            }

            //设置初始选择的对象
            if (txtTaskWBSType.Tag != null)
                frm.InitTaskType = txtTaskWBSType.Tag as ProjectTaskTypeTree;
            if (cbRelaPBS.Tag != null)
            {
                frm.InitListPBS = cbRelaPBS.Tag as List<PBSTree>;
            }

            frm.ShowDialog();

            if (frm.IsOK)
            {
                List<PBSTree> listPBS = frm.SelectedPBS;
                List<ProjectTaskTypeTree> listTaskType = frm.SelectedTaskType;

                //SaveSelectNodes(frm.SelectedPBS, frm.SelectedTaskType);

                ProjectTaskTypeTree type = listTaskType[0];
                type.FullPath = GetFullPath(type);
                txtTaskWBSType.Text = type.FullPath;
                txtTaskWBSType.Tag = type;

                if (oprNode != null && string.IsNullOrEmpty(oprNode.Id))
                {
                    oprNode.CheckRequire = type.CheckRequire.PadRight(11, 'X') + "0";
                    oprNode.DailyCheckState = oprNode.CheckRequire;

                    //检查要求
                    if (!string.IsNullOrEmpty(oprNode.CheckRequire))
                    {
                        char[] chs = oprNode.CheckRequire.ToCharArray();
                        for (int i = 0; i < chs.Length; i++)
                        {
                            Char c = chs[i];
                            if (c == '0')
                            {
                                if (cbListCheckRequire.Items.Count > i)
                                    cbListCheckRequire.SetItemChecked(i, true);
                            }
                            else
                            {
                                if (cbListCheckRequire.Items.Count > i)
                                    cbListCheckRequire.SetItemChecked(i, false);
                            }
                        }
                    }
                }
                cbRelaPBS.Items.Clear();
                for (int i = 0; i < listPBS.Count; i++)
                {
                    listPBS[i].FullPath = GetFullPath(listPBS[i]);
                    cbRelaPBS.Items.Add(listPBS[i]);
                }

                cbRelaPBS.DisplayMember = "FullPath";
                cbRelaPBS.ValueMember = "Id";
                cbRelaPBS.Tag = listPBS;

                cbRelaPBS.SelectedIndex = 0;

                //RefreshControls(MainViewState.Browser);
            }
        }

        private void SetStandardUnit(object sender)
        {
            TextBox tbUnit = sender as TextBox;
            string name = tbUnit.Text.Trim();
            if (name != "")
            {
                if (tbUnit.Tag == null || (tbUnit.Tag != null && (tbUnit.Tag as StandardUnit).Name != name))
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Name", name));
                    IList list = model.ObjectQuery(typeof(StandardUnit), oq);
                    if (list.Count > 0)
                    {
                        tbUnit.Tag = list[0] as StandardUnit;
                    }
                    else
                    {
                        MessageBox.Show("系统目前不存在该计量单位，请检查！");
                        SelectUnit(tbUnit);
                    }
                }
            }
            else
                tbUnit.Tag = null;
        }
        private void SelectUnit(TextBox txt)
        {
            StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
            if (su != null)
            {
                txt.Tag = su;
                txt.Text = su.Name;
                txt.Focus();
            }
        }


        void btnSelectRelaPBS_Click(object sender, EventArgs e)
        {
            VSelectPBSNode frm = new VSelectPBSNode();
            frm.SelectMethod = SelectNodeMethod.零散节点选择;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                List<TreeNode> listSelectNode = frm.SelectResult;

                List<PBSTree> listPBS = new List<PBSTree>();
                foreach (TreeNode tn in listSelectNode)
                {
                    PBSTree pbs = tn.Tag as PBSTree;
                    listPBS.Add(pbs);
                    cbRelaPBS.Items.Add(pbs);
                }

                cbRelaPBS.DisplayMember = "Name";
                cbRelaPBS.ValueMember = "Id";

                List<PBSTree> listOld = cbRelaPBS.Tag as List<PBSTree>;
                if (listOld != null && listOld.Count > 0)
                {
                    listPBS.AddRange(listOld);
                }
                cbRelaPBS.Tag = listPBS;

                cbRelaPBS.SelectedIndex = 0;
            }
        }
        #endregion


        void txtKeyWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyData == Keys.Enter)
            {
                btnFindTaskNode_Click(btnFindTaskNode, new EventArgs());
            }
        }

        //查找/下一个
        void btnFindTaskNode_Click(object sender, EventArgs e)
        {
            if (txtKeyWord.Text.Trim() == "")
                return;

            if (listFindNodes.Count > 0)
            {
                showFindNodeIndex += 1;
                if (showFindNodeIndex > listFindNodes.Count - 1)
                    showFindNodeIndex = 0;

                ShowFindNode(listFindNodes[showFindNodeIndex]);
            }
            else
            {
                string keyWord = txtKeyWord.Text.Trim();
                if (mouseSelectNode != null)
                {
                    foreach (TreeNode tn in mouseSelectNode.Nodes)
                    {
                        if (tn.Text.IndexOf(keyWord) > -1)
                        {
                            listFindNodes.Add(tn);
                        }

                        QueryCheckedTreeNode(tn, keyWord);
                    }
                }
                else
                {
                    foreach (TreeNode tn in tvwCategory.Nodes)
                    {
                        if (tn.Text.IndexOf(keyWord) > -1)
                        {
                            listFindNodes.Add(tn);
                        }

                        QueryCheckedTreeNode(tn, keyWord);
                    }
                }

                if (listFindNodes.Count > 0)
                {
                    showFindNodeIndex = 0;
                    ShowFindNode(listFindNodes[showFindNodeIndex]);
                }
            }
        }
        private void ShowFindNode(TreeNode tn)
        {
            TreeNode theParentNode = tn.Parent;
            while (theParentNode != null)
            {
                theParentNode.Expand();
                theParentNode = theParentNode.Parent;
            }

            tvwCategory.Select();
            tvwCategory.SelectedNode = tn;
        }

        private void QueryCheckedTreeNode(TreeNode parentNode, string keyWord)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Text.IndexOf(keyWord) > -1)
                {
                    listFindNodes.Add(tn);
                }

                QueryCheckedTreeNode(tn, keyWord);
            }
        }
        void btnSelectContract_Click(object sender, EventArgs e)
        {
            VSelectWBSContractGroup frm = new VSelectWBSContractGroup(new MWBSContractGroup());
            if (!string.IsNullOrEmpty(txtContract.Text.Trim()))
                frm.DefaultSelectedContract = txtContract.Tag as ContractGroup;
            frm.ShowDialog();
            if (frm.isOK)
            {
                ContractGroup cg = frm.SelectResult[0];
                txtContract.Text = cg.ContractName;
                txtContract.Tag = cg;
            }
        }
        #region 工程量提报
        void FillProjectNew(GWBSTree tree)
        {
            //FlashScreen.Show("加载明细中.....");
            //添加工程量确认的信息
            //GWBSTaskConfirmMaster
            dgProject.Rows.Clear();
            dgProject.Tag = null;
            //dgBear.Rows.Clear();
            colProjectDetail.Visible = true;
            colUnit.Visible = true;
            colDtlState.Visible = true;
            GWBSTaskConfirm oConfirm = null;
            btnProjectBatchSave.Visible = false;
            try
            {
                //IList temp_list = model.SearchGWBSDetail(tree, ConstObject.TheLogin.ThePerson, ConstObject.TheLogin.TheOperationOrgInfo);
                IList temp_list = SearchGWBSDetail(tree);
                if (temp_list.Count > 0)
                {
                    dgProject.Tag = temp_list;
                    ShowProject();
                }

                //FlashScreen.Close();
            }
            catch (Exception ex)
            {
                //FlashScreen.Close();
                MessageBox.Show("加载wbs明细失败:" + ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {

            }
        }
        public IList SearchGWBSDetail(GWBSTree tree)
        {
            GWBSTaskConfirm confirm = null;
            GWBSDetail oDetail = null;
            PersonInfo oPersonInfo = ConstObject.TheLogin.ThePerson;
            OperationOrgInfo oOperationOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
            CurrentProjectInfo oCurrentProjectInfo = StaticMethod.GetProjectInfo();
            IList lstResult = new ArrayList();
            DataSet ds = model.SearchGWBSDetailNow(tree.SysCode, oCurrentProjectInfo.Id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow oRow in ds.Tables[0].Rows)
                {
                    oDetail = new GWBSDetail()
                    {
                        Id = ClientUtil.ToString(oRow["gwbsdetailid"]),
                        Name = ClientUtil.ToString(oRow["gwbsdetailname"]),
                        MainResourceTypeName = ClientUtil.ToString(oRow["MainResourceTypeName"]),
                        MainResourceTypeSpec = ClientUtil.ToString(oRow["MainResourceTypeSpec"]),
                        DiagramNumber = ClientUtil.ToString(oRow["DiagramNumber"]),
                        State = (DocumentState)ClientUtil.ToInt(oRow["State"]),
                        WorkAmountUnitName = ClientUtil.ToString(oRow["WorkAmountUnitName"]),
                        ContractGroupName = ClientUtil.ToString(oRow["ContractGroupName"]),
                        ContractGroupGUID = ClientUtil.ToString(oRow["ContractGroupGUID"]),
                        OrderNo = ClientUtil.ToInt(oRow["OrderNo"]),
                        PlanWorkAmount = ClientUtil.ToDecimal(oRow["PlanWorkAmount"]),
                        GWBSTaskConfirmDetail = null,
                        TheGWBS=tree,
                        TheGWBSSysCode=tree.SysCode,
                        TheGWBSFullPath = tree.FullPath,
                    };
                    if (oRow["costItemID"] != DBNull.Value)
                    {
                        oDetail.TheCostItem = new CostItem()
                        {
                            Id = ClientUtil.ToString(oRow["costItemID"]),
                            QuotaCode = ClientUtil.ToString(oRow["QuotaCode"])
                        };
                    }
                    if (oRow["confirmDetialID"] != DBNull.Value)
                    {
                        oDetail.GWBSTaskConfirmDetail = new GWBSTaskConfirm()
                        {
                            Id = ClientUtil.ToString(oRow["confirmDetialID"]),
                            GWBSDetail = oDetail,
                            GWBSDetailName = oDetail.Name,
                            CreatePersonName = ClientUtil.ToString(oRow["CreatePersonName"]),
                            TaskHandlerName = ClientUtil.ToString(oRow["TaskHandlerName"]),
                            MaterialFeeSettlementFlag = (EnumMaterialFeeSettlementFlag)ClientUtil.ToInt(oRow["MaterialFeeSettlementFlag"]),
                            CompletedPercent = ClientUtil.ToDecimal(oRow["CompletedPercent"]),
                            ProgressBeforeConfirm = ClientUtil.ToDecimal(oRow["ProgressBeforeConfirm"]),
                            ProgressAfterConfirm = ClientUtil.ToDecimal(oRow["ProgressAfterConfirm"]),
                            RealOperationDate = ClientUtil.ToDateTime(oRow["RealOperationDate"]),
                            QuantityBeforeConfirm = ClientUtil.ToDecimal(oRow["QuantityBeforeConfirm"]),
                            QuantiyAfterConfirm = ClientUtil.ToDecimal(oRow["QuantiyAfterConfirm"]),
                            Descript = ClientUtil.ToString(oRow["Descript"]),
                            AccountingState = (EnumGWBSTaskConfirmAccountingState)ClientUtil.ToInt(oRow["AccountingState"]),
                            ActualCompletedQuantity = ClientUtil.ToDecimal(oRow["ActualCompletedQuantity"]),
                            GWBSTree=tree,
                            GwbsSysCode=tree.SysCode,
                            GWBSTreeName=tree.Name
                            ///
                        };
                    }
                    else if (oRow["obsid"] != DBNull.Value)
                    {
                        oDetail.GWBSTaskConfirmDetail = new GWBSTaskConfirm()
                        {
                            GWBSDetail = oDetail,
                            GWBSDetailName = oDetail.Name,
                            TaskHandlerName = ClientUtil.ToString(oRow["SupplierName"]),
                            TempData3 = ClientUtil.ToString(oRow["obsid"]),//存储obsid
                            AccountingState = EnumGWBSTaskConfirmAccountingState.未核算,
                            RealOperationDate = DateTime.Now,
                            GWBSTree = tree,
                            GwbsSysCode = tree.SysCode,
                            GWBSTreeName = tree.Name
                        };
                    }
                    lstResult.Add(oDetail);
                }
            }
            if (lstResult != null)
            {
                IList lstTemp = lstResult.OfType<GWBSDetail>().Where(a => (a.GWBSTaskConfirmDetail != null && a.GWBSTaskConfirmDetail.TempData3 == null)).ToList();

                foreach (GWBSDetail oGWBSDetail in lstTemp)
                {
                    confirm = oGWBSDetail.GWBSTaskConfirmDetail;
                    confirm.CreatePerson = oPersonInfo;//制单人编号
                    confirm.CreatePersonName = oPersonInfo.Name;//制单人名称
                    confirm.OperOrgInfo = oOperationOrgInfo;//登录业务组织
                    confirm.OperOrgInfoName = oOperationOrgInfo.Name;//业务组织名称
                    confirm.OpgSysCode = oOperationOrgInfo.SysCode;//业务组织编号
                }
            }
            return lstResult;
        }
        IList<GWBSDetail> FilterProject()
        {
            string sValue = string.Empty;
            IList lstTemp;
            IList<GWBSDetail> lstResult = null;
            if (dgProject.Tag != null)
            {
                lstTemp = dgProject.Tag as IList;
                if (lstTemp != null && lstTemp.Count > 0)
                {
                    lstResult = lstTemp.OfType<GWBSDetail>().ToList();
                    sValue = this.txtDECode.Text.Trim();
                    if (!string.IsNullOrEmpty(sValue))
                    {
                        lstResult = lstResult.Where(a => (a.TheCostItem != null && !string.IsNullOrEmpty(a.TheCostItem.QuotaCode) && a.TheCostItem.QuotaCode.Contains(sValue))).ToList();
                    }
                    sValue = this.txtDtlName.Text.Trim();
                    if (!string.IsNullOrEmpty(sValue))
                    {
                        if (lstResult != null && lstResult.Count > 0)
                        {
                            lstResult = lstResult.Where(a => (!string.IsNullOrEmpty(a.Name) && a.Name.Contains(sValue))).ToList();
                        }
                    }
                    sValue = txtCDZ.Text.Trim();
                    if (!string.IsNullOrEmpty(sValue))
                    {
                        if (lstResult != null && lstResult.Count > 0)
                        {
                            lstResult = lstResult.Where(a => (a.GWBSTaskConfirmDetail != null && a.GWBSTaskConfirmDetail.TaskHandlerName.Contains(sValue))).ToList();
                        }
                    }
                    sValue = txtResource.Text.Trim();
                    if (!string.IsNullOrEmpty(sValue))
                    {
                        if (lstResult != null && lstResult.Count > 0)
                        {//MainResourceTypeName MainResourceTypeSpec DiagramNumber
                            lstResult = lstResult.Where(a => (
                               (!string.IsNullOrEmpty(a.MainResourceTypeName) && a.MainResourceTypeName.Contains(sValue)) ||
                               (!string.IsNullOrEmpty(a.MainResourceTypeSpec) && a.MainResourceTypeSpec.Contains(sValue)) ||
                               (!string.IsNullOrEmpty(a.DiagramNumber) && a.DiagramNumber.Contains(sValue))
                                )).ToList();
                        }
                    }
                    sValue = txtContract.Text.Trim();
                    if (!string.IsNullOrEmpty(sValue))
                    {
                        ContractGroup oContractGroup = txtContract.Tag as ContractGroup;
                        if (oContractGroup != null && lstResult != null && lstResult.Count > 0)
                        {
                            lstResult = lstResult.Where(a => string.Equals(a.ContractGroupGUID, oContractGroup.Id)).ToList();
                        }
                    }
                }
            }
            return lstResult;
        }
        void ShowProject()
        {
            GWBSTaskConfirm oConfirm;
            dgProject.Rows.Clear();
            IList<GWBSDetail> temp_list = FilterProject();
            if (temp_list != null)
            {
                foreach (GWBSDetail detail in temp_list)
                {
                    int rowIndex = dgProject.Rows.Add();
                    dgProject[colProjectDetail.Name, rowIndex].Value = detail.Name;
                    if (detail.TheCostItem != null)
                    {
                        dgProject[colDECode.Name, rowIndex].Value = detail.TheCostItem.QuotaCode;//定额编号
                    }
                    dgProject[colMainResType.Name, rowIndex].Value = detail.MainResourceTypeName;
                    dgProject[colSpe.Name, rowIndex].Value = detail.MainResourceTypeSpec;
                    dgProject[colTH.Name, rowIndex].Value = detail.DiagramNumber;
                    dgProject[colDtlState.Name, rowIndex].Value = StaticMethod.GetWBSTaskStateText(detail.State);
                    dgProject[colUnit.Name, rowIndex].Value = detail.WorkAmountUnitName;
                    dgProject[colUnit.Name, rowIndex].Tag = detail.WorkAmountUnitGUID;
                    dgProject[colContractGroup.Name, rowIndex].Value = detail.ContractGroupName;
                    dgProject.Rows[rowIndex].Tag = detail;
                    if (detail.GWBSTaskConfirmDetail != null)
                    {
                        oConfirm = detail.GWBSTaskConfirmDetail;
                        dgProject[colGZH.Name, rowIndex].Value = oConfirm.CreatePersonName;
                        dgProject[colGZH.Name, rowIndex].Tag = oConfirm.CreatePerson;
                        dgProject[colCHDZH.Name, rowIndex].Value = oConfirm.TaskHandlerName;
                        dgProject[colMaterialSinge.Name, rowIndex].Value = EnumUtil<EnumMaterialFeeSettlementFlag>.GetDescription(oConfirm.MaterialFeeSettlementFlag);
                        dgProject[colBCTotalXXJD.Name, rowIndex].Value = ClientUtil.ToString(oConfirm.CompletedPercent + oConfirm.ProgressBeforeConfirm);//ClientUtil.ToString(ClientUtil.ToDecimal(dgProject[colBCXXJD.Name, rowIndex].Value) + ClientUtil.ToDecimal(dgProject[colXXJDBefore.Name, rowIndex].Value));
                        dgProject[colBCTotalXXJD.Name, rowIndex].Tag = dgProject[colBCTotalXXJD.Name, rowIndex].Value;
                        dgProject[colLastDate.Name, rowIndex].Value = oConfirm.RealOperationDate;
                        dgProject[colDescription.Name, rowIndex].Value = oConfirm.Descript;
                        if (oConfirm.AccountingState == EnumGWBSTaskConfirmAccountingState.己核算)
                        {
                            dgProject[colBCTotalXXJD.Name, rowIndex].ReadOnly = true;
                            dgProject[this.colProjectSelect.Name, rowIndex].ReadOnly = true;
                            dgProject[this.colDescription.Name, rowIndex].ReadOnly = true;
                            dgProject[colProjectSelect.Name, rowIndex].ToolTipText = "该wbs任务明细已经己核算";
                        }
                    }
                    else
                    {
                        dgProject[colBCTotalXXJD.Name, rowIndex].ReadOnly = true;
                        dgProject[this.colProjectSelect.Name, rowIndex].ReadOnly = true;
                        dgProject[this.colDescription.Name, rowIndex].ReadOnly = true;
                        dgProject[colProjectSelect.Name, rowIndex].ToolTipText = "OBS中没关联承担者";


                    }
                }
            }
        }
        void btnProjectBatchSave_Click(object sender, EventArgs e)
        {
            GWBSDetail oGWBSDetail = null;
            GWBSTaskConfirm confirm = null;
            IList lstConfirm = new ArrayList();
            StringBuilder oBuilder = new StringBuilder();
            decimal dBCTotalXXJD = 0;
            string sDescript = string.Empty;
            try
            {
                if (this.dgProject.Rows.Count == 0) throw new Exception("任务明细为空");
                // if (MessageBox.Show("是否保存当前修改", "是否保存", MessageBoxButtons.YesNo) == DialogResult.Yes)
                //{
                foreach (DataGridViewRow oRow in this.dgProject.Rows)
                {
                    if (oRow.Cells[this.colProjectSelect.Name].Tag != null && ClientUtil.ToBool(oRow.Cells[this.colProjectSelect.Name].Tag) == true)
                    {
                        oGWBSDetail = oRow.Tag as GWBSDetail;

                        // if (ClientUtil.ToBool(oRow.Cells[colProjectSelect.Name].Value) && oRow.Cells[colBCTotalXXJD.Name].Tag != null)
                        if (oGWBSDetail.GWBSTaskConfirmDetail != null)
                        {
                            confirm = oGWBSDetail.GWBSTaskConfirmDetail;
                            dBCTotalXXJD = ClientUtil.ToDecimal(oRow.Cells[colBCTotalXXJD.Name].Value);
                            sDescript = ClientUtil.ToString(oRow.Cells[colDescription.Name].Value);
                            sDescript = (sDescript == null ? "" : sDescript);
                            // if (dBCTotalXXJD != ClientUtil.ToDecimal(oRow.Cells[colBCTotalXXJD.Name].Tag) || sDescript != ClientUtil.ToString(oRow.Cells[colDescription.Name].Tag))
                            //if (oRow.Cells[this.colProjectSelect.Name].Tag != null && ClientUtil.ToBool(oRow.Cells[this.colProjectSelect.Name].Tag) == true)
                            //{
                            lstConfirm.Add(confirm);
                            confirm.TempData = confirm.ActualCompletedQuantity.ToString();
                            //confirm.MaterialFeeSettlementFlag = EnumUtil<EnumMaterialFeeSettlementFlag>.FromDescription(optRow.Cells[colMaterialSinge.Name].Value);
                            confirm.CompletedPercent = dBCTotalXXJD - confirm.ProgressBeforeConfirm; ;
                            confirm.ActualCompletedQuantity = (confirm.CompletedPercent * oGWBSDetail.PlanWorkAmount) / 100;
                            confirm.QuantiyAfterConfirm = confirm.ActualCompletedQuantity + confirm.QuantityBeforeConfirm;
                            confirm.ProgressAfterConfirm = dBCTotalXXJD;
                            confirm.Descript = sDescript;
                            if (string.IsNullOrEmpty(confirm.Id))
                            {
                                confirm.Master = this.conMaster;
                                //conMaster.AddDetail(confirm);
                            }
                            //}
                        }
                    }
                    //else
                    //{
                    //    oBuilder.AppendFormat("\n[0]工程明细无劳动队伍", oGWBSDetail.Name);
                    //}
                }
                if (oBuilder.Length > 0)
                {
                    throw new Exception(oBuilder.ToString());
                }
                else
                {
                    if (lstConfirm != null && lstConfirm.Count > 0)
                    {

                        //  ValidateConfrim(lstConfirm);
                        if (MessageBox.Show("是否保存当前修改?", "是否保存", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            lstConfirm = model.SaveGWBSTaskConfirmNow(lstConfirm);
                            btnProjectBatchSave.Visible = false;

                        }


                    }
                    else
                    {
                        throw new Exception("此工程WBS明细列表尚未修改,请先进行批量设置[本次累计提报进度(%)]后,再保存");
                    }
                }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                FillProjectNew(oprNode);
            }
        }
        void ValidateConfrim(IList lstConfirmDtl)
        {
            IList lstTemp = null;
            GWBSTaskConfirm oConfirm, oTempConfirm;
            StringBuilder oBuilder = new StringBuilder();
            if (lstConfirmDtl != null && lstConfirmDtl.Count > 0)
            {
                #region 进行验证GWBS任务确认明细
                string[] arrIDs = lstConfirmDtl.OfType<GWBSTaskConfirm>().Where(a => !string.IsNullOrEmpty(a.Id)).Select(a => a.Id).ToArray<string>();
                if (arrIDs != null && arrIDs.Length > 0)
                {
                    ObjectQuery oQuery = new ObjectQuery();
                    oQuery.AddCriterion(Expression.In("Id", arrIDs));
                    lstTemp = model.ObjectQuery(typeof(GWBSTaskConfirm), oQuery);
                    if (lstTemp != null && lstTemp.Count > 0)
                    {
                        for (int i = 0; i < lstConfirmDtl.Count - 1; i++)
                        {
                            oConfirm = lstConfirmDtl[i] as GWBSTaskConfirm;
                            if (!string.IsNullOrEmpty(oConfirm.Id))
                            {
                                var var = lstTemp.OfType<GWBSTaskConfirm>().Where(a => a.Id == oConfirm.Id);
                                if (var != null && var.Count() > 0)
                                {
                                    oTempConfirm = var.ElementAt<GWBSTaskConfirm>(0);
                                    if (oTempConfirm.AccountingState == EnumGWBSTaskConfirmAccountingState.己核算)
                                    {
                                        oBuilder.AppendFormat("\n[0]此提报明细正处于商务核算过程中！", oTempConfirm.GWBSDetailName);
                                        // MessageBox.Show("此提报明细正处于商务核算过程中！");
                                        continue;
                                    }
                                    if (oTempConfirm.QuantityBeforeConfirm != oConfirm.QuantityBeforeConfirm)
                                    {
                                        oBuilder.AppendFormat("\n[0]提报明细商务已核算，请重新刷新提报！", oTempConfirm.GWBSDetailName);
                                        //MessageBox.Show("此提报明细商务已核算，请重新刷新提报！");
                                        continue;
                                    }
                                }
                            }

                        }
                    }
                }

                if (oBuilder.Length > 0)
                {
                    throw new Exception(oBuilder.ToString());
                }
            }
                #endregion
        }
        void btnProjectBatchSet_Click(object sender, EventArgs e)
        {
            try
            {
                this.dgProject.EndEdit();
                bool IsSelect = false;
                decimal dJD = 0;
                // if (MessageBox.Show("是否批量设置本次累计提报进度", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                // {
                if (dgProject.Rows.Count > 0)
                {
                    if (CommonMethod.VeryValid(txtProjectTBJD.Text))
                    {
                        dJD = ClientUtil.ToDecimal(txtProjectTBJD.Text);
                        if (dJD < 0 || dJD > 100) throw new Exception("本次累计提报进度数值范围为[0,100]");
                        //if (dJD > 0)
                        //{
                        DataGridViewCheckBoxCell oCell = null;
                        foreach (DataGridViewRow oRow in dgProject.Rows)
                        {
                            oCell = oRow.Cells[colProjectSelect.Name] as DataGridViewCheckBoxCell;
                            if (ClientUtil.ToBool(oCell.Value))
                            {

                                oRow.Cells[colBCTotalXXJD.Name].Value = dJD;
                                IsSelect = true;
                                // oRow.Cells[colBCTotalXXJD.Name].Tag = true;//标志这条记录被修改
                                oCell.Tag = true;
                                btnProjectBatchSave.Visible = true;
                            }
                        }
                        if (!IsSelect)
                        {
                            throw new Exception("请选择需要批量设置的工程WBS明细");
                        }
                        //}
                        //else
                        //{
                        //    throw new Exception("[本次累计提报进度(%)]必须大于等于0");
                        //}
                    }
                    else
                    {
                        throw new Exception("[本次累计提报进度(%)]必须为大于等于零的数值类型");
                    }
                }
                else
                {
                    throw new Exception("工程WBS明细列表为空,无法批量");
                }
                // }
            }
            catch (Exception ex)
            {
                MessageBox.Show("无法批量设置:" + ex.Message);
            }
        }
        void chkProjectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkProjectAll.Checked)
            {
                if (dgProject.Rows.Count > 0)
                {
                    DataGridViewCheckBoxCell oCell = null;
                    foreach (DataGridViewRow oRow in dgProject.Rows)
                    {
                        oCell = oRow.Cells[colProjectSelect.Name] as DataGridViewCheckBoxCell;

                        if (!oCell.ReadOnly)
                        {
                            oCell.Value = true;
                        }
                    }
                }
            }
        }
        void chkProjectNot_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkProjectNot.Checked)
            {
                if (dgProject.Rows.Count > 0)
                {
                    DataGridViewCheckBoxCell oCell = null;
                    foreach (DataGridViewRow oRow in dgProject.Rows)
                    {
                        oCell = oRow.Cells[colProjectSelect.Name] as DataGridViewCheckBoxCell;
                        if (!oCell.ReadOnly)
                        {
                            oCell.Value = !ClientUtil.ToBool(oCell.Value);
                        }
                    }
                }
            }
        }
        private void dgProject_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                string colName = dgProject.Columns[e.ColumnIndex].Name;

                if (colName == colBCTotalXXJD.Name)
                {
                    //本次累计提报形象进度
                    //this.dgProject.CurrentRow.Cells[colBCXXJD.Name].Value = (ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colBCTotalXXJD.Name].Value) - ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colXXJDBefore.Name].Value)).ToString("0.00");
                    ////this.dgProject.CurrentRow.Cells[colQuantityBC.Name].Value = (ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) * ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colBCXXJD.Name].Value) / 100).ToString("0.00");
                    //this.dgProject.CurrentRow.Cells[colQuantityBC.Name].Value = 0;
                    //decimal num = ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) * ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colBCXXJD.Name].Value) / (decimal)100;
                    //this.dgProject.CurrentRow.Cells[colGZHTBL.Name].Value = (num + ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colQuantityBefore.Name].Value)).ToString("0.00");
                    string sTemp = ClientUtil.ToString(this.dgProject.Rows[e.RowIndex].Cells[colBCTotalXXJD.Name].Value);
                    if (string.IsNullOrEmpty(sTemp))
                    {

                        throw new Exception("[累计提报形象进度(%)]不能为空，请输入数值型");
                    }
                    if (!CommonMethod.VeryValid(sTemp))
                    {
                        throw new Exception("请输入数值型[累计提报形象进度(%)]");
                    }
                }
                if (colName == this.colDescription.Name || colName == this.colBCTotalXXJD.Name)
                {
                    this.dgProject.Rows[e.RowIndex].Cells[this.colProjectSelect.Name].Tag = true;//标志这条记录被修改
                    btnProjectBatchSave.Visible = true;
                }

            }
            catch (Exception ex)
            {
                this.dgProject.Rows[e.RowIndex].Cells[colBCTotalXXJD.Name].Value = this.dgProject.Rows[e.RowIndex].Cells[colBCTotalXXJD.Name].Tag;
                MessageBox.Show(ex.Message);
                this.dgProject.Rows[e.RowIndex].Cells[colBCTotalXXJD.Name].Selected = true;
                dgProject.BeginEdit(false);
            }
            finally
            {

            }
        }
        void btnQueryProject_Click(object sender, EventArgs e)
        {
            try
            {
                ShowProject();

            }
            catch (Exception ex)
            {
                MessageBox.Show("查询WBS明细失败:" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        #region
        //void dgProject_SelectionChanged(object sender, EventArgs e)
        //{
        //   // dgBear.Rows.Clear();
        //    GWBSDetail gDetail = new GWBSDetail();
        //    if (dgProject.CurrentRow != null)
        //    {
        //        gDetail = dgProject.CurrentRow.Tag as GWBSDetail;
        //        if (gDetail != null)
        //        {
        //            //if (gDetail.State == DocumentState.InExecute)
        //            //{
        //            ObjectQuery oq = new ObjectQuery();
        //            oq.AddCriterion(Expression.Eq("Master.Id", conMaster.Id));
        //            oq.AddCriterion(Expression.Eq("GWBSDetail.Id", gDetail.Id));
        //            oq.AddCriterion(Expression.Eq("Master.BillType", EnumConfirmBillType.虚拟工单));//【状态】=9.虚拟工程任务确认单
        //            oq.AddFetchMode("GWBSDetail", NHibernate.FetchMode.Eager);
        //            oq.AddFetchMode("Master", NHibernate.FetchMode.Eager);
        //            oq.AddFetchMode("TaskHandler", NHibernate.FetchMode.Eager);
        //            oq.AddFetchMode("TaskHandler.TheContractGroup", NHibernate.FetchMode.Eager);
        //            oq.AddOrder(Order.Asc("RealOperationDate"));
        //            IList temp_list = model.ObjectQuery(typeof(GWBSTaskConfirm), oq);

        //            if (temp_list.Count > 0)
        //            {
        //                //GWBSTaskConfirm confirm = temp_list[0] as GWBSTaskConfirm;
        //                //this.txtCHDZH.Text = confirm.TaskHandlerName;
        //                //this.txtCHDZH.Tag = confirm.TaskHandler;

        //                foreach (GWBSTaskConfirm dtl in temp_list)
        //                {
        //                    int rowIndex = dgBear.Rows.Add();
        //                    dgBear[colGZH.Name, rowIndex].Value = dtl.CreatePersonName;
        //                    dgBear[colGZH.Name, rowIndex].Tag = dtl.CreatePerson;
        //                    dgBear[colCHDZH.Name, rowIndex].Value = dtl.TaskHandlerName;
        //                    if (dtl.TaskHandler != null)
        //                    {
        //                        dgBear[colCHDZH.Name, rowIndex].Tag = dtl.TaskHandler;
        //                        dgBear[colQYName.Name, rowIndex].Value = dtl.TaskHandler.ContractGroupCode;
        //                    }

        //                    dgBear[colMaterialSinge.Name, rowIndex].Value = EnumUtil<EnumMaterialFeeSettlementFlag>.GetDescription(dtl.MaterialFeeSettlementFlag);
        //                    dgBear[colQuantityBefore.Name, rowIndex].Value = dtl.QuantityBeforeConfirm;
        //                    dgBear[colQuantityBC.Name, rowIndex].Value = 0;//dtl.ActualCompletedQuantity;
        //                    dgBear[colXXJDBefore.Name, rowIndex].Value = dtl.ProgressBeforeConfirm;
        //                    dgBear[colGZHTBL.Name, rowIndex].Value = dtl.QuantiyAfterConfirm;
        //                    dgBear[colBCXXJD.Name, rowIndex].Value = dtl.CompletedPercent;
        //                    dgBear[colBCTotalXXJD.Name, rowIndex].Value = ClientUtil.ToString(ClientUtil.ToDecimal(dgBear[colBCXXJD.Name, rowIndex].Value) + ClientUtil.ToDecimal(dgBear[colXXJDBefore.Name, rowIndex].Value));
        //                    dgBear[colLastDate.Name, rowIndex].Value = dtl.RealOperationDate;
        //                    dgBear[colDescription.Name, rowIndex].Value = dtl.Descript;
        //                    if (dtl.AccountingState == EnumGWBSTaskConfirmAccountingState.己核算)
        //                    {
        //                        dgBear[colAccState.Name, rowIndex].Value = "核算中";
        //                    }
        //                    if (dtl.AccountingState == EnumGWBSTaskConfirmAccountingState.未核算)
        //                    {
        //                        dgBear[colAccState.Name, rowIndex].Value = "未核算";
        //                    }
        //                    dgBear.Rows[rowIndex].Tag = dtl;
        //                    this.dgBear.Rows[rowIndex].ReadOnly = true;
        //                }
        //            }
        //            if (dgBear.Rows.Count > 0)
        //            {
        //                this.btnUpdate.Enabled = true;
        //            }
        //            else
        //            {
        //                confirm = new GWBSTaskConfirm();
        //            }
        //            if (SavedGWBSDetail == null || SavedGWBSDetail != gDetail)
        //            {
        //                btnProject_Click(null, null);
        //            }
        //            SavedGWBSDetail = null;
        //            // }
        //        }
        //    }
        //}

        //void btnUpdate_Click(object sender, EventArgs e)
        //{
        //    if (dgBear.CurrentRow != null)
        //    {
        //        GWBSTaskConfirm firm = dgBear.CurrentRow.Tag as GWBSTaskConfirm;
        //        if (firm.AccountingState != EnumGWBSTaskConfirmAccountingState.己核算)
        //        {
        //            if (ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colQuantityBefore.Name].Value) != 0)
        //            {
        //                this.dgBear.CurrentRow.Cells[colMaterialSinge.Name].ReadOnly = true;
        //            }
        //            else
        //            {
        //                this.dgBear.CurrentRow.Cells[colMaterialSinge.Name].ReadOnly = false;
        //            }
        //            this.dgBear.CurrentRow.Cells[colCHDZH.Name].ReadOnly = true;
        //            this.btnUpdate.Enabled = false;
        //            this.dgBear.CurrentRow.ReadOnly = false;
        //        }
        //        else
        //        {
        //            MessageBox.Show("此提报信息正在核算中，不可修改！");
        //        }
        //    }
        //}

        //////void dgBear_SelectionChanged(object sender, EventArgs e)
        //////{
        //////    if (!this.btnUpdate.Enabled)
        //////    {
        //////        foreach (DataGridViewRow var in this.dgBear.Rows)
        //////        {
        //////            if (var.IsNewRow) break;
        //////            if (!var.ReadOnly)
        //////            {
        //////                if (MessageBox.Show("有信息还未保存，是否保存？", "是", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
        //////                {
        //////                    btnSubmitProject_Click(sender, e);
        //////                    var.ReadOnly = true;
        //////                }
        //////                else
        //////                {
        //////                    GWBSTaskConfirm dtl = var.Tag as GWBSTaskConfirm;
        //////                    if (dtl == null)
        //////                    {
        //////                        return;
        //////                    }
        //////                    var.Cells[colGZH.Name].Value = dtl.CreatePersonName;
        //////                    var.Cells[colGZH.Name].Tag = dtl.CreatePerson;
        //////                    var.Cells[colCHDZH.Name].Value = dtl.TaskHandlerName;
        //////                    var.Cells[colCHDZH.Name].Tag = dtl.TaskHandler;
        //////                    var.Cells[colMaterialSinge.Name].Value = EnumUtil<EnumMaterialFeeSettlementFlag>.GetDescription(dtl.MaterialFeeSettlementFlag);
        //////                    var.Cells[colQuantityBefore.Name].Value = dtl.QuantityBeforeConfirm;
        //////                    var.Cells[colQuantityBC.Name].Value = 0;//dtl.ActualCompletedQuantity;
        //////                    var.Cells[colXXJDBefore.Name].Value = dtl.ProgressBeforeConfirm;
        //////                    var.Cells[colGZHTBL.Name].Value = dtl.QuantiyAfterConfirm;
        //////                    var.Cells[colBCXXJD.Name].Value = dtl.CompletedPercent;
        //////                    var.Cells[colLastDate.Name].Value = dtl.RealOperationDate;
        //////                    var.Cells[colDescription.Name].Value = dtl.Descript;
        //////                    if (dtl.AccountingState == EnumGWBSTaskConfirmAccountingState.己核算)
        //////                    {
        //////                        var.Cells[colAccState.Name].Value = "核算中";
        //////                    }
        //////                    if (dtl.AccountingState == EnumGWBSTaskConfirmAccountingState.未核算)
        //////                    {
        //////                        var.Cells[colAccState.Name].Value = "未核算";
        //////                    }
        //////                    var.ReadOnly = true;
        //////                }
        //////                this.btnUpdate.Enabled = true;
        //////                break;
        //////            }
        //////        }
        //////    }
        //////}

        //void btnAddProject_Click(object sender, EventArgs e)
        //{
        //    if (dgProject.Rows.Count > 0)
        //    {
        //        if (dgBear.Rows.Count > 0)
        //        {
        //            if ((dgBear.Rows[dgBear.Rows.Count - 1].Tag as GWBSTaskConfirm).Id != null)
        //            {
        //                int i = dgBear.Rows.Add();
        //                foreach (DataGridViewRow var in this.dgBear.Rows)
        //                {
        //                    for (int j = 0; j < dgBear.Rows.Count - 1; j++)
        //                    {
        //                        dgBear.Rows[j].Selected = false;
        //                    }
        //                }
        //                dgBear.Rows[i].Selected = true;
        //                confirm = new GWBSTaskConfirm();
        //                confirm.GWBSDetail = dgProject.CurrentRow.Tag as GWBSDetail;
        //                confirm.GWBSDetailName = dgProject.CurrentRow.Cells[colProjectDetail.Name].Value.ToString();
        //                dgBear.Rows[i].Tag = confirm;
        //            }
        //        }
        //        else
        //        {
        //            int i = dgBear.Rows.Add();
        //            confirm = new GWBSTaskConfirm();
        //            confirm.GWBSDetail = dgProject.CurrentRow.Tag as GWBSDetail;
        //            confirm.GWBSDetailName = dgProject.CurrentRow.Cells[colProjectDetail.Name].Value.ToString();
        //            dgBear.Rows[i].Tag = confirm;
        //        }
        //    }
        //}

        //void btnSubmitProject_Click(object sender, EventArgs e)
        //{
        //    if (this.dgBear.Rows.Count == 0) return;
        //    if (this.dgBear.CurrentRow.Cells[colCHDZH.Name].Value == null)
        //    {
        //        MessageBox.Show("承担者不能为空！");
        //        return;
        //    }
        //    if (this.dgBear.CurrentRow.Cells[colMaterialSinge.Name].Value == null)
        //    {
        //        MessageBox.Show("料费结算标记不能为空！");
        //        return;
        //    }
        //    if (ClientUtil.ToDecimal(this.dgBear.CurrentRow.Cells[colGZHTBL.Name].Value) < 0)//ClientUtil.ToDecimal(this.dgBear.CurrentRow.Cells[colQuantityBefore.Name].Value))
        //    {
        //        MessageBox.Show("工长累计提报工程量应不小于零！");
        //        return;
        //    }

        //    int optRowIndex = -1;
        //    confirm = new GWBSTaskConfirm();

        //    foreach (DataGridViewRow var in this.dgBear.Rows)
        //    {
        //        if (!var.ReadOnly)
        //        {
        //            optRowIndex = var.Index;
        //            break;
        //        }
        //    }
        //    if (optRowIndex == -1)
        //        return;

        //    DataGridViewRow optRow = dgBear.Rows[optRowIndex];
        //    if (optRow.Tag != null)
        //    {
        //        confirm = optRow.Tag as GWBSTaskConfirm;
        //    }
        //    if (confirm.Id != null)
        //    {
        //        //判断工程任务提报明细状态
        //        GWBSTaskConfirm q_dtl = model.GetObjectById(typeof(GWBSTaskConfirm), confirm.Id) as GWBSTaskConfirm;
        //        if (q_dtl.AccountingState == EnumGWBSTaskConfirmAccountingState.己核算)
        //        {
        //            MessageBox.Show("此提报明细正处于商务核算过程中！");
        //            return;
        //        }
        //        if (q_dtl.QuantityBeforeConfirm != confirm.QuantityBeforeConfirm)
        //        {
        //            MessageBox.Show("此提报明细商务已核算，请重新刷新提报！");
        //            return;
        //        }
        //    }

        //    //存储修改前确认的量，用于累计任务明细的总确认量
        //    confirm.TempData = "";
        //    if (btnUpdate.Enabled == false)
        //        confirm.TempData = confirm.ActualCompletedQuantity.ToString();

        //    confirm.Master = conMaster;
        //    confirm.CostItem = (dgProject.CurrentRow.Tag as GWBSDetail).TheCostItem;
        //    confirm.CostItemName = (dgProject.CurrentRow.Tag as GWBSDetail).TheCostItem.Name;
        //    confirm.WorkAmountUnitId = (dgProject.CurrentRow.Tag as GWBSDetail).WorkAmountUnitGUID;
        //    confirm.WorkAmountUnitName = (dgProject.CurrentRow.Tag as GWBSDetail).WorkAmountUnitName;

        //    confirm.TaskHandler = optRow.Cells[colCHDZH.Name].Tag as SubContractProject;
        //    confirm.TaskHandlerName = ClientUtil.ToString(optRow.Cells[colCHDZH.Name].Value);
        //    confirm.MaterialFeeSettlementFlag = EnumUtil<EnumMaterialFeeSettlementFlag>.FromDescription(optRow.Cells[colMaterialSinge.Name].Value);
        //    confirm.ActualCompletedQuantity = ClientUtil.ToDecimal(this.dgBear.CurrentRow.Cells[colGZHTBL.Name].Value) - ClientUtil.ToDecimal(this.dgBear.CurrentRow.Cells[colQuantityBefore.Name].Value);//ClientUtil.ToDecimal(optRow.Cells[colQuantityBC.Name].Value);
        //    confirm.QuantiyAfterConfirm = ClientUtil.ToDecimal(optRow.Cells[colGZHTBL.Name].Value);
        //    confirm.ProgressAfterConfirm = ClientUtil.ToDecimal(optRow.Cells[colBCTotalXXJD.Name].Value);
        //    //confirm.ProgressBeforeConfirm = ClientUtil.ToDecimal(optRow.Cells[colXXJDBefore.Name].Value);
        //    confirm.CompletedPercent = ClientUtil.ToDecimal(optRow.Cells[colBCXXJD.Name].Value);
        //    confirm.Descript = optRow.Cells[colDescription.Name].Value == null ? "" : optRow.Cells[colDescription.Name].Value.ToString();
        //    //校验
        //    foreach (DataGridViewRow var in this.dgBear.Rows)
        //    {
        //        if (var.Index != optRowIndex)
        //        {
        //            GWBSTaskConfirm firm = var.Tag as GWBSTaskConfirm;
        //            if (firm.TaskHandler != null && confirm.TaskHandler != null && firm.TaskHandler.Id == confirm.TaskHandler.Id
        //                && firm.MaterialFeeSettlementFlag == confirm.MaterialFeeSettlementFlag &&
        //                confirm.CreatePerson != null && firm.CreatePerson.Id == confirm.CreatePerson.Id)
        //            {
        //                MessageBox.Show("工长、承担者、料费结算标记相同的提报信息已经存在，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                return;
        //            }
        //        }
        //    }

        //    confirm = model.SaveGWBSTaskConfirm(confirm);
        //    this.btnUpdate.Enabled = true;
        //    MessageBox.Show("保存成功！");
        //    LogData log = new LogData();
        //    log.ProjectID = projectInfo.Id;
        //    log.OperType = "保存";
        //    log.BillId = confirm.Id;
        //    log.BillType = "工程量提报";
        //    log.Code = "";
        //    log.Descript = "节点:["+confirm.GWBSTreeName + "],明细:["+confirm.GWBSDetailName+"]";
        //    log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
        //    log.ProjectName = projectInfo.Name;
        //    StaticMethod.InsertLogData(log);
        //    updateCurrRow();
        //    SavedGWBSDetail = dgProject.CurrentRow.Tag as GWBSDetail;
        //    dgProject_SelectionChanged(dgProject, new DataGridViewCellEventArgs(0, 0));
        //    BackSchedule(confirm);
        //}
        #endregion
        public void BackSchedule(GWBSTaskConfirm confirm)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("GWBSTree.Id", confirm.GWBSTree.Id));
            IList list = model.ObjectQuery(typeof(ProductionScheduleDetail), oq);

            IList listPro = new ArrayList();
            foreach (ProductionScheduleDetail dtl in list)
            {
                if (dtl.GWBSTree.Id == confirm.GWBSTree.Id)
                {
                    dtl.ActualBeginDate = confirm.RealOperationDate;
                    if (confirm.GWBSTree.AddUpFigureProgress > 101)
                    {
                        dtl.ActualEndDate = ClientUtil.ToDateTime(confirm.GWBSTree.RealEndDate);
                        dtl.ActualDuration = (dtl.ActualEndDate - dtl.ActualBeginDate).Days;
                    }
                    listPro.Add(dtl);
                }
            }
            if (listPro.Count > 0)
            {
                model.SaveOrUpdate(listPro);
            }
        }

        ////void btnDeleteProject_Click(object sender, EventArgs e)
        ////{
        ////    if (dgBear.Rows.Count > 0 && dgBear.CurrentRow != null)
        ////    {
        ////        if (MessageBox.Show("确定要删除当前选中的记录吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
        ////        {
        ////            GWBSTaskConfirm firm = dgBear.CurrentRow.Tag as GWBSTaskConfirm;
        ////            if (firm != null && firm.Id == null)
        ////            {
        ////                dgBear.Rows.Remove(dgBear.CurrentRow);
        ////                return;
        ////            }
        ////            ObjectQuery oq = new ObjectQuery();
        ////            oq.AddCriterion(Expression.Eq("Id", firm.Id));
        ////            oq.AddFetchMode("GWBSDetail", NHibernate.FetchMode.Eager);
        ////            IList temp_list = model.ObjectQuery(typeof(GWBSTaskConfirm), oq);
        ////            firm = temp_list[0] as GWBSTaskConfirm;
        ////            if (firm.AccountingState != EnumGWBSTaskConfirmAccountingState.己核算 && firm.QuantityBeforeConfirm == 0)
        ////            {
        ////                model.DeleteGWBSTaskConfirm(firm);
        ////                dgBear.Rows.Remove(dgBear.CurrentRow);
        ////                updateCurrRow();
        ////            }
        ////            else
        ////            {
        ////                MessageBox.Show("信息已核算，不可删除！");
        ////            }
        ////        }
        ////    }
        ////}


        //void updateCurrRow()
        //{
        //    ObjectQuery oq = new ObjectQuery();
        //    oq.AddCriterion(Expression.Eq("Id", (dgProject.CurrentRow.Tag as GWBSDetail).Id));
        //    oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
        //    IList temp_list = model.ObjectQuery(typeof(GWBSDetail), oq);
        //    {
        //        GWBSDetail detail = temp_list[0] as GWBSDetail;
        //        dgProject.CurrentRow.Cells[colMainResType.Name].Value = detail.MainResourceTypeName;
        //        dgProject.CurrentRow.Cells[colSpe.Name].Value = detail.MainResourceTypeSpec;
        //        dgProject.CurrentRow.Cells[colTH.Name].Value = detail.DiagramNumber;
        //        dgProject.CurrentRow.Cells[colDtlState.Name].Value = StaticMethod.GetWBSTaskStateText(detail.State);
        //        dgProject.CurrentRow.Cells[colPlanProject.Name].Value = detail.PlanWorkAmount;
        //        dgProject.CurrentRow.Cells[colSumProject.Name].Value = detail.QuantityConfirmed;
        //        dgProject.CurrentRow.Cells[colSumXXJD.Name].Value = detail.ProgressConfirmed;
        //        dgProject.CurrentRow.Cells[colUnit.Name].Value = detail.WorkAmountUnitName;
        //        dgProject.CurrentRow.Cells[colUnit.Name].Tag = detail.WorkAmountUnitGUID;
        //        dgProject.CurrentRow.Tag = detail;
        //    }
        //    //dgProject_SelectionChanged(dgProject, new DataGridViewCellEventArgs(0, 0));
        //}


        //void btnCHDZH_Click(object sender, EventArgs e)
        //{
        //    VContractExcuteSelector vmros = new VContractExcuteSelector();
        //    vmros.ShowDialog();
        //    IList list = vmros.Result;
        //    if (list == null || list.Count == 0) return;
        //    SubContractProject engineerMaster = list[0] as SubContractProject;
        //   // this.txtCHDZH.Tag = engineerMaster;
        //    //this.txtCHDZH.Text = engineerMaster.BearerOrgName;
        //}

        ////void btnProject_Click(object sender, EventArgs e)
        ////{
        ////    if (dgProject.Rows.Count > 0)
        ////    {
        ////        if (this.btnUpdate.Enabled == false)
        ////        {
        ////            if (MessageBox.Show("有信息还未保存，是否保存？", "保存信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
        ////            {
        ////                btnSubmitProject_Click(sender, e);
        ////                return;
        ////            }
        ////            else
        ////            {
        ////                dgBear.CurrentRow.ReadOnly = true;
        ////            }
        ////        }
        ////        DateTime serverTime = model.GetServerTime();
        ////        //判断节点，是生产节点才可以新建工程量提报

        ////        if (ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) != 0)
        ////        {
        ////            if ((dgProject.CurrentRow.Tag as GWBSDetail).ProduceConfirmFlag == 1)
        ////            {
        ////                if (dgProject.Rows.Count > 0)
        ////                {
        ////                    if ((dgProject.CurrentRow.Tag as GWBSDetail).State == DocumentState.InExecute)
        ////                    {
        ////                        if (dgBear.Rows.Count > 0)
        ////                        {
        ////                            if ((dgBear.Rows[dgBear.Rows.Count - 1].Tag as GWBSTaskConfirm).Id != null)
        ////                            {
        ////                                int i = dgBear.Rows.Add();
        ////                                dgBear.ClearSelection();
        ////                                dgBear.CurrentCell = dgBear.Rows[i].Cells[colGZH.Name];
        ////                                confirm = new GWBSTaskConfirm();
        ////                                GWBSDetail dtl = dgProject.CurrentRow.Tag as GWBSDetail;
        ////                                confirm.GWBSDetail = dtl;
        ////                                confirm.GWBSDetailName = dtl.Name;
        ////                                confirm.GWBSTree = oprNode as GWBSTree;
        ////                                confirm.GWBSTreeName = (oprNode as GWBSTree).Name;
        ////                                confirm.WorkAmountUnitId = dtl.PriceUnitGUID;
        ////                                confirm.WorkAmountUnitName = dtl.PriceUnitName;
        ////                                confirm.AccountingState = EnumGWBSTaskConfirmAccountingState.未核算;
        ////                                dgBear.CurrentRow.Cells[colAccState.Name].Value = "未核算";
        ////                                confirm.PlannedQuantity = dtl.PlanWorkAmount;
        ////                                //【料费结算标识】：=<选中列表项>_【料费结算标识】或 0.不结算
        ////                                //confirm.MaterialFeeSettlementFlag = EnumMaterialFeeSettlementFlag.不结算;
        ////                                dgBear.CurrentRow.Cells[colMaterialSinge.Name].Value = "不结算";
        ////                                //this.radioN.Checked = true;
        ////                                confirm.RealOperationDate = DateTime.Now;
        ////                                confirm.GwbsSysCode = oprNode.SysCode;
        ////                                this.dgBear.CurrentRow.Cells[colGZH.Name].Tag = ConstObject.LoginPersonInfo;//制单人编号
        ////                                this.dgBear.CurrentRow.Cells[colGZH.Name].Value = ConstObject.LoginPersonInfo.Name;//制单人名称
        ////                                this.dgBear.CurrentRow.Cells[colLastDate.Name].Value = serverTime.Date.ToShortDateString();
        ////                                confirm.RealOperationDate = serverTime.Date;
        ////                                confirm.CreatePerson = ConstObject.LoginPersonInfo;//制单人编号
        ////                                confirm.CreatePersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
        ////                                confirm.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//登录业务组织
        ////                                confirm.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//业务组织名称
        ////                                confirm.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;//业务组织编号

        ////                                if (dtl.WeekScheduleDetail != null)
        ////                                {
        ////                                    confirm.TaskHandler = dtl.WeekScheduleDetail.SubContractProject;
        ////                                    confirm.TaskHandlerName = dtl.WeekScheduleDetail.SupplierName;
        ////                                    if (confirm.TaskHandler != null)
        ////                                    {
        ////                                        this.dgBear.CurrentRow.Cells[colCHDZH.Name].Value = confirm.TaskHandlerName;
        ////                                        this.dgBear.CurrentRow.Cells[colCHDZH.Name].Tag = confirm.TaskHandler;
        ////                                        this.dgBear.CurrentRow.Cells[colQYName.Name].Value = confirm.TaskHandler.ContractGroupCode;
        ////                                    }
        ////                                }
        ////                                else
        ////                                {
        ////                                    //查询服务OBS
        ////                                    ObjectQuery objectQuery = new ObjectQuery();
        ////                                    //objectQuery.AddCriterion(Expression.Eq("ProjectTask.SysCode", oprNode as GWBSTree));
        ////                                    string[] sysCodes = (oprNode as GWBSTree).SysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
        ////                                    Disjunction dis = new Disjunction();
        ////                                    for (int q = 0; q < sysCodes.Length; q++)
        ////                                    {
        ////                                        string sysCode = "";
        ////                                        for (int j = 0; j <= q; j++)
        ////                                        {
        ////                                            sysCode += sysCodes[j] + ".";
        ////                                        }
        ////                                        dis.Add(Expression.Eq("ProjectTask.SysCode", sysCode));
        ////                                    }
        ////                                    objectQuery.AddCriterion(dis);
        ////                                    objectQuery.AddFetchMode("ProjectTask", NHibernate.FetchMode.Eager);
        ////                                    objectQuery.AddFetchMode("SupplierId", NHibernate.FetchMode.Eager);
        ////                                    objectQuery.AddFetchMode("SupplierId.TheContractGroup", NHibernate.FetchMode.Eager);
        ////                                    IList list = model.ObjectQuery(typeof(OBSService), objectQuery);
        ////                                    if (list.Count > 0)
        ////                                    {
        ////                                        OBSService obs = list[0] as OBSService;
        ////                                        confirm.TaskHandler = obs.SupplierId;
        ////                                        confirm.TaskHandlerName = obs.SupplierName;
        ////                                        if (confirm.TaskHandler != null)
        ////                                        {
        ////                                            this.dgBear.CurrentRow.Cells[colCHDZH.Name].Value = confirm.TaskHandlerName;
        ////                                            this.dgBear.CurrentRow.Cells[colCHDZH.Name].Tag = confirm.TaskHandler;
        ////                                            this.dgBear.CurrentRow.Cells[colQYName.Name].Value = confirm.TaskHandler.ContractGroupCode;
        ////                                        }
        ////                                    }
        ////                                }
        ////                                if (this.txtCHDZH.Tag != null)
        ////                                {
        ////                                    SubContractProject subContractProject = this.txtCHDZH.Tag as SubContractProject;
        ////                                    this.dgBear.CurrentRow.Cells[colCHDZH.Name].Value = subContractProject.BearerOrgName;
        ////                                    this.dgBear.CurrentRow.Cells[colCHDZH.Name].Tag = subContractProject;
        ////                                    this.dgBear.CurrentRow.Cells[colQYName.Name].Value = subContractProject.ContractGroupCode;
        ////                                }
        ////                                dgBear.Rows[i].Tag = confirm;
        ////                            }
        ////                        }
        ////                        else
        ////                        {
        ////                            int i = dgBear.Rows.Add();
        ////                            confirm = new GWBSTaskConfirm();
        ////                            //dgBear.Rows[i].Selected = true;
        ////                            GWBSDetail dtl = dgProject.CurrentRow.Tag as GWBSDetail;
        ////                            confirm.GWBSDetail = dtl;
        ////                            confirm.GWBSDetailName = dtl.Name;
        ////                            confirm.GWBSTree = oprNode as GWBSTree;
        ////                            confirm.GWBSTreeName = (oprNode as GWBSTree).Name;
        ////                            confirm.WorkAmountUnitId = dtl.PriceUnitGUID;
        ////                            confirm.WorkAmountUnitName = dtl.PriceUnitName;
        ////                            confirm.AccountingState = EnumGWBSTaskConfirmAccountingState.未核算;
        ////                            dgBear.Rows[i].Cells[colAccState.Name].Value = "未核算";
        ////                            confirm.PlannedQuantity = dtl.PlanWorkAmount;
        ////                            //【料费结算标识】：=<选中列表项>_【料费结算标识】或 0.不结算
        ////                            //confirm.MaterialFeeSettlementFlag = EnumMaterialFeeSettlementFlag.不结算;
        ////                            dgBear.Rows[i].Cells[colMaterialSinge.Name].Value = "不结算";
        ////                            //this.radioN.Checked = true;
        ////                            confirm.RealOperationDate = DateTime.Now;
        ////                            confirm.GwbsSysCode = oprNode.SysCode;
        ////                            this.dgBear.Rows[i].Cells[colGZH.Name].Tag = ConstObject.LoginPersonInfo;//制单人编号
        ////                            this.dgBear.Rows[i].Cells[colGZH.Name].Value = ConstObject.LoginPersonInfo.Name;//制单人名称
        ////                            confirm.CreatePerson = ConstObject.LoginPersonInfo;//制单人编号
        ////                            confirm.CreatePersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
        ////                            confirm.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//登录业务组织
        ////                            confirm.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//业务组织名称
        ////                            confirm.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;//业务组织编号
        ////                            this.dgBear.Rows[i].Cells[colLastDate.Name].Value = serverTime.Date.ToShortDateString();
        ////                            confirm.RealOperationDate = serverTime.Date;
        ////                            if (dtl.WeekScheduleDetail != null)
        ////                            {
        ////                                confirm.TaskHandler = dtl.WeekScheduleDetail.SubContractProject;
        ////                                confirm.TaskHandlerName = dtl.WeekScheduleDetail.SupplierName;
        ////                                dgBear.Rows[i].Cells[colCHDZH.Name].Value = confirm.TaskHandlerName;
        ////                                dgBear.Rows[i].Cells[colCHDZH.Name].Tag = confirm.TaskHandler;
        ////                                dgBear.Rows[i].Cells[colQYName.Name].Value = confirm.TaskHandler.ContractGroupCode;
        ////                            }
        ////                            else
        ////                            {
        ////                                //查询服务OBS
        ////                                ObjectQuery objectQuery = new ObjectQuery();
        ////                                //objectQuery.AddCriterion(Expression.Eq("ProjectTask.SysCode", oprNode as GWBSTree));
        ////                                string[] sysCodes = (oprNode as GWBSTree).SysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
        ////                                Disjunction dis = new Disjunction();
        ////                                for (int q = 0; q < sysCodes.Length; q++)
        ////                                {
        ////                                    string sysCode = "";
        ////                                    for (int j = 0; j <= q; j++)
        ////                                    {
        ////                                        sysCode += sysCodes[j] + ".";
        ////                                    }
        ////                                    dis.Add(Expression.Eq("ProjectTask.SysCode", sysCode));
        ////                                }
        ////                                objectQuery.AddCriterion(dis);
        ////                                objectQuery.AddFetchMode("ProjectTask", NHibernate.FetchMode.Eager);
        ////                                objectQuery.AddFetchMode("SupplierId", NHibernate.FetchMode.Eager);
        ////                                objectQuery.AddFetchMode("SupplierId.TheContractGroup", NHibernate.FetchMode.Eager);
        ////                                IList list = model.ObjectQuery(typeof(OBSService), objectQuery);
        ////                                if (list.Count > 0)
        ////                                {
        ////                                    OBSService obs = list[0] as OBSService;
        ////                                    confirm.TaskHandler = obs.SupplierId;
        ////                                    confirm.TaskHandlerName = obs.SupplierName;
        ////                                    this.dgBear.Rows[i].Cells[colCHDZH.Name].Value = confirm.TaskHandlerName;
        ////                                    this.dgBear.Rows[i].Cells[colCHDZH.Name].Tag = confirm.TaskHandler;
        ////                                    this.dgBear.Rows[i].Cells[colQYName.Name].Value = confirm.TaskHandler.ContractGroupCode;
        ////                                }
        ////                            }
        ////                            if (this.txtCHDZH.Tag != null)
        ////                            {
        ////                                SubContractProject subContractProject = this.txtCHDZH.Tag as SubContractProject;
        ////                                this.dgBear.Rows[i].Cells[colCHDZH.Name].Value = subContractProject.BearerOrgName;
        ////                                this.dgBear.Rows[i].Cells[colCHDZH.Name].Tag = subContractProject;
        ////                                this.dgBear.Rows[i].Cells[colQYName.Name].Value = subContractProject.ContractGroupCode;
        ////                            }
        ////                            dgBear.Rows[i].Tag = confirm;
        ////                        }
        ////                    }
        ////                    else
        ////                    {
        ////                        MessageBox.Show("任务明细未发布，不能添加提报工程量信息！");
        ////                    }
        ////                }
        ////            }
        ////            else
        ////            {
        ////                MessageBox.Show("非生产节点不能进行工程量提报！");
        ////                return;
        ////            }
        ////        }
        ////        else
        ////        {
        ////            MessageBox.Show("该工程任务下的计划总工程量为0，不能新增！");
        ////            return;
        ////        }
        ////    }
        ////}

        //void dgBear_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
        //    if (dgBear.Rows[e.RowIndex].ReadOnly == false)
        //    {
        //        //双击承担者
        //        if (this.dgBear.Columns[e.ColumnIndex].Name.Equals(colCHDZH.Name))
        //        {
        //            if (confirm.AccountingState != EnumGWBSTaskConfirmAccountingState.己核算 && ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colQuantityBefore.Name].Value) == 0)
        //            {
        //                DataGridViewRow theCurrentRow = this.dgBear.CurrentRow;
        //                VContractExcuteSelector vmros = new VContractExcuteSelector();
        //                vmros.ShowDialog();
        //                IList list = vmros.Result;
        //                if (list == null || list.Count == 0) return;
        //                SubContractProject engineerMaster = list[0] as SubContractProject;
        //                this.dgBear.CurrentRow.Cells[colCHDZH.Name].Value = engineerMaster.BearerOrgName;
        //                this.dgBear.CurrentRow.Cells[colCHDZH.Name].Tag = engineerMaster;
        //                this.dgBear.CurrentRow.Cells[colQYName.Name].Value = engineerMaster.ContractGroupCode;
        //            }
        //            else
        //            {
        //                MessageBox.Show("提报信息已经审核，承担者信息不可更新！");
        //            }
        //        }
        //    }
        //}

        #endregion

        #region 生产检查

        void conMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (dgDetail.Rows.Count == 0)
            {
                AddRow(e);
            }
            else
            {
                if (dgDetail[colCheckPerson.Name, dgDetail.Rows.Count - 1].Value == null)
                {
                    conMenu.Hide();
                    MessageBox.Show("有新建的信息尚未保存！");
                }
                else
                {
                    AddRow(e);
                }
            }
        }

        void AddRow(ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text.Trim() == "添加生产检查信息")
            {
                conMenu.Hide();
                检查记录明细信息.Parent = tabControl1;
                tabPage2.Parent = null;
                相关附件.Parent = null;
                tabControl1.SelectedTab = 检查记录明细信息;
                相关附件.Parent = tabControl1;
                if (tvwCategory.SelectedNode == null)
                {
                    MessageBox.Show("至少选择一个工程任务节点！");
                    return;
                }
                InspectionRecord red = new InspectionRecord();
                red.DocState = DocumentState.Edit;
                red.CreateDate = ConstObject.LoginDate;
                int i = dgDetail.Rows.Add();
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    for (int j = 0; j < dgDetail.Rows.Count - 1; j++)
                    {
                        dgDetail.Rows[j].Selected = false;
                    }
                }
                dgDetail.Rows[i].Cells[0].Selected = true;
                dgDetail.Rows[i].Tag = red;
                txtCheckPerson.Value = ConstObject.LoginPersonInfo.Name;
                txtCheckPerson.Result.Add(ConstObject.LoginPersonInfo);
                dtpCheckDate.Value = ConstObject.LoginDate;
                //通过GBWS节点查询服务obs信息的承担队伍
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Sql(" instr('" + oprNode.SysCode + "',{alias}.ProjectTaskCode)>0"));

                //string[] sysCodes = oprNode.SysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                //Disjunction dis = new Disjunction();
                //for (int i = 0; i < sysCodes.Length; i++)
                //{
                //    string sysCode = "";
                //    for (int j = 0; j <= i; j++)
                //    {
                //        sysCode += sysCodes[j] + ".";
                //    }
                //    dis.Add(Expression.Eq("ProjectTask.SysCode", sysCode));
                //}
                //objectQuery.AddCriterion(dis);

                IList list = model.ObjectQuery(typeof(OBSService), oq);
                if (list.Count > 0)
                {
                    OBSService service = list[0] as OBSService;
                    txtSupply.Text = service.SupplierName;
                    txtSupply.Tag = service.SupplierId;
                }
                else
                {
                    txtSupply.Text = "";
                    txtSupply.Tag = null;
                }
                txtCheckStatus.Text = "";
                cbCheckConclusion.Text = "";
                cbWBSCheckRequir.Text = "";
                RefreshControls(MainViewState.Modify);
                检查记录明细信息.Parent = tabControl1;
                tabPage2.Parent = null;
                相关附件.Parent = null;
                tabControl1.SelectedTab = 检查记录明细信息;
                相关附件.Parent = tabControl1;
                dgDocumentMast.Rows.Clear();
                dgDocumentDetail.Rows.Clear();
            }
            if (e.ClickedItem.Text.Trim() == "添加质量验收信息")
            {
                conMenu.Hide();
                if (tvwCategory.SelectedNode == null)
                {
                    MessageBox.Show("至少选择一个工程任务节点！");
                    return;
                }
                InspectionRecord red = new InspectionRecord();
                red.DocState = DocumentState.Edit;
                red.CreateDate = ConstObject.LoginDate;
                int i = dgDetail.Rows.Add();
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    for (int j = 0; j < dgDetail.Rows.Count - 1; j++)
                    {
                        dgDetail.Rows[j].Selected = false;
                    }
                }
                dgDetail.Rows[i].Cells[0].Selected = true;
                dgDetail.Rows[i].Tag = red;
                txtCheckPerson1.Value = ConstObject.LoginPersonInfo.Name;
                txtCheckPerson1.Result.Add(ConstObject.LoginPersonInfo);
                dtpCheckDate1.Value = ConstObject.LoginDate;
                //通过GBWS节点查询服务obs信息的承担队伍
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Sql(" instr('" + oprNode.SysCode + "',{alias}.ProjectTaskCode)>0"));
                IList list = model.ObjectQuery(typeof(OBSService), oq);
                if (list.Count > 0)
                {
                    OBSService service = list[0] as OBSService;
                    txtSupply1.Text = service.SupplierName;
                    txtSupply1.Tag = service.SupplierId;
                }
                else
                {
                    txtSupply1.Text = "";
                    txtSupply1.Tag = null;
                }
                txtCheckStatus1.Text = "";
                cbCheckConclusion1.Text = "";
                cbWBSCheckRequir1.Text = "";
                RefreshControls(MainViewState.Modify);
                tabPage2.Parent = tabControl1;
                检查记录明细信息.Parent = null;
                相关附件.Parent = null;
                tabControl1.SelectedTab = tabPage2;
                相关附件.Parent = tabControl1;
            }
        }

        void ClearIns()
        {
            txtCheckPerson.Value = ConstObject.LoginPersonInfo.Name;
            txtCheckPerson.Result.Add(ConstObject.LoginPersonInfo);
            dtpCheckDate.Value = DateTime.Now;
            txtCheckStatus.Text = "";
            cbCheckConclusion.Text = "";
            txtSupply.Text = "";
            txtSupply.Tag = null;
            txtSupply1.Text = "";
            txtSupply1.Tag = null;
            cbWBSCheckRequir.Text = "";
        }

        void Clear()
        {
            txtCheckPerson1.Value = ConstObject.LoginPersonInfo.Name;
            txtCheckPerson1.Result.Add(ConstObject.LoginPersonInfo);
            dtpCheckDate1.Value = DateTime.Now;
            txtCheckStatus1.Text = "";
            cbCheckConclusion1.Text = "";
            txtSupply1.Text = "";
            cbWBSCheckRequir1.Text = "";
        }

        void dgDetail_MouseClick(object sender, MouseEventArgs e)
        {
            if (!dgDetail.ReadOnly)
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (dgDetail.Enabled)
                    {
                        conMenu.Items[ConAdd.Name].Enabled = true;
                        if (oprNode.AddUpFigureProgress == 100)
                        {
                            conMenu.Items[ConAddIns.Name].Enabled = true;
                        }
                        else
                        {
                            conMenu.Items[ConAddIns.Name].Enabled = false;
                        }
                        conMenu.Show(MousePosition.X, MousePosition.Y);
                    }
                }
            }
        }
        void dgDetail_CellEndEdit(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (!dgDetail.ReadOnly)
                {
                    if (e.RowIndex >= 0)
                    {
                        if (dgDetail.Rows[e.RowIndex].Selected == false)
                        {
                            dgDetail.ClearSelection();
                            dgDetail.Rows[e.RowIndex].Selected = true;
                        }
                        conMenu.Items[ConAdd.Name].Enabled = true;
                        if (oprNode.AddUpFigureProgress == 100)
                        {
                            conMenu.Items[ConAddIns.Name].Enabled = true;
                        }
                        else
                        {
                            conMenu.Items[ConAddIns.Name].Enabled = false;
                        }
                        conMenu.Show(MousePosition.X, MousePosition.Y);
                    }
                }
            }
        }

        void Loaddgdetail()
        {
            TreeNode node = this.tvwCategory.SelectedNode;
            if (node != null)
            {
                GWBSTree tree = node.Tag as GWBSTree;
                FilldgDtail(tree);
            }
        }

        void FilldgDtail(GWBSTree tree)
        {
            dgDetail.Rows.Clear();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("GWBSTree", tree));
            IList temp_list = model.GetInsRecord(oq);
            if (temp_list.Count > 0)
            {
                foreach (InspectionRecord Record in temp_list)
                {
                    int rowIndex = dgDetail.Rows.Add();
                    dgDetail[colCorrectiveSign.Name, rowIndex].Value = Record.CorrectiveSign;
                    dgDetail[colCheckSpecial.Name, rowIndex].Value = Record.InspectionSpecial;
                    dgDetail[colCheckConclusion.Name, rowIndex].Value = Record.InspectionConclusion;
                    dgDetail[colContent.Name, rowIndex].Value = Record.InspectionStatus;
                    dgDetail[colCheckPerson.Name, rowIndex].Value = Record.CreatePersonName;
                    dgDetail[colCheckPerson.Name, rowIndex].Tag = Record.CreatePerson;
                    dgDetail[colBearName.Name, rowIndex].Value = Record.BearTeamName;
                    dgDetail[colBearName.Name, rowIndex].Tag = Record.BearTeam;
                    dgDetail[colInspectionDate.Name, rowIndex].Value = Record.CreateDate.ToShortDateString();
                    if (Record.Id != null)
                    {
                        if (Record.DocState == DocumentState.InExecute || Record.DocState == DocumentState.InAudit)
                        {
                            dgDetail[colRecordState.Name, rowIndex].Value = "有效";
                        }
                        else
                        {
                            dgDetail[colRecordState.Name, rowIndex].Value = "编辑";
                        }
                    }
                    else
                    {
                        dgDetail[colRecordState.Name, rowIndex].Value = "编辑";
                    }
                    if (Record.InspectType == 1)
                    {
                        dgDetail[colInsType.Name, rowIndex].Value = "日常检查";
                    }
                    if (Record.InspectType == 2)
                    {
                        dgDetail[colInsType.Name, rowIndex].Value = "质量验收";
                    }
                    string strDeductionSign = ClientUtil.ToString(Record.DeductionSign);
                    string strCorrectiveSign = ClientUtil.ToString(Record.CorrectiveSign);
                    if (strCorrectiveSign.Equals("0"))
                    {
                        dgDetail[colCorrectiveSign.Name, rowIndex].Value = "不需整改";
                    }
                    if (strCorrectiveSign.Equals("1"))
                    {
                        dgDetail[colCorrectiveSign.Name, rowIndex].Value = "需要整改，未进行处理";
                    }
                    if (strCorrectiveSign.Equals("2"))
                    {
                        dgDetail[colCorrectiveSign.Name, rowIndex].Value = "需要整改，已进行处理";
                    }
                    dgDetail.Rows[rowIndex].Tag = Record;
                }

            }
            else
            {
                this.txtCheckPerson.Result.Clear();
                this.txtCheckPerson.Result.Add(ConstObject.LoginPersonInfo);
                this.txtCheckPerson.Value = ConstObject.LoginPersonInfo.Name;
            }
            if (dgDetail.Rows.Count > 0)
            {
                dgDetail.CurrentCell = dgDetail.Rows[0].Cells[0];
                dgDetail_CellClick(dgDetail, new DataGridViewCellEventArgs(0, 0));
            }
            else
            {
                RefreshControls(MainViewState.Browser);
            }
        }

        void dgDetail_CellClick(object sender, EventArgs e)
        {
            if (dgDetail.CurrentRow != null)
            {
                InspectionRecord master = dgDetail.CurrentRow.Tag as InspectionRecord;
                if (master != null)
                {
                    if (master.InspectType == 1)
                    {
                        //日常检查
                        //tabControl1
                        检查记录明细信息.Parent = tabControl1;
                        tabPage2.Parent = null;
                        相关附件.Parent = null;
                        tabControl1.SelectedTab = 检查记录明细信息;
                        相关附件.Parent = tabControl1;
                        this.EditInspectionRecord();
                    }
                    if (master.InspectType == 2)
                    {
                        //质量验收
                        tabPage2.Parent = tabControl1;
                        检查记录明细信息.Parent = null;
                        相关附件.Parent = null;
                        tabControl1.SelectedTab = tabPage2;
                        相关附件.Parent = tabControl1;
                        this.EditInspectionRecord();
                    }
                    //dgDocumentMast.Rows.Clear();
                    //if (dgDetail.CurrentRow.Tag == null) return;

                    FillDoc();
                    //ObjectQuery oqDoc = new ObjectQuery();
                    //oqDoc.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", (dgDetail.CurrentRow.Tag as InspectionRecord).Id));
                    //IList listDocument = model.ObjectQuery(typeof(ProObjectRelaDocument), oqDoc);
                    //if (listDocument != null && listDocument.Count > 0)
                    //{
                    //    foreach (ProObjectRelaDocument doc in listDocument)
                    //    {
                    //        InsertIntoGridDocument(doc);
                    //    }
                    //}
                }
            }
            else
            {
                RefreshControls(MainViewState.Browser);
            }
        }
        private void InsertIntoGridDocument(ProObjectRelaDocument doc)
        {
            //int index = gridDocument.Rows.Add();
            //DataGridViewRow row = gridDocument.Rows[index];
            //row.Cells[DocumentName.Name].Value = doc.DocumentName;
            //row.Cells[DocumentCode.Name].Value = doc.DocumentCode;
            //row.Cells[DocumentCateCode.Name].Value = doc.DocumentCateCode;
            //row.Cells[DocumentDesc.Name].Value = doc.DocumentDesc;

            //row.Tag = doc;
        }
        /// <summary>
        /// 在编辑框显示当前检查记录详细信息
        /// </summary>
        private void EditInspectionRecord()
        {
            if (tabControl1.SelectedTab.Name.Equals(检查记录明细信息.Name))
            {
                if (dgDetail.CurrentRow != null)
                {
                    DataGridViewRow theCurrentRow = dgDetail.CurrentRow;
                    if (theCurrentRow.Tag != null)
                    {
                        InspectionRecord master = theCurrentRow.Tag as InspectionRecord;
                        if (master.CreatePersonName != null)
                        {
                            txtCheckPerson.Result.Clear();
                            txtCheckPerson.Result.Add(master.CreatePerson);
                            txtCheckPerson.Value = master.CreatePersonName;
                        }
                        else
                        {
                            this.txtCheckPerson.Result.Clear();
                            this.txtCheckPerson.Result.Add(ConstObject.LoginPersonInfo);
                            this.txtCheckPerson.Value = ConstObject.LoginPersonInfo.Name;
                        }
                        if (master.BearTeam != null)
                        {
                            txtSupply.Tag = master.BearTeam;
                            txtSupply.Text = master.BearTeamName;
                        }
                        else
                        {
                            txtSupply.Text = "";
                            txtSupply.Tag = null;
                        }
                        cbWBSCheckRequir.Text = master.InspectionSpecial;
                        cbCheckConclusion.Text = master.InspectionConclusion;
                        if (master.CreateDate <= Convert.ToDateTime("1900-1-1 00:00:00"))
                        {
                            dtpCheckDate.Value = DateTime.Now;
                        }
                        else
                        {
                            dtpCheckDate.Value = master.CreateDate;
                        }
                        string strDeductionSign = ClientUtil.ToString(master.DeductionSign);
                        string strCorrectiveSign = ClientUtil.ToString(master.CorrectiveSign);
                        if (strCorrectiveSign.Equals("1") || strCorrectiveSign.Equals("2"))
                        {
                            radioButton2.Checked = true;
                        }
                        else
                        {
                            radioButton1.Checked = true;
                        }
                        txtCheckStatus.Text = master.InspectionStatus;
                        if (master.Id != null)
                        {
                            if ((master.DocState == DocumentState.InAudit || master.DocState == DocumentState.InExecute) ||
                                master.CreatePerson.Id != ConstObject.LoginPersonInfo.Id)
                            {
                                //信息为提交状态，信息不可编辑
                                RefreshControls(MainViewState.Browser);
                            }
                            else
                            {
                                RefreshControls(MainViewState.Modify);
                            }
                        }
                        else
                        {
                            RefreshControls(MainViewState.Modify);
                        }
                    }
                }
            }
            if (tabControl1.SelectedTab.Name.Equals(tabPage2.Name))
            {
                if (dgDetail.CurrentRow != null)
                {
                    DataGridViewRow theCurrentRow = dgDetail.CurrentRow;
                    if (theCurrentRow.Tag != null)
                    {
                        InspectionRecord master = theCurrentRow.Tag as InspectionRecord;
                        if (master.CreatePersonName != null)
                        {
                            txtCheckPerson1.Result.Clear();
                            txtCheckPerson1.Result.Add(master.CreatePerson);
                            txtCheckPerson1.Value = master.CreatePersonName;
                        }
                        else
                        {
                            this.txtCheckPerson1.Result.Clear();
                            this.txtCheckPerson1.Result.Add(ConstObject.LoginPersonInfo);
                            this.txtCheckPerson1.Value = ConstObject.LoginPersonInfo.Name;
                        }
                        if (master.BearTeam != null)
                        {
                            txtSupply1.Tag = null;
                            txtSupply1.Text = master.BearTeamName;
                        }
                        else
                        {
                            txtSupply1.Text = "";
                            txtSupply1.Tag = null;
                        }
                        cbWBSCheckRequir1.Text = master.InspectionSpecial;
                        cbCheckConclusion1.Text = master.InspectionConclusion;
                        if (master.CreateDate <= Convert.ToDateTime("1900-1-1 00:00:00"))
                        {
                            dtpCheckDate1.Value = DateTime.Now;
                        }
                        else
                        {
                            dtpCheckDate1.Value = master.CreateDate;
                        }
                        string strDeductionSign = ClientUtil.ToString(master.DeductionSign);
                        string strCorrectiveSign = ClientUtil.ToString(master.CorrectiveSign);
                        if (strCorrectiveSign.Equals("1") || strCorrectiveSign.Equals("2"))
                        {
                            radioButton3.Checked = true;
                        }
                        else
                        {
                            radioButton4.Checked = true;
                        }
                        txtCheckStatus1.Text = master.InspectionStatus;
                        if (master.Id != null)
                        {
                            if ((master.DocState == DocumentState.InAudit || master.DocState == DocumentState.InExecute) || master.CreatePerson.Id != ConstObject.LoginPersonInfo.Id)
                            {
                                //信息为提交状态，信息不可编辑
                                RefreshControls(MainViewState.Browser);
                            }
                            else
                            {
                                RefreshControls(MainViewState.Modify);
                            }
                        }
                        else
                        {
                            RefreshControls(MainViewState.Modify);
                        }
                    }
                }
            }
        }
        bool SetMessage()
        {
            if (dgDetail.CurrentRow == null)
            {
                MessageBox.Show("表格中没有信息，不可保存！");
                return false;
            }
            if (cbWBSCheckRequir.Text == "")
            {
                MessageBox.Show("请选择检查专业！");
                return false;
            }
            if (txtCheckPerson.Text == "")
            {
                MessageBox.Show("请选择检查人！");
                return false;
            }
            if (cbCheckConclusion.Text == "")
            {
                MessageBox.Show("请选择检查结论！");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 保存更新日常检查记录
        /// </summary>
        /// <returns></returns>
        private void SaveInspectionRecord(string strName)
        {
            try
            {
                InspectionRecord master = dgDetail.CurrentRow.Tag as InspectionRecord;
                if (master.Id == null)
                    master = new InspectionRecord();
                master.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                master.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;
                master.OpgSysCode = ConstObject.TheOperationOrg.SysCode;
                master.HandOrgLevel = ConstObject.TheOperationOrg.Level;
                master.HandlePerson = ConstObject.LoginPersonInfo;
                master.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                master.ProjectId = projectInfo.Id;
                master.ProjectName = projectInfo.Name;
                master.InspectType = 1;
                if (strName == "保存")
                {
                    master.DocState = DocumentState.Edit;
                }
                else
                {
                    master.DocState = DocumentState.InExecute;
                }
                //检查人
                if (txtCheckPerson.Result.Count > 0)
                {
                    master.CreatePerson = txtCheckPerson.Result[0] as PersonInfo;
                    master.CreatePersonName = ClientUtil.ToString(txtCheckPerson.Value);
                }
                master.InspectionSpecial = cbWBSCheckRequir.Text;//检查专业
                System.Web.UI.WebControls.ListItem li = cbWBSCheckRequir.SelectedItem as System.Web.UI.WebControls.ListItem;

                master.GWBSTree = oprNode;
                master.GWBSTreeName = oprNode.Name;
                master.GWBSTreeSysCode = oprNode.SysCode;
                if (txtSupply.Text != "")
                {
                    master.BearTeam = txtSupply.Tag as SubContractProject;
                    master.BearTeamName = ClientUtil.ToString(txtSupply.Text);
                }
                if (li != null)
                {
                    master.InspectionSpecialCode = li.Value;
                }
                master.InspectionConclusion = cbCheckConclusion.Text;//检查结论
                master.InspectionStatus = txtCheckStatus.Text;//检查内容
                master.CreateDate = dtpCheckDate.Value.Date;
                if (master.Id != null)
                {
                    if (master.DeductionSign != 2)
                    {
                        if (radioButton1.Checked)
                        {
                            master.CorrectiveSign = 0;
                        }
                        else
                        {
                            master.CorrectiveSign = 1;
                        }
                    }
                }
                else
                {
                    if (radioButton1.Checked)
                    {
                        master.CorrectiveSign = 0;
                    }
                    else
                    {
                        master.CorrectiveSign = 1;
                    }
                }
                //保存日常检查记录
                master = model.SaveInspectialRecordMaster(master);
                this.dgDetail.Rows[dgDetail.Rows.Count - 1].Tag = master;
                MessageBox.Show(strName + "成功！");
                RefreshControls(MainViewState.Browser);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存数据错误" + ex.ToString());
                return;
            }
        }
        void btnSaveInspectionRecord_Click(object sender, EventArgs e)
        {
            //校验数据
            if (!SetMessage()) return;
            try
            {
                this.SaveInspectionRecord("保存");
                FilldgDtail(tvwCategory.SelectedNode.Tag as GWBSTree);
            }
            catch (Exception err)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(err));
                return;
            }
        }
        void btnDelete_Click(object sender, EventArgs e)
        {
            bool flag = false;
            if (dgDetail.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    for (int i = 0; i < dgDetail.SelectedRows.Count; i++)
                    {
                        if ((dgDetail.SelectedRows[i].Tag as InspectionRecord).Id != null)
                        {
                            InspectionRecord record = dgDetail.SelectedRows[i].Tag as InspectionRecord;
                            if (record.CreatePerson != null)
                            {
                                if (record.CreatePerson.Id != ConstObject.LoginPersonInfo.Id)
                                {
                                    MessageBox.Show("非登录人检查的信息，不可删除！");
                                    return;
                                }

                                if (record.DocState == DocumentState.InAudit || record.DocState == DocumentState.InExecute)
                                {
                                    MessageBox.Show("信息已经提交，不可删除！");
                                    return;
                                }
                                //删除信息
                                if (!model.DeleteInspectionRecord(dgDetail.SelectedRows[i].Tag as InspectionRecord)) return;
                            }
                        }
                        else
                        {
                            dgDetail.Rows.Remove(dgDetail.SelectedRows[i]);
                        }
                        flag = true;
                    }
                    if (flag)
                    {
                        MessageBox.Show("删除成功！");
                        FilldgDtail(tvwCategory.SelectedNode.Tag as GWBSTree);
                    }
                }
            }
        }

        void btnSubmit_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.Selected)
                {
                    if (var.Cells[colCheckPerson.Name].Value == null)
                    {
                        if (!SetMessage()) return;
                        SaveInspectionRecord("提交");
                        FilldgDtail(tvwCategory.SelectedNode.Tag as GWBSTree);
                    }
                    else
                    {
                        InspectionRecord record = dgDetail.CurrentRow.Tag as InspectionRecord;
                        if (record.Id != null)
                        {
                            if (record.DocState == DocumentState.InExecute)
                            {
                                MessageBox.Show("信息已提交！");
                                return;
                            }
                            record.DocState = DocumentState.InExecute;
                            record = model.SaveInspectialRecordMaster(record);
                            MessageBox.Show("提交成功！");
                            RefreshControls(MainViewState.Browser);
                            FilldgDtail(tvwCategory.SelectedNode.Tag as GWBSTree);
                        }
                    }
                }
            }
        }
        bool SetMessage1()
        {
            if (cbWBSCheckRequir1.Text == "")
            {
                MessageBox.Show("请选择检查专业！");
                return false;
            }
            if (txtCheckPerson1.Text == "")
            {
                MessageBox.Show("请选择检查人！");
                return false;
            }
            if (cbCheckConclusion1.Text == "")
            {
                MessageBox.Show("请选择检查结论！");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 保存更新质量检验记录
        /// </summary>
        /// <returns></returns>
        private void SaveInsRecord(string strName)
        {
            try
            {
                InspectionRecord master = dgDetail.CurrentRow.Tag as InspectionRecord;
                if (master.Id == null)
                    master = new InspectionRecord();
                if (strName == "保存")
                {
                    master.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                    master.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;
                    master.OpgSysCode = ConstObject.TheOperationOrg.SysCode;
                    master.HandOrgLevel = ConstObject.TheOperationOrg.Level;
                    master.HandlePerson = ConstObject.LoginPersonInfo;
                    master.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                    CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                    master.ProjectId = projectInfo.Id;
                    master.ProjectName = projectInfo.Name;
                    master.DocState = DocumentState.Edit;
                    master.InspectType = 2;
                }
                else
                {
                    master.DocState = DocumentState.InExecute;
                }
                //检查人
                if (txtCheckPerson.Result.Count > 0)
                {
                    master.CreatePerson = txtCheckPerson1.Result[0] as PersonInfo;
                    master.CreatePersonName = txtCheckPerson1.Text;
                }
                master.InspectionSpecial = cbWBSCheckRequir1.Text;//检查专业
                System.Web.UI.WebControls.ListItem li = cbWBSCheckRequir1.SelectedItem as System.Web.UI.WebControls.ListItem;

                master.GWBSTree = oprNode;
                master.GWBSTreeName = oprNode.Name;
                master.GWBSTreeSysCode = oprNode.SysCode;
                if (txtSupply1.Tag != null)
                {
                    master.BearTeam = txtSupply1.Tag as SubContractProject;
                    master.BearTeamName = ClientUtil.ToString(txtSupply1.Text);
                }
                else
                {
                    master.BearTeam = null;
                    master.BearTeamName = "";
                }
                master.InspectionSpecialCode = li.Value;
                master.InspectionConclusion = cbCheckConclusion1.Text;//检查结论
                master.InspectionStatus = txtCheckStatus1.Text;//检查内容
                master.CreateDate = dtpCheckDate1.Value.Date;
                if (master.Id != null)
                {
                    if (master.DeductionSign != 2)
                    {
                        if (radioButton4.Checked)
                        {
                            master.CorrectiveSign = 0;
                        }
                        else
                        {
                            master.CorrectiveSign = 1;
                        }
                    }
                }
                else
                {
                    if (radioButton4.Checked)
                    {
                        master.CorrectiveSign = 0;
                    }
                    else
                    {
                        master.CorrectiveSign = 1;
                    }
                }
                //保存日常检查记录
                master = model.SaveInspectialRecordMaster(master);
                this.dgDetail.Rows[dgDetail.Rows.Count - 1].Tag = master;
                MessageBox.Show(strName + "成功！");
                RefreshControls(MainViewState.Browser);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存数据错误" + ex.ToString());
                return;
            }
        }

        void btnSave1_Click(object sender, EventArgs e)
        {
            //校验数据
            if (!SetMessage1()) return;
            try
            {
                this.SaveInsRecord("保存");
                FilldgDtail(tvwCategory.SelectedNode.Tag as GWBSTree);
            }
            catch (Exception err)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(err));
                return;
            }
        }
        void btnDelete1_Click(object sender, EventArgs e)
        {
            bool flag = false;
            if (dgDetail.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    for (int i = 0; i < dgDetail.SelectedRows.Count; i++)
                    {
                        if ((dgDetail.SelectedRows[i].Tag as InspectionRecord).Id != null)
                        {
                            InspectionRecord record = dgDetail.SelectedRows[i].Tag as InspectionRecord;
                            if (record.CreatePerson != null)
                            {
                                if (record.CreatePerson.Id != ConstObject.LoginPersonInfo.Id)
                                {
                                    MessageBox.Show("非登录人检查的信息，不可删除！");
                                    return;
                                }

                                if (record.DocState == DocumentState.InAudit || record.DocState == DocumentState.InExecute)
                                {
                                    MessageBox.Show("信息已经提交，不可删除！");
                                    return;
                                }
                                //删除信息
                                if (!model.DeleteInspectionRecord(dgDetail.SelectedRows[i].Tag as InspectionRecord)) return;
                            }
                        }
                        else
                        {
                            dgDetail.Rows.Remove(dgDetail.SelectedRows[i]);
                        }
                        flag = true;
                    }
                    if (flag)
                    {
                        MessageBox.Show("删除成功！");
                        FilldgDtail(tvwCategory.SelectedNode.Tag as GWBSTree);
                    }
                }
            }
        }

        void btnSubmit1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.Selected)
                {
                    if (var.Cells[colCheckPerson.Name].Value == null)
                    {
                        if (!SetMessage()) return;
                        SaveInspectionRecord("提交");
                        FilldgDtail(tvwCategory.SelectedNode.Tag as GWBSTree);
                    }
                    else
                    {
                        InspectionRecord record = dgDetail.CurrentRow.Tag as InspectionRecord;
                        if (record.Id != null)
                        {
                            if (record.DocState == DocumentState.InExecute)
                            {
                                MessageBox.Show("信息已提交！");
                                return;
                            }
                            record.DocState = DocumentState.InExecute;
                            record = model.SaveInspectialRecordMaster(record);
                            MessageBox.Show("提交成功！");
                            RefreshControls(MainViewState.Browser);
                            FilldgDtail(tvwCategory.SelectedNode.Tag as GWBSTree);
                        }
                    }
                }
            }
        }


        #endregion

        //#region 文档操作
        ////文档上传按钮状态
        //private void btnStates(int i)
        //{
        //    if (i == 0)
        //    {
        //        btnDownLoadDocument.Enabled = false;
        //        btnOpenDocument.Enabled = false;
        //        btnUpdateDocument.Enabled = false;
        //        btnDeleteDocument.Enabled = false;
        //        btnUpFile.Enabled = false;
        //    }
        //    if (i == 1)
        //    {
        //        btnDownLoadDocument.Enabled = true;
        //        btnOpenDocument.Enabled = true;
        //        btnUpdateDocument.Enabled = true;
        //        btnDeleteDocument.Enabled = true;
        //        btnUpFile.Enabled = true;
        //    }
        //}
        //void FillDoc()
        //{
        //    ObjectQuery oq = new ObjectQuery();
        //    oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", (dgDetail.CurrentRow.Tag as InspectionRecord).Id));
        //    IList listDocument = model.ObjectQuery(typeof(ProObjectRelaDocument), oq);
        //    if (listDocument != null && listDocument.Count > 0)
        //    {
        //        gridDocument.Rows.Clear();
        //        foreach (ProObjectRelaDocument doc in listDocument)
        //        {
        //            InsertIntoGridDocument(doc);
        //        }
        //    }
        //}
        //void btnDownLoadDocument_Click(object sender, EventArgs e)
        //{
        //    if (gridDocument.SelectedRows.Count == 0)
        //    {
        //        MessageBox.Show("请选择要下载的文档！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        gridDocument.Focus();
        //        return;
        //    }
        //    IList relaDocList = new List<ProObjectRelaDocument>();
        //    foreach (DataGridViewRow row in gridDocument.SelectedRows)
        //    {
        //        ProObjectRelaDocument relaDoc = row.Tag as ProObjectRelaDocument;
        //        relaDocList.Add(relaDoc);
        //    }
        //    VDocumentDownloadByID vdd = new VDocumentDownloadByID(relaDocList);
        //    vdd.ShowDialog();
        //}

        //void btnOpenDocument_Click(object sender, EventArgs e)
        //{
        //    if (gridDocument.SelectedRows.Count == 0)
        //    {
        //        MessageBox.Show("请选择要打开的文档！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        gridDocument.Focus();
        //        return;
        //    }

        //    List<PLMWebServices.ProjectDocument> docList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument>();
        //    PLMWebServices.ProjectDocument[] projectDocList = null;

        //    foreach (DataGridViewRow row in gridDocument.SelectedRows)
        //    {
        //        ProObjectRelaDocument relaDoc = row.Tag as ProObjectRelaDocument;
        //        PLMWebServices.ProjectDocument doc = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument();
        //        doc.EntityID = relaDoc.DocumentGUID;
        //        docList.Add(doc);
        //    }


        //    try
        //    {
        //        PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.DownLoadDocumentByIRP(docList.ToArray(), null, userName, jobId, null, out projectDocList);
        //        if (es != null)
        //        {
        //            MessageBox.Show("下载失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }

        //        List<string> errorList = new List<string>();
        //        List<string> listFileFullPaths = new List<string>();
        //        if (projectDocList != null)
        //        {
        //            string fileFullPath = AppDomain.CurrentDomain.BaseDirectory + "TempFilePreview\\" + Guid.NewGuid().ToString().Replace("-", "");
        //            if (!Directory.Exists(fileFullPath))
        //                Directory.CreateDirectory(fileFullPath);

        //            for (int i = 0; i < projectDocList.Length; i++)
        //            {
        //                //byte[] by = listFileBytes[i] as byte[];
        //                //if (by != null && by.Length > 0)
        //                //{
        //                string fileName = projectDocList[i].FileName;

        //                if (projectDocList[i].FileDataByte == null || projectDocList[i].FileDataByte.Length <= 0 || fileName == null)
        //                {
        //                    string strName = projectDocList[i].Code + projectDocList[i].Name;
        //                    errorList.Add(strName);
        //                    continue;
        //                }

        //                string tempFileFullPath = fileFullPath + @"\\" + fileName;

        //                CreateFileFromByteAarray(projectDocList[i].FileDataByte, tempFileFullPath);

        //                listFileFullPaths.Add(tempFileFullPath);
        //                //}
        //            }
        //        }

        //        foreach (string fileFullPath in listFileFullPaths)
        //        {
        //            FileInfo file = new FileInfo(fileFullPath);

        //            //定义一个ProcessStartInfo实例
        //            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
        //            //设置启动进程的初始目录
        //            info.WorkingDirectory = file.DirectoryName;//Application.StartupPath; 
        //            //设置启动进程的应用程序或文档名
        //            info.FileName = file.Name;
        //            //设置启动进程的参数
        //            info.Arguments = "";
        //            //启动由包含进程启动信息的进程资源
        //            try
        //            {
        //                System.Diagnostics.Process.Start(info);
        //            }
        //            catch (System.ComponentModel.Win32Exception we)
        //            {
        //                MessageBox.Show(this, we.Message);
        //            }
        //        }
        //        if (errorList != null && errorList.Count > 0)
        //        {
        //            string str = "";
        //            foreach (string s in errorList)
        //            {
        //                str += (s + ";");
        //            }
        //            MessageBox.Show(str + "这" + errorList.Count + "个文件，无法预览，文件不存在或未指定格式！");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.IndexOf("未将对象引用设置到对象的实例") > -1)
        //        {
        //            MessageBox.Show("操作失败，不存在的文档对象！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        }
        //        else
        //            MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}


        ////修改文档
        //void btnUpdateDocument_Click(object sender, EventArgs e)
        //{
        //    if (gridDocument.SelectedRows.Count == 0)
        //    {
        //        MessageBox.Show("请选择要修改的文档！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        gridDocument.Focus();
        //        return;
        //    }
        //    IList relaDocList = new List<ProObjectRelaDocument>();
        //    foreach (DataGridViewRow row in gridDocument.SelectedRows)
        //    {
        //        ProObjectRelaDocument relaDoc = row.Tag as ProObjectRelaDocument;
        //        relaDocList.Add(relaDoc);
        //    }
        //    VDocumentListUpdate vdlu = new VDocumentListUpdate(projectInfo, relaDocList);
        //    vdlu.ShowDialog();
        //    IList resultRelaDocList = vdlu.ResultListDoc;

        //    foreach (DataGridViewRow row in gridDocument.SelectedRows)
        //    {
        //        gridDocument.Rows.RemoveAt(row.Index);
        //    }
        //    if (resultRelaDocList == null) return;
        //    foreach (ProObjectRelaDocument doc in resultRelaDocList)
        //    {
        //        int rowIndex = gridDocument.Rows.Add();
        //        gridDocument[DocumentName.Name, rowIndex].Value = doc.DocumentName;
        //        gridDocument[DocumentCateCode.Name, rowIndex].Value = doc.DocumentCateCode;
        //        gridDocument[DocumentCode.Name, rowIndex].Value = doc.DocumentCode;
        //        gridDocument[DocumentDesc.Name, rowIndex].Value = doc.DocumentDesc;
        //        gridDocument.Rows[rowIndex].Tag = doc;
        //    }
        //}
        ////删除文档
        //void btnDeleteDocument_Click(object sender, EventArgs e)
        //{
        //    if (gridDocument.SelectedRows.Count == 0)
        //    {
        //        MessageBox.Show("请选择要删除的文档！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        gridDocument.Focus();
        //        return;
        //    }

        //    if (MessageBox.Show("确认要删除选择文档吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        //        return;

        //    try
        //    {
        //        IList relaDocList = new List<ProObjectRelaDocument>();
        //        List<string> docIds = new List<string>();
        //        List<PLMWebServices.ProjectDocument> proDocList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument>();
        //        PLMWebServices.ProjectDocument[] reultProdocList = null;
        //        PLMWebServices.ProjectDocument[] resultProDoc = null;

        //        foreach (DataGridViewRow row in gridDocument.SelectedRows)
        //        {
        //            ProObjectRelaDocument relaDoc = row.Tag as ProObjectRelaDocument;
        //            relaDocList.Add(relaDoc);
        //            docIds.Add(relaDoc.DocumentGUID);
        //        }

        //        PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectDocumentByIRP(docIds.ToArray(), Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentQueryVersion.所有版本,
        //            null, userName, jobId, null, out resultProDoc);
        //        if (es != null)
        //        {
        //            MessageBox.Show("操作失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }

        //        for (int i = 0; i < resultProDoc.Length; i++)
        //        {
        //            PLMWebServices.ProjectDocument doc = resultProDoc[i];
        //            doc.State = Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentState.作废;
        //            proDocList.Add(doc);
        //        }

        //        PLMWebServices.ErrorStack es1 = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.UpdateDocumentByIRP(proDocList.ToArray(), Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentUpdateMode.添加一个新版次文件,
        //            null, userName, jobId, null, out reultProdocList);
        //        if (es1 != null)
        //        {
        //            MessageBox.Show("操作失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //        bool flag = model.DeleteProObjRelaDoc(relaDocList);
        //        if (flag)
        //        {
        //            MessageBox.Show("删除成功！");
        //            foreach (DataGridViewRow row in gridDocument.SelectedRows)
        //            {
        //                gridDocument.Rows.RemoveAt(row.Index);
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("删除失败！");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("删除失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}


        ////批量上传
        //void btnUpFile_Click(object sender, EventArgs e)
        //{
        //    if (dgDetail.CurrentRow == null)
        //    {
        //        MessageBox.Show("没有选中的信息");
        //        return;
        //    }
        //    InspectionRecord curBillMaster = dgDetail.CurrentRow.Tag as InspectionRecord;
        //    if (curBillMaster.Id == null)
        //    {
        //        return;
        //        //if (MessageBox.Show("当前业务对象还没保存，是否保存！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
        //        //{
        //        //    try
        //        //    {
        //        //        if (tabControl1.SelectedTab.Name.Equals(相关附件.Name))
        //        //        {

        //        //        }
        //        //    }
        //        //    catch (Exception ex)
        //        //    {
        //        //        MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(ex));
        //        //    }
        //        //}
        //    }
        //    if (curBillMaster.Id != null)
        //    {
        //        VDocumentUploadList vdul = new VDocumentUploadList(projectInfo, curBillMaster, curBillMaster.Id);
        //        vdul.ShowDialog();
        //        IList resultDocumentList = vdul.ResultListDoc;
        //        if (resultDocumentList == null) return;
        //        //gridDocument.Rows.Clear();
        //        foreach (ProObjectRelaDocument doc in resultDocumentList)
        //        {
        //            int rowIndex = gridDocument.Rows.Add();
        //            gridDocument[DocumentName.Name, rowIndex].Value = doc.DocumentName;
        //            gridDocument[DocumentCode.Name, rowIndex].Value = doc.DocumentCode;
        //            gridDocument[DocumentCateCode.Name, rowIndex].Value = doc.DocumentCateCode;
        //            gridDocument[DocumentDesc.Name, rowIndex].Value = doc.DocumentDesc;
        //            gridDocument.Rows[rowIndex].Tag = doc;
        //        }
        //    }
        //}

        //private string GetExceptionMessage(PLMWebServices.ErrorStack es)
        //{
        //    string msg = es.Message;
        //    PLMWebServices.ErrorStack esTemp = es.InnerErrorStack;
        //    while (esTemp != null && !string.IsNullOrEmpty(esTemp.Message))
        //    {
        //        msg += "；\n" + esTemp.Message;
        //        esTemp = esTemp.InnerErrorStack;
        //    }

        //    if (msg.IndexOf("不能在具有唯一索引") > -1 && msg.IndexOf("插入重复键") > -1)
        //    {
        //        msg = "已存在同名文档，请重命名该文档名称.";
        //    }

        //    return msg;
        //}
        //public static void CreateFileFromByteAarray(byte[] stream, string fileName)
        //{
        //    try
        //    {
        //        FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
        //        fs.Write(stream, 0, stream.Length);
        //        fs.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        //#endregion
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
            dgDocumentDetail.ReadOnly = false;
            FileSelect.ReadOnly = false;

        }
        //加载文档数据
        void FillDoc()
        {
            if (dgDetail.CurrentRow == null || dgDetail.CurrentRow.Tag == null)
                return;
            InspectionRecord curBillMaster = dgDetail.CurrentRow.Tag as InspectionRecord;
            dgDocumentMast.Rows.Clear();
            dgDocumentDetail.Rows.Clear();
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
            if (dgDetail.CurrentRow == null)
            {
                MessageBox.Show("没有选中的信息");
                return;
            }
            InspectionRecord curBillMaster = dgDetail.CurrentRow.Tag as InspectionRecord;
            //if (curBillMaster.Id == null)
            //{
            //    if (MessageBox.Show("当前业务对象还没保存，是否保存！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            //    {
            //        if (!ValidView()) return;
            //        try
            //        {
            //            if (!ViewToModel()) return;
            //            curBillMaster = model.TargetRespBookSrc.saveTargetRespBook(curBillMaster);
            //            this.ViewCaption = ViewName;
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(ex));
            //        }
            //    }
            //}
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

        #region 生产检查打印
        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                FlashScreen.Show("正在查询信息......");
                ObjectQuery oq = new ObjectQuery();
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                string strCodes = (tvwCategory.SelectedNode.Tag as GWBSTree).SysCode;
                oq.AddCriterion(Expression.Like("GWBSTreeSysCode", strCodes, MatchMode.Start));
                //}
                //承担队伍
                if (txtBearTeam.Text != "")
                {
                    oq.AddCriterion(Expression.Eq("BearTeamName", txtBearTeam.Text));
                }

                oq.AddCriterion(Expression.Ge("CreateDate", this.txtStartDate.Value.Date));
                oq.AddCriterion(Expression.Lt("CreateDate", this.txtEndDate.Value.AddDays(1).Date));

                if (!txtInspectionPerson.Text.Trim().Equals("") && txtInspectionPerson.Result != null)
                {
                    oq.AddCriterion(Expression.Eq("CreatePerson", (txtInspectionPerson.Result[0] as PersonInfo)));
                }
                if (txtInspectionSpecial.Text != "")
                {
                    oq.AddCriterion(Expression.Eq("InspectionSpecial", txtInspectionSpecial.Text));
                }
                if (txtInspectionCnclusion.Text != "")
                {
                    oq.AddCriterion(Expression.Eq("InspectionConclusion", txtInspectionCnclusion.SelectedItem));
                }
                if (txtInspectionType.Text != "")//检查类型
                {
                    if (txtInspectionType.Text == "日常检查")
                    {
                        oq.AddCriterion(Expression.Eq("InspectType", 1));
                    }
                    if (txtInspectionType.Text == "质量验收")
                    {
                        oq.AddCriterion(Expression.Eq("InspectType", 2));
                    }
                }
                if (txtRectReq.Text != "")//整改标志
                {
                    if (txtRectReq.Text == "不需整改")
                    {
                        oq.AddCriterion(Expression.Eq("CorrectiveSign", 0));
                    }
                    if (txtRectReq.Text == "需要整改，未进行处理")
                    {
                        oq.AddCriterion(Expression.Eq("CorrectiveSign", 1));
                    }
                    if (txtRectReq.Text == "需要整改，已进行处理")
                    {
                        oq.AddCriterion(Expression.Eq("CorrectiveSign", 2));
                    }
                }
                oq.AddFetchMode("PBSTree", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("WeekScheduleDetail", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("SubContractProject", NHibernate.FetchMode.Eager);
                IList list = model.GetInspectionRecordQuery(oq);
                dgSearch.Rows.Clear();
                foreach (InspectionRecord record in list)
                {
                    if (record.GWBSTree != null)
                    {
                        if (this.tvwCategory.SelectedNode != null)
                        {
                            //string strCode = (tvwCategory.SelectedNode.Tag as GWBSTree).SysCode;
                            //if (record.GWBSTree.SysCode.Contains(strCode))
                            //{
                            int rowIndex = dgSearch.Rows.Add();
                            dgSearch.Rows[rowIndex].Tag = record;
                            this.dgSearch[colSelect.Name, rowIndex].Value = true;
                            dgSearch[colInspectionSpecial.Name, rowIndex].Value = record.InspectionSpecial;//检查专业
                            dgSearch[colBInspectionPerson.Name, rowIndex].Value = record.BearTeamName;//被检查人
                            dgSearch[colInspectionConclusion.Name, rowIndex].Value = record.InspectionConclusion;//检查结论
                            dgSearch[colInspectionDescript.Name, rowIndex].Value = record.InspectionStatus;//检查情况
                            dgSearch[colInspectionPerson.Name, rowIndex].Value = record.CreatePersonName;//检查者

                            dgSearch[colInspectionType.Name, rowIndex].Value = record.InspectType;//检查类型
                            string strInspectType = ClientUtil.ToString(record.InspectType);//检查类型
                            if (strInspectType.Equals("1"))
                            {
                                dgSearch[colInspectionType.Name, rowIndex].Value = "日常检查";
                            }
                            if (strInspectType.Equals("2"))
                            {
                                dgSearch[colInspectionType.Name, rowIndex].Value = "质量验收";
                            }
                            dgSearch[colInsDate.Name, rowIndex].Value = record.CreateDate.ToShortDateString();//检查时间
                            object objState = record.DocState;
                            if (objState != null)
                            {
                                dgSearch[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName((record.DocState));
                            }
                            string strCorrectiveSign = ClientUtil.ToString(record.CorrectiveSign);//整改标志
                            if (strCorrectiveSign.Equals("0"))
                            {
                                dgSearch[colRectifState.Name, rowIndex].Value = "不需整改";
                            }
                            if (strCorrectiveSign.Equals("1"))
                            {
                                dgSearch[colRectifState.Name, rowIndex].Value = "需要整改，未进行处理";
                            }
                            if (strCorrectiveSign.Equals("2"))
                            {
                                dgSearch[colRectifState.Name, rowIndex].Value = "需要整改，已进行处理";
                            }
                            ObjectQuery oq1 = new ObjectQuery();
                            oq1.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", record.Id));
                            IList listDocument = model.ObjectQuery(typeof(ProObjectRelaDocument), oq1);
                            if (listDocument.Count == 0)
                            {
                                dgSearch[colFJCount.Name, rowIndex].Value = "0";
                            }
                            else
                            {
                                dgSearch[colFJCount.Name, rowIndex].Value = listDocument.Count.ToString();
                            }
                            //}
                        }
                    }
                    else
                    {

                    }
                }
                FlashScreen.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
            finally
            {
                FlashScreen.Close();
            }
        }
        void btnPrint_Click(object sender, EventArgs e)
        {
            if (LoadTempleteFile(@"日常检查记录.flx") == false) return;
            FillFlex();
            flexGrid1.Print();
            //curBillMaster.PrintTimes = curBillMaster.PrintTimes + 1;
            //curBillMaster = model.saveWasteMatApply(curBillMaster);
            return;

        }
        void btnPrintPreview_Click(object sender, EventArgs e)
        {
            if (txtBearTeam.Text == "")
            {
                MessageBox.Show("请选择检查队伍！");
                return;
            }
            if (LoadTempleteFile(@"日常检查记录.flx") == false) return;
            FillFlex();
            flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            return;
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

        private void FillFlex()
        {
            //int detailStartRowNumber = 5;//5为模板中的行号
            //int detailCount = 0;
            //foreach (DataGridViewRow var in this.dgSearch.Rows)
            //{
            //    if ((bool)var.Cells[colSelect.Name].Value)
            //    {
            //        detailCount++;
            //    }
            //}
            ////插入明细行
            //flexGrid1.InsertRow(detailStartRowNumber, detailCount - 1);
            ////设置单元格的边框，对齐方式
            //FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
            //range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            //range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            //range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            //range.Mask = FlexCell.MaskEnum.Digital;
            //CommonUtil.SetFlexGridPrintFace(this.flexGrid1);

            //flexGrid1.Cell(3, 3).Text = (tvwCategory.SelectedNode.Tag as GWBSTree).Name;
            //int i = 0;
            //foreach (DataGridViewRow var in this.dgSearch.Rows)
            //{
            //    if (this.dgSearch[colSelect.Name, i].Value != null)
            //    {
            //        if ((bool)var.Cells[colSelect.Name].Value)
            //        {
            //            InspectionRecord rec = var.Tag as InspectionRecord;

            //            string strInspectType = ClientUtil.ToString(rec.InspectType);//检查类型
            //            if (strInspectType.Equals("1"))
            //            {
            //                flexGrid1.Cell(detailStartRowNumber, 1).Text = "日常检查";
            //            }
            //            if (strInspectType.Equals("2"))
            //            {
            //                flexGrid1.Cell(detailStartRowNumber, 1).Text = "质量验收";
            //            }
            //            flexGrid1.Cell(detailStartRowNumber, 1).WrapText = true;
            //            flexGrid1.Cell(detailStartRowNumber, 2).Text = rec.InspectionSpecial;
            //            flexGrid1.Cell(detailStartRowNumber, 2).WrapText = true;
            //            flexGrid1.Cell(detailStartRowNumber, 3).Text = rec.CreatePersonName;
            //            flexGrid1.Cell(detailStartRowNumber, 3).WrapText = true;
            //            flexGrid1.Cell(detailStartRowNumber, 4).Text = rec.BearTeamName;
            //            flexGrid1.Cell(detailStartRowNumber, 4).WrapText = true;
            //            flexGrid1.Cell(detailStartRowNumber, 5).Text = rec.InspectionConclusion;
            //            flexGrid1.Cell(detailStartRowNumber, 5).WrapText = true;
            //            string strCorrectiveSign = ClientUtil.ToString(rec.CorrectiveSign);//整改标志
            //            if (strCorrectiveSign.Equals("0"))
            //            {
            //                flexGrid1.Cell(detailStartRowNumber, 6).Text = "不需整改";
            //            }
            //            if (strCorrectiveSign.Equals("1"))
            //            {
            //                flexGrid1.Cell(detailStartRowNumber, 6).Text = "需要整改，未进行处理";
            //            }
            //            if (strCorrectiveSign.Equals("2"))
            //            {
            //                flexGrid1.Cell(detailStartRowNumber, 6).Text = "需要整改，已进行处理";
            //            }
            //            flexGrid1.Cell(detailStartRowNumber, 6).WrapText = true;
            //            flexGrid1.Cell(detailStartRowNumber, 7).Text = rec.CreateDate.ToShortDateString();
            //            flexGrid1.Cell(detailStartRowNumber, 7).WrapText = true;
            //            flexGrid1.Cell(detailStartRowNumber, 8).Text = rec.InspectionStatus;
            //            flexGrid1.Cell(detailStartRowNumber, 8).WrapText = true;
            //            //flexGrid1.Cell(detailStartRowNumber + i, 9).Text = rec.;
            //            flexGrid1.Cell(5 + detailCount, 2).Text = rec.ProjectName;
            //            flexGrid1.Cell(5 + detailCount, 5).Text = rec.RealOperationDate.ToShortDateString();
            //            flexGrid1.Cell(5 + detailCount, 8).Text = rec.CreatePersonName;
            //            flexGrid1.Row(detailStartRowNumber).AutoFit();
            //            detailStartRowNumber++;
            //        }
            //    }
            //    i++;
            //}

            FlexCell.Range range = flexGrid1.Range(2, 1, 7, flexGrid1.Cols - 1);
            //range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            range.Mask = FlexCell.MaskEnum.Digital;
            CommonUtil.SetFlexGridPrintFace(this.flexGrid1);

            flexGrid1.Cell(2, 2).Text = projectInfo.Name;
            flexGrid1.Cell(2, 2).WrapText = true;
            if (ConstObject.TheLogin.TheAccountOrgInfo != null)
            {
                flexGrid1.Cell(3, 2).Text = ConstObject.TheLogin.TheAccountOrgInfo.Name;
                flexGrid1.Cell(3, 2).WrapText = true;
            }
            flexGrid1.Cell(3, 4).Text = dtpPrintTime.Text.ToString();
            flexGrid1.Cell(4, 2).Text = txtBearTeam.Text;
            flexGrid1.Cell(4, 2).WrapText = true;
            flexGrid1.Cell(5, 2).Text = txtFullPath.Text;
            flexGrid1.Cell(5, 2).WrapText = true;

            IList list = new ArrayList();
            foreach (DataGridViewRow var in dgSearch.Rows)
            {
                InspectionRecord irc = var.Tag as InspectionRecord;
                if (irc.InspectionStatus != null)
                {
                    list.Add(irc);
                }
            }
            string person = "";
            for (int i = 0; i < list.Count; i++)
            {
                InspectionRecord ic = list[i] as InspectionRecord;
                string inspectionPerson = ic.CreatePersonName;
                if (person == "")
                {
                    person = ic.CreatePersonName;
                    flexGrid1.Cell(6, 2).Text = person;
                }
                else
                {
                    if (person.Contains(ic.CreatePersonName))
                    {

                    }
                    else
                    {
                        person += "、" + ic.CreatePersonName;
                        flexGrid1.Cell(6, 2).Text = person;
                        flexGrid1.Cell(6, 2).WrapText = true;
                    }
                }
                if (i == 0)
                {
                    flexGrid1.Cell(7, 1).Text = "检查记录：" + "\r\n" + "\r\n" + "  " + (i + 1) + ". " + ic.GWBSTreeName + "," + ic.InspectionStatus + "\r\n";
                }
                else
                {
                    flexGrid1.Cell(7, 1).Text += "  " + (i + 1) + ". " + ic.GWBSTreeName + "," + ic.InspectionStatus + "\r\n";
                }
                flexGrid1.Cell(7, 1).WrapText = true;
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgSearch.Rows)
            {
                var.Cells[colSelect.Name].Value = true;
            }
        }
        /// <summary>
        /// 反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkNone_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgSearch.Rows)
            {
                bool isSelected = (bool)var.Cells[colSelect.Name].Value;
                var.Cells[colSelect.Name].Value = !isSelected;
            }
        }
        void dgSearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            InspectionRecord record = dgSearch.CurrentRow.Tag as InspectionRecord;
            if (record != null)
            {
                VProductManageChangeQuery query = new VProductManageChangeQuery(record);
                query.ShowDialog();
            }
        }

        #endregion








    }
}
