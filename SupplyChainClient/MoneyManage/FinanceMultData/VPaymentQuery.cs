using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Collections;
using VirtualMachine.Core;
using NHibernate;
using NHibernate.Criterion;
using Application.Business.Erp.Financial.Client.FinanceUI.InitialSetting.AccountTitleUI;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VPaymentQuery : TBasicDataView
    {
        private MFinanceMultData model = new MFinanceMultData();
        private MAccountTitle titleModel = new MAccountTitle();
        private CurrentProjectInfo projectInfo = null;
        IList opgList = new ArrayList();
        //{备用金,应交税费,投标保证金,履约保证金,安全保证金,质量保证金,诚信保证金,其他保证金,预付款保证金,农民工工资保证金,风险抵押金,,房租押金,水电押金,处理废材押金,
        //食堂押金,其他押金,散装水泥押金,代业主垫款,代分包垫款,代职工垫款,调入租赁费,调入材料,保险费,政府规费,罚款,其他应收款其他}
        private string[] otherStrs = new string[] { "122101","2221", "22410101", "22410102", "22410106", "22410104", "22410107", "22410199", "12210203", "12210204",
            "22410203", "22410204", "22410205", "22410206", "22410208", "22410220", "12210305", "12210401", "12210402", "12210403", "224194", "224195", "224196", 
            "224197", "224198", "122199" };

        public VPaymentQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }
        private void InitEvent()
        {
            this.btnSearchBill.Click += new System.EventHandler(this.btnSearchBill_Click);
            this.btnExcelBill.Click += new System.EventHandler(this.btnExcelBill_Click);
            this.btnSelRel.Click += new EventHandler(btnSelRel_Click);
            this.btnMainSelRel.Click += new EventHandler(btnMainSelRel_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.btnOperationOrg.Click += new System.EventHandler(this.btnOperationOrg_Click);
            this.btnDtlOperationOrg.Click += new System.EventHandler(this.btnDtlOperationOrg_Click);
        }
        public void InitData()
        {
            this.dtpDateBegin.Value = new DateTime(ConstObject.TheLogin.LoginDate.Year, 1, 1);
            projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
            {
                txtOperationOrg.Text = projectInfo.Name;
                txtOperationOrg.Tag = projectInfo;
                btnOperationOrg.Visible = false;
                this.txtDtlOperationOrg.Text = projectInfo.Name;
                txtDtlOperationOrg.Tag = projectInfo;
                this.btnDtlOperationOrg.Visible = false;
            }
            else if (ConstObject.TheOperationOrg != null)
            {
                txtOperationOrg.Text = ConstObject.TheOperationOrg.Name;
                txtOperationOrg.Tag = ConstObject.TheOperationOrg;
            }
            this.tabControl1.TabPages.Remove(tabControl1.TabPages["tabPage1"]);
            this.dtpDateBeginBill.Value = new DateTime(ConstObject.TheLogin.LoginDate.Year, 1, 1);
            this.dtpDateEndBill.Value = ConstObject.TheLogin.LoginDate;

           //获取付款类别的会计科目
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Or(Expression.Eq("BusinessFlag", "02"), Expression.Eq("BusinessFlag", "01")));
            var acctTitleList = titleModel.AccountTitleTreeSvr.GetAccountTitleTreeByQuery(oq);
            if (acctTitleList == null)
            {
                acctTitleList = new ArrayList();
            }
            IList paymentList = new ArrayList();
            IList otherPaymentList = new ArrayList();
            foreach (AccountTitleTree title in acctTitleList)
            {
                if (title.Code == "220202" || title.Code == "220203" || title.Code == "220204" || title.Code == "220205" || title.Code == "220299"
                    || title.Code == "22020801" || title.Code == "22020802" || title.Code == "22020803" || title.Code == "22020804")
                {
                    paymentList.Add(title);
                }
                else
                {
                    if (title.BusinessFlag == "02" && title.Code != "224199")
                    {
                        title.OrderNo = ((System.Collections.IList)otherStrs).IndexOf(title.Code);
                        otherPaymentList.Add(title);
                    }
                    else
                    {
                        if (title.Code == "122101" || title.Code == "12210203" || title.Code == "12210204" || title.Code == "12210305"
                            || title.Code == "12210401" || title.Code == "12210402" || title.Code == "12210403" || title.Code == "122199")
                        {
                            title.OrderNo = ((System.Collections.IList)otherStrs).IndexOf(title.Code);
                            otherPaymentList.Add(title);
                        }
                    }
                }
            }
            var conPaymentList = (from AccountTitleTree t in otherPaymentList orderby t.OrderNo select t).ToList();
            IList pList = new ArrayList();
            foreach (AccountTitleTree title in paymentList)
            {
                pList.Add(title);
            }
            foreach (AccountTitleTree title in conPaymentList)
            {
                pList.Add(title);
            }
            pList.Insert(0, new AccountTitleTree());

            cmbPaymentType.DataSource = pList;
            cmbPaymentType.DisplayMember = "Name";
            cmbPaymentType.ValueMember = "Id";
            this.cmbDtlPayType.DataSource = pList;
            cmbDtlPayType.DisplayMember = "Name";
            cmbDtlPayType.ValueMember = "Id";

            opgList = model.FinanceMultDataSrv.QuerySubAndCompanyOrgInfo();//分公司和总部信息
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

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex > -1)
            {
                int ifProjectMoney = ClientUtil.ToInt(dgDetail[colState.Name, rowIndex].Tag);
                string sMasterID = ClientUtil.ToString(dgDetail.Rows[rowIndex].Tag);

                VPaymentMng oVPaymentMng = new VPaymentMng(ifProjectMoney);
                oVPaymentMng.Start(sMasterID);
                oVPaymentMng.StartPosition = FormStartPosition.CenterParent;
                oVPaymentMng.ShowDialog();
            }

        }
       
        private void btnExcelBill_Click(object sender, EventArgs e)
        {
            StaticMethod.ExcelClass.SaveDataGridViewToExcel(dgMaster, true);
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
        private void btnDtlOperationOrg_Click(object sender, EventArgs e)
        {
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect(true);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                OperationOrgInfo info = frm.Result[0] as OperationOrgInfo;
                this.txtDtlOperationOrg.Tag = info;
                txtDtlOperationOrg.Text = info.Name;
            }
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
            if (!this.txtDtlPerson.Text.Trim().Equals(""))
            {
                condition += " and t1.CreatePersonName  like '%" + this.txtDtlPerson.Text + "%'";
            }
            if (this.cmbDtlPayType.Text != "")
            {
                condition += " and t1.AccountTitleName like '%" + this.cmbDtlPayType.Text + "%'";
            }
            if (btnSubmit.Checked)
            {
                condition += " and t1.state = 5 ";
            }
            if (this.btnNoSubmit.Checked)
            {
                condition += " and t1.state != 5 ";
            }
            // 项目范围
            if (this.txtDtlOperationOrg.Tag != null)
            {
                if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                {
                    condition += " and t1.ProjectId='" + projectInfo.Id + "'";
                }
                else
                {
                    var org = txtDtlOperationOrg.Tag as OperationOrgInfo;
                    if (org != null)
                    {
                        var sProjectId = model.IndirectCostSvr.GetProjectIDByOperationOrg(org.Id);
                        if (!string.IsNullOrEmpty(sProjectId))
                        {
                            condition += " and t1.ProjectId='" + sProjectId + "'";
                        }
                        else
                        {
                            condition += " and t1.OpgSysCode like '%" + org.SysCode + "%'";
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
                        t1.supplierrelationname,t2.money,t2.paymentmoney,
                        t2.inmaterialmoney,t2.livemoney,t2.othermoney
                        from thd_paymentmaster t1,thd_paymentdetail t2 where t1.id=t2.parentid" + condition;
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
                dgDetail[colRealDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["realoperationdate"]).ToShortDateString();//付款日期
                dgDetail[colCreatePerson.Name, rowIndex].Value = ClientUtil.ToString(dataRow["createpersonname"]);
                dgDetail[colCreateDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["createdate"]).ToShortDateString();
                if (ClientUtil.ToString(dataRow["projectname"]) == "")
                {
                    string syscode = ClientUtil.ToString(dataRow["opgsyscode"]);
                    string operorgname = ClientUtil.ToString(dataRow["operationorgname"]);
                    string opgName = "";
                    foreach (OperationOrgInfo orgInfo in opgList)
                    {
                        if (syscode.Contains(orgInfo.SysCode))
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
                dgDetail[this.colDtlPMoney.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["paymentmoney"]).ToString("N2");
                dgDetail[this.colDtlInMateMoney.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["inmaterialmoney"]).ToString("N2");
                dgDetail[this.colDtlLiveMoney.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["livemoney"]).ToString("N2");
                dgDetail[this.colDtlOtherMoney.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["othermoney"]).ToString("N2");

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

        #region 主子表查询
        private void btnSearchBill_Click(object sender, EventArgs e)
        {
            try
            {
                FlashScreen.Show("正在查询，请稍候...");

                ObjectQuery objectQuery = new ObjectQuery();
                if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                {
                    colBillProject.Visible = false;
                    objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                }
                else if (txtOperationOrg.Tag != null)
                {
                    var org = txtOperationOrg.Tag as OperationOrgInfo;
                    if (org == null)
                    {
                        MessageBox.Show("请选择查询范围");
                        return;
                    }

                    colBillProject.Visible = true;
                    var sProjectId = model.IndirectCostSvr.GetProjectIDByOperationOrg(org.Id);
                    if (!string.IsNullOrEmpty(sProjectId))
                    {
                        objectQuery.AddCriterion(Expression.Eq("ProjectId", sProjectId));
                    }
                    else
                    {
                        objectQuery.AddCriterion(Expression.Like("OpgSysCode", org.SysCode, MatchMode.Start));
                    }
                }
                else
                {
                    MessageBox.Show("请选择查询范围");
                    return;
                }

                if (!string.IsNullOrEmpty(txtCodeBeginBill.Text.Trim()))
                {
                    objectQuery.AddCriterion(Expression.Like("Code", txtCodeBeginBill.Text.Trim(), MatchMode.Anywhere));
                }

                if (!string.IsNullOrEmpty(this.txtPerson.Text))
                {
                    objectQuery.AddCriterion(Expression.Like("CreatePersonName", txtPerson.Text, MatchMode.Anywhere));
                }

                if (!string.IsNullOrEmpty(this.txtMainOrgName.Text))
                {
                    objectQuery.AddCriterion(Expression.Or(Expression.Like("TheSupplierName", this.txtMainOrgName.Text, MatchMode.Anywhere),
                        Expression.Like("TheCustomerName", this.txtMainOrgName.Text, MatchMode.Anywhere)));
                }

                if (!string.IsNullOrEmpty(this.cmbPaymentType.Text))
                {
                    objectQuery.AddCriterion(Expression.Like("AccountTitleName", this.cmbPaymentType.Text, MatchMode.Anywhere));
                }

                objectQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBeginBill.Value.Date));
                objectQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEndBill.Value.AddDays(1).Date));

                objectQuery.AddFetchMode("Details", FetchMode.Eager);
                objectQuery.AddFetchMode("ListInvoice", FetchMode.Eager);
                objectQuery.AddFetchMode("ListRel", FetchMode.Eager);
                objectQuery.AddFetchMode("Details.AcceptBillID", FetchMode.Eager);
                var list = model.FinanceMultDataSrv.Query(typeof(PaymentMaster), objectQuery);
                ShowQueryResult(list);

                FlashScreen.Close();
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show(string.Format("查询付款单信息失败：{0}", ex.Message));
            }
        }
        private void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            dgDetailBill.Rows.Clear();
            var curBillMaster = dgMaster.CurrentRow.Tag as PaymentMaster;
            if (curBillMaster == null || curBillMaster.Details == null)
            {
                return;
            }

            var i = 0;
            foreach (PaymentDetail var in curBillMaster.Details)
            {
                i = this.dgDetailBill.Rows.Add();

                this.dgDetailBill[this.colDtlMoney.Name, i].Value = var.PaymentMoney;
                this.dgDetailBill[this.colInMaterialMoney.Name, i].Value = var.InMaterialMoney;
                this.dgDetailBill[this.colLiveMoney.Name, i].Value = var.LiveMoney;
                this.dgDetailBill[this.colOtherMoney.Name, i].Value = var.OtherMoney;
                this.dgDetailBill[this.colDtlSummoneyBill.Name, i].Value = var.Money;
                this.dgDetailBill[this.colDtlDescBill.Name, i].Value = var.Descript;

                if (var.AcceptBillID != null)
                {
                    this.dgDetailBill[this.colDtlBillNo.Name, i].Value = var.AcceptBillID.BillNo;
                    this.dgDetailBill[this.colDtlBillDate.Name, i].Value = var.AcceptBillID.CreateDate;
                    this.dgDetailBill[this.colDtlBillType.Name, i].Value = var.AcceptBillID.BillType;
                    this.dgDetailBill[this.colExpireDate.Name, i].Value = var.AcceptBillID.ExpireDate;
                    this.dgDetailBill[this.colDtlBillPerson.Name, i].Value = var.OtherMoney;
                }
            }
        }
        private void ShowQueryResult(IList masterList)
        {
            dgMaster.Rows.Clear();
            if (masterList == null || masterList.Count == 0)
            {
                return;
            }

            var rowIndex = 0;
            foreach (PaymentMaster master in masterList)
            {
                rowIndex = dgMaster.Rows.Add();

                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colRowNo.Name, rowIndex].Value = rowIndex + 1;
                dgMaster[colCodeBill.Name, rowIndex].Value = master.Code; //单据号
                dgMaster[colBillProject.Name, rowIndex].Value = master.ProjectName; //项目
                dgMaster[colCreatePersonBill.Name, rowIndex].Value = master.CreatePersonName;//制单人
                dgMaster[colDescriptBill.Name, rowIndex].Value = master.Descript;//备注
                if (ClientUtil.ToString(master.TheCustomerName) == "")
                {
                    dgMaster[this.colPayOrg.Name, rowIndex].Value = master.TheSupplierName;//单位
                }
                else
                {
                    dgMaster[this.colPayOrg.Name, rowIndex].Value = master.TheCustomerName;//单位
                }
                dgMaster[this.colPayType.Name, rowIndex].Value = master.AccountTitleName;//类别
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

            dgMaster_SelectionChanged(dgMaster, null);
        }
        #endregion
    }
}
