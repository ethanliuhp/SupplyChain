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
using Application.Business.Erp.SupplyChain.ProductionManagement.PenaltyDeductionManagement.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using System.Collections;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using NHibernate.Criterion;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using IRPServiceModel.Domain.Document;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement
{
    public partial class VPenaltyDeductionQuery : TBasicDataView
    {
        private MPenaltyDeductionMng model = new MPenaltyDeductionMng();

        public VPenaltyDeductionQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }
        private void InitData()
        {
            comMngType.Items.Clear();
            comMngType1.Items.Clear();
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
            this.dtpDateBeginBill.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEndBill.Value = ConstObject.TheLogin.LoginDate;
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            //txtPenaltyRank.SupplierCatCode = CommonUtil.SupplierCatCode3 + "-";
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
            this.btnChoice.Click += new EventHandler(btnChoice_Click);
            this.btnChoiceBill.Click += new EventHandler(btnChoiceBill_Click);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            dgMaster.CellContentClick += new DataGridViewCellEventHandler(dgMaster_CellContentClick);
        }

        //明细显示选择队伍
        void btnChoice_Click(object sender, EventArgs e)
        {
            if (btnChoice.Enabled == true)
            {
                VContractExcuteSelector vmros = new VContractExcuteSelector();
                vmros.ShowDialog();
                IList list = vmros.Result;
                if (list == null || list.Count == 0) return;
                SubContractProject engineerMaster = list[0] as SubContractProject;
                txtPenaltyRank.Text = engineerMaster.BearerOrgName;
                txtPenaltyRank.Tag = engineerMaster;
            }
        }
        void dgMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgMaster[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colCodeBill.Name)
            {
                PenaltyDeductionMaster master = model.PenaltyDeductionSrv.GetPenaltyDeductionByCode(dgvCell.Value.ToString());
                VPenaltyDeductionMng vmro = new VPenaltyDeductionMng();
                vmro.CurBillMaster = master;
                vmro.Preview();
            }
            else if (dgvCell.OwningColumn.Name == colDocument.Name)
            {
                PenaltyDeductionMaster master = dgMaster.Rows[e.RowIndex].Tag as PenaltyDeductionMaster;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", master.Id));
                IList listObj = model.ObjectQuery(typeof(ProObjectRelaDocument), oq);
                if (listObj != null && listObj.Count > 0)
                {
                    oq.Criterions.Clear();
                    Disjunction dis = new Disjunction();
                    foreach (ProObjectRelaDocument obj in listObj)
                    {
                        dis.Add(Expression.Eq("Id", obj.DocumentGUID));
                    }
                    oq.AddCriterion(dis);
                    IList docList = model.ObjectQuery(typeof(DocumentMaster), oq);
                    if (docList != null && docList.Count > 0)
                    {
                        VDocumentsDownloadOrOpen frm = new VDocumentsDownloadOrOpen(docList);
                        frm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("未找到相关文档！");
                    }
                }
                else
                {
                    MessageBox.Show("未找到相关文档！");
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
                PenaltyDeductionMaster master = model.PenaltyDeductionSrv.GetPenaltyDeductionByCode(dgvCell.Value.ToString());
                VPenaltyDeductionMng vmro = new VPenaltyDeductionMng();
                vmro.CurBillMaster = master;
                vmro.Preview();
            }
        }
        //明细EXCEL 按钮事件
        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        //明细查询
        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                FlashScreen.Show("正在查询信息......");
                #region 查询条件处理
                string condition = "";
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                condition += "and t1.ProjectId = '" + projectInfo.Id + "'";
                //单据
                if (txtCodeBegin.Text != "")
                {
                    condition += "and t1.Code like '%" + txtCodeBegin.Text + "%'";
                }
                //制单时间
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
                //罚扣队伍
                if (txtPenaltyRank.Text != "")
                {
                    condition += "and t1.PenaltyDeductionRant = '" + (txtPenaltyRankBill.Tag as SubContractProject).Id + "'";
                }
                //罚扣原因
                //condition += "and t1.PenaltyDeductionReason like '日常检查罚款'";

                if (!txtProjectTaskName.Text.Trim().Equals(""))
                {
                    condition += "and t2.ProjectTask like '%" + txtProjectTaskName.Text + "%'";
                }
                //状态
                if (comMngType1.Text != "")
                {
                    System.Web.UI.WebControls.ListItem li = comMngType1.SelectedItem as System.Web.UI.WebControls.ListItem;
                    int values = ClientUtil.ToInt(li.Value);
                    condition += "and t1.State= '" + values + "'";
                }
                condition += "and t1.PenaltyDeductionReason <> '代工扣款'";
                #endregion
                DataSet dataSet = model.PenaltyDeductionSrv.PenaltyDeductionQuery(condition);
                this.dgDetail.Rows.Clear();

                DataTable dataTable = dataSet.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int rowIndex = this.dgDetail.Rows.Add();
                    dgDetail[colCode.Name, rowIndex].Value = dataRow["code"];
                    object objState = dataRow["State"];
                    if (objState != null)
                    {
                        dgDetail[colStates.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                    }
                    string a = dataRow["BusinessDate"].ToString();
                    string[] aArray = a.Split(' ');
                    string strz = aArray[0];
                    dgDetail[colBusinessDate.Name, rowIndex].Value = strz;
                    dgDetail[colPenaltyDeductionRankName.Name, rowIndex].Value = dataRow["PenaltyDeductionRantName"];
                    dgDetail[colCreatePerson.Name, rowIndex].Value = dataRow["CreatePersonName"];
                    string b = dataRow["BusinessDate"].ToString();
                    string[] bArray = b.Split(' ');
                    string strb = bArray[0];
                    dgDetail[colCreateDate.Name, rowIndex].Value = strb;
                    dgDetail[colPenaltyDeductionReason.Name, rowIndex].Value = dataRow["PenaltyDeductionReason"];
                    dgDetail[colPenaltyMoney.Name, rowIndex].Value = dataRow["PenaltyMoney"];
                    //dgDetail[colPenaltyQuantity.Name,rowIndex].Value = dataRow["PenaltyQuantity"];
                    dgDetail[colPenaltySubject.Name, rowIndex].Value = dataRow["PenaltySubject"];
                    //dgDetail[colProductUnit.Name,rowIndex].Value = dataRow["ProductUnitName"];
                    dgDetail[colProjectTaskDetail.Name, rowIndex].Value = dataRow["TaskDetailName"];
                    dgDetail[colMoneyUnit.Name, rowIndex].Value = dataRow["MoneyUnitName"];
                    dgDetail[colProjectType.Name, rowIndex].Value = dataRow["ProjectTaskName"];
                    dgDetail[colMoneyUnit.Name, rowIndex].Value = dataRow["MoneyUnitName"];
                    dgDetail[colReason.Name, rowIndex].Value = dataRow["Cause"];
                    dgDetail[colDescript.Name, rowIndex].Value = dataRow["Descript"];
                    if (ClientUtil.ToString(dataRow["balancedtlguid"]) != "")
                    {
                        dgDetail[this.colMxIfBal.Name, rowIndex].Value = "是";
                    }
                    else
                    {
                        dgDetail[this.colMxIfBal.Name, rowIndex].Value = "否";
                    }
                    if (ClientUtil.ToDateTime(dataRow["RealOperationDate"]) > ClientUtil.ToDateTime("1900-01-01"))
                    {
                        dgDetail[colRealOperationDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["RealOperationDate"]).ToString();     //制单时间;
                    }
                }

                //this.txtSumQuantity.Text = sumQuantity.ToString("#,###.####");
                //lblRecordTotal.Text = "共【" + dataTable.Rows.Count + "】条记录";
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
        //主从表显示选择队伍
        void btnChoiceBill_Click(object sender, EventArgs e)
        {
            if (btnChoiceBill.Enabled == true)
            {
                VContractExcuteSelector vmros = new VContractExcuteSelector();
                vmros.ShowDialog();
                IList list = vmros.Result;
                if (list == null || list.Count == 0) return;
                SubContractProject engineerMaster = list[0] as SubContractProject;
                txtPenaltyRankBill.Text = engineerMaster.BearerOrgName;
                txtPenaltyRankBill.Tag = engineerMaster;
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
                objectQuery.AddCriterion(Expression.Like("Code", txtCodeBeginBill.Text, MatchMode.Anywhere));
            }
            //创建时间
            objectQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBeginBill.Value.Date));
            objectQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEndBill.Value.AddDays(1).Date));
            //创建人
            if (txtCreatePersonBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("CreatePersonName", txtCreatePersonBill.Text, MatchMode.Anywhere));
            }
            //罚款队伍
            if (txtPenaltyRankBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Eq("PenaltyDeductionRant", txtPenaltyRankBill.Tag as SubContractProject));
            }
            if (comMngType.Text != "")
            {
                System.Web.UI.WebControls.ListItem li = comMngType.SelectedItem as System.Web.UI.WebControls.ListItem;
                int values = ClientUtil.ToInt(li.Value);
                //objectQuery.AddCriterion(Expression.Eq("DocState", (DocumentState)values));
            }
            //else
            //{
            //    objectQuery.AddCriterion(Expression.Not(Expression.Eq("DocState", DocumentState.Edit)));//单据状态为非编辑状态的才能查询出来
            //}
            objectQuery.AddCriterion(Expression.Not(Expression.Eq("PenaltyDeductionReason", "代工扣款")));
            try
            {
                //objectQuery.AddCriterion(Expression.Not(Expression.Eq("DocState", DocumentState.Edit)));
                list = model.PenaltyDeductionSrv.GetPenaltyDeduction(objectQuery);
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
            if (masterList == null || masterList.Count == 0) return;
            decimal sumQuantitys = 0;
            foreach (PenaltyDeductionMaster master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                decimal sumQuantity = 0;
                foreach (PenaltyDeductionDetail dtl in master.Details)
                {
                    object quantity = dtl.PenaltyMoney;
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
                dgMaster[colSummaryMoney.Name, rowIndex].Value = sumQuantity.ToString();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCodeBill.Name, rowIndex].Value = master.Code;
                dgMaster[colPenaltyDeductionRankNameBill.Name, rowIndex].Value = master.PenaltyDeductionRantName;
                dgMaster[colCreatePersonBill.Name, rowIndex].Value = master.CreatePersonName;                         //制单人
                dgMaster[colDescriptBill.Name, rowIndex].Value = master.Descript;                         //备注
                dgMaster[colCreateDateBill.Name, rowIndex].Value = master.CreateDate.ToShortDateString();         //业务日期
                dgMaster[colStateBill.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);       //状态
                if (master.RealOperationDate > ClientUtil.ToDateTime("1900-1-1"))
                {
                    dgMaster[colRealOperationDateBill.Name, rowIndex].Value = master.RealOperationDate.ToShortDateString();     //制单时间;
                }
                dgMaster[colDocument.Name, rowIndex].Value = "查看文档";
            }
            lblRecordTotalBill.Text = "共【" + dgMaster.Rows.Count + "】条记录";
            this.txtSumQuantityBill.Text = sumQuantitys.ToString("#,###.####");
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
            PenaltyDeductionMaster master = dgMaster.CurrentRow.Tag as PenaltyDeductionMaster;
            if (master == null) return;
            //dgDetail.Rows[rowIndex].Tag = dtl;
            foreach (PenaltyDeductionDetail dtl in master.Details)
            {
                int rowIndex = dgDetailBill.Rows.Add();
                dgDetailBill[colPenaltyDeductionReasonBill.Name, rowIndex].Value = master.PenaltyDeductionReason;          //罚款前驱类型
                dgDetailBill[colProjectTypeBill.Name, rowIndex].Value = dtl.ProjectTaskName;                               //工程任务类型
                dgDetailBill[colProjectTaskDetailBill.Name, rowIndex].Value = dtl.TaskDetailName;                          //工程任务明细
                dgDetailBill[colPenaltyMoneyBill.Name, rowIndex].Value = dtl.PenaltyMoney;                                 //罚款金额
                dgDetailBill[colPenaltySubjectBill.Name, rowIndex].Value = dtl.PenaltySubject;                             //用工科目
                dgDetailBill[colBusinessDateBill.Name, rowIndex].Value = dtl.BusinessDate;                                 //业务发生时间
                dgDetailBill[colMoneyUnitBill.Name, rowIndex].Value = dtl.MoneyUnitName;                                   //价格单位
                dgDetailBill[colReasonBill.Name, rowIndex].Value = dtl.Cause;                                              //罚款事由
                if (ClientUtil.ToString(dtl.BalanceDtlGUID) != "")
                {
                    dgDetailBill[this.colIfBal.Name, rowIndex].Value = "是";
                }
                else {
                    dgDetailBill[this.colIfBal.Name, rowIndex].Value = "否";
                }
            }
        }
    }
}
