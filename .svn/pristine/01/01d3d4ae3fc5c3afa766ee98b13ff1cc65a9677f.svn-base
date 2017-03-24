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
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.ConcreteManage.ConcreteBalanceMng.Domain;
using VirtualMachine.Core;
using System.Collections;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConBalanceMng
{
    public partial class VConBalanceQuery : TBasicDataView
    {
        MConcreteMng model = new MConcreteMng();
        public VConBalanceQuery()
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
        }
        private void InitEvent()
        {
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnSearchBill.Click += new EventHandler(btnSearchBill_Click);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            this.dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            this.dgMaster.CellContentClick+=new DataGridViewCellEventHandler(dgMaster_CellContentClick);
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

                if (!txtHandlePerson.Text.Trim().Equals("") && txtHandlePerson.Result != null)
                {
                    condition = condition + " and t1.handleperson='" + (txtHandlePerson.Result[0] as PersonInfo).Id + "'";
                }

                if (!txtCreatePerson.Text.Trim().Equals("") && txtCreatePerson.Result != null && txtCreatePerson.Result.Count > 0)
                {
                    condition += " and t1.CreatePerson='" + (txtCreatePerson.Result[0] as PersonInfo).Id + "'";
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
                DataSet dataSet = model.ConcreteMngSrv.GetConereteBalanceByQuery(condition);
                this.dgDetail.Rows.Clear();

                DataTable dataTable = dataSet.Tables[0];
                decimal sumVolume = 0;
                decimal sumMoney = 0;

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int rowIndex = this.dgDetail.Rows.Add();
                    dgDetail[colCode.Name, rowIndex].Value = dataRow["code"];
                    dgDetail[colSupplyInfo.Name, rowIndex].Value = dataRow["SupplierName"];

                    object objState = dataRow["State"];
                    if (objState != null)
                    {
                        dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                    }

                    dgDetail[colMaterialCode.Name, rowIndex].Value = dataRow["MaterialCode"];
                    dgDetail[colMaterialName.Name, rowIndex].Value = dataRow["MaterialName"];
                    dgDetail[colSpec.Name, rowIndex].Value = dataRow["MaterialSpec"];
                    dgDetail[colPrice.Name, rowIndex].Value = dataRow["Price"];
                    dgDetail[colMoney.Name, rowIndex].Value = dataRow["Money"];
                    dgDetail[this.colCreatePerson1.Name, rowIndex].Value = dataRow["createpersonname"];
                    dgDetail[this.colBusinessDate1.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["createdate"]).ToShortDateString();
                    dgDetail[this.colSystemDate1.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["realoperationdate"]).ToString();
                    object money = dataRow["Money"];
                    if (money != null)
                    {
                        sumMoney += decimal.Parse(money.ToString());
                    }
                    bool IsPump = true;
                    if (dataRow["IsPump"].ToString() == "1")
                    {
                        IsPump = true;
                    }
                    else if (dataRow["IsPump"].ToString() == "0")
                    {
                        IsPump = false;
                    }
                    if (IsPump == true)
                    {
                        dgDetail[colIsPump.Name, rowIndex].Value = true;
                    }
                    else
                    {
                        dgDetail[colIsPump.Name, rowIndex].Value = false;
                    }
                    dgDetail[colTicketVolume.Name, rowIndex].Value = dataRow["CheckingVolume"];
                    dgDetail[colBalVolume.Name, rowIndex].Value = dataRow["BalanceVolume"];
                    dgDetail[colUsedPart.Name, rowIndex].Value = dataRow["UsedPartName"];
                    dgDetail.Rows[rowIndex].Tag = dataRow["Id"];
                     object volume = dataRow["BalanceVolume"];
                    if (volume != null)
                    {
                        sumVolume += decimal.Parse(volume.ToString());
                    }
                }
                FlashScreen.Close();
                this.txtSQuantity.Text = sumVolume.ToString("#,###.####");
                this.txtSMoney.Text = sumMoney.ToString("#,###.####");
                lblResource.Text = "共【" + dataTable.Rows.Count + "】条记录";
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

        void btnExcel_Click(object sender, EventArgs e)
        {
            StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        void dgMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgMaster[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colCodeBill.Name)
            {
                ConcreteBalanceMaster cmat = dgMaster.Rows[e.RowIndex].Tag as ConcreteBalanceMaster;
                //string masterId = dgMaster.Rows[e.RowIndex].Tag.ToString();
                //ConcreteBalanceMaster cmat = model.ConcreteMngSrv.GetConcreteBalanceMasterById(masterId);
                VConBalReport frm = new VConBalReport(cmat);
                frm.ShowDialog();
            }
        }
        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgDetail[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colCode.Name)
            {
                //ConcreteBalanceMaster cmat = dgDetail.Rows[e.RowIndex].Tag as ConcreteBalanceMaster;
                string masterId = dgDetail.Rows[e.RowIndex].Tag.ToString();
                ConcreteBalanceMaster cmat = model.ConcreteMngSrv.GetConcreteBalanceMasterById(masterId);
                VConBalReport frm = new VConBalReport(cmat);
                frm.ShowDialog();
            }
        }

        //主从表查询按钮事件
        void btnSearchBill_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("正在查询信息......");
            ObjectQuery objectQuery = new ObjectQuery();
            IList list = new ArrayList();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            //当前项目
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            //单据
            if (txtCodeBeginBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("Code", txtCodeBeginBill.Text, MatchMode.Anywhere));
            }
            //业务时间
            objectQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBeginBill.Value.Date));
            objectQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEndBill.Value.AddDays(1).Date));
            //创建人
            if (txtCreatePersonBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("CreatePersonName", txtCreatePersonBill.Text, MatchMode.Anywhere));
            }
            //承担队伍
            if (txtMaterialBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Eq("BearTeamName", txtMaterialBill.Text));
                objectQuery.AddCriterion(Expression.Eq("BearTeam", txtMaterialBill.Tag as SubContractProject));
            } 
            //供应商
            if (txtSupplierBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Eq("SupplierName", txtSupplierBill.Text));
            }
            //状态
            if (comMngTypeBill.Text != "")
            {
                System.Web.UI.WebControls.ListItem li = comMngType.SelectedItem as System.Web.UI.WebControls.ListItem;
                int values = ClientUtil.ToInt(li.Value);
                objectQuery.AddCriterion(Expression.Eq("DocState", (DocumentState)values));
            }
            try
            {
                list = model.ConcreteMngSrv.GetConcreteBalanceMaster(objectQuery);
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
            foreach (ConcreteBalanceMaster master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCodeBill.Name, rowIndex].Value = master.Code;//单据号
                dgMaster[colSupplyInfoBill.Name, rowIndex].Value = master.SupplierName;//供应商
                dgMaster[colCreatePersonBill.Name, rowIndex].Value = master.CreatePersonName;//制单人
                dgMaster[this.colBusinessDate.Name, rowIndex].Value = master.CreateDate.ToShortDateString();//制单人
                dgMaster[this.colSystemDate.Name, rowIndex].Value = master.RealOperationDate;//制单人
                dgMaster[colDescriptBill.Name, rowIndex].Value = master.Descript;         //备注
                dgMaster[colStateBill.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState) ;//状态  
            }
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
            ConcreteBalanceMaster master = dgMaster.CurrentRow.Tag as ConcreteBalanceMaster;
            if (master == null) return;
            //dgDetail.Rows[rowIndex].Tag = dtl;
            decimal sumVolume = 0;
            decimal sumMoney = 0;
            foreach (ConcreteBalanceDetail dtl in master.Details)
            {
                int rowIndex =  dgDetailBill.Rows.Add();
                dgDetailBill[colMaterialCodeBill.Name, rowIndex].Value = dtl.MaterialCode;           //物资编码
                dgDetailBill[colMaterialNameBill.Name, rowIndex].Value = dtl.MaterialName;               //物资名称 
                dgDetailBill[colSpecBill.Name, rowIndex].Value = dtl.MaterialSpec;         //规格型号
                dgDetailBill[colPriceBill.Name, rowIndex].Value = dtl.Price;      //单价
                dgDetailBill[colMoneyBill.Name, rowIndex].Value = dtl.Money;         //金额
                dgDetailBill[colTicketVolumeBill.Name, rowIndex].Value = dtl.CheckingVolume;            //对账应结方量
                dgDetailBill[colBalVolumeBill.Name, rowIndex].Value = dtl.BalanceVolume;                     //结算方量
                dgDetailBill[colUsedPartBill.Name, rowIndex].Value = dtl.UsedPartName;              //使用部位
                bool IsPump = true;
                if (dtl.IsPump == true)
                {
                    IsPump = true;
                } 
                else
                {
                    IsPump = false;
                }
                if (IsPump == true)
                {
                    dgDetailBill[colIsPumpBill.Name, rowIndex].Value = true;
                }
                else
                {
                    dgDetailBill[colIsPumpBill.Name, rowIndex].Value = false;
                }
                object money = dtl.Money;
                if (money != null)
                {
                    sumMoney += decimal.Parse(money.ToString());
                }
                object volume = dtl.BalanceVolume;
                if (volume != null)
                {
                    sumVolume += decimal.Parse(volume.ToString());
                }
            }
            this.txtSumQuantity.Text = sumVolume.ToString("#,###.####");
            this.txtSumMoney.Text = sumMoney.ToString("#,###.####");
            lblRecordTotal.Text = "共【" + master.Details.Count + "】条记录";
        }
    }
}
