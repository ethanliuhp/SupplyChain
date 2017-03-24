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


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS
{
    public partial class VProductManageChangeBak : TBasicDataView
    {
        private TreeNode currNode = null;

        private GWBSTree oprNode = null;

        //��Ȩ�޵�ҵ����֯
        private IList lstInstance;
        //Ψһ����
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
        GWBSTaskConfirmMaster conMaster = new GWBSTaskConfirmMaster();

        /// <summary>
        /// ���ƵĶ����ڵ㼯��
        /// </summary>
        private List<TreeNode> listCopyNode = new List<TreeNode>();
        /// <summary>
        /// ���Ƶ������ӽڵ㼯�ϣ��������ѡ��Ľڵ�ʱ�����ҵ����ƵĽڵ�
        /// </summary>
        Dictionary<string, TreeNode> listCopyNodeAll = new Dictionary<string, TreeNode>();

        CurrentProjectInfo projectInfo = null;

        /// <summary>
        /// �Ƿ��ǲ���Ľڵ�
        /// </summary>
        private bool IsInsertNode = false;

        /// <summary>
        /// �Ƿ����ύ(���б���)
        /// </summary>
        private bool IsSubmit = false;

        #region �ĵ���������

        private ProObjectRelaDocument oprDocument = null;

        string fileObjectType = string.Empty;
        string FileStructureType = string.Empty;

        string userName = string.Empty;
        string jobId = string.Empty;

        #endregion �ĵ�����

        public MGWBSTree model;


        string filePath = string.Empty;
        string objecIsGWBS = string.Empty;
        string addOrUpDate = string.Empty;
        //CurrentProjectInfo projectInfo = new CurrentProjectInfo();


        public VProductManageChangeBak(MGWBSTree mot)
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

                FlashScreen.Show("���ڳ�ʼ������,���Ժ�......");

                VBasicDataOptr.InitProfessionCategory(cbSpecialtyClassify, false);
                txtNoDtlColorFlag.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                colMaterialSinge.DataSource = EnumUtil<EnumMaterialFeeSettlementFlag>.GetDescriptions();
                txtStartDate.Value = DateTime.Now.AddMonths(-1);
                tvwCategory.CheckBoxes = false;
                projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                //��ѯ��������ȷ�ϵ�
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                //oq.AddCriterion(Expression.Eq("OperOrgInfo", ConstObject.TheLogin.TheOperationOrgInfo));
                oq.AddCriterion(Expression.Eq("BillType", EnumConfirmBillType.���⹤��));
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
                    conMaster.CreatePerson = ConstObject.LoginPersonInfo;//�Ƶ��˱��
                    conMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;//�Ƶ�������
                    conMaster.CreateYear = ConstObject.LoginDate.Year;//�Ƶ���
                    conMaster.CreateMonth = ConstObject.LoginDate.Month;//�Ƶ���
                    conMaster.CreateDate = ConstObject.LoginDate;
                    conMaster.RealOperationDate = ConstObject.LoginDate;
                    conMaster.BillType = EnumConfirmBillType.���⹤��;
                    conMaster = model.SaveGWBSTaskConfirmMaster(conMaster);
                }
                else
                {
                    conMaster = temp_list[0] as GWBSTaskConfirmMaster;
                }
                InitEvents();
                cbCheckConclusion.Items.AddRange(new object[] { "ͨ��", "��ͨ��" });
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
                cbCheckConclusion1.Items.AddRange(new object[] { "ͨ��", "��ͨ��" });
                cbWBSCheckRequir1.Items.Clear();
                foreach (BasicDataOptr b in list)
                {
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                    li.Text = b.BasicName;
                    li.Value = b.BasicCode;
                    cbWBSCheckRequir1.Items.Add(li);
                }

                //���Ҫ��
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
                //������鱨��
                this.txtInspectionType.Items.AddRange(new object[] { "�ճ����", "��������" });
                this.txtInspectionCnclusion.Items.AddRange(new object[] { "ͨ��", "��ͨ��" });
                this.txtPrintSign.Items.AddRange(new object[] { "�Ѵ�ӡ", "δ��ӡ" });
                this.txtRectReq.Items.AddRange(new object[] { "��������", "��Ҫ���ģ�δ���д���", "��Ҫ���ģ��ѽ��д���" });
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
                FlashScreen.Close();
                MessageBox.Show("��ʼ��ʧ�ܣ���ϸ��Ϣ��" + ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {

                FlashScreen.Close();

            }
        }

        private void InitEvents()
        {
            tvwCategory.MouseDown += new MouseEventHandler(tvwCategory_MouseDown);//����û�и��ڵ�����
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
            //����ĵ�
            btnDownLoadDocument.Click += new EventHandler(btnDownLoadDocument_Click);
            btnOpenDocument.Click += new EventHandler(btnOpenDocument_Click);
            btnUpdateDocument.Click += new EventHandler(btnUpdateDocument_Click);
            btnDeleteDocument.Click += new EventHandler(btnDeleteDocument_Click);
            btnUpFile.Click += new EventHandler(btnUpFile_Click);

            //������鱨��
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnPrintPreview.Click += new EventHandler(btnPrintPreview_Click);
            this.btnPrint.Click += new EventHandler(btnPrint_Click);
            lnkAll.Click += new EventHandler(lnkAll_Click);
            lnkNone.Click += new EventHandler(lnkNone_Click);
            this.dgSearch.CellDoubleClick += new DataGridViewCellEventHandler(dgSearch_CellDoubleClick);
            //�������ᱨ
            this.btnSubmitProject.Click += new EventHandler(btnSubmitProject_Click);
            this.dgBear.CellEndEdit += new DataGridViewCellEventHandler(dgBear_CellEndEdit);
            //this.dgBear.CellValueChanged += new DataGridViewCellEventHandler(dgBear_CellValueChanged);
            this.dgBear.SelectionChanged += new EventHandler(dgBear_SelectionChanged);
            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.dgProject.SelectionChanged += new EventHandler(dgProject_SelectionChanged);
            this.btnDeleteProject.Click += new EventHandler(btnDeleteProject_Click);
            this.btnProject.Click += new EventHandler(btnProject_Click);
            this.btnSelect.Click += new EventHandler(btnSelect_Click);
            this.btnSearch1.Click += new EventHandler(btnSearch1_Click);
            dgBear.CellDoubleClick += new DataGridViewCellEventHandler(dgBear_CellDoubleClick);
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
            btnCHDZH.Click += new EventHandler(btnCHDZH_Click);
            //tabҳ�л����ݴ���
            tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
        }
        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == ����¼��ϸ��Ϣ.Name)//�������ϸ
            {

            }
            else if (tabControl1.SelectedTab.Name == tabPage2.Name)//����������Ϣ
            {

            }
            else if (tabControl1.SelectedTab.Name == ��ظ���.Name)//����ĵ�
            {
                //if (curBillMaster != null && !string.IsNullOrEmpty(curBillMaster.Id))
                {
                    FillDoc();
                }
            }
        }

        void cbCheckConclusion_SelectedIndexChanged(object sendr, EventArgs e)
        {
            if (cbCheckConclusion.SelectedItem.ToString() == "ͨ��")
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

        void dgBear_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //this.dgBear.CellValueChanged -= new DataGridViewCellEventHandler(dgBear_CellValueChanged);
                string colName = dgBear.Columns[e.ColumnIndex].Name;
                if (colName == colQuantityBC.Name)
                {
                    //�����ᱨ
                    this.dgBear.CurrentRow.Cells[colGZHTBL.Name].Value = ClientUtil.ToString(ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colGZHTBL.Name].Value) + ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colQuantityBC.Name].Value) + ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colQuantityBefore.Name].Value));
                    //this.dgBear.CurrentRow.Cells[colXXJDBefore.Name].Value = ClientUtil.ToString(ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colQuantityBefore.Name].Value) / ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) * 100);
                    this.dgBear.CurrentRow.Cells[colBCXXJD.Name].Value = (ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colQuantityBC.Name].Value) / ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) * 100).ToString("0.00");
                    this.dgBear.CurrentRow.Cells[colBCTotalXXJD.Name].Value = (ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colBCXXJD.Name].Value) + ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colXXJDBefore.Name].Value)).ToString("0.00");
                }
                if (colName == colGZHTBL.Name)
                {
                    //�����ۼ��ᱨ������
                    //this.dgBear.CurrentRow.Cells[colQuantityBC.Name].Value = ClientUtil.ToString(ClientUtil.ToDecimal(this.dgBear.CurrentRow.Cells[colGZHTBL.Name].Value) - ClientUtil.ToDecimal(this.dgBear.CurrentRow.Cells[colQuantityBefore.Name].Value));
                    //this.dgBear.CurrentRow.Cells[colXXJDBefore.Name].Value = ClientUtil.ToString(ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colGZHTBL.Name].Value) / ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) * 100);
                    this.dgBear.CurrentRow.Cells[colBCXXJD.Name].Value = ((ClientUtil.ToDecimal(this.dgBear.CurrentRow.Cells[colGZHTBL.Name].Value) - ClientUtil.ToDecimal(this.dgBear.CurrentRow.Cells[colQuantityBefore.Name].Value)) / ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) * 100).ToString("0.00");
                    this.dgBear.CurrentRow.Cells[colBCTotalXXJD.Name].Value = (ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colBCXXJD.Name].Value) + ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colXXJDBefore.Name].Value)).ToString("0.00");
                }
                if (colName == colBCXXJD.Name)
                {
                    //�����ᱨ�������
                    //this.dgBear.CurrentRow.Cells[colQuantityBC.Name].Value 
                    decimal num = ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colBCXXJD.Name].Value) / 100 * ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value);
                    this.dgBear.CurrentRow.Cells[colGZHTBL.Name].Value = ClientUtil.ToString(num + ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colQuantityBefore.Name].Value));
                    //this.dgBear.CurrentRow.Cells[colXXJDBefore.Name].Value = ClientUtil.ToString(ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colQuantityBefore.Name].Value) / ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value)*100);
                    this.dgBear.CurrentRow.Cells[colBCTotalXXJD.Name].Value = (ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colBCXXJD.Name].Value) + ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colXXJDBefore.Name].Value)).ToString("0.00");
                }
                if (colName == colBCTotalXXJD.Name)
                {
                    //�����ۼ��ᱨ�������
                    this.dgBear.CurrentRow.Cells[colBCXXJD.Name].Value = (ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colBCTotalXXJD.Name].Value) - ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colXXJDBefore.Name].Value)).ToString("0.00");
                    //this.dgBear.CurrentRow.Cells[colQuantityBC.Name].Value = (ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) * ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colBCXXJD.Name].Value) / 100).ToString("0.00");
                    decimal num = ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) * ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colBCXXJD.Name].Value) / (decimal)100;
                    this.dgBear.CurrentRow.Cells[colGZHTBL.Name].Value = (num + ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colQuantityBefore.Name].Value)).ToString("0.00");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //this.dgBear.CellValueChanged += new DataGridViewCellEventHandler(dgBear_CellValueChanged);
            }
        }

        void txtKeyWord_TextChanged(object sender, EventArgs e)
        {
            listFindNodes.Clear();
            showFindNodeIndex = 0;
        }


        private void dgBear_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.dgBear.CellEndEdit -= new DataGridViewCellEventHandler(dgBear_CellEndEdit);
                string colName = dgBear.Columns[e.ColumnIndex].Name;
                if (colName == colQuantityBC.Name)
                {
                    //�����ᱨ
                    this.dgBear.CurrentRow.Cells[colGZHTBL.Name].Value = ClientUtil.ToString(ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colGZHTBL.Name].Value) + ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colQuantityBC.Name].Value));
                    //this.dgBear.CurrentRow.Cells[colXXJDBefore.Name].Value = ClientUtil.ToString(ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colQuantityBefore.Name].Value) / ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) * 100);
                    this.dgBear.CurrentRow.Cells[colBCXXJD.Name].Value = ((ClientUtil.ToDecimal(this.dgBear.CurrentRow.Cells[colGZHTBL.Name].Value) - ClientUtil.ToDecimal(this.dgBear.CurrentRow.Cells[colQuantityBefore.Name].Value)) / ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) * 100).ToString("0.00");
                    this.dgBear.CurrentRow.Cells[colBCTotalXXJD.Name].Value = (ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colGZHTBL.Name].Value) / ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) * 100).ToString("0.00");
                }
                if (colName == colGZHTBL.Name)
                {
                    //�����ۼ��ᱨ������
                    //this.dgBear.CurrentRow.Cells[colQuantityBC.Name].Value = ClientUtil.ToString(ClientUtil.ToDecimal(this.dgBear.CurrentRow.Cells[colGZHTBL.Name].Value) - ClientUtil.ToDecimal(this.dgBear.CurrentRow.Cells[colQuantityBefore.Name].Value));
                    this.dgBear.CurrentRow.Cells[colQuantityBC.Name].Value = 0;
                    //this.dgBear.CurrentRow.Cells[colXXJDBefore.Name].Value = ClientUtil.ToString(ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colGZHTBL.Name].Value) / ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) * 100);
                    this.dgBear.CurrentRow.Cells[colBCXXJD.Name].Value = ((ClientUtil.ToDecimal(this.dgBear.CurrentRow.Cells[colGZHTBL.Name].Value) - ClientUtil.ToDecimal(this.dgBear.CurrentRow.Cells[colQuantityBefore.Name].Value)) / ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) * 100).ToString("0.00");
                    //this.dgBear.CurrentRow.Cells[colBCTotalXXJD.Name].Value = (ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colBCXXJD.Name].Value) + ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colXXJDBefore.Name].Value)).ToString("0.00");
                    this.dgBear.CurrentRow.Cells[colBCTotalXXJD.Name].Value = (ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colGZHTBL.Name].Value) / ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) * 100).ToString("0.00");
                }
                if (colName == colBCXXJD.Name)
                {
                    //�����ᱨ�������
                    //this.dgBear.CurrentRow.Cells[colQuantityBC.Name].Value 
                    decimal num = ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colBCXXJD.Name].Value) / 100 * ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value);
                    this.dgBear.CurrentRow.Cells[colQuantityBC.Name].Value = num.ToString();
                    this.dgBear.CurrentRow.Cells[colGZHTBL.Name].Value = ClientUtil.ToString(num + ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colQuantityBefore.Name].Value));
                    //this.dgBear.CurrentRow.Cells[colXXJDBefore.Name].Value = ClientUtil.ToString(ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colQuantityBefore.Name].Value) / ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value)*100);
                    //this.dgBear.CurrentRow.Cells[colBCTotalXXJD.Name].Value = (ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colBCXXJD.Name].Value) + ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colXXJDBefore.Name].Value)).ToString("0.00");
                    this.dgBear.CurrentRow.Cells[colBCTotalXXJD.Name].Value = (ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colGZHTBL.Name].Value) / ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) * 100).ToString("0.00");
                }
                if (colName == colBCTotalXXJD.Name)
                {
                    //�����ۼ��ᱨ�������
                    this.dgBear.CurrentRow.Cells[colBCXXJD.Name].Value = (ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colBCTotalXXJD.Name].Value) - ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colXXJDBefore.Name].Value)).ToString("0.00");
                    //this.dgBear.CurrentRow.Cells[colQuantityBC.Name].Value = (ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) * ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colBCXXJD.Name].Value) / 100).ToString("0.00");
                    this.dgBear.CurrentRow.Cells[colQuantityBC.Name].Value = 0;
                    decimal num = ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) * ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colBCXXJD.Name].Value) / (decimal)100;
                    this.dgBear.CurrentRow.Cells[colGZHTBL.Name].Value = (num + ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colQuantityBefore.Name].Value)).ToString("0.00");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                this.dgBear.CellEndEdit += new DataGridViewCellEventHandler(dgBear_CellEndEdit);
            }
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
                    MessageBox.Show("�ƻ���ʼʱ�䲻�����ڼƻ�����ʱ��!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show("�ƻ���ʼʱ�䲻�����ڼƻ�����ʱ��!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show("ʵ�ʿ�ʼʱ�䲻������ʵ�ʽ���ʱ��!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show("ʵ�ʽ���ʱ�䲻������ʵ�ʿ�ʼʱ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        bool isSelectNodeInvoke = false;//�Ƿ���ѡ��(���)�ڵ�ʱ����
        bool startNodeCheckedState = false;//��shift��ѡ�ֵܽڵ�ʱ��ʼ�ڵ��ѡ��״̬

        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                this.btnUpdate.Enabled = true;
                ClearIns();
                #region ������ڵ�ʱʵ�ֶ�ѡ
                bool isMultiSelect = false;
                TreeNode preselectionNode;//Ԥѡ��ڵ�

                preselectionNode = e.Node;

                if (currNode != null && currNode.Name != preselectionNode.Name
                    && currNode.Parent != null && preselectionNode.Parent != null && currNode.Parent.Name == preselectionNode.Parent.Name)//Nameȡ�Ķ����ID
                    isMultiSelect = true;
                else
                    isMultiSelect = false;

                if (currNode != null)
                    startNodeCheckedState = currNode.Checked;

                if (isMultiSelect && (Control.ModifierKeys & Keys.Shift) != 0 && (Control.ModifierKeys & Keys.Control) != 0)//���ͬʱ������ctrl+shift
                {
                    int currNodeIndex = currNode.Index;
                    int preselectNodeIndex = preselectionNode.Index;

                    int startIndex = currNodeIndex < preselectNodeIndex ? currNodeIndex : preselectNodeIndex;
                    int endIndex = currNodeIndex < preselectNodeIndex ? preselectNodeIndex : currNodeIndex;

                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        TreeNode tn = currNode.Parent.Nodes[i];

                        isSelectNodeInvoke = true;//���ñ�־����check�¼��в��ٴ���

                        if (startNodeCheckedState)//�����ʼ�ڵ㵱ǰΪѡ�У���ȡ��ѡ��
                        {
                            TreeNode tempNode = new TreeNode();
                            tn.BackColor = tempNode.BackColor;
                            tn.ForeColor = tempNode.ForeColor;

                            if (listCheckedNode.ContainsKey(tn.Name))
                                listCheckedNode.Remove(tn.Name);

                            tn.Checked = false;
                        }
                        else//�����ʼ�ڵ㵱ǰΪδѡ�У�������ѡ��
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
                else if (isMultiSelect && (Control.ModifierKeys & Keys.Shift) != 0)//���ͬʱ������shift
                {


                    int currNodeIndex = currNode.Index;
                    int preselectNodeIndex = preselectionNode.Index;

                    int startIndex = currNodeIndex < preselectNodeIndex ? currNodeIndex : preselectNodeIndex;
                    int endIndex = currNodeIndex < preselectNodeIndex ? preselectNodeIndex : currNodeIndex;

                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        TreeNode tn = currNode.Parent.Nodes[i];

                        isSelectNodeInvoke = true;//���ñ�־����check�¼��в��ٴ���

                        if (startNodeCheckedState)//�����ʼ�ڵ㵱ǰΪѡ�У���ȡ��ѡ��
                        {
                            tn.Checked = false;

                            TreeNode tempNode = new TreeNode();
                            tn.BackColor = tempNode.BackColor;
                            tn.ForeColor = tempNode.ForeColor;

                            if (listCheckedNode.ContainsKey(tn.Name))
                                listCheckedNode.Remove(tn.Name);
                        }
                        else//�����ʼ�ڵ㵱ǰΪδѡ�У�������ѡ��
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
                if (oprNode.ProductConfirmFlag)
                {
                    colProjectDetail.Visible = true;
                    colUnit.Visible = true;
                    colDtlState.Visible = true;
                    //�����ڵ�
                    FillProject(oprNode);
                    //realStartDate.Enabled = false;
                    //realStartDate.Enabled = false;
                }
                else
                {
                    //realStartDate.Enabled = true;
                    //realStartDate.Enabled = true;
                    //�������ڵ�
                    IList dtlList = new ArrayList();
                    if (oprNode.Level != 1)
                    {
                        dtlList = model.SearchGWBSDetail(oprNode.SysCode);
                    }
                    dgProject.Rows.Clear();
                    dgBear.Rows.Clear();
                    if (dtlList.Count > 0)
                    {
                        //colProjectDetail.Visible = false;
                        colUnit.Visible = false;
                        colDtlState.Visible = false;
                        foreach (GWBSDetail detail in dtlList)
                        {
                            int rowIndex = dgProject.Rows.Add();
                            dgProject[colProjectDetail.Name, rowIndex].Value = detail.Name;
                            if (detail.TheCostItem != null)
                            {
                                dgProject[colDECode.Name, rowIndex].Value = detail.TheCostItem.QuotaCode;//������
                            }
                            dgProject[colProjectDetail.Name, rowIndex].Value = detail.Name;//����ʱȡ�������ݵĵ�һ����ϸ����
                            dgProject[colMainResType.Name, rowIndex].Value = detail.MainResourceTypeName;
                            dgProject[colSpe.Name, rowIndex].Value = detail.MainResourceTypeSpec;
                            dgProject[colTH.Name, rowIndex].Value = detail.DiagramNumber;
                            dgProject[colPlanProject.Name, rowIndex].Value = detail.PlanWorkAmount;
                            dgProject[colSumProject.Name, rowIndex].Value = detail.QuantityConfirmed;
                            if (detail.PlanWorkAmount != 0)
                            {
                                dgProject[colSumXXJD.Name, rowIndex].Value = (detail.QuantityConfirmed / detail.PlanWorkAmount * 100).ToString("0.00");
                            }
                            dgProject.Rows[rowIndex].Tag = detail;
                        }
                    }
                }
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
                #region ������ѡ�����
                if ((Control.ModifierKeys & Keys.Shift) != 0 || (Control.ModifierKeys & Keys.Control) != 0)//���ͬʱ������Ctrl //(Control.ModifierKeys & Keys.Shift) != 0 || 
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
                if (startNodeCheckedState)//�����ʼ�ڵ㵱ǰΪѡ�У���ȡ��ѡ��
                {
                    TreeNode tempNode = new TreeNode();
                    tn.BackColor = tempNode.BackColor;
                    tn.ForeColor = tempNode.ForeColor;
                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode.Remove(tn.Name);
                    tn.Checked = false;
                }
                else//�����ʼ�ڵ㵱ǰΪδѡ�У�������ѡ��
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
                //������Ϣ
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
                //���Ҫ��
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
                //�ճ����Ҫ��
                txtDayCheckState.Text = StaticMethod.GetCheckStateShowText(oprNode.DailyCheckState);
                txtAcceptanceCheckState.Text = oprNode.AcceptanceCheckState == 0 ? "" : oprNode.AcceptanceCheckState.ToString();
                txtSuperiorCheckState.Text = oprNode.SuperiorCheckState == 0 ? "" : oprNode.SuperiorCheckState.ToString();

                FillSpecial();
                //RefreshControls(MainViewState.Browser);
            }
            catch (Exception exp)
            {
                MessageBox.Show("��ʾ����" + ExceptionUtil.ExceptionMessage(exp));
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
            //������Ϣ
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
                    //MessageBox.Show("�ƻ����ڲ���Ϊ��");
                    //return false;
                }
                else
                {
                    bool validity = true;
                    string temp_quantity = txtPlanTime.Text.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("�ƻ������������֣�");
                        return false;
                    }
                }
                if (txtRealTime.Text == "")
                {
                    //MessageBox.Show("ʵ�ʹ��ڲ���Ϊ��");
                    //return false;
                }
                else
                {
                    bool validity = true;
                    string temp_quantity = txtRealTime.Text.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("ʵ�ʹ����������֣�");
                        return false;
                    }
                }
                //������Ϣ
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
                    MessageBox.Show("������ȸ�ʽ��д����ȷ��");
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
        /// ��֤�ֿ��ʶ�Ƿ���Ϲ淶���ӵ�ǰ�ڵ㵽���ڵ�����нڵ�,�������ӽڵ���ֻ������һ���˱�־��
        /// </summary>
        /// <param name="currWBSNode">��֤��GWBS�ڵ�</param>
        /// <param name="validType">��֤���ͣ�1.������2.�޸ģ�</param>
        /// <param name="fullPath">���ڴ˱�־�ڵ������·��</param>
        /// <returns>true��ʾ������ӣ�false����</returns>
        private bool ValidateWareFlag(GWBSTree currWBSNode, int validType, ref string msg)
        {
            if (currWBSNode == null)
                return false;

            //ע�����ʱ�������ݿ�������ݣ���ֹ�����������ݺ�ʵ�ʲ�һ�����⣩

            ObjectQuery oq = new ObjectQuery();


            //������и��ڵ�Ĵ˱�־
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

                msg = "�ڸ��ڵ㡰" + StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), parentNode.Name, parentNode.SysCode) + "�����Ѿ������ˡ��ֿ��־������.";

                return false;
            }

            if (validType == 2)
            {
                //��������ӽڵ�Ĵ˱�־
                oq.Criterions.Clear();
                oq.AddCriterion(Expression.Eq("WarehouseFlag", true));
                oq.AddCriterion(Expression.Like("SysCode", currWBSNode.SysCode, MatchMode.Start));
                oq.AddCriterion(Expression.Not(Expression.Eq("SysCode", currWBSNode.SysCode)));

                IList listChild = model.ObjectQuery(typeof(GWBSTree), oq);
                if (listChild.Count > 0)
                {
                    GWBSTree childNode = listChild[0] as GWBSTree;

                    msg = "���ӽڵ㡰" + StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), childNode.Name, childNode.SysCode) + "�����Ѿ������ˡ��ֿ��־������.";

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
                MessageBox.Show("�������ݳ���" + ExceptionUtil.ExceptionMessage(e));
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
                    //�����ڵ�
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
                MessageBox.Show("�������ݳ���" + ExceptionUtil.ExceptionMessage(e));
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
                MessageBox.Show("�������ݳ���" + ExceptionUtil.ExceptionMessage(e));
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
            //��Ҫ�鿴 hashtable SpecialHt
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

                    //��ť
                    //��������ؼ�
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

                    break;

                case MainViewState.Browser:

                    //��ť
                    //��������ؼ�
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
                    break;
            }

            ViewState = state;
        }

        #region ������ť
        //����
        void btnSave_Click(object sender, EventArgs e)
        {
            IsSubmit = false;
            mnuTree.Hide();
            if (!SaveView()) return;
            MessageBox.Show("����ɹ���");
            this.RefreshControls(MainViewState.Browser);
        }

        //ѡ�񹤳���������
        void btnSelectWBSType_Click(object sender, EventArgs e)
        {
            VSelectPBSAndTaskType frm = new VSelectPBSAndTaskType();
            frm.IsSingleSelectTaskType = true;

            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("ThePBS", NHibernate.FetchMode.Eager);
            Disjunction dis = new Disjunction();

            TreeNode parentTreeNode = null;
            GWBSTree parentNode = null;

            if (IsInsertNode || ViewState == MainViewState.Modify)//�޸Ľڵ�
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

            //���ó�ʼѡ��Ķ���
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

                    //���Ҫ��
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
                        MessageBox.Show("ϵͳĿǰ�����ڸü�����λ�����飡");
                        SelectUnit(tbUnit);
                    }
                }
            }
            else
                tbUnit.Tag = null;
        }
        private void SelectUnit(TextBox txt)
        {
            StandardUnit su = UCL.Locate("������λά��", StandardUnitExcuteType.OpenSelect) as StandardUnit;
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
            frm.SelectMethod = SelectNodeMethod.��ɢ�ڵ�ѡ��;
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

        //����/��һ��
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

        #region �������ᱨ
        void FillProject(GWBSTree tree)
        {
            //��ӹ�����ȷ�ϵ���Ϣ
            //GWBSTaskConfirmMaster
            dgProject.Rows.Clear();
            dgBear.Rows.Clear();
            ObjectQuery objectquery = new ObjectQuery();
            objectquery.AddCriterion("");
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheGWBS", tree));
            oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
            oq.AddOrder(NHibernate.Criterion.Order.Asc("OrderNo"));
            IList temp_list = model.ObjectQuery(typeof(GWBSDetail), oq);
            if (temp_list.Count > 0)
            {
                foreach (GWBSDetail detail in temp_list)
                {
                    int rowIndex = dgProject.Rows.Add();
                    dgProject[colProjectDetail.Name, rowIndex].Value = detail.Name;
                    if (detail.TheCostItem != null)
                    {
                        dgProject[colDECode.Name, rowIndex].Value = detail.TheCostItem.QuotaCode;//������
                    }
                    dgProject[colMainResType.Name, rowIndex].Value = detail.MainResourceTypeName;
                    dgProject[colSpe.Name, rowIndex].Value = detail.MainResourceTypeSpec;
                    dgProject[colTH.Name, rowIndex].Value = detail.DiagramNumber;
                    dgProject[colDtlState.Name, rowIndex].Value = StaticMethod.GetWBSTaskStateText(detail.State);
                    dgProject[colPlanProject.Name, rowIndex].Value = detail.PlanWorkAmount;
                    dgProject[colSumProject.Name, rowIndex].Value = detail.QuantityConfirmed;
                    dgProject[colSumXXJD.Name, rowIndex].Value = detail.ProgressConfirmed;
                    dgProject[colUnit.Name, rowIndex].Value = detail.WorkAmountUnitName;
                    dgProject[colUnit.Name, rowIndex].Tag = detail.WorkAmountUnitGUID;
                    dgProject.Rows[rowIndex].Tag = detail;
                }
            }
            if (dgProject.Rows.Count > 0)
            {
                dgProject.CurrentCell = dgProject.Rows[0].Cells[0];
                dgProject_SelectionChanged(dgProject, new DataGridViewCellEventArgs(0, 0));
            }
        }

        void dgProject_SelectionChanged(object sender, EventArgs e)
        {
            dgBear.Rows.Clear();
            GWBSDetail gDetail = new GWBSDetail();
            if (dgProject.CurrentRow != null)
            {
                gDetail = dgProject.CurrentRow.Tag as GWBSDetail;
                if (gDetail != null)
                {
                    //if (gDetail.State == DocumentState.InExecute)
                    //{
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Master.Id", conMaster.Id));
                    oq.AddCriterion(Expression.Eq("GWBSDetail.Id", gDetail.Id));
                    oq.AddCriterion(Expression.Eq("Master.BillType", EnumConfirmBillType.���⹤��));//��״̬��=9.���⹤������ȷ�ϵ�
                    oq.AddFetchMode("GWBSDetail", NHibernate.FetchMode.Eager);
                    oq.AddFetchMode("Master", NHibernate.FetchMode.Eager);
                    oq.AddFetchMode("TaskHandler", NHibernate.FetchMode.Eager);
                    oq.AddFetchMode("TaskHandler.TheContractGroup", NHibernate.FetchMode.Eager);

                    IList temp_list = model.ObjectQuery(typeof(GWBSTaskConfirm), oq);

                    if (temp_list.Count > 0)
                    {
                        //GWBSTaskConfirm confirm = temp_list[0] as GWBSTaskConfirm;
                        //this.txtCHDZH.Text = confirm.TaskHandlerName;
                        //this.txtCHDZH.Tag = confirm.TaskHandler;

                        foreach (GWBSTaskConfirm dtl in temp_list)
                        {
                            int rowIndex = dgBear.Rows.Add();
                            dgBear[colGZH.Name, rowIndex].Value = dtl.CreatePersonName;
                            dgBear[colGZH.Name, rowIndex].Tag = dtl.CreatePerson;
                            dgBear[colCHDZH.Name, rowIndex].Value = dtl.TaskHandlerName;
                            if (dtl.TaskHandler != null)
                            {
                                dgBear[colCHDZH.Name, rowIndex].Tag = dtl.TaskHandler;
                                dgBear[colQYName.Name, rowIndex].Value = dtl.TaskHandler.ContractGroupCode;
                            }

                            dgBear[colMaterialSinge.Name, rowIndex].Value = EnumUtil<EnumMaterialFeeSettlementFlag>.GetDescription(dtl.MaterialFeeSettlementFlag);
                            dgBear[colQuantityBefore.Name, rowIndex].Value = dtl.QuantityBeforeConfirm;
                            dgBear[colQuantityBC.Name, rowIndex].Value = 0;//dtl.ActualCompletedQuantity;
                            dgBear[colXXJDBefore.Name, rowIndex].Value = dtl.ProgressBeforeConfirm;
                            dgBear[colGZHTBL.Name, rowIndex].Value = dtl.QuantiyAfterConfirm;
                            dgBear[colBCXXJD.Name, rowIndex].Value = dtl.CompletedPercent;
                            dgBear[colBCTotalXXJD.Name, rowIndex].Value = ClientUtil.ToString(ClientUtil.ToDecimal(dgBear[colBCXXJD.Name, rowIndex].Value) + ClientUtil.ToDecimal(dgBear[colXXJDBefore.Name, rowIndex].Value));
                            dgBear[colLastDate.Name, rowIndex].Value = dtl.RealOperationDate;
                            dgBear[colDescription.Name, rowIndex].Value = dtl.Descript;
                            if (dtl.AccountingState == EnumGWBSTaskConfirmAccountingState.������)
                            {
                                dgBear[colAccState.Name, rowIndex].Value = "������";
                            }
                            if (dtl.AccountingState == EnumGWBSTaskConfirmAccountingState.δ����)
                            {
                                dgBear[colAccState.Name, rowIndex].Value = "δ����";
                            }
                            dgBear.Rows[rowIndex].Tag = dtl;
                            this.dgBear.Rows[rowIndex].ReadOnly = true;
                        }
                    }
                    if (dgBear.Rows.Count > 0)
                    {
                        this.btnUpdate.Enabled = true;
                    }
                    else
                    {
                        confirm = new GWBSTaskConfirm();
                    }
                    // }
                }
            }
        }

        void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgBear.CurrentRow != null)
            {
                GWBSTaskConfirm firm = dgBear.CurrentRow.Tag as GWBSTaskConfirm;
                if (firm.AccountingState != EnumGWBSTaskConfirmAccountingState.������)
                {
                    if (ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colQuantityBefore.Name].Value) != 0)
                    {
                        this.dgBear.CurrentRow.Cells[colMaterialSinge.Name].ReadOnly = true;
                    }
                    else
                    {
                        this.dgBear.CurrentRow.Cells[colMaterialSinge.Name].ReadOnly = false;
                    }
                    this.dgBear.CurrentRow.Cells[colCHDZH.Name].ReadOnly = true;
                    this.btnUpdate.Enabled = false;
                    this.dgBear.CurrentRow.ReadOnly = false;
                }
                else
                {
                    MessageBox.Show("��Ϣ�����в����޸ģ�");
                }
            }
        }

        void dgBear_SelectionChanged(object sender, EventArgs e)
        {
            if (!this.btnUpdate.Enabled)
            {
                foreach (DataGridViewRow var in this.dgBear.Rows)
                {
                    if (var.IsNewRow) break;
                    if (!var.ReadOnly)
                    {
                        if (MessageBox.Show("����Ϣ��δ���棬�Ƿ񱣴棿", "��", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            btnSubmitProject_Click(sender, e);
                            var.ReadOnly = true;
                        }
                        else
                        {
                            GWBSTaskConfirm dtl = var.Tag as GWBSTaskConfirm;
                            var.Cells[colGZH.Name].Value = dtl.CreatePersonName;
                            var.Cells[colGZH.Name].Tag = dtl.CreatePerson;
                            var.Cells[colCHDZH.Name].Value = dtl.TaskHandlerName;
                            var.Cells[colCHDZH.Name].Tag = dtl.TaskHandler;
                            var.Cells[colMaterialSinge.Name].Value = EnumUtil<EnumMaterialFeeSettlementFlag>.GetDescription(dtl.MaterialFeeSettlementFlag);
                            var.Cells[colQuantityBefore.Name].Value = dtl.QuantityBeforeConfirm;
                            var.Cells[colQuantityBC.Name].Value = 0;//dtl.ActualCompletedQuantity;
                            var.Cells[colXXJDBefore.Name].Value = dtl.ProgressBeforeConfirm;
                            var.Cells[colGZHTBL.Name].Value = dtl.QuantiyAfterConfirm;
                            var.Cells[colBCXXJD.Name].Value = dtl.CompletedPercent;
                            var.Cells[colLastDate.Name].Value = dtl.RealOperationDate;
                            var.Cells[colDescription.Name].Value = dtl.Descript;
                            if (dtl.AccountingState == EnumGWBSTaskConfirmAccountingState.������)
                            {
                                var.Cells[colAccState.Name].Value = "������";
                            }
                            if (dtl.AccountingState == EnumGWBSTaskConfirmAccountingState.δ����)
                            {
                                var.Cells[colAccState.Name].Value = "δ����";
                            }
                            var.ReadOnly = true;
                        }
                        this.btnUpdate.Enabled = true;
                        break;
                    }
                }
            }
        }

        void btnAddProject_Click(object sender, EventArgs e)
        {
            if (dgProject.Rows.Count > 0)
            {
                if (dgBear.Rows.Count > 0)
                {
                    if ((dgBear.Rows[dgBear.Rows.Count - 1].Tag as GWBSTaskConfirm).Id != null)
                    {
                        int i = dgBear.Rows.Add();
                        foreach (DataGridViewRow var in this.dgBear.Rows)
                        {
                            for (int j = 0; j < dgBear.Rows.Count - 1; j++)
                            {
                                dgBear.Rows[j].Selected = false;
                            }
                        }
                        dgBear.Rows[i].Selected = true;
                        confirm = new GWBSTaskConfirm();
                        confirm.GWBSDetail = dgProject.CurrentRow.Tag as GWBSDetail;
                        confirm.GWBSDetailName = dgProject.CurrentRow.Cells[colProjectDetail.Name].Value.ToString();
                        dgBear.Rows[i].Tag = confirm;
                    }
                }
                else
                {
                    int i = dgBear.Rows.Add();
                    confirm = new GWBSTaskConfirm();
                    confirm.GWBSDetail = dgProject.CurrentRow.Tag as GWBSDetail;
                    confirm.GWBSDetailName = dgProject.CurrentRow.Cells[colProjectDetail.Name].Value.ToString();
                    dgBear.Rows[i].Tag = confirm;
                }
            }
        }

        void btnSubmitProject_Click(object sender, EventArgs e)
        {
            if (this.dgBear.Rows.Count == 0) return;
            if (this.dgBear.CurrentRow.Cells[colCHDZH.Name].Value == null)
            {
                MessageBox.Show("�е��߲���Ϊ�գ�");
                return;
            }
            if (this.dgBear.CurrentRow.Cells[colMaterialSinge.Name].Value == null)
            {
                MessageBox.Show("�Ϸѽ����ǲ���Ϊ�գ�");
                return;
            }
            if (ClientUtil.ToDecimal(this.dgBear.CurrentRow.Cells[colGZHTBL.Name].Value) < 0)//ClientUtil.ToDecimal(this.dgBear.CurrentRow.Cells[colQuantityBefore.Name].Value))
            {
                MessageBox.Show("�����ۼ��ᱨ������Ӧ��С���㣡");
                return;
            }

            int optRowIndex = -1;
            confirm = new GWBSTaskConfirm();

            foreach (DataGridViewRow var in this.dgBear.Rows)
            {
                if (!var.ReadOnly)
                {
                    optRowIndex = var.Index;
                    break;
                }
            }
            if (optRowIndex == -1)
                return;

            DataGridViewRow optRow = dgBear.Rows[optRowIndex];
            if (optRow.Tag != null)
            {
                confirm = optRow.Tag as GWBSTaskConfirm;
            }
            if (confirm.Id != null)
            {
                //�жϹ��������ᱨ��ϸ״̬
                GWBSTaskConfirm q_dtl = model.GetObjectById(typeof(GWBSTaskConfirm), confirm.Id) as GWBSTaskConfirm;
                if (q_dtl.AccountingState == EnumGWBSTaskConfirmAccountingState.������)
                {
                    MessageBox.Show("���ᱨ��ϸ�����������������У�");
                    return;
                }
                if (q_dtl.QuantityBeforeConfirm != confirm.QuantityBeforeConfirm)
                {
                    MessageBox.Show("���ᱨ��ϸ�����Ѻ��㣬������ˢ���ᱨ��");
                    return;
                }
            }

            //�洢�޸�ǰȷ�ϵ����������ۼ�������ϸ����ȷ����
            confirm.TempData = "";
            if (btnUpdate.Enabled == false)
                confirm.TempData = confirm.ActualCompletedQuantity.ToString();

            confirm.Master = conMaster;
            confirm.CostItem = (dgProject.CurrentRow.Tag as GWBSDetail).TheCostItem;
            confirm.CostItemName = (dgProject.CurrentRow.Tag as GWBSDetail).TheCostItem.Name;
            confirm.WorkAmountUnitId = (dgProject.CurrentRow.Tag as GWBSDetail).WorkAmountUnitGUID;
            confirm.WorkAmountUnitName = (dgProject.CurrentRow.Tag as GWBSDetail).WorkAmountUnitName;

            confirm.TaskHandler = optRow.Cells[colCHDZH.Name].Tag as SubContractProject;
            confirm.TaskHandlerName = ClientUtil.ToString(optRow.Cells[colCHDZH.Name].Value);
            confirm.MaterialFeeSettlementFlag = EnumUtil<EnumMaterialFeeSettlementFlag>.FromDescription(optRow.Cells[colMaterialSinge.Name].Value);
            confirm.ActualCompletedQuantity = ClientUtil.ToDecimal(this.dgBear.CurrentRow.Cells[colGZHTBL.Name].Value) - ClientUtil.ToDecimal(this.dgBear.CurrentRow.Cells[colQuantityBefore.Name].Value);//ClientUtil.ToDecimal(optRow.Cells[colQuantityBC.Name].Value);
            confirm.QuantiyAfterConfirm = ClientUtil.ToDecimal(optRow.Cells[colGZHTBL.Name].Value);
            confirm.ProgressAfterConfirm = ClientUtil.ToDecimal(optRow.Cells[colBCTotalXXJD.Name].Value);
            //confirm.ProgressBeforeConfirm = ClientUtil.ToDecimal(optRow.Cells[colXXJDBefore.Name].Value);
            confirm.CompletedPercent = ClientUtil.ToDecimal(optRow.Cells[colBCXXJD.Name].Value);
            confirm.Descript = optRow.Cells[colDescription.Name].Value == null ? "" : optRow.Cells[colDescription.Name].Value.ToString();
            //У��
            foreach (DataGridViewRow var in this.dgBear.Rows)
            {
                if (var.Index != optRowIndex)
                {
                    GWBSTaskConfirm firm = var.Tag as GWBSTaskConfirm;
                    if (firm.TaskHandler != null && firm.TaskHandler.Id == confirm.TaskHandler.Id
                        && firm.MaterialFeeSettlementFlag == confirm.MaterialFeeSettlementFlag
                        && firm.CreatePerson.Id == confirm.CreatePerson.Id)
                    {
                        MessageBox.Show("�������е��ߡ��Ϸѽ�������ͬ���ᱨ��Ϣ�Ѿ����ڣ����飡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            confirm = model.SaveGWBSTaskConfirm(confirm);
            this.btnUpdate.Enabled = true;
            MessageBox.Show("����ɹ���");
            updateCurrRow();
            dgProject_SelectionChanged(dgProject, new DataGridViewCellEventArgs(0, 0));
            BackSchedule(confirm);
        }

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

        void btnDeleteProject_Click(object sender, EventArgs e)
        {
            if (dgBear.Rows.Count > 0 && dgBear.CurrentRow != null)
            {
                if (MessageBox.Show("ȷ��Ҫɾ����ǰѡ�еļ�¼��", "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    GWBSTaskConfirm firm = dgBear.CurrentRow.Tag as GWBSTaskConfirm;
                    if (firm != null && firm.Id == null)
                    {
                        dgBear.Rows.Remove(dgBear.CurrentRow);
                        return;
                    }
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", firm.Id));
                    oq.AddFetchMode("GWBSDetail", NHibernate.FetchMode.Eager);
                    IList temp_list = model.ObjectQuery(typeof(GWBSTaskConfirm), oq);
                    firm = temp_list[0] as GWBSTaskConfirm;
                    if (firm.AccountingState != EnumGWBSTaskConfirmAccountingState.������ && firm.QuantityBeforeConfirm == 0)
                    {
                        model.DeleteGWBSTaskConfirm(firm);
                        dgBear.Rows.Remove(dgBear.CurrentRow);
                        updateCurrRow();
                    }
                    else
                    {
                        MessageBox.Show("��Ϣ�Ѻ��㣬����ɾ����");
                    }
                }
            }
        }


        void updateCurrRow()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", (dgProject.CurrentRow.Tag as GWBSDetail).Id));
            oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
            IList temp_list = model.ObjectQuery(typeof(GWBSDetail), oq);
            {
                GWBSDetail detail = temp_list[0] as GWBSDetail;
                dgProject.CurrentRow.Cells[colMainResType.Name].Value = detail.MainResourceTypeName;
                dgProject.CurrentRow.Cells[colSpe.Name].Value = detail.MainResourceTypeSpec;
                dgProject.CurrentRow.Cells[colTH.Name].Value = detail.DiagramNumber;
                dgProject.CurrentRow.Cells[colDtlState.Name].Value = StaticMethod.GetWBSTaskStateText(detail.State);
                dgProject.CurrentRow.Cells[colPlanProject.Name].Value = detail.PlanWorkAmount;
                dgProject.CurrentRow.Cells[colSumProject.Name].Value = detail.QuantityConfirmed;
                dgProject.CurrentRow.Cells[colSumXXJD.Name].Value = detail.ProgressConfirmed;
                dgProject.CurrentRow.Cells[colUnit.Name].Value = detail.WorkAmountUnitName;
                dgProject.CurrentRow.Cells[colUnit.Name].Tag = detail.WorkAmountUnitGUID;
                dgProject.CurrentRow.Tag = detail;
            }
            dgProject_SelectionChanged(dgProject, new DataGridViewCellEventArgs(0, 0));
        }


        void btnCHDZH_Click(object sender, EventArgs e)
        {
            VContractExcuteSelector vmros = new VContractExcuteSelector();
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject engineerMaster = list[0] as SubContractProject;
            this.txtCHDZH.Tag = engineerMaster;
            this.txtCHDZH.Text = engineerMaster.BearerOrgName;
        }

        void btnProject_Click(object sender, EventArgs e)
        {
            if (dgProject.Rows.Count > 0)
            {
                if (this.btnUpdate.Enabled == false)
                {
                    if (MessageBox.Show("����Ϣ��δ���棬�Ƿ񱣴棿", "������Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        btnSubmitProject_Click(sender, e);
                        return;
                    }
                    else
                    {
                        dgBear.CurrentRow.ReadOnly = true;
                    }
                }
                DateTime serverTime = model.GetServerTime();
                //�жϽڵ㣬�������ڵ�ſ����½��������ᱨ

                if (ClientUtil.ToDecimal(dgProject.CurrentRow.Cells[colPlanProject.Name].Value) != 0)
                {
                    if ((dgProject.CurrentRow.Tag as GWBSDetail).ProduceConfirmFlag == 1)
                    {
                        if (dgProject.Rows.Count > 0)
                        {
                            if ((dgProject.CurrentRow.Tag as GWBSDetail).State == DocumentState.InExecute)
                            {
                                if (dgBear.Rows.Count > 0)
                                {
                                    if ((dgBear.Rows[dgBear.Rows.Count - 1].Tag as GWBSTaskConfirm).Id != null)
                                    {
                                        int i = dgBear.Rows.Add();
                                        dgBear.ClearSelection();
                                        dgBear.CurrentCell = dgBear.Rows[i].Cells[colGZH.Name];
                                        confirm = new GWBSTaskConfirm();
                                        GWBSDetail dtl = dgProject.CurrentRow.Tag as GWBSDetail;
                                        confirm.GWBSDetail = dtl;
                                        confirm.GWBSDetailName = dtl.Name;
                                        confirm.GWBSTree = oprNode as GWBSTree;
                                        confirm.GWBSTreeName = (oprNode as GWBSTree).Name;
                                        confirm.WorkAmountUnitId = dtl.PriceUnitGUID;
                                        confirm.WorkAmountUnitName = dtl.PriceUnitName;
                                        confirm.AccountingState = EnumGWBSTaskConfirmAccountingState.δ����;
                                        dgBear.CurrentRow.Cells[colAccState.Name].Value = "δ����";
                                        confirm.PlannedQuantity = dtl.PlanWorkAmount;
                                        //���Ϸѽ����ʶ����=<ѡ���б���>_���Ϸѽ����ʶ���� 0.������
                                        //confirm.MaterialFeeSettlementFlag = EnumMaterialFeeSettlementFlag.������;
                                        dgBear.CurrentRow.Cells[colMaterialSinge.Name].Value = "������";
                                        //this.radioN.Checked = true;
                                        confirm.RealOperationDate = DateTime.Now;
                                        confirm.GwbsSysCode = oprNode.SysCode;
                                        this.dgBear.CurrentRow.Cells[colGZH.Name].Tag = ConstObject.LoginPersonInfo;//�Ƶ��˱��
                                        this.dgBear.CurrentRow.Cells[colGZH.Name].Value = ConstObject.LoginPersonInfo.Name;//�Ƶ�������
                                        this.dgBear.CurrentRow.Cells[colLastDate.Name].Value = serverTime.Date.ToShortDateString();
                                        confirm.RealOperationDate = serverTime.Date;
                                        confirm.CreatePerson = ConstObject.LoginPersonInfo;//�Ƶ��˱��
                                        confirm.CreatePersonName = ConstObject.LoginPersonInfo.Name;//�Ƶ�������
                                        confirm.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//��¼ҵ����֯
                                        confirm.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//ҵ����֯����
                                        confirm.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;//ҵ����֯���

                                        if (dtl.WeekScheduleDetail != null)
                                        {
                                            confirm.TaskHandler = dtl.WeekScheduleDetail.SubContractProject;
                                            confirm.TaskHandlerName = dtl.WeekScheduleDetail.SupplierName;
                                            if (confirm.TaskHandler != null)
                                            {
                                                this.dgBear.CurrentRow.Cells[colCHDZH.Name].Value = confirm.TaskHandlerName;
                                                this.dgBear.CurrentRow.Cells[colCHDZH.Name].Tag = confirm.TaskHandler;
                                                this.dgBear.CurrentRow.Cells[colQYName.Name].Value = confirm.TaskHandler.ContractGroupCode;
                                            }
                                        }
                                        else
                                        {
                                            //��ѯ����OBS
                                            ObjectQuery objectQuery = new ObjectQuery();
                                            //objectQuery.AddCriterion(Expression.Eq("ProjectTask.SysCode", oprNode as GWBSTree));
                                            string[] sysCodes = (oprNode as GWBSTree).SysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                                            Disjunction dis = new Disjunction();
                                            for (int q = 0; q < sysCodes.Length; q++)
                                            {
                                                string sysCode = "";
                                                for (int j = 0; j <= q; j++)
                                                {
                                                    sysCode += sysCodes[j] + ".";
                                                }
                                                dis.Add(Expression.Eq("ProjectTask.SysCode", sysCode));
                                            }
                                            objectQuery.AddCriterion(dis);
                                            objectQuery.AddFetchMode("ProjectTask", NHibernate.FetchMode.Eager);
                                            objectQuery.AddFetchMode("SupplierId", NHibernate.FetchMode.Eager);
                                            objectQuery.AddFetchMode("SupplierId.TheContractGroup", NHibernate.FetchMode.Eager);
                                            IList list = model.ObjectQuery(typeof(OBSService), objectQuery);
                                            if (list.Count > 0)
                                            {
                                                OBSService obs = list[0] as OBSService;
                                                confirm.TaskHandler = obs.SupplierId;
                                                confirm.TaskHandlerName = obs.SupplierName;
                                                if (confirm.TaskHandler != null)
                                                {
                                                    this.dgBear.CurrentRow.Cells[colCHDZH.Name].Value = confirm.TaskHandlerName;
                                                    this.dgBear.CurrentRow.Cells[colCHDZH.Name].Tag = confirm.TaskHandler;
                                                    this.dgBear.CurrentRow.Cells[colQYName.Name].Value = confirm.TaskHandler.ContractGroupCode;
                                                }
                                            }
                                        }
                                        if (this.txtCHDZH.Tag != null)
                                        {
                                            SubContractProject subContractProject = this.txtCHDZH.Tag as SubContractProject;
                                            this.dgBear.CurrentRow.Cells[colCHDZH.Name].Value = subContractProject.BearerOrgName;
                                            this.dgBear.CurrentRow.Cells[colCHDZH.Name].Tag = subContractProject;
                                            this.dgBear.CurrentRow.Cells[colQYName.Name].Value = subContractProject.ContractGroupCode;
                                        }
                                        dgBear.Rows[i].Tag = confirm;
                                    }
                                }
                                else
                                {
                                    int i = dgBear.Rows.Add();
                                    confirm = new GWBSTaskConfirm();
                                    GWBSDetail dtl = dgProject.CurrentRow.Tag as GWBSDetail;
                                    confirm.GWBSDetail = dtl;
                                    confirm.GWBSDetailName = dtl.Name;
                                    confirm.GWBSTree = oprNode as GWBSTree;
                                    confirm.GWBSTreeName = (oprNode as GWBSTree).Name;
                                    confirm.WorkAmountUnitId = dtl.PriceUnitGUID;
                                    confirm.WorkAmountUnitName = dtl.PriceUnitName;
                                    confirm.AccountingState = EnumGWBSTaskConfirmAccountingState.δ����;
                                    dgBear.CurrentRow.Cells[colAccState.Name].Value = "δ����";
                                    confirm.PlannedQuantity = dtl.PlanWorkAmount;
                                    //���Ϸѽ����ʶ����=<ѡ���б���>_���Ϸѽ����ʶ���� 0.������
                                    //confirm.MaterialFeeSettlementFlag = EnumMaterialFeeSettlementFlag.������;
                                    dgBear.CurrentRow.Cells[colMaterialSinge.Name].Value = "������";
                                    //this.radioN.Checked = true;
                                    confirm.RealOperationDate = DateTime.Now;
                                    confirm.GwbsSysCode = oprNode.SysCode;
                                    this.dgBear.CurrentRow.Cells[colGZH.Name].Tag = ConstObject.LoginPersonInfo;//�Ƶ��˱��
                                    this.dgBear.CurrentRow.Cells[colGZH.Name].Value = ConstObject.LoginPersonInfo.Name;//�Ƶ�������
                                    confirm.CreatePerson = ConstObject.LoginPersonInfo;//�Ƶ��˱��
                                    confirm.CreatePersonName = ConstObject.LoginPersonInfo.Name;//�Ƶ�������
                                    confirm.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//��¼ҵ����֯
                                    confirm.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//ҵ����֯����
                                    confirm.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;//ҵ����֯���
                                    this.dgBear.CurrentRow.Cells[colLastDate.Name].Value = serverTime.Date.ToShortDateString();
                                    confirm.RealOperationDate = serverTime.Date;
                                    if (dtl.WeekScheduleDetail != null)
                                    {
                                        confirm.TaskHandler = dtl.WeekScheduleDetail.SubContractProject;
                                        confirm.TaskHandlerName = dtl.WeekScheduleDetail.SupplierName;
                                        this.dgBear.CurrentRow.Cells[colCHDZH.Name].Value = confirm.TaskHandlerName;
                                        this.dgBear.CurrentRow.Cells[colCHDZH.Name].Tag = confirm.TaskHandler;
                                        this.dgBear.CurrentRow.Cells[colQYName.Name].Value = confirm.TaskHandler.ContractGroupCode;
                                    }
                                    else
                                    {
                                        //��ѯ����OBS
                                        ObjectQuery objectQuery = new ObjectQuery();
                                        //objectQuery.AddCriterion(Expression.Eq("ProjectTask.SysCode", oprNode as GWBSTree));
                                        string[] sysCodes = (oprNode as GWBSTree).SysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                                        Disjunction dis = new Disjunction();
                                        for (int q = 0; q < sysCodes.Length; q++)
                                        {
                                            string sysCode = "";
                                            for (int j = 0; j <= q; j++)
                                            {
                                                sysCode += sysCodes[j] + ".";
                                            }
                                            dis.Add(Expression.Eq("ProjectTask.SysCode", sysCode));
                                        }
                                        objectQuery.AddCriterion(dis);
                                        objectQuery.AddFetchMode("ProjectTask", NHibernate.FetchMode.Eager);
                                        objectQuery.AddFetchMode("SupplierId", NHibernate.FetchMode.Eager);
                                        objectQuery.AddFetchMode("SupplierId.TheContractGroup", NHibernate.FetchMode.Eager);
                                        IList list = model.ObjectQuery(typeof(OBSService), objectQuery);
                                        if (list.Count > 0)
                                        {
                                            OBSService obs = list[0] as OBSService;
                                            confirm.TaskHandler = obs.SupplierId;
                                            confirm.TaskHandlerName = obs.SupplierName;
                                            this.dgBear.CurrentRow.Cells[colCHDZH.Name].Value = confirm.TaskHandlerName;
                                            this.dgBear.CurrentRow.Cells[colCHDZH.Name].Tag = confirm.TaskHandler;
                                            this.dgBear.CurrentRow.Cells[colQYName.Name].Value = confirm.TaskHandler.ContractGroupCode;
                                        }
                                    }
                                    if (this.txtCHDZH.Tag != null)
                                    {
                                        SubContractProject subContractProject = this.txtCHDZH.Tag as SubContractProject;
                                        this.dgBear.CurrentRow.Cells[colCHDZH.Name].Value = subContractProject.BearerOrgName;
                                        this.dgBear.CurrentRow.Cells[colCHDZH.Name].Tag = subContractProject;
                                        this.dgBear.CurrentRow.Cells[colQYName.Name].Value = subContractProject.ContractGroupCode;
                                    }
                                    dgBear.Rows[i].Tag = confirm;
                                }
                            }
                            else
                            {
                                MessageBox.Show("������ϸδ��������������ᱨ��������Ϣ��");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("�������ڵ㲻�ܽ��й������ᱨ��");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("�ù��������µļƻ��ܹ�����Ϊ0������������");
                    return;
                }
            }
        }

        void dgBear_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (dgBear.Rows[e.RowIndex].ReadOnly == false)
            {
                //˫���е���
                if (this.dgBear.Columns[e.ColumnIndex].Name.Equals(colCHDZH.Name))
                {
                    if (confirm.AccountingState != EnumGWBSTaskConfirmAccountingState.������ && ClientUtil.ToDecimal(dgBear.CurrentRow.Cells[colQuantityBefore.Name].Value) == 0)
                    {
                        DataGridViewRow theCurrentRow = this.dgBear.CurrentRow;
                        VContractExcuteSelector vmros = new VContractExcuteSelector();
                        vmros.ShowDialog();
                        IList list = vmros.Result;
                        if (list == null || list.Count == 0) return;
                        SubContractProject engineerMaster = list[0] as SubContractProject;
                        this.dgBear.CurrentRow.Cells[colCHDZH.Name].Value = engineerMaster.BearerOrgName;
                        this.dgBear.CurrentRow.Cells[colCHDZH.Name].Tag = engineerMaster;
                        this.dgBear.CurrentRow.Cells[colQYName.Name].Value = engineerMaster.ContractGroupCode;
                    }
                    else
                    {
                        MessageBox.Show("�ᱨ��Ϣ�Ѿ���ˣ��е�����Ϣ���ɸ��£�");
                    }
                }
            }
        }

        #endregion

        #region �������

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
                    MessageBox.Show("���½�����Ϣ��δ���棡");
                }
                else
                {
                    AddRow(e);
                }
            }
        }

        void AddRow(ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text.Trim() == "����ճ������Ϣ")
            {
                conMenu.Hide();
                ����¼��ϸ��Ϣ.Parent = tabControl1;
                tabPage2.Parent = null;
                ��ظ���.Parent = null;
                tabControl1.SelectedTab = ����¼��ϸ��Ϣ;
                ��ظ���.Parent = tabControl1;
                if (tvwCategory.SelectedNode == null)
                {
                    MessageBox.Show("����ѡ��һ����������ڵ㣡");
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
                //ͨ��GBWS�ڵ��ѯ����obs��Ϣ�ĳе�����
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
                ����¼��ϸ��Ϣ.Parent = tabControl1;
                tabPage2.Parent = null;
                ��ظ���.Parent = null;
                tabControl1.SelectedTab = ����¼��ϸ��Ϣ;
                ��ظ���.Parent = tabControl1;
            }
            if (e.ClickedItem.Text.Trim() == "�������������Ϣ")
            {
                conMenu.Hide();
                if (tvwCategory.SelectedNode == null)
                {
                    MessageBox.Show("����ѡ��һ����������ڵ㣡");
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
                //ͨ��GBWS�ڵ��ѯ����obs��Ϣ�ĳе�����
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
                ����¼��ϸ��Ϣ.Parent = null;
                ��ظ���.Parent = null;
                tabControl1.SelectedTab = tabPage2;
                ��ظ���.Parent = tabControl1;
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
                            dgDetail[colRecordState.Name, rowIndex].Value = "��Ч";
                        }
                        else
                        {
                            dgDetail[colRecordState.Name, rowIndex].Value = "�༭";
                        }
                    }
                    else
                    {
                        dgDetail[colRecordState.Name, rowIndex].Value = "�༭";
                    }
                    if (Record.InspectType == 1)
                    {
                        dgDetail[colInsType.Name, rowIndex].Value = "�ճ����";
                    }
                    if (Record.InspectType == 2)
                    {
                        dgDetail[colInsType.Name, rowIndex].Value = "��������";
                    }
                    string strDeductionSign = ClientUtil.ToString(Record.DeductionSign);
                    string strCorrectiveSign = ClientUtil.ToString(Record.CorrectiveSign);
                    if (strCorrectiveSign.Equals("0"))
                    {
                        dgDetail[colCorrectiveSign.Name, rowIndex].Value = "��������";
                    }
                    if (strCorrectiveSign.Equals("1"))
                    {
                        dgDetail[colCorrectiveSign.Name, rowIndex].Value = "��Ҫ���ģ�δ���д���";
                    }
                    if (strCorrectiveSign.Equals("2"))
                    {
                        dgDetail[colCorrectiveSign.Name, rowIndex].Value = "��Ҫ���ģ��ѽ��д���";
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
                        //�ճ����
                        //tabControl1
                        ����¼��ϸ��Ϣ.Parent = tabControl1;
                        tabPage2.Parent = null;
                        ��ظ���.Parent = null;
                        tabControl1.SelectedTab = ����¼��ϸ��Ϣ;
                        ��ظ���.Parent = tabControl1;
                        this.EditInspectionRecord();
                    }
                    if (master.InspectType == 2)
                    {
                        //��������
                        tabPage2.Parent = tabControl1;
                        ����¼��ϸ��Ϣ.Parent = null;
                        ��ظ���.Parent = null;
                        tabControl1.SelectedTab = tabPage2;
                        ��ظ���.Parent = tabControl1;
                        this.EditInspectionRecord();
                    }
                    gridDocument.Rows.Clear();
                    if (dgDetail.CurrentRow.Tag == null) return;

                    ObjectQuery oqDoc = new ObjectQuery();
                    oqDoc.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", (dgDetail.CurrentRow.Tag as InspectionRecord).Id));
                    IList listDocument = model.ObjectQuery(typeof(ProObjectRelaDocument), oqDoc);
                    if (listDocument != null && listDocument.Count > 0)
                    {
                        foreach (ProObjectRelaDocument doc in listDocument)
                        {
                            InsertIntoGridDocument(doc);
                        }
                    }
                }
            }
            else
            {
                RefreshControls(MainViewState.Browser);
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
        /// <summary>
        /// �ڱ༭����ʾ��ǰ����¼��ϸ��Ϣ
        /// </summary>
        private void EditInspectionRecord()
        {
            if (tabControl1.SelectedTab.Name.Equals(����¼��ϸ��Ϣ.Name))
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
                            if ((master.DocState == DocumentState.InAudit || master.DocState == DocumentState.InExecute) || master.CreatePerson.Id != ConstObject.LoginPersonInfo.Id)
                            {
                                //��ϢΪ�ύ״̬����Ϣ���ɱ༭
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
                                //��ϢΪ�ύ״̬����Ϣ���ɱ༭
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
                MessageBox.Show("�����û����Ϣ�����ɱ��棡");
                return false;
            }
            if (cbWBSCheckRequir.Text == "")
            {
                MessageBox.Show("��ѡ����רҵ��");
                return false;
            }
            if (txtCheckPerson.Text == "")
            {
                MessageBox.Show("��ѡ�����ˣ�");
                return false;
            }
            if (cbCheckConclusion.Text == "")
            {
                MessageBox.Show("��ѡ������ۣ�");
                return false;
            }
            return true;
        }
        /// <summary>
        /// ��������ճ�����¼
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
                if (strName == "����")
                {
                    master.DocState = DocumentState.Edit;
                }
                else
                {
                    master.DocState = DocumentState.InExecute;
                }
                //�����
                if (txtCheckPerson.Result.Count > 0)
                {
                    master.CreatePerson = txtCheckPerson.Result[0] as PersonInfo;
                    master.CreatePersonName = ClientUtil.ToString(txtCheckPerson.Value);
                }
                master.InspectionSpecial = cbWBSCheckRequir.Text;//���רҵ
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
                master.InspectionConclusion = cbCheckConclusion.Text;//������
                master.InspectionStatus = txtCheckStatus.Text;//�������
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
                //�����ճ�����¼
                master = model.SaveInspectialRecordMaster(master);
                this.dgDetail.Rows[dgDetail.Rows.Count - 1].Tag = master;
                MessageBox.Show(strName + "�ɹ���");
                RefreshControls(MainViewState.Browser);
            }
            catch (Exception ex)
            {
                MessageBox.Show("�������ݴ���" + ex.ToString());
                return;
            }
        }
        void btnSaveInspectionRecord_Click(object sender, EventArgs e)
        {
            //У������
            if (!SetMessage()) return;
            try
            {
                this.SaveInspectionRecord("����");
                FilldgDtail(tvwCategory.SelectedNode.Tag as GWBSTree);
            }
            catch (Exception err)
            {
                MessageBox.Show("���ݱ������" + ExceptionUtil.ExceptionMessage(err));
                return;
            }
        }
        void btnDelete_Click(object sender, EventArgs e)
        {
            bool flag = false;
            if (dgDetail.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("ȷ��Ҫɾ����ǰѡ�еļ�¼��", "ɾ����¼", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
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
                                    MessageBox.Show("�ǵ�¼�˼�����Ϣ������ɾ����");
                                    return;
                                }

                                if (record.DocState == DocumentState.InAudit || record.DocState == DocumentState.InExecute)
                                {
                                    MessageBox.Show("��Ϣ�Ѿ��ύ������ɾ����");
                                    return;
                                }
                                //ɾ����Ϣ
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
                        MessageBox.Show("ɾ���ɹ���");
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
                        SaveInspectionRecord("�ύ");
                        FilldgDtail(tvwCategory.SelectedNode.Tag as GWBSTree);
                    }
                    else
                    {
                        InspectionRecord record = dgDetail.CurrentRow.Tag as InspectionRecord;
                        if (record.Id != null)
                        {
                            if (record.DocState == DocumentState.InExecute)
                            {
                                MessageBox.Show("��Ϣ���ύ��");
                                return;
                            }
                            record.DocState = DocumentState.InExecute;
                            record = model.SaveInspectialRecordMaster(record);
                            MessageBox.Show("�ύ�ɹ���");
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
                MessageBox.Show("��ѡ����רҵ��");
                return false;
            }
            if (txtCheckPerson1.Text == "")
            {
                MessageBox.Show("��ѡ�����ˣ�");
                return false;
            }
            if (cbCheckConclusion1.Text == "")
            {
                MessageBox.Show("��ѡ������ۣ�");
                return false;
            }
            return true;
        }
        /// <summary>
        /// ����������������¼
        /// </summary>
        /// <returns></returns>
        private void SaveInsRecord(string strName)
        {
            try
            {
                InspectionRecord master = dgDetail.CurrentRow.Tag as InspectionRecord;
                if (master.Id == null)
                    master = new InspectionRecord();
                if (strName == "����")
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
                //�����
                if (txtCheckPerson.Result.Count > 0)
                {
                    master.CreatePerson = txtCheckPerson1.Result[0] as PersonInfo;
                    master.CreatePersonName = txtCheckPerson1.Text;
                }
                master.InspectionSpecial = cbWBSCheckRequir1.Text;//���רҵ
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
                master.InspectionConclusion = cbCheckConclusion1.Text;//������
                master.InspectionStatus = txtCheckStatus1.Text;//�������
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
                //�����ճ�����¼
                master = model.SaveInspectialRecordMaster(master);
                this.dgDetail.Rows[dgDetail.Rows.Count - 1].Tag = master;
                MessageBox.Show(strName + "�ɹ���");
                RefreshControls(MainViewState.Browser);
            }
            catch (Exception ex)
            {
                MessageBox.Show("�������ݴ���" + ex.ToString());
                return;
            }
        }

        void btnSave1_Click(object sender, EventArgs e)
        {
            //У������
            if (!SetMessage1()) return;
            try
            {
                this.SaveInsRecord("����");
                FilldgDtail(tvwCategory.SelectedNode.Tag as GWBSTree);
            }
            catch (Exception err)
            {
                MessageBox.Show("���ݱ������" + ExceptionUtil.ExceptionMessage(err));
                return;
            }
        }
        void btnDelete1_Click(object sender, EventArgs e)
        {
            bool flag = false;
            if (dgDetail.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("ȷ��Ҫɾ����ǰѡ�еļ�¼��", "ɾ����¼", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
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
                                    MessageBox.Show("�ǵ�¼�˼�����Ϣ������ɾ����");
                                    return;
                                }

                                if (record.DocState == DocumentState.InAudit || record.DocState == DocumentState.InExecute)
                                {
                                    MessageBox.Show("��Ϣ�Ѿ��ύ������ɾ����");
                                    return;
                                }
                                //ɾ����Ϣ
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
                        MessageBox.Show("ɾ���ɹ���");
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
                        SaveInspectionRecord("�ύ");
                        FilldgDtail(tvwCategory.SelectedNode.Tag as GWBSTree);
                    }
                    else
                    {
                        InspectionRecord record = dgDetail.CurrentRow.Tag as InspectionRecord;
                        if (record.Id != null)
                        {
                            if (record.DocState == DocumentState.InExecute)
                            {
                                MessageBox.Show("��Ϣ���ύ��");
                                return;
                            }
                            record.DocState = DocumentState.InExecute;
                            record = model.SaveInspectialRecordMaster(record);
                            MessageBox.Show("�ύ�ɹ���");
                            RefreshControls(MainViewState.Browser);
                            FilldgDtail(tvwCategory.SelectedNode.Tag as GWBSTree);
                        }
                    }
                }
            }
        }


        #endregion

        #region �ĵ�����
        //�ĵ��ϴ���ť״̬
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
            if (dgDetail.CurrentRow == null || dgDetail.CurrentRow.Tag != null)
            {
                return;
            }
            var curData = dgDetail.CurrentRow.Tag as InspectionRecord;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", curData.Id));
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
        void btnDownLoadDocument_Click(object sender, EventArgs e)
        {
            if (gridDocument.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫ���ص��ĵ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("��ѡ��Ҫ�򿪵��ĵ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + GetExceptionMessage(es), "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (errorList != null && errorList.Count > 0)
                {
                    string str = "";
                    foreach (string s in errorList)
                    {
                        str += (s + ";");
                    }
                    MessageBox.Show(str + "��" + errorList.Count + "���ļ����޷�Ԥ�����ļ������ڻ�δָ����ʽ��");
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


        //�޸��ĵ�
        void btnUpdateDocument_Click(object sender, EventArgs e)
        {
            if (gridDocument.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫ�޸ĵ��ĵ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        //ɾ���ĵ�
        void btnDeleteDocument_Click(object sender, EventArgs e)
        {
            if (gridDocument.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫɾ�����ĵ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gridDocument.Focus();
                return;
            }

            if (MessageBox.Show("ȷ��Ҫɾ��ѡ���ĵ���", "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
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

                PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectDocumentByIRP(docIds.ToArray(), Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentQueryVersion.���а汾,
                    null, userName, jobId, null, out resultProDoc);
                if (es != null)
                {
                    MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + GetExceptionMessage(es), "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                for (int i = 0; i < resultProDoc.Length; i++)
                {
                    PLMWebServices.ProjectDocument doc = resultProDoc[i];
                    doc.State = Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentState.����;
                    proDocList.Add(doc);
                }

                PLMWebServices.ErrorStack es1 = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.UpdateDocumentByIRP(proDocList.ToArray(), Application.Business.Erp.SupplyChain.Client.PLMWebServices.DocumentUpdateMode.���һ���°���ļ�,
                    null, userName, jobId, null, out reultProdocList);
                if (es1 != null)
                {
                    MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + GetExceptionMessage(es), "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                bool flag = model.DeleteProObjRelaDoc(relaDocList);
                if (flag)
                {
                    MessageBox.Show("ɾ���ɹ���");
                    foreach (DataGridViewRow row in gridDocument.SelectedRows)
                    {
                        gridDocument.Rows.RemoveAt(row.Index);
                    }
                }
                else
                {
                    MessageBox.Show("ɾ��ʧ�ܣ�");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ɾ��ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex), "������ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //�����ϴ�
        void btnUpFile_Click(object sender, EventArgs e)
        {
            if (dgDetail.CurrentRow == null)
            {
                MessageBox.Show("û��ѡ�е���Ϣ");
                return;
            }
            InspectionRecord curBillMaster = dgDetail.CurrentRow.Tag as InspectionRecord;
            if (curBillMaster.Id == null)
            {
                return;
                //if (MessageBox.Show("��ǰҵ�����û���棬�Ƿ񱣴棡", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                //{
                //    try
                //    {
                //        if (tabControl1.SelectedTab.Name.Equals(��ظ���.Name))
                //        {

                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        MessageBox.Show("���ݱ������" + ExceptionUtil.ExceptionMessage(ex));
                //    }
                //}
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
                msg += "��\n" + esTemp.Message;
                esTemp = esTemp.InnerErrorStack;
            }

            if (msg.IndexOf("�����ھ���Ψһ����") > -1 && msg.IndexOf("�����ظ���") > -1)
            {
                msg = "�Ѵ���ͬ���ĵ��������������ĵ�����.";
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

        #region ��������ӡ
        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                FlashScreen.Show("���ڲ�ѯ��Ϣ......");
                ObjectQuery oq = new ObjectQuery();
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                string strCodes = (tvwCategory.SelectedNode.Tag as GWBSTree).SysCode;
                oq.AddCriterion(Expression.Like("GWBSTreeSysCode", strCodes, MatchMode.Start));
                //}
                //�е�����
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
                if (txtInspectionType.Text != "")//�������
                {
                    if (txtInspectionType.Text == "�ճ����")
                    {
                        oq.AddCriterion(Expression.Eq("InspectType", 1));
                    }
                    if (txtInspectionType.Text == "��������")
                    {
                        oq.AddCriterion(Expression.Eq("InspectType", 2));
                    }
                }
                if (txtRectReq.Text != "")//���ı�־
                {
                    if (txtRectReq.Text == "��������")
                    {
                        oq.AddCriterion(Expression.Eq("CorrectiveSign", 0));
                    }
                    if (txtRectReq.Text == "��Ҫ���ģ�δ���д���")
                    {
                        oq.AddCriterion(Expression.Eq("CorrectiveSign", 1));
                    }
                    if (txtRectReq.Text == "��Ҫ���ģ��ѽ��д���")
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
                            dgSearch[colInspectionSpecial.Name, rowIndex].Value = record.InspectionSpecial;//���רҵ
                            dgSearch[colBInspectionPerson.Name, rowIndex].Value = record.BearTeamName;//�������
                            dgSearch[colInspectionConclusion.Name, rowIndex].Value = record.InspectionConclusion;//������
                            dgSearch[colInspectionDescript.Name, rowIndex].Value = record.InspectionStatus;//������
                            dgSearch[colInspectionPerson.Name, rowIndex].Value = record.CreatePersonName;//�����

                            dgSearch[colInspectionType.Name, rowIndex].Value = record.InspectType;//�������
                            string strInspectType = ClientUtil.ToString(record.InspectType);//�������
                            if (strInspectType.Equals("1"))
                            {
                                dgSearch[colInspectionType.Name, rowIndex].Value = "�ճ����";
                            }
                            if (strInspectType.Equals("2"))
                            {
                                dgSearch[colInspectionType.Name, rowIndex].Value = "��������";
                            }
                            dgSearch[colInsDate.Name, rowIndex].Value = record.CreateDate.ToShortDateString();//���ʱ��
                            object objState = record.DocState;
                            if (objState != null)
                            {
                                dgSearch[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName((record.DocState));
                            }
                            string strCorrectiveSign = ClientUtil.ToString(record.CorrectiveSign);//���ı�־
                            if (strCorrectiveSign.Equals("0"))
                            {
                                dgSearch[colRectifState.Name, rowIndex].Value = "��������";
                            }
                            if (strCorrectiveSign.Equals("1"))
                            {
                                dgSearch[colRectifState.Name, rowIndex].Value = "��Ҫ���ģ�δ���д���";
                            }
                            if (strCorrectiveSign.Equals("2"))
                            {
                                dgSearch[colRectifState.Name, rowIndex].Value = "��Ҫ���ģ��ѽ��д���";
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
                MessageBox.Show("��ѯ���ݳ���\n" + ex.Message);
            }
            finally
            {
                FlashScreen.Close();
            }
        }
        void btnPrint_Click(object sender, EventArgs e)
        {
            if (LoadTempleteFile(@"�ճ�����¼.flx") == false) return;
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
                MessageBox.Show("��ѡ������飡");
                return;
            }
            if (LoadTempleteFile(@"�ճ�����¼.flx") == false) return;
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
                //�����ʽ������
                flexGrid1.OpenFile(path + "\\" + modelName);//�����ʽ
            }
            else
            {
                MessageBox.Show("δ�ҵ�ģ���ʽ�ļ���" + modelName + "��");
                return false;
            }
            return true;
        }

        private void FillFlex()
        {
            //int detailStartRowNumber = 5;//5Ϊģ���е��к�
            //int detailCount = 0;
            //foreach (DataGridViewRow var in this.dgSearch.Rows)
            //{
            //    if ((bool)var.Cells[colSelect.Name].Value)
            //    {
            //        detailCount++;
            //    }
            //}
            ////������ϸ��
            //flexGrid1.InsertRow(detailStartRowNumber, detailCount - 1);
            ////���õ�Ԫ��ı߿򣬶��뷽ʽ
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

            //            string strInspectType = ClientUtil.ToString(rec.InspectType);//�������
            //            if (strInspectType.Equals("1"))
            //            {
            //                flexGrid1.Cell(detailStartRowNumber, 1).Text = "�ճ����";
            //            }
            //            if (strInspectType.Equals("2"))
            //            {
            //                flexGrid1.Cell(detailStartRowNumber, 1).Text = "��������";
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
            //            string strCorrectiveSign = ClientUtil.ToString(rec.CorrectiveSign);//���ı�־
            //            if (strCorrectiveSign.Equals("0"))
            //            {
            //                flexGrid1.Cell(detailStartRowNumber, 6).Text = "��������";
            //            }
            //            if (strCorrectiveSign.Equals("1"))
            //            {
            //                flexGrid1.Cell(detailStartRowNumber, 6).Text = "��Ҫ���ģ�δ���д���";
            //            }
            //            if (strCorrectiveSign.Equals("2"))
            //            {
            //                flexGrid1.Cell(detailStartRowNumber, 6).Text = "��Ҫ���ģ��ѽ��д���";
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
                        person += "��" + ic.CreatePersonName;
                        flexGrid1.Cell(6, 2).Text = person;
                        flexGrid1.Cell(6, 2).WrapText = true;
                    }
                }
                if (i == 0)
                {
                    flexGrid1.Cell(7, 1).Text = "����¼��" + "\r\n" + "\r\n" + "  " + (i + 1) + ". " + ic.GWBSTreeName + "," + ic.InspectionStatus + "\r\n";
                }
                else
                {
                    flexGrid1.Cell(7, 1).Text += "  " + (i + 1) + ". " + ic.GWBSTreeName + "," + ic.InspectionStatus + "\r\n";
                }
                flexGrid1.Cell(7, 1).WrapText = true;
            }
        }

        /// <summary>
        /// ȫѡ
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
        /// ��ѡ
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
