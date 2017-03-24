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
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using AuthManagerLib.AuthMng.AuthConfigMng.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using NHibernate;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;
using Application.Business.Erp.Financial.Client.FinanceUI.InitialSetting.AccountTitleUI;
using Application.Business.Erp.SupplyChain.Client.FinanceMng;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VGatheringQuery : TBasicDataView
    {
        private string sIndirectCostAccountTitleCode = "6602";
        private MFinanceMultData model = new MFinanceMultData();
        private MAccountTitle titleModel = new MAccountTitle();
        private CurrentProjectInfo projectInfo = null;
        IList opgList = new ArrayList();
        //{备用金,投标保证金,履约保证金,预付款保证金,农民工工资保证金,安全保证金,质量保证金,诚信保证金,其他保证金,房租押金,水电押金,散装水泥押金,
        //其他押金,食堂押金,处理废材押金,风险抵押金,代业主垫款,代分包垫款,代职工垫款,调出租赁费,调出材料,配合费,罚款,其他应付其他}
        private string[] otherStrs = new string[] { "122101", "12210201", "12210202", "12210203", "12210204", "12210205", "12210206", "12210207", "12210299" 
            , "12210303", "12210304", "12210305", "12210310" ,"22410208","22410206","22410203","12210401","12210402","12210403","122194","122195","122196","122197","224199"};

        public VGatheringQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        private void InitData()
        {
            this.dtpDateBegin.Value = new DateTime(ConstObject.TheLogin.LoginDate.Year, 1, 1);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            this.dtpDateBeginBill.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEndBill.Value = ConstObject.TheLogin.LoginDate;
            dgDetailBill.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);

            this.tabControl1.TabPages.Remove(tabControl1.TabPages["tabPage1"]);

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Or(Expression.Eq("BusinessFlag", "02"), Expression.Eq("BusinessFlag", "01")));
            IList acctTitleList = titleModel.AccountTitleTreeSvr.GetAccountTitleTreeByQuery(oq);
            IList otherGatherList = new ArrayList();
            IList gatheringList = new ArrayList();
            foreach (AccountTitleTree title in acctTitleList)
            {
                if (title.Code == "112201" || title.Code == "112206")
                {
                    gatheringList.Add(title);
                }
                else
                {
                    if (title.BusinessFlag == "01" && title.Code != "122199")
                    {
                        title.OrderNo = ((System.Collections.IList)otherStrs).IndexOf(title.Code);
                        otherGatherList.Add(title);
                    }
                    else
                    {
                        if (title.Code == "224199" || title.Code == "22410206" || title.Code == "22410203" || title.Code == "22410208")
                        {
                            title.OrderNo = ((System.Collections.IList)otherStrs).IndexOf(title.Code);
                            otherGatherList.Add(title);
                        }
                    }
                }
            }
            var newOtherList = (from AccountTitleTree t in otherGatherList orderby t.OrderNo select t).ToList();
            IList pList = new ArrayList();
            foreach (AccountTitleTree title in gatheringList)
            {
                pList.Add(title);
            }
            foreach (AccountTitleTree title in newOtherList)
            {
                pList.Add(title);
            }
            pList.Insert(0, new AccountTitleTree());
            cmbGatheringType.DataSource = pList;
            cmbGatheringType.DisplayMember = "Name";
            cmbGatheringType.ValueMember = "Id";
            cmbDtlGatherType.DataSource = pList;
            cmbDtlGatherType.DisplayMember = "Name";
            cmbDtlGatherType.ValueMember = "Id";

            projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
            {
                txtOperationOrg.Text = projectInfo.Name;
                txtOperationOrg.Tag = projectInfo;
                this.btnOperationOrg.Visible = false;
            }
            else if (ConstObject.TheOperationOrg != null)
            {
                txtOperationOrg.Text = ConstObject.TheOperationOrg.Name;
                txtOperationOrg.Tag = ConstObject.TheOperationOrg;
            }

            opgList = model.FinanceMultDataSrv.QuerySubAndCompanyOrgInfo();//分公司和总部信息
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
            //dgMaster.CellContentClick+=new DataGridViewCellEventHandler(dgMaster_CellContentClick);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            this.btnSelRel.Click += new EventHandler(btnSelRel_Click);
            this.btnMainSelRel.Click += new EventHandler(btnMainSelRel_Click);
            //btnSelectAccountTitle.Click += new EventHandler(btnSelectAccountTitle_Click);
            btnOperationOrg.Click += new EventHandler(btnOperationOrg_Click);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
        }
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex > -1)
            {
                int iIfProjectMoney = ClientUtil.ToInt(dgDetail[colState.Name, rowIndex].Tag);
                string sMasterID = ClientUtil.ToString(dgDetail.Rows[rowIndex].Tag);
                //GatheringMaster master = model.GatheringSrv.GetGatheringByCode(dgvCell.Value.ToString());

                VGatheringMng oVGatheringMng = new VGatheringMng(iIfProjectMoney);
                oVGatheringMng.Start(sMasterID);
               //oVGatheringMng.Preview();
                oVGatheringMng.StartPosition = FormStartPosition.CenterParent;
                oVGatheringMng.ShowDialog();
            }

        }
        void dgMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgMaster[e.ColumnIndex, e.RowIndex];
            if (dgvCell.OwningColumn.Name == colCodeBill.Name)
            {
                //GatheringMaster master = model.GatheringSrv.GetGatheringByCode(dgvCell.Value.ToString());
                GatheringMaster master = dgMaster.Rows[e.RowIndex].Tag as GatheringMaster;
                VGatheringMng vmro = new VGatheringMng(master.IfProjectMoney);
                vmro.CurBillMaster = master;
                vmro.Preview();
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }
        void btnSelRel_Click(object sender, EventArgs e)
        {
            VCusAndSupRelSelector vSelector = new VCusAndSupRelSelector();
            vSelector.ShowDialog();
            IList list = vSelector.Result;
            if (list == null || list.Count == 0) return;
            DataDomain domain = list[0] as DataDomain;
            this.txtOrgName.Tag = domain;
            this.txtOrgName.Text = ClientUtil.ToString(domain.Name4);
        }
        void btnMainSelRel_Click(object sender, EventArgs e)
        {
            VCusAndSupRelSelector vSelector = new VCusAndSupRelSelector();
            vSelector.ShowDialog();
            IList list = vSelector.Result;
            if (list == null || list.Count == 0) return;
            DataDomain domain = list[0] as DataDomain;
            this.txtMainOrgName.Tag = domain;
            this.txtMainOrgName.Text = ClientUtil.ToString(domain.Name4);
        }
        void btnSearch_Click(object sender, EventArgs e)
        {
            #region 查询条件处理
            FlashScreen.Show("正在查询信息......");
            ObjectQuery oq = new ObjectQuery();
            string condition = "";
            //CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            //condition += "and t1.ProjectId = '" + projectInfo.Id + "'  and t1.plantype=1 ";
            //单据号
            if (this.txtCodeBegin.Text != "")
            {
                condition = condition + " and t1.Code like '%" + this.txtCodeBegin.Text + "%'";//模糊查询
            }
            // 付款日期
            if (StaticMethod.IsUseSQLServer())
            {
                condition += " and t1.createdate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and t1.createdate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
            }
            else
            {
                condition += " and t1.createdate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.createdate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
            }
            // 制单人
            if (!this.txtDtlPerson.Text.Trim().Equals("") )
            {
                condition += " and t1.CreatePersonName  like '%" + this.txtDtlPerson.Text + "%'";
            }
            if (btnSubmit.Checked)
            {
                condition += " and t1.state = 5 ";
            }
            if (this.btnNoSubmit.Checked)
            {
                condition += " and t1.state != 5 ";
            }
            // 会计科目
            //if (this.txtAccountTitle.Tag != null)
            //{
            //    var temp = ((string[])this.txtAccountTitle.Tag).Select(a => "'" + a + "'").ToArray();
            //    condition += " and t1.AccountTitleID in (" + string.Join(",", temp) + ")";
            //}
            if (this.cmbDtlGatherType.Text != "")
            {
                condition += " and t1.AccountTitleName like '%" + this.cmbDtlGatherType.Text + "%'";
            }
            // 项目范围
            if (txtOperationOrg.Tag != null)
            {
                if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                {
                    condition += " and t1.ProjectId='" + projectInfo.Id + "'";
                }
                else
                {
                    var org = txtOperationOrg.Tag as OperationOrgInfo;
                    if (org != null)
                    {

                        var sProjectId = model.IndirectCostSvr.GetProjectIDByOperationOrg(org.Id);
                        if (!string.IsNullOrEmpty(sProjectId))
                        {
                            condition += " and ProjectId='" + sProjectId + "'";
                        }
                        else
                        {
                            condition += " and OpgSysCode like '%" + org.SysCode + "%'";
                        }
                    }
                }
            }
            // 单位
            if (this.txtOrgName.Text != "")
            {
                condition += string.Format(" and (thecustomername like '%{0}%' or supplierrelationname like '%{0}%')", this.txtOrgName.Text);
            }
            condition = @"select t1.id,t1.ifprojectmoney,t1.code,t1.createdate,t1.createpersonname,t1.realoperationdate,t1.opgsyscode,t1.operationorgname,
                        t1.accounttitlename,accounttitlecode,t1.thecustomername,t1.projectname,t1.descript,t1.state,
                        t1.supplierrelationname,t2.money,t2.gatheringmoney,
                        t2.WATERELECMONEY,t2.PENALTYMONEY,t2.WORKERMONEY,t2.CONCRETEMONEY,t2.AGREEMENTMONEY,t2.OTHERMONEY,t2.OTHERITEMMONEY
                        from thd_gatheringmaster t1,thd_gatheringdetail t2 where t1.id=t2.parentid" + condition;
            #endregion
            DataSet dataSet = model.FinanceMultDataSrv.QueryGatheringByCondition(condition);
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

                object objState = dataRow["state"];
                if (objState != null)
                {
                    dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                    
                }
                dgDetail[colState.Name, rowIndex].Tag = ClientUtil.ToInt(dataRow["ifprojectmoney"]);
                dgDetail[colRealDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["realoperationdate"]).ToShortDateString();//收款日期
                dgDetail[colCreatePerson.Name, rowIndex].Value = ClientUtil.ToString(dataRow["createpersonname"]);
                dgDetail[colCreateDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["createdate"]).ToShortDateString();
                if (ClientUtil.ToString(dataRow["projectname"]) == "")
                {
                    string syscode = ClientUtil.ToString(dataRow["opgsyscode"]);
                    string operorgname = ClientUtil.ToString(dataRow["operationorgname"]);
                    string opgName = "";
                    foreach (OperationOrgInfo orgInfo in opgList)
                    {
                        if(syscode.Contains(orgInfo.SysCode))
                        {
                            opgName = orgInfo.Name + "-" + operorgname;
                        }
                    }
                    if (opgName == "")
                    {
                        opgName = "公司总部" + "-" + operorgname;
                    }
                    dgDetail[colProjectName.Name, rowIndex].Value = opgName;
                }
                else
                {
                    dgDetail[colProjectName.Name, rowIndex].Value = ClientUtil.ToString(dataRow["projectname"]);
                }
                dgDetail[colAccountTitleName.Name, rowIndex].Value = ClientUtil.ToString(dataRow["accounttitlename"]);
                dgDetail[colAccountTitleCode.Name, rowIndex].Value = ClientUtil.ToString(dataRow["accounttitlecode"]);
                dgDetail[colCustomerName.Name, rowIndex].Value = ClientUtil.ToString(dataRow["thecustomername"]);
                dgDetail[colSupplierName.Name, rowIndex].Value = ClientUtil.ToString(dataRow["supplierrelationname"]);
                dgDetail[this.colGMoney1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["gatheringmoney"]).ToString("N2");
                dgDetail[this.colAgreementMoney1.Name, rowIndex].Value =ClientUtil.ToDecimal( dataRow["AgreementMoney"]).ToString("N2");
                dgDetail[this.colConcreteMoney1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["ConcreteMoney"]).ToString("N2");
                dgDetail[this.colOtherItemMoney1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["OtherItemMoney"]).ToString("N2");
                dgDetail[this.colOtherMoney1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["OtherMoney"]).ToString("N2");
                dgDetail[this.colPenaltyMoney1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["PenaltyMoney"]).ToString("N2");
                dgDetail[this.colWaterElecMoney1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["WaterElecMoney"]).ToString("N2");
                dgDetail[this.colWorkerMoney1.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["WorkerMoney"]).ToString("N2");
            
                decimal money = Convert.ToDecimal(dataRow["money"]);
                sumQuantity += money;
                dgDetail[colMoney.Name, rowIndex].Value = money;
                dgDetail[colDescript.Name, rowIndex].Value = dataRow["descript"];
            }
            FlashScreen.Close();
            this.txtSumMoney.Text = sumQuantity.ToString("#,###.####");
            lblRecordTotal.Text = "共【" + dataTable.Rows.Count + "】条记录";
            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);

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
                objectQuery.AddCriterion(Expression.Like("Code", txtCodeBeginBill.Text, MatchMode.Anywhere));
            }
            if (this.txtMainOrgName.Text != "")
            {
                objectQuery.AddCriterion(Expression.Or(Expression.Like("TheSupplierName", this.txtMainOrgName.Text, MatchMode.Anywhere),
                    Expression.Like("TheCustomerName", this.txtMainOrgName.Text, MatchMode.Anywhere)));
            }
            if (this.cmbGatheringType.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("AccountTitleName", this.cmbGatheringType.Text, MatchMode.Anywhere));
            }
            //业务日期
            objectQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBeginBill.Value.Date));
            objectQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEndBill.Value.AddDays(1).Date));
            //制单人
            if (this.txtPerson.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("CreatePersonName", txtPerson.Text, MatchMode.Anywhere));
            }
            try
            {
                objectQuery.AddFetchMode("Details", FetchMode.Eager);
                objectQuery.AddFetchMode("ListInvoice", FetchMode.Eager);
                objectQuery.AddFetchMode("ListRel", FetchMode.Eager);
                objectQuery.AddFetchMode("Details.AcceptBillID", FetchMode.Eager);
                list = model.FinanceMultDataSrv.Query(typeof(GatheringMaster), objectQuery);
                dgMaster.Rows.Clear();
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
            foreach (GatheringMaster master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCodeBill.Name, rowIndex].Value = master.Code; //单据号
                dgMaster[colCreatePersonBill.Name, rowIndex].Value = master.CreatePersonName;//制单人
                dgMaster[colDescriptBill.Name, rowIndex].Value = master.Descript;//备注
                if (ClientUtil.ToString(master.TheCustomerName) == "")
                {
                    dgMaster[this.colGatheringOrg.Name, rowIndex].Value = master.TheSupplierName;//单位
                }
                else
                {
                    dgMaster[this.colGatheringOrg.Name, rowIndex].Value = master.TheCustomerName;//单位
                }
                dgMaster[this.colGatheringTypeBill.Name, rowIndex].Value = master.AccountTitleName;//类别
                dgMaster[this.colSummoneyBill.Name, rowIndex].Value = master.SumMoney;//金额
                dgMaster[colCreateDateBill.Name, rowIndex].Value = master.CreateDate.ToShortDateString();//业务日期
                dgMaster[colStateBill.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);//状态
                if (master.RealOperationDate > ClientUtil.ToDateTime("1900-1-1"))
                {
                    dgMaster[colRealOperationDateBill.Name, rowIndex].Value = master.RealOperationDate.ToShortDateString();//制单时间;
                }
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
            GatheringMaster curBillMaster = dgMaster.CurrentRow.Tag as GatheringMaster;
            if (curBillMaster == null) return;
            foreach (GatheringDetail var in curBillMaster.Details)
            {
                int i = this.dgDetailBill.Rows.Add();

                this.dgDetailBill[this.colDtlDescBill.Name, i].Value = var.Descript;
                this.dgDetailBill[this.colDtlMoney.Name, i].Value = var.GatheringMoney;
                this.dgDetailBill[this.colAgreementMoney.Name, i].Value = var.AgreementMoney;
                this.dgDetailBill[this.colConcreteMoney.Name, i].Value = var.ConcreteMoney;
                this.dgDetailBill[this.colOtherMoney.Name, i].Value = var.OtherMoney;
                this.dgDetailBill[this.colPenaltyMoney.Name, i].Value = var.PenaltyMoney;
                this.dgDetailBill[this.colWaterElecMoney.Name, i].Value = var.WaterElecMoney;
                this.dgDetailBill[this.colWorkerMoney.Name, i].Value = var.WorkerMoney;
                this.dgDetailBill[this.colDtlSummoneyBill.Name, i].Value = var.Money;
                AcceptanceBill acceptBill = var.AcceptBillID;//票据信息
                if (acceptBill != null)
                {
                    this.dgDetailBill[this.colDtlBillNo.Name, i].Value = acceptBill.BillNo;
                    this.dgDetailBill[this.colDtlBillDate.Name, i].Value = acceptBill.CreateDate;
                    this.dgDetailBill[this.colDtlBillType.Name, i].Value = acceptBill.BillType;
                    this.dgDetailBill[this.colExpireDate.Name, i].Value = acceptBill.ExpireDate;
                }
            }

            dgInvoice.Rows.Clear();
            foreach (GatheringInvoice var in curBillMaster.ListInvoice)
            {
                int i = this.dgInvoice.Rows.Add();
                this.dgInvoice[this.colInvoiceCode.Name, i].Value = var.InvoiceCode;
                this.dgInvoice[this.colInvoiceNo.Name, i].Value = var.InvoiceNo;
                this.dgInvoice[this.colInvoiceMoney.Name, i].Value = var.SumMoney;
                this.dgInvoice[this.colInvoiceDate.Name, i].Value = var.CreateDate;
                this.dgInvoice[this.colInvoiceDesc.Name, i].Value = var.Descript;
            }
            this.dgOwnerQty.Rows.Clear();
            foreach (GatheringAndQuantityRel var in curBillMaster.ListRel)
            {
                int i = this.dgOwnerQty.Rows.Add();
                DataDomain bill = model.FinanceMultDataSrv.QueryOwnerQuantityInfoByMxID(var.OwnerQuantityMxID.Id);
                this.dgOwnerQty[this.colOwnerBillCode.Name, i].Value = bill.Name2;
                this.dgOwnerQty[this.colOwnerBillCode.Name, i].Tag = var.OwnerQuantityMxID.Id;
                this.dgOwnerQty[this.colOwnerDate.Name, i].Value = ClientUtil.ToDateTime(bill.Name3).ToShortDateString();
                this.dgOwnerQty[this.colOwnerMoney.Name, i].Value = bill.Name4;
                this.dgOwnerQty[this.colOwnerOkMoney.Name, i].Value = bill.Name5;
                this.dgOwnerQty[this.colQWBSName.Name, i].Value = bill.Name6;
                this.dgOwnerQty[this.colQGatherMoney.Name, i].Value = var.GatheringMoney;
            }
        }

        private void btnSelectAccountTitle_Click(object sender, EventArgs e)
        {
            VAccountTitleTreeSelect oAccountTitleTreeSelect = new VAccountTitleTreeSelect(sIndirectCostAccountTitleCode);
            oAccountTitleTreeSelect.IsAllowMulti = true;
            oAccountTitleTreeSelect.ShowDialog();
        }

        private void btnOperationOrg_Click(object sender, EventArgs e)
        {
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect(true);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                OperationOrgInfo info = frm.Result[0] as OperationOrgInfo;
                txtOperationOrg.Tag = info;
                txtOperationOrg.Text = info.Name;
            }
        }
    }
}
