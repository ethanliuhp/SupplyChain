﻿using System;
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
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborSporadicManagement.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using VirtualMachine.Core;
using System.Collections;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng
{
    public partial class VLaborSporadicQueryByBill : TBasicDataView
    {
        private MLaborSporadicMng model = new MLaborSporadicMng();
        string strName = "";
        public VLaborSporadicQueryByBill(string name)
        {
            InitializeComponent();
            strName = name;
            if (strName == "零星用工单生产查询")
            {
                dgDetail.Columns["colAccountQuantity"].Visible = false;
                dgDetail.Columns["colAccountPrice"].Visible = false;
                dgDetail.Columns["colAccountMoney"].Visible = false;
            }
            InitData();
            InitEvent();
            InitSporadicType();
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
                    comMngType1.Items.Add(li);
                }
            }
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
        }
        private void InitSporadicType()
        {
            //派工类型
            txtLaborType.Items.AddRange(new object[] { "计时派工", "零星机械" });//, "分包签证"
            this.txtMxLaborType.Items.AddRange(new object[] { "计时派工", "零星机械" });//, "分包签证" 
        }
        public void InitEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnSearchBill.Click += new EventHandler(btnSearchBill_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            btnExcelBill.Click += new EventHandler(btnExcelBill_Click);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            this.btnChoice.Click += new EventHandler(btnChoice_Click);
            this.btnChoiceBill.Click += new EventHandler(btnChoiceBill_Click);
            dgMaster.CellContentClick += new DataGridViewCellEventHandler(dgMaster_CellContentClick);

        }

        void dgMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgMaster[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;

            if (dgvCell.OwningColumn.Name == colCode.Name)
            {
                Preview();
            }
        }
        //Excel
        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgMaster, true);
        }
        //Excel
        void btnExcelBill_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetailBill, true);
        }
        void btnChoice_Click(object sender, EventArgs e)
        {
            if (btnChoice.Enabled == true)
            {
                VContractExcuteSelector vmros = new VContractExcuteSelector();
                vmros.ShowDialog();
                IList list = vmros.Result;
                if (list == null || list.Count == 0) return;
                SubContractProject engineerMaster = list[0] as SubContractProject;
                txtSupply.Text = engineerMaster.BearerOrgName;
                txtSupply.Tag = engineerMaster;
            }
        }
        void btnChoiceBill_Click(object sender, EventArgs e)
        {
            if (btnChoiceBill.Enabled == true)
            {
                VContractExcuteSelector vmros = new VContractExcuteSelector();
                vmros.ShowDialog();
                IList list = vmros.Result;
                if (list == null || list.Count == 0) return;
                SubContractProject engineerMaster = list[0] as SubContractProject;
                txtSupplyBill.Text = engineerMaster.BearerOrgName;
                txtSupplyBill.Tag = engineerMaster;
            }
        }
        //查找事件
        void btnSearch_Click(object sender, EventArgs e)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            FlashScreen.Show("正在查询信息......");
            IList list = new ArrayList();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            //当前项目
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            //单据
            if (txtCodeBegin.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("Code", txtCodeBegin.Text, MatchMode.Anywhere));
            }
            //创建时间
            objectQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value.Date));
            objectQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));
            //创建人
            if (txtCreatePerson.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("CreatePersonName", txtCreatePerson.Text, MatchMode.Anywhere));
            }
            //承担队伍
            if (txtSupply.Text != "")
            {
                objectQuery.AddCriterion(Expression.Eq("BearTeam", txtSupply.Tag as SubContractProject));
                //condition += "and t1.bearTeamName = '" + txtSupply.Text + "' and t1.bearTeam = '" + (txtSupply.Tag as SubContractProject).Id + "'";
            }
            if (comMngType.Text != "")
            {
                System.Web.UI.WebControls.ListItem li = comMngType.SelectedItem as System.Web.UI.WebControls.ListItem;
                int values = ClientUtil.ToInt(li.Value);
                //objectQuery.AddCriterion(Expression.Eq("DocState",(DocumentState)values));
            }
            //else
            //{
            //    objectQuery.AddCriterion(Expression.Not(Expression.Eq("DocState", DocumentState.Edit)));//单据状态为非编辑状态的才能查询出来
            //}
            //派工类型
            if (this.txtLaborType.SelectedItem != null)
            {
                //前期的工单都是“零星用工”，后期的都是“零星机械”，所以写成这样     
                if (txtLaborType.SelectedItem + "" == "零星机械")
                {
                    Disjunction dis = new Disjunction();
                    dis.Add(Expression.Eq("LaborState", "零星用工"));
                    dis.Add(Expression.Eq("LaborState", "零星机械"));
                    objectQuery.AddCriterion(dis);
                }
                else
                {
                    objectQuery.AddCriterion(Expression.Like("LaborState", txtLaborType.SelectedItem));
                }
            }
            else
            {
                Disjunction dis = new Disjunction();
                dis.Add(Expression.Eq("LaborState", "零星用工"));
                dis.Add(Expression.Eq("LaborState", "零星机械"));
                dis.Add(Expression.Eq("LaborState", "计时派工"));
                dis.Add(Expression.Eq("LaborState", "分包签证"));
                objectQuery.AddCriterion(dis);
                //objectQuery.AddCriterion(Expression.Or(Expression.Eq("LaborState", "零星用工"), Expression.Eq("LaborState", "计时派工")));
                objectQuery.AddCriterion(Expression.Not(Expression.Eq("LaborState", "代工")));
            }
            try
            {
                list = model.LaborSporadicSrv.GetLaborSporadic(objectQuery);
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
            foreach (LaborSporadicMaster master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCode.Name, rowIndex].Value = master.Code;
                dgMaster[colTaskHandler.Name, rowIndex].Value = master.BearTeamName;
                dgMaster[colSporadicType.Name, rowIndex].Value = master.LaborState;
                dgMaster[colCreatePerson.Name, rowIndex].Value = master.CreatePersonName;//制单人
                dgMaster[colCreateDate.Name, rowIndex].Value = master.CreateDate.ToShortDateString();//业务日期
                if (master.RealOperationDate > ClientUtil.ToDateTime("1900-1-1"))
                {
                    dgMaster[colRealOperationDate.Name, rowIndex].Value = master.RealOperationDate.ToShortDateString();     //制单时间;
                }
                //状态
                //object objState = ;
                //if (objState != null)
                //{
                if (master.DocState == DocumentState.InAudit)
                {
                    dgMaster[colState.Name, rowIndex].Value = "待复核";
                }
                if (master.DocState == DocumentState.InExecute)
                {
                    dgMaster[colState.Name, rowIndex].Value = "已审核";
                }
                else
                {
                    dgMaster[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);
                }
            }
            dgMaster.CurrentCell = dgMaster[1, 0];
            dgMaster_SelectionChanged(dgMaster, new EventArgs());
        }
        //主表变化
        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            dgDetail.Rows.Clear();
            LaborSporadicMaster master = dgMaster.CurrentRow.Tag as LaborSporadicMaster;
            if (master == null) return;
            foreach (LaborSporadicDetail dtl in master.Details)
            {
                int rowIndex = dgDetail.Rows.Add();
                dgDetail.Rows[rowIndex].Tag = dtl;
                dgDetail[colProjectTastType.Name, rowIndex].Value = dtl.ProjectTastName;//工程任务类型
                dgDetail[colProjectTastDetail.Name, rowIndex].Value = dtl.ProjectTastDetailName;//工程任务明细
                dgDetail[colLaborSubject.Name, rowIndex].Value = dtl.LaborSubjectName;//用工科目
                dgDetail[colRealSporadicNum.Name, rowIndex].Value = dtl.RealLaborNum;//实际用工量
                dgDetail[colQuantityUnit.Name, rowIndex].Value = dtl.QuantityUnitName;//数量单位
                dgDetail[colPriceUnit.Name, rowIndex].Value = dtl.PriceUnitName;//价格计量单位
                dgDetail[colAccountQuantity.Name, rowIndex].Value = dtl.AccountLaborNum;//核算工程量
                dgDetail[colAccountPrice.Name, rowIndex].Value = dtl.AccountPrice;//核算单价
                dgDetail[colAccountMoney.Name, rowIndex].Value = dtl.AccountSumMoney;//核算合价
                string strIsCreate = dtl.IsCreate.ToString();//是否核算
                if (strIsCreate.Equals("0"))
                {
                    dgDetail[colIsCreate.Name, rowIndex].Value = "未审核";
                }
                if (strIsCreate.Equals("1"))
                {
                    dgDetail[colIsCreate.Name, rowIndex].Value = "已审核";
                }
                dgDetail[colStartDate.Name, rowIndex].Value = dtl.StartDate.ToShortDateString();//开始时间
                dgDetail[colEndDate.Name, rowIndex].Value = dtl.EndDate.ToShortDateString();//结束时间
                if (dtl.CompleteDate > ClientUtil.ToDateTime("1900-01-01"))
                {
                    dgDetail[colComPleteDate.Name, rowIndex].Value = dtl.CompleteDate.ToShortDateString();//业务完成时间
                }
                dgDetail[colLaborDescript.Name, rowIndex].Value = dtl.LaborDescript;//用工说明
                if (ClientUtil.ToString(dtl.BalanceDtlGUID) == "")
                {
                    dgDetail[colSettleDate.Name, rowIndex].Value = "未结算";
                }
                else
                {
                    dgDetail[colSettleDate.Name, rowIndex].Value = "已结算";
                }
            }
        }

        //明细显示查询事件
        void btnSearchBill_Click(object sender, EventArgs e)
        {
            try
            {
                FlashScreen.Show("正在查询信息......");
                string condition = "";
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                projectInfo = StaticMethod.GetProjectInfo();
                condition += "and t1.ProjectId = '" + projectInfo.Id + "'";
                //单据
                if (txtCodeBegin.Text != "")
                {
                    condition += "and t1.Code like '%" + txtCodeBeginBill.Text + "%'";
                }
                //业务日期
                if (StaticMethod.IsUseSQLServer())
                {
                    condition += " and t1.CreateDate>='" + dtpDateBeginBill.Value.Date.ToShortDateString() + "' and t1.CreateDate<'" + dtpDateEndBill.Value.AddDays(1).Date.ToShortDateString() + "'";
                }
                else
                {
                    condition += " and t1.CreateDate>=to_date('" + dtpDateBeginBill.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<to_date('" + dtpDateEndBill.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
                }
                //创建人
                if (txtCreatePersonBill.Text != "")
                {
                    condition = condition + " and t1.createPersonName='" + (txtCreatePersonBill.Result[0] as PersonInfo).Name + "'";
                }
                //承担队伍
                if (txtSupplyBill.Text != "")
                {
                    condition += "and t1.bearTeamName = '" + txtSupplyBill.Text + "' and t1.bearTeam = '" + (txtSupplyBill.Tag as SubContractProject).Id + "'";

                }
                //派工类型
                if (this.txtMxLaborType.SelectedItem != null)
                {
                    //前期的工单都是“零星用工”，后期的都是“零星机械”，所以写成这样     
                    if (txtLaborType.SelectedItem + "" == "零星机械")
                    {
                        condition += " and (t1.LaborState = '零星用工' or t1.LaborState = '零星机械' ) ";
                    }
                    else
                    {
                        condition += " and t1.LaborState = '" + txtMxLaborType.SelectedItem + "' ";
                    }
                }
                //派工类型
                condition += " and t1.LaborState <> '代工' ";
                if (comMngType1.Text != "")
                {
                    System.Web.UI.WebControls.ListItem li = comMngType1.SelectedItem as System.Web.UI.WebControls.ListItem;
                    int values = ClientUtil.ToInt(li.Value);
                    condition += "and t1.State= '" + values + "'";
                }
                DataSet dataSet = model.LaborSporadicSrv.LaborSporadicQuery(condition);
                this.dgDetailBill.Rows.Clear();
                DataTable dataTable = dataSet.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int rowIndex = this.dgDetailBill.Rows.Add();
                    dgDetailBill[colCodeBill.Name, rowIndex].Value = dataRow["code"];
                    object objState = dataRow["State"];
                    if (objState != null)
                    {
                        dgDetailBill[colStateBill.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                    }
                    dgDetailBill[colAccountPriceBill.Name, rowIndex].Value = dataRow["AccountPrice"];     //核算价格
                    dgDetailBill[colAccountSporadicNumBill.Name, rowIndex].Value = dataRow["AccountLaborNum"];   //核算用工量
                    dgDetailBill[colBearTeamBill.Name, rowIndex].Value = dataRow["BearTeamName"];       //承担队伍
                    dgDetailBill[colInsteadTeamBill.Name, rowIndex].Value = dataRow["InsteadTeamName"];      //被代工单位
                    string b = dataRow["RealOperationDate"].ToString();
                    string[] bArray = b.Split(' ');
                    string strb = bArray[0];
                    dgDetailBill[colComPleteDateBill.Name, rowIndex].Value = strb;    //完成时间
                    string d = dataRow["CreateDate"].ToString();
                    string[] dArray = d.Split(' ');
                    string strd = dArray[0];
                    dgDetailBill[colCreateDateBill.Name, rowIndex].Value = strd;
                    dgDetailBill[colCreatePersonBill.Name, rowIndex].Value = dataRow["CreatePersonName"];
                    string a = dataRow["EndDate"].ToString();
                    string[] aArray = a.Split(' ');
                    string stra = aArray[0];
                    dgDetailBill[colEndDateBill.Name, rowIndex].Value = stra;
                    dgDetailBill[colLaborSubjectBill.Name, rowIndex].Value = dataRow["LaborSubjectName"];        //用工科目
                    dgDetailBill[colPredictSpotadicNumBill.Name, rowIndex].Value = dataRow["PredictLaborNum"];       //计划用工量
                    dgDetailBill[colPriceUnitBill.Name, rowIndex].Value = dataRow["PriceUnitName"];             //价格单位
                    dgDetailBill[colProjectTastDetailBill.Name, rowIndex].Value = dataRow["ProjectTastDetailName"];     //工程任务明细
                    dgDetailBill[colProjectTastTypeBill.Name, rowIndex].Value = dataRow["ProjectTastName"];            //工程任务类型
                    dgDetailBill[colQuantityUnitBill.Name, rowIndex].Value = dataRow["QuantityUnitName"];         //数量单位
                    dgDetailBill[colRealSporadicNumBill.Name, rowIndex].Value = dataRow["RealLaborNum"];        //实际用工量
                    dgDetailBill[colLaborDescriptBill.Name, rowIndex].Value = dataRow["LaborDescript"];       //用工说明
                    dgDetailBill[colSporadicTypeBill.Name, rowIndex].Value = dataRow["LaborState"];         //用工类型
                    if (ClientUtil.ToString(dataRow["balancedtlguid"]) != "")
                    {
                        dgDetailBill[this.colMxIfBal.Name, rowIndex].Value = "是";
                    }
                    else
                    {
                        dgDetailBill[this.colMxIfBal.Name, rowIndex].Value = "否";
                    }
                    string c = dataRow["StartDate"].ToString();
                    string[] cArray = c.Split(' ');
                    string strc = cArray[0];
                    dgDetailBill[colStartDateBill.Name, rowIndex].Value = strc;
                    string strIsCreate = dataRow["IsCreate"].ToString();
                    if (strIsCreate.Equals("0"))
                    {
                        dgDetailBill[colIsCreateBill.Name, rowIndex].Value = "未审核";
                    }
                    if (strIsCreate.Equals("1"))
                    {
                        dgDetailBill[colIsCreateBill.Name, rowIndex].Value = "已审核";
                    }
                    if (ClientUtil.ToDateTime(dataRow["RealOperationDate"]) > ClientUtil.ToDateTime("1900-01-01"))
                    {
                        dgDetailBill[colRealOperationDateBill.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["RealOperationDate"]).ToString();     //制单时间;
                    }
                }
                //lblRecordTotal.Text = "共【" + dataTable.Rows.Count + "】条记录";
                this.dgDetailBill.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
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

        public override bool Print()
        {
            return Preview();
        }

        public override bool Preview()
        {
            LaborSporadicMaster master = dgMaster.CurrentRow.Tag as LaborSporadicMaster;
            if (master == null) return false;
            try
            {
                LaborSporadicBillPrintPreview.Preview(master);
                return true;

            }
            catch (InvalidOperationException e)
            {
                MessageBox.Show(e.Message);
            }
            return false;
        }
    }
}
