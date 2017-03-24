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
using VirtualMachine.Component.Util;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialCollectionMng.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialCollection
{
    public partial class VMaterialCollectionQuery : TBasicDataView
    {
        private MMatRentalMng model = new MMatRentalMng();
        public VMaterialCollectionQuery()
        {
            InitializeComponent();
            InitData();
            InitEvent();
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
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            this.txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode1;
            this.txtRank.SupplierCatCode = CommonUtil.SupplierCatCode3;
            this.txtMaterial.materialCatCode = CommonUtil.TurnMaterialMatCode;
        }
        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            dgMaster.CellContentClick+=new DataGridViewCellEventHandler(dgMaster_CellContentClick);
            this.btnSearchBill.Click += new EventHandler(btnSearchBill_Click);
            this.dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
        }
        void dgMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgMaster[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colCodeMaster.Name)
            {
                MaterialCollectionMaster master = dgMaster.Rows[e.RowIndex].Tag as MaterialCollectionMaster;
                VMaterialCollection vMaterial = new VMaterialCollection();
                vMaterial.MatColMaster = master;
                vMaterial.Preview();
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
                    VMaterialCollection vOrder = new VMaterialCollection();
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
                MaterialCollectionMaster master = model.MatMngSrv.GetMaterialCollectionMasterByCode(ClientUtil.ToString(dgvCell.Value));
                VMaterialCollection vMaterial = new VMaterialCollection();
                vMaterial.MatColMaster = master;
                vMaterial.Preview();
            }
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        /// <summary>
        /// Excel导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                #region 查询条件处理
                FlashScreen.Show("正在查询信息......");
                string condition = "";
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                condition += "and t1.ProjectId = '" + projectInfo.Id + "'";
                if (this.txtCodeBegin.Text != "")
                {
                    condition = condition + " and t1.Code between '" + this.txtCodeBegin.Text + "' and '" + this.txtCodeEnd.Text + "'";
                }

                if (StaticMethod.IsUseSQLServer())
                {
                    if (rbCreateDate.Checked == true)
                    {
                        condition += " and t1.CreateDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and t1.CreateDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
                    }
                    if (rbOper.Checked == true)
                    {
                        condition += " and t1.RealOperationDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and t1.RealOperationDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
                    }
                }
                else
                {
                    if (rbCreateDate.Checked == true)
                    {
                        condition += " and t1.RealOperationDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.RealOperationDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
                    }
                    if (rbOper.Checked == true)
                    {
                        condition += " and t1.CreateDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
                    }
                }
                if (this.txtSupplier.Text != "" && this.txtSupplier.Result != null && this.txtSupplier.Result.Count != 0)
                {
                    condition = condition + " and t1.SupplierRelation='" + (this.txtSupplier.Result[0] as SupplierRelationInfo).Id.ToString() + "'";
                }
                if (this.txtRank.Text != "" && this.txtRank.Result != null && this.txtRank.Result.Count != 0)
                {
                    condition = condition + " and t2.BorrowUnit='" + (this.txtRank.Result[0] as SupplierRelationInfo).Id.ToString() + "'";
                }

                if (!txtCreatePerson.Text.Trim().Equals("") && txtCreatePerson.Result != null && txtCreatePerson.Result.Count > 0)
                {
                    condition += " and t1.CreatePerson='" + (txtCreatePerson.Result[0] as PersonInfo).Id + "'";
                }

                if (txtOriContractNo.Text != "")
                {
                    condition += " and t1.OldContractNum like '" + txtOriContractNo.Text + "%'";
                }

                if (this.txtMaterial.Text != "")
                {
                    condition = condition + " and t2.MaterialName like '%" + this.txtMaterial.Text + "%'";
                }
                if (this.txtSpec.Text != "")
                {
                    condition = condition + " and t2.MaterialSpec like '%" + this.txtSpec.Text + "%'";
                }
                if (comMngType.Text != "")
                {
                    System.Web.UI.WebControls.ListItem li = comMngType.SelectedItem as System.Web.UI.WebControls.ListItem;
                    int values = ClientUtil.ToInt(li.Value);
                    condition += "and t1.State= '" + values + "'";
                }
                #endregion
                DataSet dataSet = model.MatMngSrv.MaterialCollectionMasterQuery(condition);
                this.dgDetail.Rows.Clear();

                DataTable dataTable = dataSet.Tables[0];
                //if (dataTable == null || dataTable.Rows.Count == 0) return;
                decimal sumQuantity = 0;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int rowIndex = this.dgDetail.Rows.Add();
                    dgDetail[colCode.Name, rowIndex].Value = dataRow["code"];
                    DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                    currRow.Tag = ClientUtil.ToString(dataRow["id"]);
                    dgDetail[colOriContractNo.Name, rowIndex].Value = dataRow["OldContractNum"].ToString();
                    dgDetail[colSupplyInfo.Name, rowIndex].Value = dataRow["SupplierName"].ToString();
                    dgDetail[colRank.Name, rowIndex].Value = dataRow["TheRankName"].ToString();
                    dgDetail[colCreatePerson.Name, rowIndex].Value = dataRow["CreatePersonName"].ToString();
                    dgDetail[colDescript.Name, rowIndex].Value = dataRow["Descript"].ToString();
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
                    dgDetail[colPrice.Name, rowIndex].Value = dataRow["RentalPrice"].ToString();
                    dgDetail[colBalRule.Name, rowIndex].Value = dataRow["balRule"].ToString();
                    dgDetail[colUnit.Name, rowIndex].Value = dataRow["MatStandardUnitName"].ToString();
                    dgDetail[this.colSubject.Name, rowIndex].Value = ClientUtil.ToString(dataRow["subjectname"]);
                    dgDetail[this.colUsedPart.Name, rowIndex].Value = ClientUtil.ToString(dataRow["usedpartname"]);
                    dgDetail[colTranstCharge.Name, rowIndex].Value = dataRow["transportcharge"].ToString();
                    DateTime date = Convert.ToDateTime(dataRow["CreateDate"]);
                    dgDetail[colCreateDate.Name, rowIndex].Value = date.ToShortDateString();
                    dgDetail[colBusinessDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["RealOperationDate"]).ToString("yyyy-MM-dd");
                    dgDetail[colPrintTimes.Name, rowIndex].Value = dataRow["PrintTimes"].ToString();
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
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            //objectQuery.AddCriterion(Expression.Eq("ProjectName", projectInfo.Name));
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
            IList list = model.MatMngSrv.ObjectQuery(typeof(MaterialCollectionMaster), oq);
            dgMaster.Rows.Clear();
            dgDetailBill.Rows.Clear();
            ShowMasterList(list);
        }

        void ShowMasterList(IList list)
        {
            if (list == null || list.Count == 0) return;
            foreach (MaterialCollectionMaster master in list)
            {
                int i = this.dgMaster.Rows.Add();
                this.dgMaster[colCodeMaster.Name, i].Value = master.Code;
                this.dgMaster[colStateMaster.Name, i].Value = ClientUtil.GetDocStateName(master.DocState);
                this.dgMaster[colCreateDateMaster.Name, i].Value = master.CreateDate.ToShortDateString();
                this.dgMaster[colBusinessDateMaster.Name, i].Value = master.RealOperationDate.ToShortDateString();
                this.dgMaster[colSupplyInfoMaster.Name, i].Tag = master.TheSupplierRelationInfo;
                this.dgMaster[colSupplyInfoMaster.Name, i].Value = master.SupplierName;
                this.dgMaster[colOriContractNoMaster.Name, i].Value = master.OldContractNum;
                this.dgMaster[colPrintTimesMaster.Name, i].Value = master.PrintTimes;
                this.dgMaster[colCreatePersonMaster.Name, i].Value = master.CreatePersonName;
                this.dgMaster[colDescriptMaster.Name, i].Value = master.Descript;
                this.dgMaster[colBalRuleMaster.Name, i].Value = master.BalRule;
                this.dgMaster[colTransportChargeMaster.Name, i].Value = master.TransportCharge;
                this.dgMaster.Rows[i].Tag = master;
            }
            this.dgMaster.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            dgMaster.CurrentCell = dgMaster[1, 0];
            dgMaster_SelectionChanged(dgMaster, new EventArgs());
        }

        void dgMaster_SelectionChanged(object sneder, EventArgs e)
        {
            dgDetailBill.Rows.Clear();
            MaterialCollectionMaster master = dgMaster.CurrentRow.Tag as MaterialCollectionMaster;
            if (master == null) return;
            foreach (MaterialCollectionDetail dtl in master.Details)
            {
                int i = this.dgDetailBill.Rows.Add();
                this.dgDetailBill[colMaterialCodeBill.Name, i].Value = dtl.MaterialCode;
                this.dgDetailBill[colMaterialNameBill.Name, i].Tag = dtl.MaterialResource;
                this.dgDetailBill[colMaterialNameBill.Name, i].Value = dtl.MaterialName;
                this.dgDetailBill[colSpecBill.Name, i].Value = dtl.MaterialSpec;
                this.dgDetailBill[colDescriptBillBill.Name, i].Value = dtl.Descript;
                this.dgDetailBill[colQuantityBill.Name, i].Value = dtl.Quantity;
                this.dgDetailBill[colPriceBill.Name, i].Value = dtl.RentalPrice;
                this.dgDetailBill[colUnitBill.Name, i].Tag = dtl.MatStandardUnit;
                this.dgDetailBill[colUnitBill.Name, i].Value = dtl.MatStandardUnitName;
                this.dgDetailBill[colUsedPartBill.Name, i].Tag = dtl.UsedPart;
                this.dgDetailBill[colUsedPartBill.Name, i].Value = dtl.UsedPartName;
                this.dgDetailBill[colSubjectBill.Name, i].Tag = dtl.SubjectGUID;
                this.dgDetailBill[colSubjectBill.Name, i].Value = dtl.SubjectName;
            }
        }
    }
}
