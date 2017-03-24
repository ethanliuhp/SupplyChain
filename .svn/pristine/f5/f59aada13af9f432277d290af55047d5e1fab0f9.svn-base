using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Core;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using NHibernate;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.FinanceMng;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost
{
    public partial class VAccountMngQuery : TBasicDataView
    {
        private MIndirectCost model = new MIndirectCost();
        private CurrentProjectInfo projectInfo;
        private EnumAccountMng _ExecType;
        IList opgList = new ArrayList();
        public VAccountMngQuery(EnumAccountMng execType)
        {
            InitializeComponent();
            this._ExecType = execType;
            InitEvent();
            InitData();
        }

        private void InitData()
        {
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddMonths(-3);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            opgList = model.IndirectCostSvr.QuerySubAndCompanyOrgInfo();
            projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
            {
                txtOrgName.Text = projectInfo.Name;
                txtOrgName.Tag = projectInfo;
                this.btnSelectOrgName.Visible = false;
            }
            else if (ConstObject.TheOperationOrg != null)
            {
                txtOrgName.Text = ConstObject.TheOperationOrg.Name;
                txtOrgName.Tag = ConstObject.TheOperationOrg;
            }
        }

        private void InitEvent()
        {
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            //dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            btnSelectAccountTitle.Click += new EventHandler(btnSelectAccountTitle_Click);
            this.btnSelectOrgName.Click+=new EventHandler(btnSelectOrgName_Click);
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }

        void btnSelectAccountTitle_Click(object sender, EventArgs e)
        {
            VAccountTitleTreeSelect oAccountTitleTreeSelect = new VAccountTitleTreeSelect();
            oAccountTitleTreeSelect.IsAllowMulti = true;
            oAccountTitleTreeSelect.ShowDialog();
            if (oAccountTitleTreeSelect.SelectedNodes != null && oAccountTitleTreeSelect.SelectedNodes.Count > 0)
            {
                this.txtAccountTitle.Tag = null;
                this.txtAccountTitle.Text = string.Empty;
                this.txtAccountTitle.Tag =  oAccountTitleTreeSelect.SelectedNodes[0];               
                this.txtAccountTitle.Text = oAccountTitleTreeSelect.SelectedNodes[0].Name;
            }
        }

        void btnSelectOrgName_Click(object sender, EventArgs e)
        {
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect(true);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                OperationOrgInfo ooInfo = frm.Result[0] as OperationOrgInfo;
                txtOrgName.Tag = ooInfo;
                txtOrgName.Text = ooInfo.Name;
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            OperationOrgInfo info = txtOrgName.Tag as OperationOrgInfo;         
            if (info == null && this.btnSelectOrgName.Visible == true)
            {
                MessageBox.Show("[范围选择]不能为空，请选择！");
                return;
            }
            FlashScreen.Show("正在查询信息......");
            try
            {
                #region 设置查询条件
                DateTime startDate = this.dtpDateBegin.Value;
                DateTime endDate = this.dtpDateEnd.Value;
                string createPerson = this.txtCreatePerson.Text;
                string billCode = this.txtCodeBegin.Text;
                string condition = "select tm.CODE,tm.CREATEPERSONNAME,tm.REALOPERATIONDATE,tm.CREATEDATE,tm.OPERORGNAME,tm.PROJECTNAME,tm.OPGSYSCODE,td.ACCOUNTTITLENAME,td.BUDGETMONEY,td.MONEY,td.DESCRIPT from THD_IndirectCostMaster tm inner join THD_IndirectCostDetail td on tm.ID=td.PARENTID";
                if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
                {
                    condition += string.Format(" where tm.PROJECTID='{0}' and tm.CREATEDATE >=to_date('{1}','yyyy-mm-dd') and tm.CREATEDATE<=to_date('{2}','yyyy-mm-dd') ", projectInfo.Id, startDate.Date.ToString("yyyy-MM-dd"), endDate.Date.ToString("yyyy-MM-dd"));
                }
                else
                {
                    string selProjectID = model.IndirectCostSvr.GetProjectIDByOperationOrg(info.Id);
                    condition += string.Format(" where tm.OPGSYSCODE like '%{0}%' and tm.CREATEDATE >=to_date('{1}','yyyy-mm-dd') and tm.CREATEDATE<=to_date('{2}','yyyy-mm-dd') ", info.SysCode, startDate.Date.ToString("yyyy-MM-dd"), endDate.Date.ToString("yyyy-MM-dd"));        
                }
                AccountTitleTree att = txtAccountTitle.Tag as AccountTitleTree;
                if (att != null && !string.IsNullOrEmpty(att.SysCode))
                {

                    condition += string.Format("and td.ACCOUNTTITLESYSCODE like '%{0}%'", att.SysCode);
                }
                if (!string.IsNullOrEmpty(createPerson))
                {
                    condition += string.Format(" and tm.CREATEPERSONNAME like '%{0}%'", createPerson);
                }
                if (!string.IsNullOrEmpty(billCode))
                {
                    condition += string.Format(" and tm.CODE like '%{0}%'", billCode);
                }
                #endregion 
                DataSet ds = null;
                DataTable dt = null;
                ds = model.IndirectCostSvr.QueryAccountMng(condition);
                dt = ds == null || ds.Tables.Count == 0 ? null : ds.Tables[0];
                ShowDetailList(dt);
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
        /// <summary>
        /// 显示主表
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList masterList)
        {
            dgMaster.Rows.Clear();
            dgDetailBill.Rows.Clear();
            if (masterList == null || masterList.Count == 0) return;
            foreach (IndirectCostMaster master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCodeBill.Name, rowIndex].Value = master.Code; //单据号
                dgMaster[colCreatePersonBill.Name, rowIndex].Value = master.CreatePersonName;//制单人
                dgMaster[colDescriptBill.Name, rowIndex].Value = master.Descript;//备注
                dgMaster[this.colSummoneyBill.Name, rowIndex].Value = master.SumMoney.ToString("N2");//金额
                dgMaster[colCreateDateBill.Name, rowIndex].Value = master.CreateDate.ToString("yyyy-MM-dd");//业务日期
                dgMaster[colStateBill.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);//状态
                if (master.RealOperationDate > ClientUtil.ToDateTime("1900-1-1"))
                {
                    
                    dgMaster[colRealOperationDateBill.Name, rowIndex].Value = master.RealOperationDate.ToString("yyyy-MM-dd");//制单时间;
                }
            }
            if (dgMaster.Rows.Count > 0)
            {
                this.dgMaster.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                dgMaster.CurrentCell = dgMaster[1, 0];
                dgMaster_SelectionChanged(dgMaster, new EventArgs());
            }
        }
        public void ShowDetailList(DataTable dataTable)
        {
            if (dataTable != null)
            {
                int iRowIndex = -1;
                this.dgDetail.Rows.Clear();
                if (dataTable.Rows == null ||dataTable.Rows.Count == 0) return;
                int billNum= 0;
                decimal acutualMoneySum = 0;
                foreach (DataRow dr in dataTable.Rows)
                {
                    iRowIndex = this.dgDetail.Rows.Add();
                    this.dgDetail[this.colCode.Name, iRowIndex].Value = ClientUtil.ToString(dr["CODE"]);
                    this.dgDetail[this.colCreatePersonName.Name, iRowIndex].Value = ClientUtil.ToString(dr["CREATEPERSONNAME"]);
                    this.dgDetail[this.colRealOperateDate.Name, iRowIndex].Value = ClientUtil.ToDateTime(dr["REALOPERATIONDATE"]);
                    this.dgDetail[this.colCreateDate.Name, iRowIndex].Value = ClientUtil.ToDateTime(dr["CREATEDATE"]).ToShortDateString();
                   // this.dgDetail[this.colOrgName.Name, iRowIndex].Value = ClientUtil.ToString(dr["OPERORGNAME"]);
                    if (ClientUtil.ToString(dr["PROJECTNAME"])=="")
                    {
                        string syscode = ClientUtil.ToString(dr["OPGSYSCODE"]);
                        string operorgname = ClientUtil.ToString(dr["OPERORGNAME"]);
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
                        this.dgDetail[this.colOrgName.Name, iRowIndex].Value = opgName;
                    }
                    else
                    {
                        this.dgDetail[this.colOrgName.Name, iRowIndex].Value = ClientUtil.ToString(dr["PROJECTNAME"]);
                    }
                    this.dgDetail[this.colAccountTitle.Name, iRowIndex].Value = ClientUtil.ToString(dr["ACCOUNTTITLENAME"]);
                    this.dgDetail[this.colBudgetMoney.Name, iRowIndex].Value = ClientUtil.ToDecimal(dr["BUDGETMONEY"]).ToString("N2");
                    this.dgDetail[this.colActualMoney.Name, iRowIndex].Value = ClientUtil.ToDecimal(dr["MONEY"]).ToString("N2");
                    decimal money = ClientUtil.ToDecimal(dr["MONEY"]);
                    decimal budgetMoney = ClientUtil.ToDecimal(dr["BUDGETMONEY"]);
                    decimal rate = budgetMoney == 0 ? 0 : (Math.Round(money / budgetMoney, 2) * 100);
                    this.dgDetail[this.colRate.Name, iRowIndex].Value = rate.ToString("N2");
                    this.dgDetail[this.colDescript.Name, iRowIndex].Value = ClientUtil.ToString(dr["DESCRIPT"]);
                    billNum++;
                    acutualMoneySum += money;
                }
                this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                this.txtBillNum.Text = billNum.ToString();
                this.txtActualMoneySum.Text = acutualMoneySum.ToString("N2");
            }
        }
        /// <summary>
        /// 主表变化，明细同步变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            int i = 0;
            this.dgDetailBill.Rows.Clear();
            if (this.dgMaster.CurrentRow == null || this.dgMaster.CurrentRow.Tag == null || !(this.dgMaster.CurrentRow.Tag is IndirectCostMaster))
            {
                return;
            }
            IndirectCostMaster oMaster = this.dgMaster.CurrentRow.Tag as IndirectCostMaster;
            foreach (IndirectCostDetail oDetail in oMaster.Details)
            {
                i = this.dgDetailBill.Rows.Add();
                this.dgDetailBill[colAccountTitleBill.Name, i].Value = oDetail.AccountTitleName;

                this.dgDetailBill[colOrgNameBill.Name, i].Value = oDetail.OrgInfoName;
                this.dgDetailBill[this.colBudgetMoneyBill.Name, i].Value = oDetail.BudgetMoney.ToString("N2");
                this.dgDetailBill[this.colActualMoneyBill.Name, i].Value = oDetail.Money;
                this.dgDetailBill[this.colRateBill.Name, i].Value = oDetail.Rate.ToString("N2");
                this.dgDetailBill[this.colRemarkBill.Name, i].Value = oDetail.Descript;
            }
            this.dgDetailBill.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }

   

    }
}