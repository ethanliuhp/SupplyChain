using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
//using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatApplyMng.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborSporadicManagement.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.PenaltyDeductionManagement.Domain;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppProcessUI;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng
{
    public partial class VLaborSporadicSelector : TBasicDataView
    {
        MLaborSporadicMng model = new MLaborSporadicMng();
        private LaborSporadicMaster curBillMaster;
        /// <summary>
        /// 当前单据
        /// </summary>
        public LaborSporadicMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VLaborSporadicSelector()
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
        }

        private void InitEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
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
        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (dgDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colSelect.Name)
            {
                dgDetail.EndEdit();
            }
        }

        /// <summary>
        /// 主表变化，明细跟随变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    int rowIndex = dgDetail.Rows.Add();
                    dgDetail.Rows[rowIndex].Tag = dtl;
                    dgDetail[colDetailNumber.Name, rowIndex].Value = dtl.DetailNumber;
                    dgDetail[colProject.Name, rowIndex].Value = dtl.ProjectTastName;//工程任务
                    dgDetail[colAccountPrice.Name, rowIndex].Value = dtl.AccountPrice;//核算单价
                    dgDetail[colAccountMoney.Name, rowIndex].Value = dtl.AccountSumMoney;//核算合价
                    dgDetail[colTeam.Name, rowIndex].Value = dtl.InsteadTeamName;//被代工队伍
                    dgDetail[colTeam.Name, rowIndex].Tag = dtl.InsteadTeam;
                    dgDetail[colAccountQuantity.Name, rowIndex].Value = dtl.AccountLaborNum;//核算用工量
                    dgDetail[colBeginDate.Name, rowIndex].Value = dtl.StartDate.ToShortDateString();
                    dgDetail[colEndDate.Name, rowIndex].Value = dtl.EndDate.ToShortDateString();
                    dgDetail[colLaborSubject.Name, rowIndex].Value = dtl.LaborSubjectName;//用工科目
                    //dgDetail[colPlanLabor.Name, rowIndex].Value = dtl.PredictLaborNum;//计划用工量
                    dgDetail[colPriceUnit.Name, rowIndex].Value = dtl.PriceUnitName;//价格单位
                    dgDetail[this.colDSubInfo.Name, rowIndex].Value = dtl.InsteadTeam.ContractGroupCode;//代工队伍的分包信息
                    dgDetail[colProjectTaskName.Name, rowIndex].Value = dtl.ProjectTastDetailName;//工程任务明细
                    dgDetail[colQuantityUnit.Name, rowIndex].Value = dtl.QuantityUnitName;//数量单位名称
                    dgDetail[colQuantityUnit.Name, rowIndex].Tag = dtl.QuantityUnit;
                    dgDetail[colRealLabor.Name, rowIndex].Value = dtl.RealLaborNum;//实际用工量
                    dgDetail[this.colDescript.Name, rowIndex].Value = dtl.LaborDescript;
                    dgDetail[colPriceUnit.Name, rowIndex].Value = strUnit;
                    dgDetail[colPriceUnit.Name, rowIndex].Tag = Unit;
                    dgDetail[colSelect.Name, rowIndex].Value = true;
                    //dgDetail[colAppOpinion.Name, rowIndex].Value = "审批意见";
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
            if (this.dgMaster.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择信息！");
                return;
            }
            bool ifTip = false;
            bool isExistsNotEqualZero = false;
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) break;
                if (var.Cells[colSelect.Name].Value != null)
                {
                    if ((bool)var.Cells[colSelect.Name].Value)
                    {
                        if (ClientUtil.ToDecimal(var.Cells[colAccountQuantity.Name].Value) == 0 && ifTip == false)
                        {
                            ifTip = true;
                            DialogResult dr = MessageBox.Show("是否核算用工量为0？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            if (dr == DialogResult.No) return;
                        }

                        if (ClientUtil.ToDecimal(var.Cells[colAccountQuantity.Name].Value) > ClientUtil.ToDecimal(var.Cells[colRealLabor.Name].Value))
                        {
                            MessageBox.Show("核算用工量不可大于实际用工量！");
                            return;
                        }

                        if (ClientUtil.ToDecimal(var.Cells[colAccountPrice.Name].Value) == 0)
                        {
                            MessageBox.Show("核算单价不能为空！");
                            return;
                        }

                        curBillMaster.AccountPerson = ConstObject.LoginPersonInfo;//保存审核信息时将审核人一并保存
                        curBillMaster.AccountPersonName = ConstObject.LoginPersonInfo.Name;
                        LaborSporadicDetail curBillDtl = new LaborSporadicDetail();
                        if (var.Tag != null)
                        {
                            curBillDtl = var.Tag as LaborSporadicDetail;
                            if (curBillDtl.Id == null)
                            {
                                curBillMaster.Details.Remove(curBillDtl);
                            }
                        }
                        LaborSporadicDetail dtl = var.Tag as LaborSporadicDetail;
                        dtl.AccountLaborNum = ClientUtil.ToDecimal(var.Cells[colAccountQuantity.Name].Value);//核算用工量
                        dtl.AccountPrice = ClientUtil.ToDecimal(var.Cells[colAccountPrice.Name].Value);//核算单价
                        dtl.AccountSumMoney = ClientUtil.ToDecimal(var.Cells[colAccountMoney.Name].Value);//核算合价
                        dtl.QuantityUnit = var.Cells[colQuantityUnit.Name].Tag as StandardUnit;
                        dtl.QuantityUnitName = ClientUtil.ToString(var.Cells[colQuantityUnit.Name].Value);//工程量计量单位
                        dtl.PriceUnit = var.Cells[colPriceUnit.Name].Tag as StandardUnit;
                        dtl.PriceUnitName = ClientUtil.ToString(var.Cells[colPriceUnit.Name].Value);//价格单位 
                        dtl.InsteadTeamName = ClientUtil.ToString(var.Cells[colTeam.Name].Value);
                        dtl.InsteadTeam = var.Cells[colTeam.Name].Tag as SubContractProject;
                        dtl.LaborDescript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                        curBillMaster.AddDetail(dtl);
                        isExistsNotEqualZero = isExistsNotEqualZero || dtl.AccountSumMoney != 0;
                    }
                }
            }
            curBillMaster = model.LaborSporadicSrv.UpdateLaborSporadic(curBillMaster);
            if (isExistsNotEqualZero)
            {
                MessageBox.Show("保存成功，生成罚扣单成功！");
            }
            else
            {
                MessageBox.Show("保存成功，核算金额为0未生成罚扣单！");
            }

            this.ViewCaption = "代工核算单" + "-" + curBillMaster.Code.ToString();
            LogData log = new LogData();
            log.BillId = curBillMaster.Id;
            log.BillType = "代工核算单";
            log.Code = curBillMaster.Code;
            log.OperType = "核算";
            log.Descript = "";
            log.OperPerson = ConstObject.LoginPersonInfo.Name;
            log.ProjectName = curBillMaster.ProjectName;
            StaticMethod.InsertLogData(log);
            //PenaltyDeduction();

            Query();
            return;
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
                        dgDetail.CurrentCell = dgDetail.Rows[e.RowIndex].Cells[colAccountQuantity.Name];
                        flag = false;
                    }
                    else
                    {
                        if (ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colAccountQuantity.Name].Value) < 0)
                        {
                            MessageBox.Show("核算用工量不能小于零！");
                            dgDetail.CurrentCell = dgDetail.Rows[e.RowIndex].Cells[colAccountQuantity.Name];
                            return;
                        }
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
                        dgDetail.CurrentCell = dgDetail.Rows[e.RowIndex].Cells[colAccountPrice.Name];
                        flag = false;
                    }
                    if (ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colAccountPrice.Name].Value) < 0)
                    {
                        MessageBox.Show("核算单价不能小于零！");
                        dgDetail.CurrentCell = dgDetail.Rows[e.RowIndex].Cells[colAccountPrice.Name];
                        return;
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

        void btnSearch_Click(object sender, EventArgs e)
        {
            Query();
        }

        private void Query()
        {
            ObjectQuery oq = new ObjectQuery();
            IList list = new ArrayList();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.Suspend));
            oq.AddCriterion(Expression.Ge("CreateDate", dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", dtpDateEnd.Value.AddDays(1).Date));
            oq.AddCriterion(Expression.Eq("LaborState", "代工"));
            if (txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Between("Code", txtCodeBegin.Text, txtCodeEnd.Text));
            }
            if (txtCreatePerson.Text != "" && txtCreatePerson.Result.Count > 0)
            {
                oq.AddCriterion(Expression.Eq("CreatePerson", txtCreatePerson.Result[0] as PersonInfo));
            }
            if (txtSupply.Text != "" && txtSupply.Result.Count > 0)
            {
                oq.AddCriterion(Expression.Eq("BearTeamName", txtSupply.Text.ToString()));
            }
            try
            {
                oq.AddFetchMode("BearTeam", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("Details.InsteadTeam", NHibernate.FetchMode.Eager);
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
                dgMaster[colDgMasterCreatePerson.Name, rowIndex].Value = master.CreatePersonName;
                dgMaster[colDgMasterDescript.Name, rowIndex].Value = master.Descript;
                dgMaster[colDgMasterState.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);
                dgMaster[colBearTeam.Name, rowIndex].Value = master.BearTeamName;
                dgMaster[this.colMSubInfo.Name, rowIndex].Value = master.BearTeam.ContractGroupCode;
                dgMaster[colLaborType.Name, rowIndex].Value = master.LaborState;//派工状态
                dgMaster[colAppOpinion.Name, rowIndex].Value = "审批意见";
                curBillMaster = master;
            }
            dgMaster.CurrentCell = dgMaster[1, 0];
            dgMaster_SelectionChanged(dgMaster, new EventArgs());
        }
    }
}
