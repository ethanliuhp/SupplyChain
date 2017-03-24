using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatApplyMng.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.WasteMaterialMng.WasteMaterialManage
{
    public partial class VWasteMaterialApplyQuery : TBasicDataView
    {
        private MWasteMaterialMng model = new MWasteMaterialMng();
        public VWasteMaterialApplyQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }        

        private void InitData()
        {
            comMngType.Items.Clear();
            Array tem = Enum.GetValues(typeof(DocumentState));
            for (int i = 0; i < tem.Length; i++)
            {
                DocumentState s = (DocumentState)tem.GetValue(i);
                int k = (int)s;
                if (k != 0)
                {
                    string strNewName = ClientUtil.GetDocStateName(k);
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                    li.Text = strNewName;
                    li.Value = k.ToString();
                    comMngType.Items.Add(li);
                }
            }
            this.dtpDateBegin.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }       

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
           this.dgMaster.CellContentClick += new DataGridViewCellEventHandler(dgMaster_CellContentClick);
            this.btnSearchGWBS.Click += new EventHandler(btnSearchGWBS_Click);
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.btnSearchBill.Click += new EventHandler(btnSearchBill_Click);
            this.dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
        }
        void dgMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgMaster[e.ColumnIndex, e.RowIndex];
            if (dgvCell.OwningColumn.Name == colCodeMaster.Name)
            {
                //DailyPlanMaster master = model.DailyPlanSrv.GetDailyPlanByCode(dgvCell.Value.ToString());
                WasteMatApplyMaster master = dgMaster.Rows[e.RowIndex].Tag as WasteMatApplyMaster;
                VWasteMaterialApply vmro = new VWasteMaterialApply();
                vmro.CurBillMaster = master;
                vmro.Preview();
            }
        }
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(this.colCode.Name))
            {
                return;
            }
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = dgDetail.Rows[e.RowIndex].Tag as string;
                if (ClientUtil.ToString(billId) != "")
                {
                    VWasteMaterialApply vOrder = new VWasteMaterialApply();
                    vOrder.Start(billId);
                    vOrder.ShowDialog();
                }
            }
        }

        void btnSearchGWBS_Click(object sender,EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            List<TreeNode> list = frm.SelectResult;
            if (list.Count > 0)
            {
                this.txtGWBS.Tag = (list[0] as TreeNode).Tag as GWBSTree;
                this.txtGWBS.Text = (list[0] as TreeNode).Text;
            }
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgDetail[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colCode.Name)
            {
                WasteMatApplyMaster master = model.WasteMatSrv.GetWasteMatApplyByCode(ClientUtil.ToString(dgvCell.Value));
                VWasteMaterialApply vWasteApply = new VWasteMaterialApply();
                vWasteApply.CurBillMaster = master;
                vWasteApply.Preview();
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                #region 查询条件处理
                string condition = "";
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                condition += "and t1.ProjectId = '" + projectInfo.Id + "'";
                //单据号
                if (this.txtCodeBegin.Text != "")
                {
                    if (this.txtCodeBegin.Text == this.txtCodeEnd.Text)
                    {
                        condition = condition + "and t1.Code like '%" + this.txtCodeBegin.Text + "%'";

                    }
                    else
                    {
                        condition = condition + " and t1.Code between '" + this.txtCodeBegin.Text + "' and '" + this.txtCodeEnd.Text + "'";
                    }
                }
                //业务日期
                if (StaticMethod.IsUseSQLServer())
                {
                    condition += " and t1.CreateDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and t1.CreateDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
                }
                else
                {
                    condition += " and t1.CreateDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
                }
                //制单人
                if (!txtCreatePerson.Text.Trim().Equals("") && txtCreatePerson.Result != null && txtCreatePerson.Result.Count > 0)
                {
                    condition += " and t1.CreatePerson='" + (txtCreatePerson.Result[0] as PersonInfo).Id + "'";
                }

                //物资
                if (this.txtMaterial.Text != "")
                {
                    condition = condition + " and t2.MaterialName like '%" + this.txtMaterial.Text + "%'";
                }
                //规格型号
                if (this.txtSpec.Text != "")
                {
                    condition = condition + " and t2.MaterialSpec like '%" + this.txtSpec.Text + "%'";
                }
                if (this.txtGWBS.Tag != null)
                {
                    condition += "and t2.UsedPart like '%" + (txtGWBS.Tag as GWBSTree).Id + "%'";
                }
                if (comMngType.Text != "")
                {
                    System.Web.UI.WebControls.ListItem li = comMngType.SelectedItem as System.Web.UI.WebControls.ListItem;
                    int values = ClientUtil.ToInt(li.Value);
                    condition += "and t1.State= '" + values + "'";
                }
                #endregion
                DataSet dataSet = model.WasteMatSrv.WasteMatApplyQuery(condition);
                this.dgDetail.Rows.Clear();

                DataTable dataTable = dataSet.Tables[0];
                //if (dataTable == null || dataTable.Rows.Count == 0) return;
                decimal sumQuantity = 0;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int rowIndex = this.dgDetail.Rows.Add();

                    DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                    currRow.Tag = ClientUtil.ToString(dataRow["id"]);

                    dgDetail[colCode.Name, rowIndex].Value = dataRow["code"];
                    object objState = dataRow["State"];
                    if (objState != null)
                    {
                        dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                    }
                    dgDetail[colMaterialCode.Name, rowIndex].Value = dataRow["MaterialCode"].ToString();
                    dgDetail[colMaterialName.Name, rowIndex].Value = dataRow["MaterialName"].ToString();
                    dgDetail[colSpec.Name, rowIndex].Value = dataRow["MaterialSpec"].ToString();
                    object quantity = dataRow["Quantity"];
                    if (quantity != null)
                    {
                        sumQuantity += decimal.Parse(quantity.ToString());
                    }
                    dgDetail[colQuantity.Name, rowIndex].Value = quantity;
                    dgDetail[colGradeNo.Name, rowIndex].Value = dataRow["Grade"].ToString();
                    dgDetail[colGWBS.Name, rowIndex].Value = dataRow["UsedPartName"].ToString();
                    dgDetail[colCreatePerson.Name, rowIndex].Value = dataRow["CreatePersonName"].ToString();
                    dgDetail[colRealOperationDate.Name, rowIndex].Value = dataRow["RealOperationDate"].ToString();
                    string b = dataRow["CreateDate"].ToString();
                    string[] bArray = b.Split(' ');
                    string strb = bArray[0];
                    dgDetail[colCreateDate.Name, rowIndex].Value = strb;
                    string a = dataRow["ApplyDate"].ToString();
                    string[] sArray = a.Split(' ');
                    string stra = sArray[0];
                    dgDetail[colApplyDate.Name, rowIndex].Value = stra;
                    dgDetail[colDescript.Name, rowIndex].Value = dataRow["Descript"].ToString();
                    dgDetail[colMatStandardUnitName.Name, rowIndex].Value = dataRow["MatStandardUnitName"].ToString();
                    dgDetail[colDiagramNum.Name, rowIndex].Value = dataRow["DiagramNumber"].ToString();
                    dgDetail[colPrintTimes.Name, rowIndex].Value = dataRow["PrintTimes"].ToString();
                }
                this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
        }

        void btnSearchBill_Click(object sender, EventArgs e)
        {
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            if (this.txtCodeBeginBill.Text != "")
            {
              
                    oq.AddCriterion(Expression.Like("Code", this.txtCodeBeginBill.Text, MatchMode.Start));
            }
            oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBeginBill.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEndBill.Value.AddDays(1).Date));
            if (this.txtCreatePersonBill.Text != "")
            {
                oq.AddCriterion(Expression.Eq("CreatePersonName", this.txtCreatePersonBill.Text));
            }
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            IList list = model.WasteMatSrv.ObjectQuery(typeof(WasteMatApplyMaster), oq);
            dgMaster.Rows.Clear();
            dgDetailBill.Rows.Clear();
            ShowMasterList(list);
        }

        void ShowMasterList(IList list)
        {
            if (list == null || list.Count == 0) return;
            foreach (WasteMatApplyMaster master in list)
            {
                int i = this.dgMaster.Rows.Add();
                this.dgMaster[colCodeMaster.Name, i].Value = master.Code;
                this.dgMaster[colStateMaster.Name, i].Value = ClientUtil.GetDocStateName(master.DocState);
                this.dgMaster[colCreateDateMaster.Name, i].Value = master.CreateDate.ToShortDateString();
                this.dgMaster[colRealOperationDateMaster.Name, i].Value = master.RealOperationDate.ToShortDateString();
                this.dgMaster[colCreatePersonMaster.Name, i].Tag = master.CreatePerson;
                this.dgMaster[colCreatePersonMaster.Name, i].Value = master.CreatePersonName;
                this.dgMaster[colDescriptMaster.Name, i].Value = master.Descript;
                this.dgMaster[colPrintTimesMaster.Name, i].Value = master.PrintTimes;
                this.dgMaster[colProjectName.Name, i].Tag = master.ProjectId;
                this.dgMaster[colProjectName.Name, i].Value = master.ProjectName;
                this.dgMaster[colOperationOrg.Name, i].Tag = master.OperOrgInfo;
                this.dgMaster[colOperationOrg.Name, i].Value = master.OperOrgInfoName;
                this.dgMaster[colAuditPerson.Name, i].Tag = master.AuditPerson;
                this.dgMaster[colAuditPerson.Name, i].Value = master.AuditPersonName;
                this.dgMaster[colSumQuantity.Name, i].Value = master.SumQuantity;
                this.dgMaster[colSumMoney.Name, i].Value = master.SumMoney;
                this.dgMaster.Rows[i].Tag = master;
            }
            this.dgMaster.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            dgMaster.CurrentCell = dgMaster[1, 0];
            dgMaster_SelectionChanged(dgMaster, new EventArgs());
        }

        void dgMaster_SelectionChanged(object sneder, EventArgs e)
        {
            dgDetailBill.Rows.Clear();
            WasteMatApplyMaster master = dgMaster.CurrentRow.Tag as WasteMatApplyMaster;
            if (master == null) return;
            foreach (WasteMatApplyDetail dtl in master.Details)
            {
                int i = this.dgDetailBill.Rows.Add();
                this.dgDetailBill[colMaterialCodeBill.Name, i].Value = dtl.MaterialCode;
                this.dgDetailBill[colMaterialNameBill.Name, i].Tag = dtl.MaterialResource;
                this.dgDetailBill[colMaterialNameBill.Name, i].Value = dtl.MaterialName;
                this.dgDetailBill[colSpecBill.Name, i].Value = dtl.MaterialSpec;
                this.dgDetailBill[colQuantityBill.Name, i].Value = dtl.Quantity;
                this.dgDetailBill[colGradeNoBill.Name, i].Value = dtl.Grade;
                this.dgDetailBill[colDiagramNumBill.Name, i].Value = dtl.DiagramNumber;
                this.dgDetailBill[colGWBSBill.Name, i].Tag = dtl.UsedPart;
                this.dgDetailBill[colGWBSBill.Name, i].Value = dtl.UsedPartName;
                this.dgDetailBill[colMatStandardUnitNameBill.Name, i].Value = dtl.MatStandardUnitName;
                this.dgDetailBill[colApplyDateBill.Name, i].Value = dtl.ApplyDate.ToShortDateString();
                this.dgDetailBill[colDescriptBill.Name, i].Value = dtl.Descript;
            }
        }
    }
}
