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
using Application.Business.Erp.SupplyChain.Client.SupplyMng;
using Application.Business.Erp.SupplyChain.SettlementManagement.MaterialSettleMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.MaterialResource.Domain;
using NHibernate.Criterion;
using VirtualMachine.Core;
using System.Collections;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.Client.SettlementManagement.MaterialSettleMng
{
    public partial class VMaterialSettleQuery : TBasicDataView
    {
        private MMaterialSettleMng model = new MMaterialSettleMng();
        EnumMaterialSettleType SettleType;
        private bool IsSelectParts = true;
        public VMaterialSettleQuery(EnumMaterialSettleType excuteType)
        {
            InitializeComponent();
            SettleType = excuteType;
            InitEvent();
            InitData();
        }        

        private void InitData()
        {
            //comMngType.Items.Clear();
            //comMngType1.Items.Clear();
            //Array tem = Enum.GetValues(typeof(DocumentState));
            //for (int i = 0; i < tem.Length; i++)
            //{
            //    DocumentState s = (DocumentState)tem.GetValue(i);
            //    int k = (int)s;
            //    if (k != 0)
            //    {
            //        string strNewName = ClientUtil.GetDocStateName(k);
            //        System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
            //        li.Text = strNewName;
            //        li.Value = k.ToString();
            //        comMngType.Items.Add(li);
            //        comMngType1.Items.Add(li);
            //    }
            //}

            var projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
            {
                txtOperationOrg.Text = ConstObject.TheOperationOrg.Name;
                txtOperationOrg.Tag = ConstObject.TheOperationOrg;
                btnOperationOrg.Visible = false;
                txtOperationOrgDetail.Text = ConstObject.TheOperationOrg.Name;
                txtOperationOrgDetail.Tag = ConstObject.TheOperationOrg;
                btnOperationOrgDetail.Visible = false;
            }


            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateBeginBill.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEndBill.Value = ConstObject.TheLogin.LoginDate;
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            btnAll.Checked = true;
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }       

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnSearchBill.Click += new EventHandler(btnSearchBill_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnSelect.Click += new EventHandler(btnSelect_Click);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.btnSelectCostSubject.Click += new EventHandler(btnSelectSubject_Click);
            this.btnOperationOrgDetail.Click += new EventHandler(btnOperationOrgDetail_Click);
        }

        private void btnOperationOrg_Click(object sender, EventArgs e)
        {
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect(true);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                var selOrg = frm.Result[0] as OperationOrgInfo;
                txtOperationOrg.Tag = selOrg;
                txtOperationOrg.Text = selOrg.Name;
            }
        }

        private void btnOperationOrgDetail_Click(object sender, EventArgs e)
        {
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect(true);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                var selOrg = frm.Result[0] as OperationOrgInfo;
                txtOperationOrgDetail.Tag = selOrg;
                txtOperationOrgDetail.Text = selOrg.Name;
            }
        }



        //科目
        void btnSelectSubject_Click(object sender, EventArgs e)
        {
            VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
            frm.ShowDialog();
            CostAccountSubject cost = frm.SelectAccountSubject;
            if (cost != null)
            {
                this.txtCostSubject.Text = cost.Name;
                this.txtCostSubject.Tag = cost;
            }
        }
       //明细显示查找工程任务
        void btnSelect_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsTreeSelect = true;

            frm.IsCheck = IsSelectParts;
            frm.IsRootNode = IsSelectParts;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                if (IsSelectParts)
                {
                    GWBSTree task = null;
                    string sUsePartName = string.Empty;
                    IList list = new ArrayList();
                    foreach (TreeNode oNode in frm.SelectResult)
                    {
                        task = oNode.Tag as GWBSTree;
                        if (task != null)
                        {
                            if (!string.IsNullOrEmpty(sUsePartName))
                            {
                                sUsePartName += " | " + task.Name;

                            }
                            else
                            {
                                sUsePartName = task.Name;
                            }
                            list.Add(task);
                        }
                    }
                    this.txtProject.Text = sUsePartName;
                    this.txtProject.Tag = list;
                }
                else
                {
                    TreeNode root = frm.SelectResult[0];
                    GWBSTree task = root.Tag as GWBSTree;
                    if (task != null)
                    {
                        this.txtProject.Text = task.Name;
                        this.txtProject.Tag = task;

                    }
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
                MaterialSettleMaster master = model.MaterialSettleSrv.GetMaterialSettleByCode(dgvCell.Value.ToString());
                VMaterialSettleMng vmro = new VMaterialSettleMng();
                vmro.CurBillMaster = master;
                vmro.Preview();
            }
        }
        //明细显示Excel按钮
        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }
        /// <summary>
        /// 明细显示查找按钮事件
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
                //CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                //condition += "and t1.ProjectId = '" + projectInfo.Id + "'";
                var org = txtOperationOrgDetail.Tag as OperationOrgInfo;
                if (org != null) condition += String.Format(" and t1.OpgSysCode like '{0}%'", org.SysCode);
                else condition += String.Format(" and t1.OpgSysCode like '{0}%'", ConstObject.TheOperationOrg.SysCode);
                //if (org != null)
                //{
                //    objectQuery.AddCriterion(Expression.Like("OpgSysCode", org.SysCode, MatchMode.Start));
                //}
                //else
                //{
                //    objectQuery.AddCriterion(Expression.Like("OpgSysCode", ConstObject.TheOperationOrg.SysCode, MatchMode.Start));
                //}


                //单据
                if (txtCodeBegin.Text != "")
                {
                    condition += "and t1.Code like '%" + txtCodeBegin.Text + "%'";
                }
                if (this.btnYes.Checked)
                {
                    condition += "and t1.monthaccountbill is not null ";
                }
                if (this.btnNo.Checked)
                {
                    condition += "and t1.monthaccountbill is null ";
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
                ////工程任务
                //if (txtProject.Text != "")
                //{
                //    string[] txt = this.txtProject.Text.Split(new[] { " | " }, StringSplitOptions.RemoveEmptyEntries);
                //    condition += "and (";
                //    for (int i = txt.Length - 1; i >= 0; i--)
                //    {
                //        if (i == txt.Length - 1)
                //        {
                //            condition += "t2.ProjectTaskName like '%" + txt[i] + "%'";
                //        }
                //        else
                //        {
                //            condition += " or t2.ProjectTaskName like '%" + txt[i] + "%'";
                //        }
                //    }
                //    condition += ")";
                    
                //}
                if (txtProject.Text != "")
                {
                    IList listGwbs = txtProject.Tag as IList;
                    GWBSTree gwb = listGwbs[0] as GWBSTree;
                    if (gwb != null)
                    { 
                        condition=condition +"and  t2.ProjectTaskCode like '%" +gwb.SysCode +"%'";
                    }
                }


                //科目
                if (txtCostSubject.Text != "")
                {
                    CostAccountSubject cas = txtCostSubject.Tag as CostAccountSubject;
                    if (cas != null)
                    {
                        condition = condition + " and t2.ACCOUNTCOSTCODE like'%" + cas.SysCode + "%'";

                    }
                }
                if (SettleType == EnumMaterialSettleType.materialQuery)
                {
                    condition += "and t1.SettleState = 'materialQuery'";
                }
                if (SettleType == EnumMaterialSettleType.materialSettleQuery)
                {
                    condition += "and t1.SettleState = 'materialConsume'";
                }

                //物资             
                if (!txtMaterial.Text.Trim().Equals("") && txtMaterial.Result != null)
                {
                    condition = condition + " and t2.MaterialName like'%" + txtMaterial.Text + "%'";
                }   
                #endregion
                //condition = condition + "and t1.projectid ='" + projectInfo.Id + "'";
                DataSet dataSet = model.MaterialSettleSrv.MaterialSettleQuery(condition);
                this.dgDetail.Rows.Clear();

                DataTable dataTable = dataSet.Tables[0];
                //if (dataTable == null || dataTable.Rows.Count == 0) return;
                decimal sumQuantity = 0;
                decimal sumMoney = 0;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int rowIndex = this.dgDetail.Rows.Add();
                    dgDetail[colCode.Name, rowIndex].Value = dataRow["code"];

                    DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                    currRow.Tag = ClientUtil.ToString(dataRow["id"]);
                    object objState = dataRow["State"];
                    if (objState != null)
                    {
                        dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                    }
                    dgDetail[colAccountSubject.Name, rowIndex].Value = dataRow["AccountCostName"].ToString();
                    //dgDetail[colCostName.Name, rowIndex].Value = dataRow["CostName"];
                    dgDetail[colCreateMonth.Name, rowIndex].Value = dataRow["CreateMonth"].ToString();
                    dgDetail[colCreateYear.Name, rowIndex].Value = dataRow["CreateYear"].ToString();
                    dgDetail[colDescript.Name, rowIndex].Value = dataRow["Descript"].ToString();
                    dgDetail[colMaterialType.Name, rowIndex].Value = dataRow["MaterialName"].ToString();
                    dgDetail[colSpec.Name, rowIndex].Value = dataRow["MaterialSpec"].ToString();
                    dgDetail[colStuff.Name, rowIndex].Value = dataRow["MaterialStuff"].ToString();
                    dgDetail[colPrice.Name, rowIndex].Value = dataRow["Price"].ToString();
                    dgDetail[colProjectTask.Name, rowIndex].Value = dataRow["ProjectTaskName"].ToString();
                    dgDetail[colQuantity.Name, rowIndex].Value = dataRow["Quantity"].ToString();
                    dgDetail[colSumMoney.Name, rowIndex].Value = dataRow["Money"].ToString();
                    dgDetail[this.colMonthCost.Name, rowIndex].Value = ClientUtil.ToString(dataRow["monthaccountbill"]) == "" ? "否" : "是";
                    object quantity = dataRow["Money"];
                    if (quantity != null)
                    {
                        sumQuantity += decimal.Parse(quantity.ToString());
                    }
                    dgDetail[colCreatePerson.Name, rowIndex].Value = dataRow["CreatePersonName"].ToString();//制单人
                    dgDetail[colCreateDate.Name, rowIndex].Value = dataRow["CreateDate"].ToString();//业务日期
                    dgDetail[colRealOperationDateDg.Name, rowIndex].Value = dataRow["RealOperationDate"].ToString();//制单日期
                }
                FlashScreen.Close();
                lblRecordTotal.Text = "共【" + dataTable.Rows.Count + "】条记录";
                this.txtSumQuantity.Text = sumQuantity.ToString("#,###.####");
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
       /// 明细方式显示 双击事件
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
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
                    VMaterialSettleMng vOrder = new VMaterialSettleMng();
                    vOrder.Start(billId, SettleType);
                    vOrder.ShowDialog();
                }
            }
        }
        /// <summary>
        ///单据查询选择任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSelectBill_Click(object sender, EventArgs e)
        {

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
            //CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            
            //objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));

            //单据
            if (txtCodeBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("Code", txtCodeBill.Text, MatchMode.Anywhere));
            }
            //创建时间
            objectQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBeginBill.Value.Date));
            objectQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEndBill.Value.AddDays(1).Date));
            var org = txtOperationOrg.Tag as OperationOrgInfo;
            if (org != null)
            {
                objectQuery.AddCriterion(Expression.Like("OpgSysCode", org.SysCode, MatchMode.Start));
            }
            else
            {
                objectQuery.AddCriterion(Expression.Like("OpgSysCode", ConstObject.TheOperationOrg.SysCode, MatchMode.Start));
            }
            //创建人
            if (txtCreatePersonBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("CreatePersonName", txtCreatePersonBill.Text, MatchMode.Anywhere));
            }
            if (SettleType == EnumMaterialSettleType.materialQuery)
            {
                objectQuery.AddCriterion(Expression.Eq("SettleState", "materialQuery"));
                //condition += "and t1.SettleState = 'materialQuery'";
            }
            if (SettleType == EnumMaterialSettleType.materialSettleQuery)
            {
                objectQuery.AddCriterion(Expression.Eq("SettleState", "materialConsume"));
                //condition += "and t1.SettleState = 'materialSettleQuery'";
            }
            //会计月
            if (txtAccountMonthBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Eq("CreateMonth",ClientUtil.ToInt( txtAccountMonthBill.Text)));
            }
            //会计年
            if (txtAccountYearBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Sql("to_char(CreateYear) like '%" + txtAccountYearBill.Text + "'"));
            }
            //状态
            if (comMngType.Text != "")
            {
                System.Web.UI.WebControls.ListItem li = comMngType.SelectedItem as System.Web.UI.WebControls.ListItem;
                int values = ClientUtil.ToInt(li.Value);
                //objectQuery.AddCriterion(Expression.Eq("DocState", (DocumentState)values));
            }
  
            try
            {
                list = model.MaterialSettleSrv.GetMaterialSettle(objectQuery);
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
            dgDetailBill.Rows.Clear();
            if (masterList == null || masterList.Count == 0) return;
            decimal sumQuantitys = 0;
            foreach (MaterialSettleMaster master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                decimal sumQuantity = 0;
                foreach (MaterialSettleDetail dtl in master.Details)
                {
                    object quantity = dtl.Money;
                    if (quantity != null)
                    {
                        sumQuantity += decimal.Parse(quantity.ToString());
                    }
                }
                object quantitys = sumQuantity.ToString();
                if (quantitys != null)
                {
                    sumQuantitys += decimal.Parse(quantitys.ToString());
                }
                dgMaster[colSummaryMoney.Name, rowIndex].Value = master.SumMoney;
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCodeBill.Name, rowIndex].Value = master.Code;
                dgMaster[colCreateYearBill.Name, rowIndex].Value = master.CreateYear;   //会计年
                dgMaster[colCreateMonthBill.Name, rowIndex].Value = master.CreateMonth;   //会计月
                dgMaster[colCreatePersonBill.Name, rowIndex].Value = master.CreatePersonName;//制单人
                dgMaster[colCreateDateBill.Name, rowIndex].Value = master.CreateDate.ToShortDateString();//业务日期
                dgMaster[colStateBill.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);
                dgMaster[colRealOperationDate.Name, rowIndex].Value = master.RealOperationDate;
            }
            lblRecordTotalBill.Text = "共【" + dgMaster.Rows.Count + "】条记录";
            this.txtSumQuantityBill.Text = sumQuantitys.ToString("#,###.####");
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
            MaterialSettleMaster master = dgMaster.CurrentRow.Tag as MaterialSettleMaster;
            if (master == null) return;
            //dgDetail.Rows[rowIndex].Tag = dtl;
            foreach (MaterialSettleDetail dtl in master.Details)
            {
                int rowIndex = dgDetailBill.Rows.Add();
                dgDetailBill[colProjectTaskBill.Name, rowIndex].Value = dtl.ProjectTaskName;          //工程任务名称
                dgDetailBill[colAccountSubjectBill.Name, rowIndex].Value = dtl.AccountCostName;                               //成本核算科目
                dgDetailBill[colMaterialTypeBill.Name, rowIndex].Value = dtl.MaterialName;                               //物资类型
                dgDetailBill[colSpecBill.Name, rowIndex].Value = dtl.MaterialSpec;   //规格型号
                dgDetailBill[colStuffBill.Name, rowIndex].Value = dtl.MaterialStuff;    //材质
                dgDetailBill[colUnitNameBill.Name, rowIndex].Value = dtl.QuantityUnitName;//单位
                dgDetailBill[colQuantityBill.Name, rowIndex].Value = dtl.Quantity;                                        //数量
                dgDetailBill[colPriceBill.Name, rowIndex].Value = dtl.Price;                                           //单价
                dgDetailBill[colSumMoneyBill.Name, rowIndex].Value = dtl.Money;                              //金额            
                dgDetailBill[colDescriptBill.Name, rowIndex].Value = dtl.Descript;                            //备注             
            }
        }




      
    }
}
