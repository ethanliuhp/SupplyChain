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
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Threading;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Iesi.Collections.Generic;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppProcessUI;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng
{
    public partial class VQuantitiesAffirm : TMasterDetailView
    {
        public MProjectTaskAccount model = new MProjectTaskAccount();

        /// <summary>
        /// ����ڵ㼯��
        /// </summary>
        private List<TreeNode> ListAccountGWBSNodes = new List<TreeNode>();

        /// <summary>
        /// ����ڵ��µ�ͳ�ƽڵ㼯��
        /// </summary>
        private IList ListStatGWBSNodes = new ArrayList();
        private ProjectTaskAccountBill curBillMaster = null;

        /// <summary>
        /// ��ǰ����
        /// </summary>
        public ProjectTaskAccountBill CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        /// <summary>
        /// ��Ҫ��д���ݵ�ȷ����ϸ��
        /// </summary>
        private List<GWBSTaskConfirm> listGWBSTaskConfirms = new List<GWBSTaskConfirm>();

        /// <summary>
        /// ��ǰ��Ŀ
        /// </summary>
        private CurrentProjectInfo projectInfo = null;

        public VQuantitiesAffirm()
        {
            InitializeComponent();
            InitForm();
            RefreshControls(MainViewState.Browser);
        }

        private void InitForm()
        {
            InitEvents();
            projectInfo = StaticMethod.GetProjectInfo();
            DateTime serverTime = model.GetServerTime();
            dtAccountEndDate.Value = serverTime;
        }

        private void SetGridRowReadOnly(bool rowReadOnly)
        {
            //���ñ༭�ĵ�Ԫ��״̬
            foreach (DataGridViewRow row in gridDetail.Rows)
            {
                row.ReadOnly = rowReadOnly;
            }
        }

        private void InitEvents()
        {
            btnSelectAccountTaskRootNode.Click += new EventHandler(btnSelectAccountTaskRootNode_Click);
            btnGeneAccountBill.Click += new EventHandler(btnGeneAccountBill_Click);

            gridDetail.CellDoubleClick += new DataGridViewCellEventHandler(gridDetail_CellDoubleClick);
            gridDetail.CellValidating += new DataGridViewCellValidatingEventHandler(gridDetail_CellValidating);
            gridDetail.CellEndEdit += new DataGridViewCellEventHandler(gridDetail_CellEndEdit);
            btnCHDZH.Click += new EventHandler(btnCHDZH_Click);
            btnClear.Click += new EventHandler(btnClear_Click);
        }

        //���ѡ��ķְ���Ŀ
        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtCHDZH.Tag = null;
            this.txtCHDZH.Text = "";
        }

        //ѡ��ְ���Ŀ
        private void btnCHDZH_Click(object sender, EventArgs e)
        {
            VContractExcuteSelector vmros = new VContractExcuteSelector();
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject engineerMaster = list[0] as SubContractProject;
            this.txtCHDZH.Tag = engineerMaster;
            this.txtCHDZH.Text = engineerMaster.BearerOrgName;
        }

        private void gridDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && gridDetail.Rows[e.RowIndex].ReadOnly == false)
            {
                object value = e.FormattedValue;
                if (value != null)
                {
                    try
                    {
                        string colName = gridDetail.Columns[e.ColumnIndex].Name;
                        if (colName == DtlAccountProjectAmount.Name)
                        {
                            if (value.ToString() != "")
                                ClientUtil.ToDecimal(value);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("�����ʽ����ȷ��");
                        e.Cancel = true;
                    }
                }
            }
        }

        private void gridDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {

                object tempValue = gridDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                string value = "";
                if (tempValue != null)
                    value = tempValue.ToString().Trim();

                DataGridViewRow currEditRow = gridDetail.Rows[e.RowIndex];
                ProjectTaskDetailAccount dtl = currEditRow.Tag as ProjectTaskDetailAccount;

                if (!string.IsNullOrEmpty(dtl.Id))
                {
                    dtl = GetAccountDtlByCurrAccountBill(dtl);
                }

                if (gridDetail.Columns[e.ColumnIndex].Name == DtlAccountProjectAmount.Name) //���㹤����
                {
                    decimal AccountProjectAmount = 0;

                    if (!string.IsNullOrEmpty(value))
                        AccountProjectAmount = ClientUtil.ToDecimal(value);

                    dtl.AccountProjectAmount = AccountProjectAmount;
                    dtl.AccountTotalPrice = AccountProjectAmount*dtl.AccountPrice;

                    // a��ˢ��{����������ϸ����}���������ԣ�
                    //�����κ���������ȡ���=�����㹤������/���ƻ���������
                    //�����κ�����ֵ����=�����㹤������*���ƻ����ۡ�
                    //�����κ�ͬ����ʵ��������=�����κ���������ȡ�*����ͬ��������
                    //�����κ�ͬ����ϼۡ���=�����κ�ͬ����ʵ������*����ͬ���ۡ�
                    //���������γɱ�ʵ��������=�����κ���������ȡ�*�����ι�������
                    //���������γɱ��ϼۡ���=���������γɱ�ʵ������*�����ε��ۡ�
                    if (dtl.AccountProjectAmount == 0 || dtl.PlanQuantity == 0)
                        dtl.CurrAccFigureProgress = 0;
                    else
                        dtl.CurrAccFigureProgress = dtl.AccountProjectAmount/dtl.PlanQuantity;

                    dtl.CurrAccEV = dtl.AccountProjectAmount*dtl.PlanPrice;
                    dtl.CurrContractIncomeQny = dtl.CurrAccFigureProgress*dtl.ContractQuantity;
                    dtl.CurrContractIncomeTotal = dtl.CurrContractIncomeQny*dtl.ContractPrice;
                    dtl.CurrResponsibleCostQny = dtl.CurrAccFigureProgress*dtl.ResponsibleQuantity;
                    dtl.CurrResponsibleCostTotal = dtl.CurrResponsibleCostQny*dtl.ResponsiblePrice;
                        // ToDecimailString(dtl.CurrAccFigureProgress);
                    currEditRow.Cells[DtlAccountTotalPrice.Name].Value = ToDecimailString(dtl.AccountTotalPrice);

                    //b��ˢ������{������Դ���ú���}���������ԣ�
                    //���������������={����������ϸ����}_�����㹤������*�����㶨����������
                    //������ʵ�ֺ�ͬ����������=<����{����������ϸ����}_�����κ���������ȡ�>*����ͬ��������
                    //������ʵ�ֺ�ͬ����ϼۡ���=������ʵ�ֺ�ͬ��������*����ͬ�������ۡ�
                    //���������γɱ���������=<����{����������ϸ����}_�����κ���������ȡ�>*�����κ���������
                    //���������γɱ��ϼۡ���=���������γɱ�������*���������������ۡ�
                    for (int i = 0; i < dtl.Details.Count; i++)
                    {
                        ProjectTaskDetailAccountSubject subject = dtl.Details.ElementAt(i);

                        subject.AccUsageQny = dtl.AccountProjectAmount*subject.AccountQuantity;
                        subject.CurrContractIncomeQny = dtl.CurrAccFigureProgress*subject.ResContractQuantity;
                        subject.CurrContractIncomeTotal = subject.CurrContractIncomeQny*subject.ContractQuantityPrice;
                        subject.CurrResponsibleCostQny = dtl.CurrAccFigureProgress*subject.ResponsibleUsageQny;
                        subject.CurrResponsibleCostTotal = subject.CurrResponsibleCostQny*subject.ResponsibleQnyPrice;
                    }

                    //c���޸Ķ�Ӧ��{����������ϸ�������}_�����㹤��������=���{����������ϸ����}_�����㹤�������ĺ͡�
                    decimal accountQuantityCount = 0;
                    foreach (ProjectTaskDetailAccount item in curBillMaster.Details)
                    {
                        if (item.ProjectTaskDtlGUID.Id == dtl.ProjectTaskDtlGUID.Id)
                        {
                            accountQuantityCount += item.AccountProjectAmount;
                        }
                    }
                }
                else if (gridDetail.Columns[e.ColumnIndex].Name == DtlRemark.Name)
                {
                    dtl.Remark = value;
                }

                gridDetail.Rows[e.RowIndex].Tag = dtl;

            }
        }

        private void gridDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                ProjectTaskDetailAccount dtl = gridDetail.Rows[e.RowIndex].Tag as ProjectTaskDetailAccount;

                if (!string.IsNullOrEmpty(dtl.Id))
                {
                    dtl = GetAccountDtlByCurrAccountBill(dtl);
                }

                VAccountDetailSubject frm = new VAccountDetailSubject(ref dtl);
                frm.OptionView = ViewState;
                frm.ShowDialog();
            }
        }

        private ProjectTaskDetailAccount GetAccountDtlByCurrAccountBill(ProjectTaskDetailAccount dtl)
        {
            for (int i = 0; i < curBillMaster.Details.Count; i++)
            {
                ProjectTaskDetailAccount item = curBillMaster.Details.ElementAt(i) as ProjectTaskDetailAccount;
                if (dtl.Id == item.Id)
                {
                    dtl = item;
                    break;
                }
            }
            return dtl;
        }

        private string ToDecimailString(decimal value)
        {
            return decimal.Round(value, 3).ToString();
        }

        #region �̶�����
        /// <summary>
        /// ����������,(����״̬�����¼������е�����)
        /// </summary>
        /// <param name="code"></param>
        /// <param name="GUID"></param>
        public void Start(string code, string GUID)
        {
            try
            {
                ObjectQuery oq = new ObjectQuery();
                
                if (!string.IsNullOrEmpty(GUID))
                {
                    oq.AddCriterion(Expression.Eq("Id", GUID));
                }
                else
                {
                    return;
                }

                IList list = model.ObjectQuery(typeof (ProjectTaskAccountBill), oq);
                if (list.Count > 0)
                {
                    curBillMaster = list[0] as ProjectTaskAccountBill;
                    ModelToView();
                    RefreshState(MainViewState.Browser);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("��ͼ��������" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// ˢ��״̬(��ť״̬)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshState(MainViewState state)
        {
            base.RefreshState(state);
        }

        #endregion

        /// <summary>
        /// ˢ�¿ؼ�(�����еĿؼ�)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshControls(MainViewState state)
        {
            base.RefreshControls(state);

            switch (state)
            {
                case MainViewState.AddNew:

                    dtAccountEndDate.Enabled = true;
                    txtRemark.ReadOnly = false;
                    txtCreateDate.Enabled = true;
                    btnSelectAccountTaskRootNode.Enabled = true;
                    btnCHDZH.Enabled = true;
                    btnClear.Enabled = true;
                    btnGeneAccountBill.Enabled = true;
                    SetGridRowReadOnly(false);

                    break;
                case MainViewState.Modify:

                    dtAccountEndDate.Enabled = false;
                    txtRemark.ReadOnly = false;
                    txtCreateDate.Enabled = false;
                    btnSelectAccountTaskRootNode.Enabled = false;
                    btnCHDZH.Enabled = false;
                    btnClear.Enabled = false;
                    btnGeneAccountBill.Enabled = false;
                    SetGridRowReadOnly(false);

                    break;
                case MainViewState.Browser:

                    dtAccountEndDate.Enabled = false;
                    txtRemark.ReadOnly = true;
                    txtCreateDate.Enabled = false;
                    btnSelectAccountTaskRootNode.Enabled = false;
                    btnCHDZH.Enabled = false;
                    btnClear.Enabled = false;
                    btnGeneAccountBill.Enabled = false;
                    SetGridRowReadOnly(true);
                    break;
            }

            txtBillCode.ReadOnly = true;
            txtResponsiblePerson.ReadOnly = true;
            txtTheProject.ReadOnly = true;
            txtCompleteProjectAmount.ReadOnly = true;

            ViewState = state;
        }

        //�������
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

            //�Զ���ؼ����
            if (c is CustomEdit || c is TextBox)
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
        /// �½�
        /// </summary>
        /// <returns></returns>
        public override bool NewView()
        {
            try
            {
                base.NewView();
                ClearView();

                curBillMaster = new ProjectTaskAccountBill();
                curBillMaster.CreateDate = model.GetServerTime();
                curBillMaster.DocState = DocumentState.Edit;
                curBillMaster.AccountPersonGUID =
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;
                curBillMaster.AccountPersonName =
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                curBillMaster.OperOrgInfo =
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg;
                curBillMaster.OperOrgInfoName =
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.Name;
                curBillMaster.OpgSysCode =
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.SysCode;

                if (projectInfo != null)
                {
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
                }

                LoadAccountBillData(curBillMaster);
                RefreshControls(MainViewState.AddNew);
                if (string.IsNullOrEmpty(txtAccountRootNode.Text.Trim()))
                {
                    GetWbsRootNode();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "��ʾ", MessageBoxButtons.OK,
                                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            return true;
        }

        /// <summary>
        /// �޸�
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
            if (curBillMaster != null && curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Suspend)
            {
                base.ModifyView();

                return true;
            }
            MessageBox.Show("�˵�״̬Ϊ��" + StaticMethod.GetProjectTaskAccountBillStateText(curBillMaster.DocState) +
                            "���������޸ģ�");
            return false;
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public override bool SaveView()
        {
            try
            {
                return this.SaveOrSubmitBill(1);
            }
            catch (Exception e)
            {
                MessageBox.Show("���ݱ������" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// �ύ
        /// </summary>
        /// <returns></returns>
        public override bool SubmitView()
        {
            try
            {
                return this.SaveOrSubmitBill(2);
            }
            catch (Exception e)
            {
                MessageBox.Show("���ݱ������" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }

        //[optrType=1 ����][optrType=2 �ύ]
        private bool SaveOrSubmitBill(int optrType)
        {
            if (!ValidView())
                return false;

            if (optrType == 2)
            {
                curBillMaster.DocState = DocumentState.InExecute;
            }

            LogData log = new LogData();
            if (string.IsNullOrEmpty(curBillMaster.Id))
            {
                if (optrType == 2)
                {
                    log.OperType = "�����ύ";
                }
                else
                {
                    log.OperType = "��������";
                }
                curBillMaster.Temp1 = "1";//��ʱ�ж���
                curBillMaster = model.SaveAccBillAndSetCfmStateByVirCfmBill(curBillMaster, listGWBSTaskConfirms);
            }
            else
            {
                if (optrType == 2)
                {
                    log.OperType = "�޸��ύ";
                }
                else
                {
                    log.OperType = "�޸ı���";
                }
                curBillMaster = model.SaveOrUpdateProjectTaskAccount(curBillMaster);
            }
            this.txtBillCode.Text = curBillMaster.Code;
            log.BillId = curBillMaster.Id;
            log.BillType = "��������ȷ�ϵ�";
            log.Code = curBillMaster.Code;
            log.Descript = "";
            log.OperPerson =
                Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
            log.ProjectName = curBillMaster.ProjectName;
            StaticMethod.InsertLogData(log);

            this.ViewCaption = ViewName + "-" + txtBillCode.Text;

            MessageBox.Show(log.OperType + "�ɹ�");
            return true;
        }

        /// <summary>
        /// ��������ǰУ������
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            if (gridDetail.CurrentCell != null)
            {
                gridDetail.EndEdit();
                gridDetail_CellEndEdit(gridDetail,
                                       new DataGridViewCellEventArgs(gridDetail.CurrentCell.ColumnIndex,
                                                                     gridDetail.CurrentCell.RowIndex));
            }
            curBillMaster.Remark = this.txtRemark.Text.Trim();
            curBillMaster.CreateDate = txtCreateDate.Value;
            return true;
        }

        //��ʾ����
        private bool ModelToView()
        {
            try
            {
                FlashScreen.Show("���ڼ���ȷ�ϵ���Ϣ,���Ժ�......");

                txtBillCode.Text = curBillMaster.Code;
                dtAccountEndDate.Value = curBillMaster.EndTime;
                txtCreateDate.Value = curBillMaster.CreateDate;
                txtResponsiblePerson.Text = curBillMaster.AccountPersonName;
                txtRemark.Text = curBillMaster.Remark;

                txtAccountRootNode.Text = curBillMaster.AccountTaskName;

                txtTheProject.Text = curBillMaster.ProjectName;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheAccountBill.Id", curBillMaster.Id));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("ProjectTaskDtlGUID.TheCostItem", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("BearerGUID", NHibernate.FetchMode.Eager);

                IList listDetail = model.ObjectQuery(typeof (ProjectTaskDetailAccount), oq);
                //curBillMaster.ListDetails.Clear();
                curBillMaster.Details = new HashedSet<BaseDetail>();
                curBillMaster.Details.AddAll(listDetail.OfType<ProjectTaskDetailAccount>().ToArray());

                gridDetail.Rows.Clear();

                decimal completeProjectQuantity = 0;
                decimal accountProjectQuantity = 0;
                foreach (ProjectTaskDetailAccount dtl in listDetail)
                {
                    AddAccountDetailInGrid(dtl, ref completeProjectQuantity);

                    accountProjectQuantity += dtl.AccountProjectAmount;
                }

                txtCompleteProjectAmount.Text = completeProjectQuantity.ToString();
                oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheAccountBill.Id", curBillMaster.Id));
                oq.AddFetchMode("ProjectTaskDtlGUID", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("ProjectTaskDtlGUID.TheCostItem", NHibernate.FetchMode.Eager);
                IList listSummary = model.ObjectQuery(typeof (ProjectTaskDetailAccountSummary), oq);

                curBillMaster.ListSummary = new HashedSet<ProjectTaskDetailAccountSummary>();
                curBillMaster.ListSummary.AddAll(listSummary.OfType<ProjectTaskDetailAccountSummary>().ToList());

                return true;
            }
            catch (Exception e)
            {
                FlashScreen.Close();

                //throw e;
                MessageBox.Show("��������ʧ�ܣ���ϸ��Ϣ��" + StaticMethod.ExceptionMessage(e));
                return false;
            }
            finally
            {

                FlashScreen.Close();
            }
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                ProjectTaskAccountBill bill =
                    model.GetObjectById(typeof (ProjectTaskAccountBill), curBillMaster.Id) as ProjectTaskAccountBill;
                if (bill.DocState == DocumentState.Edit)
                {
                    IList list = new ArrayList();
                    list.Add(bill);
                    if (!model.DeleteProjectTaskAccount(list))
                        return false;

                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "��������ȷ�ϵ�";
                    log.Code = curBillMaster.Code;
                    log.OperType = "ɾ��";
                    log.Descript = "";
                    log.OperPerson =
                        Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                    log.ProjectName = curBillMaster.ProjectName;
                    StaticMethod.InsertLogData(log);

                    ClearView();

                    curBillMaster = null;

                    return true;
                }
                MessageBox.Show("�˵�״̬Ϊ��" + StaticMethod.GetProjectTaskAccountBillStateText(bill.DocState) + "��������ɾ����");
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("����ɾ������" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public override bool CancelView()
        {
            try
            {
                switch (ViewState)
                {
                    case MainViewState.Modify:

                        if (!string.IsNullOrEmpty(curBillMaster.Id))
                        {
                            //���²�ѯ����

                            ObjectQuery oq = new ObjectQuery();
                            oq.AddCriterion(Expression.Eq("Id", curBillMaster.Id));
                            curBillMaster =
                                model.ObjectQuery(typeof (ProjectTaskAccountBill), oq)[0] as ProjectTaskAccountBill;

                            ModelToView();
                        }
                        else
                        {
                            ClearView();
                        }
                        break;
                    default:
                        ClearView();
                        break;
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("���ݳ�������" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// ˢ��
        /// </summary>
        public override void RefreshView()
        {
            try
            {
                if (ViewState == MainViewState.Modify)
                {
                    if (
                        MessageBox.Show("��ǰȷ�ϵ����ڱ༭״̬����Ҫ�����޸���", "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                        DialogResult.Yes)
                    {
                        if (!SaveView())
                        {
                            return;
                        }
                    }
                }

                //���²�ѯ��������
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", curBillMaster.Id));

                curBillMaster = model.ObjectQuery(typeof (ProjectTaskAccountBill), oq)[0] as ProjectTaskAccountBill;

                ModelToView();

                RefreshControls(MainViewState.Browser);
            }
            catch (Exception e)
            {
                MessageBox.Show("����ˢ�´���" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        private void btnGeneAccountBill_Click(object sender, EventArgs e)
        {
            DateTime accountEndTime = dtAccountEndDate.Value.Date.AddDays(1).AddSeconds(-1);

            if (txtAccountRootNode.Text == "")
            {
                MessageBox.Show("��ѡ��ȷ�ϸ��ڵ㣡");
                btnSelectAccountTaskRootNode.Focus();
                return;
            }

            #region ���ɹ�������ȷ�ϵ�

            FlashScreen.Show("��������ȷ�ϵ�,���Ժ�......");

            #region 1.���ù�������ȷ�ϵ���������

            if (curBillMaster == null)
            {
                curBillMaster = new ProjectTaskAccountBill();
                curBillMaster.CreateDate = model.GetServerTime();

                curBillMaster.DocState = DocumentState.Edit;
                curBillMaster.AccountPersonGUID =
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;
                curBillMaster.AccountPersonName =
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                curBillMaster.OperOrgInfo =
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg;
                curBillMaster.OperOrgInfoName =
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.Name;
                curBillMaster.OpgSysCode =
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.SysCode;

                if (projectInfo != null)
                {
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
                }
            }

            GWBSTree AccountRange = txtAccountRootNode.Tag as GWBSTree;
            curBillMaster.AccountRange = AccountRange;
            curBillMaster.AccountTaskSyscode = AccountRange.SysCode;
            curBillMaster.AccountTaskName = txtAccountRootNode.Text;
            curBillMaster.EndTime = accountEndTime;
            curBillMaster.Remark = txtRemark.Text.Trim();

            if (txtCHDZH.Tag != null)
            {
                curBillMaster.SubContractProjectID = (txtCHDZH.Tag as SubContractProject).Id;
            }

            #endregion

            IList listResult = model.GenAccountBillByVirConfirmBill(curBillMaster);

            string errMes = listResult[2] as string;
            if (!string.IsNullOrEmpty(errMes))
            {
                FlashScreen.Close();
                MessageBox.Show(errMes, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            curBillMaster = listResult[0] as ProjectTaskAccountBill;
            listGWBSTaskConfirms = listResult[1] as List<GWBSTaskConfirm>;
            #endregion

            try
            {                
                ClearAccountBillData();
                LoadAccountBillData(curBillMaster);
                RefreshControls(MainViewState.Modify);

                if (curBillMaster.Details.Count == 0 && curBillMaster.ListSummary.Count == 0)
                {
                    btnSelectAccountTaskRootNode.Enabled = true;
                    btnCHDZH.Enabled = true;
                    btnClear.Enabled = true;
                    btnGeneAccountBill.Enabled = true;
                    dtAccountEndDate.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("����ʧ�ܣ��쳣��Ϣ��" + ExceptionUtil.ExceptionMessage(ex), "������ʾ", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        private void ClearAccountBillData()
        {
            txtBillCode.Text = "";
            txtResponsiblePerson.Text = "";
            txtRemark.Text = "";
            txtTheProject.Text = "";
            gridDetail.Rows.Clear();
            txtCompleteProjectAmount.Text = "";
        }

        private void LoadAccountBillData(ProjectTaskAccountBill bill)
        {
            txtBillCode.Text = bill.Code;
            txtResponsiblePerson.Text = bill.AccountPersonName;
            txtRemark.Text = bill.Remark;
            txtAccountRootNode.Text = bill.AccountTaskName;
            txtTheProject.Text = bill.ProjectName;
            gridDetail.Rows.Clear();

            if (bill.Details.Count > 0)
            {
                decimal completeProjectQuantity = 0;
                decimal accountProjectQuantity = 0;
                foreach (ProjectTaskDetailAccount dtl in bill.Details)
                {
                    AddAccountDetailInGrid(dtl, ref completeProjectQuantity);

                    accountProjectQuantity += dtl.AccountProjectAmount;
                }

                txtCompleteProjectAmount.Text = completeProjectQuantity.ToString();
            }
        }

        private void AddAccountDetailInGrid(ProjectTaskDetailAccount dtl, ref decimal completeProjectQuantity)
        {
            int index = gridDetail.Rows.Add();
            DataGridViewRow row = gridDetail.Rows[index];

            row.Cells[DtlProjectTaskNode.Name].Value = dtl.AccountTaskNodeName;
            row.Cells[DtlProjectTaskNode.Name].ToolTipText =
                Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetCategorTreeFullPath(
                    typeof (GWBSTree), dtl.AccountTaskNodeName, dtl.AccountTaskNodeSyscode);
            //row.Cells[TaskName.Name].ToolTipText = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.TheGWBSTaskName, dtl.TheGWBSSysCode);

            row.Cells[DtlProjectTaskDetail.Name].Value = dtl.ProjectTaskDtlName;
            row.Cells[DtlTaskBearer.Name].Value = dtl.BearerName;
            if (dtl.BearerGUID != null)
                row.Cells[DtlContractName.Name].Value = dtl.BearerGUID.ContractGroupCode;
            row.Cells[DtlOwner.Name].Value = dtl.ResponsiblePersonName;
            row.Cells[DtlMatFeeBalanceFlag.Name].Value = dtl.MatFeeBlanceFlag.ToString();
            row.Cells[DtlPlanWorkAmount.Name].Value = dtl.PlanQuantity;
            row.Cells[DtlAddupAccQuantity.Name].Value = dtl.AddupAccountQuantity;
            row.Cells[DtlAddupAccProgress.Name].Value = Math.Round(dtl.AddupAccountProgress, 2);
            row.Cells[DtlConfirmProjectAmount.Name].Value = dtl.ConfirmQuantity;
            row.Cells[DtlAccountProjectAmount.Name].Value = dtl.AccountProjectAmount;
            row.Cells[DtlQuantityUnit.Name].Value = dtl.QuantityUnitName;
            row.Cells[DtlAccountPrice.Name].Value = dtl.AccountPrice;
            row.Cells[DtlPriceUnit.Name].Value = dtl.PriceUnitName;
            row.Cells[DtlAccountTotalPrice.Name].Value = dtl.AccountTotalPrice;
            row.Cells[DtlRemark.Name].Value = dtl.Remark;

            if (dtl.ProjectTaskDtlGUID != null)
            {
                row.Cells[DtlMainResourceSpec.Name].Value = dtl.ProjectTaskDtlGUID.MainResourceTypeSpec;
                row.Cells[DtlDigramNumber.Name].Value = dtl.ProjectTaskDtlGUID.DiagramNumber;
            }
            row.Tag = dtl;
            completeProjectQuantity += dtl.ConfirmQuantity;
        }
     
        private decimal FigureProgressRound(decimal value)
        {
            return decimal.Round(value, 3);
        }

        private void btnSelectAccountTaskRootNode_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(txtAccountRootNode.Tag as GWBSTree);
            frm.DefaultSelectedGWBS = txtAccountRootNode.Tag as GWBSTree;
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                TreeNode root = frm.SelectResult[0];

                GWBSTree task = root.Tag as GWBSTree;
                if (task != null)
                {
                    txtAccountRootNode.Text = task.Name;
                    txtAccountRootNode.Tag = task;
                }
            }
        }

        private void GetWbsRootNode()
        {
            if (projectInfo == null)
            {
                return;
            }
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
            objectQuery.AddCriterion(Expression.Eq("ParentNode", null));

            var list = model.ObjectQuery(typeof(GWBSTree), objectQuery).OfType<GWBSTree>().ToList();
            if (list == null || list.Count == 0)
            {
                txtAccountRootNode.Clear();
                txtAccountRootNode.Tag = null;
            }
            else
            {
                txtAccountRootNode.Text = list[0].Name;
                txtAccountRootNode.Tag = list[0];
            }
        }
    }
}
