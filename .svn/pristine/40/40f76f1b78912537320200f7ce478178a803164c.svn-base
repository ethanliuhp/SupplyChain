using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireReturn.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;
namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireReturn
{
    public partial class VMaterialHireReturnQuery : TBasicDataView
    {
        MMaterialHireMng model = new MMaterialHireMng();
        public VMaterialHireReturnQuery()
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
            this.txtCodeBeginBill.tbTextChanged += new EventHandler(txtCodeBeginBill_tbTextChanged);
        }

        void txtCodeBeginBill_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEndBill.Text = this.txtCodeBeginBill.Text;
        }
        void dgMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgMaster[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colCodeMaster.Name)
            {
                MatHireReturnMaster master = dgMaster.Rows[e.RowIndex].Tag as MatHireReturnMaster;
               
                VMaterialHireReturn vMaterialReturn = new VMaterialHireReturn(master.IsLoss,master.MatHireType);
                vMaterialReturn.RefreshControls(VirtualMachine.Component.WinMVC.generic.MainViewState.Browser);
                vMaterialReturn.Start(master.Id);
                vMaterialReturn.StartPosition = FormStartPosition.CenterParent;
                vMaterialReturn.ShowDialog();
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
                    MatHireReturnMaster master = model.MaterialHireMngSvr.GetMaterialHireReturnById(billId);
                    //if()
                    VMaterialHireReturn vMaterialReturn = new VMaterialHireReturn(master.IsLoss, master.MatHireType);
                    vMaterialReturn.RefreshControls(VirtualMachine.Component.WinMVC.generic.MainViewState.Browser);
                    //vMaterialReturn.MatReturnMaster = master;
                    //vMaterialReturn.Preview();
                    vMaterialReturn.Start(master.Id);
                    vMaterialReturn.ShowDialog();
                }
            }
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgDetail[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colCode.Name && dgDetail.Rows[e.RowIndex].Tag!=null)
            {//GetMaterialReturnByCode
                string sMatserID = dgDetail.Rows[e.RowIndex].Tag as string;
                MatHireReturnMaster master = model.MaterialHireMngSvr.GetMaterialHireReturnById(sMatserID);
                //if()
                VMaterialHireReturn vMaterialReturn = new VMaterialHireReturn(master.IsLoss,master.MatHireType);
                vMaterialReturn.RefreshControls(VirtualMachine.Component.WinMVC.generic.MainViewState.Browser);
                //vMaterialReturn.MatReturnMaster = master;
                //vMaterialReturn.Preview();
                vMaterialReturn.Start(master.Id);
                vMaterialReturn.ShowDialog();
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
                //CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                //condition += "and t1.ProjectId = '" + projectInfo.Id + "'";
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
                    condition = condition + " and t1.TheRank='" + (this.txtRank.Result[0] as SupplierRelationInfo).Id.ToString() + "'";
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
                if (!string.IsNullOrEmpty(txtOldCode.Text))
                {
                    condition += string.Format(" and t1.BillCode like '%{0}%' ",txtOldCode.Text);
                }
                #endregion
                //GetMaterialReturnMasterQuery
                DataSet dataSet = model.MaterialHireMngSvr.GetMaterialHireReturnMasterQuery(condition);
                this.dgDetail.Rows.Clear();

                DataTable dataTable = dataSet.Tables[0];
                EnumMatHireType matHire;
                //if (dataTable == null || dataTable.Rows.Count == 0) return;
                decimal sumQuantity = 0;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int rowIndex = this.dgDetail.Rows.Add();
                    dgDetail[colCode.Name, rowIndex].Value = dataRow["code"];


                    DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                    currRow.Tag = dataRow["id"];

                    dgDetail[colOriContractNo.Name, rowIndex].Value = dataRow["OldContractNum"].ToString();
                    dgDetail[colSupplyInfo.Name, rowIndex].Value = dataRow["SupplierName"].ToString();
                    dgDetail[colCreatePerson.Name, rowIndex].Value = dataRow["CreatePersonName"].ToString();

                    object objState = dataRow["State"];
                    if (objState != null)
                    {
                        dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                    }

                    dgDetail[colMaterialCode.Name, rowIndex].Value = dataRow["MaterialCode"].ToString();
                    dgDetail[colMaterialName.Name, rowIndex].Value = dataRow["MaterialName"].ToString();
                    dgDetail[colSpec.Name, rowIndex].Value = dataRow["MaterialSpec"].ToString();
                    object quantity = dataRow["ExitQuantity"];
                    if (quantity != null)
                    {
                        sumQuantity += decimal.Parse(quantity.ToString());
                    }
                    dgDetail[colExitQuantity.Name, rowIndex].Value = quantity;
                    dgDetail[colPrice.Name, rowIndex].Value = dataRow["RentalPrice"].ToString();

                    dgDetail[colBalRule.Name, rowIndex].Value = dataRow["balRule"].ToString();
                    dgDetail[colUnit.Name, rowIndex].Value = dataRow["MatStandardUnitName"].ToString();
                    dgDetail[colRank.Name, rowIndex].Value = dataRow["TheRankName"].ToString();
                    dgDetail[colUesedPart.Name, rowIndex].Value = dataRow["UsedPartName"].ToString();
                    dgDetail[this.colSubject.Name, rowIndex].Value = ClientUtil.ToString(dataRow["subjectname"]);
                    dgDetail[colBroachQuantity.Name, rowIndex].Value = dataRow["BroachQuantity"].ToString();
                    dgDetail[colConsumeQuantity.Name, rowIndex].Value = dataRow["ConsumeQuantity"].ToString();
                    dgDetail[colRejectQuantity.Name, rowIndex].Value = dataRow["RejectQuantity"].ToString();
                    dgDetail[this.colDisCardQty.Name, rowIndex].Value = dataRow["discardqty"].ToString();//切头
                    dgDetail[this.colRepairQty.Name, rowIndex].Value = dataRow["repairqty"].ToString();//维修
                    dgDetail[this.colLossQty.Name, rowIndex].Value = dataRow["lossqty"].ToString();//报损
                    dgDetail[colTransChrge.Name, rowIndex].Value = dataRow["transportcharge"].ToString();
                    DateTime date = Convert.ToDateTime(dataRow["CreateDate"]);
                    dgDetail[colCreateDate.Name, rowIndex].Value = date.ToShortDateString();
                    dgDetail[colBusinessDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["RealOperationDate"]).ToString("yyyy-MM-dd");
                    dgDetail[colPrintTimes.Name, rowIndex].Value = dataRow["PrintTimes"].ToString();
                    dgDetail[this.colProjectName.Name, rowIndex].Value =ClientUtil.ToString( dataRow["projectname"]) ;
                    //,t2.beforestockqty,t1.mathiretype,t2.materialtype,t2.materiallength
                    dgDetail[colProjectQuantity.Name, rowIndex].Value = dataRow["beforestockqty"].ToString();
                    matHire = (EnumMatHireType)ClientUtil.ToInt(dataRow["mathiretype"]);
                    dgDetail[this.colDtlMatLen.Name, rowIndex].Value =matHire==EnumMatHireType.钢管?ClientUtil.ToString( dataRow["materiallength"]):"" ;
                    dgDetail[this.colDtlMatType.Name, rowIndex].Value = matHire == EnumMatHireType.碗扣 ? ClientUtil.ToString(dataRow["materialtype"]) : "";
                    dgDetail[this.colBillCode.Name, rowIndex].Value = ClientUtil.ToString(dataRow["BillCode"]);
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
            //CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            //oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
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
            oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBeginBill.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEndBill.Value.AddDays(1).Date));
            if (this.txtCreatePersonBill.Text != "")
            {
                oq.AddCriterion(Expression.Eq("CreatePersonName", this.txtCreatePersonBill.Text));
            }
            if (!string.IsNullOrEmpty(txtOldCodeBill.Text))
            {
                oq.AddCriterion(Expression.Like("BillCode",txtOldCodeBill.Text,MatchMode.Anywhere));
            }
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            IList list = model.MaterialHireMngSvr.ObjectQuery(typeof(MatHireReturnMaster), oq);
            dgMaster.Rows.Clear();
            dgDetailBill.Rows.Clear();
            ShowMasterList(list);
        }

        void ShowMasterList(IList list)
        {
            if (list == null || list.Count == 0) return;
            foreach (MatHireReturnMaster master in list)
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
                //this.dgMaster[colTransportChargeMaster.Name, i].Value = master.TransportCharge;
                this.dgMaster[this.colProjectNameMaster.Name, i].Value = master.ProjectName;
                this.dgMaster[this.colMasterQtMoney.Name, i].Value = master.SumExtMoney;
                this.dgMaster[this.colBillCodeMaster.Name, i].Value = master.BillCode;
                if (master.MatHireType == EnumMatHireType.钢管)
                {
                    this.dgMaster[this.colLessOneQty.Name, i].Value = master.LessOneQuanity;
                }
                this.dgMaster.Rows[i].Tag = master;
            }
            this.dgMaster.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            dgMaster.CurrentCell = dgMaster[1, 0];
            dgMaster_SelectionChanged(dgMaster, new EventArgs());
        }

        void dgMaster_SelectionChanged(object sneder, EventArgs e)
        {
            dgDetailBill.Rows.Clear();
            MatHireReturnMaster master = dgMaster.CurrentRow.Tag as MatHireReturnMaster;
            if (master == null) return;
            colMatTypeBill.Visible = master.MatHireType!=EnumMatHireType.普通料具;
            if (master.MatHireType == EnumMatHireType.钢管)
            {
                colMatTypeBill.Name = "长度";
            }
            else if (master.MatHireType == EnumMatHireType.碗扣)
            {
                colMatTypeBill.Name = "规格";
            }
            foreach (MatHireReturnDetail dtl in master.Details)
            {
                int i = this.dgDetailBill.Rows.Add();
                this.dgDetailBill[colMaterialCodeBill.Name, i].Value = dtl.MaterialCode;
                this.dgDetailBill[colMaterialNameBill.Name, i].Value = dtl.MaterialName;
                this.dgDetailBill[colSpecBill.Name, i].Value = dtl.MaterialSpec;
                this.dgDetailBill[colProjectQuantityBill.Name, i].Value = dtl.ProjectQuantity;
                this.dgDetailBill[colExitQuantityBill.Name, i].Value = dtl.ExitQuantity;
                this.dgDetailBill[colBroachQuantityBill.Name, i].Value = dtl.BroachQuantity;
                this.dgDetailBill[colRejectQuantityBill.Name, i].Value = dtl.RejectQuantity;
                this.dgDetailBill[colRepairQtyBill.Name, i].Value = dtl.RepairQty;
                this.dgDetailBill[colLossQtyBill.Name, i].Value = dtl.LoseQuantity;
                this.dgDetailBill[colConsumeQuantityBill.Name, i].Value = dtl.ConsumeQuantity;
                this.dgDetailBill[colDisCardQtyBill.Name, i ].Value = dtl.DisCardQty;
                this.dgDetailBill[colPriceBill.Name, i].Value = dtl.RentalPrice;
                this.dgDetailBill[colUesedPartBill.Name, i].Value = dtl.UsedPartName;
                this.dgDetailBill[colSubjectBill.Name, i].Value = dtl.SubjectName;
                this.dgDetailBill[colUnitBill.Name, i].Value = dtl.MatStandardUnitName;
                this.dgDetailBill[colDescriptBill.Name, i].Value = dtl.Descript;
                this.dgDetailBill[colRankBill.Name, i].Value = dtl.BorrowUnitName;
                if (master.MatHireType == EnumMatHireType.钢管){
                    this.dgDetailBill[colMatTypeBill.Name, i].Value = dtl.MaterialLength;
                }
                else if (master.MatHireType == EnumMatHireType.碗扣)
                {
                    this.dgDetailBill[colMatTypeBill.Name, i].Value = dtl.MaterialType; ;
                }
            }
        }
    }
}

