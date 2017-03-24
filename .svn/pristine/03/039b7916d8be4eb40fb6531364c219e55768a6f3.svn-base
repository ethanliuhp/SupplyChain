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
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborSporadicManagement.Domain;
using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppProcessUI;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.PenaltyDeductionManagement.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng
{
    public partial class VTimeDispatchingSelect : TBasicDataView
    {
        MLaborSporadicMng model = new MLaborSporadicMng();
        private string type = "计时派工";
        private LaborSporadicMaster curBillMaster;
        /// <summary>
        /// 当前单据
        /// </summary>
        public LaborSporadicMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }
        /// <summary>
        /// 登陆人所在项目
        /// </summary>
        CurrentProjectInfo projectInfo;

        public VTimeDispatchingSelect()
        {
            InitializeComponent();
            InitEvent();
            InitForm();
        }

        private void InitForm()
        {
            dtpDateBegin.Value = ConstObject.LoginDate.AddDays(-7);
            dtpDateEnd.Value = ConstObject.LoginDate;
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode3 + "-";
            projectInfo = StaticMethod.GetProjectInfo();
        }

        private void InitEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            dgMaster.CellContentClick += new DataGridViewCellEventHandler(dgMaster_CellContentClick);
        }

        void dgMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (dgMaster[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colAppOpinion.Name)
            {
                LaborSporadicMaster master = dgMaster.Rows[e.RowIndex].Tag as LaborSporadicMaster;
                VAppOpinion vap = new VAppOpinion(master.Id);
                vap.ShowDialog();
            }
        }

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colLaborSubject.Name))
            {
                VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
                frm.ShowDialog();
                CostAccountSubject cost = frm.SelectAccountSubject;
                if (cost != null)
                {
                    this.dgDetail.CurrentRow.Cells[colLaborSubject.Name].Tag = cost;
                    this.dgDetail.CurrentRow.Cells[colLaborSubject.Name].Value = cost.Name;
                }
                this.btnOK.Focus();
            }
            else if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colTeam.Name))
            {
                VContractExcuteSelector vmros = new VContractExcuteSelector();
                vmros.ShowDialog();
                IList list = vmros.Result;
                if (list == null || list.Count == 0) return;
                SubContractProject engineerMaster = list[0] as SubContractProject;
                this.dgDetail.CurrentRow.Cells[e.ColumnIndex].Tag = engineerMaster;
                this.dgDetail.CurrentRow.Cells[e.ColumnIndex].Value = engineerMaster.BearerOrgName;
            }
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (dgDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colSelect.Name)
            {
                dgDetail.EndEdit();
            }
        }

        //主表变化，明细跟随变化
        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            dgDetail.Rows.Clear();
            curBillMaster = dgMaster.CurrentRow.Tag as LaborSporadicMaster;
            if (curBillMaster == null) return;
            string strUnit = "元";
            Application.Resource.MaterialResource.Domain.StandardUnit Unit = null;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Name", strUnit));
            IList lists = model.LaborSporadicSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq);
            if (lists != null && lists.Count > 0)
            {
                Unit = lists[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
            }
            List<LaborSporadicDetail> details = curBillMaster.Details.OfType<LaborSporadicDetail>().ToList<LaborSporadicDetail>();
            var query = from q in details
                        orderby q.DetailNumber ascending
                        select q;
            foreach (LaborSporadicDetail dtl in query)
            {
                if (dtl.IsCreate == 0)
                {
                    dtl.AccountLaborNum = dtl.RealLaborNum;

                    int rowIndex = dgDetail.Rows.Add();
                    dgDetail.Rows[rowIndex].Tag = dtl;
                    dgDetail[colDetailNumber.Name, rowIndex].Value = dtl.DetailNumber;
                    dgDetail[colAccountPrice.Name, rowIndex].Value = dtl.AccountPrice;//核算单价
                    dgDetail[colAccountMoney.Name, rowIndex].Value = dtl.AccountSumMoney;//核算合价
                    dgDetail[colproject.Name, rowIndex].Value = dtl.ProjectTastName;
                    dgDetail[colAccountQuantity.Name, rowIndex].Value = dtl.AccountLaborNum;//核算用工量
                    dgDetail[colBeginDate.Name, rowIndex].Value = dtl.StartDate.ToShortDateString();
                    dgDetail[colEndDate.Name, rowIndex].Value = dtl.EndDate.ToShortDateString();
                    dgDetail[colLaborSubject.Name, rowIndex].Tag = dtl.LaborSubject;
                    dgDetail[colLaborSubject.Name, rowIndex].Value = dtl.LaborSubjectName;//用工科目
                    dgDetail[colPriceUnit.Name, rowIndex].Value = dtl.PriceUnitName;//价格单位
                    dgDetail[colProjectTaskName.Name, rowIndex].Value = dtl.ProjectTastDetailName;//工程任务明细
                    dgDetail[colQuantityUnit.Name, rowIndex].Value = dtl.QuantityUnitName;//数量单位名称
                    dgDetail[colRealLabor.Name, rowIndex].Value = dtl.RealLaborNum;//实际用工量
                    dgDetail[this.colDescript.Name, rowIndex].Value = dtl.LaborDescript;//用工说明
                    dgDetail[colPriceUnit.Name, rowIndex].Value = strUnit;
                    dgDetail[colPriceUnit.Name, rowIndex].Tag = Unit;
                    dgDetail[colPriceUnit.Name, rowIndex].Tag = Unit;
                    dgDetail[colSelect.Name, rowIndex].Value = true;
                }
            }
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.dgDetail.EndEdit();
            bool flag = true;
            bool flags = false;
            if (this.dgMaster.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择信息！");
                return;
            }
            curBillMaster.AccountPerson = ConstObject.LoginPersonInfo;//保存审核信息时将审核人一并保存
            curBillMaster.AccountPersonName = ConstObject.LoginPersonInfo.Name;
            curBillMaster.IsCreate = 1;
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if ((bool)var.Cells[colSelect.Name].EditedFormattedValue)
                {
                    //var teamCell = var.Cells[colTeam.Name];
                    //if (teamCell.Tag == null)
                    //{
                    //    MessageBox.Show("请选择被代工单位！");
                    //    return;
                    //}
                    if (var.Cells[colAccountPrice.Name].Value.ToString() == "0")
                    {
                        MessageBox.Show("请输入被核算单价！");
                        return;
                    }

                    // 如果有一行勾选了，则执行扣款
                    flags = true;
                }
                else
                {
                    // 如果有一行没有勾选，则不修改该单据的执行状态
                    flag = false;
                }
            }
            if (flag)
            {
                curBillMaster.DocState = DocumentState.InExecute;
            }
            if (flags)
            {
               IList lstPenaltyDeductionMaster=CreatePenalty();

               curBillMaster = model.LaborSporadicSrv.SaveOrUpdateLaborSporadic(curBillMaster, lstPenaltyDeductionMaster);
                //this.ViewCaption = "计时派工核算单" + "-" + curBillMaster.Code.ToString();
                LogData log = new LogData();
                log.BillId = curBillMaster.Id;
                log.BillType = "计时派工核算单";
                log.Code = curBillMaster.Code;
                log.OperType = "核算";
                log.Descript = "";
                log.OperPerson = ConstObject.LoginPersonInfo.Name;
                log.ProjectName = curBillMaster.ProjectName;
                StaticMethod.InsertLogData(log);
                MessageBox.Show("保存成功！");
                btnSearch_Click(sender, e);
            }
            else
            {
                MessageBox.Show("没有选中任何信息！");
            }
        }

        /// <summary>
        /// 生成罚款单
        /// </summary>
        private IList  CreatePenalty()
        {
            IList lstMaster = new ArrayList();
            // 计量单位信息
            string strUnit = "项";
            StandardUnit Unit = null;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Name", strUnit));
            IList lists = model.LaborSporadicSrv.GetDomainByCondition(typeof(StandardUnit), oq);
            if (lists != null && lists.Count > 0)
            {
                Unit = lists[0] as StandardUnit;
            }
            string strPriceUnit = "元";
            StandardUnit PriceUnit = null;
            ObjectQuery oq1 = new ObjectQuery();
            oq1.AddCriterion(Expression.Eq("Name", strPriceUnit));
            IList list = model.LaborSporadicSrv.GetDomainByCondition(typeof(StandardUnit), oq1);
            if (list != null && list.Count > 0)
            {
                PriceUnit = list[0] as StandardUnit;
            }
            // 创建罚款单主表
            var penaltyModel = new PenaltyDeductionMaster();
            penaltyModel.CreatePerson = ConstObject.LoginPersonInfo;
            penaltyModel.CreatePersonName = ConstObject.LoginPersonInfo.Name;
            penaltyModel.CreateDate = ConstObject.LoginDate;
            penaltyModel.CreateYear = ConstObject.LoginDate.Year;
            penaltyModel.CreateMonth = ConstObject.LoginDate.Month;
            penaltyModel.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
            penaltyModel.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
            penaltyModel.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
            penaltyModel.HandOrgLevel = ConstObject.TheOperationOrg.Level;
            penaltyModel.HandlePerson = ConstObject.LoginPersonInfo;
            penaltyModel.HandlePersonName = ConstObject.LoginPersonInfo.Name;
            penaltyModel.ProjectId = projectInfo.Id;
            penaltyModel.ProjectName = projectInfo.Name;
            penaltyModel.Descript = "计时派工代工扣款";//备注
            penaltyModel.PenaltyDeductionReason = "代工扣款";
            penaltyModel.PenaltyType = EnumUtil<PenaltyDeductionType>.FromDescription("代工扣款");
            penaltyModel.OEMId = curBillMaster.Id;
            penaltyModel.DocState = DocumentState.InExecute;
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if ((bool)var.Cells[colSelect.Name].Value)
                {
                    // 修改派工单明细的状态
                    LaborSporadicDetail detail = var.Tag as LaborSporadicDetail;
                    detail.AccountLaborNum = ClientUtil.ToDecimal(var.Cells[colAccountQuantity.Name].Value);//核算用工量
                    detail.AccountPrice = ClientUtil.ToDecimal(var.Cells[colAccountPrice.Name].Value);//核算单价
                    detail.AccountSumMoney = ClientUtil.ToDecimal(var.Cells[colAccountMoney.Name].Value);//核算合价
                    detail.PriceUnit = var.Cells[colPriceUnit.Name].Tag as StandardUnit;
                    detail.PriceUnitName = ClientUtil.ToString(var.Cells[colPriceUnit.Name].Value);//价格单位 
                    detail.LaborSubject = var.Cells[colLaborSubject.Name].Tag as CostAccountSubject;
                    detail.LaborSubjectName = ClientUtil.ToString(var.Cells[colLaborSubject.Name].Value);
                    detail.LaborDescript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                    detail.IsCreate = 1;
                    detail.InsteadTeam = var.Cells[colTeam.Name].Tag as SubContractProject;
                    if (detail.InsteadTeam == null) continue;   // 如果没有选择被代工队伍，则不执行罚款
                    detail.InsteadTeamName = var.Cells[colTeam.Name].Value.ToString();
                    // 创建罚款主表
                    var penaltyMaster = penaltyModel.Clone() as PenaltyDeductionMaster;
                    penaltyMaster.Details = new Iesi.Collections.Generic.HashedSet<BaseDetail>();
                    penaltyMaster.PenaltyDeductionRant = detail.InsteadTeam;
                    penaltyMaster.PenaltyDeductionRantName = detail.InsteadTeamName;
                    // 根据明细生成罚款单明细
                    var penaltyDetail = new PenaltyDeductionDetail();
                    penaltyDetail.BusinessDate = detail.StartDate;

                    penaltyDetail.PenaltyQuantity = detail.AccountLaborNum;
                    penaltyDetail.PenaltyMoney = detail.AccountSumMoney;
                    penaltyDetail.AccountPrice = detail.AccountPrice;
                    penaltyDetail.AccountQuantity = detail.AccountLaborNum;
                    penaltyDetail.AccountMoney = detail.AccountSumMoney;
                    penaltyDetail.Cause = detail.LaborDescript;
                    penaltyDetail.LaborDetailGUID = detail;

                    penaltyDetail.PenaltySubjectGUID = detail.LaborSubject;
                    penaltyDetail.PenaltySubject = detail.LaborSubjectName;
                    penaltyDetail.PenaltySysCode = detail.LaborSubjectSysCode;
                    penaltyDetail.ProjectTaskSyscode = detail.ProjectTaskSyscode;
                    penaltyDetail.ResourceSysCode = detail.ResourceSysCode;
                    penaltyDetail.ResourceType = detail.ResourceType;
                    penaltyDetail.ResourceTypeName = detail.ResourceTypeName;
                    penaltyDetail.ResourceTypeSpec = detail.ResourceTypeSpec;
                    penaltyDetail.ResourceTypeStuff = detail.ResourceTypeStuff;
                    penaltyDetail.ProjectTaskDetail = detail.ProjectTastDetail;
                    penaltyDetail.TaskDetailName = detail.ProjectTastDetailName;
                    penaltyDetail.PenaltyType = "文明施工";
                    penaltyDetail.ProjectTask = detail.ProjectTast;
                    penaltyDetail.ProjectTaskName = detail.ProjectTastName;
                    //penaltyDetail.Cause = "计时派工代工扣款";
                    penaltyDetail.ProductUnit = detail.QuantityUnit;
                    penaltyDetail.ProductUnitName = detail.QuantityUnitName;
                    penaltyDetail.MoneyUnit = detail.PriceUnit;
                    penaltyDetail.MoneyUnitName = detail.PriceUnitName;
                    penaltyDetail.Master = penaltyMaster;
                    penaltyMaster.AddDetail(penaltyDetail);
                    // 保存罚款单
                    //model.LaborSporadicSrv.SaveOrUpdateByDao(penaltyMaster);
                    // 记录罚款单id
                    //curBillMaster.PenaltyDeductionMaster += penaltyMaster.Id + "|";
                    lstMaster.Add(penaltyMaster);
                }
            }
            return lstMaster;
        }

        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            bool flag = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            if (colName == colAccountPrice.Name || colName == colAccountQuantity.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colAccountQuantity.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colAccountQuantity.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("核算用工量为数字！");
                        this.dgDetail.Rows[e.RowIndex].Cells[colAccountQuantity.Name].Value = "";
                        //dgDetail.CurrentCell = dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name];
                        flag = false;
                    }
                }

                if (dgDetail.Rows[e.RowIndex].Cells[colAccountPrice.Name].Value != null)
                {
                    string temp_price = dgDetail.Rows[e.RowIndex].Cells[colAccountPrice.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_price);
                    if (validity == false)
                    {
                        MessageBox.Show("核算单价为数字！");
                        this.dgDetail.Rows[e.RowIndex].Cells[colAccountPrice.Name].Value = "";
                        //dgDetail.CurrentCell = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name];
                        flag = false;
                    }
                }
                if (flag)
                {
                    //根据单价和数量计算金额  
                    decimal money = 0;
                    object price = dgDetail.Rows[e.RowIndex].Cells[colAccountPrice.Name].Value;
                    object quantity = dgDetail.Rows[e.RowIndex].Cells[colAccountQuantity.Name].Value;
                    for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                    {
                        quantity = dgDetail.Rows[i].Cells[colAccountQuantity.Name].Value;
                        if (quantity == null) quantity = 0;

                        if (ClientUtil.ToString(dgDetail.Rows[i].Cells[colAccountQuantity.Name].Value) != "" && ClientUtil.ToString(dgDetail.Rows[i].Cells[colAccountPrice.Name].Value) != "")
                        {
                            money = ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colAccountQuantity.Name].Value) * ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colAccountPrice.Name].Value);
                            dgDetail.Rows[i].Cells[colAccountMoney.Name].Value = ClientUtil.ToString(money);
                        }
                    }
                }

            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearch_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            IList list = new ArrayList();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Ge("CreateDate", dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", dtpDateEnd.Value.AddDays(1).Date));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.Suspend));
            if (txtCreatePerson.Text != "" && txtCreatePerson.Result.Count > 0)
            {
                oq.AddCriterion(Expression.Eq("CreatePerson", txtCreatePerson.Result[0] as PersonInfo));
            }
            if (txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Between("Code", txtCodeBegin.Text, txtCodeEnd.Text));
            }
            if (txtSupply.Text != "" && txtSupply.Result.Count > 0)
            {
                oq.AddCriterion(Expression.Eq("BearTeamName", txtSupply.Text.ToString()));
            }
            oq.AddCriterion(Expression.Eq("LaborState", type));
            try
            {
                oq.AddFetchMode("BearTeam", NHibernate.FetchMode.Eager);
                list = model.LaborSporadicSrv.GetLaborSporadic(oq);
                ShowMasterList(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
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
                dgMaster[colDgMasterCode.Name, rowIndex].Value = master.Code;
                string[] sArray = master.CreateDate.ToString().Split(' ');
                string stra = sArray[0];
                dgMaster[colDgMasterCreateDate.Name, rowIndex].Value = stra;
                if (master.RealOperationDate > ClientUtil.ToDateTime("1900-1-1"))
                {
                    dgMaster[colRealOperationDate.Name, rowIndex].Value = master.RealOperationDate.ToString();     //制单时间;
                }
                dgMaster[this.colMSubInfo.Name, rowIndex].Value = master.BearTeam.ContractGroupCode;
                dgMaster[colDgMasterCreatePerson.Name, rowIndex].Value = master.CreatePersonName;
                dgMaster[colDgMasterDescript.Name, rowIndex].Value = master.Descript;
                dgMaster[colDgMasterState.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);
                dgMaster[colBearTeam.Name, rowIndex].Value = master.BearTeamName;
                dgMaster[colLaborType.Name, rowIndex].Value = master.LaborState;//派工状态
                dgMaster[colAppOpinion.Name, rowIndex].Value = "审批意见";
                curBillMaster = master;
            }
            dgMaster.CurrentCell = dgMaster[1, 0];
            dgMaster_SelectionChanged(dgMaster, new EventArgs());
        }
    }
}
