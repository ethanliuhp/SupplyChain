using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Core;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyOrderMng
{
    public partial class VSupplyOrderQuery : TBasicDataView
    {
        private MSupplyOrderMng model = new MSupplyOrderMng();

        public VSupplyOrderQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
            InitZYFL();
        }

        private void InitZYFL()
        {
            this.txtMaterialCategory.rootCatCode = "01";
            //添加专业分类下拉框
            VBasicDataOptr.InitProfessionCategory(comSpecailType, false);
            //comSpecailType.ContextMenuStrip = cmsDg;
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
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            this.dtpDateBeginBill.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEndBill.Value = ConstObject.TheLogin.LoginDate;
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
            this.btnExcelBill.Click += new EventHandler(btnExcel_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.btnSearchBill.Click += new EventHandler(btnSearchBill_Click);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);

        }

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colCode.Name))
            {
                return;
            }
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = dgDetail.Rows[e.RowIndex].Tag as string;
                if (ClientUtil.ToString(billId) != "")
                {
                    VSupplyOrder vOrder = new VSupplyOrder();
                    vOrder.Start(billId);
                    vOrder.ShowDialog();
                }
            }
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgDetail[e.ColumnIndex, e.RowIndex];
           // if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            string billId = dgDetail.Rows[e.RowIndex].Tag as string;
            if (string.IsNullOrEmpty(billId)) return;
            if (dgvCell.OwningColumn.Name == colCode.Name)
            {
                //SupplyOrderMaster master = model.SupplyOrderSrv.GetSupplyOrderByCode(dgvCell.Value.ToString());
                SupplyOrderMaster master = model.SupplyOrderSrv.GetSupplyOrderById (billId);
                VSupplyOrder vmro = new VSupplyOrder();
                vmro.CurBillMaster = master;
                vmro.Preview();
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            

            if (tabControl1.SelectedTab == tabPage2)
            {
                //oCustomDataGridView = this.dgDetail;
                if (this.dgDetail.Rows.Count > 0)
                {
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
                }
                else
                {
                    MessageBox.Show("没有记录");
                }
               
            }
            else if (tabControl1.SelectedTab == tabPage1)
            {
                if (this.dgDetailBill.Rows.Count > 0)
                {
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetailBill, true);
                }
                else
                {
                    MessageBox.Show("没有记录");
                }
                //Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetailBill, true);
            }
           
            
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
                FlashScreen.Show("正在查询信息......");
                string condition = "";
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                condition += "and t1.ProjectId = '" + projectInfo.Id + "'";
                //单据号
                if (this.txtCodeBegin.Text != "")
                {
                    if (this.txtCodeBegin.Text == this.txtCodeEnd.Text)
                    {
                        condition = condition + "and t1.Code like '%" + this.txtCodeBegin.Text + "%'";//模糊查询
                    }
                    else
                    {
                        condition = condition + " and t1.Code between '" + this.txtCodeBegin.Text + "' and '" + this.txtCodeEnd.Text + "'";//精确查询
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
                //负责人
                if (!txtHandlePerson.Text.Trim().Equals("") && txtHandlePerson.Result != null)
                {
                    condition = condition + " and t1.handleperson='" + (txtHandlePerson.Result[0] as PersonInfo).Id + "'";
                }
                //制单人
                if (!txtCreatePerson.Text.Trim().Equals("") && txtCreatePerson.Result != null && txtCreatePerson.Result.Count > 0)
                {
                    condition += " and t1.CreatePerson='" + (txtCreatePerson.Result[0] as PersonInfo).Id + "'";
                }

                //会计月
                if (!txtAccountMonth.Text.Trim().Equals(""))
                {
                    condition += "and t1.CreateMonth = '" + txtAccountMonth.Text + "'";
                }

                //会计年
                if (!txtAccountYear.Text.Trim().Equals(""))
                {
                    condition += "and t1.CreateYear = '" + txtAccountYear.Text + "'";
                }

                //供应商
                if (this.txtSupplier.Text != "" && this.txtSupplier.Result != null && this.txtSupplier.Result.Count != 0)
                {
                    condition = condition + " and t1.SupplierRelation='" + (this.txtSupplier.Result[0] as SupplierRelationInfo).Id.ToString() + "'";
                }

                //采购合同号
                if (!this.txtSupplyNum.Text.Trim().Equals(""))
                {
                    condition += "and t1.OldContractNum like '%" + txtSupplyNum.Text + "%'";
                }

                //专业分类
                if (this.comSpecailType.SelectedItem != null)
                {
                    condition += "and t2.SpecialType = '" + comSpecailType.SelectedItem + "'";
                }

                //物资
                if (this.txtMaterial.Text != "")
                {
                    condition = condition + " and t2.MaterialName like '%" + this.txtMaterial.Text + "%'";
                }
                //物资分类
                if (txtMaterialCategory.Text != "" && txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
                {
                    MaterialCategory materialCategory = txtMaterialCategory.Result[0] as MaterialCategory;
                    condition += " and t2.materialcode like '" + materialCategory.Code + "%'";
                }
                //规格型号
                if (this.txtMaterialSpec.Text != "")
                {
                    condition = condition + " and t2.MaterialSpec like '%" + this.txtMaterialSpec.Text + "%'";
                }
                if (comMngType.Text != "")
                {
                    System.Web.UI.WebControls.ListItem li = comMngType.SelectedItem as System.Web.UI.WebControls.ListItem;
                    int values = ClientUtil.ToInt(li.Value);
                    condition += "and t1.State= '" + values + "'";
                }
                #endregion
                DataSet dataSet = model.SupplyOrderSrv.SupplyOrderQuery(condition);
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
                    dgDetail[colMaterialCode.Name, rowIndex].Value = dataRow["MaterialCode"].ToString();//物资编码
                    dgDetail[colMaterialName.Name, rowIndex].Value = dataRow["MaterialName"].ToString();//物资名称
                    dgDetail[colSpec.Name, rowIndex].Value = dataRow["MaterialSpec"].ToString(); //规格型号
                    dgDetail[colUnit.Name, rowIndex].Value = dataRow["MatStandardUnitName"].ToString();//计量单位
                    object quantity = dataRow["Quantity"];//总数量
                    if (quantity != null)
                    {
                        sumQuantity += decimal.Parse(quantity.ToString());
                    }
                    dgDetail[colQuantity.Name, rowIndex].Value = quantity;
                    dgDetail[colPrice.Name, rowIndex].Value = dataRow["SupplyPrice"].ToString();//单价
                    dgDetail[colSupplier.Name, rowIndex].Value = dataRow["SupplierName"].ToString();//供应商
                    dgDetail[colSupplyContractNum.Name, rowIndex].Value = dataRow["OldContractNum"].ToString();//采购合同号
                    dgDetail[colDescript.Name, rowIndex].Value = dataRow["Descript"].ToString();//备注
                    dgDetail[colCreatePerson.Name, rowIndex].Value = dataRow["CreatePersonName"].ToString();//制单人
                    string b = dataRow["CreateDate"].ToString();
                    string[] bArray = b.Split(' ');
                    string strb = bArray[0];
                    this.txtSumMoney.Text = ClientUtil.ToString(dataRow["ContractMoney"]);//总金额/合约金额
                    dgDetail[colCreateDate.Name, rowIndex].Value = strb;//业务日期
                    dgDetail[colRealOperationDate.Name, rowIndex].Value = dataRow["RealOperationDate"].ToString();//制单日期
                    dgDetail[colSumMoney.Name, rowIndex].Value = dataRow["Money"].ToString();//金额

                    decimal dConfrimPrice = ClientUtil.ToDecimal(dataRow["confirmprice"]);
                    dgDetail[colConfrimPrice.Name, rowIndex].Value = dConfrimPrice;//认价单价
                    dgDetail[colConfrimMoney.Name, rowIndex].Value = decimal.Parse(quantity.ToString()) * dConfrimPrice;//认价金额
                    dgDetail[colDiagramNum.Name, rowIndex].Value = dataRow["DiagramNumber"].ToString();
                    dgDetail[colPP.Name, rowIndex].Value = dataRow["BRAND"].ToString();
                    dgDetail[colTelParameter.Name, rowIndex].Value = dataRow["TECHNOLOGYPARAMETER"].ToString();
                }
                FlashScreen.Close();
                this.txtSumQuantity.Text = sumQuantity.ToString("#,###.####");
                lblRecordTotal.Text = "共【" + dataTable.Rows.Count + "】条记录";
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
        /// <summary>
        /// 主从表显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearchBill_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("正在查询信息......");
            ObjectQuery objectQuery = new ObjectQuery();
            IList list = new ArrayList();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            //objectQuery.AddCriterion(Expression.Eq("ProjectName", projectInfo.Name));
            //单据
            if (txtCodeBeginBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("Code", txtCodeBeginBill.Text, MatchMode.Anywhere));
            }
            //业务日期
            objectQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBeginBill.Value.Date));
            objectQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEndBill.Value.AddDays(1).Date));
            //制单人
            if (txtCreatePersonBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("CreatePersonName", txtCreatePersonBill.Text, MatchMode.Anywhere));
            }
            //采购合同号
            if (txtSupplyNumBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("OldContractNum", txtSupplyNumBill.Text, MatchMode.Anywhere));
            }
            try
            {
                list = model.SupplyOrderSrv.GetSupplyOrder(objectQuery);
                dgMaster.Rows.Clear();
                dgDetailBill.Rows.Clear();
                ShowMasterList(list);
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
        /// <summary>
        /// 显示主表
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList masterList)
        {
            dgMaster.Rows.Clear();
            if (masterList == null || masterList.Count == 0) return;
            foreach (SupplyOrderMaster master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCodeBill.Name, rowIndex].Value = master.Code;                                                           //单据号
                dgMaster[colSupplyContractNumBill.Name, rowIndex].Value = master.OldContractNum;                           //采购合同号
                dgMaster[colSupplierBill.Name, rowIndex].Value = master.SupplierName;                                                 //供应商
                dgMaster[colCreateDateBill.Name, rowIndex].Value = master.CreateDate.ToShortDateString();         //业务日期
                dgMaster[colCreatePersonBill.Name, rowIndex].Value = master.CreatePersonName;                                        //制单人
                if (master.RealOperationDate > ClientUtil.ToDateTime("1900-1-1"))
                {
                    dgMaster[colRealOperationDateBill.Name, rowIndex].Value = master.RealOperationDate.ToShortDateString();     //制单时间;
                }
                dgMaster[colStateBill.Name, rowIndex].Value =ClientUtil.GetDocStateName( master.DocState);                                     //状态
                dgMaster[colDescriptBill.Name, rowIndex].Value = master.Descript;                                                 //备注
            }
            this.dgMaster.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            dgMaster.CurrentCell = dgMaster[1, 0];
            dgMaster_SelectionChanged(dgMaster, new EventArgs());
        }
        /// <summary>
        /// 主表变化，明细同步变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            dgDetailBill.Rows.Clear();
            SupplyOrderMaster master = dgMaster.CurrentRow.Tag as SupplyOrderMaster;
            if (master == null) return;
            //dgDetail.Rows[rowIndex].Tag = dtl;
            decimal sumQuantity = 0;
            foreach (SupplyOrderDetail dtl in master.Details)
            {
                int rowIndex = dgDetailBill.Rows.Add();
                dgDetailBill[colMaterialCodeBill.Name, rowIndex].Value = dtl.MaterialCode;                               //物资编码
                dgDetailBill[colMaterialNameBill.Name, rowIndex].Value = dtl.MaterialName;                             //物资名称
                dgDetailBill[colSpecBill.Name, rowIndex].Value = dtl.MaterialSpec;                                                 //规格型号   
                object quantity = dtl.Quantity;//总数量
                if (quantity != null)
                {
                    sumQuantity += decimal.Parse(quantity.ToString());
                }
                dgDetailBill[colQuantityBill.Name, rowIndex].Value = quantity;                                                 //数量
                dgDetailBill[colPriceBill.Name, rowIndex].Value = dtl.SupplyPrice;                                               //单价
                dgDetailBill[colDiagramNumBill.Name, rowIndex].Value = dtl.DiagramNumber;                           //图号
                dgDetailBill[colSumMoneyBill.Name, rowIndex].Value = dtl.Money;                                       //金额
                decimal dConfrimPrice = ClientUtil.ToDecimal(dtl.ConfirmPrice);
                dgDetailBill[colConfrimPriceBill.Name, rowIndex].Value = dConfrimPrice;//认价单价
                dgDetailBill[colConfrimMoneyBill.Name, rowIndex].Value = decimal.Parse(quantity.ToString()) * dConfrimPrice; //认价金额
                dgDetailBill[colUnitBill.Name, rowIndex].Value = dtl.MatStandardUnitName;                                 //计量单位名称
                dgDetailBill[colDescriptDg.Name, rowIndex].Value = dtl.Descript;                                           //备注
            }
        }
    }
}
