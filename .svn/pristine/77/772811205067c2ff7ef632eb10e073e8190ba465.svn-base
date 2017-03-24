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
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.Util;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng
{
    public partial class VAccountDetailSubjectSetBalState : TBasicDataView
    {
        public MProjectTaskAccount model = new MProjectTaskAccount();
        public bool IsSave = false;
        private bool IsNeedSure = false;

        IList<ProjectTaskDetailAccountSubject> CurrProjectTaskDetailAccountSubject = null;
        private string MaterialSysCode = TransUtil.MaterialResourceSysCode;//材料的syscode
        /// <summary>
        /// 操作的工程任务明细核算
        /// </summary>
     

        public MainViewState OptionView = MainViewState.Modify;

        public VAccountDetailSubjectSetBalState(IList lstProjectTaskDetailAccount)
        {


            if (lstProjectTaskDetailAccount != null)
            {
                CurrProjectTaskDetailAccountSubject = new List<ProjectTaskDetailAccountSubject>();
                foreach (ProjectTaskDetailAccount oProjectTaskDetailAccount in lstProjectTaskDetailAccount)
                {
                    foreach (ProjectTaskDetailAccountSubject oProjectTaskDetailAccountSubject in oProjectTaskDetailAccount.Details)
                    {
                        CurrProjectTaskDetailAccountSubject.Add(oProjectTaskDetailAccountSubject);
                    }
                }
            }
            InitializeComponent();

            InitForm();
        }
     

        private void InitForm()
        {
            InitEvents();
            gridDetail.ReadOnly = false;
            DtlIsBalance.Items.Add("是");
            DtlIsBalance.Items.Add("否");
            LoadAccountSubjectByAccountDtl(this.CurrProjectTaskDetailAccountSubject);
            SetVisible();
        }
        private void InitEvents()
        {
           this.btnExit.Click+=new EventHandler(btnExit_Click);
            this.btnQuery.Click+=new EventHandler(btnQuery_Click);
            this.btnSure.Click+=new EventHandler(btnSure_Click);
            this.btnMatrailSetNoBal.Click+=new EventHandler(btnMatrailSetNoBal_Click);
            this.btnSureAndExit.Click+=new EventHandler(btnSureAndExit_Click);
            this.gridDetail.CellEndEdit += new DataGridViewCellEventHandler(gridDetail_CellEndEdit);
        }

        private void LoadAccountSubjectByAccountDtl( IList<ProjectTaskDetailAccountSubject> lstShow )
        {
            try
            {
                this.gridDetail.Rows.Clear();
                if (lstShow != null)
                {
                    foreach (ProjectTaskDetailAccountSubject subject in lstShow)
                    {
                        AddAccountDetailSubjectInGrid(subject);
                    }
                    gridDetail.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载数据出错，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        void gridDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (gridDetail.Columns[e.ColumnIndex].Name == this.DtlIsBalance.Name)
            {
                ProjectTaskDetailAccountSubject oProjectTaskDetailAccount = null;
                DataGridViewCell oCell=null;
                bool bFlag = false;
                oCell=gridDetail.Rows[e.RowIndex].Cells[DtlIsBalance.Name];
                oProjectTaskDetailAccount = gridDetail.Rows[e.RowIndex].Tag as ProjectTaskDetailAccountSubject;
                if (oProjectTaskDetailAccount != null)
                {
                    bFlag = ClientUtil.ToString(oCell.EditedFormattedValue) == "是";
                    oCell.Tag = oProjectTaskDetailAccount.IsBalance != bFlag;
                }
                SetVisible();
            }
        }
        void btnExit_Click(object sender, EventArgs e)
        {
            if (this.gridDetail.Rows.OfType<DataGridViewRow>().Where(a => ClientUtil.ToBool(a.Cells[DtlIsBalance.Name].Tag)).Count() > 0)
            {
                if (MessageBox.Show("是否确认修改记录后退出", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    btnSure_Click(null, null);
                    IsSave = true;
                }
            }
            this.Close();
        }

 
        private void AddAccountDetailSubjectInGrid(ProjectTaskDetailAccountSubject dtl)
        {
            int index = gridDetail.Rows.Add();
            DataGridViewRow row = gridDetail.Rows[index];
            row.Cells[DtlProjectTaskDetail.Name].Value = dtl.TheAccountDetail.ProjectTaskDtlName;
            row.Cells[DtlIsBalance.Name].Value = dtl.IsBalance ? "是" : "否";
            row.Cells[DtlCostName.Name].Value = dtl.BestaetigtCostSubjectName;
            row.Cells[DtlCostAccountSubject.Name].Value = dtl.CostingSubjectName;
            row.Cells[DtlResourceType.Name].Value = dtl.ResourceTypeName;
            row.Cells[DtlResourceTypeQuanlity.Name].Value = dtl.ResourceTypeQuality;
            row.Cells[DtlResourceTypeSpec.Name].Value = dtl.ResourceTypeSpec;
            row.Cells[DtlDiagramNumber.Name].Value = dtl.DiagramNumber;
            row.Tag = dtl;
        }
        private void ModelToView(ProjectTaskDetailAccountSubject dtl)
        {
            if (dtl != null)
            {
                DataGridViewRow oRow = gridDetail.Rows.OfType<DataGridViewRow>().Where(a => (a.Tag as ProjectTaskDetailAccountSubject) == dtl).FirstOrDefault();
                if (oRow != null)
                {
                    oRow.Cells[this.DtlIsBalance.Name].Value = dtl.IsBalance ? "是" : "否";
                }
            }
        }
        private void ViewToModel()
        {
            IList lstRows = gridDetail.Rows.OfType<DataGridViewRow>().Where(a => ClientUtil.ToBool(  a.Cells[this.DtlIsBalance.Name].Tag)).ToList() ;
            foreach (DataGridViewRow oRow in lstRows)
            {
                (oRow.Tag as ProjectTaskDetailAccountSubject).IsBalance = ClientUtil.ToString(oRow.Cells[this.DtlIsBalance.Name].Value) == "是";
            }
        }
        void btnQuery_Click(object sender, EventArgs e)
        {
            if (this.CurrProjectTaskDetailAccountSubject != null)
            {
                string sValue=txtTaskName.Text.Trim();
                IList<ProjectTaskDetailAccountSubject> lstShow =this.CurrProjectTaskDetailAccountSubject;
                if (!string.IsNullOrEmpty(sValue))
                {
                    lstShow = this.CurrProjectTaskDetailAccountSubject.Where(a => a.TheAccountDetail.ProjectTaskDtlName.IndexOf(sValue)>-1).ToList();
                }
                LoadAccountSubjectByAccountDtl(lstShow);
                SetVisible();
            }
        }
        void btnMatrailSetNoBal_Click(object sender, EventArgs e)
        {
           
            ProjectTaskDetailAccountSubject oProjectTaskDetailAccountSubject=null;
            DataGridViewCell oCell=null;
            IList lstRows = gridDetail.Rows.OfType<DataGridViewRow>().Where(a => (a.Tag as ProjectTaskDetailAccountSubject).ResourceCategorySysCode.StartsWith(MaterialSysCode)).ToList();
            if (lstRows != null && lstRows.Count>0)
            {
                foreach (DataGridViewRow oRow in lstRows)
                {
                    oProjectTaskDetailAccountSubject = oRow.Tag as ProjectTaskDetailAccountSubject;
                    if (oProjectTaskDetailAccountSubject.IsBalance) this.IsSave = true;
                    oProjectTaskDetailAccountSubject.IsBalance = false;
                    oCell = oRow.Cells[DtlIsBalance.Name];
                    oCell.Value = "否";
                    oCell.Tag = false;
                }
            }
            this.gridDetail.EndEdit();
            SetVisible();
        }
        //保存
        void btnSure_Click(object sender, EventArgs e)
        {
            this.gridDetail.EndEdit();
            //SaveSubjectAccount();
            ViewToModel();
            IsSave = true;
            SetVisible();
        }
        //保存并退出
        void btnSureAndExit_Click(object sender, EventArgs e)
        {
            this.gridDetail.EndEdit();
            ViewToModel();
            IsSave = true;
            this.Close();
        }
        void SetVisible()
        {
            this.gridDetail.EndEdit();
            if (this.CurrProjectTaskDetailAccountSubject == null || this.CurrProjectTaskDetailAccountSubject.Count == 0)
            {
              this.btnSureAndExit.Enabled=this.btnMatrailSetNoBal.Enabled = this.btnQuery.Enabled = this.btnSure.Enabled = false;
            }
            else
            {
                this.btnQuery.Enabled = true;
                this.btnMatrailSetNoBal.Enabled = this.gridDetail.Rows.OfType<DataGridViewRow>().
                    Where(a => (a.Tag as ProjectTaskDetailAccountSubject).ResourceCategorySysCode.StartsWith(this.MaterialSysCode)).
                    Where(a => ClientUtil.ToString(a.Cells[DtlIsBalance.Name].Value) == "是").Count() > 0;
                this.btnSureAndExit.Enabled=this.btnSure.Enabled = this.gridDetail.Rows.OfType<DataGridViewRow>().Where(a => ClientUtil.ToBool(a.Cells[DtlIsBalance.Name].Tag)).Count() > 0;
            }
        }
      
        

       
    }
}
