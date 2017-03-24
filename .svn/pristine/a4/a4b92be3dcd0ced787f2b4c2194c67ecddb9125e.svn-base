using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain;
using Application.Business.Erp.SupplyChain.Client.PLMWebServices;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder
{
    public partial class VMaterialHireOrderQuery : TBasicDataView
    {
        private MMaterialHireMng model = new MMaterialHireMng();

        public VMaterialHireOrderQuery()
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
            //this.dtpDateBegin.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
            this.txtMaterial.materialCatCode = CommonUtil.TurnMaterialMatCode;
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
          txtSupplyBill.SupplierCatCode=  txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode1;

           
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
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.btnSearchBill.Click += new EventHandler(btnSearchBill_Click);
            this.dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            this.txtCodeBeginBill.tbTextChanged += new EventHandler(txtCodeBeginBill_tbTextChanged);
            //this.TenantSelectorBill.TenantSelectorAfterEvent += (a) => { projectInfo = a.SelectedProject; };
        }

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = dgDetail.Rows[e.RowIndex].Tag as string;
                if (ClientUtil.ToString(billId) != "")
                {
                    VMaterialHireOrder vOrder = new VMaterialHireOrder();
                    vOrder.Start(billId);
                    vOrder.ShowDialog();
                }
            }
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgDetail[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colCode.Name)
            {
                MatHireOrderMaster master = model.MaterialHireMngSvr.GetMaterialHireOrderByCode(dgvCell.Value.ToString());
                VMaterialHireOrder vmro = new VMaterialHireOrder();
                vmro.CurBillMaster = master;
                vmro.Preview();
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

        void txtCodeBeginBill_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEndBill.Text = this.txtCodeBeginBill.Text;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                #region 查询条件处理
                FlashScreen.Show("正在查询信息......");
                string condition = "";
                if (TenantSelector.SelectedProject != null)
                {
                    condition += "and t1.ProjectId = '" + TenantSelectorBill.SelectedProject.Id + "'";
                }
                if (txtSupplier.Result != null && txtSupplier.Result.Count > 0)
                {
                    condition += " t1.supplierrelation='" + (txtSupplier.Result as SupplierRelationInfo).Id + "' ";
                }
                if (this.txtCodeBegin.Text != "")
                {
                    condition = condition + " and t1.Code between '" + this.txtCodeBegin.Text + "' and '" + this.txtCodeEnd.Text + "'";
                }

                if (StaticMethod.IsUseSQLServer())
                {
                    condition += " and t1.CreateDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and t1.CreateDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
                }
                else
                {
                    condition += " and t1.CreateDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
                }

                if (this.txtSupplier.Text != "" && this.txtSupplier.Result != null && this.txtSupplier.Result.Count != 0)
                {
                    condition = condition + " and t1.SupplierRelation='" + (this.txtSupplier.Result[0] as SupplierRelationInfo).Id.ToString() + "'";
                }
                if (!txtCreatePerson.Text.Trim().Equals("") && txtCreatePerson.Result != null && txtCreatePerson.Result.Count > 0)
                {
                    condition += " and t1.CreatePerson='" + (txtCreatePerson.Result[0] as PersonInfo).Id + "'";
                }

                if (txtOriContractNo.Text != "")
                {
                    condition += " and t1.OriginalContractNo like '" + txtOriContractNo.Text + "%'";
                }

                if (this.txtMaterial.Text != "")
                {
                    condition = condition + " and t7.matName like '%" + this.txtMaterial.Text + "%'";
                }
                if (this.txtSpec.Text != "")
                {
                    condition = condition + " and t7.MatSpecification like '%" + this.txtSpec.Text + "%'";
                }
                if (comMngType.Text != "")
                {
                    System.Web.UI.WebControls.ListItem li = comMngType.SelectedItem as System.Web.UI.WebControls.ListItem;
                    int values = ClientUtil.ToInt(li.Value);
                    condition += " and t1.State= '" + values + "' ";
                }
                if (!string.IsNullOrEmpty(this.txtOldCode.Text))
                {
                    condition += string.Format(" and t1.BillCode like '%{0}%'", this.txtOldCode.Text);
                }
                #endregion
                DataSet dataSet = model.MaterialHireMngSvr.MaterialHireOrderQuery(condition);
                this.dgDetail.Rows.Clear();
                DataTable dataTable = dataSet.Tables[0];
                //if (dataTable == null || dataTable.Rows.Count == 0) return;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int rowIndex = this.dgDetail.Rows.Add();
                    dgDetail[colCode.Name, rowIndex].Value = dataRow["code"];
                    DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                    currRow.Tag = ClientUtil.ToString(dataRow["id"]);
                    dgDetail[colOriContractNo.Name, rowIndex].Value = dataRow["originalContractNo"].ToString();
                    dgDetail[colSupplyInfo.Name, rowIndex].Value = dataRow["ORGNAME"].ToString();
                    object objState = dataRow["State"];
                    if (objState != null)
                    {
                        dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                    }
                    dgDetail[colMaterialCode.Name, rowIndex].Value = dataRow["MATCODE"].ToString();
                    dgDetail[colMaterialName.Name, rowIndex].Value = dataRow["MATNAME"].ToString();
                    dgDetail[colSpec.Name, rowIndex].Value = dataRow["MATSPECIFICATION"].ToString();
                    dgDetail[colRemark.Name, rowIndex].Value = dataRow["Descript"].ToString();
                    dgDetail[colPrice.Name, rowIndex].Value = dataRow["price"].ToString();
                    dgDetail[colBalRule.Name, rowIndex].Value = dataRow["balRule"].ToString();
                    dgDetail[colUnit.Name, rowIndex].Value = dataRow["STANDUNITNAME"].ToString();
                    dgDetail[colCreateDate.Name, rowIndex].Value = dataRow["RealOperationDate"].ToString();
                    dgDetail[colRealOperationDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["CreateDate"]).ToShortDateString();
                    dgDetail[colBillCode.Name, rowIndex].Value = ClientUtil.ToString(dataRow["BillCode"]);
                }
                FlashScreen.Close();
                this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
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

        void btnSearchBill_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            if (this.txtCodeBeginBill.Text != "")
            {
                if (this.txtCodeBeginBill.Text == this.txtCodeEndBill.Text)
                {
                    oq.AddCriterion(Expression.Like("Code", this.txtCodeBeginBill.Text, MatchMode.Start));
                }
                else
                {
                    oq.AddCriterion(Expression.Ge("Code", this.txtCodeBeginBill.Text));
                    oq.AddCriterion(Expression.Lt("Code", this.txtCodeEndBill.Text));
                }
            }
            if (TenantSelectorBill.SelectedProject != null)
            {
                oq.AddCriterion(Expression.Eq("ProjectId", TenantSelector.SelectedProject.Id));
            }
            if (txtSupplyBill.Result != null && txtSupplyBill.Result.Count > 0 && txtSupplyBill.Result[0] is SupplierRelationInfo)
            {
                oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", (txtSupplyBill.Result[0] as SupplierRelationInfo)));
            }
                oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBeginBill.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEndBill.Value.AddDays(1).Date));
            if (!string.IsNullOrEmpty(this.txtOldCodeBill.Text))
            {
                oq.AddCriterion(Expression.Like("BillCode", this.txtOldCodeBill.Text, MatchMode.Anywhere));
            }
            if (this.txtCreatePersonBill.Text != "")
            {
                oq.AddCriterion(Expression.Eq("CreatePersonName", this.txtCreatePersonBill.Text));
            }
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.MatStandardUnit", NHibernate.FetchMode.Eager);
            IList list = model.MaterialHireMngSvr.GetMaterialHireOrder( oq);
            dgMaster.Rows.Clear();
            dgDetailBill.Rows.Clear();
            ShowMasterList(list);
        }

        void ShowMasterList(IList list)
        {
            if (list == null || list.Count == 0) return;
            foreach (MatHireOrderMaster master in list)
            {
                int i = this.dgMaster.Rows.Add();
                this.dgMaster[colCodeMaster.Name, i].Value = master.Code;
                this.dgMaster[colStateMaster.Name, i].Value = ClientUtil.GetDocStateName(master.DocState);
                this.dgMaster[colCreateDateMaster.Name, i].Value = master.CreateDate.ToShortDateString();
                this.dgMaster[colRealOperationDateMaster.Name, i].Value = master.RealOperationDate.ToShortDateString();
                this.dgMaster[colSupplyInfoMaster.Name, i].Tag = master.TheSupplierRelationInfo;
                this.dgMaster[colSupplyInfoMaster.Name, i].Value = master.SupplierName;
                this.dgMaster[colProjectNameBill.Name, i].Value = master.ProjectName;
                this.dgMaster[colOriContractNoMaster.Name, i].Value = master.OriginalContractNo;
                this.dgMaster[colBalRuleMaster.Name, i].Value = master.BalRule;
                this.dgMaster[this.colPersonName.Name, i].Value = master.CreatePersonName;
                this.dgMaster[this.colBillCodeBill.Name, i].Value = master.BillCode;
                this.dgMaster.Rows[i].Tag = master;
            }
            dgMaster.CurrentCell = dgMaster[1, 0];
            dgMaster_SelectionChanged(dgMaster, new EventArgs());
        }

        void dgMaster_SelectionChanged(object sneder, EventArgs e)
        {
            dgDetailBill.Rows.Clear();
            MatHireOrderMaster master = dgMaster.CurrentRow.Tag as MatHireOrderMaster;
            if (master == null) return;
            foreach (MatHireOrderDetail dtl in master.Details)
            {
                int i = this.dgDetailBill.Rows.Add();
                this.dgDetailBill[colMaterialCodeBill.Name, i].Value = dtl.MaterialResource.Code;
                this.dgDetailBill[colMaterialNameBill.Name, i].Tag = dtl.MaterialResource;
                this.dgDetailBill[colMaterialNameBill.Name, i].Value = dtl.MaterialResource.Name;
                this.dgDetailBill[colSpecBill.Name, i].Value = dtl.MaterialResource.Specification;
                this.dgDetailBill[colRemarkBill.Name, i].Value = dtl.Descript;
                this.dgDetailBill[colPriceBill.Name, i].Value = dtl.Price;
                this.dgDetailBill[colUnitBill.Name, i].Tag = dtl.MatStandardUnit;
                this.dgDetailBill[colUnitBill.Name, i].Value = dtl.MatStandardUnit.Name;
            }
        }

        
    }
}
