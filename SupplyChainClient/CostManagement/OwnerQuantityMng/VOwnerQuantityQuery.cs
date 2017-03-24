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
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.OwnerQuantityMng.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Collections;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Core;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.OwnerQuantityMng
{
    public partial class VOwnerQuantityQuery : TBasicDataView
    {
        private MOwnerQuantityMng model = new MOwnerQuantityMng();

        public VOwnerQuantityQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }        

        private void InitData()
        {
            //comMngType.Items.Clear();
            //comMngType1.Items.Clear();
            Array tem = Enum.GetValues(typeof(DocumentState));
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddMonths(-6);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            this.dtpDateBeginBill.Value = ConstObject.TheLogin.LoginDate.AddMonths(-6);
            this.dtpDateEndBill.Value = ConstObject.TheLogin.LoginDate;
            txtSign.Items.AddRange(new object[] { "已登帐", "未登帐" });
            txtSignBill.Items.AddRange(new object[] { "已登帐", "未登帐" });

            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }       

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnSearchBill.Click += new EventHandler(btnSearchBill_Click);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            dgMaster.CellContentClick+=new DataGridViewCellEventHandler(dgMaster_CellContentClick);
        }
          void dgMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgMaster[e.ColumnIndex, e.RowIndex];
            if (dgvCell.OwningColumn.Name == colCodeBill.Name)
            {
                //DailyPlanMaster master = model.DailyPlanSrv.GetDailyPlanByCode(dgvCell.Value.ToString());
                OwnerQuantityMaster master =dgMaster.Rows[e.RowIndex].Tag as OwnerQuantityMaster;
                VOwnerQuantityMng vmro = new VOwnerQuantityMng();
                vmro.CurBillMaster = master;
                vmro.Preview();
            }
        }
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = dgDetail.Rows[e.RowIndex].Tag as string;
                if (ClientUtil.ToString(billId) != "")
                {
                    VOwnerQuantityMng vOrder = new VOwnerQuantityMng();
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
                OwnerQuantityMaster master = model.OwnerQuantitySrv.GetOwnerQuantityByCode(dgvCell.Value.ToString());
                VOwnerQuantityMng vmro = new VOwnerQuantityMng();
                vmro.CurBillMaster = master;
                vmro.Preview();
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        /// <summary>
        /// 明细查询按钮事件
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
                //单据号
                if (this.txtCodeBegin.Text != "")
                {
                    condition = condition + "and t1.Code like '%" + this.txtCodeBegin.Text + "%'";//模糊查询

                }
                //制单日期
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
                //登帐记录
                if (txtSign.SelectedItem != null)
                {
                    condition += "and AccountSign = '" + txtSign.SelectedItem + "'";
                }
                //报送类型
                if (this.cmbDtlQuantityType.SelectedItem != null)
                {
                    condition += "and QuantityType = '" + cmbDtlQuantityType.SelectedItem + "'";
                }
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                condition += "and t1.ProjectId = '" + projectInfo.Id + "'";
                #endregion
                DataSet dataSet = model.OwnerQuantitySrv.OwnerQuantityQuery(condition);
                this.dgDetail.Rows.Clear();

                DataTable dataTable = dataSet.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int rowIndex = this.dgDetail.Rows.Add();
                    dgDetail[colCode.Name, rowIndex].Value = dataRow["code"].ToString();
                    DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                    currRow.Tag = ClientUtil.ToString(dataRow["id"]);

                    object objState = dataRow["State"];
                    if (objState != null)
                    {
                        dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                    }
                    decimal submitQuantity = ClientUtil.ToDecimal(dataRow["SubmitQuantity"]) / 10000;
                    dgDetail[this.colQuantityMoney.Name, rowIndex].Value = ClientUtil.ToString(submitQuantity);
                    dgDetail[colCreatePerson.Name, rowIndex].Value = dataRow["CreatePersonName"].ToString();
                    dgDetail[this.colQuantityType.Name, rowIndex].Value = ClientUtil.ToString(dataRow["QuantityType"]);
                    dgDetail[colQWBSName.Name, rowIndex].Value =  ClientUtil.ToString(dataRow["QWBSName"]);
                    dgDetail[colSign.Name, rowIndex].Value =  ClientUtil.ToString(dataRow["AccountSign"]);
                    decimal confirmMoney = ClientUtil.ToDecimal(dataRow["ConfirmMoney"]) / 10000;//审定金额
                    dgDetail[this.colConfirmMoney.Name, rowIndex].Value = ClientUtil.ToString(confirmMoney);
                    dgDetail[colDescript.Name, rowIndex].Value =  ClientUtil.ToString(dataRow["Descript"]);
                    //报送时间
                    dgDetail[colOwnerDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["QuantityDate"]).ToShortDateString();
                    dgDetail[this.colConfirmDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["ConfirmDate"]).ToShortDateString();
                    dgDetail[this.colConfirmStartDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["confirmStartDate"]).ToShortDateString();
                    dgDetail[this.colConfirmEndDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["confirmEndDate"]).ToShortDateString();
                    decimal gatheringMoney = ClientUtil.ToDecimal(dataRow["acctGatheringMoney"]) / 10000;//应收款金额
                    dgDetail[this.colGatheringMoney.Name, rowIndex].Value = ClientUtil.ToString(gatheringMoney);
                    dgDetail[this.colGatheringRate.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["gatheringRate"]) * 100;
                    //制单时间
                    if (ClientUtil.ToDateTime(dataRow["CreateDate"]) > ClientUtil.ToDateTime("1900-01-01"))
                    {
                        dgDetail[colRealOperationDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["CreateDate"]).ToShortDateString();
                    }
                }
                FlashScreen.Close();
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

            //单据
            if (txtCodeBeginBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("Code", txtCodeBeginBill.Text, NHibernate.Criterion.MatchMode.Anywhere));
            }
            //报送时间
            objectQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBeginBill.Value.Date));
            objectQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEndBill.Value.AddDays(1).Date));
            //制单人
            if (txtCreatePersonBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("CreatePersonName", txtCreatePersonBill.Text, MatchMode.Anywhere));
            }
            //报送类型
            if (this.cmbQuantityType.SelectedItem != null)
            {
                objectQuery.AddCriterion(Expression.Like("QuantityType", cmbQuantityType.Text, MatchMode.Anywhere));
            }
            //登帐记录
            if (txtSignBill.SelectedItem != null)
            {
                objectQuery.AddCriterion(Expression.Like("AccountSign", txtSignBill.Text, MatchMode.Anywhere));
                //}
                ////状态
                //if (comMngType.Text != "")
                //{
                //    System.Web.UI.WebControls.ListItem li = comMngType.SelectedItem as System.Web.UI.WebControls.ListItem;
                //    int values = ClientUtil.ToInt(li.Value);
                //    //objectQuery.AddCriterion(Expression.Eq("DocState", (DocumentState)values));
                //}
            }
            try
            {
                list = model.OwnerQuantitySrv.GetOwnerQuantity(objectQuery);
                dgDetail.Rows.Clear();
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
            dgDetail.Rows.Clear();
            if (masterList == null || masterList.Count == 0) return;
            foreach (OwnerQuantityMaster master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCodeBill.Name, rowIndex].Value = master.Code;
                dgMaster[colCreatePersonBill.Name, rowIndex].Value = master.CreatePersonName;//制单人
                dgMaster[colSignBill.Name, rowIndex].Value = master.AccountSign;   //登帐标志
                dgMaster[this.colQuantityTypeBill.Name, rowIndex].Value = master.QuantityType;   //报送类型
                dgMaster[colOwnerDateBill.Name, rowIndex].Value = master.CreateDate.ToShortDateString();//报送日期
                dgMaster[colRealOperationDateBill.Name, rowIndex].Value = master.RealOperationDate.ToShortDateString();//制单日期
                dgMaster[colDescriptBill.Name, rowIndex].Value = master.Descript;//备注
                dgMaster[colStateBill.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);//状态

            }
            lblRecordTotalBill.Text = "共【" + dgMaster.Rows.Count + "】条记录";
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
            OwnerQuantityMaster master = dgMaster.CurrentRow.Tag as OwnerQuantityMaster;
            if (master == null) return;
            //dgDetail.Rows[rowIndex].Tag = dtl;
            foreach (OwnerQuantityDetail dtl in master.Details)
            {
                int rowIndex = dgDetailBill.Rows.Add();
                dgDetailBill[colQWBSNameBill.Name, rowIndex].Value = dtl.QWBSName;//清单WBS名称
                this.dgDetailBill[this.colQuantityDateBill.Name, rowIndex].Value = ClientUtil.ToDateTime(dtl.QuantityDate).ToShortDateString();
                dgDetailBill[colSubmitMoneyBill.Name, rowIndex].Value = dtl.SubmitQuantity / 10000; //报送金额
                this.dgDetailBill[this.colConfirmDateBill.Name, rowIndex].Value = ClientUtil.ToDateTime(dtl.ConfirmDate).ToShortDateString(); ;
                dgDetailBill[colConfirmMoneyBill.Name, rowIndex].Value = dtl.ConfirmMoney / 10000; //审定金额
                this.dgDetailBill[this.colConfirmStartDateBill.Name, rowIndex].Value = ClientUtil.ToDateTime(dtl.ConfirmStartDate).ToShortDateString();
                this.dgDetailBill[this.colConfirmEndDateBill.Name, rowIndex].Value = ClientUtil.ToDateTime(dtl.ConfirmEndDate).ToShortDateString();
                this.dgDetailBill[this.colGatheringMoneyBill.Name, rowIndex].Value = dtl.AcctGatheringMoney / 10000;
                this.dgDetailBill[this.colGatheringRateBill.Name, rowIndex].Value = dtl.GatheringRate * 100;
                this.dgDetailBill[this.colDtlDescript.Name, rowIndex].Value = dtl.Descript;
            }
            
        }
    }
}
