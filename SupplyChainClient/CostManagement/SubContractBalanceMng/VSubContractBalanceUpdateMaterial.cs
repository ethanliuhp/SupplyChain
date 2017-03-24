using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng;
using VirtualMachine.Core;
using NHibernate;
using NHibernate.Criterion;
using System.Collections;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component.Util;
 

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng
{
    public partial class VSubContractBalanceUpdateMaterial : TBasicDataView
    {
        public MProjectTaskAccount model = new MProjectTaskAccount();
        public bool IsRefresh=false;
        /// <summary>
        /// 操作的工程任务明细核算
        /// </summary>
        public ProjectTaskDetailAccount optAccountDtl = null;

        private ProjectTaskDetailAccountSubject optSubject = null;

        public MainViewState OptionView = MainViewState.Modify;

        public VSubContractBalanceUpdateMaterial(ProjectTaskDetailAccount dtl)
        {
          
            optAccountDtl = dtl;

            InitializeComponent();

            InitForm();
        }
      
        private void InitForm()
        {
            InitEvents();
            QueryTaskDetail();
            
           RefreshControls(MainViewState.Browser);
        }
        public void QueryTaskDetail()
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddFetchMode("Details", FetchMode.Eager);
            oQuery.AddCriterion(Expression.Eq("Id", optAccountDtl.Id));
            IList lst = model.ObjectQuery(typeof(ProjectTaskDetailAccount), oQuery);
            optAccountDtl = lst == null || lst.Count == 0 ? null : lst[0] as ProjectTaskDetailAccount;
        }
        private void InitEvents()
        {
            btnSave.Click += new EventHandler(btnSave_Click);
            btnSaveAndExit.Click += new EventHandler(btnSaveAndExit_Click);
            btnCancel.Click += new EventHandler(btnExit_Click);
            this.Load += new EventHandler(VAccountDetailSubject_Load);
            gridDetail.CellEndEdit+=new DataGridViewCellEventHandler(gridDetail_CellEndEdit);
            gridDetail.CellContentClick += new DataGridViewCellEventHandler(gridDetail_CellEndEdit);
        }
        void gridDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                DataGridViewColumn oColumn = gridDetail.Columns[e.ColumnIndex];
                if (oColumn == DtlIsBalance)
                {
                    SetControl();
                }
            }
        }
        void VAccountDetailSubject_Load(object sender, EventArgs e)
        {
            if (optAccountDtl != null)
            {
                foreach (ProjectTaskDetailAccountSubject oSubject in optAccountDtl.Details)
                {
                    AddAccountDetailSubjectInGrid(oSubject);
                }
            }
            if (OptionView == MainViewState.Browser)
            {
                btnSave.Enabled = false;
                btnSaveAndExit.Enabled = false;
            }
            SetControl();
        }
        private void AddAccountDetailSubjectInGrid(ProjectTaskDetailAccountSubject dtl)
        {
            int index = gridDetail.Rows.Add();
            DataGridViewRow row = gridDetail.Rows[index];

            row.Cells[DtlProjectTaskDetail.Name].Value = dtl.TheAccountDetail.ProjectTaskDtlName;
            row.Cells[DtlIsBalance.Name].Value = dtl.IsBalance ? "是" : "否";
            row.Cells[DtlIsBalance.Name].Tag = row.Cells[DtlIsBalance.Name].Value;
            row.Cells[DtlCostName.Name].Value = dtl.BestaetigtCostSubjectName;
            row.Cells[DtlCostAccountSubject.Name].Value = dtl.CostingSubjectName;

            row.Cells[DtlResourceType.Name].Value = dtl.ResourceTypeName;
            row.Cells[DtlResourceTypeQuanlity.Name].Value = dtl.ResourceTypeQuality;
            row.Cells[DtlResourceTypeSpec.Name].Value = dtl.ResourceTypeSpec;
            row.Cells[DtlAccQuotaQuantity.Name].Value = DecimalToString(dtl.AccountQuantity);
            row.Cells[DtlAccQuantityPrice.Name].Value = DecimalToString(dtl.AccountPrice);
            row.Cells[DtlAccProjectAmountPrice.Name].Value = DecimalToString(dtl.AccWorkQnyPrice);
            row.Cells[DtlAccUsageQuantity.Name].Value = DecimalToString(dtl.AccUsageQny);
            row.Cells[DtlAccTotalPrice.Name].Value = DecimalToString(dtl.AccountTotalPrice);

            row.Cells[DtlCurrContractIncomeQny.Name].Value = DecimalToString(dtl.CurrContractIncomeQny);
            row.Cells[DtlCurrIncomeContractTotal.Name].Value = DecimalToString(dtl.CurrContractIncomeTotal);
            row.Cells[DtlCurrResponsibleCostQny.Name].Value = DecimalToString(dtl.CurrResponsibleCostQny);
            row.Cells[DtlCurrResponsibleCostTotal.Name].Value = DecimalToString(dtl.CurrResponsibleCostTotal);

            row.Tag = dtl;
        }
        public string DecimalToString(decimal value)
        {
            return decimal.Round(value, 5).ToString();
        }
        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

   
        //保存
        void btnSave_Click(object sender, EventArgs e)
        {
            SaveSubjectAccount();
        }
        //保存并退出
        void btnSaveAndExit_Click(object sender, EventArgs e)
        {
            if (SaveSubjectAccount())
                this.Close();
        }
        bool SaveSubjectAccount()
        {
            ProjectTaskDetailAccountSubject oTemp = null;
            IEnumerable<DataGridViewRow> lstRow = gridDetail.Rows.OfType<DataGridViewRow>().Where(a => !string.Equals(a.Cells[DtlIsBalance.Name].EditedFormattedValue, a.Cells[DtlIsBalance.Name].Tag));
            IList lstSubject = lstRow.Select(a => a.Tag as ProjectTaskDetailAccountSubject).ToList();
            if (lstSubject.Count > 0)
            {
                foreach (ProjectTaskDetailAccountSubject oSubject in lstSubject)
                {
                    oSubject.IsBalance = !oSubject.IsBalance;
                }
               IList lstResult= model.SaveOrUpdate(lstSubject);
               IsRefresh = true;
               foreach (ProjectTaskDetailAccountSubject oTemp1 in lstResult)
               {
                   foreach (DataGridViewRow oRow in lstRow)
                   {
                       oTemp = oRow.Tag as ProjectTaskDetailAccountSubject;
                       if (oTemp != null)
                       {
                           if (oTemp.Id == oTemp1.Id)
                           {
                               oRow.Tag = oTemp1;
                               oRow.Cells[DtlIsBalance.Name].Tag = oTemp1.IsBalance ? "是" : "否";
                               break;
                           }
                       }
                   }
               }
               SetControl();
               return true;
            }
            else
            {
                MessageBox.Show("没有需要保存确认单/核算单耗用明细");
            }
            return false;
        }
        public void SetControl()
        {
            bool IsExist = gridDetail.Rows.OfType<DataGridViewRow>().ToList().Exists(a => !string.Equals(a.Cells[DtlIsBalance.Name].EditedFormattedValue, a.Cells[DtlIsBalance.Name].Tag));
            btnSave.Enabled = IsExist;
            btnSaveAndExit.Enabled = IsExist;
        }

        /// <summary>
        /// 刷新控件(窗体中的控件)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshControls(MainViewState state)
        {
            switch (state)
            {
                case MainViewState.AddNew:
       
                case MainViewState.Modify:

                    DtlIsBalance.ReadOnly = false;
                    btnSave.Enabled = true;
                    btnSaveAndExit.Enabled = true;
                    break;

                case MainViewState.Browser:

                    DtlIsBalance.ReadOnly = true;
                    btnSave.Enabled = false;
                    btnSaveAndExit.Enabled = false;
                    break;
            }
        }
    }
}
