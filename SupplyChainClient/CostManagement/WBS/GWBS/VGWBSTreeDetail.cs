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
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Main;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonForm;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using ImportIntegration;
using System.IO;
using System.Diagnostics;
using Application.Resource.MaterialResource.Domain;
using Iesi.Collections;
using Iesi.Collections.Generic;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using IRPServiceModel.Domain.Document;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using Application.Business.Erp.SupplyChain.Util;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VGWBSTreeDetail : TBasicDataView
    {
        private TreeNode currNode = null;
        private GWBSTree oprNode = null;

        //有权限的GWBSTree
        private IList lstInstance;

        //唯一编码
        private string uniqueCode;

        private Hashtable hashtableRules = new Hashtable();

        private List<GWBSDetail> listCopyNodeDetail = new List<GWBSDetail>();

        private TreeNode mouseSelectNode = null;
        private List<TreeNode> listFindNodes = new List<TreeNode>();
        private int showFindNodeIndex = 0;

        /// <summary>
        /// 选择成本项时，是否是单选
        /// </summary>
        private bool IsCostItemSingleSelect = true;
        /// <summary>
        /// 当前选择节点所对应的分摊比例信息
        /// </summary>
        List<GWBSTreeDetailRelationship> lstSelectedGWBSRate = null;

        #region 文档操作变量
        string fileObjectType = string.Empty;
        string FileStructureType = string.Empty;

        string userName = string.Empty;
        string jobId = string.Empty;

        #endregion 文档操作

        ContractGroup optContract = null;

        CurrentProjectInfo projectInfo = null;

        GWBSDetail oprDtl = null;
        /// <summary>
        /// 当前选中节点的明细
        /// </summary>
        List<GWBSDetail> lstCurNodeDetail = null;
        GWBSDetail updateBeforeDtl = null;//明细修改前的值

        private bool IsAddDetail = true;

        public MGWBSTree model;

        #region 相关文档
        string filePath = string.Empty;
        string objecIsGWBS = string.Empty;
        string addOrUpDate = string.Empty;
        public PersonInfo person = null;
        public MDocumentCategory docModel = new MDocumentCategory();
        #endregion

        public VGWBSTreeDetail(MGWBSTree mot)
        {
            model = mot;
            InitializeComponent();
            person = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;
            InitForm();
        }

        private void InitForm()
        {
            try
            {
                this.TopMost = true;
                FlashScreen.Show("正在初始化数据,请稍候......");
                mnuTree.Items.Add("批量签证变更");
                InitEvents();

                projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();

                //检查要求
                IList list = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetBasicDataByName(VBasicDataOptr.WBS_CheckRequire);
                if (list != null)
                {
                    foreach (BasicDataOptr bdo in list)
                    {
                        cbListCheckRequire.Items.Add(bdo.BasicName);
                    }
                }

                tvwCategory.CheckBoxes = false;
                // LoadGWBSTreeTree();
                //LoadGWBSTree(null);
                LoadGWBSTree();
                RefreshControls(MainViewState.Browser);
                RefreshDetailControls(MainViewState.Browser);

                txtNoDtlColorFlag.BackColor = ColorTranslator.FromHtml("#D7E8FE");

                cbRelaPBS.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

                tabBaseInfo.TabPages.Remove(tabCostSubject);


                List<string> listParams = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetUploadFileParamsByMBP_IRP();
                fileObjectType = listParams[0];
                FileStructureType = listParams[1];

                userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
                jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;


                #region 加载相关文档
                rbtnDocumentStencil.Checked = true;
                gbAdd.Enabled = false;
                txtCateCode.Enabled = false;
                colAlterMode.Visible = colState.Visible = false;
                cmbVerifySwitch.Visible = false;

                foreach (string infoType in Enum.GetNames(typeof(ProjectDocumentSubmitState)))
                {
                    cmbVerifyState.Items.Add(infoType);
                }
                foreach (string infoType in Enum.GetNames(typeof(ProjectTaskTypeAlterMode)))
                {
                    cmbAlterMode.Items.Add(infoType);
                }
                foreach (string infoType in Enum.GetNames(typeof(ProjectDocumentVerifySwitch)))
                {
                    cmbVerifySwitch.Items.Add(infoType);
                }
                //文档信息类型
                foreach (string infoType in Enum.GetNames(typeof(PLMWebServices.DocumentInfoType)))
                {
                    cmbInforType.Items.Add(infoType);
                }
                //projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                foreach (string infoType in Enum.GetNames(typeof(ProjectDocumentAssociateLevel)))
                {
                    cmbAssociateLevel.Items.Add(infoType);
                }
                cmbAssociateLevel.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbAssociateLevel.SelectedIndex = 0;
                #endregion
                tabPageWBSDocInfo.Parent = null;
                this.TopMost = true;
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("初始化失败，详细信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                FlashScreen.Close();
                this.TopMost = true;
            }
        }

        private void InitEvents()
        {
            btnSelectContractGroup.Click += new EventHandler(btnSelectContractGroup_Click);

            #region 工程任务
            tvwCategory.NodeMouseClick += new TreeNodeMouseClickEventHandler(tvwCategory_NodeMouseClick);
            tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);
            //tvwCategory.BeforeExpand += new TreeViewCancelEventHandler(tvwCategory_BeforeExpand);
            tvwCategory.AfterExpand += new TreeViewEventHandler(tvwCategory_AfterExpand);
            btnRefreshTreeStatus.Click += new EventHandler(btnRefreshTreeStatus_Click);

            txtKeyWord.TextChanged += new EventHandler(txtKeyWord_TextChanged);
            txtKeyWord.KeyDown += new KeyEventHandler(txtKeyWord_KeyDown);
            btnFindTaskNode.Click += new EventHandler(btnFindTaskNode_Click);
            mnuTree.ItemClicked += new ToolStripItemClickedEventHandler(mnuTree_ItemClicked);

            //btnDownLoadDocument.Click += new EventHandler(btnDownLoadDocument_Click);
            //btnOpenDocument.Click += new EventHandler(btnOpenDocument_Click);

            tabControlDtl.SelectedIndexChanged += new EventHandler(tabControlDtl_SelectedIndexChanged);
            #endregion

            #region 任务明细
            this.btnChange.Click += new EventHandler(btnChange_Click);
            btnAddDetail.Click += new EventHandler(btnAddDetail_Click);
            btnBatchAddDtl.Click += new EventHandler(btnBatchAddDtl_Click);
            btnInsertDetail.Click += new EventHandler(btnInsertDetail_Click);
            btnUpdateDetail.Click += new EventHandler(btnUpdateDetail_Click);
            btnDeleteDetail.Click += new EventHandler(btnDeleteDetail_Click);
            btnPublishDetail.Click += new EventHandler(btnPublishDetail_Click);
            btnCancellationDetail.Click += new EventHandler(btnCancellationDetail_Click);
            btnSaveTaskDetail.Click += new EventHandler(btnSaveTaskDetail_Click);
            btnSaveBaseInfo.Click += new EventHandler(btnSave_Click);
            btnChangeTaskDetail.Click += new EventHandler(btnChangeTaskDetail_Click);
            btnTaskBackout.Click += new EventHandler(btnTaskBackout_Click);
            btnSelectChangeContract.Click += new EventHandler(btnSelectChangeContract_Click);

            btnOrderDetailUp.Click += new EventHandler(btnOrderDetailUp_Click);
            btnOrderDetailDown.Click += new EventHandler(btnOrderDetailDown_Click);

            btnSelectCostItem.Click += new EventHandler(btnSelectCostItem_Click);
            btnSelectMainResourceType.Click += new EventHandler(btnSelectMainResourceType_Click);

            btnExportMPP.Click += new EventHandler(btnExportMPP_Click);
            btnDtlExportExcel.Click += new EventHandler(btnExportExcel_Click);

            txtBudgetProjectUnit.TextChanged += new EventHandler(txtBudgetProjectUnit_TextChanged);
            txtBudgetPriceUnit.TextChanged += new EventHandler(txtBudgetPriceUnit_TextChanged);

            txtBudgetProjectUnit.LostFocus += new EventHandler(txtBudgetProjectUnit_LostFocus);
            txtBudgetPriceUnit.LostFocus += new EventHandler(txtBudgetPriceUnit_LostFocus);
            btnSelProjectQnyUnit.Click += new EventHandler(btnSelProjectQnyUnit_Click);
            btnSelPriceUnit.Click += new EventHandler(btnSelPriceUnit_Click);

            gridGWBDetail.CellClick += new DataGridViewCellEventHandler(gridGWBDetail_CellClick);

            mnuTaskDetail.ItemClicked += new ToolStripItemClickedEventHandler(mnuTaskDetail_ItemClicked);

            txtDtlCostItem.LostFocus += new EventHandler(txtDtlCostItem_LostFocus);
            txtDtlCostItem.KeyDown += new KeyEventHandler(txtDtlCostItem_KeyDown);

            txtBudgetContractPrice.TextChanged += new EventHandler(txtBudgetContractPrice_TextChanged);
            txtBudgetContractProjectAmount.TextChanged += new EventHandler(txtBudgetContractProjectAmount_TextChanged);

            txtBudgetResponsibilityPrice.TextChanged += new EventHandler(txtBudgetResponsibilityPrice_TextChanged);
            txtBudgetResponsibilityProjectAmount.TextChanged += new EventHandler(txtBudgetResponsibilityProjectAmount_TextChanged);

            txtBudgetPlanPrice.TextChanged += new EventHandler(txtBudgetPlanPrice_TextChanged);
            txtBudgetPlanProjectAmount.TextChanged += new EventHandler(txtBudgetPlanProjectAmount_TextChanged);

            btnAccountCostData.Click += new EventHandler(btnAccountCostData_Click);

            cbUpdateContract.Click += new EventHandler(cbUpdateContract_Click);

            txtMainResourceType.LostFocus += new EventHandler(txtMainResourceType_LostFocus);

            txtDrawNumber.LostFocus += new EventHandler(txtDrawNumber_LostFocus);
            #endregion

            #region 任务明细分科目成本
            btnAddCostSubject.Click += new EventHandler(btnAddCostSubject_Click);
            btnUpdateCostSubject.Click += new EventHandler(btnUpdateCostSubject_Click);
            btnDeleteCostSubject.Click += new EventHandler(btnDeleteCostSubject_Click);

            btnContractUsage.Click += new EventHandler(btnContractUsage_Click);
            btnResponsibleUsage.Click += new EventHandler(btnResponsibleUsage_Click);
            btnPlanUsage.Click += new EventHandler(btnPlanUsage_Click);

            btnUnionUsage.Click += new EventHandler(btnUnionUsage_Click);
            #endregion

            #region 相关文档
            btnDownLoadDocument.Click += new EventHandler(btnDownLoadDocument_Click);
            //btnOpenDocument.Click += new EventHandler(btnOpenDocument_Click);

            btnUpdateDocument.Click += new EventHandler(btnUpdateDocument_Click);
            btnDeleteDocument.Click += new EventHandler(btnDeleteDocument_Click);


            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnAddNew.Click += new EventHandler(btnAddNew_Click);
            cmbBusinessObject.SelectedIndexChanged += new EventHandler(cmbBusinessObject_SelectedIndexChanged);
            btnSearchCateCode.Click += new EventHandler(btnSearchCateCode_Click);
            btnCheckDocumentStencil.Click += new EventHandler(btnCheckDocumentStencil_Click);
            btnSaveAdd.Click += new EventHandler(btnSaveAdd_Click);
            btnQuit.Click += new EventHandler(btnQuit_Click);
            btnCheckFile.Click += new EventHandler(btnCheckFile_Click);
            btnCheckAll.Click += new EventHandler(btnCheckAll_Click);
            btnEmpty.Click += new EventHandler(btnEmpty_Click);

            rbtnDocumentStencil.CheckedChanged += new EventHandler(rbtnDocumentStencil_CheckedChanged);
            //rbtnProjectDocument.CheckedChanged += new EventHandler(rbtnProjectDocument_CheckedChanged);
            #endregion

        }



        void tabControlDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlDtl.SelectedTab == tabPageWBSBaseInfo)
            {
                GWBSTree optTask = tabPageWBSBaseInfo.Tag as GWBSTree;
                if (optTask != null && optTask.Id == oprNode.Id)
                    return;

                oprNode = LoadRelaPBS(oprNode);

                tvwCategory.SelectedNode.Tag = oprNode;

                this.ShownTaskNodeInfo();

                LoadCostItemsZoning();

                tabPageWBSBaseInfo.Tag = oprNode;
            }
            else if (tabControlDtl.SelectedTab == tabPageWBSDocInfo)
            {
                GWBSTree optTask = tabPageWBSDocInfo.Tag as GWBSTree;
                if (optTask != null && optTask.Id == oprNode.Id)
                    return;

                LoadTaskDocumentInfo();

                tabPageWBSDocInfo.Tag = oprNode;
            }
        }

        private void LoadTaskDocumentInfo()
        {
            SearchIns(oprNode);

            //查询相关文档
            if (rbtnDocumentStencil.Checked)
            {
                ObjectQuery oqDoc = new ObjectQuery();
                oqDoc.AddCriterion(NHibernate.Criterion.Expression.Eq("ProjectTask.Id", oprNode.Id));
                IList relaDocumentList = model.ObjectQuery(typeof(ProjectDocumentVerify), oqDoc);
                gridDocument.Rows.Clear();
                ShowList(relaDocumentList, "ProjectDocumentVerify");
            }
            else
            {
                rbtnDocumentStencil.Checked = true;
            }

            gridDocument.ClearSelection();

        }

        void btnSelectChangeContract_Click(object sender, EventArgs e)
        {
            VSelectWBSContractGroup frm = new VSelectWBSContractGroup(new MWBSContractGroup(), false);
            if (txtChangeContractName.Tag != null)
            {
                frm.DefaultSelectedContract = txtChangeContractName.Tag as ContractGroup;
            }
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                ContractGroup cg = frm.SelectResult[0] as ContractGroup;
                txtChangeContractName.Text = cg.ContractName;
                txtChangeContractName.Tag = cg;
                txtChangeContractType.Text = cg.ContractGroupType;
                txtDtlContractGroupName.Text = cg.ContractName;
                txtDtlContractGroupType.Text = cg.ContractGroupType;
                txtDtlContractGroupName.Tag = cg;

            }
        }

        //删除主资源
        void txtMainResourceType_LostFocus(object sender, EventArgs e)
        {
            if (txtMainResourceType.Text.Trim() == "")
            {
                txtMainResourceType.Tag = null;
            }
        }

        //图号
        void txtDrawNumber_LostFocus(object sender, EventArgs e)
        {
            oprDtl.DiagramNumber = txtDrawNumber.Text.Trim();

            var queryMainUsage = from d in oprDtl.ListCostSubjectDetails
                                 where d.MainResTypeFlag == true
                                 select d;

            if (queryMainUsage.Count() > 0)
            {
                queryMainUsage.ElementAt(0).DiagramNumber = oprDtl.DiagramNumber;
            }
        }

        void cbUpdateContract_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(oprDtl.Id))
            {
                ContractGroup cg = null;
                if (cbUpdateContract.Checked)
                {
                    cg = txtContractGroupName.Tag as ContractGroup;
                }
                else if (!string.IsNullOrEmpty(oprDtl.ContractGroupGUID))
                {
                    cg = new ContractGroup();
                    cg.ContractName = oprDtl.ContractGroupName;
                    cg.ContractGroupType = oprDtl.ContractGroupType;
                }

                if (cg != null)
                {
                    txtDtlContractGroupName.Text = cg.ContractName;
                    txtDtlContractGroupType.Text = cg.ContractGroupType;
                }
            }
        }

        void btnSelPriceUnit_Click(object sender, EventArgs e)
        {
            SelectUnit(txtBudgetPriceUnit);
        }

        void btnSelProjectQnyUnit_Click(object sender, EventArgs e)
        {
            SelectUnit(txtBudgetProjectUnit);
        }

        void txtBudgetProjectUnit_LostFocus(object sender, EventArgs e)
        {
            txtBudgetPriceUnit.LostFocus -= new EventHandler(txtBudgetPriceUnit_LostFocus);
            SetStandardUnit(sender);
            txtBudgetPriceUnit.LostFocus += new EventHandler(txtBudgetPriceUnit_LostFocus);
        }

        void txtBudgetPriceUnit_LostFocus(object sender, EventArgs e)
        {
            txtBudgetProjectUnit.LostFocus -= new EventHandler(txtBudgetProjectUnit_LostFocus);
            SetStandardUnit(sender);
            txtBudgetProjectUnit.LostFocus += new EventHandler(txtBudgetProjectUnit_LostFocus);
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

        //成本数据计算
        void btnAccountCostData_Click(object sender, EventArgs e)
        {

            #region 从界面上更新明细的工程量信息
            decimal ContractProjectQuantity = 0;
            if (txtBudgetContractProjectAmount.Text.Trim() != "")
            {
                try
                {
                    ContractProjectQuantity = ClientUtil.ToDecimal(txtBudgetContractProjectAmount.Text);
                }
                catch { }
            }
            oprDtl.ContractProjectQuantity = ContractProjectQuantity;

            decimal ResponsibilitilyWorkAmount = 0;
            if (txtBudgetResponsibilityProjectAmount.Text.Trim() != "")
            {
                try
                {
                    ResponsibilitilyWorkAmount = ClientUtil.ToDecimal(txtBudgetResponsibilityProjectAmount.Text);
                }
                catch { }
            }
            oprDtl.ResponsibilitilyWorkAmount = ResponsibilitilyWorkAmount;

            decimal PlanWorkAmount = 0;
            if (txtBudgetPlanProjectAmount.Text.Trim() != "")
            {
                try
                {
                    PlanWorkAmount = ClientUtil.ToDecimal(txtBudgetPlanProjectAmount.Text);
                }
                catch { }
            }
            oprDtl.PlanWorkAmount = PlanWorkAmount;

            #endregion

            #region 计算耗用信息
            for (int i = 0; i < oprDtl.ListCostSubjectDetails.Count; i++)
            {
                GWBSDetailCostSubject dtl = oprDtl.ListCostSubjectDetails.ElementAt(i);

                //合同收入
                //i）如果【成本核算科目】为“人工费”，则【资源合同工程量单价】等于所属{工程任务明细}_【合同单价】减去其余兄弟{工程资源耗用明细}_【资源合同工程量单价】之和；
                //ii）【资源合同工程量】：=所属{工程任务明细}_【合同工程量】*【合同定额数量】；
                //iii）【资源合同合价】：=【资源合同工程量】*【资源合同工程量单价】；

                //if (!string.IsNullOrEmpty(dtl.CostAccountSubjectName) && dtl.CostAccountSubjectName.Trim() == "人工费")
                //{
                //    decimal brotherProjectAmountPrice = 0;//兄弟合同工程量单价之和
                //    foreach (GWBSDetailCostSubject tempDtl in oprDtl.ListCostSubjectDetails)
                //    {
                //        if (tempDtl.Id != dtl.Id)
                //        {
                //            brotherProjectAmountPrice += tempDtl.ContractPrice;
                //        }
                //    }

                //    dtl.ContractPrice = oprDtl.ContractPrice - brotherProjectAmountPrice;
                //}

                dtl.ContractProjectAmount = oprDtl.ContractProjectQuantity * dtl.ContractQuotaQuantity;
                dtl.ContractTotalPrice = dtl.ContractProjectAmount * dtl.ContractQuantityPrice;


                //责任成本
                //a）针对<操作{工程资源耗用明细}集>在列表框中显示的每一个对象进行如下操作：
                //【责任耗用数量】：=所属{工程任务明细}_【责任工程量】*【责任定额数量】；
                //【责任耗用合价】：=【责任耗用数量】*【责任数量单价】；
                dtl.ResponsibilitilyWorkAmount = oprDtl.ResponsibilitilyWorkAmount * dtl.ResponsibleQuotaNum;
                dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyWorkAmount * dtl.ResponsibilitilyPrice;



                //计划成本
                //a）针对<操作{工程资源耗用明细}集>在列表框中显示的每一个对象进行如下操作：
                //【计划耗用数量】：=所属{工程任务明细}_【计划工程量】*【计划定额数量】；
                //【计划耗用合价】：=【计划耗用数量】*【计划数量单价】；
                dtl.PlanWorkAmount = oprDtl.PlanWorkAmount * dtl.PlanQuotaNum;
                dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;


            }
            #endregion

            #region 计算合价
            //合同收入
            //【合同单价】：=下属{工程资源耗用明细}_【合同工程量单价】
            //【合同合价】：=【合同工程量】*【合同单价】
            //责任成本
            //【责任单价】：=下属{工程资源耗用明细}_【责任工程量单价】
            //【责任合价】：=【责任工程量】*【责任单价】
            //计划成本
            //【计划单价】：=下属{工程资源耗用明细}_【计划工程量单价】
            //【计划合价】：=【计划工程量】*【计划单价】

            decimal dtlUsageProjectAmountPriceByContract = 0;
            decimal dtlUsageProjectAmountPriceByResponsible = 0;
            decimal dtlUsageProjectAmountPriceByPlan = 0;
            foreach (GWBSDetailCostSubject subject in oprDtl.ListCostSubjectDetails)
            {
                dtlUsageProjectAmountPriceByContract += subject.ContractPrice;
                dtlUsageProjectAmountPriceByResponsible += subject.ResponsibleWorkPrice;
                dtlUsageProjectAmountPriceByPlan += subject.PlanWorkPrice;
            }

            oprDtl.ContractPrice = dtlUsageProjectAmountPriceByContract;
            oprDtl.ResponsibilitilyPrice = dtlUsageProjectAmountPriceByResponsible;
            oprDtl.PlanPrice = dtlUsageProjectAmountPriceByPlan;


            oprDtl.ContractTotalPrice = oprDtl.ContractProjectQuantity * oprDtl.ContractPrice;
            oprDtl.ResponsibilitilyTotalPrice = oprDtl.ResponsibilitilyWorkAmount * oprDtl.ResponsibilitilyPrice;
            oprDtl.PlanTotalPrice = oprDtl.PlanWorkAmount * oprDtl.PlanPrice;



            txtBudgetContractPrice.Text = ToDecimailString(oprDtl.ContractPrice);
            txtBudgetContractTotalPrice.Text = ToDecimailString(oprDtl.ContractTotalPrice);

            txtBudgetResponsibilityPrice.Text = ToDecimailString(oprDtl.ResponsibilitilyPrice);
            txtBudgetResponsibilityTotalPrice.Text = ToDecimailString(oprDtl.ResponsibilitilyTotalPrice);

            txtBudgetPlanPrice.Text = ToDecimailString(oprDtl.PlanPrice);
            txtBudgetPlanTotalPrice.Text = ToDecimailString(oprDtl.PlanTotalPrice);

            #endregion
        }

        private string ToDecimailString(decimal value)
        {
            return decimal.Round(value, 3).ToString();
        }

        //计划耗用
        void btnPlanUsage_Click(object sender, EventArgs e)
        {
            if (oprDtl == null)
            {
                MessageBox.Show("请选择一条任务明细！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            VGWBSDetailUsageInfoEdit frm = new VGWBSDetailUsageInfoEdit();

            //ContractGroup cg = txtContractGroupCode.Tag as ContractGroup;
            //oprDtl.ContractGroupGUID = cg.Id;
            //oprDtl.ContractGroupCode = cg.Code;
            //oprDtl.ContractGroupType = cg.ContractGroupType;

            oprDtl.WorkAmountUnitGUID = null;
            oprDtl.WorkAmountUnitName = txtBudgetProjectUnit.Text.Trim();
            oprDtl.PriceUnitGUID = null;
            oprDtl.PriceUnitName = txtBudgetPriceUnit.Text.Trim();

            frm.OptionGWBSDtl = oprDtl;
            frm.OptionUsageType = OptUsageType.计划耗用;
            frm.OptionViewState = ViewState;
            frm.ShowDialog();

            oprDtl = frm.OptionGWBSDtl;

            if (!string.IsNullOrEmpty(oprDtl.Id))
                UpdateDetailInfoInGrid(oprDtl, true);

            string matStr = "";// string.IsNullOrEmpty(oprDtl.MainResourceTypeQuality) ? "" : oprDtl.MainResourceTypeQuality + ".";
            matStr += string.IsNullOrEmpty(oprDtl.MainResourceTypeSpec) ? "" : oprDtl.MainResourceTypeSpec + ".";
            matStr += oprDtl.MainResourceTypeName;

            txtMainResourceType.Text = matStr;
            txtMainResourceType.Tag = oprDtl.MainResourceTypeId;

            txtDrawNumber.Text = oprDtl.DiagramNumber;

            btnAccountCostData_Click(btnAccountCostData, new EventArgs());
        }
        //责任耗用
        void btnResponsibleUsage_Click(object sender, EventArgs e)
        {
            if (oprDtl == null)
            {
                MessageBox.Show("请选择一条任务明细！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            VGWBSDetailUsageInfoEdit frm = new VGWBSDetailUsageInfoEdit();

            //ContractGroup cg = txtContractGroupCode.Tag as ContractGroup;
            //oprDtl.ContractGroupGUID = cg.Id;
            //oprDtl.ContractGroupCode = cg.Code;
            //oprDtl.ContractGroupType = cg.ContractGroupType;

            oprDtl.WorkAmountUnitGUID = null;
            oprDtl.WorkAmountUnitName = txtBudgetProjectUnit.Text.Trim();
            oprDtl.PriceUnitGUID = null;
            oprDtl.PriceUnitName = txtBudgetPriceUnit.Text.Trim();

            frm.OptionGWBSDtl = oprDtl;
            frm.OptionUsageType = OptUsageType.责任耗用;
            frm.OptionViewState = ViewState;
            frm.ShowDialog();

            oprDtl = frm.OptionGWBSDtl;

            if (!string.IsNullOrEmpty(oprDtl.Id))
                UpdateDetailInfoInGrid(oprDtl, true);

            string matStr = "";// string.IsNullOrEmpty(oprDtl.MainResourceTypeQuality) ? "" : oprDtl.MainResourceTypeQuality + ".";
            matStr += string.IsNullOrEmpty(oprDtl.MainResourceTypeSpec) ? "" : oprDtl.MainResourceTypeSpec + ".";
            matStr += oprDtl.MainResourceTypeName;

            txtMainResourceType.Text = matStr;
            txtMainResourceType.Tag = oprDtl.MainResourceTypeId;

            txtDrawNumber.Text = oprDtl.DiagramNumber;

            btnAccountCostData_Click(btnAccountCostData, new EventArgs());
        }
        //合同耗用
        void btnContractUsage_Click(object sender, EventArgs e)
        {
            if (oprDtl == null)
            {
                MessageBox.Show("请选择一条任务明细！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            VGWBSDetailUsageInfoEdit frm = new VGWBSDetailUsageInfoEdit();

            //ContractGroup cg = txtContractGroupCode.Tag as ContractGroup;
            //oprDtl.ContractGroupGUID = cg.Id;
            //oprDtl.ContractGroupCode = cg.Code;
            //oprDtl.ContractGroupType = cg.ContractGroupType;

            oprDtl.WorkAmountUnitGUID = null;
            oprDtl.WorkAmountUnitName = txtBudgetProjectUnit.Text.Trim();
            oprDtl.PriceUnitGUID = null;
            oprDtl.PriceUnitName = txtBudgetPriceUnit.Text.Trim();

            frm.OptionGWBSDtl = oprDtl;
            frm.OptionUsageType = OptUsageType.合同耗用;
            frm.OptionViewState = ViewState;
            frm.ShowDialog();

            oprDtl = frm.OptionGWBSDtl;

            if (!string.IsNullOrEmpty(oprDtl.Id))
                UpdateDetailInfoInGrid(oprDtl, true);

            string matStr = "";// string.IsNullOrEmpty(oprDtl.MainResourceTypeQuality) ? "" : oprDtl.MainResourceTypeQuality + ".";
            matStr += string.IsNullOrEmpty(oprDtl.MainResourceTypeSpec) ? "" : oprDtl.MainResourceTypeSpec + ".";
            matStr += oprDtl.MainResourceTypeName;

            txtMainResourceType.Text = matStr;
            txtMainResourceType.Tag = oprDtl.MainResourceTypeId;

            txtDrawNumber.Text = oprDtl.DiagramNumber;

            btnAccountCostData_Click(btnAccountCostData, new EventArgs());
        }
        void btnUnionUsage_Click(object sender, EventArgs e)
        {
            if (oprDtl == null)
            {
                MessageBox.Show("请选择一条任务明细！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            VGWBSDetailUsageInfoEditUnion frm = new VGWBSDetailUsageInfoEditUnion();


            oprDtl.WorkAmountUnitGUID = null;
            oprDtl.WorkAmountUnitName = txtBudgetProjectUnit.Text.Trim();
            oprDtl.PriceUnitGUID = null;
            oprDtl.PriceUnitName = txtBudgetPriceUnit.Text.Trim();

            frm.OptionGWBSDtl_HT = oprDtl;
            frm.OptionGWBSDtl_ZR = oprDtl;
            frm.OptionGWBSDtl_JH = oprDtl;

            frm.OptionViewState = ViewState;
            frm.ShowDialog();

            oprDtl = frm.OptionGWBSDtl_HT;

            if (!string.IsNullOrEmpty(oprDtl.Id))
                UpdateDetailInfoInGrid(oprDtl, true);

            oprDtl = frm.OptionGWBSDtl_JH;

            if (!string.IsNullOrEmpty(oprDtl.Id))
                UpdateDetailInfoInGrid(oprDtl, true);

            oprDtl = frm.OptionGWBSDtl_ZR;

            if (!string.IsNullOrEmpty(oprDtl.Id))
                UpdateDetailInfoInGrid(oprDtl, true);


            string matStr = "";// string.IsNullOrEmpty(oprDtl.MainResourceTypeQuality) ? "" : oprDtl.MainResourceTypeQuality + ".";
            matStr += string.IsNullOrEmpty(oprDtl.MainResourceTypeSpec) ? "" : oprDtl.MainResourceTypeSpec + ".";
            matStr += oprDtl.MainResourceTypeName;

            txtMainResourceType.Text = matStr;
            txtMainResourceType.Tag = oprDtl.MainResourceTypeId;

            txtDrawNumber.Text = oprDtl.DiagramNumber;

            btnAccountCostData_Click(btnAccountCostData, new EventArgs());





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

        #region 工程任务树操作

        void tvwCategory_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            mouseSelectNode = e.Node;
            listFindNodes.Clear();
            if (e.Button == MouseButtons.Right)// && ConstMethod.Contains(lstInstance, e.Node.Tag as CategoryNode)
            {
                tvwCategory.SelectedNode = e.Node;

                if (listCopyNodeDetail.Count == 0)
                {
                    mnuTree.Items[粘贴明细到该节点.Name].Enabled = false;
                    mnuTree.Items[粘贴明细到多个节点.Name].Enabled = false;
                }
                else
                {
                    mnuTree.Items[粘贴明细到该节点.Name].Enabled = true;
                    mnuTree.Items[粘贴明细到多个节点.Name].Enabled = true;
                }

                //非成本核算节点、明细为空、明细中全是编辑状态的、当前节点为叶节点，满足四种情况的任一情况则不允许分摊
                if (oprNode.CostAccFlag == false || lstCurNodeDetail == null
                    || lstCurNodeDetail.All(p => p.State == DocumentState.Edit) || oprNode.CategoryNodeType == NodeType.LeafNode)
                {
                    mnuTree.Items[分摊计划工程量.Name].Enabled = false;
                }
                else
                {
                    mnuTree.Items[分摊计划工程量.Name].Enabled = true;
                }

                mnuTree.Show(tvwCategory, new Point(e.X, e.Y));
            }
        }

        void txtKeyWord_TextChanged(object sender, EventArgs e)
        {
            listFindNodes.Clear();
            showFindNodeIndex = 0;
        }

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
        void mnuTree_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            mnuTree.Hide();

            if (e.ClickedItem.Name == 复制节点明细.Name)
            {
                listCopyNodeDetail.Clear();

                //ObjectQuery oq = new ObjectQuery();
                //oq.AddCriterion(Expression.Eq("TheGWBS.Id", oprNode.Id));
                //oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
                //oq.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
                //oq.AddFetchMode("WorkAmountUnitGUID", NHibernate.FetchMode.Eager);
                //oq.AddOrder(NHibernate.Criterion.Order.Asc("OrderNo"));
                //IList list = model.ObjectQuery(typeof(GWBSDetail), oq);

                //点击时会促发节点选择事件加载所有明细
                foreach (DataGridViewRow row in gridGWBDetail.Rows)
                {
                    GWBSDetail dtl = row.Tag as GWBSDetail;
                    listCopyNodeDetail.Add(dtl);
                }
            }
            else if (e.ClickedItem.Name == 复制节点部分明细.Name)
            {
                DataGridViewRowCollection details = gridGWBDetail.Rows;
                VGWBSTreeDetailSelect vselect = new VGWBSTreeDetailSelect(details);
                vselect.ShowDialog();
                listCopyNodeDetail = vselect.RtnLstDetail;
            }
            else if (e.ClickedItem.Name == 粘贴明细到该节点.Name)
            {
                if (!valideContractGroup())
                {
                    return;
                }

                //要当前lstSelectedGWBSRate节点
                #region 根据选中节点获取相关分摊比例设置信息
                var oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TargetGWBSTreeID", oprNode.Id));
                var lstNodeGWBSRate = model.ObjectQuery(typeof(GWBSTreeDetailRelationship), oq).OfType<GWBSTreeDetailRelationship>().ToList();
                #endregion
                if (lstNodeGWBSRate.Any(p => p.TargetGWBSTreeID == oprNode.Id))
                {
                    MessageBox.Show("【" + oprNode.Name + "】" + "节点下存在分摊的任务明细，不能做粘贴操作！");
                    return;
                }

                SaveCopyDetailByNode();

                tvwCategory_AfterSelect(oprNode, new TreeViewEventArgs(tvwCategory.SelectedNode));
                RefreshTreeNodeStatus(tvwCategory.SelectedNode);
            }
            else if (e.ClickedItem.Name == 粘贴明细到多个节点.Name)
            {
                if (!valideContractGroup())
                {
                    return;
                }

                VSelectGWBSTreeByAddDetail frm = new VSelectGWBSTreeByAddDetail(new MGWBSTree());
                //frm.SelectTree = tvwCategory.Nodes;
                frm.ShowDialog();
                if (frm.SelectResult.Count > 0)
                {
                    #region 根据选中节点获取相关分摊比例设置信息
                    var oq = new ObjectQuery();
                    Disjunction dis = new Disjunction();
                    foreach (var item in frm.SelectResult)
                    {
                        dis.Add(Expression.Eq("TargetGWBSTreeID", item.Id));
                    }
                    oq.AddCriterion(dis);
                    var lstNodeGWBSRate = model.ObjectQuery(typeof(GWBSTreeDetailRelationship), oq).OfType<GWBSTreeDetailRelationship>().ToList();
                    #endregion
                    string errMsg = "";
                    foreach (var item in frm.SelectResult)
                    {
                        /////要当前节点
                        if (item != null && lstNodeGWBSRate.Any(p => p.TargetGWBSTreeID == item.Id))
                        {
                            errMsg += "【" + item.Name + "】，";
                        }
                    }

                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        MessageBox.Show(errMsg.Trim('，') + "节点下存在分摊的任务明细，不能做粘贴操作！");
                        return;
                    }

                    try
                    {
                        FlashScreen.Show("正在复制数据,请稍候......");
                        bool flag = model.SaveCopyDetailByNode((txtContractGroupName.Tag as ContractGroup), frm.SelectResult, listCopyNodeDetail);
                        if (!flag)
                        {
                            FlashScreen.Close();
                            MessageBox.Show("操作失败，请重试！");
                            return;
                        }

                        currNode = null;//清掉选择的节点,以便重新加载界面信息
                        tvwCategory_AfterSelect(tvwCategory, new TreeViewEventArgs(tvwCategory.SelectedNode));
                    }
                    catch (Exception ex)
                    {
                        FlashScreen.Close();
                        MessageBox.Show("复制出错，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
                    }
                    finally
                    {
                        FlashScreen.Close();
                    }
                }
            }
            else if (e.ClickedItem.Name == 导入成本明细Excel表.Name)
            {
                VImportGWBSDetail frm = new VImportGWBSDetail();
                frm.DefaultGWBSTreeNode = tvwCategory.SelectedNode;
                frm.ShowDialog();

                TreeNode node = tvwCategory.SelectedNode;
                tvwCategory.SelectedNode = tvwCategory.Nodes[0];
                tvwCategory.SelectedNode = node;

                //tvwCategory_AfterSelect(tvwCategory, new TreeViewEventArgs(tvwCategory.SelectedNode));
                RefreshTreeNodeStatus(tvwCategory.SelectedNode);
            }
            else if (e.ClickedItem.Name == 导入成本明细Excel表_New.Name)
            {
                VImportGWBSDetail_New frm = new VImportGWBSDetail_New(10);
                frm.DefaultGWBSTreeNode = tvwCategory.SelectedNode;
                frm.contract = txtContractGroupName.Tag as ContractGroup;
                frm.ShowDialog();

                TreeNode node = tvwCategory.SelectedNode;
                tvwCategory.SelectedNode = tvwCategory.Nodes[0];
                tvwCategory.SelectedNode = node;
                //将用户选择的契约回填
                if (frm.contract != null)
                {
                    ContractGroup cg = frm.contract;
                    txtContractGroupName.Text = cg.ContractName;
                    txtContractGroupType.Text = cg.ContractGroupType;
                    txtContractGroupName.Tag = cg;
                }

                RefreshTreeNodeStatus(tvwCategory.SelectedNode);
            }
            else if (e.ClickedItem.Name == 编辑成本数据.Name)
            {
                VGWBSDetailCostEditAndUsageEdit frm = new VGWBSDetailCostEditAndUsageEdit();
                frm.DefaultGWBSTreeNode = tvwCategory.SelectedNode;
                frm.ShowDialog();

                TreeNode node = tvwCategory.SelectedNode;
                tvwCategory.SelectedNode = tvwCategory.Nodes[0];
                tvwCategory.SelectedNode = node;
                //tvwCategory_AfterSelect(tvwCategory, new TreeViewEventArgs(tvwCategory.Nodes[0]));
                //tvwCategory_AfterSelect(tvwCategory, new TreeViewEventArgs(node));
                //tvwCategory_AfterSelect(tvwCategory, new TreeViewEventArgs(tvwCategory.SelectedNode));
            }
            else if (e.ClickedItem.Name == 商务数据统计ToolStripMenuItem.Name)
            {
                GWBSTree wbs = new GWBSTree();
                if (tvwCategory.SelectedNode.Tag != null)
                {
                    wbs = tvwCategory.SelectedNode.Tag as GWBSTree;
                    VGWBSBusinessStatistics bs = new VGWBSBusinessStatistics(wbs);
                    bs.ShowDialog();

                    TreeNode node = tvwCategory.SelectedNode;
                    tvwCategory.SelectedNode = tvwCategory.Nodes[0];
                    tvwCategory.SelectedNode = node;
                    //tvwCategory_AfterSelect(tvwCategory, new TreeViewEventArgs(tvwCategory.SelectedNode));
                }
            }
            else if (e.ClickedItem.Name == 分摊计划工程量.Name)
            {
                //GWBSTree wbs = new GWBSTree();
                //if (tvwCategory.SelectedNode.Tag != null)
                //{
                //    //wbs = tvwCategory.SelectedNode.Tag as GWBSTree;
                //    VPlanProjectAmountSharing vs = new VPlanProjectAmountSharing(tvwCategory.SelectedNode);
                //    vs.ShowDialog();

                //    RefreshTreeNodeAndChildsStatus(tvwCategory.SelectedNode);
                //}

                if (tvwCategory.SelectedNode.Tag != null)
                {
                    GWBSTree wbs = tvwCategory.SelectedNode.Tag as GWBSTree;
                    //VPlanProjectAmountShareByRate vs = new VPlanProjectAmountShareByRate(wbs.Id);
                    VPlanProjectAmountShareRateSet vs = new VPlanProjectAmountShareRateSet(wbs.Id, wbs.SysCode);
                    vs.ShowDialog();
                    RefreshTreeNodeAndChildsStatus(tvwCategory.SelectedNode);
                }
            }
            else if (e.ClickedItem.Name == 选择成本项区域.Name)
            {
                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();
                string[] sysCodes = oprNode.SysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < sysCodes.Length; i++)
                {
                    string sysCode = "";
                    for (int j = 0; j <= i; j++)
                    {
                        sysCode += sysCodes[j] + ".";
                    }
                    dis.Add(Expression.Eq("GwbsSyscode", sysCode));
                }
                oq.AddCriterion(dis);
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                IList list = model.ObjectQuery(typeof(CostItemsZoning), oq);
                if (list != null && list.Count > 0)
                {
                    CostItemsZoning cost = list[0] as CostItemsZoning;
                    MessageBox.Show("节点【" + cost.GwbsName + "】已选择区域");
                    return;
                }
                VCostItemsZoning frm = new VCostItemsZoning();
                frm.ShowDialog();
                CostItemCategory cateResult = frm.Result;
                if (cateResult != null)
                {
                    CostItemsZoning z = model.CostItemZoningChange(oprNode, cateResult, projectInfo);
                    txtCostItemsZoning.Tag = z;
                    txtCostItemsZoning.Text = z.CostItemsCateName;
                }
            }
            else if (e.ClickedItem.Name == 删除成本项区域.Name)
            {
                if (txtCostItemsZoning.Tag == null)
                {
                    MessageBox.Show("未找到要删除的成本项区域！");
                    return;
                }
                if (MessageBox.Show("删除当前成本项区域，根据当前区域下做的任务明细，也要一并删除，确定要删除吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    try
                    {
                        model.DeleteCostItemZoning(txtCostItemsZoning.Tag as CostItemsZoning);
                        MessageBox.Show("删除成功！");
                        tvwCategory_AfterSelect(tvwCategory, new TreeViewEventArgs(currNode));
                        txtCostItemsZoning.Tag = null;
                        txtCostItemsZoning.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("删除失败：" + ExceptionUtil.ExceptionMessage(ex));
                    }
                }
            }
            else if (string.Equals(e.ClickedItem.Text, "批量签证变更"))
            {
                if (tvwCategory.SelectedNode != null && tvwCategory.SelectedNode.Tag != null)
                {
                    VGWBSDetailBatchChange oVGWBSDetailBatchChange = new VGWBSDetailBatchChange();
                    oVGWBSDetailBatchChange.DefaultGWBSTreeNode = tvwCategory.SelectedNode;
                    oVGWBSDetailBatchChange.StartPosition = FormStartPosition.CenterScreen;
                    oVGWBSDetailBatchChange.ShowDialog();
                }

            }
        }

        private bool valideContractGroup()
        {
            if (oprDtl == null)
            {
                if (txtContractGroupName.Text.Trim() == "" || txtContractGroupName.Tag == null)
                {
                    MessageBox.Show("请选择驱动契约组！");

                    VSelectWBSContractGroup frm = new VSelectWBSContractGroup(new MWBSContractGroup(), false);
                    if (txtContractGroupName.Tag != null)
                    {
                        frm.DefaultSelectedContract = txtContractGroupName.Tag as ContractGroup;
                    }
                    frm.ShowDialog();
                    if (frm.SelectResult.Count > 0)
                    {
                        ContractGroup cg = frm.SelectResult[0] as ContractGroup;
                        txtContractGroupName.Text = cg.ContractName;
                        txtContractGroupType.Text = cg.ContractGroupType;
                        txtContractGroupDesc.Text = cg.ContractDesc;
                        txtContractGroupName.Tag = cg;

                        if (oprDtl != null && btnSaveBaseInfo.Enabled && cbUpdateContract.Checked)
                        {
                            txtDtlContractGroupName.Text = cg.ContractName;
                            txtDtlContractGroupType.Text = cg.ContractGroupType;
                        }

                        return true;
                    }
                    return false;
                }
                return true;
            }
            else
            {
                if (txtContractGroupName.Text.Trim() == "" || txtContractGroupName.Tag == null)
                {
                    MessageBox.Show("请选择驱动契约组！");

                    VSelectWBSContractGroup frm = new VSelectWBSContractGroup(new MWBSContractGroup(), false);
                    if (txtContractGroupName.Tag != null)
                    {
                        frm.DefaultSelectedContract = txtContractGroupName.Tag as ContractGroup;
                    }
                    frm.ShowDialog();
                    if (frm.SelectResult.Count > 0)
                    {
                        ContractGroup cg = frm.SelectResult[0] as ContractGroup;
                        txtContractGroupName.Text = cg.ContractName;
                        txtContractGroupType.Text = cg.ContractGroupType;
                        txtContractGroupDesc.Text = cg.ContractDesc;
                        txtContractGroupName.Tag = cg;

                        if (oprDtl != null && btnSaveBaseInfo.Enabled && cbUpdateContract.Checked)
                        {
                            txtDtlContractGroupName.Text = cg.ContractName;
                            txtDtlContractGroupType.Text = cg.ContractGroupType;
                        }

                        return true;
                    }
                    return false;
                }
                return true;
            }

        }
        private bool valideChangeContractGroup()
        {
            if (txtChangeContractName.Text.Trim() == "" || txtChangeContractName.Tag == null)
            {
                tabBaseInfo.SelectedIndex = 1;

                MessageBox.Show("修改的任务明细状态为“执行中”，请选择变更契约！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                VSelectWBSContractGroup frm = new VSelectWBSContractGroup(new MWBSContractGroup(), false);
                if (txtChangeContractName.Tag != null)
                {
                    frm.DefaultSelectedContract = txtChangeContractName.Tag as ContractGroup;
                }
                frm.ShowDialog();
                if (frm.SelectResult.Count > 0)
                {
                    ContractGroup cg = frm.SelectResult[0] as ContractGroup;
                    txtChangeContractName.Text = cg.ContractName;
                    txtChangeContractName.Tag = cg;
                    txtChangeContractType.Text = cg.ContractGroupType;
                    return true;
                }
                return false;
            }
            return true;
        }
        private void SaveCopyDetailByNode()
        {
            if (listCopyNodeDetail.Count > 0)
            {
                ContractGroup selectedContractGroup = txtContractGroupName.Tag as ContractGroup;

                //保存复制的明细
                IList listResult = model.SaveCopyDetailByNode(selectedContractGroup, oprNode, listCopyNodeDetail);

                oprNode = listResult[0] as GWBSTree;
                List<GWBSDetail> listNewDtl = listResult[1] as List<GWBSDetail>;

                //更新节点tag值
                tvwCategory.SelectedNode.Tag = oprNode;

                //更新WBS合价信息
                ShownTaskNodeTotalPriceInfo();

                //添加新增的明细到Grid
                foreach (GWBSDetail dtl in listNewDtl)
                {
                    AddDetailInfoInGrid(dtl, false);
                }
                gridGWBDetail.ClearSelection();

                ClearDetailAll();
            }
        }

        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (currNode != null && currNode.Name == e.Node.Name)
            {
                return;
            }

            if (gbAdd.Enabled == true)
            {
                if (MessageBox.Show("是否放弃文档操作！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    AddAfter();
                }
                else
                {
                    tvwCategory.AfterSelect -= new TreeViewEventHandler(tvwCategory_AfterSelect);
                    tvwCategory.SelectedNode = currNode;
                    tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);
                    return;
                }
            }

            try
            {


                currNode = tvwCategory.SelectedNode;

                oprNode = tvwCategory.SelectedNode.Tag as GWBSTree;

                if (tabControlDtl.SelectedTab == tabPageWBSBaseInfo)
                {
                    oprNode = LoadRelaPBS(oprNode);

                    tvwCategory.SelectedNode.Tag = oprNode;

                    this.ShownTaskNodeInfo();

                    LoadCostItemsZoning();

                    tabPageWBSBaseInfo.Tag = oprNode;
                }
                else if (tabControlDtl.SelectedTab == tabPageWBSDocInfo)
                {
                    LoadTaskDocumentInfo();

                    tabPageWBSDocInfo.Tag = oprNode;
                }

                #region 根据选中节点获取相关分摊比例设置信息
                var oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TargetGWBSTreeID", oprNode.Id));
                lstSelectedGWBSRate = model.ObjectQuery(typeof(GWBSTreeDetailRelationship), oq).OfType<GWBSTreeDetailRelationship>().ToList();
                #endregion


            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
            }
        }

        void tvwCategory_AfterExpand(object sender, TreeViewEventArgs e)
        {
            RefreshTreeNodeAndChildsStatus(e.Node);
            // TreeNode tempNode = new TreeNode();

            //string countSql = "select count(0) from THD_GWBSDetail where ParentId=";
            //foreach (TreeNode tn in e.Node.Nodes)
            //{
            //    string sqlQuery = countSql + "'" + tn.Name + "'";
            //    DataSet ds = model.SearchSQL(sqlQuery);
            //    if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) == 0)
            //    {
            //        tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
            //        tn.ForeColor = ColorTranslator.FromHtml("#000000");
            //    }
            //    else
            //    {
            //        tn.BackColor = tempNode.BackColor;
            //        tn.ForeColor = tempNode.ForeColor;
            //    }
            //}


            //string countSql = "select distinct ParentId from THD_GWBSDetail where ParentId in ( ";
            //string where = "";
            //foreach (TreeNode tn in e.Node.Nodes)
            //{
            //    where += "'" + tn.Name + "',";
            //}
            //countSql += where.Substring(0, where.Length - 1) + ")";

            //DataSet ds = model.SearchSQL(countSql);
            //DataTable dt = ds.Tables[0];

            //List<string> listParentId = new List<string>();

            //foreach (DataRow row in dt.Rows)
            //{
            //    string parentId = row[0].ToString();
            //    listParentId.Add(parentId);
            //}

            //foreach (TreeNode tn in e.Node.Nodes)
            //{
            //    if (listParentId.Contains(tn.Name) == false)
            //    {
            //        tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
            //        tn.ForeColor = ColorTranslator.FromHtml("#000000");
            //    }
            //    else
            //    {
            //        tn.BackColor = tempNode.BackColor;
            //        tn.ForeColor = tempNode.ForeColor;
            //    }
            //}
        }

        void btnRefreshTreeStatus_Click(object sender, EventArgs e)
        {
            RefreshTreeNodeAndChildsStatus(tvwCategory.SelectedNode);
        }

        //刷新树节点是否有明细的状态
        private void RefreshTreeNodeStatus(TreeNode selectedNode)
        {
            if (selectedNode != null)
            {
                TreeNode tempNode = new TreeNode();

                //查询
                string countSql = "select count(0) from THD_GWBSDetail where ParentId = '" + selectedNode.Name + "'";

                DataSet ds = model.SearchSQL(countSql);
                DataTable dt = ds.Tables[0];

                if (Convert.ToInt32(dt.Rows[0][0]) == 0)//节点下无任务明细
                {
                    selectedNode.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    selectedNode.ForeColor = ColorTranslator.FromHtml("#000000");
                }
                else
                {
                    selectedNode.BackColor = tempNode.BackColor;
                    selectedNode.ForeColor = tempNode.ForeColor;
                }
            }
        }
        //刷新树节点是否有明细的状态
        private void RefreshTreeNodeAndChildsStatus(TreeNode selectedNode)
        {
            if (selectedNode != null)
            {
                TreeNode tempNode = new TreeNode();

                //查询
                string countSql = "select distinct ParentId from THD_GWBSDetail where ParentId in ( ";
                string where = "";

                where = "'" + selectedNode.Name + "'";

                if (selectedNode.Nodes.Count > 0)
                {
                    where += ",";

                    foreach (TreeNode tn in selectedNode.Nodes)
                    {
                        where += "'" + tn.Name + "',";
                    }

                    where = where.Substring(0, where.Length - 1);
                }
                countSql += where + ")";

                DataSet ds = model.SearchSQL(countSql);
                DataTable dt = ds.Tables[0];

                //设置
                List<string> listParentId = new List<string>();

                foreach (DataRow row in dt.Rows)
                {
                    string parentId = row[0].ToString();
                    listParentId.Add(parentId);
                }

                if (listParentId.Contains(selectedNode.Name) == false)
                {
                    selectedNode.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    selectedNode.ForeColor = ColorTranslator.FromHtml("#000000");
                }
                else
                {
                    selectedNode.BackColor = tempNode.BackColor;
                    selectedNode.ForeColor = tempNode.ForeColor;
                }

                foreach (TreeNode tn in selectedNode.Nodes)
                {
                    if (listParentId.Contains(tn.Name) == false)
                    {
                        tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                        tn.ForeColor = ColorTranslator.FromHtml("#000000");
                    }
                    else
                    {
                        tn.BackColor = tempNode.BackColor;
                        tn.ForeColor = tempNode.ForeColor;
                    }
                }
            }
        }

        //显示任务信息
        private void ShownTaskNodeInfo()
        {
            try
            {
                ClearAll();

                //基本信息
                this.txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath;

                this.txtTaskCode.Text = oprNode.Code;
                this.txtTaskName.Text = oprNode.Name;

                this.txtTaskState.Text = StaticMethod.GetWBSTaskStateText(oprNode.TaskState);
                this.txtTaskStateTime.Text = oprNode.TaskStateTime.ToString();

                if (oprNode.ListRelaPBS.Count > 0)
                {
                    foreach (GWBSRelaPBS rela in oprNode.ListRelaPBS)
                    {
                        this.cbRelaPBS.Items.Add(rela.ThePBS);
                    }
                    cbRelaPBS.DisplayMember = "FullPath";
                    cbRelaPBS.SelectedIndex = 0;
                }

                this.txtTaskWBSType.Text = oprNode.ProjectTaskTypeName;
                this.txtTaskWBSType.Tag = oprNode.ProjectTaskTypeGUID;

                //this.txtTaskRelaContractGroupCode.Text = oprNode.ContractGroupCode;
                //this.txtTaskRelaContractGroupCode.Tag = oprNode.ContractGroupGUID;

                this.txtTaskOwner.Text = oprNode.OwnerName;
                //this.txtTaskOrg.Text = oprNode.BearOrgName;

                //if (oprNode.ManagementMode != 0)
                //    this.txtTaskManagementMethod.Text = oprNode.ManagementMode.ToString();

                this.txtTaskStartTime.Text = oprNode.TaskPlanStartTime != null ? oprNode.TaskPlanStartTime.Value.ToShortDateString() : "";
                this.txtTaskEndTime.Text = oprNode.TaskPlanEndTime != null ? oprNode.TaskPlanEndTime.Value.ToShortDateString() : "";

                txtTaskDesc.Text = oprNode.Describe;
                ShownTaskNodeTotalPriceInfo();
                txtPriceUnit.Text = oprNode.PriceAmountUnitName;
                txtFigureProgress.Text = oprNode.AddUpFigureProgress.ToString();
                txtCheckBatchNum.Text = oprNode.CheckBatchNumber.ToString();
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

                LoadGWBSDetailInGrid();

                RefreshControls(MainViewState.Browser);
            }
            catch (Exception exp)
            {
                MessageBox.Show("显示出错：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        private void ShownTaskNodeTotalPriceInfo()
        {
            txtContractTotalPrice.Text = oprNode.ContractTotalPrice.ToString();
            txtResponsibilityTotalPrice.Text = oprNode.ResponsibilityTotalPrice.ToString();
            txtPlanTotalPrice.Text = oprNode.PlanTotalPrice.ToString();
        }

        private GWBSTree LoadRelaPBS(GWBSTree wbs)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", wbs.Id));
            oq.AddFetchMode("ListRelaPBS.ThePBS", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ProjectTaskTypeGUID", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof(GWBSTree), oq);
            if (list.Count > 0)
            {
                wbs = list[0] as GWBSTree;
            }
            return wbs;
        }
        private GWBSDetail LoadRelaObjByTaskDetail(GWBSDetail dtl)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", dtl.Id));
            oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof(GWBSDetail), oq);
            if (list.Count > 0)
            {
                dtl = list[0] as GWBSDetail;
            }
            return dtl;
        }
        //显示任务明细信息
        private void ShownTaskDetailInfo(bool isLazy)
        {
            try
            {
                ClearDetailAll();

                //基本信息
                txtDtlName.Text = oprDtl.Name;
                //txtDtlCode.Text = oprDtl.Code;

                if (isLazy)
                {
                    ObjectQuery oqDtl = new ObjectQuery();
                    oqDtl.AddCriterion(Expression.Eq("Id", oprDtl.Id));
                    oqDtl.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
                    oqDtl.AddFetchMode("ListCostSubjectDetails", NHibernate.FetchMode.Eager);
                    oqDtl.AddFetchMode("WorkAmountUnitGUID", NHibernate.FetchMode.Eager);
                    oqDtl.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
                    oqDtl.AddFetchMode("ListCostSubjectDetails.ListGWBSDtlCostSubRate", NHibernate.FetchMode.Eager);

                    oprDtl = model.ObjectQuery(typeof(GWBSDetail), oqDtl)[0] as GWBSDetail;
                }

                if (oprDtl.TheCostItem != null)
                {
                    if (!string.IsNullOrEmpty(oprDtl.TheCostItem.QuotaCode))
                        txtDtlCostItem.Text = oprDtl.TheCostItem.QuotaCode;
                    else
                        txtDtlCostItem.Text = oprDtl.TheCostItem.Name;

                    txtDtlCostItem.Tag = oprDtl.TheCostItem;
                }

                string matStr = string.IsNullOrEmpty(oprDtl.MainResourceTypeQuality) ? "" : oprDtl.MainResourceTypeQuality + ".";
                matStr += string.IsNullOrEmpty(oprDtl.MainResourceTypeSpec) ? "" : oprDtl.MainResourceTypeSpec + ".";
                matStr += oprDtl.MainResourceTypeName;

                txtMainResourceType.Text = matStr;
                txtMainResourceType.Tag = oprDtl.MainResourceTypeId;

                txtDrawNumber.Text = oprDtl.DiagramNumber;

                txtDtlDesc.Text = oprDtl.ContentDesc;

                txtDtlState.Text = StaticMethod.GetWBSTaskStateText(oprDtl.State);
                txtDtlStateTime.Text = oprDtl.CurrentStateTime.ToString();

                txtDtlContractGroupName.Text = oprDtl.ContractGroupName;
                txtDtlContractGroupType.Text = oprDtl.ContractGroupType;

                cbResponseAccount.Checked = oprDtl.ResponseFlag == 1;
                cbCostAccount.Checked = oprDtl.CostingFlag == 1;
                cbProductConfirm.Checked = oprDtl.ProduceConfirmFlag == 1;
                cbSubContractFee.Checked = oprDtl.SubContractFeeFlag;
                txtSubcontractFee.Text = StaticMethod.DecimelTrimEnd0(oprDtl.SubContractStepRate);

                //预算信息
                txtBudgetProjectUnit.Text = oprDtl.WorkAmountUnitName;
                txtBudgetProjectUnit.Tag = oprDtl.WorkAmountUnitGUID;
                txtBudgetPriceUnit.Text = oprDtl.PriceUnitName;
                txtBudgetPriceUnit.Tag = oprDtl.PriceUnitGUID;

                txtBudgetContractProjectAmount.Text = oprDtl.ContractProjectQuantity.ToString();
                txtBudgetContractPrice.Text = oprDtl.ContractPrice.ToString();
                txtBudgetContractTotalPrice.Text = oprDtl.ContractTotalPrice.ToString();

                txtBudgetResponsibilityProjectAmount.Text = oprDtl.ResponsibilitilyWorkAmount.ToString();
                txtBudgetResponsibilityPrice.Text = oprDtl.ResponsibilitilyPrice.ToString();
                txtBudgetResponsibilityTotalPrice.Text = oprDtl.ResponsibilitilyTotalPrice.ToString();

                txtBudgetPlanProjectAmount.Text = oprDtl.PlanWorkAmount.ToString();
                txtBudgetPlanPrice.Text = oprDtl.PlanPrice.ToString();
                txtBudgetPlanTotalPrice.Text = oprDtl.PlanTotalPrice.ToString();

                //执行情况
                txtAddupAccProjectQuantity.Text = oprDtl.AddupAccQuantity.ToString();
                txtAddupAccProQuantityUnit.Text = oprDtl.WorkAmountUnitName;
                txtAddupAccFigureProgress.Text = oprDtl.AddupAccFigureProgress.ToString();

                txtAddupConfirmProQuantity.Text = oprDtl.QuantityConfirmed.ToString();
                txtAddupConfirmProQnyUnit.Text = oprDtl.WorkAmountUnitName;
                txtAddupConfirmFigureProgress.Text = oprDtl.ProgressConfirmed.ToString();

                txtExecuteDesc.Text = oprDtl.DetailExecuteDesc;


                //加载分科目成本信息
                //gridCostSubject.Rows.Clear();
                //ObjectQuery oq = new ObjectQuery();
                //oq.AddCriterion(Expression.Eq("TheGWBSDetail.Id", oprDtl.Id));
                //oq.AddOrder(NHibernate.Criterion.Order.Asc("CreateTime"));
                //IList listQuota = model.ObjectQuery(typeof(GWBSDetailCostSubject), oq);
                //foreach (GWBSDetailCostSubject subject in oprDtl.ListCostSubjectDetails)
                //{
                //    AddCostSubjectInfoInGrid(subject);
                //}


            }
            catch (Exception exp)
            {
                MessageBox.Show("显示出错：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        private void LoadGWBSDetailInGrid()
        {
            if (oprNode != null)
            {
                //ObjectQuery oq = new ObjectQuery();
                //oq.AddCriterion(Expression.Eq("TheGWBS.Id", oprNode.Id));
                //oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
                ////oq.AddFetchMode("WorkAmountUnitGUID", NHibernate.FetchMode.Eager);
                ////oq.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);

                //oq.AddOrder(NHibernate.Criterion.Order.Asc("OrderNo"));

                //IList list = model.ObjectQuery(typeof(GWBSDetail), oq);

                lstCurNodeDetail = model.GetWBSDetail(oprNode.Id, "");
                //oprNode.Details.ToList().AddRange(lstCurNodeDetail);

                //oprNode.Details.AddAll(lstCurNodeDetail);

                gridGWBDetail.Rows.Clear();
                lstCurNodeDetail = lstCurNodeDetail.OrderBy(p => p.OrderNo).ToList();
                //oprNode.Details = new HashedSet<GWBSDetail>();
                foreach (GWBSDetail dtl in lstCurNodeDetail)
                {
                    //oprNode.Details.Add(dtl);
                    AddDetailInfoInGrid(dtl, true);
                }
                gridGWBDetail.ClearSelection();
            }
        }
        /// <summary>
        /// 加载成本项区域
        /// </summary>
        private void LoadCostItemsZoning()
        {
            if (txtCostItemsZoning.Tag != null)
            {
                CostItemsZoning z = txtCostItemsZoning.Tag as CostItemsZoning;
                if (oprNode.SysCode.Contains(z.GwbsSyscode)) return;
            }
            //else//2016-9-19
            //{
            //    return;
            //}
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            string[] sysCodes = oprNode.SysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < sysCodes.Length; i++)
            {
                string sysCode = "";
                for (int j = 0; j <= i; j++)
                {
                    sysCode += sysCodes[j] + ".";
                }
                dis.Add(Expression.Eq("GwbsSyscode", sysCode));
            }
            oq.AddCriterion(dis);
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));

            IList list = model.ObjectQuery(typeof(CostItemsZoning), oq);
            if (list != null && list.Count > 0)
            {
                CostItemsZoning z = list[0] as CostItemsZoning;
                txtCostItemsZoning.Tag = z;
                txtCostItemsZoning.Text = z.CostItemsCateName;
            }
            else
            {
                txtCostItemsZoning.Tag = null;
                txtCostItemsZoning.Text = "";
            }
        }

        private void ClearAll()
        {
            //基本信息
            this.txtCurrentPath.Text = "";
            this.txtTaskCode.Text = "";
            this.txtTaskName.Text = "";
            this.txtTaskState.Text = "";
            this.txtTaskStateTime.Text = "";
            this.cbRelaPBS.Items.Clear();
            this.txtTaskWBSType.Text = "";
            this.txtTaskOwner.Text = "";
            this.txtTaskStartTime.Text = "";
            this.txtTaskEndTime.Text = "";

            txtTaskDesc.Text = "";
            txtContractTotalPrice.Text = "";
            txtResponsibilityTotalPrice.Text = "";
            txtPlanTotalPrice.Text = "";
            txtPriceUnit.Text = "";
            txtFigureProgress.Text = "";
            txtCheckBatchNum.Text = "";

            for (int i = 0; i < cbListCheckRequire.Items.Count; i++)
            {
                cbListCheckRequire.SetItemChecked(i, false);
            }

            gridGWBDetail.Rows.Clear();
            oprDtl = null;
            ClearDetailAll();
        }

        private bool ValideSave()
        {
            try
            {
                if (oprDtl == null)
                {
                    if (oprNode == null)
                    {
                        MessageBox.Show("请先选择要添加明细的工程WBS节点！");
                        return false;
                    }
                    else if (!valideContractGroup())
                    {
                        return false;
                    }

                    AddAndSetNewDetailInfo();
                }
                //此处只做了当前节点下的重复验证,发布时如果是成本核算明细时，会做同一路径重复验证
                if (VaildateRepeate() == false)
                {
                    MessageBox.Show("当前节点下已存在与任务明细" + oprDtl.Name + "”的成本项、主资源类型、图号相同的明细，请检查！");
                    return false;
                }

                if ((string.IsNullOrEmpty(oprDtl.TheProjectGUID) || string.IsNullOrEmpty(oprDtl.TheProjectName)) && projectInfo != null)
                {
                    oprDtl.TheProjectGUID = projectInfo.Id;
                    oprDtl.TheProjectName = projectInfo.Name;
                }

                #region 基本信息
                if (txtDtlName.Text.Trim() == "")
                {
                    tabBaseInfo.SelectedIndex = 0;
                    MessageBox.Show("明细名称不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDtlName.Focus();
                    return false;
                }
                if (cbResponseAccount.Checked == false && cbCostAccount.Checked == false && cbProductConfirm.Checked == false && cbSubContractFee.Checked == false)
                {
                    MessageBox.Show("请选择明细类型！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                //if (cbCostAccount.Checked == false && cbProductConfirm.Checked)
                //{
                //    MessageBox.Show("不能单独添加“生产确认明细”，请使用分摊方式从“成本核算明细”上分摊工程量，或将“成本核算明细”和“生产确认明细”做到一个明细上！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return false;
                //}
                oprDtl.Name = txtDtlName.Text.Trim();

                if (txtDtlCostItem.Tag != null)//成本项
                {
                    //
                }
                else
                {
                    tabBaseInfo.SelectedIndex = 0;
                    MessageBox.Show("请选择成本项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDtlCostItem.Focus();
                    return false;
                }

                if (txtMainResourceType.Tag != null && txtMainResourceType.Tag.GetType() == typeof(ResourceGroup))
                {
                    ResourceGroup rg = txtMainResourceType.Tag as ResourceGroup;
                    oprDtl.MainResourceTypeId = rg.ResourceTypeGUID;
                    oprDtl.MainResourceTypeName = rg.ResourceTypeName;
                    oprDtl.MainResourceTypeQuality = rg.ResourceTypeQuality;
                    oprDtl.MainResourceTypeSpec = rg.ResourceTypeSpec;
                    oprDtl.DiagramNumber = rg.DiagramNumber;
                }
                else if (txtMainResourceType.Text.Trim() == "" && txtMainResourceType.Tag == null)
                {
                    oprDtl.MainResourceTypeId = string.Empty;
                    oprDtl.MainResourceTypeName = string.Empty;
                    oprDtl.MainResourceTypeQuality = string.Empty;
                    oprDtl.MainResourceTypeSpec = string.Empty;
                    oprDtl.DiagramNumber = null;

                    var queryUsage = from u in oprDtl.ListCostSubjectDetails
                                     where u.MainResTypeFlag == true
                                     select u;

                    if (queryUsage.Count() > 0)
                    {
                        for (int i = 0; i < queryUsage.Count(); i++)
                        {
                            GWBSDetailCostSubject dtlUsage = queryUsage.ElementAt(i);
                            dtlUsage.MainResTypeFlag = false;
                        }
                    }
                }

                oprDtl.DiagramNumber = txtDrawNumber.Text.Trim();

                oprDtl.ContentDesc = txtDtlDesc.Text.Trim();

                //契约
                ContractGroup cg = txtContractGroupName.Tag as ContractGroup;
                if (string.IsNullOrEmpty(oprDtl.Id) && cg != null && oprDtl.ContractGroupGUID != cg.Id)//新增
                {
                    oprDtl.ContractGroupGUID = cg.Id;
                    oprDtl.ContractGroupName = cg.ContractName;
                    oprDtl.ContractGroupCode = cg.Code;
                    oprDtl.ContractGroupType = cg.ContractGroupType;
                }
                else if (cbUpdateContract.Checked && cg != null && oprDtl.ContractGroupGUID != cg.Id)//修改
                {
                    oprDtl.ContractGroupGUID = cg.Id;
                    oprDtl.ContractGroupName = cg.ContractName;
                    oprDtl.ContractGroupCode = cg.Code;
                    oprDtl.ContractGroupType = cg.ContractGroupType;

                    txtDtlContractGroupName.Text = cg.ContractName;
                    txtDtlContractGroupType.Text = cg.ContractGroupType;
                }
                //xl 20160912  为了提供契约修改
                if (!string.IsNullOrEmpty(oprDtl.Id) && txtChangeContractName.Tag != null)
                {
                    ContractGroup cg1 = txtChangeContractName.Tag as ContractGroup;
                    if (cg1 != null)
                    {
                        oprDtl.ContractGroupGUID = cg1.Id;
                        oprDtl.ContractGroupName = cg1.ContractName;
                        oprDtl.ContractGroupCode = cg1.Code;
                        oprDtl.ContractGroupType = cg1.ContractGroupType;

                    }
                }

                #endregion

                #region 预算信息
                if (txtBudgetProjectUnit.Text.Trim() != "" && txtBudgetProjectUnit.Tag != null)
                {
                    oprDtl.WorkAmountUnitGUID = txtBudgetProjectUnit.Tag as StandardUnit;
                    if (oprDtl.WorkAmountUnitGUID != null)
                        oprDtl.WorkAmountUnitName = oprDtl.WorkAmountUnitGUID.Name;
                }
                else
                {
                    oprDtl.WorkAmountUnitGUID = null;
                    oprDtl.WorkAmountUnitName = "";
                }

                if (txtBudgetPriceUnit.Text.Trim() != "" && txtBudgetPriceUnit.Tag != null)
                {
                    oprDtl.PriceUnitGUID = txtBudgetPriceUnit.Tag as StandardUnit;
                    if (oprDtl.PriceUnitGUID != null)
                        oprDtl.PriceUnitName = oprDtl.PriceUnitGUID.Name;
                }
                else
                {
                    oprDtl.PriceUnitGUID = null;
                    oprDtl.PriceUnitName = "";
                }

                try
                {
                    decimal ContractProjectQuantity = 0;
                    if (txtBudgetContractProjectAmount.Text.Trim() != "")
                        ContractProjectQuantity = Convert.ToDecimal(txtBudgetContractProjectAmount.Text);

                    //oprDtl.ContractProjectQuantity = ContractProjectQuantity;
                }
                catch
                {
                    tabBaseInfo.SelectedIndex = 1;
                    MessageBox.Show("合同工程量格式填写不正确！");
                    txtBudgetContractProjectAmount.Focus();
                    return false;
                }

                try
                {
                    decimal ContractPrice = 0;
                    if (txtBudgetContractPrice.Text.Trim() != "")
                        ContractPrice = Convert.ToDecimal(txtBudgetContractPrice.Text);

                    //oprDtl.ContractPrice = ContractPrice;
                }
                catch
                {
                    tabBaseInfo.SelectedIndex = 1;
                    MessageBox.Show("合同单价格式填写不正确！");
                    txtBudgetContractPrice.Focus();
                    return false;
                }

                try
                {
                    decimal ContractTotalPrice = 0;
                    if (txtBudgetContractTotalPrice.Text.Trim() != "")
                        ContractTotalPrice = Convert.ToDecimal(txtBudgetContractTotalPrice.Text);

                    oprDtl.ContractTotalPrice = ContractTotalPrice;
                }
                catch
                {
                    tabBaseInfo.SelectedIndex = 1;
                    MessageBox.Show("合同合价格式填写不正确！");
                    txtBudgetContractTotalPrice.Focus();
                    return false;
                }

                try
                {
                    decimal ResponsibilitilyWorkAmount = 0;
                    if (txtBudgetResponsibilityProjectAmount.Text.Trim() != "")
                        ResponsibilitilyWorkAmount = Convert.ToDecimal(txtBudgetResponsibilityProjectAmount.Text);

                    //oprDtl.ResponsibilitilyWorkAmount = ResponsibilitilyWorkAmount;
                }
                catch
                {
                    tabBaseInfo.SelectedIndex = 1;
                    MessageBox.Show("责任工程量格式填写不正确！");
                    txtBudgetResponsibilityProjectAmount.Focus();
                    return false;
                }

                try
                {
                    decimal ResponsibilitilyPrice = 0;
                    if (txtBudgetResponsibilityPrice.Text.Trim() != "")
                        ResponsibilitilyPrice = Convert.ToDecimal(txtBudgetResponsibilityPrice.Text);

                    //oprDtl.ResponsibilitilyPrice = ResponsibilitilyPrice;
                }
                catch
                {
                    tabBaseInfo.SelectedIndex = 1;
                    MessageBox.Show("责任单价填写不正确！");
                    txtBudgetResponsibilityPrice.Focus();
                    return false;
                }

                try
                {
                    decimal ResponsibilitilyTotalPrice = 0;
                    if (txtBudgetResponsibilityTotalPrice.Text.Trim() != "")
                        ResponsibilitilyTotalPrice = Convert.ToDecimal(txtBudgetResponsibilityTotalPrice.Text);

                    oprDtl.ResponsibilitilyTotalPrice = ResponsibilitilyTotalPrice;
                }
                catch
                {
                    tabBaseInfo.SelectedIndex = 1;
                    MessageBox.Show("责任合价格式填写不正确！");
                    txtBudgetResponsibilityTotalPrice.Focus();
                    return false;
                }

                try
                {
                    decimal PlanWorkAmount = 0;
                    if (txtBudgetPlanProjectAmount.Text.Trim() != "")
                        PlanWorkAmount = Convert.ToDecimal(txtBudgetPlanProjectAmount.Text);

                    //oprDtl.PlanWorkAmount = PlanWorkAmount;
                }
                catch
                {
                    tabBaseInfo.SelectedIndex = 1;
                    MessageBox.Show("计划工程量格式填写不正确！");
                    txtBudgetPlanProjectAmount.Focus();
                    return false;
                }

                try
                {
                    decimal PlanPrice = 0;
                    if (txtBudgetPlanPrice.Text.Trim() != "")
                        PlanPrice = Convert.ToDecimal(txtBudgetPlanPrice.Text);

                    //oprDtl.PlanPrice = PlanPrice;
                }
                catch
                {
                    MessageBox.Show("计划单价格式填写不正确！");
                    tabBaseInfo.SelectedIndex = 1;
                    txtBudgetPlanPrice.Focus();
                    return false;
                }

                try
                {
                    decimal PlanTotalPrice = 0;
                    if (txtBudgetPlanTotalPrice.Text.Trim() != "")
                        PlanTotalPrice = Convert.ToDecimal(txtBudgetPlanTotalPrice.Text);

                    oprDtl.PlanTotalPrice = PlanTotalPrice;
                }
                catch
                {
                    MessageBox.Show("计划合价格式填写不正确！");
                    tabBaseInfo.SelectedIndex = 1;
                    txtBudgetPlanTotalPrice.Focus();
                    return false;
                }

                #endregion

                oprDtl.ResponseFlag = cbResponseAccount.Checked ? 1 : 0;
                oprDtl.CostingFlag = cbCostAccount.Checked ? 1 : 0;
                oprDtl.ProduceConfirmFlag = cbProductConfirm.Checked ? 1 : 0;
                oprDtl.SubContractFeeFlag = cbSubContractFee.Checked;

                #region 执行情况
                //try
                //{
                //    decimal AddupAccQuantity = 0;
                //    if (txtAddupAccProjectQuantity.Text.Trim() != "")
                //        AddupAccQuantity = Convert.ToDecimal(txtAddupAccProjectQuantity.Text);

                //    oprDtl.AddupAccQuantity = AddupAccQuantity;
                //}
                //catch
                //{
                //    tabBaseInfo.SelectedIndex = 2;
                //    MessageBox.Show("累计核算工程量格式填写不正确！");
                //    txtAddupAccProjectQuantity.Focus();
                //    return false;
                //}

                //try
                //{
                //    decimal AddupAccFigureProgress = 0;
                //    if (txtAddupAccFigureProgress.Text.Trim() != "")
                //        AddupAccFigureProgress = Convert.ToDecimal(txtAddupAccFigureProgress.Text);

                //    oprDtl.AddupAccFigureProgress = AddupAccFigureProgress;
                //}
                //catch
                //{
                //    tabBaseInfo.SelectedIndex = 2;
                //    MessageBox.Show("累计核算形象进度格式填写不正确！");
                //    txtAddupAccFigureProgress.Focus();
                //    return false;
                //}

                //try
                //{
                //    decimal QuantityConfirmed = 0;
                //    if (txtAddupConfirmProQuantity.Text.Trim() != "")
                //        QuantityConfirmed = Convert.ToDecimal(txtAddupConfirmProQuantity.Text);

                //    oprDtl.QuantityConfirmed = QuantityConfirmed;
                //}
                //catch
                //{
                //    tabBaseInfo.SelectedIndex = 2;
                //    MessageBox.Show("累计工长确认量格式填写不正确！");
                //    txtAddupConfirmProQuantity.Focus();
                //    return false;
                //}

                //try
                //{
                //    decimal ProgressConfirmed = 0;
                //    if (txtAddupConfirmFigureProgress.Text.Trim() != "")
                //        ProgressConfirmed = Convert.ToDecimal(txtAddupConfirmFigureProgress.Text);

                //    oprDtl.ProgressConfirmed = ProgressConfirmed;
                //}
                //catch
                //{
                //    tabBaseInfo.SelectedIndex = 2;
                //    MessageBox.Show("累计工长确认形象进度格式填写不正确！");
                //    txtAddupConfirmFigureProgress.Focus();
                //    return false;
                //}

                //oprDtl.WorkAmountUnitName=txtAddupConfirmProQnyUnit.Text;
                //oprDtl.WorkAmountUnitName=txtAddupAccProQuantityUnit.Text;
                //oprDtl.CostingFlag = 1;

                #endregion

                oprDtl.DetailExecuteDesc = txtExecuteDesc.Text;
                if (oprDtl.Id != null && oprDtl.State == DocumentState.InExecute)
                {
                    if (oprDtl.CostingFlag == 1)
                    {
                        #region 成本核算明细
                        IList list = new List<GWBSDetail>();
                        ObjectQuery oq = new ObjectQuery();
                        Disjunction dis = new Disjunction();
                        oq.AddCriterion(Expression.Eq("CostingFlag", 1));
                        //oq.AddCriterion(Expression.Not(Expression.Eq("State", DocumentState.Invalid)));
                        oq.AddCriterion(Expression.Eq("State", DocumentState.InExecute));
                        oq.AddCriterion(Expression.Eq("TheCostItem.Id", oprDtl.TheCostItem.Id));
                        if (!string.IsNullOrEmpty(oprDtl.MainResourceTypeId))
                        {
                            oq.AddCriterion(Expression.Eq("MainResourceTypeId", oprDtl.MainResourceTypeId));
                        }
                        else
                        {
                            oq.AddCriterion(Expression.IsNull("MainResourceTypeId"));
                        }

                        if (string.IsNullOrEmpty(oprDtl.DiagramNumber))
                        {
                            oq.AddCriterion(Expression.IsNull("DiagramNumber"));
                        }
                        else
                        {
                            oq.AddCriterion(Expression.Eq("DiagramNumber", oprDtl.DiagramNumber));
                        }

                        dis.Add(Expression.Like("TheGWBSSysCode", oprNode.SysCode, MatchMode.Start));
                        string[] sysCodes = oprNode.SysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < sysCodes.Length - 1; i++)
                        {
                            string sysCode = "";
                            for (int j = 0; j <= i; j++)
                            {
                                sysCode += sysCodes[j] + ".";
                            }
                            dis.Add(Expression.Eq("TheGWBSSysCode", sysCode));
                        }
                        oq.AddCriterion(dis);

                        if (string.IsNullOrEmpty(oprDtl.Id) == false)
                        {
                            oq.AddCriterion(Expression.Not(Expression.Eq("Id", oprDtl.Id)));
                        }

                        oq.AddFetchMode("TheGWBS", NHibernate.FetchMode.Eager);
                        list = model.ObjectQuery(typeof(GWBSDetail), oq);

                        if (list.Count > 0)
                        {
                            GWBSDetail detail = list[0] as GWBSDetail;

                            string str = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), detail.TheGWBS.Name, detail.TheGWBSSysCode);
                            MessageBox.Show(str + " 节点下存在相同成本项、主资源类型、图号的“成本核算明细”，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        #endregion
                    }

                    if (oprDtl.ProduceConfirmFlag == 1)
                    {
                        #region 生产确认明细验重
                        if (string.IsNullOrEmpty(oprDtl.MainResourceTypeId))
                            oprDtl.MainResourceTypeId = null;
                        if (string.IsNullOrEmpty(oprDtl.DiagramNumber))
                            oprDtl.DiagramNumber = null;

                        foreach (DataGridViewRow row in gridGWBDetail.Rows)
                        {
                            GWBSDetail d = row.Tag as GWBSDetail;
                            if (d.Id != oprDtl.Id && d.ProduceConfirmFlag == 1 && d.State == DocumentState.InExecute
                                && oprDtl.TheCostItem.Id == d.TheCostItem.Id && oprDtl.MainResourceTypeId == d.MainResourceTypeId && oprDtl.DiagramNumber == d.DiagramNumber)
                            {
                                MessageBox.Show("当前节点下已存在与任务明细“" + oprDtl.Name + "”的成本项、主资源类型、图号相同的“生产确认明细”，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                        }
                        #endregion
                    }
                }
                if (oprDtl.State == DocumentState.InExecute)
                {
                    return valideChangeContractGroup();
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
        /// 验证重复性【此处只做了当前节点下的重复验证,发布时如果是成本核算明细时，会做同一路径重复验证】
        /// </summary>
        /// <returns>验证通过时返回true，不通过【存在重复项】时返回false</returns>
        private bool VaildateRepeate()
        {
            oprDtl.MainResourceTypeId += "";
            oprDtl.DiagramNumber += "";
            foreach (DataGridViewRow row in gridGWBDetail.Rows)
            {
                GWBSDetail d = row.Tag as GWBSDetail;
                if (d.Id != oprDtl.Id &&
                    d.State != DocumentState.Invalid
                    && oprDtl.TheCostItem.Id == d.TheCostItem.Id
                    && oprDtl.MainResourceTypeId == (d.MainResourceTypeId + "")
                    && oprDtl.DiagramNumber == (d.DiagramNumber + ""))
                {
                    //MessageBox.Show("当前节点下已存在与任务明细“" + oprDtl.Name + "”的成本项、主资源类型、图号相同的“生产确认明细”，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }

        public void Start()
        {
            RefreshState(MainViewState.Browser);

            //LoadGWBSTreeTree();
            //LoadGWBSTree(null);
            LoadGWBSTree();
        }

        private void LoadGWBSTreeTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();

                IList list = model.GetGWBSTreesByInstance(projectInfo.Id);
                //lstInstance = listAll[1] as IList;
                //IList list = listAll[0] as IList;
                foreach (GWBSTree childNode in list)
                {
                    //if (childNode.State == 0)
                    //    continue;

                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = childNode.Name;
                    tnTmp.Tag = childNode;
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
                }
                if (list.Count > 0)
                {
                    this.tvwCategory.SelectedNode = this.tvwCategory.Nodes[0];
                    this.tvwCategory.SelectedNode.Expand();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        //刷新任务主表相关控件的状态
        public override void RefreshControls(MainViewState state)
        {
            switch (state)
            {
                case MainViewState.Modify:

                    break;

                case MainViewState.Browser:

                    //基本信息
                    this.txtCurrentPath.ReadOnly = true;
                    this.txtTaskCode.ReadOnly = true;
                    this.txtTaskName.ReadOnly = true;
                    this.txtTaskState.ReadOnly = true;
                    txtTaskStateTime.ReadOnly = true;
                    txtTaskOwner.ReadOnly = true;
                    this.txtTaskWBSType.ReadOnly = true;

                    this.txtTaskStartTime.ReadOnly = true;
                    this.txtTaskEndTime.ReadOnly = true;

                    txtTaskDesc.ReadOnly = true;
                    txtContractTotalPrice.ReadOnly = true;
                    txtResponsibilityTotalPrice.ReadOnly = true;
                    txtPlanTotalPrice.ReadOnly = true;
                    txtPriceUnit.ReadOnly = true;
                    txtFigureProgress.ReadOnly = true;
                    txtCheckBatchNum.ReadOnly = true;

                    cbListCheckRequire.Enabled = false;


                    RefreshDetailControls(MainViewState.Browser);
                    break;
            }
        }

        //刷新任务明细相关控件的状态
        public void RefreshDetailControls(MainViewState state)
        {
            switch (state)
            {

                case MainViewState.Modify:

                    btnSaveBaseInfo.Enabled = true;
                    btnSaveTaskDetail.Enabled = true;
                    //不需要修改
                    //txtDtlCode.ReadOnly = true;
                    txtDtlState.ReadOnly = true;
                    txtDtlStateTime.ReadOnly = true;

                    txtDtlContractGroupName.ReadOnly = true;
                    txtDtlContractGroupType.ReadOnly = true;


                    //基本信息
                    txtDtlName.ReadOnly = false;

                    txtDtlCostItem.ReadOnly = false;
                    btnSelectCostItem.Enabled = true;

                    txtMainResourceType.ReadOnly = false;//主资源类型
                    btnSelectMainResourceType.Enabled = true;

                    txtDrawNumber.ReadOnly = false;

                    txtDtlDesc.ReadOnly = false;

                    //txtBudgetProjectUnit.ReadOnly = false;
                    //txtBudgetPriceUnit.ReadOnly = false;


                    cbResponseAccount.Enabled = true;
                    cbCostAccount.Enabled = true;
                    cbProductConfirm.Enabled = true;
                    cbSubContractFee.Enabled = false;

                    cbUpdateContract.Enabled = true;

                    //预算信息
                    //txtBudgetProjectUnit.ReadOnly = false;
                    //txtBudgetPriceUnit.ReadOnly = false;

                    btnSelProjectQnyUnit.Enabled = true;
                    btnSelPriceUnit.Enabled = true;

                    txtBudgetContractProjectAmount.ReadOnly = false;
                    txtBudgetContractPrice.ReadOnly = false;
                    //txtBudgetContractTotalPrice.ReadOnly = false;

                    txtBudgetResponsibilityProjectAmount.ReadOnly = false;
                    txtBudgetResponsibilityPrice.ReadOnly = false;
                    //txtBudgetResponsibilityTotalPrice.ReadOnly = false;

                    txtBudgetPlanProjectAmount.ReadOnly = false;
                    txtBudgetPlanPrice.ReadOnly = false;
                    //txtBudgetPlanTotalPrice.ReadOnly = false;

                    btnAddCostSubject.Enabled = true;
                    btnUpdateCostSubject.Enabled = true;
                    btnDeleteCostSubject.Enabled = true;

                    //btnContractUsage.Enabled = true;
                    //btnResponsibleUsage.Enabled = true;
                    //btnPlanUsage.Enabled = true;

                    btnAccountCostData.Enabled = true;


                    //执行情况
                    txtAddupAccProjectQuantity.ReadOnly = true;
                    txtAddupAccProQuantityUnit.ReadOnly = true;
                    txtAddupAccFigureProgress.ReadOnly = true;

                    txtAddupConfirmProQuantity.ReadOnly = true;
                    txtAddupConfirmProQnyUnit.ReadOnly = true;
                    txtAddupConfirmFigureProgress.ReadOnly = true;

                    txtExecuteDesc.ReadOnly = true;

                    break;

                case MainViewState.Browser:

                    btnSaveBaseInfo.Enabled = false;
                    btnSaveTaskDetail.Enabled = false;

                    //基本信息
                    txtDtlName.ReadOnly = true;
                    //txtDtlCode.ReadOnly = true;
                    txtDtlCostItem.ReadOnly = true;
                    btnSelectCostItem.Enabled = false;

                    txtMainResourceType.ReadOnly = true;
                    btnSelectMainResourceType.Enabled = false;

                    txtDrawNumber.ReadOnly = true;

                    txtDtlDesc.ReadOnly = true;

                    txtDtlState.ReadOnly = true;
                    txtDtlStateTime.ReadOnly = true;

                    txtDtlContractGroupName.ReadOnly = true;
                    txtDtlContractGroupType.ReadOnly = true;

                    //txtBudgetProjectUnit.ReadOnly = true;
                    //txtBudgetPriceUnit.ReadOnly = true;

                    cbResponseAccount.Enabled = false;
                    cbCostAccount.Enabled = false;
                    cbProductConfirm.Enabled = false;
                    cbSubContractFee.Enabled = false;

                    cbUpdateContract.Enabled = false;

                    //预算信息
                    //txtBudgetProjectUnit.ReadOnly = true;
                    //txtBudgetPriceUnit.ReadOnly = true;

                    btnSelProjectQnyUnit.Enabled = false;
                    btnSelPriceUnit.Enabled = false;


                    txtBudgetContractProjectAmount.ReadOnly = true;
                    txtBudgetContractPrice.ReadOnly = true;
                    txtBudgetContractTotalPrice.ReadOnly = true;

                    txtBudgetResponsibilityProjectAmount.ReadOnly = true;
                    txtBudgetResponsibilityPrice.ReadOnly = true;
                    txtBudgetResponsibilityTotalPrice.ReadOnly = true;

                    txtBudgetPlanProjectAmount.ReadOnly = true;
                    txtBudgetPlanPrice.ReadOnly = true;
                    txtBudgetPlanTotalPrice.ReadOnly = true;


                    //btnContractUsage.Enabled = false;
                    //btnResponsibleUsage.Enabled = false;
                    //btnPlanUsage.Enabled = false;

                    btnAccountCostData.Enabled = false;

                    //执行情况
                    txtAddupAccProjectQuantity.ReadOnly = true;
                    txtAddupAccProQuantityUnit.ReadOnly = true;
                    txtAddupAccFigureProgress.ReadOnly = true;

                    txtAddupConfirmProQuantity.ReadOnly = true;
                    txtAddupConfirmProQnyUnit.ReadOnly = true;
                    txtAddupConfirmFigureProgress.ReadOnly = true;

                    txtExecuteDesc.ReadOnly = true;


                    if (oprDtl != null && oprDtl.State == VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Invalid)
                    {
                        btnAddCostSubject.Enabled = false;
                        btnUpdateCostSubject.Enabled = false;
                        btnDeleteCostSubject.Enabled = false;
                    }
                    else
                    {
                        btnAddCostSubject.Enabled = true;
                        btnUpdateCostSubject.Enabled = true;
                        btnDeleteCostSubject.Enabled = true;
                    }

                    break;
            }
            ViewState = state;
        }

        public override bool ModifyView()
        {
            return true;
        }

        public override bool CancelView()
        {
            try
            {
                //LoadGWBSTreeTree();
                //LoadGWBSTree(null);
                LoadGWBSTree();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        public override void RefreshView()
        {
            try
            {
                //LoadGWBSTreeTree();
                //LoadGWBSTree(null);
                LoadGWBSTree();
            }
            catch (Exception e)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(e));
            }
        }

        public override bool SaveView()
        {
            try
            {

                if (!ValideSave())
                    return false;

                List<GWBSDetailLedger> listLedger = null;
                if (oprDtl.State == DocumentState.InExecute)
                {
                    listLedger = new List<GWBSDetailLedger>();
                    GWBSDetail tempDtl = model.GetObjectById(typeof(GWBSDetail), oprDtl.Id) as GWBSDetail;
                    #region 记录变更台帐

                    decimal contractQuantity = ClientUtil.ToDecimal(txtBudgetContractProjectAmount.Text);
                    decimal responsibleQuantity = ClientUtil.ToDecimal(txtBudgetResponsibilityProjectAmount.Text);
                    decimal planQuantity = ClientUtil.ToDecimal(txtBudgetPlanProjectAmount.Text);

                    decimal contractPrice = ClientUtil.ToDecimal(txtBudgetContractPrice.Text);
                    decimal responsiblePrice = ClientUtil.ToDecimal(txtBudgetResponsibilityPrice.Text);
                    decimal planPrice = ClientUtil.ToDecimal(txtBudgetPlanPrice.Text);

                    if (tempDtl.ContractProjectQuantity != contractQuantity || tempDtl.ResponsibilitilyWorkAmount != responsibleQuantity || tempDtl.PlanWorkAmount != planQuantity)
                    {
                        GWBSDetailLedger led = new GWBSDetailLedger();

                        led.ProjectTaskID = tempDtl.TheGWBS.Id;
                        led.ProjectTaskName = tempDtl.TheGWBS.Name;
                        led.TheProjectTaskSysCode = tempDtl.TheGWBSSysCode;

                        led.ProjectTaskDtlID = tempDtl.Id;
                        led.ProjectTaskDtlName = tempDtl.Name;

                        if (tempDtl.ContractProjectQuantity != contractQuantity)
                        {
                            led.ContractChangeMode = ContractIncomeChangeModeEnum.合同收入工程量变化;
                            led.ContractWorkAmount = contractQuantity - tempDtl.ContractProjectQuantity;
                            led.ContractPrice = tempDtl.ContractPrice;
                            led.ContractTotalPrice = led.ContractWorkAmount * led.ContractPrice;
                        }
                        else
                        {
                            led.ContractChangeMode = ContractIncomeChangeModeEnum.合同收入无变化;
                            led.ContractWorkAmount = 0;
                            led.ContractPrice = 0;
                            led.ContractTotalPrice = 0;
                        }

                        if (tempDtl.ResponsibilitilyWorkAmount != responsibleQuantity)
                        {
                            led.ResponsibleCostChangeMode = ResponsibleCostChangeModeEnum.责任工程量变化;
                            led.ResponsibleWorkAmount = responsibleQuantity - tempDtl.ResponsibilitilyWorkAmount;
                            led.ResponsiblePrice = tempDtl.ResponsibilitilyPrice;
                            led.ResponsibleTotalPrice = led.ResponsibleWorkAmount * led.ResponsiblePrice;
                        }
                        else
                        {
                            led.ResponsibleCostChangeMode = ResponsibleCostChangeModeEnum.责任成本无变化;
                            led.ResponsiblePrice = 0;
                            led.ResponsibleWorkAmount = 0;
                            led.ResponsibleTotalPrice = 0;
                        }


                        if (tempDtl.PlanWorkAmount != planQuantity)
                        {
                            led.PlanCostChangeMode = PlanCostChangeModeEnum.计划工程量变化;
                            led.PlanWorkAmount = planQuantity - tempDtl.PlanWorkAmount;
                            led.PlanPrice = tempDtl.PlanPrice;
                            led.PlanTotalPrice = led.PlanWorkAmount * led.PlanPrice;
                        }
                        else
                        {
                            led.PlanCostChangeMode = PlanCostChangeModeEnum.计划成本无变化;
                            led.PlanWorkAmount = 0;
                            led.PlanPrice = 0;
                            led.PlanTotalPrice = 0;
                        }

                        led.WorkAmountUnit = tempDtl.WorkAmountUnitGUID;
                        led.WorkAmountUnitName = tempDtl.WorkAmountUnitName;
                        led.PriceUnit = tempDtl.PriceUnitGUID;
                        led.PriceUnitName = tempDtl.PriceUnitName;
                        led.TheContractGroup = txtChangeContractName.Tag as ContractGroup;
                        led.TheProjectGUID = tempDtl.TheProjectGUID;
                        led.TheProjectName = tempDtl.TheProjectName;

                        listLedger.Add(led);
                    }

                    tempDtl.ContractProjectQuantity = contractQuantity;
                    tempDtl.ResponsibilitilyWorkAmount = responsibleQuantity;
                    tempDtl.PlanWorkAmount = planQuantity;

                    if (tempDtl.ContractPrice != contractPrice ||
                         tempDtl.ResponsibilitilyPrice != responsiblePrice ||
                         tempDtl.PlanPrice != planPrice)
                    {
                        GWBSDetailLedger led = new GWBSDetailLedger();

                        led.ProjectTaskID = tempDtl.TheGWBS.Id;
                        led.ProjectTaskName = tempDtl.TheGWBS.Name;
                        led.TheProjectTaskSysCode = tempDtl.TheGWBSSysCode;

                        led.ProjectTaskDtlID = tempDtl.Id;
                        led.ProjectTaskDtlName = tempDtl.Name;

                        if (tempDtl.ContractPrice != contractPrice)
                        {
                            led.ContractChangeMode = ContractIncomeChangeModeEnum.合同单价变化;
                            led.ContractPrice = contractPrice - tempDtl.ContractPrice;
                            led.ContractWorkAmount = tempDtl.ContractProjectQuantity;
                            led.ContractTotalPrice = led.ContractWorkAmount * led.ContractPrice;
                        }
                        else
                        {
                            led.ContractChangeMode = ContractIncomeChangeModeEnum.合同收入无变化;
                            led.ContractWorkAmount = 0;
                            led.ContractPrice = 0;
                            led.ContractTotalPrice = 0;
                        }

                        if (tempDtl.ResponsibilitilyPrice != responsiblePrice)
                        {
                            led.ResponsibleCostChangeMode = ResponsibleCostChangeModeEnum.责任单价变化;
                            led.ResponsiblePrice = responsiblePrice - tempDtl.ResponsibilitilyPrice;
                            led.ResponsibleWorkAmount = tempDtl.ResponsibilitilyWorkAmount;
                            led.ResponsibleTotalPrice = led.ResponsibleWorkAmount * led.ResponsiblePrice;
                        }
                        else
                        {
                            led.ResponsibleCostChangeMode = ResponsibleCostChangeModeEnum.责任成本无变化;
                            led.ResponsiblePrice = 0;
                            led.ResponsibleWorkAmount = 0;
                            led.ResponsibleTotalPrice = 0;
                        }

                        if (tempDtl.PlanPrice != planPrice)
                        {
                            led.PlanCostChangeMode = PlanCostChangeModeEnum.计划单价变化;
                            led.PlanPrice = planPrice - tempDtl.PlanPrice;
                            led.PlanWorkAmount = tempDtl.PlanWorkAmount;
                            led.PlanTotalPrice = led.PlanWorkAmount * led.PlanPrice;
                        }
                        else
                        {
                            led.PlanCostChangeMode = PlanCostChangeModeEnum.计划成本无变化;
                            led.PlanWorkAmount = 0;
                            led.PlanPrice = 0;
                            led.PlanTotalPrice = 0;
                        }
                        led.WorkAmountUnit = tempDtl.WorkAmountUnitGUID;
                        led.WorkAmountUnitName = tempDtl.WorkAmountUnitName;
                        led.PriceUnit = tempDtl.PriceUnitGUID;
                        led.PriceUnitName = tempDtl.PriceUnitName;
                        led.TheContractGroup = txtChangeContractName.Tag as ContractGroup;
                        led.TheProjectGUID = tempDtl.TheProjectGUID;
                        led.TheProjectName = tempDtl.TheProjectName;

                        listLedger.Add(led);
                    }
                    #endregion
                }
                btnAccountCostData_Click(btnAccountCostData, new EventArgs());

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", oprNode.Id));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("ProjectTaskTypeGUID", NHibernate.FetchMode.Eager);
                oprNode = model.ObjectQuery(typeof(GWBSTree), oq)[0] as GWBSTree;

                if (!string.IsNullOrEmpty(oprDtl.Id))
                {
                    for (int i = 0; i < oprNode.Details.Count; i++)
                    {
                        GWBSDetail dtl = oprNode.Details.ElementAt(i);
                        if (dtl.Id == oprDtl.Id)
                        {
                            oprNode.Details.Remove(dtl);
                            oprNode.Details.Add(oprDtl);
                            //dtl = oprDtl;
                            break;
                        }
                    }
                }
                else
                {
                    oprDtl.TheGWBS = oprNode;
                    oprNode.Details.Add(oprDtl);
                }

                //根据明细设置任务对象的标记
                bool taskResponsibleFlag = false;
                bool taskCostAccFlag = false;
                bool taskProductConfirmFlag = false;
                bool taskSubContractFeeFlag = false;

                foreach (GWBSDetail dtl in oprNode.Details)
                {
                    if (dtl.ResponseFlag == 1)
                        taskResponsibleFlag = true;
                    if (dtl.CostingFlag == 1)
                        taskCostAccFlag = true;
                    if (dtl.ProduceConfirmFlag == 1)
                        taskProductConfirmFlag = true;
                    if (dtl.SubContractFeeFlag)
                        taskSubContractFeeFlag = true;
                }

                oprNode.ResponsibleAccFlag = taskResponsibleFlag;
                oprNode.CostAccFlag = taskCostAccFlag;
                oprNode.ProductConfirmFlag = taskProductConfirmFlag;
                oprNode.SubContractFeeFlag = taskSubContractFeeFlag;

                #region 计算合价

                GWBSTree tempNode = new GWBSTree();
                tempNode.Id = oprNode.Id;
                tempNode.SysCode = oprNode.SysCode;
                tempNode = model.AccountTotalPriceByChilds(tempNode);

                List<Decimal> listTotalPrice = StaticMethod.GetTaskDtlTotalPrice(oprNode);
                //
                oprNode.ContractTotalPrice = tempNode.ContractTotalPrice + listTotalPrice[0];
                oprNode.ResponsibilityTotalPrice = tempNode.ResponsibilityTotalPrice + listTotalPrice[1];
                oprNode.PlanTotalPrice = tempNode.PlanTotalPrice + listTotalPrice[2];

                #endregion

                oprDtl.TheGWBSSysCode = oprNode.SysCode;
                //oprDtl.TheGWBSFullPath = oprNode.FullPath;

                //更新累计核算形象进度和累计工长确认形象进度 2014-09-17
                if (oprDtl.PlanWorkAmount != 0)
                {
                    oprDtl.AddupAccFigureProgress = decimal.Round(oprDtl.AddupAccQuantity * 100 / oprDtl.PlanWorkAmount, 2);
                    oprDtl.ProgressConfirmed = decimal.Round(oprDtl.QuantityConfirmed * 100 / oprDtl.PlanWorkAmount, 2);
                }

                if (oprDtl.Id == null)
                {
                    if (!IsAddDetail)//插入明细需更新部分排序号
                    {
                        for (int i = 0; i < oprNode.Details.Count; i++)
                        {
                            GWBSDetail dtl = oprNode.Details.ElementAt(i);
                            if (dtl.OrderNo >= oprDtl.OrderNo && !string.IsNullOrEmpty(dtl.Id))
                            {
                                dtl.OrderNo += 1;
                            }
                        }
                    }

                    oprNode = model.SaveOrUpdateGWBSTree(oprNode, listLedger);
                    //oprNode = LoadRelaPBS(oprNode);
                    tvwCategory.SelectedNode.Tag = oprNode;

                    oprDtl = oprNode.Details.ElementAt(oprNode.Details.Count - 1);

                    if (IsAddDetail)
                        AddDetailInfoInGrid(oprDtl, true);
                    else
                    {
                        InsertDetailInfoInGrid(oprDtl, gridGWBDetail.SelectedRows[0].Index);

                        //更新排序号到表格
                        oq.Criterions.Clear();
                        oq.FetchModes.Clear();
                        oq.Orders.Clear();
                        Disjunction dis = new Disjunction();
                        foreach (GWBSDetail dtl in oprNode.Details)
                        {
                            if (dtl.OrderNo > oprDtl.OrderNo)
                            {
                                dis.Add(Expression.Eq("Id", dtl.Id));
                            }
                        }
                        oq.AddCriterion(dis);
                        oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
                        oq.AddFetchMode("ListCostSubjectDetails", NHibernate.FetchMode.Eager);
                        oq.AddFetchMode("WorkAmountUnitGUID", NHibernate.FetchMode.Eager);
                        oq.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);

                        IList listUpdateDtl = model.ObjectQuery(typeof(GWBSDetail), oq);
                        foreach (GWBSDetail dtl in listUpdateDtl)
                        {
                            UpdateDetailInfoInGrid(dtl, false);
                        }
                    }
                }
                else
                {
                    //if (oprDtl.State == DocumentState.InExecute && (oprDtl.TheCostItem.Id != oprTheCostItemId || oprDtl.MainResourceTypeId != oprMainResourceTypeId || oprDtl.DiagramNumber != oprDiagramNumber))
                    //{
                    //    oprNode = model.SaveOrUpdateGWBSTree(oprNode, listLedger, oprDtl, oprTheCostItemId, oprMainResourceTypeId, oprDiagramNumber);
                    //}
                    if (oprDtl.State == DocumentState.InExecute && (oprDtl.TheCostItem.Id != updateBeforeDtl.TheCostItem.Id || TransUtil.ToString(oprDtl.MainResourceTypeId) != TransUtil.ToString(updateBeforeDtl.MainResourceTypeId) || TransUtil.ToString(oprDtl.DiagramNumber) != TransUtil.ToString(updateBeforeDtl.DiagramNumber)))
                    {
                        oprNode = model.SaveOrUpdateGWBSTree(oprNode, listLedger, oprDtl, updateBeforeDtl);
                        updateBeforeDtl = null;
                    }
                    else
                    {
                        oprNode = model.SaveOrUpdateGWBSTree(oprNode, listLedger);
                    }

                    foreach (GWBSDetail dtl in oprNode.Details)
                    {
                        if (dtl.Id == oprDtl.Id)
                        {
                            oprDtl = dtl;
                            break;
                        }
                    }

                    //oprNode = LoadRelaPBS(oprNode);

                    tvwCategory.SelectedNode.Tag = oprNode;

                    //oprDtl = LoadRelaObjByTaskDetail(oprDtl);

                    UpdateDetailInfoInGrid(oprDtl, true);

                    //if (optContract != null)
                    //{
                    //    txtContractGroupName.Tag = optContract;
                    //}

                }

                //更新显示任务节点的合同合价、责任合价、计划合价信息
                txtContractTotalPrice.Text = oprNode.ContractTotalPrice.ToString();
                txtResponsibilityTotalPrice.Text = oprNode.ResponsibilityTotalPrice.ToString();
                txtPlanTotalPrice.Text = oprNode.PlanTotalPrice.ToString();
                //执行情况
                txtAddupAccProjectQuantity.Text = oprDtl.AddupAccQuantity.ToString();
                txtAddupAccProQuantityUnit.Text = oprDtl.WorkAmountUnitName;
                txtAddupAccFigureProgress.Text = oprDtl.AddupAccFigureProgress.ToString();

                txtAddupConfirmProQuantity.Text = oprDtl.QuantityConfirmed.ToString();
                txtAddupConfirmProQnyUnit.Text = oprDtl.WorkAmountUnitName;
                txtAddupConfirmFigureProgress.Text = oprDtl.ProgressConfirmed.ToString();

                return true;

            }
            catch (Exception exp)
            {
                if (exp.InnerException != null && exp.InnerException.Message.Contains("违反唯一约束条件"))
                    MessageBox.Show("编码必须唯一！");
                else
                    MessageBox.Show("保存失败，异常信息：" + ExceptionUtil.ExceptionMessage(exp));


                //if (!string.IsNullOrEmpty(oprDtl.Id) && optContract != null)
                //{
                //    txtContractGroupName.Tag = optContract;
                //}
            }
            return false;
        }
        #endregion

        #region 任务明细操作
        private void ClearDetailAll()
        {
            //基本信息
            txtDtlName.Text = "";
            //txtDtlCode.Text = "";

            txtDtlCostItem.Text = "";
            txtDtlCostItem.Tag = null;

            txtMainResourceType.Text = "";
            txtMainResourceType.Tag = null;
            txtDrawNumber.Text = "";

            txtDtlDesc.Text = "";

            txtDtlState.Text = "";
            txtDtlStateTime.Text = "";

            txtDtlContractGroupName.Text = "";
            txtDtlContractGroupName.Tag = null;
            txtDtlContractGroupType.Text = "";


            cbResponseAccount.Checked = false;
            cbCostAccount.Checked = false;
            cbProductConfirm.Checked = false;
            cbSubContractFee.Checked = false;
            txtSubcontractFee.Text = "";

            cbUpdateContract.Checked = false;

            //预算
            txtBudgetProjectUnit.Text = "";
            txtBudgetProjectUnit.Tag = null;
            txtBudgetPriceUnit.Text = "";
            txtBudgetPriceUnit.Tag = null;

            txtBudgetContractProjectAmount.Text = "";
            txtBudgetContractPrice.Text = "";
            txtBudgetContractTotalPrice.Text = "";

            txtBudgetResponsibilityProjectAmount.Text = "";
            txtBudgetResponsibilityPrice.Text = "";
            txtBudgetResponsibilityTotalPrice.Text = "";

            txtBudgetPlanProjectAmount.Text = "";
            txtBudgetPlanPrice.Text = "";
            txtBudgetPlanTotalPrice.Text = "";

            txtChangeContractName.Text = "";
            txtChangeContractName.Tag = null;
            txtChangeContractType.Text = "";

            //执行情况
            txtAddupAccProjectQuantity.Text = "";
            txtAddupAccProQuantityUnit.Text = "";
            txtAddupAccFigureProgress.Text = "";

            txtAddupConfirmProQuantity.Text = "";
            txtAddupConfirmProQnyUnit.Text = "";
            txtAddupConfirmFigureProgress.Text = "";

            txtExecuteDesc.Text = "";

            gridCostSubject.Rows.Clear();
        }

        void gridGWBDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                GWBSDetail tempItem = gridGWBDetail.Rows[e.RowIndex].Tag as GWBSDetail;
                if (oprDtl != null && tempItem.Id == oprDtl.Id)
                    return;

                if (btnSaveBaseInfo.Enabled && oprDtl != null && tempItem.Id != oprDtl.Id)
                {
                    if (MessageBox.Show("有尚未保存的任务明细信息，要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (!SaveView())
                        {
                            //修改时还原选中的修改行
                            foreach (DataGridViewRow row in gridGWBDetail.Rows)
                            {
                                GWBSDetail temp = row.Tag as GWBSDetail;
                                if (temp.Id == oprDtl.Id)
                                {
                                    gridGWBDetail.CurrentCell = row.Cells[0];
                                    break;
                                }
                            }
                            return;
                        }
                    }
                }


                oprDtl = tempItem;

                if (oprDtl != null)
                {
                    ShownTaskDetailInfo(true);
                    gridGWBDetail.Rows[e.RowIndex].Tag = oprDtl;
                }
                gridGWBDetail.CurrentCell = gridGWBDetail.Rows[e.RowIndex].Cells[0];

                RefreshDetailControls(MainViewState.Browser);


                #region 如果该明细是通过分摊下来的禁用按钮
                //如果分摊关系的目标节点中存在当前节点下的当前明细时，说明该节点明细是分摊下来的，则该明细不允许任何操作
                if (lstSelectedGWBSRate.Any(p => p.TargetGWBSTreeID == oprNode.Id && oprDtl.Id == p.TargetGWBSDetailID))
                {
                    btnUpdateDetail.Enabled = false;
                    btnDeleteDetail.Enabled = false;
                    btnBatchAddDtl.Enabled = false;
                    btnAddDetail.Enabled = false;
                    btnInsertDetail.Enabled = false;
                    btnPublishDetail.Enabled = false;
                    btnChangeTaskDetail.Enabled = false;
                    btnSaveTaskDetail.Enabled = false;
                    btnChange.Enabled = false;
                    btnCancellationDetail.Enabled = false;
                }
                else
                {
                    btnUpdateDetail.Enabled = true;
                    btnDeleteDetail.Enabled = true;
                    btnBatchAddDtl.Enabled = true;
                    btnAddDetail.Enabled = true;
                    btnInsertDetail.Enabled = true;
                    btnPublishDetail.Enabled = true;
                    btnChangeTaskDetail.Enabled = true;
                    btnSaveTaskDetail.Enabled = true;
                    btnChange.Enabled = true;
                    btnCancellationDetail.Enabled = true;
                }
                #endregion
            }
        }

        void txtBudgetProjectUnit_TextChanged(object sender, EventArgs e)
        {
            txtAddupAccProQuantityUnit.Text = txtBudgetProjectUnit.Text;
            txtAddupConfirmProQnyUnit.Text = txtBudgetProjectUnit.Text;
        }

        void txtBudgetPriceUnit_TextChanged(object sender, EventArgs e)
        {

        }

        //新增明细
        void btnAddDetail_Click(object sender, EventArgs e)
        {
            IsAddDetail = true;
            IsCostItemSingleSelect = false;//新增时，可多选成本项
            if (oprNode == null)
            {
                MessageBox.Show("请先选择要添加明细的工程WBS节点！");
                return;
            }
            else if (!valideContractGroup())
            {
                return;
            }

            //在节点选择的时候有对象关联查询，但偶尔会有丢失延迟加载的对象，根据需要重新加载
            try
            {
                if (oprNode.ProjectTaskTypeGUID != null)
                {
                    string name = oprNode.ProjectTaskTypeGUID.Name;
                }
            }
            catch
            {
                oprNode = LoadRelaPBS(oprNode);
            }

            ClearDetailAll();
            RefreshDetailControls(MainViewState.Modify);

            AddAndSetNewDetailInfo();

            tabBaseInfo.SelectedIndex = 0;
            txtDtlCostItem.Focus();
        }
        //批量添加明细
        void btnBatchAddDtl_Click(object sender, EventArgs e)
        {
            if (oprNode == null)
            {
                MessageBox.Show("请先选择要添加明细的工程WBS节点！");
                return;
            }
            else if (!valideContractGroup())
            {
                return;
            }

            //在节点选择的时候有对象关联查询，但偶尔会有丢失延迟加载的对象，根据需要重新加载
            try
            {
                if (oprNode.ProjectTaskTypeGUID != null)
                {
                    string name = oprNode.ProjectTaskTypeGUID.Name;
                }
            }
            catch
            {
                oprNode = LoadRelaPBS(oprNode);
            }

            #region 选择成本项 批量
            string syscode = string.Empty;
            if (txtCostItemsZoning.Tag != null)
            {
                CostItemsZoning z = txtCostItemsZoning.Tag as CostItemsZoning;
                syscode = z.CostItemsCateSyscode;
            }
            VSelectCostItem frm = new VSelectCostItem(new MCostItem(), syscode, false);
            frm.ShowDialog();
            IList resultList = frm.SelectCostItemList;
            #endregion
            if (resultList != null && resultList.Count > 0)
            {
                #region 新建明细并赋值
                IList newDetailList = new ArrayList();
                ObjectQuery oqQuota = new ObjectQuery();
                Disjunction dis = new Disjunction();
                foreach (CostItem item in resultList)
                {
                    dis.Add(Expression.Eq("TheCostItem.Id", item.Id));
                }
                oqQuota.AddCriterion(dis);
                oqQuota.AddCriterion(Expression.Eq("State", SubjectCostQuotaState.生效));
                oqQuota.AddFetchMode("CostAccountSubjectGUID", NHibernate.FetchMode.Eager);
                oqQuota.AddFetchMode("ProjectAmountUnitGUID", NHibernate.FetchMode.Eager);
                oqQuota.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
                oqQuota.AddFetchMode("ListResources", NHibernate.FetchMode.Eager);
                IList listQuota = model.ObjectQuery(typeof(SubjectCostQuota), oqQuota);

                int maxOrderNo = 1;
                int oldMaxOrderNo = 0;
                int code = gridGWBDetail.Rows.Count + 1;
                #region 获取父对象下最大明细号
                for (int i = 0; i < gridGWBDetail.Rows.Count; i++)
                {
                    GWBSDetail dtl = gridGWBDetail.Rows[i].Tag as GWBSDetail;

                    if (dtl != null)
                    {
                        if (dtl.OrderNo > maxOrderNo)
                            maxOrderNo = dtl.OrderNo;

                        if (!string.IsNullOrEmpty(dtl.Code))
                        {
                            try
                            {
                                if (dtl.Code.IndexOf("-") > -1)
                                {
                                    int tempCode = Convert.ToInt32(dtl.Code.Substring(dtl.Code.LastIndexOf("-") + 1));
                                    if (tempCode >= code)
                                        code = tempCode + 1;
                                }
                            }
                            catch
                            {

                            }
                        }
                    }
                }
                oldMaxOrderNo = maxOrderNo;
                #endregion
                foreach (CostItem item in resultList)
                {
                    newDetailList.Add(NewGWBSDetail(item, maxOrderNo, code, listQuota));
                    maxOrderNo++;
                    code++;
                }
                #endregion
                #region 更新工程任务
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", oprNode.Id));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("ProjectTaskTypeGUID", NHibernate.FetchMode.Eager);
                oprNode = model.ObjectQuery(typeof(GWBSTree), oq)[0] as GWBSTree;

                foreach (GWBSDetail d in newDetailList)
                {
                    d.TheGWBS = oprNode;
                    d.TheGWBSSysCode = oprNode.SysCode;
                    //d.TheGWBSFullPath = oprNode.FullPath;

                    oprNode.Details.Add(d);
                }

                //根据明细设置任务对象的标记
                bool taskResponsibleFlag = false;
                bool taskCostAccFlag = false;
                bool taskProductConfirmFlag = false;
                bool taskSubContractFeeFlag = false;

                foreach (GWBSDetail dtl in oprNode.Details)
                {
                    if (dtl.ResponseFlag == 1)
                        taskResponsibleFlag = true;
                    if (dtl.CostingFlag == 1)
                        taskCostAccFlag = true;
                    if (dtl.ProduceConfirmFlag == 1)
                        taskProductConfirmFlag = true;
                    if (dtl.SubContractFeeFlag)
                        taskSubContractFeeFlag = true;
                }

                oprNode.ResponsibleAccFlag = taskResponsibleFlag;
                oprNode.CostAccFlag = taskCostAccFlag;
                oprNode.ProductConfirmFlag = taskProductConfirmFlag;
                oprNode.SubContractFeeFlag = taskSubContractFeeFlag;

                #region 计算合价
                GWBSTree tempNode = new GWBSTree();
                tempNode.Id = oprNode.Id;
                tempNode.SysCode = oprNode.SysCode;
                tempNode = model.AccountTotalPriceByChilds(tempNode);
                List<Decimal> listTotalPrice = StaticMethod.GetTaskDtlTotalPrice(oprNode);
                oprNode.ContractTotalPrice = tempNode.ContractTotalPrice + listTotalPrice[0];
                oprNode.ResponsibilityTotalPrice = tempNode.ResponsibilityTotalPrice + listTotalPrice[1];
                oprNode.PlanTotalPrice = tempNode.PlanTotalPrice + listTotalPrice[2];
                #endregion
                #endregion
                #region 保存

                string sqlExWhere = "";
                foreach (GWBSDetail dtl in oprNode.Details)
                {
                    if (!string.IsNullOrEmpty(dtl.Id))
                        sqlExWhere += "'" + dtl.Id + "',";
                }
                if (sqlExWhere != "")
                {
                    sqlExWhere = sqlExWhere.Substring(0, sqlExWhere.Length - 1);
                    sqlExWhere = " and t1.id not in(" + sqlExWhere + ")";
                }

                IList listResult = model.SaveBatchDetail(oprNode, sqlExWhere);
                oprNode = listResult[0] as GWBSTree;
                tvwCategory.SelectedNode.Tag = oprNode;

                List<GWBSDetail> listDtl = listResult[1] as List<GWBSDetail>;
                foreach (GWBSDetail dtl in listDtl)
                {
                    AddDetailInfoInGrid(dtl, false);
                }

                //IList oprNodeSave = new ArrayList();
                //oprNodeSave.Add(oprNode);
                //oprNode = model.SaveOrUpdate(oprNodeSave)[0] as GWBSTree;
                //tvwCategory.SelectedNode.Tag = oprNode;
                //oprDtl = oprNode.Details.ElementAt(oprNode.Details.Count - 1);
                //foreach (GWBSDetail d in oprNode.Details)
                //{
                //    if (d.OrderNo > oldMaxOrderNo)
                //    {
                //        AddDetailInfoInGrid(d);
                //    }
                //}

                #endregion
            }


        }
        // 新建明细并赋值
        GWBSDetail NewGWBSDetail(CostItem item, int maxOrderNo, int code, IList listQuota)
        {
            oprDtl = new GWBSDetail();
            oprDtl.TheGWBS = oprNode;
            oprDtl.TheGWBSSysCode = oprNode.SysCode;
            //oprDtl.TheGWBSFullPath = oprNode.FullPath;
            oprDtl.Name = item.Name;
            oprDtl.OrderNo = maxOrderNo + 1;

            if (oprNode.ProjectTaskTypeGUID != null)
                oprDtl.ProjectTaskTypeCode = oprNode.ProjectTaskTypeGUID.Code;

            oprDtl.Code = oprNode.Code + "-" + code.ToString().PadLeft(5, '0');
            oprDtl.State = VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit;
            oprDtl.CurrentStateTime = model.GetServerTime();

            ContractGroup cg = txtContractGroupName.Tag as ContractGroup;
            oprDtl.ContractGroupCode = cg.Code;
            oprDtl.ContractGroupGUID = cg.Id;
            oprDtl.ContractGroupName = cg.ContractName;
            oprDtl.ContractGroupType = cg.ContractGroupType;

            if (projectInfo != null)
            {
                oprDtl.TheProjectGUID = projectInfo.Id;
                oprDtl.TheProjectName = projectInfo.Name;
            }

            oprDtl.TheCostItem = item;
            oprDtl.TheCostItemCateSyscode = item.TheCostItemCateSyscode;

            ResourceGroup mainResource = null;

            foreach (SubjectCostQuota quota in listQuota)
            {
                if (quota.TheCostItem.Id == item.Id)
                {
                    if (quota.MainResourceFlag && quota.ListResources.Count > 0)
                    {
                        mainResource = quota.ListResources.ElementAt(0);
                    }

                    var query = from q in oprDtl.ListCostSubjectDetails
                                where q.ResourceUsageQuota.Id == quota.Id
                                select q;

                    if (query.Count() > 0)
                    {
                        continue;
                    }

                    AddResourceUsageInTaskDetail(quota);
                }
            }
            if (mainResource != null)
            {
                oprDtl.MainResourceTypeId = mainResource.ResourceTypeGUID;
                oprDtl.MainResourceTypeName = mainResource.ResourceTypeName;
                oprDtl.MainResourceTypeQuality = mainResource.ResourceTypeQuality;
                oprDtl.MainResourceTypeSpec = mainResource.ResourceTypeSpec;
                oprDtl.DiagramNumber = mainResource.DiagramNumber;
            }
            //更新分包措施费率
            if (item.ContentType == CostItemContentType.分包取费)
            {
                oprDtl.SubContractFeeFlag = true;
                oprDtl.SubContractStepRate = item.PricingRate;
            }
            else
            {
                oprDtl.SubContractFeeFlag = false;
                oprDtl.SubContractStepRate = 0;
            }
            oprDtl.WorkAmountUnitGUID = item.ProjectUnitGUID;
            oprDtl.WorkAmountUnitName = item.ProjectUnitName;
            oprDtl.PriceUnitGUID = item.PriceUnitGUID;
            oprDtl.PriceUnitName = item.PriceUnitName;

            oprDtl.ResponseFlag = 0;
            oprDtl.CostingFlag = 1;
            oprDtl.ProduceConfirmFlag = 0;
            //if (oprNode.CategoryNodeType == NodeType.MiddleNode)
            //{
            //    oprDtl.CostingFlag = 1;
            //    oprDtl.ProduceConfirmFlag = 0;
            //}
            //else if (oprNode.CategoryNodeType == NodeType.LeafNode)
            //{
            //    oprDtl.CostingFlag = 0;
            //    oprDtl.ProduceConfirmFlag = 1;
            //}
            oprDtl.SubContractFeeFlag = false;

            oprDtl.AddupAccQuantity = 0;
            oprDtl.AddupAccFigureProgress = 0;
            oprDtl.QuantityConfirmed = 0;
            oprDtl.ProgressConfirmed = 0;

            //重新计算合同、责任、计划单价、合价
            #region 计算耗用信息
            for (int i = 0; i < oprDtl.ListCostSubjectDetails.Count; i++)
            {
                GWBSDetailCostSubject dtl = oprDtl.ListCostSubjectDetails.ElementAt(i);

                //合同收入
                //i）如果【成本核算科目】为“人工费”，则【资源合同工程量单价】等于所属{工程任务明细}_【合同单价】减去其余兄弟{工程资源耗用明细}_【资源合同工程量单价】之和；
                //ii）【资源合同工程量】：=所属{工程任务明细}_【合同工程量】*【合同定额数量】；
                //iii）【资源合同合价】：=【资源合同工程量】*【资源合同工程量单价】；

                dtl.ContractProjectAmount = oprDtl.ContractProjectQuantity * dtl.ContractQuotaQuantity;
                dtl.ContractTotalPrice = dtl.ContractProjectAmount * dtl.ContractQuantityPrice;

                //责任成本
                //a）针对<操作{工程资源耗用明细}集>在列表框中显示的每一个对象进行如下操作：
                //【责任耗用数量】：=所属{工程任务明细}_【责任工程量】*【责任定额数量】；
                //【责任耗用合价】：=【责任耗用数量】*【责任数量单价】；
                dtl.ResponsibilitilyWorkAmount = oprDtl.ResponsibilitilyWorkAmount * dtl.ResponsibleQuotaNum;
                dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyWorkAmount * dtl.ResponsibilitilyPrice;



                //计划成本
                //a）针对<操作{工程资源耗用明细}集>在列表框中显示的每一个对象进行如下操作：
                //【计划耗用数量】：=所属{工程任务明细}_【计划工程量】*【计划定额数量】；
                //【计划耗用合价】：=【计划耗用数量】*【计划数量单价】；
                dtl.PlanWorkAmount = oprDtl.PlanWorkAmount * dtl.PlanQuotaNum;
                dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;


            }
            #endregion

            #region 计算合价
            //合同收入
            //【合同单价】：=下属{工程资源耗用明细}_【合同工程量单价】
            //【合同合价】：=【合同工程量】*【合同单价】
            //责任成本
            //【责任单价】：=下属{工程资源耗用明细}_【责任工程量单价】
            //【责任合价】：=【责任工程量】*【责任单价】
            //计划成本
            //【计划单价】：=下属{工程资源耗用明细}_【计划工程量单价】
            //【计划合价】：=【计划工程量】*【计划单价】

            decimal dtlUsageProjectAmountPriceByContract = 0;
            decimal dtlUsageProjectAmountPriceByResponsible = 0;
            decimal dtlUsageProjectAmountPriceByPlan = 0;
            foreach (GWBSDetailCostSubject subject in oprDtl.ListCostSubjectDetails)
            {
                dtlUsageProjectAmountPriceByContract += subject.ContractPrice;
                dtlUsageProjectAmountPriceByResponsible += subject.ResponsibleWorkPrice;
                dtlUsageProjectAmountPriceByPlan += subject.PlanWorkPrice;
            }

            oprDtl.ContractPrice = dtlUsageProjectAmountPriceByContract;
            oprDtl.ResponsibilitilyPrice = dtlUsageProjectAmountPriceByResponsible;
            oprDtl.PlanPrice = dtlUsageProjectAmountPriceByPlan;


            oprDtl.ContractTotalPrice = oprDtl.ContractProjectQuantity * oprDtl.ContractPrice;
            oprDtl.ResponsibilitilyTotalPrice = oprDtl.ResponsibilitilyWorkAmount * oprDtl.ResponsibilitilyPrice;
            oprDtl.PlanTotalPrice = oprDtl.PlanWorkAmount * oprDtl.PlanPrice;

            #endregion
            return oprDtl;
        }
        //插入明细
        void btnInsertDetail_Click(object sender, EventArgs e)
        {
            IsAddDetail = false;

            if (gridGWBDetail.SelectedRows.Count == 0 || gridGWBDetail.SelectedRows.Count > 1)
            {
                MessageBox.Show("请选择一条基于插入的任务明细！");
                return;
            }
            else if (!valideContractGroup())
            {
                return;
            }


            //在节点选择的时候有对象关联查询，但偶尔会有丢失延迟加载的对象，根据需要重新加载
            try
            {
                if (oprNode.ProjectTaskTypeGUID != null)
                {
                    string name = oprNode.ProjectTaskTypeGUID.Name;
                }
            }
            catch
            {
                oprNode = LoadRelaPBS(oprNode);
            }

            ClearDetailAll();
            RefreshDetailControls(MainViewState.Modify);

            InsertAndSetNewDetailInfo();

            tabBaseInfo.SelectedIndex = 0;
            txtDtlCostItem.Focus();
        }
        //向上排序
        void btnOrderDetailUp_Click(object sender, EventArgs e)
        {
            if (gridGWBDetail.SelectedRows.Count == 0 || gridGWBDetail.SelectedRows.Count > 1)
            {
                MessageBox.Show("请选择一条要排序的任务明细！");
                return;
            }
            int rowIndex = gridGWBDetail.SelectedRows[0].Index;
            if (rowIndex == 0)
                return;

            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);

            GWBSDetail currDtl = gridGWBDetail.Rows[rowIndex].Tag as GWBSDetail;
            oq.AddCriterion(Expression.Eq("Id", currDtl.Id));
            currDtl = model.ObjectQuery(typeof(GWBSDetail), oq)[0] as GWBSDetail;

            oq.Criterions.Clear();

            GWBSDetail prevDtl = gridGWBDetail.Rows[rowIndex - 1].Tag as GWBSDetail;
            oq.AddCriterion(Expression.Eq("Id", prevDtl.Id));
            prevDtl = model.ObjectQuery(typeof(GWBSDetail), oq)[0] as GWBSDetail;

            int tempOrderNo = currDtl.OrderNo;
            currDtl.OrderNo = prevDtl.OrderNo;
            prevDtl.OrderNo = tempOrderNo;

            try
            {
                IList list = new ArrayList();
                list.Add(prevDtl);
                list.Add(currDtl);
                list = model.SaveOrUpdateDetail(list);

                gridGWBDetail.Rows.RemoveAt(rowIndex);
                InsertDetailInfoInGrid(list[1] as GWBSDetail, rowIndex - 1);

                foreach (GWBSDetail dtl in list)
                {
                    UpdateDetailInfoInGrid(dtl, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        //向下排序
        void btnOrderDetailDown_Click(object sender, EventArgs e)
        {
            if (gridGWBDetail.SelectedRows.Count == 0 || gridGWBDetail.SelectedRows.Count > 1)
            {
                MessageBox.Show("请选择一条要排序的任务明细！");
                return;
            }
            int rowIndex = gridGWBDetail.SelectedRows[0].Index;
            if (rowIndex == gridGWBDetail.Rows.Count - 1)
                return;

            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);

            GWBSDetail currDtl = gridGWBDetail.Rows[rowIndex].Tag as GWBSDetail;
            oq.AddCriterion(Expression.Eq("Id", currDtl.Id));
            currDtl = model.ObjectQuery(typeof(GWBSDetail), oq)[0] as GWBSDetail;

            oq.Criterions.Clear();

            GWBSDetail nextDtl = gridGWBDetail.Rows[rowIndex + 1].Tag as GWBSDetail;
            oq.AddCriterion(Expression.Eq("Id", nextDtl.Id));
            nextDtl = model.ObjectQuery(typeof(GWBSDetail), oq)[0] as GWBSDetail;


            int tempOrderNo = currDtl.OrderNo;
            currDtl.OrderNo = nextDtl.OrderNo;
            nextDtl.OrderNo = tempOrderNo;

            try
            {
                IList list = new ArrayList();
                list.Add(nextDtl);
                list.Add(currDtl);
                list = model.SaveOrUpdateDetail(list);

                gridGWBDetail.Rows.RemoveAt(rowIndex);
                InsertDetailInfoInGrid(list[1] as GWBSDetail, rowIndex + 1);

                foreach (GWBSDetail dtl in list)
                {
                    UpdateDetailInfoInGrid(dtl, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        //增加并设置新明细信息
        private void AddAndSetNewDetailInfo()
        {
            oprDtl = new GWBSDetail();
            oprDtl.TheGWBS = oprNode;
            oprDtl.TheGWBSSysCode = oprNode.SysCode;
            //oprDtl.TheGWBSFullPath = oprNode.FullPath;
            int maxOrderNo = 1;

            int code = gridGWBDetail.Rows.Count + 1;
            //获取父对象下最大明细号
            for (int i = 0; i < gridGWBDetail.Rows.Count; i++)
            {
                GWBSDetail dtl = gridGWBDetail.Rows[i].Tag as GWBSDetail;

                if (dtl != null)
                {
                    if (dtl.OrderNo > maxOrderNo)
                        maxOrderNo = dtl.OrderNo;

                    if (!string.IsNullOrEmpty(dtl.Code))
                    {
                        try
                        {
                            if (dtl.Code.IndexOf("-") > -1)
                            {
                                int tempCode = Convert.ToInt32(dtl.Code.Substring(dtl.Code.LastIndexOf("-") + 1));
                                if (tempCode >= code)
                                    code = tempCode + 1;
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
            oprDtl.OrderNo = maxOrderNo + 1;

            if (oprNode.ProjectTaskTypeGUID != null)
                oprDtl.ProjectTaskTypeCode = oprNode.ProjectTaskTypeGUID.Code;

            oprDtl.Code = oprNode.Code + "-" + code.ToString().PadLeft(5, '0');
            oprDtl.State = VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit;
            oprDtl.CurrentStateTime = model.GetServerTime();

            ContractGroup cg = txtContractGroupName.Tag as ContractGroup;
            oprDtl.ContractGroupCode = cg.Code;
            oprDtl.ContractGroupGUID = cg.Id;
            oprDtl.ContractGroupName = cg.ContractName;
            oprDtl.ContractGroupType = cg.ContractGroupType;

            if (projectInfo != null)
            {
                oprDtl.TheProjectGUID = projectInfo.Id;
                oprDtl.TheProjectName = projectInfo.Name;
            }

            //txtDtlCode.Text = oprDtl.Code;
            txtDtlState.Text = StaticMethod.GetWBSTaskStateText(oprDtl.State);
            txtDtlStateTime.Text = oprDtl.CurrentStateTime.ToString();

            txtDtlContractGroupName.Text = cg.ContractName;
            txtDtlContractGroupType.Text = cg.ContractGroupType;
            cbResponseAccount.Checked = cbCostAccount.Checked = cbProductConfirm.Checked = true;

        }
        //复制任务明细信息
        private GWBSDetail CopyDetailInfo(GWBSDetail targetDtl)
        {
            GWBSDetail newDtl = new GWBSDetail();
            newDtl.TheProjectGUID = targetDtl.TheProjectGUID;
            newDtl.TheProjectName = targetDtl.TheProjectName;
            newDtl.TheGWBS = oprNode;
            newDtl.TheGWBSSysCode = oprNode.SysCode;
            //newDtl.TheGWBSFullPath = oprNode.FullPath;
            int maxOrderNo = 1;

            int code = gridGWBDetail.Rows.Count + 1;
            //获取父对象下最大明细号
            for (int i = 0; i < gridGWBDetail.Rows.Count; i++)
            {
                GWBSDetail dtl = gridGWBDetail.Rows[i].Tag as GWBSDetail;

                if (dtl != null)
                {
                    if (dtl.OrderNo > maxOrderNo)
                        maxOrderNo = dtl.OrderNo;

                    if (!string.IsNullOrEmpty(dtl.Code))
                    {
                        try
                        {
                            if (dtl.Code.IndexOf("-") > -1)
                            {
                                int tempCode = Convert.ToInt32(dtl.Code.Substring(dtl.Code.LastIndexOf("-") + 1));
                                if (tempCode >= code)
                                    code = tempCode + 1;
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
            newDtl.OrderNo = maxOrderNo + 1;
            newDtl.Code = oprNode.Code + "-" + code.ToString().PadLeft(5, '0');

            newDtl.ContractGroupCode = targetDtl.ContractGroupCode;
            newDtl.ContractGroupGUID = targetDtl.ContractGroupGUID;
            newDtl.ContractGroupName = targetDtl.ContractGroupName;
            newDtl.ContractGroupType = targetDtl.ContractGroupType;
            newDtl.ContractPrice = targetDtl.ContractPrice;
            newDtl.ContractProjectQuantity = targetDtl.ContractProjectQuantity;
            newDtl.ContractTotalPrice = targetDtl.ContractTotalPrice;
            newDtl.CostingFlag = targetDtl.CostingFlag;
            newDtl.DiagramNumber = targetDtl.DiagramNumber;
            newDtl.MainResourceTypeId = targetDtl.MainResourceTypeId;
            newDtl.MainResourceTypeName = targetDtl.MainResourceTypeName;
            newDtl.MainResourceTypeQuality = targetDtl.MainResourceTypeQuality;
            newDtl.MainResourceTypeSpec = targetDtl.MainResourceTypeSpec;
            newDtl.Name = targetDtl.Name;
            newDtl.PlanPrice = targetDtl.PlanPrice;
            newDtl.PlanWorkAmount = targetDtl.PlanWorkAmount;
            newDtl.PlanTotalPrice = targetDtl.PlanTotalPrice;
            newDtl.PriceUnitGUID = targetDtl.PriceUnitGUID;
            newDtl.PriceUnitName = targetDtl.PriceUnitName;
            newDtl.ProduceConfirmFlag = targetDtl.ProduceConfirmFlag;
            newDtl.ProjectTaskTypeCode = targetDtl.ProjectTaskTypeCode;
            newDtl.ResponseFlag = targetDtl.ResponseFlag;
            newDtl.ResponsibilitilyPrice = targetDtl.ResponsibilitilyPrice;
            newDtl.ResponsibilitilyWorkAmount = targetDtl.ResponsibilitilyWorkAmount;
            newDtl.ResponsibilitilyTotalPrice = targetDtl.ResponsibilitilyTotalPrice;
            newDtl.State = DocumentState.Edit;
            newDtl.CurrentStateTime = model.GetServerTime();
            newDtl.SubContractFeeFlag = targetDtl.SubContractFeeFlag;
            newDtl.SubContractStepRate = targetDtl.SubContractStepRate;
            newDtl.TheCostItem = targetDtl.TheCostItem;//临时存储
            newDtl.WorkAmountUnitGUID = targetDtl.WorkAmountUnitGUID;
            newDtl.WorkAmountUnitName = targetDtl.WorkAmountUnitName;

            newDtl = model.SaveBackoutGWBSDetail(newDtl, "拆");

            return newDtl;
        }
        private void btnChange_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridGWBDetail.CurrentRow == null)
                {
                    throw new Exception("请选择需要签证变更的任务明细记录");
                }
                else
                {
                    GWBSDetail targetDtl = gridGWBDetail.CurrentRow.Tag as GWBSDetail;
                    ContractGroup oContractGroup = null;
                    if (string.IsNullOrEmpty(targetDtl.ChangeParentID))
                    {
                        //if (MessageBox.Show("是否确定对[" + targetDtl.Name + "]进行签证变更", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //{
                        VSelectWBSContractGroup frm = new VSelectWBSContractGroup(new MWBSContractGroup(), false, new string[] { "工程签证索赔", "设计变更单" });

                        frm.ShowDialog();
                        if (!frm.isOK) return;
                        oContractGroup = frm.SelectResult == null || frm.SelectResult.Count == 0 ? null : frm.SelectResult[0] as ContractGroup;
                        if (oContractGroup == null) throw new Exception("请选择签证变更契约");
                        oprDtl = targetDtl.Clone(oContractGroup);
                        int maxOrderNo = 1;
                        int code = gridGWBDetail.Rows.Count + 1;
                        int iNum = 0;
                        //获取父对象下最大明细号
                        #region
                        IList<GWBSDetail> lstDetail = new List<GWBSDetail>();
                        GWBSDetail dtl = null;
                        lstDetail = gridGWBDetail.Rows.OfType<DataGridViewRow>().Where(a => a.Tag != null).Select(a => a.Tag as GWBSDetail).ToList();
                        //for (int i = 0; i < gridGWBDetail.Rows.Count; i++)
                        //{
                        //    dtl = gridGWBDetail.Rows[i].Tag as GWBSDetail;
                        //    if (dtl != null)
                        //    {
                        //        lstDetail.Add(dtl);
                        //    }
                        //}
                        if (lstDetail.Count > 0)
                        {
                            maxOrderNo = lstDetail.Max(a => a.OrderNo);
                            string[] arr1 = lstDetail.Select(a => a.Code).ToArray();
                            int[] arr = lstDetail.Where(a => !string.IsNullOrEmpty(a.Code)).Select(a => Convert.ToInt32(a.Code.Substring(a.Code.LastIndexOf("-") + 1))).ToArray<int>();
                            code = lstDetail.Where(a => !string.IsNullOrEmpty(a.Code)).Select(a => Convert.ToInt32(a.Code.Substring(a.Code.LastIndexOf("-") + 1))).Max();
                            iNum = lstDetail.Where(a => !string.IsNullOrEmpty(a.ChangeParentID) && a.ChangeParentID == targetDtl.Id).Count();
                        }
                        else
                        {
                            maxOrderNo = 0;
                            code = 0;
                            iNum = 0;
                        }
                        #endregion
                        #region 获取编号 和code
                        //if (dtl != null)
                        //    {
                        //        if (dtl.OrderNo > maxOrderNo)
                        //            maxOrderNo = dtl.OrderNo;

                        //        if (!string.IsNullOrEmpty(dtl.Code))
                        //        {
                        //            try
                        //            {
                        //                if (dtl.Code.IndexOf("-") > -1)
                        //                {
                        //                    int tempCode = Convert.ToInt32(dtl.Code.Substring(dtl.Code.LastIndexOf("-") + 1));
                        //                    if (tempCode >= code)
                        //                        code = tempCode + 1;
                        //                }
                        //            }
                        //            catch
                        //            {

                        //            }
                        //        }
                        //    }
                        #endregion

                        oprDtl.OrderNo = maxOrderNo + 1;
                        oprDtl.Code = oprNode.Code + "-" + (code + 1).ToString().PadLeft(5, '0');
                        if (string.IsNullOrEmpty(targetDtl.Name))
                        {
                            oprDtl.Name = string.Format("[变{0}]", iNum + 1);
                        }
                        else
                        {
                            oprDtl.Name = string.Format("{0}-[变{1}]", targetDtl.Name, iNum + 1);
                        }
                        if (string.IsNullOrEmpty(targetDtl.DiagramNumber))
                        {
                            oprDtl.DiagramNumber = string.Format("[变{0}]", iNum + 1);
                        }
                        else
                        {
                            oprDtl.DiagramNumber = string.Format("{0}-[变{1}]", targetDtl.DiagramNumber, iNum + 1);
                        }
                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("Id", oprNode.Id));
                        oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                        oq.AddFetchMode("ProjectTaskTypeGUID", NHibernate.FetchMode.Eager);
                        oprNode = model.ObjectQuery(typeof(GWBSTree), oq)[0] as GWBSTree;
                        oprDtl.TheGWBS = oprNode;
                        oprNode.Details.Add(oprDtl);
                        oprNode = model.SaveOrUpdateGWBSTree(oprNode, null);
                        tvwCategory.SelectedNode.Tag = oprNode;
                        oprDtl = oprNode.Details.ElementAt(oprNode.Details.Count - 1);
                        AddDetailInfoInGrid(oprDtl, true);
                        // }
                    }
                    else
                    {
                        throw new Exception("请不要选择已经签证变更明细作为变更单据");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("签证变更失败:{0}", ExceptionUtil.ExceptionMessage(ex)));
            }
        }

        //插入并设置新明细信息
        private void InsertAndSetNewDetailInfo()
        {
            oprDtl = new GWBSDetail();
            oprDtl.TheGWBS = oprNode;
            oprDtl.TheGWBSSysCode = oprNode.SysCode;
            //oprDtl.TheGWBSFullPath = oprNode.FullPath;

            int code = gridGWBDetail.Rows.Count + 1;
            //获取父对象下最大明细号
            for (int i = 0; i < gridGWBDetail.Rows.Count; i++)
            {
                GWBSDetail dtl = gridGWBDetail.Rows[i].Tag as GWBSDetail;

                if (dtl != null && !string.IsNullOrEmpty(dtl.Code))
                {
                    try
                    {
                        if (dtl.Code.IndexOf("-") > -1)
                        {
                            int tempCode = Convert.ToInt32(dtl.Code.Substring(dtl.Code.LastIndexOf("-") + 1));
                            if (tempCode >= code)
                                code = tempCode + 1;
                        }
                    }
                    catch
                    {
                    }
                }
            }

            GWBSDetail selectedDtl = gridGWBDetail.SelectedRows[0].Tag as GWBSDetail;
            oprDtl.OrderNo = selectedDtl.OrderNo;

            if (oprNode.ProjectTaskTypeGUID != null)
                oprDtl.ProjectTaskTypeCode = oprNode.ProjectTaskTypeGUID.Code;

            oprDtl.Code = oprNode.Code + "-" + code.ToString().PadLeft(5, '0');
            oprDtl.State = VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit;
            oprDtl.CurrentStateTime = model.GetServerTime();

            ContractGroup cg = txtContractGroupName.Tag as ContractGroup;
            oprDtl.ContractGroupCode = cg.Code;
            oprDtl.ContractGroupGUID = cg.Id;
            oprDtl.ContractGroupName = cg.ContractName;
            oprDtl.ContractGroupType = cg.ContractGroupType;

            if (projectInfo != null)
            {
                oprDtl.TheProjectGUID = projectInfo.Id;
                oprDtl.TheProjectName = projectInfo.Name;
            }

            //txtDtlCode.Text = oprDtl.Code;
            txtDtlState.Text = StaticMethod.GetWBSTaskStateText(oprDtl.State);
            txtDtlStateTime.Text = oprDtl.CurrentStateTime.ToString();

            txtDtlContractGroupName.Text = cg.ContractName;
            txtDtlContractGroupType.Text = cg.ContractGroupType;

        }

        private void AddDetailInfoInGrid(GWBSDetail dtl, bool isSetCurrCell)
        {
            int index = gridGWBDetail.Rows.Add();
            DataGridViewRow row = gridGWBDetail.Rows[index];
            row.Cells[DtlName.Name].Value = dtl.Name;
            row.Cells[DtlOrderNo.Name].Value = dtl.OrderNo.ToString();
            if (dtl.TheCostItem != null)
            {
                row.Cells[DtlCostItem.Name].Value = dtl.TheCostItem.Name;
                row.Cells[DtlCostItemCode.Name].Value = dtl.TheCostItem.QuotaCode;
            }
            row.Cells[DtlMainResourceType.Name].Value = dtl.MainResourceTypeName;
            row.Cells[DtlMainResourceTypeQuality.Name].Value = dtl.MainResourceTypeQuality;
            row.Cells[DtlMainResourceTypeSpec.Name].Value = dtl.MainResourceTypeSpec;
            row.Cells[DiagramNumber.Name].Value = dtl.DiagramNumber;

            row.Cells[DtlQuantityUnit.Name].Value = dtl.WorkAmountUnitName;
            row.Cells[DtlPlanQuantity.Name].Value = dtl.PlanWorkAmount;
            row.Cells[DtlPlanPrice.Name].Value = dtl.PlanPrice;
            row.Cells[DtlPlanTotalPrice.Name].Value = dtl.PlanTotalPrice;
            row.Cells[DtlPriceUnit.Name].Value = dtl.PriceUnitName;

            row.Cells[DtlState.Name].Value = StaticMethod.GetWBSTaskStateText(dtl.State);
            row.Cells[DtlDesc.Name].Value = dtl.ContentDesc;
            row.Cells[this.colContractGroupName.Name].Value = dtl.ContractGroupName;
            row.Tag = dtl;

            if (isSetCurrCell)
                gridGWBDetail.CurrentCell = row.Cells[0];

            if (dtl.PlanWorkAmount == 0)
            {
                //row.Cells[DtlPlanQuantity.Name].Style.BackColor = Color.Pink;
                row.DefaultCellStyle.BackColor = Color.Pink;
            }
        }
        private void InsertDetailInfoInGrid(GWBSDetail dtl, int currRowIndex)
        {
            int index = currRowIndex;
            gridGWBDetail.Rows.Insert(index, 1);
            DataGridViewRow row = gridGWBDetail.Rows[index];

            row.Cells[DtlName.Name].Value = dtl.Name;
            row.Cells[DtlOrderNo.Name].Value = dtl.OrderNo.ToString();
            if (dtl.TheCostItem != null)
            {
                row.Cells[DtlCostItem.Name].Value = dtl.TheCostItem.Name;
                row.Cells[DtlCostItemCode.Name].Value = dtl.TheCostItem.QuotaCode;
            }
            row.Cells[DtlMainResourceType.Name].Value = dtl.MainResourceTypeName;
            row.Cells[DtlMainResourceTypeQuality.Name].Value = dtl.MainResourceTypeQuality;
            row.Cells[DtlMainResourceTypeSpec.Name].Value = dtl.MainResourceTypeSpec;
            row.Cells[DiagramNumber.Name].Value = dtl.DiagramNumber;

            row.Cells[DtlQuantityUnit.Name].Value = dtl.WorkAmountUnitName;
            row.Cells[DtlPlanQuantity.Name].Value = dtl.PlanWorkAmount;
            row.Cells[DtlPlanPrice.Name].Value = dtl.PlanPrice;
            row.Cells[DtlPlanTotalPrice.Name].Value = dtl.PlanTotalPrice;
            row.Cells[DtlPriceUnit.Name].Value = dtl.PriceUnitName;

            row.Cells[DtlState.Name].Value = StaticMethod.GetWBSTaskStateText(dtl.State);
            row.Cells[DtlDesc.Name].Value = dtl.ContentDesc;



            row.Tag = dtl;

            gridGWBDetail.CurrentCell = row.Cells[0];

            try
            {
                //if (dtl.ListCostSubjectDetails != null && dtl.ListCostSubjectDetails.Count > 0)
                //{
                //    gridCostSubject.Rows.Clear();
                //    foreach (GWBSDetailCostSubject subject in dtl.ListCostSubjectDetails)
                //    {
                //        AddCostSubjectInfoInGrid(subject);
                //    }
                //}
            }
            catch { }
        }

        private void UpdateDetailInfoInGrid(GWBSDetail dtl, bool isSelected)
        {
            for (int i = 0; i < gridGWBDetail.Rows.Count; i++)
            {
                DataGridViewRow row = gridGWBDetail.Rows[i];
                GWBSDetail d = row.Tag as GWBSDetail;
                if (d.Id == dtl.Id)
                {
                    row.Cells[DtlName.Name].Value = dtl.Name;
                    row.Cells[DtlOrderNo.Name].Value = dtl.OrderNo.ToString();
                    if (dtl.TheCostItem != null)
                    {
                        row.Cells[DtlCostItem.Name].Value = dtl.TheCostItem.Name;
                        row.Cells[DtlCostItemCode.Name].Value = dtl.TheCostItem.QuotaCode;
                    }
                    row.Cells[DtlMainResourceType.Name].Value = dtl.MainResourceTypeName;
                    row.Cells[DtlMainResourceTypeQuality.Name].Value = dtl.MainResourceTypeQuality;
                    row.Cells[DtlMainResourceTypeSpec.Name].Value = dtl.MainResourceTypeSpec;
                    row.Cells[DiagramNumber.Name].Value = dtl.DiagramNumber;

                    row.Cells[DtlQuantityUnit.Name].Value = dtl.WorkAmountUnitName;
                    row.Cells[DtlPlanQuantity.Name].Value = dtl.PlanWorkAmount;
                    row.Cells[DtlPlanPrice.Name].Value = dtl.PlanPrice;
                    row.Cells[DtlPlanTotalPrice.Name].Value = dtl.PlanTotalPrice;
                    row.Cells[DtlPriceUnit.Name].Value = dtl.PriceUnitName;

                    row.Cells[DtlState.Name].Value = StaticMethod.GetWBSTaskStateText(dtl.State);
                    row.Cells[DtlDesc.Name].Value = dtl.ContentDesc;


                    row.Tag = dtl;

                    if (isSelected)
                        gridGWBDetail.CurrentCell = row.Cells[0];
                    break;
                }
            }
        }
        //选择成本项
        void btnSelectCostItem_Click(object sender, EventArgs e)
        {
            string syscode = string.Empty;
            if (txtCostItemsZoning.Tag != null)
            {
                CostItemsZoning z = txtCostItemsZoning.Tag as CostItemsZoning;
                syscode = z.CostItemsCateSyscode;
            }

            VSelectCostItem frm = new VSelectCostItem(new MCostItem(), syscode, IsCostItemSingleSelect);
            if (txtDtlCostItem.Tag != null)
            {
                frm.DefaultSelectedCostItem = txtDtlCostItem.Tag as CostItem;
            }
            frm.ShowDialog();
            if (IsCostItemSingleSelect == false)//多选时
            {
                FlashScreen.Show();
                if (frm.SelectCostItemList == null || frm.SelectCostItemList.Count == 0)
                {
                    return;
                }
                else if (frm.SelectCostItemList.Count == 1)
                {
                    CostItem item = frm.SelectCostItemList[0] as CostItem;
                    UpdateDtlCostItem(item, false);
                }
                else//选择多个成本项时直接保存为陈本明细
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", oprNode.Id));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                    oq.AddFetchMode("ProjectTaskTypeGUID", NHibernate.FetchMode.Eager);
                    oq.AddFetchMode("Details.TheCostItem", NHibernate.FetchMode.Eager);
                    oprNode = model.ObjectQuery(typeof(GWBSTree), oq)[0] as GWBSTree;
                    /*
                     * 此四项的取值算法为：只要有一个明细的该项为true,则节点的该属性为true
                     * 而此处新增的明细默认全为true，故将节点的属性也设置为true
                    */
                    oprNode.ResponsibleAccFlag = true;
                    oprNode.CostAccFlag = true;
                    oprNode.ProductConfirmFlag = true;
                    oprNode.SubContractFeeFlag = false;

                    ObjectQuery oqQuota = new ObjectQuery();
                    Disjunction dis = new Disjunction();
                    foreach (CostItem item in frm.SelectCostItemList)
                    {
                        dis.Add(Expression.Eq("TheCostItem.Id", item.Id));
                    }
                    oqQuota.AddCriterion(dis);
                    oqQuota.AddCriterion(Expression.Eq("State", SubjectCostQuotaState.生效));
                    oqQuota.AddFetchMode("CostAccountSubjectGUID", NHibernate.FetchMode.Eager);
                    oqQuota.AddFetchMode("ProjectAmountUnitGUID", NHibernate.FetchMode.Eager);
                    oqQuota.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
                    oqQuota.AddFetchMode("ListResources", NHibernate.FetchMode.Eager);
                    List<SubjectCostQuota> listQuota = model.ObjectQuery(typeof(SubjectCostQuota), oqQuota).OfType<SubjectCostQuota>().ToList();

                    oqQuota = new ObjectQuery();
                    dis = new Disjunction();
                    foreach (CostItem item in frm.SelectCostItemList)
                    {
                        dis.Add(Expression.Eq("Id", item.Id));
                    }
                    oqQuota.AddCriterion(dis);
                    //oqQuota.AddCriterion(Expression.Not(Expression.Eq("ItemState", CostItemState.作废)));
                    List<CostItem> lstCostItem = model.ObjectQuery(typeof(CostItem), oqQuota).OfType<CostItem>().ToList();

                    List<string> lstDetailIdInDB = oprNode.Details.Select(p => p.Id).ToList();//数据库中原有的明细ID
                    foreach (CostItem item in lstCostItem)
                    {
                        var curItemQuotas = listQuota.Where(p => p.TheCostItem.Id == item.Id).ToList();
                        ResourceGroup mainResource = null;
                        GWBSDetail newDeatail = new GWBSDetail();
                        newDeatail.Name = item.Name;
                        //合同收入
                        newDeatail.ContractProjectQuantity = 0;
                        //责任成本
                        newDeatail.ResponseFlag = 1;
                        //计划成本
                        newDeatail.CostingFlag = 1;
                        newDeatail.ProduceConfirmFlag = 1;

                        newDeatail.TheGWBS = oprNode;
                        newDeatail.TheGWBSSysCode = oprNode.SysCode;
                        //更新分包措施费率
                        if (item.ContentType == CostItemContentType.分包取费)
                        {
                            oprNode.SubContractFeeFlag = true;
                            newDeatail.SubContractFeeFlag = true;
                            newDeatail.SubContractStepRate = item.PricingRate;
                        }
                        else
                        {
                            newDeatail.SubContractFeeFlag = false;
                            newDeatail.SubContractStepRate = 0;
                        }
                        newDeatail.WorkAmountUnitGUID = item.ProjectUnitGUID;
                        newDeatail.WorkAmountUnitName = item.ProjectUnitName;
                        newDeatail.PriceUnitGUID = item.PriceUnitGUID;
                        newDeatail.PriceUnitName = item.PriceUnitName;
                        if (oprNode.ProjectTaskTypeGUID != null)
                        {
                            newDeatail.ProjectTaskTypeCode = oprNode.ProjectTaskTypeGUID.Code;
                        }
                        newDeatail.TheProjectGUID = oprNode.TheProjectGUID;
                        newDeatail.TheProjectName = oprNode.TheProjectName;
                        ContractGroup contract = txtContractGroupName.Tag as ContractGroup;
                        newDeatail.ContractGroupCode = contract.Code;
                        newDeatail.ContractGroupGUID = contract.Id;
                        newDeatail.ContractGroupName = contract.ContractName;
                        newDeatail.ContractGroupType = contract.ContractGroupType;
                        int maxOrderNo = 1;
                        newDeatail.Code = oprNode.Code + "-" + GetChildNodeMaxCode(oprNode, out maxOrderNo).PadLeft(5, '0');
                        newDeatail.OrderNo = maxOrderNo + 1; //oprNode.Details.Max(p => p.OrderNo) + 1;
                        newDeatail.State = DocumentState.Edit;
                        newDeatail.CurrentStateTime = model.GetServerTime();
                        newDeatail.TheCostItem = item;
                        newDeatail.TheCostItemCateSyscode = item.TheCostItemCateSyscode;

                        if (newDeatail.TheCostItem == null || newDeatail.TheCostItem.Id == item.Id)//如果未更改成本项，则更新资源耗用明细
                        {
                            foreach (SubjectCostQuota quota in curItemQuotas)
                            {
                                if (quota.MainResourceFlag && quota.ListResources.Count > 0)
                                {
                                    mainResource = quota.ListResources.ElementAt(0);
                                }

                                var query = from q in newDeatail.ListCostSubjectDetails
                                            where q.ResourceUsageQuota.Id == quota.Id
                                            select q;

                                if (query.Count() > 0)///如果大于0说明已经存在
                                {
                                    continue;
                                }
                                AddResourceUsageInTaskDetail(quota, newDeatail);
                            }
                        }
                        oprNode.Details.Add(newDeatail);
                    }

                    //计算项目任务的合价
                    GWBSTree tempNode = new GWBSTree();
                    tempNode.Id = oprNode.Id;
                    tempNode.SysCode = oprNode.SysCode;
                    tempNode = model.AccountTotalPrice(tempNode);

                    List<Decimal> listTotalPrice = StaticMethod.GetTaskDtlTotalPrice(oprNode);

                    oprNode.ContractTotalPrice = tempNode.ContractTotalPrice + listTotalPrice[0];
                    oprNode.ResponsibilityTotalPrice = tempNode.ResponsibilityTotalPrice + listTotalPrice[1];
                    oprNode.PlanTotalPrice = tempNode.PlanTotalPrice + listTotalPrice[2];

                    oprNode.ResponsibleAccFlag = true;
                    oprNode.CostAccFlag = true;
                    List<GWBSTree> listGWBS = new List<GWBSTree>();
                    listGWBS.Add(oprNode);

                    //保存，入库
                    var resultlst = model.SaveOrUpdateDetail(listGWBS, null, true);
                    var lstNode = resultlst[0] as IList;
                    oprNode = lstNode[0] as GWBSTree;
                    //将保存后的结果绑定到wbs树上
                    tvwCategory.SelectedNode.Tag = oprNode;
                    //
                    int index = gridGWBDetail.Rows.Count;
                    List<GWBSDetail> lstNewDetails = oprNode.Details.Where(p => lstDetailIdInDB.Any(m => p.Id == m) == false).ToList();
                    //将新增的明细添加到明细表格中
                    foreach (GWBSDetail item in lstNewDetails)
                    {
                        AddDetailInfoInGrid(item, false);
                    }
                    DataGridViewRow row = gridGWBDetail.Rows[index];
                    var rowDetail = row.Tag as GWBSDetail;
                    oprDtl = oprNode.Details.FirstOrDefault(p => p.Id == rowDetail.Id);
                    if (oprDtl != null)
                    {
                        ShownTaskDetailInfo(false);
                        row.Tag = oprDtl;
                    }
                    gridGWBDetail.CurrentCell = row.Cells[0];
                    //页面进入查看状态
                    RefreshDetailControls(MainViewState.Browser);
                    FlashScreen.Close();
                }
            }
            else
            {
                if (frm.SelectCostItem != null)
                {
                    #region
                    //if (txtDtlCostItem.Tag != null && txtDtlCostItem.Tag.GetType() == typeof(CostItem))
                    //{
                    //    //清掉上次选择的成本项生成的任务明细成本科目
                    //    for (int i = gridCostSubject.Rows.Count - 1; i > -1; i--)
                    //    {
                    //        DataGridViewRow row = gridCostSubject.Rows[i];

                    //        GWBSDetailCostSubject subject = row.Tag as GWBSDetailCostSubject;
                    //        if (string.IsNullOrEmpty(subject.Id) && (string.IsNullOrEmpty(subject.TheProjectGUID) || string.IsNullOrEmpty(subject.TheProjectName)))
                    //        {
                    //            gridCostSubject.Rows.RemoveAt(i);
                    //        }
                    //    }
                    //} 
                    #endregion
                    CostItem item = frm.SelectCostItem;
                    #region
                    //if (txtCostItemsZoning.Tag != null)
                    //{
                    //    CostItemsZoning z = txtCostItemsZoning.Tag as CostItemsZoning;
                    //    if (!item.TheCostItemCateSyscode.Contains(z.CostItemsCateSyscode))
                    //    {
                    //        MessageBox.Show("请选择地域【" + z.CostItemsCateName + "】下的成本项");
                    //        return;
                    //    }
                    //} 
                    #endregion
                    UpdateDtlCostItem(item, false);
                }
            }
        }
        private string GetChildNodeMaxCode(GWBSTree parentNode, out int maxOrderNo)
        {
            maxOrderNo = 1;
            int code = parentNode.Details.Count + 1;

            foreach (GWBSDetail dtl in parentNode.Details)
            {
                if (dtl.OrderNo >= maxOrderNo)
                {
                    maxOrderNo = dtl.OrderNo;
                }

                if (!string.IsNullOrEmpty(dtl.Code) && dtl.Code.IndexOf("-") > -1)
                {
                    int tempCode = 0;
                    if (Int32.TryParse(dtl.Code.Substring(dtl.Code.LastIndexOf("-") + 1), out tempCode))
                    {
                        if (tempCode >= code)
                        {
                            code = tempCode + 1;
                        }
                    }
                }
            }
            return code.ToString();
        }
        /// <summary>
        /// 获取资源耗用
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private IList GetListQuotaByCostItemID(List<CostItem> listCostItem)
        {
            ObjectQuery oqQuota = new ObjectQuery();
            Disjunction or = new Disjunction();
            foreach (var item in listCostItem)
            {
                or.Add(Expression.Eq("TheCostItem.Id", item.Id));
            }
            oqQuota.AddCriterion(or);
            oqQuota.AddFetchMode("CostAccountSubjectGUID", NHibernate.FetchMode.Eager);
            oqQuota.AddFetchMode("ProjectAmountUnitGUID", NHibernate.FetchMode.Eager);
            oqQuota.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
            oqQuota.AddFetchMode("ListResources", NHibernate.FetchMode.Eager);

            IList listQuota = model.ObjectQuery(typeof(SubjectCostQuota), oqQuota);
            return listQuota;
        }

        //选择主资源
        void btnSelectMainResourceType_Click(object sender, EventArgs e)
        {
            bool flag = false;

            if (oprDtl.ListCostSubjectDetails.Count == 0)
                flag = true;

            var queryMainUsage = from d in oprDtl.ListCostSubjectDetails
                                 where d.MainResTypeFlag == true
                                 select d;

            if (queryMainUsage.Count() == 0)
            {
                flag = true;
            }

            if (flag)
            {
                MessageBox.Show("当前任务明细下的资源耗用未设置主资源标记，请检查！");
                return;
            }

            GWBSDetailCostSubject dtlUsage = queryMainUsage.ElementAt(0);

            bool updateFlag = false;//更新标记

            if (dtlUsage.ResourceUsageQuota != null)//依据成本库里的定额过滤资源类型
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", dtlUsage.ResourceUsageQuota.Id));
                oq.AddFetchMode("ListResources", NHibernate.FetchMode.Eager);

                IList listQuota = model.ObjectQuery(typeof(SubjectCostQuota), oq);

                ISet<ResourceGroup> listResources = null;
                if (listQuota.Count > 0)
                {
                    listResources = (listQuota[0] as SubjectCostQuota).ListResources;

                    VSelectResourceTypeByUsage frm = new VSelectResourceTypeByUsage();
                    frm.ListResourceGroup = listResources.ToList();
                    frm.DefaultSelectedMaterilId = oprDtl.MainResourceTypeId;
                    frm.ShowDialog();

                    if (frm.SelectedMateril != null)
                    {
                        updateFlag = true;

                        Material mainMat = frm.SelectedMateril;

                        oprDtl.MainResourceTypeId = mainMat.Id;
                        oprDtl.MainResourceTypeName = mainMat.Name;
                        oprDtl.MainResourceTypeQuality = mainMat.Quality;
                        oprDtl.MainResourceTypeSpec = mainMat.Specification;


                        //给有这个资源类型的资源耗用更新主资源类型信息
                        dtlUsage.ResourceTypeGUID = mainMat.Id;
                        dtlUsage.ResourceTypeCode = mainMat.Code;
                        dtlUsage.ResourceTypeName = mainMat.Name;
                        dtlUsage.ResourceTypeQuality = mainMat.Quality;
                        dtlUsage.ResourceTypeSpec = mainMat.Specification;
                        dtlUsage.ResourceCateSyscode = mainMat.TheSyscode;
                    }
                }
            }
            else//成本项定额无资源组
            {
                CommonMaterial materialSelector = new CommonMaterial();
                materialSelector.OpenSelect();

                IList list = materialSelector.Result;
                if (list.Count > 0)
                {
                    updateFlag = true;

                    Material mainMat = list[0] as Material;
                    oprDtl.MainResourceTypeId = mainMat.Id;
                    oprDtl.MainResourceTypeName = mainMat.Name;
                    oprDtl.MainResourceTypeQuality = mainMat.Quality;
                    oprDtl.MainResourceTypeSpec = mainMat.Specification;
                }
            }

            if (updateFlag)
            {
                //if (!string.IsNullOrEmpty(oprDtl.Id))
                //    UpdateDetailInfoInGrid(oprDtl, true);

                string matStr = "";// string.IsNullOrEmpty(oprDtl.MainResourceTypeQuality) ? "" : oprDtl.MainResourceTypeQuality + ".";
                matStr += string.IsNullOrEmpty(oprDtl.MainResourceTypeSpec) ? "" : oprDtl.MainResourceTypeSpec + ".";
                matStr += oprDtl.MainResourceTypeName;

                txtMainResourceType.Text = matStr;
                txtMainResourceType.Tag = oprDtl.MainResourceTypeId;
            }
        }
        //修改明细
        void btnUpdateDetail_Click(object sender, EventArgs e)
        {
            if (gridGWBDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要修改的行！");
                return;
            }
            if (gridGWBDetail.SelectedRows.Count > 1)
            {
                MessageBox.Show("一次只能修改一行！");
                return;
            }

            oprDtl = gridGWBDetail.SelectedRows[0].Tag as GWBSDetail;
            if (oprDtl.State == VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Invalid)
            {
                MessageBox.Show("不能修改状态为‘" + StaticMethod.GetWBSTaskStateText(oprDtl.State) + "’的任务明细！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (oprDtl.CostingFlag == 0 && oprDtl.ProduceConfirmFlag == 1)
            {
                MessageBox.Show("生产明细不能修改！");
                return;
            }
            //if (!valideContractGroup())
            //{
            //    return;
            //}
            //oprTheCostItemId = oprDtl.TheCostItem.Id;
            //oprMainResourceTypeId = oprDtl.MainResourceTypeId;
            //oprDiagramNumber = oprDtl.DiagramNumber;
            IsCostItemSingleSelect = true;//修改时，只能修改一项，故为单选
            updateBeforeDtl = oprDtl;

            ShownTaskDetailInfo(true);

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", oprDtl.ContractGroupGUID));
            oq.AddFetchMode("ChangeContract", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof(ContractGroup), oq);
            if (list != null && list.Count > 0)
            {
                ContractGroup cg = list[0] as ContractGroup;
                txtChangeContractName.Text = cg.ContractName;
                txtChangeContractName.Tag = cg;
                txtChangeContractType.Text = cg.ContractGroupType;
            }

            RefreshDetailControls(MainViewState.Modify);
        }
        //发布明细
        void btnPublishDetail_Click(object sender, EventArgs e)
        {
            if (gridGWBDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要发布的明细行！");
                return;
            }
            IList list = new List<GWBSDetail>();

            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();

            foreach (DataGridViewRow row in gridGWBDetail.SelectedRows)
            {
                GWBSDetail cg = row.Tag as GWBSDetail;
                if (cg.State == VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit)
                {
                    dis.Add(Expression.Eq("Id", cg.Id));
                }
            }
            oq.AddCriterion(dis);
            oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("WorkAmountUnitGUID", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
            list = model.ObjectQuery(typeof(GWBSDetail), oq);

            if (list.Count == 0)
            {
                MessageBox.Show("选择中没有符合发布的明细，请选择状态为‘制定’的明细！");
                return;
            }

            #region 明细验重

            if (list.Count > 0)
            {
                #region 在选中明细中验重

                var listGroup = from g in list.OfType<GWBSDetail>()
                                group g by new { costItemId = g.TheCostItem.Id, g.MainResourceTypeId, g.DiagramNumber } into g
                                select new { g.Key.costItemId, g.Key.MainResourceTypeId, g.Key.DiagramNumber, count = g.Count() };

                listGroup = from d in listGroup
                            where d.count > 1
                            select d;

                if (listGroup.Count() > 0)
                {
                    MessageBox.Show("选择发布的任务明细中存在相同成本项、主资源类型、图号的任务明细，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                #endregion

                #region 在已发布明细中验重
                foreach (GWBSDetail item in list)
                {
                    if (item.ProduceConfirmFlag == 1)
                    {
                        if (string.IsNullOrEmpty(item.MainResourceTypeId))
                            item.MainResourceTypeId = null;
                        if (string.IsNullOrEmpty(item.DiagramNumber))
                            item.DiagramNumber = null;

                        foreach (DataGridViewRow row in gridGWBDetail.Rows)
                        {
                            GWBSDetail d = row.Tag as GWBSDetail;
                            if (d.Id != item.Id && d.State == VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute && d.ProduceConfirmFlag == 1
                                && item.TheCostItem.Id == d.TheCostItem.Id && item.MainResourceTypeId == d.MainResourceTypeId && item.DiagramNumber == d.DiagramNumber)
                            {
                                MessageBox.Show("当前节点下已存在与任务明细“" + item.Name + "”的成本项、主资源类型、图号相同的“生产确认明细”，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                }

                var listCostDtl = from g in list.OfType<GWBSDetail>()
                                  where g.CostingFlag == 1
                                  select g;

                if (listCostDtl.Count() > 0)
                {
                    string errMsg = model.CheckGWBSDetail(listCostDtl.ToList());

                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        MessageBox.Show(errMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                #endregion
            }
            #endregion

            try
            {
                List<GWBSDetailLedger> listLedger = new List<GWBSDetailLedger>();

                for (int i = 0; i < list.Count; i++)
                {
                    GWBSDetail dtl = list[i] as GWBSDetail;
                    dtl.State = VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute;

                    #region 记录明细台账
                    GWBSDetailLedger led = new GWBSDetailLedger();

                    led.ProjectTaskID = dtl.TheGWBS.Id;
                    led.ProjectTaskName = dtl.TheGWBS.Name;
                    led.TheProjectTaskSysCode = dtl.TheGWBS.SysCode;

                    led.ProjectTaskDtlID = dtl.Id;
                    led.ProjectTaskDtlName = dtl.Name;

                    led.ContractWorkAmount = dtl.ContractProjectQuantity;
                    led.ContractPrice = dtl.ContractPrice;
                    led.ContractTotalPrice = dtl.ContractTotalPrice;

                    led.ResponsiblePrice = dtl.ResponsibilitilyPrice;
                    led.ResponsibleWorkAmount = dtl.ResponsibilitilyWorkAmount;
                    led.ResponsibleTotalPrice = dtl.ResponsibilitilyTotalPrice;

                    led.PlanPrice = dtl.PlanPrice;
                    led.PlanWorkAmount = dtl.PlanWorkAmount;
                    led.PlanTotalPrice = dtl.PlanTotalPrice;

                    led.WorkAmountUnit = dtl.WorkAmountUnitGUID;
                    led.WorkAmountUnitName = dtl.WorkAmountUnitName;

                    led.PriceUnit = dtl.PriceUnitGUID;
                    led.PriceUnitName = dtl.PriceUnitName;

                    led.TheContractGroup = model.GetObjectById(typeof(ContractGroup), dtl.ContractGroupGUID) as ContractGroup;

                    led.TheProjectGUID = dtl.TheProjectGUID;
                    led.TheProjectName = dtl.TheProjectName;

                    listLedger.Add(led);

                    #endregion
                }

                IList listResult = model.SaveOrUpdateDetail(list, null, listLedger);
                list = listResult[0] as IList;

                foreach (GWBSDetail tempDtl in list)
                {
                    foreach (DataGridViewRow row in gridGWBDetail.Rows)
                    {
                        GWBSDetail dtl = row.Tag as GWBSDetail;

                        if (tempDtl.Id == dtl.Id)
                        {
                            row.Tag = tempDtl;
                            row.Cells["DtlState"].Value = StaticMethod.GetWBSTaskStateText(tempDtl.State);
                            break;
                        }
                    }
                }

                RefreshDetailControls(MainViewState.Browser);

                MessageBox.Show("发布成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("发布失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        //删除/作废明细 
        void btnDeleteDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridGWBDetail.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请选择要删除或作废的行！");
                    gridGWBDetail.Focus();
                    return;
                }

                List<GWBSDetail> list = new List<GWBSDetail>();

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", oprNode.Id));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("ProjectTaskTypeGUID", NHibernate.FetchMode.Eager);
                oprNode = model.ObjectQuery(typeof(GWBSTree), oq)[0] as GWBSTree;
                List<int> listRowIndex = new List<int>();
                foreach (DataGridViewRow row in gridGWBDetail.SelectedRows)
                {
                    GWBSDetail item = row.Tag as GWBSDetail;

                    var queryDtl = from d in oprNode.Details
                                   where d.Id == item.Id && (d.State == VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit || d.State == VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute || d.State == 0)
                                   select d;

                    if (queryDtl.Count() > 0)
                    {
                        GWBSDetail dtl = queryDtl.ElementAt(0) as GWBSDetail;
                        list.Add(dtl);

                        listRowIndex.Add(row.Index);
                    }
                }

                if (list.Count == 0)
                {
                    MessageBox.Show("选择行中没有符合删除或作废的记录，请选择状态为‘编制’或‘执行中’的任务明细！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("删除或作废后不能恢复，您确认要操作选中任务明细吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                List<GWBSDetail> listInvalid = new List<GWBSDetail>();
                for (int i = list.Count - 1; i > -1; i--)
                {
                    GWBSDetail dtl = list[i] as GWBSDetail;
                    if (dtl.State == VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit)
                    {
                        oprNode.Details.Remove(dtl);
                    }
                    else if (dtl.State == DocumentState.InExecute)
                    {
                        listInvalid.Add(dtl);
                        dtl.State = DocumentState.Invalid;
                    }
                }
                #region 该段代码主要判断作废的任务明细下是否有任务确认单或者是否进行核算
                //if (listInvalid != null && listInvalid.Count > 0)
                //{
                //    string errorMess = model.DeleteGWBSDetailBeforeOperat(listInvalid, oprNode);
                //    if (errorMess != "")
                //    {
                //        MessageBox.Show(errorMess);
                //        return;
                //    }
                //}
                #endregion
                //根据明细设置任务对象的标记
                bool taskResponsibleFlag = false;
                bool taskCostAccFlag = false;
                bool taskProductConfirmFlag = false;
                bool taskSubContractFeeFlag = false;

                foreach (GWBSDetail dtl in oprNode.Details)
                {
                    if (dtl.State != VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Invalid)
                    {
                        if (dtl.ResponseFlag == 1)
                            taskResponsibleFlag = true;
                        if (dtl.CostingFlag == 1)
                            taskCostAccFlag = true;
                        if (dtl.ProduceConfirmFlag == 1)
                            taskProductConfirmFlag = true;
                        if (dtl.SubContractFeeFlag)
                            taskSubContractFeeFlag = true;
                    }
                }
                oprNode.ResponsibleAccFlag = taskResponsibleFlag;
                oprNode.CostAccFlag = taskCostAccFlag;
                oprNode.ProductConfirmFlag = taskProductConfirmFlag;
                oprNode.SubContractFeeFlag = taskSubContractFeeFlag;


                //计算项目任务的合价
                GWBSTree tempNode = new GWBSTree();
                tempNode.Id = oprNode.Id;
                tempNode.SysCode = oprNode.SysCode;
                tempNode = model.AccountTotalPriceByChilds(tempNode);//计算下属节点的任务明细数据

                List<Decimal> listTotalPrice = StaticMethod.GetTaskDtlTotalPrice(oprNode);//计算自己的任务明细数据

                oprNode.ContractTotalPrice = tempNode.ContractTotalPrice + listTotalPrice[0];
                oprNode.ResponsibilityTotalPrice = tempNode.ResponsibilityTotalPrice + listTotalPrice[1];
                oprNode.PlanTotalPrice = tempNode.PlanTotalPrice + listTotalPrice[2];


                //oprNode = model.SaveOrUpdateGWBSTree(oprNode, null);
                oprNode = model.SaveOrUpdateGWBSTree(oprNode, listInvalid);
                oprDtl = null;


                //更新显示任务节点的合同合价、责任合价、计划合价信息
                txtContractTotalPrice.Text = oprNode.ContractTotalPrice.ToString();
                txtResponsibilityTotalPrice.Text = oprNode.ResponsibilityTotalPrice.ToString();
                txtPlanTotalPrice.Text = oprNode.PlanTotalPrice.ToString();


                //清除界面显示信息
                listRowIndex.Sort();
                for (int i = listRowIndex.Count - 1; i > -1; i--)
                {
                    int rowIndex = listRowIndex[i];
                    GWBSDetail dtl = gridGWBDetail.Rows[rowIndex].Tag as GWBSDetail;
                    if (dtl.State == DocumentState.Edit)
                        gridGWBDetail.Rows.RemoveAt(rowIndex);
                    else
                    {
                        var query = from g in oprNode.Details
                                    where g.Id == dtl.Id
                                    select g;
                        GWBSDetail dtlUpdate = query.ElementAt(0);
                        DataGridViewRow row = gridGWBDetail.Rows[rowIndex];
                        row.Cells[DtlState.Name].Value = StaticMethod.GetWBSTaskStateText(dtlUpdate.State);
                        row.Tag = dtlUpdate;
                    }
                }
                gridGWBDetail.ClearSelection();

                ClearDetailAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        //终止(作废)明细
        void btnCancellationDetail_Click(object sender, EventArgs e)
        {
            if (gridGWBDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要终止的任务明细！");
                return;
            }

            IList list = new List<GWBSDetail>();
            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridGWBDetail.SelectedRows)
            {
                GWBSDetail cg = row.Tag as GWBSDetail;
                if (cg.State == VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute)
                {
                    GWBSDetail temp = model.GetObjectById(typeof(GWBSDetail), cg.Id) as GWBSDetail;
                    temp.State = VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Invalid;
                    list.Add(temp);

                    listRowIndex.Add(row.Index);
                }
            }
            if (list.Count == 0)
            {
                MessageBox.Show("选择中没有符合终止的任务明细，请选择状态为‘有效’的任务明细！");
                return;
            }

            if (!valideContractGroup())
                return;

            try
            {


                List<GWBSDetailLedger> listLedger = new List<GWBSDetailLedger>();

                ContractGroup theContract = txtContractGroupName.Tag as ContractGroup;
                foreach (GWBSDetail dtl in list)
                {
                    #region 记录明细台账
                    GWBSDetailLedger led = new GWBSDetailLedger();

                    led.ProjectTaskID = dtl.TheGWBS.Id;
                    led.ProjectTaskName = dtl.TheGWBS.Name;
                    led.TheProjectTaskSysCode = dtl.TheGWBS.SysCode;

                    led.ProjectTaskDtlID = dtl.Id;
                    led.ProjectTaskDtlName = dtl.Name;

                    led.ContractChangeMode = ContractIncomeChangeModeEnum.合同收入工程量变化;
                    led.ContractWorkAmount = (dtl.AddupAccFigureProgress - 1) - dtl.ContractProjectQuantity;
                    led.ContractPrice = dtl.ContractPrice;
                    led.ContractTotalPrice = dtl.ContractTotalPrice;

                    led.ResponsibleCostChangeMode = ResponsibleCostChangeModeEnum.责任工程量变化;
                    led.ResponsibleWorkAmount = (dtl.AddupAccFigureProgress - 1) - dtl.ResponsibilitilyWorkAmount;
                    led.ResponsiblePrice = dtl.ResponsibilitilyPrice;
                    led.ResponsibleTotalPrice = dtl.ResponsibilitilyTotalPrice;

                    led.PlanCostChangeMode = PlanCostChangeModeEnum.计划工程量变化;
                    led.PlanWorkAmount = (dtl.AddupAccFigureProgress - 1) - dtl.PlanWorkAmount;
                    led.PlanPrice = dtl.PlanPrice;
                    led.PlanTotalPrice = dtl.PlanTotalPrice;

                    led.WorkAmountUnit = dtl.WorkAmountUnitGUID;
                    led.WorkAmountUnitName = dtl.WorkAmountUnitName;

                    led.PriceUnit = dtl.PriceUnitGUID;
                    led.PriceUnitName = dtl.PriceUnitName;

                    led.TheContractGroup = theContract;

                    led.TheProjectGUID = dtl.TheProjectGUID;
                    led.TheProjectName = dtl.TheProjectName;

                    listLedger.Add(led);

                    #endregion
                }

                IList listResult = model.SaveOrUpdateDetail(list, null, listLedger);
                list = listResult[0] as IList;

                for (int i = 0; i < listRowIndex.Count; i++)
                {
                    int rowIndex = listRowIndex[i];
                    GWBSDetail temp = list[i] as GWBSDetail;
                    if (temp.Id == oprDtl.Id)
                        oprDtl = temp;
                    gridGWBDetail.Rows[rowIndex].Tag = temp;
                    gridGWBDetail.Rows[rowIndex].Cells["DtlState"].Value = StaticMethod.GetWBSTaskStateText(temp.State);
                }

                RefreshControls(MainViewState.Browser);

                MessageBox.Show("设置成功！");

            }
            catch (Exception ex)
            {
                MessageBox.Show("设置失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        //保存明细
        void btnSaveTaskDetail_Click(object sender, EventArgs e)
        {
            if (!SaveView())
                return;
            RefreshDetailControls(MainViewState.Browser);
        }
        //保存明细
        void btnSave_Click(object sender, EventArgs e)
        {
            if (!SaveView())
                return;

            //GetTaskNodeDetailInfo();//刷新契约组信息

            RefreshDetailControls(MainViewState.Browser);
        }
        //任务变更 量价
        void btnChangeTaskDetail_Click(object sender, EventArgs e)
        {
            if (gridGWBDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要变更的明细行！");
                return;
            }
            IList list = new List<GWBSDetail>();

            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (DataGridViewRow row in gridGWBDetail.SelectedRows)
            {
                GWBSDetail cg = row.Tag as GWBSDetail;
                if (cg.State == VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute && cg.CostingFlag == 1)
                {
                    dis.Add(Expression.Eq("Id", cg.Id));
                }
            }
            oq.AddCriterion(dis);

            list = model.ObjectQuery(typeof(GWBSDetail), oq);

            if (list.Count == 0)
            {
                MessageBox.Show("选择中没有符合变更操作的明细，请选择状态为‘" + StaticMethod.GetWBSTaskStateText(DocumentState.InExecute) + "’的‘成本核算明细’！");
                return;
            }

            try
            {

                VGWBSDetailChange frm = new VGWBSDetailChange();
                frm.listSelectedDtl = list;
                frm.DefaultContract = txtContractGroupName.Tag as ContractGroup;
                frm.ShowDialog();

                List<GWBSDetail> listChangeDtl = frm.listChangeDtl;

                if (listChangeDtl.Count > 0)
                {
                    foreach (GWBSDetail tempDtl in listChangeDtl)
                    {
                        foreach (DataGridViewRow row in gridGWBDetail.Rows)
                        {
                            GWBSDetail dtl = row.Tag as GWBSDetail;

                            if (tempDtl.Id == dtl.Id)
                            {
                                row.Tag = tempDtl;
                                row.Cells[DtlName.Name].Value = tempDtl.Name.ToString();
                                row.Cells[DtlState.Name].Value = StaticMethod.GetWBSTaskStateText(tempDtl.State);
                                row.Cells[DtlPlanQuantity.Name].Value = tempDtl.PlanWorkAmount;
                                row.Cells[DtlPlanPrice.Name].Value = tempDtl.PlanPrice;
                                row.Cells[DtlPlanTotalPrice.Name].Value = tempDtl.PlanTotalPrice;
                                break;
                            }
                        }
                    }

                    if (oprDtl != null)
                    {
                        var query = from d in listChangeDtl
                                    where d.Id == oprDtl.Id
                                    select d;
                        if (query.Count() > 0)
                        {
                            oprDtl = query.ElementAt(0);

                            ShownTaskDetailInfo(true);
                        }
                    }

                    #region  刷新gwbs节点的计划合价

                    GWBSTree tempNode = model.GetObjectById(typeof(GWBSTree), oprNode.Id) as GWBSTree;

                    ShownTaskNodeTotalPriceInfo();

                    #endregion

                    RefreshDetailControls(MainViewState.Browser);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("变更操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        //任务拆除
        void btnTaskBackout_Click(object sender, EventArgs e)
        {
            if (gridGWBDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择一条要拆除的任务明细！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (gridGWBDetail.SelectedRows.Count > 1)
            {
                MessageBox.Show("一次只能操作一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            oprDtl = gridGWBDetail.SelectedRows[0].Tag as GWBSDetail;

            IsAddDetail = true;

            //在节点选择的时候有对象关联查询，但偶尔会有丢失延迟加载的对象，根据需要重新加载
            try
            {
                //任务明细需要存储任务类型代码
                if (oprNode.ProjectTaskTypeGUID != null)
                {
                    string name = oprNode.ProjectTaskTypeGUID.Name;
                }
            }
            catch
            {
                oprNode = LoadRelaPBS(oprNode);
            }

            try
            {
                FlashScreen.Show("正在生成拆除成本项,请稍候......");

                oprDtl = CopyDetailInfo(oprDtl);

                ShownTaskDetailInfo(false);

                btnAccountCostData_Click(btnAccountCostData, new EventArgs());

                tabBaseInfo.SelectedIndex = 0;
                txtDtlCostItem.Focus();

                RefreshDetailControls(MainViewState.Modify);

                FlashScreen.Close();

            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("生成成本项失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                FlashScreen.Close();
            }
        }
        //导出MPP
        void btnExportMPP_Click(object sender, EventArgs e)
        {

        }
        //导出Excel
        void btnExportExcel_Click(object sender, EventArgs e)
        {

        }
        //选择契约组
        void btnSelectContractGroup_Click(object sender, EventArgs e)
        {
            VSelectWBSContractGroup frm = new VSelectWBSContractGroup(new MWBSContractGroup(), false);
            if (txtContractGroupName.Tag != null)
            {
                frm.DefaultSelectedContract = txtContractGroupName.Tag as ContractGroup;
            }
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                ContractGroup cg = frm.SelectResult[0] as ContractGroup;
                optContract = cg;
                txtContractGroupName.Text = cg.ContractName;
                txtContractGroupType.Text = cg.ContractGroupType;
                txtContractGroupDesc.Text = cg.ContractDesc;
                txtContractGroupName.Tag = cg;

                if (oprDtl != null && btnSaveBaseInfo.Enabled && cbUpdateContract.Checked)
                {
                    txtDtlContractGroupName.Text = cg.ContractName;
                    txtDtlContractGroupType.Text = cg.ContractGroupType;
                }
            }
        }

        void txtDtlCostItem_LostFocus(object sender, EventArgs e)
        {
            //if (txtDtlCostItem.ReadOnly || !txtDtlCostItem.Enabled)
            //    return;

            //string costItemCode = txtDtlCostItem.Text.Trim();
            //if (string.IsNullOrEmpty(costItemCode))
            //{
            //    //MessageBox.Show("请输入成本项编号或定额编号，或选择一个成本项！");

            //    txtDtlCostItem.Tag = null;

            //    txtMainResourceType.Text = "";
            //    txtMainResourceType.Tag = null;

            //    return;
            //}
            //costItemCode = costItemCode.ToUpper();

            ////else if (txtDtlCostItem.Tag != null)
            ////{
            ////    CostItem selItem = txtDtlCostItem.Tag as CostItem;
            ////    if (selItem != null && (selItem.Code == costItemCode || selItem.QuotaCode == costItemCode))
            ////    {
            ////        return;
            ////    }
            ////}

            //ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Or(Expression.Eq("Code", costItemCode), Expression.Eq("QuotaCode", costItemCode)));
            //oq.AddCriterion(Expression.Eq("ItemState", CostItemState.发布));
            //IList list = model.ObjectQuery(typeof(CostItem), oq);
            //if (list != null && list.Count > 0)
            //{
            //    CostItem item = list[0] as CostItem;
            //    oq.Criterions.Clear();
            //    oq.AddCriterion(Expression.Eq("Id", item.Id));
            //    oq.AddFetchMode("ListQuotas", NHibernate.FetchMode.Eager);
            //    item = model.ObjectQuery(typeof(CostItem), oq)[0] as CostItem;

            //    //清掉上次选择的成本项生成的任务明细成本科目
            //    //for (int i = gridCostSubject.Rows.Count - 1; i > -1; i--)
            //    //{
            //    //    DataGridViewRow row = gridCostSubject.Rows[i];

            //    //    GWBSDetailCostSubject subject = row.Tag as GWBSDetailCostSubject;
            //    //    if (string.IsNullOrEmpty(subject.Id) && (string.IsNullOrEmpty(subject.TheProjectGUID) || string.IsNullOrEmpty(subject.TheProjectName)))
            //    //    {
            //    //        gridCostSubject.Rows.RemoveAt(i);
            //    //    }
            //    //}

            //    ObjectQuery oqQuota = new ObjectQuery();
            //    Disjunction disQuota = new Disjunction();
            //    foreach (SubjectCostQuota quota in item.ListQuotas)
            //    {
            //        disQuota.Add(Expression.Eq("Id", quota.Id));
            //    }
            //    oqQuota.AddCriterion(disQuota);
            //    oqQuota.AddCriterion(Expression.Eq("State", SubjectCostQuotaState.生效));
            //    oqQuota.AddFetchMode("CostAccountSubjectGUID", NHibernate.FetchMode.Eager);
            //    oqQuota.AddFetchMode("ProjectAmountUnitGUID", NHibernate.FetchMode.Eager);
            //    oqQuota.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
            //    oqQuota.AddFetchMode("ListResources", NHibernate.FetchMode.Eager);

            //    IList listQuota = model.ObjectQuery(typeof(SubjectCostQuota), oqQuota);

            //    ResourceGroup mainResource = null;

            //    if (oprDtl.TheCostItem == null || oprDtl.TheCostItem.Id == item.Id)//如果未更改成本项，则更新资源耗用明细
            //    {
            //        foreach (SubjectCostQuota quota in listQuota)
            //        {
            //            if (quota.MainResourceFlag && quota.ListResources.Count > 0)
            //            {
            //                mainResource = quota.ListResources.ElementAt(0);
            //            }

            //            var query = from q in oprDtl.ListCostSubjectDetails
            //                        where q.ResourceUsageQuota.Id == quota.Id
            //                        select q;

            //            if (query.Count() > 0)
            //            {
            //                continue;
            //            }

            //            AddResourceUsageInTaskDetail(quota);

            //            //AddCostSubjectInfoInGrid(subject);
            //        }
            //    }
            //    else//如果更改成本项，则重新添加资源耗用明细
            //    {
            //        if (MessageBox.Show("您更改了成本项，任务明细的资源耗用将根据新的成本项重新生成，您确认要执行此操作吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //        {
            //            txtDtlCostItem.Text = oprDtl.TheCostItem.Code;
            //            return;
            //        }

            //        oprDtl.ListCostSubjectDetails.Clear();
            //        foreach (SubjectCostQuota quota in listQuota)
            //        {
            //            if (quota.MainResourceFlag && quota.ListResources.Count > 0)
            //            {
            //                mainResource = quota.ListResources.ElementAt(0);
            //            }

            //            AddResourceUsageInTaskDetail(quota);

            //            //AddCostSubjectInfoInGrid(subject);
            //        }
            //    }

            //    oprDtl.TheCostItem = item;

            //    oprDtl.WorkAmountUnitGUID = item.ProjectUnitGUID;
            //    oprDtl.WorkAmountUnitName = item.ProjectUnitName;
            //    oprDtl.PriceUnitGUID = item.PriceUnitGUID;
            //    oprDtl.PriceUnitName = item.PriceUnitName;

            //    txtDtlCostItem.Tag = item;

            //    txtBudgetProjectUnit.Tag = oprDtl.WorkAmountUnitGUID;
            //    txtBudgetProjectUnit.Text = oprDtl.WorkAmountUnitName;

            //    txtBudgetPriceUnit.Tag = oprDtl.PriceUnitGUID;
            //    txtBudgetPriceUnit.Text = oprDtl.PriceUnitName;

            //    txtAddupAccProQuantityUnit.Text = oprDtl.WorkAmountUnitName;
            //    txtAddupConfirmProQnyUnit.Text = oprDtl.WorkAmountUnitName;


            //    if (mainResource != null)
            //    {
            //        string matStr = string.IsNullOrEmpty(mainResource.ResourceTypeQuality) ? "" : mainResource.ResourceTypeQuality + ".";
            //        matStr += string.IsNullOrEmpty(mainResource.ResourceTypeSpec) ? "" : mainResource.ResourceTypeSpec + ".";
            //        matStr += mainResource.ResourceTypeName;

            //        txtMainResourceType.Text = matStr;
            //        txtMainResourceType.Tag = mainResource;
            //    }
            //    else
            //    {
            //        txtMainResourceType.Text = "";
            //        txtMainResourceType.Tag = null;
            //    }
            //}
            //else
            //{
            //    txtDtlCostItem.Tag = null;
            //    MessageBox.Show("请检查输入的成本项编号或定额编号，未找到相关成本项！");
            //}
        }

        void txtDtlCostItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtDtlCostItem.ReadOnly || !txtDtlCostItem.Enabled)
                return;

            if (e.KeyCode != Keys.Enter)
                return;

            string costItemCode = txtDtlCostItem.Text.Trim();
            if (string.IsNullOrEmpty(costItemCode))
            {
                MessageBox.Show("请输入成本项编号或定额编号，或选择一个成本项！");

                txtDtlCostItem.Tag = null;

                txtMainResourceType.Text = "";
                txtMainResourceType.Tag = null;

                return;
            }
            costItemCode = costItemCode.ToUpper();

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Or(Expression.Eq("Code", costItemCode), Expression.Eq("QuotaCode", costItemCode)));
            oq.AddCriterion(Expression.Eq("ItemState", CostItemState.发布));
            //oq.AddFetchMode("ListQuotas", NHibernate.FetchMode.Eager);
            if (txtCostItemsZoning.Tag != null)
            {
                CostItemsZoning z = txtCostItemsZoning.Tag as CostItemsZoning;
                oq.AddCriterion(Expression.Like("TheCostItemCateSyscode", z.CostItemsCateSyscode, MatchMode.Start));
            }
            oq.AddFetchMode("ProjectUnitGUID", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);

            IList list = model.ObjectQuery(typeof(CostItem), oq);
            if (list != null && list.Count == 1)
            {
                CostItem item = list[0] as CostItem;
                UpdateDtlCostItem(item, true);
            }
            else if (list != null && list.Count > 1)
            {
                if (txtCostItemsZoning.Tag != null)
                {
                    CostItem item = list[0] as CostItem;
                    UpdateDtlCostItem(item, true);
                }
                else
                {
                    txtDtlCostItem.Tag = null;
                    MessageBox.Show("检查输入的成本项编号或定额编号，找到相关成本项有多个，请选择地域！");
                }
            }
            else
            {
                txtDtlCostItem.Tag = null;
                MessageBox.Show("请检查输入的成本项编号或定额编号，未找到相关成本项！");
            }
        }

        /// <summary>
        /// 修改成本项
        /// </summary>
        /// <param name="item">修改的成本项</param>
        /// <param name="isKeyOpt">是否是键盘操作</param>
        /// <returns></returns>
        private bool UpdateDtlCostItem(CostItem item, bool isKeyOpt)
        {

            ObjectQuery oqQuota = new ObjectQuery();
            //Disjunction disQuota = new Disjunction();
            //foreach (SubjectCostQuota quota in item.ListQuotas)
            //{
            //    disQuota.Add(Expression.Eq("Id", quota.Id));
            //}
            //oqQuota.AddCriterion(disQuota);


            oqQuota.AddCriterion(Expression.Eq("TheCostItem.Id", item.Id));
            oqQuota.AddCriterion(Expression.Eq("State", SubjectCostQuotaState.生效));
            oqQuota.AddFetchMode("CostAccountSubjectGUID", NHibernate.FetchMode.Eager);
            oqQuota.AddFetchMode("ProjectAmountUnitGUID", NHibernate.FetchMode.Eager);
            oqQuota.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
            oqQuota.AddFetchMode("ListResources", NHibernate.FetchMode.Eager);

            IList listQuota = model.ObjectQuery(typeof(SubjectCostQuota), oqQuota);

            ResourceGroup mainResource = null;

            if (oprDtl.TheCostItem == null || oprDtl.TheCostItem.Id == item.Id)//如果未更改成本项，则更新资源耗用明细
            {
                foreach (SubjectCostQuota quota in listQuota)
                {
                    if (quota.MainResourceFlag && quota.ListResources.Count > 0)
                    {
                        mainResource = quota.ListResources.ElementAt(0);
                    }

                    var query = from q in oprDtl.ListCostSubjectDetails
                                where q.ResourceUsageQuota.Id == quota.Id
                                select q;

                    if (query.Count() > 0)
                    {
                        continue;
                    }
                    AddResourceUsageInTaskDetail(quota);

                    //AddCostSubjectInfoInGrid(subject);
                }
            }
            else//如果更改成本项，则重新添加资源耗用明细
            {
                if (MessageBox.Show("您更改了成本项，任务明细的资源耗用将根据新的成本项重新生成，您确认要执行此操作吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    txtDtlCostItem.Text = oprDtl.TheCostItem.QuotaCode;
                    return false;
                }
                oprDtl.ListCostSubjectDetails.Clear();
                foreach (SubjectCostQuota quota in listQuota)
                {
                    if (quota.MainResourceFlag && quota.ListResources.Count > 0)
                    {
                        mainResource = quota.ListResources.ElementAt(0);
                    }
                    AddResourceUsageInTaskDetail(quota);

                    //AddCostSubjectInfoInGrid(subject);
                }


            }

            //更新分包措施费率
            if (item.ContentType == CostItemContentType.分包取费)
            {
                oprDtl.SubContractFeeFlag = true;
                oprDtl.SubContractStepRate = item.PricingRate;
            }
            else
            {
                oprDtl.SubContractFeeFlag = false;
                oprDtl.SubContractStepRate = 0;
            }

            oprDtl.TheCostItem = item;
            oprDtl.TheCostItemCateSyscode = item.TheCostItemCateSyscode;

            oprDtl.WorkAmountUnitGUID = item.ProjectUnitGUID;
            oprDtl.WorkAmountUnitName = item.ProjectUnitName;
            oprDtl.PriceUnitGUID = item.PriceUnitGUID;
            oprDtl.PriceUnitName = item.PriceUnitName;

            if (isKeyOpt == false)
            {
                if (!string.IsNullOrEmpty(item.QuotaCode))
                    txtDtlCostItem.Text = item.QuotaCode;
                else
                    txtDtlCostItem.Text = item.Name;
            }
            txtDtlCostItem.Tag = item;

            txtDtlName.Text = item.Name;

            txtBudgetProjectUnit.Tag = oprDtl.WorkAmountUnitGUID;
            txtBudgetProjectUnit.Text = oprDtl.WorkAmountUnitName;

            txtBudgetPriceUnit.Tag = oprDtl.PriceUnitGUID;
            txtBudgetPriceUnit.Text = oprDtl.PriceUnitName;

            txtAddupAccProQuantityUnit.Text = oprDtl.WorkAmountUnitName;
            txtAddupConfirmProQnyUnit.Text = oprDtl.WorkAmountUnitName;


            if (mainResource != null)
            {
                string matStr = string.IsNullOrEmpty(mainResource.ResourceTypeQuality) ? "" : mainResource.ResourceTypeQuality + ".";
                matStr += string.IsNullOrEmpty(mainResource.ResourceTypeSpec) ? "" : mainResource.ResourceTypeSpec + ".";
                matStr += mainResource.ResourceTypeName;

                txtMainResourceType.Text = matStr;
                txtMainResourceType.Tag = mainResource;
                txtDrawNumber.Text = mainResource.DiagramNumber;
            }
            else
            {
                txtMainResourceType.Text = "";
                txtMainResourceType.Tag = null;
            }

            //显示分包措施信息
            cbSubContractFee.Checked = oprDtl.SubContractFeeFlag;
            txtSubcontractFee.Text = StaticMethod.DecimelTrimEnd0(oprDtl.SubContractStepRate);
            //重新计算合同、责任、计划单价、合价
            btnAccountCostData_Click(btnAccountCostData, new EventArgs());

            return true;
        }

        /// <summary>
        /// 添加资源耗用到工程任务明细
        /// </summary>
        /// <param name="quota"></param>
        private void AddResourceUsageInTaskDetail(SubjectCostQuota quota)
        {
            GWBSDetailCostSubject subject = new GWBSDetailCostSubject();
            subject.Name = quota.Name;
            subject.MainResTypeFlag = quota.MainResourceFlag;

            subject.ProjectAmountUnitGUID = quota.ProjectAmountUnitGUID;
            subject.ProjectAmountUnitName = quota.ProjectAmountUnitName;

            subject.PriceUnitGUID = quota.PriceUnitGUID;
            subject.PriceUnitName = quota.PriceUnitName;

            //合同
            subject.ContractQuotaQuantity = quota.QuotaProjectAmount;

            subject.ContractBasePrice = quota.QuotaPrice;
            subject.ContractPricePercent = 1;
            subject.ContractQuantityPrice = subject.ContractBasePrice * subject.ContractPricePercent;

            subject.ContractPrice = subject.ContractQuotaQuantity * subject.ContractQuantityPrice;
            subject.ContractProjectAmount = oprDtl.ContractProjectQuantity * subject.ContractQuotaQuantity;
            subject.ContractTotalPrice = subject.ContractProjectAmount * subject.ContractQuantityPrice;

            //责任
            subject.ResponsibleQuotaNum = quota.QuotaProjectAmount;

            subject.ResponsibleBasePrice = quota.QuotaPrice;
            subject.ResponsiblePricePercent = 1;
            subject.ResponsibilitilyPrice = subject.ResponsibleBasePrice * subject.ResponsiblePricePercent;

            subject.ResponsibleWorkPrice = subject.ResponsibleQuotaNum * subject.ResponsibilitilyPrice;
            subject.ResponsibilitilyWorkAmount = oprDtl.ResponsibilitilyWorkAmount * subject.ResponsibleQuotaNum;
            subject.ResponsibilitilyTotalPrice = subject.ResponsibilitilyWorkAmount * subject.ResponsibilitilyPrice;//责任包干单价未知

            //计划
            subject.PlanQuotaNum = quota.QuotaProjectAmount;

            subject.PlanBasePrice = quota.QuotaPrice;
            subject.PlanPricePercent = 1;
            subject.PlanPrice = subject.PlanBasePrice * subject.PlanPricePercent;

            subject.PlanWorkPrice = subject.PlanQuotaNum * subject.PlanPrice;
            subject.PlanWorkAmount = oprDtl.PlanWorkAmount * subject.PlanQuotaNum;
            subject.PlanTotalPrice = subject.PlanWorkAmount * subject.PlanPrice;//计划包干单价未知

            subject.AssessmentRate = quota.AssessmentRate;

            subject.ResourceUsageQuota = quota;

            if (quota.ListResources.Count > 0)
            {
                ResourceGroup itemResource = quota.ListResources.ElementAt(0);
                subject.ResourceTypeGUID = itemResource.ResourceTypeGUID;
                subject.ResourceTypeCode = itemResource.ResourceTypeCode;
                subject.ResourceTypeName = itemResource.ResourceTypeName;
                subject.ResourceTypeQuality = itemResource.ResourceTypeQuality;
                subject.ResourceTypeSpec = itemResource.ResourceTypeSpec;
                subject.ResourceCateSyscode = itemResource.ResourceCateSyscode;
                subject.DiagramNumber = itemResource.DiagramNumber;
            }

            subject.CostAccountSubjectGUID = quota.CostAccountSubjectGUID;
            subject.CostAccountSubjectName = quota.CostAccountSubjectName;
            if (quota.CostAccountSubjectGUID != null)
                subject.CostAccountSubjectSyscode = quota.CostAccountSubjectGUID.SysCode;

            subject.ProjectAmountWasta = quota.Wastage;

            subject.State = GWBSDetailCostSubjectState.编制;

            if (projectInfo != null)
            {
                subject.TheProjectGUID = projectInfo.Id;
                subject.TheProjectName = projectInfo.Name;
            }

            subject.TheGWBSDetail = oprDtl;

            subject.TheGWBSTree = oprDtl.TheGWBS;
            subject.TheGWBSTreeName = oprDtl.TheGWBS.Name;
            subject.TheGWBSTreeSyscode = oprDtl.TheGWBS.SysCode;

            oprDtl.ListCostSubjectDetails.Add(subject);
        }

        private void AddResourceUsageInTaskDetail(SubjectCostQuota quota, GWBSDetail detail)
        {
            GWBSDetailCostSubject subject = new GWBSDetailCostSubject();
            subject.Name = quota.Name;
            subject.MainResTypeFlag = quota.MainResourceFlag;

            subject.ProjectAmountUnitGUID = quota.ProjectAmountUnitGUID;
            subject.ProjectAmountUnitName = quota.ProjectAmountUnitName;

            subject.PriceUnitGUID = quota.PriceUnitGUID;
            subject.PriceUnitName = quota.PriceUnitName;

            //合同
            subject.ContractQuotaQuantity = quota.QuotaProjectAmount;

            subject.ContractBasePrice = quota.QuotaPrice;
            subject.ContractPricePercent = 1;
            subject.ContractQuantityPrice = subject.ContractBasePrice * subject.ContractPricePercent;

            subject.ContractPrice = subject.ContractQuotaQuantity * subject.ContractQuantityPrice;
            subject.ContractProjectAmount = detail.ContractProjectQuantity * subject.ContractQuotaQuantity;
            subject.ContractTotalPrice = subject.ContractProjectAmount * subject.ContractQuantityPrice;

            //责任
            subject.ResponsibleQuotaNum = quota.QuotaProjectAmount;

            subject.ResponsibleBasePrice = quota.QuotaPrice;
            subject.ResponsiblePricePercent = 1;
            subject.ResponsibilitilyPrice = subject.ResponsibleBasePrice * subject.ResponsiblePricePercent;

            subject.ResponsibleWorkPrice = subject.ResponsibleQuotaNum * subject.ResponsibilitilyPrice;
            subject.ResponsibilitilyWorkAmount = detail.ResponsibilitilyWorkAmount * subject.ResponsibleQuotaNum;
            subject.ResponsibilitilyTotalPrice = subject.ResponsibilitilyWorkAmount * subject.ResponsibilitilyPrice;//责任包干单价未知

            //计划
            subject.PlanQuotaNum = quota.QuotaProjectAmount;

            subject.PlanBasePrice = quota.QuotaPrice;
            subject.PlanPricePercent = 1;
            subject.PlanPrice = subject.PlanBasePrice * subject.PlanPricePercent;

            subject.PlanWorkPrice = subject.PlanQuotaNum * subject.PlanPrice;
            subject.PlanWorkAmount = detail.PlanWorkAmount * subject.PlanQuotaNum;
            subject.PlanTotalPrice = subject.PlanWorkAmount * subject.PlanPrice;//计划包干单价未知

            subject.AssessmentRate = quota.AssessmentRate;

            subject.ResourceUsageQuota = quota;

            if (quota.ListResources.Count > 0)
            {
                ResourceGroup itemResource = quota.ListResources.ElementAt(0);
                subject.ResourceTypeGUID = itemResource.ResourceTypeGUID;
                subject.ResourceTypeCode = itemResource.ResourceTypeCode;
                subject.ResourceTypeName = itemResource.ResourceTypeName;
                subject.ResourceTypeQuality = itemResource.ResourceTypeQuality;
                subject.ResourceTypeSpec = itemResource.ResourceTypeSpec;
                subject.ResourceCateSyscode = itemResource.ResourceCateSyscode;
                subject.DiagramNumber = itemResource.DiagramNumber;
            }

            subject.CostAccountSubjectGUID = quota.CostAccountSubjectGUID;
            subject.CostAccountSubjectName = quota.CostAccountSubjectName;
            if (quota.CostAccountSubjectGUID != null)
                subject.CostAccountSubjectSyscode = quota.CostAccountSubjectGUID.SysCode;

            subject.ProjectAmountWasta = quota.Wastage;

            subject.State = GWBSDetailCostSubjectState.编制;

            if (projectInfo != null)
            {
                subject.TheProjectGUID = projectInfo.Id;
                subject.TheProjectName = projectInfo.Name;
            }

            subject.TheGWBSDetail = detail;

            subject.TheGWBSTree = detail.TheGWBS;
            subject.TheGWBSTreeName = detail.TheGWBS.Name;
            subject.TheGWBSTreeSyscode = detail.TheGWBS.SysCode;

            detail.ListCostSubjectDetails.Add(subject);
        }

        //private decimal DecimalRound(Decimal val)
        //{
        //    return decimal.Round(val, 5);
        //}

        void txtBudgetResponsibilityPrice_TextChanged(object sender, EventArgs e)
        {
            if (txtBudgetResponsibilityPrice.Text.Trim() != "" && txtBudgetResponsibilityProjectAmount.Text.Trim() != "")
            {
                try
                {
                    txtBudgetResponsibilityTotalPrice.Text =
                        Decimal.Round(ClientUtil.ToDecimal(txtBudgetResponsibilityPrice.Text) * ClientUtil.ToDecimal(txtBudgetResponsibilityProjectAmount.Text), 3).ToString();
                }
                catch { }
            }
        }
        void txtBudgetResponsibilityProjectAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtBudgetResponsibilityPrice.Text.Trim() != "" && txtBudgetResponsibilityProjectAmount.Text.Trim() != "")
            {
                try
                {
                    txtBudgetResponsibilityTotalPrice.Text =
                        Decimal.Round(ClientUtil.ToDecimal(txtBudgetResponsibilityPrice.Text) * ClientUtil.ToDecimal(txtBudgetResponsibilityProjectAmount.Text), 3).ToString();
                }
                catch { }
            }
        }

        void txtBudgetPlanPrice_TextChanged(object sender, EventArgs e)
        {
            if (txtBudgetPlanPrice.Text.Trim() != "" && txtBudgetPlanProjectAmount.Text.Trim() != "")
            {
                try
                {
                    txtBudgetPlanTotalPrice.Text =
                        Decimal.Round(ClientUtil.ToDecimal(txtBudgetPlanPrice.Text) * ClientUtil.ToDecimal(txtBudgetPlanProjectAmount.Text), 3).ToString();
                }
                catch { }
            }
        }
        void txtBudgetPlanProjectAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtBudgetPlanPrice.Text.Trim() != "" && txtBudgetPlanProjectAmount.Text.Trim() != "")
            {
                try
                {
                    txtBudgetPlanTotalPrice.Text =
                        Decimal.Round(ClientUtil.ToDecimal(txtBudgetPlanPrice.Text) * ClientUtil.ToDecimal(txtBudgetPlanProjectAmount.Text), 3).ToString();
                }
                catch { }
            }
        }

        void txtBudgetContractPrice_TextChanged(object sender, EventArgs e)
        {
            if (txtBudgetContractPrice.Text.Trim() != "" && txtBudgetContractProjectAmount.Text.Trim() != "")
            {
                try
                {
                    txtBudgetContractTotalPrice.Text =
                        Decimal.Round(ClientUtil.ToDecimal(txtBudgetContractPrice.Text) * ClientUtil.ToDecimal(txtBudgetContractProjectAmount.Text), 3).ToString();
                }
                catch { }
            }
        }
        void txtBudgetContractProjectAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtBudgetContractPrice.Text.Trim() != "" && txtBudgetContractProjectAmount.Text.Trim() != "")
            {
                try
                {
                    txtBudgetContractTotalPrice.Text =
                        Decimal.Round(ClientUtil.ToDecimal(txtBudgetContractPrice.Text) * ClientUtil.ToDecimal(txtBudgetContractProjectAmount.Text), 3).ToString();
                }
                catch { }
            }
        }

        void mnuTaskDetail_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == 复制选中明细.Name)
            {
                mnuTree.Hide();

                if (gridGWBDetail.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请选中要复制的行！");
                    return;
                }

                listCopyNodeDetail.Clear();

                foreach (DataGridViewRow row in gridGWBDetail.SelectedRows)
                {
                    GWBSDetail dtl = row.Tag as GWBSDetail;
                    listCopyNodeDetail.Add(dtl);
                }
            }
        }
        #endregion

        #region 任务明细分科目成本操作
        private void AddCostSubjectInfoInGrid(GWBSDetailCostSubject dtl)
        {
            int index = gridCostSubject.Rows.Add();
            DataGridViewRow row = gridCostSubject.Rows[index];
            row.Cells["SubjectName"].Value = dtl.Name;
            row.Cells["SubjectCateName"].Value = dtl.CostAccountSubjectName;
            row.Cells["SubjectContractPrice"].Value = dtl.ContractPrice.ToString();
            row.Cells["SubjectContractProjectAmount"].Value = dtl.ContractProjectAmount;
            row.Cells["SubjectContractTotalPrice"].Value = dtl.ContractTotalPrice;

            row.Cells["SubjectResourceType"].Value = dtl.ResourceTypeName;
            row.Cells["SubjectResponsibilityPrice"].Value = dtl.ResponsibilitilyPrice;
            row.Cells["SubjectResponsibilityProjectAmount"].Value = dtl.ResponsibilitilyWorkAmount;
            row.Cells["SubjectResponsibilityTotalPrice"].Value = dtl.ResponsibilitilyTotalPrice;
            row.Cells["SubjectPlanPrice"].Value = dtl.PlanPrice;

            row.Cells["SubjectPlanProjectAmount"].Value = dtl.PlanWorkAmount;
            row.Cells["SubjectPlanTotalPrice"].Value = dtl.PlanTotalPrice;
            row.Cells["SubjectAddupAccountProjectAmount"].Value = dtl.AddupAccountProjectAmount;
            row.Cells["SubjectAddupAccountCost"].Value = dtl.AddupAccountCost;
            if (dtl.AddupAccountCostEndTime != null)
                row.Cells["SubjectAddupAccountCostEndTime"].Value = dtl.AddupAccountCostEndTime;

            row.Cells["SubjectCurrPeriodAccountProjectAmount"].Value = dtl.CurrentPeriodAccountProjectAmount;
            row.Cells["SubjectCurrPeriodAccountCost"].Value = dtl.CurrentPeriodAccountCost;
            if (dtl.CurrentPeriodAccountCostEndTime != null)
                row.Cells["SubjectCurrPeriodAccountCostEndTime"].Value = dtl.CurrentPeriodAccountCostEndTime;
            row.Cells["SubjectAddupBalanceProjectAmount"].Value = dtl.AddupBalanceProjectAmount;
            row.Cells["SubjectCurrPeriodBalanceProjectAmount"].Value = dtl.CurrentPeriodBalanceProjectAmount;

            row.Cells["SubjectCurrPeriodBalanceTotalPrice"].Value = dtl.CurrentPeriodBalanceTotalPrice;
            row.Cells["SubjectAddupBalanceTotalPrice"].Value = dtl.AddupBalanceTotalPrice;
            row.Cells["SubjectPriceUnit"].Value = dtl.PriceUnitName;
            row.Cells["SubjectProjectAmountUnit"].Value = dtl.ProjectAmountUnitName;
            row.Cells["SubjectAssessmentRate"].Value = dtl.AssessmentRate;

            row.Cells["SubjectProjectAmountWastage"].Value = dtl.ProjectAmountWasta;

            row.Tag = dtl;

            gridCostSubject.CurrentCell = row.Cells[0];
        }
        private void UpdateCostSubjectInfoInGrid(GWBSDetailCostSubject dtl)
        {
            foreach (DataGridViewRow row in gridCostSubject.SelectedRows)
            {
                GWBSDetailCostSubject d = row.Tag as GWBSDetailCostSubject;
                if (d.Id == dtl.Id)
                {
                    row.Cells["SubjectName"].Value = dtl.Name;
                    row.Cells["SubjectCateName"].Value = dtl.CostAccountSubjectName;
                    row.Cells["SubjectContractPrice"].Value = dtl.ContractPrice.ToString();
                    row.Cells["SubjectContractProjectAmount"].Value = dtl.ContractProjectAmount;
                    row.Cells["SubjectContractTotalPrice"].Value = dtl.ContractTotalPrice;

                    row.Cells["SubjectResourceType"].Value = dtl.ResourceTypeName;
                    row.Cells["SubjectResponsibilityPrice"].Value = dtl.ResponsibilitilyPrice;
                    row.Cells["SubjectResponsibilityProjectAmount"].Value = dtl.ResponsibilitilyWorkAmount;
                    row.Cells["SubjectResponsibilityTotalPrice"].Value = dtl.ResponsibilitilyTotalPrice;
                    row.Cells["SubjectPlanPrice"].Value = dtl.PlanPrice;

                    row.Cells["SubjectPlanProjectAmount"].Value = dtl.PlanWorkAmount;
                    row.Cells["SubjectPlanTotalPrice"].Value = dtl.PlanTotalPrice;
                    row.Cells["SubjectAddupAccountProjectAmount"].Value = dtl.AddupAccountProjectAmount;
                    row.Cells["SubjectAddupAccountCost"].Value = dtl.AddupAccountCost;
                    if (dtl.AddupAccountCostEndTime != null)
                        row.Cells["SubjectAddupAccountCostEndTime"].Value = dtl.AddupAccountCostEndTime;

                    row.Cells["SubjectCurrPeriodAccountProjectAmount"].Value = dtl.CurrentPeriodAccountProjectAmount;
                    row.Cells["SubjectCurrPeriodAccountCost"].Value = dtl.CurrentPeriodAccountCost;
                    if (dtl.CurrentPeriodAccountCostEndTime != null)
                        row.Cells["SubjectCurrPeriodAccountCostEndTime"].Value = dtl.CurrentPeriodAccountCostEndTime;
                    row.Cells["SubjectAddupBalanceProjectAmount"].Value = dtl.AddupBalanceProjectAmount;
                    row.Cells["SubjectCurrPeriodBalanceProjectAmount"].Value = dtl.CurrentPeriodBalanceProjectAmount;

                    row.Cells["SubjectCurrPeriodBalanceTotalPrice"].Value = dtl.CurrentPeriodBalanceTotalPrice;
                    row.Cells["SubjectAddupBalanceTotalPrice"].Value = dtl.AddupBalanceTotalPrice;
                    row.Cells["SubjectPriceUnit"].Value = dtl.PriceUnitName;
                    row.Cells["SubjectProjectAmountUnit"].Value = dtl.ProjectAmountUnitName;
                    row.Cells["SubjectAssessmentRate"].Value = dtl.AssessmentRate;

                    row.Cells["SubjectProjectAmountWastage"].Value = dtl.ProjectAmountWasta;
                    row.Tag = dtl;
                    break;
                }
            }
        }
        void btnAddCostSubject_Click(object sender, EventArgs e)
        {
            if (oprDtl == null)
            {
                MessageBox.Show("请先选择一个工程任务明细！");
                return;
            }
            else if (oprDtl.State != VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit)
            {
                MessageBox.Show("请选择状态为‘编制’的任务明细进行操作！");
                return;
            }

            bool addCostItemFlag = false;
            if (string.IsNullOrEmpty(oprDtl.Id))
                addCostItemFlag = true;

            VEditGWBSDetailCostSubject frm = new VEditGWBSDetailCostSubject(new MGWBSTree());
            frm.OptGWBSDetail = oprDtl;
            frm.ShowDialog();

            if (frm.OptCostSubject != null)// && !string.IsNullOrEmpty(frm.OptCostQuota.Id)
            {
                oprDtl = frm.OptGWBSDetail;
                if (!addCostItemFlag)
                    UpdateDetailInfoInGrid(oprDtl, true);

                AddCostSubjectInfoInGrid(frm.OptCostSubject);
            }
        }
        void btnUpdateCostSubject_Click(object sender, EventArgs e)
        {
            if (gridCostSubject.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要修改的行！");
                return;
            }
            if (gridCostSubject.SelectedRows.Count > 1)
            {
                MessageBox.Show("一次只能修改一行！");
                return;
            }
            GWBSDetailCostSubject subject = gridCostSubject.SelectedRows[0].Tag as GWBSDetailCostSubject;

            VEditGWBSDetailCostSubject frm = new VEditGWBSDetailCostSubject(new MGWBSTree());
            frm.OptGWBSDetail = oprDtl;
            frm.OptCostSubject = subject;
            frm.ShowDialog();

            UpdateCostSubjectInfoInGrid(frm.OptCostSubject);
        }
        void btnDeleteCostSubject_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridCostSubject.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请选择要删除的行！");
                    gridCostSubject.Focus();
                    return;
                }

                IList list = new List<GWBSDetailCostSubject>();
                List<int> listRowIndex = new List<int>();
                foreach (DataGridViewRow row in gridCostSubject.SelectedRows)
                {
                    GWBSDetailCostSubject item = row.Tag as GWBSDetailCostSubject;
                    list.Add(item);
                    listRowIndex.Add(row.Index);
                }

                if (MessageBox.Show("删除后不能恢复，您确认要删除选中分科目成本信息吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                model.DeleteCostSubject(list);

                listRowIndex.Sort();
                for (int i = listRowIndex.Count - 1; i > -1; i--)
                {
                    gridCostSubject.Rows.RemoveAt(listRowIndex[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        #endregion


        #region  相关文档
        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnEmpty_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in gridDocument.Rows)
            {
                var.Cells["colCheck"].Value = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnCheckAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in gridDocument.Rows)
            {
                var.Cells["colCheck"].Value = true;
            }
        }

        #region
        /// <summary>
        /// 工程对象关联文档   选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnCheckFile_Click(object sender, EventArgs e)
        {
            filePath = GetfilePath();
            if (filePath != "")
            {
                txtFileURL.Text = filePath;
                FileInfo file = new FileInfo(filePath);
                txtDocName.Text = file.Name.Substring(0, file.Name.LastIndexOf("."));
            }
        }
        /// <summary>
        /// 得到上传文件路径
        /// </summary>
        /// <returns></returns>
        string GetfilePath()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "所有文件(*.*)|*.*";
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog();
            return openFileDialog1.FileName;
        }


        /// <summary>
        /// 放弃新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnQuit_Click(object sender, EventArgs e)
        {
            AddAfter();
        }

        void AddAfter()
        {
            txtFileURL.Text = "";
            txtCateCode.Text = "";
            txtDocName.Text = "";
            txtDocumentDescripion.Text = "";
            txtDocumentCode.Text = "";
            txtDocumentKeyWord.Text = "";
            //cmbAlterMode.Text = "";
            //cmbVerifyState.Text = "";
            //cmbVerifySwitch.Text = "";
            //cmbInforType.Text = "";
            cmbAlterMode.SelectedIndex = -1;
            cmbVerifyState.SelectedIndex = -1;
            cmbVerifySwitch.SelectedIndex = -1;
            cmbInforType.SelectedIndex = -1;
            gbAdd.Enabled = false;
            filePath = "";
            gbShow.Enabled = true;
            txtDocName.Tag = null;
            //txtDocumentName.Tag = null;
            txtDocumentKeyWord.Tag = null;
            txtCateCode.Tag = null;
            lblState.Visible = lblAlter.Visible = true;
            cmbAlterMode.Visible = cmbVerifyState.Visible = true;
        }
        /// <summary>
        /// 保存新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSaveAdd_Click(object sender, EventArgs e)
        {
            if (addOrUpDate == "add")
            {
                if (rbtnProjectDocument.Checked)
                {
                    #region 新增文档、文件、MBP对象

                    if (txtFileURL.Text.Trim() == "" || filePath == null)
                    {
                        MessageBox.Show("请选择文件！");
                        return;
                    }

                    int rowCount = 1;

                    progressBarDocUpload.Minimum = 0;
                    progressBarDocUpload.Maximum = rowCount >= 10 ? rowCount + 1 : 11;
                    progressBarDocUpload.Value = 1;

                    //文档
                    DocumentMaster master = new DocumentMaster();

                    if (txtCateCode.Tag != null)
                    {
                        DocumentCategory cate = txtCateCode.Tag as DocumentCategory;
                        master.Category = cate;
                        master.CategoryCode = cate.Code;
                        master.CategoryName = cate.Name;
                        master.CategorySysCode = cate.SysCode;
                    }
                    master.ProjectCode = projectInfo.Code;
                    master.ProjectId = projectInfo.Id;
                    master.ProjectName = projectInfo.Name;

                    master.CreateTime = DateTime.Now;

                    master.OwnerID = person;
                    master.OwnerName = person.Name;
                    master.OwnerOrgSysCode = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.SysCode;

                    master.Name = txtDocName.Text;
                    master.Code = txtDocumentCode.Text;
                    //master.Author = txtDocumentAuthor.Text;
                    //master.KeyWords = txtDocumentKeywords.Text;
                    //master.Description = txtDocumentExplain.Text;
                    //master.Title = txtDocumentTitle.Text;

                    //master.DocType = DocumentInfoTypeEnum.普通文档;
                    //master.CheckoutState = DocumentCheckOutStateEnum.检入;
                    //master.SecurityLevel = securityLevel;
                    master.State = DocumentState.Edit;

                    master.IsInspectionLot = false;
                    master.SetNewVersion();
                    master.SetNewRevision();

                    master.UpdateTime = DateTime.Now;
                    //文件
                    FileCabinet appFileCabinet = null;
                    appFileCabinet = StaticMethod.GetDefaultFileCabinet();

                    DocumentDetail dtl = new DocumentDetail();
                    dtl.Master = master;
                    master.ListFiles.Add(dtl);

                    string filePath1 = txtFileURL.Text;
                    FileInfo file = new FileInfo(filePath1);
                    if (file.Exists)
                    {
                        FileStream fileStream = file.OpenRead();
                        int FileLen = (int)file.Length;
                        Byte[] FileData = new Byte[FileLen];
                        //将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
                        fileStream.Read(FileData, 0, FileLen);

                        dtl.FileDataByte = FileData;
                    }
                    dtl.ExtendName = Path.GetExtension(filePath1);
                    dtl.FileName = Path.GetFileName(filePath1);
                    dtl.TheFileCabinet = appFileCabinet;

                    IList listDoc = new List<ProObjectRelaDocument>();

                    #region 保存MBP对象关联文档信息

                    ProObjectRelaDocument rdoc = new ProObjectRelaDocument();
                    rdoc.TheProjectGUID = projectInfo.Id;
                    rdoc.TheProjectName = projectInfo.Name;
                    rdoc.TheProjectCode = projectInfo.Code;
                    if (cmbBusinessObject.Text == "工程任务包")
                    {
                        //rdoc.ProObjectName = oprNode.GetType().Name;
                        rdoc.ProObjectName = typeof(GWBSTree).Name; //oprNode.GetType().Name
                        rdoc.ProObjectGUID = oprNode.Id;
                    }
                    else
                    {
                        System.Web.UI.WebControls.ListItem li = cmbBusinessObject.SelectedItem as System.Web.UI.WebControls.ListItem;
                        rdoc.ProObjectName = typeof(InspectionLot).Name;
                        rdoc.ProObjectGUID = li.Value;
                    }

                    rdoc.DocumentOwner = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;
                    rdoc.DocumentOwnerName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                    //rdoc.DocumentGUID = resultList[0].EntityID;
                    rdoc.DocumentName = master.Name;
                    rdoc.DocumentDesc = master.Description;
                    rdoc.SubmitTime = DateTime.Now;

                    rdoc.DocumentCateCode = master.CategoryCode;
                    rdoc.DocumentCateName = master.CategoryName;

                    rdoc.FileURL = filePath;
                    rdoc.DocumentCode = master.Code;
                    rdoc.OrganizationSyscode = oprNode.SysCode;
                    if (cmbDocVerify.SelectedItem != null)
                    {
                        IList docmentVerifyList = new List<ProjectDocumentVerify>();
                        ProjectDocumentVerify pdv = cmbDocVerify.SelectedItem as ProjectDocumentVerify;
                        rdoc.ProjectDocumentVerifyID = pdv.Id;
                        if (pdv.AssociateLevel == ProjectDocumentAssociateLevel.GWBS)
                        {
                            pdv.SubmitState = ProjectDocumentSubmitState.已提交;
                            docmentVerifyList.Add(pdv);
                            model.SaveOrUpdate(docmentVerifyList);
                        }
                        if (pdv.AssociateLevel == ProjectDocumentAssociateLevel.检验批)
                        {
                            ObjectQuery oq = new ObjectQuery();
                            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProjectDocumentVerifyID", pdv.Id));
                            IList relaDocument = model.ObjectQuery(typeof(ProObjectRelaDocument), oq);

                            ObjectQuery oq1 = new ObjectQuery();
                            oq1.AddCriterion(NHibernate.Criterion.Expression.Eq("ProjectTask.Id", oprNode.Id));
                            IList il = model.ObjectQuery(typeof(InspectionLot), oq1);

                            if (relaDocument.Count == il.Count)
                            {
                                pdv.SubmitState = ProjectDocumentSubmitState.已提交;
                                docmentVerifyList.Add(pdv);
                                model.SaveOrUpdate(docmentVerifyList);
                            }

                        }
                    }
                    rdoc = docModel.SaveDocumentAndFileAndDocObject(master, rdoc);
                    IList resultListDoc = new ArrayList();
                    resultListDoc.Add(rdoc);
                    if (resultListDoc != null)
                    {
                        if (rowCount < 10)
                            progressBarDocUpload.Value += (int)Math.Floor((decimal)10 / rowCount);
                        else
                            progressBarDocUpload.Value += 1;

                        MessageBox.Show("新增成功！");
                        ShowList(resultListDoc, "ProObjectRelaDocument");
                    }
                    else
                    {
                        MessageBox.Show("新增失败！");
                    }

                    progressBarDocUpload.Value = 0;
                    #endregion 保存MBP对象关联文档信息

                    #endregion
                }
                if (rbtnDocumentStencil.Checked)
                {
                    #region 新增工程文档验证
                    if (txtDocName.Tag == null)
                    {
                        MessageBox.Show("请选择模版！");
                        return;
                    }
                    DocumentMaster doc = txtDocName.Tag as DocumentMaster;
                    ProjectDocumentVerify docVerify = new ProjectDocumentVerify();
                    docVerify.AssociateLevel = VirtualMachine.Component.Util.EnumUtil<ProjectDocumentAssociateLevel>.FromDescription(cmbAssociateLevel.Text);//ProjectDocumentAssociateLevel.GWBS;
                    docVerify.SubmitState = VirtualMachine.Component.Util.EnumUtil<ProjectDocumentSubmitState>.FromDescription(cmbVerifyState.Text);
                    docVerify.DocuemntID = doc.Id;

                    docVerify.DocumentCategoryCode = doc.CategoryCode;
                    docVerify.DocumentCategoryName = doc.CategoryName;

                    docVerify.DocumentCode = txtDocumentCode.Text;
                    docVerify.DocumentDesc = txtDocumentDescripion.Text;
                    docVerify.DocumentName = txtDocName.Text;
                    docVerify.ProjectTaskName = oprNode.Name;
                    docVerify.ProjectTask = oprNode;
                    docVerify.TaskSyscode = oprNode.SysCode;
                    docVerify.VerifySwitch = VirtualMachine.Component.Util.EnumUtil<ProjectDocumentVerifySwitch>.FromDescription(cmbVerifySwitch.Text);
                    docVerify.AlterMode = VirtualMachine.Component.Util.EnumUtil<ProjectTaskTypeAlterMode>.FromDescription(cmbAlterMode.Text);
                    docVerify.ProjectName = "知识库";
                    docVerify.ProjectCode = "KB";
                    IList docVerifyList = new List<ProjectDocumentVerify>();
                    docVerifyList.Add(docVerify);
                    IList resultDocVerify = model.SaveOrUpdate(docVerifyList);
                    if (resultDocVerify != null && resultDocVerify.Count > 0)
                    {
                        MessageBox.Show("保存成功！");
                        ShowList(resultDocVerify, "ProjectDocumentVerify");
                    }
                    else
                    {
                        MessageBox.Show("保存失败！");
                    }
                    #endregion
                }
            }
            else
            {
                if (colAlterMode.Visible == false)
                {
                    #region 修改文档、文件、MBP对象
                    ProObjectRelaDocument relaDocument = txtDocumentCode.Tag as ProObjectRelaDocument;
                    DocumentMaster pd = txtDocumentKeyWord.Tag as DocumentMaster;

                    pd.Name = relaDocument.DocumentName = txtDocName.Text;
                    pd.Code = relaDocument.DocumentCode = txtDocumentCode.Text;
                    pd.Description = relaDocument.DocumentDesc = txtDocumentDescripion.Text;
                    if (txtCateCode.Tag != null)
                    {
                        pd.CategoryCode = relaDocument.DocumentCateCode = (txtCateCode.Tag as DocumentCategory).Code;
                        pd.CategoryName = relaDocument.DocumentCateName = (txtCateCode.Tag as DocumentCategory).Name;
                    }

                    if (cmbDocVerify.SelectedItem != null)
                    {
                        ProjectDocumentVerify pdv = cmbDocVerify.SelectedItem as ProjectDocumentVerify;

                        if (relaDocument.ProjectDocumentVerifyID != pdv.Id)
                        {
                            if (relaDocument.ProjectDocumentVerifyID != null)
                            {
                                ObjectQuery oq = new ObjectQuery();
                                oq.AddCriterion(NHibernate.Criterion.Expression.Eq("Id", relaDocument.ProjectDocumentVerifyID));
                                IList docVerifyList = model.ObjectQuery(typeof(ProjectDocumentVerify), oq);
                                ProjectDocumentVerify docVerify = docVerifyList[0] as ProjectDocumentVerify;
                                docVerify.SubmitState = ProjectDocumentSubmitState.未提交;
                                IList updateDocVerify = new List<ProjectDocumentVerify>();
                                updateDocVerify.Add(docVerify);
                                model.SaveOrUpdate(updateDocVerify);
                            }
                            if (pdv.AssociateLevel == ProjectDocumentAssociateLevel.GWBS)
                            {
                                pdv.SubmitState = ProjectDocumentSubmitState.已提交;
                                IList updateDocVerify = new List<ProjectDocumentVerify>();
                                updateDocVerify.Add(pdv);
                                model.SaveOrUpdate(updateDocVerify);
                            }
                            if ((pdv.AssociateLevel == ProjectDocumentAssociateLevel.检验批))
                            {
                                ObjectQuery oq = new ObjectQuery();
                                oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProjectDocumentVerifyID", pdv.Id));
                                IList relaDoc = model.ObjectQuery(typeof(ProObjectRelaDocument), oq);

                                ObjectQuery oq1 = new ObjectQuery();
                                oq1.AddCriterion(NHibernate.Criterion.Expression.Eq("ProjectTask.Id", relaDocument.Id));
                                IList il = model.ObjectQuery(typeof(InspectionLot), oq1);

                                if (relaDoc.Count == il.Count)
                                {
                                    pdv.SubmitState = ProjectDocumentSubmitState.已提交;
                                    IList docmentVerifyList = new List<ProjectDocumentVerify>();
                                    docmentVerifyList.Add(pdv);
                                    model.SaveOrUpdate(docmentVerifyList);
                                }

                            }
                        }
                        relaDocument.ProjectDocumentVerifyID = pdv.Id;
                    }

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        DocumentDetail dtl = pd.ListFiles.ElementAt(0);
                        //string filePath1 = txtFileURL.Text;
                        FileInfo file = new FileInfo(filePath);
                        if (file.Exists)
                        {
                            FileStream fileStream = file.OpenRead();
                            int FileLen = (int)file.Length;
                            Byte[] FileData = new Byte[FileLen];
                            //将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
                            fileStream.Read(FileData, 0, FileLen);

                            dtl.FileDataByte = FileData;
                        }
                        dtl.ExtendName = Path.GetExtension(filePath);
                        dtl.FileName = Path.GetFileName(filePath);

                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("Id", dtl.Id));
                        oq.AddFetchMode("TheFileCabinet", NHibernate.FetchMode.Eager);
                        DocumentDetail resultDtl = docModel.ObjectQuery(typeof(DocumentDetail), oq)[0] as DocumentDetail;
                        dtl.TheFileCabinet = resultDtl.TheFileCabinet;
                        relaDocument = docModel.UpdateDocumentAndFileAndDocObject(pd, dtl, relaDocument);
                    }
                    else
                    {
                        relaDocument = docModel.UpdateDocumentAndFileAndDocObject(pd, null, relaDocument);
                    }

                    IList updateRelaDoc = new List<ProObjectRelaDocument>();
                    updateRelaDoc.Add(relaDocument);
                    if (updateRelaDoc != null)
                    {
                        MessageBox.Show("修改成功！");
                        int rowIndex = -1;
                        foreach (DataGridViewRow row in this.gridDocument.Rows)
                        {
                            if ((bool)row.Cells[1].EditedFormattedValue)
                            {
                                rowIndex = row.Index;
                            }
                        }
                        ProObjectRelaDocument pord = updateRelaDoc[0] as ProObjectRelaDocument;
                        gridDocument[DocumentName.Name, rowIndex].Value = pord.DocumentName;
                        gridDocument[DocumentCateCode.Name, rowIndex].Value = pord.DocumentCateCode;
                        gridDocument[DocumentCateName.Name, rowIndex].Value = pord.DocumentCateName;
                        gridDocument[DocumentCode.Name, rowIndex].Value = pord.DocumentCode;
                        gridDocument[DocumentDesc.Name, rowIndex].Value = pord.DocumentDesc;
                        gridDocument.Rows[rowIndex].Tag = pord;

                    }
                    else
                    {
                        MessageBox.Show("修改失败！");
                    }
                    #endregion
                }
                else
                {

                    #region 修改工程文档验证
                    ProjectDocumentVerify docVerify = txtDocumentCode.Tag as ProjectDocumentVerify;
                    if (txtDocName.Tag != null)
                    {
                        DocumentMaster doc = txtDocName.Tag as DocumentMaster;
                        docVerify.DocumentCategoryCode = doc.CategoryCode;
                        docVerify.DocumentCategoryName = doc.CategoryName;
                        docVerify.DocuemntID = doc.Id;
                    }
                    docVerify.DocumentCode = txtDocumentCode.Text;
                    docVerify.DocumentDesc = txtDocumentDescripion.Text;
                    docVerify.DocumentName = txtDocName.Text;
                    docVerify.VerifySwitch = VirtualMachine.Component.Util.EnumUtil<ProjectDocumentVerifySwitch>.FromDescription(cmbVerifySwitch.Text);
                    docVerify.AlterMode = VirtualMachine.Component.Util.EnumUtil<ProjectTaskTypeAlterMode>.FromDescription(cmbAlterMode.Text);
                    docVerify.SubmitState = VirtualMachine.Component.Util.EnumUtil<ProjectDocumentSubmitState>.FromDescription(cmbVerifyState.Text);
                    docVerify.AssociateLevel = VirtualMachine.Component.Util.EnumUtil<ProjectDocumentAssociateLevel>.FromDescription(cmbAssociateLevel.Text);
                    IList docVerifyList = new List<ProjectDocumentVerify>();
                    docVerifyList.Add(docVerify);
                    IList resultDocVerify = model.SaveOrUpdate(docVerifyList);
                    if (resultDocVerify != null && resultDocVerify.Count > 0)
                    {
                        MessageBox.Show("修改成功！");
                        int rowIndex = -1;
                        foreach (DataGridViewRow row in this.gridDocument.Rows)
                        {
                            if ((bool)row.Cells[1].EditedFormattedValue)
                            {
                                rowIndex = row.Index;
                            }
                        }
                        ProjectDocumentVerify pdv = resultDocVerify[0] as ProjectDocumentVerify;
                        gridDocument[DocumentName.Name, rowIndex].Value = pdv.DocumentName;
                        gridDocument[DocumentCateCode.Name, rowIndex].Value = pdv.DocumentCategoryCode;
                        gridDocument[DocumentCateName.Name, rowIndex].Value = pdv.DocumentCategoryName;
                        gridDocument[DocumentCode.Name, rowIndex].Value = pdv.DocumentCode;
                        gridDocument[DocumentDesc.Name, rowIndex].Value = pdv.DocumentDesc;
                        gridDocument[colAlterMode.Name, rowIndex].Value = pdv.AlterMode.ToString();
                        gridDocument[colState.Name, rowIndex].Value = pdv.SubmitState.ToString();
                        gridDocument.Rows[rowIndex].Tag = pdv;
                    }
                    else
                    {
                        MessageBox.Show("修改失败！");
                    }
                    #endregion
                }
            }

            AddAfter();
        }

        void ShowList(IList list, string s)
        {
            if (s == "ProObjectRelaDocument")
            {
                colAlterMode.Visible = colState.Visible = false;
                if (list == null && list.Count <= 0) return;
                foreach (ProObjectRelaDocument rd in list)
                {
                    int rowIndex = gridDocument.Rows.Add();
                    gridDocument[DocumentName.Name, rowIndex].Value = rd.DocumentName;
                    gridDocument[DocumentCateCode.Name, rowIndex].Value = rd.DocumentCateCode;
                    gridDocument[DocumentCateName.Name, rowIndex].Value = rd.DocumentCateName;
                    gridDocument[DocumentCode.Name, rowIndex].Value = rd.DocumentCode;
                    gridDocument[DocumentDesc.Name, rowIndex].Value = rd.DocumentDesc;
                    gridDocument.Rows[rowIndex].Tag = rd;
                }
            }
            else
            {
                colAlterMode.Visible = colState.Visible = true;
                if (list == null && list.Count <= 0) return;
                foreach (ProjectDocumentVerify dv in list)
                {
                    int rowIndex = gridDocument.Rows.Add();
                    gridDocument[DocumentName.Name, rowIndex].Value = dv.DocumentName;
                    gridDocument[DocumentCateCode.Name, rowIndex].Value = dv.DocumentCategoryCode;
                    gridDocument[DocumentCateName.Name, rowIndex].Value = dv.DocumentCategoryName;
                    gridDocument[DocumentCode.Name, rowIndex].Value = dv.DocumentCode;
                    gridDocument[DocumentDesc.Name, rowIndex].Value = dv.DocumentDesc;
                    gridDocument[colAlterMode.Name, rowIndex].Value = dv.AlterMode.ToString();
                    gridDocument[colState.Name, rowIndex].Value = dv.SubmitState.ToString();
                    gridDocument.Rows[rowIndex].Tag = dv;
                }
            }
        }

        /// <summary>
        /// 选择文档模版
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnCheckDocumentStencil_Click(object sender, EventArgs e)
        {
            VDocumentsModelAndTaskType frm = new VDocumentsModelAndTaskType(oprNode);
            frm.ShowDialog();
            IList resultList = frm.ResultList;
            if (resultList != null && resultList.Count > 0)
            {
                ShowList(resultList, "ProjectDocumentVerify");
            }
            //VDocumentsSelect vsdl = new VDocumentsSelect("update");
            //vsdl.ShowDialog();
            //IList resultList = vsdl.ResultList;
            //if (resultList == null) return;
            //if (resultList.Count > 0 && resultList != null)
            //{
            //    foreach (DocumentMaster doc in resultList)
            //    {
            //        txtCateCode.Text = doc.CategoryName;
            //        txtCateCode.Tag = doc.Category;
            //        txtDocName.Text = doc.Name;
            //        txtDocName.Tag = doc;
            //        txtDocumentCode.Text = doc.Code;
            //        txtDocumentDescripion.Text = doc.Description;
            //    }
            //}
        }
        /// <summary>
        /// 选择分类编码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearchCateCode_Click(object sender, EventArgs e)
        {
            bool flag = false;

            if (rbtnDocumentStencil.Checked)//文档模板
            {
                flag = true;
            }
            VDocumentCategorySelect frm = new VDocumentCategorySelect(flag);
            frm.ShowDialog();
            DocumentCategory cate = frm.ResultCate;
            if (cate != null)
            {
                //txtCateCode.Text = cate.CategoryCode;
                txtCateCode.Text = cate.Name;
                txtCateCode.Tag = cate;
            }
        }

        /// <summary>
        /// 业务对象改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbBusinessObject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBusinessObject.Text != "工程任务包")
            {
                rbtnDocumentStencil.Enabled = false;
                rbtnProjectDocument.Checked = true;
            }
            else
            {
                rbtnDocumentStencil.Enabled = true;
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAddNew_Click(object sender, EventArgs e)
        {
            gbAdd.Enabled = true;
            addOrUpDate = "add";
            //if (cmbBusinessObject.Text == "工程任务包")
            //{
            if (rbtnProjectDocument.Checked)
            {
                //System.Web.UI.WebControls.ListItem li = cmbBusinessObject.SelectedItem as System.Web.UI.WebControls.ListItem;
                //MessageBox.Show(li.Value);
                AddProjectObjectDocument();

                ShowDocVerify(cmbBusinessObject.Text);
            }
            if (rbtnDocumentStencil.Checked)
            {
                AddStencil();
            }

        }
        void ShowDocVerify(string s)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProjectTask.Id", oprNode.Id));
            if (s == "工程任务包")
            {
                oq.AddCriterion(NHibernate.Criterion.Expression.Eq("AssociateLevel", ProjectDocumentAssociateLevel.GWBS));
            }
            else
            {
                oq.AddCriterion(NHibernate.Criterion.Expression.Eq("AssociateLevel", ProjectDocumentAssociateLevel.检验批));
            }
            IList listDocVerify = model.ObjectQuery(typeof(ProjectDocumentVerify), oq);
            cmbDocVerify.DataSource = listDocVerify;
            cmbDocVerify.DisplayMember = "DocumentName";
        }
        /// <summary>
        /// 新增工程文档验证时界面控件
        /// </summary>
        void AddStencil()
        {
            gbShow.Enabled = false;
            txtFileURL.Enabled = false;
            btnCheckDocumentStencil.Enabled = true;
            cmbVerifyState.Enabled = cmbVerifySwitch.Enabled = cmbAlterMode.Enabled = true;
            cmbVerifyState.Visible = cmbAlterMode.Visible = true;
            lblAlter.Visible = lblState.Visible = true;
            cmbDocVerify.Enabled = false;
            btnCheckFile.Enabled = false;
            txtDocumentKeyWord.Enabled = false;
            cmbInforType.Enabled = false;
            cmbAssociateLevel.Enabled = true;
        }
        /// <summary>
        /// 新增工程对象关联文档时界面控件
        /// </summary>
        void AddProjectObjectDocument()
        {
            gbShow.Enabled = false;
            //gbAdd.Enabled = true
            btnCheckDocumentStencil.Enabled = false;
            cmbVerifyState.Enabled = cmbVerifySwitch.Enabled = cmbAlterMode.Enabled = false;
            cmbVerifyState.Visible = cmbAlterMode.Visible = false;
            lblAlter.Visible = lblState.Visible = false;
            txtFileURL.Enabled = true;
            cmbDocVerify.Enabled = true;
            btnCheckFile.Enabled = true;
            txtDocumentKeyWord.Enabled = true;
            cmbInforType.Enabled = true;
            cmbAssociateLevel.Enabled = false;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearch_Click(object sender, EventArgs e)
        {
            if (cmbBusinessObject.Text == "工程任务包")
            {
                if (rbtnProjectDocument.Checked)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", oprNode.Id));
                    IList relaDocumentList = model.ObjectQuery(typeof(ProObjectRelaDocument), oq);

                    gridDocument.Rows.Clear();
                    //if (relaDocumentList != null && relaDocumentList.Count > 0)
                    ShowList(relaDocumentList, "ProObjectRelaDocument");
                }
                if (rbtnDocumentStencil.Checked)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProjectTask.Id", oprNode.Id));
                    IList relaDocumentList = model.ObjectQuery(typeof(ProjectDocumentVerify), oq);

                    gridDocument.Rows.Clear();
                    //if (relaDocumentList != null && relaDocumentList.Count > 0)
                    ShowList(relaDocumentList, "ProjectDocumentVerify");
                }
                objecIsGWBS = "工程任务包";
            }
            else
            {
                if (rbtnProjectDocument.Checked)
                {
                    System.Web.UI.WebControls.ListItem li = cmbBusinessObject.SelectedItem as System.Web.UI.WebControls.ListItem;
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", li.Value));
                    IList relaDocumentList = model.ObjectQuery(typeof(ProObjectRelaDocument), oq);

                    gridDocument.Rows.Clear();
                    //if (relaDocumentList != null && relaDocumentList.Count > 0)
                    ShowList(relaDocumentList, "ProObjectRelaDocument");
                }
                objecIsGWBS = "检验批";
            }
        }
        /// <summary>
        /// 得到选中的集合
        /// </summary>
        /// <returns></returns>
        IList GetCheckedList()
        {
            IList selctedDocList = new ArrayList();
            foreach (DataGridViewRow row in this.gridDocument.Rows)
            {
                if ((bool)row.Cells[1].EditedFormattedValue)
                {
                    if (colAlterMode.Visible == true)
                    {
                        ProjectDocumentVerify docVerify = row.Tag as ProjectDocumentVerify;
                        selctedDocList.Add(docVerify);
                    }
                    else
                    {
                        ProObjectRelaDocument relaDocument = row.Tag as ProObjectRelaDocument;
                        selctedDocList.Add(relaDocument);
                    }
                }
            }
            return selctedDocList;
        }
        #endregion

        #region 文档操作
        //下载 预览
        void btnDownLoadDocument_Click(object sender, EventArgs e)
        {
            IList getCheckedList = GetCheckedList();
            if (gridDocument.SelectedRows.Count == 0 || getCheckedList.Count < 1)
            {
                MessageBox.Show("请选择要下载的文档！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gridDocument.Focus();
                return;
            }
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();

            if (colAlterMode.Visible == true)
            {
                IList downList = new ArrayList();
                foreach (ProjectDocumentVerify pdv in getCheckedList)
                {
                    dis.Add(Expression.Eq("Id", pdv.DocuemntID));
                }
            }
            else
            {
                foreach (ProObjectRelaDocument obj in getCheckedList)
                {
                    dis.Add(Expression.Eq("Id", obj.DocumentGUID));
                }
            }
            oq.AddCriterion(dis);
            IList docMasterList = model.ObjectQuery(typeof(DocumentMaster), oq);
            if (docMasterList != null && docMasterList.Count > 0)
            {
                VDocumentsDownloadOrOpen frm = new VDocumentsDownloadOrOpen(docMasterList);
                frm.ShowDialog();
            }

        }
        void btnOpenDocument_Click(object sender, EventArgs e)
        {
            IList getCheckedList = GetCheckedList();
            if (gridDocument.SelectedRows.Count == 0 || getCheckedList.Count < 1)
            {
                MessageBox.Show("请选择要打开的文档！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gridDocument.Focus();
                return;
            }
            if (getCheckedList.Count > 1)
            {
                MessageBox.Show("一次请打开一个文档！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gridDocument.Focus();
                return;
            }

            try
            {
                List<string> errorList = new List<string>();
                List<string> listFileFullPaths = new List<string>();
                if (colAlterMode.Visible == true)
                {

                    //ProjectDocumentVerify pdv = GetCheckedList()[0] as ProjectDocumentVerify;
                    ProjectDocumentVerify docVerify = getCheckedList[0] as ProjectDocumentVerify;
                    if (docVerify.ProjectCode == "KB")
                    {
                        #region
                        List<PLMWebServicesByKB.ProjectDocument> docList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.ProjectDocument>();
                        PLMWebServicesByKB.ProjectDocument[] projectDocList = null;

                        //ProjectDocumentVerify docVerify = GetCheckedList()[0] as ProjectDocumentVerify;
                        PLMWebServicesByKB.ProjectDocument doc = new Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.ProjectDocument();
                        doc.EntityID = docVerify.DocuemntID;
                        docList.Add(doc);

                        PLMWebServicesByKB.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.DownLoadDocumentByKB(docList.ToArray(), null,
                            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_UserName, Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_JobId,
                            null, out projectDocList);
                        if (es != null)
                        {
                            MessageBox.Show("下载失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (projectDocList != null)
                        {
                            string fileFullPath = AppDomain.CurrentDomain.BaseDirectory + "TempFilePreview\\" + Guid.NewGuid().ToString().Replace("-", "");
                            if (!Directory.Exists(fileFullPath))
                                Directory.CreateDirectory(fileFullPath);

                            for (int i = 0; i < projectDocList.Length; i++)
                            {
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
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region
                        List<PLMWebServices.ProjectDocument> docList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument>();
                        PLMWebServices.ProjectDocument[] projectDocList = null;

                        //ProjectDocumentVerify docVerify = GetCheckedList()[0] as ProjectDocumentVerify;
                        PLMWebServices.ProjectDocument doc = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument();
                        doc.EntityID = docVerify.DocuemntID;
                        docList.Add(doc);

                        PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.DownLoadDocumentByIRP(docList.ToArray(), null, userName, jobId, null, out projectDocList);
                        if (es != null)
                        {
                            MessageBox.Show("下载失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (projectDocList != null)
                        {
                            string fileFullPath = AppDomain.CurrentDomain.BaseDirectory + "TempFilePreview\\" + Guid.NewGuid().ToString().Replace("-", "");
                            if (!Directory.Exists(fileFullPath))
                                Directory.CreateDirectory(fileFullPath);

                            for (int i = 0; i < projectDocList.Length; i++)
                            {
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
                            }
                        }
                    }
                        #endregion
                }
                else
                {
                    //ProObjectRelaDocument pord = GetCheckedList()[0] as ProObjectRelaDocument;
                    ProObjectRelaDocument relaDocument = getCheckedList[0] as ProObjectRelaDocument;
                    if (relaDocument.TheProjectCode == "KB")
                    {
                        #region
                        List<PLMWebServicesByKB.ProjectDocument> docList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.ProjectDocument>();
                        PLMWebServicesByKB.ProjectDocument[] projectDocList = null;

                        //ProObjectRelaDocument relaDocument = GetCheckedList()[0] as ProObjectRelaDocument;
                        PLMWebServicesByKB.ProjectDocument doc = new Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.ProjectDocument();
                        doc.EntityID = relaDocument.DocumentGUID;
                        docList.Add(doc);

                        PLMWebServicesByKB.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.DownLoadDocumentByKB(docList.ToArray(), null,
                            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_UserName, Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_JobId,
                            null, out projectDocList);
                        if (es != null)
                        {
                            MessageBox.Show("下载失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (projectDocList != null)
                        {
                            string fileFullPath = AppDomain.CurrentDomain.BaseDirectory + "TempFilePreview\\" + Guid.NewGuid().ToString().Replace("-", "");
                            if (!Directory.Exists(fileFullPath))
                                Directory.CreateDirectory(fileFullPath);

                            for (int i = 0; i < projectDocList.Length; i++)
                            {
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
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region
                        List<PLMWebServices.ProjectDocument> docList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument>();
                        PLMWebServices.ProjectDocument[] projectDocList = null;

                        //ProObjectRelaDocument relaDocument = GetCheckedList()[0] as ProObjectRelaDocument;
                        PLMWebServices.ProjectDocument doc = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.ProjectDocument();
                        doc.EntityID = relaDocument.DocumentGUID;
                        docList.Add(doc);

                        PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.DownLoadDocumentByIRP(docList.ToArray(), null, userName, jobId, null, out projectDocList);
                        if (es != null)
                        {
                            MessageBox.Show("下载失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (projectDocList != null)
                        {
                            string fileFullPath = AppDomain.CurrentDomain.BaseDirectory + "TempFilePreview\\" + Guid.NewGuid().ToString().Replace("-", "");
                            if (!Directory.Exists(fileFullPath))
                                Directory.CreateDirectory(fileFullPath);

                            for (int i = 0; i < projectDocList.Length; i++)
                            {
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
                            }
                        }
                        #endregion
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
            IList getCheckedList = GetCheckedList();
            if (gridDocument.SelectedRows.Count == 0 || getCheckedList.Count < 1)
            {
                MessageBox.Show("请选择要修改的文档！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gridDocument.Focus();
                return;
            }
            else if (gridDocument.SelectedRows.Count > 1 || getCheckedList.Count > 1)
            {
                MessageBox.Show("一次只能修改一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gridDocument.Focus();
                return;
            }

            gbAdd.Enabled = true;
            addOrUpDate = "update";

            lblSave.Visible = progressBarDocUpload.Visible = false;

            if (colAlterMode.Visible == false)
            {
                AddProjectObjectDocument();
                ShowDocVerify(objecIsGWBS);
                ProObjectRelaDocument relaDocument = getCheckedList[0] as ProObjectRelaDocument;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", relaDocument.DocumentGUID));
                oq.AddFetchMode("Category", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("ListFiles", NHibernate.FetchMode.Eager);
                DocumentMaster docMaster = docModel.ObjectQuery(typeof(DocumentMaster), oq)[0] as DocumentMaster;

                txtDocName.Text = relaDocument.DocumentName;
                txtDocumentCode.Text = relaDocument.DocumentCode;
                txtDocumentCode.Tag = relaDocument;
                txtDocumentDescripion.Text = relaDocument.DocumentDesc;

                txtCateCode.Text = docMaster.CategoryName;
                txtCateCode.Tag = docMaster.Category;

                //txtDocumentKeyWord.Text = pd[0].KeyWords;
                txtDocumentKeyWord.Tag = docMaster;
                //cmbInforType.Text = pd[0].DocType.ToString();
            }
            else
            {
                AddStencil();

                ProjectDocumentVerify docVerify = getCheckedList[0] as ProjectDocumentVerify;

                txtDocName.Text = docVerify.DocumentName;

                DocumentCategory cate = new DocumentCategory();
                cate.Code = docVerify.DocumentCategoryCode;
                cate.Name = docVerify.DocumentCategoryName;
                txtCateCode.Text = cate.Name;
                txtCateCode.Tag = cate;

                txtDocumentCode.Text = docVerify.DocumentCode;
                txtDocumentCode.Tag = docVerify;
                txtDocumentDescripion.Text = docVerify.DocumentDesc;
                cmbAlterMode.Text = docVerify.AlterMode.ToString();
                cmbVerifyState.Text = docVerify.SubmitState.ToString();
                cmbVerifySwitch.Text = docVerify.VerifySwitch.ToString();
                cmbAssociateLevel.Text = docVerify.AssociateLevel.ToString();
            }
        }
        //删除文档
        void btnDeleteDocument_Click(object sender, EventArgs e)
        {
            IList checkList = GetCheckedList();
            if (checkList.Count < 1)
            {
                MessageBox.Show("请选择要删除的文档！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gridDocument.Focus();
                return;
            }

            if (MessageBox.Show("确认要删除选择文档吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                bool flag = false; //model.Delete(checkList);
                if (colAlterMode.Visible == false)
                {
                    #region 删除工程对象关联文档
                    ObjectQuery oq = new ObjectQuery();
                    Disjunction dis = new Disjunction();

                    foreach (ProObjectRelaDocument relaDoc in checkList)
                    {
                        if (relaDoc.ProjectDocumentVerifyID != null)
                        {
                            dis.Add(Expression.Eq("Id", relaDoc.ProjectDocumentVerifyID));
                        }
                    }

                    oq.AddCriterion(dis);

                    IList docVerify = model.ObjectQuery(typeof(ProjectDocumentVerify), oq);

                    for (int i = 0; i < docVerify.Count; i++)
                    {
                        ProjectDocumentVerify pdv = docVerify[i] as ProjectDocumentVerify;

                        if (pdv.AssociateLevel == ProjectDocumentAssociateLevel.GWBS)
                        {
                            pdv.SubmitState = ProjectDocumentSubmitState.未提交;
                        }
                        else if (pdv.AssociateLevel == ProjectDocumentAssociateLevel.检验批)
                        {
                            ObjectQuery oq0 = new ObjectQuery();
                            oq0.AddCriterion(NHibernate.Criterion.Expression.Eq("ProjectDocumentVerifyID", pdv.Id));
                            IList listDoc = model.ObjectQuery(typeof(ProObjectRelaDocument), oq0);

                            ObjectQuery oq1 = new ObjectQuery();
                            oq1.AddCriterion(NHibernate.Criterion.Expression.Eq("ProjectTask.Id", oprNode.Id));
                            IList il = model.ObjectQuery(typeof(InspectionLot), oq1);

                            if (listDoc.Count < il.Count)
                            {
                                pdv.SubmitState = ProjectDocumentSubmitState.未提交;
                            }
                            else
                            {
                                pdv.SubmitState = ProjectDocumentSubmitState.已提交;
                            }
                        }
                    }

                    model.SaveOrUpdate(docVerify);

                    flag = docModel.DeleteProObjectRelaDocumentAndDocument(checkList);
                    #endregion
                }
                else
                {
                    //删除工程文档验证
                    flag = docModel.DeleteProjectDocumentVerifyAndDocument(checkList);
                }
                if (flag)
                {
                    MessageBox.Show("删除成功！");
                    List<int> index = new List<int>();
                    foreach (DataGridViewRow row in this.gridDocument.Rows)
                    {
                        if ((bool)row.Cells[1].EditedFormattedValue)
                        {
                            index.Add(row.Index);
                        }
                    }
                    int deleteCount = 0;
                    foreach (int rowIndex in index)
                    {
                        gridDocument.Rows.RemoveAt(rowIndex - deleteCount);
                        deleteCount++;
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

        private void InsertIntoGridDocument(ProObjectRelaDocument doc)
        {
            int index = gridDocument.Rows.Add();
            DataGridViewRow row = gridDocument.Rows[index];
            row.Cells[DocumentName.Name].Value = doc.DocumentName;
            //row.Cells[UploadPerson.Name].Value = doc.DocumentOwnerName;
            //row.Cells[UploadDate.Name].Value = doc.SubmitTime.ToString();
            row.Cells[DocumentDesc.Name].Value = doc.DocumentDesc;
            row.Cells[DocumentCateCode.Name].Value = doc.DocumentCateCode;
            row.Cells[DocumentCateName.Name].Value = doc.DocumentCateName;
            row.Cells[DocumentCode.Name].Value = doc.DocumentCode;
            row.Tag = doc;
        }
        private void UpdateDocument(ProObjectRelaDocument doc)
        {
            foreach (DataGridViewRow row in gridDocument.Rows)
            {
                ProObjectRelaDocument docTemp = row.Tag as ProObjectRelaDocument;
                if (docTemp.Id == doc.Id)
                {
                    row.Cells[DocumentName.Name].Value = doc.DocumentName;
                    row.Cells[DocumentDesc.Name].Value = doc.DocumentDesc;

                    row.Tag = doc;

                    gridDocument.CurrentCell = row.Cells[1];
                    break;
                }
            }
        }

        private string GetExceptionMessage(PLMWebServicesByKB.ErrorStack es)
        {
            string msg = es.Message;
            PLMWebServicesByKB.ErrorStack esTemp = es.InnerErrorStack;
            while (esTemp != null && !string.IsNullOrEmpty(esTemp.Message))
            {
                msg += "；\n" + esTemp.Message;
                esTemp = esTemp.InnerErrorStack;
            }

            if ((msg.IndexOf("不能在具有唯一索引") > -1 && msg.IndexOf("插入重复键") > -1) || msg.IndexOf("违反唯一约束") > -1)
            {
                msg = "已存在同名文档，请重命名该文档名称.";
            }

            return msg;
        }
        #endregion 文档操作

        private void SearchIns(GWBSTree tree)
        {
            IList InsList = new ArrayList();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectTask.Id", tree.Id));
            oq.AddOrder(NHibernate.Criterion.Order.Asc("SerialNumber"));
            InsList = model.GetInspectionLot(oq);

            cmbBusinessObject.Items.Clear();

            System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
            li.Text = "工程任务包";
            li.Value = tree.Id;
            cmbBusinessObject.Items.Add(li);
            cmbBusinessObject.Text = "工程任务包";

            if (InsList != null && InsList.Count > 0)
            {
                foreach (InspectionLot lot in InsList)
                {
                    System.Web.UI.WebControls.ListItem li1 = new System.Web.UI.WebControls.ListItem();
                    li1.Text = lot.Name;
                    li1.Value = lot.Id;
                    cmbBusinessObject.Items.Add(li1);
                }
            }
        }

        void rbtnProjectDocument_CheckedChanged(object sender, EventArgs e)
        {
            btnSearch_Click(sender, e);
        }

        void rbtnDocumentStencil_CheckedChanged(object sender, EventArgs e)
        {
            btnSearch_Click(sender, e);
        }

        #endregion

        #region 分层加载工程任务
        /// <summary>
        /// 加载当前节点的子节点
        /// </summary>
        /// <param name="oNode"></param>
        //private void LoadGWBSTree(TreeNode oNode)
        //{
        //    try
        //    {
        //        int iLevel = 1;
        //        string sSysCode = string.Empty;
        //        if (oNode != null)
        //        {
        //            GWBSTree oTree = oNode.Tag as GWBSTree;
        //            iLevel = oTree.Level + 1;
        //            sSysCode = oTree.SysCode;
        //            oNode.Nodes.Clear();
        //        }

        //        IList list = model.GetGWBSTreesByInstance(projectInfo.Id, sSysCode, iLevel);

        //        if (list != null && list.Count > 0)
        //        {
        //            foreach (GWBSTree childNode in list)
        //            {
        //                TreeNode tnTmp = new TreeNode();
        //                tnTmp.Name = childNode.Id.ToString();
        //                tnTmp.Text = childNode.Name;
        //                tnTmp.Tag = childNode;
        //                if (childNode.CategoryNodeType != NodeType.LeafNode)//不为叶节点 就添加一个空节点
        //                {
        //                    tnTmp.Nodes.Add("Test");
        //                }
        //                if (oNode != null)
        //                {
        //                    oNode.Nodes.Add(tnTmp);

        //                }
        //                else
        //                {
        //                    tvwCategory.Nodes.Add(tnTmp);
        //                }
        //            }
        //            this.tvwCategory.SelectedNode = this.tvwCategory.Nodes[0];
        //            //  this.tvwCategory.SelectedNode.Expand();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(e));
        //    }
        //}
        //private void tvwCategory_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Node != null)
        //        {
        //            LoadGWBSTree(e.Node);
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}
        #endregion
        #region SQL查询加载
        private void LoadGWBSTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();
                DataSet dataSet = model.GetGWBSTreesByInstanceSql(projectInfo.Id);
                DataTable table = dataSet.Tables[0];
                IList list = new ArrayList();
                foreach (DataRow dataRow in table.Rows)
                {
                    GWBSTree wbs = new GWBSTree();
                    wbs.Id = dataRow["Id"].ToString();
                    wbs.Name = dataRow["Name"].ToString();
                    wbs.SysCode = dataRow["SysCode"].ToString();
                    wbs.ParentNode = new GWBSTree();
                    wbs.ParentNode.Id = dataRow["parentnodeid"].ToString();
                    list.Add(wbs);
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
                        if (childNode.ParentNode != null && !string.IsNullOrEmpty(childNode.ParentNode.Id))
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

        #endregion

        /// <summary>
        /// 初始化任务节点按钮状态
        /// </summary>
        private void InitWBSNode()
        {



        }
    }
}
